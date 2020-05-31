using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using QRCodeAPI.Models;

namespace QRCodeAPI.Services
{
    public class FileQrProvider : IQrProvider
    {
        private const string TAG = "[FileQrProvider]";
        private readonly ILogger<FileQrProvider> _logger;

        public FileQrProvider(ILogger<FileQrProvider> logger)
        {
            _logger = logger;
        }
        
        public async Task<QrFileModel> ProvideQrAsync(string path)
        {
            _logger.LogTrace($"{TAG} ProvideQrAsync called for path: {path}");

            var fileExists = File.Exists(path);
            if (!fileExists)
            {
                throw new ApplicationException("There is no QR code at the given path.");
            }
            
            var content = await File.ReadAllBytesAsync(path);

            return new QrFileModel
            {
                Name = Path.GetFileName(path),
                Content = content
            };
        }
    }
}