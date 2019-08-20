# XMusicDownloader

XMusicDownloader，一款 支持从百度、网易、qq和酷狗等音乐网站搜索并下载歌曲的程序。

## 更新说明
- 2019.8 V1.1.0 发布，支持歌单、专辑、歌手歌曲下载，支持无损下载
   +支持歌单、专辑、歌手歌曲下载
   +支持无损下载


缘起：

一直用网易音乐听歌，但是诸如李健、周杰伦的不少歌曲，网易都没有版权，要从QQ等音乐去下载，因此一直想写一个小程序，可以从其他音乐网站下载相关歌曲，趁放假，花了几小时做了这样一个程序。

BTW: 之前写过一个[从酷狗和网易音乐提取缓存文件的程序](https://github.com/jadepeng/musicDecryptor)，感兴趣的可以查看。

## 功能

* 聚合搜索多家音乐网站
* 支持音乐批量下载
* 搜索结果综合排序
* 可以编写Provider程序，支持其他音乐网站

实现IMusicProvider即可，主要是搜索和获取下载链接的方法。


``` csharp?linenums
    public interface IMusicProvider
    {
        string Name { get; }

        string getDownloadUrl(Song song);
        List<Song> SearchSongs(string keyword, int page, int pageSize);
    }
```


## 界面截图

![预览](https://www.github.com/jadepeng/blogpic/raw/master/images/2019/1-25/1548431781568.png)

## 下载程序

https://github.com/jadepeng/XMusicDownloader/releases

## 实现方案介绍

### 定义song实体

``` javascript
public class Song
    {
        public string id { get; set; }
        public string name { get; set; }
        public string singer { get; set; }
        public string album { get; set; }
        public string source { get; set; }
        public double duration { get; set; }
        public double size { get; set; }
        public string url { get; set; }
        public int rate { get; set; }
        public int index { get; set; }

        public string getFileName()
        {
            return singer + "-" + name + ".mp3";
        }

        public string getMergedKey()
        {
            return singer.Replace(" ", "") + name.Replace(" ", "");
        }
    }
```

### 封装各个音乐网站

抽象为MusicProvider，音乐提供方:)，定义Name为名称，SearchSongs搜索歌曲，getDownloadUrl获取音乐下载地址。

``` c#

    public interface IMusicProvider
    {
        string Name { get; }

        string getDownloadUrl(Song song);
        List<Song> SearchSongs(string keyword, int page, int pageSize);
    }
```

然后就是依次实现百度、网易等音乐网站，以QQ为例。


``` csharp
 public class QQProvider : IMusicProvider
    {
        static HttpConfig DEFAULT_CONFIG = new HttpConfig
        {
            Referer = "http://m.y.qq.com",

        };

        public string Name { get; } = "QQ";

        static string[] prefixes = new string[] { "M800", "M500", "C400" };

        public List<Song> SearchSongs(string keyword,int page,int pageSize)
        {
            var searchResult = HttpHelper.GET(string.Format("http://c.y.qq.com/soso/fcgi-bin/search_for_qq_cp?w={0}&format=json&p={1}&n={2}", keyword, page,pageSize), DEFAULT_CONFIG);
            var searchResultJson = JsonParser.Deserialize(searchResult).data.song;
            var result = new List<Song>();

            var index = 1;
            foreach(var songItem in searchResultJson.list)
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

            var key = JsonParser.Deserialize(HttpHelper.GET(string.Format("http://base.music.qq.com/fcgi-bin/fcg_musicexpress.fcg?guid={0}&format=json&json=3",guid), DEFAULT_CONFIG)).key;
            foreach(var prefix in prefixes)
            {
               
                var musicUrl = string.Format("http://dl.stream.qqmusic.qq.com/{0}{1}.mp3?vkey={2}&guid={3}&fromtag=1", prefix, song.id, key, guid);
                if (HttpHelper.GetUrlContentLength(musicUrl) > 0)
                {
                    return musicUrl;
                }
            }

            return null;

        }
    
    }
```

- 搜索调用`http://c.y.qq.com/soso/fcgi-bin/search_for_qq_cp?w={0}&format=json&p={1}&n={2}`接口，获取下载地址调用`http://base.music.qq.com/fcgi-bin/fcg_musicexpress.fcg?guid={0}&format=json&json=3`,然后再组合。

### 聚合搜索

设计一个MusicProviders，加载所有的IMusicProvider，提供一个SearchSongs方法，并发调用各个网站的搜索，然后merge到一起。

``` csharp

  public List<MergedSong> SearchSongs(string keyword, int page, int pageSize)
        {
            var songs = new List<Song>();
            Providers.AsParallel().ForAll(provider =>
            {
                var currentSongs = provider.SearchSongs(keyword, page, pageSize);
                songs.AddRange(currentSongs);
            });

            // merge

            return songs.GroupBy(s => s.getMergedKey()).Select(g => new MergedSong(g.ToList())).OrderByDescending(s => s.score).ToList();
        }
```

关于merge，核心就是将相同的歌曲合并到一起，我们暂且认为歌手+歌曲名相同的为同一首歌曲：

``` csharp?linenums
   public string getMergedKey()
        {
            return singer.Replace(" ", "") + name.Replace(" ", "");
        }
		
```

因此按megekey分组，就能实现聚合。我们设计一个`MergedSong`来包裹。


``` csharp
public class MergedSong
    {
        public List<Song> items
        {
            get; set;
        }

        public MergedSong(List<Song> items)
        {
            this.items = items;
        }

        public string name
        {
            get
            {
                return this.items[0].name;
            }
        }
        public string singer
        {
            get
            {
                return this.items[0].singer;
            }
        }
        public string album
        {
            get
            {
                return this.items[0].album;
            }
        }

        public string source
        {
            get
            {
                return string.Join(",", this.items.Select(i => i.source).ToArray());
            }
        }


        public double duration
        {
            get
            {
                return this.items[0].duration;
            }
        }

        public double size
        {
            get
            {
                return this.items[0].size;
            }
        }

        public double rate
        {
            get
            {
                return this.items[0].rate;
            }
        }


        public double score
        {
            get
            {
                // 投票+排序加权  (各50%）
                return this.items.Count / (MusicProviders.Instance.Providers.Count - 1) + (20 - this.items.Average(i => i.index)) / 20;
            }
        }

    }
```

MergedSong的核心是定义了一个score，我们通过投票+搜索结果排序，用来决定合并结果的排序。

### 下载

下载主要是通过provider获取真实url，然后下载即可。

``` csharp?linenums
public class SongItemDownloader
    {
        MusicProviders musicProviders;
        string target;
        MergedSong song;

        public event DownloadFinishEvent DownloadFinish;

        public SongItemDownloader(MusicProviders musicProviders, string target, MergedSong song)
        {
            this.musicProviders = musicProviders;
            this.target = target;
            this.song = song;
        }

        public long totalBytes;

        public long bytesReceived;

        public double ReceiveProgress;


        public double receiveSpeed;

        DateTime lastTime = DateTime.Now;

        public void Download()
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            new Thread(() =>
            {
                // 多来源，防止单个来源出错
                foreach (var item in song.items)
                {
                    try
                    {
                        client.DownloadFile(musicProviders.getDownloadUrl(item), target + "\\" + item.getFileName());
                        DownloadFinish?.Invoke(this, this);
                        break;

                    }
                    catch
                    {
                    }
                }

            }).Start();
        }

        private void Client_DownloadProgressChanged(object sender, DownloadEventArgs e)
        {
            this.bytesReceived = e.bytesReceived;
            this.totalBytes = e.totalBytes;
            this.receiveSpeed = e.receiveSpeed;
            this.ReceiveProgress = e.ReceiveProgress;
        }
    }
```


## 参考

- 程序界面，使用了https://github.com/Gsangu/KugouDownloader代码
- 搜索和下载方案参考 https://github.com/0xHJK/music-dl
