using System.Collections.Generic;

namespace NLayer {
	/// <summary>
	/// Defines the various methods available to work with person in the system.
	/// </summary>
	public interface IPersonRepository : IRepository<Person, int> {
		/// <summary>
		/// Gets a list of all the person whose last name exactly matches the search string.
		/// </summary>
		/// <param name="lastName">The last name that the system should search for.</param>
		/// <returns>An IEnumerable of person with the matching Person.</returns>
		IEnumerable<Person> FindByLastName(string lastName);
	}
}
