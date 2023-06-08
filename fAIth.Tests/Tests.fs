module Tests =

    open Faith.Faith
    open Faith.Types.State
    open Console.Helpers
    open System.IO
    open Faith.Types
    open Xunit


    let blankState = {NumberStack=[]; Variables=[]; Functions=[]; Depth = 0}

    let fillNumberStack stack = 
        Ok {blankState with NumberStack=stack}

    [<Fact>]
    let ``My test`` () =

        let expected = fillNumberStack [Int 3]
        let actual = faith "7 3 4 5 6 + - * /
        ."
        Assert.Equal(expected, actual)
 
 