Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:18/06/2007
    '
    ' Description: Main interface.
    '
    ' Edit History: VB
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    'Constants for Defining Width of Columns in List View
    Private Const ColWidth As Integer = 1590

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iTask As gPMConstants.PMEComponentAction

    ' Variables for Find Case
    Private m_sCaseNumber As String = ""
    Private m_lProgressStatusId As Integer
    Private m_dtCaseOpenDate As Date
    Private m_sAnalyst As String = ""
    Private m_sAssistant As String = ""
    Private m_sCaseProgressStatus As String = ""
    Private m_lTotalIndemnity As Integer
    Private m_lTotalExpense As Integer
    Private m_lTotalExcess As Integer
    Private m_lClaimNumber As Integer
    Private m_lRiskTypeId As Integer
    Private m_lCaseID As Integer
    Private m_lBaseCaseID As Integer
    Private m_lClaimID As Integer
    Private m_lPartyCnt As Integer
    Private blnSearchFieldEdited As Boolean = False

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMFindCase.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    Private m_oRisk As iPMURisk.Interface_Renamed

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData As Object

    Dim m_lOldCaseID As Integer

    'Developer guide No. 8
    Private Const vbFormCode As Integer = 0

    '------------------------------------------------

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.11.2.1)
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
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.11.2.1)

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


    'Private Sub Status(ByVal Value As Integer)
    ' Standard Property.
    ' Set the interface exit status.
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get
            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus
        End Get
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


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property


    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property

    Public Property ProgressStatusID() As Integer
        Get
            Return m_lProgressStatusId
        End Get
        Set(ByVal Value As Integer)
            m_lProgressStatusId = Value
        End Set
    End Property

    Public Property CaseOpenDate() As Date
        Get
            Return m_dtCaseOpenDate
        End Get
        Set(ByVal Value As Date)
            m_dtCaseOpenDate = Value
        End Set
    End Property

    Public Property ClaimNumber() As Integer
        Get
            Return m_lClaimNumber
        End Get
        Set(ByVal Value As Integer)
            m_lClaimNumber = Value
        End Set
    End Property

    Public Property RiskTypeID() As Integer
        Get
            Return m_lRiskTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name          : GetBusiness
    ' Description   : Retrieves the details from the business object.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sSQL As String
        Dim vCaseOpenDate As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while a search is in progress.
            m_lReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_vSearchData = Nothing

            If cboCaseOpenDate.Checked Then

                vCaseOpenDate = cboCaseOpenDate.Value
            Else
                vCaseOpenDate = ""
            End If


            m_lReturn = g_oBusiness.GenerateSQL(v_sClaimNumber:=txtClaimNumber.Text, v_sCaseNumber:=txtCaseNumber.Text, v_lRiskTypeID:=cboRiskType.ItemId, v_lProgressStatusID:=cboProgressStatus.ItemId, v_vCaseOpenDate:=vCaseOpenDate, r_sSQL:=sSQL)


            ' Get the Case details from the business object.

            m_lReturn = g_oBusiness.FindCase(v_sSQL:=sSQL, r_vResultArray:=m_vSearchData)


            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMNotFound
                    If txtClaimNumber.Text.Trim().Length > 0 And Not Information.IsArray(m_vSearchData) Then
                        MessageBox.Show("No case is associated with the selected claim." & _
                                        Strings.Chr(13) & Strings.Chr(10) & "Use maintain claim task to proceed", "Find Case", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            'Assign Values to Interface
            m_lReturn = DataToInterface()
            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name        : DataToInterface
    ' Description : Updates all interface details from the search data.
    '               storage.
    ' Date        : 18/06/2007
    ' Edit History: VB
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwsearchdetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                ' Disable the interface now that the search has completed.
                m_lReturn = DisableInterface(bDisable:=True)
                Return result
            End If

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Assign the details to the first column.


                'developer guide no.49
                oListItem = lvwsearchdetails.Items.Add(CStr(m_vSearchData(kICaseNumber, lRow)).Trim(), ACFindImage)

                ' Assign details to other the columns

                oListItem.SubItems.Add(1).Text = gPMFunctions.ToSafeDate(m_vSearchData(kICaseOpenDate, lRow))

                oListItem.SubItems.Add(2).Text = gPMFunctions.ToSafeString(m_vSearchData(kIAnalyst, lRow)).Trim()

                oListItem.SubItems.Add(3).Text = gPMFunctions.ToSafeString(m_vSearchData(kIAssistant, lRow)).Trim()

                oListItem.SubItems.Add(4).Text = gPMFunctions.ToSafeString(m_vSearchData(kIProgressStatus, lRow)).Trim()

                oListItem.SubItems.Add(5).Text = CStr(gPMFunctions.ToSafeLong(m_vSearchData(kITotalIndemnity, lRow))).Trim()

                oListItem.SubItems.Add(6).Text = CStr(gPMFunctions.ToSafeLong(m_vSearchData(kITotalExpense, lRow))).Trim()

                oListItem.SubItems.Add(7).Text = CStr(gPMFunctions.ToSafeLong(m_vSearchData(kITotalExcess, lRow))).Trim()

                oListItem.SubItems.Add(8).Text = CStr(gPMFunctions.ToSafeLong(m_vSearchData(kICaseID, lRow)))

                'oListItem.SubItems.Add(9).Text = CStr(gPMFunctions.ToSafeLong(m_vSearchData(KIPartyCnt, lRow)))

                ' Set the tag property with the index of the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows,
                ' to allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwsearchdetails.Items.Item(0).Selected = True
                    ' Refresh the initial results.
                    lvwsearchdetails.Refresh()
                End If
            Next lRow


            If Information.IsArray(m_vSearchData) Then
                ' Enable the interface now that the search has completed.
                m_lReturn = DisableInterface(bDisable:=False)
            Else
                ' Disable the interface now that the search has completed.
                m_lReturn = DisableInterface(bDisable:=True)
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : DataToProperties
    ' Description   : Updates the property member from the search data
    '                 storage.
    '
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem, lCopyCaseId As Integer
        Dim sMessage As String = ""
        Dim sClientShortName As String = ""

        'Const kMethodName As String = "DataToProperties"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(lvwsearchdetails.FocusedItem.Index).Tag)

            ' use details from the initial search data array get generic fields

            m_lCaseID = gPMFunctions.ToSafeLong(CStr(m_vSearchData(kICaseID, lSelectedItem)))

            m_lBaseCaseID = gPMFunctions.ToSafeLong(CStr(m_vSearchData(kIBaseCaseID, lSelectedItem)))

            m_sCaseNumber = CStr(m_vSearchData(kILvwCaseNumber, lSelectedItem)).Trim()
            ''62125

            'm_lPartyCnt = gPMFunctions.ToSafeLong(CStr(m_vSearchData(KIPartyCnt, lSelectedItem)))

            If m_iTask <> gPMConstants.PMEComponentAction.PMView And m_sTransactionType = "C_EC" Then

                ' clean up any existing dirty case prior to taking out a new lock

                m_lReturn = g_oBusiness.CleanUpDirtyCase(m_lCaseID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '       For Future Amendment....
                '        m_lReturn& = LockCase()
                '        If m_lReturn <> PMTrue Then
                '            DataToProperties = PMRecordInUse
                '            Exit Function
                '        End If
                '        g_oBusiness.TransactionType = "C_EC"


                m_lReturn = g_oBusiness.ProcessCopyCase(v_lCaseID:=m_lCaseID, r_lCopyCaseId:=lCopyCaseId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lOldCaseID = m_lCaseID
                m_lCaseID = lCopyCaseId
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : SetInterfaceDefaults
    ' Description   : Sets all of the interface default values.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            lvwsearchdetails.Columns.Item(kILvwCaseNumber - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth + 100))
            lvwsearchdetails.Columns.Item(kILvwCaseOpenDate - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth + 100))
            lvwsearchdetails.Columns.Item(kILvwAnalyst - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))
            lvwsearchdetails.Columns.Item(kILvwAssistant - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))
            lvwsearchdetails.Columns.Item(kILvwProgressStatus - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))
            lvwsearchdetails.Columns.Item(kILvwTotalIndemnity - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))
            lvwsearchdetails.Columns.Item(kILvwTotalExpense - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))
            lvwsearchdetails.Columns.Item(kILvwTotalExcess - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))
            lvwsearchdetails.Columns.Item(kILvwCaseID - 1).Width = CInt(0)


            lvwsearchdetails.Top = tabMainTab.Height + VB6.TwipsToPixelsY(600)
            RemoveHandler cboCaseOpenDate.ValueChanged, AddressOf cboCaseOpenDate_ValueChanged
            cboCaseOpenDate.Value = DateTime.Today
            AddHandler cboCaseOpenDate.ValueChanged, AddressOf cboCaseOpenDate_ValueChanged
            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Made full row select on list views
            'developer guide no.303
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwsearchdetails.Handle.ToInt32(), v_vShowRowSelect:=True)
            lvwsearchdetails.FullRowSelect = True
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : ClearInterface
    ' Description   : Clears all of the interface details for a new
    '                 search.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function ClearInterface(Optional ByVal bIsMsgRequired As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If bIsMsgRequired Then
                ' Display the message.
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return result
                End If
            End If

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwsearchdetails.Items.Clear()

            ' Clear the search status bar.
            stbstatus.Text = "Ready"

            ' All fields should be cleared.
            txtCaseNumber.Text = ""
            txtClaimNumber.Text = ""
            cboCaseOpenDate.Value = DateTime.Today

            'cboCaseOpenDate.Value = Nothing
            cboProgressStatus.ListIndex = 0
            cboRiskType.ListIndex = 0
            uctPBSearchField1.ClearInterface()

            ' Set focus to the search details.
            txtCaseNumber.Focus()

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = DisableInterface(bDisable:=True)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : SetFirstLastControls
    ' Description   : Sets the first and last data entry controls for
    '                 each tab to the control array, for use with the
    '                 keyboard navigation.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 1)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            m_ctlTabFirstLast(ACControlStart, 0) = txtCaseNumber
            m_ctlTabFirstLast(ACControlEnd, 0) = cboRiskType

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : DisplayCaptions
    ' Description   : Display all language specific captions.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCaseNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCaseNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblProgressStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kProgressStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCaseOpenDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCaseOpenDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblClaimNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRiskType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kRiskType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNewcase.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kNewCaseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEditCase.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kEditCaseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCloseCase.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCloseCaseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdClose.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCloseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Case number


            lvwsearchdetails.Columns.Item(kILvwCaseNumber - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameCaseNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Case Open date


            lvwsearchdetails.Columns.Item(kILvwCaseOpenDate - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameOpenedDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Analyst


            lvwsearchdetails.Columns.Item(kILvwAnalyst - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameAnalyst, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Assistant


            lvwsearchdetails.Columns.Item(kILvwAssistant - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameAssistant, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Case Progress Status


            lvwsearchdetails.Columns.Item(kILvwProgressStatus - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameProgressStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Total Indemnity


            lvwsearchdetails.Columns.Item(kILvwTotalIndemnity - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameTotalIndemnity, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Total Expense


            lvwsearchdetails.Columns.Item(kILvwTotalExpense - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameTotalExpense, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Total Excess


            lvwsearchdetails.Columns.Item(kILvwTotalExcess - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameTotalExcess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Clear the search status bar.
            'stbstatus.Text = "Ready"
            _stbstatus_Panel1.Text = "Ready"
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : DisableInterface
    ' Description   : Disables parts of the interface while a search is
    '                 in progress.
    '
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdEditCase.Enabled = Not bDisable
            cmdCloseCase.Enabled = Not bDisable

            If cboProgressStatus.ItemCode.ToUpper = "CLOSED" Then
                cmdCloseCase.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : DisplayStatusSearching
    ' Description   : Display the status searching message.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbstatus_Panel1.Text = " " & sMessage

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : DisplayStatusFound
    ' Description   : Display the status found message.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else

                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbstatus_Panel1.Text = " " & lItemsFound & " " & sMessage


        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : CheckMandatory
    ' Description   : Check if all mandatory fields have been entered in
    '                 order for the search to proceed.
    '
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If txtCaseNumber.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtClaimNumber.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            If cboRiskType.ListIndex > 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cboProgressStatus.ListIndex > 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : ResizeInterface
    ' Description   : Resizes the interface controls.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1425)
            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1425)

            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1650)

            lvwsearchdetails.Top = tabMainTab.Height + VB6.TwipsToPixelsY(220)
            lvwsearchdetails.Width = Me.Width - VB6.TwipsToPixelsX(320)
            lvwsearchdetails.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - VB6.PixelsToTwipsY(stbstatus.Height) - cmdNewcase.Focused - VB6.PixelsToTwipsY(tabMainTab.Height) - 1240)

            cmdClose.Left = Me.Width - VB6.TwipsToPixelsX(1415)

            cmdNewcase.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(lvwsearchdetails.Top) + VB6.PixelsToTwipsY(lvwsearchdetails.Height) + 60)
            cmdEditCase.Top = cmdNewcase.Top
            cmdCloseCase.Top = cmdNewcase.Top
            cmdClose.Top = cmdNewcase.Top

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub cboCaseOpenDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCaseOpenDate.ValueChanged
        cmdFindNow.Enabled = True
    End Sub

    Private Sub cboProgressStatus_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProgressStatus.Click
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
    End Sub

    Private Sub cboRiskType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRiskType.Click
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
    End Sub

    Private Sub cmdCloseCase_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCloseCase.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        m_sTransactionType = ""
        ' Process the next set of actions.
        m_lReturn = m_oGeneral.ProcessCommand()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        CloseCase()

        'm_lReturn = ClearInterface(bIsMsgRequired:=False)
        cboProgressStatus.ListIndex = 0
        cmdFindNow_Click(cmdFindNow, New EventArgs())
    End Sub

    Private Sub cmdNewcase_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewcase.Click

        m_lReturn = ShowCaseScreen(v_lTask:=gPMConstants.PMEComponentAction.PMAdd, v_sTransactionType:="C_NC")

        m_lReturn = ClearInterface(bIsMsgRequired:=False)

        txtCaseNumber.Text = m_sCaseNumber

        If Len(Trim(m_sCaseNumber)) > 0 Then
            cmdFindNow_Click(cmdFindNow, New EventArgs())
        End If
    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            m_lReturn = ResizeInterface()
        End If
    End Sub

    ' ***************************************************************** '
    ' Name          : FormIntialise
    ' Description   : Intialise all required details of the form
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'For viewing the Form in TaskBar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMFindCase.General()

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID


            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            Dim temp_m_oRisk As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRisk, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oRisk = temp_m_oRisk

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            m_lReturn = CType(m_oRisk, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            'Load user control
            uctPBSearchField1.Load_Renamed()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : FormLoad
    ' Description   : Loads all required details of the form
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try
            Me.cboRiskType.FirstItem = "(ALL)"
            Me.cboProgressStatus.FirstItem = "(ALL)"
            'For viewing the Form in TaskBar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Validate fields using Forms Control

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            If g_bChangeCloseCaption Then
                txtCaseNumber.Text = g_sCaseNumber
                cmdClose.Text = "OK"
                cmdCloseCase.Visible = False
                cmdEditCase.Visible = False
                cmdNewcase.Visible = False
                If Trim(g_sCaseNumber) <> "" Then
                    cboCaseOpenDate.Checked = False
                    cmdFindNow_Click(cmdFindNow, New EventArgs())
                End If

            Else
                If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Inadequate data so cannot
                    ' continue with the search.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

                ' Gets the interface details to be displayed.
                m_lReturn = m_oGeneral.GetInterfaceDetails()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the interface details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : Form_Query Unload
    ' Description   : Store all Property Details before unloading form
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                'm_lReturn& = m_oGeneral.ProcessCommand()

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



            ' Destroy the instance of the general object from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : Form_KeyDown
    ' Description   : Determine the Position of Tab and Control on
    '                 pressing pageup,pagedown,home,end buttons
    '
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'developer guide no.293
            'start
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If

            'end
        Catch




            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name          :Form_Resize
    ' Description   :Resize the the controls on form
    ' Date          :18/06/2007
    ' Edit History  :VB
    ' ***************************************************************** '
    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = ResizeInterface()

        Catch



            Exit Sub
        End Try


    End Sub


    Private Sub Form_Terminate_Renamed()

        '    If Not m_oParty Is Nothing Then
        '        m_lReturn = m_oParty.Terminate
        '        Set m_oParty = Nothing
        '    End If

    End Sub

    ' ***************************************************************** '
    ' Name          : tabMainTab_Click
    ' Description   : Set the Focus on the First control on the relevant Tab Clicked
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                If .SelectedIndex = 0 Then blnSearchFieldEdited = False
                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch





            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : cmdEditCase_Click
    ' Description   : Set Properties of the form on clicking Edit Case Button from the
    '                 relevant list item under focus or clicked
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub cmdEditCase_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCase.Click

        Const ACMethod As String = "cmdEditCase_click"

        Dim bShowCustomScreen As Boolean
        Dim lPreviousDataModelId, lGISPolicyLinkID As Integer

        Try

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_sTransactionType = "C_EC"
            m_iTask = gPMConstants.PMEComponentAction.PMEdit

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If GetPreviousCaseBuilderDataModel(v_lCaseID:=m_lCaseID, r_lPreviousDataModelId:=lPreviousDataModelId, r_lGISPolicyLinkID:=lGISPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get previous screen data model info.")
            End If

            If lPreviousDataModelId > 0 Then
                If MessageBox.Show("Warning: the data model screen has been changed for Case," & _
                                   Strings.Chr(13) & Strings.Chr(10) & "continuing will reset the custom data.", "Custom Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Cancel Then

                    bShowCustomScreen = False
                Else
                    'Delete all Party Builder GIS data for the policy link
                    bShowCustomScreen = Not (DeleteCustomData(v_lGISPolicyLinkID:=lGISPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue)
                End If
            Else
                bShowCustomScreen = True
            End If

            If bShowCustomScreen Then
                m_lReturn = ShowCaseScreen(v_lTask:=gPMConstants.PMEComponentAction.PMEdit, v_sTransactionType:="C_EC", v_lCaseID:=m_lCaseID)

                cboProgressStatus.ListIndex = 0
            End If

            If m_sCaseNumber <> "" Then
                txtCaseNumber.Text = m_sCaseNumber
            End If

            cmdFindNow_Click(cmdFindNow, New EventArgs())

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdEditCase_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : cmdCancel_Click
    ' Description   : Unload the Form
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        ' Click event of the Cancel button.

        Try
            If g_bChangeCloseCaption Then
                m_sCaseNumber = txtCaseNumber.Text
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
            Else
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
            End If
        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdCloseClick_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name          : cmdFindNow_Click
    ' Description   : Get the Details from Bussiness Object
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.11.2.2)
        Dim sWildcardErrorMessage As String = ""
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.11.2.2)

        ' Click event of the Find Now button.

        Try

            Dim sSQL As String

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.11.2.2)
            'Check wildcard searches

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtCaseNumber.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Case")
                txtCaseNumber.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtClaimNumber.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Case")
                txtClaimNumber.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=uctPBSearchField1.RiskIndex, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Case")
                uctPBSearchField1.RiskIndex_setfocus()
                Exit Sub

            End If
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.11.2.2)

            If SSTabHelper.GetSelectedIndex(tabMainTab) = 1 Then
                If uctPBSearchField1.DataModelTypeID = 0 And uctPBSearchField1.RiskIndex.Trim() <> "" Then

                    m_lReturn = GetCaseByRiskIndex()
                    ' Check for errors

                ElseIf uctPBSearchField1.DataModelTypeID > 0 Then
                    If uctPBSearchField1.SearchFields.Count > 0 Then

                        m_lReturn = g_oBusiness.GenerateSQL(r_sSQL:=sSQL, v_vSearchFields:=uctPBSearchField1.SearchFields)


                        ' Get the Case details from the business object.

                        m_lReturn = g_oBusiness.FindCase(v_sSQL:=sSQL, r_vResultArray:=m_vSearchData)
                    End If
                End If


                ' Assign the details from the search data storage
                ' to the interface.
                m_lReturn = DataToInterface()

                ' Display the number of item found message.
                DisplayStatusFound()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            Else
                ' Gets the interface details to be displayed.
                m_lReturn = m_oGeneral.GetInterfaceDetails()
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            If lvwsearchdetails.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                VB6.SetDefault(cmdEditCase, False)
                VB6.SetDefault(cmdCloseCase, False)
                m_lReturn = DisableInterface(bDisable:=False)
            End If

            If g_bChangeCloseCaption Then
                If (lvwsearchdetails.Items.Count > 0) Then
                    lvwsearchdetails.Items.Item(0).Selected = True
                    lvwsearchdetails_Click(lvwsearchdetails, New EventArgs())
                    cmdClose.Enabled = True
                Else
                    cmdClose.Enabled = False
                End If
            Else
                ' Set the focus.
                lvwsearchdetails.Focus()
            End If



            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : cmdNewSearch_Click
    ' Description   : Clear all controls on the form
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = ClearInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : lvwSearchDetails_GotFocus
    ' Description   : Set Ok Button a default
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub lvwsearchdetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwsearchdetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Unset any default buttons so can select with Enter key.
            VB6.SetDefault(cmdFindNow, False)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : lvwSearchDetails_lostfocus
    ' Description   : Set find now as default
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub lvwsearchdetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwsearchdetails.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : lvwSearchDetails_Click
    ' Description   : Fill the Claim Reference,Policy No.,Client Short Name
    '                 in Text Box for the listitem clicked
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub lvwsearchdetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwsearchdetails.SelectedIndexChanged
        Dim lIndex As Integer

        If lvwsearchdetails.SelectedItems.Count > 0 Then


            lIndex = Convert.ToString(lvwsearchdetails.SelectedItems(0).Tag)

            txtCaseNumber.Text = CStr(m_vSearchData(kILvwCaseNumber, lIndex)).Trim()

            cboCaseOpenDate.Value = gPMFunctions.ToSafeDate(CStr(m_vSearchData(kILvwCaseOpenDate, lIndex)))

            For lRow As Integer = 0 To cboProgressStatus.ListCount

                If cboProgressStatus.ItemCaption.ToUpper() = gPMFunctions.ToSafeString(CStr(m_vSearchData(kILvwProgressStatus, lIndex))).Trim().ToUpper() Then
                    Exit For
                End If
                If Not (cboProgressStatus.IsItemDeleted(lRow)) Then
                    cboProgressStatus.ListIndex = lRow
                End If
            Next lRow
            If cboProgressStatus.ItemCode.ToUpper = "CLOSED" Then
                cmdCloseCase.Enabled = False
            Else
                cmdCloseCase.Enabled = True

            End If
            VB6.SetDefault(cmdEditCase, True)
        End If

    End Sub

    ' ***************************************************************** '
    ' Name         : lvwSearchDetails_DblClick
    ' Description  : Move to the next form in the road map
    ' Date         : 18/06/2007
    ' Edit History : VB
    ' ***************************************************************** '
    Private Sub lvwsearchdetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwsearchdetails.DoubleClick

        Try

            ' Check if there are any items available.
            If lvwsearchdetails.Items.Count = 0 Then
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If g_bChangeCloseCaption = False Then
                m_lReturn = ShowCaseScreen(v_lTask:=gPMConstants.PMEComponentAction.PMEdit, v_sTransactionType:="C_EC", v_lCaseID:=m_lCaseID)

                'PN48602
                cmdFindNow_Click(cmdFindNow, New EventArgs())
            Else
                m_sCaseNumber = txtCaseNumber.Text
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name          : lvwSearchDetails_ColumnClick
    ' Description   : Sort the Details of List View as per the column clicked
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub lvwsearchdetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwsearchdetails.ColumnClick


        ListViewFunc.SortListView(lvwsearchdetails, eventArgs)




        'Dim ColumnHeader As ColumnHeader = lvwsearchdetails.Columns(eventArgs.Column)

        'Dim iDirection As SortOrder

        '' Column click event for the search details

        'Try 

        '	With lvwsearchdetails

        '		' If date column clicked, then sort by date sort column
        '		If ColumnHeader.Index + 1 - 1 = 4 Then

        '			If ListViewHelper.GetSortKeyProperty(lvwsearchdetails) <> 4 Then
        '				ListViewHelper.SetSortKeyProperty(lvwsearchdetails, 4)
        '				iDirection = SortOrder.Ascending
        '			Else
        '				iDirection = (ListViewHelper.GetSortOrderProperty(lvwsearchdetails) + 1) Mod 2
        '			End If
        '                  'Developer Guide No 271
        '                  m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=lvwsearchdetails, v_iSourceColumn:=4, v_iDirection:=iDirection)

        '			' If current sort column header is
        '			' pressed.
        '		ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwsearchdetails)) Then 
        '			' Set sort order opposite of
        '			' current direction.
        '			ListViewHelper.SetSortOrderProperty(lvwsearchdetails, (ListViewHelper.GetSortOrderProperty(lvwsearchdetails) + 1) Mod 2)
        '		Else
        '			' Sort by this column (ascending).
        '			ListViewHelper.SetSortedProperty(lvwsearchdetails, False)
        '			' Turn off sorting so that the list
        '			' is not sorted twice
        '			ListViewHelper.SetSortOrderProperty(lvwsearchdetails, SortOrder.Ascending)
        '			ListViewHelper.SetSortKeyProperty(lvwsearchdetails, ColumnHeader.Index + 1 - 1)
        '			ListViewHelper.SetSortedProperty(lvwsearchdetails, True)
        '		End If
        '	End With

        'Catch excep As System.Exception



        '	' Log Error.
        '	iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        '	Exit Sub

        'End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : DisplayMessage
    ' Description   : Display the Suitable Message
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '

    'Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)
    '
    'Static sMessage As String = ""
    '
    'Try 
    '

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '
    ' Display the status message.
    '
    'MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    ' Name          : ValidateIndex
    ' Description   : Validates the interface index.
    ' History       :
    ' ***************************************************************** '

    'Private Function ValidateIndex() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If Not Information.IsArray(m_vSearchData) Then
    'result = gPMConstants.PMEReturnCode.PMNotFound
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate index", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result


    '
    'Return result
    'End Try
    'End Function

    Private Sub txtCaseNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCaseNumber.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
    End Sub

    Private Sub txtClaimNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimNumber.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
    End Sub


    ' ***************************************************************** '
    ' Name: ShowCaseScreen
    ' Parameters:
    ' Description:
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function ShowCaseScreen(ByVal v_lTask As Integer, ByVal v_sTransactionType As String, Optional ByVal v_lCaseID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim iPMURisk As Object

        Const kMethodName As String = "ShowCaseScreen"

        Dim vResultArray(,) As Object

        Dim oObject As iPMURisk.Interface_Renamed
        Dim sCaseScreenID As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '    m_lReturn& = oObject.Initialise()
            '
            '    If m_lReturn <> PMTrue Then
            '        RaiseError kMethodName, "Initialise Failed", PMLogError
            '    End If


            m_lReturn = oObject.SetProcessModes(vTask:=v_lTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=v_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            iPMFunc.GetSystemOption(5035, sCaseScreenID)

            If sCaseScreenID.Trim() = "" Or sCaseScreenID = "0" Then
                MessageBox.Show("Please select the Case Screen from System Option", "Find Case", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If


            oObject.ScreenId = gPMFunctions.ToSafeLong(sCaseScreenID)

            oObject.CallingAppName = ACApp
            ''62125
            If v_sTransactionType <> "C_EC" Then
                oObject.PartyCnt = m_lPartyCnt
            End If
            If v_lCaseID > 0 Then

                oObject.CaseID = v_lCaseID

                oObject.BaseCaseID = m_lBaseCaseID

                oObject.CaseNumber = m_sCaseNumber
            End If


            m_lReturn = oObject.Start()

            m_sCaseNumber = oObject.CaseNumber



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally

            If Not (oObject Is Nothing) Then

                oObject.Dispose()
            End If




        End Try
        Return result
    End Function


     ''' <summary>
    ''' CloseCase
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CloseCase() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "CloseCase"
        Const kCaseClaimListClaimID As Integer = 0
        Const kCaseClaimListClaimNumber As Integer = 1
        Const kCaseClaimListStatus As Integer = 5

        Dim sDescription As String = ""
        Dim nReturn As PMEReturnCode
        Dim oCaseClaimList(,) As Object
        Dim lRow As Integer
        Dim sMsg As String = ""
        Dim nDocumentTemplateID As Integer
        Dim sDocumentTemplateID As String = ""
        Dim nDocumentTypeID As Integer
        Dim lAction As DialogResult
        Dim nCountClosedClaim As Integer

        Try


            nResult = PMEReturnCode.PMTrue


            nReturn = g_oBusiness.GetLinks(v_lBaseCaseID:=m_lBaseCaseID, r_vLinks:=oCaseClaimList)

            If nReturn <> PMEReturnCode.PMTrue And nReturn <> PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, " Failed", PMELogLevel.PMLogError)
            End If

            If Information.IsArray(oCaseClaimList) Then
                sMsg = Strings.Chr(13) & Strings.Chr(10)

                For lRow = oCaseClaimList.GetLowerBound(1) To oCaseClaimList.GetUpperBound(1)

                    If gPMFunctions.ToSafeString(CStr(oCaseClaimList(kCaseClaimListStatus, lRow))).Trim().ToUpper() <> "CLOSED" AndAlso _
                        gPMFunctions.ToSafeString(CStr(oCaseClaimList(kCaseClaimListStatus, lRow))).Trim().ToUpper() <> "RECLOSED" Then

                        sMsg = sMsg & gPMFunctions.ToSafeString(CStr(oCaseClaimList(kCaseClaimListClaimNumber, lRow))) & Strings.Chr(13) & Strings.Chr(10)
                    Else
                        nCountClosedClaim += 1
                    End If
                Next lRow
            End If
            If nCountClosedClaim <> lRow Then
                MessageBox.Show("The following claims linked to this case are still open :-" & Strings.Chr(13) & Strings.Chr(10) & sMsg & Strings.Chr(13) & Strings.Chr(10) & _
                                "This case cannot be closed yet.", "Close Case", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return nResult
            Else
                If nCountClosedClaim > 0 Then

                    m_lClaimID = CInt(oCaseClaimList(kCaseClaimListClaimID, 0))
                Else
                    m_lClaimID = 0
                End If

                lAction = MessageBox.Show("Are you sure you want to close the case " & m_sCaseNumber & " ?", "Close Case", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                If lAction = System.Windows.Forms.DialogResult.OK Then

                    nReturn = g_oBusiness.CloseCase(v_lCaseId:=m_lCaseID)

                    sDescription = Interaction.InputBox("What is the description for this change of Case?", "Case")

                    If sDescription.Trim() = "" Then
                        sDescription = "Close Case"
                    End If

                    nReturn = g_oBusiness.CreateEvent(v_lCaseId:=m_lCaseID, v_sEventTypeCode:="CASES", v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription)
                    If nReturn <> PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to create event", PMELogLevel.PMLogError)
                    End If
                Else
                    Return nResult
                End If
            End If

            iPMFunc.GetSystemOption(5034, sDocumentTemplateID)

            nDocumentTemplateID = gPMFunctions.ToSafeLong(sDocumentTemplateID, 0)
            sMsg = "Do you wish to produce the Close Case documentation ?"

            If nDocumentTemplateID <> 0 Then
                If MessageBox.Show(sMsg, "Close Case", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    nReturn = CType(GetTemplateType(lDocumentTemplateID:=nDocumentTemplateID, r_lDocumentTypeID:=nDocumentTypeID), PMEReturnCode)
                    If nReturn <> PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetTemplateType Failed", PMELogLevel.PMLogError)
                    End If
                    nReturn = CType(UseTheTemplate(v_lDocId:=nDocumentTemplateID, v_lDocTypeId:=nDocumentTemplateID), PMEReturnCode)

                    If nReturn <> PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PrintDocument Failed", PMELogLevel.PMLogError)
                    End If
                End If
            End If


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        Finally

        End Try
        Return nResult
    End Function



    ' ***************************************************************** '
    ' Name: GetTemplateType
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function GetTemplateType(ByVal lDocumentTemplateID As Integer, ByRef r_lDocumentTypeID As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRDocTemplate As Object


        Const kMethodName As String = "GetTemplateType"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oDocTemplate As bSIRDocTemplate.Business

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            Dim temp_oDocTemplate As Object
            lReturn = g_oObjectManager.GetInstance(temp_oDocTemplate, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oDocTemplate = temp_oDocTemplate
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRDocManagerWrapper", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oDocTemplate.GetDetails(vDocumentTemplateID:=lDocumentTemplateID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = oDocTemplate.GetNext(vDocumentTypeId:=r_lDocumentTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            oDocTemplate.Dispose()

            oDocTemplate = Nothing




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: PrintDocument
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '

    'Private Function PrintDocument(ByVal v_lDocumentTemplateID As Integer, ByVal v_lDocumentTypeID As Integer, ByVal v_lSpoolMode As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "PrintDocument"
    '
    'Dim lReturn As gPMConstants.PMEReturnCode
    'Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '
    'If v_lDocumentTemplateID <> 0 Then
    '
    'Hook up document management
    'oDocManagerWrapper = New bSIRDocManagerWrapper.Interface()
    'lReturn = CType(CType(oDocManagerWrapper, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRDocManagerWrapper", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'Set up the document
    'oDocManagerWrapper.DocumentTemplateId = v_lDocumentTemplateID
    'oDocManagerWrapper.DocumentTypeId = v_lDocumentTypeID
    'oDocManagerWrapper.DocumentRef = m_sCaseNumber
    'oDocManagerWrapper.SpoolDesc = "Close Case Print Letter"
    'oDocManagerWrapper.Mode = v_lSpoolMode
    '
    ' Print the document
    'lReturn = oDocManagerWrapper.Start()
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Failed to Start", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    '
    'oDocManagerWrapper.Terminate()
    '
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    'Return result
    'Resume 
    'Return result
    'End Function



    ' ***************************************************************** '
    ' Name: GetCaseByRiskIndex
    '
    ' Description: Gets Case by Risk
    '
    ' ***************************************************************** '
    Private Function GetCaseByRiskIndex() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCaseByRiskIndex"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sIndex As String = ""
        Dim vCaseData, vResultData(,) As Object
        Dim iFromRow, iMaxRow, iNumClaims As Integer
        Dim sSQL As String = ""
        Dim vGISSearchDataArray(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            sIndex = uctPBSearchField1.RiskIndex.Trim()


            lReturn = g_oBackofficelink.FindLikeIndex(sIndex:=sIndex, lNumberOfRecords:=gPMConstants.PMAllRecords, vResultArray:=vGISSearchDataArray, sDataModelType:="CASE")

            'clear this array, were going to use it again
            'and definitely dont want to display it or append to it later

            m_vSearchData = Nothing

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to FindLikeIndex", gPMConstants.PMELogLevel.PMLogError)

            Else



                lReturn = g_oBusiness.GetPolicyCase(vGISSearchDataArray, vCaseData, v_vsiriusproduct:=g_oBackofficelink.Sirius_Product, v_vRegNumber:=uctPBSearchField1.RiskIndex)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue And lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    ' Log Error Message
                    gPMFunctions.RaiseError(kMethodName, "Failed to FindLikeIndex", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' If NO Indexes were found return Not Found
                If Not Information.IsArray(vCaseData) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Else
                    'We have claim number(s) now use these to get the clients

                    For iCounter1 As Integer = vCaseData.GetLowerBound(1) To vCaseData.GetUpperBound(1)


                        m_lReturn = g_oBusiness.GenerateSQL(v_sClaimNumber:=vCaseData(3, iCounter1), v_sCaseNumber:="", v_lRiskTypeID:=0, v_lProgressStatusID:=0, v_vCaseOpenDate:="", r_sSQL:=sSQL)


                        ' Get the Case details from the business object.

                        m_lReturn = g_oBusiness.FindCase(v_sSQL:=sSQL, r_vResultArray:=vResultData)

                        If Information.IsArray(vResultData) Then
                            ' Get the no of fields selected

                            iNumClaims = vResultData.GetUpperBound(0)

                            If Not Information.IsArray(m_vSearchData) Then


                                m_vSearchData = vResultData
                            Else
                                ' We alreay have some data and we have to merge it with new data

                                iFromRow = m_vSearchData.GetUpperBound(1)


                                iMaxRow = m_vSearchData.GetUpperBound(1) + vResultData.GetUpperBound(1) + 1
                                ReDim Preserve m_vSearchData(iNumClaims, iMaxRow)


                                For iCounter2 As Integer = vResultData.GetLowerBound(1) To vResultData.GetUpperBound(1)
                                    iFromRow += 1
                                    For iCounter3 As Integer = 0 To iNumClaims


                                        m_vSearchData(iCounter3, iFromRow) = vResultData(iCounter3, iCounter2)
                                    Next iCounter3
                                Next iCounter2
                            End If
                        End If
                    Next
                End If
            End If

            Return result

        Catch ex As Exception



            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here


            Return result
        End Try
    End Function


    ' ************************************************************************************** '
    ' Name: GetPreviousCaseBuilderDataModel
    ' Description: Returns previous data model Id if screen data model has changed else zero
    '              Returns GIS Policy Link Id if there is any
    ' ************************************************************************************** '
    Private Function GetPreviousCaseBuilderDataModel(ByVal v_lCaseID As Integer, ByRef r_lPreviousDataModelId As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPreviousCaseBuilderDataModel"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the details of the screen from the db based on the screen code

            lReturn = g_oBusiness.GetPreviousDataModel(v_lCaseId:=v_lCaseID, r_lPreviousDataModelId:=r_lPreviousDataModelId, r_lGISPolicyLinkID:=r_lGISPolicyLinkID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(lReturn), "GetPreviousDataModel failed.")
            End If

            If r_lPreviousDataModelId > 0 And r_lGISPolicyLinkID <= 0 Then
                gPMFunctions.RaiseError(CStr(lReturn), "Failed to get GIS Policy Link.")
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function


    ' **************************************************************************** '
    ' Name: DeleteCustomData
    ' Description: Deletes all corresponding GIS data for a GIS Policy Link Id
    ' **************************************************************************** '
    Private Function DeleteCustomData(ByVal v_lGISPolicyLinkID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteCustomData"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the details of the screen from the db based on the screen code

            lReturn = g_oBusiness.DeleteCustomData(v_lGisPolicyLinkId:=v_lGISPolicyLinkID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(lReturn), "DeleteCustomData failed.")
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: UseTheTemplate
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '
    Private Function UseTheTemplate(ByVal v_lDocId As Integer, ByVal v_lDocTypeId As Integer) As Integer
        Dim result As Integer = 0
        Dim iPMBDocTemplate As Object

        Const kMethodName As String = "UseTheTemplate"

        Dim lReturn As Integer

        Dim oObject As iPMBDocTemplate.Interface_Renamed

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oObject = Nothing
                Return result
            End If

            ' oObject.Claimcnt = m_lClaimID

            oObject.DocumentTemplateId = v_lDocId

            oObject.DocumentTypeId = v_lDocTypeId

            oObject.SpoolDesc = "Close Case Print Letter"

            oObject.CallingAppName = ACApp

            oObject.Mode = gSIRLibrary.ACMergeMode

            oObject.ClaimCnt = m_lClaimID
            ''62125

            oObject.PartyCnt = m_lPartyCnt

            m_lReturn = oObject.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            oObject.Dispose()





        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            oObject = Nothing
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: LockCase
    ' History: 23-09-2008 Amit.
    ' ***************************************************************** '

    'Private Function LockCase() As Integer
    'Dim result As Integer = 0
    'Dim bPMLock As Object
    '

    'Dim oPMLock As bPMLock.User
    'Dim sLockedBy As String = ""
    'Dim lOriginalCaseId As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Get bPMLock
    'Dim temp_oPMLock As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'oPMLock = temp_oPMLock
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="LockCase", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '
    'Return result
    'End If
    '

    'm_lReturn = g_oBusiness.GetOriginalCaseId(m_lCaseID, lOriginalCaseId)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalCaseId Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="LockCase", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    'Return result
    'End If
    '

    'm_lReturn = oPMLock.LockKey(sKeyName:="Case_id", vKeyValue:=lOriginalCaseId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy, v_bOtherUserOnly:=False)
    '
    '
    'Select Case m_lReturn
    'Case gPMConstants.PMEReturnCode.PMTrue
    'OK
    'm_lOldCaseID = lOriginalCaseId
    'Case gPMConstants.PMEReturnCode.PMFalse
    'Locked or error
    'If sLockedBy = "ERROR" Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockCase", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'MessageBox.Show("Case currently locked by " & sLockedBy &  _
    '                Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Find Case")
    '
    'Return result
    'End If
    '
    '
    'Case Else
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the screen", vApp:=ACApp, vClass:=ACClass, vMethod:="LockCase", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End Select
    '
    'oPMLock = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockCase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockCase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result


    'Return result
    'End Try
    'End Function

    ' PRIVATE Methods (End)

    Private Sub uctPBSearchField1_SearchFieldEdited(ByVal Sender As System.Object, ByVal e As uctPBSearchFields.uctPBSearchField.SearchFieldEditedEventArgs) Handles uctPBSearchField1.SearchFieldEdited
        cmdFindNow.Enabled = gPMFunctions.ToSafeString(e.vRiskIndex) <> "" Or uctPBSearchField1.DataModelTypeID <> 0
        If uctPBSearchField1.DataModelTypeID <= 0 Then
            blnSearchFieldEdited = False
        Else
            blnSearchFieldEdited = True
        End If
    End Sub

    Private Sub cboCaseOpenDate_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCaseOpenDate.Enter

        cmdFindNow.Enabled = True

    End Sub

    Private Sub frmInterface_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyValue = 13 AndAlso cmdFindNow.Enabled AndAlso tabMainTab.SelectedIndex = 1 AndAlso blnSearchFieldEdited Then
            cmdFindNow_Click(Nothing, Nothing)
        End If
    End Sub
End Class
