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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.API.Controllers
{
    [ApiController]
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    public class SeasonGenresController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<SeasonGenresController>();
        private readonly ISeasonGenreCreation seasonGenreCreationHandler;
        private readonly ISeasonGenreDelete seasonGenreDeleteHandler;

        public SeasonGenresController(ISeasonGenreCreation seasonGenreCreationHandler, ISeasonGenreDelete seasonGenreDeleteHandler)
        {
            this.seasonGenreCreationHandler = seasonGenreCreationHandler;
            this.seasonGenreDeleteHandler = seasonGenreDeleteHandler;
        }

        [HttpPost("batchCreate")]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> CreateGenreMappings([FromBody] IList<SeasonGenreCreationRequestModel> requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(requestModel)}: [{string.Join(", ", requestModel ?? new List<SeasonGenreCreationRequestModel>())}].");

                var createdSeasonGenreMappings = await seasonGenreCreationHandler.CreateSeasonGenres(requestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");

                return Ok(createdSeasonGenreMappings);
            }
            catch (EmptyObjectException<SeasonGenreCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
            }
            catch (NotExistingIdException notExistingEx)
            {
                logger.Warning(notExistingEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{notExistingEx.Message}].");
                return NotFound(notExistingEx.Error);
            }
            catch (NotFoundObjectException<Season> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return BadRequest(notFoundEx.Error);
            }
            catch (NotFoundObjectException<Genre> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return BadRequest(notFoundEx.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("batchDelete")]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> DeleteGenreMappings([FromBody] IList<long> requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(requestModel)}: [{string.Join(", ", requestModel ?? new List<long>())}].");

                await seasonGenreDeleteHandler.DeleteSeasonGenres(requestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");

                return Ok();
            }
            catch (NotExistingIdException notExistingEx)
            {
                logger.Warning(notExistingEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{notExistingEx.Message}].");
                return NotFound(notExistingEx.Error);
            }
            catch (NotFoundObjectException<SeasonGenre> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return NotFound(notFoundEx.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
