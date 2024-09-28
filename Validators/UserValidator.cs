using FluentValidation;
using RelatedEntities.Models;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Names)
            .NotEmpty().WithMessage("El nombre es requerido")
            .Length(2, 50).WithMessage("el nombre debe tener de 2 a 50 caracteres.");
        
        RuleFor(user => user.LastNames)
            .NotEmpty().WithMessage("El epellido es requerido.")
            .Length(2, 50).WithMessage("el apellido debe tener de 2 a 50 caracteres.");
        
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("El correo es requerido.")
            .EmailAddress().WithMessage("el correo no es valido.");
    }
}
