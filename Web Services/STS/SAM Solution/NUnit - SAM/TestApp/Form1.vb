Imports SiriusFS.SAM.Nunit

Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents Button14 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button7 = New System.Windows.Forms.Button
        Me.Button8 = New System.Windows.Forms.Button
        Me.Button9 = New System.Windows.Forms.Button
        Me.Button10 = New System.Windows.Forms.Button
        Me.Button12 = New System.Windows.Forms.Button
        Me.Button13 = New System.Windows.Forms.Button
        Me.Button14 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(0, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(176, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "GetList"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(0, 24)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(176, 23)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "AddAnonCustomer"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(0, 48)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(176, 23)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "AddQuote"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(0, 77)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(176, 23)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "UpdateRisk"
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(0, 121)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(176, 23)
        Me.Button7.TabIndex = 6
        Me.Button7.Text = "AddAddress"
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(0, 145)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(176, 23)
        Me.Button8.TabIndex = 7
        Me.Button8.Text = "GetAddress"
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(0, 169)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(176, 23)
        Me.Button9.TabIndex = 8
        Me.Button9.Text = "GetQuoteAndSummariesByKey"
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(0, 193)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(176, 23)
        Me.Button10.TabIndex = 9
        Me.Button10.Text = "GetQuoteAndSummariesByRef"
        '
        'Button12
        '
        Me.Button12.Location = New System.Drawing.Point(0, 97)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(176, 23)
        Me.Button12.TabIndex = 11
        Me.Button12.Text = "GetRisk"
        '
        'Button13
        '
        Me.Button13.Location = New System.Drawing.Point(0, 217)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(176, 23)
        Me.Button13.TabIndex = 12
        Me.Button13.Text = "RunDefaultRulesEdit"
        '
        'Button14
        '
        Me.Button14.Location = New System.Drawing.Point(184, 0)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(200, 23)
        Me.Button14.TabIndex = 13
        Me.Button14.Text = "Button14"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(392, 317)
        Me.Controls.Add(Me.Button14)
        Me.Controls.Add(Me.Button13)
        Me.Controls.Add(Me.Button12)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form1"
        Me.Text = "Re"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim obj As New Anonymous.GetList

        obj.Success_ListTypeUD()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim obj As New Anonymous.AddAnonCustomer

        obj.Success_GenderMale()

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim obj As New Anonymous.AddQuote

        obj.Success()

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim obj As New Anonymous.UpdateRisk

        obj.Success()

    End Sub
    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Dim obj As New Anonymous.GetQuoteAndSummariesByKey

        obj.Success()

    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Dim obj As New Anonymous.GetQuoteAndSummariesByRef

        obj.Success()

    End Sub


    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        Dim obj As New Anonymous.RunDefaultRulesEdit

        obj.Success()

    End Sub
End Class
