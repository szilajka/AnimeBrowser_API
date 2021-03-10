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

        public SeasonNamesController(ISeasonNameCreation seasonNameCreationHandler)
        {
            this.seasonNameCreationHandler = seasonNameCreationHandler;
        }

        [HttpPost]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Create([FromBody] SeasonNameCreationRequestModel seasonName)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(seasonName)}: [{seasonName}].");

                var createdSeasonName = await seasonNameCreationHandler.CreateSeasonName(seasonName);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdSeasonName}].");
                return Created($"{ControllerHelper.SEASON_NAMES_CONTROLLER_NAME}/{createdSeasonName.Id}", createdSeasonName);
            }
            catch (EmptyObjectException<SeasonNameCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Empty request model [{nameof(seasonName)}] in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
            }
            catch (NotFoundObjectException<Season> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Not found {nameof(Season)} in database in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
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
    }
}
