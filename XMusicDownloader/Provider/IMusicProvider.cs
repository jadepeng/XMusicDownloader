using System.Collections.Generic;
using XMusicDownloader.Domain;

namespace XMusicDownloader.Provider
{
    public interface IMusicProvider
    {
        string Name { get; }

        string getDownloadUrl(Song song);
        List<Song> SearchSongs(string keyword, int page, int pageSize);
    }
}