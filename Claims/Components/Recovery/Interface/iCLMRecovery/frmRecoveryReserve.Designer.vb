<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRecoveryReserve
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.grpRecoveryReserve = New System.Windows.Forms.GroupBox()
        Me.cboPartyType = New PMLookupControl.cboPMLookup()
        Me.cmdGetParty = New System.Windows.Forms.Button()
        Me.txtPartyCode = New System.Windows.Forms.TextBox()
        Me.txtTotalReserve = New System.Windows.Forms.TextBox()
        Me.txtThisRevision = New System.Windows.Forms.TextBox()
        Me.cboRecoveryType = New System.Windows.Forms.ComboBox()
        Me.txtInitialReserve = New System.Windows.Forms.TextBox()
        Me.txtRevisedReserve = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblTotalReserve = New System.Windows.Forms.Label()
        Me.lblThisRevision = New System.Windows.Forms.Label()
        Me.lblRecoveryType = New System.Windows.Forms.Label()
        Me.lblInitialReserve = New System.Windows.Forms.Label()
        Me.lblRevisedReserve = New System.Windows.Forms.Label()
        Me.grpRecoveryReserve.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' cboPartyType
        ' 
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(166, 209)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(80, 22)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(252, 209)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(80, 22)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'grpRecoveryReserve
        '
        Me.grpRecoveryReserve.Controls.Add(Me.cboPartyType)
        Me.grpRecoveryReserve.Controls.Add(Me.cmdGetParty)
        Me.grpRecoveryReserve.Controls.Add(Me.txtPartyCode)
        Me.grpRecoveryReserve.Controls.Add(Me.txtTotalReserve)
        Me.grpRecoveryReserve.Controls.Add(Me.txtThisRevision)
        Me.grpRecoveryReserve.Controls.Add(Me.cboRecoveryType)
        Me.grpRecoveryReserve.Controls.Add(Me.txtInitialReserve)
        Me.grpRecoveryReserve.Controls.Add(Me.txtRevisedReserve)
        Me.grpRecoveryReserve.Controls.Add(Me.Label2)
        Me.grpRecoveryReserve.Controls.Add(Me.Label1)
        Me.grpRecoveryReserve.Controls.Add(Me.lblTotalReserve)
        Me.grpRecoveryReserve.Controls.Add(Me.lblThisRevision)
        Me.grpRecoveryReserve.Controls.Add(Me.lblRecoveryType)
        Me.grpRecoveryReserve.Controls.Add(Me.lblInitialReserve)
        Me.grpRecoveryReserve.Controls.Add(Me.lblRevisedReserve)
        Me.grpRecoveryReserve.Location = New System.Drawing.Point(7, -2)
        Me.grpRecoveryReserve.Name = "grpRecoveryReserve"
        Me.grpRecoveryReserve.Size = New System.Drawing.Size(327, 209)
        Me.grpRecoveryReserve.TabIndex = 10
        Me.grpRecoveryReserve.TabStop = False
        '
        'cboPartyType
        '
        Me.cboPartyType.DefaultItemId = 0
        Me.cboPartyType.FirstItem = ""
        Me.cboPartyType.ItemId = 0
        Me.cboPartyType.ListIndex = -1
        Me.cboPartyType.Location = New System.Drawing.Point(121, 38)
        Me.cboPartyType.Name = "cboPartyType"
        Me.cboPartyType.PMLookupProductFamily = 1
        Me.cboPartyType.SingleItemId = 0
        Me.cboPartyType.Size = New System.Drawing.Size(199, 21)
        Me.cboPartyType.Sorted = True
        Me.cboPartyType.TabIndex = 18
        Me.cboPartyType.TableName = "Recovery_Party_Type"
        Me.cboPartyType.ToolTipText = ""
        Me.cboPartyType.WhereClause = ""
        ' 
        'cmdGetParty
        ' 
        Me.cmdGetParty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGetParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGetParty.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGetParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGetParty.Location = New System.Drawing.Point(285, 66)
        Me.cmdGetParty.Name = "cmdGetParty"
        Me.cmdGetParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGetParty.Size = New System.Drawing.Size(27, 19)
        Me.cmdGetParty.TabIndex = 20
        Me.cmdGetParty.Text = "..."
        Me.cmdGetParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGetParty.UseVisualStyleBackColor = False
        ' 
        'txtPartyCode
        ' 
        Me.txtPartyCode.AcceptsReturn = True
        Me.txtPartyCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPartyCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyCode.Enabled = False
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyCode.Location = New System.Drawing.Point(121, 66)
        Me.txtPartyCode.MaxLength = 0
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyCode.Size = New System.Drawing.Size(160, 20)
        Me.txtPartyCode.TabIndex = 19
        ' 
        'txtTotalReserve
        ' 
        Me.txtTotalReserve.AcceptsReturn = True
        Me.txtTotalReserve.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalReserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalReserve.Location = New System.Drawing.Point(121, 169)
        Me.txtTotalReserve.MaxLength = 15
        Me.txtTotalReserve.Name = "txtTotalReserve"
        Me.txtTotalReserve.ReadOnly = True
        Me.txtTotalReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalReserve.Size = New System.Drawing.Size(160, 20)
        Me.txtTotalReserve.TabIndex = 24
        ' 
        'txtThisRevision
        ' 
        Me.txtThisRevision.AcceptsReturn = True
        Me.txtThisRevision.BackColor = System.Drawing.SystemColors.Window
        Me.txtThisRevision.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThisRevision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThisRevision.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtThisRevision.Location = New System.Drawing.Point(121, 142)
        Me.txtThisRevision.MaxLength = 15
        Me.txtThisRevision.Name = "txtThisRevision"
        Me.txtThisRevision.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThisRevision.Size = New System.Drawing.Size(160, 20)
        Me.txtThisRevision.TabIndex = 23
        ' 
        'cboRecoveryType
        ' 
        Me.cboRecoveryType.BackColor = System.Drawing.SystemColors.Window
        Me.cboRecoveryType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRecoveryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRecoveryType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRecoveryType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRecoveryType.Location = New System.Drawing.Point(121, 11)
        Me.cboRecoveryType.Name = "cboRecoveryType"
        Me.cboRecoveryType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRecoveryType.Size = New System.Drawing.Size(200, 21)
        Me.cboRecoveryType.TabIndex = 17
        ' 
        'txtInitialReserve
        ' 
        Me.txtInitialReserve.AcceptsReturn = True
        Me.txtInitialReserve.BackColor = System.Drawing.SystemColors.Window
        Me.txtInitialReserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInitialReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInitialReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInitialReserve.Location = New System.Drawing.Point(121, 92)
        Me.txtInitialReserve.MaxLength = 15
        Me.txtInitialReserve.Name = "txtInitialReserve"
        Me.txtInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInitialReserve.Size = New System.Drawing.Size(160, 20)
        Me.txtInitialReserve.TabIndex = 21
        ' 
        'txtRevisedReserve
        ' 
        Me.txtRevisedReserve.AcceptsReturn = True
        Me.txtRevisedReserve.BackColor = System.Drawing.SystemColors.Control
        Me.txtRevisedReserve.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRevisedReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRevisedReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRevisedReserve.Location = New System.Drawing.Point(121, 117)
        Me.txtRevisedReserve.MaxLength = 15
        Me.txtRevisedReserve.Name = "txtRevisedReserve"
        Me.txtRevisedReserve.ReadOnly = True
        Me.txtRevisedReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRevisedReserve.Size = New System.Drawing.Size(160, 20)
        Me.txtRevisedReserve.TabIndex = 22
        ' 
        'Label2
        ' 
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(3, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Party:"
        ' 
        'Label1
        ' 
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(3, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Party Type:"
        ' 
        'lblTotalReserve
        ' 
        Me.lblTotalReserve.AutoSize = True
        Me.lblTotalReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalReserve.Location = New System.Drawing.Point(3, 172)
        Me.lblTotalReserve.Name = "lblTotalReserve"
        Me.lblTotalReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalReserve.Size = New System.Drawing.Size(77, 13)
        Me.lblTotalReserve.TabIndex = 29
        Me.lblTotalReserve.Text = "Total Reserve:"
        ' 
        'lblThisRevision
        ' 
        Me.lblThisRevision.AutoSize = True
        Me.lblThisRevision.BackColor = System.Drawing.SystemColors.Control
        Me.lblThisRevision.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThisRevision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThisRevision.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblThisRevision.Location = New System.Drawing.Point(3, 147)
        Me.lblThisRevision.Name = "lblThisRevision"
        Me.lblThisRevision.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThisRevision.Size = New System.Drawing.Size(74, 13)
        Me.lblThisRevision.TabIndex = 28
        Me.lblThisRevision.Text = "This Revision:"
        ' 
        'lblRecoveryType
        ' 
        Me.lblRecoveryType.AutoSize = True
        Me.lblRecoveryType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRecoveryType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRecoveryType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecoveryType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRecoveryType.Location = New System.Drawing.Point(3, 15)
        Me.lblRecoveryType.Name = "lblRecoveryType"
        Me.lblRecoveryType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRecoveryType.Size = New System.Drawing.Size(83, 13)
        Me.lblRecoveryType.TabIndex = 27
        Me.lblRecoveryType.Text = "Recovery Type:"
        ' 
        'lblInitialReserve
        ' 
        Me.lblInitialReserve.AutoSize = True
        Me.lblInitialReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblInitialReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInitialReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInitialReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInitialReserve.Location = New System.Drawing.Point(3, 95)
        Me.lblInitialReserve.Name = "lblInitialReserve"
        Me.lblInitialReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInitialReserve.Size = New System.Drawing.Size(77, 13)
        Me.lblInitialReserve.TabIndex = 26
        Me.lblInitialReserve.Text = "Initial Reserve:"
        ' 
        'lblRevisedReserve
        ' 
        Me.lblRevisedReserve.AutoSize = True
        Me.lblRevisedReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblRevisedReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRevisedReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRevisedReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRevisedReserve.Location = New System.Drawing.Point(3, 120)
        Me.lblRevisedReserve.Name = "lblRevisedReserve"
        Me.lblRevisedReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRevisedReserve.Size = New System.Drawing.Size(92, 13)
        Me.lblRevisedReserve.TabIndex = 25
        Me.lblRevisedReserve.Text = "Revised Reserve:"
        ' 
        'frmRecoveryReserve
        ' 
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(340, 234)
        Me.Controls.Add(Me.grpRecoveryReserve)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRecoveryReserve"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Recovery Reserve"
        Me.grpRecoveryReserve.ResumeLayout(False)
        Me.grpRecoveryReserve.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpRecoveryReserve As System.Windows.Forms.GroupBox
    Public WithEvents cboPartyType As PMLookupControl.cboPMLookup
    Public WithEvents cmdGetParty As System.Windows.Forms.Button
    Public WithEvents txtPartyCode As System.Windows.Forms.TextBox
    Public WithEvents txtTotalReserve As System.Windows.Forms.TextBox
    Public WithEvents txtThisRevision As System.Windows.Forms.TextBox
    Public WithEvents cboRecoveryType As System.Windows.Forms.ComboBox
    Public WithEvents txtInitialReserve As System.Windows.Forms.TextBox
    Public WithEvents txtRevisedReserve As System.Windows.Forms.TextBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblTotalReserve As System.Windows.Forms.Label
    Public WithEvents lblThisRevision As System.Windows.Forms.Label
    Public WithEvents lblRecoveryType As System.Windows.Forms.Label
    Public WithEvents lblInitialReserve As System.Windows.Forms.Label
    Public WithEvents lblRevisedReserve As System.Windows.Forms.Label
#End Region
End Class