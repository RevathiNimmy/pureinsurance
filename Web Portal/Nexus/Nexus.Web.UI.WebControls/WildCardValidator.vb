Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Security.Permissions
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports NexusProvider
''' <summary>
''' Validation control which will validate multiple controls in a form to check if the 
''' use of wild cards complies with the setting of system options
''' </summary>
''' <remarks></remarks>
<AspNetHostingPermission(SecurityAction.LinkDemand, Level:=AspNetHostingPermissionLevel.Minimal)> _
<AspNetHostingPermission(SecurityAction.InheritanceDemand, Level:=AspNetHostingPermissionLevel.Minimal)> _
<ToolboxData("<{0}:WildCardValidator runat=server id=vldWildCard />")> _
Public Class WildCardValidator
    Inherits BaseValidator

#Region "Overriden Methods"
    Protected Overloads Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)

        ' Setting SetFocusOnError before calling the base AddAttributesToRender because
        ' the AddAttributesToRender is going to check for "SetFocusOnError" value
        MyBase.SetFocusOnError = False
        ' Because we have many fields to check!
        MyBase.AddAttributesToRender(writer)

        'if enable client script is true then first javascript will be called
        If Me.RenderUplevel AndAlso Me.EnableClientScript Then
            Dim clientID As String = Me.ClientID

            Page.ClientScript.RegisterExpandoAttribute(clientID, "controlstovalidate", Me.GenerateClientSideControlsToValidate())
            Page.ClientScript.RegisterExpandoAttribute(clientID, "condition", PropertyConverter.EnumToString(GetType(Conditions), Condition))
            Page.ClientScript.RegisterExpandoAttribute(clientID, "evaluationfunction", "WildCardValidator")
        End If
    End Sub
    'It holds the list of the controld to be validated by this control
    Protected Overloads Overrides Function ControlPropertiesValid() As Boolean

        If Me.ControlsToValidate.Trim().Length = 0 Then
            Throw New HttpException(String.Format("The ControlsToValidate property of {0} cannot be blank.", Me.ID))
        End If

        Dim controlToValidateIDs As String() = Me.GetControlsToValidateIDs()

        For Each controlToValidateID As String In controlToValidateIDs
            MyBase.CheckControlValidationProperty(controlToValidateID, "ControlsToValidate")
        Next

        Return True
    End Function
    'This methods Validates the value 
    Protected Overloads Overrides Function EvaluateIsValid() As Boolean

        Dim controlToValidateIDs As String() = Me.GetControlsToValidateIDs()

        Select Case Condition
            Case Conditions.NoWildCard
                'loop though controls and if any wild card is found return false.
                'also check the string length meets the minimum string length condition
                For Each controlToValidateID As String In controlToValidateIDs
                    Dim controlToValidateValue As String = MyBase.GetControlValidationValue(controlToValidateID)
                    controlToValidateValue = IIf(controlToValidateValue Is Nothing, String.Empty, controlToValidateValue.Trim())
                    If controlToValidateValue.Contains("%") Then

                        Return False
                    End If
                Next
                'no wild card found
                Return True
            Case Conditions.AllowWildCardAtEnd
                'loop through and if we find a wild card then check where it it
                'if it's not at the end of the string then return false
                'also check the string length meets the minimum string length condition
                For Each controlToValidateID As String In controlToValidateIDs
                    Dim controlToValidateValue As String = MyBase.GetControlValidationValue(controlToValidateID)
                    controlToValidateValue = IIf(controlToValidateValue Is Nothing, String.Empty, controlToValidateValue.Trim())
                    If controlToValidateValue.Length > 0 Then
                        'allow wild card at end, but nowhere else. wild card not allowed at end if the string is a single character
                        If Left(controlToValidateValue, controlToValidateValue.Length - 1).Contains("%") _
                            Or controlToValidateValue = "%" Then
                            Return False
                        End If
                    End If
                Next
                'no wild card found other than at the end of the string
                Return True
            Case Conditions.AllowWildCard
                'no need to validate
                Return True
            Case Else
                ' This line shouldn't be reached
                Throw New Exception("End of validation has been reached without a result!")
        End Select
    End Function

    Protected Overloads Overrides Sub OnPreRender(ByVal e As EventArgs)
        MyBase.OnPreRender(e)
        If MyBase.RenderUplevel Then
            Me.Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "WildCardValidator", Page.ClientScript.GetWebResourceUrl(Me.GetType(), "Nexus.Web.UI.WebControls.WildCardValidator.js"))
        End If
    End Sub
#End Region

#Region "Helper Methods"

    Private Function GetControlsToValidateIDs() As String()
        Dim controlsToValidate As String = Me.ControlsToValidate.Replace(" ", "")
        Dim controlToValidateIDs As String()
        Try
            controlToValidateIDs = controlsToValidate.Split(","c)
        Catch ex As ArgumentOutOfRangeException
            Throw New FormatException(String.Format("The ControlsToValidate property of {0} is not well-formatted.", Me.ID), ex)
        End Try
        Return controlToValidateIDs
    End Function

    Private Function GenerateClientSideControlsToValidate() As String
        Dim controlToValidateIDs As String() = Me.GetControlsToValidateIDs()
        Dim controlToValidateIDTrimmed As String
        Dim controlRenderIDs As String = String.Empty
        For Each controlToValidateID As String In controlToValidateIDs
            controlToValidateIDTrimmed = controlToValidateID.Trim()
            If controlToValidateIDTrimmed = String.Empty Then
                Throw New FormatException(String.Format("The ControlsToValidate property of {0} is not well-formatted.", Me.ID))
            End If
            controlRenderIDs += "," + MyBase.GetControlRenderID(controlToValidateIDTrimmed)
        Next
        controlRenderIDs = controlRenderIDs.Remove(0, 1)
        ' Removing the first ","
        Return controlRenderIDs
    End Function
#End Region

#Region "Properties"
    <Browsable(False)> _
       <EditorBrowsable(EditorBrowsableState.Never)> _
       Public Shadows Property ErrorMessage() As String
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)
            Throw New NotSupportedException("ErrorMessage is not supported because you have multiple controls to validate")
        End Set
    End Property
    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Property SetFocusOnError() As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
            Throw New NotSupportedException("SetFocusOnError is not supported because you have multiple controls to validate")
        End Set
    End Property

    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Property ControlToValidate() As String
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)
            Throw New NotSupportedException("ControlToValidate is not supported because you have multiple controls to validate")
        End Set
    End Property

    ''' <summary>
    ''' To Set the Different Error Message
    ''' </summary>
    <Browsable(True)> _
    <Category("Behavior")> _
    <Themeable(False)> _
    <DefaultValue("")> _
    <Description("To set the error message on AllowWildCardAtEnd condition")> _
    Public Property AllowWildCardAtEndErrorMessage() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("AllowWildCardAtEndErrorMessage"))), String.Empty, Convert.ToString(ViewState("AllowWildCardAtEndErrorMessage"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("AllowWildCardAtEndErrorMessage") = value
        End Set
    End Property

    ''' <summary>
    ''' To Set the Different Error Message
    ''' </summary>
    <Browsable(True)> _
    <Category("Behavior")> _
    <Themeable(False)> _
    <DefaultValue("")> _
    <Description("To set the error message on NoWildCard condition")> _
    Public Property NoWildCardErrorMessage() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("NoWildCardErrorMessage"))), String.Empty, Convert.ToString(ViewState("NoWildCardErrorMessage"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("NoWildCardErrorMessage") = value
        End Set
    End Property
    ''' <summary>
    ''' Comma separated list of control IDs that you want to check
    ''' </summary>
    <Browsable(True)> _
    <Category("Behavior")> _
    <Themeable(False)> _
    <DefaultValue("")> _
    <Description("Comma separated list of control IDs that you want to check")> _
    Public Property ControlsToValidate() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("ControlsToValidate"))), String.Empty, Convert.ToString(ViewState("ControlsToValidate"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("ControlsToValidate") = value
        End Set
    End Property
    ''' <summary>
    ''' The condition used to compare the value of the fields, 
    ''' e.g. 'NoWildCard', will return false if any wild card is found
    ''' If condition is set to Auto then property is set according to system option settings
    ''' </summary>
    <Browsable(True)> _
    <Themeable(False)> _
    <Category("Behavior")> _
    <DefaultValue(Conditions.AllowWildCard)> _
    <Description("The condition used to compare the value of the fields, e.g. 'NoWildCard', will return false if any wild card is found")> _
    Public Property Condition() As Conditions
        Get
            If String.IsNullOrEmpty(ViewState("Condition")) Then
                Return Conditions.AllowWildCard
            Else
                Return ViewState("Condition")
            End If
        End Get

        Set(ByVal value As Conditions)
            If value = Conditions.AllowWildCardAtEnd Then
                MyBase.ErrorMessage = AllowWildCardAtEndErrorMessage
                ViewState("Condition") = value
            ElseIf value = Conditions.NoWildCard Then
                MyBase.ErrorMessage = NoWildCardErrorMessage
                ViewState("Condition") = value
            ElseIf value = Conditions.Auto Then
                Dim oWebService As ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oDisableOptionSettings, oEnableOptionSettings As NexusProvider.OptionTypeSetting
                oEnableOptionSettings = Nothing
                'Disable All Wildcard Searches
                oDisableOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5065)
                'Enable Wildcard Searches Ending With %
                oEnableOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5066)

                'If SAM returns nothing for the option then set "0" -start
                If oDisableOptionSettings.OptionValue Is Nothing Then
                    oDisableOptionSettings.OptionValue = "0"
                End If

                If oEnableOptionSettings.OptionValue Is Nothing Then
                    oEnableOptionSettings.OptionValue = "0"
                End If
                'If SAM returns nothing for the option then set "0" -end
                'If Wild card is not disable then check the enable wildcard searches option,if it is on then set 
                If oDisableOptionSettings IsNot Nothing AndAlso oDisableOptionSettings.OptionValue = "0" AndAlso oEnableOptionSettings.OptionValue = "1" Then
                    ViewState("Condition") = Conditions.AllowWildCardAtEnd
                    MyBase.ErrorMessage = AllowWildCardAtEndErrorMessage
                ElseIf oDisableOptionSettings IsNot Nothing AndAlso oDisableOptionSettings.OptionValue = "0" AndAlso oEnableOptionSettings.OptionValue = "0" Then
                    ViewState("Condition") = Conditions.AllowWildCard
                ElseIf oDisableOptionSettings.OptionValue = "1" Then
                    'If wild card is  disable then set it
                    ViewState("Condition") = Conditions.NoWildCard
                    MyBase.ErrorMessage = NoWildCardErrorMessage
                End If
            Else
                'in rest of the condition
                ViewState("Condition") = value
            End If

        End Set
    End Property
#End Region
   
#Region "Enum"

    Public Enum Conditions
        ''' <summary>
        ''' No wild cards allowed
        ''' </summary>
        NoWildCard = 0
        ''' <summary>
        ''' Wild card allowed at end of search string
        ''' </summary>
        AllowWildCardAtEnd = 1
        ''' <summary>
        ''' Wild cards allowed
        ''' </summary>
        AllowWildCard = 2
        ''' <summary>
        ''' SAM call made to check system option setting
        ''' </summary>
        Auto = 3
    End Enum
#End Region
End Class




