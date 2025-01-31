using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.DTO;

public class SessionLogResponse
{
    public DateTime CreatedAt { get; set; }
    public string Duration { get; set; }
}
