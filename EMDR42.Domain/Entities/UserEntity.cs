﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Entities;

public class UserEntity : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public bool IsConfirmed { get; set; }
    public int RoleId { get; set; }
}
