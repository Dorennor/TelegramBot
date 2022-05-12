using DesktopApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopApp.Models.Data;

public class SongsRepository : ISongsRepository
{
    private readonly TgBotDbContext _context;

    public SongsRepository(TgBotDbContext context)
    {
        _context = context;
        _context.Songs.Load();
        Count = _context.Songs.Count();
    }

    public static int Count { get; private set; }

    public async Task<List<Song>> GetAllAsync()
    {
        return await _context.Songs.ToListAsync();
    }

    public async Task<Song?> GetByKeyAsync(int key)
    {
        var song = await _context.Songs.FirstOrDefaultAsync(s => s.Key == key);
        return song;
    }

    public async Task<Song?> FindByHashCodeAsync(string hashCode)
    {
        var song = await _context.Songs.FirstOrDefaultAsync(s => s.HashCode == hashCode);
        return song;
    }

    public async Task<List<Song>> FindAllByFileUniqueIdAsync(string fileUniqueId)
    {
        return await _context.Songs.Where(s => s.FileUniqueId == fileUniqueId).ToListAsync();
    }

    public async Task<List<Song>> FindAllByTitleAsync(string title)
    {
        return await _context.Songs.Where(s => s.Title == title).ToListAsync();
    }

    public async Task<List<Song>> FindAllByFileNameAsync(string fileName)
    {
        return await _context.Songs.Where(s => s.FileName == fileName).ToListAsync();
    }

    public async Task<List<Song>> FindAllByDurationAsync(int duration)
    {
        return await _context.Songs.Where(s => s.Duration == duration).ToListAsync();
    }

    public async Task<List<Song>> FindAllByAddedDateTimeAsync(DateTime addedDateTime)
    {
        return await _context.Songs.Where(s => s.AddedDateTime == addedDateTime).ToListAsync();
    }

    public async Task<List<Song>> FindAllByArtistAsync(string artist)
    {
        return await _context.Songs.Where(s => s.Artist == artist).ToListAsync();
    }

    public async Task<List<Song>> FindAllByAlbumAsync(string album)
    {
        return await _context.Songs.Where(s => s.Album == album).ToListAsync();
    }

    public async Task<List<Song>> FindAllByYearAsync(int year)
    {
        return await _context.Songs.Where(s => s.Year == year).ToListAsync();
    }

    public async Task<List<Song>> FindAllByRatingAsync(int rating)
    {
        return await _context.Songs.Where(s => s.Rating == rating).ToListAsync();
    }

    //TODO
    public Task<List<Song>>? FindAllByPerformersAsync(string[] performers)
    {
        return null;
    }

    //TODO
    public Task<List<Song>>? FindAllByGenresAsync(string[] genres)
    {
        return null;
    }

    //TODO
    public Task<List<Song>>? FindAllByTagsAsync(string[] tags)
    {
        return null;
    }

    public async Task AddSongAsync(Song song)
    {
        await _context.Songs.AddAsync(song);
        await _context.SaveChangesAsync();
        Count = _context.Songs.Count();
    }
}