Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctDocumentLink_NET.uctDocumentLink")> _
Partial Public Class uctDocumentLink
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event ProcessModeChange()
    Public Event StatusChange()
    Public Event TaskChange()
    Public Event CallingAppNameChange()
    ' ***************************************************************** '
    '
    ' Date: 26/02/2008
    '
    ' Description: Document Link
    '
    ' ***************************************************************** '
    'Developer Guide No. 69
    Public frmDocumentLinkEdit As frmDocumentLinkEdit
    Public frmCopyDocumentLink As frmCopyDocumentLink

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctPMUDocumentLink"

    Private m_iSourceID As Integer
    Private m_iUserId As Integer
    Private m_iCurrencyID As Integer
    Private m_sUserName As String = ""

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
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_bWrittenPolicyStatus As Boolean
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_iFunctionalArea As Integer
    Private m_lProductID As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupProcess As Object
    Private m_vLookupBranch As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""


    Private m_vDocumentLinksData(,) As Object



    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

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
    <Browsable(True)> _
    Public Property WrittenPolicyStatus() As Boolean
        Get
            Return m_bWrittenPolicyStatus

        End Get
        Set(ByVal Value As Boolean)

            m_bWrittenPolicyStatus = Value


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
    <Browsable(False)> _
    Public WriteOnly Property FuntionalArea() As Integer
        Set(ByVal Value As Integer)

            m_iFunctionalArea = Value

        End Set
    End Property
    <Browsable(False)> _
    Public WriteOnly Property ProductID() As Integer
        Set(ByVal Value As Integer)

            m_lProductID = Value

        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property TabCaption(ByVal iIndex As Integer) As String
        Set(ByVal Value As String)

            SSTabHelper.SetTabCaption(tabMainTab, iIndex, Value)

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
    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Pankaj : 26-02-2008 : Renewal Printing
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"
        Dim vResultArray As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Static bIsInitialised As Boolean
            ''Developer Guide No. 38
            ' Me.cboFilterByBranch.FirstItem = "(All Branches)"
            'If m_lProductID = 0 Then

            '    tabMainTab.Enabled = False

            '    GoTo Finally_Renamed
            'End If

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

            tabMainTab.Enabled = True

            m_lReturn = SetInputControls()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
                m_iUserId = .UserID
                m_iCurrencyID = .CurrencyID
                m_sUserName = .UserName
            End With


            ' Get an instance of the contact interface object via
            ' the public object manager.

            g_oBusiness = New bPMBDocLink.Business()
            'Developer Guide No. 9
            m_lReturn = g_oBusiness.Initialise(sUsername:=m_sUserName, sPassword:=g_oObjectManager.Password, iUserID:=m_iUserId, iSourceID:=m_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=m_sCallingAppName)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Doc Link object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initaise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            m_lReturn = GetProcessType()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = GetBusiness()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' hold Initialised status
            bIsInitialised = True
            'Developer Guide No. 38
            Me.cboFilterByBranch.FirstItem = "(All Branches)"

            If m_lProductID = 0 Then

                tabMainTab.Enabled = False

                Return result
            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
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
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer


        Dim result As Integer = 0
        Dim lBranchId As Integer
        Const kMethodName As String = "GetBusiness"



        'set zero for all document templates
        Dim lDocumentTemplateID As Integer = 0
        Dim iProcessTypeID As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        If cboFilterByProcess.SelectedIndex <> -1 Then
            iProcessTypeID = VB6.GetItemData(cboFilterByProcess, cboFilterByProcess.SelectedIndex)
        End If

        If cboFilterByBranch.ListIndex >= 1 Then
            lBranchId = cboFilterByBranch.ItemId
        End If

        m_lReturn = g_oBusiness.GetSFIDocLink(v_iFunctionalArea:=m_iFunctionalArea, v_lProductID:=m_lProductID, v_iProcessTypeID:=iProcessTypeID, v_lSourceID:=lBranchId, v_lDocumentTemplateID:=lDocumentTemplateID, r_vResultarray:=m_vDocumentLinksData)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataToInterface(Optional ByRef iTask As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Const kMethodName As String = "DataToInterface"



        result = gPMConstants.PMEReturnCode.PMTrue

        'The task here checks for updating the Grid from scrach or adding the records
        'from frmCopyDocumentLink. If the records are being added from frmCopydoumentLinks
        'then set the iTask = PMAdd
        If iTask <> gPMConstants.PMEComponentAction.PMAdd Then
            ' Clear the search details.
            lvwSearchDetails.Items.Clear()
        End If

        ' Assign the details to the interface.

        If Not Information.IsArray(m_vDocumentLinksData) Then
            GoTo Finally_Renamed
        End If

        For lRow As Integer = m_vDocumentLinksData.GetLowerBound(1) To m_vDocumentLinksData.GetUpperBound(1)

            oListItem = lvwSearchDetails.Items.Add(CStr(m_vDocumentLinksData(ACPMBProductionOrder, lRow)))
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vDocumentLinksData(ACPMBProcessTypeCode, lRow)).Trim()
            If gPMFunctions.ToSafeString(CStr(m_vDocumentLinksData(ACPMBSourceDescription, lRow)).Trim(), "") = "" Then
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "(All Branches)"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vDocumentLinksData(ACPMBSourceDescription, lRow)).Trim()
            End If
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vDocumentLinksData(ACPMBProcessTypeDocDescription, lRow)).Trim()
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vDocumentLinksData(ACPMBDocumentTypeTemplateDescription, lRow))
            If gPMFunctions.ToSafeInteger(CStr(m_vDocumentLinksData(ACPMBIsClient, lRow))) > 0 Then
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = "X"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = ""
            End If
            If gPMFunctions.ToSafeInteger(CStr(m_vDocumentLinksData(ACPMBIsAgent, lRow))) > 0 Then
                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = "X"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = ""
            End If
            If gPMFunctions.ToSafeInteger(CStr(m_vDocumentLinksData(ACPMBIsOffice, lRow))) > 0 Then
                ListViewHelper.GetListViewSubItem(oListItem, 7).Text = "X"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 7).Text = ""
            End If
            '(Start)-(Arul Stephen)-(Document Configuration)
            If gPMFunctions.ToSafeInteger(CStr(m_vDocumentLinksData(ACPMBSpoolDocument, lRow))) = 2 Then
                ListViewHelper.GetListViewSubItem(oListItem, 8).Text = "User Choice"
            ElseIf gPMFunctions.ToSafeInteger(CStr(m_vDocumentLinksData(ACPMBSpoolDocument, lRow))) = 1 Then
                ListViewHelper.GetListViewSubItem(oListItem, 8).Text = "Spooler"
            ElseIf gPMFunctions.ToSafeInteger(CStr(m_vDocumentLinksData(ACPMBSpoolDocument, lRow))) = 0 Then
                ListViewHelper.GetListViewSubItem(oListItem, 8).Text = "Printer"
            End If
            '(End)-(Arul Stephen)-(Document Configuration)
            ListViewHelper.GetListViewSubItem(oListItem, 9).Text = CStr(gPMFunctions.ToSafeLong(CStr(m_vDocumentLinksData(ACPMBProcessTypeId, lRow))))
            ListViewHelper.GetListViewSubItem(oListItem, 10).Text = CStr(gPMFunctions.ToSafeLong(CStr(m_vDocumentLinksData(ACPMBSourceId, lRow))))
            ListViewHelper.GetListViewSubItem(oListItem, 11).Text = CStr(gPMFunctions.ToSafeLong(CStr(m_vDocumentLinksData(ACPMBProcessTypesDocsId, lRow))))
            ListViewHelper.GetListViewSubItem(oListItem, 12).Text = CStr(gPMFunctions.ToSafeLong(CStr(m_vDocumentLinksData(ACPMBDocumentTypeId, lRow))))
            ListViewHelper.GetListViewSubItem(oListItem, 13).Text = CStr(gPMFunctions.ToSafeLong(CStr(m_vDocumentLinksData(ACPMBDocumentTemplateId, lRow))))

            If iTask = gPMConstants.PMEComponentAction.PMAdd Then
                ListViewHelper.GetListViewSubItem(oListItem, 14).Text = CStr(0)
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 14).Text = CStr(gPMFunctions.ToSafeLong(CStr(m_vDocumentLinksData(ACPMBDocLinkId, lRow))))
            End If

            If gPMFunctions.ToSafeInteger(CStr(m_vDocumentLinksData(ACPMBDocumentTypeTemplateBO, lRow))) > 0 Then
                ListViewHelper.GetListViewSubItem(oListItem, 15).Text = "X"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 15).Text = ""
            End If

            If gPMFunctions.ToSafeInteger(CStr(m_vDocumentLinksData(ACPMBDocumentTypeTemplateSAM, lRow))) > 0 Then
                ListViewHelper.GetListViewSubItem(oListItem, 16).Text = "X"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 16).Text = ""
            End If

        Next lRow
        If lvwSearchDetails.Items.Count > 0 Then
            lvwSearchDetails.Items.Item(0).Selected = True
        End If


        GoTo Finally_Renamed



        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result

        Resume

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayCaptions) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisplayCaptions() As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "DisplayCaptions"
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}


    'lvwSearchDetails.Columns.Item(ACColHeadEventDate - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '


    'lvwSearchDetails.Columns.Item(ACColHeadPolicy - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'GoTo Finally_Renamed
    '
    '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    'Finally_Renamed: '
    '
    'Return result
    '
    'Resume 
    'Return result
    'End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0



        Const kMethodName As String = "ProcessCommand"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check if form has been cancelled, if so, prompt
        ' if you wish to lose details.

        Select Case m_iTask
            Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                m_lReturn = AddNewDocumentLinks()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If

            Case gPMConstants.PMEComponentAction.PMDelete
                m_lReturn = DeleteDocumentLinks()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If

            Case gPMConstants.PMEComponentAction.PMCopy
                m_lReturn = CopyDocumentLinks()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If
        End Select

        lvwSearchDetails.Focus()

        GoTo Finally_Renamed



        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result

        Resume
        Return result
    End Function

    Private Sub cboFilterByBranch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFilterByBranch.Click
        m_lReturn = GetBusiness()
    End Sub

    Private Sub cboFilterByProcess_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFilterByProcess.SelectionChangeCommitted
        m_lReturn = GetBusiness()
    End Sub
    'Developer Guide No. 78
    Private Sub cboFilterByProcess_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboFilterByProcess.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        KeyAscii = 0
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
        m_iTask = gPMConstants.PMEComponentAction.PMAdd
        m_lReturn = ProcessCommand()
    End Sub

    Private Sub cmdCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCopy.Click
        m_iTask = gPMConstants.PMEComponentAction.PMCopy
        m_lReturn = ProcessCommand()
    End Sub
    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        m_iTask = gPMConstants.PMEComponentAction.PMEdit
        m_lReturn = ProcessCommand()
    End Sub

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click
        If lvwSearchDetails.Items.Count > 0 Then
            m_iTask = gPMConstants.PMEComponentAction.PMDelete
            m_lReturn = ProcessCommand()
        End If
    End Sub

    Private Sub cmdUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUp.Click
        If lvwSearchDetails.Items.Count > 0 Then
            If Conversion.Val(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Text) > 1 Then
                lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Text = CStr(Conversion.Val(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Text) - 1)
            End If
        End If
    End Sub
    Private Sub cmdDown_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDown.Click
        If lvwSearchDetails.Items.Count > 0 Then
            If Conversion.Val(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Text) < 255 Then
                lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Text = CStr(Conversion.Val(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Text) + 1)
            End If
        End If
    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick
        cmdEdit_Click(cmdEdit, New EventArgs())
    End Sub

    Private Sub uctDocumentLink_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Const kMethodName As String = "UserControl_Resize"



        tabMainTab.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height)
        lvwSearchDetails.SetBounds(tabMainTab.Left + 100 / 15, tabMainTab.Top + 900 / 15, ClientRectangle.Width - 1300 / 15, ClientRectangle.Height - 1400 / 15)

        cmdAdd.Left = lvwSearchDetails.Left + lvwSearchDetails.Width + 5
        cmdAdd.Top = lvwSearchDetails.Top - 5

        cmdEdit.Left = cmdAdd.Left
        cmdEdit.Top = cmdAdd.Top + 25

        cmdCopy.Left = cmdAdd.Left
        cmdCopy.Top = cmdEdit.Top + 25

        cmdRemove.Left = cmdAdd.Left
        cmdRemove.Top = cmdCopy.Top + 25

        cmdDown.Left = cmdAdd.Left
        cmdDown.Top = lvwSearchDetails.Bottom - cmdDown.Height

        cmdUp.Left = cmdAdd.Left
        cmdUp.Top = cmdDown.Top - (cmdUp.Height + 5)

        GoTo Finally_Renamed



        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn)

