module TwitterConnect

type Config = FSharp.Configuration.AppSettings<"app.config">
open FSharp.Data.Toolbox.Twitter
let twitter = Twitter.AuthenticateAppOnly(Config.ConsumerKey, Config.ConsumerSecret)

let Test () =
    let query = "fintech"
    for status in twitter.Search.Tweets(query, count=10, lang="en").Statuses do
        printfn "@%s: %s" status.User.ScreenName status.Text

let load sinceId maxId =
    let query = "fintech"
    let pageSize = 100 // max = 800
    let tweets =
        match sinceId, maxId with
            | None, None -> twitter.Search.Tweets(query, count=pageSize, lang="en")
            | None, Some mId -> twitter.Search.Tweets(query, count=pageSize, lang="en", maxId=mId)
            | Some sId, None -> twitter.Search.Tweets(query, count=pageSize, lang="en", sinceId=sId)
            | Some sId, Some mId-> twitter.Search.Tweets(query, count=pageSize, lang="en", sinceId=sId, maxId=mId)
    tweets.Statuses

type Tweets = FSharp.Data.JsonProvider<Sample="json/search_tweets.json", EmbeddedResource="FSharp.Data.Toolbox.Twitter,search_tweets.json">

type MyTweet =
    { Id: int64; Lang:string; Source:string; Text:string; UserName:string }
        static member fromJson (x: Tweets.Status) = { Id=x.Id; Lang=x.Lang; Source=x.Source; Text=x.Text; UserName=x.User.Name }

let Run () =
    let minId = Repo.GetMinId ()
    printf "minId = %A; " minId
    let maxId = Repo.GetMaxId ()
    printfn "maxId = %A" maxId
    let newData : Tweets.Status [] =
        match maxId with
            | None -> load minId None
            | Some id -> load minId (Some (id-1L))
    printfn "Loaded %i new items" <| Array.length newData
    printf "minId = %A; " <| (Seq.map (fun (s:Tweets.Status) -> s.Id) newData |> Seq.min)
    printfn "maxId = %A" <| (Seq.map (fun (s:Tweets.Status) -> s.Id) newData |> Seq.max)
    Repo.Add newData