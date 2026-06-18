Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'refer developer guide no. 129
Imports SharedFiles
Partial Friend Class frmDetails
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 03/10/2002
    '
    ' Description: Main Details interface.
    '
    ' Edit History:
    ' RKS 04/07/2006    Added Use Effective Date checkbox (NEM Insurance)
    ' ***************************************************************** '
    'developer guide no.7
    Public Const vbFormCode As Integer = 0

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmDetails"

    Private Const ACYesChar As String = "Y"

    ' PUBLIC Data Members (Begin)
    'Now OK to use PUBLIC variable instead of Property (as long as no validation, read only, etc)

    Private m_lCreditControlRuleID As Integer
    Private m_sDescription As String = ""
    Private m_vAllStepsArray As Object
    Private m_lSourceId As Integer
    Private m_sBusinessType As String = ""
    Private m_lPFFrequencyID As String = ""
    Private m_sPFFrequencyDescription As String = ""
    Private m_iIsActive As CheckState
    Private m_lProcessingDays As Object
    Private m_iUseEffectiveDate As CheckState
    Private m_iUseGreaterTransEffDate As CheckState
    Private m_iUseDueDate As CheckState
    Private m_lInstalmentResultId As Integer
    Private m_oUseInceptionDate As CheckState
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
    Private m_oGeneral As iACTCreditControlMaint.General

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

    '' Control array to store the first and last
    '' text box controls for each tab.
    'Private m_ctlTabFirstLast() As Control

    'Result Array columns for GetDetails
    Private Const ACCreditControlStepId As Integer = 0
    Private Const ACStepNumber As Integer = 2
    Private Const ACElapsedDays As Integer = 3
    Private Const ACBrokerDays As Integer = 4
    Private Const ACPolicyAmt As Integer = 8
    Private Const ACAccountAmt As Integer = 9
    Private Const ACNextStep As Integer = 14
    Private Const ACAutoCancel As Integer = 13




    ''Store the results from SP for ComponentNameSecurity
    'Private m_vAllStepsArraySecurity As Variant
    'Private Const ksArraySecurity As String = "ArraySecurity"

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}


    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PRIVATE Const Members (Begin)
    ' {* USER DEFINED CODE (Begin) *}

    ''Result Array columns for CashListDrawer Security (ARRAY and LIST)
    'Private Const ACSecurityUserGroup As Integer = 0
    'Private Const ACSecurityUserGroupId As Integer = 1
    'Private Const ACSecurityCompany As Integer = 2
    'Private Const ACSecurityCompanyId As Integer = 3
    '
    'Private Const ACTABDetails As Integer = 0
    'Private Const ACTABSecurity As Integer = 1
    'Private Const ACTABGenerateTask As Integer = 2
    Private m_bParentChanged As Boolean

    Private m_vTaskGroupUserGroups(,) As Object
    Private m_vTaskGroupTask(,) As Object
    Private m_vLookupTables(,) As Object
    Private m_vValidInsuranceFileStatuses(,) As Object
    Private m_sAgencyOrUnderwriting As String = ""
    Private m_lPolicyIsPaid As Integer
    Private m_lProductID As Integer
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String
    'Developer Guide No. 33
    Public WriteOnly Property ValidInsuranceFileStatuses() As Object(,)
        Set(ByVal Value As Object(,))
            m_vValidInsuranceFileStatuses = Value
        End Set
    End Property
    'Developer Guide No. 33
    Public WriteOnly Property LookupTables() As Object(,)
        Set(ByVal Value As Object(,))
            m_vLookupTables = Value
        End Set
    End Property
    'Developer Guide No. 33
    Public WriteOnly Property LookupDetails() As Object(,)
        Set(ByVal Value As Object(,))
            m_vLookupDetails = Value
        End Set
    End Property
    'Developer Guide No. 33
    Public WriteOnly Property TaskGroupTasks() As Object(,)
        Set(ByVal Value As Object(,))
            m_vTaskGroupTask = Value
        End Set
    End Property

    Public WriteOnly Property TaskGroupUsers() As Object(,)
        Set(ByVal Value As Object(,))
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

    Public WriteOnly Property CreditControlRuleID() As Integer
        Set(ByVal Value As Integer)
            m_lCreditControlRuleID = Value
        End Set
    End Property

    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    '' ***************************************************************** '
    '' Name: SetStatus (Standard Method)
    ''
    '' Description: Set the Process, Map and Step status.
    '' Note:        A Property Get is provided for the Step Status only
    ''              as this is the only one which this component can
    ''              alter directly.
    '' ***************************************************************** '
    ''Public Function SetStatus( _
    ''    sProcessStatus As String, _
    ''    sMapStatus As String, _
    ''    sStepStatus As String) As Long
    ''
    ''    On Error GoTo Err_SetStatus
    ''
    ''    SetStatus = PMTrue
    ''
    ''     Assign the current Status settings.
    ''    m_sProcessStatus$ = Trim$(sProcessStatus$)
    ''    m_sMapStatus$ = Trim$(sMapStatus$)
    ''    m_sStepStatus$ = Trim$(sStepStatus$)
    ''
    ''    Exit Function
    ''
    ''Err_SetStatus:
    ''
    ''     Error Section.
    ''
    ''    SetStatus = PMError
    ''
    ''     Log Error Message
    ''    LogMessage _
    ''            iType:=PMLogOnError, _
    ''          sMsg:="SetStatus Failed", _
    ''           vApp:=ACApp, _
    ''           vClass:=ACClass, _
    ''           vMethod:="SetStatus", _
    ''            vErrNo:=Err.Number, _
    ''           vErrDesc:=Err.Description
    ''
    ''    Exit Function
    ''
    ''End Function

    '' ***************************************************************** '
    '' Name: GetCompany (Standard Method)
    ''
    '' Description: Gets valid Source ID's and if nessessary displays selection
    ''
    '' ***************************************************************** '
    'Public Function GetCompany( _
    ''        ByRef m_iCompanyID As Integer) As Long
    '
    'Dim m_oBranch As Object
    '
    '    On Error GoTo Err_GetCompany
    '
    '    GetCompany = PMTrue
    '
    '    m_lReturn& = g_oObjectManager.GetInstance( _
    ''            oObject:=m_oBranch, _
    ''            sClassName:="iPMBBranch.Interface", _
    ''            vInstanceManager:=PMGetLocalInterface)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetCompany = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oBranch.GetSource(iSourceID:=m_iCompanyID)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetCompany = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oBranch.Terminate()
    '    Set m_oBranch = Nothing
    '    ' Check if we have had an error so far.
    '    If m_lReturn <> PMTrue Then
    '      GetCompany = PMError
    '      Exit Function
    '    End If
    '
    '    Exit Function
    '
    'Err_GetCompany:
    '
    '    ' Error Section
    '
    '    GetCompany = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to get Company", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetCompany", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

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
            m_lReturn = GetCreditControlRule()

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

            ' get credit control rule insurance_file_status
            m_lReturn = GetCreditControlRuleInsuranceFileStatuses()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCreditControlRuleInsuranceFileStatus Failed", gPMConstants.PMELogLevel.PMLogError)
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

            If (m_iUseEffectiveDate = 1) Then
                chkUseEffectiveDate.CheckState = CheckState.Checked
            Else
                chkRenewalPrintDate.CheckState = CheckState.Checked
            End If

            chkUseGreaterTransEffDate.CheckState = m_iUseGreaterTransEffDate

            chkUseDueDate.CheckState = m_iUseDueDate

            cboPMLookupSource.ItemId = m_lSourceId

            cboPMLookupPFFrequency.ItemId = IIf(m_lPFFrequencyID = "", 0, m_lPFFrequencyID)
            cboPMLookupInstalmentResult.RefreshList()
            cboPMLookupInstalmentResult.ItemId = m_lInstalmentResultId
            cboProduct.ItemId = m_lProductID
            chkUseInceptionDate.CheckState = m_oUseInceptionDate
            'Populate the Business Type Combobox and select correct item
            m_lReturn = SetBusinessType(m_sBusinessType)

            m_lReturn = PopulateStepListView()


            chkPaid.CheckState = CheckState.Unchecked
            chkUnpaid.CheckState = CheckState.Unchecked

            If m_lPolicyIsPaid = 1 Then
                chkPaid.CheckState = CheckState.Checked
            ElseIf m_lPolicyIsPaid = 0 Then
                chkUnpaid.CheckState = CheckState.Checked
            End If

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
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Const kMethodType As String = "InterfaceToBusiness"

        Dim lBusinessDataID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            If cboBusinessType.Text <> ACBusTypeINS And cboBusinessType.Text <> AcBusTypeINSC And cboBusinessType.Text <> AcBusTypeINSH Then
                ClearDownInstalmentSpecificFields()
            End If

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    m_lReturn = AddCreditControlRule()

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.
                    m_lReturn = EditCreditControlRule()

            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            m_lReturn = SetCreditControlRuleInsuranceFileStatuses()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodType, "SetCreditControlRuleInsuranceFileStatus Failed", gPMConstants.PMELogLevel.PMLogError)
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

                'Developer Guide No. 243
                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                'Developer Guide No. 243
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

            'Only need to set up defaults in add mode
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                'Set the default Source to that passed from interface screen
                cboPMLookupSource.ItemId = m_lSourceId

                m_lReturn = SetBusinessType("")

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

            'Start in the 'Details' tab
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            '    For iTab = ACTABSecurity To ACTABGenerateTask
            '        tabMainTab.TabVisible(iTab) = False
            '    Next iTab
            '
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
                'refer developer guide no. 170(Latest Guide)
                ListViewFunc.ListViewBatchStart(lvwSteps)

                'Populate the listview
                With lvwSteps.Items
                    'Loop through the results array and populate the listview
                    For lRow As Integer = lLower To lUpper

                        'add a new listitem to the listview

                        AddOrEditListViewItem(r_oListItem:=Nothing, r_vStepNumber:=CStr(m_vAllStepsArray(ACStepNumber, lRow)), r_vElapsedDays:=CStr(m_vAllStepsArray(ACElapsedDays, lRow)), r_vBrokerDays:=CStr(m_vAllStepsArray(ACBrokerDays, lRow)), r_vPolicyAmt:=CStr(m_vAllStepsArray(ACPolicyAmt, lRow)), r_vAccountAmt:=CStr(m_vAllStepsArray(ACAccountAmt, lRow)), r_vNextStep:=CStr(m_vAllStepsArray(ACNextStep, lRow)), r_vAutoCancel:=CStr(m_vAllStepsArray(ACAutoCancel, lRow)), r_lCreditControlStepId:=CInt(m_vAllStepsArray(ACCreditControlStepId, lRow)))

                    Next lRow

                End With

                'Turn on listview updating
                'refer developer guide no. 170(Latest Guide)
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

    Private Function SetBusinessType(ByVal v_sBusstype As String) As Integer
        Dim result As Integer = 0
        Dim iPos As Integer
        Dim vBusinessType() As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim Preserve vBusinessType(7)

            ' Set up business type array
            vBusinessType(0) = ACBusTypeBlank
            vBusinessType(1) = ACBusTypeMTA
            vBusinessType(2) = ACBusTypeNB
            vBusinessType(3) = ACBusTypeREN

            ReDim Preserve vBusinessType(8)

            vBusinessType(4) = ACBusTypeINS
            vBusinessType(5) = AcBusTypeINSC
            vBusinessType(6) = AcBusTypeINSH
            vBusinessType(7) = ACBusTypeRENUPD
            vBusinessType(8) = ACBusTypeTRANS

            cboBusinessType.Items.Clear()

            ' add business types
            For iLoop As Integer = 0 To vBusinessType.GetUpperBound(0)
                If vBusinessType(iLoop).Trim() <> "" Then
                    cboBusinessType.Items.Add(vBusinessType(iLoop))
                    If String.Compare(vBusinessType(iLoop), v_sBusstype.Trim()) = 0 Then
                        iPos = iLoop
                    End If
                End If
            Next

            cboBusinessType.SelectedIndex = iPos

            Return result

        Catch excep As System.Exception

            result = False
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate security list", vApp:=ACApp, vClass:=ACClass, vMethod:="SetBusinessType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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

    Private Sub chkPaid_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPaid.CheckStateChanged
        If chkPaid.CheckState = CheckState.Checked Then
            chkUnpaid.CheckState = CheckState.Unchecked
        End If
    End Sub

    Private Sub chkUnpaid_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkUnpaid.CheckStateChanged
        If chkUnpaid.CheckState = CheckState.Checked Then
            chkPaid.CheckState = CheckState.Unchecked
        End If
    End Sub

    Private Sub chkUseEffectiveDate_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkUseEffectiveDate.CheckStateChanged

        ParentChanged_Renamed(r_bChanged:=True)

        If (cboBusinessType.Text = ACBusTypeNB) Or (cboBusinessType.Text = ACBusTypeMTA) Or (cboBusinessType.Text = ACBusTypeREN) Then

            If chkUseEffectiveDate.Enabled And chkUseEffectiveDate.CheckState = CheckState.Checked Then
                chkUseGreaterTransEffDate.Visible = True
                chkUseDueDate.CheckState = CheckState.Unchecked
                chkUseDueDate.Visible = False
            ElseIf chkUseEffectiveDate.CheckState = CheckState.Unchecked Then
                chkUseDueDate.Visible = True
                chkUseGreaterTransEffDate.CheckState = CheckState.Unchecked
                chkUseGreaterTransEffDate.Visible = False
            End If

        End If
        If cboBusinessType.Text = ACBusTypeRENUPD Then
            If chkUseEffectiveDate.CheckState = CheckState.Checked Then
                chkRenewalPrintDate.CheckState = CheckState.Unchecked
            End If
        End If

    End Sub

    Private Sub chkUseInceptionDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseInceptionDate.CheckedChanged
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Sub chkUseDueDate_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkUseDueDate.CheckStateChanged

        ParentChanged_Renamed(r_bChanged:=True)

        If (cboBusinessType.Text = ACBusTypeNB) Or (cboBusinessType.Text = ACBusTypeMTA) Or (cboBusinessType.Text = ACBusTypeREN) Then

            If chkUseDueDate.Enabled And chkUseDueDate.CheckState = CheckState.Checked Then
                chkUseEffectiveDate.Enabled = False
            Else
                chkUseEffectiveDate.Enabled = True
            End If

        End If
    End Sub

    Private isInitializingComponent As Boolean

    Private Sub txtDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Sub cboPMLookupPFFrequency_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMLookupPFFrequency.Click
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub

    Private Sub cboBusinessType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBusinessType.SelectedIndexChanged
        ParentChanged_Renamed(r_bChanged:=True)
        chkRenewalPrintDate.Visible = False
        If (cboBusinessType.Text = ACBusTypeNB) Or (cboBusinessType.Text = ACBusTypeMTA) Or (cboBusinessType.Text = ACBusTypeREN) Or (cboBusinessType.Text = ACBusTypeTRANS) Then
            chkUseEffectiveDate.Enabled = True
            chkUseDueDate.Enabled = True
        ElseIf cboBusinessType.Text = ACBusTypeRENUPD Then
            chkUseEffectiveDate.Enabled = True
            chkRenewalPrintDate.Visible = True
            chkUseGreaterTransEffDate.CheckState = CheckState.Unchecked
            chkUseGreaterTransEffDate.Visible = False
            chkUseDueDate.Enabled = False
            chkUseDueDate.CheckState = CheckState.Unchecked
            If chkUseEffectiveDate.CheckState = CheckState.Unchecked And chkRenewalPrintDate.CheckState = CheckState.Unchecked Then
                chkUseEffectiveDate.CheckState = CheckState.Checked
            Else
                If chkUseEffectiveDate.CheckState = CheckState.Unchecked Then
                    chkRenewalPrintDate.CheckState = CheckState.Checked
                Else
                    chkUseEffectiveDate.CheckState = CheckState.Checked
                End If
            End If
        Else
            chkUseEffectiveDate.Enabled = False
            chkUseGreaterTransEffDate.CheckState = CheckState.Unchecked
            chkUseGreaterTransEffDate.Visible = False
            chkUseDueDate.Enabled = False
            chkUseDueDate.CheckState = CheckState.Unchecked
        End If
        If (cboBusinessType.Text = ACBusTypeNB) Or (cboBusinessType.Text = ACBusTypeREN) Or (cboBusinessType.Text = ACBusTypeTRANS) Then
            chkUseInceptionDate.Enabled = True
        Else
            chkUseInceptionDate.CheckState = CheckState.Unchecked
            chkUseInceptionDate.Enabled = False
        End If
    End Sub
    ''' <summary>
    ''' cmdAddStep_Click-Show the Credit Control Step form for Adding
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdAddStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddStep.Click

        Dim sTitle, sMessage As String
        Dim lCreditControlStepId As Integer
        Dim vElapsedDays As Object
        Dim lNextAvailableInstalmentFailureCount As Integer


        Try

            'Make sure a business type is selected
            If cboBusinessType.Text.Trim() = "" Then
                'Have to first select a business type.
                ' Get description from the resource file.

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoBusinessTypeTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoBusinessTypeDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub

            End If

            'Derive the document template list (not a PMLookup field yet)
            m_lReturn = GetDocTemplateList() 'm_vDocTemplateList

            ' get next available instalment failure count
            GetNextAvailableInstalmentFailureCount(lNextAvailableInstalmentFailureCount)
            ofrmStep = New frmStep()

            With ofrmStep

                .LookupTables = VB6.CopyArray(m_vLookupTables)
                .LookupDetails = VB6.CopyArray(m_vLookupDetails)
                .TaskGroupTasks = VB6.CopyArray(m_vTaskGroupTask)
                .TaskGroupUsers = VB6.CopyArray(m_vTaskGroupUserGroups)

                .Status = gPMConstants.PMEComponentAction.PMAdd
                .StepID = 0
                'refer developer guide no. 24
                .DocumentList = m_vDocTemplateList
                .ShowDocumentLists()
                .BusinessType = cboBusinessType.Text.Trim()

                .StepNumber = Nothing

                .ElapsedDays = Nothing

                .BrokerDays = Nothing

                .ClientLetterId = Nothing

                .ClientLetterId2 = Nothing

                .OIPLetterId = Nothing

                .OIPLetterId2 = Nothing

                .BrokerReportId = Nothing

                .BrokerLetterId = Nothing
                .BrokerLetterIdSingleInst = Nothing
                .PolicyAmt = Nothing

                .AccountAmt = Nothing

                .WrkManagerTaskId = Nothing

                .WrkManagerTaskId2 = Nothing

                .CheckAutoCancelRules = Nothing

                .RunAutoCancelRules = Nothing

                .CheckAutoLapseRenewal = Nothing

                .NextStep = Nothing

                .PreviousStep = Nothing

                .OffHoldStep = Nothing

                .RecurringDays = Nothing

                .Reprint = Nothing

                .JumpToNextStep = Nothing

                .StopAccount = Nothing
                .NextAvailableInstalmentFailureCount = lNextAvailableInstalmentFailureCount

                .InstalmentFailureCount = Nothing

                .WriteOffToleranceAmount = Nothing

                .AutoCancelDocumentTemplate1TriggerAmount = Nothing

                .AutoCancelDocumentTemplate2TriggerAmount = Nothing
                .JumpToNextStepBroker = Nothing

                .ShowDialog()

                'Determine action
                Select Case .Status
                    Case gPMConstants.PMEReturnCode.PMOK

                        'Write it to the database now
                        If m_sUniqueId = "" Then
                            m_sUniqueId = GetUniqueID()
                        End If

                        m_sScreenHierarchy = $"Credit Control({Me.cboPMLookupSource.ItemCaption}/{Me.txtDescription.Text})/Step({ .StepNumber})"

                        m_lReturn = m_oBusiness.DirectAddStep(r_vCreditControlStepID:=lCreditControlStepId, v_vCreditControlRuleID:=m_lCreditControlRuleID, v_vStepNumber:= .StepNumber, v_vNumberOfDays:= .ElapsedDays, v_vBrokerDays:= .BrokerDays, v_vClientDocumentTemplateID:= .ClientLetterId, v_vClientDocumentTemplateID2:= .ClientLetterId2, v_vOIPDocumentTemplateID:= .OIPLetterId, v_vOIPDocumentTemplateID2:= .OIPLetterId2, v_vBrokerReportID:= .BrokerReportId, v_vPolicyToleranceAmount:= .PolicyAmt, v_vAccountToleranceAmount:= .AccountAmt, v_vPMWrkTaskID:=ZeroToNull(.TaskId), v_vPMWrkTaskID2:=ZeroToNull(.WrkManagerTaskId2), v_vPMUserGroupID:=ZeroToNull(.UserGroupId), v_vPMUserGroupID2:=DBNull.Value, v_vActionType:=DBNull.Value, v_vActionType2:=DBNull.Value, v_vTolerancePercentage1:= .TolerancePercentage1, v_vTolerancePercentage2:= .TolerancePercentage2, v_vCheckAutoCancel:= .CheckAutoCancelRules, v_vAutoCancelPolicy:= .RunAutoCancelRules, v_vNextStep:= .NextStep, v_vPreviousStep:= .PreviousStep, v_vOffHoldStep:= .OffHoldStep, v_vRecurringDays:= .RecurringDays, v_vRecurringLetters:= .Reprint, v_vJumpToNextStep:= .JumpToNextStep, v_vStepDescription:= .StepDescription, v_vPMWrkTaskGroupId:=ZeroToNull(.TaskGroupId), v_vBrokerLetterId:= .BrokerLetterId, v_vStopAccount:= .StopAccount, v_vAutoLapseRenewal:= .CheckAutoLapseRenewal, v_vInstalmentFailureCount:= .InstalmentFailureCount, v_vAutoCancelDocument1TriggerAmount:= .AutoCancelDocumentTemplate1TriggerAmount, v_vAutoCancelDocument2TriggerAmount:= .AutoCancelDocumentTemplate2TriggerAmount, v_vAutoCancelDocument1TemplateId:= .AutoCancelDocumentTemplate1, v_vAutoCancelDocument2TemplateId:= .AutoCancelDocumentTemplate2, v_vWriteOffToleranceAmount:= .WriteOffToleranceAmount, v_vWriteOffReasonId:= .WriteOffReasonId, cJumpToNextStepBroker:= .JumpToNextStepBroker, cJumpToNextStepBrokerSingleInst:=(.JumpToNextStepBrokerSingleInst), cAccountNoofDaysSingleInst:=(.BrokerDaysSingleInst), cAccountToleranceAmountSingleInst:=(.AccountAmtSingleInst), cBrokerLetterIDSingleInst:=(.BrokerLetterIdSingleInst), v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

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

                        'developer guide no.98
                        m_lReturn = AddOrEditListViewItem(r_oListItem:=Nothing, r_vStepNumber:= .StepNumber, r_vElapsedDays:= .ElapsedDays, r_vBrokerDays:= .BrokerDays, r_vPolicyAmt:= .PolicyAmt, r_vAccountAmt:= .AccountAmt, r_vNextStep:= .NextStep, r_vAutoCancel:= .RunAutoCancelRules, r_lCreditControlStepId:=lCreditControlStepId)

                    Case gPMConstants.PMEReturnCode.PMCancel

                End Select

            End With



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddStep_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdApply_Click
        ' PURPOSE: Apply user changes to the database
        ' AUTHOR: Paul Cunningham
        ' DATE: 11 October 2002, 10:24:44
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------
        Dim iTab As Integer
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

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
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

            'Check that a Business type has been selected
            If cboBusinessType.SelectedIndex < 1 Then

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoBusTypeSelected, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                If Me.Visible Then
                    cboBusinessType.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Check that a frequency has been selected
            If cboPMLookupPFFrequency.ListIndex < 1 And VB6.GetItemString(cboBusinessType, cboBusinessType.SelectedIndex) = "INS" Then


                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoFrequencySelected, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                If Me.Visible Then
                    cboPMLookupPFFrequency.Focus()
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



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally



        End Try
        Exit Sub
    End Sub

    ''' <summary>
    ''' cmdDeleteSecurity_Click : Call business object to delete a CashList_Drawer_Security
    '''          record from the database
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks>AUTHOR: Paul Cunningham DATE: 08 October 2002, 15:03:56</remarks>
    Private Sub cmdDeleteStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteStep.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdDeleteSecurity_Click
        ' PURPOSE: Call business object to delete a CashList_Drawer_Security
        '          record from the database
        ' AUTHOR: Paul Cunningham
        ' DATE: 08 October 2002, 15:03:56
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
            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            m_sScreenHierarchy = $"Credit Control({Me.cboPMLookupSource.ItemCaption}/{Me.txtDescription.Text})/Step({ ListViewHelper.GetListViewSubItem(oListItem, 0).Text})"

            m_lReturn = m_oBusiness.DirectDeleteStep(v_lCreditControlStepID:=ListViewHelper.GetListViewSubItem(oListItem, 7).Text, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

            ' Check for errors
            'MsgBox "You cannot delete this step because it is in use.", vbExclamation, "Step in use"
            'Exit Sub
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                MessageBox.Show("You cannot delete this step because it is in use.", "Step in use", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                'Remove the item form the listview
                'Developer Guide No. 101
                lvwSteps.Items.Remove(oListItem)
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteStep_Click function failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteStep_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, sUsername:=g_oObjectManager.UserName, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
        Exit Sub
    End Sub
    ''' <summary>
    ''' Show the Credit Control Step form for Editing
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdEditStep_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditStep.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdEditStep_Click
        ' PURPOSE: Show the Credit Control Step form for Editing
        ' CHANGES:
        ' ---------------------------------------------------------------------------
        Dim sTitle, sMessage As String
        Dim oListItem As ListViewItem
        Dim lCreditControlStepId As Integer

        'Developer Guide No. 101
        Dim vStepNumber As Object
        Dim vNumberOfDays As Object
        Dim vBrokerDays As Object
        'developer guide no.101
        Dim vClientDocumentTemplateID As Object
        Dim vClientDocumentTemplateID2 As Object
        Dim vOIPDocumentTemplateID As Object
        Dim vOIPDocumentTemplateID2 As Object
        Dim vBrokerReportID As Object
        Dim vPolicyToleranceAmount As Object
        Dim vAccountToleranceAmount As Object
        Dim vPMWrkTaskID As Object
        Dim vPMWrkTaskID2 As Object
        Dim vPMUserGroupID As Object
        Dim vPMUserGroupID2, vActionType, vActionType2 As Object
        Dim vTolerancePercentage1 As Object
        Dim vTolerancePercentage2 As Object
        Dim vCheckAutoCancel As Object
        Dim vAutoCancelPolicy As Object
        Dim vNextStep As Object
        Dim vPreviousStep As Object
        Dim vOffHoldStep As Object
        Dim vRecurringDays As Object
        Dim vRecurringLetters As Object
        Dim vJumpToNextStep As Object
        Dim vElapsedDays As Object
        Dim vStepDescription As Object
        Dim vPMWrkTaskGroupId As Object
        Dim vBrokerLetterId As Object
        Dim vStopAccount As Object
        Dim vAutoLapseRenewal As Object
        Dim vInstalmentFailureCount As Object, lNextAvailableInstalmentFailureCount As Integer

        Dim vAutoCancelDoc1TriggerAmount As Object
        Dim vAutoCancelDoc2TriggerAmount As Object
        Dim vAutoCancelDoc1TemplateId As Object
        Dim vAutoCancelDoc2TemplateId As Object
        Dim vWriteOffTolerance As Object
        Dim vWriteOffReasonId As Object
        Dim cJumpToNextStepBroker As Integer
        Dim cJumpToNextStepBrokerSingleInst As Integer
        Dim cAccountNoofDaysSingleInst As Integer
        Dim cAccountToleranceAmountSingleInst As Integer
        Dim cBrokerLetterIDSingleInst As Integer


        Try

            'Make sure a business type is selected
            If cboBusinessType.Text.Trim() = "" Then
                'Have to first select a business type.
                ' Get description from the resource file.

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoBusinessTypeTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoBusinessTypeDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub

            End If

            'Only process if there is a selection made
            oListItem = lvwSteps.FocusedItem
            If oListItem Is Nothing Then
                DisplayMessage(r_lTitleId:=ACNoSelectionTitle, r_lMessageId:=ACNoSelectionDetails, r_lOptions:=MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'Derive the details of the step
            lCreditControlStepId = CInt(ListViewHelper.GetListViewSubItem(oListItem, 7).Text)

            ' get next available instalment failure count
            GetNextAvailableInstalmentFailureCount(lNextAvailableInstalmentFailureCount)


            m_lReturn = m_oBusiness.GetStepDetails(v_lCreditControlStepID:=lCreditControlStepId, r_vStepNumber:=vStepNumber, r_vNumberOfDays:=vNumberOfDays, r_vBrokerDays:=vBrokerDays, r_vClientDocumentTemplateID:=vClientDocumentTemplateID, r_vClientDocumentTemplateID2:=vClientDocumentTemplateID2, r_vOIPDocumentTemplateID:=vOIPDocumentTemplateID, r_vOIPDocumentTemplateID2:=vOIPDocumentTemplateID2, r_vBrokerReportID:=vBrokerReportID, r_vPolicyToleranceAmount:=vPolicyToleranceAmount, r_vAccountToleranceAmount:=vAccountToleranceAmount, r_vPMWrkTaskID:=vPMWrkTaskID, r_vPMWrkTaskID2:=vPMWrkTaskID2, r_vPMUserGroupID:=vPMUserGroupID, r_vPMUserGroupID2:=vPMUserGroupID2, r_vActionType:=vActionType, r_vActionType2:=vActionType2, r_vTolerancePercentage1:=vTolerancePercentage1, r_vTolerancePercentage2:=vTolerancePercentage2, r_vCheckAutoCancel:=vCheckAutoCancel, r_vAutoCancelPolicy:=vAutoCancelPolicy, r_vNextStep:=vNextStep, r_vPreviousStep:=vPreviousStep, r_vOffHoldStep:=vOffHoldStep, r_vRecurringDays:=vRecurringDays, r_vRecurringLetters:=vRecurringLetters, r_vJumpToNextStep:=vJumpToNextStep, r_vStepDescription:=vStepDescription, r_vPMWrkTaskGRoupId:=vPMWrkTaskGroupId, r_vBrokerLetterId:=vBrokerLetterId, r_vStopAccount:=vStopAccount, r_vAutoLapseRenewal:=vAutoLapseRenewal, r_vInstalmentFailureCount:=vInstalmentFailureCount, r_vAutoCancelDoc1Id:=vAutoCancelDoc1TemplateId, r_vAutoCancelDoc2Id:=vAutoCancelDoc2TemplateId, r_vAutoCancelDoc1TriggerAmount:=vAutoCancelDoc1TriggerAmount, r_vAutoCancelDoc2TriggerAmount:=vAutoCancelDoc2TriggerAmount, r_vWriteOffToleranceAmount:=vWriteOffTolerance, r_vWriteOffReasonId:=vWriteOffReasonId, o_nJumpToNextStepBroker:=cJumpToNextStepBroker, o_nJumpToNextStepBrokerSingleInst:=cJumpToNextStepBrokerSingleInst, o_nAccountNoofDaysSingleInst:=cAccountNoofDaysSingleInst, o_nAccountToleranceAmountSingleInst:=cAccountToleranceAmountSingleInst, o_nBrokerLetterIDSingleInst:=cBrokerLetterIDSingleInst)

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
                .StepID = lCreditControlStepId
                'refer developer guide no. 24
                .DocumentList = m_vDocTemplateList
                .ShowDocumentLists()
                .BusinessType = cboBusinessType.Text.Trim()
                .StepNumber = vStepNumber
                .ElapsedDays = vNumberOfDays
                .BrokerDays = vBrokerDays
                .ClientLetterId = vClientDocumentTemplateID
                .BrokerLetterId = vBrokerLetterId
                .ClientLetterId2 = vClientDocumentTemplateID2
                .OIPLetterId = vOIPDocumentTemplateID
                .OIPLetterId2 = vOIPDocumentTemplateID2
                .BrokerReportId = vBrokerReportID
                .PolicyAmt = vPolicyToleranceAmount
                .AccountAmt = vAccountToleranceAmount
                .WrkManagerTaskId = vPMWrkTaskID
                .WrkManagerTaskId2 = vPMWrkTaskID2
                .TolerancePercentage1 = vTolerancePercentage1
                .TolerancePercentage2 = vTolerancePercentage2
                .CheckAutoCancelRules = vCheckAutoCancel
                .RunAutoCancelRules = vAutoCancelPolicy
                .CheckAutoLapseRenewal = vAutoLapseRenewal
                .NextStep = vNextStep
                .PreviousStep = vPreviousStep
                .OffHoldStep = vOffHoldStep
                .RecurringDays = vRecurringDays
                .Reprint = vRecurringLetters
                .JumpToNextStep = vJumpToNextStep
                .StopAccount = vStopAccount
                If cboBusinessType.Text = "REN WTG UPDATE" Then

                    'refer developer guide no. 24
                    .CaptionRenWTGUpdate = True
                Else

                    'refer developer guide no. 24
                    .CaptionRenWTGUpdate = False
                End If
                .NextAvailableInstalmentFailureCount = lNextAvailableInstalmentFailureCount
                .InstalmentFailureCount = vInstalmentFailureCount

                .AutoCancelDocumentTemplate1 = gPMFunctions.ToSafeLong(vAutoCancelDoc1TemplateId, 0)
                .AutoCancelDocumentTemplate1TriggerAmount = vAutoCancelDoc1TriggerAmount
                .AutoCancelDocumentTemplate2 = gPMFunctions.ToSafeLong(vAutoCancelDoc2TemplateId, 0)
                .AutoCancelDocumentTemplate2TriggerAmount = vAutoCancelDoc2TriggerAmount
                .WriteOffToleranceAmount = vWriteOffTolerance
                .WriteOffReasonId = gPMFunctions.ToSafeLong(vWriteOffReasonId, 0)
                .JumpToNextStepBroker = gPMFunctions.ToSafeInteger(cJumpToNextStepBroker)
                .JumpToNextStepBrokerSingleInst = gPMFunctions.ToSafeInteger(cJumpToNextStepBrokerSingleInst)
                .BrokerDaysSingleInst = gPMFunctions.ToSafeInteger(cAccountNoofDaysSingleInst)
                .AccountAmtSingleInst = gPMFunctions.ToSafeInteger(cAccountToleranceAmountSingleInst)
                .BrokerLetterIdSingleInst = gPMFunctions.ToSafeInteger(cBrokerLetterIDSingleInst)

                .ShowDialog()

                'Determine action
                Select Case .Status
                    Case gPMConstants.PMEReturnCode.PMOK

                        'Write it to the database now
                        If m_sUniqueId = "" Then
                            m_sUniqueId = GetUniqueID()
                        End If

                        m_sScreenHierarchy = $"Credit Control({Me.cboPMLookupSource.ItemCaption}/{Me.txtDescription.Text})/Step({ .StepNumber})"

                        m_lReturn = m_oBusiness.DirectEditStep(v_vCreditControlStepID:=lCreditControlStepId, v_vCreditControlRuleID:=m_lCreditControlRuleID, v_vStepNumber:= .StepNumber, v_vNumberOfDays:= .ElapsedDays, v_vBrokerDays:= .BrokerDays, v_vClientDocumentTemplateID:= .ClientLetterId, v_vClientDocumentTemplateID2:= .ClientLetterId2, v_vOIPDocumentTemplateID:= .OIPLetterId, v_vOIPDocumentTemplateID2:= .OIPLetterId2, v_vBrokerReportID:= .BrokerReportId, v_vPolicyToleranceAmount:= .PolicyAmt, v_vAccountToleranceAmount:= .AccountAmt, v_vPMWrkTaskID:=ZeroToNull(.TaskId), v_vPMWrkTaskID2:=ZeroToNull(.WrkManagerTaskId2), v_vPMUserGroupID:=ZeroToNull(.UserGroupId), v_vPMUserGroupID2:=DBNull.Value, v_vActionType:=DBNull.Value, v_vActionType2:=DBNull.Value, v_vTolerancePercentage1:= .TolerancePercentage1, v_vTolerancePercentage2:= .TolerancePercentage2, v_vCheckAutoCancel:= .CheckAutoCancelRules, v_vAutoCancelPolicy:= .RunAutoCancelRules, v_vNextStep:= .NextStep, v_vPreviousStep:= .PreviousStep, v_vOffHoldStep:= .OffHoldStep, v_vRecurringDays:= .RecurringDays, v_vRecurringLetters:= .Reprint, v_vJumpToNextStep:= .JumpToNextStep, v_vStepDescription:= .StepDescription, v_vPMWrkTaskGroupId:=ZeroToNull(.TaskGroupId), v_vBrokerLetterId:= .BrokerLetterId, v_vStopAccount:= .StopAccount, v_vAutoLapseRenewal:= .CheckAutoLapseRenewal, v_vInstalmentFailureCount:= .InstalmentFailureCount, v_vAutoCancelDocument1TriggerAmount:= .AutoCancelDocumentTemplate1TriggerAmount, v_vAutoCancelDocument2TriggerAmount:= .AutoCancelDocumentTemplate2TriggerAmount, v_vAutoCancelDocument1TemplateId:= .AutoCancelDocumentTemplate1, v_vAutoCancelDocument2TemplateId:= .AutoCancelDocumentTemplate2, v_vWriteOffToleranceAmount:= .WriteOffToleranceAmount, v_vWriteOffReasonId:= .WriteOffReasonId, cJumpToNextStepBroker:=(.JumpToNextStepBroker), cJumpToNextStepBrokerSingleInst:=(.JumpToNextStepBrokerSingleInst), cAccountNoofDaysSingleInst:=(.BrokerDaysSingleInst), cAccountToleranceAmountSingleInst:=(.AccountAmtSingleInst), cBrokerLetterIDSingleInst:=(.BrokerLetterIdSingleInst), v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

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
                        m_lReturn = AddOrEditListViewItem(r_oListItem:=oListItem, r_vStepNumber:= .StepNumber, r_vElapsedDays:= .ElapsedDays, r_vBrokerDays:= .BrokerDays, r_vPolicyAmt:= .PolicyAmt, r_vAccountAmt:= .AccountAmt, r_vNextStep:= .NextStep, r_vAutoCancel:= .RunAutoCancelRules, r_lCreditControlStepId:=lCreditControlStepId)

                    Case gPMConstants.PMEReturnCode.PMCancel

                End Select

            End With



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditStep_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
        Exit Sub
    End Sub

    'Private Sub cmdHelp_Click()
    '' Fire up the help screen
    '    m_lReturn& = ShowHelp(dlgHelp, ScreenHelpID)
    'End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        Me.cboPMLookupPFFrequency.FirstItem = "(Null)"
        Me.cboPMLookupSource.FirstItem = ""
        Me.cboProduct.FirstItem = "(Any)"
        'task 3301
        Me.cboPMLookupInstalmentResult.FirstItem = "(None)"
        ' Forms initialise event.

        Try

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


    Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        '   Dim vValue As Object 'DD 28/05/2003
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

            m_lReturn = PopulateInsuranceFileStatusCheckedListBox()

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            If (m_sBusinessType = ACBusTypeNB) Or (m_sBusinessType = ACBusTypeMTA) Or (m_sBusinessType = ACBusTypeREN) Then
                chkUseGreaterTransEffDate.Visible = chkUseEffectiveDate.Enabled And chkUseEffectiveDate.CheckState = CheckState.Checked

                If chkUseEffectiveDate.Enabled And chkUseEffectiveDate.CheckState = CheckState.Checked Then
                    chkUseDueDate.CheckState = CheckState.Unchecked
                    chkUseDueDate.Visible = False
                End If

            End If

            cmdApply.Enabled = False
            If (cboBusinessType.Text = ACBusTypeNB) Or (cboBusinessType.Text = ACBusTypeREN) Or (cboBusinessType.Text = ACBusTypeTRANS) Then
                chkUseInceptionDate.Enabled = True
            Else
                chkUseInceptionDate.CheckState = CheckState.Unchecked
                chkUseInceptionDate.Enabled = False
            End If
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

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to terminate the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload")
            End If

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

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoBranchSelected, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Me.Visible Then
                    cboPMLookupSource.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub

            End If

            'Check that a Business type has been selected
            If cboBusinessType.SelectedIndex < 1 Then

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoBusTypeSelected, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                If Me.Visible Then
                    cboBusinessType.Focus()
                End If
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            If (VB6.GetItemString(cboBusinessType, cboBusinessType.SelectedIndex) = ACBusTypeINS) Or (VB6.GetItemString(cboBusinessType, cboBusinessType.SelectedIndex) = AcBusTypeINSC) Or (VB6.GetItemString(cboBusinessType, cboBusinessType.SelectedIndex) = AcBusTypeINSH) Then

                If chkUnpaid.CheckState = CheckState.Unchecked And chkPaid.CheckState = CheckState.Unchecked Then

                    ' Display message.
                    MessageBox.Show("A paid position must be specified.", "Information Missing", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                    chkPaid.Focus()
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Exit Sub
                End If

                'Check that a frequency has been selected
                If cboPMLookupPFFrequency.ListIndex < 1 Then


                    'Developer Guide No. 243
                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    'Developer Guide No. 243
                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoFrequencySelected, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    ' Display message.
                    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    If Me.Visible Then
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        cboPMLookupPFFrequency.Focus()
                    End If
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Exit Sub
                End If

            End If

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

    Private Function GetCreditControlRule() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetCreditControlRule
        ' PURPOSE: Get a GetCreditControlRule record form the database
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetRuleDetails(v_lCreditControlRuleId:=m_lCreditControlRuleID,
                                                   r_vDescription:=m_sDescription, r_vSourceID:=m_lSourceId,
                                                   r_vBusinessType:=m_sBusinessType,
                                                   r_vPFFrequencyID:=m_lPFFrequencyID,
                                                   r_vPFFrequencyDescription:=m_sPFFrequencyDescription,
                                                   r_vIsActive:=m_iIsActive, r_vProcessingDays:=m_lProcessingDays,
                                                   r_vUseEffectiveDate:=m_iUseEffectiveDate,
                                                   r_vUseGreaterTransEffDate:=m_iUseGreaterTransEffDate,
                                                   r_vPFInstalmentsResultId:=m_lInstalmentResultId,
                                                   r_vPolicyIsPaid:=m_lPolicyIsPaid, r_vProductID:=m_lProductID,
                                                   r_vUseDueDate:=m_iUseDueDate,
                                                   r_oUseInceptionDateForAutoCancel:=m_oUseInceptionDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get ComponentName details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlRule")

                Return result
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlRule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



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


            m_lReturn = m_oBusiness.GetStepList(v_lCreditControlRuleId:=m_lCreditControlRuleID, r_vResultArray:=m_vAllStepsArray)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to getSteplist details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSteps")

                Return result
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSteps", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



        End Try
        Return result
    End Function

    Private Function AddOrEditListViewItem(ByRef r_oListItem As ListViewItem, ByVal r_vStepNumber As String, ByVal r_vElapsedDays As String, ByVal r_vBrokerDays As String, ByVal r_vPolicyAmt As String, ByVal r_vAccountAmt As String, ByVal r_vNextStep As String, ByVal r_vAutoCancel As String, ByVal r_lCreditControlStepId As Integer) As Integer

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddOrEditListViewItem
        ' PURPOSE: Adds a ListItem to the Steps ListView or edits an existing one
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Convert.IsDBNull(r_vStepNumber) Or IsNothing(r_vStepNumber) Then
                r_vStepNumber = ""
            End If


            If Convert.IsDBNull(r_vElapsedDays) Or IsNothing(r_vElapsedDays) Then
                r_vElapsedDays = ""
            End If


            If Convert.IsDBNull(r_vBrokerDays) Or IsNothing(r_vBrokerDays) Then
                r_vBrokerDays = ""
            End If


            If Convert.IsDBNull(r_vPolicyAmt) Or IsNothing(r_vPolicyAmt) Then
                r_vPolicyAmt = ""
            End If


            If Convert.IsDBNull(r_vAccountAmt) Or IsNothing(r_vAccountAmt) Then
                r_vAccountAmt = ""
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
                .Name = "Key" & r_lCreditControlStepId ' Save the key so that it is easier to delete an item
                ListViewHelper.GetListViewSubItem(r_oListItem, 1).Text = r_vElapsedDays
                ListViewHelper.GetListViewSubItem(r_oListItem, 2).Text = r_vBrokerDays
                ListViewHelper.GetListViewSubItem(r_oListItem, 3).Text = r_vPolicyAmt
                ListViewHelper.GetListViewSubItem(r_oListItem, 4).Text = r_vAccountAmt
                ListViewHelper.GetListViewSubItem(r_oListItem, 5).Text = r_vNextStep
                ListViewHelper.GetListViewSubItem(r_oListItem, 6).Text = r_vAutoCancel
                ListViewHelper.GetListViewSubItem(r_oListItem, 7).Text = CStr(r_lCreditControlStepId)
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

    Private Function AddCreditControlRule() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddCreditControlRule
        ' PURPOSE: Adds a ComponentName record to the database
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lCreditControlRuleID As Integer
        'Developer Guide No. 101
        Dim vPolicyIsPaid As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Me.chkPaid.CheckState = CheckState.Unchecked And Me.chkUnpaid.CheckState = CheckState.Unchecked Then

                vPolicyIsPaid = Nothing
            ElseIf Me.chkPaid.CheckState = CheckState.Checked Then
                vPolicyIsPaid = 1
            ElseIf Me.chkUnpaid.CheckState = CheckState.Checked Then
                vPolicyIsPaid = 0
            End If

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            m_sScreenHierarchy = $"Credit Control({Me.cboPMLookupSource.ItemCaption}/{Me.txtDescription.Text})"

            m_lReturn = m_oBusiness.DirectAddRule(r_vCreditControlRuleID:=lCreditControlRuleID, v_vDescription:=Me.txtDescription.Text.Trim(),
                                                  v_vSourceID:=Me.cboPMLookupSource.ItemId, v_vBusinessType:=Me.cboBusinessType.Text,
                                                  v_vPFFrequencyID:=cboPMLookupPFFrequency.ItemId, v_vIsActive:=Me.chkActive.CheckState,
                                                  v_vProcessingDays:=Me.txtProcessingDays.Text, v_vUseEffectiveDate:=Me.chkUseEffectiveDate.CheckState,
                                                  v_vUseGreaterTranEffDate:=Me.chkUseGreaterTransEffDate.CheckState,
                                                  v_vPFInstalmentsResultId:=Me.cboPMLookupInstalmentResult.ItemId,
                                                  v_vPolicyIsPaid:=vPolicyIsPaid, v_vProductID:=cboProduct.ItemId, v_vUseDueDate:=Me.chkUseDueDate.CheckState,
                                                  oUseInceptionDate:=Me.chkUseInceptionDate.CheckState, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add Credit_Control_Rule details to the database", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCreditControlRule")

                'Exit Function
                Return result
            End If

            m_lCreditControlRuleID = lCreditControlRuleID



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCreditControlRule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function EditCreditControlRule() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: EditCreditControlRule
        ' PURPOSE: Updates changes to a CashList_Drawer record on the database
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vPolicyIsPaid As Object

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            If Me.chkPaid.CheckState = CheckState.Unchecked And Me.chkUnpaid.CheckState = CheckState.Unchecked Then
                vPolicyIsPaid = Nothing
            ElseIf Me.chkPaid.CheckState = CheckState.Checked Then
                vPolicyIsPaid = 1
            ElseIf Me.chkUnpaid.CheckState = CheckState.Checked Then
                vPolicyIsPaid = 0
            End If

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            m_sScreenHierarchy = $"Credit Control({Me.cboPMLookupSource.ItemCaption}/{Me.txtDescription.Text})"
            m_lReturn = m_oBusiness.DirectEditRule(v_vCreditControlRuleID:=m_lCreditControlRuleID, v_vDescription:=Me.txtDescription.Text.Trim(),
                                                   v_vSourceID:=Me.cboPMLookupSource.ItemId, v_vBusinessType:=Me.cboBusinessType.Text,
                                                   v_vPFFrequencyID:=IIf(cboPMLookupPFFrequency.ItemId = 0, DBNull.Value, cboPMLookupPFFrequency.ItemId),
                                                   v_vIsActive:=Me.chkActive.CheckState, v_vProcessingDays:=Me.txtProcessingDays.Text,
                                                   v_vUseEffectiveDate:=Me.chkUseEffectiveDate.CheckState, v_vUseGreaterTranEffDate:=Me.chkUseGreaterTransEffDate.CheckState,
                                                   v_vPFInstalmentsResultId:=Me.cboPMLookupInstalmentResult.ItemId, v_vPolicyIsPaid:=vPolicyIsPaid,
                                                   v_vProductID:=cboProduct.ItemId, v_vUseDueDate:=Me.chkUseDueDate.CheckState,
                                                   oUseInceptionDate:=Me.chkUseInceptionDate.CheckState, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update Credit_Control_Rule details to the database", vApp:=ACApp, vClass:=ACClass, vMethod:="EditCreditControlRule")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="EditCreditControlRule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
            Return result

        End Try

    End Function

    Private Sub ParentChanged_Renamed(ByRef r_bChanged As Boolean)
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ParentChanged
        ' PURPOSE: Sets flag to indicate whether user has changed parent record
        '          (Also enables / disables Apply command)
        ' CHANGES:
        ' ---------------------------------------------------------------------------

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

            lblPMLookupPFFrequency.Visible = (cboBusinessType.Text = ACBusTypeINS) Or (cboBusinessType.Text = AcBusTypeINSC) Or (cboBusinessType.Text = AcBusTypeINSH)
            cboPMLookupPFFrequency.Visible = (cboBusinessType.Text = ACBusTypeINS) Or (cboBusinessType.Text = AcBusTypeINSC) Or (cboBusinessType.Text = AcBusTypeINSH)

            SSTabHelper.SetTabVisible(tabMainTab, 1, (cboBusinessType.Text = ACBusTypeINS) Or (cboBusinessType.Text = AcBusTypeINSC) Or (cboBusinessType.Text = AcBusTypeINSH))




            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ParentChanged", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
        Exit Sub
    End Sub

    Private Function GetDocTemplateList() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetDocTemplateList
        ' PURPOSE: Get list of Document_Template records
        ' CHANGES:
        ' ---------------------------------------------------------------------------

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



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplateList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



        End Try
        Return result
    End Function

    Private Sub txtProcessingDays_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProcessingDays.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ParentChanged_Renamed(r_bChanged:=True)
    End Sub
    'Developer Guide No/ 101
    Private Function ZeroToNull(ByRef vValue As Object) As Object
        ZeroToNull = vValue

        If Not IsDBNull(vValue) Then
            If vValue = 0 Then
                ZeroToNull = DBNull.Value
            End If
        End If

    End Function

    ' ***************************************************************** '
    ' Name: PopulateInsuranceFileStatusCheckedListBox
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Function PopulateInsuranceFileStatusCheckedListBox() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateInsuranceFileStatusCheckedListBox"

        Dim lReturn, llBound, lUBound As Integer
        Dim Description As String = ""
        Dim Id As Integer
        Dim InsuranceFileStatusListItem As ListViewItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            chklbInsuranceFileStatus.Items.Clear()

            If Information.IsArray(m_vValidInsuranceFileStatuses) Then

                llBound = m_vValidInsuranceFileStatuses.GetLowerBound(1)
                lUBound = m_vValidInsuranceFileStatuses.GetUpperBound(1)

                For lItem As Integer = llBound To lUBound

                    ' get the required insurance file status details
                    Description = CStr(m_vValidInsuranceFileStatuses(2, lItem))

                    Id = CInt(m_vValidInsuranceFileStatuses(0, lItem))

                    ' populate the list box
                    chklbInsuranceFileStatus.Items.Add(New ListBoxItem(ToSafeString(Description), ToSafeString(Id)))
                Next

            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetCreditControlRuleInsuranceFileStatuses
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Function GetCreditControlRuleInsuranceFileStatuses() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCreditControlRuleInsuranceFileStatuses"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vCreditControlRuleInsuranceFileStatuses(,) As Object
        Dim llBound, lUBound, NoOfListItems, Id As Integer


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected insurance file statuses

            lReturn = m_oBusiness.GetCreditControlRuleInsuranceFileStatuses(m_lCreditControlRuleID, vCreditControlRuleInsuranceFileStatuses)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bACTCreditControl.Business.GetCreditControlRuleInsuranceFileStatuses Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if some insurance file statuses have been selected
            If Information.IsArray(vCreditControlRuleInsuranceFileStatuses) Then


                llBound = vCreditControlRuleInsuranceFileStatuses.GetLowerBound(1)

                lUBound = vCreditControlRuleInsuranceFileStatuses.GetUpperBound(1)

                For lItem As Integer = llBound To lUBound

                    ' get already selected details

                    Id = CInt(vCreditControlRuleInsuranceFileStatuses(0, lItem))

                    NoOfListItems = chklbInsuranceFileStatus.Items.Count

                    ' for each item in the listbox
                    For ListItem As Integer = 0 To NoOfListItems - 1

                        ' if the list item matches a selected one
                        If ToSafeInteger(chklbInsuranceFileStatus.Items(ListItem).ItemData) = Id Then

                            ' select it
                            chklbInsuranceFileStatus.SetItemChecked(ListItem, True)
                            Exit For
                        End If
                    Next

                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetCreditControlRuleInsuranceFileStatuses
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Function SetCreditControlRuleInsuranceFileStatuses() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetCreditControlRuleInsuranceFileStatuses"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vInsuranceFileStatuses() As Object
        Dim lNoOfListItems, lSelectedCount, lArrayItem As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lSelectedCount = chklbInsuranceFileStatus.CheckedIndices.Count
            If lSelectedCount > 0 Then

                ' resize array
                ReDim vInsuranceFileStatuses(lSelectedCount - 1)

                lArrayItem = 0

                ' build the insurance file status array
                lNoOfListItems = chklbInsuranceFileStatus.Items.Count

                ' for each insurance file status item in the list
                For lItem As Integer = 0 To lNoOfListItems - 1

                    ' if the item is selected
                    If chklbInsuranceFileStatus.GetItemChecked(lItem) Then

                        ' add it to the array

                        vInsuranceFileStatuses(lArrayItem) = ToSafeInteger(chklbInsuranceFileStatus.Items(lItem).ItemData)

                        lArrayItem += 1

                    End If

                Next


                lReturn = m_oBusiness.AddCreditControlInsuranceFileStatuses(m_lCreditControlRuleID, vInsuranceFileStatuses, m_sUniqueId, m_sScreenHierarchy)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bACTCreditControl.Business.AddCreditControlInsuranceFileStatuses Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else

                ' if there are no selected ensure any already saved in the database are removed

                lReturn = m_oBusiness.DeleteCreditControlRuleInsuranceFileStatus(m_lCreditControlRuleID, m_sUniqueId, m_sScreenHierarchy)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "DeleteCreditControlRuleInsuranceFileStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: ClearDownInstalmentSpecificFields
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Sub ClearDownInstalmentSpecificFields()

        Const kMethodName As String = "ClearDownInstalmentSpecificFields"

        Dim lReturn, lSubValue As Integer

        Try



            ' clear down the instalment result if the selected business type doesnt support it
            ' clear down the frequency if the selected business type doesnt support it
            If (cboBusinessType.Text <> ACBusTypeINS) And (cboBusinessType.Text <> AcBusTypeINSC) And (cboBusinessType.Text <> AcBusTypeINSH) Then

                cboPMLookupPFFrequency.ListIndex = -1
                cboPMLookupInstalmentResult.ListIndex = -1

                m_lPolicyIsPaid = 0

                For lItem As Integer = 0 To chklbInsuranceFileStatus.Items.Count - 1
                    chklbInsuranceFileStatus.SetItemChecked(lItem, False)
                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: GetNextAvailableInstalmentFailureCount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Function GetNextAvailableInstalmentFailureCount(ByRef r_lNextInstalmentFailureCount As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNextAvailableInstalmentFailureCount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lNext As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetNextAvailableInstalmentFailureCount(m_lCreditControlRuleID, r_lNextInstalmentFailureCount)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bACTCreditControl.Business.GetNextAvailableInstalmentFailureCount", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Private Sub _tabMainTab_TabPage0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _tabMainTab_TabPage0.Click

    End Sub

    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
    End Sub
    'task 3301
    Public Sub PopulatePaymentFailedReasonDropdown()
        cboPMLookupPFFrequency.ItemId = IIf(m_lPFFrequencyID = "", 0, m_lPFFrequencyID)
        cboPMLookupInstalmentResult.RefreshList()
        cboPMLookupInstalmentResult.ItemId = m_lInstalmentResultId
        cboProduct.ItemId = m_lProductID
    End Sub
    Private Sub chkRenewalPrintDate_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRenewalPrintDate.CheckStateChanged
        If chkRenewalPrintDate.CheckState = CheckState.Checked Then
            chkUseEffectiveDate.CheckState = CheckState.Unchecked
        End If
    End Sub


End Class