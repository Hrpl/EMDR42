using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Entities;

public class TherapyEntity : BaseEntity
{
    public int UserId { get; set; }
    public string? Methods { get; set; }
    public string? AgePatients { get; set; }
    public string? CategoryPatients { get; set; }
    public string? Location { get; set; }
    public string? VariantSessions { get; set; }
    public string? Problems { get; set; }
}
