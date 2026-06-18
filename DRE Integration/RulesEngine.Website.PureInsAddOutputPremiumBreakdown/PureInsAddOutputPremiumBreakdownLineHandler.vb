Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports RulesEngine.RuleLineHandlers
Imports RulesEngine.EngineCommon
Imports RulesEngine.EngineSupport.Utilities

Public NotInheritable Class PureInsAddOutputPremiumBreakdownLineHandler 
    Inherits LineHandler

    Private Const maxInputs As Integer = 36

    Public Overrides Function AssemblyCode(ByVal line As EngineCommon.IRuleLine, ByVal declarations As EngineCommon.RuleEngineDeclarationCollection) As String
        If line Is Nothing Then
            Throw New ArgumentNullException("record")
        End If
        If declarations Is Nothing Then
            Throw New ArgumentNullException("declarations")
        End If
        If (String.Compare(line.Task, MyBase.TaskName, True, LineHandler.Culture) = 0) Then
            Return HandleAddOutputPremiumBreakdownFunction(line, declarations)
        Else
            Return MyBase.NextHandler(line.Task).AssemblyCode(line, declarations)
        End If
    End Function

    Private Function HandleAddOutputPremiumBreakdownFunction(ByVal line As IRuleLine, ByVal declarations As RuleEngineDeclarationCollection) As String
        Dim dataModelCode As String = ""
        Dim ratingSection As String = ""
        'Dim premium As String = ""
        'Dim rate As String = ""
        Dim sumInsured As String = ""
        Dim outputPremiumBreakdownObject As String
        Dim PremiumBreakdownVarNames(maxInputs) As String
        Dim PremiumBreakdownVarValues(maxInputs) As String
        Dim code As New Text.StringBuilder()

        ratingSection = line.Var1
        'premium = line.Var2
        'rate = line.Var3
        PremiumBreakdownVarNames = Strings.Split(line.Parameters, "~", maxInputs + 1)
        PremiumBreakdownVarValues = Strings.Split(line.Var3, "~", maxInputs + 1)

        'sumInsured = line.Parameters
        dataModelCode = line.MathOperator.Replace("""", "")

        'If (String.IsNullOrEmpty(premium)) Then
        '    premium = "0"
        'End If
        'If (String.IsNullOrEmpty(rate)) Then
        '    rate = "0"
        'End If
        'If (String.IsNullOrEmpty(sumInsured)) Then
        ' sumInsured = "0"
        'End If

        'Get Data Model Code
        outputPremiumBreakdownObject = String.Format("input.RISK_OBJECTS.{0}_POLICY_BINDER.{0}_OUTPUT_PREMIUMBREAKDOWN", dataModelCode)

        'Try catch block for variable declarations
        code.AppendLine("try {")

        'Generate the correct c# code for the method
        code.AppendLine(String.Format("AddEntity({0});", outputPremiumBreakdownObject))

        'Get and increment NextOIKey
        code.AppendLine("int nextOI = int.Parse(input.NextOINumber) + 1;")
        code.AppendLine("input.NextOINumber = nextOI.ToString();")

        'Get Index
        code.AppendLine(String.Format("int outCount = {0}.Count;", outputPremiumBreakdownObject))

        'Set Properties on new entity
        code.AppendLine(String.Format("{0}[outCount -1].OI = ""OI"" + nextOI.ToString();", outputPremiumBreakdownObject))
        code.AppendLine(String.Format("{0}[outCount -1].US = 1;", outputPremiumBreakdownObject))

        'code.AppendLine(String.Format("{0}[outCount -1].RATE = {1};", outputPremiumBreakdownObject, rate))
        'code.AppendLine(String.Format("{0}[outCount -1].SUM_INSURED = {1};", outputPremiumBreakdownObject, sumInsured))
        'code.AppendLine(String.Format("{0}[outCount -1].PREMIUM = {1};", outputPremiumBreakdownObject, premium))
        code.AppendLine(String.Format("{0}[outCount -1].RISK_RATING_SECTION = NullSafeGet({1});", outputPremiumBreakdownObject, ratingSection))

        'need to loop the PremiumBreakdown variables array?
        For i As Integer = 0 To PremiumBreakdownVarNames.GetUpperBound(0)
            code.AppendLine(String.Format("{0}[outCount -1]." + PremiumBreakdownVarNames(i).ToUpper + " = NullSafeGet({1});", outputPremiumBreakdownObject, PremiumBreakdownVarValues(i)))
        Next
        'end of loop

        code.AppendLine("} catch(Exception ex) {AddNotEvaluatedWarning(ex);}")

        Return code.ToString()
    End Function
End Class

