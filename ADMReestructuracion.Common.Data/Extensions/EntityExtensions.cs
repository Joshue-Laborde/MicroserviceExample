using Microsoft.EntityFrameworkCore;

namespace ADMReestructuracion.Common.Data.Extensions
{
    public static class EntityExtensions
    {
        public static string GetNextSerie<T>(this DbSet<T> entity, int tipo, string periodo, string param, string codigo, int index = 1) where T : class
        {
            var schname = entity.EntityType.GetSchemaQualifiedTableName();

            var secu = entity.FromSqlRaw($"SELECT * FROM {schname} WHERE Periodo='{periodo}' AND {param}={tipo} ").Count();

            return $"{codigo}-{periodo}-{secu + index:0000}";
        }
    }
}
