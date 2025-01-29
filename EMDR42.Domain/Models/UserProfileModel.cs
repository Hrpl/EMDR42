namespace EMDR42.Domain.Models;

public class UserProfileModel
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Patronymic { get; set; }
    public string? Gender { get; set; }
    public DateTime Birthday { get; set; }
    public string? Address { get; set; }
    public bool IsPublic { get; set; } = true;
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
