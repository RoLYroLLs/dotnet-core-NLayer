using Microsoft.AspNetCore.Http;

namespace NLayer {
	/// <summary>
	/// A Helper class to store objects like a DataContext in the HttpContext.Current.Items collection.
	/// </summary>
	/// <typeparam name="T">The type of object to store.</typeparam>
	public class HttpDataContextStorageContainer<T> : IDataContextStorageContainer<T> where T : class {
		private readonly string _dataContextKey;

		/// <summary>
		/// A field to hold the http context.
		/// https://stackoverflow.com/questions/47956577/an-alternative-for-httpcontext-current-items-containsdatacontextkey-in-asp-net
		/// https://stackoverflow.com/questions/47357689/httpcontext-in-net-standard-library
		/// </summary>
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HttpDataContextStorageContainer(IHttpContextAccessor httpContextAccessor) {
			_dataContextKey = typeof(T) + "DataContext";
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Returns an object from the container when it exists. Returns null otherwise.
		/// </summary>
		/// <returns>The object from the container when it exists, null otherwise.</returns>
		public T GetDataContext() {
			T objectContext = null;
			if (HttpContext.Items.ContainsKey(_dataContextKey)) {
				objectContext = (T)HttpContext.Items[_dataContextKey];
			}

			return objectContext;
		}

		/// <summary>
		/// Clears the object from the container.
		/// </summary>
		public void Clear() {
			if (HttpContext.Items.ContainsKey(_dataContextKey)) {
				HttpContext.Items[_dataContextKey] = null;
			}
		}

		/// <summary>
		/// Stores the object in HttpContext.Items.
		/// </summary>
		/// <param name="objectContext">The object to store.</param>
		public void Store(T objectContext) {
			if (HttpContext.Items.ContainsKey(_dataContextKey)) {
				HttpContext.Items[_dataContextKey] = objectContext;
			} else {
				HttpContext.Items.Add(_dataContextKey, objectContext);
			}
		}

		/// <summary>
		/// A property to hold the http context from the constructor.
		/// https://stackoverflow.com/questions/47956577/an-alternative-for-httpcontext-current-items-containsdatacontextkey-in-asp-net
		/// https://stackoverflow.com/questions/47357689/httpcontext-in-net-standard-library
		/// </summary>
		public HttpContext HttpContext {
			get { return _httpContextAccessor.HttpContext; }
		}
	}
}
