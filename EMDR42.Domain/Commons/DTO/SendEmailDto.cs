using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.DTO;

public class SendEmailDto
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public string MessageBody { get; set; }
}
