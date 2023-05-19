namespace MathInterpreter

open System
open System.IO

module Helpers = 

    open Parse
    open Types.BasicOperations
    open Types.Errors
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

    let splitLine (line: string) =
        line.Split(" ")
        |> Array.toList
    let processFile (filePath: string) =
        let lines = readFromFile filePath
        let parsedLines =
            lines
            |> Array.toList
            |> List.fold ( fun list line -> List.append list (splitLine line)) [] 
            |> parseOperation

        parsedLines
        |> List.fold operationValidationCheck (Ok [])


module main =
    open Types.BasicOperations
    open Types.Stack
    open Types.Errors
    open Operation
    open Parse
    open Helpers


    [<EntryPoint>]
    let main args =

        let mutable state = Ok { NumberStack = []; Variables = []; Functions = [] }

        match args with
        [|filepath|] ->
            match processFile filepath with
            | Ok validInput ->
                match List.fold matchElementType state validInput with
                | Ok x -> 
                    state <- Ok x
                    0
                | Error err -> 
                    printfn "%A" err
                    state <- state
                    1
            | Error e -> 
                printfn "%O" e
                1
        | _ -> 
            printfn "Invalid number of arguments. Please provide the file path."
            1

