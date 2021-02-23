using AnimeBrowser.API.Helpers;
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
    [ApiController]
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    public class EpisodesController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<EpisodesController>();
        private readonly IEpisodeCreation episodeCreationHandler;

        public EpisodesController(IEpisodeCreation episodeCreationHandler)
        {
            this.episodeCreationHandler = episodeCreationHandler;
        }

        [HttpPost]
        [Authorize("EpisodeAdmin")]
        public async Task<IActionResult> Create([FromBody] EpisodeCreationRequestModel episodeRequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(episodeRequestModel)}: [{episodeRequestModel}].");

                var createdEpisode = await episodeCreationHandler.CreateEpisode(episodeRequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdEpisode }].");
                return Created($"{ControllerHelper.EPISODES_CONTROLLER_NAME}/{createdEpisode.Id}", createdEpisode);
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                return BadRequest(mismatchEx.Error);
            }
            catch (EmptyObjectException<EpisodeCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Empty request model [{nameof(episodeRequestModel)}] in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
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
