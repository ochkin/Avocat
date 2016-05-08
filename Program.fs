// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' pr
[<EntryPoint>]
let main argv =

    TwitterConnect.Run ()  |> ignore
    System.Console.ReadKey () |> ignore
    0 // return an integer exit code
