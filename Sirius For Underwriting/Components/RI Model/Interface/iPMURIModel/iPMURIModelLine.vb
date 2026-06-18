Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Reflection
Imports System.Collections.Generic


Partial Friend Class frmRIModelLine
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmRIModelLine"
    Private Const ACApp As String = "iPMURIModel"


    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public Status As gPMConstants.PMEReturnCode
    Public m_iConfigStatusTask As gPMConstants.PMEComponentAction

    ' Declare an instance of the Business object.
    Public Business As Object
    Private m_oBusinessObject As Object
    Private m_lTreatyID As Integer
    Private m_lVariableQuotaShareID As Integer
    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Store current priority allocations
    ' Store current priority allocations
    Private m_oPriorities As PriorityCollection
    Private m_oPriority As Priority

    Private m_sUnderwritingType As String = ""
    Private m_bIsRI2007Enabled As Boolean
    Private m_bIsMultipleRetained As Boolean
    Private m_iTask As gPMConstants.PMEComponentAction

    Private m_vRIModelLines(,) As Object
    Private m_lSelectedIndex As Integer
    Private m_vTreatyVariableQuotaShare(,) As Object = Nothing
    Private m_bDoIncrementPriorities As Boolean
    Private m_bIsObligatory As Boolean
    Private m_iTreatyPremiumType As Integer = 0

    Private m_doriginalShare As Double
    Private m_bIsVariableQuotaShare As Boolean = False
    Private m_tabVariableQuotaShare As System.Windows.Forms.TabPage = Nothing
    Private m_bHasVariableQuotaShareTab As Boolean = False
    Private m_bTreatyLineTabDisabled As Boolean = False
    Private m_bAllowTabSwitch As Boolean = False

    Private m_dRawLines As Decimal = 0

    Private Const vbFormCode As Integer = 0
    Public WriteOnly Property RIModelLines() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vRIModelLines = Value
        End Set
    End Property

    Public Property RIModelLinesVariableQuotaShare() As Object(,)
        Get
            Return m_vTreatyVariableQuotaShare
        End Get
        Set(ByVal Value(,) As Object)
            m_vTreatyVariableQuotaShare = Value
        End Set
    End Property

    Public WriteOnly Property SelectedIndex() As Integer
        Set(ByVal Value As Integer)
            m_lSelectedIndex = Value
        End Set
    End Property

    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property TreatyPremiumType() As Integer
        Set(ByVal Value As Integer)
            m_iTreatyPremiumType = Value
            ' Show/Hide Premium Calculation Basis based on Treaty Premium Type only for RI2007
            If Value = TreatyPremiumTypeEnum.VariableCessionOrder Then
                cboPremiumCalculationBasis.Visible = True
                lblPremiumCalculationBasis.Visible = True
            Else
                cboPremiumCalculationBasis.Visible = False
                lblPremiumCalculationBasis.Visible = False
            End If
        End Set
    End Property
    Public ReadOnly Property GetUpdatedRIModelLines() As Object(,)
        Get
            Return VB6.CopyArray(m_vRIModelLines)
        End Get
    End Property
    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '
    Public Function Clear(ByVal vPriorities As PriorityCollection) As Integer

        Dim result As Integer = 0
        Dim oPriority As Priority

        Dim lReturn As Integer
        Const kMethodName As String = "Clear"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store priorities and set new fields based on existing data
            m_oPriorities = vPriorities

            ' Get next available priority
            oPriority = m_oPriorities.NextAvailable

            ' Set base values
            If txtPriority.Text.Trim() = "0" Or txtPriority.Text.Trim() = "" Then
                m_oFormFields.FormatControl(ctlControl:=txtPriority, vControlValue:=oPriority.Priority)
            End If

            m_oFormFields.FormatControl(ctlControl:=txtSharePercent, vControlValue:=100 - oPriority.Share)

            ' Populate lines, limit and summary automatically
            RefreshLineValues()
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
        End Try
        Return result
    End Function
    'Note:- Two optional parameters(bIsObligatory,bDoIncrementPriority) are added
    Public Function GetProperties(ByRef lPriority As Integer, ByRef lLines As Decimal, ByRef cLineLimit As Decimal, ByRef lTreatyID As Integer, ByRef sTreatyName As String, ByRef dShare As Double, Optional ByRef cLowerLimit As Decimal = 0, Optional ByRef lTreatyTypeID As Integer = 0, Optional ByRef dCedingrate As Double = 0, Optional ByRef lReinsuranceTypeID As Integer = 0, Optional ByRef bIsObligatory As Boolean = False, Optional ByRef bDoIncrementPriority As Boolean = False, Optional ByRef iCedePremiumOnly As Integer = 0, Optional ByRef iCalculationOrder As Integer = 0, Optional ByRef iCalculationBase As Integer = 0, Optional ByRef iIsVariableQuotaShare As Integer = 0, Optional ByRef iPremiumCalculationBasis As Integer = 0, Optional ByRef m_vTreatyVariableQuotaShare(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "GetProperties"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Return all detail data

            lPriority = CInt(m_oFormFields.UnformatControl(ctlControl:=txtPriority))

            ' Surplus lines decimals - use m_dRawLines to preserve decimal before LostFocus reformats txtLines
            lLines = If(m_dRawLines > 0, m_dRawLines, CDec(gPMFunctions.ToSafeDouble(txtLines.Text)))

            cLineLimit = CDec(m_oFormFields.UnformatControl(ctlControl:=txtLineLimit))
            lTreatyID = cboTreaty.ItemId
            m_lTreatyID = lTreatyID
            sTreatyName = cboTreaty.ItemCaption
            bIsObligatory = chkObligatory.CheckState <> CheckState.Unchecked
            bDoIncrementPriority = m_bDoIncrementPriorities
            dShare = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtSharePercent))
            If m_bIsRI2007Enabled Then

                cLowerLimit = CDec(m_oFormFields.UnformatControl(ctlControl:=txtlowerlimit))

                dCedingrate = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtcedingrate))
                lTreatyTypeID = cboTreatyType.ItemId

                lReinsuranceTypeID = cboRIType.ItemId
                If chkCedePremiumOnly.Checked Then
                    If Trim(cboTreatyType.ItemCode) = "XOL" _
