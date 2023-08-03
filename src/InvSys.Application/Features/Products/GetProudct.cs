using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;
using InvSys.Application.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Application.Features.Products;

[Route("/api/product")]
public class GetProductController : ApiControllerBase
{
    [HttpGet(Name = "")]
    public async Task<ActionResult> Get([FromQuery] GetProductQuery query)
    {
        var queryResult = await Mediator.Send(query);

        if (queryResult.IsFailed)
        {
            return Problem(queryResult.Errors.Select(r => r.Metadata));
        }

        return Ok(queryResult.Value);
    }
}

public record GetProductQuery(Guid ProductId) : IRequest<Result<Product>>;
// TODO: implement paginated list

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<Product>>
{
    private readonly InvSysDbContext _dbContext;

    public GetProductQueryHandler(InvSysDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync(ProductId.Create(request.ProductId));

        if (product is not null)
        {
            return product;
        }

        return Result.Fail($"Product with ID: {request.ProductId.ToString()} is not found");
    }
}