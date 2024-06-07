using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using PecuarioProPlatform.API.Shared.Domain.Model.Queries;
using PecuarioProPlatform.API.Shared.Domain.Services;
using PecuarioProPlatform.API.Shared.Interfaces.REST.Resources;
using PecuarioProPlatform.API.Shared.Interfaces.REST.Transform;

namespace PecuarioProPlatform.API.Shared.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class DistrictController(IDistrictCommandService districtCommandService, IDistrictQueryService districtQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDistrict([FromBody] CreateDistrictResource createDistrictResource)
    {
        var createDistrictCommand = CreateDistrictCommandFromResourceAssembler.ToCommandFrontResource(createDistrictResource);
        var district = await districtCommandService.Handle(createDistrictCommand);
        if (district is null) return BadRequest();
        var resource = DistrictResourceFromEntityAssembler.ToResourceFromEntity(district);
        return CreatedAtAction(nameof(GetDistrictById), new { districtId = resource.Id }, resource);
    }

    [HttpGet("{districtId:int}")]
    public async Task<IActionResult> GetDistrictById([FromRoute] int districtId)
    {
        var district = await districtQueryService.Handle(new GetDistrictByIdQuery(districtId));
        if (district is null) return NotFound();
        var resource = DistrictResourceFromEntityAssembler.ToResourceFromEntity(district);
        return Ok(resource);
    }
}