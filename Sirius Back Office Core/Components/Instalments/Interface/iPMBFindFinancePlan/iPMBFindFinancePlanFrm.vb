Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Imports System.Runtime.InteropServices

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17/02/1997
    '
    ' Description: Main interface.
    '
    ' Edit History: 170297 - Created
    ' TF 240498 - ProcessPartyInterface() added to activate refresh on
    '           return to Find
    ' SP 011298 - changes to support new business roadmap
    ' CJB051005 - PN24604 Changed cmdEdit_Click to only set g_bEditFinancePlanAuthority (to
    '             a possible False value) if we are running via Client Manager...else set it
    '             to True to NOT disable fields in Finance Plan screen.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sLongName As String = ""
    Private m_lFinancePlanCnt As Integer
    Private m_lFinancePlanVersion As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBFindFinancePlan.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookUpDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Declare an instance of the Lock object.

    Private m_oPMLock As Object

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object

    'SJ 26/02/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    'SJ 26/02/2004 - end

    ' PUBLIC Property Procedures (Begin)
    ' Developer Guide NO 7
    Private Const vbFormCode As Integer = 0
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0
    Private _hScollValue As Integer = 0


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
    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property








    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value

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

    ' {* USER DEFINED CODE (Begin) *}
    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property


    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property

    Public Property LongName() As String
        Get

            Return m_sLongName

        End Get
        Set(ByVal Value As String)

            m_sLongName = Value

        End Set
    End Property
    Public Property FinancePlanCnt() As Integer
        Get

            Return m_lFinancePlanCnt

        End Get
        Set(ByVal Value As Integer)

            m_lFinancePlanCnt = FinancePlanCnt

        End Set
    End Property
    Public Property FinancePlanVersion() As Integer
        Get

            Return m_lFinancePlanVersion

        End Get
        Set(ByVal Value As Integer)

            m_lFinancePlanVersion = FinancePlanVersion

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
        Dim sPartyTypeOther As String = ""
        Dim lStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If cboStatus.SelectedIndex >= 0 Then
                lStatus = VB6.GetItemData(cboStatus, cboStatus.SelectedIndex)
            End If

            m_lReturn = g_oBusiness.GetFinancePlanDetails(vClientCnt:=m_lPartyCnt, vStatus:=lStatus, vFinancePlanArray:=m_vSearchData)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

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
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        'DC030106 PN26062
        Dim iStatus As Integer

        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                'DC030106 PN26062 added extra fields for broking
                If m_bIsUnderwritingBranch Then
                    ' Assign the details to the first column.
                    ' Column 1 Plan Reference
                    'SJ 26/02/2004 - start
                    '        Set oListItem = lvwSearchDetails.ListItems.Add(, , _
                    ''            Trim$(m_vSearchData(ACIPolicyNumber, lRow&)), , ACFindImage)
                    If CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim() <> "" Then

                        'Changes as per Vb code
                        'oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim(), "")
                        oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim(), ACFindImage)
                    Else

                        'Changes as per Vb code
                        'oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIPolicyNumber, lRow)).Trim(), "")
                        oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIPolicyNumber, lRow)).Trim(), ACFindImage)
                    End If
                    'SJ 26/02/2004 - end
                    ' Assign details to other the columns
                    ' Column 2 Start Date
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIAccountNo, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vSearchData(ACIAmount, lRow)))
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACIFrequency, lRow)).Trim()
                    'PN13609 Don't display next instalment date if no instalments outstanding
                    If StringsHelper.ToDoubleSafe(CStr(m_vSearchData(ACIRemainingInstalments, lRow)).Trim()) <> 0 Then
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, CStr(m_vSearchData(ACINextPaymentDate, lRow)))
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = ""
                    End If
                    ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vSearchData(ACIRemainingInstalments, lRow)).Trim()

                    iStatus = 5

                Else


                    'Changes as per Vb code
                    'oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIFinanceProvider, lRow)).Trim(), "")
                    oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIFinanceProvider, lRow)).Trim(), ACFindImage)
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIPolicyNumber, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIPlanReference, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACIAccountNo, lRow)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vSearchData(ACIAmount, lRow)))
                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vSearchData(ACIFrequency, lRow)).Trim()
                    If StringsHelper.ToDoubleSafe(CStr(m_vSearchData(ACIRemainingInstalments, lRow)).Trim()) <> 0 Then
                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, CStr(m_vSearchData(ACINextPaymentDate, lRow)))
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = ""
                    End If
                    ListViewHelper.GetListViewSubItem(oListItem, 8).Text = CStr(m_vSearchData(ACIRemainingInstalments, lRow)).Trim()

                    iStatus = 7

                End If

                'DD 02/09/2003: Added Status
                'DC030106 PN26062 set iStatus depending on Underwriting or Broking
                Select Case CStr(m_vSearchData(ACIStatus, lRow)).Trim()
                    Case bSIRPremFinConst.PFStatusIndSaved
                        ListViewHelper.GetListViewSubItem(oListItem, iStatus).Text = "Saved"
                    Case bSIRPremFinConst.PFStatusIndUpdated
                        ListViewHelper.GetListViewSubItem(oListItem, iStatus).Text = "Updated"
                    Case bSIRPremFinConst.PFStatusIndQuotePrinted
                        ListViewHelper.GetListViewSubItem(oListItem, iStatus).Text = "Quoted"
                    Case bSIRPremFinConst.PFStatusIndLive
                        ListViewHelper.GetListViewSubItem(oListItem, iStatus).Text = "Live"
                    Case bSIRPremFinConst.PFStatusIndOnHold
                        ListViewHelper.GetListViewSubItem(oListItem, iStatus).Text = "On Hold"
                    Case bSIRPremFinConst.PFStatusIndCompleted
                        ListViewHelper.GetListViewSubItem(oListItem, iStatus).Text = "Completed"
                    Case bSIRPremFinConst.PFStatusIndSuperseded
                        ListViewHelper.GetListViewSubItem(oListItem, iStatus).Text = "Superceded"
                    Case bSIRPremFinConst.PFStatusIndCancelled
                        ListViewHelper.GetListViewSubItem(oListItem, iStatus).Text = "Cancelled"
                End Select

                ' Set the tag property with the index of the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwSearchDetails.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwSearchDetails.Refresh()
                End If
            Next lRow

            ' Select the first item.
            If lvwSearchDetails.Items.Count > 0 Then
                lvwSearchDetails.Items.Item(0).Selected = True

                ' Enable the interface now that the search has completed.
                m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
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
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.SelectedItems(0).Index).Tag)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            If m_sLongName = "" Then
                m_sLongName = m_sShortName
            End If

            'DC030106 PN26062 was labelled as ACIPlanReference
            m_lFinancePlanCnt = CInt(m_vSearchData(ACIPlanRefCnt, lSelectedItem))
            m_lFinancePlanVersion = CInt(m_vSearchData(ACIPlanVersion, lSelectedItem))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)



    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' {* USER DEFINED CODE (Begin) *}


            txtPartyCode.Text = m_sShortName.Trim()

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



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

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled

                    cmdNavigate.Visible = False
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            cmdEdit.Visible = True
            cmdEdit.Enabled = True
            cmdStatement.Visible = True
            cmdStatement.Enabled = True
            cmdNew.Visible = True
            cmdNew.Enabled = True

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                cmdNew.Visible = False
                cmdNew.Enabled = False
                cmdStatement.Left = cmdEdit.Left
                cmdEdit.Left = cmdNew.Left
                'PN10571 eck 240204
            Else
                cmdOK.Enabled = False
            End If

            'DC030106 PN26062 different columns between broking and underwriting
            If m_bIsUnderwritingBranch Then

                ' Set the column widths for the search list.
                lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(2250))
                lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2100))
                lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(900))
                lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1280))
                lvwSearchDetails.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1800))
                lvwSearchDetails.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(700))
                lvwSearchDetails.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(800))
                'DC030106 PN26062 set extra broking columns to zero for underwriting
                lvwSearchDetails.Columns.Item(7).Width = CInt(0)
                lvwSearchDetails.Columns.Item(8).Width = CInt(0)

            Else

                ' Set the column widths for the search list.
                lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(2250))
                lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2250))
                lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(2250))
                lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(2100))
                lvwSearchDetails.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(900))
                lvwSearchDetails.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1280))
                lvwSearchDetails.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(1800))
                lvwSearchDetails.Columns.Item(7).Width = CInt(VB6.TwipsToPixelsX(700))
                lvwSearchDetails.Columns.Item(8).Width = CInt(VB6.TwipsToPixelsX(800))

            End If

            ' Update the interface details with the
            ' property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DD 02/09/2003: Added default
            'eck PN10448 Set default status to live
            'cboStatus.ListIndex = 0
            For iTemp As Integer = 0 To cboStatus.Items.Count
                If VB6.GetItemString(cboStatus, iTemp) = "Live" Then
                    cboStatus.SelectedIndex = iTemp
                End If
            Next iTemp


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

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



            'GetResdata is copied locally
            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShortName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then

                cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If


            cmdStatement.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatementButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' {* USER DEFINED CODE (Begin) *}

            'DC030106 PN26062 different columns between broking and underwriting
            If m_bIsUnderwritingBranch Then



                lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Else

                'finance provider


                lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle8, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'policy number


                lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'plan refernce


                lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle9, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(7).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                lvwSearchDetails.Columns.Item(8).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




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

            result = gPMConstants.PMEReturnCode.PMTrue
            'PN10571 eck 240204
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                cmdOK.Enabled = Not bDisable
            End If
            'If we're here we're searching.  Disable it until an item is clicked.
            cmdEdit.Enabled = False

            Return result

        Catch excep As System.Exception




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
            _stbStatus_Panel1.Text = " " & sMessage

        Catch excep As System.Exception




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
                lItemsFound = m_vSearchData.GetUpperBound(1) + 1
            End If
            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub



    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1560)
            lvwSearchDetails.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(3100)

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1150)

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1150)

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1150)

            cmdNew.Top = Me.Height - VB6.TwipsToPixelsY(1150)
            cmdEdit.Top = Me.Height - VB6.TwipsToPixelsY(1150)
            cmdStatement.Top = Me.Height - VB6.TwipsToPixelsY(1150)
            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1335)

            If cmdNavigate.Visible Then
                cmdNavigate.Top = Me.Height - VB6.TwipsToPixelsY(1150)
            End If

            Return result

        Catch





            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ProcessFinancePlanInterface(Private)
    '
    ' Description: Calls the Finance Plan Interface
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ProcessFinancePlanInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ProcessFinancePlanInterface() As Integer
    '
    '
    '
    'Dim result As Integer = 0
    'Try 
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Finance Plan.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFinancePlanInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    '
    'Return result
    '
    'End Try
    'End Function


    Private Sub cboStatus_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboStatus.SelectedIndexChanged
        m_lReturn = CType(RebuildList(), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        RebuildList()
    End Sub

    ' PRIVATE Methods (End)
    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        'developer guide no. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(objCnt:=cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If

    End Sub

    Private Sub cmdStatement_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdStatement.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdStatement_Click
        ' PURPOSE: Print the Instalments report for the selected plan
        ' AUTHOR: Paul Cunningham
        ' DATE: 31 March 2003, 10:27:32
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Const ACMethod As String = "cmdStatement_Click"

        Dim oReport As Object
        Dim lRowID As Integer

        Const ksReportGroupCode As String = "PFSTATE"


        Try

            oReport = CreateLateBoundObject("iPMBReportPrint.Interface_Renamed")

            With oReport

                m_lErrorNumber = .Initialise()
                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to initialise report component")
                End If

                .CallingAppName = "iACTFindCashList.Interface"

                'Ensure the PMView is set so we enter in read only mode
                m_lErrorNumber = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to set process modes for report component")
                End If

                ' Get cnt and version

                lRowID = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
                'DC030106 PN26062 changed label from ACIPlanReference
                m_lFinancePlanCnt = CInt(m_vSearchData(ACIPlanRefCnt, lRowID))
                m_lFinancePlanVersion = CInt(m_vSearchData(ACIPlanVersion, lRowID))

                'Set up the prompts
                Dim vKeyPrompts(1, 1) As Object

                vKeyPrompts(0, 0) = "dtStartDate"

                vKeyPrompts(1, 0) = "Start Date"

                vKeyPrompts(0, 1) = "dtEndDate"

                vKeyPrompts(1, 1) = "End Date"


                'Set the start up options
                Dim vKeys(1, 7) As Object
                'Set the name of parameter1

                vKeys(0, 0) = PMNavKeyConst.PMKeyNameParam1Name

                vKeys(1, 0) = "lPFPremFinanceCnt"
                'set the value of parameter1

                vKeys(0, 1) = "lPFPremFinanceCnt"

                vKeys(1, 1) = 1 'r_lCashListId


                vKeys(0, 2) = PMNavKeyConst.PMKeyNameParam2Name

                vKeys(1, 2) = "lPFPremFinanceVersion"
                'set the value of parameter1

                vKeys(0, 3) = "lPFPremFinanceVersion"

                vKeys(1, 3) = 1 'r_lCashListId

                'Tell the Report Print component that we want to
                'filter the list of reports that are displayed

                vKeys(0, 4) = "filter_reports"
                'set the filter name to 'report_group'

                vKeys(1, 4) = "report_group"
                'set the filter value for the above filter to csFindCashListReportGroup

                vKeys(0, 5) = "report_group"

                vKeys(1, 5) = ksReportGroupCode
                'set so the params we pass don't get trashed

                vKeys(0, 6) = "save_params"
                'vKeys(1, 6) 'NOT USED

                vKeys(0, 7) = PMNavKeyConst.PMKeyNameKeyPrompts

                vKeys(1, 7) = vKeyPrompts

                '@dtStartDate DATETIME = NULL,
                '@dtEndDate DATETIME

                m_lErrorNumber = .SetKeys(vKeys)
                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to set keys for report component")
                End If

                m_lErrorNumber = .Start()
                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to start processing for report component")
                End If

            End With

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    Exit Sub

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub

            End Select

        Finally
            If Not (oReport Is Nothing) Then
                oReport.Dispose()
                oReport = Nothing
            End If



        End Try
        Exit Sub
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBFindFinancePlan.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get bPMLock
            Dim temp_m_oPMLock As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                'Initialise = PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'SJ 26/02/2004 - start
            m_lReturn = CType(CheckForUnderwritingBranch(v_iSourceId:=g_oObjectManager.SourceID, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
                Exit Sub
            End If
            'SJ 26/02/2004 - end

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

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
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

#If CodeBase = 18 Then
            'DD 27/05/2003: Not available on 1.8.x
            cmdStatement.Visible = False
#End If

            '2005 Client Manager Security - Only prevent new plans from being created IF a) we came via Client
            'Manager b) Client Manager Security is on c) User has not got the authority to add/edit plans PN24921
            If m_sCallingAppName = "ClientManager" Then
                If Not g_bEditFinancePlanAuthority Then
                    cmdNew.Enabled = False
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
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                'Developer Guide No 7
                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    '    ' Do not procced with the interface termination.
                    eventArgs.Cancel = True

                    '    ' Set the mouse pointer to normal.
                    '    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    '    Exit Sub
                End If

                ' CJB 130904 PN14921
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            End If


            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Destroy the instance of the lock object
            ' from memory.
            If Not (m_oPMLock Is Nothing) Then

                m_oPMLock = Nothing
            End If

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

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)

        Catch




            Exit Sub
        End Try


    End Sub


    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.

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





            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Me.Hide()
            End If

        Catch excep As System.Exception




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

            Me.Hide()

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

        Dim sPartyType As String = ""
        ' Click event of the New Button.

        Try

            ' {* USER DEFINED CODE (Begin) *}
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lFinancePlanCnt = 0
            m_lFinancePlanVersion = 0
            Me.Hide()
            ' {* USER DEFINED CODE (End) *}

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim lRowID As Integer

        ' Click event of the Edit Button.

        Try

            ' {* USER DEFINED CODE (Begin) *}

            'If no items selected then exit procedure.
            If lvwSearchDetails.FocusedItem Is Nothing Then
                Exit Sub
            End If

            ' Get id of the row that has been selected for an edit

            lRowID = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)

            ' Get cnt and version
            'DC030106 PN26062 - relablled from ACIPlanReference
            m_lFinancePlanCnt = CInt(m_vSearchData(ACIPlanRefCnt, lRowID))
            m_lFinancePlanVersion = CInt(m_vSearchData(ACIPlanVersion, lRowID))

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            '2005 Client Manager Security  Add Extra Key
            Dim oMaint As Object
            Dim vKeyArray(1, 5) As Object


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClientCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClientName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_sLongName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lFinancePlanVersion

            ' We only restrict what can be edited in the finance plan (apart from fields that are
            ' restricted anyway) if we cam via Client Manager, client manager security has been
            ' turned on and Edit Finance Plan authority has been disallowed for the current user
            ' PN24604

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMKeyNameFinancePlanEditAuthority
            If m_sCallingAppName = "ClientManager" Then

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = g_bEditFinancePlanAuthority
            Else

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = True
            End If

            oMaint = CreateLateBoundObject("iPMBFinancePlanMaint.Interface_Renamed")
            m_lReturn = CType(oMaint, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If CStr(m_vSearchData(ACIStatus, Convert.ToString(lvwSearchDetails.FocusedItem.Tag))) <> bSIRPremFinConst.PFStatusIndLive AndAlso
                CStr(m_vSearchData(ACIStatus, Convert.ToString(lvwSearchDetails.FocusedItem.Tag))) <> bSIRPremFinConst.PFStatusIndOnHold AndAlso
                CStr(m_vSearchData(ACIStatus, Convert.ToString(lvwSearchDetails.FocusedItem.Tag))) <> bSIRPremFinConst.PFStatusIndCompleted Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            Else
                m_iTask = gPMConstants.PMEComponentAction.PMEdit
            End If

            m_lReturn = oMaint.SetProcessModes(vTask:=m_iTask, vEffectiveDate:=DateTime.Now)

            oMaint.CallingAppName = ACApp

            m_lReturn = oMaint.SetKeys(vKeyArray)
            m_lReturn = oMaint.Start()
            oMaint.Dispose()
            m_lReturn = CType(RebuildList(), gPMConstants.PMEReturnCode)
            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        Dim sStatus As String = ""
        sStatus = Convert.ToString(lvwSearchDetails.SelectedItems(0).SubItems(7).Text)
        If lvwSearchDetails.Items.Count > 0 AndAlso (sStatus = "Live" OrElse sStatus = "On Hold" OrElse sStatus.ToUpper = "COMPLETED") Then
            VB6.SetDefault(cmdOK, True)
            cmdEdit.Text = "Edit"
            cmdStatement.Enabled = True
        Else
            VB6.SetDefault(cmdCancel, True)
            cmdEdit.Text = "View"
            cmdStatement.Enabled = False
        End If
        cmdEdit.Enabled = True

    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        If lvwSearchDetails.Items.Count > 0 Then
            If Not (lvwSearchDetails.FocusedItem Is Nothing) Then
                If m_sCallingAppName = "ClientManager" Then
                    cmdEdit_Click(cmdEdit, New EventArgs())
                Else
                    cmdOK_Click(cmdOK, New EventArgs())
                End If
            End If
        End If

        Exit Sub




        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)
        _hScollValue = GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
        ' Column click event for the search details

        Try
            ListViewFunc.SortListView(lvwSearchDetails, eventArgs)
            RecoverHorizontalScroll()
        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    <DllImport("user32.dll")>
    Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As System.IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")>
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwSearchDetails.Handle)
        Dim dx As Integer = _hScollValue - GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
        Dim b As Boolean = SendMessage(lvwSearchDetails.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)
    End Sub
    Private Sub cmdPartyFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPartyFind.Click

        Dim sName As String
        Dim lReturn As Integer

        Try

            sName = txtPartyCode.Text.Trim()

            ' Process the find party.
            lReturn = ProcessFindParty()

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Find Party.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPartyFind_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: ProcessFindParty
    '
    ' Description: Process the Party lookup component.
    '
    ' ***************************************************************** '

    Private Function ProcessFindParty() As Integer
        Dim result As Integer = 0
        Dim sCompanyNo As String = ""
        Dim oFindParty As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create Find Party object
            Dim temp_oFindParty As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                result = gPMConstants.PMEReturnCode.PMFalse
                oFindParty = Nothing
                Return result
            End If

            With oFindParty
                ' Set the process modes.

                m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' carry on - let FindParty use defaults
                End If

                ' Set the properties.


                .CallingAppName = m_sCallingAppName

                .ShortName = txtPartyCode.Text.Trim()

                .AllowAgentSearch = True

                m_lReturn = .Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Retrieve Party properties

                If .Status = gPMConstants.PMEReturnCode.PMOK Then

                    m_lPartyCnt = .PartyCnt

                    txtPartyCode.Text = .ShortName.Trim()

                    m_lReturn = CType(RebuildList(), gPMConstants.PMEReturnCode)

                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End With



            ' Destroy Find Party object

            oFindParty.Dispose()
            oFindParty = Nothing

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Find Party.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFindParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function RebuildList() As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMFalse
        Try

            cmdStatement.Enabled = False

            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_lReturn

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RebuildList failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
