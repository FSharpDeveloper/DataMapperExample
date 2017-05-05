namespace FSharpEmployee

open System
open System.Windows.Forms

module Program = 
    [<EntryPoint>]
    [<STAThread>]
    let main argv = 
        Application.EnableVisualStyles() |> ignore
        Application.SetCompatibleTextRenderingDefault true |> ignore

        use form = new EmployeeForm()
        Application.Run form |> ignore
        0 // return an integer exit code
