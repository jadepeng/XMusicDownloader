using Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMusicDownloader.Domain;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace XMusicDownloader.Provider
{
    public class NeteaseProvider : IMusicProvider
    {
        static HttpConfig DEFAULT_CONFIG = new HttpConfig
        {
            Referer = "http://music.163.com/",

        };

        public string Name { get; } = "网易";



        public List<Song> SearchSongs(string keyword, int page, int pageSize)
        {

            var searchResult = HttpHelper.GET(string.Format("http://music.163.com/api/cloudsearch/pc?s={0}&type=1&offset={1}&limit={2}", keyword, (page - 1) * pageSize, pageSize), DEFAULT_CONFIG);
            var result = new List<Song>();
            try
            {

                var songList = JObject.Parse(searchResult)["result"]["songs"];
                var index = 1;

                foreach (var songItem in songList)
                {

                    if ((int)songItem["privilege"]["fl"] == 0)
                    {
                        // 无版权
                        continue;
                    }

                    Song song = extractSong(ref index, songItem);
                    result.Add(song);
                }
            }
            catch (Exception ex)
            {

            }
            return result;

        }

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

            return url.StartsWith("https://music.163.com/#/playlist?id=") || url.StartsWith("https://music.163.com/#/album?id=");
        }

        Regex regex = new Regex("id=(\\d+)");

        public List<Song> GetSongList(string url)
        {
            var isSongList = url.StartsWith("https://music.163.com/#/playlist?id=");

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

        private void GetAlbum(string id, List<Song> result)
        {
            var requestUrl = "https://v1.itooi.cn/netease/album?id=" + id;
            var searchResult = HttpHelper.GET(requestUrl, DEFAULT_CONFIG);

            var songList = JObject.Parse(searchResult)["data"]["songs"];
            var index = 1;

            foreach (var songItem in songList)
            {
                Song song = extractSong(ref index, songItem);
                result.Add(song);

            }
        }

        private Song extractSong(ref int index, JToken songItem)
        {
            var song = new Song
            {
                id = (string)songItem["id"],
                name = (string)songItem["name"],

                album = (string)songItem["al"]["name"],
                //rate = 128,
                index = index++,
                //size = (double)songItem["FileSize"],
                source = Name,
                duration = (double)songItem["dt"] / 1000
            };

            song.singer = "";
            foreach (var ar in songItem["ar"])
            {
                song.singer += ar["name"] + " ";
            }

            song.rate = ((int)songItem["privilege"]["fl"]) / 1000;

            var fl = (int)songItem["privilege"]["fl"];
            if (songItem["h"] != null && fl >= 320000)
            {
                song.size = (double)songItem["h"]["size"];
            }
            else if (songItem["m"] != null && fl >= 192000)
            {
                song.size = (double)songItem["m"]["size"];
            }
            else if (songItem["l"] != null)
            {
                song.size = (double)songItem["l"]["size"];
            }

            return song;
        }

        private void GetSongListDetail(string id, List<Song> result)
        {
            var requestUrl = "https://v1.itooi.cn/netease/songList?id=" + id;
            var searchResult = HttpHelper.GET(requestUrl, DEFAULT_CONFIG);

            var songList = JObject.Parse(searchResult)["data"]["tracks"];
            var index = 1;

            foreach (var songItem in songList)
            {
                var song = new Song
                {
                    id = (string)songItem["id"],
                    name = (string)songItem["name"],
                    album = (string)songItem["album"]["name"],
                    //rate = 128,
                    index = index++,
                    //size = (double)songItem["FileSize"],
                    source = Name,
                    duration = (double)songItem["duration"] / 1000
                };
                song.singer = "";
                foreach (var ar in songItem["artists"])
                {
                    song.singer += ar["name"] + " ";
                }


                if (songItem["hMusic"] != null)
                {
                    song.size = (double)songItem["hMusic"]["size"];
                    song.rate = (int)songItem["hMusic"]["bitrate"];
                }
                else if (songItem["mMusic"] != null)
                {
                    song.size = (double)songItem["mMusic"]["size"];
                    song.rate = (int)songItem["mMusic"]["bitrate"];
                }
                else if (songItem["lMusic"] != null)
                {
                    song.size = (double)songItem["lMusic"]["size"];
                    song.rate = (int)songItem["lMusic"]["bitrate"];
                }
                result.Add(song);

            }
        }

        public string getDownloadUrl(string id, string rate)
        {
            return HttpHelper.DetectLocationUrl("https://v1.itooi.cn/netease/url?id=" + id + "&quality=" + rate,DEFAULT_CONFIG);
        }

        public string getDownloadUrl(Song song)
        {

            var param = new JObject();
            //param["method"] = "POST";
            //param["url"] = "http://music.163.com/api/song/enhance/player/url";
            //var param_params = new JObject();
            //param_params["ids"] = new JArray(new string[] { song.id });
            //param_params["br"] = 320000;
            //param["params"] = param_params;

            //var param_json = param.ToString();

            var urlInfO = JsonParser.Deserialize(HttpHelper.GET(string.Format(" http://music.163.com/api/song/enhance/player/url?id={0}&ids=%5B{0}%5D&br=3200000", song.id), DEFAULT_CONFIG));
            return urlInfO.data[0]["url"];

        }



    }

}
