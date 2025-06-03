using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    // Set default session timeout (optional)
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout in 30 minutes
    options.Cookie.HttpOnly = true; // Ensures the session cookie is only accessible by the server
    options.Cookie.IsEssential = true; // Make the cookie essential for the app
});

// Register HttpClient with Basic Authentication
builder.Services.AddHttpClient("ApiWithBasicAuth", client =>
{
    var username = "admin";
    var password = "password";
    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
    var base64String = Convert.ToBase64String(byteArray);

    client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64String}");


    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Enable session middleware
app.UseSession();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
