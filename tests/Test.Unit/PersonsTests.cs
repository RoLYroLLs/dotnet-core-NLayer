using System;
using System.Collections.Generic;
using System.Linq;
using NLayer;
using Xunit;

namespace Test.Unit {
	public class PersonTests : BaseUnitTests {
		[Fact]
		public void CanCreateInstance() {
			var person = new Person();

			Assert.NotNull(person);
		}

		[Fact]
		public void NewPersonHasInstantiatedEmailAddressCollectionAsNull() {
			var person = new Person();

			Assert.NotNull(person.EmailAddresses);
		}

		[Fact]
		public void NewPersonHasInstantiatedEmailAddressCollectionAsEmpty() {
			var person = new Person();

			Assert.Empty(person.EmailAddresses);
		}

		[Fact]
		public void NewPersonHasInstantiatedPhoneNumberCollectionAsNull() {
			var person = new Person();

			Assert.NotNull(person.PhoneNumbers);
		}

		[Fact]
		public void NewPersonHasInstantiatedPhoneNumberCollectionAsEmpty() {
			var person = new Person();

			Assert.Empty(person.PhoneNumbers);
		}

		[Fact]
		public void NewPersonShouldDefaultToTypeNone() {
			var person = new Person();

			Assert.Equal(PersonType.None, person.Type);
		}

		[Fact]
		public void FirstNameIsRequired() {
			var person = new Person();

			Assert.True(person.Validate().Count(x => x.MemberNames.Contains("FirstName")) > 0);
		}

		[Fact]
		public void LastNameIsRequired() {
			var person = new Person();

			Assert.True(person.Validate().Count(x => x.MemberNames.Contains("LastName")) > 0);
		}

		[Fact]
		public void PersonWithTypeNoneIsInvalid() {
			var person = CreatePerson();
			person.Type = PersonType.None;

			Assert.True(person.Validate().Count(x => x.MemberNames.Contains("Type")) > 0);
		}

		[Fact]
		public void DateOfBirthMustBeOnOBeforeToday() {
			var person = CreatePerson();
			person.DateOfBirth = DateTime.Now.AddDays(1);

			Assert.True(person.Validate().Count(x => x.MemberNames.Contains("DateOfBirth")) > 0);
			Assert.True(person.Validate().Count(x => x.ErrorMessage.Contains("Invalid range")) > 0);
		}

		[Fact]
		public void DateOfBirthBeforeTodayIsOk() {
			var person = CreatePerson();
			person.DateOfBirth = DateTime.Now.AddDays(-1);

			Assert.Equal(0, person.Validate().Count(x => x.MemberNames.Contains("DateOfBirth")));
		}

		[Fact]
		public void TodayIsAValidDateOfBirth() {
			var person = CreatePerson();
			person.DateOfBirth = DateTime.Now;

			Assert.Equal(0, person.Validate().Count(x => x.MemberNames.Contains("DateOfBirth")));
		}

		[Fact]
		public void DateOfBirthMustBeLessThan130YearsAgo() {
			var person = CreatePerson();
			person.DateOfBirth = DateTime.Now.AddYears(-130).AddDays(-1);

			Assert.True(person.Validate().Count(x => x.MemberNames.Contains("DateOfBirth")) > 0);
			Assert.True(person.Validate().Count(x => x.ErrorMessage.Contains("Invalid range")) > 0);
		}

		[Fact]
		public void DateOfBirthLessThan130YearsAgoIsValid() {
			var person = CreatePerson();
			person.DateOfBirth = DateTime.Now.AddYears(-130).AddDays(1);

			Assert.Equal(0, person.Validate().Count(x => x.MemberNames.Contains("DateOfBirth")));
		}

		[Theory]
		[InlineData("John", "Doe", "John Doe")]
		[InlineData("John", "", "John")]
		[InlineData("", "Doe", "Doe")]
		[InlineData("", "", "")]
		public void FirstAndLastNameWithoutSpaces(string firstName, string lastName, string expected) {
			var person = new Person { FirstName = firstName, LastName = lastName };

			Assert.Equal(expected, person.FullName);
		}

		[Theory]
		[InlineData("John", " Doe", "John Doe")]
		[InlineData("John ", "Doe", "John Doe")]
		[InlineData("John", " ", "John")]
		[InlineData(" ", "Doe", "Doe")]
		public void FirstAndLastNameWithSpaces(string firstName, string lastName, string expected) {
			var person = new Person { FirstName = firstName, LastName = lastName };

			Assert.NotEqual(expected, person.FullName);
		}

