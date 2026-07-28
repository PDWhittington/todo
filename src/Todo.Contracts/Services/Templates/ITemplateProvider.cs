using Todo.Contracts.Data.Markdown;

namespace Todo.Contracts.Services.Templates;

public interface ITemplateProvider
{
    TodoFile GetTemplate();
}