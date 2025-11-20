using gcam.Frontend.Repositories;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace gcam.Frontend.Components.Pages.Companies;

public partial class CompanyEdit
{
    private Company? company;
    private List<Country>? countries;
    private List<State>? states;
    private List<City>? cities;
    private bool loading = true;

    private Country selectedCountry = new();
    private State selectedState = new();
    private City selectedCity = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    // Minimal DTO for updates to avoid sending navigation properties
    private sealed record CompanyUpdateDto(int Id, string Name, string Address, int CityId);

    protected override async Task OnInitializedAsync()
    {
        await LoadCompanyAsync();
        await LoadCountriesAsync();
        await LoadStatesAsync(company!.City!.State!.Country!.Id);
        await LoadCitiesAsync(company!.City!.State!.Id);

        selectedCountry = company!.City!.State!.Country;
        selectedState = company!.City!.State;
        selectedCity = company!.City;
    }

    private async Task LoadCompanyAsync()
    {
        var responseHttp = await Repository.GetAsync<Company>($"/api/companies/{Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/companies");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError!, Severity.Error);
                return;
            }
        }
        else
        {
            company = responseHttp.Response;
            loading = false;
        }
    }

    private async Task LoadCountriesAsync()
    {
        var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }
        countries = responseHttp.Response;
    }

    private async Task LoadStatesAsync(int countryId)
    {
        var responseHttp = await Repository.GetAsync<List<State>>($"/api/states/combo/{countryId}");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }
        states = responseHttp.Response;
    }

    private async Task LoadCitiesAsync(int stateId)
    {
        var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo/{stateId}");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }
        cities = responseHttp.Response;
    }

    private async Task CountryChangesAsync(Country country)
    {
        selectedCountry = country;
        selectedState = new State();
        selectedCity = new City();
        states = null;
        cities = null;
        await LoadStatesAsync(country.Id);
    }

    private async Task StateChangeAsync(State state)
    {
        selectedState = state;
        selectedCity = new City();
        cities = null;
        await LoadCitiesAsync(state.Id);
    }

    private void CityChangeAsync(City city)
    {
        selectedCity = city;
        company!.CityId = city.Id;
    }

    private async Task<IEnumerable<Country>> SearchCountries(string searchText, CancellationToken token)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return countries!;
        }

        return countries!
            .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private async Task<IEnumerable<State>> SearchStates(string searchText, CancellationToken token)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return states!;
        }
        return states!
            .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private async Task<IEnumerable<City>> SearchCities(string searchText, CancellationToken token)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return cities!;
        }

        return cities!
            .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private void InvalidForm()
    {
        Snackbar.Add("Por favor llenar todos los campos del formulario", Severity.Warning);
    }

    private async Task EditAsync()
    {
        if (company == null) return;

        // Build a minimal DTO to send to the API (avoid navigation props)
        var dto = new CompanyUpdateDto(company.Id, company.Name, company.Address, company.CityId);
        var responseHttp = await Repository.PutAsync($"/api/companies/", dto);
        if (responseHttp.Error)
        {
            var messageError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(messageError!, Severity.Error);
            return;
        }
        Return();
        Snackbar.Add("Empresa " + company.Name + " modificada con éxito.", Severity.Success);
    }

    private void Return()
    {
        NavigationManager.NavigateTo("/companies");
    }
}