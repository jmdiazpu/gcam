using gcam.Frontend.Components.Pages.CompanyContacts;
using gcam.Frontend.Components.Pages.Shared;
using gcam.Frontend.Repositories;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Charts;
using MudBlazor.Extensions;
using System.Net;

namespace gcam.Frontend.Components.Pages.Companies;

public partial class CompanyDetails
{
    private Company? company;
    private List<CompanyContact>? contacts;

    private MudTable<CompanyContact> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrl = "api/companycontacts";
    private string infoFormat = "Registros {first_item} al {last_item} de {all_items}";

    [Parameter] public int CompanyId { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        await LoadTotalRecordsAsync();
    }

    private async Task<bool> LoadTotalRecordsAsync()
    {
        loading = true;
        if (company is null)
        {
            var ok = await LoadCompanyAsync();
            if (!ok)
            {
                NoCompany();
                return false;
            }
        }

        var url = $"{baseUrl}/totalRecords?id={CompanyId}";
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var respondeHttp = await Repository.GetAsync<int>(url);
        if (respondeHttp.Error)
        {
            var message = await respondeHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return false;
        }

        totalRecords = respondeHttp.Response;
        loading = false;
        return true;
    }

    private async Task<bool> LoadCompanyAsync()
    {
        var responseHttp = await Repository.GetAsync<Company>($"/api/companies/{CompanyId}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/companies");
                return false;
            }

            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return false;
        }

        company = responseHttp.Response;
        return true;
    }

    private async Task<TableData<CompanyContact>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;
        int pageSize = state.PageSize;
        var url = $"{baseUrl}/paginated?id={CompanyId}&page={page}&recordsnumber={pageSize}";
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<CompanyContact>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return new TableData<CompanyContact> { Items = [], TotalItems = 0 };
        }

        if (responseHttp.Response == null)
        {
            return new TableData<CompanyContact> { Items = [], TotalItems = 0 };
        }
        return new TableData<CompanyContact>
        {
            Items = responseHttp.Response,
            TotalItems = totalRecords
        };
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await LoadAsync();
        await table.ReloadServerData();
    }

    private void ReturnAction()
    {
        NavigationManager.NavigateTo("/companies");
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
            };
            dialog = await DialogService.ShowAsync<CompanyContactEdit>("Editar contacto", parameters, options);
        }
        else
        {
            var parameters = new DialogParameters
            {
                { "CompanyId", CompanyId }
            };
            dialog = await DialogService.ShowAsync<CompanyContactCreate>("Nuevo contacto", parameters, options);
        }

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();
            await table.ReloadServerData();
        }
    }

    private async Task DeleteAsync(CompanyContact contact)
    {
        var parameters = new DialogParameters
        {
            {"Message", $"¿Realmente quieres eliminar el contacto: {contact.FullName}?" }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmación", parameters, options);
        var result = await dialog.Result;
        if (result!.Canceled)
        {
            return;
        }

        var responseHttp = await Repository.DeleteAsync($"api/companycontacts/{contact.Id}");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message, Severity.Error);
            return;
        }
        await LoadAsync();
        await table.ReloadServerData();
        Snackbar.Add("Contacto " + contact.FullName + " eliminado.", Severity.Success);
    }

    private void NoCompany()
    {
        NavigationManager.NavigateTo("/companies");
    }
}