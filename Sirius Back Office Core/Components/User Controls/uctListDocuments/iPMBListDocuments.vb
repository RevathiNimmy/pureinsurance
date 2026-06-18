Option Strict Off
Option Explicit On
Imports System
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


    Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer)


    Public Const ScreenHelpID As Integer = 44000

    ' Main public constant for all functions to identify which application this is.
    Public Const ACApp As String = "uctListDocumentsControl"
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public interface constants used when retrieving data from the resource file.

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102

    Public Const ACClientCode As Integer = 103
    Public Const ACPolicyCode As Integer = 104

    Public Const ACListTitle1 As Integer = 113
    Public Const ACListTitle2 As Integer = 114
    Public Const ACListTitle3 As Integer = 115
    Public Const ACListTitle4 As Integer = 116
    Public Const ACListTitle5 As Integer = 117
    Public Const ACListTitle6 As Integer = 118
    Public Const ACListTitle7 As Integer = 119
    Public Const ACListTitle8 As Integer = 120
    Public Const ACListTitle9 As Integer = 121
    Public Const ACListTitle10 As Integer = 122
    Public Const ACListTitle11 As Integer = 123
    Public Const ACListTitle12 As Integer = 124
    Public Const ACListTitle13 As Integer = 125

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACNewButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACFindNowButton As Integer = 206
    Public Const ACNewSearchButton As Integer = 207

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307

    Public Const ACLookupFailTitle As Integer = 308
    Public Const ACLookupFail As Integer = 309

    ' Menus

    Public Const ACDSDocumentSpoolerId As Integer = 0
    Public Const ACDSPartyCnt As Integer = 1
    Public Const ACDSInsuranceFolderCnt As Integer = 2
    Public Const ACDSInsuranceFileCnt As Integer = 3
    Public Const ACDSClaimCnt As Integer = 4
    Public Const ACDSDocumentTypeId As Integer = 5
    Public Const ACDSContact As Integer = 6
    Public Const ACDSDescription As Integer = 7
    Public Const ACDSDocumentPrinter As Integer = 8
    Public Const ACDSSpoolLevelInd As Integer = 9
    Public Const ACDSUnzipped As Integer = 10
    Public Const ACDSTemplateCode As Integer = 11 'FSA Phase III
    Public Const ACDSTimesPrinted As Integer = 12 'AR20050304 - PN15228
    Public Const ACDSTimesArchived As Integer = 13
    Public Const ACDSCreatedBy As Integer = 14 'AR20050304 - PN15228
    Public Const ACDSDateCreated As Integer = 15 'AR20050304 - PN15228
    Public Const ACDSDocumentTemplateID As Integer = 16 'PN:73866

    Public Const ACDSDocumentTemplateGroupID As Integer = 17 'PN:73866
    Public Const ACDSDocumentTemplateSubGroupID As Integer = 18 'PN:73866
    Public Const ACDSInternalOnly As Integer = 19 'PN:73866


    Public Const ACDSArraySize As Integer = 19

    ' Constants for the search data array indexes.
    Public Const ACDDocumentSpoolerId As Integer = 0
    Public Const ACDDocumentTypeId As Integer = 1
    Public Const ACDDocumentType As Integer = 2
    Public Const ACDPartyCnt As Integer = 3
    Public Const ACDShortName As Integer = 4
    Public Const ACDInsuranceFolderCnt As Integer = 5
    ' Broking fields
    Public Const ACDInsuranceFolderDesc As Integer = 6
    Public Const ACDInsuranceFileCntBR As Integer = 7
    ' Underwriting fields
    Public Const ACDInsuranceFileCntUW As Integer = 6
    Public Const ACDInsuranceReferenceUW As Integer = 7
    Public Const ACDClaimCnt As Integer = 8
    Public Const ACDClaim As Integer = 9
    Public Const ACDDescription As Integer = 10
    Public Const ACDIsDeletable As Integer = 11
    Public Const ACDIsEditable As Integer = 12
    Public Const ACDCreatedById As Integer = 13
    Public Const ACDCreatedBy As Integer = 14
    Public Const ACDDateCreated As Integer = 15
    Public Const ACDModifiedById As Integer = 16
    Public Const ACDModifiedBy As Integer = 17
    Public Const ACDDateModified As Integer = 18
    Public Const ACDTimesPrinted As Integer = 19
    Public Const ACDTimesArchived As Integer = 20
    Public Const ACDDocumentTypeCode As Integer = 21
    Public Const ACDInsuranceReferenceBR As Integer = 21
    Public Const ACDDocumentPrinter As Integer = 23
    Public Const ACDSpoolLevelInd As Integer = 24
    Public Const ACDTemplateCode As Integer = 25
    Public Const ACDAccountHandler As Integer = 26
    Public Const ACDLeadAgent As Integer = 22
    Public Const ACDProductionOrder As Integer = 27
    Public Const ACDDocumentTemplateID As Integer = 28 'PN:73866

    Public Const ACDDDocumentTemplateGroupID As Integer = 29 'PN:73866
    Public Const ACDDDocumentTemplateSubGroupID As Integer = 30 'PN:73866
    Public Const ACDDInternalOnly As Integer = 31 'PN:73866

    ' Public contants used for the start and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    Public Const ACMinSearchLength As Integer = 1

    'Constants for Date and Date Sort Column
    Public Const ACDateColumn1 As Integer = 6
    Public Const ACDateColumn2 As Integer = 7

    ' Public source and language ID's from the Object Manager.
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()>
    Public g_oBusiness As Object
    'developer guide no. 107
    <ThreadStatic()>
    Public g_oZipper As Object
End Module
