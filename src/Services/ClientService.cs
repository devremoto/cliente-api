using Domain.Entities;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;

namespace Services
{
	public class ClientService:BaseService<Cliente>,IClienteService
  {
    public ClientService(IClienteRepository repository)
    :base(repository)
    {

    }
  }
}
