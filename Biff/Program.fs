// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open FSharp.Charting
open System
open System.Drawing

[<EntryPoint>]
let main argv = 
    let f_sine x = sin(x / 100.0)

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
    let iterations = 2
    let samples = 20
    let distributey d = List.map (fun b -> (fst d, b)) (snd d) 
    let data = [for rate in 3.0 .. 0.01 .. 4.0 -> rate, (iterateCapture (iterate x rate iterations f_biff) rate samples f_biff)]
    let datapairs = List.concat (List.map (fun a -> distributey a) data)
    let myForm = (datapairs |> Chart.Point).ShowChart()

    System.Windows.Forms.Application.Run(myForm)

    0 // return an integer exit code
