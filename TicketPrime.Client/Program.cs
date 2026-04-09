using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TicketPrime.Client;
using TicketPrime.Client.Services.Cupons;
using TicketPrime.Client.Services.Eventos;
using TicketPrime.Client.Services.Usuarios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient pointing to the API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5201") });

// Coupon flow connected to API.
builder.Services.AddScoped<ICupomService, ApiCupomService>();

// Evento flow connected to API.
builder.Services.AddScoped<IEventoService, ApiEventoService>();

// Usuario flow connected to API.
builder.Services.AddScoped<IUsuarioService, ApiUsuarioService>();

// MudBlazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();
