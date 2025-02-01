using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Request;

public class ChangePasswordRequest
{
    public string Salt { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}
