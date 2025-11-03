using gcam.Frontend.Repositories;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace gcam.Frontend.Components.Pages.Countries;

public partial class CountriesIndex
{
    [Inject] private IRepository Repository { get; set; } = null!;
    private List<Country>? countries;

    protected override async Task OnInitializedAsync()
    {
        var HttpResult = await Repository.GetAsync<List<Country>>("/api/countries");
        countries = HttpResult.Response;
    }
}