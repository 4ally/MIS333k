//add a using statement for currency
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Tam_Allyson_HW3.DAL;
//create a web application builder
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

//Add the database
String connectionString = "Server=tcp:fa24tamallysonhw3.database.windows.net,1433;Initial Catalog=fa24TamAllysonhw3;Persist Security Info=False;User ID=MISadmin;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

//Connect to the database
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//build the app
var app = builder.Build();
//These lines allow you to see more detailed error messages
app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
//This line allows you to use static pages like style sheets and images
app.UseStaticFiles();
//This marks the position in the middleware pipeline where a routing decision
//is made for a URL.
app.UseRouting();
//This allows the data annotations for currency to work on Macs
app.Use(async (context, next) =>
{
    CultureInfo.CurrentCulture =
    System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
    CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
    await next.Invoke();
});
//TODO: (HW4 & Beyond) Once you have added Identity into your project, you will
//need to uncomment these lines
//app.UseAuthentication();
//app.UseAuthorization();
//This method maps the controllers and their actions to a patter for
//requests that's known as the default route. This route identifies
//the Home controller as the default controller and the Index() action
//method as the default action. The default route also identifies a
//third segment of the URL that's a parameter named id.
app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
