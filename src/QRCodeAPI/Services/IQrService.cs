using System.Threading.Tasks;
using QRCodeAPI.Models;

namespace QRCodeAPI.Services
{
    public interface IQrService
    {
        Task<QrResponseModel> ScanQrAsync(string qrPath);
    }
}