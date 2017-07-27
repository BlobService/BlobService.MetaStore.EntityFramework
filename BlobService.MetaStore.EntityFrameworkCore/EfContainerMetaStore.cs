using BlobService.Core.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using BlobService.Core.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BlobService.Core.Models;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class EfContainerMetaStore<TContext, TContainerMeta, TBlobMeta> : IContainerMetaStore
        where TContext : BlobServiceContext<TContainerMeta, TBlobMeta>
        where TContainerMeta : class, IContainerMeta, new()
        where TBlobMeta : class, IBlobMeta, new()
    {
        protected readonly TContext _dbContext;
        public EfContainerMetaStore(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IContainerMeta> AddAsync(ContainerCreateModel contianerModel)
        {
            var container = new TContainerMeta()
            {
                Name = contianerModel.Name
            };

            _dbContext.ContainersMetadata.Add(container);
            await _dbContext.SaveChangesAsync();

            return container;
        }

        public async Task<IEnumerable<IContainerMeta>> GetAllAsync()
        {
            return await _dbContext.ContainersMetadata.ToListAsync();
        }

        public async Task<IContainerMeta> GetAsync(string key)
        {
            var container = await _dbContext.ContainersMetadata.FindAsync(key);
            return container;
        }

        public async Task<IEnumerable<IBlobMeta>> GetBlobsAsync(string containerKey)
        {
            var blobs = await _dbContext.BlobsMetadata
                .Where(x => x.ContainerId == containerKey)
                .ToListAsync();

            return blobs;
        }

        public async Task<IContainerMeta> GetByNameAsync(string name)
        {
            var container = await _dbContext.ContainersMetadata
                .Where(x => x.Name == name)
                .SingleOrDefaultAsync();

            return container;
        }

        public async Task RemoveAsync(string key)
        {
            var container = await _dbContext.ContainersMetadata.FindAsync(key);
            if (container != null)
            {
                _dbContext.ContainersMetadata.Remove(container);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IContainerMeta> UpdateAsync(string key, IContainerMeta container)
        {
            var existingContainer = await _dbContext.ContainersMetadata.FindAsync(key);

            _dbContext.Entry(existingContainer).CurrentValues.SetValues(container);
            _dbContext.Entry(existingContainer).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingContainer;
        }
    }
}
