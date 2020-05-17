using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NLayer {
	/// <summary>
	/// This is the main DbContext to work with data in the database.
	/// </summary>
	public class BaseDataContext : DbContext {
		/// <summary>
		/// Initializes a new instance of the BarryUSqlContext class.
		/// </summary>
		public BaseDataContext(DbContextOptions<BaseDataContext> options)
			: base(options) {
		}

		/// <summary>
		/// Hooks into the Save process to get a last-minute chance to look at the entities and change them. Also intercepts exceptions and 
		/// wraps them in a new Exception type.
		/// </summary>
		/// <returns>The number of affected rows.</returns>
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
			ValidateEntries();

			return base.SaveChangesAsync(cancellationToken);
		}

		/// <summary>
		/// Hooks into the Save process to get a last-minute chance to look at the entities and change them. Also intercepts exceptions and 
		/// wraps them in a new Exception type.
		/// </summary>
		/// <returns>The number of affected rows.</returns>
		public override int SaveChanges() {
			ValidateEntries();

			return base.SaveChanges();

			//// Need to manually delete all "owned objects" that have been removed from their owner, otherwise they'll be orphaned.
			//var orphanedObjects = ChangeTracker.Entries().Where(
			//	e => (e.State == EntityState.Modified || e.State == EntityState.Added) &&
			//		e.Entity.GetType().GetInterfaces().Any(x => x.IsGenericType &&
			//			x.GetGenericTypeDefinition() == typeof(IHasOwner<>)) &&
			//		e.Reference("Owner").CurrentValue == null);

			//foreach (var orphanedObject in orphanedObjects) {
			//	orphanedObject.State = EntityState.Deleted;
			//}

			//try {
			//	var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
			//	foreach (var item in modified) {
			//		if (item.Entity is IDateTracking changedOrAddedItem) {
			//			changedOrAddedItem.DateModified = DateTime.Now;
			//			if (item.State == EntityState.Added) {
			//				changedOrAddedItem.DateCreated = changedOrAddedItem.DateModified;
			//			}
			//		}

			//		if (item.Entity is IHandlesConcurrency) {
			//			if (item.State == EntityState.Modified) {
			//				item.Property("Version").OriginalValue = item.Property("Version").CurrentValue;
			//			}
			//		}
			//	}

			//	return base.SaveChanges();
			//} catch (DbEntityValidationException entityException) {
			//	var errors = entityException.EntityValidationErrors;
			//	var result = new StringBuilder();
			//	var allErrors = new List<ValidationResult>();
			//	foreach (var error in errors) {
			//		foreach (var validationError in error.ValidationErrors) {
			//			result.AppendFormat("\r\n  Entity of type {0} has validation error \"{1}\" for property {2}.\r\n", error.Entry.Entity.GetType(), validationError.ErrorMessage, validationError.PropertyName);
			//			var domainEntity = error.Entry.Entity as DomainEntity<int>;
			//			if (domainEntity != null) {
			//				result.Append(domainEntity.IsTransient() ? "  This entity was added in this session.\r\n" : $"  The Id of the entity is {domainEntity.Id}.\r\n");
			//			}

			//			allErrors.Add(new ValidationResult(validationError.ErrorMessage, new[] { validationError.PropertyName }));
			//		}
			//	}

			//	throw new ModelValidationException(result.ToString(), entityException, allErrors);
			//}
		}

		/// <summary>
		/// Configures the EF context.
		/// </summary>
		/// <param name="modelBuilder">The model builder that needs to be configured.</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			if (modelBuilder == null) {
				return;
			}

			foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.Name == "Id")) {
				property.IsKey();
				property.SetColumnName($"{(property.ClrType != null && property.ClrType.ReflectedType != null ? property.ClrType.Name : string.Empty)}Id");
			}
		}

		/// <summary>
		/// Validates the Entities.
		/// </summary>
		protected virtual void ValidateEntries() {
			// Need to manually delete all "owned objects" that have been removed from their owner, otherwise they'll be orphaned.
			var orphanedObjects = ChangeTracker.Entries().Where(
				e => (e.State == EntityState.Modified || e.State == EntityState.Added) &&
				e.Entity.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHasOwner<>)) &&
				e.Reference("Owner").CurrentValue == null);

			foreach (var orphanedObject in orphanedObjects) {
				orphanedObject.State = EntityState.Deleted;
			}

			var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
			foreach (var entry in entries) {
				// Validate entity, throws ValidationException if validation fails
				// https://stackoverflow.com/questions/46430619/net-core-2-ef-core-error-handling-save-changes
				// https://www.bricelam.net/2016/12/13/validation-in-efcore.html
				// https://stackoverflow.com/questions/54058196/ef-core-not-raising-validationexception (entry.Entity)
				var validationContext = new ValidationContext(entry.Entity);
				Validator.ValidateObject(entry.Entity, validationContext, validateAllProperties: true);

				if (entry.Entity is IDateTracking changedOrAddedItem) {
					changedOrAddedItem.DateModified = DateTime.UtcNow;
					if (entry.State == EntityState.Added) {
						changedOrAddedItem.DateCreated = changedOrAddedItem.DateModified;
					}
				}

				if (entry.Entity is IHandlesConcurrency) {
					if (entry.State == EntityState.Modified) {
						entry.Property("Version").OriginalValue = entry.Property("Version").CurrentValue;
					}
				}
			}
		}
	}
}
