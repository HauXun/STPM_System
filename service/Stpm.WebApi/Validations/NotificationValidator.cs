using FluentValidation;
using Stpm.WebApi.Models.Notification;

namespace Stpm.WebApi.Validations;

public class NotificationValidator : AbstractValidator<NotificationEditModel>
{
    public NotificationValidator()
    {
        RuleFor(n => n.Title)
        .NotEmpty()
        .WithMessage("Tiêu đề của thông báo không được để trống")
        .MaximumLength(200)
        .WithMessage("Tiêu đề dài tối đa '{MaxLength}'");

        RuleFor(n => n.Content)
        .NotEmpty()
        .WithMessage("Nội dung về thông báo không được để trống");

        RuleFor(n => n.DueDate)
        .LessThan(DateTime.MaxValue)
        .WithMessage("Thời điểm phát thông báo không hợp lệ")
        .GreaterThan(DateTime.MinValue)
        .WithMessage("Thời điểm phát thông báo không hợp lệ");

        RuleFor(n => n.LevelId)
        .NotEmpty()
        .WithMessage("Phải chọn độ ưu tiên");
    }
}
