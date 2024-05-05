using System.Linq;
using System.Runtime.Remoting.Contexts;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.VB.RearrangeCode;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperPlugin.MyPlugin.Highlighter;

[ElementProblemAnalyzer(typeof(IComment), HighlightingTypes =
    new[] {typeof(BadWordNamingWarning)})]
public class BadWordNamingAnalyzer : ElementProblemAnalyzer<IComment>
{
    protected override void Run(IComment element, ElementProblemAnalyzerData data,
        IHighlightingConsumer consumer)
    {
        var nodeText = element.CommentText.ToLower();
        if (BadWordRepository.Words==null)
            return;
        //This logic only flags comments where bad words are found on their own (will flag "hell", won't flag "hello")
        //I found this more convenient for testing but flagging all words would be a trivial change
        if (!BadWordRepository.Words.Keys.Any(k=> nodeText.Split(' ').Any(word => word.Equals(k))))
            return;
        BadWordNamingWarning warning = new BadWordNamingWarning(element, element.GetDocumentRange());
        CorrectBadWordQuickFix fix = new CorrectBadWordQuickFix(warning);
        consumer.AddHighlighting(warning);
    }
}