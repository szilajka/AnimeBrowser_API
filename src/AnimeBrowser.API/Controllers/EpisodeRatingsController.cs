using AnimeBrowser.API.Helpers;
using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Entities.Identity;
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
    public class EpisodeRatingsController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<EpisodeRatingsController>();
        private readonly IEpisodeRatingCreation episodeRatingCreationHandler;
        private readonly IEpisodeRatingEditing episodeRatingEditingHandler;
        private readonly IEpisodeRatingDelete episodeRatingDeleteHandler;

        public EpisodeRatingsController(IEpisodeRatingCreation episodeRatingCreationHandler, IEpisodeRatingEditing episodeRatingEditingHandler, IEpisodeRatingDelete episodeRatingDeleteHandler)
        {
            this.episodeRatingCreationHandler = episodeRatingCreationHandler;
            this.episodeRatingEditingHandler = episodeRatingEditingHandler;
            this.episodeRatingDeleteHandler = episodeRatingDeleteHandler;
        }

        [HttpPost]
        [Authorize("RatingWrite")]
        public async Task<IActionResult> Create([FromBody] EpisodeRatingCreationRequestModel episodeRatingRequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(episodeRatingRequestModel)}: [{episodeRatingRequestModel}].");
                //TODO: Check if the user is the same, as the requestor (HttpContext.UserId == rm.UserId?)
                var createdEpisodeRating = await episodeRatingCreationHandler.CreateEpisodeRating(episodeRatingRequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdEpisodeRating}].");
                return Created($"{ControllerHelper.EPISODE_RATINGS_CONTROLLER_NAME}/{createdEpisodeRating.Id}", createdEpisodeRating);
            }
            catch (EmptyObjectException<EpisodeRatingCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
            }
            catch (NotFoundObjectException<Episode> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return BadRequest(notFoundEx.Error);
            }
            catch (NotFoundObjectException<User> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return BadRequest(notFoundEx.Error);
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                return BadRequest(valEx.Errors);
            }
            catch (AlreadyExistingObjectException<EpisodeRating> alreadyExistingEx)
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
        [Authorize("RatingWrite")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] EpisodeRatingEditingRequestModel episodeRatingRequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(episodeRatingRequestModel)}: [{episodeRatingRequestModel}].");
                //TODO: Check if the user is the same, as the requestor (HttpContext.UserId == rm.UserId?)
                var updatedEpisodeRating = await episodeRatingEditingHandler.EditEpisodeRating(id, episodeRatingRequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{updatedEpisodeRating}].");
                return Ok(updatedEpisodeRating);
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                return BadRequest(mismatchEx.Error);
            }
            catch (EmptyObjectException<EpisodeRatingEditingRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
            }
            catch (NotFoundObjectException<EpisodeRating> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return NotFound(notFoundEx.Error);
            }
            catch (NotFoundObjectException<Episode> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return BadRequest(notFoundEx.Error);
            }
            catch (NotFoundObjectException<User> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
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


        [HttpDelete("{id}")]
        [Authorize("RatingWrite")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(id)}: [{id}].");
                //TODO: Check if the user is the same, as the requestor (HttpContext.UserId == rm.UserId?)
                await episodeRatingDeleteHandler.DeleteEpisodeRating(id);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished.");
                return Ok();
            }
            catch (NotExistingIdException notExistingEx)
            {
                logger.Warning(notExistingEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{notExistingEx.Message}].");
                return NotFound(notExistingEx.Error);
            }
            catch (NotFoundObjectException<EpisodeRating> nfoEx)
            {
                logger.Warning(nfoEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{nfoEx.Message}].");
                return NotFound(nfoEx.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
