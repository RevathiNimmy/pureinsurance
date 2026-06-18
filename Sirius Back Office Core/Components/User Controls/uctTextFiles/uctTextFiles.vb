Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctTextFiles_NET.uctTextFiles")> _
Partial Public Class uctTextFiles
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event RiskGroupIdChange()
    Public Event RiskCodeIdChange()
    Public Event ClaimDescChange()
    Public Event PolicyDescChange()
    Public Event FileNumberChange()
    Public Event SlotNumberChange()
    Public Event ClaimCntChange()
    Public Event InsuranceFileCntChange()
    Public Event InsuranceFolderCntChange()
    Public Event PartyCntChange()
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event ProcessModeChange()
    Public Event StatusChange()
    Public Event TaskChange()
    Public Event CallingAppNameChange()
    ' ***************************************************************** '
    '
    ' Date: 23/08/1999
    '
    ' Description: Text File User Control
    '
    ' Edit History: TO230899 - Based on Event Control
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctTextFilesControl"

    'Developer Guide No. 
    Private Const vbFormCode As Integer = 0

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
    Dim m_BorderStyle As Integer
    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)
    Event lvwSearchDetailsClick(ByVal Sender As Object, ByVal e As lvwSearchDetailsClickEventArgs)

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lClaimCnt As Integer
    Private m_lEntityTypeId As Integer
    Private m_lEntityCnt As Integer
    Private m_lSlotNumber As Integer
    Private m_lFileNumber As Integer
    'CT 27/11/00 added source
    Private m_lSourceId As Integer

    Private m_lRiskCodeId As Integer
    Private m_lRiskGroupId As Integer

    Private m_sPolicyDesc As String = ""
    Private m_sClaimDesc As String = ""

    ' TF311298 - changed from NavProcessCode
    Private m_sInsFileType As String = ""
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRTextFile.Business
    'Private m_oBusiness As bSIRTextFile.Business

    ' Declare an instance of the Interface object.

    'Developer Guide No. 108
    'Private m_oTextFile As iPMBTextFile.Interface
    Private m_oTextFile As iPMBTextFile.Interface_Renamed
    'Private m_oTextFile As iPMBTextFile.Interface

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lItemsFound As Integer
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object
    Private Const ACTextFileClientLock As String = "Client Text Files"
    Private Const ACTextFilePolicyLock As String = "Policy Text Files"
    Private Const ACTextFileClaimLock As String = "Claim Text Files"

    Private m_sDocumentTemplateDescription As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' CF 020799
    <Browsable(False)> _
    Public ReadOnly Property Controls_Renamed() As Object
        Get
            Return Me.Controls_Renamed
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

    <Browsable(False)> _
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

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
    Public Property ClaimCnt() As Integer
        Get

            Return m_lClaimCnt

        End Get
        Set(ByVal Value As Integer)

            m_lClaimCnt = Value
            RaiseEvent ClaimCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property SlotNumber() As Integer
        Get

            Return m_lSlotNumber

        End Get
        Set(ByVal Value As Integer)

            m_lSlotNumber = Value
            RaiseEvent SlotNumberChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property FileNumber() As Integer
        Get

            Return m_lFileNumber

        End Get
        Set(ByVal Value As Integer)

            m_lFileNumber = Value
            RaiseEvent FileNumberChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property PolicyDesc() As String
        Get

            Return m_sPolicyDesc

        End Get
        Set(ByVal Value As String)

            m_sPolicyDesc = Value
            RaiseEvent PolicyDescChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property ClaimDesc() As String
        Get

            Return m_sClaimDesc

        End Get
        Set(ByVal Value As String)

            m_sClaimDesc = Value
            RaiseEvent ClaimDescChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property RiskCodeId() As Integer
        Get

            Return m_lRiskCodeId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskCodeId = Value
            RaiseEvent RiskCodeIdChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property RiskGroupId() As Integer
        Get

            Return m_lRiskGroupId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskGroupId = Value
            RaiseEvent RiskGroupIdChange()

        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    Public Function CancelClick() As Integer

        ' Click event of the Cancel button.

        Try


            Return CancelTextFiles()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowHelpScreen
    '
    ' Description: Shows the help screen
    '
    ' ***************************************************************** '
    Public Function ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing, Optional ByRef ScreenHelpID As Object = Nothing) As Integer
        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        Return PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)


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
    ' Name: EditClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function EditClick() As Integer
        Dim result As Integer = 0


        Dim vTask As gPMConstants.PMEComponentAction

        Dim oPMLock As bPMLock.User
        Dim sLockedBy, sLockType As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC210405 : PN20426 : check if any slots first
            If lvwSearchDetails.Items.Count = 0 Then

                MessageBox.Show("Slots need to be created via Document Slot Maintenance, before Text Files can be editted.", "Text File Slots", MessageBoxButtons.OK)
                Return result

            End If

            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetLocalBusiness)
            oPMLock = temp_oPMLock

            m_lReturn = CType(SelectTextFiles(), gPMConstants.PMEReturnCode)

            If m_lEntityTypeId = PMBConst.PMBClientTextFile Then
                sLockType = ACTextFileClientLock
            ElseIf m_lEntityTypeId = PMBConst.PMBPolicyTextFile Then
                sLockType = ACTextFilePolicyLock
            Else
                sLockType = ACTextFileClaimLock
            End If


            m_lReturn = oPMLock.LockKey(sKeyName:=sLockType, vKeyValue:=m_lEntityCnt, iUserID:=g_oObjectManager.UserID, sCurrentlylockedby:=sLockedBy, v_botheruseronly:=True, vKey2Value:=m_lSlotNumber)

            If sLockedBy.Length > 0 Then
                ' text file is already locked
                sLockedBy = "This text file is currently locked by '" & sLockedBy & "'." & Strings.Chr(13) & Strings.Chr(10) & "Please try again later."
                MessageBox.Show(sLockedBy, "Text File Slots", MessageBoxButtons.OK)
            Else
                'Here is where we start to call the text file interface...
                If m_oTextFile Is Nothing Then
                    Dim temp_m_oTextFile As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_m_oTextFile, sClassName:="iPMBTextFile.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    m_oTextFile = temp_m_oTextFile
                End If

                If m_lFileNumber = 0 Then
                    vTask = gPMConstants.PMEComponentAction.PMAdd
                Else
                    vTask = gPMConstants.PMEComponentAction.PMEdit
                End If


                m_lReturn = m_oTextFile.SetProcessModes(vTask:=vTask)


                m_oTextFile.PartyCnt = m_lPartyCnt

                m_oTextFile.InsuranceFolderCnt = m_lInsuranceFolderCnt

                m_oTextFile.InsuranceFileCnt = m_lInsuranceFileCnt

                m_oTextFile.ClaimCnt = m_lClaimCnt

                m_oTextFile.RiskCodeId = m_lRiskCodeId

                m_oTextFile.RiskGroupId = m_lRiskGroupId

                m_oTextFile.SlotNumber = m_lSlotNumber

                m_oTextFile.FileNumber = m_lFileNumber
                'CT 27/11/00 passing through sourceid

                m_oTextFile.SourceID = m_lSourceId

                m_oTextFile.DocumentTemplateDescription = m_sDocumentTemplateDescription


                m_lReturn = m_oTextFile.Start


                m_lReturn = oPMLock.UnLockKey(sKeyName:=sLockType, vKeyValue:=m_lEntityCnt, iUserID:=g_oObjectManager.UserID, bIgnoreNoLock:=True, vKey2Value:=m_lSlotNumber)
            End If


            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrintClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function PrintClick() As Integer
        Dim result As Integer = 0


        Dim vTask As Byte

        Dim oPMLock As bPMLock.User
        Dim sLockedBy, sLockType As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC210405 : PN20426 : check if any slots first
            If lvwSearchDetails.Items.Count = 0 Then

                MessageBox.Show("Slots need to be created via Document Slot Maintenance, before Text Files can be printed.", "Text File Slots", MessageBoxButtons.OK)
                Return result

            End If

            m_lReturn = CType(SelectTextFiles(), gPMConstants.PMEReturnCode)

            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetLocalBusiness)
            oPMLock = temp_oPMLock

            'Here is where we start to call the text file interface...

            If m_oTextFile Is Nothing Then
                Dim temp_m_oTextFile As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oTextFile, sClassName:="iPMBTextFile.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oTextFile = temp_m_oTextFile
            End If

            If m_lEntityTypeId = PMBConst.PMBClientTextFile Then
                sLockType = ACTextFileClientLock
            ElseIf m_lEntityTypeId = PMBConst.PMBPolicyTextFile Then
                sLockType = ACTextFilePolicyLock
            Else
                sLockType = ACTextFileClaimLock
            End If


            m_lReturn = oPMLock.LockKey(sKeyName:=sLockType, vKeyValue:=m_lEntityCnt, iUserID:=g_oObjectManager.UserID, sCurrentlylockedby:=sLockedBy, v_botheruseronly:=True, vKey2Value:=m_lSlotNumber)

            If sLockedBy.Length > 0 Then
                ' text file is already locked
                sLockedBy = "This text file is currently locked by '" & sLockedBy & "'." & Strings.Chr(13) & Strings.Chr(10) & "Please try again later."
                MessageBox.Show(sLockedBy, "Text File Slots", MessageBoxButtons.OK)
            Else
                'vTask = PMprint
                vTask = 20


                m_lReturn = m_oTextFile.SetProcessModes(vTask:=vTask)


                m_oTextFile.PartyCnt = m_lPartyCnt

                m_oTextFile.InsuranceFolderCnt = m_lInsuranceFolderCnt

                m_oTextFile.InsuranceFileCnt = m_lInsuranceFileCnt

                m_oTextFile.ClaimCnt = m_lClaimCnt

                m_oTextFile.RiskCodeId = m_lRiskCodeId

                m_oTextFile.RiskGroupId = m_lRiskGroupId

                m_oTextFile.SlotNumber = m_lSlotNumber

                m_oTextFile.FileNumber = m_lFileNumber


                m_lReturn = m_oTextFile.Start


                m_lReturn = oPMLock.UnLockKey(sKeyName:=sLockType, vKeyValue:=m_lEntityCnt, iUserID:=g_oObjectManager.UserID, bIgnoreNoLock:=True, vKey2Value:=m_lSlotNumber)
            End If


            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


        Dim vTask As gPMConstants.PMEComponentAction

        Dim oPMLock As bPMLock.User
        Dim sLockedBy, sLockType As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC210405 : PN20426 : check if any slots first
            If lvwSearchDetails.Items.Count = 0 Then

                MessageBox.Show("Slots need to be created via Document Slot Maintenance, before Text Files can be deleted.", "Text File Slots", MessageBoxButtons.OK)
                Return result

            End If

            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetLocalBusiness)
            oPMLock = temp_oPMLock

            m_lReturn = CType(SelectTextFiles(), gPMConstants.PMEReturnCode)

            'Here is where we start to call the text file interface...

            If m_oTextFile Is Nothing Then
                Dim temp_m_oTextFile As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oTextFile, sClassName:="iPMBTextFile.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oTextFile = temp_m_oTextFile
            End If

            If m_lEntityTypeId = PMBConst.PMBClientTextFile Then
                sLockType = ACTextFileClientLock
            ElseIf m_lEntityTypeId = PMBConst.PMBPolicyTextFile Then
                sLockType = ACTextFilePolicyLock
            Else
                sLockType = ACTextFileClaimLock
            End If


            m_lReturn = oPMLock.LockKey(sKeyName:=sLockType, vKeyValue:=m_lEntityCnt, iUserID:=g_oObjectManager.UserID, sCurrentlylockedby:=sLockedBy, v_botheruseronly:=True, vKey2Value:=m_lSlotNumber)

            If sLockedBy.Length > 0 Then
                ' text file is already locked
                sLockedBy = "This text file is currently locked by '" & sLockedBy & "'." & Strings.Chr(13) & Strings.Chr(10) & "Please try later."
                MessageBox.Show(sLockedBy, "Text File Slots", MessageBoxButtons.OK)
            Else
                vTask = gPMConstants.PMEComponentAction.PMDelete


                m_lReturn = m_oTextFile.SetProcessModes(vTask:=vTask)


                m_oTextFile.PartyCnt = m_lPartyCnt

                m_oTextFile.InsuranceFolderCnt = m_lInsuranceFolderCnt

                m_oTextFile.InsuranceFileCnt = m_lInsuranceFileCnt

                m_oTextFile.ClaimCnt = m_lClaimCnt

                m_oTextFile.RiskCodeId = m_lRiskCodeId

                m_oTextFile.RiskGroupId = m_lRiskGroupId

                m_oTextFile.SlotNumber = m_lSlotNumber

                m_oTextFile.FileNumber = m_lFileNumber
                'CT 27/11/00 passing through sourceid

                m_oTextFile.SourceID = m_lSourceId


                m_lReturn = m_oTextFile.Start


                m_lReturn = oPMLock.UnLockKey(sKeyName:=sLockType, vKeyValue:=m_lEntityCnt, iUserID:=g_oObjectManager.UserID, bIgnoreNoLock:=True, vKey2Value:=m_lSlotNumber)
            End If


            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                m_iTask = CInt(vTask)
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
                'Developer Guie No 39(no solution)
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRTextFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_oFormFields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

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
                If m_oTextFile IsNot Nothing Then
                    m_oTextFile.Dispose()
                    m_oTextFile = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: UnloadControl
    '
    ' Description: Cleans up then unloads the control
    '
    ' ***************************************************************** '
    Public Function UnLoadControl(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer

        ' Forms query unload event.

        Debug.WriteLine("unload control")

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

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


            ' Terminate the general object.
            Dispose()

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
    ' Name: GetEvents
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Public Function GetTextFiles() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.
            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details from the search data storage
            ' to the interface.
            'm_lReturn& = DataToInterface()

            ' Check for errors
            'If (m_lReturn& <> PMTrue) Then
            '    ' Failed to assign the details.
            '    GetTextFiles = PMFalse
            '    Exit Function
            'End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Text Files", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTextFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lEntityTypeId = PMBConst.PMBClientTextFile
            m_lEntityCnt = m_lPartyCnt

            If m_lInsuranceFileCnt > 0 Then
                m_lEntityTypeId = PMBConst.PMBPolicyTextFile
                m_lEntityCnt = m_lInsuranceFileCnt
            End If

            If m_lClaimCnt > 0 Then
                m_lEntityTypeId = PMBConst.PMBClaimTextFile
                m_lEntityCnt = m_lClaimCnt
            End If


            m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_lEntityTypeId:=m_lEntityTypeId, v_lEntityCnt:=m_lEntityCnt)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

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

                    ' Log Error.
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

        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            m_lItemsFound = 0

            ' Check that search details are valid before
            ' continuing.

            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                m_lItemsFound += 1
                ' Assign the details to the first column.
                ' Column 1 slot number

                oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACISlotNumber, lRow)), "")
                oListItem.ImageKey = ACFindImage
                ' Assign details to the other columns

                ' Column 2 description
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIDescription, lRow)).Trim()

                ' Column 3 file number
                If CStr(m_vSearchData(ACIFileNumber, lRow)).Trim() = "" Then
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Not defined"
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIFileNumber, lRow)).Trim()
                End If

                'CT 27/11/00 added new column to show source
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACISourceId, lRow)).Trim()

                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.
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

            ' Enable the interface now that the search
            ' has completed.
            m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
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

            ' CF 210699 - Can't set properties if theres nothing in the list, so
            '             just exit.
            If lvwSearchDetails.Items.Count = 0 Then
                Return result
            End If

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            'Initialise things
            m_lSlotNumber = 0
            m_lFileNumber = 0

            m_lSlotNumber = CInt(m_vSearchData(ACISlotNumber, lSelectedItem))
            'CT 27/11/00 set source
            m_lSourceId = CInt(m_vSearchData(ACISourceId, lSelectedItem))

            If CStr(m_vSearchData(ACIFileNumber, lSelectedItem)).Trim() <> "" Then
                m_lFileNumber = CInt(m_vSearchData(ACIFileNumber, lSelectedItem))
            End If

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
    'UPGRADE_NOTE: (7001) The following declaration (PropertiesToInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function PropertiesToInterface() As Integer
    '
    'Dim result As Integer = 0
    'Dim lSelectedItem As Integer
    '
    'Try 
    '
    '
    ' Update the interface details.
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    '
    ' {* USER DEFINED CODE (End) *}
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ValidateLookups
    '
    ' Description: Validates the interface lookups.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ValidateLookups) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidateLookups() As Integer
    '
    'Dim result As Integer = 0
    'Dim lReturn, lPartyCnt As Integer
    'Static sTitle, sMessage As String
    '
    'Try 
    '
    '
    '
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
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate lookups", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            txtPolicy.Visible = False
            lblPolicy.Visible = False
            txtClaim.Visible = False
            lblClaim.Visible = False

            If m_lInsuranceFileCnt > 0 Then
                txtPolicy.Visible = True
                lblPolicy.Visible = True
                txtPolicy.Text = m_sPolicyDesc
            End If

            If m_lClaimCnt > 0 Then
                txtClaim.Visible = True
                lblClaim.Visible = True
                'DC190101 adjust display of Claim when in use
                txtClaim.Left = VB6.TwipsToPixelsX(1440)
                txtClaim.Top = VB6.TwipsToPixelsY(480)
                lblClaim.Left = VB6.TwipsToPixelsX(120)
                lblClaim.Top = VB6.TwipsToPixelsY(525)
                'DC190101
                txtClaim.Text = m_sClaimDesc
            End If

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add something here to set the default status to current


            ' {* USER DEFINED CODE (Begin) *}

            ' Set the column widths for the search list.
            lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(720))

            lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(4320))

            lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(2880))

            'CT 27/11/00 new column showing source/branch
            'lvwSearchDetails.ColumnHeaders(3).Width = 2880
            lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(2120))
            lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(500))

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ClearInterface() As Integer
    '
    'Dim result As Integer = 0
    'Dim iMsgResult As DialogResult
    'Dim sMessage, sTitle As String
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Check if the user still wishes to clear
    ' the interface.
    '

    'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '
    ' Display the message.
    'iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
    '
    ' Check message result.
    'If iMsgResult = System.Windows.Forms.DialogResult.No Then
    ' Don't continue with the clear.
    'Return result
    'End If
    '
    ' Clear the interface details.
    '
    ' Clear the search data array.
    'm_vSearchData = Nothing
    '
    ' Clear the search list details.
    'lvwSearchDetails.Items.Clear()
    '
    ' Clear the search status bar.
    'stbStatus.Text = ""
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' Set to the first tab.
    'SSTabHelper.SetSelectedIndex(tabMainTab, 0)
    '
    ' {* USER DEFINED CODE (End) *}
    '
    ' Disable parts of the interface, so the
    ' user can now only enter a new search
    'm_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try


            ' {* USER DEFINED CODE (Begin) *}

            '    'Date
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                             ctlControl:=txtDate, _
            ''                             lFieldType:=PMString, _
            ''                             lFormat:=PMFormatDateTimeShort, _
            ''                             lMandatory:=PMNonMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If

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
            ReDim m_ctlTabFirstLast(1, 2)

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

            ' {* USER DEFINED CODE (Begin) *}

            txtPolicy.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicy, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            txtClaim.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaim, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    ' Name: CancelTextFiles
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function CancelTextFiles() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                'Me.Hide
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the TextFiles", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelTextFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectTextFiles
    '
    ' Description: Called when we wish to select
    '
    ' ***************************************************************** '
    Private Function SelectTextFiles() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                '        unloadInterface
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select the TextFiles", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTextFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.
            If Status = gPMConstants.PMEReturnCode.PMCancel Then
                ' Get string messages

                'Not necessary in events...
                '        sTitle$ = GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACCancelDetailsTitle, _
                'iDataType:=PMResString)

                '        sMessage$ = GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACCancelDetails, _
                'iDataType:=PMResString)

                '        iMsgResult = MsgBox(sMessage$, _
                'vbYesNo + vbDefaultButton2 + vbQuestion, sTitle$)

                ' Check message result.
                '        If (iMsgResult = vbNo) Then
                '            ' Set return to false, meaning
                '            ' don't cancel.
                '            ProcessCommand = PMFalse
                '        End If

                ' its a cancel, so set STEPSTATUS to INCOMPLETE...

                'm_lReturn& = m_frmInterface.SetStatus(PMNavStatusIncomplete, PMNavStatusIncomplete, PMNavStatusIncomplete)

            Else
                ' Update the property member from the interface.
                m_lReturn = CType(DataToProperties(), gPMConstants.PMEReturnCode)


                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update business.
                    Return result
                End If


            End If

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


        Try

            '    ' Store the total of item found.
            '    If (IsArray(m_vSearchData) = False) Then
            '        lItemsFound& = 0
            '    Else
            '        lItemsFound& = (UBound(m_vSearchData, 2) + 1)
            '    End If

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & m_lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

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
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Check all fields for data.
    '
    'If txtPolicy.Text.Trim() <> "" Then
    'Return gPMConstants.PMEReturnCode.PMTrue
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            ' Error Section.


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)


    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        ' Double click event for the search details.

        Try

            RaiseEvent lvwSearchDetailsClick(Me, New lvwSearchDetailsClickEventArgs(m_lFileNumber))

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)



        ' Column click event for the search details

        Try

            With lvwSearchDetails

                ' If current sort column header is
                ' pressed.
                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
                End If
            End With

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lReturn = OKClick()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            Else
            End If

            '    RaiseEvent lvwSearchDetailsDblClick(m_lPartyCnt, _
            ''                                        m_lInsuranceFileCnt, _
            ''                                        m_sPolicyDesc, _
            ''                                        m_lClaimCnt, _
            ''                                        m_sClaimDesc, _
            ''                                        m_lSlotNumber, _
            ''                                        m_lFileNumber)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Set the default button
        'cmdOK.Default = True
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave

        ' LostFocus Event for the search details

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Set the default button.
        'cmdFindNow.Default = True
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Private Sub lvwSearchDetails_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Dim lSelectedItem As Integer

        Dim oListItem As ListViewItem = lvwSearchDetails.GetItemAt(x, y)

        If oListItem Is Nothing Then
            m_lFileNumber = -1
        Else

            lSelectedItem = Convert.ToString(oListItem.Tag)
            m_lFileNumber = 0
            If CStr(m_vSearchData(ACIFileNumber, lSelectedItem)).Trim() <> "" Then
                m_lFileNumber = CInt(m_vSearchData(ACIFileNumber, lSelectedItem))
            End If
            If gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIDescription, lSelectedItem))).Trim() <> "" Then
                m_sDocumentTemplateDescription = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIDescription, lSelectedItem)))
            End If

            oListItem = Nothing

        End If

    End Sub

    Private Sub uctTextFiles_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try

            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    ' PRIVATE Events (End)
End Class