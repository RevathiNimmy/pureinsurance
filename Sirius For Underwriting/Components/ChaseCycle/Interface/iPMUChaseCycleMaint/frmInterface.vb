Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form


	' ***************************************************************** '
	' Form Name: frmInterface
	'
    ' Date: 01/03/2013
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '

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
    Private m_vChaseCycleStatus(,) As Object

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUChaseCycleMaint.General

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
    Private Const ACChaseCycleRuleID As Integer = 0
    Private Const ACChaseCycleRuleDesc As Integer = 1
    Private Const ACChaseCycleRuleSourceId As Integer = 2
    Private Const ACChaseCycleRuleGISDataModelID As Integer = 3

    Private Const ACChaseCycleDesc As Integer = 4
    Private Const ACChaseCycleRuleGISPropertyID As Integer = 5
    Private Const ACChaseCycleRuleGISPropertyDesc As Integer = 6
    Private Const ACChaseCycleRuleUDLStatus As Integer = 7
    Private ConstACChaseCycleRuleActive As Integer = 8
    Private Const ACChaseCycleRuleUseEffectiveDate As Integer = 9
    Private Const ACDataModelDesc As Integer = 10

    Private Const ACYesChar As String = "Y"

    Private m_vTaskGroupUserGroups(,) As Object
    Private m_vTaskGroupTask(,) As Object
    Private m_vLookupTables(,) As Object

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


                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

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
            m_lReturn = iPMForms.DisplayCaptions(Me, My.Resources.ResourceManager)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lvwChaseCycleRules.FullRowSelect = True
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
    ' Name: cboPMLookupSource_Click (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Sub cboPMLookupSource_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMLookupSource.SelectedIndexChanged
        'Populate the list
        m_lReturn = PopulateList()
        lvwChaseCycleRules.FullRowSelect = True
        lvwChaseCycleRules.Select()

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
    ' ***************************************************************** '
    ' Name: cmdClose_Click (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
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
        ' PURPOSE: Call business object to delete a Chase Cycle Rule
        '          record from the database
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim oListItem As ListViewItem
        Dim vResultArray As Object


        Try

            'Only process if there is a selection made
            oListItem = lvwChaseCycleRules.FocusedItem
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

            m_lReturn = m_oBusiness.GetStepList(v_lChaseCycleRuleId:=Conversion.Val(Me.lvwChaseCycleRules.FocusedItem.SubItems(2).Text), r_vResultArray:=vResultArray) 'Check if steps exist for a rule

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the step list from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepList")

                Exit Sub
            End If

            If Not Information.IsArray(vResultArray) Then
                'Call through to the business object to delete an item

                m_lReturn = m_oBusiness.DirectDeleteRule(v_lChaseCycleRuleId:=Conversion.Val(Me.lvwChaseCycleRules.FocusedItem.SubItems(2).Text))

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    ' GetBusiness = PMFalse

                    DisplayMessage(r_lTitleId:=ACRuleUsedTitle, r_lMessageId:=ACRuleUsedDetails, r_lOptions:=MsgBoxStyle.Exclamation)

                    'Exit Function
                    Exit Sub
                End If
                lvwChaseCycleRules.Items.Remove(oListItem)

            Else
                DisplayMessage(r_lTitleId:=ACRuleUsedTitle, r_lMessageId:=ACRuleUsedDetails, r_lOptions:=MsgBoxStyle.Exclamation)

                Exit Sub
            End If
        Catch excep As System.Exception



            ' Error Section.



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

         
        End Try


    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click


        ' Click event of the Edit Button.

        Dim oListItem As ListViewItem

        Try

            ' {* USER DEFINED CODE (Begin) *}

            'Only process if there is a selection made
            oListItem = lvwChaseCycleRules.FocusedItem
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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRChaseCycle.Business", vInstanceManager:="ClientManager")
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
            m_oGeneral = New iPMUChaseCycleMaint.General()

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

            ListViewHelper.SetSortedProperty(lvwChaseCycleRules, True)

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
            lvwChaseCycleRules.FullRowSelect = True
            lvwChaseCycleRules.Select()

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

        If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If

        Exit Sub


    End Sub


    Private Sub lvwChaseCycleRules_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        cmdEdit_Click(cmdEdit, New EventArgs())
    End Sub


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
            lvwChaseCycleRules.Items.Clear()

            'Ensure tere is data in the array

            'Get array limits
            If gArrays.GetArrayBounds(r_vArray:=m_vResultArray, r_lDimension:=gArrays.klRowDimension, r_lLower:=lLower, r_lUpper:=lUpper) Then

                'Turn off listview updating
                ListViewFunc.ListViewBatchStart(lvwChaseCycleRules)

                With lvwChaseCycleRules.Items
                    'Loop through the results array and populate the listview
                    For lRow As Integer = lLower To lUpper

                        'Check that the source id of the row is a match
                        If CInt(m_vResultArray(ACChaseCycleRuleSourceId, lRow)) = lSourceID Then

                            'ADD a new listitem to the listview
                            AddOrEditListViewItem(r_oListItem:=Nothing, r_sDescription:=CStr(m_vResultArray(ACChaseCycleRuleDesc, lRow)), r_sGISDataModel:=CStr(m_vResultArray(ACChaseCycleRuleGISDataModelID, lRow)), r_sGISProperty:=CStr(m_vResultArray(ACChaseCycleRuleGISPropertyDesc, lRow)), r_sChaseCycleStatus:=CStr(m_vResultArray(ACChaseCycleDesc, lRow)), r_lActive:=CInt(m_vResultArray(ConstACChaseCycleRuleActive, lRow)), r_lRuleID:=CInt(m_vResultArray(ACChaseCycleRuleID, lRow)), r_lUseEffectiveDate:=CInt(m_vResultArray(ACChaseCycleRuleUseEffectiveDate, lRow)), r_lGisDataModelDesc:=CStr(m_vResultArray(ACDataModelDesc, lRow)))

                        End If
                    Next lRow
                End With
                'Turn on listview updating

                ListViewFunc.ListViewBatchEnd()

                lvwChaseCycleRules.FocusedItem = Nothing

                'Clear the data from the array as it's now stored in the listview

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate the list", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'Turn on listview updating
            ListViewFunc.ListViewBatchEnd()

            Return result

        End Try

    End Function

    Private Function AddOrEditListViewItem(ByRef r_oListItem As ListViewItem, ByRef r_sDescription As String, ByRef r_sGISDataModel As String, ByRef r_sGISProperty As String, ByRef r_sChaseCycleStatus As String, ByRef r_lActive As Integer, ByRef r_lRuleID As Integer, ByRef r_lUseEffectiveDate As Integer, ByRef r_lGisDataModelDesc As String) As Integer

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddOrEditListViewItem
        ' PURPOSE: Adds a new ListItem to the ListView or edits an exitsing one
        ' DATE:01/03/2013
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If a reference to a listitem is not passed we need to create a new one
            If r_oListItem Is Nothing Then
                'Add the new item
                r_oListItem = lvwChaseCycleRules.Items.Add(r_sDescription)
            Else
                'Edit the existing item
                r_oListItem.Text = r_sDescription
            End If

            'Populate the subitems
            With r_oListItem
                ' Save the key so that it is easier to delete an item
                .Name = "Key" & r_lRuleID
                If r_lActive Then
                    ListViewHelper.GetListViewSubItem(r_oListItem, 1).Text = ACYesChar
                Else
                    ListViewHelper.GetListViewSubItem(r_oListItem, 1).Text = ""
                End If
                ListViewHelper.GetListViewSubItem(r_oListItem, 2).Text = r_lRuleID
                If r_lUseEffectiveDate Then
                    ListViewHelper.GetListViewSubItem(r_oListItem, 3).Text = ACYesChar
                Else
                    ListViewHelper.GetListViewSubItem(r_oListItem, 4).Text = ""
                End If
                ListViewHelper.GetListViewSubItem(r_oListItem, 4).Text = r_sChaseCycleStatus
                ListViewHelper.GetListViewSubItem(r_oListItem, 5).Text = r_sGISProperty
                ListViewHelper.GetListViewSubItem(r_oListItem, 6).Text = r_lGisDataModelDesc

             
            End With
        Catch excep As System.Exception



            ' Error Section.

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AddOrEditListViewItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMFalse
            Return result

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
                ' {* USER DEFINED CODE (Begin) *)


                ' Check the task.
                Select Case (r_iTaskType)
                    Case gPMConstants.PMEComponentAction.PMEdit
                        .ChaseCycleRuleID = CInt(Conversion.Val(Me.lvwChaseCycleRules.FocusedItem.SubItems(2).Text))

                    Case gPMConstants.PMEComponentAction.PMAdd
                        .ChaseCycleRuleID = 0
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
        ' DATE: 01/03/2013
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
        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadfrmDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)
            result = gPMConstants.PMEReturnCode.PMFalse


        End Try

        Return result

    End Function


    Private Function DetailsFormProcessWrapper(ByRef r_iTaskType As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DetailsFormProcessWrapper
        ' PURPOSE: Wrapper for Add and Edit processing
        ' DATE: 01/03/2013
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
        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcessWrapper", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)
            result = gPMConstants.PMEReturnCode.PMFalse


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
    '           Created : 01/03/2013 : Chase Cycle RetroFit
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
                gPMFunctions.RaiseError(kMethodName, "bSIRChaseCycle.GetALLPMWrkTaskGroupTasks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get all effective pmusergroups for all effective pwrktaskgroups

            lReturn = m_oBusiness.GetALLPMWrkTaskGroupPMUserGroups(r_vResults:=m_vTaskGroupUserGroups)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bSIRChaseCycle.GetALLPMWrkTaskGroupPMUserGroups Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get lookup data for task groups
            lReturn = GetLookups()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)
            result = gPMConstants.PMEReturnCode.PMFalse


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
    '           Created :  01/03/2013
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
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSourceCombo
        ' PURPOSE:
        ' DATE: 01/03/2013
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
            If Information.IsArray(vSourceArray) Then

                For i As Integer = 0 To vSourceArray.GetUpperBound(1)
                    'Add using branch description (3).
                    Dim cboPMLookupSource_NewIndex As Integer = -1

                    Dim listIndex As Integer = cboPMLookupSource.Items.Add(New VB6.ListBoxItem(vSourceArray(1, i), vSourceArray(0, i)))
                Next i

            End If


            m_oPMUser.Dispose()
            m_oPMUser = Nothing
        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMFalse
            '*******************************

            Return result


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


    Private Sub lvwChaseCycleRules_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwChaseCycleRules.DoubleClick
        If cmdEdit.Enabled Then
            cmdEdit_Click(sender, e)
        End If
    End Sub

End Class
