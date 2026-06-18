Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
Imports Artinsoft.VB6.Utils

Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: {TodaysDate}
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMCurrency"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACGridIsoCodeCaption As Integer = 103
    Public Const ACGridDescriptionCaption As Integer = 104
    Public Const ACGridIsBaseCaption As Integer = 105
    Public Const ACGridMinorPartCaption As Integer = 106
    Public Const ACGridSymbolCaption As Integer = 107
    Public Const ACGridAlignmentCaption As Integer = 108
    Public Const ACGridDecimalPlacesCaption As Integer = 109
    Public Const ACGridIsDeletedCaption As Integer = 110
    Public Const ACGridEffectiveDateCaption As Integer = 111
    Public Const ACGridFormatStringCaption As Integer = 112
    Public Const ACGridRoundToPlacesCaption As Integer = 113


    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    ' Menus

    ' Grid field positions
    '(display)
    Public Const ACGridIsoCode As Integer = 0
    Public Const ACGridDescription As Integer = 1
    Public Const ACGridIsBase As Integer = 2
    Public Const ACGridMinorPart As Integer = 3
    Public Const ACGridSymbol As Integer = 4
    Public Const ACGridAlignment As Integer = 5
    Public Const ACGridDecimalPlaces As Integer = 6
    Public Const ACGridIsDeleted As Integer = 7
    Public Const ACGridEffectiveDate As Integer = 8
    Public Const ACGridFormatString As Integer = 9
    Public Const ACGridRoundToPlaces As Integer = 10
    '(non-display)
    Public Const ACGridCurrencyId As Integer = 11
    Public Const ACGridCaptionId As Integer = 12
    Public Const ACGridCode As Integer = 13
    ' {* USER DEFINED CODE (End) *}

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer

    ' Public array for storing data for the grid.
    'developer guide no.111
    'Public g_vGridData() As Object
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_vGridData As Object
    Public gridData As New XArrayHelper
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager
    <ThreadStatic()> _
   Public g_ToMessage As Boolean


    Sub Main()
        Dim obj As New Interface_Renamed
        obj.Initialise()
        obj.Start()
    End Sub
    
End Module