Finally_Renamed:

        Exit Sub

        Resume
    End Sub

    Private Function AddNewDocumentLinks() As Integer

        Dim result As Integer = 0
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim iProcessTypeID As Integer

        Const kMethodName As String = "AddNewDocumentLinks"

        frmDocumentLinkEdit = New frmDocumentLinkEdit

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            If lvwSearchDetails.Items.Count = 0 Then
                GoTo Finally_Renamed
            End If
        End If

        If cboFilterByProcess.SelectedIndex <> -1 Then
            iProcessTypeID = VB6.GetItemData(cboFilterByProcess, cboFilterByProcess.SelectedIndex)
        End If

        With frmDocumentLinkEdit
            .ProductID = m_lProductID
            .ProcessID = iProcessTypeID
            .SourceID = cboFilterByBranch.ItemId

            'Developer Guide No. 24
            .lvwSearchDetails = lvwSearchDetails
            .Task = m_iTask
            .FunctionalArea = m_iFunctionalArea
            'Start- Written Status
            .WrittenPolicyStatus = m_bWrittenPolicyStatus
            'Start-  Written Status
            .ShowDialog()
            lStatus = .Status
        End With

        If lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
            cmdCopy.Enabled = False
            cboFilterByProcess.Enabled = False
            cboFilterByBranch.Enabled = False
        End If

        GoTo Finally_Renamed



        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result

        Resume

        Return result
    End Function

    Private Function DeleteDocumentLinks() As Integer
        Dim result As Integer = 0
        Dim lDocLinkId As Integer

        Const kMethodName As String = "DeleteDocumentLinks"



        result = gPMConstants.PMEReturnCode.PMTrue

        If lvwSearchDetails.Items.Count = 0 Then
            Return result
        End If

        If lvwSearchDetails.FocusedItem Is Nothing Then
            Return result
        End If

        'Developer Guide No. 243
        Dim sMessage As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRemoveEntry, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        Dim iMsgResult As DialogResult = MessageBox.Show(sMessage, "Document Link", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        ' Check message result.
        If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
            Dim focusedIndex As Integer = lvwSearchDetails.FocusedItem.Index
            Dim sDescription As String = CStr(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(focusedIndex), 4).Text).Trim()
            lDocLinkId = CInt(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(focusedIndex), 14).Text)

            lvwSearchDetails.Items.RemoveAt(focusedIndex)

            If m_iFunctionalArea = 1 Then
                m_sScreenHierarchy = m_sScreenHierarchy & $"/DocumentLinks/Description({sDescription})"
            ElseIf m_iFunctionalArea = 2 Then
                m_sScreenHierarchy = m_sScreenHierarchy & $"/ClaimsDocuments(Open/MaintainClaims)/Description({sDescription})"
            Else
                m_sScreenHierarchy = m_sScreenHierarchy & $"/ClaimsDocuments(ClaimPayments)/Description({sDescription})"
            End If

            If lDocLinkId > 0 Then

                m_lReturn = g_oBusiness.DeleteDocLink(v_lDocLinkId:=lDocLinkId, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

        End If
        GoTo Finally_Renamed



        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result

        Resume

        Return result
    End Function

    Private Function CopyDocumentLinks() As Integer
        Dim result As Integer = 0
        Dim lDocumentTemplateID, lProductID As Integer
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim iProcessTypeID As Integer

        Const kMethodName As String = "CopyDocumentLinks"
        Try


            'Developer Guide No. 50
            frmCopyDocumentLink = New frmCopyDocumentLink
            result = gPMConstants.PMEReturnCode.PMTrue

            If cboFilterByProcess.SelectedIndex <> -1 Then
                iProcessTypeID = VB6.GetItemData(cboFilterByProcess, cboFilterByProcess.SelectedIndex)
            End If

            With frmCopyDocumentLink
                .FunctionalArea = m_iFunctionalArea
                .ProcessID = iProcessTypeID
                .SourceID = cboFilterByBranch.ItemId
                .ProductID = m_lProductID
                .ShowDialog()
                lProductID = .ProductID
                lDocumentTemplateID = .DocumentTemplateID
                lStatus = .Status
            End With

            If lStatus = gPMConstants.PMEReturnCode.PMCancel OrElse lStatus = 0 Then
                Return result
            End If

            If lProductID = 0 Then
                Return result
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = g_oBusiness.GetSFIDocLink(v_iFunctionalArea:=m_iFunctionalArea, v_lProductID:=lProductID, v_iProcessTypeID:=iProcessTypeID, v_lSourceID:=cboFilterByBranch.ItemId, v_lDocumentTemplateID:=lDocumentTemplateID, r_vResultarray:=m_vDocumentLinksData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(m_vDocumentLinksData) Then
                Return result
            End If

            m_lReturn = CType(DataToInterface(gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

            cmdCopy.Enabled = False
            cboFilterByProcess.Enabled = False
            cboFilterByBranch.Enabled = False

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)






        End Try
        Return result
    End Function

    Public Function AddDocLinks() As Integer

        Dim result As Integer = 0
        Dim lCol As Integer
        Dim lProcessTypesDocsId As Integer
        Dim iFunctionalArea As Integer
        Dim lSourceID As Integer
        Dim iIsClient, iIsAgent, iIsOffice, iProductionOrder, iSpoolDocument As Integer
        Dim lProcessID, lDocumentTypeID, lDocumentTemplateID, lGisSchemeId, lDocLinkId As Integer
        Dim iIsBO, iIsSAM As Integer
        Const kMethodName As String = "AddDocLinks"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            For lRow As Integer = 1 To lvwSearchDetails.Items.Count


                'PMB_Doc_Link Table Field
                'PMB_Doc_Link_Id
                lDocLinkId = CInt(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 14).Text)

                'GIS_Scheme_Id
                lGisSchemeId = 1

                'Process_Type_Id
                lProcessID = CInt(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 9).Text)

                'Document_Type_Id
                lDocumentTypeID = CInt(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 12).Text)

                'Document_Template_Id
                lDocumentTemplateID = CInt(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 13).Text)

                'spool_document
                '(Start)-(Arul Stephen)-(Document Configuration)
                If ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 8).Text.Trim().ToUpper() = ("Printer").ToUpper() Then
                    iSpoolDocument = 0
                ElseIf ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 8).Text.Trim().ToUpper() = ("Spooler").ToUpper() Then
                    iSpoolDocument = 1
                ElseIf ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 8).Text.Trim().ToUpper() = ("User Choice").ToUpper() Then
                    iSpoolDocument = 2
                End If
                '(End)-(Arul Stephen)-(Document Configuration)
                'process_types_docs_id
                lProcessTypesDocsId = CInt(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 11).Text)

                'functional_area
                'will be set by global variable

                'Producd id
                'will be set by global variable

                ' source_id
                lSourceID = CInt(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 10).Text)

                'is_client
                If ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 5).Text = "X" Then
                    iIsClient = 1
                Else
                    iIsClient = 0
                End If

                'is_agent
                If ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 6).Text = "X" Then
                    iIsAgent = 1
                Else
                    iIsAgent = 0
                End If

                'is_office
                If ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 7).Text = "X" Then
                    iIsOffice = 1
                Else
                    iIsOffice = 0
                End If

                If ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 15).Text = "X" Then
                    iIsBO = 1
                Else
                    iIsBO = 0
                End If

                If ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 16).Text = "X" Then
                    iIsSAM = 1
                Else
                    iIsSAM = 0
                End If

                'production_order
                iProductionOrder = gPMFunctions.ToSafeInteger(lvwSearchDetails.Items.Item(lRow - 1).Text)
                If m_sUniqueId IsNot Nothing Then
                    If m_iFunctionalArea = 1 Then
                        m_sScreenHierarchy = m_sScreenHierarchy & $"/DocumentLinks/Description({CStr(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 4).Text).Trim()})"
                    ElseIf m_iFunctionalArea = 2 Then
                        m_sScreenHierarchy = m_sScreenHierarchy & $"/ClaimsDocuments(Open/MaintainClaims)/Description({CStr(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 4).Text).Trim()})"
                    Else
                        m_sScreenHierarchy = m_sScreenHierarchy & $"/ClaimsDocuments(ClaimPayments)/Description({CStr(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 4).Text).Trim()})"
                    End If
                End If

                m_lReturn = g_oBusiness.AddDocLink(r_lDocLinkId:=lDocLinkId, v_lGisSchemeId:=lGisSchemeId, v_lProcessID:=lProcessID, v_lDocumentTypeID:=lDocumentTypeID, v_lDocumentTemplateID:=lDocumentTemplateID, v_iSpoolDocument:=iSpoolDocument, v_lProcessTypesDocsId:=lProcessTypesDocsId, v_iFunctionalArea:=m_iFunctionalArea, v_lProductID:=m_lProductID, v_lSourceID:=lSourceID, v_iIsClient:=iIsClient, v_iIsAgent:=iIsAgent, v_iIsOffice:=iIsOffice, v_iProductionOrder:=iProductionOrder, v_iIsBO:=iIsBO, v_iIsSAM:=iIsSAM, m_sUniqueId, m_sScreenHierarchy)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If

                ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), 14).Text = CStr(lDocLinkId)
            Next

            cmdCopy.Enabled = True
            cboFilterByProcess.Enabled = True
            cboFilterByBranch.Enabled = True

            'To filter the List as per the selected Branch and Process
            cboFilterByBranch_Click(cboFilterByBranch, Nothing)
            cboFilterByProcess_SelectionChangeCommitted(cboFilterByProcess, New EventArgs())


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '        Return result

            '        Resume


            '        Return result
        End Try
        Return result
    End Function

    Private Function SetInputControls() As Integer

        Dim result As Integer = 0
        Dim iIndex As Integer
        Dim sColumnHeading As String = ""

        Const kMethodName As String = "SetInputControls"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            lvwSearchDetails.Columns.Clear()

            lvwSearchDetails.BorderStyle = BorderStyle.Fixed3D
            lvwSearchDetails.FullRowSelect = True
            lvwSearchDetails.View = View.Details
            lvwSearchDetails.LabelEdit = False
            lvwSearchDetails.HideSelection = False


            '1.Col Order

            sColumnHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColOrder, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sColumnHeading, CInt(VB6.TwipsToPixelsX(700)), HorizontalAlignment.Left)

            '2.Col Process

            sColumnHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColProcess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sColumnHeading, CInt(VB6.TwipsToPixelsX(2200)), HorizontalAlignment.Left)

            '3.Col Branch

            sColumnHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sColumnHeading, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left)

            '4.Col Document Code

            sColumnHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColDocumentCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sColumnHeading, CInt(VB6.TwipsToPixelsX(1600)), HorizontalAlignment.Left)

            '5.Col Description

            sColumnHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sColumnHeading, CInt(VB6.TwipsToPixelsX(2600)), HorizontalAlignment.Left)

            '6.Col Cleint

            sColumnHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColClient, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sColumnHeading, CInt(VB6.TwipsToPixelsX(700)), HorizontalAlignment.Left)

            '7.Col Agent

            sColumnHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sColumnHeading, CInt(VB6.TwipsToPixelsX(700)), HorizontalAlignment.Left)

            '8.Col Office

            sColumnHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColOffice, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sColumnHeading, CInt(VB6.TwipsToPixelsX(700)), HorizontalAlignment.Left)

            '9.Col Send To

            sColumnHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColSendTo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sColumnHeading, CInt(VB6.TwipsToPixelsX(900)), HorizontalAlignment.Left)

            'Some Hidden fields to store the corresponding Id of col No.2-5
            lvwSearchDetails.Columns.Add("ProcessTypeDoc_Id", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left)
            lvwSearchDetails.Columns.Add("Branch_Id", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left)
            lvwSearchDetails.Columns.Add("Process_Doc_Type_Id", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left)
            lvwSearchDetails.Columns.Add("Document_Id", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left)
            lvwSearchDetails.Columns.Add("Document_Template_Id", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left)
            lvwSearchDetails.Columns.Add("PMDDoc_Link_Id", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left)
            lvwSearchDetails.Columns.Add("BO", CInt(VB6.TwipsToPixelsX(500)), HorizontalAlignment.Center)
            lvwSearchDetails.Columns.Add("SAM", CInt(VB6.TwipsToPixelsX(500)), HorizontalAlignment.Center)
            'Set Tab Indexes to controls
            iIndex = 1

            cboFilterByProcess.TabIndex = iIndex
            iIndex += 1
            cboFilterByBranch.TabIndex = iIndex
            iIndex += 1
            cmdAdd.TabIndex = iIndex
            iIndex += 1
            cmdCopy.TabIndex = iIndex
            iIndex += 1
            cmdRemove.TabIndex = iIndex
            iIndex += 1
            cmdUp.TabIndex = iIndex
            iIndex += 1
            cmdDown.TabIndex = iIndex



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally






        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (GetLookUpList) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookUpList(ByVal v_sTableName As String, ByRef r_cboControl As ComboBox) As Integer
    '
    'Dim result As Integer = 0
    'Dim vResultArray(,) As Object
    '
    'On Error GoTo Catch_Renamed
    '
    'Const kMethodName As String = "GetLookUpList"
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'add N/A to combo box
    'r_cboControl.Items.Clear()
    'Dim r_cboControl_NewIndex As Integer = -1
    'r_cboControl_NewIndex = r_cboControl.Items.Add("(All Processes)")
    'VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, 0)
    '
    'm_lReturn = g_oBusiness.GetLookUpList(v_sTableName:=v_sTableName, r_vResultArray:=vResultArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    '
    'If Information.IsArray(vResultArray) Then

    'For 'lCount As Integer = 0 To vResultArray.GetUpperBound(1)

    'r_cboControl_NewIndex = r_cboControl.Items.Add(CStr(vResultArray(1, lCount)))

    'VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, CInt(vResultArray(0, lCount)))
    'Next 
    'End If
    '
    'r_cboControl.SelectedIndex = 0
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    'Finally_Renamed: '
    '
    'Return result
    '
    'Resume 
    '
    'Return result
    'End Function

    Private Function GetProcessType() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Const kMethodName As String = "GetProcessType"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            'add N/A to combo box
            cboFilterByProcess.Items.Clear()
            Dim cboFilterByProcess_NewIndex As Integer = -1
            cboFilterByProcess_NewIndex = cboFilterByProcess.Items.Add("(All Processes)")
            VB6.SetItemData(cboFilterByProcess, cboFilterByProcess_NewIndex, 0)

            m_lReturn = g_oBusiness.GetProcessType(r_vProcessType:=vResultArray, v_iFunctionalArea:=m_iFunctionalArea)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            'cboFilterByProcess
            If Information.IsArray(vResultArray) Then

                For lCount As Integer = 0 To vResultArray.GetUpperBound(1)
                    cboFilterByProcess_NewIndex = -1


                    'start Written Status
                    If m_bWrittenPolicyStatus = True Then

                        cboFilterByProcess.Items.Add(New VB6.ListBoxItem(CStr(vResultArray(1, lCount)), CInt(vResultArray(0, lCount))))

                    Else
                        If (vResultArray(1, lCount)) <> ("New Business Written") Then
                            If (vResultArray(1, lCount)) <> ("Renewal Acceptance Written") Then
                                cboFilterByProcess.Items.Add(New VB6.ListBoxItem(CStr(vResultArray(1, lCount)), CInt(vResultArray(0, lCount))))
                            End If
                        End If
                    End If
                    'End  Written Status

                Next
            End If

            cboFilterByProcess.SelectedIndex = 0



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally






        End Try
        Return result
    End Function
End Class
