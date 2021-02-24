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
    public class GenresController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<GenresController>();
        private readonly IGenreCreation genreCreationHandler;
        private readonly IGenreEditing genreEditingHandler;
        private readonly IGenreDelete genreDeleteHandler;

        public GenresController(IGenreCreation genreCreation, IGenreEditing genreEditing, IGenreDelete genreDelete)
        {
            this.genreCreationHandler = genreCreation;
            this.genreEditingHandler = genreEditing;
            this.genreDeleteHandler = genreDelete;
        }

        [HttpPost]
        [Authorize("GenreAdmin")]
        public async Task<IActionResult> Create([FromBody] GenreCreationRequestModel genreRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(genreRequestModel)}: [{genreRequestModel}].");

                var createdGenre = await genreCreationHandler.CreateGenre(genreRequestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(createdGenre)}.{nameof(createdGenre.Id)}: [{createdGenre.Id}].");

                return Created($"{ControllerHelper.GENRES_CONTROLLER_NAME}/{createdGenre.Id}", createdGenre);
            }
            catch (EmptyObjectException<GenreCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Empty request model in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
                return BadRequest(emptyEx.Error);
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
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] GenreEditingRequestModel genreRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}], {nameof(genreRequestModel)}: [{genreRequestModel}].");

                var updatedGenre = await genreEditingHandler.EditGenre(id, genreRequestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(updatedGenre)}.{nameof(updatedGenre.Id)}: [{updatedGenre?.Id}].");

                return Ok(updatedGenre);
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
            catch (NotFoundObjectException<Genre> ex)
            {
                logger.Warning(ex, $"Not found object error in {MethodNameHelper.GetCurrentMethodName()}. Returns 404 - Not Found. Message: [{ex.Message}].");
                return NotFound(ex.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("GenreAdmin")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(id)}: [{id}].");

                await genreDeleteHandler.DeleteGenre(id);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished.");
                return Ok();
            }
            catch (NotExistingIdException notExistingEx)
            {
                logger.Warning(notExistingEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{notExistingEx.Message}].");
                return NotFound(notExistingEx.Error);
            }
            catch (NotFoundObjectException<Genre> nfoEx)
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
