using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YouBikeAPI.Extensions
{
    public static class ValidationExtension
    {
        public static async Task<int> SaveChangesWithValidationAsync(this DbContext context)
        {
            var recordsToValidate = context.ChangeTracker.Entries();
            foreach (var recordToValidate in recordsToValidate)
            {
                var entity = recordToValidate.Entity;
                var validationContext = new ValidationContext(entity);
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(entity, validationContext, results, true)) // Need to set all properties, otherwise it just checks required.
                {
                    var messages = results.Select(r => r.ErrorMessage).ToList().Aggregate((message, nextMessage) => message + ", " + nextMessage);
                    //throw new ApplicationException($"Unable to save changes for {entity.GetType().FullName} due to error(s): {messages}");
                    return -1;
                }
            }
            return await context.SaveChangesAsync();
        }
    }
}
