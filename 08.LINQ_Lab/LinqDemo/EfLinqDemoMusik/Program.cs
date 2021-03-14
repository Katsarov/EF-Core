using EfLinqDemoMusik.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EfLinqDemoMusik
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new MusicXContext();
            var songs1 = db.Songs
                .Where(x => x.SongArtists.Count() == 1)
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    Artist1 = x.SongArtists.FirstOrDefault().Artist.Name
                })
                .Skip(100)
                .Take(10)
                .ToList();

            foreach (var song in songs1)
            {
                Console.WriteLine(song.Artist1 + " " + song.Name);
            }





            return;

            // ---------------------- SelectMany -----------------------------------
            var artistsSongs = db.Artists
                .OrderBy(x => x.Name)
                .SelectMany(x => x.SongArtists
                    .Select(sa => x.Name + " - " + sa.Song.Name))
                .ToList();

                foreach (var song in artistsSongs)
                {
                    Console.WriteLine(song);
                }
            

            return;


            // --------------------- GroupBy ------------------------------
            var groups = db.Artists
                .GroupBy(x => x.Name.Substring(0, 1))
                .Select(x => new
                {
                    FirstLetter = x.Key,
                    Count = x.Count()
                })
                .ToList();

            foreach (var group in groups)
            {
                Console.WriteLine($"{group.FirstLetter} {group.Count}");
            }

            return;



            // ----------------- OrderBy --------------------------------
            var artists = db.Artists
                .OrderByDescending(x => x.SongArtists.Count())
                .Select(x => new 
                    { 
                        x.Name, 
                        Count = x.SongArtists.Count()
                    })
                .Take(10)
                .ToList();

            foreach (var artist in artists)
            {
                Console.WriteLine($"{artist.Name} -> {artist.Count}");
                //Console.WriteLine(artist);
            }


            return;


            // ----------------- Where -----------------------------
            var songs = db.Songs
                .Where(x => x.Source.Name == "User")
                .ToList();

            int i = 1;
            foreach (var song in songs)
            {
                Console.WriteLine($" {i} {song.Name}");
                i++;
                
            }

        }
    }
}
