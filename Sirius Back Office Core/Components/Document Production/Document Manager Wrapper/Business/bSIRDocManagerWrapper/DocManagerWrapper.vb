Option Strict Off
Option Explicit On
Imports System.IO
'developer guide no. 129 (guide)
Imports SSP.Shared
Imports Word = Microsoft.Office.Interop.Word
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: DocManagerWrapper
    '
    ' Date: 11/10/2002
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    '
    '   PSL 27/11/2002 Added Archiving when printing
    '
    '   TJB 10/12/2002 Changes for use with AOL
    '
    '   PW140105 - PN18078 - in the SpoolDocument function, pass in the Document
    '              Template ID to the spooler function. This now gets added to the
    '              document_spooler record.
    ' ***************************************************************** '

    Private Const ACClass As String = "Interface"


    ' Private m_oObjectManager As bObjectManager.ObjectManager

    ' ************************************************
    ' Added to replace global variables 09/01/2004
    Private m_sUsername As String = ""
    Private m_Background_Job_Id As Integer
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' ************************************************
    Private m_lReturn As Integer
    Private m_lDocumentTemplateId As Integer
    Private m_sDocumentTemplateCode As String = ""
    Private m_lDocumentTypeId As Integer
    Private m_lPartyCnt As Integer
    Private m_lPartyEmailCnt As Integer ''PN62462
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lClaimCnt As Integer
    Private m_sDocumentRef As String = ""
    Private m_lMode As Integer
    Private m_sServer As String = ""
    Private m_sClient As String = ""
    Private m_sClientDocument As String = ""
    Private m_sClientDocList As List(Of String)
    Private m_oSplitDocMergedCodes As List(Of String)
    Private m_sDocumentTemplateDescription As String = ""
    Private m_lSpoolNumber As Integer
    Private m_lWordHwnd As Integer
    Private m_bSpoolAsHTML As Boolean
    Private m_bSpoolAsTXT As Boolean
    Private m_sDocumentTextContents As String
    Private m_sSpoolDesc As String = ""
    Private m_sDocName As String = ""
    Private m_sWordVersion As String = ""
    Private m_lProcessTypesDocsId As Integer
    Private m_bArchiveDoc As Boolean
    Private m_sPartyName As String = ""
    Private m_sInsuranceFileRef As String = ""
    Private m_oBusiness As bSIRDocTemplate.Business
    Private m_oDocManager As bPMBDocManager.Interface_Renamed
    Private m_oDocSpooler As bSIRDocSpooler.Business
    Private m_oDocTemplate As bSIRDocTemplate.Business
    Private m_oTaskInstance As bPMWrkTaskInstanceTemp.FormClass
    Private m_oTaskInstancetaskControl As bPMWrkTaskInstance.TaskControl
    Private m_oFindDocTemplate As bSIRFindDocTemplate.Form
    Private m_oLookup As BPMLOOKUP.Business
    Private m_oSIRDOCAPI As bSIRDOCAPI.Form
    Private m_sClaimRef As String = ""
    Private m_oWord As Object
    Private m_oDocument As Word.Document
    Private m_oZipper As bPMZipper.Business
    Private m_bSpoolAsPDF As Boolean
    Private m_bInternalOnly As Boolean
    Private m_oSharePoint As bSIRSharepoint.Business
    Private m_iDocumentTemplateGroupID As Integer
    Private m_iDocumentTemplateSubGroupID As Integer
    Private m_sDestinationFilename As String

    '*************************************
    ' ME : 28-11-2002 : 202
    ' Holds return value from Documaster _
    '' indicating a document that has been archived
    Private m_lDocumasterId As Integer
    '*************************************
    ' ME : 29-11-2002 : 202
    ' Holds an array: stored procs name, value, datatype
    ' This array passes additional parameters to
    ' the stored procedures referenced by fields
    ' in wp_fields
    Private m_vFieldParams As Object
    '*************************************

    'TJB : 10-12-2002
    Private m_sMergedFilePath As String = ""
    Private m_sSpooledFilePath As String = ""
    Private m_sTransactionType As String = ""

    Private m_bCalledFromSwift As Boolean ' RAM20050201 - Added for Swift Support
    Private m_sParameterXML As String = ""
    Private m_bCalledFromSAM As Boolean

    Private m_oEvent As bSIREvent.Business ' PN28156
    Private m_iIsEditableAfterMerging As Integer
    Private m_dtEffectiveDate As Date

    'Renewal Printing
    Private m_iIsClient As Integer
    Private m_iIsAgent As Integer
    Private m_iIsOffice As Integer
    Private m_iProductionOrder As Integer

    Private m_bArchiveWithNoPrint As Boolean
    Private m_bEmailAsBody As Boolean
    Private m_bSpoolDocument As Boolean
    Private m_bArchiveAsText As Boolean
    Private m_bArchiveAsXML As Boolean
    Private m_bArchiveWithNoMerge As Boolean
    Private m_sPartyCode As String = ""
    Private m_bRetainTempFiles As Boolean = False
    Private m_bIsGeneratedMail As Boolean = False
    Private IsSpooled As Boolean = False
    Private m_sArchiveDocFileName As String = ""
    Private m_bIsSuppressArchive As Boolean = False
    Dim sClient As String
    Private m_IsCalledFromBatchProcessing As Boolean

    ''' <summary>
    ''' it is used to take the decision wheather it is being called from the bussiness.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsCalledFromBatchProcessing() As Boolean
        Get
            Return m_IsCalledFromBatchProcessing
        End Get
        Set(ByVal Value As Boolean)
            m_IsCalledFromBatchProcessing = Value
        End Set
    End Property
    Private m_sCCMDocumentName As String = ""
    Private m_bIsCalledFromBackGroundJob As Boolean = False
    Private m_bIsDMEMigration As Boolean = False
    Private m_sCreatedDate As String = False
    Private m_bIsNonBatchProcess As Boolean

    Public ReadOnly Property DocumasterID() As Integer
        Get
            Return m_lDocumasterId
        End Get
    End Property

    Public WriteOnly Property FieldParameters() As Object
        Set(ByVal Value As Object)
            m_vFieldParams = Value
        End Set
    End Property

    Public WriteOnly Property SpoolDesc() As String
        Set(ByVal Value As String)
            m_sSpoolDesc = Value
        End Set
    End Property
    Public WriteOnly Property DocName() As String
        Set(ByVal Value As String)
            m_sDocName = Value
        End Set
    End Property
    Public WriteOnly Property ProcessTypesDocsId() As Integer
        Set(ByVal Value As Integer)
            m_lProcessTypesDocsId = Value
        End Set
    End Property

    Public WriteOnly Property DestinationFilename() As String
        Set(ByVal Value As String)
            m_sDestinationFilename = Value
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

    Public WriteOnly Property DocumentRef() As String
        Set(ByVal Value As String)
            m_sDocumentRef = Value
        End Set
    End Property
    Public WriteOnly Property DocumentTemplateId() As Integer
        Set(ByVal Value As Integer)
            m_lDocumentTemplateId = Value
        End Set
    End Property
    Public WriteOnly Property DocumentTypeId() As Integer
        Set(ByVal Value As Integer)
            m_lDocumentTypeId = Value
        End Set
    End Property


    Public Property ArchiveDoc() As Boolean
        Get
            Return m_bArchiveDoc
        End Get
        Set(ByVal Value As Boolean)
            m_bArchiveDoc = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFileRef() As String
        Set(ByVal Value As String)
            m_sInsuranceFileRef = Value
        End Set
    End Property

    Public WriteOnly Property PartyCode() As String
        Set(ByVal value As String)
            m_sPartyCode = value
        End Set
    End Property

    Public Property ClaimRef() As String
        Get
            Return m_sClaimRef
        End Get
        Set(ByVal Value As String)
            m_sClaimRef = Value
        End Set
    End Property

    Public WriteOnly Property PartyName() As String
        Set(ByVal Value As String)
            m_sPartyName = Value
        End Set
    End Property

    Public WriteOnly Property OutputAsHTML() As Boolean
        Set(ByVal Value As Boolean)
            m_bSpoolAsHTML = Value
        End Set
    End Property

    Public WriteOnly Property OutputAsTXT() As Boolean
        Set(ByVal Value As Boolean)
            m_bSpoolAsTXT = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public Property IsGeneratedMail() As Boolean
        Get
            Return m_bIsGeneratedMail
        End Get
        Set(ByVal Value As Boolean)
            m_bIsGeneratedMail = Value
        End Set
    End Property

    Public Property MergedFilePath() As String
        Get
            Return m_sMergedFilePath
        End Get

        Set(ByVal Value As String)
            m_sMergedFilePath = Value
        End Set
    End Property
    'TJB : 10-12-2002
    Public ReadOnly Property SpooledFilePath() As String
        Get
            Return m_sSpooledFilePath
        End Get
    End Property

    Public ReadOnly Property ResolvedDocumentList() As List(Of String)
        Get
            Return m_sClientDocList
        End Get
    End Property

    Public ReadOnly Property SplitDocMergedCodes() As List(Of String)
        Get
            Return m_oSplitDocMergedCodes
        End Get
    End Property

    'RDT : 30-03-2006
    Public WriteOnly Property OutputAsPDF() As Boolean
        Set(ByVal Value As Boolean)
            m_bSpoolAsPDF = Value
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

    Public Property Background_Job_Id() As Integer
        Get
            Return m_Background_Job_Id
        End Get
        Set(ByVal Value As Integer)
            m_Background_Job_Id = Value
        End Set
    End Property
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    ''PN 62462
    Public WriteOnly Property PartyEmailCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyEmailCnt = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public WriteOnly Property ClaimCnt() As Integer
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property
    Public WriteOnly Property Mode() As Integer
        Set(ByVal Value As Integer)
            m_lMode = Value
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

    Public Property InternalOnly() As Boolean
        Get
            Return m_bInternalOnly
        End Get
        Set(ByVal Value As Boolean)
            m_bInternalOnly = Value
        End Set
    End Property

    Public Property ArchiveWithNoMerge() As Boolean
        Get
            Return m_bArchiveWithNoMerge
        End Get
        Set(ByVal Value As Boolean)
            m_bArchiveWithNoMerge = Value
        End Set
    End Property

    Public Property DocumentTemplateGroupID() As Integer
        Get
            Return m_iDocumentTemplateGroupID
        End Get
        Set(ByVal Value As Integer)
            m_iDocumentTemplateGroupID = Value
        End Set
    End Property

    Public Property DocumentTemplateSubGroupID() As Integer
        Get
            Return m_iDocumentTemplateSubGroupID
        End Get
        Set(ByVal Value As Integer)
            m_iDocumentTemplateSubGroupID = Value
        End Set
    End Property

    Public WriteOnly Property DocumentTemplateCode() As String
        Set(ByVal Value As String)
            m_sDocumentTemplateCode = Value
        End Set
    End Property

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

    Public WriteOnly Property CalledFromSAM() As Boolean
        Set(ByVal Value As Boolean)
            m_bCalledFromSAM = Value
        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public WriteOnly Property RetainTempFiles() As Boolean
        Set(ByVal value As Boolean)
            m_bRetainTempFiles = value
        End Set
    End Property
    Public Property IsSuppressArchive() As Boolean
        Get
            Return m_bIsSuppressArchive
        End Get
        Set(ByVal value As Boolean)
            m_bIsSuppressArchive = value
        End Set
    End Property

    Public WriteOnly Property ArchieveDocFileName() As String
        Set(ByVal value As String)
            m_sArchiveDocFileName = value
        End Set
    End Property

    Public Property CCMDocumentName() As String
        Get
            Return m_sCCMDocumentName
        End Get
        Set(ByVal value As String)
            m_sCCMDocumentName = value
        End Set
    End Property
    Public Property IsCalledFromBackGroundJob() As Boolean
        Get
            Return m_bIsCalledFromBackGroundJob
        End Get
        Set(ByVal value As Boolean)
            m_bIsCalledFromBackGroundJob = value
        End Set
    End Property
    Public Property SkipArchiveOnEdit() As Boolean = False

    Public Property IsDMEMigration() As Boolean
        Get
            Return m_bIsDMEMigration
        End Get
        Set(ByVal value As Boolean)
            m_bIsDMEMigration = value
        End Set
    End Property

    Public WriteOnly Property CreatedDate() As String
        Set(ByVal value As String)
            m_sCreatedDate = value
        End Set
    End Property

    Public WriteOnly Property IsNonBatchProcess() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsNonBatchProcess = Value
        End Set
    End Property

    Public Function DocumentTypeCode(ByRef lDocumentTypeCode As String) As Integer
        Dim result As Integer = 0
        Dim oLookup As BPMLOOKUP.Business = Nothing
        Dim lID As Integer

        result = GetLookup(oLookup)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DocumentTypeCode")
            Return result
        End If

        result = oLookup.GetEffectiveIDFromCode("document_type", lDocumentTypeCode, DateTime.Now, lID)
        DocumentTypeId = lID

        If lID < 1 Or result <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot find Document type ID for code: " & lDocumentTypeCode, vApp:=ACApp, vClass:=ACClass, vMethod:="DocumentTypeCode")
            Return result
        End If

        Return result
    End Function
    Private Function GetLookup(ByRef r_oLookup As Object) As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        If m_oLookup Is Nothing Then
            m_oLookup = New BPMLOOKUP.Business()


            m_lReturn = m_oLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oLookup = Nothing
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oLookup.Initialise failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

        End If
        r_oLookup = m_oLookup

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get an instance of the document template object

            m_oBusiness = New bSIRDocTemplate.Business
            m_lReturn = m_oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                               iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocTemplate.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If
            'Get an instance of the document spooler object
            m_oDocSpooler = New bSIRDocSpooler.Business

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocSpooler.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            m_lReturn = m_oDocSpooler.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                               iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocSpooler.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If
            'Are we going to spool in HTML format
            m_bSpoolAsHTML = SpoolAsHtml()

            'RDT - Default to false
            m_bSpoolAsPDF = False

            'Get instance of document manager object
            m_oDocManager = New bPMBDocManager.Interface_Renamed()

            m_lReturn = m_oDocManager.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed initialise iPMBDocManager.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            m_oDocManager.SpoolAsHTML = m_bSpoolAsHTML

            m_oDocManager.Mode = m_lMode

            m_oTaskInstance = New bPMWrkTaskInstanceTemp.FormClass

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMWrkTaskInstanceTemp.FormClass", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            m_lReturn = m_oTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                               iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMWrkTaskInstanceTemp.FormClass", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If
            'Get instance of find document template object

            m_oFindDocTemplate = New bSIRFindDocTemplate.Form

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRFindDocTemplate.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            m_lReturn = m_oFindDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                               iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRFindDocTemplate.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If

            m_oZipper = New bPMZipper.Business()
            m_dtEffectiveDate = DateTime.Now

            m_bCalledFromSAM = False

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: InitialiseBusiness (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function InitialiseBusiness(ByRef sUsername As String, ByRef sPassword As String,
                                       ByRef iUserID As Integer, ByRef iSourceID As Integer,
                                       ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer,
                                       ByRef iLogLevel As Integer, ByRef sCallingAppName As String,
                                       Optional ByRef vDatabase As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            'Get an instance of the document template object
            m_oBusiness = New bSIRDocTemplate.Business()

            m_lReturn = m_oBusiness.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocTemplate.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If
            m_oTaskInstancetaskControl = New bPMWrkTaskInstance.TaskControl
            m_lReturn = m_oTaskInstancetaskControl.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMWrkTaskInstance.TaskControl", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If
            'Get an instance of the document spooler object
            m_oDocSpooler = New bSIRDocSpooler.Business()


            m_lReturn = m_oDocSpooler.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocSpooler.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If

            'Are we going to spool in HTML format
            m_bSpoolAsHTML = SpoolAsHtml()
            m_bSpoolAsPDF = False
            'Get instance of document manager object
            m_oDocManager = New bPMBDocManager.Interface_Renamed()

            m_lReturn = m_oDocManager.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMBDocManager.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If

            m_oDocManager.SpoolAsHTML = m_bSpoolAsHTML
            m_oDocManager.Mode = m_lMode
            m_oDocManager.IsCalledFromBatchProcessing = IsCalledFromBatchProcessing
            'Get an instance of the document template object
            m_oDocTemplate = New bSIRDocTemplate.Business()

            m_lReturn = m_oDocTemplate.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocTemplate.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If

            'Get an instance of the task instance object
            m_oTaskInstance = New bPMWrkTaskInstanceTemp.FormClass()

            m_lReturn = m_oTaskInstance.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMWrkTaskInstanceTemp.FormClass", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If

            'Get an instance of the find doc template object
            m_oFindDocTemplate = New bSIRFindDocTemplate.Form()


            m_lReturn = m_oFindDocTemplate.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRFindDocTemplate.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If

            m_oEvent = New bSIREvent.Business

            m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername,
                                            sPassword:=m_sPassword,
                                            iUserID:=m_iUserID,
                                            iSourceID:=m_iSourceID,
                                            iLanguageID:=m_iLanguageID,
                                            iCurrencyID:=m_iCurrencyID,
                                            iLogLevel:=m_iLogLevel,
                                            sCallingAppName:=ACApp,
                                            vDatabase:=vDatabase
                                            )
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername,
                    iType:=gPMConstants.PMELogLevel.PMLogOnError,
                    sMsg:="Failed to get instance of bSIREvent.Business",
                    vApp:=ACApp,
                    vClass:=ACClass,
                    vMethod:="InitialiseBusiness")
                Return result
            End If

            m_oZipper = New bPMZipper.Business()

            m_oSIRDOCAPI = New bSIRDOCAPI.Form()
            m_lReturn = m_oSIRDOCAPI.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SpoolAsHtml
    '
    ' Description:
    '
    ' History: 14/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function SpoolAsHtml() As Boolean
        Dim result As Boolean = False

        Dim vValue As Object = Nothing

        m_lReturn = bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSpoolAsHTML, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTSpoolAsHTML, vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolAsHtml")
            Return result
        End If

        Return gPMFunctions.ToSafeInteger(vValue) = 1
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                m_oZipper = Nothing
                If m_oDocument IsNot Nothing Then
                    m_oDocument.Dispose()
                    m_oDocument = Nothing
                End If
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_oDocManager IsNot Nothing Then
                    m_oDocManager.Dispose()
                    m_oDocManager = Nothing
                End If
                If m_oDocSpooler IsNot Nothing Then
                    m_oDocSpooler.Dispose()
                    m_oDocSpooler = Nothing
                End If
                If m_oDocTemplate IsNot Nothing Then
                    m_oDocTemplate.Dispose()
                    m_oDocTemplate = Nothing
                End If
                If m_oTaskInstance IsNot Nothing Then
                    m_oTaskInstance.Dispose()
                    m_oTaskInstance = Nothing
                End If

                If Not (m_oWord Is Nothing) Then
                    m_oWord.Application.Quit()
                    m_oWord = Nothing
                End If
                If m_oFindDocTemplate IsNot Nothing Then
                    m_oFindDocTemplate.Dispose()
                    m_oFindDocTemplate = Nothing
                End If
                'If m_oObjectManager IsNot Nothing Then
                '    m_oObjectManager.Dispose()
                '    m_oObjectManager = Nothing
                'End If
                If m_oSIRDOCAPI IsNot Nothing Then
                    m_oSIRDOCAPI.Dispose()
                    m_oSIRDOCAPI = Nothing
                End If

                ' Clear the temp directory if they exists
                If m_lPartyEmailCnt = 0 And m_lDocumentTypeId <> 8 And Not m_bRetainTempFiles And Not m_bCalledFromSAM Then ''PN62462
                    If gPMFunctions.FolderExists(m_sClient) Then
                        Directory.Delete(m_sClient, True)
                    End If
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 11/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim oLookup As BPMLOOKUP.Business = Nothing
        Dim lID, lEventTypeCode As Integer
        Dim sAutoArchive As String = ""
        Dim vInsuranceFolderCnt As Object = Nothing
        Const AUTO_ARCHIVE_ENABLED As Integer = 5008
        Const kMethodName As String = "Start"

        Try
            IsSpooled = False

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oBusiness.IsNonBatchProcess = m_bIsNonBatchProcess

            ' RAM20050124 : Check if the code is supplied, if supplied and if m_lDocumentTemplateId is 0 then
            '               Get the Template ID. We want to create document based on code
            '               This will be used by bGIS
            m_sDocumentTemplateCode = m_sDocumentTemplateCode.Trim()

            If m_sDocumentTemplateCode.Length > 0 And (m_lDocumentTemplateId = 0 Or m_lDocumentTypeId = 0) Then
                ' Get the document details

                m_lReturn = m_oBusiness.GetTemplateFromCode(sCode:=m_sDocumentTemplateCode, lDocId:=m_lDocumentTemplateId, lDocType:=m_lDocumentTypeId, sDocDesc:=m_sDocumentTemplateDescription, v_dtEffectiveDate:=m_dtEffectiveDate, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimCnt:=m_lClaimCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details of the document template : " & m_sDocumentTemplateCode, vApp:=ACApp, vClass:=ACClass, vMethod:="EstablishRequiredTemplate")
                    Return result
                End If
                m_sDocumentTemplateDescription = m_sDocumentTemplateDescription.Trim()
            End If

            'Only call for SAM for Back office or call from other roadmap see statem end of the condition
            If (m_bCalledFromSAM) Then
                If m_lDocumentTemplateId = 0 Then
                    RaiseError("DocManagerWrapper_Start", "DocumentTemplateCode  - " & m_sDocumentTemplateCode & " Not Found. ")
                ElseIf m_lDocumentTemplateId <> 0 Then
                    m_lReturn = m_oBusiness.GetDetails(vDocumentTemplateId:=m_lDocumentTemplateId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetDetails failed for Template ID : " & m_lDocumentTemplateId, vApp:=ACApp, vClass:=ACClass, vMethod:="m_oBusiness.GetIsEditable")
                        Return result
                    End If
                End If

                Dim iTemplateGroupID As Integer
                Dim iTemplateSubGroupID As Integer
                Dim bInternalOnly As Boolean
                bInternalOnly = m_bInternalOnly

                m_lReturn = m_oBusiness.GetNext(vDocumentTemplateId:=m_lDocumentTemplateId,
                                                      vTemplateGroupID:=iTemplateGroupID,
                                                      vTemplateSubGroupID:=iTemplateSubGroupID,
                                                      vArchiveWithNoPrint:=m_bArchiveWithNoPrint,
                                                      vEmailAsBody:=m_bEmailAsBody,
                                                      vSpoolDocument:=m_bSpoolDocument,
                                                      vArchiveAsText:=m_bArchiveAsText,
                                                      vIsInternalOnly:=m_bInternalOnly,
                                                      vIsEditableAfterMerging:=m_iIsEditableAfterMerging,
                                                      vDescription:=m_sDocumentTemplateDescription, vArchiveAsXML:=m_bArchiveAsXML,
                                                      r_sCCMDocumentName:=m_sCCMDocumentName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetNext failed for Template ID : " & m_lDocumentTemplateId, gPMConstants.PMELogLevel.PMLogError)
                End If

                If Background_Job_Id > 0 Then
                    m_bInternalOnly = bInternalOnly
                End If

                If iTemplateGroupID <> 0 And m_iDocumentTemplateGroupID = 0 Then
                    m_iDocumentTemplateGroupID = iTemplateGroupID
                End If

                If iTemplateSubGroupID <> 0 And m_iDocumentTemplateSubGroupID = 0 Then
                    m_iDocumentTemplateSubGroupID = iTemplateSubGroupID
                End If

                m_lReturn = MergeDocument()

                m_lReturn = PrintOrSpoolDocument(IsSpooled)

                If m_bEmailAsBody AndAlso Not m_lMode = gSIRLibrary.ACEmailMode Then
                    EmailDocument()
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PrintOrSpoolDocument Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'PM042004
            'If Not m_bCalledFromSAM Then
            If m_bCalledFromSAM OrElse Not m_bArchiveWithNoMerge Then
                If m_lDocumentTemplateId = 0 Or m_lDocumentTypeId = 0 Then
                    If m_lProcessTypesDocsId <> 0 Then
                        If EstablishRequiredTemplate() <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get available template", vApp:=ACApp, vClass:=ACClass, vMethod:="EstablishRequiredTemplate")

                            Return result
                        End If
                    End If
                End If

                If m_lDocumentTemplateId <> 0 Then
                    m_lReturn = m_oBusiness.GetDetails(vDocumentTemplateId:=m_lDocumentTemplateId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetDetails failed for Template ID : " & m_lDocumentTemplateId, vApp:=ACApp, vClass:=ACClass, vMethod:="m_oBusiness.GetIsEditable")
                        Return result
                    End If

                    Dim iTemplateGroupID As Integer
                    Dim iTemplateSubGroupID As Integer
                    Dim bInternalOnly As Boolean
                    bInternalOnly = m_bInternalOnly

                    m_lReturn = m_oBusiness.GetNext(vDocumentTemplateId:=m_lDocumentTemplateId,
                                                  vTemplateGroupID:=iTemplateGroupID,
                                                  vTemplateSubGroupID:=iTemplateSubGroupID,
                                                  vArchiveWithNoPrint:=m_bArchiveWithNoPrint,
                                                  vEmailAsBody:=m_bEmailAsBody,
                                                  vSpoolDocument:=m_bSpoolDocument,
                                                  vArchiveAsText:=m_bArchiveAsText,
                                                  vIsInternalOnly:=m_bInternalOnly,
                                                  vIsEditableAfterMerging:=m_iIsEditableAfterMerging,
                                                  vDescription:=m_sDocumentTemplateDescription, vArchiveAsXML:=m_bArchiveAsXML,
                                                  r_sCCMDocumentName:=m_sCCMDocumentName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetNext failed for Template ID : " & m_lDocumentTemplateId, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If iTemplateGroupID <> 0 And m_iDocumentTemplateGroupID = 0 Then
                        m_iDocumentTemplateGroupID = iTemplateGroupID
                    End If

                    If Background_Job_Id > 0 Then
                        m_bInternalOnly = bInternalOnly
                    End If
                    If iTemplateSubGroupID <> 0 And m_iDocumentTemplateSubGroupID = 0 Then
                        m_iDocumentTemplateSubGroupID = iTemplateSubGroupID
                    End If

                    If m_bEmailAsBody Or m_bArchiveWithNoPrint Then
                        m_lMode = gSIRLibrary.ACEmailMode
                    End If

                    If m_bSpoolDocument Then
                        m_lMode = gSIRLibrary.ACSpoolDocMode
                    End If
                End If

                If m_lMode <> gSIRLibrary.ACSpoolReportMode And Not m_bCalledFromSAM Then
                    m_lReturn = MergeDocument()
                End If

                Select Case m_lMode
                    Case gSIRLibrary.ACPrintMode, gSIRLibrary.ACPrintSilentMode, gSIRLibrary.ACSpoolDocMode, gSIRLibrary.ACSpoolReportMode
                        If m_lDocumentTypeId <> 0 Then
                            ' Print it
                            m_lReturn = PrintOrSpoolDocument(IsSpooled)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PrintOrSpoolDocument Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    Case gSIRLibrary.ACEmailMode
                        If m_sClientDocument <> "" Then
                            m_sMergedFilePath = m_sClientDocument
                        Else
                            m_sClientDocument = m_sMergedFilePath
                        End If
                        If m_sClientDocList Is Nothing OrElse m_sClientDocList.Count = 0 Then
                            If Not String.IsNullOrEmpty(m_sMergedFilePath) Then
                                m_sClientDocList = New List(Of String)({m_sMergedFilePath})
                            End If
                        End If
                End Select

                If m_bEmailAsBody AndAlso Not m_lMode = gSIRLibrary.ACEmailMode Then
                    EmailDocument()
                End If
            Else
                'We're archiving without merge, so the complete file already exists
                m_sClientDocument = m_sMergedFilePath
                If m_sClientDocList Is Nothing OrElse m_sClientDocList.Count = 0 Then
                    If Not String.IsNullOrEmpty(m_sMergedFilePath) Then
                        m_sClientDocList = New List(Of String)({m_sMergedFilePath})
                    End If
                End If
            End If

            ' RDC 29/09/2005
            ' Get system option auto-archive
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=AUTO_ARCHIVE_ENABLED, r_sOptionValue:=sAutoArchive)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End If
            '*************************************************************
            ' ME : 28-11-2002 : 202
            ' Need to be able to call archive without printing the document
            ' so call to archive moved to start routine and now implemented off
            ' archive doc indicator

            ''Saurabh Agrawal - Added extra Condition to the If Statement (5.5.3.4)
            'PN:75574


            If (m_bArchiveWithNoPrint = True _
                Or m_bArchiveDoc = True _
                Or m_bArchiveWithNoMerge Or m_bCalledFromSAM) Then
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                If m_sClientDocList IsNot Nothing Then
                    For Each doc In m_sClientDocList
                        If Not SkipArchiveOnEdit AndAlso Not IsSuppressArchive Then
                            m_lReturn = ArchiveDocument(sArchiveDocFileName:=doc)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Archive of Split Document Failed: " & doc, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        Else
                            If ToSafeString(m_sMergedFilePath) = "" AndAlso ToSafeString(m_sClientDocument) <> "" Then
                                m_sMergedFilePath = doc
                            End If
                        End If
                    Next
                End If

                If Not String.IsNullOrEmpty(m_sClientDocument) AndAlso (m_sClientDocList Is Nothing OrElse Not m_sClientDocList.Contains(m_sClientDocument)) Then
                    If Not SkipArchiveOnEdit AndAlso Not IsSuppressArchive Then
                        If (Not String.IsNullOrEmpty(m_sArchiveDocFileName) AndAlso m_sArchiveDocFileName.Length > 0) Then
                            m_lReturn = ArchiveDocument(sArchiveDocFileName:=m_sArchiveDocFileName)
                        Else
                            m_lReturn = ArchiveDocument()
                        End If
                    Else
                        If ToSafeString(m_sMergedFilePath) = "" AndAlso ToSafeString(m_sClientDocument) <> "" Then
                            m_sMergedFilePath = m_sClientDocument
                        End If
                    End If
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Archive of Document Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' ***************************************************************** '
                ' Get the event_type table id
                ' ***************************************************************** '
                lEventTypeCode = GetLookup(oLookup)

                If lEventTypeCode <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the GetLookup object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent")
                    Return result
                End If


                lEventTypeCode = oLookup.GetEffectiveIDFromCode("event_type", gPMConstants.PMDocumentEventType, DateTime.Now.AddDays(1), lID)

                If m_lInsuranceFolderCnt = 0 Then
                    vInsuranceFolderCnt = DBNull.Value
                Else
                    vInsuranceFolderCnt = m_lInsuranceFolderCnt
                End If

                m_lReturn = CreateEvent(v_lPartyCnt:=m_lPartyCnt, v_vInsuranceFolderCnt:=vInsuranceFolderCnt, v_vInsuranceFileCnt:=m_lInsuranceFileCnt, v_vClaimCnt:=m_lClaimCnt, v_vDocumentCnt:=m_lDocumasterId, v_lEventTypeId:=lID, v_vDocumentTypeId:=m_lDocumentTypeId, v_vDescription:=m_sDocumentTemplateDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Event For Archived Document Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' clear down documaster id as no archive has taken place
                ' to protect against incorrect ids being returned
                m_lDocumasterId = 0
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=excep)
            Throw
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
        Dim sOptionValue As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDocManager.PartyCnt = m_lPartyCnt
            m_oDocManager.InsuranceFileCnt = m_lInsuranceFileCnt
            m_oDocManager.ClaimCnt = m_lClaimCnt
            m_oDocManager.DocumentRef = m_sDocumentRef
            m_oDocManager.DocumentTemplateId = m_lDocumentTemplateId
            m_oDocManager.DocumentTypeId = m_lDocumentTypeId
            m_oDocManager.WordVersion = m_sWordVersion
            m_oDocManager.FieldParameters = m_vFieldParams
            m_oDocManager.ParameterXML = m_sParameterXML
            m_oDocManager.CalledFromSwift = m_bCalledFromSwift
            m_oDocManager.SpoolAsHTML = m_bSpoolAsHTML
            m_oDocManager.SpoolAsTXT = m_bSpoolAsTXT
            m_oDocManager.Mode = m_lMode
            m_oDocManager.CCMDocumentName = m_sCCMDocumentName
            m_oDocManager.SpoolAsPDF = m_bSpoolAsPDF

            m_lReturn = m_oDocManager.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMBDocManager.Interface.Start Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeDocument")
                m_oDocManager = Nothing
                Return result
            End If

            m_sClient = m_oDocManager.Client
            m_sServer = m_oDocManager.Server
            m_sClientDocument = m_oDocManager.ResolvedDocumentName
            m_sClientDocList = m_oDocManager.ResolvedDocumentList
            ' ResolvedDocumentList contains only split doc paths; main doc is in ResolvedDocumentName
            m_oSplitDocMergedCodes = Nothing
            If m_sClientDocList IsNot Nothing AndAlso m_sClientDocList.Count > 0 Then
                Dim docCodeList = m_oDocManager.ResolvedDocumentCodeList
                If docCodeList IsNot Nothing AndAlso docCodeList.Count > 0 Then
                    m_oSplitDocMergedCodes = New List(Of String)(docCodeList)
                End If
            End If
            'm_sDocumentTextContents = m_oDocManager.DocumentTextContents
            m_sWordVersion = m_oDocManager.WordVersion
            m_bArchiveAsXML = CType(m_oDocManager, bPMBDocManager.Interface_Renamed).ArchiveAsXML
            m_bArchiveAsText = CType(m_oDocManager, bPMBDocManager.Interface_Renamed).ArchiveAsText

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to merge the document", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PrintOrSpoolDocument
    '
    ' Description: Calls the print document function in the interface
    '
    ' History: 13/06/2000 CTAF - Created
    ' ***************************************************************** '
    Private Function PrintOrSpoolDocument(Optional ByRef IsSpooled As Boolean = False) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Silent or verbose?

        Select Case m_lMode
            Case gSIRLibrary.ACPrintSilentMode, gSIRLibrary.ACPrintMode
                ' Call PrintOrSpoolDocument on the form
                m_lReturn = PrintDocumentSilent()

            Case gSIRLibrary.ACSpoolDocMode, gSIRLibrary.ACSpoolReportMode
                If IsSpooled = False Then
                    If m_bSpoolDocument Then
                        If m_sClientDocList IsNot Nothing Then
                            For Each doc In m_sClientDocList
                                m_lReturn = SpoolDocument(v_sDesc:=m_sSpoolDesc, v_sDocName:=doc)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintOrSpoolDocument")
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            Next
                        End If
                        If Not String.IsNullOrEmpty(m_oDocManager.ResolvedDocumentName) Then
                            m_sClientDocument = m_oDocManager.ResolvedDocumentName
                            m_lReturn = SpoolDocument(v_sDesc:=m_sSpoolDesc, v_sDocName:="")
                        End If
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintOrSpoolDocument")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        Else
                            IsSpooled = True
                        End If
                    End If
                End If
            Case Else

        End Select

        'AK 110402 - check if there is a attached work manager task for this template
        '            if yes, create a workmanager task instance

        m_lReturn = CreateTask()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintOrSpoolDocument")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SpoolDocument
    '
    ' Description:
    '
    ' History: 08/05/2000 Tomo - Created.
    '           : 30/01/2001 Tinny - change it to public and add optional parameters
    '             for description and document to be spooled
    ' RAM20050527   : Code changes to Support Swift Document Production
    ' ***************************************************************** '
    Public Function SpoolDocument(Optional ByVal v_sDesc As String = "", Optional ByVal v_sDocName As String = "") As Integer

        Dim result As Integer = 0
        Dim vPartyCnt, vInsuranceFolderCnt, vInsuranceFileCnt, vClaimCnt As Object
        Dim sSpoolFile As String = ""
        Dim sTemp As String = ""
        Dim sDescription As String = ""
        Dim sSpoolDoc As String = ""
        Dim sSpoolZip As String = ""
        Dim sSpoolPDF As String = ""
        Dim iTemp As Integer

        'TN20010801
        Dim sPrinterName As String = ""
        Dim lArchiveCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_sClient.Length = 0 Then

                m_sClient = m_oDocManager.Client
            End If
            If m_sServer.Length = 0 Then

                m_sServer = m_oDocManager.Server
            End If
            If m_sClientDocument.Length = 0 Then
                m_sClientDocument = m_oDocManager.ResolvedDocumentName
            End If

            If m_oDocManager.ResolvedDocumentList IsNot Nothing Then

                m_sClientDocList = New List(Of String)(m_oDocManager.ResolvedDocumentList)
            Else
                m_sClientDocList = New List(Of String)() ' Initialize as empty list
            End If


            'Set up some variables
            If m_lPartyCnt = 0 Then


                vPartyCnt = DBNull.Value
            Else

                vPartyCnt = m_lPartyCnt
            End If

            If m_lInsuranceFolderCnt = 0 Then


                vInsuranceFolderCnt = DBNull.Value
            Else

                vInsuranceFolderCnt = m_lInsuranceFolderCnt
            End If

            If m_lInsuranceFileCnt = 0 Then


                vInsuranceFileCnt = DBNull.Value
            Else

                vInsuranceFileCnt = m_lInsuranceFileCnt
            End If

            If m_lClaimCnt = 0 Then


                vClaimCnt = DBNull.Value
            Else

                vClaimCnt = m_lClaimCnt
            End If

            If v_sDesc <> "" Then
                sDescription = v_sDesc
            End If

            If sDescription.Trim() = "" Then
                sDescription = m_sDocumentTemplateDescription
            End If

            If v_sDocName <> "" Then
                m_sClientDocument = v_sDocName
                sSpoolDoc = v_sDocName
            End If

            lArchiveCount = 0

            ' don't bother to set up document variables if we are spooling
            ' a report (ie Crystal generated)
            If m_lMode <> gSIRLibrary.ACSpoolReportMode Then

                Dim sSTR() As String
                Dim sExtn As String = ""

                sSTR = Split(m_sClientDocument, ".")

                If sSTR.GetUpperBound(0) > 0 Then
                    sExtn = sSTR(1).ToUpper
                End If

                If sExtn.ToUpper = "DOC" Then
                    ' don't bother to set up document variables if we are spooling
                    ' a report (ie Crystal generated)
                    m_lReturn = SetDocumentVariables()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetDocumentVariables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Save as a word document
                    sSpoolDoc = m_sClientDocument.Substring(0, m_sClientDocument.Length - 3) & "doc"

                    m_oDocument.SaveAs(FileName:=sSpoolDoc, FileFormat:=Word.WdSaveFormat.wdFormatDocument)
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    m_oDocument.Close()

                    m_oDocument = Nothing
                Else
                    sSpoolDoc = m_sClientDocument
                End If

            End If

            'Get template printer

            m_lReturn = m_oBusiness.GetTemplatePrinter(v_lDocTemplateId:=m_lDocumentTemplateId, r_sPrinterName:=sPrinterName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRDocTemplate.Business.GetTemplatePrinter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            ' RAM20050527 : We don't need to add to the spooler database table, if we are calling this
            '               from swift. Reason being, the PartyCnt passed in the PartyCnt in Swift
            '               NOT Sirius.
            If Not m_bCalledFromSwift Then
                ' PGR 8.11
                If m_bArchiveWithNoPrint Then
                    lArchiveCount = 1
                End If

                'Add a record to the document spooler

                m_lReturn = m_oDocSpooler.DirectAdd(vDocumentSpoolerId:=m_lSpoolNumber, vDocumentTypeId:=m_lDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=sDescription, vIsEditable:=m_iIsEditableAfterMerging, vPrinter:=sPrinterName, vSpoolLevelInd:=1, vDocumentTemplateID:=m_lDocumentTemplateId, v_iIsClient:=m_iIsClient, v_iIsAgent:=m_iIsAgent, v_iIsOffice:=m_iIsOffice, v_iOrderByProductionOrder:=m_iProductionOrder, vTimesArchived:=lArchiveCount)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRDocSpooler.Business.DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Get the path of the spool file
            m_lReturn = GetSpoolFilePath(v_sBaseDirectory:=m_sServer, v_bIsLevelTwo:=True, r_sSpoolDirectory:=sSpoolFile)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetSpoolFilePath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument")
                Return result
            End If

            'Build the path of the 'zip' file
            sSpoolZip = m_sClientDocument.Substring(0, m_sClientDocument.Length - 3) & "zip"

            'delete zip file if it exists
            sTemp = Path.GetFileName(sSpoolZip)
            If sTemp <> "" Then
                File.Delete(sSpoolZip)
            End If

            ' 'zip' up the document
            iTemp = m_oZipper.ZipFile(sFileIn:=sSpoolDoc, sFileOut:=sSpoolZip)

            If Not iTemp Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMZipper.Business.ZipFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sSpoolPDF.Length > 0 Then
                If m_bSpoolAsPDF Then
                    'sTemp = Directory.GetFiles(sSpoolZip, FileAttribute.Normal)(0)
                    sTemp = Path.GetFileName(sSpoolZip)
                    If sTemp.Length > 0 Then
                        If File.Exists(sSpoolPDF) Then
                            File.Delete(sSpoolPDF)
                        End If
                    End If
                End If
            End If
            ' Copy it to the spool directory on the server
            File.Copy(sSpoolZip, sSpoolFile, True)

            File.Delete(sSpoolZip)
            If (Not m_bSpoolAsHTML) And m_lMode <> ACSpoolDocMode Then
                File.Delete(sSpoolDoc)
            End If

            'TJB : 10/12/2002
            m_sMergedFilePath = sSpoolDoc
            m_sSpooledFilePath = sSpoolFile

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetSpoolFilePath
    '
    ' Description:
    '
    ' History: 14/10/2002 sj - Created.
    '
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
        r_sSpoolDirectory = v_sBaseDirectory & "\Spooled Documents"
        sTemp = FileSystem.Dir(r_sSpoolDirectory)

        If sTemp = "" Then
            Directory.CreateDirectory(r_sSpoolDirectory)
        End If

        r_sSpoolDirectory = v_sBaseDirectory & "\Spooled Documents" & "\Company " & CStr(m_iSourceID)

        sTemp = FileSystem.Dir(r_sSpoolDirectory)
        If sTemp = "" Then
            Directory.CreateDirectory(r_sSpoolDirectory)
        End If

        r_sSpoolDirectory = v_sBaseDirectory & "\Spooled Documents" & "\Company " & CStr(m_iSourceID) & "\" & StringsHelper.Format(lTemp, "000")

        sTemp = FileSystem.Dir(r_sSpoolDirectory)
        If sTemp = "" Then
            Directory.CreateDirectory(r_sSpoolDirectory)
        End If

        If v_bIsLevelTwo Then
            r_sSpoolDirectory = v_sBaseDirectory & "\Spooled Documents" & "\Company " & CStr(m_iSourceID) & "\" & StringsHelper.Format(lTemp, "000") & "\" & StringsHelper.Format(lTemp1, "000")
            sTemp = FileSystem.Dir(r_sSpoolDirectory)
            If sTemp = "" Then
                Directory.CreateDirectory(r_sSpoolDirectory)
            End If
        End If

        r_sSpoolDirectory = r_sSpoolDirectory & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: PrintDocumentSilent
    '
    ' Description:
    '
    ' History: 15/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function PrintDocumentSilent() As Integer
        Dim result As Integer = 0
        Dim sPrinterName As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        'Get template printer
        m_lReturn = m_oBusiness.GetTemplatePrinter(
            v_lDocTemplateId:=m_lDocumentTemplateId,
                r_sPrinterName:=sPrinterName)
        If m_sClientDocList IsNot Nothing AndAlso m_sClientDocList.Count > 0 Then
            For Each doc In m_sClientDocList
                m_lReturn = m_oDocManager.PrintDoc(doc, sPrinterName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            Next
        End If
        If Not String.IsNullOrEmpty(m_sClientDocument) Then
            m_lReturn = m_oDocManager.PrintDoc(m_sClientDocument, sPrinterName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End If

        Return result
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
        Dim oDocument As Word.Document
        Dim aVar As Word.Variable
        Dim lCompanyIdIndex, lPartyCntIndex, lInsuranceFolderCntIndex, lInsuranceFileCntIndex, lClaimCntIndex, lDocumentTypeIdIndex, lDocumentTypeDescriptionIndex, lFormatIndex As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = OpenDocument()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDocumentVariables")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            m_lReturn = LaunchOurDoc()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LaunchOurDoc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDocumentVariables")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        oDocument = m_oDocument

        'Get any already stored values - there shouldn't be any, but if there are and we
        'don't check we'll get an error when assigning them
        For Each aVar2 As Word.Variable In oDocument.Variables
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
            oDocument.Variables.Add(Name:="CompanyId", Value:=m_iSourceID)
        Else
            oDocument.Variables.Item(lCompanyIdIndex).Value = CStr(m_iSourceID)
        End If

        If lPartyCntIndex = 0 Then
            oDocument.Variables.Add(Name:="PartyCnt", Value:=m_lPartyCnt)
        Else
            oDocument.Variables.Item(lPartyCntIndex).Value = CStr(m_lPartyCnt)
        End If

        If m_lInsuranceFolderCnt <> 0 Then
            If lInsuranceFolderCntIndex = 0 Then
                oDocument.Variables.Add(Name:="InsuranceFolderCnt", Value:=m_lInsuranceFolderCnt)
            Else
                oDocument.Variables.Item(lInsuranceFolderCntIndex).Value = CStr(m_lInsuranceFolderCnt)
            End If
        End If

        If m_lInsuranceFileCnt <> 0 Then
            If lInsuranceFileCntIndex = 0 Then
                oDocument.Variables.Add(Name:="InsuranceFileCnt", Value:=m_lInsuranceFileCnt)
            Else
                oDocument.Variables.Item(lInsuranceFileCntIndex).Value = CStr(m_lInsuranceFileCnt)
            End If
        End If

        If m_lClaimCnt <> 0 Then
            If lClaimCntIndex = 0 Then
                oDocument.Variables.Add(Name:="ClaimCnt", Value:=m_lClaimCnt)
            Else
                oDocument.Variables.Item(lClaimCntIndex).Value = CStr(m_lClaimCnt)
            End If
        End If

        If lDocumentTypeIdIndex = 0 Then
            oDocument.Variables.Add(Name:="DocumentTypeId", Value:=m_lDocumentTypeId)
        Else
            oDocument.Variables.Item(lDocumentTypeIdIndex).Value = CStr(m_lDocumentTypeId)
        End If

        If lDocumentTypeDescriptionIndex = 0 Then
            oDocument.Variables.Add(Name:="DocumentTypeDescription", Value:=m_sDocumentTemplateDescription)
        Else
            oDocument.Variables.Item(lDocumentTypeDescriptionIndex).Value = m_sDocumentTemplateDescription
        End If

        If lFormatIndex = 0 Then
            oDocument.Variables.Add(Name:="FMFormat", Value:="RTF")
        Else
            oDocument.Variables.Item(lFormatIndex).Value = "RTF"
        End If

        oDocument = Nothing

        Return result

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
        Dim sWindowText, sGUID As String


        result = gPMConstants.PMEReturnCode.PMTrue

        'Launch Word.
        m_oWord = New Word.Application()

        sWindowText = m_oWord.Caption
        sGUID = bPMFunc.GetGUID()
        m_oWord.Caption = sGUID

        m_lWordHwnd = FindWindow(Nothing, sGUID)

        m_oWord.Caption = sWindowText

        If m_lWordHwnd = 0 Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Get Word Handle", vApp:=ACApp, vClass:=ACClass, vMethod:="LaunchOurDoc")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Open current document.
        m_lReturn = OpenDocument()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LaunchOurDoc")
            Return gPMConstants.PMEReturnCode.PMFalse
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
            m_lReturn = IsWindow(m_lWordHwnd)


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
    ' Name: OpenDocument
    '
    ' Description:
    '
    ' History: 28/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function OpenDocument() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDocument = m_oWord.Documents.Open(CStr(m_sClientDocument), ConfirmConversions:=False)
        ' Application.DoEvents()

        Return result

    End Function

    ' AK 110402
    ' Function to check the existance of work manager task and create them
    ' Doc production in print/print-silent/spool mode will use this

    Public Function CreateTask() As Integer
        Dim result As Integer = 0
        Try
            Dim vTaskId As Object = Nothing
            Dim lInstanceID As Integer
            Dim vPartyType As Object = Nothing
            Dim lPMWrkTaskGroupId, lPMWrkTaskId As Integer
            Dim sCustomer As String = ""
            Dim dtTaskDueDate As Date
            Dim lPMUserGroupID As Integer
            Dim iUserID As Integer
            Dim sDescription As String = ""
            Dim iTaskStatus, iIsUrgent As Integer
            Dim dtDateCreated As Date
            Dim iCreatedByID As Integer
            Dim dtLastModified As Date
            Dim iModifiedByID As Integer
            Dim vPartyPolicy As Object = Nothing

            'AK 220402
            Dim iNumDays As Integer


            result = gPMConstants.PMEReturnCode.PMTrue

            'If there is a task attached to this document then get the task_id

            m_lReturn = m_oDocTemplate.GetTaskID(r_vTaskId:=vTaskId, m_lDocumentTemplateId:=m_lDocumentTemplateId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRDocTemplate.Business.GetTaskID Failed for document_template_id " & m_lDocumentTemplateId, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTask")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vTaskId) Then
                'No task for this document
                Return result
            End If


            If String.IsNullOrEmpty(vTaskId(0, 0)) Then
                'No task for this document
                Return result
            End If

            'Get customer details

            m_lReturn = m_oDocTemplate.GetPartyPolicy(r_vArray:=vPartyPolicy, m_lInsuranceFileCnt:=m_lInsuranceFileCnt, m_lPartyCnt:=m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRDocTemplate.Business.GetPartyPolicy Failed for party_cnt " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTask")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the task related details

            m_lReturn = m_oTaskInstance.GetDetails(v_lPMWrkTaskInstanceCnt:=vTaskId(0, 0), r_lPMWrkTaskGroupID:=lPMWrkTaskGroupId, r_lPMWrkTaskID:=lPMWrkTaskId, r_sCustomer:=sCustomer, r_dtTaskDueDate:=dtTaskDueDate, r_lPMUserGroupID:=lPMUserGroupID, r_iUserID:=iUserID, r_sDescription:=sDescription, r_iTaskStatus:=iTaskStatus, r_iIsUrgent:=iIsUrgent, r_dtDateCreated:=dtDateCreated, r_iCreatedByID:=iCreatedByID, r_dtLastModified:=dtLastModified, r_iModifiedByID:=iModifiedByID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMWrkTaskInstanceTemp.FormClass.GetDetails Failed for task_id " & lPMWrkTaskId, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTask")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the party type

            m_lReturn = m_oBusiness.GetPartyType(r_vPartytype:=vPartyType, v_lPartyCnt:=m_lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRDocTemplate.Business.GetPartyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTask")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AK 220402 - get the number of days between - TaskDueDate and DateCreated
            iNumDays = Informations.DateDiff("d", dtTaskDueDate, dtDateCreated, FirstDayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
            'Set the customer like this,  as being done by the interface


            sCustomer = CStr(vPartyPolicy(1)) & " - " & CStr(vPartyPolicy(2))
            dtDateCreated = DateTime.Today
            'AK 220402 - set it to the number of days difference, calculated earlier
            dtTaskDueDate = DateTime.Today.AddDays(iNumDays).AddDays(CDate("23:59:59").ToOADate())


            m_lReturn = m_oTaskInstance.CreateNew(r_lPMWrkTaskInstanceCnt:=lInstanceID, v_lPMWrkTaskGroupID:=lPMWrkTaskGroupId, v_lPMWrkTaskID:=lPMWrkTaskId, v_sCustomer:=sCustomer, v_dtTaskDueDate:=dtTaskDueDate, v_lPMUserGroupID:=lPMUserGroupID, v_iUserID:=iUserID, v_sDescription:=sDescription, v_iTaskStatus:=iTaskStatus, v_iIsUrgent:=iIsUrgent, v_dtDateCreated:=dtDateCreated, v_iCreatedByID:=iCreatedByID, v_bIsNewInstance:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMWrkTaskInstanceTemp.FormClass.CreateNew Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTask")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create the task instance keys now


            m_lReturn = m_oTaskInstance.CreateKeys(v_lTaskInstanceID:=lInstanceID, v_lPartyCnt:=m_lPartyCnt, v_sShortName:=vPartyPolicy(1), v_sPartyType:=CStr(vPartyType(0, 0)).Trim(), v_sResolvedName:=CStr(vPartyType(1, 0)).Trim(), v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMWrkTaskInstanceTemp.FormClass.CreateKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTask")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SendToFish
    '
    ' Description:
    '
    ' History: 26/09/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function SendToFish() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sMessage As String = ""

            sMessage = "Document Path: " & m_sClientDocument & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "PartyCnt: " & CStr(m_lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            sMessage = sMessage & "InsuranceFileCnt: " & CStr(m_lInsuranceFileCnt)

            'CJR this is not meant to be an interface component
            'MsgBox sMessage, vbOKOnly, "SendToFish"

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendToFish Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToFish", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Establish RequiredTemplate
    '
    ' Description:
    '
    ' History: 26/09/2000 RWH - Created.
    '        : PW160702 - change to use Process Types Docs lookup table
    '                     instead of constants
    ' ***************************************************************** '
    Private Function EstablishRequiredTemplate() As Integer

        Dim result As Integer = 0
        Dim sTemplateCode As String = ""
        Dim lReportPointer As Integer
        Dim sBusinessType As String = ""

        ' PW160702
        Dim vResultArray(,) As Object
        Dim vTabArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue
        '
        ' PW160702 - use the new lookups table to get Process Type Code
        '

        vResultArray = Nothing
        ReDim vTabArray(3, 0)


        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "process_types_docs"

        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = m_lProcessTypesDocsId


        m_lReturn = m_oFindDocTemplate.GetProcessTypesLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, vResultArray:=vResultArray)

        ' PW160702 - Assigned returned lookup type

        sTemplateCode = CStr(vResultArray(2, 0)).Trim()

        ' PW160702 - If cancellation...
        If sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancel Then


            m_lReturn = m_oFindDocTemplate.GetBusinessType(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_sBusinessType:=sBusinessType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case sBusinessType
                Case "DIRECT"
                    sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancelClient
                Case Else
                    sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancelAgent
            End Select

        End If

        'TJB : 10/12/2002
        'This line was commented out
        sTemplateCode = sTemplateCode & m_sTransactionType.Trim()


        m_lReturn = m_oFindDocTemplate.GetReportPointer(m_lInsuranceFileCnt, lReportPointer)


        If lReportPointer <> 0 Then
            sTemplateCode = sTemplateCode & CStr(lReportPointer)
        End If

        'Ensure template exists. If not, apply rules until  suitable template is found.

        m_lReturn = m_oFindDocTemplate.GetAvailableTemplate(sTemplateCode, m_lDocumentTemplateId, m_lDocumentTypeId, m_sDocumentTemplateDescription, m_dtEffectiveDate)

        Select Case (m_lReturn)
            Case gPMConstants.PMEReturnCode.PMTrue
                'That's OK.
            Case gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Template not found with code '" & sTemplateCode & "'.", vApp:=ACApp, vClass:=ACClass, vMethod:="EstablishRequiredTemplate")
                Return result
            Case Else
                'Log error.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get available template", vApp:=ACApp, vClass:=ACClass, vMethod:="EstablishRequiredTemplate")
                Return result
        End Select

        Return result

    End Function


    ''' <summary>
    ''' Archive a document in the destination defined in the the system options. This is currently Documaster or Sharepoint
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ArchiveDocument() As Integer
        Return ArchiveDocument(sArchiveDocFileName:="")
    End Function
    ''' <summary>
    ''' Archive a document in the destination defined in the the system options. This is currently Documaster or Sharepoint
    ''' </summary>
    ''' <param name="sArchiveDocFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ArchiveDocument(ByVal sArchiveDocFileName As String) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sDocType As String = ""
        Dim sPageType As String = ""
        Dim lDocNumber As Integer
        Dim sOptionValue As String = ""
        Dim vIndexArray(,) As Object = Nothing
        Dim sArchiveDoc As String = ""
        Dim sArchiveAsPDF As String = ""
        Dim sDocName As String = ""
        Dim sEffectiveDoc As String = If(Not String.IsNullOrEmpty(sArchiveDocFileName), sArchiveDocFileName, m_sClientDocument)
        ' Sanitise double extension e.g. Doc 100250.xml.xml -> Doc 100250.xml
        sEffectiveDoc = System.Text.RegularExpressions.Regex.Replace(sEffectiveDoc, "\.xml\.xml$", ".xml", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim sExtension As String = IO.Path.GetExtension(sEffectiveDoc).ToUpper
        Dim sTemp As String = ""
        Dim v_vArchiveDocumentPath As Object = Nothing

        Const ARCHIVE_AS_PDF As Integer = 5009
        If m_oDocManager.ResolvedDocumentList IsNot Nothing Then
            sArchiveDoc = sArchiveDocFileName
        End If
        If String.IsNullOrEmpty(sArchiveDocFileName) AndAlso Not String.IsNullOrEmpty(m_oDocManager.ResolvedDocumentName) Then
            m_sMergedFilePath = m_sClientDocument
            sArchiveDoc = m_sClientDocument
        End If


        'Now check is Archive as PDF is on at System Options

        m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=ARCHIVE_AS_PDF, r_sOptionValue:=sArchiveAsPDF)

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
            sArchiveDoc = sClient
        ElseIf m_bArchiveAsXML Then
            sDocType = "S"
            sPageType = "XML"

            If sExtension = ".XML" Then
                sArchiveDoc = sEffectiveDoc.Substring(0, sEffectiveDoc.Length - sExtension.Length) & ".xml"
                Dim filepath(), filename As String
                Dim i As Integer = 0
                filepath = sArchiveDoc.Split("\")
                filename = filepath(filepath.Length - 1)
                filename = "ResolvedXML_" + filename
                sArchiveDoc = ""
                For i = 0 To filepath.Length - 2
                    sArchiveDoc += filepath(i) + "\"
                Next
                sArchiveDoc = sArchiveDoc + filename
            End If
        ElseIf ((sArchiveAsPDF <> "0") Or m_bSpoolAsPDF) AndAlso m_lDocumentTemplateId <> 0 Then
            sDocType = "F"
            sPageType = "PDF"
            'Since the Direct document is routed tothe background job,add condition to avoid pdf to pdf conversions
            If sExtension <> ".PDF" Then
                If (sExtension = ".DOC" Or sExtension = ".DOCX" Or sExtension = ".XML" Or m_sDocumentTemplateCode.Length > 0) _
                    AndAlso (m_bCalledFromSAM Or m_bIsCalledFromBackGroundJob) Then
                    sArchiveDoc = sEffectiveDoc.Substring(0, sEffectiveDoc.Length - sExtension.Length) & ".pdf"
                    m_lReturn = m_oDocManager.ConvertDocumentUsingSiriusDocumentUtility(v_sSourceDocument:=sEffectiveDoc, v_sDestDocument:=sArchiveDoc)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to convert the following document to PDF - " & sEffectiveDoc, vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument")

                        Return m_lReturn
                    End If
                    m_sMergedFilePath = sArchiveDoc
                End If
            End If
        ElseIf sExtension = ".DOC" OrElse sExtension = ".XML" OrElse sExtension.ToUpper() = ".DOCX" Then
            'Default format is DocX
            sDocType = "D"
            sPageType = "DOC"
            sArchiveDoc = sEffectiveDoc.Substring(0, sEffectiveDoc.Length - sExtension.Length) & ".docx"

            m_lReturn = m_oDocManager.ConvertDocumentUsingSiriusDocumentUtility(v_sSourceDocument:=sEffectiveDoc, v_sDestDocument:=sArchiveDoc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to convert the following document to PDF - " & sEffectiveDoc, vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument")

                Return m_lReturn
            End If
        End If

        'SAM Is expecting a zip containing the pdf or xml
        If m_bCalledFromSAM Then
            Dim sZipPath As String = ""
            Dim iTemp As Integer

            Dim sRndNumber As String = ""
            Dim vArchieveDocPathSplit As Object
            Dim iUpperBound As Integer

            ' PN77305
            vArchieveDocPathSplit = Split(sArchiveDoc, "\")
            If Informations.IsArray(vArchieveDocPathSplit) Then
                iUpperBound = vArchieveDocPathSplit.GetUpperBound(0)
                If iUpperBound > 0 Then
                    iUpperBound = iUpperBound - 1
                    sRndNumber = vArchieveDocPathSplit(iUpperBound)
                End If
            End If

            sZipPath = If(Environ$("tmp") <> "", Environ$("tmp"), Environ$("temp"))

            If sRndNumber <> "" Then
                sZipPath = sZipPath & "\" & sRndNumber & ".zip"
            Else
                Dim rnd As New Random
                sZipPath = sZipPath & "\" & StringsHelper.Replace(rnd.Next, ".", "") & ".zip"
            End If
            'sTemp = Directory.GetFiles(sZipPath, FileAttribute.Normal)(0)
            sTemp = Path.GetFileName(sZipPath)
            If sTemp.Length > 0 Then
                If File.Exists(sZipPath) Then
                    File.Delete(sZipPath)
                End If
            End If

            iTemp = m_oZipper.ZipFile(sFileIn:=sArchiveDoc, sFileOut:=sZipPath)

            m_sSpooledFilePath = sZipPath
            m_sMergedFilePath = sArchiveDoc
        End If

        If Not Informations.IsArray(v_vArchiveDocumentPath) Then
            v_vArchiveDocumentPath = sArchiveDoc
        End If

        If m_bArchiveWithNoPrint = True _
            Or m_bArchiveDoc = True _
            Or m_bArchiveWithNoMerge Then


            'Check if we have documaster installed
            m_lReturn = GetOption(v_iOptionNumber:=10, r_sOptionValue:=sOptionValue)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Dim sDestinationFilenameSharePoint As String = ""
            If IsDMEMigration And m_bIsCalledFromBackGroundJob And sOptionValue = "2" Then
                sDestinationFilenameSharePoint = m_sDestinationFilename
            End If
            If m_sDestinationFilename IsNot Nothing And Not String.IsNullOrEmpty(m_sDestinationFilename) Then
                If IO.Path.GetExtension(m_sDestinationFilename).ToUpper <> sPageType.ToUpper Then
                    m_sDestinationFilename = IO.Path.GetFileNameWithoutExtension(m_sDestinationFilename) & "." & sPageType
                End If
            End If

            If sOptionValue = "1" Then
                'Create the document API object
                If m_oSIRDOCAPI Is Nothing Then
                    m_oSIRDOCAPI = New bSIRDOCAPI.Form()
                    m_lReturn = m_oSIRDOCAPI.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If
                End If
                'It's not used, but we need to define it anyway...
                Dim sKeywords(0) As String

                'Populate the vIndexArray Variable
                m_lReturn = EncryptKeyArray(r_vKeyArray:=vIndexArray)
                If m_sDestinationFilename <> "" Then
                    sDocName = m_sDestinationFilename
                ElseIf m_sDocName.Trim() <> "" Then
                    sDocName = m_sDocName.Trim()
                Else
                    sDocName = m_sDocumentTemplateDescription.Trim()
                End If
                If sDocName.Trim() = String.Empty AndAlso m_bIsCalledFromBackGroundJob Then
                    sDocName = m_sClientDocument.Substring(m_sClientDocument.LastIndexOf("\") + 1, m_sClientDocument.Length - m_sClientDocument.LastIndexOf("\") - 1)
                End If

                m_lReturn = m_oSIRDOCAPI.AddDocument(lPartyId:=m_lPartyCnt, sPartyName:="", lInsuranceFolderId:=m_lInsuranceFolderCnt,
                                                     sInsuranceFileRef:=m_sInsuranceFileRef, lClaimId:=m_lClaimCnt, sClaimRef:=m_sClaimRef,
                                                     lFSAComplaintFolderCnt:=0, sFSAComplaintReference:="", sDocType:=sDocType, sPageType:=sPageType,
                                                     sDocName:=sDocName, sFilename:=sArchiveDoc, sAnnotation:="", sKeywords:=sKeywords, lDocNumber:=lDocNumber, vDocumentTemplateID:=m_lDocumentTemplateId, bVisibleFromWeb:=gPMConstants.PMEReturnCode.PMTrue, bArchiveAsText:=m_bArchiveAsText, bArchiveAsXML:=m_bArchiveAsXML,
                                                     DocumentTemplateGroupID:=m_iDocumentTemplateGroupID, DocumentTemplateSubGroupID:=m_iDocumentTemplateSubGroupID)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lDocumasterId = 0
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Archive Document", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If

                ' ME : 28-11-2002 : 202 Loss Schedule
                ' Set the Documaster Id for this document
                m_lDocumasterId = lDocNumber
            ElseIf sOptionValue = "2" Then
                'Sharepoint Integration
                If m_oSharePoint Is Nothing Then
                    m_oSharePoint = New bSIRSharepoint.Business()

                    m_lReturn = m_oSharePoint.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the m_oSharePoint object", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return nResult
                    End If
                End If
                Dim oFileInfo As New FileInfo(sArchiveDoc)
                If m_sDocumentTemplateDescription IsNot Nothing Then
                    If Not m_sDocumentTemplateDescription.Contains(".") Then
                        m_sDestinationFilename = m_sDocumentTemplateDescription & oFileInfo.Extension
                    End If
                Else

                    If ToSafeString(m_sDestinationFilename).Trim = "" Then
                        m_sDestinationFilename = oFileInfo.Name
                    End If
                    If Not m_sDestinationFilename.Contains(".") Then
                        m_sDestinationFilename = m_sDestinationFilename & oFileInfo.Extension
                    End If
                End If
                oFileInfo = Nothing

                nResult = m_oSharePoint.ArchiveDocument(PartyCnt:=m_lPartyCnt, InsuranceFileCnt:=m_lInsuranceFileCnt,
                                                ClaimID:=m_lClaimCnt, CaseID:=0, DocumentTemplateID:=m_lDocumentTemplateId,
                                                TemplateGroupID:=m_iDocumentTemplateGroupID,
                                                TemplateSubGroupID:=m_iDocumentTemplateSubGroupID,
                                                SourceFile:=sArchiveDoc, InternalOnly:=m_bInternalOnly, SharepointPath:=sArchiveDoc,
                                                DestinationFilename:=IIf(String.IsNullOrEmpty(sDestinationFilenameSharePoint), m_sDestinationFilename, sDestinationFilenameSharePoint),
                                                PartyCode:=m_sPartyCode, PolicyNumber:=m_sInsuranceFileRef,
                                                ClaimNumber:=m_sClaimRef, Background_Job_Id:=m_Background_Job_Id,
                                                IsGeneratedMail:=m_bIsGeneratedMail, sArchiveDocFileName:=sArchiveDocFileName,
                                                bIsCalledFromBackGroundJob:=m_bIsCalledFromBackGroundJob,
                                                bIsDMEMigration:=m_bIsDMEMigration,
                                                sCreateddate:=ToSafeDate(m_sCreatedDate))

            End If
        End If
        Return nResult

    End Function
    ''' <summary>
    ''' ConvertDocumentUsingSiriusDocumentUtility
    ''' </summary>
    ''' <param name="v_sSourceDocument"></param>
    ''' <param name="v_sDestDocument"></param>
    ''' <returns></returns>
    Public Function ConvertDocumentUsingSiriusDocumentUtility(ByVal v_sSourceDocument As String, ByVal v_sDestDocument As String) As Integer
        Dim result As Integer = 0
        ' Dim SiriusDocumentUtility As Object
        Const kMethodName As String = "ConvertDocumentUsingSiriusDocumentUtility"
        Dim oConvert As SiriusDocumentUtility.Document

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oConvert = New SiriusDocumentUtility.Document()

            oConvert.Convert(v_sSourceDocument, v_sDestDocument)

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)
        Finally
            oConvert = Nothing

            '        Return result

            ' This is for debugging only
            '        
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get a system option.
    '
    ' PSL 27/11/2002 Created
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer
        Dim result As Integer = 0
        Dim m_oSystemOption As bSIROptions.Business = Nothing

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSystemOption Is Nothing Then
                m_oSystemOption = New bSIROptions.Business()
                m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            m_oSystemOption = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' Function Name : EncryptKeyArray
    '
    ' Description   : Function to Initialise the supplied key array, with the proper
    '                   values, as it will be recogonised by Documaster
    '
    ' Author        : Ram Chandrabose
    '
    ' Notes         : 1. This function is introduced to support the changes made in DME
    '                    which now supports 5 Levels of Folder (for future more)
    '                 2. This will be called from UpdateFileMaster Function
    '
    ' REF           : NRMA Project Changes. Sirius Process No. 189
    '
    ' Edit History  :
    ' RAM20021217   : Created
    ' **************************************************************** '
    Private Function EncryptKeyArray(ByRef r_vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const KeyLevelSourceID As Integer = 0
        Const KeyLevelPartyCnt As Integer = 1
        Const KeyLevelPartyShortName As Integer = 2
        Const KeyLevelInsuranceFileCnt As Integer = 3
        Const KeyLevelInsuranceFolderCnt As Integer = 4
        Const KeyLevelInsuranceFileRef As Integer = 5
        Const KeyLevelClaimID As Integer = 6
        Const KeyLevelClaimRef As Integer = 7
        Const KeyLevelLossScheduleID As Integer = 8
        Const KeyLevelLossScheduleRef As Integer = 9

        Const KeyNameSourceID As String = "SourceID"
        Const KeyNamePartyCnt As String = "PartyCnt"
        Const KeyNamePartyShortName As String = "PartyShortName"
        Const KeyNameInsuranceFileCnt As String = "InsuranceFileCnt"
        Const KeyNameInsuranceFolderCnt As String = "InsuranceFolderCnt"
        Const KeyNameInsuranceFileRef As String = "InsuranceFileRef"
        Const KeyNameClaimID As String = "ClaimID"
        Const KeyNameClaimRef As String = "ClaimRef"
        Const KeyNameLossScheduleID As String = "LossScheduleID"
        Const KeyNameLossScheduleRef As String = "LossScheduleRef"

        Const KeyLevelMAX As Integer = 9 ' ie. 0 To 9  (so 10 Keys and values)

        Dim vIndexArray As Object ' Variable to store the KeyNames and values



        result = gPMConstants.PMEReturnCode.PMTrue

        'Initialise the vIndexArray
        ReDim vIndexArray(1, KeyLevelMAX)

        'Set the Key Names

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelSourceID) = KeyNameSourceID

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelPartyCnt) = KeyNamePartyCnt

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelPartyShortName) = KeyNamePartyShortName

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelInsuranceFileCnt) = KeyNameInsuranceFileCnt

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelInsuranceFolderCnt) = KeyNameInsuranceFolderCnt

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelInsuranceFileRef) = KeyNameInsuranceFileRef

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelClaimID) = KeyNameClaimID

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelClaimRef) = KeyNameClaimRef

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelLossScheduleID) = KeyNameLossScheduleID

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, KeyLevelLossScheduleRef) = KeyNameLossScheduleRef

        ' Set the values

        ' We are setting the SourceID to 0, since it will be fetched in the bSIRDOCAPI
        ' so don't worry.


        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelSourceID) = 0 ' Party's Source ID

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelPartyCnt) = m_lPartyCnt ' Party Cnt

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelPartyShortName) = m_sPartyName ' Party Short Name

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelInsuranceFileCnt) = m_lInsuranceFileCnt

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelInsuranceFolderCnt) = m_lInsuranceFolderCnt

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelInsuranceFileRef) = m_sInsuranceFileRef

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelClaimID) = m_lClaimCnt

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelClaimRef) = m_sClaimRef

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelLossScheduleID) = 0

        vIndexArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, KeyLevelLossScheduleRef) = ""



        r_vKeyArray = vIndexArray

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' ***************************************************************** '
    Public Function CreateEvent(Optional ByRef r_lEventCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vDocumentCnt As Object = Nothing, Optional ByVal v_vOldAddressCnt As Object = Nothing, Optional ByVal v_vNewAddressCnt As Object = Nothing, Optional ByVal v_vCampaignId As Object = Nothing, Optional ByVal v_vDocumentTypeId As Object = Nothing, Optional ByVal v_vReportTypeId As Object = Nothing, Optional ByVal v_lEventTypeId As Integer = 0, Optional ByVal v_dtEventDate As Date = #12/30/1899#, Optional ByVal v_vDescription As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sDocumentpath As String = String.Empty
        Dim iEventType As Integer = 0
        Dim sDescription As String = String.Empty
        Dim sClientDoc As String

        If m_sClientDocument.Trim().ToString.Substring(m_sClientDocument.Trim().ToString.Length - 3, 3).ToString.ToUpper = "EML" Then
            sClientDoc = m_sClientDocument
        Else
            sClientDoc = MergedFilePath
        End If

        If ToSafeInteger(v_vDocumentCnt) = 0 Then
            If String.IsNullOrEmpty(m_sDocumentTemplateDescription) Then
                Dim oLookup As BPMLOOKUP.Business = Nothing
                'Dim lID As Integer
                sDocumentpath = sClientDoc.Trim().ToString.Split("\")(sClientDoc.Trim().ToString.Split("\").Length - 1)
                If sClientDoc.Trim().ToString.Substring(sClientDoc.Trim().ToString.Length - 3, 3).ToString.ToUpper = "EML" Then
                    sDocumentpath = "Generated Emails/" + sDocumentpath
                ElseIf String.IsNullOrEmpty(m_sInsuranceFileRef) And String.IsNullOrEmpty(ClaimRef) Then
                    sDocumentpath = "General/" + sDocumentpath
                ElseIf Not String.IsNullOrEmpty(m_sInsuranceFileRef) And String.IsNullOrEmpty(ClaimRef) Then
                    sDocumentpath = "Policy/" + m_sInsuranceFileRef + "/" + sDocumentpath
                ElseIf Not String.IsNullOrEmpty(ClaimRef) Then
                    sDocumentpath = "Claim/" + ClaimRef + "/" + sDocumentpath
                End If
                result = GetLookup(oLookup)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DocumentTypeCode")
                    Return result
                End If

                If sClientDoc.Trim().ToString.Substring(sClientDoc.Trim().ToString.Length - 3, 3).ToString.ToUpper = "EML" Then
                    sDescription = "Email Sent - " + sClientDoc.Trim().ToString.Split("\")(sClientDoc.Trim().ToString.Split("\").Length - 1)
                    result = oLookup.GetEffectiveIDFromCode("event_type", gPMConstants.PMEmailSentEventType, DateTime.Now, iEventType)
                Else
                    result = oLookup.GetEffectiveIDFromCode("event_type", gPMConstants.PMExternalDocUploadEventType, DateTime.Now, iEventType)
                    sDescription = "Document Uploaded - " + sClientDoc.Trim().ToString.Split("\")(sClientDoc.Trim().ToString.Split("\").Length - 1).ToString
                End If
            Else
                sDocumentpath = sClientDoc.Trim().ToString.Split("\")(sClientDoc.Trim().ToString.Split("\").Length - 1)
                If sClientDoc.Trim().ToString.Substring(sClientDoc.Trim().ToString.Length - 3, 3).ToString.ToUpper = "EML" Then
                    sDocumentpath = "Generated Emails/" + sDocumentpath
                ElseIf String.IsNullOrEmpty(m_sInsuranceFileRef) And String.IsNullOrEmpty(ClaimRef) Then
                    sDocumentpath = "General/" + sDocumentpath
                ElseIf Not String.IsNullOrEmpty(m_sInsuranceFileRef) And String.IsNullOrEmpty(ClaimRef) Then
                    sDocumentpath = "Policy/" + m_sInsuranceFileRef + "/" + m_sDestinationFilename
                ElseIf Not String.IsNullOrEmpty(ClaimRef) Then
                    sDocumentpath = "Claim/" + ClaimRef + "/" + m_sDestinationFilename
                End If
                sDescription = v_vDescription
            End If
        End If

        iEventType = v_lEventTypeId


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oEvent Is Nothing Then
                m_oEvent = New bSIREvent.Business()

                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            ' Directly add the event

            m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt,
                                           vPartyCnt:=v_lPartyCnt,
                                           vInsuranceFolderCnt:=v_vInsuranceFolderCnt,
                                           vInsuranceFileCnt:=gPMFunctions.ZeroToNull(v_vInsuranceFileCnt),
                                           vClaimCnt:=v_vClaimCnt,
                                           vDocumentCnt:=v_vDocumentCnt,
                                           vOldAddressCnt:=v_vOldAddressCnt,
                                           vNewAddressCnt:=v_vNewAddressCnt,
                                           vCampaignId:=v_vCampaignId,
                                           vDocumentType:=v_vDocumentTypeId,
                                           vReportType:=v_vReportTypeId,
                                           vEventType:=iEventType,
                                           vUserId:=m_iUserID,
                                           vEventDate:=v_dtEventDate,
                                           vDescription:=sDescription,
                                           vDocument_Path:=sDocumentpath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function EmailDocument() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EmailDocument"
        Dim sOptionValue As String = String.Empty
        Try
            Dim sMainEmailAddress As String = String.Empty

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
                If m_sClientDocList IsNot Nothing AndAlso m_sClientDocList.Count > 0 Then
                    For Each doc In m_sClientDocList
                        m_lReturn = m_oBusiness.SendEMail(v_sTo:=gPMFunctions.ToSafeString(sMainEmailAddress).Trim(), v_sSubject:=m_sDocumentTemplateDescription, v_sMessagePath:=doc, v_sAttachment:="")
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "SendEmail Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Next
                End If
                If Not String.IsNullOrEmpty(m_sClientDocument) Then
                    m_lReturn = m_oBusiness.SendEMail(v_sTo:=gPMFunctions.ToSafeString(sMainEmailAddress).Trim(), v_sSubject:=m_sDocumentTemplateDescription, v_sMessagePath:=m_sClientDocument, v_sAttachment:="")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SendEmail Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If
            Return result
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    Private Function AddFailedEmailWorkManagerTask() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "AddFailedEmailWorkManagerTask"
        Dim lTaskInstanceCnt As Integer
        Dim oTaskControl As bPMWrkTaskInstance.TaskControl
        Dim sGroupId As String = ""
        Dim vClientCode As Object = Nothing
        Dim vPmWrkTaskId As Object = Nothing

        Const lSIROptionFailedEmailWorkManagerTask As Integer = 5068
        result = gPMConstants.PMEReturnCode.PMTrue
        'If m_oObjectManager IsNot Nothing Then
        '    Dim temp_oTaskControl As Object = Nothing
        '    m_lReturn = m_oObjectManager.GetInstance(temp_oTaskControl, "bPMWrkTaskInstance.TaskControl", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        '    oTaskControl = temp_oTaskControl
        'Else
        oTaskControl = New bPMWrkTaskInstance.TaskControl

        '  End If
        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            gPMFunctions.RaiseError(kMethodName, "Failed To create the new task", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=lSIROptionFailedEmailWorkManagerTask, r_sOptionValue:=sGroupId, v_iSourceID:=m_iSourceID)
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

            m_lReturn = oTaskControl.CreateNew(v_lPMWrkTaskGroupID:=gPMFunctions.ToSafeLong(sGroupId), v_lPMWrkTaskID:=gPMFunctions.ToSafeLong(vPmWrkTaskId(0, 0)), v_sCustomer:="Email Document Failed", v_dtTaskDueDate:=DateTime.Now, v_lPMUserGroupID:=1, v_sDescription:="Attempt to email '" & m_sDocumentTemplateDescription & "' to party '" &
                        gPMFunctions.ToSafeString(vClientCode(0, 0)) & "' failed. No Main Email address present", v_iTaskStatus:=0, v_iIsUrgent:=1, r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_iIsVisible:=gPMConstants.PMEReturnCode.PMTrue)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                gPMFunctions.RaiseError(kMethodName, "Failed to cerate new task ", gPMConstants.PMELogLevel.PMLogError)
            End If
        Else
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed Email User Group not Configured", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
        End If

        Return result
    End Function
End Class
