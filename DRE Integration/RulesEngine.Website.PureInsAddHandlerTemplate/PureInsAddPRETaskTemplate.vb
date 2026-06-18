Imports RulesEngine.EngineCommon
Imports RulesEngine.Website.RuleLineEditor
Imports RulesEngine.Website.WebCustomControls
Imports System.Web.UI
Imports System.Web.UI.WebControls

<ToolboxData("<{0}:PureInsAddOutputCommissionControl runat=""server""> </{0}:PureInsAddOutputCommissionControl>")>
Public NotInheritable Class PureInsAddPRETaskTemplate 
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
    Private ctrlYourObjectHere(maxInputs) As LabelControlPair
    Private ctrlYourObjectHereNames(maxInputs) As String
    Private ctrlYourObjectHereValues(maxInputs) As String

    Private Const controlPrefix As String = "ctlPureAdd<YourObjectNameHere>"
    Private Const labelPrefix As String = "lblPureAdd<YourObjectNameHere>"
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
            For i As Integer = 0 To ctrlYourObjectHere.GetUpperBound(0)
                ruleLineObj.Var3 = ruleLineObj.Var3 + GetPostedControlValue(ctrlYourObjectHere(i).Control)
                ruleLineObj.Var3 = ruleLineObj.Var3 + "~"
            Next
            ruleLineObj.Var3 = Strings.Left(ruleLineObj.Var3, ruleLineObj.Var3.Length - 1)

            'SumInsured
            'ruleLineObj.Parameters = GetPostedControlValue(ctrlSumInsured.Control)
            ruleLineObj.Parameters = ""
            For i As Integer = 0 To ctrlYourObjectHereNames.GetUpperBound(0)
                ruleLineObj.Parameters = ruleLineObj.Parameters + ctrlYourObjectHereNames(i).ToString
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
            'If ctrlYourObjectHereOriginal IsNot Nothing Then
            '    controls.Add(ctrlYourObjectHereOriginal.Control)
            'End If
            If ctrlYourObjectHere.Length > 0 Then
                For i As Integer = 0 To ctrlYourObjectHere.GetUpperBound(0)
                    controls.Add(ctrlYourObjectHere(i).Control)
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
            'this list is tied to the properties of the object in the input screen
            'every field you need to see on the screen needs to be in this list
            'and the control array is size limited by the maxInputs variable
            ctrlYourObjectHereNames(0) = "unique_id"
            ctrlYourObjectHereNames(1) = "output_id"
            ctrlYourObjectHereNames(2) = "property_name_here"
            ctrlYourObjectHereNames(3) = "property_name_here"
            ctrlYourObjectHereNames(4) = "property_name_here"
            ctrlYourObjectHereNames(5) = "property_name_here"
            ctrlYourObjectHereNames(6) = "property_name_here"
            ctrlYourObjectHereNames(7) = "property_name_here"
            ctrlYourObjectHereNames(8) = "property_name_here"
            ctrlYourObjectHereNames(9) = "property_name_here"
            ctrlYourObjectHereNames(10) = "property_name_here"
            ctrlYourObjectHereNames(11) = "property_name_here"
            ctrlYourObjectHereNames(12) = "property_name_here"
            ctrlYourObjectHereNames(13) = "property_name_here"
            ctrlYourObjectHereNames(14) = "property_name_here"
            ctrlYourObjectHereNames(15) = "property_name_here"
            ctrlYourObjectHereNames(16) = "property_name_here"
            ctrlYourObjectHereNames(17) = "property_name_here"
            ctrlYourObjectHereNames(18) = "property_name_here"
            ctrlYourObjectHereNames(19) = "property_name_here"
            ctrlYourObjectHereNames(20) = "property_name_here"
            ctrlYourObjectHereNames(21) = "property_name_here"
            ctrlYourObjectHereNames(22) = "property_name_here"
            ctrlYourObjectHereNames(23) = "property_name_here"
            ctrlYourObjectHereNames(24) = "property_name_here"
            ctrlYourObjectHereNames(25) = "property_name_here"
            ctrlYourObjectHereNames(26) = "property_name_here"
            ctrlYourObjectHereNames(27) = "property_name_here"
            ctrlYourObjectHereNames(28) = "property_name_here"

            If ruleLineObj IsNot Nothing Then
                ratingSection = ruleLineObj.Var1
                premium = ruleLineObj.Var2
                'rate = ruleLineObj.Var3

                tempValArr = Strings.Split(ruleLineObj.Var3, "~", ctrlYourObjectHere.GetUpperBound(0) + 1)

                If tempValArr.GetUpperBound(0) < ctrlYourObjectHere.GetUpperBound(0) Then
                    ReDim ctrlYourObjectHereValues(maxInputs)
                Else
                    ctrlYourObjectHereValues = Strings.Split(ruleLineObj.Var3, "~", ctrlYourObjectHere.GetUpperBound(0) + 1)
                End If

                'For i As Integer = 0 To ctrlYourObjectHere.GetUpperBound(0)
                '    'need to split the var 3 out and into ctrlYourObjectHereValues
                'Next

                sumInsured = ruleLineObj.Parameters
                dataModel = ruleLineObj.MathOperator
            End If

            'ctrlRatingSection = AddLabelControlPair(ratingSectionControlName, "Rating Section", ratingSection, Nothing, Nothing)
            ctrlPremium = AddLabelControlPair(premiumControlName, "Premium", premium, Nothing, Nothing)
            'ctrlRate = AddLabelControlPair(rateControlName, "Rate", rate, Nothing, Nothing)
            'ctrlSumInsured = AddLabelControlPair(sumInsuredControlName, "Sum Insured", sumInsured, Nothing, Nothing)

            For i As Integer = 0 To ctrlYourObjectHereNames.GetUpperBound(0)
                ctrlYourObjectHere(i) = AddLabelControlPair(ctrlYourObjectHereNames(i), ctrlYourObjectHereNames(i), ctrlYourObjectHereValues(i), Nothing, Nothing)
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
        For i As Integer = 0 To ctrlYourObjectHere.GetUpperBound(0)
            RuleLineEditorHelper.SetControlValue(ctrlYourObjectHere(i).Control, "")
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
