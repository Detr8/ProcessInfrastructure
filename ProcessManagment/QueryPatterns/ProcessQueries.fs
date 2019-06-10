module ProcessQueries

type ProcessQueries=
    |GetById
    

let patterns queryType=
    match queryType with
    |GetById _-> ""
