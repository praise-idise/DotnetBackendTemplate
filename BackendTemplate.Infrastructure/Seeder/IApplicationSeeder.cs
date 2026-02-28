using System.Threading.Tasks;

namespace BackendTemplate.Infrastructure.Seeder;

public interface IApplicationSeeder
{
    Task SeedAsync();
}
