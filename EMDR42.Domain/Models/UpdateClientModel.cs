using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Models;

public class UpdateClientModel
{
    [SqlKata.Column("user_name")]
    public string UserName { get; set; }
    [SqlKata.Column("country")]
    public string Country { get; set; }
    [SqlKata.Column("language")]
    public string Language { get; set; }
    [SqlKata.Column("email")]
    public string Email { get; set; }
    [SqlKata.Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}
