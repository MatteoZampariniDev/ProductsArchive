using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.ProductsSection;
using ProductsArchive.Application.ProductsSection.ProductSizes.Commands;
using ProductsArchive.Application.ProductsSection.ProductSizes.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ProductsArchive.WebUI.Controllers.ProductSection;

public class ProductSizeController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductSizeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<PaginatedList<ProductSizeDto>>> GetWithPagination([FromQuery] GetProductSizesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductSizeDto>> Get(Guid id)
    {
        return await Mediator.Send(new GetProductSizeQuery(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateProductSizeCommand command)
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
        await Mediator.Send(new DeleteProductSizeCommand(id));

        return NoContent();
    }
}
