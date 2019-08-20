using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XMusicDownloader.Domain;

namespace XMusicDownloader.Provider
{
    public class MusicProviders
    {

        public static MusicProviders Instance
        {
            get
            {
                return Holder.providers;
            }
        }

        public List<IMusicProvider> Providers
        {
            get; set;
        } = new List<IMusicProvider>();


        public void AddMusicProvider(IMusicProvider provider)
        {
            Providers.Add(provider);
            type2Provider.Add(provider.Name, provider);
        }

        Dictionary<string, IMusicProvider> type2Provider = new Dictionary<string, IMusicProvider>();

        public string Name => "MusicProviders";

        public string getDownloadUrl(Song song)
        {
            return type2Provider[song.source].getDownloadUrl(song);
        }

        public string getDownloadUrl(Song song,string rate)
        {
            return type2Provider[song.source].getDownloadUrl(song.id,rate);
        }

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


        public List<MergedSong> SearchSongsList(string url)
        {
            var songs = new List<Song>();
            Providers.AsParallel().ForAll(provider =>
            {
                if (provider.Support(url)) {
                    var currentSongs = provider.GetSongList(url);
                    songs.AddRange(currentSongs);
                }  
            });

            // merge

            return songs.GroupBy(s => s.getMergedKey()).Select(g => new MergedSong(g.ToList())).OrderByDescending(s => s.score).ToList();
        }



        static class Holder
        {
            public static MusicProviders providers = Load();

            /// <summary>
            /// 从当前Assembly加载
            /// </summary>
            /// <returns></returns>
            private static MusicProviders Load()
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                List<Type> hostTypes = new List<Type>();

                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.Name == "MusicProviders")
                    {
                        continue;
                    }
                    //确定type为类并且继承自(实现)IMyInstance
                    if (type.IsClass && typeof(IMusicProvider).IsAssignableFrom(type) && !type.IsAbstract)
                        hostTypes.Add(type);
                }

                MusicProviders musicProviders = new MusicProviders();
                foreach (var type in hostTypes)
                {
                    IMusicProvider instance = (IMusicProvider)Activator.CreateInstance(type);
                    musicProviders.AddMusicProvider(instance);
                }

                return musicProviders;
            }
        }
    }
}
