Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
'Friend Partial Class frmCopyDocumentLink
Public Class frmCopyDocumentLink
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmCopyDocumentLink"
    Public bMessageShow As Boolean = True

    Private m_lProductID As Integer
    Private m_lDocumentTemplateID As Integer
    Private m_iFunctionalArea As Integer
    Private m_lProcessID As Integer
    Private m_lSourceID As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    Public WriteOnly Property FunctionalArea() As Integer
        Set(ByVal Value As Integer)

            m_iFunctionalArea = Value

        End Set
    End Property

    Public WriteOnly Property ProcessID() As Integer
        Set(ByVal Value As Integer)

            m_lProcessID = Value

        End Set
    End Property
    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)

            m_lSourceID = Value

        End Set
    End Property

    Public Property ProductID() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)

            m_lProductID = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property DocumentTemplateID() As Integer
        Get
            Return m_lDocumentTemplateID
        End Get
    End Property

    Private Sub cboProduct_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProduct.Click
        Dim sMessage As String = ""
        Dim iMsgResult As DialogResult

        Const kMethodName As String = "cboProduct_Click"
        Try




            m_lReturn = GetSFIDocumentTemplates()
            If bMessageShow = True Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocFound, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    iMsgResult = MessageBox.Show(sMessage, "Document Link", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

        End Try
    End Sub


    Private Sub frmCopyDocumentLink_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        'Developer Guide No. 38
        If (cboProduct.ListCount = 0) Then
            bMessageShow = False
            cboProduct.FirstItem = ""
        End If
        m_lReturn = GetSFIDocumentTemplates()
        bMessageShow = True
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

    End Sub

    Private Function GetSFIDocumentTemplates() As Integer

        Dim result As Integer = 0
        Dim lDocumentTemplateID, lProductID As Integer
        Dim vResultArray(,) As Object

        Const kMethodName As String = "GetSFIDocumentTemplates"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            lProductID = cboProduct.ItemId

            cboLinkedDocument.Items.Clear()

            m_lReturn = g_oBusiness.GetSFIDocumentTemplates(v_iFunctionalArea:=m_iFunctionalArea, v_lProductID:=lProductID, v_iProcessTypeID:=m_lProcessID, v_lSourceID:=m_lSourceID, r_vResultarray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If

            Dim cboLinkedDocument_NewIndex As Integer = -1
            cboLinkedDocument_NewIndex = 0
            cboLinkedDocument.Items.Insert(cboLinkedDocument_NewIndex, "(All Documents)")


            For lCnt As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                cboLinkedDocument_NewIndex = cboLinkedDocument.Items.Add(CStr(vResultArray(1, lCnt)))

                VB6.SetItemData(cboLinkedDocument, cboLinkedDocument_NewIndex, CInt(vResultArray(0, lCnt)))
            Next

            cboLinkedDocument.SelectedIndex = 0



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (SetInputControls) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SetInputControls() As Integer
    'Dim result As Integer = 0
    'Dim iIndex As Integer
    '
    'On Error GoTo Catch_Renamed
    '
    'Const kMethodName As String = "SetInputControls"
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'iIndex = 1
    '
    'cboProduct.TabIndex = iIndex
    'iIndex += 1
    'cboLinkedDocument.TabIndex = iIndex
    'iIndex += 1
    'cmdOK.TabIndex = iIndex
    'iIndex += 1
    'cmdCancel.TabIndex = iIndex
    '
    'cboProduct.ItemId = m_lProductID
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn)
    '
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    '
    'Return result
    'End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        m_lReturn = ProcessCommand()
    End Sub


    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer


        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String
        Const kMethodName As String = "ProcessCommand"

        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                m_lProductID = 0
                m_lDocumentTemplateID = 0
                Me.Close()
                Return result
            End If
        End If

        If m_lStatus = gPMConstants.PMEReturnCode.PMOK Then
            m_lProductID = cboProduct.ItemId
            If cboLinkedDocument.SelectedIndex = -1 Then


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocFound, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                iMsgResult = MessageBox.Show(sMessage, "Document Link", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)

		Return result

            End If

            m_lDocumentTemplateID = VB6.GetItemData(cboLinkedDocument, cboLinkedDocument.SelectedIndex)

            Me.Close()
        End If


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

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        m_lReturn = ProcessCommand()
    End Sub

 
  
    
End Class
