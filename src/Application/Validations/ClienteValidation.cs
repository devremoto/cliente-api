using Application.ViewModels;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Application.Validations
{
    public class ClienteValidation
    {
        private readonly IClienteService _clienteService;

        public ClienteValidation(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        void Reset(ClienteViewModel model)
        {
            model.Errors = new List<string>();
            model.IsValid = true;
        }

        public void ValidateDelete(ClienteViewModel model)
        {
            Reset(model);
            ValidateId(model);
            ValidateExists(model);
        }

        public ClienteViewModel ValidateDelete(params object[] key)
        {
            var model = ValidateId(key);
            if (!model.IsValid)
            {
                return model;
            }

            model = ValidateExists(key);
            return model;
        }

        public void ValidateInsert(ClienteViewModel model)
        {
            Reset(model);
            IdShouldBeEmpty(model);
            ValidateNotExists(model);
            ValidadeIdade(model);
            ValidadeNome(model);
        }



        public void ValidateUpdate(ClienteViewModel model)
        {
            ValidateId(model);
            ValidateExists(model);
            ValidadeNome(model);
        }

        private static void ValidadeIdade(ClienteViewModel model)
        {
            if (model.Idade < 0)
            {
                model.IsValid = false;
                model.Errors.Add("Idade Invalida");
            }
        }

        private static void ValidadeNome(ClienteViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nome))
            {
                model.IsValid = false;
                model.Errors.Add("Nome não pode ser vazio");
            }
        }

        private static void IdShouldBeEmpty(ClienteViewModel model)
        {
            if (model.Id != Guid.Empty)
            {
                model.IsValid = false;
                model.Errors.Add("Id deve ser vazio");
                model.ErrorCode = 400;
            }
        }

        private static void ValidateId(ClienteViewModel model)
        {
            if (model.Id == Guid.Empty)
            {
                model.IsValid = false;
                model.Errors.Add("Id Não pode ser vazio");
                model.ErrorCode = 400;
            }
        }

        private void ValidateExists(ClienteViewModel model)
        {
            var exists = _clienteService.GetOne(model.Id);
            if (exists == null)
            {
                model.IsValid = false;
                model.Errors.Add("Cliente não existe");
                model.ErrorCode = 404;
            }
        }

        private ClienteViewModel ValidateId(params object[] key)
        {
            var model = new ClienteViewModel { IsValid = true };
            if (key == null)
            {
                model.IsValid = false;
                model.Errors.Add("Id Não pode ser vazio");
                model.ErrorCode = 400;
            }
            return model;
        }

        private ClienteViewModel ValidateExists(params object[] key)
        {
            var model = new ClienteViewModel { IsValid = true };
            var exists = _clienteService.GetOne(key);
            if (exists == null)
            {
                model.IsValid = false;
                model.Errors.Add("Cliente não existe");
                model.ErrorCode = 404;
            }
            return model;
        }

        private void ValidateNotExists(ClienteViewModel model)
        {
            var exists = _clienteService.GetOne(model.Id);
            if (exists != null)
            {
                model.IsValid = false;
                model.Errors.Add("Cliente já existe");
                model.ErrorCode = 400;
            }
        }
    }
}
