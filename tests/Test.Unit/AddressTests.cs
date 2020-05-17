using System.Linq;
using NLayer;
using Xunit;

namespace Test.Unit {
	public class AddressTests : BaseUnitTests {
		[Fact]
		public void TwoIdenticalAddressesShouldBeTheSame() {
			var address1 = new Address("Street", "City", "ZipCode", "Country", ContactType.Business);
			var address2 = new Address("Street", "City", "ZipCode", "Country", ContactType.Business);

			Assert.True(address1 == address2);
		}

		[Fact]
		public void DifferentAddressesShouldNotBeTheSame() {
			var address1 = new Address("Some other street", "City", "ZipCode", "Country", ContactType.Business);
			var address2 = new Address("Street", "City", "ZipCode", "Country", ContactType.Business);
			
			Assert.True(address1 != address2);
		}

		[Fact]
		public void CanCreateInstanceOfAddress() {
			var address = new Address(null, null, null, null, ContactType.Business);

			Assert.NotNull(address);
		}

		[Fact]
		public void EmptyAddressIsNull() {
			var address = new Address(null, null, null, null, ContactType.Business);

			Assert.True(address.IsNull);
		}

		[Fact]
		public void AddressWithValuesIsNotNull() {
			var address = new Address("Street", "City", "ZipCode", "Country", ContactType.Business);

			Assert.NotNull(address);
		}

		[Fact]
		public void AddressWithSomeValuesIsNotNull() {
			var address = new Address(null, null, "ZipCode", "Country", ContactType.Business);

			Assert.NotNull(address);
		}

		[Fact]
		public void NewAddressShouldBeOfCorrectType() {
			var address = new Address(null, null, null, null, ContactType.Business);

			Assert.Equal(ContactType.Business, address.ContactType);
		}

		[Fact]
		public void EmptyAddressIsNotInvalid() {
			var address = new Address(null, null, null, null, ContactType.Business);
			var errors = address.Validate();

			Assert.Empty(errors);
		}

		[Fact]
		public void PartialAddressHasValidatonMessageAboutMissingStreet() {
			var address = new Address(null, null, null, "Country", ContactType.Business);

			Assert.True(address.Validate().Count(x => x.MemberNames.Contains("Street")) > 0);
		}

		[Fact]
		public void PartialAddressHasValidatonMessageAboutMissingZipCode() {
			var address = new Address(null, null, null, "Country", ContactType.Business);

			Assert.True(address.Validate().Count(x => x.MemberNames.Contains("ZipCode")) > 0);
		}

		[Fact]
		public void PartialAddressHasValidatonMessageAboutMissingCity() {
			var address = new Address(null, null, null, "Country", ContactType.Business);

			Assert.True(address.Validate().Count(x => x.MemberNames.Contains("City")) > 0);
		}

		[Fact]
		public void PartialAddressHasValidatonMessageAboutMissingCountry() {
			var address = new Address(null, "City", null, null, ContactType.Business);

			Assert.True(address.Validate().Count(x => x.MemberNames.Contains("Country")) > 0);
		}

		[Fact]
		public void AddressWithTypeNoneIsInvalid() {
			var address = new Address("Street", "City", "ZipCode", "Country", ContactType.None);

			Assert.True(address.Validate().Count(x => x.MemberNames.Contains("ContactType")) > 0);
		}
	}
}
