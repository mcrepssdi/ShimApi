using Newtonsoft.Json;
using NLog;
using SdiHttpLib;
using SdiUtility;
using ShimApi.DataProviders;
using ShimApi.Enumerations;
using ShimApi.Models;
using ShimApi.Services;
using ShimApi.Services.Rimas;
using ShimApi.ShimConfigMgr;
using ShimApi.ShimConfigMgr.Sections;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
IShimConfigurations config = new ApiConfig(configuration);

Logger logger = LogManager.GetCurrentClassLogger();
GlobalDiagnosticsContext.Set("logDirectory", config.Environment.LogDirectory);
LogManager.ReconfigExistingLoggers();

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

IMySqlProvider mysqlProvider = new SaiProvider(config.SaiConfigurations.Database.ConnectionStr);
(SqlState sqlState, string token) = mysqlProvider.SaiServiceToken(
    config.SaiConfigurations.Database.Schema, logger: logger);
if(sqlState != SqlState.Ok || token.IsEmpty())
{
    throw new ArgumentNullException($"APIParams", "API Params cannot be null or empty");
}

ApiParameters parameters = new()
{
    Token = token,
    ServiceUrl = config.SaiConfigurations.WebServices.InternalServicePath,
    ServicePath = config.SaiConfigurations.WebServices.InternalServerName,
};

RimasApi rimaApi = new()
{
    Token = string.Empty,
    RimasUser = config.RimasConfigurations.RimasAPI.RimasUser,
    RimasPwd = config.RimasConfigurations.RimasAPI.RimasPwd,
    Company = config.RimasConfigurations.RimasAPI.Company,
    Division = config.RimasConfigurations.RimasAPI.Division,
    ServicePath = config.RimasConfigurations.RimasAPI.ServicePath,
    ServiceUrl = config.RimasConfigurations.RimasAPI.ServiceUrl
};

JsonSerializerSettings jsonSerializerSettings = new()
{
    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
};

VersionUtil vu = new();
logger.Debug($"=========================== Branch:            {vu.GitBranch}");
logger.Debug($"=========================== Commit ID:         {vu.CommitId}");
logger.Debug($"=========================== App Name:          {config.Environment.ApplicationName}");
logger.Debug($"=========================== Prod:              {builder.Environment.IsProduction()}");
logger.Debug($"=========================== Staging:           {builder.Environment.IsStaging()}");
logger.Debug($"=========================== Development:       {builder.Environment.IsDevelopment()}");
logger.Debug($"=========================== SAI Environment:   {config.SaiConfigurations.Database.Schema}");
logger.Debug($"=========================== SAI Service Path:  {config.SaiConfigurations.WebServices.InternalServicePath}");
logger.Debug($"=========================== SAI Service Name:  {config.SaiConfigurations.WebServices.InternalServerName}");
logger.Debug($"=========================== SAI Service Token: {(token.IsEmpty() ? "Token Missing" : "Token Found")}");
logger.Debug($"=========================== SAI Service URL:   {parameters.EndPoint(string.Empty)}");
logger.Debug($"=========================== RIMAS Service URL: {rimaApi.EndPoint(string.Empty)}");
logger.Debug($"=========================== RIMAS Company:     {rimaApi.Company}");
logger.Debug($"=========================== RIMAS Division:    {rimaApi.Division}");
logger.Debug($"=========================== RIMAS Schema:      {rimaApi.Schema}");
logger.Debug($"=========================== RIMAS Service URL: {rimaApi.EndPoint(string.Empty)}");

// Add services to the container.
builder.Services.AddSingleton(config);
builder.Services.AddSingleton( new ApiProperties()
{
    DataEnvironmentName = config.SaiConfigurations.Database.Schema,
    ApiParameters =parameters,
    RimasApi = rimaApi
});
builder.Services.AddSingleton(jsonSerializerSettings);
builder.Services.AddScoped<IHttp, SdiHttp>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IShipmentService<SaiShipmentService>, SaiShipmentService>();
builder.Services.AddScoped<IShipmentService<RimasShipmentService>, RimasShipmentService>();
builder.Services.AddSingleton<IMsSql>(new MsSql(config.Environment.ConnectionStr));
builder.Services.AddSingleton<IMySqlMillProvider>(new SaiMillProvider(config.SaiConfigurations.MillLocations.ConnectionStr));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
