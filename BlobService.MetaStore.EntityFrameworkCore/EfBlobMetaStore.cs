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
    public class EfBlobMetaStore : IBlobMetaStore
    {
        protected readonly BlobServiceContext _dbContext;
        public EfBlobMetaStore(BlobServiceContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<BlobMeta> AddAsync(BlobMeta blob)
        {
            _dbContext.BlobsMetaData.Add(blob);
            await _dbContext.SaveChangesAsync();

            return blob;
        }

        public async Task<IEnumerable<BlobMeta>> GetAllAsync(string containerId)
        {
            var blobs = await _dbContext.BlobsMetaData
                .Where(x => x.ContainerId == containerId)
                .ToListAsync();

            return blobs;
        }

        public async Task<BlobMeta> GetAsync(string key)
        {
            var blob = await _dbContext.BlobsMetaData.FindAsync(key);

            return blob;
        }

        public async Task RemoveAsync(string key)
        {
            var blob = await _dbContext.BlobsMetaData.FindAsync(key);
            if(blob != null)
            {
                _dbContext.BlobsMetaData.Remove(blob);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<BlobMeta> UpdateAsync(string key, BlobMeta blob)
        {
            var existingBlob = await _dbContext.BlobsMetaData.FindAsync(key);

            _dbContext.Entry(existingBlob).CurrentValues.SetValues(blob);
            _dbContext.Entry(existingBlob).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingBlob;
        }
    }
}
