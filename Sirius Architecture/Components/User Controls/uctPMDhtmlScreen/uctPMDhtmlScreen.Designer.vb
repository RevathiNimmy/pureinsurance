<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPMDhtmlScreen
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Friend WithEvents wbMain As System.Windows.Forms.WebBrowser
	Friend WithEvents Timer1 As System.Windows.Forms.Timer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPMDhtmlScreen))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.wbMain = New System.Windows.Forms.WebBrowser
		Me.Timer1 = New System.Windows.Forms.Timer(components)
		Me.SuspendLayout()
		' 
		' wbMain
		' 
		Me.wbMain.AllowWebBrowserDrop = True
		Me.wbMain.Location = New System.Drawing.Point(8, 8)
		Me.wbMain.Name = "wbMain"
		Me.wbMain.Size = New System.Drawing.Size(553, 345)
		Me.wbMain.TabIndex = 0
		' 
		' Timer1
		' 
		Me.Timer1.Enabled = False
		Me.Timer1.Interval = 1000
		' 
		' uctPMDhtmlScreen
		' 
		Me.ClientSize = New System.Drawing.Size(566, 365)
		Me.Controls.Add(Me.wbMain)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctPMDhtmlScreen"
		Me.ResumeLayout(False)
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("onlostfocusEventArgs_NET.onlostfocusEventArgs")> _
	Public NotInheritable Class onlostfocusEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public Sub New(ByVal sID As String)
			MyBase.New()
			Me.sID = sID
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("ongotfocusEventArgs_NET.ongotfocusEventArgs")> _
	Public NotInheritable Class ongotfocusEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public Sub New(ByVal sID As String)
			MyBase.New()
			Me.sID = sID
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onmouseupEventArgs_NET.onmouseupEventArgs")> _
	Public NotInheritable Class onmouseupEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public lButton As Integer
		Public Sub New(ByVal sID As String, ByVal lButton As Integer)
			MyBase.New()
			Me.sID = sID
			Me.lButton = lButton
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onmouseoverEventArgs_NET.onmouseoverEventArgs")> _
	Public NotInheritable Class onmouseoverEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public Sub New(ByVal sID As String)
			MyBase.New()
			Me.sID = sID
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onmouseoutEventArgs_NET.onmouseoutEventArgs")> _
	Public NotInheritable Class onmouseoutEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public Sub New(ByVal sID As String)
			MyBase.New()
			Me.sID = sID
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onmousemoveEventArgs_NET.onmousemoveEventArgs")> _
	Public NotInheritable Class onmousemoveEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public Sub New(ByVal sID As String)
			MyBase.New()
			Me.sID = sID
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onmousedownEventArgs_NET.onmousedownEventArgs")> _
	Public NotInheritable Class onmousedownEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public lButton As Integer
		Public Sub New(ByVal sID As String, ByVal lButton As Integer)
			MyBase.New()
			Me.sID = sID
			Me.lButton = lButton
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onkeypressEventArgs_NET.onkeypressEventArgs")> _
	Public NotInheritable Class onkeypressEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public iKeyCode As Integer
		Public bExecute As Boolean
		Public Sub New(ByVal sID As String, ByVal iKeyCode As Integer, ByRef bExecute As Boolean)
			MyBase.New()
			Me.sID = sID
			Me.iKeyCode = iKeyCode
			Me.bExecute = bExecute
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onhelpEventArgs_NET.onhelpEventArgs")> _
	Public NotInheritable Class onhelpEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public bExecute As Boolean
		Public Sub New(ByVal sID As String, ByRef bExecute As Boolean)
			MyBase.New()
			Me.sID = sID
			Me.bExecute = bExecute
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onerrorupdateEventArgs_NET.onerrorupdateEventArgs")> _
	Public NotInheritable Class onerrorupdateEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public bExecute As Boolean
		Public Sub New(ByVal sID As String, ByRef bExecute As Boolean)
			MyBase.New()
			Me.sID = sID
			Me.bExecute = bExecute
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("ondragstartEventArgs_NET.ondragstartEventArgs")> _
	Public NotInheritable Class ondragstartEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public bExecute As Boolean
		Public Sub New(ByVal sID As String, ByRef bExecute As Boolean)
			MyBase.New()
			Me.sID = sID
			Me.bExecute = bExecute
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("ondblclickEventArgs_NET.ondblclickEventArgs")> _
	Public NotInheritable Class ondblclickEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public bExecute As Boolean
		Public Sub New(ByVal sID As String, ByRef bExecute As Boolean)
			MyBase.New()
			Me.sID = sID
			Me.bExecute = bExecute
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onclickEventArgs_NET.onclickEventArgs")> _
	Public NotInheritable Class onclickEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public bExecute As Boolean
		Public Sub New(ByVal sID As String, ByRef bExecute As Boolean)
			MyBase.New()
			Me.sID = sID
			Me.bExecute = bExecute
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onbeforeupdateEventArgs_NET.onbeforeupdateEventArgs")> _
	Public NotInheritable Class onbeforeupdateEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public bExecute As Boolean
		Public Sub New(ByVal sID As String, ByRef bExecute As Boolean)
			MyBase.New()
			Me.sID = sID
			Me.bExecute = bExecute
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("onafterupdateEventArgs_NET.onafterupdateEventArgs")> _
	Public NotInheritable Class onafterupdateEventArgs
		Inherits System.EventArgs
		Public sID As String = ""
		Public Sub New(ByVal sID As String)
			MyBase.New()
			Me.sID = sID
		End Sub
	End Class
#End Region 
End Class