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
    public class QQProvider : IMusicProvider
    {
        static HttpConfig DEFAULT_CONFIG = new HttpConfig
        {
            Referer = "http://m.y.qq.com",

        };

        public string Name { get; } = "QQ";

        static string[] prefixes = new string[] { "M800", "M500", "C400" };

        public List<Song> SearchSongs(string keyword, int page, int pageSize)
        {
            var searchResult = HttpHelper.GET(string.Format("http://c.y.qq.com/soso/fcgi-bin/search_for_qq_cp?w={0}&format=json&p={1}&n={2}", keyword, page, pageSize), DEFAULT_CONFIG);
            var searchResultJson = JsonParser.Deserialize(searchResult).data.song;
            //return json.mods.itemlist.data.collections[0];
            var result = new List<Song>();

            var index = 1;
            foreach (var songItem in searchResultJson.list)
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

            var key = JsonParser.Deserialize(HttpHelper.GET(string.Format("http://base.music.qq.com/fcgi-bin/fcg_musicexpress.fcg?guid={0}&format=json&json=3", guid), DEFAULT_CONFIG)).key;
            foreach (var prefix in prefixes)
            {

                var musicUrl = string.Format("http://dl.stream.qqmusic.qq.com/{0}{1}.mp3?vkey={2}&guid={3}&fromtag=1", prefix, song.id, key, guid);
                if (HttpHelper.GetUrlContentLength(musicUrl) > 0)
                {
                    return musicUrl;
                }
            }

            return null;

        }



        // https://y.qq.com/n/yqq/playsquare/6924336223.html#stat=y_new.playlist.dissname
        // https://y.qq.com/n/yqq/album/00153q8l2vldMz.html
        // 歌手 https://y.qq.com/n/yqq/singer/000CK5xN3yZDJt.html

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

            return url.StartsWith("https://y.qq.com/n/yqq/playsquare") || url.StartsWith("https://y.qq.com/n/yqq/album") || url.StartsWith("https://y.qq.com/n/yqq/singer");
        }

        Regex regex = new Regex("\\/(\\w+).html");

        public List<Song> GetSongList(string url)
        {
            var isSongList = url.StartsWith("https://y.qq.com/n/yqq/playsquare");

            var id = regex.Match(url).Groups[1].Value;

            var result = new List<Song>();

            if (isSongList)
            {
                GetSongListDetail(id, result);
            }
            else if (url.StartsWith("https://y.qq.com/n/yqq/albu"))
            {
                GetAlbum(id, result);
            }
            else
            {
                GetSingerSong(id, result);
            }


            return result;

        }

        private void GetSongListDetail(string id, List<Song> result)
        {
            var requestUrl = "https://v1.itooi.cn/tencent/songList?id=" + id;
            var searchResult = HttpHelper.GET(requestUrl, DEFAULT_CONFIG);

            var songList = JObject.Parse(searchResult)["data"][0]["songlist"];
            var index = 1;

            foreach (var songItem in songList)
            {
                var song = new Song
                {
                    id = (string)songItem["songmid"],
                    name = (string)songItem["title"],
                    album = (string)songItem["album"]["name"],
                    rate = 320,
                    index = index++,
                    size = (double)songItem["file"]["size_320mp3"],
                    source = Name,
                    //singer = (string)songItem["author"],
                    duration = (double)songItem["interval"]
                };
                if (song.size == 0d)
                {
                    song.size = (double)songItem["file"]["size_128mp3"];
                    song.rate = 128;
                }
                song.singer = "";
                foreach (var ar in songItem["singer"])
                {
                    song.singer += ar["name"] + " ";
                }
                result.Add(song);

            }
        }



        private void GetAlbum(string id, List<Song> result)
        {
            var requestUrl = "https://v1.itooi.cn/tencent/album?id=" + id;
            var searchResult = HttpHelper.GET(requestUrl, DEFAULT_CONFIG);

            var songList = JObject.Parse(searchResult)["data"]["getSongInfo"];
            var index = 1;

            foreach (var songItem in songList)
            {
                var song = new Song
                {
                    id = (string)songItem["songmid"],
                    name = (string)songItem["songname"],
                    album = (string)songItem["albumname"],
                    rate = 320,
                    index = index++,
                    size = (double)songItem["size320"],
                    source = Name,
                    //singer = (string)songItem["author"],
                    duration = (double)songItem["interval"]
                };

                if (song.size == 0d)
                {
                    song.size = (double)songItem["size128"];
                    song.rate = 128;
                }

                song.singer = "";
                foreach (var ar in songItem["singer"])
                {
                    song.singer += ar["name"] + " ";
                }
                result.Add(song);

            }
        }

        private void GetSingerSong(string id, List<Song> result)
        {
            // top 200 歌曲
            var requestUrl = "https://v1.itooi.cn/tencent/song/artist?id=" + id + "&pageSize=200";
            var searchResult = HttpHelper.GET(requestUrl, DEFAULT_CONFIG);

            var songList = JObject.Parse(searchResult)["data"];
            var index = 1;

            foreach (var item in songList)
            {
                var songItem = item["musicData"];
                var song = new Song
                {
                    id = (string)songItem["songmid"],
                    name = (string)songItem["songname"],
                    album = (string)songItem["albumname"],
                    rate = 320,
                    index = index++,
                    size = (double)songItem["size320"],
                    source = Name,
                    //singer = (string)songItem["author"],
                    duration = (double)songItem["interval"]
                };

                if (song.size == 0d)
                {
                    song.size = (double)songItem["size128"];
                    song.rate = 128;
                }

                song.singer = "";
                foreach (var ar in songItem["singer"])
                {
                    song.singer += ar["name"] + " ";
                }
                result.Add(song);

            }
        }



        public string getDownloadUrl(string id, string rate)
        {
            return HttpHelper.DetectLocationUrl("https://v1.itooi.cn/tencent/url?id=" + id + "&quality=" + rate, DEFAULT_CONFIG);
        }
    }
}
