Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmParameters
    Inherits System.Windows.Forms.Form
    '
    ' History:
    ' CJB 160805 PN23199 Changed Form_QueryUnload to set Status to Cancel if OK not clicked to prevent
    '            processing carrying on in calling form.
    '
    'Developer Guide No. 69
    Public frmUsers As frmUsers
    'Developer Guide No. 69
    Public frmInterface As frmInterface
    Private Const ACClass As String = "frmParameters"

    'DC290202
    Private Const m_sCallingAppName As String = "ReportPrint"
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_lReturn As Integer
    Private m_iStatus As gPMConstants.PMEReturnCode

    Private m_sReportName As String = ""
    'Developer Guide No. 101
    Private m_vParameters As Object

    Private m_vDefaultValues As Object
    Private m_sUniqueReportName As String = ""

    'DC270303 -ISS1911 -Start
    Private m_vRisks(,) As Object
    Private m_vGroups(,) As Object
    Private m_bMultiUsers As Boolean
    Private m_vUserNames() As Object
    Private m_vUsers() As Object
    Private m_ifrmUserStatus As gPMConstants.PMEReturnCode
    Private m_lSessionId As Integer

    Private m_vAccountExecutives(,) As Object
    Private m_vAccountHandlers(,) As Object
    Private m_vInsurers(,) As Object
    Private m_vThirdParties(,) As Object
    Private m_vBranches(,) As Object

    'Declare an instance of the general interface object.
    Private m_oGeneral As iPMBReportPrint.General
    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_bAllBranches As Boolean

    Private m_bAttachToScheduler As Boolean
    Private m_sFrequency As String = ""
    Private Const nSourceId As Integer = 1

    Dim count As Integer ' --Ritu

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property
    'DC270303 -ISS1911 -End
    Public Property Parameters() As Object
        Get

            Return VB6.CopyArray(m_vParameters)

        End Get
        Set(ByVal Value As Object)

            m_vParameters = Value

        End Set
    End Property
    'Developer Guide No 101
    Public WriteOnly Property DefaultValues() As Object
        Set(ByVal Value As Object)

            m_vDefaultValues = Value

        End Set
    End Property

    Public WriteOnly Property ReportName() As String
        Set(ByVal Value As String)

            m_sReportName = Value

        End Set
    End Property

    Public WriteOnly Property UniqueReportName() As String
        Set(ByVal Value As String)

            m_sUniqueReportName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            Return m_iStatus

        End Get
    End Property

    'DC270303 -ISS1911 -Start
    Public Property SessionId() As Integer
        Get

            Return m_lSessionId

        End Get
        Set(ByVal Value As Integer)

            m_lSessionId = Value

        End Set
    End Property
    'DC270303 -ISS1911 -End
    '8.5
    Public Property AttachToScheduler() As Boolean
        Get
            Return m_bAttachToScheduler
        End Get
        Set(ByVal Value As Boolean)
            m_bAttachToScheduler = Value
        End Set
    End Property

    Public Property Frequency() As String
        Get
            Return m_sFrequency
        End Get
        Set(ByVal Value As String)
            m_sFrequency = Value
        End Set
    End Property

    Public WriteOnly Property Business() As Object
        Set(ByVal value As Object)
            m_oBusiness = value
        End Set
    End Property

    'DC270303 -ISS1911 -Start
    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC270303 -ISS1911 -End

    'DC270303 -ISS1911 -Start
    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the parameter details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC270303 -ISS1911 -End

    ' ***************************************************************** '
    ' Name: GetParamFields
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetParamFields() As Integer

        Dim result As Integer = 0
        Dim bFound As Boolean
        Dim lControlCount, lDefaultCount As Integer
        Dim sDefaultValue As String = ""
        Dim dtDate As Date
        Dim lAdjust As Integer
        Dim sUnderwritingYear As String = ""
        Dim bContinue As Boolean
        Dim lIndex As Integer
        Dim bAddValue As Boolean
        Dim bIsTPVisible As Boolean
        Dim bIsDTParameters(14) As Boolean
        Dim bIsAllBranches As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lFraTypeCount As Integer = fraType.GetUpperBound(0) To 1 Step -1
                ContainerHelper.UnloadControl(Me, "lblIncluded", lFraTypeCount)
                ContainerHelper.UnloadControl(Me, "lvwIncluded", lFraTypeCount)
                ContainerHelper.UnloadControl(Me, "lblExcluded", lFraTypeCount)
                ContainerHelper.UnloadControl(Me, "lvwExcluded", lFraTypeCount)
                ContainerHelper.UnloadControl(Me, "cmdAddIncluded", lFraTypeCount)
                ContainerHelper.UnloadControl(Me, "cmdAddAllIncluded", lFraTypeCount)
                ContainerHelper.UnloadControl(Me, "cmdDeleteExcluded", lFraTypeCount)
                ContainerHelper.UnloadControl(Me, "cmdDeleteAllExcluded", lFraTypeCount)
                ContainerHelper.UnloadControl(Me, "fraType", lFraTypeCount)

                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                SSTabHelper.SetTabs(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
            Next lFraTypeCount

            SSTabHelper.SetTabVisible(tabMainTab, 1, False)

            '8.5
            If AttachToScheduler Then
                lblFrequencyPrompt.Visible = True
                lblFrequencyType.Visible = True
                cboFrequency.Visible = True
                cboFrequency.FirstItem = "(None)"
            Else
                lblFrequencyPrompt.Visible = False
                lblFrequencyType.Visible = False
                cboFrequency.Visible = False
                For lControlCount = 0 To (lblParameterName.GetUpperBound(0))
                    lblParameterName(lControlCount).Top -= VB6.TwipsToPixelsY(480)
                    lblParameterType(lControlCount).Top -= VB6.TwipsToPixelsY(480)
                    DTParameters(lControlCount).Top -= VB6.TwipsToPixelsY(480)
                    chkGroupBy(lControlCount).Top -= VB6.TwipsToPixelsY(480)
                    cboParameterValues(lControlCount).Top -= VB6.TwipsToPixelsY(480)
                    cmdParameterValues(lControlCount).Top -= VB6.TwipsToPixelsY(480)
                    List1(lControlCount).Top -= VB6.TwipsToPixelsY(480)
                Next
            End If

            ' Clear all captions
            For lControlCount = 0 To (lblParameterName.GetUpperBound(0))
                lblParameterName(lControlCount).Text = ""
            Next lControlCount

            ' Set form fields
            lControlCount = -1
            Dim vAccountingPeriods(,) As Object = Nothing
            For lParamCount As Integer = 0 To m_vParameters.GetUpperBound(0)
                ' Not required if prompt suppressed
                If (m_vParameters(lParamCount, 3) IsNot Nothing AndAlso CStr(m_vParameters(lParamCount, 3)).Trim() <> "") Then
                    ' Check not already in array
                    bFound = False
                    For lArrayCount As Integer = 0 To lParamCount - 1
                        If m_vParameters(lArrayCount, 0).Equals(m_vParameters(lParamCount, 0)) Then
                            bFound = True
                            Exit For
                        End If
                    Next lArrayCount
                    If Not bFound Then

                        bContinue = True

                        'check if parameters required for Exclude Types
                        If CStr(m_vParameters(lParamCount, 3)) = "INCEXC AE" Or CStr(m_vParameters(lParamCount, 3)) = "INCEXC AH" Or CStr(m_vParameters(lParamCount, 3)) = "INCEXC RC" Or CStr(m_vParameters(lParamCount, 3)) = "INCEXC IN" Or CStr(m_vParameters(lParamCount, 3)) = "INCEXC TP" Or CStr(m_vParameters(lParamCount, 3)) = "INCEXC RG" Or CStr(m_vParameters(lParamCount, 3)) = "INCEXC BR" Then

                            m_lReturn = CreateTab(SSTabHelper.GetTabCount(tabMainTab), CStr(m_vParameters(lParamCount, 3)))

                            m_lReturn = PopulateTypeOfIncludeLists(CStr(m_vParameters(lParamCount, 3)), fraType.GetUpperBound(0))

                            bContinue = False
                        End If

                        If CStr(m_vParameters(lParamCount, 3)) = "Underwriting Year:" Then
                            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, v_vBranch:=1, r_vUnderwriting:=sUnderwritingYear)
                            If sUnderwritingYear <> "1" Then
                                m_vParameters(lParamCount, 1) = "<ALL>"
                                bContinue = False
                            End If
                        End If

                        If bContinue Then

                            lControlCount += 1

                            'check for grouping
                            If CStr(m_vParameters(lParamCount, 3)).StartsWith("GROUP BY ") Then
                                lblGroupBy.Visible = True
                                chkGroupBy(lControlCount).Visible = True
                                chkGroupBy(lControlCount).CheckState = False
                                m_vParameters(lParamCount, 3) = CStr(m_vParameters(lParamCount, 3)).Substring(9, Math.Min(CStr(m_vParameters(lParamCount, 3)).Length, 50))
                            End If

                            If CStr(m_vParameters(lParamCount, 5)) <> "" Then
                                cmdParameterValues(lControlCount).Visible = True
                                cmdParameterValues(lControlCount).Enabled = True
                                cmdParameterValues(lControlCount).Tag = CStr(m_vParameters(lParamCount, 5))
                            End If

                            'DC270303 -ISS1911 -Start
                            'Display the command button and populate users to pass to the user form if neccessary
                            If CStr(m_vParameters(lParamCount, 3)) = "User Names:" Then
                                m_bMultiUsers = True
                                cmdParameterValues(lControlCount).Visible = True
                                cmdParameterValues(lControlCount).Enabled = True
                                ReDim m_vUserNames(m_vDefaultValues.GetUpperBound(1) - 1)
                                ReDim m_vUsers(m_vDefaultValues.GetUpperBound(1) - 1)
                                For lDefaultCount = 1 To m_vDefaultValues.GetUpperBound(1)
                                    m_vUserNames(lDefaultCount - 1) = m_vDefaultValues(lParamCount, lDefaultCount)
                                    m_vUsers(lDefaultCount - 1) = m_vDefaultValues(lParamCount, lDefaultCount)
                                Next lDefaultCount
                            End If
                            'DC270303 -ISS1911 -end
                            'PN15830
                            If CStr(m_vParameters(lParamCount, 3)) = "Document Reference:" Or CStr(m_vParameters(lParamCount, 3)) = "Policy" Then
                                cmdParameterValues(lControlCount).Visible = True
                                cmdParameterValues(lControlCount).Enabled = False
                            End If
                            'PN15830End
                            'PN16139
                            If CStr(m_vParameters(lParamCount, 3)) = "Client Start Code:" Or CStr(m_vParameters(lParamCount, 3)) = "Client End Code" Or CStr(m_vParameters(lParamCount, 3)) = "Insured" Then
                                cmdParameterValues(lControlCount).Visible = True
                                cmdParameterValues(lControlCount).Enabled = False
                            End If

                            'PN16139End
                            If CStr(m_vParameters(lParamCount, 3)) = "Elapsed days:" Then
                                cmdParameterValues(lControlCount).Visible = True
                                cmdParameterValues(lControlCount).Enabled = False
                            End If
                            'Batch Renewal
                            If CStr(m_vParameters(lParamCount, 3)) = "Policy Number:" Then
                                cmdParameterValues(lControlCount).Visible = True
                                cmdParameterValues(lControlCount).Enabled = False
                            End If
                            If CStr(m_vParameters(lParamCount, 3)) = "TPACode" AndAlso m_sReportName = "Claims\Outstanding_Claims" Then
                                lblParameterName(lControlCount).Visible = False
                                cboParameterValues(lControlCount).Visible = False
                                lblParameterType(lControlCount).Visible = False
                            End If
                            If CStr(m_vParameters(lParamCount, 3)) = "party_cnt:" AndAlso m_sReportName = "Navigator\Client_Statement_By_PartyCnt_U" Then
                                lblParameterName(lControlCount).Visible = False
                                cboParameterValues(lControlCount).Visible = False
                                lblParameterType(lControlCount).Visible = False
                            End If

                            ' Set Prompt
                            lblParameterName(lControlCount).Text = CStr(m_vParameters(lParamCount, 3))
                            ' Add prompt according to parameter type
                            Select Case m_vParameters(lParamCount, 2)
                                Case VariantType.Short, VariantType.Integer
                                    lblParameterType(lControlCount).Text = "(Integer)"
                                Case VariantType.Single, VariantType.Double
                                    lblParameterType(lControlCount).Text = "(Number)"
                                Case VariantType.Decimal
                                    lblParameterType(lControlCount).Text = "(Currency)"
                                Case VariantType.Boolean
                                    lblParameterType(lControlCount).Text = "(True/False)"
                                Case VariantType.Date
                                    lblParameterType(lControlCount).Text = "(Date)"
                                Case VariantType.String
                                    lblParameterType(lControlCount).Text = "(Text)"
                            End Select
                            cboParameterValues(lControlCount).Text = CStr(m_vParameters(lParamCount, 1))
                            cboParameterValues(lControlCount).Tag = CStr(m_vParameters(lParamCount, 2))
                            'End If

                            ' Add default date values
                            sDefaultValue = CStr(m_vParameters(lParamCount, 0)).ToLower()
                            If (sDefaultValue.IndexOf("start date") + 1) Or (sDefaultValue.IndexOf("start_date") + 1) Then

                                If m_vDefaultValues.GetUpperBound(1) < 3 Then
                                    ReDim Preserve m_vDefaultValues(m_vDefaultValues.GetUpperBound(0), 3)
                                End If

                                ' Today, 1st of This Month, Last Month, This Year
                                ' Today
                                lDefaultCount = 0
                                dtDate = DateTime.Now
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.ToString("MMM dd yyyy")
                                ' 1st of this month
                                If DateAndTime.Day(dtDate) <> 1 Then
                                    lDefaultCount += 1
                                    dtDate = dtDate.AddDays((DateAndTime.Day(dtDate) - 1) * -1)
                                    m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.ToString("MMM dd yyyy")
                                End If
                                ' 1st of last month
                                If dtDate.Month <> 1 Then
                                    lDefaultCount += 1
                                    dtDate = dtDate.AddMonths(-1)
                                    m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.ToString("MMM dd yyyy")
                                End If
                                ' 1st of this year
                                If dtDate.Month <> 1 Then
                                    lDefaultCount += 1
                                    dtDate = dtDate.AddMonths((dtDate.Month - 1) * -1)
                                    m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.ToString("MMM dd yyyy")
                                End If

                                'Today, End of This Month, Last Month, Last Year
                            ElseIf ((sDefaultValue.IndexOf("end date") + 1) Or (sDefaultValue.IndexOf("end_date") + 1) Or (sDefaultValue.IndexOf("to_date") + 1) Or (sDefaultValue.IndexOf("todate") + 1)) Then

                                If m_vDefaultValues.GetUpperBound(1) < 3 Then
                                    ReDim Preserve m_vDefaultValues(m_vDefaultValues.GetUpperBound(0), 3)
                                End If

                                If sDefaultValue = "period_end_date" Or ((m_sReportName.ToUpper = ACRptName_AgencyDebitingBordereau Or m_sReportName.ToUpper = ACRptName_AgencyPaidBordereau) And sDefaultValue = "end_date") Then

                                    m_lReturn = m_oBusiness.GetAccountingPeriods(nSourceId, 4, vAccountingPeriods)

                                    If Information.IsArray(vAccountingPeriods) Then

                                        For lDefaultCount = vAccountingPeriods.GetLowerBound(1) To vAccountingPeriods.GetUpperBound(1)

                                            dtDate = CDate(vAccountingPeriods(0, lDefaultCount))
                                            m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.ToString("MMM dd yyyy")
                                        Next
                                    End If
                                Else
                                    ' Today
                                    lDefaultCount = 0
                                    dtDate = DateTime.Now
                                    m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.ToString("MMM dd yyyy")
                                    ' End of this month
                                    If DateAndTime.Day(dtDate.AddDays(1)) <> 1 Then
                                        lDefaultCount += 1
                                        dtDate = dtDate.AddMonths(1)
                                        dtDate = dtDate.AddDays((DateAndTime.Day(dtDate)) * -1)
                                        m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.ToString("MMM dd yyyy")
                                    End If
                                    ' End of previous month
                                    lDefaultCount += 1
                                    dtDate = dtDate.AddDays((DateAndTime.Day(dtDate)) * -1)
                                    m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.ToString("MMM dd yyyy")
                                    ' End of last year
                                    If dtDate.Month <> 12 Then
                                        lDefaultCount += 1
                                        dtDate = dtDate.AddMonths((dtDate.Month - 1) * -1)
                                        dtDate = dtDate.AddDays((DateAndTime.Day(dtDate)) * -1)
                                        m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.ToString("MMM dd yyyy")
                                    End If
                                End If

                                'Current Year, and Last 4 calendar years
                            ElseIf ((sDefaultValue.IndexOf("year 1") + 1) Or (sDefaultValue.IndexOf("year_1") + 1) Or (sDefaultValue.IndexOf("calenderyear") + 1)) Then

                                If m_vDefaultValues.GetUpperBound(1) < 4 Then
                                    ReDim Preserve m_vDefaultValues(m_vDefaultValues.GetUpperBound(0), 4)
                                End If

                                ' Current Calendar Year
                                lDefaultCount = 0
                                dtDate = DateTime.Now
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year
                                ' Current Calendar Year -1
                                lDefaultCount += 1
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year - 1

                                ' Current Calendar Year -2
                                lDefaultCount += 1
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year - 2

                                ' Current Calendar Year -3
                                lDefaultCount += 1
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year - 3

                                ' Current Calendar Year -4
                                lDefaultCount += 1
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year - 4

                                'Current Year, and Last 4 calendar years
                            ElseIf ((sDefaultValue.IndexOf("year 2") + 1) Or (sDefaultValue.IndexOf("year_2") + 1)) Then

                                If m_vDefaultValues.GetUpperBound(1) < 4 Then
                                    ReDim Preserve m_vDefaultValues(m_vDefaultValues.GetUpperBound(0), 4)
                                End If

                                ' Current Calendar Year
                                lDefaultCount = 0
                                dtDate = DateTime.Now
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year
                                ' Current Calendar Year -1
                                lDefaultCount += 1
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year - 1

                                ' Current Calendar Year -2
                                lDefaultCount += 1
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year - 2

                                ' Current Calendar Year -3
                                lDefaultCount += 1
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year - 3

                                ' Current Calendar Year -4
                                lDefaultCount += 1
                                m_vDefaultValues(lParamCount, lDefaultCount) = dtDate.Year - 4

                            End If

                            ' Set True/False for Boolean
                            If m_vParameters(lParamCount, 2) = VariantType.Boolean Then
                                For lDefaultCount = 0 To 1
                                    If CDbl(m_vDefaultValues(lParamCount, lDefaultCount)) = 0 Then
                                        m_vDefaultValues(lParamCount, lDefaultCount) = "False"
                                    Else
                                        m_vDefaultValues(lParamCount, lDefaultCount) = "True"
                                    End If
                                Next lDefaultCount
                            End If

                            ' TB stop exceeds dropdown limit error
                            If m_vDefaultValues.GetUpperBound(1) > ACDropDownLimit Then
                                g_lDDLimit = ACDropDownLimit
                                ' Tell user values may be missing.
                                MessageBox.Show("Control Limit Exceeded " & Strings.Chr(13) & Strings.Chr(10) & "Not all Values for the " & "xxx" & " list may be present", Application.ProductName)
                            Else
                                g_lDDLimit = m_vDefaultValues.GetUpperBound(1)
                            End If
                            ' Add default values
                            For lDefaultCount = 0 To g_lDDLimit
                                sDefaultValue = CStr(m_vDefaultValues(lParamCount, lDefaultCount))

                                If Not (Convert.IsDBNull(sDefaultValue) Or IsNothing(sDefaultValue)) Then
                                    If sDefaultValue.Trim() > "" Then

                                        bAddValue = True

                                        'PN16144 Exception for Manual Remittance
                                        'If branch parameter, display "<ALL BRANCHES>" instead of "<ALL>"
                                        If CStr(m_vParameters(lParamCount, 0)) = "Branch" Then
                                            If frmInterface.AllowAll Or frmInterface.AllBranch = "1" Then
                                                If sDefaultValue = "<ALL>" Then
                                                    If m_sReportName = "General\ManualRemittanceAdvice" Then
                                                        sDefaultValue = ""
                                                    Else
                                                        'If <ALL> is in the list then it will be initially selected.
                                                        m_lReturn = m_oBusiness.GetPMUserSource(r_bIsAllBranches:=bIsAllBranches, iUserId:=g_iUserID)

                                                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                            GetParamFields = gPMConstants.PMEReturnCode.PMFalse
                                                            Exit Function
                                                        End If

                                                        If bIsAllBranches = True Then
                                                            m_bAllBranches = True
                                                            sDefaultValue = "<ALL BRANCHES>"
                                                        Else
                                                            sDefaultValue = ""
                                                        End If
                                                    End If

                                                End If
                                            Else
                                                If sDefaultValue = "<ALL>" Then
                                                    If m_sReportName = "General\ManualRemittanceAdvice" Then
                                                        sDefaultValue = ""
                                                    Else
                                                        'If <ALL> is in the list then it will be initially selected.
                                                        m_bAllBranches = True
                                                        sDefaultValue = ""
                                                    End If

                                                End If
                                            End If
                                        End If

                                        If CStr(m_vParameters(lParamCount, 3)).ToUpper() = ACLabelTypeOfCurrency.ToUpper() Then
                                            If Not (sDefaultValue.IndexOf(ACRestrictionCodeBranchSelectedOnly) >= 0 And m_bAllBranches) Then
                                                Select Case sDefaultValue.Substring(0, 1)
                                                    Case "A"
                                                        sDefaultValue = ACTypeOfCurrencyAccount
                                                    Case "B"
                                                        sDefaultValue = ACTypeOfCurrencyBase
                                                    Case "S"
                                                        sDefaultValue = ACTypeOfCurrencySystem
                                                    Case "T"
                                                        sDefaultValue = ACTypeOfCurrencyTransaction
                                                End Select
                                            Else
                                                bAddValue = False
                                            End If
                                        End If

                                        If CStr(m_vParameters(lParamCount, 3)) = ACLabelLookUpTable And sDefaultValue = "<ALL>" Then
                                            bAddValue = False
                                        End If

                                        If bAddValue Then
                                            If m_sReportName = "General\Third_party_Details" Then
                                                bIsTPVisible = False
                                                If (Strip(sDefaultValue).IndexOf("All") + 1) = 0 Then

                                                    m_lReturn = m_oBusiness.GetTPVisibility(sTPType:=Strip(sDefaultValue), r_bTPVisible:=bIsTPVisible)
                                                    If bIsTPVisible Then
                                                        Dim cboParameterValues_NewIndex As Integer = -1
                                                        cboParameterValues_NewIndex = cboParameterValues(lControlCount).Items.Add(sDefaultValue.Trim())
                                                        VB6.SetItemData(cboParameterValues(lControlCount), cboParameterValues_NewIndex, lDefaultCount)
                                                    Else
                                                        ' Don't add to the combo
                                                    End If
                                                Else
                                                    'Developer Guide No. 153
                                                    cboParameterValues(lControlCount).Items.Add(New VB6.ListBoxItem(sDefaultValue.Trim(), lDefaultCount))
                                                End If
                                            Else
                                                If sDefaultValue.StartsWith("DatePickerCombo") Then
                                                    DTParameters(lControlCount).Visible = True
                                                    DTParameters(lControlCount).Value = DateTime.Now
                                                    'DTParameters(lControlCount).Value = CDate("")
                                                    lblParameterType(lControlCount).Text = "(Date)"
                                                    bIsDTParameters(lControlCount) = True
                                                    cboParameterValues(lControlCount).Visible = False
                                                    cboParameterValues(lControlCount).TabStop = False
                                                    cboParameterValues(lControlCount).Text = "Combo1"
                                                    List1(lControlCount).Visible = False
                                                Else
                                                    If m_vParameters(lParamCount, 10) = True Then
                                                        'this is a multi select - we don't want to see the combo
                                                        List1(lControlCount).Items.Add(New VB6.ListBoxItem(sDefaultValue.Trim(), lDefaultCount))
                                                        List1(lControlCount).Left = cboParameterValues(lControlCount).Left
                                                        List1(lControlCount).Top = cboParameterValues(lControlCount).Top
                                                        List1(lControlCount).Visible = True
                                                        'hide the drop down
                                                        cboParameterValues(lControlCount).Visible = False
                                                        cboParameterValues(lControlCount).TabStop = False
                                                        cboParameterValues(lControlCount).Text = "Combo1"
                                                    Else
                                                        'hide the multi select and run as normal combo
                                                        List1(lControlCount).Visible = False
                                                        If Not String.IsNullOrEmpty(sDefaultValue) Then
                                                            cboParameterValues(lControlCount).Items.Add(New VB6.ListBoxItem(sDefaultValue.Trim(), lDefaultCount))
                                                        End If
                                                    End If

                                                    '-------- PN 1280 and 1468-----------
                                                    If lblParameterType(lControlCount).Text = "(Date)" Then
                                                        cboParameterValues(lControlCount).SelectedIndex = "0"
                                                    End If
                                                    '-------------------------------------

                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Next lDefaultCount
                        End If
                    End If
                End If
            Next lParamCount

            ' SET 25112002 ISS1317 - Re size the form to suit the number of visible controls
            'DC190302
            ''DC270303 -ISS1911 - was 11
            If AttachToScheduler Then
                lAdjust = ((14 - (lControlCount + 2)) * 480)
            Else
                lAdjust = ((14 - (lControlCount + 1)) * 480)
            End If

            If lAdjust > 2400 Then
                lAdjust = 480
            End If

            Me.Height = VB6.TwipsToPixelsY(9120 - lAdjust) 'DC270303 -ISS1911 -was 8100 - lAdjust
            tabMainTab.Height = VB6.TwipsToPixelsY(7935 - lAdjust + 50) ''DC270303 -ISS1911 -was 7095 - lAdjust
            fraParameters.Height = VB6.TwipsToPixelsY(7215 - lAdjust - 50) ''DC270303 -ISS1911 -was 6375 - lAdjust

            cmdOK.Top = VB6.TwipsToPixelsY(8160 - lAdjust) ''DC270303 -ISS1911 - was 7260 - lAdjust
            cmdCancel.Top = VB6.TwipsToPixelsY(8160 - lAdjust) ''DC270303 -ISS1911 - was 7260 - lAdjust
            ' SET 25112002 - End

            If AttachToScheduler Then
                cmdAddToScheduler.Visible = True
                cmdAddToScheduler.BringToFront()
                cmdAddToScheduler.Top = VB6.TwipsToPixelsY(8160 - lAdjust)
            End If

            For lFraTypeCount As Integer = 0 To fraType.GetUpperBound(0)
                fraType(lFraTypeCount).Height = VB6.TwipsToPixelsY(7215 - lAdjust - 50)
                lvwIncluded(lFraTypeCount).Height = VB6.TwipsToPixelsY(6105 - lAdjust)
                lvwExcluded(lFraTypeCount).Height = VB6.TwipsToPixelsY(6105 - lAdjust)
                cmdDeleteExcluded(lFraTypeCount).Top = VB6.TwipsToPixelsY(5880 - lAdjust)
                cmdDeleteAllExcluded(lFraTypeCount).Top = VB6.TwipsToPixelsY(6375 - lAdjust)
            Next lFraTypeCount

            iPMFunc.CenterForm(Me)

            'Set starting index value (0, 1 and 2 are OK, Cancel and the tabs)
            lIndex = 3
            For lControlCount = 0 To (lblParameterName.GetUpperBound(0))
                If lblParameterName(lControlCount).Text = "" Then
                    'Hide unused controls
                    lblParameterName(lControlCount).Visible = False
                    lblParameterType(lControlCount).Visible = False
                    cboParameterValues(lControlCount).Visible = False

                    'Disable tabbing for unused controls
                    cboParameterValues(lControlCount).TabStop = False
                    cmdParameterValues(lControlCount).TabStop = False
                Else
                    If Not bIsDTParameters(lControlCount) Then
                        'Set tab order for the displayed controls
                        cboParameterValues(lControlCount).TabIndex = lIndex
                        cboParameterValues(lControlCount).TabStop = True
                        lIndex += 1

                        If cmdParameterValues(lControlCount).Enabled Then
                            cmdParameterValues(lControlCount).TabIndex = lIndex
                            cmdParameterValues(lControlCount).TabStop = True
                            lIndex += 1
                        End If

                        'If the label has the words Period and date in them then don't default to today
                        If Not ((lblParameterName(lControlCount).Text.ToUpper().IndexOf("PERIOD") + 1) >= 1 And (lblParameterName(lControlCount).Text.ToUpper().IndexOf("DATE") + 1) >= 1) Then
                            If cboParameterValues(lControlCount).Items.Count > 0 Then
                                cboParameterValues(lControlCount).SelectedIndex = 0
                            End If
                        ElseIf (m_sReportName.ToUpper = ACRptName_AgencyDebitingBordereau Or m_sReportName.ToUpper = ACRptName_AgencyPaidBordereau) And lblParameterName(lControlCount).Text.ToUpper().IndexOf("PERIOD END DATE") + 1 > 1 Then
                            If cboParameterValues(lControlCount).Items.Count > 0 Then
                                cboParameterValues(lControlCount).SelectedIndex = 0
                            End If
                        End If
                    End If

                End If
            Next lControlCount

            'If the other two tabs are not showing then disable the frames on them to
            'prevent the tabbing from using the controls on them.
            fraType(0).Enabled = SSTabHelper.GetTabVisible(tabMainTab, 1)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParamfieldsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Getparamfields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboParameterValues_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cboParameterValues_11.SelectedIndexChanged, _cboParameterValues_6.SelectedIndexChanged, _cboParameterValues_5.SelectedIndexChanged, _cboParameterValues_4.SelectedIndexChanged, _cboParameterValues_3.SelectedIndexChanged, _cboParameterValues_2.SelectedIndexChanged, _cboParameterValues_1.SelectedIndexChanged, _cboParameterValues_0.SelectedIndexChanged, _cboParameterValues_7.SelectedIndexChanged, _cboParameterValues_8.SelectedIndexChanged, _cboParameterValues_9.SelectedIndexChanged, _cboParameterValues_10.SelectedIndexChanged, _cboParameterValues_12.SelectedIndexChanged, _cboParameterValues_13.SelectedIndexChanged
        Dim Index As Integer = Array.IndexOf(cboParameterValues, eventSender)
        'index = Array.IndexOf(cboParameterValues, eventSender)
        Dim sDefaultValue As String = ""
        Dim bOldAllBranches As Boolean
        Dim bFoundChild As Boolean
        Dim lLoop As Integer = 0
        count = 0

        If lblParameterName(Index).Text = ACLabelBranch Then

            bOldAllBranches = m_bAllBranches

            m_bAllBranches = cboParameterValues(Index).Text.Substring(0, 2) = "<ALL"
           
            'If combo has changed from or to showing all branches then refresh the type of currency combo.
            If m_bAllBranches <> bOldAllBranches Then
                m_lReturn = PopulateTypeOfCurrencyList()
            End If
        End If

        If lblParameterName(Index).Text.ToUpper() = ACLabelTypeOfCurrency.ToUpper() Then

            For lLoop = 0 To m_vParameters.GetUpperBound(0)
                If CStr(m_vParameters(lLoop, 3)).ToUpper() = ACLabelTypeOfCurrency.ToUpper() Then

                    If cboParameterValues(Index).SelectedIndex <> -1 Then
                        sDefaultValue = CStr(m_vDefaultValues(lLoop, cboParameterValues(Index).SelectedIndex))
                        'Strip out nulls
                        If sDefaultValue.IndexOf(Strings.Chr(0).ToString()) >= 0 Then
                            sDefaultValue = sDefaultValue.Substring(0, sDefaultValue.IndexOf(Strings.Chr(0).ToString()))
                        End If

                        m_lReturn = PopulateGroupByList(sDefaultValue)
                    End If
                    Exit For
                End If
            Next

        End If

        '----If "currency type" parameter is "Base", then disable the "Currency Name" parameter with "ALL" as default value. .....-PN 1517
        If cboParameterValues(Index).SelectedItem.ToString = "Base" And lblParameterName(Index + 1).Text = "Select Currency Name :" Then

            cboParameterValues(Index + 1).SelectedIndex() = "0"
            cboParameterValues(Index + 1).Enabled() = False
            Exit Sub
        Else
            If lblParameterName(Index + 1).Text = "Select Currency Name :" Then
                cboParameterValues(Index + 1).Enabled() = True
            End If
        End If

        'count how many parameters are there in report which are not shown in the running report,ie,the parameters present in 'm_vParameters' array and not in 'cboParamaters'array.
        ' Count the number and decrease that count in the 'cboParameter'array index(wherever required). Becoz mismatch occurs while comparing the values from both the arrays. '--Ritu
        count = 0
        For lParamCount As Integer = 0 To m_vParameters.GetUpperBound(0)
            If m_vParameters(lParamCount, 3) <> Nothing Then
                Exit For
            Else
                count = count + 1
            End If
        Next

        'loop round all the params finding any that depend on this one
        For lLoop = 0 To UBound(m_vParameters, 1)
            Dim param, param1 As String
            If m_vParameters(lLoop, 6) <> Nothing Then
                param = CStr(m_vParameters(lLoop, 6)).ToUpper
            Else
                param = m_vParameters(lLoop, 6)
            End If

            ''If (count <= Index) Then
            If m_vParameters(Index + count, 0) <> Nothing Then
                param1 = CStr(m_vParameters(Index + count, 0)).ToUpper
            Else
                param1 = m_vParameters(Index + count, 0)
            End If

            If param = param1 Then
                bFoundChild = True
                Exit For
            End If
            ''End If
        Next
        '-----------------------------------------------------------------------------------------
        If bFoundChild Then
            'we need to filter the child list - if ALL is found in parent then this will clear the filter
            m_lReturn = FilterChild(Index, lLoop)
        End If
        count = 0
    End Sub

    Private Sub cboParameterValues_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles _cboParameterValues_11.KeyPress, _cboParameterValues_6.KeyPress, _cboParameterValues_5.KeyPress, _cboParameterValues_4.KeyPress, _cboParameterValues_3.KeyPress, _cboParameterValues_2.KeyPress, _cboParameterValues_1.KeyPress, _cboParameterValues_0.KeyPress, _cboParameterValues_7.KeyPress, _cboParameterValues_8.KeyPress, _cboParameterValues_9.KeyPress, _cboParameterValues_10.KeyPress, _cboParameterValues_12.KeyPress, _cboParameterValues_13.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim Index As Integer = Array.IndexOf(cboParameterValues, eventSender)

        Try

            If CInt(Convert.ToString(cboParameterValues(Index).Tag)) <> VariantType.Date And Not cmdParameterValues(Index).Visible Then

                ' We stop user from typing if it's not a date and
                ' there's no "browse" button (he can pick from a list)
                KeyAscii = 0

            End If

            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub

        Catch
        End Try

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cboParameterValues_KeyPress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cboParameterValues_KeyPress", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cmdAddAllIncluded_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdAddAllIncluded_0.Click
        Dim Index As Integer = Array.IndexOf(cmdAddAllIncluded, eventSender)
        Dim oListItem As ListViewItem

        Try
            For iRow As Integer = 1 To lvwExcluded(Index).Items.Count
                If Convert.ToString(lvwExcluded(Index).Items.Item(iRow - 1).Tag) = 0 Then

                    lvwExcluded(Index).Items.RemoveAt(iRow - 1)
                    oListItem = lvwIncluded(Index).Items.Add(lvwExcluded(Index).Items.Item(iRow - 1).Name, lvwExcluded(Index).Items.Item(iRow - 1).Text, "")
                    With oListItem

                        .Tag = Convert.ToString(lvwExcluded(Index).Items.Item(iRow - 1).Tag)
                    End With
                End If
            Next iRow

            For iRow As Integer = lvwIncluded(Index).Items.Count To 1 Step -1
                If Convert.ToString(lvwIncluded(Index).Items.Item(iRow - 1).Tag) <> 0 Then
                    oListItem = lvwExcluded(Index).Items.Add(lvwIncluded(Index).Items.Item(iRow - 1).Name, lvwIncluded(Index).Items.Item(iRow - 1).Text, "")
                    With oListItem

                        .Tag = Convert.ToString(lvwIncluded(Index).Items.Item(iRow - 1).Tag)
                    End With

                    lvwIncluded(Index).Items.RemoveAt(iRow - 1)
                End If
            Next iRow

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Err_cmdAddAllIncluded_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_cmdAddAllIncluded_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub

    Private Sub cmdAddIncluded_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdAddIncluded_0.Click
        Dim Index As Integer = Array.IndexOf(cmdAddIncluded, eventSender)
        Dim oListItem As ListViewItem
        Dim bIsFound As Boolean
        Try

            Select Case Convert.ToString(lvwIncluded(Index).FocusedItem.Tag)
                Case 0
                    cmdDeleteAllExcluded_Click(cmdDeleteAllExcluded(Index), New EventArgs())
                    cmdAddIncluded(Index).Enabled = False
                    cmdAddAllIncluded(Index).Enabled = False

                    oListItem = lvwExcluded(Index).Items.Add(lvwIncluded(Index).FocusedItem.Name, lvwIncluded(Index).FocusedItem.Text, "")
                    With oListItem

                        .Tag = Convert.ToString(lvwIncluded(Index).FocusedItem.Tag)
                    End With

                    lvwIncluded(Index).Items.RemoveAt(CInt(lvwIncluded(Index).FocusedItem.Name) - 1)

                Case Else

                    If lvwIncluded(Index).Items.Count > 0 Then
                        For iRow As Integer = 1 To lvwIncluded(Index).Items.Count
                            If Convert.ToString(lvwIncluded(Index).Items.Item(iRow - 1).Tag) = 0 Then
                                bIsFound = True
                                Exit For
                            End If
                        Next iRow

                        If bIsFound And lvwIncluded(Index).Items.Count = 1 Then
                            cmdAddIncluded(Index).Enabled = False
                            cmdAddAllIncluded(Index).Enabled = False
                        End If
                    End If
                    oListItem = lvwExcluded(Index).Items.Add(lvwIncluded(Index).FocusedItem.Name, lvwIncluded(Index).FocusedItem.Text, "")
                    With oListItem

                        .Tag = Convert.ToString(lvwIncluded(Index).FocusedItem.Tag)
                    End With
                    lvwIncluded(Index).Items.RemoveAt(CInt(lvwIncluded(Index).FocusedItem.Name) - 1)

            End Select

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Err_cmdAddIncluded_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_cmdAddIncluded_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub

    Private Sub cmdAddToScheduler_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddToScheduler.Click
        Dim vStartDate As Date
        Dim vEndDate As Date

        Dim sExcludeType As String = ""
        Dim bRiskCodeFlag, bRiskGroupFlag As Boolean

        m_lReturn = m_oBusiness.GetNextSessionId(m_lSessionId)
        Me.SessionId = m_lSessionId

        For lControlCount As Integer = 0 To m_vParameters.GetUpperBound(0)
            If CStr(m_vParameters(lControlCount, 0)).ToLower() = "session_id" Then
                m_vParameters(lControlCount, 1) = m_lSessionId
            End If
        Next lControlCount

        ' Validate fields
        For lControlCount As Integer = 0 To lblParameterName.GetUpperBound(0)
            If lblParameterName(lControlCount).Visible Then
                If cboParameterValues(lControlCount).Text.Trim() = "" Then
                    m_lReturn = MessageBox.Show("All Parameters must be supplied.", "Report Parameters", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                'PN 19341 - Extracting the start and end dates if supplied...
                For lParamCount As Integer = 0 To m_vParameters.GetUpperBound(0)
                    If lblParameterName(lControlCount).Text = CStr(m_vParameters(lParamCount, 3)) Then
                        If (CStr(m_vParameters(lParamCount, 0)).ToLower().IndexOf("start date") + 1) Or (CStr(m_vParameters(lParamCount, 0)).ToLower().IndexOf("start_date") + 1) Then
                            vStartDate = CDate(cboParameterValues(lControlCount).Text.Trim())
                        ElseIf ((CStr(m_vParameters(lParamCount, 0)).ToLower().IndexOf("end date") + 1) Or (CStr(m_vParameters(lParamCount, 0)).ToLower().IndexOf("end_date") + 1)) Then
                            vEndDate = CDate(cboParameterValues(lControlCount).Text.Trim())
                        End If
                    End If
                Next lParamCount
            End If
        Next lControlCount

        'PN 19341 - Checking for valid date ranges...

        If Not vStartDate.Equals(DateTime.FromOADate(0)) And Not vEndDate.Equals(DateTime.FromOADate(0)) Then
            If DateAndTime.DateDiff("d", vStartDate, vEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                m_lReturn = MessageBox.Show("End Date sholud be greater than or equal to Start Date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        ' TB specials  certain parameter combinations not allowed
        If m_sReportName.Trim().ToUpper() = ACRptName_UWPolicyListLong Then
            ' cant report on 'ALL' policies if no client selected
            ' param 0 is sort order, param 1 is PersonalClient, 2 is Group client, 3 is corporate 4 is policy
            ' Check if NULL is allowed for that parameter
            If cboParameterValues(1).Text.Trim() = "<NULL>" Then
                If cboParameterValues(2).Text.Trim() = "<NULL>" Then
                    If cboParameterValues(3).Text.Trim() = "<NULL>" Then
                        If cboParameterValues(4).Text.Trim() = "ALL" Then
                            m_lReturn = MessageBox.Show("Please select a Client or enter a policy number", "Report Parameters", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If
        If SSTabHelper.GetTabVisible(tabMainTab, 1) Then
            For lTemp As Integer = 2 To SSTabHelper.GetTabCount(tabMainTab)
                If (SSTabHelper.GetTabCaption(tabMainTab, lTemp - 1).IndexOf("Risk Codes") + 1) And lvwExcluded(lTemp - 2).Items.Count > 0 Then
                    bRiskCodeFlag = True
                ElseIf (SSTabHelper.GetTabCaption(tabMainTab, lTemp - 1).IndexOf("Risk Groups") + 1) And lvwExcluded(lTemp - 2).Items.Count > 0 Then
                    bRiskGroupFlag = True
                End If
            Next
        End If
        If (bRiskCodeFlag) And (bRiskGroupFlag) Then
            m_lReturn = MessageBox.Show("Cannot exclude Risks and Risk Groups at the same time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Update parameters array
        For lControlCount As Integer = 0 To lblParameterName.GetUpperBound(0)
            If lblParameterName(lControlCount).Visible Then
                For lParamCount As Integer = 0 To m_vParameters.GetUpperBound(0)
                    If lblParameterName(lControlCount).Text = CStr(m_vParameters(lParamCount, 3)) Then
                        Dim length As Integer
                        length = Len(m_vDefaultValues(lParamCount, 0))
                        If Not m_vDefaultValues(lParamCount, 0) Is Nothing Then
                            If Not m_vDefaultValues(lParamCount, 0) Is Nothing AndAlso Not CStr(m_vDefaultValues(lParamCount, 0)).StartsWith("DatePickerCombo") Then
                                m_vParameters(lParamCount, 1) = cboParameterValues(lControlCount).Text
                                If cboParameterValues(lControlCount).Items.Count > 0 Then
                                    'TF080800 - Only get ItemData if list item selected
                                    If cboParameterValues(lControlCount).SelectedIndex <> -1 Then
                                        m_vParameters(lParamCount, 4) = VB6.GetItemData(cboParameterValues(lControlCount), cboParameterValues(lControlCount).SelectedIndex)
                                    End If
                                End If
                            ElseIf Not m_vDefaultValues(lParamCount, 0) Is Nothing AndAlso CStr(m_vDefaultValues(lParamCount, 0)).StartsWith("DatePickerCombo") Then
                                If DTParameters(lControlCount).Enabled Then
                                    'Developer Guide No. 197
                                    m_vParameters(lParamCount, 1) = gPMFunctions.ToSafeString(DateTime.FromOADate(DTParameters(lControlCount).Value.ToOADate()).ToString("dd-MMM-yyyy"))
                                End If
                            End If
                            'Exit For
                        End If
                    End If
                Next lParamCount
                If chkGroupBy(lControlCount).CheckState = CheckState.Checked Then
                    'DC270303 -ISS1911
                    'DC120503 -ISS3841 -added Caption bit

                    m_lReturn = m_oBusiness.InsertGroupBy(sGroupBy:=lblParameterName(lControlCount).Text, lSessionID:=m_lSessionId)
                End If
            End If
        Next lControlCount

        'Update Parameters Array With Unique Report Name
        For lParamCount As Integer = 0 To m_vParameters.GetUpperBound(0)
            If CStr(m_vParameters(lParamCount, 0)) = "unique_report_name" Then
                m_vParameters(lParamCount, 1) = m_sUniqueReportName
                'Exit For
            End If
        Next lParamCount

        'Inserting Records in Temp Exclude Table
        For lFraTypeCount As Integer = 0 To fraType.GetUpperBound(0)
            If lvwExcluded(lFraTypeCount).Items.Count > 0 Then
                If SSTabHelper.GetTabCaption(tabMainTab, lFraTypeCount + 1).IndexOf("A/C Exec") >= 0 Then
                    sExcludeType = "AE"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, lFraTypeCount + 1).IndexOf("Account Handler") >= 0 Then
                    sExcludeType = "AH"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, lFraTypeCount + 1).IndexOf("Risk Codes") >= 0 Then
                    sExcludeType = "RC"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, lFraTypeCount + 1).IndexOf("Insurers") >= 0 Then
                    sExcludeType = "IN"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, lFraTypeCount + 1).IndexOf("Third Parties") >= 0 Then
                    sExcludeType = "TP"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, lFraTypeCount + 1).IndexOf("Risk Groups") >= 0 Then
                    sExcludeType = "RG"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, lFraTypeCount + 1).IndexOf("Branches") >= 0 Then
                    sExcludeType = "BR"
                End If

                For lTemp As Integer = 1 To lvwExcluded(lFraTypeCount).Items.Count

                    m_lReturn = m_oBusiness.InsertExcludedType(sUniqueReportName:=m_sUniqueReportName, stype:=sExcludeType, lTypeId:=Convert.ToString(lvwExcluded(lFraTypeCount).Items.Item(lTemp - 1).Tag))
                Next lTemp
            End If
        Next lFraTypeCount

        If m_bMultiUsers Then

            If Information.IsArray(m_vUsers) Then
                For Each m_vUsers_item As Object In m_vUsers

                    m_lReturn = m_oBusiness.InsertUser(sUser:=m_vUsers_item, lSessionID:=m_lSessionId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Unable to write to temporary user table", "Error")
                        m_iStatus = gPMConstants.PMEReturnCode.PMError
                        Exit Sub
                    End If
                Next m_vUsers_item
            End If
        End If
        'DC270303 -ISS1911 -End

        '8.5
        Frequency = gPMFunctions.ToSafeString(cboFrequency.ItemCaption(cboFrequency.ItemId)).Trim()
        m_iStatus = gPMConstants.PMEReturnCode.PMOK
        Me.Close()

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        'Developer Guide No. 146
        'Array.Clear(m_vParameters, 0, m_vParameters.Length)
        m_vParameters = Nothing
        m_iStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Close()
    End Sub

    Private Sub cmdDeleteAllExcluded_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdDeleteAllExcluded_0.Click
        Dim Index As Integer = Array.IndexOf(cmdDeleteAllExcluded, eventSender)
        Dim oListItem As ListViewItem

        Try

            For iRow As Integer = lvwExcluded(Index).Items.Count To 1 Step -1
                oListItem = lvwIncluded(Index).Items.Add(lvwExcluded(Index).Items.Item(iRow - 1).Name, lvwExcluded(Index).Items.Item(iRow - 1).Text, "")
                With oListItem

                    .Tag = Convert.ToString(lvwExcluded(Index).Items.Item(iRow - 1).Tag)
                End With
                lvwExcluded(Index).Items.RemoveAt(iRow - 1)
            Next iRow
            cmdAddIncluded(Index).Enabled = True
            cmdAddAllIncluded(Index).Enabled = True

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteAllExcluded_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAllExcluded_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
    End Sub

    Private Sub cmdDeleteExcluded_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdDeleteExcluded_0.Click
        Dim Index As Integer = Array.IndexOf(cmdDeleteExcluded, eventSender)
        Dim oListItem As ListViewItem

        Try

            If lvwExcluded(Index).Items.Count > 0 Then
                oListItem = lvwIncluded(Index).Items.Add(lvwExcluded(Index).FocusedItem.Name, lvwExcluded(Index).FocusedItem.Text, "")
                With oListItem

                    .Tag = Convert.ToString(lvwExcluded(Index).FocusedItem.Tag)
                End With
                lvwExcluded(Index).Items.RemoveAt(CInt(lvwExcluded(Index).FocusedItem.Name) - 1)

                cmdAddIncluded(Index).Enabled = True
                cmdAddAllIncluded(Index).Enabled = True
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteExcluded_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteExcluded_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim dtStartDate As Date
        Dim dtEndDate As Date
        Dim sExcludeType As String = ""
        Dim bRiskCodeFlag As Boolean
        Dim bRiskGroupFlag As Boolean
        Dim sAgencyCommissionStatement As String = "AGENCY\COMMISSION_STATEMENT"
        m_lReturn = m_oBusiness.GetNextSessionId(m_lSessionId)
        Me.SessionId = m_lSessionId

        For iControlCount As Integer = 0 To m_vParameters.GetUpperBound(0)
            If CStr(m_vParameters(iControlCount, 0)).ToLower() = "session_id" Then
                m_vParameters(iControlCount, 1) = m_lSessionId
            End If
        Next iControlCount

        ' Validate fields
        For iControlCount As Integer = 0 To lblParameterName.GetUpperBound(0)
            If lblParameterName(iControlCount).Visible Then
                'if this control is disabled then it doesn't matter whether a value is supplied as the content should be the value we want
                If cboParameterValues(iControlCount).Text.Trim() = "" And cboParameterValues(iControlCount).Enabled = True Then
                    m_lReturn = MessageBox.Show("All Parameters must be supplied.", "Report Parameters", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                'PN 19341 - Extracting the start and end dates if supplied...
                For iParamCount As Integer = 0 To m_vParameters.GetUpperBound(0)
                    If lblParameterName(iControlCount).Text = CStr(m_vParameters(iParamCount, 3)) Then
                        If (CStr(m_vParameters(iParamCount, 0)).ToLower().IndexOf("start date") + 1) Or (CStr(m_vParameters(iParamCount, 0)).ToLower().IndexOf("start_date") + 1) Then
                            dtStartDate = CDate(cboParameterValues(iControlCount).Text.Trim())
                        ElseIf ((CStr(m_vParameters(iParamCount, 0)).ToLower().IndexOf("end date") + 1) Or (CStr(m_vParameters(iParamCount, 0)).ToLower().IndexOf("end_date") + 1)) Then
                            dtEndDate = CDate(cboParameterValues(iControlCount).Text.Trim())
                        End If
                    End If
                Next iParamCount
            End If
        Next iControlCount

        'PN 19341 - Checking for valid date ranges...

        If Not dtStartDate.Equals(DateTime.FromOADate(0)) And Not dtEndDate.Equals(DateTime.FromOADate(0)) Then
            If m_sReportName.Trim().ToUpper() <> sAgencyCommissionStatement Then
                If DateAndTime.DateDiff("d", dtStartDate, dtEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                    m_lReturn = MessageBox.Show("End Date sholud be greater than or equal to Start Date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        ' TB specials  certain parameter combinations not allowed
        If m_sReportName.Trim().ToUpper() = ACRptName_UWPolicyListLong Then
            ' cant report on 'ALL' policies if no client selected
            ' param 0 is sort order, param 1 is PersonalClient, 2 is Group client, 3 is corporate 4 is policy
            ' Check if NULL is allowed for that parameter
            If cboParameterValues(1).Text.Trim() = "<NULL>" Then
                If cboParameterValues(2).Text.Trim() = "<NULL>" Then
                    If cboParameterValues(3).Text.Trim() = "<NULL>" Then
                        If cboParameterValues(4).Text.Trim() = "ALL" Then
                            m_lReturn = MessageBox.Show("Please select a Client or enter a policy number", "Report Parameters", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If
        If SSTabHelper.GetTabVisible(tabMainTab, 1) Then
            For iTemp As Integer = 2 To SSTabHelper.GetTabCount(tabMainTab)
                If (SSTabHelper.GetTabCaption(tabMainTab, iTemp - 1).IndexOf("Risk Codes") + 1) And lvwExcluded(iTemp - 2).Items.Count > 0 Then
                    bRiskCodeFlag = True
                ElseIf (SSTabHelper.GetTabCaption(tabMainTab, iTemp - 1).IndexOf("Risk Groups") + 1) And lvwExcluded(iTemp - 2).Items.Count > 0 Then
                    bRiskGroupFlag = True
                End If
            Next
        End If
        If (bRiskCodeFlag) And (bRiskGroupFlag) Then
            m_lReturn = MessageBox.Show("Cannot exclude Risks and Risk Groups at the same time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Update parameters array
        For iControlCount As Integer = 0 To lblParameterName.GetUpperBound(0)
            If lblParameterName(iControlCount).Visible Then
                For lParamCount As Integer = 0 To m_vParameters.GetUpperBound(0)
                    If lblParameterName(iControlCount).Text = CStr(m_vParameters(lParamCount, 3)) Then
                        If Not m_vDefaultValues(lParamCount, 0) Is Nothing Then
                            If CStr(m_vDefaultValues(lParamCount, 0)).StartsWith("DatePickerCombo") Then

                                If m_vParameters(lParamCount, 10) = True Then

                                    Dim lSelectedCount As Long
                                    For lSelectedCount = 0 To List1(iControlCount).Items.Count - 1
                                        If List1(iControlCount).SelectedItem Then
                                            m_vParameters(lParamCount, 1) = m_vParameters(lParamCount, 1) & List1(iControlCount).Items(lSelectedCount)
                                            m_vParameters(lParamCount, 1) = m_vParameters(lParamCount, 1) & ","
                                        End If
                                    Next lSelectedCount
                                    If Trim(m_vParameters(lParamCount, 1)) <> "" Then
                                        m_vParameters(lParamCount, 1) = Microsoft.VisualBasic.Strings.Left(m_vParameters(lParamCount, 1), Len(m_vParameters(lParamCount, 1)) - 1)
                                    Else
                                        m_vParameters(lParamCount, 1) = List1(iControlCount).Items(0)
                                    End If

                                Else
                                    m_vParameters(lParamCount, 1) = cboParameterValues(iControlCount).Text
                                End If
                                If DTParameters(iControlCount).Enabled Then
                                    'Developer Guide No. 197
                                    m_vParameters(lParamCount, 1) = gPMFunctions.ToSafeString(DateTime.FromOADate(DTParameters(iControlCount).Value.ToOADate).ToString("dd-MMM-yyyy"))
                                End If
                                Continue For
                            End If
                        End If
                        m_vParameters(lParamCount, 1) = cboParameterValues(iControlCount).Text
                        If cboParameterValues(iControlCount).Items.Count > 0 Then
                            'TF080800 - Only get ItemData if list item selected
                            If cboParameterValues(iControlCount).SelectedIndex <> -1 Then
                                m_vParameters(lParamCount, 4) = VB6.GetItemData(cboParameterValues(iControlCount), cboParameterValues(iControlCount).SelectedIndex)
                            End If

                        End If
                    End If
                Next lParamCount
                If chkGroupBy(iControlCount).CheckState = CheckState.Checked Then
                    'DC270303 -ISS1911
                    'DC120503 -ISS3841 -added Caption bit

                    m_lReturn = m_oBusiness.InsertGroupBy(sGroupBy:=lblParameterName(iControlCount).Text, lSessionID:=m_lSessionId)
                End If
            End If
        Next iControlCount

        'Update Parameters Array With Unique Report Name
        For iParamCount As Integer = 0 To m_vParameters.GetUpperBound(0)
            If CStr(m_vParameters(iParamCount, 0)) = "unique_report_name" Then
                m_vParameters(iParamCount, 1) = m_sUniqueReportName
                'Exit For
            End If
        Next iParamCount

        'Inserting Records in Temp Exclude Table
        For iFraTypeCount As Integer = 0 To fraType.GetUpperBound(0)
            If lvwExcluded(iFraTypeCount).Items.Count > 0 Then
                If SSTabHelper.GetTabCaption(tabMainTab, iFraTypeCount + 1).IndexOf("A/C Exec") >= 0 Then
                    sExcludeType = "AE"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, iFraTypeCount + 1).IndexOf("Account Handler") >= 0 Then
                    sExcludeType = "AH"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, iFraTypeCount + 1).IndexOf("Risk Codes") >= 0 Then
                    sExcludeType = "RC"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, iFraTypeCount + 1).IndexOf("Insurers") >= 0 Then
                    sExcludeType = "IN"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, iFraTypeCount + 1).IndexOf("Third Parties") >= 0 Then
                    sExcludeType = "TP"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, iFraTypeCount + 1).IndexOf("Risk Groups") >= 0 Then
                    sExcludeType = "RG"
                ElseIf SSTabHelper.GetTabCaption(tabMainTab, iFraTypeCount + 1).IndexOf("Branches") >= 0 Then
                    sExcludeType = "BR"
                End If

                For lTemp As Integer = 1 To lvwExcluded(iFraTypeCount).Items.Count

                    m_lReturn = m_oBusiness.InsertExcludedType(sUniqueReportName:=m_sUniqueReportName, stype:=sExcludeType, lTypeId:=Convert.ToString(lvwExcluded(iFraTypeCount).Items.Item(lTemp - 1).Tag))
                Next lTemp
            End If
        Next iFraTypeCount

        If m_bMultiUsers Then

            'DC270303 -ISS1911
            '        m_lReturn = m_oBusiness.CreateReportUserTable()
            '        If m_lReturn <> PMTrue Then
            '            MsgBox "Unable to create temporary user table", , "Error"
            '            m_iStatus% = PMError
            '            Exit Sub
            '        End If

            If Information.IsArray(m_vUsers) Then
                For Each m_vUsers_item As Object In m_vUsers

                    m_lReturn = m_oBusiness.InsertUser(sUser:=m_vUsers_item, lSessionID:=m_lSessionId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Unable to write to temporary user table", "Error")
                        m_iStatus = gPMConstants.PMEReturnCode.PMError
                        Exit Sub
                    End If
                Next m_vUsers_item
            End If
        End If
        'DC270303 -ISS1911 -End

        m_iStatus = gPMConstants.PMEReturnCode.PMOK
        Me.Close()

    End Sub

    'eck190602
    Private Sub cmdParameterValues_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdParameterValues_13.Click, _cmdParameterValues_12.Click, _cmdParameterValues_11.Click, _cmdParameterValues_10.Click, _cmdParameterValues_9.Click, _cmdParameterValues_8.Click, _cmdParameterValues_7.Click, _cmdParameterValues_0.Click, _cmdParameterValues_1.Click, _cmdParameterValues_6.Click, _cmdParameterValues_5.Click, _cmdParameterValues_4.Click, _cmdParameterValues_3.Click, _cmdParameterValues_2.Click
        Dim Index As Integer = Array.IndexOf(cmdParameterValues, eventSender)
        Dim sTable As String = String.Empty
        Dim sField As String = String.Empty
        Dim sPartyType As String = String.Empty
        Dim iSeparator As Integer
        'eck040702
        Dim iFileSeparator As String = ""
        Dim vParties() As Object = Nothing
        Dim bSelected As Boolean
        Dim lPartyCount As Integer
        '
        Dim vCnt As Object = Nothing
        Dim ShortName As String = String.Empty
        Dim Name_Renamed As String = String.Empty
        Dim ResolvedName As String = String.Empty

        If Convert.ToString(cmdParameterValues(Index).Tag) = "" Then
            m_lReturn = SelectUsers(Index)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to load User selection Form ", "Error")
            End If
            Exit Sub
        End If

        Dim sParty As String = Convert.ToString(cmdParameterValues(Index).Tag)

        Do While sParty.Length <> 0
            iFileSeparator = CStr(sParty.IndexOf("+"c) + 1)

            If Not Information.IsArray(vParties) Then
                ReDim vParties(0)
            Else

                ReDim Preserve vParties(vParties.GetUpperBound(0) + 1)
            End If
            If StringsHelper.ToDoubleSafe(iFileSeparator) = 0 Then

                vParties(vParties.GetUpperBound(0)) = Mid(sParty, 1, sParty.Length)
                sParty = ""
            Else

                vParties(vParties.GetUpperBound(0)) = Mid(sParty, 1, CInt(CDbl(iFileSeparator) - 1))
                sParty = Mid(sParty, CInt(CDbl(iFileSeparator) + 1), sParty.Length)
            End If
        Loop

        Do Until bSelected

            sParty = CStr(vParties(lPartyCount))
            iSeparator = (sParty.IndexOf("|"c) + 1)

            sTable = sParty.Substring(0, iSeparator - 1)
            sField = sParty.Substring(sParty.Length - (sParty.Length - iSeparator))
            If sTable.Split("-"c).GetUpperBound(0) = 1 Then
                sPartyType = sTable.Split("-"c)(1)
            End If

            Select Case sPartyType
                Case "PC", "CC", "GC", "CL"
                    'sPartyType = ""
                    bSelected = True
            End Select

            If sTable.ToUpper() = "ACCOUNT" Then
                sPartyType = "ACCOUNT"
            End If

            ' SET 26112002 ISS 1317 - Policy Numbers
            If sTable.ToUpper() = "INSURANCE_FOLDER" Then
                sPartyType = "INSURANCE_FOLDER"
            End If
            ' SET 26112002 - End

            'DN 11/07/02 - Added resolved name
            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=ShortName, vName:=Name_Renamed, vResolvedName:=ResolvedName, vSpecialParty:=sPartyType)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bSelected = True

                ' SET 26112002 ISS 1317 - only set these values if user
                '                         makes a valid selection
                Select Case sField.ToUpper()
                    Case "NAME"
                        cboParameterValues(Index).Text = Name_Renamed
                        'DC280602
                        ' SET 26112002 ISS 1317 - insurance folder find returns code
                    Case "SHORTNAME", "SHORT_CODE", "CODE", "INSURANCE_FOLDER_CNT" '' Condition added (Insurance_folder_cnt) PN 3105
                        cboParameterValues(Index).Text = ShortName
                        'DN 11/07/02
                    Case "RESOLVED_NAME"
                        cboParameterValues(Index).Text = ResolvedName
                End Select
            End If

            lPartyCount += 1
            'Uer didn't

            If lPartyCount > vParties.GetUpperBound(0) Then
                bSelected = True
                ShortName = ""
                Name_Renamed = ""
            End If
        Loop

    End Sub

    Private Sub frmParameters_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            ' Deal with no parameters
            If cboParameterValues(0).Visible = False And DTParameters(0).Visible = False Then
                m_iStatus = gPMConstants.PMEReturnCode.PMOK
                Me.Close()
            Else
                cboParameterValues(0).Focus()
            End If

        End If
        'frmparameters_activate = True

    End Sub

    Private Sub cboParameterValues_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cboParameterValues_11.Leave, _cboParameterValues_6.Leave, _cboParameterValues_5.Leave, _cboParameterValues_4.Leave, _cboParameterValues_3.Leave, _cboParameterValues_2.Leave, _cboParameterValues_1.Leave, _cboParameterValues_0.Leave, _cboParameterValues_7.Leave, _cboParameterValues_8.Leave, _cboParameterValues_9.Leave, _cboParameterValues_10.Leave, _cboParameterValues_12.Leave, _cboParameterValues_13.Leave
        Dim Index As Integer = Array.IndexOf(cboParameterValues, eventSender)
        Dim sText As String = cboParameterValues(Index).Text.Trim()

        If Not cboParameterValues(Index).Tag Is Nothing Then
            Select Case CInt(Convert.ToString(cboParameterValues(Index).Tag))
                Case VariantType.Short, VariantType.Integer, VariantType.Single, VariantType.Double, VariantType.Decimal
                    cboParameterValues(Index).Text = CStr(Conversion.Val(sText))
                Case VariantType.Boolean
                    Select Case sText.ToLower()
                        Case "t", "true", "y", "yes", "1"
                            cboParameterValues(Index).Text = "True"
                        Case Else
                            cboParameterValues(Index).Text = "False"
                    End Select
                Case VariantType.Date
                    If gSIRLibrary.SIRDateType(sText) = gSIRLibrary.SIREDateType.sireValidDate Then
                        Dim parsedDate As DateTime
                        If DateTime.TryParse(sText, parsedDate) Then
                            cboParameterValues(Index).Text = parsedDate.ToString("MMM dd yyyy")
                        Else
                            cboParameterValues(Index).Text = ""
                        End If
                    Else
                        cboParameterValues(Index).Text = ""
                    End If
                Case VariantType.String
                    cboParameterValues(Index).Text = sText
            End Select
        End If
        'DC270303 -ISS1911
        If lblParameterName(Index).Text = "User Names:" Then
            Select Case cboParameterValues(Index).Text.Trim()
                Case "<ALL>"
                    m_vUsers = VB6.CopyArray(m_vUserNames)
                Case "<Selected>"
                    ' nothing
                Case Else
                    ReDim m_vUsers(0)
                    m_vUsers(0) = cboParameterValues(Index).Text.Trim()
            End Select

        End If

    End Sub

    Private Sub lvwExcluded_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _lvwExcluded_0.DoubleClick
        Dim Index As Integer = Array.IndexOf(lvwExcluded, eventSender)
        cmdDeleteExcluded_Click(cmdDeleteExcluded(Index), New EventArgs())
    End Sub

    Private Sub lvwIncluded_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _lvwIncluded_0.DoubleClick
        Dim Index As Integer = Array.IndexOf(lvwIncluded, eventSender)
        If Not cmdAddIncluded(Index).Enabled Or Not cmdAddAllIncluded(Index).Enabled Then
            Exit Sub
        End If
        cmdAddIncluded_Click(cmdAddIncluded(Index), New EventArgs())
    End Sub

    'eck190601 Find Party Types

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vSpecialParty As String = "") As Integer

        Dim result As Integer = 0
        Dim oFindParty As Object
        Dim vKeyArray(,) As Object
        Const kAgentCode As Integer = 5
        Const kIntroducerPromtingText As Integer = 3
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DJM 23/08/2002 : Do it differently when selecting from account table.
            If vSpecialParty = "ACCOUNT" Then
                oFindParty = New iACTFindAccount.Interface_Renamed
                ' SET 26112002 ISS 1317 - Policy Numbers
            ElseIf vSpecialParty = "INSURANCE_FOLDER" Then
                oFindParty = New iPMBFindInsurance.Interface_Renamed()
            Else
                'Developer Guide No. 108
                oFindParty = New iPMBFindParty.Interface_Renamed()

                'PN18809 Ignore DPA Questions where selecting Client For Report.

                oFindParty.IgnoreDPAQuestions = True
            End If

            'Set appropriate key if agent only

            If ((Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty))) And vSpecialParty <> "ACCOUNT" Then

                ReDim vKeyArray(1, 3)
                vKeyArray(0, 0) = "special_party"
                vKeyArray(1, 0) = vSpecialParty

                vKeyArray(0, 1) = "FindType"
                vKeyArray(1, 1) = 0

                vKeyArray(0, 2) = "CF;party_party_type_id;R;0"
                vKeyArray(1, 2) = vSpecialParty

                vKeyArray(0, 3) = "DefaultFindScreen"
                vKeyArray(1, 3) = "iPMBFindParty.Interface"

                'If m_sReportName.Trim().ToUpper() = ACRptName_BRPolicyListLong Then
                '    ReDim Preserve vKeyArray(1, 1)
                '    vKeyArray(0, 1) = "risk_transfer_agreement"
                '    vKeyArray(1, 1) = 1
                'End If

                m_lErrorNumber = oFindParty.SetKeys(vKeyArray)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = m_sCallingAppName

            m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' SET 26112002 ISS 1317 - Policy Numbers
            If vSpecialParty <> "INSURANCE_FOLDER" Then

                oFindParty.NotEditable = 1
            End If

            If vSpecialParty = "AG" Then
                If UBound(m_vParameters, 1) >= 5 Then '' Paralleled from 1.13.5 PN 3104
                    If CStr(m_vParameters(kAgentCode, kIntroducerPromtingText)).ToUpper().StartsWith("INTRODUCER") Then

                        oFindParty.IntroducerOnly = True
                    End If
                End If
            End If

            m_lErrorNumber = oFindParty.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                'DJM 23/08/2002 : Do it differently when selecting from account table.
                If vSpecialParty = "ACCOUNT" Then

                    vShortName = oFindParty.ShortCode
                    ' SET 26112002 ISS 1317 - Policy Numbers
                ElseIf vSpecialParty = "INSURANCE_FOLDER" Then

                    vShortName = oFindParty.InsReference
                Else

                    vPartyCnt = oFindParty.PartyCnt

                    vShortName = oFindParty.ShortName

                    If Not Information.IsNothing(vName) Then

                        vName = oFindParty.LongName
                    End If
                    'DN 11/07/02

                    If Not Information.IsNothing(vResolvedName) Then

                        vResolvedName = oFindParty.ResolvedName
                    End If
                End If
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.Dispose()

            oFindParty = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    Private Sub Form_Initialize_Renamed()

        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.

            'OK, in this situation the business object has a form.  It needs to be displayed.
            'So we don't create the object on the server, but locally
            'Update - no it doesn't, at least not in the latest version that was nicked

            'Dim temp_m_oBusiness As Object
            'm_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRReportPrint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            'm_oBusiness = temp_m_oBusiness

            '' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    ' Failed to get an instance of the business object.
            '    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            '    ' Display error stating the problem.

            '    ' Get description from the resource file.

            '    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    ' Display message.
            '    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

            '    Exit Sub
            'End If

            '' Create an instance of the general interface object.
            'm_oGeneral = New iPMBReportPrint.General()

            '' Call the initialise method passing this interface
            '' and the business object as parameters.
            'm_lReturn = m_oGeneral.InitialiseParameters(frmParameters:=Me, oBusiness:=m_oBusiness)

            '' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            '    Exit Sub
            'End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'DC270303 -ISS1911

    Public Sub frmParametersLoad()

        ' Set the interface default values.
        m_lReturn = SetInterfaceDefaults()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If

        ' Gets the interface details to be displayed.
        m_lReturn = m_oGeneral.GetParameterDetails()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get the interface details.
            m_lErrorNumber = m_lReturn

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If

        ' Set the interface status to cancelled. This is done
        ' so that any interface termination will be noted
        ' as cancelled except in the event of accepting
        ' the interface.
        m_iStatus = gPMConstants.PMEReturnCode.PMCancel

        iPMFunc.CenterForm(Me)

    End Sub

    'Private Sub frmParameters_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

    '    ' Set the interface default values.
    '    m_lReturn = SetInterfaceDefaults()

    '    ' Check for errors.
    '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

    '        ' Set the mouse pointer to normal.
    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    '        Exit Sub
    '    End If

    '    ' Gets the interface details to be displayed.
    '    m_lReturn = m_oGeneral.GetParameterDetails()

    '    ' Check for errors.
    '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        ' Failed to get the interface details.
    '        m_lErrorNumber = m_lReturn

    '        ' Set the mouse pointer to normal.
    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    '        Exit Sub
    '    End If

    '    ' Set the interface status to cancelled. This is done
    '    ' so that any interface termination will be noted
    '    ' as cancelled except in the event of accepting
    '    ' the interface.
    '    m_iStatus = gPMConstants.PMEReturnCode.PMCancel

    '    iPMFunc.CenterForm(Me)

    'End Sub

    'DC270303 -ISS1911
    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            '    ' Display all language specific captions.
            '    m_lReturn& = DisplayCaptions()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        SetInterfaceDefaults = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' Set the status of the Navigate button.
            '    Select Case (m_lNavigate&)
            '        Case PMNavigateEnabled
            '            cmdNavigate.Visible = True
            '            cmdNavigate.Enabled = True
            '
            '        Case PMNavigateDisabled
            '            cmdNavigate.Visible = True
            '            cmdNavigate.Enabled = False
            '
            '        Case Else
            '            cmdNavigate.Visible = False
            '    End Select

            '    m_lReturn& = SetFirstLastControls()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        SetInterfaceDefaults = PMFalse
            '        Exit Function
            '    End If

            ' Set any other default values to the interface.

            m_lReturn = m_oBusiness.GetOtherDetails(r_vRisks:=m_vRisks, r_vAccountExecutives:=m_vAccountExecutives, r_vAccountHandlers:=m_vAccountHandlers, r_vInsurers:=m_vInsurers, r_vThirdParties:=m_vThirdParties, r_vGroups:=m_vGroups, r_vBranches:=m_vBranches)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get other details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")

                Return result
            End If

            'DC250402 -Start
            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'DC250402 -End

            ' CTAF 260601 - Dont show the page not found thing!
            'WebBrowser1.Navigate2 "about:blank"

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmParameters_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        'If OK not clicked signify cancel to calling form  PN23199
        If m_iStatus <> gPMConstants.PMEReturnCode.PMOK Then
            m_iStatus = gPMConstants.PMEReturnCode.PMCancel
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            Dim m_ctlTabFirstLast(1, 1) As Object

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            m_ctlTabFirstLast(ACControlStart, 0) = cboParameterValues(0)
            m_ctlTabFirstLast(ACControlEnd, 0) = chkGroupBy(10)
            m_ctlTabFirstLast(ACControlStart, 1) = lvwIncluded(0)
            m_ctlTabFirstLast(ACControlEnd, 1) = lvwExcluded(0)

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SelectUsers
    '
    ' Description: Call To select More than One user
    '
    ' ***************************************************************** '
    Private Function SelectUsers(ByRef Index As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        With frmUsers
            'Developer Guide No. 24
            .Users = m_vUserNames
            .Status = gPMConstants.PMEReturnCode.PMCancel
            .ShowDialog()
        End With

        With frmUsers
            m_ifrmUserStatus = .Status
            m_vUsers = .Users
        End With

        If m_ifrmUserStatus = gPMConstants.PMEReturnCode.PMOK And Information.IsArray(m_vUsers) Then
            cboParameterValues(Index).SelectedIndex = -1
            cboParameterValues(Index).Text = "<Selected>"
        Else
            If cboParameterValues(Index).Text = "<Selected>" Then
                cboParameterValues(Index).SelectedIndex = 0
            End If
        End If

        frmUsers.Close()

        Return result

    End Function

    'Group list string is in the format "A-GBGX"
    Private Function PopulateGroupByList(ByVal v_sGroupList As String) As Integer
        Dim result As Integer = 0
        Dim lStart As Integer
        Dim sCurrentValue As String = ""
        Dim lCurrentValue As Integer
        Dim sDefaultValue As String = ""
        Dim lDefaultCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lLoop As Integer = lblParameterName.GetLowerBound(0) To lblParameterName.GetUpperBound(0)
                If lblParameterName(lLoop).Text = ACLabelGroupBy Then

                    'Get current item selected.
                    sCurrentValue = VB6.GetItemString(cboParameterValues(lLoop), cboParameterValues(lLoop).SelectedIndex)
                    lCurrentValue = -1

                    'Clear existing list
                    cboParameterValues(lLoop).Items.Clear()

                    If Mid(v_sGroupList, 3) = "" OrElse (Mid(v_sGroupList, 3, 2) <> "GX" AndAlso Mid(v_sGroupList, 3, 2) <> "GB" AndAlso Mid(v_sGroupList, 3, 2) <> "GC" AndAlso Mid(v_sGroupList, 3, 2) <> "GD" AndAlso Mid(v_sGroupList, 3, 2) <> "GE") Then

                        'No list specified so load the defaults for the group by list.

                        For lParamLoop As Integer = 0 To m_vParameters.GetUpperBound(0)
                            If CStr(m_vParameters(lParamLoop, 3)) = ACLabelGroupBy Then

                                For lDefaultCount = 0 To m_vDefaultValues.GetUpperBound(1)
                                    sDefaultValue = CStr(m_vDefaultValues(lParamLoop, lDefaultCount))

                                    If Not (Convert.IsDBNull(sDefaultValue) Or IsNothing(sDefaultValue)) Then
                                        If sDefaultValue.Trim() > "" Then
                                            Dim cboParameterValues_NewIndex As Integer = -1
                                            cboParameterValues_NewIndex = cboParameterValues(lLoop).Items.Add(sDefaultValue.Trim())
                                            VB6.SetItemData(cboParameterValues(lLoop), cboParameterValues_NewIndex, lDefaultCount)

                                            'Does this item match the one that was previously selected in the list.
                                            If sCurrentValue = VB6.GetItemString(cboParameterValues(lLoop), cboParameterValues(lLoop).Items.Count - 1) Then
                                                lCurrentValue = cboParameterValues(lLoop).Items.Count - 1
                                            End If
                                        End If
                                    End If
                                Next

                                Exit For
                            End If
                        Next
                    Else

                        'There is a list for putting into the group by list.
                        lStart = 3
                        lDefaultCount = 0
                        Do While Mid(v_sGroupList, lStart, 2) <> ""
                            Select Case Mid(v_sGroupList, lStart, 2)
                                Case ACGroupByCodeNothing
                                    'Developer Guide No. 153
                                    cboParameterValues(lLoop).Items.Add(New VB6.ListBoxItem(MainModule.ACGroupByNothing, lDefaultCount))
                                    lDefaultCount += 1
                                Case ACGroupByCodeBranch
                                    'Developer Guide No. 153
                                    cboParameterValues(lLoop).Items.Add(New VB6.ListBoxItem(MainModule.ACGroupByBranch, lDefaultCount))
                                    lDefaultCount += 1
                                Case ACGroupByCodeCurrency
                                    'Developer Guide No. 153
                                    cboParameterValues(lLoop).Items.Add(New VB6.ListBoxItem(MainModule.ACGroupByCurrency, lDefaultCount))
                                    lDefaultCount += 1
                                Case ACGroupByCodeBranchCurrency
                                    'Developer Guide No. 153
                                    cboParameterValues(lLoop).Items.Add(New VB6.ListBoxItem(MainModule.ACGroupByBranchCurrency, lDefaultCount))
                                    lDefaultCount += 1
                                Case ACGroupByCodeCurrencyBranch
                                    'Developer Guide No. 153
                                    cboParameterValues(lLoop).Items.Add(New VB6.ListBoxItem(MainModule.ACGroupByCurrencyBranch, lDefaultCount))
                                    lDefaultCount += 1
                                Case Else
                                    Exit Do
                            End Select

                            lStart += 2

                            'Does this item match the one that was previously selected in the list.
                            If sCurrentValue = VB6.GetItemString(cboParameterValues(lLoop), cboParameterValues(lLoop).Items.Count - 1) Then
                                lCurrentValue = cboParameterValues(lLoop).Items.Count - 1
                            End If
                        Loop
                    End If

                    If lCurrentValue >= 0 Then
                        'Previously selected group by item is still in list so default to it.
                        cboParameterValues(lLoop).SelectedIndex = lCurrentValue
                    Else
                        cboParameterValues(lLoop).SelectedIndex = 0
                    End If

                    Exit For

                End If
            Next

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in PopulateGroupByList", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateGroupByList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function PopulateTypeOfCurrencyList() As Integer
        Dim result As Integer = 0
        Dim sCurrentValue As String = ""
        Dim lCurrentValue As Integer
        Dim sDefaultValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lLoop As Integer = lblParameterName.GetLowerBound(0) To lblParameterName.GetUpperBound(0)
                If lblParameterName(lLoop).Text = ACLabelTypeOfCurrency Then

                    'Get current item selected.
                    sCurrentValue = VB6.GetItemString(cboParameterValues(lLoop), cboParameterValues(lLoop).SelectedIndex)
                    lCurrentValue = -1

                    'Clear existing list
                    cboParameterValues(lLoop).Items.Clear()

                    'No list specified so load the defaults for the group by list.
                    For lParamLoop As Integer = 0 To m_vParameters.GetUpperBound(0)
                        If CStr(m_vParameters(lParamLoop, 3)) = ACLabelTypeOfCurrency Then

                            For lDefaultCount As Integer = 0 To m_vDefaultValues.GetUpperBound(1)
                                sDefaultValue = CStr(m_vDefaultValues(lParamLoop, lDefaultCount))

                                If Not (Convert.IsDBNull(sDefaultValue) Or IsNothing(sDefaultValue)) Then
                                    If sDefaultValue.Trim() > "" Then
                                        If Not (sDefaultValue.IndexOf(ACRestrictionCodeBranchSelectedOnly) >= 0 And m_bAllBranches) Then

                                            Select Case sDefaultValue.Substring(0, 1)
                                                Case "A"
                                                    sDefaultValue = ACTypeOfCurrencyAccount
                                                Case "B"
                                                    sDefaultValue = ACTypeOfCurrencyBase
                                                Case "S"
                                                    sDefaultValue = ACTypeOfCurrencySystem
                                                Case "T"
                                                    sDefaultValue = ACTypeOfCurrencyTransaction
                                            End Select

                                            Dim cboParameterValues_NewIndex As Integer = -1
                                            cboParameterValues_NewIndex = cboParameterValues(lLoop).Items.Add(sDefaultValue.Trim())
                                            VB6.SetItemData(cboParameterValues(lLoop), cboParameterValues_NewIndex, lDefaultCount)

                                            'Does this item match the one that was previously selected in the list.
                                            If sCurrentValue = VB6.GetItemString(cboParameterValues(lLoop), cboParameterValues(lLoop).Items.Count - 1) Then
                                                lCurrentValue = cboParameterValues(lLoop).Items.Count - 1
                                            End If
                                        End If
                                    End If
                                End If
                            Next

                            Exit For
                        End If
                    Next

                    If lCurrentValue >= 0 Then
                        'Previously selected group by item is still in list so default to it.
                        cboParameterValues(lLoop).SelectedIndex = lCurrentValue
                    Else
                        cboParameterValues(lLoop).SelectedIndex = 0
                    End If

                    Exit For
                End If
            Next

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in PopulateTypeOfCurrencyList", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateTypeOfCurrencyList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CreateTab(ByVal iTabCount As Integer, ByVal stype As String) As Integer
        Dim result As Integer = 0
        Dim iControlCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If iTabCount = 2 And Not SSTabHelper.GetTabVisible(tabMainTab, 1) Then
                SSTabHelper.SetTabVisible(tabMainTab, 1, True)
                m_lReturn = CreateCaption(iTabCount - 1, stype)
                Return result
            End If

            SSTabHelper.SetTabs(tabMainTab, iTabCount + 1)
            SSTabHelper.SetSelectedIndex(tabMainTab, iTabCount)
            m_lReturn = CreateCaption(iTabCount, stype)

            iControlCount = iTabCount - 1
            ContainerHelper.LoadControl(Me, "fraType", iControlCount)
            With fraType(iControlCount)
                .Visible = True
                .Top = fraType(0).Top
                .Left = VB6.TwipsToPixelsX(120)
            End With

            ContainerHelper.LoadControl(Me, "lblIncluded", iControlCount)
            With lblIncluded(iControlCount)
                .Visible = True
                .Parent = fraType(iControlCount)
            End With

            ContainerHelper.LoadControl(Me, "lvwIncluded", iControlCount)
            With lvwIncluded(iControlCount)
                .Visible = True
                .Parent = fraType(iControlCount)
            End With
            lvwIncluded(iControlCount).Items.Clear()

            ContainerHelper.LoadControl(Me, "lblExcluded", iControlCount)
            With lblExcluded(iControlCount)
                .Visible = True
                .Parent = fraType(iControlCount)
            End With

            ContainerHelper.LoadControl(Me, "lvwExcluded", iControlCount)
            With lvwExcluded(iControlCount)
                .Visible = True
                .Parent = fraType(iControlCount)
            End With

            ContainerHelper.LoadControl(Me, "cmdAddIncluded", iControlCount)
            With cmdAddIncluded(iControlCount)
                .Visible = True
                .Parent = fraType(iControlCount)
            End With

            ContainerHelper.LoadControl(Me, "cmdAddAllIncluded", iControlCount)
            With cmdAddAllIncluded(iControlCount)
                .Visible = True
                .Parent = fraType(iControlCount)
            End With

            ContainerHelper.LoadControl(Me, "cmdDeleteExcluded", iControlCount)
            With cmdDeleteExcluded(iControlCount)
                .Visible = True
                .Parent = fraType(iControlCount)
            End With

            ContainerHelper.LoadControl(Me, "cmdDeleteAllExcluded", iControlCount)
            With cmdDeleteAllExcluded(iControlCount)
                .Visible = True
                .Parent = fraType(iControlCount)
            End With

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in CreateTab", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTab", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CreateCaption(ByVal iTabCount As Integer, ByVal sCaptionText As String) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case sCaptionText
                Case "INCEXC AE"
                    SSTabHelper.SetTabCaption(tabMainTab, iTabCount, "&" & (CStr(iTabCount + 1)) & " - " & "A/C Exec")
                Case "INCEXC AH"
                    SSTabHelper.SetTabCaption(tabMainTab, iTabCount, "&" & (CStr(iTabCount + 1)) & " - " & "Account Handler")
                Case "INCEXC RC"
                    SSTabHelper.SetTabCaption(tabMainTab, iTabCount, "&" & (CStr(iTabCount + 1)) & " - " & "Risk Codes")
                Case "INCEXC IN"
                    SSTabHelper.SetTabCaption(tabMainTab, iTabCount, "&" & (CStr(iTabCount + 1)) & " - " & "Insurers")
                Case "INCEXC TP"
                    SSTabHelper.SetTabCaption(tabMainTab, iTabCount, "&" & (CStr(iTabCount + 1)) & " - " & "Third Parties")
                Case "INCEXC RG"
                    SSTabHelper.SetTabCaption(tabMainTab, iTabCount, "&" & (CStr(iTabCount + 1)) & " - " & "Risk Groups")
                Case "INCEXC BR"
                    SSTabHelper.SetTabCaption(tabMainTab, iTabCount, "&" & (CStr(iTabCount + 1)) & " - " & "Branches")
            End Select

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Caption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function PopulateTypeOfIncludeLists(ByVal sTypeOfInclude As String, ByVal iListIndex As Integer) As Integer
        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case sTypeOfInclude
                Case "INCEXC AE" 'A/C Exec

                    oListItem = lvwIncluded(iListIndex).Items.Add("L" & "NO", "<No Account Executives>", "")

                    With oListItem

                        .Tag = CStr(0)
                    End With

                    oListItem = Nothing

                    If Information.IsArray(m_vAccountExecutives) And SSTabHelper.GetTabVisible(tabMainTab, 1) Then

                        For lTemp As Integer = m_vAccountExecutives.GetLowerBound(1) To m_vAccountExecutives.GetUpperBound(1)

                            oListItem = lvwIncluded(iListIndex).Items.Add("L" & lTemp, CStr(m_vAccountExecutives(2, lTemp)).Trim(), "")

                            With oListItem

                                .Tag = CStr(m_vAccountExecutives(0, lTemp))
                            End With

                            oListItem = Nothing

                        Next lTemp

                    End If

                Case "INCEXC AH" 'Account Handler

                    oListItem = lvwIncluded(iListIndex).Items.Add("L" & "NO", "<No Account Handlers>", "")

                    With oListItem

                        .Tag = CStr(0)
                    End With

                    If Information.IsArray(m_vAccountHandlers) And SSTabHelper.GetTabVisible(tabMainTab, 1) Then

                        For lTemp As Integer = m_vAccountHandlers.GetLowerBound(1) To m_vAccountHandlers.GetUpperBound(1)

                            oListItem = lvwIncluded(iListIndex).Items.Add("L" & lTemp, CStr(m_vAccountHandlers(2, lTemp)).Trim(), "")

                            With oListItem

                                .Tag = CStr(m_vAccountHandlers(0, lTemp))
                            End With

                            oListItem = Nothing

                        Next lTemp

                    End If

                Case "INCEXC RC" 'Risk Codes

                    oListItem = lvwIncluded(iListIndex).Items.Add("L" & "NO", "<No Risk Codes>", "")

                    With oListItem

                        .Tag = CStr(0)
                    End With

                    If Information.IsArray(m_vRisks) And SSTabHelper.GetTabVisible(tabMainTab, 1) Then

                        For lTemp As Integer = m_vRisks.GetLowerBound(1) To m_vRisks.GetUpperBound(1)

                            oListItem = lvwIncluded(iListIndex).Items.Add("L" & lTemp, CStr(m_vRisks(2, lTemp)).Trim(), "")

                            With oListItem

                                .Tag = CStr(m_vRisks(0, lTemp))
                            End With

                            oListItem = Nothing

                        Next lTemp
                    End If

                Case "INCEXC IN" 'Insurers

                    oListItem = lvwIncluded(iListIndex).Items.Add("L" & "NO", "<No Insurers>", "")

                    With oListItem

                        .Tag = CStr(0)
                    End With

                    If Information.IsArray(m_vInsurers) Then

                        For lTemp As Integer = m_vInsurers.GetLowerBound(1) To m_vInsurers.GetUpperBound(1)

                            oListItem = lvwIncluded(iListIndex).Items.Add("L" & lTemp, CStr(m_vInsurers(2, lTemp)).Trim(), "")

                            With oListItem

                                .Tag = CStr(m_vInsurers(0, lTemp))
                            End With

                            oListItem = Nothing

                        Next lTemp

                    End If

                Case "INCEXC TP" 'Third Parties

                    oListItem = lvwIncluded(iListIndex).Items.Add("L" & "NO", "<No Third Parties>", "")

                    With oListItem

                        .Tag = CStr(0)
                    End With

                    If Information.IsArray(m_vThirdParties) And SSTabHelper.GetTabVisible(tabMainTab, 1) Then

                        For lTemp As Integer = m_vThirdParties.GetLowerBound(1) To m_vThirdParties.GetUpperBound(1)

                            oListItem = lvwIncluded(iListIndex).Items.Add("L" & lTemp, CStr(m_vThirdParties(2, lTemp)).Trim(), "")

                            With oListItem

                                .Tag = CStr(m_vThirdParties(0, lTemp))
                            End With

                            oListItem = Nothing

                        Next lTemp

                    End If

                Case "INCEXC RG"

                    oListItem = lvwIncluded(iListIndex).Items.Add("L" & "NO" & CStr(iListIndex), "<No Risk Groups>", "")

                    With oListItem

                        .Tag = CStr(0)
                    End With

                    If Information.IsArray(m_vGroups) And SSTabHelper.GetTabVisible(tabMainTab, 1) Then

                        For lTemp As Integer = m_vGroups.GetLowerBound(1) To m_vGroups.GetUpperBound(1)

                            oListItem = lvwIncluded(iListIndex).Items.Add("L" & lTemp & CStr(iListIndex), CStr(m_vGroups(2, lTemp)).Trim(), "")

                            With oListItem

                                .Tag = CStr(m_vGroups(0, lTemp))
                            End With

                            oListItem = Nothing

                        Next lTemp

                    End If

                Case "INCEXC BR"

                    oListItem = lvwIncluded(iListIndex).Items.Add("L" & "NO" & CStr(iListIndex), "<No Branches>", "")

                    With oListItem

                        .Tag = CStr(0)
                    End With

                    If Information.IsArray(m_vBranches) And SSTabHelper.GetTabVisible(tabMainTab, 1) Then

                        For lTemp As Integer = m_vBranches.GetLowerBound(1) To m_vBranches.GetUpperBound(1)

                            oListItem = lvwIncluded(iListIndex).Items.Add("L" & lTemp & CStr(iListIndex), CStr(m_vBranches(1, lTemp)).Trim(), "")

                            With oListItem

                                .Tag = CStr(m_vBranches(0, lTemp))
                            End With

                            oListItem = Nothing

                        Next lTemp

                    End If

            End Select

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateTypeOfIncludeLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateTypeOfIncludeLists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmParameters_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
    End Sub

    Private Sub frmParameters_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetParamFields()
    End Sub

    Private Function FilterChild(ByVal lParentIndex As Long, ByVal lChildIndex As Long) As Integer
        Const kMethodName As String = "FilterChild"

        Dim sParentTableName As String
        Dim sChildTableName As String
        Dim sIDCol As String
        Dim sDescCol As String
        Dim sFilterVal As String
        Dim vDefaultValues As Object
        Dim lLoop As Long
        Dim sCustomStoredProcedure As String
        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            ReDim vDefaultValues(UBound(m_vDefaultValues, 1), UBound(m_vDefaultValues, 2))

            sParentTableName = m_vParameters(lParentIndex, 7)
            sChildTableName = m_vParameters(lChildIndex, 7)
            sIDCol = m_vParameters(lChildIndex, 8)
            sDescCol = m_vParameters(lChildIndex, 9)
            sCustomStoredProcedure = ToSafeString(m_vParameters(lChildIndex, 11))
            sFilterVal = cboParameterValues(lParentIndex).Text 'get the value from the dropdown of selected combo

            'first we need to call the business object to get the list of default values and ids with this filter
            m_lReturn = m_oBusiness.FilterChildList(vDefaultValues, sParentTableName, sChildTableName, sIDCol, sDescCol, sFilterVal, sCustomStoredProcedure)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Return iResult
            End If

            'we now need to update the m_vDefaultValues array for this child to only contain the new values
            For lLoop = 0 To UBound(m_vDefaultValues, 2)
                m_vDefaultValues(lChildIndex, lLoop) = ""
            Next lLoop

            For lLoop = 0 To UBound(vDefaultValues)
                m_vDefaultValues(lChildIndex, lLoop) = vDefaultValues(lLoop)
            Next lLoop

            ' Filter the child combo or listbox
            If cboParameterValues(lChildIndex - count).Visible = True Or m_vParameters(lChildIndex, 10) = False Then

                'Do While cboParameterValues(lChildIndex).Items.Count > 0
                '    cboParameterValues(lChildIndex).Items.RemoveAt(cboParameterValues(lChildIndex).Items.Count - 1)
                'Loop

                cboParameterValues(lChildIndex - count).Items.Clear() '--Ritu

                ' cboParameterValues(lControlCount).ItemData(cboParameterValues(lControlCount).NewIndex) = lDefaultCount

                For lLoop = 0 To UBound(m_vDefaultValues, 2)
                    If Trim(m_vDefaultValues(lChildIndex, lLoop)) <> "" Then
                        cboParameterValues(lChildIndex - count).Items.Add(Trim(m_vDefaultValues(lChildIndex, lLoop)))
                    End If
                Next lLoop
                'We minus the childindex with the no. of parameters which are not visible on the report screen,in order to get the right childindex value --Ritu
                If cboParameterValues(lChildIndex - count).Items.Count <> 0 Then
                    cboParameterValues(lChildIndex - count).Enabled = True
                    cboParameterValues(lChildIndex - count).SelectedIndex = "0"
                Else
                    cboParameterValues(lChildIndex - count).Enabled = False
                End If

            ElseIf List1(lChildIndex).Visible = True Or m_vParameters(lChildIndex, 10) = True Then
                Do While List1(lChildIndex).Items.Count > 0
                    List1(lChildIndex).Items.RemoveAt(List1(lChildIndex).Items.Count - 1)
                Loop

                For lLoop = 0 To UBound(m_vDefaultValues, 2)
                    If Trim(m_vDefaultValues(lChildIndex, lLoop)) <> "" Then
                        List1(lChildIndex).Items.Add(New VB6.ListBoxItem(m_vDefaultValues(lChildIndex, lLoop), lChildIndex))
                    End If
                Next lLoop

                If List1(lChildIndex).Items.Count <> 0 Then
                    List1(lChildIndex).Enabled = True
                    List1(lChildIndex).SelectedItem = 0
                Else
                    List1(lChildIndex).Enabled = False
                End If

            End If

            ' End If

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try

        Return iResult

    End Function

End Class
