using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMusicDownloader.Extensions;

namespace XMusicDownloader.Http
{
    public class Douyin
    {
        static HttpConfig DEFAULT_CONFIG = new HttpConfig
        {
            Referer = "http://music.baidu.com/",

        };

        public static string ParseVideoUrl(string shareUrL)
        {
            string longUrl = HttpHelper.DetectLocationUrl(shareUrL, DEFAULT_CONFIG);
            // /video/6883418578486349070/?
            string videoId = longUrl.GetBetween("/video/", "/?");
            string videoJson = HttpHelper.GET("https://www.iesdouyin.com/web/api/v2/aweme/iteminfo/?item_ids=" + videoId, DEFAULT_CONFIG);
            var video = JObject.Parse(videoJson)["item_list"][0]["video"];
            return video["play_addr"]["url_list"][0].ToString().Replace("playwm", "play");
        }
    }
}
