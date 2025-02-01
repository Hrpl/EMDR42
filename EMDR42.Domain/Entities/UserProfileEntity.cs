using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Entities;

public class UserProfileEntity : BaseEntity
{
    public int UserId { get; set; }
    public string Photo { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Patronymic { get; set; }
    public string? Gender { get; set; }
    public DateTime Birthday { get; set; }
    public string? Address { get; set; }
    public bool IsPublic { get; set; }
}
