using System.Threading.Tasks;

namespace GamesService.Services
{
    public interface IMigrationService
    {
        Task MigrateAsync();
    }
}