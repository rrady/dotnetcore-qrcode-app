using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QRCodeAPI.Services;

namespace QRCodeAPI.Controllers
{
    [Route("api/qr")]
    public class QrController : ControllerBase
    {
        private readonly IQrService _service;

        public QrController(IQrService service)
        {
            _service = service;
        }
        
        [HttpGet("scan")]
        public async Task<IActionResult> ScanQr([FromQuery(Name = "path")] string path)
        {
            var result = await _service.ScanQrAsync(path);
            if (!string.IsNullOrWhiteSpace(result.Error))
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}