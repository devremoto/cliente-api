using Api.Controllers.Base;
using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : CrudBaseController<Cliente, ClienteViewModel>
    {

        public ClienteController(IClienteAppService service, ILogger<Cliente> logger)
            : base(service, logger)
        {

        }

    }
}