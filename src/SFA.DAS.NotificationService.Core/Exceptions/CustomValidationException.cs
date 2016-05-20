using System;
using FluentValidation.Results;

namespace SFA.DAS.NotificationService.Application.Exceptions
{
    public class CustomValidationException : ApplicationException
    {
        public CustomValidationException(ValidationResult validationResult) : base()
        {
            ValidationResult = validationResult;
        }

        public ValidationResult ValidationResult { get; set; }
    }
}