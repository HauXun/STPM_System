using FluentValidation;
using Stpm.WebApi.Models.ProjectTimeline;

namespace Stpm.WebApi.Validations;

public class ProjectTimelineValidator : AbstractValidator<ProjectTimelineEditModel>
{
    public ProjectTimelineValidator()
    {
        RuleFor(n => n.Title)
        .NotEmpty()
        .WithMessage("Tiêu đề của thông báo không được để trống")
        .MaximumLength(200)
        .WithMessage("Tiêu đề dài tối đa '{MaxLength}' kí tự");

        RuleFor(n => n.ShortDescription)
        .NotEmpty()
        .WithMessage("Mô tả ngắn về Project không được để trống")
        .MaximumLength(200)
        .WithMessage("Tiêu đề dài tối đa '{MaxLength}' kí tự");
    }
}
