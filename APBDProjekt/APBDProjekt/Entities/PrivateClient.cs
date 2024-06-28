namespace APBDProjekt.Entities;

public class PrivateClient
{
    public int PrivateClientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PESEL { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}