using System.ComponentModel.DataAnnotations;

namespace BackendTemplate.Domain.Entities;

// Example entity showing repository pattern usage
// Delete this file when creating your actual entities
public class ExampleEntity : BaseEntity
{
    [Key]
    public Guid ExampleEntityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
