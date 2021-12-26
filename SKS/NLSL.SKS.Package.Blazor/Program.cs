using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using NLSL.SKS.Package.Blazor;
using NLSL.SKS.Package.Blazor.Helper;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var httpClient = new HttpClient { BaseAddress = new Uri("https://nlsl.azurewebsites.net/")};

builder.Services.AddScoped(sp =>  httpClient);
builder.Services.AddTransient(sp => new HttpHelper(httpClient));

await builder.Build().RunAsync();