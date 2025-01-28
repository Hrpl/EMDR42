using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Entities;

public class QualificationEntity : BaseEntity
{
    public int UserId { get; set; }
    public string? School {  get; set; }
    public string? Supervisor { get; set; }
    public string? InPractic {  get; set; }
}
