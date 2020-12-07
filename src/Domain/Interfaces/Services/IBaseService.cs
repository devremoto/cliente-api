using Domain.Interfaces.Repository;

namespace Domain.Interfaces.Services
{
	public interface IBaseService<T> : IBaseRepository<T> where T : class
	{
	}
}