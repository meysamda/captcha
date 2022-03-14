using System;
using System.Threading.Tasks;
using KaptchaNET.Exceptions;
using KaptchaNET.Options;
using KaptchaNET.Services.Storage;
using Microsoft.Extensions.Options;

namespace KaptchaNET.Services.Validation
{
    public class CaptchaValidationService : ICaptchaValidationService
    {
        private readonly ICaptchaStorageService _storage;
        private readonly CaptchaOptions _captchaOptions;

        public string ValidationMessage => "The captcha solution is invalid.";

        public CaptchaValidationService(ICaptchaStorageService storage, IOptions<CaptchaOptions> captchaOptions)
        {
            _storage = storage;
            _captchaOptions = captchaOptions?.Value;
        }

        public Task ValidateAsync(Guid id, string solution)
        {
            if (string.IsNullOrWhiteSpace(solution))
            {
                throw new CaptchaValidationException("Empty solution.");
            }

            Captcha captcha = _storage.GetCaptcha(id);
            if (captcha == null)
            {
                throw new CaptchaValidationException("Invalid solution.");
            }

            TimeSpan diff = DateTime.Now - captcha.Created;
            if (diff > _captchaOptions.Timeout)
            {
                throw new CaptchaTimeoutException();
            }
            if (captcha.Solution != solution)
            {
                throw new CaptchaValidationException("Invalid solution.");
            }

            _storage.RemoveCaptcha(id);

            return Task.CompletedTask;
        }
    }
}
