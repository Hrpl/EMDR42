﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.DTO;

public class GetUserProfileDTO
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string Gender { get; set; }
    public string Photo { get; set; }
    public string AboutMe { get; set; }
    public string ClinicName { get; set; }
    public DateOnly Birthday { get; set; }
    public string Address { get; set; }
    public bool IsPublic { get; set; }
}
