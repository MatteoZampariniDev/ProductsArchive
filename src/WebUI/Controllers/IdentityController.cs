using Microsoft.AspNetCore.Mvc;
using ProductsArchive.Application.Common.Interfaces;
using ProductsArchive.Application.Identity.Queries;

namespace ProductsArchive.WebUI.Controllers;

public class IdentityController : ApiControllerBase
{
    private readonly ICurrentUserService _currentUser;

    public IdentityController(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<string?>> GetRole(Guid id)
    {
        return await Mediator.Send(new GetUserRoleQuery(id));
    }

    [HttpGet()]
    public  ActionResult<string?> GetUserId()
    {
        return _currentUser.UserId;
    }
}
