using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DesktopApp.Models;

[Index(nameof(FileUniqueId), IsUnique = true)]
public class Song
{
    [NotMapped]
    private static readonly char delimiter = ';';

    [Key]
    public int Key { get; set; }

    [Required]
    public string FileId { get; set; }

    [Required]
    public string FileUniqueId { get; set; }

    [Required]
    public string FileName { get; set; }

    [Required]
    public int Duration { get; set; }

    [Required]
    public DateTime AddedDateTime { get; set; }

    public string? Title { get; set; }

    public string? Artist { get; set; }

    public string? Album { get; set; }

    public int? Year { get; set; }

    public int? Rating { get; set; }

    public string? Performers { get; set; }

    public string? Genres { get; set; }

    public string? Tags { get; set; }

    public string[]? GetPerformers()
    {
        return Performers?.Split(delimiter);
    }

    public void SetPerformers(string[]? performers)
    {
        Performers = string.Join($"{delimiter}", performers);
    }

    public string[]? GetGenres()
    {
        return Genres?.Split(delimiter);
    }

    public void SetGenres(string[]? genres)
    {
        Genres = string.Join($"{delimiter}", genres);
    }

    public string[]? GetTags()
    {
        return Tags?.Split(delimiter);
    }

    public void SetTags(string[]? tags)
    {
        Tags = string.Join($"{delimiter}", tags);
    }

    public Song(string fileId, string fileUniqueId, string fileName, int duration, DateTime addedDateTime, string? title, string? artist, string? album, int? year, int? rating, string? performers, string? genres, string? tags)
    {
        FileId = fileId;
        FileUniqueId = fileUniqueId;
        FileName = fileName;
        Duration = duration;
        AddedDateTime = addedDateTime;
        Title = title;
        Artist = artist;
        Album = album;
        Year = year;
        Rating = rating;
        Performers = performers;
        Genres = genres;
        Tags = tags;
    }

    public Song(string fileId, string fileUniqueId, string fileName, int duration, DateTime addedDateTime, string? title, string? artist, string? album, int? year, int? rating, string[]? performers, string[]? genres, string[]? tags)
    {
        FileId = fileId;
        FileUniqueId = fileUniqueId;
        FileName = fileName;
        Duration = duration;
        AddedDateTime = addedDateTime;
        Title = title;
        Artist = artist;
        Album = album;
        Year = year;
        Rating = rating;
        SetPerformers(performers);
        SetGenres(genres);
        SetTags(tags);
    }
}