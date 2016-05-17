
let Run () =
    let minId = Repo.GetMinId ()
    printf "minId = %A; " minId
    let maxId = Repo.GetMaxId ()
    printfn "maxId = %A" maxId
    let newData : TwitterConnect.Tweets.Status [] =
        match maxId with
            | None -> TwitterConnect.load minId None
            | Some id -> TwitterConnect.load minId (Some (id-1L))
    printfn "Loaded %i new items" <| Array.length newData
    printf "minId = %A; " <| (Seq.map (fun (s:TwitterConnect.Tweets.Status) -> s.Id) newData |> Seq.min)
    printfn "maxId = %A" <| (Seq.map (fun (s:TwitterConnect.Tweets.Status) -> s.Id) newData |> Seq.max)
    Repo.Add newData

[<EntryPoint>]
let main argv =
    Database.ConfigureIndexes()


    use db = Database.initRavenDb ()
    use connection = db.OpenSession()
//    for tweet in TwitterConnect.load None None |> Seq.map TwitterConnect.MyTweet.fromJson do
//        connection.Store tweet
//    connection.SaveChanges ()

//    // index creation
//    let index = Database.putTweetIdIndex db
//    printfn "%s" index


    //let result = connection.Query<Database.MyTweet>() |> List.ofSeq    
//    printfn "Total %i statuses." <| List.length result
//    printfn "%A" <| List.head result

    printfn "Min = %i; max=%i" minimum maximum

    System.Console.ReadKey () |> ignore
    0 // return an integer exit code
