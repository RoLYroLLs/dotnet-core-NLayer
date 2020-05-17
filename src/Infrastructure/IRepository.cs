using System;
using System.Linq;
using System.Linq.Expressions;

namespace NLayer {
	/// <summary>
	/// Defines various methods for working with data in the system.
	/// </summary>
	public interface IRepository<TClass, TId> where TClass : class {
		/// <summary>
		/// Finds an item by its unique ID.
		/// </summary>
		/// <param name="id">The unique ID of the item in the database.</param>
		/// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
		/// <returns>The requested item when found, or null otherwise.</returns>
		TClass FindById(TId id, params Expression<Func<TClass, object>>[] includeProperties);

		/// <summary>
		/// Returns an IQueryable of all items of type TClass.
		/// </summary>
		/// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
		/// <returns>An IQueryable of the requested type TClass.</returns>
		IQueryable<TClass> FindAll(params Expression<Func<TClass, object>>[] includeProperties);

		/// <summary>
		/// Returns an IQueryable of items of type TClass.
		/// </summary>
		/// <param name="predicate">A predicate to limit the items being returned.</param>
		/// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
		/// <returns>An IQueryable of the requested type TClass.</returns>
		IQueryable<TClass> FindAll(Expression<Func<TClass, bool>> predicate, params Expression<Func<TClass, object>>[] includeProperties);

		/// <summary>
		/// Adds an entity to the underlying collection.
		/// </summary>
		/// <param name="entity">The entity that should be added.</param>
		void Add(TClass entity);

		/// <summary>
		/// Removes an entity from the underlying collection.
		/// </summary>
		/// <param name="entity">The entity that should be removed.</param>
		void Remove(TClass entity);

		/// <summary>
		/// Removes an entity from the underlying collection.
		/// </summary>
		/// <param name="id">The ID of the entity that should be removed.</param>
		void Remove(TId id);

		void Edit(TClass entity);

		void DoNotEdit(TClass entity);
	}
}
