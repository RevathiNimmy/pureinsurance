Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports RulesEngine.RuleLineHandlers
Imports RulesEngine.EngineCommon
Imports RulesEngine.EngineSupport.Utilities

Public NotInheritable Class PureInsPropertiesLineHandler 
    Inherits LineHandler

    Public Overrides Function AssemblyCode(ByVal line As EngineCommon.IRuleLine, ByVal declarations As EngineCommon.RuleEngineDeclarationCollection) As String
        If line Is Nothing Then
            Throw New ArgumentNullException("record")
        End If
        If declarations Is Nothing Then
            Throw New ArgumentNullException("declarations")
        End If
        If (String.Compare(line.Task, MyBase.TaskName, True, LineHandler.Culture) = 0) Then
            Return HandlePropertiesFunction(line, declarations)
        Else
            Return MyBase.NextHandler(line.Task).AssemblyCode(line, declarations)
        End If
    End Function

    Private Function HandlePropertiesFunction(ByVal line As IRuleLine, ByVal declarations As RuleEngineDeclarationCollection) As String
        Dim result As String = ""
        Dim prop As String = ""
        Dim dataModel As String = ""

        Dim assemblyName As String = System.Reflection.Assembly.GetExecutingAssembly().FullName
        Dim className = ""

        Dim gisPolicyLinkRef As String

        Dim code As New Text.StringBuilder()

        result = line.Var1
        prop = line.Var2
        dataModel = line.Var3

        'Get Data Model Code
        gisPolicyLinkRef = String.Format("input.RISK_OBJECTS.{0}_POLICY_BINDER.GIS_POLICY_LINK_ID", dataModel)


        code.AppendLine("try")
        code.AppendLine("{")
        code.AppendFormat("  {0} = NullSafeGet(ExtensionHelper.Invoke(""{1}"", ""RulesEngine.Website.PureInsProperties.GetProperty"", {2}, ""{3}""));{4}", _
                                result, assemblyName, gisPolicyLinkRef, prop, Environment.NewLine)
        code.AppendLine("} catch(Exception ex)")
        code.AppendLine("{")
        code.AppendLine("  AddNotEvaluatedWarning(ex);")
        code.AppendLine("}")

        Return code.ToString()
    End Function

End Class
