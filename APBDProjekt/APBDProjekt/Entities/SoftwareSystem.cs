using Azure.Identity;

namespace APBDProjekt.Entities;

public class SoftwareSystem
{
    public int SoftwareSystemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public virtual ICollection<Discount> IdDiscounts { get; set; } = new List<Discount>();
    public virtual ICollection<SoftwareSystemVersion> SoftwareSystemVersions { get; set; } = new List<SoftwareSystemVersion>();
    public virtual ICollection<Category> IdCategories { get; set; } = new List<Category>();
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}