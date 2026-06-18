Imports RulesEngine.EngineCommon
Imports RulesEngine.Website.RuleLineEditor
Imports RulesEngine.Website.WebCustomControls
Imports System.Web.UI
Imports System.Web.UI.WebControls

<ToolboxData("<{0}:PureInsAddOutputCommissionControl runat=""server""> </{0}:PureInsAddOutputCommissionControl>")>
Public NotInheritable Class PureInsAddOutputCommissionControl 
    Inherits RuleLineInputControl

#Region "Fields"
    Private dataEntryControlsAdded As Boolean = False

    Private ruleLineObj As IRuleLine
    Private ruleSetObj As IRuleSet
    Private taskConfigObj As IDynamicRuleEngineTaskConfig

    'Private ctrlRatingSection As LabelControlPair
    'Private ctrlRate As LabelControlPair
    'Private ctrlSumInsured As LabelControlPair
    Private ctrlPremium As LabelControlPair
    Private ctrlDataModelCode As LabelControlPair
    Private Const maxInputs As Integer = 28
    Private ctrlCommission(maxInputs) As LabelControlPair
    Private ctrlCommissionNames(maxInputs) As String
    Private ctrlCommissionValues(maxInputs) As String

    Private Const controlPrefix As String = "ctlPureAddOutputCommission"
    Private Const labelPrefix As String = "lblPureAddOutputCommission"
    'Private Const ratingSectionControlName As String = "ratingSection"
    'Private Const rateControlName As String = "rate"
    'Private Const sumInsuredControlName As String = "sumInsured"
    Private Const premiumControlName As String = "premium"
    Private Const dataModelControlName As String = "dataModel"    

    Private labelUniqueId As Integer = 0
#End Region

