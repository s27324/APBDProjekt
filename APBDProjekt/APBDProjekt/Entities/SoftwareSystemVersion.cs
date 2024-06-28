namespace APBDProjekt.Entities;

public class SoftwareSystemVersion
{
    public int SoftwareSystemVersionId { get; set; }
    public int VersionId { get; set; }
    public int SoftwareSystemId { get; set; }
    public DateTime ReleaseDate { get; set; }
    
    public virtual Version Version { get; set; }
    public virtual SoftwareSystem SoftwareSystem { get; set; }
}