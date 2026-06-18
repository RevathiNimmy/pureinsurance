Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmCoverNoteSheet
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmCoverNoteSheet
    '
    ' Date: 02 Aug 2007
    '
    ' Description: Main interface.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmCoverNoteSheet"

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Stores the return value for a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Authority Level
    Private m_lPMAuthorityLevel As Integer
    Private m_oBusiness As Object

    Private m_lBookId As Integer

    Private m_lSheetId As Integer
    Private m_lSheetNumber As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sPolicyRef As String = ""
    Private m_dtAssignedDate As Date
    Private m_lSheetStatusId As Integer
    Private m_sComments As String = ""
    Private m_lLowerLimit As Integer
    Private m_lUpperLimit As Integer

    ' PUBLIC Property Procedures (Begin)

    Public Property bookId() As Integer
        Get
            Return m_lBookId
        End Get
        Set(ByVal Value As Integer)
            m_lBookId = Value
        End Set
    End Property


    Public Property SheetId() As Integer
        Get
            Return m_lSheetId
        End Get
        Set(ByVal Value As Integer)
            m_lSheetId = Value
        End Set
    End Property


    Public Property Insurance_File_Cnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property


    Public Property SheetNumber() As Integer
        Get
            Return m_lSheetNumber
        End Get
        Set(ByVal Value As Integer)
            m_lSheetNumber = Value
        End Set
    End Property


    Public Property PolicyRef() As String
        Get
            Return m_sPolicyRef
        End Get
        Set(ByVal Value As String)
            m_sPolicyRef = Value
        End Set
    End Property


    Public Property SheetStatusId() As Integer
        Get
            Return m_lSheetStatusId
        End Get
        Set(ByVal Value As Integer)
            m_lSheetStatusId = Value
        End Set
    End Property


    Public Property Comments() As String
        Get
            Return m_sComments
        End Get
        Set(ByVal Value As String)
            m_sComments = Value
        End Set
    End Property


    Public Property AssignedDate() As Date
        Get
            Return m_dtAssignedDate
        End Get
        Set(ByVal Value As Date)
            m_dtAssignedDate = Value
        End Set
    End Property

    Public WriteOnly Property LowerSheetRange() As Integer
        Set(ByVal Value As Integer)
            m_lLowerLimit = Value
        End Set
    End Property

    Public WriteOnly Property UpperSheetRange() As Integer
        Set(ByVal Value As Integer)
            m_lUpperLimit = Value
        End Set
    End Property


    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property


    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    ' Set the interface exit status.
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property



    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisplayLookupDetails"
        Try


        result = gPMConstants.PMEReturnCode.PMTrue

        cboSheetStatus.Items.Clear()
        m_lReturn = CType(GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupCover_Note_Sheet_Status, ctlLookup:=cboSheetStatus), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to get lookup details.", gPMConstants.PMELogLevel.PMLogError)
        End If

        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        

        End Try
	Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "GetLookupValues"
        Try


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Gets all of the lookup values.


        ReDim m_vLookupValues(3, ACLMax)

        ' Setup Lookup Table Names
        m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLSourceType) = gSIRLibrary.SIRLookupSource
        m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLBookType) = gSIRLibrary.SIRLookupCover_Note_Sheet_Status

        ' Do not supply a key
        For i As Integer = 0 To ACLMax
            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, i) = ""
        Next i

        ' Get all of the lookup values with the correct
        ' effective date.

        m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to get lookup values.", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Const kMethodName As String = "GetLookupDetails"
        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the lookup values.

        bFoundMatch = False

        For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
            ' Check for a match of the table name.
            If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                ' Found a match
                bFoundMatch = True
                Exit For
            End If
        Next lRow

        ' Check if there has been a table match.
        If Not bFoundMatch Then
            gPMFunctions.RaiseError(kMethodName, "Failed to get lookup details.", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Using the lookup values, populate the control with
        ' the details from the lookup details array.

        For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
            ' Add the details to the control.


            'Developer Guide No. 29
            Dim newIndex As Integer = CType(ctlLookup, ComboBox).Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))




            ' Check if this is the selected index.
            If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                End If
            End If

        Next lCntr

        ' Check if the selected index is blank. If so set the controls index to zero.
        If CStr(m_vLookupValues(ACValueNumber, lRow)) = "" Then

        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Const kMethodName As String = "cmdCancel_Click"
        Try


        ' Set the interface status.
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Hide()


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        Const kMethodName As String = "cmdOk_Click"
        Try


        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        If ValidateForm() <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        If cboSheetStatus.SelectedIndex = -1 Then
            MessageBox.Show("Please select a valid sheet status.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        m_lReturn = applyChanges()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Log should have been prepared till now
            Exit Sub
        End If

        Me.Hide()


        Catch ex As Exception

        m_lStatus = gPMConstants.PMEReturnCode.PMError
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub Form_Initialize_Renamed()
        Dim sMessage, sTitle As String

        Const kMethodName As String = "Form_Initialize"
        Try


            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRCoverNote.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                gPMFunctions.RaiseError(kMethodName, sMessage & Strings.Chr(13) & Strings.Chr(10) & "bSIRCoverNote.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)
        End Try




    End Sub


    Private Sub frmCoverNoteSheet_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Const kMethodName As String = "Form_Load"
        Try


        m_lReturn = SetInterfaceDefaults()
        m_lReturn = BusinessToInterface()


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

        End Try
        Exit Sub
    End Sub

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim i As Integer
        Dim vInstallation As Object

        Const kMethodName As String = "SetInterfaceDefaults"
        Try


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get all of the lookup values as related to effective date
        m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to get lookup details.", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Display all of the lookup details.
        m_lReturn = CType(DisplayLookupDetails(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to display lookup details.", gPMConstants.PMELogLevel.PMLogError)
        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Private Function BusinessToInterface() As Integer
        Dim result As Integer = 0
        Dim lRow As Integer
        Dim m_vSheet As Object

        Const kMethodName As String = "BusinessToInterface"
        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lSheetId > 0 And m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            'Fetch corresponding sheets for SheetId

            m_lReturn = m_oBusiness.SelectCoverNoteSheet(lCoverNoteBookID:=m_lBookId, lCoverNoteSheetNumber:=m_lSheetNumber, r_vResultArray:=m_vSheet)

            'There should be at least one sheet attached to a book
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to select cover note sheet", gPMConstants.PMELogLevel.PMLogError)
            End If

            txtSheetNumber.Text = CStr(gPMFunctions.ToSafeLong(m_vSheet(ACISheetNumber, 0)))
            m_lInsuranceFileCnt = gPMFunctions.ToSafeLong(m_vSheet(ACIPolicyId, 0))
            txtPolicyNumber.Text = gPMFunctions.ToSafeString(m_vSheet(ACIPolicyRef, 0))


            If CStr(m_vSheet(ACIAssignedDate, 0)).Trim() = "" Then
                'Move to current date
                cboAssignedDate.Value = DateTime.Now

            Else

                cboAssignedDate.Value = gPMFunctions.ToSafeDate(m_vSheet(ACIAssignedDate, 0))
            End If
            txtComments.Text = gPMFunctions.ToSafeString(m_vSheet(ACIComments, 0))

            m_lSheetStatusId = gPMFunctions.ToSafeLong(m_vSheet(ACIStatusId, 0))

            If m_lSheetStatusId > 0 Then
                For cnt As Integer = 0 To cboSheetStatus.Items.Count - 1
                    If VB6.GetItemData(cboSheetStatus, cnt) = m_lSheetStatusId Then
                        cboSheetStatus.SelectedIndex = cnt
                        If gPMFunctions.ToSafeString(m_vSheet(ACIStatusCode, 0)).ToUpper() = "ISSUED" Then
                            cboSheetStatus.Enabled = False
                            txtSheetNumber.Enabled = True
                        Else
                            cboSheetStatus.Enabled = True
                            txtSheetNumber.Enabled = False
                        End If
                        Exit For
                    End If
                Next cnt
            Else
                cboSheetStatus.SelectedIndex = -1
            End If
        ElseIf m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            cboSheetStatus.Enabled = True
            For cnt As Integer = 0 To cboSheetStatus.Items.Count
                If VB6.GetItemString(cboSheetStatus, cnt).ToUpper().StartsWith("NOT ISSUED") Then
                    cboSheetStatus.Enabled = False
                    cboSheetStatus.SelectedIndex = cnt
                    Exit For
                End If
            Next cnt
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ValidateForm
    ' Description:
    ' ***************************************************************** '
    Private Function ValidateForm() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ValidateForm"
        Try

        Dim m_vSheet As Object

        result = gPMConstants.PMEReturnCode.PMTrue
        If gPMFunctions.ToSafeLong(txtSheetNumber.Text) <= 0 Then
            result = gPMConstants.PMEReturnCode.PMFalse
            MessageBox.Show("Please enter a valid sheet number.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
        ElseIf txtPolicyNumber.Text.Trim() = "" Then
            If cboSheetStatus.Text.ToUpper() = ACSTATUSISSUED Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Sheets cannot be issued from Cover Note Maintenance", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
            ElseIf CDbl(txtSheetNumber.Text) > m_lUpperLimit Or CDbl(txtSheetNumber.Text) < m_lLowerLimit Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Sheet number entered is out of range", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
            End If
        ElseIf CDbl(txtSheetNumber.Text) > m_lUpperLimit Or CDbl(txtSheetNumber.Text) < m_lLowerLimit Then
            result = gPMConstants.PMEReturnCode.PMFalse
            MessageBox.Show("Sheet number entered is out of range", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
        ElseIf m_lSheetNumber <> gPMFunctions.ToSafeLong(txtSheetNumber.Text) Then
            'Check if new sheet number exist and valid for issuance

            m_lReturn = m_oBusiness.SelectCoverNoteSheet(lCoverNoteBookID:=m_lBookId, lCoverNoteSheetNumber:=gPMFunctions.ToSafeLong(txtSheetNumber.Text), r_vResultArray:=m_vSheet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SelectCoverNoteSheet Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If Not Information.IsArray(m_vSheet) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Cover Note Number entered is not a valid number." & Strings.Chr(13) & Strings.Chr(10) & _
                                "Please enter a valid Cover Note Number.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
            ElseIf CStr(m_vSheet(ACIStatusCode, 0)).ToUpper() <> ACSTATUSNOTISSUED Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Cover Note Number status should be NOT ISSUED." & Strings.Chr(13) & Strings.Chr(10) & _
                                "Please enter a valid Cover Note Number.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
            End If

        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Function applyChanges() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "applyChanges"
        Try


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

            m_lReturn = m_oBusiness.AddCoverNoteSheet(r_lCoverNoteSheetID:=m_lSheetId, lBook_Id:=m_lBookId, lSheet_Number:=gPMFunctions.ToSafeLong(txtSheetNumber.Text), sComments:=gPMFunctions.ToSafeString(txtComments.Text))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or m_lSheetId = 0 Then
                gPMFunctions.RaiseError(kMethodName, "Unable to Add cover note sheet", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_lSheetId = -1 Then
                MessageBox.Show("Sheet Number already exists.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFail
                    Return result
            End If

        ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

            m_lReturn = m_oBusiness.EditCoverNoteSheet(lCoverNoteBookID:=m_lBookId, lNewCoverNoteSheetNumber:=gPMFunctions.ToSafeLong(txtSheetNumber.Text), lOldCoverNoteSheetNumber:=m_lSheetNumber, lCoverNoteSheetStatusID:=VB6.GetItemData(cboSheetStatus, cboSheetStatus.SelectedIndex), lInsurance_file_cnt:=m_lInsuranceFileCnt, dtAssignedDate:=cboAssignedDate.Value, sComments:=txtComments.Text)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to update cover note sheet", gPMConstants.PMELogLevel.PMLogError)
            End If

        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function
End Class
