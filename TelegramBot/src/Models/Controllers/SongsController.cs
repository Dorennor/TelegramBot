using DesktopApp.Models.Data;
using DesktopApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DesktopApp.Models.Controllers;

public class SongsController : IDisposable
{
    private readonly BotController _botController;
    private readonly TgBotDbContext _context;

    public SongsController()
    {
        _botController = new BotController();
        _context = new TgBotDbContext();
        _context.Songs.Load();
    }

    private async Task<string> GenerateHashCode(string filePath, string fileName)
    {
        using (var sha512 = SHA512.Create())
        {
            using (var openAudioStream = new FileStream(filePath + fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] hashCode = await sha512.ComputeHashAsync(openAudioStream);
                return BitConverter.ToString(hashCode);
            }
        }
    }

    private static void CreatePathIfNotExist()
    {
        if (Directory.Exists(@"\Downloads\Music\")) return;

        var filePath = Directory.GetCurrentDirectory();

        Directory.CreateDirectory("Downloads");
        Directory.SetCurrentDirectory("Downloads");
        Directory.CreateDirectory("Music");
        Directory.SetCurrentDirectory(filePath);
    }

    public async Task<bool> AddToDbIfDoesNotExist(Audio song)
    {
        if (_context.Songs.FirstOrDefault(s => s.FileUniqueId == song.FileUniqueId) != null) return false;

        var file = _botController.BotClient.GetFileAsync(song.FileId);
        var fileName = song.FileName;

        CreatePathIfNotExist();

        var filePath = Directory.GetCurrentDirectory() + @"\Downloads\Music\";

        using (var saveAudioStream = new FileStream(filePath + fileName, FileMode.Create, FileAccess.Write))
        {
            await _botController.BotClient.DownloadFileAsync(file.Result.FilePath!, saveAudioStream);
        }

        var songHashCode = GenerateHashCode(filePath, fileName).Result;

        if (_context.Songs.FirstOrDefault(s => s.HashCode == songHashCode) != null) return false;

        var songTags = TagLib.File.Create(filePath + fileName).Tag;

        await _context.Songs.AddAsync(new Song(song.FileId, song.FileUniqueId, song.FileName, song.Duration, DateTime.Now, song.Title, songTags.FirstAlbumArtist, songTags.Album, songTags.Year, null, songTags.Performers, songTags.Genres, null, songHashCode));
        await _context.SaveChangesAsync();
        return true;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}