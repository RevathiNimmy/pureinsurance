Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports uctListEventsControl
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmInterface"

    Private m_lPartyCnt As Integer
    Private m_lClaimCnt As Integer
    Private m_lBaseClaimId As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_sClaimRef As String = ""

    Private m_sTransactionType As String = ""
    Private m_sCallingAppName As String = ""
    Private m_iTask As Integer
    Private m_lStatus As Integer
    Private m_dtEffectiveDate As Date
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_lAccountKey As Integer
    Private m_sEventGroupCode As String = ""
    Private m_bDisableEventGroupLookup As Boolean

    Private m_bEnableDefaultedFields As Boolean ' S4B Claim Enhancements R&D 2005
    Private m_bShowNonNotes As Boolean
    Private m_bShowNotes As Boolean
    Private m_bRTFNotes As Boolean
    Private m_lCaseID As Integer
    Private m_sCaseNumber As String = ""
    Private m_lBaseCaseID As Integer
    Private m_bShowAllClaimVersionEvents As Boolean

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
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property
    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property
    Public WriteOnly Property BaseClaimId() As Integer
        Set(ByVal Value As Integer)
            m_lBaseClaimId = Value
        End Set
    End Property
    Public WriteOnly Property ClaimCnt() As Integer
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public WriteOnly Property AccountKey() As Integer
        Set(ByVal Value As Integer)
            m_lAccountKey = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceRef() As String
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property
    Public WriteOnly Property ClaimRef() As String
        Set(ByVal Value As String)
            m_sClaimRef = Value
        End Set
    End Property
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property EventGroupCode() As String
        Set(ByVal Value As String)
            m_sEventGroupCode = Value
        End Set
    End Property
    Public WriteOnly Property DisableEventGroupLookup() As Boolean
        Set(ByVal Value As Boolean)
            m_bDisableEventGroupLookup = Value
        End Set
    End Property

    Public Property EnableDefaultedFields() As Boolean
        Get
            Return m_bEnableDefaultedFields
        End Get
        Set(ByVal Value As Boolean)
            m_bEnableDefaultedFields = Value
        End Set
    End Property

    Public Property ShowNonNotes() As Boolean
        Get
            Return m_bShowNonNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bShowNonNotes = Value
        End Set
    End Property

    Public Property ShowNotes() As Boolean
        Get
            Return m_bShowNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bShowNotes = Value
        End Set
    End Property

    Public Property RTFNotes() As Boolean
        Get
            Return m_bRTFNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bRTFNotes = Value
        End Set
    End Property


    Public Property CaseID() As Integer
        Get
            Return m_lCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lCaseID = Value
        End Set
    End Property

    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property

    Public Property BaseCaseID() As Integer
        Get
            Return m_lBaseCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lBaseCaseID = Value
        End Set
    End Property
    Public Property ShowAllClaimVersionEvents() As Boolean
        Get
            Return m_bShowAllClaimVersionEvents
        End Get
        Set(ByVal Value As Boolean)
            m_bShowAllClaimVersionEvents = Value
        End Set
    End Property

    Private Sub CmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdCancel.Click
        Me.Close()
    End Sub

    ' ***************************************************************** '
    ' Name: CallNotes
    '
    ' Description:  TF031298
    '               Call FreeformText component, passing Party details
    '               MS220601
    '               Added Note Date for highlighting and party cnt
    ' ***************************************************************** '
    Public Function CallNotes(ByRef v_sEntityType As String, ByRef v_lEntityCnt As Integer, ByRef v_sTextType As String, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_sNoteDate As Date = #12/30/1899#) As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing

        Dim oObjectManager As Object
        oObjectManager = New bObjectManager.ObjectManager

        Dim oFreeformText As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FreeformText object

            m_lReturn = oObjectManager.GetInstance(oObject:=oFreeformText, sClassName:="iPMBFreeFormText.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iSIRFreeformText.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Pass key data to FreeformText component
            With oFreeformText
                'ECK 06/05/99
                '        .EntityName = SIREntityNameParty
                '        .KeyFieldValue = frmPartyPC.PartyCnt
                '        .TextType = "Private"

                .EntityName = v_sEntityType

                .KeyFieldValue = v_lEntityCnt

                .TextType = v_sTextType

                ' MS220106

                .PartyCnt = v_lPartyCnt
                If Strings.Len(DateTimeHelper.ToString(v_sNoteDate)) > 0 Then
                    ' Pass Date for highlight

                    .CallingAppName = "EventLog"

                    .NoteDate = v_sNoteDate
                End If

            End With

            ' Call Start method on Interface class

            m_lReturn = oFreeformText.Start()
            Me.Activate()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'iSIRFreeformText.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                oFreeformText.Dispose()
                oFreeformText = Nothing
                Return result
            End If

            ' Destroy FreeformText object

            oFreeformText.Dispose()
            oFreeformText = Nothing

            ' MSS06062001. No return state coming back from function
            ' Added one for Event notes so we know whether to refresh the grid or not.
            ' It's pointless and annoying to refresh if we cancel.
            If v_sEntityType = "Event" Then
                result = m_lReturn
            End If

            '    If m_vfrmListEvents.Visible = True Then
            '        frmListEvents.RefreshList
            '    End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Freeform Text.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (oFreeformText Is Nothing) Then

                oFreeformText.Dispose()
                oFreeformText = Nothing
            End If

            Return result

        End Try
    End Function


    Private Sub cmdNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNotes.Click
        Dim ACApp As Object = Nothing

        Try

            If uctListEvents1.EventCnt > 0 Then
                m_lReturn = CType(CallNotes("Event", uctListEvents1.EventCnt, "Public"), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = uctListEvents1.GetBusiness()
                End If

            End If

            Me.Enabled = True
            cmdNotes.Focus()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the notes command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNotes_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Me.Enabled = True
            cmdNotes.Focus()

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub
    Private Sub cmdNotesAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNotesAdd.Click
        uctListEvents1.TransactionType = m_sTransactionType
        m_lReturn = uctListEvents1.AddClick()
    End Sub
    Private Sub cmdNotesView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNotesView.Click
        uctListEvents1.TransactionType = m_sTransactionType
        m_lReturn = uctListEvents1.ViewClick()
    End Sub

    Private Sub cmdPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrint.Click
        uctListEvents1.PrintNotes()
    End Sub

    Private Sub cmdPrintAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrintAll.Click
        uctListEvents1.PrintAllNotes()
    End Sub

    Private Sub Form_Initialize_Renamed()
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
    End Sub

    ' ***************************************************************** '
    '
    ' Name: Form_Load
    '
    ' Description:
    '
    ' History: 01/10/2002 SJ - Created.
    '
    ' ***************************************************************** '

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim ACApp As Object = Nothing

        Try

            iPMFunc.CenterForm(Me)

            If m_bRTFNotes Then
                cmdNotes.Visible = True
                'cmdPrint.Left = cmdNotes.Left
                cmdPrintAll.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(cmdPrint.Left) + VB6.PixelsToTwipsX(cmdPrint.Width)) + 120)
            End If
            'developer guide no.9
            m_lReturn = uctListEvents1.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="uctListEvents1.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            uctListEvents1.InsuranceFolderCnt = m_lInsuranceFolderCnt
            uctListEvents1.ClaimCnt = m_lClaimCnt
            uctListEvents1.ClaimDesc = m_sClaimRef
            uctListEvents1.PartyCnt = m_lPartyCnt
            uctListEvents1.InsuranceFileCnt = m_lInsuranceFileCnt
            uctListEvents1.PolicyDesc = m_sInsuranceRef
            uctListEvents1.AccountKey = m_lAccountKey
            uctListEvents1.EventGroupCode = m_sEventGroupCode
            uctListEvents1.DisableEventGroupLookup = m_bDisableEventGroupLookup
            uctListEvents1.TransactionType = m_sTransactionType
            uctListEvents1.EnableDefaultedFields = m_bEnableDefaultedFields
            uctListEvents1.BaseClaimId = m_lBaseClaimId
            uctListEvents1.CaseID = m_lCaseID
            uctListEvents1.CaseNumber = m_sCaseNumber
            uctListEvents1.RTFNotes = m_bRTFNotes
            uctListEvents1.ShowNotes = m_bShowNotes
            uctListEvents1.ShowNonNotes = m_bShowNonNotes
            uctListEvents1.BaseCaseID = m_lBaseCaseID
            uctListEvents1.PassBaseClaimID = m_bShowAllClaimVersionEvents
            If m_bShowNotes And Not m_bShowNonNotes Then
                uctListEvents1.EventGroupCode = "N_CASES"
            End If

            m_lReturn = uctListEvents1.LoadControl()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="uctListEvents1.LoadControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            m_lReturn = uctListEvents1.GetEvents()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="uctListEvents1.GetEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            If m_bDisableEventGroupLookup Then
                cmdNotesAdd.Enabled = False
                cmdNotesView.Enabled = False
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            Exit Sub




        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        m_lReturn = uctListEvents1.UnLoadControl(Cancel, UnloadMode)
        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub uctListEvents1_lvwSearchDetailsClick(ByVal Sender As Object, ByVal e As uctListEvents.lvwSearchDetailsClickEventArgs) Handles uctListEvents1.lvwSearchDetailsClick
        'developer guide no. Refernce needed
        Select Case e.sEventType
            Case gSIRLibrary.ACNotesCustomer, gSIRLibrary.ACNotesAccount, gSIRLibrary.ACNotesClaims, gSIRLibrary.ACNotesPolicy, gSIRLibrary.ACNotesFSA, gSIRLibrary.ACNotesWarning, gSIRLibrary.ACEventFSAProductDisclosure, gSIRLibrary.ACNotesFSAGIIDisclosure, gSIRLibrary.ACNotesFSAGIIDN, gSIRLibrary.ACNotesCase
                cmdNotesView.Enabled = True
            Case Else
                cmdNotesView.Enabled = False
        End Select

    End Sub


    Private Sub uctListEvents1_lvwSearchDetailsDblClick(ByVal Sender As Object, ByVal e As uctListEvents.lvwSearchDetailsDblClickEventArgs) Handles uctListEvents1.lvwSearchDetailsDblClick

        Dim sCommand As String = ""
        'developer guide no. todo list (added as per the vb code)
        Dim sEventType As String = String.Empty
        Dim lDocumentCnt As Long

        Try

            Select Case sEventType
                Case gSIRLibrary.ACNotesCustomer, gSIRLibrary.ACNotesAccount, gSIRLibrary.ACNotesClaims, gSIRLibrary.ACNotesPolicy, gSIRLibrary.ACNotesFSA, gSIRLibrary.ACNotesWarning, gSIRLibrary.ACEventFSAProductDisclosure, gSIRLibrary.ACNotesFSAGIIDisclosure, gSIRLibrary.ACNotesFSAGIIDN, gSIRLibrary.ACNotesCase
                    m_lReturn = uctListEvents1.ViewClick()
                Case "DOCUMENT"
                    sCommand = ""
                    If m_sClaimRef <> "" Then
                        sCommand = "SBO" & m_sClaimRef & "2"
                    ElseIf m_sInsuranceRef <> "" Then
                        sCommand = "SBO" & m_sInsuranceRef & "2"
                    End If
                    If sCommand <> "" Then
                        m_lReturn = CType(ShowDocumasterDocument(lDocumentCnt, sCommand), gPMConstants.PMEReturnCode)
                    End If
                Case Else

            End Select

        Catch
        End Try



        Exit Sub
    End Sub

    ' ***************************************************************** '
    '
    ' Name: ShowDocumasterDocumentDocument
    '
    ' Description: Receives document number, creates an instance of
    '              documaster and then shows document level in tree and
    '              then opens the document to view
    '
    ' ***************************************************************** '
    Function ShowDocumasterDocument(ByRef v_lDocNum As Integer, ByRef sCommand As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing

        Dim iDocManager As Object
        Dim lWinHand As Integer
        Dim sLinkCodeAndLevel As String = ""
        Dim vTaskInstKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' check link code is not zero
            If v_lDocNum = 0 Then
                ' error
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Link Code incorrectly set to blank", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create Navigator keys to pass to Documaster
            ReDim vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0)

            vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameTaskDescription

            vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lDocNum


            ' See if Documaster is already running
            'SP040898 - see above
            lWinHand = iPMFunc.FindWindow(0, "DocuMaster Enterprise  ")

            If lWinHand <> 0 Then

                'DocuMaster is already running
                iDocManager = CreateLateBoundObject("iDOCManager.Interface_Renamed")  ' System.Runtime.InteropServices.Marshal.GetActiveObject("iDOCManager.Interface")

                'Set nav keys so documaster will load document
                m_lReturn = CType(iDocManager.SetKeys(vTaskInstKeyArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'error
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Set Navigator Keys", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iDocManager.Dispose()
                    iDocManager = Nothing
                    Return result

                End If

                'Show the interface
                'eck150201 pass new parameter

                m_lReturn = CType(iDocManager.Activate(sCommand), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'error
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Active DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iDocManager.Dispose()
                    iDocManager = Nothing
                    Return result

                End If

            Else

                'DocuMaster is not already running
                iDocManager = CreateLateBoundObject("iDOCManager.Interface_Renamed")

                'initialise the main interface
                m_lReturn = CType(CType(iDocManager, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)


                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMTrue

                    Case gPMConstants.PMEReturnCode.PMCancel
                        iDocManager.Dispose()
                        iDocManager = Nothing
                        Return result
                    Case Else
                        'error
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Initilise DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        result = gPMConstants.PMEReturnCode.PMFalse
                        iDocManager.Dispose()
                        iDocManager = Nothing
                        Return result
                End Select

                'Set nav keys so documaster will load document
                m_lReturn = CType(iDocManager.SetKeys(vTaskInstKeyArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'error
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Set Navigator Keys", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iDocManager.Dispose()
                    iDocManager = Nothing
                    Return result

                End If

                'Start the interface
                m_lReturn = CType(iDocManager.Start(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'error
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Start DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iDocManager.Dispose()
                    iDocManager = Nothing
                    Return result
                End If

            End If

            'Finished now
            iDocManager.Dispose()
            iDocManager = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowDocumasterDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctListEvents1.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
    End Sub
End Class
