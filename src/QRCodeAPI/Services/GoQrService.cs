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
        private readonly IQrProvider _provider;
        private readonly ICacheStore _cacheStore;
        private readonly ILogger<GoQrService> _logger;

        public GoQrService(IQrClient client, IQrProvider provider, ICacheStore cacheStore, ILogger<GoQrService> logger)
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
                _logger.LogTrace($"{TAG} Returning response found in cache.");
                return cachedResponse;
            }

            QrFileModel file = null;
            try
            {
                file = await _provider.ProvideQrAsync(qrPath);
            }
            catch (Exception e)
            {
                _logger.LogError($"{TAG} Provider call failed. Reason: '{e.Message}'.");
                throw;
            }

            QrResponseModel result = null;
            try
            {
                result = await _client.PostQrAsync(file);
            }
            catch (Exception e)
            {
                _logger.LogError($"{TAG} QR api call failed. Reason: '{e.Message}'.");
                throw;
            }

            await _cacheStore.AddAsync(cacheKey, result);
            return result;
        }
    }
}