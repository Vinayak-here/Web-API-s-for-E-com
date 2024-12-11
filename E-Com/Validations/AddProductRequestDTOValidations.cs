using E_Com.DTO_s.RequestDTO_s;
using FluentValidation;

namespace E_Com.Validations
{
    public class AddProductRequestDTOValidations : AbstractValidator<AddProductRequestDTO>
    {
        public AddProductRequestDTOValidations() 
        {
            RuleFor(x => x.SellerId)
                .NotNull().WithMessage("Seller Id is Required")
                .GreaterThan(0).WithMessage("Seller Id can't be Lesser Than 1");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Please Enter the Product  Name for adding Product");
        
        }
    }
}
