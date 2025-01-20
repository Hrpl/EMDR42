using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Response;

public class JwtResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}
