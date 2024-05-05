using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperPlugin.MyPlugin.Highlighter;

[StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.GutterMarks))]
public class BadWordNamingWarning : IHighlighting
{
    private readonly DocumentRange _documentRange;
    public readonly IComment Comment;

    public BadWordNamingWarning(IComment comment, DocumentRange documentRange)
    {
        Comment = comment;
        _documentRange = documentRange;
    }

    public bool IsValid()
    {
        return Comment.IsValid();
    }

    public DocumentRange CalculateRange()
    {
        return _documentRange;
    }

    public string ToolTip => "The comment contains a bad word";
    public string ErrorStripeToolTip { get; }
}