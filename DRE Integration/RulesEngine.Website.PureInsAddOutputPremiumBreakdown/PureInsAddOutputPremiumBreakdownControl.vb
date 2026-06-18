Imports RulesEngine.EngineCommon
Imports RulesEngine.Website.RuleLineEditor
Imports RulesEngine.Website.WebCustomControls
Imports System.Web.UI
Imports System.Web.UI.WebControls

<ToolboxData("<{0}:PureInsAddOutputPremiumBreakdownControl runat=""server""> </{0}:PureInsAddOutputPremiumBreakdownControl>")>
Public NotInheritable Class PureInsAddOutputPremiumBreakdownControl 
    Inherits RuleLineInputControl

#Region "Fields"
    Private dataEntryControlsAdded As Boolean = False

    Private ruleLineObj As IRuleLine
    Private ruleSetObj As IRuleSet
    Private taskConfigObj As IDynamicRuleEngineTaskConfig

    Private ctrlRatingSection As LabelControlPair
    'Private ctrlRate As LabelControlPair
    Private ctrlSumInsured As LabelControlPair
    Private ctrlPremium As LabelControlPair
    Private ctrlDataModelCode As LabelControlPair
    Private Const maxInputs As Integer = 36
    Private ctrlPremiumBreakdown(maxInputs) As LabelControlPair
    Private ctrlPremiumBreakdownNames(maxInputs) As String
    Private ctrlPremiumBreakdownValues(maxInputs) As String

    Private Const controlPrefix As String = "ctlPureAddOutputPremiumBreakdown"
    Private Const labelPrefix As String = "lblPureAddOutputPremiumBreakdown"
    Private Const ratingSectionControlName As String = "ratingSection"
    'Private Const rateControlName As String = "rate"
    Private Const sumInsuredControlName As String = "sumInsured"
    Private Const premiumControlName As String = "premium"
    Private Const dataModelControlName As String = "dataModel"    
    Private Const sClassName As String = "PremiumBreakdown"

    Private labelUniqueId As Integer = 0
#End Region

