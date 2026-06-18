Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports RulesEngine.RuleLineHandlers
Imports RulesEngine.EngineCommon
Imports RulesEngine.EngineSupport.Utilities

Public NotInheritable Class PureInsAddEntityLineHandler 
    Inherits LineHandler

    Public Overrides Function AssemblyCode(ByVal line As EngineCommon.IRuleLine, ByVal declarations As EngineCommon.RuleEngineDeclarationCollection) As String
        If line Is Nothing Then
            Throw New ArgumentNullException("record")
        End If
        If declarations Is Nothing Then
            Throw New ArgumentNullException("declarations")
        End If
        If (String.Compare(line.Task, MyBase.TaskName, True, LineHandler.Culture) = 0) Then
            Return HandleAddEntityFunction(line, declarations)
        Else
            Return MyBase.NextHandler(line.Task).AssemblyCode(line, declarations)
        End If
    End Function

    Private Function HandleAddEntityFunction(ByVal line As IRuleLine, ByVal declarations As RuleEngineDeclarationCollection) As String
        Dim entity As String = ""

        Dim code As New Text.StringBuilder()

        entity = line.Var1

        'Try catch block for variable declarations
        code.AppendLine("try {")

        'Generate the correct c# code for the method
        code.AppendLine(String.Format("AddEntity({0});", entity))

        'Get and increment NextOIKey
        code.AppendLine("int nextOI = int.Parse(input.NextOINumber) + 1;")
        code.AppendLine("input.NextOINumber = nextOI.ToString();")

        'Get Index
        code.AppendLine(String.Format("int outCount = {0}.Count;", entity))

        'Set Properties on new entity
        code.AppendLine(String.Format("{0}[outCount -1].OI = ""OI"" + nextOI.ToString();", entity))
        code.AppendLine(String.Format("{0}[outCount -1].US = 1;", entity))

        code.AppendLine("} catch(Exception ex) {AddNotEvaluatedWarning(ex);}")

        Return code.ToString()
    End Function

End Class
