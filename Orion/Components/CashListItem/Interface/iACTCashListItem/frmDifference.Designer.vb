<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDifference
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Form_Initialize_Renamed()
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not fTerminateCalled_Form_Terminate_Renamed Then
                fTerminateCalled_Form_Terminate_Renamed = True
                Form_Terminate_Renamed()
            End If
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
    Public WithEvents cmdTakeExact As System.Windows.Forms.Button
    Public WithEvents cmdWriteOff As System.Windows.Forms.Button
    Public WithEvents txtDifference As System.Windows.Forms.TextBox
    Public WithEvents txtInstalmentsMarked As System.Windows.Forms.TextBox
    Public WithEvents txtReceiptBaseCurrency As System.Windows.Forms.TextBox
    Public WithEvents txtReceiptAmount As System.Windows.Forms.TextBox
    Public WithEvents cboReceiptAmount As UserControls.CurrencyLookup
    Public WithEvents cboReceiptBaseCurrency As UserControls.CurrencyLookup
    Public WithEvents cboInstalmentsMarked As UserControls.CurrencyLookup
    Public WithEvents cboDifference As UserControls.CurrencyLookup
    Public WithEvents lblMessageBottom As System.Windows.Forms.Label
    Public WithEvents lblDifference As System.Windows.Forms.Label
    Public WithEvents lblInstalmentsMarked As System.Windows.Forms.Label
    Public WithEvents lblReceiptBaseCurrency As System.Windows.Forms.Label
    Public WithEvents lblReceiptAmount As System.Windows.Forms.Label
    Public WithEvents lblMessageTop As System.Windows.Forms.Label
    Public WithEvents Image1 As System.Windows.Forms.PictureBox
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDifference))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdTakeExact = New System.Windows.Forms.Button()
        Me.cmdWriteOff = New System.Windows.Forms.Button()
        Me.txtDifference = New System.Windows.Forms.TextBox()
        Me.txtInstalmentsMarked = New System.Windows.Forms.TextBox()
        Me.txtReceiptBaseCurrency = New System.Windows.Forms.TextBox()
        Me.txtReceiptAmount = New System.Windows.Forms.TextBox()
        Me.cboReceiptAmount = New UserControls.CurrencyLookup
        Me.cboReceiptBaseCurrency = New UserControls.CurrencyLookup
        Me.cboInstalmentsMarked = New UserControls.CurrencyLookup
        Me.cboDifference = New UserControls.CurrencyLookup
        Me.lblMessageBottom = New System.Windows.Forms.Label()
        Me.lblDifference = New System.Windows.Forms.Label()
        Me.lblInstalmentsMarked = New System.Windows.Forms.Label()
        Me.lblReceiptBaseCurrency = New System.Windows.Forms.Label()
        Me.lblReceiptAmount = New System.Windows.Forms.Label()
        Me.lblMessageTop = New System.Windows.Forms.Label()
        Me.Image1 = New System.Windows.Forms.PictureBox()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.pnlDiffCurrencyContainer = New System.Windows.Forms.Panel()
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cboWriteOffReason = New PMLookupControl.cboPMLookup
        Me.lblWOffDifference = New System.Windows.Forms.Label
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDiffCurrencyContainer.SuspendLayout()
        Me.SuspendLayout()
        ' 
        'cmdCancel
        ' 
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(368, 215)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(113, 33)
        Me.cmdCancel.TabIndex = 16
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        ' 
        'cmdTakeExact
        ' 
        Me.cmdTakeExact.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTakeExact.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTakeExact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTakeExact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTakeExact.Location = New System.Drawing.Point(248, 215)
        Me.cmdTakeExact.Name = "cmdTakeExact"
        Me.cmdTakeExact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTakeExact.Size = New System.Drawing.Size(113, 33)
        Me.cmdTakeExact.TabIndex = 15
        Me.cmdTakeExact.Text = "Take Exact Amount"
        Me.cmdTakeExact.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTakeExact.UseVisualStyleBackColor = False
        ' 
        'cmdWriteOff
        ' 
        Me.cmdWriteOff.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWriteOff.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWriteOff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWriteOff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWriteOff.Location = New System.Drawing.Point(128, 215)
        Me.cmdWriteOff.Name = "cmdWriteOff"
        Me.cmdWriteOff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWriteOff.Size = New System.Drawing.Size(113, 33)
        Me.cmdWriteOff.TabIndex = 14
        Me.cmdWriteOff.Text = "Write Off Difference"
        Me.cmdWriteOff.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdWriteOff.UseVisualStyleBackColor = False
        ' 
        'txtDifference
        ' 
        Me.txtDifference.AcceptsReturn = True
        Me.txtDifference.BackColor = System.Drawing.SystemColors.Window
        Me.txtDifference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDifference.Enabled = False
        Me.txtDifference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDifference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDifference.Location = New System.Drawing.Point(158, 75)
        Me.txtDifference.MaxLength = 0
        Me.txtDifference.Name = "txtDifference"
        Me.txtDifference.ReadOnly = True
        Me.txtDifference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDifference.Size = New System.Drawing.Size(105, 19)
        Me.txtDifference.TabIndex = 11
        Me.txtDifference.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        ' 
        'txtInstalmentsMarked
        ' 
        Me.txtInstalmentsMarked.AcceptsReturn = True
        Me.txtInstalmentsMarked.BackColor = System.Drawing.SystemColors.Window
        Me.txtInstalmentsMarked.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInstalmentsMarked.Enabled = False
        Me.txtInstalmentsMarked.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInstalmentsMarked.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInstalmentsMarked.Location = New System.Drawing.Point(158, 51)
        Me.txtInstalmentsMarked.MaxLength = 0
        Me.txtInstalmentsMarked.Name = "txtInstalmentsMarked"
        Me.txtInstalmentsMarked.ReadOnly = True
        Me.txtInstalmentsMarked.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInstalmentsMarked.Size = New System.Drawing.Size(105, 19)
        Me.txtInstalmentsMarked.TabIndex = 8
        Me.txtInstalmentsMarked.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        ' 
        'txtReceiptBaseCurrency
        ' 
        Me.txtReceiptBaseCurrency.AcceptsReturn = True
        Me.txtReceiptBaseCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtReceiptBaseCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReceiptBaseCurrency.Enabled = False
        Me.txtReceiptBaseCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptBaseCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReceiptBaseCurrency.Location = New System.Drawing.Point(158, 27)
        Me.txtReceiptBaseCurrency.MaxLength = 0
        Me.txtReceiptBaseCurrency.Name = "txtReceiptBaseCurrency"
        Me.txtReceiptBaseCurrency.ReadOnly = True
        Me.txtReceiptBaseCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReceiptBaseCurrency.Size = New System.Drawing.Size(105, 19)
        Me.txtReceiptBaseCurrency.TabIndex = 5
        Me.txtReceiptBaseCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        ' 
        'txtReceiptAmount
        ' 
        Me.txtReceiptAmount.AcceptsReturn = True
        Me.txtReceiptAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtReceiptAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReceiptAmount.Enabled = False
        Me.txtReceiptAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReceiptAmount.Location = New System.Drawing.Point(158, 3)
        Me.txtReceiptAmount.MaxLength = 0
        Me.txtReceiptAmount.Name = "txtReceiptAmount"
        Me.txtReceiptAmount.ReadOnly = True
        Me.txtReceiptAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReceiptAmount.Size = New System.Drawing.Size(105, 19)
        Me.txtReceiptAmount.TabIndex = 2
        Me.txtReceiptAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        ' 
        'cboReceiptAmount
        ' 
        Me.cboReceiptAmount.CompanyId = 0
        Me.cboReceiptAmount.CurrencyId = 0
        Me.cboReceiptAmount.DefaultCurrencyId = 0
        Me.cboReceiptAmount.Enabled = False
        Me.cboReceiptAmount.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cboReceiptAmount.Location = New System.Drawing.Point(270, 3)
        Me.cboReceiptAmount.Name = "cboReceiptAmount"
        Me.cboReceiptAmount.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboReceiptAmount.Size = New System.Drawing.Size(145, 21)
        Me.cboReceiptAmount.TabIndex = 3
        Me.cboReceiptAmount.ToolTipText = ""
        Me.cboReceiptAmount.WhatsThisHelpID = 0
        ' 
        'cboReceiptBaseCurrency
        ' 
        Me.cboReceiptBaseCurrency.CompanyId = 0
        Me.cboReceiptBaseCurrency.CurrencyId = 0
        Me.cboReceiptBaseCurrency.DefaultCurrencyId = 0
        Me.cboReceiptBaseCurrency.Enabled = False
        Me.cboReceiptBaseCurrency.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cboReceiptBaseCurrency.Location = New System.Drawing.Point(270, 27)
        Me.cboReceiptBaseCurrency.Name = "cboReceiptBaseCurrency"
        Me.cboReceiptBaseCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboReceiptBaseCurrency.Size = New System.Drawing.Size(145, 21)
        Me.cboReceiptBaseCurrency.TabIndex = 6
        Me.cboReceiptBaseCurrency.ToolTipText = ""
        Me.cboReceiptBaseCurrency.WhatsThisHelpID = 0
        ' 
        'cboInstalmentsMarked
        ' 
        Me.cboInstalmentsMarked.CompanyId = 0
        Me.cboInstalmentsMarked.CurrencyId = 0
        Me.cboInstalmentsMarked.DefaultCurrencyId = 0
        Me.cboInstalmentsMarked.Enabled = False
        Me.cboInstalmentsMarked.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cboInstalmentsMarked.Location = New System.Drawing.Point(270, 51)
        Me.cboInstalmentsMarked.Name = "cboInstalmentsMarked"
        Me.cboInstalmentsMarked.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboInstalmentsMarked.Size = New System.Drawing.Size(145, 21)
        Me.cboInstalmentsMarked.TabIndex = 9
        Me.cboInstalmentsMarked.ToolTipText = ""
        Me.cboInstalmentsMarked.WhatsThisHelpID = 0
        ' 
        'cboDifference
        ' 
        Me.cboDifference.CompanyId = 0
        Me.cboDifference.CurrencyId = 0
        Me.cboDifference.DefaultCurrencyId = 0
        Me.cboDifference.Enabled = False
        Me.cboDifference.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cboDifference.Location = New System.Drawing.Point(270, 75)
        Me.cboDifference.Name = "cboDifference"
        Me.cboDifference.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboDifference.Size = New System.Drawing.Size(145, 21)
        Me.cboDifference.TabIndex = 12
        Me.cboDifference.ToolTipText = ""
        Me.cboDifference.WhatsThisHelpID = 0
        ' 
        'lblMessageBottom
        ' 
        Me.lblMessageBottom.BackColor = System.Drawing.SystemColors.Control
        Me.lblMessageBottom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMessageBottom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessageBottom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMessageBottom.Location = New System.Drawing.Point(64, 185)
        Me.lblMessageBottom.Name = "lblMessageBottom"
        Me.lblMessageBottom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMessageBottom.Size = New System.Drawing.Size(417, 25)
        Me.lblMessageBottom.TabIndex = 13
        Me.lblMessageBottom.Text = "You can write-off the difference or collect the exact amount by [partially paying" & _
            " one of the instalments | leaving a credit on the account.]"
        ' 
        'lblDifference
        ' 
        Me.lblDifference.BackColor = System.Drawing.SystemColors.Control
        Me.lblDifference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDifference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDifference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDifference.Location = New System.Drawing.Point(3, 80)
        Me.lblDifference.Name = "lblDifference"
        Me.lblDifference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDifference.Size = New System.Drawing.Size(145, 17)
        Me.lblDifference.TabIndex = 10
        Me.lblDifference.Text = "Difference:"
        ' 
        'lblInstalmentsMarked
        ' 
        Me.lblInstalmentsMarked.BackColor = System.Drawing.SystemColors.Control
        Me.lblInstalmentsMarked.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstalmentsMarked.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstalmentsMarked.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInstalmentsMarked.Location = New System.Drawing.Point(3, 56)
        Me.lblInstalmentsMarked.Name = "lblInstalmentsMarked"
        Me.lblInstalmentsMarked.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstalmentsMarked.Size = New System.Drawing.Size(145, 17)
        Me.lblInstalmentsMarked.TabIndex = 7
        Me.lblInstalmentsMarked.Text = "Instalments Marked:"
        ' 
        'lblReceiptBaseCurrency
        ' 
        Me.lblReceiptBaseCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceiptBaseCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceiptBaseCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceiptBaseCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceiptBaseCurrency.Location = New System.Drawing.Point(3, 32)
        Me.lblReceiptBaseCurrency.Name = "lblReceiptBaseCurrency"
        Me.lblReceiptBaseCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceiptBaseCurrency.Size = New System.Drawing.Size(145, 17)
        Me.lblReceiptBaseCurrency.TabIndex = 4
        Me.lblReceiptBaseCurrency.Text = "Receipt Base Currency:"
        ' 
        'lblReceiptAmount
        ' 
        Me.lblReceiptAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceiptAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceiptAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceiptAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceiptAmount.Location = New System.Drawing.Point(3, 8)
        Me.lblReceiptAmount.Name = "lblReceiptAmount"
        Me.lblReceiptAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceiptAmount.Size = New System.Drawing.Size(145, 17)
        Me.lblReceiptAmount.TabIndex = 1
        Me.lblReceiptAmount.Text = "Receipt Amount:"
        ' 
        'lblMessageTop
        ' 
        Me.lblMessageTop.BackColor = System.Drawing.SystemColors.Control
        Me.lblMessageTop.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMessageTop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessageTop.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMessageTop.Location = New System.Drawing.Point(64, 16)
        Me.lblMessageTop.Name = "lblMessageTop"
        Me.lblMessageTop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMessageTop.Size = New System.Drawing.Size(409, 33)
        Me.lblMessageTop.TabIndex = 0
        Me.lblMessageTop.Text = "Due to currency fluctuations the receipt amount is [less|more] than the instalmen" & _
            "t amount."
        ' 
        'Image1
        ' 
        Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
        Me.Image1.Location = New System.Drawing.Point(16, 16)
        Me.Image1.Name = "Image1"
        Me.Image1.Size = New System.Drawing.Size(32, 32)
        Me.Image1.TabIndex = 17
        Me.Image1.TabStop = False
        '
        'cboWriteOffReason
        '
        Me.cboWriteOffReason.DefaultItemId = 0
        Me.cboWriteOffReason.FirstItem = "(N/A)"
        Me.cboWriteOffReason.ItemId = 0
        Me.cboWriteOffReason.ListIndex = -1
        Me.cboWriteOffReason.Location = New System.Drawing.Point(224, 151)
        Me.cboWriteOffReason.Name = "cboWriteOffReason"
        Me.cboWriteOffReason.PMLookupProductFamily = 1
        Me.cboWriteOffReason.SingleItemId = 0
        Me.cboWriteOffReason.Size = New System.Drawing.Size(205, 21)
        Me.cboWriteOffReason.Sorted = True
        Me.cboWriteOffReason.TabIndex = 18
        Me.cboWriteOffReason.TableName = "Write_Off_Reason"
        Me.cboWriteOffReason.ToolTipText = ""
        Me.cboWriteOffReason.WhereClause = "Is_Only_Valid_For_Instalment=1"
        '
        'lblWOffDifference
        '
        Me.lblWOffDifference.BackColor = System.Drawing.SystemColors.Control
        Me.lblWOffDifference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWOffDifference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWOffDifference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWOffDifference.Location = New System.Drawing.Point(62, 154)
        Me.lblWOffDifference.Name = "lblWOffDifference"
        Me.lblWOffDifference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWOffDifference.Size = New System.Drawing.Size(125, 17)
        Me.lblWOffDifference.TabIndex = 19
        Me.lblWOffDifference.Text = "Write-Off Reason:"
        Me.Image1.TabIndex = 17
        Me.Image1.TabStop = False
        '
        'pnlDiffCurrencyContainer
        '
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.lblReceiptAmount)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.lblReceiptBaseCurrency)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.lblInstalmentsMarked)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.lblDifference)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.txtDifference)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.txtReceiptAmount)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.txtInstalmentsMarked)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.txtReceiptBaseCurrency)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.cboReceiptAmount)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.cboReceiptBaseCurrency)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.cboInstalmentsMarked)
        Me.pnlDiffCurrencyContainer.Controls.Add(Me.cboDifference)
        Me.pnlDiffCurrencyContainer.Location = New System.Drawing.Point(60, 51)
        Me.pnlDiffCurrencyContainer.Name = "pnlDiffCurrencyContainer"
        Me.pnlDiffCurrencyContainer.Size = New System.Drawing.Size(413, 100)
        Me.pnlDiffCurrencyContainer.TabIndex = 18
        ' 
        'frmDifference
        ' 
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(488, 240)
        Me.Controls.Add(Me.pnlDiffCurrencyContainer)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdTakeExact)
        Me.Controls.Add(Me.cmdWriteOff)
        Me.Controls.Add(Me.lblMessageBottom)
        Me.Controls.Add(Me.lblMessageTop)
        Me.Controls.Add(Me.Image1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDifference"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Currency Difference"
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDiffCurrencyContainer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlDiffCurrencyContainer As System.Windows.Forms.Panel
    Public WithEvents cboWriteOffReason As PMLookupControl.cboPMLookup
    Public WithEvents lblWOffDifference As System.Windows.Forms.Label
#End Region
End Class