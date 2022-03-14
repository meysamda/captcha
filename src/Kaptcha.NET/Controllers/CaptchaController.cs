using System;
using System.IO;
using System.Threading.Tasks;
using KaptchaNET.Services.CaptchaGenerator;
using KaptchaNET.Services.Validation;
using Microsoft.AspNetCore.Mvc;

namespace KaptchaNET.Controllers
{
    [Route("api")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaGeneratorService _generator;
        private readonly ICaptchaValidationService _validation;

        public CaptchaController(ICaptchaGeneratorService generator, ICaptchaValidationService validation)
        {
            _generator = generator;
            _validation = validation;
        }

        [HttpGet("create")]
        public async Task<IActionResult> CreateCaptcha([FromQuery] Guid id)
        {
            Captcha captcha = await _generator.CreateCaptchaAsync(id);
            using (var ms = new MemoryStream())
            {
                captcha.Image.Save(ms, _generator.Options.ImageFormat);
                byte[] b = ms.ToArray();
                string imageFormatHeader = $"image/{_generator.Options.ImageFormat.ToString().ToLower()}";
                return File(b, imageFormatHeader);
            }
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateCaptcha([FromQuery] Guid id, [FromQuery] string solution)
        {
            try
            {
                await _validation.ValidateAsync(id, solution);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
