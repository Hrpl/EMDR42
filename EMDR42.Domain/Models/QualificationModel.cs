using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Models;

public class QualificationModel
{
    public int UserId { get; set; }
    public string? School { get; set; }
    public string? Supervisor { get; set; }
    public string? InPractic { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
