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
    public class BaiduProvider : IMusicProvider
    {
        static HttpConfig DEFAULT_CONFIG = new HttpConfig
        {
            Referer = "http://music.baidu.com/",

        };

        public string Name { get; } = "百度";


        public List<Song> SearchSongs(string keyword, int page, int pageSize)
        {
            var searchResult = HttpHelper.GET(string.Format("http://musicapi.qianqian.com/v1/restserver/ting?query={0}&method=baidu.ting.search.common&format=json&page_no={1}&page_size={2}", keyword, page, pageSize), DEFAULT_CONFIG);
            var result = new List<Song>();
            try
            {
                var searchResultJson = JsonParser.Deserialize(searchResult).song_list;
                var songIds = new List<string>();

                foreach (var item in searchResultJson)
                {
                    songIds.Add(item["song_id"]);
                }

                var songIdsStr = string.Join(",", songIds.ToArray());

                var songInfos = HttpHelper.GET(string.Format("http://music.taihe.com/data/music/links?songIds={0}", songIdsStr), DEFAULT_CONFIG);
                var songList = JObject.Parse(songInfos)["data"]["songList"];

                var index = 1;
                foreach (var songItem in songList)
                {
                    var song = new Song
                    {
                        id = (string)songItem["queryId"],
                        name = (string)songItem["songName"],
                        singer = (string)songItem["artistName"],
                        album = (string)songItem["albumName"],
                        rate = 128,
                        index = index++,
                        size = (double)songItem["size"],
                        source = Name,
                        url = (string)songItem["songLink"],
                        duration = (double)songItem["time"]
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
            return song.url;

        }
        // http://music.taihe.com/songlist/516288502
        // http://music.taihe.com/album/182772

        public bool Support(string url)
        {
            if (url == null)
            {
                return false;
            }

            if (!regex.IsMatch(url))
            {
                return false;
            }

            return url.StartsWith("http://music.taihe.com/songlist") || url.StartsWith("http://music.taihe.com/album");
        }

        Regex regex = new Regex("(\\d+)");

        public List<Song> GetSongList(string url)
        {
            var isSongList = url.StartsWith("http://music.taihe.com/songlist");

            var id = regex.Match(url).Groups[1].Value;

            var result = new List<Song>();

            if (isSongList)
            {
                GetSongListDetail(id, result);
            }
            else
            {
                GetAlbum(id, result);
            }


            return result;

        }

        private void GetSongListDetail(string id, List<Song> result)
        {
            var requestUrl = "https://v1.itooi.cn/baidu/songList?id=" + id;
            var searchResult = HttpHelper.GET(requestUrl, DEFAULT_CONFIG);

            var songList = JObject.Parse(searchResult)["data"];
            var index = 1;

            foreach (var songItem in songList)
            {
                var song = new Song
                {
                    id = (string)songItem["song_id"],
                    name = (string)songItem["title"],
                    album = (string)songItem["album_title"],
                    //rate = 128,
                    index = index++,
                    //size = (double)songItem["FileSize"],
                    source = Name,
                    singer = (string)songItem["author"],
                    duration = double.Parse((string)songItem["file_duration"])
                };
   

                if (songItem["all_rate"] != null)
                {
                    if (songItem["all_rate"].ToString().Contains("flac"))
                    {
                        song.rate = 999;
                    }
                    else if (songItem["all_rate"].ToString().Contains("320"))
                    {
                        song.rate = 320;
                    }
                    else
                    {
                        song.rate = 128;
                    }
                   
                }
             
                result.Add(song);

            }
        }



        private void GetAlbum(string id, List<Song> result)
        {
            var requestUrl = "https://v1.itooi.cn/baidu/album?id=" + id;
            var searchResult = HttpHelper.GET(requestUrl, DEFAULT_CONFIG);

            var songList = JObject.Parse(searchResult)["data"]["songlist"];
            var index = 1;

            foreach (var songItem in songList)
            {
                var song = new Song
                {
                    id = (string)songItem["song_id"],
                    name = (string)songItem["title"],
                    album = (string)songItem["album_title"],
                    //rate = 128,
                    index = index++,
                    //size = (double)songItem["FileSize"],
                    source = Name,
                    singer = (string)songItem["author"],
                    duration = double.Parse((string)songItem["file_duration"])
                };


                if (songItem["all_rate"] != null)
                {
                    if (songItem["all_rate"].ToString().Contains("flac"))
                    {
                        song.rate = 999;
                    }
                    else if (songItem["all_rate"].ToString().Contains("320"))
                    {
                        song.rate = 320;
                    }
                    else
                    {
                        song.rate = 128;
                    }

                }

                result.Add(song);

            }
        }

        public string getDownloadUrl(string id, string rate)
        {
          return HttpHelper.DetectLocationUrl("https://v1.itooi.cn/baidu/url?id=" + id + "&quality=" + rate, DEFAULT_CONFIG);
        }

    }
}
