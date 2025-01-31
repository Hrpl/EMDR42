using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.DTO;

public class SessionDTO
{
    public int ClientId { get; set; }
    public DateTime? CreatedAt { get; set; }
}
