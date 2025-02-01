using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Response;

public class ClientsResponse
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? Country { get; set; }
    public int Sessions { get; set; } = 0;
    public DateTime? LastSession { get; set; } = DateTime.MinValue;
}
