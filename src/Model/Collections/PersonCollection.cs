using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NLayer {
	/// <summary>
	/// Represents a collection of PersonCollection instances in the system.
	/// </summary>
	public class PersonCollection : BaseCollection<Person> {
		/// <summary>
		/// Initializes a new instance of the <see cref="PersonCollection"/> class.
		/// </summary>
		public PersonCollection() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="PersonCollection"/> class.
		/// </summary>
		/// <param name="initialList">Accepts an IList of Person as the initial list.</param>
		public PersonCollection(IList<Person> initialList) : base(initialList) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="PersonCollection"/> class.
		/// </summary>
		/// <param name="initialList">Accepts a CollectionBase of Person as the initial list.</param>
		public PersonCollection(BaseCollection<Person> initialList) : base(initialList) { }

		/// <summary>
		/// Validates the current collection by validating each individual item in the collection.
		/// </summary>
		/// <returns>A IEnumerable of ValidationResult. The IEnumerable is empty when the object is in a valid state.</returns>
		public IEnumerable<ValidationResult> Validate() {
			var errors = new List<ValidationResult>();
			foreach (var person in this) {
				errors.AddRange(person.Validate());
			}
			return errors;
		}
	}
}