		[Fact]
		public void AllEmptyReturnsEmpty() {
			var person = new Person();

			Assert.Equal(string.Empty, person.FullName);
		}

		[Fact]
		public void TwoPersonCollectionWithSameIdAreTheSame() {
			var person1 = new Person { Id = 1, FirstName = "John", LastName = "Doe" };
			var person2 = new Person { Id = 1, FirstName = "John", LastName = "Doe" };

			Assert.True(person1 == person2);
		}

		[Fact]
		public void CanAddEmailAddressToNewPerson() {
			var person = new Person();
			person.EmailAddresses.Add(new EmailAddress());

			Assert.Single(person.EmailAddresses);
		}

		[Fact]
		public void CanAddPhoneNumberToNewPerson() {
			var person = new Person();
			person.PhoneNumbers.Add(new PhoneNumber());

			Assert.Single(person.PhoneNumbers);
		}

		[Fact]
		public void NewPersonHasInstantiatedWorkAddress() {
			var person = new Person();

			Assert.NotNull(person.WorkAddress);
		}

		[Fact]
		public void NewPersonHasInstantiatedHomeAddress() {
			var person = new Person();

			Assert.NotNull(person.HomeAddress);
		}

		[Fact]
		public void PersonDotValidateDetectsInvalidPhoneNumber() {
			var person = CreatePerson();
			person.PhoneNumbers.Add("", ContactType.None);
			var errors = person.Validate();

			Assert.Single(errors.Where(x => x.ErrorMessage.Contains("The Number field is required")));
		}

		[Fact]
		public void PersonDotValidateDetectsMissingPhonenumberType() {
			var person = CreatePerson();
			person.PhoneNumbers.Add("(555)1234567", ContactType.None);
			var errors = person.Validate();

			Assert.Single(errors.Where(x => x.ErrorMessage.Contains("ContactType can't be None")));
		}

		[Fact]
		public void PersonDotValidateDetectsInvalidEmailAddress() {
			var person = CreatePerson();
			person.EmailAddresses.Add("", ContactType.None);
			var errors = person.Validate();

			Assert.Single(errors.Where(x => x.ErrorMessage.Contains("The EmailAddressText field is required")));
		}

		[Fact]
		public void PersonDotValidateDetectsMissingEmailAddressType() {
			var person = CreatePerson();
			person.EmailAddresses.Add("test@example.com", ContactType.None);
			var errors = person.Validate();

			Assert.Single(errors.Where(x => x.ErrorMessage.Contains("ContactType can't be None")));
		}

		[Fact]
		public void PersonDotValidateDetectsInvalidHomeAddress() {
			var person = CreatePerson();
			person.HomeAddress = new Address("Street", null, null, null, ContactType.Personal);
			var errors = person.Validate();

			Assert.Single(errors.Where(x => x.ErrorMessage.Contains("City can't be null or empty")));
		}

		[Fact]
		public void PersonDotValidateDetectsInvalidWorkAddress() {
			var person = CreatePerson();
			person.WorkAddress = new Address("Street", null, null, null, ContactType.Personal);
			var errors = person.Validate();

			Assert.Single(errors.Where(x => x.ErrorMessage.Contains("City can't be null or empty")));
		}

		[Fact]
		public void ConstructorWithInitialIListAddsToList() {
			IList<Person> initial = new List<Person> { new Person(), new Person() };
			var personCollection = new PersonCollection(initial);

			Assert.Equal(2, personCollection.Count);
		}

		[Fact]
		public void ConstructorWithInitialCollectionAddsToList() {
			var initial = new PersonCollection { new Person(), new Person() };
			var personCollection = new PersonCollection(initial);

			Assert.Equal(2, personCollection.Count);
		}

		[Fact]
		public void ValidateOnPersonCollectionDetectsInvalidPersonCollection() {
			var personCollection = new PersonCollection { new Person { FirstName = "John" }, new Person { LastName = "Doe" } };
			var result = personCollection.Validate().ToList();

			Assert.Single(result.Where(x => x.ErrorMessage.Contains("The LastName field is required"))); // for Person 1
			Assert.Single(result.Where(x => x.ErrorMessage.Contains("The FirstName field is required"))); // for Person 2
		}


		private static Person CreatePerson() {
			return new Person { FirstName = "John", LastName = "Doe", Type = PersonType.Friend, DateOfBirth = DateTime.Now.AddYears(-20) };
		}
	}
}