#Region "Properties"

    Public Overrides Property RuleLine As EngineCommon.IRuleLine
        Get
            If ruleLineObj Is Nothing Or Not dataEntryControlsAdded Then
                Return Nothing
            End If

            'RatingSection
            ruleLineObj.Var1 = GetPostedControlValue(ctrlRatingSection.Control)
            'Premium
            ruleLineObj.Var2 = GetPostedControlValue(ctrlPremium.Control)
            'PremiumBreakdownValues
            ruleLineObj.Var3 = "" 'reset this as we don't want to keep appending to it
            For i As Integer = 0 To ctrlPremiumBreakdown.GetUpperBound(0)
                ruleLineObj.Var3 = ruleLineObj.Var3 + GetPostedControlValue(ctrlPremiumBreakdown(i).Control)
                ruleLineObj.Var3 = ruleLineObj.Var3 + "~"
            Next
            ruleLineObj.Var3 = Strings.Left(ruleLineObj.Var3, ruleLineObj.Var3.Length - 1)

            'SumInsured
            'ruleLineObj.Parameters = GetPostedControlValue(ctrlSumInsured.Control)
            ruleLineObj.Parameters = ""
            For i As Integer = 0 To ctrlPremiumBreakdownNames.GetUpperBound(0)
                ruleLineObj.Parameters = ruleLineObj.Parameters + ctrlPremiumBreakdownNames(i).ToString
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

            If ctrlRatingSection IsNot Nothing Then
                controls.Add(ctrlRatingSection.Control)
            End If
            If ctrlSumInsured IsNot Nothing Then
                controls.Add(ctrlSumInsured.Control)
            End If
            If ctrlPremium IsNot Nothing Then
                controls.Add(ctrlPremium.Control)
            End If
            'If ctrlRate IsNot Nothing Then
            'controls.Add(ctrlRate.Control)
            'End If
            'If ctrlCommissionOriginal IsNot Nothing Then
            '    controls.Add(ctrlCommissionOriginal.Control)
            'End If
            If ctrlPremiumBreakdown.Length > 0 Then
                For i As Integer = 0 To ctrlPremiumBreakdown.GetUpperBound(0)
                    controls.Add(ctrlPremiumBreakdown(i).Control)
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
            ctrlPremiumBreakdownNames(0) = "unique_id"
            ctrlPremiumBreakdownNames(1) = "header_group"
            ctrlPremiumBreakdownNames(2) = "level1"
            ctrlPremiumBreakdownNames(3) = "level2"
            ctrlPremiumBreakdownNames(4) = "level3"
            ctrlPremiumBreakdownNames(5) = "level4"
            ctrlPremiumBreakdownNames(6) = "is_header"
            ctrlPremiumBreakdownNames(7) = "orderby"
            ctrlPremiumBreakdownNames(8) = "description"
            ctrlPremiumBreakdownNames(9) = "sum_insured"
            ctrlPremiumBreakdownNames(10) = "percent_original"
            ctrlPremiumBreakdownNames(11) = "percent_base"
            ctrlPremiumBreakdownNames(12) = "percent_base_previous"
            ctrlPremiumBreakdownNames(13) = "percent_applicable"
            ctrlPremiumBreakdownNames(14) = "percent_override"
            ctrlPremiumBreakdownNames(15) = "percent_override_previous"
            ctrlPremiumBreakdownNames(16) = "base_applicable_multiplier"
            ctrlPremiumBreakdownNames(17) = "premium_original"
            ctrlPremiumBreakdownNames(18) = "premium_applicable"
            ctrlPremiumBreakdownNames(19) = "premium_override"
            ctrlPremiumBreakdownNames(20) = "is_original"
            ctrlPremiumBreakdownNames(21) = "is_locked"
            ctrlPremiumBreakdownNames(22) = "is_overriden"
            ctrlPremiumBreakdownNames(23) = "risk_rating_section"
            ctrlPremiumBreakdownNames(24) = "rate_type_id"
            ctrlPremiumBreakdownNames(25) = "state_id"
            ctrlPremiumBreakdownNames(26) = "country_id"
            ctrlPremiumBreakdownNames(27) = "net_ap_rp"
            ctrlPremiumBreakdownNames(28) = "gross_ap_rp"
            ctrlPremiumBreakdownNames(29) = "reason_for_override"
            ctrlPremiumBreakdownNames(30) = "premium_base"
            ctrlPremiumBreakdownNames(31) = "rating_string"
            ctrlPremiumBreakdownNames(32) = "use_si_for_rating"
            ctrlPremiumBreakdownNames(33) = "include_si_to_header"
            ctrlPremiumBreakdownNames(34) = "premium_multiplier"
            ctrlPremiumBreakdownNames(35) = "premium_override_calculated"
            ctrlPremiumBreakdownNames(36) = "rounding_amount"
            'ctrlPremiumBreakdownNames(37) = "total"

            If ruleLineObj IsNot Nothing Then
                ratingSection = ruleLineObj.Var1
                premium = ruleLineObj.Var2
                'rate = ruleLineObj.Var3

                tempValArr = Strings.Split(ruleLineObj.Var3, "~", ctrlPremiumBreakdown.GetUpperBound(0) + 1)

                If tempValArr.GetUpperBound(0) < ctrlPremiumBreakdown.GetUpperBound(0) Then
                    ReDim ctrlPremiumBreakdownValues(maxInputs)
                Else
                    ctrlPremiumBreakdownValues = Strings.Split(ruleLineObj.Var3, "~", ctrlPremiumBreakdown.GetUpperBound(0) + 1)
                End If

                'For i As Integer = 0 To ctrlPremiumBreakdown.GetUpperBound(0)
                '    'need to split the var 3 out and into ctrlCommissionValues
                'Next

                sumInsured = ruleLineObj.Parameters
                dataModel = ruleLineObj.MathOperator
            End If

            ctrlRatingSection = AddLabelControlPair(ratingSectionControlName, "Rating Section", ratingSection, Nothing, Nothing)
            ctrlPremium = AddLabelControlPair(premiumControlName, "Premium", premium, Nothing, Nothing)
            'ctrlRate = AddLabelControlPair(rateControlName, "Rate", rate, Nothing, Nothing)
            ctrlSumInsured = AddLabelControlPair(sumInsuredControlName, "Sum Insured", sumInsured, Nothing, Nothing)

            For i As Integer = 0 To ctrlPremiumBreakdownNames.GetUpperBound(0)
                ctrlPremiumBreakdown(i) = AddLabelControlPair(ctrlPremiumBreakdownNames(i), ctrlPremiumBreakdownNames(i), ctrlPremiumBreakdownValues(i), Nothing, Nothing)
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
        RuleLineEditorHelper.SetControlValue(ctrlRatingSection.Control, "")
        RuleLineEditorHelper.SetControlValue(ctrlPremium.Control, "")
        'RuleLineEditorHelper.SetControlValue(ctrlRate.Control, "")
        RuleLineEditorHelper.SetControlValue(ctrlSumInsured.Control, "")
        RuleLineEditorHelper.SetControlValue(ctrlDataModelCode.Control, "")
        For i As Integer = 0 To ctrlPremiumBreakdown.GetUpperBound(0)
            RuleLineEditorHelper.SetControlValue(ctrlPremiumBreakdown(i).Control, "")
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
