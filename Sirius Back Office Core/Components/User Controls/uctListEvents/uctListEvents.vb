Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctListEvents_NET.uctListEvents")> _
Partial Public Class uctListEvents
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event FSAComplaintFolderCntChange()
    Public Event ClaimDescChange()
    Public Event PolicyDescChange()
    Public Event EventTypeChange()
    Public Event NewAddressCntChange()
    Public Event OldAddressCntChange()
    Public Event DocumentCntChange()
    Public Event ClaimCntChange()
    Public Event OldPartyTypeIDChange()
    Public Event InsuranceFileStructureIdChange()
    Public Event InsuranceFileCntChange()
    Public Event InsuranceFolderCntChange()
    Public Event EventCntChange()
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event ProcessModeChange()
    Public Event StatusChange()
    Public Event TaskChange()
    Public Event CallingAppNameChange()
    ' ***************************************************************** '
    '
    ' Date: 07/10/1998
    '
    ' Description: List Policy User Control
    '
    ' Edit History: TO150799 - Based on Policy List Control
    ' RAM20040224 : Code changes related to PN Issue 10592
    '               1. cmdRefresh Button added
    '               2. Lost Focus Events Removed for txtFromDate, txtToDate
    '               3. Click Event Removed for cboPolicy, cboClaim, cboType, cboUser
    ' CJB20050816 : PN23203 Changed DataToInterface to show "" in status column for
    '               events that are not sticky notes
    ' ***************************************************************** '
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctListEventsControl"

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
    Event lvwSearchDetailsDblClick(ByVal Sender As Object, ByVal e As lvwSearchDetailsDblClickEventArgs)
    'sj 15/09/2003 - End

    'sj 01/10/2002 - start
    Event lvwSearchDetailsClick(ByVal Sender As Object, ByVal e As lvwSearchDetailsClickEventArgs)
    Event lvwSearchDetailsKeyUp(ByVal Sender As Object, ByVal e As lvwSearchDetailsKeyUpEventArgs)
    'sj 01/10/2002 - end

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
    Private m_lEventCnt As Integer
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_lInsuranceFolderCnt As Integer

    Private m_lInsuranceFileCnt As Integer
    Private m_sPolicyDesc As String = ""
    Private m_lInsuranceFileStructureId As Integer
    Private m_lClaimCnt As Integer
    Private m_lBaseClaimId As Integer
    Private m_sClaimDesc As String = ""
    Private m_lOldAddressCnt As Integer
    Private m_lNewAddressCnt As Integer
    Private m_lDocumentCnt As Integer
    Private m_sEventType As String = ""
    Private m_vNotesArray As Object
    'MS220601
    Private m_dtNoteDate As Date

    ' CTAF - 240800 - OldPartyTypeID
    Private m_lOldPartyTypeID As Integer

    ' MS190601
    Private m_sDocumentRef As String = ""

    ' TF311298 - changed from NavProcessCode
    Private m_sInsFileType As String = ""
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIREvent.Business
    'Private m_oBusiness As bSIREvent.Business

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lItemsFound As gPMConstants.PMEFormatStyle
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object

    'sj 27/09/2002 - start
    Private m_vEventTypeGroupArray(,) As Object
    Private m_bIsNRMA As Boolean
    'sj 27/09/2002 - end

    'sj 30/09/2002 - start
    Private m_oNotes As iPMBNote.Interface_Renamed
    Private m_sEventLogSubject As String = ""
    Private m_lEventTypeGroupId As Integer
    Private m_sEventTypeGroupDescription As String = ""
    Private m_lEventLogSubjectId As Integer
    Private m_lClassInsuranceFileCnt As Integer
    Private m_lClassInsuranceFolderCnt As Integer
    Private m_lClassClaimCnt As Integer
    Private m_lAccountKey As Integer
    Private m_sEventGroupCode As String = ""
    Private m_bDisableEventGroupLookup As Boolean
    'sj 30/09/2002 - end

    'sj 15/09/2003 - Start
    Private m_lFSAComplaintFolderCnt As Integer
    'sj 15/09/2003 - End
    'SJ 20/02/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    'SJ 20/02/2004 - end

    Private m_bSetup As Boolean
    Private m_lInsuranceFileCntSearch As Integer ' RAM20040224 : Ref PN Issue 10592
    Private m_lInsuranceFolderCntSearch As Integer ' RAM20040224 : Ref PN Issue 10592
    Private m_lClaimCntSearch As Integer ' RAM20040224 : Ref PN Issue 10592
    'As per VB Code
    'start
    'Private m_dtFromDate As Date ' RAM20040224 : Ref PN Issue 10592
    'Private m_dtToDate As Date ' RAM20040224 : Ref PN Issue 10592
    Private m_dtFromDate As Object ' RAM20040224 : Ref PN Issue 10592
    Private m_dtToDate As Object ' RAM20040224 : Ref PN Issue 10592
    'end
    Private m_bFromRefreshButton As Boolean ' RAM20040224 : Ref PN Issue 10592

    Private m_bEnableDefaultedFields As Boolean ' S4B Claim Enhancements R&D 2005
    Private m_bResizing As Boolean

    Private m_bShowNonNotes As Boolean
    Private m_bShowNotes As Boolean
    Private m_bRTFNotes As Boolean
    Private m_lCaseID As Integer
    Private m_sCaseNumber As String = ""
    Private m_lBaseCaseID As Integer
    Private m_lBaseClaimCntSearch As Long
    Private m_bPassBaseClaimID As Boolean
    'developer guide no. 50
    Dim objNotes As frmNotesPreview

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
    Public Property EventCnt() As Integer
        Get

            Return m_lEventCnt

        End Get
        Set(ByVal Value As Integer)

            m_lEventCnt = Value
            RaiseEvent EventCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property
    <Browsable(True)> _
    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

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
    Public Property OldPartyTypeID() As Integer
        Get
            Return m_lOldPartyTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lOldPartyTypeID = Value
            RaiseEvent OldPartyTypeIDChange()
        End Set
    End Property

    <Browsable(True)> _
    Public Property BaseClaimId() As Integer
        Get
            Return m_lBaseClaimId
        End Get
        Set(ByVal Value As Integer)
            m_lBaseClaimId = Value
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
    Public Property DocumentCnt() As Integer
        Get

            Return m_lDocumentCnt

        End Get
        Set(ByVal Value As Integer)

            m_lDocumentCnt = Value
            RaiseEvent DocumentCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property OldAddressCnt() As Integer
        Get

            Return m_lOldAddressCnt

        End Get
        Set(ByVal Value As Integer)

            m_lOldAddressCnt = Value
            RaiseEvent OldAddressCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property NewAddressCnt() As Integer
        Get

            Return m_lNewAddressCnt

        End Get
        Set(ByVal Value As Integer)

            m_lNewAddressCnt = Value
            RaiseEvent NewAddressCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property EventType() As String
        Get

            Return m_sEventType

        End Get
        Set(ByVal Value As String)

            m_sEventType = Value
            RaiseEvent EventTypeChange()

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
    'sj 04/10/2002 - start
    <Browsable(False)> _
    Public WriteOnly Property AccountKey() As Integer
        Set(ByVal Value As Integer)
            m_lAccountKey = Value
        End Set
    End Property
    <Browsable(False)> _
    Public WriteOnly Property EventGroupCode() As String
        Set(ByVal Value As String)
            m_sEventGroupCode = Value
        End Set
    End Property
    <Browsable(False)> _
    Public WriteOnly Property DisableEventGroupLookup() As Boolean
        Set(ByVal Value As Boolean)
            m_bDisableEventGroupLookup = Value
        End Set
    End Property
    'sj 04/10/2002 - end
    'FSA Phase III
    <Browsable(True)> _
    Public Property FSAComplaintFolderCnt() As Integer
        Get

            Return m_lFSAComplaintFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lFSAComplaintFolderCnt = Value
            RaiseEvent FSAComplaintFolderCntChange()

        End Set
    End Property

    <Browsable(True)> _
    Public Property EnableDefaultedFields() As Boolean
        Get
            Return m_bEnableDefaultedFields
        End Get
        Set(ByVal Value As Boolean)
            m_bEnableDefaultedFields = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property ShowNonNotes() As Boolean
        Get
            Return m_bShowNonNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bShowNonNotes = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property ShowNotes() As Boolean
        Get
            Return m_bShowNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bShowNotes = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property RTFNotes() As Boolean
        Get
            Return m_bRTFNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bRTFNotes = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property CaseID() As Integer
        Get
            Return m_lCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lCaseID = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property
    <Browsable(True)> _
    Public Property BaseCaseID() As Integer
        Get
            Return m_lBaseCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lBaseCaseID = Value
        End Set
    End Property
    Public Property BaseClaimCntSearch() As Long
        Get
            Return m_lBaseClaimCntSearch
        End Get
        Set(value As Long)
            m_lBaseClaimCntSearch = value
        End Set
    End Property

    Public Property PassBaseClaimID() As Boolean
        Get
            Return m_bPassBaseClaimID
        End Get
        Set(value As Boolean)
            m_bPassBaseClaimID = value
        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    Public Function CancelClick() As Integer
        'Dim ACApp As Object

        ' Click event of the Cancel button.

        Try


            Return CancelListEvents()

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
    Public Function ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing, Optional ByRef lContextID As Object = Nothing) As Integer

        ' Fire up the help screen
        'developer guide no. 20
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        Return PMHelpFunc.ShowHelp(cmdHelp, lContextID)


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
        'Dim ACApp As Object

        Try


            Return SelectListEvents()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ViewClick
    '
    ' Description:
    '
    ' History: 30/09/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function ViewClick() As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(CallNotesInterface(v_iTask:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallNotesInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewClick")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ViewClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AddClick
    '
    ' Description:
    '
    ' History: 30/09/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function AddClick() As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(CallNotesInterface(v_iTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallNotesInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClick")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: CallNotesInterface
    '
    ' Description:
    '
    ' History: 30/09/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function CallNotesInterface(ByVal v_iTask As Integer) As Integer
        Dim result As Integer = 0
        'Dim ACApp, ACIEventCnt As Object
        'Dim ACIClaimCnt, ACIDate, ACIDescription, ACIEventLogSubjectId, ACIEventTypeGroupDescription, ACIInsuranceFileCnt, ACIInsuranceFolderCnt, ACIIsCompleted, ACIPriorityCode, ACIRTFNotes, ACIUserName As Byte

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lIndex As Integer
            Dim lStatus As gPMConstants.PMEReturnCode

            If v_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                If lvwSearchDetails.FocusedItem.Index + 1 < 1 Then
                    Return result
                End If
                '  2005StickyNotes

                m_lEventCnt = CInt(m_vSearchData(CInt(ACIEventCnt), lIndex))


                lIndex = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
            End If

            If m_oNotes Is Nothing Then

                m_oNotes = New iPMBNote.Interface_Renamed()
                'm_lReturn = CType(m_oNotes, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                m_lReturn = m_oNotes.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oNotes.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotesInterface")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            '
            With m_oNotes
                .PartyCnt = m_lPartyCnt
                'AR20041020 - PN15932
                'Use module level variables if Adding an Event
                If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    .InsuranceFileCnt = m_lInsuranceFileCnt
                Else
                    If Strings.Len(CStr(m_vSearchData(ACIInsuranceFileCnt, lIndex))) > 0 Then
                        .InsuranceFileCnt = CInt(m_vSearchData(ACIInsuranceFileCnt, lIndex))
                    Else
                        .InsuranceFileCnt = 0
                    End If
                End If

                'AR20041020 - PN15932
                'Use module level variables if Adding an Event
                If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    .InsuranceFolderCnt = m_lInsuranceFolderCnt
                Else
                    If Strings.Len(CStr(m_vSearchData(ACIInsuranceFolderCnt, lIndex))) > 0 Then
                        .InsuranceFolderCnt = CInt(m_vSearchData(ACIInsuranceFolderCnt, lIndex))
                    Else
                        .InsuranceFolderCnt = 0
                    End If
                End If

                'AR20041020 - PN15932
                'Use module level variables if Adding an Event
                If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    .ClaimCnt = m_lClaimCnt
                    .RTFNotes = m_bRTFNotes
                    .CaseID = m_lCaseID
                    .BaseClaimID = m_lBaseClaimId
                Else
                    If Strings.Len(CStr(m_vSearchData(ACIClaimCnt, lIndex))) > 0 Then
                        .ClaimCnt = CInt(m_vSearchData(ACIClaimCnt, lIndex))
                    Else
                        .ClaimCnt = 0
                    End If
                End If
                .AccountKey = m_lAccountKey
                .ClaimRef = m_sClaimDesc 'm_vSearchData(ACIClaimDesc, lIndex)
                If v_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                    .EventCnt = m_lEventCnt '2005 StickyNotes
                    .NoteDate = CDate(m_vSearchData(ACIDate, lIndex))
                    .UserName = CStr(m_vSearchData(ACIUserName, lIndex))
                    .EventLogSubjectId = CInt(Conversion.Val(CStr(m_vSearchData(ACIEventLogSubjectId, lIndex))))
                    .Context = CStr(m_vSearchData(ACIEventTypeGroupDescription, lIndex))
                    .Description = CStr(m_vSearchData(ACIDescription, lIndex))
                    .PriorityCode = CStr(m_vSearchData(ACIPriorityCode, lIndex)) '2005StickyNotes
                    Dim dbNumericTemp As Double
                    If Double.TryParse(CStr(m_vSearchData(ACIIsCompleted, lIndex)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        .IsCompleted = CInt(m_vSearchData(ACIIsCompleted, lIndex)) '2005StickyNotes
                    End If
                    .RTFNotes = m_bRTFNotes
                    .RTFText = CStr(m_vSearchData(ACIRTFNotes, lIndex))
                Else
                    .NoteDate = DateTime.Now
                    .UserName = g_sUserName
                End If
            End With

            m_lReturn = m_oNotes.SetProcessModes(vTask:=v_iTask, vTransactionType:=m_sTransactionType)

            m_lReturn = m_oNotes.SetProcessModes(vTask:=v_iTask)

            m_lReturn = m_oNotes.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oNotes.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotesInterface")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lStatus = m_oNotes.Status

            '2005 Sticky Notes
            If v_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                m_vSearchData(ACIIsCompleted, lIndex) = m_oNotes.IsCompleted
            End If

            m_oNotes.Dispose()

            m_oNotes = Nothing

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd And lStatus = gPMConstants.PMEReturnCode.PMOK Then
                m_lReturn = GetBusiness()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to refresh search details from business", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotesInterface")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallNotesInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotesInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




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
        'Dim ACApp As Object

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
        'Dim ACBusinessFail, ACBusinessFailTitle As Object
        Dim g_iSourceID, g_iLanguageID As Integer
        'Dim ACApp As Object


        Static bIsInitialised As Boolean

        Dim sTitle, sMessage As String

        Dim sHelpFile As String = ""
        Dim m_lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        'developer guide no.101
        Dim vValue As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=CStr(ACApp))

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.

                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="Initialise")

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
                g_sUserName = .UserName
            End With

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIREvent.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACBusinessFailTitle), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACBusinessFail), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            'sj 27/09/2002 - start
            'developer guide no.98
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.ToSafeInteger(vValue) = 1 Then
                m_bIsNRMA = True
            Else
                m_bIsNRMA = False
            End If
            'sj 27/09/2002 - end

            'SJ 20/02/2004 - start
            m_lReturn = CheckForUnderwritingBranch(v_iSourceId:=g_oObjectManager.SourceID, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            'SJ 20/02/2004 - end

            m_bShowNonNotes = True
            m_bShowNotes = True

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' hold Initialised status
            bIsInitialised = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
        'Dim ACApp As Object
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
                If m_oNotes IsNot Nothing Then
                    m_oNotes.Dispose()
                    m_oNotes = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub

    Private Const vbFormCode As Integer = 3
    ' ***************************************************************** '
    ' Name: UnloadControl
    '
    ' Description: Cleans up then unloads the control
    '
    ' ***************************************************************** '
    Public Function UnLoadControl(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer
        'Dim ACApp As Object

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
    Public Function GetEvents() As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.
            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Events", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEvents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    ' Edit History  :
    ' RAM20040224   : Bug fix for PN Issue 10592
    '                 Code added to filter events based on FromDate & ToDate
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lInsuranceFolderCnt, lInsuranceFileCnt, lClaimCnt As Integer
            Dim sPolicyDesc As String = ""
            Dim lFSAComplaintFolderCnt As Integer 'FSA Phase III
            Dim lBaseClaimId As Integer

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

            ' RAM20040224 : Bug fix for PN Issue 10592
            If m_bFromRefreshButton Then
                lInsuranceFolderCnt = m_lInsuranceFolderCntSearch
                'lInsuranceFileCnt = m_lInsuranceFileCntSearch
                lClaimCnt = m_lClaimCntSearch
                If m_bPassBaseClaimID Then   'PN: 75724
                    lBaseClaimId = m_lBaseClaimId
                    lClaimCnt = 0
                Else
                    lBaseClaimId = 0
                End If
                lBaseClaimId = m_lBaseClaimId 'PN: 74822
                lFSAComplaintFolderCnt = m_lFSAComplaintFolderCnt
            Else
                If m_bEnableDefaultedFields Then
                    lInsuranceFolderCnt = 0
                    lInsuranceFileCnt = 0
                    lClaimCnt = 0
                    lBaseClaimId = 0
                Else
                    lInsuranceFolderCnt = m_lInsuranceFolderCnt
                    'lInsuranceFileCnt = m_lInsuranceFileCnt
                    lClaimCnt = m_lClaimCnt
                    lBaseClaimId = m_lBaseClaimId
                End If
                lFSAComplaintFolderCnt = m_lFSAComplaintFolderCnt
            End If


            If (Convert.IsDBNull(m_dtFromDate) Or IsNothing(m_dtFromDate)) And (Convert.IsDBNull(m_dtToDate) Or IsNothing(m_dtToDate)) Then
                ' Do as normal

                m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_vPartyCnt:=m_lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolderCnt, v_vInsuranceFileCnt:=lInsuranceFileCnt, v_vClaimCnt:=lClaimCnt, v_vOldPartyTypeID:=m_lOldPartyTypeID, v_vFSAComplaintFolderCnt:=m_lFSAComplaintFolderCnt, v_vNotesArray:=m_vNotesArray, r_vEventTypeGroupArray:=m_vEventTypeGroupArray, v_vAccountKey:=m_lAccountKey, v_vBaseClaimID:=lBaseClaimId, v_vCaseID:=m_lCaseID, v_vBaseCaseID:=m_lBaseCaseID)
            Else
                If Information.IsDate(m_dtFromDate) And Information.IsDate(m_dtToDate) Then
                    ' we have both From date and To Date

                    m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_vPartyCnt:=m_lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolderCnt, v_vInsuranceFileCnt:=lInsuranceFileCnt, v_vClaimCnt:=lClaimCnt, v_vOldPartyTypeID:=m_lOldPartyTypeID, v_vFSAComplaintFolderCnt:=m_lFSAComplaintFolderCnt, v_vNotesArray:=m_vNotesArray, r_vEventTypeGroupArray:=m_vEventTypeGroupArray, v_vAccountKey:=m_lAccountKey, v_vFromDate:=m_dtFromDate, v_vToDate:=m_dtToDate, v_vBaseClaimID:=lBaseClaimId, v_vCaseID:=m_lCaseID, v_vBaseCaseID:=m_lBaseCaseID)
                ElseIf Information.IsDate(m_dtFromDate) Then
                    ' We have From Date

                    m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_vPartyCnt:=m_lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolderCnt, v_vInsuranceFileCnt:=lInsuranceFileCnt, v_vClaimCnt:=lClaimCnt, v_vOldPartyTypeID:=m_lOldPartyTypeID, v_vFSAComplaintFolderCnt:=m_lFSAComplaintFolderCnt, v_vNotesArray:=m_vNotesArray, r_vEventTypeGroupArray:=m_vEventTypeGroupArray, v_vAccountKey:=m_lAccountKey, v_vFromDate:=m_dtFromDate, v_vBaseClaimID:=lBaseClaimId, v_vCaseID:=m_lCaseID, v_vBaseCaseID:=m_lBaseCaseID)
                ElseIf Information.IsDate(m_dtToDate) Then
                    ' We have To Date

                    m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_vPartyCnt:=m_lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolderCnt, v_vInsuranceFileCnt:=lInsuranceFileCnt, v_vClaimCnt:=lClaimCnt, v_vOldPartyTypeID:=m_lOldPartyTypeID, v_vFSAComplaintFolderCnt:=m_lFSAComplaintFolderCnt, v_vNotesArray:=m_vNotesArray, r_vEventTypeGroupArray:=m_vEventTypeGroupArray, v_vAccountKey:=m_lAccountKey, v_vToDate:=m_dtToDate, v_vBaseClaimID:=lBaseClaimId, v_vCaseID:=m_lCaseID, v_vBaseCaseID:=m_lBaseCaseID)
                End If
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lInsuranceFileCnt > 0 Then
                sPolicyDesc = txtPolicy.Text
                m_lReturn = CType(GetPolicyDesc(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_sPolicyDesc:=sPolicyDesc), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                    Return result
                End If
                txtPolicy.Text = sPolicyDesc
            Else
                txtPolicy.Text=String.Empty
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
        'Dim ACApp As Object
        'Dim ACColHeadClaim, ACColHeadDescription, ACColHeadIsCompleted, ACColHeadPolicy, ACColHeadPriorityCode, ACColHeadType, ACColHeadUser As Double
        'Dim ACIAlternateReference, ACICampaignDesc, ACICaseNumber, ACIClaimDesc, ACIDate, ACIDescription, ACIDocumentDesc, ACIEventLogSubject, ACIEventType, ACIEventTypeGroupDescription, ACIHasNotes, ACIInsuranceFolderDesc, ACIIsCompleted, ACIPriorityCode, ACIReason, ACIReportDesc, ACIUserName As Byte

        Dim oListItem As ListViewItem
        Dim sDescription As String = ""
        Dim bInclude As Boolean
        Dim bNotes As Boolean
        'SJ 20/02/2004 - start
        Dim sInsuranceReference As String
        'SJ 20/02/2004 - end

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            m_lItemsFound = gPMConstants.PMEFormatStyle.PMFormatString

            ' Check that search details are valid before
            ' continuing.
            m_lReturn = PopulateAndShowCombos()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PopulateAndShowCombos Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(m_vSearchData) Then
                'sj 04/10/2002 - start
                DisplayStatusFound()
                'sj 04/10/2002 - end
                Return result
            End If

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                m_lReturn = CType(IncludeRow(lRow:=lRow, bInclude:=bInclude), gPMConstants.PMEReturnCode)

                If bInclude Then

                    m_lItemsFound = CType(m_lItemsFound + 1, gPMConstants.PMEFormatStyle)
                    ' Assign the details to the first column.
                    ' Column 1 date
                    m_lReturn = m_oFormFields.FormatControl(txtDate, m_vSearchData(ACIDate, lRow))

                    bNotes = gPMFunctions.NullToString(m_vSearchData(ACIHasNotes, lRow)).ToUpper() = "YES"

                    If bNotes Then

                        oListItem = lvwSearchDetails.Items.Add(txtDate.Text, "NotesImage")
                    Else

                        oListItem = lvwSearchDetails.Items.Add(txtDate.Text, "FindImage")
                    End If

                    'sj 27/09/2002 - start
                    'Type
                    ListViewHelper.GetListViewSubItem(oListItem, ACColHeadType - 1).Text = CStr(m_vSearchData(ACIEventTypeGroupDescription, lRow)).Trim()
                    'sj 27/09/2002 - end

                    'SJ 20/02/2004 - start
                    sInsuranceReference = CStr(m_vSearchData(ACIInsuranceFolderDesc, lRow)).Trim()
                    If m_bIsUnderwritingBranch Then
                        If gPMFunctions.NullToString(m_vSearchData(ACIAlternateReference, lRow)) <> "" Then
                            sInsuranceReference = CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim()
                        End If
                    End If
                    'SJ 20/02/2004 - end

                    ' Assign details to the other columns
                    ' Column 2 Insurance Ref
                    'oListItem.SubItems(ACColHeadPolicy - 1) = Trim$(m_vSearchData(ACIInsuranceFolderDesc, lRow&))
                    ListViewHelper.GetListViewSubItem(oListItem, ACColHeadPolicy - 1).Text = sInsuranceReference

                    ' Column 3 claim
                    If lvwSearchDetails.Columns.Item(CInt(ACColHeadClaim) - 1).Text = "Case" And m_lClaimCnt <> 0 Then
                        ListViewHelper.GetListViewSubItem(oListItem, ACColHeadClaim - 1).Text = CStr(m_vSearchData(ACICaseNumber, lRow)).Trim()
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, ACColHeadClaim - 1).Text = CStr(m_vSearchData(ACIClaimDesc, lRow)).Trim()
                    End If
                    ' Column 4 description

                    If Convert.IsDBNull(m_vSearchData(ACIDescription, lRow)) Or IsNothing(m_vSearchData(ACIDescription, lRow)) Or (CStr(m_vSearchData(ACIDescription, lRow)) = "") Then

                        Select Case CStr(m_vSearchData(ACIEventType, lRow)).Trim()
                            Case "NEWCLIENT"
                                sDescription = "Client created"
                            Case "NEWPOLICY"
                                sDescription = "Added policy " & CStr(m_vSearchData(ACIInsuranceFolderDesc, lRow)).Trim()
                            Case "NEWCLAIM"
                                sDescription = "Added claim " & CStr(m_vSearchData(ACIClaimDesc, lRow)).Trim()
                            Case "ADDCHANGE"
                                sDescription = "Changed client's address"
                            Case "POLCHANGE"
                                sDescription = "Changed policy details"
                            Case "CLACHANGE"
                                sDescription = "Changed claim details"
                            Case "DELCLIENT"
                                sDescription = "Client deleted"
                            Case "DELPOLICY"
                                sDescription = "Deleted policy " & CStr(m_vSearchData(ACIInsuranceFolderDesc, lRow)).Trim()
                            Case "DELCLAIM"
                                sDescription = "Deleted claim " & CStr(m_vSearchData(ACIClaimDesc, lRow)).Trim()
                            Case "DOCUMENT"
                                sDescription = "Raised Document - " & CStr(m_vSearchData(ACIDocumentDesc, lRow)).Trim()
                            Case "REPORT"
                                sDescription = "Ran report - " & CStr(m_vSearchData(ACIReportDesc, lRow)).Trim()
                            Case "MAILSHOT"
                                sDescription = "Client included in mailshot " & CStr(m_vSearchData(ACICampaignDesc, lRow)).Trim()
                            Case "TRANSACT"
                                ' MS130601
                                If CStr(m_vSearchData(ACIReason, lRow)).Trim().Length > 0 Then
                                    sDescription = CStr(m_vSearchData(ACIReason, lRow)).Trim()
                                Else
                                    sDescription = "Transaction raised"
                                End If
                                ' CTAF 240800
                            Case "PTYPECHNG"
                                sDescription = "Change of party type"
                            Case Else
                                sDescription = "Unknown event"
                        End Select
                    Else
                        'sj 27/09/2002 - start
                        Select Case CStr(m_vSearchData(ACIEventType, lRow)).Trim()
                            Case "N_CUST", "N_ACCOUNT", "N_POLICY", "N_CLAIMS", "N_FSA"
                                If CStr(m_vSearchData(ACIEventLogSubject, lRow)).Trim() <> "" Then
                                    sDescription = "Note:" & CStr(m_vSearchData(ACIEventLogSubject, lRow)).Trim()
                                Else
                                    sDescription = "Note:" & CStr(m_vSearchData(ACIDescription, lRow)).Trim()
                                End If
                            Case Else
                                sDescription = CStr(m_vSearchData(ACIDescription, lRow)).Trim()
                        End Select
                        'sj 27/09/2002 - end
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, ACColHeadDescription - 1).Text = sDescription.Trim().Replace(Strings.Chr(13) & Strings.Chr(10), " ")

                    ' Column 5 user
                    ListViewHelper.GetListViewSubItem(oListItem, ACColHeadUser - 1).Text = CStr(m_vSearchData(ACIUserName, lRow)).Trim()
                    '2005 StickyNotes
                    ' Column 6 PriorityCode
                    ListViewHelper.GetListViewSubItem(oListItem, ACColHeadPriorityCode - 1).Text = CStr(m_vSearchData(ACIPriorityCode, lRow)).Trim()
                    ' Column 7 Status
                    If CStr(m_vSearchData(ACIIsCompleted, lRow)).Trim() = "1" Then
                        ListViewHelper.GetListViewSubItem(oListItem, ACColHeadIsCompleted - 1).Text = "Completed"
                    Else
                        ' If the event is a sticky note  PN23203
                        If CStr(m_vSearchData(ACIEventType, lRow)).Trim() = gSIRLibrary.ACNotesWarning Then
                            ListViewHelper.GetListViewSubItem(oListItem, ACColHeadIsCompleted - 1).Text = "Outstanding"
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, ACColHeadIsCompleted - 1).Text = ""
                        End If
                    End If
                    ' {* USER DEFINED CODE (End) *}

                    ' Set the tag property with the index of
                    ' the search data storage.
                    oListItem.Tag = CStr(lRow)

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    '            If (lRow& = PMListRefreshValue) Then
                    If m_lItemsFound = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwSearchDetails.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwSearchDetails.Refresh()
                    End If
                End If
            Next lRow

            'sj 01/10/2002 - start
            DisplayStatusFound()
            'sj 01/10/2002 - end

            If lvwSearchDetails.Items.Count = 0 Then EventCnt = 0

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
        'Dim ACApp As Object
        'Dim ACIClaimCnt, ACIClaimDesc, ACIDocumentCnt, ACIDocumentRef, ACIEventCnt, ACIEventLogSubject, ACIEventLogSubjectId, ACIEventType, ACIEventTypeGroupDescription, ACIEventTypeGroupId, ACIFSAComplaintFolderCnt, ACIInsuranceFileCnt, ACIInsuranceFileStructureId, ACIInsuranceFolderCnt, ACIInsuranceFolderDesc, ACINewAddressCnt, ACIOldAddressCnt As Byte

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
            m_lEventCnt = 0
            'sj 02/10/2002 - start
            m_lClassInsuranceFileCnt = 0
            'sj 02/10/2002 - end
            m_lInsuranceFileStructureId = 0
            m_sPolicyDesc = ""
            'sj 02/10/2002 - start
            m_lClassClaimCnt = 0
            'm_lClaimCnt = 0
            'sj 02/10/2002 - end
            m_sClaimDesc = ""
            m_lOldAddressCnt = 0
            m_lNewAddressCnt = 0
            m_lDocumentCnt = 0
            m_sEventType = ""
            m_lClassInsuranceFolderCnt = 0

            m_lEventCnt = CInt(m_vSearchData(ACIEventCnt, lSelectedItem))

            If CStr(m_vSearchData(ACIInsuranceFolderCnt, lSelectedItem)) <> "" Then
                m_lClassInsuranceFolderCnt = CInt(m_vSearchData(ACIInsuranceFolderCnt, lSelectedItem))
            End If

            If CStr(m_vSearchData(ACIInsuranceFileCnt, lSelectedItem)) <> "" Then
                m_lClassInsuranceFileCnt = CInt(m_vSearchData(ACIInsuranceFileCnt, lSelectedItem))
            End If

            If CStr(m_vSearchData(ACIInsuranceFileStructureId, lSelectedItem)) <> "" Then
                m_lInsuranceFileStructureId = CInt(m_vSearchData(ACIInsuranceFileStructureId, lSelectedItem))
            End If

            If CStr(m_vSearchData(ACIInsuranceFolderDesc, lSelectedItem)) <> "" Then
                m_sPolicyDesc = CStr(m_vSearchData(ACIInsuranceFolderDesc, lSelectedItem)).Trim()
            End If

            If CStr(m_vSearchData(ACIClaimCnt, lSelectedItem)) <> "" Then
                m_lClassClaimCnt = CInt(m_vSearchData(ACIClaimCnt, lSelectedItem))
            End If

            If CStr(m_vSearchData(ACIClaimDesc, lSelectedItem)) <> "" Then
                m_sClaimDesc = CStr(m_vSearchData(ACIClaimDesc, lSelectedItem)).Trim()
            End If

            If CStr(m_vSearchData(ACIOldAddressCnt, lSelectedItem)) <> "" Then
                m_lOldAddressCnt = CInt(m_vSearchData(ACIOldAddressCnt, lSelectedItem))
            End If

            If CStr(m_vSearchData(ACINewAddressCnt, lSelectedItem)) <> "" Then
                m_lNewAddressCnt = CInt(m_vSearchData(ACINewAddressCnt, lSelectedItem))
            End If

            If CStr(m_vSearchData(ACIDocumentCnt, lSelectedItem)) <> "" Then
                m_lDocumentCnt = CInt(m_vSearchData(ACIDocumentCnt, lSelectedItem))
            End If

            If CStr(m_vSearchData(ACIEventType, lSelectedItem)) <> "" Then
                m_sEventType = CStr(m_vSearchData(ACIEventType, lSelectedItem)).Trim()
            End If

            If CStr(m_vSearchData(ACIDocumentRef, lSelectedItem)) <> "" Then
                m_sDocumentRef = CStr(m_vSearchData(ACIDocumentRef, lSelectedItem)).Trim()
            End If

            If CStr(m_vSearchData(ACIEventLogSubject, lSelectedItem)) <> "" Then
                m_sEventLogSubject = CStr(m_vSearchData(ACIEventLogSubject, lSelectedItem)).Trim()
            End If

            If CStr(m_vSearchData(ACIEventTypeGroupId, lSelectedItem)) <> "" Then
                m_lEventTypeGroupId = CInt(Conversion.Val(CStr(m_vSearchData(ACIEventTypeGroupId, lSelectedItem))))
            End If

            If CStr(m_vSearchData(ACIEventTypeGroupDescription, lSelectedItem)) <> "" Then
                m_sEventTypeGroupDescription = CStr(m_vSearchData(ACIEventTypeGroupDescription, lSelectedItem)).Trim()
            End If

            If CStr(m_vSearchData(ACIEventLogSubjectId, lSelectedItem)) <> "" Then
                m_lEventLogSubjectId = CInt(Conversion.Val(CStr(m_vSearchData(ACIEventLogSubjectId, lSelectedItem))))
            End If

            If CStr(m_vSearchData(ACIFSAComplaintFolderCnt, lSelectedItem)) <> "" Then
                m_lFSAComplaintFolderCnt = CInt(Conversion.Val(CStr(m_vSearchData(ACIFSAComplaintFolderCnt, lSelectedItem))))
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
    'Dim result As Integer = 0
    'Dim ACApp As Object
    '
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
    'Dim result As Integer = 0
    'Dim ACApp As Object
    '
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
    ' Edit History  :
    ' RAM20040224   : Code changes related to PN Issue 10592
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        'Dim ACApp, ACColHeadUser, ACColHeadDescription, ACColHeadClaim, ACColHeadPolicy, ACColHeadEventDate As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MSS081001 - Added AJM's check for merge
            'AJM 27/09/2001 - in open claim show all events for the client so enable policy filter
            If m_sTransactionType = "C_CO" Then
                txtPolicy.Enabled = True
                cmdFindPolicy.Enabled = True
            Else
                'sj 21/04/2004 - Add insurance_folder_cnt
                If (m_lInsuranceFileCnt = 0) And (m_lInsuranceFolderCnt = 0) Then
                    txtPolicy.Enabled = True
                    cmdFindPolicy.Enabled = True
                Else
                    txtPolicy.Text = m_sPolicyDesc
                    txtPolicy.Enabled = m_bEnableDefaultedFields
                    cmdFindPolicy.Enabled = m_bEnableDefaultedFields
                End If
            End If

            If m_lClaimCnt = 0 And m_lBaseClaimId = 0 Then
                txtClaim.Enabled = True
                cmdFindClaim.Enabled = True
            Else
                txtClaim.Text = m_sClaimDesc
                txtClaim.Enabled = m_bEnableDefaultedFields
                cmdFindClaim.Enabled = m_bEnableDefaultedFields
                If (m_lClaimCnt = 0 And m_lBaseClaimId <> 0 And m_bPassBaseClaimID = True) Then
                    m_bPassBaseClaimID = True
                Else
                    m_bPassBaseClaimID = m_bEnableDefaultedFields
                End If
            End If

            If m_lCaseID = 0 Then
                lblCaseNumber.Visible = False
                txtCaseNumber.Visible = False
            Else
                lblCaseNumber.Visible = True
                txtCaseNumber.Visible = True
                cmdFindPolicy.Visible = False
                txtPolicy.Enabled = False
                txtPolicy.Visible = False
                txtCaseNumber.Text = m_sCaseNumber
            End If

            'SJ 21/04/2004 - start
            If m_bDisableEventGroupLookup And m_sEventGroupCode = "RENEWALS" Then
                txtClaim.Text = ""
                txtClaim.Enabled = False
                cmdFindClaim.Enabled = False
            End If
            'SJ 21/04/2004 - end

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add something here to set the default status to current


            ' {* USER DEFINED CODE (Begin) *}

            ' Set the column widths for the search list.

            lvwSearchDetails.Columns.Item(CInt(ACColHeadEventDate) - 1).Width = CInt(VB6.TwipsToPixelsX(1600))

            'MSS081001 - Added AJM's check for merge
            'AJM 27/09/2001 - In open claim we also need to show the policy number
            If m_sTransactionType = "C_CO" Then

                lvwSearchDetails.Columns.Item(CInt(ACColHeadPolicy) - 1).Width = CInt(VB6.TwipsToPixelsX(1440))
            Else
                If m_lInsuranceFileCnt = 0 Then

                    lvwSearchDetails.Columns.Item(CInt(ACColHeadPolicy) - 1).Width = CInt(VB6.TwipsToPixelsX(1440))
                Else

                    lvwSearchDetails.Columns.Item(CInt(ACColHeadPolicy) - 1).Width = CInt(0)
                End If
            End If
            'MSS081001 - Merge end

            If (m_lClaimCnt = 0) Or (m_lCaseID <> 0) Then

                lvwSearchDetails.Columns.Item(CInt(ACColHeadClaim) - 1).Width = CInt(VB6.TwipsToPixelsX(1440))
            Else

                lvwSearchDetails.Columns.Item(CInt(ACColHeadClaim) - 1).Width = CInt(0)
            End If

            If m_lClaimCnt <> 0 And (Not (m_lCaseID <> 0)) Then

                lvwSearchDetails.Columns.Item(CInt(ACColHeadClaim) - 1).Width = CInt(VB6.TwipsToPixelsX(1440))
            End If


            lvwSearchDetails.Columns.Item(CInt(ACColHeadDescription) - 1).Width = CInt(VB6.TwipsToPixelsX(7200))


            lvwSearchDetails.Columns.Item(CInt(ACColHeadUser) - 1).Width = CInt(VB6.TwipsToPixelsX(1440))

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            _stbStatus_Panel1.Width = stbStatus.Width - 10
            '''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040224   : PN Issue 10592 - START
            '''''''''''''''''''''''''''''''''''''''''''
            ' set to Null before searching

            m_dtFromDate = Nothing

            m_dtToDate = Nothing
            '''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040224   : PN Issue 10592 - END
            '''''''''''''''''''''''''''''''''''''''''''

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
    'Dim result As Integer = 0
    'Dim ACApp, ACClearDetails As Object
    'Dim g_iLanguageID As Integer
    'Dim ACClearDetailsTitle As Object
    '
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


    'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACClearDetailsTitle), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '


    'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACClearDetails), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
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
        'Dim ACApp As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            'Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'From Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFromDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'To Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtToDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

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
        'Dim ACApp As Object

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
    ' Edit History  :
    ' RAM20040224   : Bug fix for PN Issue 10592.
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer
        Dim result As Integer = 0
        'Dim ACApp, ACCaseNumber, ACRefreshButton, ACToDate, ACFromDate, ACUserName, ACEventType, ACClaim, ACPolicy, ACListTitle5, ACColHeadUser, ACListTitle4, ACColHeadDescription, ACListTitle3, ACListTitleCase, ACColHeadClaim, ACListTitle2, ACColHeadPolicy As Object
        Dim g_iLanguageID As Integer
        'Dim ACListTitle1, ACColHeadEventDate As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' {* USER DEFINED CODE (Begin) *}




            lvwSearchDetails.Columns.Item(CInt(ACColHeadEventDate) - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACListTitle1), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))





            lvwSearchDetails.Columns.Item(CInt(ACColHeadPolicy) - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACListTitle2), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_lClaimCnt <> 0 Then




                lvwSearchDetails.Columns.Item(CInt(ACColHeadClaim) - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACListTitleCase), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            If m_lCaseID <> 0 Then




                lvwSearchDetails.Columns.Item(CInt(ACColHeadClaim) - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACListTitle3), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If





            lvwSearchDetails.Columns.Item(CInt(ACColHeadDescription) - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACListTitle4), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))





            lvwSearchDetails.Columns.Item(CInt(ACColHeadUser) - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACListTitle5), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAM20040224 : PN Issue 10592


            cmdFindPolicy.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACPolicy), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAM20040224 : PN Issue 10592


            cmdFindClaim.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACClaim), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_bIsNRMA Then
                lblType.Text = "Context:"
            Else


                lblType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACEventType), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If



            lblUser.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACUserName), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lblFromDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACFromDate), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lblToDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACToDate), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAM20040224 : PN Issue 10592


            cmdRefresh.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACRefreshButton), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lblCaseNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACCaseNumber), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
        'Dim ACApp As Object

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
    ' Name: CancelListEvents
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function CancelListEvents() As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the ListEvents", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelListEvents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SelectListEvents
    '
    ' Description: Called when we wish to select
    '
    ' ***************************************************************** '
    Private Function SelectListEvents() As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select the ListEvents", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectListEvents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'Dim ACApp As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.
            If Status = gPMConstants.PMEReturnCode.PMCancel Then
                ' Get string messages
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
        'Dim ACApp As Object
        Dim g_iLanguageID As Integer
        'Dim ACStatusSearching As Integer

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
        'Dim ACApp As Object
        Dim g_iLanguageID As Integer
        'Dim ACStatusFound As Integer

        Static sMessage As String = ""

        Try

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
    'Dim result As Integer = 0
    'Dim ACApp As Object
    '
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
    '
    ' Name: IncludeRow
    '
    ' Description:
    '
    ' History: 30/11/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function IncludeRow(ByRef lRow As Integer, ByRef bInclude As Boolean) As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object
        'Dim ACIAlternateReference, ACIClaimDesc, ACIDate, ACIEventTypeGroupId, ACIHasNotes, ACIInsuranceFolderDesc, ACIPolicyTypeId, ACISourceId, ACIUnderwritingBranchInd, ACIUserName As Byte
        Dim g_iSourceID As Object = Nothing
        Dim dtTemp As Date
        Dim sTemp As String = ""

        'SJ 20/02/2004 - start
        Dim sInsuranceReference, sAlternateReference As String
        'SJ 20/02/2004 - end
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bInclude = False

            'SJ 18/8/2004 - PN 14263 Client record does not have a policy reference
            If m_bUnderwritingBranchEnabled Then
                If m_bIsUnderwritingBranch Then

                    If gPMFunctions.NullToString(m_vSearchData(ACIAlternateReference, lRow)) = "" And CDbl(m_vSearchData(ACIPolicyTypeId, lRow)) <> 3 And Not m_vSearchData(ACISourceId, lRow).Equals(g_iSourceID) And CStr(m_vSearchData(ACIInsuranceFolderDesc, lRow)).Trim() <> "" Then
                        Return result
                    End If
                Else
                    If Conversion.Val(CStr(m_vSearchData(ACIUnderwritingBranchInd, lRow))) = 1 Then
                        Return result
                    End If
                End If
            End If

            If txtPolicy.Text.Trim().Length > 0 Then
                'SJ 20/02/2004 - start
                sInsuranceReference = CStr(m_vSearchData(ACIInsuranceFolderDesc, lRow)).Trim()
                If m_bIsUnderwritingBranch Then

                    If Not (Convert.IsDBNull(m_vSearchData(ACIAlternateReference, lRow)) Or IsNothing(m_vSearchData(ACIAlternateReference, lRow))) Then
                        sAlternateReference = CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim()
                        If sAlternateReference <> "" Then
                            sInsuranceReference = sAlternateReference
                        End If
                    End If
                End If
                If sInsuranceReference <> txtPolicy.Text.Trim() Then
                    Return result
                End If
                'SJ 20/02/2004 - end
            End If

            If txtClaim.Text.Trim().Length > 0 Then
                If CStr(m_vSearchData(ACIClaimDesc, lRow)) <> txtClaim.Text Then
                    Return result
                End If
            End If

            'This item is a little more complicated - don't forget
            If cboType.SelectedIndex > 0 Then
                'sj 27/09/2002 - start
                If VB6.GetItemData(cboType, cboType.SelectedIndex) <> Conversion.Val(CStr(m_vSearchData(ACIEventTypeGroupId, lRow))) Then
                    Return result
                End If
            End If

            If cboUser.SelectedIndex > 0 Then
                If CStr(m_vSearchData(ACIUserName, lRow)) <> cboUser.Text Then
                    Return result
                End If
            End If

            If txtFromDate.Text.Trim() <> "" Then

                dtTemp = CDate(m_oFormFields.UnformatControl(ctlControl:=txtFromDate))

                If dtTemp > CDate(m_vSearchData(ACIDate, lRow)) Then
                    Return result
                End If
            End If

            If txtToDate.Text.Trim() <> "" Then

                dtTemp = CDate(m_oFormFields.UnformatControl(ctlControl:=txtToDate)).AddDays(1)

                If dtTemp < CDate(m_vSearchData(ACIDate, lRow)) Then
                    Return result
                End If
            End If

            If gPMFunctions.NullToString(m_vSearchData(ACIHasNotes, lRow)).ToUpper() = "YES" And m_bShowNonNotes And Not m_bShowNotes Then
                Return result
            End If

            bInclude = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IncludeRow Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IncludeRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateAndShowCombos
    '
    ' Description:
    '
    ' History: 30/11/1999 Tomo - Created.
    ' RAM20040224   : Added to refresh the controls, if m_bForceRefresh is true
    '                   Note: m_bFromRefreshButton is set in cmdRefresh_Click event
    ' ***************************************************************** '
    Private Function PopulateAndShowCombos() As Integer
        Dim result As Integer = 0
        'Dim ACApp, ACGDescription As Object
        'Dim ACGCode, ACGEventTypeGroupId, ACIUserName As Byte

        Static bDone As Boolean

        Dim vUsers As Object = Nothing
        Dim bFound As Boolean
        Dim sTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAM20040224 : PN Issue 10592 Changes
            If Not m_bFromRefreshButton Then
                If bDone Then
                    Return result
                End If
            End If

            m_bSetup = True

            bDone = True

            'Clear them down, just in case
            cboType.Items.Clear()
            cboUser.Items.Clear()
            Dim cboType_NewIndex As Integer = -1
            cboType_NewIndex = cboType.Items.Add("(all)")
            cboUser.Items.Add("(all)")

            ' RAM20040224 : PN Issue 10592 Changes
            If Not m_bFromRefreshButton Then
                txtToDate.Text = ""
                txtFromDate.Text = ""
            End If

            If Information.IsArray(m_vSearchData) Then

                For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                    'Users
                    If CStr(m_vSearchData(ACIUserName, lRow)) <> "" Then
                        If Not Information.IsArray(vUsers) Then
                            ReDim vUsers(0)

                            vUsers(0) = m_vSearchData(ACIUserName, lRow)
                        Else
                            bFound = False

                            For lRow2 As Integer = vUsers.GetLowerBound(0) To vUsers.GetUpperBound(0)

                                If m_vSearchData(ACIUserName, lRow).Equals(vUsers(lRow2)) Then
                                    bFound = True
                                    Exit For
                                End If
                            Next lRow2

                            If Not bFound Then

                                ReDim Preserve vUsers(vUsers.GetUpperBound(0) + 1)


                                vUsers(vUsers.GetUpperBound(0)) = m_vSearchData(ACIUserName, lRow)
                            End If
                        End If
                    End If

                Next lRow
            End If

            'Types
            cboType.SelectedIndex = 0

            'sj 27/09/2002 - start
            'Get event_type_group from database rather than hard coding in program
            If Information.IsArray(m_vEventTypeGroupArray) Then
                For lRow As Integer = 0 To m_vEventTypeGroupArray.GetUpperBound(1)


                    cboType_NewIndex = cboType.Items.Add(CStr(m_vEventTypeGroupArray(CInt(ACGDescription), lRow)).Trim())
                    VB6.SetItemData(cboType, cboType_NewIndex, Conversion.Val(CStr(m_vEventTypeGroupArray(ACGEventTypeGroupId, lRow))))
                    If CStr(m_vEventTypeGroupArray(ACGCode, lRow)).Trim().ToUpper() = m_sEventGroupCode.Trim().ToUpper() Then
                        cboType.SelectedIndex = cboType_NewIndex
                    End If
                Next lRow
            End If
            If m_bDisableEventGroupLookup Then
                cboType.Enabled = False
            End If

            'Sort users
            If Information.IsArray(vUsers) Then

                For lRow As Integer = vUsers.GetLowerBound(0) To vUsers.GetUpperBound(0)

                    cboUser.Items.Add(CStr(vUsers(lRow)))
                Next lRow
            End If

            cboUser.SelectedIndex = 0
            '1 for (all), 2 for the only one there...
            If cboUser.Items.Count > 2 Then
                cboUser.Enabled = True
            Else
                cboUser.SelectedIndex = cboUser.Items.Count - 1
                If m_bShowNotes And Not m_bShowNonNotes Then
                    cboUser.Enabled = False
                End If
            End If

            vUsers = Nothing

            m_bSetup = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateAndShowCombos Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAndShowCombos", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)


    ' PRIVATE Events (Begin)

    Private Sub cboType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboType.SelectedIndexChanged

        If Not m_bSetup Then
            m_lReturn = DataToInterface()
        End If

    End Sub

    Private Sub cboUser_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUser.SelectedIndexChanged

        If Not m_bSetup Then
            m_lReturn = DataToInterface()
        End If

    End Sub



    ' ***************************************************************** '
    '
    ' Name: lvwSearchDetails_Click
    '
    ' Description:
    '
    ' History: 01/10/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click
        'Dim ACApp As Object
        'Dim ACIDate, ACIEventCnt As Byte

        Try

            If lvwSearchDetails.Items.Count > 0 Then
                EventCnt = CInt(m_vSearchData(ACIEventCnt, Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)))
                m_dtNoteDate = CDate(m_vSearchData(ACIDate, Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)))
            Else
                EventCnt = 0
                Exit Sub
            End If
            m_lReturn = OKClick()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            End If

            RaiseEvent lvwSearchDetailsClick(Me, New lvwSearchDetailsClickEventArgs(m_lEventCnt, m_lPartyCnt, m_lClassInsuranceFileCnt, m_sPolicyDesc, m_lInsuranceFileStructureId, m_lClassClaimCnt, m_sClaimDesc, m_lOldAddressCnt, m_lNewAddressCnt, m_lDocumentCnt, m_sEventType, m_lOldPartyTypeID, m_sDocumentRef, m_dtNoteDate, m_sEventLogSubject, m_lEventTypeGroupId, m_sEventTypeGroupDescription, m_lEventLogSubjectId))

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="lvwSearchDetails_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub




        End Try
    End Sub

    Private Sub lvwSearchDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvwSearchDetails.KeyUp
        Try
            If e.KeyValue = 40 Or e.KeyValue = 38 Then
                If lvwSearchDetails.Items.Count > 0 Then
                    EventCnt = CInt(m_vSearchData(ACIEventCnt, Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)))
                    m_dtNoteDate = CDate(m_vSearchData(ACIDate, Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)))
                Else
                    EventCnt = 0
                    Exit Sub
                End If
                m_lReturn = OKClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                End If

                RaiseEvent lvwSearchDetailsKeyUp(Me, New lvwSearchDetailsKeyUpEventArgs(m_lEventCnt, m_lPartyCnt, m_lClassInsuranceFileCnt, m_sPolicyDesc, m_lInsuranceFileStructureId, m_lClassClaimCnt, m_sClaimDesc, m_lOldAddressCnt, m_lNewAddressCnt, m_lDocumentCnt, m_sEventType, m_lOldPartyTypeID, m_sDocumentRef, m_dtNoteDate, m_sEventLogSubject, m_lEventTypeGroupId, m_sEventTypeGroupDescription, m_lEventLogSubjectId))
            End If
        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="lvwSearchDetails_KeyUp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_KeyUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub




        End Try
    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick
        'Dim ACApp As Object

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

            RaiseEvent lvwSearchDetailsDblClick(Me, New lvwSearchDetailsDblClickEventArgs(m_lEventCnt, m_lPartyCnt, m_lClassInsuranceFileCnt, m_sPolicyDesc, m_lInsuranceFileStructureId, m_lClassClaimCnt, m_sClaimDesc, m_lOldAddressCnt, m_lNewAddressCnt, m_lDocumentCnt, m_sEventType, m_lOldPartyTypeID, m_sDocumentRef, m_dtNoteDate, m_lFSAComplaintFolderCnt))

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        ListViewFunc.SortListView(lvwSearchDetails, eventArgs)
    End Sub

    Private Sub txtFromDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFromDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFromDate)
    End Sub

    Private Sub txtFromDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFromDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFromDate)
    End Sub

    Private Sub txtToDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtToDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtToDate)
    End Sub

    Private Sub txtToDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtToDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtToDate)
    End Sub

    Private Sub uctListEvents_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try

            If Not m_bResizing Then

                m_bResizing = True

                'tabMainTab.Height = MyBase.Height
                'tabMainTab.Width = MyBase.Width

                'lvwSearchDetails.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(MyBase.Height) - 1680 - 395)
                'lvwSearchDetails.Width = MyBase.Width - VB6.TwipsToPixelsX(240)

                'stbStatus.Top = MyBase.Height - VB6.TwipsToPixelsY(395)
                'stbStatus.Width = MyBase.Width - VB6.TwipsToPixelsX(240)


                m_bResizing = False

            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub cmdFindPolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindPolicy.Click
        'Dim ACApp As Object

        Try

            ' Click event of the Find Policy button.
            m_lReturn = GetPolicies()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the list of policies", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFindPolicy_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Find Policy command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFindPolicy_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdFindClaim_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindClaim.Click
        'Dim ACApp As Object

        Try

            ' Click event of the Find Policy button.
            m_lReturn = GetClaims()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the list of Claims", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFindClaim_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Find Policy command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFindClaim_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' PRIVATE Events (End)

    ' RAM20040224 : Added the following sub. Ref. PN Issue 10592
    Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click
        'Dim ACInvalidToDate, ACInvalidToDateTitle, ACBusinessFail As Object
        Dim g_iLanguageID As Integer
        'Dim ACInvalidFromDateTitle, ACApp As Object

        ' Click event of the Refresh button.

        Dim sFromDate, sToDate, sMessage, sTitle As String
        Dim bGetInsuranceFileFolder, bGetClaimCnt As Boolean
        Dim sClaimCode, sPolicyCode As String

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the flag, that we clicked the cmdRefresh button
            m_bFromRefreshButton = True
            bGetInsuranceFileFolder = False
            bGetClaimCnt = False
            sPolicyCode = ""
            m_lInsuranceFileCntSearch = 0
            m_lInsuranceFolderCntSearch = 0
            sClaimCode = ""
            If Not txtCaseNumber.Visible Then
                m_lClaimCntSearch = 0
            End If
            ' Get the Insurance File Cnt from Policy Code (InsuranceRef)
            If txtPolicy.Enabled Then
                If txtPolicy.Text.Trim().Length > 0 Then
                    sPolicyCode = txtPolicy.Text
                    bGetInsuranceFileFolder = True
                End If
            Else
                ' This is disabled. so we should have the insurance file cnt, insurance folder cnt
                m_lInsuranceFileCntSearch = m_lInsuranceFileCnt
                m_lInsuranceFolderCntSearch = m_lInsuranceFolderCnt
                bGetInsuranceFileFolder = False
            End If

            If bGetInsuranceFileFolder Then
                sPolicyCode = sPolicyCode.Trim()
                m_lReturn = CType(GetInsuranceFileFolderCntForPolicyCode(v_sPolicyCode:=sPolicyCode, r_lInsuranceFileCnt:=m_lInsuranceFileCntSearch, r_lInsuranceFolderCnt:=m_lInsuranceFolderCntSearch), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch InsuranceFileFolderCnt for Policy Code " & sPolicyCode, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRefresh_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If
            End If

            ' Get the ClaimCnt from Claim Code (Claim Reference)
            If txtClaim.Enabled Then
                If txtClaim.Text.Trim().Length > 0 Then
                    sClaimCode = txtClaim.Text
                    bGetClaimCnt = True
                End If
            Else
                ' This is disabled. so we should have the claim file cnt
                If m_lClaimCnt <> 0 Then 'PN 58985
                    m_lClaimCntSearch = m_lClaimCnt
                Else
                    If m_bPassBaseClaimID Then
                        m_lBaseClaimCntSearch = m_lBaseClaimId
                    Else
                        m_lClaimCntSearch = m_lBaseClaimId
                    End If
                End If
                bGetClaimCnt = False
            End If

            If bGetClaimCnt Then
                sClaimCode = sClaimCode.Trim()

                m_lReturn = CType(GetClaimCntForClaimCode(v_sClaimCode:=sClaimCode, r_lClaimCnt:=m_lClaimCntSearch, _
                                            r_lBaseClaimCnt:=m_lBaseClaimCntSearch), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch ClaimCnt for Claim Code " & sClaimCode, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRefresh_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

            End If

            ' set to Null before searching

            m_dtFromDate = Nothing

            m_dtToDate = Nothing

            ' Check if we have a valid date
            sFromDate = txtFromDate.Text.Trim()
            If sFromDate.Length > 0 Then
                If Not Information.IsDate(sFromDate) Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    'Invalid From date


                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACInvalidFromDateTitle), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACBusinessFail), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    ' Display message.
                    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtFromDate.Focus()
                    Exit Sub
                Else
                    m_dtFromDate = CDate(sFromDate)
                End If
            End If

            sToDate = txtToDate.Text.Trim()
            If sToDate.Length > 0 Then
                If Not Information.IsDate(sToDate) Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    'Invalid To date


                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACInvalidToDateTitle), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=CInt(ACInvalidToDate), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    ' Display message.
                    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtToDate.Focus()
                    Exit Sub
                Else
                    m_dtToDate = CDate(sToDate)
                End If
            End If

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch events", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRefresh_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            ' Resets the Refresh flag
            m_bFromRefreshButton = False

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Refresh command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRefresh_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Function GetInsuranceFileFolderCntForPolicyCode(ByVal v_sPolicyCode As String, ByRef r_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object
        'Dim ACIAlternateReference, ACIInsuranceFileCnt, ACIInsuranceFolderCnt, ACIInsuranceFolderDesc As Byte

        Dim sComparePolicyCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vSearchData) Then
                ' Invalid PolicyCode
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Search Data Array is Empty", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFileFolderCntForPolicyCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            v_sPolicyCode = v_sPolicyCode.Trim()
            If v_sPolicyCode.Length = 0 Then

                ' Invalid PolicyCode
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Policy Code is Empty", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFileFolderCntForPolicyCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                sComparePolicyCode = CStr(m_vSearchData(ACIInsuranceFolderDesc, lRow)).Trim()

                If m_bUnderwritingBranchEnabled And m_bIsUnderwritingBranch And gPMFunctions.NullToString(m_vSearchData(ACIAlternateReference, lRow)) <> "" Then
                    sComparePolicyCode = CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim()
                End If

                If sComparePolicyCode = v_sPolicyCode Then
                    r_lInsuranceFileCnt = CInt(m_vSearchData(ACIInsuranceFileCnt, lRow))
                    r_lInsuranceFolderCnt = CInt(m_vSearchData(ACIInsuranceFolderCnt, lRow))
                    Exit For
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFileFolderCntForPolicyCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFileFolderCntForPolicyCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    Private Function GetClaimCntForClaimCode(ByVal v_sClaimCode As String, ByRef r_lClaimCnt As Integer, Optional ByRef r_lBaseClaimCnt As Long = 0) As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object
        'Dim ACIClaimCnt, ACIClaimDesc As Byte


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vSearchData) Then
                '        ' Invalid PolicyCode
                '        GetClaimCntForClaimCode = PMFalse
                '        ' Log Error.
                '        LogMessage _
                ''            iType:=PMLogOnError, _
                ''            sMsg:="Search Data Array is Empty", _
                ''            vApp:=ACApp, _
                ''            vClass:=ACClass, _
                ''            vMethod:="GetClaimCntForClaimCode", _
                ''            vErrNo:=Err.Number, _
                ''            vErrDesc:=Err.Description
                Return result
            End If

            v_sClaimCode = v_sClaimCode.Trim()
            If v_sClaimCode.Length = 0 Then
                ' Invalid PolicyCode
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Claim Code is Empty", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimCntForClaimCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If CStr(m_vSearchData(ACIClaimDesc, lRow)).Trim() = v_sClaimCode Then
                    r_lClaimCnt = CInt(m_vSearchData(ACIClaimCnt, lRow))
                    r_lBaseClaimCnt = m_vSearchData(ACIBaseClaimCnt, lRow)
                    Exit For
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimCntForClaimCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimCntForClaimCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : GetPolicies
    ' Description   : Function to fetch the policies (i.e. based on search criteria)
    ' Notes         : Bug fix for PN Issue 10592
    ' Edit History  :
    ' RAM20040224   : Created
    ' ***************************************************************** '
    Private Function GetPolicies() As Integer
        Dim result As Integer = 0
        'Dim ACApp, g_oObjectManager As Object

        Const PMKeyNamePartyCnt As String = "party_cnt"
        Const PMKeyNameShortName As String = "shortname"
        Const PMKeyNameInsReference As String = "insurance_ref"
        Const PMKeyNameRunMode As String = "Run_Mode"


        'developer guide no. 88
        Dim oFindPolicy As Object = Nothing
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Insurance object

            'developer guide no.108
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oFindPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface


            oFindPolicy.CallingAppName = ACApp

            ReDim vKeyArray(1, 3)
            ' Party Cnt

            vKeyArray(0, 0) = PMKeyNamePartyCnt

            vKeyArray(1, 0) = m_lPartyCnt

            ' Party ShortName

            vKeyArray(0, 1) = PMKeyNameShortName

            vKeyArray(1, 1) = m_sShortName

            ' InsuranceReference

            vKeyArray(0, 2) = PMKeyNameInsReference
            If txtPolicy.Text.Trim().Length = 0 Then

                vKeyArray(1, 2) = "%%%"
            Else

                vKeyArray(1, 2) = txtPolicy.Text
            End If

            ' PMKeyNameRunMode

            vKeyArray(0, 3) = PMKeyNameRunMode

            vKeyArray(1, 3) = "SearchByClientCode"


            m_lReturn = oFindPolicy.SetKeys(vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = oFindPolicy.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Check the status first

            If oFindPolicy.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                ' Retrieve the details back from the find policy component

                m_lInsuranceFileCntSearch = oFindPolicy.InsFileCnt

                m_lInsuranceFolderCntSearch = oFindPolicy.InsuranceFolderCnt

                ' Display Policy Reference on form

                txtPolicy.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=oFindPolicy.InsReference.Trim())

                m_lClaimCntSearch = 0
                txtClaim.Text = ""

            End If

            ' Destroy Find Insurance object

            oFindPolicy.Dispose()
            oFindPolicy = Nothing

            Return result

        Catch excep As System.Exception



            ' Destroy Find Insurance object
            If oFindPolicy Is Nothing Then
            Else

                oFindPolicy.Dispose()
                oFindPolicy = Nothing
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : GetClaims
    '
    ' Description   : Function to fetch the claims (i.e. based on search criteria)
    ' Notes         : Bug fix for PN Issue 10592
    ' Edit History  :
    ' RAM20040224   : Created
    ' ***************************************************************** '
    Private Function GetClaims() As Integer
        Dim result As Integer = 0
        'Dim ACApp, g_oObjectManager As Object
        'developer guide no. 88
        Dim oFindClaim As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Claim object

            'developer guide no.108
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oFindClaim, sClassName:="iCLMFindClaim.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iCLMFindClaim.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaims", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface


            oFindClaim.CallingAppName = ACApp
            'oFindClaim.Task = PMView

            oFindClaim.PolicyHolderCnt = m_lPartyCnt ' PartyCnt

            oFindClaim.PolicyHolder = m_sShortName ' PartyShortCode

            oFindClaim.PolicyRef = txtPolicy.Text ' Policy Ref

            oFindClaim.ClaimRef = txtClaim.Text ' Claim Ref

            oFindClaim.ClaimCnt = m_lClaimCnt ' Claim Cnt

            oFindClaim.CaseID = m_lCaseID


            m_lReturn = oFindClaim.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start object 'iCLMFindClaim.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaims", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Check the status first

            If oFindClaim.Status <> gPMConstants.PMEReturnCode.PMCancel Then


                m_lClaimCntSearch = oFindClaim.ClaimCnt

                m_lInsuranceFileCntSearch = oFindClaim.InsFileCnt
                m_lInsuranceFolderCntSearch = 0


                txtClaim.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=oFindClaim.ClaimRef.Trim())

                txtPolicy.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=oFindClaim.PolicyRef.Trim())
                txtPolicy.Text = IIf(txtPolicy.Text = "0", "", txtPolicy.Text)
            End If

            ' Destroy Find Insurance object

            oFindClaim.Dispose()
            oFindClaim = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Destroy Find Insurance object
            If oFindClaim Is Nothing Then
            Else

                oFindClaim.Dispose()
                oFindClaim = Nothing
            End If

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaims Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaims", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetPolicyDesc
    '
    ' Description:
    '
    ' History: 08/04/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetPolicyDesc(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sPolicyDesc As String) As Integer
        Dim result As Integer = 0
        'Dim ACApp As Object
        'Dim ACIAlternateReference, ACIInsuranceFileCnt As Byte

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lInsuranceFileCnt As Integer
            Dim sPolicyDesc As String = ""

            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            If Not m_bIsUnderwritingBranch Then
                Return result
            End If

            For lIndex As Integer = 0 To m_vSearchData.GetUpperBound(1)
                'SJ 21/04/2004 - start
                If CStr(m_vSearchData(ACIInsuranceFileCnt, lIndex)) <> "" Then
                    'SJ 21/04/2004 - end
                    lInsuranceFileCnt = gPMFunctions.NullToLong(m_vSearchData(ACIInsuranceFileCnt, lIndex))
                    If lInsuranceFileCnt = v_lInsuranceFileCnt Then
                        sPolicyDesc = gPMFunctions.NullToString(m_vSearchData(ACIAlternateReference, lIndex))
                        If sPolicyDesc <> "" Then
                            r_sPolicyDesc = sPolicyDesc
                        End If
                        Exit For
                    End If
                End If
            Next lIndex

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: PrintNotes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function PrintNotes() As Integer

        Dim result As Integer = 0


        Const kMethodName As String = "Print"

        Dim lReturn As Integer
        'Dim sRTFText As New StringBuilder
        Dim sRTFText As String = Nothing
        Dim lTag As Integer
        Dim bFirstRow As Boolean
        'developer guide no. 50
        objNotes = New frmNotesPreview
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            'lTag = lvwSearchDetails.ListItems(lvwSearchDetails.SelectedItem.Index).Tag

            For lIndex As Integer = 1 To lvwSearchDetails.Items.Count
                If lvwSearchDetails.Items.Item(lIndex - 1).Selected Then

                    lTag = Convert.ToString(lvwSearchDetails.Items.Item(lIndex - 1).Tag)
                    If bFirstRow Then
                        'sRTFText.Append(Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10))
                        sRTFText = sRTFText & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
                    End If
                    'sRTFText = sRTFText.ToString() & "[" & DateTime.Parse(gPMFunctions.ToSafeDate(m_vSearchData(ACIDate, lTag))).ToString("D") & StringsHelper.Format(gPMFunctions.ToSafeDate(m_vSearchData(ACIDate, lTag)), " hh:mm:ss AMPM") & "]" & "[" & (IIf(m_sCaseNumber = "", gPMFunctions.ToSafeString(m_vSearchData(ACICaseNumber, lTag)), m_sCaseNumber)) & "]" & "[" + CDbl(m_vSearchData(ACIClaimDesc, lTag)) + "]" + "[" + CDbl(m_vSearchData(ACIDescription, lTag)) + "]" + "[" + CDbl(m_vSearchData(ACIUserName, lTag)) + "]"
                    sRTFText = sRTFText & "[" & DateTime.Parse(gPMFunctions.ToSafeDate(m_vSearchData(ACIDate, lTag))).ToString("D") & StringsHelper.Format(gPMFunctions.ToSafeDate(m_vSearchData(ACIDate, lTag)), " hh:mm:ss AMPM") & "]" & "[" & (IIf(m_sCaseNumber = "", gPMFunctions.ToSafeString(m_vSearchData(ACICaseNumber, lTag)), m_sCaseNumber)) & "]" & "[" & (m_vSearchData(ACIClaimDesc, lTag)) & "]" & "[" & (m_vSearchData(ACIDescription, lTag)) & "]" & "[" & (m_vSearchData(ACIUserName, lTag)) & "]"

                    bFirstRow = True
                End If
            Next

            ' guide no. 50
            'With frmNotesPreview


            With objNotes
                '.RTFText = sRTFText.ToString()
                .RTFText = sRTFText
                .Task = m_iTask
                m_lReturn = CType(iPMFunc.SetWindowPlacement(.Handle.ToInt32(), True), gPMConstants.PMEReturnCode)
                .ShowDialog()
            End With


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: PrintAllNotes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function PrintAllNotes() As Integer
        Dim result As Integer = 0
        Dim ACICaseNumber As Integer


        Const kMethodName As String = "PrintAllNotes"

        Dim lReturn As Integer
        'Dim sRTFText As New StringBuilder
        Dim sRTFText As String = Nothing
        Dim lTag As Integer
        Dim bFirstRow As Boolean
        Dim sCaseNo As String = ""
        'developer guide no. 50
        objNotes = New frmNotesPreview
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            For lIndex As Integer = 1 To lvwSearchDetails.Items.Count


                lTag = Convert.ToString(lvwSearchDetails.Items.Item(lIndex - 1).Tag)

                If m_sCaseNumber.Trim().Length > 0 Then
                    sCaseNo = m_sCaseNumber
                ElseIf m_vSearchData.GetUpperBound(0) >= ACICaseNumber Then
                    sCaseNo = gPMFunctions.ToSafeString(m_vSearchData(ACICaseNumber, lTag))
                End If

                If bFirstRow Then
                    'sRTFText.Append(Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10))
                    sRTFText = sRTFText & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
                End If
                'sRTFText = New StringBuilder(sRTFText.ToString() & "[" & DateTime.Parse(gPMFunctions.ToSafeDate(m_vSearchData(ACIDate, lTag))).ToString("D") & StringsHelper.Format(gPMFunctions.ToSafeDate(m_vSearchData(ACIDate, lTag)), " hh:mm:ss AMPM") & "]" & "[" & sCaseNo & "]" & "[" + CDbl(m_vSearchData(ACIClaimDesc, lTag)) + "]" + "[" + CDbl(m_vSearchData(ACIDescription, lTag)) + "]" + "[" + CDbl(m_vSearchData(ACIUserName, lTag)) + "]")
                sRTFText = sRTFText & "[" & DateTime.Parse(gPMFunctions.ToSafeDate(m_vSearchData(ACIDate, lTag))).ToString("D") & StringsHelper.Format(gPMFunctions.ToSafeDate(m_vSearchData(ACIDate, lTag)), " hh:mm:ss AMPM") & "]" & "[" & sCaseNo & "]" & "[" & (m_vSearchData(ACIClaimDesc, lTag)) & "]" & "[" & (m_vSearchData(ACIDescription, lTag)) & "]" & "[" & (m_vSearchData(ACIUserName, lTag)) & "]"

                bFirstRow = True

            Next

            'developer guide no. 50
            'With frmNotesPreview


            With objNotes
                .RTFText = sRTFText.ToString()
                .Task = m_iTask
                m_lReturn = CType(iPMFunc.SetWindowPlacement(.Handle.ToInt32(), True), gPMConstants.PMEReturnCode)
                .ShowDialog()
            End With



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    Private Sub txtClaim_TextChanged(sender As Object, e As EventArgs) Handles txtClaim.TextChanged
        If txtClaim.Enabled = True And txtClaim.Text <> "" And cmdFindClaim.Enabled = True Then
            m_bPassBaseClaimID = True
        Else
            m_bPassBaseClaimID = False
        End If
    End Sub
End Class
