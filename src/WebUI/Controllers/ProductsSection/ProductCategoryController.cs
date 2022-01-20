using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.ProductsSection;
using ProductsArchive.Application.ProductsSection.ProductCategories.Commands;
using ProductsArchive.Application.ProductsSection.ProductCategories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ProductsArchive.WebUI.Controllers.ProductSection;

public class ProductCategoryController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<PaginatedList<ProductCategoryDto>>> GetWithPagination([FromQuery] GetProductCategoriesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductCategoryDto?>> Get(Guid id)
    {
        return await Mediator.Send(new GetProductCategoryQuery(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateProductCategoryCommand command)
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
        await Mediator.Send(new DeleteProductCategoryCommand(id));

        return NoContent();
    }
}
