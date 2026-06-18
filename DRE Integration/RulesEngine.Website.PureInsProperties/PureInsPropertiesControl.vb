Imports RulesEngine.EngineCommon
Imports RulesEngine.Website.RuleLineEditor
Imports RulesEngine.Website.WebCustomControls
Imports System.Web.UI
Imports System.Web.UI.WebControls

<ToolboxData("<{0}:PureInsPropertiesControl runat=""server""> </{0}:PureInsPropertiesControl>")>
Public NotInheritable Class PureInsPropertiesControl 
    Inherits RuleLineInputControl

#Region "Fields"
    Private dataEntryControlsAdded As Boolean = False

    Private ruleLineObj As IRuleLine
    Private ruleSetObj As IRuleSet
    Private taskConfigObj As IDynamicRuleEngineTaskConfig

    Private ctrlResult As LabelControlPair
    Private ctrlProperty As LabelControlPair
    Private ctrlDataModelCode As LabelControlPair

    Private Const controlPrefix As String = "ctlPureProperties"
    Private Const labelPrefix As String = "lblPureProperties"
    Private Const resultControlName As String = "result"
    Private Const propertyControlName As String = "property"
    Private Const dataModelControlName As String = "dataModel"

    Private labelUniqueId As Integer = 0
#End Region

#Region "Properties"

    Public Overrides Property RuleLine As EngineCommon.IRuleLine
        Get
            If ruleLineObj Is Nothing Or Not dataEntryControlsAdded Then
                Return Nothing
            End If

            'Result
            ruleLineObj.Var1 = GetPostedControlValue(ctrlResult.Control)
            'Property
            ruleLineObj.Var2 = GetPostedControlValue(ctrlProperty.Control)
            'Data Model Code 
            ruleLineObj.Var3 = GetPostedControlValue(ctrlDataModelCode.Control)

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

            If ctrlResult IsNot Nothing Then
                controls.Add(ctrlResult.Control)
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
        Dim result As String = ""
        Dim prop As String = ""
        Dim dataModel As String = ""

        If dataEntryControlsAdded Then
            Exit Sub
        End If

        If ruleLineObj IsNot Nothing Then
            result = ruleLineObj.Var1
            prop = ruleLineObj.Var2
            dataModel = ruleLineObj.Var3
        End If

        Dim properties As New List(Of ListItem)
        properties.Add(New ListItem("Policy - Branch Code", "POLICY/BranchCode"))
        properties.Add(New ListItem("Policy - Lead Agent Name", "POLICY/LeadAgentName"))
        properties.Add(New ListItem("Policy - Lead Agent Code", "POLICY/LeadAgentCode"))
        properties.Add(New ListItem("Policy - Renewal Stop Code", "POLICY/RenewalStopCode"))
        properties.Add(New ListItem("Risk - Risk Type Code", "RISK/RiskTypeCode"))
        properties.Add(New ListItem("Risk - Risk Type Description", "RISK/RiskTypeDescription"))
        properties.Add(New ListItem("Risk - Risk Description", "RISK/RiskDescription"))
        properties.Add(New ListItem("Party - Date Of Birth", "PARTY/DateOfBirth"))
        properties.Add(New ListItem("Party - Forename", "PARTY/Forename"))
        properties.Add(New ListItem("Party - Surname", "PARTY/Surname"))
        properties.Add(New ListItem("Party - Initials", "PARTY/Initials"))
        properties.Add(New ListItem("Party - Salutation", "PARTY/Salutation"))
        properties.Add(New ListItem("Party - Resolved Name", "PARTY/ResolvedName"))
        properties.Add(New ListItem("Party - Shortname", "PARTY/ShortName"))
        properties.Add(New ListItem("Party - Branch Code", "PARTY/BranchCode"))
        properties.Add(New ListItem("Party - Sub-Branch Code", "PARTY/SubBranchCode"))
        properties.Add(New ListItem("Party - Party Type", "PARTY/PartyType"))
        properties.Add(New ListItem("Party - Renewal Stop Code", "PARTY/RenewalStopCode"))

        MyBase.Controls.Add(New LiteralControl("<div id=""TextBoxDropDown"">"))
        ctrlResult = AddLabelControlPair(resultControlName, "Result", result, Nothing, Nothing, Nothing)
        ctrlDataModelCode = AddLabelControlPair(dataModelControlName, "Data Model", dataModel, Nothing, Nothing, Nothing)
        ctrlProperty = AddLabelControlPair(propertyControlName, "Property", prop, Nothing, Nothing, properties)
        MyBase.Controls.Add(New LiteralControl("</div>"))

        dataEntryControlsAdded = True
    End Sub

    Public Overrides Sub ClearAndInitialiseInputControlValues()
        RuleLineEditorHelper.SetControlValue(ctrlResult.Control, "")
        RuleLineEditorHelper.SetControlValue(ctrlProperty.Control, "")
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

    Private Function AddLabelControlPair(ByVal name As String, ByVal caption As String, ByVal value As String, _
                                         ByVal labelAttributes As Dictionary(Of String, String), _
                                         ByVal controlAttributes As Dictionary(Of String, String), _
                                         ByVal dataSource As IEnumerable(Of ListItem)) As LabelControlPair
        Dim ctl As WebControl
        ctl = CreateControl(name, value, dataSource)

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
        MyBase.Controls.Add(New LiteralControl("<div class=""" + cssClass + """>" & Environment.NewLine))
        MyBase.Controls.Add(lbl)
        MyBase.Controls.Add(ctl)
        MyBase.Controls.Add(New LiteralControl("</div>" + postControlExtraHtml.ToString() + Environment.NewLine))
    End Sub

    Private Function CreateControl(ByVal name As String, ByVal value As String, ByVal dataSource As IEnumerable(Of ListItem)) As WebControl
        If dataSource Is Nothing Then
            Dim txt As New TextBox
            txt.Text = value
            txt.ID = controlPrefix + name

            Return txt
        Else
            Dim list As New DropDownList
            list.ID = controlPrefix + name

            For Each item As ListItem In dataSource
                list.Items.Add(item)
            Next

            list.SelectedValue = value

            Return list
        End If
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
