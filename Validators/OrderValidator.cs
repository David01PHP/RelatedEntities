using FluentValidation;
using RelatedEntities.Models;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(order => order.ProductId)
            .GreaterThan(0).WithMessage("A valid product ID is required.");
        
        RuleFor(order => order.UserId)
            .GreaterThan(0).WithMessage("A valid user ID is required.");
    }
}
