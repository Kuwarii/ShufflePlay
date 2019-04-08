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
        /// Gets or sets the playing tracks.
        /// </summary>
        /// <value>
        /// The playing tracks.
        /// </value>
        protected List<string> PlayingTracks { get; set; }

        /// <summary>
        /// Gets or sets the original songs.
        /// </summary>
        /// <value>
        /// The original songs.
        /// </value>
        protected List<string> OriginalTracks { get; set; }

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
        /// Gets or sets a value indicating whether this <see cref="ShufflePlayList"/> is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if playing; otherwise, <c>false</c>.
        /// </value>
        public bool Playing
        {
            get
            {
                return _playing;
            }
            protected set
            {
                _playing = value;
            }
        }

        /// <summary>
        /// The track counter
        /// </summary>
        private int _trackCounter = 0;

        /// <summary>
        /// Gets or sets the track counter.
        /// </summary>
        /// <value>
        /// The track counter.
        /// </value>
        public int CurrentTrackNumber
        {
            get
            {
                return _trackCounter;
            }
        }

        /// <summary>
        /// Converts to taltracks.
        /// </summary>
        /// <value>
        /// The total tracks.
        /// </value>
        public int TotalTracks
        {
            get
            {
                return this.OriginalTracks.Count;
            }
        }


        /// <summary>
        /// Gets the songs tracks.
        /// </summary>
        /// <value>
        /// The songs tracks.
        /// </value>
        public string[] OriginalSongsTracks
        {
            get
            {
                return this.OriginalTracks.ToArray();
            }
        }

        /// <summary>
        /// Gets the shuffle history tracks.
        /// </summary>
        /// <value>
        /// The shuffle history tracks.
        /// </value>
        public string[] ShuffleHistoryTracks
        {
            get
            {
                return this.ShuffleHistory.Values.ToArray();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShufflePlayList"/> class.
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        public ShufflePlayList(IEnumerable<string> tracks)
        {
            this.OriginalTracks = tracks.ToList();
            this.PlayingTracks = new List<string>();
            this.ShuffleHistory = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public void AddTrack(string track)
        {
            this.OriginalTracks.Add(track);

            if (this.Playing)
            {
                this.PlayingTracks.Add(track);
            }
        }

        /// <summary>
        /// Resets the tracks.
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        public void ResetTracks(IEnumerable<string> tracks)
        {
            this.OriginalTracks = tracks.ToList();
        }

        /// <summary>
        /// Shuffles the play asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task ShufflePlayAsync()
        {
            // Exit if already playing...
            if (this.Playing)
            {
                return;
            }

            // Run code on another thread...
            await Task.Run(() =>
            {
                this.Playing = true;

                double unixTime = ConvertToUnixTimestamp(DateTime.Now.ToUniversalTime());
                int seed = Convert.ToInt32(unixTime);

                Random rnd = new Random(seed);

                //this.PlayingTracks.Clear();
                if (this.PlayingTracks.Count == 0)
                {
                    _trackCounter = 0;
                    this.PlayingTracks.AddRange(this.OriginalTracks);
                    this.ShuffleHistory.Clear();
                }

                while (this.PlayingTracks.Count > 0 && this.Playing)
                {
                    this._trackCounter++;
                    int track = rnd.Next(0, this.PlayingTracks.Count);

                    if (!this.ShuffleHistory.ContainsKey(this.PlayingTracks[track]))
                    {
                        // Song has not been played before...
                        this.ShuffleHistory.Add(this.PlayingTracks[track], this.PlayingTracks[track]);

                        // Play song at this point...
                        Console.WriteLine("{1:000}. Playing {0}", this.PlayingTracks[track], _trackCounter);
                        Thread.Sleep(200);
                    }

                    this.PlayingTracks.RemoveAt(track);
                }
            });

            //this.PlayingTracks.Clear();
            this.Playing = false;

            return;
        }

        /// <summary>
        /// Stops the play.
        /// </summary>
        public void StopPlay()
        {
            this.Playing = false;
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
