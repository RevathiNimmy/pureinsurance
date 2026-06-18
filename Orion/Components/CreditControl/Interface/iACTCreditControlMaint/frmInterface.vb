Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'refer developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    'refer developer guide no. 50

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 02nd October 2002
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '
    'developer guide no.7
    Public Const vbFormCode As Integer = 0

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    'Now OK to use PUBLIC variable instead of Property (as long as no validation, read only, etc)

    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

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
    Private m_oGeneral As iACTCreditControlMaint.General

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

    '' Control array to store the first and last
    '' text box controls for each tab.
    'Private m_ctlTabFirstLast() As Control

    ' Stores private details from the business object.

    ' {* USER DEFINED CODE (Begin) *}

    ' Stores the List data from the business object.
    Private m_vListData As Object
    Private m_lSelectedRow As Integer

    Private m_vResultArray(,) As Object

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PRIVATE Const Members (Begin)
    ' {* USER DEFINED CODE (Begin) *}

    'Result Array columns for CashListDrawer Security (ARRAY and LIST)
    Private Const ACCreditControlRuleID As Integer = 0
    Private Const ACCreditControlRuleDesc As Integer = 1
    Private Const ACCreditControlRuleSource As Integer = 2
    Private Const ACCreditControlRuleBusType As Integer = 3
    Private Const ACCreditControlRuleFreqID As Integer = 4
    Private Const ACCreditControlRuleFreqDesc As Integer = 5
    Private Const ACCreditControlRuleActive As Integer = 6
    Private Const ACCreditControlRuleUseEffectiveDate As Integer = 7

    Private Const ACYesChar As String = "Y"

    Private m_vTaskGroupUserGroups(,) As Object
    Private m_vTaskGroupTask(,) As Object
    Private m_vLookupTables(,) As Object
    Private m_vValidInsuranceFileStatuses(,) As Object

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

            'Get the list of rules (should send source_id really - but not coded)


            m_lReturn = m_oBusiness.GetRuleList(v_lSourceID:=0, r_vResultArray:=m_vResultArray)

            ' {* USER DEFINED CODE (End) *}

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
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    ' Use DirectAdd as we need to get the ID back

                    ' {* USER DEFINED CODE (Begin) *}

                    ' ************************************************************
                    ' Enter your code here to perform a direct add in to the database
                    '
                    ' Example:-
                    '
                    ' m_lReturn& = m_oBusiness.DirectAdd( _
                    ''                    vCashListID:=m_lCashlistID, _
                    ''                    vCashliststatusID:=m_lCashListStatusID, _
                    ''                    vCashListTypeID:=m_lCashlistTypeID)
                    '
                    ' NOTE: Replace this section with your new code.
                    ' ************************************************************

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    ' ************************************************************
                    ' Enter your code here to perform an update to the database
                    '
                    ' Example:-
                    '
                    ' m_lReturn& = m_oBusiness.EditUpdate( _
                    ''                    lRow:=lBusinessDataID&, _
                    ''                    vCashListID:=m_lCashlistID, _
                    ''                    vCashliststatusID:=m_lCashListStatusID, _
                    ''                    vCashListTypeID:=m_lCashlistTypeID)
                    '
                    ' NOTE: Replace this section with your new code.
                    ' ************************************************************

                    ' {* USER DEFINED CODE (End) *}

            End Select

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
            'developer guide no.243
            m_lReturn = iPMForms.DisplayCaptions(Me, My.Resources.ResourceManager)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set up interface defaults.

            ' ************************************************************

            'Added call to SetExtraListViewProperties to select entire row
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=Me.lvwCreditControlRules.Handle.ToInt32(), v_vShowRowSelect:=True)
            lvwCreditControlRules.FullRowSelect = True
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


            '    ' Initialise the control array with the number of
            '    ' tabs which contain data entry fields on (Remember
            '    ' that arrays start from zero, therefore you must
            '    ' subtract one from the number of tabs).
            '    ReDim m_ctlTabFirstLast(1, 0)

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
    ' Name: cboPMLookupSource_Click (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Sub cboPMLookupSource_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMLookupSource.SelectedIndexChanged
        'Populate the list
        m_lReturn = PopulateList()
        lvwCreditControlRules.FullRowSelect = True
        lvwCreditControlRules.Select()

    End Sub

    ' ***************************************************************** '
    ' Name: cmdAdd_Click (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        ' Click event of the Add Button.

        Try

            m_lReturn = ValidateBranch()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Process the Add request
            m_lReturn = DetailsFormProcessWrapper(r_iTaskType:=gPMConstants.PMEComponentAction.PMAdd)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Add button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        ' Click event of the Close button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=False)

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

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdDeleteSecurity_Click
        ' PURPOSE: Call business object to delete a Credit Control Rule
        '          record from the database
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim oListItem As ListViewItem
        Dim lDrawerUsed As Integer
        Dim vResultArray As Object


        Try

            'Only process if there is a selection made
            oListItem = lvwCreditControlRules.FocusedItem
            If oListItem Is Nothing Then
                'Display standard message
                DisplayMessage(r_lTitleId:=ACNoSelectionTitle, r_lMessageId:=ACNoSelectionDetails, r_lOptions:=MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'Ensure that delete should proceed
            m_lReturn = DisplayMessage(r_lTitleId:=ACConfirmDeleteTitle, r_lMessageId:=ACConfirmDeleteDetails, r_lOptions:=MsgBoxStyle.YesNo + MsgBoxStyle.Question)

            If m_lReturn = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If


            'refer developer guide no. 52(Latest Guide)
            m_lReturn = m_oBusiness.GetStepList(v_lCreditControlRuleId:=Conversion.Val(Me.lvwCreditControlRules.FocusedItem.SubItems(4).Text), r_vResultArray:=vResultArray) 'Check if steps exist for a rule

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the step list from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepList")

                Exit Sub
            End If

            If Not IsArray(vResultArray) Then
                'Call through to the business object to delete an item
                Dim sUniqueId As String = GetUniqueID()
                Dim sScreenHierarchy As String = $"Credit Control({cboPMLookupSource.SelectedItem}/{oListItem.Text})"
                'refer developer guide no. 52(latest Guide)
                m_lReturn = m_oBusiness.DirectDeleteRule(v_lCreditControlRuleId:=Conversion.Val(Me.lvwCreditControlRules.FocusedItem.SubItems(4).Text), v_sUniqueId:=sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    ' GetBusiness = PMFalse

                    DisplayMessage(r_lTitleId:=ACRuleUsedTitle, r_lMessageId:=ACRuleUsedDetails, r_lOptions:=MsgBoxStyle.Exclamation)

                    'Exit Function
                    Exit Sub
                End If
                lvwCreditControlRules.Items.Remove(oListItem)

            Else
                DisplayMessage(r_lTitleId:=ACRuleUsedTitle, r_lMessageId:=ACRuleUsedDetails, r_lOptions:=MsgBoxStyle.Exclamation)

                Exit Sub
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        ' Click event of the Edit Button.

        Dim oListItem As ListViewItem

        Try

            ' {* USER DEFINED CODE (Begin) *}

            'Only process if there is a selection made
            oListItem = lvwCreditControlRules.FocusedItem
            If oListItem Is Nothing Then
                'Display standard message
                DisplayMessage(r_lTitleId:=ACNoSelectionTitle, r_lMessageId:=ACNoSelectionDetails, r_lOptions:=MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'Process the edit request
            m_lReturn = DetailsFormProcessWrapper(r_iTaskType:=gPMConstants.PMEComponentAction.PMEdit)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCreditControl.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTCreditControlMaint.General()

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


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lReturn As gPMConstants.PMEReturnCode

        ' Forms load event.

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            ListViewHelper.SetSortedProperty(lvwCreditControlRules, True)

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

            m_lReturn = GetSourceCombo()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' {* USER DEFINED CODE (End) *}

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

            ' get task creation lookup data - task groups / tasks / pmusergroups
            lReturn = GetTaskCreationLookupData()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            lvwCreditControlRules.FullRowSelect = True
            lvwCreditControlRules.Select()

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

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                'Process the next set of actions depending
                'upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=False)

                'Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Do not procced with the interface termination.
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

                    'Set the mouse pointer to normal.
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

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        'Dim iCtrlDown  As Integer
        '
        'Const ACCtrlMask = 2
        '
        '    On Error GoTo Err_FormKeyDown
        '
        '    ' Set the control key value.
        '    iCtrlDown = (Shift And ACCtrlMask) > 0
        '
        '    With tabMainTab
        '        ' Check the key pressed.
        '        Select Case KeyCode
        '            Case vbKeyPageUp
        '                ' Page Up key has been pressed.
        '
        '                ' Check if the control key has also
        '                ' been pressed.
        '                If (iCtrlDown) Then
        '                    ' Display the first tab.
        '                    .Tab = 0
        '                Else
        '                    ' Check we are not on the
        '                    ' first tab.
        '                    If (.Tab > 0) Then
        '                        ' Display the previous tab.
        '                        .Tab = .Tab - 1
        '                    End If
        '                End If
        '
        '            Case vbKeyPageDown
        '                ' Page Down key has been pressed.
        '
        '                ' Check if the control key has also
        '                ' been pressed.
        '                If (iCtrlDown) Then
        '                    ' Display the last tab.
        '                    .Tab = .Tabs - 1
        '                Else
        '                    ' Check we are not on the
        '                    ' last tab.
        '                    If (.Tab < (.Tabs - 1)) Then
        '                        ' Display the next tab.
        '                        .Tab = .Tab + 1
        '                    End If
        '                End If
        '
        '            Case vbKeyHome
        '                ' Home key has been pressed.
        '
        '                ' Check if the control key has also
        '                ' been pressed.
        '                If (iCtrlDown) Then
        '                    ' Set focus the the start control on
        '                    ' the tab.
        '                    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '                         m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '                    End If
        '                End If
        '
        '            Case vbKeyEnd
        '                ' End key has been pressed.
        '
        '                ' Check if the control key has also
        '                ' been pressed.
        '                If (iCtrlDown) Then
        '                    ' Set focus the the start control on
        '                    ' the tab.
        '                    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '                         m_ctlTabFirstLast(ACControlEnd, .Tab).SetFocus
        '                    End If
        '                End If
        '        End Select
        '    End With

        'Developer Guide No 293
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If

        Exit Sub



        ' Error Section.

        Exit Sub

    End Sub


    Private Sub lvwCreditControlRules_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        cmdEdit_Click(cmdEdit, New EventArgs())
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        '    With tabMainTab
        '        ' Set the default button.
        '        If (.Tab < cmdNext.Count) Then
        '            cmdNext(.Tab).Default = True
        '        Else
        '            'cmdOK.Default = True
        '        End If
        ''
        '        ' Now I know this is crap, this goes against
        '        ' all my principles, but for some reason when
        '        ' using the mouse to select a tab the setfocus
        '        ' code below doesn't work. The cursor sticks,
        '        ' and you can't tab off. Therefore I've used
        '        ' this to get around the problem.
        '        DoEvents
        ''
        '        ' Set focus to the first control on the tab.
        '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '        End If
        '    End With
        '
        'Catch 
        '
        '
        '
        ' Error Section.
        '
        '
        'tabMainTabPreviousTab = tabMainTab.SelectedIndex
        'End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Navigate button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=False)
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

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
    Private Function PopulateList(Optional ByVal v_lSourceID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lLower, lUpper As Integer
        Dim lSourceID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the source ID to that which user wants to filter by
            If Not False Then
                If v_lSourceID > 0 Then
                    lSourceID = v_lSourceID
                Else
                    'Check if a source has yet been selected
                    If cboPMLookupSource.SelectedIndex >= 0 Then
                        lSourceID = VB6.GetItemData(cboPMLookupSource, cboPMLookupSource.SelectedIndex)
                    Else
                        lSourceID = 0
                    End If
                End If
            Else
                'Check if a source has yet been selected
                If cboPMLookupSource.SelectedIndex >= 0 Then
                    lSourceID = VB6.GetItemData(cboPMLookupSource, cboPMLookupSource.SelectedIndex)
                Else
                    lSourceID = 0
                End If
            End If

            'Clear the existing items
            lvwCreditControlRules.Items.Clear()

            'Ensure tere is data in the array

            'Get array limits
            If gArrays.GetArrayBounds(r_vArray:=m_vResultArray, r_lDimension:=gArrays.klRowDimension, r_lLower:=lLower, r_lUpper:=lUpper) Then

                'Turn off listview updating
                ' refer developer guide no. 170 (Latest solutions)
                ListViewFunc.ListViewBatchStart(lvwCreditControlRules)

                With lvwCreditControlRules.Items
                    'Loop through the results array and populate the listview
                    For lRow As Integer = lLower To lUpper

                        'Check that the source id of the row is a match
                        If CInt(m_vResultArray(ACCreditControlRuleSource, lRow)) = lSourceID Then

                            'ADD a new listitem to the listview
                            AddOrEditListViewItem(r_oListItem:=Nothing, r_sDescription:=CStr(m_vResultArray(ACCreditControlRuleDesc, lRow)), r_sBussType:=CStr(m_vResultArray(ACCreditControlRuleBusType, lRow)), r_sFrequency:=CStr(m_vResultArray(ACCreditControlRuleFreqDesc, lRow)), r_lActive:=CInt(m_vResultArray(ACCreditControlRuleActive, lRow)), r_lRuleID:=CInt(m_vResultArray(ACCreditControlRuleID, lRow)), r_lUseEffectiveDate:=CInt(m_vResultArray(ACCreditControlRuleUseEffectiveDate, lRow)))

                        End If
                    Next lRow
                End With
                'Turn on listview updating
                'refer developer guide no. 170
                ListViewFunc.ListViewBatchEnd()

                lvwCreditControlRules.FocusedItem = Nothing

                'Clear the data from the array as it's now stored in the listview

            End If

            'Reset source dropdown
            'For lRow As Integer = 0 To cboPMLookupSource.Items.Count - 1
            '	If VB6.GetItemData(cboPMLookupSource, lRow) = lSourceID Then
            '		cboPMLookupSource.SelectedIndex = lRow
            '		Exit For
            '	End If
            'Next lRow


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate the list", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'Turn on listview updating
            'refer developer guide no. 170
            ListViewFunc.ListViewBatchEnd()

            Return result

        End Try
    End Function

    Private Function AddOrEditListViewItem(ByRef r_oListItem As ListViewItem, ByRef r_sDescription As String, ByRef r_sBussType As String, ByRef r_sFrequency As String, ByRef r_lActive As Integer, ByRef r_lRuleID As Integer, ByRef r_lUseEffectiveDate As Integer) As Integer

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddOrEditListViewItem
        ' PURPOSE: Adds a new ListItem to the ListView or edits an exitsing one
        ' AUTHOR: Paul Cunningham
        ' DATE: 09 October 2002, 14:05:03
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If a reference to a listitem is not passed we need to create a new one
            If r_oListItem Is Nothing Then
                'Add the new item
                r_oListItem = lvwCreditControlRules.Items.Add(r_sDescription)
            Else
                'Edit the existing item
                r_oListItem.Text = r_sDescription
            End If

            'Populate the subitems
            With r_oListItem
                ' Save the key so that it is easier to delete an item
                .Name = "Key" & r_lRuleID
                ListViewHelper.GetListViewSubItem(r_oListItem, 1).Text = r_sBussType
                ListViewHelper.GetListViewSubItem(r_oListItem, 2).Text = r_sFrequency
                If r_lActive Then
                    ListViewHelper.GetListViewSubItem(r_oListItem, 3).Text = ACYesChar
                Else
                    ListViewHelper.GetListViewSubItem(r_oListItem, 3).Text = ""
                End If
                ListViewHelper.GetListViewSubItem(r_oListItem, 4).Text = CStr(r_lRuleID)

                If r_lUseEffectiveDate Then
                    ListViewHelper.GetListViewSubItem(r_oListItem, 5).Text = ACYesChar
                Else
                    ListViewHelper.GetListViewSubItem(r_oListItem, 5).Text = ""
                End If

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


    ' ***************************************************************** '
    ' Name: DetailsFormProcess (Standard Method)
    '
    ' Description: Display details form and Add, Edit or View
    '
    ' ***************************************************************** '
    Private Function DetailsFormProcess(ByRef r_iTaskType As Integer) As Integer

        Dim result As Integer = 0

        Try
            'developer guide no.69
            ofrmDetails = New frmDetails
            result = gPMConstants.PMEReturnCode.PMTrue

            'Pass control to form asap

            ' Pass standard details to form properties
            With ofrmDetails
                .LookupTables = VB6.CopyArray(m_vLookupTables)
                .LookupDetails = VB6.CopyArray(m_vLookupDetails)
                .TaskGroupTasks = VB6.CopyArray(m_vTaskGroupTask)
                .TaskGroupUsers = VB6.CopyArray(m_vTaskGroupUserGroups)
                .CallingAppName = m_sCallingAppName
                .Task = r_iTaskType
                .Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                .TransactionType = m_sTransactionType
                .EffectiveDate = m_dtEffectiveDate
                .ValidInsuranceFileStatuses = VB6.CopyArray(m_vValidInsuranceFileStatuses)
                ' {* USER DEFINED CODE (Begin) *)

                ' Check the task.
                Select Case (r_iTaskType)
                    Case gPMConstants.PMEComponentAction.PMEdit
                        .CreditControlRuleID = CInt(Conversion.Val(Me.lvwCreditControlRules.FocusedItem.SubItems(4).Text))
                    Case gPMConstants.PMEComponentAction.PMAdd
                        .CreditControlRuleID = 0
                        'If a source has been selected - pass it as a default to add mode
                        If cboPMLookupSource.SelectedIndex > 0 Then
                            .DefaultSourceID = VB6.GetItemData(cboPMLookupSource, cboPMLookupSource.SelectedIndex)
                        End If
                End Select
                ' {* USER DEFINED CODE (End) *}

            End With

            ' Call the Load method to setup the interface details
            Dim tempLoadForm As frmDetails = ofrmDetails

            'Show the form
            ofrmDetails.ShowDialog()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function UnloadfrmDetails() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: UnloadfrmDetails
        ' PURPOSE: Common routine to unload forms and release references
        ' AUTHOR: Paul Cunningham
        ' DATE: 11 October 2002, 12:07:05
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ofrmStep.Close()
            ofrmStep = Nothing

            ofrmDetails.Close()
            ofrmDetails = Nothing



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadfrmDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function DetailsFormProcessWrapper(ByRef r_iTaskType As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DetailsFormProcessWrapper
        ' PURPOSE: Wrapper for Add and Edit processing
        ' AUTHOR: Paul Cunningham
        ' DATE: 11 October 2002, 15:23:57
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show the Details form in Add mode
            m_lReturn = DetailsFormProcess(r_iTaskType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'Refresh the list if changes written to db
            If ofrmDetails.Status <> gPMConstants.PMEReturnCode.PMCancel Or ofrmDetails.Applied Then

                'Re-query to get updated details
                m_lReturn = GetBusiness()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                'Populate the list
                m_lReturn = PopulateList(ofrmDetails.cboPMLookupSource.ItemId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

            End If

            '		UnloadfrmDetails()



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcessWrapper", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskCreationLookupData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function GetTaskCreationLookupData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTaskCreationLookupData"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get all effective pmwrk_tasks for all effective pwrktaskgroups

            lReturn = m_oBusiness.GetALLPMWrkTaskGroupTasks(r_vResults:=m_vTaskGroupTask)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bACTCreditControl.GetALLPMWrkTaskGroupTasks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get all effective pmusergroups for all effective pwrktaskgroups

            lReturn = m_oBusiness.GetALLPMWrkTaskGroupPMUserGroups(r_vResults:=m_vTaskGroupUserGroups)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bACTCreditControl.GetALLPMWrkTaskGroupPMUserGroups Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get lookup data for task groups
            lReturn = GetLookups()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = m_oBusiness.GetInstalmentImportInsuranceFileStatuses(m_vValidInsuranceFileStatuses)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstalmentImportInsuranceFileStatuses Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: GetLookups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-06-2003 : 223
    ' ***************************************************************** '
    Private Function GetLookups() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetLookups"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim m_vLookupTables(3, 0)

            m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = ACLookupTablePMWrkTaskGroup


            If m_oBusiness.GetLookupValues(v_vLookupTables:=m_vLookupTables, r_vLookupDetails:=m_vLookupDetails) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    Private Function GetSourceCombo() As Integer
        Dim result As Integer = 0
        Dim bPMUser As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSourceCombo
        ' PURPOSE:
        ' AUTHOR: Danny Davis
        ' DATE: 02 August 2005, 03:26 PM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim m_oPMUser As bPMUser.Business
            Dim vSourceArray(,) As Object

            Dim temp_m_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMUser = temp_m_oPMUser

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMUser", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            'Only populate combo with addresses the user is authorised to access.

            m_lReturn = m_oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_oObjectManager.UserID, v_bIncludeDeletedSources:=False)

            'Clear combo.
            cboPMLookupSource.Items.Clear()

            'Populate branch combo
            If IsArray(vSourceArray) Then

                For i As Integer = 0 To vSourceArray.GetUpperBound(1)
                    'Add using branch description (3).
                    Dim cboPMLookupSource_NewIndex As Integer = -1


                    'Developer Guide No. 154
                    Dim listIndex As Integer = cboPMLookupSource.Items.Add(New VB6.ListBoxItem(vSourceArray(2, i), vSourceArray(0, i)))
                Next i

            End If


            m_oPMUser.Dispose()
            m_oPMUser = Nothing



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



        End Try
        Return result
    End Function
    Private Function ValidateBranch() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If cboPMLookupSource.SelectedIndex = -1 Then
                MessageBox.Show("Please select Branch" & Strings.Chr(9), "Invalid branch", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If cboPMLookupSource.Enabled Then
                    cboPMLookupSource.Focus()
                End If
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateBranchFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateBranch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwCreditControlRules_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwCreditControlRules.DoubleClick
        If cmdEdit.Enabled Then
            cmdEdit_Click(sender, e)
        End If
    End Sub

End Class