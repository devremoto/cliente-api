using System;
using System.Net;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.ViewModels.Common;
using Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers.Base
{
    public class CrudBaseController<T, TViewModel> : ControllerBase
        where T : class, IEntity
       where TViewModel : class, IViewModel, new()
    {
        protected IBaseAppService<T, TViewModel> _appService;
        protected readonly ILogger<T> _logger;

        public CrudBaseController(IBaseAppService<T, TViewModel> appService, ILogger<T> logger)
        {
            _appService = appService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var result = _appService.GetAll();
                return Ok(await Task.FromResult(result));
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Get([FromRoute] object id)
        {
            try
            {
                var result = _appService.GetOne(id);
                if (result == null)
                {
                    return NotFound(new { Message = $"{nameof(T)} {id?.ToString()} not found" });
                }
                return Ok(await Task.FromResult(result));
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TViewModel model)
        {
            try
            {
                var result = _appService.Add(model);
                if (!result.IsValid)
                {
                    return StatusCode(result.ErrorCode, result.Errors);
                }
                return Created($"api/Client/{result.Id}", await Task.FromResult(result));
            }
            catch (ArgumentException)
            {
                return BadRequest(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal error");
            }
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] TViewModel model)
        {
            try
            {
                var result = _appService.Update(model);
                if (!result.IsValid)
                {
                    return StatusCode(result.ErrorCode, result.Errors);
                }
                return Ok(await Task.FromResult(result));
            }
            catch (ArgumentException)
            {
                return BadRequest(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal error");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var result = _appService.Remove(id);

                await Task.FromResult(result);
                if (!result.IsValid)
                {
                    return StatusCode(result.ErrorCode, result.Errors);
                }
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal error");
            }
        }
    }
}