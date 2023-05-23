namespace Faith

open System
open Types
open Types.BasicOperations
open Types.Variables
open Types.Errors
open Types.State
open System.Text.RegularExpressions

module NumberOperations =
    let add (number1: Number) (number2: Number) : Number =
        match number1, number2 with
        | Int(Some x), Int(Some y) -> Int <| Some(x + y)
        | Double(Some x), Double(Some y) -> Double <| Some(x + y)
        | Int(Some x), Double(Some y) -> Double <| Some(double (x) + y)
        | Double(Some x), Int(Some y) -> Double <| Some(x + double (y))
        | _ -> Double None

    let subtract (number1: Number) (number2: Number) : Number =
        match number1, number2 with
        | Int(Some x), Int(Some y) -> Int <| Some(x - y)
        | Double(Some x), Double(Some y) -> Double <| Some(x - y)
        | Int(Some x), Double(Some y) -> Double <| Some(double (x) - y)
        | Double(Some x), Int(Some y) -> Double <| Some(x - double (y))
        | _ -> Double None

    let multiply (number1: Number) (number2: Number) : Number =
        match number1, number2 with
        | Int(Some x), Int(Some y) -> Int <| Some(x * y)
        | Double(Some x), Double(Some y) -> Double <| Some(x * y)
        | Int(Some x), Double(Some y) -> Double <| Some(double (x) * y)
        | Double(Some x), Int(Some y) -> Double <| Some(x * double (y))
        | _ -> Double None

    let divide (number1: Number) (number2: Number) =
        match number1, number2 with
        | Int(Some x), Int(Some y) -> Int <| Some(x / y)
        | Double(Some x), Double(Some y) -> Double <| Some(x / y)
        | Int(Some x), Double(Some y) -> Double <| Some(double (x) / y)
        | Double(Some x), Int(Some y) -> Double <| Some(x / double (y))
        | _ -> Double None

    let printfn number =
        match number with
        | Int(Some x) -> printfn "%i" x
        | Double(Some x) -> printfn "%f" x
        | _ -> ()

