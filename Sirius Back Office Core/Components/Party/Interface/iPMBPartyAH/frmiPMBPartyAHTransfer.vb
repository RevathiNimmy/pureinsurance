Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
'developer guide no. 211
Public Class frmTransfer
    Inherits System.Windows.Forms.Form


    Private m_oBusiness As bSIRHandlerTransfer.Business

    Private m_lReturn As Integer

    Private m_sOldHandlerType As String = ""


    Private m_lOldHandlerCnt As Integer
    Private m_sOldHandlerRef As String = ""

    Private m_lNewHandlerCnt As Integer
    Private m_sNewHandlerRef As String = ""

    Private m_lOldExecutiveCnt As Integer
    Private m_sOldExecutiveRef As String = ""

    Private m_lNewExecutiveCnt As Integer
    Private m_sNewExecutiveRef As String = ""
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    'Private m_lClientExecutivesChanged As Long
    'Private m_lPolicyExecutivesChanged As Long
    'Private m_lPolicyHandlersChanged As Long


    Public Property OldHandlerCnt() As Integer
        Get
            Return m_lOldHandlerCnt
        End Get
        Set(ByVal Value As Integer)
            m_lOldHandlerCnt = Value
        End Set
    End Property
    Public Property OldHandlerType() As String
        Get
            Return m_sOldHandlerType
        End Get
        Set(ByVal Value As String)
            m_sOldHandlerType = Value
        End Set
    End Property
    Public WriteOnly Property OldHandlerRef() As String
        Set(ByVal Value As String)
            m_sOldHandlerRef = Value
        End Set
    End Property
    Public Property OldExecutiveCnt() As Integer
        Get
            Return m_lOldExecutiveCnt
        End Get
        Set(ByVal Value As Integer)
            m_lOldExecutiveCnt = Value
        End Set
    End Property


    Public WriteOnly Property OldExecutiveRef() As String
        Set(ByVal Value As String)
            m_sOldExecutiveRef = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property




    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer
        Dim result As Integer = 0
        Dim ACClass As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRHandlerTransfer.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse


                ' Display message.
                MessageBox.Show("Failed to create instance of " & Strings.Chr(13) & Strings.Chr(10) & "bSIRHandlerTransfer.Business" & Strings.Chr(13) & Strings.Chr(10) & "(" & ACApp & "." & ACClass & ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception


            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ShowForm (Standard Method)
    '
    ' Description: Show the form
    '
    ' ***************************************************************** '
    Public Function ShowForm(ByRef lDisplayState As Integer) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object

        Try
            Select Case m_sOldHandlerType
                Case "AH"
                    lblExecutive.Enabled = False
                    lblExecutive.Visible = False
                    pnlAccountExecutive.Visible = False
                    cmdLookupAccountExecutive.Enabled = False
                    cmdLookupAccountExecutive.Visible = False
                Case "CO"
                    lblHandler.Enabled = False
                    lblHandler.Visible = False
                    pnlAccountHandler.Visible = False
                    cmdLookUpAccountHandler.Enabled = False
                    cmdLookUpAccountHandler.Visible = False
                    OldExecutiveCnt = m_lOldHandlerCnt
                    OldExecutiveRef = m_sOldHandlerRef

                Case "HC"
                    OldExecutiveCnt = m_lOldHandlerCnt
                    OldExecutiveRef = m_sOldHandlerRef

            End Select

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show the the form, allow user input etc.
            VB6.ShowForm(Me, lDisplayState)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vSpecialParty As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vDateCancelled As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACClass, GIIPMProcessModeGeneric As Object
        Dim m_lErrorNumber As Integer

        'developer guide no. 108
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 108
            oFindParty = New iPMBFindParty.Interface_Renamed()

            'Set appropriate key if agent only


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lErrorNumber = oFindParty.SetKeys(vKeyArray)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = "iPMBPartyAH.Interface"

            'SD 31/07/2002
            m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=GIIPMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.NotEditable = 1

            m_lErrorNumber = oFindParty.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName


                vDateCancelled = oFindParty.DateCancelled


                If Not Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                End If
                vResolvedName = oFindParty.ResolvedName
            Else
                If oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    result = gPMConstants.PMEReturnCode.PMCancel
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            oFindParty.Dispose()

            oFindParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Me.Hide()

    End Sub

    Private Sub cmdLookupAccountExecutive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLookupAccountExecutive.Click
        Dim ACClass As Object
        Dim vCnt As String = ""
        Dim vShortName As String = ""
        Dim vName, vResolvedName As String

        Try

            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="CO", vResolvedName:=vResolvedName)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    vCnt = ""
                    vShortName = ""
                    vName = ""
                    Exit Sub
                Else
                    Exit Sub
                End If

            End If

            m_lNewExecutiveCnt = vCnt

            m_sNewExecutiveRef = vShortName

            pnlAccountExecutive.Text = m_sNewExecutiveRef

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLookupAccountExecutive_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub cmdLookUpAccountHandler_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLookUpAccountHandler.Click
        Dim ACClass As Object
        Dim vCnt As String = ""
        Dim vShortName As String = ""
        Dim vName, vResolvedName As String

        Try

            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="AH", vResolvedName:=vResolvedName)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    vCnt = ""
                    vShortName = ""
                    vName = ""
                    Exit Sub
                Else
                    Exit Sub
                End If

            End If

            m_lNewHandlerCnt = CInt(vCnt)

            m_sNewHandlerRef = vShortName

            pnlAccountHandler.Text = m_sNewHandlerRef

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLookupAccountHandler_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub



    'Private Sub TabStrip1_Click()
    '
    'End Sub


    'Private Sub Text1_Change()
    '
    'End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim ACClass As Object

        Dim sHandlerType As String = ""

        Select Case m_sOldHandlerType
            Case "AH"
                sHandlerType = "Account Handler"
            Case "CO"
                sHandlerType = "Account Executive"
            Case "HC"
                sHandlerType = "Executive Handler"
        End Select

        If m_sOldHandlerType <> "CO" Then
            If m_lNewHandlerCnt = m_lOldHandlerCnt Then
                MessageBox.Show("Please Enter a different Account Handler", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            If m_lNewHandlerCnt = 0 Then
                MessageBox.Show("Please Enter a Valid Account Handler", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If
        If m_sOldHandlerType <> "AH" Then
            If m_lNewExecutiveCnt = m_lOldExecutiveCnt Then
                MessageBox.Show("Please Enter a different Account Executive", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            If m_lNewExecutiveCnt = 0 Then
                MessageBox.Show("Please Enter a Valid Account Executive", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If

        If MessageBox.Show("Are you sure you want to transfer data associated with " & sHandlerType.Trim() & " " & m_sOldHandlerRef.Trim() & " ?", "Transfer " & sHandlerType.Trim(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then


            m_oBusiness.OldHandlerCnt = m_lOldHandlerCnt

            m_oBusiness.NewHandlerCnt = m_lNewHandlerCnt

            m_oBusiness.OldHandlerRef = m_sOldHandlerRef

            m_oBusiness.NewHandlerRef = m_sNewHandlerRef

            m_oBusiness.OldExecutiveCnt = m_lOldExecutiveCnt

            m_oBusiness.NewExecutiveCnt = m_lNewExecutiveCnt

            m_oBusiness.OldExecutiveRef = m_sOldExecutiveRef

            m_oBusiness.NewExecutiveRef = m_sNewExecutiveRef

            m_oBusiness.UniqueId = m_sUniqueId
            m_oBusiness.ScreenHierarchy = m_sScreenHierarchy


            m_lReturn = m_oBusiness.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete " & sHandlerType, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
            End If

            MessageBox.Show("Sucessfully Updated ", "Transfer Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.Hide()

        End If

    End Sub

    Private Sub frmTransfer_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Terminate business

		m_oBusiness.Dispose()
        m_oBusiness = Nothing
        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub frmTransfer_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tab.SelectedIndex = 0
        End If
    End Sub
End Class
