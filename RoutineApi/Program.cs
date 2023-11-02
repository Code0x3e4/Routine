using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using RoutineApi.Data;
using RoutineApi.Services;
using RoutineApi.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable = true;
    option.CacheProfiles.Add("120sCacheProfile", new CacheProfile
    {
        Duration = 120
    });
})
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new Microsoft.AspNetCore.Mvc.ValidationProblemDetails(context.ModelState)
            {
                Type = "http://www.bing.com",
                Title = "´íÎó±êÌâ",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = "ÏêÇé",
                Instance = context.HttpContext.Request.Path,
            };

            problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
            return new UnprocessableEntityObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        };
    })
    .AddNewtonsoftJson(setup =>
    {
        setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    })
    .AddXmlDataContractSerializerFormatters();

builder.Services.AddDbContext<RoutineDbContext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("DemoSQLite"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpCacheHeaders(expires =>
{
    expires.MaxAge = 60;
    expires.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
}, validation =>
{
    validation.MustRevalidate = true;
});
builder.Services.AddResponseCaching();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddTransient<IPropertyMappingService, PropertyMappingService>();
builder.Services.AddTransient<IPropertyCheckerServic, PropertyCheckerServic>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetService<RoutineDbContext>();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }
    catch (Exception e)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "Database Migration Error!");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(builder =>
    {
        builder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Unexpected Error!");
        });
    });
}
//app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

