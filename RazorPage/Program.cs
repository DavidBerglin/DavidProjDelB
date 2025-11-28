using Configuration.Extensions;
using DbRepos;
using DbContext;
using Encryption.Extensions;
using Seido.Utilities.SeedGenerator;
using Services;
using Services.Interfaces;
using DbContext.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<SeedGenerator>();
builder.Configuration.AddSecrets(builder.Environment);
builder.Services.AddEncryptions(builder.Configuration);
builder.Services.AddDatabaseConnections(builder.Configuration);
builder.Services.AddUserBasedDbContext();

builder.Services.AddScoped<AdminDbRepos>();
builder.Services.AddScoped<FriendsDbRepos>();
builder.Services.AddScoped<AddressesDbRepos>();
builder.Services.AddScoped<PetsDbRepos>();
builder.Services.AddScoped<QuotesDbRepos>();

builder.Services.AddScoped<IAdminService, AdminServiceDb>();
builder.Services.AddScoped<IFriendsService, FriendsServiceDb>();
builder.Services.AddScoped<IAddressesService, AddressesServiceDb>();
builder.Services.AddScoped<IPetsService, PetsServiceDb>();
builder.Services.AddScoped<IQuotesService, QuotesServiceDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
