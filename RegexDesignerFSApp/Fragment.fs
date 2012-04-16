namespace RegexDesigner.App.Models

type Fragment(matchType:MatchType, content:string) =
    member x.MatchType with get() = matchType
    member x.Content with get() = content
