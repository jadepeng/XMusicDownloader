using System.Collections.Generic;
using XMusicDownloader.Domain;

namespace XMusicDownloader.Provider
{
    public interface IMusicProvider
    {
        string Name { get; }

        string getDownloadUrl(Song song);
        List<Song> SearchSongs(string keyword, int page, int pageSize);

        // 歌单
        bool Support(string url);
        List<Song> GetSongList(string url);

        /// <summary>
        /// 获取下载地址
        /// </summary>
        /// <param name="id">歌曲id</param>
        /// <param name="rate">码率，音质 如果最大音质获取出错则自动转其他音质	</param>
        /// <returns>歌曲下载地址</returns>
        string getDownloadUrl(string id, string rate);
    }
}