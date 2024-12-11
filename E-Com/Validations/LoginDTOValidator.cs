using E_Com.DTO_s.RequestDTO_s;
using FluentValidation;

namespace E_Com.Validations
{
    public class LoginDTOValidator : AbstractValidator<LoginRequestDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Username)
            .NotEmpty().WithMessage("User Name is Required for Login");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is Required"); 
        }
    
    }
}
