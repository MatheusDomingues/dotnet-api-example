using api.Data.Context;
using api.Domain.Interfaces.Repositories;
using api.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.OpenApi.Models;
using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;
using api.Services;
using api.Domain.Interfaces.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var connectionString = builder.Configuration.GetConnectionString("CONNECTION_STRING");

builder.Services.AddDbContext<MyContext>(options => options.UseNpgsql(connectionString ?? throw new InvalidOperationException($"Connection string 'Connection' not found.")));

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks().AddDbContextCheck<MyContext>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    NullValueHandling = NullValueHandling.Ignore,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.UseInlineDefinitionsForEnums();
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"Não é necessário colocar o bearer no campo abaixo",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });

        var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

        var contact = new OpenApiContact()
        {
            Name = "Sportiva",
            Email = "contato@sportiva.app"
        };

        foreach (var description in provider.ApiVersionDescriptions)
        {
            var apiInfo = new OpenApiInfo()
            {
                Title = $"Sportiva Api",
                Version = description.ApiVersion.ToString(),
                Description = "Api utilizada para acessar funções relacionadas ao app Sportiva",
                Contact = contact,
            };

            options.SwaggerDoc(description.GroupName, apiInfo);
        }

        var appXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var appXmlPath = Path.Combine(AppContext.BaseDirectory, appXmlFile);

        // var coreXmlPath = Path.Combine(AppContext.BaseDirectory, "api.Domain.xml");

        if (string.IsNullOrEmpty(appXmlFile) is false)
            options.IncludeXmlComments(appXmlPath, true);

        // if (string.IsNullOrEmpty(coreXmlPath) is false)
        //     options.IncludeXmlComments(coreXmlPath);
    }
); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

            options.DocumentTitle = "Sportiva Api";
            options.DocExpansion(DocExpansion.None);
        });

    app.UseCors(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );

    app.Use((context, next) =>
    {
        context.Response.Headers.AccessControlAllowOrigin = "*";
        context.Response.Headers.AccessControlAllowMethods = "GET,HEAD,OPTIONS,POST,PUT";
        context.Response.Headers.AccessControlExposeHeaders = "Content-Disposition";
        context.Response.Headers.AccessControlAllowHeaders = "Origin, X-Requested-With, Content-Type, Accept, Authorization";

        return next.Invoke();
    });
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/healthz");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
