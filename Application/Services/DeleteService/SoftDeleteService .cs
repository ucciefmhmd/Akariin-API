using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.DeleteService
{
    public class SoftDeleteService(ApplicationDbContext _dbContext) : ISoftDeleteService
    {
        public async Task<(bool Success, string Message)> SoftDeleteAsync<T>(long id, CancellationToken cancellationToken = default) where T : class
        {
            var entity = await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
            if (entity == null)
            {
                return (false, $"{typeof(T).Name} with ID {id} not found.");
            }

            // Check for related entities
            bool hasReferences = await IsPrimaryKeyReferencedAsync<T>(id, cancellationToken);
            if (hasReferences)
            {
                return (false, $"Can't delete {typeof(T).Name} because it has related records.");
            }

            // Set soft delete properties
            SetSoftDeleteProperties(entity);

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return (true, $"{typeof(T).Name} deleted successfully.");
        }

        public async Task<bool> IsPrimaryKeyReferencedAsync<T>(long id, CancellationToken cancellationToken = default) where T : class
        {
            var entityType = _dbContext.Model.FindEntityType(typeof(T));
            if (entityType == null) return false;

            var primaryKey = entityType.FindPrimaryKey()?.Properties.FirstOrDefault();
            if (primaryKey == null) return false; // No primary key found

            foreach (var otherEntityType in _dbContext.Model.GetEntityTypes())
            {
                foreach (var foreignKey in otherEntityType.GetForeignKeys())
                {
                    if (foreignKey.PrincipalEntityType == entityType)
                    {
                        var dependentEntityType = otherEntityType.ClrType;
                        var foreignKeyProperty = foreignKey.Properties.FirstOrDefault();
                        if (foreignKeyProperty == null) continue;

                        var relatedDbSet = _dbContext.GetType()
                            .GetMethod("Set", Type.EmptyTypes)?
                            .MakeGenericMethod(dependentEntityType)
                            .Invoke(_dbContext, null) as IQueryable<object>;

                        if (relatedDbSet == null) continue;

                        // Step 1: Fetch related records where IsDeleted == false
                        var relatedRecords = await relatedDbSet
                            .Where(x =>
                                EF.Property<long>(x, foreignKeyProperty.Name) == id &&
                                (!EF.Property<bool>(x, "IsDeleted"))) // Ensuring IsDeleted == false
                            .ToListAsync(cancellationToken);

                        // Step 2: Check if any record has non-null fields other than the foreign key
                        bool hasMeaningfulData = relatedRecords.Any(record =>
                            otherEntityType.GetProperties()
                                .Where(p => p.Name != foreignKeyProperty.Name && p.Name != "IsDeleted") // Exclude FK and IsDeleted
                                .Any(p => p.PropertyInfo.GetValue(record) != null) // Check if any field has data
                        );

                        if (hasMeaningfulData)
                        {
                            return true; // Found a reference with meaningful data
                        }
                    }
                }
            }

            return false; // No references found
        }


        private void SetSoftDeleteProperties(object entity)
        {
            var type = entity.GetType();

            var isActiveProperty = type.GetProperty("IsActive");
            var isDeletedProperty = type.GetProperty("IsDeleted");
            var modifiedDateProperty = type.GetProperty("ModifiedDate");

            isActiveProperty?.SetValue(entity, false);
            isDeletedProperty?.SetValue(entity, true);
            modifiedDateProperty?.SetValue(entity, DateTime.UtcNow);
        }
    }
}
