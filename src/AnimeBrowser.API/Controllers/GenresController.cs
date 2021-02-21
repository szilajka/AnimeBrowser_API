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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.API.Controllers
{
    [ApiController]
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    public class GenresController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<GenresController>();
        private readonly IGenreCreation genreCreationHandler;
        private readonly IGenreEditing genreEditingHandler;

        public GenresController(IGenreCreation genreCreation, IGenreEditing genreEditing)
        {
            this.genreCreationHandler = genreCreation;
            this.genreEditingHandler = genreEditing;
        }

        [HttpPost]
        [Authorize("GenreAdmin")]
        public async Task<IActionResult> Create([FromBody] GenreCreationRequestModel requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. requestModel: [{requestModel}].");

                var createdGenre = await genreCreationHandler.CreateGenre(requestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result.Id: [{createdGenre?.Id}].");

                return Created($"{ControllerHelper.GENRES_CONTROLLER_NAME}/{createdGenre.Id}", createdGenre);
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
        [Authorize("GenreAdmin")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] GenreEditingRequestModel requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. id: [{id}], requestModel: [{requestModel}].");

                var updatedGenre = await genreEditingHandler.EditGenre(id, requestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result.Id: [{updatedGenre?.Id}].");

                return Ok(updatedGenre);
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                return BadRequest(valEx.Errors);
            }
            catch (NotFoundObjectException<GenreEditingRequestModel> ex)
            {
                logger.Warning(ex, $"Not found object error in {MethodNameHelper.GetCurrentMethodName()}. Returns 404 - Not Found. Message: [{ex.Message}].");
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
