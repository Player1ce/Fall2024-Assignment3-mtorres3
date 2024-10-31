using Fall2024_Assignment3_mtorres3.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//var apiKey = builder.Configuration["AZURE_OPENAI_KEY"];
//var endpoint = builder.Configuration["AZURE_OPENAI_ENDPOINT"];
//var deploymentName = builder.Configuration["DEPLOYMENT_NAME"];

//if (string.IsNullOrEmpty(apiKey))
//{
//    Console.WriteLine("ApiKey is null or empty");
//}

//Console.WriteLine($"ApiKey: {apiKey}");
//Console.WriteLine($"endpoint: {endpoint}");
//Console.WriteLine($"deploymentName: {deploymentName}");

builder.Services.AddSingleton<ChatClient>(sp =>
{
    var apiKey = builder.Configuration["AZURE_OPENAI_KEY"];
    var endpoint = builder.Configuration["AZURE_OPENAI_ENDPOINT"];
    var deploymentName = builder.Configuration["DEPLOYMENT_NAME"];
    
    //Console.WriteLine("In Singleton Service.");
    //Console.WriteLine($"ApiKey: {apiKey}");
    //Console.WriteLine($"endpoint: {endpoint}");
    //Console.WriteLine($"deploymentName: {deploymentName}");

    var azureClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
    return azureClient.GetChatClient(deploymentName);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
