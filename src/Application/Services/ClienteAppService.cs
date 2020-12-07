using Application.Interfaces;
using Application.Validations;
using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Application.Services
{
    public class ClienteAppService : BaseAppService<Cliente, ClienteViewModel>, IClienteAppService
    {
        private readonly IClienteService _clientService;
        private readonly IMapper _mapper;
        private readonly ClienteValidation _validation;

        public ClienteAppService(IClienteService clientService, IUnitOfWork uow, IMapper mapper, ClienteValidation validation)
        : base(clientService, mapper, uow)
        {
            _clientService = clientService;
            _mapper = mapper;
            _validation = validation;
        }

        public override ClienteViewModel ValidateDelete(ClienteViewModel model)
        {
            _validation.ValidateDelete(model);
            return model;
        }

        public override ClienteViewModel ValidateDelete(params object[] key)
        {
            var model = _validation.ValidateDelete(key);
            return model;
        }

        public override ClienteViewModel ValidateInsert(ClienteViewModel model)
        {
            _validation.ValidateInsert(model);
            return model;
        }

        public override ClienteViewModel ValidateUpdate(ClienteViewModel model)
        {
            _validation.ValidateUpdate(model);
            return model;
        }
    }
}