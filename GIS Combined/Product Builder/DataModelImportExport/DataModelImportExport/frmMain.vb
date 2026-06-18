Option Strict On
Option Explicit On

Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
Imports System.Security.Cryptography

Public Class frmMain

#Region "Private Constants"
    Private Const kWindowsAuthentication As String = "Windows Authentication"
    Private Const kSQLAuthentication As String = "SQL Server Authentication"
#End Region

#Region "Private Variables"
    Private m_sServername As String = String.Empty
    Private m_sDatabase As String = String.Empty
    Private m_sUsername As String = String.Empty
    Private m_sPassword As String = String.Empty
    Private m_bIntegratedSecurity As Boolean = False
    Private m_sSystemLoggedin As String = String.Empty
    Private m_conConnection As SqlConnection
    Private m_cmdCommand As SqlCommand
    Private m_sConectionString As String = String.Empty
    Private m_bIsConnected As Boolean = False
#End Region

#Region "Private Methods"
    ''' <summary>
    ''' This procedure will setup default values in controls
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetLoginDefaults()

        cboAuthentication.Items.Clear()
        cboAuthentication.Items.Add(kWindowsAuthentication)
        cboAuthentication.Items.Add(kSQLAuthentication)
        cboAuthentication.SelectedIndex = 0
        Call ControlRenderLogic()
    End Sub

    ''' <summary>
    ''' This procedure will setup default values in controls
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ControlRenderLogic()
        'TAB LOGIN 
        If Not m_bIsConnected Then
            m_sSystemLoggedin = txtServerName.Text & " Database"
        End If

        If cboAuthentication.Text = kSQLAuthentication Then
            grpLoginDetails.Visible = True
        Else
            grpLoginDetails.Visible = False
            txtUsername.Text = String.Empty
            txtPassword.Text = String.Empty
        End If

        If m_bIsConnected Then
            grpUtlity.Visible = True
            lblInfo.Visible = False
            btnLogin.Text = "Log Off! From " & m_sSystemLoggedin
            btnLogin.BackColor = Color.LightGreen
        Else
            grpUtlity.Visible = False
            lblInfo.Visible = True
            btnLogin.Text = "Log On! To " & m_sSystemLoggedin
            btnLogin.BackColor = Color.Transparent
        End If

        'TAB UTLITY
        If m_bIsConnected Then
            If optImport.Checked Then
                grpSelectDataModel.Visible = False
                'grpInclude.Visible = False
                grpFileDetails.Text = "Please Click Browse button to provide Import File Details"
                btnExecute.Text = "Run Import"
                btnBrowseLocation.Text = "&Browse File"
            ElseIf optExport.Checked Then
                grpSelectDataModel.Visible = True
                'grpInclude.Visible = True
                grpFileDetails.Text = "Please Click Save button to provide Export File Details"
                btnBrowseLocation.Text = "&Save As"
                btnExecute.Text = "Run Export"
            End If
        End If
    End Sub

    ''' <summary>
    ''' This procedure will create sql connection
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateConnection()
        m_sServername = txtServerName.Text
        m_sDatabase = txtDatabase.Text
        m_sUsername = txtUsername.Text
        m_sPassword = txtPassword.Text
        m_bIntegratedSecurity = cboAuthentication.Text = kWindowsAuthentication
        Try
            'Testing Connection
            m_sConectionString = GetConnectionString()
            If Not String.IsNullOrEmpty(m_sConectionString) Then
                m_conConnection = New SqlConnection(m_sConectionString)
                m_cmdCommand = New SqlCommand(Nothing, m_conConnection)
                m_bIsConnected = True
                m_conConnection.Open()
            End If

        Catch ex As Exception
            m_bIsConnected = False
            MessageBox.Show("Not able to establish a connection" & vbCrLf & ex.Message, "Create Connection")
        Finally
            If Not String.IsNullOrEmpty(m_sConectionString) Then
                m_conConnection.Close()
            End If
        End Try

    End Sub

    ''' <summary>
    ''' To create connection string from data enter in controls
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetConnectionString() As String

        Dim loValidationList As New List(Of String)
        Dim bIsValid As Boolean = True
        If String.IsNullOrEmpty(m_sServername) Then
            loValidationList.Add("Servername")
            bIsValid = False
        End If

        If String.IsNullOrEmpty(m_sDatabase) Then
            loValidationList.Add("Database")
            bIsValid = False
        End If

        If Not m_bIntegratedSecurity Then
            If String.IsNullOrEmpty(m_sUsername) Then
                loValidationList.Add("Username")
                bIsValid = False
            End If
            If String.IsNullOrEmpty(m_sPassword) Then
                loValidationList.Add("Password")
                bIsValid = False
            End If
        End If

        If Not bIsValid Then
            Dim sValidationString As New StringBuilder
            sValidationString.Append("Following details are missing:" & vbNewLine)
            For i = 0 To loValidationList.Count - 1
                sValidationString.Append(loValidationList.Item(i).ToString & vbNewLine)
            Next
            MessageBox.Show(sValidationString.ToString, "Data Model Import/Export Utlity")
            Return String.Empty
        Else
            Dim sConnection As New SqlConnectionStringBuilder
            sConnection.DataSource = m_sServername
            sConnection.InitialCatalog = m_sDatabase
            sConnection.IntegratedSecurity = m_bIntegratedSecurity
            sConnection.UserID = m_sUsername
            sConnection.Password = m_sPassword
            Return sConnection.ConnectionString
        End If
        
    End Function

    ''' <summary>
    ''' To populate data models codes in dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillDataModels()
        Try
            Dim drDatamodels As SqlDataReader

            m_conConnection.Open()
            m_cmdCommand.CommandText = "Select code from gis_data_model"

            drDatamodels = m_cmdCommand.ExecuteReader()

            cboDataModel.Items.Clear()

            While drDatamodels.Read
                cboDataModel.Items.Add(drDatamodels(0).ToString)
            End While
            cboDataModel.SelectedIndex = 0
            m_cmdCommand.CommandText = String.Empty
            m_conConnection.Close()
        Catch ex As Exception
            MessageBox.Show("Failed to populate data model codes." & vbNewLine & ex.Message, "Data Model Import/Export Utlity")
        End Try
    End Sub

    ''' <summary>
    ''' This procedure will import data model through dynamic sql to selected data base  
    ''' </summary>
    ''' <param name="sSqlScript"></param>
    ''' <remarks></remarks>
    Private Sub ImportData(ByVal sSqlScript As String)
        Dim txnTransaction As SqlTransaction
        
        Try
            m_conConnection.Open()
            txnTransaction = m_conConnection.BeginTransaction("DataModelImport")
            m_cmdCommand = New SqlCommand(Nothing, m_conConnection)
            m_cmdCommand.Connection = m_conConnection
            m_cmdCommand.Transaction = txnTransaction
            m_cmdCommand.CommandText = sSqlScript
            m_cmdCommand.ExecuteNonQuery()
            ' Attempt to commit the transaction.
            txnTransaction.Commit()
            MessageBox.Show("Data Model Imported Sucessfully", "Data Model Import/Export Utlity")
        Catch ex As SqlException
            MessageBox.Show("Failed to Import." & vbNewLine & ex.Message, "Data Model Import/Export Utlity")
            Try
                txnTransaction.Rollback()
                MessageBox.Show("Failed to Import." & vbNewLine & "Rolledback changes successfully.", "Data Model Import/Export Utlity")
            Catch ex2 As Exception
                MessageBox.Show("Failed to Import." & vbNewLine & "Failed to Rollback changes as well. Error type is:" & _
                    vbNewLine & ex2.GetType.ToString, "Data Model Import/Export Utlity")
            Finally
                m_conConnection.Close()
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' This function will return dynamic sql for data model to export data model to other data base
    ''' </summary>
    ''' <param name="sDMcode"></param>
    ''' <param name="bCaptureScreens"></param>
    ''' <param name="bProductandRisks"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDataToExport(ByVal sDMcode As String, ByVal bCaptureScreens As Boolean, ByVal bProductandRisks As Boolean) As String
        Dim sText As New StringBuilder
        Dim bSPExists As Boolean
        Try
            Dim drData As SqlDataReader
            m_conConnection.Open()
            m_cmdCommand = New SqlCommand(kCheckSP, m_conConnection)
            m_cmdCommand.CommandType = CommandType.Text
            drData = m_cmdCommand.ExecuteReader()
            bSPExists = drData.HasRows
            drData.Close()
            If Not bSPExists Then
                Dim arrString As String() = System.Text.RegularExpressions.Regex.Split(kExportSP, "GO" & vbCrLf)
                For iCount As Integer = 0 To arrString.Length - 1
                    If arrString(iCount).ToString.ToUpper.Trim <> "GO" Then
                        m_cmdCommand = New SqlCommand(arrString(iCount).ToString, m_conConnection)
                        m_cmdCommand.CommandType = CommandType.Text
                        m_cmdCommand.ExecuteNonQuery()
                    End If
                Next
            End If
            m_cmdCommand = New SqlCommand(kGISData, m_conConnection)
            m_cmdCommand.CommandType = CommandType.StoredProcedure
            m_cmdCommand.Parameters.AddWithValue("@sDataModelCode", sDMcode)
            'this has been defaulted parameters in store proc now but will be enhanced in future so keeping the code
            'm_cmd.Parameters.AddWithValue("@COPYPRODUCTANDRISK", IIf(bProductandRisks = True, 1, 0))
            'm_cmd.Parameters.AddWithValue("@CAPTURESCREENS", IIf(bCaptureScreens = True, 1, 0))

            drData = m_cmdCommand.ExecuteReader()
            While drData.Read
                sText.AppendLine(drData(0).ToString)
            End While
            drData.Close()
        Catch ex As Exception
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MessageBox.Show("Failed to Export." & vbNewLine & ex.Message, "Data Model Import/Export Utlity")
        Finally
            m_conConnection.Close()
        End Try
        Return sText.ToString
    End Function

    ''' <summary>
    ''' This procedure will write dynamic sql to a file which was exported through data model export process
    ''' </summary>
    ''' <param name="sOutpath"></param>
    ''' <param name="sData"></param>
    ''' <remarks></remarks>
    Private Sub WriteToFile(ByVal sOutpath As String, ByVal sData As String)
        Try
            'If the file is already there let us know
            If File.Exists(sOutpath) Then
                If MessageBox.Show(sOutpath & " already exists. Do you want to overwrite this?", "File Already Exists", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    File.Delete(sOutpath)
                Else
                    Exit Sub
                End If
            End If

            Dim sDatatobeSaved As String
            sDatatobeSaved = sData

            Dim writer As New StreamWriter(File.Open(sOutpath, FileMode.CreateNew))
            writer.Write(sDatatobeSaved)
            writer.Close()

            MessageBox.Show("Data Model " & cboDataModel.Text.Trim & " Exported Sucessfully", "Data Model Import/Export Utlity")
        Catch ex As Exception
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MessageBox.Show("Failed to Export Data Model. " & vbCrLf & ex.Message, "Data Model Import/Export Utlity")
        End Try
    End Sub

    ''' <summary>
    ''' This procedure will read dynamic sql from a file which need to import through data model import process
    ''' </summary>
    ''' <param name="sPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReadFromFile(ByVal sPath As String) As String
        Dim sData As String = String.Empty
        Try

            Dim srReader As New StreamReader(sPath)
            sData = srReader.ReadToEnd
            srReader.Close()

        Catch ex As Exception
            MessageBox.Show("Failed to read from file. " & vbCrLf & ex.Message, "Data Model Import/Export Utlity")
        End Try
        Return sData
    End Function

#End Region

#Region "Event Handlers"

    ''' <summary>
    ''' This will handle optImport_CheckedChanged event and rerender controls by caling ControlRenderLogic()
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub optImport_CheckedChanged(sender As Object, e As EventArgs) Handles optImport.CheckedChanged
        ControlRenderLogic()
    End Sub

    ''' <summary>
    ''' This will handle optExport_CheckedChanged event and rerender controls by caling ControlRenderLogic()
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub optExport_CheckedChanged(sender As Object, e As EventArgs) Handles optExport.CheckedChanged
        ControlRenderLogic()
    End Sub

    ''' <summary>
    ''' This will handle btnExecute_Click event and start the process import/export as selected by user
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        If String.IsNullOrEmpty(txtFileName.Text) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MessageBox.Show("Please Provide File Details.", "Data Model Import/Export Utlity")
            btnBrowseLocation.Focus()
            Exit Sub
        End If

        Dim sSqlScript As String
        If optExport.Checked Then
            sSqlScript = GetDataToExport(cboDataModel.Text, chkScreenDesign.Checked, chkProductandRisk.Checked)
            WriteToFile(txtFileName.Text, sSqlScript)
        Else
            sSqlScript = ReadFromFile(txtFileName.Text)
            ImportData(sSqlScript)
        End If
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    ''' <summary>
    ''' This will handle btnBrowseLocation_Click event and will open appropriate dialog box as per process selected by user
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBrowseLocation_Click(sender As Object, e As EventArgs) Handles btnBrowseLocation.Click
        If optImport.Checked Then
            dlgOpenFile.DefaultExt = ".sql"
            dlgOpenFile.Filter = "SQL Files (*.sql)|*.sql"
            If dlgOpenFile.ShowDialog() <> Windows.Forms.DialogResult.Cancel Then
                txtFileName.Text = dlgOpenFile.FileName
            End If
        Else
            dlgSaveFile.DefaultExt = ".sql"
            dlgSaveFile.Filter = "SQL Files (*.sql)|*.sql"
            If dlgSaveFile.ShowDialog() <> Windows.Forms.DialogResult.Cancel Then
                txtFileName.Text = dlgSaveFile.FileName
            End If
        End If
    End Sub

    ''' <summary>
    ''' This is default form load event 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetLoginDefaults()

        'These have to be true for every datamodel and readonly
        chkDataModelRelatedUDLs.Checked = True
        chkDataModelRelatedUMLs.Checked = True

        chkDataModelRelatedUDLs.Enabled = False
        chkDataModelRelatedUMLs.Enabled = False

        'For Now following options are not available
        grpInclude.Visible = False
        chkScreenDesign.Checked = False
        chkProductandRisk.Checked = False
        chkProductandRisk.Enabled = False
        chkAllUDLsInDatabase.Enabled = False
        chkAllUMLsInDatabase.Enabled = False
    End Sub

    ''' <summary>
    ''' This procedure rerender the controls according to selected option by calling ControlRenderLogic procedure
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboAuthentication_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAuthentication.SelectedIndexChanged
        ControlRenderLogic()
    End Sub

    ''' <summary>
    ''' This will handle btnLogin_Click event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        m_sSystemLoggedin = txtServerName.Text & " Database"
        If Not m_bIsConnected Then
            CreateConnection()
            If m_bIsConnected Then
                FillDataModels()
            End If
        Else
            m_bIsConnected = False
        End If

        ControlRenderLogic()
    End Sub

#End Region
End Class
