namespace APBDProjekt.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    
    public virtual ICollection<SoftwareSystem> IdSoftwareSystems { get; set; } = new List<SoftwareSystem>();
}