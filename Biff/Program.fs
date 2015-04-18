// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

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
    let f_biff x r = r * (1.0 - 2.0 * x) 
    let f_sine x = sin(x / 100.0)
    let data = seq { for i in 1..1000 do yield f_sine (float i) }
    let form = new LineChartForm("Sine", data)
    System.Windows.Forms.Application.Run(form)

    printfn "%A" argv
    0 // return an integer exit code
