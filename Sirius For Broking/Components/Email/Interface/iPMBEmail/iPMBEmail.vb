Option Strict Off
Option Explicit On
Module MainModule

    ' Constant for the functions to identify
    ' which class this is.
    Public Const ACApp As String = "Email"
    Private Const ACClass As String = "MainModule"

    Public Const USER_PROPERTY_ARCHIVE As String = "Sirius Archive"
    Public Const USER_PROPERTY_DATABASE As String = "Sirius Database Name"
    Public Const USER_PROPERTY_CLIENT_KEY As String = "Sirius Client Key"
    Public Const USER_PROPERTY_INSURANCEFILE_KEY As String = "Sirius Insurance File Key"
    Public Const USER_PROPERTY_INSURANCEFOLDER_KEY As String = "Sirius Insurance Folder Key"
    Public Const USER_PROPERTY_CLAIM_KEY As String = "Sirius Claim Key"
    Public Const USER_PROPERTY_BRANCH_CODE As String = "Sirius Branch Code"
    Public Const USER_PROPERTY_ORIGIN As String = "Sirius Origin"

    Public Const ARRAY_EMAIL_LOWER As Integer = 0
    Public Const ARRAY_EMAIL_EMAILADDRESS As Integer = 0
    Public Const ARRAY_EMAIL_DESCRIPTION As Integer = 1
    Public Const ARRAY_EMAIL_UPPER As Integer = 1
    '
    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iSourceID As Integer
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    Public Sub Main()

    End Sub
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module