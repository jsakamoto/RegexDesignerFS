namespace RegexDesigner.App.Controllers

open System.Web
open System.Web.Mvc
open System.Linq
open System.Text.RegularExpressions
open RegexDesigner.App.Models

type RegexDesignerController() =
    inherit Controller()

    member x.ToFragments (input:string) (pattern:string) =
        let oddEven = Seq.initInfinite (fun n -> if n % 2 = 0 then MatchType.Even else MatchType.Odd)

        let points = 
            Regex.Matches(input, pattern).Cast<Match>()
            |> Seq.zip oddEven
            |> Seq.map (fun (mt, m) -> seq [| (m.Index, mt); (m.Index + m.Length, MatchType.None); |])
            |> Seq.concat

        let seq1 = Seq.append (seq [|(0, MatchType.None)|]) points
        let seq2 = Seq.append points (seq [|(input.Length, MatchType.None)|])
        Seq.zip seq1 seq2
            |> Seq.map (fun ((i1, t1), (i2, _)) -> (i1, i2-i1, t1))
            |> Seq.map (fun (index, length, matchType) -> {MatchType=matchType; Content=input.Substring(index, length)})
            |> Seq.filter (fun f -> f.Content <> "")

    [<HttpGet>]
    member x.Index () =
        let model = new IndexViewModel("", "", Seq.empty<Fragment>);
        x.View(model) :> ActionResult

    [<HttpPost>]
    member x.Index (input:string, pattern:string) =
        match x.ModelState.IsValid with
        | true ->
            let fragments = x.ToFragments input pattern
            let model = new IndexViewModel(input, pattern, fragments)
            x.View(model) :> ActionResult
        | _ -> x.View() :> ActionResult
