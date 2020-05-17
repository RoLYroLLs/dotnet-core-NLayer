using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NLayer {
	/// <summary>
	/// Serves as the base class for all collections.
	/// </summary>
	/// <typeparam name="T">A type parameter to determine the type in the collection.</typeparam>
	public abstract class BaseCollection<T> : Collection<T>, IList<T> {
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCollection{T}"/> class.
		/// </summary>
		protected BaseCollection() : base(new List<T>()) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCollection{T}"/> class.
		/// </summary>
		/// <param name="initialList">Accepts an IList of T as the initial list.</param>
		protected BaseCollection(IList<T> initialList) : base(initialList) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCollection{T}"/> class.
		/// </summary>
		/// <param name="initialList">Accepts a CollectionBase of T as the initial list.</param>
		protected BaseCollection(BaseCollection<T> initialList) : base(initialList) { }

		/// <summary>
		/// Sorts the collection based on the specified comparer.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public void Sort(IComparer<T> comparer) {
			if (Items is List<T> list) {
				list.Sort(comparer);
			}
		}

		/// <summary>
		/// Sorts the collection based on the specified comparer. Uses equals on the objects being compared.
		/// </summary>
		public void Sort() {
			if (Items is List<T> list) {
				list.Sort();
			}
		}

		/// <summary>
		/// Adds a range of T instances to the current collection.
		/// </summary>
		/// <param name="collection">The collection of T instances that must be added.</param>
		public void AddRange(IEnumerable<T> collection) {
			if (collection == null) {
				throw new ArgumentNullException(nameof(collection), "Parameter collection is null.");
			}
			foreach (var item in collection) {
				Add(item);
			}
		}
	}
}
