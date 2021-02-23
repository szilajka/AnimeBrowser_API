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
    public class SeasonsController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<SeasonsController>();
        private readonly ISeasonCreation seasonCreationHandler;
        private readonly ISeasonEditing seasonEditingHandler;
        private readonly ISeasonDelete seasonDeleteHandler;

        public SeasonsController(ISeasonCreation seasonCreationHandler, ISeasonEditing seasonEditingHandler, ISeasonDelete seasonDeleteHandler)
        {
            this.seasonCreationHandler = seasonCreationHandler;
            this.seasonEditingHandler = seasonEditingHandler;
            this.seasonDeleteHandler = seasonDeleteHandler;
        }

        [HttpPost]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Create([FromBody] SeasonCreationRequestModel requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. requestModel: [{requestModel}].");

                var createdSeason = await seasonCreationHandler.CreateSeason(requestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result.Id: [{createdSeason.Id}].");

                return Created($"{ControllerHelper.SEASONS_CONTROLLER_NAME}/{createdSeason.Id}", createdSeason);
            }
            catch (EmptyObjectException<SeasonCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Empty request model in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
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

        [HttpPatch("{id}")]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] SeasonEditingRequestModel requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. requestModel: [{requestModel}].");

                var updatedSeason = await seasonEditingHandler.EditSeason(id, requestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result.Id: [{updatedSeason?.Id}].");

                return Ok(updatedSeason);
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
            catch (NotFoundObjectException<SeasonEditingRequestModel> ex)
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

        [HttpDelete("{id}")]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(id)}: [{id}].");

                await seasonDeleteHandler.DeleteSeason(id);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished.");
                return Ok();
            }
            catch (NotExistingIdException notExistingEx)
            {
                logger.Warning(notExistingEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{notExistingEx.Message}].");
                return NotFound(notExistingEx.Error);
            }
            catch (NotFoundObjectException<Season> nfoEx)
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
