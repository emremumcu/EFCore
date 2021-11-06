namespace EFCore.AppLib.CodeFirst
{
    using EFCore.AppLib.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private IQueryable<T> EvaluateSpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }

        private readonly CodeFirstDbContext _dbContext;

        public GenericRepository(CodeFirstDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await EvaluateSpecification(spec).CountAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> ListByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListBySpecAsync(ISpecification<T> spec)
        {
            return await EvaluateSpecification(spec).ToListAsync();
        }
    }
}
