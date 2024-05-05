using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Application.Threading;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Feature.Services.Refactorings.Specific.Rename;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.ReSharper.Feature.Services.Code;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Refactorings;
using JetBrains.ReSharper.Refactorings.VB;
using JetBrains.ReSharper.Resources.Shell;

namespace ReSharperPlugin.MyPlugin.Highlighter;

[QuickFix]
public class CorrectBadWordQuickFix : QuickFixBase
{
    private readonly IComment _comment;
    private readonly BadWordNamingWarning _warning;
    public CorrectBadWordQuickFix([NotNull] BadWordNamingWarning warning)
    {
        _comment = warning.Comment;
        _warning = warning;
    }


    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
        
        var factory = CSharpElementFactory.GetInstance(_comment.GetPsiModule());
        var oldComment = _comment;
        string newCommentText = StringUtils.CleanUpBadWords(oldComment.CommentText);
        if (!newCommentText.Contains("//"))
        {
            newCommentText = "//" + newCommentText;
        }
        var newComment = factory.CreateComment(newCommentText);
        ModificationUtil.ReplaceChild(oldComment, newComment);
        return null;
    }

    public override string Text => "Replace the bad word";

    public override bool IsAvailable(IUserDataHolder cache)
    {
        return true;
    }
}