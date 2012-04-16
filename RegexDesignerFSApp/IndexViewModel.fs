namespace RegexDesigner.App.Models

open System.ComponentModel.DataAnnotations

type IndexViewModel(input:string, pattern:string, fragments:seq<Fragment>) =

    [<Required>]
    member x.Input with get() = input

    [<Required>]
    member x.Pattern with get() = pattern

    member x.Fragments with get() = fragments
