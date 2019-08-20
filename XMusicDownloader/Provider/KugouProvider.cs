using Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMusicDownloader.Domain;

namespace XMusicDownloader.Provider
{
    public class KugouProvider : IMusicProvider
    {
        static HttpConfig DEFAULT_CONFIG = new HttpConfig
        {
            Referer = "http://m.kugou.com",

        };

        public string Name { get; } = "酷狗";


        public List<Song> SearchSongs(string keyword, int page, int pageSize)
        {

            var searchResult = HttpHelper.GET(string.Format("http://songsearch.kugou.com/song_search_v2?keyword={0}&platform=WebFilter&format=json&page={1}&pagesize={2}", keyword, page, pageSize), DEFAULT_CONFIG);
            var result = new List<Song>();
            try
            {
 
                var songList = JObject.Parse(searchResult)["data"]["lists"];
                var index = 1;

                foreach (var songItem in songList)
                {
                    var song = new Song
                    {
                        id = (string)songItem["FileHash"],
                        name = (string)songItem["SongName"],
                        singer = (string)songItem["SingerName"],
                        album = (string)songItem["AlbumName"],
                        rate = 128,
                        index = index++,
                        size = (double)songItem["FileSize"],
                        source = Name,
                        duration = (double)songItem["Duration"]
                    };

                    result.Add(song);
                }
            }
            catch (Exception ex)
            {

            }
            return result;

        }

        public string getDownloadUrl(Song song)
        {
            var urlInfO = JsonParser.Deserialize(HttpHelper.GET(string.Format("http://m.kugou.com/app/i/getSongInfo.php?cmd=playInfo&hash={0}", song.id), DEFAULT_CONFIG));
            return urlInfO.url;

        }

        public bool Support(string url)
        {
            return false;
        }

        public List<Song> GetSongList(string url)
        {
            throw new NotImplementedException();
        }

        public string getDownloadUrl(string id, string rate)
        {
            return HttpHelper.DetectLocationUrl("https://v1.itooi.cn/kugou/url?id=" + id + "&quality=" + rate,DEFAULT_CONFIG);
        }

    }
}
