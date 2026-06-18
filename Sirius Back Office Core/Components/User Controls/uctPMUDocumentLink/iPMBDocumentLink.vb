Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
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
    Public Const ACApp As String = "uctPMUDocumentLink"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102

    Public Const ACPolicy As Integer = 103
    Public Const ACClaim As Integer = 104

    Public Const ACListTitle1 As Integer = 105
    Public Const ACListTitle2 As Integer = 106
    Public Const ACListTitle3 As Integer = 107
    Public Const ACListTitle4 As Integer = 108
    Public Const ACListTitle5 As Integer = 109
    'sj 27/09/2002 - start
    Public Const ACListTitle6 As Integer = 114
    'sj 27/09/2002 - end

    Public Const ACEventType As Integer = 110
    Public Const ACUserName As Integer = 111
    Public Const ACFromDate As Integer = 112
    Public Const ACToDate As Integer = 113
    Public Const ACCaseNumber As Integer = 114
    Public Const ACListTitleCase As Integer = 115

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACNewButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACFindNowButton As Integer = 206
    Public Const ACNewSearchButton As Integer = 207
    Public Const ACRefreshButton As Integer = 208 ' RAM20040219 : PN Issue10473

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307
    Public Const ACRemoveEntry As Integer = 308
    Public Const ACColOrder As Integer = 309
    Public Const ACColProcess As Integer = 310
    Public Const ACColBranch As Integer = 311
    Public Const ACColDocumentCode As Integer = 312
    Public Const ACColDescription As Integer = 313
    Public Const ACColClient As Integer = 314
    Public Const ACColAgent As Integer = 315
    Public Const ACColOffice As Integer = 316
    Public Const ACColSendTo As Integer = 317
    Public Const ACDocFound As Integer = 318




    ' Menus


    ' Constants for the search data array indexes.
    Public Const ACIEventCnt As Integer = 0
    Public Const ACIInsuranceFolderCnt As Integer = 1
    Public Const ACIInsuranceFolderDesc As Integer = 2
    Public Const ACIInsuranceFileCnt As Integer = 3
    Public Const ACIInsuranceFileStructureId As Integer = 4
    Public Const ACIClaimCnt As Integer = 5
    Public Const ACIClaimDesc As Integer = 6
    Public Const ACIDocumentCnt As Integer = 7
    Public Const ACIDocumentDesc As Integer = 8
    Public Const ACIOldAddressCnt As Integer = 9
    Public Const ACINewAddressCnt As Integer = 10
    Public Const ACICampaignDesc As Integer = 11
    Public Const ACIReportDesc As Integer = 12
    Public Const ACIEventType As Integer = 13
    Public Const ACIUserName As Integer = 14
    Public Const ACIDate As Integer = 15
    Public Const ACIDescription As Integer = 16
    Public Const ACIOldPartyType As Integer = 17
    Public Const ACIReason As Integer = 18
    Public Const ACIDocumentRef As Integer = 19
    Public Const ACIEventLogSubject As Integer = 20
    Public Const ACIEventTypeGroupId As Integer = 21
    Public Const ACIEventTypeGroupDescription As Integer = 22
    Public Const ACIEventLogSubjectId As Integer = 23
    Public Const ACIFSAComplaintFolderCnt As Integer = 24
    Public Const ACIAlternateReference As Integer = 25
    Public Const ACISourceId As Integer = 26
    Public Const ACIPolicyTypeId As Integer = 27
    Public Const ACIUnderwritingBranchInd As Integer = 28
    Public Const ACIHasNotes As Integer = 29
    Public Const ACIPriorityCode As Integer = 30 '2005 StickyNotes
    Public Const ACIIsCompleted As Integer = 31 '2005 StickyNotes
    Public Const ACIRTFNotes As Integer = 32
    Public Const ACICaseNumber As Integer = 33


    ' {* USER DEFINED CODE (End) *}

    'sj 27/09/2002 - start
    Public Const ACGDescription As Integer = 0
    Public Const ACGEventTypeGroupId As Integer = 1
    Public Const ACGCode As Integer = 2

    Public Const ACColHeadEventDate As Integer = 1
    Public Const ACColHeadType As Integer = 2
    Public Const ACColHeadPolicy As Integer = 3
    Public Const ACColHeadClaim As Integer = 4
    Public Const ACColHeadDescription As Integer = 5
    Public Const ACColHeadUser As Integer = 6
    Public Const ACColHeadPriorityCode As Integer = 7 '2005 StickyNotes
    Public Const ACColHeadIsCompleted As Integer = 8 '2005 StickyNotes
    'sj 27/09/2002 - end
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



    'Constants for Date and Date Sort Column
    Public Const ACDateColumn As Integer = 0
    Public Const ACDateSortColumn As Integer = 4

    Public Const ACPMBDocLinkId As Integer = 0
    Public Const ACPMBGISSchemeId As Integer = 1
    Public Const ACPMBProcessTypeId As Integer = 2
    Public Const ACPMBDocumentTypeId As Integer = 3
    Public Const ACPMBDocumentTemplateId As Integer = 4
    Public Const ACPMBSpoolDocument As Integer = 5
    Public Const ACPMBProcessTypesDocsId As Integer = 6
    Public Const ACPMBFunctionalArea As Integer = 7
    Public Const ACPMBProductId As Integer = 8
    Public Const ACPMBSourceId As Integer = 9
    Public Const ACPMBIsClient As Integer = 10
    Public Const ACPMBIsAgent As Integer = 11
    Public Const ACPMBIsOffice As Integer = 12
    Public Const ACPMBProductionOrder As Integer = 13
    Public Const ACPMBProcessTypeCode As Integer = 14
    Public Const ACPMBSourceDescription As Integer = 15
    Public Const ACPMBProcessTypeDocDescription As Integer = 16
    Public Const ACPMBDocumentTypeTemplateDescription As Integer = 17
    Public Const ACPMBDocumentTypeTemplateBO As Integer = 18
    Public Const ACPMBDocumentTypeTemplateSAM As Integer = 19

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    ' Public instance of the business object.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oBusiness As bPMBDocLink.Business

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenHelpID As Integer = 10

    Sub Main_Renamed()

    End Sub
End Module