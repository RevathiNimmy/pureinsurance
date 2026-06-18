<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializetxtExtra()
        InitializelblExtra()
        InitializechkExtra()
        InitializecboLookupExtra()
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
    Public WithEvents cmdLinkObject As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtCode As System.Windows.Forms.TextBox
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
    Private WithEvents _chkExtra_0 As System.Windows.Forms.CheckBox
    Private WithEvents _cboLookupExtra_0 As PMLookupControl.cboPMLookup
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Private WithEvents _lblExtra_0 As System.Windows.Forms.Label
    Public WithEvents picCanvas As System.Windows.Forms.PictureBox
    Public WithEvents picContainer As System.Windows.Forms.PictureBox
    Public WithEvents VScroll1 As System.Windows.Forms.VScrollBar
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public cboLookupExtra(0) As PMLookupControl.cboPMLookup
    ''''
    Public cboLookupTemp As PMLookupControl.cboPMLookup
    Public chkExtra(0) As System.Windows.Forms.CheckBox
    Public lblExtra(0) As System.Windows.Forms.Label
    Public txtExtra(0) As System.Windows.Forms.TextBox
    Public btnExtra(0) As System.Windows.Forms.Button
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdLinkObject = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.picContainer = New System.Windows.Forms.PictureBox()
        Me.picCanvas = New System.Windows.Forms.PictureBox()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me._txtExtra_0 = New System.Windows.Forms.TextBox()
        Me._chkExtra_0 = New System.Windows.Forms.CheckBox()
        Me._cboLookupExtra_0 = New PMLookupControl.cboPMLookup()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me._lblExtra_0 = New System.Windows.Forms.Label()
        Me.VScroll1 = New System.Windows.Forms.VScrollBar()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.UctCompiledRule1 = New uctCompiledRule.uctCompiledRule()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.picContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.picContainer.SuspendLayout()
        CType(Me.picCanvas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.picCanvas.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdLinkObject
        '
        Me.cmdLinkObject.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLinkObject.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLinkObject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLinkObject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLinkObject.Location = New System.Drawing.Point(8, 199)
        Me.cmdLinkObject.Name = "cmdLinkObject"
        Me.cmdLinkObject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLinkObject.Size = New System.Drawing.Size(81, 22)
        Me.cmdLinkObject.TabIndex = 3
        Me.cmdLinkObject.Text = "Link Object"
        Me.cmdLinkObject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLinkObject.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(381, 199)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
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
        Me.cmdCancel.Location = New System.Drawing.Point(460, 199)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(520, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(525, 188)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabMainTab.TabIndex = 6
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.AutoScroll = True
        Me._tabMainTab_TabPage0.Controls.Add(Me.picContainer)
        Me._tabMainTab_TabPage0.Controls.Add(Me.VScroll1)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(517, 162)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Details"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'picContainer
        '
        Me.picContainer.BackColor = System.Drawing.SystemColors.Control
        Me.picContainer.Controls.Add(Me.picCanvas)
        Me.picContainer.Cursor = System.Windows.Forms.Cursors.Default
        Me.picContainer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picContainer.Location = New System.Drawing.Point(5, 8)
        Me.picContainer.Name = "picContainer"
        Me.picContainer.Size = New System.Drawing.Size(491, 149)
        Me.picContainer.TabIndex = 7
        Me.picContainer.TabStop = False
        '
        'picCanvas
        '
        Me.picCanvas.BackColor = System.Drawing.SystemColors.Control
        Me.picCanvas.Controls.Add(Me.txtCode)
        Me.picCanvas.Controls.Add(Me.txtDescription)
        Me.picCanvas.Controls.Add(Me.txtEffectiveDate)
        Me.picCanvas.Controls.Add(Me._txtExtra_0)
        Me.picCanvas.Controls.Add(Me._chkExtra_0)
        Me.picCanvas.Controls.Add(Me._cboLookupExtra_0)
        Me.picCanvas.Controls.Add(Me.lblCode)
        Me.picCanvas.Controls.Add(Me.lblDescription)
        Me.picCanvas.Controls.Add(Me.lblEffectiveDate)
        Me.picCanvas.Controls.Add(Me._lblExtra_0)
        Me.picCanvas.Controls.Add(Me.UctCompiledRule1)
        Me.picCanvas.Cursor = System.Windows.Forms.Cursors.Default
        Me.picCanvas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picCanvas.Location = New System.Drawing.Point(0, 0)
        Me.picCanvas.Name = "picCanvas"
        Me.picCanvas.Size = New System.Drawing.Size(491, 149)
        Me.picCanvas.TabIndex = 8
        Me.picCanvas.TabStop = False
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(216, 0)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(145, 20)
        Me.txtCode.TabIndex = 0
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(216, 22)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(265, 20)
        Me.txtDescription.TabIndex = 1
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(216, 44)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(265, 20)
        Me.txtEffectiveDate.TabIndex = 2
        '
        '_txtExtra_0
        '
        Me._txtExtra_0.AcceptsReturn = True
        Me._txtExtra_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtExtra_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtExtra_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtExtra_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtExtra_0.Location = New System.Drawing.Point(216, 66)
        Me._txtExtra_0.MaxLength = 255
        Me._txtExtra_0.Name = "_txtExtra_0"
        Me._txtExtra_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtExtra_0.Size = New System.Drawing.Size(89, 20)
        Me._txtExtra_0.TabIndex = 11
        Me._txtExtra_0.Visible = False
        '
        '_chkExtra_0
        '
        Me._chkExtra_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkExtra_0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._chkExtra_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkExtra_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkExtra_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkExtra_0.Location = New System.Drawing.Point(3, 112)
        Me._chkExtra_0.Name = "_chkExtra_0"
        Me._chkExtra_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkExtra_0.Size = New System.Drawing.Size(226, 17)
        Me._chkExtra_0.TabIndex = 9
        Me._chkExtra_0.UseVisualStyleBackColor = False
        Me._chkExtra_0.Visible = False
        '
        '_cboLookupExtra_0
        '
        Me._cboLookupExtra_0.DefaultItemId = 0
        Me._cboLookupExtra_0.FirstItem = "(null)"
        Me._cboLookupExtra_0.ItemId = 0
        Me._cboLookupExtra_0.ListIndex = -1
        Me._cboLookupExtra_0.Location = New System.Drawing.Point(216, 88)
        Me._cboLookupExtra_0.Name = "_cboLookupExtra_0"
        Me._cboLookupExtra_0.PMLookupProductFamily = 1
        Me._cboLookupExtra_0.SingleItemId = 0
        Me._cboLookupExtra_0.Size = New System.Drawing.Size(81, 21)
        Me._cboLookupExtra_0.SortColumnName = ""
        Me._cboLookupExtra_0.Sorted = True
        Me._cboLookupExtra_0.TabIndex = 10
        Me._cboLookupExtra_0.TableName = "Null"
        Me._cboLookupExtra_0.ToolTipText = ""
        Me._cboLookupExtra_0.Visible = False
        Me._cboLookupExtra_0.WhereClause = ""
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(5, 3)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(35, 13)
        Me.lblCode.TabIndex = 15
        Me.lblCode.Text = "Code:"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(5, 25)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 14
        Me.lblDescription.Text = "Description:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(5, 47)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(78, 13)
        Me.lblEffectiveDate.TabIndex = 13
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        '_lblExtra_0
        '
        Me._lblExtra_0.AutoSize = True
        Me._lblExtra_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblExtra_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblExtra_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblExtra_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblExtra_0.Location = New System.Drawing.Point(5, 69)
        Me._lblExtra_0.Name = "_lblExtra_0"
        Me._lblExtra_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblExtra_0.Size = New System.Drawing.Size(37, 13)
        Me._lblExtra_0.TabIndex = 12
        Me._lblExtra_0.Text = "Extra :"
        Me._lblExtra_0.Visible = False
        '
        'VScroll1
        '
        Me.VScroll1.Cursor = System.Windows.Forms.Cursors.Default
        Me.VScroll1.LargeChange = 1
        Me.VScroll1.Location = New System.Drawing.Point(502, 8)
        Me.VScroll1.Maximum = 32767
        Me.VScroll1.Name = "VScroll1"
        Me.VScroll1.Size = New System.Drawing.Size(15, 149)
        Me.VScroll1.TabIndex = 16
        Me.VScroll1.Visible = False
        '
        'UctCompiledRule1
        '
        Me.UctCompiledRule1.bEnterOnlyAssemblyName = False
        Me.UctCompiledRule1.Location = New System.Drawing.Point(221, 118)
        Me.UctCompiledRule1.Name = "UctCompiledRule1"
        Me.UctCompiledRule1.Size = New System.Drawing.Size(237, 20)
        Me.UctCompiledRule1.TabIndex = 17
        '
        'frmDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(538, 225)
        Me.Controls.Add(Me.cmdLinkObject)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Details"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.picContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.picContainer.ResumeLayout(False)
        CType(Me.picCanvas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.picCanvas.ResumeLayout(False)
        Me.picCanvas.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializetxtExtra()
        Me.txtExtra(0) = _txtExtra_0
    End Sub
    Sub InitializelblExtra()
        Me.lblExtra(0) = _lblExtra_0
    End Sub
    Sub InitializechkExtra()
        Me.chkExtra(0) = _chkExtra_0
    End Sub
    Sub InitializecboLookupExtra()
        Me.cboLookupExtra(0) = _cboLookupExtra_0
    End Sub
    Public WithEvents _txtExtra_0 As System.Windows.Forms.TextBox
    Friend WithEvents UctCompiledRule1 As uctCompiledRule.uctCompiledRule
#End Region
End Class
