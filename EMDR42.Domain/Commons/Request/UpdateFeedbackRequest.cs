using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Request;

public class UpdateFeedbackRequest
{
    public int FeedbackId { get; set; }
    public string Feedback { get; set; }
}
