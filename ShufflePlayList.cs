using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShufflePlay
{
    public class ShufflePlayList
    {
        /// <summary>
        /// Gets or sets the tracks.
        /// </summary>
        /// <value>
        /// The tracks.
        /// </value>
        protected List<string> Tracks { get; set; }

        /// <summary>
        /// Gets or sets the shuffle history.
        /// </summary>
        /// <value>
        /// The shuffle history.
        /// </value>
        protected Dictionary<string, string> ShuffleHistory { get; set; }

        /// <summary>
        /// Songs playing flag.
        /// </summary>
        private bool _playing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShufflePlayList"/> class.
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        public ShufflePlayList(IEnumerable<string> tracks)
        {
            this.Tracks = tracks.ToList();
        }

        /// <summary>
        /// Adds the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public void AddTrack(string track)
        {
            this.Tracks.Add(track);
        }

        /// <summary>
        /// Shuffles the play asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task ShufflePlayAsync()
        {
            // Exit if already playing...
            if (_playing)
            {
                return;
            }

            // Incase of a suffle replay...
            if (this.Tracks.Count == 0 && this.ShuffleHistory.Values.Count > 0)
            {
                this.Tracks.AddRange(this.ShuffleHistory.Values.OrderBy(x => x));
            }

            // Run code on another thread...
            await Task.Run(() =>
            {
                _playing = true;
                double unixTime = ConvertToUnixTimestamp(DateTime.Now.ToUniversalTime());
                int seed = Convert.ToInt32(unixTime);

                Random rnd = new Random(seed);

                this.ShuffleHistory = new Dictionary<string, string>();
                int count = 0;

                while (this.Tracks.Count > 0 && _playing)
                {
                    count++;
                    int track = rnd.Next(0, this.Tracks.Count);

                    if (!this.ShuffleHistory.ContainsKey(this.Tracks[track]))
                    {
                        // Song has not been played before...
                        this.ShuffleHistory.Add(this.Tracks[track], this.Tracks[track]);

                        // Play song at this point...
                        Console.WriteLine("{1:000}. Playing {0}", this.Tracks[track], count);
                        Thread.Sleep(200);
                    }

                    this.Tracks.RemoveAt(track);
                }
            });

            Console.WriteLine("*\n* PLAY THREAD DONE!!!\n*");

            return;
        }

        /// <summary>
        /// Stops the play.
        /// </summary>
        public void StopPlay()
        {
            _playing = false;
        }


        /// <summary>
        /// Converts to unix timestamp.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;

            return Math.Floor(diff.TotalSeconds);
        }
    }
}
