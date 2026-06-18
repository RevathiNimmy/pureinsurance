Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports System.Data
Imports SharedFiles
Friend Partial Class frmDetails
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
    ' Date: 01/03/2013
	'
	' Description: Main Details interface.
	'
	' Edit History:
    ' 	' ***************************************************************** '

    Public Const vbFormCode As Integer = 0

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmDetails"

    Private Const ACYesChar As String = "Y"

    ' PUBLIC Data Members (Begin)
    'Now OK to use PUBLIC variable instead of Property (as long as no validation, read only, etc)

    Private m_lChaseCycleRuleID As Integer
    Private m_sDescription As String = ""
    Private m_vAllStepsArray As Object
    Private m_lSourceId As Integer
    Private m_lGISDataModelID As Integer = 0
    Private m_sGISDataModelCode As String = ""
    Private m_sChaseCycleUDL As String = ""
    Private m_lChaseCycleStatusID As String = 0
    Private m_sChaseCycleStatusDescription As String = ""
    Private m_iIsActive As CheckState
    Private m_lProcessingDays As Object
    Private m_iUseEffectiveDate As CheckState
    Private m_iUseGreaterTransEffDate As CheckState
    Private m_iIncludeCancelled As CheckState
    Private m_iCancelledOnly As CheckState


    'Let's caller know if changes were applied
    Public Applied As Boolean
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
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

    ' {* USER DEFINED CODE (Begin) *}


    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUChaseCycleMaint.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Form control
    Private m_oFormfields As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails(,) As Object
    Private m_vDocTemplateList(,) As Object


    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    'Result Array columns for GetDetails
    Private Const ACChaseCycleStepId As Integer = 0
    Private Const ACStepNumber As Integer = 2
    Private Const ACElapsedDays As Integer = 3
    Private Const ACNextStep As Integer = 8
    Private Const ACAutoCancel As Integer = 7


    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}


    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PRIVATE Const Members (Begin)
    ' {* USER DEFINED CODE (Begin) *}

    ''Result Array columns for CashListDrawer Security (ARRAY and LIST)

    Private m_bParentChanged As Boolean

    Private m_vTaskGroupUserGroups(,) As Object
    Private m_vTaskGroupTask(,) As Object
    Private m_vLookupTables(,) As Object
    Private m_vGISDataModel(,) As Object
    Private m_vChaseCycleStatus(,) As Object
    Private m_sAgencyOrUnderwriting As String = ""
    Private m_lProductID As Integer
    Private m_lGISPropertyID As Integer

    Public WriteOnly Property ChaseCycleStatus() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vChaseCycleStatus = Value
        End Set
    End Property
    Public WriteOnly Property GISDataModel() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vGISDataModel = Value
        End Set
    End Property


    Public WriteOnly Property LookupTables() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vLookupTables = Value
        End Set
    End Property

    Public WriteOnly Property LookupDetails() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vLookupDetails = Value
        End Set
    End Property

    Public WriteOnly Property TaskGroupTasks() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vTaskGroupTask = Value
        End Set
    End Property

    Public WriteOnly Property TaskGroupUsers() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vTaskGroupUserGroups = Value
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

    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property DefaultSourceID() As Integer
        Set(ByVal Value As Integer)
            m_lSourceId = Value
        End Set
    End Property
 
    Public WriteOnly Property ChaseCycleRuleID() As Integer
        Set(ByVal Value As Integer)
            m_lChaseCycleRuleID = Value
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

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            'Get details for the PARENT from the business object
            m_lReturn = GetChaseCycleRule()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get details for Security (CHILD) from the business object
            'This is an example of retrieving child records
            m_lReturn = GetSteps()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch ex As Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

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

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            'Populate the controls with data from the Result Array
            txtDescription.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sDescription.Trim())

            txtProcessingDays.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatInteger, vFieldValue:=m_lProcessingDays)
            chkActive.CheckState = m_iIsActive
            chkUseEffectiveDate.CheckState = m_iUseEffectiveDate

            chkUseGreaterTransEffDate.CheckState = m_iUseGreaterTransEffDate

            chkIncludeCancelled.CheckState = m_iIncludeCancelled

            chkCancelledOnly.CheckState = m_iCancelledOnly

            cboPMLookupSource.ItemId = m_lSourceId

            cboGISDataModel.ItemId = IIf(m_lGISDataModelID = 0, 0, m_lGISDataModelID)

            cboProduct.ItemId = m_lProductID

            cboGISProperty.SelectedItem = m_lGISPropertyID

            m_lReturn = PopulateStepListView()
            Dim r_vUDLArrayDescription As Object

            'following call wii retrive property and chase cyclestaus description
            Dim iChaseCycleStatus As Integer = GetChaseCycleStatusDesc(v_chaseCycleRuleID:=m_lChaseCycleRuleID, r_vUDLArrayDesc:=r_vUDLArrayDescription)
            cboGISProperty.Text = r_vUDLArrayDescription(4, 0)
            cboUDLChaseCycle.Text = r_vUDLArrayDescription(1, 0)


            ' {* USER DEFINED CODE (End) *}

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
    ' Name: GetChaseCycleStatusDesc
    '
    ' Description: Uses Chase_cycle_rulee_id to get description of Chase cycle status UDl description
    '              
    '
    ' ***************************************************************** '
    Public Function GetChaseCycleStatusDesc(ByVal v_chaseCycleRuleID As Integer, ByRef r_vUDLArrayDesc As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim r_vUDLArrayDescription As Object
            m_lReturn = m_oBusiness.GetChaseCycleUDLDesc(v_chaseCycleRuleID, r_vUDLArrayDescription:=r_vUDLArrayDesc)

            If Information.IsArray(r_vUDLArrayDescription) Then
                Return result
            End If
        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log rror.
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
        '---------Done on 27/jan/14 by Samarjeet-------
        'Const kMethodType As String = "InterfaceToBusiness"

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
                    m_lReturn = AddChaseCycleRule()

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.
                    m_lReturn = EditChaseCycleRule()

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
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the calling List form

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


            m_lReturn = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions(Me, My.Resources.ResourceManager)

            'Display main caption
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then


                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************

            'Filter out branches user is not authorised to access.
            cboPMLookupSource.WhereClause = "source_id NOT IN (SELECT source_id FROM pmuser_source WHERE user_id =" & g_oObjectManager.UserID & " AND is_deleted = 0)"
            cboPMLookupSource.RefreshList()

            'Filter out data model on the basis of gis_data_model_type_id.
            cboGISDataModel.WhereClause = "gis_data_model_type_id=" & 1
            cboGISDataModel.RefreshList()

            'Only need to set up defaults in add mode
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                'Set the default Source to that passed from interface screen
                cboPMLookupSource.ItemId = m_lSourceId

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If




            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                'Enable the step buttons because we have an existing item
                cmdAddStep.Enabled = True
                cmdEditStep.Enabled = True
                cmdDeleteStep.Enabled = True

            End If

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



    Private Function PopulateStepListView() As Integer

        Dim result As Integer = 0
        Dim lLower, lUpper As Integer

        Try

            result = True

            'Clear the existing items
            lvwSteps.Items.Clear()

            'Ensure there is data in the array

            'Get array limits
            If gArrays.GetArrayBounds(r_vArray:=m_vAllStepsArray, r_lDimension:=gArrays.klRowDimension, r_lLower:=lLower, r_lUpper:=lUpper) Then

                'Turn off listview updating

                ListViewFunc.ListViewBatchStart(lvwSteps)

                'Populate the listview
                With lvwSteps.Items
                    'Loop through the results array and populate the listview
                    For lRow As Integer = lLower To lUpper

                        'add a new listitem to the listview

                        AddOrEditListViewItem(r_oListItem:=Nothing, r_vStepNumber:=CStr(m_vAllStepsArray(ACStepNumber, lRow)), r_vElapsedDays:=CStr(m_vAllStepsArray(ACElapsedDays, lRow)), r_vNextStep:=CStr(m_vAllStepsArray(ACNextStep, lRow)), r_vAutoCancel:=CStr(m_vAllStepsArray(ACAutoCancel, lRow)), r_lChaseCycleStepId:=CInt(m_vAllStepsArray(ACChaseCycleStepId, lRow)))


                    Next lRow

                End With

                'Turn on listview updating
                ListViewFunc.ListViewBatchEnd()

                'Clear selection
                lvwSteps.FocusedItem = Nothing

                'Clear the data from the array as it's now stored in the listview

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = False

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate security list", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateStepListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

    End Function

    Private Sub cboPMLookupSource_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMLookupSource.Click
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Sub cboProduct_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProduct.Click
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Sub chkActive_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkActive.CheckStateChanged
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Sub chkUseEffectiveDate_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkUseEffectiveDate.CheckStateChanged


        ParentChanged_Renamed(r_bChanged:=True)

        If chkUseEffectiveDate.Enabled And chkUseEffectiveDate.CheckState = CheckState.Checked Then
            chkUseGreaterTransEffDate.Visible = True
        ElseIf chkUseEffectiveDate.CheckState = CheckState.Unchecked Then
            chkUseGreaterTransEffDate.CheckState = CheckState.Unchecked
            chkUseGreaterTransEffDate.Visible = False
        End If

    End Sub

    Private isInitializingComponent As Boolean

    Private Sub txtDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Sub cboPMLookupPFFrequency_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Sub cmdAddStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddStep.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdAddStep_Click
        ' PURPOSE: Show the Chase Cycle Step form for Adding
        ' CHANGES:
        ' ---------------------------------------------------------------------------
        Dim lChaseCycleStepId As Integer


        Try

            'Derive the document template list (not a PMLookup field yet)
            m_lReturn = GetDocTemplateList() 'm_vDocTemplateList

            ' get next available instalment failure count
            ofrmStep = New frmStep()

            With ofrmStep

                .LookupTables = VB6.CopyArray(m_vLookupTables)
                .LookupDetails = VB6.CopyArray(m_vLookupDetails)
                .TaskGroupTasks = VB6.CopyArray(m_vTaskGroupTask)
                .TaskGroupUsers = VB6.CopyArray(m_vTaskGroupUserGroups)

                .Status = gPMConstants.PMEComponentAction.PMAdd
                .StepID = 0
                .DocumentList = m_vDocTemplateList
                .ShowDocumentLists()

                .StepNumber = Nothing

                .ElapsedDays = Nothing

                .ClientLetterId = Nothing

                .CheckAutoCancelRules = Nothing

                .RunAutoCancelRules = Nothing

                .NextStep = Nothing

                .PreviousStep = Nothing

                .ShowDialog()

                'Determine action
                Select Case .Status
                    Case gPMConstants.PMEReturnCode.PMOK

                        'Write it to the database now


                        m_lReturn = m_oBusiness.DirectAddStep(r_vChaseCycleStepID:=lChaseCycleStepId, v_vChaseCycleRuleID:=m_lChaseCycleRuleID, v_vStepNumber:=.StepNumber, v_vNumberOfDays:=.ElapsedDays, v_vClientDocumentTemplateID:=.ClientLetterId, v_vPMWrkTaskID:=ZeroToNull(.TaskId), v_vPMUserGroupID:=ZeroToNull(.UserGroupId), v_vCheckAutoCancel:=.CheckAutoCancelRules, v_vAutoCancelPolicy:=.RunAutoCancelRules, v_vNextStep:=.NextStep, v_vPreviousStep:=.PreviousStep, v_vStepDescription:=.StepDescription, v_vPMWrkTaskGroupId:=ZeroToNull(.TaskGroupId))

                        ' Check for errors
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Display standard message
                            DisplayMessage(r_lTitleId:=ACFailedToUpdateTitle, r_lMessageId:=ACFailedUpdateDetail, r_lOptions:=MsgBoxStyle.Exclamation)

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add step details to the database", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddStep_Click")

                            'Exit Function
                            Exit Sub
                        End If

                        'ADD the new item to the listview


                        m_lReturn = AddOrEditListViewItem(r_oListItem:=Nothing, r_vStepNumber:=.StepNumber, r_vElapsedDays:=.ElapsedDays, r_vNextStep:=.NextStep, r_vAutoCancel:=.RunAutoCancelRules, r_lChaseCycleStepId:=lChaseCycleStepId)

                    Case gPMConstants.PMEReturnCode.PMCancel

                End Select

            End With
        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddStep_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdApply_Click
        ' PURPOSE: Apply user changes to the database
        ' AUTHOR: vidya Rangdale
        ' DATE: 01/03/2013
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------
        Dim sTitle, sMessage As String


        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Check mandatory controls have been entered into.

            m_lReturn = m_oFormfields.CheckMandatoryControls()
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Check that a branch has been selected
            If cboPMLookupSource.ListIndex < 0 Then


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoBranchSelected, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Me.Visible Then
                    cboPMLookupSource.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtProcessingDays.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("Processing Days must be numeric.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtProcessingDays.Focus()
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Check that a frequency has been selected
            If cboGISDataModel.ListIndex < 0 Then

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoDataModelSelected, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                If Me.Visible Then
                    cboGISDataModel.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            If cboGISProperty.SelectedIndex < 0 Then
                MessageBox.Show("Please select Chase Cycle Property.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Me.Visible Then
                    cboGISProperty.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If
            If cboUDLChaseCycle.SelectedIndex < 0 Then
                MessageBox.Show("Please select Chase Cycle Status.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Me.Visible Then
                    cboUDLChaseCycle.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If
            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=m_bParentChanged)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK

                'Changes have been saved
                ParentChanged_Renamed(r_bChanged:=False)

                'Tell the caller that changes were applied
                '(So that the list can be updated)
                Me.Applied = True
                Me.Task = gPMConstants.PMEComponentAction.PMEdit

                'Enable the step buttons because we have an existing item
                cmdAddStep.Enabled = True
                cmdEditStep.Enabled = True
                cmdDeleteStep.Enabled = True

            End If
        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdDeleteStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteStep.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdDeleteSecurity_Click
        ' PURPOSE: Call business object to delete a CashList_Drawer_Security
        '          record from the database
        ' AUTHOR: Vidya Rangdale
        ' DATE: 01/03/2013
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim oListItem As ListViewItem


        Try

            'Only process if there is a selection made
            oListItem = lvwSteps.FocusedItem
            If oListItem Is Nothing Then
                DisplayMessage(r_lTitleId:=ACNoSelectionTitle, r_lMessageId:=ACNoSelectionDetails, r_lOptions:=MsgBoxStyle.Exclamation)

                Exit Sub
            End If

            'Ensure that delete should proceed
            m_lReturn = DisplayMessage(r_lTitleId:=ACConfirmDeleteTitle, r_lMessageId:=ACConfirmDeleteDetails, r_lOptions:=MsgBoxStyle.YesNo + MsgBoxStyle.Question)

            If m_lReturn = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
            'Pass call through to business object to do the work

            m_lReturn = m_oBusiness.DirectDeleteStep(v_lChaseCycleStepID:=ListViewHelper.GetListViewSubItem(oListItem, 4).Text)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details
                MessageBox.Show("You cannot delete this step because it is in use.", "Step in use", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
                Exit Sub
            End If

            'Remove the item form the listview
            lvwSteps.Items.Remove(oListItem)
        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteStep_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdEditStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditStep.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdEditStep_Click
        ' PURPOSE: Show the Chase Cycle Step form for Editing
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim oListItem As ListViewItem
        Dim lChaseCycleStepId As Integer
        Dim vStepNumber As Object
        Dim vNumberOfDays As Object
        Dim vClientDocumentTemplateID As Object
        Dim vPMWrkTaskID As Object
        Dim vPMUserGroupID As Object
        Dim vCheckAutoCancel As Object
        Dim vAutoCancelPolicy As Object
        Dim vNextStep As Object
        Dim vPreviousStep As Object
        Dim vStepDescription As Object
        Dim vPMWrkTaskGroupId As Object


        Try

            'Only process if there is a selection made
            oListItem = lvwSteps.FocusedItem
            If oListItem Is Nothing Then
                DisplayMessage(r_lTitleId:=ACNoSelectionTitle, r_lMessageId:=ACNoSelectionDetails, r_lOptions:=MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'Derive the details of the step
            lChaseCycleStepId = CInt(ListViewHelper.GetListViewSubItem(oListItem, 4).Text)


            m_lReturn = m_oBusiness.GetStepDetails(v_lChaseCycleStepID:=lChaseCycleStepId, r_vStepNumber:=vStepNumber, r_vNumberOfDays:=vNumberOfDays, r_vClientDocumentTemplateID:=vClientDocumentTemplateID, r_vPMWrkTaskID:=vPMWrkTaskID, r_vPMUserGroupID:=vPMUserGroupID, r_vCheckAutoCancel:=vCheckAutoCancel, r_vAutoCancelPolicy:=vAutoCancelPolicy, r_vNextStep:=vNextStep, r_vPreviousStep:=vPreviousStep, r_vStepDescription:=vStepDescription, r_vPMWrkTaskGRoupId:=vPMWrkTaskGroupId)

            'Display the form with the selected details

            'Get the data for the selected item in the listview

            'Derive the document template list (not a PMLookup field yet)
            m_lReturn = GetDocTemplateList()
            ofrmStep = New frmStep()

            With ofrmStep
                .LookupTables = VB6.CopyArray(m_vLookupTables)
                .LookupDetails = VB6.CopyArray(m_vLookupDetails)
                .TaskGroupTasks = VB6.CopyArray(m_vTaskGroupTask)
                .TaskGroupUsers = VB6.CopyArray(m_vTaskGroupUserGroups)
                .TaskGroupId = gPMFunctions.ToSafeLong(vPMWrkTaskGroupId)
                .UserGroupId = gPMFunctions.ToSafeLong(vPMUserGroupID)
                .TaskId = gPMFunctions.ToSafeLong(vPMWrkTaskID)
                .StepDescription = vStepDescription

                .Status = gPMConstants.PMEComponentAction.PMEdit
                .StepID = lChaseCycleStepId
                .DocumentList = m_vDocTemplateList
                .ShowDocumentLists()
                .StepNumber = vStepNumber
                .ElapsedDays = vNumberOfDays
                .ClientLetterId = vClientDocumentTemplateID
                .CheckAutoCancelRules = vCheckAutoCancel
                .RunAutoCancelRules = vAutoCancelPolicy
                .NextStep = vNextStep
                .PreviousStep = vPreviousStep
                .ShowDialog()

                'Determine action
                Select Case .Status
                    Case gPMConstants.PMEReturnCode.PMOK

                        'Write it to the database now


                        m_lReturn = m_oBusiness.DirectEditStep(v_vChaseCycleStepID:=lChaseCycleStepId, v_vChaseCycleRuleID:=m_lChaseCycleRuleID, v_vStepNumber:=.StepNumber, v_vNumberOfDays:=.ElapsedDays, v_vClientDocumentTemplateID:=.ClientLetterId, v_vPMWrkTaskID:=ZeroToNull(.TaskId), v_vPMUserGroupID:=ZeroToNull(.UserGroupId), v_vCheckAutoCancel:=.CheckAutoCancelRules, v_vAutoCancelPolicy:=.RunAutoCancelRules, v_vNextStep:=.NextStep, v_vPreviousStep:=.PreviousStep, v_vStepDescription:=.StepDescription, v_vPMWrkTaskGroupId:=ZeroToNull(.TaskGroupId))

                        ' Check for errors
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Display standard message
                            DisplayMessage(r_lTitleId:=ACFailedToUpdateTitle, r_lMessageId:=ACFailedUpdateDetail, r_lOptions:=MsgBoxStyle.Exclamation)

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to edit step details to the database", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditStep_Click")

                            'Exit Function
                            Exit Sub
                        End If

                        'ADD the new item to the listview
                        m_lReturn = AddOrEditListViewItem(r_oListItem:=oListItem, r_vStepNumber:=.StepNumber, r_vElapsedDays:=.ElapsedDays, r_vNextStep:=.NextStep, r_vAutoCancel:=.RunAutoCancelRules, r_lChaseCycleStepId:=lChaseCycleStepId)

                    Case gPMConstants.PMEReturnCode.PMCancel

                End Select

            End With
        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditStep_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String
        Me.cboProduct.FirstItem = ""
        Me.cboPMLookupSource.FirstItem = ""

        ' Forms initialise event.

        Try

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

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
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


    Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

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

            m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sAgencyOrUnderwriting)

            If m_oBusiness Is Nothing Then
                Form_Initialize_Renamed()
            End If
            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the rules for validating fields.

            m_lReturn = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Enable row select
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSteps.Handle.ToInt32(), v_vShowRowSelect:=True)
            lvwSteps.FullRowSelect = True
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            txtProcessingDays.Visible = True
            lblProcessingDays.Visible = True

            'm_lReturn = PopulateInsuranceFileStatusCheckedListBox()

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            chkUseGreaterTransEffDate.Visible = chkUseEffectiveDate.Enabled And chkUseEffectiveDate.CheckState = CheckState.Checked



            cmdApply.Enabled = False

            'Set form so that no changes are registered
            'Added the check for correct functioning 
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                ParentChanged_Renamed(r_bChanged:=False)
            End If


            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try



    End Sub

    Private Sub frmDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                'Process the next set of actions depending
                'upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=m_bParentChanged)

                ' Check the return value.
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

    Private Sub lvwSteps_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSteps.DoubleClick

        ' Double click event for the List details.

        Try

            ' Check if there are any items available.
            If lvwSteps.Items.Count = 0 Then
                Exit Sub
            End If

            ' Only edit if Edit is enabled
            If cmdEditStep.Enabled Then

                ' Bring up Edit Details form
                cmdEditStep.PerformClick()

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSteps_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim sTitle, sMessage As String

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Check mandatory controls have been entered into.

            m_lReturn = m_oFormfields.CheckMandatoryControls()
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtProcessingDays.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("Processing Days must be numeric.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtProcessingDays.Focus()
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Check that a branch has been selected
            If cboPMLookupSource.ListIndex < 0 Then


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoBranchSelected, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Me.Visible Then
                    cboPMLookupSource.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub

            End If
            If cboGISProperty.SelectedIndex < 0 Then
                MessageBox.Show("Please select Chase Cycle Property.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Me.Visible Then
                    cboGISProperty.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If
            If cboUDLChaseCycle.SelectedIndex < 0 Then
                MessageBox.Show("Please select Chase Cycle Status.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Me.Visible Then
                    cboUDLChaseCycle.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Check that a Business type has been selected


            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=m_bParentChanged)

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

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=m_bParentChanged)

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

    Private Function GetChaseCycleRule() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetChaseCycleRule
        ' PURPOSE: Get a GetChaseCycleRule record form the database
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetRuleDetails(v_lChaseCycleRuleId:=m_lChaseCycleRuleID, _
                                                   r_vDescription:=m_sDescription, _
                                                   r_vSourceID:=m_lSourceId, _
                                                   r_vGISDataModel:=m_lGISDataModelID, r_vGISPropertyId:=m_lGISPropertyID, _
                                                   r_vChaseCycleUDLID:=m_lChaseCycleStatusID, r_vIsActive:=m_iIsActive, r_vProcessingDays:=m_lProcessingDays, _
                                                   r_vUseEffectiveDate:=m_iUseEffectiveDate, _
                                                   r_vUseGreaterTransEffDate:=m_iUseGreaterTransEffDate, _
    r_vProductID:=m_lProductID, _
    r_vIncludeCancelled:=m_iIncludeCancelled, _
    r_vCancelledOnly:=m_iCancelledOnly)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get ComponentName details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChaseCycleRule")

                Return result
            End If
        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChaseCycleRule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            Exit Function

        End Try

        Return result


    End Function

    Private Function GetSteps() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSteps
        ' PURPOSE: Get steps associated with this rule
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetStepList(v_lChaseCycleRuleId:=m_lChaseCycleRuleID, r_vResultArray:=m_vAllStepsArray)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to getSteplist details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSteps")

                Return result
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSteps", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)
            result = gPMConstants.PMEReturnCode.PMFalse
            Exit Function

        End Try

        Return result


    End Function
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: AddOrEditListViewItem
    ' PURPOSE: Adds a ListItem to the Steps ListView or edits an existing one
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function AddOrEditListViewItem(ByRef r_oListItem As ListViewItem, ByRef r_vStepNumber As String, ByRef r_vElapsedDays As String, ByVal r_vNextStep As String, ByVal r_vAutoCancel As String, ByRef r_lChaseCycleStepId As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Convert.IsDBNull(r_vStepNumber) Or IsNothing(r_vStepNumber) Then
                r_vStepNumber = ""
            End If


            If Convert.IsDBNull(r_vElapsedDays) Or IsNothing(r_vElapsedDays) Then
                r_vElapsedDays = ""
            End If


            If Convert.IsDBNull(r_vNextStep) Or IsNothing(r_vNextStep) Then
                r_vNextStep = ""
            End If


            If Convert.IsDBNull(r_vAutoCancel) Or IsNothing(r_vAutoCancel) Then
                r_vAutoCancel = ""
            Else
                If Conversion.Val(r_vAutoCancel) = 1 Then
                    r_vAutoCancel = ACYesChar
                Else
                    r_vAutoCancel = ""
                End If
            End If

            If r_oListItem Is Nothing Then
                'Add the new item
                r_oListItem = lvwSteps.Items.Add(r_vStepNumber)
            Else
                'Edit the new item
                r_oListItem.Text = r_vStepNumber
            End If

            'Populate the subitems
            With r_oListItem
                .Name = "Key" & r_lChaseCycleStepId ' Save the key so that it is easier to delete an item
                ListViewHelper.GetListViewSubItem(r_oListItem, 1).Text = r_vElapsedDays
                ListViewHelper.GetListViewSubItem(r_oListItem, 2).Text = r_vNextStep
                ListViewHelper.GetListViewSubItem(r_oListItem, 3).Text = r_vAutoCancel
                ListViewHelper.GetListViewSubItem(r_oListItem, 4).Text = CStr(r_lChaseCycleStepId)
            End With
        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AddOrEditListViewItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMFalse
            Exit Function

        End Try

        Return result


    End Function


    Private Function AddChaseCycleRule() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddChaseCycleRule
        ' PURPOSE: Adds a ComponentName record to the database
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lChaseCycleRuleID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.DirectAddRule(r_vChaseCycleRuleID:=lChaseCycleRuleID, v_vDescription:=Me.txtDescription.Text.Trim(), _
                                                  v_vSourceID:=Me.cboPMLookupSource.ItemId, v_vGISDataModel:=Me.cboGISDataModel.ItemId, v_vGISProperty:=VB6.GetItemData(cboGISProperty, cboGISProperty.SelectedIndex), _
                                                  v_vChaseCycleStatusID:=VB6.GetItemData(cboUDLChaseCycle, cboUDLChaseCycle.SelectedIndex), v_vIsActive:=Me.chkActive.CheckState, _
                                                  v_vProcessingDays:=Me.txtProcessingDays.Text, v_vUseEffectiveDate:=Me.chkUseEffectiveDate.CheckState, _
                                                  v_vUseGreaterTranEffDate:=Me.chkUseGreaterTransEffDate.CheckState, v_vProductID:=Me.cboProduct.ItemId, v_vIncludeCancelled:=Me.chkIncludeCancelled.CheckState, v_vCancelledOnly:=Me.chkCancelledOnly.CheckState)


            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add Chase_Cycle_Rule details to the database", vApp:=ACApp, vClass:=ACClass, vMethod:="AddChaseCycleRule")

                'Exit Function
                Return result
            End If

            m_lChaseCycleRuleID = lChaseCycleRuleID
        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AddChaseCycleRule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMFalse
            Exit Function

        End Try
        Return result

    End Function
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: EditChaseCycleRule
    ' PURPOSE: Updates changes to a CashList_Drawer record on the database
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function EditChaseCycleRule() As Integer


        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_lReturn = m_oBusiness.DirectEditRule(v_vChaseCycleRuleID:=m_lChaseCycleRuleID, v_vDescription:=Me.txtDescription.Text.Trim(), _
                                              v_vSourceID:=Me.cboPMLookupSource.ItemId, v_vGISDataModel:=Me.cboGISDataModel.ItemId, v_lGISPropertyID:=VB6.GetItemData(cboGISProperty, cboGISProperty.SelectedIndex), _
                                              v_vChaseCycleStatusID:=VB6.GetItemData(cboUDLChaseCycle, cboUDLChaseCycle.SelectedIndex), v_vIsActive:=Me.chkActive.CheckState, _
                                              v_vProcessingDays:=Me.txtProcessingDays.Text, v_vUseEffectiveDate:=Me.chkUseEffectiveDate.CheckState, _
                                              v_vUseGreaterTranEffDate:=Me.chkUseGreaterTransEffDate.CheckState, v_vProductID:=cboProduct.ItemId, _
v_vIncludeCancelled:=Me.chkIncludeCancelled.CheckState, _
v_vCancelledOnly:=Me.chkCancelledOnly.CheckState)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update Chase_Cycle_Rule details to the database", vApp:=ACApp, vClass:=ACClass, vMethod:="EditChaseCycleRule")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="EditChaseCycleRule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
            Return result

        End Try

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: ParentChanged
    ' PURPOSE: Sets flag to indicate whether user has changed parent record
    '          (Also enables / disables Apply command)
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Sub ParentChanged_Renamed(ByRef r_bChanged As Boolean)


        Try

            If m_bParentChanged <> r_bChanged Then
                m_bParentChanged = r_bChanged
            End If
            If cmdApply.Enabled <> r_bChanged Then
                cmdApply.Enabled = r_bChanged
            End If

            'If the apply button is enabled, disable the add and edit button
            ' (this ensures that the business type is applied and relevant to the step screen)
            If Me.Visible Then 'only activated after form visible
                If cmdApply.Enabled Then
                    cmdAddStep.Enabled = False
                    cmdEditStep.Enabled = False
                Else
                    cmdAddStep.Enabled = True
                    cmdEditStep.Enabled = True
                End If
            End If

        Catch excep As System.Exception

            ' Error Section.


            ' Log rror.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ParentChanged", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

        End Try

    End Sub
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetDocTemplateList
    ' PURPOSE: Get list of Document_Template records
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetDocTemplateList() As Integer


        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetDocTemplateList(m_vDocTemplateList)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to return list of Document Templates from Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplateList")

                Return result

            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplateList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)
            result = gPMConstants.PMEReturnCode.PMFalse
            Exit Function

        End Try
        Return result


    End Function


    Private Sub txtProcessingDays_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProcessingDays.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Function ZeroToNull(ByRef vValue As Object) As Object
        ZeroToNull = vValue

        If Not IsDBNull(vValue) Then
            If vValue = 0 Then
                ZeroToNull = DBNull.Value
            End If
        End If

    End Function

    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
    End Sub

    Private Sub cboGISDataModel_Click(ByVal Sender As Object, ByVal e As System.EventArgs) Handles cboGISDataModel.Click
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Sub cboUDLChaseCycle_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboUDLChaseCycle.SelectedIndexChanged
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME:  cboGISDataModel_ItemCodeChange
    ' PURPOSE: Call business object to get a Chase Cycle Properties
    '          record from the database
    ' DATE:04/03/2013
    ' ---------------------------------------------------------------------------
    Private Sub cboGISDataModel_ItemCodeChange() Handles cboGISDataModel.ItemCodeChange
        Dim vUDLArray(,) As Object
        'Populate the list

        m_lReturn = m_oBusiness.GetChaseCycleProperties(v_lGISDataModel:=cboGISDataModel.ItemId, r_vUDLArray:=vUDLArray)

        cboUDLChaseCycle.Items.Clear()
        cboGISProperty.Items.Clear()
        cboUDLChaseCycle.Text = ""
        cboGISProperty.Text = ""
        'Populate udl combo
        If Information.IsArray(vUDLArray) Then

            For i As Integer = 0 To vUDLArray.GetUpperBound(1)
                'Add using branch description (3).
                Dim cboGISProperty_NewIndex As Integer = -1

                Dim listIndex As Integer = cboGISProperty.Items.Add(New VB6.ListBoxItem(vUDLArray(1, i), vUDLArray(0, i)))

            Next i
            cboGISProperty.SelectedIndex = 0


        End If

    End Sub
    Public Sub PopulatePaymentFailedReasonDropdown()
        cboProduct.ItemId = m_lProductID
        cboGISDataModel.FirstItem = ""
    End Sub
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME:  cboGISProperty_SelectedIndexChanged
    ' PURPOSE: Call business object to get a Chase Cycle UDL status
    '          record from the database
    ' DATE:04/03/2013
    ' ---------------------------------------------------------------------------
    Private Sub cboGISProperty_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGISProperty.SelectedIndexChanged
        Dim vPropertyArray(,) As Object

        'Populate the list

        m_lReturn = m_oBusiness.GetChaseCycleUDL(v_lGISProperty:=VB6.GetItemData(cboGISProperty, cboGISProperty.SelectedIndex), r_vPropertyArray:=vPropertyArray)

        cboUDLChaseCycle.Items.Clear()
        cboUDLChaseCycle.Text = ""
        If Information.IsArray(vPropertyArray) Then

            For i As Integer = 0 To vPropertyArray.GetUpperBound(1)
                'Add using branch description (3).
                Dim cboGISProperty_NewIndex As Integer = -1

                Dim listIndex As Integer = cboUDLChaseCycle.Items.Add(New VB6.ListBoxItem(vPropertyArray(1, i), vPropertyArray(0, i)))

            Next i
            cboUDLChaseCycle.SelectedIndex = 0


        End If


    End Sub


    Private Sub chkIncludeCancelled_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIncludeCancelled.CheckStateChanged
        If chkIncludeCancelled.CheckState = CheckState.Checked Then
            chkCancelledOnly.CheckState = CheckState.Unchecked
        End If

    End Sub

    Private Sub chkCancelledOnly_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCancelledOnly.CheckStateChanged
        If chkCancelledOnly.CheckState = CheckState.Checked Then
            chkIncludeCancelled.CheckState = CheckState.Unchecked
        End If
    End Sub

End Class
