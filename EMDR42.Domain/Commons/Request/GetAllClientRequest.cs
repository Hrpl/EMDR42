using EMDR42.Domain.Commons.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Request;

public class GetAllClientRequest : BasePaginationFilter
{
    public string? Search { get; set; }
    public bool IsArchived { get; set; }
}
