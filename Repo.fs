module Repo

//EmbeddableDocumentStore store = new EmbeddableDocumentStore
//{
//	DataDirectory = "Data"
//};
open Database

let getMinMax () =
    use db = Database.initRavenDb ()
    use connection = db.OpenSession()
    let qry = connection.Query<Database.MyTweet>(Database.MYTWEETS_ID)
    let minimum = qry |> Seq.map (fun tweet -> tweet.Id) |> Seq.min
    let maximum = qry |> Seq.map (fun tweet -> tweet.Id) |> Seq.max
    ()

let GetMinId () =
    use db = initRavenDb()
    use connection = db.OpenSession()
    Some 0L

let GetMaxId () =
    None

let Add tweets =
    Array.length tweets