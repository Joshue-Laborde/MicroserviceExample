using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Data;
using System.Linq.Expressions;

namespace ADMReestructuracion.Common.Interfaces
{
    public interface IEntityRepository<T> : IDisposable
    where T : class
    {
        void ClearContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        string GetNextSerie(int tipoEntidad, string periodo, string param, string codigo, int index = 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> predicate, bool loadAll = false);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool loadAll = false);

        /// <summary>
        /// 
        /// </summary>
        IQueryable<T> All { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Any(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> SearchAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> SearchAll(Expression<Func<T, bool>> predicate, bool details);

        //
        // Summary:
        //     Specifies related entities to include in the query results. The navigation property
        //     to be included is specified starting with the type of entity being queried (TEntity).
        //     If you wish to include additional types based on the navigation properties of
        //     the type being included, then chain a call to Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ThenInclude``3(Microsoft.EntityFrameworkCore.Query.IIncludableQueryable{``0,System.Collections.Generic.IEnumerable{``1}},System.Linq.Expressions.Expression{System.Func{``1,``2}})
        //     after this call.
        //
        // Parameters:
        //   source:
        //     The source query.
        //
        //   navigationPropertyPath:
        //     A lambda expression representing the navigation property to be included (t =>
        //     t.Property1).
        //
        // Type parameters:
        //   TEntity:
        //     The type of entity being queried.
        //
        //   TProperty:
        //     The type of the related entity to be included.
        //
        // Returns:
        //     A new query with the related data included.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or navigationPropertyPath is null.
        IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> Search(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IQueryable<T> Load(IQueryable<T> entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IQueryable<T> Load(IQueryable<T> entity, bool includeDetails = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        T GetByID(params object[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Insert(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> InsertAsync(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<ICollection<T>> InsertAllAsync(ICollection<T> list);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<ICollection<T>> UpdateAllAsync(ICollection<T> list);

        /// <summary>
        /// Deshabilita y elimina logicamente el registro especificado.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> DeleteAsync(T entity);
        Task<List<T>> DeleteAllAsync(List<T> entity);

        /// <summary>
        /// Elimina definitivamente la entidad especificada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> RemoveAsync(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="usuario"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        Task<int> SaveAsync(int idEmpresa, string usuario = default, string ip = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<int> SaveAsync(IOperationRequest request);


        /// <summary>
        /// Provides access to change tracking and loading information for a collection navigation property that associates this entity to a collection of another entities.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyExpression">A lambda expression representing the property to access information and operations for (t => t.Property1).</param>
        /// <returns>An object that exposes change tracking information and operations for the given navigation property.</returns>
        CollectionEntry<T, TProperty> Collection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class;

        /// <summary>
        /// Provides access to change tracking information and operations for a given property of this entity.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyExpression">A lambda expression representing the property to access information and operations for (t => t.Property1).</param>
        /// <returns>An object that exposes change tracking information and operations for the given property.</returns>
        PropertyEntry<T, TProperty> Property<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class;

        /// <summary>
        ///  Provides access to change tracking and loading information for a reference (i.e. non-collection) navigation property that associates this entity to another entity.
        /// </summary>
        /// <typeparam name="TProperty"> </typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyExpression"> A lambda expression representing the property to access information and operations  for (t => t.Property1).</param>
        /// <returns>An object that exposes change tracking information and operations for the given navigation property.</returns>
        ReferenceEntry<T, TProperty> Reference<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<DataSet> ExecuteAsync(CommandType type, string sql, params KeyValuePair<string, object>[] parameters);
    }
}
