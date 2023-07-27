using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace InvSys.Application.Features.Products;

[Route("/api/product")]
public class CreateProduct : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(CreateProductCommand command)
    {
        var commandResult = await Mediator.Send(command);

        if (commandResult.IsFailed)
        {
            return Problem();
        }

        return Ok(commandResult.Value);
    }
}

public record CreateProductCommand(
        string Name,
        string SKU,
        string Condition,
        string Location,
        int AvailableQuantity,
        int StockQuantity,
        double Price) : IRequest<Result<ProductResult>>;

public record ProductResult(Product Product);

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductResult>>
{
    private readonly List<Product> _products;

    public CreateProductCommandHandler()
    {
        _products = new List<Product>();
    }

    public async Task<Result<ProductResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var product = Product.Create(
                request.Name,
                request.SKU,
                request.Condition,
                ProductLocation.WAREHOUSE1,
                request.AvailableQuantity,
                request.StockQuantity,
                request.Price);

        _products.Add(product);

        return new ProductResult(product);
    }
}
