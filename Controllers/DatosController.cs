using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DatosController : ControllerBase
{
    private readonly IAzureStorageService _storage;

    public DatosController(IAzureStorageService storage)
    {
        _storage = storage;
    }


    [HttpPost("insert")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Insert([FromBody] DatoUsuario data)
    {
        if (data == null || string.IsNullOrWhiteSpace(data.Nombre))
            return BadRequest("Datos incompletos");

        await _storage.InsertUserDataAsync(data);
        return Ok();
    }


    [HttpGet("downloadBase64")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadImageAsDataUrl()
    {
        try
        {
            var dataUrl = await _storage.GetImageAsDataUrlAsync("imgpub", "demo1.png");

            if (string.IsNullOrEmpty(dataUrl))
                return NotFound("Imagen no encontrada");

            return Content(dataUrl, "text/plain"); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }



    [HttpGet("sas")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult GetSasUrl()
    {
        var url = _storage.GenerateSasToken("imgpriv", "demo2.png", TimeSpan.FromHours(1));
        return Ok(url);
    }
}
