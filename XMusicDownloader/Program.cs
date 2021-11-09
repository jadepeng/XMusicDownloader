using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMusicDownloader.Http;
using XMusicDownloader.Provider;
using XMusicDownloader.Utils;

namespace XMusicDownloader
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            //string key = Douyin.ParseVideoUrl("https://v.douyin.com/JPa1xhq/");
            //Console.WriteLine(key);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12; //加上这一句

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
