namespace EMDR42.Domain.Models;

public class ClientModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Country { get; set; }
    public string Language { get; set; }
    public string Email { get; set; }
    public bool IsArchived { get; set; } = false;
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
