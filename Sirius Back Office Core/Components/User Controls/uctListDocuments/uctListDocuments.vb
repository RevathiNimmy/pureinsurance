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
Imports System.IO
Imports System.Windows.Forms
Imports Word = Microsoft.Office.Interop.Word
'Modified by Vijay Pal on 5/10/2010 6:03:05 PM refer developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctListDocuments_NET.uctListDocuments")> _
Partial Public Class uctListDocuments
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
    ' Date: 05/05/2000
    '
    ' Description: List Documents User Control
    '
    ' Edit History: Tomo05052000 - Lifted from List Policy
    ' CJB 01/04/05 PN13505 Removed the sorted property from listview as SQL does the sorting.
    ' CJB 04/04/05 PN19866 Enhancement to enable one doc desc to be entered when archiving > 1 doc.
    '              Also updated the statusbar during this process as it can take quite a while. All
    '              done in ArchiveClick.
    ' CJB 11/04/05 PN20005 Only allow one doc to be edited at once as when you close a word session.
    '              Changes done in EditClick & Timer1_Timer to support this.
    ' CJB 11/04/05 PN20004 Implement record locking for Print, Delete and Archive actions and fix
    '              it for Editing. Functions changed are ArchiveClick, DeleteClick, Timer1_Timer &
    '              PrintDocument. Also now show the name of any docs that are locked already (as many
    '              may be selected and we'll carry on with others).
    ' PW270905 - PN24093 - On CopyClientToServer use the DeleteFile function which
    '            now incorporates retrying. This is because there was a timing issue
    '            sometimes when pasting a lot of information into a document being
    '            edited from the spooler. A process can still be holding the client
    '            file locked when Sirius first tries to delete it.
    ' RKS 10/05/2006 Fixed PN28284 Not able to archive .DOC or other type of documents as PDF
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctListDocumentsControl"

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
    'Event lvwSearchDetailsDblClick(lInsHolderCnt As Long, _
    'lInsuranceFolderCnt As Long, _
    'lInsFileCnt As Long, _
    'sShortName As String, _
    'sInsReference As String, _
    'lPolicyTypeId As Long)
    Event lvwSearchDetailsClick(ByVal Sender As Object, ByVal e As lvwSearchDetailsClickEventArgs)
    Event lvwSearchDetailsItemClick(ByVal Sender As Object, ByVal e As lvwSearchDetailsItemClickEventArgs)

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
    Private m_sShortName As String = ""
    Private m_sLongName As String = ""
    Private m_sInsReference As String = ""
    Private m_lDocumentSpoolerId As Integer
    Private m_sDMEOption As String = ""
    'DC240603 -ISS4097 -added new parameter
    Private m_lSourceId As Integer

    Private m_vDocumentSpoolerIds(,) As Object
    Private m_lNumberSelected As Integer

    Private m_lSelectedItem As Integer

    Private m_sClient As String = ""
    Private m_sServer As String = ""
    Private m_sServerPath As String = ""

    Private m_lDateColumn1 As Integer
    Private m_lDateColumn2 As Integer

    'TN20010206
    Private m_sUnderwritingOrAgency As String = ""
    'AR20050315 - PN19493
    Private m_bPrintMode As Boolean

    ' {* USER DEFINED CODE (End) *}


    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRDocSpooler.Business
    Private m_oBusinessUser As Object

    ' Declare an instance of the Lock object.

    Private m_oPMLock As bPMLock.User

    ' Declare an instance of the Spooler OLE object.
    Private m_oSpoolerOLE As Object
    'Private m_oSpoolerOLE As iPMBSpoolerOLE.Interface

    Private m_oWord As Word.Application

    Private m_bCreatedWord As Boolean

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the
    ' function call.
    Private m_lReturn As Integer
    Private m_lItemsFound As gPMConstants.PMEFormatStyle
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object
    Public m_vSearchDataUser(,) As Object

    Private m_bIsInitialised As Boolean
    Private m_sZIP_DIRECTORY As String = ""
    Private m_sClientDocument As String = ""
    Private m_sClientHTML As String = ""
    Private m_bUnZipped As Boolean

    Private m_sWordVersion As String = ""
    Private m_lWordHwnd As Integer
    Private m_bAutoArchiveEnabled As Boolean
    Private m_lcountDocuments As Integer
    Private m_lSelItem As Integer
    'PN67015
    Private m_bComesViaPrintEvent As Boolean

    'Designed to get the no of documents in the list

    <Browsable(False)> _
    Public Property countDocuments() As Integer
        Get
            Return m_lcountDocuments
        End Get
        Set(ByVal Value As Integer)
            m_lcountDocuments = Value
        End Set
    End Property

    'To set the selected item of the list view
    'explicitly
    <Browsable(False)> _
    Public WriteOnly Property setSelectedItem() As Integer
        Set(ByVal Value As Integer)
            If lvwSearchDetails.Items.Count > 0 Then
                If lvwSearchDetails.Items.Count >= Value Then
                    lvwSearchDetails.Items.Item(Value - 1).Selected = True
                Else
                    lvwSearchDetails.Items.Item(lvwSearchDetails.Items.Count - 1).Selected = True
                End If
            End If
        End Set
    End Property


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
        End Set
    End Property

    <Browsable(True)> _
    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property ClaimCnt() As Integer
        Get
            Return m_lClaimCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
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
    Public Property InsReference() As String
        Get
            Return m_sInsReference
        End Get
        Set(ByVal Value As String)
            m_sInsReference = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property SourceId() As Integer
        Get
            Return m_lSourceId
        End Get
        Set(ByVal Value As Integer)
            m_lSourceId = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property AutoArchiveEnabled() As Boolean
        Set(ByVal Value As Boolean)
            m_bAutoArchiveEnabled = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property ComesViaPrintEvent() As Boolean
        Get
            Return m_bComesViaPrintEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bComesViaPrintEvent = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: GetDocuments
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Public Function GetDocuments() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.
            m_lReturn = GetBusiness()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details from the search data storage
            ' to the interface.
            'm_lReturn = DataToInterface()

            ' Check for errors
            'If (m_lReturn <> PMTrue) Then
            '    ' Failed to assign the details.
            '    GetDocuments = PMFalse
            '    Exit Function
            'End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Documents", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocuments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' Retrieves the details from the business object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBusiness() As Integer
        Dim nResult As Integer = 0
        Dim sStrUser As String = String.Empty
        Dim iIsClient As CheckState
        Dim iIsAgent As CheckState
        Dim iIsOffice As CheckState
        Dim nOrderingOption As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'JMK 19 Sep 02 add parameter to limit search to current user or selected user
            sStrUser = IIf((chkShowCurrent.CheckState = CheckState.Checked), g_oObjectManager.UserName.Trim(), cboUser.Text.Trim())

            'DC240603 -ISS4097 -added SourceId parameter
            iIsClient = chkClient.CheckState
            iIsAgent = chkAgent.CheckState
            iIsOffice = chkOffice.CheckState

            If optPrintDate.Checked Then
                ' Print Date
                nOrderingOption = 1
            ElseIf optPartyThenProductionOrder.Checked Then
                'Party then Production Order
                nOrderingOption = 2
            Else
                'Agent, Party then Production Order
                nOrderingOption = 3
            End If

            m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, _
                                              v_lClaimCnt:=m_lClaimCnt, v_sUser:=sStrUser, v_lSourceId:=m_lSourceId, _
                                              v_lAccountHandlerCnt:=VB6.GetItemData(cboAccountHandler, cboAccountHandler.SelectedIndex), _
                                              v_iIsClient:=iIsClient, v_iIsAgent:=iIsAgent, _
                                              v_iIsOffice:=iIsOffice, v_iOrderProductionOrder:=nOrderingOption, sYearSelected:=Trim(CboYear.SelectedItem))

            m_lReturn = DataToInterface()

            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.

                Case Else
                    ' Failed to get details.
                    nResult = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return nResult
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try

    End Function
    ''' <summary>
    ''' Updates all interface details from the search data storage.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DataToInterface() As Integer
        Dim bErr_DataToInterface As Boolean = False

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oListItem As ListViewItem
        Dim nEntity As Integer
        Dim sCaption As String = String.Empty
        Dim strUser As String = String.Empty
        'DC070706 PN29257 used a counter for index, as broking crashes when numbers are not in sync
        Dim iCnt As Integer
        Dim iCurrRow As Integer

        lvwSearchDetails.VirtualMode = False
        Const ACFindImage As String = "FindImages"

        Try
            bErr_DataToInterface = True

            lvwSearchDetails.BeginUpdate()

            ' Get the user we are after
            strUser = IIf((chkShowCurrent.CheckState = CheckState.Checked), g_oObjectManager.UserName.Trim(), cboUser.Text.Trim())

            ' Update the interface details.

            txtClientCode.Text = ShortName
            txtPolicy.Text = InsReference

            If lvwSearchDetails.SelectedItems.Count > 0 Then
                iCurrRow = lvwSearchDetails.SelectedItems(0).Index + 1
            End If

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            m_lItemsFound = gPMConstants.PMEFormatStyle.PMFormatString

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return nResult
            End If

            If m_lPartyCnt <> 0 Then
                nEntity = 1
            End If

            If m_lInsuranceFileCnt <> 0 Then
                nEntity = 2
            End If

            If m_lClaimCnt <> 0 Then
                nEntity = 3
            End If

            m_lDateColumn1 = ACDateColumn1 - nEntity
            m_lDateColumn2 = ACDateColumn2 - nEntity

            iCnt = 1
            lvwSearchDetails.Columns.Clear()

            Select Case nEntity
                Case 0

                    sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S0", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1


                    sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S1", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1

                    If m_sUnderwritingOrAgency = "U" Then

                        sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle11, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        lvwSearchDetails.Columns.Insert(iCnt - 1, "S2", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                        iCnt += 1
                    End If


                    sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S3", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1

                    'RWH(18/07/01) Show Column $ as 'Regarding' for UW.
                    If m_sUnderwritingOrAgency = "U" Then

                        sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    Else

                        sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    End If

                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S4", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1

                Case 1

                    sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S1", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1

                    If m_sUnderwritingOrAgency = "U" Then

                        sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle11, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        lvwSearchDetails.Columns.Insert(iCnt - 1, "S2", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                        iCnt += 1
                    End If


                    sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S3", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1
                    sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S4", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1

                Case 2

                    sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S2", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1
                    sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S3", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1
                Case 3
                    sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    lvwSearchDetails.Columns.Insert(iCnt - 1, "S3", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                    iCnt += 1

            End Select

            'RWH(18/07/01) Show Column 5 as 'Document Type' for UW.
            If m_sUnderwritingOrAgency = "U" Then

                sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            lvwSearchDetails.Columns.Insert(iCnt - 1, "S5", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
            iCnt += 1


            sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Insert(iCnt - 1, "S6", sCaption, CInt(VB6.TwipsToPixelsX(2500)))
            iCnt += 1


            sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Insert(iCnt - 1, "S7", sCaption, CInt(VB6.TwipsToPixelsX(2500)))
            iCnt += 1

            sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle9, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Insert(iCnt - 1, "S8", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
            iCnt += 1


            sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle10, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Insert(iCnt - 1, "S9", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
            iCnt += 1

            'DC260602 -Started
            If m_sUnderwritingOrAgency = "A" Then

                sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle11, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                lvwSearchDetails.Columns.Insert(iCnt - 1, "S10", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
                iCnt += 1
            End If

            'Account Handler

            sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle12, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            If m_sUnderwritingOrAgency = "A" Then
                lvwSearchDetails.Columns.Insert(iCnt - 1, "S11", sCaption, CInt(VB6.TwipsToPixelsX(1800)))

            Else
                lvwSearchDetails.Columns.Insert(iCnt - 1, "S10", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
            End If
            iCnt += 1

            'Production Order

            sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle13, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            If m_sUnderwritingOrAgency = "U" Then
                lvwSearchDetails.Columns.Insert(iCnt - 1, "S11", sCaption, CInt(VB6.TwipsToPixelsX(1800)))
            End If


            iCnt += 1
            'DC260602 -End
            strUser = strUser.Trim().ToLower()
            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                ' Only add documents for current users
                'DN 05/12/02 - Ensure both strings are of same format when comparing
                If strUser.Trim().ToLower() = CStr(m_vSearchData(ACDCreatedBy, lRow)).Trim().ToLower() Or strUser.Trim().ToLower() = CStr(m_vSearchData(ACDModifiedBy, lRow)).Trim().ToLower() Or strUser.Trim().ToLower() = "(all)" Then

                    ' If chkHidePrintedDocs is checked, then don't display if Printed > 0
                    If (chkHidePrintedDocs.CheckState = CheckState.Unchecked) OrElse (Conversion.Val(CStr(m_vSearchData(ACDTimesPrinted, lRow))) = 0) Then

                        ' Assign the details to the first column.
                        m_lItemsFound = CType(m_lItemsFound + 1, gPMConstants.PMEFormatStyle)

                        Select Case nEntity
                            Case 0
                                ' Column 1 party
                                ' If m_sUnderwritingOrAgency = "U" Then
                                If CStr(m_vSearchData(ACDDocumentTypeCode, lRow)) = "REPORT" Then
                                    oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACDDescription, lRow)), ACFindImage)
                                Else
                                    oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACDShortName, lRow)), ACFindImage)
                                End If

                                oListItem.SubItems.Add(CStr(m_vSearchData(ACDInsuranceReferenceUW, lRow)))
                                oListItem.SubItems.Add(CStr(m_vSearchData(ACDLeadAgent, lRow)))

                                'Column 4 Claim
                                oListItem.SubItems.Add(CStr(m_vSearchData(ACDClaim, lRow)))
                                oListItem.SubItems.Add(CStr(m_vSearchData(ACDDescription, lRow)))

                            Case 1
                                ' Column 1 policy

                                oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(21, lRow)), ACFindImage)

                                ' Assign details to the other columns
                                ' Column 2 Claim
                                oListItem.SubItems.Add(CStr(m_vSearchData(ACDClaim, lRow)))


                                ' Column 4 Description
                                'DC100203 -ISS1460 -start -was setting description as insurancefilecnt before, so keeping that for UW
                                '                   for BR setting to the description as expected
                                oListItem.SubItems.Add(CStr(m_vSearchData(ACDInsuranceFileCntUW, lRow)))
                            Case 2
                                ' Column 1 Claim
                                oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACDClaim, lRow)), ACFindImage)

                                ' Assign details to the other columns
                                ' Column 2 Description
                                'DC100203 -ISS1460 -start -was setting description as insurancefilecnt before, so keeping that for UW
                                '                   for BR setting to the description as expected
                                oListItem.SubItems.Add(CStr(m_vSearchData(ACDInsuranceFileCntUW, lRow)))
                            Case 3
                                ' Column 1 Description
                                'DC100203 -ISS1460 -start -was setting description as insurancefilecnt before, so keeping that for UW
                                '                   for BR setting to the description as expected
                                oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACDInsuranceFileCntUW, lRow)), ACFindImage)
                        End Select

                        'lvwSearchDetails.Items.AddRange(New ListViewItem() {oListItem})
                        ' Column 4 Document Type
                        If Convert.IsDBNull(m_vSearchData(ACDDocumentType, lRow)) OrElse IsNothing(m_vSearchData(ACDDocumentType, lRow)) Then
                            oListItem.SubItems.Add("")
                        Else
                            oListItem.SubItems.Add(CStr(m_vSearchData(ACDDocumentType, lRow)))
                        End If
                        ' Column 6 Created By
                        If Convert.IsDBNull(m_vSearchData(ACDCreatedBy, lRow)) OrElse IsNothing(m_vSearchData(ACDCreatedBy, lRow)) Then
                            oListItem.SubItems.Add("")
                        Else
                            oListItem.SubItems.Add(CStr(m_vSearchData(ACDCreatedBy, lRow)).Trim() & " - " & gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, vFieldValue:=CStr(m_vSearchData(ACDDateCreated, lRow))))
                        End If

                        ' Column 7 Modified By
                        If Convert.IsDBNull(m_vSearchData(ACDModifiedBy, lRow)) OrElse IsNothing(m_vSearchData(ACDModifiedBy, lRow)) Then
                            oListItem.SubItems.Add("")
                        Else
                            oListItem.SubItems.Add(CStr(m_vSearchData(ACDModifiedBy, lRow)).Trim() & " - " & gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, vFieldValue:=CStr(m_vSearchData(ACDDateModified, lRow))))
                        End If

                        ' Column 8 Times Printed
                        If Convert.IsDBNull(m_vSearchData(ACDTimesPrinted, lRow)) OrElse IsNothing(m_vSearchData(ACDTimesPrinted, lRow)) Then
                            oListItem.SubItems.Add(CStr(0))
                        Else
                            oListItem.SubItems.Add(CStr(m_vSearchData(ACDTimesPrinted, lRow)))
                        End If


                        ' Column 9 Times Archived
                        If Convert.IsDBNull(m_vSearchData(ACDTimesArchived, lRow)) OrElse IsNothing(m_vSearchData(ACDTimesArchived, lRow)) Then
                            oListItem.SubItems.Add(CStr(0))
                        Else
                            oListItem.SubItems.Add(CStr(m_vSearchData(ACDTimesArchived, lRow)))
                        End If
                        oListItem.SubItems.Add(CStr(m_vSearchData(ACDAccountHandler, lRow)))  'handler

                        'Column 11 Production Order
                        oListItem.SubItems.Add(CStr(m_vSearchData(ACDProductionOrder, lRow)))
                        ' Set the tag property with the index of
                        ' the search data storage.
                        oListItem.Tag = CStr(lRow)

                        ' Refresh the first X amount of rows, to
                        ' allow the user to see the results instantly.
                        If m_lItemsFound = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                            ' Select the first item.
                            lvwSearchDetails.Items.Item(0).Selected = True
                        End If
                    End If
                    ' RG End
                End If
            Next lRow

            countDocuments = lvwSearchDetails.Items.Count
            If countDocuments >= iCurrRow And iCurrRow > 0 Then
                lvwSearchDetails.Items.Item(iCurrRow - 1).Selected = True
            End If

            ' Enable the interface now that the search
            ' has completed.

            m_lReturn = DisableInterface(bDisable:=False)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            If Not bErr_DataToInterface Then
                Throw excep
            End If
            If bErr_DataToInterface Then

                nResult = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return nResult

            End If
        Finally
            lvwSearchDetails.EndUpdate()
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data storage.
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lTemp2 As Integer
        Dim sContactType As String = String.Empty
        Dim sErrorMessage As String = String.Empty


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vDocumentSpoolerIds = Nothing
            m_lNumberSelected = 0

            If lvwSearchDetails.Items.Count = 0 Then
                Return result
            End If

            For lTemp As Integer = 1 To lvwSearchDetails.Items.Count
                ' Check if this item is selected
                If lvwSearchDetails.Items.Item(lTemp - 1).Selected Then

                    ' Get array index for selected item

                    m_lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lTemp - 1).Tag)
                    If CInt(m_vSearchData(ACDTimesArchived, m_lSelectedItem)) > 0 And ComesViaPrintEvent Then
                        'Do Nothing when comes via Print Event and times_archived > 0 -ie do not Archive again
                    Else
                        ' Create or expand array
                        If Information.IsArray(m_vDocumentSpoolerIds) Then
                            lTemp2 = m_vDocumentSpoolerIds.GetUpperBound(1) + 1
                            ReDim Preserve m_vDocumentSpoolerIds(ACDSArraySize, lTemp2)
                        Else
                            lTemp2 = 0
                            ReDim m_vDocumentSpoolerIds(ACDSArraySize, lTemp2)
                        End If

                        ' Copy array elements
                        m_vDocumentSpoolerIds(ACDSDocumentSpoolerId, lTemp2) = CInt(m_vSearchData(ACDDocumentSpoolerId, m_lSelectedItem))
                        m_vDocumentSpoolerIds(ACDSPartyCnt, lTemp2) = m_vSearchData(ACDPartyCnt, m_lSelectedItem)
                        m_vDocumentSpoolerIds(ACDSInsuranceFolderCnt, lTemp2) = m_vSearchData(ACDInsuranceFolderCnt, m_lSelectedItem)

                        If m_sUnderwritingOrAgency = "U" Then
                            m_vDocumentSpoolerIds(ACDSInsuranceFileCnt, lTemp2) = m_vSearchData(ACDInsuranceFileCntUW, m_lSelectedItem)
                        Else
                            m_vDocumentSpoolerIds(ACDSInsuranceFileCnt, lTemp2) = m_vSearchData(ACDInsuranceFileCntBR, m_lSelectedItem)
                        End If

                        m_vDocumentSpoolerIds(ACDSClaimCnt, lTemp2) = m_vSearchData(ACDClaimCnt, m_lSelectedItem)
                        m_vDocumentSpoolerIds(ACDSDocumentTypeId, lTemp2) = CInt(m_vSearchData(ACDDocumentTypeId, m_lSelectedItem))
                        m_vDocumentSpoolerIds(ACDSSpoolLevelInd, lTemp2) = Conversion.Val(CStr(m_vSearchData(ACDSpoolLevelInd, m_lSelectedItem)))
                        If m_sUnderwritingOrAgency <> "U" Then
                            m_vDocumentSpoolerIds(ACDSTemplateCode, lTemp2) = m_vSearchData(ACDTemplateCode, m_lSelectedItem)
                        End If

                        m_vDocumentSpoolerIds(ACDSTimesPrinted, lTemp2) = m_vSearchData(ACDTimesPrinted, m_lSelectedItem)
                        m_vDocumentSpoolerIds(ACDSTimesArchived, lTemp2) = m_vSearchData(ACDTimesArchived, m_lSelectedItem)
                        m_vDocumentSpoolerIds(ACDSCreatedBy, lTemp2) = m_vSearchData(ACDCreatedBy, m_lSelectedItem)
                        m_vDocumentSpoolerIds(ACDSDateCreated, lTemp2) = m_vSearchData(ACDDateCreated, m_lSelectedItem)
                        'PN:73866
                        m_vDocumentSpoolerIds(ACDSDocumentTemplateID, lTemp2) = m_vSearchData(ACDDocumentTemplateID, m_lSelectedItem)

                        m_vDocumentSpoolerIds(ACDSDocumentTemplateGroupID, lTemp2) = m_vSearchData(ACDDDocumentTemplateGroupID, m_lSelectedItem)
                        m_vDocumentSpoolerIds(ACDSDocumentTemplateSubGroupID, lTemp2) = m_vSearchData(ACDDDocumentTemplateSubGroupID, m_lSelectedItem)
                        m_vDocumentSpoolerIds(ACDSInternalOnly, lTemp2) = m_vSearchData(ACDDInternalOnly, m_lSelectedItem)


                        If m_sUnderwritingOrAgency = "U" Then
                            m_vDocumentSpoolerIds(ACDSDocumentPrinter, lTemp2) = m_vSearchData(ACDDocumentPrinter, m_lSelectedItem)

                            If Strings.Len(CStr(m_vSearchData(ACDPartyCnt, m_lSelectedItem))) Then
                                ' Get the preferred method of contact for that client

                                m_lReturn = m_oBusiness.Party.GetPreferredContact(v_lPartyCnt:=CInt(m_vSearchData(ACDPartyCnt, m_lSelectedItem)), r_sContactType:=sContactType, r_sErrorMessage:=sErrorMessage)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse

                                    ' Log Error Message
                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=IIf(sErrorMessage.Length, sErrorMessage, "DataToProperties Failed on m_oBusiness.Party.GetPreferredContact"), vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                    Return result
                                End If

                                ' Store the contact type
                                m_vDocumentSpoolerIds(ACDSContact, lTemp2) = sContactType
                            Else
                                m_vDocumentSpoolerIds(ACDSContact, lTemp2) = gSIRLibrary.SIRMainContactCode
                            End If
                        Else
                            If m_bPrintMode Then
                                ' Get the preferred method of contact for that client

                                m_lReturn = m_oBusiness.Party.GetPreferredContact(v_lPartyCnt:=CInt(m_vSearchData(ACDPartyCnt, m_lSelectedItem)), r_sContactType:=sContactType, r_sErrorMessage:=sErrorMessage)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    If sErrorMessage.Length > 0 Then
                                        'DJM 22/08/2003 : Only error if contact type is E-Mail, faxes can still be printed/archived.
                                        'AR20050315 - PN19493 If printing, display error for email and fax if no such contact has been set up
                                        If sContactType = gSIRLibrary.SIREmailContactCode Or sContactType = gSIRLibrary.SIRFaxConactCode Then
                                            result = gPMConstants.PMEReturnCode.PMFalse
                                            MessageBox.Show(sErrorMessage, "Document Spooler", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        End If
                                    Else
                                        result = gPMConstants.PMEReturnCode.PMFalse
                                        ' Log Error Message
                                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToProperties Failed on m_oBusiness.Party.GetPreferredContact", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                        Return result
                                    End If
                                End If

                                ' Store the contact type
                                m_vDocumentSpoolerIds(ACDSContact, lTemp2) = sContactType
                            Else
                                'AR20050315 - PN19493 Default setting
                                m_vDocumentSpoolerIds(ACDSContact, lTemp2) = gSIRLibrary.SIRMainContactCode
                            End If
                        End If

                        m_vDocumentSpoolerIds(ACDSDescription, lTemp2) = m_vSearchData(ACDDescription, m_lSelectedItem)
                    End If
                End If
            Next lTemp

            If Information.IsArray(m_vDocumentSpoolerIds) Then
                m_lNumberSelected = m_vDocumentSpoolerIds.GetUpperBound(1) + 1
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ArchiveClick
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function ArchiveClick() As Integer

        Dim result As Integer = 0
        Dim lDocumentSpoolerId, lPartyCnt, lInsuranceFolderCnt, lInsuranceFileCnt, lClaimCnt, lDocumentTypeId As Integer
        Dim sDocumentDescription As String = ""
        Dim lSpoolLevelInd As Integer
        Dim iResponse As DialogResult
        Dim bOneArchiveDescription As Boolean
        Dim sDescription As String = ""
        Dim bUnzipped As Boolean
        Dim DocumentTemplateID As Integer

        Dim lDocumentTemplateGroupID As Integer
        Dim lDocumentTemplateSubGroupID As Integer
        Dim bInternalOnly As Integer

        Dim sOptionValue As String = ""
        Dim iOptionNumber As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Busy Mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_bPrintMode = False
            m_lReturn = DataToProperties()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lNumberSelected = 0 Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' For Broking: If > 1 doc selected then ask the user if they want to enter an archive reason in
            ' that will apply to all (as opposed to one reason per doc)  PN19866

            If m_oBusiness.UnderwritingOrAgency <> "U" Then
                If m_lNumberSelected > 1 Then
                    iResponse = MessageBox.Show("Do you want to enter ONE archive description that will be applied to ALL of the " & m_lNumberSelected & " documents you have selected?" & Strings.Chr(13) & Strings.Chr(10) & "(Click 'No' if you want to enter a different description for each document).", "Archive Spooled Documents", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

                    If iResponse = System.Windows.Forms.DialogResult.Yes Then
                        bOneArchiveDescription = True
                    ElseIf iResponse = System.Windows.Forms.DialogResult.No Then
                        bOneArchiveDescription = False
                    ElseIf iResponse = System.Windows.Forms.DialogResult.Cancel Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Return result
                    End If
                End If
            End If

            For lTemp As Integer = m_vDocumentSpoolerIds.GetLowerBound(1) To m_vDocumentSpoolerIds.GetUpperBound(1)
                ' If this is S4I and auto-archive is enabled only archive this document once per edit
                ' Or, logically we archive this document if:
                '   - We are not S4I
                '   - Auto archive is not enabled
                '   - We are S4I and auto archive is enabled but our archive count is Zero

                If (m_oBusiness.UnderwritingOrAgency <> "U") Or (Not m_bAutoArchiveEnabled) Or (CDbl(m_vDocumentSpoolerIds(ACDSTimesArchived, lTemp)) = 0) Then
                    'developer guide no.168
                    _stbStatus_Panel1.Text = " " & "Archiving document " & CStr(lTemp + 1) & " of " & CStr(m_lNumberSelected) & "..." 'PN19866
                    Application.DoEvents()
                    lDocumentSpoolerId = CInt(m_vDocumentSpoolerIds(ACDSDocumentSpoolerId, lTemp))
                    sDescription = "'" & CStr(m_vDocumentSpoolerIds(ACDSDescription, lTemp)) & "' created by " & CStr(m_vDocumentSpoolerIds(ACDSCreatedBy, lTemp)) & " on " & gPMFunctions.ToSafeString(CStr(m_vDocumentSpoolerIds(ACDSDateCreated, lTemp)), "unknown")

                    ' Check doc hasn't already been locked before we archive it! Also, lock it at the start
                    ' of this process and unlock after   PN20004
                    m_lReturn = LockDocument(lDocumentSpoolerId:=lDocumentSpoolerId, v_sDocumentDescription:=sDescription)

                    ' Carry on if locked ok but if record in use just skip to next (if there is one), else exit
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMMAlreadyInUse Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Get properties
                        lPartyCnt = gPMFunctions.ToSafeLong(m_vDocumentSpoolerIds(ACDSPartyCnt, lTemp))
                        lInsuranceFolderCnt = gPMFunctions.ToSafeLong(m_vDocumentSpoolerIds(ACDSInsuranceFolderCnt, lTemp))
                        lInsuranceFileCnt = gPMFunctions.ToSafeLong(m_vDocumentSpoolerIds(ACDSInsuranceFileCnt, lTemp))
                        lClaimCnt = gPMFunctions.ToSafeLong(m_vDocumentSpoolerIds(ACDSClaimCnt, lTemp))
                        lDocumentTypeId = gPMFunctions.ToSafeInteger(m_vDocumentSpoolerIds(ACDSDocumentTypeId, lTemp))
                        lSpoolLevelInd = gPMFunctions.ToSafeInteger(m_vDocumentSpoolerIds(ACDSSpoolLevelInd, lTemp))
                        DocumentTemplateID = gPMFunctions.ToSafeInteger(m_vDocumentSpoolerIds(ACDSDocumentTemplateID, lTemp))

                        lDocumentTemplateGroupID = gPMFunctions.ToSafeInteger(m_vDocumentSpoolerIds(ACDSDocumentTemplateGroupID, lTemp))
                        lDocumentTemplateSubGroupID = gPMFunctions.ToSafeInteger(m_vDocumentSpoolerIds(ACDSDocumentTemplateSubGroupID, lTemp))
                        bInternalOnly = gPMFunctions.ToSafeInteger(m_vDocumentSpoolerIds(ACDSInternalOnly, lTemp)) = 1


                        If m_oBusiness.UnderwritingOrAgency <> "U" Then
                            ' If entering just one reason for all selected docs then just get for the first   PN19866
                            If bOneArchiveDescription And lTemp = 0 Then
                                sDocumentDescription = Interaction.InputBox("Description for all documents?", "Document Description", sDocumentDescription)
                            ElseIf Not bOneArchiveDescription Then
                                ' Pass in Document name
                                sDocumentDescription = CStr(m_vDocumentSpoolerIds(ACDSDescription, lTemp))
                                sDocumentDescription = Interaction.InputBox("Description for document?", "Document Description", sDocumentDescription)
                            End If
                        Else
                            sDocumentDescription = CStr(m_vDocumentSpoolerIds(ACDSDescription, lTemp))
                        End If

                        ' Skip archiving if client, policy, and claim are all zero
                        If lPartyCnt = 0 AndAlso lInsuranceFolderCnt = 0 AndAlso lInsuranceFileCnt = 0 AndAlso lClaimCnt = 0 Then
                            m_lReturn = UnlockDocument(lDocumentSpoolerId:=lDocumentSpoolerId)
                            Continue For
                        End If

                        iOptionNumber = 10 ' possibly use a set of constants?
                        m_lReturn = m_oBusiness.getOption(v_iOptionNumber:=iOptionNumber, r_sOptionValue:=sOptionValue)

                        ' Archive the document
                        If m_sDMEOption = "1" Then
                            m_lReturn = ArchiveDocument(lDocumentSpoolerId:=lDocumentSpoolerId, lPartyCnt:=lPartyCnt, lInsuranceFolderCnt:=lInsuranceFolderCnt, lInsuranceFileCnt:=lInsuranceFileCnt, lClaimCnt:=lClaimCnt, lDocumentTypeId:=lDocumentTypeId, sDocumentDescription:=sDocumentDescription, lSpoolLevelInd:=lSpoolLevelInd, bUnzipped:=bUnzipped, DocumentTemplateID:=DocumentTemplateID)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                ' Unlock doc.
                                m_lReturn = UnlockDocument(lDocumentSpoolerId:=lDocumentSpoolerId)
                                Return result
                            End If

                        ElseIf sOptionValue = "2" Then
                            m_lReturn = SendToSharePoint(lDocumentSpoolerId:=lDocumentSpoolerId, lPartyCnt:=lPartyCnt, lInsuranceFolderCnt:=lInsuranceFolderCnt, lInsuranceFileCnt:=lInsuranceFileCnt, lClaimCnt:=lClaimCnt, lDocumentTypeId:=lDocumentTypeId, sDocumentDescription:=sDocumentDescription, lSpoolLevelInd:=lSpoolLevelInd, bUnzipped:=bUnzipped, DocumentTemplateID:=DocumentTemplateID, DocumentTemplateGroupID:=lDocumentTemplateGroupID, DocumentTemplateSubGroupID:=lDocumentTemplateSubGroupID, bInternalOnly:=bInternalOnly)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                ' Unlock doc.
                                m_lReturn = UnlockDocument(lDocumentSpoolerId:=lDocumentSpoolerId)
                                Return result
                            End If
                        End If


                        ' Unlock doc.
                        m_lReturn = UnlockDocument(lDocumentSpoolerId:=lDocumentSpoolerId)
                    End If
                End If
            Next lTemp

            ' Busy Normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ArchiveClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        End Try
    End Function


    Private Function SendToSharePoint(ByRef lDocumentSpoolerId As Integer, ByRef lPartyCnt As Integer, ByRef lInsuranceFolderCnt As Integer, ByRef lInsuranceFileCnt As Integer, ByRef lClaimCnt As Integer, ByRef lDocumentTypeId As Integer, ByRef sDocumentDescription As String, ByRef lSpoolLevelInd As Integer, ByVal bUnzipped As Boolean, Optional ByRef DocumentTemplateID As Integer = 0, Optional ByRef DocumentTemplateGroupID As Integer = 0, Optional ByRef DocumentTemplateSubGroupID As Integer = 0, Optional ByRef bInternalOnly As Boolean = False) As Integer
        Dim vInsuranceFolderCnt As Integer
        Dim vInsuranceFileCnt As Integer
        Dim vClaimCnt As Integer
        Dim lEventCnt As Integer

        Dim result As Integer = 0
        Dim sActualFileName As String = ""
        Dim sDocType As String = ""
        Dim sPageType As String = ""
        Dim lDocNumber As Integer
        Dim sOptionValue As String = ""
        Dim vIndexArray As Object = Nothing
        Dim sArchiveDoc As String = ""
        Dim sArchiveAsPDF As String = ""
        Dim sDocName As String = ""
        Dim sExtension As String
        Const ARCHIVE_AS_PDF As Integer = 5009

        Dim bHTMLFormat As Boolean
        Dim m_oSharePoint As bSIRSharepoint.Business
        Dim sStr() As String = New String() {}
        Dim m_sUsername As String = ""
        Dim bIsDigitalSignature As Boolean
        Dim sDigitalSignature As String = ""
        Dim DIGITAL_SIGNATURE As Integer
        Dim m_sMergedFilePath As String

        Try

            If m_sDMEOption = "1" Or m_sDMEOption = "2" Then

                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=ARCHIVE_AS_PDF, r_sOptionValue:=sArchiveAsPDF, v_iSourceID:=g_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                Return result
            End If

            m_lReturn = CopyServerToExport(lDocumentSpoolerId:=lDocumentSpoolerId, lSpoolLevelInd:=lSpoolLevelInd, bUnzipped:=bUnzipped, bHTMLFormat:=bHTMLFormat, sActualFileName:=sActualFileName)

            'DC260401 check that everythings okay here
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sStr = sActualFileName.Split(".")
            sExtension = IO.Path.GetExtension(sActualFileName).ToUpper

            If sArchiveAsPDF = "1" Then

                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=DIGITAL_SIGNATURE, r_sOptionValue:=sDigitalSignature, v_iSourceID:=g_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    bIsDigitalSignature = gPMFunctions.ToSafeBoolean(sDigitalSignature, False)
                End If

                sDocType = "F"
                sPageType = "PDF"

                If sExtension = ".DOC" Or sExtension = ".DOCX" Or sExtension = ".XML" Then
                    sArchiveDoc = m_sClient & "\" & sStr(0) & ".PDF"
                    m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(v_sSourceDocument:=m_sClient & "\" & sActualFileName, v_sDestDocument:=sArchiveDoc)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" Failed to convert the following document to PDF : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return m_lReturn
                    End If
                    m_sMergedFilePath = sArchiveDoc
                End If
            Else
                sArchiveDoc = m_sClient & "\" & sActualFileName
                sDocType = "D"
                sPageType = "DOC"

                If sExtension = ".DOC" Or sExtension = ".XML" Then
                    sPageType = "DOCX"
                    sArchiveDoc = m_sClient & "\" & sStr(0) & ".DOCX"
                    m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(v_sSourceDocument:=m_sClient & "\" & sActualFileName, v_sDestDocument:=sArchiveDoc)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" Failed to convert the following document to PDF : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return m_lReturn
                    End If
                    m_sMergedFilePath = sArchiveDoc
                End If

            End If


            'Sharepoint Integration
            If m_oSharePoint Is Nothing Then
                m_oSharePoint = New bSIRSharepoint.Business()

                m_lReturn = CType(m_oSharePoint, SSP.S4I.Interfaces.IBusiness).Initialise(MainModule.g_oObjectManager.UserName, MainModule.g_oObjectManager.Password, MainModule.g_oObjectManager.UserID, MainModule.g_iSourceID, MainModule.g_iLanguageID, MainModule.g_oObjectManager.CurrencyID, MainModule.g_oObjectManager.LogLevel, m_sCallingAppName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the m_oSharePoint object", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oSharePoint.ArchiveDocument(PartyCnt:=lPartyCnt, InsuranceFileCnt:=lInsuranceFileCnt, ClaimID:=lClaimCnt, CaseID:=0, DocumentTemplateID:=DocumentTemplateID, _
                                          TemplateGroupID:=DocumentTemplateGroupID, _
                                          TemplateSubGroupID:=DocumentTemplateSubGroupID, _
                                          SourceFile:=sArchiveDoc, InternalOnly:=bInternalOnly, SharepointPath:=sArchiveDoc, _
                                          DestinationFilename:=sDocumentDescription & "." & sPageType, _
                                          PartyCode:="", PolicyNumber:="", _
                                          ClaimNumber:="")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ArchiveDocument the SharePoint object", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            If lPartyCnt <> 0 Then
                If lInsuranceFolderCnt = 0 Then

                    vInsuranceFolderCnt = Nothing
                Else
                    vInsuranceFolderCnt = lInsuranceFolderCnt
                End If

                If lInsuranceFileCnt = 0 Then

                    vInsuranceFileCnt = Nothing
                Else
                    vInsuranceFileCnt = lInsuranceFileCnt
                End If

                If lClaimCnt = 0 Then

                    vClaimCnt = Nothing
                Else
                    vClaimCnt = lClaimCnt
                End If

                'Add the created event
                'MKW 070403 Pass Document Description to create event


                m_lReturn = m_oBusiness.CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=vInsuranceFolderCnt, v_vInsuranceFileCnt:=vInsuranceFileCnt, v_vClaimCnt:=vClaimCnt, v_vDocumentCnt:=lDocNumber, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=lDocumentTypeId, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventDocument, v_dtEventDate:=DateTime.Today, v_vDescription:=sDocumentDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oBusiness.UpdateArchived(lDocumentSpoolerId:=lDocumentSpoolerId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Archive Document", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Public Function CancelClick() As Integer

        ' Click event of the Cancel button.

        Try


            Return CancelListDocuments()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteClick
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteClick() As Integer

        Dim result As Integer = 0
        Dim lDocumentSpoolerId, lSpoolLevelInd, lTimesPrinted As Integer 'AR20050304 - PN15228
        Dim bDelete As Boolean 'AR20050304 - PN15228
        Dim sDescription As String = "" 'AR20050304 - PN15228
        Dim lResponse As DialogResult 'AR20050304 - PN15228

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AR20050315 - PN19493 - Set Print flag
            m_bPrintMode = False
            m_lReturn = DataToProperties()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lNumberSelected = 0 Then
                Return result
            End If

            For lTemp As Integer = m_vDocumentSpoolerIds.GetLowerBound(1) To m_vDocumentSpoolerIds.GetUpperBound(1)
                lDocumentSpoolerId = CInt(m_vDocumentSpoolerIds(ACDSDocumentSpoolerId, lTemp))
                'sj 23/10/2002 - start
                lSpoolLevelInd = CInt(m_vDocumentSpoolerIds(ACDSSpoolLevelInd, lTemp))
                'sj 23/10/2002 - end

                'AR20050304 - PN15228 Get the number of times spooled document has been printed
                lTimesPrinted = gPMFunctions.ToSafeLong(CStr(m_vDocumentSpoolerIds(ACDSTimesPrinted, lTemp)), 0)

                sDescription = "'" & CStr(m_vDocumentSpoolerIds(ACDSDescription, lTemp)) & "' created by " & CStr(m_vDocumentSpoolerIds(ACDSCreatedBy, lTemp)) & " on " & gPMFunctions.ToSafeString(CStr(m_vDocumentSpoolerIds(ACDSDateCreated, lTemp)), "unknown")

                If lTimesPrinted = 0 Then
                    'AR20050304 - PN15228 Ask user if they wish to delete unprinted document
                    lResponse = MessageBox.Show("The document " & sDescription & " has not been printed." & Strings.Chr(13) & Strings.Chr(10) & "Are you sure you want to delete this document?", "Delete Spooled Documents", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

                    If lResponse = System.Windows.Forms.DialogResult.Yes Then
                        bDelete = True
                    ElseIf lResponse = System.Windows.Forms.DialogResult.No Then
                        bDelete = False
                    ElseIf lResponse = System.Windows.Forms.DialogResult.Cancel Then
                        Exit For
                    End If
                Else
                    bDelete = True
                End If

                If bDelete Then

                    ' Check doc hasn't already been locked before we delete it! Also, lock it at the start
                    ' of this process and unlock after as two users may try to delete the same doc(s)  PN20004
                    m_lReturn = LockDocument(lDocumentSpoolerId:=lDocumentSpoolerId, v_sDocumentDescription:=sDescription)

                    ' Carry on if locked ok but if record in use just skip to next (if there is one), else exit
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMMAlreadyInUse Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else

                        m_lReturn = DeleteDocument(lDocumentSpoolerId:=lDocumentSpoolerId, lSpoolLevelInd:=lSpoolLevelInd)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Unlock doc.  PN20004
                            m_lReturn = UnlockDocument(lDocumentSpoolerId:=lDocumentSpoolerId)
                            Return result
                        End If

                        ' Unlock doc.  PN20004
                        m_lReturn = UnlockDocument(lDocumentSpoolerId:=lDocumentSpoolerId)
                    End If

                End If

            Next lTemp

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
    ' Name: EditClick
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function EditClick() As Integer

        Dim result As Integer = 0
        Dim lDocumentSpoolerId As Integer
        'sj 15/10/2002 - start
        Dim lSpoolLevelInd As Integer
        'sj 15/10/2002 - end
        Dim sDescription As String = ""

        ' PW250403 (Document Issuance changes)
        Dim bUnzipped As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Only allow one doc to be edited at once as when you close a word session
            ' all will be closed and changes may be lost. We check m_lDocumentSpoolerId as
            ' it'll only have a value if a word session is loaded...   PN20005
            If m_lDocumentSpoolerId > 0 Then
                MessageBox.Show("You may only have one Word editing session open at a time.", "Document Spooler", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            ' Busy Mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'AR20050315 - PN19493 - Set Print flag
            m_bPrintMode = False
            m_lReturn = DataToProperties()

            ' Busy Normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If m_lNumberSelected = 0 Then
                Return result
            End If

            If m_lNumberSelected > 1 Then
                MessageBox.Show("You may only edit one document at a time.", "Document Spooler", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            For lTemp As Integer = m_vDocumentSpoolerIds.GetLowerBound(1) To m_vDocumentSpoolerIds.GetUpperBound(1)
                lDocumentSpoolerId = CInt(m_vDocumentSpoolerIds(ACDSDocumentSpoolerId, lTemp))
                'sj 15/10/2002 - start
                lSpoolLevelInd = CInt(m_vDocumentSpoolerIds(ACDSSpoolLevelInd, lTemp))
                'sj 15/10/2002 - end

                ' PW250403 - Get unzipped flag (Document Issuance changes)
                ' TB070503 - Tinny Branched the wrong version for me
                'bUnzipped = (Val(m_vDocumentSpoolerIds(ACDSUnzipped, lTemp)) = 1)

                sDescription = "'" & CStr(m_vDocumentSpoolerIds(ACDSDescription, lTemp)) & "' created by " & CStr(m_vDocumentSpoolerIds(ACDSCreatedBy, lTemp)) & " on " & gPMFunctions.ToSafeString(CStr(m_vDocumentSpoolerIds(ACDSDateCreated, lTemp)), "unknown")

                ' Note that we now unlock when the word session has closed - done in Timer1_Timer  PN20004
                m_lReturn = LockDocument(lDocumentSpoolerId:=lDocumentSpoolerId, v_sDocumentDescription:=sDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' PW250403 - add unzipped flag (Document Issuance changes)
                m_lReturn = EditDocument(lDocumentSpoolerId:=lDocumentSpoolerId, lSpoolLevelInd:=lSpoolLevelInd, bUnzipped:=bUnzipped)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: MailClick
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function MailClick() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = DataToProperties()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AR20050315 - PN19493 - Set Print flag
            m_bPrintMode = False
            m_lReturn = MailDocument()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MailClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MailClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        ' Fire up the help screen
        'Modified by Vijay Pal on 5/11/2010 1:03:05 PM comment the line todolist
        'Return SSfunc.ShowHelp(dlgHelp, ScreenHelpID)

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


            Return SelectListDocuments()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PrintClick
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function PrintClick() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Busy Mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'AR20050315 - PN19493 - Set Print flag
            m_bPrintMode = True
            m_lReturn = DataToProperties()

            ' Busy Mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = PrintDocument()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 030800
            ' Now send any emails
            m_lReturn = EmailDocument()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Refresh
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Shadows Function Refresh() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    If (m_lNumberSelected = 0) Then
            '        'We've done nothing...
            '        'Though perhaps we should refresh anyway in case someone else has?
            '        Exit Function
            '    End If

            m_lReturn = GetDocuments()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Refresh Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Refresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim sTitle, sMessage As String

        Dim sHelpFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim bFound As Boolean
        Dim vUsers As Object = Nothing
        Dim sTemp As String = ""

        'Thinh Nguyen 25/03/2003
        Dim lSelectItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If m_bIsInitialised Then
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

            bPMDocFunctions.Username = g_oObjectManager.UserName

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'Modified by Vijay Pal on 5/11/2010 1:03:05 PM comment the line todolist
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRDocSpooler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

            m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Set default values for user
            cboUser.SelectedIndex = 0
            chkShowCurrent.CheckState = CheckState.Checked

            If cboUser.Items.Count <= 0 Then
                Dim cboUser_NewIndex As Integer = -1
                cboUser_NewIndex = cboUser.Items.Add("(All)")
            End If

            m_lReturn = m_oBusiness.GetUsers(r_vResultArray:=m_vSearchDataUser)

            ' Get all users
            'RWH(17/07/01) Protect against empty array.
            If Information.IsArray(m_vSearchDataUser) Then
                For lRow As Integer = m_vSearchDataUser.GetLowerBound(1) To m_vSearchDataUser.GetUpperBound(1)
                    If CStr(m_vSearchDataUser(0, lRow)) <> "" Then
                        If Not Information.IsArray(vUsers) Then
                            ReDim vUsers(0)

                            vUsers(0) = m_vSearchDataUser(0, lRow)
                        Else
                            bFound = False

                            For lRow2 As Integer = vUsers.GetLowerBound(0) To vUsers.GetUpperBound(0)

                                If m_vSearchDataUser(0, lRow).Equals(vUsers(lRow2)) Then
                                    bFound = True
                                    Exit For
                                End If
                            Next lRow2

                            If Not bFound Then

                                ReDim Preserve vUsers(vUsers.GetUpperBound(0) + 1)


                                vUsers(vUsers.GetUpperBound(0)) = m_vSearchDataUser(0, lRow)
                            End If
                        End If
                    End If
                Next lRow
            End If

            ' Sort users before adding them to the combo
            If Information.IsArray(vUsers) Then

                For lRow As Integer = 0 To vUsers.GetUpperBound(0) - 1

                    For lRow2 As Integer = lRow + 1 To vUsers.GetUpperBound(0)



                        If vUsers(lRow) > vUsers(lRow2) Then

                            sTemp = CStr(vUsers(lRow))


                            vUsers(lRow) = vUsers(lRow2)

                            vUsers(lRow2) = sTemp
                        End If
                    Next lRow2
                Next lRow

                ' Add users to combo

                For lRow As Integer = vUsers.GetLowerBound(0) To vUsers.GetUpperBound(0)

                    ' cboUser_NewIndex  = cboUser.Items.Add(CStr(vUsers(lRow)))
                    Dim cboUser_NewIndex As Integer = cboUser.Items.Add(CStr(vUsers(lRow)))

                    'Thinh Nguyen 25/03/2003 (start)- default to current user

                    If g_oObjectManager.UserName.Trim().ToUpper() = CStr(vUsers(lRow)).ToUpper() Then
                        lSelectItem = cboUser_NewIndex
                    End If
                    'Thinh Nguyen 25/03/2003 (end)- default to current user
                Next lRow

                'Thinh Nguyen 25/03/2003 (start)- default to current user
                cboUser.SelectedIndex = lSelectItem
                cboUser.Tag = CStr(lSelectItem)
                'Thinh Nguyen 25/03/2003 (end)- default to current user
            End If

            'Add Years to Combo Here add previous 10 Years EH
            Dim nCurrentyear As Integer
            nCurrentyear = CInt(Year(Now))
            For lRow As Int16 = 1 To 10
                CboYear.Items.Add(CStr(nCurrentyear))
                nCurrentyear = nCurrentyear - 1
            Next lRow
            CboYear.SelectedIndex = 0

            PopulateAccountHandlers()

            m_oBusinessUser = Nothing
            vUsers = Nothing
            ' hold Initialised status
            m_bIsInitialised = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.getOption(v_iOptionNumber:=10, r_sOptionValue:=m_sDMEOption)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_sDMEOption = "0"
            End If

            ' CTAF 021100 - Use CreateObject because it doesnt have Initialise
            ' Get an instance of bPMZipper.Business
            '    Set g_oZipper = CreateObject("bPMZipper.Business")

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
                If m_oSpoolerOLE IsNot Nothing Then
                    m_oSpoolerOLE.Dispose()
                    m_oSpoolerOLE = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If Not (m_oPMLock Is Nothing) Then

                    m_oPMLock = Nothing
                End If

            End If
            CloseWord()
        End If
        Me.disposedValue = True
    End Sub

    Private Const vbFormCode As Integer = 0
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
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Function
                End If
            End If

            ' Unload g_oZipper
            '    If (g_oZipper Is Nothing = False) Then
            '
            '        Set g_oZipper = Nothing
            '
            '    End If

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

            ' Center the interface.
            '    CenterForm Me

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lInsuranceFileCnt = 0 Then
                lblPolicy.Visible = False
                txtPolicy.Visible = False
            End If

            If m_lPartyCnt = 0 Then
                lblClient.Visible = False
                txtClientCode.Visible = False
            End If

            'TN20010801 - start
            If (m_lPartyCnt = 0) And (m_lInsuranceFileCnt = 0) Then
                chkSelectPrinter.Top -= VB6.TwipsToPixelsY(480)

                ' RG 20070410
                chkHidePrintedDocs.Top = VB6.ToPixelsUserY(VB6.FromPixelsUserY(chkHidePrintedDocs.Top, 0, 9270, 618) - 480, 0, 9270, 618)
                stbStatus.Top -= VB6.TwipsToPixelsY(480)
                ' RG End

            End If
            'TN20010801 - end

            If (m_lPartyCnt = 0) And (m_lInsuranceFileCnt = 0) Then
                lvwSearchDetails.Top -= VB6.TwipsToPixelsY(405)
                lvwSearchDetails.Height += VB6.TwipsToPixelsY(405)
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Set the column widths for the search list.
            '    lvwSearchDetails.ColumnHeaders(1).Width = 1600
            '    lvwSearchDetails.ColumnHeaders(2).Width = 1000
            '    lvwSearchDetails.ColumnHeaders(3).Width = 1000
            '    lvwSearchDetails.ColumnHeaders(4).Width = 1800
            '    lvwSearchDetails.ColumnHeaders(5).Width = 1800
            '    lvwSearchDetails.ColumnHeaders(6).Width = 1000
            '    lvwSearchDetails.ColumnHeaders(7).Width = 1000
            '    lvwSearchDetails.ColumnHeaders(8).Width = 1000

            'developer guide no.303
            lvwSearchDetails.FullRowSelect = True
            ' Check for errors.

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            optPrintDate.Checked = True


            If m_oBusiness.UnderwritingOrAgency <> "U" Then
                fraGroupClientAgentOffice.Visible = False
                fraOrderBy.Visible = False
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

    'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
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
    'm_lReturn = DisableInterface(bDisable:=True)
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

            lblClient.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPolicy.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    lvwSearchDetails.ColumnHeaders(1).Text = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitle1, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(2).Text = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitle2, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(3).Text = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitle3, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(4).Text = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitle4, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(5).Text = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitle5, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(6).Text = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitle6, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(7).Text = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitle7, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(8).Text = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitle8, _
            ''        iDataType:=PMResString)


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    ' Name: CancelListDocuments
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function CancelListDocuments() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the ListDocuments", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelListDocuments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectListDocuments
    '
    ' Description: Called when we wish to select
    '
    ' ***************************************************************** '
    Private Function SelectListDocuments() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select the ListDocuments", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectListDocuments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.
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

                ' its a cancel, so set STEPSTATUS to INCOMPLETE...

                'm_lReturn = m_frmInterface.SetStatus(PMNavStatusIncomplete, PMNavStatusIncomplete, PMNavStatusIncomplete)

            Else
                ' Update the property member from the interface.
                'AR20050315 - PN19493 - Set Print flag
                m_bPrintMode = False
                m_lReturn = DataToProperties()

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
            'developer guide no.168
            _stbStatus_Panel1.Text = " " & sMessage
            Application.DoEvents()

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
            'developer guide no.168
            _stbStatus_Panel1.Text = " " & m_lItemsFound & " " & sMessage
            Application.DoEvents()

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
    'If txtClientCode.Text.Trim() <> "" Then
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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            tabMainTab.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height)

            'If VB6.PixelsToTwipsY(ClientRectangle.Height) > 2700 And ClientRectangle.Width > 200 Then
            'lvwSearchDetails.SetBounds(VB6.TwipsToPixelsX(VB6.FromPixelsUserX(tabMainTab.Left, 0, 13140, 876) + 100), VB6.TwipsToPixelsY(VB6.FromPixelsUserY(tabMainTab.Top, 0, 9270, 618) + 2400), ClientRectangle.Width - VB6.TwipsToPixelsX(200), ClientRectangle.Height - VB6.TwipsToPixelsY(2700))
            'End If
            If VB6.PixelsToTwipsY(ClientRectangle.Height) > 2700 And ClientRectangle.Width > 200 Then
                lvwSearchDetails.SetBounds(tabMainTab.Left + VB6.TwipsToPixelsX(100), tabMainTab.Top + VB6.TwipsToPixelsY(2500), ClientRectangle.Width - VB6.TwipsToPixelsX(300), ClientRectangle.Height - VB6.TwipsToPixelsY(3000))
            End If
            Return result

        Catch



            ' Error Section.


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    '**************************************************************************
    ' Name: ArchiveDocument
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '        : PW250403 - add unzipped flag (Document Issuance changes)
    '**************************************************************************
    Private Function ArchiveDocument(ByRef lDocumentSpoolerId As Integer, ByRef lPartyCnt As Integer, ByRef lInsuranceFolderCnt As Integer, ByRef lInsuranceFileCnt As Integer, ByRef lClaimCnt As Integer, ByRef lDocumentTypeId As Integer, ByRef sDocumentDescription As String, ByRef lSpoolLevelInd As Integer, ByVal bUnzipped As Boolean, Optional ByRef DocumentTemplateID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim bSIRDOCAPI, bSIRDocTemplate As Object

        Dim lDocNumber, lEventCnt As Integer
        Dim vInsuranceFolderCnt As Integer
        Dim vInsuranceFileCnt As Integer
        Dim vClaimCnt As Integer
        'sj 16/10/2002 - start
        Dim bHTMLFormat As Boolean
        Dim sSpoolDoc As String
        Dim sActualFileName As String = String.Empty
        Dim oDocument As Word.Document
        'sj 16/10/2002 - end
        Dim oFSO As Object 'MKW210703 PN5343

        Const ARCHIVE_AS_PDF As Integer = 5009
        Dim sArchiveAsPDF As String = ""
        Dim sKeywords(0) As String

        Dim oSIRDocTemplate As bSIRDocTemplate.Business

        Dim oSIRDOCAPI As bSIRDOCAPI.Form
        Dim sPartyName As String = String.Empty
        Dim sInsuranceFileRef As String = String.Empty
        Dim sClaimRef As String = String.Empty

        Const DIGITAL_SIGNATURE As Integer = 5023
        Dim sDigitalSignature As String = ""
        Dim bIsDigitalSignature As Boolean
        Dim leventTypeID As Integer
        Dim vResultArray As Object
        Dim lDocumentTemplateID As Integer
        Dim sStr() As String
        Dim sDocType As String
        Dim sPageType As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Only call the archive to DMS thing if the option is set
            If m_sDMEOption = "1" Or m_sDMEOption = "2" Then

                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=ARCHIVE_AS_PDF, r_sOptionValue:=sArchiveAsPDF, v_iSourceID:=g_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                Return result
            End If
            m_lReturn = CopyServerToExport(lDocumentSpoolerId:=lDocumentSpoolerId, lSpoolLevelInd:=lSpoolLevelInd, bUnzipped:=bUnzipped, bHTMLFormat:=bHTMLFormat, sActualFileName:=sActualFileName)

            'DC260401 check that everythings okay here
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PDF files are archived differently because they do not need further
            'manipulation by Word, we skip iPMBSpooleOLE and go direct to DocuMaster
            Dim temp_oSIRDOCAPI As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oSIRDOCAPI, "bSIRDOCAPI.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSIRDOCAPI = temp_oSIRDOCAPI

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the bSIRDOCAPI object", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Dim temp_oSIRDocTemplate As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oSIRDocTemplate, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSIRDocTemplate = temp_oSIRDocTemplate

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the bSIRDocTemplate object", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'Convert the HTML to PDF
            sStr = sActualFileName.Split(".")

            If sArchiveAsPDF = "1" Then

                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=DIGITAL_SIGNATURE, r_sOptionValue:=sDigitalSignature, v_iSourceID:=g_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    bIsDigitalSignature = gPMFunctions.ToSafeBoolean(sDigitalSignature, False)
                End If

                sSpoolDoc = m_sClient & "\" & sStr(0) & ".PDF"

                m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(m_sClient & "\" & sActualFileName, sSpoolDoc)

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Get additional information
                sDocType = "F"
                sPageType = "PDF"
            Else
                '        sStr = Split(sActualFileName, ".")
                sSpoolDoc = m_sClient & "\" & sActualFileName
                sDocType = "D"
                sPageType = "DOC"

            End If
            m_lReturn = oSIRDocTemplate.GetInformation(lPartyCnt:=lPartyCnt, lInsuranceFolderCnt:=lInsuranceFolderCnt, lClaimCnt:=lClaimCnt, sPartyName:=sPartyName, sInsuranceFileRef:=sInsuranceFileRef, sClaimRef:=sClaimRef, lInsuranceFileCnt:=lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetInformation from bSIRDocTemplate object", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            m_lReturn = oSIRDOCAPI.AddDocument(lPartyId:=lPartyCnt, sPartyName:=sPartyName, lInsuranceFolderId:=lInsuranceFolderCnt, sInsuranceFileRef:=sInsuranceFileRef, lClaimId:=lClaimCnt, sClaimRef:=sClaimRef, lFSAComplaintFolderCnt:=0, sFSAComplaintReference:="", sDocType:=sDocType, sPageType:=sPageType, sDocName:=sDocumentDescription, sFilename:=sSpoolDoc, sAnnotation:="", sKeywords:=sKeywords, lDocNumber:=lDocNumber, vDocumentTemplateID:=DocumentTemplateID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to AddDocument in DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            oSIRDOCAPI.Dispose()
            oSIRDOCAPI = Nothing


            oSIRDocTemplate.Dispose()
            oSIRDocTemplate = Nothing

            'Only write to the event table if we've got a party cnt
            If lPartyCnt <> 0 Then
                If lInsuranceFolderCnt = 0 Then

                    vInsuranceFolderCnt = Nothing
                Else
                    vInsuranceFolderCnt = lInsuranceFolderCnt
                End If

                If lInsuranceFileCnt = 0 Then

                    vInsuranceFileCnt = Nothing
                Else
                    vInsuranceFileCnt = lInsuranceFileCnt
                End If

                If lClaimCnt = 0 Then

                    vClaimCnt = Nothing
                Else
                    vClaimCnt = lClaimCnt
                End If

                'Add the created event
                'MKW 070403 Pass Document Description to create event


                m_lReturn = m_oBusiness.CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=vInsuranceFolderCnt, v_vInsuranceFileCnt:=vInsuranceFileCnt, v_vClaimCnt:=vClaimCnt, v_vDocumentCnt:=lDocNumber, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=lDocumentTypeId, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventDocument, v_dtEventDate:=DateTime.Today, v_vDescription:=sDocumentDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = m_oBusiness.UpdateArchived(lDocumentSpoolerId:=lDocumentSpoolerId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Reset mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ArchiveDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            oFSO = Nothing 'MKW210703 PN5343

            ' Reset mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteDocument
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteDocument(ByRef lDocumentSpoolerId As Integer, ByRef lSpoolLevelInd As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Delete it from the table...

            m_lReturn = m_oBusiness.DirectDelete(vDocumentSpoolerId:=lDocumentSpoolerId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete it from the server...

            m_lReturn = DeleteServer(lDocumentSpoolerId:=lDocumentSpoolerId, lSpoolLevelInd:=lSpoolLevelInd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '**************************************************************************
    ' Name: EditDocument
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '        : PW250403 - add unzipped flag (Document Issuance changes)
    '**************************************************************************
    Private Function EditDocument(ByRef lDocumentSpoolerId As Integer, ByRef lSpoolLevelInd As Integer, ByVal bUnzipped As Boolean) As Integer

        Dim result As Integer = 0
        Dim bHTMLFormat As Boolean
        Dim sActualFileName As String = String.Empty
        Dim sSpoolDoc As String = String.Empty
        Dim oDocument As Word.Document
        Dim dtPause As Date
        Dim sOptionValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set variables for SaveDocument
            m_bUnZipped = bUnzipped
            m_lDocumentSpoolerId = lDocumentSpoolerId

            m_lReturn = CopyServerToClient(lDocumentSpoolerId:=lDocumentSpoolerId, lSpoolLevelInd:=lSpoolLevelInd, bUnzipped:=bUnzipped, bUseUniqueDirectory:=True, bHTMLFormat:=bHTMLFormat, sActualFileName:=sActualFileName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bHTMLFormat Then
                'Convert document to word format
                m_lReturn = ConvertDocumentToWord(v_sHTMLDocumentName:=sActualFileName, r_sWordDocumentFullPath:=sSpoolDoc)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_sClientHTML = sActualFileName
                m_sClientDocument = sSpoolDoc
            ElseIf sActualFileName.ToUpper().EndsWith("XML") Or sActualFileName.ToUpper().EndsWith("DOC") Then
                m_sClientHTML = sActualFileName
                m_sClientDocument = m_sClient & "\" & sActualFileName
            Else
                sSpoolDoc = Path.Combine(m_sClient, sActualFileName)
                sActualFileName = "Doc 0.doc"
                File.Move(sSpoolDoc, m_sClient & "\" & sActualFileName)
                m_sClientHTML = ""
                m_sClientDocument = m_sClient & "\" & sActualFileName
            End If

            'The length of the pause is determined by a system option
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5036, r_sOptionValue:=sOptionValue)

            Dim dbNumericTemp As Double
            If Double.TryParse(sOptionValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                If CInt(sOptionValue) > 0 Then
                    dtPause = DateTime.Now.AddSeconds(CInt(sOptionValue))
                    Do While DateTime.Now < dtPause
                        Application.DoEvents()
                    Loop
                End If
            End If

            m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oWord.Visible = False

            oDocument = m_oWord.Documents.Open(m_sClientDocument, ConfirmConversions:=False)

            m_oWord.CommandBars("Standard").Visible = True

            m_oWord.CommandBars("Formatting").Visible = True

            m_oWord.WindowState = Word.WdWindowState.wdWindowStateMaximize 'wdWindowStateMaximize

            'Force into PrintView
            m_oWord.ActiveWindow.View.Type = Word.WdViewType.wdPrintView

            MyBase.ParentForm.SendToBack()
            m_oWord.Visible = True

            AppActivate(m_oWord.Windows(1).Caption)

            'Wait until the word instance we opened, is closed.
            Do While OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue
                Sleep(500) 'Do nothing for half a second
                Application.DoEvents()
            Loop

            m_lReturn = bPMDocFunctions.CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

            'Save changed document.
            SaveDocument()

            m_lReturn = UnlockDocument(lDocumentSpoolerId:=m_lDocumentSpoolerId) 'PN20004

            ' Reset variable to allow other docs to be edited now   PN20005
            m_lDocumentSpoolerId = 0
            MyBase.ParentForm.BringToFront()
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function SaveDocument() As Integer

        Dim result As Integer = 0
        Dim sActualFileName As String = ""
        Dim bHTMLFormat As Boolean
        Dim oFSO As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        bHTMLFormat = True
        sActualFileName = m_sClientHTML

        m_lReturn = CopyClientToServer(sActualFileName:=sActualFileName, bHTMLFormat:=bHTMLFormat, bUnzipped:=m_bUnZipped)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oBusiness.UpdateModified(lDocumentSpoolerId:=m_lDocumentSpoolerId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' clear up folders and files
        oFSO = New Object()
        Try
            Directory.Delete(m_sClient)

        Catch excep As System.Exception
            'permission denied
            gPMFunctions.LogMessageToFile("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Folder: " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="Edit Document", excep:=excep)
        End Try



        oFSO = Nothing

        Return result

Err_SaveDocument:

        result = gPMConstants.PMEReturnCode.PMError

        oFSO = Nothing

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result


        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: ConvertDocumentToWord
    '
    ' Description:
    '
    ' History: 23/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ConvertDocumentToWord(ByVal v_sHTMLDocumentName As String, ByRef r_sWordDocumentFullPath As String) As Integer

        Dim result As Integer = 0
        Dim sSpoolDoc As String = ""
        Dim oDocument As Word.Document
        Dim dtPause As Date
        Dim sOptionValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = RunWord()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oWord.ScreenUpdating = False

            'open the HTML document
            sSpoolDoc = m_sClient & "\" & v_sHTMLDocumentName
            oDocument = m_oWord.Documents.Open(sSpoolDoc, ConfirmConversions:=False)

            'Some documents in Word 2000 or less require a pause after opening in order to allow them to fully open
            If CInt(m_sWordVersion.Substring(0, 1)) <= 9 Then
                'The length of the pause is determined by a system option
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5036, r_sOptionValue:=sOptionValue)

                Dim dbNumericTemp As Double
                If Double.TryParse(sOptionValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    If CInt(sOptionValue) > 0 Then
                        dtPause = DateTime.Now.AddSeconds(CInt(sOptionValue))
                        Do While DateTime.Now < dtPause
                            Application.DoEvents()
                        Loop
                    End If
                End If
            End If

            'Save it as a word document
            sSpoolDoc = m_sClient & "\" & v_sHTMLDocumentName.Substring(0, v_sHTMLDocumentName.Length - 3) & "doc"
            oDocument.SaveAs(FileName:=sSpoolDoc, FileFormat:=Word.WdSaveFormat.wdFormatDocument)

            'Close the document
            oDocument.Close()
            oDocument = Nothing

            'Close word
            m_lReturn = CloseWord()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sWordDocumentFullPath = sSpoolDoc

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertDocumentToWord Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertDocumentToWord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: MailDocument
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function MailDocument() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSpoolerOLE Is Nothing Then
                Dim temp_m_oSpoolerOLE As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oSpoolerOLE, sClassName:="iPMBSpoolerOLE.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oSpoolerOLE = temp_m_oSpoolerOLE

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Spooler OLE", vApp:=ACApp, vClass:=ACClass, vMethod:="MailDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            'sj 15/10/2002 - start
            'Not required
            'm_lReturn = CopyServerToClient(lDocumentSpoolerId:=lDocumentSpoolerId)
            'sj 15/10/2002 - end


            m_oSpoolerOLE.Mode = 3

            m_oSpoolerOLE.FileName = m_sClient & "\Doc 0.doc"


            m_lReturn = m_oSpoolerOLE.Start

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MailDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MailDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SendEmail
    '
    ' Description:
    '
    ' History: 25/07/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SendEmail(ByVal v_sEmailAddress As String, ByVal v_sDocument As String) As Integer

        Dim result As Integer = 0
        Dim oMapi As iPMMapi.PMMAPI
        Dim sSubject, sMessage As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a new instance of PM Mapi
            oMapi = New iPMMapi.PMMAPI()

            m_lReturn = CType(oMapi, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            sSubject = ""

            ' Get a subject for the email
            While (sSubject = "")
                sSubject = Interaction.InputBox("Please enter the E-Mail subject.", "E-mail")
            End While

            sMessage = ""

            ' Get the text for the email
            While (sMessage = "")
                sMessage = Interaction.InputBox("Please enter a text for the E-Mail.", "E-Mail text", "Please refer to the attached document for more details.")
            End While

            ' Add a message
            m_lReturn = oMapi.Messages.AddMessage(v_vSubject:=sSubject, v_vNoteText:=sMessage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to AddMessage to PMMapi." & Environment.NewLine & _
                                   "Subject: " & sSubject & Environment.NewLine & _
                                   "Message: " & sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="SendEmail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            With oMapi.Messages.LastItem

                ' Add the file

                .Attachments.Clear()

                m_lReturn = .Attachments.AddAttachment(v_vName:=v_sDocument, v_vPath:=v_sDocument)

                .Recipients.Clear()

                ' Add the recipients
                m_lReturn = .Recipients.AddRecipient(v_vAddress:=v_sEmailAddress, v_vName:=v_sEmailAddress, v_vAddressType:="SMTP")

                ' Set the email
                m_lReturn = .Send()

            End With

            ' Remove the instance
            oMapi = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendEmail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendEmail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: EmailDocument
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function EmailDocument() As Integer

        Dim result As Integer = 0
        Dim lDocumentSpoolerId As Integer
        Dim sClient As String = ""
        Dim sContactType As String = String.Empty
        Dim sErrorMessage As String = String.Empty
        Dim sEmailAddress As String = String.Empty
        Dim sDocument As String = String.Empty
        Dim lPartyCnt As Integer
        Dim lSpoolLevelInd As Integer
        Dim bHTMLFormat As Boolean
        Dim sActualFileName As String = ""
        
        Dim bUnzipped As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lNumberSelected = 0 Then
                Return result
            End If

            For lTemp As Integer = m_vDocumentSpoolerIds.GetLowerBound(1) To m_vDocumentSpoolerIds.GetUpperBound(1)

                ' Only print it if its not an email contact type
                If CStr(m_vDocumentSpoolerIds(ACDSContact, lTemp)) = gSIRLibrary.SIREmailContactCode Then

                    lDocumentSpoolerId = CInt(m_vDocumentSpoolerIds(ACDSDocumentSpoolerId, lTemp))
                    'sj 15/10/2002 - start
                    '            m_lReturn = CopyServerToClient(lDocumentSpoolerId:=lDocumentSpoolerId, _
                    ''                                           lSpoolLevelInd:=lSpoolLevelInd, _
                    ''                                           lCount:=lTemp)
                    '            If (m_lReturn <> PMTrue) Then
                    '                EmailDocument = PMFalse
                    '                Exit Function
                    '            End If
                    lSpoolLevelInd = CInt(m_vDocumentSpoolerIds(ACDSSpoolLevelInd, lTemp))

                    ' PW250403 - Get unzipped flag (Document Issuance changes)
                    ' TB070503 - Tinny Branched the wrong version for me
                    'bUnzipped = (Val(m_vDocumentSpoolerIds(ACDSUnzipped, lTemp)) = 1)

                    ' PW250403 - add unzipped flag (Document Issuance changes)
                    m_lReturn = CopyServerToClient(lDocumentSpoolerId:=lDocumentSpoolerId, lSpoolLevelInd:=lSpoolLevelInd, bUnzipped:=bUnzipped, lCount:=lTemp, bUseUniqueDirectory:=True, bHTMLFormat:=bHTMLFormat, sActualFileName:=sActualFileName)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If bHTMLFormat Then
                        m_lReturn = ConvertDocumentToWord(v_sHTMLDocumentName:=sActualFileName, r_sWordDocumentFullPath:=sDocument)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        sDocument = m_sClient & "\" & sActualFileName
                    End If
                    'sj 15/10/2002 - end

                    ' Set the party_cnt
                    lPartyCnt = CInt(m_vDocumentSpoolerIds(ACDSPartyCnt, lTemp))

                    ' Get the email address

                    m_lReturn = m_oBusiness.Party.GetPreferredContact(v_lPartyCnt:=lPartyCnt, r_sContactType:=sContactType, r_sErrorMessage:=sErrorMessage, r_sEmailAddress:=sEmailAddress)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set the document path
                    'sj 23/10/2002 - start
                    '            sDocument$ = m_sClient$ & "\Doc " & CStr(lTemp&) & ".doc"
                    'sj 23/10/2002 - end

                    ' Email the document here
                    m_lReturn = SendEmail(v_sEmailAddress:=sEmailAddress, v_sDocument:=sDocument)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' CTAF 030800 - Dont update anything for the moment
                    'lDocumentSpoolerId = CLng(m_vDocumentSpoolerIds(0, lTemp))
                    'Now we need to update the printed thing by 1
                    'Does this count as a modification?

                    'm_lReturn = m_oBusiness.UpdatePrinted(lDocumentSpoolerId:=lDocumentSpoolerId)

                    ' Now delete the file
                    File.Delete(sDocument)

                    Directory.Delete(m_sClient)
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EmailDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EmailDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function CopyServerToExport(ByRef lDocumentSpoolerId As Integer, ByRef lSpoolLevelInd As Integer, Optional ByRef bHTMLFormat As Boolean = False, Optional ByRef bUnzipped As Boolean = False, Optional ByRef sActualFileName As String = "") As Integer

        Dim result As Integer = 0
        Dim sExport As String = String.Empty
        Dim sServer As String = String.Empty
        Dim lFileNumber As Integer

        Try

            ' SET 16/07/2003 ISS5343
            result = gPMConstants.PMEReturnCode.PMTrue
            lFileNumber = lDocumentSpoolerId

            If m_sServer = "" Then
                m_lReturn = GetDocumentDirectory(r_sDocDirectory:=m_sServer)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to find the document directory", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToExport")
                    Return result
                End If
            End If

            m_lReturn = GetFullServerPath(v_lSpoolLevelInd:=lSpoolLevelInd, v_lDocumentSpoolerId:=lDocumentSpoolerId, r_sFullServerPath:=sServer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFullServerPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToExport")
                Return result
            End If

            m_lReturn = GetZipDirectory(r_sZipDirectory:=m_sZIP_DIRECTORY)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to locate zip directory", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToExport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            If Not bUnzipped Then
                ' unzip the contents
                m_lReturn = UnZip(sZipFile:=sServer, sOutputDirectory:=m_sZIP_DIRECTORY)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unzip files", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToExport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            ' find the export directory
            m_lReturn = GetExportDirectory(r_sExportDirectory:=sExport, bUnique:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to locate export directory", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToExport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' copy the current contents from the zipdirectory to the export directory
            m_lReturn = MoveFolderContents(sSourceDir:=m_sZIP_DIRECTORY, sDestDir:=sExport)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move files to export directory", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToExport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' return the filename and type
            m_lReturn = GetFileNameAndType(v_sFolder:=sExport, r_sFileName:=sActualFileName, r_bHTMLFormat:=bHTMLFormat)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get file type", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToExport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' return the export directory
            m_sClient = sExport

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy document from server to client", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToExport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LockDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function LockDocument(ByVal lDocumentSpoolerId As Integer, ByVal v_sDocumentDescription As String) As Integer

        Dim result As Integer = 0
        Dim sLockedBy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPMLock Is Nothing Then
                Dim temp_m_oPMLock As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPMLock = temp_m_oPMLock

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If


            m_lReturn = m_oPMLock.LockKey(sKeyName:="document_spooler_id", vKeyValue:=lDocumentSpoolerId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMMAlreadyInUse
                        MessageBox.Show("Document " & v_sDocumentDescription & " is currently locked by " & sLockedBy & "." & _
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later...", "Document Lock", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return result
                    End If


                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the document", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnlockDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function UnlockDocument(ByRef lDocumentSpoolerId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPMLock Is Nothing Then
                Dim temp_m_oPMLock As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPMLock = temp_m_oPMLock

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If


            m_lReturn = m_oPMLock.UnLockKey(sKeyName:="document_spooler_id", vKeyValue:=lDocumentSpoolerId, iUserID:=g_oObjectManager.UserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock the document", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CloseWord() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oWord Is Nothing Then
            Return result
        End If

        If m_bCreatedWord Then
            Try
                m_oWord.Quit(SaveChanges:=Word.WdSaveOptions.wdDoNotSaveChanges)

            Catch
            End Try


        End If

        m_oWord = Nothing

        Return result

Err_CloseWord:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to close Word", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseWord", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


        Return result
    End Function

    Private Function RunWord() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oWord Is Nothing) Then
                Return result
            End If

            m_bCreatedWord = False

            m_oWord = New Word.Application()
            m_bCreatedWord = True

            'TN20010801 - start
            If chkSelectPrinter.CheckState = CheckState.Checked Then
                m_oWord.Visible = True
            End If


            m_sWordVersion = ""

            ' Now find the version of Word that we're running
            m_sWordVersion = m_oWord.Application.Version

            If m_sWordVersion <> "" Then
                If Conversion.Val(m_sWordVersion) < 8 Then
                    MessageBox.Show("Incorrect Word version for Sirius For Broking (" & m_sWordVersion & ")." & Strings.Chr(13) & Strings.Chr(10) & "Contact Policy Master Support.", ACApp)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_oWord = Nothing
                    Return result
                End If
            Else
                MessageBox.Show("An error has occurred with Microsoft Word." & Strings.Chr(13) & Strings.Chr(10) & "Try starting with Word already open or Contact Policy Master Support.", "WordMaster 97 (RunWord)")
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oWord = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to run Word", vApp:=ACApp, vClass:=ACClass, vMethod:="RunWord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'sj 18/10/2002 - start
    ' PW250403 - add unzipped parameter (Document Issuance changes)
    Private Function CopyClientToServer(ByRef sActualFileName As String, ByRef bHTMLFormat As Boolean, ByRef bUnzipped As Boolean) As Integer

        Dim result As Integer = 0
        Dim sFileIn, sFileOut As String
        Dim iTemp As Integer
        Dim oFolder As DirectoryInfo
        Dim sFileNameNoExt, sDependancyFolder, sFile As String
        Dim bFolderExists As Boolean
        Dim oZipper As bPMZipper.Business

        Dim sFileCopyMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oZipper = New bPMZipper.Business()

            'First get rid of the old one...
            m_lReturn = bPMDocFunctions.DeleteFile(m_sServerPath)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the original file from the Server." & Strings.Chr(13) & Strings.Chr(10) & sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            sFileNameNoExt = sActualFileName.Substring(0, sActualFileName.Length - 4)
            sFileIn = m_sClient & "\" & sActualFileName
            sFileOut = m_sClient & "\" & sFileNameNoExt & ".zip"

            ' PW250403 - if this is an unzipped document, simply copy it back
            ' without zipping up (Document Issuance changes)
            If bUnzipped Then
                m_lReturn = PMCopyFile.PMFileCopy(sFileIn, m_sServerPath, sFileCopyMsg)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy document from client to server." & Strings.Chr(13) & Strings.Chr(10) & sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
                m_lReturn = bPMDocFunctions.DeleteFile(sFileIn)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the edited file from the Client." & Strings.Chr(13) & Strings.Chr(10) & sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
                Return result
            End If

            iTemp = oZipper.ZipFile(sFileIn:=sFileIn, sFileOut:=sFileOut)

            If Not iTemp Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" oZipper.ZipFile Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = bPMDocFunctions.DeleteFile(sFileIn)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the edited file from the Client." & Strings.Chr(13) & Strings.Chr(10) & sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If bHTMLFormat Then

                sDependancyFolder = m_sClient & "\" & sFileNameNoExt & "_files"
                bFolderExists = Directory.Exists(sDependancyFolder)
                If bFolderExists Then
                    oFolder = New DirectoryInfo(sDependancyFolder)
                    For Each vFile As FileInfo In oFolder.GetFiles
                        sFile = sDependancyFolder & "\" & vFile.Name
                        iTemp = oZipper.addFileToZIP(sFileOut, sFile)
                    Next vFile
                End If
            End If

            ' 141102 TB PMFileCopy tells you why a FileCopy operation failed
            m_lReturn = PMCopyFile.PMFileCopy(sFileOut, m_sServerPath, sFileCopyMsg)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy document from client to server." & Strings.Chr(13) & Strings.Chr(10) & sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If
            ' FileCopy sFileOut, m_sServerPath

            'Delete the local copy
            m_lReturn = bPMDocFunctions.DeleteFile(sFileOut)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the zipped file from the Client." & Strings.Chr(13) & Strings.Chr(10) & sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            oFolder = Nothing
            oZipper = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy document from client to server", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '**************************************************************************
    ' Name: CopyServerToClient
    '
    ' Description: copies the file from the server to the client
    '
    ' History: PW250403 - Add Unzipped parameter (Document Issuance changes)
    '**************************************************************************
    Private Function CopyServerToClient(ByRef lDocumentSpoolerId As Integer, ByRef lSpoolLevelInd As Integer, ByVal bUnzipped As Boolean, Optional ByRef bUseUniqueDirectory As Boolean = False, Optional ByRef lCount As Integer = 0, Optional ByRef bHTMLFormat As Boolean = False, Optional ByRef sActualFileName As String = "") As Integer

        Dim result As Integer = 0
        Dim sExport As String = ""
        Dim sServer As String = ""
        Dim lFileNumber As Integer
        

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lFileNumber = lDocumentSpoolerId
            m_lReturn = GetDocumentDirectory(r_sDocDirectory:=m_sServer)
            

            m_lReturn = GetFullServerPath(v_lSpoolLevelInd:=lSpoolLevelInd, v_lDocumentSpoolerId:=lDocumentSpoolerId, r_sFullServerPath:=sServer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFullServerPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient")
                Return result
            End If

            'MKW210703 PN5343 START
            '    ' PW250403 - If this is an unzipped document, just copy it over
            '    ' without unzipping (Document Issuance changes)
            '    If bUnzipped Then
            '        sServer = Left(sServer, Len(sServer) - 3) + "htm"
            '        sClient = oFSO.BuildPath(m_sClient, oFSO.GetFileName(sServer))
            '        sActualFileName = oFSO.GetFileName(sServer)
            '        Call oFSO.CopyFile(sServer, sClient)
            '        bHTMLFormat = True
            '        Set oFSO = Nothing
            '        m_sServerPath = sServer
            '        Exit Function
            '    End If
            '
            '    'Delete the folder and all its contents then recreate it
            '    If Dir(m_sClient, vbDirectory) <> "" Then
            '        Call oFSO.DeleteFolder(m_sClient, True)
            '    End If
            '
            '    Call oFSO.CreateFolder(m_sClient)
            '
            '
            '    'This lot is because of the way PMZipper unzips, and the way we're changing the
            '    'number of each document.  e.g. doc 2 is copied over as doc 4.  The problem is that
            '    'unzipping retains the zipped name, then the unzipped document is renamed.  So it
            '    'doc 2, then renames.  Most unwanted.  So we use a temporary directory to hold the zip,
            '    '
            '
            '    sClient = m_sClient & "\Temp"
            '    Call oFSO.CreateFolder(sClient)
            '
            '    sClient = oFSO.BuildPath(sClient, oFSO.GetFileName(sServer))
            '    Call oFSO.CopyFile(sServer, sClient)
            '
            '    m_lReturn = UnZip(sClient, bHTMLFormat, sActualFileName)
            '
            '    sTemp = m_sClient & "\" & sActualFileName
            '    sClient = m_sClient & "\Temp\" & sActualFileName
            '
            '    Name sClient As sTemp
            '
            '    If bHTMLFormat = True Then
            '        sDependancyDirectory = _
            ''            m_sClient & "\" & _
            ''            Left(sActualFileName, (Len(sActualFileName) - 4)) & _
            ''            "_files"
            '        'Call oFSO.CopyFolder(m_sClient & "\Temp", sDependancyDirectory)
            '        'Call oFSO.DeleteFolder(m_sClient & "\Temp", True)
            '        Name m_sClient & "\Temp" As sDependancyDirectory
            '    End If
            '    Set oFSO = Nothing

            If Not bUnzipped Then
                m_lReturn = GetZipDirectory(r_sZipDirectory:=m_sZIP_DIRECTORY)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to locate zip directory", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                ' unzip the contents
                m_lReturn = bPMDocFunctions.UnZip(sZipFile:=sServer, sOutputDirectory:=m_sZIP_DIRECTORY)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unzip files", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            ' find the client directory
            m_lReturn = GetClientDirectory(r_sClientDir:=m_sClient, bUnique:=bUseUniqueDirectory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to locate client directory", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' copy the current contents from the zipdirectory to the client directory
            m_lReturn = MoveFolderContents(sSourceDir:=m_sZIP_DIRECTORY, sDestDir:=m_sClient)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move files to client directory", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' return the filename and type
            m_lReturn = GetFileNameAndType(v_sFolder:=m_sClient, r_sFileName:=sActualFileName, r_bHTMLFormat:=bHTMLFormat)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get file type", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If


            'MKW210703 PN5343 END

            'Set this so that if we're editing we know where to copy it back...
            m_sServerPath = sServer

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy document from server to client", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetFullServerPath
    '
    ' Description:
    '
    ' History: 23/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetFullServerPath(ByVal v_lSpoolLevelInd As Integer, ByVal v_lDocumentSpoolerId As Integer, ByRef r_sFullServerPath As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lTemp, lTemp1, lTemp2 As Integer

            If v_lSpoolLevelInd = 1 Then
                lTemp = v_lDocumentSpoolerId \ 1000000
                lTemp1 = (v_lDocumentSpoolerId \ 1000) - (lTemp * 1000)
                lTemp2 = v_lDocumentSpoolerId - (lTemp2 * 1000) - (lTemp * 1000000)

                r_sFullServerPath = m_sServer & "\Spooled Documents" & "\Company " & CStr(g_iSourceID) & "\" & StringsHelper.Format(lTemp, "000") & "\" & StringsHelper.Format(lTemp1, "000") & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"
            Else
                lTemp = v_lDocumentSpoolerId \ 1000
                lTemp2 = v_lDocumentSpoolerId - (lTemp * 1000)

                r_sFullServerPath = m_sServer & "\Spooled Documents" & "\Company " & CStr(g_iSourceID) & "\" & StringsHelper.Format(lTemp, "000") & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFullServerPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFullServerPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: DeleteServer
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteServer(ByRef lDocumentSpoolerId As Integer, ByRef lSpoolLevelInd As Integer) As Integer

        Dim result As Integer = 0
        Dim sServer As String = ""
        Dim sTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = GetServer()

            'sj 23/10/2002 - start
            m_lReturn = GetFullServerPath(v_lSpoolLevelInd:=lSpoolLevelInd, v_lDocumentSpoolerId:=lDocumentSpoolerId, r_sFullServerPath:=sServer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFullServerPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteServer")
                Return result
            End If

           

            'Check that it's still there...
            sTemp = FileSystem.Dir(sServer, FileAttribute.Normal)

            If sTemp <> "" Then
                File.Delete(sServer)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteServer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

  
    ' ***************************************************************** '
    '
    ' Name: GetFileNameAndType
    '
    ' Description:
    '
    ' History: 18/10/2002 sj - Created.
    '
    ' 11/07/2003 JMK - Include .log (Renewal Acceptance.log)
    ' ***************************************************************** '
    Private Function GetFileNameAndType(ByVal v_sFolder As String, ByRef r_sFileName As String, ByRef r_bHTMLFormat As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oFSO As Object
            Dim oFolder As DirectoryInfo
            Dim sExt As String
            Dim sPath As String = String.Empty

            r_sFileName = ""
            r_bHTMLFormat = False

            oFSO = New Object()
            oFolder = New DirectoryInfo(v_sFolder)

            For Each vFile As FileInfo In oFolder.GetFiles
                sExt = Path.GetExtension(sPath & "\" & vFile.Name).Substring(1)
                Select Case (sExt.ToUpper())
                    Case "HTM"
                        r_sFileName = vFile.Name
                        r_bHTMLFormat = True
                        oFSO = Nothing
                        oFolder = Nothing
                        Return result
                    Case "DOC"
                        r_sFileName = vFile.Name
                        r_bHTMLFormat = False
                        oFSO = Nothing
                        oFolder = Nothing
                        Return result
                        'JMK
                    Case "LOG"
                        r_sFileName = vFile.Name
                        r_bHTMLFormat = False
                        oFSO = Nothing
                        oFolder = Nothing
                        Return result
                    Case "XML"
                        r_sFileName = vFile.Name
                        r_bHTMLFormat = False
                        oFSO = Nothing
                        oFolder = Nothing
                        Return result
                    Case "WORD"
                        r_sFileName = vFile.Name
                        r_bHTMLFormat = False
                        oFSO = Nothing
                        oFolder = Nothing
                        Return result
                End Select
            Next vFile

            oFSO = Nothing
            oFolder = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFileNameAndType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFileNameAndType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetServer
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    '
    ' ***************************************************************** '
    Private Function GetServer() As Integer

        Dim result As Integer = 0
        Dim sServer As String = ""

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sServer.Trim() > "" Then
                Return result
            End If

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            sServer = ""

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DocServer", r_sSettingValue:=sServer)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Server from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_sServer = sServer
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cboUser_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUser.SelectedIndexChanged

        If m_bIsInitialised Then
            'Thinh Nguyen 25/03/2003 - refresh when user is changed set it as current user
            If cboUser.SelectedIndex <> CInt(Convert.ToString(cboUser.Tag)) Then
                cboUser.Tag = CStr(cboUser.SelectedIndex)

                m_lReturn = GetDocuments()
            End If
        End If
    End Sub

    Private Sub chkShowCurrent_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkShowCurrent.CheckStateChanged

        If m_bIsInitialised Then
            cboUser.Enabled = Not (chkShowCurrent.CheckState = CheckState.Checked)

            If chkShowCurrent.CheckState = CheckState.Checked Then
                'Setting cboUser combo with the current logged in user
                For lRow As Integer = 0 To cboUser.Items.Count - 1
                    If g_oObjectManager.UserName.Trim().ToUpper() = VB6.GetItemString(cboUser, lRow).ToUpper() Then
                        cboUser.SelectedIndex = lRow
                        cboUser.Tag = CStr(lRow)
                        Exit For
                    End If
                Next lRow
            End If

            '        m_lReturn = GetDocuments() 'Thinh Nguyen  25/03/2003 we might have thousands of docs..only refresh when refresh button is clicked
        End If
    End Sub


    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        Dim iDirection As SortOrder

        ' Column click event for the search details

        Try


            ' If current sort column header is
            ' pressed.

            With lvwSearchDetails

                'AR20050323 - PN13216 use correct column index
                If (ColumnHeader.Index + 1 = m_lDateColumn2) Then

                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)

                    iDirection = ListViewHelper.GetSortOrderProperty(lvwSearchDetails)

                    m_lReturn = ListViewSortByDate_Special(v_oListView:=lvwSearchDetails, v_iSourceColumn:=ColumnHeader.Index + 1, v_iDirection:=iDirection)

                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails)) Then

                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)

                    ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
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

    'AR20050323 - PN13216 Custom sort order for date columns .. by username then date
    Private Function ListViewSortByDate_Special(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer

        Dim result As Integer = 0
        Dim sDate As String = ""
        Dim iEndColumn, iIndex As Integer
        Dim sUsername As String = ""
        Dim lItemId As Integer
        Dim sSearchKey As String = ""

        Const ACLVTag As String = "SPOOL_DATE_HIDDEN"
        Const ACPadChar As Integer = 32

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'KB PN 4360
            'Check if we already have the extra sort column
            iEndColumn = v_oListView.Columns.Count
            If v_oListView.Columns.Item(iEndColumn - 1).Name = ACLVTag Then
                'do nothing as no need to add it
            Else
                ' Add the column
                v_oListView.Columns.Add(ACLVTag, "Internal Spool Sort", CInt(VB6.TwipsToPixelsX(0)))
            End If

            ' Get the index of this new column, -1 because it's a sub item
            iIndex = v_oListView.Columns.Count - 1

            ' Not sorted yet
            ListViewHelper.SetSortedProperty(v_oListView, False)

            ' Add the items
            For iLoop1 As Integer = 1 To v_oListView.Items.Count

                lItemId = Convert.ToString(v_oListView.Items.Item(iLoop1 - 1).Tag)
                sUsername = CStr(m_vSearchData(ACDCreatedBy, lItemId))

                If v_iSourceColumn = m_lDateColumn1 Then
                    sDate = CStr(m_vSearchData(ACDDateCreated, lItemId))
                Else
                    sDate = CStr(m_vSearchData(ACDDateModified, lItemId))
                End If

                Dim TempDate As Date
                sDate = IIf(DateTime.TryParse(sDate, TempDate), TempDate.ToString("yyyyMMddHHMMss"), sDate)
                sSearchKey = sUsername & New String(Strings.Chr(ACPadChar), 255 - sUsername.Length) & sDate

                ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), iIndex).Text = sSearchKey

            Next iLoop1

            ' Sort now
            ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)

            ' Set the sort key
            ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)
            ListViewHelper.SetSortedProperty(v_oListView, True)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByDate_Special Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByDate_Special", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Dim bSelected As Boolean

        If lvwSearchDetails.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            bSelected = False
        Else
            bSelected = True
        End If

        RaiseEvent lvwSearchDetailsClick(Me, New lvwSearchDetailsClickEventArgs(bSelected))

    End Sub

    Private Sub uctListDocuments_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try

            m_lReturn = ResizeInterface()

        Catch

            Exit Sub
        End Try


    End Sub

    ' PRIVATE Events (End)

    ' ***************************************************************** '
    '
    ' Name: PrintDocument
    '
    ' Description:
    '
    ' History: 05/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function PrintDocument() As Integer

        Dim result As Integer = 0
        Dim lDocumentSpoolerId As Integer
        Dim oDocument As Word.Document
        Dim oSelection As Word.Selection
        Dim sClient As String = ""
        Dim oRange As Word.Range

       
        Dim sOriginalPrinter As String = String.Empty
        Dim m_bPrinterChanged As Boolean

        Dim lSpoolLevelInd As Integer
        Dim bHTMLFormat As Boolean
        Dim sActualFileName As String = ""
        Dim bUnzipped As Boolean
        Dim sDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lNumberSelected = 0 Then
                Return result
            End If

            m_lReturn = RunWord()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove this as it causes outlines to not be printed. PN23607.
            '    m_oWord.ScreenUpdating = False

            For lTemp As Integer = m_vDocumentSpoolerIds.GetLowerBound(1) To m_vDocumentSpoolerIds.GetUpperBound(1)

                ' Only print it if its not an email contact type
                If CStr(m_vDocumentSpoolerIds(ACDSContact, lTemp)) <> gSIRLibrary.SIREmailContactCode Then

                    sDescription = "'" & CStr(m_vDocumentSpoolerIds(ACDSDescription, lTemp)) & "' created by " & CStr(m_vDocumentSpoolerIds(ACDSCreatedBy, lTemp)) & " on " & gPMFunctions.ToSafeString(CStr(m_vDocumentSpoolerIds(ACDSDateCreated, lTemp)), "unknown")
                    lDocumentSpoolerId = CInt(m_vDocumentSpoolerIds(ACDSDocumentSpoolerId, lTemp))

                    ' Check doc hasn't already been locked before we print it! Also, lock it at the start
                    ' of this process and unlock after   PN20004
                    m_lReturn = LockDocument(lDocumentSpoolerId:=lDocumentSpoolerId, v_sDocumentDescription:=sDescription)

                    ' Carry on if locked ok but if record in use just skip to next (if there is one), else exit
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMMAlreadyInUse Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else


                        'sj 15/10/2002 - start
                        lSpoolLevelInd = CInt(m_vDocumentSpoolerIds(ACDSSpoolLevelInd, lTemp))
                        'sj 15/10/2002 - end

                        ' PW250403 - Get unzipped flag (Document Issuance changes)
                        ' TB070503 - Tinny Branched the wrong version for me
                        'bUnzipped = (Val(m_vDocumentSpoolerIds(ACDSUnzipped, lTemp)) = 1)

                        m_lReturn = CopyServerToClient(lDocumentSpoolerId:=lDocumentSpoolerId, lSpoolLevelInd:=lSpoolLevelInd, bUnzipped:=bUnzipped, bUseUniqueDirectory:=True, lCount:=lTemp, bHTMLFormat:=bHTMLFormat, sActualFileName:=sActualFileName)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Unlock doc.  PN20004
                            m_lReturn = UnlockDocument(lDocumentSpoolerId:=lDocumentSpoolerId)
                            Return result
                        End If
                        'sj 16/10/2002 - start
                        sClient = m_sClient & "\" & sActualFileName
                        'sj 16/10/2002 - end

                        oDocument = m_oWord.Documents.Open(sClient, ConfirmConversions:=False)
                        Application.DoEvents()

                        'RWH(17/07/01) Set PrintBackground to false to
                        'asynchronously print doc and enable us to shut down properly.
                        m_oWord.Application.Options.PrintBackground = False

                        m_bPrinterChanged = False
                        'RWH(27/07/01) Use specific printer stored against a template.

                        If m_oBusiness.UnderwritingOrAgency = "U" Then

                            If Not (Convert.IsDBNull(m_vDocumentSpoolerIds(ACDSDocumentPrinter, lTemp)) Or IsNothing(m_vDocumentSpoolerIds(ACDSDocumentPrinter, lTemp))) Then
                                If CStr(m_vDocumentSpoolerIds(ACDSDocumentPrinter, lTemp)) <> "" Then
                                    sOriginalPrinter = m_oWord.ActivePrinter
                                    m_oWord.ActivePrinter = CStr(m_vDocumentSpoolerIds(ACDSDocumentPrinter, lTemp))
                                    m_bPrinterChanged = True
                                End If
                            End If
                        End If

                        'TN20010801 - start
                        If chkSelectPrinter.CheckState = CheckState.Checked Then
                            m_oWord.Run("Normal.PMDocumentManager.PMBPrintDocument")
                        Else
                            oDocument.PrintOut()
                        End If
                        'TN20010801 - end

                        'RWH(27/07/01) Reset original printer if necessary.
                        If m_bPrinterChanged Then
                            m_oWord.ActivePrinter = sOriginalPrinter
                        End If

                        oDocument.Close()

                        oDocument = Nothing


                        Try
                            File.Delete(sClient)
                            Directory.Delete(m_sClient)
                        Catch ex As Exception

                        End Try

                        lDocumentSpoolerId = CInt(m_vDocumentSpoolerIds(ACDSDocumentSpoolerId, lTemp))
                        'Now we need to update the printed thing by 1
                        'Does this count as a modification?

                        ' TB070503 186 BO doesn't have 2nd parameter
                        'm_lReturn = m_oBusiness.UpdatePrinted(lDocumentSpoolerId:=lDocumentSpoolerId, _
                        ''                                      lDocumentStatus:=SIREDocumentStatus.SIRLocalPrint)

                        m_lReturn = m_oBusiness.UpdatePrinted(lDocumentSpoolerId:=lDocumentSpoolerId)

                        'FSA Phase III
                        'RKS 09/11/2004 PN16587
                        'The below mentioned code is not underwriting specific
                        If m_sUnderwritingOrAgency <> "U" Then
                            If CStr(m_vDocumentSpoolerIds(ACDSTemplateCode, lTemp)).Trim() = "TOB" Then
                                m_lReturn = UpdatePartyTobLetter(lPartyCnt:=CInt(m_vDocumentSpoolerIds(ACDSPartyCnt, lTemp)))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument")
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    ' Unlock doc.  PN20004
                                    m_lReturn = UnlockDocument(lDocumentSpoolerId:=lDocumentSpoolerId)
                                    Return result
                                End If
                            End If
                        End If

                        ' Unlock doc.  PN20004
                        m_lReturn = UnlockDocument(lDocumentSpoolerId:=lDocumentSpoolerId)

                    End If
                End If

            Next lTemp

            m_lReturn = CloseWord()


            ' This m_lReturn is got from the above select statement
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            'RWH(17/07/01) Let's tidy up if we error.
            m_lReturn = CloseWord()

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End Try

        Return result
    End Function

    Private Function OurInstanceOfWordIsRunning() As Integer
        Dim sTest As String = ""

        Try

            'is our word still running?
            m_lReturn = bPMDocFunctions.IsWindow(m_lWordHwnd)


            If m_lReturn = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try

        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdatePartyTobLetter
    '
    ' Description: FSA Phase III
    '
    ' History: 03/11/2004 Elaine Knott.
    '
    ' ***************************************************************** '
    Public Function UpdatePartyTobLetter(ByRef lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim oParty As bSIRParty.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sMessage As String = ""

            Dim temp_oParty As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oParty, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oParty = temp_oParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get Instance of bSIRParty.", "UpdatePartyTobLetter")
            End If


            m_lReturn = oParty.UpdateTobLetter(lPartyCnt:=lPartyCnt)

            'Inform user if failed to complete.
            'OK to continue

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to Update party Tob Letter.", "UpdatePartyTobLetter")
            End If


            oParty.Dispose()

            oParty = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyTobLetter", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePartyTobLetter", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    Private Sub cboAccountHandler_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccountHandler.SelectedIndexChanged

        If m_bIsInitialised Then
            'refresh when Account Handler
            If cboAccountHandler.SelectedIndex <> CInt(Convert.ToString(cboAccountHandler.Tag)) Then
                cboAccountHandler.Tag = CStr(cboAccountHandler.SelectedIndex)
                m_lReturn = GetDocuments()
            End If
        End If
    End Sub

    Private Sub PopulateAccountHandlers()
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PopulateAccountHandlers
        ' PURPOSE:
        ' AUTHOR: Danny Davis
        ' DATE: 03 March 2005, 14:56:45
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try

            Dim vAccountHandler(,) As Object = Nothing

            ' Set default values for user
            cboAccountHandler.SelectedIndex = 0
            'Modified by Vijay Pal on 5/11/2010 11:38:05 AM declare variable outside if loop
            Dim cboAccountHandler_NewIndex As Integer
            If cboAccountHandler.Items.Count <= 0 Then
                'Modified by Vijay Pal on 5/11/2010 11:38:05 AM  comment the line and declare variable outside if loop
                'Dim cboAccountHandler_NewIndex As Integer = -1
                cboAccountHandler_NewIndex = -1
                cboAccountHandler_NewIndex = cboAccountHandler.Items.Add("(All)")
            End If

            m_lReturn = m_oBusiness.GetAccountHandlers(r_vResultArray:=vAccountHandler)

            If Information.IsArray(vAccountHandler) Then

                For iLoop1 As Integer = 0 To vAccountHandler.GetUpperBound(1)

                    cboAccountHandler_NewIndex = cboAccountHandler.Items.Add(CStr(vAccountHandler(1, iLoop1)))

                    VB6.SetItemData(cboAccountHandler, cboAccountHandler_NewIndex, CInt(vAccountHandler(0, iLoop1)))
                Next iLoop1

                ' Set to first item
                cboAccountHandler.SelectedIndex = 0
                cboAccountHandler.Tag = CStr(0)
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountHandlers", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
        Exit Sub
    End Sub


    ' ***************************************************************** '
    '
    ' Name: ConvertDocumentToHTML
    '
    ' Description: Converting Word compatible document to HTML
    '
    ' History: 10/05/2006 Ramakant Singh Created
    '
    ' ***************************************************************** '

    Private Function ConvertDocumentToHTML(ByVal v_sDocumentName As String, ByRef r_sHTMLDocumentFullPath As String) As Integer

        Dim result As Integer = 0
        Dim sSpoolDoc As String = ""
        Dim oDocument As Word.Document
        Dim dtPause As Date
        Dim sOptionValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = RunWord()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oWord.ScreenUpdating = False

            'open the document
            sSpoolDoc = m_sClient & "\" & v_sDocumentName
            oDocument = m_oWord.Documents.Open(sSpoolDoc, ConfirmConversions:=False)

            'Some documents in Word 2000 or less require a pause after opening in order to allow them to fully open
            If CInt(m_sWordVersion.Substring(0, 1)) <= 9 Then
                'The length of the pause is determined by a system option
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5036, r_sOptionValue:=sOptionValue)

                Dim dbNumericTemp As Double
                If Double.TryParse(sOptionValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    If CInt(sOptionValue) > 0 Then
                        dtPause = DateTime.Now.AddSeconds(CInt(sOptionValue))
                        Do While DateTime.Now < dtPause
                            Application.DoEvents()
                        Loop
                    End If
                End If
            End If

            'Save it as a HTML document
            sSpoolDoc = m_sClient & "\" & v_sDocumentName.Substring(0, v_sDocumentName.Length - 3) & "htm"

            oDocument.SaveAs(FileName:=sSpoolDoc, FileFormat:=Word.WdSaveFormat.wdFormatHTML)

            'Close the document
            oDocument.Close()
            oDocument = Nothing

            'Close word
            m_lReturn = CloseWord()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sHTMLDocumentFullPath = sSpoolDoc

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertDocumentToHTML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertDocumentToHTML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwSearchDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSearchDetails.SelectedIndexChanged
        Try
            If Not IsNothing(lvwSearchDetails.FocusedItem) Then
                RaiseEvent lvwSearchDetailsItemClick(Me, New lvwSearchDetailsItemClickEventArgs(gPMFunctions.ToSafeInteger(m_vSearchData(ACDIsEditable, gPMFunctions.ToSafeInteger(lvwSearchDetails.FocusedItem.Tag))) = 1))
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="lvwSearchDetails_ItemClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

    Public Function UnZip(ByVal sZipFile As String, ByVal sOutputDirectory As String, Optional ByVal v_bDeleteZipFile As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oZipper As bPMZipper.Business
        Dim oFSO As Object = Nothing
        Dim oFolder As DirectoryInfo
        Dim oFile As FileInfo
        Try
            Dim sFileIn As String
            Dim sFileOut As String = String.Empty
            Dim sTemp As String = String.Empty
            Dim bSuccess As Boolean
            Dim sDependencyDir As String = ""
            Dim bIsHTML As Boolean

            Dim sZipFileName, sZipFileFolderName As String
            Dim m_bSameDirectory As Boolean

            result = gPMConstants.PMEReturnCode.PMTrue

            'Changes done as per VB code

            oFSO = New Scripting.FileSystemObject()

            ' does the file exist
            If Not File.Exists(sZipFile) Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="File (" & sZipFile & ") does not exist", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:="Unzip failed")

                oFSO = Nothing
                Return result
            End If

            ' does the output folder exist
            If Not Directory.Exists(sOutputDirectory) Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Directory (" & sOutputDirectory & ") does not exist", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:="Unzip failed")

                oFSO = Nothing
                Return result
            End If

            ' Check if the Zip File is in the same output directory
            sZipFileName = oFSO.GetFileName(sZipFile)
            sZipFileName = sZipFileName.ToLower()
            sZipFileFolderName = Path.GetDirectoryName(sZipFile)
            sZipFileFolderName = sZipFileFolderName.ToLower()

            If sOutputDirectory.ToLower() <> sZipFileFolderName Then
                ' since the zip file is not in the output directory, so we need to
                '  make sure the Output directory is empty,
                m_lReturn = CType(ClearDirectory(sDirectory:=sOutputDirectory), gPMConstants.PMEReturnCode)
                ' Error message to be placed here
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Build the path
                sFileIn = Path.Combine(sOutputDirectory, sZipFileName)

                m_lReturn = CType(bPMDocFunctions.CopyFile(sZipFile, sFileIn, True, False, sTemp), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file." & Strings.Chr(13) & Strings.Chr(10) & _
                                       "Source File      : " & sZipFile & Strings.Chr(13) & Strings.Chr(10) & _
                                       "Destination File : " & sFileIn & Strings.Chr(13) & Strings.Chr(10) & _
                                       "Error Details    : " & sTemp, vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            Else
                ' The zip file is in the same directory (output directory), so we need to
                ' clear the directory other than this zip file
                m_bSameDirectory = True
                sFileIn = sZipFile

                oFolder = New DirectoryInfo(sZipFileFolderName)

                ' Clear the sub folders first
                For Each oSubFolder As DirectoryInfo In oFolder.GetDirectories
                    oSubFolder.Delete(True)
                Next oSubFolder

                ' Clear the files then
                For Each oFile2 As FileInfo In oFolder.GetFiles
                    oFile = oFile2
                    If oFile.Name.ToLower() <> sZipFileName Then
                        'oFile.Delete(True)
                        oFile.Delete()
                    End If
                Next oFile2

            End If

            oZipper = New bPMZipper.Business()
            If oZipper Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load zipper object", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:="Unzip failed")

                Return result
            End If

            bSuccess = oZipper.UnZipFile(sFileIn, sOutputDirectory)

            oZipper = Nothing
            If Not bSuccess Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not m_bSameDirectory Then
                ' remove the zip file in the target directory, since this is
                ' a copy of the original file
                'UPGRADE_WARNING: (2081) DeleteFile has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2081.aspx
                File.Delete(sFileIn)
            End If

            ' what type of files have we extracted
            m_lReturn = CType(GetFileNameAndType(v_sFolder:=sOutputDirectory, r_sFileName:=sFileOut, r_bHTMLFormat:=bIsHTML), gPMConstants.PMEReturnCode)

            If bIsHTML Then
                'Move extra files into dependency folder
                sDependencyDir = sFileOut.Substring(0, sFileOut.Length - 4) & "_files"
                sDependencyDir = Path.Combine(sOutputDirectory, sDependencyDir)
                If Not Directory.Exists(sDependencyDir) Then
                    m_lReturn = CType(CreateFolderTree(sDependencyDir), gPMConstants.PMEReturnCode)

                    ' did we create the folder
                    If Not Directory.Exists(sDependencyDir) Then
                        result = gPMConstants.PMEReturnCode.PMError
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create directory (" & sDependencyDir & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:="Unzip failed")

                        Return result
                    End If
                End If

                ' move the files to the sub directory
                oFolder = New DirectoryInfo(sOutputDirectory)
                For Each oFile In oFolder.GetFiles
                    If oFile.Name.ToLower() <> sZipFileName Then
                        ' Don't move the zip file
                        If oFile.Name <> sFileOut Then
                            Debug.WriteLine("moving file " & oFile.Name)
                            sFileIn = Path.Combine(sDependencyDir, oFile.Name)
                            oFile.MoveTo(sFileIn)
                        End If
                    End If
                Next oFile
            End If

            ' RAM20040301 : Delete the zip file
            If v_bDeleteZipFile Then
                If File.Exists(sZipFile) Then
                    'UPGRADE_WARNING: (2081) DeleteFile has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2081.aspx
                    File.Delete(sZipFile)
                End If
            End If

            oFile = Nothing
            oFolder = Nothing
            oFSO = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Clean up
            If oZipper Is Nothing Then
            Else
                oZipper = Nothing
            End If
            If oFile Is Nothing Then
            Else
                oFile = Nothing
            End If
            If oFolder Is Nothing Then
            Else
                oFolder = Nothing
            End If
            If oFSO Is Nothing Then
            Else
                oFSO = Nothing
            End If

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unzip the document. " & Strings.Chr(13) & Strings.Chr(10) & "Zip File [" & sZipFile & "]" & Strings.Chr(13) & Strings.Chr(10) & "sOutputDirectory [" & sOutputDirectory & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Developer Guide No 32

            Return result
        End Try
    End Function
End Class
