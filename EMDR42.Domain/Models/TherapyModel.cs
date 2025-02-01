using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Models;

public class TherapyModel
{
    [SqlKata.Column("user_id")]
    public string UserId { get; set; }
    [SqlKata.Column("methods")]
    public string? Methods { get; set; }
    [SqlKata.Column("age_patients")]
    public string? AgePatients { get; set; }
    [SqlKata.Column("category_patients")]
    public string? CategoryPatients { get; set; }
    [SqlKata.Column("location")]
    public string? Location { get; set; }
    [SqlKata.Column("variant_sessions")]
    public string? VariantSessions { get; set; }
    [SqlKata.Column("problems")]
    public string? Problems { get; set; }
    [SqlKata.Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [SqlKata.Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
