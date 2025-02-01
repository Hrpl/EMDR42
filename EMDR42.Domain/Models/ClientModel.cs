namespace EMDR42.Domain.Models;

public class ClientModel
{
    [SqlKata.Column("user_id")]
    public int UserId { get; set; }
    [SqlKata.Column("user_name")]
    public string UserName { get; set; }
    [SqlKata.Column("country")]
    public string Country { get; set; }
    [SqlKata.Column("language")]
    public string Language { get; set; }
    [SqlKata.Column("email")]
    public string Email { get; set; }
    [SqlKata.Column("is_archived")]
    public bool IsArchived { get; set; } = false;
    [SqlKata.Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    [SqlKata.Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    [SqlKata.Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
