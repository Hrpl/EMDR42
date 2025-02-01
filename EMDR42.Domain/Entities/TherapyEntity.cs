using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Entities;

public class TherapyEntity : BaseEntity
{
    public string UserId { get; set; }
    public string? Methods { get; set; }
    public string? AgePacients { get; set; }
    public string? CategoryPacients { get; set; }
    public string? Location { get; set; }
    public string? VariantSessions { get; set; }
    public string? Problems { get; set; }
}
