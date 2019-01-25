using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMusicDownloader.Provider;

namespace XMusicDownloader.Domain
{
    public class Song
    {
        public string id { get; set; }
        public string name { get; set; }
        public string singer { get; set; }
        public string album { get; set; }
        public string source { get; set; }
        public double duration { get; set; }
        public double size { get; set; }
        public string url { get; set; }
        public int rate { get; set; }
        public int index { get; set; }

        public string getFileName()
        {
            return singer + "-" + name + ".mp3";
        }

        public string getMergedKey()
        {
            return singer.Replace(" ", "") + name.Replace(" ", "");
        }
    }

    public class MergedSong
    {
        public List<Song> items
        {
            get; set;
        }

        public MergedSong(List<Song> items)
        {
            this.items = items;
        }

        public string name
        {
            get
            {
                return this.items[0].name;
            }
        }
        public string singer
        {
            get
            {
                return this.items[0].singer;
            }
        }
        public string album
        {
            get
            {
                return this.items[0].album;
            }
        }

        public string source
        {
            get
            {
                return string.Join(",", this.items.Select(i => i.source).ToArray());
            }
        }




        public double duration
        {
            get
            {
                return this.items[0].duration;
            }
        }

        public double size
        {
            get
            {
                return this.items[0].size;
            }
        }

        public double rate
        {
            get
            {
                return this.items[0].rate;
            }
        }


        public double score
        {
            get
            {
                // 投票+排序加权  (各50%）
                return this.items.Count / (MusicProviders.Instance.Providers.Count - 1) + (20 - this.items.Average(i => i.index)) / 20;
            }
        }

    }
}
