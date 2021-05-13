using AnimeBrowser.API.Helpers;
using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
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
        private readonly ISeasonInactivation seasonInactivationHandler;

        public SeasonsController(ISeasonCreation seasonCreationHandler, ISeasonEditing seasonEditingHandler, ISeasonDelete seasonDeleteHandler,
            ISeasonInactivation seasonInactivationHandler)
        {
            this.seasonCreationHandler = seasonCreationHandler;
            this.seasonEditingHandler = seasonEditingHandler;
            this.seasonDeleteHandler = seasonDeleteHandler;
            this.seasonInactivationHandler = seasonInactivationHandler;
        }

        [HttpPost]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Create([FromBody] SeasonCreationRequestModel seasonRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonRequestModel)}: [{seasonRequestModel}].");

                var createdSeason = await seasonCreationHandler.CreateSeason(seasonRequestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(createdSeason)}.{nameof(createdSeason.Id)}: [{createdSeason.Id}].");

                return Created($"{ControllerHelper.SEASONS_CONTROLLER_NAME}/{createdSeason.Id}", createdSeason);
            }
            catch (EmptyObjectException<SeasonCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                return BadRequest(valEx.Errors);
            }
            catch (AlreadyExistingObjectException<Season> alreadyExistingEx)
            {
                logger.Warning(alreadyExistingEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{alreadyExistingEx.Message}].");
                return BadRequest(alreadyExistingEx.Error);
            }
            catch (NotFoundObjectException<AnimeInfo> notFoundEx)
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

        [HttpPatch("{id}")]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] SeasonEditingRequestModel seasonRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonRequestModel)}: [{seasonRequestModel}].");

                var updatedSeason = await seasonEditingHandler.EditSeason(id, seasonRequestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.  {nameof(updatedSeason)}.{nameof(updatedSeason.Id)}: [{updatedSeason?.Id}].");

                return Ok(updatedSeason);
            }
            catch (MismatchingIdException misEx)
            {
                logger.Warning(misEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{misEx.Message}].");
                return BadRequest(misEx.Error);
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                return BadRequest(valEx.Errors);
            }
            catch (AlreadyExistingObjectException<Season> alreadyExistingEx)
            {
                logger.Warning(alreadyExistingEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{alreadyExistingEx.Message}].");
                return BadRequest(alreadyExistingEx.Error);
            }
            catch (NotFoundObjectException<Season> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                return NotFound(id);
            }
            catch (NotFoundObjectException<AnimeInfo> notFoundEx)
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

        [HttpPatch("{id}/inactivate")]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Inactivate([FromRoute] long id)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

                var updatedSeason = await seasonInactivationHandler.Inactivate(id);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");

                return Ok(updatedSeason);
            }
            catch (NotExistingIdException idEx)
            {
                logger.Warning(idEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{idEx.Message}].");
                return BadRequest(idEx.Error);
            }
            catch (NotFoundObjectException<Season> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
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
