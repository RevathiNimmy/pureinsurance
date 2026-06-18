Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmAuthority
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmAuthority"

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' UnallocatedProducts
    Private m_vUnallocatedProducts(,) As Object
    ' AuthorityLevelTypes
    Private m_vAuthorityLevelTypes(,) As Object
    ' AuthorityTypeId
    Private m_lAuthorityTypeId As Integer
    ' ProductId
    Private m_lProductId As Integer
    ' Status
    Private m_lStatus As gPMConstants.PMEReturnCode
    ' Task
    Private m_iTask As gPMConstants.PMEComponentAction
    ' ProductDescription
    Private m_sProductDescription As String = ""
    ' AuthorityTypeDescription
    Private m_sAuthorityTypeDescription As String = ""

    Private m_oFormfields As iPMFormControl.FormFields


    Public Property AuthorityTypeDescription() As String
        Get
            Return m_sAuthorityTypeDescription
        End Get
        Set(ByVal Value As String)
            m_sAuthorityTypeDescription = Value
        End Set
    End Property


    Public Property ProductDescription() As String
        Get
            Return m_sProductDescription
        End Get
        Set(ByVal Value As String)
            m_sProductDescription = Value
        End Set
    End Property


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
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


    Public Property AuthorityLevelTypes() As Object
        Get
            Return VB6.CopyArray(m_vAuthorityLevelTypes)
        End Get
        Set(ByVal Value As Object)
            m_vAuthorityLevelTypes = Value
        End Set
    End Property


    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property


    Public Property AuthorityTypeId() As Integer
        Get
            Return m_lAuthorityTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lAuthorityTypeId = Value
        End Set
    End Property


    Public Property UnallocatedProducts() As Object
        Get
            Return VB6.CopyArray(m_vUnallocatedProducts)
        End Get
        Set(ByVal Value As Object)
            m_vUnallocatedProducts = Value
        End Set
    End Property


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

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblProduct.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAuthorityLevel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAuthorityLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=cboProduct, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=cboAuthorityLevel, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim sTitle, sMessage As String
        Dim iMsgResult As DialogResult

        ' Click event of the Cancel button.

        Try


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim sMsg As String = ""

        ' Check mandatory controls have been entered into.
        m_lReturn = m_oFormfields.CheckMandatoryControls()

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        '    'Check data has been selected.
        '    sMsg = ""
        '    If (cboProduct.ListIndex = -1) Then
        '        sMsg = vbCrLf & "Product"
        '    End If
        '    If (cboAuthorityLevel.ListIndex = -1) Then
        '        sMsg = vbCrLf & "Authority Level"
        '    End If
        '    If (sMsg <> "") Then
        '        sMsg = "Please select the following fields:" & vbCrLf & sMsg
        '        MsgBox sMsg, vbExclamation, "Mandatory Fields"
        '        Exit Sub
        '    End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        'Set properties from interface.
        m_lProductId = VB6.GetItemData(cboProduct, cboProduct.SelectedIndex)
        m_sProductDescription = cboProduct.Text.Trim()
        m_lAuthorityTypeId = VB6.GetItemData(cboAuthorityLevel, cboAuthorityLevel.SelectedIndex)
        m_sAuthorityTypeDescription = cboAuthorityLevel.Text.Trim()

        Me.Hide()


    End Sub


    Private Sub frmAuthority_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim iAuthorityIndex As Integer

        Try

            m_oFormfields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            m_lReturn = SetInterfaceDefaults()

            'Populate Product combo.

            If Information.IsArray(m_vUnallocatedProducts) Then
                For iCount As Integer = 0 To m_vUnallocatedProducts.GetUpperBound(1)
                    Dim cboProduct_NewIndex As Integer = -1
                    cboProduct_NewIndex = cboProduct.Items.Add(CStr(m_vUnallocatedProducts(1, iCount)))
                    VB6.SetItemData(cboProduct, cboProduct_NewIndex, CInt(m_vUnallocatedProducts(0, iCount)))
                Next iCount
            End If

            iAuthorityIndex = -1
            'Populate Authority Level Type combo.
            If Information.IsArray(m_vAuthorityLevelTypes) Then
                For iCount As Integer = 0 To m_vAuthorityLevelTypes.GetUpperBound(1)
                    Dim cboAuthorityLevel_NewIndex As Integer = -1
                    cboAuthorityLevel_NewIndex = cboAuthorityLevel.Items.Add(CStr(m_vAuthorityLevelTypes(ACALTDesc, iCount)))
                    VB6.SetItemData(cboAuthorityLevel, cboAuthorityLevel_NewIndex, CInt(m_vAuthorityLevelTypes(ACALTId, iCount)))
                    If m_lAuthorityTypeId <> 0 Then
                        If m_lAuthorityTypeId = CDbl(m_vAuthorityLevelTypes(ACALTId, iCount)) Then
                            iAuthorityIndex = iCount
                        End If
                    End If
                Next iCount
            End If

            'Disable product combo if we are editing.
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                cboProduct.SelectedIndex = 0
                cboProduct.Enabled = False
                cboAuthorityLevel.SelectedIndex = iAuthorityIndex
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub


    Private Sub frmAuthority_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If Not (m_oFormfields Is Nothing) Then

            ' Terminate the form control object.
		m_oFormfields.Dispose()

            ' Check for errors.

            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormfields = Nothing

        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub frmAuthority_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
