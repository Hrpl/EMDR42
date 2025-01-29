using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Entities;

public class SessionEntity : BaseEntity
{
    public int UserId { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
}
