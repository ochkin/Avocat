module Database

Raven.Database.Server.NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);

let initRavenDb () =
    let documentStore = new Raven.Client.Embedded.EmbeddableDocumentStore(DataDirectory="..\..\Data", UseEmbeddedHttpServer=true)
    documentStore.Initialize ()

type MyTweet =
    { mutable Id: int64; Lang:string; Source:string; Text:string; UserName:string }
        static member fromJson (x: TwitterConnect.Tweets.Status) = { Id=x.Id; Lang=x.Lang; Source=x.Source; Text=x.Text; UserName=x.User.Name }

[<Literal>]
let MYTWEETS_ID = "MyTweets/Id"

open Raven.Client
open Raven.Abstractions.Indexing
let putTweetIdIndex (db:IDocumentStore) =
    let index = db.DatabaseCommands.GetIndex(MYTWEETS_ID)
    if null = index then
        db.DatabaseCommands.PutIndex(
            MYTWEETS_ID,
            IndexDefinition(Map="from tweet in docs.MyTweets select new { tweet.Id }"),
            false) |> Some
    else
        None

let ConfigureIndexes () =
    use db = initRavenDb()
    putTweetIdIndex(db) |> ignore
