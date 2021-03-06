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
    [ApiController]
    [Route(ControllerHelper.CONTROLLER_ROUTE)]
    public class EpisodesController : ControllerBase
    {
        private readonly ILogger logger = Log.ForContext<EpisodesController>();
        private readonly IEpisodeCreation episodeCreationHandler;
        private readonly IEpisodeEditing episodeEditingHandler;
        private readonly IEpisodeDelete episodeDeleteHandler;
        private readonly IEpisodeInactivation episodeInactivationHandler;
        private readonly IEpisodeActivation episodeActivationHandler;

        public EpisodesController(IEpisodeCreation episodeCreationHandler, IEpisodeEditing episodeEditingHandler, IEpisodeDelete episodeDeleteHandler,
            IEpisodeInactivation episodeInactivationHandler, IEpisodeActivation episodeActivationHandler)
        {
            this.episodeCreationHandler = episodeCreationHandler;
            this.episodeEditingHandler = episodeEditingHandler;
            this.episodeDeleteHandler = episodeDeleteHandler;
            this.episodeInactivationHandler = episodeInactivationHandler;
            this.episodeActivationHandler = episodeActivationHandler;
        }

        [HttpPost]
        [Authorize("EpisodeAdmin")]
        public async Task<IActionResult> Create([FromBody] EpisodeCreationRequestModel episodeRequestModel)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(episodeRequestModel)}: [{episodeRequestModel}].");

                var createdEpisode = await episodeCreationHandler.CreateEpisode(episodeRequestModel);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished with result: [{createdEpisode}].");
                return Created($"{ControllerHelper.EPISODES_CONTROLLER_NAME}/{createdEpisode.Id}", createdEpisode);
            }
            catch (EmptyObjectException<EpisodeCreationRequestModel> emptyEx)
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
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                return BadRequest(mismatchEx.Error);
            }
            catch (AlreadyExistingObjectException<Episode> alreadyExistingEx)
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
        [Authorize("EpisodeAdmin")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] EpisodeEditingRequestModel episodeRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}], {nameof(episodeRequestModel)}: [{episodeRequestModel}].");

                var updatedEpisode = await episodeEditingHandler.EditEpisode(id, episodeRequestModel);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(updatedEpisode)}.{nameof(updatedEpisode.Id)}: [{updatedEpisode?.Id}].");

                return Ok(updatedEpisode);
            }
            catch (MismatchingIdException misEx)
            {
                logger.Warning(misEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{misEx.Message}].");
                return BadRequest(misEx.Error);
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
            catch (NotFoundObjectException<Episode> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                return NotFound(id);
            }
            catch (AlreadyExistingObjectException<Episode> alreadyEx)
            {
                logger.Warning(alreadyEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{alreadyEx.Message}].");
                return BadRequest(alreadyEx.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("EpisodeAdmin")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method started. {nameof(id)}: [{id}].");

                await episodeDeleteHandler.DeleteEpisode(id);

                logger.Information($"{MethodNameHelper.GetCurrentMethodName()} method finished.");
                return Ok();
            }
            catch (NotExistingIdException notExistingEx)
            {
                logger.Warning(notExistingEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{notExistingEx.Message}].");
                return NotFound(notExistingEx.Error);
            }
            catch (NotFoundObjectException<Episode> nfoEx)
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
        [Authorize("EpisodeAdmin")]
        public async Task<IActionResult> Inactivate([FromRoute] long id)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

                var updatedEpisode = await episodeInactivationHandler.Inactivate(id);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");

                return Ok(updatedEpisode);
            }
            catch (NotExistingIdException idEx)
            {
                logger.Warning(idEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{idEx.Message}].");
                return BadRequest(idEx.Error);
            }
            catch (NotFoundObjectException<Episode> ex)
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

        [HttpPatch("{id}/activate")]
        [Authorize("EpisodeAdmin")]
        public async Task<IActionResult> Activate([FromRoute] long id)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

                var updatedEpisode = await episodeActivationHandler.Activate(id);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");

                return Ok(updatedEpisode);
            }
            catch (NotExistingIdException idEx)
            {
                logger.Warning(idEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{idEx.Message}].");
                return BadRequest(idEx.Error);
            }
            catch (NotFoundObjectException<Episode> ex)
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
