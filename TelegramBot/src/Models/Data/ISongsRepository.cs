using DesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesktopApp.Models.Data;

public interface ISongsRepository
{
    static int Count { get; set; }

    Task<List<Song>> GetAllAsync();

    Task<Song?> GetByKeyAsync(int key);

    Task<Song?> FindByHashCodeAsync(string hashCode);

    Task<List<Song>> FindAllByFileUniqueIdAsync(string fileUniqueId);

    Task<List<Song>> FindAllByTitleAsync(string title);

    Task<List<Song>> FindAllByFileNameAsync(string fileName);

    Task<List<Song>> FindAllByDurationAsync(int duration);

    Task<List<Song>> FindAllByAddedDateTimeAsync(DateTime addedDateTime);

    Task<List<Song>> FindAllByArtistAsync(string artist);

    Task<List<Song>> FindAllByAlbumAsync(string album);

    Task<List<Song>> FindAllByYearAsync(int year);

    Task<List<Song>> FindAllByRatingAsync(int rating);

    Task<List<Song>>? FindAllByPerformersAsync(string[] performers);

    Task<List<Song>>? FindAllByGenresAsync(string[] genres);

    Task<List<Song>>? FindAllByTagsAsync(string[] tags);

    Task AddSongAsync(Song song);
}