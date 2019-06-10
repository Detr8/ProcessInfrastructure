module SqlBuilder

type QueryType=
    |ReadAll
    |ReadById
    |Insert
    |Update
    |DeleteAll
    |DeleteById

let BuildSelect <'TEntity> =
    "select"

let BuildInsert <'TEntity> =
    "insert"

let BuildUpdate <'TEntity> =
    "update"


let GetQuery<'TEntity> (queryType:QueryType) =
    match queryType with
    |ReadAll _->BuildSelect<'TEntity>