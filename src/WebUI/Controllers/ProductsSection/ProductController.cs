using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.ProductsSection;
using ProductsArchive.Application.ProductsSection.Products.Commands;
using ProductsArchive.Application.ProductsSection.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ProductsArchive.WebUI.Controllers.ProductSection;

public class ProductController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<PaginatedList<ProductDto>>> GetWithPagination([FromQuery] GetProductsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto?>> Get(Guid id)
    {
        return await Mediator.Send(new GetProductQuery(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteProductCommand(id));

        return NoContent();
    }
}
