<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCancelPolicy
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
    Public WithEvents txtPolicyLapseDate As System.Windows.Forms.TextBox
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdViewAccount As System.Windows.Forms.Button
    Public WithEvents cboLapseReason As PMLookupControl.cboPMLookup
    Public WithEvents chkSpoolDocument As System.Windows.Forms.CheckBox
    Public WithEvents chkWriteOff As System.Windows.Forms.CheckBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents lblLapseReason As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Image1 As System.Windows.Forms.PictureBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCancelPolicy))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtPolicyLapseDate = New System.Windows.Forms.TextBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdViewAccount = New System.Windows.Forms.Button
        Me.chkSpoolDocument = New System.Windows.Forms.CheckBox
        Me.chkWriteOff = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblLapseReason = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Image1 = New System.Windows.Forms.PictureBox
        Me.cboLapseReason = New PMLookupControl.cboPMLookup
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtPolicyLapseDate
        '
        Me.txtPolicyLapseDate.AcceptsReturn = True
        Me.txtPolicyLapseDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyLapseDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyLapseDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyLapseDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyLapseDate.Location = New System.Drawing.Point(130, 150)
        Me.txtPolicyLapseDate.MaxLength = 0
        Me.txtPolicyLapseDate.Name = "txtPolicyLapseDate"
        Me.txtPolicyLapseDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyLapseDate.Size = New System.Drawing.Size(136, 20)
        Me.txtPolicyLapseDate.TabIndex = 9
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(245, 181)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(97, 19)
        Me.cmdCancel.TabIndex = 7
        Me.cmdCancel.Text = "Do Not Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(192, 181)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(49, 19)
        Me.cmdOK.TabIndex = 6
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdViewAccount
        '
        Me.cmdViewAccount.BackColor = System.Drawing.SystemColors.Control
        Me.cmdViewAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdViewAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdViewAccount.Location = New System.Drawing.Point(8, 180)
        Me.cmdViewAccount.Name = "cmdViewAccount"
        Me.cmdViewAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdViewAccount.Size = New System.Drawing.Size(89, 20)
        Me.cmdViewAccount.TabIndex = 5
        Me.cmdViewAccount.Text = "View Account"
        Me.cmdViewAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdViewAccount.UseVisualStyleBackColor = False
        '
        'chkSpoolDocument
        '
        Me.chkSpoolDocument.BackColor = System.Drawing.SystemColors.Control
        Me.chkSpoolDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSpoolDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSpoolDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSpoolDocument.Location = New System.Drawing.Point(48, 90)
        Me.chkSpoolDocument.Name = "chkSpoolDocument"
        Me.chkSpoolDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSpoolDocument.Size = New System.Drawing.Size(241, 19)
        Me.chkSpoolDocument.TabIndex = 2
        Me.chkSpoolDocument.Text = "Spool Policy Cancellation Document"
        Me.chkSpoolDocument.UseVisualStyleBackColor = False
        '
        'chkWriteOff
        '
        Me.chkWriteOff.BackColor = System.Drawing.SystemColors.Control
        Me.chkWriteOff.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkWriteOff.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkWriteOff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWriteOff.Location = New System.Drawing.Point(48, 70)
        Me.chkWriteOff.Name = "chkWriteOff"
        Me.chkWriteOff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkWriteOff.Size = New System.Drawing.Size(161, 19)
        Me.chkWriteOff.TabIndex = 1
        Me.chkWriteOff.Text = "Write Off Difference"
        Me.chkWriteOff.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(5, 150)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(119, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Policy Lapse Date:"
        '
        'lblLapseReason
        '
        Me.lblLapseReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblLapseReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLapseReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLapseReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLapseReason.Location = New System.Drawing.Point(8, 119)
        Me.lblLapseReason.Name = "lblLapseReason"
        Me.lblLapseReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLapseReason.Size = New System.Drawing.Size(89, 17)
        Me.lblLapseReason.TabIndex = 3
        Me.lblLapseReason.Text = "Lapse Reason:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(48, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(280, 57)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "YOU ARE ABOUT TO CANCEL THE POLICIES ON THIS FINANCE PLAN                        " & _
            "                                                                                " & _
            "   ARE YOU SURE?"
        '
        'Image1
        '
        Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
        Me.Image1.Location = New System.Drawing.Point(8, 8)
        Me.Image1.Name = "Image1"
        Me.Image1.Size = New System.Drawing.Size(32, 32)
        Me.Image1.TabIndex = 10
        Me.Image1.TabStop = False
        '
        'cboLapseReason
        '
        Me.cboLapseReason.DefaultItemId = 0
        Me.cboLapseReason.FirstItem = ""
        Me.cboLapseReason.ItemId = 0
        Me.cboLapseReason.ListIndex = -1
        Me.cboLapseReason.Location = New System.Drawing.Point(130, 120)
        Me.cboLapseReason.Name = "cboLapseReason"
        Me.cboLapseReason.PMLookupProductFamily = 1
        Me.cboLapseReason.SingleItemId = 0
        Me.cboLapseReason.Size = New System.Drawing.Size(211, 21)
        Me.cboLapseReason.Sorted = True
        Me.cboLapseReason.TabIndex = 4
        Me.cboLapseReason.TableName = "Lapsed_Reason"
        Me.cboLapseReason.ToolTipText = ""
        Me.cboLapseReason.WhereClause = ""
        '
        'frmCancelPolicy
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(352, 209)
        Me.Controls.Add(Me.txtPolicyLapseDate)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdViewAccount)
        Me.Controls.Add(Me.cboLapseReason)
        Me.Controls.Add(Me.chkSpoolDocument)
        Me.Controls.Add(Me.chkWriteOff)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblLapseReason)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Image1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 30)
        Me.Name = "frmCancelPolicy"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cancel Policies                                 "
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region
End Class