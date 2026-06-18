Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Partial Friend Class frmList
    Inherits System.Windows.Forms.Form
    Private Sub frmList_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 02nd October 2002
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '
    'developer guide no.69
    Dim frmUsers As frmUsers
    Dim frmBanking As frmBanking

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmList"
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    'Now OK to use PUBLIC variable instead of Property (as long as no validation, read only, etc)
    Public CompanyID As Integer
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTCashList.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Form control
    Private m_oFormfields As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores private details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private m_oSelectedItem As ListViewItem

    ' Stores the List data from the business object.
    'developer guide no.33
    Private m_vResultArray As Object

    Private m_lNewCashListId As Integer

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PRIVATE Const Members (Begin)
    ' {* USER DEFINED CODE (Begin) *}

    'Result Array columns for CashListDrawer (ARRAY and LIST)
    Private Const ACCashDrawer As Integer = 0
    Private Const ACCashDrawerId As Integer = 1
    Private Const ACCashListId As Integer = 2
    Private Const ACCashDrawerOpen As Integer = 3
    Private Const ACCashDrawerMultiUser As Integer = 4
    Private Const ACCashDrawerBankAccountId As Integer = 5
    Private Const ACCashDrawerCashFloatAmount As Integer = 6
    Private Const ACCashDrawerAutoClose As Integer = 7
    Private Const ACCashDrawerCompanyID As Integer = 8
    Private Const ACCashDrawerSubBranchID As Integer = 9

    'The key name used to lock a cash drawer
    Private Const ksLockKeyName As String = "CashDrawer"

    'Indiactes no record returned from business / db call
    Private Const klNoRecord As Integer = 0

    'Constants to indicate whether a drawer is open (created) or closed
    Private Const ksListBoolTrue As String = "Yes"
    Private Const ksListBoolFalse As String = "No"

    'PSL 15/04/2003 Screen mode if you are using form just to select a drawer
    Private m_sScreenMode As String = ""
    Private m_lCashlistID As Integer

    Public Property ScreenMode() As String
        Get
            Return m_sScreenMode
        End Get
        Set(ByVal Value As String)
            m_sScreenMode = Value
        End Set
    End Property

    'SMJB CQ1966 06/08/03 Need to make business object available so we can check cash list status
    'from Interface.ProcessInterface()
    Public ReadOnly Property Business() As Object
        Get
            Return m_oBusiness
        End Get
    End Property
    'PSL 15/04/2003 Need to keep the cashlistid if using form to select
    Public Property CashlistID() As Integer
        Get

            ' Return the Cash List ID
            Return m_lCashlistID

        End Get
        Set(ByVal Value As Integer)

            ' Set the Cash List ID
            m_lCashlistID = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Const Members (End)

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
    'PSL 15/04/2003 Changed this to public for code where it defaults
    'to the only cashlistdrawer if there is only one
    'Set in process interface
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
    ' Name: GetCompany (Standard Method)
    '
    ' Description: Gets valid Source ID's  and if nessessary displays selection
    '
    ' ***************************************************************** '
    Public Function GetCompany(ByRef m_iCompanyID As Integer) As Integer
        Dim result As Integer = 0

        Dim m_oBranch As iPMBBranch.Interface_Renamed
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oBranch As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oBranch = temp_m_oBranch

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBranch.GetSource(iSourceID:=m_iCompanyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBranch.Dispose()
            m_oBranch = Nothing
            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Company", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetAllUserCashListDrawer(v_lUserId:=g_iUserID, r_vResultArray:=m_vResultArray)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

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

            'Poulate the listview with the results
            PopulateList()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ChkCreateCashList
    '
    ' Description: Chk/Create a cashlist.Optionally return a CashList ID.
    '
    ' Author    : Kevin Grandison
    ' Date      : 30/06/2003
    '
    '
    ' ***************************************************************** '
    Public Function ChkCreateCashList(ByVal v_bFirstItem As Boolean, Optional ByRef lCashlistID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' KG 30/06/03 - Check a selection made
            'Only process if there is a selection made
            If v_bFirstItem Then
                m_oSelectedItem = lvwCashListDrawers.Items.Item(0)
            Else
                m_oSelectedItem = lvwCashListDrawers.FocusedItem
                If m_oSelectedItem Is Nothing Then
                    'Display standard message
                    DisplayMessage(ACNoSelectionTitle, ACNoSelectionDetails, MsgBoxStyle.Exclamation)
                    Return result
                End If
            End If

            m_lCashlistID = CInt(ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashListId).Text)
            '   m_lCashlistID = CLng(lvwCashListDrawers.SelectedItem.SubItems(ACCashListId))

            ' KG 30/06/03 - Create, if a Cashlist has not been created
            If m_lCashlistID = 0 Then
                'Update the business from the interface.
                m_lReturn = InterfaceToBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update business.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'm_lNewCashListId will have been set to the Id of the new record
                m_lCashlistID = m_lNewCashListId
            End If

            ' Rtn Cashlist Id
            lCashlistID = m_lCashlistID

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to chk/create CashList", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkCreateCashList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the business object.


            m_lReturn = m_oBusiness.DirectAdd(vCashListID:=m_lNewCashListId, vCashliststatusID:=0, vCashListTypeID:=2, vCashlistRef:="", vCompanyID:=ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerCompanyID).Text, vBankAccountID:=ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerBankAccountId).Text, vCurrencyID:=g_iCurrencyID, vListDate:=DateTime.Now, vControlTotal:=0, vItemCount:=0, vCashlist_drawer_id:=ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerId).Text, vPMUser_id:=g_iUserID, vCash_Float_Amount:=ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerCashFloatAmount).Text, vSubBranchID:=ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerSubBranchID).Text)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'End If
    '
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

    'ctlLookup.ListIndex = 0
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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
            m_lReturn = iPMForms.DisplayCaptions(Me)

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
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set up interface defaults.
            '
            ' Example:-
            '
            '    cmdOK.Default = True
            '    uctType.ListIndex = 0
            '    txtDate.Text = FormatField( _
            ''                        iFormatType:=PMFormatDateLong, _
            ''                        vFieldValue:=Now)
            '
            '   'Setup default data for Add
            '   If (m_iTask% = PMAdd) Then
            '       cmdPopulate.Enabled = True
            '       uctType.ListIndex = 0
            '   Else
            '       uctType.ListIndex = 1
            '   End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdBanking_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBanking.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdBanking_Click
        ' PURPOSE: Process the banking request for the selected drawer
        ' AUTHOR: Paul Cunnigham
        ' DATE: 18 October 2002, 12:29:09
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim bMultiUser As Boolean
        Dim lCashDrawerId, lCashlistID As Integer
        Dim sLockKeyName As String = ""
        Dim vResultArray(,) As Object
        Dim sCurrentlyLockedBy As String = ""
        Dim bAuthorised As Boolean


        Try

            'Only process if there is a selection made
            m_oSelectedItem = lvwCashListDrawers.FocusedItem
            If m_oSelectedItem Is Nothing Then
                'Display standard message
                DisplayMessage(ACNoSelectionTitle, ACNoSelectionDetails, MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'Has the Cash Drawer been created
            lCashlistID = CInt(ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashListId).Text)
            If lCashlistID = 0 Then
                'Display standard message
                DisplayMessage(ACNotCreatedTitle, ACNotCreatedDetails, MsgBoxStyle.Exclamation)
                Exit Sub
            End If



            'Determine if user can process banking for this cash drawer
            lCashDrawerId = CInt(ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerId).Text)

            m_lReturn = m_oBusiness.GetUserBankingAuthorisation(v_lUserId:=g_iUserID, v_lCashDrawerId:=lCashDrawerId, r_bAuthorised:=bAuthorised)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBanking")

                Exit Sub
            End If

            If Not bAuthorised Then
                'Display standard message
                DisplayMessage(ACNotAuthorisedBankingTitle, ACNotAuthorisedBankingDetails, MsgBoxStyle.Critical)
                Exit Sub
            End If


            'Convert the "Yes" / "No" MulitUser to a real True / False value
            ConvertListBoolToBool(r_sValue:=ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerMultiUser).Text, r_bResult:=bMultiUser)

            'Set the KeyName for the lock
            If bMultiUser Then
                sLockKeyName = ksLockKeyName & "|%"
            Else
                sLockKeyName = ksLockKeyName
            End If

            'Ensure that the drawer is not opened by anyone
            '(Test for the exsitance of a lock)
            'If the lock already exists, sCurrentlyLockedBy will tell us by who

            m_lReturn = m_oBusiness.GetLocks(v_sLockName:=sLockKeyName, v_lLockValue:=lCashlistID, r_vResultArray:=vResultArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to obtain the lock
                Exit Sub
            End If

            'If drawer is locked then vResultArray will contain data
            If gArrays.IsArrayDimensioned(vResultArray) = gPMConstants.PMEReturnCode.PMTrue Then
                With frmUsers
                    'Initialise the form

                    'devloper guide no.24
                    .Business = m_oBusiness
                    .General = m_oGeneral
                    .m_sLockName = sLockKeyName


                    .m_vResultArray = vResultArray
                    .m_lCashlistID = lCashlistID
                    .m_sDrawerName = m_oSelectedItem.Text
                    .PopulateList()
                    .ShowDialog()

                    'Release the memory
                    frmUsers = Nothing

                    vResultArray = Nothing

                    Exit Sub
                End With
            End If

            'Apply a single-user lock to the cash drawer
            '(Even multi-user cash drawers are locked as single-user when banking)
            m_lReturn = ObtainLock(r_sKeyName:=ksLockKeyName, r_lKeyValue:=lCashlistID, r_sCurrentlyLockedBy:=sCurrentlyLockedBy)

            ' Check for errors.
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                ' Failed to obtain the lock
                Exit Sub
            End If

            'If drawer is locked then (somebody snuck in so better handle this)
            If sCurrentlyLockedBy.Length Then
                'Already locked so display message to user
                DisplayMessage(ACDrawerLockedTitle, ACDrawerLockedDetails, MsgBoxStyle.Exclamation, "user", sCurrentlyLockedBy) 'Key / Value pair used to replace tokens

                Exit Sub
            End If

            frmBanking.CashlistID = lCashlistID

            'sw 23/12/2002, pass reference to business object,


            'developer guide no.24
            frmBanking.Business = m_oBusiness

            ' KG 16/06/03 -  Pass Sub Branch ID
            frmBanking.SubBranchID = CInt(ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerSubBranchID).Text)

            'Show banking form (this is currently a dummy form)
            frmBanking.ShowDialog() 'over to Paul Harris to do his stuff

            frmBanking = Nothing

            'Release the lock on the drawer
            m_lReturn = ReleaseLock(r_sKeyName:=ksLockKeyName, r_lKeyValue:=lCashlistID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to release the lock
                Exit Sub
            End If

            'Release the lock on the drawer
            m_lReturn = GetBusiness()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get business
                Exit Sub
            End If

            m_lReturn = PopulateList()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to populate list
                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdBanking_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        ' Click event of the Close button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Close command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClose_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOpen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOpen.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdOpen_Click
        ' PURPOSE: Open (create) a CashList record
        ' AUTHOR: Paul Cunnigham
        ' DATE: 15 October 2002, 12:07:48
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim lCashDrawerId, lCashlistID As Integer
        Dim bMultiUser As Boolean 'Whether a CashList is multi user
        Dim bCreate As Boolean 'Whether a CashList needs to be created
        Dim sCurrentlyLockedBy As String = ""
        Dim vResultArray(,) As Object
        Dim sStatusCode As String = ""

        Const cPendingBanking As String = "B"
        Const cStatusClosed As String = "C"


        Try

            'PSL 15/04/2003 is Form just used to select a cashlistdrawer
            If m_sScreenMode = "SELECT" Then

                ' KG 30/06/03 - Chk/Create a cashlist
                m_lReturn = ChkCreateCashList(False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'fail
                    Exit Sub
                End If

                'Single user check required - drawer may be in the banking process
                'If the lock already exists, sCurrentlyLockedBy will tell us by who
                m_lReturn = CheckLock()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Me.Hide()
                Else

                    m_lReturn = m_oBusiness.GetCashListStatusCode(m_lCashlistID, sStatusCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                        Me.Hide()
                    End If

                    If sStatusCode = cPendingBanking Then
                        MessageBox.Show("This cash drawer is currently in Banking, receipts cannot be added until Banking is complete.", "Cash Drawer Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Else
                        m_lStatus = gPMConstants.PMEReturnCode.PMOK
                        Me.Hide()
                    End If


                End If
            Else
                'Only process if there is a selection made
                m_oSelectedItem = lvwCashListDrawers.FocusedItem
                If m_oSelectedItem Is Nothing Then
                    'Display standard message
                    DisplayMessage(ACNoSelectionTitle, ACNoSelectionDetails, MsgBoxStyle.Exclamation)
                    Exit Sub
                End If

                'A selection is made so determine if the CashList has
                'been created for the selected CashListDrawer
                lCashDrawerId = CInt(ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerId).Text)
                lCashlistID = CInt(ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashListId).Text)

                'sw front office receipting. 04-12-2002
                'first check the status of the cash list


                m_lReturn = m_oBusiness.GetCashListStatusCode(lCashlistID, sStatusCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'fail
                    Exit Sub
                End If

                If sStatusCode = cPendingBanking Then
                    MessageBox.Show("This cash drawer is currently in Banking, receipts cannot be added until Banking is complete.", "Cash Drawer Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                ElseIf sStatusCode = cStatusClosed Then
                    'set this to zero as we wish to force the opening of a new cash list
                    lCashlistID = 0
                End If

                'end sw

                'Set flags based on selected item - these will drive the process
                bCreate = (lCashlistID = klNoRecord)
                'Convert the "Yes" / "No" to a real True / False value
                ConvertListBoolToBool(r_sValue:=ListViewHelper.GetListViewSubItem(m_oSelectedItem, ACCashDrawerMultiUser).Text, r_bResult:=bMultiUser)

                'Process the 'Opening' of the CashList
                If Not bMultiUser Then
                    'Single User CashList
                    'Create it if needs be and display it
                    m_lReturn = ProcessCashList(r_lCashListId:=lCashlistID, r_sLockKey:=ksLockKeyName, r_bCreate:=bCreate, r_bDisplay:=True)
                Else
                    'Multi User CashList
                    'Has it already been created?
                    If bCreate Then
                        'First apply a single-user lock and create the CashList
                        '(don't display it)
                        m_lReturn = ProcessCashList(r_lCashListId:=lCashlistID, r_sLockKey:=ksLockKeyName & "|" & CStr(g_iUserID), r_bCreate:=True, r_bDisplay:=True)

                        'SW ClearQuest 1040 14/05/2003
                        'added else as cash list was getting shown twice, this code always worked b4
                        'someone must have bust it. fixed now.
                    Else
                        'SMJB CQ2811 Even though this is a multi-user drawer, we must still check for a single
                        'lock, as this will be applied if it is in banking

                        m_lReturn = m_oBusiness.GetLocks(v_sLockName:=ksLockKeyName, v_lLockValue:=lCashlistID, r_vResultArray:=vResultArray)

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to obtain the lock
                            Exit Sub
                        End If

                        'Check the lock status
                        If gArrays.IsArrayDimensioned(vResultArray) Then
                            'Already locked so display message to user
                            DisplayMessage(ACDrawerSingleUserLockedTitle, ACDrawerSingleUserLockedDetails, MsgBoxStyle.Exclamation, "user", vResultArray(0, 0)) 'Key / Value pair used to replace tokens
                            Exit Sub
                        End If


                        'Now check that the current user doesn't already have the cash drawer open

                        m_lReturn = m_oBusiness.GetLocks(v_sLockName:=ksLockKeyName & "|" & CStr(g_iUserID), v_lLockValue:=lCashlistID, r_vResultArray:=vResultArray)

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to obtain the lock
                            Exit Sub
                        End If

                        'SW ClearQuest 1040 14/05/2003
                        'If drawer is locked by the current user then vResultArray will contain data
                        If gArrays.IsArrayDimensioned(vResultArray) Then
                            'Already locked so display message to user
                            DisplayMessage(ACDrawerLockedTitle, ACDrawerLockedDetails, MsgBoxStyle.Exclamation, "user", vResultArray(0, 0)) 'Key / Value pair used to replace tokens
                            Exit Sub
                        End If

                        'Now apply a multi-user lock and display the CashList
                        m_lReturn = ProcessCashList(r_lCashListId:=lCashlistID, r_sLockKey:=ksLockKeyName & "|" & CStr(g_iUserID), r_bCreate:=False, r_bDisplay:=True)
                    End If

                End If

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update business.
                    Exit Sub
                End If

                'Refresh the list if the Drawer was created
                If bCreate Then
                    ' Get the interface details from the
                    ' business object.
                    m_lReturn = GetBusiness()

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to get the details.
                        Exit Sub
                    End If

                    'Now populate the list
                    PopulateList()

                    'Set lvwCashListDrawers.SelectedItem = m_oSelectedItem
                End If

            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOpen_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally



        End Try
        Exit Sub
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCashList.Form", vInstanceManager:="ClientManager")
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
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTCashList.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

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


    Private Sub frmList_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

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

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the rules for validating fields.

            m_lReturn = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)

            'Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}


            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            'PSL 115/04/2003
            If m_sScreenMode = "SELECT" Then
                cmdBanking.Visible = False
            End If

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

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmList_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'developer guide no.19
            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

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

            ' Terminate the business object

            m_oBusiness.Dispose()

            

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing


            m_oFormfields.Dispose()

            m_oFormfields = Nothing

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

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd
    ' Get all of the lookup values.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView
    ' Get lookup values for viewing only.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: PopulateList
    '
    ' Description: Populates the listview with data.
    '
    ' ***************************************************************** '
    Private Function PopulateList() As Integer

        Dim result As Integer = 0
        Dim lLower, lUpper As Integer
        Dim sOpen, sMultiUser As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the existing items
            lvwCashListDrawers.Items.Clear()

            'Get array limits (returns false if not dimensioned)
            If gArrays.GetArrayBounds(r_vArray:=m_vResultArray, r_lDimension:=gArrays.klRowDimension, r_lLower:=lLower, r_lUpper:=lUpper) Then

                'Turn off listview updating
                ListView6Func.ListViewBatchStart(lvwCashListDrawers)

                With lvwCashListDrawers.Items
                    'Loop through the results array and populate the listview
                    For lRow As Integer = lLower To lUpper
                        'Set drawer open or closed
                        ConvertBoolToListBool(r_vValue:=CByte(m_vResultArray(ACCashDrawerOpen, lRow)), r_sResult:=sOpen)

                        'Set multiuser
                        ConvertBoolToListBool(r_vValue:=CByte(m_vResultArray(ACCashDrawerMultiUser, lRow)), r_sResult:=sMultiUser)

                        'ADD a new listitem to the listview
                        AddOrEditListViewItem(r_oListItem:=Nothing, r_sDescription:=m_vResultArray(ACCashDrawer, lRow), r_lDrawerId:=CInt(m_vResultArray(ACCashDrawerId, lRow)), r_lCashListId:=CInt(Conversion.Val(m_vResultArray(ACCashListId, lRow))), r_sOpen:=sOpen, r_sMultiUser:=sMultiUser, r_lBankAccountId:=CInt(m_vResultArray(ACCashDrawerBankAccountId, lRow)), r_cCashFloatAmount:=CDec(m_vResultArray(ACCashDrawerCashFloatAmount, lRow)), r_lAutoClose:=CInt(m_vResultArray(ACCashDrawerAutoClose, lRow)), r_lCompanyID:=CInt(m_vResultArray(ACCashDrawerCompanyID, lRow)), r_lSubBranchID:=CInt(m_vResultArray(ACCashDrawerSubBranchID, lRow)))
                    Next lRow
                End With
                'Turn on listview updating
                ListView6Func.ListViewBatchEnd()

                If lvwCashListDrawers.Items.Count = 1 Then
                    lvwCashListDrawers.FocusedItem = lvwCashListDrawers.Items.Item(0)
                    'developer guide no.185
                    lvwCashListDrawers_ItemClick(lvwCashListDrawers.SelectedItems.Item(0))
                Else
                    lvwCashListDrawers.FocusedItem = Nothing
                End If

                'Clear the data from the array as it's now stored in the listview
                m_vResultArray = Nothing

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate the list", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'Turn on listview updating
            ListView6Func.ListViewBatchEnd()

            Return result

        End Try
    End Function

    Private Function AddOrEditListViewItem(ByRef r_oListItem As ListViewItem, ByRef r_sDescription As String, ByRef r_lDrawerId As Integer, ByRef r_lCashListId As Integer, ByRef r_sOpen As String, ByRef r_sMultiUser As String, ByRef r_lBankAccountId As Integer, ByRef r_cCashFloatAmount As Decimal, ByRef r_lAutoClose As Integer, ByRef r_lCompanyID As Integer, ByRef r_lSubBranchID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddOrEditListViewItem
        ' PURPOSE: Adds a new ListItem to the ListView or edits an exitsing one
        ' AUTHOR: Paul Cunningham
        ' DATE: 15 October 2002, 14:05:03
        ' RETURNS: PMTrue for success
        ' CHANGES: KG 12/06/03 - CompanyID and SubBranchID
        ' ---------------------------------------------------------------------------


        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If a reference to a listitem is not passed we need to create a new one
            If r_oListItem Is Nothing Then
                'Add the new item
                r_oListItem = lvwCashListDrawers.Items.Add(r_sDescription)
            Else
                'Edit the existing item
                r_oListItem.Text = r_sDescription
            End If

            'Populate the subitems
            With r_oListItem
                ' Save the key so that it is easier to delete an item
                '06/11/2002 - PWC - Use the CashListId
                .Name = "Key" & r_lDrawerId & "*" & CStr(r_lCashListId)
                ListViewHelper.GetListViewSubItem(r_oListItem, ACCashDrawerId).Text = CStr(r_lDrawerId)
                ListViewHelper.GetListViewSubItem(r_oListItem, ACCashListId).Text = CStr(r_lCashListId)
                ListViewHelper.GetListViewSubItem(r_oListItem, ACCashDrawerOpen).Text = r_sOpen
                ListViewHelper.GetListViewSubItem(r_oListItem, ACCashDrawerMultiUser).Text = r_sMultiUser
                ListViewHelper.GetListViewSubItem(r_oListItem, ACCashDrawerBankAccountId).Text = CStr(r_lBankAccountId)
                ListViewHelper.GetListViewSubItem(r_oListItem, ACCashDrawerCashFloatAmount).Text = CStr(r_cCashFloatAmount)
                ListViewHelper.GetListViewSubItem(r_oListItem, ACCashDrawerAutoClose).Text = CStr(r_lAutoClose)
                ListViewHelper.GetListViewSubItem(r_oListItem, ACCashDrawerCompanyID).Text = CStr(r_lCompanyID)
                ListViewHelper.GetListViewSubItem(r_oListItem, ACCashDrawerSubBranchID).Text = CStr(r_lSubBranchID)
            End With





            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AddOrEditListViewItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function ObtainLock(ByRef r_sKeyName As String, ByRef r_lKeyValue As Integer, ByRef r_sCurrentlyLockedBy As String) As Integer
        Dim result As Integer = 0
        Dim bPMLock As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ObtainLock
        ' PURPOSE: Request a lock
        ' AUTHOR: Paul Cunnigham
        ' DATE: 15 October 2002, 14:45:27
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Dim oLock As bPMLock.User


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Request a reference to the business obect of the PMLock component
            Dim temp_oLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oLock, "bPMLock.User", vInstanceManager:="ClientManager")
            oLock = temp_oLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return False
            End If

            'Attempt to obtain a lock

            m_lReturn = oLock.LockKey(sKeyName:=r_sKeyName, vKeyValue:=r_lKeyValue, iUserID:=g_iUserID, sCurrentlyLockedBy:=r_sCurrentlyLockedBy)

            'Test for an error
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to obtain the requested lock", vApp:=ACApp, vClass:=ACClass, vMethod:="ObtainLock", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                result = gPMConstants.PMEReturnCode.PMFalse
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ObtainLock", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function ReleaseLock(ByRef r_sKeyName As String, ByRef r_lKeyValue As Integer) As Integer
        Dim result As Integer = 0
        Dim bPMLock As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ObtainLock
        ' PURPOSE: Release a lock
        ' AUTHOR: Paul Cunnigham
        ' DATE: 15 October 2002, 14:45:27
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Dim oLock As bPMLock.User


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Request a reference to the business obect of the PMLock component
            Dim temp_oLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oLock, "bPMLock.User", vInstanceManager:="ClientManager")
            oLock = temp_oLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return False
            End If


            m_lReturn = oLock.UnLockKey(sKeyName:=r_sKeyName, vKeyValue:=r_lKeyValue, iUserID:=g_iUserID)



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseLock", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function ProcessCashList(ByRef r_lCashListId As Integer, ByRef r_sLockKey As String, ByRef r_bCreate As Boolean, ByRef r_bDisplay As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ProcessCashList
        ' PURPOSE: Handles locking, creating and opening of a CashList
        ' AUTHOR: Paul Cunnigham
        ' DATE: 15 October 2002, 16:37:44
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        'Username of user who has CashListDrawer opened / locked
        Dim result As Integer = 0
        Dim sCurrentlyLockedBy As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create the cashlist if it's not already created
            If r_bCreate Then
                'Update the business from the interface.
                m_lReturn = InterfaceToBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update business.
                    Return False
                End If

                'm_lNewCashListId will have been set to the Id of the new record
                r_lCashListId = m_lNewCashListId
            End If

            'Attempt to obtain a lock on the cash list
            m_lReturn = ObtainLock(r_sKeyName:=r_sLockKey, r_lKeyValue:=r_lCashListId, r_sCurrentlyLockedBy:=sCurrentlyLockedBy)

            ' Check for errors.
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                ' Failed to obtain the lock
                Return False
            End If

            If sCurrentlyLockedBy.Length Then
                'Already locked so display message to user
                DisplayMessage(ACDrawerLockedTitle, ACDrawerLockedDetails, MsgBoxStyle.Exclamation, "user", sCurrentlyLockedBy) 'Key / Value pair used to replace tokens

                Return False
            End If

            'Display the CashList
            m_lReturn = DisplayCashList(r_lCashListId:=r_lCashListId)

            'Release the lock on the cash list
            m_lReturn = ReleaseLock(r_sKeyName:=r_sLockKey, r_lKeyValue:=r_lCashListId)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to release the lock
                Return False
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCashList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



        End Try
        Return result
    End Function

    Private Function ConvertBoolToListBool(ByRef r_vValue As Byte, ByRef r_sResult As String) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ConvertBoolToListBool
        ' PURPOSE: Converts a true / false value into a string equivalent
        '          (e.g. true becomes "yes")
        ' AUTHOR: Paul Cunnigham
        ' DATE: 16 October 2002, 15:46:09
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case r_vValue
                Case 0
                    r_sResult = ksListBoolFalse
                Case Else
                    r_sResult = ksListBoolTrue
            End Select



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertBoolToListBool", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function


    Private Function ConvertListBoolToBool(ByRef r_sValue As String, ByRef r_bResult As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ConvertListBoolToBool
        ' PURPOSE: Converts a string boolean value into a real boolean
        '          (e.g. "no" becomes "false")
        ' AUTHOR: Paul Cunnigham
        ' DATE: 16 October 2002, 15:46:09
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case r_sValue
                Case ksListBoolTrue
                    r_bResult = True
                Case Else
                    r_bResult = False
            End Select



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertListBoolToBool", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function DisplayCashList(ByRef r_lCashListId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayCashList
        ' PURPOSE:
        ' AUTHOR: Paul Cunnigham
        ' DATE: 17 October 2002, 16:12:11
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oCashListItem As iACTCashListItem.Interface_Renamed


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oCashListItem = New iACTCashListItem.Interface_Renamed()

            With oCashListItem

                m_lReturn = .Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                .CallingAppName = "iACTCashList.Interface"

                m_lErrorNumber = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                'DD 20/11/2002: Default to Receipts
                .CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts
                .CashlistID = r_lCashListId

                m_lErrorNumber = .Start()

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

            End With

            result = gPMConstants.PMEReturnCode.PMTrue



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCashList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


            End Select

        Finally
            oCashListItem.Dispose()
            oCashListItem = Nothing



        End Try
        Return result
    End Function



    Private Sub lvwCashListDrawers_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwCashListDrawers.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwCashListDrawers.Columns(eventArgs.Column)

        ' Column click event for the search details

        Try

            With lvwCashListDrawers

                'Change the sort key if column is date
                If ColumnHeader.Index + 1 - 1 = 3 Then
                    ListViewHelper.SetSortedProperty(lvwCashListDrawers, False)
                    If ListViewHelper.GetSortKeyProperty(lvwCashListDrawers) <> 8 Then
                        ListViewHelper.SetSortKeyProperty(lvwCashListDrawers, 8)
                        ListViewHelper.SetSortOrderProperty(lvwCashListDrawers, SortOrder.Ascending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwCashListDrawers, (ListViewHelper.GetSortOrderProperty(lvwCashListDrawers) + 1) Mod 2)
                    End If
                    ListViewHelper.SetSortedProperty(lvwCashListDrawers, True)

                    ' If current sort column header is
                    ' pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwCashListDrawers)) And ListViewHelper.GetSortedProperty(lvwCashListDrawers) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwCashListDrawers, (ListViewHelper.GetSortOrderProperty(lvwCashListDrawers) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwCashListDrawers, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwCashListDrawers, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwCashListDrawers, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwCashListDrawers, True)
                End If
            End With

        Catch excep As System.Exception


            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwCashListDrawers_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub lvwCashListDrawers_ItemClick(ByVal Item As ListViewItem)

        cmdBanking.Enabled = ListViewHelper.GetListViewSubItem(lvwCashListDrawers.Items.Item(lvwCashListDrawers.FocusedItem.Index), ACCashDrawerAutoClose).Text = gPMConstants.PMEReturnCode.PMFalse

    End Sub

    Public Function CheckLock() As Integer

        Dim result As Integer = 0
        Try

            Dim vResultArray(,) As Object

            result = gPMConstants.PMEReturnCode.PMFalse

            'Single user check required - drawer may be in the banking process
            'If the lock already exists, sCurrentlyLockedBy will tell us by who

            m_lReturn = m_oBusiness.GetLocks(v_sLockName:=ksLockKeyName, v_lLockValue:=m_lCashlistID, r_vResultArray:=vResultArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to obtain the lock
                Return result
            End If

            'If drawer is locked then vResultArray will contain data

            If gArrays.IsArrayDimensioned(vResultArray) Then
                'Already locked so display message to user
                DisplayMessage(ACDrawerLockedTitle, ACDrawerLockedDetails, MsgBoxStyle.Exclamation, "user", vResultArray(0, 0)) 'Key / Value pair used to replace tokens

                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="failed to check if Cash List Drawer is locked", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLock", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function
End Class