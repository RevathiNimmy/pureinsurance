Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
'developer giude no. 129
Imports SharedFiles
Friend Class frmCertificateYear
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    Private Const ACClass As String = "frmDetails"

    Public Const ACDateConversion As String = "dd/mm/yyyy"
    Public Const ACDateDispaly As String = "long date"
    Public Const ACShortDate As String = "short date"
    Private m_lReturn As Integer
    Private m_lStatus As Integer
    Private m_sAgencyOrUnderwriting As String

    'developer guide no. 50
    Dim frminterface As frmInterface
    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRPartyAG.Business

    ' Form Fields
    Private m_oFormField As iPMFormControl.FormFields
    Private m_sCode As String
    Private m_sDescription As String
    Private m_dtStartDate As Date
    Private m_dtEndDate As Date
    Private m_sTask As String
    Private m_vLvwArray As Object(,)
    ' *** Public Properties (Begin) *** '
    'developer giude no. 101
    Public Property Code() As String
        Set(ByVal Value As String)
            m_sCode = Value
        End Set
        Get
            Return m_sCode
        End Get
    End Property
    Public Property Description() As String
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
        Get
            Return m_sDescription
        End Get
    End Property
    Public Property StartDate() As Date
        Set(ByVal Value As Date)
            m_dtStartDate = Value
        End Set
        Get
            Return m_dtStartDate
        End Get
    End Property
    Public Property EndDate() As Date
        Set(ByVal Value As Date)
            m_dtEndDate = Value
        End Set
        Get
            Return m_dtEndDate
        End Get
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property
    Public Property Task() As String
        Set(ByVal Value As String)
            m_sTask = Value
        End Set
        Get
            Return m_sTask
        End Get
    End Property
    Public Property LvwArray() As Object(,)
        Get
            Return m_vLvwArray
        End Get
        Set(ByVal value As Object(,))
            m_vLvwArray = value
        End Set
    End Property

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyAG.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Initialise form control
            m_oFormField = New iPMFormControl.FormFields()
            'developer guide no. 9
            m_lReturn = m_oFormField.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMFormControl.FormFields", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = SetFieldValidation()
            m_sAgencyOrUnderwriting = m_oBusiness.UnderwritingOrAgency

            If m_sTask = Convert.ToString(gPMConstants.PMEComponentAction.PMAdd) Then
                txtCertYearCode.Text = ""
                txtCertYearDescription.Text = ""
                txtCertYearEndDate.Text = ""
                txtCertYearStartDate.Text = ""
            Else
                ' Display the interface details
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
    Public Function DataToInterface() As Integer

        Dim result As Integer = 1

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            txtCertYearCode.Text = m_sCode
            txtCertYearDescription.Text = m_sDescription
            txtCertYearStartDate.Text = CDate(m_dtStartDate)
            txtCertYearEndDate.Text = CDate(m_dtEndDate)
            If Task = gPMConstants.PMEComponentAction.PMEdit Then
                txtCertYearCode.Enabled = False
                txtCertYearDescription.Enabled = False
                txtCertYearStartDate.Enabled = False
            Else
                txtCertYearCode.Enabled = True
                txtCertYearDescription.Enabled = True
                txtCertYearStartDate.Enabled = True
            End If

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
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

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCertYearCancel.Click

        ' Cancel
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' Exit out
        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCertYearOK.Click
        Try
            m_lReturn = m_oFormField.CheckMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = ValidateData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the properties
            m_lReturn = InterfaceToProperties()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Exit out
            Me.Hide()

        Catch excep As System.Exception

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdOK_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub
    Private Function ValidateData() As Integer

        Try
            If Not (Information.IsArray(m_vLvwArray)) Then
                Exit Function
            End If
            Dim iCnt As Integer = 0

            If ToSafeDate(txtCertYearEndDate.Text) <= ToSafeDate(txtCertYearStartDate.Text) Then
                MessageBox.Show(" Certificate Year End Date should be Greater than Start Date", "Invalid End Date")
                Return 0
                Exit Function
            End If

            For iCnt = 0 To m_vLvwArray.GetUpperBound(1)
                If m_vLvwArray(0, iCnt) IsNot Nothing Then

                    If Task <> gPMConstants.PMEComponentAction.PMEdit Then



                        If m_vLvwArray(0, iCnt) IsNot Nothing Then
                            If Convert.ToInt32(m_vLvwArray(4, iCnt)) = 0 Then
                                If m_vLvwArray(0, iCnt).ToString.ToUpper = txtCertYearCode.Text.ToUpper Then
                                    MessageBox.Show(" Certificate Year Code already exist in collection for this party")
                                    Return 0
                                    Exit Function
                                End If

                                If (Convert.ToDateTime(txtCertYearStartDate.Text) <= Convert.ToDateTime(m_vLvwArray(3, iCnt).ToString) AndAlso Convert.ToDateTime(txtCertYearEndDate.Text) >= Convert.ToDateTime(m_vLvwArray(2, iCnt).ToString)) Or _
                              (Convert.ToDateTime(txtCertYearStartDate.Text) = Convert.ToDateTime(m_vLvwArray(2, iCnt).ToString) AndAlso Convert.ToDateTime(txtCertYearEndDate.Text) = Convert.ToDateTime(m_vLvwArray(3, iCnt).ToString)) Then
                                    MessageBox.Show(" Certificate Year Start/End date conflicts with other collection items for this party")
                                    Return 0
                                    Exit Function
                                End If
                            ElseIf Convert.ToInt32(m_vLvwArray(4, iCnt)) = 1 Then
                                Continue For
                            End If
                        End If




                    End If
                End If
            Next
            Return 1

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMCancel

        End Try
    End Function
    Private Function InterfaceToProperties() As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_sCode = txtCertYearCode.Text
            m_sDescription = txtCertYearDescription.Text
            m_dtStartDate = Convert.ToDateTime(txtCertYearStartDate.Text)
            m_dtEndDate = Convert.ToDateTime(txtCertYearEndDate.Text)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMCancel

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormField.AddNewFormField(ctlControl:=txtCertYearCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormField.AddNewFormField(ctlControl:=txtCertYearDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormField.AddNewFormField(ctlControl:=txtCertYearStartDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormField.AddNewFormField(ctlControl:=txtCertYearEndDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
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

    Private Sub txtCertYearStartDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCertYearStartDate.TextChanged
        If txtCertYearStartDate.Text.Trim() = "" Then
            txtCertYearStartDate.Tag = CStr(True)
            ' m_dtStartDate = ""
        End If

        'If Information.IsDate(txtCertYearStartDate.Text.Trim()) Then
        '    m_dtStartDate = StringsHelper.Format(txtCertYearStartDate.Text.Trim(), ACShortDate)
        '    txtCertYearStartDate.Tag = CStr(True)
        'End If
    End Sub

    Private Sub txtCertYearStartDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCertYearStartDate.Enter

        m_lReturn = m_oFormField.GotFocus(ctlControl:=txtCertYearStartDate)

    End Sub

    Private Sub txtCertYearStartDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCertYearStartDate.Leave

        Dim sDisplayText As String = ""

        m_lReturn = m_oFormField.LostFocus(ctlControl:=txtCertYearStartDate)

    End Sub

    Private Sub txtCertYearEndDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCertYearEndDate.TextChanged
        If txtCertYearEndDate.Text.Trim() = "" Then

            txtCertYearEndDate.Tag = CStr(True)
            'm_dtEndDate = ""
        End If

        'If Information.IsDate(txtCertYearEndDate.Text.Trim()) Then
        '    m_dtEndDate = StringsHelper.Format(txtCertYearEndDate.Text.Trim(), ACShortDate)
        '    txtCertYearEndDate.Tag = CStr(True)
        'End If
    End Sub

    Private Sub txtCertYearEndDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCertYearEndDate.Enter

        m_lReturn = m_oFormField.GotFocus(ctlControl:=txtCertYearEndDate)

    End Sub

    Private Sub txtCertYearEndDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCertYearEndDate.Leave

        Dim sDisplayText As String = ""

        m_lReturn = m_oFormField.LostFocus(ctlControl:=txtCertYearEndDate)

    End Sub

End Class
