using System.Collections.Generic;

namespace NLayer {
	/// <summary>
	/// This interface is used to handle concurrency of an object.
	/// </summary>
	public interface IHandlesConcurrency {
		/// <summary>
		/// The Version of this object.
		/// </summary>
		IEnumerable<byte> Version { get; set; }
	}
}
