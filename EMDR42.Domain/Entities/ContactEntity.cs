using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Entities;

public class ContactEntity : BaseEntity
{
    public int UserId { get; set; }
    public string PhoneNumber { get; set; }
    public string ContactEmail { get; set; }
    public string ContactWebSite { get; set; }
}
