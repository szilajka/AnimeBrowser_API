using AnimeBrowser.BL.Interfaces.Read;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Read
{
    public class AnimeInfoItemReader : IAnimeInfoItemReader
    {
        public AnimeInfoItemReader()
        {

        }

        public async Task Read(Dictionary<string, string> filter, int page = 0, int size = 0)
        {

        }
    }
}
