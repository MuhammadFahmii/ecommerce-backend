// ------------------------------------------------------------------------------------
// CompressionHandler.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace netca.Api.Handlers
{
    /// <summary>
    /// CompressionHandler
    /// </summary>
    public static class CompressionHandler
    {
        /// <summary>
        /// ApplyCompress
        /// </summary>
        /// <param name="services"></param>
        public static void ApplyCompress(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                var mimeTypes = new[]
                    {
                        "application/pdf",
                        "text/plain",
                        "image/jpg",
                        "application/json",
                        "application/octet-stream",
                        "application/problem+json",
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "text/csv"
                    };
                options.EnableForHttps = true;
                options.MimeTypes = mimeTypes;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
        }
    }

    /// <summary>
    /// AddCompressionHandlerExtension
    /// </summary>
    public static class AddCompressionHandlerExtension
    {
        /// <summary>
        /// AddCompressionHandler
        /// </summary>
        /// <param name="services"></param>
        public static void AddCompressionHandler(this IServiceCollection services)
        {
            CompressionHandler.ApplyCompress(services);
        }
    }
}