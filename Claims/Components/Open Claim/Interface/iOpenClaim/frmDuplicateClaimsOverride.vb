Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmDuplicateClaimsOverride
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmDuplicateClaimsOverride
    '
    ' Date:
    '
    ' Description: Main interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmDuplicateClaimsOverride"

    '********************************
    ' General Property variables
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lPMAuthorityLevel As Integer
    Private m_bError As Integer
    Private m_oBusiness As Object
    Private m_lReturn As Integer
    Private m_bInterfaceError As Boolean
    '********************************

    Private m_vDuplicateClaimDetails(,) As Object
    Private m_vOverrideUserDetails(,) As Object
    Private m_sOverrideUsername As String = ""
    Private m_sSecurityModel As String

    '********************************
    ' General Interface Properties
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property
    Public ReadOnly Property Error_Renamed() As Integer
        Get
            Return m_bError
        End Get
    End Property
    '********************************

    Public WriteOnly Property DuplicateClaimDetails() As Object
        Set(ByVal Value As Object)
            m_vDuplicateClaimDetails = Value
        End Set
    End Property

    Public WriteOnly Property OverrideUserDetails() As Object
        Set(ByVal Value As Object)
            m_vOverrideUserDetails = Value
        End Set
    End Property

    Public ReadOnly Property OverrideUserName() As String
        Get
            Return m_sOverrideUsername
        End Get
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        ProcessCancel()
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        ProcessOk()
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Initialize
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Sub Form_Initialize_Renamed()

        Const kMethodName As String = "Form_Initialize"

        Dim lReturn, lSubValue As Integer

        Try



            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' initialise form error indicator
            m_bError = False

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Get an instance of the business object via
            ' the public object manager.
            'lReturn = g_oObjectManager.GetInstance(oObject:=m_oBusiness, _
            'sClassName:="BUSINESS", _
            'vInstanceManager:=PMGetViaClientManager)

            'If lReturn <> PMTrue Then
            '    RaiseError kMethodName, "Failed to get instance of BUSINESS", PMLogError
            'End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_QueryUnload
    '
    ' Description: Determines whether any actions need to take place
    '               before unload.
    ' History:
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '
    Private Sub frmDuplicateClaimsOverride_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)


        eventArgs.Cancel = Cancel <> 0
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Unload
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Sub frmDuplicateClaimsOverride_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Const kMethodName As String = "Form_Unload"

        Dim lReturn, lSubValue As Integer

        Try



            ' Terminate the business object

            m_oBusiness.Dispose()




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' destroy object reference
            m_oBusiness = Nothing



        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Load
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '

    Public Sub frmDuplicateClaimsOverride_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try



            ' set up interface
            lReturn = SetupForm()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupForm Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: SetupForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function SetupForm() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupForm"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' setup duplicate claim list defaults
            lReturn = SetUpListView()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetUpListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate duplicate claim list view
            lReturn = PopulateDuplicateClaims()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateDuplicateClaims Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate override user combo
            lReturn = CType(PopulateCombo(m_vOverrideUserDetails, cboApprovedUsers), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateCombo Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = iPMFunc.GetSystemSecurityModel(m_sSecurityModel)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemSecurityModel Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_sSecurityModel = "2" Then
                fraApproverDetails.Visible = False
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetUpListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function SetUpListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetUpListView"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwDuplicateClaims.Columns.Clear()

            'set header properties
            With lvwDuplicateClaims

                'clear the column headers
                .Columns.Clear()

                .Columns.Insert(kDupClaimsColClaimNumber - 1, "kDupClaimsColClaimNumber", "Claim Number", CInt(VB6.TwipsToPixelsX(1600)))
                .Columns.Insert(kDupClaimsColDescription - 1, "kDupClaimsColDescription", "Description", CInt(VB6.TwipsToPixelsX(1600)))
                .Columns.Insert(kDupClaimsColProgressStatus - 1, "kDupClaimsColProgressStatus", "Progress Status", CInt(VB6.TwipsToPixelsX(1400)))
                .Columns.Insert(kDupClaimsColPrimaryCause - 1, "kDupClaimsColPrimaryCause", "Primary Cause", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)
                .Columns.Insert(kDupClaimsColReportedDate - 1, "kDupClaimsColReportedDate", "Reported Date", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)
                .Columns.Insert(kDupClaimsColLastModified - 1, "kDupClaimsColLastModified", "Last Modified", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)
                .Columns.Insert(kDupClaimsColStatus - 1, "kDupClaimsColStatus", "Status", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)

            End With



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateDuplicateClaims
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function PopulateDuplicateClaims() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateDuplicateClaims"

        Dim lReturn, lLBound, lUBound As Integer
        Dim oListItem As ListViewItem

        Dim lClaimId As Integer
        Dim sClaimNumber, sClaimDescription, sProgressStatus, sPrimaryCause As String
        Dim dtReportedDate, dtLastModifiedDate As Date
        Dim sClaimStatus As String = ""
        Dim lClaimStatusId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 170
            ListViewFunc.ListViewBatchStart(lvwDuplicateClaims)

            lvwDuplicateClaims.Items.Clear()

            If Information.IsArray(m_vDuplicateClaimDetails) Then

                lLBound = m_vDuplicateClaimDetails.GetLowerBound(1)
                lUBound = m_vDuplicateClaimDetails.GetUpperBound(1)

                ' Assign the details to the interface.
                For lDuplicateClaim As Integer = lLBound To lUBound

                    ' **************************************
                    ' add item to fee list view
                    ' **************************************

                    ' get details from array
                    lClaimId = CInt(m_vDuplicateClaimDetails(kDupClaimId, lDuplicateClaim))
                    sClaimNumber = CStr(m_vDuplicateClaimDetails(kDupClaimNumber, lDuplicateClaim))
                    sClaimDescription = CStr(m_vDuplicateClaimDetails(kDupClaimDescription, lDuplicateClaim))
                    sProgressStatus = CStr(m_vDuplicateClaimDetails(kDupClaimProgressStatus, lDuplicateClaim))
                    dtReportedDate = CDate(DateTime.Parse(m_vDuplicateClaimDetails(kDupClaimReportedDate, lDuplicateClaim)).ToString("d"))
                    dtLastModifiedDate = CDate(DateTime.Parse(m_vDuplicateClaimDetails(kDupClaimModifiedDate, lDuplicateClaim)).ToString("d"))
                    sPrimaryCause = CStr(m_vDuplicateClaimDetails(kDupClaimPrimaryCause, lDuplicateClaim))
                    lClaimStatusId = CInt(m_vDuplicateClaimDetails(kDupClaimStatusId, lDuplicateClaim))

                    ' determine the hardcoded claim status
                    Select Case lClaimStatusId
                        Case CLMProvisionalOpenClaim
                            sClaimStatus = g_sPROVISIONALOPENCLAIM
                        Case CLMLiveOpenClaim
                            sClaimStatus = g_sLIVEOPENCLAIM
                        Case CLMClosed
                            sClaimStatus = g_sCLOSED
                        Case CLMReOpened
                            sClaimStatus = g_REOPENED
                        Case CLMReClosed
                            sClaimStatus = g_RECLOSED
                    End Select

                    ' add list item
                    oListItem = lvwDuplicateClaims.Items.Add(sClaimNumber)

                    ' claim description
                    ListViewHelper.GetListViewSubItem(oListItem, kDupClaimsColDescription - 1).Text = sClaimDescription

                    ' progress status
                    ListViewHelper.GetListViewSubItem(oListItem, kDupClaimsColProgressStatus - 1).Text = sProgressStatus

                    ' primary cause
                    ListViewHelper.GetListViewSubItem(oListItem, kDupClaimsColPrimaryCause - 1).Text = sPrimaryCause

                    ' reported date
                    ListViewHelper.GetListViewSubItem(oListItem, kDupClaimsColReportedDate - 1).Text = DateTimeHelper.ToString(dtReportedDate)

                    ' last modified
                    ListViewHelper.GetListViewSubItem(oListItem, kDupClaimsColLastModified - 1).Text = DateTimeHelper.ToString(dtLastModifiedDate)

                    ' claim status
                    ListViewHelper.GetListViewSubItem(oListItem, kDupClaimsColStatus - 1).Text = sClaimStatus

                    ' Store the Fee_cnt
                    oListItem.Tag = CStr(lClaimId)

                Next lDuplicateClaim

            End If
            'developer guide no. 170
            ListViewFunc.ListViewBatchEnd()

            lvwDuplicateClaims.Refresh()

            lvwDuplicateClaims.Visible = True



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateCombo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function PopulateCombo(ByVal v_vArray(,) As Object, ByVal oComboBox As ComboBox, Optional ByVal sFirstItem As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateCombo"

        Dim lReturn, lLBound, lUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If sFirstItem <> "" Then
                Dim oComboBox_NewIndex As Integer = -1
                oComboBox_NewIndex = oComboBox.Items.Add(sFirstItem)
                VB6.SetItemData(oComboBox, oComboBox_NewIndex, 0)
            End If

            If Information.IsArray(v_vArray) Then

                lLBound = v_vArray.GetLowerBound(1)
                lUBound = v_vArray.GetUpperBound(1)

                For lItem As Integer = lLBound To lUBound
                    'developer guide no. 153(latest guide)
                    oComboBox.Items.Add(New VB6.ListBoxItem(v_vArray(kDetailDesc, lItem), CInt(v_vArray(kDetailKey, lItem))))
                Next

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetArrayItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetArrayItem(ByVal v_vArray(,) As Object, ByRef r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetArrayItem"

        Dim lReturn As Integer
        Dim v_vLookupItem As String = ""
        Dim lLookupItem, lLBound, lUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(v_vArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            lLBound = v_vArray.GetLowerBound(1)
            lUBound = v_vArray.GetUpperBound(1)

            ' set lookup properties
            If r_lItemId <> 0 Then
                v_vLookupItem = CStr(r_lItemId)
                lLookupItem = 0

            ElseIf r_sItemDesc <> "" Then
                v_vLookupItem = r_sItemDesc
                lLookupItem = 1

            ElseIf r_sItemCode <> "" Then
                v_vLookupItem = r_sItemCode
                lLookupItem = 2
            End If

            ' loop around the available items in the specified array
            For lItem As Integer = lLBound To lUBound

                ' look for a match

                If CStr(v_vArray(lLookupItem, lItem)).Trim() = v_vLookupItem Then

                    ' return the requested code, id, description

                    r_sItemDesc = CStr(v_vArray(kDetailDesc, lItem)).Trim()

                    r_sItemCode = CStr(v_vArray(kDetailCode, lItem)).Trim()

                    r_lItemId = CInt(CStr(v_vArray(kDetailKey, lItem)).Trim())

                    Exit For
                End If

            Next lItem

            ' if we dont find the values specified then return false
            If r_sItemCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessOk
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessOk() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessOk"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = ValidateOverrideUserDetails()
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lStatus = gPMConstants.PMEReturnCode.PMOk
                Me.Hide()
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ValidateOverrideUserDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ValidateOverrideUserDetails() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "ValidateOverrideUserDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim nUserId As Integer = 0
        Dim sEncryptedPassword As String = String.Empty
        Dim sEnteredPassword As String = String.Empty
        Dim sEnteredEncryptedPassword As String = ""
        Dim nLBound As Integer, nUBound As Integer

        Try



            ' default to false
            nResult = gPMConstants.PMEReturnCode.PMFalse
            If m_sSecurityModel <> "2" Then
                If cboApprovedUsers.SelectedIndex = -1 Then
                    MessageBox.Show("You must select an override user to proceed", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    cboApprovedUsers.Focus()
                    Return nResult
                End If

                sEnteredPassword = txtPassword.Text

                If sEnteredPassword.Trim() = "" Then
                    MessageBox.Show("You must enter the override users password", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtPassword.Focus()
                    Return nResult
                End If

                ' get the override users id
                If cboApprovedUsers.SelectedIndex <> -1 Then
                    nUserId = VB6.GetItemData(cboApprovedUsers, cboApprovedUsers.SelectedIndex)
                End If

                ' get the encrypted password - stored in position code in the array
                lReturn = CType(GetArrayItem(m_vOverrideUserDetails, m_sOverrideUsername, sEncryptedPassword, nUserId), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'verify the password
                If bPMFunc.CheckPassword(sEnteredPassword, sEncryptedPassword) Then
                    nResult = gPMConstants.PMEReturnCode.PMTrue
                Else
                    MessageBox.Show("Invalid password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    nResult = gPMConstants.PMEReturnCode.PMError
                    txtPassword.Focus()
                End If
            Else
                Dim nFoundUser As Boolean = False
                If Information.IsArray(m_vOverrideUserDetails) Then


                    nLBound = m_vOverrideUserDetails.GetLowerBound(1)
                    nUBound = m_vOverrideUserDetails.GetUpperBound(1)

                    For nItem As Integer = nLBound To nUBound
                        ' look for a match
                        If m_vOverrideUserDetails(0, nItem) = g_iUserID Then
                            nFoundUser = True
                            Exit For
                        End If
                    Next
                End If

                If nFoundUser = False Then
                    MessageBox.Show("You don't have override access. Please contact System Administrator.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    nResult = gPMConstants.PMEReturnCode.PMError
                Else
                    nResult = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return nResult
    End Function


    ' ***************************************************************** '
    ' Name: ProcessCancel
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessCancel() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessCancel"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
End Class