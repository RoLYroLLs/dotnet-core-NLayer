using System;
using System.Collections.Generic;
using NLayer;
using Xunit;

namespace Test.Unit {
	public class CollectionBaseTests : BaseUnitTests {
		[Fact]
		public void NewCollectionUsingNewListsAddsValues() {
			var collection = new IntCollection(new List<int> { 1, 2, 3 });

			Assert.Equal(3, collection.Count);
		}

		[Fact]
		public void NewCollectionUsingExistingCollectionAddsValues() {
			var collection1 = new IntCollection(new List<int> { 1, 2, 3 });
			var collection2 = new IntCollection(collection1);

			Assert.Equal(3, collection2.Count);
		}

		[Fact]
		public void UsingAddRangeAddsValues() {
			var collection1 = new IntCollection(new List<int> { 1, 2, 3 });
			var collection2 = new IntCollection();

			collection2.AddRange(collection1);

			Assert.Equal(3, collection2.Count);
		}

		[Fact]
		public void SortPersonCollectionWithSpecifiedComparerSortsCorrectly() {
			var personCollection = new PersonCollection();
			personCollection.Add(new Person { FirstName = "John", LastName = "Doe" });
			personCollection.Add(new Person { FirstName = "Jane", LastName = "Doe" });

			personCollection.Sort(new PersonComparer());

			Assert.Equal("Jane Doe", personCollection[0].FullName);
			Assert.Equal("John Doe", personCollection[1].FullName);
		}

		[Fact]
		public void SortIntsSorts() {
			var ints = new IntCollection { 3, 2, 1 };
			ints.Sort();

			Assert.Equal(1, ints[0]);
			Assert.Equal(2, ints[1]);
			Assert.Equal(3, ints[2]);
		}

		[Fact]
		public void AddRangeThrowsWhenCollectionIsNull() {
			Action act = () => {
				var collection = new IntCollection();
				collection.AddRange(null);
			};

			var exception = Assert.Throws<ArgumentNullException>(act);
			Assert.Contains("collection is null", exception.Message);
		}
	}

	internal class IntCollection : BaseCollection<int> {
		public IntCollection() { }

		public IntCollection(IList<int> initialList)
		  : base(initialList) { }

		public IntCollection(BaseCollection<int> initialList)
		  : base(initialList) { }
	}

	public class PersonComparer : IComparer<Person> {
		public int Compare(Person x, Person y) {
			return x.FullName.CompareTo(y.FullName);
		}
	}
}
