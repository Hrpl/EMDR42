namespace EMDR42.Domain.Entities;

public class ClientEntity : BaseEntity
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Country { get; set; }
    public string Language { get; set; }
    public string Email { get; set; }
    public bool IsArchived { get; set; }
}
