using FluentValidation;
using Stpm.WebApi.Models.SpecificAward;

namespace Stpm.WebApi.Validations;

public class SpecificAwardValidator : AbstractValidator<SpecificAwardEditModel>
{
    public SpecificAwardValidator()
    {
        RuleFor(n => n.BonusPrize)
        .NotEmpty()
        .WithMessage("Giá trị giải thưởng phải thiết lập")
        .LessThan(int.MaxValue)
        .WithMessage("Giá trị giải thưởng không hợp lệ")
        .GreaterThan(0)
        .WithMessage("Giá trị giải thưởng không hợp lệ");

        RuleFor(n => n.Year)
        .NotEmpty()
        .WithMessage("Phải chọn năm tương ứng cho giải thưởng")
        .LessThan(short.MaxValue)
        .WithMessage("Năm không hợp lệ")
        .GreaterThan(short.MinValue)
        .WithMessage("Năm không hợp lệ");

        RuleFor(n => n.RankAwardId)
        .NotEmpty()
        .WithMessage("Phải chọn giải thưởng tương ứng");
    }
}
