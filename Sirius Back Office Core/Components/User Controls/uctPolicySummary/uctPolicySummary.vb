Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPolicySummControl_NET.uctPolicySummControl")> _
Partial Public Class uctPolicySummControl
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event InsuranceFileStructureIdChange()
    Public Event RiskCntChange()
    Public Event RiskGroupIdChange()
    Public Event RiskCodeIdChange()
    Public Event InsuranceFileCntChange()
    Public Event InsuranceFolderCntChange()
    Public Event PartyCntChange()
    Public Event ProcessModeChange()
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event TaskChange()
    Public Event StatusChange()
    Public Event CallingAppNameChange()

    ' Date: 23/12/1997
    '
    ' Description: Main interface for frmPolicy form
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctPolicySummaryControl"

    ' PUBLIC Data Members (Begin)
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

    ' {* USER DEFINED CODE (Begin) *}
    'Fields from Insurance File
    Private m_lPartyCnt As Integer

    Private m_lInsuranceFolderCnt As Integer

    Private m_vFieldArray As Object
    Private m_sFooter As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_vInsuranceFileStatusId As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_sInsurerRef As String = ""
    Private m_lLeadInsurerCnt As Integer
    Private m_vAccountHandlerCnt As String = ""
    Private m_sAccountHandlerDesc As String = ""

    ' SJP (CMG) 09/04/2003 PS235
    Private m_vAccountExecutiveCnt As String = ""
    Private m_sAccountExecutiveDesc As String = ""

    Private m_iCurrencyID As Integer
    Private m_iLanguageID As Integer
    Private m_dtCoverStartDate As Date
    Private m_dtExpiryDate As Date
    Private m_vThisPremium As String = ""
    Private m_vRiskCodeId As String = ""
    Private m_sRiskDesc As String = ""
    Private m_vRiskGroupId As Object
    Private m_lInsuranceFileStructureId As Integer
    Private m_lPolicyTypeId As Integer
    Private m_vGeminiPolicyStatus As String = ""
    Private m_lRiskScreenId As Integer 'CT 19/10/00 new field on RiskGroup table
    'eck070301
    Private m_vRiskCnt As Object

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRInsuranceFile.Business

    ' Declare an instance of the Lock object.
    Private m_oPMLock As Object

    'CT 19/10/00 object that will provide risk group screen id
    ' Declare an instance of the RiskGroup object.

    Private m_oRiskGroup As Object

    ' SJP (CMG) 09/04/2003 PS235
    Private m_bShowAccountExecutive As Boolean

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control

    'SJ 18/02/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    Private m_vAlternateReference As String = ""
    'SJ 18/02/2004 - end

    'SJ 20/02/2004 - start
    <Browsable(False)> _
    Public ReadOnly Property UnderwritingBranchEnabled() As Boolean
        Get
            Return m_bUnderwritingBranchEnabled
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property IsUnderwritingBranch() As Boolean
        Get
            Return m_bIsUnderwritingBranch
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property AlternateReference() As String
        Get
            Return m_vAlternateReference
        End Get
    End Property
    'SJ 20/02/2004 - end

    <Browsable(False)> _
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value
            RaiseEvent CallingAppNameChange()

        End Set
    End Property
    <Browsable(True)> _
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
            RaiseEvent StatusChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value
            RaiseEvent TaskChange()

        End Set
    End Property
    <Browsable(False)> _
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value
            RaiseEvent TransactionTypeChange()

        End Set
    End Property
    <Browsable(False)> _
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value
            RaiseEvent EffectiveDateChange()

        End Set
    End Property
    <Browsable(False)> _
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value
            RaiseEvent ProcessModeChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value
            RaiseEvent PartyCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property InsuranceFolderCnt() As Integer
        Get

            Return m_lInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFolderCnt = Value
            RaiseEvent InsuranceFolderCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value
            RaiseEvent InsuranceFileCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property RiskCodeId() As String
        Get

            Return m_vRiskCodeId

        End Get
        Set(ByVal Value As String)


            m_vRiskCodeId = CStr(Value)
            RaiseEvent RiskCodeIdChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property RiskGroupId() As Object
        Get

            Return m_vRiskGroupId

        End Get
        Set(ByVal Value As Object)



            m_vRiskGroupId = Value
            RaiseEvent RiskGroupIdChange()

        End Set
    End Property
    'eck070301
    <Browsable(True)> _
    Public Property RiskCnt() As Object
        Get

            Return m_vRiskCnt

        End Get
        Set(ByVal Value As Object)



            m_vRiskCnt = Value
            RaiseEvent RiskCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property InsuranceFileStructureId() As Integer
        Get

            Return m_lInsuranceFileStructureId

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileStructureId = Value
            RaiseEvent InsuranceFileStructureIdChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property PolicyTypeId() As Integer
        Get

            Return m_lPolicyTypeId

        End Get
        Set(ByVal Value As Integer)

            m_lPolicyTypeId = Value

        End Set
    End Property

    <Browsable(True)> _
    Public Property GeminiPolicyStatus() As String
        Get

            Return m_vGeminiPolicyStatus

        End Get
        Set(ByVal Value As String)


            m_vGeminiPolicyStatus = CStr(Value)

        End Set
    End Property

    'JSB 07/06/2001 - added new public property to get GII policy number
    <Browsable(False)> _
    Public ReadOnly Property GIIPolicyNumber() As String
        Get

            Return m_sInsuranceRef

        End Get
    End Property

    'CT 19/10/00 added new property to old new field on risk_group table  - (start)

    <Browsable(True)> _
    Public Property RiskScreenId() As Integer
        Get
            Return m_lRiskScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskScreenId = Value
        End Set
    End Property
    'CT 19/10/00 added new property to old new field on risk_group table  - (end)


    'Public Methods
    ' ***************************************************************** '
    ' Name: CancelClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CancelClick() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowHelpScreen
    '
    ' Description: Shows the help screen
    '
    ' ***************************************************************** '
    Public Function ShowHelpScreen() As Integer
        'Developer Guide No.199
        Dim dlghelp As New Object()
        ' Fire up the help screen
        Return SSfunc.ShowHelp(dlghelp, ScreenHelpID)

    End Function

    ' ***************************************************************** '
    '
    ' Name: LockThePolicy
    '
    ' Description:
    '
    ' History: 06/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function LockThePolicy() As Integer

        Dim result As Integer = 0
        Try


            Return LockPolicy()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockThePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockThePolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LockPolicy
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function LockPolicy() As Integer

        Dim result As Integer = 0
        Dim sLockedBy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oPMLock.LockKey(sKeyName:="insurance_file_cnt", vKeyValue:=m_lInsuranceFileCnt, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Policy currently locked by " & sLockedBy & _
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Policy Lock")
                        Return result
                    End If


                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnlockThePolicy
    '
    ' Description:
    '
    ' History: 06/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UnlockThePolicy() As Integer

        Dim result As Integer = 0
        Try


            Return UnlockPolicy()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockThePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockThePolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnlockPolicy
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function UnlockPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oPMLock.UnLockKey(sKeyName:="insurance_file_cnt", vKeyValue:=m_lInsuranceFileCnt, iUserID:=g_oObjectManager.UserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: OKClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function OKClick() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Static bIsInitialised As Boolean

        Dim sTitle, sMessage As String

        Dim sHelpFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'Developer Guide No.39(no solution)
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRInsuranceFile.Business", gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            'Get bPMLock
            Dim temp_m_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            'SJ 20/02/2004 - start
            m_lReturn = CType(CheckForUnderwritingBranch(v_iSourceId:=g_oObjectManager.SourceID, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            'SJ 20/02/2004 - end

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' hold Initialised status
            bIsInitialised = True

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    Public Function LoadControl() As Integer
        ' Forms load event.

        Dim result As Integer = 0
        Dim vOptionValue As Object

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            '    If (m_lErrorNumber& = PMFalse) Then
            '        ' We have already encountered an error,
            '        ' so we MUST exit now.
            '        Exit Function
            '    End If

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

                Return result
            End If

            ' SJP (CMG) 09/04/2003 PS235

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vOptionValue))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetProductOptionValue for " & gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")
                Return result
            End If

            'Show Account Executive Field

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vOptionValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_bShowAccountExecutive = CBool(vOptionValue)
            End If

            ' Set the business keys.
            '{* USER DEFINED CODE (Begin) *}

            m_oBusiness.PartyCnt = m_lPartyCnt

            m_oBusiness.InsuranceFolderCnt = m_lInsuranceFolderCnt

            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt
            '{* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            ' Validate fields using Forms Control
            '    m_lReturn& = SetFieldValidation()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicy
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Public Function GetPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            If Task = gPMConstants.PMEComponentAction.PMEdit Or Task = gPMConstants.PMEComponentAction.PMView Then
                ' Get the interface details from the
                ' business object.
                m_lReturn = GetBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the interface.
                m_lReturn = BusinessToInterface()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' Display all of the lookup details.
            m_lReturn = DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the task.
            If Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(lDisabled:=True)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Const vbFormCode As Integer = 0
    Public Function UnloadControl(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer
        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Function
                End If
            End If

            ' Terminate the object.
            Dispose()
            ' Destroy the instance of the object
            ' from memory.

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If Not (m_oPMLock Is Nothing) Then

                    m_oPMLock = Nothing
                End If


            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: ValidateParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function ValidateInsurer() As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateInsurerFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateInsurer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetNext(r_vFieldArray:=m_vFieldArray)
            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If


            m_lReturn = SetValuesFromArray(v_vFieldArray:=m_vFieldArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set the values from the array", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If
            'Get additional details required for display that not stored on this
            'record
            'eck070301 Get the new Risk Cnt


            'Developer Guide No 119
            m_lReturn = m_oBusiness.GetOtherDetails(vInsurerCnt:=m_lLeadInsurerCnt, vInsurerName:=m_sInsurerRef, vBrokerCnt:=DBNull.Value, vBrokerName:="", vRiskId:=m_vRiskCodeId, vRiskDesc:=m_sRiskDesc, vRiskGroupId:=m_vRiskGroupId, vAnalysisId:=DBNull.Value, vAnalysisDesc:="", vHandlerCnt:=m_vAccountHandlerCnt, vHandlerName:=m_sAccountHandlerDesc, vAgentCnt:=DBNull.Value, vAgentName:="", vInsuranceFileCnt:=m_lInsuranceFileCnt, vRelatedPolicyCnt:="", vRelatedPolicyCode:=Nothing, vRelationshipType:="", vPolicyTypeId:=DBNull.Value, vPolicyTypeDesc:="", vSchemeId:=Nothing, vSchemeDesc:="", vRiskCnt:=m_vRiskCnt, vExecutiveCnt:=m_vAccountExecutiveCnt, vExecutiveName:=m_sAccountExecutiveDesc)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If
            'CT 19/10/00 Store which Risk Screen this policy uses
            m_lReturn = GetRiskScreen(m_vRiskGroupId)


            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SJ 18/02/2004 - start
            If m_bIsUnderwritingBranch And m_vAlternateReference <> "" Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPolicyNumber, vControlValue:=m_vAlternateReference)
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPolicyNumber, vControlValue:=m_sInsuranceRef)
            End If
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtPolicyNumber, _
            ''                                             vControlValue:=m_sInsuranceRef)
            'SJ 18/02/2004 - end

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInsurer, vControlValue:=m_sInsurerRef)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRisk, vControlValue:=m_sRiskDesc)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAccountHandler, vControlValue:=m_sAccountHandlerDesc)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' SJP (CMG) 09/04/2003 PS235
            If m_bShowAccountExecutive Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAccountExecutive, vControlValue:=m_sAccountExecutiveDesc)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCoverFrom, vControlValue:=m_dtCoverStartDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not (Convert.IsDBNull(m_vThisPremium) Or IsNothing(m_vThisPremium)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremiumInc, vControlValue:=m_vThisPremium)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCoverTo, vControlValue:=m_dtExpiryDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' Name: DeleteClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function DeleteClick() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = LockPolicy()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = DeletePolicy()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'Need to unlock, so let's not exit just yet
            End If

            m_lReturn = UnlockPolicy()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeletePolicy
    '
    ' Description:
    '
    ' History: 17/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function DeletePolicy() As Integer

        Dim result As Integer = 0
        Dim bOKToDelete As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt


            bOKToDelete = m_oBusiness.OKToDelete()

            If Not bOKToDelete Then


                MessageBox.Show("Cannot delete -" & Strings.Chr(13) & Strings.Chr(10) & m_oBusiness.NoDeleteReasons, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If


            m_lReturn = m_oBusiness.DeletePolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set all of the forms controls to the disable state.
            For Each ctlFormControl As Control In Controls
                ' Check the type of the control.
                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is RadioButton) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                End If
            Next ctlFormControl

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            lblPolicyNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblInsurer.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInsurer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRisk.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRisk, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountHandler.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountHandler, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblcoverFrom.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCoverFrom, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPremiumPayable.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPremiumPayable, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCoverTo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCoverTo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' SJP (CMG) 09/04/2003 PS235

            lblAccountExecutive.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountExecutive, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupInsuranceFileStatus, ctlLookup:=txtPolicyStatus)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If txtPolicyStatus.Text = "" Then
                txtPolicyStatus.Text = "Live"
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupCurrency, ctlLookup:=txtCurrency)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'eck180500

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSource, ctlLookup:=txtBranchName)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function EditClick() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = LockPolicy()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)

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
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer

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
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            'eck180500
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then
                        ctlLookup.Text = CStr(m_vLookupDetails(ACDetailDesc, lCntr))
                    End If

                End If
            Next lCntr

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskScreen
    '
    ' Description: calls RiskGroup business object to find out which
    '              Risk screen this riskGroup has assigned to it
    ' CT 19/10/00 created to make the risk screen id of a risk group available (new field)
    ' ***************************************************************** '
    Private Function GetRiskScreen(ByRef vRiskGroupId As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' SET 06112002 - Amended to cater for the Scheme policy type
            '    If m_lPolicyTypeId <> PMBPolicyTypeGeneral Then
            If m_lPolicyTypeId <> PMBConst.PMBPolicyTypeGeneral And m_lPolicyTypeId <> PMBConst.PMBPolicyTypeSchemes Then
                Return result
            End If
            Dim temp_m_oRiskGroup As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskGroup, "bSIRRiskGroup.Business", gPMConstants.PMGetViaClientManager)
            'm_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskGroup, "bSIRRiskGroup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRiskGroup = temp_m_oRiskGroup

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get interface of business.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bSIRRiskGroup", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            m_lReturn = m_oRiskGroup.GetRiskScreenId(vRiskGroupId)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            RiskScreenId = m_oRiskGroup.RiskScreenId
            m_oRiskGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Risk Screen from business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer
        Dim result As Integer = 0
        Dim stemp As String = ""


        Try


            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            ' {* GENERATED CODE (End) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            Select Case (Task)
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                    ' Update the business from the interface.
                    m_lReturn = InterfaceToBusiness()

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to update business.
                        Return result
                    End If
            End Select

            ' Check the task.
            Select Case (Task)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Form hasn't been cancelled, so we just go
                        ' ahead and add the details.

                        ' Add the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Check the details havn't changed.

                        m_lReturn = m_oBusiness.Cancel()
                        'MH Request - Allways confirm cancellation
                        '                If (m_lReturn& = PMDataChanged) Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        '               End If
                    Else
                        ' Update the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If
            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oFormFields = New iPMFormControl.FormFields()

            m_oFormFields.LanguageID = g_iLanguageID

            ' {* GENERATED CODE (Begin) *}
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPolicyNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Insurer Name
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurer, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Risk
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRisk, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AccountHandler
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountHandler, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Status
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPolicyStatus, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Currency
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCurrency, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Premium
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremiumInc, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Cover From
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCoverFrom, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Cover To
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCoverTo, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' SJP (CMG) 10/04/2003 PS235
            'Account Executive
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountExecutive, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* GENERATED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Dim sDMSDir As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            'CenterForm Me

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

            ' SJP (CMG) 09/04/2003 PS235
            lblAccountExecutive.Visible = m_bShowAccountExecutive
            txtAccountExecutive.Visible = m_bShowAccountExecutive

            If Not (m_bShowAccountExecutive) Then
                ' Re-shuffle controls below Account Executive upwards to fill
                ' gap caused by invisible Account Executive field.
                lblcoverFrom.Top -= VB6.TwipsToPixelsY(420)
                lblCoverTo.Top -= VB6.TwipsToPixelsY(420)
                txtCoverFrom.Top -= VB6.TwipsToPixelsY(420)
                txtCoverTo.Top -= VB6.TwipsToPixelsY(420)
            End If

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
    ' Name: SetValuesFromArray (Public)
    '
    ' Description: Sets the values from the array.
    '
    ' ***************************************************************** '
    Private Function SetValuesFromArray(ByVal v_vFieldArray() As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lInsuranceFileCnt = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt))

            m_vInsuranceFileStatusId = CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID))

            m_sInsuranceRef = CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceRef))

            If Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt)) Or IsNothing(v_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt)) Then
                m_lLeadInsurerCnt = 0
            Else

                m_lLeadInsurerCnt = CInt(v_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt))
            End If

            m_vAccountHandlerCnt = CStr(v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt))

            ' SJP (CMG) 09/04/2003 PS235

            m_vAccountExecutiveCnt = CStr(v_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt))


            m_iCurrencyID = CInt(v_vFieldArray(InsuranceFileConst.ACCurrencyID))

            m_iLanguageID = CInt(v_vFieldArray(InsuranceFileConst.ACLanguageID))

            m_dtCoverStartDate = CDate(v_vFieldArray(InsuranceFileConst.ACCoverStartDate))

            m_dtExpiryDate = CDate(v_vFieldArray(InsuranceFileConst.ACExpiryDate))

            m_vRiskCodeId = CStr(v_vFieldArray(InsuranceFileConst.ACRiskCodeId))

            m_vThisPremium = CStr(v_vFieldArray(InsuranceFileConst.ACThisPremium))

            m_lInsuranceFileStructureId = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID))

            m_lPolicyTypeId = CInt(v_vFieldArray(InsuranceFileConst.ACPolicyTypeId))

            m_vGeminiPolicyStatus = CStr(v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus))
            'SJ 18/02/2004 - start

            m_vAlternateReference = CStr(v_vFieldArray(InsuranceFileConst.ACAlternateReference)).Trim()
            'SJ 18/02/2004 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetValuesFromArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetValuesFromArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
