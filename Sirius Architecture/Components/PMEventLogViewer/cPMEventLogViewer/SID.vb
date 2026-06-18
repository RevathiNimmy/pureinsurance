Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Imports System.Text
Module modSID
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '


    Private Const SID_REVISION As Integer = 1 ' Current revision level
    Private Const SID_MAX_SUB_AUTHORITIES As Integer = 15

    Private Structure SID_IDENTIFIER_AUTHORITY
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=5)> _
        Dim Value() As Byte
        Private Shared Sub InitStruct(ByRef result As SID_IDENTIFIER_AUTHORITY, ByVal init As Boolean)
            ReDim result.Value(5)
        End Sub
        Public Shared Function CreateInstance() As SID_IDENTIFIER_AUTHORITY
            Dim result As New SID_IDENTIFIER_AUTHORITY
            InitStruct(result, True)
            Return result
        End Function
        Public Function Clone() As SID_IDENTIFIER_AUTHORITY
            Dim result As SID_IDENTIFIER_AUTHORITY = Me
            InitStruct(result, False)
            Array.Copy(Me.Value, result.Value, 6)
            Return result
        End Function
    End Structure

    Private Declare Function IsValidSid Lib "advapi32" (ByVal pSid As Integer) As Integer

    Private Declare Function GetSidIdentifierAuthority Lib "advapi32" (ByVal pSid As Integer) As Integer

    Private Declare Function GetSidSubAuthorityCount Lib "advapi32" (ByVal pSid As Integer) As Integer

    Private Declare Function GetSidSubAuthority Lib "advapi32" (ByVal pSid As Integer, ByVal nSubAuthority As Integer) As Integer

    Private Declare Function LookupAccountSid Lib "advapi32.dll" Alias "LookupAccountSidA" (ByVal lpSystemName As String, ByVal Sid As Integer, ByVal Name As String, ByRef cbName As Integer, ByVal ReferencedDomainName As String, ByRef cbReferencedDomainName As Integer, ByRef peUse As Integer) As Integer

    Private Declare Function LookupAccountName Lib "advapi32.dll" Alias "LookupAccountNameA" (ByVal lpSystemName As String, ByVal lpAccountName As String, ByVal Sid As Integer, ByRef cbSID As Integer, ByVal ReferencedDomainName As Integer, ByRef cbReferencedDomainName As Integer, ByRef peUse As Short) As Integer

    Public Function FormatHex(ByVal xi_lngValue As Integer, ByVal xi_lngNumChars As Integer) As String
        Return New String("0", xi_lngNumChars - xi_lngValue.ToString("X").Length) & xi_lngValue.ToString("X")
    End Function

    Public Function GetTextualSid(ByVal xi_lngPtrSID As Integer) As String


        Dim p_typSecurityID As SID_IDENTIFIER_AUTHORITY = SID_IDENTIFIER_AUTHORITY.CreateInstance()
        Dim p_lngRtn, p_lngNumSubAuthorities, p_lngPtrSubAuthorities As Integer
        Dim p_dblBigNum As Double
        Dim p_strSID As New StringBuilder

        Const OFFSET_4 As Double = 4294967296.0#

        ' test if Sid passed in is valid
        Dim handle10 As GCHandle = GCHandle.Alloc(xi_lngPtrSID, GCHandleType.Pinned)
        Try
            Dim tmpPtr10 As IntPtr = handle10.AddrOfPinnedObject()
            If IsValidSid(tmpPtr10) Then

                ' Get the SID's identifier authority value
                ' (if the function fails, the return value is undefined)
                Dim handle As GCHandle = GCHandle.Alloc(xi_lngPtrSID, GCHandleType.Pinned)
                Try
                    Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
                    p_lngRtn = GetSidIdentifierAuthority(tmpPtr)
                Finally
                    handle.Free()
                End Try

                If p_lngRtn And (Information.Err().LastDllError = 0) Then
                    Dim handle3 As GCHandle = GCHandle.Alloc(p_lngRtn, GCHandleType.Pinned)
                    Try

                        Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()
                        Dim tmpPtr2 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(p_typSecurityID))
                        Marshal.StructureToPtr(p_typSecurityID, tmpPtr2, True)
                        MoveMem(tmpPtr2, tmpPtr3, Marshal.SizeOf(p_typSecurityID))
                        p_typSecurityID = Marshal.PtrToStructure(tmpPtr2, p_typSecurityID.GetType())
                    Finally
                        handle3.Free()
                    End Try

                    ' prepare S-SID_REVISION-
                    p_strSID = New StringBuilder("S-" & SID_REVISION & "-")

                    ' prepare SidIdentifierAuthority
                    If (p_typSecurityID.Value(0) <> 0) Or (p_typSecurityID.Value(1) <> 0) Then
                        p_strSID.Append("0x" & _
                                        FormatHex(p_typSecurityID.Value(0), 2) & _
                                        FormatHex(p_typSecurityID.Value(1), 2) & _
                                        FormatHex(p_typSecurityID.Value(2), 2) & _
                                        FormatHex(p_typSecurityID.Value(3), 2) & _
                                        FormatHex(p_typSecurityID.Value(4), 2) & _
                                        FormatHex(p_typSecurityID.Value(5), 2))
                    Else
                        p_strSID.Append( _
                                        (CStr(p_typSecurityID.Value(5) + (p_typSecurityID.Value(4) * 2 ^ 8) + (p_typSecurityID.Value(3) * 2 ^ 16) + (p_typSecurityID.Value(2) * 2 ^ 24))))
                    End If

                    ' obtain the SID's sub-authority count and add each to the string
                    Dim handle4 As GCHandle = GCHandle.Alloc(xi_lngPtrSID, GCHandleType.Pinned)
                    Try
                        Dim tmpPtr4 As IntPtr = handle4.AddrOfPinnedObject()
                        p_lngRtn = GetSidSubAuthorityCount(tmpPtr4)
                    Finally
                        handle4.Free()
                    End Try

                    If p_lngRtn Then
                        Dim handle5 As GCHandle = GCHandle.Alloc(p_lngNumSubAuthorities, GCHandleType.Pinned)
                        Dim handle6 As GCHandle = GCHandle.Alloc(p_lngRtn, GCHandleType.Pinned)
                        Try
                            Dim tmpPtr6 As IntPtr = handle6.AddrOfPinnedObject()
                            Dim tmpPtr5 As IntPtr = handle5.AddrOfPinnedObject()
                            MoveMem(tmpPtr5, tmpPtr6, 4)
                            p_lngNumSubAuthorities = Marshal.ReadInt32(tmpPtr5)
                        Finally
                            handle5.Free()
                            handle6.Free()
                        End Try

                        For p_lngLoop As Integer = 0 To p_lngNumSubAuthorities - 1
                            Dim handle7 As GCHandle = GCHandle.Alloc(xi_lngPtrSID, GCHandleType.Pinned)
                            Try
                                Dim tmpPtr7 As IntPtr = handle7.AddrOfPinnedObject()
                                p_lngRtn = GetSidSubAuthority(tmpPtr7, p_lngLoop)
                            Finally
                                handle7.Free()
                            End Try

                            If p_lngRtn <> 0 Then
                                Dim handle8 As GCHandle = GCHandle.Alloc(p_lngPtrSubAuthorities, GCHandleType.Pinned)
                                Dim handle9 As GCHandle = GCHandle.Alloc(p_lngRtn, GCHandleType.Pinned)
                                Try
                                    Dim tmpPtr9 As IntPtr = handle9.AddrOfPinnedObject()
                                    Dim tmpPtr8 As IntPtr = handle8.AddrOfPinnedObject()
                                    MoveMem(tmpPtr8, tmpPtr9, 4)
                                    p_lngPtrSubAuthorities = Marshal.ReadInt32(tmpPtr8)
                                Finally
                                    handle8.Free()
                                    handle9.Free()
                                End Try

                                ' Have to handle the #@$&#@( signed longs
                                If p_lngPtrSubAuthorities < 0 Then
                                    p_dblBigNum = p_lngPtrSubAuthorities + OFFSET_4
                                    p_strSID.Append("-" & p_dblBigNum)
                                Else
                                    p_strSID.Append("-" & p_lngPtrSubAuthorities)
                                End If
                            End If
                        Next

                    End If

                End If

            End If
        Finally
            handle10.Free()
        End Try

        Return p_strSID.ToString()

    End Function
End Module