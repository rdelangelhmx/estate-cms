﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tti.Estate.Data.Entities;
using Tti.Estate.Data.Specifications;

namespace Tti.Estate.Data.Repositories
{
    internal abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected AppDbContext DbContext { get; }

        protected BaseRepository(AppDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetAsync(long id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<bool> AnyAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).AnyAsync();
        }

        public async Task<TEntity> SingleAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> ListAsync()
        {
            return await DbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> ListAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(DbContext.Set<TEntity>().AsQueryable(), specification);
        }
    }
}
