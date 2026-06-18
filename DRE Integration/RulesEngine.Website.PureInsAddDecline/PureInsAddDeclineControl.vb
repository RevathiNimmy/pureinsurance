Imports RulesEngine.EngineCommon
Imports RulesEngine.Website.RuleLineEditor
Imports RulesEngine.Website.WebCustomControls
Imports System.Web.UI
Imports System.Web.UI.WebControls

<ToolboxData("<{0}:PureInsAddDeclineControl runat=""server""> </{0}:PureInsAddDeclineControl>")>
Public NotInheritable Class PureInsAddDeclineControl 
    Inherits RuleLineInputControl

#Region "Fields"
    Private dataEntryControlsAdded As Boolean = False

    Private ruleLineObj As IRuleLine
    Private ruleSetObj As IRuleSet
    Private taskConfigObj As IDynamicRuleEngineTaskConfig

    Private ctrlMessage As LabelControlPair
    Private ctrlDataModelCode As LabelControlPair

    Private Const controlPrefix As String = "ctlPureAddDecline"
    Private Const labelPrefix As String = "lblPureAddDecline"
    Private Const messageControlName As String = "message"
    Private Const dataModelControlName As String = "dataModel"

    Private labelUniqueId As Integer = 0
#End Region

#Region "Properties"

    Public Overrides Property RuleLine As EngineCommon.IRuleLine
        Get
            If ruleLineObj Is Nothing Or Not dataEntryControlsAdded Then
                Return Nothing
            End If

            'Message
            ruleLineObj.Var1 = GetPostedControlValue(ctrlMessage.Control)
            'Data Model Code
            ruleLineObj.Var2 = GetPostedControlValue(ctrlDataModelCode.Control)

            Return ruleLineObj
        End Get

        Set(ByVal value As EngineCommon.IRuleLine)
            ruleLineObj = value
        End Set
    End Property

    Public Overrides WriteOnly Property RuleSet As EngineCommon.IRuleSet
        Set(ByVal value As EngineCommon.IRuleSet)
            ruleSetObj = value
        End Set
    End Property

    Public Overrides ReadOnly Property TargetControls As System.Collections.Generic.IEnumerable(Of System.Web.UI.Control)
        Get
            If Not dataEntryControlsAdded Then
                Throw New InvalidOperationException("Cannot be called before BuildUI has been called")
            End If

            Dim controls As New List(Of System.Web.UI.Control)

            If ctrlMessage IsNot Nothing Then
                controls.Add(ctrlMessage.Control)
            End If
            If ctrlDataModelCode IsNot Nothing Then
                controls.Add(ctrlDataModelCode.Control)
            End If

            Return controls
        End Get
    End Property

    Public Overrides WriteOnly Property TaskConfig As RuleLineEditor.IDynamicRuleEngineTaskConfig
        Set(ByVal value As RuleLineEditor.IDynamicRuleEngineTaskConfig)
            taskConfigObj = value
        End Set
    End Property
#End Region

#Region "Methods"

#End Region

    Public Overrides Sub BuildUI()
        Dim dataModel As String = ""
        Dim message As String = ""

        If dataEntryControlsAdded Then
            Exit Sub
        End If

        If ruleLineObj IsNot Nothing Then
            message = ruleLineObj.Var1
            dataModel = ruleLineObj.Var2
        End If

        ctrlMessage = AddLabelControlPair(messageControlName, "Decline Message", message, Nothing, Nothing)
        ctrlDataModelCode = AddLabelControlPair(dataModelControlName, "Data Model", dataModel, Nothing, Nothing)

        dataEntryControlsAdded = True
    End Sub

    Public Overrides Sub ClearAndInitialiseInputControlValues()
        RuleLineEditorHelper.SetControlValue(ctrlMessage.Control, "")
        RuleLineEditorHelper.SetControlValue(ctrlDataModelCode.Control, "")
    End Sub

    Protected Function GetPostedControlValue(ByVal control As Control) As String
        Dim value As String = Nothing

        For Each key As String In Me.Page.Request.Form.AllKeys
            If key IsNot Nothing Then
                Dim ele As String()
                ele = key.Split(Char.Parse("$"))
                If ele(ele.Length - 1) = control.ID Then
                    value = Me.Page.Request.Form(key)
                    Exit For
                End If
            End If
        Next

        Return value
    End Function

    Private Function AddLabelControlPair(ByVal name As String, ByVal caption As String, ByVal value As String, ByVal labelAttributes As Dictionary(Of String, String), ByVal controlAttributes As Dictionary(Of String, String)) As LabelControlPair
        Dim ctl As WebControl
        ctl = CreateControl(name, value)

        Dim lbl As Label
        lbl = CreateLabel(caption, ctl)

        ApplyAttributesToUIElement(lbl, labelAttributes)
        ApplyAttributesToUIElement(ctl, controlAttributes)

        AddControl(lbl, ctl, "question", "")

        Return New LabelControlPair(lbl, ctl)
    End Function

    Private Sub ApplyAttributesToUIElement(ByVal ctl As WebControl, ByVal attributes As Dictionary(Of String, String))
        Dim found As Boolean

        If attributes IsNot Nothing Then
            For Each key As String In attributes.Keys
                found = False

                For Each existingKey As String In ctl.Attributes.Keys
                    If key.Equals(existingKey, StringComparison.OrdinalIgnoreCase) Then
                        ctl.Attributes(existingKey) = attributes(key)
                        found = True
                        Exit For
                    End If
                Next

                If Not found Then
                    ctl.Attributes.Add(key, attributes(key))
                End If
            Next
        End If
    End Sub

    Private Sub AddControl(ByVal lbl As Label, ByVal ctl As WebControl, ByVal cssClass As String, ByVal postControlExtraHtml As String)
        MyBase.Controls.Add(New LiteralControl("<div class=""" + cssClass + """ id=""" + ctl.ID + "_Div"">" & Environment.NewLine))
        MyBase.Controls.Add(lbl)
        MyBase.Controls.Add(ctl)
        If TypeOf ctl Is GridView Then
            ctl.DataBind()
        End If
        MyBase.Controls.Add(New LiteralControl("</div>" + postControlExtraHtml.ToString() + Environment.NewLine))
    End Sub

    Private Function CreateControl(ByVal name As String, ByVal value As String) As WebControl
        Dim txt As New TextBox
        txt.Text = value
        txt.ID = controlPrefix + name

        Return txt
    End Function
    Private Function CreateLabel(ByVal text As String, ByVal associatedControl As WebControl) As Label
        Dim lbl As New Label()

        labelUniqueId += 1

        lbl.ID = labelPrefix & labelUniqueId.ToString()
        lbl.Text = text
        lbl.AssociatedControlID = associatedControl.ID

        Return lbl
    End Function

End Class
