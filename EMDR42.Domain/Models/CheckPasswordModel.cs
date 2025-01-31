using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Models;

public class CheckPasswordModel
{
    [SqlKata.Column("password")]
    public string Password { get; set; }
    [SqlKata.Column("salt")]
    public string Salt { get; set; }
}
