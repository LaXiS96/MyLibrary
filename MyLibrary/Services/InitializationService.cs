using LaXiS.MyLibrary.Models;
using Microsoft.Extensions.Options;

namespace LaXiS.MyLibrary.Services
{
    public class InitializationService : IHostedService
    {
        private readonly ILogger _logger;

        public InitializationService(
            ILogger<InitializationService> logger,
            IOptions<MyLibraryOptions> options)
        {
            _logger = logger;
            _logger.LogDebug(options.Value.ContentPath);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //// TODO move database initialization code somewhere else

            //using var connection = new SqliteConnection("Data Source=mylibrary.db");
            //connection.Open();

            //_logger.LogInformation("SQLite version: {Version}", connection.ServerVersion);

            //int version;
            //using (var command = connection.CreateCommand())
            //{
            //    command.CommandText = @"PRAGMA user_version";
            //    version = Convert.ToInt32(await command.ExecuteScalarAsync());
            //}
            //_logger.LogInformation("Database version: {Version}", version);

            //// TODO check latest version and update accordingly following Migrations files

            //if (version == 0)
            //{
            //    var sql = await File.ReadAllTextAsync("Migrations/0001.sql");

            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = sql;
            //        await command.ExecuteNonQueryAsync();
            //    }
            //}
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
