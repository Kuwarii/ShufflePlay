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
            Random rnd = new Random(Convert.ToInt32(ShufflePlayList.ConvertToUnixTimestamp(DateTime.Now)));

            // Create 100 songs...
            Console.WriteLine("*\n* Creating 100 Songs\n*");
            List<string> songs = new List<string>();

            for (int i = 0; i < 100; i++)
            {
                songs.Add(string.Format("Song {0}", i + 1));
            }

            ShufflePlayList shufflePlayList = new ShufflePlayList(songs);
            var result = shufflePlayList.ShufflePlayAsync();


            // Pause before adding new songs...
            Thread.Sleep(3000);

            Console.WriteLine("*\n* Adding 20 New Songs\n*");

            for (int i = 0; i < 20; i++)
            {
                shufflePlayList.AddTrack(string.Format("*NEW* Song {0}", i + 101));
                Thread.Sleep(100);
            }

            Console.WriteLine("*\n* MAIN THREAD DONE!!!\n*\n* Please press ENTER to exit!\n*");
            Console.ReadKey();
        }
    }
}
