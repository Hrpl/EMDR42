using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Models;

public class FeedbackModel
{
    [SqlKata.Column("name")]
    public string Name { get; set; }
    [SqlKata.Column("email")]
    public string Email { get; set; }
    [SqlKata.Column("feedback")]
    public string Feedback { get; set; }
    [SqlKata.Column("is_approved ")]
    public bool IsApproved { get; set; }
    [SqlKata.Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    [SqlKata.Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    [SqlKata.Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
