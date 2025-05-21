namespace InsuraNova.Helpers
{
    public class EntityPatchHelper
    {
        public static void PatchEntity<T>(T existingEntity, T updatedEntity)
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var existingValue = property.GetValue(existingEntity);
                var updatedValue = property.GetValue(updatedEntity);

                // Only update if the value is not null and different from the existing value
                if (updatedValue != null && !updatedValue.Equals(existingValue))
                {
                    property.SetValue(existingEntity, updatedValue);
                }
                if (property.Name == "Version")
                {
                    var existingVersion = (int?)existingValue;
                    property.SetValue(existingEntity, (existingVersion ?? 0) + 1);
                }

            }
        }
    }
}
