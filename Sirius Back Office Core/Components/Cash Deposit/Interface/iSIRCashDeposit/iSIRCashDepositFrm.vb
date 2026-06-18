Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:06/10/2009
    '
    ' Description: Main interface.
    '
    ' Edit History: Sankar Chellathurai
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    Private m_bIsInitialised As Boolean
    'Constants for Defining Width of Columns in List View

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRCashDeposit.General

    ' Declare an instance of the FormControl object
    'Private m_oFormFields As iPMFormControl.FormFields

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iTask As Integer

    Private m_sPartyType As String = ""

    Private m_lPartyCnt As Integer
    Private m_sPartyCode As String = ""
    Private m_sPartyName As String = ""
    Private m_vCashDepositItem As Object
    Private m_vCashDepositLinkedAccounts As Object

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRCashDeposit.Business

    'Private m_vResultArray As Variant
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_bFromAgentOrClientMaintenance As Boolean

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property


    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.5.1.1)

    Public Property CashDepositItem() As Object
        Get
            Return m_vCashDepositItem
        End Get
        Set(ByVal Value As Object)


            m_vCashDepositItem = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property


    Public Property PartyCode() As String
        Get
            Return m_sPartyCode
        End Get
        Set(ByVal Value As String)
            m_sPartyCode = Value
        End Set
    End Property


    Public Property PartyName() As String
        Get
            Return m_sPartyName
        End Get
        Set(ByVal Value As String)
            m_sPartyName = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property


    Public Property FromAgentOrClientMaintenance() As Boolean
        Get
            Return m_bFromAgentOrClientMaintenance
        End Get
        Set(ByVal Value As Boolean)
            m_bFromAgentOrClientMaintenance = Value
        End Set
    End Property
    'End - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.5.1.1)

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' Date :
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DisplayCaptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=MainModule.g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                gPMFunctions.RaiseError(kMethodName, "Unable to Retrive Information from Resource File", gPMConstants.PMELogLevel.PMLogError)
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=MainModule.g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=MainModule.g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraPartyDetails.Text = CStr(iPMFunc.GetResData(iLangID:=MainModule.g_iLanguageID, lId:=ACFrameName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPartyCode.Text = CStr(iPMFunc.GetResData(iLangID:=MainModule.g_iLanguageID, lId:=ACPartyCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPartyName.Text = CStr(iPMFunc.GetResData(iLangID:=MainModule.g_iLanguageID, lId:=ACPartyName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=MainModule.g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=MainModule.g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch ex As Exception



            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here


            Return result
        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Try


            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


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
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch ex As Exception


            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdOK_Click", r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        End Try




    End Sub

    ' ***************************************************************** '
    ' Name: FormIntialise
    '
    ' Description: Intialise all required details of the form
    '
    ' Date:15/07/00
    '
    ' Edit History:
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        Const kMethodName As String = "Initialise"
        Dim temp_m_oBusiness As Object = Nothing
        Try

            iPMFunc.ShowFormInTaskBar_Attach()


            ' Check if already initialised
            If m_bIsInitialised Then
                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iSIRCashDeposit.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc)
            'Get an instance of the business object via the public object manager.

            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRCashDeposit.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRCashDeposit.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc)

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' hold Initialised status
            m_bIsInitialised = True



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: FormLoad
    '
    ' Description: Loads all required details of the form
    '
    ' Date:15/07/00
    '
    ' Edit History:
    ' ***************************************************************** '

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            'For viewing the Form in TaskBar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Display all language specific captions.
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_Load", "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.5.1.2)
            txtPartyName.Text = m_sPartyName
            txtPartyCode.Text = m_sPartyCode

            'intiliase the user control

            'developer guide no. 9
            uctCashDepositControl.Initialise()

            'set the preoperties of the user control
            uctCashDepositControl.PartyCnt = m_lPartyCnt
            uctCashDepositControl.PartyCode = m_sPartyCode
            uctCashDepositControl.PartyName = m_sPartyName
            uctCashDepositControl.FromAgentOrClientMaintenance = m_bFromAgentOrClientMaintenance
            uctCashDepositControl.Task = gPMConstants.PMEComponentAction.PMView


            uctCashDepositControl.Load_Renamed()
            'End - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.5.1.2)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub


        End Try

    End Sub

    Private Const vbFormCode As Integer = 0
    ' ***************************************************************** '
    ' Name: Form_Query Unload
    '
    ' Description: Store all Property Details before unloading form
    '
    ' Date:11/07/00
    '
    ' Edit History:
    ' ***************************************************************** '
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        ' Forms query unload event.



        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Check if the interface has been terminated by means
        ' other than pressing the command buttons.


        If UnloadMode <> vbFormCode Then
            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

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

        ' Reset the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        eventArgs.Cancel = Cancel <> 0
    End Sub
End Class
