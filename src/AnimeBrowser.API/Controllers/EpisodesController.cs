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
    [ApiController]
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    public class EpisodesController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<EpisodesController>();
        private readonly IEpisodeCreation episodeCreationHandler;
        private readonly IEpisodeEditing episodeEditingHandler;

        public EpisodesController(IEpisodeCreation episodeCreationHandler, IEpisodeEditing episodeEditingHandler)
        {
            this.episodeCreationHandler = episodeCreationHandler;
            this.episodeEditingHandler = episodeEditingHandler;
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
            catch (AlreadyExistingObjectException<Episode> alreadyExistingEx)
            {
                logger.Warning(alreadyExistingEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{alreadyExistingEx.Message}].");
                return BadRequest(alreadyExistingEx.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}")]
        [Authorize("EpisodeAdmin")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] EpisodeEditingRequestModel episodeRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. requestModel: [{episodeRequestModel}].");

                var updatedEpisode = await episodeEditingHandler.EditEpisode(id, episodeRequestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result.Id: [{updatedEpisode?.Id}].");

                return Ok(updatedEpisode);
            }
            catch (MismatchingIdException misEx)
            {
                logger.Warning(misEx, $"Mismatching Id error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{misEx.Message}].");
                return BadRequest(misEx.Error);
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                return BadRequest(valEx.Errors);
            }
            catch (NotFoundObjectException<EpisodeEditingRequestModel> ex)
            {
                logger.Warning(ex, $"Not found object error in {MethodNameHelper.GetCurrentMethodName()}. Returns 404 - Not Found. Message: [{ex.Message}].");
                return NotFound(id);
            }
            catch (NotFoundObjectException<AnimeInfo> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Not found {nameof(AnimeInfo)} in database in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return BadRequest(notFoundEx.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
