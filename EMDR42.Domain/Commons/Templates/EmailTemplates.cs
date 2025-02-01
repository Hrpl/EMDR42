using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Templates;

public static class EmailTemplates
{
    public const string RegistrationEmailTemplate = @"
<body >
    
    <div >
        <h3 >Благодарим вас за регистрацию в EMDR42.</h3>
        <br>
        <p >Для завершения процесса регистрации, пожалуйста, перейдите по <span>
            <a href="""">ССЫЛКЕ</a>
        </span> для подтверждения email.</p>
        
    </div>
</body>";

    public const string ChangePasswordEmailTemplate = @"
<body >
    
    <div >
        <h3 >Смена пароля.</h3>
        <p >Ключ: @salt</p>
        <br>
        <p >Для смены пароля перейдите по <span>
            <a href="""">ССЫЛКЕ</a>
        </span> </p>
        
    </div>
</body>";

    public const string ChangeEmailAddressTemplate = @"
<body >
    
    <div >
        <h3 >Смена почты.</h3>
        <br>
        <p >Для смены пароля перейдите по <span>
            <a href=""/@email/@id?contact=@contact"">ССЫЛКЕ</a>
        </span> </p>
        
    </div>
</body>";
}
