using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeBrowser.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AnimeInfoController : ControllerBase
    {
        private readonly IAnimeInfoCreation animeInfoCreateHandler;

        public AnimeInfoController(IAnimeInfoCreation animeInfoCreateHandler)
        {
            this.animeInfoCreateHandler = animeInfoCreateHandler;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get(Dictionary<string, string> filter, int size = 20, int page = 0)
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(long id)
        //{

        //}


        [HttpPost]
        [Authorize("AnimeInfoAdmin")]
        public async Task<IActionResult> Create([FromBody] AnimeInfoCreationRequestModel animeInfo)
        {
            try
            {
                Log.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. animeInfo: [{animeInfo}]");

                var createdAnimeInfo = await animeInfoCreateHandler.CreateAnimeInfo(animeInfo);

                Log.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdAnimeInfo }].");
                return Ok(createdAnimeInfo);
                //return Ok();
            }
            catch (ValidationException valEx)
            {
                Log.Warning($"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}]");
                return BadRequest(valEx.Errors);
            }
        }

    }
}
