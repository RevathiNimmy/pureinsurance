<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.serverGrid = New System.Windows.Forms.DataGridView()
        Me.outputText = New System.Windows.Forms.TextBox()
        Me.timer = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.labelQueueDepth = New System.Windows.Forms.Label()
        Me.BatchIdDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProcessedDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FailedDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.InProgressDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MinDurationDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MaxDurationDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AverageDurationDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RunStatisticBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.serverGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.RunStatisticBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'serverGrid
        '
        Me.serverGrid.AllowUserToAddRows = False
        Me.serverGrid.AllowUserToDeleteRows = False
        Me.serverGrid.AutoGenerateColumns = False
        Me.serverGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.serverGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.BatchIdDataGridViewTextBoxColumn, Me.ProcessedDataGridViewTextBoxColumn, Me.FailedDataGridViewTextBoxColumn, Me.InProgressDataGridViewTextBoxColumn, Me.MinDurationDataGridViewTextBoxColumn, Me.MaxDurationDataGridViewTextBoxColumn, Me.AverageDurationDataGridViewTextBoxColumn})
        Me.serverGrid.DataSource = Me.RunStatisticBindingSource
        Me.serverGrid.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.serverGrid.Location = New System.Drawing.Point(10, 283)
        Me.serverGrid.MultiSelect = False
        Me.serverGrid.Name = "serverGrid"
        Me.serverGrid.ReadOnly = True
        Me.serverGrid.RowHeadersVisible = False
        Me.serverGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.serverGrid.ShowEditingIcon = False
        Me.serverGrid.Size = New System.Drawing.Size(703, 150)
        Me.serverGrid.TabIndex = 0
        '
        'outputText
        '
        Me.outputText.AcceptsReturn = True
        Me.outputText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.outputText.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.outputText.Location = New System.Drawing.Point(10, 10)
        Me.outputText.Multiline = True
        Me.outputText.Name = "outputText"
        Me.outputText.ReadOnly = True
        Me.outputText.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.outputText.Size = New System.Drawing.Size(703, 237)
        Me.outputText.TabIndex = 1
        Me.outputText.WordWrap = False
        '
        'timer
        '
        Me.timer.Interval = 2500
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.labelQueueDepth)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(10, 247)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(703, 36)
        Me.Panel1.TabIndex = 2
        '
        'labelQueueDepth
        '
        Me.labelQueueDepth.AutoSize = True
        Me.labelQueueDepth.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelQueueDepth.Location = New System.Drawing.Point(4, 7)
        Me.labelQueueDepth.Name = "labelQueueDepth"
        Me.labelQueueDepth.Size = New System.Drawing.Size(0, 24)
        Me.labelQueueDepth.TabIndex = 0
        '
        'BatchIdDataGridViewTextBoxColumn
        '
        Me.BatchIdDataGridViewTextBoxColumn.DataPropertyName = "BatchId"
        Me.BatchIdDataGridViewTextBoxColumn.HeaderText = "BatchId"
        Me.BatchIdDataGridViewTextBoxColumn.Name = "BatchIdDataGridViewTextBoxColumn"
        Me.BatchIdDataGridViewTextBoxColumn.ReadOnly = True
        '
        'ProcessedDataGridViewTextBoxColumn
        '
        Me.ProcessedDataGridViewTextBoxColumn.DataPropertyName = "Processed"
        Me.ProcessedDataGridViewTextBoxColumn.HeaderText = "Processed"
        Me.ProcessedDataGridViewTextBoxColumn.Name = "ProcessedDataGridViewTextBoxColumn"
        Me.ProcessedDataGridViewTextBoxColumn.ReadOnly = True
        '
        'FailedDataGridViewTextBoxColumn
        '
        Me.FailedDataGridViewTextBoxColumn.DataPropertyName = "Failed"
        Me.FailedDataGridViewTextBoxColumn.HeaderText = "Failed"
        Me.FailedDataGridViewTextBoxColumn.Name = "FailedDataGridViewTextBoxColumn"
        Me.FailedDataGridViewTextBoxColumn.ReadOnly = True
        '
        'InProgressDataGridViewTextBoxColumn
        '
        Me.InProgressDataGridViewTextBoxColumn.DataPropertyName = "InProgress"
        Me.InProgressDataGridViewTextBoxColumn.HeaderText = "InProgress"
        Me.InProgressDataGridViewTextBoxColumn.Name = "InProgressDataGridViewTextBoxColumn"
        Me.InProgressDataGridViewTextBoxColumn.ReadOnly = True
        '
        'MinDurationDataGridViewTextBoxColumn
        '
        Me.MinDurationDataGridViewTextBoxColumn.DataPropertyName = "MinDuration"
        DataGridViewCellStyle1.Format = "N2"
        DataGridViewCellStyle1.NullValue = Nothing
        Me.MinDurationDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle1
        Me.MinDurationDataGridViewTextBoxColumn.HeaderText = "Min (ms)"
        Me.MinDurationDataGridViewTextBoxColumn.Name = "MinDurationDataGridViewTextBoxColumn"
        Me.MinDurationDataGridViewTextBoxColumn.ReadOnly = True
        '
        'MaxDurationDataGridViewTextBoxColumn
        '
        Me.MaxDurationDataGridViewTextBoxColumn.DataPropertyName = "MaxDuration"
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.MaxDurationDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.MaxDurationDataGridViewTextBoxColumn.HeaderText = "Max (ms)"
        Me.MaxDurationDataGridViewTextBoxColumn.Name = "MaxDurationDataGridViewTextBoxColumn"
        Me.MaxDurationDataGridViewTextBoxColumn.ReadOnly = True
        '
        'AverageDurationDataGridViewTextBoxColumn
        '
        Me.AverageDurationDataGridViewTextBoxColumn.DataPropertyName = "AverageDuration"
        DataGridViewCellStyle3.Format = "N2"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.AverageDurationDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle3
        Me.AverageDurationDataGridViewTextBoxColumn.HeaderText = "Average (ms)"
        Me.AverageDurationDataGridViewTextBoxColumn.Name = "AverageDurationDataGridViewTextBoxColumn"
        Me.AverageDurationDataGridViewTextBoxColumn.ReadOnly = True
        '
        'RunStatisticBindingSource
        '
        Me.RunStatisticBindingSource.DataSource = GetType(Pure.DMEMigration.Controller.RunStatistic)
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(723, 443)
        Me.Controls.Add(Me.outputText)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.serverGrid)
        Me.Name = "MainForm"
        Me.Opacity = 0.0R
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DME Migration Progress"
        CType(Me.serverGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.RunStatisticBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents serverGrid As System.Windows.Forms.DataGridView
    Friend WithEvents outputText As System.Windows.Forms.TextBox
    Friend WithEvents RunStatisticBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents timer As System.Windows.Forms.Timer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents labelQueueDepth As System.Windows.Forms.Label
    Friend WithEvents BatchIdDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ProcessedDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FailedDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents InProgressDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MinDurationDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MaxDurationDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AverageDurationDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
