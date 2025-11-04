using gcam.Frontend.Repositories;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace gcam.Frontend.Components.Pages.Countries;

public partial class CountryCreate
{
    private Country country = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("/api/countries", country);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }
        Return();
        Snackbar.Add("País " + country.Name + " creado", Severity.Success);
    }

    private void Return()
    {
        NavigationManager.NavigateTo("/countries");
    }
}