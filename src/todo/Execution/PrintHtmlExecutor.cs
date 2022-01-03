using Markdig;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;

namespace Todo.Execution;

public class PrintHtmlExecutor : IPrintHtmlExecutor
{
    private const string _test = @"# Heading 1

## Heading 2

* Bullet 1
* Bullet 2";

    public void Execute(PrintHtmlCommand command)
    {
        var _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseBootstrap().Build();

        var result = Markdown.ToHtml(_test, _pipeline);

        //return result;
    }
}
