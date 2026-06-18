Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Word = Microsoft.Office.Interop.Word
Imports SharedFiles
Imports System.Collections.Generic

Friend NotInheritable Class iPMBDocTemplateFormless
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: iPMBDocTemplateFormless
    '
    ' Date: 10/05/1999
    '
    ' Description: iPMBDocTemplateFormless class to accompany the interface form.
    '
    ' Edit History:
    ' CJB 23/11/2005 PN25924 Changed GetInterfaceDetails to improve error recovery.
    ' ***************************************************************** '


    Dim bConfirmConversions As Boolean
    Dim g_oFieldManager As Object

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "iPMBDocTemplateFormless"

    Private Const vbFormCode As Integer = 0
    Private Const WMInstance As Integer = 0
    Private Const WMTemplate As Integer = 1

    ' Lookup detail contants.
    Private Const ACDetailKey As Integer = 0
    Private Const ACDetailDesc As Integer = 1
    Private Const ACDetailCode As Integer = 2 'PN22739

    ' Doc Type Code for Clauses                PN22739
    Private Const ACDocTypeClauses As String = "CLAUSES"
    Private Const ACDocTypeStandardLetter As String = "LETTER"
    Private Const ACDocTypeDebitNote As String = "DEBIT"


    'DJM 19/02/2003 PN10480 : Copied fix from 1.8.5 issue 4697.
    Private Const ACColTaskInstanceCnt As Integer = 0
    Private Const ACColTaskGroupCode As Integer = 1
    Private Const ACColTaskGroupID As Integer = 2
    Private Const ACColTaskCode As Integer = 3
    Private Const ACColTaskID As Integer = 4
    Private Const ACColTaskDescription As Integer = 5
    Private Const ACColTaskCustomer As Integer = 6
    Private Const ACColTaskDueDate As Integer = 7
    Private Const ACColTaskIsUrgent As Integer = 8
    Private Const ACColUserGroupID As Integer = 9
    Private Const ACColUserID As Integer = 10
    Private Const ACColTaskCreatedDate As Integer = 11
    Private Const ACColTaskCreatedByID As Integer = 12
    Private Const ACColMaxColumns As Integer = 12

    'JAS 10012005 PN17860
    Private Const ACSFSTOBCode As String = "SFSTOB"

    'RWH(27/07/2000)
    Private m_sClientDocument As String = ""
    Dim m_oWord As Word.Application = Nothing
    'Private m_oWord As Object
    Private m_oDocument As Object
    Private m_oSharePoint As Object
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""
    Private m_lDocumentTemplateId As Integer
    Private m_sDocumentTemplateCode As String = ""
    Private m_sDocumentTemplateDescription As String = ""
    Private m_lDocumentTypeId As Integer
    Private m_lOldDocumentTemplateId As Integer
    Private m_lOldDocumentTypeId As Integer
    Private m_sDocumentTypeCode As String = ""
    Private m_lSourceId As Integer
    Private m_sDocumentTypeDescription As String = ""
    Private m_iIsDeleted As Integer
    Private m_vSlotNumber As Integer
    Private m_vRiskCodeId As Integer
    Private m_vRiskGroupId As Integer
    Private m_iIsEditableAfterMerging As Integer
    Private m_sDocFileExtension As String = ""
    Private m_lMode As Integer
    Private m_bFromBatchTrans As Boolean
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lClaimCnt As Integer
    Private m_sDocumentRef As String = ""
    Private m_lEventCnt As Integer
    Private m_lWrkTaskCnt As Integer
    Private m_lOldWrkTaskCnt As Integer
    Private m_sDocumentFilter As String = ""
    Private m_lFSAComplaintFolderCnt As Integer
    Private m_sFSAComplaintReference As String = ""
    Private m_sPartyName As String = ""
    Private m_sInsuranceFileRef As String = ""
    Private m_sClaimRef As String = ""
    Private m_lPMWrkTaskTemplate As Integer
    Private m_vPartyPolicy() As Object
    Private m_sClient As String = ""
    Private m_sServer As String = ""
    Private m_sOK As String = ""
    Private m_sDelete As String = ""
    Private m_sUndelete As String = ""
    Private m_sWordVersion As String = ""
    Private m_bSetUp As Boolean
    Private m_vClientArray() As Object
    Private m_vPolicyArray() As Object
    Private m_vClaimArray() As Object 'MKW020503 PN3890
    Private m_vDocumentTypeArray(,) As Object
    Private m_vSourceArray(,) As Object
    Private m_vRiskBySource(,) As Object
    Private m_vRiskGroupBySource(,) As Object
    Private m_vRiskClauseLinkArray(,) As Object 'RWH(21/08/2000) RSAIB Process 12
    Private m_vDocClauseLinkArray() As Object 'RWH(21/08/2000) RSAIB Process 12

    'RWH(22/08/2000) Merge Field markers (These are inserted in Word
    'as "<@" and "@>" but when viewed as flat text appear as
    ' "&lt;@" and "@&gt;" respectively).
    Private m_sFieldStartMarker As String = ""
    Private m_sFieldEndMarker As String = ""
    Private m_iFieldMarkerLength As Integer
    Private m_sZIP_DIRECTORY As String = ""
    Private m_sDrive As String = ""
    Private m_bSpoolMessage As Boolean
    Private m_bArchiveMessage As Boolean 'DN 04/05/01
    Private m_lSpoolNumber As Integer

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBDocTemplate.General
    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_oSIRDOCAPI As bSIRDOCAPI.Form

    Private m_oDocSpooler As bSIRDocSpooler.Business
    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object
    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
    Private m_sUnderwritingOrAgency As String = ""
    'TN20010711 - (our word handle)
    Private m_lWordHwnd As Integer
    'RWH(26/07/01) Printers.
    Private m_vPrinters() As Object
    Private m_sDefaultPrinter As String = ""
    Private m_sSelectedPrinter As String = ""
    'AK 090402
    Private m_vChasers(,) As Object
    Private m_sSelectedChaser As String = ""
    Private m_bChaserLettersEnabled As Boolean
    ' FishRequired
    Private m_bFishRequired As Boolean
    Private m_bSpoolAsHtml As Boolean
    Private m_bFishSent As Boolean
    'DC170203 -ISS1405 -For processing of shared invoices
    Private m_vPolicySharesArray As Object
    'MKW030903 (TR - 12/05/03 - TS17 Recovery By Instalments changes)
    Private m_bArchiveAfterPrinting As Boolean
    Private m_sDocumentDescription As String = ""
    ' Stores the details from the business object.
    Private m_bUniqueClientDirNeedsDeleting As Boolean 'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup
    Private m_sFileCopyMsg As String = ""

    'DC260104 PN9904 added to fix problem editting uniplex document
    Private m_bCreatedWord As Boolean

    Private WithEvents Hdoc As mshtml.HTMLDocument 'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup

    'DJM 19/02/2003 PN10480 : Copied fix from 1.8.5 issue 4697.
    Private m_bUseSuppliedTaskTemplateDetails As Boolean
    Private m_bDoNotSaveTaskTemplateToDB As Boolean
    Private m_vTaskTemplateDetailsArray() As Object
    Private m_bDeleteTaskTemplate As Boolean
    Private m_bAssociatedTaskTemplateMissing As Boolean

    Private m_bNavigateButton As Boolean
    Private m_bPrintButton As Boolean
    Private m_bTaskButton As Boolean
    Private m_bRemoveTaskButton As Boolean
    Private m_bOkButton As Boolean
    Private m_bCancelButton As Boolean
    Private m_bHelpButton As Boolean
    Private m_bEditButton As Boolean
    Private m_bToolbar As Boolean

    ' CJB 180804 PN14209
    Private m_bBypassDocDelete As Integer

    Private m_oDocManager As iPMBDocManager.Interface_Renamed

    Private m_sParameterXML As String = ""
    Private m_bCalledFromSwift As Boolean ' RAM20050201 - Added to support Swift
    Private m_bEditted As Boolean

    ' RDC 27/09/2005
    Private m_bAutoArchiveEnabled As Boolean
    Private m_bEmailDocsAsPDF As Boolean
    Private m_bArchiveAsPDF As Boolean
    'Plico21
    Private m_dtDocEffectiveDate As Date

    Private m_lRiskCnt As Integer
    Private m_bVisibleFromWeb As Boolean 'AR - NEXUS MTA

    'Renewal Printing
    Private m_iIsClient As Integer
    Private m_iIsAgent As Integer
    Private m_iIsOffice As Integer
    Private m_iProductionOrder As Integer


    Private m_bArchiveWithNoPrint As Boolean
    Private m_bEmailAsBody As Boolean
    Private m_bSpoolDocument As Boolean
    Private m_bArchiveAsText As Boolean

    Private m_bVisibleFromClientManager As Boolean
    Private m_vCaseClaimLinkArray As Object
    Private m_bCalledFromGetDocument As Boolean

    Private m_lTemplateGroupID As Integer
    Private m_lTemplateSubGroupID As Integer
    Private m_bIsInternalOnly As Boolean
    Private m_bIsSelectedByDefault As Boolean
    Private m_sClientConvertedHTMDocument As String
    Private m_bArchiveAsXML As Boolean
    Private WebBrowser1 As System.Windows.Forms.WebBrowser

    Private m_bIsCCMDocProduction As Boolean = False
    Private m_sCCMCustomer As String
    Private m_sCCMPartner As String
    Private m_sCCMContractTypeName As String
    Private m_sCCMRepositoryProject As String
    Private m_sCCMContractTypeVersion As String
    Private m_sCCMDocumentName As String
    Private m_sCCMRepStatus As String
    Private m_sCCMWebServiceURL As String
    Private m_bIsDocumentEdited As Boolean
    Private m_bIsNonBatchProcess As Boolean
    Private m_bFormlessDocument As Boolean = False

    Public Property bFormlessDocument() As Boolean
        Get
            Return m_bFormlessDocument
        End Get
        Set(ByVal Value As Boolean)
            m_bFormlessDocument = Value
        End Set
    End Property

    Public WriteOnly Property IsNonBatchProcess() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsNonBatchProcess = Value

        End Set
    End Property

    Public Property CalledFromGetDocument() As Boolean
        Get
            Return m_bCalledFromGetDocument
        End Get
        Set(ByVal Value As Boolean)
            m_bCalledFromGetDocument = Value
        End Set
    End Property

    'sj 23/10/2002 - start
    Public WriteOnly Property FishRequired() As Boolean
        Set(ByVal Value As Boolean)
            m_bFishRequired = Value
        End Set
    End Property
    Public WriteOnly Property SpoolAsHtml() As Boolean
        Set(ByVal Value As Boolean)
            m_bSpoolAsHtml = Value
        End Set
    End Property
    'sj 23/10/2002 - end

    'MKW030903 START
    'TR - 12/05/03 - TS17 Recovery By Instalments changes
    'TR - Property Let to Force Archiving of Document after Printing, to Documaster
    Public WriteOnly Property ArchiveAfterPrinting() As Boolean
        Set(ByVal Value As Boolean)
            m_bArchiveAfterPrinting = Value
        End Set
    End Property
    Public WriteOnly Property DocumentDescription() As String
        Set(ByVal Value As String)
            m_sDocumentDescription = Value
        End Set
    End Property
    Public Property PMWrkTaskTemplate() As Integer
        Get
            Return m_lPMWrkTaskTemplate
        End Get
        Set(ByVal Value As Integer)
            m_lPMWrkTaskTemplate = Value
        End Set
    End Property

    Public Property WordVersion() As String
        Get
            Return m_sWordVersion
        End Get
        Set(ByVal Value As String)
            m_sWordVersion = Value
        End Set
    End Property

    Public Property FieldEndMarker() As String
        Get
            Return m_sFieldEndMarker
        End Get
        Set(ByVal Value As String)
            m_sFieldEndMarker = Value.Trim()
        End Set
    End Property

    Public Property DocFileExtension() As String
        Get
            Return m_sDocFileExtension
        End Get
        Set(ByVal Value As String)
            m_sDocFileExtension = Value.Trim()
        End Set
    End Property

    Public Property FieldStartMarker() As String
        Get
            Return m_sFieldStartMarker
        End Get
        Set(ByVal Value As String)
            m_sFieldStartMarker = Value.Trim()
            m_iFieldMarkerLength = m_sFieldStartMarker.Length
        End Set
    End Property

    Public Property DocumentTemplateId() As Integer
        Get
            Return m_lDocumentTemplateId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTemplateId = Value
        End Set
    End Property

    Public Property DocumentTemplateCode() As String
        Get
            Return m_sDocumentTemplateCode
        End Get
        Set(ByVal Value As String)
            m_sDocumentTemplateCode = Value
        End Set
    End Property

    Public Property DocumentTemplateDescription() As String
        Get
            Return m_sDocumentTemplateDescription
        End Get
        Set(ByVal Value As String)
            m_sDocumentTemplateDescription = Value
        End Set
    End Property


    Public Property DocumentFilter() As String
        Get
            Return m_sDocumentFilter
        End Get
        Set(ByVal Value As String)
            m_sDocumentFilter = Value
        End Set
    End Property

    Public Property DocumentTypeId() As Integer
        Get
            Return m_lDocumentTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTypeId = Value
        End Set
    End Property

    Public Property DocumentTypeCode() As String
        Get
            Return m_sDocumentTypeCode
        End Get
        Set(ByVal Value As String)
            m_sDocumentTypeCode = Value
        End Set
    End Property

    Public Property DocumentTypeDescription() As String
        Get
            Return m_sDocumentTypeDescription
        End Get
        Set(ByVal Value As String)
            m_sDocumentTypeDescription = Value
        End Set
    End Property
    Public Property Mode() As Integer
        Get
            Return m_lMode
        End Get
        Set(ByVal Value As Integer)
            m_lMode = Value
        End Set
    End Property
    'eck050402
    Public Property FromBatchTrans() As Boolean
        Get
            Return m_bFromBatchTrans
        End Get
        Set(ByVal Value As Boolean)
            m_bFromBatchTrans = Value
        End Set
    End Property
    'eck050402 - end
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property ClaimCnt() As Integer
        Get
            Return m_lClaimCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property

    Public Property DocumentRef() As String
        Get
            Return m_sDocumentRef
        End Get
        Set(ByVal Value As String)
            m_sDocumentRef = Value
        End Set
    End Property
    Public Property EventCnt() As Integer
        Get
            Return m_lEventCnt
        End Get
        Set(ByVal Value As Integer)
            m_lEventCnt = Value
        End Set
    End Property
    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus
        End Get
    End Property
    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    ' We have a let status here, so if we're printing that we can set the status to PMOK
    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property
    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property
    'DC170203 -ISS1405 -start -for processing of shared invoices
    Public Property PolicySharesArray() As Object
        Get
            Return m_vPolicySharesArray
        End Get
        Set(ByVal Value As Object)


            m_vPolicySharesArray = Value
        End Set
    End Property
    'FSA Phase 3.2
    Public Property FSAComplaintFolderCnt() As Integer
        Get
            Return m_lFSAComplaintFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lFSAComplaintFolderCnt = Value
        End Set
    End Property
    Public Property FSAComplaintReference() As String
        Get
            Return m_sFSAComplaintReference
        End Get
        Set(ByVal Value As String)
            m_sFSAComplaintReference = Value
        End Set
    End Property

    'Renewal Printing
    Public Property IsClient() As Integer
        Get
            Return m_iIsClient
        End Get
        Set(ByVal Value As Integer)
            m_iIsClient = Value
        End Set
    End Property

    Public Property IsAgent() As Integer
        Get
            Return m_iIsAgent
        End Get
        Set(ByVal Value As Integer)
            m_iIsAgent = Value
        End Set
    End Property

    Public Property IsOffice() As Integer
        Get
            Return m_iIsOffice
        End Get
        Set(ByVal Value As Integer)
            m_iIsOffice = Value
        End Set
    End Property

    Public Property ProductionOrder() As Integer
        Get
            Return m_iProductionOrder
        End Get
        Set(ByVal Value As Integer)
            m_iProductionOrder = Value
        End Set
    End Property


    'FSA Phase 3.2 End

    ' RAM20050117 - allow a parameter xml to be passed in
    Public WriteOnly Property ParameterXML() As String
        Set(ByVal Value As String)
            m_sParameterXML = Value
        End Set
    End Property

    Public WriteOnly Property CalledFromSwift() As Boolean
        Set(ByVal Value As Boolean)
            m_bCalledFromSwift = Value
        End Set
    End Property

    ' RDC 27/09/2005
    Public WriteOnly Property AutoArchiveEnabled() As Boolean
        Set(ByVal Value As Boolean)
            Dim sOptionValue As String = ""
            m_bAutoArchiveEnabled = Value

            m_lReturn = m_oBusiness.getOption(v_iOptionNumber:=10, r_sOptionValue:=sOptionValue)

            If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And (sOptionValue.Trim() = "0") Then
                m_bAutoArchiveEnabled = False
            End If
        End Set
    End Property

    Public WriteOnly Property EmailDocsAsPDF() As Boolean
        Set(ByVal Value As Boolean)
            m_bEmailDocsAsPDF = Value
        End Set
    End Property

    Public WriteOnly Property ArchiveAsPDF() As Boolean
        Set(ByVal Value As Boolean)
            m_bArchiveAsPDF = Value
        End Set
    End Property


    Public Property ArchiveAsText() As Boolean
        Get
            Return m_bArchiveAsText
        End Get
        Set(ByVal Value As Boolean)
            m_bArchiveAsText = Value
        End Set
    End Property
    Public Property ArchiveAsXML() As Boolean
        Get
            Return m_bArchiveAsXML
        End Get
        Set(ByVal Value As Boolean)
            m_bArchiveAsXML = Value
        End Set
    End Property


    Public Property DocEffectiveDate() As Date
        Get
            EffectiveDate = m_dtDocEffectiveDate
        End Get
        Set(ByVal Value As Date)

            m_dtDocEffectiveDate = CDate(Value)
        End Set
    End Property


    Public Property RiskCnt() As Integer
        Get
            Return m_lRiskCnt
        End Get
        Set(ByVal Value As Integer)
            m_lRiskCnt = Value
        End Set
    End Property

    Public Property IsCCMDocProduction As Boolean
        Get
            Return m_bIsCCMDocProduction
        End Get
        Set(ByVal Value As Boolean)
            m_bIsCCMDocProduction = Value
        End Set
    End Property

    Public Property CCMCustomer As String
        Get
            Return m_sCCMCustomer
        End Get
        Set(ByVal Value As String)
            m_sCCMCustomer = Value
        End Set
    End Property

    Public Property CCMPartner As String
        Get
            Return m_sCCMPartner
        End Get
        Set(ByVal Value As String)
            m_sCCMPartner = Value
        End Set
    End Property

    Public Property CCMContractTypeName As String
        Get
            Return m_sCCMContractTypeName
        End Get
        Set(ByVal Value As String)
            m_sCCMContractTypeName = Value
        End Set
    End Property

    Public Property CCMRepositoryProject As String
        Get
            Return m_sCCMRepositoryProject
        End Get
        Set(ByVal Value As String)
            m_sCCMRepositoryProject = Value
        End Set
    End Property

    Public Property CCMContractTypeVersion As String
        Get
            Return m_sCCMContractTypeVersion
        End Get
        Set(ByVal Value As String)
            m_sCCMContractTypeVersion = Value
        End Set
    End Property

    Public Property CCMDocumentName As String
        Get
            Return m_sCCMDocumentName
        End Get
        Set(ByVal Value As String)
            m_sCCMDocumentName = Value
        End Set
    End Property

    Public Property CCMWebServiceURL As String
        Get
            Return m_sCCMWebServiceURL
        End Get
        Set(ByVal Value As String)
            m_sCCMWebServiceURL = Value
        End Set
    End Property

    Public Property CCMRepStatus As String
        Get
            Return m_sCCMRepStatus
        End Get
        Set(ByVal Value As String)
            m_sCCMRepStatus = Value
        End Set
    End Property

    Public Property IsDocumentEdited() As Boolean
        Get
            Return m_bIsDocumentEdited
        End Get
        Set(ByVal Value As Boolean)
            m_bIsDocumentEdited = Value
        End Set
    End Property

    Public Sub New()

        Initialise()

    End Sub


    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '	

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            m_lReturn = m_oBusiness.GetDetails(vLockMode:=gPMConstants.PMELockMode.PMNoLock, vDocumentTemplateID:=m_lDocumentTemplateId)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            'DC290501 -start -retreive more information
            'KB 07012003 pass the insurance file cnt through as well
            'this will allow us to find the insurance_folder_cnt
            'we need this if we are archiving
            'at policy level but from a task rather than the policy itself

            m_lReturn = m_oBusiness.GetInformation(lPartyCnt:=m_lPartyCnt, lInsuranceFolderCnt:=m_lInsuranceFolderCnt, lClaimCnt:=m_lClaimCnt, sPartyName:=m_sPartyName, sInsuranceFileRef:=m_sInsuranceFileRef, sClaimRef:=m_sClaimRef, lInsuranceFileCnt:=m_lInsuranceFileCnt)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            'DC290501 -end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '


    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '


    'DC260104 PN9904 added to fix problem editting uniplex document
    ' ***************************************************************** '
    ' Name: CopyServerToClient
    '
    ' Description: copies the template from the server to the client
    '
    ' Edit History :
    ' RAM20040205  : Bug fix for PN Issue 10231.
    '                Notes : Removed unwanted Dir Commands as it locks the directory
    ' ***************************************************************** '
    Public Function CopyServerToClient() As Integer

        Dim result As Integer = 0
        Dim sServer, sClient, sTemp, sMessage As String
        Dim oTemplate As Object
        Dim sAsposeConvert As String = ""

        Const VB_FileAccessError As Integer = 75

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040204 : Bug fix for PN Issue 10231.
            '               1. Removed EnsureClientDirectoryClear Function Call
            '                   since every time, the user will be provided a
            '                   temporary directory with random name, so always the
            '                   directory will be clear.
            '               2. Removed unnecessary DIR Command as it locks the directory
            '               3. So use the File System Objects (FSO)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Setup Zip Directory
            m_lReturn = SetZipDirectory()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to setup the default ZIP Directory.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Create Client Work Folder
            m_lReturn = CreateFolderTree(m_sClient, True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create Client Work Directory. (" & m_sClient & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'If we're adding we want a new one...
            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                If m_sDocFileExtension.ToUpper() = "XML" Then
                    sServer = m_sServer & "\blankXML.zip"
                Else
                    sServer = m_sServer & "\blank.zip"
                End If
                m_lDocumentTemplateId = 0
                m_lDocumentTypeId = 0
            Else
                sServer = m_sServer & "\Type " & CStr(m_lDocumentTypeId) & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip"
                If m_lDocumentTypeId = lLETTER_TYPE_ID And FileSystem.Dir(sServer, FileAttribute.Normal) = "" Then
                    'Search in subdoc folder
                    If FileSystem.Dir(m_sServer & "\Type " & CStr(lSUBDOC_TYPE_ID) & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip", FileAttribute.Normal) <> "" Then
                        sServer = m_sServer & "\Type " & CStr(lSUBDOC_TYPE_ID) & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip"
                    End If
                End If
            End If

            sClient = m_sZIP_DIRECTORY & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip"

            'RWH(26/07/2000) Change to .htm file.
            sTemp = m_sZIP_DIRECTORY & "\Doc " & CStr(m_lDocumentTemplateId) & "." & m_sDocFileExtension

            'Make sure the file's not there
            m_lReturn = bPMDocFunctions.DeleteFile(sTemp) ' RAM20040209 : Bug fix for PN Issue 10231
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete file [" & sTemp & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_sFileCopyMsg = ""
            'DC250304 PN11138 do not remove blank.zip file so do not delete sourcefile
            m_lReturn = bPMDocFunctions.CopyFile(sServer, sClient, True, False, m_sFileCopyMsg)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy template from Server To Client." & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Source File      : " & sServer & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Destination File : " & sClient & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Error Details    : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            ' Use the bPMDocFunctions UnZip Function.
            m_lReturn = UnZip(sClient, m_sClient, True) ' m_sZIP_DIRECTORY is a placeholder for Zip File ONLY
            ' m_sClient is the Unique Working Folder for Client for that document
            ' Also, UnZip directly unzip to that working folder
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Unzip the Template." & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Template        : " & sClient & Strings.Chr(13) & Strings.Chr(10) & _
                                   "UnZip Directory : " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=0, vErrDesc:="Failed to UnZip the template.")
                Return result
            End If

            'DC
            'DJM 02/09/2002 : Convert files in the zip directory not in the client directory
            'DN 12/07/02 - Convert document if not HTML

            If CheckFileTypeIsDoc() = gPMConstants.PMEReturnCode.PMTrue Then

                'Need to convert to specific format (regardless of Aspose Product Option) as is need later.

                m_lReturn = CheckFileHasCorrectName(m_sClient, m_lDocumentTemplateId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    oTemplate = Nothing
                    ' SET 18/10/2004 ISS13245
                    m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                    Return result
                End If

                m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".doc", m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".htm")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to convert file", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'DJM 02/09/2002 : Remove old word document.
                sTemp = m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".doc"

                ' RAM20040209 : Removed unwanted Dir Command, PN Issue 10231
                m_lReturn = bPMDocFunctions.DeleteFile(sTemp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete File [" & sTemp & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End If 'Set this so we know if it's moved

            If m_sDocFileExtension.ToUpper() = "HTM" Then
                m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)
                'UPGRADE_TODO: (1067) Member Documents is not defined in type Object. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                oTemplate = m_oWord.Documents.Open(m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".xml")
                'UPGRADE_TODO: (1067) Member SaveAs is not defined in type Object. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                oTemplate.SaveAs(m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".htm", 8)
                'UPGRADE_TODO: (1067) Member Close is not defined in type Object. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                oTemplate.Close()

                oTemplate = Nothing
                ' SET 18/10/2004 ISS13245
                m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

                'DJM 02/09/2002 : Remove old word document.
                sTemp = m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".xml"

                ' RAM20040209 : Removed unwanted Dir Command, PN Issue 10231
                m_lReturn = bPMDocFunctions.DeleteFile(sTemp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete File [" & sTemp & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If

            m_lOldDocumentTypeId = m_lDocumentTypeId
            m_lOldDocumentTemplateId = m_lDocumentTemplateId

            'DC050603 -ISS4264 -added from 1.6.9
            'DJM 27/05/2003 : Remove old word document.
            sTemp = m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".doc"
            m_lReturn = bPMDocFunctions.DeleteFile(sTemp) ' RAM20040209 : Bug fix for PN Issue 10231
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete File [" & sTemp & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            If Information.Err().Number = VB_FileAccessError Then
                sMessage = "User does not have access to Document server: '" & m_sServer & "'"
            Else
                sMessage = "Failed to copy template from server to client"
            End If

            If oTemplate Is Nothing Then

            Else
                oTemplate = Nothing
            End If
            ' SET 18/10/2004 ISS13245
            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    'DC260104 PN9904 added to fix problem editting uniplex document
    ' ***************************************************************** '
    '
    ' Name: CheckFileType
    '
    ' Description: Opens file downloaded from server & checks first 2
    '               bytes to ensure this is a standard .doc document
    '               This is to see whether it is necessary to carry
    '               out a conversion.
    '
    ' Edit History  :
    ' 29/08/2000 RWH - Created.
    ' RAM20040206    - Removed unwanted Dir Commands, Since it lock the directory.
    '                  Ref. PN Issue 10231
    ' ***************************************************************** '
    Private Function CheckFileTypeIsDoc() As Integer
        Dim result As Integer = 0
        Dim sFile As String = ""
        Dim lFileCount, lFileNum As Integer
        Dim sLine As String = ""



        result = gPMConstants.PMEReturnCode.PMFalse

        'DJM 02/09/2002 : Convert files in the zip directory not in the client directory
        'RAM20040205    : Use FSO rather than the Dir Commands
        '                 Use m_sClient instead of m_sZIP_DIRECTORY, since that is the working directory
        lFileCount = NoOfFilesInDirectory(v_sDirectoryName:=m_sClient, r_vFirstFileName:=sFile)


        Select Case lFileCount
            Case Is > 1
                ' Too many files
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Too Many Files in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            Case 0
                ' No Files Found
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            Case 1
                ' Only one file found, so check the type of the file

                lFileNum = FileSystem.FreeFile()
                FileSystem.FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)
                sLine = FileSystem.InputString(lFileNum, 5)
                FileSystem.FileClose(lFileNum)

                Select Case sLine.ToUpper()
                    Case "<?XML", "<HTML"
                        'Do Nothing
                    Case Else
                        If sLine.StartsWith("﻿") Then
                            'File is UTF-8 Encoded.
                            FileSystem.FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)
                            sLine = FileSystem.InputString(lFileNum, 8)
                            FileSystem.FileClose(lFileNum)
                            If Mid(sLine, 4, 5).ToUpper() = "<?XML" Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                            ElseIf Mid(sLine, 4, 5).ToUpper() = "<HTML" Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                            Else
                                result = gPMConstants.PMEReturnCode.PMTrue
                            End If
                        Else
                            result = gPMConstants.PMEReturnCode.PMTrue
                        End If
                End Select

            Case Else
                ' Some other no, so error

                ' No Files Found
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
        End Select

        Return result

    End Function

    'DC260104 PN9904 added to fix problem editting uniplex document
    ' ***************************************************************** '
    '
    ' Name: CheckFileHasCorrectName
    '
    ' Description: Templates were formerly created with 'Doc 0' name
    '               and not converted to the correct number until
    '               they were first edited or merged. We need to check
    '               that the file we have unzipped has the correctname.
    '               If not, then we must convert it.
    '
    ' Edit History:
    ' 04/09/2000 RWH - Created.
    ' RAM20040206    - Removed unwanted Dir Command, since it locks the directory.
    '                  Ref. PN Issue 10231
    ' ***************************************************************** '
    Private Function CheckFileHasCorrectName(ByRef sPath As String, ByVal lCorrectFileNumber As Integer) As Integer
        Dim result As Integer = 0
        Dim sFileName, sCorrectedFile As String
        Dim lFilesCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get current file name.
        lFilesCount = NoOfFilesInDirectory(v_sDirectoryName:=sPath, r_vFirstFileName:=sFileName)

        If sFileName <> "" Then
            sCorrectedFile = "Doc " & lCorrectFileNumber & ".doc"
            FileSystem.Rename(sPath & "\" & sFileName, sPath & "\" & sCorrectedFile)
        End If

        Return result

    End Function

    'DC260104 PN9904 added to fix problem editting uniplex document
    ' Open a document, resolve the fields, and return the
    ' document object
    'UPGRADE_NOTE: (7001) The following declaration (ConvertDocument) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Private Function ConvertDocument(ByRef oDocument As Object, ByVal lFileNo As Integer) As Integer

        Dim result As Integer = 0
        Dim oBookmark As Object
        Dim sFieldCode, sNewMergeField As String
        Dim iSep As Integer
        Dim sFieldType, sFieldName, sQuestion As String

        'DJM 30/08/2002 : When converting document, put markers in properly.
        Const c_sFieldStartMarker As String = "<@"
        Const c_sFieldEndMarker As String = "@>"

        Select Case Information.Err().Number
            Case Is < 0
                Conversion.ErrorToString(5)
            Case 1
                GoTo Err_ConvertDocument
        End Select

        result = gPMConstants.PMEReturnCode.PMTrue

        'SET 12/10/2004 ISS15027 - document is now in the client directory
        Dim sFileName As String = m_sClient & "\" & "Doc " & CStr(lFileNo) & ".doc"

        'Open the chosen template document
        'UPGRADE_TODO: (1067) Member Documents is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
        oDocument = m_oWord.Documents.Open(sFileName)

        'Get the active window
        'UPGRADE_TODO: (1067) Member ActiveWindow is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
        Dim oActiveWindow As Object = oDocument.ActiveWindow

        'Get the bookmarks collection
        'UPGRADE_TODO: (1067) Member bookmarks is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
        Dim oBookmarks As Object = oDocument.bookmarks

        'UPGRADE_TODO: (1067) Member Count is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
        If oBookmarks.Count = 0 Then
            oBookmarks = Nothing
            oActiveWindow = Nothing
            Return result
        End If

        'Reget the bookmarks collection
        'UPGRADE_TODO: (1067) Member bookmarks is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
        oBookmarks = oDocument.bookmarks

        'UPGRADE_TODO: (1067) Member Count is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
        If oBookmarks.Count = 0 Then
            oBookmarks = Nothing
            oActiveWindow = Nothing
            Return result
        End If

        'Load the bookmarks into an array
        For Each oBookmark2 As Object In oBookmarks
            oBookmark = oBookmark2

            'Get the field code for the bookmark
            'UPGRADE_TODO: (1067) Member Name is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            sFieldCode = oBookmark.Name

            'Determine the field type
            iSep = (sFieldCode.IndexOf("_"c) + 1)
            If iSep > 0 Then
                sFieldType = sFieldCode.Substring(0, iSep - 1)
            End If

            'Select the bookmark so it can be overwritten.
            'UPGRADE_TODO: (1067) Member Select is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            oBookmark.Select()


            Select Case sFieldType
                Case DbTag
                    'extract the field name
                    sFieldName = sFieldCode.Substring(sFieldCode.Length - (sFieldCode.Length - iSep))

                    'Strip off the file name at the beginning
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(iSep)
                    End If

                    'Strip off the id character at the end
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(0, iSep - 1)
                    End If

                    'Construct new merge field
                    sNewMergeField = c_sFieldStartMarker & DbTag & Separator & sFieldName & c_sFieldEndMarker

                    'UPGRADE_TODO: (1067) Member selection is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    oActiveWindow.selection = sNewMergeField

                Case QuestionTag

                    'UPGRADE_TODO: (1067) Member selection is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    sQuestion = oActiveWindow.selection

                    'Construct new merge field
                    sNewMergeField = c_sFieldStartMarker & QuestionTag & Separator & sQuestion & c_sFieldEndMarker

                    'UPGRADE_TODO: (1067) Member selection is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    oActiveWindow.selection = sNewMergeField

                Case LoopTag
                    'extract the field name
                    sFieldName = sFieldCode.Substring(sFieldCode.Length - (sFieldCode.Length - iSep))

                    'Strip off the file name at the beginning
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(iSep)
                    End If

                    'Strip off the id character at the end
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(0, iSep - 1)
                    End If

                    'Construct new merge field
                    sNewMergeField = c_sFieldStartMarker & LoopTag & Separator & sFieldName & c_sFieldEndMarker

                    'UPGRADE_TODO: (1067) Member selection is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    oActiveWindow.selection = sNewMergeField


                Case EndLoopTag
                    'extract the field name
                    sFieldName = sFieldCode.Substring(sFieldCode.Length - (sFieldCode.Length - iSep))

                    'Strip off the file name at the beginning
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(iSep)
                    End If

                    ' Strip off the id character at the end
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(0, iSep - 1)
                    End If

                    'Construct new merge field
                    sNewMergeField = c_sFieldStartMarker & EndLoopTag & Separator & sFieldName & c_sFieldEndMarker

                    'UPGRADE_TODO: (1067) Member selection is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    oActiveWindow.selection = sNewMergeField


                Case Else
                    oBookmark.Range.Text = "Invalid Bookmark"

            End Select

        Next oBookmark2

        'Update the fields
        'Alix()
        'ActiveDocument.Fields.Update()
        'UPGRADE_TODO: (1067) Member Document is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
        oActiveWindow.Document.Application.ActiveDocument.Fields.Update()

        'Return the document


        Return result

Err_ConvertDocument:  '

        If oBookmark Is Nothing Then
        Else
            oBookmark = Nothing
        End If
        If oBookmarks Is Nothing Then
        Else
            oBookmarks = Nothing
        End If
        If oActiveWindow Is Nothing Then
        Else
            oActiveWindow = Nothing
        End If

        result = gPMConstants.PMEReturnCode.PMError

        'Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    'DC260104 PN9904 added to fix problem editting uniplex document
    ' ***************************************************************** '
    '
    ' Name: SaveDocumentAsHTML
    '
    ' Description:
    '
    ' History: 25/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function SaveDocumentAsHTML(ByRef oTemplate As Object, ByVal lFileNo As Integer) As Integer
        Dim result As Integer = 0
        Dim sFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' SET 12/10/2004 ISS15027 - document is now in the client directory
            sFileName = m_sClient & "\" & "Doc " & CStr(lFileNo) & ".htm"


            oTemplate.SaveAs(sFileName, 8)

            oTemplate.Close()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveDocumentAsHTML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveDocumentAsHTML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function CopyClientToServer() As Integer
        Dim result As Integer = 0
        Dim sServer, sClient As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lMode = gSIRLibrary.ACMergeMode Then
                Return result
            End If

            'RWH(01/09/2000) RSAIB Process 108.
            UpdateTemplateNumberAndDependencies(m_lOldDocumentTemplateId, m_lDocumentTemplateId)

            m_lReturn = SetZipDirectory()

            'RWH(04/09/2000) - RSAIB Process 108.
            'Use new absolute directory to zip & unzip files.
            m_lReturn = DeleteHTMDependecies()

            CopyFilesToZipTemp()

            'First get rid of the old one...
            If Task <> gPMConstants.PMEComponentAction.PMAdd Then
                If m_lOldDocumentTypeId <> m_lDocumentTypeId Then
                    sServer = m_sServer & "\Type " & CStr(m_lOldDocumentTypeId) & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip"
                    m_lReturn = bPMDocFunctions.DeleteFile(sServer) ' RAM20040209 : Bug fix for PN Issue 10231
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete File [" & sServer & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
            End If

            sServer = m_sServer & "\Type " & CStr(m_lDocumentTypeId)

            m_lReturn = CreateFolderTree(sServer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create destination Folder [" & sServer & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            sServer = sServer & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip"

            'RWH(04/09/2000) - RSAIB Process 108.
            sClient = m_sZIP_DIRECTORY & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip"

            ' RAM20040206 : Use the Zip function in bPMDocFunctions.
            m_lReturn = Zip(sClient, m_sDocFileExtension)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Zip the files in [" & sClient & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' RAM20040209 : Use bPMDocFunctions.CopyFile rather than FileCopy
            m_sFileCopyMsg = ""
            m_lReturn = bPMDocFunctions.CopyFile(sClient, sServer, True, True, m_sFileCopyMsg)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file from Client To Server." & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Source File      : " & sClient & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Destination File : " & sServer & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Error Details    : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy template from client to server", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: DeleteServer
    '
    ' Description: deletes the template from the server
    ' Edit History  :
    ' RAM20040209   : Bug fix Ref. PN Issue 10231
    '                 1. Removed unwanted Dir Commands as it Locks the Directory
    '                 2. Modify code to use bPMDocFunctions.CreateFolderTree
    '                      rather than the MkDir Command
    ' ***************************************************************** '
    Public Function DeleteServer() As Integer

        Dim result As Integer = 0
        Dim sServer As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Make sure the directory's there
            m_lReturn = CreateFolderTree(sServer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create folder [" & sServer & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            sServer = m_sServer & "\Type " & CStr(m_lDocumentTypeId) & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip"

            m_lReturn = bPMDocFunctions.DeleteFile(sServer) ' RAM20040209 : Bug fix for PN Issue 10231
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete file [" & sServer & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete template from server", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MergeDocument
    '
    ' Description: merges the document
    '
    ' ***************************************************************** '
    Public Function MergeDocument() As Integer

        Dim result As Integer = 0
        Dim iOptionNumber As Integer
        Dim sOptionValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If (m_lMode = gSIRLibrary.ACNormalMode) Or (m_lMode = gSIRLibrary.ACSwiftEditMode) Then
                Return result
            End If

            m_lOldDocumentTypeId = m_lDocumentTypeId
            m_lOldDocumentTemplateId = m_lDocumentTemplateId

            If m_oDocManager Is Nothing Then

                m_lReturn = g_oObjectManager.GetInstance(m_oDocManager, sClassName:="iPMBDocManager.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Error getting instance of iPMBDocManager.Interface", "Merge Document Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    m_oDocManager = Nothing
                    Return result
                End If
            End If

            m_oDocManager.PartyCnt = m_lPartyCnt

            m_oDocManager.InsuranceFileCnt = m_lInsuranceFileCnt

            m_oDocManager.ClaimCnt = m_lClaimCnt

            m_oDocManager.DocumentRef = m_sDocumentRef


            m_oDocManager.DocumentTemplateId = m_lDocumentTemplateId

            m_oDocManager.DocumentTypeId = m_lDocumentTypeId

            m_oDocManager.IsEditableAfterMerging = m_iIsEditableAfterMerging

            m_oDocManager.WordVersion = m_sWordVersion
            'DC170203 -ISS1405 -For processing of shared invoices



            'm_oDocManager.set_PolicySharesArray(m_vPolicySharesArray)
            m_oDocManager.PolicySharesArray = m_vPolicySharesArray


            m_oDocManager.client = m_sClient 'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup


            m_oDocManager.CalledFromSwift = m_bCalledFromSwift


            m_oDocManager.ParameterXML = m_sParameterXML


            m_oDocManager.RiskCnt = m_lRiskCnt


            m_lReturn = m_oDocManager.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'MessageBox.Show("Error running iPMBDocManager.Start", "Error running merge", MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_oDocManager = Nothing
                Return result
            End If

            m_bSpoolMessage = True

            'DN 04/05/01 - Determine if DME is installed
            iOptionNumber = 10


            m_lReturn = m_oBusiness.getOption(v_iOptionNumber:=iOptionNumber, r_sOptionValue:=sOptionValue)

            m_bArchiveMessage = (sOptionValue.Trim() = "1")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to merge the document", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetBrowser
    '
    ' Description: sets the Browser control
    '
    ' ***************************************************************** '


    ' ***************************************************************** '
    ' Name: ShowWorkManager
    '
    ' Description: Shows Task editor for work manager
    '
    ' MS260601
    ' ***************************************************************** '

    Public Function ShowWorkManager(ByVal Mode As Integer, Optional ByVal TaskID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oWorkManager As Object
        Dim lInstanceID As Integer
        Dim vPartyType As Object
        Dim vKeyArray(,) As Object

        Try
            Select Case Mode
                ' This is actually adding the task to the work manager list
                Case WMInstance


                    m_lReturn = m_oBusiness.GetPartyType(r_vPartytype:=vPartyType, v_lPartyCnt:=PartyCnt)


                    Dim temp_oWorkManager As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oWorkManager, sClassName:="iPMWrkTaskInstanceTemp.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    oWorkManager = temp_oWorkManager


                    m_lReturn = oWorkManager.Initialise

                    ' Pass needed properties to the interface

                    oWorkManager.InsuranceFileCnt = InsuranceFileCnt

                    oWorkManager.InsuranceFolderCnt = InsuranceFolderCnt

                    oWorkManager.PartyCnt = PartyCnt


                    oWorkManager.PartyType = CStr(vPartyType(0, 0)).Trim()

                    oWorkManager.Party = m_vPartyPolicy(1)


                    oWorkManager.ResolvedName = CStr(vPartyType(1, 0)).Trim()

                    If Strings.Len(CStr(m_vPartyPolicy(2))) Then

                        oWorkManager.PolicyRef = m_vPartyPolicy(2)
                    End If

                    'DJM 12/03/2004 : Add extra parameters so that task shows correct user who created it.
                    ReDim vKeyArray(1, 2)
                    ' Task Created Date

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "task_created_date"

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = DateTime.Now
                    ' Task Created by User ID

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "task_created_by_id"

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = g_iUserID


                    m_lReturn = oWorkManager.SetKeys(vKeyArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetKeys to Task Instance Template Editor", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowWorkManager", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                        oWorkManager.Dispose()
                        oWorkManager = Nothing
                        Return m_lReturn
                    End If

                    'Set the ID of the current attached task.

                    oWorkManager.PMWrkTaskInstanceCnt = TaskID

                    oWorkManager.CallingAppName = "Raised Document"


                    m_lReturn = oWorkManager.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Start the Form

                    m_lReturn = oWorkManager.Start

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        oWorkManager.Dispose()
                        oWorkManager = Nothing
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    With oWorkManager

                        lInstanceID = .PMWrkTaskInstanceCnt
                    End With


                    oWorkManager.Dispose()
                    oWorkManager = Nothing

                    ' Don't need to pass anything back. We have already created the task.
                    result = 0

                    ' This is saving the task template to be used off the back of this document template
                Case WMTemplate

                    'UPGRADE_NOTE: (7015) The following call to GetInstance could not be automatically upgraded because of invalid parameters. More Information: http://www.vbtonet.com/ewis/ewi7015.aspx
                    m_lReturn = g_oObjectManager.GetInstance(oObject:=oWorkManager, sClassName:="iPMWrkTaskInstanceTemp.Interface_Renamed", vInstanceManager:=PMGetLocalInterface)


                    m_lReturn = oWorkManager.Initialise
                    'DJM 19/02/2003 PN10480 : Copied fix from 1.8.5 issue 4697.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise Task Instance Template Editor", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowWorkManager", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                        oWorkManager.Dispose()
                        oWorkManager = Nothing
                        Return m_lReturn
                    End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' RAM20031117 : PN Issue 4697 - START
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' Set the flag, so that the taskinstance template doesn't save the values to the DB
                    m_bDoNotSaveTaskTemplateToDB = True

                    If Information.IsArray(m_vTaskTemplateDetailsArray) Then

                        ' We have an array, with values, so we need to use these values to be used in
                        ' the Task Template Editor

                        ReDim vKeyArray(1, ACColMaxColumns + 2)

                        ' Task Instance Cnt

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskInstanceCnt) = PMNavKeyConst.PMKeyNameTaskInstanceCnt

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskInstanceCnt) = m_vTaskTemplateDetailsArray(ACColTaskInstanceCnt)
                        ' Task Group Code

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskGroupCode) = PMNavKeyConst.PMKeyNameTaskGroupCode

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskGroupCode) = m_vTaskTemplateDetailsArray(ACColTaskGroupCode)
                        ' Task Group ID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskGroupID) = PMNavKeyConst.PMKeyNameTaskGroupID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskGroupID) = m_vTaskTemplateDetailsArray(ACColTaskGroupID)
                        ' Task Code

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskCode) = PMNavKeyConst.PMKeyNameTaskCode

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskCode) = m_vTaskTemplateDetailsArray(ACColTaskCode)
                        ' Task ID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskID) = PMNavKeyConst.PMKeyNameTaskID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskID) = m_vTaskTemplateDetailsArray(ACColTaskID)
                        ' Task Description

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskDescription) = PMNavKeyConst.PMKeyNameTaskDescription

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskDescription) = m_vTaskTemplateDetailsArray(ACColTaskDescription)
                        ' Task Customer

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskCustomer) = PMNavKeyConst.PMKeyNameTaskCustomer

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskCustomer) = m_vTaskTemplateDetailsArray(ACColTaskCustomer)
                        ' Task Due Date

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskDueDate) = PMNavKeyConst.PMKeyNameTaskDueDate

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskDueDate) = m_vTaskTemplateDetailsArray(ACColTaskDueDate)
                        ' Task Is Urgent

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskIsUrgent) = PMNavKeyConst.PMKeyNameTaskIsUrgent

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskIsUrgent) = m_vTaskTemplateDetailsArray(ACColTaskIsUrgent)
                        ' User Group ID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColUserGroupID) = PMNavKeyConst.PMKeyNameUserGroupID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColUserGroupID) = m_vTaskTemplateDetailsArray(ACColUserGroupID)
                        ' User ID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColUserID) = PMNavKeyConst.PMKeyNameUserID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColUserID) = m_vTaskTemplateDetailsArray(ACColUserID)
                        ' Task Created Date

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskCreatedDate) = "task_created_date"

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskCreatedDate) = m_vTaskTemplateDetailsArray(ACColTaskCreatedDate)
                        ' Task Created by User ID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColTaskCreatedByID) = "task_created_by_id"

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColTaskCreatedByID) = m_vTaskTemplateDetailsArray(ACColTaskCreatedByID)

                        ' The following flag triggers not to fetch details from Database
                        m_bUseSuppliedTaskTemplateDetails = True

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColMaxColumns + 1) = "UseSuppliedTaskTemplateDetails"

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColMaxColumns + 1) = m_bUseSuppliedTaskTemplateDetails

                        ' Flag to determine, do we need to stored the values into the database

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACColMaxColumns + 2) = "DoNotSaveTaskTemplateToDB"

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACColMaxColumns + 2) = m_bDoNotSaveTaskTemplateToDB

                    Else

                        ReDim vKeyArray(1, 1)

                        ' If the document template is called, and we havn't got the details of the task
                        ' associated with this document template.

                        ' Flag to determine, do we need to fetch the values from the database
                        ' The following flag triggers to fetch details from Database
                        m_bUseSuppliedTaskTemplateDetails = False

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "UseSuppliedTaskTemplateDetails"

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_bUseSuppliedTaskTemplateDetails

                        ' Flag to determine, do we need to stored the values into the database

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "DoNotSaveTaskTemplateToDB"

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_bDoNotSaveTaskTemplateToDB
                    End If

                    ' Set the keys

                    m_lReturn = oWorkManager.SetKeys(vKeyArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetKeys to Task Instance Template Editor", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowWorkManager", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                        oWorkManager.Dispose()
                        oWorkManager = Nothing
                        Return m_lReturn
                    End If

                    ' Added to pass template ID if we are editing (yes, I know it passes a PMAdd state)
                    ' The edit is used for instances of tasks so the template uses add
                    If m_lWrkTaskCnt > 0 Then

                        oWorkManager.PMWrkTaskInstanceCnt = m_lWrkTaskCnt
                    ElseIf m_lOldWrkTaskCnt > 0 Then

                        oWorkManager.PMWrkTaskInstanceCnt = m_lOldWrkTaskCnt
                    End If

                    'MKW170703 PN5225 Allow user to select User Group

                    oWorkManager.CallingAppName = "Document Task"


                    m_lReturn = oWorkManager.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Start the Form

                    m_lReturn = oWorkManager.Start

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        oWorkManager.Dispose()
                        oWorkManager = Nothing
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    With oWorkManager

                        lInstanceID = .PMWrkTaskInstanceCnt

                        'DJM 19/02/2003 PN10480 : Copied fix from 1.8.5 issue 4697.
                        ' If they press, ok, then only update the array

                        If .Status = gPMConstants.PMEReturnCode.PMOK Then

                            ' Get Back the other details,
                            If Information.IsArray(m_vTaskTemplateDetailsArray) Then
                            Else
                                ReDim m_vTaskTemplateDetailsArray(ACColMaxColumns)
                            End If

                            ' Task Instance Cnt

                            m_vTaskTemplateDetailsArray(ACColTaskInstanceCnt) = .PMWrkTaskInstanceCnt
                            ' Task Group Code
                            m_vTaskTemplateDetailsArray(ACColTaskGroupCode) = ""
                            ' Task Group ID

                            m_vTaskTemplateDetailsArray(ACColTaskGroupID) = .PMWrkTaskGroupId
                            ' Task Code
                            m_vTaskTemplateDetailsArray(ACColTaskCode) = ""
                            ' Task ID

                            m_vTaskTemplateDetailsArray(ACColTaskID) = .PMWrkTaskId
                            ' Task Description

                            m_vTaskTemplateDetailsArray(ACColTaskDescription) = .Description
                            ' Task Customer

                            m_vTaskTemplateDetailsArray(ACColTaskCustomer) = .Customer
                            ' Task Due Date

                            m_vTaskTemplateDetailsArray(ACColTaskDueDate) = .DueDate
                            ' Task Is Urgent

                            m_vTaskTemplateDetailsArray(ACColTaskIsUrgent) = .IsUrgent
                            ' User Group ID

                            m_vTaskTemplateDetailsArray(ACColUserGroupID) = .PMUserGroupID
                            ' User ID

                            m_vTaskTemplateDetailsArray(ACColUserID) = .UserID
                            ' Task Created Date

                            m_vTaskTemplateDetailsArray(ACColTaskCreatedDate) = .TaskCreatedDate
                            ' Task Created by User ID

                            m_vTaskTemplateDetailsArray(ACColTaskCreatedByID) = .TaskCreatedByID
                        End If
                    End With


                    oWorkManager.Dispose()
                    oWorkManager = Nothing

                    result = lInstanceID

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show Task Editor", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowWorkManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the details to the data storage.
        m_lReturn = m_oBusiness.GetNext(vDocumentTemplateID:=m_lDocumentTemplateId, vCode:=m_sDocumentTemplateCode, _
                                        vDescription:=m_sDocumentTemplateDescription, vSourceId:=m_lSourceId, _
                                        vDocumentTypeId:=m_lDocumentTypeId, vIsDeleted:=m_iIsDeleted, _
                                        vSlotNumber:=m_vSlotNumber, vRiskCodeId:=m_vRiskCodeId, _
                                        vRiskGroupId:=m_vRiskGroupId, vIsEditableAfterMerging:=m_iIsEditableAfterMerging, _
                                        vPrinter:=m_sSelectedPrinter, vChaser:=m_sSelectedChaser, _
                                        vDocumentFilter:=m_sDocumentFilter, vEffectiveDate:=m_dtDocEffectiveDate, _
                                        vIsVisibleFromWeb:=m_bVisibleFromWeb, vIsVisibleFromClientManager:=m_bVisibleFromClientManager, _
                                        vArchiveWithNoPrint:=m_bArchiveWithNoPrint, vEmailAsBody:=m_bEmailAsBody, _
                                        vSpoolDocument:=m_bSpoolDocument, vArchiveAsText:=m_bArchiveAsText, _
                                        vTemplateGroupID:=m_lTemplateGroupID, vTemplateSubGroupID:=m_lTemplateSubGroupID, _
                                        vIsInternalOnly:=m_bIsInternalOnly, vIsSelectedByDefault:=m_bIsSelectedByDefault, vArchiveAsXML:=m_bArchiveAsXML)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
        End If

        Return result

    End Function



    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Gets all of the lookup values.


        m_oBusiness.DocumentTypeId = DocumentTypeId

        ' Check the task.
        Select Case (m_iTask)
            Case gPMConstants.PMEComponentAction.PMAdd
                ' Get all of the lookup values.

                m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookupDetails)

            Case gPMConstants.PMEComponentAction.PMEdit
                ' Get all of the lookup values with the correct
                ' effective date.

                m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookupDetails)

            Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDelete
                ' Get lookup values for viewing only.

                m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookupDetails)
        End Select

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

            Return result
        End If

        'MKW020503 PN3890 Added Claim Text File Slots variable

        m_lReturn = m_oBusiness.GetOtherDetails(vClientArray:=m_vClientArray, vPolicyArray:=m_vPolicyArray, vDocumentTypeArray:=m_vDocumentTypeArray, vSourceArray:=m_vSourceArray, vRiskBySource:=m_vRiskBySource, vRiskGroupBySource:=m_vRiskGroupBySource, vClaimArray:=m_vClaimArray)

        ' {* USER DEFINED CODE (End) *}

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get details.
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get other details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

            Return result
        End If


        'm_lReturn = m_oBusiness.GetAvailablePrinters(r_vPrinterArray:=m_vPrinters, r_sDefaultPrinter:=m_sDefaultPrinter)

        '' Check for errors.
        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '    result = gPMConstants.PMEReturnCode.PMFalse

        '    ' Log Error.
        '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve printer details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
        'End If

        'sj 26/06/2002 - start
        'Bug No 202
        If m_bChaserLettersEnabled Then
            'sj 26/06/2002 - end
            'Ak 090402

            m_lReturn = m_oBusiness.GetAvailableChasers(r_vChaserArray:=m_vChasers)

            ' Check for errors.
            'AK 010502 - will need to continue, even if there are no chasers
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve Chaser details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If
        End If


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '

    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the lookup values.

        If (ctlLookup.Name = "cboRisk") Or (ctlLookup.Name = "cboGroup") Then



            'ctlLookup.AddItem("(None)")


            'ctlLookup.ItemData(ctlLookup.NewIndex) = 0
            Dim listIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem("(None)", 0))

        End If

        bFoundMatch = False

        'RWH(18/09/2000) Check array is not empty first.
        If Information.IsArray(m_vLookupValues) Then
            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow
        End If

        ' Check if there has been a table match.
        If Not bFoundMatch Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

            Return result
        End If

        ' Using the lookup values, populate the control with
        ' the details from the lookup details array.

        For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
            ' Add the details to the control.


            'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))




            'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))

            Dim listIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))
            'SP150998 - compare long value not string
            ' Check if this is the selected index.
            If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then



                    'ctlLookup.ListIndex = ctlLookup.NewIndex
                    ctlLookup.SelectedIndex = listIndex
                End If
            End If

        Next lCntr

        ' Check if the selected index is blank. If so,
        ' we set the controls index to zero.
        If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then


            ctlLookup.SelectedIndex = 0
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: EditDocument
    '
    ' Description:
    '
    ' Edit History  :
    ' RAM20040206   : Bug fix for PN Issue 10231
    '                 1. Removed unwanted Dir Commands as it locks the directory
    '                 2. Used Environ$ instead of Hardcoded "C:\"
    ' ***************************************************************** '
    Private Function EditDocument() As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sPath As String = ""
        Dim sDirToCopyTo As String = ""
        Dim sUsername As String = ""
        Dim lpVersionInfo As MainModule.OSVERSIONINFO = MainModule.OSVERSIONINFO.CreateInstance() 'MKW170703 PN5375
        Dim bRetryOpening As Boolean
        Dim bError As Boolean


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim bInstallAttempted As Boolean = False

        Do
            bRetryOpening = False ' PN17947

            If OurDocIsRunning() = gPMConstants.PMEReturnCode.PMFalse Then
                If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = OpenDocument()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else
                    lReturn = LaunchOurDoc()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            'Show the field manager
            If (m_lMode = gSIRLibrary.ACNormalMode) Or (m_lMode = gSIRLibrary.ACSwiftEditMode) Then

                Information.Err().Clear()
                bError = False

                If m_lMode = gSIRLibrary.ACSwiftEditMode Then
                    ' The following line will set a flag, indicating that we are in SWIFT Mode
                    ' This line will be executed only if we are calling from SWIFT
                    Try
                        m_oWord.Run("Normal.PMFieldManager.CalledFromSwift")
                    Catch
                        bError = True
                    End Try
                End If

                If Not bError Then
                    'Run Field Manager Macro to launch floating Field Manager app.
                    Try
                        m_oWord.Run("Normal.PMFieldManager.ShowFieldManagerMacro")
                    Catch
                        bError = True

                    End Try
                End If

                ' IJB 18/12/02 Install macros if not already present
                If bError Then
                    ' Macros not installed
                    'On Error GoTo Err_EditDocument


                    ' Release lock on normal.dot

                    If bInstallAttempted Then
                        'Install already attempted and the macro still failed
                        MessageBox.Show("Unable to run the field manager macro. This document might be protected." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Click OK to continue without field manager.", "Error Running Macro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else

                        ' Change wording and method for copying the .dot file to be the
                        ' same as was in 1.9. PN17947.
                        If MessageBox.Show("The required Word Macros do not appear to be installed.  " & _
                                           "Please ensure that the Word Macro Security Level (Tools/Macro/Security) is " & _
                                           "set to Medium.  Once this has been done, " & _
                                           "click yes to install the macros. You will require administrator " & _
                                           "rights on this PC to install the macros. If you do not have rights, " & _
                                           "please log in as a user with administrator rights and run the program " & _
                                           "iPMUCopyfile.exe in Pure\Application." & _
                                           Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Click No to continue without " & _
                                           "installing macros." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Install Macros?", "Word Macros Not Installed", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.Yes Then

                            bInstallAttempted = True

                            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

                            'sPath = CStr(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\", "PMDIR")) & _
                            '      "\Pure\Application\iPMUCopyfile.exe"

                            gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", sPath)
                            sPath &= "\Pure\Application\iPMUCopyfile.exe"


                            If IsFileExists(sPath) = gPMConstants.PMEReturnCode.PMNotFound Then
                                MessageBox.Show("Unable to install macros as iPMUCopyfile.exe cannot be found", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            Else
                                Process.Start(sPath)
                            End If

                            ' Word was closed in order to install the .dot file. Set the
                            ' retry flag, so the open process gets called again from the
                            ' beginning. 
                            bRetryOpening = True
                        End If
                    End If
                End If
            End If

        Loop While bRetryOpening

        ' Maximize window for word application and make it visible
        m_oWord.WindowState = Word.WdWindowState.wdWindowStateMaximize
        m_oWord.ActiveWindow.ActivePane.View.Zoom.Percentage = 100
        m_oWord.Visible = True
        activateDocumentWindow(m_oWord.Name.Split(".")(0))
        m_oWord.Activate()

        Dim sTemp As String = ""
        If m_sWordVersion.Substring(0, 1) = "8" Then


            m_oWord.selection.WholeStory()

            sTemp = m_oWord.selection.Text.Trim()
            sTemp = sTemp.Replace(Strings.Chr(13).ToString(), "")
            sTemp = sTemp.Replace(Strings.Chr(160).ToString(), "")
            If sTemp.Trim() = "" Then
                m_oWord.selection.Delete()
            Else
                m_oWord.selection.StartOf()
            End If

        End If

        'Wait until the word instance we opened, is closed.
        Do While OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue
            Sleep(500) 'Do nothing for half a second
            Application.DoEvents()
        Loop

        'Prevent message if in Document Template
        If (m_iTask <> gPMConstants.PMEComponentAction.PMAdd) And (m_iTask <> gPMConstants.PMEComponentAction.PMEdit) Then
            If m_sDocumentTemplateCode.Trim() = ACSFSTOBCode Then
                lReturn = MessageBox.Show("Did you print Terms of Business letter?", "Terms of Business letter printing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                If lReturn = System.Windows.Forms.DialogResult.Yes Then
                    m_lReturn = UpdatePartyTobLetter()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Store TOB Letter Date", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDocument")
                    End If
                End If
            End If
        End If


        Return result

        Return result

    End Function
    Public Sub activateDocumentWindow(ByVal docname As String) ' code to activate the document window through loop(by catching all running process's main window title name one by one)
        Try
            Dim alllocal As Process() = Process.GetProcesses()
            Dim item As Process

            For Each item In alllocal
                If item.MainWindowTitle.ToString <> "" Then
                    If InStr(item.MainWindowTitle.ToString, docname) > 0 Then
                        Dim intourdoc As Integer = item.Id
                        AppActivate(intourdoc)
                        Exit For
                    End If
                End If
            Next
        Catch
            'Do Nothing
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: MailDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function MailDocument() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sMsg As String = ""
        Dim sMainEmailAddress As String = String.Empty
        Dim msgReply As DialogResult
        Dim sOptionValue As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bEmailAsBody Then

                m_lReturn = m_oBusiness.GetPolicyLevelEmailAddress(m_lInsuranceFileCnt, sMainEmailAddress)
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    gPMFunctions.RaiseError(ACClass & "." & "MailDocument", "Failed to retreive Policy Level Email Address", gPMConstants.PMELogLevel.PMLogError)
                End If

                If String.IsNullOrEmpty(sMainEmailAddress) Then
                    m_lReturn = m_oBusiness.GetPartyMainEmailAddress(v_lParty_cnt:=m_lPartyCnt, v_sEmailAddress:=sMainEmailAddress)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        gPMFunctions.RaiseError(ACClass & "." & "MailDocument", "Failed to retreive main Email Address", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                If String.IsNullOrEmpty(sMainEmailAddress) Then
                    m_lReturn = AddFailedEmailWorkManagerTask()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(ACClass & "." & "MailDocument", "Could not create an Failed Email Work Manager Task", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    msgReply = MessageBox.Show("Do You Wish Send This Document as Email Body?", "Spool", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If msgReply = System.Windows.Forms.DialogResult.Yes Then

                        m_lReturn = m_oBusiness.SendEMail(v_sTo:=gPMFunctions.ToSafeString(sMainEmailAddress), v_sSubject:=m_sDocumentDescription, v_sMessagePath:=m_sClientDocument, v_sAttachment:="")

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(ACClass & "." & "MailDocument", "SendEmail Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Else
                        Return result
                    End If

                End If

            End If

            If m_bEmailDocsAsPDF Then
                Return EmailPDFDocument()
            End If

            If OurDocIsRunning() = gPMConstants.PMEReturnCode.PMFalse Then
                If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                    m_oDocument = m_oWord.Documents.Open(m_sClientDocument, ConfirmConversions:=False)
                    m_lReturn = OpenDocument()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else
                    lReturn = CType(LaunchOurDoc(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            m_oWord.WindowState = 1

            m_lReturn = SetDocumentVariables()

            m_oWord.Run("Normal.PMDocumentManager.PMBEmailDocument")

            m_lReturn = ShutItDown()

            m_bSpoolMessage = False

            If m_sDocumentTemplateCode = ACSFSTOBCode Then
                m_lReturn = UpdatePartyTobLetter()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MailDocument  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MailDocument")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            Return result

        Catch excep As System.Exception

            Select Case Information.Err().Number
                Case Else
                    result = gPMConstants.PMEReturnCode.PMError

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MailDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MailDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End Select

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EmailPDFDocument
    '
    ' Description:
    '
    ' History: 27/09/2005 RDC - created
    '
    ' ***************************************************************** '
    Private Function EmailPDFDocument() As Integer

        Dim result As Integer = 0
        Dim iReturnValue As Integer = 0
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim sOutputFilePath As String = ""

        Dim sEmailTo, sEmailSubject, sEmailMessage As String

        Const DIGITAL_SIGNATURE As Integer = 5023
        Dim sDigitalSignature As String = ""
        Dim bIsDigitalSignature As Boolean

        Dim emailAttachments As New List(Of String)
        Dim emailCreateArchive As Boolean = False


        Const EMAIL_DOC_AS_PDF As Integer = 5010
        Const SHAREPOINT_PATH As Integer = 5085
        Dim sArchiveAsPDF As String = ""

        Dim sOption As String = ""
        Dim sFailMsg As String = ""
        Dim sSharePointServer_Path As String = ""
        Dim sOptionValue As String = String.Empty

        Dim oForm As frmEmail


        result = gPMConstants.PMEReturnCode.PMFalse

        oForm = New frmEmail()

        With oForm
            .ShowDialog()

            lStatus = .Status
            sEmailTo = .EmailTo
            sEmailSubject = .EmailSubject
            sEmailMessage = .EmailMessage
        End With

        oForm.Close()

        oForm = Nothing

        If lStatus <> gPMConstants.PMEReturnCode.PMOK Then
            Return result
        End If

        ' Retrieve the system option to find whether to Digital Signature is required or not.

        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=DIGITAL_SIGNATURE, r_sOptionValue:=sDigitalSignature, v_iSourceID:=g_iSourceID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        Else
            bIsDigitalSignature = gPMFunctions.ToSafeBoolean(sDigitalSignature, False)
        End If


        'Convert existing document into PDF
        sOutputFilePath = m_sClientDocument.Substring(0, m_sClientDocument.Length - 3) & "pdf"

        m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(m_sClientDocument, sOutputFilePath)

        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=EMAIL_DOC_AS_PDF, r_sOptionValue:=sArchiveAsPDF, v_iSourceID:=g_iSourceID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (sArchiveAsPDF = 1) Then
            emailCreateArchive = True
        End If


        emailAttachments.Add(sOutputFilePath)

        m_lReturn = m_oBusiness.GetPolicyLevelEmailAddress(m_lInsuranceFileCnt)
        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.RaiseError("EmailPDFDocument", "Failed to retreive Policy Level Email Address", gPMConstants.PMELogLevel.PMLogError)
        End If

        iReturnValue = gPMConstants.PMEReturnCode.PMTrue
        iReturnValue = m_oBusiness.SendEMail(v_sTo:=sEmailTo,
                                  sCC:="",
                                  v_sSubject:=sEmailSubject,
                                  v_sMessageString:=sEmailMessage,
                                  v_sAttachment:=emailAttachments.ToArray,
                                  sBCC:="",
                                  bSaveEMLFile:=emailCreateArchive,
                                  sEMLFile:=sOutputFilePath)



        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'Check for sharepoint
        If sOption = "2" Then

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=SHAREPOINT_PATH, r_sOptionValue:=sSharePointServer_Path, v_iSourceID:=g_iSourceID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "SharePoint is not configured"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            'Sharepoint Integration
            If m_oSharePoint Is Nothing Then
                m_oSharePoint = New bSIRSharepoint.Business()

                m_lReturn = m_oSharePoint.Initialise(MainModule.g_oObjectManager.UserName, MainModule.g_oObjectManager.Password, g_iUserID, g_iSourceID, g_iLanguageID, 0, 0, m_sCallingAppName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the m_oSharePoint object", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oSharePoint.ArchiveDocument(PartyCnt:=m_lPartyCnt, InsuranceFileCnt:=0,
                                          ClaimID:=0, CaseID:=0, DocumentTemplateId:=m_lDocumentTemplateId,
                                          TemplateGroupID:=m_lTemplateGroupID,
                                          TemplateSubGroupID:=m_lTemplateSubGroupID,
                                          SourceFile:=sOutputFilePath, InternalOnly:=m_bIsInternalOnly, SharepointPath:=sSharePointServer_Path,
                                          DestinationFilename:="",
                                          PartyCode:="", PolicyNumber:="",
                                          ClaimNumber:="")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ArchiveDocument the SharePoint ", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function
    ' ***************************************************************** '
    '
    ' Name: SetDocumentVariables
    '
    ' Description:
    '
    ' History: 06/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function SetDocumentVariables() As Integer

        Dim result As Integer = 0
        Dim oDocument, aVar As Object
        Dim lCompanyIdIndex, lPartyCntIndex, lInsuranceFolderCntIndex, lInsuranceFileCntIndex, lClaimCntIndex, lDocumentTypeIdIndex, lDocumentTypeDescriptionIndex, lFormatIndex As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'RWH(03/08/2000) Replaced OLE container stuff.
        '    Set oDocument = OLE1.object.Application.ActiveDocument
        If OurDocIsRunning() <> gPMConstants.PMEReturnCode.PMTrue Then
            If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                m_oDocument = m_oWord.Documents.Open(m_sClientDocument, ConfirmConversions:=False)
                System.Threading.Thread.Sleep(1000)
                m_lReturn = OpenDocument()
                System.Threading.Thread.Sleep(1000)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else

                'TN20010711 - start (why redo what LaunchOurDoc() is already done - doh)
                m_lReturn = LaunchOurDoc()
                System.Threading.Thread.Sleep(1000)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'm_lReturn = LaunchOurDoc()
                'm_oWord = New Word.Application
                'm_oDocument = m_oWord.Documents.Open(m_sClientDocument, ConfirmConversions:=False)
                'm_lReturn = OpenDocument()
                'If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                '    SetDocumentVariables = gPMConstants.PMEReturnCode.PMFalse
                '    Exit Function

                'TN20010711 - end
            End If

        End If
        oDocument = m_oDocument

        'Get any already stored values - there shouldn't be any, but if there are and we
        'don't check we'll get an error when assigning them

        For Each aVar2 As Object In oDocument.Variables
            aVar = aVar2

            Select Case aVar.Name
                Case "CompanyId"

                    lCompanyIdIndex = aVar.Index
                Case "PartyCnt"

                    lPartyCntIndex = aVar.Index
                Case "InsuranceFolderCnt"

                    lInsuranceFolderCntIndex = aVar.Index
                Case "InsuranceFileCnt"

                    lInsuranceFileCntIndex = aVar.Index
                Case "ClaimCnt"

                    lClaimCntIndex = aVar.Index
                Case "DocumentTypeId"

                    lDocumentTypeIdIndex = aVar.Index
                Case "DocumentTypeDescription"

                    lDocumentTypeDescriptionIndex = aVar.Index
                Case "FMFormat"

                    lFormatIndex = aVar.Index
            End Select
        Next aVar2

        aVar = Nothing

        If lCompanyIdIndex = 0 Then

            oDocument.Variables.Add(Name:="CompanyId", Value:=g_iSourceID)
        Else

            oDocument.Variables(lCompanyIdIndex).Value = g_iSourceID
        End If

        If lPartyCntIndex = 0 Then

            oDocument.Variables.Add(Name:="PartyCnt", Value:=m_lPartyCnt)
        Else

            oDocument.Variables(lPartyCntIndex).Value = m_lPartyCnt
        End If

        If m_lInsuranceFolderCnt <> 0 Then
            If lInsuranceFolderCntIndex = 0 Then

                oDocument.Variables.Add(Name:="InsuranceFolderCnt", Value:=m_lInsuranceFolderCnt)
            Else

                oDocument.Variables(lInsuranceFolderCntIndex).Value = m_lInsuranceFolderCnt
            End If
        End If

        If m_lInsuranceFileCnt <> 0 Then
            If lInsuranceFileCntIndex = 0 Then

                oDocument.Variables.Add(Name:="InsuranceFileCnt", Value:=m_lInsuranceFileCnt)
            Else

                oDocument.Variables(lInsuranceFileCntIndex).Value = m_lInsuranceFileCnt
            End If
        End If

        If m_lClaimCnt <> 0 Then
            If lClaimCntIndex = 0 Then

                oDocument.Variables.Add(Name:="ClaimCnt", Value:=m_lClaimCnt)
            Else

                oDocument.Variables(lClaimCntIndex).Value = m_lClaimCnt
            End If
        End If

        If lDocumentTypeIdIndex = 0 Then

            oDocument.Variables.Add(Name:="DocumentTypeId", Value:=m_lDocumentTypeId)
        Else

            oDocument.Variables(lDocumentTypeIdIndex).Value = m_lDocumentTypeId
        End If

        If lDocumentTypeDescriptionIndex = 0 Then

            oDocument.Variables.Add(Name:="DocumentTypeDescription", Value:=m_sDocumentTemplateDescription)
        Else

            oDocument.Variables(lDocumentTypeDescriptionIndex).Value = m_sDocumentTemplateDescription
        End If

        If lFormatIndex = 0 Then

            oDocument.Variables.Add(Name:="FMFormat", Value:="RTF")
        Else

            oDocument.Variables(lFormatIndex).Value = "RTF"
        End If


        For iLoop As Integer = 1 To m_oDocument.Inlineshapes.Count
            ' Only linked images need to be saved

            If m_oDocument.Inlineshapes(iLoop).Type = 4 Then

                m_oDocument.Inlineshapes(iLoop).LinkFormat.SavePictureWithDocument = True
            End If
        Next

        oDocument = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PrintDocument
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function PrintDocument() As Integer
        Dim result As Integer = 0

        'RWH(01/08/2000) Modified to deal with html documents.
        'RWH(26/07/2001) Set active printer to that stored against template.

        Dim lDocumentCnt As Integer
        Dim vInsuranceFolderCnt As String = ""
        Dim vInsuranceFileCnt As String = ""
        Dim vClaimCnt As String = ""

        Dim sOptionValue As String = ""
        Dim lReturn As Integer

        Dim oEvent As bSIREvent.Business
        Dim lEventTypeId As Integer
        Dim sDescription As String = ""
        Dim vPartyPolicy As Object
        Dim sPrinterSystemName As String
        Dim vOriginalActivePrinter As Object
        Dim sSystemName As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If (OurDocIsRunning() = gPMConstants.PMEReturnCode.PMFalse) Then
                If (OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue) Then
                    OpenDocument()
                Else
                    lReturn = LaunchOurDoc()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Launch our Doc : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If
                End If
            End If

            'MKW 24062004 PN12225 - Remove Systemname from local printers. START
            If Strings.Left(m_sSelectedPrinter, 2) = "\\" Then
                GetSystemNameNoSID(sSystemName)
                sPrinterSystemName = Mid(m_sSelectedPrinter, 3, IIf(InStr(3, m_sSelectedPrinter, "\") < 3, 0, InStr(3, m_sSelectedPrinter, "\") - 3))

                If UCase(sSystemName) = UCase(sPrinterSystemName) Then
                    m_sSelectedPrinter = Strings.Right(m_sSelectedPrinter, Len(m_sSelectedPrinter) - InStrRev(m_sSelectedPrinter, "\"))
                End If
            End If
            'MKW 24062004 PN12225 - Remove Systemname from local printers. END

            'RWH(26/07/01)Select printer for specific document.
            If (m_sSelectedPrinter <> "") Then
                vOriginalActivePrinter = m_oWord.ActivePrinter
                m_oWord.ActivePrinter = m_sSelectedPrinter
            End If

            'MKW171203 PN1977.  Show Word (in order to show printer dialog box).
            m_oWord.Visible = True

            'Run Document Manager Macro to print document.
            m_oWord.Run("Normal.PMDocumentManager.PMBPrintDocument")

            'MKW171203 PN1977.  Hide Word.
            m_oWord.Visible = False

            'RWH - reset previously active printer.
            If (m_sSelectedPrinter <> "") Then
                m_oWord.ActivePrinter = vOriginalActivePrinter
            End If

            lReturn = ShutItDown()

            'Print used to send to FileMaster, but not any more.  Should it still?

            'sj 23/10/2002 - start
            If m_bFishRequired = True Then
                m_lReturn = SendToFish()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" Failed to SendToFish : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            'JAS 07012005 PN17856 - if TOB letter then ask user if it has printed OK
            If m_sDocumentTemplateCode.Trim() = ACSFSTOBCode Then
                lReturn = MessageBox.Show("Did the Terms of Business letter print successfully?", "Terms of Business letter printing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                If lReturn = System.Windows.Forms.DialogResult.Yes Then
                    m_lReturn = UpdatePartyTobLetter()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If m_lInsuranceFileCnt <= 0 Then

                vInsuranceFileCnt = Nothing
            Else
                ' Only valid insurance file cnt
                vInsuranceFileCnt = CStr(m_lInsuranceFileCnt)

                ' RAM20031008 : Check if we have an insurance folder cnt
                '               If not, fetch the Insurance Folder Cnt
                If m_lInsuranceFolderCnt = 0 Then

                    m_lReturn = m_oBusiness.GetPartyPolicy(r_vArray:=vPartyPolicy, m_lInsuranceFileCnt:=m_lInsuranceFileCnt, m_lPartyCnt:=m_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch Insurance Folder Cnt for Insurance File : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                    ' Array contains Insurance_ref and Insurance folder cnt
                    If m_sInsuranceFileRef.Trim().Length = 0 Then

                        m_sInsuranceFileRef = CStr(vPartyPolicy(1))
                    End If


                    m_lInsuranceFolderCnt = CInt(vPartyPolicy(2))

                End If

            End If
            'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup END

            ' Set Insurance Folder Cnt
            If m_lInsuranceFolderCnt = 0 Then

                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = CStr(m_lInsuranceFolderCnt)
            End If

            If m_lClaimCnt = 0 Then

                vClaimCnt = Nothing
            Else
                vClaimCnt = CStr(m_lClaimCnt)
            End If

            If m_lEventCnt = 0 Then
                lEventTypeId = PMBConst.PMBEventDocument
            Else
                lEventTypeId = PMBConst.PMBEventTransaction
            End If

            If sDescription.Trim() = "" Then
                sDescription = m_sDocumentTemplateDescription
            Else
                sDescription = sDescription
            End If

            If m_lEventCnt <> 0 Then
                ' If it is a transaction, just amend the doc reference
                Dim temp_oEvent As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oEvent, "bSIREvent.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oEvent = temp_oEvent


                m_lReturn = oEvent.WriteTemplate(TemplateID:=lDocumentCnt, v_lEventCnt:=m_lEventCnt)


                oEvent.Dispose()
                oEvent = Nothing

            Else


                m_lReturn = m_oBusiness.CreateEvent(r_lEventCnt:=EventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=vInsuranceFolderCnt, v_vInsuranceFileCnt:=vInsuranceFileCnt, v_vClaimCnt:=vClaimCnt, v_vDocumentCnt:=lDocumentCnt, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DocumentTypeId, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=lEventTypeId, v_dtEventDate:=DateTime.Today, v_sDescription:="Document Printed - " & sDescription)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Event Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LaunchOurDoc
    '
    ' Description:  Runs word and sets required document as current.
    '
    ' History: 01/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function LaunchOurDoc() As Integer
        Dim result As Integer = 0
        'TN20010711
        Dim sWindowText As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' SET 18/10/2004 ISS13245 - launch word            
        m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)


        'Open current document.
        m_lReturn = OpenDocument()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'For Debug.
        'm_oWord.Visible = True            
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PrintDocumentSilent
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function PrintDocumentSilent() As Integer
        Dim result As Integer = 0
        Dim lDocumentCnt As Integer
        Dim vInsuranceFolderCnt As String = ""
        Dim vInsuranceFileCnt As String = ""
        Dim vClaimCnt As String = ""
        Dim sOptionValue As String = ""
        Dim oEvent As bSIREvent.Business
        Dim lEventTypeId As Integer
        Dim sDescription As String = ""
        Dim vPartyPolicy As Object
        Dim sAsposeConvert As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sClientDocument = String.Empty Then
                m_sClientDocument = m_sClient & "\Doc " & m_lDocumentTemplateId & "." & m_sDocFileExtension
            End If


            m_lReturn = PrintDocumentUsingSiriusDocumentUtility(m_sClientDocument, "")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sDocumentTemplateCode = ACSFSTOBCode Then
                m_lReturn = UpdatePartyTobLetter()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocumentSilent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocumentSilent")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_lInsuranceFileCnt <= 0 Then

                vInsuranceFileCnt = Nothing
            Else
                ' Only valid insurance file cnt
                vInsuranceFileCnt = CStr(m_lInsuranceFileCnt)

                ' RAM20031008 : Check if we have an insurance folder cnt
                '               If not, fetch the Insurance Folder Cnt
                If m_lInsuranceFolderCnt = 0 Then

                    m_lReturn = m_oBusiness.GetPartyPolicy(r_vArray:=vPartyPolicy, m_lInsuranceFileCnt:=m_lInsuranceFileCnt, m_lPartyCnt:=m_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch Insurance Folder Cnt for Insurance File : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocumentSilent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                    ' Array contains Insurance_ref and Insurance folder cnt
                    If m_sInsuranceFileRef.Trim().Length = 0 Then

                        m_sInsuranceFileRef = CStr(vPartyPolicy(1))
                    End If


                    m_lInsuranceFolderCnt = CInt(vPartyPolicy(2))

                End If

            End If
            'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup END

            ' Set Insurance Folder Cnt
            If m_lInsuranceFolderCnt = 0 Then

                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = CStr(m_lInsuranceFolderCnt)
            End If

            If m_lClaimCnt = 0 Then

                vClaimCnt = Nothing
            Else
                vClaimCnt = CStr(m_lClaimCnt)
            End If

            If m_lEventCnt = 0 Then
                lEventTypeId = PMBConst.PMBEventDocument
            Else
                lEventTypeId = PMBConst.PMBEventTransaction
            End If

            If sDescription.Trim() = "" Then
                sDescription = m_sDocumentTemplateDescription
            Else
                sDescription = sDescription
            End If

            If m_lEventCnt = 0 Then
                lEventTypeId = PMBConst.PMBEventDocument
            Else
                lEventTypeId = PMBConst.PMBEventTransaction
            End If

            If sDescription.Trim() = "" Then
                sDescription = m_sDocumentTemplateDescription
            Else
                sDescription = sDescription
            End If

            If m_lEventCnt <> 0 Then
                ' If it is a transaction, just amend the doc reference
                Dim temp_oEvent As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oEvent, "bSIREvent.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oEvent = temp_oEvent


                m_lReturn = oEvent.WriteTemplate(TemplateID:=lDocumentCnt, v_lEventCnt:=m_lEventCnt)


                oEvent.Dispose()
                oEvent = Nothing

            Else


                m_lReturn = m_oBusiness.CreateEvent(r_lEventCnt:=EventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=vInsuranceFolderCnt, v_vInsuranceFileCnt:=vInsuranceFileCnt, v_vClaimCnt:=vClaimCnt, v_vDocumentCnt:=lDocumentCnt, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DocumentTypeId, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=lEventTypeId, v_dtEventDate:=DateTime.Today, v_sDescription:="Document Printed - " & sDescription)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Event Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocumentSilent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocumentSilent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SpoolDocument
    '
    ' Description:
    '
    ' History   : 08/05/2000 Tomo   - Created.
    '           : 30/01/2001 Tinny  - Change it to public and add optional parameters
    '                                  for description and document to be spooled
    '           : 25/03/2003 APS    - Amended to use mouse busy
    '           : RAM20040206       - Removed unwanted Dir Command as it locks the directory
    '           : RAM20040227       - Use bPMDocFunctions.zip function, rather than zipper
    '                                   (Done because, this method fails, on a pure client machine,
    '                                       works ok on server, standalone but not on client. It will
    '                                       provide 'Permissing Denied' error)
    '           : CJB20040818 PN14209 - Set a new flag to not delete .doc (in ArchiveDocument...UpdateFileMaster)
    '                                   as we are spooling and need to use the file on return from there...
    '           : MKW20041027       - Added optional parameter to stop files been deleted
    '                                   (as they have not been spooled).
    ' ***************************************************************** '
    Public Function SpoolDocument(Optional ByVal v_sDesc As Object = Nothing, Optional ByVal v_sDocName As Object = Nothing, Optional ByVal v_ArchiveCount As Object = Nothing) As Integer
        Dim result As Integer = 0

        Dim vPartyCnt As Object = Nothing
        Dim vInsuranceFolderCnt As Object = Nothing
        Dim vInsuranceFileCnt As Object = Nothing
        Dim vClaimCnt As Object = Nothing
        Dim sServer, sDescription As String
        Dim sSpoolDoc, sSpoolZip As String
        Dim iTemp As Integer


        Dim oFile As FileInfo
        Dim oFilesCollection As FileInfo()
        Dim oFolder As DirectoryInfo
        Dim sDocumentFileExtension As String '   RAM20040227
        Dim iTimesArchived As Integer
        'TN20010801
        Dim sPrinterName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If String.IsNullOrEmpty(v_sDocName) Then
                If String.IsNullOrEmpty(m_sClientDocument) Then
                    m_sClientDocument = m_sClient & "\Doc " & m_lDocumentTemplateId & "." & m_sDocFileExtension
                End If
            Else
                m_sClientDocument = v_sDocName
            End If

            If m_lPartyCnt = 0 Then
                vPartyCnt = Nothing
            Else
                vPartyCnt = CStr(m_lPartyCnt)
            End If

            If m_lInsuranceFolderCnt = 0 Then
                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = CStr(m_lInsuranceFolderCnt)
            End If

            If m_lInsuranceFileCnt = 0 Then
                vInsuranceFileCnt = Nothing
            Else
                vInsuranceFileCnt = CStr(m_lInsuranceFileCnt)
            End If

            If m_lClaimCnt = 0 Then
                vClaimCnt = Nothing
            Else
                vClaimCnt = CStr(m_lClaimCnt)
            End If

            If v_sDesc <> "" Then
                sDescription = v_sDesc
            End If

            If sDescription.Trim() = "" Then
                sDescription = m_sDocumentTemplateDescription
            End If

            If m_sClientDocument = String.Empty Then
                m_sClientDocument = m_sClient & "\Doc " & m_lDocumentTemplateId & "." & m_sDocFileExtension
            End If


            If v_sDocName <> "" Then
                m_sClientDocument = v_sDocName
                sSpoolDoc = v_sDocName
            End If

            'PN14445
            'Archive now as spooling destroys the document.
            'PM031316 - Moved it below as m_sClientDocument has to be set before archiving and PN14445 not altered.
            If m_bArchiveAfterPrinting Or m_bAutoArchiveEnabled Or m_bArchiveWithNoPrint Then
                m_lReturn = ArchiveDocument()
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    v_ArchiveCount = v_ArchiveCount + 1
                End If
            End If
            'PN14445End

            ' don't bother to set up document variables if we are spooling a report (ie Crystal generated)
            If m_lMode <> gSIRLibrary.ACSpoolReportMode Then

                'Do these here in case it's mailed from the spooler
                If Not m_bSpoolAsHtml Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                    If m_bArchiveAsPDF Then
                        sDocumentFileExtension = "pdf" ' RAM20040227
                        sSpoolDoc = m_sClientDocument.Substring(0, m_sClientDocument.Length - 3) & sDocumentFileExtension

                        m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(m_sClientDocument, sSpoolDoc)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        sSpoolDoc = m_sClientDocument
                    End If

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Else
                    'this is a HTML document
                    sSpoolDoc = m_sClientDocument
                    sDocumentFileExtension = "xml"
                End If
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If m_oDocSpooler Is Nothing Then

                m_lReturn = g_oObjectManager.GetInstance(m_oDocSpooler, "bSIRDocSpooler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the Document Spooler object", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oBusiness.GetTemplatePrinter(v_lDocTemplateID:=m_lDocumentTemplateId, r_sPrinterName:=sPrinterName)
            m_lReturn = m_oDocSpooler.DirectAdd(vDocumentSpoolerId:=m_lSpoolNumber, vDocumentTypeId:=m_lDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=sDescription, vIsEditable:=m_iIsEditableAfterMerging, vPrinter:=sPrinterName, vSpoolLevelInd:=1, vDocumentTemplateID:=m_lDocumentTemplateId, v_iIsClient:=m_iIsClient, v_iIsAgent:=m_iIsAgent, v_iIsOffice:=m_iIsOffice, v_iOrderByProductionOrder:=m_iProductionOrder, vTimesArchived:=v_ArchiveCount)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the business object, since we don't need it anymore

            m_oDocSpooler.Dispose()


            ' Destroy the instance of the spooler object
            ' from memory.
            m_oDocSpooler = Nothing


            'Get the path of the spool file
            m_lReturn = GetSpoolFilePath(v_sBaseDirectory:=m_sServer, v_bIsLevelTwo:=True, r_sSpoolDirectory:=sServer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetSpoolFilePath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument")
                Return result
            End If

            ''''''''''''''''''''''''NEW''''''''''''''''''''''''''''''''''''''''''
            'RWH(12/01/2001) Store spooled doc as standard word doc instead of html.

            ' RAM20040227   : Use the bPMDocFunctions.Zip rather than the g_oZipper
            'RKS 09/11/2004 PN15927

            sSpoolZip = m_sClientDocument.Substring(0, m_sClientDocument.Length - 3) & "zip"

            ' delete zip file if it exists
            ' RAM20040206 : Removed unwanted use of Dir Command. Use FSO instead
            m_lReturn = bPMDocFunctions.DeleteFile(sSpoolZip)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to delete the already existing file. ( " & sSpoolZip & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument")
                Return result
            End If

            '    ' Call the bPMDocFunctions.Zip method
            '    m_lReturn = Zip(sPath:=sSpoolZip, sInputDocumentFileExtension:=sDocumentFileExtnesion)
            '    If m_lReturn <> PMTrue Then
            '        SpoolDocument = PMError
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to Zip the following document. " & vbCrLf & _
            ''                  "m_lSpoolNumber = [" & m_lSpoolNumber & "]" & vbCrLf & _
            ''                  "sSpoolDoc = [" & sSpoolDoc & "]" & vbCrLf & _
            ''                  "m_sClientDocument = [" & m_sClientDocument & "]" & vbCrLf & _
            ''                  "sDocumentFileExtension = [" & sDocumentFileExtension & "]" & vbCrLf & _
            ''                  "sSpoolZip = [" & sSpoolZip & "]" & vbCrLf & _
            ''                  "m_sServer = [" & m_sServer & "]" & vbCrLf & _
            ''                  "sServer   = [" & sServer & "]" & vbCrLf, _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="SpoolDocument", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:="Zip failed"
            '    End If


            ' Add it to the zipper
            iTemp = g_oZipper.ZipFile(sFileIn:=sSpoolDoc, sFileOut:=sSpoolZip)
            If Not iTemp Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sFileCopyMsg = ""
            m_lReturn = bPMDocFunctions.CopyFile(sSpoolZip, sServer, True, False, m_sFileCopyMsg)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDocument failed. Failed to copy file." & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Source File      : " & sSpoolZip & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Destination File : " & sServer & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Error Details    : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If m_bFishRequired Then
                m_lReturn = SendToFish()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendToFish Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            If oFile Is Nothing Then
            Else
                oFile = Nothing
            End If
            If oFilesCollection Is Nothing Then
            Else
                oFilesCollection = Nothing
            End If
            If oFolder Is Nothing Then
            Else
                oFolder = Nothing
            End If

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetSpoolFilePath
    '
    ' Description:
    '
    ' Edit History  :
    ' 14/10/2002 sj : Created.
    ' RAM20040206   : Bug fix for PN Issue 10231
    '                 1. Removed unwanted Dir Command, as it lock the directory.
    '                 2. Use the CreateFolderTree function, rather than MkDir
    ' ***************************************************************** '
    Private Function GetSpoolFilePath(ByVal v_sBaseDirectory As String, ByVal v_bIsLevelTwo As Boolean, ByRef r_sSpoolDirectory As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lTemp, lTemp2, lTemp1 As Integer
        Dim sTemp As String = ""

        If v_bIsLevelTwo Then
            lTemp = m_lSpoolNumber \ 1000000
            lTemp1 = (m_lSpoolNumber \ 1000) - (lTemp * 1000)
            lTemp2 = m_lSpoolNumber - (lTemp2 * 1000) - (lTemp * 1000000)
        Else
            lTemp = m_lSpoolNumber \ 1000
            lTemp2 = m_lSpoolNumber - (lTemp * 1000)
        End If

        'Build it up and make sure all sub-directories are there...
        r_sSpoolDirectory = v_sBaseDirectory & "\Spooled Documents" & "\Company " & CStr(g_iSourceID) & "\" & StringsHelper.Format(lTemp, "000")


        If v_bIsLevelTwo Then
            r_sSpoolDirectory = v_sBaseDirectory & "\Spooled Documents" & "\Company " & CStr(g_iSourceID) & "\" & StringsHelper.Format(lTemp, "000") & "\" & StringsHelper.Format(lTemp1, "000")
        End If

        m_lReturn = CreateFolderTree(r_sSpoolDirectory)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            ' Log Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Directory ( " & r_sSpoolDirectory & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSpoolFilePath", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

        r_sSpoolDirectory = r_sSpoolDirectory & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ArchiveDocument
    '
    ' Description:
    '
    ' History : ArchiveDocument made public so that it could be accessed from Inteface PN 17576
    ' ***************************************************************** '
    Public Function ArchiveDocument() As Integer
        Dim result As Integer = 0
        Dim vInsuranceFolderCnt As String = ""
        Dim vInsuranceFileCnt As String = ""
        Dim vClaimCnt As String = ""
        Dim iOptionNumber As Integer
        Dim sOptionValue As String = ""
        Dim lDocNumber, lEventTypeId As Integer
        'eck100101
        Dim sDescription As String = ""

        Dim oEvent As bSIREvent.Business
        Dim vPartyPolicy As Object 'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add it to FileMaster, returning the document cnt

            'First check if FileMaster is installed

            iOptionNumber = 10 ' possibly use a set of constants?


            m_lReturn = m_oBusiness.getOption(v_iOptionNumber:=iOptionNumber, r_sOptionValue:=sOptionValue)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sOptionValue.Trim() <> "1" And sOptionValue.Trim() <> "2") Then
                MessageBox.Show("DocuMaster And Sharepoint is not enabled", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Steve Watton 11/05/2004
            'Added this is to prevent document being archived more than once
            If (Not m_bArchiveMessage) And (sOptionValue.Trim() <> "2") Then
                'If we have got this far then must have already been archived
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Insurance File Cnt
            'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup START
            If m_lInsuranceFileCnt <= 0 Then

                vInsuranceFileCnt = Nothing
            Else
                ' Only valid insurance file cnt
                vInsuranceFileCnt = CStr(m_lInsuranceFileCnt)

                ' RAM20031008 : Check if we have an insurance folder cnt
                '               If not, fetch the Insurance Folder Cnt
                If m_lInsuranceFolderCnt = 0 Then

                    m_lReturn = m_oBusiness.GetPartyPolicy(r_vArray:=vPartyPolicy, m_lInsuranceFileCnt:=m_lInsuranceFileCnt, m_lPartyCnt:=m_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch Insurance Folder Cnt for Insurance File : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                    ' Array contains Insurance_ref and Insurance folder cnt
                    If m_sInsuranceFileRef.Trim().Length = 0 Then

                        m_sInsuranceFileRef = CStr(vPartyPolicy(1))
                    End If


                    m_lInsuranceFolderCnt = CInt(vPartyPolicy(2))

                End If

            End If
            'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup END

            ' Set Insurance Folder Cnt
            If m_lInsuranceFolderCnt = 0 Then

                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = CStr(m_lInsuranceFolderCnt)
            End If

            If m_lClaimCnt = 0 Then

                vClaimCnt = Nothing
            Else
                vClaimCnt = CStr(m_lClaimCnt)
            End If

            If m_lEventCnt = 0 Then
                lEventTypeId = PMBConst.PMBEventDocument
            Else
                lEventTypeId = PMBConst.PMBEventTransaction
            End If
            'eck1001001 Prompt for None default description

            'PSL 3760 19/06/2003 Always ask for a description now
            'If lEventTypeId <> PMBEventTransaction Then
            ' CLG 01/09/2004 : RFC71 Added product option to allow spool and archive of documents
            If (m_bArchiveAfterPrinting And m_sDocumentDescription <> "") Or m_bAutoArchiveEnabled Then
                If m_bArchiveAfterPrinting Then
                    sDescription = m_sDocumentTemplateDescription
                Else
                    sDescription = m_sDocumentTemplateDescription
                End If
            Else
                sDescription = Interaction.InputBox("Event description for document?", "Event Description", m_sDocumentTemplateDescription)
            End If


            ' KB 07012003 set mouse pointer to busy in an attempt to stop them clicking OK
            ' multiple times
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If sDescription.Trim() = "" Then
                sDescription = m_sDocumentTemplateDescription
                'DJM 20040902 Issue 14445: Just asked for description, and m_sDocumentDescription is not always populated.
                '    ElseIf m_bArchiveAfterPrinting = True And m_sDocumentDescription <> "" Then
                '        sDescription = m_sDocumentDescription
            Else
                sDescription = sDescription
            End If

            'DN 08/12/01 - Archive the document with the input description

            If sOptionValue = "1" Then
                m_lReturn = UpdateFileMaster(lDocNumber:=lDocNumber, sDescription:=sDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            ElseIf sOptionValue = "2" Then
                m_lReturn = SendToSharePoint(lDocNumber:=lDocNumber, v_sDescription:=sDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Now create the event...

            If lEventTypeId = PMBConst.PMBEventTransaction Then
                ' If it is a transaction, just amend the doc reference
                Dim temp_oEvent As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oEvent, "bSIREvent.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oEvent = temp_oEvent


                m_lReturn = oEvent.WriteTemplate(TemplateID:=lDocNumber, v_lEventCnt:=m_lEventCnt)


                oEvent.Dispose()
                oEvent = Nothing

            Else
                'eck1001001 pass description
                'Add the created event


                m_lReturn = m_oBusiness.CreateEvent(r_lEventCnt:=EventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=vInsuranceFolderCnt, v_vInsuranceFileCnt:=vInsuranceFileCnt, v_vClaimCnt:=vClaimCnt, v_vDocumentCnt:=lDocNumber, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DocumentTypeId, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=lEventTypeId, v_dtEventDate:=DateTime.Today, v_sDescription:="Raised Document - " & sDescription, v_lFSAComplaintFolderCnt:=m_lFSAComplaintFolderCnt)

            End If

            ' KB 07012003 and now set it back
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Event Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            m_bSpoolMessage = False

            m_bArchiveMessage = False 'DN 04/05/01

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ArchiveDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SendToSharePoint
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function SendToSharePoint(ByVal lDocNumber As String, Optional ByVal v_sDescription As String = "") As Integer

        Dim result As Integer = 0
        Dim sDocType As String = ""
        Dim sPageType As String = ""
        Dim sOptionValue As String = ""
        Dim vIndexArray As Object = Nothing
        Dim sArchiveDoc As String = ""
        Dim sArchiveAsPDF As String = ""
        Dim sDocName As String = ""

        Dim sExtension As String = IO.Path.GetExtension(m_sClientDocument).ToUpper

        'Defined
        Dim m_sMergedFilePath As String = ""
        Dim m_bSpoolAsPDF As Boolean




        ''End defined region

        'Const ARCHIVE_AS_PDF As Integer = 5009     ''Unused Local Variable


        m_sMergedFilePath = m_sClientDocument
        sArchiveDoc = m_sClientDocument

        If m_oDocManager Is Nothing Then
            Dim temp_m_oDocManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocManager, sClassName:="iPMBDocManager.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oDocManager = temp_m_oDocManager

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Error getting instance of iPMBDocManager.Interface", "Merge Document Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_oDocManager = Nothing
                Return result
            End If
        End If



        If m_bArchiveAsText Then
            sDocType = "T"
            sPageType = "TXT"
            If m_sClientDocument.EndsWith("htm") Then
                m_lReturn = gPMFunctions.ConvertHTMLToTxt(sInputFileName:=m_sClientDocument, r_sOutputFilename:=sArchiveDoc)
            End If
        ElseIf m_bArchiveAsPDF Or m_bSpoolAsPDF Then
            sDocType = "F"
            sPageType = "PDF"

            If sExtension = ".DOC" Or sExtension = ".DOCX" Or sExtension = ".XML" Then
                sArchiveDoc = m_sClientDocument.Substring(0, m_sClientDocument.Length - sExtension.Length) & ".pdf"

                m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(v_sSourceDocument:=m_sClientDocument, v_sDestDocument:=sArchiveDoc)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" Failed to convert the following document to PDF : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return m_lReturn
                End If
                m_sMergedFilePath = sArchiveDoc
            End If
        ElseIf sExtension = ".DOC" Or sExtension = ".XML" Then
            'Default format is DocX

            sPageType = "DOCX"
            sArchiveDoc = m_sClientDocument.Substring(0, m_sClientDocument.Length - sExtension.Length) & ".docx"

            m_lReturn = m_oDocManager.ConvertDocumentUsingSiriusDocumentUtility(v_sSourceDocument:=m_sClientDocument, v_sDestDocument:=sArchiveDoc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" Failed to convert the following document to PDF : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return m_lReturn
            End If
        End If


        'Sharepoint Integration
        If m_oSharePoint Is Nothing Then
            m_oSharePoint = New bSIRSharepoint.Business()

            m_lReturn = m_oSharePoint.Initialise(MainModule.g_oObjectManager.UserName, MainModule.g_oObjectManager.Password, g_iUserID, g_iSourceID, g_iLanguageID, 0, 0, m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the m_oSharePoint object", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If
        m_lReturn = m_oSharePoint.ArchiveDocument(PartyCnt:=m_lPartyCnt, InsuranceFileCnt:=m_lInsuranceFileCnt, _
                                      ClaimID:=m_lClaimCnt, CaseID:=0, DocumentTemplateId:=m_lDocumentTemplateId, _
                                      TemplateGroupID:=m_lTemplateGroupID, _
                                      TemplateSubGroupID:=m_lTemplateSubGroupID, _
                                      SourceFile:=sArchiveDoc, InternalOnly:=m_bIsInternalOnly, SharepointPath:=sArchiveDoc, _
                                      DestinationFilename:=v_sDescription & "." & sPageType, _
                                      PartyCode:="", PolicyNumber:="", _
                                      ClaimNumber:="")

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ArchiveDocument the SharePoint ", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
    'KB 17/06/2003 PN 4264 1.8.5 -> 1.8.6 catchup
    ' ***************************************************************** '
    ' Name: ArchiveDocumentSilent
    '
    ' Description: Created from PrintDocumentSilent, in order to archieve documents in silent mode.
    '
    '
    ' ***************************************************************** '
    Public Function ArchiveDocumentSilent() As Integer
        Dim result As Integer = 0
        Dim vInsuranceFolderCnt As String = ""
        Dim vInsuranceFileCnt As String = ""
        Dim vClaimCnt As String = ""
        Dim iOptionNumber As Integer
        Dim sOptionValue As String = ""
        Dim lDocNumber, lEventTypeId As Integer
        Dim sDescription As String = ""

        Dim oEvent As bSIREvent.Business
        Dim vPartyPolicy As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add it to DocuMaster, returning the document cnt

            'First check if DocuMaster is installed

            iOptionNumber = 10 ' possibly use a set of constants?


            m_lReturn = m_oBusiness.getOption(v_iOptionNumber:=iOptionNumber, r_sOptionValue:=sOptionValue)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sOptionValue.Trim() <> "1") Then
                ' SET 27/11/2003 ISS7861 - documaster not installed so just exit...
                Return result
            End If

            ' Set Insurance File Cnt
            'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup
            If m_lInsuranceFileCnt <= 0 Then

                vInsuranceFileCnt = Nothing
            Else
                ' Only valid insurance file cnt
                vInsuranceFileCnt = CStr(m_lInsuranceFileCnt)

                ' RAM20031008 : Check if we have an insurance folder cnt
                '               If not, fetch the Insurance Folder Cnt
                If m_lInsuranceFolderCnt = 0 Then

                    m_lReturn = m_oBusiness.GetPartyPolicy(r_vArray:=vPartyPolicy, m_lInsuranceFileCnt:=m_lInsuranceFileCnt, m_lPartyCnt:=m_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch Insurance Folder Cnt for Insurance File : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocumentSilent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                    ' Array contains Insurance_ref and Insurance folder cnt
                    If m_sInsuranceFileRef.Trim().Length = 0 Then

                        m_sInsuranceFileRef = CStr(vPartyPolicy(1))
                    End If


                    m_lInsuranceFolderCnt = CInt(vPartyPolicy(2))

                End If

            End If
            'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup

            ' Set Insurance Folder Cnt
            If m_lInsuranceFolderCnt = 0 Then

                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = CStr(m_lInsuranceFolderCnt)
            End If

            If m_lClaimCnt = 0 Then

                vClaimCnt = Nothing
            Else
                vClaimCnt = CStr(m_lClaimCnt)
            End If

            If m_lEventCnt = 0 Then
                lEventTypeId = PMBConst.PMBEventDocument
            Else
                lEventTypeId = PMBConst.PMBEventTransaction
            End If

            If sDescription.Trim() = "" Then
                sDescription = m_sDocumentTemplateDescription
            Else
                sDescription = sDescription
            End If

            'DN 08/12/01 - Archive the document with the input description
            m_lReturn = UpdateFileMaster(lDocNumber:=lDocNumber, sDescription:=sDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now create the event...

            If lEventTypeId = PMBConst.PMBEventTransaction Then
                ' If it is a transaction, just amend the doc reference
                Dim temp_oEvent As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oEvent, "bSIREvent.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oEvent = temp_oEvent


                m_lReturn = oEvent.WriteTemplate(TemplateID:=lDocNumber, v_lEventCnt:=m_lEventCnt)


                oEvent.Dispose()
                oEvent = Nothing

            Else
                'eck1001001 pass description
                'Add the created event


                m_lReturn = m_oBusiness.CreateEvent(r_lEventCnt:=EventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=vInsuranceFolderCnt, v_vInsuranceFileCnt:=vInsuranceFileCnt, v_vClaimCnt:=vClaimCnt, v_vDocumentCnt:=lDocNumber, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DocumentTypeId, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=lEventTypeId, v_dtEventDate:=DateTime.Today, v_sDescription:="Raised Document - " & sDescription)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Event Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocumentSilent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ArchiveDocumentSilent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocumentSilent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'KB 17/06/2003 PN 4264 1.8.5 -> 1.8.6 catchup end



    ' ***************************************************************** '
    '
    ' Name: UpdateFileMaster
    '
    ' Description:
    '
    ' Edit History  :
    ' 03/09/1999 Tomo - Created.
    ' RAM20040609   : Bug fix for PN Issue 10321
    '                 1. Removed unwanted Dir Commands
    '                 2. Used bPMDocFunctions CopyFile instead of Standard FileCopy Function
    '                 3. Modified code to use CreateFolderTree instead of MkDir Function
    ' CJB20040818   : Bug fix for PN Issue 14209
    '                 Check new flag to see if to bypass the delete of the .doc file (flag
    '                 will be set to true in SpoolDocument as .doc file is used in there
    '                 on return from calling this (via ArchiveDocument function)
    ' ***************************************************************** '
    Private Function UpdateFileMaster(ByRef lDocNumber As Integer, ByRef sDescription As String) As Integer
        Dim result As Integer = 0
        Dim sClient, sDocType, sPageType, sDocName, sTemp As String ' DN 12/02/01
        Dim sServer As String = "" ' DN 12/02/01
        Dim sErrorMessage As String = "" ' RAM20040209
        Dim sDigitalSignature As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oSIRDOCAPI Is Nothing Then
            Dim temp_m_oSIRDOCAPI As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oSIRDOCAPI, "bSIRDOCAPI.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oSIRDOCAPI = temp_m_oSIRDOCAPI

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If

        If m_sClientDocument = String.Empty Then
            m_sClientDocument = m_sClient & "\Doc " & m_lDocumentTemplateId & "." & m_sDocFileExtension
        End If

        If m_bArchiveAsText Then
            sDocType = "T"
            sPageType = "TXT"
            sClient = m_sClient & "\Doc " & CStr(m_lDocumentTemplateId) & ".txt"
            If m_sClientDocument.EndsWith("htm") Then
                m_lReturn = gPMFunctions.ConvertHTMLToTxt(sInputFileName:=m_sClientDocument, r_sOutputFilename:=sClient)
            Else
                m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(m_sClientDocument, sClient)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Retrieve the system option to find whether to
            ' Digital Signature is required or not.
        ElseIf m_bArchiveAsXML Then
            sDocType = "S"
            sPageType = "XML"
            sClient = m_sClient & "\ResolvedXML_Doc " & CStr(m_lDocumentTemplateId) & ".xml"


        ElseIf m_bArchiveAsPDF Then

            sClient = m_sClient & "\Doc " & CStr(m_lDocumentTemplateId) & ".pdf"

            m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(m_sClientDocument, sClient)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else
            'DN 12/02/01 - First need to copy doc over to server
            sClient = m_sClient & "\Doc " & CStr(m_lDocumentTemplateId) & ".doc"
            m_sClientDocument = m_sClient & "\Doc " & CStr(m_lDocumentTemplateId) & ".xml"
            'DN 30/01/02 - Change document into Word format
            m_lReturn = SetDocumentVariables()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oDocument.SaveAs(FileName:=sClient, FileFormat:=0)

            m_oDocument.Close()

            m_oDocument = Nothing


            m_oWord.Application.Quit()
            'm_oWord = Nothing
        End If

        ' SET 27/06/2003 ISS4980 - get the location of the export directory

        'sTemp = CStr(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, CInt("SOFTWARE\PM\SiriusSolutions\Client"), CInt("PrntFileDir")))
        sTemp = CStr(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\PureInstallation\Client", "PrntFileDir"))
        If sTemp = "Not Found" Then
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to find the registry entry", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:="Unable to find the registry entry for the PrntFileDir directory location")

            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' create a temp directory
        sTemp = sTemp.Trim()
        If sTemp.EndsWith("\") Then
            sServer = sTemp & "DocArchiveTemp"
        Else
            sServer = sTemp & "\DocArchiveTemp"
        End If

        ' create a user specific directory
        sServer = sServer & "\" & g_oObjectManager.UserName.Trim()

        m_lReturn = CreateFolderTree(sServer)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Create Directory ( " & sServer & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

        If m_bArchiveAsText Then
            sServer = sServer & "\Doc " & CStr(m_lDocumentTemplateId) & ".txt"
        ElseIf m_bArchiveAsXML Then
            sServer = sServer & "\Doc " & CStr(m_lDocumentTemplateId) & ".xml"
        ElseIf m_bArchiveAsPDF Then
            sServer = sServer & "\Doc " & CStr(m_lDocumentTemplateId) & ".pdf"
        Else
            sServer = sServer & "\Doc " & CStr(m_lDocumentTemplateId) & ".doc"
        End If

        'Copy the document to the server
        ' RAM20040209 : Use bPMDocFunctions.CopyFile rather than FileCopy
        m_lReturn = bPMDocFunctions.CopyFile(sClient, sServer, True, False, sErrorMessage)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            ' Log Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file from Client To Server." & Strings.Chr(13) & Strings.Chr(10) & _
                               "Source File      : " & sClient & Strings.Chr(13) & Strings.Chr(10) & _
                               "Destination File : " & sServer & Strings.Chr(13) & Strings.Chr(10) & _
                               "Error Details    : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        'It's not used, but we need to define it anyway...
        Dim sKeywords(0) As String

        If m_bArchiveAsText Then
            sDocType = "T"
            sPageType = "TXT"
        ElseIf m_bArchiveAsXML Then
            sDocType = "S"
            sPageType = "XML"
        ElseIf m_bArchiveAsPDF Then
            sDocType = "F"
            sPageType = "PDF"
        Else
            sDocType = "D"
            sPageType = "DOC"
        End If

        'DN 08/12/01 - Trim description as DME can only handle desc of 50
        sDocName = sDescription.Substring(0, Math.Min(sDescription.Length, 50))

        'DJM 18/09/2003 : Pass in everything and let Documaster sort it out.
        'FSA 3.2 Pass Complaint details

        m_lReturn = m_oSIRDOCAPI.AddDocument(lPartyId:=m_lPartyCnt, sPartyName:=m_sPartyName, lInsuranceFolderId:=m_lInsuranceFolderCnt, sInsuranceFileRef:=m_sInsuranceFileRef, lClaimId:=m_lClaimCnt, sClaimRef:=m_sClaimRef, lFSAComplaintFolderCnt:=m_lFSAComplaintFolderCnt, sFSAComplaintReference:=m_sFSAComplaintReference, sDocType:=sDocType, sPageType:=sPageType, sDocName:=sDocName, sFilename:=sServer, sAnnotation:="", sKeywords:=sKeywords, lDocNumber:=lDocNumber, bArchiveAsText:=m_bArchiveAsText, vDocumentTemplateID:=m_lDocumentTemplateId, bArchiveAsXML:=m_bArchiveAsXML)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            MessageBox.Show("Failed to Archive Document  - " & sDocName, "UpdateFileMaster", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Archive Document", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End If

        ' CJB20040818 : Bug fix for PN Issue 14209 - Bypass delete if necessary
        If Not m_bBypassDocDelete Then

            ' RAM20040209 : Bug fix for PN Issue 10231
            m_lReturn = bPMDocFunctions.DeleteFile(sClient)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete [" & sClient & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: OurInstanceOfWordIsRunning
    '
    ' Description: Checks to see if the instance of word we created to
    '               edit or print a document is still running.
    '
    ' History: 03/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function OurInstanceOfWordIsRunning() As Integer
        Dim sTest As String = ""

        Try

            'TN20010711 - start

            'is our word still running?
            m_lReturn = bPMDocFunctions.IsWindow(m_lWordHwnd)


            'TN20010711 - end

            If m_lReturn = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try




        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    '
    ' Name: OurDocIsRunning
    '
    ' Description: Checks to see if our document is still open. Does this by
    '               trying to access ActiveDocument property of module level
    '               Word object. If an error occurs, we assume document has
    '               already been closed down.
    '
    ' History:  01/08/2000 RWH - Created.
    '           05/02/2002 DN - Changed logic as using ActiveWindow didn't always work.
    ' ***************************************************************** '
    Private Function OurDocIsRunning() As Integer
        Dim result As Integer = 0
        Dim sTest As String = ""
        Dim iDocNum As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If Not (m_oWord Is Nothing) Then


                iDocNum = m_oWord.Documents.Count

                For iCount As Integer = 1 To iDocNum

                    sTest = m_oWord.Documents.Item(iCount).FullName
                    'DN 25/02/02 - Ensure case is the same
                    If sTest.ToLower() = m_sClientDocument.ToLower() Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                    End If
                Next

            End If

            Return result

        Catch
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function ShutItDown() As Integer
        Dim result As Integer = 0
        Dim sTemp As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'TN20010711 - start
            'is our word session still open
            m_lReturn = bPMDocFunctions.IsWindow(m_lWordHwnd)

            'yeap its still open
            If m_lReturn <> 0 Then
                'do we have any document open
                If OurDocIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then

                    m_oWord.Documents.Close(SaveChanges:=True)
                End If

                ' SET 18/10/2004 ISS13245
                m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
            End If

            'close down document first if its open


            '    'RWH(01/08/2000)
            '    If (OurDocIsRunning = PMTrue) Then
            '        m_oWord.Documents.Close SaveChanges:=True
            '        m_oWord.Application.Quit
            '        Set m_oWord = Nothing
            '    End If
            'TN20010711 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to shut it down", vApp:=ACApp, vClass:=ACClass, vMethod:="ShutItDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetClient
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    '          MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup
    ' ***************************************************************** '
    Private Function GetClient() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_sClient.Trim() > "" Then
            Return result
        End If

        m_lReturn = GetClientDirectory(m_sClient, True)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_bUniqueClientDirNeedsDeleting = True
            Return result
        End If

        m_bUniqueClientDirNeedsDeleting = False

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetServer
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    '
    ' ***************************************************************** '
    Private Function GetServer() As Integer

        Dim result As Integer = 0
        Dim sServer As String = ""

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_sServer.Trim() > "" Then
            Return result
        End If

        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

        sServer = ""

        m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DocServer", r_sSettingValue:=sServer)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Server from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            m_sServer = sServer
        End If

        Return result

    End Function






    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.




        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Initialise the error number value.
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

        g_oZipper = New bPMZipper.Business()

        If g_oObjectManager Is Nothing Then
            g_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

        End If

        ' Get an instance of the business object via
        ' the public object manager.
        Dim temp_m_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Display error stating the problem.

            ' Get description from the resource file.


            'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display message.
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

            Exit Sub
        End If

        ' Create an instance of the general interface object.
        m_oGeneral = New iPMBDocTemplate.General()

        ' Call the initialise method passing this interface
        ' and the business object as parameters.
        m_lReturn = m_oGeneral.InitialiseFormless(frmInterface:=Me, oBusiness:=m_oBusiness)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            Exit Sub
        End If

        ' Create an instance of the form control object.


        ' Set language
        m_oFormFields.LanguageID = g_iLanguageID


        m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency


        m_bChaserLettersEnabled = m_oBusiness.ChaserLettersEnabled


        m_oBusiness.CallingAppName = ACApp

        m_bEditted = False

        bPMDocFunctions.Username = g_oObjectManager.UserName.Trim()

        ' Set the interface status to cancelled. This is done
        ' so that any interface termination will be noted
        ' as cancelled except in the event of accepting
        ' the interface.
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                DeleteHTMDependecies()
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                'DC120304 PN10583 -copied from further down to give Zipper more time to unlock directory
                ' Alix - 22/01/2004 - PN9565
                ' Delete Zipper object before we delete the folder, as the folder is
                ' locked by the Zipper!
                If Not (g_oZipper Is Nothing) Then
                    g_oZipper = Nothing
                End If

                ' Remove the Hdoc first
                Hdoc = Nothing
                If m_oGeneral IsNot Nothing Then
                    m_oGeneral.Dispose()

                End If
                m_oGeneral = Nothing
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()

                End If
                m_oBusiness = Nothing
                If m_oSIRDOCAPI IsNot Nothing Then
                    m_oSIRDOCAPI.Dispose()
                    m_oSIRDOCAPI = Nothing
                End If
                If m_oDocSpooler IsNot Nothing Then
                    m_oDocSpooler.Dispose()
                    m_oDocSpooler = Nothing
                End If


                m_vClientArray = Nothing
                m_vPolicyArray = Nothing
                m_vClaimArray = Nothing
                m_vDocumentTypeArray = Nothing
                m_vSourceArray = Nothing
                m_vRiskBySource = Nothing
                m_vRiskGroupBySource = Nothing

                ' Alix - 22/01/2004 - PN9565
                ' Delete Zipper object before we delete the folder, as the folder is
                ' locked by the Zipper!
                If Not (g_oZipper Is Nothing) Then
                    g_oZipper = Nothing
                End If
                'Microsoft
                Application.DoEvents() ' I don't know why, but it doesn't unlock the folder quick
                Application.DoEvents() ' enougn without these DoEvents.
                ' /Alix
                If m_oDocManager IsNot Nothing Then
                    m_oDocManager.Dispose()
                    m_oDocManager = Nothing
                End If
                If m_bUniqueClientDirNeedsDeleting Then
                    'Allow other processes to shutdown and release the temp files
                    'Microsoft
                    Application.DoEvents()
                    Application.DoEvents()

                    DelDirectory(m_sClient)


                    m_sClient = ""


                End If
                'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup END

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            End If
        End If
        Me.disposedValue = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    ' ***************************************************************** '
    '
    ' Name: GetClauseNumbersInDoc
    '
    ' Description: Searches thru' document line by line, compiling list
    '               of clauses called.
    '
    ' History: 22/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GetClauseNumbersInDoc() As Integer
        Dim result As Integer = 0
        Dim iFileNum, iClauseStart As Integer
        Dim sCurrentLine, sClauseStartMarker, sClauseCode As String
        Dim iClauseEnd As Integer
        Dim sDoc As String = ""
        Dim bAddClauseToArray As Boolean



        ReDim m_vDocClauseLinkArray(0)

        result = gPMConstants.PMEReturnCode.PMTrue

        sDoc = m_sClient & "\Doc " & CStr(m_lDocumentTemplateId) & "." & m_sDocFileExtension

        sClauseStartMarker = m_sFieldStartMarker & "CL_"

        ' Open the chosen template document
        iFileNum = FileSystem.FreeFile()
        FileSystem.FileOpen(iFileNum, sDoc, OpenMode.Input)

        'Read in each line of document and analyse 1 by 1.
        Do While Not FileSystem.EOF(iFileNum)

            sCurrentLine = FileSystem.LineInput(iFileNum)

            'Debug.Print sCurrentLine

            sClauseStartMarker = m_sFieldStartMarker & "CL_"
            'Check for Clause field present on this line.
            iClauseStart = (sCurrentLine.IndexOf(sClauseStartMarker) + 1)
            '        If (iFieldStart <> 0) Then
            If iClauseStart <> 0 Then
                'If clause is present then extract number.
                sClauseCode = sCurrentLine.Substring(iClauseStart + sClauseStartMarker.Length - 1)
                iClauseEnd = (sClauseCode.IndexOf(m_sFieldEndMarker) + 1)
                sClauseCode = sClauseCode.Substring(0, iClauseEnd - 1)

                '+++ JJ 15/08/2003 IR 5011
                If sClauseCode.IndexOf("_"c) + 1 Then
                    sClauseCode = Mid(sClauseCode, (sClauseCode.IndexOf("_"c) + 1) + 1)
                End If

                bAddClauseToArray = True

                If Information.IsArray(m_vDocClauseLinkArray) Then
                    ReDim Preserve m_vDocClauseLinkArray(m_vDocClauseLinkArray.GetUpperBound(0) + 1)
                Else
                    ReDim m_vDocClauseLinkArray(0)
                End If

                For Each m_vDocClauseLinkArray_item As Object In m_vDocClauseLinkArray
                    If CStr(m_vDocClauseLinkArray_item) = sClauseCode Then
                        bAddClauseToArray = False
                    End If
                Next m_vDocClauseLinkArray_item

                If bAddClauseToArray Then
                    m_vDocClauseLinkArray(m_vDocClauseLinkArray.GetUpperBound(0)) = sClauseCode
                End If

            End If


            sClauseStartMarker = m_sFieldStartMarker & "STANDARDWORDINGS_CL_"
            'Check for Clause field present on this line.
            iClauseStart = (sCurrentLine.IndexOf(sClauseStartMarker) + 1)
            '        If (iFieldStart <> 0) Then
            If iClauseStart <> 0 Then
                'If clause is present then extract number.
                sClauseCode = sCurrentLine.Substring(iClauseStart + sClauseStartMarker.Length - 1)
                iClauseEnd = (sClauseCode.IndexOf(m_sFieldEndMarker) + 1)
                sClauseCode = sClauseCode.Substring(0, iClauseEnd - 1)

                '+++ JJ 15/08/2003 IR 5011
                If sClauseCode.IndexOf("_"c) + 1 Then
                    sClauseCode = Mid(sClauseCode, (sClauseCode.IndexOf("_"c) + 1) + 1)
                End If

                bAddClauseToArray = True

                If Information.IsArray(m_vDocClauseLinkArray) Then
                    ReDim Preserve m_vDocClauseLinkArray(m_vDocClauseLinkArray.GetUpperBound(0) + 1)
                Else
                    ReDim m_vDocClauseLinkArray(0)
                End If

                For i As Integer = m_vDocClauseLinkArray.GetLowerBound(0) To m_vDocClauseLinkArray.GetUpperBound(0)
                    If CStr(m_vDocClauseLinkArray(i)) = sClauseCode Then
                        bAddClauseToArray = False
                    End If
                Next i

                If bAddClauseToArray Then
                    m_vDocClauseLinkArray(m_vDocClauseLinkArray.GetUpperBound(0)) = sClauseCode
                End If
                '--- JJ 15/08/2003 IR 5011

            End If





        Loop

        FileSystem.FileClose(iFileNum)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateTemplateNumberAndDependencies
    '
    ' Description: Originally done as part of RSAIB Process 108.
    '               Renames main template. Moves dependency files to new
    '               directory using new template number. Also hacks the
    '               template and dependency xml file replacing all
    '               references to old dependency directory.
    '
    ' Edit History  :
    ' 01/09/2000 RWH - Created.
    ' RAM20040209    - Bug fix for PN Issue 10231
    '                  1. Removed unwanted Dir Command as it locks the directory
    '                  2. Removed RmDir and modify code to use CreateFolderTree
    '                  3. Modified code to use CopyFolder rather than individual
    '                       filecopy (name function)
    '                  4. Modified code to use IsFileExists function, instead of Dir
    ' ***************************************************************** '
    Private Function UpdateTemplateNumberAndDependencies(ByRef lOldId As Integer, ByRef lNewId As Integer) As Integer
        Dim result As Integer = 0
        Dim sOldClient, sNewClient, sParentFile, sDependencyDir, sNewDependencyDir As String
        Dim iFileNumIn, iFileNumOut As Integer
        Dim sTempFile, sCurrentLine As String
        Dim iPos As Integer
        Dim sReplacement, sXML_ListFile As String

        Dim sLogFilename As String = "" 'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup
        Dim sTemp As String = "" 'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup

        Dim sOldDocRef As String = "Doc%20" & lOldId 'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RWH(01/09/2000) - RSAIB Process 108. Rename template to correct
            'number now.
            sOldClient = m_sClient & "\Doc " & CStr(lOldId) & "." & m_sDocFileExtension
            Debug.WriteLine(sOldClient)

            sNewClient = m_sClient & "\Doc " & CStr(lNewId) & "." & m_sDocFileExtension
            Debug.WriteLine(sNewClient)

            If sOldClient = sNewClient Then
                Return result
            End If

            FileSystem.Rename(sOldClient, sNewClient)

            'Check for dependencies and rename directory.
            sParentFile = sOldClient.Substring(0, sOldClient.Length - 4)
            sDependencyDir = sParentFile & "_files"


            If IsFolderExists(sDependencyDir) = gPMConstants.PMEReturnCode.PMTrue And m_sDocFileExtension.ToUpper() <> "XML" Then

                'Create directory of correct name.
                sNewDependencyDir = sNewClient.Substring(0, sNewClient.Length - 4) & "_files"

                m_lReturn = CreateFolderTree(sNewDependencyDir)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to create [" & sNewDependencyDir & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'Move all dependencies to correct directory.
                m_lReturn = CopyFolder(sDependencyDir, sNewDependencyDir)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Copy Folder." & Strings.Chr(13) & Strings.Chr(10) & _
                                       "From [" & sDependencyDir & "] " & Strings.Chr(13) & Strings.Chr(10) & _
                                       "To [" & sNewDependencyDir & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'Remove original directory.
                m_lReturn = DelDirectory(sDependencyDir)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete [" & sDependencyDir & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'Analyse template source and replace references to old dependency directory with new.
                sTempFile = sNewClient.Substring(0, sNewClient.Length - 3) & "tmp"

                sReplacement = sOldDocRef.Substring(0, sOldDocRef.Length - 1) & CStr(m_lDocumentTemplateId)

                iFileNumIn = FileSystem.FreeFile()
                FileSystem.FileOpen(iFileNumIn, sNewClient, OpenMode.Input)

                iFileNumOut = FileSystem.FreeFile()
                FileSystem.FileOpen(iFileNumOut, sTempFile, OpenMode.Output)

                Do While Not FileSystem.EOF(iFileNumIn)
                    sCurrentLine = FileSystem.LineInput(iFileNumIn)
                    iPos = (sCurrentLine.IndexOf(sOldDocRef) + 1)
                    If iPos > 0 Then
                        sCurrentLine = sCurrentLine.Substring(0, iPos + (sOldDocRef.Length - 2)) & CStr(m_lDocumentTemplateId) & Mid(sCurrentLine, iPos + (sOldDocRef.Length))
                    End If

                    FileSystem.PrintLine(iFileNumOut, sCurrentLine)

                Loop

                FileSystem.FileClose(iFileNumIn)
                FileSystem.FileClose(iFileNumOut)

                sLogFilename = "Kill1 " & sNewClient

                m_lReturn = bPMDocFunctions.DeleteFile(sNewClient) ' RAM20040209 : Bug fix for PN Issue 10231

                sLogFilename = "FileCopy1 " & sNewClient
                ' TB 151102: PMFileCopy returns message with failure reasons
                m_lReturn = PMCopyFile.PMFileCopy(sTempFile, sNewClient, m_sFileCopyMsg)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update template number" & _
                                       Strings.Chr(13) & Strings.Chr(10) & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
                '        FileCopy sTempFile, sNewClient

                sLogFilename = "Kill2 " & sTempFile

                m_lReturn = bPMDocFunctions.DeleteFile(sTempFile) ' RAM20040209 : Bug fix for PN Issue 10231

                'Now do xml list file.
                sXML_ListFile = sNewDependencyDir & "\filelist.xml"
                sTempFile = sXML_ListFile.Substring(0, sXML_ListFile.Length - 3) & "tmp"

                'Make sure the XML File is there
                If IsFileExists(sXML_ListFile) = gPMConstants.PMEReturnCode.PMTrue Then

                    iFileNumIn = FileSystem.FreeFile()
                    FileSystem.FileOpen(iFileNumIn, sXML_ListFile, OpenMode.Input)

                    iFileNumOut = FileSystem.FreeFile()
                    FileSystem.FileOpen(iFileNumOut, sTempFile, OpenMode.Output)

                    Do While Not FileSystem.EOF(iFileNumIn)
                        sCurrentLine = FileSystem.LineInput(iFileNumIn)
                        iPos = (sCurrentLine.IndexOf(sOldDocRef) + 1)
                        If iPos > 0 Then
                            sCurrentLine = sCurrentLine.Substring(0, iPos + (sOldDocRef.Length - 2)) & CStr(m_lDocumentTemplateId) & Mid(sCurrentLine, iPos + (sOldDocRef.Length))
                        End If

                        FileSystem.PrintLine(iFileNumOut, sCurrentLine)

                    Loop

                    FileSystem.FileClose(iFileNumIn)
                    FileSystem.FileClose(iFileNumOut)

                    sLogFilename = "Kill3 " & sXML_ListFile

                    m_lReturn = bPMDocFunctions.DeleteFile(sXML_ListFile) ' RAM20040209 : Bug fix for PN Issue 10231

                    sLogFilename = "FileCopy2 " & sTempFile
                    ' TB 151102: PMFileCopy returns message with failure reasons
                    m_lReturn = PMCopyFile.PMFileCopy(sTempFile, sXML_ListFile, m_sFileCopyMsg)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update Temnplate Number" & _
                                           Strings.Chr(13) & Strings.Chr(10) & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                    '        FileCopy sTempFile, sXML_ListFile

                    sLogFilename = "Kill4 " & sTempFile

                    m_lReturn = bPMDocFunctions.DeleteFile(sTempFile) ' RAM20040209 : Bug fix for PN Issue 10231

                End If
            End If

            Return result

        Catch excep As System.Exception



            If Information.Err().Number = 53 Then
                'retain sLogFilename
            Else
                sLogFilename = "Not Applicable"
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTemplateNumber Failed. Filename: " & sLogFilename, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    Private Function CopyFilesToZipTemp() As Integer
        Dim result As Integer = 0
        Dim sClient, sDependencyDir, sParentFile, sDepDirName As String

        Dim sSourceFile, sDestinationFile As String
        Dim lFileCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'Copy files to ZipTemp.
        'This gives us an absolute directory to zip & unzip to as apposed to the
        'client-specific processing directory.

        lFileCount = NoOfFilesInDirectory(m_sClient, sClient, m_sDocFileExtension)
        If lFileCount > 0 Then

            'RWH(18/10/2000) If we are using Word 97 we need to break
            'browsers link to our document.
            If m_sWordVersion.Substring(0, 1) = "8" Then

                WebBrowser1.Navigate(New Uri("about:blank"))
                WebBrowser1.Refresh()
                Application.DoEvents()
            End If

            'Copy parent file to ZipTemp.
            sSourceFile = m_sClient & "\" & sClient
            sDestinationFile = m_sZIP_DIRECTORY & "\" & sClient

            m_lReturn = bPMDocFunctions.CopyFile(sSourceFile, sDestinationFile, True, True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Copy File." & Strings.Chr(13) & Strings.Chr(10) & _
                                   "From [" & sSourceFile & "] " & Strings.Chr(13) & Strings.Chr(10) & _
                                   "To [" & sDestinationFile & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesToZipTemp", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            'Check for dependencies and copy them to the temp zip directory if they exist.
            sParentFile = m_sClient & "\" & sClient
            sParentFile = sParentFile.Substring(0, sParentFile.Length - 4)

            sDependencyDir = sParentFile & "_files"

            If IsFolderExists(sDependencyDir) = gPMConstants.PMEReturnCode.PMTrue Then

                sDepDirName = m_sZIP_DIRECTORY & "\" & sClient.Substring(0, sClient.Length - 4) & "_files"

                m_lReturn = CreateFolderTree(sDepDirName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to create [" & sDepDirName & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesToZipTemp", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If


                'Move all dependencies to correct directory.
                m_lReturn = CopyFolder(sDependencyDir, sDepDirName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Copy Folder." & Strings.Chr(13) & Strings.Chr(10) & _
                                       "From [" & sDependencyDir & "] " & Strings.Chr(13) & Strings.Chr(10) & _
                                       "To [" & sDepDirName & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesToZipTemp", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'Remove original directory.
                m_lReturn = DelDirectory(sDependencyDir)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete [" & sDependencyDir & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesToZipTemp", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If


            End If

        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: OpenDocument
    '
    ' Description:
    '
    ' History: 28/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function OpenDocument() As Integer
        Dim result As Integer = 0
        Dim dtPause As Date
        Dim sOptionValue As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Alix - 31/10/2002
        If m_sWordVersion.Substring(0, 1) = "8" Then

            ' Avoid message when saving document

            m_oWord.DefaultSaveFormat = "HTML"

            ' Open document, specifying HTML as default opening format

            m_oDocument = m_oWord.Documents.Open(FileName:=m_sClientDocument, Format:=11)

        Else


            m_oDocument = m_oWord.Documents.Open(m_sClientDocument, ConfirmConversions:=False)

        End If

        'Some documents in Word 2000 or less require a pause after opening in order to allow them to fully open
        If CLng(Strings.Left(m_sWordVersion, m_sWordVersion.IndexOf("."))) <= 9 Then
            'The length of the pause is determined by a system option
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5036, r_sOptionValue:=sOptionValue)

            If IsNumeric(sOptionValue) Then
                If CInt(sOptionValue) > 0 Then
                    dtPause = DateTime.Now.AddSeconds(CInt(sOptionValue))

                    Do While DateTime.Now < dtPause
                        Sleep(500)
                        Application.DoEvents()
                    Loop
                End If
            End If
        End If

        'PSL 24/09/2003 Issue 6085


        'm_oWord.CommandBars("Standard").Visible = True

        'm_oWord.CommandBars("Formatting").Visible = True

        Application.DoEvents()

        Return result

    End Function

    Private Function SetZipDirectory() As Integer
        Dim result As Integer = 0
        Dim sTemp As String



        result = gPMConstants.PMEReturnCode.PMTrue
        If m_sZIP_DIRECTORY <> "" Then
            Return result
        End If

        ' SET 20/06/2003 ISS4571 - Fix for document locking
        'Get the DocZipTemp dir

        'sTemp = CStr(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, CInt("SOFTWARE\PM\SiriusSolutions\Client"), CInt("DocZipPMDir")))
        sTemp = CStr(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\PureInstallation\Client", "DocZipPMDir"))

        'Make sure we have an install path
        If sTemp = "Not Found" Then
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetZipDirectory", vErrNo:=Information.Err().Number, vErrDesc:="Unable to find the registry entry for the doczip directory location")

            Return gPMConstants.PMEReturnCode.PMError
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20030612 : To Fix the document locking issue,  We are creating
        '               individual directory for every user
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Create the directory name
        m_sZIP_DIRECTORY = sTemp & "\" & g_oObjectManager.UserName.Trim()

        ' RAM20040204 : Bug Fix for PN Issue 10321. Use FSO, rather than DIR command
        m_lReturn = CreateFolderTree(m_sZIP_DIRECTORY, True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetZipDirectory", vErrNo:=0, vErrDesc:="Unable to create the directory (" & m_sZIP_DIRECTORY & ")")

            Return result
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20030612 : END
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return result

    End Function


    ' AK 110402
    ' Function to check the existance of work manager task and create them
    ' Doc production in print/print-silent/spool mode will use this

    Public Function CreateTask() As Integer
        Dim result As Integer = 0
        Try
            Dim oTaskBusiness As bSIRDocTemplate.Business
            Dim vTaskId As Object
            Dim oWorkManager As Object
            Dim lInstanceID As Integer
            Dim vPartyType As Object
            Dim lPMWrkTaskGroupId, lPMWrkTaskId As Integer
            Dim sCustomer As String = ""
            Dim dtTaskDueDate As Date
            Dim lPMUserGroupID As Integer
            Dim iUserId As Integer
            Dim sDescription As String = ""
            Dim iTaskStatus, iIsUrgent As Integer
            Dim dtDateCreated As Date
            Dim iCreatedByID As Integer
            Dim dtLastModified As Date
            Dim iModifiedByID As Integer

            'AK 220402
            Dim iNumDays As Integer


            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetTaskID(r_vTaskId:=vTaskId, m_lDocumentTemplateId:=m_lDocumentTemplateId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oTaskBusiness = Nothing
                Return result
            End If

            If Not Information.IsArray(vTaskId) Then
                oTaskBusiness = Nothing
                Return result
            End If


            If CStr(vTaskId(0, 0)) = "" Then
                oTaskBusiness = Nothing
                Return result
            End If


            m_lReturn = m_oBusiness.GetPartyPolicy(r_vArray:=m_vPartyPolicy, m_lInsuranceFileCnt:=InsuranceFileCnt, m_lPartyCnt:=PartyCnt)

            oTaskBusiness = Nothing

            m_lReturn = g_oObjectManager.GetInstance(oWorkManager, "bPMWrkTaskInstanceTemp.FormClass", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            'Get the task related details

            m_lReturn = oWorkManager.GetDetails(v_lPMWrkTaskInstanceCnt:=vTaskId(0, 0), r_lPMWrkTaskGroupID:=lPMWrkTaskGroupId, r_lPMWrkTaskID:=lPMWrkTaskId, r_sCustomer:=sCustomer, r_dtTaskDueDate:=dtTaskDueDate, r_lPMUserGroupID:=lPMUserGroupID, r_iUserID:=iUserId, r_sDescription:=sDescription, r_iTaskStatus:=iTaskStatus, r_iIsUrgent:=iIsUrgent, r_dtDateCreated:=dtDateCreated, r_iCreatedByID:=iCreatedByID, r_dtLastModified:=dtLastModified, r_iModifiedByID:=iModifiedByID)

            m_lReturn = m_oBusiness.GetPartyType(r_vPartytype:=vPartyType, v_lPartyCnt:=PartyCnt)

            'AK 220402 - get the number of days between - TaskDueDate and DateCreated
            'ED 31102002 - Reversed as originally returning negative
            iNumDays = DateAndTime.DateDiff("d", dtDateCreated, dtTaskDueDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
            'Set the customer like this,  as being done by the interface
            sCustomer = CStr(m_vPartyPolicy(1)) & " - " & CStr(m_vPartyPolicy(2))
            dtDateCreated = DateTime.Today
            'AK 220402 - set it to the number of days difference, calculated earlier
            dtTaskDueDate = DateTime.Today.AddDays(iNumDays).AddDays(CDate("23:59:59").ToOADate())

            m_lReturn = oWorkManager.CreateNew(r_lPMWrkTaskInstanceCnt:=lInstanceID, v_lPMWrkTaskGroupID:=lPMWrkTaskGroupId, v_lPMWrkTaskID:=lPMWrkTaskId, v_sCustomer:=sCustomer, v_dtTaskDueDate:=dtTaskDueDate, v_lPMUserGroupID:=lPMUserGroupID, v_iUserID:=iUserId, v_sDescription:=sDescription, v_iTaskStatus:=iTaskStatus, v_iIsUrgent:=iIsUrgent, v_dtDateCreated:=dtDateCreated, v_iCreatedByID:=iCreatedByID, v_bIsNewInstance:=True)

            'Create the task instance keys now
            m_lReturn = oWorkManager.CreateKeys(v_lTaskInstanceID:=lInstanceID, v_lPartyCnt:=PartyCnt, v_sShortName:=m_vPartyPolicy(1), v_sPartyType:=CStr(vPartyType(0, 0)).Trim(), v_sResolvedName:=CStr(vPartyType(1, 0)).Trim(), v_lInsuranceFileCnt:=InsuranceFileCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oWorkManager.Dispose()
                oWorkManager = Nothing
                Return result
            End If

            oWorkManager.Dispose()
            oWorkManager = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Public Function SendToFish() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sMessage As String = ""

            If m_bFishSent Then
                Return result
            End If

            m_bFishSent = True

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendToFish Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToFish", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : DeleteTaskTemplate
    ' Description   : Remove Task From the DocumentTemplate
    ' Edit History  :
    ' RAM20031118   : Created
    ' RAM20031118   : PN Issue 4697 Changes
    ' ***************************************************************** '
    Private Function DeleteTaskTemplate(ByVal v_lPMWrkTaskTempCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim oWrkManager As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_lPMWrkTaskTempCnt > 0 Then

            Dim temp_oWrkManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWrkManager, "bPMWrkTaskInstanceTemp.FormClass", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oWrkManager = temp_oWrkManager

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get 'bPMWrkTaskInstanceTemp.FormClass' component.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTaskTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Delete it physically from the database

            m_lReturn = oWrkManager.DeleteTemplate(PMWrkTaskInstTempCnt:=v_lPMWrkTaskTempCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to detete the Task Template associated with the Document Template", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTaskTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Clear the memory
            oWrkManager = Nothing

        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdatePartyTobLetter
    '
    ' Description: FSA Phase III
    '
    ' History: 03/11/2004 Elaine Knott.
    '
    ' ***************************************************************** '
    Public Function UpdatePartyTobLetter() As Integer
        Dim result As Integer = 0
        Dim oParty As bSIRParty.Business
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sMessage As String = ""

            Dim temp_oParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oParty, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oParty = temp_oParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get Instance of bSIRParty.", "UpdatePartyTobLetter")
            End If


            m_lReturn = oParty.UpdateTobLetter(lPartyCnt:=m_lPartyCnt)

            'Inform user if failed to complete.
            'OK to continue

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to Update party Tob Letter.", "UpdatePartyTobLetter")
            End If


            oParty.Dispose()

            oParty = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyTobLetter", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePartyTobLetter", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    Private Function CheckFileTypeIsHtml() As Integer
        Dim result As Integer = 0
        Dim sFile As String = ""
        Dim lFileCount, lFileNum As Integer
        Dim sLine As String = ""


        result = gPMConstants.PMEReturnCode.PMFalse

        'DJM 02/09/2002 : Convert files in the zip directory not in the client directory
        'RAM20040205    : Use FSO rather than the Dir Commands
        '                 Use m_sClient instead of m_sZIP_DIRECTORY, since that is the working directory
        lFileCount = NoOfFilesInDirectory(v_sDirectoryName:=m_sClient, r_vFirstFileName:=sFile)
        Select Case lFileCount
            Case Is > 1
                ' Too many files
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Too Many Files in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            Case 0
                ' No Files Found
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            Case 1
                ' Only one file found, so check the type of the file

                lFileNum = FileSystem.FreeFile()
                FileSystem.FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)
                sLine = FileSystem.InputString(lFileNum, 5)
                FileSystem.FileClose(lFileNum)
                Select Case sLine.ToUpper()
                    Case "<HTML"
                        result = gPMConstants.PMEReturnCode.PMTrue
                End Select

            Case Else
                ' Some other no, so error

                ' No Files Found
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
        End Select

        Return result

    End Function
    Private Function AddFailedEmailWorkManagerTask() As Integer
        Dim result As Integer = 0
        Dim bPMWrkTaskInstance As Object

        Const kMethodName As String = "AddFailedEmailWorkManagerTask"

        Dim lTaskInstanceCnt As Integer

        Dim oTaskControl As bPMWrkTaskInstance.TaskControl
        Dim sGroupId As String = ""
        Dim vClientCode, vPmWrkTaskId As Object

        Const lSIROptionFailedEmailWorkManagerTask As Integer = 5068
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oTaskControl As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oTaskControl, "bPMWrkTaskInstance.TaskControl", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oTaskControl = temp_oTaskControl
        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            gPMFunctions.RaiseError(kMethodName, "Failed To create the new task", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=lSIROptionFailedEmailWorkManagerTask, r_sOptionValue:=sGroupId, v_iSourceID:=g_iSourceID)
        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            gPMFunctions.RaiseError(kMethodName, "Failed To create the new task", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Not String.IsNullOrEmpty(sGroupId) And sGroupId <> "" Then

            m_lReturn = m_oBusiness.GetClientCode(v_iPartyID:=m_lPartyCnt, r_vClientarray:=vClientCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get party code", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = m_oBusiness.GetPMWrkTaskID(v_sTaskCode:="MEMO", r_vTaskId:=vPmWrkTaskId)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get task Id for code MEMO", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Sankar - PN - As Per Richard, modified TaskGroupID to 5 and UserGroupID to sGroupId


            m_lReturn = oTaskControl.CreateNew(v_lPMWrkTaskGroupID:=5, v_lPMWrkTaskID:=gPMFunctions.ToSafeLong(vPmWrkTaskId(0, 0)), v_sCustomer:="Email Document Failed", v_dtTaskDueDate:=DateTime.Now, v_lPMUserGroupID:=gPMFunctions.ToSafeLong(sGroupId), v_sDescription:="Attempt to email '" & m_sDocumentTemplateDescription & "' to party '" & _
                        gPMFunctions.ToSafeString(CStr(vClientCode(0, 0))) & "' failed. No Main Email address present", v_iTaskStatus:=0, v_iIsUrgent:=1, r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_iIsVisible:=gPMConstants.PMEReturnCode.PMTrue)


            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                gPMFunctions.RaiseError(kMethodName, "Failed to cerate new task ", gPMConstants.PMELogLevel.PMLogError)
            End If
        Else
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed Emaild User Group not Configured", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)

        End If

        Return result
    End Function

    Public Function EmailDocumentSilent() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "EmailDocumentSilent"
        Dim msgReply As DialogResult
        Dim sOptionValue As String = String.Empty
        Dim sMainEmailAddress As String = String.Empty
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetPolicyLevelEmailAddress(m_lInsuranceFileCnt, sMainEmailAddress)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "Failed to retreive Policy Level Email Address", gPMConstants.PMELogLevel.PMLogError)
            End If

            If String.IsNullOrEmpty(sMainEmailAddress) Then
                m_lReturn = m_oBusiness.GetPartyMainEmailAddress(v_lParty_cnt:=m_lPartyCnt, v_sEmailAddress:=sMainEmailAddress)
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    gPMFunctions.RaiseError(kMethodName, "Failed to retreive main Email Address", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If String.IsNullOrEmpty(sMainEmailAddress) Then
                m_lReturn = AddFailedEmailWorkManagerTask()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Could not create an Failed Email Work Manager Task", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                m_lReturn = m_oBusiness.SendEMail(v_sTo:=gPMFunctions.ToSafeString(sMainEmailAddress).Trim(), v_sSubject:=m_sDocumentDescription, v_sMessagePath:=m_sClientDocument, v_sAttachment:="")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SendEmail Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            m_lReturn = m_oBusiness.CreateEvent(r_lEventCnt:=EventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_vInsuranceFileCnt:=m_lInsuranceFileCnt, v_vClaimCnt:=m_lClaimCnt, v_vDocumentCnt:=m_lDocumentTemplateId, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=m_lDocumentTypeId, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventDocument, v_dtEventDate:=DateTime.Today, v_sDescription:="Document Emailed - " & m_sDocumentDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateEvent Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    Private Function CheckFileTypeIsXML() As Integer
        Dim result As Integer = 0
        Dim sFile As String = ""
        Dim lFileCount, lFileNum As Integer
        Dim sLine As String = ""


        result = gPMConstants.PMEReturnCode.PMFalse

        'DJM 02/09/2002 : Convert files in the zip directory not in the client directory
        'RAM20040205    : Use FSO rather than the Dir Commands
        '                 Use m_sClient instead of m_sZIP_DIRECTORY, since that is the working directory
        lFileCount = NoOfFilesInDirectory(v_sDirectoryName:=m_sClient, r_vFirstFileName:=sFile)

        Select Case lFileCount
            Case Is > 1
                ' Too many files
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Too Many Files in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsXML", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            Case 0
                ' No Files Found
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsXML", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            Case 1
                ' Only one file found, so check the type of the file

                lFileNum = FileSystem.FreeFile()
                FileSystem.FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)
                sLine = FileSystem.InputString(lFileNum, 5)
                FileSystem.FileClose(lFileNum)
                Select Case sLine.ToUpper()
                    Case "<?XML"
                        result = gPMConstants.PMEReturnCode.PMTrue
                End Select

            Case Else
                ' Some other no, so error

                ' No Files Found
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsXML", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
        End Select

        Return result
    End Function



    Private Function ConvertDocumentIntoHTML() As Long
        Dim sStr() As String
        Dim sDocument As String

        sDocument = m_sClient & "\Doc " & m_lDocumentTemplateId & "." & m_sDocFileExtension
        m_sClientDocument = sDocument
        sStr = sDocument.Split(".")
        m_sClientConvertedHTMDocument = sStr(0) & ".htm"
        ConvertDocumentUsingSiriusDocumentUtility(sDocument, m_sClientConvertedHTMDocument)
    End Function

    Private Function DeleteHTMDependecies() As Integer

        Dim sSTR() As String
        Dim sTmpFile As String
        Dim sFiles() As String
        Dim iCnt As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_sDocFileExtension.ToUpper() = "XML" Then
            If m_sClientConvertedHTMDocument <> "" Then
                sFiles = Directory.GetFiles(m_sClient)

                If IsArray(sFiles) Then
                    For iCnt = 0 To UBound(sFiles)
                        sTmpFile = sFiles(iCnt)
                        sSTR = sTmpFile.Split(".")
                        If sSTR(1).ToUpper <> "XML" Then
                            File.Delete(sTmpFile)
                        End If
                    Next
                End If
            End If
        End If

        Return result

    End Function

    Public Function Close() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    Public Function Handle() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    Public Function frmInterfaceLoad() As Integer

        m_oBusiness.IsNonBatchProcess = m_bIsNonBatchProcess

        m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to process the interface.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

            Exit Function
        End If


        m_lReturn = SetFormlessInterfaceDefaults()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Function
        End If

        'TN20010205 Start
        If m_lMode <> gSIRLibrary.ACSpoolReportMode Then
            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

        End If

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get the interface details.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Function
        End If

    End Function

    Private Function Initialise() As Integer

        Dim sMessage, sTitle As String



        ' Initialise the error number value.
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

        g_oZipper = New bPMZipper.Business()

        If g_oObjectManager Is Nothing Then
            g_oObjectManager = New bObjectManager.ObjectManager()
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
        End If

        ' Get an instance of the business object via
        ' the public object manager.            
        m_lReturn = g_oObjectManager.GetInstance(m_oBusiness, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Get description from the resource file.                
            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display message.
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return m_lErrorNumber
        End If

        ' Create an instance of the general interface object.
        m_oGeneral = New iPMBDocTemplate.General()

        ' Call the initialise method passing this interface
        ' and the business object as parameters.
        m_lReturn = m_oGeneral.InitialiseFormless(frmInterface:=Me, oBusiness:=m_oBusiness)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            Return m_lErrorNumber
        End If

        m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency
        m_bChaserLettersEnabled = m_oBusiness.ChaserLettersEnabled
        m_oBusiness.CallingAppName = ACApp

        m_bEditted = False

        bPMDocFunctions.Username = g_oObjectManager.UserName.Trim()
        Return m_lErrorNumber


    End Function

    Private Function SetFormlessInterfaceDefaults() As Integer

        Dim result As Integer
        Dim vTaskId As Object



        result = gPMConstants.PMEReturnCode.PMTrue
        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

            ' Get current task ID.                
            m_lReturn = m_oBusiness.GetTaskID(r_vTaskId:=vTaskId, m_lDocumentTemplateId:=m_lDocumentTemplateId)

            ' Lets store the old task so that if we hit cancel, we can revert the
            ' template to look at this old task.

            'RKS 240904 PN15082
            'corrected a Type Mismatch error - vTaskId Array.
            If Information.IsArray(vTaskId) Then

                If Strings.Len(CStr(vTaskId(0, 0))) Then

                    m_lOldWrkTaskCnt = CInt(vTaskId(0, 0))
                    'ED 31102002 - Store Current Task Id here as well

                    m_lWrkTaskCnt = CInt(vTaskId(0, 0))
                End If
            Else
                m_lOldWrkTaskCnt = 0
                m_lWrkTaskCnt = 0
            End If

        End If

        m_lReturn = GetServer()
        m_lReturn = GetClient()
        m_bFishSent = False
        Return result


    End Function

    Public Function BusinessToInterface() As Integer
        Dim result As Integer = 0
        Dim bEditVisible As Boolean 'DJM 02/04/2001

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            'DJM 02/04/2002 : Uncommented the following if statement to remove edit icon from
            'toolbar if the document has been merged and is not allowed to be edited.
            bEditVisible = True
            If m_lMode = gSIRLibrary.ACMergeMode Then
                If m_iIsEditableAfterMerging = 0 Then
                    bEditVisible = False
                End If
            End If



            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Dim lSource As Integer
        Dim vArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.
            m_bSetUp = True

            'cboType.Items.Clear()
            'm_lReturn = GetLookupDetails(sLookupTable:="document_type", ctlLookup:=cboType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'cboClient.Items.Clear()
            'cboPolicy.Items.Clear()
            'cboSourceId.Items.Clear()
            'cboClaim.Items.Clear()

            lSource = 0

            vArray = m_vClientArray(lSource)

            'If Information.IsArray(vArray) Then

            '    For iTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
            '        Dim cboClient_NewIndex As Integer = -1

            '        cboClient_NewIndex = cboClient.Items.Add(CStr(vArray(1, iTemp)))

            '        VB6.SetItemData(cboClient, cboClient_NewIndex, CInt(vArray(0, iTemp)))

            '        If Not (Convert.IsDBNull(m_vSlotNumber) Or IsNothing(m_vSlotNumber)) Then

            '            If m_vSlotNumber = CDbl(vArray(0, iTemp)) Then
            '                cboClient.SelectedIndex = cboClient_NewIndex
            '            End If
            '        End If
            '    Next iTemp
            'End If

            lSource = 0

            vArray = m_vPolicyArray(lSource)

            'If Information.IsArray(vArray) Then
            '    Dim cboPolicy_NewIndex As Integer = -1
            '    cboPolicy_NewIndex = cboPolicy.Items.Add("")

            '    For iTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

            '        cboPolicy_NewIndex = cboPolicy.Items.Add(CStr(vArray(1, iTemp)))

            '        VB6.SetItemData(cboPolicy, cboPolicy_NewIndex, CInt(vArray(0, iTemp)))

            '        If Not (Convert.IsDBNull(m_vSlotNumber) Or IsNothing(m_vSlotNumber)) Then

            '            If m_vSlotNumber = CDbl(vArray(0, iTemp)) Then
            '                cboPolicy.SelectedIndex = cboPolicy_NewIndex
            '            End If
            '        End If
            '    Next iTemp

            'End If

            If Information.IsArray(m_vSourceArray) Then
                'For iTemp As Integer = m_vSourceArray.GetLowerBound(1) To m_vSourceArray.GetUpperBound(1)
                '    Dim cboSourceId_NewIndex As Integer = -1
                '    cboSourceId_NewIndex = cboSourceId.Items.Add(CStr(m_vSourceArray(2, iTemp)))
                '    VB6.SetItemData(cboSourceId, cboSourceId_NewIndex, CInt(m_vSourceArray(0, iTemp)))
                '    If m_lSourceId = CDbl(m_vSourceArray(0, iTemp)) Then
                '        cboSourceId.SelectedIndex = cboSourceId_NewIndex
                '    End If
                'Next iTemp

                m_lSourceId = 0

            End If

            'MKW020503 PN3890 START Display claims text slots
            lSource = 0

            vArray = m_vClaimArray(lSource)
            If Information.IsArray(vArray) Then

                'For iTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                '    Dim cboClaim_NewIndex As Integer = -1

                '    cboClaim_NewIndex = cboClaim.Items.Add(CStr(vArray(1, iTemp)))

                '    VB6.SetItemData(cboClaim, cboClaim_NewIndex, CInt(vArray(0, iTemp)))

                '    If Not (Convert.IsDBNull(m_vSlotNumber) Or IsNothing(m_vSlotNumber)) Then

                '        If m_vSlotNumber = CDbl(vArray(0, iTemp)) Then
                '            cboClaim.SelectedIndex = cboClaim_NewIndex
                '        End If
                '    End If
                'Next iTemp
            End If
            'MKW020503 PN3890 END

            'sj 12/08/2002 - start
            'Do not allow "All branches" for text file templates
            'If VB6.GetItemData(cboType, cboType.SelectedIndex) = PMBConst.PMBClientTextFile Or VB6.GetItemData(cboType, cboType.SelectedIndex) = PMBConst.PMBPolicyTextFile Or VB6.GetItemData(cboType, cboType.SelectedIndex) = PMBConst.PMBClaimTextFile Then 'MKW020503 PN3890 Added claims text files
            '    If VB6.GetItemData(cboSourceId, 0) = 0 Then
            '        cboSourceId.Items.RemoveAt(0)
            '    End If
            'End If
            'sj 12/08/2002 - end

            'Select Case VB6.GetItemData(cboType, cboType.SelectedIndex)
            '    Case PMBConst.PMBClientTextFile
            '        lblSlot.Visible = True
            '        cboClient.Visible = True
            '        cboPolicy.Visible = False
            '        cboClaim.Visible = False 'MKW020503 PN3890
            '        'CT 21/12/00 if there are items in the listbox...
            '        If cboPolicy.Items.Count > 0 Then
            '            cboPolicy.SelectedIndex = 0
            '        End If
            '        'MKW020503 PN3890 START
            '        If cboClaim.Items.Count > 0 Then
            '            cboClaim.SelectedIndex = 0
            '        End If
            '        'MKW020503 PN3890 END
            '        lblRisk.Visible = False
            '        cboRisk.Visible = False
            '        cboRisk.SelectedIndex = -1
            '        lblGroup.Visible = False
            '        cboGroup.Visible = False
            '        cboGroup.SelectedIndex = 0
            '    Case PMBConst.PMBPolicyTextFile
            '        lblSlot.Visible = True
            '        cboClient.Visible = False
            '        cboClaim.Visible = False 'MKW020503 PN3890
            '        'CT 21/12/00 if there are items in the listbox...
            '        If cboClient.Items.Count > 0 Then
            '            cboClient.SelectedIndex = 0
            '        End If
            '        'MKW020503 PN3890 START
            '        If cboClaim.Items.Count > 0 Then
            '            cboClaim.SelectedIndex = 0
            '        End If
            '        'MKW020503 PN3890 END
            '        cboPolicy.Visible = True
            '        lblRisk.Visible = True
            '        cboRisk.Visible = True
            '        lblGroup.Visible = True
            '        cboGroup.Visible = True
            '        'MKW020503 PN3890 START
            '    Case PMBConst.PMBClaimTextFile
            '        lblSlot.Visible = True
            '        cboClient.Visible = False
            '        cboPolicy.Visible = False
            '        cboClaim.Visible = True
            '        If cboClient.Items.Count > 0 Then
            '            cboClient.SelectedIndex = 0
            '        End If
            '        If cboPolicy.SelectedIndex > 0 Then
            '            cboPolicy.SelectedIndex = 0
            '        End If
            '        lblRisk.Visible = False
            '        cboRisk.Visible = False
            '        cboRisk.SelectedIndex = -1
            '        lblGroup.Visible = False
            '        cboGroup.Visible = False
            '        cboGroup.SelectedIndex = 0
            '        'MKW020503 PN3890 END
            '    Case Else
            '        lblSlot.Visible = False
            '        cboClient.Visible = False
            '        cboClient.SelectedIndex = -1
            '        cboPolicy.Visible = False
            '        cboPolicy.SelectedIndex = -1
            '        lblRisk.Visible = False
            '        cboRisk.Visible = False
            '        cboRisk.SelectedIndex = 0
            '        lblGroup.Visible = False
            '        cboGroup.Visible = False
            '        cboGroup.SelectedIndex = 0
            '        cboClaim.Visible = False 'MKW020503 PN3890
            '        cboClaim.SelectedIndex = -1 'MKW020503 PN3890
            'End Select

            m_bSetUp = False

            'Filter textBox disabled for document type - clauses (7 is hardcoded value in u/w, broking use the code) PN22739
            'If VB6.GetItemData(cboType, cboType.SelectedIndex) = 7 Then
            '    txtDocument_Filter.Visible = True
            '    lblDocument_Filter.Visible = True
            'Else
            '    txtDocument_Filter.Visible = False
            '    lblDocument_Filter.Visible = False
            'End If

            'RWH(21/08/2000) - RSAIB Process 12
            'DN 30/01/02 - Only applicable for Underwriting
            If (m_lDocumentTypeId = lCLAUSE_TYPE_ID) And (m_sUnderwritingOrAgency <> "A") Then

                m_lReturn = m_oBusiness.GetRiskClauseLinks(m_lDocumentTemplateId, m_vRiskClauseLinkArray)

                ' {* USER DEFINED CODE (End) *}

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get RiskClauseLinks from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")

                    Return result
                End If
            End If

            'RWH(21/08/2000) - RSAIB Process 12.
            'DN 30/01/02 - Only applicable for Underwriting

            If (m_lDocumentTypeId = lCLAUSE_TYPE_ID) Then
                If Information.IsArray(m_vRiskClauseLinkArray) Then
                    'lvwRisks.Items.Clear()
                    'For iRiskCount As Integer = 0 To m_vRiskClauseLinkArray.GetUpperBound(1)
                    '    If CStr(m_vRiskClauseLinkArray(3, iRiskCount)) <> "" Then
                    '        oListItem = lvwRisks.Items.Add("Yes")
                    '        oListItem.Tag = CStr(1)
                    '    Else
                    '        oListItem = lvwRisks.Items.Add("No")
                    '        oListItem.Tag = CStr(0)
                    '    End If
                    '    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRiskClauseLinkArray(1, iRiskCount))
                    '    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vRiskClauseLinkArray(2, iRiskCount))

                    'Next iRiskCount
                End If
            End If

            Dim iPrinterListIndex As Integer

            iPrinterListIndex = 0
            'cboPrinter.Items.Clear()
            'cboPrinter.Items.Add("Default")
            'RWH(26/07/01) Printer stuff.
            If Information.IsArray(m_vPrinters) Then
                'For iPrinterCount As Integer = m_vPrinters.GetLowerBound(0) To m_vPrinters.GetUpperBound(0)
                '    cboPrinter.Items.Add(CStr(m_vPrinters(iPrinterCount)))
                '    If CStr(m_vPrinters(iPrinterCount)).Trim().ToUpper() = m_sSelectedPrinter.Trim().ToUpper() Then
                '        iPrinterListIndex = iPrinterCount + 1
                '    End If

                'Next iPrinterCount
            End If
            'cboPrinter.SelectedIndex = iPrinterListIndex


            'AK 090402 - add chaser list
            Dim iChaserListIndex As Integer

            iChaserListIndex = 0
            'cboChaser.Items.Clear()
            'cboChaser.Items.Add("None")
            'If Information.IsArray(m_vChasers) Then
            '    For iChaserCount As Integer = m_vChasers.GetLowerBound(1) To m_vChasers.GetUpperBound(1)
            '        cboChaser.Items.Add(CStr(m_vChasers(0, iChaserCount)))
            '        If CStr(m_vChasers(0, iChaserCount)).Trim().ToUpper() = m_sSelectedChaser.Trim().ToUpper() Then
            '            iChaserListIndex = iChaserCount + 1
            '        End If

            '    Next iChaserCount
            'End If
            'cboChaser.SelectedIndex = iChaserListIndex
            ''Plico21
            'If CStr(m_vLookupDetails(ACDetailCode, cboType.SelectedIndex)).Trim().ToUpper() = ACDocTypeClauses Or CStr(m_vLookupDetails(ACDetailCode, cboType.SelectedIndex)).Trim().ToUpper() = ACDocTypeStandardLetter Or CStr(m_vLookupDetails(ACDetailCode, cboType.SelectedIndex)).Trim().ToUpper() = ACDocTypeDebitNote Then
            '    lblEffectiveDate.Visible = True
            '    cboEffectiveDate.Visible = True

            '    cboEffectiveDate.Value = IIf(m_dtDocEffectiveDate = DateTime.MinValue, DateTime.Today, m_dtDocEffectiveDate)
            'Else
            '    lblEffectiveDate.Visible = False
            '    cboEffectiveDate.Visible = False
            'End If            

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function SetTypeEditable() As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'lType = VB6.GetItemData(cboType, cboType.SelectedIndex)

            ''RWH(18/09/2000) Check array is not empty first.
            'If Information.IsArray(m_vDocumentTypeArray) Then
            '    For lTemp As Integer = m_vDocumentTypeArray.GetLowerBound(1) To m_vDocumentTypeArray.GetUpperBound(1)

            '        If CDbl(m_vDocumentTypeArray(0, lTemp)) = lType Then
            '            If CDbl(m_vDocumentTypeArray(1, lTemp)) = 1 Then
            '                chkIsTypeEditable.CheckState = CheckState.Checked
            '            Else
            '                chkIsTypeEditable.CheckState = CheckState.Unchecked
            '            End If

            '            Exit For
            '        End If

            '    Next lTemp
            'End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetTypeEditable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetTypeEditable", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Public Function SetBrowser() As Integer

        Dim result As Integer = 0
        Dim sClient As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If WebBrowser1 IsNot Nothing Then
                WebBrowser1.Visible = True

                m_lReturn = ConvertDocumentIntoHTML()

                WebBrowser1.Navigate(New Uri(m_sClientConvertedHTMDocument))

                Do While WebBrowser1.ReadyState <> WebBrowserReadyState.Complete
                    Sleep(500)
                    Application.DoEvents()
                Loop
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the Browser control", vApp:=ACApp, vClass:=ACClass, vMethod:="SetBrowser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

End Class
