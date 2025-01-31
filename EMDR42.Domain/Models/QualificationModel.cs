using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Models;

public class QualificationModel
{
    [SqlKata.Column("user_id")]
    public int UserId { get; set; }
    [SqlKata.Column("school")]
    public string? School { get; set; }
    [SqlKata.Column("supervisor")]
    public string? Supervisor { get; set; }
    [SqlKata.Column("in_practic")]
    public string? InPractic { get; set; }
    [SqlKata.Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    [SqlKata.Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    [SqlKata.Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
