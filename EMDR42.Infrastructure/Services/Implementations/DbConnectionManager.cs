using EMDR42.Domain.Commons.Singleton;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
using System.Xml.Linq;

namespace EMDR42.Infrastructure.Services.Implementations;

public class DbConnectionManager : IDbConnectionManager
{
    private readonly Config _config;
    private readonly ILogger<DbConnectionManager> _logger;

    public DbConnectionManager(Config configuration, ILogger<DbConnectionManager> logger)
    {
        _config = configuration;
        _logger = logger;
    }
    private string NpgsqlConnectionString => $"Host={_config.DbHost};Port={_config.DbPort};Database={_config.DbName};Username={_config.DbUser};Password={_config.DbPassword};";


    public NpgsqlConnection PostgresDbConnection => new(NpgsqlConnectionString);

    public QueryFactory PostgresQueryFactory => new(PostgresDbConnection, new PostgresCompiler(), 60)
    {
        Logger = compiled => { _logger.LogInformation("Query = {@Query}", compiled.ToString()); }
    };
}
