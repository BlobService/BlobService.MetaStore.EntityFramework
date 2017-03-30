using BlobService.Core.Configuration;
using BlobService.Core.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Store.EntityFramework.Configuration
{
    public static class IBlobServiceBuilderExtensions
    {
        public static IBlobServiceBuilder AddEfMetaStores(this IBlobServiceBuilder builder, string connectionString)
        {
            builder.Services
               .AddEntityFramework()
               .AddDbContext<BlobServiceContext>(options =>
               {
                   options.UseSqlServer(connectionString);
               });

            builder.Services.AddScoped<IBlobMetaStore, EfBlobMetaStore>();
            builder.Services.AddScoped<IContainerMetaStore, EfContainerMetaStore>();

            return builder;
        }
    }
}
