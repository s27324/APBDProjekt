namespace APBDProjekt.Entities;

public class Company
{
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string KRS { get; set; }
    
    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}