﻿namespace XMusicDownloader
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtSearchBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDownloadPath = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.searchBtn = new CCWin.SkinControl.SkinButton();
            this.downBtn = new CCWin.SkinControl.SkinButton();
            this.pathBtn = new CCWin.SkinControl.SkinButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.resultListView = new System.Windows.Forms.ListView();
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.singer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bitRate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.source = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastPageBtn = new CCWin.SkinControl.SkinButton();
            this.nextPageBtn = new CCWin.SkinControl.SkinButton();
            this.pageNum = new System.Windows.Forms.Label();
            this.tblSearch = new System.Windows.Forms.TabControl();
            this.tabKeyword = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtSongListUrl = new System.Windows.Forms.TextBox();
            this.btnGetSongList = new CCWin.SkinControl.SkinButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cbRate = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.tblSearch.SuspendLayout();
            this.tabKeyword.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSearchBox.Location = new System.Drawing.Point(4, 3);
            this.txtSearchBox.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(452, 26);
            this.txtSearchBox.TabIndex = 0;
            this.txtSearchBox.Text = "周杰伦";
            this.txtSearchBox.TextChanged += new System.EventHandler(this.txtSearchBox_TextChanged);
            this.txtSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(21, 580);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "下载到：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtDownloadPath
            // 
            this.txtDownloadPath.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDownloadPath.Location = new System.Drawing.Point(77, 577);
            this.txtDownloadPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtDownloadPath.Name = "txtDownloadPath";
            this.txtDownloadPath.Size = new System.Drawing.Size(417, 26);
            this.txtDownloadPath.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // searchBtn
            // 
            this.searchBtn.BackColor = System.Drawing.Color.Transparent;
            this.searchBtn.BaseColor = System.Drawing.Color.LightBlue;
            this.searchBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.searchBtn.DownBack = null;
            this.searchBtn.DownBaseColor = System.Drawing.Color.Gainsboro;
            this.searchBtn.GlowColor = System.Drawing.Color.LightBlue;
            this.searchBtn.Location = new System.Drawing.Point(471, 4);
            this.searchBtn.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.searchBtn.MouseBack = null;
            this.searchBtn.MouseBaseColor = System.Drawing.Color.LightBlue;
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.NormlBack = null;
            this.searchBtn.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.searchBtn.Size = new System.Drawing.Size(60, 26);
            this.searchBtn.TabIndex = 9;
            this.searchBtn.Text = "搜索";
            this.searchBtn.UseVisualStyleBackColor = false;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // downBtn
            // 
            this.downBtn.BackColor = System.Drawing.Color.Transparent;
            this.downBtn.BaseColor = System.Drawing.Color.LightBlue;
            this.downBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.downBtn.DownBack = null;
            this.downBtn.DownBaseColor = System.Drawing.Color.LightBlue;
            this.downBtn.Location = new System.Drawing.Point(606, 574);
            this.downBtn.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.downBtn.MouseBack = null;
            this.downBtn.MouseBaseColor = System.Drawing.Color.LightBlue;
            this.downBtn.Name = "downBtn";
            this.downBtn.NormlBack = null;
            this.downBtn.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.downBtn.Size = new System.Drawing.Size(124, 35);
            this.downBtn.TabIndex = 10;
            this.downBtn.Text = "开始下载";
            this.downBtn.UseVisualStyleBackColor = false;
            this.downBtn.Click += new System.EventHandler(this.downBtn_Click);
            // 
            // pathBtn
            // 
            this.pathBtn.BackColor = System.Drawing.Color.Transparent;
            this.pathBtn.BaseColor = System.Drawing.Color.LightBlue;
            this.pathBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.pathBtn.DownBack = null;
            this.pathBtn.DownBaseColor = System.Drawing.Color.LightBlue;
            this.pathBtn.Location = new System.Drawing.Point(500, 577);
            this.pathBtn.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.pathBtn.MouseBack = null;
            this.pathBtn.MouseBaseColor = System.Drawing.Color.LightBlue;
            this.pathBtn.Name = "pathBtn";
            this.pathBtn.NormlBack = null;
            this.pathBtn.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.pathBtn.Size = new System.Drawing.Size(60, 26);
            this.pathBtn.TabIndex = 11;
            this.pathBtn.Text = "浏览";
            this.pathBtn.UseVisualStyleBackColor = false;
            this.pathBtn.Click += new System.EventHandler(this.pathBtn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(4, 613);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel1.Text = "准备就绪";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(201, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // resultListView
            // 
            this.resultListView.CheckBoxes = true;
            this.resultListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.singer,
            this.bitRate,
            this.size,
            this.time,
            this.source});
            this.resultListView.FullRowSelect = true;
            this.resultListView.Location = new System.Drawing.Point(8, 91);
            this.resultListView.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.resultListView.Name = "resultListView";
            this.resultListView.Size = new System.Drawing.Size(786, 441);
            this.resultListView.TabIndex = 14;
            this.resultListView.UseCompatibleStateImageBehavior = false;
            this.resultListView.View = System.Windows.Forms.View.Details;
            // 
            // name
            // 
            this.name.Text = "歌名";
            this.name.Width = 200;
            // 
            // singer
            // 
            this.singer.Text = "歌手";
            this.singer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.singer.Width = 200;
            // 
            // bitRate
            // 
            this.bitRate.Text = "比特率";
            this.bitRate.Width = 80;
            // 
            // size
            // 
            this.size.Text = "大小";
            this.size.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.size.Width = 65;
            // 
            // time
            // 
            this.time.Text = "时长";
            this.time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.time.Width = 100;
            // 
            // source
            // 
            this.source.Text = "来源";
            this.source.Width = 150;
            // 
            // lastPageBtn
            // 
            this.lastPageBtn.BackColor = System.Drawing.Color.Transparent;
            this.lastPageBtn.BaseColor = System.Drawing.Color.LightBlue;
            this.lastPageBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.lastPageBtn.DownBack = null;
            this.lastPageBtn.DownBaseColor = System.Drawing.Color.Gainsboro;
            this.lastPageBtn.Enabled = false;
            this.lastPageBtn.GlowColor = System.Drawing.Color.LightBlue;
            this.lastPageBtn.Location = new System.Drawing.Point(670, 536);
            this.lastPageBtn.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.lastPageBtn.MouseBack = null;
            this.lastPageBtn.MouseBaseColor = System.Drawing.Color.LightBlue;
            this.lastPageBtn.Name = "lastPageBtn";
            this.lastPageBtn.NormlBack = null;
            this.lastPageBtn.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.lastPageBtn.Size = new System.Drawing.Size(60, 26);
            this.lastPageBtn.TabIndex = 15;
            this.lastPageBtn.Text = "上一页";
            this.lastPageBtn.UseVisualStyleBackColor = false;
            this.lastPageBtn.Click += new System.EventHandler(this.lastPageBtn_Click);
            // 
            // nextPageBtn
            // 
            this.nextPageBtn.BackColor = System.Drawing.Color.Transparent;
            this.nextPageBtn.BaseColor = System.Drawing.Color.LightBlue;
            this.nextPageBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.nextPageBtn.DownBack = null;
            this.nextPageBtn.DownBaseColor = System.Drawing.Color.Gainsboro;
            this.nextPageBtn.Enabled = false;
            this.nextPageBtn.GlowColor = System.Drawing.Color.LightBlue;
            this.nextPageBtn.Location = new System.Drawing.Point(734, 536);
            this.nextPageBtn.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.nextPageBtn.MouseBack = null;
            this.nextPageBtn.MouseBaseColor = System.Drawing.Color.LightBlue;
            this.nextPageBtn.Name = "nextPageBtn";
            this.nextPageBtn.NormlBack = null;
            this.nextPageBtn.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.nextPageBtn.Size = new System.Drawing.Size(60, 26);
            this.nextPageBtn.TabIndex = 16;
            this.nextPageBtn.Text = "下一页";
            this.nextPageBtn.UseVisualStyleBackColor = false;
            this.nextPageBtn.Click += new System.EventHandler(this.nextPageBtn_Click);
            // 
            // pageNum
            // 
            this.pageNum.AutoSize = true;
            this.pageNum.Location = new System.Drawing.Point(359, 541);
            this.pageNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pageNum.Name = "pageNum";
            this.pageNum.Size = new System.Drawing.Size(0, 17);
            this.pageNum.TabIndex = 17;
            // 
            // tblSearch
            // 
            this.tblSearch.Controls.Add(this.tabKeyword);
            this.tblSearch.Controls.Add(this.tabPage2);
            this.tblSearch.Location = new System.Drawing.Point(7, 17);
            this.tblSearch.Name = "tblSearch";
            this.tblSearch.SelectedIndex = 0;
            this.tblSearch.Size = new System.Drawing.Size(783, 67);
            this.tblSearch.TabIndex = 18;
            // 
            // tabKeyword
            // 
            this.tabKeyword.Controls.Add(this.txtSearchBox);
            this.tabKeyword.Controls.Add(this.searchBtn);
            this.tabKeyword.Location = new System.Drawing.Point(4, 26);
            this.tabKeyword.Name = "tabKeyword";
            this.tabKeyword.Padding = new System.Windows.Forms.Padding(3);
            this.tabKeyword.Size = new System.Drawing.Size(775, 37);
            this.tabKeyword.TabIndex = 0;
            this.tabKeyword.Text = "歌曲搜索";
            this.tabKeyword.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtSongListUrl);
            this.tabPage2.Controls.Add(this.btnGetSongList);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(775, 37);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "歌单&专辑&歌手歌曲";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtSongListUrl
            // 
            this.txtSongListUrl.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSongListUrl.Location = new System.Drawing.Point(7, 7);
            this.txtSongListUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtSongListUrl.Name = "txtSongListUrl";
            this.txtSongListUrl.Size = new System.Drawing.Size(452, 26);
            this.txtSongListUrl.TabIndex = 10;
            this.txtSongListUrl.Text = "https://y.qq.com/n/yqq/singer/0025NhlN2yWrP4.html";
            // 
            // btnGetSongList
            // 
            this.btnGetSongList.BackColor = System.Drawing.Color.Transparent;
            this.btnGetSongList.BaseColor = System.Drawing.Color.LightBlue;
            this.btnGetSongList.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnGetSongList.DownBack = null;
            this.btnGetSongList.DownBaseColor = System.Drawing.Color.Gainsboro;
            this.btnGetSongList.GlowColor = System.Drawing.Color.LightBlue;
            this.btnGetSongList.Location = new System.Drawing.Point(474, 8);
            this.btnGetSongList.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnGetSongList.MouseBack = null;
            this.btnGetSongList.MouseBaseColor = System.Drawing.Color.LightBlue;
            this.btnGetSongList.Name = "btnGetSongList";
            this.btnGetSongList.NormlBack = null;
            this.btnGetSongList.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnGetSongList.Size = new System.Drawing.Size(107, 26);
            this.btnGetSongList.TabIndex = 11;
            this.btnGetSongList.Text = "获取歌曲列表";
            this.btnGetSongList.UseVisualStyleBackColor = false;
            this.btnGetSongList.Click += new System.EventHandler(this.btnGetSongList_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(4, 541);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "下载码率：";
            // 
            // cbRate
            // 
            this.cbRate.FormattingEnabled = true;
            this.cbRate.Items.AddRange(new object[] {
            "128",
            "320",
            "flac"});
            this.cbRate.Location = new System.Drawing.Point(77, 541);
            this.cbRate.Name = "cbRate";
            this.cbRate.Size = new System.Drawing.Size(121, 25);
            this.cbRate.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 545);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(224, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "如果最大音质获取出错则自动转其他音质";
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.AutoSize = true;
            this.cbSelectAll.Location = new System.Drawing.Point(461, 543);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(51, 21);
            this.cbSelectAll.TabIndex = 22;
            this.cbSelectAll.Text = "全选";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CaptionHeight = 20;
            this.ClientSize = new System.Drawing.Size(800, 639);
            this.CloseBoxSize = new System.Drawing.Size(40, 20);
            this.Controls.Add(this.cbSelectAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tblSearch);
            this.Controls.Add(this.pageNum);
            this.Controls.Add(this.nextPageBtn);
            this.Controls.Add(this.lastPageBtn);
            this.Controls.Add(this.resultListView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pathBtn);
            this.Controls.Add(this.downBtn);
            this.Controls.Add(this.txtDownloadPath);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 639);
            this.MinimumSize = new System.Drawing.Size(800, 639);
            this.MiniSize = new System.Drawing.Size(40, 20);
            this.Name = "Form1";
            this.Radius = 10;
            this.ShowDrawIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XMusic 音乐下载器";
            this.TitleCenter = true;
            this.TitleSuitColor = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tblSearch.ResumeLayout(false);
            this.tabKeyword.ResumeLayout(false);
            this.tabKeyword.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearchBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDownloadPath;
        private System.Windows.Forms.Timer timer1;
        private CCWin.SkinControl.SkinButton searchBtn;
        private CCWin.SkinControl.SkinButton downBtn;
        private CCWin.SkinControl.SkinButton pathBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ListView resultListView;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader bitRate;
        private System.Windows.Forms.ColumnHeader singer;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.ColumnHeader size;
        private CCWin.SkinControl.SkinButton lastPageBtn;
        private CCWin.SkinControl.SkinButton nextPageBtn;
        private System.Windows.Forms.Label pageNum;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ColumnHeader source;
        private System.Windows.Forms.TabControl tblSearch;
        private System.Windows.Forms.TabPage tabKeyword;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtSongListUrl;
        private CCWin.SkinControl.SkinButton btnGetSongList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbSelectAll;
    }
}

