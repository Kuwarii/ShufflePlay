using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShufflePlay
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 30;
            Random rnd = new Random(Convert.ToInt32(ShufflePlayList.ConvertToUnixTimestamp(DateTime.Now)));

            // Create 100 songs...
            Console.WriteLine("*\n* Creating {0} Songs\n*", counter);
            List<string> tracks = new List<string>();

            for (int i = 1; i <= counter; i++)
            {
                tracks.Add(string.Format("Song {0}", i));
            }

            ShufflePlayList shufflePlayList = new ShufflePlayList(tracks);

            Console.WriteLine("*\n* P=Play ; A=Add Song ; S=Stop ; R=Reset Songs ; C=Commit Playlist ; E=Exit\n*");

            bool running = true;
            while (running)
            {
                ConsoleKeyInfo keypress = Console.ReadKey();
                string key = keypress.Key.ToString().ToUpper();

                switch(key)
                {
                    case "E":
                        running = false;
                        break;

                    case "P":
                        Console.WriteLine("\n*\n* PLAYING\n*");
                        Task result = shufflePlayList.ShufflePlayAsync();
                        break;

                    case "S":
                        Console.WriteLine("\n*\n* STOPPING\n*");
                        shufflePlayList.StopPlay();
                        break;

                    case "A":
                        Console.WriteLine("\n*\n* ADDING SONG\n*");
                        shufflePlayList.AddTrack(string.Format("*NEW* Song {0}", counter++));
                        break;

                    case "R":
                        Console.WriteLine("\n*\n* RESETTING\n*");
                        shufflePlayList.ResetTracks(tracks);
                        break;

                    case "C":
                        Console.WriteLine("\n*\n* COMMITTING\n*");
                        tracks = shufflePlayList.OriginalSongsTracks.ToList();
                        break;

                    case "L":
                        Console.WriteLine("\n*\n* LISTING\n*");
                        int tc = 0;

                        foreach (string track in shufflePlayList.OriginalSongsTracks)
                        {
                            Console.WriteLine("{0:000}. {1}", ++tc, track);
                        }
                        break;
                }

            }
        }
    }
}
