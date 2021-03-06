﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NLayer {
	/// <summary>
	/// Represents a collection of EmailAddress instances in the system.
	/// </summary>
	public class EmailAddressCollection : BaseCollection<EmailAddress> {
		/// <summary>
		/// Initializes a new instance of the <see cref="EmailAddressCollection"/> class.
		/// </summary>
		public EmailAddressCollection() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailAddressCollection"/> class.
		/// </summary>
		/// <param name="initialList">Accepts an IList of EmailAddress as the initial list.</param>
		public EmailAddressCollection(IList<EmailAddress> initialList) : base(initialList) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailAddressCollection"/> class.
		/// </summary>
		/// <param name="initialList">Accepts a CollectionBase of EmailAddress as the initial list.</param>
		public EmailAddressCollection(BaseCollection<EmailAddress> initialList) : base(initialList) { }

		/// <summary>
		/// Adds a new instance of EmailAddress to the collection.
		/// </summary>
		/// <param name="emailAddressText">The e-mail address text.</param>
		/// <param name="contactType">The type of the e-mail address.</param>
		public void Add(string emailAddressText, ContactType contactType) {
			Add(new EmailAddress { ContactType = contactType, EmailAddressText = emailAddressText });
		}

		/// <summary>
		/// Validates the current collection by validating each individual item in the collection.
		/// </summary>
		/// <returns>A IEnumerable of ValidationResult. The IEnumerable is empty when the object is in a valid state.</returns>
		public IEnumerable<ValidationResult> Validate() {
			var errors = new List<ValidationResult>();
			foreach (var address in this) {
				errors.AddRange(address.Validate());
			}
			return errors;
		}
	}
}
