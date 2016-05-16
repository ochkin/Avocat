module Database

Raven.Database.Server.NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);

let initRavenDb () =
    let documentStore = new Raven.Client.Embedded.EmbeddableDocumentStore(DataDirectory="..\..\Data", UseEmbeddedHttpServer=true)
    documentStore.Initialize ()

type MyTweet =
    { mutable Id: int64; Lang:string; Source:string; Text:string; UserName:string }
        /// TODO: move outise of this class
        static member fromJson (x: TwitterConnect.Tweets.Status) = { Id=x.Id; Lang=x.Lang; Source=x.Source; Text=x.Text; UserName=x.User.Name }

open Raven.Client.Indexes
//type TweetById () as this =
//    inherit AbstractIndexCreationTask<MyTweet>()
//    do
//        this.Map <- Seq.map (fun (tweet:MyTweet) -> tweet.Id)

//type testClass()=
//    do
//        let builder = IndexDefinitionBuilder<MyTweet>()
//        builder.Map <- orders => from order in orders
//						        select new
//						        {
//							        order.Employee,
//							        order.Company,
//							        Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
//						        };

[<Literal>]
let MYTWEETS_ID = "MyTweets/Id"
[<Literal>]
let MYTWEETS_MIN_MAX_ID = "MyTweets/MinMaxId"

open Raven.Client
//open Raven.Database
//open Raven.Database.Indexing
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
//
//type AggResult() =
//    member val MinId = 0 with get, set
//    member val MaxId = 0 with get, set
//
////let testSeq = seq { 0 .. 10 }
////System.Linq.Enumerable.Aggregate(testSeq, System.Math.Min()
//
//let putTweetMinMaxIdIndex (db:IDocumentStore) =
//    let index = db.DatabaseCommands.GetIndex(MYTWEETS_MIN_MAX_ID)
//    if index = null then
//        db.DatabaseCommands.PutIndex(
//            MYTWEETS_MIN_MAX_ID,
//            IndexDefinition(
//                Map="from tweet in docs.MyTweets select new { MinId=tweet.Id, MaxId=tweet.Id }",
//                Reduce="System.Linq.Enumerable.Aggregate(results, (a, b) => new { MinId=System.Math.Min(a.MinId,b.MinId), MaxId=System.Math.Max(a.MaxId, b.MaxId) })"),
//            false)
//    else
//        System.String.Empty

let ConfigureIndexes () =
    use db = initRavenDb()
    putTweetIdIndex(db) |> ignore
    //putTweetMinMaxIdIndex(db) |> ignore