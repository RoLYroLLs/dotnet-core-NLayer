using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace NLayer {
	/// <summary>
	/// Manages instances of the DbContext and stores them in an appropriate storage container.
	/// </summary>
	public static class DataContextFactory<T> where T : DbContext, new() {
		/// <summary>
		/// Clears out the current DbContext.
		/// </summary>
#pragma warning disable CA1000 // Do not declare static members on generic types
		public static void Clear(IHttpContextAccessor httpContextAccessor) {
#pragma warning restore CA1000 // Do not declare static members on generic types
			var dataContextStorageContainer = DataContextStorageFactory<T>.CreateDataContextStorageContainer(httpContextAccessor);
			dataContextStorageContainer.Clear();
		}

		/// <summary>
		/// Retrieves an instance of DbContext from the appropriate storage container or
		/// creates a new instance and stores that in a container.
		/// </summary>
		/// <returns>An instance of DbContext.</returns>
#pragma warning disable CA1000 // Do not declare static members on generic types
		public static T GetDataContext(IHttpContextAccessor httpContextAccessor) {
#pragma warning restore CA1000 // Do not declare static members on generic types
			var dataContextStorageContainer = DataContextStorageFactory<T>.CreateDataContextStorageContainer(httpContextAccessor);
			var context = dataContextStorageContainer.GetDataContext();
			if (context == null) {
				context = new T();
				dataContextStorageContainer.Store(context);
			}
			return context;
		}
	}
}
