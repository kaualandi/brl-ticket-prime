using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TicketPrime.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient pointing to the API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5294") });

// MudBlazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();
