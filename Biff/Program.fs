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

    let rec iterateCapture x rate iteration resolution func = 
        let filterSimilar resolution fx data =
            let epsilon = (resolution / 2.0)
            let minX = fx - epsilon
            let maxX = fx + epsilon
            data |> List.filter(fun fx -> fx < minX || fx > maxX)
        if iteration > 0 then
            let fx = func x rate
            let result = iterateCapture fx rate (iteration - 1) resolution func
            let filteredResult = result |> filterSimilar resolution fx
            fx :: filteredResult
        else
            []
    let calcBiff x rate iterations samples resolution f_biff = 
        iterateCapture (iterate x rate iterations f_biff) rate samples resolution f_biff 
        |> List.map (fun fx -> (rate, fx)) 

    let minX = 2.9;
    let maxX = 4.0;
    let minY = 0.3;
    let maxY = 1.0;
    let x = 0.5
    let iterations = 1000
    let samples = 500
    let resolution = 0.005

    let createTimer interval =
        let timer = new System.Windows.Forms.Timer(Interval = int(interval * 1000.0), Enabled = true)
        timer.Start()
        timer.Tick

    let events = createTimer 0.05

    let eventStream = 
        events 
        |> Observable.scan(fun rate _ -> rate + resolution) minX 
        |> Observable.map(fun rate -> calcBiff x rate iterations samples resolution f_biff)
        |> Observable.scan(fun data source -> data |> List.append source) List.Empty

    let chart = LiveChart.FastPoint(eventStream, Name = "Biff").WithXAxis(Enabled = false, Min = minX, Max = maxX).WithYAxis(Enabled = false, Min = minY, Max = maxY)
    let form = chart.ShowChart()
    form.Width <- 1000
    form.Height <- 1000

    System.Windows.Forms.Application.Run(form)
    0 // return an integer exit code
