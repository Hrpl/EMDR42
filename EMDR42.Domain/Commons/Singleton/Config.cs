using DotNetEnv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Singleton;

public class Config
{
    public Config()
    {
        Env.Load(); // Загружаем .env файл
    }

    //Database
    public string DbHost => Environment.GetEnvironmentVariable("DB_HOST");
    public string DbUser => Environment.GetEnvironmentVariable("DB_USER");
    public string DbPassword => Environment.GetEnvironmentVariable("DB_PASSWORD");
    public string DbName => Environment.GetEnvironmentVariable("DB_NAME");
    public string DbPort => Environment.GetEnvironmentVariable("DB_PORT");

    //Smtp
    public string SmtpHost => Environment.GetEnvironmentVariable("SMTP_HOST");
    public string SmtpPort => Environment.GetEnvironmentVariable("SMTP_PORT");
    public string SmtpEmail => Environment.GetEnvironmentVariable("SMTP_EMAIL");
    public string SmtpPassword => Environment.GetEnvironmentVariable("SMTP_PASSWORD");
}
