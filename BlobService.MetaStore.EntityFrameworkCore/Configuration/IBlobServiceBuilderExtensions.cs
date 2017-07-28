using BlobService.Core.Configuration;
using BlobService.Core.Entities;
using BlobService.Core.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.MetaStore.EntityFrameworkCore.Configuration
{
    public static class IBlobServiceBuilderExtensions
    {
        public static IBlobServiceBuilder AddEfMetaStores<TContext, TContainerMeta, TBlobMeta>(this IBlobServiceBuilder builder, Action<EfStoreOptions> setupAction = null)
            where TContext : BlobServiceContext<TContainerMeta, TBlobMeta>
            where TContainerMeta : ContainerMeta, new()
            where TBlobMeta : BlobMeta, new()
        {
            var efStoreOptions = new EfStoreOptions();
            setupAction?.Invoke(efStoreOptions);

            builder.Services.AddSingleton(efStoreOptions);

            builder.Services
               .AddEntityFramework()
               .AddDbContext<TContext>();

            builder.Services.AddScoped<IBlobMetaStore, EfBlobMetaStore<TContext, TContainerMeta, TBlobMeta>>();
            builder.Services.AddScoped<IContainerMetaStore, EfContainerMetaStore<TContext, TContainerMeta, TBlobMeta>>();

            return builder;
        }
    }
}
