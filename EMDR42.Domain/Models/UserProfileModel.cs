namespace EMDR42.Domain.Models;

public class UserProfileModel
{
    [SqlKata.Column("user_id")]
    public int UserId { get; set; }
    [SqlKata.Column("name")]
    public string? Name { get; set; }
    [SqlKata.Column("surname")]
    public string? Surname { get; set; }
    [SqlKata.Column("patronymic")]
    public string? Patronymic { get; set; }
    [SqlKata.Column("gender")]
    public string? Gender { get; set; }
    [SqlKata.Column("birthday")]
    public DateTime Birthday { get; set; }
    [SqlKata.Column("address")]
    public string? Address { get; set; }
    [SqlKata.Column("is_public")]
    public bool IsPublic { get; set; } = true;
    [SqlKata.Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    [SqlKata.Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    [SqlKata.Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
