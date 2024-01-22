using System.Text.Json.Serialization;
using Microsoft.AspNetCore.StaticFiles;
using Stylophonix.Interfaces;
using Stylophonix.Interfaces.Controllers;
using Stylophonix.Services;
using Stylophonix.Services.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddScoped<IAnimationService, AnimationService>();
builder.Services.AddScoped<IDataControllerService, DataControllerService>();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var provider = new FileExtensionContentTypeProvider
{
    Mappings =
    {
        [".avif"] = "image/avif"
    }
};

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();