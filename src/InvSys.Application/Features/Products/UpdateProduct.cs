using System.Reflection;

using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;
using InvSys.Application.Infrastructure;
using InvSys.Application.ValueObjects;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Application.Features.Products;

[Route("/api/product")]
public class UpdateProductController : ApiControllerBase
{
    private readonly IMapper _mapper;

    public UpdateProductController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromQuery] Guid productId, ProductUpdateCommand command)
    {
        // var command = _mapper.Map<ProductUpdateCommand>((productId, request));
        command.ProductId = productId;
        var updateResult = await Mediator.Send(command);

        if (updateResult.IsFailed)
        {
            return Problem();
        }

        return Ok(updateResult.Value);
    }
}

public record ProductUpdateResult(Product Product);

public record ProductUpdateCommand : IRequest<Result<ProductUpdateResult>>
{
    public ProductUpdateCommand(
        Guid productId,
        string condition,
        string name,
        int availableQuantity,
        int stockQuantity,
        double price)
    {
        ProductId = productId;
        Condition = condition;
        Name = name;
        AvailableQuantity = availableQuantity;
        StockQuantity = stockQuantity;
        Price = price;
    }

    public Guid ProductId { get; set; }
    public string Condition { get; set; }
    public string Name { get; set; }
    public int AvailableQuantity { get; set; }
    public int StockQuantity { get; set; }
    public double Price { get; set; }
}

public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, Result<ProductUpdateResult>>
{
    private readonly InvSysDbContext _dbContext;

    public ProductUpdateCommandHandler(InvSysDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ProductUpdateResult>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        // get the product with the same sku
        // create a new product 
        // populate new product with product on hand read-only properties and request new property values
        // delete the product on hand
        // add the new product
        // save changes

        var updatingProperties = request.GetType().GetProperties();

        var product = await _dbContext.Products
            .FindAsync(ProductId.Create(request.ProductId), cancellationToken);

        // how can i improve this?
        product!.Condition = request.Condition == default ? product.Condition : request.Condition;
        product.Name = request.Name == default ? product.Name : request.Name;
        product.AvailableQuantity = request.AvailableQuantity == default ? product.AvailableQuantity : ProductQuantity.Create(request.AvailableQuantity);
        product.StockQuantity = request.StockQuantity == default ? product.StockQuantity : ProductQuantity.Create(request.StockQuantity);
        product.Price = request.Price == default ? product.Price : ProductPrice.Create(request.Price);

        product.DomainEvents.Add(new ProductUpdatedEvent(product));

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ProductUpdateResult(product);
    }
}

public class ProductUpdatedEvent : DomainEvent
{
    public ProductUpdatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}