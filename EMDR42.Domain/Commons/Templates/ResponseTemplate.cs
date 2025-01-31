using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Templates;

public static class ResponseTemplate
{
    public static string ConfirmResponse = @"
            <html>
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Redirecting...</title>
            </head>
            <body>
                <center>
                    <h1>Ваш email подтверждён</h1>
                    <h2>Теперь вы можете авторизоваться в приложении</h2>
                </center>
            </body>
            </html>";
}
