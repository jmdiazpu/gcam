using gcam.Frontend.Repositories;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace gcam.Frontend.Components.Pages.Companies;

public partial class CompanyCreate
{
    private Company company = new();
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("/api/companies", company);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }
        Return();
        Snackbar.Add("Empresa " + company.Name + " creada", Severity.Success);
    }

    private void Return()
    {
        NavigationManager.NavigateTo("/companies");
    }
}