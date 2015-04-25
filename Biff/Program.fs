// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open FSharp.Charting
open System
open System.Drawing

[<EntryPoint>]
let main argv = 
    let f_sine x = sin(x / 100.0)

    let f_biff x rate = rate * (1.0 - 2.0 * x)

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

    let rate = 3.83
    let distributey d = List.map (fun b -> (fst d, b)) (snd d) 
    let data = [for x in 1.0 .. 1.0 .. 10.0 -> x, (iterateCapture (iterate x rate 2 f_biff) rate 20 f_biff)]
    let datapairs = List.concat (List.map (fun a -> distributey a) data)
    let myForm = (datapairs |> Chart.Point).ShowChart()

    System.Windows.Forms.Application.Run(myForm)

    0 // return an integer exit code
