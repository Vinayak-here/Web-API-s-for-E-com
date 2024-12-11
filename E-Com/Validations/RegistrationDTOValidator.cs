using E_Com.DTO_s.RequestDTO_s;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_Com.Validations
{
    public class RegistrationDTOValidator : AbstractValidator<RegistrationRequestDTO>
    {
        public RegistrationDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage($"First Name is required." )
                .MaximumLength(30).WithMessage("First Name must not exceed 30 characters.");

            RuleFor(x => x.LastName)
                .MaximumLength(30).WithMessage("Last Name must not exceed 30 characters.");

            RuleFor(x => x.Phonenumber)
                .Matches(@"^\+?\d+$").WithMessage("Phone number must contain only digits and can start with a '+' sign.")
                .When(x => !string.IsNullOrEmpty(x.Phonenumber));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .Matches(@".*\d{3,}.*").WithMessage("Password must contain at least 3 numbers.")
                .Matches(@"[!@#$%^&*(),.?""{}|<>]").WithMessage("Password must contain at least 1 special character.")
                .NotEmpty().WithMessage("Password is required.").MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    
    }
}
