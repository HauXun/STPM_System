using FluentValidation;
using Stpm.WebApi.Models.Timeline;

namespace Stpm.WebApi.Validations;

public class TimelineValidator : AbstractValidator<TimelineEditModel>
{
    public TimelineValidator()
    {
        RuleFor(n => n.Title)
        .NotEmpty()
        .WithMessage("Tiêu đề không được để trống")
        .MaximumLength(200)
        .WithMessage("Tiêu đề dài tối đa '{MaxLength}' kí tự");

        RuleFor(n => n.ShortDescription)
        .NotEmpty()
        .WithMessage("Mô tả Timeline không được để trống")
        .MaximumLength(200)
        .WithMessage("Tiêu đề dài tối đa '{MaxLength}' kí tự");

        RuleFor(n => n.DueDate)
        .LessThan(DateTime.MaxValue)
        .WithMessage("Thời điểm không hợp lệ")
        .GreaterThan(DateTime.MinValue)
        .WithMessage("Thời điểm không hợp lệ");

        RuleFor(n => n.ProjectId)
        .NotEmpty()
        .WithMessage("Phải chọn Project của Timeline");
    }
}
