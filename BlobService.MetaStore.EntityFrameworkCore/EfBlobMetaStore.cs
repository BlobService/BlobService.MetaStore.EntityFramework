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
    public class EfBlobMetaStore<TContainerMeta, TBlobMeta> : IBlobMetaStore
        where TContainerMeta : class, IContainerMeta, new()
        where TBlobMeta : class, IBlobMeta, new()
    {
        protected readonly BlobServiceContext<TContainerMeta, TBlobMeta> _dbContext;
        public EfBlobMetaStore(BlobServiceContext<TContainerMeta, TBlobMeta> dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IBlobMeta> AddAsync(BlobCreateModel blobModel)
        {
            var blob = new TBlobMeta
            {
                ContainerId = blobModel.ContainerId,
                MimeType = blobModel.MimeType,
                OrigFileName = blobModel.OrigFileName,
                SizeInBytes = blobModel.SizeInBytes,
                StorageSubject = blobModel.StorageSubject
            };

            _dbContext.BlobsMetadata.Add(blob);
            await _dbContext.SaveChangesAsync();

            return blob;
        }

        public async Task<IEnumerable<IBlobMeta>> GetAllAsync(string containerId)
        {
            var blobs = await _dbContext.BlobsMetadata
                .Where(x => x.ContainerId == containerId)
                .ToListAsync();

            return blobs;
        }

        public async Task<IBlobMeta> GetAsync(string key)
        {
            var blob = await _dbContext.BlobsMetadata.FindAsync(key);

            return blob;
        }

        public async Task RemoveAsync(string key)
        {
            var blob = await _dbContext.BlobsMetadata.FindAsync(key);
            if (blob != null)
            {
                _dbContext.BlobsMetadata.Remove(blob);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IBlobMeta> UpdateAsync(string key, IBlobMeta blob)
        {
            var existingBlob = await _dbContext.BlobsMetadata.FindAsync(key);

            _dbContext.Entry(existingBlob).CurrentValues.SetValues(blob);
            _dbContext.Entry(existingBlob).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingBlob;
        }
    }
}
