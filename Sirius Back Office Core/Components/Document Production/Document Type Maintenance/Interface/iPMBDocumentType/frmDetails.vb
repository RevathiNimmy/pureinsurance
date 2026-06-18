Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmDetails
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    '
    ' History:
    ' CJB 180405 PN14486 It seems that the Is Editable After Merging checkbox has no functionality
    '            behind it so have made it invisible (+the label).
    '

    Private Const ACClass As String = "frmDetails"

    Private m_lReturn As Integer
    Private m_lStatus As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_lDocumentTypeID As Integer
    Private m_iTask As Integer
    Private m_bInUse As Boolean
    Private m_iIsEditableAfterMerging As Integer

    Private m_vDataArray(,) As Object

    ' Form Fields
    Private m_oFormField As iPMFormControl.FormFields

    ' *** Public Properties (Begin) *** '

    Public WriteOnly Property InUse() As Boolean
        Set(ByVal Value As Boolean)
            m_bInUse = Value
        End Set
    End Property


    'developer guide no. 17
    Public WriteOnly Property DataArray() As Object(,)
        Set(ByVal Value As Object(,))
            m_vDataArray = Value
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

    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
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

    Public Property IsEditableAfterMerging() As Integer
        Get
            Return m_iIsEditableAfterMerging
        End Get
        Set(ByVal Value As Integer)
            m_iIsEditableAfterMerging = Value
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

    Public Property DocumentTypeID() As Integer
        Get
            Return m_lDocumentTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTypeID = Value
        End Set
    End Property

    ' *** Public Properties (End) *** '

    ' ***************************************************************** '
    '
    ' Name: DataToInterface
    '
    ' Description:
    '
    ' History: 16/07/01 DC - Created.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the code
            m_lReturn = m_oFormField.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the description
            m_lReturn = m_oFormField.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iIsEditableAfterMerging = 1 Then
                chkIsEditable.CheckState = CheckState.Checked
            Else
                chkIsEditable.CheckState = CheckState.Unchecked
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetFieldValidation
    '
    ' Description:
    '
    ' History: 16/07/01 DC - Created.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormField.AddNewFormField(ctlControl:=txtCode, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormField.AddNewFormField(ctlControl:=txtDescription, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFieldValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 16/07/01 DC - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise form control
            m_oFormField = New iPMFormControl.FormFields()

            m_lReturn = CType(m_oFormField, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMFormControl.FormFields", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = SetFieldValidation()

            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the interface details if we're in edit mode
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = DataToInterface()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 16/07/01 DC - Created.
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
                If m_oFormField IsNot Nothing Then
                    m_oFormField.Dispose()
                End If
                m_oFormField = Nothing
                
            End If
        End If
        Me.disposedValue = True
    End Sub


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Cancel
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' Exit out
        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        m_lReturn = m_oFormField.CheckMandatoryControls()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lReturn = ValidateOurControls()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Set the properties
        m_lReturn = InterfaceToProperties()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Exit out
        Me.Hide()

    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter

        m_lReturn = m_oFormField.GotFocus(ctlControl:=txtCode)

    End Sub

    Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave

        m_lReturn = m_oFormField.LostFocus(ctlControl:=txtCode)

    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter

        m_lReturn = m_oFormField.GotFocus(ctlControl:=txtDescription)

    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave

        m_lReturn = m_oFormField.LostFocus(ctlControl:=txtDescription)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: InterfaceToProperties
    '
    ' Description: Set the values of the properties from the interface
    '
    ' History: 16/07/01 DC - Created.
    '
    ' ***************************************************************** '
    Private Function InterfaceToProperties() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Code

            m_sCode = CStr(m_oFormField.UnformatControl(ctlControl:=txtCode))
            ' Description

            m_sDescription = CStr(m_oFormField.UnformatControl(ctlControl:=txtDescription))

            ' Is Editable After Merging
            If chkIsEditable.CheckState = CheckState.Checked Then
                m_iIsEditableAfterMerging = 1
            Else
                m_iIsEditableAfterMerging = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckUniqueCode
    '
    ' Description: Checks that the user has entered a valid code
    '
    ' History: 16/07/01 DC - Created.
    '
    ' ***************************************************************** '
    Private Function CheckUniqueCode() As Integer

        Dim result As Integer = 0
        Dim sCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the code

            sCode = CStr(m_oFormField.UnformatControl(ctlControl:=txtCode))

            ' Check it isnt the same as we started with
            If sCode.Trim() = m_sCode.Trim() Then
                Return result
            End If


            If m_vDataArray Is Nothing Then
                Return result
            End If

            ' Loop through the array and check if its the same as any other
            For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)
                If CStr(m_vDataArray(ACArrayCode, iLoop1)).Trim().ToUpper() = sCode.ToUpper() Then
                    ' Found a match so PMFalse outta here

                    'set so that it can be used in message
                    txtMatchCodeDesc.Text = CStr(m_vDataArray(ACArrayDescription, iLoop1)).Trim()

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit For

                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUniqueCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUniqueCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateOurControls
    '
    ' Description: Validates control values that arent covered by Form Fields
    '
    ' History: 16/07/00 DC - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateOurControls() As Integer

        Dim result As Integer = 0
        Dim sMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the Code is unique
            m_lReturn = CheckUniqueCode()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("The code '" & txtCode.Text & "' is already in use for '" & txtMatchCodeDesc.Text & "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOurControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOurControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets defaults for adding a new record
    '
    ' History: 16/07/01 DC - Created.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the captions
            m_lReturn = DisplayCaptions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMEdit

                    ' Lock code text box
                    txtCode.Enabled = False

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: DisplayCaptions
    '
    ' Description:
    '
    ' History: 16/07/01 DC - Created.
    '
    ' ***************************************************************** '
    Public Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Form's caption

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACInterface2Title, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tab

            SSTabHelper.SetTabCaption(tabMain, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Labels

            lblCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCodeLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDescriptionLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblIsEditable.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACIsEditableAfterMergingLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMain.SelectedIndex = 0
        End If
    End Sub
End Class
