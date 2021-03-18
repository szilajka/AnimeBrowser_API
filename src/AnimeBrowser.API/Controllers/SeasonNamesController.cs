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
    [ApiController]
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    public class SeasonNamesController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<SeasonNamesController>();
        private readonly ISeasonNameCreation seasonNameCreationHandler;
        private readonly ISeasonNameEditing seasonNameEditingHandler;
        private readonly ISeasonNameDelete seasonNameDeleteHandler;

        public SeasonNamesController(ISeasonNameCreation seasonNameCreationHandler, ISeasonNameEditing seasonNameEditingHandler, ISeasonNameDelete seasonNameDeleteHandler)
        {
            this.seasonNameCreationHandler = seasonNameCreationHandler;
            this.seasonNameEditingHandler = seasonNameEditingHandler;
            this.seasonNameDeleteHandler = seasonNameDeleteHandler;
        }

        [HttpPost]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Create([FromBody] SeasonNameCreationRequestModel seasonNameRequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(seasonNameRequestModel)}: [{seasonNameRequestModel}].");

                var createdSeasonName = await seasonNameCreationHandler.CreateSeasonName(seasonNameRequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdSeasonName}].");
                return Created($"{ControllerHelper.SEASON_NAMES_CONTROLLER_NAME}/{createdSeasonName.Id}", createdSeasonName);
            }
            catch (EmptyObjectException<SeasonNameCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
            }
            catch (NotFoundObjectException<Season> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                return BadRequest(notFoundEx.Error);
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                return BadRequest(valEx.Errors);
            }
            catch (AlreadyExistingObjectException<SeasonName> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                return BadRequest(ex.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}")]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] SeasonNameEditingRequestModel seasonNameRequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(id)}: [{id}], {nameof(seasonNameRequestModel)}: [{seasonNameRequestModel}].");

                var updatedSeasonName = await seasonNameEditingHandler.EditSeasonName(id, seasonNameRequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished. result: [{updatedSeasonName}].");

                return Ok(updatedSeasonName);
            }
            catch (MismatchingIdException misEx)
            {
                logger.Warning(misEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{misEx.Message}].");
                return BadRequest(misEx.Error);
            }
            catch (NotFoundObjectException<SeasonName> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                return NotFound(ex.Error);
            }
            catch (NotFoundObjectException<Season> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                return NotFound(ex.Error);
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                return BadRequest(valEx.Errors);
            }
            catch (AlreadyExistingObjectException<SeasonName> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                return BadRequest(ex.Error);
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

                await seasonNameDeleteHandler.DeleteSeasonName(id);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished.");
                return Ok();
            }
            catch (NotExistingIdException notExistingEx)
            {
                logger.Warning(notExistingEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{notExistingEx.Message}].");
                return NotFound(notExistingEx.Error);
            }
            catch (NotFoundObjectException<SeasonName> nfoEx)
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
