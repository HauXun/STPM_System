using FluentValidation;
using Stpm.WebApi.Models.AppUser;

namespace Stpm.WebApi.Validations;

public class UserValidator : AbstractValidator<AppUserEditModel>
{
    public UserValidator()
    {
        RuleFor(n => n.UserName)
        .MaximumLength(50)
        .WithMessage("Tài khoản dài tối đa '{MaxLength}' kí tự");

        RuleFor(n => n.Email)
        .EmailAddress()
        .WithMessage("Email không hợp lệ");

        RuleFor(n => n.PhoneNumber)
        .Matches("^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$")
        .WithMessage("Số điện thoại không hợp lệ")
        .MaximumLength(50)
        .WithMessage("Số điện thoại dài tối đa '{MaxLength}' kí tự");
    }
}
