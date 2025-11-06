using gcam.Backend.UnitsOfWork.Interfaces;
using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace gcam.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : GenericController<Company>
{
    private readonly ICompaniesUnitOfWork _companiesUnitOfWork;

    public CompaniesController(IGenericUnitOfWork<Company> unitOfWork, ICompaniesUnitOfWork companiesUnitOfWork) : base(unitOfWork)
    {
        _companiesUnitOfWork = companiesUnitOfWork;
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _companiesUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecords")]
    public override async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _companiesUnitOfWork.GetTotalRecordsAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var action = await _companiesUnitOfWork.GetAsync();
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("{id:int}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var action = await _companiesUnitOfWork.GetAsync(id);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return NotFound();
    }
}