Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No 129. 
Imports SharedFiles
Imports PMUPolicyVersion
Imports System.Data

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form


    Private Const ACClass As String = "frmInterface"

    ' Private variables
    Private m_sShortName As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_lStatus As Integer

    ' Process mode variables
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lReturn As Integer
    Private m_lErrorNumber As Integer

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    'ED 18072002
    Private m_bUnderwriting As Boolean

    Private m_lOriginalInsuranceFileCnt As Integer ' RAM20050825 - PN 23018
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.2.1.1)
    'Private m_bRI2007Enabled As Boolean
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.2.1.1)
    'PLICO 45
    Private Const MTA_AUTHORITY_WITH_CLAIMS As Integer = 3
    Private Const MTA_AUTHORITY_WITHOUT_CLAIMS As Integer = 2
    Private Const MTA_AUTHORITY_NOT_ALLOWED As Integer = 1

    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    Private m_bBackDatedMTAsAllowed As Boolean
    'vivek63205
    Private m_bBackDatedCanAllowed As Boolean
    'Vivek63205
    Private m_sSelectedPolicyStatus As String = ""
    Private m_bIsMTATemp As Boolean
    Private m_dtRenewaldate As Date
    Private m_bIsPriorDate As Boolean
    Private m_bIsRenewed As Boolean
    Private m_dtLapsedDate As Date
    'WPR12- Enhancement Quote Collection Process
    Private m_bDontProceedMarkedForCollection As Boolean
    'Developer Guide No.7
    Private Const vbFormCode As Integer = 0
    Private oAutoMTA As Object

    Public Property BackDatedMTAsAllowed() As Boolean
        Get
            Return m_bBackDatedMTAsAllowed
        End Get
        Set(ByVal Value As Boolean)
            m_bBackDatedMTAsAllowed = Value
        End Set
    End Property
    Public Property SelectedPolicyStatus() As String
        Get
            Return m_sSelectedPolicyStatus
        End Get
        Set(ByVal Value As String)
            m_sSelectedPolicyStatus = Value
        End Set
    End Property
    Public Property IsMTATemp() As String
        Get
            Return CStr(m_bIsMTATemp)
        End Get
        Set(ByVal Value As String)
            m_bIsMTATemp = CBool(Value)
        End Set
    End Property
    Public Property Renewaldate() As Date
        Get
            Return m_dtRenewaldate
        End Get
        Set(ByVal Value As Date)
            m_dtRenewaldate = Value
        End Set
    End Property
    Public Property IsPriorDate() As Boolean
        Get
            Return m_bIsPriorDate
        End Get
        Set(ByVal Value As Boolean)
            m_bIsPriorDate = Value
        End Set
    End Property
    Public Property IsRenewed() As Boolean
        Get
            Return m_bIsRenewed
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRenewed = Value
        End Set
    End Property
    Public Property LapsedDate() As Date
        Get
            Return m_dtLapsedDate
        End Get
        Set(ByVal Value As Date)
            m_dtLapsedDate = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property
    'ED 18072002

    Public Property IsUnderwriting() As Boolean
        Get
            Return m_bUnderwriting
        End Get
        Set(ByVal Value As Boolean)
            m_bUnderwriting = Value
        End Set
    End Property
    'ED 18072002 (End)
    ' RAM20050825 - Bug fix for PN 23018
    Public Property OriginalInsuranceFileCnt() As Integer
        Get
            Return m_lOriginalInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lOriginalInsuranceFileCnt = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMTADate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        ' Click event of the Cancel button.
        Try

            If tabMTAtab.Visible Then
                tabMTAtab.Visible = False
                uctPMUListPolicy1.Visible = True

                uctPMUListPolicy1.lvwSearchDetailsSetFocus()
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctPMUListPolicy1.CancelClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                'Developer Guide No. 231
                Me.Hide()
            End If

            'Unlock Current Policy 'PN35753 --RC
            If m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA Or m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then
                UNLOCKPOLICY()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    ''' <summary>
    ''' cmdOK_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim nType As Integer
        Dim sMTADate As String = ""
        Dim dtMTADate As Date
        Dim nVersion As Integer
        Dim nErrorCode As Integer
        Dim sMessage As String = ""
        Dim nNewInsuranceFileCnt As Integer
        Dim bPermanentMTA As Boolean
        Dim nRenewalStatus As Integer
        Dim vbMsg As DialogResult
        Dim sDoNotDefaultMTADate As String = ""

        Dim oBusinessPolicy As bPMUPolicy.Business
        Dim obSirListRisk As bSIRListRisks.Business
        Dim nRecordsAffected As Integer
        Dim nRow As Integer
        Dim nClaimStatus As Integer
        Dim bIsBackdateMTA As Boolean
        Dim nBaseInsuranceFileCnt As Integer
        Dim dtMTAEndDate As Object
        Dim m_dtCancellationDate As Date
        Dim bBrokerlink As Boolean
        Dim vValue As Object
        Dim bNexus, bQuoteExpired As Boolean
        Dim nUserMTAAuthority As Integer
        Dim sMsgTitle As String = ""
        Dim nSubErrorCode As Integer
        ' Click event of the OK button.
        Dim r_vArray(,) As Object
        Dim nInsuranceFileStatusId As Integer
        Dim sInsuranceFileStatus As String = ""
        Dim nDontProceed As Integer
        Dim bIsWritten As Boolean
        Dim sDisableTempMTA As String
        Dim bDisableTempMTA As Boolean

        Dim r_dtResult As DataTable
        Dim bRetainAnniversaryCopy As Boolean
        Dim nIsTrueMonthlyPolicy As Long
        Dim nDoNotDeleteRenewalQuoteOnMta As Long
        Dim nDeleteRenQuoteReRunRenewal As Long
        Dim nAnniversaryCopy As Integer
        Dim oOOSResultArray(,) As Object = Nothing

        Try

            m_bIsRenewed = False
            m_bIsPriorDate = False
            bIsWritten = False
            nRow = uctPMUListPolicy1.SelectedItem

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5116, r_sOptionValue:=sDisableTempMTA)
            If sDisableTempMTA = "1" Then
                bDisableTempMTA = True
            Else
                bDisableTempMTA = False
            End If

            If bDisableTempMTA = True Then
                chkPermanentMTA.CheckState = CheckState.Checked
                chkPermanentMTA.Enabled = False
            End If

            If chkPermanentMTA.CheckState = "1" Or uctPMUListPolicy1.m_vSearchData(5, nRow) = "MTA Quotation Permanent" Then
                m_bIsMTATemp = False
            Else
                If bDisableTempMTA = True Then
                    m_bIsMTATemp = False
                Else
                    m_bIsMTATemp = True
                End If
            End If

            m_bDontProceedMarkedForCollection = False
            m_lReturn = CheckMarkedForCollection(r_iDontProceed:=nDontProceed)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            If nDontProceed = 1 Then
                m_bDontProceedMarkedForCollection = True
                Exit Sub
            End If

            ' Fetch user level authority
            m_lReturn = GetOutOfSequenceMTAUserAuthority(nUserMTAAuthority)

            If Not IsUnderwriting Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK

                If m_sTransactionType = gPMConstants.PMTransactionTypeMTA Then
                    m_lReturn = CheckIfBrokerlinkRisk(v_lInsuranceFileCnt:=uctPMUListPolicy1.InsFileCnt, r_bBrokerlink:=bBrokerlink)
                    If bBrokerlink Then
                        nRow = uctPMUListPolicy1.SelectedItem
                        If uctPMUListPolicy1.m_vSearchData(11, nRow).ToUpper() = "REPLACED" Then
                            MessageBox.Show("You cannot process an MTA on a replaced policy version", "Brokerlink", MessageBoxButtons.OK)
                            Exit Sub
                        End If
                    End If

                    m_lReturn = CheckIfNexusRisk(v_lInsuranceFileCnt:=uctPMUListPolicy1.InsFileCnt, r_bNexus:=bNexus)
                    If bNexus Then
                        nRow = uctPMUListPolicy1.SelectedItem
                        If (uctPMUListPolicy1.m_vSearchData(11, nRow).ToUpper() = "MTAQTETEMP") And (uctPMUListPolicy1.m_vSearchData(11, nRow).ToUpper() = "MTATEMP") Then

                            'Check Selected Quote Expiry Date
                            m_lReturn = CheckIfQuoteExpired(v_lInsuranceFileCnt:=uctPMUListPolicy1.InsFileCnt, r_bQuoteExpired:=bQuoteExpired)
                            If bQuoteExpired Then
                                MessageBox.Show("Quote Expired.", "Error", MessageBoxButtons.OK)
                            End If

                        End If
                    End If

                End If

                ' Process the OK in the control
                m_lReturn = uctPMUListPolicy1.OKClick()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                m_lInsuranceFileCnt = uctPMUListPolicy1.InsFileCnt
                m_lInsuranceFolderCnt = uctPMUListPolicy1.InsuranceFolderCnt
                m_sInsuranceRef = uctPMUListPolicy1.InsReference
                Me.Hide()
                Exit Sub
            End If

            If tabMTAtab.Visible Then

                bIsBackdateMTA = False

                dtMTADate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtMTADate))

                If CInt(dtMTADate.ToOADate) = -1 Then
                    MessageBox.Show("Please enter a valid date in order to proceed.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtMTADate.Focus()
                    Exit Sub
                End If

                bPermanentMTA = (chkPermanentMTA.CheckState = CheckState.Checked)
                nRow = uctPMUListPolicy1.SelectedItem

                m_dtRenewaldate = CDate(uctPMUListPolicy1.m_vSearchData(7, nRow))

                If uctPMUListPolicy1.m_vSearchData(11, nRow) = "" And uctPMUListPolicy1.m_vSearchData(12, nRow) = 2 And uctPMUListPolicy1.m_vSearchData(29, nRow) > 1 Then
                    m_bIsRenewed = True
                    If dtMTADate < m_dtRenewaldate Then
                        m_bIsPriorDate = True
                    End If
                End If

                If uctPMUListPolicy1.m_vSearchData(11, nRow) = "Cancelled" Then
                    For iCount As Integer = 0 To uctPMUListPolicy1.m_vSearchData.GetUpperBound(1)
                        If (uctPMUListPolicy1.m_vSearchData(12, iCount)) = 8 And IsDate(uctPMUListPolicy1.m_vSearchData(20, iCount)) Then
                            m_dtLapsedDate = CDate(uctPMUListPolicy1.m_vSearchData(20, iCount))
                        End If
                    Next iCount
                End If

                If uctPMUListPolicy1.m_vSearchData(11, nRow) = "Lapsed" Then
                    For iLoop As Integer = 0 To uctPMUListPolicy1.m_vSearchData.GetUpperBound(1)
                        If uctPMUListPolicy1.m_vSearchData(20, iLoop) <> "" And IsDate(uctPMUListPolicy1.m_vSearchData(20, iLoop)) And uctPMUListPolicy1.m_vSearchData(29, iLoop) <> "" Then
                            m_dtLapsedDate = CDate(uctPMUListPolicy1.m_vSearchData(20, iLoop))
                        End If
                    Next iLoop
                End If
                If (uctPMUListPolicy1.m_vSearchData(11, nRow) = "Under Renewal") Then
                    For iCount As Integer = 0 To UBound(uctPMUListPolicy1.m_vSearchData, 2)
                        If (uctPMUListPolicy1.m_vSearchData(12, iCount)) = 11 Then
                            bIsWritten = True
                        End If
                    Next iCount
                End If
                'if this is a lapsed policy, double check that the user wants to do a permanent MTA
                If m_sSelectedPolicyStatus = "Lapsed" And chkPermanentMTA.CheckState = "1" Then
                    If MsgBox("This is a lapsed policy and you are doing a permanent MTA , do you wish to proceed",
                        vbOKCancel, "Lapsed Policy") = vbCancel Then
                        'If temporary MTA are allowed on this system then de-check permanent MTA tick box
                        If bDisableTempMTA = False Then
                            chkPermanentMTA.CheckState = CheckState.Unchecked
                        End If
                        Exit Sub
                    End If
                End If

                'Pass in whether we're doing a permanent MTA or not
                'This is only relevant in the business object if we're MTAing an Underwriting policy
                If bPermanentMTA Then
                    nErrorCode = 1
                Else
                    nErrorCode = 0
                End If
                ' If this is a reinstatement we have already found and validate the cancelled version
                If m_sTransactionType = "MTR" Then
                    m_lReturn = uctPMUListPolicy1.GetLatestPolicyVersion(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                     r_lPolicyVersion:=nVersion)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        Exit Sub
                    End If
                    m_lReturn = uctPMUListPolicy1.GetCancellationDate(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, m_dtCancellationDate:=m_dtCancellationDate, r_lInsFileCnt:=m_lInsuranceFileCnt)
                    If m_dtCancellationDate > dtMTADate Then
                        MessageBox.Show("Reinstatement date cannot be prior than policy cancellation date. " & Strings.Chr(13) & Strings.Chr(10) & "Cannot continue.", "Find Policy", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtMTADate.Text = m_dtCancellationDate
                        Exit Sub
                    End If

                    If m_dtCancellationDate < dtMTADate Then
                        ' check if it's OOS reinstatement, enforce original cancellation date
                        m_lReturn = uctPMUListPolicy1.GetVersionByDate(r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_dtStartDate:=m_dtCancellationDate, r_lPolicyVersion:=nVersion, r_lErrorCode:=nErrorCode, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_lSubErrorCode:=nSubErrorCode)
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Exit Sub
                        End If
                        If nErrorCode = 8 Then
                            MsgBox("The Out of Sequence Reinstatement date must be from original cancellation date. This function is not Permitted." & vbCrLf &
                                        "Click OK to continue.", vbCritical, "Find Policy")
                            txtMTADate.Text = m_dtCancellationDate
                            Exit Sub
                        End If
                    End If
                End If
                'Pass in whether we're doing a permanent MTA or not
                'This is only relevant in the business object if we're MTAing an Underwriting policy
                If bIsWritten = False Then
                    If bPermanentMTA Then
                        nErrorCode = 1
                    Else
                        nErrorCode = 0
                    End If
                Else
                    nErrorCode = 0
                End If

                m_lReturn = uctPMUListPolicy1.GetVersionByDate(r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_dtStartDate:=dtMTADate, r_lPolicyVersion:=nVersion, r_lErrorCode:=nErrorCode, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_lSubErrorCode:=nSubErrorCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                If nErrorCode <> 0 Then
                    Select Case nErrorCode
                        Case 1
                            sMessage = "Future adjustment found"
                            sMsgTitle = "Find Policy"
                        Case 2
                            sMessage = "Overlaps with temporary MTA"
                            sMsgTitle = "Find Policy"
                        Case 3
                            If nSubErrorCode = 0 Then
                                sMessage = "MTA Effective date is before original inception date of the policy. Cannot continue"
                            Else
                                sMessage = "No version found"
                            End If
                            sMsgTitle = "Find Policy"
                        Case 4
                            sMessage = "This policy version is attached to a closed branch which doesn't allow permanent MTAs."
                            sMsgTitle = "Find Policy"
                        Case 5
                            sMessage = "This policy version is attached to a closed branch which doesn't allow temporary MTAs."
                            sMsgTitle = "Find Policy"
                        Case 6
                            If m_sTransactionType = "MTA" Then
                                sMessage = "MTA Effective date is before original inception date of the policy. Cannot continue"
                            ElseIf m_sTransactionType = "MTC" Then
                                sMessage = "S4I does not support Out of Sequence MTAs prior to the Inception Date of the Policy. Please Contact your system administrator"
                            End If
                            sMsgTitle = "Out of Sequence MTA"
                        Case 7
                            sMessage = "This Policy's product is not configured to allow an Out of Sequence MTA or Cancellation or Reinstatement before the last Renewal or Inception Date." & Strings.Chr(13) & Strings.Chr(10) &
                                       "Please contact your System Administrator."
                            sMsgTitle = "Out of Sequence MTA"
                        Case 8
                            If m_sTransactionType = "MTC" Then
                                sMessage = "Cancellation Date  is earlier than the latest permanent change. Is this a  backdated cancellation?"
                                sMsgTitle = "Out of Sequence MTC"
                            ElseIf m_sTransactionType = "MTR" Then
                                sMessage = "This is Out of Sequence Reinstatement, do you wish to proceed?"
                                sMsgTitle = "Out of Sequence MTR"
                            Else
                                sMessage = "MTA Effective Date is prior to a previous transaction effective date. This will result in previous transactions " &
                                           "being reversed." & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?"
                                sMsgTitle = "Out of Sequence MTA"
                            End If
                        Case 9
                            sMessage = "This Policy's product is not configured to allow an Out of Sequence MTA or Cancellation or Reinstatement." & Strings.Chr(13) & Strings.Chr(10) &
                                       "Please contact your System Administrator."
                            sMsgTitle = "Out of Sequence MTA"
                        Case 10
                            sMessage = "This Policy's product is not configured to allow an Out of Sequence MTA or Cancellation or Reinstatement before the last Renewal Period - 1 or Inception Date." & Strings.Chr(13) & Strings.Chr(10) &
                                       "Please contact your System Administrator."
                            sMsgTitle = "Out of Sequence MTA"
                    End Select

                    If nErrorCode > 6 Then 'It can be a Backdate MTA check user authority before proceeding
                        If nUserMTAAuthority = MTA_AUTHORITY_NOT_ALLOWED Then
                            MessageBox.Show("You are not permitted to process an Out of Sequence MTA or Cancellation or Reinstatement." & Strings.Chr(13) & Strings.Chr(10) &
                                            "Please contact your System Administrator.", "Out of Sequence MTA", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If

                    'Allowing back dated cancellation

                    If m_sTransactionType = "MTC" OrElse m_sTransactionType = "MTR" Then
                        m_lInsuranceFileCnt = uctPMUListPolicy1.InsFileCnt
                        'Get an instance of the policy business object via
                        'the public object manager.
                        Dim temp_oBusinessPolicy As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oBusinessPolicy, "bPMUPolicy.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        oBusinessPolicy = temp_oBusinessPolicy

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the policy business object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                            Exit Sub
                        End If

                        'UPGRADE_TODO: (1067) Member BackDatedCanAllowed is not defined in type bPMUPolicy.Business. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                        m_lReturn = oBusinessPolicy.BackDatedCanAllowed(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, vvalue:=vValue)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the Risk Status", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                            Exit Sub
                        End If

                        'Terminate the Business Policy Object
                        'UPGRADE_TODO: (1067) Member Terminate is not defined in type bPMUPolicy.Business. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                        oBusinessPolicy.Dispose()
                    End If

                    If Information.IsArray(vValue) Then
                        m_bBackDatedCanAllowed = gPMFunctions.ToSafeInteger(vValue(0, 0), 0) = 1
                    End If

                    If nErrorCode <> 8 Then
                        MessageBox.Show(sMessage, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    ElseIf (nErrorCode = 8 And m_bBackDatedMTAsAllowed) Or (nErrorCode = 8 And Not m_bBackDatedMTAsAllowed And m_bBackDatedCanAllowed) Then  'Backdated MTA
                        bIsBackdateMTA = True

                        'check if there is any claim

                        m_lReturn = uctPMUListPolicy1.CheckInClaim(v_sInsuranceRef:=m_sInsuranceRef, r_lClaimStatus:=nClaimStatus, v_dtStartDate:=dtMTADate)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If
                        If nClaimStatus <> -1 Then
                            If nUserMTAAuthority = MTA_AUTHORITY_WITHOUT_CLAIMS Then
                                MessageBox.Show("You are not permitted to process an Out of Sequence MTA or Cancellation where there has been " &
                                                "a claim on this policy after the effective date entered." & Strings.Chr(13) & Strings.Chr(10) &
                                                "Please contact your System Administrator.", "Out of Sequence MTA", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Exit Sub
                            ElseIf nUserMTAAuthority = MTA_AUTHORITY_WITH_CLAIMS Then
                                vbMsg = MessageBox.Show("Warning - A claim has been lodged after the effective date of this MTA." &
                                        Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?", "Find Policy", MessageBoxButtons.YesNo)
                                If vbMsg = System.Windows.Forms.DialogResult.No Then
                                    Exit Sub
                                End If
                            End If
                        End If
                        If MessageBox.Show(sMessage, sMsgTitle, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                            Exit Sub
                        End If

                    ElseIf (nErrorCode = 8 And Not m_bBackDatedMTAsAllowed And Not m_bBackDatedCanAllowed) Then
                        MessageBox.Show("MTA Effective Date is prior to a previous transaction effective date." &
                                        "This function is not Permitted." & Strings.Chr(13) & Strings.Chr(10) & "Click OK to Continue.", "Find Policy", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                m_lReturn = g_oObjectManager.GetInstance(oObject:=oAutoMTA,
                                       sClassName:="bSIRAutoMTA.Business",
                                       vInstanceManager:=PMGetViaClientManager)

                If (m_lReturn <> PMEReturnCode.PMTrue) Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the policy business object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If

                m_lReturn = oAutoMTA.GetSavedOOSQuotes(
                                v_nInsuranceFolderCnt:=m_lInsuranceFolderCnt,
                                v_nInsuranceFileCnt:=m_lInsuranceFileCnt,
                                r_oResults:=oOOSResultArray)
                If (m_lReturn <> PMEReturnCode.PMTrue) Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the Risk Status", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If

                If oOOSResultArray IsNot Nothing AndAlso IsArray(oOOSResultArray) Then
                    vbMsg = MsgBox("Making this MTA live will result in automatic cancellation" & vbCrLf & "all existing backdated OOS MTA quotes for this policy. " & vbCrLf & vbCrLf & "Are you sure you wish to proceed with this MTA?", vbYesNo + vbQuestion, ToSafeString(m_sTransactionType))
                    If (vbMsg = vbNo) Then
                        'Terminate the Business Policy Object
                        m_lReturn = oAutoMTA.Dispose()
                        Exit Sub
                    End If
                End If

                'Terminate the Business Policy Object
                m_lReturn = oAutoMTA.Dispose()

                m_lReturn = uctPMUListPolicy1.GetRenewalDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_dtResult:=r_dtResult)

                If (r_dtResult IsNot Nothing AndAlso r_dtResult.Rows.Count > 0) Then
                    If r_dtResult.Rows.Count > 1 Then
                        For nLoop As Integer = 0 To r_dtResult.Rows.Count - 1

                            nRenewalStatus = CLng(r_dtResult.Rows(nLoop).Item(0))
                            nIsTrueMonthlyPolicy = CLng(r_dtResult.Rows(nLoop).Item(1))
                            nDoNotDeleteRenewalQuoteOnMta = CLng(r_dtResult.Rows(nLoop).Item(2))
                            nAnniversaryCopy = CLng(r_dtResult.Rows(nLoop).Item(3))

                            If (nRenewalStatus <> -1 And (nDoNotDeleteRenewalQuoteOnMta = 1) And (nIsTrueMonthlyPolicy = 1) And (nAnniversaryCopy = 1)) Then
                                vbMsg = MsgBox("This policy is in renewal. Please confirm that you will apply your amendment to the renewal version of this policy manually", vbYesNo, "Find Policy")
                                If (vbMsg = vbNo) Then
                                    Exit Sub
                                End If
                            End If
                        Next nLoop
                    Else
                        nRenewalStatus = CLng(r_dtResult.Rows(0).Item(0))
                        nIsTrueMonthlyPolicy = CLng(r_dtResult.Rows(0).Item(1))
                        nDoNotDeleteRenewalQuoteOnMta = CLng(r_dtResult.Rows(0).Item(2))
                        nAnniversaryCopy = CLng(r_dtResult.Rows(0).Item(3))
                        nDeleteRenQuoteReRunRenewal = CLng(r_dtResult.Rows(0).Item(5))
                        If (nRenewalStatus <> -1) Then
                            If (nDoNotDeleteRenewalQuoteOnMta = 0 AndAlso nDeleteRenQuoteReRunRenewal = 0) Or m_sTransactionType = "MTC" Then
                                vbMsg = MsgBox("This policy is in renewal" & vbCrLf & "Do you wish to proceed?", vbYesNo, "Find Policy")
                                If (vbMsg = vbYes) Then
                                    m_lReturn = DeletePolicyFromRenewal(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

                                    If Not (m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMNotFound) Then
                                        Exit Sub
                                    End If
                                Else
                                    Exit Sub
                                End If

                            ElseIf ((nDoNotDeleteRenewalQuoteOnMta = 1) And (nIsTrueMonthlyPolicy = 0)) Then
                                vbMsg = MsgBox("This policy is in renewal. Please confirm that you will apply your amendment to the renewal version of this policy manually", vbYesNo, "Find Policy")
                                If (vbMsg = vbNo) Then
                                    Exit Sub
                                End If

                            ElseIf ((nDoNotDeleteRenewalQuoteOnMta = 1) And (nIsTrueMonthlyPolicy = 1) And (nAnniversaryCopy = 1)) Then
                                vbMsg = MsgBox("This policy is in renewal. Please confirm that you will apply your amendment to the renewal version of this policy manually", vbYesNo, "Find Policy")
                                If (vbMsg = vbNo) Then
                                    Exit Sub
                                End If
                                bRetainAnniversaryCopy = True
                            ElseIf ((nDoNotDeleteRenewalQuoteOnMta = 1) And (nIsTrueMonthlyPolicy = 1) And (nAnniversaryCopy = 0)) Then
                                vbMsg = MsgBox("This policy is in renewal" & vbCrLf & "Do you wish to proceed?", vbYesNo, "Find Policy")
                                If (vbMsg = vbNo) Then
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                End If

                m_lReturn = uctPMUListPolicy1.GetInsuranceFileStatus(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vArray:=r_vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                If Information.IsArray(r_vArray) Then

                    If CStr(r_vArray(0, 0)) <> "" And CStr(r_vArray(1, 0)) <> "" Then

                        nInsuranceFileStatusId = gPMFunctions.ToSafeLong(CStr(r_vArray(0, 0)), 0)

                        sInsuranceFileStatus = gPMFunctions.ToSafeString(CStr(r_vArray(1, 0)), "")

                        If m_sSelectedPolicyStatus.ToUpper() = "CANCELLED" Then
                            m_sSelectedPolicyStatus = sInsuranceFileStatus.Trim()
                        End If
                    End If
                End If

                If bIsBackdateMTA Then
                    If nErrorCode = 8 And m_sTransactionType = "MTR" Then
                        ' Just set the version to -1 so Copy Policy will increment the current version
                        nVersion = -1
                    End If

                    'get backdatebaseinsurancefilecount
                    m_lReturn = uctPMUListPolicy1.GetBasePolicyCntForBackDateMTA(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_dtMTADate:=dtMTADate, lBaseInsuranceFileCnt:=nBaseInsuranceFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to fetch the BaseInsuranceFile", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                        Exit Sub
                    End If

                    'In case of BackDated MTA update the InsFileCnt with actual base cnt
                    If nBaseInsuranceFileCnt <> -1 Then
                        m_lInsuranceFileCnt = nBaseInsuranceFileCnt
                    End If

                    m_lReturn = uctPMUListPolicy1.GetCoverEndDate(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_dtMTADate:=dtMTADate, lBaseInsuranceFileCnt:=nBaseInsuranceFileCnt, dtMTAEndDate:=dtMTAEndDate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to fetch the GetCoverFromDate", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                        Exit Sub
                    End If

                End If

                If Not bIsBackdateMTA Then
                    m_lReturn = uctPMUListPolicy1.CopyPolicy(v_lOldInsuranceFileCnt:=m_lInsuranceFileCnt, r_lNewInsuranceFileCnt:=nNewInsuranceFileCnt, v_lVersion:=nVersion, v_bPermanentMTA:=bPermanentMTA, v_dtMTADate:=dtMTADate)
                Else
                    m_lReturn = uctPMUListPolicy1.CopyPolicy(v_lOldInsuranceFileCnt:=m_lInsuranceFileCnt, r_lNewInsuranceFileCnt:=nNewInsuranceFileCnt, v_lVersion:=nVersion, v_bPermanentMTA:=bPermanentMTA, v_dtMTADate:=dtMTADate, v_vMTAEndDate:=dtMTAEndDate)
                End If

                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lOriginalInsuranceFileCnt = m_lInsuranceFileCnt

                    m_lInsuranceFileCnt = nNewInsuranceFileCnt

                    Dim vCopyRiskOnMTA As Object = ""
                    m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTCopyRiskInMTA, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vCopyRiskOnMTA)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get product option", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                        Exit Sub
                    End If
                    If vCopyRiskOnMTA = "1" AndAlso m_sTransactionType = "MTA" Then
                        Dim temp_obSirListRisk As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_obSirListRisk, "bSirListRisks.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        obSirListRisk = temp_obSirListRisk

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the list risk business object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                            Exit Sub
                        End If

                        m_lReturn = obSirListRisk.CopyRisksMTA(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, bCopyRiskMTA:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to copy Risks for MTA", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                            Exit Sub
                        End If
                        obSirListRisk.Dispose()
                        'Destroy the instance of the business object from memory.
                        obSirListRisk = Nothing
                    End If

                    'RKS 30/11/2004 PN16596 - Setting Risk Status to "unquoted"
                    If m_sTransactionType = "MTR" Or m_sTransactionType = "MTC" Then
                        'Get an instance of the policy business object via
                        'the public object manager.
                        Dim temp_oBusinessPolicy As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oBusinessPolicy, "bPMUPolicy.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        oBusinessPolicy = temp_oBusinessPolicy

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the policy business object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                            Exit Sub
                        End If

                        'Set all risks to "unquoted" when reinstating
                        m_lReturn = oBusinessPolicy.SetRisksUnquoted(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lRecordsAffected:=nRecordsAffected)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the Risk Status", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                            Exit Sub
                        End If

                        'Terminate the Business Policy Object

                        oBusinessPolicy.Dispose()
                        'Destroy the instance of the business object from memory.
                        oBusinessPolicy = Nothing
                    End If

                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                    Exit Sub
                End If

                tabMTAtab.Visible = False
                uctPMUListPolicy1.Visible = True

                Exit Sub

            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the OK in the control
            m_lReturn = uctPMUListPolicy1.OKClick()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Get the insurance file and folder count
            m_lInsuranceFileCnt = uctPMUListPolicy1.InsFileCnt
            m_lInsuranceFolderCnt = uctPMUListPolicy1.InsuranceFolderCnt
            m_sInsuranceRef = uctPMUListPolicy1.InsReference

            'Right, new stuff now (13/6/2001)
            'We can call this as part of an 'Edit Policy' roadmap, as well as MTA.
            'In this case, we do something slightly different -
            If m_sTransactionType = "EDIT" Then
                'We check that it's not in renewal, just like above
                m_lReturn = uctPMUListPolicy1.CheckInRenewal(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lRenewalStatus:=nRenewalStatus)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                If nRenewalStatus <> -1 Then
                    vbMsg = MessageBox.Show("This policy is in renewal" & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to proceed?", "Find Policy", MessageBoxButtons.YesNo)

                    If vbMsg = System.Windows.Forms.DialogResult.Yes Then
                        m_lReturn = DeletePolicyFromRenewal(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

                        If Not (m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMNotFound) Then
                            Exit Sub
                        End If
                    Else
                        Exit Sub
                    End If
                End If

                'Then we copy it
                m_lReturn = uctPMUListPolicy1.CopyPolicyForEdit(v_lOldInsuranceFileCnt:=m_lInsuranceFileCnt, r_lNewInsuranceFileCnt:=nNewInsuranceFileCnt)

                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    ' RAM20050825 - PN 23018 - Store the original InsuranceFileCnt
                    m_lOriginalInsuranceFileCnt = m_lInsuranceFileCnt

                    m_lInsuranceFileCnt = nNewInsuranceFileCnt
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                    Exit Sub
                End If

                Exit Sub
            ElseIf m_sTransactionType = "MTA" Or m_sTransactionType = "MTC" Then
                'Reloading MTA from saved quote, get the last live policy version from the internal array
                For nRow = uctPMUListPolicy1.m_vSearchData.GetLowerBound(1) To uctPMUListPolicy1.m_vSearchData.GetUpperBound(1)
                    If uctPMUListPolicy1.m_vSearchData(12, nRow) = "2" Then
                        'Last live version

                        m_lOriginalInsuranceFileCnt = CInt(uctPMUListPolicy1.m_vSearchData(1, nRow))
                        Exit For
                    End If
                Next nRow
            End If

            nType = uctPMUListPolicy1.InsuranceFileTypeID

            'We should have back 2, 4 or 7.  If it's 2 we're looking at the main policy, and we
            'need to make a copy.  Otherwise we can just get on with this one.
            'JMK 19/07/2001... and 5 or 6 - treat as 2 if it's 5 or 6 (MTA PERM or TEMP)

            'Tomo200902 - MTA Temp Quote updating renewal status is wrong
            Select Case nType
                ' Quotes - Temp MTA or Reinstatement
                Case 7, 10
                    'It's MTA Temp Quote so do nothing

                    Me.Hide()

                    ' Quotes - Perm MTA
                Case 4, 12

                    'Start Girija - PN 55381
                    If (m_bBackDatedMTAsAllowed And m_sSelectedPolicyStatus = "Cancelled") Then
                        chkPermanentMTA.CheckState = CheckState.Unchecked
                        chkPermanentMTA.Enabled = False
                        Exit Sub
                    End If
                    If Not bDisableTempMTA And (m_bBackDatedMTAsAllowed And m_sSelectedPolicyStatus = "Lapsed") Then
                        chkPermanentMTA.CheckState = CheckState.Unchecked
                        chkPermanentMTA.Enabled = False
                        Exit Sub
                    End If

                    m_lReturn = uctPMUListPolicy1.GetRenewalDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_dtResult:=r_dtResult)
                    If (m_lReturn <> PMEReturnCode.PMTrue) Then
                        Exit Sub
                    End If
                    Dim lRenewalStatus As Integer
                    If r_dtResult IsNot Nothing AndAlso r_dtResult.Rows.Count > 0 Then
                        lRenewalStatus = CLng(r_dtResult.Rows(0).Item(0))
                        nIsTrueMonthlyPolicy = CLng(r_dtResult.Rows(0).Item(1))
                        nDoNotDeleteRenewalQuoteOnMta = CLng(r_dtResult.Rows(0).Item(2))
                        nAnniversaryCopy = CInt(r_dtResult.Rows(0).Item(3))
                        nDeleteRenQuoteReRunRenewal = CInt(r_dtResult.Rows(0).Item(4))


                        If (lRenewalStatus <> -1) Then
                            If (nDoNotDeleteRenewalQuoteOnMta = 0 And nDeleteRenQuoteReRunRenewal = 0) Then
                                vbMsg = MsgBox("This policy is in renewal" & vbCrLf & "Do you wish to proceed?", vbYesNo, "Find Policy")
                                If (vbMsg = vbNo) Then
                                    Exit Sub
                                End If

                            ElseIf ((nDoNotDeleteRenewalQuoteOnMta = 1) And (nIsTrueMonthlyPolicy = 0)) Then
                                vbMsg = MsgBox("This policy is in renewal. Please confirm that you will apply your amendment to the renewal version of this policy manually", vbYesNo, "Find Policy")
                                If (vbMsg = vbNo) Then
                                    Exit Sub
                                End If

                            ElseIf ((nDoNotDeleteRenewalQuoteOnMta = 1) And (nIsTrueMonthlyPolicy = 1) And (nAnniversaryCopy = 1)) Then
                                vbMsg = MsgBox("This policy is in renewal. Please confirm that you will apply your amendment to the renewal version of this policy manually", vbYesNo, "Find Policy")
                                If (vbMsg = vbNo) Then
                                    Exit Sub
                                End If
                                bRetainAnniversaryCopy = True
                            ElseIf ((nDoNotDeleteRenewalQuoteOnMta = 1) And (nIsTrueMonthlyPolicy = 1) And (nAnniversaryCopy = 0)) Then
                                vbMsg = MsgBox("This policy is in renewal" & vbCrLf & "Do you wish to proceed?", vbYesNo, "Find Policy")
                                If (vbMsg = vbNo) Then
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If

                    Me.Hide()

                    ' Cancelled
                Case 8
                    ' If we are reinstating a cancelled policy then we need to get the reinstatement date
                    If m_sTransactionType = "MTR" Then
                        ' Check and hide permanent checkbox
                        chkPermanentMTA.CheckState = CheckState.Checked
                        chkPermanentMTA.Visible = False

                        lblPermanentMTA.Visible = False

                        ' Set new captions
                        SSTabHelper.SetTabCaption(tabMTAtab, 0, "1 - Reinstate Policy")
                        lblMTADate.Text = "Reinstatement Date:"

                        lblMTADate.Width = VB6.TwipsToPixelsX(2000)

                        txtMTADate.Left = VB6.TwipsToPixelsX(4545)

                        ' Set default date to start date (this is actually the cancellation date)
                        m_lReturn = m_oFormFields.FormatControl(txtMTADate, uctPMUListPolicy1.CoverStartDate)

                        uctPMUListPolicy1.Visible = False
                        tabMTAtab.Visible = True
                    Else
                        MessageBox.Show("Please select valid version of policy", "Policy Version", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                    'Policy, MTA, Temp MTA or Reinstated,MTC
                Case 2, 5, 6, 9, 12
                    chkPermanentMTA.CheckState = CheckState.Checked

                    If nType = 2 Or nType = 5 Or nType = 6 Then 'Check if MTA
                        If (m_sSelectedPolicyStatus = "Cancelled") Then
                            chkPermanentMTA.CheckState = CheckState.Unchecked
                            chkPermanentMTA.Enabled = False
                        End If
                        If Not bDisableTempMTA And (m_sSelectedPolicyStatus = "Lapsed") Then
                            chkPermanentMTA.CheckState = CheckState.Unchecked
                            chkPermanentMTA.Enabled = False
                        End If
                    End If


                    If m_sTransactionType = "MTR" Or m_sTransactionType = "MTC" Then
                        ' Check and hide permanent checkbox
                        chkPermanentMTA.Visible = False
                        lblPermanentMTA.Visible = False

                        If m_sTransactionType = "MTR" Then
                            ' Set new captions
                            SSTabHelper.SetTabCaption(tabMTAtab, 0, "1 - Reinstate Policy")
                            lblMTADate.Text = "Reinstatement Date:"
                        Else
                            SSTabHelper.SetTabCaption(tabMTAtab, 0, "1 - Cancel Policy")
                            lblMTADate.Text = "Cancellation Date:"
                        End If

                        lblMTADate.Width = VB6.TwipsToPixelsX(2000)
                    End If

                    ' For reinstatement default date to start date (this is actually the cancellation date)
                    If m_sTransactionType = "MTR" Then
                        m_lReturn = m_oFormFields.FormatControl(txtMTADate, uctPMUListPolicy1.CoverStartDate)
                    Else
                        'default to today's date only if system option is not set
                        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=1026, r_sOptionValue:=sDoNotDefaultMTADate)

                        If sDoNotDefaultMTADate = "1" Then
                            txtMTADate.Text = ""
                        Else
                            m_lReturn = m_oFormFields.FormatControl(txtMTADate, DateTime.Today)
                        End If
                    End If

                    uctPMUListPolicy1.Visible = False
                    tabMTAtab.Visible = True
                    If txtMTADate.Text = "" Then
                        txtMTADate.Focus()
                    End If

                Case Else
                    MessageBox.Show("Please select valid version of policy", "Policy Version", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Select


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to terminate the policy business object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
            End If
            'Destroy the instance of the business object from memory.
            oBusinessPolicy = Nothing

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        Finally

            If Not oBusinessPolicy Is Nothing Then
                oBusinessPolicy.Dispose()
                oBusinessPolicy = Nothing
            End If

        End Try

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            uctPMUListPolicy1.lvwSearchDetailsSetFocus()
            cmdOK.Enabled = (uctPMUListPolicy1.Selected = gPMConstants.PMEReturnCode.PMTrue)
            Me.Text = "Policy List Version : [" & m_sInsuranceRef.Trim() & "]" ' PN19733

        End If
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_oFormFields = New iPMFormControl.FormFields()

            cmdOK.Enabled = False

            tabMTAtab.Visible = False
            tabMTAtab.Top = uctPMUListPolicy1.Top
            tabMTAtab.Left = uctPMUListPolicy1.Left

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            With uctPMUListPolicy1
                ' Task
                m_lReturn = .SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

                .InsFileCnt = m_lInsuranceFileCnt
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .ShortName = m_sShortName

            End With
            'Developer Guide No
            m_lReturn = uctPMUListPolicy1.Initialise

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = uctPMUListPolicy1.LoadControl()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = uctPMUListPolicy1.GetPolicies()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the policies.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'Start(Sriram P)PN55374
            txtMTADate.Left += VB6.TwipsToPixelsX(545)
            'End(Sriram P)PN55374
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                'Unlock Current Policy 'PN35753 --RC
                If m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA Or m_sTransactionType = "MTC" OrElse m_sTransactionType = "MTR" Then
                    UNLOCKPOLICY()
                End If

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = uctPMUListPolicy1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

            End If

            ' Terminate the control
            uctPMUListPolicy1.Dispose()
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub txtMTADate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMTADate.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMTADate)

    End Sub

    Private Sub txtMTADate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMTADate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMTADate)
    End Sub

    Private Sub uctPMUListPolicy1_lvwSearchDetailsMouseDown(ByVal Sender As Object, ByVal e As uctPMUListPolicy.lvwSearchDetailsMouseDownEventArgs) Handles uctPMUListPolicy1.lvwSearchDetailsMouseDown

        cmdOK.Enabled = (e.m_lSelected = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    '**********************************************************************
    ' remove renewal version of policy, renewal_status and all associate records
    '**********************************************************************
    Private Function DeletePolicyFromRenewal(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim bSirRenewal As Object


        Dim oRenewal As bSIRRenewal.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oRenewal, "bSIRRenewal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oRenewal = temp_oRenewal

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            result = oRenewal.DeletePolicyFromRenewal(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete policy from renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicyFromRenewal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Return result

        Finally

            If Not (oRenewal Is Nothing) Then

                oRenewal.Dispose()
                oRenewal = Nothing
            End If


        End Try
        Return result
    End Function

    Private Function CheckIfBrokerlinkRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bBrokerlink As Boolean, Optional ByRef r_sDmCode As String = "", Optional ByRef r_sQemCode As String = "") As Integer
        Dim result As Integer = 0


        Dim oGis As bGIS.Application
        Dim sQemCode, sDmCode As String
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oGis As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oGis, "bGIS.Application", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oGis = temp_oGis

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bGIS", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowBrokerlinkRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = oGis.GetQemDmCode(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sQemCode:=sQemCode, r_sDmCode:=sDmCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load risk", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowBrokerlinkRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Not (oGis Is Nothing) Then

                oGis.Dispose()
                oGis = Nothing
            End If

            If sQemCode = "BROKERLINK" Then
                r_bBrokerlink = True
            End If

            r_sDmCode = sDmCode
            r_sQemCode = sQemCode

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfBrokerlinkRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfBrokerlinkRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UNLOCKPOLICY
    '
    ' Description: UnLock Policy  'PN35753 --RC
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 18 Jul 2007
    '' ***************************************************************** '
    Private Function UNLOCKPOLICY() As Integer
        Dim result As Integer = 0


        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMLock.UnLockKey("insurance_folder_cnt", vKeyValue:=m_lInsuranceFolderCnt, iUserID:=g_oObjectManager.UserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="Error trying to unlock the policy", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If

            'Terminate the business object

            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="UNLOCKPOLICY Failed", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function CheckIfNexusRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bNexus As Boolean) As Integer
        Dim result As Integer = 0


        Dim oGis As bGIS.Application
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oGis As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oGis, "bGIS.Application", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oGis = temp_oGis

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bGIS", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfNexusRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = oGis.CheckIfNexusRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_bNexus:=r_bNexus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check If Nexus Scheme", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfNexusRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Not (oGis Is Nothing) Then

                oGis.Dispose()
                oGis = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfNexusRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfNexusRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function CheckIfQuoteExpired(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bQuoteExpired As Boolean) As Integer
        Dim result As Integer = 0


        Dim oInsurance As bSIRInsuranceFile.Services
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oInsurance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oInsurance, "bSirInsuranceFile.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oInsurance = temp_oInsurance

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bSirInsuranceFile", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfQuoteExpired", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            oInsurance.InsuranceFileCnt = v_lInsuranceFileCnt

            r_bQuoteExpired = False

            If Information.IsDate(oInsurance.QuoteExpiryDate) Then

                If DateAndTime.DateDiff("d", DateTime.Today, oInsurance.QuoteExpiryDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                    r_bQuoteExpired = True
                End If
            End If

            If Not (oInsurance Is Nothing) Then

                oInsurance.Dispose()
                oInsurance = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfQuoteExpired Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfQuoteExpired", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetOutOfSequenceMTAUserAuthority(ByRef v_iMTAAuthority As Integer) As Integer
        Dim result As Integer = 0
        Dim bACTUserAuthorities As Object


        Dim oACTUserAuthority As bACTUserAuthorities.Business
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oACTUserAuthority As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oACTUserAuthority, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oACTUserAuthority = temp_oACTUserAuthority

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                gPMFunctions.RaiseError("GetOutOfSequenceMTAUserAuthority", "Failed to get bACTUserAuthorities", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oACTUserAuthority.GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="out_of_sequence_mta_authority", v_sKeyColumn:="user_id", v_sKeyValue:=1, v_iDataType:=gPMConstants.PMEDataType.PMInteger, r_vResult:=v_iMTAAuthority)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Failed to get (out_of_sequence_mta_authority) value", "Get User Authority", MessageBoxButtons.OK)
                Return result
            End If

            If Not (oACTUserAuthority Is Nothing) Then

                oACTUserAuthority.Dispose()
                oACTUserAuthority = Nothing
            End If



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOutOfSequenceMTAUserAuthority Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOutOfSequenceMTAUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Function CheckMarkedForCollection(ByRef r_iDontProceed As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRFindInsurance As Object

        Const kMethodName As String = "CheckMarkedForCollection"


        Dim oCheckMarkedForCollection As bSIRFindInsurance.Form
        Dim r_lIsMarked As Integer
        Dim r_dMarkedDate, dttodaydate As Date
        Dim lCheckMarkedInsuranceFileCnt As Integer

        Dim lRow As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lRow = uctPMUListPolicy1.SelectedItem
            If gPMFunctions.ToSafeLong(uctPMUListPolicy1.m_vSearchData(1, lRow)) <= 0 Then
                Return result
            End If

            lCheckMarkedInsuranceFileCnt = gPMFunctions.ToSafeLong(uctPMUListPolicy1.m_vSearchData(1, lRow))

            Dim temp_oCheckMarkedForCollection As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCheckMarkedForCollection, "bSIRFindInsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCheckMarkedForCollection = temp_oCheckMarkedForCollection
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oCheckMarkedForCollection.IsMarkedForCollection(v_lInsuranceFileCnt:=lCheckMarkedInsuranceFileCnt, r_lIsMarked:=r_lIsMarked, r_dMarkedDate:=r_dMarkedDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide No.40
            dttodaydate = CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=DateTime.Now))
            If r_lIsMarked = 1 Then
                'Developer Guide No.40
                r_dMarkedDate = CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=r_dMarkedDate))
                If r_dMarkedDate = dttodaydate Then
                    If MessageBox.Show("Quote already passed for collection process," & Strings.Chr(13) & Strings.Chr(10) & "do you wish to proceed ?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.Yes Then

                        m_lReturn = oCheckMarkedForCollection.UpdateMarkedForCollectionStatus(v_lInsuranceFileCnt:=lCheckMarkedInsuranceFileCnt, r_lIsMarked:=0)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        r_iDontProceed = 1
                    End If
                Else


                    m_lReturn = oCheckMarkedForCollection.UpdateMarkedForCollectionStatus(v_lInsuranceFileCnt:=lCheckMarkedInsuranceFileCnt, r_lIsMarked:=0)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            End If

            oCheckMarkedForCollection = Nothing



        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMarkedForCollection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMarkedForCollection", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally





        End Try
        Return result
    End Function

    Private Sub uctPMUListPolicy1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uctPMUListPolicy1.Load

    End Sub
End Class
