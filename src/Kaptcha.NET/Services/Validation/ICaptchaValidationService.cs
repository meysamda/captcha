using System;
using System.Threading.Tasks;

namespace KaptchaNET.Services.Validation
{
    public interface ICaptchaValidationService
    {
        Task ValidateAsync(Guid id, string solution);

        string ValidationMessage { get; }
    }
}
