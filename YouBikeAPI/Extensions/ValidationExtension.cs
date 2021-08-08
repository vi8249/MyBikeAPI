using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace YouBikeAPI.Extensions
{
	public static class ValidationExtension
	{
		public static async Task<int> SaveChangesWithValidationAsync(this DbContext context)
		{
			IEnumerable<EntityEntry> recordsToValidate = context.ChangeTracker.Entries();
			foreach (EntityEntry recordToValidate in recordsToValidate)
			{
				object entity = recordToValidate.Entity;
				ValidationContext validationContext = new ValidationContext(entity);
				List<ValidationResult> results = new List<ValidationResult>();
				if (!Validator.TryValidateObject(entity, validationContext, results, validateAllProperties: true))
				{
					results.Select((ValidationResult r) => r.ErrorMessage).ToList().Aggregate((string message, string nextMessage) => message + ", " + nextMessage);
					return -1;
				}
			}
			return await context.SaveChangesAsync();
		}
	}
}
