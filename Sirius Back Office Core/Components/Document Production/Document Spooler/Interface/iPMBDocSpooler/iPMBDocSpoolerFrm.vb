Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
'developer guide no.186
Imports uctListDocControl

Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    Implements IDisposable


    Private Const ACClass As String = "frmInterface"
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const vbFormCode As Integer = 0
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_lClaimCnt As Integer
    'DC240603 -ISS4097 -added new parameter
    Private m_lSourceId As Integer
    ' RDC 22/09/2005
    Private m_bAutoArchiveEnabled As Boolean

    Private m_iTask As Integer
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
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

    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property

    Public Property ClaimCnt() As Integer
        Get
            Return m_lClaimCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property

    ' RDC 22/09/2005
    Public WriteOnly Property AutoArchiveEnabled() As Boolean
        Set(ByVal Value As Boolean)
            m_bAutoArchiveEnabled = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    'DC240603 -ISS4097 -new parameter
    Public Property SourceId() As Integer
        Get
            Return m_lSourceId
        End Get
        Set(ByVal Value As Integer)
            m_lSourceId = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer


        '


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With uctListDocuments1

                .Task = gPMConstants.PMEComponentAction.PMEdit
                'developer guide no.24
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0

            End With
            'developer guide no.9
            m_lReturn = uctListDocuments1.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            uctListDocuments1.ShortName = m_sShortName
            uctListDocuments1.PartyCnt = m_lPartyCnt

            uctListDocuments1.InsuranceFolderCnt = m_lInsuranceFolderCnt
            uctListDocuments1.InsuranceFileCnt = m_lInsuranceFileCnt
            uctListDocuments1.InsReference = m_sInsuranceRef

            uctListDocuments1.ClaimCnt = m_lClaimCnt

            'DC240603 -ISS4097 -new parameter
            uctListDocuments1.SourceId = MainModule.g_iSourceID

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Initialise Failed", MainModule.ACApp, ACClass, "Initialise", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description:
    '
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
                If uctListDocuments1 IsNot Nothing Then
                    uctListDocuments1.Dispose()
                End If

                uctListDocuments1 = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: LoadInterface
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function LoadInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = uctListDocuments1.LoadControl()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = uctListDocuments1.GetDocuments()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Pass auto archive flag to user control
            uctListDocuments1.AutoArchiveEnabled = m_bAutoArchiveEnabled

            ' RDC 22/09/2005 auto-archive option. Disable Archive button if option enabled
            cmdArchive.Enabled = Not (m_bAutoArchiveEnabled)
            cmdArchive.Visible = Not (m_bAutoArchiveEnabled)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "LoadInterface Failed", MainModule.ACApp, ACClass, "LoadInterface", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdArchive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdArchive.Click

        ' Click event of the Archive button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctListDocuments1.ArchiveClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can refresh the interface.
                uctListDocuments1.Refresh()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the Archive command button", MainModule.ACApp, ACClass, "cmdArchive_Click", Information.Err().Number, excep.Message, excep:=excep)

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
            m_lReturn = uctListDocuments1.CancelClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the Cancel command button", MainModule.ACApp, ACClass, "cmdCancel_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        ' Click event of the Delete button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctListDocuments1.DeleteClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can refresh the interface.
                uctListDocuments1.Refresh()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the Delete command button", MainModule.ACApp, ACClass, "cmdDelete_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeleteAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAll.Click
        'LOCAL VARIABLES AREA
        Dim lListCount As Integer
        '--------------------


        Dim lMsg As DialogResult = MessageBox.Show("Are you sure you want to delete all the spooled documents?", Application.ProductName, MessageBoxButtons.YesNo)

        If lMsg = System.Windows.Forms.DialogResult.Yes Then
            lListCount = uctListDocuments1.countDocuments
            For lLoopThrough As Integer = lListCount - 1 To 0 Step -1
                uctListDocuments1.setSelectedItem = lLoopThrough + 1
                cmdDelete_Click(cmdDelete, New EventArgs())
            Next
        End If

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        ' Click event of the Edit button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctListDocuments1.EditClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can refresh the interface.
                'uctListDocuments1.Refresh()
                Me.TopMost = True
            End If
            Me.Focus()
        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the Edit command button", MainModule.ACApp, ACClass, "cmdEdit_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        '        m_lReturn& = ShowHelp(dlgHelp, ScreenHelpID)
        ' Click event of the Help button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctListDocuments1.ShowHelpScreen()

            ' Check the return value.

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the Help command button", MainModule.ACApp, ACClass, "cmdHelp_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: cmdOK_Click
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = uctListDocuments1.OKClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' Everything OK, so we can hide the interface.
                RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the OK command button", MainModule.ACApp, ACClass, "cmdOK_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrint.Click

        ' Click event of the Print button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctListDocuments1.PrintClick()
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'PN67015
                uctListDocuments1.ComesViaPrintEvent = True
                ' only attempt to archive if printing was successful
                If m_bAutoArchiveEnabled Then
                    cmdArchive_Click(cmdArchive, New EventArgs())
                End If
                uctListDocuments1.ComesViaPrintEvent = False

            End If

            ' refresh document list after print and archive
            ' otherwise it clears the selection
            uctListDocuments1.Refresh()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the Print command button", MainModule.ACApp, ACClass, "cmdPrint_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click

        ' Click event of the Refresh button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctListDocuments1.Refresh()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the Refresh command button", MainModule.ACApp, ACClass, "cmdRefresh_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try
            iPMFunc.ShowFormInTaskBar_Attach()

        Catch excep As System.Exception



            ' Error Section
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to initialise the interface object", MainModule.ACApp, ACClass, "Form_Initialise", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load


        '
        '
        '        ' Forms load event.
        '
        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            With uctListDocuments1
                .Task = m_iTask
                'developer guide no.24
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
                .PartyCnt = m_lPartyCnt
                .ShortName = m_sShortName
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .InsuranceFileCnt = m_lInsuranceFileCnt
                .InsReference = m_sInsuranceRef
                .ClaimCnt = m_lClaimCnt
                'DC240603 -ISS4097 -added new parameter
                .SourceId = MainModule.g_iSourceID

                m_lReturn = .Initialise()

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    Exit Sub
                End If

                '        m_lReturn& = .LoadControl
                '
                '        m_lReturn& = .GetDocuments
                '
                '        lStatus = .Status

            End With

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to load the form", MainModule.ACApp, ACClass, "Form_Load", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = uctListDocuments1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

            End If

            ' Terminate the control
            uctListDocuments1.Dispose()

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to terminate the interface", MainModule.ACApp, ACClass, "Form_QueryUnload", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            If VB6.PixelsToTwipsY(Me.Height) > 1000 And VB6.PixelsToTwipsX(Me.Width) > 100 Then
                uctListDocuments1.Height = Me.Height - VB6.TwipsToPixelsY(1200)
                uctListDocuments1.Width = Me.Width - VB6.TwipsToPixelsX(400)
            End If
        End If

        'If (Me.WindowState <> vbMinimized) Then
        '  Me.Height = 6430
        '  Me.Width = 10200
        ' End If

    End Sub

    Private Sub uctListDocuments1_lvwSearchDetailsItemClick(ByVal Sender As Object, ByVal e As uctListDocuments.lvwSearchDetailsItemClickEventArgs) Handles uctListDocuments1.lvwSearchDetailsItemClick
        cmdEdit.Enabled = e.bIsEditable
    End Sub
End Class
