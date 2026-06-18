Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Module modFormatMsg
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '


    ' Used by FormatMessage function
    Private Const FORMAT_MESSAGE_ALLOCATE_BUFFER As Integer = &H100
    Private Const FORMAT_MESSAGE_IGNORE_INSERTS As Integer = &H200
    Private Const FORMAT_MESSAGE_FROM_STRING As Integer = &H400
    Private Const FORMAT_MESSAGE_FROM_HMODULE As Integer = &H800
    Private Const FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000
    Private Const FORMAT_MESSAGE_ARGUMENT_ARRAY As Integer = &H2000
    Private Const FORMAT_MESSAGE_MAX_WIDTH_MASK As Integer = &HFF

    Public Declare Function FormatMessage Lib "Kernel32" Alias "FormatMessageA" (ByVal dwFlags As Integer, ByVal lpSource As Integer, ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByVal lpBuffer As String, ByVal nSize As Integer, ByVal Arguments As Integer) As Integer

    Public Structure FmtMsgArrayType
        Dim s1 As String
        Dim s2 As String
        Dim s3 As String
        Dim s4 As String
        Dim s5 As String
        Dim s6 As String
        Dim s7 As String
        Dim s8 As String
        Dim s9 As String
        Dim s10 As String
        Dim s11 As String
        Dim s12 As String
        Dim s13 As String
        Dim s14 As String
        Dim s15 As String
        Dim s16 As String
        Dim s17 As String
        Dim s18 As String
        Dim s19 As String
        Dim s20 As String
        Dim s21 As String
        Dim s22 As String
        Dim s23 As String
        Dim s24 As String
        Dim s25 As String
        Dim s26 As String
        Dim s27 As String
        Dim s28 As String
        Dim s29 As String
        Dim s30 As String
        Dim s31 As String
        Dim s32 As String
        Dim s33 As String
        Dim s34 As String
        Dim s35 As String
        Dim s36 As String
        Dim s37 As String
        Dim s38 As String
        Dim s39 As String
        Dim s40 As String
        Dim s41 As String
        Dim s42 As String
        Dim s43 As String
        Dim s44 As String
        Dim s45 As String
        Dim s46 As String
        Dim s47 As String
        Dim s48 As String
        Dim s49 As String
        Dim s50 As String
        Dim s51 As String
        Dim s52 As String
        Dim s53 As String
        Dim s54 As String
        Dim s55 As String
        Dim s56 As String
        Dim s57 As String
        Dim s58 As String
        Dim s59 As String
        Dim s60 As String
        Dim s61 As String
        Dim s62 As String
        Dim s63 As String
        Dim s64 As String
        Dim s65 As String
        Dim s66 As String
        Dim s67 As String
        Dim s68 As String
        Dim s69 As String
        Dim s70 As String
        Dim s71 As String
        Dim s72 As String
        Dim s73 As String
        Dim s74 As String
        Dim s75 As String
        Dim s76 As String
        Dim s77 As String
        Dim s78 As String
        Dim s79 As String
        Dim s80 As String
        Dim s81 As String
        Dim s82 As String
        Dim s83 As String
        Dim s84 As String
        Dim s85 As String
        Dim s86 As String
        Dim s87 As String
        Dim s88 As String
        Dim s89 As String
        Dim s90 As String
        Dim s91 As String
        Dim s92 As String
        Dim s93 As String
        Dim s94 As String
        Dim s95 As String
        Dim s96 As String
        Dim s97 As String
        Dim s98 As String
        Dim s99 As String
        Private Shared Sub InitStruct(ByRef result As FmtMsgArrayType, ByVal init As Boolean)
            If init Then
                result.s1 = String.Empty
                result.s2 = String.Empty
                result.s3 = String.Empty
                result.s4 = String.Empty
                result.s5 = String.Empty
                result.s6 = String.Empty
                result.s7 = String.Empty
                result.s8 = String.Empty
                result.s9 = String.Empty
                result.s10 = String.Empty
                result.s11 = String.Empty
                result.s12 = String.Empty
                result.s13 = String.Empty
                result.s14 = String.Empty
                result.s15 = String.Empty
                result.s16 = String.Empty
                result.s17 = String.Empty
                result.s18 = String.Empty
                result.s19 = String.Empty
                result.s20 = String.Empty
                result.s21 = String.Empty
                result.s22 = String.Empty
                result.s23 = String.Empty
                result.s24 = String.Empty
                result.s25 = String.Empty
                result.s26 = String.Empty
                result.s27 = String.Empty
                result.s28 = String.Empty
                result.s29 = String.Empty
                result.s30 = String.Empty
                result.s31 = String.Empty
                result.s32 = String.Empty
                result.s33 = String.Empty
                result.s34 = String.Empty
                result.s35 = String.Empty
                result.s36 = String.Empty
                result.s37 = String.Empty
                result.s38 = String.Empty
                result.s39 = String.Empty
                result.s40 = String.Empty
                result.s41 = String.Empty
                result.s42 = String.Empty
                result.s43 = String.Empty
                result.s44 = String.Empty
                result.s45 = String.Empty
                result.s46 = String.Empty
                result.s47 = String.Empty
                result.s48 = String.Empty
                result.s49 = String.Empty
                result.s50 = String.Empty
                result.s51 = String.Empty
                result.s52 = String.Empty
                result.s53 = String.Empty
                result.s54 = String.Empty
                result.s55 = String.Empty
                result.s56 = String.Empty
                result.s57 = String.Empty
                result.s58 = String.Empty
                result.s59 = String.Empty
                result.s60 = String.Empty
                result.s61 = String.Empty
                result.s62 = String.Empty
                result.s63 = String.Empty
                result.s64 = String.Empty
                result.s65 = String.Empty
                result.s66 = String.Empty
                result.s67 = String.Empty
                result.s68 = String.Empty
                result.s69 = String.Empty
                result.s70 = String.Empty
                result.s71 = String.Empty
                result.s72 = String.Empty
                result.s73 = String.Empty
                result.s74 = String.Empty
                result.s75 = String.Empty
                result.s76 = String.Empty
                result.s77 = String.Empty
                result.s78 = String.Empty
                result.s79 = String.Empty
                result.s80 = String.Empty
                result.s81 = String.Empty
                result.s82 = String.Empty
                result.s83 = String.Empty
                result.s84 = String.Empty
                result.s85 = String.Empty
                result.s86 = String.Empty
                result.s87 = String.Empty
                result.s88 = String.Empty
                result.s89 = String.Empty
                result.s90 = String.Empty
                result.s91 = String.Empty
                result.s92 = String.Empty
                result.s93 = String.Empty
                result.s94 = String.Empty
                result.s95 = String.Empty
                result.s96 = String.Empty
                result.s97 = String.Empty
                result.s98 = String.Empty
                result.s99 = String.Empty
            End If
        End Sub
        Public Shared Function CreateInstance() As FmtMsgArrayType
            Dim result As New FmtMsgArrayType
            InitStruct(result, True)
            Return result
        End Function
        Public Function Clone() As FmtMsgArrayType
            Dim result As FmtMsgArrayType = Me
            InitStruct(result, False)
            Return result
        End Function
    End Structure

    Public Function TranslateArray(ByRef xi_astrArgs() As String, ByRef xio_UDT As FmtMsgArrayType) As Integer

        Try  ' Don't accept an error here
            Dim p_lngCounter As Integer

            Dim p_lngLower As Integer = xi_astrArgs.GetLowerBound(0)
            Dim p_lngUpper As Integer = xi_astrArgs.GetUpperBound(0)

            If p_lngLower < 0 Then
                Exit Function
            ElseIf p_lngLower > p_lngUpper Then
                Exit Function
            End If

            For p_lngLoop As Integer = p_lngLower To p_lngUpper
                p_lngCounter += 1
                Select Case p_lngCounter
                    Case 1 : xio_UDT.s1 = xi_astrArgs(p_lngLoop)
                    Case 2 : xio_UDT.s2 = xi_astrArgs(p_lngLoop)
                    Case 3 : xio_UDT.s3 = xi_astrArgs(p_lngLoop)
                    Case 4 : xio_UDT.s4 = xi_astrArgs(p_lngLoop)
                    Case 5 : xio_UDT.s5 = xi_astrArgs(p_lngLoop)
                    Case 6 : xio_UDT.s6 = xi_astrArgs(p_lngLoop)
                    Case 7 : xio_UDT.s7 = xi_astrArgs(p_lngLoop)
                    Case 8 : xio_UDT.s8 = xi_astrArgs(p_lngLoop)
                    Case 9 : xio_UDT.s9 = xi_astrArgs(p_lngLoop)
                    Case 10 : xio_UDT.s10 = xi_astrArgs(p_lngLoop)
                    Case 11 : xio_UDT.s11 = xi_astrArgs(p_lngLoop)
                    Case 12 : xio_UDT.s12 = xi_astrArgs(p_lngLoop)
                    Case 13 : xio_UDT.s13 = xi_astrArgs(p_lngLoop)
                    Case 14 : xio_UDT.s14 = xi_astrArgs(p_lngLoop)
                    Case 15 : xio_UDT.s15 = xi_astrArgs(p_lngLoop)
                    Case 16 : xio_UDT.s16 = xi_astrArgs(p_lngLoop)
                    Case 17 : xio_UDT.s17 = xi_astrArgs(p_lngLoop)
                    Case 18 : xio_UDT.s18 = xi_astrArgs(p_lngLoop)
                    Case 19 : xio_UDT.s19 = xi_astrArgs(p_lngLoop)
                    Case 20 : xio_UDT.s20 = xi_astrArgs(p_lngLoop)
                    Case 21 : xio_UDT.s21 = xi_astrArgs(p_lngLoop)
                    Case 22 : xio_UDT.s22 = xi_astrArgs(p_lngLoop)
                    Case 23 : xio_UDT.s23 = xi_astrArgs(p_lngLoop)
                    Case 24 : xio_UDT.s24 = xi_astrArgs(p_lngLoop)
                    Case 25 : xio_UDT.s25 = xi_astrArgs(p_lngLoop)
                    Case 26 : xio_UDT.s26 = xi_astrArgs(p_lngLoop)
                    Case 27 : xio_UDT.s27 = xi_astrArgs(p_lngLoop)
                    Case 28 : xio_UDT.s28 = xi_astrArgs(p_lngLoop)
                    Case 29 : xio_UDT.s29 = xi_astrArgs(p_lngLoop)
                    Case 30 : xio_UDT.s30 = xi_astrArgs(p_lngLoop)
                    Case 31 : xio_UDT.s31 = xi_astrArgs(p_lngLoop)
                    Case 32 : xio_UDT.s32 = xi_astrArgs(p_lngLoop)
                    Case 33 : xio_UDT.s33 = xi_astrArgs(p_lngLoop)
                    Case 34 : xio_UDT.s34 = xi_astrArgs(p_lngLoop)
                    Case 35 : xio_UDT.s35 = xi_astrArgs(p_lngLoop)
                    Case 36 : xio_UDT.s36 = xi_astrArgs(p_lngLoop)
                    Case 37 : xio_UDT.s37 = xi_astrArgs(p_lngLoop)
                    Case 38 : xio_UDT.s38 = xi_astrArgs(p_lngLoop)
                    Case 39 : xio_UDT.s39 = xi_astrArgs(p_lngLoop)
                    Case 40 : xio_UDT.s40 = xi_astrArgs(p_lngLoop)
                    Case 41 : xio_UDT.s41 = xi_astrArgs(p_lngLoop)
                    Case 42 : xio_UDT.s42 = xi_astrArgs(p_lngLoop)
                    Case 43 : xio_UDT.s43 = xi_astrArgs(p_lngLoop)
                    Case 44 : xio_UDT.s44 = xi_astrArgs(p_lngLoop)
                    Case 45 : xio_UDT.s45 = xi_astrArgs(p_lngLoop)
                    Case 46 : xio_UDT.s46 = xi_astrArgs(p_lngLoop)
                    Case 47 : xio_UDT.s47 = xi_astrArgs(p_lngLoop)
                    Case 48 : xio_UDT.s48 = xi_astrArgs(p_lngLoop)
                    Case 49 : xio_UDT.s49 = xi_astrArgs(p_lngLoop)
                    Case 50 : xio_UDT.s50 = xi_astrArgs(p_lngLoop)
                    Case 51 : xio_UDT.s51 = xi_astrArgs(p_lngLoop)
                    Case 52 : xio_UDT.s52 = xi_astrArgs(p_lngLoop)
                    Case 53 : xio_UDT.s53 = xi_astrArgs(p_lngLoop)
                    Case 54 : xio_UDT.s54 = xi_astrArgs(p_lngLoop)
                    Case 55 : xio_UDT.s55 = xi_astrArgs(p_lngLoop)
                    Case 56 : xio_UDT.s56 = xi_astrArgs(p_lngLoop)
                    Case 57 : xio_UDT.s57 = xi_astrArgs(p_lngLoop)
                    Case 58 : xio_UDT.s58 = xi_astrArgs(p_lngLoop)
                    Case 59 : xio_UDT.s59 = xi_astrArgs(p_lngLoop)
                    Case 60 : xio_UDT.s60 = xi_astrArgs(p_lngLoop)
                    Case 61 : xio_UDT.s61 = xi_astrArgs(p_lngLoop)
                    Case 62 : xio_UDT.s62 = xi_astrArgs(p_lngLoop)
                    Case 63 : xio_UDT.s63 = xi_astrArgs(p_lngLoop)
                    Case 64 : xio_UDT.s64 = xi_astrArgs(p_lngLoop)
                    Case 65 : xio_UDT.s65 = xi_astrArgs(p_lngLoop)
                    Case 66 : xio_UDT.s66 = xi_astrArgs(p_lngLoop)
                    Case 67 : xio_UDT.s67 = xi_astrArgs(p_lngLoop)
                    Case 68 : xio_UDT.s68 = xi_astrArgs(p_lngLoop)
                    Case 69 : xio_UDT.s69 = xi_astrArgs(p_lngLoop)
                    Case 70 : xio_UDT.s70 = xi_astrArgs(p_lngLoop)
                    Case 71 : xio_UDT.s71 = xi_astrArgs(p_lngLoop)
                    Case 72 : xio_UDT.s72 = xi_astrArgs(p_lngLoop)
                    Case 73 : xio_UDT.s73 = xi_astrArgs(p_lngLoop)
                    Case 74 : xio_UDT.s74 = xi_astrArgs(p_lngLoop)
                    Case 75 : xio_UDT.s75 = xi_astrArgs(p_lngLoop)
                    Case 76 : xio_UDT.s76 = xi_astrArgs(p_lngLoop)
                    Case 77 : xio_UDT.s77 = xi_astrArgs(p_lngLoop)
                    Case 78 : xio_UDT.s78 = xi_astrArgs(p_lngLoop)
                    Case 79 : xio_UDT.s79 = xi_astrArgs(p_lngLoop)
                    Case 80 : xio_UDT.s80 = xi_astrArgs(p_lngLoop)
                    Case 81 : xio_UDT.s81 = xi_astrArgs(p_lngLoop)
                    Case 82 : xio_UDT.s82 = xi_astrArgs(p_lngLoop)
                    Case 83 : xio_UDT.s83 = xi_astrArgs(p_lngLoop)
                    Case 84 : xio_UDT.s84 = xi_astrArgs(p_lngLoop)
                    Case 85 : xio_UDT.s85 = xi_astrArgs(p_lngLoop)
                    Case 86 : xio_UDT.s86 = xi_astrArgs(p_lngLoop)
                    Case 87 : xio_UDT.s87 = xi_astrArgs(p_lngLoop)
                    Case 88 : xio_UDT.s88 = xi_astrArgs(p_lngLoop)
                    Case 89 : xio_UDT.s89 = xi_astrArgs(p_lngLoop)
                    Case 90 : xio_UDT.s90 = xi_astrArgs(p_lngLoop)
                    Case 91 : xio_UDT.s91 = xi_astrArgs(p_lngLoop)
                    Case 92 : xio_UDT.s92 = xi_astrArgs(p_lngLoop)
                    Case 93 : xio_UDT.s93 = xi_astrArgs(p_lngLoop)
                    Case 94 : xio_UDT.s94 = xi_astrArgs(p_lngLoop)
                    Case 95 : xio_UDT.s95 = xi_astrArgs(p_lngLoop)
                    Case 96 : xio_UDT.s96 = xi_astrArgs(p_lngLoop)
                    Case 97 : xio_UDT.s97 = xi_astrArgs(p_lngLoop)
                    Case 98 : xio_UDT.s98 = xi_astrArgs(p_lngLoop)
                    Case 99 : xio_UDT.s99 = xi_astrArgs(p_lngLoop)
                End Select
            Next p_lngLoop

            ' Set the return value
            Return p_lngCounter

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function

    Public Function FormatMsgFromResource(ByVal xi_lngModuleHwnd As Integer, ByVal xi_lngEventID As Integer, ByRef xi_typMsgs As FmtMsgArrayType, ByRef xio_strMsg As String, ByRef xio_strApiError As String, Optional ByVal xi_blnRtnErrors As Boolean = True) As Integer

        ' Set the flags for the API call
        Dim result As Integer = 0
        Dim p_lngFlags As Integer = FORMAT_MESSAGE_FROM_HMODULE Or FORMAT_MESSAGE_ARGUMENT_ARRAY
        Dim p_lngSize As Integer = 10240
        Dim p_strMsg As String = New String(" "c, p_lngSize)

        ' Get the message string from the file
        Dim handle As GCHandle = GCHandle.Alloc(xi_lngModuleHwnd, GCHandleType.Pinned)
        Dim p_lngRtn As Integer = 0
        Try
            Dim tmpPtr2 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(xi_typMsgs))
            Marshal.StructureToPtr(xi_typMsgs, tmpPtr2, True)
            Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
            p_lngRtn = FormatMessage(p_lngFlags, tmpPtr, xi_lngEventID, 0, p_strMsg, p_lngSize, tmpPtr2)
            xi_typMsgs = Marshal.PtrToStructure(tmpPtr2, xi_typMsgs.GetType())
        Finally
            handle.Free()
        End Try

        ' Check for errors
        If p_lngRtn = 0 Then
            xio_strMsg = Nothing

            result = (ERR_FAILED_FORMAT_MSG + Constants.vbObjectError)

            If xi_blnRtnErrors Then
                xio_strApiError = ReturnApiErrString(Information.Err().LastDllError)
            Else
                xio_strApiError = Nothing
            End If

        Else
            If xi_blnRtnErrors Then
                xio_strMsg = p_strMsg
                result = 0
                xio_strApiError = Nothing
            Else
                xio_strMsg = p_strMsg
                result = 0
                xio_strApiError = Nothing
            End If

        End If

        Return result
    End Function
End Module