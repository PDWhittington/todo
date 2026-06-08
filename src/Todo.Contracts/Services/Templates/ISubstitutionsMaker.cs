using Todo.Contracts.Data.Memory;
using Todo.Contracts.Data.Substitutions;

namespace Todo.Contracts.Services.Templates;

public interface ISubstitutionsMaker<in T> where T : SubstitutionsBase
{
    void WriteSubstitutionsToStream(UnmanagedByteArray template, 
        T substitutions, Stream stream);
}
