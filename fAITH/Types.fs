namespace Faith

module Types =

    type Number =
        | Int of int
        | Double of double

    module Variables =
        type Variable = { Name: string; Value: (Number option) }

    module BasicOperations =
        open Variables

        type Operation =
            | Add
            | Add'
            | Subtract
            | Subtract'
            | Multiply
            | Multiply'
            | Divide
            | Divide'
            | PAP
            | PAP'
            | Print
            | Exit
            | VCreate of string
            | VFetch of string
            | VStore of string
            | FCreate of string * string list
            | FUse of string

        type ProcessElement =
            | Operation of Operation
            | Value of Number

    module Functions =
        open BasicOperations

        type Function =
            { Name: string
              Elements: ProcessElement list }

    module State =
        open Functions
        open Variables

        type State =
            { NumberStack: Number list
              Variables: Variable list
              Functions: Function list 
              Depth: int
            }

    module Errors =
        open BasicOperations
        open State


        type OperationError =
            | StackOverFlow
            | FailedOperationAttempt of Operation * State
            | DivideByZero
            | UnableToParseInput of string
            | VariableExistsAlready of string
            | InvalidVariableType of string
            | VariableDoesntExist of string
            | InvalidFunctionDeffinition
            | FunctionAlreadyExists of string
            | FunctionDoesntExist of string
            | UnassignedVariableError of string
