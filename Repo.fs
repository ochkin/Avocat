module Repo

//EmbeddableDocumentStore store = new EmbeddableDocumentStore
//{
//	DataDirectory = "Data"
//};
open Database

let GetMinId () =
    use db = initRavenDb()
    use connection = db.OpenSession()
    Some 0L

let GetMaxId () =
    None

let Add tweets =
    Array.length tweets