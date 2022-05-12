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

namespace DesktopApp.Models.Services;

public class SongsService
{
    private readonly BotService _botController;
    private readonly TgBotDbContext _context;

    public SongsService(TgBotDbContext context)
    {
        _context = context;

        _context.Songs.Load();
    }

    public async Task<bool> AddToDbIfDoesNotExist(Audio song)
    {
        //if (_context.Songs.FirstOrDefault(s => s.FileUniqueId == song.FileUniqueId) != null) return false;

        //var file = _botController.BotClient.GetFileAsync(song.FileId);
        //var fileName = song.FileName;

        //CreatePathIfNotExist();

        //var filePath = Directory.GetCurrentDirectory() + @"\Downloads\Music\";

        //await using (var saveAudioStream = new FileStream(filePath + fileName, FileMode.Create, FileAccess.Write))
        //{
        //    await _botController.BotClient.DownloadFileAsync(file.Result.FilePath!, saveAudioStream);
        //}

        //var songHashCode = GenerateHashCode(filePath, fileName).Result;

        //if (_context.Songs.FirstOrDefault(s => s.HashCode == songHashCode) != null) return false;

        //var songTags = TagLib.File.Create(filePath + fileName).Tag;

        //await _context.Songs.AddAsync(new Song(songHashCode, song.FileId, song.FileUniqueId, song.FileName, song.Duration, DateTime.Now, song.Title, songTags.FirstAlbumArtist, songTags.Album, songTags.Year, null, songTags.Performers, songTags.Genres, null));
        //await _context.SaveChangesAsync();

        //await DeleteAllFiles();
        return true;
    }

    private Task DeleteAllFiles()
    {
        var di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Downloads\Music\");

        foreach (var file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (var dir in di.GetDirectories())
        {
            dir.Delete(true);
        }

        return Task.CompletedTask;
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
}