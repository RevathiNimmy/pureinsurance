Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class frmMIDRuleConfiguration
    Inherits System.Windows.Forms.Form

#Region "Constants"

    Public Const vbFormCode As Integer = 0

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmMIDRuleConfiguration"

    Private Const ACYesChar As String = "Y"

#End Region

#Region "Public Varible"

    'Let's caller know if changes were applied
    Public Applied As Boolean

    ''' <summary>
    ''' Variable specificaaly used to validate a rule
    ''' aginst all rules for a branch to avoid duplicacy
    ''' </summary>
    ''' <remarks></remarks>
    Public m_aoRulesArray(,) As Object

#End Region

#Region "Private Variables"

    Private m_sDescription As String = ""
    Private m_nSourceId As Integer
    Private m_nMIDRuleSourceId As Integer
    Private m_sBusinessType As String = ""
    Private m_nIsActive As CheckState
    Private m_oProcessingDays As Object
    'Private m_nUseEffectiveDate As CheckState
    'Private m_iUseDueDate As CheckState
    Private m_sPrevTaskGroup As String = ""

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_nStatus As Integer
    Private m_nErrorNumber As Integer
    Private m_nTask As gPMConstants.PMEComponentAction
    Private m_nNavigate As Integer
    Private m_nProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As String
    Private m_sMapStatus As String
    Private m_sStepStatus As String

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRMIDMaintenance.General
    Private m_oMIDRule As iSIRMIDMaintenance.frmMIDRules

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    ' Form control
    Private m_oFormfields As Object

    ' Variables to store the lookup values/details.
    Private m_oLookupValues As Object
    Private m_aoLookupDetails(,) As Object
    Private m_aoDocTemplateList(,) As Object
    Private m_aoTaskGroupUserGroups(,) As Object
    Private m_aoTaskGroupTask(,) As Object
    Private m_aoLookupTables(,) As Object
    Private m_aoValidInsuranceFileStatuses(,) As Object
    Private m_nPolicyIsPaid As Integer
    Private m_nProductID As Integer

    Private m_nMIDRuleID As Integer
    Private m_sMIDRuleCode As String
    Private m_sMIDRuleDescription As String
    Private m_dtMIDruleEffectiveDate As Date
    Private m_dtMIDruleExpiryDate As Date
    Private m_dtMIDruleStartDate As Date
    Private m_sMIDRuleMIDType As String
    Private m_nMIDRuleSupplierID As Integer
    Private m_nMIDRuleSupplierTypeID As Integer
    Private m_nMIDRuleInsurerID As Integer
    Private m_nMIDRuleDelegateAuthorityID As Integer
    Private m_nMIDRuleSitenumber As Integer
    Private m_nMIDRuleUserGroupId As Integer
    Private m_nMIDRuleTaskGroupId As Integer
    Private m_sMIDRuleFileName As String
    Private m_bMIDRuleTestIndicator As Boolean
    Private m_sMIDRuleFileSequenceNumberStart As String
    Private m_sMIDRuleCurrenctFileSequenceNumber As String
    Private m_sMIDRuleTaskGroupCode As String
    Private m_sMIDRuleUserGroupCode As String
    Private m_sMIDRuleSupplierCode As String

#End Region

#Region "Public Properties"

    Public WriteOnly Property LookupTables() As Object(,)
        Set(ByVal Value As Object(,))
            m_aoLookupTables = Value
        End Set
    End Property
    Public WriteOnly Property LookupDetails() As Object(,)
        Set(ByVal Value As Object(,))
            m_aoLookupDetails = Value
        End Set
    End Property
    Public WriteOnly Property TaskGroupTasks() As Object(,)
        Set(ByVal Value As Object(,))
            m_aoTaskGroupTask = Value
        End Set
    End Property

    Public WriteOnly Property TaskGroupUsers() As Object(,)
        Set(ByVal Value As Object(,))
            m_aoTaskGroupUserGroups = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_nErrorNumber
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_nStatus
        End Get
        Set(ByVal Value As Integer)
            ' Set the interface exit status.
            m_nStatus = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get
            ' Return the objects task.
            Return m_nTask
        End Get
        Set(ByVal Value As Integer)
            ' Set the objects task.
            m_nTask = Value
        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Set the navigate flag.
            m_nNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the process mode.
            m_nProcessMode = Value
        End Set
    End Property
    Public WriteOnly Property DefaultSourceID() As Integer
        Set(ByVal Value As Integer)
            m_nSourceId = Value
            m_nMIDRuleSourceId = Value
        End Set
    End Property


    Public Property MIDRuleID() As Integer
        Get
            Return m_nMIDRuleID
        End Get
        Set(value As Integer)
            m_nMIDRuleID = value
        End Set
    End Property
    Public Property MIDRuleSourceID() As Integer
        Get
            Return m_nMIDRuleSourceId
        End Get
        Set(value As Integer)
            m_nMIDRuleSourceId = value
        End Set
    End Property
    Public Property MIDRuleCode() As String
        Get
            Return m_sMIDRuleCode
        End Get
        Set(value As String)
            m_sMIDRuleCode = value
        End Set
    End Property

    Public Property MIDRuleDescription() As String
        Get
            Return m_sMIDRuleDescription
        End Get
        Set(value As String)
            m_sMIDRuleDescription = value
        End Set
    End Property
    Public Property MIDRuleEffectiveDate() As DateTime
        Get
            Return m_dtMIDruleEffectiveDate
        End Get
        Set(value As DateTime)
            m_dtMIDruleEffectiveDate = value
        End Set
    End Property
    Public Property MIDRuleExpiryDate() As DateTime
        Get
            Return m_dtMIDruleExpiryDate
        End Get
        Set(value As DateTime)
            m_dtMIDruleExpiryDate = value
        End Set
    End Property

    Public Property MIDRuleStartDate() As DateTime
        Get
            Return m_dtMIDruleStartDate
        End Get
        Set(value As DateTime)
            m_dtMIDruleStartDate = value
        End Set
    End Property
    Public Property MIDRuleMIDType() As String
        Get
            Return m_sMIDRuleMIDType
        End Get
        Set(value As String)
            m_sMIDRuleMIDType = value
        End Set
    End Property
    Public Property MIDRuleSupplierTypeID() As Integer
        Get
            Return m_nMIDRuleSupplierTypeID
        End Get
        Set(value As Integer)
            m_nMIDRuleSupplierTypeID = value
        End Set
    End Property
    Public Property MIDRuleSupplierTypeCode() As String
        Get
            Return m_sMIDRuleSupplierCode
        End Get
        Set(value As String)
            m_sMIDRuleSupplierCode = value
        End Set
    End Property
    Public Property MIDRuleSupplierID() As Integer
        Get
            Return m_nMIDRuleSupplierID
        End Get
        Set(value As Integer)
            m_nMIDRuleSupplierID = value
        End Set
    End Property

    Public Property MIDRuleInsurerID() As Integer
        Get
            Return m_nMIDRuleInsurerID
        End Get
        Set(value As Integer)
            m_nMIDRuleInsurerID = value
        End Set
    End Property

    Public Property MIDRuleDelegateAuthorityID() As Integer
        Get
            Return m_nMIDRuleDelegateAuthorityID
        End Get
        Set(value As Integer)
            m_nMIDRuleDelegateAuthorityID = value
        End Set
    End Property

    Public Property MIDRuleSitenumber() As Integer
        Get
            Return m_nMIDRuleSitenumber
        End Get
        Set(value As Integer)
            m_nMIDRuleSitenumber = value
        End Set
    End Property
    Public Property MIDRuleUserGroupId() As Integer
        Get
            Return m_nMIDRuleUserGroupId
        End Get
        Set(value As Integer)
            m_nMIDRuleUserGroupId = value
        End Set
    End Property
    Public Property MIDRuleUserGroupCode() As String
        Get
            Return m_sMIDRuleUserGroupCode
        End Get
        Set(value As String)
            m_sMIDRuleUserGroupCode = value
        End Set
    End Property

    Public Property MIDRuleTaskGroupId() As Integer
        Get
            Return m_nMIDRuleTaskGroupId
        End Get
        Set(value As Integer)
            m_nMIDRuleTaskGroupId = value
        End Set
    End Property
    Public Property MIDRuleTaskGroupCode() As String
        Get
            Return m_sMIDRuleTaskGroupCode
        End Get
        Set(value As String)
            m_sMIDRuleTaskGroupCode = value
        End Set
    End Property
    Public Property MIDRuleFileName() As String
        Get
            Return m_sMIDRuleFileName
        End Get
        Set(value As String)
            m_sMIDRuleFileName = value
        End Set
    End Property
    Public Property MIDRuleTestIndicator() As Boolean
        Get
            Return m_bMIDRuleTestIndicator
        End Get
        Set(value As Boolean)
            m_bMIDRuleTestIndicator = value
        End Set
    End Property
    Public Property MIDRuleFileSequenceNumberStart() As String
        Get
            Return m_sMIDRuleFileSequenceNumberStart
        End Get
        Set(value As String)
            m_sMIDRuleFileSequenceNumberStart = value
        End Set
    End Property
    Public Property MIDRuleCurrenctFileSequenceNumber() As String
        Get
            Return m_sMIDRuleCurrenctFileSequenceNumber
        End Get
        Set(value As String)
            m_sMIDRuleCurrenctFileSequenceNumber = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Entry point for any initialisation code for this object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise() As Integer

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting the interface.
            m_nStatus = gPMConstants.PMEReturnCode.PMCancel
            nResult = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If
        Catch Excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            m_nErrorNumber = Information.Err().Number
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", Excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Updates all interface details from the business object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BusinessToInterface() As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            Select Case (Task)
                Case gPMConstants.PMEComponentAction.PMAdd
                    txtEffectiveDate.Text = DateTime.Today.ToShortDateString
                Case gPMConstants.PMEComponentAction.PMEdit
                    txtCode.Text = MIDRuleCode
                    txtCode.Enabled = False
                    txtDescription.Text = m_sMIDRuleDescription
                    txtEffectiveDate.Text = MIDRuleEffectiveDate
                    txtExpiryDate.Text = MIDRuleExpiryDate
                    txtStartDate.Text = MIDRuleStartDate

                    txtSupplierID.Text = MIDRuleSupplierID
                    txtInsurerID.Text = MIDRuleInsurerID

                    txtSiteNumber.Text = IIf(MIDRuleSitenumber = 0, String.Empty, MIDRuleSitenumber)
                    txtDelegatedAuthorityID.Text = IIf(MIDRuleDelegateAuthorityID = 0, String.Empty, MIDRuleDelegateAuthorityID)

                    txtFileName.Text = MIDRuleFileName
                    txtFileSequenceNumberStart.Enabled = False
                    txtFileSequenceNumberStart.Text = MIDRuleFileSequenceNumberStart
                    txtCurrentfileSequenceNumber.Text = MIDRuleCurrenctFileSequenceNumber

                    cboMIDType.SelectedValue = MIDRuleMIDType
                    cboTaskGroup.Text = MIDRuleTaskGroupCode
                    cboUserGroup.Text = MIDRuleUserGroupCode
                    cboSupplierType.ItemId = MIDRuleSupplierTypeID

                    chkTestIndicator.Checked = MIDRuleTestIndicator

                    AllowDelegatedAuthrityID()
            End Select

        Catch Excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            m_nErrorNumber = Information.Err().Number
            'Log Error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", excep:=Excep)
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Updates all business members from the interface details.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InterfaceToBusiness() As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Check the task.
            Select Case (m_nTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    nResult = AddorEditMIDRule(0, PMEComponentAction.PMAdd)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.
                    nResult = AddorEditMIDRule(MIDRuleID, PMEComponentAction.PMEdit)
            End Select

            ' Check for errors.
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

        Catch Excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", Excep:=Excep)
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Get task creation lookup data - task groups / tasks / pmusergroups
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTaskCreationLookupData() As Integer

        Const KMethodName As String = "GetTaskCreationLookupData"
        Dim nReturn As gPMConstants.PMEReturnCode
        Try
            nReturn = gPMConstants.PMEReturnCode.PMTrue

            ' get all effective pmwrk_tasks for all effective pwrktaskgroups
            nReturn = m_oBusiness.GetALLPMWrkTaskGroupTasks(r_oaResults:=m_aoTaskGroupTask)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(KMethodName, "bSIRMIDMAIntenance.GetALLPMWrkTaskGroupTasks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get all effective pmusergroups for all effective pwrktaskgroups
            nReturn = m_oBusiness.GetALLPMWrkTaskGroupPMUserGroups(r_aoResults:=m_aoTaskGroupUserGroups)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(KMethodName, "bSIRMIDMAIntenance.GetALLPMWrkTaskGroupPMUserGroups Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get lookup data for task groups
            nReturn = GetLookups()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(KMethodName, "GetLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        Catch Excep As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=KMethodName, r_lFunctionReturn:=nReturn, excep:=Excep)
            m_nErrorNumber = Information.Err().Number
            nReturn = PMEReturnCode.PMError
        End Try

        Return nReturn
    End Function

    ''' <summary>
    ''' Set Validations for Rule fields
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFieldValidation() As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Const kMethodName As String = "SetFieldValidation"
        Try

            ' Pass control and required settings to FormControl
            nResult = m_oFormfields.AddNewFormField(ctlControl:=txtCode, lFieldType:=PMEDataType.PMString, lMandatory:=PMEMandatoryStatus.PMMandatory)
            txtCode.MaxLength = 10
            nResult = m_oFormfields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=PMEDataType.PMString, lMandatory:=PMEMandatoryStatus.PMMandatory)
            txtDescription.MaxLength = 255

            'Error checking
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Expiry Date Date
            nResult = m_oFormfields.AddNewFormField(ctlControl:=txtExpiryDate, lFieldType:=PMEDataType.PMDate, lFormat:=PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "txtExpiryDate - AddNewFormField Failed ")
            End If

            'Effective Date
            nResult = m_oFormfields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=PMEDataType.PMDate, lFormat:=PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "txtEffectiveDate - AddNewFormField Failed ")
            End If

            'start Date
            nResult = m_oFormfields.AddNewFormField(ctlControl:=txtStartDate, lFieldType:=PMEDataType.PMDate, lFormat:=PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "txtStartDate - AddNewFormField Failed ")
            End If

            'Supplier ID
            nResult = m_oFormfields.AddNewFormField(ctlControl:=txtSupplierID, lFieldType:=PMEDataType.PMInteger, lFormat:=PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            txtSupplierID.MaxLength = 3

            'Insurer ID
            nResult = m_oFormfields.AddNewFormField(ctlControl:=txtInsurerID, lFieldType:=PMEDataType.PMInteger, lFormat:=PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            txtInsurerID.MaxLength = 3

            'SiteNumber
            nResult = m_oFormfields.AddNewFormField(ctlControl:=txtSiteNumber, lFieldType:=PMEDataType.PMInteger, lFormat:=PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            txtSiteNumber.MaxLength = 3

            'Delegated Authority ID
            txtDelegatedAuthorityID.MaxLength = 3

            'file Sequence number
            nResult = m_oFormfields.AddNewFormField(ctlControl:=txtFileSequenceNumberStart, lFieldType:=PMEDataType.PMInteger, lFormat:=PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            txtFileSequenceNumberStart.MaxLength = 6

        Catch Excep As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=Excep)
            m_nErrorNumber = Information.Err().Number
            nResult = PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Sets all of the interface default values.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        ' Center the interface.
        iPMFunc.CenterForm(Me)

        ' populate lookup combos
        nResult = PopulateLookups()
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("SetInterfaceDefaults ", "PopulateLookups Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        nResult = SetupTaskCombos()
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("SetInterfaceDefaults ", "SetupTaskCombos Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        cboSupplierType.RefreshList()
        Return nResult

    End Function

    ''' <summary>
    ''' set values imn task grp combo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetupTaskCombos() As Integer
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim nTaskGroupId As Integer

        ' get task group id
        If cboTaskGroup.Text <> "" Then
            nResult = GetLookupItem(sLookupTable:=ACLookupTablePMWrkTaskGroup, sItemDesc:=cboTaskGroup.Text, r_sItemCode:="", r_nItemId:=nTaskGroupId)
        End If

        ' if a new item has been selected or onload no item has yet been selected
        If m_sPrevTaskGroup <> cboTaskGroup.Text Or m_sPrevTaskGroup = "" Then

            If cboTaskGroup.Text <> "" Then
                ' populate user groups
                If PopulateUserGroupscbo(nTaskGroupId:=nTaskGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                cboUserGroup.Items.Clear()
                cboUserGroup.Enabled = False
            End If
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get Lookup item vales
    ''' </summary>
    ''' <param name="sLookupTable"></param>
    ''' <param name="sItemDesc"></param>
    ''' <param name="r_sItemCode"></param>
    ''' <param name="r_nItemId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLookupItem(ByVal sLookupTable As String, ByVal sItemDesc As String, _
                                   ByRef r_sItemCode As String, ByRef r_nItemId As Integer) As Integer

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim nRow As Integer
        Dim bFoundMatch As Boolean
        Dim sCode As String = ""
        Dim nlBound, nUBound As Integer
        Dim sLookupItem As String = ""
        Dim nLookupItem As Integer
        Const sFunctionName As String = "GetLookupItem"
        Try
            ' Lookup value contants.
            Const kACValueTableName As Integer = 0
            '  Const ACValueID As Integer = 1
            Const kACValueStartPos As Integer = 2
            Const kACValueNumber As Integer = 3

            Const kACDetailKey As Integer = 0
            Const kACDetailDesc As Integer = 1
            Const kACDetailCode As Integer = 2

            ' Initilisation
            bFoundMatch = False
            For nRow = m_aoLookupTables.GetLowerBound(1) To m_aoLookupTables.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_aoLookupTables(kACValueTableName, nRow)).Trim() = sLookupTable.Trim() Then
                    bFoundMatch = True
                    Exit For
                End If
            Next nRow

            If bFoundMatch Then
                ' get array boundaries for specified table
                nlBound = CInt(m_aoLookupTables(kACValueStartPos, nRow))
                nUBound = CInt((CDbl(m_aoLookupTables(kACValueStartPos, nRow)) + CDbl(m_aoLookupTables(kACValueNumber, nRow))) - 1)

                ' set lookup properties
                If r_nItemId <> 0 Then
                    sLookupItem = CStr(r_nItemId)
                    nLookupItem = 0
                ElseIf sItemDesc <> "" Then
                    sLookupItem = sItemDesc
                    nLookupItem = 1
                ElseIf r_sItemCode <> "" Then
                    sLookupItem = r_sItemCode
                    nLookupItem = 2
                End If

                ' loop around the available items for the specified table
                For lCntr As Integer = nlBound To nUBound
                    ' get the code for the specified lookup items key
                    If CStr(m_aoLookupDetails(nLookupItem, lCntr)).Trim() = sLookupItem Then
                        ' return the requested code, id, description
                        sItemDesc = CStr(m_aoLookupDetails(kACDetailDesc, lCntr)).Trim()
                        r_sItemCode = CStr(m_aoLookupDetails(kACDetailCode, lCntr)).Trim()
                        r_nItemId = CInt(CStr(m_aoLookupDetails(kACDetailKey, lCntr)).Trim())

                        Exit For
                    End If
                Next lCntr
            End If
            ' if we dont find the code then log an error
            If r_sItemCode = "" Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("r_sItemCode", r_sItemCode)
                oDict.Add("r_lItemId", r_nItemId)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find code for lookuptable:" & sLookupTable & "and lookup Item:" & sLookupItem, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            End If
        Catch Excep As Exception
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_sItemCode", r_sItemCode)
            oDict.Add("r_lItemId", r_nItemId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=Excep, oDicParms:=oDict)
            m_nErrorNumber = Information.Err().Number
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' populate user group combo
    ''' </summary>
    ''' <param name="nTaskGroupId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateUserGroupscbo(ByVal nTaskGroupId As Integer) As Integer

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim nIndex, nlBound, nUBound, n_TaskGroupId, nUserGroupId As Integer
        Dim sUserGroup As String = String.Empty
        Const kFunctionName As String = "PopulateUserGroupscbo"
        Try
            cboUserGroup.Items.Clear()
            nIndex = 0

            ' if we have a selected task group
            If cboTaskGroup.Text <> "" Then
                ' if we have an array
                If Information.IsArray(m_aoTaskGroupUserGroups) Then
                    ' get array boundaries
                    nlBound = m_aoTaskGroupUserGroups.GetLowerBound(1)
                    nUBound = m_aoTaskGroupUserGroups.GetUpperBound(1)

                    ' for each item in the array
                    For lItem As Integer = nlBound To nUBound
                        ' get item details
                        n_TaskGroupId = CInt(m_aoTaskGroupUserGroups(ACTaskGroupUserGroup_TaskGroupId, lItem))
                        nUserGroupId = CInt(m_aoTaskGroupUserGroups(ACTaskGroupUserGroup_UserGroupId, lItem))
                        sUserGroup = CStr(m_aoTaskGroupUserGroups(ACTaskGroupUserGroup_UserGroupDescription, lItem))

                        ' if task group matches the selected task group
                        If n_TaskGroupId = nTaskGroupId Then
                            ' add user group to combo
                            cboUserGroup.Items.Insert(nIndex, sUserGroup)
                            VB6.SetItemData(cboUserGroup, nIndex, nUserGroupId)
                            nIndex += 1
                        End If
                    Next lItem
                End If
            End If
            cboUserGroup.Enabled = Not (nIndex = 0)
        Catch Excep As Exception
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskGroupId", nTaskGroupId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kFunctionName, excep:=Excep, oDicParms:=oDict)
            m_nErrorNumber = Information.Err().Number
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Initilaise renamed
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Form_Initialize_Renamed()
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim sMessage, sTitle As String
        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_nErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            nResult = g_oObjectManager.GetInstance(m_oBusiness, "bSIRMIDMaintenance.Business", vInstanceManager:="ClientManager")
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_nLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_nLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            '  Create an instance of the general interface object.
            m_oGeneral = New iSIRMIDMaintenance.General()
            '  Call the initialise method passing this interface
            '  and the business object as parameters.
            nResult = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting the interface.
            m_nStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Catch Excep As Exception
            m_nErrorNumber = Information.Err().Number
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, Excep:=Excep)
            Exit Sub
        End Try

    End Sub

    ''' <summary>
    ''' Add or Edit MID rule
    ''' </summary>
    ''' <param name="nMIDRuleID"></param>
    ''' <param name="nActiontype"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddorEditMIDRule(nMIDRuleID As Integer, nActiontype As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        nResult = m_oBusiness.AddorEditMIDRule(nSourceID:=MIDRuleSourceID, nMIDRuleID:=nMIDRuleID, sCode:=txtCode.Text.Trim, sDescription:=txtDescription.Text.Trim, dtEffectiveDate:=MIDRuleEffectiveDate, dtStartDate:=MIDRuleStartDate, _
                             dtExpiryDate:=MIDRuleExpiryDate, sMIDType:=cboMIDType.SelectedValue, nSupplierTypeId:=cboSupplierType.ItemId, nSupplierid:=txtSupplierID.Text, _
                             nInsurerId:=txtInsurerID.Text, nDelegatedAuthorityID:=ToSafeInteger(txtDelegatedAuthorityID.Text), nSiteNumber:=ToSafeInteger(txtSiteNumber.Text), _
                             nPMUserGroupId:=DirectCast(cboUserGroup.SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData, _
                             nPMwrkTaskGroupid:=DirectCast(cboTaskGroup.SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData, sfileName:=txtFileName.Text, _
                             nTestIndicator:=IIf(chkTestIndicator.Checked, 1, 0), sFileSeqNumStart:=txtFileSequenceNumberStart.Text, sCurrentFileSeqNum:=txtCurrentfileSequenceNumber.Text)
        ' Check for errors
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to add MID rule details to the database")
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' PopulateLookups
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateLookups() As Integer
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue

        ' Populate Lookup Values         ' Task Group
        nResult = GetLookupDetails(v_sLookupTable:=ACLookupTablePMWrkTaskGroup, r_octlLookup:=cboTaskGroup, v_bAddBlankEntry:=True)

        'MID1 or MID2
        Dim dtMIDTypes As New DataTable
        ' Create four typed columns in the DataTable.
        dtMIDTypes.Columns.Add("Code", GetType(String))
        dtMIDTypes.Columns.Add("ID", GetType(Integer))

        Dim dr1 As DataRow = dtMIDTypes.NewRow
        dr1("Code") = "MID1"
        dr1("ID") = 1
        dtMIDTypes.Rows.Add(dr1)
        Dim dr2 As DataRow = dtMIDTypes.NewRow
        dr2("Code") = "MID2"
        dr2("ID") = 2
        dtMIDTypes.Rows.Add(dr2)

        cboMIDType.DisplayMember = "Code"
        cboMIDType.ValueMember = "Code"
        cboMIDType.DataSource = dtMIDTypes

        Return nResult
    End Function

    ''' <summary>
    ''' Gets all of the lookup details using the lookup
    ''' values, then assigns them to the control passed.
    ''' Selects the specified row
    ''' </summary>
    ''' <param name="v_sLookupTable"></param>
    ''' <param name="r_octlLookup"></param>
    ''' <param name="v_nSelectedItemId"></param>
    ''' <param name="v_bAddBlankEntry"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLookupDetails(ByVal v_sLookupTable As String, ByRef r_octlLookup As ComboBox, _
                                      Optional ByVal v_nSelectedItemId As Integer = 0, _
                                      Optional ByVal v_bAddBlankEntry As Boolean = False) As Integer
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Const sFunctionName As String = "GetLookupDetails"

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        ' Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        Dim nRow As Integer
        Dim bFoundMatch As Boolean
        Dim nItemIndex, nItemFoundIndex, nIndex As Integer
        Try
            ' Get the lookup values.
            r_octlLookup.Items.Clear()
            bFoundMatch = False
            For nRow = m_aoLookupTables.GetLowerBound(1) To m_aoLookupTables.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_aoLookupTables(ACValueTableName, nRow)).Trim() = v_sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next nRow
            ' Check if there has been a table match.
            If Not bFoundMatch Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & v_sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
                Return nResult
            End If
            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            nItemIndex = -1
            nItemFoundIndex = -1
            If v_bAddBlankEntry Then
                r_octlLookup.Items.Insert(0, "")
                nIndex = 1
            Else
                nIndex = 0
            End If

            For lCntr As Integer = ToSafeInteger(m_aoLookupTables(ACValueStartPos, nRow)) To ToSafeInteger((CDbl(m_aoLookupTables(ACValueStartPos, nRow)) + CDbl(m_aoLookupTables(ACValueNumber, nRow))) - 1)
                r_octlLookup.Items.Add(CStr(m_aoLookupDetails(ACDetailDesc, lCntr)))
                VB6.SetItemData(r_octlLookup, nIndex, CInt(m_aoLookupDetails(ACDetailKey, lCntr)))
                If CDbl(m_aoLookupDetails(ACDetailKey, lCntr)) = v_nSelectedItemId Then
                    nItemFoundIndex = nItemIndex
                End If
                Debug.WriteLine(VB6.GetItemString(r_octlLookup, nIndex) & ":" & CStr(nIndex))
                nIndex += 1
                nItemIndex += 1
            Next lCntr
            ' set the item we want to display in the list
            If nItemFoundIndex <> -1 Then
                r_octlLookup.SelectedIndex = nItemFoundIndex
            End If
        Catch Excep As Exception
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSelectedItemId", v_nSelectedItemId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, Excep:=Excep, oDicParms:=oDict)
            m_nErrorNumber = Information.Err().Number
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

    Private Function ZeroToNull(ByRef vValue As Object) As Object
        ZeroToNull = vValue

        If Not IsDBNull(vValue) Then
            If vValue = 0 Then
                ZeroToNull = DBNull.Value
            End If
        End If

    End Function
    Private Function GetLookups() As Integer
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Const sFunctionName As String = "GetLookups"
        Try
            ReDim m_aoLookupTables(3, 0)
            m_aoLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = ACLookupTablePMWrkTaskGroup

            nResult = m_oBusiness.GetLookupValues(v_vLookupTables:=m_aoLookupTables, r_vLookupDetails:=m_aoLookupDetails)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
        Catch Excep As Exception
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=Excep)
            m_nErrorNumber = Information.Err().Number
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Allow Delegated authority id only if supplier type id Delegated authority
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AllowDelegatedAuthrityID()
        If (cboSupplierType.ItemCode IsNot Nothing) Then
            If (cboSupplierType.ItemCaption.Trim().ToUpper() = "DELEGATED AUTHORITY") Then
                txtDelegatedAuthorityID.Enabled = True
                txtDelegatedAuthorityID.Visible = True
                lblDelegatedAuthorityID.Visible = True
            Else
                txtDelegatedAuthorityID.Enabled = False
                txtDelegatedAuthorityID.Visible = False
                lblDelegatedAuthorityID.Visible = False
                If Task = gPMConstants.PMEComponentAction.PMAdd Then
                    txtDelegatedAuthorityID.Text = String.Empty
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Checks that date range for new rule being added does not overlap with any existing rule
    ''' </summary>
    ''' <param name="sMIDRuleType"></param>
    ''' <param name="dtStartDate"></param>
    ''' <param name="dtExpiryDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConfirmSingularityOfActiveRule(ByVal sMIDRuleType As String, ByVal dtStartDate As Date, _
                                                    ByVal dtExpiryDate As Date)
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Dim nLower, nUpper As Integer
        Const sFunctionName As String = "ConfirmSingularityOfActiveRule"

        Const kACMIDRuleID As Integer = 0
        Const kACMIDRuleStartDate As Integer = 4
        Const kACMIDRuleExpiryDate As Integer = 5
        Const kACMIDRuleTypeLoc As Integer = 6
        Const kACMIDRuleSourceIdLoc As Integer = 18
        Const kACMIDisDeleted As Integer = 23
        Try
            'Get array limits
            If gArrays.GetArrayBounds(r_vArray:=m_aoRulesArray, r_lDimension:=gArrays.klRowDimension, r_lLower:=nLower, r_lUpper:=nUpper) Then
                For lRow As Integer = nLower To nUpper
                    If Not MIDRuleID = CInt(m_aoRulesArray(kACMIDRuleID, lRow)) Then
                        If MIDRuleSourceID = CInt(m_aoRulesArray(kACMIDRuleSourceIdLoc, lRow)) AndAlso _
                            sMIDRuleType = ToSafeString(m_aoRulesArray(kACMIDRuleTypeLoc, lRow)) AndAlso _
                            Not ToSafeBoolean(m_aoRulesArray(kACMIDisDeleted, lRow)) Then

                            'for selected branch check that there is no datae range over lap
                            If (dtStartDate >= ToSafeDate(m_aoRulesArray(kACMIDRuleStartDate, lRow)) And String.IsNullOrEmpty(m_aoRulesArray(kACMIDRuleExpiryDate, lRow))) Or _
                                 (dtStartDate >= ToSafeDate(m_aoRulesArray(kACMIDRuleStartDate, lRow)) And dtStartDate <= ToSafeDate(m_aoRulesArray(kACMIDRuleExpiryDate, lRow))) Or _
                                  (dtExpiryDate >= ToSafeDate(m_aoRulesArray(kACMIDRuleStartDate, lRow)) And dtExpiryDate <= ToSafeDate(m_aoRulesArray(kACMIDRuleExpiryDate, lRow))) _
                                  Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                Return nResult
                            End If
                        End If
                    End If
                Next lRow
            End If
        Catch Excep As Exception
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=Excep)
            m_nErrorNumber = Information.Err().Number
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

