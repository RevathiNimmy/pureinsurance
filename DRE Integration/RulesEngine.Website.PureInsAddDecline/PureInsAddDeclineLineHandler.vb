Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports RulesEngine.RuleLineHandlers
Imports RulesEngine.EngineCommon
Imports RulesEngine.EngineSupport.Utilities

Public NotInheritable Class PureInsAddDeclineLineHandler 
    Inherits LineHandler

    Public Overrides Function AssemblyCode(ByVal line As EngineCommon.IRuleLine, ByVal declarations As EngineCommon.RuleEngineDeclarationCollection) As String
        If line Is Nothing Then
            Throw New ArgumentNullException("record")
        End If
        If declarations Is Nothing Then
            Throw New ArgumentNullException("declarations")
        End If
        If (String.Compare(line.Task, MyBase.TaskName, True, LineHandler.Culture) = 0) Then
            Return HandleAddDeclineFunction(line, declarations)
        Else
            Return MyBase.NextHandler(line.Task).AssemblyCode(line, declarations)
        End If
    End Function

    Private Function HandleAddDeclineFunction(ByVal line As IRuleLine, ByVal declarations As RuleEngineDeclarationCollection) As String
        Dim dataModelCode As String = ""
        Dim message As String = ""
        Dim outputObject As String

        Dim code As New Text.StringBuilder()

        message = line.Var1
        dataModelCode = line.Var2.Replace("""", "")

        'Get Data Model Code
        outputObject = String.Format("input.RISK_OBJECTS.{0}_POLICY_BINDER.{0}_OUTPUT", dataModelCode)

        'Try catch block for variable declarations
        code.AppendLine("try {")

        'Generate the correct c# code for the method
        code.AppendLine(String.Format("AddEntity({0});", outputObject))

        'Get and increment NextOIKey
        code.AppendLine("int nextOI = int.Parse(input.NextOINumber) + 1;")
        code.AppendLine("input.NextOINumber = nextOI.ToString();")

        'Get Index
        code.AppendLine(String.Format("int outCount = {0}.Count;", outputObject))

        'Set Properties on new entity
        code.AppendLine(String.Format("{0}[outCount -1].OI = ""OI"" + nextOI.ToString();", outputObject))
        code.AppendLine(String.Format("{0}[outCount -1].US = 1;", outputObject))

        code.AppendLine(String.Format("{0}[outCount -1].DECLINE_REASON = NullSafeGet({1});", outputObject, message))

        code.AppendLine("} catch(Exception ex) {AddNotEvaluatedWarning(ex);}")

        Return code.ToString()
    End Function

End Class
