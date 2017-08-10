using BlobService.Core.Configuration;
using BlobService.Core.Stores;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlobService.MetaStore.EntityFrameworkCore.Configuration
{
    public static class IBlobServiceBuilderExtensions
    {
        public static IBlobServiceBuilder AddEfMetaStores<TContext>(this IBlobServiceBuilder builder, Action<EfStoreOptions> setupAction = null)
            where TContext : BlobServiceContext
        {
            var efStoreOptions = new EfStoreOptions();
            setupAction?.Invoke(efStoreOptions);

            builder.Services.AddSingleton(efStoreOptions);

            builder.Services
               .AddEntityFramework()
               .AddDbContext<TContext>();

            builder.Services.AddScoped<IBlobStore, EfBlobStore<TContext>>();
            builder.Services.AddScoped<IContainerStore, EfContainerStore<TContext>>();
            builder.Services.AddScoped<IBlobMetaDataStore, EfBlobMetaDataStore<TContext>>();
            return builder;
        }
    }
}