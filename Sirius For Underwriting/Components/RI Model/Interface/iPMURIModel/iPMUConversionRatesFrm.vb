Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Reflection
Imports Artinsoft.VB6.Gui
Partial Friend Class frmRIConversionRates
    Inherits System.Windows.Forms.Form
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "iPMUConversionRatesFrm"
    Public Property DialogResult As DialogResult = DialogResult.Cancel
    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public Status As gPMConstants.PMEReturnCode
    ' Declare an instance of the Business object.
    Public Business As Object
    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    ' Stores the return value for the a function call.
    Private m_lReturn As Integer
    Private m_vRIModelCurrRates(,) As Object
    Private Const vbFormCode As Integer = 0
    Private m_lRIModelID As Integer
    Private m_sModelCurrencyDescription As String
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String
    Public WriteOnly Property RIModelCurrRates() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vRIModelCurrRates = Value
        End Set
    End Property
    Public WriteOnly Property RIModelID() As Integer
        Set(ByVal Value As Integer)
            m_lRIModelID = Value
        End Set
    End Property
    Public WriteOnly Property ModelCurrencyDescription() As String
        Set(ByVal Value As String)
            m_sModelCurrencyDescription = Value
        End Set
    End Property

    Public WriteOnly Property UniqueId() As String
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public WriteOnly Property ScreenHierarchy() As String
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property
    Public Function GetRIModelCurrencyRates(Optional ByVal v_lIndex As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetRIModelCurrencyRates"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Me.Text = "Conversion Rates"
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.ShowIcon = False
            ' Add title label with multi-line text
            lblTitle.Text = "Currency conversion rates to be applied" & vbCrLf & "throughout the model validity"
            Dim dt As New DataTable()
            ' Create two columns: Currency Description and Model Currency Rate
            dt.Columns.Add("Currency", GetType(String))
            dt.Columns.Add("ConversionRate", GetType(String))
            If Information.IsArray(m_vRIModelCurrRates) Then
                ' Process all rows in the array
                For lCount As Integer = m_vRIModelCurrRates.GetLowerBound(1) To m_vRIModelCurrRates.GetUpperBound(1)
                    Dim dr As DataRow = dt.NewRow()
                    dr(0) = If(m_vRIModelCurrRates(1, lCount), "") ' Currency Description
                    dr(1) = If(m_vRIModelCurrRates(2, lCount), "") ' Conversion Rate
                    dt.Rows.Add(dr)
                Next lCount
            End If
            lvwRIModelCurrencyRates.DataSource = dt
            ' Configure columns
            lvwRIModelCurrencyRates.Columns(0).HeaderText = ""
            lvwRIModelCurrencyRates.Columns(0).ReadOnly = True
            lvwRIModelCurrencyRates.Columns(0).Width = 130
            lvwRIModelCurrencyRates.Columns(1).HeaderText = "Model Currency" & vbCrLf & "(" & m_sModelCurrencyDescription & ")"
            lvwRIModelCurrencyRates.Columns(1).Width = 130
            lvwRIModelCurrencyRates.BackgroundColor = Color.White
            lvwRIModelCurrencyRates.AllowUserToAddRows = False
            lvwRIModelCurrencyRates.RowHeadersVisible = False
            lvwRIModelCurrencyRates.ColumnHeadersVisible = True
            lvwRIModelCurrencyRates.BorderStyle = BorderStyle.None
            ' Style model currency rows
            For i As Integer = 0 To lvwRIModelCurrencyRates.Rows.Count - 1
                If lvwRIModelCurrencyRates.Rows(i).Cells(0).Value?.ToString() = m_sModelCurrencyDescription Then
                    lvwRIModelCurrencyRates.Rows(i).Cells(1).Style.BackColor = Color.LightGray
                End If
            Next

            ' Add event handlers
            AddHandler lvwRIModelCurrencyRates.CellValueChanged, AddressOf Grid_CellValueChanged
            AddHandler lvwRIModelCurrencyRates.CellValidating, AddressOf Grid_CellValidating
            AddHandler lvwRIModelCurrencyRates.CellBeginEdit, AddressOf Grid_CellBeginEdit
            ' Use same positioning logic as iPMURIModelFrm
            Me.StartPosition = FormStartPosition.CenterParent
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
        End Try
        Return result
    End Function
    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        ' Check if all conversion rates are filled
        Dim allRatesFilled As Boolean = True
        If Information.IsArray(m_vRIModelCurrRates) Then
            For lCount As Integer = m_vRIModelCurrRates.GetLowerBound(1) To m_vRIModelCurrRates.GetUpperBound(1)
                Dim currencyDesc As String = Convert.ToString(m_vRIModelCurrRates(1, lCount)).Trim()
                Dim rateValue As String = Convert.ToString(m_vRIModelCurrRates(2, lCount)).Trim()
                ' Skip validation for model currency
                If currencyDesc <> m_sModelCurrencyDescription Then
                    If String.IsNullOrEmpty(rateValue) OrElse rateValue = "{}" OrElse rateValue = "0" Then
                        allRatesFilled = False
                        Exit For
                    End If
                End If
            Next lCount
        End If
        If allRatesFilled Then
            SaveAndClose()
        Else
            Dim result As DialogResult = MessageBox.Show(
                "All the Currency conversion rates are not filled. The fixed conversion rate will only be applied where the rate is provided.",
                "Conversion Rates",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning)
            If result = DialogResult.OK Then
                SaveAndClose()
            End If
        End If
    End Sub
    Private Sub SaveAndClose()
        Dim lReturn As gPMConstants.PMEReturnCode
        lReturn = CType(Business.UpdateRIModelCurrencyRates(m_lRIModelID, m_vRIModelCurrRates, m_sUniqueId, m_sScreenHierarchy), gPMConstants.PMEReturnCode)
        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Failed to save currency rates.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        ' Check the user wants to close
        If MessageBox.Show("Cancelling will lose all of your current details." &
                           Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
            ' Set status to cancel and close
            Status = gPMConstants.PMEReturnCode.PMCancel
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End If
    End Sub
    Private Sub Grid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 And e.ColumnIndex = 1 Then ' Only for conversion rate column
            Dim dt As DataTable = CType(lvwRIModelCurrencyRates.DataSource, DataTable)
            If dt IsNot Nothing And e.RowIndex < dt.Rows.Count Then
                m_vRIModelCurrRates(2, e.RowIndex) = dt.Rows(e.RowIndex)(e.ColumnIndex)
            End If
        End If
    End Sub
    Private Sub Grid_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs)
        If e.RowIndex >= 0 And e.ColumnIndex = 1 Then ' Only validate conversion rate column
            Dim value As String = e.FormattedValue.ToString().Trim()
            If Not String.IsNullOrEmpty(value) Then
                Dim numericValue As Double
                If Not Double.TryParse(value, numericValue) OrElse numericValue < 0 Then
                    MessageBox.Show("Please enter a valid positive numeric value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    Private Sub Grid_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs)
        If e.ColumnIndex = 1 And e.RowIndex >= 0 Then
            Dim currencyValue As String = lvwRIModelCurrencyRates.Rows(e.RowIndex).Cells(0).Value?.ToString()
            If currencyValue = m_sModelCurrencyDescription Then
                e.Cancel = True
            End If
        End If
    End Sub
End Class