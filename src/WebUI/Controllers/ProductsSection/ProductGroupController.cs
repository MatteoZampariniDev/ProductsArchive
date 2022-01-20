using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.ProductsSection;
using ProductsArchive.Application.ProductsSection.ProductGroups.Commands;
using ProductsArchive.Application.ProductsSection.ProductGroups.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ProductsArchive.WebUI.Controllers.ProductSection;

public class ProductGroupController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductGroupCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<PaginatedList<ProductGroupDto>>> GetWithPagination([FromQuery] GetProductGroupsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductGroupDto?>> Get(Guid id)
    {
        return await Mediator.Send(new GetProductGroupQuery(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateProductGroupCommand command)
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
        await Mediator.Send(new DeleteProductGroupCommand(id));

        return NoContent();
    }
}
