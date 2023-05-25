namespace Console

open System.IO

module Helpers = 

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
            
module main =
    open Faith.Faith
    open Helpers


    [<EntryPoint>]
    let main args =

        match args with
        [|filepath|] -> 
            match filepath|> processFile|> faith with
            | Ok _ -> 
                0
            | Error err -> 
                printfn "%A" err
                1   
        | _ -> 
            faithConsole
            0