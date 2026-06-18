Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports RulesEngine.RuleLineHandlers
Imports RulesEngine.EngineCommon
Imports RulesEngine.EngineSupport.Utilities

Public NotInheritable Class PureInsAddOutputCommissionLineHandler 
    Inherits LineHandler

    Private Const maxInputs As Integer = 28

    Public Overrides Function AssemblyCode(ByVal line As EngineCommon.IRuleLine, ByVal declarations As EngineCommon.RuleEngineDeclarationCollection) As String
        If line Is Nothing Then
            Throw New ArgumentNullException("record")
        End If
        If declarations Is Nothing Then
            Throw New ArgumentNullException("declarations")
        End If
        If (String.Compare(line.Task, MyBase.TaskName, True, LineHandler.Culture) = 0) Then
            Return HandleAddOutputCommissionFunction(line, declarations)
        Else
            Return MyBase.NextHandler(line.Task).AssemblyCode(line, declarations)
        End If
    End Function

    Private Function HandleAddOutputCommissionFunction(ByVal line As IRuleLine, ByVal declarations As RuleEngineDeclarationCollection) As String
        Dim dataModelCode As String = ""
        'Dim ratingSection As String = ""
        Dim premium As String = ""
        'Dim rate As String = ""
        'Dim sumInsured As String = ""
        Dim outputCommissionObject As String
        Dim commissionVarNames(maxInputs) As String
        Dim commissionVarValues(maxInputs) As String
        Dim code As New Text.StringBuilder()

        'ratingSection = line.Var1
        premium = line.Var2
        'rate = line.Var3

        'commissionVarValues = Strings.Split(line.Var3, "~", commissionVarValues.GetUpperBound(0))
        commissionVarValues = Strings.Split(line.Var3, "~", maxInputs + 1)
        commissionVarNames = Strings.Split(line.Parameters, "~", maxInputs + 1)

        'sumInsured = line.Parameters
        dataModelCode = line.MathOperator.Replace("""", "")

        If (String.IsNullOrEmpty(premium)) Then
            premium = "0"
        End If
        'If (String.IsNullOrEmpty(rate)) Then
        '    rate = "0"
        'End If
        'If (String.IsNullOrEmpty(sumInsured)) Then
        '    sumInsured = "0"
        'End If

        'Get Data Model Code
        outputCommissionObject = String.Format("input.RISK_OBJECTS.{0}_POLICY_BINDER.{0}_OUTPUT_COMMISSION", dataModelCode)

        'Try catch block for variable declarations
        code.AppendLine("try {")

        'Generate the correct c# code for the method
        code.AppendLine(String.Format("AddEntity({0});", outputCommissionObject))

        'Get and increment NextOIKey
        code.AppendLine("int nextOI = int.Parse(input.NextOINumber) + 1;")
        code.AppendLine("input.NextOINumber = nextOI.ToString();")

        'Get Index
        code.AppendLine(String.Format("int outCount = {0}.Count;", outputCommissionObject))

        'Set Properties on new entity
        code.AppendLine(String.Format("{0}[outCount -1].OI = ""OI"" + nextOI.ToString();", outputCommissionObject))
        code.AppendLine(String.Format("{0}[outCount -1].US = 1;", outputCommissionObject))

        'code.AppendLine(String.Format("{0}[outCount -1].RATE = {1};", outputCommissionObject, rate))
        'code.AppendLine(String.Format("{0}[outCount -1].SUM_INSURED = {1};", outputCommissionObject, sumInsured))
        code.AppendLine(String.Format("{0}[outCount -1].PREMIUM = {1};", outputCommissionObject, premium))
        'code.AppendLine(String.Format("{0}[outCount -1].RISK_RATING_SECTION = NullSafeGet({1});", outputCommissionObject, ratingSection))

        'need to loop the commission variables array?
        For i As Integer = 0 To commissionVarNames.GetUpperBound(0)
            code.AppendLine(String.Format("{0}[outCount -1]." + commissionVarNames(i).ToUpper + " = NullSafeGet({1});", outputCommissionObject, commissionVarValues(i)))
        Next
        'end of loop

        code.AppendLine("} catch(Exception ex) {AddNotEvaluatedWarning(ex);}")

        Return code.ToString()
    End Function
End Class
