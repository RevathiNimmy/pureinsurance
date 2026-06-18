Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Module MainModule
	
	Public Const ACApp As String = "iPMBCMManager"
	
	Private Const ACClass As String = "MainModule"
	
    ' Array containing client manager infos
    '<ThreadStatic()> _
    Public m_vCMArray As Object
	
    ' Positions in array
    Public Const ACArrayObject As Integer = 0
	Public Const ACArrayPartyCnt As Integer = 1
	Public Const ACArrayStatus As Integer = 2
	
	' Status of client managers
	Public Const ACStatusLive As Integer = 1
	Public Const ACStatusDead As Integer = 2
	Public Const ACStatusEmpty As Integer = 3
	
	' Registry setting
	Public Const ACRegSettingMaximumCM As String = "MaxCM"
	
    ' Object Manager
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module