Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
Imports Artinsoft.VB6.Utils

Module MainModule

    Private Const ACClass As String = "MainModule"
    Public Const ACApp As String = "KCMBackboneGeneration"

    ' constants for user name and password - apparently, these should be available on all systems...
    Public Const m_sUserName As String = "sirius"
    Public Const m_sPassword As String = ""

    ' Form
    Public Const ACInterfaceTitle As Integer = 100

    ' Buttons
    Public Const ACGenerateButton As Integer = 200
    Public Const ACCancelButton As Integer = 201

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    ' Public source and language ID's from the
    <ThreadStatic()>
    Public g_iSourceID As Integer
    <ThreadStatic()>
    Public g_iLanguageID As Integer

    <ThreadStatic()>
    Public g_vGridData As Object
    Public gridData As New XArrayHelper

    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager
    <ThreadStatic()> _
   Public g_ToMessage As Boolean

    Sub Main()
        Dim obj As New Interface_Renamed
        obj.Initialise()
        obj.Start()
    End Sub
    
End Module