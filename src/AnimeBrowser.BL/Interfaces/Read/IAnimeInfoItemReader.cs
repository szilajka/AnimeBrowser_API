﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Read
{
    public interface IAnimeInfoItemReader
    {
        Task Read(Dictionary<string, string> filter, int page = 0, int size = 0);
    }
}
