using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Implementations;

public class DbConnectionManager : IDbConnectionManager
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbConnectionManager> _logger;
    private string NpgsqlConnectionString => _configuration["ConnectionString:DefaultConnection"];

    public DbConnectionManager(IConfiguration configuration, ILogger<DbConnectionManager> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public NpgsqlConnection PostgresDbConnection => new(NpgsqlConnectionString);

    public QueryFactory PostgresQueryFactory => new(PostgresDbConnection, new PostgresCompiler(), 60)
    {
        Logger = compiled => { _logger.LogInformation("Query = {@Query}", compiled.ToString()); }
    };
}
