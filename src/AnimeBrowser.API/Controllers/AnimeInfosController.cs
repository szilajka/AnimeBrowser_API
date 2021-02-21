using AnimeBrowser.API.Helpers;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.API.Controllers
{
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    [ApiController]
    public class AnimeInfosController : ControllerBase
    {
        private readonly IAnimeInfoCreation animeInfoCreateHandler;
        private readonly IAnimeInfoEditing animeInfoEditorHandler;
        private readonly IAnimeInfoDelete animeInfoDeleteHandler;
        private readonly ILogger logger = Log.ForContext<AnimeInfosController>();

        public AnimeInfosController(IAnimeInfoCreation animeInfoCreateHandler, IAnimeInfoEditing animeInfoEditorHandler, IAnimeInfoDelete animeInfoDeleteHandler)
        {
            this.animeInfoCreateHandler = animeInfoCreateHandler;
            this.animeInfoEditorHandler = animeInfoEditorHandler;
            this.animeInfoDeleteHandler = animeInfoDeleteHandler;
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
                return Created($"{ControllerHelper.ANIME_INFOS_CONTROLLER_NAME}/{createdAnimeInfo.Id}", createdAnimeInfo);
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
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] AnimeInfoEditingRequestModel updateModel)
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

        [HttpDelete("{id}")]
        [Authorize("AnimeInfoAdmin")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. animeInfo.Id: [{id}].");

                await animeInfoDeleteHandler.DeleteAnimeInfo(id);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished.");
                return Ok();
            }
            catch (ArgumentOutOfRangeException arEx)
            {
                logger.Warning(arEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{arEx.Message}].");
                return NotFound(id);
            }
            catch (NotFoundObjectException<AnimeInfo> nfoEx)
            {
                logger.Warning(nfoEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{nfoEx.Message}].");
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
