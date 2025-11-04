using gcam.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace gcam.Frontend.Components.Pages.Countries;

public partial class CountryForm
{
    private EditContext EditContext = null!;
    [EditorRequired, Parameter] public Country Country { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    protected override void OnInitialized()
    {
        EditContext = new(Country);
    }
}