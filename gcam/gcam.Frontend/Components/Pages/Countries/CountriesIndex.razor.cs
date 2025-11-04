using gcam.Frontend.Components.Pages.Shared;
using gcam.Frontend.Repositories;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace gcam.Frontend.Components.Pages.Countries;

public partial class CountriesIndex
{
    private List<Country>? Countries { get; set; }
    private MudTable<Country> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int TotalRecords;
    private bool Loading;
    private const string BaseUrl = "api/countries";
    private string infoFormat = "Registros {first_item} al {last_item} de {all_items}";

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string? Filter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadTotalRecordsAsync();
    }

    private async Task LoadTotalRecordsAsync()
    {
        Loading = true;
        var url = $"{BaseUrl}/totalRecords";
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"?filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<int>(url);
        if (responseHttp.Error)
        {
            var Message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Message!, Severity.Error);
            return;
        }
        TotalRecords = responseHttp.Response;
        Loading = false;
    }

    private async Task<TableData<Country>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;
        int pageSize = state.PageSize;
        var url = $"{BaseUrl}/paginated/?page={page}&recordsnumber={pageSize}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<Country>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return new TableData<Country> { Items = [], TotalItems = 0 };
        }
        if (responseHttp.Response == null)
        {
            return new TableData<Country> { Items = [], TotalItems = 0 };
        }
        return new TableData<Country>
        {
            Items = responseHttp.Response,
            TotalItems = TotalRecords
        };
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await LoadTotalRecordsAsync();
        await table.ReloadServerData();
    }

    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true
        };
        IDialogReference? dialog;
        if (isEdit)
        {
            var parameters = new DialogParameters
            {
                { "Id", id }
            }; dialog = await DialogService.ShowAsync<CountryEdit>("Editar país", parameters, options);
        }
        else
        {
            dialog = await DialogService.ShowAsync<CountryCreate>("Nuevo país", options);
        }

        var result = await dialog.Result;
        if (result!.Canceled!)
        {
            await LoadTotalRecordsAsync();
            await table.ReloadServerData();
        }
    }

    private async Task DeleteAsync(Country country)
    {
        var parameters = new DialogParameters
        {
            { "Message", $"¿Estás seguro de querer borrar el país {country.Name}?" }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmación", parameters, options);
        var result = await dialog.Result;
        if (result!.Canceled)
        {
            return;
        }

        var responseHttp = await Repository.DeleteAsync($"{BaseUrl}/{country.Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/countries");
            }
            else
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(message!, Severity.Error);
            }
            return;
        }
        await LoadTotalRecordsAsync();
        await table.ReloadServerData();
        Snackbar.Add("País " + @country.Name + " borrado", Severity.Success);
    }
}