#End Region

#Region "Event Methods"

    ''' <summary>
    ''' Form Load Event
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            '  Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            If m_oBusiness Is Nothing Then
                Form_Initialize_Renamed()
            End If

            nResult = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            nResult = SetFieldValidation()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' get task creation lookup data - task groups / tasks / pmusergroups
            nResult = GetTaskCreationLookupData()
            nResult = SetInterfaceDefaults()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            BusinessToInterface()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Catch Excep As Exception
            m_nErrorNumber = Information.Err().Number
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", Excep:=Excep)
            Exit Sub
        End Try

    End Sub

    ''' <summary>
    ''' Event Handling for form closing
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim nCancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim nUnloadMode As Integer = CInt(eventArgs.CloseReason)
        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.
            If nUnloadMode <> vbFormCode Then
                'Process the next set of actions depending
                'upon the interface task etc.
                nResult = m_oGeneral.ProcessCommand(r_bChangesMade:=False)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Do not procced with the interface termination.
                    eventArgs.Cancel = True
                    nCancel = 1
                    'Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            m_oGeneral = Nothing

            ' Terminate the business object
            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            m_oBusiness = Nothing

            m_oFormfields.Dispose()
            m_oFormfields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Catch Excep As Exception
            ' Error Section.
            m_nErrorNumber = Information.Err().Number
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, Excep:=Excep)
            Exit Sub
            eventArgs.Cancel = nCancel <> 0
        End Try

    End Sub

    ''' <summary>
    ''' Adds or updates MID rule. Click  event of Ok Button
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            '  Set the interface status.
            m_nStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            If m_oFormfields IsNot Nothing Then
                nResult = m_oFormfields.CheckMandatoryControls()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_nStatus = gPMConstants.PMEReturnCode.PMCancel
                    Exit Sub
                End If
            End If

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtInsurerID.Text, NumberStyles.Integer, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("Insurer ID must be numeric.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtInsurerID.Focus()
                m_nStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If
            If Not Double.TryParse(txtSupplierID.Text, NumberStyles.Integer, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("Supplier ID must be numeric.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtSupplierID.Focus()
                m_nStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            If Not Double.TryParse(txtFileSequenceNumberStart.Text, NumberStyles.Integer, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("File Sequence number must be numeric.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtFileSequenceNumberStart.Focus()
                m_nStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            If txtSiteNumber.Text.Trim() <> String.Empty AndAlso Not Double.TryParse(txtSiteNumber.Text, NumberStyles.Integer, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("Site Number must be numeric.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtSiteNumber.Focus()
                m_nStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            If cboTaskGroup.SelectedIndex < 0 Then
                MessageBox.Show("Task Group must be selected.", "Invalid Value.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_nStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If
            If cboUserGroup.SelectedIndex < 0 Then
                MessageBox.Show("User Group must be selected.", "Invalid Value.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_nStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Interface to properties
            MIDRuleEffectiveDate = ToSafeDate(txtEffectiveDate.Text)
            MIDRuleExpiryDate = ToSafeDate(txtExpiryDate.Text)
            MIDRuleStartDate = ToSafeDate(txtStartDate.Text)
            MIDRuleSupplierTypeID = cboSupplierType.ItemId

            If MIDRuleStartDate < MIDRuleEffectiveDate Then
                MessageBox.Show("Start date can not be less than effective date", "Invalid Value.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_nStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If
            If MIDRuleExpiryDate < MIDRuleStartDate Then
                MessageBox.Show("Expiry date can not be less than start date", "Invalid Value.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_nStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            nResult = ConfirmSingularityOfActiveRule(cboMIDType.SelectedValue, MIDRuleStartDate, MIDRuleExpiryDate)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim sMessage As String
                sMessage = String.Format("Active date range for this rule ovarlapes with another active {0} rule for this branch.", cboMIDType.SelectedText)
                MessageBox.Show(sMessage, "Invalid Value.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            '  upon the interface task etc.
            nResult = m_oGeneral.ProcessCommand(r_bChangesMade:=False)
            If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If
        Catch Excep As Exception
            ' Error Section.
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            Exit Sub
        End Try

    End Sub

    ''' <summary>
    ''' Click event of cancel button
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            ' Set the interface status.
            m_nStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            nResult = m_oGeneral.ProcessCommand(r_bChangesMade:=False)
            If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch Excep As Exception
            ' Error Section.
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, Excep:=Excep)
            Exit Sub
        End Try
    End Sub

    ''' <summary>
    ''' key down event for details form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
    End Sub

    ''' <summary>
    ''' textbox enter event of start date
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtStartDate_Enter(sender As Object, e As EventArgs) Handles txtStartDate.Enter
        m_oFormfields.LostFocus(ctlControl:=txtStartDate)
    End Sub

    ''' <summary>
    ''' textbox leave event of start date
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtStartDate_Leave(sender As Object, e As EventArgs) Handles txtStartDate.Leave
        m_oFormfields.LostFocus(ctlControl:=txtStartDate)
    End Sub

    ''' <summary>
    ''' textbox enter event of effective date
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtEffectiveDate_Enter(sender As Object, e As EventArgs) Handles txtEffectiveDate.Enter
        m_oFormfields.LostFocus(ctlControl:=txtEffectiveDate)
    End Sub

    ''' <summary>
    ''' textbox leave event of effective date
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtEffectiveDate_Leave(sender As Object, e As EventArgs) Handles txtEffectiveDate.Leave
        m_oFormfields.LostFocus(ctlControl:=txtEffectiveDate)
    End Sub

    ''' <summary>
    ''' textbox enter event of expirty date
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtExpiryDate_Enter(sender As Object, e As EventArgs) Handles txtExpiryDate.Enter
        m_oFormfields.LostFocus(ctlControl:=txtExpiryDate)
    End Sub

    ''' <summary>
    ''' textbox leave event of expiry date
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtExpiryDate_Leave(sender As Object, e As EventArgs) Handles txtExpiryDate.Leave
        m_oFormfields.LostFocus(ctlControl:=txtExpiryDate)
    End Sub

    ''' <summary>
    ''' CBO event for taskGrp selection change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboTaskGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTaskGroup.SelectedIndexChanged
        SetupTaskCombos()
        ' store the currenct task group
        m_sPrevTaskGroup = cboTaskGroup.Text
    End Sub

    ''' <summary>
    ''' textbox key press event of supplier ID
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtSupplierID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSupplierID.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' textbox key press event of insurer ID
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtInsurerID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtInsurerID.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' textbox key press event of DA ID
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtDelegatedAuthorityID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDelegatedAuthorityID.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' textbox key press event of site number
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtSiteNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSiteNumber.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    ' ''' <summary>
    ' ''' textbox key press event of da bramnch id text
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub txtDABranchID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDABranchID.KeyPress
    '    If Asc(e.KeyChar) <> 8 Then
    '        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
    '            e.Handled = True
    '        End If
    '    End If
    'End Sub

    ''' <summary>
    ''' textbox key press event of file seq number text
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtFileSequenceNumberStart_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFileSequenceNumberStart.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' combo lost focus event of cbo supplier type
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboSupplierType_LostFocus(sender As Object, e As EventArgs) Handles cboSupplierType.Leave
        AllowDelegatedAuthrityID()
    End Sub

    ''' <summary>
    ''' textbox leave event of file seq number text
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtFileSequenceNumberStart_Leave(sender As Object, e As EventArgs) Handles txtFileSequenceNumberStart.Leave
        txtFileSequenceNumberStart.Text = txtFileSequenceNumberStart.Text.ToString().PadLeft(6, "0")
        txtCurrentfileSequenceNumber.Text = txtFileSequenceNumberStart.Text
    End Sub


#End Region

End Class
