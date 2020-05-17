using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace NLayer {
	/// <summary>
	/// Defines a Unit of Work using an EF DbContext under the hood.
	/// </summary>
	public class EFUnitOfWork<T> : IDisposable, IUnitOfWork<T> where T : DbContext, new() {
		/// <summary>
		/// A field to hold the http context.
		/// https://stackoverflow.com/questions/47956577/an-alternative-for-httpcontext-current-items-containsdatacontextkey-in-asp-net
		/// https://stackoverflow.com/questions/47357689/httpcontext-in-net-standard-library
		/// </summary>
		private readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// Initializes a new instance of the EFUnitOfWork class.
		/// </summary>
		/// <param name="forceNewContext">When true, clears out any existing data context first.</param>
		public EFUnitOfWork(bool forceNewContext, IHttpContextAccessor httpContextAccessor) {
			_httpContextAccessor = httpContextAccessor;
			if (forceNewContext) {
				DataContextFactory<T>.Clear(_httpContextAccessor);
			}
		}

		/// <summary>
		/// Saves the changes to the underlying DbContext.
		/// </summary>
		/// <param name="resetAfterCommit">When true, clears out the data context afterwards.</param>
		public void Commit(bool resetAfterCommit) {
			DataContextFactory<T>.GetDataContext(_httpContextAccessor).SaveChanges();
			if (resetAfterCommit) {
				DataContextFactory<T>.Clear(_httpContextAccessor);
			}
		}

		/// <summary>
		/// Undoes changes to the current DbContext by removing it from the storage container.
		/// </summary>
		public void Undo() {
			DataContextFactory<T>.Clear(_httpContextAccessor);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		/// <summary>
		/// Saves the changes to the underlying DbContext.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				if (disposing) {
					// TODO: dispose managed state (managed objects).
					DataContextFactory<T>.GetDataContext(_httpContextAccessor).SaveChanges();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~EFUnitOfWork() {
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
