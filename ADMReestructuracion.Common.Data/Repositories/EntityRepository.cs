using ADMReestructuracion.Common.Data.Extensions;
using ADMReestructuracion.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Data;
using System.Linq.Expressions;

namespace ADMReestructuracion.Common.Data.Repositories
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class
    {
        private readonly DbContext context;

        public EntityRepository(DbContext context)
        {
            this.context = context;
        }

        public IQueryable<T> All => context.Set<T>();

        public string GetNextSerie(int tipo, string periodo, string param, string codigo, int index = 1)
        {
            return context.Set<T>().GetNextSerie(tipo, periodo, param, codigo, index);
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Any(predicate);
        }

        public IQueryable<T> SearchAll(Expression<Func<T, bool>> predicate)
        {
            return SearchAll(predicate, false);
        }

        public IQueryable<T> SearchAll(Expression<Func<T, bool>> predicate, bool details)
        {
            return Load(Search(predicate), details);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate, bool loadAll = false)
        {
            return loadAll ? SearchAll(predicate)?.FirstOrDefault() : All?.Where(predicate)?.FirstOrDefault();
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool loadAll = false)
        {
            var en = await (loadAll ? SearchAll(predicate)?.FirstOrDefaultAsync() : All?.Where(predicate)?.FirstOrDefaultAsync());
            return en;
        }

        public CollectionEntry<T, TProperty> Collection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
        {
            return context.Entry(entity).Collection(propertyExpression);
        }

        public PropertyEntry<T, TProperty> Property<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class
        {
            return context.Entry(entity).Property(propertyExpression);
        }

        public ReferenceEntry<T, TProperty> Reference<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class
        {
            return context.Entry(entity).Reference(propertyExpression);
        }

        public T GetByID(params object[] keys)
        {
            return (T)context.Find(typeof(T), keys);
        }

        public IQueryable<T> Load(IQueryable<T> entity)
        {
            return Load(entity, false);
        }

        public IQueryable<T> Load(IQueryable<T> entity, bool includeDetails)
        {
            try
            {
                var obj = entity.FirstOrDefault();

                if (obj != null)
                {
                    var entry = context.Entry(obj);
                    foreach (var reference in entry.References)
                    {
                        entity = entity.Include(reference.Metadata.Name);
                    }

                    if (includeDetails)
                    {
                        foreach (var collection in entry.Collections)
                        {
                            entity = entity.Include(collection.Metadata.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }


            return entity;
        }

        public T Insert(T entity)
        {
            return context.Add(entity).Entity;
        }

        public async Task<T> InsertAsync(T entity)
        {
            return (await context.AddAsync(entity)).Entity;
        }

        public async Task<T> RemoveAsync(T entity)
        {
            return await Task.FromResult(context.Remove(entity).Entity);
        }

        public async Task<T> DeleteAsync(T entity)
        {
            context.Entry(entity).SetProperty("IdEstado", false);
            return await UpdateAsync(entity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await Task.FromResult(context.Update(entity).Entity);
        }

        public async Task<ICollection<T>> InsertAllAsync(ICollection<T> list)
        {
            foreach (var entity in list)
            {
                await InsertAsync(entity);
            }

            return await Task.FromResult(list);
        }

        public async Task<ICollection<T>> UpdateAllAsync(ICollection<T> list)
        {
            foreach (var entity in list)
            {
                await UpdateAsync(entity);
            }

            return await Task.FromResult(list);
        }

        public async Task<int> SaveAsync(IOperationRequest request)
        {
            return await SaveAsync(request.IdEmpresa, request.InicioSesion, request.Ip);
        }

        public async Task<int> SaveAsync(int idEmpresa = default, string usuario = default, string ip = default)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified)
                {

                    entry.SetProperty("UsuarioModificacion", usuario ?? "DESCONOCIDO")
                        .SetProperty("FechaModificacion", DateTime.Now)
                        .SetProperty("IpModificacion", ip ?? "0.0.0.0")
                        .SetProperty("IdEmpresa", idEmpresa);

                }

                if (entry.State == EntityState.Added)
                {
                    entry.SetProperty("UsuarioRegistro", usuario ?? "DESCONOCIDO")
                        .SetProperty("FechaRegistro", DateTime.Now)
                        .SetProperty("IpRegistro", ip ?? "0.0.0.0")
                        .SetProperty("IdEmpresa", idEmpresa)
                        .SetProperty("IdEstado", true);
                }

            }

            return await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<DataSet> ExecuteAsync(CommandType type, string sql, params KeyValuePair<string, object>[] parameters)
        {
            var connection = context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandType = type;
            command.CommandText = sql;

            foreach (var item in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = item.Key;
                parameter.Value = item.Value;

                command.Parameters.Add(parameter);
            }

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            var result = new DataSet();
            var dt = result.Tables.Add("Result");

            dt.Load(reader);

            return result;

        }

        public IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return All.Include(navigationPropertyPath);
        }

        public async Task<List<T>> DeleteAllAsync(List<T> list)
        {
            foreach (var entity in list)
            {
                await DeleteAsync(entity);
            }

            return await Task.FromResult(list);
        }

        public void ClearContext()
        {
            context.ChangeTracker.Clear();
        }
    }
}
