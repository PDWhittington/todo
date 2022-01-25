using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.Templates;

public interface ITemplateProvider<in TKeyType>
{
    TodoFile GetTemplate(TKeyType key);
}
