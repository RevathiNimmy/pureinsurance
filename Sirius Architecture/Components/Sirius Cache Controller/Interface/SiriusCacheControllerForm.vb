Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    ' Constants form gPMConstants
    Private Const PMFalse As Integer = 0
    Private Const PMTrue As Integer = 1
    Private Const PMFail As Integer = 10
    Private Const PMError As Integer = 11
    Private Const PMSucceed As Integer = 12
    Private Const PMOk As Integer = 20
    Private Const PMCancel As Integer = 21
    Private Const PMNotFound As Integer = 811

    Private Const ACApp As String = "SiriusCacheController"
    Private Const PMGetViaClientManager As String = "CLIENTMANAGER"

    Dim m_oBusiness As Object
    Dim m_oObjectManager As bObjectManager.ObjectManager
    Dim m_vCacheKeyArray As Object
    Dim m_lReturn As Integer

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Create the Object Manager
        m_oObjectManager = New bObjectManager.ObjectManager()

        ' Inititalise Object Manager
        m_lReturn = CInt(m_oObjectManager.Initialise("SiriusCacheController"))
        If m_lReturn <> PMTrue Then
            ' Log Message
            LogMessage("Form_Load", "Failed to Inititalise Object Manager")
            ' Quit Program
            Environment.Exit(0)
        End If

        'Check if we can continue
        m_lReturn = GetSysAdminStatus()
        If m_lReturn <> PMTrue Then
            ' Log Message
            'LogMessage "Form_Load", "Failed to Check System Administrator Status"
            ' Quit Program
            Environment.Exit(0)
        End If

        ' Load the cache keys, by default
        m_lReturn = LoadCacheKeys()
        If m_lReturn <> PMTrue Then
            ' Log Message
            LogMessage("Form_Load", "Failed to Load Cache Keys")
            ' Quit Program
            Exit Sub
        End If

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        Dispose()
        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.
        Try
            Me.Close()
        Catch
            Exit Sub
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Try
            Me.Close()
        Catch
        End Try

        Exit Sub

    End Sub

    Private Function LogMessage(ByVal v_sMethod As String, ByVal v_sMessage As String) As Integer

        Dim sMessage As String = "Method       : " & v_sMethod & Strings.Chr(13) & Strings.Chr(10) & _
                                 "Error Detail : " & v_sMessage

        If sMessage.Trim().Length > 0 Then
            MessageBox.Show(sMessage, "Sirius Cachce Controller - Interface", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If Not (m_oObjectManager Is Nothing) Then
                    m_oObjectManager.Dispose()
                    m_oObjectManager = Nothing
                End If
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click

        m_lReturn = LoadCacheKeys()
        If m_lReturn <> PMTrue Then
            LogMessage("cmdRefresh_Click", "Failed to LoadCacheKeys into Sirius Cache Controller")
            Exit Sub
        End If

        If lvwCache.Items.Count = 0 Then
            MsgBox("No Items in cache", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Sirius Cache Controller")
        End If

    End Sub

    Private Sub cmdClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClear.Click

        ' Check if we have a selected item
        Dim oItem As ListViewItem = lvwCache.FocusedItem

        If oItem Is Nothing Then
            LogMessage("cmdClear_Click", "No Cache Key is Selected.")
            Exit Sub
        End If

        ' get the key
        Dim sKey As String = oItem.Name

        If sKey.Trim().Length = 0 Then
            LogMessage("cmdClear_Click", "Invalid Cache Key.")
            Exit Sub
        End If

        m_lReturn = ClearCache(v_sKey:=sKey)
        If m_lReturn <> PMTrue Then
            LogMessage("cmdClear_Click", "Failed to Clear Cache. Key [" & sKey & "]")
            Exit Sub
        End If

        ' Refresh the list
        m_lReturn = LoadCacheKeys()
        If m_lReturn <> PMTrue Then
            ' Log Message
            LogMessage("cmdClear_Click", "Failed to Refresh Cache Keys")
            ' Quit Program
            Exit Sub
        End If

    End Sub

    Private Sub cmdClearAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClearAll.Click

        ' Confirm before clearing it
        Dim lMessage As DialogResult = MessageBox.Show("Are you sure you want to Clear all the Cache?", "Sirius Cache Controller", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
        If lMessage = System.Windows.Forms.DialogResult.Yes Then

            m_lReturn = ClearCache()
            If m_lReturn <> PMTrue Then
                LogMessage("cmdClearAll_Click", "Failed to Clear Cache")
                Exit Sub
            End If

            ' Refresh the list
            m_lReturn = LoadCacheKeys()
            If m_lReturn <> PMTrue Then
                ' Log Message
                LogMessage("cmdClear_Click", "Failed to Refresh Cache Keys")
                ' Quit Program
                Exit Sub
            End If
        End If
    End Sub

    Private Function GetCacheKeyArray() As Integer

        Dim result As Integer = 0
        Try

            Dim vKeyArray() As Object

            result = PMTrue

            If m_oBusiness Is Nothing Then
                Dim temp_m_oBusiness As Object

                m_lReturn = CInt(m_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRCacheController.Business", PMGetViaClientManager))
                m_oBusiness = temp_m_oBusiness
                If m_lReturn <> PMTrue Then
                    result = PMFalse
                    LogMessage("GetCacheKeyArray", "Failed to Create bSIRCacheController.Business")
                    Return result
                End If
            End If

            m_lReturn = m_oBusiness.GetCacheKeyArray(vKeyArray)

            If m_lReturn <> PMTrue Then
                result = m_lReturn
                If m_lReturn <> PMNotFound Then
                    LogMessage("GetCacheKeyArray", "Failed to get Cache Key Array from Business Object")
                End If
                Return result
            End If

            If Information.IsArray(vKeyArray) Then
                ' Set the module level variable
                m_vCacheKeyArray = vKeyArray
            End If

            Return result

        Catch excep As System.Exception

            result = PMError
            LogMessage("GetCacheKeyArray", "Error occured in GetCacheKeyArray" & Strings.Chr(13) & Strings.Chr(10) & excep.Message)
            Return result

        End Try
    End Function

    Private Function LoadCacheKeys() As Integer

        Dim result As Integer = 0
        Dim iRowCount As Integer
        Dim sKey, sText As String

        Try

            result = PMTrue

            'Clear the list view
            lvwCache.Items.Clear()

            m_vCacheKeyArray = Nothing

            ' Get the cache key array
            m_lReturn = GetCacheKeyArray()
            If m_lReturn <> PMTrue Then
                ' Error Message
                LogMessage("LoadCacheKeys", "Failed to Get Cache Key Array")
                Return result
            End If

            If Information.IsArray(m_vCacheKeyArray) Then

                ' Zero base

                iRowCount = m_vCacheKeyArray.GetUpperBound(0)

                For iCounter As Integer = 0 To iRowCount

                    With lvwCache

                        sKey = CStr(m_vCacheKeyArray(iCounter))
                        sText = sKey
                        If sKey.Trim().Length > 0 Then

                            .Items.Insert(iCounter, sKey, sText, 0)
                        End If
                    End With

                Next iCounter
            End If

            If lvwCache.Items.Count > 0 Then
                ' Enable the command buttons
                cmdClear.Enabled = True
                cmdClearAll.Enabled = True
            Else
                ' Disable the command buttons
                cmdClear.Enabled = False
                cmdClearAll.Enabled = False
            End If

            Return result

        Catch excep As System.Exception

            result = PMError
            LogMessage("LoadCacheKeys", "Error occured in LoadCacheKeys" & Strings.Chr(13) & Strings.Chr(10) & excep.Message)
            Return result

        End Try
    End Function

    Private Function ClearCache(Optional ByVal v_sKey As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = PMTrue

            If m_oBusiness Is Nothing Then
                Dim temp_m_oBusiness As Object

                m_lReturn = CInt(m_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRCacheController.Business", PMGetViaClientManager))
                m_oBusiness = temp_m_oBusiness
                If m_lReturn <> PMTrue Then
                    result = PMFalse
                    LogMessage("ClearCache", "Failed to Create bSIRCacheController.Business")
                    Return result
                End If
            End If

            If Information.IsNothing(v_sKey) Then
                ' Clear all the cache
                m_lReturn = m_oBusiness.ClearCache()
            Else
                ' If a key is passed in just, the we need to clear that key
                m_lReturn = m_oBusiness.ClearCache(v_sKey:=v_sKey)
            End If

            If m_lReturn <> PMTrue Then
                result = PMFalse
                LogMessage("ClearCache", "Failed to Clear Cache.")
                Return result
            End If

            ' if nothing is passed, clear all the cache
            Return result

        Catch excep As System.Exception

            result = PMError
            LogMessage("ClearCache", "Error occured While Clearing Cache" & Strings.Chr(13) & Strings.Chr(10) & excep.Message)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 16102002 created
    ' ***************************************************************** '
    Private Function GetSysAdminStatus() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer
        Dim oUser As Object

        Try

            result = PMFalse

            ' Get an instance of the bPMUser.Business object via
            ' the public object manager.
            Dim temp_oUser As Object

            m_lReturn = CInt(m_oObjectManager.GetInstance(temp_oUser, "bPMUser.Business", vInstanceManager:="ClientManager"))
            oUser = temp_oUser

            m_lReturn = oUser.GetSysAdminStatus(lStatus)

            ' Check if we have an instance of the Object Manager.
            If Not (oUser Is Nothing) Then
                ' Terminate the business object

                oUser.Dispose()
                ' Destroy the instance of the business object from memory.
                oUser = Nothing
            End If

            If m_lReturn <> PMTrue Or lStatus = 0 Then

                MessageBox.Show("You do not have permission to access " & _
                                "User Maintenance." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Please contact your System Administrator.", Application.ProductName)
            End If

            Return PMTrue

        Catch excep As System.Exception

            result = PMError

            ' Log Error.
            LogMessage("GetSysAdminStatus", "Error occured in GetSysAdminStatus" & Strings.Chr(13) & Strings.Chr(10) & excep.Message)

            ' Check if we have an instance of the Object Manager.
            If Not (oUser Is Nothing) Then
                ' Terminate the business object

                oUser.Dispose()
                ' Destroy the instance of the business object from memory.
                oUser = Nothing
            End If

            Return result

        End Try
    End Function
End Class