AndAlso (Trim(cboRIType.ItemCode) = "XOL" Or Trim(cboRIType.ItemCode) = "CAT") Then
                        iCedePremiumOnly = 1
                    Else
                        iCedePremiumOnly = 0
                    End If
                Else
                    iCedePremiumOnly = 0
                End If
                If chkVariableQuotaShare.Checked Then
                    If (Trim(cboRIType.ItemCode) = "QUO") Then
                        iIsVariableQuotaShare = 1
                    Else
                        iIsVariableQuotaShare = 0
                    End If
                Else
                    iIsVariableQuotaShare = 0
                End If

                ' Store Variable Quota Share configuration if checkbox is checked
                If iIsVariableQuotaShare AndAlso m_bHasVariableQuotaShareTab AndAlso m_tabVariableQuotaShare IsNot Nothing Then
                    Dim lvwVariableQS As DataGridView = Nothing
                    For Each ctrl As Control In m_tabVariableQuotaShare.Controls
                        If TypeOf ctrl Is DataGridView AndAlso ctrl.Name = "lvwVariableQS" Then
                            lvwVariableQS = DirectCast(ctrl, DataGridView)
                            Exit For
                        End If
                    Next

                    If lvwVariableQS IsNot Nothing AndAlso lvwVariableQS.Rows.Count > 0 Then
                        Dim riModelLineID As Integer = GetRIModelLineID()
                        Dim gridRowCount As Integer = lvwVariableQS.Rows.Count

                        ' Count existing rows for other RIModelLineIDs
                        Dim otherRowCount As Integer = 0
                        If Information.IsArray(m_vTreatyVariableQuotaShare) Then
                            For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                                If gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i)) <> riModelLineID Then
                                    otherRowCount += 1
                                End If
                            Next
                        End If

                        ' Create new array with combined data
                        Dim totalRows As Integer = otherRowCount + gridRowCount - 1
                        Dim newArray(DBMVMax, totalRows) As Object
                        Dim newIndex As Integer = 0

                        ' Copy data from other RIModelLineIDs
                        If Information.IsArray(m_vTreatyVariableQuotaShare) Then
                            For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                                If gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i)) <> cboTreaty.ItemId Then
                                    For col As Integer = 0 To DBMVMax
                                        newArray(col, newIndex) = m_vTreatyVariableQuotaShare(col, i)
                                    Next
                                    newIndex += 1
                                End If
                            Next
                        End If

                        ' Add current grid data
                        For i As Integer = 0 To gridRowCount - 1
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLVariableQuotaShareId, newIndex) = lvwVariableQS.Rows(i).Cells("VariableQuotaShareId").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLSALowerLimit, newIndex) = lvwVariableQS.Rows(i).Cells("SALowerLimit").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLSAUpperLimit, newIndex) = lvwVariableQS.Rows(i).Cells("SAUpperLimit").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLSharePercent, newIndex) = lvwVariableQS.Rows(i).Cells("SharePercent").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyLimit, newIndex) = lvwVariableQS.Rows(i).Cells("TreatyLimit").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, newIndex) = cboTreaty.ItemId
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, newIndex) = riModelLineID
                            newIndex += 1
                        Next

                        m_vTreatyVariableQuotaShare = newArray
                        ' arrVariableQuotaShare = m_vTreatyVariableQuotaShare
                    End If
                End If
            Else
                lTreatyTypeID = cboTreatyType.ItemId
                lReinsuranceTypeID = cboRIType.ItemId
                'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
            End If
            ' Get Premium Calculation Basis for all treaty types
            iPremiumCalculationBasis = cboPremiumCalculationBasis.ItemId
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
        End Try
        Return result
    End Function

    'Note:- one optional parameter(bIsObligatory, cLineLimit) are added
    Public Function SetProperties(ByVal lPriority As Integer, ByVal lTreatyID As Integer, ByVal dShare As Double, ByVal vPriorities As PriorityCollection, Optional ByRef lTreatyTypeID As Integer = -1, Optional ByVal cLowerLimit As Decimal = 0, Optional ByVal bIsObligatory As Boolean = False, Optional ByVal cLineLimit As Decimal = 0, Optional ByVal iCedePremiumOnly As Integer = 0, Optional ByVal dCedePercentage As Double = 0, Optional ByVal iIsVariableQuotaShare As Integer = 0, Optional ByVal iPremiumCalculationBasis As Integer = 0, Optional ByVal m_vTreatyVariableQuotaShare(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SetProperties"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store priorities
            m_oPriorities = vPriorities

            ' Set all detail data
            m_oFormFields.FormatControl(txtPriority, lPriority)
            cboTreaty.ItemId = lTreatyID

            m_doriginalShare = dShare
            m_oFormFields.FormatControl(txtSharePercent, dShare)

            If bIsObligatory Then
                chkObligatory.CheckState = CheckState.Checked
                Me.m_bIsObligatory = True
            Else
                chkObligatory.CheckState = CheckState.Unchecked
                Me.m_bIsObligatory = False
            End If

            Me.m_bIsVariableQuotaShare = iIsVariableQuotaShare
            If m_bIsRI2007Enabled Then
                If iIsVariableQuotaShare = 1 AndAlso cboRIType.ItemCode.Trim() = "QUO" AndAlso cboTreatyType.ItemCode.Trim() = "PROP" Then
                    chkVariableQuotaShare.CheckState = CheckState.Checked
                    AddVariableQuotaShareTab()
                    tabControl.SelectedTab = tabTreatyLine
                Else
                    chkVariableQuotaShare.CheckState = CheckState.Unchecked
                    RemoveVariableQuotaShareTab()
                End If
            End If
            If lTreatyTypeID > -1 Then cboTreatyType.ItemId = lTreatyTypeID
            If Not m_bIsRI2007Enabled Then

                If cLowerLimit <> 0 Then m_oFormFields.FormatControl(txtLineLimit, cLowerLimit)
            Else
                If cLowerLimit <> 0 Then m_oFormFields.FormatControl(txtlowerlimit, cLowerLimit)
                If cLineLimit <> 0 Then m_oFormFields.FormatControl(txtLineLimit, cLineLimit)
            End If
            If dCedePercentage <> 0 Then m_oFormFields.FormatControl(txtcedingrate, dCedePercentage)
            If iCedePremiumOnly = 1 Then chkCedePremiumOnly.Checked = True 'E005 Part1 Santosh Singh
            If iIsVariableQuotaShare = 1 Then chkVariableQuotaShare.Checked = True
            ' Set Premium Calculation Basis for all treaty types
            If iPremiumCalculationBasis <> 0 Then
                ' Use the passed parameter value
                cboPremiumCalculationBasis.ItemId = iPremiumCalculationBasis
            End If

            ' Populate lines, limit and summary automatically
            Dim isSurplusLoad As Boolean = cboRIType.ItemId = ACSurplus OrElse
                                           cboRIType.ItemId = ACSecondSurplus OrElse
                                           cboRIType.ItemId = ACThirdSurplus
            ' SetProperties receives the stored lines value directly - use it to seed m_dRawLines
            ' so GetProperties fallback has the correct decimal even before the user edits the field
            Dim rawLines As Decimal = CDec(gPMFunctions.ToSafeDouble(txtLines.Text))
            m_dRawLines = If(isSurplusLoad AndAlso rawLines > 0, Math.Round(rawLines, 2), rawLines)
            RefreshLineValues()
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '
    Private Function RefreshLineValues() As Integer

        Dim result As Integer = 0
        Dim dShare As Double
        Dim cLimit As Decimal
        Dim dCeding As Double
        Dim cLowerLimit As Decimal
        Dim bIsObligatory As Boolean

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "RefreshLineValues"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the ri line has changed
            If gPMFunctions.ToSafeInteger(txtPriority.Text) <> gPMFunctions.ToSafeInteger(Convert.ToString(txtPriority.Tag)) Or Convert.ToString(txtPriority.Tag) = "" Then
                ' Get the new priority
                m_oPriority = m_oPriorities.Item(gPMFunctions.ToSafeInteger(txtPriority.Text))

                ' Refresh the number of lines and line limit
                lReturn = m_oFormFields.FormatControl(txtLines, m_oPriority.Lines)

                If m_bIsRI2007Enabled Then

                    If m_oPriority.LowerLimit <> 0 Then
                        lReturn = m_oFormFields.FormatControl(txtlowerlimit, m_oPriority.LowerLimit)
                    End If
                    If txtLineLimit.Text = "" Then
                        txtLineLimit.Text = (0).ToString("N2")
                    End If
                    If m_oPriority.LineLimit <> 0 Then
                        lReturn = m_oFormFields.FormatControl(txtLineLimit, m_oPriority.LineLimit)
                    End If
                    lReturn = m_oFormFields.FormatControl(txtcedingrate, m_oPriority.Ceding)
                End If
                ' Update current item tag
                txtPriority.Tag = CStr(gPMFunctions.ToSafeInteger(txtPriority.Text))
            End If

            ' Update total allocated
            If m_bIsRI2007Enabled And m_oPriority.Share > 0 Then
                dShare = m_oPriority.Share
            Else
                dShare = m_oPriority.Share + gPMFunctions.ToSafeDouble(m_oFormFields.UnformatControl(txtSharePercent))
            End If
            txtAllocatedPercent.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, dShare, -5)

            ' Update effective treaty limit
            If cboTreatyType.ItemCode.Trim() = "XOL" Then
                cLimit = (m_oPriority.LineLimit - m_oPriority.LowerLimit)
            Else
                Dim dLines As Double = gPMFunctions.ToSafeDouble(txtLines.Text)
                Dim dLineLimit As Double = gPMFunctions.ToSafeCurrency(txtLineLimit.Text)
                Dim dSharePct As Decimal = CDec(gPMFunctions.ToSafeDouble(m_oFormFields.UnformatControl(txtSharePercent)))
                cLimit = dLines * dLineLimit * (dSharePct / 100)
            End If

            If chkCedePremiumOnly.Checked Then
                cLimit = 0
            End If
            txtTreatyLimit.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cLimit)
            If m_bIsRI2007Enabled Then
                If txtcedingrate.Text.Trim().Length > 0 Then
                    dCeding = gPMFunctions.ToSafeDouble(m_oFormFields.UnformatControl(txtcedingrate))
                    txtcedingrate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercentFourDecimal, dCeding, -5)
                    m_oPriority.Ceding = dCeding
                End If
                cLowerLimit = gPMFunctions.ToSafeCurrency(txtlowerlimit.Text)
                txtlowerlimit.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeDouble(cLowerLimit))
                m_oPriority.LowerLimit = cLowerLimit
            End If

            If chkCedePremiumOnly.Checked AndAlso chkCedePremiumOnly.Enabled Then
                txtTreatyLimit.Text = 0
            End If

            ' Update Variable Quota Share state after refreshing line values
            UpdateVariableQuotaShareState()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
        End Try
        Return result
    End Function
    Private Function SetFieldValidation() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetFieldValidation"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()
            m_oFormFields.LanguageID = g_iLanguageID

            ' Add controls
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPriority, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtPriority")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLineLimit, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtLineLimit")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTreaty, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add cboTreaty")
            End If
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboRIType, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add cboRIType")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSharePercent, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=-5)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtSharePercent")
            End If
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPremiumCalculationBasis, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add cboPremiumCalculationBasis")
            End If
            If m_bIsRI2007Enabled Then
                lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtcedingrate, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=-5)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtcedingrate")
                End If
                lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLines, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtLines")
                End If
                lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtlowerlimit, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtLines")
                End If
            Else
                lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLines, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtLines")
                End If
            End If

            UpdateLinesFieldFormat()

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function
    Private Function Validate_Renamed() As Integer

        Dim result As Integer = 0
        Dim dShare As Double
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim dCeding As Double
        Dim bIsRetainedReinsurer As Boolean
        Dim bQSLineObligatoryExist As Boolean
        Dim lPriorityCount As Integer
        Const kMethodName As String = "Validate"
        Try
            bQSLineObligatoryExist = False
            ' Default to false, only set true if we get to the end
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Standard validation
            lReturn = m_oFormFields.CheckMandatoryControls()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' #37147 Surplus Lines in Decimals — validate number of lines
            If txtLines.Enabled AndAlso txtLines.Visible Then
                Dim dLines As Decimal = CDec(m_oFormFields.UnformatControl(ctlControl:=txtLines))
                Dim isSurplus As Boolean = cboRIType.ItemId = ACSurplus OrElse
                                           cboRIType.ItemId = ACSecondSurplus OrElse
                                           cboRIType.ItemId = ACThirdSurplus
                If dLines <= 0 Then
                    MessageBox.Show("Number of lines must be a positive value" & If(isSurplus, " with up to 2 decimal places.", "."), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtLines.Focus()
                    Return result
                End If
                If isSurplus Then
                    Dim sLines As String = dLines.ToString()
                    Dim dotPos As Integer = sLines.IndexOf("."c)
                    If dotPos >= 0 AndAlso sLines.Length - dotPos - 1 > 2 Then
                        MessageBox.Show("Number of lines must be a positive value with up to 2 decimal places.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtLines.Focus()
                        Return result
                    End If
                End If
            End If

            ' Check share is valid

            dShare = CDbl(m_oFormFields.UnformatControl(txtSharePercent))
            If (dShare <= 0) Or (dShare > 100) Then
                If m_bIsRI2007Enabled Then
                    MessageBox.Show("Allocated % must be in range of 0.01% and 100.00% when Reinsurance Type is Quota Share", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtSharePercent.Focus()
                    Return result
                Else
                    MessageBox.Show("Share percentage must be greater than zero and less that or equal to 100%", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtSharePercent.Focus()
                    Return result
                End If
            End If
            If m_bIsRI2007Enabled Then
                If cboRIType.ItemId = ACQuotaShare Then
                    If cboTreatyType.ItemId <> ACProportional Then
                        MessageBox.Show("Treaty Type must be Proportional when Reinsurance Type is Quota Share", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return result
                    End If

                End If
                If cboRIType.ItemId = ACExcessofLoss Then
                    If cboTreatyType.ItemId <> ACtreatyExcessofLoss Then
                        MessageBox.Show("Treaty Type must be Excess Of Loss when Reinsurance Type is Excess Of Loss", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return result
                    End If
                End If

                If cboRIType.ItemId = ACRetained Then
                    If cboTreatyType.ItemId <> ACProportional Then
                        MessageBox.Show("Treaty Type must be Proportional when Reinsurance Type is Retained", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return result
                    End If
                End If

                If cboRIType.ItemId = ACProportionalXOL Then
                    If cboTreatyType.ItemId <> ACtreatyExcessofLoss Then
                        MessageBox.Show("Treaty Type must be Excess Of Loss when Reinsurance Type is Proportional XOL", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return result
                    End If
                End If

                If cboRIType.ItemId = ACQuotaShareRetained Then
                    If cboTreatyType.ItemId <> ACProportional Then
                        MessageBox.Show("Treaty Type must be Proportional when Reinsurance Type is Quota Share Retained", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return result
                    End If
                    ' Validate that Retained treaty is configured before allowing Quota Share Retained
                    Dim bRetainedConfigured As Boolean = False
                    If Information.IsArray(m_vRIModelLines) Then
                        For lIdx As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                            If gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIdx)) = ACRetained Then
                                bRetainedConfigured = True
                                Exit For
                            End If
                        Next
                    End If
                    If Not bRetainedConfigured Then
                        MessageBox.Show("Please configure the retained treaty before configuring the Quota Share Retained Treaty", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        cboTreaty.Focus()
                        Return result
                    End If
                End If

                If Information.IsArray(m_vRIModelLines) Then

                    For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                        ' Check for matching priority
                        Select Case m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount)
                            Case ACQuotaShare
                                '1
                                If chkObligatory.CheckState = CheckState.Unchecked Then
                                Else
                                    If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)) = gPMFunctions.ToSafeLong(txtPriority.Text) And cboRIType.ItemId = ACQuotaShare Then
                                        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                                            MessageBox.Show("This treaty would be made priority 1", "Treaty Priority", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        End If
                                        'Note:- This GoTo Finally is commented since the priority will be taken care by code
                                        'if  obligatory Quota share is changed to Noraml Quota Share as told by Sucheta
                                        bQSLineObligatoryExist = True
                                        Me.Hide()
                                    End If
                                End If
                            Case ACExcessofLoss

                            Case ACRetained
                                '7
                                m_bIsMultipleRetained = True

                        End Select
                    Next
                End If
            End If
            If m_bIsRI2007Enabled Then
                If cboRIType.ItemCode.Trim() = "QUO" AndAlso Not bQSLineObligatoryExist AndAlso chkObligatory.Checked Then
                    If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                        MessageBox.Show("This treaty would be made priority 1", "Treaty Priority", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                    Me.Hide()
                End If

            End If

            If m_bIsRI2007Enabled And cboTreatyType.ItemCode.Trim() = "XOL" Then
                'check ceding is valid

                dCeding = CDbl(m_oFormFields.UnformatControl(txtcedingrate))
                If (dCeding < 0) Or (dCeding > 100) Then
                    MessageBox.Show("Ceding Rate must be between 0% and 100%", "Invalid Ceding Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtcedingrate.Focus()
                    Return result
                End If

                If gPMFunctions.ToSafeCurrency(txtlowerlimit.Text) > 0.01 Then
                    If gPMFunctions.ToSafeCurrency(txtLineLimit.Text) <= gPMFunctions.ToSafeCurrency(txtlowerlimit.Text) Then
                        MessageBox.Show("The Line/Upper Limit must be greater than Lower Limit", "Invalid Lower Limit", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtlowerlimit.Focus()
                        Return result
                    End If
                Else
                    MessageBox.Show("The Lower Limit should be atleast 0.01 or more", "Invalid Lower Limit", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                End If

                lReturn = Business.CheckRetainedReinsurer(cboTreaty.ItemId, bIsRetainedReinsurer)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                End If

                If bIsRetainedReinsurer Then
                    MessageBox.Show("One or more XOL treaty contains a Retained party. Edit the treaties to correct.", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                End If

            End If

            If chkCedePremiumOnly.Checked Then
                If Trim(cboTreatyType.ItemCode) = "XOL" _
                AndAlso (Trim(cboRIType.ItemCode) = "XOL" Or Trim(cboRIType.ItemCode) = "CAT") Then
                    txtTreatyLimit.Text = 0
                Else
                    chkCedePremiumOnly.Checked = False
                End If
            End If
            ' Validate Premium Calculation Basis
            Dim iPremiumCalculationBasis As Integer = cboPremiumCalculationBasis.ItemId
            Dim iReinsuranceTypeID As Integer = cboRIType.ItemId

            If m_iTreatyPremiumType = TreatyPremiumTypeEnum.VariableCessionOrder AndAlso m_bIsRI2007Enabled AndAlso chkObligatory.CheckState <> CheckState.Checked Then
                If iPremiumCalculationBasis > 0 Then
                    If ValidateSameReinsuranceTypeConsistency(iReinsuranceTypeID, iPremiumCalculationBasis) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If
                End If
            End If
            If m_bIsRI2007Enabled And cboTreatyType.ItemCode.Trim() = "PROP" And cboRIType.ItemCode.Trim() = "QUO" Then
                If chkVariableQuotaShare.Checked Then
                    AddVariableQuotaShareTab()
                Else
                    RemoveVariableQuotaShareTab()
                End If
            Else
                RemoveVariableQuotaShareTab()
            End If
            ' All validation passed return True
            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Sub UpdateLinesFieldFormat()
        Dim isSurplus As Boolean = cboRIType.ItemId = ACSurplus OrElse
                                   cboRIType.ItemId = ACSecondSurplus OrElse
                                   cboRIType.ItemId = ACThirdSurplus
        If Not isSurplus Then
            ' Strip any decimal portion from current value for non-surplus types
            Dim dLines As Double = gPMFunctions.ToSafeDouble(txtLines.Text)
            If dLines > 0 Then txtLines.Text = CInt(Math.Floor(dLines)).ToString()
        End If
    End Sub

    Private Sub cboRIType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRIType.Click
        If cboRIType.ItemCode.Trim() = "QUO" Then
            chkObligatory.Enabled = True
        Else
            chkObligatory.Enabled = False
            chkObligatory.CheckState = CheckState.Unchecked
        End If
        UpdateLinesFieldFormat()
        ' Filter Premium Calculation Basis based on Reinsurance Type
        If cboRIType.ItemId > 0 Then
            If IsSurplusOrQuotaShare(cboRIType.ItemId) Then
                cboPremiumCalculationBasis.WhereClause = "reinsurance_type_id IS NULL"
            Else
                cboPremiumCalculationBasis.WhereClause = "reinsurance_type_id = " & cboRIType.ItemId
            End If
            cboPremiumCalculationBasis.RefreshList()
            ' Default Premium Calculation Basis to existing treaties of same type
            If (m_iTreatyPremiumType = TreatyPremiumTypeEnum.VariableCessionOrder OrElse cboPremiumCalculationBasis.Visible) Then
                Dim existingBasis As Integer = GetExistingPremiumCalculationBasisForSameType(cboRIType.ItemId)
                If existingBasis > 0 Then
                    cboPremiumCalculationBasis.ItemId = existingBasis
                End If
            End If
        End If
        ' Enable Variable Quota Share checkbox only for Quota Share reinsurance type
        UpdateVariableQuotaShareState()
    End Sub

    ''' <summary>
    ''' Updates the Variable Quota Share state based on current selections
    ''' </summary>
    Private Sub UpdateVariableQuotaShareState()
        Try
            If chkVariableQuotaShare IsNot Nothing AndAlso m_oFormFields IsNot Nothing Then
                Dim isQuotaShare As Boolean = cboRIType.ItemCode.Trim() = "QUO" AndAlso cboTreatyType.ItemCode.Trim() = "PROP"
                Dim sharePercent As Double = gPMFunctions.ToSafeDouble(m_oFormFields.UnformatControl(txtSharePercent))
                Dim treatyLimit As Double = gPMFunctions.ToSafeDouble(txtTreatyLimit.Text)

                chkVariableQuotaShare.Enabled = isQuotaShare AndAlso sharePercent > 0 AndAlso treatyLimit > 0

                If Not isQuotaShare Then
                    chkVariableQuotaShare.Visible = False
                    chkVariableQuotaShare.Checked = False
                    RemoveVariableQuotaShareTab()
                End If
            End If
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Adds the Variable Quota Share tab with ListView control
    ''' </summary>
    Private Sub AddVariableQuotaShareTab()
        Try
            If Not m_bHasVariableQuotaShareTab Then
                ' Create new tab page
                m_tabVariableQuotaShare = New System.Windows.Forms.TabPage()
                m_tabVariableQuotaShare.Text = "Variable Quota Share"
                m_tabVariableQuotaShare.UseVisualStyleBackColor = True

                ' Title label
                Dim lblTitle As New System.Windows.Forms.Label()
                lblTitle.Text = "Variable Quota Share Configuration"
                lblTitle.Location = New System.Drawing.Point(20, 15)
                lblTitle.Size = New System.Drawing.Size(300, 20)
                lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold)

                ' Binding Authority label and textbox
                Dim lblBindingAuth As New System.Windows.Forms.Label()
                lblBindingAuth.Text = "Binding Authority"
                lblBindingAuth.Location = New System.Drawing.Point(26, 50)
                lblBindingAuth.Size = New System.Drawing.Size(120, 20)

                Dim txtBindingAuth As New System.Windows.Forms.TextBox()
                txtBindingAuth.Location = New System.Drawing.Point(155, 47)
                txtBindingAuth.Size = New System.Drawing.Size(400, 23)
                txtBindingAuth.Text = cboTreaty.ItemCaption
                txtBindingAuth.Enabled = False
                txtBindingAuth.Name = "txtBindingAuth"

                ' Upper Limit label and textbox
                Dim lblUpperLimit As New System.Windows.Forms.Label()
                lblUpperLimit.Text = "Upper Limit"
                lblUpperLimit.Location = New System.Drawing.Point(26, 80)
                lblUpperLimit.Size = New System.Drawing.Size(120, 20)

                Dim txtUpperLimit As New System.Windows.Forms.TextBox()
                txtUpperLimit.Location = New System.Drawing.Point(155, 77)
                txtUpperLimit.Size = New System.Drawing.Size(150, 23)
                txtUpperLimit.Text = If(String.IsNullOrEmpty(txtLineLimit.Text), "0", txtLineLimit.Text)
                txtUpperLimit.Enabled = False
                txtUpperLimit.Name = "txtUpperLimit"

                ' No. Of. Lines label and textbox
                Dim lblNoOfLines As New System.Windows.Forms.Label()
                lblNoOfLines.Text = "No. Of Lines"
                lblNoOfLines.Location = New System.Drawing.Point(320, 80)
                lblNoOfLines.Size = New System.Drawing.Size(100, 20)

                Dim txtNoOfLines As New System.Windows.Forms.TextBox()
                txtNoOfLines.Location = New System.Drawing.Point(425, 77)
                txtNoOfLines.Size = New System.Drawing.Size(130, 23)
                txtNoOfLines.Text = If(String.IsNullOrEmpty(txtLines.Text), "1", txtLines.Text)
                txtNoOfLines.Enabled = False
                txtNoOfLines.Name = "txtNoOfLines"

                ' DataGridView for Variable Quota Share data
                Dim lvwVariableQS As New System.Windows.Forms.DataGridView()
                lvwVariableQS.Location = New System.Drawing.Point(26, 120)
                lvwVariableQS.Size = New System.Drawing.Size(600, 120)
                lvwVariableQS.Name = "lvwVariableQS"
                lvwVariableQS.AllowUserToAddRows = False
                lvwVariableQS.AllowUserToDeleteRows = False
                lvwVariableQS.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
                lvwVariableQS.SelectionMode = DataGridViewSelectionMode.CellSelect
                lvwVariableQS.EditMode = DataGridViewEditMode.EditOnEnter
                AddHandler lvwVariableQS.SelectionChanged, AddressOf lvwVariableQS_SelectionChanged
                AddHandler lvwVariableQS.CellClick, AddressOf lvwVariableQS_CellClick
                AddHandler lvwVariableQS.CellValueChanged, AddressOf lvwVariableQS_CellValueChanged
                AddHandler lvwVariableQS.CellEndEdit, AddressOf lvwVariableQS_CellEndEdit
                AddHandler lvwVariableQS.CellValidating, AddressOf lvwVariableQS_CellValidating

                ' Add columns as DataGridViewTextBoxColumn
                Dim col1 As New DataGridViewTextBoxColumn()
                col1.Name = "SALowerLimit"
                col1.HeaderText = "SI Lower Limit"
                lvwVariableQS.Columns.Add(col1)

                Dim col2 As New DataGridViewTextBoxColumn()
                col2.Name = "SAUpperLimit"
                col2.HeaderText = "SI Upper Limit"
                lvwVariableQS.Columns.Add(col2)

                Dim col3 As New DataGridViewTextBoxColumn()
                col3.Name = "SharePercent"
                col3.HeaderText = "Share %"
                lvwVariableQS.Columns.Add(col3)

                Dim col4 As New DataGridViewTextBoxColumn()
                col4.Name = "TreatyLimit"
                col4.HeaderText = "Treaty Limit"
                col4.ReadOnly = True
                lvwVariableQS.Columns.Add(col4)

                ' Hidden column for variable_quota_share_id
                Dim colId As New DataGridViewTextBoxColumn()
                colId.Name = "VariableQuotaShareId"
                colId.Visible = False
                lvwVariableQS.Columns.Add(colId)

                ' Set column widths
                lvwVariableQS.Columns(0).Width = 150
                lvwVariableQS.Columns(1).Width = 150
                lvwVariableQS.Columns(2).Width = 100
                lvwVariableQS.Columns(3).Width = 160

                ' Buttons
                Dim btnAdd As New System.Windows.Forms.Button()
                btnAdd.Text = "ADD"
                btnAdd.Location = New System.Drawing.Point(230, 250)
                btnAdd.Size = New System.Drawing.Size(75, 23)
                btnAdd.Name = "btnAddVQS"
                AddHandler btnAdd.Click, AddressOf btnAddVQS_Click

                Dim btnDelete As New System.Windows.Forms.Button()
                btnDelete.Text = "DELETE"
                btnDelete.Location = New System.Drawing.Point(315, 250)
                btnDelete.Size = New System.Drawing.Size(75, 23)
                btnDelete.Name = "btnDeleteVQS"
                btnDelete.Enabled = False
                btnDelete.CausesValidation = False
                AddHandler btnDelete.Click, AddressOf btnDeleteVQS_Click

                Dim btnCancel As New System.Windows.Forms.Button()
                btnCancel.Text = "Cancel"
                btnCancel.Location = New System.Drawing.Point(400, 250)
                btnCancel.Size = New System.Drawing.Size(75, 23)
                btnCancel.Name = "btnCancelVQS"
                btnCancel.CausesValidation = False
                AddHandler btnCancel.Click, AddressOf btnCancelVQS_Click

                Dim btnBack As New System.Windows.Forms.Button()
                btnBack.Text = "BACK TO TREATY LINE"
                btnBack.Location = New System.Drawing.Point(485, 250)
                btnBack.Size = New System.Drawing.Size(145, 23)
                btnBack.Name = "btnBackVQS"
                btnBack.Enabled = False
                AddHandler btnBack.Click, AddressOf btnBackVQS_Click

                ' Add controls to tab
                m_tabVariableQuotaShare.Controls.Add(lblTitle)
                m_tabVariableQuotaShare.Controls.Add(lblBindingAuth)
                m_tabVariableQuotaShare.Controls.Add(txtBindingAuth)
                m_tabVariableQuotaShare.Controls.Add(lblUpperLimit)
                m_tabVariableQuotaShare.Controls.Add(txtUpperLimit)
                m_tabVariableQuotaShare.Controls.Add(lblNoOfLines)
                m_tabVariableQuotaShare.Controls.Add(txtNoOfLines)
                m_tabVariableQuotaShare.Controls.Add(lvwVariableQS)
                m_tabVariableQuotaShare.Controls.Add(btnCancel)
                m_tabVariableQuotaShare.Controls.Add(btnAdd)
                m_tabVariableQuotaShare.Controls.Add(btnDelete)
                m_tabVariableQuotaShare.Controls.Add(btnBack)

                ' Add tab to the existing tab control and make it active
                If tabControl IsNot Nothing Then
                    tabControl.TabPages.Add(m_tabVariableQuotaShare)
                    tabControl.SelectedTab = m_tabVariableQuotaShare
                    AddHandler tabControl.SelectedIndexChanged, AddressOf tabControl_SelectedIndexChanged
                End If

                m_bHasVariableQuotaShareTab = True

                ' Load existing data AFTER tab is created
                LoadVariableQuotaShareData(lvwVariableQS)
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="AddVariableQuotaShareTab", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Removes the Variable Quota Share tab
    ''' </summary>
    Private Sub RemoveVariableQuotaShareTab()
        Try
            If m_bHasVariableQuotaShareTab AndAlso m_tabVariableQuotaShare IsNot Nothing Then
                If tabControl IsNot Nothing Then
                    tabControl.TabPages.Remove(m_tabVariableQuotaShare)
                End If
                m_tabVariableQuotaShare.Dispose()
                m_tabVariableQuotaShare = Nothing
                m_bHasVariableQuotaShareTab = False
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="RemoveVariableQuotaShareTab", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for Add button in Variable Quota Share tab
    ''' </summary>
    Private Sub btnAddVQS_Click(sender As Object, e As EventArgs)
        Try
            Dim lvwVariableQS As DataGridView = Nothing
            For Each ctrl As Control In m_tabVariableQuotaShare.Controls
                If TypeOf ctrl Is DataGridView AndAlso ctrl.Name = "lvwVariableQS" Then
                    lvwVariableQS = DirectCast(ctrl, DataGridView)
                    Exit For
                End If
            Next

            If lvwVariableQS IsNot Nothing Then
                lvwVariableQS.Rows.Add("", "", "", "", Nothing)
                lvwVariableQS.CurrentCell = lvwVariableQS.Rows(lvwVariableQS.Rows.Count - 1).Cells(0)
            End If
            m_iConfigStatusTask = gPMConstants.PMEComponentAction.PMAdd
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="btnAddVQS_Click", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for Delete button in Variable Quota Share tab
    ''' </summary>
    Private Sub btnDeleteVQS_Click(sender As Object, e As EventArgs)
        Try
            Dim lvwVariableQS As DataGridView = Nothing
            For Each ctrl As Control In m_tabVariableQuotaShare.Controls
                If TypeOf ctrl Is DataGridView AndAlso ctrl.Name = "lvwVariableQS" Then
                    lvwVariableQS = DirectCast(ctrl, DataGridView)
                    Exit For
                End If
            Next

            If lvwVariableQS IsNot Nothing AndAlso lvwVariableQS.SelectedRows.Count > 0 Then
                Dim lReturn As Integer
                m_lVariableQuotaShareID = lvwVariableQS.Rows(lvwVariableQS.SelectedRows(0).Index).Cells("VariableQuotaShareId").Value
                If m_lVariableQuotaShareID <> 0 Then
                    Dim result As Integer = m_oBusinessObject.DeleteVariableQuotaShareConfigByTreaty(m_lVariableQuotaShareID)
                End If

                lvwVariableQS.Rows.RemoveAt(lvwVariableQS.SelectedRows(0).Index)
            End If
            EnableBackButton(lvwVariableQS)
            m_iConfigStatusTask = gPMConstants.PMEComponentAction.PMDelete
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="btnDeleteVQS_Click", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for DataGridView cell click - selects full row
    ''' </summary>
    Private Sub lvwVariableQS_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Try
            Dim lvwVariableQS As DataGridView = DirectCast(sender, DataGridView)
            If e.RowIndex >= 0 Then
                lvwVariableQS.Rows(e.RowIndex).Selected = True
                lvwVariableQS.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="lvwVariableQS_CellClick", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for DataGridView selection change
    ''' </summary>
    Private Sub lvwVariableQS_SelectionChanged(sender As Object, e As EventArgs)
        Try
            Dim lvwVariableQS As DataGridView = DirectCast(sender, DataGridView)
            Dim btnDelete As Button = Nothing

            For Each ctrl As Control In m_tabVariableQuotaShare.Controls
                If TypeOf ctrl Is Button AndAlso ctrl.Name = "btnDeleteVQS" Then
                    btnDelete = DirectCast(ctrl, Button)
                    Exit For
                End If
            Next

            If btnDelete IsNot Nothing Then
                btnDelete.Enabled = lvwVariableQS.SelectedRows.Count > 0
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="lvwVariableQS_SelectionChanged", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for cell value changed in DataGridView
    ''' </summary>
    Private Sub lvwVariableQS_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        Try
            If e.RowIndex < 0 Then Return
            CalculateTreatyLimit(DirectCast(sender, DataGridView), e.RowIndex)
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="lvwVariableQS_CellValueChanged", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for cell end edit in DataGridView
    ''' </summary>
    Private Sub lvwVariableQS_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        Try
            If e.RowIndex < 0 Then Return
            Dim lvwVariableQS As DataGridView = DirectCast(sender, DataGridView)
            Dim cellValue As Double
            ' Format numeric columns to 2 decimal places
            If e.ColumnIndex = lvwVariableQS.Columns("SALowerLimit").Index OrElse
               e.ColumnIndex = lvwVariableQS.Columns("SAUpperLimit").Index Then
                cellValue = gPMFunctions.ToSafeCurrency(lvwVariableQS.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
                lvwVariableQS.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = cellValue.ToString("N2")
            End If

            If e.ColumnIndex = lvwVariableQS.Columns("SharePercent").Index Then
                cellValue = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
                lvwVariableQS.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = cellValue.ToString("N2")
            End If

            CalculateTreatyLimit(lvwVariableQS, e.RowIndex)


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="lvwVariableQS_CellEndEdit", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Calculates treaty limit for a row
    ''' </summary>
    Private Sub CalculateTreatyLimit(lvwVariableQS As DataGridView, rowIndex As Integer)
        Try
            Dim saLowerLimit As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(rowIndex).Cells("SALowerLimit").Value)
            Dim saUpperLimit As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(rowIndex).Cells("SAUpperLimit").Value)
            Dim sharePercent As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(rowIndex).Cells("SharePercent").Value)
            Dim noOfLines As Double = gPMFunctions.ToSafeDouble(txtLines.Text)

            If sharePercent > 0 Then 'saLowerLimit > 0 AndAlso saUpperLimit > 0 AndAlso
                Dim treatyLimit As Double = gPMFunctions.ToSafeCurrency(txtLineLimit.Text) * noOfLines * (sharePercent / 100)
                lvwVariableQS.Rows(rowIndex).Cells("TreatyLimit").Value = treatyLimit.ToString("N2")

                EnableBackButton(lvwVariableQS)
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="CalculateTreatyLimit", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Enable Back button 
    ''' </summary>
    Private Sub EnableBackButton(lvwVariableQS As DataGridView)
        Dim btnBackVQS As Button = Nothing

        For Each ctrl As Control In m_tabVariableQuotaShare.Controls
            If TypeOf ctrl Is Button AndAlso ctrl.Name = "btnBackVQS" Then
                btnBackVQS = DirectCast(ctrl, Button)
                Exit For
            End If
        Next

        If btnBackVQS IsNot Nothing Then
            btnBackVQS.Enabled = lvwVariableQS.Rows.Count > 0
        End If
    End Sub
    ''' <summary>
    ''' Event handler for cell validation in DataGridView
    ''' </summary>
    Private Sub lvwVariableQS_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs)
        Try
            Dim lvwVariableQS As DataGridView = DirectCast(sender, DataGridView)

            ' Skip validation for hidden column
            If e.ColumnIndex = lvwVariableQS.Columns("VariableQuotaShareId").Index Then Return
            If e.ColumnIndex = lvwVariableQS.Columns("TreatyLimit").Index Then Return

            ' Validate mandatory fields
            If Not m_iConfigStatusTask = gPMConstants.PMEComponentAction.PMReverse Then
                If String.IsNullOrWhiteSpace(e.FormattedValue?.ToString()) AndAlso String.IsNullOrEmpty(e.FormattedValue?.ToString()) Then
                    MessageBox.Show("This field is mandatory.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    e.Cancel = True
                    Return
                End If

                ' Validate SA Lower Limit is less than SA Upper Limit
                If e.ColumnIndex = lvwVariableQS.Columns("SALowerLimit").Index OrElse e.ColumnIndex = lvwVariableQS.Columns("SAUpperLimit").Index Then
                    Dim saLowerLimit As Double = 0
                    Dim saUpperLimit As Double = 0

                    If e.ColumnIndex = lvwVariableQS.Columns("SALowerLimit").Index Then
                        saLowerLimit = gPMFunctions.ToSafeCurrency(e.FormattedValue)
                        saUpperLimit = gPMFunctions.ToSafeCurrency(lvwVariableQS.Rows(e.RowIndex).Cells("SAUpperLimit").Value)
                    Else
                        saLowerLimit = gPMFunctions.ToSafeCurrency(lvwVariableQS.Rows(e.RowIndex).Cells("SALowerLimit").Value)
                        saUpperLimit = gPMFunctions.ToSafeCurrency(e.FormattedValue)
                    End If
                    If saLowerLimit > 0.01 Then
                        If saLowerLimit > 0.01 AndAlso saUpperLimit > 0 AndAlso saLowerLimit >= saUpperLimit Then
                            MessageBox.Show("SA Lower Limit must be less than SA Upper Limit.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            e.Cancel = True
                        End If
                    Else
                        MessageBox.Show("The Lower Limit should be atleast 0.01 or more", "Invalid Lower Limit", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        e.Cancel = True
                    End If
                End If
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="lvwVariableQS_CellValidating", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for Is Variable Share checkbox
    ''' </summary>
    Private Sub chkVariableQuotaShare_CheckedChanged(sender As Object, e As EventArgs)
        Try
            If chkVariableQuotaShare.Checked Then
                AddVariableQuotaShareTab()
                DisableTreatyLineTab()
            Else
                RemoveVariableQuotaShareTab()
                EnableTreatyLineTab()
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="chkVariableQuotaShare_CheckedChanged", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Disables navigation to the Treaty Line tab when Variable Quota Share is active
    ''' </summary>
    Private Sub DisableTreatyLineTab()
        If Not m_bTreatyLineTabDisabled Then
            m_bTreatyLineTabDisabled = True
            AddHandler tabControl.Selecting, AddressOf tabControl_Selecting
        End If
    End Sub

    ''' <summary>
    ''' Re-enables navigation to the Treaty Line tab
    ''' </summary>
    Private Sub EnableTreatyLineTab()
        If m_bTreatyLineTabDisabled Then
            m_bTreatyLineTabDisabled = False
            RemoveHandler tabControl.Selecting, AddressOf tabControl_Selecting
        End If
    End Sub

    ''' <summary>
    ''' Prevents selecting the Treaty Line tab when it is disabled
    ''' </summary>
    Private Sub tabControl_Selecting(sender As Object, e As TabControlCancelEventArgs)
        If e.TabPage Is tabTreatyLine AndAlso m_bTreatyLineTabDisabled AndAlso Not m_bAllowTabSwitch Then
            e.Cancel = True
        End If
    End Sub

    Private Sub cboTreaty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTreaty.Click
        Dim bSIRRIModel As Object
        Dim vRITypeId(,) As Object

        Dim oBusiness As bSIRRIModel.Business
        Dim vValue As String = ""
        Const kMethodName As String = "cboTreaty_Click"

        Try

            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRRIModel.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of business object")
            End If
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form Load", "getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, gPMConstants.PMELogLevel.PMLogError)
            End If
            m_bIsRI2007Enabled = vValue = "1"

            'call the m_oBusiness.GetRITypeForTreaty
            m_lReturn = oBusiness.GetRITypeForTreaty(v_lTreatyID:=cboTreaty.ItemId, r_vRITypeId:=vRITypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(" cboRIType_Click ", " m_oBusiness.GetRITypeForTreaty Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            cboRIType.ItemId = gPMFunctions.ToSafeLong(vRITypeId(0, 0))

            ' Validate Quota Share Retained: Retained treaty must be configured first
            If cboRIType.ItemId = ACQuotaShareRetained Then
                Dim bRetainedExists As Boolean = False
                Dim dRetainedTreatyLimit As Decimal = 0
                Dim iRetainedPriority As Integer = 0
                If Information.IsArray(m_vRIModelLines) Then
                    For lIdx As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                        If gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIdx)) = ACRetained Then
                            bRetainedExists = True
                            Dim dRetLines As Decimal = CDec(gPMFunctions.ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lIdx)))
                            Dim dRetLineLimit As Decimal = CDec(gPMFunctions.ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lIdx)))
                            Dim dRetShare As Decimal = CDec(gPMFunctions.ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lIdx)))
                            dRetainedTreatyLimit = dRetLines * dRetLineLimit * (dRetShare / 100)
                            iRetainedPriority = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIdx))
                            Exit For
                        End If
                    Next
                End If

                If Not bRetainedExists Then
                    MessageBox.Show("Please configure the retained treaty before configuring the Quota Share Retained Treaty", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    cboTreaty.ItemId = 0
                    cboRIType.ItemId = 0
                    cboTreaty.Focus()
                    Exit Sub
                Else
                    ' Default Line/Upper Limit to the Treaty Limit of the Retained treaty
                    If m_oFormFields IsNot Nothing Then
                        m_oFormFields.FormatControl(txtLineLimit, dRetainedTreatyLimit)
                    Else
                        txtLineLimit.Text = dRetainedTreatyLimit.ToString("N2")
                    End If
                    If m_oPriority IsNot Nothing Then
                        m_oPriority.LineLimit = dRetainedTreatyLimit
                    End If
                    ' Set priority same as the retained treaty
                    If m_oFormFields IsNot Nothing Then
                        m_oFormFields.FormatControl(txtPriority, iRetainedPriority)
                    Else
                        txtPriority.Text = iRetainedPriority.ToString()
                    End If
                    ' Recalculate Treaty Limit after setting Line/Upper Limit
                    RefreshLineValues()
                End If
            End If

            ' Set premium calculation basis to 6 (PROPRETND) for ACQuotaShareRetained
            If cboRIType.ItemId = ACQuotaShareRetained AndAlso (m_iTreatyPremiumType = TreatyPremiumTypeEnum.VariableCessionOrder OrElse cboPremiumCalculationBasis.Visible) Then
                cboPremiumCalculationBasis.ItemId = PremiumCalculationBasisEnum.PROPRETND
            End If

            'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
            Dim bIsObligatory As Boolean
            If Information.IsArray(m_vRIModelLines) Then
                For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                    If gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Or gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) = 1 Then
                        bIsObligatory = True
                        Exit For
                    End If
                Next
            End If
            If m_bIsRI2007Enabled Then
                If (Trim(cboTreatyType.ItemCode) = "XOL" AndAlso Trim(cboRIType.ItemCode) = "XOL") Then
                    chkCedePremiumOnly.Visible = True
                    chkCedePremiumOnly.Enabled = True
                    If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                        'Do Nothing
                    ElseIf m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                        chkCedePremiumOnly.Checked = False
                    End If
                ElseIf (Trim(cboTreatyType.ItemCode) = "XOL" AndAlso Trim(cboRIType.ItemCode) = "CAT") Then
                    chkCedePremiumOnly.Visible = True
                    chkCedePremiumOnly.Enabled = False
                    chkCedePremiumOnly.Checked = True
                    chkVariableQuotaShare.Visible = False
                ElseIf cboRIType.ItemCode.Trim() = "QUO" Then
                    chkCedePremiumOnly.Visible = False
                    chkVariableQuotaShare.Visible = True
                    chkVariableQuotaShare.Enabled = False
                    If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                        'Do Nothing
                    ElseIf m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                        chkVariableQuotaShare.Checked = False
                    End If
                    If chkVariableQuotaShare IsNot Nothing Then
                        chkVariableQuotaShare = chkVariableQuotaShare
                        AddHandler chkVariableQuotaShare.CheckedChanged, AddressOf chkVariableQuotaShare_CheckedChanged
                        UpdateVariableQuotaShareState()
                        CheckAndLoadVariableQuotaShare()
                    End If
                Else
                    chkCedePremiumOnly.Visible = False
                    chkVariableQuotaShare.Visible = False
                    RemoveVariableQuotaShareTab()
                End If
            Else
                chkCedePremiumOnly.Visible = False
                chkVariableQuotaShare.Visible = False
                RemoveVariableQuotaShareTab()
            End If

            ' Update Variable Quota Share state when treaty changes
            UpdateVariableQuotaShareState()

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)
        Finally
        End Try
        Exit Sub
    End Sub
    Private Sub cboTreatyType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTreatyType.Click
        If m_bIsRI2007Enabled Then
            If cboTreatyType.ItemCode.Trim() = "XOL" Then
                lbllowerlimit.Enabled = True
                txtlowerlimit.Enabled = True
                lblcedingrate.Enabled = True
                txtcedingrate.Enabled = True
                lblLines.Enabled = False
                txtLines.Enabled = False
                chkObligatory.Enabled = False
                If m_bIsObligatory Then
                    chkObligatory.Checked = True
                End If
            Else
                lbllowerlimit.Enabled = False
                txtlowerlimit.Enabled = False
                lblcedingrate.Enabled = False
                txtcedingrate.Enabled = False
                txtcedingrate.Text = CStr(0)
                lblLines.Enabled = True
                txtLines.Enabled = True
                txtPriority.Enabled = True
                lblPriority.Enabled = True
                If txtlowerlimit.Text <> "" Then
                    RefreshLineValues()
                Else
                    txtlowerlimit.Text = (0).ToString("N2")
                End If
                chkObligatory.Enabled = cboTreatyType.ItemCode.Trim() = "PROP" And cboRIType.ItemCode.Trim() = "QUO"
                If m_bIsObligatory Then
                    chkObligatory.Checked = True
                End If
                chkVariableQuotaShare.Enabled = cboRIType.ItemCode.Trim() = "QUO" AndAlso cboTreatyType.ItemCode.Trim() = "PROP"
                If m_bIsVariableQuotaShare AndAlso cboRIType.ItemCode.Trim() = "QUO" AndAlso cboTreatyType.ItemCode.Trim() = "PROP" Then
                    chkVariableQuotaShare.Checked = True
                Else
                    RemoveVariableQuotaShareTab()
                End If
            End If
        ElseIf Not m_bIsRI2007Enabled Then
            If cboTreatyType.ItemCode.Trim() = "XOL" Then
                chkObligatory.Enabled = False
                If m_bIsObligatory Then
                    chkObligatory.Checked = True
                End If
            ElseIf cboTreatyType.ItemCode.Trim() = "PROP" Then
                chkObligatory.Enabled = True
                If m_bIsObligatory Then
                    chkObligatory.Checked = True
                End If
            End If
        End If
        If m_bIsRI2007Enabled Then

            If (Trim(cboTreatyType.ItemCode) = "XOL" AndAlso Trim(cboRIType.ItemCode) = "XOL") Then
                chkVariableQuotaShare.Visible = False
                chkCedePremiumOnly.Visible = True
                chkCedePremiumOnly.Enabled = True
                If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                    'Do Nothing
                ElseIf m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    chkCedePremiumOnly.Checked = False
                End If
            ElseIf (Trim(cboTreatyType.ItemCode) = "XOL" AndAlso Trim(cboRIType.ItemCode) = "CAT") Then
                chkVariableQuotaShare.Visible = False
                chkCedePremiumOnly.Visible = True
                chkCedePremiumOnly.Enabled = False
                chkCedePremiumOnly.Checked = True
            ElseIf cboTreatyType.ItemCode.Trim() = "PROP" And cboRIType.ItemCode.Trim() = "QUO" Then
                chkCedePremiumOnly.Visible = False
                chkVariableQuotaShare.Visible = True
                chkVariableQuotaShare.Enabled = False
                If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                    'Do Nothing
                ElseIf m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    chkVariableQuotaShare.Checked = False
                End If
            Else
                chkCedePremiumOnly.Visible = False
            End If
        Else
            chkCedePremiumOnly.Visible = False
        End If

    End Sub
    Private Sub chkCedePremiumOnly_Click()
        If chkCedePremiumOnly.Checked AndAlso chkCedePremiumOnly.Enabled Then
            txtTreatyLimit.Text = 0
        Else
            RefreshLineValues()
        End If
    End Sub

    Private Sub chkObligatory_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkObligatory.CheckStateChanged

        Dim lCount, lTemp As Integer

        Const kMethodName As String = "chkIsObligatory_Click"

        Try

            If chkObligatory.CheckState = CheckState.Checked Then

                txtPriority.Text = CStr(1)
                txtPriority.Enabled = False
                lblPriority.Enabled = False

                txtLines.Text = CStr(1)
                txtLines.Enabled = False
                lblLines.Enabled = False

                txtlowerlimit.Text = "0"
                txtlowerlimit.Enabled = False
                lbllowerlimit.Enabled = False

                lblLineLimit.Enabled = False
                txtLineLimit.Text = "0"
                txtLineLimit.Enabled = False
                ' Set Premium Calculation Basis to "Prop RI % X Gross" for obligatory treaties
                If m_iTreatyPremiumType = TreatyPremiumTypeEnum.VariableCessionOrder OrElse cboPremiumCalculationBasis.Visible Then
                    cboPremiumCalculationBasis.ItemId = PremiumCalculationBasisEnum.PROPGRSS
                    cboPremiumCalculationBasis.Enabled = False
                End If

                m_bDoIncrementPriorities = True
            Else
                txtPriority.Enabled = True
                lblPriority.Enabled = True

                txtLines.Text = "1"
                txtLines.Enabled = True

                lblLines.Enabled = True

                If m_bIsRI2007Enabled Then
                    If cboTreatyType.ItemCode <> "PROP" Then
                        txtlowerlimit.Enabled = True
                        lbllowerlimit.Enabled = True
                    End If
                End If

                txtLineLimit.Enabled = True
                lblLineLimit.Enabled = True
                ' Re-enable Premium Calculation Basis when not obligatory
                cboPremiumCalculationBasis.Enabled = True
                m_bDoIncrementPriorities = False

            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        ' Check the user wants to close
        If MessageBox.Show("Cancelling will lose all of your current details." &
                           Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
            ' Set status to cancel and close
            Status = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()
        End If
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim dShare As Double
        Dim lReturn As Integer
        Dim iSelectedIndexInArray As Integer
        Const kMethodName As String = "cmdOK_Click"
        'Start Renuka - Bug Fixing (PN 63036)

        'End Renuka - Bug Fixing (PN 63036)

        Try


            ' Validate data
            Dim bResult As Boolean
            Dim lTemp, lVerifyObligatory As Integer
            Dim dSharePercent As Decimal
            If Validate_Renamed() = gPMConstants.PMEReturnCode.PMTrue Then
                If Information.IsArray(m_vRIModelLines) Then
                    For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                            If gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) = CBool(chkObligatory.CheckState) And chkObligatory.CheckState Then
                                MessageBox.Show("Only one Obligatory line allowed per RI Model", "Invalid Choice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Sub
                            End If
                            If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) = cboTreaty.ItemId Then
                                MessageBox.Show("Selected Treaty exists in the list. Please select another treaty.", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Sub
                            End If
                            If cboRIType.ItemId = ACRetained And m_bIsMultipleRetained = True Then
                                MessageBox.Show("Only 1 Treaty Retained Line can be added (attached) to an RI Model", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Sub
                            End If
                            If m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount) <> 0 Then
                                If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)) = gPMFunctions.ToSafeInteger(txtPriority.Text) Then
                                    If chkObligatory.CheckState = CheckState.Unchecked AndAlso cboTreatyType.ItemCode.Trim() <> "XOL" Then
                                        If gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount)) = 1 Then
                                            dSharePercent = CDec(dSharePercent + CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lCount)))
                                            If (dSharePercent + CDbl(m_oFormFields.UnformatControl(txtSharePercent))) > 100 Then
                                                MessageBox.Show("You can assign only 100% to treaty with same priority in the list." &
                                                                Strings.Chr(13) & "Please select another treaty with different priority.", "Invalid Treaty Share", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                Exit Sub
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                        ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                            If gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) = 1 Or gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Then
                                lTemp += 1
                                lVerifyObligatory = CInt(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount))
                                iSelectedIndexInArray = lCount
                            End If

                            If m_lSelectedIndex <> lCount Then
                                If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) = cboTreaty.ItemId Then
                                    MessageBox.Show("Selected Treaty exists in the list. Please select another treaty.", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Sub
                                End If
                            End If

                            'If Not (m_bIsRI2007Enabled) Then
                            If m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount) <> 0 Then
                                If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)) = gPMFunctions.ToSafeInteger(txtPriority.Text) Then
                                    If gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount)) = 1 Then
                                        If m_lSelectedIndex <> lCount Then
                                            dSharePercent = CInt(dSharePercent + CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lCount)))
                                        End If
                                        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                                            If chkObligatory.CheckState = CheckState.Unchecked AndAlso cboTreatyType.ItemCode.Trim() <> "XOL" Then
                                                If (dSharePercent + CDbl(m_oFormFields.UnformatControl(txtSharePercent))) > 100 Then
                                                    MessageBox.Show("You can assign only 100% to treaty with same priority in the list." &
                                                                    Strings.Chr(13) & "Please select another treaty with different priority.", "Invalid Treaty Share", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    Exit Sub
                                                End If
                                            End If

                                        Else
                                            If chkObligatory.CheckState = CheckState.Unchecked AndAlso cboTreatyType.ItemCode.Trim() <> "XOL" Then
                                                If (dSharePercent + CDbl(m_oFormFields.UnformatControl(txtSharePercent))) > 100 Then
                                                    MessageBox.Show("You can assign only 100% to treaty with same priority in the list." &
                                                                            Strings.Chr(13) & "Please select another treaty with different priority.", "Invalid Treaty Share", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    Exit Sub
                                                End If
                                            End If

                                        End If
                                    End If
                                End If
                            End If

                            'End If
                        End If
                    Next

                    If m_iTask = gPMConstants.PMEComponentAction.PMEdit And lTemp = 1 And lVerifyObligatory <> 0 And chkObligatory.CheckState = CheckState.Checked And m_lSelectedIndex <> iSelectedIndexInArray Then
                        MessageBox.Show("Only one Obligatory line allowed per RI Model", "Invalid Choice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                    If gPMFunctions.ToSafeCurrency(txtLineLimit.Text) < 0 Then
                        MessageBox.Show("Enter a valid Line Limit.", "Invalid LineLimit", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                ' Store Variable Quota Share data before closing
                If chkVariableQuotaShare.Checked AndAlso m_bHasVariableQuotaShareTab AndAlso m_tabVariableQuotaShare IsNot Nothing Then
                    Dim lvwVariableQS As DataGridView = Nothing
                    For Each ctrl As Control In m_tabVariableQuotaShare.Controls
                        If TypeOf ctrl Is DataGridView AndAlso ctrl.Name = "lvwVariableQS" Then
                            lvwVariableQS = DirectCast(ctrl, DataGridView)
                            Exit For
                        End If
                    Next

                    If lvwVariableQS IsNot Nothing AndAlso lvwVariableQS.Rows.Count > 0 Then
                        Dim riModelLineID As Integer = GetRIModelLineID()
                        Dim gridRowCount As Integer = lvwVariableQS.Rows.Count

                        ' Count existing rows for other RIModelLineIDs
                        Dim otherRowCount As Integer = 0
                        If Information.IsArray(m_vTreatyVariableQuotaShare) Then
                            For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                                If gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i)) <> riModelLineID Then
                                    otherRowCount += 1
                                End If
                            Next
                        End If

                        ' Create new array with combined data
                        Dim totalRows As Integer = otherRowCount + gridRowCount - 1
                        Dim newArray(DBMVMax, totalRows) As Object
                        Dim newIndex As Integer = 0

                        ' Copy data from other RIModelLineIDs
                        If Information.IsArray(m_vTreatyVariableQuotaShare) Then
                            For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                                If gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i)) <> riModelLineID Then
                                    For col As Integer = 0 To DBMVMax
                                        newArray(col, newIndex) = m_vTreatyVariableQuotaShare(col, i)
                                    Next
                                    newIndex += 1
                                End If
                            Next
                        End If

                        ' Add current grid data
                        For i As Integer = 0 To gridRowCount - 1
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLVariableQuotaShareId, newIndex) = lvwVariableQS.Rows(i).Cells("VariableQuotaShareId").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLSALowerLimit, newIndex) = lvwVariableQS.Rows(i).Cells("SALowerLimit").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLSAUpperLimit, newIndex) = lvwVariableQS.Rows(i).Cells("SAUpperLimit").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLSharePercent, newIndex) = lvwVariableQS.Rows(i).Cells("SharePercent").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyLimit, newIndex) = lvwVariableQS.Rows(i).Cells("TreatyLimit").Value
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, newIndex) = cboTreaty.ItemId
                            newArray(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, newIndex) = riModelLineID
                            newIndex += 1
                        Next

                        m_vTreatyVariableQuotaShare = newArray
                    End If
                End If
                ' Set status to OK and close
                ' Surplus lines decimals - restore raw decimal value before GetProperties reads txtLines
                If m_dRawLines > 0 Then txtLines.Text = m_dRawLines.ToString()
                Status = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
            End If
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    Public Sub frmRIModelLineLoad()

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Form_Load"
        Dim vValue As String = ""

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialize business object
            m_lReturn = g_oObjectManager.GetInstance(m_oBusinessObject, "bSIRRIModel.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of bSIRRIModel business object")
            End If

            cboTreaty.FirstItem = ""
            cboRIType.FirstItem = ""
            cboTreatyType.FirstItem = ""
            cboPremiumCalculationBasis.FirstItem = "Please Select"
            m_lReturn = iPMFunc.GetSystemOption(5005, m_sUnderwritingType)

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form Load", "getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bIsRI2007Enabled = vValue = "1"

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultipleRetainedTreaty, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form Load", "getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTMultipleRetainedTreaty, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bIsMultipleRetained = vValue = "1"

            lReturn = CType(SetRI2007InterfaceDefaults(), gPMConstants.PMEReturnCode)
            ' Validate fields using Forms Control
            lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetFieldValidation", "Unable to set field validation")
            End If

            If m_sUnderwritingType = "1" Then
                lblTreaty.Text = "Binding Authority :"
            End If
            ' Surplus lines decimals - call cboTreaty_Click for both RI2007 on and off so cboRIType.ItemId is set correctly
            If cboTreaty.ItemId > 0 Then
                cboTreaty_Click(cboTreaty, New EventArgs())
            End If

            ' Initialize Variable Quota Share state after form is fully loaded

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub
    Private Sub frmRIModelLine_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"
        Try

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Check the user wants to close
                If MessageBox.Show("Cancelling will lose all of your current details." &
                               Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                    ' Do not procced with the interface termination.
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    Cancel = 1
                Else
                    Status = gPMConstants.PMEReturnCode.PMCancel
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)
        Finally
        End Try
        Exit Sub
    End Sub
    Private Sub txtcedingrate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtcedingrate.Enter
        m_oFormFields.GotFocus(ctlControl:=txtcedingrate)
    End Sub

    Private Sub txtcedingrate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtcedingrate.Leave
        m_oFormFields.LostFocus(ctlControl:=txtcedingrate)
        RefreshLineValues()
    End Sub
    Private Sub txtLineLimit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLineLimit.Enter
        m_oFormFields.GotFocus(ctlControl:=txtLineLimit)
    End Sub
    Private Sub txtLineLimit_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLineLimit.Leave
        m_oFormFields.LostFocus(ctlControl:=txtLineLimit)

        ' Update priority line limit and summary
        ''PN 71310
        txtLineLimit.Text = CStr(gPMFunctions.ToSafeCurrency(txtLineLimit.Text))
        m_oPriority.LineLimit = CDec(m_oFormFields.UnformatControl(txtLineLimit))
        RefreshLineValues()
    End Sub

    Private Sub txtLines_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtLines.KeyPress
        Dim isSurplus As Boolean = cboRIType.ItemId = ACSurplus OrElse
                                   cboRIType.ItemId = ACSecondSurplus OrElse
                                   cboRIType.ItemId = ACThirdSurplus
        If Char.IsControl(e.KeyChar) Then
            ' Always allow control characters (backspace, delete, etc.)
        ElseIf e.KeyChar = "."c Then
            If Not isSurplus OrElse txtLines.Text.Contains(".") Then
                e.Handled = True
            End If
        ElseIf Char.IsDigit(e.KeyChar) Then
            If isSurplus Then
                Dim dotPos As Integer = txtLines.Text.IndexOf("."c)
                If dotPos >= 0 AndAlso txtLines.SelectionStart > dotPos Then
                    Dim decimals As Integer = txtLines.Text.Length - dotPos - 1 - txtLines.SelectionLength
                    If decimals >= 2 Then e.Handled = True
                End If
            End If
        Else
            e.Handled = True
        End If
        ' Surplus lines decimals - store raw value on each keystroke before LostFocus can reformat
        If isSurplus Then
            Dim raw As Decimal
            Dim newText As String = txtLines.Text
            If Not Char.IsControl(e.KeyChar) Then
                newText = txtLines.Text.Substring(0, txtLines.SelectionStart) & e.KeyChar & txtLines.Text.Substring(txtLines.SelectionStart + txtLines.SelectionLength)
            End If
            If Decimal.TryParse(newText, raw) Then m_dRawLines = raw
        End If
    End Sub

    Private Sub txtLines_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtLines.TextChanged
        Dim isSurplus As Boolean = cboRIType.ItemId = ACSurplus OrElse
                                   cboRIType.ItemId = ACSecondSurplus OrElse
                                   cboRIType.ItemId = ACThirdSurplus
        If Not isSurplus AndAlso txtLines.Text.Contains(".") Then
            Dim caretPos As Integer = txtLines.SelectionStart
            Dim dotIndex As Integer = txtLines.Text.IndexOf("."c)
            txtLines.Text = txtLines.Text.Substring(0, dotIndex)
            txtLines.SelectionStart = Math.Min(caretPos, txtLines.Text.Length)
        End If
    End Sub

    Private Sub txtLines_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLines.Enter
        m_oFormFields.GotFocus(ctlControl:=txtLines)
    End Sub
    Private Sub txtLines_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLines.Leave
        ' Surplus lines decimals - capture raw value before LostFocus reformats it
        Dim raw As Decimal
        If Decimal.TryParse(txtLines.Text, raw) Then m_dRawLines = raw
        m_oFormFields.LostFocus(ctlControl:=txtLines)

        ' For non-surplus types, strip decimals and display as integer
        Dim isSurplus As Boolean = cboRIType.ItemId = ACSurplus OrElse
                                   cboRIType.ItemId = ACSecondSurplus OrElse
                                   cboRIType.ItemId = ACThirdSurplus
        If Not isSurplus Then
            Dim dLines As Double = gPMFunctions.ToSafeDouble(txtLines.Text)
            If dLines > 0 Then txtLines.Text = CInt(Math.Floor(dLines)).ToString()
        End If

        ' Update priority line count - use raw decimal for surplus to avoid integer reformatting
        m_oPriority.Lines = If(isSurplus AndAlso m_dRawLines > 0, m_dRawLines, CDec(m_oFormFields.UnformatControl(txtLines)))
        RefreshLineValues()
    End Sub

    Private Sub txtlowerlimit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtlowerlimit.Enter
        m_oFormFields.GotFocus(ctlControl:=txtlowerlimit)
    End Sub

    Private Sub txtlowerlimit_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtlowerlimit.Leave
        m_oFormFields.LostFocus(ctlControl:=txtlowerlimit)
        m_oPriority.LowerLimit = CDec(m_oFormFields.UnformatControl(txtlowerlimit))
        ' Check and update priority values
        RefreshLineValues()
    End Sub

    Private Sub txtPriority_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPriority.Enter
        m_oFormFields.GotFocus(ctlControl:=txtPriority)
    End Sub
    Private Sub txtPriority_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPriority.Leave
        m_oFormFields.LostFocus(ctlControl:=txtPriority)
        ' Check and update priority values
        RefreshLineValues()
    End Sub
    Private Sub txtSharePercent_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSharePercent.Enter
        m_oFormFields.GotFocus(ctlControl:=txtSharePercent)
    End Sub
    Private Sub txtSharePercent_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSharePercent.Leave
        m_oFormFields.LostFocus(ctlControl:=txtSharePercent)
        If txtSharePercent.Text <> "" Then
            RefreshLineValues()
        End If
        UpdateVariableQuotaShareState()
    End Sub



    Private Function SetRI2007InterfaceDefaults() As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            cboTreatyType.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            cboTreatyType.TableName = "Treaty_Type"
            cboTreatyType.DefaultItemId = 1
            cboTreatyType.RefreshList()

            cboRIType.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            cboRIType.TableName = "reinsurance_type"
            cboRIType.RefreshList()
            cboRIType.Enabled = False
            cboPremiumCalculationBasis.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            cboPremiumCalculationBasis.TableName = "premium_calculation_basis"
            ' Filter by reinsurance type if one is already selected, otherwise show none until RI type is chosen
            If cboRIType.ItemId > 0 Then
                If IsSurplusOrQuotaShare(cboRIType.ItemId) Then
                    cboPremiumCalculationBasis.WhereClause = "reinsurance_type_id IS NULL"
                Else
                    cboPremiumCalculationBasis.WhereClause = "reinsurance_type_id = " & cboRIType.ItemId
                End If
            Else
                cboPremiumCalculationBasis.WhereClause = "1=0"
            End If
            cboPremiumCalculationBasis.RefreshList()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set RI 2007 interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRI2007InterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Sub frmRIModelLine_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If m_bIsRI2007Enabled Then
            If cboTreatyType.ItemCode.Trim() = "XOL" Then
                lbllowerlimit.Enabled = True
                txtlowerlimit.Enabled = True
                lblcedingrate.Enabled = True
                txtcedingrate.Enabled = True
                lblLines.Enabled = False
                txtLines.Enabled = False
                chkObligatory.Enabled = False
                If m_bIsObligatory Then
                    chkObligatory.Checked = True
                End If

            Else
                lblLines.Visible = True
                txtLines.Visible = True
                lbllowerlimit.Enabled = False
                txtlowerlimit.Enabled = False
                lblcedingrate.Enabled = False
                txtcedingrate.Enabled = False
                txtcedingrate.Text = CStr(0)
                lblLines.Enabled = True
                txtLines.Enabled = True
                txtPriority.Enabled = True
                lblPriority.Enabled = True
                If txtlowerlimit.Text <> "" Then
                    RefreshLineValues()
                Else
                    txtlowerlimit.Text = (0).ToString("N2")
                End If
                'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
                chkObligatory.Enabled = cboTreatyType.ItemCode.Trim() = "PROP" And cboRIType.ItemCode.Trim() = "QUO"
                'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
                If m_bIsObligatory Then
                    chkObligatory.Checked = True
                    lblLines.Enabled = False
                    txtLines.Enabled = False
                    txtPriority.Enabled = False
                    lblPriority.Enabled = False
                End If
            End If
            'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
        ElseIf Not m_bIsRI2007Enabled Then
            If cboTreatyType.ItemCode.Trim() = "XOL" Then
                chkObligatory.Enabled = False
            End If
            'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
            If m_bIsObligatory Then
                chkObligatory.Checked = True
            End If
        End If

        If m_bIsRI2007Enabled Then
            lbllowerlimit.Visible = True
            txtlowerlimit.Visible = True
            lblcedingrate.Visible = True
            txtcedingrate.Visible = True
            If m_iTask <> gPMConstants.PMEComponentAction.PMEdit Then
                lblTreatyType.Enabled = True
                cboTreatyType.Enabled = True
            Else
                lblTreatyType.Enabled = False
                cboTreatyType.Enabled = False
            End If
            lblLineLimit.Text = "Line/Upper Limit:"
            lblLines.Visible = True
            txtLines.Visible = True
            If cboRIType.ItemCode.Trim() = "QUO" AndAlso cboTreatyType.ItemCode.Trim() = "PROP" Then
                chkVariableQuotaShare.Visible = True
                chkVariableQuotaShare.Enabled = False
                If chkVariableQuotaShare IsNot Nothing Then
                    chkVariableQuotaShare = chkVariableQuotaShare
                    AddHandler chkVariableQuotaShare.CheckedChanged, AddressOf chkVariableQuotaShare_CheckedChanged
                    UpdateVariableQuotaShareState()
                    CheckAndLoadVariableQuotaShare()
                End If
            Else
                chkVariableQuotaShare.Visible = False
                RemoveVariableQuotaShareTab()
            End If


        Else
            lbllowerlimit.Visible = False
            txtlowerlimit.Visible = False
            lblcedingrate.Visible = False
            txtcedingrate.Visible = False
            cboTreatyType.Enabled = False
            lblTreatyType.Enabled = False
        End If




        ' Form dimensions are set in designer - no need to override
    End Sub

    ''' <summary>
    ''' Gets the RI Model Line ID from the current selection
    ''' </summary>
    Private Function GetRIModelLineID() As Integer
        Try
            If Information.IsArray(m_vRIModelLines) AndAlso m_lSelectedIndex >= 0 AndAlso
            m_vRIModelLines.GetUpperBound(0) >= MainModule.RIModelLineEnum.DBMLRIModelLineID Then
                Return gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIModelLineID, m_lSelectedIndex))
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="GetRIModelLineID", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
        Return 0
    End Function

    Private Function GetRIModelID() As Integer
        Try
            If Information.IsArray(m_vRIModelLines) AndAlso m_lSelectedIndex >= 0 AndAlso
            m_vRIModelLines.GetUpperBound(0) >= MainModule.RIModelLineEnum.DBMLRIModelID Then
                Return gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIModelID, m_lSelectedIndex))
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="GetRIModelID", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
        Return 0
    End Function


    ''' <summary>
    ''' Checks for existing Variable Quota Share data and sets up UI accordingly
    ''' </summary>
    Private Sub CheckAndLoadVariableQuotaShare()
        Try
            If cboTreaty.ItemId > 0 Then
                Dim configData(,) As Object = Nothing
                Dim riModelLineID As Integer = GetRIModelLineID()
                Dim riModelId As Integer = GetRIModelID()

                Dim lReturn As Integer = m_oBusinessObject.GetRIModelVariableQuotaShareConfig(riModelId, configData)
                m_vTreatyVariableQuotaShare = configData

                If lReturn = gPMConstants.PMEReturnCode.PMTrue AndAlso configData IsNot Nothing Then
                    If Information.IsArray(m_vTreatyVariableQuotaShare) AndAlso m_vTreatyVariableQuotaShare.GetUpperBound(1) >= 0 Then
                        Dim hasData As Boolean = False
                        For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                            Dim dataRIModelLineID As Integer = gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i))
                            Dim dataTreatyId As Integer = gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i))

                            ' Always filter by treatyId to ensure correct treaty configuration is displayed
                            Dim bshouldDisplay As Boolean = (dataTreatyId = cboTreaty.ItemId)
                            If dataRIModelLineID = riModelLineID Then
                                If bshouldDisplay Then

                                    hasData = True
                                    Exit For
                                End If
                            End If
                        Next
                        If hasData = True Then
                            If chkVariableQuotaShare IsNot Nothing Then
                                chkVariableQuotaShare.Checked = True
                                If m_iConfigStatusTask = PMEComponentAction.PMReverse Then
                                    Dim lvwVariableQS As DataGridView = Nothing
                                    For Each ctrl As Control In m_tabVariableQuotaShare.Controls
                                        If TypeOf ctrl Is DataGridView AndAlso ctrl.Name = "lvwVariableQS" Then
                                            lvwVariableQS = DirectCast(ctrl, DataGridView)
                                            Exit For
                                        End If
                                    Next

                                    FillDataGridView(lvwVariableQS, m_vTreatyVariableQuotaShare, False)
                                Else
                                    AddVariableQuotaShareTab()
                                End If

                                tabControl.SelectedTab = tabTreatyLine
                            End If
                        End If
                    End If
                Else
                    If m_iConfigStatusTask = gPMConstants.PMEComponentAction.PMReverse Then
                        chkVariableQuotaShare.Checked = False
                        RemoveVariableQuotaShareTab()
                    End If
                End If
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="CheckAndLoadVariableQuotaShare", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    Private Sub FillDataGridView(lvwVariableQS As DataGridView, m_vTreatyVariableQuotaShare As Object(,), addMethodCalled As Boolean)
        lvwVariableQS.Rows.Clear()
        Dim riModelLineID As Integer = GetRIModelLineID()
        ' Filter and display data: by treatyId (always filter by treaty)
        If Information.IsArray(m_vTreatyVariableQuotaShare) AndAlso m_vTreatyVariableQuotaShare.GetUpperBound(1) >= 0 Then
            Dim hasData As Boolean = False
            For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                Dim dataRIModelLineID As Integer = gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i))
                Dim dataTreatyId As Integer = gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i))

                ' Always filter by treatyId to ensure correct treaty configuration is displayed
                Dim bshouldDisplay As Boolean = (dataTreatyId = cboTreaty.ItemId)
                If m_iConfigStatusTask = PMEComponentAction.PMReverse Then
                    If dataRIModelLineID = riModelLineID Then
                        addMethodCalled = True
                    Else
                        addMethodCalled = False
                    End If
                End If
                If bshouldDisplay AndAlso addMethodCalled = True Then
                    lvwVariableQS.Rows.Add(
                            m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSALowerLimit, i),
                            m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSAUpperLimit, i),
                            m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSharePercent, i),
                            m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyLimit, i),
                            m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLVariableQuotaShareId, i)
                        )
                    hasData = True
                End If
            Next
            If Not hasData Then
                lvwVariableQS.Rows.Add("", "", "", "", Nothing)
            End If
        Else
            lvwVariableQS.Rows.Add("", "", "", "", Nothing)
        End If

    End Sub

    ''' <summary>
    ''' Loads Variable Quota Share configuration data from database and in-memory new additions
    ''' </summary>
    Private Sub LoadVariableQuotaShareData(lvwVariableQS As DataGridView)
        Try
            Dim riModelLineID As Integer = GetRIModelLineID()
            Dim treatyId As Integer = cboTreaty.ItemId
            Dim riModelId As Integer = GetRIModelID()
            ' Load data from database and merge with existing in-memory data
            If treatyId > 0 Then
                Dim dbData(,) As Object = Nothing

                m_oBusinessObject.GetRIModelVariableQuotaShareConfig(riModelId, dbData)

                ' Merge database data with existing in-memory data
                If dbData IsNot Nothing AndAlso dbData.GetUpperBound(1) >= 0 Then
                    If Information.IsArray(m_vTreatyVariableQuotaShare) Then
                        ' Merge: keep in-memory data and add new database data that doesn't exist in memory
                        Dim mergedList As New List(Of Integer)
                        Dim existingIds As New HashSet(Of String)

                        ' Add existing in-memory data
                        For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                            Dim key As String = gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i)).ToString() & "_" &
                                               gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i)).ToString()
                            existingIds.Add(key)
                        Next

                        ' Calculate total rows needed
                        Dim totalRows As Integer = m_vTreatyVariableQuotaShare.GetUpperBound(1) + 1
                        For i As Integer = 0 To dbData.GetUpperBound(1)
                            Dim key As String = gPMFunctions.ToSafeInteger(dbData(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i)).ToString() & "_" &
                                               gPMFunctions.ToSafeInteger(dbData(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i)).ToString()
                            If Not existingIds.Contains(key) Then
                                totalRows += 1
                            End If
                        Next

                        ' Create merged array
                        Dim mergedArray(DBMVMax, totalRows - 1) As Object
                        Dim idx As Integer = 0

                        ' Copy existing in-memory data
                        For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                            For col As Integer = 0 To DBMVMax
                                mergedArray(col, idx) = m_vTreatyVariableQuotaShare(col, i)
                            Next
                            idx += 1
                        Next

                        ' Add new database data
                        For i As Integer = 0 To dbData.GetUpperBound(1)
                            Dim key As String = gPMFunctions.ToSafeInteger(dbData(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i)).ToString() & "_" &
                                               gPMFunctions.ToSafeInteger(dbData(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i)).ToString()
                            If Not existingIds.Contains(key) Then
                                For col As Integer = 0 To DBMVMax
                                    mergedArray(col, idx) = dbData(col, i)
                                Next
                                idx += 1
                            End If
                        Next

                        m_vTreatyVariableQuotaShare = mergedArray
                    Else
                        m_vTreatyVariableQuotaShare = dbData
                    End If
                End If
            End If
            FillDataGridView(lvwVariableQS, m_vTreatyVariableQuotaShare, True)

        Catch ex As Exception
            lvwVariableQS.Rows.Clear()
            lvwVariableQS.Rows.Add("", "", "", "", Nothing)
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="LoadVariableQuotaShareData", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for Save button in Variable Quota Share tab
    ''' </summary>
    Private Sub btnBackVQS_Click(sender As Object, e As EventArgs)
        Try
            Dim lvwVariableQS As DataGridView = Nothing
            For Each ctrl As Control In m_tabVariableQuotaShare.Controls
                If TypeOf ctrl Is DataGridView AndAlso ctrl.Name = "lvwVariableQS" Then
                    lvwVariableQS = DirectCast(ctrl, DataGridView)
                    Exit For
                End If
            Next

            ' Run mandatory validations for all cells in the datagrid
            If lvwVariableQS IsNot Nothing Then
                For i As Integer = 0 To lvwVariableQS.Rows.Count - 1
                    For j As Integer = 0 To lvwVariableQS.Columns.Count - 1
                        If j = lvwVariableQS.Columns("VariableQuotaShareId").Index OrElse j = lvwVariableQS.Columns("TreatyLimit").Index Then
                            Continue For
                        End If

                        Dim cellValue As Object = lvwVariableQS.Rows(i).Cells(j).Value
                        If String.IsNullOrWhiteSpace(cellValue?.ToString()) Then
                            MessageBox.Show("This field is mandatory.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            lvwVariableQS.CurrentCell = lvwVariableQS.Rows(i).Cells(j)
                            Exit Sub
                        End If

                        If j = lvwVariableQS.Columns("SALowerLimit").Index OrElse j = lvwVariableQS.Columns("SAUpperLimit").Index Then
                            Dim saLowerLimit As Double = gPMFunctions.ToSafeCurrency(lvwVariableQS.Rows(i).Cells("SALowerLimit").Value)
                            Dim saUpperLimit As Double = gPMFunctions.ToSafeCurrency(lvwVariableQS.Rows(i).Cells("SAUpperLimit").Value)
                            If saLowerLimit > 0.01 Then
                                If saLowerLimit > 0.01 AndAlso saUpperLimit > 0 AndAlso saLowerLimit >= saUpperLimit Then
                                    MessageBox.Show("SA Lower Limit must be less than SA Upper Limit.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                    lvwVariableQS.CurrentCell = lvwVariableQS.Rows(i).Cells(j)
                                    Exit Sub
                                End If
                            Else
                                MessageBox.Show("The Lower Limit should be atleast 0.01 or more", "Invalid Lower Limit", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                lvwVariableQS.CurrentCell = lvwVariableQS.Rows(i).Cells(j)
                                Exit Sub
                            End If
                        End If
                    Next
                Next
            End If

            If m_bIsRI2007Enabled Then
                ' Check Variable Quota Share configuration if enabled
                '                If chkVariableQuotaShare IsNot Nothing AndAlso chkVariableQuotaShare.Checked Then


                If lvwVariableQS IsNot Nothing AndAlso lvwVariableQS.Rows.Count = 1 Then
                    Dim result As DialogResult = MessageBox.Show(
                    "To configure variable Quota Share treaty at least two Share % for Sum Insured ranges are required, do you still want to continue.",
                    "Variable Quota Share Configuration",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)

                    If result = DialogResult.No Then
                        Exit Sub
                    End If
                End If


                ' End If
            End If
            ' Validate SA Upper Limit against treaty Upper Limit
            'If lvwVariableQS IsNot Nothing Then
            '    Dim treatyUpperLimit As Double = gPMFunctions.ToSafeDouble(txtTreatyLimit.Text)
            '    For i As Integer = 0 To lvwVariableQS.Rows.Count - 1
            '        Dim saUpperLimit As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(i).Cells("SAUpperLimit").Value)
            '        If saUpperLimit > treatyUpperLimit Then
            '            MessageBox.Show("SA Upper Limit is be more than the Upper Limit for the treaty.", "Invalid SA Upper Limit", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            '            Exit Sub
            '        End If
            '    Next
            'End If

            ' Validate overlapping ranges
            If Not ValidateOverlappingRanges(lvwVariableQS) Then
                Exit Sub
            End If

            ' Validate missing ranges
            If Not ValidateMissingRanges(lvwVariableQS) Then
                Exit Sub
            End If
            m_lTreatyID = cboTreaty.ItemId
            If lvwVariableQS IsNot Nothing Then
                Dim riModelLineID As Integer = GetRIModelLineID()
                Dim gridRowCount As Integer = lvwVariableQS.Rows.Count

                ' Count existing rows for other RIModelLineIDs
                Dim otherRowCount As Integer = 0
                If Information.IsArray(m_vTreatyVariableQuotaShare) Then
                    For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                        Dim dataRIModelLineID As Integer = gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i))
                        If dataRIModelLineID <> riModelLineID Then
                            otherRowCount += 1
                        End If
                    Next
                End If

                ' Create new array with combined data
                Dim totalRows As Integer = otherRowCount + gridRowCount - 1
                Dim newArray(DBMVMax, totalRows) As Object
                Dim newIndex As Integer = 0

                ' Copy data from other RIModelLineIDs
                If Information.IsArray(m_vTreatyVariableQuotaShare) Then
                    For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                        Dim dataRIModelTreatyID As Integer = gPMFunctions.ToSafeInteger(m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i))
                        If dataRIModelTreatyID <> cboTreaty.ItemId Then
                            For col As Integer = 0 To DBMVMax
                                newArray(col, newIndex) = m_vTreatyVariableQuotaShare(col, i)
                            Next
                            newIndex += 1
                        End If
                    Next
                End If

                ' Add current grid data
                For i As Integer = 0 To gridRowCount - 1
                    newArray(MainModule.RIModelLineQuotaShareEnum.DBMLVariableQuotaShareId, newIndex) = lvwVariableQS.Rows(i).Cells("VariableQuotaShareId").Value
                    newArray(MainModule.RIModelLineQuotaShareEnum.DBMLSALowerLimit, newIndex) = lvwVariableQS.Rows(i).Cells("SALowerLimit").Value
                    newArray(MainModule.RIModelLineQuotaShareEnum.DBMLSAUpperLimit, newIndex) = lvwVariableQS.Rows(i).Cells("SAUpperLimit").Value
                    newArray(MainModule.RIModelLineQuotaShareEnum.DBMLSharePercent, newIndex) = lvwVariableQS.Rows(i).Cells("SharePercent").Value
                    newArray(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyLimit, newIndex) = lvwVariableQS.Rows(i).Cells("TreatyLimit").Value
                    newArray(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, newIndex) = m_lTreatyID
                    newArray(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, newIndex) = riModelLineID
                    newIndex += 1
                Next

                m_vTreatyVariableQuotaShare = newArray
                m_bAllowTabSwitch = True
                tabControl.SelectedIndex = 0
                m_bAllowTabSwitch = False
            End If
            m_iConfigStatusTask = gPMConstants.PMEComponentAction.PMAdded
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="btnSaveVQS_Click", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for Cancel button in Variable Quota Share tab
    ''' </summary>
    Private Sub btnCancelVQS_Click(sender As Object, e As EventArgs)
        Try
            m_iConfigStatusTask = gPMConstants.PMEComponentAction.PMReverse
            CheckAndLoadVariableQuotaShare()
            ' Only re-enable Treaty Line tab if checkbox is unchecked after reload
            If Not chkVariableQuotaShare.Checked Then
                EnableTreatyLineTab()
            End If
            m_bAllowTabSwitch = True
            tabControl.SelectedIndex = 0
            m_bAllowTabSwitch = False
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="btnCancelVQS_Click", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for tab selection change - refreshes Variable Quota Share tab values
    ''' </summary>
    Private Sub tabControl_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            If m_bHasVariableQuotaShareTab AndAlso m_tabVariableQuotaShare IsNot Nothing AndAlso tabControl.SelectedTab Is m_tabVariableQuotaShare Then
                ' Update txtUpperLimit and txtNoOfLines when tab is selected
                For Each ctrl As Control In m_tabVariableQuotaShare.Controls
                    If TypeOf ctrl Is TextBox Then
                        Dim txt As TextBox = DirectCast(ctrl, TextBox)
                        If txt.Name = "txtUpperLimit" Then
                            txt.Text = If(String.IsNullOrEmpty(txtLineLimit.Text), "0", txtLineLimit.Text)
                        ElseIf txt.Name = "txtNoOfLines" Then
                            txt.Text = If(String.IsNullOrEmpty(txtLines.Text), "1", txtLines.Text)
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="tabControl_SelectedIndexChanged", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' Validates that there are no gaps in SA ranges
    ''' </summary>
    Private Function ValidateMissingRanges(lvwVariableQS As DataGridView) As Boolean
        Try
            If lvwVariableQS Is Nothing OrElse lvwVariableQS.Rows.Count = 0 Then
                Return True
            End If

            Dim treatyLimit As Double = gPMFunctions.ToSafeDouble(txtTreatyLimit.Text)

            ' Create sorted list of ranges
            Dim ranges As New List(Of Tuple(Of Double, Double))()
            For i As Integer = 0 To lvwVariableQS.Rows.Count - 1
                Dim lowerLimit As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(i).Cells("SALowerLimit").Value)
                Dim upperLimit As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(i).Cells("SAUpperLimit").Value)
                ranges.Add(New Tuple(Of Double, Double)(lowerLimit, upperLimit))
            Next

            ' Sort ranges by lower limit
            ranges.Sort(Function(x, y) x.Item1.CompareTo(y.Item1))

            ' Check for gaps between consecutive ranges
            For i As Integer = 0 To ranges.Count - 2
                If ranges(i).Item2 < ranges(i + 1).Item1 Then
                    MessageBox.Show("There is a missing range in the Quota Share treaty limits. Please adjust the ranges so there are no overlaps or gaps.", "Missing Range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
            Next

            ' Check if ranges cover up to treaty limit
            If ranges.Count > 0 AndAlso ranges(ranges.Count - 1).Item2 < treatyLimit Then
                MessageBox.Show("Missing range detected. The ranges do not cover up to the treaty limit of " & treatyLimit.ToString("N2") & ".", "Missing Range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If

            Return True
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="ValidateMissingRanges", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Validates that SA ranges do not overlap with each other or treaty limit
    ''' </summary>
    Private Function ValidateOverlappingRanges(lvwVariableQS As DataGridView) As Boolean
        Try
            If lvwVariableQS Is Nothing OrElse lvwVariableQS.Rows.Count = 0 Then
                Return True
            End If

            Dim treatyLimit As Double = gPMFunctions.ToSafeDouble(txtTreatyLimit.Text)

            ' Check each row against all other rows and treaty limit
            For i As Integer = 0 To lvwVariableQS.Rows.Count - 1
                Dim lowerLimit1 As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(i).Cells("SALowerLimit").Value)
                Dim upperLimit1 As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(i).Cells("SAUpperLimit").Value)

                ' Check if range overlaps with treaty limit
                'If lowerLimit1 < treatyLimit AndAlso upperLimit1 > treatyLimit Then
                '    'MessageBox.Show("Overlap range entered. The SA range in row " & (i + 1).ToString() & " overlaps with the treaty limit of " & treatyLimit.ToString("N2") & ".", "Overlapping Range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                '    MessageBox.Show("The treaty limit ranges exceed the overall treaty upper limit. Please adjust the ranges so they fall within the treaty limit and have no overlaps or gaps. ", "Overlapping Range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                '    Return False
                'End If

                ' Check against all other rows
                For j As Integer = i + 1 To lvwVariableQS.Rows.Count - 1
                    Dim lowerLimit2 As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(j).Cells("SALowerLimit").Value)
                    Dim upperLimit2 As Double = gPMFunctions.ToSafeDouble(lvwVariableQS.Rows(j).Cells("SAUpperLimit").Value)

                    ' Check for overlap: ranges overlap if one starts before the other ends
                    If (lowerLimit1 < upperLimit2 AndAlso upperLimit1 > lowerLimit2) Then
                        'MessageBox.Show("Overlap range entered. The SA ranges in row " & (i + 1).ToString() & " and row " & (j + 1).ToString() & " overlap.", "Overlapping Range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        MessageBox.Show("The treaty limits overlap. Please adjust the ranges to ensure there are no overlaps or gaps.", "Overlapping Range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End If
                Next
            Next

            Return True
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:="frmRIModelLine", v_sMethod:="ValidateOverlappingRanges", r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMError, excep:=ex)
            Return False
        End Try
    End Function
    ' ***************************************************************** '
    ' Validate Same Reinsurance Type Consistency
    ' ***************************************************************** '
    Private Function ValidateSameReinsuranceTypeConsistency(ByVal iReinsuranceTypeID As Integer, ByVal iPremiumCalculationBasis As Integer) As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim existingPremiumCalculationBasis As Integer = -1
        Dim hasExistingTreaty As Boolean = False

        Const kMethodName As String = "ValidateSameReinsuranceTypeConsistency"

        Try
            ' Check existing treaties of same reinsurance type (excluding current treaty being edited)
            If Information.IsArray(m_vRIModelLines) Then
                For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                    ' Skip current treaty being edited
                    If m_iTask = gPMConstants.PMEComponentAction.PMEdit And lCount = m_lSelectedIndex Then
                        Continue For
                    End If

                    ' Skip obligatory treaties
                    If gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Then
                        Continue For
                    End If

                    Dim existingRITypeID As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount))

                    ' Treat all surplus and quota share as one RI type
                    If IsSurplusOrQuotaShare(iReinsuranceTypeID) AndAlso IsSurplusOrQuotaShare(existingRITypeID) Then
                        If gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) > 0 Then
                            If Not hasExistingTreaty Then
                                existingPremiumCalculationBasis = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, lCount))
                                hasExistingTreaty = True
                            End If
                        End If
                    ElseIf existingRITypeID = iReinsuranceTypeID Then
                        If gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) > 0 Then
                            If Not hasExistingTreaty Then
                                existingPremiumCalculationBasis = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, lCount))
                                hasExistingTreaty = True
                            End If
                        End If
                    End If
                Next
            End If

            ' If no existing treaties of same type, allow any selection
            If Not hasExistingTreaty Then
                Return result
            End If

            ' If premium calculation basis differs from existing treaties of same type
            If existingPremiumCalculationBasis >= 0 And existingPremiumCalculationBasis <> iPremiumCalculationBasis Then
                Dim dialogResult As DialogResult = MessageBox.Show(
                    "The Premium Calculation Basis must be the same for all treaties under the same Reinsurance Type. Changing the premium calculation basis for this treaty will automatically update the basis for all previously added treaties of the same reinsurance type.",
                    "Premium Calculation Basis Consistency",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question)

                If dialogResult = DialogResult.OK Then
                    ' Update all existing treaties of same reinsurance type
                    UpdateSameReinsuranceTypePremiumCalculationBasis(iReinsuranceTypeID, iPremiumCalculationBasis)
                Else
                    ' User cancelled, focus back to premium calculation basis
                    cboPremiumCalculationBasis.Focus()
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            ElseIf hasExistingTreaty And existingPremiumCalculationBasis >= 0 Then
                ' Set current treaty to match existing treaties of same type
                cboPremiumCalculationBasis.ItemId = existingPremiumCalculationBasis
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Update Premium Calculation Basis for Same Reinsurance Type
    ' ***************************************************************** '
    Private Sub UpdateSameReinsuranceTypePremiumCalculationBasis(ByVal iReinsuranceTypeID As Integer, ByVal iPremiumCalculationBasis As Integer)
        Const kMethodName As String = "UpdateSameReinsuranceTypePremiumCalculationBasis"

        Try
            If Information.IsArray(m_vRIModelLines) Then
                For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                    ' Skip obligatory treaties
                    If gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Then
                        Continue For
                    End If

                    Dim existingRITypeID As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount))

                    ' Treat all surplus and quota share as one RI type
                    Dim shouldUpdate As Boolean = False
                    If IsSurplusOrQuotaShare(iReinsuranceTypeID) AndAlso IsSurplusOrQuotaShare(existingRITypeID) Then
                        shouldUpdate = True
                    ElseIf existingRITypeID = iReinsuranceTypeID Then
                        shouldUpdate = True
                    End If

                    If shouldUpdate AndAlso gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) > 0 Then
                        m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, lCount) = iPremiumCalculationBasis
                    End If
                Next
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=0, excep:=ex)
        End Try
    End Sub

    ' ***************************************************************** '
    ' Check if Reinsurance Type is Surplus or Quota Share
    ' ***************************************************************** '
    Private Function IsSurplusOrQuotaShare(ByVal iReinsuranceTypeID As Integer) As Boolean
        Return iReinsuranceTypeID = ACQuotaShare OrElse
               iReinsuranceTypeID = ACSurplus OrElse
               iReinsuranceTypeID = ACSecondSurplus OrElse
               iReinsuranceTypeID = ACThirdSurplus
        ' iReinsuranceTypeID = ACRetained
    End Function

    ' ***************************************************************** '
    ' Get Existing Premium Calculation Basis for Same Reinsurance Type
    ' ***************************************************************** '
    Private Function GetExistingPremiumCalculationBasisForSameType(ByVal iReinsuranceTypeID As Integer) As Integer
        Dim result As Integer = -1

        Try
            If Information.IsArray(m_vRIModelLines) Then
                For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                    ' Skip obligatory treaties
                    If gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Then
                        Continue For
                    End If

                    Dim existingRITypeID As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount))

                    ' Check if same reinsurance type (treat surplus and quota share as one type)
                    Dim isSameType As Boolean = False
                    If IsSurplusOrQuotaShare(iReinsuranceTypeID) AndAlso IsSurplusOrQuotaShare(existingRITypeID) Then
                        isSameType = True
                    ElseIf existingRITypeID = iReinsuranceTypeID Then
                        isSameType = True
                    End If

                    If isSameType AndAlso gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) > 0 Then
                        Dim calcBasis As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, lCount))
                        If calcBasis > 0 Then
                            result = calcBasis
                            Exit For
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            result = -1
        End Try

        Return result
    End Function
End Class