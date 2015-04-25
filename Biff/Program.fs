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

    let f_biff x = 2.4 * (1.0 - 2.0 * x)

    let rec iterate x i = 
        if i > 1 then
            let y = f_biff x
            let result = iterate y (i - 1)
            result
        else
            f_biff x

    printfn "%A" (iterate 2.1 2)

    let rec iterateCapture x i = 
        if i > 1 then
            let y = f_biff x
            let result = iterateCapture y (i - 1)
            y :: result
        else
            []

    printfn "%A" (iterateCapture (iterate 2.1 2) 10)

    let data = seq { for i in 1..1000 -> f_sine (float i) }

    let form = new LineChartForm("Sine", data)
    printfn "%A" data2
    System.Windows.Forms.Application.Run(form)
    0 // return an integer exit code
