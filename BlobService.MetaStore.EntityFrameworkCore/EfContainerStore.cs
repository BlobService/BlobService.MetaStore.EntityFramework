using BlobService.Core.Entities;
using BlobService.Core.Models;
using BlobService.Core.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class EfContainerStore<TContext> : IContainerStore
        where TContext : BlobServiceContext
    {
        protected readonly TContext _dbContext;

        public EfContainerStore(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IContainer> AddAsync(ContainerCreateModel contianerModel)
        {
            var container = new Container()
            {
                Name = contianerModel.Name
            };

            _dbContext.Containers.Add(container);
            await _dbContext.SaveChangesAsync();

            return container;
        }

        public async Task<IEnumerable<IContainer>> GetAllAsync()
        {
            return await _dbContext.Containers.ToListAsync();
        }

        public async Task<IContainer> GetAsync(string key)
        {
            var container = await _dbContext.Containers.FindAsync(key);
            return container;
        }

        public async Task<IEnumerable<IBlob>> GetBlobsAsync(string containerKey)
        {
            var blobs = await _dbContext.Blobs
                .Where(x => x.ContainerId == containerKey).Include(x => x.MetaData)
                .ToListAsync();

            return blobs;
        }

        public async Task<IContainer> GetByNameAsync(string name)
        {
            var container = await _dbContext.Containers
                .Where(x => x.Name == name)
                .SingleOrDefaultAsync();

            return container;
        }

        public async Task RemoveAsync(string key)
        {
            var container = await _dbContext.Containers.FindAsync(key);
            if (container != null)
            {
                _dbContext.Containers.Remove(container);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IContainer> UpdateAsync(string key, IContainer container)
        {
            var existingContainer = await _dbContext.Containers.FindAsync(key);

            _dbContext.Entry(existingContainer).CurrentValues.SetValues(container);
            _dbContext.Entry(existingContainer).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingContainer;
        }
    }
}