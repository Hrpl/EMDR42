using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.DTO;

public class UpdateClientDTO
{
    public string UserName { get; set; }
    public string Country { get; set; }
    public string Language { get; set; }
    public string Email { get; set; }
}
