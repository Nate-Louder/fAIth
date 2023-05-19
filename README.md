# fAIth Interpreter for Basic Math Computation

This repository contains an interpreter written in F# that enables basic math computation, variable creation, function definition, and printing functionality. The interpreter follows a stack-based approach, where numbers (integer or double) are stored onto a stack, popped off for computation, and the resulting answer is placed back onto the stack.

## Key Features

- **Basic Math Computation:** Perform arithmetic operations such as addition, subtraction, multiplication, and division on numbers.
- **Variable Creation:** Define and assign values to variables, allowing for storing and retrieval of data during interpretation.
- **Function Definition:** Create custom functions with parameters, enabling code modularity and reuse.
- **Printing:** Output results or variables to the console for visual representation and debugging.

## How it works

1. The interpreter reads and tokenizes input code, breaking it down into individual components.
2. Numbers and variables are stored onto a stack, which acts as a temporary storage for computations.
3. Mathematical operations are performed by popping operands from the stack, calculating the result, and pushing it back onto the stack.
4. Variables are stored in a dictionary-like structure, allowing for efficient retrieval and updating.
5. Functions are defined using a syntax that supports "parameter passing" and execution.
6. The interpreter handles printing functionality, displaying results or variables to the console.

## Usage

1. Clone or download the repository.
2. Build and run the interpreter using an F# compiler or development environment.
3. Write code in an external .fth file using the provided syntax for math computation, variable creation, function definition, and printing.
4. Execute the interpreter on the code passing the file path of your fAIth file, and observe the output or printed results.

## Get Started

Explore the codebase and the provided examples to understand how the interpreter handles basic math computation, variable creation, function definition, and printing. Refer to the documentation for further details on the syntax and usage.

### Stack

Numbers are added to the front of the stack as they are read by the interpreter, and popped off the front as they are needed. Adding numbers to the stack can be done in one of three ways.

1. Numbers are written in line - The line  
 ``` 1 2 3 ```  
 results in a stack that looks something like ``` [3, 2, 1] ```.

2. Operations are performed on the stack - performing the operation ``` + ``` on a stack of ``` [3, 2, 1] ``` will result in ``` 3 and 2 ``` popping off the stack and ``` 5 ``` getting pushed to the stack. The stack will them appear like so ``` [5, 1] ``` .

3. A variable is fetched - Performing the operation ``` {variable-name} @ ``` will push the value assigned to the variable to the stack. Therefore if Variable x has a value of 2.1, calling ``` x @ ``` will result in a stack that looks something like ``` [2.1] ```.

### Operations 

There are currently _ operations that fAIth will recognize. 

#### Add
 ``` + ``` :`` Adds the first element of the stack to the second element of the stack and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [1, 2]
    command: +
    process: 1 + 2
    stack: [3]

Example 2)
    stack: []
    command: 1 2 +
    process: 2 + 1
    stack: [3]
```

#### Add'
 ``` +' ``` : Adds every element of the list together recursivley (Adds the first two elements ) and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [1, 2, 3]
    command: +'
    process: ((1 + 2) + 3)
    stack: [6]

Example 2)
    stack: []
    command: 1 2 3 +'
    process: ((3 + 2) + 1)
    stack: [6]
```


