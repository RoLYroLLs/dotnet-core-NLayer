using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NLayer;
using Xunit;

namespace Test.Unit {
	public class DomainEntityTests : BaseUnitTests {
		#region Nested helpers

		internal class PersonWithIntAsId : DomainEntity<int> {
			public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
				throw new NotImplementedException();
			}
		}

		internal class PersonWithGuidAsId : DomainEntity<Guid> {
			public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
				throw new NotImplementedException();
			}
		}

		#endregion

		[Fact]
		public void NewPersonWithIntAsIdIsTransient() {
			var person = new PersonWithIntAsId();

			Assert.True(person.IsTransient());
			//person.IsTransient().Should().BeTrue();
		}

		[Fact]
		public void PersonWithIntAsIdWithValueIsNotTransient() {
			var person = new PersonWithIntAsId { Id = 4 };

			Assert.False(person.IsTransient());
			//person.IsTransient().Should().BeFalse();
		}

		[Fact]
		public void NewPersonWithGuidAsIdIsTransient() {
			var person = new PersonWithGuidAsId();

			Assert.Equal(Guid.Empty, person.Id);
			Assert.True(person.IsTransient());
			//person.Id.Should().Be(Guid.Empty);
			//person.IsTransient().Should().BeTrue();
		}

		[Fact]
		public void PersonWithGuidAsIdWithValueIsNotTransient() {
			var person = new PersonWithGuidAsId { Id = Guid.NewGuid() };

			Assert.False(person.IsTransient());
			//person.IsTransient().Should().BeFalse();
		}

		[Fact]
		public void SameIdentityProduceEqualsTrueTest() {
			var entityLeft = new PersonWithIntAsId { Id = 1 };
			var entityRight = new PersonWithIntAsId { Id = 1 };

			//Act
			bool resultOnEquals = entityLeft.Equals(entityRight);
			bool resultOnOperator = entityLeft == entityRight;

			//Assert
			Assert.True(resultOnEquals);
			Assert.True(resultOnOperator);
			//resultOnEquals.Should().BeTrue();
			//resultOnOperator.Should().BeTrue();
		}

		[Fact]
		public void DifferentIdProduceEqualsFalseTest() {
			//Arrange
			var entityLeft = new PersonWithIntAsId { Id = 1 };
			var entityRight = new PersonWithIntAsId { Id = 2 };

			//Act
			bool resultOnEquals = entityLeft.Equals(entityRight);
			bool resultOnOperator = entityLeft == entityRight;

			//Assert
			Assert.False(resultOnEquals);
			Assert.False(resultOnOperator);
			//resultOnEquals.Should().BeFalse();
			//resultOnOperator.Should().BeFalse();
		}

		[Fact]
		public void CompareUsingEqualsOperatorsAndNullOperandsTest() {
			//Arrange

			PersonWithIntAsId entityLeft = null;
			PersonWithIntAsId entityRight = new PersonWithIntAsId { Id = 2 };

			//Act
			if (!(entityLeft == null)) {
				Assert.True(false, "entityLeft is not null");
			}

			if (!(entityRight != null)) {
				Assert.True(false, "entityRight is null");
			}

			entityRight = null;

			//Act
			if (!(entityLeft == entityRight)) {
				Assert.True(false, "entityLeft is not equal to entityRight");
			}

			if (entityLeft != entityRight) {
				Assert.True(false, "entityLeft is not equal to entityRight");
			}


		}

		[Fact]
		public void CompareTheSameReferenceReturnTrueTest() {
			//Arrange
			var entityLeft = new PersonWithIntAsId();
			PersonWithIntAsId entityRight = entityLeft;

			//Act
			if (!entityLeft.Equals(entityRight)) {
				Assert.True(false, "entityLeft is not equal to entityRight");
			}

			if (!(entityLeft == entityRight)) {
				Assert.True(false, "entityLeft is not equal to entityRight");
			}
		}

		[Fact]
		public void CompareWhenLeftIsNullAndRightIsNullReturnFalseTest() {
			//Arrange
			PersonWithIntAsId entityLeft = null;
			var entityRight = new PersonWithIntAsId { Id = 1 };

			//Act
			if (!(entityLeft == null)) { //this perform ==(left,right)
				Assert.True(false, "entityLeft is not null");
			}

			if (!(entityRight != null)) { //this perform !=(left,right)
				Assert.True(false, "entityRight is null");
			}
		}

		[Fact]
		public void SetIdentitySetANonTransientEntity() {
			//Arrange
			var entity = new PersonWithIntAsId { Id = 1 };

			//Assert
			Assert.False(entity.IsTransient());
		}
	}
}
