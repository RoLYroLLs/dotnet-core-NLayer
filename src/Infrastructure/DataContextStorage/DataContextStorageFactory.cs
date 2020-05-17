using Microsoft.AspNetCore.Http;

namespace NLayer {
	/// <summary>
	/// A helper class to create application platform specific storage containers.
	/// </summary>
	/// <typeparam name="T">The type for which to create the container.</typeparam>
	public static class DataContextStorageFactory<T> where T : class {
		private static IDataContextStorageContainer<T> _dataContextStorageContainer;

		/// <summary>
		/// Creates a new container that uses HttpContext.Items (when HttpContext is not null) or Thread.Items.
		/// </summary>
		/// <returns>A contact storage container to store objects. </returns>
#pragma warning disable CA1000 // Do not declare static members on generic types
		public static IDataContextStorageContainer<T> CreateDataContextStorageContainer(IHttpContextAccessor httpContextAccessor) {
#pragma warning restore CA1000 // Do not declare static members on generic types
			if (_dataContextStorageContainer == null) {
				if (httpContextAccessor == null) {
					_dataContextStorageContainer = new ThreadDataContextStorageContainer<T>();
				} else {
					_dataContextStorageContainer = new HttpDataContextStorageContainer<T>(httpContextAccessor);
				}
			}

			return _dataContextStorageContainer;
		}
	}
}
