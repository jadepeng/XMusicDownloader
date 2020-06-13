using Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XMusicDownloader.Domain;

namespace XMusicDownloader.Provider
{
    public class MiguProvider : IMusicProvider
    {
        static HttpConfig DEFAULT_CONFIG = new HttpConfig
        {
            Referer = "http://m.music.migu.cn/",

        };

        public string Name { get; } = "咪咕";


        public List<Song> SearchSongs(string keyword, int page, int pageSize)
        {
            var searchResult = HttpHelper.GET(string.Format("http://m.music.migu.cn/migu/remoting/scr_search_tag?keyword={0}&pgc={1}&rows={2}&type=2", keyword, page, pageSize), DEFAULT_CONFIG);
            var result = new List<Song>();
            try
            {
                var searchResultJson = JsonParser.Deserialize(searchResult).musics;
                var songIds = new List<string>();
                var index = 1;

                foreach (var songItem in searchResultJson)
                {
                    var song = new Song
                    {
                        id = (string)songItem["id"],
                        name = (string)songItem["songName"],
                        singer = (string)songItem["singerName"],
                        album = (string)songItem["albumName"],
                        rate = 128,
                        index = index++,
                        size = 0,
                        source = Name,
                        url = (string)songItem["mp3"],
                        duration = 0
                    };

                    result.Add(song);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;

        }

        public string getDownloadUrl(Song song)
        {
            return song.url;

        }
        // http://music.taihe.com/songlist/516288502
        // http://music.taihe.com/album/182772

        public bool Support(string url)
        {
            return false;
        }

        Regex regex = new Regex("(\\d+)");

        public List<Song> GetSongList(string url)
        {
            var result = new List<Song>();
            return result;
         }

      
        public string getDownloadUrl(string id, string rate)
        {
          return HttpHelper.DetectLocationUrl("https://v1.itooi.cn/baidu/url?id=" + id + "&quality=" + rate, DEFAULT_CONFIG);
        }

    }
}
