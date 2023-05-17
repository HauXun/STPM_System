using FluentValidation;
using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Validations;

public class TopicValidator : AbstractValidator<TopicEditModel>
{
    public TopicValidator()
    {
        RuleFor(n => n.TopicName)
        .NotEmpty()
        .WithMessage("Tiêu đề không được để trống")
        .MaximumLength(200)
        .WithMessage("Tiêu đề dài tối đa '{MaxLength}' kí tự");

        RuleFor(n => n.TopicRankId)
        .NotEmpty()
        .WithMessage("Phải chọn hạng mục của đề tài");
    }
}
