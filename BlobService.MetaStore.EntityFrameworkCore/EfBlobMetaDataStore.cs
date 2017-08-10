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
    public class EfBlobMetaDataStore<TContext> : IBlobMetaDataStore
        where TContext : BlobServiceContext
    {

        protected readonly TContext _dbContext;

        public EfBlobMetaDataStore(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IBlobMetaData> AddAsync(string blobId, BlobMetaDataCreateModel blobMetaDataModel)
        {
            BlobMetaData metaData = null;

            if (_dbContext.BlobMetaDatas.Any(x => x.BlobId == blobId && x.Key == blobMetaDataModel.Key))
            {
                metaData = await UpdateAsync(blobId, blobMetaDataModel.Key, blobMetaDataModel.Value) as BlobMetaData;
            }
            else
            {
                var newMetaData = new BlobMetaData
                {
                    BlobId = blobId,
                    Key = blobMetaDataModel.Key,
                    Value = blobMetaDataModel.Value
                };

                await _dbContext.BlobMetaDatas.AddAsync(newMetaData);

                metaData = newMetaData;
            }

            await _dbContext.SaveChangesAsync();

            return metaData;
        }

        public async Task DeleteByKeyAsync(string blobId, string key)
        {
            var metaData = _dbContext.BlobMetaDatas.Where(x => x.BlobId == blobId && x.Key == key).FirstOrDefault();
            _dbContext.BlobMetaDatas.Remove(metaData);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<IBlobMetaData>> GetAllFromBlobAsync(string blobId)
        {
            var metaData = await _dbContext.BlobMetaDatas.Where(x => x.BlobId == blobId).ToListAsync();
            return metaData;
        }

        public async Task<string> GetValueAsync(string blobId, string key)
        {
            var bmd = await _dbContext.BlobMetaDatas.Where(x => x.BlobId == blobId && x.Key == key).FirstOrDefaultAsync();
            return bmd.Value;
        }

        public async Task<IBlobMetaData> UpdateAsync(string blobId, string key, string value)
        {
            var bmd = _dbContext.BlobMetaDatas.Where(x => x.BlobId == blobId && x.Key == key).FirstOrDefault();
            bmd.Value = value;
            await _dbContext.SaveChangesAsync();
            return bmd;
        }
    }
}
