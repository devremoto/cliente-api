using CrossCutting.Ioc;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repository;
using Infra.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using NUnit.Framework;

namespace Tests.Repositories
{

    public class ClienteTests : IDisposable
    {
        public ClienteTests()
        {


        }


        private ServiceProvider _provider;
        private IClienteRepository _repository;
        private IUnitOfWork _uow;
        private Guid _guid;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.ConfigureTestServices<AppDbContext>();
            _provider = services.GetProvider();
            _uow = _provider.GetService<IUnitOfWork>();
            _repository = _provider.GetService<IClienteRepository>();
            var entity = _repository.Add(new Cliente { Nome = "Teste", Idade = 50 });
            _uow.Commit();
            _guid = entity.Id;
        }


        [Test]
        public void GetOne()
        {
            var entity = _repository.GetOne(_guid);
            Assert.AreEqual(_guid, entity.Id);
        }


        [Test]
        public void Remove()
        {
            var entity = _repository.GetOne(_guid);
            Assert.IsNotNull(entity);
            _repository.Remove(entity);
            _uow.Commit();
            entity = _repository.GetOne(_guid);
            Assert.IsNull(entity);
        }

        [Test]
        public void List()
        {
            var list = _repository.GetAll();
            var count = list.Count();
            Assert.IsTrue(count > 0, $"count = {count}");
            Assert.IsTrue(count == 1, $"count = {count}");
        }

        [Test]
        public void Update()
        {
            var entity = _repository.GetOne(_guid);

            var currentNome = entity.Nome;
            entity.Nome = "Novo nome";

            var currentIdade = entity.Idade;
            entity.Idade = 125;

            var result = _repository.Update(entity);
            Assert.AreEqual(_guid, entity.Id);
            Assert.AreEqual(_guid, result.Id);

            Assert.AreEqual(result.Nome, entity.Nome);
            Assert.AreNotEqual(result.Nome, currentNome);

            Assert.AreEqual(result.Idade, entity.Idade);
            Assert.AreNotEqual(result.Idade, currentIdade);
        }

        [Test]
        public void Find()
        {
            var entity = _repository.Find(x => x.Id == _guid);
            Assert.IsNotNull(entity);
        }

        [Test]
        public void Cleanup()
        {
            _repository.Remove(_guid);
            _uow.Commit();
        }

        public void Dispose()
        {
            Cleanup();
            _uow.Dispose();
        }
    }
}
