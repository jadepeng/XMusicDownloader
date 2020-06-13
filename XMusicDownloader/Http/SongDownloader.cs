using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XMusicDownloader.Domain;
using XMusicDownloader.Provider;

namespace XMusicDownloader.Http
{

    public class SongDownloader
    {
        public string rate
        {
            get;
            set;
        } = "320";

        public SongDownloader(MusicProviders musicProviders, string target)
        {
            this.musicProviders = musicProviders;
            this.target = target;
        }

        MusicProviders musicProviders;
        string target;
        List<SongItemDownloader> songs = new List<SongItemDownloader>();
        Queue<MergedSong> queqes = new Queue<MergedSong>();
        int max_downloading_size = 3;

        public double totalPercent
        {

            get
            {
                if (songs.Count == 0)
                {
                    return 100;
                }

                return songs.Sum(s => s.ReceiveProgress) * 100 / songs.Count;
            }
        }

        public double totalSpeed
        {
            get
            {
                return songs.Sum(s => s.receiveSpeed);

            }
        }

        public int queqeCount
        {
            get
            {
                return this.queqes.Count;
            }
        }

        public string currentSongName
        {
            get
            {
                return string.Join(",", songs.Select(s => s.songName).ToList());
            }
        }


        public void AddDownload(MergedSong song)
        {
            if (songs.Count >= max_downloading_size)
            {
                queqes.Enqueue(song);
                return;
            }

            SongItemDownloader downloader = new SongItemDownloader(musicProviders, target, song, rate);
            downloader.DownloadFinish += Downloader_DownloadFinish;

            songs.Add(downloader);

            downloader.Download();

        }

        private void Downloader_DownloadFinish(object sender, SongItemDownloader e)
        {
            songs.Remove(e);
            if (songs.Count < max_downloading_size)
            {
                if (queqes.Count > 0)
                {
                    MergedSong song = queqes.Dequeue();
                    AddDownload(song);
                }

            }
        }


    }

    public delegate void DownloadFinishEvent(object sender, SongItemDownloader e);

    /// <summary>
    /// 单文件下载
    /// </summary>
    public class SongItemDownloader
    {
        MusicProviders musicProviders;
        string target;
        MergedSong song;
        string rate = "320";

        public string songName
        {
            get
            {
                return song.name;
            }
        }


        public event DownloadFinishEvent DownloadFinish;

        public SongItemDownloader(MusicProviders musicProviders, string target, MergedSong song, string rate)
        {
            this.musicProviders = musicProviders;
            this.target = target;
            this.song = song;
            this.rate = rate;
        }

        public long totalBytes;

        public long bytesReceived;

        public double ReceiveProgress;


        public double receiveSpeed;

        DateTime lastTime = DateTime.Now;

        public void Download()
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            new Thread(() =>
            {
                // 多来源，防止单个来源出错
                foreach (var item in song.items)
                {
                    try
                    {
                        client.DownloadFile(musicProviders.getDownloadUrl(item), target + "\\" + item.getFileName());
                        break;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                DownloadFinish?.Invoke(this, this);

            }).Start();
        }

        private void Client_DownloadProgressChanged(object sender, DownloadEventArgs e)
        {
            this.bytesReceived = e.bytesReceived;
            this.totalBytes = e.totalBytes;
            this.receiveSpeed = e.receiveSpeed;
            this.ReceiveProgress = e.ReceiveProgress;
        }
    }
}
