using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using PecuarioProPlatform.API.BusinessAdministration.Domain.Model.Commands;
using PecuarioProPlatform.API.BusinessAdministration.Domain.Model.Queries;
using PecuarioProPlatform.API.BusinessAdministration.Domain.Services;
using PecuarioProPlatform.API.BusinessAdministration.Interfaces.REST.Resources;
using PecuarioProPlatform.API.BusinessAdministration.Interfaces.REST.Transform;

namespace PecuarioProPlatform.API.BusinessAdministration.Interfaces.REST;



[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class BovinesController(IBovineCommandService bovineCommandService,
    IBovineQueryService bovineQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBovine([FromBody] CreateBovineResource createBovineResource)
    {
        var createBovineCommand = CreateBovineCommandFromResourceAssembler.ToCommandFromResource(createBovineResource);
        var bovine = await bovineCommandService.Handle(createBovineCommand);
        if (bovine is null) return BadRequest();
        var resource = BovineResourceFromEntityAssembler.ToResourceFromEntity(bovine);
        return CreatedAtAction(nameof(GetBovineById), new { bovineId = resource.Id }, resource);
    }
    
    
    [HttpGet("{bovineId:int}")]
    public async Task<IActionResult> GetBovineById([FromRoute] int bovineId)
    {
        var bovine = await bovineQueryService.Handle(new GetBovineByIdQuery(bovineId));
        Console.WriteLine(bovine);
        if (bovine == null) return NotFound();
        var resource = BovineResourceFromEntityAssembler.ToResourceFromEntity(bovine);
        Console.WriteLine(resource);
        return Ok(resource);
    }
    
    [HttpPut("{bovineId:int}")]
    public async Task<IActionResult> ModifyWeightBovine([FromRoute] int bovineId, [FromBody] ModifyWeightBovineResource modifyWeightBovineResource)
    {
        var modifyWeightBovineCommand = ModifyWeightBovineFromResourceAssembler.ToCommandFromResource(modifyWeightBovineResource, bovineId);
        var bovine = await bovineCommandService.Handle(modifyWeightBovineCommand);
        if (bovine is null) return BadRequest();
        var resource = BovineResourceFromEntityAssembler.ToResourceFromEntity(bovine);
        return CreatedAtAction(nameof(GetBovineById), new { bovineId = resource.Id }, resource);
    }
    //
    // [HttpPut("{bovineId:int}")]
    // public async Task<IActionResult> AddWeightRecord([FromRoute] int bovineId, [FromBody] AddNewWeightRecordResource addNewWeightRecordResource)
    // {
    //     var addNewWeightRecordCommand =
    //         AddWeightRecordToBovineCommandFromResourceAssembler.ToCommandFromResource(bovineId,
    //             addNewWeightRecordResource);
    //     var bovine = await bovineCommandService.Handle(addNewWeightRecordCommand);
    //     if (bovine is null) return BadRequest();
    //     var weightRecord = bovine.WeightRecords.Last();
    //     var resource = WeightRecordResourceFromEntityAssembler.ToResourceFromEntity(weightRecord);
    //     
    // }
    
    [HttpPut("{bovineId:int}/add-weight-record")]
    public async Task<IActionResult> AddWeightRecord([FromRoute] int bovineId, [FromBody] AddNewWeightRecordResource addNewWeightRecordResource)
    {
        var addNewWeightRecordCommand = AddWeightRecordToBovineCommandFromResourceAssembler.ToCommandFromResource(bovineId, addNewWeightRecordResource);
        var bovine = await bovineCommandService.Handle(addNewWeightRecordCommand);
        if (bovine is null) return BadRequest();
    
        var weightRecords = await bovineQueryService.Handle(new GetAllWeightRecordsByBovineIdQuery(bovineId));
        var resourceList = weightRecords.Select(WeightRecordResourceFromEntityAssembler.ToResourceFromEntity).ToList();

        return Ok(resourceList);
    }
    
    [HttpGet("{bovineId:int}/weight-records")]
    public async Task<IActionResult> GetAllWeightRecordsByBovineId([FromRoute] int bovineId)
    {
        var weightRecords = await bovineQueryService.Handle(new GetAllWeightRecordsByBovineIdQuery(bovineId));
        if (weightRecords is null) return NotFound();

        var resourceList = new List<WeightRecordResource>();
        foreach (var weightRecord in weightRecords)
        {
            var resource = WeightRecordResourceFromEntityAssembler.ToResourceFromEntity(weightRecord);
            resourceList.Add(resource);
        }

        return Ok(resourceList);
    }
    
}