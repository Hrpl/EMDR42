using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Response;

public class GetSpecialistsResponse
{
    public string UserId { get; set; }
    public string Photo {  get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string AboutMe { get; set; }
    public string ClinicName { get; set; }
}
