using Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // Relación con ServiceType
    public ICollection<ServiceType> ServiceTypes { get; set; } = new List<ServiceType>();
}
