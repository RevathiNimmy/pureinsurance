Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  18/06/2007
    '
    ' Description: Main Module.
    '
    ' Edit History:VB
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bCLMCase"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"


    ' Constants for the search data array indexes.
    ' ResultArray
    Public Const kICaseID As Integer = 0
    Public Const kICaseNumber As Integer = 1
    Public Const kICaseOpenedDate As Integer = 2
    Public Const kICaseVersion As Integer = 3
    Public Const kICaseProgressStatusID As Integer = 4
    Public Const kICaseAnaystID As Integer = 5
    Public Const kICaseAssistantID As Integer = 6
    Public Const kIBaseCaseID As Integer = 7
    Public Const kIUserID As Integer = 8


    'Date Formats
    Public Const ACDateConversion As String = "dd/mm/yyyy"
    Public Const ACDateDispaly As String = "dddd , mmmm d ,yyyy"
    Public Const ACShortDate As String = "short date"
    Public Const ACDateReverse As String = "yyyy/mm/dd"
End Module