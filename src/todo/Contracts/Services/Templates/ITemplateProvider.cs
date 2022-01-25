using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.Templates;

public interface ITemplateProvider<TKeyType>
{
    TodoFile GetTemplate(TKeyType key);
}
