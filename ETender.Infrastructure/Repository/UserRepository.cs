using ETender.Core.Entities;
using ETender.Core.Repositories;
using ETender.Infrastructure.Data;
using ETender.Infrastructure.Repository.Base;

namespace ETender.Infrastructure.Repository
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private readonly WebAppContext _dbContext;

        public UserRepository(WebAppContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
