using Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMusicDownloader.Domain;

namespace XMusicDownloader.Provider
{
    public class QQProvider : IMusicProvider
    {
        static HttpConfig DEFAULT_CONFIG = new HttpConfig
        {
            Referer = "http://m.y.qq.com",

        };

        public string Name { get; } = "QQ";

        static string[] prefixes = new string[] { "M800", "M500", "C400" };

        public List<Song> SearchSongs(string keyword,int page,int pageSize)
        {
            var searchResult = HttpHelper.GET(string.Format("http://c.y.qq.com/soso/fcgi-bin/search_for_qq_cp?w={0}&format=json&p={1}&n={2}", keyword, page,pageSize), DEFAULT_CONFIG);
            var searchResultJson = JsonParser.Deserialize(searchResult).data.song;
            //return json.mods.itemlist.data.collections[0];
            var result = new List<Song>();

            var index = 1;
            foreach(var songItem in searchResultJson.list)
            {
                var song = new Song
                {
                    id = songItem["songmid"],
                    name = songItem["songname"],
                    album = songItem["albumname"],
                    rate = 128,
                    size = songItem["size128"],
                    source = Name,
                    index = index++,
                    duration = songItem["interval"]
                };
                song.singer = "";
                foreach (var ar in songItem["singer"])
                {
                    song.singer += ar["name"] + " ";
                }
                result.Add(song);
            }

            return result;

        }

        public string getDownloadUrl(Song song)
        {
            var guid = new Random().Next(1000000000, 2000000000);

            var key = JsonParser.Deserialize(HttpHelper.GET(string.Format("http://base.music.qq.com/fcgi-bin/fcg_musicexpress.fcg?guid={0}&format=json&json=3",guid), DEFAULT_CONFIG)).key;
            foreach(var prefix in prefixes)
            {
               
                var musicUrl = string.Format("http://dl.stream.qqmusic.qq.com/{0}{1}.mp3?vkey={2}&guid={3}&fromtag=1", prefix, song.id, key, guid);
                if (HttpHelper.GetUrlContentLength(musicUrl) > 0)
                {
                    return musicUrl;
                }
            }

            return null;

        }
    
    }
}
