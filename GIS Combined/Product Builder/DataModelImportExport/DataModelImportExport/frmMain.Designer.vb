<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.APIETabs = New System.Windows.Forms.TabControl()
        Me.TabLogin = New System.Windows.Forms.TabPage()
        Me.grplogin = New System.Windows.Forms.GroupBox()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.grpDBConnect = New System.Windows.Forms.GroupBox()
        Me.txtDatabase = New System.Windows.Forms.TextBox()
        Me.lblDatabase = New System.Windows.Forms.Label()
        Me.grpLoginDetails = New System.Windows.Forms.GroupBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.lblAuthentication = New System.Windows.Forms.Label()
        Me.cboAuthentication = New System.Windows.Forms.ComboBox()
        Me.txtServerName = New System.Windows.Forms.TextBox()
        Me.lblServerName = New System.Windows.Forms.Label()
        Me.TabUtlity = New System.Windows.Forms.TabPage()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpUtlity = New System.Windows.Forms.GroupBox()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.grpSelectDataModel = New System.Windows.Forms.GroupBox()
        Me.cboDataModel = New System.Windows.Forms.ComboBox()
        Me.grpInclude = New System.Windows.Forms.GroupBox()
        Me.chkProductandRisk = New System.Windows.Forms.CheckBox()
        Me.chkAllUMLsInDatabase = New System.Windows.Forms.CheckBox()
        Me.chkAllUDLsInDatabase = New System.Windows.Forms.CheckBox()
        Me.chkDataModelRelatedUMLs = New System.Windows.Forms.CheckBox()
        Me.chkDataModelRelatedUDLs = New System.Windows.Forms.CheckBox()
        Me.chkScreenDesign = New System.Windows.Forms.CheckBox()
        Me.grpFileDetails = New System.Windows.Forms.GroupBox()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.btnBrowseLocation = New System.Windows.Forms.Button()
        Me.grpActionSelection = New System.Windows.Forms.GroupBox()
        Me.optExport = New System.Windows.Forms.RadioButton()
        Me.optImport = New System.Windows.Forms.RadioButton()
        Me.dlgOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.dlgSaveFile = New System.Windows.Forms.SaveFileDialog()
        Me.APIETabs.SuspendLayout()
        Me.TabLogin.SuspendLayout()
        Me.grplogin.SuspendLayout()
        Me.grpDBConnect.SuspendLayout()
        Me.grpLoginDetails.SuspendLayout()
        Me.TabUtlity.SuspendLayout()
        Me.grpUtlity.SuspendLayout()
        Me.grpSelectDataModel.SuspendLayout()
        Me.grpInclude.SuspendLayout()
        Me.grpFileDetails.SuspendLayout()
        Me.grpActionSelection.SuspendLayout()
        Me.SuspendLayout()
        '
        'APIETabs
        '
        Me.APIETabs.Controls.Add(Me.TabLogin)
        Me.APIETabs.Controls.Add(Me.TabUtlity)
        Me.APIETabs.Location = New System.Drawing.Point(3, 2)
        Me.APIETabs.Name = "APIETabs"
        Me.APIETabs.SelectedIndex = 0
        Me.APIETabs.Size = New System.Drawing.Size(513, 478)
        Me.APIETabs.TabIndex = 1
        '
        'TabLogin
        '
        Me.TabLogin.Controls.Add(Me.grplogin)
        Me.TabLogin.Controls.Add(Me.grpDBConnect)
        Me.TabLogin.Location = New System.Drawing.Point(4, 22)
        Me.TabLogin.Name = "TabLogin"
        Me.TabLogin.Padding = New System.Windows.Forms.Padding(3)
        Me.TabLogin.Size = New System.Drawing.Size(505, 452)
        Me.TabLogin.TabIndex = 0
        Me.TabLogin.Text = "Login"
        Me.TabLogin.UseVisualStyleBackColor = True
        '
        'grplogin
        '
        Me.grplogin.Controls.Add(Me.btnLogin)
        Me.grplogin.Location = New System.Drawing.Point(53, 290)
        Me.grplogin.Name = "grplogin"
        Me.grplogin.Size = New System.Drawing.Size(408, 71)
        Me.grplogin.TabIndex = 1
        Me.grplogin.TabStop = False
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(37, 19)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(333, 35)
        Me.btnLogin.TabIndex = 4
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'grpDBConnect
        '
        Me.grpDBConnect.Controls.Add(Me.txtDatabase)
        Me.grpDBConnect.Controls.Add(Me.lblDatabase)
        Me.grpDBConnect.Controls.Add(Me.grpLoginDetails)
        Me.grpDBConnect.Controls.Add(Me.lblAuthentication)
        Me.grpDBConnect.Controls.Add(Me.cboAuthentication)
        Me.grpDBConnect.Controls.Add(Me.txtServerName)
        Me.grpDBConnect.Controls.Add(Me.lblServerName)
        Me.grpDBConnect.Location = New System.Drawing.Point(53, 40)
        Me.grpDBConnect.Name = "grpDBConnect"
        Me.grpDBConnect.Size = New System.Drawing.Size(408, 244)
        Me.grpDBConnect.TabIndex = 0
        Me.grpDBConnect.TabStop = False
        '
        'txtDatabase
        '
        Me.txtDatabase.Location = New System.Drawing.Point(134, 53)
        Me.txtDatabase.Name = "txtDatabase"
        Me.txtDatabase.Size = New System.Drawing.Size(208, 20)
        Me.txtDatabase.TabIndex = 16
        '
        'lblDatabase
        '
        Me.lblDatabase.AutoSize = True
        Me.lblDatabase.Location = New System.Drawing.Point(44, 56)
        Me.lblDatabase.Name = "lblDatabase"
        Me.lblDatabase.Size = New System.Drawing.Size(56, 13)
        Me.lblDatabase.TabIndex = 15
        Me.lblDatabase.Text = "Database:"
        '
        'grpLoginDetails
        '
        Me.grpLoginDetails.Controls.Add(Me.txtPassword)
        Me.grpLoginDetails.Controls.Add(Me.lblPassword)
        Me.grpLoginDetails.Controls.Add(Me.txtUsername)
        Me.grpLoginDetails.Controls.Add(Me.lblUsername)
        Me.grpLoginDetails.Location = New System.Drawing.Point(47, 124)
        Me.grpLoginDetails.Name = "grpLoginDetails"
        Me.grpLoginDetails.Size = New System.Drawing.Size(304, 88)
        Me.grpLoginDetails.TabIndex = 10
        Me.grpLoginDetails.TabStop = False
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(87, 49)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(203, 20)
        Me.txtPassword.TabIndex = 5
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(11, 52)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblPassword.TabIndex = 4
        Me.lblPassword.Text = "Password:"
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(87, 22)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(203, 20)
        Me.txtUsername.TabIndex = 3
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(11, 25)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(61, 13)
        Me.lblUsername.TabIndex = 2
        Me.lblUsername.Text = "User name:"
        '
        'lblAuthentication
        '
        Me.lblAuthentication.AutoSize = True
        Me.lblAuthentication.Location = New System.Drawing.Point(44, 87)
        Me.lblAuthentication.Name = "lblAuthentication"
        Me.lblAuthentication.Size = New System.Drawing.Size(78, 13)
        Me.lblAuthentication.TabIndex = 9
        Me.lblAuthentication.Text = "Authentication:"
        '
        'cboAuthentication
        '
        Me.cboAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAuthentication.FormattingEnabled = True
        Me.cboAuthentication.Items.AddRange(New Object() {""})
        Me.cboAuthentication.Location = New System.Drawing.Point(134, 84)
        Me.cboAuthentication.Name = "cboAuthentication"
        Me.cboAuthentication.Size = New System.Drawing.Size(208, 21)
        Me.cboAuthentication.TabIndex = 8
        '
        'txtServerName
        '
        Me.txtServerName.Location = New System.Drawing.Point(134, 24)
        Me.txtServerName.Name = "txtServerName"
        Me.txtServerName.Size = New System.Drawing.Size(208, 20)
        Me.txtServerName.TabIndex = 7
        '
        'lblServerName
        '
        Me.lblServerName.AutoSize = True
        Me.lblServerName.Location = New System.Drawing.Point(44, 27)
        Me.lblServerName.Name = "lblServerName"
        Me.lblServerName.Size = New System.Drawing.Size(75, 13)
        Me.lblServerName.TabIndex = 6
        Me.lblServerName.Text = "Server Name: "
        '
        'TabUtlity
        '
        Me.TabUtlity.Controls.Add(Me.lblInfo)
        Me.TabUtlity.Controls.Add(Me.grpUtlity)
        Me.TabUtlity.Location = New System.Drawing.Point(4, 22)
        Me.TabUtlity.Name = "TabUtlity"
        Me.TabUtlity.Padding = New System.Windows.Forms.Padding(3)
        Me.TabUtlity.Size = New System.Drawing.Size(505, 452)
        Me.TabUtlity.TabIndex = 1
        Me.TabUtlity.Text = "Run Utlity"
        Me.TabUtlity.UseVisualStyleBackColor = True
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.lblInfo.Location = New System.Drawing.Point(162, 45)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(159, 24)
        Me.lblInfo.TabIndex = 27
        Me.lblInfo.Text = "Please Login First"
        '
        'grpUtlity
        '
        Me.grpUtlity.Controls.Add(Me.btnExecute)
        Me.grpUtlity.Controls.Add(Me.grpSelectDataModel)
        Me.grpUtlity.Controls.Add(Me.grpInclude)
        Me.grpUtlity.Controls.Add(Me.grpFileDetails)
        Me.grpUtlity.Controls.Add(Me.grpActionSelection)
        Me.grpUtlity.Location = New System.Drawing.Point(3, 6)
        Me.grpUtlity.Name = "grpUtlity"
        Me.grpUtlity.Size = New System.Drawing.Size(508, 450)
        Me.grpUtlity.TabIndex = 1
        Me.grpUtlity.TabStop = False
        '
        'btnExecute
        '
        Me.btnExecute.Location = New System.Drawing.Point(149, 403)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(228, 31)
        Me.btnExecute.TabIndex = 25
        Me.btnExecute.Text = "Run Import"
        Me.btnExecute.UseVisualStyleBackColor = True
        '
        'grpSelectDataModel
        '
        Me.grpSelectDataModel.Controls.Add(Me.cboDataModel)
        Me.grpSelectDataModel.Location = New System.Drawing.Point(37, 106)
        Me.grpSelectDataModel.Name = "grpSelectDataModel"
        Me.grpSelectDataModel.Size = New System.Drawing.Size(436, 54)
        Me.grpSelectDataModel.TabIndex = 23
        Me.grpSelectDataModel.TabStop = False
        Me.grpSelectDataModel.Text = "Please Select Data Model"
        '
        'cboDataModel
        '
        Me.cboDataModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDataModel.FormattingEnabled = True
        Me.cboDataModel.Location = New System.Drawing.Point(35, 19)
        Me.cboDataModel.Name = "cboDataModel"
        Me.cboDataModel.Size = New System.Drawing.Size(340, 21)
        Me.cboDataModel.TabIndex = 0
        '
        'grpInclude
        '
        Me.grpInclude.Controls.Add(Me.chkProductandRisk)
        Me.grpInclude.Controls.Add(Me.chkAllUMLsInDatabase)
        Me.grpInclude.Controls.Add(Me.chkAllUDLsInDatabase)
        Me.grpInclude.Controls.Add(Me.chkDataModelRelatedUMLs)
        Me.grpInclude.Controls.Add(Me.chkDataModelRelatedUDLs)
        Me.grpInclude.Controls.Add(Me.chkScreenDesign)
        Me.grpInclude.Location = New System.Drawing.Point(37, 244)
        Me.grpInclude.Name = "grpInclude"
        Me.grpInclude.Size = New System.Drawing.Size(436, 153)
        Me.grpInclude.TabIndex = 22
        Me.grpInclude.TabStop = False
        Me.grpInclude.Text = "Include :"
        '
        'chkProductandRisk
        '
        Me.chkProductandRisk.AutoSize = True
        Me.chkProductandRisk.Location = New System.Drawing.Point(35, 42)
        Me.chkProductandRisk.Name = "chkProductandRisk"
        Me.chkProductandRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkProductandRisk.Size = New System.Drawing.Size(173, 17)
        Me.chkProductandRisk.TabIndex = 14
        Me.chkProductandRisk.Text = "Associated Products and Risks"
        Me.chkProductandRisk.UseVisualStyleBackColor = True
        '
        'chkAllUMLsInDatabase
        '
        Me.chkAllUMLsInDatabase.AutoSize = True
        Me.chkAllUMLsInDatabase.Location = New System.Drawing.Point(35, 134)
        Me.chkAllUMLsInDatabase.Name = "chkAllUMLsInDatabase"
        Me.chkAllUMLsInDatabase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllUMLsInDatabase.Size = New System.Drawing.Size(129, 17)
        Me.chkAllUMLsInDatabase.TabIndex = 13
        Me.chkAllUMLsInDatabase.Text = "All UMLs In Database"
        Me.chkAllUMLsInDatabase.UseVisualStyleBackColor = True
        '
        'chkAllUDLsInDatabase
        '
        Me.chkAllUDLsInDatabase.AutoSize = True
        Me.chkAllUDLsInDatabase.Location = New System.Drawing.Point(35, 111)
        Me.chkAllUDLsInDatabase.Name = "chkAllUDLsInDatabase"
        Me.chkAllUDLsInDatabase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllUDLsInDatabase.Size = New System.Drawing.Size(128, 17)
        Me.chkAllUDLsInDatabase.TabIndex = 12
        Me.chkAllUDLsInDatabase.Text = "All UDLs In Database"
        Me.chkAllUDLsInDatabase.UseVisualStyleBackColor = True
        '
        'chkDataModelRelatedUMLs
        '
        Me.chkDataModelRelatedUMLs.AutoSize = True
        Me.chkDataModelRelatedUMLs.Location = New System.Drawing.Point(35, 88)
        Me.chkDataModelRelatedUMLs.Name = "chkDataModelRelatedUMLs"
        Me.chkDataModelRelatedUMLs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDataModelRelatedUMLs.Size = New System.Drawing.Size(152, 17)
        Me.chkDataModelRelatedUMLs.TabIndex = 11
        Me.chkDataModelRelatedUMLs.Text = "Data Model Related UMLs"
        Me.chkDataModelRelatedUMLs.UseVisualStyleBackColor = True
        '
        'chkDataModelRelatedUDLs
        '
        Me.chkDataModelRelatedUDLs.AutoSize = True
        Me.chkDataModelRelatedUDLs.Location = New System.Drawing.Point(35, 65)
        Me.chkDataModelRelatedUDLs.Name = "chkDataModelRelatedUDLs"
        Me.chkDataModelRelatedUDLs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDataModelRelatedUDLs.Size = New System.Drawing.Size(151, 17)
        Me.chkDataModelRelatedUDLs.TabIndex = 10
        Me.chkDataModelRelatedUDLs.Text = "Data Model Related UDLs"
        Me.chkDataModelRelatedUDLs.UseVisualStyleBackColor = True
        '
        'chkScreenDesign
        '
        Me.chkScreenDesign.AutoSize = True
        Me.chkScreenDesign.Location = New System.Drawing.Point(35, 19)
        Me.chkScreenDesign.Name = "chkScreenDesign"
        Me.chkScreenDesign.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkScreenDesign.Size = New System.Drawing.Size(96, 17)
        Me.chkScreenDesign.TabIndex = 9
        Me.chkScreenDesign.Text = "Screen Design"
        Me.chkScreenDesign.UseVisualStyleBackColor = True
        '
        'grpFileDetails
        '
        Me.grpFileDetails.Controls.Add(Me.txtFileName)
        Me.grpFileDetails.Controls.Add(Me.btnBrowseLocation)
        Me.grpFileDetails.Location = New System.Drawing.Point(37, 176)
        Me.grpFileDetails.Name = "grpFileDetails"
        Me.grpFileDetails.Size = New System.Drawing.Size(436, 62)
        Me.grpFileDetails.TabIndex = 21
        Me.grpFileDetails.TabStop = False
        Me.grpFileDetails.Text = "Please Provide File Details"
        '
        'txtFileName
        '
        Me.txtFileName.Location = New System.Drawing.Point(35, 19)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.ReadOnly = True
        Me.txtFileName.Size = New System.Drawing.Size(315, 20)
        Me.txtFileName.TabIndex = 3
        '
        'btnBrowseLocation
        '
        Me.btnBrowseLocation.Location = New System.Drawing.Point(356, 17)
        Me.btnBrowseLocation.Name = "btnBrowseLocation"
        Me.btnBrowseLocation.Size = New System.Drawing.Size(74, 23)
        Me.btnBrowseLocation.TabIndex = 4
        Me.btnBrowseLocation.Text = "..."
        Me.btnBrowseLocation.UseVisualStyleBackColor = True
        '
        'grpActionSelection
        '
        Me.grpActionSelection.Controls.Add(Me.optExport)
        Me.grpActionSelection.Controls.Add(Me.optImport)
        Me.grpActionSelection.Location = New System.Drawing.Point(37, 19)
        Me.grpActionSelection.Name = "grpActionSelection"
        Me.grpActionSelection.Size = New System.Drawing.Size(436, 69)
        Me.grpActionSelection.TabIndex = 20
        Me.grpActionSelection.TabStop = False
        Me.grpActionSelection.Text = "Please Select Action"
        '
        'optExport
        '
        Me.optExport.AutoSize = True
        Me.optExport.Checked = True
        Me.optExport.Location = New System.Drawing.Point(35, 19)
        Me.optExport.Name = "optExport"
        Me.optExport.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.optExport.Size = New System.Drawing.Size(113, 17)
        Me.optExport.TabIndex = 3
        Me.optExport.TabStop = True
        Me.optExport.Text = "Export Data Model"
        Me.optExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.optExport.UseVisualStyleBackColor = True
        '
        'optImport
        '
        Me.optImport.AutoSize = True
        Me.optImport.Location = New System.Drawing.Point(35, 42)
        Me.optImport.Name = "optImport"
        Me.optImport.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.optImport.Size = New System.Drawing.Size(112, 17)
        Me.optImport.TabIndex = 2
        Me.optImport.Text = "Import Data Model"
        Me.optImport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.optImport.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(518, 483)
        Me.Controls.Add(Me.APIETabs)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.Text = "Data Model Import Export Utility"
        Me.APIETabs.ResumeLayout(False)
        Me.TabLogin.ResumeLayout(False)
        Me.grplogin.ResumeLayout(False)
        Me.grpDBConnect.ResumeLayout(False)
        Me.grpDBConnect.PerformLayout()
        Me.grpLoginDetails.ResumeLayout(False)
        Me.grpLoginDetails.PerformLayout()
        Me.TabUtlity.ResumeLayout(False)
        Me.TabUtlity.PerformLayout()
        Me.grpUtlity.ResumeLayout(False)
        Me.grpSelectDataModel.ResumeLayout(False)
        Me.grpInclude.ResumeLayout(False)
        Me.grpInclude.PerformLayout()
        Me.grpFileDetails.ResumeLayout(False)
        Me.grpFileDetails.PerformLayout()
        Me.grpActionSelection.ResumeLayout(False)
        Me.grpActionSelection.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents APIETabs As System.Windows.Forms.TabControl
    Friend WithEvents TabLogin As System.Windows.Forms.TabPage
    Friend WithEvents grpDBConnect As System.Windows.Forms.GroupBox
    Friend WithEvents lblDatabase As System.Windows.Forms.Label
    Friend WithEvents grpLoginDetails As System.Windows.Forms.GroupBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents lblAuthentication As System.Windows.Forms.Label
    Friend WithEvents cboAuthentication As System.Windows.Forms.ComboBox
    Friend WithEvents txtServerName As System.Windows.Forms.TextBox
    Friend WithEvents lblServerName As System.Windows.Forms.Label
    Friend WithEvents TabUtlity As System.Windows.Forms.TabPage
    Friend WithEvents grpUtlity As System.Windows.Forms.GroupBox
    Friend WithEvents btnExecute As System.Windows.Forms.Button
    Friend WithEvents grpSelectDataModel As System.Windows.Forms.GroupBox
    Friend WithEvents cboDataModel As System.Windows.Forms.ComboBox
    Friend WithEvents grpInclude As System.Windows.Forms.GroupBox
    Friend WithEvents chkAllUMLsInDatabase As System.Windows.Forms.CheckBox
    Friend WithEvents chkAllUDLsInDatabase As System.Windows.Forms.CheckBox
    Friend WithEvents chkDataModelRelatedUMLs As System.Windows.Forms.CheckBox
    Friend WithEvents chkDataModelRelatedUDLs As System.Windows.Forms.CheckBox
    Friend WithEvents chkScreenDesign As System.Windows.Forms.CheckBox
    Friend WithEvents grpFileDetails As System.Windows.Forms.GroupBox
    Friend WithEvents txtFileName As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseLocation As System.Windows.Forms.Button
    Friend WithEvents grpActionSelection As System.Windows.Forms.GroupBox
    Friend WithEvents optExport As System.Windows.Forms.RadioButton
    Friend WithEvents optImport As System.Windows.Forms.RadioButton
    Friend WithEvents grplogin As System.Windows.Forms.GroupBox
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents txtDatabase As System.Windows.Forms.TextBox
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents dlgOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents dlgSaveFile As System.Windows.Forms.SaveFileDialog
    Friend WithEvents chkProductandRisk As System.Windows.Forms.CheckBox

End Class
