module ConnectionManager
open Npgsql


let private connection=lazy(new NpgsqlConnection(""))

//получить один раз и контролить во внешнем коде
let GetConnection connectionStr=
    connection.Value

let dispose=
    connection.Value.Dispose()



