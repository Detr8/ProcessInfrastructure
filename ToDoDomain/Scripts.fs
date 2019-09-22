namespace ToDoDomain

open System.Data
open PgSqlDapper.DAL

module Scripts=

    open ToDoTypes

    let getConnection()=
        Database.GetNewConnection("")

    let private saveToDoItem insertQuery updateQuery (queryExecuter:string->bool) (item:ToDoItem)=
        ()
    
    let SaveToDoItem (getConnection:unit->IDbConnection)=
        let sql=""
        Database.ExecuteRW<ToDoItem> sql (getConnection ())

//let SaveToDoItem (item:ToDoItem)=
//    //get insert q
//    let insertQ=""
    
//    //get update q
//    let updateQ=""

//    saveToDoItem insertQ updateQ item

