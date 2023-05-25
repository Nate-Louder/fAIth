namespace Test

module Program = 
    open Faith.Faith
    open System.IO

    let readFromFile (filePath: string) =
        try
            File.ReadAllLines filePath
        with
            | :? System.IO.FileNotFoundException ->
                printfn "File not found: %s" filePath
                [||]
            | ex -> 
                printfn "An error occurred while reading the file: %s" ex.Message
                [||]

    let processFile (filePath: string) =
           filePath
            |> readFromFile
            |> Array.toList
            |> String.concat " "

    let test filepath =     
        match filepath|> processFile|> faith with
        | Ok state -> Ok state.NumberStack
        | Error err -> Error err

    printfn "%A" <| test 


