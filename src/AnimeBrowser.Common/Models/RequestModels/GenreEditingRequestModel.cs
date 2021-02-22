﻿using AnimeBrowser.Common.Models.BaseModels;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.RequestModels
{
    public class GenreEditingRequestModel : GenreRequestModel
    {
        public GenreEditingRequestModel(long id, string genreName = "", string description = "")
            : base(genreName: genreName, description: description)
        {
            this.Id = id;
        }

        public long Id { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}