using System.Threading.Tasks;
using QRCodeAPI.Models;

namespace QRCodeAPI.Client
{
    public interface IQrClient
    {
        Task<QrResponseModel> PostQrAsync(QrFileModel file);
    }
}