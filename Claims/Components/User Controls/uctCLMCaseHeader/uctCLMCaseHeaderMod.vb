Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "uctCLMCaseHeader"
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'Label
	Public Const kRegKeyConstCaseNumber As Integer = 100
	Public Const kRegKeyConstCaseOpenDate As Integer = 101
	Public Const kRegKeyConstCaseProgressStatus As Integer = 102
	Public Const kRegKeyConstAnalyst As Integer = 103
	Public Const kRegKeyConstAssistant As Integer = 104
	Public Const kRegKeyConstCaseVersion As Integer = 105
	
	'Error Message
	Public Const kRegKeyConstErrCaseOpendate As Integer = 200
	Public Const kRegKeyConstErrCaseProgressStatus As Integer = 201
	Public Const kRegKeyConstErrCaseAssistant As Integer = 202
	Public Const kRegKeyConstErrCaseAnalyst As Integer = 203
End Module