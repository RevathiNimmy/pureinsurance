Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 08/09/1998
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' CJB 180405 PN17391 Changed Form_KeyPress to not allow "]" key for consistency as "[" isn't allowed.
    ' CJB 200705 PN22509 Undo changes that made subject mandatory, position to 'Default Subject' or 1st
    '            one in cbo if not found (in DisplayLookupDetails ). Pass null for subject id in AddNoteEvent if not set.
    ' CJB 240805 PN23456 Changed BusinessToInterface to allow the description to be updated (and so overwritten) for Warnings
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"


    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Free Form Text Type
    Private m_lKeyFieldValue As Integer

    Private m_lPartyCnt As Integer
    Private m_dtNoteDate As Date

    Private m_sTextLine As String = ""
    Private m_vTextSet As Object
    Private m_iRecordCount As Integer
    Private m_sEntityName As String = ""

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    Private m_oFreeFormText As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    Private m_vNoteEventTypeArray(,) As Object
    ' InsuranceFileCnt
    Private m_lInsuranceFileCnt As Integer
    ' InsuranceFolderCnt
    Private m_lInsuranceFolderCnt As Integer
    ' ClaimCnt
    Private m_lClaimCnt As Integer
    ' Context
    Private m_sContext As String = ""
    ' EventTypeId
    Private m_lEventTypeId As Integer
    ' Description
    Private m_sDescription As String = ""
    Private m_sOriginalDesc As String = ""
    ' EventLogSubjectId
    Private m_lEventLogSubjectId As Integer
    ' AccountKey
    Private m_lAccountKey As Integer
    ' UserName
    Private m_sUserName As String = ""

    Private m_sPriorityCode As String = ""
    Private m_iIsCompleted As Integer
    Private m_bCompleted As Boolean
    Private m_lEventCnt As Integer
    Private m_bAddSticky As Boolean
    Private m_sSubjectDesc As String = ""
    Private m_sTypeDesc As String = ""

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_bRTFNotes As Boolean
    Private m_sRTFText As String = ""
    Private m_lCaseID As Integer
    Private m_sClaimRef As String = ""
    Private m_lBaseClaimid As Integer

    Public WriteOnly Property UserName() As String
        Set(ByVal Value As String)
            m_sUserName = Value
        End Set
    End Property
    Public WriteOnly Property AccountKey() As Integer
        Set(ByVal Value As Integer)
            m_lAccountKey = Value
        End Set
    End Property
    Public Property EventLogSubjectId() As Integer
        Get
            Return m_lEventLogSubjectId
        End Get
        Set(ByVal Value As Integer)
            m_lEventLogSubjectId = Value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property
    '2005 StickyNotes
    Public ReadOnly Property SubjectDesc() As String
        Get
            Return m_sSubjectDesc
        End Get
    End Property
    Public ReadOnly Property TypeDesc() As String
        Get
            Return m_sTypeDesc
        End Get
    End Property

    Public Property PriorityCode() As String
        Get
            Return m_sPriorityCode
        End Get
        Set(ByVal Value As String)
            m_sPriorityCode = Value
        End Set
    End Property
    Public Property IsCompleted() As Integer
        Get
            Return m_iIsCompleted
        End Get
        Set(ByVal Value As Integer)
            m_iIsCompleted = Value
        End Set
    End Property
    Public Property EventCnt() As Integer
        Get
            Return m_lEventCnt
        End Get
        Set(ByVal Value As Integer)
            m_lEventCnt = Value
        End Set
    End Property
    Public WriteOnly Property AddSticky() As Boolean
        Set(ByVal Value As Boolean)
            m_bAddSticky = Value
        End Set
    End Property
    '2005 StickyNotesEnd
    Public WriteOnly Property EventTypeId() As Integer
        Set(ByVal Value As Integer)
            m_lEventTypeId = Value
        End Set
    End Property
    Public WriteOnly Property Context() As String
        Set(ByVal Value As String)
            m_sContext = Value
        End Set
    End Property
    Public WriteOnly Property ClaimCnt() As Integer
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    'Standard Stuff
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
    Public WriteOnly Property NoteDate() As Date
        Set(ByVal Value As Date)
            m_dtNoteDate = Value
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

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus.Value
        End Get
    End Property

    Public Property RTFNotes() As Boolean
        Get
            Return m_bRTFNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bRTFNotes = Value
        End Set
    End Property

    Public Property RTFText() As String
        Get
            Return m_sRTFText
        End Get
        Set(ByVal Value As String)
            m_sRTFText = Value
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
    Public Property BaseClaimID() As Integer
        Get
            Return m_lBaseClaimid
        End Get
        Set(ByVal Value As Integer)
            m_lBaseClaimid = Value
        End Set
    End Property

    Public WriteOnly Property ClaimRef() As String
        Set(ByVal Value As String)
            m_sClaimRef = Value
        End Set
    End Property
    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try


            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>

    ' BusinessToInterface: Updates all interface details from the business object.

    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BusinessToInterface() As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim nIndex As Integer
        Dim nTextStart As Integer
        Dim nTextEnd As Integer
        Dim sCR As String
        Dim nPosition As Integer
        Dim nLength As Integer

        Dim sText As String = ""

        Dim sEventTypeCode As String = "" 'TF121103 - PN7730

        Try



            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboContext.Items.Add(m_sContext)
            cboContext.SelectedIndex = 0

            If m_bRTFNotes Then
                uctRichTextBox1.TextRTF = m_sRTFText
            Else
                txtFreeText.Text = m_sDescription
            End If

            'TF071103 - PN7730 - Get full text from Public_Text tables
            'Finished if not dealing with notes event
            If m_sContext.Substring(0, 5) <> "Notes" Then
                Return nResult
            End If

            '2005StickyNotes
            If m_sContext = "Notes - Customer Warning" Then

                cboContext.Enabled = False
                cboSubject.Enabled = False

                lblPriority.Visible = True
                cboPriority.Visible = True
                For nIndex = 0 To cboPriority.Items.Count - 1
                    If VB6.GetItemString(cboPriority, nIndex) = m_sPriorityCode Then
                        cboPriority.SelectedIndex = nIndex
                        Exit For
                    End If
                Next

                lblStatus.Visible = True
                cboStatus.Visible = True
                cboStatus.SelectedIndex = m_iIsCompleted

                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    cboPriority.Enabled = False
                    cboStatus.Enabled = False
                    txtFreeText.Enabled = False

                Else

                    If m_iIsCompleted = 1 Then
                        cboPriority.Enabled = False
                        cboStatus.Enabled = False
                        txtFreeText.Enabled = False
                    Else
                        cboPriority.Enabled = True
                        cboStatus.Enabled = True

                        ' Store the original description
                        m_sOriginalDesc = m_sDescription

                        ' Allow the description to be updated (+overwritten) for Warnings so extract
                        ' it from the user and timestamp  PN23456
                        If m_sContext = "Notes - Customer Warning" Then
                            nPosition = (m_sDescription.IndexOf("]"c) + 1)
                            If nPosition > 0 Then

                                ' Ignore the crlf
                                nPosition += 2

                                nLength = m_sDescription.Length - nPosition
                                m_sDescription = m_sDescription.Substring(m_sDescription.Length - nLength)

                            End If
                            txtFreeText.Text = m_sDescription
                        Else
                            txtFreeText.Text = ""
                        End If

                        txtFreeText.Enabled = True
                    End If
                End If

            End If

            'Need to populate EventType array

            m_lReturn = m_oBusiness.GetNoteEventType(r_vNoteEventTypeArray:=m_vNoteEventTypeArray, r_iListIndex:=nIndex, _
                                                     v_vInsuranceFileCnt:=m_lInsuranceFileCnt, v_vClaimCnt:=m_lClaimCnt, _
                                                     v_vAccountKey:=m_lAccountKey, v_vCaseID:=m_lCaseID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to process m_oBusiness.GetNoteEventType().", _
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface ")
                Return nResult
            End If

            'Get existing FreeForm Text
            sEventTypeCode = CStr(m_vNoteEventTypeArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, nIndex)).Trim().ToUpper()

            m_lReturn = GetFreeFormTextFromDB(v_sEventTypeCode:=sEventTypeCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to process GetFreeFormTextFromDB().", _
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface ")
                Return nResult
            End If

            'Exit if no existing text or doing Event Notes
            If (Not Information.IsArray(m_vTextSet)) Or (m_sEntityName = "") Then
                Return nResult
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                'Get all text and organise by date order descending
                sCR = Strings.Chr(13).ToString() & Strings.Chr(10).ToString()
                txtFreeText.Text = ""

                'Start from the end of the collection

                nTextStart = m_vTextSet.GetUpperBound(1)
                nTextEnd = nTextStart

                Do While nTextStart >= 0

                    'Check for text header

                    sText = CStr(m_vTextSet(2, nTextStart)).Trim()
                    'If sText.Length > 2 Then
                    '	sText = sText.Substring(0, sText.Length - 2)
                    'End If

                    If sText.Substring(0, 1) = "[" And sText.EndsWith("]") Then
                        'Get Text Entry
                        For nIndex = nTextStart To nTextEnd

                            txtFreeText.Text = txtFreeText.Text & CStr(m_vTextSet(2, nIndex)).Trim()
                            txtFreeText.Text = txtFreeText.Text & sCR
                        Next nIndex
                        txtFreeText.Text = txtFreeText.Text & sCR


                        'Next FreeText Entry
                        nTextStart -= 1
                        nTextEnd = nTextStart
                    Else
                        'This free text entry is over more than one line
                        nTextStart -= 1
                    End If
                Loop
            End If

            m_sDescription = txtFreeText.Text
            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

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
        Dim sEventTypeCode As String = ""

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

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    m_lReturn = AddNoteEvent(r_sEventTypeCode:=sEventTypeCode)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    '2005 Assume call from Sticky Note
                    m_lReturn = UpdateNoteEvent()

                    '2005 Sticky Notes Only update when status changed to Completed
                    If m_bCompleted Then
                        m_lReturn = CompleteNoteEvent()
                    End If
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            ' Now write description to Public_Text tables
            'Code built from iPMBFreeFormText

            'Get existing FreeForm Text
            m_lReturn = GetFreeFormTextFromDB(v_sEventTypeCode:=sEventTypeCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to process GetFreeFormTextFromDB().", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                Return result
            End If

            'Doesn't apply to Event Note (already done elsewhere)
            If (m_sEntityName = "") Or (m_iTask <> gPMConstants.PMEComponentAction.PMAdd) Then
                Return result
            End If

            m_lReturn = PrepareText()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process PrepareText().", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            'Business ID = count of text lines plus 1 (next TEXT ID)
            lBusinessDataID = m_iRecordCount + 1


            For iIndex As Integer = 1 To m_vTextSet.Count


                'developer guide no.98
                m_lReturn = m_oFreeFormText.EditAdd(lRow:=lBusinessDataID, vInsuranceFileCnt:=m_lKeyFieldValue, vTextLine:=m_vTextSet(iIndex))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process m_oFreeFormText.EditAdd().", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    m_vTextSet = Nothing
                    Exit For
                End If

                lBusinessDataID += 1
            Next iIndex


            m_lReturn = m_oFreeFormText.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process m_oFreeFormText.Update.", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            m_vTextSet = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AddNoteEvent
    '
    ' Description:
    '
    ' History: 30/09/2002 SJ - Created.
    '           TF121103    PN7730 - Only store 1st 1000 characters of multi-line text
    '                       Return EventTypeCode in case it has been changed
    ' 26/05/2005            Sticky Notes - return New Event Cnt
    ' ***************************************************************** '
    Private Function AddNoteEvent(ByRef r_sEventTypeCode As String) As Integer
        Dim result As Integer = 0

        Dim sDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim vEventLogSubjectId As Object
            Dim lEventType, lArrayIndex As Integer
            '2005 StickyNotes
            Dim sPriorityCode As String = ""
            Dim iIsCompleted As Integer
            Dim lClaimCnt As Integer


            Dim oBusinessFindClaim As bCLMFindClaim.Business
            Dim vResultArray(,) As Object = Nothing



            lArrayIndex = VB6.GetItemData(cboContext, cboContext.SelectedIndex)
            lEventType = CInt(Conversion.Val(CStr(m_vNoteEventTypeArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lArrayIndex))))
            r_sEventTypeCode = CStr(m_vNoteEventTypeArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lArrayIndex)).Trim().ToUpper()

            If cboSubject.SelectedIndex = -1 Then 'PN22509

                vEventLogSubjectId = Nothing

            Else
                vEventLogSubjectId = VB6.GetItemData(cboSubject, cboSubject.SelectedIndex)
            End If

            '2005 StickyNotes
            If r_sEventTypeCode = "N_WARN" Then
                sPriorityCode = cboPriority.Text
                iIsCompleted = m_iIsCompleted '   cboStatus.ItemData(cboStatus.ListIndex)
            End If
            lClaimCnt = m_lClaimCnt
            m_lClaimCnt = IIf(m_lClaimCnt > 0, m_lClaimCnt, m_lBaseClaimid)
            'These need re-confirming in case alternative selected in cboContext
            m_lKeyFieldValue = m_lPartyCnt
            RTFText = uctRichTextBox1.TextRTF

            If r_sEventTypeCode = gSIRLibrary.ACNotesClaims Then

                Dim temp_oBusinessFindClaim As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusinessFindClaim, "bCLMFindClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oBusinessFindClaim = temp_oBusinessFindClaim


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If


                m_lReturn = oBusinessFindClaim.GetClaimDetailsUW(r_vResultArray:=vResultArray, v_vsiriusproduct:="", v_vClaimNumber:=gPMFunctions.ToSafeString(m_sClaimRef), v_vClientName:="", v_vPolicyNumber:="", v_vRegNumber:="", v_vLossFromDate:="", v_vLossToDate:="", v_vClaimStatus:="", v_lCaseID:=m_lCaseID)

                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    'Don't Raise an Error _
                    'It is an Open Claim (with is_dirty=0)

                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oBusinessFindClaim.GetClaimDetailsUW Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNoteEvent")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vResultArray) Then

                    m_lInsuranceFileCnt = CInt(vResultArray(0, 0))

                    m_lPartyCnt = CInt(vResultArray(23, 0))

                    m_lInsuranceFolderCnt = CInt(vResultArray(24, 0))
                    'm_sClaimRef = vResultArray(3, 0)
                End If
            End If

            If r_sEventTypeCode = gSIRLibrary.ACNotesClaims Then
                If m_lCaseID > 0 Then
                    'PN48632
                    If m_lInsuranceFolderCnt > 0 And m_lInsuranceFileCnt > 0 Then


                        m_lReturn = m_oBusiness.DirectAdd(vPartyCnt:=m_lPartyCnt, vInsuranceFolderCnt:=m_lInsuranceFolderCnt, vInsuranceFileCnt:=m_lInsuranceFileCnt, vClaimCnt:=m_lClaimCnt, vEventType:=lEventType, vDescription:=GetDescription(m_sDesc:=m_sDescription), vEventLogSubjectId:=vEventLogSubjectId, vCaseID:=m_lCaseID, v_vRTFText:=RTFText)
                    Else

                        ' If m_sDescription.Length > 1000 Then
                        m_lReturn = m_oBusiness.DirectAdd(vPartyCnt:=m_lPartyCnt, vClaimCnt:=m_lClaimCnt, vEventType:=lEventType, vDescription:=GetDescription(m_sDesc:=m_sDescription), vEventLogSubjectId:=vEventLogSubjectId, vCaseID:=m_lCaseID, v_vRTFText:=RTFText)
                        ' Else
                        '  m_lReturn = m_oBusiness.DirectAdd(vPartyCnt:=m_lPartyCnt, vClaimCnt:=m_lClaimCnt, vEventType:=lEventType, vDescription:=m_sDescription, vEventLogSubjectId:=vEventLogSubjectId, vCaseID:=m_lCaseID, v_vRTFText:=RTFText)

                        'End If
                    End If
                Else

                    m_lReturn = m_oBusiness.DirectAdd(vPartyCnt:=m_lPartyCnt, vInsuranceFolderCnt:=m_lInsuranceFolderCnt, vInsuranceFileCnt:=m_lInsuranceFileCnt, vClaimCnt:=m_lClaimCnt, vEventType:=lEventType, vDescription:=GetDescription(m_sDesc:=m_sDescription), vEventLogSubjectId:=vEventLogSubjectId, v_vRTFText:=RTFText)
                End If

            ElseIf r_sEventTypeCode = gSIRLibrary.ACNotesPolicy Then
                If m_lClaimCnt > 0 Then

                    m_lReturn = m_oBusiness.DirectAdd(vPartyCnt:=m_lPartyCnt, vInsuranceFolderCnt:=m_lInsuranceFolderCnt, vInsuranceFileCnt:=m_lInsuranceFileCnt, vClaimCnt:=m_lClaimCnt, vEventType:=lEventType, vDescription:=GetDescription(m_sDesc:=m_sDescription), vEventLogSubjectId:=vEventLogSubjectId, v_vRTFText:=RTFText)
                Else

                    m_lReturn = m_oBusiness.DirectAdd(vPartyCnt:=m_lPartyCnt, vInsuranceFolderCnt:=m_lInsuranceFolderCnt, vInsuranceFileCnt:=m_lInsuranceFileCnt, vEventType:=lEventType, vDescription:=GetDescription(m_sDesc:=m_sDescription), vEventLogSubjectId:=vEventLogSubjectId, v_vRTFText:=RTFText)
                End If
            ElseIf r_sEventTypeCode = gSIRLibrary.ACNotesAccount Then

                m_lReturn = m_oBusiness.DirectAdd(vPartyCnt:=m_lPartyCnt, vAccountKey:=m_lAccountKey, vEventType:=lEventType, vDescription:=GetDescription(m_sDesc:=m_sDescription), vEventLogSubjectId:=vEventLogSubjectId, v_vRTFText:=RTFText)
            Else

                If m_lCaseID > 0 Then
                    If m_lPartyCnt > 0 And m_lInsuranceFolderCnt > 0 And m_lInsuranceFileCnt > 0 Then

                        m_lReturn = m_oBusiness.DirectAdd(vEventCnt:=m_lEventCnt, vPartyCnt:=m_lPartyCnt, vInsuranceFolderCnt:=m_lInsuranceFolderCnt, vInsuranceFileCnt:=m_lInsuranceFileCnt, vClaimCnt:=m_lClaimCnt, vEventType:=lEventType, vDescription:=GetDescription(m_sDesc:=m_sDescription), vEventLogSubjectId:=vEventLogSubjectId, vPriorityCode:=sPriorityCode, vIsCompleted:=gPMFunctions.ToSafeString(iIsCompleted), vCaseID:=m_lCaseID, v_vRTFText:=RTFText)
                    Else

                        m_lReturn = m_oBusiness.DirectAdd(vEventCnt:=m_lEventCnt, vPartyCnt:=m_lPartyCnt, vEventType:=lEventType, vDescription:=GetDescription(m_sDesc:=m_sDescription), vEventLogSubjectId:=vEventLogSubjectId, vPriorityCode:=sPriorityCode, vIsCompleted:=gPMFunctions.ToSafeString(iIsCompleted), vCaseID:=m_lCaseID, v_vRTFText:=RTFText)

                    End If
                Else
                    'developer guide no.40
                    m_sDescription = "[" & m_sUserName & " - " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, DateTime.Now) & "]" & Strings.Chr(13) & Strings.Chr(10) & m_sDescription

                    '2005 StickyNotes Return priority code & iscompleted ind
                    If m_lPartyCnt > 0 And m_lInsuranceFolderCnt > 0 And m_lInsuranceFileCnt > 0 Then

                        m_lReturn = m_oBusiness.DirectAdd(vEventCnt:=m_lEventCnt, vPartyCnt:=m_lPartyCnt, vInsuranceFolderCnt:=m_lInsuranceFolderCnt, vInsuranceFileCnt:=m_lInsuranceFileCnt, vClaimCnt:=m_lClaimCnt, vEventType:=lEventType, vDescription:=GetDescription(m_sDesc:=m_sDescription), vEventLogSubjectId:=vEventLogSubjectId, vPriorityCode:=sPriorityCode, vIsCompleted:=gPMFunctions.ToSafeString(iIsCompleted), v_vRTFText:=RTFText)
                    Else

                        m_lReturn = m_oBusiness.DirectAdd(vEventCnt:=m_lEventCnt, vPartyCnt:=m_lPartyCnt, _
                                                          vClaimCnt:=m_lClaimCnt, vEventType:=lEventType, _
                                                          vDescription:=GetDescription(m_sDesc:=m_sDescription), _
                                                          vEventLogSubjectId:=vEventLogSubjectId, vPriorityCode:=sPriorityCode, _
                                                          vIsCompleted:=gPMFunctions.ToSafeString(iIsCompleted), v_vRTFText:=RTFText)
                    End If
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNoteEvent")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not (oBusinessFindClaim Is Nothing) Then
                ' Remove the instance of the object
                oBusinessFindClaim.Dispose()
            End If
            oBusinessFindClaim = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddNoteEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNoteEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateNoteEvent
    '
    ' Description:
    '
    ' History: 26/04/2005 ECK Update Description & Status on Sticky Notes
    ' ***************************************************************** '
    Private Function UpdateNoteEvent() As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no.40
            m_sDescription = "[" & m_sUserName & " - " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateTimeLong, DateTime.Now) & "]" & Strings.Chr(13) & Strings.Chr(10) & m_sDescription


            m_oBusiness.EventCnt = m_lEventCnt

            m_lReturn = m_oBusiness.UpdateWarning(v_vDescription:=GetDescription(m_sDesc:=m_sDescription), v_vIsCompleted:=m_iIsCompleted, v_vPriority:=m_sPriorityCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.UpdateNoteEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateNoteEvent")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateNoteEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateNoteEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CompleteNoteEvent
    '
    ' Description:
    '
    ' History: 12/04/2005 ECK Update Incomplete status on Stick Notes
    ' ***************************************************************** '
    Private Function CompleteNoteEvent() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oBusiness.EventCnt = m_lEventCnt

            m_lReturn = m_oBusiness.SetCompleted()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.CompleteNoteEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CompleteNoteEvent")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CompleteNoteEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CompleteNoteEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing
            Dim iListIndex As Integer

            ' Get the lookup values.
            'Event Log Subject
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = m_oBusiness.GetEventLogSubjectList(r_vResultArray:=vResultArray)
            Else

                m_lReturn = m_oBusiness.GetEventLogSubjectList(r_vResultArray:=vResultArray, v_vEventLogSubjectId:=m_lEventLogSubjectId)
            End If

            If Information.IsArray(vResultArray) Then
                iListIndex = 0

                For i As Integer = 0 To vResultArray.GetUpperBound(1)
                    Dim cboSubject_NewIndex As Integer = -1

                    cboSubject_NewIndex = cboSubject.Items.Add(CStr(vResultArray(1, i)))

                    VB6.SetItemData(cboSubject, cboSubject_NewIndex, CInt(vResultArray(0, i)))
                    ' Position to default subject, or first if not found  PN22509

                    If CStr(vResultArray(1, i)).Trim().ToUpper() = "DEFAULT SUBJECT" Then
                        iListIndex = i
                    End If
                Next i
                cboSubject.SelectedIndex = iListIndex
            End If

            'Event Type
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then


                m_lReturn = m_oBusiness.GetNoteEventType(r_vNoteEventTypeArray:=m_vNoteEventTypeArray, r_iListIndex:=iListIndex, v_vInsuranceFileCnt:=m_lInsuranceFileCnt, v_vClaimCnt:=m_lClaimCnt, v_vAccountKey:=m_lAccountKey, v_vCaseID:=m_lCaseID, v_lBaseClaimID:=m_lBaseClaimid, v_bAddSticky:=m_bAddSticky)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetNoteEventType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
                    Return result
                End If
                cboContext.Items.Clear()

                '2005 StickyNotes added empty default
                Dim cboContext_NewIndex As Integer = -1
                cboContext_NewIndex = cboContext.Items.Add("EMPTY")
                VB6.SetItemData(cboContext, cboContext_NewIndex, 0)
                If Information.IsArray(m_vNoteEventTypeArray) Then
                    For i As Integer = 0 To m_vNoteEventTypeArray.GetUpperBound(1)
                        cboContext_NewIndex = cboContext.Items.Add(CStr(m_vNoteEventTypeArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, i)))
                        VB6.SetItemData(cboContext, cboContext_NewIndex, i)
                        If m_bAddSticky Then
                            If CStr(m_vNoteEventTypeArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, i)).Trim().ToUpper() = "N_WARN" Then
                                cboContext.SelectedIndex = cboContext_NewIndex
                            End If
                        Else
                            cboContext.SelectedIndex = 0
                        End If
                    Next i
                End If
            End If

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
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            'Check that text has been entered
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                If txtFreeText.Text = "" Then
                    m_sDescription = ""
                Else
                    'Start(Sriram P)55812
                    m_sDescription = IIf(m_bRTFNotes, uctRichTextBox1.Text, txtFreeText.Text)
                    'End(Sriram P)55812
                End If
            Else
                If txtFreeText.Text = "" And (m_bRTFNotes And uctRichTextBox1.Text = "") Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf m_sTransactionType = "C_EC" Then
                    If Not String.IsNullOrEmpty(Convert.ToString(uctRichTextBox1.Text)) Then
                        m_sDescription = uctRichTextBox1.Text
                    ElseIf Not String.IsNullOrEmpty(Convert.ToString(txtFreeText.Text)) Then
                        m_sDescription = txtFreeText.Text

                    End If
                Else
                    'DD 29/09/2003: Pass back the entered text
                    If m_bRTFNotes Then
                        m_sDescription = uctRichTextBox1.Text
                    Else
                        m_sDescription = txtFreeText.Text
                    End If
                End If
            End If

            '2005StickyNotes
            If cboStatus.SelectedIndex <> -1 Then
                If cboStatus.Text = "Completed" Then
                    IsCompleted = 1
                    m_bCompleted = True
                Else
                    IsCompleted = 0
                    m_bCompleted = False
                End If
            End If

            If cboSubject.SelectedIndex <> -1 Then
                m_sSubjectDesc = cboSubject.Text
                m_lEventLogSubjectId = VB6.GetItemData(cboSubject, cboSubject.SelectedIndex)
            End If
            If cboContext.SelectedIndex <> -1 Then
                m_sTypeDesc = cboContext.Text
            End If
            If cboPriority.SelectedIndex <> -1 Then
                m_sPriorityCode = cboPriority.Text
            End If
            '2005StickyNotesEnd

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            '2005 StickyNotes
            lblPriority.Visible = False
            cboPriority.Visible = False
            cboPriority.SelectedIndex = 0
            lblStatus.Visible = False
            cboStatus.Visible = False
            cboStatus.SelectedIndex = 0

            If m_bRTFNotes Then
                uctRichTextBox1.Visible = True
                uctRichTextBox1.ShowToolbar = True
                uctRichTextBox1.SpellCheck = True
                txtFreeText.Visible = False
            Else
                uctRichTextBox1.Visible = False
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblContext.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabelContext, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSubject.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabelSubject, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_bRTFNotes Then

                lblFreeText.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

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
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Private Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Or m_iTask = gPMConstants.PMEComponentAction.PMView Then
                ' Get the interface details from the
                ' business object.
                m_lReturn = GetBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return m_lReturn
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

            txtUser.Text = m_sUserName.Trim()
            txtDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, m_dtNoteDate)

            ' Check the task.
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(lDisabled:=True)
            Else
                'Enable form
                m_lReturn = DisableForm(lDisabled:=False)
            End If

            m_lReturn = DisplayLookupDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '2005 StickyNotes
            If m_bAddSticky Or (m_sContext = "Notes - Customer Warning") Then
                cboContext.Enabled = False
                cboSubject.Enabled = m_bAddSticky
                lblPriority.Visible = True
                cboPriority.Visible = True
                lblStatus.Visible = True
                cboStatus.Visible = True
                txtFreeText.Enabled = True
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            txtFreeText.ReadOnly = lDisabled
            cboSubject.Enabled = Not lDisabled
            cboContext.Enabled = Not lDisabled
            uctRichTextBox1.Locked = lDisabled
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)


    Private Sub cboContext_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboContext.SelectedIndexChanged
        If cboContext.Text = "Notes - Claims" And m_lCaseID > 0 And Not (m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CR") And (m_lClaimCnt = 0 And m_lBaseClaimid = 0) Then
            cmdOpenClaim.Visible = True
            txtclaim.Visible = True
        Else
            cmdOpenClaim.Visible = False
            txtclaim.Visible = False
        End If
    End Sub

    Private Sub cboContext_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboContext.Leave

        If cboContext.Text = "Notes - Customer Warning" Then
            lblPriority.Visible = True
            cboPriority.Visible = True
            lblStatus.Visible = True
            cboStatus.Visible = True
            txtFreeText.Enabled = True
            cmdOpenClaim.Visible = False
            txtclaim.Visible = False
        ElseIf cboContext.Text = "Notes - Claims" And m_lCaseID > 0 And (m_lClaimCnt = 0 And m_lBaseClaimid = 0) Then
            cmdOpenClaim.Visible = True
            txtclaim.Visible = True
        Else
            lblPriority.Visible = False
            cboPriority.Visible = False
            lblStatus.Visible = False
            cboStatus.Visible = False
            txtFreeText.Enabled = True
            cmdOpenClaim.Visible = False
            txtclaim.Visible = False
        End If

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        'developer guide no. 20
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub cmdOpenClaim_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOpenClaim.Click
        m_lReturn = GetClaim(r_lClaimID:=m_lClaimCnt, r_sClaimRef:=m_sClaimRef)
        If m_sClaimRef.Trim() <> "" Then
            txtclaim.Text = m_sClaimRef
        End If
    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            uctPMResizer1.SetControlResizeOption(v_sControlName:="cmdOK", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="cmdCancel", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="cmdHelp", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            uctPMResizer1.SetControlResizeOption(v_sControlName:="cboContext", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROWidthOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="cboSubject", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROWidthOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            uctPMResizer1.SetControlResizeOption(v_sControlName:="lblContext", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="lblSubject", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="lblText", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="lblDate", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="lblUser", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            uctPMResizer1.SetControlResizeOption(v_sControlName:="txtDate", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="txtUser", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            uctPMResizer1.SetControlResizeOption(v_sControlName:="txtFreeText", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="tabMainTab", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="lblFreeText", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            uctPMResizer1.SetControlResizeOption(v_sControlName:="cboPriority", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROWidthOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="cboStatus", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROWidthOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            uctPMResizer1.SetControlResizeOption(v_sControlName:="lblPriority", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption(v_sControlName:="lblStatus", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            uctPMResizer1.SetControlResizeOption(v_sControlName:="uctRichTextBox1", v_lResizeOption:=PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, v_lResizeType:=PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

        End If
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIREvent.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID


            Dim temp_m_oFreeFormText As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFreeFormText, "bSIRFreeFormText.Business", vInstanceManager:="ClientManager")
            m_oFreeFormText = temp_m_oFreeFormText
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create instance of bSirFreeFormText.Business.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialize")
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        If (KeyAscii = Strings.Asc("["c)) Or (KeyAscii = Strings.Asc("]"c)) Then
            'Do not allow user to press "[" or "]"  PN17391
            KeyAscii = 0
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ''' <summary>
    ''' frmInterface_Load
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the status for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = GetInterfaceDetails()

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Task = gPMConstants.PMEComponentAction.PMAdd
                txtFreeText.Text = ""
                'txtFreeText.SetFocus
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView And m_sContext = "Notes - Claims" Then
                cmdOpenClaim.Visible = True
                txtclaim.Visible = True
                cmdOpenClaim.Enabled = False
                txtclaim.Enabled = False
                txtclaim.Text = m_sClaimRef

            End If
            If m_sTransactionType = "C_EC" Then
                txtFreeText.Text = m_sDescription
            End If
            'uctRichTextBox1.Text = m_sDescription

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Terminate the business object

            m_oBusiness.Dispose()

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the FreeFormText business object

            m_oFreeFormText.Dispose()

            ' Destroy the instance of the business object
            ' from memory.
            m_oFreeFormText = Nothing

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Try

            If Trim(uctRichTextBox1.Text) = "" AndAlso Trim(txtFreeText.Text) = "" Then
                MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Text", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Else
                If Trim(uctRichTextBox1.Text) <> "" Then
                    txtFreeText.Text = uctRichTextBox1.Text
                End If
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If cboContext.Text = "Notes - Claims" And m_lCaseID > 0 And (m_lClaimCnt = 0 And m_lBaseClaimid = 0) Then
                If txtclaim.Text.Trim() = "" Then
                    MessageBox.Show("Please enter claim number", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtclaim.Focus()
                    Exit Sub
                End If
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = InterfaceToBusiness()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            Me.Hide()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    ''' <summary>
    ''' PrepareText : Copied from iPMBNote.InterfaceToData
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PrepareText() As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            Dim nTxtPointer As Integer
            Dim sTextLine As String = ""



            ' Update the data storage.
            If Not m_bRTFNotes Then
                txtFreeText.Text = uctRichTextBox1.Text
            End If
            'Check that text has been entered
            If txtFreeText.Text = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'New collection of text
            m_vTextSet = Nothing
            'developer guide no.(As per VB Code)
            m_vTextSet = New Collection()

            'Add Username and date/time of creation as first line
            'developer guide no.40
            sTextLine = "[" & m_sUserName.ToUpper() & " " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, DateTime.Now) & "]" & Strings.Chr(13) & Strings.Chr(10)


            m_vTextSet.Add(sTextLine)

            'Break up the entered free text into a collection of TextLines
            nTxtPointer = 1
            While nTxtPointer < Strings.Len(txtFreeText.Text)
                sTextLine = txtFreeText.Text.Substring(nTxtPointer - 1, Math.Min(txtFreeText.Text.Length - (nTxtPointer - 1), 255))

                m_vTextSet.Add(sTextLine)
                nTxtPointer += 255
            End While

            ' {* USER DEFINED CODE (End) *}

            Return nResult

        Catch excep As System.Exception




            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="PrepareText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFreeFormTextFromDB
    '
    ' Description: TF071103 - PN7730
    '              Retrieve existing Free Form text for given entity
    '
    ' ***************************************************************** '
    Private Function GetFreeFormTextFromDB(ByVal v_sEventTypeCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sEntityName = ""

            If v_sEventTypeCode = gSIRLibrary.ACNotesClaims Then
                m_lKeyFieldValue = m_lClaimCnt
                m_sEntityName = gSIRLibrary.SIREntityNameClaim
            ElseIf (v_sEventTypeCode = gSIRLibrary.ACNotesPolicy) Then
                m_lKeyFieldValue = m_lInsuranceFileCnt
                m_sEntityName = gSIRLibrary.SIREntityNamePolicy
            ElseIf (v_sEventTypeCode = gSIRLibrary.ACNotesAccount) Then
                m_lKeyFieldValue = m_lAccountKey
                m_sEntityName = gSIRLibrary.SIREntityNameAccount
            ElseIf (v_sEventTypeCode = gSIRLibrary.ACNotesCustomer) Then
                m_lKeyFieldValue = m_lPartyCnt
                m_sEntityName = gSIRLibrary.SIREntityNameParty
            Else
                'Exit if Event Notes
                m_sEntityName = ""
                Return result
            End If

            'Use bSIRFreeFormText business object
            With m_oFreeFormText

                .KeyFieldValue = m_lKeyFieldValue

                .EntityName = m_sEntityName

                .Texttype = "public"

                .SQLSet()
            End With

            'Get full text array

            m_lReturn = m_oFreeFormText.GetDetails(m_lKeyFieldValue)
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process m_oFreeFormText.GetDetails().", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFreeFormTextFromDB")
                Return result
            End If


            m_vTextSet = m_oFreeFormText.TextSet
            If Not Information.IsArray(m_vTextSet) Then
                Return result
            End If

            'Organise the text

            m_iRecordCount = m_vTextSet.GetUpperBound(1) + 1

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve freeform text.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFreeFormTextFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' SetFieldValidation : Sets the rules for validating fields.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFieldValidation() As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try



            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            If Not m_bRTFNotes Then
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=uctRichTextBox1, lFieldType:=gPMConstants.PMEDataType.PMString, _
                                                          lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, _
                                                          lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            Else
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFreeText, lFieldType:=gPMConstants.PMEDataType.PMString, _
                                                            lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, _
                                                            lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetClaim
    '
    ' Parameters: n/a
    '
    ' Description: Function to fetch the claim
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function GetClaim(ByRef r_lClaimID As Integer, ByRef r_sClaimRef As String) As Integer
        Dim result As Integer = 0
        Dim ICLMFINDCLAIM As Object

        Const kMethodName As String = "GetClaim"

        Dim lReturn As Integer

        Dim oFindClaim As iCLMFindClaim.Interface_Renamed
        Dim vKeyArray As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oFindClaim As Object = Nothing
            lReturn = g_oObjectManager.GetInstance(temp_oFindClaim, sClassName:="iCLMFindClaim.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindClaim = temp_oFindClaim

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create object 'iCLMFindClaim.Interface'.", gPMConstants.PMELogLevel.PMLogError)
            End If


            oFindClaim.CaseID = m_lCaseID


            lReturn = oFindClaim.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start Find Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Check the status first

            If oFindClaim.Status <> gPMConstants.PMEReturnCode.PMCancel Then


                r_lClaimID = oFindClaim.ClaimCnt
                '      MsgBox oFindClaim.InsFileCnt

                r_sClaimRef = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=oFindClaim.ClaimRef.Trim())
                '      MsgBox FormatField(iFormatType:=PMFormatString, vFieldValue:=Trim$(oFindClaim.PolicyRef))
            End If

            ' Destroy Find Insurance object

            oFindClaim.Dispose()



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function
    'developer guide no.292 (Added the code to get the substring to maximum of 1000 characters)
    Function GetDescription(ByVal m_sDesc As String) As String
        Dim desc As String
        If m_sDescription.Length > 1000 Then
            desc = m_sDesc.Substring(0, 1000)
        Else
            desc = m_sDesc.Substring(0, m_sDesc.Length)
        End If
        Return desc
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                cmdCancel.Focus()
                cmdCancel.PerformClick()
        End Select
    End Sub
End Class
