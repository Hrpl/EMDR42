using EMDR42.Domain.Commons.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Request;

public class GetAllFeedbacksRequest : BasePaginationFilter
{
    public bool Feedback {  get; set; }
    public string Search { get; set; } = "";
    public int SortOnDate { get; set; }
    public int SortBy {  get; set; }
}
