namespace APBDProjekt.Entities;

public class Version
{
    public int VersionId { get; set; }
    public string Name { get; set; }
    public DateTime VersionDate { get; set; }

    public virtual ICollection<SoftwareSystemVersion> SoftwareSystemVersions { get; set; } = new List<SoftwareSystemVersion>();
}