Imports Microsoft.VisualBasic.CompilerServices
Public NotInheritable Class Interaction
    Public Shared Function CallByName(ByVal ObjectRef As Object, ByVal ProcName As String, ByVal UseCallType As CallType, ParamArray Args As Object()) As Object
        Dim copyArray As Boolean()
        Select Case UseCallType
            Case CallType.Method
                LateBinding.LateCall(ObjectRef, Nothing, ProcName, Args, Nothing, copyArray)
            Case CallType.[Get]
                Return LateBinding.LateGet(ObjectRef, Nothing, ProcName, Args, Nothing, Nothing)
            Case CallType.[Let], CallType.[Set]
                Dim objType As Type = Nothing
                LateBinding.LateSetComplex(ObjectRef, objType, ProcName, Args, Nothing, OptimisticSet:=False, UseCallType)
                Return Nothing
            Case Else
                Throw New ArgumentException("Argument_InvalidValue1", "CallType")
        End Select
    End Function
End Class

