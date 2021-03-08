using AnimeBrowser.API.Helpers;
using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
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
    public class AnimeInfoNamesController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoNamesController>();
        private readonly IAnimeInfoNameCreation animeInfoNameCreateHandler;

        public AnimeInfoNamesController(IAnimeInfoNameCreation animeInfoNameCreateHandler)
        {
            this.animeInfoNameCreateHandler = animeInfoNameCreateHandler;
        }


        [HttpPost]
        [Authorize("AnimeInfoAdmin")]
        public async Task<IActionResult> Create([FromBody] AnimeInfoNameCreationRequestModel animeInfoName)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(animeInfoName)}: [{animeInfoName}].");

                var createdAnimeInfoName = await animeInfoNameCreateHandler.CreateAnimeInfoName(animeInfoName);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdAnimeInfoName}].");
                return Created($"{ControllerHelper.ANIME_INFO_NAMES_CONTROLLER_NAME}/{createdAnimeInfoName.Id}", createdAnimeInfoName);
            }
            catch (EmptyObjectException<AnimeInfoNameCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Empty request model [{nameof(animeInfoName)}] in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
            }
            catch (NotFoundObjectException<AnimeInfo> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Not found {nameof(AnimeInfo)} in database in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return BadRequest(notFoundEx.Error);
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
    }
}
