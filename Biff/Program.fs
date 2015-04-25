// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System
open System.Windows.Forms
open System.Windows.Forms.DataVisualization.Charting

type LineChartForm(title, xs : float seq) =
    inherit Form(Text = title)
    
    let chart = new Chart(Dock = DockStyle.Fill)
    let area = new ChartArea(Name = "Area1")
    let series = new Series()
    do series.Color <- System.Drawing.Color.Black
    do series.ChartType <- SeriesChartType.Line
    do xs |> Seq.iter(series.Points.Add >> ignore)
    do series.ChartArea <- "Area1"
    do chart.Series.Add(series)
    do chart.ChartAreas.Add(area)
    do base.Controls.Add(chart)


[<EntryPoint>]
let main argv = 
    let f_sine x = sin(x / 100.0)

    let f_biff x c = c * (1.0 - 2.0 * x)

    let rec iterate x c i f = 
        if i > 1 then
            let y = f x c
            let result = iterate y c (i - 1) f
            result
        else
            f x c

    printfn "%A" (iterate 2.1 2.0 2 f_biff)

    let rec iterateCapture x c i f = 
        if i > 0 then
            let y = f x c
            let result = iterateCapture y c (i - 1) f
            y :: result
        else
            []

    printfn "%A" (iterateCapture (iterate 2.1 2.0 2 f_biff) 2.0 10 f_biff)

    let data = seq { for i in 1..1000 -> f_sine (float i) }

    let form = new LineChartForm("Sine", data)
    
    System.Windows.Forms.Application.Run(form)
    0 // return an integer exit code
