
let Run () =
    let minId = Repo.GetMinId ()
    printf "minId = %A; " minId
    let maxId = Repo.GetMaxId ()
    printfn "maxId = %A" maxId
    let pageSize = 100
    let query = "fintech"
    let newData : TwitterConnect.Tweets.Status [] =
        match maxId with
            | None -> TwitterConnect.load None None pageSize query
            | Some id -> TwitterConnect.load None (Some (id-1L)) pageSize query
    printfn "Loaded %i new items" <| Array.length newData
    printf "minId = %A; " <| (Seq.map (fun (s:TwitterConnect.Tweets.Status) -> s.Id) newData |> Seq.min)
    printfn "maxId = %A" <| (Seq.map (fun (s:TwitterConnect.Tweets.Status) -> s.Id) newData |> Seq.max)
    Repo.Add <| Array.map Database.MyTweet.fromJson newData

[<EntryPoint>]
let main argv =
    use db = Database.Store
    Database.ConfigureIndexes db
    let minus1 = function
        | Some x -> Some (x-1L)
        | None -> None
    let rec run () =
        let mi = Repo.GetMinId()
        //let ma = Repo.GetMaxId()
        let newData = Array.map Database.MyTweet.fromJson <| TwitterConnect.load None (minus1 mi) 100 "fintech"
        if Repo.Add newData then
            run ()
    run ()
//    use connection = db.OpenSession()
//    for tweet in TwitterConnect.load None None |> Seq.map TwitterConnect.MyTweet.fromJson do
//        connection.Store tweet
//    connection.SaveChanges ()
//    // index creation
//    let index = Database.putTweetIdIndex db
//    printfn "%s" index

    //let result = connection.Query<Database.MyTweet>() |> List.ofSeq    
//    printfn "Total %i statuses." <| List.length result
//    printfn "%A" <| List.head result
    //printfn "Min = %A; max=%A" minimum maximum
    printfn " Press any key ... "
    System.Console.ReadKey () |> ignore
    0 // return an integer exit code
