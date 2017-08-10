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
    public class EfBlobStore<TContext> : IBlobStore
        where TContext : BlobServiceContext
    {
        protected readonly TContext _dbContext;

        public EfBlobStore(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IBlob> AddAsync(BlobCreateModel blobModel)
        {
            var blob = new Blob
            {
                ContainerId = blobModel.ContainerId,
                MimeType = blobModel.MimeType,
                OrigFileName = blobModel.OrigFileName,
                SizeInBytes = blobModel.SizeInBytes,
                StorageSubject = blobModel.StorageSubject
            };

            _dbContext.Blobs.Add(blob);
            await _dbContext.SaveChangesAsync();

            return blob;
        }

        public async Task<IEnumerable<IBlob>> GetAllAsync(string containerId)
        {
            var blobs = await _dbContext.Blobs
                .Where(x => x.ContainerId == containerId)
                .ToListAsync();

            return blobs;
        }

        public async Task<IBlob> GetByIdAsync(string key)
        {
            var blob = await _dbContext.Blobs.Where(x => x.Id == key).Include(x => x.MetaData).FirstOrDefaultAsync();
            return blob;
        }

        public async Task RemoveAsync(string key)
        {
            var blob = await _dbContext.Blobs.FindAsync(key);
            if (blob != null)
            {
                _dbContext.Blobs.Remove(blob);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IBlob> UpdateAsync(string key, IBlob blob)
        {
            var existingBlob = await _dbContext.Blobs.FindAsync(key);

            _dbContext.Entry(existingBlob).CurrentValues.SetValues(blob);
            _dbContext.Entry(existingBlob).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingBlob;
        }
    }
}