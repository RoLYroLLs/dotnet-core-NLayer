using System.Collections.Generic;
using System.Linq;
using NLayer;
using Xunit;

namespace Test.Unit {
	public class EmailAddressTests : BaseUnitTests {
		[Fact]
		public void CanCreateInstanceOfEmailAddress() {
			var emailAddress = new EmailAddress();

			Assert.NotNull(emailAddress);
		}

		[Fact]
		public void NewEmailAddressShouldDefaultToTypeNone() {
			var emailAddress = new EmailAddress();

			Assert.Equal(ContactType.None, emailAddress.ContactType);
		}

		[Fact]
		public void NullForEmailAddressTextIsInvalid() {
			var emailAddress = new EmailAddress();

			Assert.True(emailAddress.Validate().Count(x => x.MemberNames.Contains("EmailAddressText")) > 0);
		}

		[Fact]
		public void EmptyStringForEmailAddressTextIsInvalid() {
			var emailAddress = new EmailAddress() { EmailAddressText = string.Empty };

			Assert.True(emailAddress.Validate().Count(x => x.MemberNames.Contains("EmailAddressText")) > 0);
		}

		[Fact]
		public void EmptyEmailAddressHasValidatonMessageAboutMissingAddress() {
			var emailAddress = new EmailAddress();

			Assert.True(emailAddress.Validate().Count(x => x.MemberNames.Contains("EmailAddressText")) > 0);
		}

		[Fact]
		public void EmailAddressWithOwnerIsValid() {
			var emailAddress = new EmailAddress() { Owner = new Person() { FirstName = "John" } };

			Assert.Equal(0, emailAddress.Validate().Count(x => x.MemberNames.Contains("Owner")));
		}

		[Fact]
		public void EmailAddressWithTypeNoneIsInvalid() {
			var emailAddress = new EmailAddress() { EmailAddressText = "test@test.com", Owner = new Person() { FirstName = "John" } };

			Assert.True(emailAddress.Validate().Count(x => x.MemberNames.Contains("ContactType")) > 0);
		}

		[Fact]
		public void InvalidEmailAddressTextShouldInvalidateEmailAddress() {
			var emailAddress = new EmailAddress() { EmailAddressText = "John", Owner = new Person() { FirstName = "John" } };

			Assert.True(emailAddress.Validate().Count(x => x.MemberNames.Contains("EmailAddressText")) > 0);
		}

		[Fact]
		public void ConstructorWithInitialIListAddsToList() {
			IList<EmailAddress> initial = new List<EmailAddress> { new EmailAddress(), new EmailAddress() };
			var emailAddressCollection = new EmailAddressCollection(initial);

			Assert.Equal(2, emailAddressCollection.Count);
		}

		[Fact]
		public void ConstructorWithInitialCollectionAddsToList() {
			var initial = new EmailAddressCollection { new EmailAddress(), new EmailAddress() };
			var emailAddressCollection = new EmailAddressCollection(initial);

			Assert.Equal(2, emailAddressCollection.Count);
		}

		[Fact]
		public void OverloadOfAddAddsItemToCollection() {
			var emailAddressCollection = new EmailAddressCollection();
			const string addressText = "test@test.com";
			emailAddressCollection.Add(addressText, ContactType.Business);

			Assert.Single(emailAddressCollection);
			Assert.Equal(ContactType.Business, emailAddressCollection[0].ContactType);
			Assert.Equal(addressText, emailAddressCollection[0].EmailAddressText);
		}
	}
}
