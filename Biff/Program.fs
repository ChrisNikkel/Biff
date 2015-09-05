// Copyright (c)  2015 Christopher Nikkel
open FSharp.Charting
open System
open System.Drawing
open System.Windows.Forms

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
    let calcBiff x rate iterations samples f_biff = 
        iterateCapture (iterate x rate iterations f_biff) rate samples f_biff) |> List.map (fun fx -> (rate, fx))

    let x = 0.5
    let iterations = 2000
    let samples = 100

    let data = 
        seq {2.5 .. 0.001 .. 4.0 } 
        |> Seq.map (fun rate -> calcBiff x rate iterations samples f_biff)
        |> List.concat

    let createTimer interval =
        let timer = new System.Windows.Forms.Timer(Interval = int(interval * 1000.0), Enabled = true)
        timer.Start()
        timer.Tick

    //let events = createTimer 0.1
    //let eventStream = events |> Observable.map (fun data _ -> doIteration data) initialData
    //let chart = LiveChart.FastPoint(eventStream, Name = "Biff").WithXAxis(Enabled = false).WithYAxis(Enabled = false)
    let chart = Chart.FastPoint(data, Name = "Biff").WithXAxis(Enabled = false).WithYAxis(Enabled = false)
    let form = chart.ShowChart()
    form.Width <- 1000
    form.Height <- 1000

    System.Windows.Forms.Application.Run(form)
    0 // return an integer exit code
