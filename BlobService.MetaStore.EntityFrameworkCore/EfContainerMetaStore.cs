using BlobService.Core.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using BlobService.Core.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class EfContainerMetaStore : IContainerMetaStore
    {
        protected readonly BlobServiceContext _dbContext;
        public EfContainerMetaStore(BlobServiceContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ContainerMeta> AddAsync(ContainerMeta container)
        {
            _dbContext.ContainersMetaData.Add(container);
            await _dbContext.SaveChangesAsync();

            return container;
        }

        public async Task<IEnumerable<ContainerMeta>> GetAllAsync()
        {
            return await _dbContext.ContainersMetaData.ToListAsync();
        }

        public async Task<ContainerMeta> GetAsync(string key)
        {
            var container = await _dbContext.ContainersMetaData.FindAsync(key);
            return container;
        }

        public async Task<IEnumerable<BlobMeta>> GetBlobsAsync(string containerKey)
        {
            var blobs = await _dbContext.BlobsMetaData
                .Where(x => x.ContainerId == containerKey)
                .ToListAsync();

            return blobs;
        }

        public async Task<ContainerMeta> GetByNameAsync(string name)
        {
            var container = await _dbContext.ContainersMetaData
                .Where(x => x.Name == name)
                .SingleOrDefaultAsync();

            return container;
        }

        public async Task RemoveAsync(string key)
        {
            var container = await _dbContext.ContainersMetaData.FindAsync(key);
            if (container != null)
            {
                _dbContext.ContainersMetaData.Remove(container);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ContainerMeta> UpdateAsync(string key, ContainerMeta container)
        {
            var existingContainer = await _dbContext.ContainersMetaData.FindAsync(key);

            _dbContext.Entry(existingContainer).CurrentValues.SetValues(container);
            _dbContext.Entry(existingContainer).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingContainer;
        }
    }
}
