<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCopyDocumentLink
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboLinkedDocument As System.Windows.Forms.ComboBox
	Public WithEvents cboProduct As PMLookupControl.cboPMLookup
	Public WithEvents lblProduct As System.Windows.Forms.Label
	Public WithEvents lblLinkedDocument As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cboLinkedDocument = New System.Windows.Forms.ComboBox
        Me.cboProduct = New PMLookupControl.cboPMLookup
        Me.lblProduct = New System.Windows.Forms.Label
        Me.lblLinkedDocument = New System.Windows.Forms.Label
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(269, 120)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(66, 22)
        Me.cmdCancel.TabIndex = 5
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
        Me.cmdOK.Location = New System.Drawing.Point(198, 120)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(65, 23)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cboLinkedDocument)
        Me.Frame1.Controls.Add(Me.cboProduct)
        Me.Frame1.Controls.Add(Me.lblProduct)
        Me.Frame1.Controls.Add(Me.lblLinkedDocument)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(5, 6)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(331, 107)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Copy Link from"
        '
        'cboLinkedDocument
        '
        Me.cboLinkedDocument.BackColor = System.Drawing.SystemColors.Window
        Me.cboLinkedDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLinkedDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLinkedDocument.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLinkedDocument.Location = New System.Drawing.Point(136, 52)
        Me.cboLinkedDocument.Name = "cboLinkedDocument"
        Me.cboLinkedDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLinkedDocument.Size = New System.Drawing.Size(177, 21)
        Me.cboLinkedDocument.TabIndex = 6
        '
        'cboProduct
        '
        Me.cboProduct.DefaultItemId = 0
        Me.cboProduct.FirstItem = ""
        Me.cboProduct.ItemId = 0
        Me.cboProduct.ListIndex = -1
        Me.cboProduct.Location = New System.Drawing.Point(137, 19)
        Me.cboProduct.Name = "cboProduct"
        Me.cboProduct.PMLookupProductFamily = 1
        Me.cboProduct.SingleItemId = 0
        Me.cboProduct.Size = New System.Drawing.Size(176, 21)
        Me.cboProduct.Sorted = True
        Me.cboProduct.TabIndex = 1
        Me.cboProduct.TableName = "Product"
        Me.cboProduct.ToolTipText = ""
        Me.cboProduct.WhereClause = ""
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(11, 21)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(116, 18)
        Me.lblProduct.TabIndex = 3
        Me.lblProduct.Text = "Product:"
        '
        'lblLinkedDocument
        '
        Me.lblLinkedDocument.BackColor = System.Drawing.SystemColors.Control
        Me.lblLinkedDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLinkedDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLinkedDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLinkedDocument.Location = New System.Drawing.Point(11, 54)
        Me.lblLinkedDocument.Name = "lblLinkedDocument"
        Me.lblLinkedDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLinkedDocument.Size = New System.Drawing.Size(116, 18)
        Me.lblLinkedDocument.TabIndex = 2
        Me.lblLinkedDocument.Text = "Linked Document:"
        '
        'frmCopyDocumentLink
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(343, 150)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Frame1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCopyDocumentLink"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Copy Document Link"
        Me.Frame1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class