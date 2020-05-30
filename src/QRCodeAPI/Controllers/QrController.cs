using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCodeAPI.Services;

namespace QRCodeAPI.Controllers
{
    [Route("api")]
    public class QrController : ControllerBase
    {
        private readonly IQrService _service;

        public QrController(IQrService service)
        {
            _service = service;
        }
        
        /// <summary>
        ///    Gets the encoded message from a QR code by the path for a QR image.
        /// </summary>
        /// <remarks>
        ///    Sample request:
        ///         GET /api/qr/scan?path=code.png
        ///
        ///     Sample response:
        ///         {
        ///             "data": "HelloWorld",
        ///             "error": null
        ///         }
        /// </remarks>
        /// <param name="path">The path for QR image.</param>
        /// <returns></returns>
        /// <response code="200">Returns the encoded message.</response>
        /// <response code="400">If the QR image was not found or the QR couldn't be decoded.</response>
        [HttpGet("scan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> ScanQr([FromQuery(Name = "path")] string path)
        {
            var result = await _service.ScanQrAsync(path);
            if (!string.IsNullOrWhiteSpace(result.Error))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}