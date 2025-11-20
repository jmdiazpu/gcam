using gcam.Frontend.Repositories;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace gcam.Frontend.Components.Pages.CompanyContacts;

public partial class CompanyContactEdit
{
    private CompanyContact? companyContact;
    private bool loading = true;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<CompanyContact>($"/api/companycontacts/{Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo($"/companies");
            }
            else
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(message, Severity.Error);
            }
        }
        else
        {
            companyContact = responseHttp.Response;
            loading = false;

            StateHasChanged();
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync($"/api/companyContacts/", companyContact);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add($"{message}", Severity.Error);
            return;
        }
        Return();
        Snackbar.Add("Contacto " + companyContact!.FullName + " modificado exitosamente.", Severity.Success);
    }

    private void Return()
    {
        NavigationManager.NavigateTo($"/companies/details/{companyContact!.CompanyId}");
    }

    private void InvalidForm()
    {
        Snackbar.Add("Por favor llenar todos los campos del formulario", Severity.Warning);
    }
}