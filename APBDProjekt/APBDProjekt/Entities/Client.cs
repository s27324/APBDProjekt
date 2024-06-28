namespace APBDProjekt.Entities;

public class Client
{
    public int ClientId { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? PrivateClientId { get; set; }
    public int? CompanyId { get; set; }
    
    public virtual PrivateClient? PrivateClient { get; set; }
    public virtual Company? Company { get; set; }
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}