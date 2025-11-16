using gcam.Backend.UnitsOfWork.Interfaces;
using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gcam.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyContactsController : GenericController<CompanyContact>
{
    private readonly ICompanyContactsUnitOfWork _companyContactsUnitOfWork;

    public CompanyContactsController(IGenericUnitOfWork<CompanyContact> unitOfWork, ICompanyContactsUnitOfWork companyContactsUnitOfWork) : base(unitOfWork)
    {
        _companyContactsUnitOfWork = companyContactsUnitOfWork;
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _companyContactsUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecords")]
    public override async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _companyContactsUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var action = await _companyContactsUnitOfWork.GetAsync();
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("{id:int}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var action = await _companyContactsUnitOfWork.GetAsync(id);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return NotFound();
    }

    [AllowAnonymous]
    [HttpGet("combo/{companyId:int}")]
    public async Task<IActionResult> GetComboAsync(int companyId)
    {
        return Ok(await _companyContactsUnitOfWork.GetComboAsync(companyId));
    }
}