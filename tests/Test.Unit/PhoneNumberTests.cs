using System;
using System.Collections.Generic;
using System.Linq;
using NLayer;
using Xunit;

namespace Test.Unit {
	public class PhoneNumberTests : BaseUnitTests {
		[Fact]
		public void CanCreateInstanceOfPhoneNumber() {
			var phoneNumber = new PhoneNumber();

			Assert.NotNull(phoneNumber);
		}

		[Fact]
		public void NewPhoneNumberCollectionhouldDefaultToTypeNone() {
			var phoneNumber = new PhoneNumber();

			Assert.Equal(ContactType.None, phoneNumber.ContactType);
		}

		[Fact]
		public void EmptyPhoneNumberIsInvalid() {
			var phoneNumber = new PhoneNumber();

			Assert.True(phoneNumber.Validate().Count() > 0);
		}

		[Fact]
		public void EmptyPhoneNumberHasValidatonMessageAboutMissingNumber() {
			var phoneNumber = new PhoneNumber() { Owner = new Person() };

			Assert.True(phoneNumber.Validate().Count(x => x.MemberNames.Contains("Number")) > 0);
		}

		[Fact]
		public void PhoneNumberWithTypeNoneIsInvalid() {
			var phoneNumber = new PhoneNumber() { Number = "555-1234567", Owner = new Person() { FirstName = "John" } };

			Assert.True(phoneNumber.Validate().Count(x => x.MemberNames.Contains("ContactType")) > 0);
		}

		[Fact]
		public void ConstructorWithInitialIListAddsToList() {
			IList<PhoneNumber> initial = new List<PhoneNumber> { new PhoneNumber(), new PhoneNumber() };
			var phoneNumberCollection = new PhoneNumberCollection(initial);

			Assert.Equal(2, phoneNumberCollection.Count);
		}

		[Fact]
		public void ConstructorWithInitialCollectionAddsToList() {
			var initial = new PhoneNumberCollection { new PhoneNumber(), new PhoneNumber() };
			var phoneNumberCollection = new PhoneNumberCollection(initial);

			Assert.Equal(2, phoneNumberCollection.Count);
		}
	}
}
