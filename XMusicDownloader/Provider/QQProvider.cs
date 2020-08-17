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
           return getMusicUrl(song.id,"320");

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

        private string GetOggVkey(string songmid)
        {
            string param = "{\"comm\":{\"ct\":\"19\",\"cv\":\"1724\",\"patch\":\"118\",\"uin\":\"0\",\"wid\":\"0\"},\"queryvkey\":{\"method\":\"CgiGetEVkey\",\"module\":\"vkey.GetEVkeyServer\",\"param\":{\"checklimit\":0,\"ctx\":1,\"downloadfrom\":0,\"filename\":[\"O6M0003uw9dp2HcDl2.mgg\",\"O6M0"+ songmid+".mgg\"],\"guid\":\"CD2594E1E7AD35046B95E7E1482E074B\",\"musicfile\":[\"O6M0003uw9dp2HcDl2.mgg\",\"O6M0"+ songmid+".mgg\"],\"nettype\":\"\",\"referer\":\"y.qq.com\",\"scene\":0,\"songmid\":[\"003uw9dp2HcDl2\",\""+ songmid+"\"],\"songtype\":[1,1],\"uin\":\"1719982754\"}}}";
            string result = HttpHelper.POST("https://u.y.qq.com/cgi-bin/musicu.fcg", param, DEFAULT_CONFIG);
            return (string)JObject.Parse(result)["queryvkey"]["data"]["midurlinfo"][1]["purl"];
        }

        double getGuid()
        {
            return new Random().Next(1000000000, 2000000000);
        }

        string getPurl(string songmid)
        {
            string paramStr = "{\"req\":{\"module\":\"CDN.SrfCdnDispatchServer\",\"method\":\"GetCdnDispatch\",\"param\":{\"guid\":\""+ getGuid()+"\",\"calltype\":0,\"userip\":\"\"}},\"req_0\":{\"module\":\"vkey.GetVkeyServer\",\"method\":\"CgiGetVkey\",\"param\":{\"guid\":\""+ getGuid()+"\",\"songmid\":[\""+songmid+"\"],\"songtype\":[0],\"uin\":\"2461958018\",\"loginflag\":1,\"platform\":\"20\"}},\"comm\":{\"uin\":2461958018,\"format\":\"json\",\"ct\":24,\"cv\":0}}";
            string url = "https://u.y.qq.com/cgi-bin/musicu.fcg?g_tk=5381&format=json&inCharset=utf8&outCharset=utf-8&data=" + paramStr;
            var response = HttpHelper.GET(url, DEFAULT_CONFIG);

            JObject result = JObject.Parse(response);
            string vkey =  (string)result["req_0"]["data"]["midurlinfo"][0]["purl"];
            if(vkey.Length == 0)
            {
                return null;
            }

           return (string)result["req_0"]["data"]["sip"][0] + vkey;
        }

        public string getMusicUrl(string songmid, string size)
        {

            return getPurl(songmid);
          
            //string vkey = GetOggVkey(songmid);

            //if(vkey.Length == 0)
            //{
            //    size = "128";
            //}


            //string[] prefix = {
            //"http://124.89.197.14/amobile.music.tc.qq.com/",
            //"http://124.89.197.15/amobile.music.tc.qq.com/",
            //"http://isure.stream.qqmusic.qq.com/",
            //"http://ws.stream.qqmusic.qq.com/",
            //"http://183.240.120.28/amobile.music.tc.qq.com"
            //};

            ////    选择不同音质
            //switch (size)
            //{
            //    case "flac":
            //        return string.Format("{0}F000{1}.flac?guid=CD2594E1E7AD35046B95E7E1482E074B&vkey={2}&uin=0&fromtag=53", prefix[1], songmid, vkey);
            //    case "ape":
            //        return string.Format("{0}A000{1}.ape?guid=CD2594E1E7AD35046B95E7E1482E074B&vkey={2}&uin=0&fromtag=8", prefix[1], songmid, vkey);
            //    case "320":
            //        return string.Format("{0}M800{1}.mp3?guid=CD2594E1E7AD35046B95E7E1482E074B&vkey={2}&uin=0&fromtag=30", prefix[1], songmid, vkey);
            //    case "mgg":
            //        return string.Format("{0}O6M0{1}.mgg?guid=CD2594E1E7AD35046B95E7E1482E074B&vkey={2}&uin=0&fromtag=77", prefix[1], songmid, vkey);
            //    case "128":
            //        {

            //        }
            //    default:
            //        return string.Format("{0}{1}", prefix[3], getPurl(songmid));
            //}
        }
    }
}
