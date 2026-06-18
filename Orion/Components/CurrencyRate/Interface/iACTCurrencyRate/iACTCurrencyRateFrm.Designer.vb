<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		Form_Initialize_Renamed()
	End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
     Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents uctAnchor As uSIRCommonControls.uctAnchor
    Public WithEvents divFilter As uSIRCommonControls.uctDivider
    Public WithEvents cmdEffectiveDateBack As System.Windows.Forms.Button
    Public WithEvents cmdEffectiveDateForward As System.Windows.Forms.Button
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents dtpEffectiveDate As System.Windows.Forms.DateTimePicker
    Public WithEvents cboBranch As PMLookupControl.cboPMLookup
    Public WithEvents divRates As uSIRCommonControls.uctDivider
    Public WithEvents lblAllBranches As System.Windows.Forms.Label
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.uctAnchor = New uSIRCommonControls.uctAnchor
        Me.divFilter = New uSIRCommonControls.uctDivider
        Me.cmdEffectiveDateBack = New System.Windows.Forms.Button
        Me.cmdEffectiveDateForward = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.dtpEffectiveDate = New System.Windows.Forms.DateTimePicker
        Me.cboBranch = New PMLookupControl.cboPMLookup
        Me.divRates = New uSIRCommonControls.uctDivider
        Me.lblAllBranches = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.grdMainData = New System.Windows.Forms.DataGridView
        CType(Me.grdMainData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(476, 302)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 2
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdApply, "This will apply the currecny rates to all branches")
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(56, 306)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 0
        Me.uctAnchor.Visible = False
        '
        'divFilter
        '
        Me.divFilter.Caption = "Filters"
        Me.divFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.divFilter.Location = New System.Drawing.Point(6, 8)
        Me.divFilter.Name = "divFilter"
        Me.divFilter.Size = New System.Drawing.Size(625, 21)
        Me.divFilter.TabIndex = 10
        '
        'cmdEffectiveDateBack
        '
        Me.cmdEffectiveDateBack.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEffectiveDateBack.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEffectiveDateBack.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEffectiveDateBack.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEffectiveDateBack.Location = New System.Drawing.Point(570, 30)
        Me.cmdEffectiveDateBack.Name = "cmdEffectiveDateBack"
        Me.cmdEffectiveDateBack.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEffectiveDateBack.Size = New System.Drawing.Size(17, 21)
        Me.cmdEffectiveDateBack.TabIndex = 5
        Me.cmdEffectiveDateBack.Text = "<"
        Me.cmdEffectiveDateBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEffectiveDateBack.UseVisualStyleBackColor = False
        '
        'cmdEffectiveDateForward
        '
        Me.cmdEffectiveDateForward.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEffectiveDateForward.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEffectiveDateForward.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEffectiveDateForward.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEffectiveDateForward.Location = New System.Drawing.Point(586, 30)
        Me.cmdEffectiveDateForward.Name = "cmdEffectiveDateForward"
        Me.cmdEffectiveDateForward.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEffectiveDateForward.Size = New System.Drawing.Size(17, 21)
        Me.cmdEffectiveDateForward.TabIndex = 4
        Me.cmdEffectiveDateForward.Text = ">"
        Me.cmdEffectiveDateForward.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEffectiveDateForward.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(556, 302)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(396, 302)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'dtpEffectiveDate
        '
        Me.dtpEffectiveDate.CustomFormat = ""
        Me.dtpEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEffectiveDate.Location = New System.Drawing.Point(416, 30)
        Me.dtpEffectiveDate.Name = "dtpEffectiveDate"
        Me.dtpEffectiveDate.Size = New System.Drawing.Size(129, 21)
        Me.dtpEffectiveDate.TabIndex = 6
        '
        'cboBranch
        '
        Me.cboBranch.DefaultItemId = 0
        Me.cboBranch.Enabled = False
        Me.cboBranch.FirstItem = ""
        Me.cboBranch.ItemId = 0
        Me.cboBranch.ListIndex = -1
        Me.cboBranch.Location = New System.Drawing.Point(70, 30)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.PMLookupProductFamily = 1
        Me.cboBranch.SingleItemId = 0
        Me.cboBranch.Size = New System.Drawing.Size(233, 21)
        Me.cboBranch.Sorted = True
        Me.cboBranch.TabIndex = 7
        Me.cboBranch.TableName = "Source"
        Me.cboBranch.ToolTipText = ""
        Me.cboBranch.WhereClause = ""
        '
        'divRates
        '
        Me.divRates.Caption = "Rates"
        Me.divRates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.divRates.Location = New System.Drawing.Point(6, 62)
        Me.divRates.Name = "divRates"
        Me.divRates.Size = New System.Drawing.Size(625, 21)
        Me.divRates.TabIndex = 11
        '
        'lblAllBranches
        '
        Me.lblAllBranches.AutoSize = True
        Me.lblAllBranches.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllBranches.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllBranches.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllBranches.Location = New System.Drawing.Point(152, 304)
        Me.lblAllBranches.Name = "lblAllBranches"
        Me.lblAllBranches.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllBranches.Size = New System.Drawing.Size(39, 13)
        Me.lblAllBranches.TabIndex = 12
        Me.lblAllBranches.Text = "Label1"
        Me.lblAllBranches.Visible = False
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(18, 34)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(44, 13)
        Me.lblBranch.TabIndex = 9
        Me.lblBranch.Text = "Branch:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(320, 34)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(78, 13)
        Me.lblEffectiveDate.TabIndex = 8
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'grdMainData
        '
        Me.grdMainData.AllowUserToAddRows = False
        Me.grdMainData.AllowUserToDeleteRows = False
        Me.grdMainData.AllowUserToResizeRows = False
        Me.grdMainData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdMainData.Location = New System.Drawing.Point(13, 90)
        Me.grdMainData.Name = "grdMainData"
        Me.grdMainData.Size = New System.Drawing.Size(618, 206)
        Me.grdMainData.TabIndex = 13
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(638, 332)
        Me.Controls.Add(Me.grdMainData)
        Me.Controls.Add(Me.uctAnchor)
        Me.Controls.Add(Me.divFilter)
        Me.Controls.Add(Me.cmdEffectiveDateBack)
        Me.Controls.Add(Me.cmdEffectiveDateForward)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.dtpEffectiveDate)
        Me.Controls.Add(Me.cboBranch)
        Me.Controls.Add(Me.divRates)
        Me.Controls.Add(Me.lblAllBranches)
        Me.Controls.Add(Me.lblBranch)
        Me.Controls.Add(Me.lblEffectiveDate)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(176, 219)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Currency rates"
        CType(Me.grdMainData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grdMainData As System.Windows.Forms.DataGridView
#End Region 
End Class