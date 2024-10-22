using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DB.Extension
{
    public static class DbExtension
    {
        public static IServiceCollection AddDatabase<T>( this IServiceCollection services ) where T : DbContext
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath ( folder );
            var DbPath = Path.Join ( path , "inventory.db" );

            // The following configures EF to create a Sqlite database file in the
            // special "local" folder for your platform.
            return services.AddDbContext<T> (
                options => options.UseSqlite ( $"Data Source={DbPath}" ) );
        }
    }
}
