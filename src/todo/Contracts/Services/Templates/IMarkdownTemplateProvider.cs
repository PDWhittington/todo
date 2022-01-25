namespace Todo.Contracts.Services.Templates;

public enum MarkdownTemplateEnum
{
    DayListTemplate,
    TopicListTemplate
}

public interface IMarkdownTemplateProvider : ITemplateProvider<MarkdownTemplateEnum> { }
