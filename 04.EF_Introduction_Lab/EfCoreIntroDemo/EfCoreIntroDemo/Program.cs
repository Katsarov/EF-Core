using EfCoreIntroDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EfCoreIntroDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //var db = new MusicXContext();

            //var songs = db.Songs
            //    .Where(x => x.SongArtists.Count() > 5)
            //    .Select(x => new
            //    {
            //        x.Name,
            //        Artist = string.Join(", ", x.SongArtists
            //        .Select(a => a.Artist.Name))
            //    });

            //foreach (var item in songs)
            //{
            //    Console.WriteLine(item.Artist + " --> " + item.Name);
            //}

            //-------------------------------------------------------------------------
            //First()
            //var songs = db.Songs.Where(x => x.Name.StartsWith("N"));

            //var song = songs.First();
            //Console.WriteLine(song.Name);


            //-------------------------------------------------------------------------
            //FirstOrDefault()
            //var songs = db.Songs.Where(x => x.Name.StartsWith("Nikolay"));

            //var song = songs.FirstOrDefault();
            //Console.WriteLine(song.Name);



            //-------------------------------------------------------------------------
            //FirstOrDefault()
            //var songs = db.Songs.Select(x => new
            //{

            //    x.Name,
            //    ArtistsCount = x.SongArtists.Count(),
            //    FirstArtist = x.SongArtists.FirstOrDefault().Artist.Name,
            //    SourceName = x.Source.Name,
            //    AnySArtist = x.SongArtists.Any(x => x.Artist.Name.StartsWith("С"))

            //}).OrderByDescending(x => x.FirstArtist)
            //.ThenByDescending(x => x.Name)
            //.Take(10);
            //.ToList();


            //Console.WriteLine(songs.ToQueryString());

            //foreach (var song in songs)
            //{
            //    Console.WriteLine(song);
            //}


            //-------------------------------------------------------------------------

            var db = new MusicXContext();

            //var artist = new Artist
            //{
            //    CreatedOn = DateTime.UtcNow,
            //    Name = "Nakov",
            //    MoneyEarned = 120.12M
            //};

            ////INSERT (Вкарваме нов изпълнител, с нова песен)
            //artist.SongArtists.Add(new SongArtist
            //{
            //    Song = new Song
            //    {
            //        Name = "SoftUni",
            //        CreatedOn = DateTime.UtcNow
            //    },
            //});
            //artist.SongArtists.Add(new SongArtist
            //{
            //    Song = new Song
            //    {
            //        Name = "Rakiq",
            //        CreatedOn = DateTime.UtcNow
            //    },
            //});

            //db.Artists.Add(artist);
            //db.SaveChanges();


            //--------------------------------------------------------------------------------------------
            ////UPDATE
            //var song = db.Songs.OrderByDescending(x => x.Id)
            //    .FirstOrDefault();
            //song.CreatedOn = DateTime.UtcNow;
            //db.SaveChanges();


            //--------------------------------------------------------------------------------------------
            ////DELETE

            //var newSong = new Song
            //{
            //    Name = "Tra-La-La"
            //};
            //db.Songs.Add(newSong);
            //db.SaveChanges();

            //var song = db.Songs.OrderByDescending(x => x.Id)
            //    .FirstOrDefault();
            //song.CreatedOn = DateTime.UtcNow;
            //db.SaveChanges();



            ////DELETE Variant 1
            //var song = db.Songs.OrderByDescending(x => x.Id)
            //        .FirstOrDefault();
            //db.Songs.Remove(song);
            //db.SaveChanges();

            ////DELETE Variant 2
            var song = new Song { Id = 52009 };
            db.Songs.Remove(song);
            db.SaveChanges();

        }
    }
}
