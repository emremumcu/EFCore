namespace EFCore.AppLib.CodeFirst
{
    using EFCore.AppLib.CodeFirst.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> ListByIdAsync(int id);

        Task<IReadOnlyList<T>> ListBySpecAsync(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<int> CountAsync(ISpecification<T> spec);
    }
}
