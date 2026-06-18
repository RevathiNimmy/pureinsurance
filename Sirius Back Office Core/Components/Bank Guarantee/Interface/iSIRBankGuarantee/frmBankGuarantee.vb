Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmInterface"
    Private m_bIsInitialised As Boolean
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lErrorNumber As Integer

    'developer guide no. 7
    Private Const vbFormCode As Integer = 0

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRBankGuarantee.General

    Private m_sCallingAppName As String = ""
    'developer guide no.101
    Private m_vPartyCnt As Object
    Private m_vAccountId As Object
    Private m_bLoadChildEdit As Boolean
    Private m_bLoadChildAdd As Boolean
    Private m_bLoadChildView As Boolean
    Private m_lBG_id As Integer
    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lStatus As Integer
    Private m_sPartyCode As String = ""
    Private m_sPartyName As String = ""
    'Start - Sankar - Bank Guarantee Bug Fixing
    Dim m_bCallFromClientManager As Boolean
    'End - Sankar - Bank Guarantee Bug Fixing



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


    Public Property LoadChildView() As Boolean
        Get
            Return m_bLoadChildView
        End Get
        Set(ByVal Value As Boolean)
            m_bLoadChildView = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
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


    'DC180202
    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property


    Public Property BGId() As Integer
        Get
            Return m_lBG_id
        End Get
        Set(ByVal Value As Integer)
            m_lBG_id = Value
        End Set
    End Property


    Public Property LoadChildEdit() As Boolean
        Get
            PartyCnt = m_bLoadChildEdit
        End Get
        Set(ByVal Value As Boolean)
            m_bLoadChildEdit = Value
        End Set
    End Property


    Public Property LoadChildAdd() As Boolean
        Get
            PartyCnt = m_bLoadChildAdd
        End Get
        Set(ByVal Value As Boolean)
            m_bLoadChildAdd = Value
        End Set
    End Property

    'developer guide no.101
    Public Property PartyCnt() As Object
        Get
            Return m_vPartyCnt
        End Get
        Set(ByVal Value As Object)

            m_vPartyCnt = Value
        End Set
    End Property

    'Start - Sankar - Bank Guarantee Bug Fixing

    Public Property CallFromClientManager() As Boolean
        Get
            Return m_bCallFromClientManager
        End Get
        Set(ByVal Value As Boolean)
            m_bCallFromClientManager = Value
        End Set
    End Property
    'End - Sankar - Bank Guarantee Bug Fixing

    Private Sub cmdAppy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAppy.Click

        'developer guide no. changed as per the project name
        m_lReturn = uctBankGuarenteeControl.UpdateBankGuaranteeDetails()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CmdAppy", "uctBankGuarenteeControl.UpdateBankGuaranteeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        Else
            cmdAppy.Enabled = False
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            If CallFromClientManager Then
                Me.Hide()
            Else
                ' Process the next set of actions.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        If cmdAppy.Enabled Then
            cmdAppy_Click(cmdAppy, New EventArgs())
        End If
        Me.Close()
    End Sub

    Private Sub Form_Initialize_Renamed()
        Const kMethodName As String = "Initialise"

        Try




            ' Check if already initialised
            If m_bIsInitialised Then
                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iSIRBankGuarantee.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If


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


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        '	'intiliase the user control

        'developer guide no. 97
        uctBankGuarenteeControl.Initialise()

        ' set the preoperties of the user control
        uctBankGuarenteeControl.PartyCnt = PartyCnt
        uctBankGuarenteeControl.SelBgId = m_lBG_id

        uctBankGuarenteeControl.PartyCode = m_sPartyCode
        uctBankGuarenteeControl.PartyName = m_sPartyName

        uctBankGuarenteeControl.SelBgId = m_lBG_id

        If m_bLoadChildEdit Then
            ' Load the control with relevant data
            uctBankGuarenteeControl.Task = gPMConstants.PMEComponentAction.PMEdit
            uctBankGuarenteeControl.LoadEdit()

            m_lStatus = uctBankGuarenteeControl.Status
        ElseIf m_bLoadChildAdd Then
            uctBankGuarenteeControl.Task = gPMConstants.PMEComponentAction.PMAdd
            uctBankGuarenteeControl.LoadAdd()

            m_lStatus = uctBankGuarenteeControl.Status
        ElseIf m_bLoadChildView Then
            uctBankGuarenteeControl.Task = gPMConstants.PMEComponentAction.PMAdd
            uctBankGuarenteeControl.LoadView()

            m_lStatus = uctBankGuarenteeControl.Status
        Else
            uctBankGuarenteeControl.Task = m_iTask
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdAppy.Enabled = False
            End If
            uctBankGuarenteeControl.Load_Renamed()

        End If
        If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
            Me.Dispose()
        End If
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        ' Forms query unload event.

        Try

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
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

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

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub


End Class
