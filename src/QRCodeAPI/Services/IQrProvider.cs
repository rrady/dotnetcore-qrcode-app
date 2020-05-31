using System.Threading.Tasks;
using QRCodeAPI.Models;

namespace QRCodeAPI.Services
{
    public interface IQrProvider
    {
        Task<QrFileModel> ProvideQrAsync(string path);
    }
}