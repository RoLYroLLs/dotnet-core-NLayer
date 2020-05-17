using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NLayer {
	/// <summary>
	/// Represents a collection of PhoneNumber instances in the system.
	/// </summary>
	public class PhoneNumberCollection : BaseCollection<PhoneNumber> {
		/// <summary>
		/// Initializes a new instance of the <see cref="PhoneNumberCollection"/> class.
		/// </summary>
		public PhoneNumberCollection() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="PhoneNumberCollection"/> class.
		/// </summary>
		/// <param name="initialList">Accepts an IList of PhoneNumber as the initial list.</param>
		public PhoneNumberCollection(IList<PhoneNumber> initialList) : base(initialList) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="PhoneNumberCollection"/> class.
		/// </summary>
		/// <param name="initialList">Accepts a CollectionBase of PhoneNumber as the initial list.</param>
		public PhoneNumberCollection(BaseCollection<PhoneNumber> initialList) : base(initialList) { }

		/// <summary>
		/// Adds a new instance of PhoneNumber to the collection.
		/// </summary>
		/// <param name="number">The e-phone number text.</param>
		/// <param name="contactType">The type of the phone number.</param>
		public void Add(string number, ContactType contactType) {
			Add(new PhoneNumber() { ContactType = contactType, Number = number });
		}

		/// <summary>
		/// Validates the current collection by validating each individual item in the collection.
		/// </summary>
		/// <returns>A IEnumerable of ValidationResult. The IEnumerable is empty when the object is in a valid state.</returns>
		public IEnumerable<ValidationResult> Validate() {
			var errors = new List<ValidationResult>();
			foreach (var number in this) {
				errors.AddRange(number.Validate());
			}
			return errors;
		}
	}
}
