Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles


<System.Runtime.InteropServices.ProgId("uctPartySummControl_NET.uctPartySummControl")> _
Partial Public Class uctPartySummControl
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event UnderwritingOrAgencyChange()
    Public Event BorderStyleChange()
    Public Event BackStyleChange()
    Public Event FontChange()
    Public Event EnabledChange()
    Public Event ForeColorChange()
    Public Event BackColorChange()
    Public Event PartyCntChange()
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event ProcessModeChange()
    Public Event NavigateChange()
    Public Event TaskChange()
    Public Event StatusChange()
    Public Event CallingAppNameChange()
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23/06/1998
    '
    ' Description: Main interface.
    '
    ' Edit History: TF031298 - Menu & Toolbar activity
    '               SD170702 - Control layout changes
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctPartySummaryControl"

    'Default Property Values:
    Const m_def_BackColor As Integer = 0
    Const m_def_ForeColor As Integer = 0
    Const m_def_Enabled As Integer = 0
    Const m_def_BackStyle As Integer = 0
    Const m_def_BorderStyle As Integer = 0
    Const m_def_PartyCnt As Integer = 0
    'Property Variables:
    Dim m_BackColor As Integer
    Dim m_ForeColor As Integer
    Dim m_Enabled As Boolean
    Dim m_Font As Font
    Dim m_BackStyle As Integer
    Dim m_BorderStyle As BorderStyle
    Dim m_PartyCnt As Integer
    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)


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
    Private m_lAddressCount As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRParty.Business
    'SD 12/07/2002

    Private m_oServices As bSIRParty.Services
    'Private m_oBusiness As bSIRParty.Business

    ' Declare an instance of the Lock object.
    Private m_oPMLock As bPMLock.User

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
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private Const vbScrollBars As String = "0x80000000"
    Private Const vbDesktop As String = "0x80000001"
    Private Const vbActiveTitleBar As String = "0x80000002"
    Private Const vbInactiveTitleBar As String = "0x80000003"
    Private Const vbMenuBar As String = "0x80000004"
    Private Const vbWindowBackground As String = "H00FFFFC0"
    Private Const vbWindowFrame As String = "0x80000006"
    Private Const vbMenuText As String = "0x80000007"
    Private Const vbWindowText As String = "0x80000008"
    Private Const vbTitleBarText As String = "0x80000009"
    Private Const vbActiveBorder As String = "0x8000000A"
    Private Const vbInactiveBorder As String = "0x8000000B"
    Private Const vbApplicationWorkspace As String = "0x8000000C"
    Private Const vbHighlight As String = "0x8000000D"
    Private Const vbHighlightText As String = "0x8000000E"
    Private Const vbButtonFace As String = "0x8000000F"
    Private Const vbButtonShadow As String = "0x80000010"
    Private Const vbGrayText As String = "0x80000011"
    Private Const vbButtonText As String = "0x80000012"
    Private Const vbInactiveCaptionText As String = "0x80000013"
    Private Const vb3DHighlight As String = "0x80000014"
    Private Const vb3DDKShadow As String = "0x80000015"
    Private Const vb3DLight As String = "0x80000016"
    Private Const vbInfoText As String = "0x80000017"
    Private Const vbInfoBackground As String = "0x80000018"

    'Data retrieved via Get Details from Party & PartySummary

    Private m_lPartyCnt As Integer
    Private m_iPartyTypeId As Integer
    Private m_sShortName As String = ""
    Private m_sResolved As String = ""
    Private m_sBranch As String = ""
    Private m_lAgentCnt As Integer
    Private m_lConsultantCnt As Integer
    'SD 10/07/02 START added fields for extra summary information
    Private m_sTradingName As String = ""
    Private m_lSubBranchId As Integer
    Private m_sSubBranchName As String = ""
    Private m_sLoyaltyNo As String = ""
    Private m_dtDOB As Date
    'SD 10/07/02 START added fields for extra summary information


    'References from Party Lookups
    Private m_sAgentRef As String = ""
    Private m_sAgentName As String = ""
    Private m_sConsultantRef As String = ""
    Private m_sConsultantName As String = ""
    Private m_vAssociates(,) As Object
    Private m_lAssociatedCnt As Integer
    Private m_sAssociatedName As String = ""
    Private m_sAssociateRole As String = ""

    'Addresses and Contacts
    Private m_iLine As Integer
    Private m_lAddressCnt As Integer
    Private m_lAddressUsageTypeID As Integer
    Private m_lContactCnt As Integer
    Private m_sMainPostCode As String = ""
    Private m_vAddresses(,) As Object
    Private m_vAddressTypes(,) As Object
    Private m_vContacts(,) As Object
    'EK 22/10/99
    Private m_vContactTypes As Object
    Private m_vAddressContacts As Object
    '
    Private m_vConvictions As Object
    Private m_sAddressLine1 As String = ""
    'Extras for PMB

    Private m_iMainAddressIndex As Integer

    Private m_sCaption As String = ""
    Private m_sOKCaption As String = ""
    Private m_sHelpCaption As String = ""
    Private m_sCancelCaption As String = ""

    Private m_bSwiftLinked As Boolean
    Private m_oSwiftLink As Object

    ' CTAF 011200
    Private m_bSwiftInstalled As Boolean

    'MSS210901 - Added for merge
    Private m_sUnderwritingOrAgency As String = ""
    'MSS210901 - Merge end

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    <Browsable(False)> _
    Public ReadOnly Property Controls_Renamed() As Object
        Get
            Return Me.Controls
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

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
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value
            RaiseEvent NavigateChange()

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
    ' {* USER DEFINED CODE (Begin) *}


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

    ' {* USER DEFINED CODE (End) *}


    <Browsable(True)> _
    Public Shadows Property BackColor() As Integer
        Get
            Return m_BackColor
        End Get
        Set(ByVal Value As Integer)
            m_BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Shadows Property ForeColor() As Integer
        Get
            Return m_ForeColor
        End Get
        Set(ByVal Value As Integer)
            m_ForeColor = Value
            RaiseEvent ForeColorChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return m_Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property

    'TODO
    '   <Browsable(True)> _
    'Public Overrides Property Font() As Font
    '       Get
    '           Return m_Font
    '       End Get
    '       Set(ByVal Value As Font)
    '           m_Font = Value
    '           RaiseEvent FontChange()
    '       End Set
    '   End Property


    <Browsable(True)> _
    Public Property BackStyle() As Integer
        Get
            Return m_BackStyle
        End Get
        Set(ByVal Value As Integer)
            m_BackStyle = Value
            RaiseEvent BackStyleChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Shadows Property BorderStyle() As Integer
        Get
            Return m_BorderStyle
        End Get
        Set(ByVal Value As Integer)
            m_BorderStyle = Value
            RaiseEvent BorderStyleChange()
        End Set
    End Property
    'MSS210901 - Added property for merge

    <Browsable(True)> _
    Public Property UnderwritingOrAgency() As String
        Get
            Return m_sUnderwritingOrAgency
        End Get
        Set(ByVal Value As String)
            m_sUnderwritingOrAgency = Value
            RaiseEvent UnderwritingOrAgencyChange()
        End Set
    End Property
    'MSS210901 - Merge end
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)

    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    ' PUBLIC Methods (Begin)

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
    ' Name: GetParty
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Public Function GetParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    'If in edit mode, lock the party
            '    If (m_iTask = PMEdit) Then
            '
            '        m_lReturn = LockParty
            '
            '        If (m_lReturn <> PMTrue) Then
            '            GetParty = PMFalse
            '            Exit Function
            '        End If
            '
            '    End If

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the party", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                result = m_lReturn

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
                'Developer Guide No. 20
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = m_lReturn

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'SD 17/07/2002
                If sTitle = "" Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve error text from GetResData", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'SD 17/07/2002
                If sMessage = "" Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve error text from GetResData", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            'SD 12/07/2002 START
            ' Get an instance of the services object via
            ' the public object manager.
            Dim temp_m_oServices As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oServices, "bSIRParty.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oServices = temp_m_oServices

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the Services object.
                result = m_lReturn

                ' Display error stating the problem.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACServicesFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                If sTitle = "" Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve error text from GetResData", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACServicesFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'SD 17/07/2002
                If sMessage = "" Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve error text from GetResData", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If
            'SD 12/07/2002 END


            m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency

            'PN 21864 - Hiding Sub Branch for Broking
            If m_sUnderwritingOrAgency = "A" Then 'For Broking
                lblSubBranch.Visible = False
                txtSubBranch.Visible = False
            End If

            'Get bPMLock
            Dim temp_m_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If


            m_lReturn = GetSystemOptions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function GetSystemOptions failed.")
            End If

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

    ' ***************************************************************** '
    ' Name: LoadControl
    '
    ' Description: Does all the extra stuff that initialise doesn't
    '
    ' ***************************************************************** '
    Public Function LoadControl() As Integer

        Dim result As Integer = 0

        ' Forms load event.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.PartyCnt = m_lPartyCnt
            ' {* USER DEFINED CODE (End) *}


            m_oFormFields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()


            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'We call this explicitly from outside the control, renamed the GetParty method
            '    ' Gets the interface details to be displayed.
            '    m_lReturn& = GetInterfaceDetails()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get the interface details.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Set the mouse pointer to normal.
            '        SetMousePointer PMMouseNormal
            '
            '        Exit Sub
            '    End If

            'If adding, still need to get address types for populating
            'the combo box cells in the grid control
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                'Get addresse type lookups for the party

                m_lReturn = m_oBusiness.GetAddressTypeLookups(vAddressTypes:=m_vAddressTypes)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

                'Set the index of the main address
                For i As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)

                    'See if this is the main address
                    If CStr(m_vAddressTypes(2, i)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                        m_iMainAddressIndex = i
                        Exit For
                    End If

                Next i

            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Refresh
    '
    ' Description: What is this supposed to do?
    '
    ' ***************************************************************** '
    Public Overrides Sub Refresh()

    End Sub

    ' ***************************************************************** '
    ' Name: ShowHelpScreen
    '
    ' Description: Shows the help screen
    '
    ' ***************************************************************** '
    Public Function ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing, Optional ByRef ScreenHelpID As Object = Nothing) As Integer

        ' Fire up the help screen
        'Developer Guide No. 20
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        Return PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID)


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
                If m_oServices IsNot Nothing Then
                    m_oServices.Dispose()
                    m_oServices = Nothing
                End If

                If Not (m_oPMLock Is Nothing) Then

                    m_oPMLock = Nothing
                End If
                If m_oSwiftLink IsNot Nothing Then
                    m_oSwiftLink.Dispose()
                    m_oSwiftLink = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub

    Private Const vbFormCode As Integer = BootMode.Normal
    ' ***************************************************************** '
    ' Name: UnloadControl
    '
    ' Description: Cleans up then unloads the control
    '
    ' ***************************************************************** '
    Public Function UnLoadControl(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer

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
                    'eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Function
                End If
            End If

            Dispose()

            'Terminate will have done this, but just in case...
            If Not (m_oBusiness Is Nothing) Then
                ' Terminate the business object

                m_oBusiness.Dispose()

                ' Destroy the instance of the business object
                ' from memory.
                m_oBusiness = Nothing

            End If

            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ValidateParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function ValidateParty() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)
    ' PRIVATE Methods (Begin)

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
            'SP090998
            'DC 24/01/00 Added extra parameter for Party Type Id
            'SD 10/07/02 START Added Loyalty no, Sub Branch and Trading name

            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vPartyTypeID:=m_iPartyTypeId, vSourceID:=m_sBranch, vShortname:=m_sShortName, vResolvedName:=m_sResolved, vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt, vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId, vLoyaltyNumber:=m_sLoyaltyNo, vSubBranchName:=m_sSubBranchName)

            'SD 10/07/02 END
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get additional details required for display that not stored on this
            'record

            m_lReturn = m_oBusiness.GetOtherSummaryDetails(vAgentCnt:=m_lAgentCnt, vAgentRef:=m_sAgentRef, vAgentName:=m_sAgentName, vConsultantCnt:=m_lConsultantCnt, vConsultantRef:=m_sConsultantRef, vConsultantname:=m_sConsultantName, vPartyCnt:=m_lPartyCnt, vAssociates:=m_vAssociates)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            If Information.IsArray(m_vAssociates) Then
                m_lAssociatedCnt = CInt(m_vAssociates(0, 0))
                m_sAssociateRole = CStr(m_vAssociates(1, 0))
                m_sAssociatedName = CStr(m_vAssociates(2, 0))
            End If

            'Get addresses for the party

            m_lReturn = m_oBusiness.GetAddressDetails(vPartyCnt:=m_lPartyCnt, vAddresses:=m_vAddresses)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get addresse type lookups for the party

            m_lReturn = m_oBusiness.GetAddressTypeLookups(vAddressTypes:=m_vAddressTypes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If
            'EK 22/10/99 Not always set up so replaced for now
            'Get contacts for the party

            m_lReturn = m_oBusiness.GetAddressContactDetails(vPartyCnt:=m_lPartyCnt, vContacts:=m_vAddressContacts)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the contact details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If
            'EK 22/10/99
            'Get contact type information for the party

            m_lReturn = m_oBusiness.GetContactTypeLookups(vContactTypes:=m_vContactTypes)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get contact type lookup details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")

                Return result
            End If

            'Get contact information for the party

            m_lReturn = m_oBusiness.GetContactDetails(vPartyCnt:=m_lPartyCnt, vContacts:=m_vContacts)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get contact details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")

                Return result
            End If

            If m_bSwiftInstalled Then

                m_lReturn = CreateSwiftLink()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function CreateSwiftLink failed.")
                End If

                ' We have a link to swift?

                m_lReturn = m_oSwiftLink.IsPartyLinked(v_lPartyCnt:=m_lPartyCnt, r_bLinked:=m_bSwiftLinked)
            End If

            'SD 12/07/2002 Code for DOB
            'if the party type is personal we need the DOB additionally
            If m_iPartyTypeId = 1 Then

                m_oServices.PartyCnt = m_lPartyCnt

                m_lReturn = m_oServices.GetDetails()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = m_lReturn

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get contact details from the services object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")

                    Return result
                End If

                m_dtDOB = ReflectionHelper.GetMember(m_oServices, "DateOfBirth")
            End If

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

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            txtIDReference.Text = m_sShortName.Trim()
            txtResolvedName.Text = m_sResolved.Trim()

            txtAgent.Text = m_sAgentName.Trim()
            txtConsultant.Text = m_sConsultantName.Trim()
            txtBranch.Text = m_sBranch.Trim()
            'SD 10/07/02 START added extra feilds
            txtTradingName.Text = m_sTradingName.Trim()
            txtSubBranch.Text = m_sSubBranchName.Trim()
            txtLoyaltyNo.Text = m_sLoyaltyNo.Trim()
            'SD 10/07/02 END

            'for different regional date settings.
            'If m_dtDOB = "29/12/1899" Then
            'If Format(m_dtDOB, "DD/MM/YYYY") = "30/12/1899" Then
            'sj 16/08/2002 - start
            If CInt(m_dtDOB.ToOADate) = 0 Or CInt(m_dtDOB.ToOADate) = -1 Then
                txtDOB.Text = ""
            Else
                'txtDOB = Format(m_dtDOB, "DD/MM/YYYY")
                txtDOB.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, m_dtDOB)

            End If
            'sj 16/08/2002 - end

            'EK 22/10/99
            'Populate fields from the address contact data
            'SD 11/07/02 Redundant Function
            'PopulateAddressContacts

            'Populate fields from the contact data
            PopulateContacts()

            'Fill the address list view
            PopulateAddresses()

            ' CTAF 011200
            If m_bSwiftInstalled Then
                If m_bSwiftLinked Then
                    lblExists.Text = "exists."
                    cmdDeleteLink.Visible = True
                Else
                    lblExists.Text = "does not exist."
                    cmdDeleteLink.Visible = False
                End If
            Else
                ' CTAF 011200
                lblSwiftLink.Visible = False
                picLink.Visible = False
                lblExists.Visible = False
                cmdDeleteLink.Visible = False
            End If

            'frmInterface.Caption = "Personal Client: " & m_sShortName & " " & m_sMainPostCode

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
            For Each ctlFormControl As Control In Controls_Renamed
                ' Check the type of the control.
                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                    'ElseIf (TypeOf ctlFormControl Is SSOption) Then
                    '            ctlFormControl.Enabled = Not lDisabled&
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

            ' Display all language specific captions.

            '    Me.Caption = GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACInterfaceTitle, _
            'iDataType:=PMResString)

            ' Check for an error.
            '    If (Me.Caption = "") Then
            ' Failed to get data from the resource file.
            '        DisplayCaptions = PMFalse

            ' Log Error.
            '        LogMessage _
            'iType:=PMLogError, _
            'sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
            '"Please check the file exists and the correct captions are available", _
            'vApp:=ACApp, _
            'vClass:=ACClass, _
            'vMethod:="DisplayCaptions"

            '        Exit Function
            '    End If

            '    cmdOK.Caption = GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACOKButton, _
            'iDataType:=PMResString)

            '    cmdCancel.Caption = GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACCancelButton, _
            'iDataType:=PMResString)

            '    cmdHelp.Caption = GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACHelpButton, _
            'iDataType:=PMResString)


            lblIDReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblResolvedName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAddress.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddress, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblConsultant.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConsultant, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'PN 21867 - Adding a label for Contact Details

            lblClientContactDetails.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientContactDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
            'SD 30/07/2002
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSource, ctlLookup:=cboBranchName)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            txtBranch.Text = cboBranchName.Text
            ' {* USER DEFINED CODE (End) *}

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

            m_lReturn = LockParty()

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

            'SP090998
            'm_lPartyCnt& = 25

            m_lReturn = m_oBusiness.GetDetails(vPartyCnt:=m_lPartyCnt)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If



            ' {* USER DEFINED CODE (End) *}

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
    'Developer Guide No.101
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox, Optional ByRef bSecondary As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lRow, lRow2 As Integer
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
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                'Developer Guide No.153
                Dim NewIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))
                ' Check if this is the selected index.
                If bSecondary Then
                    lRow2 = lRow + 1
                Else
                    lRow2 = lRow
                End If
                If CStr(m_vLookupValues(ACValueID, lRow2)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow2)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'Developer Guide No.153
                        ctlLookup.SelectedIndex = NewIndex
                    End If

                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow2)) = "" Then

                'Developer Guide No.153
                ctlLookup.SelectedIndex = 0
            End If

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

            '    ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
                    'eck120500 - lookuptype AllEffective
                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
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

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.

            '***** BEWARE THIS COMMAND MAY SELF-CORRUPT IF MODIFIED
            'IF THIS HAPPENS RESTORE TO COMMENTED VERSION
            ' Check the task.
            '    Select Case (m_iTask)
            '        Case PMAdd
            '            m_lReturn& = m_oBusiness.EditAdd(lRow:=lBusinessDataID&, _
            ''                                    vPartyTitleCode:=m_sPartyTitleCode, _
            ''                                    vForename:=m_sForeName$, vInitials:=m_sInitials$, _
            ''                                    vEmploymentStatusCode:=m_sEmploymentStatusCode, _
            ''                                    vEmployerBusiness:=m_lEmployerBusiness, _
            ''                                    vSecondaryEmploymentStatusC:=m_sSecondaryEmploymentStatusCode, _
            ''                                    vSecondaryEmployerBusiness:=m_lSecondaryEmployerBusiness, _
            ''                                    vMaritalStatusCode:=m_sMaritalStatusCode, _
            ''                                    vNumberOfChildren:=m_lNumberOFChildren, vNationalityId:=m_lNationalityId, _
            ''                                    vSeasonalGiftId:=m_lSeasonalGiftId, _
            ''                                    vMailshot:=m_iMailshot, vIsPetOwner:=m_iIsPetOwner, _
            ''                                    vAccommodationTypeCode:=m_sAccommodationTypeCode, _
            ''                                    vShortName:=m_sShortName$, vName:=m_sSurName$, vResolved:=m_sResolved, _
            ''                                    vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, _
            ''                                    vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt, _
            ''                                    vFileCode:=m_sFileCode, vCurrencyId:=m_iCurrencyId, _
            ''                                    vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, _
            ''                                    vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, _
            ''                                    vPaymentTermCode:=m_sPaymentTermCode, vCCJs:=m_lCCJs, vPartyLifestyleId:=m_lPartyLifestyleId, vPartyLifeStyleName:=m_sPartyLifeStyleName, _
            ''                                    vCategory:=m_lCategory, vDateOfBirth:=m_dtDOB, vGender:=m_sGender, _
            ''                                    vOccupationCode:=m_sOccupatim_iIsSmoker)
            '        Case PMEdit
            '            m_lReturn& = m_oBusiness.EditUpdate(lRow:=lBusinessDataID&, _
            ''                                    vPartyTitleCode:=m_sPartyTitleCode, _
            ''                                    vForename:=m_sForeName$, vInitials:=m_sInitials$, _
            ''                                    vEmploymentStatusCode:=m_sEmploymentStatusCode, _
            ''                                    vEmployerBusiness:=m_lEmployerBusiness, _
            ''                                    vSecondaryEmploymentStatusC:=m_sSecondaryEmploymentStatusCode, _
            ''                                    vSecondaryEmployerBusiness:=m_lSecondaryEmployerBusiness, _
            ''                                    vMaritalStatusCode:=m_sMaritalStatusCode, _
            ''                                    vNumberOfChildren:=m_lNumberOFChildren, vNationalityId:=m_lNationalityId, _
            ''                                    vSeasonalGiftId:=m_lSeasonalGiftId, _
            ''                                    vMailshot:=m_iMailshot, vIsPetOwner:=m_iIsPetOwner, _
            ''                                    vAccommodationTypeCode:=m_sAccommodationTypeCode, _
            ''                                    vShortName:=m_sShortName$, vName:=m_sSurName$, vResolved:=m_sResolved, _
            ''                                    vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, _
            ''                                    vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt, _
            ''                                    vFileCode:=m_sFileCode, vCurrencyId:=m_iCurrencyId, _
            ''                                    vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, _
            ''                                    vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, _
            ''                                    vPaymentTermCode:=m_sPaymentTermCode, vCCJs:=m_lCCJs, vPartyLifestyleId:=m_lPartyLifestyleId, vPartyLifeStyleName:=m_sPartyLifeStyleName, _
            ''                                    vCategory:=m_lCategory, vDateOfBirth:=m_dtDOB, vGender:=m_sGender, _
            ''                                    vOccupationCode:=m_sOccupation, vSecondaryOccupationCode:=m_sSecondaryOccupation, vIsSmoker:=m_iIsSmoker)
            '    End Select
            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        InterfaceToBusiness = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to assign the interface details to business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="InterfaceToBusiness"
            '    End If

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
        Dim sMsg As String = ""


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
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

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
    ' Name: LockParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function LockParty() As Integer

        Dim result As Integer = 0
        Dim sLockedBy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oPMLock.LockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Party currently locked by " & sLockedBy & _
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Party Lock")
                        Return result
                    End If


                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Name: PopulateAddresses
    '
    ' Description: Gets the correspondence address and displays it
    '
    ' ***************************************************************** '
    Private Function PopulateAddresses() As Integer

        Dim result As Integer = 0
        Dim iCorrespondence As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Just go if no addresses
            If Not Information.IsArray(m_vAddresses) Then
                Return result
            End If

            m_lAddressCount = 0

            'First find which address type is correspondence address

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If CStr(m_vAddressTypes(2, k)).Trim() = gSIRLibrary.SIRMainAddressABICode Then
                    iCorrespondence = CInt(m_vAddressTypes(0, k))
                    Exit For
                End If
            Next k

            ' Assign the details to the interface.
            For i As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                If CDbl(m_vAddresses(1, i)) = iCorrespondence Then
                    If CStr(m_vAddresses(2, i)).Trim() <> "" Then
                        txtAddress.Text = CStr(m_vAddresses(2, i))
                    End If
                    If CStr(m_vAddresses(3, i)).Trim() <> "" Then
                        If txtAddress.Text.Trim() <> "" Then
                            txtAddress.Text = txtAddress.Text & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        txtAddress.Text = txtAddress.Text & CStr(m_vAddresses(3, i))
                    End If
                    If CStr(m_vAddresses(4, i)).Trim() <> "" Then
                        If txtAddress.Text.Trim() <> "" Then
                            txtAddress.Text = txtAddress.Text & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        txtAddress.Text = txtAddress.Text & CStr(m_vAddresses(4, i))
                    End If
                    If CStr(m_vAddresses(5, i)).Trim() <> "" Then
                        If txtAddress.Text.Trim() <> "" Then
                            txtAddress.Text = txtAddress.Text & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        txtAddress.Text = txtAddress.Text & CStr(m_vAddresses(5, i))
                    End If
                    If CStr(m_vAddresses(0, i)).Trim() <> "" Then
                        If txtAddress.Text.Trim() <> "" Then
                            txtAddress.Text = txtAddress.Text & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        txtAddress.Text = txtAddress.Text & CStr(m_vAddresses(0, i))
                    End If
                    'MSS210901 - Added for merge
                    If m_sUnderwritingOrAgency = "U" Then
                        If CStr(m_vAddresses(7, i)).Trim() <> "" Then
                            If txtAddress.Text.Trim() <> "" Then
                                txtAddress.Text = txtAddress.Text & Strings.Chr(13) & Strings.Chr(10)
                            End If
                            txtAddress.Text = txtAddress.Text & CStr(m_vAddresses(7, i))
                        End If
                    End If
                    'MSS210901 - Merge end
                    Exit For
                End If

            Next i

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'EK 22/10/99 Only used if there area no address contacts
    ' ***************************************************************** '
    ' Name: PopulateContacts
    '
    ' Description: Fills the listview control with contact details
    '
    ' ***************************************************************** '
    Private Function PopulateContacts() As Integer

        Dim result As Integer = 0
        Dim sTemp As New StringBuilder
        Dim oListItem As ListViewItem

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            If Not Information.IsArray(m_vContacts) Then
                Return result
            End If
            lvwContacts.Items.Clear()

            'SD 12/07/2002 SOme textboxes replaced by listview control
            ' Assign the details to the interface.
            For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwContacts.Items.Add(CStr(m_vContacts(4, i)).Trim())

                ' Assign details to other the columns
                ' Column 2
                sTemp = New StringBuilder(CStr(m_vContacts(1, i)).Trim() & " " & CStr(m_vContacts(2, i)).Trim())

                If CStr(m_vContacts(3, i)).Trim() <> "" Then
                    sTemp.Append(" ext: " & CStr(m_vContacts(3, i)).Trim())
                End If
                sTemp = New StringBuilder(sTemp.ToString().Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = sTemp.ToString()

                sTemp = New StringBuilder("")
                If CStr(m_vContacts(5, i)).Trim() <> "" Then
                    sTemp = New StringBuilder(CStr(m_vContacts(5, i)).Trim())
                End If
                sTemp = New StringBuilder(sTemp.ToString().Trim())
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = sTemp.ToString()

            Next i
            '    'Populate the cells


            ' Assign the details to the interface.
            '    For i = LBound(m_vContacts, 2) To UBound(m_vContacts, 2)
            '
            '        ' {* USER DEFINED CODE (Begin) *}
            '
            '        sTemp = Trim$(m_vContacts(1, i)) & " " & Trim$(m_vContacts(2, i))
            '
            '        If (Trim$(m_vContacts(3, i)) <> "") Then
            '            sTemp = sTemp & " ext: " & Trim$(m_vContacts(3, i))
            '        End If
            '        sTemp = Trim$(sTemp)
            '        For j = LBound(m_vContactTypes, 2) To UBound(m_vContactTypes, 2)
            '            If Trim$(m_vContactTypes(1, j)) = Trim$(m_vContacts(4, i)) Then
            '            Select Case Trim$(m_vContactTypes(2, j))
            '             Case "TELEPHONE"
            ''DC 24/01/00
            ''if a corporate client and the work phone no. not already
            ''populated, then set work phone no.
            ''otherwise, store in home phone no.
            '                If m_iPartyTypeId = 4 Then
            '                    If txtWorkPhone.Text = "" Then
            '                        txtWorkPhone.Text = sTemp
            '                        If txtHomePhone.Text = txtWorkPhone.Text Then
            '                            txtHomePhone.Text = ""
            '                        End If
            '                    Else
            '                        If txtHomePhone.Text = "" Then
            '                            txtHomePhone.Text = sTemp
            '                        End If
            '                    End If
            '                Else
            '                    If txtHomePhone.Text = "" Then
            '                        txtHomePhone.Text = sTemp
            '                    End If
            '                End If
            '
            '            Case "FAX"
            ''DC 24/01/00
            ''if a corporate client and the work fax no. not already
            ''populated, then set work fax no.
            ''otherwise, store in home fax no.
            '                If m_iPartyTypeId = 4 Then
            '                    If txtWorkFax.Text = "" Then
            '                        txtWorkFax.Text = sTemp
            '                        If txtWorkFax.Text = txtHomeFax.Text Then
            '                            txtHomeFax.Text = ""
            '                        End If
            '                    Else
            '                        If txtHomeFax.Text = "" Then
            '                            txtHomeFax.Text = sTemp
            '                        End If
            '                    End If
            '                Else
            '                    If txtHomeFax.Text = "" Then
            '                            txtHomeFax.Text = sTemp
            '                    End If
            '                End If
            '
            '            Case "E-MAIL"
            '                If txtEmailAddress.Text = "" Then
            '                    txtEmailAddress.Text = m_vContacts(2, i)
            '                End If
            '            Case "MOBILE"
            '                If txtMobilePhone.Text = "" Then
            '                    txtMobilePhone.Text = sTemp
            '                End If
            '            Case "WEB"
            '                If txtWebAddress.Text = "" Then
            '                    txtWebAddress.Text = m_vContacts(2, i)
            '                End If
            '            End Select
            '            Exit For
            '          End If
            '        Next j
            '         ' {* USER DEFINED CODE (End) *}
            '
            '    Next i

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



        Try

            Return gPMConstants.PMEReturnCode.PMTrue

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
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try


            'It's all display-only, and no special formatting
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

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
            m_ctlTabFirstLast(ACControlStart, 0) = txtIDReference
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtWebAddress
            'SD 12/07/2002 Changed last control
            m_ctlTabFirstLast(ACControlEnd, 0) = txtLoyaltyNo
            '
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
            'CenterForm Me

            'sj 02/10/2002 - start
            With Toolbar1

                .ImageList = ImageList1
                .Items.Item("_Event").ImageIndex = 10
            End With
            'sj 02/10/2002 - end

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            tabMainTab.Visible = True
            'sj 02/10/2002 - start
            'tabMainTab.Top = 0
            tabMainTab.Top = VB6.TwipsToPixelsY(360)
            'sj 02/10/2002 - end
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            'SD 11/07/002 START Set row select on listview
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwContacts.Handle.ToInt32(), v_vShowRowSelect:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'SD 11/07/002 END Set row select on listview
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
    '
    ' Name: DeleteSwiftLink
    '
    ' Description:
    '
    ' History: 30/11/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteSwiftLink() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = CreateSwiftLink()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function CreateSwiftLink failed.")
            End If


            m_lReturn = m_oSwiftLink.DeleteLink(v_lPartyCnt:=m_lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Read the data in again
            m_lReturn = GetParty()

            ' Set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteSwiftLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteSwiftLink", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdDeleteLink_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteLink.Click

        m_lReturn = DeleteSwiftLink()

    End Sub

    'sj 02/10/2002 - start
    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Event.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Select Case Button.Name
            Case "_Event"
                'developer guide no.294
                SharedFiles.iPMBListEvents.g_oObjectManager = g_oObjectManager
                m_lReturn = SharedFiles.iPMBListEvents.ShowEvents(v_lPartyCnt:=m_lPartyCnt, v_sTransactionType:=m_sTransactionType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar1_ButtonClick")
                    Exit Sub
                End If

        End Select

    End Sub

    ' PRIVATE Methods (End)
    ' PRIVATE Events (Begin)

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_Enabled = m_def_Enabled

        'Developer Guide No solution 2
        m_Font = Me.Font
        m_BackStyle = m_def_BackStyle
        'TODO
        m_BorderStyle = Windows.Forms.BorderStyle.None
        m_PartyCnt = m_def_PartyCnt
    End Sub

    Private Sub uctPartySummControl_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
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

        Catch

            ' Error Section.

            Exit Sub
        End Try


    End Sub

    'Load property values from storage


    'Developer Guide No  1 (No Solution)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)

        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))
        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))
        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))
        m_Font = PropBag.ReadProperty("Font", Me.Font)

        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))
        m_BorderStyle = PropBag.ReadProperty("BorderStyle", m_def_BorderStyle)
        m_PartyCnt = CInt(PropBag.ReadProperty("PartyCnt", m_def_PartyCnt))
    End Sub

    Private Sub uctPartySummControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        ' Maintain minimum width
        If VB6.PixelsToTwipsX(Width) < 9090 Then Width = VB6.TwipsToPixelsX(9090)
        ' and height width
        If VB6.PixelsToTwipsY(Height) < 4950 Then Height = VB6.TwipsToPixelsY(4950)
        

    End Sub


    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)


        'Developer Guide No 1
        PropBag.WriteProperty("Font", m_Font, Me.Font)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("PartyCnt", m_PartyCnt, m_def_PartyCnt)
    End Sub

    ' PRIVATE Events (End)

    Private Function GetSystemOptions() As Integer
        Dim result As Integer = 0



        Dim oOption As bSIROptions.Business
        Dim sValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get an instance of bSIROption.Business
            Dim temp_oOption As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oOption, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oOption = temp_oOption
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get instance of bSIROptions.Business")
            End If

            'Get the option value for swift installed check box.

            m_lReturn = oOption.GetOption(iOptionNumber:=ACSwiftInstalledOption, sValue:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , " + "Failed to read system option number " & ACSwiftInstalledOption)
            End If

            'Set variable from returned value
            m_bSwiftInstalled = sValue.Trim() = "1"

            'Terminate instance of bSIROption.Business

            oOption.Dispose()
            oOption = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CreateSwiftLink() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSwiftLink Is Nothing Then

                'Get an instance of Swift Link
                Dim temp_m_oSwiftLink As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oSwiftLink, sClassName:="iPMBSwiftLink.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oSwiftLink = temp_m_oSwiftLink
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get instance of iPMBSwiftLink.Interface")
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateSwiftLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateSwiftLink", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class