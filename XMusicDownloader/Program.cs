using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            //String key = AESHelper.AESEncrypt("{\"copyrightId\":\"60054701934\",\"auditionsFlag\":0}", "4ea5c508a6566e76240543f8feb06fd457777be39549c4016436afda65d2330e");

            //Console.WriteLine(key);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12; //加上这一句

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
