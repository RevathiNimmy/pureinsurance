Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 02 Aug 2007
    '
    ' Description: Main interface.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' PRIVATE Data Members (Begin)


    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""

    Private m_lMaxColWidth As Array = Array.CreateInstance(GetType(Integer), New Integer() {ACListIDateImported - ACListISheetId + 1}, New Integer() {ACListISheetId})

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUCoverNote.General

    Private m_oBusiness As Object
    ' Stores the return value for a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control

    ' Authority Level
    Private m_lPMAuthorityLevel As Integer

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    Private m_lParty_Cnt As Integer
    Private m_sPartyCode As String = ""
    Private m_sPartyName As String = ""

    Private m_lCoverNoteBookId As Integer

    Private v_sBookNumber As String = ""
    Private v_lStart_Number As Object
    Private v_lEnd_Number As Object
    Private v_dtEffective_Date As Object
    Private v_lSource_Id As Object
    Private v_lCover_Note_Book_Status_Id As Object
    Private v_dtCreated_Date As Object
    Private v_dtLastUpdated As Object

    Private m_aSheets(,) As Object

    ' PUBLIC Property Procedures (Begin)


    Public Property CoverNoteBookId() As Integer
        Get

            Return m_lCoverNoteBookId

        End Get
        Set(ByVal Value As Integer)

            m_lCoverNoteBookId = Value

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



    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
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

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

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

            cboBranch.Items.Clear()
            m_lReturn = GetBranches()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get lookup detail.", gPMConstants.PMELogLevel.PMLogError)
            End If

            cboCoverNoteBookStatus.Items.Clear()
            m_lReturn = CType(GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupCover_Note_Book_Status, ctlLookup:=cboCoverNoteBookStatus), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get lookup detail.", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "InterfaceToData"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}
            v_sBookNumber = txtBookNumber.Text.Trim()

            v_lStart_Number = txtStartNumber.Text.Trim()

            v_lEnd_Number = txtEndNumber.Text.Trim()


            v_dtEffective_Date = cboEffectiveDate.Value
            If cboBranch.SelectedIndex >= 0 Then

                v_lSource_Id = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
            End If
            If cboCoverNoteBookStatus.SelectedIndex >= 0 Then

                v_lCover_Note_Book_Status_Id = VB6.GetItemData(cboCoverNoteBookStatus, cboCoverNoteBookStatus.SelectedIndex)
            Else

                v_lCover_Note_Book_Status_Id = 0
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Dim vInstallation As Object

        Const kMethodName As String = "SetInterfaceDefaults"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get all of the lookup values as related to effective date
            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Display all of the lookup details.
            m_lReturn = CType(DisplayLookupDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = CType(EnableDisableInterface(), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Update the interface details with the
            ' property members.
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                For i As Integer = 0 To cboCoverNoteBookStatus.Items.Count - 1
                    If VB6.GetItemString(cboCoverNoteBookStatus, i).ToUpper().StartsWith("NOT ISSUED") Then
                        cboCoverNoteBookStatus.SelectedIndex = i
                        Exit For
                    End If
                Next
                cboEffectiveDate.Value = DateTime.Now
                cboCreatedDate.Value = DateTime.Now
            Else
                'ToDo
            End If


            uctPickListProducts.AvailableCaption = "Available"

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSheets.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ClearInterface() As Integer
    '
    'Dim result As Integer = 0
    'Dim iMsgResult As DialogResult
    'Dim sMessage, sTitle As String
    '
    'Const kMethodName As String = "ClearInterface"
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Check if the user still wishes to clear
    ' the interface.
    '

    'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '
    ' Display the message.
    'iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
    '
    ' Check message result.
    'If iMsgResult = System.Windows.Forms.DialogResult.No Then
    ' Don't continue with the clear.
    'GoTo Finally_Renamed
    'End If
    '
    ' Clear the interface details.
    '
    ' Clear the search list details.
    'lvwSheets.Items.Clear()
    '
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    'Finally_Renamed: '
    '
    'Return result
    '
    'End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer
        Dim result As Integer = 0
        Dim lRow As Integer
        Dim oListItem As ListViewItem

        Const kMethodName As String = "BusinessToInterface"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_aSheets = Nothing
            lvwSheets.Items.Clear()

            If m_lCoverNoteBookId > 0 Then

                m_lReturn = m_oBusiness.SelectCoverNoteBook(lCoverNoteBookID:=m_lCoverNoteBookId, r_sBookNumber:=v_sBookNumber, r_lStart_Number:=v_lStart_Number, r_lEnd_Number:=v_lEnd_Number, r_dtEffective_Date:=v_dtEffective_Date, r_lAgent_Cnt:=m_lParty_Cnt, r_lAgent_Name:=m_sPartyName, r_lSource_Id:=v_lSource_Id, r_lCover_Note_Book_Status_Id:=v_lCover_Note_Book_Status_Id, r_dtCreated_Date:=v_dtCreated_Date, r_dtLastUpdated:=v_dtLastUpdated)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Unable to select cover note book", gPMConstants.PMELogLevel.PMLogError)
                End If


                If Convert.IsDBNull(v_sBookNumber) Or IsNothing(v_sBookNumber) Then
                    txtBookNumber.Text = ""
                Else
                    txtBookNumber.Text = v_sBookNumber
                End If


                If Convert.IsDBNull(v_lStart_Number) Or IsNothing(v_lStart_Number) Then
                    txtStartNumber.Text = ""
                Else

                    txtStartNumber.Text = CStr(v_lStart_Number)
                End If


                If Convert.IsDBNull(v_lEnd_Number) Or IsNothing(v_lEnd_Number) Then
                    txtEndNumber.Text = ""
                Else

                    txtEndNumber.Text = CStr(v_lEnd_Number)
                End If


                If Convert.IsDBNull(m_sPartyName) Or IsNothing(m_sPartyName) Then
                    m_lParty_Cnt = 0
                    txtAgent.Text = ""
                Else
                    txtAgent.Text = m_sPartyName
                End If

                If gPMFunctions.ToSafeLong(v_lSource_Id) > 0 Then
                    'select from available lookup values
                    For cnt As Integer = 0 To cboBranch.Items.Count - 1

                        If VB6.GetItemData(cboBranch, cnt) = CDbl(v_lSource_Id) Then
                            cboBranch.SelectedIndex = cnt
                            Exit For
                        End If
                    Next cnt
                Else
                    cboBranch.SelectedIndex = -1
                End If

                If gPMFunctions.ToSafeLong(v_lCover_Note_Book_Status_Id) > 0 Then
                    For cnt As Integer = 0 To cboCoverNoteBookStatus.Items.Count - 1

                        If VB6.GetItemData(cboCoverNoteBookStatus, cnt) = CDbl(v_lCover_Note_Book_Status_Id) Then
                            cboCoverNoteBookStatus.SelectedIndex = cnt
                            Exit For
                        End If
                    Next cnt
                Else
                    cboCoverNoteBookStatus.SelectedIndex = -1
                End If

                'Let it handle the nulls


                cboEffectiveDate.Value = CDate(v_dtEffective_Date)


                cboCreatedDate.Value = CDate(v_dtCreated_Date)

                m_lReturn = RefreshSheetData()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Unable to refresh sheet listview", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RefreshSheetData
    ' Description: Updates SheetData list view
    ' ***************************************************************** '
    Private Function RefreshSheetData() As Integer
        Dim nresult As Integer = 0
        Dim oListItem As ListViewItem

        Const kMethodName As String = "RefreshSheetData"
        Try

            nresult = gPMConstants.PMEReturnCode.PMTrue

            m_aSheets = Nothing
            lvwSheets.Items.Clear()

            If m_lCoverNoteBookId > 0 Then

                nresult = m_oBusiness.GetCoverNoteSheets(lCoverNoteBookID:=m_lCoverNoteBookId, r_vResultArray:=m_aSheets)

                If nresult = gPMConstants.PMEReturnCode.PMFalse Then
                    gPMFunctions.RaiseError(kMethodName, "Unable to select cover note sheets", gPMConstants.PMELogLevel.PMLogError)
                End If

                If nresult = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return nresult
                End If


                For lRow As Integer = m_aSheets.GetLowerBound(1) To m_aSheets.GetUpperBound(1)

                    oListItem = lvwSheets.Items.Add(ToSafeString(m_aSheets(ACListISheetId, lRow)).Trim())

                    With oListItem

                        ListViewHelper.GetListViewSubItem(oListItem, ACIColSheetNumber).Text = ToSafeString(m_aSheets(ACListISheetNumber, lRow)).Trim()

                        ListViewHelper.GetListViewSubItem(oListItem, ACIColCustomerName).Text = ToSafeString(m_aSheets(ACListICustomerName, lRow)).Trim()

                        ListViewHelper.GetListViewSubItem(oListItem, ACIColStatus).Text = ToSafeString(m_aSheets(ACListIStatusDescription, lRow)).Trim()

                        ListViewHelper.GetListViewSubItem(oListItem, ACIColPolicyNumber).Text = ToSafeString(m_aSheets(ACListIPolicyNumber, lRow)).Trim()

                        ListViewHelper.GetListViewSubItem(oListItem, ACIColBranch).Text = ToSafeString(m_aSheets(ACListIBranch, lRow)).Trim()

                        ListViewHelper.GetListViewSubItem(oListItem, ACIColAgent).Text = ToSafeString(m_aSheets(ACListIAgent, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, ACIColDateImported).Text = DateTime.Parse(gPMFunctions.ToSafeDate(m_aSheets(ACListIDateImported, lRow))).ToString("d")
                    End With

                    With lvwSheets

                    End With

                    oListItem.Tag = ToSafeString(lRow)

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwSheets.Focus()
                        lvwSheets.FullRowSelect = True
                        lvwSheets.Items.Item(0).Selected = True
                        lvwSheets.Refresh()
                    End If
                Next lRow

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nresult, excep:=ex)

        Finally

        End Try
        Return nresult
    End Function


    Private Function EnableDisableInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EnableDisableInterface"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                    cboEffectiveDate.Enabled = True
                    txtAgent.Enabled = True
                    cboBranch.Enabled = True
                    cboCoverNoteBookStatus.Enabled = True
                    cmdOk.Enabled = True
                    cmdApply.Enabled = True
                    cmdCancel.Enabled = True
                    If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                        txtBookNumber.Enabled = True
                        txtStartNumber.Enabled = True
                        txtEndNumber.Enabled = True
                        cmdAdd.Enabled = False
                        cmdEdit.Enabled = False
                        cmdDelete.Enabled = False
                    Else
                        txtBookNumber.Enabled = False
                        txtStartNumber.Enabled = False
                        txtEndNumber.Enabled = False
                        cmdAdd.Enabled = True
                        cmdEdit.Enabled = True
                        cmdDelete.Enabled = True
                    End If
                Case Else 'PMView
                    txtBookNumber.Enabled = False
                    txtStartNumber.Enabled = False
                    txtEndNumber.Enabled = False
                    cboEffectiveDate.Enabled = False
                    txtAgent.Enabled = False
                    cboBranch.Enabled = False
                    cboCoverNoteBookStatus.Enabled = False
                    cmdOk.Enabled = False
                    cmdApply.Enabled = False
                    cmdCancel.Enabled = True
                    cmdAdd.Enabled = False
                    cmdEdit.Enabled = True
                    cmdDelete.Enabled = False
            End Select



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            tabMainTab.SelectedTab.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraProducts.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProducts, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBookNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBookNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblStartNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStartNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEndNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEndNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCoverNoteBookStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBookStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCreatedDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCreatedDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCoverNoteSheet.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCoverNoteSheets, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOk.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOkButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdApply.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACApplyButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then

                cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACViewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCloseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If



            lvwSheets.Columns.Item(ACIColSheetId).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColSheetId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSheets.Columns.Item(ACIColSheetNumber).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColSheetNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSheets.Columns.Item(ACIColCustomerName).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColCustomerName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSheets.Columns.Item(ACIColStatus).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSheets.Columns.Item(ACIColPolicyNumber).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColPolicyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSheets.Columns.Item(ACIColBranch).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSheets.Columns.Item(ACIColAgent).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSheets.Columns.Item(ACIColDateImported).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColDateImported, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Sub cboBranch_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.SelectedIndexChanged

        If gPMFunctions.ToSafeLong(v_lSource_Id) <> gPMFunctions.ToSafeLong(VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)) Then
            '        txtAgent.Text = ""

            v_lSource_Id = gPMFunctions.ToSafeLong(VB6.GetItemData(cboBranch, cboBranch.SelectedIndex))
        End If

    End Sub

    Private Sub cmdAgentLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgentLookup.Click
        Dim vShortName, vName, vResolvedName As String
        Dim vCnt As Integer
        Const kMethodName As String = "cmdAgentLookup_Click"
        Try






            m_lReturn = CType(SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:=gSIRLibrary.SIRPartyTypeAgent, vResolvedName:=vResolvedName), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                txtAgent.Text = gPMFunctions.ToSafeString(vName)
                m_lParty_Cnt = gPMFunctions.ToSafeLong(vCnt)
                m_sPartyCode = gPMFunctions.ToSafeString(vShortName)
                m_sPartyName = gPMFunctions.ToSafeString(vName)

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
        Const kMethodName As String = "cmdAdd_Click"
        Try

            Dim oInterface As frmCoverNoteSheet

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            oInterface = New frmCoverNoteSheet()
            ' Pass standard details to form properties
            With oInterface
                .bookId = gPMFunctions.ToSafeLong(m_lCoverNoteBookId)
                .LowerSheetRange = gPMFunctions.ToSafeLong(txtStartNumber.Text)
                .UpperSheetRange = gPMFunctions.ToSafeLong(txtEndNumber.Text)
                .SheetId = 0
                .Task = gPMConstants.PMEComponentAction.PMAdd
            End With


            'Developer Guide No. 68
            oInterface.ShowDialog()

            If oInterface.Status = gPMConstants.PMEReturnCode.PMOK Then
                'Refresh list
                m_lReturn = RefreshSheetData()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Unable to refresh sheet listview", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            oInterface.Close()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        Const kMethodName As String = "cmdDelete_Click"
        Try

            Dim iMsgResult As DialogResult
            Dim sMessage As String = ""

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If lvwSheets.Items.Count = 0 Then
                Exit Sub
            ElseIf lvwSheets.FocusedItem Is Nothing Then
                MessageBox.Show("Please select a sheet to delete.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
                'Developer Guide No. 52
            ElseIf lvwSheets.FocusedItem.SubItems(MainModule.ACIColStatus).Text.Trim().ToUpper() = "ISSUED" Then
                MessageBox.Show("You can't delete an ISSUED sheet.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            sMessage = "Are you sure you want to delete this" & Strings.Chr(13) & Strings.Chr(10) & _
                       "Sheet from the Cover Note Book?"

            iMsgResult = MessageBox.Show(sMessage, "Cover Note Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If


            m_lReturn = m_oBusiness.DeleteCoverNoteSheet(lSheet_Id:=CInt(lvwSheets.FocusedItem.Text))


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Delete Cover Note Book", gPMConstants.PMELogLevel.PMLogError)
            End If

            RefreshSheetData()


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

            m_lReturn = applyChanges()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log should have been prepared till now
                Exit Sub
            End If

            Me.Hide()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        Const kMethodName As String = "cmdApply_Click"
        Try



            m_lReturn = applyChanges()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log should have been prepared till now
                Exit Sub
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

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

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Const kMethodName As String = "Form_Initialize"
        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUCoverNote.General()

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

                gPMFunctions.RaiseError(kMethodName, sMessage & Strings.Chr(13) & Strings.Chr(10) & "bSIRCoverNote.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialize", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim key As uctPickList.PickListKey

        Const kMethodName As String = "Form_Load"
        Try


            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to set interface defaults", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get interface details", gPMConstants.PMELogLevel.PMLogError)
            End If

            key = New uctPickList.PickListKey()
            key.KeyName = "cover_note_book_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListProducts.ForeignKeys.Add(key, Key:="cover_note_book_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "product_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListProducts.ForeignKeys.Add(key, Key:="product_id")


            uctPickListProducts.ForeignKeys.Item("cover_note_book_id").value = m_lCoverNoteBookId
            'Developer Guide No. 68
            m_lReturn = uctPickListProducts.Load_Renamed()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.
        Const kMethodName As String = "Form_QueryUnload"
        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally




        End Try
    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000


        Const ACCtrlMask As Integer = 2

        ' Set the control key value.
        Dim iCtrlDown As Integer = (Shift And ACCtrlMask) > 0

        Exit Sub

    End Sub

    Private Sub frmInterface_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000


        Const ACShiftMask As Integer = 1
        Const ACCtrlMask As Integer = 2

        ' Set the control key value.
        Dim iShiftDown As Integer = (Shift And ACShiftMask) > 0
        Dim iCtrlDown As Integer = (Shift And ACCtrlMask) > 0

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim oInterface As frmCoverNoteSheet

        ' Click event of the Edit Button.
        Const kMethodName As String = "cmdEdit_Click"
        Try


            ' {* USER DEFINED CODE (Begin) *}

            'Check before calling edit that any item is selecetd or not
            If lvwSheets.FocusedItem Is Nothing Then
                Exit Sub
            End If

            '    m_lReturn& = DataToProperties()

            oInterface = New frmCoverNoteSheet()
            ' Pass standard details to form properties
            With oInterface
                .bookId = gPMFunctions.ToSafeLong(m_lCoverNoteBookId)
                .SheetId = gPMFunctions.ToSafeLong(lvwSheets.FocusedItem.Text)
                'Developer Guide No. 52
                .SheetNumber = gPMFunctions.ToSafeLong(lvwSheets.FocusedItem.SubItems(MainModule.ACIColSheetNumber).Text)
                .LowerSheetRange = gPMFunctions.ToSafeLong(txtStartNumber.Text)
                .UpperSheetRange = gPMFunctions.ToSafeLong(txtEndNumber.Text)
                .Task = gPMConstants.PMEComponentAction.PMEdit
            End With


            'Developer Guide No. 68
            oInterface.ShowDialog()

            If oInterface.Status = gPMConstants.PMEReturnCode.PMOK Then
                'Refresh list
                m_lReturn = RefreshSheetData()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Unable to refresh sheet listview", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            oInterface.Close()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

            ' Set the interface object to nothing.
            oInterface = Nothing



        End Try
        Exit Sub
    End Sub

    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As String = "") As Integer

        Dim result As Integer = 0
        'Developer Guide No. 108
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Const kMethodName As String = "SelectParty"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No. 108
            oFindParty = New iPMBFindParty.Interface_Renamed()


            oFindParty.BranchID = g_iSourceID

            m_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialize find pary.", gPMConstants.PMELogLevel.PMLogError)
            End If

            oFindParty.CallingAppName = ACApp

            m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="CoverNote", vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to set process mode.", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set appropriate key


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lErrorNumber = oFindParty.SetKeys(vKeyArray)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to set keys.", gPMConstants.PMELogLevel.PMLogError)
                End If
                oFindParty.NotEditable = 1
                oFindParty.SuppressSubAgents = True
            End If


            m_lErrorNumber = oFindParty.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start find party.", gPMConstants.PMELogLevel.PMLogError)
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName

                If Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                End If
                vResolvedName = oFindParty.ResolvedName
            Else
                result = oFindParty.Status
            End If

            oFindParty.Dispose()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            oFindParty.Dispose()
            oFindParty = Nothing



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
            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLBookType) = gSIRLibrary.SIRLookupCover_Note_Book_Status

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
                gPMFunctions.RaiseError(kMethodName, "Failed to get lookup details", gPMConstants.PMELogLevel.PMLogError)
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


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                If txtBookNumber.Text.Trim().Length = 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Please enter a valid book number.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtBookNumber.Focus()
                    Return result
                Else
                    txtBookNumber.Text = txtBookNumber.Text.Trim()
                    txtStartNumber.Text = txtStartNumber.Text.Trim()
                    txtEndNumber.Text = txtEndNumber.Text.Trim()
                End If

                Dim dbNumericTemp2 As Double
                Dim dbNumericTemp As Double
                If Not Double.TryParse(txtStartNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Or gPMFunctions.ToSafeLong(txtStartNumber.Text) <= 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Please enter a valid start number.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtStartNumber.Focus()
                    Return result
                ElseIf Not Double.TryParse(txtEndNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Or gPMFunctions.ToSafeLong(txtEndNumber.Text) <= 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Please enter a valid end number.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtEndNumber.Focus()
                    Return result
                ElseIf gPMFunctions.ToSafeLong(txtStartNumber.Text) > gPMFunctions.ToSafeLong(txtEndNumber.Text) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Please enter a valid combination of start number and end number.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtStartNumber.Focus()
                    Return result
                Else
                    txtStartNumber.Text = CStr(gPMFunctions.ToSafeLong(txtStartNumber.Text))
                    txtEndNumber.Text = CStr(gPMFunctions.ToSafeLong(txtEndNumber.Text))
                End If
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                If cboCoverNoteBookStatus.SelectedIndex = -1 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Please select a valid Book status.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboCoverNoteBookStatus.Focus()
                    Return result
                End If

                If txtAgent.Text.Trim() <> "" Or cboBranch.SelectedIndex <> -1 Or uctPickListProducts.SelectedItems > 0 Then
                    If txtAgent.Text.Trim() = "" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Please select an agent to assign to a book.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If txtAgent.Enabled Then
                            txtAgent.Focus()
                        End If
                        Return result
                    ElseIf (cboBranch.SelectedIndex = -1) Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Please select a branch to assign to a book.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cboBranch.Enabled Then
                            cboBranch.Focus()
                        End If
                        Return result
                    ElseIf (uctPickListProducts.SelectedItems <= 0) Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Please select at least one product to assign to a book.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        uctPickListProducts.Focus()
                        Return result
                    End If
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
        Dim vArray(,) As Object
        Try


            Dim key As uctPickList.PickListKey
            Dim iMsgResult As DialogResult
            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                If ValidateForm() <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                Else
                    If InterfaceToData() = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lCoverNoteBookId = 0


                        m_lReturn = m_oBusiness.FindCoverNoteBook(r_vResultArray:=vArray, sBookNumber:=v_sBookNumber, lStart_Number:=DBNull.Value, lEnd_Number:=DBNull.Value, lAgent_Cnt:=DBNull.Value, dtLast_Updated:=DBNull.Value, lSource_Id:=DBNull.Value, lCover_Note_Book_Status_Id:=DBNull.Value, sPolicy_ref:=DBNull.Value, dtAssigned_Date:=DBNull.Value, iUserId:=DBNull.Value)

                        If Information.IsArray(vArray) Then
                            MessageBox.Show("Book Number already exists." & Strings.Chr(13) & Strings.Chr(10) & _
                                            "Please enter a valid book number.", "Cover Note Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            result = gPMConstants.PMEReturnCode.PMFalse
                            Return result
                        End If

                        sMessage = "Creating this Cover Note Book will " & Strings.Chr(13) & Strings.Chr(10) & "generate " & _
                                   (gPMFunctions.ToSafeLong(v_lEnd_Number) - gPMFunctions.ToSafeLong(v_lStart_Number)) + 1 & _
                                   " Sheet Records." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Are you sure you want to continue?"

                        iMsgResult = MessageBox.Show(sMessage, "Cover Note Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning don't apply.
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Return result
                        End If

                        'It will come out with new cover note id

                        m_lReturn = m_oBusiness.AddCoverNoteBook(r_lCoverNoteBookID:=m_lCoverNoteBookId, sBookNumber:=v_sBookNumber, lStart_Number:=v_lStart_Number, lEnd_Number:=v_lEnd_Number, dtEffective_Date:=v_dtEffective_Date, lAgent_Cnt:=m_lParty_Cnt, lSource_Id:=v_lSource_Id, lCover_Note_Book_Status_Id:=v_lCover_Note_Book_Status_Id)




                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to Add Cover Note Book", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        '                If m_lCoverNoteBookId < 0 Then
                        '                    MsgBox "Book Number already exists." & vbCrLf & _
                        ''                        "Please enter a valid book number.", vbCritical, "Cover Note Maintenance"
                        '
                        '                    applyChanges = PMFalse
                        '                    GoTo Finally
                        '                End If


                        uctPickListProducts.ForeignKeys.Item("cover_note_book_id").value = m_lCoverNoteBookId

                        m_lReturn = uctPickListProducts.Save()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to Add Cover Note Book", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'If successfully added then switch the form into Edit mode
                        m_iTask = gPMConstants.PMEComponentAction.PMEdit
                        EnableDisableInterface()
                        RefreshSheetData()
                    End If
                End If
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                If ValidateForm() <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                Else
                    'Just for re-assignment of Book
                    If InterfaceToData() = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = m_oBusiness.EditCoverNoteBook(lCoverNoteBookID:=m_lCoverNoteBookId, dtEffective_Date:=v_dtEffective_Date, lAgent_Cnt:=m_lParty_Cnt, lSource_Id:=gPMFunctions.ToSafeLong(v_lSource_Id), lCover_Note_Book_Status_Id:=v_lCover_Note_Book_Status_Id)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to Update Cover Note Book", gPMConstants.PMELogLevel.PMLogError)
                        End If


                        uctPickListProducts.ForeignKeys.Item("cover_note_book_id").value = m_lCoverNoteBookId
                        m_lReturn = uctPickListProducts.Save()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to Update Cover Note Book", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    'Private Sub lvwSheets_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSheets.DoubleClick
    '    'If cmdEdit.Enabled Then
    '    '    cmdEdit_Click(cmdEdit, New EventArgs())
    '    'End If
    'End Sub

    Private Function GetBranches() As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0

        Dim vBranches(,) As Object

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Const kMethodName As String = "GetBranches"
        Try
            Catch_Renamed = True


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the branch values.

            m_lReturn = m_oBusiness.GetBranches(iUserId:=g_oObjectManager.UserID, r_vResult:=vBranches)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get Branches.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Loop through branches
            If Information.IsArray(vBranches) Then

                For lCount As Integer = vBranches.GetLowerBound(1) To vBranches.GetUpperBound(1)
                    ' Add the details to the control
                    Dim cboBranch_NewIndex As Integer = -1

                    cboBranch_NewIndex = cboBranch.Items.Add(CStr(vBranches(ACDetailDesc, lCount)))

                    VB6.SetItemData(cboBranch, cboBranch_NewIndex, CInt(vBranches(ACDetailKey, lCount)))
                Next lCount
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed
            If Catch_Renamed Then


                ' DO Not Call any functions before here or the error will be lost
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            End If
Finally_Renamed:
        End Try
    End Function

    Private Sub lvwSheets_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        lvwSheets.GetItemAt(e.Location.X, e.Location.Y).Selected = True

        If cmdEdit.Enabled Then
            cmdEdit_Click(cmdEdit, New EventArgs())
        End If
    End Sub

End Class
