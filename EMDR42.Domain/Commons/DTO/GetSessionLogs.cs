using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.DTO;

public class GetSessionLogs
{
    public int ClientId { get; set; }
    public DateTime Start {  get; set; }
    public DateTime End { get; set; }
}
