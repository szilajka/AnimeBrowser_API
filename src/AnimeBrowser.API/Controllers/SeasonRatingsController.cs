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
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    [ApiController]
    public class SeasonRatingsController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<SeasonRatingsController>();
        private readonly ISeasonRatingCreation seasonRatingCreationHandler;
        private readonly ISeasonRatingEditing seasonRatingEditingHandler;

        public SeasonRatingsController(ISeasonRatingCreation seasonRatingCreationHandler, ISeasonRatingEditing seasonRatingEditingHandler)
        {
            this.seasonRatingCreationHandler = seasonRatingCreationHandler;
            this.seasonRatingEditingHandler = seasonRatingEditingHandler;
        }

        [HttpPost]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> CreateSeasonRating([FromBody] SeasonRatingCreationRequestModel seasonRatingrequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(seasonRatingrequestModel)}: [{seasonRatingrequestModel}].");

                var createdSeasonRating = await seasonRatingCreationHandler.CreateSeasonRating(seasonRatingrequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdSeasonRating}].");
                return Created($"{ControllerHelper.SEASON_RATINGS_CONTROLLER_NAME}/{createdSeasonRating.Id}", createdSeasonRating);
            }
            catch (EmptyObjectException<SeasonRatingCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Empty request model [{nameof(seasonRatingrequestModel)}] in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
            }
            catch (NotFoundObjectException<Season> notFoundEx)
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
            catch (AlreadyExistingObjectException<SeasonRating> alreadyExistingEx)
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
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> EditSeasonRating([FromRoute] long id, [FromBody] SeasonRatingEditingRequestModel seasonRatingRequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(seasonRatingRequestModel)}: [{seasonRatingRequestModel}].");

                var updatedSeasonRating = await seasonRatingEditingHandler.EditSeasonRating(id, seasonRatingRequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{updatedSeasonRating}].");
                return Ok(updatedSeasonRating);
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                return BadRequest(mismatchEx.Error);
            }
            catch (NotFoundObjectException<SeasonRating> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return BadRequest(notFoundEx.Error);
            }
            catch (NotFoundObjectException<Season> notFoundEx)
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

    }
}
