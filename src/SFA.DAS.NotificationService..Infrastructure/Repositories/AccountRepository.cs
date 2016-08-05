using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Domain.Models;
using SFA.DAS.NotificationService.Domain.Repositories;

namespace SFA.DAS.NotificationService.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfigurationService _configurationService;

        public AccountRepository(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<Account> Get(int id)
        {
            var config = await _configurationService.GetAsync<NotificationServiceConfiguration>();

            using (var connection = new SqlConnection(config.LevyDatabase.ConnectionString))
            {
                var results = await connection.QueryAsync<Account>("select * from [dbo].[Account] where Id = @Id", new { Id = id });

                return results?.FirstOrDefault();
            }
        }

        public async Task<int> Save(Account entity)
        {
            var config = await _configurationService.GetAsync<NotificationServiceConfiguration>();

            using (var connection = new SqlConnection(config.LevyDatabase.ConnectionString))
            {
                return (await connection.QueryAsync<int>("INSERT INTO [dbo].[Account] (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() as int)", entity)).Single();
            }
        }

        public async Task Delete(Account entity)
        {
            var config = await _configurationService.GetAsync<NotificationServiceConfiguration>();

            using (var connection = new SqlConnection(config.LevyDatabase.ConnectionString))
            {
                await connection.ExecuteAsync("DELETE [dbo].[Account] WHERE Id = @Id", entity);
            }
        }
    }
}
