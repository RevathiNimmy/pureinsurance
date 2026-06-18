Option Strict Off
Option Explicit On
Imports System
Imports System.Runtime.InteropServices
Imports System.Text
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 18 July 1996
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' DAK040100 - Add API functions GetPrivateProfileString
    ' ***************************************************************** '


    ' Main global constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bLicenceAdmin"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Other constants
    Public Const ACSystemPrefix As String = "SYSTEM_"
    Public Const ACProductPrefix As String = "PRODUCT_"
    Public Const ACInstalledProducts As String = "INSTALLED_PRODUCTS"
    Public Const ACProductDescription As String = "ProductDescription"
    Public Const ACLicenceLimit As String = "LicenceLimit"
    Public Const ACLicenceKey As String = "LicenceKey"
    Public Const ACExceedLimitAction As String = "ExceedLimitAction"
    Public Const ACExceedLimitBlock As String = "Block"
    Public Const ACExceedLimitWarn As String = "Warn"
    Public Const ACExceedLimitNone As String = "None"

    ' Username and Password
    'developer guide no. 107
    <ThreadStatic()>
    Public g_sUsername As String = ""
    'developer guide no. 107
    <ThreadStatic()>
    Public g_sPassword As String = ""
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_sCallingAppName As String = ""
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLogLevel As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iUserID As Integer
    Public Const kGetProductUserName As String = "SelectProductUser"
    Public Const kGetProductUserSQL As String = "spu_PMWrk_Product_User_Sel"

    'DAK040100   
    <DllImport("kernel32.dll", CharSet:=CharSet.Ansi, SetLastError:=True)>
    Public Function GetPrivateProfileString(
        ByVal lpAppName As String,
        ByVal lpKeyName As String,
        ByVal lpDefault As String,
        ByVal lpReturnedString As StringBuilder,
        ByVal nSize As Integer,
        ByVal lpFileName As String) As Integer
    End Function
    Sub Main_Renamed()
        ' Main entry point for the component.

        ' Default LanguageID, SourceID & CallingAppName
        g_iLanguageID = 1
		g_iSourceID = 1
		g_iCurrencyID = 1
		g_sCallingAppName = "LICENCEMANAGER"
		g_iUserID = 1


	End Sub
End Module