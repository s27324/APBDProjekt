namespace APBDProjekt.Entities;

public class Discount
{
    public int DiscountId { get; set; }
    public string Name { get; set; }
    public string Offer { get; set; }
    public decimal Value { get; set; }
    public string Timeslot { get; set; }

    public virtual ICollection<SoftwareSystem> IdSoftwareSystems { get; set; } = new List<SoftwareSystem>();
}