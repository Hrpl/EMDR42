using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Entities;

public class FeedbackEntity : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Feedback {  get; set; }
    public bool IsApproved { get; set; }
}
