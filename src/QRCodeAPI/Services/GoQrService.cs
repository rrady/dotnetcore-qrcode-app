using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using QRCodeAPI.CacheStore;
using QRCodeAPI.CacheStore.CacheKeys;
using QRCodeAPI.Models;
using QRCodeAPI.Client;

namespace QRCodeAPI.Services
{
    public class GoQrService : IQrService
    {
        private const string TAG = "[GoQrService]";
        
        private readonly IQrClient _client;
        private readonly IFileProvider _provider;
        private readonly ICacheStore _cacheStore;
        private readonly ILogger<GoQrService> _logger;

        public GoQrService(IQrClient client, IFileProvider provider, ICacheStore cacheStore, ILogger<GoQrService> logger)
        {
            _client = client;
            _provider = provider;
            _cacheStore = cacheStore;
            _logger = logger;
        }
        
        public async Task<QrResponseModel> ScanQrAsync(string qrPath)
        {
            _logger.LogTrace($"{TAG} ScanQrAsync called with path: {qrPath}");
            
            if (string.IsNullOrWhiteSpace(qrPath))
            {
                throw new ArgumentException("The path cannot be null or empty.");
            }
            
            var cacheKey = new QrCacheKey(qrPath);
            var cachedResponse = await _cacheStore.GetAsync<QrResponseModel>(cacheKey);
            if (cachedResponse != null)
            {
                _logger.LogTrace($"{TAG} Response found in cache.");
                return cachedResponse;
            }
            
            var file = await ReadQrFromDisk(qrPath);

            var qrFile = new QrFileModel
            {
                Name = file.name,
                Content = file.content
            };
            _logger.LogTrace($"{TAG} Calling qr api.");
            var result = await _client.PostQrAsync(qrFile);
            return result;
        }

        private async Task<(string name, byte[] content)> ReadQrFromDisk(string path)
        {
            _logger.LogTrace($"{TAG} Reading qr image from disk.");
            
            var fileInfo = _provider.GetFileInfo(path);
            if (!fileInfo.Exists && !fileInfo.IsDirectory)
            {
                throw new ApplicationException("There is no QR code at the path provided.");
            }
            
            var fileContent = await File.ReadAllBytesAsync(fileInfo.PhysicalPath);

            return (fileInfo.Name, fileContent);
        }
    }
}