using FluentValidation;
using MyBlazorApp.Models;

namespace MyBlazorApp.Validators;

public class ChamadoValidator : AbstractValidator<AdicionarChamadoFormModel>
{
    private const long MaxFileSize = 10485760;

    public ChamadoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("O titlulo é obrigatória");

        RuleForEach(x => x.Arquivos).Must(file => file.Size <= MaxFileSize).WithMessage((model, file) => $"O arquivo {file.Name} excede o limite de 10MB.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<AdicionarChamadoFormModel>.CreateWithOptions((AdicionarChamadoFormModel)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid) return [];

        return result.Errors.Select(x => x.ErrorMessage);
    };
}
