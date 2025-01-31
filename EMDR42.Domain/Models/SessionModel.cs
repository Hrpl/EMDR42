using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Models;

public class SessionModel
{
    [SqlKata.Column("client_id")]
    public int ClientId { get; set; }
    [SqlKata.Column("duration")]
    public string? Duration { get; set; }
    [SqlKata.Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [SqlKata.Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    [SqlKata.Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
