using ShopApi.AppDatabase;

namespace ShopApi.Services
{
    public abstract class BaseContextService<T> where T:class
    {
        protected readonly ApplicationDbContext _db;
        public BaseContextService(ApplicationDbContext db)
        {
            _db = db;
        }

    }
}
