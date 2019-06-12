

module ProcessRepository



type ProcessQueries=
    |GetById
    

let GetQueryPatterns queryType=
    match queryType with
    |GetById _-> """select * from processes"""



//let SaveProcess (process:Process)=
//    let sql=process.Id
    