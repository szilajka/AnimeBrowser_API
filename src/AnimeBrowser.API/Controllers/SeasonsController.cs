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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.API.Controllers
{
    [ApiController]
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    public class SeasonsController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<SeasonsController>();
        private readonly ISeasonCreation seasonCreationHandler;

        public SeasonsController(ISeasonCreation seasonCreationHandler)
        {
            this.seasonCreationHandler = seasonCreationHandler;
        }

        [HttpPost]
        [Authorize("SeasonAdmin")]
        public async Task<IActionResult> Create([FromBody] SeasonCreationRequestModel requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. requestModel: [{requestModel}].");

                var createdSeason = await seasonCreationHandler.CreateSeason(requestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result.Id: [{createdSeason?.Id}].");

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
    }
}
