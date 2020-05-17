using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace NLayer {
	/// <summary>
	/// Creates new instances of an EF unit of Work.
	/// </summary>
	public class EFUnitOfWorkFactory<T> : IUnitOfWorkFactory<T> where T : DbContext, new() {
		/// <summary>
		/// Creates a new instance of an EFUnitOfWork.
		/// </summary>
		public IUnitOfWork<T> Create() {
			return Create(false, null);
		}

		/// <summary>
		/// Creates a new instance of an EFUnitOfWork.
		/// </summary>
		/// <param name="httpContextAccessor">The current HttpContext.</param>
		public IUnitOfWork<T> Create(IHttpContextAccessor httpContextAccessor) {
			return Create(false, httpContextAccessor);
		}

		/// <summary>
		/// Creates a new instance of an EFUnitOfWork.
		/// </summary>
		/// <param name="httpContextAccessor">The current HttpContext.</param>
		public IUnitOfWork<T> Create(bool forceNew) {
			return Create(forceNew, null);
		}

		/// <summary>
		/// Creates a new instance of an EFUnitOfWork.
		/// </summary>
		/// <param name="forceNew">When true, clears out any existing data context from the storage container.</param>
		public IUnitOfWork<T> Create(bool forceNew, IHttpContextAccessor httpContextAccessor) {
			return new EFUnitOfWork<T>(forceNew, httpContextAccessor);
		}
	}
}
