using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AnimeInfoController : ControllerBase
    {
        private readonly IAnimeInfoCreation animeInfoCreateHandler;
        private readonly IAnimeInfoEditor animeInfoEditorHandler;
        private readonly ILogger logger = Log.ForContext<AnimeInfoController>();

        public AnimeInfoController(IAnimeInfoCreation animeInfoCreateHandler, IAnimeInfoEditor animeInfoEditorHandler)
        {
            this.animeInfoCreateHandler = animeInfoCreateHandler;
            this.animeInfoEditorHandler = animeInfoEditorHandler;
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
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. animeInfo: [{animeInfo}].");

                var createdAnimeInfo = await animeInfoCreateHandler.CreateAnimeInfo(animeInfo);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdAnimeInfo }].");
                return Created($"api/v1/animeInfo/{createdAnimeInfo.Id}", createdAnimeInfo);
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                return BadRequest(valEx.Errors);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}")]
        [Authorize("AnimeInfoAdmin")]
        public async Task<IActionResult> Edit(long id, [FromBody] AnimeInfoEditingRequestModel updateModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. updateModel: [{updateModel}].");

                var updatedAnimeInfo = await animeInfoEditorHandler.EditAnimeInfo(id, updateModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished. result: [{updatedAnimeInfo}].");

                return Ok(updatedAnimeInfo);
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                return BadRequest(valEx.Errors);
            }
            catch (NotFoundObjectException<AnimeInfoEditingRequestModel> ex)
            {
                logger.Warning(ex, $"Not found object error in {MethodNameHelper.GetCurrentMethodName()}. Returns 404 - Not Found. Message: [{ex.Message}].");
                return NotFound(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
