Option Strict Off
Option Explicit On
Imports System

Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 17/02/1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBFindDocTemplate"


    <ThreadStatic()> _
    Public objfrmInteface As frmInterface
    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    Public Const ACCode As Integer = 102
    Public Const ACType As Integer = 103

    Public Const ACListTitle1 As Integer = 104
    Public Const ACListTitle2 As Integer = 105
    Public Const ACListTitle3 As Integer = 106
    Public Const ACListTitle4 As Integer = 308

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACNewButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACFindNowButton As Integer = 206
    Public Const ACNewSearchButton As Integer = 207
    Public Const ACDeleteButton As Integer = 208
    Public Const ACUnDeleteButton As Integer = 209

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307

    'SB 31/03/98 defect 37
    Public Const ACLookupFailTitle As Integer = 308
    Public Const ACLookupFail As Integer = 309

    ' Menus


    ' Constants for the search data array indexes.
    Public Const ACIDocumentTemplateId As Integer = 0
    Public Const ACIDocumentTemplateCode As Integer = 1
    Public Const ACIDocumentTemplateDescription As Integer = 2
    Public Const ACIDocumentTypeId As Integer = 3
    Public Const ACIDocumentTypeCode As Integer = 4
    Public Const ACIDocumentIsDeleted As Integer = 5
    Public Const ACIDocumentTypeDescription As Integer = 6
    Public Const ACIDocumentEffectiveDate As Integer = 7

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    Public Const ACMinSearchLength As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Const ACOptionUseUserFilter As Integer = 3

    Public Const ACInvisibleMergeMode As Integer = 2 'RWH(26/09/2000) RSAIB Process 28 - Risk docs.

    ' Public source and language ID's from the
    ' Object Manager.

    <ThreadStatic()> _
 Public g_iSourceID As Integer

    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.

    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.

    <ThreadStatic()> _
 Public g_oBusiness As bSIRFindDocTemplate.Form
    'Public g_oBusiness As bSIRFindDocTemplate.Form


    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenHelpID As Integer = 17

    Public Const lCLAUSE_TYPE_ID As Integer = 7 ' RWH(18/08/2000) RSAIB Process 12.  (Underwritnig system)
    Public Const CLAUSES_TYPE_CODE As String = "CLAUSES" ' RAM20050107 : Added to support SWIFT
    Public Const SUBDOC_TYPE_CODE As String = "SUBDOC" ' RAM20050107 : Added to support SWIFT
    Public Const lLETTER_TYPE_ID As Integer = 5
    Public Const lEMAIL_TYPE_ID As Integer = 8
    Public Const lSUBDOC_TYPE_ID As Integer = 9

    Sub Main_Renamed()

    End Sub

End Module