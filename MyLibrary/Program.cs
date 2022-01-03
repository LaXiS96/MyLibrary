using LaXiS.MyLibrary.Jobs;
using LaXiS.MyLibrary.Models;
using LaXiS.MyLibrary.Repositories;
using LaXiS.MyLibrary.Services;
using Microsoft.Extensions.FileProviders;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3} {SourceContext}] {Message:lj}{NewLine}{Exception}")
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.Configure<MyLibraryOptions>(
        builder.Configuration.GetSection("MyLibrary"));

    builder.Services.AddHostedService<InitializationService>();

    builder.Services.AddSingleton<FileRepository>();

    builder.Services.AddSingleton<JobManager>();
    builder.Services.AddTransient<ScanJob>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(app.Configuration.GetValue<string>("MyLibrary:ContentPath")),
        RequestPath = new PathString("/Content")
    });

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "");
}
finally
{
    Log.CloseAndFlush();
}
