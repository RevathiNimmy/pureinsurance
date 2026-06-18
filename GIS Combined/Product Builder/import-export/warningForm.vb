Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms

Friend Partial Class warningForm
	Inherits System.Windows.Forms.Form
	Private bOkay As Boolean
	Private Sub Check1_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Check1.CheckStateChanged
		Command1.Enabled = Check1.CheckState
	End Sub
	
	Private Sub Command1_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command1.Click
		bOkay = True
		Me.Close()
	End Sub
	
	Private Sub Command2_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command2.Click
		Environment.Exit(0)
	End Sub
	
	Private Sub Command3_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command3.Click
		Check1.Focus()
        RichTextBox1.Text = RichTextBox1.Rtf
	End Sub
	
	Private Sub warningForm_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			Me.Check1.Focus()
		End If
	End Sub

    Private Sub warningForm_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Me.Text = ACApp

        RichTextBox1.SelectedRtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}{\f1\froman\fprq2\fcharset2 Symbol;}{\f2\fnil\fcharset0 MS Sans Serif;}}" & _
                            "{\stylesheet{ Normal;}{\s1 heading 1;}{\s2 heading 2;}{\s3 heading 3;}{\s4 heading 4;}}" & _
                            "\viewkind4\uc1\pard\keepn\s1\sb240\sa60\lang2057\kerning32\b\f0\fs32 Warning" & _
                            "\par \pard\kerning0\b0\fs20 Before using the Product Builder Product Import/Export Utility you must read and understand the guidelines for its use." & _
                            "\par In particular you must understand and accept the restrictions of its use. \par These are fully defined in the user documentation and are included here in part." & _
                            "\par \pard\keepn\s4\fi-720\li720\sb240\sa60\qj\tx720\b\fs22 Import Target Condition" & _
                            "\par \pard\b0\fs20 This utility will only successfully import into an environment that is either" & _
                            "\par \pard\fi-360\li720\qj\tx720\f1\'b7\tab\f0 A blank system, with no products" & _
                            "\par \pard\fi-360\li720\qj\f1\'b7\tab\f0 A system that has only ever been updated by using this utility." & _
                            "\par \pard" & _
                            "\par Any existing products not initially created from an import or extra product development performed on the target server is likely to be lost and made unusable by further updates and may prevent further updates from being successful." & _
                            "\par \pard\keepn\s4\fi-720\li720\sb240\sa60\qj\tx720\b\fs22 Single Source of Export" & _
                            "\par \pard\b0\fs20 You must use only one system as the source system but can import into multiple targets." & _
                            "\par Writing two products on two different development environments with a view to merging them to a single server will not work with this utility." & _
                            "\par \fs24" & _
                            "\par \lang1033\f2\fs17" & _
                            "\par }"

    End Sub
	
	Private Sub warningForm_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		If Not bOkay Then Environment.Exit(0)
	End Sub
End Class