﻿using AnimeBrowser.Common.Models.BaseModels.MainModels;

namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public partial class GenreCreationRequestModel : GenreRequestModel
    {
        public GenreCreationRequestModel(string genreName = "", string description = "")
            : base(genreName: genreName, description: description)
        {
        }
    }
}
