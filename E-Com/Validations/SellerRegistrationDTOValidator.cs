using E_Com.DTO_s.RequestDTO_s;
using FluentValidation;

namespace E_Com.Validations
{
    public class SellerRegistrationDTOValidator : AbstractValidator<SellerRegistrationRequestDTO>

    {
        public SellerRegistrationDTOValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Please eneter UserId");

            RuleFor(x => x.SellerName)
                .NotEmpty().WithMessage("Please Mention the name");


            RuleFor(s => s.GSTnumber)
                .NotEmpty().WithMessage("GST Number is required.")
                .Matches(@"^GST\d{10}$").WithMessage("GST Number must start with 'GST' and be followed by Number");
        }

    }
}
