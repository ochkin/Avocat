module TwitterConnect

type Config = FSharp.Configuration.AppSettings<"app.config">
open FSharp.Data.Toolbox.Twitter
let twitter = Twitter.AuthenticateAppOnly(Config.ConsumerKey, Config.ConsumerSecret)

let Test () =
    let query = "fintech"
    for status in twitter.Search.Tweets(query, count=10, lang="en").Statuses do
        printfn "@%s: %s" status.User.ScreenName status.Text

let Load sinceId maxId =
    let query = "fintech"
    let pageSize = 500 // max = 800
    let tweets =
        match sinceId, maxId with
            | None, None -> twitter.Search.Tweets(query, count=pageSize, lang="en")
            | None, Some mId -> twitter.Search.Tweets(query, count=pageSize, lang="en", maxId=mId)
            | Some sId, None -> twitter.Search.Tweets(query, count=pageSize, lang="en", sinceId=sId)
            | Some sId, Some mId-> twitter.Search.Tweets(query, count=pageSize, lang="en", sinceId=sId, maxId=mId)
    tweets.Statuses

let Run () =
    let minId = Repo.GetMinId ()
    printfn "minId = %A" minId
    let maxId = Repo.GetMaxId ()
    printfn "maxId = %A" maxId
    let newData =
        match maxId with
            | None -> Load minId None
            | Some id -> Load minId (Some (id-1L))
    printfn "Loaded %i new items" <| Array.length newData
    Repo.Add newData