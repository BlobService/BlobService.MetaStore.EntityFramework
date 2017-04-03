using BlobService.Core.Configuration;
using BlobService.Core.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.MetaStore.EntityFramework.Configuration
{
    public static class IBlobServiceBuilderExtensions
    {
        public static IBlobServiceBuilder AddEfMetaStores(this IBlobServiceBuilder builder, Action<EfStoreOptions> setupAction = null)
        {
            var efStoreOptions = new EfStoreOptions();
            setupAction?.Invoke(efStoreOptions);
            efStoreOptions.TryValidate();

            builder.Services.AddSingleton(efStoreOptions);

            builder.Services
               .AddEntityFramework()
               .AddDbContext<BlobServiceContext>(options =>
               {
                   options.UseSqlServer(efStoreOptions.ConnectionString);
               });

            builder.Services.AddScoped<IBlobMetaStore, EfBlobMetaStore>();
            builder.Services.AddScoped<IContainerMetaStore, EfContainerMetaStore>();

            return builder;
        }
    }
}
