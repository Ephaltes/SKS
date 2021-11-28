using Microsoft.EntityFrameworkCore;

namespace NLSL.SKS.Package.DataAccess.Sql.Extensions
{
    public static class DbSetExtension
    {

        public static string GetSqlDeleteStatementForTable<T>(this DbSet<T> dbSet) where T : class
        {
            return $"DELETE FROM {dbSet.EntityType.GetTableName()}";
        }
    }
}