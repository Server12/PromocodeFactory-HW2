using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly IList<T> Data;

        public InMemoryRepository(IList<T> data)
        {
            Data = data;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Task.FromResult(Data);
        }

        public async Task<T> GetByPredicate(Predicate<T> predicate)
        {
            return await Task.FromResult(Data.FirstOrDefault(entity => predicate(entity)));
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public async Task<T> AddAsync(T entity)
        {
            Data.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<T> DeleteAsync(Guid guid)
        {
            var entity = await GetByIdAsync(guid);
            Data.Remove(entity);
            return entity;
        }
    }
}