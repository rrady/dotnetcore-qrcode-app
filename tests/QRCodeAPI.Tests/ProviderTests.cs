using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRCodeAPI.Services;

namespace QRCodeAPI.Tests
{
    [TestClass]
    public class ProviderTests
    {
        [TestMethod]
        public async Task ProviderTest_ThrowsErrorIfArgumentIsNull()
        {
            // ARRANGE
            ILogger<FileQrProvider> logger = new Logger<FileQrProvider>(new NullLoggerFactory());
            IQrProvider provider = new FileQrProvider(logger);
            string path = null;
            
            // ACT
            Func<Task> task = async () =>
            {
                await provider.ProvideQrAsync(path);
            };
            
            // ASSERT
            await Assert.ThrowsExceptionAsync<ApplicationException>(task);
        }
        
        [TestMethod]
        public async Task ProviderTest_ThrowsErrorIfFileDoesntExists()
        {
            // ARRANGE
            ILogger<FileQrProvider> logger = new Logger<FileQrProvider>(new NullLoggerFactory());
            IQrProvider provider = new FileQrProvider(logger);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "file_which_doesnt_exists.png");
            
            // ACT
            Func<Task> task = async () =>
            {
                await provider.ProvideQrAsync(path);
            };
            
            // ASSERT
            await Assert.ThrowsExceptionAsync<ApplicationException>(task);
        }

        [TestMethod]
        public async Task ProviderTest_FileExists()
        {
            // ARRANGE
            ILogger<FileQrProvider> logger = new Logger<FileQrProvider>(new NullLoggerFactory());
            IQrProvider provider = new FileQrProvider(logger);
            string fileName = "code.png";
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            // ACT
            var result = await provider.ProvideQrAsync(path);
            
            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(fileName, result.Name);
            Assert.IsTrue(result.Content.Length > 0);
        }
    }
}