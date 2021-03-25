﻿using AnimeBrowser.Common.Models.BaseModels.MainModels;

namespace AnimeBrowser.Common.Models.ResponseModels.MainModels
{
    public class AnimeInfoCreationResponseModel : AnimeInfoResponseModel
    {
        public AnimeInfoCreationResponseModel(long id, string title = "", string description = "", bool isNsfw = false, bool isActive = true)
            : base(id: id, title: title, description: description, isNsfw: isNsfw, isActive: isActive)
        {
        }
    }
}
