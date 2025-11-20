using gcam.Frontend.Repositories;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace gcam.Frontend.Components.Pages.CompanyContacts;

public partial class CompanyContactCreate
{
    private CompanyContact companyContact = new();
    private bool loading;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter] public int CompanyId { get; set; }

    private async Task CreateAsync()
    {
        companyContact.CompanyId = CompanyId;
        var responseHttp = await Repository.PostAsync("/api/companyContacts", companyContact);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message, Severity.Error);
            return;
        }

        Return();
        Snackbar.Add("Contato " + companyContact.FullName + " registrado.", Severity.Success);
    }

    private void InvalidForm()
    {
        Snackbar.Add("Por favor llenar todos los campos del formulario", Severity.Warning);
    }

    private void Return()
    {
        NavigationManager.NavigateTo($"/companies/details/{CompanyId}");
    }
}