Attribute VB_Name = "MSecurityPasswords"
' Module:   Secret database login information
' Shared:   Yes
' Needs:    Nothing
'
' This information is *NOT* for general use in code, because the
' architecture DLLs normally take care of it for you. It is in a
' separate module *ONLY* because some database utilities require
' access to the actual login information used by the DLLs.
'
' This information should NEVER have to be typed in manually
' or specified in any application.
'
Option Explicit

' Swift database login.
Public Const ksLoginName = "Swift"
Public Const ksLoginPassword = "hy4u8hv5495tyc92y637dx45t5c46y"

' Pure databases login.
Public Const ksSALoginName = "SIRIUS"
Public Const ksSALoginPassword = "$1R1U5"

' Interactive databases login.
Public Const ksInteractiveLoginName = "Interactive"
Public Const ksInteractiveLoginPassword = "h85gf756xdT576gu856q0p6ygf9jkt"

' Scottish Mutual: CRP-TVAS login.  This will use the same password as Swift (ksLoginPassword).
Public Const ksCRPLoginName As String = "SwiftCRP"

