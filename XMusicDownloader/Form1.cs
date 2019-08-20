using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinControl;
using System.Threading.Tasks;
using System.Threading;
using XMusicDownloader.Provider;
using XMusicDownloader.Domain;
using XMusicDownloader.Http;
using System.IO;

namespace XMusicDownloader
{
    public partial class Form1 : CCSkinMain
    {
        // 用于将ListView更新的的委托类型
        delegate void UpdateListCallback(List<ListViewItem> listViewItems);

        public Form1()
        {
            InitializeComponent();
        }
        string target = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Download\\";

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.Text = target;
            this.cbRate.SelectedIndex = 2;
        }

        //浏览
        private void pathBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd.SelectedPath + "\\";
                target = textBox2.Text;
            }
        }

        int page = 1;

        //搜索
        private void searchBtn_Click(object sender, EventArgs e)
        {
            page = 1;
            GetList(page);
        }

        //上一页
        private void lastPageBtn_Click(object sender, EventArgs e)
        {
            if (page > 1)
            {
                page--;
                GetList(page);

                if (page == 1)
                    lastPageBtn.Enabled = false;
            }
        }

        //下一页
        private void nextPageBtn_Click(object sender, EventArgs e)
        {
            page++;
            GetList(page);

            if (page > 1)
            {
                lastPageBtn.Enabled = true;
            }
        }

        /// <summary>
        /// 开始显示进度栏动画
        /// </summary>
        private void StartProcessBar()
        {
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            toolStripProgressBar1.MarqueeAnimationSpeed = 10;
        }

        /// <summary>
        /// 结束显示进度栏动画
        /// </summary>
        private void StopProcessBar()
        {
            toolStripProgressBar1.Visible = false;
            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
        }

        MusicProviders provider = MusicProviders.Instance;


        /// <summary>
        /// 获取歌曲列表
        /// </summary>
        /// <param name="page"></param>
        private void GetList(int page)
        {
            StartProcessBar();
            pageNum.Text = "第" + page + "页";
            resultListView.Items.Clear();
            toolStripStatusLabel1.Text = "搜索中...";
            List<ListViewItem> listViewItems = new List<ListViewItem>();


            var songs = tblSearch.SelectedIndex == 0 ? provider.SearchSongs(textBox1.Text, page, 20) : provider.SearchSongsList(txtSongListUrl.Text);

            songs.ForEach(item =>
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = item.name;
                lvi.SubItems.Add(item.singer);
                lvi.SubItems.Add(item.rate + "kb");
                lvi.SubItems.Add((item.size / (1024 * 1024)).ToString("F2") + "MB");  //将文件大小装换成MB的单位
                TimeSpan ts = new TimeSpan(0, 0, (int)item.duration); //把秒数换算成分钟数
                lvi.SubItems.Add(ts.Minutes + ":" + ts.Seconds.ToString("00"));
                lvi.SubItems.Add(item.source);
                lvi.Tag = item;
                listViewItems.Add(lvi);
            });


            UpdateUI(listViewItems);

            if(songs.Count == 0)
            {
                toolStripStatusLabel1.Text = "未找到记录";
            }

        }

        /// <summary>
        /// 用于在获取歌曲列表的Task中更新界面
        /// </summary>
        /// <param name="listViewItems"></param>
        private void UpdateUI(List<ListViewItem> listViewItems)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (this.resultListView.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.resultListView.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.resultListView.Disposing || this.resultListView.IsDisposed)
                        return;
                }
                UpdateListCallback d = new UpdateListCallback(UpdateUI);
                resultListView.Invoke(d, new object[] { listViewItems });
            }
            else
            {
                resultListView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
                resultListView.Items.AddRange(listViewItems.ToArray());
                resultListView.EndUpdate();  //结束数据处理，UI界面一次性绘制
                toolStripStatusLabel1.Text = "搜索完成";
                StopProcessBar();

                if (tblSearch.SelectedIndex == 0)
                {
                    if (resultListView.Items.Count > 0)
                    {
                        nextPageBtn.Enabled = true;
                    }
                    else
                    {
                        nextPageBtn.Enabled = false;
                    }
                }
                else
                {
                    nextPageBtn.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 计算MD5获得下载的key值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] bf = Encoding.Default.GetBytes(str);
            byte[] mbf = md5.ComputeHash(bf);
            string s = "";
            for (int i = 0; i < mbf.Length; i++)
            {
                s += mbf[i].ToString("x2");
            }
            return s;
        }



        private string getSongUrl(Song song)
        {
            return provider.getDownloadUrl(song);
        }

        SongDownloader downloader;

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downBtn_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Visible = true;

                if (!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                }

                if (downloader == null)
                {
                    downloader = new SongDownloader(provider, target);
                }
                downloader.rate = this.cbRate.SelectedItem.ToString();

                foreach (ListViewItem item in resultListView.CheckedItems)
                {
                    timer1.Enabled = true;
                    timer1.Interval = 500;
                    var song = (MergedSong)item.Tag;
                    downloader.AddDownload(song);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "当前下载："+downloader.currentSongName+" 下载进度：" + (int)(downloader.totalPercent) + "%" + string.Format(" 下载速度：{0}", (downloader.totalSpeed / 1024.0 / 1024.0).ToString("F2") + "MB/s 排队数:" + downloader.queqeCount);
            toolStripProgressBar1.Value = (int)(downloader.totalPercent);
            if (downloader.totalPercent >= 100d)
            {
                toolStripStatusLabel1.Text = "下载完成！";
                timer1.Enabled = false;
                toolStripProgressBar1.Visible = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnGetSongList_Click(object sender, EventArgs e)
        {
            GetList(1);
        }

        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in resultListView.Items)
            {
                item.Checked = cbSelectAll.Checked;
            }
        }
    }
}
