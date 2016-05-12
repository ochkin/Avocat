// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' pr
[<EntryPoint>]
let main argv =

    //TwitterConnect.Run ()  |> ignore

    use db = Repo.testRavenDb ()
    use connection = db.OpenSession()

//    for tweet in TwitterConnect.load None None |> Seq.map TwitterConnect.MyTweet.fromJson do
//        connection.Store tweet
//    connection.SaveChanges ()

    let result = connection.Query<TwitterConnect.MyTweet>() |> List.ofSeq
    
    
    printfn "Total %i statuses." <| List.length result
    printfn "%A" <| List.head result

    System.Console.ReadKey () |> ignore
    0 // return an integer exit code
