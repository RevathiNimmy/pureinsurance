Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
'Friend Partial Class frmDocumentLinkEdit
Public Class frmDocumentLinkEdit
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmDocumentLinkEdit"

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lStatus As Integer
    Private m_lReturn As Integer

    'Developer Guide No. 108
    Private m_oFindDocs As iPMBFindDocTemplate.Interface_Renamed

    Private m_lProductID As Integer
    Private m_lDocLinkId As Integer
    Private m_iFunctionalArea As Integer
    Private m_lProcessID As Integer
    Private m_lSourceID As Integer
    Private m_lDocumentTemplateID As Integer
    Private m_lDocumentTypeID As Integer
    Private m_bEntryDone As Boolean
    Private m_sDocumentCode As String = ""
    Private m_sDocumentTemplateDescription As String = ""

    Private m_olvwSearchDetails As Object
    Private m_oFormFields As iPMFormControl.FormFields
    'Start-Written Status
    Private m_bWrittenPolicyStatus As Boolean
    'End Written Status-
    'Start- Written Status 
    Public Property WrittenPolicyStatus() As Boolean
        Get
            Return m_bWrittenPolicyStatus
        End Get
        Set(ByVal bWrittenPolicyStatus As Boolean)
            m_bWrittenPolicyStatus = bWrittenPolicyStatus
        End Set
    End Property
    'End- Written Status

    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            Return m_lStatus

        End Get
    End Property

    Public WriteOnly Property lvwSearchDetails() As Object
        Set(ByVal Value As Object)

            m_olvwSearchDetails = Value

        End Set
    End Property

    Public WriteOnly Property ProductID() As Integer
        Set(ByVal Value As Integer)

            m_lProductID = Value

        End Set
    End Property

    Public WriteOnly Property DocLinkId() As Integer
        Set(ByVal Value As Integer)

            m_lDocLinkId = Value

        End Set
    End Property

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

    Private Sub cboBranch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.Click
        m_bEntryDone = False
    End Sub

    Private Sub cboDocumentType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDocumentType.SelectedIndexChanged
        m_bEntryDone = False
    End Sub

    Private Sub cboProcess_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProcess.SelectionChangeCommitted
        m_bEntryDone = False
    End Sub

    Private Sub cboProcess_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cboProcess.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        KeyAscii = 0
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub chkDefault_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDefault.CheckStateChanged
        m_bEntryDone = False

        If chkDefault.CheckState = CheckState.Checked Then
            chkClient.CheckState = CheckState.Unchecked
            chkAgent.CheckState = CheckState.Unchecked
            chkOffice.CheckState = CheckState.Unchecked
        End If
        If chkAgent.CheckState = CheckState.Unchecked And chkClient.CheckState = CheckState.Unchecked And chkOffice.CheckState = CheckState.Unchecked Then
            chkDefault.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub chkAgent_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAgent.CheckStateChanged
        m_bEntryDone = False
        If chkAgent.CheckState = CheckState.Checked Then
            chkDefault.CheckState = CheckState.Unchecked
        End If

        If chkAgent.CheckState = CheckState.Unchecked And chkClient.CheckState = CheckState.Unchecked And chkOffice.CheckState = CheckState.Unchecked Then
            chkDefault.CheckState = CheckState.Checked
        End If

    End Sub

    Private Sub chkClient_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkClient.CheckStateChanged
        m_bEntryDone = False
        If chkClient.CheckState = CheckState.Checked Then
            chkDefault.CheckState = CheckState.Unchecked
        End If

        If chkAgent.CheckState = CheckState.Unchecked And chkClient.CheckState = CheckState.Unchecked And chkOffice.CheckState = CheckState.Unchecked Then
            chkDefault.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub chkOffice_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkOffice.CheckStateChanged

        m_bEntryDone = False

        If chkOffice.CheckState = CheckState.Checked Then
            chkDefault.CheckState = CheckState.Unchecked
        End If

        If chkAgent.CheckState = CheckState.Unchecked And chkClient.CheckState = CheckState.Unchecked And chkOffice.CheckState = CheckState.Unchecked Then
            chkDefault.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        m_lReturn = ProcessCommand()
    End Sub

    Private Sub cmdDeSelectDoc_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeSelectDoc.Click
        m_bEntryDone = False
        txtDocument.Text = ""
        txtDocument.Tag = CStr(0)
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        m_lReturn = ProcessCommand()
    End Sub

    Private Sub cmdSelectDoc_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectDoc.Click
        Dim sDocumentCode As String = ""

        m_bEntryDone = False

        m_lReturn = GetDocumentTemplate()

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            txtDocument.Text = m_sDocumentTemplateDescription
        End If

    End Sub


    Private Sub frmDocumentLinkEdit_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"
        Try



            'Developer Guide No. 38
            Me.cboBranch.FirstItem = "(All Branches)"
            SetInputControls()

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()


            m_lReturn = SetFieldValidation()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            'm_lReturn = GetLookUpList(v_sTableName:="process_type", r_cboControl:=cboProcess)
            m_lReturn = GetProcessType()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProcessType Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = GetProcessTypeDocs()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProcessTypeDocs  Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = SetItemId(m_lProcessID, cboProcess)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If

                cboBranch.ItemId = m_lSourceID
                chkBO.CheckState = CheckState.Checked
                chkSAM.CheckState = CheckState.Checked
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = LoadDataFromParentControl()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetDocumentTemplate
    '
    ' Description: Creates an instance of find document template and gets the properties
    '
    ' ***************************************************************** '
    Private Function GetDocumentTemplate() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDocumentTemplate"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            'Create iPMBFindDocTemplate object if not already done so
            If m_oFindDocs Is Nothing Then
                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oFindDocs As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindDocs, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFindDocs = temp_m_oFindDocs

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


            m_lReturn = m_oFindDocs.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Run it in Merge mode to hide some buttons

            m_oFindDocs.Mode = 1


            m_oFindDocs.DocumentTypeId = 0 'cboDocumentType.ItemId


            m_oFindDocs.ProductId = 0 'm_lProductID


            m_lReturn = m_oFindDocs.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Exit out if it was cancelled

            If m_oFindDocs.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
                Return result
            End If

            ' Get it's properties
            With m_oFindDocs

                m_lDocumentTemplateID = .DocumentTemplateId

                m_sDocumentCode = .DocumentCode

                m_lDocumentTypeID = .DocumentTypeId

                m_sDocumentTemplateDescription = .DocumentTemplateDescription
            End With



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally






        End Try
        Return result
    End Function

    Private Sub SetInputControls()

        Dim iIndex As Integer
        Dim option1 As String
        Const kMethodName As String = "SetInputControls"
        Try


            '--Start vivek 68514 and 66530
            m_lReturn = iPMFunc.GetSystemOption(5097, option1)

            If ToSafeInteger(option1) <> 1 Then
                'Set Default Values
                Me.OptSpooler.Checked = True
                '(Start)-(Arul Stephen)-(Document Configuration)
                Me.OptPrinter.Enabled = True
                Me.OptSpooler.Enabled = True
                '(End)-(Arul Stephen)-(Document Configuration)
                Me.chkDefault.CheckState = CheckState.Checked
                Me.KeyPreview = True
            ElseIf option1 = 1 Then
                'Set Default Values
                Me.OptSpooler.Checked = True
                '(Start)-(Arul Stephen)-(Document Configuration)
                Me.OptPrinter.Enabled = True
                Me.OptSpooler.Enabled = True
                '(End)-(Arul Stephen)-(Document Configuration)
                Me.OptUserChoice.Visible = False
                Me.OptUserChoice.Checked = False
                Me.OptUserChoice.Enabled = False
                Me.chkDefault.Checked = CheckState.Checked
                Me.KeyPreview = True
            End If
            '--END vivek 68514 and 66530
            iIndex = 1

            lblProcess.TabIndex = iIndex
            iIndex += 1
            cboProcess.TabIndex = iIndex
            iIndex += 1

            lblDocumetType.TabIndex = iIndex
            iIndex += 1
            cboDocumentType.TabIndex = iIndex
            iIndex += 1

            lblBranch.TabIndex = iIndex
            iIndex += 1
            cboBranch.TabIndex = iIndex
            iIndex += 1

            lblDocument.TabIndex = iIndex
            iIndex += 1
            txtDocument.TabIndex = iIndex
            iIndex += 1

            cmdSelectDoc.TabIndex = iIndex
            iIndex += 1

            cmdDeSelectDoc.TabIndex = iIndex
            iIndex += 1

            lblFor.TabIndex = iIndex
            iIndex += 1

            chkDefault.TabIndex = iIndex
            iIndex += 1

            chkClient.TabIndex = iIndex
            iIndex += 1

            chkAgent.TabIndex = iIndex
            iIndex += 1

            chkOffice.TabIndex = iIndex
            iIndex += 1

            lblSendTo.TabIndex = iIndex
            iIndex += 1

            OptPrinter.TabIndex = iIndex
            iIndex += 1

            OptSpooler.TabIndex = iIndex
            iIndex += 1
            '(Start)-(Arul Stephen)-(Document Configuration)
            OptUserChoice.TabIndex = iIndex
            iIndex += 1
            '(End)-(Arul Stephen)-(Document Configuration)
            cmdOk.TabIndex = iIndex
            iIndex += 1

            cmdCancel.TabIndex = iIndex



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub frmDocumentLinkEdit_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Const kMethodName As String = "Form_QueryUnload"
        Try




            ' Terminate object (if used)
            If Not (m_oFindDocs Is Nothing) Then


                m_oFindDocs.Dispose()
                ' Destroy the instance of m_oFindDocs object
                ' from memory.
                m_oFindDocs = Nothing

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            m_oFormFields = Nothing

        Finally

            'Destroy the instance of the form control object



            eventArgs.Cancel = Cancel <> 0
        End Try
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
                ' Check the details havn't changed.

                If m_bEntryDone Then
                    m_lStatus = gPMConstants.PMEComponentAction.PMAdd
                    Me.Close()
                    Return result
                End If


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                    Me.Close()
                End If

                Return result
            End If

            ' Check the task.
            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit

                    ' Check mandatory controls have been entered into.
                    m_lReturn = m_oFormFields.CheckMandatoryControls()

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    m_lReturn = SetDataToParentControl()
                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to update business.
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_bEntryDone = True

                    Me.Close()
                    'Me.Hide()
            End Select



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

    ' ***************************************************************** '
    ' Name: SetDataToParentControl
    '
    ' Description: Set the form data to the Grid of Parent Form
    '
    ' ***************************************************************** '
    Public Function SetDataToParentControl() As Integer

        Dim result As Integer = 0
        Dim oListItem As Object
        Dim lRow As Integer
        Const kMethodName As String = "SetDataToParentControl"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                oListItem = m_olvwSearchDetails.items.Add("1")

                oListItem.SubItems.Add(VB6.GetItemString(cboProcess, cboProcess.SelectedIndex).Trim())

                oListItem.SubItems.Add(cboBranch.ItemCaption.Trim())

                oListItem.SubItems.Add(cboDocumentType.Text.Trim())

                oListItem.SubItems.Add(txtDocument.Text)

                If chkClient.CheckState > 0 Then

                    'oListItem.SubItems(5) = "X"
                    oListItem.SubItems.Add("X")
                Else

                    'oListItem.SubItems(5) = ""
                    oListItem.SubItems.Add("")
                End If
                If chkAgent.CheckState > 0 Then

                    'oListItem.SubItems(6) = "X"
                    oListItem.SubItems.Add("X")
                Else

                    'oListItem.SubItems(6) = ""
                    oListItem.SubItems.Add("")
                End If
                If chkOffice.CheckState > 0 Then

                    'oListItem.SubItems(7) = "X"
                    oListItem.SubItems.Add("X")
                Else

                    'oListItem.SubItems(7) = ""
                    oListItem.SubItems.Add("")
                End If
                '(Start)-(Arul Stephen)-(Document Configuration)
                If OptPrinter.Checked Then

                    'oListItem.SubItems(8) = "Printer"
                    oListItem.SubItems.Add("Printer")
                ElseIf OptSpooler.Checked Then

                    'oListItem.SubItems(8) = "Spooler"
                    oListItem.SubItems.Add("Spooler")
                ElseIf OptUserChoice.Checked Then

                    'oListItem.SubItems(8) = "User Choice"
                    oListItem.SubItems.Add("User Choice")
                End If
                '(End)-(Arul Stephen)-(Document Configuration)
                If cboProcess.SelectedIndex <> -1 Then

                    'oListItem.SubItems(9) = VB6.GetItemData(cboProcess, cboProcess.SelectedIndex)
                    oListItem.SubItems.Add(VB6.GetItemData(cboProcess, cboProcess.SelectedIndex))
                Else

                    'oListItem.SubItems(9) = 0
                    oListItem.SubItems.Add("0")
                End If


                'oListItem.SubItems(10) = cboBranch.ItemId
                oListItem.SubItems.Add(cboBranch.ItemId)

                'oListItem.SubItems(11) = VB6.GetItemData(cboDocumentType, cboDocumentType.SelectedIndex)
                oListItem.SubItems.Add(VB6.GetItemData(cboDocumentType, cboDocumentType.SelectedIndex))

                'oListItem.SubItems(12) = m_lDocumentTypeID
                oListItem.SubItems.Add(m_lDocumentTypeID)

                'oListItem.SubItems(13) = m_lDocumentTemplateID
                oListItem.SubItems.Add(m_lDocumentTemplateID)

                'oListItem.SubItems(14) = 0
                oListItem.SubItems.Add("0")

                If chkBO.CheckState > 0 Then

                    'oListItem.SubItems(15) = "X"
                    oListItem.SubItems.Add("X")
                Else

                    'oListItem.SubItems(15) = ""
                    oListItem.SubItems.Add("")
                End If

                If chkSAM.CheckState > 0 Then

                    'oListItem.SubItems(16) = "X"
                    oListItem.SubItems.Add("X")
                Else

                    'oListItem.SubItems(16) = ""
                    oListItem.SubItems.Add("")
                End If

            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then


                m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(1).Text = VB6.GetItemString(cboProcess, cboProcess.SelectedIndex)


                m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(2).Text = cboBranch.ItemCaption.Trim()


                m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(3).Text = cboDocumentType.Text


                m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(4).Text = txtDocument.Text
                If chkClient.CheckState = CheckState.Checked Then


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(5).Text = "X"
                Else


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(5).Text = ""
                End If
                If chkAgent.CheckState = CheckState.Checked Then


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(6).Text = "X"
                Else


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(6).Text = ("")
                End If
                If chkOffice.CheckState = CheckState.Checked Then


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(7).Text = "X"
                Else


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(7).Text = ""
                End If
                '(Start)-(Arul Stephen)-(Document Configuration)
                If OptPrinter.Checked Then


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(8).Text = "Printer"
                ElseIf OptSpooler.Checked Then


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(8).Text = "Spooler"
                ElseIf OptUserChoice.Checked Then


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(8).Text = "User Choice"
                End If
                '(End)-(Arul Stephen)-(Document Configuration)

                If cboProcess.SelectedIndex <> -1 Then


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(9).Text = VB6.GetItemData(cboProcess, cboProcess.SelectedIndex)
                Else


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(9).Text = "0"
                End If



                m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(10).Text = cboBranch.ItemId


                m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(11).Text = VB6.GetItemData(cboDocumentType, cboDocumentType.SelectedIndex)


                m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(12).Text = m_lDocumentTypeID


                m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(13).Text = m_lDocumentTemplateID


                m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(14).Text = m_lDocLinkId

                If chkBO.CheckState = CheckState.Checked Then


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(15).Text = "X"
                Else


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(15).Text = ""
                End If
                If chkSAM.CheckState = CheckState.Checked Then


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(16).Text = "X"
                Else


                    m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(16).Text = ""
                End If

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
    ' ***************************************************************** '
    ' Name: LoadDataFromParentControl
    '
    ' Description: Set the form data to the Grid of Parent Form
    '
    ' ***************************************************************** '
    Public Function LoadDataFromParentControl() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadDataFromParentControl"

        Try
            'Vivek 68514
            Dim option1 As String

            result = gPMConstants.PMEReturnCode.PMTrue
            'Vivek 68514
            m_lReturn = iPMFunc.GetSystemOption(5097, option1)


            'Developer Guide No. 49
            If m_olvwSearchDetails.SelectedItems.count > 0 Then
                m_lReturn = SetItemId(m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(9).Text, cboProcess)



                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If



                'cboBranch.ItemId = m_olvwSearchDetails.ListItems.Item(m_olvwSearchDetails.SelectedItem.Index).SubItems(10)
                cboBranch.ItemId = m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(10).Text

                'cboDocumentType.ItemId = m_olvwSearchDetails.ListItems.Item(m_olvwSearchDetails.SelectedItem.Index).SubItems(11)


                'm_lReturn = SetItemId(m_olvwSearchDetails.ListItems.Item(m_olvwSearchDetails.SelectedItem.Index).SubItems(11), cboDocumentType)
                m_lReturn = SetItemId(m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(11).Text, cboDocumentType)



                'txtDocument.Text = m_olvwSearchDetails.ListItems.Item(m_olvwSearchDetails.SelectedItem.Index).SubItems(4)
                txtDocument.Text = m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(4).Text


                m_lDocumentTypeID = m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(12).Text


                m_lDocumentTemplateID = m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(13).Text


                m_lDocLinkId = m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(14).Text



                If m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(5).Text = "X" Then
                    chkClient.CheckState = CheckState.Checked
                End If


                If m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(6).Text = "X" Then
                    chkAgent.CheckState = CheckState.Checked
                End If


                If m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(7).Text = "X" Then
                    chkOffice.CheckState = CheckState.Checked
                End If

                If (chkClient.CheckState = CheckState.Unchecked) And (chkAgent.CheckState = CheckState.Unchecked) And (chkOffice.CheckState = CheckState.Unchecked) Then
                    chkDefault.CheckState = CheckState.Checked
                End If

            If ToSafeInteger(option1) <> 1 Then

                    If m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(8).Text.Trim().ToUpper() = ("Printer").ToUpper() Then
                        OptPrinter.Checked = True
                        '(Start)-(Arul Stephen)-(Document Configuration)
                    ElseIf m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(8).Text.Trim().ToUpper() = ("Spooler").ToUpper() Then
                        OptSpooler.Checked = True
                    ElseIf m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(8).Text.Trim().ToUpper() = ("User Choice").ToUpper() Then
                        OptUserChoice.Checked = True
                    End If
                End If

                '(End)-(Arul Stephen)-(Document Configuration)



                If m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(15).Text = "X" Then
                    chkBO.CheckState = CheckState.Checked
                End If


                If m_olvwSearchDetails.Items(m_olvwSearchDetails.SelectedItems.Item(0).Index).SubItems(16).Text = "X" Then
                    chkSAM.CheckState = CheckState.Checked
                End If
            Else
                m_lReturn = SetItemId(m_olvwSearchDetails.Items.Item(0).SubItems(9).Text, cboProcess)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If



                'cboBranch.ItemId = m_olvwSearchDetails.ListItems.Item(m_olvwSearchDetails.SelectedItem.Index).SubItems(10)
                cboBranch.ItemId = m_olvwSearchDetails.Items.Item(0).SubItems(10).Text

                m_lReturn = SetItemId(m_olvwSearchDetails.Items.Item(0).SubItems(11).Text, cboDocumentType)

                txtDocument.Text = m_olvwSearchDetails.Items.Item(0).SubItems(4).Text

                m_lDocumentTypeID = m_olvwSearchDetails.Items.Item(0).SubItems(12).Text

                m_lDocumentTemplateID = m_olvwSearchDetails.Items.Item(0).SubItems(13).Text

                m_lDocLinkId = m_olvwSearchDetails.Items.Item(0).SubItems(14).Text

                If m_olvwSearchDetails.Items.Item(0).SubItems(5).Text = "X" Then
                    chkClient.CheckState = CheckState.Checked
                End If


                If m_olvwSearchDetails.Items.Item(0).SubItems(6).Text = "X" Then
                    chkAgent.CheckState = CheckState.Checked
                End If

                If m_olvwSearchDetails.Items.Item(0).SubItems(7).Text = "X" Then
                    chkOffice.CheckState = CheckState.Checked
                End If

                If (chkClient.CheckState = CheckState.Unchecked) And (chkAgent.CheckState = CheckState.Unchecked) And (chkOffice.CheckState = CheckState.Unchecked) Then
                    chkDefault.CheckState = CheckState.Checked
                End If

                If ToSafeInteger(option1) <> 1 Then
                    If m_olvwSearchDetails.Items.Item(0).SubItems(8).Text.Trim().ToUpper() = ("Printer").ToUpper() Then
                        OptPrinter.Checked = True
                        '(Start)-(Arul Stephen)-(Document Configuration)
                    ElseIf m_olvwSearchDetails.Items.Item(0).SubItems(8).Text.Trim().ToUpper() = ("Spooler").ToUpper() Then
                        OptSpooler.Checked = True
                    ElseIf m_olvwSearchDetails.Items.Item(0).SubItems(8).Text.Trim().ToUpper() = ("User Choice").ToUpper() Then
                        OptUserChoice.Checked = True
                    End If
                End If

                If m_olvwSearchDetails.Items.Item(0).SubItems(15).Text = "X" Then
                    chkBO.CheckState = CheckState.Checked
                End If

                If m_olvwSearchDetails.Items.Item(0).SubItems(16).Text = "X" Then
                    chkSAM.CheckState = CheckState.Checked
                End If
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

    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFieldValidation"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDocument, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally





        End Try
        Return result
    End Function
    Private Sub frmDocumentLinkEdit_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii = CInt(Keys.Escape) Then
            cmdCancel_Click(cmdCancel, New EventArgs())
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub OptPrinter_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptPrinter.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            m_bEntryDone = False
        End If
    End Sub

    Private Sub OptSpooler_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptSpooler.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            m_bEntryDone = False
        End If
    End Sub

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
    'r_cboControl.Items.Clear()
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
    'Dim r_cboControl_NewIndex As Integer = -1

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


    Private Function SetItemId(ByVal v_lItemId As Integer, ByRef r_cboControl As ComboBox) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object

        Const kMethodName As String = "SetItemId"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            With r_cboControl
                For lIndex As Integer = 0 To .Items.Count - 1
                    If VB6.GetItemData(r_cboControl, lIndex) = v_lItemId Then
                        .SelectedIndex = lIndex
                        Exit For
                    End If
                Next lIndex
            End With




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally






        End Try
        Return result
    End Function

    Private Function GetProcessType() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Const kMethodName As String = "GetProcessType"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            'add N/A to combo box
            cboProcess.Items.Clear()
            '   cboProcess.AddItem ("(All Processes)")
            '  cboProcess.ItemData(cboProcess.NewIndex) = 0

            m_lReturn = g_oBusiness.GetProcessType(r_vProcessType:=vResultArray, v_iFunctionalArea:=m_iFunctionalArea)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            'cboFilterByProcess
            If Information.IsArray(vResultArray) Then

                For lCount As Integer = 0 To vResultArray.GetUpperBound(1)
                    Dim cboProcess_NewIndex As Integer = -1


                    'start Written Status
                    If m_bWrittenPolicyStatus = True Then

                        cboProcess.Items.Add(New VB6.ListBoxItem(CStr(vResultArray(1, lCount)), CInt(vResultArray(0, lCount))))

                    Else
                        If (vResultArray(1, lCount)) <> ("New Business Written") Then
                            If (vResultArray(1, lCount)) <> ("Renewal Acceptance Written") Then
                                cboProcess.Items.Add(New VB6.ListBoxItem(CStr(vResultArray(1, lCount)), CInt(vResultArray(0, lCount))))
                            End If
                        End If
                    End If
                    'End  Written Status

                Next
            End If

            cboProcess.SelectedIndex = 0



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally






        End Try
        Return result
    End Function

    Private Function GetProcessTypeDocs() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Const kMethodName As String = "GetProcessTypeDocs"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'add N/A to combo box
            cboDocumentType.Items.Clear()
            'cboDocumentType.AddItem ("(All Processes)")
            'cboDocumentType.ItemData(cboDocumentType.NewIndex) = 0

            m_lReturn = g_oBusiness.GetProcessTypeDocuments(r_vResultarray:=vResultArray, v_iFunctionalArea:=m_iFunctionalArea)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            'cboFilterByProcess
            If Information.IsArray(vResultArray) Then

                For lCount As Integer = 0 To vResultArray.GetUpperBound(1)
                    Dim cboDocumentType_NewIndex As Integer = -1

                    cboDocumentType_NewIndex = cboDocumentType.Items.Add(CStr(vResultArray(1, lCount)))

                    VB6.SetItemData(cboDocumentType, cboDocumentType_NewIndex, CInt(vResultArray(0, lCount)))
                Next
            End If

            cboDocumentType.SelectedIndex = 0


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally






        End Try
        Return result
    End Function


End Class