#Region "Properties"

    Public Overrides Property RuleLine As EngineCommon.IRuleLine
        Get
            If ruleLineObj Is Nothing Or Not dataEntryControlsAdded Then
                Return Nothing
            End If

            'RatingSection
            'ruleLineObj.Var1 = GetPostedControlValue(ctrlRatingSection.Control)
            'Premium
            'ruleLineObj.Var2 = GetPostedControlValue(ctrlPremium.Control)
            'CommissionValues

            ruleLineObj.Var3 = "" 'reset this as we don't want to keep appending to it
            For i As Integer = 0 To ctrlCommission.GetUpperBound(0)
                ruleLineObj.Var3 = ruleLineObj.Var3 + GetPostedControlValue(ctrlCommission(i).Control)
                ruleLineObj.Var3 = ruleLineObj.Var3 + "~"
            Next
            ruleLineObj.Var3 = Strings.Left(ruleLineObj.Var3, ruleLineObj.Var3.Length - 1)

            'SumInsured
            'ruleLineObj.Parameters = GetPostedControlValue(ctrlSumInsured.Control)
            ruleLineObj.Parameters = ""
            For i As Integer = 0 To ctrlCommissionNames.GetUpperBound(0)
                ruleLineObj.Parameters = ruleLineObj.Parameters + ctrlCommissionNames(i).ToString
                ruleLineObj.Parameters = ruleLineObj.Parameters + "~"
            Next

            ruleLineObj.Parameters = Strings.Left(ruleLineObj.Parameters, ruleLineObj.Parameters.Length - 1)

            'DataModelCode
            ruleLineObj.MathOperator = GetPostedControlValue(ctrlDataModelCode.Control)

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

            'If ctrlRatingSection IsNot Nothing Then
            '    controls.Add(ctrlRatingSection.Control)
            'End If
            'If ctrlSumInsured IsNot Nothing Then
            '    controls.Add(ctrlSumInsured.Control)
            'End If
            If ctrlPremium IsNot Nothing Then
                controls.Add(ctrlPremium.Control)
            End If
            'If ctrlRate IsNot Nothing Then
            '    'controls.Add(ctrlRate.Control)
            'End If
            'If ctrlCommissionOriginal IsNot Nothing Then
            '    controls.Add(ctrlCommissionOriginal.Control)
            'End If
            If ctrlCommission.Length > 0 Then
                For i As Integer = 0 To ctrlCommission.GetUpperBound(0)
                    controls.Add(ctrlCommission(i).Control)
                Next
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

    Public Overrides Sub BuildUI()

        Dim ratingSection As String = ""
        Dim premium As String = ""
        Dim rate As String = ""
        Dim sumInsured As String = ""
        Dim dataModel As String = ""
        Dim tempValArr() As String

        If dataEntryControlsAdded Then
            Exit Sub
        End If

        Try
            'initialise each control's name
            ctrlCommissionNames(0) = "unique_id"
            ctrlCommissionNames(1) = "output_id"
            ctrlCommissionNames(2) = "agent_party_code"
            ctrlCommissionNames(3) = "peril_code"
            ctrlCommissionNames(4) = "commission_original"
            ctrlCommissionNames(5) = "commission_percent"
            ctrlCommissionNames(6) = "commission_band_code"
            ctrlCommissionNames(7) = "maximum_commission"
            ctrlCommissionNames(8) = "maximum_commission_percent"
            ctrlCommissionNames(9) = "currency_code"
            ctrlCommissionNames(10) = "tax_group_code"
            ctrlCommissionNames(11) = "is_locked"
            ctrlCommissionNames(12) = "is_overriden"
            ctrlCommissionNames(13) = "reason_for_override"
            ctrlCommissionNames(14) = "COB_code"
            ctrlCommissionNames(15) = "COB_description"
            ctrlCommissionNames(16) = "agent_party_description"
            ctrlCommissionNames(17) = "gross_annual_commission"
            ctrlCommissionNames(18) = "net_annual_commission"
            ctrlCommissionNames(19) = "net_annual_commission_overriden"
            ctrlCommissionNames(20) = "net_ap_rp"
            ctrlCommissionNames(21) = "net_ap_rp_overriden"
            ctrlCommissionNames(22) = "gross_ap_rp"
            ctrlCommissionNames(23) = "commission_percent_overriden"
            ctrlCommissionNames(24) = "is_lead"
            ctrlCommissionNames(25) = "is_retained"
            ctrlCommissionNames(26) = "premium_ap_rp"
            ctrlCommissionNames(27) = "commission_percent_overriden_previous"
            ctrlCommissionNames(28) = "output_to_s4i"

            If ruleLineObj IsNot Nothing Then
                ratingSection = ruleLineObj.Var1
                premium = ruleLineObj.Var2
                'rate = ruleLineObj.Var3

                tempValArr = Strings.Split(ruleLineObj.Var3, "~", ctrlCommission.GetUpperBound(0) + 1)

                If tempValArr.GetUpperBound(0) < ctrlCommission.GetUpperBound(0) Then
                    ReDim ctrlCommissionValues(maxInputs)
                Else
                    ctrlCommissionValues = Strings.Split(ruleLineObj.Var3, "~", ctrlCommission.GetUpperBound(0) + 1)
                End If

                'For i As Integer = 0 To ctrlCommission.GetUpperBound(0)
                '    'need to split the var 3 out and into ctrlCommissionValues
                'Next

                sumInsured = ruleLineObj.Parameters
                dataModel = ruleLineObj.MathOperator
            End If

            'ctrlRatingSection = AddLabelControlPair(ratingSectionControlName, "Rating Section", ratingSection, Nothing, Nothing)
            ctrlPremium = AddLabelControlPair(premiumControlName, "Premium", premium, Nothing, Nothing)
            'ctrlRate = AddLabelControlPair(rateControlName, "Rate", rate, Nothing, Nothing)
            'ctrlSumInsured = AddLabelControlPair(sumInsuredControlName, "Sum Insured", sumInsured, Nothing, Nothing)

            For i As Integer = 0 To ctrlCommissionNames.GetUpperBound(0)
                ctrlCommission(i) = AddLabelControlPair(ctrlCommissionNames(i), ctrlCommissionNames(i), ctrlCommissionValues(i), Nothing, Nothing)
            Next

            ctrlDataModelCode = AddLabelControlPair(dataModelControlName, "Data Model", dataModel, Nothing, Nothing)
            dataEntryControlsAdded = True

        Catch ex As Exception
            'add a control that contains the error message
            AddLabelControlPair("Error", ex.Message, ex.InnerException.ToString, Nothing, Nothing)
            dataEntryControlsAdded = False
        End Try

    End Sub

    Public Overrides Sub ClearAndInitialiseInputControlValues()
        'RuleLineEditorHelper.SetControlValue(ctrlRatingSection.Control, "")
        RuleLineEditorHelper.SetControlValue(ctrlPremium.Control, "")
        'RuleLineEditorHelper.SetControlValue(ctrlRate.Control, "")
        'RuleLineEditorHelper.SetControlValue(ctrlSumInsured.Control, "")
        RuleLineEditorHelper.SetControlValue(ctrlDataModelCode.Control, "")
        For i As Integer = 0 To ctrlCommission.GetUpperBound(0)
            RuleLineEditorHelper.SetControlValue(ctrlCommission(i).Control, "")
        Next
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

#End Region

End Class
