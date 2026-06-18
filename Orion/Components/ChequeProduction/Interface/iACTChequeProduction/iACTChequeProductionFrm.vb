Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 10/10/2000
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' CJB 30/06/05      PN22008 Changed cmdAction_Click as system option no. for Default Cheque Letter
    '                   has been changed from 65 to 5003
    ' RKS 27/06/2006    Enchancement to Cheque Production (NEM Insurance)
    ' ***************************************************************** '
    'Changed iPMFunc.GetResData to GetResData in the whole document

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    Private m_iTask As Integer
    Private m_bCancelled As Boolean

    ' {* USER DEFINED CODE (Begin) *}
    Private m_sLedgerCode As String = ""

    ' What solution are we running as part of?
    Private m_iSolutionConfig As Integer

    ' Instance of Find Transaction
    Private m_oFindTransaction As Object
    Private m_lAccountId As Integer
    Private m_sAccountCode As String = ""
    Private m_vSourceArray(,) As Object

    'Limit of cheque no.
    'PN: 45791
    Private Const knMaxChequeNo As Double = 9999999999.0#

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTChequeProduction.General

    Private m_oFindAccount As Object

    ' Declare an instance of the nav starter
    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Private m_vSearchData(,) As Object
    Private m_vChequeData As Object

    'System option setting
    Private m_sChequeSystemOption As String = ""

    Private m_vBankWiseStartChequeNumber As Object
    Public m_bCanUserOverrideChequeNumber As Boolean
    Private m_oFormFields As iPMFormControl.FormFields

    Private sortColumn As Integer = -1
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

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


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
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
    Public ReadOnly Property ChequeArray() As String
        Get

            ' Return the Transaction Array



            ChequeData = m_vChequeData

        End Get
    End Property
    Public WriteOnly Property ChequeData() As Object
        Set(ByVal Value As Object)

            ' Set the valid sources for the user


            m_vChequeData = Value
        End Set
    End Property


    Public Property SourceArray() As Object
        Get

            ' Return the Source Array

            Return VB6.CopyArray(m_vSourceArray)

        End Get
        Set(ByVal Value As Object)

            ' Set the valid sources for the user
            m_vSourceArray = Value

        End Set
    End Property



    Public ReadOnly Property NavigatorTitle() As String
        Get

            ' Return the objects parameter value.
            Return m_sNavigatorTitle

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)


    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim dtDateTo As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            m_vSearchData = Nothing


            dtDateTo = CDate(txtDateTo.Text)

            If uctBankAccount.Text = "ALL" Then

                m_lReturn = g_oBusiness.GetCheques(r_vResultArray:=m_vSearchData, r_dtTransactionDate:=dtDateTo, v_iSourceID:=VB6.GetItemData(cboSource, cboSource.SelectedIndex))
            Else

                m_lReturn = g_oBusiness.GetCheques(r_vResultArray:=m_vSearchData, r_dtTransactionDate:=dtDateTo, v_vBankAccountId:=uctBankAccount.AccountId, v_iSourceID:=VB6.GetItemData(cboSource, cboSource.SelectedIndex))
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No found search details

                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

        Dim lCurrencyID As Integer
        Dim sFormattedAmount As String


        'Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                CheckButtons()
                Return result
            End If
            ' Hide the list view
            'developer guide no. 170
            m_lReturn = ListViewFunc.ListViewBatchStart(lvwList:=lvwSearchDetails)

            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Don't use any icon
                If CStr(m_vSearchData(ACIChequeNumber, lRow)).Trim() = "" Then
                    oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACISourceDescription, lRow)).Trim())
                    oListItem.Checked = True
                Else
                    oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACISourceDescription, lRow)).Trim())
                    oListItem.Checked = False
                End If

                oListItem.Name = "K" & CStr(m_vSearchData(ACIChequeID, lRow))

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIBankCode, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIDocumentRef, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACITransactionDate, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vSearchData(ACIAccountName, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vSearchData(ACIReference, lRow)).Trim()

                lCurrencyID = CInt(m_vSearchData(ACICurrencyID, lRow))

                'Use Transaction currency not Base currency

                m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=CInt(m_vSearchData(ACICurrencyID, lRow)), vCurrencyAmount:=m_vSearchData(ACIAmount, lRow), vFormattedCurrency:=sFormattedAmount, vConversionDate:=DateTime.Today)
                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = sFormattedAmount
                ListViewHelper.GetListViewSubItem(oListItem, 7).Text = CStr(m_vSearchData(ACIChequeNumber, lRow)).Trim()
                oListItem.Tag = CStr(lRow)
            Next lRow

            If lvwSearchDetails.Items.Count = 0 Then
                Return result
            End If

            ' Size the columns
            ListView6Autosize(lvwList:=lvwSearchDetails, bSizeHeaders:=True)

            ' Show the list view

            'developer guide no. 170
            m_lReturn = ListViewFunc.ListViewBatchEnd()


            ' Enable the interface now that the search
            ' has completed.
            m_lReturn = DisableInterface(bDisable:=False)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            CheckButtons()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

        Dim lChequeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lRow As Integer = 0 To m_vChequeData.GetUpperBound(1)

                lChequeId = CInt(m_vChequeData(ACIChequeID, lRow))
                For lLoop As Integer = 0 To m_vSearchData.GetUpperBound(1)
                    If lChequeId = CInt(m_vSearchData(ACIChequeID, lLoop)) Then

                        m_vSearchData(ACIChequeNumber, lLoop) = m_vChequeData(ACIChequeNumber, lRow)
                        Exit For
                    End If
                Next lLoop
            Next lRow

            If g_bPrinted Then
                cmdOK.Visible = True
                cmdOK.Enabled = True

                cmdOK.Left = cmdCancel.Left
                cmdCancel.Visible = False
            End If

            m_lReturn = DataToInterface()


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
        Try


            ' Get the lookup values.

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
        Dim sResult, sAppName, sSection As String
        Dim vDefault As Object

        Dim lLower, lUpper As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Get what solution we're part of

            m_lReturn = g_oSirConfig.GetRegSettings(sResult:=sResult, sAppName:=sAppName, sSection:=sSection, sKey:=gACTLibrary.ACTOrionSolutionValue, vDefault:=vDefault)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMOK) Or (sResult = "0") Then
                ' Default to MBP style of solution
                sResult = CStr(gACTLibrary.ACTOrionSolutionMBP)
            End If

            m_iSolutionConfig = CInt(sResult)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cmdDelete.Enabled = False
            'cmdDelete.Visible = False


            ' Handle button captions
            Select Case m_sChequeSystemOption
                Case "0", "1", "2"
                    cmdAction.Text = "&Print"
                    cmdPrint(0).Visible = True
                    cmdPrint(1).Visible = True
                Case "3"
                    cmdAction.Text = "&Export"
                    cmdPrint(0).Visible = False
                    cmdPrint(1).Visible = False
            End Select

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Default the date
            txtDateTo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, DateTime.Now)

            cmdOK.Enabled = False
            cmdOK.Visible = False

            uctBankAccount.ListIndex = uctBankAccount.ListCount - 1

            'Populate the source combo box
            If Information.IsArray(m_vSourceArray) Then
                cboSource.Items.Clear()

                lLower = m_vSourceArray.GetLowerBound(1)
                lUpper = m_vSourceArray.GetUpperBound(1)

                ' If we have more than one branch add an all branches options
                If lLower < lUpper Then
                    Dim cboSource_NewIndex As Integer = -1
                    cboSource_NewIndex = cboSource.Items.Add("ALL")
                    VB6.SetItemData(cboSource, cboSource_NewIndex, 0)
                End If

                ' Load Options From Source File
                For lCount As Integer = lLower To lUpper
                    'developer guide no. 153 & 162
                    Dim listIndex As Integer = cboSource.Items.Add(New VB6.ListBoxItem(CStr(m_vSourceArray(2, lCount)), CInt(m_vSourceArray(0, lCount))))
                Next lCount

                ' Set index (should always be 0, either "ALL" or the only branch
                If cboSource.Items.Count > 0 Then
                    cboSource.SelectedIndex = 0
                End If
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the message.
            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Don't continue with the clear.
                Return result
            End If

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.
            _stbStatus_Panel1.Text = ""

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to clear all of the interface details
            ' for a new search.
            '
            ' Example:-
            '
            '    txtName.Text = ""
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            txtDateTo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, DateTime.Now)

            ' Set focus to the search details.

            ' {* USER DEFINED CODE (End) *}

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = DisableInterface(bDisable:=True)


            cboSource.SelectedIndex = 0

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            'ReDim m_ctlTabFirstLast(1, )

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

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

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0

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


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdSelect.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            For iLoop1 As Integer = ACListTitle1 To ACListTitle8



                lvwSearchDetails.Columns.Item(iLoop1 - ACListTitle1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=iLoop1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Next iLoop1

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            stbStatus.Text = " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

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
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.l

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data.

            'eck110901 removed
            '    If (Trim$(pnlAccountCode.Caption) <> "") Then
            '        CheckMandatory = PMTrue
            '        Exit Function
            '    End If

            If txtDateTo.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub CheckMandatoryEnable()

        ' Check mandatory and enable the Find Now button accordingly
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub
    ' PRIVATE Methods (End)


    ' ********************************************************************************* '
    ' Name: Private Function                                                            '
    '                                                                                   '
    ' Description: Checks that the transaction is for one of the branches being paid    '
    '                                                                                   '
    ' ********************************************************************************* '
    'UPGRADE_NOTE: (7001) The following declaration (ValidSource) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidSource(ByVal vSource As Object) As Boolean
    'Dim result As Boolean = False
    'If Not Information.IsArray(m_vSourceArray) Then
    'Return True
    'End If
    'For 'i As Integer = 1 To m_vSourceArray.GetUpperBound(1)

    'If CInt(m_vSourceArray(1, i)) = CInt(vSource) Then
    'result = True
    'End If
    'Next i
    'Return result
    'End Function


    ' ***************************************************************** '
    '
    ' Name: ChequeMasterCheques
    '
    ' Description: Writes array of cheques for printing and creates a
    '               csv file for Checkprint software
    '
    ' ***************************************************************** '
    Private Function ChequeMasterCheques() As Integer
        Dim result As Integer = 0

        Dim sSourceDesc As String = ""

        Dim oQuickSort As New QuickSort
        Dim vSortColumn(1) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vChequeData) Then
                m_lReturn = GenerateBankWiseStartingCheckNumber()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result 'GoTo Err_ChequeMasterCheques
                End If



                g_oBusiness.BankWiseStartChequeNumber = m_vBankWiseStartChequeNumber

                ' CJB 070203 - Start
                ' Modify this initial version only currently used by Carribean customers
                ' to also be used by a UK customer (Lumleys) with a different version of the CheckMaster SW...
                ' This needs to be changed before the Carribean customers upgrade to 1.8 to work with multiple
                ' customers. ideally we would have one CheckMaster install (rather than 2 versions like we have
                ' now) and we would pass a unique customer identifier to CheckMaster to print the relevant chq.
                ' bACTChequeProduction would also need to be changed to work for all customers...

                ' Get the desc of the currently logged on source (branch) as it needed to be passed to CheckMaster as
                ' a parameter for Lumleys cheques
                sSourceDesc = CStr(SourceArray(3, g_iSourceID)).Trim()


                m_lReturn = g_oBusiness.ChequeMasterCheques(v_sSourceDescription:=sSourceDesc, r_vResultArray:=m_vChequeData)

                'Now sort the resulting array by description/list id

                vSortColumn(0) = ACBankID

                vSortColumn(1) = ACIChequeNumber
                oQuickSort.IsDecending = False


                oQuickSort.SortArray = m_vChequeData
                oQuickSort.SortColumn = vSortColumn
                oQuickSort.QuickSort()


                m_vChequeData = oQuickSort.SortArray

                oQuickSort = Nothing

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                'ChequePrinted Confirmation
                m_lReturn = ChequePrintedConfirmation()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChequeMasterCheques Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChequeMasterCheques", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function BuildSelectionArray() As Integer
        ' ---------------------------------------------------------------------------
        ' NAME: BuildSelectionArray
        ' DESCRIPTION:
        ' AUTHOR: Danny Davis
        ' DATE: 23 May 2005, 14:45:02
        ' HISTORY:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "BuildSelectionArray"
        Dim lRow As Integer

        Dim lFoundRow As Integer

        Dim oQuickSort As New QuickSort
        Dim vSortColumn(1) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_vChequeData = Nothing

            For lCheques As Integer = 1 To lvwSearchDetails.Items.Count
                lRow = lCheques - 1
                If lvwSearchDetails.Items.Item(lCheques - 1).Checked Then
                    If Not Information.IsArray(m_vChequeData) Then
                        ReDim m_vChequeData(m_vSearchData.GetUpperBound(0), 0)
                    Else

                        ReDim Preserve m_vChequeData(m_vSearchData.GetUpperBound(0), m_vChequeData.GetUpperBound(1) + 1)
                    End If

                    'Search the key of selected item in m_vSearchData
                    For lFoundRow = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                        If lvwSearchDetails.Items.Item(lCheques - 1).Name = "K" & CStr(m_vSearchData(ACIChequeID, lFoundRow)) Then
                            Exit For
                        End If
                    Next lFoundRow

                    'Extract the Cheque Detail from m_vSearchData to m_vChequeData

                    For lCol As Integer = 0 To m_vChequeData.GetUpperBound(0)


                        m_vChequeData(lCol, m_vChequeData.GetUpperBound(1)) = m_vSearchData(lCol, lFoundRow)
                    Next lCol
                End If
            Next lCheques

            'Now sort the resulting array by description/list id

            'vSortColumn(0) = ACBankID

            'vSortColumn(1) = ACIChequeID
            'oQuickSort.IsDecending = False


            'oQuickSort.SortArray = m_vChequeData
            'oQuickSort.SortColumn = vSortColumn
            'oQuickSort.QuickSort()


            'm_vChequeData = oQuickSort.SortArray

            'oQuickSort = Nothing


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Private Sub cmdAction_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAction.Click
        Dim sDocumentTemplateID As String = ""
        Dim lDocumentTemplateID As Integer
        Dim sExportPath, sExportFile As String
        Dim lSpoolMode As Integer

        If txtDateTo.Text.Trim() = "" Then
            MessageBox.Show("Date To field can not be blank.", "Cheque Production", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDateTo.Focus()
            Exit Sub
        End If
        'Get the selection
        BuildSelectionArray()
        If Not Information.IsArray(m_vChequeData) Then
            Exit Sub
        End If

        ' Handle button captions
        Select Case m_sChequeSystemOption
            Case "0"
                MessageBox.Show("Cheque Production is not enabled. Please check your system options.", "Cheque Production not enabled", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Case "1"
                'ChequeMaster
                m_lReturn = ChequeMasterCheques()

                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    MessageBox.Show("The CheckMaster software is not installed on your server.", "CheckMaster not installed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to Print CheckMaster Cheques.", "Print failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                If Not cmdSelect.Enabled Then
                    m_lReturn = EnableDisableControls(True)
                End If
                cmdFindNow_Click(cmdFindNow, New EventArgs())


            Case "2"
                'In-House
                iPMFunc.GetSystemOption(5003, sDocumentTemplateID) 'PN22008
                lDocumentTemplateID = gPMFunctions.ToSafeLong(sDocumentTemplateID, 0)
                If lDocumentTemplateID = 0 Then
                    'Get the user to choose a template
                    m_lReturn = GetDocumentTemplate(lDocumentTemplateID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lDocumentTemplateID = 0 Then
                        Exit Sub
                    End If
                End If

                m_lReturn = GenerateBankWiseStartingCheckNumber()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If



                g_oBusiness.BankWiseStartChequeNumber = m_vBankWiseStartChequeNumber

                If cmdPrint(0).Checked Then
                    lSpoolMode = 3 'ACPrintSilentMode 3
                Else
                    lSpoolMode = 4 'ACSpoolDocMode 4
                End If


                m_lReturn = g_oBusiness.PrintCheques(m_vChequeData, lDocumentTemplateID, lSpoolMode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to Print In-House Cheques.", "Print failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                'ChequePrinted Confirmation
                m_lReturn = ChequePrintedConfirmation()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to call ChequePrintedConfirmation", "Check Printed Confirmation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                If Not cmdSelect.Enabled Then
                    m_lReturn = EnableDisableControls(True)
                End If
                cmdFindNow_Click(cmdFindNow, New EventArgs())
            Case "3"
                'Export
                iPMFunc.GetSystemOption(158, sExportPath)
                If sExportPath = "" Then
                    MessageBox.Show("There is no server Export Path set. Please check your System Options.", "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If


                m_lReturn = g_oBusiness.ExportCheques(m_vChequeData, sExportPath, sExportFile)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to Export Cheque Data.", "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    m_lReturn = ChequeExportedConfirmation()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to call ChequeExportedConfirmation", "Check Exported Confirmation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                End If

                If Not cmdSelect.Enabled Then
                    m_lReturn = EnableDisableControls(True)
                End If
                cmdFindNow_Click(cmdFindNow, New EventArgs())
        End Select

    End Sub


    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        BuildSelectionArray()

        If Information.IsArray(m_vChequeData) Then
            If MessageBox.Show("Are you sure you wish to Delete the selected items?", "Delete Cheques", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then

                m_lReturn = g_oBusiness.ClearCheques(r_vResultArray:=m_vChequeData)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to Delete the Cheques", "Delete failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                'Refresh list
                m_oGeneral.GetInterfaceDetails()
            End If
        End If
    End Sub


    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click

        For Each oListItem As ListViewItem In lvwSearchDetails.Items
            oListItem.Checked = True
        Next oListItem

        CheckButtons()
    End Sub

    Private Sub cmdUnselect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUnselect.Click

        For Each oListItem As ListViewItem In lvwSearchDetails.Items
            oListItem.Checked = False
        Next oListItem

        CheckButtons()
    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Try

                ' Tell the resizer control about the controls on the form
                With uctPMResizer

                    .SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("cmdFindNow", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdNewSearch", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("imgImage", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROWidthOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lvwSearchDetails", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("lblSource", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblAccountCode", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblDateTo", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("lblTotalSelectedLabel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblTotalSelected", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)




                    .SetControlResizeOption("pnlAccountCode", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cboSource", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("txtDateTo", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("uctBankAccount", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("cmdSelect", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdAction", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdPrint", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("cmdUnselect", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("stbStatus", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .FormMinHeight = 5685
                    .FormMinWidth = 9675

                End With

                Exit Sub

            Catch excep As System.Exception



                ' Error Section
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to activate the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub

            End Try
        End If
    End Sub

    ' ***************************************************************** '
    '
    ' Name: GetDocumentTemplate
    '
    ' Description: Creates an instance of find document template and gets the properties
    '
    ' History: 01/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetDocumentTemplate(ByRef r_lDocumentTemplateID As Integer) As Integer
        Dim result As Integer = 0



        Dim oObject As iPMBFindDocTemplate.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of iPMBFindDocTemplate.Interface
            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Start it up

            m_lReturn = oObject.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Exit out if it was cancelled

            If oObject.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            ' Get it's properties
            With oObject

                r_lDocumentTemplateID = .DocumentTemplateID
            End With

            ' Terminate it

            oObject.Dispose()

            ' Clear up
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTChequeProduction.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

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



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

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

            ' Get the system setting
            iPMFunc.GetSystemOption(60, m_sChequeSystemOption)

            'Add ALL in As Bank account
            uctBankAccount.AddItem("ALL")

            'Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
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

            'To get the value that user is authorised to override the starting cheque number

            m_lReturn = g_oBusiness.CanOverrideChequeNumber(v_lUserId:=g_oObjectManager.UserID, r_bCanOverrideChequeNumber:=m_bCanUserOverrideChequeNumber)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            Me.Refresh()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the find transaction if needed
            If Not (m_oFindTransaction Is Nothing) Then
                ' Terminate the instance

                m_oFindTransaction.Dispose()
                

                ' Remove the instance
                m_oFindTransaction = Nothing
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

           

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate it
            If Not (m_oNavStart Is Nothing) Then
                m_oNavStart.Dispose()
                

                ' Remove the instance
                m_oNavStart = Nothing
            End If

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
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lvwSearchDetails_ItemCheck(ByVal eventSender As Object, ByVal eventArgs As ItemCheckEventArgs) Handles lvwSearchDetails.ItemCheck

    End Sub

    Private Sub CheckButtons()

        Dim bChecked As Boolean = False

        If lvwSearchDetails.Items.Count = 0 Then
            cmdSelect.Enabled = False
            cmdUnselect.Enabled = False
        Else
            cmdSelect.Enabled = True
            cmdUnselect.Enabled = True
            For Each oListItem As ListViewItem In lvwSearchDetails.Items
                If oListItem.Checked Then
                    bChecked = True
                    Exit For
                End If
            Next oListItem
        End If

        cmdAction.Enabled = bChecked
        cmdDelete.Enabled = bChecked
        cmdPrint(0).Enabled = bChecked
        cmdPrint(1).Enabled = bChecked
    End Sub

    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavStart.NavigatorClose

        ' Re-enable the controls
        lvwSearchDetails.Enabled = True

        cmdFindNow.Enabled = True
        cmdNewSearch.Enabled = True

        cmdOK.Enabled = True
        cmdCancel.Enabled = True
        cmdHelp.Enabled = True
        ' Gets the interface details to be displayed.
        m_lReturn = m_oGeneral.GetInterfaceDetails()
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                'If (.Tab < cmdNext.Count) Then
                '    cmdNext(.Tab).Default = True
                'Else
                '    cmdOK.Default = True
                'End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            If lvwSearchDetails.Items.Count = 0 Then
                Me.Hide()
                Exit Sub
            End If

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            If lvwSearchDetails.Items.Count = 0 Then
                Me.Hide()
                Exit Sub
            End If

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click


        ' Click event of the Cancel button.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            ' Set the focus.
            lvwSearchDetails.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.
        Try

            ' Check if there are any items available.
            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            ' Call the edit button function
            lvwSearchDetails.FocusedItem.Checked = Not lvwSearchDetails.FocusedItem.Checked

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Set the default button
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)
        Dim iDirection As SortOrder
        'Dim sortColumn As Integer = -1
        ' Column click event for the search details

        Try
            RemoveHandler lvwSearchDetails.ItemChecked, AddressOf lvwSearchDetails_ItemChecked
            If ListViewHelper.GetSortOrderProperty(lvwSearchDetails) = 1 Then
                iDirection = SortOrder.Descending
            Else
                iDirection = SortOrder.Ascending
            End If


            With lvwSearchDetails

                If eventArgs.Column <> sortColumn Then
                    sortColumn = eventArgs.Column
                    .Sorting = SortOrder.Ascending
                Else
                    If .Sorting = SortOrder.Ascending Then
                        .Sorting = SortOrder.Descending
                    Else
                        .Sorting = SortOrder.Ascending
                    End If

                End If
                ' If current sort column header is
                ' pressed.

                Select Case ColumnHeader.Index + 1
                    ' Sort the date column
                    Case 5
                        'developer guide no. 170
                        'm_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=lvwSearchDetails, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                        m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=lvwSearchDetails, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=iDirection)

                    Case Else

                        If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails) Then
                            ' Set sort order opposite of
                            ' current direction.
                            'ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                            ListViewHelper.SetSortOrderProperty(lvwSearchDetails, iDirection)
                        Else
                            ' Sort by this column (ascending).
                            ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

                            ' Turn off sorting so that the list
                            ' is not sorted twice
                            ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                            ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
                            ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
                        End If

                        Select Case ColumnHeader.Text
                            Case "Amount"

                                ListViewSortByStringVal(v_oListView:=lvwSearchDetails, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=iDirection)
                            Case Else
                                .Sort()
                                .ListViewItemSorter = New ListViewItemComparer(eventArgs.Column, .Sorting)
                        End Select

                End Select

            End With
            AddHandler lvwSearchDetails.ItemChecked, AddressOf lvwSearchDetails_ItemChecked

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (End)

    Private isInitializingComponent As Boolean
    Private Sub txtDateTo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Enter
        ' Hightlight any text.
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateTo)
        'SelectText txtDateTo
    End Sub

    Private Function AskForStartingChequeNumber(ByRef vBankID As String, ByRef vBankCode As String) As Integer
        Dim result As Integer = 0
        Dim sChequeNumber As String = ""
        Dim bValidChequeNumber As Boolean
        Dim vDuplicateChequeFound As Object

        Dim sGeneratedChequeNumber, sStartChequeNumber, sHighestIssuedChequeNumber As String
        Dim bIsOutOfSequence As Boolean
        Const kMethodName As String = "AskForStartingChequeNumber"

        Try

            result = True

            bValidChequeNumber = False
            sGeneratedChequeNumber = "0"

            Do

                m_lReturn = g_oBusiness.GetBankStartChequeNumber(v_lBankID:=vBankID, r_sStartChequeNumber:=sStartChequeNumber)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("g_oBusiness.GetBankStartChequeNumber()", "BankId = " & vBankID)
                End If


                m_lReturn = g_oBusiness.GetBankHighestIssuedChequeNumber(v_lBankID:=vBankID, r_sHighestIssuedChequeNumber:=sHighestIssuedChequeNumber)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("g_oBusiness.GetBankHighestIssuedChequeNumber()", "BankId = " & vBankID)
                End If

                If sHighestIssuedChequeNumber = "" And sStartChequeNumber = "" Then
                    sGeneratedChequeNumber = "0"
                ElseIf sHighestIssuedChequeNumber <> "" Then
                    sGeneratedChequeNumber = CStr(gPMFunctions.ToSafeLong(sHighestIssuedChequeNumber) + 1)
                ElseIf sHighestIssuedChequeNumber = "" And sStartChequeNumber <> "" Then
                    sGeneratedChequeNumber = sStartChequeNumber
                End If

                'Prompt for starting cheque Number
                sChequeNumber = Interaction.InputBox("Enter Starting Cheque Number for the Bank : " & vBankCode, "Starting Cheque " & "Number", sGeneratedChequeNumber)
                'If sChequeNumber.Trim() = "" Or StringsHelper.ToDoubleSafe(sChequeNumber.Trim()) = 0 Then Exit Do
                If sChequeNumber.Trim() = "" Then Exit Do

                Dim dbNumericTemp As Double
                If StringsHelper.ToDoubleSafe(sChequeNumber.Trim()) = 0 Then
                    MessageBox.Show("Cheque Number can not be 0. Please specify a valid number ", "Cheque Production", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    bValidChequeNumber = False
                ElseIf Not Double.TryParse(sChequeNumber, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    bValidChequeNumber = False
                ElseIf StringsHelper.ToDoubleSafe(sChequeNumber) > knMaxChequeNo Then
                    MessageBox.Show("Cheque Number Entered is greater than the maximum number valid " & knMaxChequeNo & Strings.Chr(13) & Strings.Chr(10) & "Please specify a valid number ", "Cheque Production", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    bValidChequeNumber = False
                Else
                    sChequeNumber = StringsHelper.Format(sChequeNumber, "##########")
                    'Check for validity of Cheque Number

                    m_lReturn = g_oBusiness.CheckDuplicateCheque(vBankAccoutID:=vBankID, sChequeNumber:=sChequeNumber, r_vDuplicateChequeFound:=vDuplicateChequeFound)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check duplicate cheque", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If

                    If Information.IsArray(vDuplicateChequeFound) Then
                        'Duplicate Cheque found
                        bValidChequeNumber = False
                        MessageBox.Show("The cheque number entered has already been used", "Duplicate Cheque Number", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ElseIf sChequeNumber <> sGeneratedChequeNumber And Not m_bCanUserOverrideChequeNumber And sGeneratedChequeNumber <> "0" Then
                        MessageBox.Show("User does not have the authority to override cheque numbers", "Cheque Number Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        bValidChequeNumber = False
                    ElseIf sStartChequeNumber <> "" And gPMFunctions.ToSafeLong(sChequeNumber) < gPMFunctions.ToSafeLong(sStartChequeNumber) Then
                        MessageBox.Show("Warning: Number entered precedes the Start Cheque Number as configured against the bank account. Please enter a different number.", "Invalid Starting Cheque Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        sGeneratedChequeNumber = sChequeNumber
                        bValidChequeNumber = False
                    ElseIf gPMFunctions.ToSafeLong(sChequeNumber) < gPMFunctions.ToSafeInteger(sHighestIssuedChequeNumber) And sHighestIssuedChequeNumber <> "" Then
                        bIsOutOfSequence = False

                        m_lReturn = g_oBusiness.IsOutOfSequenceCheques(r_bIsOutofSequenceCheques:=bIsOutOfSequence, v_sStartChequeNumber:=sChequeNumber, v_vChequeData:=m_vChequeData, v_lBankID:=vBankID)
                        If bIsOutOfSequence Then
                            If System.Windows.Forms.DialogResult.Cancel = MessageBox.Show("Warning: Cheque number will not be sequential when produced." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?", "Out of Sequence Cheques", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) Then
                                Return result
                            Else
                                bValidChequeNumber = True
                            End If
                        Else
                            bValidChequeNumber = True
                        End If
                    Else
                        bValidChequeNumber = True
                    End If
                End If

            Loop While (Not bValidChequeNumber)

            result = gPMFunctions.ToSafeLong(sChequeNumber, -1)



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
    End Function
    Private Function GenerateBankWiseStartingCheckNumber() As Integer

        Dim result As Integer = 0
        Dim lStartChequeNo As Integer
        Dim bStartingChequeFound As Boolean



        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_vBankWiseStartChequeNumber = Nothing


            'Loop through all the rows of selected cheque data and
            'ask for starting cheque number for each selected bank

            For lStartRow As Integer = m_vChequeData.GetLowerBound(1) To m_vChequeData.GetUpperBound(1)
                'Search the Bank ID in vBankWiseStartChequeNumber
                'if found do not ask for the starting cheque number
                'else ask for the cheque number and update the vBankWiseStartChequeNumber array
                bStartingChequeFound = False

                If Information.IsArray(m_vBankWiseStartChequeNumber) Then

                    For lStartSearch As Integer = m_vBankWiseStartChequeNumber.GetLowerBound(1) To m_vBankWiseStartChequeNumber.GetUpperBound(1)


                        If m_vBankWiseStartChequeNumber(ACBankID, lStartSearch).Equals(m_vChequeData(ACIBankID, lStartRow)) Then
                            bStartingChequeFound = True
                            Exit For
                        End If
                    Next lStartSearch
                End If

                If Not bStartingChequeFound Then


                    lStartChequeNo = AskForStartingChequeNumber(vBankID:=CStr(m_vChequeData(ACIBankID, lStartRow)), vBankCode:=CStr(m_vChequeData(ACIBankCode, lStartRow)))

                    If lStartChequeNo <= 0 Then
                        Return False
                    ElseIf lStartChequeNo > knMaxChequeNo - m_vChequeData.GetUpperBound(1) Then
                        MessageBox.Show("Starting Cheque Number is crossing the limit." & Strings.Chr(13) & Strings.Chr(10) & "Please specify a valid number.", "Cheque Production", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    Else
                        If Information.IsArray(m_vBankWiseStartChequeNumber) Then

                            ReDim Preserve m_vBankWiseStartChequeNumber(2, m_vBankWiseStartChequeNumber.GetUpperBound(1) + 1)
                        Else
                            ReDim m_vBankWiseStartChequeNumber(2, 0)
                        End If




                        m_vBankWiseStartChequeNumber(ACBankID, m_vBankWiseStartChequeNumber.GetUpperBound(1)) = m_vChequeData(ACIBankID, lStartRow)



                        m_vBankWiseStartChequeNumber(ACBankCode, m_vBankWiseStartChequeNumber.GetUpperBound(1)) = m_vChequeData(ACIBankCode, lStartRow)


                        m_vBankWiseStartChequeNumber(ACStartChequeNumber, m_vBankWiseStartChequeNumber.GetUpperBound(1)) = lStartChequeNo
                    End If
                End If
            Next lStartRow


            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateBankWiseStartingCheckNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Private Function ChequePrintedConfirmation() As Integer
        Dim result As Integer = 0
        Dim dtPrintedDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If MessageBox.Show("Have the cheques been printed successfully?", "Cheque Printing Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then

                m_lReturn = EnableDisableControls(False)
                'update the cheque with the printed_date and printed_by_user_id
                'display the Cheque Register
                dtPrintedDate = DateTime.Now


                For lRow As Integer = 0 To m_vChequeData.GetUpperBound(1)


                    m_lReturn = g_oBusiness.UpdateChequePrinted(lChequeId:=gPMFunctions.ToSafeLong(CStr(m_vChequeData(ACIChequeID, lRow))), dtPrintedDate:=dtPrintedDate, lPrintedByUserID:=gPMFunctions.ToSafeLong(CStr(g_iUserID)))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to UpdateChequePrinted", "Update failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Next lRow

                'display cheque register
                m_lReturn = ReportChequeRegister(vPrintedDate:=dtPrintedDate.ToString("yyyy/MM/dd HH:mm:ss"), lPrintedByUserID:=gPMFunctions.ToSafeLong(g_iUserID))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to print Cheque Register", "ReportChequeRegister failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                g_bPrinted = True
            Else
                g_bPrinted = False

                For lRow As Integer = 0 To m_vChequeData.GetUpperBound(1)


                    m_lReturn = g_oBusiness.UpdateCheque(lChequeId:=CInt(m_vChequeData(ACIChequeID, lRow)), sChequeNumber:="")
                Next
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to Rollback the Changes", "Update failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured", vApp:=ACApp, vClass:=ACClass, vMethod:="ChequePrintedConfirmation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Private Function ChequeExportedConfirmation() As Integer
        Dim result As Integer = 0
        Dim dtExportedDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If MessageBox.Show("Have the cheques been exported successfully?", "Cheque Export Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then

                m_lReturn = EnableDisableControls(False)

                dtExportedDate = DateTime.Now


                For lRow As Integer = 0 To m_vChequeData.GetUpperBound(1)


                    m_lReturn = g_oBusiness.UpdateChequePrinted(lChequeId:=gPMFunctions.ToSafeLong(CStr(m_vChequeData(ACIChequeID, lRow))), dtPrintedDate:=dtExportedDate, lPrintedByUserID:=gPMFunctions.ToSafeLong(CStr(g_iUserID)))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to UpdateChequeExported", "Update failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Next lRow

                g_bPrinted = True
            Else
                g_bPrinted = False
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error occured", vApp:=ACApp, vClass:=ACClass, vMethod:="ChequeExportedConfirmation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    Private Function ReportChequeRegister(ByRef vPrintedDate As Object, ByRef lPrintedByUserID As Integer) As Integer
        Dim result As Integer = 0



        Dim oReport As iPMBReportPrint.Interface_Renamed 'Instance of iPMBReportPrint Component
        Dim vKeyArray(1, 6) As Object 'Array used to set the value of parameters required by report

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Instance of Reportprint component
            Dim temp_oReport As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReport = temp_oReport

            ' Trap the error if Instance of iPMBReportPrint is not created properly
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create 'iPMBReportPrint.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportChequeRegister", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                If Information.IsReference(oReport) Then

                    oReport.Dispose()
                    oReport = Nothing
                End If
                Return result
            End If

            With oReport

                ' Assign the parameter value to the array variable

                vKeyArray(0, 0) = PMNavKeyConst.PMKeyNameReportName '"report_name"

                vKeyArray(1, 0) = "Cheque_Register"


                vKeyArray(0, 1) = PMNavKeyConst.PMKeyNamePrintReport '"report_print_options"

                vKeyArray(1, 1) = PMNavKeyConst.AC_VIEW_ONLY


                'Submit (printed date) to generate report.

                vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameParam1Name '"param_name1"

                vKeyArray(1, 2) = "printed_date"


                vKeyArray(0, 3) = "printed_date"


                vKeyArray(1, 3) = vPrintedDate
                '

                'Submit (printed_by_user_id) Parameter to generate report.

                vKeyArray(0, 4) = PMNavKeyConst.PMKeyNameParam2Name '"param_name2"

                vKeyArray(1, 4) = "printed_by_user_id"


                vKeyArray(0, 5) = "printed_by_user_id"

                vKeyArray(1, 5) = lPrintedByUserID


                ' Send the parameter value the iPMBReportPrint Component via SetKeys function.

                m_lReturn = .SetKeys(vKeyArray:=vKeyArray)

                ' Trap error if generated
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Keys for Report.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportChequeRegister", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                    .Dispose()
                    oReport = Nothing
                    Return result
                End If


                ' Generate Report

                m_lReturn = .Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Generate the Report.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportChequeRegister", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                    .Dispose()
                    oReport = Nothing
                    Return result
                End If

                ' Close Report Component

                .Dispose()
                oReport = Nothing

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReportChequeRegister Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportChequeRegister", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub txtDateTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateTo)
    End Sub

    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            'From Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateTo, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function EnableDisableControls(ByRef bEnable As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EnableDisableControls"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdSelect.Enabled = bEnable
            cmdUnselect.Enabled = bEnable
            cmdAction.Enabled = bEnable
            cmdDelete.Enabled = bEnable
            cmdPrint(0).Enabled = bEnable
            cmdPrint(1).Enabled = bEnable
            cmdOK.Enabled = bEnable
            cmdCancel.Enabled = bEnable
            lvwSearchDetails.Enabled = bEnable
            tabMainTab.Enabled = bEnable
            cmdFindNow.Enabled = bEnable
            cmdNewSearch.Enabled = bEnable



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: ListViewSortByStringValue
    '
    ' Description: Sorts the list view based on the column passed, and
    '              the order given.
    '
    ' Note : This is the copy of the original function of iPMListViewFunc.Bas
    '        with some changes particular to the issue no 32220
    ' ***************************************************************** '
    Public Function ListViewSortByStringVal(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer

        Dim result As Integer = 0
        Const ACLVTag As String = "SORT_VALUE_HIDDEN"

        Dim cValue As Decimal
        Dim sValue As String = ""
        Dim iIndex As Integer
        Dim bNegative As Boolean
        Dim iLen As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the column
            'PSL 02/10/2003 Should be zero width as well
            v_oListView.Columns.Add(ACLVTag, "Internal Sort", CInt(VB6.TwipsToPixelsX(0)))

            ' Get the index of this new column, -1 because it's a sub item
            iIndex = v_oListView.Columns.Count - 1

            ' Not sorted yet
            ListViewHelper.SetSortedProperty(v_oListView, False)

            ' Add the items
            For lLoop1 As Integer = 1 To v_oListView.Items.Count

                If v_iSourceColumn = 0 Then
                    sValue = StringsHelper.Format(v_oListView.Items.Item(lLoop1 - 1).Text, "#,##0.00")
                Else

                    'PSL 05/08/2003 Issue 5830
                    'Changed various bits, so negative numbers, and various currency formats work
                    Dim dbNumericTemp5 As Double
                    If ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1 Then
                        Dim dbNumericTemp3 As Double
                        Dim dbNumericTemp As Double
                        If Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(0, ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                            sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(0, ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1)
                        ElseIf Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1))), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1)))
                            If ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.StartsWith("-") Then
                                sValue = CStr(CDbl(sValue) * -1)
                            End If
                        Else
                            iLen = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Trim().Length

                            For iCount As Integer = iLen To 1 Step -1
                                sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, iCount, 1)
                                Dim dbNumericTemp2 As Double
                                If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                    sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, 1, iCount)
                                    Exit For
                                End If
                            Next
                        End If
                    ElseIf Not Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                        iLen = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Trim().Length

                        For iCount As Integer = iLen To 1 Step -1
                            sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, iCount, 1)
                            Dim dbNumericTemp4 As Double
                            If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                                sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, 1, iCount)
                                Exit For
                            End If
                        Next

                    Else
                        sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text
                    End If
                    sValue.TrimEnd()

                    If sValue.StartsWith("-") Then
                        sValue = Mid(sValue, 2, sValue.Length - 1)
                        bNegative = True
                    Else
                        bNegative = False
                    End If
                    If sValue.Substring(0, 1) < "0" Or sValue.Substring(0, 1) > "9" Then
                        sValue = sValue.Substring(sValue.Length - (sValue.Length - 1))
                    End If
                    If bNegative Then
                        cValue = 1000000000 - CDec(sValue)
                    Else
                        cValue = CDec(sValue) + 1000000000
                    End If
                    sValue = StringsHelper.Format(cValue, "0000000000.00")

                End If
                ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), iIndex).Text = sValue

            Next lLoop1

            ' Sort now
            ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)

            ' Set the sort key
            ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)

            ListViewHelper.SetSortedProperty(v_oListView, True)

            ' Remove the column now
            v_oListView.Columns.RemoveAt(iIndex)

            ' Reset the sort key
            'eck 010800
            '    v_oListView.SortKey = v_iSourceColumn%
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByStringVal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByStringVal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwSearchDetails_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwSearchDetails.ItemChecked
        Dim Item As ListViewItem = lvwSearchDetails.Items(e.Item.Index)
        CheckButtons()
    End Sub

    Private Sub frmInterface_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate

    End Sub
End Class
