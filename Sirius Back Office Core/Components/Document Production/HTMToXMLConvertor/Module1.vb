Imports System.IO
Imports System.Reflection

Module Module1

    Public Function CreateLateBoundObject(ByVal ClassName As String) As Object
        Dim sPurePath As String

        Try
            sPurePath = New Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath
            sPurePath = Path.GetDirectoryName(sPurePath)

            Dim libraryPath As String
            Dim DLLAssembly As [Assembly]

            libraryPath = Path.Combine(sPurePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".dll")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName)
            End If

            libraryPath = Path.Combine(sPurePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".exe")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName)
            End If

            Throw New IO.FileNotFoundException("Cannot find " & libraryPath & " in " & sPurePath & " as exe or dll")
        Catch excep As Exception
            'LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateLateBoundObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateLateBoundObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
        Return Nothing
    End Function
End Module
