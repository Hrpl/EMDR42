using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Models;

public class SessionModel
{
    public int ClientId { get; set; }
    public string? Duration { get; set; }
    public DateTime? CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