module StackOperations =
    let head list = List.head list
    let rest list = List.tail list
    let push stack value = (value :: stack)

    let add stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Add, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Add, stack)
        | x :: (y :: ys) ->
            Ok
                { stack with
                    NumberStack = (NumberOperations.add x y :: ys) }

    let add' stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Add', stack)
        | x :: [] -> Error <| FailedOperationAttempt(Add', stack)
        | x :: xs ->
            Ok
                { stack with
                    NumberStack = ([ List.fold (fun sum element -> NumberOperations.add sum element) x xs ]) }

    let subtract stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Subtract, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Subtract, stack)
        | x :: (y :: ys) ->
            Ok
                { stack with
                    NumberStack = (NumberOperations.subtract x y :: ys) }

    let subtract' stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Subtract', stack)
        | x :: [] -> Error <| FailedOperationAttempt(Subtract', stack)
        | x :: xs ->
            Ok
                { stack with
                    NumberStack =
                        ([ List.fold (fun inverseSum element -> NumberOperations.subtract inverseSum element) x xs ]) }

    let multiply stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Multiply, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Multiply, stack)
        | x :: (y :: ys) ->
            Ok
                { stack with
                    NumberStack = ((NumberOperations.multiply x y) :: ys) }

    let multiply' stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Multiply', stack)
        | x :: [] -> Error <| FailedOperationAttempt(Multiply', stack)
        | x :: xs ->
            Ok
                { stack with
                    NumberStack =
                        ([ List.fold (fun product element -> NumberOperations.multiply product element) x xs ]) }

    let divide stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Divide, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Divide, stack)
        | x :: (y :: ys) ->
            match y with
            | Int(Some 0) -> Error <| DivideByZero
            | Double(Some 0.) -> Error <| DivideByZero
            | _ ->
                Ok
                    { stack with
                        NumberStack = (NumberOperations.divide x y :: ys) }

    let divide' stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Divide, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Divide, stack)
        | x :: xs ->
            match
                List.fold
                    (fun quotient element ->
                        match quotient, element with
                        | Error err, _ -> Error err
                        | Ok _, Int(Some 0) -> Error <| DivideByZero
                        | Ok _, Double(Some 0.) -> Error <| DivideByZero
                        | Ok q, e -> Ok(NumberOperations.divide q e))
                    (Ok x)
                    xs
            with
            | Ok x -> Ok { stack with NumberStack = [ x ] }
            | Error err -> Error err

    let popAndPrint stack =
        match stack.NumberStack with
        | [] ->
            printfn "Nothing to pop."
            Error <| FailedOperationAttempt(PAP, stack)
        | x :: xs ->
            NumberOperations.printfn x
            Ok { stack with NumberStack = xs }

    let popAndPrint' stack : Result<State, _> =
        match stack.NumberStack with
        | [] ->
            printfn "Nothing to pop."
            Error <| FailedOperationAttempt(PAP', stack)
        | _ ->
            List.map (fun x -> NumberOperations.printfn x) stack.NumberStack |> ignore
            Ok { stack with NumberStack = [] }

    let printStack stack =
        printfn "%A" stack.NumberStack
        Ok stack

module VariableOperations =

    let createVariable name state =
        match List.tryFind (fun (variable: Variable) -> name = variable.Name) state.Variables with
        | Some x -> Error <| VariableExistsAlready x.Name
        | None -> Ok { state with Variables = List.append state.Variables [ { Name = name; Value = Int(None) } ] }

    let storeVariable name state =
        let { NumberStack = numbers; Variables = variables } = state
        match List.tryFind (fun (variable: Variable) -> name = variable.Name) variables with
        | Some matchedVariable ->
            match numbers with
            | Int x :: _ ->
                Ok <| { state with NumberStack = List.tail numbers; Variables = List.append (List.except [ matchedVariable ] variables) [ { Name = name; Value = Int x } ]}
            | Double x :: _ ->
                Ok <| {state with NumberStack = List.tail numbers; Variables = List.append (List.except [ matchedVariable ] variables) [ { Name = name; Value = Double x } ]}
            | _ ->
                Error <| FailedOperationAttempt( VStore name, { state with NumberStack = numbers; Variables = variables })
        | _ -> Error <| VariableDoesntExist name

    let fetchVariable name state  =
        let { NumberStack = numbers; Variables = variables } = state
        match List.tryFind (fun (variable: Variable) -> name = variable.Name) variables with
        | Some matchedVariable -> 
            Ok <| { state with NumberStack = matchedVariable.Value :: numbers; Variables = variables }
        | _ -> Error <| VariableDoesntExist name

module Parse =
    open Types.Errors
    open Types.Variables

    let operationValidationCheck state element =
            match state with
            | Ok xs ->
                match element with
                | Ok x -> Ok(List.append xs [ x ])
                | Error e -> Error e
            | Error e -> Error e

    let parseNumber (input: string) =
        match Int32.TryParse input with
        | (true, number) -> Ok <| Value(Int <| Some number)
        | (false, _) ->
            match Double.TryParse input with
            | (true, number) -> Ok <| Value(Double <| Some number)
            | (false, _) -> Error <| UnableToParseInput input

    let rec functionOperationsStr lst =
        match lst with
        | [] -> ([], [])
        | "END" :: xs -> ([], xs)
        | x :: xs -> 
            let (before, after) = functionOperationsStr xs
            (x :: before, after)

    let rec parseOperation input =
        match input with
        | x :: y ->
            match x with
            | "+" -> List.append [ Ok <| Operation Add ] (parseOperation y)
            | "+'" -> List.append [ Ok <| Operation Add' ] (parseOperation y)
            | "-" -> List.append [ Ok <| Operation Subtract ] (parseOperation y)
            | "-'" -> List.append [ Ok <| Operation Subtract' ] (parseOperation y)
            | "*" -> List.append [ Ok <| Operation Multiply ] (parseOperation y)
            | "*'" -> List.append [ Ok <| Operation Multiply' ] (parseOperation y)
            | "/" -> List.append [ Ok <| Operation Divide ] (parseOperation y)
            | "/'" -> List.append [ Ok <| Operation Divide' ] (parseOperation y)
            | "." -> List.append [ Ok <| Operation PAP ] (parseOperation y)
            | ".'" -> List.append [ Ok <| Operation PAP' ] (parseOperation y)
            | "PRINT" -> List.append [ Ok <| Operation Print ] (parseOperation y)
            | "EXIT" -> List.append [ Ok <| Operation Exit ] (parseOperation y)
            | "VARIABLE" -> List.append [ Ok <| Operation(VCreate(List.head y)) ] (parseOperation (List.tail y))
            | "FUN" ->
                match y with
                | y :: ys -> 
                    let name = y
                    match functionOperationsStr ys with
                    | ([], _) -> List.append [ Error <| InvalidFunctionDeffinition ] []
                    | (operations, rest) -> List.append [ Ok <| Operation(FCreate(name, operations)) ] (parseOperation rest)
                | [] -> List.append [ Error <| InvalidFunctionDeffinition ] []
            | _ ->
                match y with
                | "@" :: _ -> List.append [ Ok <| Operation(VFetch x) ] (parseOperation (List.tail y))
                | "!" :: _ -> List.append [ Ok <| Operation(VStore x) ] (parseOperation (List.tail y))
                | _ -> 
                    match parseNumber x with
                    | Error _-> List.append [ Ok <| Operation(FUse x) ] (parseOperation y)
                    | _ -> List.append [parseNumber x] (parseOperation y)
        | _ -> []

module Operation =
    open StackOperations
    open VariableOperations
    open Functions
    open Parse

    let rec tryPerformOperation state operation =
        match operation with
        | Add -> add state
        | Add' -> add' state
        | Subtract -> subtract state
        | Subtract' -> subtract' state
        | Multiply -> multiply state
        | Multiply' -> multiply' state
        | Divide -> divide state
        | Divide' -> divide' state
        | PAP -> popAndPrint state
        | PAP' -> popAndPrint' state
        | Print -> printStack state
        | Exit -> exit 0
        | VCreate name -> createVariable name state
        | VStore name -> storeVariable name state
        | VFetch name -> fetchVariable name state
        | FCreate (name, elements) -> createFunction name elements state
        | FUse name -> useFunction name state

    and useFunction name state =
        match List.tryFind (fun func -> func.Name = name) state.Functions with  
        | Some func -> 
            match List.fold (fun state element -> matchElementType state element) (Ok state) func.Elements  with
            | Ok x -> Ok x
            | Error(FailedOperationAttempt(y, x)) ->
                printfn "Operation %O failed with parameters %A" y x
                Ok state
            | Error DivideByZero -> 
                printfn "Cannot divide by zero"
                Ok state
            | _ -> Ok state
            
        | None -> Error <| FunctionDoesntExist name

    and createFunction name elementsStr state =
        match elementsStr
            |> parseOperation
            |> List.fold operationValidationCheck (Ok[]) with
        | Ok elements -> 
            match List.tryFind (fun func -> func.Name = name) state.Functions with
            | None -> Ok <| { state with Functions = List.append [{ Name = name; Elements = elements}] state.Functions} 
            | Some _ -> Error <| FunctionAlreadyExists name
        | Error e -> Error <| InvalidFunctionDeffinition
    
    and matchElementType state processElement =
        match state with
        | Ok s ->
            match processElement with
            | Operation o -> tryPerformOperation s o
            | Value v -> Ok { s with NumberStack = (v :: s.NumberStack) }
        | Error e -> Error e
    