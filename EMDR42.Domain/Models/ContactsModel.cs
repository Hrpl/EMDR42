using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Models;

public class ContactsModel
{
    [SqlKata.Column("user_id")]
    public int UserId { get; set; }
    [SqlKata.Column("phone_number")]
    public string? PhoneNumber { get; set; }
    [SqlKata.Column("contact_email")]
    public string ContactEmail { get; set; }
    [SqlKata.Column("contact_web_site")]
    public string? ContactWebSite { get; set; }
    [SqlKata.Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    [SqlKata.Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    [SqlKata.Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
