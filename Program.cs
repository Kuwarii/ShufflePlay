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
            int counter = 100;
            Random rnd = new Random(Convert.ToInt32(ShufflePlayList.ConvertToUnixTimestamp(DateTime.Now)));

            // Create 100 songs...
            Console.WriteLine("*\n* Creating 100 Songs\n*");
            List<string> songs = new List<string>();

            for (int i = 1; i <= counter; i++)
            {
                songs.Add(string.Format("Song {0}", i));
            }

            ShufflePlayList shufflePlayList = new ShufflePlayList(songs);

            Console.WriteLine("*\n* P=Play ; A=Add Song ; S=Stop ; E=Exit\n*");

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
                }

            }


            //// Pause before adding new songs...
            //Thread.Sleep(3000);

            //Console.WriteLine("*\n* Adding 20 New Songs\n*");

            //for (int i = 0; i < 20; i++)
            //{
            //    shufflePlayList.AddTrack(string.Format("*NEW* Song {0}", i + 101));
            //    Thread.Sleep(100);
            //}



            //Console.WriteLine("*\n* Please press ENTER to continue!\n*");
            //Console.ReadKey();

            //var r2 = shufflePlayList.ShufflePlayAsync();

            //Console.WriteLine("*\n* MAIN THREAD DONE!!!\n*\n* Please press ENTER to exit!\n*");
            //Console.ReadKey();
        }
    }
}
