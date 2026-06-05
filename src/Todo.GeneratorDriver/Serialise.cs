using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Todo.GeneratorDriver;

public static class Serialise
{
    public static string Object(object obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles, // Optional: prevents reference loop errors
            
            // Add this resolver:
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    static typeInfo =>
                    {
                        for (int i = typeInfo.Properties.Count - 1; i >= 0; i--)
                        {
                            var property = typeInfo.Properties[i];
                    
                            // Skip properties that System.Text.Json can't serialize
                            if (property.PropertyType == typeof(ReadOnlySpan<byte>) ||
                                property.PropertyType.IsByRef ||
                                property.PropertyType.IsByRefLike || 
                                property.PropertyType.ContainsGenericParameters ||
                                property.PropertyType.Name.Contains("Encoding") ||
                                property.PropertyType.IsPointer)
                            {
                                typeInfo.Properties.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        });
    }

    public static void ObjectToFile(object obj)
    {
        var serialised = Object(obj);
        var path = @$"/Users/philipwhittington/Workspace/debug/{DateTime.Now:yyyy-MM-dd-HH-mm-ss}-{obj.GetType().Name}-{Guid.NewGuid()}.json";
        
        File.WriteAllText(path, serialised);
    }
}