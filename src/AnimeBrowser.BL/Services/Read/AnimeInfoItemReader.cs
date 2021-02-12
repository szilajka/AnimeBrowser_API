using AnimeBrowser.BL.Interfaces.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
