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
    ' Date: 12 Aug 2007
    '
    ' Description: Main interface.
    ' ***************************************************************** '


    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUFindCoverNote.General

    'Developer Guide No. 108
    Private m_oCoverNote As ipmucovernote.Interface_Renamed

    ' Stores the return value for a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object

    ' Authority Level
    Private m_lPMAuthorityLevel As Integer

    Private m_lParty_Cnt As Integer
    Private m_sPartyCode As String = ""
    Private m_sPartyName As String = ""

    Private v_sBookId As String = ""
    Private v_sBookNumber As String = ""
    Private v_sPolicyNumber As String = ""
    Private v_lStart_Number As String = ""
    Private v_lEnd_Number As String = ""
    Private v_lAgent_Cnt As Object
    Private v_lSource_Id As String = ""
    Private v_dtLast_Updated As String = ""
    Private v_dtAssigned_Date As String = ""
    Private v_lCover_Note_Book_Status_Id As String = ""

    ' Public instance of the business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.2.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean


    Public Property DisableWildcardSearchOption() As Boolean
        Get
            Return m_bDisableWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableWildcardSearchOption = Value
        End Set
    End Property


    Public Property EnablePartialWildcardSearchOption() As Boolean
        Get
            Return m_bEnablePartialWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bEnablePartialWildcardSearchOption = Value
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.2.1)
    ' PUBLIC Property Procedures (Begin)

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

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

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

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property


    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display a searching message.
            DisplayStatusSearching()

            If InterfaceToData() = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = m_oBusiness.FindCoverNoteBook(r_vResultArray:=m_vSearchData, sBookNumber:=v_sBookNumber, lStart_Number:=v_lStart_Number, lEnd_Number:=v_lEnd_Number, lAgent_Cnt:=v_lAgent_Cnt, dtLast_Updated:=v_dtLast_Updated, lSource_Id:=v_lSource_Id, lCover_Note_Book_Status_Id:=v_lCover_Note_Book_Status_Id, sPolicy_ref:=v_sPolicyNumber, dtAssigned_Date:=v_dtAssigned_Date, iUserId:=g_iUserID)



                ' Check the return values.
                Select Case (m_lReturn)
                    Case gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMNotFound

                    Case Else
                        gPMFunctions.RaiseError(kMethodName, "Failed to get search details", gPMConstants.PMELogLevel.PMLogError)
                End Select

            End If

            'Clear/Assign Values to Interface
            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DataToInterface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ListView6Autosize(lvwSearchResults, True)
            ' hide first col
            lvwSearchResults.Columns.Item(0).Width = CInt(0)

            ' Display the number of item found message.
            DisplayStatusFound()

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim sLookupDesc As String = ""
        Dim vLedgerName As Object
        Dim iCol, lTextWidth As Integer
        Dim v_vValue As Object
        Dim bMultiTreeAccounting As Boolean

        Const ACFindImage As String = "FindImage"

        Const kMethodName As String = "DataToInterface"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchResults.Items.Clear()

            ' Check that search details are valid before continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Dont update the list view till we've populated it.
            'Developer Guide No. 178
            m_lReturn = CType(ListViewFunc.ListViewBatchStart(lvwSearchResults), gPMConstants.PMEReturnCode)

            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Column 1 Book Id hidden
                oListItem = lvwSearchResults.Items.Add(CStr(m_vSearchData(ACIBookId, lRow)).Trim()) ', , ACFindImage)

                ' Assign details to other the columns
                With oListItem
                    ListViewHelper.GetListViewSubItem(oListItem, ACIColBookNumber).Text = CStr(m_vSearchData(ACIBookNumber, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, ACIColStartNumber).Text = CStr(m_vSearchData(ACIStartNumber, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, ACIColEndNumber).Text = CStr(m_vSearchData(ACIEndNumber, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, ACIColAgentName).Text = CStr(m_vSearchData(ACIAgentName, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, ACIColStatus).Text = CStr(m_vSearchData(ACIStatusDescription, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, ACIColBranch).Text = CStr(m_vSearchData(ACIBranchName, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, ACIColDateUpdated).Text = DateTime.Parse(CStr(m_vSearchData(ACIDateUpdated, lRow)).Trim()).ToString("d")
                    ListViewHelper.GetListViewSubItem(oListItem, ACIColCreatedDate).Text = DateTime.Parse(CStr(m_vSearchData(ACICreatedDate, lRow)).Trim()).ToString("d")
                End With

                ' Set the tag property with the index of
                ' the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                'If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                '    ' Select the first item.
                '    lvwSearchResults.Items.Item(0).Selected = True

                '    ' Refresh the initial results.
                '    lvwSearchResults.Refresh()
                'End If

            Next lRow
            lvwSearchResults.Focus()
            lvwSearchResults.FullRowSelect = True
            lvwSearchResults.Items(0).Selected = True
            'Developer Guide No. 178
            m_lReturn = CType(ListViewFunc.ListViewBatchEnd(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If lvwSearchResults.Items.Count > 0 Then
                ' Select the first item.
                lvwSearchResults.FocusedItem = lvwSearchResults.Items.Item(0)
                cmdEdit.Enabled = True
            Else
                cmdEdit.Enabled = True
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Const kMethodName As String = "DataToProperties"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(lvwSearchResults.FocusedItem.Index).Tag)

            ' Update the property members.
            v_sBookId = CStr(gPMFunctions.ToSafeLong(m_vSearchData(ACIBookId, lSelectedItem)))
            v_sBookNumber = CStr(m_vSearchData(ACIBookNumber, lSelectedItem)).Trim()
            v_lStart_Number = CStr(m_vSearchData(ACIStartNumber, lSelectedItem)).Trim()
            v_lEnd_Number = CStr(m_vSearchData(ACIEndNumber, lSelectedItem)).Trim()
            v_lAgent_Cnt = CStr(m_vSearchData(ACIAgentId, lSelectedItem)).Trim()
            v_lCover_Note_Book_Status_Id = CStr(m_vSearchData(ACIStatusId, lSelectedItem)).Trim()
            v_lSource_Id = CStr(m_vSearchData(ACIBranchId, lSelectedItem)).Trim()
            v_dtLast_Updated = CStr(m_vSearchData(ACIDateUpdated, lSelectedItem)).Trim()
            v_dtAssigned_Date = CStr(m_vSearchData(ACICreatedDate, lSelectedItem)).Trim()

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

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

            ' Get all of the lookup details.

            cboBranch.Items.Clear()
            m_lReturn = GetBranches()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboCoverNoteBookStatus.Items.Clear()
            m_lReturn = CType(GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupCover_Note_Book_Status, ctlLookup:=cboCoverNoteBookStatus), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.

            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup values as related to effective date
            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display all of the lookup details.
            m_lReturn = CType(DisplayLookupDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set the default date
            cboAssignedDate.Value = DateTime.Today
            cboLastUpdate.Value = DateTime.Today
            cboLastUpdate.Checked = False
            cboAssignedDate.Checked = False
            'Reset to null

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchResults.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            'Always false until we find a cover note
            cmdEdit.Enabled = False

            'TODO LIST
            cmdNew.Enabled = Not (m_iTask = gPMConstants.PMEComponentAction.PMView)
            cmdNew.Enabled = True

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
    Private Function ClearInterface(ByRef bSilent As Boolean) As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Const kMethodName As String = "ClearInterface"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.
            If Not bSilent Then

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display the message.
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return result
                End If
            End If

            ' Clear the interface details.
            txtBookNumber.Text = ""
            txtStartNumber.Text = ""
            txtEndNumber.Text = ""
            txtAgent.Text = ""
            m_lParty_Cnt = 0
            m_sPartyCode = ""
            m_sPartyName = ""

            cboBranch.SelectedIndex = -1
            txtPolicyNumber.Text = ""
            cboCoverNoteBookStatus.SelectedIndex = -1

            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchResults.Items.Clear()

            ' Clear the search status bar.
            'stbStatus.Text = ""
            _stbStatus_Panel1.Text = ""
            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)


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
                gPMFunctions.RaiseError(kMethodName, "Failed to load data from Resource file", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Developer Guide No. 76
            fraMain.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFrameTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            lblBookNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBookNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            lblStartNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStartNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            lblEndNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEndNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            lblAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            lblLastUpdate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLastUpdate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            lblPolicyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            lblCoverNoteBookStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCoverNoteStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            lblAssignedDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAssignedDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 76
            cmdClose.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCloseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 76
            cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'TODO LIST
            'If m_iTask = gPMConstants.PMEComponentAction.PMView Then

            '          cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACViewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Else

            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'End If



            'Developer Guide No. 76
            lvwSearchResults.Columns.Item(ACListIBookId).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColBookId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 76
            lvwSearchResults.Columns.Item(ACListIBookNumber).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColBookNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 76
            lvwSearchResults.Columns.Item(ACListIStartNumber).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColStartNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 76
            lvwSearchResults.Columns.Item(ACListIEndNumber).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColEndNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 76
            lvwSearchResults.Columns.Item(ACListIAgent).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 76
            lvwSearchResults.Columns.Item(ACListIStatus).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 76
            lvwSearchResults.Columns.Item(ACListIBranch).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 76
            lvwSearchResults.Columns.Item(ACListIDateUpdated).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColDateUpdated, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            m_lReturn = iPMForms.DisplayCaptions(Me, My.Resources.ResourceManager)


            'Developer Guide No. 76
            lvwSearchResults.Columns.Item(ACListICreatedDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColCreatedDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Const kMethodName As String = "DisplayStatusSearching"
        Try


            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            stbStatus.Text = " " & sMessage


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""
        Dim lItemsFound As Integer

        Const kMethodName As String = "DisplayStatusFound"
        Try


            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            lItemsFound = lvwSearchResults.Items.Count

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'stbStatus.Text = " " & lItemsFound & " " & sMessage
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ResizeInterface"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            fraMain.Width = Me.Width - VB6.TwipsToPixelsX(1720)

            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1500)
            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1500)

            lvwSearchResults.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwSearchResults.Height = Me.Height - VB6.TwipsToPixelsY(4000)

            cmdNew.Top = Me.Height - VB6.TwipsToPixelsY(1100)
            cmdEdit.Top = Me.Height - VB6.TwipsToPixelsY(1100)
            cmdClose.Top = Me.Height - VB6.TwipsToPixelsY(1100)
            cmdClose.Left = Me.Width - VB6.TwipsToPixelsX(1500)

            Me.Refresh()


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: FindNow
    '
    ' Description: Get the interface details from the query
    '
    ' ***************************************************************** '
    Private Sub FindNow()
        Const kMethodName As String = "FindNow"
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.2.2)
        Dim sWildcardErrorMessage As String = ""
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.2.2)
        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.2.2)
            'Check wildcard searches

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtBookNumber.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find CoverNote")
                txtBookNumber.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtStartNumber.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find CoverNote")
                txtStartNumber.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtEndNumber.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find CoverNote")
                txtEndNumber.Focus()
                Exit Sub

            End If

            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.2.2)


            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get interface details", gPMConstants.PMELogLevel.PMLogError)
            End If

            If lvwSearchResults.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                cmdEdit.Enabled = True

            Else
                VB6.SetDefault(cmdFindNow, True)
                cmdEdit.Enabled = False
            End If

            ' Set the focus.
            lvwSearchResults.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub
    ' PRIVATE Methods (End)


    Private Sub cmdAgentLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgentLookup.Click
        Dim vCnt As Integer
        Dim vShortName, vName, vResolvedName As String
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
        End Try




    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.
        Dim sMessage, sTitle As String


        Const kMethodName As String = "Form_Initialize"
        Try


            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRCoverNote.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                gPMFunctions.RaiseError(kMethodName, sMessage & Strings.Chr(13) & Strings.Chr(10) & "bSIRCoverNote.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUFindCoverNote.General()

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

        Const kMethodName As String = "Form_Load"
        Try

            iPMFunc.ShowFormInTaskBar_Detach()

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
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                gPMFunctions.RaiseError(kMethodName, "Failed to set interface defaults", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Gets the interface details to be displayed.
            'm_lReturn& = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.RaiseError(kMethodName, "Failed to get interface details", gPMConstants.PMELogLevel.PMLogError)
            End If

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
                    eventArgs.cancel = True
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

            ' Check if we have an instance of the business object.
            If Not (m_oBusiness Is Nothing) Then
                ' Terminate the business object

                m_oBusiness.Dispose()
                ' Destroy the instance of the business object
                ' from memory.
                m_oBusiness = Nothing
            End If

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

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        Const kMethodName As String = "Form_Resize"
        Try


            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        Const kMethodName As String = "cmdClose_Click"
        Try


            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        ' Click event of the Find Now button.
        FindNow()

    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        Const kMethodName As String = "cmdNewSearch_Click"
        Try


            ' Clear the interface details.
            m_lReturn = CType(ClearInterface(bSilent:=False), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

        Const kMethodName As String = "cmdNew_Click"
        Try


            ' Set the interface status.
            m_lStatus = gPMConstants.PMEComponentAction.PMAdd

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Create cover note object if not already done so
            If m_oCoverNote Is Nothing Then
                ' Get an instance of the cover note interface object via the public object manager.
                Dim temp_m_oCoverNote As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oCoverNote, sClassName:="iPMUCoverNote.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oCoverNote = temp_m_oCoverNote

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get iPMUCoverNote.Interface", gPMConstants.PMELogLevel.PMLogError)
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Exit Sub
                End If

            End If

            m_lReturn = CType(m_oCoverNote.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_oCoverNote.CoverNoteBookId = 0


            m_lReturn = m_oCoverNote.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start interface", gPMConstants.PMELogLevel.PMLogError)
            End If

            'If not cancelled, refresh grid

            If m_oCoverNote.Status = gPMConstants.PMEReturnCode.PMCancel Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            ClearInterface(bSilent:=True)
            cmdFindNow_Click(cmdFindNow, New EventArgs())

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim oInterface As Object

        Const kMethodName As String = "cmdEdit_Click"
        Try


            'Check before calling edit that any item is selecetd or not
            If lvwSearchResults.FocusedItem Is Nothing Then
                Exit Sub
            End If

            m_lReturn = CType(DataToProperties(), gPMConstants.PMEReturnCode)
            'There has to be a valid book id available to edit
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (StringsHelper.ToDoubleSafe(v_sBookId) <= 0) Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Create cover note object if not already done so
            If m_oCoverNote Is Nothing Then
                ' Get an instance of the cover note interface object via the public object manager.
                Dim temp_m_oCoverNote As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oCoverNote, sClassName:="iPMUCoverNote.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oCoverNote = temp_m_oCoverNote

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get iPMUCoverNote.Interface", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            m_lReturn = CType(m_oCoverNote.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetProcessMode Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_oCoverNote.CoverNoteBookId = v_sBookId


            m_lReturn = m_oCoverNote.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start interface", gPMConstants.PMELogLevel.PMLogError)
            End If

            'If not cancelled, refresh grid

            If m_oCoverNote.Status = gPMConstants.PMEReturnCode.PMCancel Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            FindNow()

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub lvwSearchResults_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        Const kMethodName As String = "lvwSearchResults_GotFocus"
        Try


            ' Unset any default buttons so can select by keys
            VB6.SetDefault(cmdFindNow, False)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub lvwSearchResults_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        Const kMethodName As String = "lvwSearchResults_LostFocus"
        Try


            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub lvwSearchResults_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        Const kMethodName As String = "lvwSearchResults_DblClick"
        Try


            ' Check if there are any items available.
            If lvwSearchResults.Items.Count = 0 Then
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                cmdEdit_Click(cmdEdit, New EventArgs())
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub lvwSearchResults_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs)
        Dim ColumnHeader As ColumnHeader = lvwSearchResults.Columns(eventArgs.Column)

        ' Column click event for the search details
        OnColumnClick(lvwSearchResults, ColumnHeader)

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
            If cboBranch.SelectedIndex >= 0 Then

                oFindParty.BranchID = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
            Else

                oFindParty.BranchID = g_iSourceID
            End If

            m_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = ACApp

            m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="CoverNote", vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "FindParty.SetProcessMode Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set appropriate key


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lErrorNumber = oFindParty.SetKeys(vKeyArray)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "FindParty.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                oFindParty.NotEditable = 1
            End If


            m_lErrorNumber = oFindParty.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "FindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName

                If Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                End If
                vResolvedName = oFindParty.ResolvedName
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


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
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "InterfaceToData"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If ValidateForm() = gPMConstants.PMEReturnCode.PMTrue Then
                If txtBookNumber.Text.Trim() = "" Then

                    v_sBookNumber = Nothing
                Else
                    v_sBookNumber = txtBookNumber.Text.Trim()
                End If

                Dim dbNumericTemp As Double
                If txtStartNumber.Text.Trim() = "" Or gPMFunctions.ToSafeLong(txtStartNumber) = 0 Then

                    v_lStart_Number = Nothing
                ElseIf Double.TryParse(txtStartNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    v_lStart_Number = CStr(gPMFunctions.ToSafeLong(txtStartNumber))
                End If

                Dim dbNumericTemp2 As Double
                If txtEndNumber.Text.Trim() = "" Or gPMFunctions.ToSafeLong(txtEndNumber) = 0 Then

                    v_lEnd_Number = Nothing
                ElseIf Double.TryParse(txtEndNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    v_lEnd_Number = CStr(gPMFunctions.ToSafeLong(txtEndNumber))
                End If

                If m_lParty_Cnt <= 0 Then

                    v_lAgent_Cnt = Nothing
                Else
                    v_lAgent_Cnt = m_lParty_Cnt
                End If


                If cboAssignedDate.Checked = True Then
                    v_dtAssigned_Date = cboAssignedDate.Value
                Else
                    v_dtAssigned_Date = Nothing
                End If


                If cboLastUpdate.Checked = True Then
                    v_dtLast_Updated = cboLastUpdate.Value
                Else
                    v_dtLast_Updated = Nothing
                End If

                If txtPolicyNumber.Text.Trim() = "" Then

                    v_sPolicyNumber = Nothing
                Else
                    v_sPolicyNumber = txtPolicyNumber.Text.Trim()
                End If

                If cboBranch.SelectedIndex = -1 Then

                    v_lSource_Id = Nothing
                Else
                    v_lSource_Id = CStr(VB6.GetItemData(cboBranch, cboBranch.SelectedIndex))
                End If

                If cboCoverNoteBookStatus.SelectedIndex = -1 Then

                    v_lCover_Note_Book_Status_Id = Nothing
                Else
                    v_lCover_Note_Book_Status_Id = CStr(VB6.GetItemData(cboCoverNoteBookStatus, cboCoverNoteBookStatus.SelectedIndex))
                End If
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
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


            result = gPMConstants.PMEReturnCode.PMTrue

            If txtStartNumber.Text.Trim() <> "" Then
                Dim dbNumericTemp As Double
                If Not Double.TryParse(txtStartNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Please enter a valid start number.", "Find Cover Note", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            End If

            If txtEndNumber.Text.Trim() <> "" Then
                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(txtEndNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Please enter a valid end number.", "Find Cover Note", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues() As Integer

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
                gPMFunctions.RaiseError(kMethodName, "Failed to get lookup values", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupRow
    '
    ' Description: Converts a lookup table name to its matching row index
    '              in the table of lookup values.
    '              May be used to indirect GetLookupDetails, GetLookupDesc.
    '              Returns -1 if no match found
    '
    ' ***************************************************************** '
    Public Function GetLookupRow(ByRef sLookupTable As String) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        Const kMethodName As String = "GetLookupRow"
        Try


            result = -1

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            If bFoundMatch Then
                result = lRow
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
    Public Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer

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
    ' Name: GetLookupDesc
    '
    ' Description: Gets a description string for a given lookup set
    '              and lookup id.
    '
    ' ***************************************************************** '
    Public Function GetLookupDesc(ByRef lLookupRow As Integer, ByRef lLookupID As Integer, ByRef sLookupDesc As String) As Integer

        Dim result As Integer = 0
        Dim bFoundMatch As Boolean

        Const kMethodName As String = "GetLookupDesc"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if there has been a table match.
            If lLookupRow = -1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Using the lookup values, populate the lookup
            ' string from the lookup details array when the
            ' lookup ID has been matched.

            For lCntr As Integer = CInt(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) To CInt((CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) + CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lLookupRow))) - 1)
                ' Check for a match on the ID.
                If CInt(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)) = lLookupID Then
                    ' Found a match

                    ' Store the details to the lookup string.
                    sLookupDesc = CStr(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lCntr)).Trim()

                    Exit For
                End If
            Next lCntr

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

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
End Class
