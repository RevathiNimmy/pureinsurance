Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports RulesEngine.EngineCommon

Public NotInheritable Class GetProperty 
    Implements IRuleHelper

    Public Function ProcessLine(ByVal ParamArray args() As Object) As EngineCommon.ScriptType Implements EngineCommon.IRuleHelper.ProcessLine
        Dim output As ScriptType

        Dim arguments As ScriptType()
        Dim gisPolicyLink As Integer
        Dim fullPropertyName As String
        Dim systemArea As String
        Dim areaProperty As String
        Try
            arguments = args(0)
            gisPolicyLink = CType(arguments(0).Value, Integer)
            fullPropertyName = arguments(1).Value.ToString()

            Dim split As String() = fullPropertyName.Split(Char.Parse("/"))
            systemArea = split(0)
            areaProperty = split(1)

            Dim procedureName As String
            Select Case systemArea.ToUpper()
                Case "PARTY"
                    procedureName = "spu_DRE_party"
                Case "RISK"
                    procedureName = "spu_DRE_risk"
                Case "POLICY"
                    procedureName = "spu_DRE_policy"
                Case Else
                    Throw New ApplicationException("Invalid System Area")
            End Select

            Dim x As New DataSetType("s4i", procedureName)
            x.Parameters.Add("@gisPolicyLink", gisPolicyLink)
            x.Execute()

            output = x.Value(areaProperty)
        Catch ex As Exception
            output = New ScriptType(ex.Message)
        End Try

        Return output
    End Function
End Class
