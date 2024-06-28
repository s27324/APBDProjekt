namespace APBDProjekt.Entities;

public class Contract
{
    public int ContractId { get; set; }
    public decimal CurrentCharge { get; set; }
    public decimal MaxCharge { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsSigned { get; set; }
    public int YearsOfSupport { get; set; }
    
    public int SoftwareSystemId { get; set; }
    public int ClientId { get; set; }
    
    public virtual SoftwareSystem SoftwareSystem { get; set; }
    public virtual Client Client { get; set; }
}