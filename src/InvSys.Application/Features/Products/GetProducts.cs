using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace InvSys.Application.Features.Products;

[Route("/api/product")]
public class GetProducts : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] GetProductsQuery query)
    {
        // Mediator.Send()
        await Task.CompletedTask;
        return Ok();
    }

}

public record GetProductsQuery(
            int PageNumber,
            int PageSize) : IRequest<Result<List<Product>>>;
// TODO: implement paginated list

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<Product>>>
{
    public Task<Result<List<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}


