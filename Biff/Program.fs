// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open FSharp.Charting
open System
open System.Drawing
open FSharp.Collections.ParallelSeq

[<EntryPoint>]
let main argv = 

    let f_biff x rate = rate * x * (1.0 - x)

    let rec iterate x rate iteration func = 
        if iteration > 1 then
            let fx = func x rate
            let result = iterate fx rate (iteration - 1) func
            result
        else
            func x rate

    let rec iterateCapture x rate iteration func = 
        if iteration > 0 then
            let fx = func x rate
            let result = iterateCapture fx rate (iteration - 1) func
            fx :: result
        else
            []

    let x = 0.5
    let iterations = 200
    let samples = 20

    let distributey d = List.map (fun b -> (fst d, b)) (snd d) 
    let data = seq {2.5 .. 0.001 .. 4.0 } |> PSeq.map (fun rate -> (rate, (iterateCapture (iterate x rate iterations f_biff) rate samples f_biff)))
    let datapairs = List.concat (PSeq.map (fun a -> distributey a) data)

    let chart = (datapairs |> Chart.FastPoint)
    let form = chart.ShowChart()

    System.Windows.Forms.Application.Run(form)

    0 // return an integer exit code
