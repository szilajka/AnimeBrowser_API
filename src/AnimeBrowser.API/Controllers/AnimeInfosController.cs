﻿using AnimeBrowser.API.Helpers;
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
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    [ApiController]
    public class AnimeInfosController : ControllerBase
    {
        private readonly IAnimeInfoCreation animeInfoCreateHandler;
        private readonly IAnimeInfoEditing animeInfoEditorHandler;
        private readonly IAnimeInfoDelete animeInfoDeleteHandler;
        private readonly IAnimeInfoInactivation animeInfoInactivationHandler;
        private readonly IAnimeInfoActivation animeInfoActivationHandler;
        private readonly ILogger logger = Log.ForContext<AnimeInfosController>();

        public AnimeInfosController(IAnimeInfoCreation animeInfoCreateHandler, IAnimeInfoEditing animeInfoEditorHandler, IAnimeInfoDelete animeInfoDeleteHandler,
            IAnimeInfoInactivation animeInfoInactivationHandler, IAnimeInfoActivation animeInfoActivationHandler)
        {
            this.animeInfoCreateHandler = animeInfoCreateHandler;
            this.animeInfoEditorHandler = animeInfoEditorHandler;
            this.animeInfoDeleteHandler = animeInfoDeleteHandler;
            this.animeInfoInactivationHandler = animeInfoInactivationHandler;
            this.animeInfoActivationHandler = animeInfoActivationHandler;
        }


        [HttpPost]
        [Authorize("AnimeInfoAdmin")]
        public async Task<IActionResult> Create([FromBody] AnimeInfoCreationRequestModel animeInfoRequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(animeInfoRequestModel)}: [{animeInfoRequestModel}].");

                var createdAnimeInfo = await animeInfoCreateHandler.CreateAnimeInfo(animeInfoRequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdAnimeInfo}].");
                return Created($"{ControllerHelper.ANIME_INFOS_CONTROLLER_NAME}/{createdAnimeInfo.Id}", createdAnimeInfo);
            }
            catch (EmptyObjectException<AnimeInfoCreationRequestModel> emptyEx)
            {
                logger.Warning(emptyEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyEx.Message}].");
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
        [Authorize("AnimeInfoAdmin")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] AnimeInfoEditingRequestModel animeInfoRequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(id)}: [{id}], {nameof(animeInfoRequestModel)}: [{animeInfoRequestModel}].");

                var updatedAnimeInfo = await animeInfoEditorHandler.EditAnimeInfo(id, animeInfoRequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished. result: [{updatedAnimeInfo}].");

                return Ok(updatedAnimeInfo);
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
            catch (NotFoundObjectException<AnimeInfo> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                return NotFound(ex.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("AnimeInfoAdmin")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(id)}: [{id}].");

                await animeInfoDeleteHandler.DeleteAnimeInfo(id);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished.");
                return Ok();
            }
            catch (NotExistingIdException notExistingEx)
            {
                logger.Warning(notExistingEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{notExistingEx.Message}].");
                return NotFound(notExistingEx.Error);
            }
            catch (NotFoundObjectException<AnimeInfo> nfoEx)
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
        [Authorize("AnimeInfoAdmin")]
        public async Task<IActionResult> Inactivate([FromRoute] long id)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(id)}: [{id}].");

                var updatedAnimeInfo = await animeInfoInactivationHandler.Inactivate(id);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished. result: [{updatedAnimeInfo}].");

                return Ok(updatedAnimeInfo);
            }
            catch (NotExistingIdException idEx)
            {
                logger.Warning(idEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{idEx.Message}].");
                return BadRequest(idEx.Error);
            }
            catch (NotFoundObjectException<AnimeInfo> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                return NotFound(ex.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}/activate")]
        [Authorize("AnimeInfoAdmin")]
        public async Task<IActionResult> Activate([FromRoute] long id)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(id)}: [{id}].");

                var updatedAnimeInfo = await animeInfoActivationHandler.Activate(id);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished. result: [{updatedAnimeInfo}].");

                return Ok(updatedAnimeInfo);
            }
            catch (NotExistingIdException idEx)
            {
                logger.Warning(idEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{idEx.Message}].");
                return BadRequest(idEx.Error);
            }
            catch (NotFoundObjectException<AnimeInfo> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                return NotFound(ex.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
