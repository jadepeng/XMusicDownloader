using Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMusicDownloader.Domain;
using System.Security.Cryptography;

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

                    var song = new Song
                    {
                        id = (string)songItem["id"],
                        name = (string)songItem["name"],
                       
                        album = (string)songItem["al"]["name"],
                        //rate = 128,
                        index = index++,
                        //size = (double)songItem["FileSize"],
                        source = Name,
                        duration = (double)songItem["dt"]/ 1000
                    };

                    song.singer = "";
                    foreach(var ar in songItem["ar"])
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
