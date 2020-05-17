using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace NLayer {
	/// <summary>
	/// Serves as a generic base class for concrete repositories to support basic CRUD operations on data in the system.
	/// </summary>
	/// <typeparam name="T">The type of entity this repository works with. Must be a class inheriting <see cref="DomainEntity{T}"/>.</typeparam>
	/// <typeparam name="TDomainEntityType">The type of entity <see cref="DomainEntity{T}"/> works with.</typeparam>
	/// <typeparam name="TDbContext">The type of DbContext this repository works with.</typeparam>
	public abstract class Repository<T, TDomainEntityType, TDbContext> : IRepository<T, TDomainEntityType>, IDisposable where T : DomainEntity<TDomainEntityType> where TDbContext : DbContext, new() where TDomainEntityType : IEquatable<TDomainEntityType> {
		private readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// Initializes a new instance of the Repository class.
		/// </summary>
		public Repository(IHttpContextAccessor httpContextAccessor) {
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Finds an item by its unique ID.
		/// </summary>
		/// <param name="id">The unique ID of the item in the database.</param>
		/// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
		/// <returns>The requested item when found, or null otherwise.</returns>
		public virtual T FindById(TDomainEntityType id, params Expression<Func<T, object>>[] includeProperties) {
			return FindAll(includeProperties).SingleOrDefault(x => id.Equals(x.Id));
		}

		/// <summary>
		/// Returns an IQueryable of all items of type T.
		/// </summary>
		/// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
		/// <returns>An IQueryable of the requested type T.</returns>
		public virtual IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties) {
			IQueryable<T> items = DataContextFactory<TDbContext>.GetDataContext(_httpContextAccessor).Set<T>();

			if (includeProperties != null) {
				foreach (var includeProperty in includeProperties) {
					items = items.Include(includeProperty);
				}
			}
			return items;
		}

		/// <summary>
		/// Returns an IQueryable of items of type T.
		/// </summary>
		/// <param name="predicate">A predicate to limit the items being returned.</param>
		/// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
		/// <returns>An IQueryable of the requested type T.</returns>
		public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) {
			IQueryable<T> items = DataContextFactory<TDbContext>.GetDataContext(_httpContextAccessor).Set<T>();
			if (includeProperties != null) {
				foreach (var includeProperty in includeProperties) {
					items = items.Include(includeProperty);
				}
			}
			return items.Where(predicate);
		}

		/// <summary>
		/// Adds an entity to the underlying DbContext.
		/// </summary>
		/// <param name="entity">The entity that should be added.</param>
		public virtual void Add(T entity) {
			DataContextFactory<TDbContext>.GetDataContext(_httpContextAccessor).Set<T>().Add(entity);
		}

		/// <summary>
		/// Removes an entity from the underlying DbContext.
		/// </summary>
		/// <param name="entity">The entity that should be removed.</param>
		public virtual void Remove(T entity) {
			DataContextFactory<TDbContext>.GetDataContext(_httpContextAccessor).Set<T>().Remove(entity);
		}

		/// <summary>
		/// Removes an entity from the underlying DbContext. Calls <see cref="FindById" /> to resolve the item.
		/// </summary>
		/// <param name="id">The ID of the entity that should be removed.</param>
		public virtual void Remove(TDomainEntityType id) {
			Remove(FindById(id));
		}

		public virtual void Edit(T entity) {
			DataContextFactory<TDbContext>.GetDataContext(_httpContextAccessor).Entry(entity).State = EntityState.Modified;
		}

		public virtual void DoNotEdit(T entity) {
			DataContextFactory<TDbContext>.GetDataContext(_httpContextAccessor).Entry(entity).State = EntityState.Unchanged;
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		/// <summary>
		/// Disposes the underlying data context.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				if (disposing && DataContextFactory<TDbContext>.GetDataContext(_httpContextAccessor) != null) {
					// TODO: dispose managed state (managed objects).
					DataContextFactory<TDbContext>.GetDataContext(_httpContextAccessor).Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~Repository() {
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		/// <summary>
		/// Saves the changes to the underlying DbContext.
		/// </summary>
		public void Dispose() {
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
