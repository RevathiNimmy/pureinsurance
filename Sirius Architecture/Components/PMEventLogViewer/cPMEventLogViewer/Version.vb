Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Runtime.InteropServices
<System.Runtime.InteropServices.ProgId("cVersion_NET.cVersion")> _
Public NotInheritable Class cVersion 
	' ************************************************************
	' Copyright ? 1999-2001 Slightly Tilted Software
	' All rights reserved
	' You're absolutely free to use these resources within your
	'     own applications, but you may not redistribute them
	'     (as source) in any manner whatsoever, whether for profit
	'     or not.
	' The only legitimate source for the original source code is
	'     at the VBPJ site and my own web site at:
	'     http://www.SlightlyTiltedSoftware.com
	' ************************************************************
	
	' ---------------------------------------------
	' Module    : Version.cls
	' By        : L.J. Johnson       Date: 04-28-2001
	' Comments  : Get the version info for the
	'           : current Operating System
	' ---------------------------------------------
	
	' ************************************************
	' Added to replace global variables 18/09/2003
	' Username.
	Private m_sUsername As String = ""
	
	' Password.
	Private m_sPassword As String = ""
	
	' User ID
	Private m_iUserID As Integer
	
	' Calling Application
	Private m_sCallingAppName As String = ""
	' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	' ************************************************
	
	
	' -------------------------------------------------
	' For keeping track of our own error info
	' -------------------------------------------------
	Private m_lngErrNumber As Integer
	Private m_strErrSource As String = ""
	Private m_strErrDesc As String = ""
	
	' --------------------------------------------
	' Public enumeration
	' --------------------------------------------
	Public Enum enmValidOSs
		Invalid_OS = 0
		Valid_Win98 = 1
		Valid_NT = 2
		Valid_Win2000 = 3
		Valid_Whistler = 4
	End Enum
	
	' --------------------------------------------
	' API calls
	' --------------------------------------------

	Private Declare Function GetVersionEx Lib "Kernel32"  Alias "GetVersionExA"(ByRef lpVersionInformation As OSVERSIONINFO) As Integer
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Private Structure OSVERSIONINFO
		Dim dwOSVersionInfoSize As Integer
		Dim dwMajorVersion As Integer
		Dim dwMinorVersion As Integer
		Dim dwBuildNumber As Integer
		Dim dwPlatformId As Integer
        '<VBFixedString(128),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=128)> _

        Public szCSDVersion As FixedLengthString
		Public Shared Function CreateInstance() As OSVERSIONINFO
			Dim result As New OSVERSIONINFO
			result.szCSDVersion = New FixedLengthString(128)
			Return result
		End Function
	End Structure
	
	Private Enum enmPlatform
		VER_PLATFORM_UNKNOWN = -1
		VER_PLATFORM_WIN32s = 0
		VER_PLATFORM_WIN32_NT = 2
		VER_PLATFORM_WIN32_WINDOWS = 1
	End Enum
	

	Private m_typOSVersionInfo As OSVERSIONINFO = OSVERSIONINFO.CreateInstance()
	
	Private m_blnIsValidOS As Boolean
	Private m_strVersionText As String = ""
	Private m_strErrMsg As String = ""
	Private m_typOS As enmValidOSs
	
	' *****************************************************************************
	' *****************************************************************************
	' *****                         PUBLIC Methods                            *****
	' *****************************************************************************
	' *****************************************************************************
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Function TypeValidOS
	' Written By   : L.J. Johnson
	' Programmer   : L.J. Johnson [Slightly Tilted Software]
	' Date Writen  : 05/05/2001 -- 13:27:54
	' Inputs       : N/A
	' Outputs      : enmValidOSs --
	' Description  : Return enum for the OS type (thus, the
	'              :     public enum)
	' Called By    : *****
	' *******************************************************
	Public Function TypeValidOS() As enmValidOSs
		Return m_typOS
	End Function
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Function OS_TypeError
	' Written By   : L.J. Johnson
	' Programmer   : L.J. Johnson [Slightly Tilted Software]
	' Date Writen  : 05/05/2001 -- 13:27:50
	' Inputs       : N/A
	' Outputs      : String --
	' Description  : If you got an Invalid_OS back from TypeValidOS(),
	'              :     then you can get more info here
	' Called By    : *****
	' *******************************************************
	Public Function OS_TypeError() As String
		Return m_strErrMsg
	End Function
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Function OS_VersionText
	' Written By   : L.J. Johnson
	' Programmer   : L.J. Johnson [Slightly Tilted Software]
	' Date Writen  : 05/05/2001 -- 13:27:47
	' Inputs       : N/A
	' Outputs      : String --
	' Description  : Get the text string for the Operating System
	' Called By    : *****
	' *******************************************************
	Public Function OS_VersionText() As String
		Return m_strVersionText
	End Function
	
	' *****************************************************************************
	' *****************************************************************************
	' *****                         PRIVATE Methods                           *****
	' *****************************************************************************
	' *****************************************************************************
	
	' *******************************************************
	' Routine Name : (PRIVATE in CLASS) Sub GetOSVersion
	' Written By   : L.J. Johnson
	' Programmer   : L.J. Johnson [Slightly Tilted Software]
	' Date Writen  : 05/05/2001 -- 13:22:38
	' Inputs       : N/A
	' Outputs      : N/A
	' Description  : Get the current Operating System version
	' Called By    : Class_Initialize()
	' *******************************************************
	Private Sub GetOSVersion()
		Dim p_strVersion As String = ""
		Dim p_intServicePack As Integer
		
		' ------------------------------------------
		' Set the length and make API call
		' ------------------------------------------

		m_typOSVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(m_typOSVersionInfo)
		GetVersionEx(m_typOSVersionInfo)
		
		' ------------------------------------------
		' Trim the null from the Version string
		' ------------------------------------------
		Dim p_lngPos As Integer = (m_typOSVersionInfo.szCSDVersion.Value.IndexOf(Strings.Chr(0).ToString()) + 1)
		If p_lngPos > 0 Then
			p_strVersion = m_typOSVersionInfo.szCSDVersion.Value.Substring(0, Math.Min(m_typOSVersionInfo.szCSDVersion.Value.Length, p_lngPos - 1)).Trim()
		Else
			p_strVersion = Nothing
		End If
		
		' ------------------------------------------
		' Get the details
		' ------------------------------------------
		Dim p_lngMajorVer As Integer = m_typOSVersionInfo.dwMajorVersion
		Dim p_lngMinorVer As Integer = m_typOSVersionInfo.dwMinorVersion
		Dim p_intBuildNum As Integer = WordLow(m_typOSVersionInfo.dwBuildNumber)
		If p_strVersion.Length > 0 Then
			p_intServicePack = Conversion.Val(p_strVersion.Substring(p_strVersion.Length - 1))
		Else
			p_intServicePack = 0
		End If
		
		' ------------------------------------------
		' Create a combined version string
		' ------------------------------------------
		m_strVersionText = CStr(p_lngMajorVer) & "." &  _
		                   p_lngMinorVer
		
		If p_intBuildNum > 0 Then
			m_strVersionText = m_strVersionText & " Build " & CStr(p_intBuildNum)
		End If
		
		If p_intServicePack > 0 Then
			m_strVersionText = m_strVersionText & " SP " & CStr(p_intServicePack)
		End If
		
		' ------------------------------------------
		'
		' ------------------------------------------
		Select Case m_typOSVersionInfo.dwPlatformId
			Case enmPlatform.VER_PLATFORM_WIN32s
				m_strVersionText = "Win32s " & m_strVersionText
				
				' ------------------------------------
				' Not supported
				' ------------------------------------
				m_strErrMsg = "You are running Win32s. This is not supported!"
				m_blnIsValidOS = False
				m_typOS = enmValidOSs.Invalid_OS
				
			Case enmPlatform.VER_PLATFORM_WIN32_NT
				m_strVersionText = "Windows NT " & m_strVersionText
				
				' ------------------------------------
				' Pre-NT 4.0
				' ------------------------------------
				If p_lngMajorVer < 4 Then
					m_strErrMsg = "You are running " & m_strVersionText &  _
					              ". You must be running at least NT 4.0 with SP 4 or greater"
					m_blnIsValidOS = False
					m_typOS = enmValidOSs.Invalid_OS
					
					' ------------------------------------
					' NT 4.0
					' ------------------------------------
				ElseIf p_lngMajorVer = 4 Then 
					' ---------------------------------
					' It's NT 4.0, now need to check for
					'     SP4 or greater
					' ---------------------------------
					If p_intServicePack < 4 Then
						m_strErrMsg = "You are running " & m_strVersionText &  _
						              ". You must be running at least NT 4.0 with SP 4 or greater"
						m_blnIsValidOS = False
						m_typOS = enmValidOSs.Invalid_OS
					Else
						m_strErrMsg = Nothing
						m_blnIsValidOS = True
						m_typOS = enmValidOSs.Valid_NT
					End If
					
					' ------------------------------------
					' Win2000
					' ------------------------------------
				ElseIf p_lngMajorVer = 5 Then 
					' ---------------------------------
					' Win2000 -- check for build 2195
					'     or greater
					' ---------------------------------
					If p_intBuildNum < 2031 Then
						m_strErrMsg = "You are running " & m_strVersionText &  _
						              ". For Win2000, you must be running at least " &  _
						              "build 2195 (RTM version)."
						m_blnIsValidOS = False
						m_typOS = enmValidOSs.Invalid_OS
					Else
						m_strErrMsg = Nothing
						m_blnIsValidOS = True
						m_typOS = enmValidOSs.Valid_Win2000
					End If
				Else
					m_strErrMsg = "Unknown operating system: " & m_strVersionText
					m_blnIsValidOS = False
					m_typOS = enmValidOSs.Invalid_OS
				End If
				
			Case enmPlatform.VER_PLATFORM_WIN32_WINDOWS
				m_strVersionText = "Windows 98 " & m_strVersionText
				m_strErrMsg = Nothing
				m_blnIsValidOS = True
				m_typOS = enmValidOSs.Valid_Win98
				
			Case Else
				m_strErrMsg = "Unknown operating system: " & m_strVersionText
				m_blnIsValidOS = False
				m_typOS = enmValidOSs.Invalid_OS
				
		End Select
		
	End Sub
	
	' *******************************************************
	' Routine Name : (PRIVATE in CLASS) Function WordLow
	' Written By   : L.J. Johnson
	' Programmer   : L.J. Johnson [Slightly Tilted Software]
	' Date Writen  : 05/05/2001 -- 13:21:56
	' Inputs       : ByVal xi_lngInput:Long -
	' Outputs      : Integer --
	' Description  : Get the low word from a long value
	' Called By    : GetOSVersion()
	' *******************************************************
	Private Function WordLow(ByVal xi_lngInput As Integer) As Integer
		
		If (xi_lngInput And &HFFFF) > &H7FFFs Then
			Return (xi_lngInput And &HFFFF) - &H10000
		Else
			Return xi_lngInput And &HFFFF
		End If
		
	End Function
	
	' *******************************************************
	' Routine Name : (PRIVATE in CLASS) Sub Class_Initialize
	' Written By   : L.J. Johnson
	' Programmer   : L.J. Johnson [Slightly Tilted Software]
	' Date Writen  : 05/05/2001 -- 13:23:06
	' Inputs       : N/A
	' Outputs      : N/A
	' Description  : Only need to get this info once, so do
	'              :     it when the object is created
	' Called By    : *****
	' *******************************************************
	Public Sub New()
		MyBase.New()
		GetOSVersion()
	End Sub
End Class
