Option Strict Off
Option Explicit On

Imports System.Data
Imports System.Data.SqlTypes
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Xml
Imports Aspose.Words
Imports Scripting
Imports SSP.Shared
Imports System.Drawing
Imports Microsoft.Win32
Imports System.Runtime.ExceptionServices
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
Public NotInheritable Class Interface_Renamed

    Implements IDisposable
    Private Const ACClass As String = "Interface"
    Private Const PolicyStandardWordingTag As String = "POLICYSTANDARDWORDINGS"
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lClaimCnt As Integer
    Private m_sDocumentRef As String = ""
    Private m_lDocumentTemplateId As Integer
    Private m_lDocumentTypeId As Integer
    Private m_lFileNumber As Integer
    Private m_iIsEditableAfterMerging As Integer
    Private m_bAddTOC As Boolean
    Private m_sClient As String = ""
    Private m_sServer As String = ""
    Private m_sResolvedDocumentName As String = ""
    Private m_oWord As Object
    Private m_lReturn As Integer
    Private m_vRiskArray(,) As Object
    Private m_vMailshotArray(,) As Object
    Private m_lMailshotDocumentTemplateId As Integer
    Private m_lMailshotDocumentTypeId As Integer
    Private m_proProgress As Object
    Private m_iSplitDocFileNo As Integer = 1
    Private m_sResolvedDocList As List(Of String)
    Private m_sResolvedDocCodeList As List(Of String)
    Private m_sUnderWritingFlag As String = ""

    Private m_sWordVersion As String = ""

    'RWH(19/10/2000) Merge Field markers (These are inserted in Word
    'as "<@" and "@>" but when viewed as flat text appear as
    ' "&lt;@" and "@&gt;" respectively).
    Private m_sFieldStartMarker As String = ""
    Private m_sFieldEndMarker As String = ""
    Private m_sHyperLinkFieldStartMarker As String = ""
    Private m_sHyperLinkFieldEndMarker As String = ""
    Private m_iFieldMarkerLength As Integer
    Private m_iHyperLinkFieldMarkerLength As Integer
    Private m_sDocFileExtension As String = ""

    Private m_vTotals() As Object

    Private m_sZIP_DIRECTORY As String = ""

    Private Err_No As Integer
    Private Err_Line As Integer
    Private Err_Col As Integer
    Private Err_Description As String = ""
    Private Err_Text As String = ""

    Private m_bSpoolAsHTML As Boolean
    Private m_bSpoolAsTXT As Boolean
    Private m_bSpoolAsPDF As Boolean
    Private m_sDocumentTextContent As String
    Private m_bPrintDocument As Boolean
    Private m_lRiskMode As Integer
    Private m_sOldDocumentRef As String = ""
    Private m_lOldPartyCnt As Integer
    Private m_vPolicySharesArray(,) As Object
    'Private m_colQuestions As Collection
    Private m_colQuestions As New List(Of Object)
    'Private m_colAnswers As Collection
    Private m_colAnswers As New Dictionary(Of Object, Object)
    Private m_sFileCopyMsg As String = ""
    Private m_lWordHwnd As Integer 'PN15072
    Private m_lCaseID As Integer
    Private m_sClauses() As String
    '*************************************
    ' ME : 29-11-2002 : 202
    ' Holds an array: stored procs name, value, datatype
    ' This array passes additional parameters to
    ' the stored procedures referenced by fields
    ' in wp_fields
    Private m_vFieldParams As Object
    ' SET 04/03/2004
    Private m_bRunningOnServer As Boolean
    ' UniqueId
    Private m_lUniqueId As Integer
    'MKW 1.8.5 to 1.8.6 catchup PN7287
    Private m_bUniqueClientDirNeedsDeleting As Boolean
    Private m_sMailshotFilenameForSpooling As String = ""

    Private m_oObjectManager As Object ' bObjectManager.ObjectManager 'jmf 29/8/2003 bObjectManager on server and client
    Private m_oBusiness As bSIRFieldManager.Business
    Private m_oZipper As bPMZipper.Business
    Private m_oDatabase As Object
    Private m_lLineCount As Integer
    Private m_lFileCount As Integer
    Dim m_strCurrencyISOCode As String = ""
    Dim m_strCurrencyMajorPart As String = ""
    Dim m_strCurrencyMinorPart As String = ""
    Private m_oScriptControl As MSScriptControl.ScriptControl
    Private m_oDocTemplate As bSIRDocTemplate.Business

    Private m_bCalledFromSwift As Boolean ' RAM20050201 - Added for Swift Support
    Private m_sParameterXML As String = ""
    Private m_oXMLDOM As XmlDocument

    Private m_lMainFileNumber As Integer
    'Plico 21
    Private m_dtDocEffectiveDate As Date
    Private m_bInHeader As Boolean
    Private m_lRiskCnt As Integer
    Private m_lMode As Integer

    Private m_sSubDocHTMLText As String = ""
    Private m_sSubDocStyleText As String = ""
    Private bIsNestedloop As Boolean

    Private Const kXmlImageName As Integer = 0
    Private Const kXmlImageTag As Integer = 1
    Private Const kXmlImageUsed As Integer = 2

    Private m_bNewDocument As Boolean
    Private m_bDisablePDFenhancement As Boolean
    Dim dtDocument As DataTable
    Dim m_bArchiveAsXML As Boolean
    Dim m_bArchiveAsText As Boolean
    Private m_IsCalledFromBatchProcessing As Boolean

    Private m_sCCMDocumentName As String
    Private m_dtDocument As DataTable
    Private m_dtStdWording As DataTable

    Private Const kxmlns As String = "urn:backbone:Data_Backbone"
    Private Const kXMLHeader As String = "<PureDataBackbone xmlns=""urn:backbone:Data_Backbone"" xmlns:p1=""urn:itp:backbone"" p1:databackbone-revision=""databackbone-revision1"" p1:version=""version1"" p1:format=""1.0""></PureDataBackbone>"
    ''' <summary>
    ''' it is used to take the decision wheather it is being called from the Batch processing or not.
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

    Private m_sCCMDocProduction As String = String.Empty
    Private m_oFilePathArray As New ArrayList
    Private m_sDocTemplatePath As String = String.Empty
    Private m_aoDocCurrency(,) As Object
    Private m_oListClauses As List(Of String)
    Private m_oDocumentTemplateInfo As New List(Of DocumentTemplateInfo)
    Private m_oGetKeys As New Dictionary(Of String, Integer)
    Private isKCMApplicableForSelectedDocument As String = String.Empty


    Public Property RiskCnt() As Integer
        Get
            Return m_lRiskCnt
        End Get
        Set(ByVal Value As Integer)
            m_lRiskCnt = Value
        End Set
    End Property


    Public ReadOnly Property MailshotFilenameForSpooling() As String
        Get
            Return m_sMailshotFilenameForSpooling
        End Get
    End Property
    Public WriteOnly Property UniqueId() As Integer
        Set(ByVal Value As Integer)
            m_lUniqueId = Value
        End Set
    End Property
    Public WriteOnly Property FieldParameters() As Object
        Set(ByVal Value As Object)


            m_vFieldParams = Value
        End Set
    End Property
    Public WriteOnly Property PrintDocument() As Boolean
        Set(ByVal Value As Boolean)
            m_bPrintDocument = Value
        End Set
    End Property
    Public WriteOnly Property SpoolAsHTML() As Boolean
        Set(ByVal Value As Boolean)
            m_bSpoolAsHTML = Value
        End Set
    End Property
    Public WriteOnly Property SpoolAsTXT() As Boolean
        Set(ByVal Value As Boolean)
            m_bSpoolAsTXT = Value
        End Set
    End Property
    Public WriteOnly Property SpoolAsPDF() As Boolean
        Set(ByVal Value As Boolean)
            m_bSpoolAsPDF = Value
        End Set
    End Property
    Public ReadOnly Property Server() As String
        Get
            Return m_sServer
        End Get
    End Property
    Public Property Client() As String
        Get
            Return m_sClient
        End Get
        Set(ByVal Value As String)
            m_sClient = Value
        End Set
    End Property
    Public ReadOnly Property ResolvedDocumentName() As String
        Get
            Return m_sResolvedDocumentName
        End Get
    End Property
    Public ReadOnly Property ResolvedDocumentList() As List(Of String)
        Get
            Return m_sResolvedDocList
        End Get
    End Property
    Public ReadOnly Property ResolvedDocumentCodeList() As List(Of String)
        Get
            Return m_sResolvedDocCodeList
        End Get
    End Property
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
    Public Property DocumentTemplateId() As Integer
        Get
            Return m_lDocumentTemplateId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTemplateId = Value
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
    Public Property FileNumber() As Integer
        Get
            Return m_lFileNumber
        End Get
        Set(ByVal Value As Integer)
            m_lFileNumber = Value
        End Set
    End Property
    Public Property RiskArray() As Object
        Get
            Return (m_vRiskArray).Clone
        End Get
        Set(ByVal Value As Object)
            m_vRiskArray = Value
        End Set
    End Property
    Public Property AddTOC() As Boolean
        Get
            Return m_bAddTOC
        End Get
        Set(ByVal Value As Boolean)
            m_bAddTOC = Value
        End Set
    End Property

    Public Property MailshotArray() As Object
        Get
            Return m_vMailshotArray.Clone
        End Get
        Set(ByVal Value As Object)
            m_vMailshotArray = Value
        End Set
    End Property

    Public Property MailshotDocumentTemplateId() As Integer
        Get
            Return m_lMailshotDocumentTemplateId
        End Get
        Set(ByVal Value As Integer)
            m_lMailshotDocumentTemplateId = Value
        End Set
    End Property

    Public Property MailshotDocumentTypeId() As Integer
        Get
            Return m_lMailshotDocumentTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lMailshotDocumentTypeId = Value
        End Set
    End Property

    Public Property Progress() As Object
        Get
            Return m_proProgress
        End Get
        Set(ByVal Value As Object)
            m_proProgress = Value
        End Set
    End Property

    Public Property IsEditableAfterMerging() As Integer
        Get
            Return m_iIsEditableAfterMerging
        End Get
        Set(ByVal Value As Integer)
            m_iIsEditableAfterMerging = Value
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

    Public Property PolicySharesArray() As Object
        Get
            Return m_vPolicySharesArray.Clone
        End Get
        Set(ByVal Value As Object)
            m_vPolicySharesArray = Value
        End Set
    End Property

    Public Property Clauses() As String()
        Get
            Return m_sClauses
        End Get
        Set(ByVal Value As String())
            m_sClauses = Value
        End Set
    End Property
    Public Property CaseID() As Integer
        Get
            Return m_lCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lCaseID = Value
        End Set
    End Property


    Public WriteOnly Property RiskMode() As String
        Set(ByVal Value As String)
            m_lRiskMode = CInt(Value)
        End Set
    End Property

    Public WriteOnly Property CalledFromSwift() As Boolean
        Set(ByVal Value As Boolean)
            m_bCalledFromSwift = Value

            If m_oBusiness Is Nothing Then
            Else
                m_oBusiness.CalledFromSwift = m_bCalledFromSwift ' RAM20050201 - Added to support Swift
            End If

        End Set
    End Property

    Public WriteOnly Property ParameterXML() As String
        Set(ByVal Value As String)
            m_sParameterXML = Value

            ' Initialise the DOM
            m_lReturn = InitialiseParameterXMLDOM()

        End Set
    End Property

    Public WriteOnly Property Mode() As Integer
        Set(ByVal Value As Integer)
            m_lMode = Value
        End Set
    End Property

    Public Property ArchiveAsXML() As Boolean
        Get
            Return m_bArchiveAsXML
        End Get
        Set(ByVal value As Boolean)
            m_bArchiveAsXML = value
        End Set
    End Property
    Public Property ArchiveAsText() As Boolean
        Get
            Return m_bArchiveAsText
        End Get
        Set(ByVal value As Boolean)
            m_bArchiveAsText = value
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

    ' Public Methods

    '************************************************************************
    'Name: Initialise
    '
    'Description: Initialises the object
    '
    'History:       ??/??/????  ???     Created
    '               06/05/2003  APS     Initialise the questions and answers collections
    '
    '************************************************************************
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const ACCurrencyISOCode As Integer = 2
        Const ACCurrencyMajorPart As Integer = 3 ' Note : We are using the description here. We need a new column Major part as in 1.9.2
        Const ACCurrencyMinorPart As Integer = 4

        Dim sMessage, sTitle As String
        Dim vCurrencyDetailsArray(,) As Object = Nothing
        Dim vValue As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'm_colQuestions = New Collection()
            'm_colAnswers = New Collection()
            m_colQuestions = New List(Of Object)
            m_colAnswers = New Dictionary(Of Object, Object)
            ' Create an instance of the object manager.
            'sj 08/11/2001 - start
            ' m_oObjectManager = New bObjectManager.ObjectManager()
            'sj 08/11/2001 - end
            ' Call the initialise method.
            ' m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to call the initialise method.
            '  result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the object manager to nothing.
            '  m_oObjectManager = Nothing

            ' Log Error.
            ' bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

            ' Return result
            'End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            'With m_oObjectManager
            '    m_iLanguageID = .LanguageID
            '    m_iSourceID = .SourceID
            '    m_sUsername = .UserName

            '    'mrh 18/05/2004 PN8946
            '    m_iUserID = .UserID
            'End With

            bPMDocFunctions.Username = m_sUsername

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
            'DJM 11/08/2003
            m_lRiskMode = ACRiskMode
            'Saj240224
            ' Get an instance of the business object via
            ' the public object manager.
            'Dim temp_m_oBusiness As Object = Nothing
            'm_lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRFieldManager.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            'm_oBusiness = temp_m_oBusiness

            '   oSIRRiskScreen = Nothing
            'result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oBusiness, v_sClassName:="bSIRFieldManager.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            'If result <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Dim r_sMessage As String = "Failed to create an instance of bSIRFieldManager.Business"
            '    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRRiskScreen.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            '    Return result
            'End If

            m_oBusiness = New bSIRFieldManager.Business
            m_lReturn = m_oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                               iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.
                sTitle = ACApp
                sMessage = "Unable to get instance of bSIRFieldManager.Business"

                ' Display message.

                'MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                'Saj240224
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            ' Note : If the m_iCurrencyID is 0, then, business component's
            '           m_iCurrencyID is used to fetch the details
            If m_oBusiness Is Nothing Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="1. m_oBusiness is not instantiated", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            m_oBusiness.CalledFromSwift = m_bCalledFromSwift ' RAM20050201 - Added to support Swift

            m_lReturn = m_oBusiness.GetCurrencyDetails(v_iCurrencyID:=m_iCurrencyID, r_vCurrencyDetails:=vCurrencyDetailsArray)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_strCurrencyISOCode = CStr(vCurrencyDetailsArray(ACCurrencyISOCode, 0)).Trim()

                m_strCurrencyMajorPart = CStr(vCurrencyDetailsArray(ACCurrencyMajorPart, 0)).Trim()

                m_strCurrencyMinorPart = CStr(vCurrencyDetailsArray(ACCurrencyMinorPart, 0)).Trim()
            End If

            'jmf create the instance here rather than in the start
            'Dim temp_m_oDocTemplate As Object = Nothing
            'm_lReturn = m_oObjectManager.GetInstance(temp_m_oDocTemplate, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            'm_oDocTemplate = temp_m_oDocTemplate
            Dim temp_m_oDocTemplate As bSIRDocTemplate.Business
            temp_m_oDocTemplate = New bSIRDocTemplate.Business
            m_lReturn = temp_m_oDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                               iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sTitle = ACApp
                sMessage = "Unable to get instance of bSIRDocTemplate.Business"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="initialise")
                Return result
            End If


            m_lReturn = GetServer()

            m_oZipper = New bPMZipper.Business()

            ' SET 04/03/2004
            m_bRunningOnServer = False

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=1, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTDisablePDFenhancement, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue)

            If Convert.ToString(vValue) = "1" Then
                m_bDisablePDFenhancement = True
            Else
                m_bDisablePDFenhancement = False
            End If

            m_lMainFileNumber = -1
            m_bInHeader = False

            m_sUnderWritingFlag = "U"
            g_sDocPreBodyFragment = ""
            g_sDocEndBodyFragment = ""
            m_dtDocument = Nothing
            m_dtStdWording = Nothing

            'Get the List of All Clauses
            GetListOfAllClauses()

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
    Public Function InitialiseBusiness(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const ACCurrencyISOCode As Integer = 2
        Const ACCurrencyMajorPart As Integer = 3 ' Note : We are using the description here. We need a new column Major part as in 1.9.2
        Const ACCurrencyMinorPart As Integer = 4
        Dim sTitle As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sMessage As String
            Dim vCurrencyDetailsArray(,) As Object = Nothing

            ' Initialisation Code.

            '***************
            ' MEvans : 09-08-2004 : Resilience
            ' save the database away to a constanct

            If Not Information.IsNothing(vDatabase) Then
                m_oDatabase = vDatabase
            End If
            '***************

            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword

            ' Set User ID
            m_iUserID = iUserID

            ' Set Calling Application
            m_sCallingAppName = sCallingAppName

            ' Set Language ID
            m_iLanguageID = iLanguageID

            ' Set Source ID
            m_iSourceID = iSourceID

            ' Set Currency ID
            m_iCurrencyID = iCurrencyID

            ' Set Log Level
            m_iLogLevel = iLogLevel

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            bPMDocFunctions.Username = m_sUsername

            ' Get an instance of the business object via
            ' the public object manager.
            m_oBusiness = New bSIRFieldManager.Business()

            m_lReturn = m_oBusiness.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                sTitle = ACApp
                sMessage = "Unable to get instance of bSIRFieldManager.Business"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Note : If the m_iCurrencyID is 0, then, business component's
            '           m_iCurrencyID is used to fetch the details
            'CQ6053 DEBUG
            If m_oBusiness Is Nothing Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="1. m_oBusiness is not instantiated", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If

            m_oBusiness.CalledFromSwift = m_bCalledFromSwift ' RAM20050201 - Added to support Swift

            ' Get the currency details
            m_lReturn = m_oBusiness.GetCurrencyDetails(v_iCurrencyID:=m_iCurrencyID, r_vCurrencyDetails:=vCurrencyDetailsArray)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_strCurrencyISOCode = CStr(vCurrencyDetailsArray(ACCurrencyISOCode, 0)).Trim()

                m_strCurrencyMajorPart = CStr(vCurrencyDetailsArray(ACCurrencyMajorPart, 0)).Trim()

                m_strCurrencyMinorPart = CStr(vCurrencyDetailsArray(ACCurrencyMinorPart, 0)).Trim()
            End If

            'jmf create the instance here rather than in start
            m_oDocTemplate = New bSIRDocTemplate.Business()
            m_lReturn = m_oDocTemplate.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sTitle = ACApp
                sMessage = "Unable to get instance of bSIRDocTemplate.Business"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetServer()

            ' m_lReturn = GetClientFolder()

            'sj 14/10/2002 - start
            m_oZipper = New bPMZipper.Business()
            'sj 14/10/2002 - end
            ' SET 04/03/2004
            m_bRunningOnServer = True

            m_lMainFileNumber = -1
            m_bInHeader = False

            m_sUnderWritingFlag = "U"
            g_sDocPreBodyFragment = ""
            g_sDocEndBodyFragment = ""

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                m_oZipper = Nothing
                If m_oDocTemplate IsNot Nothing Then
                    m_oDocTemplate.Dispose()
                    m_oDocTemplate = Nothing
                End If
                If m_oObjectManager IsNot Nothing Then
                    m_oObjectManager.Dispose()
                    m_oObjectManager = Nothing
                End If

                m_colQuestions = Nothing
                m_colAnswers = Nothing
                If m_oXMLDOM Is Nothing Then
                Else
                    m_oXMLDOM = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function Start() As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            nResult = m_oDocTemplate.GetFurtherDetails(v_lDocTemplateId:=m_lDocumentTemplateId,
                                                        r_bArchiveWithNoPrint:=Nothing,
                                                        r_bEmailAsBody:=Nothing,
                                                        r_bSpoolDocument:=Nothing,
                                                        r_bArchiveAsText:=m_bArchiveAsText,
                                                        r_sDocumentDescription:=Nothing,
                                                        r_bArchiveAsXML:=m_bArchiveAsXML,
                                                        r_sCCMDocumentName:=m_sCCMDocumentName)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get system option CCMDocProduction
            nResult = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                             v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=GeneralConst.kSystemOptionDocumentProductionSystem,
                                             r_sOptionValue:=m_sCCMDocProduction)

            If (String.IsNullOrEmpty(m_sCCMDocProduction)) Then
                m_sCCMDocProduction = "0"
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Is KCM applicable only for selected documents
            If Not (String.IsNullOrEmpty(m_sCCMDocProduction) OrElse m_sCCMDocProduction = "0") Then
                nResult = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                             v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=GeneralConst.KSystemOptionKCMForSelectedTemplate,
                                             r_sOptionValue:=isKCMApplicableForSelectedDocument)

                If String.IsNullOrEmpty(isKCMApplicableForSelectedDocument) Then
                    isKCMApplicableForSelectedDocument = "0"
                End If
            Else
                isKCMApplicableForSelectedDocument = "0"
            End If

            If m_sCCMDocProduction = "1" AndAlso (isKCMApplicableForSelectedDocument = "0" OrElse (Not String.IsNullOrEmpty(m_sCCMDocumentName))) Then
                If Not String.IsNullOrEmpty(m_sCCMDocumentName) Then
                    ''Get system option CCM Clause Location
                    nResult = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                     v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=GeneralConst.kSystemOptionCCMClauseLocation,
                                                     r_sOptionValue:=m_sDocTemplatePath)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If String.IsNullOrEmpty(m_sDocTemplatePath) Then ''pick GIS path if not entered in system options
                        ''Get Client location for Document Generation
                        nResult = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusSolutions,
                                                                 v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocClient", r_sSettingValue:=m_sDocTemplatePath)
                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Client from Registry.", vApp:=ACApp, vClass:=ACClass,
                                               vMethod:="GetPMRegSetting", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_sDocTemplatePath = m_sDocTemplatePath & "\CCM\"
                    Else
                        m_sDocTemplatePath = m_sDocTemplatePath.Replace("/", "\")
                        If Not m_sDocTemplatePath.EndsWith("\") Then
                            m_sDocTemplatePath = m_sDocTemplatePath & "\"
                        End If
                    End If

                    nResult = GenerateDocumentCCM(m_sCCMDocumentName)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Else
                nResult = GenerateDocumentPure()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try

        Return nResult
    End Function

    ' **********************************************************
    ' CTAF 03072001
    '
    ' Function to check if the word object is valid (ie, has it Quit or not)
    '
    ' **********************************************************
    Private Function IsWordValid() As Boolean

        Dim result As Boolean = False
        Dim sTemp As String = ""

        Try

            result = True

            ' Try and get the name of the object

            sTemp = m_oWord.name

            Return result

        Catch



            Return False
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: CopyMailshotServerToClient
    '
    ' Description: copies the mailshot file from the server to the client
    ' Edit History  :
    ' RAM20040301   : Removed unwanted Dir Commands as it locks the directory
    '                 Use bPMDocFunctions Unzip function
    '                 Added code for error handling
    '                 Use m_sClient as the work dir rather than the m_sZIP_DIRECTORY
    '                 Code added for clean up of word object
    ' ***************************************************************** '
    Private Function CopyMailshotServerToClient(ByRef lCount As Integer) As Integer

        Dim result As Integer = 0
        Dim sServer, sClient, sTemp As String
        Dim lTempFileNo As Integer
        Dim oTemplate As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sServer = m_sServer & "\Type " & CStr(m_lMailshotDocumentTypeId) & "\Doc " & CStr(m_lMailshotDocumentTemplateId) & ".zip"

            lTempFileNo = m_lMailshotDocumentTemplateId

            'We're using the same file all the time, so we copy it once, unzip it,
            'then use the local copy

            'RWH(08/09/2000) Changed count to start at 1.

            If lCount = 1 Then

                ' RAM20040301 : The following Function will clear the work directory & Zip Temp Dir
                m_lReturn = EnsureClientDirectoryClear()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the contents of the client work directory", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                sClient = m_sZIP_DIRECTORY & "\Doc " & CStr(m_lMailshotDocumentTemplateId) & ".zip"

                m_sFileCopyMsg = ""
                m_lReturn = bPMDocFunctions.CopyFile(sServer, sClient, True, False, m_sFileCopyMsg, v_bCalledFromBatchProcessing:=IsCalledFromBatchProcessing)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyPolicySharesServerToClient(m_lReturn)
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy template from Server to Client." & Chr(13) & Chr(10) &
                                       "sServer       : " & sServer & Chr(13) & Chr(10) &
                                       "sClient       : " & sClient & Chr(13) & Chr(10) &
                                       "Error Details : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=0, vErrDesc:="Failed to copy the template.")
                    Return result
                End If

                ' RAM20040301 : Use the bPMDocFunctions UnZip Function.
                m_lReturn = UnZip(sClient, m_sClient, True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Unzip the Template." & Chr(13) & Chr(10) &
                                       "Template        : " & sClient & Chr(13) & Chr(10) &
                                       "UnZip Directory : " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=0, vErrDesc:="Failed to UnZip the template.")
                    Return result
                End If

                If CheckFileTypeIsDoc() = gPMConstants.PMEReturnCode.PMTrue Then

                    oTemplate = m_oWord.Documents.Add

                    CheckFileHasCorrectName(m_sClient, lTempFileNo)

                    m_lReturn = ConvertDocument(oTemplate, lTempFileNo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        oTemplate = Nothing
                        ' SET 18/10/2004 ISS13245
                        m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                        Return result
                    End If

                    m_lReturn = SaveDocumentAsHTML(oTemplate, lTempFileNo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        oTemplate = Nothing
                        ' SET 18/10/2004 ISS13245
                        m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                        Return result
                    End If

                    ' Clear memory
                    If oTemplate Is Nothing Then
                    Else
                        oTemplate = Nothing
                    End If
                    ' SET 18/10/2004 ISS13245
                    m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                    'DJM 02/09/2002 : Remove old word document.
                    sTemp = m_sClient & "\" & "Doc " & CStr(lTempFileNo) & ".doc"
                    m_lReturn = DOCGeneralFunc.DeleteFile(sTemp)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the file [" & sTemp & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                ElseIf (CheckFileTypeIsHtml() = gPMConstants.PMEReturnCode.PMTrue) And (m_sDocFileExtension.ToUpper() = "XML") Then
                    If CDbl(m_sWordVersion) > 11 And Not m_bDisablePDFenhancement Then

                        'Word 2003 or Higher So WordML available.

                        Dim oWordDocument As New Document(m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".htm")
                        oWordDocument.Save(m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".xml", SaveFormat.WordML)

                        If Directory.Exists(m_sClient & "\Doc " & CStr(m_lDocumentTemplateId) & "_files") Then
                            Directory.Delete(m_sClient & "\Doc " & CStr(m_lDocumentTemplateId) & "_files")
                        End If

                        'DJM 02/09/2002 : Remove old word document.
                        sTemp = m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".htm"

                        ' RAM20040209 : Removed unwanted Dir Command, PN Issue 10231
                        m_lReturn = DOCGeneralFunc.DeleteFile(sTemp)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete File [" & sTemp & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return result
                        End If

                    End If
                End If

                If m_sDocFileExtension.ToUpper() <> "XML" Then

                    m_lReturn = UpdateTemplateNumberAndDependencies(m_sClient, m_lMailshotDocumentTemplateId, 0)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMError
                        ' Log Error.
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to UpdateTemplateNumberAndDependencies." & Chr(13) & Chr(10) &
                                           "m_sClient : " & m_sClient & Chr(13) & Chr(10) &
                                           "lOldID : " & CStr(m_lDocumentTemplateId) & Chr(13) & Chr(10) &
                                           "lNewID : 0", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                Else
                    'Rename File "Doc " & m_lMailshotDocumentTemplateId & "." & m_sDocFileExtension to "Doc " & (lTemp + 1) & "." & m_sDocFileExtension
                    Directory.CreateDirectory(m_sClient & "\temp\")
                    System.IO.File.Move(m_sClient & "\Doc " & CStr(m_lMailshotDocumentTemplateId) & "." & m_sDocFileExtension, m_sClient & "\temp\Doc 0." & m_sDocFileExtension)
                End If

                If m_sDocFileExtension.ToUpper() <> "XML" Then
                    m_lReturn = CopyDocAndDependencies(m_sClient, m_sClient & "\Temp")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMError
                        ' Log Error.
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to CopyDocAndDependencies" & Chr(13) & Chr(10) &
                                           "From : " & m_sClient & Chr(13) & Chr(10) &
                                           "To   : " & m_sClient & "\Temp", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
            End If

            'RWH(05/09/2000) RSAIB Process 108. As docs now stored as HTML, we need to take account
            'of document dependencies.
            ' we don't need to do this for the first time, since we have the document already at the
            ' client location
            If lCount > 1 Then
                If m_sDocFileExtension.ToUpper() <> "XML" Then
                    m_lReturn = CopyDocAndDependencies(m_sClient & "\Temp", m_sClient)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMError
                        ' Log Error.
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to CopyDocAndDependencies" & Chr(13) & Chr(10) &
                                           "From : " & m_sClient & "\Temp" & Chr(13) & Chr(10) &
                                           "To   : " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
            End If

            If m_sDocFileExtension.ToUpper() <> "XML" Then
                m_lReturn = UpdateTemplateNumberAndDependencies(m_sClient, 0, lCount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError
                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to UpdateTemplateNumberAndDependencies." & Chr(13) & Chr(10) &
                                       "m_sClient : " & m_sClient & Chr(13) & Chr(10) &
                                       "lOldID : 0" & Chr(13) & Chr(10) &
                                       "lNewID :" & CStr(lCount), vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            Else
                System.IO.File.Copy(m_sClient & "\temp\Doc 0." & m_sDocFileExtension, m_sClient & "\Doc " & CStr(lCount) & "." & m_sDocFileExtension)
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Clear memory
            If oTemplate Is Nothing = False Then
                oTemplate = Nothing
            End If
            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy mailshot file from server to client", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyMailshotServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopyRiskServerToClient
    '
    ' Description: copies the risk file from the server to the client
    '
    ' Changes: (05/09/2000) Changed to use absolute temporary zip
    '           directory and deal with new HTML format and associated
    '           dependencies.
    ' RAM20040301   : Removed unwanted dir Commands as it locks the directory
    '                 Use bPMDocFunctions Unzip function
    '                 Added code for error handling
    '                 Use m_sClient as the work dir rather than the m_sZIP_DIRECTORY
    '                 Code added for clean up of word object
    ' ***************************************************************** '
    Private Function CopyRiskServerToClient(ByRef lType As Integer, ByRef lTemplate As Integer, ByRef lFileNumber As Integer, ByRef lCount As Integer) As Integer

        Dim result As Integer = 0
        Dim sServer, sClient, sTemp As String
        Dim lTemp, lTemp2, lTempFileNo As Integer
        Dim oTemplate As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lFileNumber <> 0 Then
                lTemp = lFileNumber \ 500

                lTemp2 = lFileNumber - (lTemp * 500)
                'DJM 11/08/2003
                'DN 11/12/01 - Remove company level for text files path on the server.
                sServer = m_sServer & "\Policy Text Files"
                If m_lRiskMode = ACRiskMode Then
                    sServer = sServer & "\Slot 1"
                Else
                    sServer = sServer & "\Slot 11"
                End If
                sServer = sServer & "\" & StringsHelper.Format(lTemp, "000") & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"
                'DN 29/01/02 - Use correct file number if greater than 500
                lTempFileNo = lTemp2

            Else
                sServer = m_sServer & "\Type " & ToSafeString(lType) & "\Doc " & ToSafeString(lTemplate) & ".zip"

                lTempFileNo = lTemplate

            End If

            'RWH(11/09/2000) Changed count to start at 1.
            If lCount = 1 Then
                'Make sure the directory's there

                m_lReturn = CreateFolderTree(m_sClient, True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError
                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Create the Directory. (" & m_sClient & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'Make sure the directory's there
                m_lReturn = SetZipDirectory()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            sClient = m_sZIP_DIRECTORY & "\Doc " & ToSafeString(lTempFileNo) & ".zip"

            'Make sure the file's not there
            sTemp = sClient.Substring(0, sClient.Length - 3) & "." & m_sDocFileExtension

            m_lReturn = DOCGeneralFunc.DeleteFile(sTemp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the file. (" & sTemp & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_sFileCopyMsg = ""
            m_lReturn = bPMDocFunctions.CopyFile(sServer, sClient, True, False, m_sFileCopyMsg, v_bCalledFromBatchProcessing:=IsCalledFromBatchProcessing)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyPolicySharesServerToClient(m_lReturn)
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy template from Server to Client." & Chr(13) & Chr(10) &
                                   "sServer       : " & sServer & Chr(13) & Chr(10) &
                                   "sClient       : " & sClient & Chr(13) & Chr(10) &
                                   "Error Details : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskServerToClient", vErrNo:=0, vErrDesc:="Failed to Copy the template.")
                Return result
            End If

            m_lReturn = UnZip(sClient, m_sClient, True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Unzip the Template." & Chr(13) & Chr(10) &
                                   "Template        : " & sClient & Chr(13) & Chr(10) &
                                   "UnZip Directory : " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskServerToClient", vErrNo:=0, vErrDesc:="Failed to UnZip the template.")
                Return result
            End If

            'DJM 02/09/2002 : Convert files in the zip directory not in the client directory
            'DN 12/07/02 - Convert document if not HTML

            If CheckFileTypeIsDoc(m_sDocFileExtension.ToUpper() = "XML") = gPMConstants.PMEReturnCode.PMTrue Then


                oTemplate = m_oWord.Documents.Add

                m_lReturn = CheckFileHasCorrectName(m_sClient, lTempFileNo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    oTemplate = Nothing
                    ' SET 18/10/2004 ISS13245
                    m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                    Return result
                End If

                m_lReturn = ConvertDocument(oTemplate, lTempFileNo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    oTemplate = Nothing
                    ' SET 18/10/2004 ISS13245
                    m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                    Return result
                End If

                m_lReturn = SaveDocumentAsHTML(oTemplate, lTempFileNo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    oTemplate = Nothing
                    ' SET 18/10/2004 ISS13245
                    m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                    Return result
                End If

                If oTemplate Is Nothing Then
                Else
                    oTemplate = Nothing
                End If
                ' SET 18/10/2004 ISS13245
                m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                'DJM 02/09/2002 : Remove old word document.
                sTemp = m_sClient & "\" & "Doc " & ToSafeString(lTempFileNo) & ".doc"

                ' RAM20040301 : Remvoed unwanted Dir Commands as it locks the directory
                m_lReturn = DOCGeneralFunc.DeleteFile(sTemp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError
                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the file. (" & sTemp & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            ElseIf (CheckFileTypeIsHtml() = gPMConstants.PMEReturnCode.PMTrue) And (m_sDocFileExtension.ToUpper() = "XML") Then
                If ToSafeDouble(m_sWordVersion) > 11 And Not m_bDisablePDFenhancement Then

                    'Word 2003 or Higher So WordML available.

                    Dim oWordDocument As New Document(m_sClient & "\" & "Doc " & ToSafeString(lTempFileNo) & ".htm")
                    oWordDocument.Save(m_sClient & "\" & "Doc " & ToSafeString(lTempFileNo) & ".xml", SaveFormat.WordML)


                    If Directory.Exists(m_sClient & "\Doc " & ToSafeString(lTempFileNo) & "_files") Then
                        Directory.Delete(m_sClient & "\Doc " & ToSafeString(lTempFileNo) & "_files")
                    End If

                    'DJM 02/09/2002 : Remove old word document.
                    sTemp = m_sClient & "\" & "Doc " & ToSafeString(lTempFileNo) & ".htm"

                    ' RAM20040209 : Removed unwanted Dir Command, PN Issue 10231
                    m_lReturn = DOCGeneralFunc.DeleteFile(sTemp)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete File [" & sTemp & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRisksServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
            End If

            If m_sDocFileExtension.ToUpper() <> "XML" Then
                m_lReturn = UpdateTemplateNumberAndDependencies(m_sClient, lTempFileNo, lCount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError
                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to UpdateTemplateNumberAndDependencies." & Chr(13) & Chr(10) &
                                       "m_sClient : " & m_sClient & Chr(13) & Chr(10) &
                                       "lOldID : " & ToSafeString(lTempFileNo) & Chr(13) & Chr(10) &
                                       "lNewID : " & ToSafeString(lCount), vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            Else
                'Rename File
                System.IO.File.Move(m_sClient & "\Doc " & ToSafeString(lTempFileNo) & "." & m_sDocFileExtension, m_sClient & "\Doc " & ToSafeString(lCount) & "." & m_sDocFileExtension)
            End If

            ' RAM20040301 : Commented the following line, since, we dont' need this.
            ' We don't need this, since the Unzip function, already unzip to the target directory
            'm_lReturn = MoveFilesFromZipTemp(m_sClient)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            If oTemplate Is Nothing = False Then
                oTemplate = Nothing
            End If
            ' SET 18/10/2004 ISS13245
            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy risk file from server to client", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyPolicySharesServerToClient
    '
    ' Description: copies the policy shares file from the server to the client
    ' Edit History  :
    ' RAM20040301   : Removed unwanted Dir Commands, since it locks the directory
    '                 Use bPMDocFunctions Unzip function
    '                 Added code for error handling
    '                 Use m_sClient as the work dir rather than the m_sZIP_DIRECTORY
    '                 Code added for clean up of word object
    ' ***************************************************************** '
    Private Function CopyPolicySharesServerToClient(ByRef lCount As Integer) As Integer

        Dim result As Integer = 0
        Dim sServer, sClient, sTemp As String

        Dim oTemplate As Object = Nothing
        Dim sErrorMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sServer = m_sServer & "\Type " & ToSafeString(m_lDocumentTypeId) & "\Doc " & ToSafeString(m_lDocumentTemplateId) & ".zip"

            'We're using the same file all the time, so we copy it once, unzip it,
            'then use the local copy

            'RWH(08/09/2000) Changed count to start at 1.

            If lCount = 1 Then

                m_lReturn = EnsureClientDirectoryClear()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the contents of the client work directory", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                sClient = m_sZIP_DIRECTORY & "\Doc " & ToSafeString(m_lDocumentTemplateId) & ".zip"

                m_sFileCopyMsg = ""
                m_lReturn = bPMDocFunctions.CopyFile(sServer, sClient, True, False, sErrorMessage, v_bCalledFromBatchProcessing:=IsCalledFromBatchProcessing)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyPolicySharesServerToClient(m_lReturn)
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file from Server to Client." & Chr(13) & Chr(10) &
                                       "sServer       : " & sServer & Chr(13) & Chr(10) &
                                       "sClient       : " & sClient & Chr(13) & Chr(10) &
                                       "Error Details : " & sErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=0, vErrDesc:="Failed to UnZip the template.")
                    Return result
                End If

                m_lReturn = UnZip(sClient, m_sClient, True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyPolicySharesServerToClient(m_lReturn)
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Unzip the Template." & Chr(13) & Chr(10) &
                                       "Template        : " & sClient & Chr(13) & Chr(10) &
                                       "UnZip Directory : " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=0, vErrDesc:="Failed to UnZip the template.")
                    Return result
                End If

                'DJM 14/01/2003 : Convert document if not HTML
                If CheckFileTypeIsDoc(m_sDocFileExtension.ToUpper() = "XML") = gPMConstants.PMEReturnCode.PMTrue Then

                    oTemplate = m_oWord.Documents.Add

                    m_lReturn = CheckFileHasCorrectName(m_sClient, m_lDocumentTemplateId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        oTemplate = Nothing
                        ' SET 18/10/2004 ISS13245
                        m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                        Return result
                    End If

                    m_lReturn = ConvertDocument(oTemplate, m_lDocumentTemplateId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        oTemplate = Nothing
                        ' SET 18/10/2004 ISS13245
                        m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                        Return result
                    End If

                    m_lReturn = SaveDocumentAsHTML(oTemplate, m_lDocumentTemplateId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        oTemplate = Nothing
                        ' SET 18/10/2004 ISS13245
                        m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                        Return result
                    End If

                    ' Clear memory
                    If oTemplate Is Nothing Then
                    Else
                        oTemplate = Nothing
                    End If
                    ' SET 18/10/2004 ISS13245
                    m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                    'DJM 02/09/2002 : Remove old word document.
                    sTemp = m_sClient & "\" & "Doc " & ToSafeString(m_lDocumentTemplateId) & ".doc"
                    m_lReturn = DOCGeneralFunc.DeleteFile(sTemp)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMError
                        ' Log Error.
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the file. (" & sTemp & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                ElseIf (CheckFileTypeIsHtml() = gPMConstants.PMEReturnCode.PMTrue) And (m_sDocFileExtension.ToUpper() = "XML") Then
                    If ToSafeDouble(m_sWordVersion) > 11 And Not m_bDisablePDFenhancement Then

                        'Word 2003 or Higher So WordML available.

                        Dim oWordDocument As New Document(m_sClient & "\" & "Doc " & ToSafeString(m_lDocumentTemplateId) & ".htm")
                        oWordDocument.Save(m_sClient & "\" & "Doc " & ToSafeString(m_lDocumentTemplateId) & ".xml", SaveFormat.WordML)


                        If Directory.Exists(m_sClient & "\Doc " & ToSafeString(m_lDocumentTemplateId) & "_files") Then
                            Directory.Delete(m_sClient & "\Doc " & ToSafeString(m_lDocumentTemplateId) & "_files")
                        End If

                        'DJM 02/09/2002 : Remove old word document.
                        sTemp = m_sClient & "\" & "Doc " & ToSafeString(m_lDocumentTemplateId) & ".htm"

                        ' RAM20040209 : Removed unwanted Dir Command, PN Issue 10231
                        m_lReturn = DOCGeneralFunc.DeleteFile(sTemp)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete File [" & sTemp & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return result
                        End If

                    End If
                End If

                If m_sDocFileExtension.ToUpper() <> "XML" Then
                    m_lReturn = UpdateTemplateNumberAndDependencies(m_sClient, m_lDocumentTemplateId, 0)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMError
                        ' Log Error.
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to UpdateTemplateNumberAndDependencies." & Chr(13) & Chr(10) &
                                           "m_sClient : " & m_sClient & Chr(13) & Chr(10) &
                                           "lOldID : " & ToSafeString(m_lDocumentTemplateId) & Chr(13) & Chr(10) &
                                           "lNewID : 0", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                Else
                    Directory.CreateDirectory(m_sClient & "\temp\")
                    System.IO.File.Move(m_sClient & "\Doc " & ToSafeString(m_lDocumentTemplateId) & "." & m_sDocFileExtension, m_sClient & "\temp\Doc 0." & m_sDocFileExtension)
                End If

                If m_sDocFileExtension.ToUpper() <> "XML" Then
                    m_lReturn = CopyDocAndDependencies(m_sClient, m_sClient & "\Temp")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMError
                        ' Log Error.
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to CopyDocAndDependencies" & Chr(13) & Chr(10) &
                                           "From : " & m_sClient & Chr(13) & Chr(10) &
                                           "To   : " & m_sClient & "\Temp", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
            End If

            ' we don't need to do this for the first time, since we have the document already at the
            ' client location
            If lCount > 1 Then
                If m_sDocFileExtension.ToUpper() <> "XML" Then
                    m_lReturn = CopyDocAndDependencies(m_sClient & "\Temp", m_sClient)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMError
                        ' Log Error.
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to CopyDocAndDependencies" & Chr(13) & Chr(10) &
                                           "From : " & m_sClient & "\Temp" & Chr(13) & Chr(10) &
                                           "To   : " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
            End If

            If m_sDocFileExtension.ToUpper() <> "XML" Then
                m_lReturn = UpdateTemplateNumberAndDependencies(m_sClient, 0, lCount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError
                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to UpdateTemplateNumberAndDependencies." & Chr(13) & Chr(10) &
                                       "m_sClient : " & m_sClient & Chr(13) & Chr(10) &
                                       "lOldID : 0" & Chr(13) & Chr(10) &
                                       "lNewID :" & ToSafeString(lCount), vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            Else
                System.IO.File.Copy(m_sClient & "\temp\Doc 0." & m_sDocFileExtension, m_sClient & "\Doc " & ToSafeString(lCount) & "." & m_sDocFileExtension)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Clear memory
            If oTemplate Is Nothing = False Then
                oTemplate = Nothing
            End If
            ' SET 18/10/2004 ISS13245
            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy policy shares file from server to client", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicySharesServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ********************************************************************** '
    ' Name: CopyServerToClient
    '
    ' Description: copies the template from the server to the client
    '
    ' Changes:     RWH(18/08/2000) - parameterised to enable use in Clauses.
    ' ********************************************************************** '
    Private Function CopyServerToClient(ByVal lTypeId As Integer, ByVal lTemplateId As Integer) As Integer

        Dim result As Integer = 0
        Dim sServer, sClient, sTemp As String



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lFileNumber <> 0 Then
            Return result
        End If

        sServer = m_sServer & "\Type " & CStr(lTypeId) & "\Doc " & CStr(lTemplateId) & ".zip"

        If System.IO.File.Exists(sServer) = False And lTypeId = lLETTER_TYPE_ID Then


            'If letter not found then search in subdoc folder
            If Directory.GetFiles(m_sServer & "\Type " & CStr(lSUBDOC_TYPE_ID) & "\Doc " & CStr(lTemplateId) & ".zip", FileAttribute.Normal).ToString <> "" Then
                sServer = m_sServer & "\Type " & CStr(lSUBDOC_TYPE_ID) & "\Doc " & CStr(lTemplateId) & ".zip"
            End If
        End If

        'RWH(04/09/2000) RSAIB Process 108.
        m_lReturn = SetZipDirectory()

        sClient = m_sZIP_DIRECTORY & "\Doc " & CStr(lTemplateId) & ".zip"

        ' SET 12/10/2004 ISS15027

        m_sFileCopyMsg = ""
        m_lReturn = bPMDocFunctions.CopyFile(sServer, sClient, True, False, m_sFileCopyMsg)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file from Server to Client." & Chr(13) & Chr(10) &
                               "sServer       : " & sServer & Chr(13) & Chr(10) &
                               "sClient       : " & sClient & Chr(13) & Chr(10) &
                                   "Error Details : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=0, vErrDesc:="Failed to UnZip the template.")
            Return result
        End If

        'Make sure the target file's not there
        'RWH(31/07/2000) Changed to .htm.
        sTemp = m_sClient & "\Doc " & CStr(lTemplateId) & "." & m_sDocFileExtension
        m_lReturn = DOCGeneralFunc.DeleteFile(sTemp)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the file. (" & sTemp & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' RAM20040301 : 1. Use the DOCGeneralFunc UnZip Function.
        '               2. Also use the m_sClient Work Directory to unzip the files directly
        '               3. Delete the original zip file
        m_lReturn = UnZip(sClient, m_sClient, True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Unzip the Template." & Chr(13) & Chr(10) &
                               "Template        : " & sClient & Chr(13) & Chr(10) &
                                   "Client Work Directory : " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=0, vErrDesc:="Failed to UnZip the template.")
            Return result
        End If

        Return result

    End Function


    '***********************************************************************
    ' Name: ResolveReadFullLine
    '
    ' Description: Read a full line of the file, i.e. everything between
    '              a <p> and </p>.
    '
    ' History:  PW151003 - created
    '           RVH 08/11/2004  (Performance) Pass file class instead of num
    '***********************************************************************
    Private Function ResolveReadFullLine(ByRef cInputFile As FileClass, ByRef r_sCurrentLine As String) As Integer

        Dim result As Integer = 0
        Dim iPos As Integer
        Dim sNextLine As String = ""
        Dim lFieldStart, iCurrentNestingLevel, lLoop As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' PW170903 - CQ1979 - need to ensure <p> and </p> are in the same line
        ' RVH 08/11/2004: (Performance) Use file class
        m_lReturn = cInputFile.GetNextLine(sNextLine)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            Return result
        End If
        r_sCurrentLine = sNextLine

        m_lLineCount += 1

        If m_sDocFileExtension.ToUpper() = "XML" Then
            m_lReturn = FixBrokenMarkers(cInputFile, r_sCurrentLine)
        End If

        'Check for Merge field present on this line.
        lFieldStart = InStr(r_sCurrentLine, m_sFieldStartMarker)
        If lFieldStart <> 0 Then
            ' Reset variables
            iCurrentNestingLevel = 0
            lLoop = lFieldStart

            ' Loop until a full line is found
            Do While True

                iCurrentNestingLevel = 0
                iPos = 1
                Do While InStr(iPos, r_sCurrentLine, m_sFieldStartMarker) > 0
                    iPos = InStr(iPos, r_sCurrentLine, m_sFieldStartMarker) + 1
                    iCurrentNestingLevel = iCurrentNestingLevel + 1
                Loop
                iPos = 1
                Do While InStr(iPos, r_sCurrentLine, m_sFieldEndMarker) > 0
                    iPos = InStr(iPos, r_sCurrentLine, m_sFieldEndMarker) + 1
                    iCurrentNestingLevel = iCurrentNestingLevel - 1
                Loop

                ' If level is not back to 0, we don't have a full statement,
                ' so get the next line and carry on going
                If iCurrentNestingLevel > 0 Then
                    ' RVH 04/11/2004: (Performance) Get next line from file class
                    If Not cInputFile.EOF Then
                        m_lReturn = cInputFile.GetNextLine(sNextLine)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Error fall out of do....loop loop
                            Exit Do
                        End If
                        If m_sDocFileExtension.ToUpper = "XML" Then
                            r_sCurrentLine = r_sCurrentLine & sNextLine
                            If Right(sNextLine, 1) <> ">" Then
                                m_lReturn = FixBrokenMarkers(cInputFile, r_sCurrentLine)
                            End If
                        Else
                            r_sCurrentLine = r_sCurrentLine & " " & Trim(sNextLine)
                        End If
                        m_lLineCount = m_lLineCount + 1
                    Else
                        Exit Do
                    End If
                Else
                    Exit Do
                End If
            Loop

            m_lReturn = FixSplitMergeCodes(r_sCurrentLine)
        End If

        m_lReturn = RemoveSpellingAndGrammerTagsXML(r_sCurrentLine)
        m_lReturn = FixIfStatement(r_sCurrentLine)

        Return result

    End Function

    ' ************************************************************************
    ' Name: ResolveDocumentXML
    '
    ' Description: Open a document, resolve the fields, and return the document object
    '
    ' History : Created by Pankaj Kaushik
    ' ************************************************************************
    Private Function ResolveDocumentXML(ByRef sFileName As String,
                                         Optional ByVal vInstanceArray() As Object = Nothing,
                                         Optional ByVal vRiskId As Object = Nothing,
                                         Optional ByVal bInRiskLoop As Boolean = False,
                                         Optional ByVal v_iIsSubDoc As Integer = 0, Optional ByVal sDocType As String = "", Optional ByVal sDocName As String = "", Optional ByVal sRiskDescription As String = "") As Integer

        Const k_sFUNCTION_NAME As String = "ResolveDocumentXML"

        Const iRISK_ID_IDX As Integer = 0
        Const iRISK_DESC_IDX As Integer = 1
        Const iRISK_TYPE_IDX As Integer = 2
        Const iRPT_POINTER_IDX As Integer = 3
        Const iHEADER_IDX As Integer = 4
        Const iTRAILER_IDX As Integer = 5
        Const sLOOP_FILE As String = "loop_merge.xml"
        Const sLOOP_FILE_TEMP As String = "loop_template.xml"
        Const sRISK_FILE_TEMP As String = "risk_temp.xml"
        Const sSTD_WORDING_TEMP As String = "std_word.xml"
        Const sRESOLVEDXML_FILE_NAME As String = "ResolvedXML_"
        Const ACPolicyCurrencyDesc As Integer = 3
        Const ACPolicyCurrencyMinorPart As Integer = 4

        'Const ACSTdWrdsPosSequence As Integer = 0
        Const ACStdWrdsPosDocId As Integer = 1
        Const ACStdWrdsPosDocTypeId As Integer = 2
        Const ACStdWrdsPosCode As Integer = 3
        Const ACStdWrdsPosDescription As Integer = 4

        Dim sFieldCode As String = String.Empty
        Dim sValue As String = String.Empty
        Dim iType As Integer
        Dim iSep As Integer
        Dim sFieldType As String = String.Empty
        Dim sFieldName As String = String.Empty
        Dim sQuestion As String = String.Empty
        Dim sAnswer As String = String.Empty
        Dim bTempFlag As Boolean = False
        Static bIfExist As Boolean = False


        Dim vFieldArray(,) As Object
        Dim sCurrentLine As String = ""
        Dim sOriginalLine As String

        Dim sLoopLine As String = String.Empty

        Dim lFieldStart As Integer
        Dim lFieldEnd As Integer

        Dim lFieldCount As Integer
        Dim sFullFileName As String = String.Empty
        Dim sRiskDesc As String = String.Empty
        Dim sCommentValue As String = String.Empty
        Dim sMergeFile As String = String.Empty
        Dim sTmpFile As String = String.Empty
        Dim bSuppressOriginalLine As Boolean
        Dim sNextLine As String = String.Empty
        Dim bShowHeaders As Boolean
        Dim lTempDocId As Integer
        Dim lTempDocType As Integer
        Dim vRiskArray As Object = Nothing
        Dim iRiskCount As Integer
        Dim lPreviousRiskType As Integer
        Dim lPreviousRiskDocId As Integer
        Dim vKeyArray As Object = Nothing
        Dim lParentKey As Integer
        Dim iInstanceCount As Integer
        Dim lLoop As Integer
        Dim sLoopFile As String = String.Empty
        Dim sLoopFileTemp As String = String.Empty
        Dim iTotalNumber As Integer
        Dim sEquation As String = String.Empty
        Dim lRiskId As Integer  'DN 06/03/02
        Dim lLoopCount As Integer 'DJM 22/05/2002
        Dim lPos As Integer
        Dim sTemp As String = String.Empty
        Dim iInstanceTotal As Integer
        Dim iDocTypeSort As Integer = 1

        Dim vDocCurrency As Object = Nothing
        Dim sTempLineFragment As String = String.Empty
        Dim sTempLineFragment1 As String = String.Empty
        Dim sTempLoopLine As String = String.Empty

        Dim vClauseList As Object = Nothing
        Dim iItem As Integer
        Dim iClauseCnt As Integer

        Dim sCurrencyMajorPart As String = String.Empty
        Dim sCurrencyMinorPart As String = String.Empty
        Dim lCurrencySep As Integer
        Dim lCurrencySep2 As Integer

        Static iLoopDepth As Integer 'this determines how many recursions deep we are

        Dim bFullFileNameIsOpen As Boolean
        Dim bMergeFileIsOpen As Boolean
        Dim bTmpFileIsOpen As Boolean
        Dim iCurrentNestingLevel As Integer
        Dim iDeepestNestingLevel As Integer
        Dim sIfNumber As String = String.Empty               ' PW240603 - CQ1575
        Dim bCondition As Boolean
        Dim sSaveLine As String
        Dim bLineMerged As Boolean
        Dim vHoldInstanceArray As Object
        Dim bResultIsNumeric As Boolean
        Dim vValue As Object = Nothing
        Dim iNoofOccurences As Integer
        Dim strSearch As String = String.Empty
        Dim strToken As String = String.Empty
        Dim iCounter As Integer
        Dim vReturnValue As Object = Nothing
        Dim iExpressionStart As Integer
        Dim iExpressionEnd As Integer
        Dim iExpressionLen As Integer
        Dim cInputFile As FileClass

        Static sParentDocumentName As String = String.Empty
        Static vSubDocumentsArray As Object = Nothing
        Dim lFrom As Integer
        Dim lTo As Integer
        Dim lCounter As Integer
        Dim sParentDocumentWorkDir As String = String.Empty
        Dim sSubDocumentTemplateChain As String = String.Empty
        Dim bSubDocumentTemplateAlreadyExists As Boolean
        Dim sSubDocumentTemplateCode As String = String.Empty
        Dim lSubDocumentTemplateID As Integer
        Dim lSubDocumentTemplateTypeID As Integer
        Dim sSubDocumentTemplateDescription As String = String.Empty

        Dim sStandardWordingProperty As String = String.Empty
        Dim lStandardWordingDocumentTemplateID As Integer
        Dim lStandardWordingDocumentTemplateTypeID As Integer
        Dim sParentDocumentWorkDirectory As String
        Dim sParentDocumentWorkDirectoryRiskLoop As String = String.Empty
        Dim sParentDocumentWorkDirectoryStandardWording As String

        Dim lParentDocumentFileNumber As Integer
        Dim vDecimalLength As Object = Nothing

        Dim sSubDocContents As String = String.Empty
        Dim sRiskType As String = String.Empty
        Dim bValidRisk As Boolean
        Dim sMainFieldName As String = String.Empty
        Dim sTag() As String
        Dim lDocId As Integer

        Dim vStandardWordingsArray As Object = Nothing
        Dim iStdWordingCnt As Integer

        'RKS 051004 PN15089
        Dim lSelectedWordingCode As Integer
        Dim sStartingPartOfSplitLine As String = String.Empty
        Dim sEndingPartOfSplitLine As String = String.Empty
        Dim sLoopFullLine As String = String.Empty
        Dim sErrMsg As String = String.Empty
        Dim lTmpPos As Integer
        Dim oFile As Scripting.FileSystemObject
        Dim oTextStreamOut As Scripting.TextStream = Nothing
        Dim vRiskClausesArray As Object = Nothing
        Dim iClausesCnt As Integer
        Dim oFileNumLoop As Scripting.TextStream = Nothing
        Dim lTREndPos As Integer
        Dim sEndIFEndFragement As String = String.Empty
        Dim sEndIfResolvedFullLineFragement As String = String.Empty
        Dim bEndLoopFoundInSameLine As Boolean
        Dim bEndIfFoundInSameLine As Boolean
        Dim sEndIFFragement As String = String.Empty

        Dim sTmpStartLine As String
        Dim sTmpEndLine As String
        Dim sDocPreBodyFragment As String = String.Empty

        'declare to fatch previous Risk ID and insurance file count Etana Parallel
        Dim bPullDataFromPreviousVer As Boolean = False
        Dim oResultArray As Object = Nothing
        Dim nPrevVerInsuranceFileCnt As Long = 0
        Dim nPrevVerRiskID As Long = 0

        Dim sDocCode As String = ""
        Dim sDocDesc As String = ""
        Dim sFullFileNameResolvedXML As String
        Dim sGroup As String = String.Empty
        Dim sLoop1 As String = String.Empty
        Dim sLoop2 As String = String.Empty
        Dim sLoop3 As String = String.Empty
        Dim sSubGroup As String = String.Empty
        Dim sTableName As String = ""
        Dim nFirstInstance As Integer = 0
        Try
            Const DoubleQuotes As String = """"
            If (sDocType = "" Or sDocType = "Main") Then
                sDocType = "Main"
                iDocTypeSort = 0
            End If

            ResolveDocumentXML = gPMConstants.PMEReturnCode.PMTrue

            vHoldInstanceArray = vInstanceArray ' PW050304 - CQ4645

            'ED 26092002 - Initialise
            lFieldCount = 0

            vFieldArray = Nothing
            oFile = New Scripting.FileSystemObject

            m_lReturn = InitialiseExpressionParser(m_bNewDocument)
            m_bNewDocument = False

            sFullFileName = m_sClient & "\" & sFileName
            sFullFileNameResolvedXML = m_sClient & "\" & sRESOLVEDXML_FILE_NAME & sFileName
            sMergeFile = Left(sFullFileName, Len(sFullFileName) - 4) & ".tmp"

            iLoopDepth = iLoopDepth + 1 'increment depth of recursion

            If ToSafeLong(vRiskId) = 0 Then
                If m_oBusiness Is Nothing Then
                    sErrMsg = "2. m_oBusiness is not instantiated| filename = " & sFileName & "| loop depth = " & CStr(iLoopDepth)

                    ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                vRiskId = 0

                If m_lRiskCnt > 0 Then
                    vRiskId = m_lRiskCnt
                Else
                    m_lReturn = m_oBusiness.GetRiskForPolicy(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                          r_lRiskID:=lRiskId, r_sDescription:=sRiskDesc)
                    vRiskId = lRiskId
                    If sRiskDescription = "" Then
                        sRiskDescription = sRiskDesc
                    End If
                End If
            End If

            'if we are processing a claim document then make sure the risk_cnt is the one on this claim
            If m_lClaimCnt <> 0 Then
                m_lReturn = m_oBusiness.GetRiskID(v_lClaimCnt:=m_lClaimCnt, r_lRiskCnt:=lRiskId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Call LogMessageToFile(
                        sUsername:=m_sUsername,
                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                        sMsg:="Failed to get risk_id for claim - " & m_lClaimCnt,
                        vApp:=ACApp,
                        vClass:=ACClass,
                     vMethod:="ResolveDocumentXML")
                    ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                vRiskId = lRiskId
            End If
            m_lReturn = m_oBusiness.GetDocumentCurrency(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sDocumentRef:=m_sDocumentRef, r_vResultArray:=vDocCurrency, v_lPartyCnt:=m_lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Call bPMFunc.LogMessage(sUsername:=m_sUsername,
                                iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                sMsg:="Failed to get currency detail for this policy " & m_lInsuranceFileCnt,
                                vApp:=ACApp,
                                vClass:=ACClass,
                            vMethod:="ResolveDocumentXML")

                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            If m_oListClauses Is Nothing Then
                'Get the List of All Clauses
                GetListOfAllClauses()
            End If
            m_lFileCount = m_lFileCount + 1

            oTextStreamOut = oFile.CreateTextFile(sMergeFile, True, True)
            'Open sMergeFile For Output As #iFileNumOut
            bMergeFileIsOpen = True

            ' RVH 08/11/2004: (Performance) Open the input file, read to the <body> tag and transfer all data prior to
            '                               that tag to the output file.
            cInputFile = New FileClass

            m_lReturn = cInputFile.OpenAndReadFileXML(sFullFileName, oTextStreamOut, m_sUsername, v_iIsSubDoc, sDocPreBodyFragment)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cInputFile.OpenAndReadFileXML - Failed to open file.")
                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            bFullFileNameIsOpen = True

            ' If we are at the top level, store the document name
            If iLoopDepth = 1 Then
                sParentDocumentName = sFileName
            End If


            'Read in each line of document and analyse 1 by 1.
            Do While Not cInputFile.EOF

                bSuppressOriginalLine = False

                'RDT PN17131 - Migrated from 1.9.  This ensures that the entire line is read in
                '              when unwanted carrige returns exist.

                m_lReturn = ResolveReadFullLine(cInputFile:=cInputFile, r_sCurrentLine:=sCurrentLine)
                If sCurrentLine.Contains("wx:sub-section") Then
                    sCurrentLine = sCurrentLine.Replace("<wx:sub-section>", "")
                    sCurrentLine = sCurrentLine.Replace("</wx:sub-section>", "")
                End If

                Dim oAllIndexOfFieldStartMarker As List(Of Integer) = AllIndexOf(sCurrentLine, m_sFieldStartMarker)
                Dim oAllIndexOfFieldEndMarker As List(Of Integer) = AllIndexOf(sCurrentLine, m_sFieldEndMarker)
                For Each n As Integer In oAllIndexOfFieldEndMarker
                    oAllIndexOfFieldStartMarker.Add(n)
                Next
                If oAllIndexOfFieldStartMarker.Count > 0 Then
                    oAllIndexOfFieldStartMarker.Sort()
                End If
                'Check for Merge field present on this line.
                lFieldStart = InStr(sCurrentLine, m_sFieldStartMarker)
                If lFieldStart <> 0 Then
                    'Find the deepest nested field in the current
                    ' Reset variables
                    bLineMerged = True
                    iCurrentNestingLevel = 0
                    iDeepestNestingLevel = 0
                    lLoop = lFieldStart
                    lFieldStart = 0
                    ' Search through current line
                    Do Until lLoop > Len(sCurrentLine)
                        ' If mergefield found, increase the level, and check
                        ' if it is the deepest
                        If Mid(sCurrentLine, lLoop, m_iFieldMarkerLength) = m_sFieldStartMarker Then
                            If m_bArchiveAsXML Then
                                If Mid(sCurrentLine, lLoop + m_iFieldMarkerLength, 3).ToUpper() = "IF_" Then
                                    Dim i As Integer
                                    i = lLoop + m_iFieldMarkerLength + 1
                                    Do Until i > Len(sCurrentLine)
                                        If Mid(sCurrentLine, i, m_iFieldMarkerLength) = m_sFieldStartMarker Then
                                            Do Until i > Len(sCurrentLine)
                                                If Mid(sCurrentLine, i + m_iFieldMarkerLength, 1) = " " Then
                                                    i = i + 1
                                                Else
                                                    Exit Do
                                                End If
                                            Loop
                                            If Mid(sCurrentLine, i + m_iFieldMarkerLength, 3).ToUpper() = "DB_" Then
                                                bIfExist = True
                                            End If
                                            Exit Do
                                        ElseIf Mid(sCurrentLine, i, Len(m_sFieldEndMarker)) = m_sFieldEndMarker Then
                                            Exit Do
                                        End If
                                        i = i + 1
                                    Loop
                                End If
                            End If
                            iCurrentNestingLevel = iCurrentNestingLevel + 1
                            If iCurrentNestingLevel > iDeepestNestingLevel Then
                                iDeepestNestingLevel = iCurrentNestingLevel
                                ' If this is the deepest level, set the field pointer
                                lFieldStart = lLoop
                            End If
                            ' RVH 08/11/2004: (Performance) If it's a start marker, it can't be an end marker
                            ' If end of mergefield decrease the level
                        ElseIf Mid(sCurrentLine, lLoop, Len(m_sFieldEndMarker)) = m_sFieldEndMarker Then
                            iCurrentNestingLevel = iCurrentNestingLevel - 1
                            ' PW240603 - If level is back to 0, we have found the
                            ' end of a set of mergefields, so process these, i.e
                            ' exit (CQ1575)
                            If iCurrentNestingLevel = 0 Then
                                Exit Do
                            End If
                        End If
                        'lLoop = lLoop + 1
                        'Fwd the loop to the next
                        Dim nNewVal As Integer = 0
                        For Each n As Integer In oAllIndexOfFieldStartMarker
                            If n > lLoop Then
                                nNewVal = n
                                Exit For
                            End If
                        Next
                        If nNewVal > 0 Then
                            lLoop = nNewVal
                        Else
                            lLoop = Len(sCurrentLine) + 1
                        End If
                    Loop

                    ' Set the current mergefield nesting level we are working at
                    iCurrentNestingLevel = iDeepestNestingLevel
                End If

                Do While (lFieldStart <> 0)
                    'If Merge field is present then extract field.
                    sFieldCode = Mid$(sCurrentLine, lFieldStart + m_iFieldMarkerLength)
                    lFieldEnd = InStr(sFieldCode, m_sFieldEndMarker)

                    If lFieldEnd > 0 Then
                        sFieldCode = Left$(sFieldCode, lFieldEnd - 1)
                    End If
                    sNextLine = ""
                    iSep = 0
                    'DJM 01/07/2002 : Remove format tags from field code.
                    m_lReturn = RemoveFormatTags(sFieldCode)

                    ' Trim the type (for just in case)
                    sFieldType = Trim$(sFieldType)

                    'if it is standard wording extract standardwordingproperty field name
                    sStandardWordingProperty = ""
                    If sFieldCode.StartsWith(StandardWordingsTag) Then
                        If Left(sFieldCode, Len(StandardWordingNPTag)) = StandardWordingNPTag Then
                            'if there is a tag
                            If Len(sFieldCode) >= Len(StandardWordingNPTag) Then
                                'extract tag <@STANDARDWORDINGSNP:tag@>
                                sStandardWordingProperty = Mid(sFieldCode, Len(StandardWordingNPTag) + 2)
                                sFieldType = StandardWordingNPTag
                                sFieldCode = StandardWordingNPTag
                            End If
                        ElseIf Left(sFieldCode, Len(StandardWordingsCodeTag)) = StandardWordingsCodeTag Then
                            If Len(sFieldCode) >= Len(StandardWordingsCodeTag) Then
                                'extract tag <@STANDARDWORDINGS_CODE:tag@>
                                sStandardWordingProperty = Mid(sFieldCode, Len(StandardWordingsCodeTag) + 2)
                                'sFieldType = Trim$(UCase(sFieldCode)) 'PN No. 71032
                                sFieldType = StandardWordingsCodeTag
                                sFieldCode = StandardWordingsCodeTag
                            End If
                        ElseIf Left(sFieldCode, Len(StandardWordingsDescTag)) = StandardWordingsDescTag Then
                            If Len(sFieldCode) >= Len(StandardWordingsDescTag) Then
                                'extract tag <@STANDARDWORDINGS_DESC:tag@>
                                sStandardWordingProperty = Mid(sFieldCode, Len(StandardWordingsDescTag) + 2)
                                'sFieldType = Trim$(UCase(sFieldCode))'PN No. 71032
                                sFieldType = StandardWordingsDescTag
                                sFieldCode = StandardWordingsDescTag
                            End If
                        ElseIf Left(sFieldCode, Len(StandardWordingsTag)) = StandardWordingsTag Then
                            'if there is a tag
                            If Len(sFieldCode) >= Len(StandardWordingsTag) Then
                                'extract tag <@STANDARDWORDINGS:tag@>
                                sStandardWordingProperty = Mid(sFieldCode, Len(StandardWordingsTag) + 2)
                                sFieldType = StandardWordingsTag
                                sFieldCode = StandardWordingsTag
                                iSep = 0
                            End If
                        End If
                    Else
                        ' Determine the field type
                        iSep = InStr(sFieldCode, "_")
                        If (iSep > 0) Then
                            sFieldType = Trim$(UCase(Left$(sFieldCode, iSep - 1)))
                        Else
                            sFieldType = Trim$(UCase(sFieldCode))
                        End If
                    End If

                    Select Case sFieldType

                        Case ParameterTag

                            sValue = ""

                            ' extract the field name
                            sFieldName = LCase(Right(sFieldCode, Len(sFieldCode) - iSep))
                            sFieldName = Trim$(sFieldName)

                            m_lReturn = ExtractParameterDetails(v_iCurrentNestingLevel:=iCurrentNestingLevel,
                                                                v_sName:=sFieldName,
                                                                r_sValue:=sValue,
                                                            r_iType:=iType)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sValue = Chr(147) & Chr(148)
                            End If

                            sCurrentLine = Left(sCurrentLine, lFieldStart - 1) & sValue _
                                           & Mid(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)

                        Case DbTag

                            ' extract the field name
                            sFieldName = Right$(sFieldCode, Len(sFieldCode) - iSep)

                            If Left(sFieldName, 3) = "SW_" And m_bCalledFromSwift Then
                                ' This is a Swift Field, so do it accordingly
                                ' It will start with SW_
                                iSep = InStr(Len("SW_") + 1, sFieldName, "_")
                                If (iSep > 0) Then
                                    sFieldName = Mid$(sFieldName, iSep + 1)

                                    ' Strip off the id character at the end
                                    iSep = InStr(Len("SW_") + 1, sFieldName, "_")
                                    If (iSep > 0) Then
                                        sFieldName = Mid$(sFieldName, Len("SW_") + 1, iSep - Len("SW_") - 1)
                                    End If
                                End If
                            Else
                                'Start - (Prakash Varghese) - PN 63549
                                'Merged code from 1.12 build
                                sTag = Split(sFieldName, "_")

                                If IsArray(sTag) Then
                                    If UCase$(sTag(UBound(sTag))) <> "ID" Then
                                        sFieldName = sTag(UBound(sTag))
                                    End If
                                End If

                                If UBound(sTag) > 0 Then
                                    If UCase$(sTag(UBound(sTag))) = "ID" Then
                                        sFieldName = sTag(UBound(sTag) - 1)
                                    End If
                                End If
                            End If

                            If UCase$((Right(sFieldName, Len("USRSignatureFile")))) = "USRSIGNATUREFILE" Then
                                ' PW280403 - special case for signature file (which will be an image file)

                                m_lReturn = GetUserSignatureFile(sTmpFile)

                                sTmpFile = sTmpFile & ".xml"

                                ConvertDocumentUsingSiriusDocumentUtility(sTmpFile, sTmpFile)

                                sSubDocContents = GetSubDocXmlContents(cInputFile, sTmpFile, True)

                                bSuppressOriginalLine = False

                                sStartingPartOfSplitLine = Left$(sCurrentLine, lFieldStart - 1)

                                sEndingPartOfSplitLine = Mid(sCurrentLine, lFieldStart + lFieldEnd +
                                                    (m_iFieldMarkerLength * 2) - 1)

                                m_lReturn = GetFullLines(sStartingPartOfSplitLine, sEndingPartOfSplitLine)

                                m_lReturn = RemoveEmptyParagraphsXML(sStartingPartOfSplitLine, True)

                                If Len(Trim$(sStartingPartOfSplitLine)) = 0 Then
                                    oTextStreamOut.Write(sSubDocContents)
                                    m_lReturn = RemoveEmptyParagraphsXML(sEndingPartOfSplitLine)
                                    sCurrentLine = sEndingPartOfSplitLine
                                Else
                                    sCurrentLine = sStartingPartOfSplitLine & sSubDocContents & sEndingPartOfSplitLine
                                End If

                            Else
                                iInstanceTotal = MinFieldInstances

                                'Dimension vFieldArray using vInstanceArray, if supplied
                                If Not vInstanceArray Is Nothing Then

                                    'if greater than MinFieldInstances then
                                    If UBound(vInstanceArray) > (MinFieldInstances - 1) Then

                                        'Set for later use
                                        iInstanceTotal = UBound(vInstanceArray) + 1

                                        ReDim vFieldArray(UBound(vInstanceArray) + FieldInstance1, lFieldCount)
                                    Else

                                        'Otherwise use defaults - (Start of first instance in vFieldArray - 1) +
                                        '                          MinFieldInstances
                                        ReDim vFieldArray(((FieldInstance1 - 1) + MinFieldInstances), lFieldCount)
                                    End If
                                Else

                                    'Otherwise use defaults - (Start of first instance in vFieldArray - 1) +
                                    '                          MinFieldInstances
                                    ReDim vFieldArray(((FieldInstance1 - 1) + MinFieldInstances), lFieldCount)
                                End If
                                'ED 26092002 - End

                                ' Stuff the bookmark name and field name into the array
                                vFieldArray(FieldCode, lFieldCount) = sFieldCode
                                vFieldArray(FieldName, lFieldCount) = sFieldName
                                For iInstanceCount = 0 To iInstanceTotal - 1
                                    If Information.IsNothing(vInstanceArray) Then
                                        vFieldArray(iInstanceIndexArray(iInstanceCount), lFieldCount) = 0
                                    Else
                                        If (iInstanceCount > UBound(vInstanceArray)) Then
                                            If (iInstanceIndexArray(iInstanceCount) < UBound(vFieldArray)) Then
                                                vFieldArray(iInstanceIndexArray(iInstanceCount) + 1, lFieldCount) = 0
                                            End If
                                        Else
                                            'DC190505 PN20472 broking uses instance2 and not instance1 for loop count

                                            vFieldArray(iInstanceIndexArray(iInstanceCount), lFieldCount) = vInstanceArray(iInstanceCount)

                                        End If
                                    End If
                                Next iInstanceCount

                                'Need to pass in the user_id when retrieving user details.
                                If InStr(1, UCase(sFieldName), "USERDET") <> 0 Or UCase(sFieldName) = "USERNAME" Then
                                    vFieldArray(FieldInstance1, lFieldCount) = m_iUserID
                                End If

                                If m_oBusiness Is Nothing Then
                                    'CQ6053 DEBUG
                                    Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=
                                        "3. m_oBusiness is not instantiated| filename = " & sFileName & "| loop depth = " & CStr(iLoopDepth),
                                    vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)
                                    ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If


                                If Right(sFieldName, 3) = "[1]" Then
                                    Dim v_lRiskCnt As Long
                                    v_lRiskCnt = ToSafeLong(vRiskId)
                                    m_lReturn = m_oBusiness.GetPreviousLivePolicyDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                                           v_lRiskCnt:=v_lRiskCnt,
                                                                                           r_vResultArray:=oResultArray)
                                    If IsArray(oResultArray) Then
                                        nPrevVerInsuranceFileCnt = CLng(oResultArray(0, 0))
                                        nPrevVerRiskID = CLng(oResultArray(1, 0))
                                    End If
                                    sFieldName = Left(sFieldName, Len(sFieldName) - 3)
                                    bPullDataFromPreviousVer = True
                                    vRiskId = nPrevVerRiskID
                                ElseIf Right(sFieldName, 3) = "[0]" Then
                                    sFieldName = Left(sFieldName, Len(sFieldName) - 3)
                                End If




                                If bPullDataFromPreviousVer = True Then
                                    m_lReturn = m_oBusiness.GetFieldValues(lPartyCnt:=m_lPartyCnt,
                                                                                lInsuranceFileCnt:=nPrevVerInsuranceFileCnt,
                                                                                lClaimCnt:=m_lClaimCnt,
                                                                                sDocumentRef:=m_sDocumentRef,
                                                                                vArray:=vFieldArray,
                                                                                vRiskId:=nPrevVerRiskID,
                                                                   sGroup:=sGroup, sSubGroup:=sSubGroup, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3)
                                    bPullDataFromPreviousVer = False
                                Else
                                    m_lReturn = m_oBusiness.GetFieldValues(lPartyCnt:=m_lPartyCnt,
                                                                   lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                   lClaimCnt:=m_lClaimCnt,
                                                                   sDocumentRef:=m_sDocumentRef,
                                                                   vArray:=vFieldArray,
                                                                   vRiskId:=vRiskId,
                                                                   sGroup:=sGroup, sSubGroup:=sSubGroup, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, r_sTableName:=sTableName)
                                End If

                                ' Get field name and value
                                sFieldName = UCase$(ToSafeString(vFieldArray(FieldName, lFieldCount)))
                                If Right(sFieldName, 3) = "[1]" Or Right(sFieldName, 3) = "[0]" Then
                                    sFieldName = Left(sFieldName, Len(sFieldName) - 3)
                                End If

                                iType = ToSafeInteger(vFieldArray(FieldType, lFieldCount))
                                If iType = 11 Or iType = 16 Then
                                    sValue = ToSafeCurrency(vFieldArray(FieldValue, lFieldCount))
                                Else
                                    sValue = Trim$(ToSafeString(vFieldArray(FieldValue, lFieldCount)))
                                End If

                                If (m_bArchiveAsXML) Then
                                    If (bIfExist = False And sGroup <> "" And (Not sValue.Contains("<w:") Or Not sValue.Contains("</w:") Or sValue.IndexOf("{\rtf1") <> 0)) Then
                                        AddToTable(iDocTypeSort:=iDocTypeSort, sDocType:=sDocType, sDocName:=sDocName, sMainGroup:=sGroup, sSubGroup:=sSubGroup, sFieldName:=sFieldName, sValue:=sValue, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, iRiskCnt:=ToSafeInteger(vRiskId), sRiskDescription:=sRiskDescription)
                                    End If
                                    bIfExist = False
                                End If

                                'Convert to words if need be
                                If InStr(1, sFieldName, "THISPAYMENTWORD") > 0 _
                                    Or UCase(Right(sFieldName, Len("CLIAmountInWords"))) = "CLIAMOUNTINWORDS" _
                                    Or UCase(Right(sFieldName, Len("CashAmountWord"))) = "CASHAMOUNTWORD" Then

                                    'default to policy level currency details
                                    If m_aoDocCurrency Is Nothing Then
                                        GetDocCurrency()
                                    End If
                                    If Not m_aoDocCurrency Is Nothing Then
                                        sCurrencyMajorPart = m_aoDocCurrency(ACPolicyCurrencyDesc, 0)
                                        sCurrencyMinorPart = m_aoDocCurrency(ACPolicyCurrencyMinorPart, 0)
                                    End If

                                    'check to see if we have currency details attached to this field
                                    ' format expected - value|major part|minor part
                                    'look at spu_wp_ThisClaim for example
                                    lCurrencySep = InStr(1, sValue, "|")
                                    If lCurrencySep > 0 Then
                                        lCurrencySep2 = InStr(lCurrencySep + 1, sValue, "|")

                                        If lCurrencySep2 > 0 Then
                                            sCurrencyMajorPart = Mid(sValue, lCurrencySep + 1, lCurrencySep2 - lCurrencySep - 1)
                                            sCurrencyMinorPart = Mid(sValue, lCurrencySep2 + 1)
                                            sValue = Mid(sValue, 1, lCurrencySep - 1)
                                        End If
                                    End If

                                    Dim dbNumericTemp As Double
                                    If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                                        sValue = NumToWord(v_cValue:=System.Math.Abs(CDec(sValue)), v_sIntegerPartCode:=sCurrencyMajorPart, v_sDecimalPartcode:=sCurrencyMinorPart)
                                    Else
                                        sValue = NumToWord(v_cValue:=0,
                                                         v_sIntegerPartCode:=sCurrencyMajorPart,
                                                     v_sDecimalPartcode:=sCurrencyMinorPart)
                                    End If
                                ElseIf InStr(1, sFieldName, "THISPAYMENT") > 0 Then
                                    If Not IsNumeric(sValue) Then
                                        sValue = "0.00"
                                    End If
                                ElseIf UCase(Right(sFieldName, Len("CASHAMOUNTWORDSMAJOR"))) = "CASHAMOUNTWORDSMAJOR" Then
                                    'Convert integer part to just words
                                    If sValue <> "" Then
                                        sValue = NumToWord(v_cValue:=System.Math.Abs(IIf(sValue > 0, System.Math.Floor(CDbl(sValue)), System.Math.Ceiling(CDbl(sValue)))), v_bConvertDecimal:=False, v_sIntegerPartCode:="", v_sDecimalPartcode:="")
                                    End If
                                ElseIf UCase(Right(sFieldName, Len("CASHAMOUNTWORDSMINOR"))) = "CASHAMOUNTWORDSMINOR" Then
                                    'Convert decimal part to just words
                                    If sValue <> "" Then
                                        sValue = NumToWord(v_cValue:=System.Math.Abs(CDec(sValue) - (IIf(sValue > 0, System.Math.Floor(CDbl(sValue)), System.Math.Ceiling(CDbl(sValue))))), v_bConvertDecimal:=True, v_sIntegerPartCode:="", v_sDecimalPartcode:="")
                                        sValue = sValue.Replace("Zero  and ", "").Trim()
                                    End If
                                Else
                                    'check if it is a standard wording property
                                    'Keep it in the end because there could be other property types with sValue = ""
                                    If sValue = "" Then
                                        vRiskClausesArray = ""
                                        If Not Information.IsNothing(vInstanceArray) Then

                                            If UBound(vInstanceArray) >= 1 Then
                                                If sFieldName = "SWCODE" Or sFieldName = "SWDESC" Then
                                                    sMainFieldName = Left(sFieldCode, Len(sFieldCode) - (Len(sFieldName) + 1))
                                                    sMainFieldName = Mid(sMainFieldName, InStr(sMainFieldName, "_") + 1, Len(sMainFieldName))
                                                    sMainFieldName = Mid(sMainFieldName, InStr(sMainFieldName, "_") + 1, Len(sMainFieldName))
                                                    If (m_oListClauses.Contains(sMainFieldName)) Then
                                                        m_lReturn = m_oBusiness.GetRiskClauses(vRiskClausesArray, vRiskId, vInstanceArray(UBound(vInstanceArray)), sMainFieldName)
                                                    End If
                                                Else
                                                    If (m_oListClauses.Contains(sFieldName)) Then
                                                        m_lReturn = m_oBusiness.GetRiskClauses(vRiskClausesArray, vRiskId, vInstanceArray(UBound(vInstanceArray)), sFieldName)
                                                    End If
                                                End If
                                            Else
                                                If sFieldName = "SWCODE" OrElse sFieldName = "SWDESC" Then
                                                    sMainFieldName = Left(sFieldCode, Len(sFieldCode) - (Len(sFieldName) + 1))
                                                    sMainFieldName = Mid(sMainFieldName, InStr(sMainFieldName, "_") + 1, Len(sMainFieldName))
                                                    sMainFieldName = Mid(sMainFieldName, InStr(sMainFieldName, "_") + 1, Len(sMainFieldName))
                                                    If (m_oListClauses.Contains(sMainFieldName)) Then
                                                        m_lReturn = m_oBusiness.GetRiskClauses(vRiskClausesArray, vRiskId, DBNull.Value, sMainFieldName)
                                                    End If
                                                Else
                                                    If (m_oListClauses.Contains(sFieldName)) Then
                                                        m_lReturn = m_oBusiness.GetRiskClauses(vRiskClausesArray, vRiskId, DBNull.Value, sFieldName)
                                                    End If

                                                End If

                                            End If
                                        Else
                                            If sFieldName = "SWCODE" OrElse sFieldName = "SWDESC" Then

                                                sMainFieldName = Left(sFieldCode, Len(sFieldCode) - (Len(sFieldName) + 1))
                                                sMainFieldName = Mid(sMainFieldName, InStr(sMainFieldName, "_") + 1, Len(sMainFieldName))
                                                sMainFieldName = Mid(sMainFieldName, InStr(sMainFieldName, "_") + 1, Len(sMainFieldName))
                                                If (m_oListClauses.Contains(sMainFieldName)) Then
                                                    m_lReturn = m_oBusiness.GetRiskClauses(vRiskClausesArray, vRiskId, DBNull.Value, sMainFieldName)
                                                End If

                                            Else
                                                If (m_oListClauses.Contains(sFieldName)) Then
                                                    m_lReturn = m_oBusiness.GetRiskClauses(vRiskClausesArray, vRiskId, DBNull.Value, sFieldName)
                                                End If

                                            End If
                                        End If

                                        'Merge if successful and clauses available otherwise continue with rest of the docs
                                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And IsArray(vRiskClausesArray) Then
                                            'store the original line as there may be multiple clauses

                                            sOriginalLine = sCurrentLine

                                            For iClausesCnt = LBound(vRiskClausesArray, 2) To UBound(vRiskClausesArray, 2)
                                                sCurrentLine = sOriginalLine
                                                'To retain any formatting, replace the tags with the values
                                                If UCase(sFieldName) = "SWCODE" Then
                                                    sCurrentLine = Replace(sCurrentLine, m_sFieldStartMarker, "")
                                                    sCurrentLine = Replace(sCurrentLine, m_sFieldEndMarker, "")
                                                    sCurrentLine = Replace(sCurrentLine, vFieldArray(FieldCode, lFieldCount), vRiskClausesArray(2, iClausesCnt))

                                                    m_lReturn = RemoveInvalidCharacters(sCurrentLine)
                                                    oTextStreamOut.Write(sCurrentLine)
                                                    sCurrentLine = ""
                                                ElseIf UCase(sFieldName) = "SWDESC" Then
                                                    sCurrentLine = Replace(sCurrentLine, m_sFieldStartMarker, "")
                                                    sCurrentLine = Replace(sCurrentLine, m_sFieldEndMarker, "")
                                                    sCurrentLine = Replace(sCurrentLine, vFieldArray(FieldCode, lFieldCount), vRiskClausesArray(3, iClausesCnt))

                                                    m_lReturn = RemoveInvalidCharacters(sCurrentLine)
                                                    oTextStreamOut.Write(sCurrentLine)
                                                    sCurrentLine = ""
                                                Else
                                                    ' Get temp file name.
                                                    sTmpFile = vRiskClausesArray(1, iClausesCnt)
                                                    If Not String.IsNullOrEmpty(sTmpFile) Then
                                                        sParentDocumentWorkDirectoryStandardWording = m_sClient

                                                        m_sClient = ""
                                                        m_lReturn = GetClientFolder()

                                                        m_lReturn = CopyServerToClient(lCLAUSE_TYPE_ID, sTmpFile)
                                                        sTmpFile = "Doc " & sTmpFile & "." & m_sDocFileExtension
                                                        If m_dtStdWording IsNot Nothing AndAlso (m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(1) Is Nothing OrElse m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(1).ToString() = "") Then
                                                            m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(1) = vRiskClausesArray(2, iClausesCnt)
                                                        ElseIf m_dtStdWording IsNot Nothing Then
                                                            AddClauseToTableForCCM(m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(0))
                                                            m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(1) = vRiskClausesArray(2, iClausesCnt)
                                                        End If
                                                        m_lReturn = ProcessSubDocXml(sSourceTemplate:=sTmpFile,
                                                                         sDestinationMerge:=sSTD_WORDING_TEMP,
                                                                         oTextStreamOut:=oTextStreamOut,
                                                                         cInputFile:=cInputFile,
                                                                     vRiskId:=vRiskId, bInRiskLoop:=bInRiskLoop, v_bMergeStyles:=True, sDocType:=sDocType, sDocName:=sDocName, sRiskDescription:=sRiskDescription)


                                                        'Delete standard wording doc as it has been merged into
                                                        'parent by now.
                                                        m_lReturn = DeleteClient(m_sClient, sTmpFile)

                                                        ' Restore the Original ParentWorkDirectory to current work directory
                                                        m_sClient = sParentDocumentWorkDirectoryStandardWording
                                                    End If
                                                End If

                                            Next iClausesCnt

                                        End If
                                    End If
                                End If

                                'Need some formatting...
                                ' PW110603 - format differently if nested: start
                                If iCurrentNestingLevel > 1 Then
                                    Select Case iType
                                        Case gPMConstants.PMEFormatStyle.PMFormatString
                                            sValue = DoubleQuotes & sValue & DoubleQuotes

                                        Case gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatDateShort,
                                             gPMConstants.PMEFormatStyle.PMFormatDayOnlyLong, gPMConstants.PMEFormatStyle.PMFormatDayOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatDayOnlyShort,
                                             gPMConstants.PMEFormatStyle.PMFormatMonthOnlyLong, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyShort,
                                         gPMConstants.PMEFormatStyle.PMFormatDateYearOnly
                                            If IsDate(sValue) Then
                                                sValue = Format(CDate(sValue), "dd-MMM-yyyy")
                                            Else
                                                sValue = Format(#12/29/1899#, "dd-MMM-yyyy")
                                            End If
                                        Case gPMConstants.PMEFormatStyle.PMFormatDateTimeLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium, gPMConstants.PMEFormatStyle.PMFormatDateTimeShort
                                            If IsDate(sValue) Then
                                                sValue = Format(CDate(sValue), "dd-MMM-yyyy hh:mm:ss")
                                            Else
                                                sValue = Format(#12/29/1899#, "dd-MMM-yyyy hh:mm:ss")
                                            End If
                                        Case gPMConstants.PMEFormatStyle.PMFormatBoolean
                                            If sValue = "1" Or sValue = "True" Then
                                                sValue = DoubleQuotes & "Yes" & DoubleQuotes
                                            ElseIf sValue = "0" Or sValue = "False" Then
                                                sValue = DoubleQuotes & "No" & DoubleQuotes
                                            Else
                                                sValue = DoubleQuotes & String.Empty & DoubleQuotes
                                            End If

                                        Case Else
                                            'DJM 07/05/2002 : Moved the following lines here so
                                            'that empty PMFormatStrings aren't replaced with zeros
                                            If sValue = "" Then
                                                sValue = "0"
                                            End If
                                            sValue = " " & sValue
                                    End Select
                                Else
                                    ' PW110603 - format differently if nested: end
                                    Select Case iType
                                        Case gPMConstants.PMEFormatStyle.PMFormatLong
                                        Case gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatDateShort,
                                             gPMConstants.PMEFormatStyle.PMFormatDateTimeLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium, gPMConstants.PMEFormatStyle.PMFormatDateTimeShort,
                                             gPMConstants.PMEFormatStyle.PMFormatDayOnlyLong, gPMConstants.PMEFormatStyle.PMFormatDayOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatDayOnlyShort,
                                             gPMConstants.PMEFormatStyle.PMFormatMonthOnlyLong, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyShort,
                                         gPMConstants.PMEFormatStyle.PMFormatDateYearOnly
                                            If IsDate(sValue) Then
                                                If (CDate(sValue) = #12/29/1899#) Then
                                                    sValue = ""
                                                Else
                                                    'Start(Sriram P)PNcovernote
                                                    If sFieldName = "COVERNOTEENDDATETIME" Or sFieldName = "COVERNOTESTARTDATETIME" Then
                                                        iType = 10
                                                    End If
                                                    'End(Sriram P)PNcovernote
                                                    sValue = gPMFunctions.FormatField(iFormatType:=iType, vFieldValue:=sValue)
                                                End If
                                            Else
                                                sValue = ""
                                            End If

                                        Case gPMConstants.PMEFormatStyle.PMFormatBoolean
                                            If IsNumeric(sValue) Then
                                                If sValue = "1" Then
                                                    sValue = "Yes"
                                                ElseIf sValue = "0" Or sValue = "False" Then
                                                    sValue = "No"
                                                Else
                                                    sValue = String.Empty
                                                End If
                                            Else
                                                If sValue = "1" Or sValue = "True" Or ToSafeBoolean(sValue) Then
                                                    sValue = "Yes"
                                                Else
                                                    sValue = "No"
                                                End If
                                            End If
                                        Case gPMConstants.PMEFormatStyle.PMFormatPercent
                                            'vDecimalLength = Len(Mid(sValue, InStrRev(sValue, ".") + 1, Len(sValue)))
                                            sValue = gPMFunctions.FormatField(iFormatType:=iType, vFieldValue:=sValue, vDecimalPlaces:=2)
                                        Case Else
                                            sValue = gPMFunctions.FormatField(iFormatType:=iType, vFieldValue:=sValue)
                                    End Select
                                End If

                                'Thinh Nguyen 25/06/2002 (start) - format it for cheque printing
                                If InStr(1, sFieldName, "THISPAYMENTCHEQUE") > 0 Then
                                    sValue = Pad(sValue, 1, "*", 17)
                                End If
                                'Thinh Nguyen 25/06/2002 (end) - format it for cheque printing
                            End If

                            If sValue.IndexOf("{\rtf1") = 0 Then

                                Dim sRTFTempFile As String
                                Dim sRTFTempXMLFile As String
                                bTempFlag = True
                                '<generate a unique temp filename.rtf>
                                sValue = sValue.Replace("&#xD;", Global.Microsoft.VisualBasic.ChrW(13))
                                sValue = sValue.Replace("&#xA;", Global.Microsoft.VisualBasic.ChrW(10))
                                sRTFTempFile = m_sClient & "\" & "TempRTF" & ".doc"
                                sRTFTempXMLFile = m_sClient & "\" & "TempXML" & ".xml"
                                Dim rtfBytes As Byte() = Encoding.UTF8.GetBytes(sValue)
                                'Create memorystream
                                Dim rtfStream As MemoryStream = New MemoryStream(rtfBytes)
                                'Create document from stream
                                Dim rtfDoc As SiriusDocumentUtility.Document = New SiriusDocumentUtility.Document(rtfStream, sRTFTempFile)

                                ConvertDocumentUsingSiriusDocumentUtility(sRTFTempFile, sRTFTempXMLFile)

                                sValue = GetSubDocXmlContents(cInputFile, sRTFTempXMLFile, True)
                                sValue = Replace(sValue, "&lt;br&gt;", "")
                                sRTFTempFile = Nothing
                                sRTFTempXMLFile = Nothing
                                rtfStream.Close()
                                rtfBytes = Nothing
                            End If
                            If m_bArchiveAsXML And (sValue.Contains("<w:") Or sValue.Contains("</w:")) Then
                                Dim vParagraphArray1 As Object
                                m_lReturn = BreakStringIntoArray("<w:p", "</w:p>", sValue, vParagraphArray1)
                                If IsArray(vParagraphArray1) Then
                                    For lCnt As Integer = 0 To UBound(vParagraphArray1)
                                        sTemp = vParagraphArray1(lCnt)
                                        If (bTempFlag = False) Then
                                            Dim i, j As Integer
                                            i = sTemp.IndexOf("<w:")
                                            j = sTemp.IndexOf("</w:")

                                            If (i > -1 And j > -1) Then
                                                If i > j Then
                                                    sCommentValue = sCommentValue + sTemp.Substring(0, j)
                                                Else
                                                    sCommentValue = sCommentValue + sTemp.Substring(0, i)
                                                End If
                                            End If
                                        End If
                                        sCommentValue += " " + GetParagarphText(sTemp, False)


                                    Next
                                    sValue = sCommentValue
                                    bTempFlag = False
                                    AddToTable(iDocTypeSort:=iDocTypeSort, sDocType:=sDocType, sDocName:=sDocName, sMainGroup:=sGroup, sSubGroup:=sSubGroup, sFieldName:=sFieldName, sValue:=sValue, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, iRiskCnt:=ToSafeInteger(vRiskId), sRiskDescription:=sRiskDescription)
                                End If

                            End If



                            sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) _
                                                    & sValue _
                                            & Mid$(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)

                        Case ClauseTag

                            m_lReturn = m_oBusiness.GetPolicyEffectiveDate(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                            r_dtEffectiveDate:=m_dtDocEffectiveDate)

                            ' extract the field name
                            sTmpFile = Right$(sFieldCode, Len(sFieldCode) - iSep)

                            If IsNumeric(sTmpFile) Then
                                ' we have a clause request

                                If m_lClaimCnt = 0 And m_lInsuranceFileCnt > 0 Then
                                    'PN36517
                                    'Filter out claims and fix as per section; 4.3.1 Clauses and Risk Looping of the PLICO 21
                                    lDocId = sTmpFile
                                    m_lReturn = m_oBusiness.GetTemplateFromCode(sCode:="", lDocId:=lDocId, lDocType:=lCLAUSE_TYPE_ID, v_dtEffectiveDate:=m_dtDocEffectiveDate)
                                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                        'Assign effective doc Id to process & merge
                                        'It will be 0 if no clause available for eff date
                                        sTmpFile = lDocId
                                    End If
                                End If

                                If ToSafeLong(sTmpFile) <> 0 Then
                                    'Merge if a clause doc available for effective date
                                    sParentDocumentWorkDirectory = m_sClient

                                    'RKS 051004
                                    'make sure we have a unique folder for each document
                                    m_sClient = ""
                                    m_lReturn = GetClientFolder()

                                    m_lReturn = CopyServerToClient(lCLAUSE_TYPE_ID, CLng(sTmpFile))
                                    m_lReturn = m_oBusiness.GetTemplateCodeAndDescFromID(sCode:=sDocCode, sDesc:=sDocDesc, lDocId:=Convert.ToInt32(sTmpFile))
                                    sTmpFile = "Doc " & sTmpFile & "." & m_sDocFileExtension
                                    If m_bArchiveAsXML Then
                                        AddToTable(iDocTypeSort:=1, sDocType:="CL", sDocName:=sDocCode, sMainGroup:="", sSubGroup:="", sFieldName:="", sValue:="", sLoop1:="", sLoop2:="", sLoop3:="", iRiskCnt:=ToSafeInteger(vRiskId), sRiskDescription:=sRiskDescription)
                                    End If
                                    'RWH(16/01/2001) Use Instance Array
                                    m_lReturn = ResolveDocumentXML(sFileName:=sTmpFile,
                                                                        vInstanceArray:=vInstanceArray,
                                                                        vRiskId:=vRiskId,
                                                                    bInRiskLoop:=bInRiskLoop, sDocType:="CL", sDocName:=sDocCode, sRiskDescription:=sRiskDescription)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        ResolveDocumentXML = m_lReturn       ' Set the error status
                                        'Remove the current working directory.  Since we don't need them anymore
                                        If m_bUniqueClientDirNeedsDeleting Then
                                            m_lReturn = DelDirectory(m_sClient)
                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                                ' Log Error.
                                                bPMFunc.LogMessage(
                                                    sUsername:=m_sUsername,
                                                    iType:=gPMConstants.PMEReturnCode.PMError,
                                                    sMsg:="Failed to delete Folder [" & m_sClient & "]" & vbCrLf & "after error in ResolveDocumentXML for " & vbCrLf & sTmpFile,
                                                    vApp:=ACApp,
                                                    vClass:=ACClass,
                                                    vMethod:="ResolveDocumentXML",
                                                    vErrNo:=Err.Number,
                                                vErrDesc:=Err.Description)
                                            End If
                                        End If
                                        'Reduce the level to represent that we have left recursion by one level
                                        iLoopDepth = iLoopDepth - 1
                                        ' Reset the m_sClient Directory, while exiting
                                        m_sClient = sParentDocumentWorkDirectory
                                        ' Closing all files  - TO DO

                                        ' Exit function
                                        Exit Function
                                    End If
                                    sTmpFile = m_sClient & "\" & sTmpFile

                                    If sCurrentLine.Contains("<w:br w:type=") Then
                                        m_lReturn = RemoveContentsOfTag("<w:t>", "</w:t>", sCurrentLine, True)
                                        oTextStreamOut.Write(sCurrentLine)
                                    End If

                                    m_lReturn = InsertSubDocXML(sTmpFile, oTextStreamOut, cInputFile, v_bMergeStyles:=True)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        ResolveDocumentXML = m_lReturn
                                        ' Log Error.
                                        bPMFunc.LogMessage(
                                            sUsername:=m_sUsername,
                                            iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                            sMsg:="InsertSubDocxml Failed for [" & sTmpFile & "]" & vbCrLf & "while resolving Clauses : " & sFieldCode,
                                            vApp:=ACApp,
                                            vClass:=ACClass,
                                            vMethod:="ResolveDocumentXML",
                                            vErrNo:=Err.Number,
                                        vErrDesc:=Err.Description)
                                        Exit Function
                                    End If


                                    ' RAM20040301 : Use FSO to delete the file, rather than Kill
                                    If m_bUniqueClientDirNeedsDeleting Then
                                        m_lReturn = DelDirectory(m_sClient)
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                            ' Log Error.
                                            bPMFunc.LogMessage(
                                                    sUsername:=m_sUsername,
                                                    iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                    sMsg:="Failed to delete Folder [" & m_sClient & "]" & vbCrLf & "while resolving Clauses : " & sFieldCode,
                                                    vApp:=ACApp,
                                                    vClass:=ACClass,
                                                    vMethod:="ResolveDocumentXML",
                                                    vErrNo:=Err.Number,
                                            vErrDesc:=Err.Description)
                                            Exit Function
                                        End If
                                    End If


                                    ' Restore the Original ParentWorkDirectory to current work directory
                                    m_sClient = sParentDocumentWorkDirectory
                                End If
                                sCurrentLine = ""

                                bSuppressOriginalLine = True

                                ' Reset the flag for parent document. Else, when the object terminates and
                                ' if this flag is set by the Clause, then it will enforce the parent
                                ' document work directory deletion
                                If iLoopDepth = 1 Then
                                    m_bUniqueClientDirNeedsDeleting = False
                                End If

                            Else
                                ' we have a clause code or desc request

                                ' load up the clause codes for this document
                                If Not IsArray(vClauseList) Then
                                    m_lReturn = m_oBusiness.GetClauseList(m_lDocumentTemplateId, vClauseList)
                                End If

                                If IsArray(vClauseList) Then
                                    If InStr(sTmpFile, "_") Then
                                        If Left(sTmpFile, 1) = "C" Then
                                            ' Code
                                            iItem = 1
                                        Else
                                            ' Desc
                                            iItem = 2
                                        End If

                                        sTmpFile = Mid(sTmpFile, InStr(sTmpFile, "_") + 1)
                                    End If

                                    If m_lClaimCnt = 0 And m_lInsuranceFileCnt > 0 Then
                                        'PN36517
                                        'Filter out claims and fix as per section; 4.3.1 Clauses and Risk Looping of the PLICO 21
                                        lDocId = sTmpFile
                                        m_lReturn = m_oBusiness.GetTemplateFromCode(sCode:="", lDocId:=lDocId, lDocType:=7, v_dtEffectiveDate:=m_dtDocEffectiveDate)
                                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                            'Assign effective doc Id to process & merge
                                            'It will be 0 if no clause available for eff date
                                            sTmpFile = lDocId
                                        End If
                                    End If

                                    If ToSafeLong(sTmpFile) <> 0 Then
                                        'Merge if a clause doc available for effective date
                                        For iClauseCnt = LBound(vClauseList, 2) To UBound(vClauseList, 2)
                                            If CStr(vClauseList(0, iClauseCnt)) = sTmpFile Then
                                                ' We have a request for a clause Code or Description
                                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) _
                                                    & Trim(vClauseList(iItem, iClauseCnt)) _
                                                    & Mid$(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)
                                                Exit For
                                            End If
                                        Next iClauseCnt
                                    Else
                                        sCurrentLine = ""
                                    End If
                                End If
                            End If

                        Case FileTag

                            ' Resolve the sub-document in a new window
                            m_lReturn = ResolveDocumentXML(sFieldCode)

                            m_lReturn = InsertSubDocXML(sFieldCode, oTextStreamOut, cInputFile)

                        Case QuestionTag

                            sQuestion = Right$(sFieldCode, Len(sFieldCode) - iSep)

                            ' SET 04/03/2004 - only ask questions if we were initialised
                            ' using Initialise method (ie running on a client machine) as opposed to
                            ' using the InitialiseBusiness method (ie running on a server machine)
                            sAnswer = ""
                            If (m_bRunningOnServer = False) Then
                                sQuestion = Right$(sFieldCode, Len(sFieldCode) - iSep)

                                If Not (bQuestionAlreadyAsked(sQuestion)) Then

                                    'Saj240224
                                    ' sAnswer = InputBox(sQuestion, "Document Manager")
                                    If m_colQuestions.Count = 0 Then
                                        m_colQuestions.Add(Nothing)
                                    End If
                                    m_colQuestions.Add(sQuestion) ', sAnswer REMOVED MKW130303 PN2950 Error occured in Same Answer Given Twice
                                    m_colAnswers.Add(sAnswer, sQuestion)

                                Else
                                    'CQ6053 DEBUG
                                    If m_colAnswers Is Nothing Then
                                        'CQ6053 DEBUG
                                        Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=
                                            "6. m_colAnswers is not instantiated| filename = " & sFileName & "| loop depth = " & CStr(iLoopDepth),
                                        vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)
                                        ResolveDocumentXML = gPMConstants.PMEReturnCode.PMTrue
                                        Exit Function
                                    End If

                                    sAnswer = m_colAnswers.Item(sQuestion)
                                End If
                            End If

                            ' PW291003 - CQ2963 - Get rid of any < and > characters
                            If InStr(sAnswer, "<") <> 0 Then
                                sAnswer = Replace(sAnswer, "<", "&lt;")
                            End If
                            If InStr(sAnswer, ">") <> 0 Then
                                sAnswer = Replace(sAnswer, ">", "&gt;")
                            End If
                            ' PW110603 - Put quotes round if this is a nested
                            ' merge field: start
                            If iCurrentNestingLevel > 1 Then
                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) +
                                Chr(147) + sAnswer + Chr(148) +
                            Mid$(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)
                            Else
                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) +
                                sAnswer +
                            Mid$(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)
                            End If

                        'Thinh Nguyen 10/10/2002 start - mandatory question
                        Case MandQuestionTag

                            sQuestion = Right$(sFieldCode, Len(sFieldCode) - iSep)

                            ' SET 04/03/2004 - only ask questions if we were initialised
                            ' using Initialise method (ie running on a client machine) as opposed to
                            ' using the InitialiseBusiness method (ie running on a server machine)
                            sAnswer = ""
                            If (m_bRunningOnServer = False) Then
                                sQuestion = Right$(sFieldCode, Len(sFieldCode) - iSep)

                                If Not (bQuestionAlreadyAsked(sQuestion)) Then
                                    Do While sAnswer = ""
                                        ' sAnswer = InputBox(sQuestion, "Document Manager")
                                    Loop

                                    m_colQuestions.Add(sQuestion) ', sAnswer REMOVED MKW130303 PN2950 Error occured in Same Answer Given Twice
                                    m_colAnswers.Add(sAnswer, sQuestion)

                                Else
                                    'CQ6053 DEBUG
                                    If m_colAnswers Is Nothing Then
                                        'CQ6053 DEBUG
                                        Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=
                                            "8. m_colAnswers is not instantiated| filename = " & sFileName & "| loop depth = " & CStr(iLoopDepth),
                                        vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)
                                        ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                        Exit Function
                                    End If
                                    sAnswer = m_colAnswers.Item(sQuestion)
                                End If
                            End If

                            ' PW291003 - CQ2963 - Get rid of any < and > characters
                            If InStr(sAnswer, "<") <> 0 Then
                                sAnswer = Replace(sAnswer, "<", "&lt;")
                            End If
                            If InStr(sAnswer, ">") <> 0 Then
                                sAnswer = Replace(sAnswer, ">", "&gt;")
                            End If
                            ' PW110603 Put quotes round if this is a nested merge field
                            If iCurrentNestingLevel > 1 Then
                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) +
                                Chr(147) + sAnswer + Chr(148) +
                            Mid$(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)
                            Else
                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) &
                            sAnswer & Mid$(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)
                            End If

                        Case LoopTag

                            ' extract the loop name
                            sFieldName = Trim(Right(sFieldCode, Len(sFieldCode) - iSep))

                            'DJM 29/08/2002 : Blank the keyarray
                            vKeyArray = Nothing

                            'CQ6053 DEBUG
                            If m_oBusiness Is Nothing Then
                                'CQ6053 DEBUG
                                Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=
                                    "9. m_oBusiness is not instantiated| filename = " & sFileName & "| loop depth = " & CStr(iLoopDepth),
                                vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)
                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If

                            '' ''Check in dictionary before calling GetKeyExists
                            If (m_oGetKeys.ContainsKey(sFieldName.ToLower)) Then
                                m_oGetKeys.TryGetValue(sFieldName.ToLower, m_lReturn)
                            Else
                                m_lReturn = m_oBusiness.GetKeysExists(sFieldName)
                                m_oGetKeys.Add(sFieldName.ToLower, m_lReturn)
                            End If

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                If m_lRiskCnt = 0 And Not bInRiskLoop Then
                                    m_lReturn = m_oBusiness.GetActualRiskForLoopPolicy(m_lInsuranceFileCnt, sFieldName.Substring(2), vRiskId)
                                End If
                                'RWH(16/01/2001) Check to see if we need to retrieve Parent Key.
                                'array not initialised or first level deep
                                If (Not IsArray(vInstanceArray)) Or iLoopDepth = 1 Then

                                    'Retrieve Parent key.
                                    m_lReturn = m_oBusiness.GetParentKey(r_lParentKey:=lParentKey,
                                                                         v_sChildTable:=sFieldName,
                                                                         v_lPartyCnt:=m_lPartyCnt,
                                                                         v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                         v_lClaimCnt:=m_lClaimCnt,
                                                                         v_sDocumentRef:=m_sDocumentRef,
                                                                     v_vRiskId:=vRiskId)

                                    ReDim vInstanceArray(0)
                                    vInstanceArray(0) = lParentKey
                                End If

                                'RWH(16/01/2001) GetLoopKeys.

                                'Did we find the Parent key.
                                If (Val(vInstanceArray(UBound(vInstanceArray))) <> 0) Then
                                    m_lReturn = m_oBusiness.GetLoopKeys(r_vKeyArray:=vKeyArray,
                                                                    v_sTableName:=sFieldName,
                                                                    v_lPartyCnt:=m_lPartyCnt,
                                                                    v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                    v_lClaimCnt:=m_lClaimCnt,
                                                                    v_sDocumentRef:=m_sDocumentRef,
                                                                    v_vInstanceArray:=vInstanceArray,
                                                                v_vRiskId:=vRiskId)

                                End If

                            Else

                                'DJM 22/05/2002 - Get count of Loop Lines
                                m_lReturn = m_oBusiness.GetLoopCount(lLoopCount,
                                                            sFieldName,
                                                            m_lPartyCnt,
                                                            m_lInsuranceFileCnt,
                                                            m_lClaimCnt,
                                                            m_sDocumentRef,
                                                        ToSafeLong(vRiskId))

                                'DJM 02/04/2002 - Fill vKeyArray with incrementing numbers.
                                If lLoopCount <> 0 And m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                                    ReDim vKeyArray(0 To 0, 0 To lLoopCount - 1)

                                    For lLoop = 0 To lLoopCount - 1
                                        vKeyArray(0, lLoop) = lLoop + 1
                                    Next
                                End If

                                ReDim vInstanceArray(0)

                            End If

                            sStartingPartOfSplitLine = ""
                            sEndingPartOfSplitLine = ""
                            sLoopFullLine = ""

                            bEndLoopFoundInSameLine = False

                            If InStr(1, sCurrentLine, m_sFieldStartMarker & EndLoopTag & Separator & sFieldName & m_sFieldEndMarker) > 0 Then
                                bEndLoopFoundInSameLine = True
                            End If

                            sLoopLine = ""

                            If Not bEndLoopFoundInSameLine Then
                                Do While True
                                    If cInputFile.EOF Then ' PN25924
                                        Exit Function
                                    End If

                                    'Read in next line of parent
                                    m_lReturn = ResolveReadFullLine(
                                                            cInputFile:=cInputFile,
                                                        r_sCurrentLine:=sLoopLine)

                                    If InStr(1, sLoopLine, m_sFieldStartMarker & EndLoopTag & Separator & sFieldName & m_sFieldEndMarker) > 0 Then
                                        sCurrentLine = sCurrentLine & sLoopLine
                                        Exit Do
                                    Else
                                        sCurrentLine = sCurrentLine & sLoopLine
                                    End If
                                Loop
                            End If

                            sStartingPartOfSplitLine = Left$(sCurrentLine, lFieldStart - 1)

                            sLoopFullLine = Mid$(sCurrentLine, lFieldStart + (m_iFieldMarkerLength * 2) + (lFieldEnd - 1))

                            lPos = InStr(1, sLoopFullLine, m_sFieldStartMarker & EndLoopTag & Separator & sFieldName & m_sFieldEndMarker)

                            If lPos > 0 Then
                                sEndingPartOfSplitLine = Mid$(sLoopFullLine, lPos + Len(m_sFieldStartMarker & EndLoopTag & Separator & sFieldName & m_sFieldEndMarker))
                                sLoopFullLine = Left$(sLoopFullLine, lPos - 1)
                            End If

                            If (IsArray(vKeyArray)) Then
                                sTempLineFragment = sStartingPartOfSplitLine
                                sTempLineFragment1 = sEndingPartOfSplitLine
                                sTempLoopLine = sLoopFullLine

                                m_lReturn = RemoveContentsOfTag("<w:t>", "</w:t>", sTempLineFragment, True)
                                m_lReturn = RemoveContentsOfTag("<w:t>", "</w:t>", sTempLineFragment1, True)
                                m_lReturn = RemoveContentsOfTag("<w:t>", "</w:t>", sTempLoopLine, True)

                                'Make loop full line
                                sLoopFullLine = sTempLineFragment & sLoopFullLine & sTempLineFragment1

                                If Not IsEmptyString(sStartingPartOfSplitLine) Then
                                    'Just extract the contents of line
                                    sStartingPartOfSplitLine = GetParagarphText(sStartingPartOfSplitLine, True)
                                Else
                                    sStartingPartOfSplitLine = ""
                                End If

                                sCurrentLine = ""

                                If Not IsEmptyString(sEndingPartOfSplitLine) Then
                                    'make full line of ending part
                                    'check if ending fragement has any start or end marker
                                    If InStr(1, sEndingPartOfSplitLine, m_sFieldStartMarker) > 0 Then
                                        bIsNestedloop = True
                                        sEndingPartOfSplitLine = sTempLineFragment & sTempLoopLine & sEndingPartOfSplitLine
                                        sCurrentLine = sEndingPartOfSplitLine
                                        sEndingPartOfSplitLine = ""
                                    Else
                                        sEndingPartOfSplitLine = GetParagarphText(sEndingPartOfSplitLine, True)
                                        bIsNestedloop = False
                                    End If
                                Else
                                    sEndingPartOfSplitLine = ""
                                    bIsNestedloop = False
                                End If

                                m_lReturn = RemoveEmptyParagraphsXML(sLoopFullLine, True, True)

                                'If (IsArray(vKeyArray)) Then

                                sLoopFile = CStr(UBound(vInstanceArray)) _
                                        & sLOOP_FILE

                                sLoopFileTemp = CStr(UBound(vInstanceArray)) _
                                        & sLOOP_FILE_TEMP

                                sTmpFile = m_sClient & "\" _
                                        & sLoopFile


                                oFileNumLoop = oFile.CreateTextFile(sTmpFile, True, True)

                                'Open Temp output file to copy loop section into.
                                bTmpFileIsOpen = True

                                m_lReturn = RemoveInvalidCharacters(sDocPreBodyFragment)
                                oFileNumLoop.Write(sDocPreBodyFragment)

                                m_lReturn = RemoveInvalidCharacters(sLoopFullLine)
                                oFileNumLoop.Write(sLoopFullLine & g_sDocEndBodyFragment)


                                oFileNumLoop.Close()
                                bTmpFileIsOpen = False

                                'Copy loop template to separate location so it is not lost when the first
                                'loop merge is done.
                                ' Use CopyFile rather than PMFileCopy
                                m_sFileCopyMsg = ""

                                '    m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(m_sClient & "\" & sLoopFile, m_sClient & "\" & sLoopFile)

                                m_lReturn = bPMDocFunctions.CopyFile(m_sClient & "\" & sLoopFile, m_sClient & "\" & sLoopFileTemp, True, False, m_sFileCopyMsg)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    ResolveDocumentXML = m_lReturn
                                    bPMFunc.LogMessage(
                                        sUsername:=m_sUsername,
                                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                        sMsg:="Failed to copy file." & vbCrLf &
                                              "Source File      : " & m_sClient & "\" & sLoopFile & vbCrLf &
                                              "Destination File : " & m_sClient & "\" & sLoopFileTemp & vbCrLf &
                                              "Error Details    : " & m_sFileCopyMsg,
                                        vApp:=ACApp,
                                        vClass:=ACClass,
                                        vMethod:="ResolveDocumentXML",
                                        vErrNo:=Err.Number,
                                        vErrDesc:=Err.Description)
                                    Exit Function
                                End If

                                ReDim Preserve vInstanceArray(UBound(vInstanceArray) + 1)
                                Dim bEmptyDoc As Boolean = True
                                Dim bLoopPrefixMerged As Boolean = False
                                Dim bLoopSuffixMerged As Boolean = False

                                If m_oBusiness Is Nothing Then
                                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness is not instantiated")
                                    Return PMEReturnCode.PMFalse
                                End If
                                For lLoop = 0 To UBound(vKeyArray, 2)

                                    'Check in dictionary before calling GetKeyExists
                                    If (m_oGetKeys.ContainsKey(sFieldName.ToLower)) Then
                                        m_oGetKeys.TryGetValue(sFieldName.ToLower, m_lReturn)
                                    Else
                                        m_lReturn = m_oBusiness.GetKeysExists(sFieldName)
                                        m_oGetKeys.Add(sFieldName.ToLower, m_lReturn)
                                    End If

                                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                        vInstanceArray(UBound(vInstanceArray)) = vKeyArray(0, lLoop)
                                    Else
                                        'Fill field 0 for non-DataModel
                                        vInstanceArray(0) = vKeyArray(0, lLoop)
                                    End If

                                    sTmpStartLine = ""
                                    sTmpEndLine = ""

                                    If Not bLoopPrefixMerged Then
                                        sTmpStartLine = sStartingPartOfSplitLine
                                    End If

                                    If lLoop = UBound(vKeyArray, 2) Then
                                        'End line can be merged only if the same has no unmerged tag
                                        If InStr(1, sEndingPartOfSplitLine, m_sFieldStartMarker) = 0 Then
                                            sTmpEndLine = sEndingPartOfSplitLine
                                        Else
                                            sCurrentLine = sEndingPartOfSplitLine
                                        End If
                                    End If

                                    m_lReturn = ProcessSubDocXml(sSourceTemplate:=sLoopFileTemp,
                                                          sDestinationMerge:=sLoopFile,
                                                          oTextStreamOut:=oTextStreamOut,
                                                          cInputFile:=cInputFile,
                                                          vRiskId:=vRiskId,
                                                          vInstanceArray:=vInstanceArray,
                                                          v_sContentMergedWithFirstLine:=sTmpStartLine,
                                                          v_sContentMergedWithLastLine:=sTmpEndLine,
                                                          v_sCurrentLine:=sCurrentLine,
                                                              bInRiskLoop:=bInRiskLoop, sDocType:=sDocType, sDocName:=sDocName, sRiskDescription:=sRiskDescription,
                                                          IsEmptyDoc:=bEmptyDoc)

                                    If Not bEmptyDoc Then
                                        bLoopPrefixMerged = True
                                    End If

                                    If lLoop = UBound(vKeyArray, 2) Then
                                        If bEmptyDoc Then
                                            If sTmpStartLine <> "" Or sTmpEndLine <> "" Then
                                                m_lReturn = RemoveContentsOfTag("<w:t>", "</w:t>", sLoopFullLine)

                                                If sTmpStartLine <> "" Then
                                                    m_lReturn = MergeContentsAtStartingOfDocuemnt(sTmpStartLine, sLoopFullLine)
                                                End If

                                                If sTmpEndLine <> "" Then
                                                    m_lReturn = MergeContentsAtEndOfDocuemnt(sTmpEndLine, sLoopFullLine)
                                                End If

                                                sCurrentLine = sLoopFullLine
                                            End If
                                        End If
                                    End If

                                Next lLoop

                                'Thinh Nguyen 18/06/2003 (start) - only redim if UBound(vInstanceArray) > 0
                                If UBound(vInstanceArray) > 0 Then

                                    'Knock bottom level off array so next time round we use next
                                    'parent key.
                                    ReDim Preserve vInstanceArray(UBound(vInstanceArray) - 1)
                                End If
                                'Thinh Nguyen 18/06/2003 (end) - only redim if UBound(vInstanceArray) > 0

                                ' RAM20040301 : Use FSO to delete the file, rather than Kill
                                m_lReturn = DOCGeneralFunc.DeleteFile(m_sClient & "\" & sLoopFileTemp)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                    ' Log Error.
                                    bPMFunc.LogMessage(
                                        sUsername:=m_sUsername,
                                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                        sMsg:="Failed to delete file [" & m_sClient & "\" & sLoopFileTemp & "]",
                                        vApp:=ACApp,
                                        vClass:=ACClass,
                                        vMethod:="ResolveDocumentXML",
                                        vErrNo:=Err.Number,
                                    vErrDesc:=Err.Description)
                                    Exit Function
                                End If

                                bSuppressOriginalLine = False

                            Else

                                'If there is no sub-loop data on the database we need to
                                'remove all the redundant stuff from the document.

                                sCurrentLine = sStartingPartOfSplitLine & sEndingPartOfSplitLine

                                m_lReturn = RemoveEmptyParagraphsXML(sCurrentLine, True, False)

                                sNextLine = ""

                            End If  'IsArray(vKeyArray)

                            ' PW190204 - CQ4645 - restore instance array to what it was
                            ' at the beginning, for the next occurrence of a loop
                            vInstanceArray = vHoldInstanceArray

                        Case RiskHeaderTag

                            ' extract the field name
                            sFieldName = Right$(sFieldCode, Len(sFieldCode) - iSep)

                            If (Trim$(sFieldName) = "Y") Then
                                bShowHeaders = True
                            Else
                                bShowHeaders = False
                            End If
                            bSuppressOriginalLine = True
                            '5632 Sumit.K Here start line was missing. So need to append start line with end line to make a correct single line.
                            sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) & Mid$(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)
                            m_lReturn = RemoveEmptyParagraphsXML(sCurrentLine)

                        Case RiskLoopTag
                            ' extract the template code
                            sFieldName = Right$(sFieldCode, Len(sFieldCode) - iSep)


                            'Extract Risk Type from Field Name.
                            sRiskType = Left$(sFieldName, 1)
                            sFieldName = Mid$(sFieldName, 2)

                            vRiskArray = "" 'make sure variable is empty before we fill it up

                            'CQ6053 DEBUG
                            If m_oBusiness Is Nothing Then
                                'CQ6053 DEBUG
                                Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=
                                    "12. m_oBusiness is not instantiated| filename = " & sFileName & "| loop depth = " & CStr(iLoopDepth),
                                vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)
                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If

                            'Retrieve list of risks associated with the current policy.
                            m_lReturn = m_oBusiness.GetRisksForPolicy(m_lInsuranceFileCnt, sRiskType, vRiskArray)
                            sStartingPartOfSplitLine = ""
                            sEndingPartOfSplitLine = ""

                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                If (m_bRunningOnServer = False) Then

                                    ' MsgBox("Error getting risks for current policy.", vbExclamation, "Document Manager")
                                Else
                                    Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                        sMsg:="Error getting risks for current policy",
                                    vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)

                                End If
                            Else
                                sTemp = ""
                                If (IsArray(vRiskArray)) Then
                                    '
                                    sStartingPartOfSplitLine = Left$(sCurrentLine, lFieldStart - 1)

                                    sEndingPartOfSplitLine = Mid(sCurrentLine, lFieldStart + lFieldEnd +
                                    (m_iFieldMarkerLength * 2) - 1)

                                    m_lReturn = GetFullLines(sStartingPartOfSplitLine, sEndingPartOfSplitLine)

                                    m_lReturn = RemoveEmptyParagraphsXML(sStartingPartOfSplitLine, True)

                                    m_lReturn = RemoveInvalidCharacters(sStartingPartOfSplitLine)

                                    oTextStreamOut.Write(sStartingPartOfSplitLine)

                                    lPreviousRiskType = 0

                                    For iRiskCount = LBound(vRiskArray, 2) To UBound(vRiskArray, 2)
                                        If (m_lClaimCnt = 0) Or (m_lClaimCnt <> 0 And vRiskArray(iRISK_ID_IDX, iRiskCount) = lRiskId) Then
                                            bValidRisk = True

                                            If m_lRiskCnt > 0 Then
                                                If m_lRiskCnt <> ToSafeLong(vRiskArray(iRISK_ID_IDX, iRiskCount)) Then
                                                    bValidRisk = False
                                                End If
                                            End If

                                            If bValidRisk Then

                                                If (lPreviousRiskType <> vRiskArray(iRISK_TYPE_IDX, iRiskCount)) Then
                                                    If (bShowHeaders = True) Then
                                                        If (iRiskCount <> 0) Then
                                                            '***********************************************
                                                            '************* Insert trailer for last Risk Type
                                                            '***********************************************
                                                            Dim auxVar As Object = vRiskArray(iTRAILER_IDX, iRiskCount - 1)

                                                            If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                                                                If (vRiskArray(iTRAILER_IDX, iRiskCount - 1) <> 0) Then
                                                                    'make sure we have a unique folder for each document

                                                                    sParentDocumentWorkDirectoryRiskLoop = m_sClient
                                                                    m_sClient = ""
                                                                    m_lReturn = GetClientFolder()

                                                                    m_lReturn = CopyServerToClient(lCLAUSE_TYPE_ID, vRiskArray(iTRAILER_IDX, iRiskCount - 1))
                                                                    If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                                                                        sTmpFile = "Doc " & CStr(vRiskArray(iTRAILER_IDX, iRiskCount - 1)) & "." & m_sDocFileExtension

                                                                        'Get doc from server, resolve it and insert it into the main doc.
                                                                        'JJ 27/08/2003 4332 Added indicator for RiskLoop
                                                                        m_lReturn = ProcessSubDocXml(sTmpFile, "T" & sRISK_FILE_TEMP, oTextStreamOut, cInputFile, bInRiskLoop:=True, sDocType:=sDocType, sDocName:=sDocName, sRiskDescription:=sRiskDescription)

                                                                        ' RAM20040301 : Use FSO to delete the file, rather than Kill
                                                                        If m_bUniqueClientDirNeedsDeleting Then
                                                                            m_lReturn = DelDirectory(m_sClient)
                                                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                                                                ' Log Error.
                                                                                bPMFunc.LogMessage(
                                                                                        sUsername:=m_sUsername,
                                                                                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                                        sMsg:="Failed to delete Folder [" & m_sClient & "]" & vbCrLf & "while resolving Clauses : " & sFieldCode,
                                                                                        vApp:=ACApp,
                                                                                        vClass:=ACClass,
                                                                                        vMethod:="ResolveDocumentXML",
                                                                                        vErrNo:=Err.Number,
                                                                                    vErrDesc:=Err.Description)
                                                                                Exit Function
                                                                            End If
                                                                        End If

                                                                        ' Restore the Original ParentWorkDirectory to current work directory
                                                                        m_sClient = sParentDocumentWorkDirectoryRiskLoop


                                                                    Else
                                                                        If (m_bRunningOnServer = False) Then
                                                                            Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                                        sMsg:="Could not locate Risk Trailer clause, continuing....",
                                                                                    vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)

                                                                        Else
                                                                            Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                                            sMsg:="Could not locate Risk Trailer clause, continuing....",
                                                                                        vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If

                                                        '*********************************************
                                                        '*********** Now do header for next Risk Type.
                                                        '*********************************************
                                                        Dim auxVar_2 As Object = vRiskArray(iHEADER_IDX, iRiskCount)


                                                        If Not (Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2)) Then
                                                            If ToSafeInteger((vRiskArray(iHEADER_IDX, iRiskCount))) <> 0 Then
                                                                'make sure we have a unique folder for each document
                                                                sParentDocumentWorkDirectoryRiskLoop = m_sClient
                                                                m_sClient = ""
                                                                m_lReturn = GetClientFolder()

                                                                m_lReturn = CopyServerToClient(lCLAUSE_TYPE_ID, ToSafeLong(vRiskArray(iHEADER_IDX, iRiskCount), 0))
                                                                If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                                                                    sTmpFile = "Doc " & CStr(vRiskArray(iHEADER_IDX, iRiskCount)) & "." & m_sDocFileExtension

                                                                    'Get doc from server, resolve it and insert it into the main doc.
                                                                    'JJ 27/08/2003 4332 Added indicator for RiskLoop
                                                                    m_lReturn = ProcessSubDocXml(sTmpFile, "H" & sRISK_FILE_TEMP, oTextStreamOut, cInputFile, bInRiskLoop:=True, sDocType:=sDocType, sDocName:=sDocName, sRiskDescription:=sRiskDescription)

                                                                    ' RAM20040301 : Use FSO to delete the file, rather than Kill
                                                                    If m_bUniqueClientDirNeedsDeleting Then
                                                                        m_lReturn = DelDirectory(m_sClient)
                                                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                            ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                                                            ' Log Error.
                                                                            bPMFunc.LogMessage(
                                                                                    sUsername:=m_sUsername,
                                                                                    iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                                    sMsg:="Failed to delete Folder [" & m_sClient & "]" & vbCrLf & "while resolving Clauses : " & sFieldCode,
                                                                                    vApp:=ACApp,
                                                                                    vClass:=ACClass,
                                                                                    vMethod:="ResolveDocumentXML",
                                                                                    vErrNo:=Err.Number,
                                                                                vErrDesc:=Err.Description)
                                                                            Exit Function
                                                                        End If
                                                                    End If

                                                                    ' Restore the Original ParentWorkDirectory to current work directory
                                                                    m_sClient = sParentDocumentWorkDirectoryRiskLoop

                                                                Else
                                                                    If (m_bRunningOnServer = False) Then
                                                                        MsgBox("Could not locate Risk Header clause, continuing....", vbExclamation, "Merge Document")
                                                                    Else
                                                                        Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                                        sMsg:="Could not locate Risk Header clause, continuing....",
                                                                                    vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)

                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If

                                                    '************************************************
                                                    '********** Now establish the actual Risk clause.
                                                    '************************************************

                                                    'Set print pointer to zero if not set
                                                    If vRiskArray(iRPT_POINTER_IDX, iRiskCount) = "" Then
                                                        vRiskArray(iRPT_POINTER_IDX, iRiskCount) = 0
                                                    End If

                                                    'Get type and id of template.
                                                    m_lReturn = GetRiskTemplate(
                                                                    sRiskCode:=sFieldName,
                                                                    iRptPointer:=vRiskArray(iRPT_POINTER_IDX, iRiskCount),
                                                                    lDocId:=lTempDocId,
                                                                lDocType:=lTempDocType)

                                                    If (lTempDocId <> 0) Then

                                                        sParentDocumentWorkDirectoryRiskLoop = m_sClient

                                                        'make sure we have a unique folder for each document
                                                        m_sClient = ""
                                                        m_lReturn = GetClientFolder()

                                                        If (lPreviousRiskDocId <> 0) Then
                                                            'Delete client copy of Risk Template.
                                                            m_lReturn = DeleteClient(m_sClient, "Doc " & lPreviousRiskDocId & "." & m_sDocFileExtension)
                                                        End If
                                                        lPreviousRiskDocId = lTempDocId
                                                        m_lReturn = CopyServerToClient(lTempDocType, lTempDocId)

                                                        sTmpFile = "Doc " & CStr(lTempDocId) & "." & m_sDocFileExtension
                                                    Else
                                                        If (m_bRunningOnServer = False) Then
                                                            'MsgBox("RK" & sFieldName & " Risk Template not found.", vbExclamation, "Merge Document")
                                                            'Saj240224
                                                            Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                sMsg:="RK" & sFieldName & " Risk Template not found.",
                                                            vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)
                                                        Else
                                                            Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                sMsg:="RK" & sFieldName & " Risk Template not found.",
                                                            vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)
                                                        End If

                                                        Throw New Exception

                                                    End If
                                                End If

                                                '********** Process Risk clause
                                                'Get doc from server, resolve it and insert it into the main doc.
                                                'JJ 27/08/2003 4332 Added indicator for RiskLoop
                                                'JJ 08/09/2003 Don't use a standard name for the risk_temp.txt file
                                                '
                                                m_lReturn = ProcessSubDocXml(sTmpFile,
                                                                                sFieldName & ".xml",
                                                                                oTextStreamOut,
                                                                                cInputFile,
                                                                            vRiskArray(iRISK_ID_IDX, iRiskCount), sRiskDescription:=vRiskArray(iRISK_DESC_IDX, iRiskCount), bInRiskLoop:=True, sDocType:=sDocType, sDocName:=sDocName, vRiskNumber:=iRiskCount)

                                                ' RAM20040301 : Use FSO to delete the file, rather than Kill
                                                If m_bUniqueClientDirNeedsDeleting Then
                                                    m_lReturn = DelDirectory(m_sClient)
                                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                        ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                                        ' Log Error.
                                                        bPMFunc.LogMessage(
                                                                sUsername:=m_sUsername,
                                                                iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                sMsg:="Failed to delete Folder [" & m_sClient & "]" & vbCrLf & "while resolving Clauses : " & sFieldCode,
                                                                vApp:=ACApp,
                                                                vClass:=ACClass,
                                                                vMethod:="ResolveDocumentXML",
                                                                vErrNo:=Err.Number,
                                                            vErrDesc:=Err.Description)
                                                        Exit Function
                                                    End If
                                                End If

                                                ' Restore the Original ParentWorkDirectory to current work directory
                                                m_sClient = sParentDocumentWorkDirectoryRiskLoop
                                            End If
                                        End If
                                    Next iRiskCount

                                    m_lReturn = DeleteClient(m_sClient, sTmpFile)

                                    lPreviousRiskDocId = 0
                                End If

                                m_lReturn = RemoveEmptyParagraphsXML(sEndingPartOfSplitLine, True)

                            End If

                            sCurrentLine = sEndingPartOfSplitLine
                            bSuppressOriginalLine = True

                        Case StandardWordingsTag, StandardWordingNPTag, StandardWordingsCodeTag, StandardWordingsDescTag
                            'RWH(23/04/2001) Standard wordings.
                            nFirstInstance = nFirstInstance + 1
                            Dim sStandardWordingTag As String
                            Dim sOriginalSWFormatLine As String

                            ' extract the field name
                            sTmpFile = Right$(sFieldCode, Len(sFieldCode) - iSep)
                            sStandardWordingTag = ""

                            If sTmpFile = StandardWordingsTag Or sTmpFile = StandardWordingNPTag Then
                                lSelectedWordingCode = -1
                            ElseIf sTmpFile = StandardWordingsCodeTag Then
                                sStandardWordingTag = "CODE"
                            ElseIf sTmpFile = StandardWordingsDescTag Then
                                sStandardWordingTag = "DESC"
                            Else
                                ' we have a selective standard wording clause
                                lSelectedWordingCode = Val(Right$(sTmpFile, Len(sTmpFile) - InStr(1, sTmpFile, Separator)))
                            End If

                            'Ensure we print remainder of line preceding std wrdings field.
                            'Tomo181002
                            'And let's not forget what's after it either...
                            sTemp = Mid$(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)

                            sCurrentLine = Left$(sCurrentLine, lFieldStart - 1)
                            'Print #iFileNumOut, sCurrentLine
                            oTextStreamOut.Write(sCurrentLine)

                            'Tomo181002
                            '                    sCurrentLine = ""
                            '                    bSuppressOriginalLine = True
                            sOriginalSWFormatLine = sCurrentLine
                            sCurrentLine = sTemp
                            bSuppressOriginalLine = False


                            'CQ6053 DEBUG
                            If m_oBusiness Is Nothing Then
                                Call bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=
                                    "11. m_oBusiness is not instantiated| filename = " & sFileName & "| loop depth = " & CStr(iLoopDepth),
                                vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME)
                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If
                            'Get the standard wordings for this document/sub-document.
                            'JJ 26/08/2003 4332 get standard wording for policy or risk

                            If Len(sStandardWordingProperty) > 0 Then

                                'Standard wording at risk level
                                If Not Information.IsNothing(vInstanceArray) Then
                                    m_lReturn = m_oBusiness.GetStandardWordings(
                                        r_vStandardWordingsArray:=vStandardWordingsArray,
                                        vInsFileCnt:=m_lInsuranceFileCnt,
                                        vRiskCnt:=vRiskId,
                                        vStandardWordingProperty:=sStandardWordingProperty,
                                    vChildID:=vInstanceArray(UBound(vInstanceArray)))
                                Else
                                    m_lReturn = m_oBusiness.GetStandardWordings(
                                        r_vStandardWordingsArray:=vStandardWordingsArray,
                                        vInsFileCnt:=m_lInsuranceFileCnt,
                                        vRiskCnt:=vRiskId,
                                    vStandardWordingProperty:=sStandardWordingProperty)
                                End If
                            Else
                                If bInRiskLoop Then
                                    'Standard wording at risk level
                                    m_lReturn = m_oBusiness.GetStandardWordings(
                                        r_vStandardWordingsArray:=vStandardWordingsArray,
                                        vInsFileCnt:=m_lInsuranceFileCnt,
                                    vRiskCnt:=vRiskId)
                                Else
                                    'Standard wording at policy level
                                    If sStandardWordingTag = "CODE" Then
                                        ''GetStandardWordings Code sorting
                                        m_lReturn = m_oBusiness.GetStandardWordings(
                                        r_vStandardWordingsArray:=vStandardWordingsArray,
                                        sStandardWordingTag:="CODE",
                                    vInsFileCnt:=m_lInsuranceFileCnt)
                                    ElseIf sStandardWordingTag = "DESC" Then
                                        ''GetStandardWordings Desc sorting
                                        m_lReturn = m_oBusiness.GetStandardWordings(
                                        r_vStandardWordingsArray:=vStandardWordingsArray,
                                        sStandardWordingTag:="DESC",
                                        vInsFileCnt:=m_lInsuranceFileCnt)
                                    Else
                                        m_lReturn = m_oBusiness.GetStandardWordings(
                                        r_vStandardWordingsArray:=vStandardWordingsArray,
                                        vInsFileCnt:=m_lInsuranceFileCnt)
                                    End If
                                End If
                            End If
                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                bPMFunc.LogMessage(
                                    sUsername:=m_sUsername,
                                    iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                    sMsg:="Failed to get Standard Wordings",
                                    vApp:=ACApp,
                                    vClass:=ACClass,
                                    vMethod:="ResolveDocumentXML",
                                    vErrNo:=Err.Number,
                                vErrDesc:=Err.Description)
                                'But carry on with document
                            Else
                                If (IsArray(vStandardWordingsArray)) Then

                                    For iStdWordingCnt = LBound(vStandardWordingsArray, 2) To UBound(vStandardWordingsArray, 2)

                                        ' Get temp file name.
                                        lStandardWordingDocumentTemplateID = ToSafeLong(vStandardWordingsArray(ACStdWrdsPosDocId, iStdWordingCnt), 0)
                                        lStandardWordingDocumentTemplateTypeID = ToSafeLong(vStandardWordingsArray(ACStdWrdsPosDocTypeId, iStdWordingCnt), 0)

                                        If sStandardWordingTag = "CODE" Then
                                            'PN57191
                                            If nFirstInstance = 1 Then
                                                sCurrentLine = sOriginalSWFormatLine & sCurrentLine
                                            Else
                                                If iStdWordingCnt > 0 Then
                                                    sCurrentLine = sOriginalSWFormatLine & sCurrentLine
                                                End If
                                            End If

                                            ' Print #iFileNumOut, sCurrentLine
                                            sCurrentLine = vStandardWordingsArray(ACStdWrdsPosCode, iStdWordingCnt) & sTemp

                                            sCurrentLine = sCurrentLine.Replace("</w:t></w:r></w:p>", "") _
                                                            .Replace("&lt;@STANDARDWORDINGS_DESC@&gt;", "") _
                                                            .Replace("&lt;@STANDARDWORDINGS@&gt;", "") _
                                                            .Replace("&lt;@STANDARDWORDINGS_CODE@&gt;", "")

                                            sCurrentLine = sCurrentLine & "<w:br/>"

                                            oTextStreamOut.Write(sCurrentLine)
                                            If iStdWordingCnt <> UBound(vStandardWordingsArray, 2) Then
                                                sCurrentLine = ""
                                            Else
                                                sCurrentLine = sTemp
                                            End If
                                        ElseIf sStandardWordingTag = "DESC" Then
                                            'PN57191
                                            If nFirstInstance = 1 Then
                                                sCurrentLine = sOriginalSWFormatLine & sCurrentLine
                                            Else
                                                If iStdWordingCnt > 0 Then
                                                    sCurrentLine = sOriginalSWFormatLine & sCurrentLine
                                                End If
                                            End If

                                            sCurrentLine = vStandardWordingsArray(ACStdWrdsPosDescription, iStdWordingCnt) & sTemp
                                            sCurrentLine = sCurrentLine.Replace("</w:t></w:r></w:p>", "") _
                                                            .Replace("&lt;@STANDARDWORDINGS_DESC@&gt;", "") _
                                                            .Replace("&lt;@STANDARDWORDINGS@&gt;", "") _
                                                            .Replace("&lt;@STANDARDWORDINGS_CODE@&gt;", "")

                                            sCurrentLine = sCurrentLine & "<w:br/>"
                                            oTextStreamOut.Write(sCurrentLine)
                                            'Print #iFileNumOut, sCurrentLine
                                            If iStdWordingCnt <> UBound(vStandardWordingsArray, 2) Then
                                                sCurrentLine = ""
                                            Else
                                                sCurrentLine = sTemp
                                            End If
                                        ElseIf lSelectedWordingCode = -1 Or lSelectedWordingCode = lStandardWordingDocumentTemplateID Then

                                            'insert page break if required
                                            If sFieldType = StandardWordingNPTag Then
                                                sTemp = "<w:br w:type=""page""/>"
                                                oTextStreamOut.Write(sTemp)
                                            End If

                                            'RKS 051004 PN15089
                                            'make sure we have a unique folder for each document
                                            sParentDocumentWorkDirectoryStandardWording = m_sClient
                                            m_sClient = ""
                                            m_lReturn = GetClientFolder()

                                            m_lReturn = CopyServerToClient(lStandardWordingDocumentTemplateTypeID, lStandardWordingDocumentTemplateID)
                                            sTmpFile = "Doc " & lStandardWordingDocumentTemplateID & "." & m_sDocFileExtension
                                            If m_bArchiveAsXML Then
                                                AddToTable(iDocTypeSort:=1, sDocType:="SWCL", sDocName:=ToSafeString(vStandardWordingsArray(ACStdWrdsPosCode, iStdWordingCnt), 0), sMainGroup:="", sSubGroup:="", sFieldName:="", sValue:="", sLoop1:="", sLoop2:="", sLoop3:="", iRiskCnt:=ToSafeInteger(vRiskId), sRiskDescription:=sRiskDescription)
                                            End If

                                            If m_dtStdWording IsNot Nothing AndAlso (m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(1) Is Nothing OrElse m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(1).ToString() = "") Then
                                                m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(1) = vStandardWordingsArray(ACStdWrdsPosCode, iStdWordingCnt)
                                            ElseIf m_dtStdWording IsNot Nothing Then
                                                AddClauseToTableForCCM(m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(0))
                                                m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(1) = vStandardWordingsArray(ACStdWrdsPosCode, iStdWordingCnt)
                                            End If
                                            m_lReturn = ProcessSubDocXml(sSourceTemplate:=sTmpFile,
                                                    sDestinationMerge:=sSTD_WORDING_TEMP,
                                                    cInputFile:=cInputFile,
                                                    oTextStreamOut:=oTextStreamOut,
                                                vRiskId:=vRiskId, bInRiskLoop:=bInRiskLoop, v_bMergeStyles:=True, sDocType:="SWCL", sDocName:=ToSafeString(vStandardWordingsArray(ACStdWrdsPosCode, iStdWordingCnt), 0), sRiskDescription:=sRiskDescription)

                                            'don't ask me why, but without this we get an empty page
                                            If sFieldType = StandardWordingNPTag Then
                                                'sCurrentLine = "<w:br/>"
                                                'Print #iFileNumOut, sCurrentLine
                                                'oTextStreamOut.Write(sCurrentLine)
                                            End If

                                            'Delete standard wording doc as it has been merged into
                                            'parent by now.
                                            m_lReturn = DeleteClient(m_sClient, sTmpFile)
                                            m_lReturn = DelDirectory(m_sClient)

                                            ' Restore the Original ParentWorkDirectory to current work directory
                                            m_sClient = sParentDocumentWorkDirectoryStandardWording

                                        End If
                                    Next iStdWordingCnt
                                    'Sumeet: Not required as sTemp is carried forward in scurrentline so will be written furthur.
                                    'oTextStreamOut.Write(sTemp)
                                End If
                            End If

                        Case TotalTag
                            '2 possible situations
                            '<@TOTAL_n = equation@>
                            '<@TOTAL_n@> - print the sucker

                            m_lReturn = ExtractTotalInfo(sCurrentLine:=sCurrentLine,
                                                         lFieldStart:=lFieldStart,
                                                         lFieldEnd:=lFieldEnd,
                                                         iTotalNumber:=iTotalNumber,
                                                         sEquation:=sEquation,
                                                     cInputFile:=cInputFile)

                            m_lReturn = PrimeTotals(iTotalNumber:=iTotalNumber)

                            If (sEquation = "") Then
                                'We're just after the value
                                sValue = m_vTotals(iTotalNumber)
                                sValue = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=ToSafeDouble(sValue))
                                ' PW110603 - don't use formatted output if nested
                                If iCurrentNestingLevel > 1 Then
                                    sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) _
                                                & CDbl(sValue) & Mid$(sCurrentLine,
                                            lFieldStart + lFieldEnd + m_iFieldMarkerLength - 1)
                                Else
                                    sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) _
                                                & sValue & Mid$(sCurrentLine, lFieldStart +
                                            lFieldEnd + m_iFieldMarkerLength - 1)
                                End If
                            Else
                                m_lReturn = ResolveEquation(iTotalNumber:=iTotalNumber,
                                                        sEquation:=sEquation)
                                ' PW240603 - CQ1575 - just
                                ' lops off the rest of the line - need to keep it
                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) +
                                    Mid$(sCurrentLine, lFieldStart + lFieldEnd + m_iFieldMarkerLength - 1)
                            End If

                            m_lReturn = RemoveEmptyParagraphsXML(sCurrentLine)

                        Case VariableTag
                            ' RAM20030411 : Document Issuance Changes. 4.4 Scripting 4.4.1.1 Text Variables

                            '2 possible situations
                            '<@VAR_n = expression@> ' An expression or string value is assigned to a variable
                            '<@VAR_n@>              ' Print the content of the variable

                            ' eg. <@VAR_1 = <@FUNC_LCase("ABC")@>@>

                            vValue = EvaluateVariableExpression(v_vStrInput:=sFieldCode,
                                cInputFile:=cInputFile)

                            ' PW110603 - Put quotes round if this is a nested merge field
                            If iCurrentNestingLevel > 1 Then
                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) _
                                            & Chr(147) & vValue & Chr(148) & Mid$(
                                            sCurrentLine, lFieldStart + lFieldEnd +
                                        (m_iFieldMarkerLength * 2) - 1)
                            Else
                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) _
                                            & vValue & Mid$(sCurrentLine, lFieldStart +
                                        lFieldEnd + (m_iFieldMarkerLength * 2) - 1)
                            End If

                        Case FunctionTag
                            ' RAM20030411 : Document Issuance Changes. 4.4 Scripting 4.4.1.2 Functions
                            ' <@FUNC_functype(param1, param2,...)@>

                            ' eg.   <@FUNC_UCase(<@VAR_1@>)@>
                            '       <@FUNC_LCase("ABC")@>
                            '       FUNC_Date()@&gt;&lt;@FUNC_Time()    ' When 2 or more functions are placed in the same line

                            ' Check if we have multiple functions in one line
                            strSearch = FunctionTag & "_"
                            iNoofOccurences = NoOfOccurences(sFieldCode, strSearch)

                            If iNoofOccurences = 1 Then
                                ' PW240604 - CQ5678 - get flag back to indicate if
                                ' result of function is to be treated as a numeric
                                ' value
                                ' RVH 09/11/2004: (Performance) Pass file class rather than number
                                vValue = EvaluateFunctionExpression(v_vStrInput:=
                                        sFieldCode, cInputFile:=cInputFile,
                                    r_bResultIsNumeric:=bResultIsNumeric)

                            ElseIf iNoofOccurences > 1 Then
                                iExpressionStart = 1
                                iExpressionEnd = 0

                                For iCounter = 1 To iNoofOccurences
                                    ' Get the start of next expression
                                    iExpressionEnd = NthInstanceOf(sFieldCode, strSearch, iCounter + 1)
                                    If iExpressionEnd = 0 Then
                                        ' We come to an end
                                        iExpressionLen = Len(sFieldCode) - iExpressionStart
                                        strToken = Mid$(sFieldCode, iExpressionStart)
                                    Else
                                        ' Length of the expression
                                        iExpressionLen = iExpressionEnd - iExpressionStart
                                        ' Previous expression ends 1 chr before
                                        iExpressionEnd = iExpressionEnd - 1
                                        strToken = Mid$(sFieldCode, iExpressionStart, iExpressionLen)
                                    End If

                                    ' PW240604 - CQ5678 - get flag back to indicate if
                                    ' result of function is to be treated as a numeric
                                    ' value
                                    ' RVH 09/11/2004: (Performance) Pass file class rather than number
                                    vReturnValue = EvaluateFunctionExpression(
                                            v_vStrInput:=strToken, cInputFile:=cInputFile,
                                        r_bResultIsNumeric:=bResultIsNumeric)

                                    ' Append the return value with a space (JIC)
                                    vValue = vValue & " " & vReturnValue

                                    If iExpressionEnd <> 0 Then
                                        ' Start of Next expression
                                        iExpressionStart = iExpressionEnd + 1
                                    End If
                                Next iCounter

                            End If

                            'Put quotes round if this is a nested merge field: start
                            'but only if the function result is not to be treated as numeric
                            If iCurrentNestingLevel > 1 AndAlso Not bResultIsNumeric Then
                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) _
                                            & DoubleQuotes & vValue & DoubleQuotes & Mid$(sCurrentLine, lFieldStart +
                                        lFieldEnd + (m_iFieldMarkerLength * 2) - 1)
                            Else
                                sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) _
                                            & vValue & Mid$(sCurrentLine, lFieldStart +
                                        lFieldEnd + (m_iFieldMarkerLength * 2) - 1)
                            End If

                        Case IfTag
                            ' Extract the equation
                            ' RVH 09/11/2004: (Performance) Pass file class rather than number
                            m_lReturn = ExtractEquation(sCurrentLine:=sCurrentLine,
                                    lFieldStart:=lFieldStart, lFieldEnd:=lFieldEnd,
                                    sIfNumber:=sIfNumber, sEquation:=sEquation,
                                cInputFile:=cInputFile)

                            ' Resolve the equation
                            If sEquation <> "" Then
                                Call ResolveEquation(iTotalNumber:=-1,
                                        sEquation:=sEquation,
                                      bCondition:=bCondition)
                                ' If condition is not true, proceed to remove all
                                ' bits of the document between, and including, the IF
                                ' and ENDIF tags
                                If Not bCondition Then
                                    sTemp = ""

                                    sSaveLine = Left(sCurrentLine, lFieldStart - 1)

                                    bEndIfFoundInSameLine = IsEndLoopTagFound(sCurrentLine, sIfNumber, sEndIFEndFragement, sEndIfResolvedFullLineFragement)

                                    If bEndIfFoundInSameLine Then
                                        sSaveLine = sSaveLine & sEndIFEndFragement
                                    Else
                                        sTemp = Mid(sCurrentLine, (lFieldStart))

                                        'Remove IF Tag upto next "</w:t>"
                                        lPos = InStr(1, sTemp, "</w:t>")

                                        If lPos > 0 Then
                                            sEndIFFragement = Mid$(sTemp, lPos)
                                        End If

                                        m_lReturn = RemoveContentsOfTag("<w:t>", "</w:t>", sEndIFFragement)

                                        sSaveLine = sSaveLine & sEndIFFragement

                                    End If

                                    '                            m_lReturn = RemoveEmptyParagraphsXML(sSaveLine, True)

                                    If Not bEndIfFoundInSameLine Then
                                        sTemp = ""

                                        Do While Not cInputFile.EOF
                                            '                                sPrevLine = sNextLine
                                            'Check if eof before reading next line. Not after reading as that way the last line won't be checked.
                                            If cInputFile.EOF Then ' PN25922
                                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                                bPMFunc.LogMessage(
                                                    sUsername:=m_sUsername,
                                                    iType:=gPMConstants.PMELogLevel.PMLogFatal,
                                                    sMsg:="The document template selected is not in a valid format. Cannot find:" & vbCrLf & "<@" & EndIfTag & Separator & sIfNumber & "@>",
                                                    vApp:=ACApp,
                                                    vClass:=ACClass,
                                                    vMethod:="ResolveDocumentXML",
                                                    vErrNo:=Err.Number,
                                                    vErrDesc:=Err.Description)
                                                Exit Function
                                            End If

                                            'Get full line
                                            m_lReturn = ResolveReadFullLine(
                                                cInputFile:=cInputFile,
                                            r_sCurrentLine:=sNextLine)

                                            If IsEndLoopTagFound(sNextLine, sIfNumber, sEndIFEndFragement, sEndIfResolvedFullLineFragement) Then
                                                m_lReturn = RemoveEmptyParagraphsXML(sEndIfResolvedFullLineFragement, True)
                                                m_lReturn = RemoveSpecificTags("<w:br", sEndIfResolvedFullLineFragement)
                                                sTemp = sTemp & sEndIfResolvedFullLineFragement
                                                Exit Do
                                            Else
                                                'If (InStr(1, sNextLine, "</wx:sect>") > 0) Or (InStr(1, sNextLine, "</w:body>") > 0) Then
                                                If (InStr(1, sNextLine, "</w:body>") > 0) Then
                                                    'raise error as no end loop tag found
                                                    bPMFunc.LogMessage(
                                                            sUsername:=m_sUsername,
                                                            iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                            sMsg:="No EndIF Tag found for IF_" & sIfNumber & ", Doc-" & sFileName,
                                                            vApp:=ACApp,
                                                            vClass:=ACClass,
                                                            vMethod:="ResolveDocumentXML",
                                                            vErrNo:=Err.Number,
                                                        vErrDesc:=Err.Description)
                                                    Exit Function
                                                Else
                                                    Dim iPageMgrPos As Integer = 0
                                                    Dim iPageMgrEndPos As Integer = 0
                                                    Dim sPageMargine As String = ""
                                                    'check if paragraph has any page settings 

                                                    If InStr(1, sNextLine, "<w:pgMar") > 0 Then
                                                        iPageMgrPos = InStr(1, sNextLine, "<w:pgMar")
                                                        iPageMgrEndPos = InStr(iPageMgrPos, sNextLine, ">")
                                                        sPageMargine = Mid(sNextLine, iPageMgrPos, iPageMgrEndPos - iPageMgrPos + 1)
                                                        cInputFile.ReplaceTag("<w:pgMar", sPageMargine)
                                                    End If
                                                    If InStr(1, sNextLine, "<w:pgSz") > 0 Then
                                                        iPageMgrPos = InStr(1, sNextLine, "<w:pgSz")
                                                        iPageMgrEndPos = InStr(iPageMgrPos, sNextLine, ">")
                                                        sPageMargine = Mid(sNextLine, iPageMgrPos, iPageMgrEndPos - iPageMgrPos + 1)
                                                        cInputFile.ReplaceTag("<w:pgSz", sPageMargine)

                                                    End If
                                                    sNextLine = ""
                                                End If
                                            End If

                                            sTemp = sTemp & sNextLine
                                        Loop
                                    End If

                                    If InStr(1, sSaveLine, m_sFieldStartMarker) = 0 Then
                                        m_lReturn = RemoveEmptyParagraphsXML(sSaveLine, True)
                                        oTextStreamOut.Write(sSaveLine)
                                        sCurrentLine = sTemp
                                    Else
                                        sCurrentLine = sSaveLine & sTemp
                                    End If

                                    m_lReturn = RemoveEmptyParagraphsXML(sSaveLine, True)

                                Else
                                    ' Condition is true, so just remove the IF tag

                                    sCurrentLine = Left$(sCurrentLine, lFieldStart - 1) _
                                            + Mid(sCurrentLine, lFieldStart + lFieldEnd + m_iFieldMarkerLength - 1)
                                End If
                            End If

                        Case EndIfTag
                            ' If we've reached an ENDIF tag, it must
                            ' have been a true condition, else the IfTag processing
                            ' would've removed it. Just remove the tag
                            sCurrentLine = Left(sCurrentLine, lFieldStart - 1) _
                                    + Mid(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)

                            m_lReturn = RemoveSpecificTags("<w:br", sCurrentLine)
                            m_lReturn = RemoveEmptyParagraphsXML(sCurrentLine, True)
                        Case SubDocumentTag, DocumentSplitTag

                            ' extract the Sub Document Template Code
                            sSubDocumentTemplateCode = Right$(sFieldCode, Len(sFieldCode) - iSep)
                            sSubDocumentTemplateCode = Trim$(sSubDocumentTemplateCode)

                            If Len(sSubDocumentTemplateCode) <= 0 Then
                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                ' Log Error.
                                bPMFunc.LogMessage(
                                        sUsername:=m_sUsername,
                                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                        sMsg:="Missng Sub-Document Template Code",
                                        vApp:=ACApp,
                                        vClass:=ACClass,
                                        vMethod:="ResolveDocumentXML",
                                        vErrNo:=Err.Number,
                                    vErrDesc:=Err.Description)
                                Exit Function
                            End If

                            ' Check if this is valid sub-document template code !!!
                            m_lReturn = m_oBusiness.GetSubDocumentTemplateIdFromCode(
                                v_sSubDocumentTemplateCode:=sSubDocumentTemplateCode,
                                r_lSubDocumentTemplateID:=lSubDocumentTemplateID,
                                r_lSubDocumentTemplateTypeID:=lSubDocumentTemplateTypeID,
                            r_sSubDocumentTemplateDescription:=sSubDocumentTemplateDescription)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                ' Log Error.
                                bPMFunc.LogMessage(
                                        sUsername:=m_sUsername,
                                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                        sMsg:="m_oBusiness.GetTemplateFromCode Failed for : " &
                                        sSubDocumentTemplateCode,
                                        vApp:=ACApp,
                                        vClass:=ACClass,
                                        vMethod:="ResolveDocumentXML",
                                        vErrNo:=Err.Number,
                                    vErrDesc:=Err.Description)
                                Exit Function
                            End If

                            If lSubDocumentTemplateID <= 0 Then
                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                ' Log Error.
                                bPMFunc.LogMessage(
                                        sUsername:=m_sUsername,
                                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                        sMsg:="Invalid Sub-Document Template Code Found : " & sSubDocumentTemplateCode,
                                        vApp:=ACApp,
                                        vClass:=ACClass,
                                        vMethod:="ResolveDocumentXML",
                                        vErrNo:=Err.Number,
                                    vErrDesc:=Err.Description)
                                Exit Function
                            End If

                            ' we have a valid sub-Document ID
                            ' Check if we have any circular reference of sub-documents
                            bSubDocumentTemplateAlreadyExists = False
                            If iLoopDepth > 1 And IsArray(vSubDocumentsArray) Then
                                lFrom = LBound(vSubDocumentsArray)
                                lTo = UBound(vSubDocumentsArray)
                                For lCounter = lFrom To lTo

                                    If Len(sSubDocumentTemplateChain) = 0 Then
                                        sSubDocumentTemplateChain = Trim$(vSubDocumentsArray(lCounter))
                                    Else
                                        sSubDocumentTemplateChain = sSubDocumentTemplateChain & " ==> " &
                                            Trim$(vSubDocumentsArray(lCounter))
                                    End If

                                    ' Check if we have the same code in the array
                                    If LCase$(Trim$(vSubDocumentsArray(lCounter))) =
                                        LCase$(sSubDocumentTemplateCode) Then
                                        ' We found an instance already, so this means its going to create a circular reference
                                        ' so exist and throw error
                                        bSubDocumentTemplateAlreadyExists = True
                                        'Exit For       ' We need to build the hierarchy of sub-docs (chain), so don't exit the loop
                                    End If

                                Next lCounter
                            End If

                            If bSubDocumentTemplateAlreadyExists Then

                                ' Show the full document Template chain
                                sSubDocumentTemplateChain = sSubDocumentTemplateChain & " ==> " & sSubDocumentTemplateCode

                                ' We are going to create a circular reference, so log error and exit
                                ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                ' Log Error.
                                bPMFunc.LogMessage(
                                        sUsername:=m_sUsername,
                                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                        sMsg:="Found a circular Reference to Sub-Documents Templates" & vbCrLf & "in " &
                                        "document : " & sParentDocumentName & vbCrLf & sSubDocumentTemplateChain,
                                        vApp:=ACApp,
                                        vClass:=ACClass,
                                        vMethod:="ResolveDocumentXML",
                                        vErrNo:=Err.Number,
                                    vErrDesc:=Err.Description)

                                ' Exit function, with closing all files
                                Exit Function
                            End If

                            ' We haven't find the code, so,its a valid sub-document template
                            ' add it to the array as the last element
                            If IsArray(vSubDocumentsArray) Then
                                ReDim Preserve vSubDocumentsArray(UBound(vSubDocumentsArray) + 1)
                            Else
                                ReDim vSubDocumentsArray(0)
                            End If
                            ' Save it to the top element
                            vSubDocumentsArray(UBound(vSubDocumentsArray)) = Trim$(sSubDocumentTemplateCode)

                            'Set main file number so that sub document images use main document dependant directory
                            sTemp = Left(sFullFileName, (Len(sFullFileName) - 4))

                            sTemp = Mid(sTemp, InStrRev(sTemp, "\Doc ", -1, vbTextCompare) + 5)
                            m_lMainFileNumber = ToSafeLong(sTemp, -1)

                            ' Store the current document work directory (client)
                            sParentDocumentWorkDir = m_sClient

                            'make sure we have a unique folder for each document
                            m_sClient = ""
                            m_lReturn = GetClientFolder()

                            'Store the current document FileNumber
                            lParentDocumentFileNumber = m_lFileNumber

                            'Clear FileNumber so that subdocument is retrieved
                            m_lFileNumber = 0

                            ' Copy the sub-document template from server to client
                            m_lReturn = CopyServerToClient(
                                lSubDocumentTemplateTypeID,
                            lSubDocumentTemplateID)

                            sTmpFile = "Doc " & lSubDocumentTemplateID & "." & m_sDocFileExtension
                            If m_bArchiveAsXML Then
                                AddToTable(iDocTypeSort:=1, sDocType:="SUBDOC", sDocName:=sSubDocumentTemplateCode, sMainGroup:="", sSubGroup:="", sFieldName:="", sValue:="", sLoop1:="", sLoop2:="", sLoop3:="", iRiskCnt:=ToSafeInteger(vRiskId))
                            End If
                            ' Resolve the sub-document (call the function again to start recursion)
                            m_lReturn = ResolveDocumentXML(sFileName:=sTmpFile,
                                    vInstanceArray:=vInstanceArray,
                                    vRiskId:=vRiskId,
                                bInRiskLoop:=bInRiskLoop, sDocType:="SUBDOC", sDocName:=sSubDocumentTemplateCode, sRiskDescription:=sRiskDescription)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ResolveDocumentXML = m_lReturn       ' Set the error status
                                'Remove the current working directory.  Since we don't need them anymore
                                If m_bUniqueClientDirNeedsDeleting Then
                                    m_lReturn = DelDirectory(m_sClient)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                        ' Log Error.
                                        bPMFunc.LogMessage(
                                                sUsername:=m_sUsername,
                                                iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                sMsg:="Failed to delete Folder [" & m_sClient & "]" & vbCrLf & "after error in " &
                                                "ResolveDocumentXML for " & vbCrLf & sTmpFile,
                                                vApp:=ACApp,
                                                vClass:=ACClass,
                                                vMethod:="ResolveDocumentXML",
                                                vErrNo:=Err.Number,
                                            vErrDesc:=Err.Description)
                                    End If
                                End If
                                'Reduce the level to represent that we have left recursion by one level
                                iLoopDepth = iLoopDepth - 1
                                ' Reset the m_sClient Directory, while exiting
                                m_sClient = sParentDocumentWorkDir
                                'Reset the m_lFileNumber value
                                m_lFileNumber = lParentDocumentFileNumber
                                ' Exit function, with closing all files
                                Exit Function
                            End If

                            ' Insert the sub-document into the parent document
                            ' Use the parent document work dir
                            sTmpFile = m_sClient & "\" & sTmpFile

                            ConvertDocumentUsingSiriusDocumentUtility(sTmpFile, sTmpFile)

                            sSubDocContents = GetSubDocXmlContents(cInputFile, sTmpFile, True)

                            DocumentSplit(sFieldType, sTmpFile, sParentDocumentWorkDir, sSubDocumentTemplateCode)

                            'Remove the subDocument template directory.  Since we don't need them anymore
                            If m_bUniqueClientDirNeedsDeleting Then
                                m_lReturn = DelDirectory(m_sClient)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    ResolveDocumentXML = gPMConstants.PMEReturnCode.PMFalse
                                    ' Log Error.
                                    bPMFunc.LogMessage(
                                                sUsername:=m_sUsername,
                                                iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                sMsg:="Failed to delete Folder [" & m_sClient & "]" & vbCrLf & "while resolving Sub-Document : " & sSubDocumentTemplateCode,
                                                vApp:=ACApp,
                                                vClass:=ACClass,
                                                vMethod:="ResolveDocumentXML",
                                                vErrNo:=Err.Number,
                                            vErrDesc:=Err.Description)
                                    Throw New Exception
                                End If
                            End If

                            ' Reset the m_sClient, since we came out of the loop
                            m_sClient = sParentDocumentWorkDir
                            m_lFileNumber = lParentDocumentFileNumber

                            ' Remove the Sub-Document from the static array
                            If IsArray(vSubDocumentsArray) Then
                                If UBound(vSubDocumentsArray) > 0 Then
                                    ' Remove the last element
                                    ReDim Preserve vSubDocumentsArray(UBound(vSubDocumentsArray) - 1)
                                Else
                                    vSubDocumentsArray = Nothing     ' Just reset
                                End If
                            End If

                            ' Reset the flag for parent document. Else, when the object terminates and
                            ' if this flag is set by the sub-document, then it will enforce the parent
                            ' document work directory deletion
                            If iLoopDepth = 1 Then
                                m_bUniqueClientDirNeedsDeleting = False
                            End If

                            bSuppressOriginalLine = False

                            sStartingPartOfSplitLine = Left$(sCurrentLine, lFieldStart - 1)

                            sEndingPartOfSplitLine = Mid(sCurrentLine, lFieldStart + lFieldEnd +
                                                (m_iFieldMarkerLength * 2) - 1)

                            m_lReturn = GetFullLines(sStartingPartOfSplitLine, sEndingPartOfSplitLine)

                            m_lReturn = RemoveEmptyParagraphsXML(sStartingPartOfSplitLine, True)
                            m_lReturn = RemoveEmptyParagraphsXML(sEndingPartOfSplitLine, True)

                            If InStr(1, sStartingPartOfSplitLine, m_sFieldStartMarker) > 0 Then
                                sCurrentLine = sStartingPartOfSplitLine & sSubDocContents & sEndingPartOfSplitLine
                            Else

                                If Len(Trim$(sStartingPartOfSplitLine)) > 0 Then
                                    sTemp = GetParagarphText(sStartingPartOfSplitLine)

                                    If Len(sTemp) = 0 Then
                                        lTmpPos = InStr(1, sStartingPartOfSplitLine, "<w:br w:type")

                                        If lTmpPos > 0 Then

                                            lTREndPos = InStr(lTmpPos, sStartingPartOfSplitLine, ">")

                                            If lTREndPos > 0 Then
                                                sStartingPartOfSplitLine = Mid$(sStartingPartOfSplitLine, lTmpPos, lTREndPos - lTmpPos + 1)
                                                sStartingPartOfSplitLine = "<w:r>" & sStartingPartOfSplitLine & "</w:r>"
                                            End If
                                        End If

                                        m_lReturn = MergeContentsAtStartingOfDocuemnt(sStartingPartOfSplitLine, sSubDocContents, "<w:p")

                                        oTextStreamOut.Write(sSubDocContents)
                                    Else
                                        oTextStreamOut.Write(sStartingPartOfSplitLine & sSubDocContents)
                                    End If
                                Else
                                    oTextStreamOut.Write(sSubDocContents)
                                End If

                                sCurrentLine = sEndingPartOfSplitLine
                            End If

                        Case Else

                            ' Some other tag, which haven't cater for
                            sCurrentLine = Left(sCurrentLine, lFieldStart - 1) _
                                   + Mid(sCurrentLine, lFieldStart + lFieldEnd + (m_iFieldMarkerLength * 2) - 1)

                    End Select
                    oAllIndexOfFieldStartMarker.Clear()
                    oAllIndexOfFieldEndMarker.Clear()
                    oAllIndexOfFieldStartMarker = AllIndexOf(sCurrentLine, m_sFieldStartMarker)
                    oAllIndexOfFieldEndMarker = AllIndexOf(sCurrentLine, m_sFieldEndMarker)
                    For Each n As Integer In oAllIndexOfFieldEndMarker
                        oAllIndexOfFieldStartMarker.Add(n)
                    Next
                    If oAllIndexOfFieldStartMarker.Count > 0 Then
                        oAllIndexOfFieldStartMarker.Sort()
                    End If


                    lFieldStart = InStr(sCurrentLine, m_sFieldStartMarker)
                    ' Only do this if there is a mergefield found in the line
                    If lFieldStart <> 0 Then
                        ' Reset the variables
                        iCurrentNestingLevel = 0
                        iDeepestNestingLevel = 0
                        ' Loop round the current line
                        For lLoop = lFieldStart To Len(sCurrentLine)
                            ' If mergefield found, increase the level, and check
                            ' if it is the deepest

                            If Mid(sCurrentLine, lLoop, m_iFieldMarkerLength) = m_sFieldStartMarker Then
                                If m_bArchiveAsXML Then
                                    If Mid(sCurrentLine, lLoop + m_iFieldMarkerLength, 3).ToUpper() = "IF_" Then
                                        Dim i As Integer
                                        i = lLoop + m_iFieldMarkerLength + 1
                                        Do Until i > Len(sCurrentLine)
                                            If Mid(sCurrentLine, i, m_iFieldMarkerLength) = m_sFieldStartMarker Then
                                                Do Until i > Len(sCurrentLine)
                                                    If Mid(sCurrentLine, i + m_iFieldMarkerLength, 1) = " " Then
                                                        i = i + 1
                                                    Else
                                                        Exit Do
                                                    End If
                                                Loop
                                                If Mid(sCurrentLine, i + m_iFieldMarkerLength, 3).ToUpper() = "DB_" Then
                                                    bIfExist = True
                                                End If
                                                Exit Do
                                            ElseIf Mid(sCurrentLine, i, Len(m_sFieldEndMarker)) = m_sFieldEndMarker Then
                                                Exit Do
                                            End If
                                            i = i + 1
                                        Loop
                                    End If
                                End If
                                iCurrentNestingLevel = iCurrentNestingLevel + 1
                                If iCurrentNestingLevel > iDeepestNestingLevel Then
                                    iDeepestNestingLevel = iCurrentNestingLevel
                                    ' If this is the deepest level, set the field pointer
                                    lFieldStart = lLoop
                                End If
                                ' RVH 08/11/2004: (Performance) If this is a start marker, it can't be an end marker as well
                                ' If end of mergefield decrease the level
                            ElseIf Mid(sCurrentLine, lLoop, Len(m_sFieldEndMarker)) = m_sFieldEndMarker Then
                                iCurrentNestingLevel = iCurrentNestingLevel - 1
                                ' PW240603 - If level is back to 0, we have found the
                                ' end of a set of mergefields, so process these, i.e
                                ' exit (CQ1575)
                                If iCurrentNestingLevel = 0 Then
                                    Exit For
                                End If
                            End If

                            'Fwd the loop to the next
                            Dim nNewVal As Integer = 0
                            For Each n As Integer In oAllIndexOfFieldStartMarker
                                If n > lLoop Then
                                    nNewVal = n
                                    Exit For
                                End If
                            Next
                            If nNewVal > 0 Then
                                lLoop = nNewVal - 1
                            Else
                                lLoop = Len(sCurrentLine) + 1
                            End If
                        Next

                        ' Set the current mergefield nesting level we are working at
                        iCurrentNestingLevel = iDeepestNestingLevel

                    End If
                Loop

                If (bSuppressOriginalLine = False) Then
                    ' PW311003 - CQ1980 - Do removing of O:P tags here, note this
                    ' should only be done if there is no content on the line
                    If bLineMerged Then
                        m_lReturn = RemoveEmptyParagraphsXML(r_sLine:=sCurrentLine)
                    End If
                End If

                m_lReturn = RemoveInvalidCharacters(sCurrentLine)
                oTextStreamOut.Write(sCurrentLine)


                bLineMerged = False
            Loop

            ' RVH 08/11/2004: (Performance) Remove the file class
            cInputFile = Nothing
            bFullFileNameIsOpen = False

            oTextStreamOut.Close()
            'Close #iFileNumOut
            bMergeFileIsOpen = False

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ResolveDocumentXML = m_lReturn
                Exit Function
            End If

            ' Use CopyFile rather than PMFileCopy, since it will delete the sourcefile too and
            ' and also it will overwrite the destination file, so we don't need to delete it first
            m_sFileCopyMsg = ""

            m_lReturn = bPMDocFunctions.CopyFile(sMergeFile, sFullFileName, True, True, m_sFileCopyMsg)
            If m_bArchiveAsXML And iLoopDepth = 1 Then
                Dim soutput As String = String.Empty
                CreateXML(soutput)
                Dim sw As StreamWriter = New StreamWriter(sFullFileNameResolvedXML)
                sw.Write(soutput)
                sw.Close()
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ResolveDocumentXML = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file." & vbCrLf & "sMergeFile    : " & sMergeFile & vbCrLf &
                          "sFullFileName : " & sFullFileName & vbCrLf & "Error Details : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="ResolveDocumentXML")
                Exit Function        ' Don't exit, Exit with proper error handling
            End If

            iLoopDepth = iLoopDepth - 1 'indicate leaving recursion

            m_sResolvedDocumentName = sFullFileName







        Catch ex As Exception

            If Err.Number = 5 Then
                sErrMsg = "There is an invalid character in document in line - " & sCurrentLine
            End If

            ResolveDocumentXML = gPMConstants.PMEReturnCode.PMError

            iLoopDepth = iLoopDepth - 1 'indicate leaving recursion

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="ResolveDocumentXML " & sErrMsg, r_lFunctionReturn:=ResolveDocumentXML, excep:=ex)

        Finally

            'TR Make sure that any open files are closed
            If bFullFileNameIsOpen = True Then
                ' RVH 08/11/2004: (Performance) Remove the file class
                cInputFile = Nothing
                m_lReturn = DOCGeneralFunc.DeleteFile(sFullFileName)
            End If
            If bMergeFileIsOpen = True Then
                'Close #iFileNumOut
                oTextStreamOut.Close()
                m_lReturn = DOCGeneralFunc.DeleteFile(sMergeFile)
            End If
            If bTmpFileIsOpen = True Then
                oFileNumLoop.Close()
            End If


        End Try
    End Function
    Public Sub DocumentSplit(ByVal sFieldType As String, ByVal sTmpFile As String, Optional ByVal sParentWorkDir As String = "", Optional ByVal sSubDocTemplateCode As String = "")
        If sFieldType = DocumentSplitTag Then
            ' Build split document filename
            Dim sTmpFileDocSplit As String = $"Doc {DocumentTemplateId}_Split_{m_iSplitDocFileNo}.{m_sDocFileExtension}"
            m_iSplitDocFileNo += 1

            ' Place split document directly in the parent working directory
            Dim basePath As String = If(String.IsNullOrEmpty(sParentWorkDir), m_sClient, sParentWorkDir)
            sTmpFileDocSplit = Path.Combine(basePath, sTmpFileDocSplit)
            ConvertDocumentUsingSiriusDocumentUtility(sTmpFile, sTmpFileDocSplit)

            ' Apply same format conversion as the parent document
            If m_lMode = ACEmailMode And m_bSpoolAsHTML Then
                If sTmpFileDocSplit.ToUpper.EndsWith("XML") Then
                    Dim sConvertedPath As String = sTmpFileDocSplit.Remove(sTmpFileDocSplit.Length - 3) & "HTM"
                    ConvertDocumentUsingSiriusDocumentUtility(sTmpFileDocSplit, sConvertedPath)
                    sTmpFileDocSplit = sConvertedPath
                End If
            ElseIf m_bSpoolAsTXT Then
                Dim sConvertedPath As String = sTmpFileDocSplit.Remove(sTmpFileDocSplit.Length - 3) & "TXT"
                ConvertDocumentUsingSiriusDocumentUtility(sTmpFileDocSplit, sConvertedPath)
                sTmpFileDocSplit = sConvertedPath
            ElseIf m_bSpoolAsPDF Then
                Dim sConvertedPath As String = sTmpFileDocSplit.Remove(sTmpFileDocSplit.Length - 3) & "pdf"
                ConvertDocumentUsingSiriusDocumentUtility(sTmpFileDocSplit, sConvertedPath)
                sTmpFileDocSplit = sConvertedPath
            Else
                If sTmpFileDocSplit.ToUpper.EndsWith("XML") Then
                    Dim sConvertedPath As String = sTmpFileDocSplit.Remove(sTmpFileDocSplit.Length - 3) & "docx"
                    ConvertDocumentUsingSiriusDocumentUtility(sTmpFileDocSplit, sConvertedPath)
                    sTmpFileDocSplit = sConvertedPath
                End If
            End If
            If m_sResolvedDocList Is Nothing Then m_sResolvedDocList = New List(Of String)()
            If m_sResolvedDocCodeList Is Nothing Then m_sResolvedDocCodeList = New List(Of String)()
            m_sResolvedDocList.Add(sTmpFileDocSplit)
            m_sResolvedDocCodeList.Add(sSubDocTemplateCode)
        End If
    End Sub

    Private Function IsEmptyString(ByRef r_sString As String) As Boolean
        Dim sContents As String

        IsEmptyString = False

        sContents = GetParagarphText(r_sString)

        If Len(sContents) = 0 Then

            IsEmptyString = True

        End If
    End Function
    Public Sub BreakStringIntoList(ByVal v_sStartTag As String,
                                     ByVal v_sEndTag As String,
                                     ByVal v_sString As String, ByRef r_sList As List(Of String))

        Dim sStr() As String
        Dim lCnt As Integer
        Dim sTmpLine As String
        Dim sEndFragment As String

        Dim lPos As Integer

        r_sList = New List(Of String)


        If Right$(v_sStartTag, 1) <> ">" Then
            v_sStartTag = GetTagProperName(v_sStartTag, v_sString)
        End If

        If v_sStartTag.Trim = "" Then
            r_sList.Add(v_sString)
            Exit Sub
        End If

        sStr = v_sString.Split(New String() {v_sStartTag}, StringSplitOptions.None)

        For lCnt = 0 To UBound(sStr)

            If lCnt = 0 Then
                If Len(sStr(lCnt)) > 0 Then
                    r_sList.Add(sStr(lCnt))
                End If
            Else
                lPos = InStr(1, sStr(lCnt), v_sEndTag)

                If lPos > 0 Then
                    sTmpLine = Left$(sStr(lCnt), lPos - 1 + Len(v_sEndTag))
                    sEndFragment = Mid$(sStr(lCnt), lPos + Len(v_sEndTag))

                    If sEndFragment.Trim = "" Then
                        r_sList.Add(v_sStartTag & sTmpLine & sEndFragment)
                    Else
                        r_sList.Add(v_sStartTag & sTmpLine)
                        r_sList.Add(sEndFragment)
                    End If
                Else
                    r_sList.Add(v_sStartTag & sStr(lCnt))
                End If

            End If

        Next

    End Sub


    Private Function GetParagarphText(ByRef r_sString As String, Optional ByVal bWithStyles As Boolean = False) As String
        Dim vArray As New List(Of String)

        Dim lCnt As Integer
        Dim sValue As String = ""
        Dim lPos As Integer
        Dim lEndPos As Integer
        Dim sContents As String = ""
        Dim sSTR() As String

        BreakStringIntoList("<w:t", "</w:t>", r_sString, vArray)

        sValue = ""

        If bWithStyles Then

            sSTR = r_sString.Split(New String() {"<w:t>"}, StringSplitOptions.None)

            For lCnt = 0 To UBound(sSTR)
                If sSTR(lCnt).Contains("</w:t>") Then
                    If sContents = "" Then
                        sContents = sContents & sSTR(lCnt)
                    Else
                        sContents = sContents & "<w:t>" & sSTR(lCnt)
                    End If
                Else
                    If lCnt = sSTR.Length - 1 And sSTR(lCnt) <> "" Then
                        If sContents = "" Then
                            sContents = sContents & sSTR(lCnt) & "</w:t>"
                        Else
                            sContents = sContents & "<w:t>" & sSTR(lCnt) & "</w:t>"
                        End If
                    End If
                End If
            Next

            sContents = Left(sContents, InStrRev(sContents, "</w:t>") - 1)
            sValue = sContents
        Else
            For lCnt = 0 To vArray.Count - 1

                If vArray(lCnt).StartsWith("<w:t>") Then

                    lPos = InStr(5, vArray(lCnt), ">")

                    If lPos > 0 Then
                        lEndPos = InStrRev(vArray(lCnt), "<")
                    End If

                    If lEndPos > lPos Then
                        sValue = sValue & Mid(vArray(lCnt), lPos + 1, lEndPos - lPos - 1)
                    Else
                        sValue = sValue & Mid(vArray(lCnt), lPos + 1)
                    End If
                Else
                    lPos = InStr(1, vArray(lCnt), "<")

                    If lPos > 1 Then
                        sValue = sValue & Left(vArray(lCnt), lPos - 1)
                    End If
                End If

            Next
        End If


        Return ToSafeString(sValue).Trim()
    End Function

    Private Function IsEndLoopTagFound(ByVal v_sLine As String, ByVal v_sIfNum As String,
                                       ByRef r_sEndLineFragement As String,
                                       ByRef r_sResolvedFullLine As String) As Boolean
        Dim lPos As Integer


        Dim lEndMarkerPos As Long
        Dim sStartingFragment As String
        Dim sEndFragement As String
        Dim stemp As String = ""

        IsEndLoopTagFound = False

        r_sEndLineFragement = ""

        lPos = InStr(v_sLine, m_sFieldStartMarker & EndIfTag & Separator & v_sIfNum & m_sFieldEndMarker)

        Do While True
            If lPos > 0 Then

                stemp = Mid(v_sLine, lPos + Len(m_sFieldStartMarker & EndIfTag & Separator), InStr(lPos, v_sLine, m_sFieldEndMarker) - (lPos + Len(m_sFieldStartMarker & EndIfTag & Separator)))
                'lNextCharNum = lPos + Len(m_sFieldStartMarker & EndIfTag & Separator & v_sIfNum)
                If v_sIfNum = stemp Then
                    IsEndLoopTagFound = True
                End If
                'iNextCharAsc = Asc(Mid$(v_sLine, lNextCharNum, 1))

                'IsEndLoopTagFound = True

                'If iNextCharAsc > 47 And iNextCharAsc < 58 Then
                '    IsEndLoopTagFound = False
                'End If

                If IsEndLoopTagFound Then
                    sStartingFragment = Left$(v_sLine, lPos - 1)
                    lEndMarkerPos = InStr(lPos, v_sLine, m_sFieldEndMarker)
                    sEndFragement = Mid$(v_sLine, lEndMarkerPos + m_iFieldMarkerLength)

                    m_lReturn = RemoveContentsOfTag("<w:t>", "</w:t>", sStartingFragment, True)

                    r_sEndLineFragement = sEndFragement
                    r_sResolvedFullLine = sStartingFragment & sEndFragement

                    Exit Do
                End If

                'lPos = lPos + 1
                v_sLine = v_sLine.Substring(lPos)
                lPos = InStr(v_sLine, m_sFieldStartMarker & EndIfTag & Separator & v_sIfNum)
            Else
                Exit Do
            End If
        Loop
    End Function
    ' ************************************************************************
    ' Name : bQuestionAlreadyAsked
    '
    ' Description: Checks if a question in a document has already been asked
    '
    ' History :   07/03/2003    APS     Created

    ' ************************************************************************
    Private Function bQuestionAlreadyAsked(ByRef v_sQuestion As String) As Boolean
        Dim result As Boolean = False
        Dim bQuestionAsked As Boolean



        bQuestionAsked = False

        If Not (m_colQuestions Is Nothing) Then
            If m_colQuestions.Count - 1 > 0 Then
                For iCount As Integer = 1 To m_colQuestions.Count - 1

                    If CStr(m_colQuestions(iCount)).ToUpper() = v_sQuestion.ToUpper() Then
                        bQuestionAsked = True
                        Exit For
                    End If
                Next iCount
            End If
        End If


        Return bQuestionAsked

    End Function


    ' ***************************************************************** '
    '
    ' Name: ExtractTotalInfo
    '
    ' Description:
    '
    ' History: 25/07/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ExtractTotalInfo(ByRef sCurrentLine As String, ByRef lFieldStart As Integer, ByRef lFieldEnd As Integer, ByRef iTotalNumber As Integer, ByRef sEquation As String, ByRef cInputFile As FileClass) As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        Dim iCount As Integer
        Dim bComplete As Boolean
        Dim sNextLine As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'Theory, we've got a line which has on it either <@TOTAL_1@>, which means its
        'value is returned, or we've got <@TOTAL_1 = <@....@>...@>
        'So we can have nested start and end tokens, and we need to split out what's between
        'the first and last, noting that we could have more stuff following

        sEquation = ""
        sTemp = sCurrentLine.Substring(lFieldStart - 1)
        lFieldEnd = 0

        iCount = 0
        iTotalNumber = 0

        bComplete = False

        Do While Not bComplete
            For lTemp As Integer = 1 To sTemp.Length - m_sFieldStartMarker.Length + 1
                If sTemp.Substring(lTemp - 1, System.Math.Min(sTemp.Length, m_sFieldStartMarker.Length)) = m_sFieldStartMarker Then
                    iCount += 1
                End If

                If sTemp.Substring(lTemp - 1, System.Math.Min(sTemp.Length, m_sFieldEndMarker.Length)) = m_sFieldEndMarker Then
                    iCount -= 1
                    If iCount = 0 Then
                        lFieldEnd = lTemp
                        bComplete = True
                        Exit For
                    End If
                End If
            Next lTemp

            If Not bComplete Then
                'It's split over lines, and so let's get the next one and start again...
                'There'll be nothing else on one of these lines, so we're ok doing this
                ' RVH 09/11/2004: (Performance) Use passed file class
                m_lReturn = cInputFile.GetNextLine(sNextLine)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Error fall out of while.....wend loop
                    bComplete = True
                End If
                sTemp = sTemp & sNextLine.Trim()
                sCurrentLine = sCurrentLine & sNextLine.Trim()
                iCount = 0
            End If
        Loop

        'What's between the tokens?
        If lFieldEnd <> 0 Then
            sTemp = sTemp.Substring(m_sFieldStartMarker.Length, System.Math.Min(sTemp.Length, lFieldEnd - m_sFieldEndMarker.Length - 1))
        End If

        'DJM 01/07/2002 : Remove format tags from the equation.
        m_lReturn = RemoveFormatTagsFromOurTags(sTemp)

        'So now we've got it, let's find out what the equation is...
        'Strip off the leading TOTAL_
        sTemp = sTemp.Substring(6)

        'Strip off the leading numbers

        Dim dbNumericTemp As Double
        Do While Double.TryParse(sTemp.Substring(0, 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)
            iTotalNumber = iTotalNumber * 10 + CInt(sTemp.Substring(0, 1))
            sTemp = sTemp.Substring(1)
            If sTemp.Length = 0 Then
                Exit Do
            End If
        Loop
        'What's left is the equation...
        sTemp = sTemp.Trim

        If Left$(sTemp, 1) = "=" Then
            sTemp = Mid$(sTemp, 2)
        End If

        sEquation = sTemp

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ExtractEquation
    '
    ' Description:
    '
    ' History: 17/10/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ExtractEquation(ByRef sCurrentLine As String, ByRef lFieldStart As Integer, ByRef lFieldEnd As Integer, ByRef sIfNumber As String, ByRef sEquation As String, ByRef cInputFile As FileClass) As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        'DC270503 -ISS4236 -integer to long
        Dim iCount As Integer 'MKW100703 PN5298 1.6.9 to 1.8.6 Catchup
        Dim bComplete As Boolean
        Dim sNextLine As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'Theory, we've got <@IF_1 <@....@> = <@....@>...@>
        'So we can have nested start and end tokens, and we need to split out what's between
        'the first and last, noting that we could have more stuff following

        sEquation = ""
        sTemp = sCurrentLine.Substring(lFieldStart - 1)
        lFieldEnd = 0

        iCount = 0
        sIfNumber = ""

        bComplete = False

        If m_sDocFileExtension.ToUpper() = "XML" Then
            'PM043434 - If a field in condition is multiline, then no need to remove tags as lFieldEnd will be incorrect.  
            If Not sTemp.Contains("<w:br") Then
                m_lReturn = RemoveFormatTags(r_sFieldCode:=sTemp)
            End If
        End If

        'DC270503 -ISS4236 -integer to long
        While Not bComplete
            For lTemp As Integer = 1 To sTemp.Length - m_sFieldStartMarker.Length + 1
                If sTemp.Substring(lTemp - 1, System.Math.Min(sTemp.Length, m_sFieldStartMarker.Length)) = m_sFieldStartMarker Then
                    iCount += 1
                End If

                If sTemp.Substring(lTemp - 1, System.Math.Min(sTemp.Length, m_sFieldEndMarker.Length)) = m_sFieldEndMarker Then
                    iCount -= 1
                    If iCount = 0 Then
                        lFieldEnd = lTemp
                        bComplete = True
                        Exit For
                    End If
                End If
            Next lTemp

            If Not bComplete Then
                'It's split over lines, and so let's get the next one and start again...
                'There'll be nothing else on one of these lines, so we're ok doing this
                ' RVH 09/11/2004: (Performance) Use passed file class
                m_lReturn = cInputFile.GetNextLine(sNextLine)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Error fall out of while.....wend loop
                    bComplete = True
                End If
                If m_sDocFileExtension.ToUpper() = "XML" Then
                    sTemp = sTemp & sNextLine.Trim()
                    sCurrentLine = sCurrentLine & sNextLine.Trim()
                    m_lReturn = RemoveFormatTags(r_sFieldCode:=sTemp)
                Else
                    sTemp = sTemp & " " & sNextLine.Trim()
                    sCurrentLine = sCurrentLine & " " & sNextLine.Trim()
                End If

                iCount = 0
            End If
        End While

        'What's between the tokens?
        If lFieldEnd <> 0 Then
            sTemp = sTemp.Substring(m_sFieldStartMarker.Length, System.Math.Min(sTemp.Length, lFieldEnd - m_sFieldEndMarker.Length - 1))
        End If

        If m_sDocFileExtension.ToUpper() <> "XML" Then
            m_lReturn = RemoveFormatTagsFromOurTags(sTemp)
        End If

        'So now we've got it, let's find out what the equation is...
        'Strip off the leading IF
        sTemp = Mid$(sTemp, 4)

        While IsNumeric(Left$(sTemp, 1))
            sIfNumber = sIfNumber & CInt(Left$(sTemp, 1))
            sTemp = Mid$(sTemp, 2)
        End While

        'What's left is the equation...
        sEquation = sTemp.Trim()

        'DJM 16/09/2003 : Make sure ampersands match correctly.
        sEquation = sEquation.Replace("&amp;", "&")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: PrimeTotals
    '
    ' Description:
    '
    ' History: 25/07/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function PrimeTotals(ByRef iTotalNumber As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsArray(m_vTotals) Then
            If m_vTotals.GetUpperBound(0) < iTotalNumber Then
                ReDim Preserve m_vTotals(iTotalNumber)
            End If
        Else
            ReDim m_vTotals(iTotalNumber)
        End If


        If Object.Equals(m_vTotals(iTotalNumber), Nothing) Then
            m_vTotals(iTotalNumber) = 0.0#
        End If

        Return result

    End Function

    ''' <summary>
    ''' ResolveEquation
    ''' </summary>
    ''' <param name="iTotalNumber"></param>
    ''' <param name="sEquation"></param>
    ''' <param name="bCondition"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' History: 25/07/2001 Tomo - Created.
    '''  Amended: 11/02/2003 APS  - Ameneded to handle 'ifs' against strings in loops
    '''RAM20050113   : Ported the generic resolving features from 1.9.2 code set 
    ''' The logic used in here is now moved to ResolveDocumentHTML method
    '''</remarks>
    <HandleProcessCorruptedStateExceptions>
    Private Function ResolveEquation(ByVal iTotalNumber As Integer,
                                     ByVal sEquation As String,
                                  Optional ByRef bCondition As Boolean = False) As Integer

        Dim nResult As Integer
        Dim sResolved As String
        Dim nFieldStart As Integer
        Dim nFieldEnd As Integer
        Dim sValue As String = ""
        Dim bisDate As Boolean
        Dim dt1 As Date
        Dim dt2 As Date
        Dim bSpan As Boolean
        Dim sChar As String
        Dim nNum As Integer

        nResult = PMEReturnCode.PMTrue
        sResolved = Replace(sEquation, "�", "")
        sResolved = Replace(sResolved, Chr(148), Chr(34))
        sResolved = Replace(sResolved, Chr(147), Chr(34))

        If Right$(sResolved, 1) = "'" Then
            sResolved = Left$(sResolved, Len(sResolved) - 1) & Chr(34)
        End If
        'There's a complication.  There are possible <span style...> and </span> tokens within
        'the string.  The one good thing is that we don't have to get any more lines...

        bSpan = True
        While bSpan
            nFieldStart = (sResolved.IndexOf("<span", StringComparison.CurrentCulture) + 1)

            If nFieldStart = 0 Then
                bSpan = False
            Else
                For iTemp As Integer = nFieldStart To sResolved.Length
                    If sResolved.Substring(iTemp - 1, 1) = ">" Then
                        nFieldEnd = iTemp
                        Exit For
                    End If
                Next iTemp

                sResolved = sResolved.Substring(0, nFieldStart - 1) & sResolved.Substring(nFieldEnd)
            End If
        End While

        bSpan = True
        While bSpan
            nFieldStart = (sResolved.IndexOf("</span>", StringComparison.CurrentCulture) + 1)
            If nFieldStart = 0 Then
                bSpan = False
            Else
                sResolved = sResolved.Substring(0, nFieldStart - 1) & sResolved.Substring(nFieldStart + 6)
            End If
        End While

        ' Do we have something to resolve, if not just exit
        If sResolved = "" Then
            Return nResult
        End If

        ' Remove any other formatting tags that are in there e.g. </b>
        m_lReturn = RemoveFormatTags(r_sFieldCode:=sResolved)

        If sResolved = "" Then
            Return nResult
        End If
        'handle looping if statements / remove the 'a' tag if needed
        If (sResolved.IndexOf("name", StringComparison.CurrentCultureIgnoreCase) + 1) > 1 Then
            sResolved = sResolved.Substring(sResolved.Length - (sResolved.Length - (sResolved.IndexOf(">"c) + 1)))
            If sResolved.IndexOf("<"c) >= 0 Then
                sResolved = sResolved.Substring(0, sResolved.IndexOf("<"c)) & sResolved.Substring(sResolved.Length - (sResolved.Length - (sResolved.IndexOf(">"c) + 1)))
            End If
        End If
        If (sResolved.IndexOf("&") > 0 And sResolved.IndexOf("=") > 0) AndAlso (IsDate(RemoveDoubleQuotes(Left(sResolved, InStr(sEquation, "&") - 1))) _
                                                                       And Not IsNumeric(RemoveDoubleQuotes(Left(sResolved, InStr(sEquation, "&") - 1)))) Then
            bisDate = True
            dt1 = ToSafeDate(RemoveDoubleQuotes(Left(sResolved, InStr(sEquation, "&") - 1).Trim()), #12/29/1899#)
            dt2 = ToSafeDate(RemoveDoubleQuotes(Right(sResolved, (Len(sEquation) - InStrRev(sEquation, "=")))), #12/29/1899#)
        ElseIf sResolved.IndexOf("&") > 0 AndAlso (IsDate(RemoveDoubleQuotes(Left(sResolved, InStr(sEquation, "&") - 1))) _
                                          And Not IsNumeric(RemoveDoubleQuotes(Left(sResolved, InStr(sEquation, "&") - 1)))) Then
            bisDate = True
            dt1 = ToSafeDate(RemoveDoubleQuotes(Left(sResolved, InStr(sEquation, "&") - 1).Trim()), #12/29/1899#)
            dt2 = ToSafeDate(RemoveDoubleQuotes(Right(sResolved, (Len(sEquation) - InStrRev(sEquation, ";")))), #12/29/1899#)
        ElseIf sResolved.IndexOf("=") > 0 AndAlso (IsDate(RemoveDoubleQuotes(Left(sResolved, InStr(sEquation, "=") - 1))) _
                                                   And Not IsNumeric(RemoveDoubleQuotes(Left(sResolved, InStr(sEquation, "=") - 1)))) Then
            bisDate = True
            dt1 = ToSafeDate(RemoveDoubleQuotes(Left(sResolved, InStr(sEquation, "=") - 1).Trim()), #12/29/1899#)
            dt2 = ToSafeDate(RemoveDoubleQuotes(Right(sResolved, (Len(sEquation) - InStrRev(sEquation, "=")))), #12/29/1899#)
        End If

        'Bugger, < = &lt; > = &gt;
        bSpan = True
        While bSpan
            nFieldStart = (sResolved.IndexOf("&lt;", StringComparison.CurrentCulture) + 1)
            If nFieldStart = 0 Then
                bSpan = False
            Else
                sResolved = sResolved.Substring(0, nFieldStart - 1) & "<" & sResolved.Substring(nFieldStart + 3)
            End If
        End While

        bSpan = True
        While bSpan
            nFieldStart = (sResolved.IndexOf("&gt;", StringComparison.CurrentCulture) + 1)
            If nFieldStart = 0 Then
                bSpan = False
            Else
                sResolved = sResolved.Substring(0, nFieldStart - 1) & ">" & sResolved.Substring(nFieldStart + 3)
            End If
        End While

        bSpan = True
        While bSpan
            nFieldStart = (sResolved.IndexOf("&quot;", StringComparison.CurrentCulture) + 1)
            If nFieldStart = 0 Then
                bSpan = False
            Else
                sResolved = sResolved.Substring(0, nFieldStart - 1) & Chr(148).ToString() & sResolved.Substring(nFieldStart + 5)
            End If
        End While

        If sResolved.IndexOf(Chr(226).ToString() & Chr(128).ToString() & Chr(157).ToString(), StringComparison.CurrentCulture) >= 0 Then
            sResolved = sResolved.Replace(Chr(226).ToString() & Chr(128).ToString() & Chr(157).ToString(), """")
        End If

        If sResolved.IndexOf(Chr(226).ToString() & Chr(128).ToString() & Chr(156).ToString(), StringComparison.CurrentCulture) >= 0 Then
            sResolved = sResolved.Replace(Chr(226).ToString() & Chr(128).ToString() & Chr(156).ToString(), """")
        End If

        For iTemp As Integer = 1 To sResolved.Length
            sChar = sResolved.Substring(iTemp - 1, 1)

            'Worry about Word's weird representation of open and close " and '
            If Asc(sChar(0)) > 128 Then
                Select Case Asc(sChar(0))
                    Case 145, 146, 147, 148
                        Mid(sResolved, iTemp, 1) = """"
                    Case Else
                        Mid(sResolved, iTemp, 1) = Chr(Asc(sChar(0)) - 128).ToString()
                End Select
            End If
        Next iTemp
        If sResolved.IndexOf("%"c) >= 0 Then
            sResolved = sResolved.Replace("%", "")
        End If

        If sResolved.IndexOf(","c) >= 0 Then
            sResolved = sResolved.Replace(",", "")
        End If

        If InStr(sResolved, ",") > 0 Then
            For nNum = 0 To 9
                sResolved = Replace(sResolved, "," & nNum, "" & nNum)
            Next
        End If
        'We should now have the thing as a set of values with operators.  So now what?
        'How about using vbscript?

        ' RVH 11/11/2004: (Performance) Use same object rather than destroy and re-create
        If m_oScriptControl Is Nothing Then
            m_oScriptControl = New MSScriptControl.ScriptControl()

            If m_oScriptControl Is Nothing Then
                ' ScriptControl is not Created. Trap Error.
                Return PMEReturnCode.PMFalse
            End If

            m_oScriptControl.Language = "VBScript"
        Else
            m_oScriptControl.Reset()
        End If
        If iTotalNumber < 0 Then
            'do nothing
        Else
            sResolved = sResolved.Replace(",", "")
        End If

        Try
            If bisDate Then
                If sResolved.IndexOf("<>", StringComparison.CurrentCulture) > 0 Then
                    sValue = dt1 <> dt2
                ElseIf sResolved.IndexOf(">", StringComparison.CurrentCulture) > 0 Then
                    If sResolved.IndexOf("=", StringComparison.CurrentCulture) > 0 Then
                        sValue = dt1 >= dt2
                    Else
                        sValue = dt1 > dt2
                    End If
                ElseIf sResolved.IndexOf("<", StringComparison.CurrentCulture) > 0 Then
                    If sResolved.IndexOf("=", StringComparison.CurrentCulture) > 0 Then
                        sValue = dt1 <= dt2
                    Else
                        sValue = dt1 < dt2
                    End If
                ElseIf sResolved.IndexOf("=", StringComparison.CurrentCulture) > 0 Then
                    sValue = dt1 = dt2
                End If
            Else
                'Validate if the expression is valid
                If IsValidExpression(sResolved) Then
                    sValue = m_oScriptControl.Eval(sResolved)
                Else
                    sValue = Nothing
                End If
            End If

            If iTotalNumber < 0 Then

                If Convert.IsDBNull(sValue) OrElse IsNothing(sValue) Then
                    bCondition = gPMFunctions.ToSafeBoolean(sValue, True)
                Else
                    bCondition = gPMFunctions.ToSafeBoolean(sValue)
                End If
            Else
                m_lReturn = PrimeTotals(iTotalNumber:=iTotalNumber)

                Dim dbNumericTemp As Double

                If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    m_vTotals(iTotalNumber) = sValue
                End If
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMError

            Err_No = m_oScriptControl.Error.Number
            Err_Line = m_oScriptControl.Error.Line
            Err_Col = m_oScriptControl.Error.Column
            Err_Description = m_oScriptControl.Error.Description
            Err_Text = m_oScriptControl.Error.Text

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogError, sMsg:="ResolveEquation - Engine Failed. Error Running Rule." & Chr(13) & Chr(10) & "Error No     : " & CStr(Err_No) & Chr(13) & Chr(10) & "Error Desc   : " & Err_Description & Chr(13) & Chr(10) & "Error Line   : " & CStr(Err_Line) & Chr(13) & Chr(10) & "Error Column : " & CStr(Err_Col) & Chr(13) & Chr(10) & "Error Text   : " & Err_Text & Chr(13) & Chr(10), vApp:=ACApp, vClass:=ACClass, vMethod:="ResolveEquation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            m_oScriptControl = Nothing

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessMailshot
    '
    ' Description:
    '
    ' History: 07/12/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessMailshot() As Integer

        Dim result As Integer = 0

        Dim oDocument As Object = Nothing
        Dim oSelection As Object = Nothing
        Dim sClient As String = ""

        Dim sFileName As String = ""
        Dim bConfirmConversionsTemp As Boolean
        Dim sVersion As String = ""
        Dim lStop As Integer
        Dim bWordOK As Boolean
        Dim iSaveFileNumber As Integer
        Dim sMainFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' SET 18/10/2004 ISS13245 - launch word
            'For Xml we do not use word....
            If m_sDocFileExtension.ToUpper() <> "XML" Then

                m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)
            End If

            'RWH(18/09/2000) Check array is not empty first.
            If Not Information.IsArray(m_vMailshotArray) Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If Not (m_proProgress Is Nothing) Then

                m_proProgress.Min = m_vMailshotArray.GetLowerBound(1)

                m_proProgress.Max = m_vMailshotArray.GetUpperBound(1) + 2

                m_proProgress.value = m_vMailshotArray.GetLowerBound(1)
            End If

            If m_sDocFileExtension.ToUpper() <> "XML" Then

                m_oWord.ScreenUpdating = False
            End If

            ' Load the fields into the Business object
            m_lReturn = m_oBusiness.LoadFields()

            'Column party_cnt
            For lTemp As Integer = m_vMailshotArray.GetLowerBound(1) To m_vMailshotArray.GetUpperBound(1)
                m_lReturn = CopyMailshotServerToClient(lCount:=lTemp + 1)

                'RWH(07/09/2000) RSAIB Process 108, changed to html.
                sFileName = "Doc " & (CStr(lTemp + 1)) & "." & m_sDocFileExtension
                sClient = m_sClient & "\" & sFileName

                'set the values...
                m_lPartyCnt = CInt(m_vMailshotArray(0, lTemp))
                m_lInsuranceFileCnt = 0
                m_lClaimCnt = 0
                'DJM 17/04/2002 : Set the document ref for EACH entry in the array.
                If m_vMailshotArray.GetUpperBound(0) = 2 Then
                    m_sDocumentRef = CStr(m_vMailshotArray(2, lTemp))
                End If

                m_lReturn = ResolveDocumentXML(sFileName)

                If Not (m_proProgress Is Nothing) Then

                    m_proProgress.value += 1
                End If
            Next lTemp

            'Now we have a whole load of word documents called doc 0 through doc n.
            'We must make them all part of one document.

            'DJM 12/11/2002 : Check word ready for the new document
            If m_sDocFileExtension.ToUpper() <> "XML" Then
                bWordOK = False
                Do While Not bWordOK
                    bWordOK = IsWordValid()
                Loop
            End If

            sFileName = "Doc " & (CStr(m_vMailshotArray.GetLowerBound(1) + 1)) & "." & m_sDocFileExtension
            sClient = m_sClient & "\" & sFileName
            sMainFileName = sClient

            If m_sDocFileExtension.ToUpper() <> "XML" Then
                ' Open a new document

                oDocument = m_oWord.Documents.Open(ToSafeString(sClient))

                'DJM 12/11/2002 : Check word completed the new document
                bWordOK = False
                Do While Not bWordOK
                    bWordOK = IsWordValid()
                Loop


                oDocument.Select()

                oSelection = m_oWord.Selection

                oSelection.Collapse(Direction:=0)

                oSelection.InsertBreak(Type:=7)
                iSaveFileNumber = 150
            Else
                'Insert Break into sMainFilename
                InsertBreakUsingSiriusDocumentUtility(sMainFileName)
            End If

            For lTemp As Integer = m_vMailshotArray.GetLowerBound(1) To m_vMailshotArray.GetUpperBound(1)

                If lTemp <> m_vMailshotArray.GetLowerBound(1) Then

                    'RWH(07/09/2000) RSAIB Process 108, changed to html.
                    sFileName = "Doc " & (CStr(lTemp + 1)) & "." & m_sDocFileExtension
                    sClient = m_sClient & "\" & sFileName

                    If m_sDocFileExtension.ToUpper() <> "XML" Then

                        bConfirmConversionsTemp = m_oWord.Options.ConfirmConversions

                        'Save word Document After insert certain Insert Complete
                        'For PN-41194
                        If lTemp = iSaveFileNumber Then

                            m_oWord.Documents.Application.DefaultSaveFormat = True

                            m_oWord.Documents.Save(NoPrompt:=True, OriginalFormat:=0)
                            iSaveFileNumber += 150
                        End If


                        oSelection.InsertFile(FileName:=ToSafeString(sClient), Range:="", ConfirmConversions:=False, link:=False, Attachment:=False)

                        'DJM 12/11/2002 : Check word has completed the insert
                        bWordOK = False
                        Do While Not bWordOK
                            bWordOK = IsWordValid()
                        Loop


                        oSelection.Collapse(Direction:=0)


                        m_oWord.Options.ConfirmConversions = bConfirmConversionsTemp

                        If lTemp < m_vMailshotArray.GetUpperBound(1) Then

                            oSelection.InsertBreak(Type:=7)
                        End If
                    Else
                        'Append Xml File to document (using aspose)
                        ' Append sClient to sMainFilename
                        ' Add Break if lTemp < UBound(m_vMailshotArray, 2)
                        AppendDocumentUsingSiriusDocumentUtility(sClient, sMainFileName, lTemp < m_vMailshotArray.GetUpperBound(1))
                    End If
                End If
            Next lTemp

            If Not (m_proProgress Is Nothing) Then

                m_proProgress.value += 1
            End If

            'RWH(07/09/2000) RSAIB Process 108, changed to HTML.
            sClient = m_sClient & "\Doc 0." & m_sDocFileExtension
            'TR(23/02/03) Save this filename locally in a prop, for calling app
            m_sMailshotFilenameForSpooling = sClient


            If m_sDocFileExtension.ToUpper() <> "XML" Then
                'DJM 20/08/2002 : Allow XP to save as HTML
                'RWH(19/10/2000) Word 97 doesn't allow to save as HTML.
                lStop = (m_sWordVersion.IndexOf("."c) + 1)
                If lStop = 0 Then
                    sVersion = m_sWordVersion
                Else
                    sVersion = m_sWordVersion.Substring(0, lStop - 1)
                End If
                If CInt(sVersion) >= 9 Then

                    oDocument.SaveAs(FileName:=ToSafeString(sClient), FileFormat:=8)
                Else

                    oDocument.SaveAs(FileName:=ToSafeString(sClient))
                End If

                oDocument.Close()

                oSelection = Nothing
                oDocument = Nothing
                m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
            Else

                System.IO.File.Move(sMainFileName, sClient)
                Directory.Delete(m_sClient & "\temp")
            End If

            For lTemp As Integer = m_vMailshotArray.GetLowerBound(1) To m_vMailshotArray.GetUpperBound(1)

                'RWH(07/09/2000) RSAIB Process 108, changed to html.
                sFileName = "Doc " & (CStr(lTemp + 1)) & "." & m_sDocFileExtension

                DeleteClient(m_sClient, sFileName)

            Next lTemp

            m_proProgress = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Clean up memory
            If oSelection Is Nothing = False Then
                oSelection = Nothing
            End If
            If oDocument Is Nothing = False Then
                oDocument.Close()
                oDocument = Nothing
            End If
            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
            m_proProgress = Nothing
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessMailshot Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessMailshot", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function InsertBreakUsingSiriusDocumentUtility(ByRef v_sDocument As String) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "InsertBreakUsingSiriusDocumentUtility"

        Dim oConvert As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            oConvert = New SiriusDocumentUtility.Document()

            oConvert.AppendBreakToDocument(ToSafeString(v_sDocument))

            Return result
        Catch ex As Exception
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)

        Finally
            oConvert = Nothing
        End Try
        Return result
    End Function


    Public Function AppendDocumentUsingSiriusDocumentUtility(ByVal v_sSourceDocument As String, ByVal v_sDestDocument As String, ByVal v_bAppendBreak As Boolean) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "AppendDocumentUsingSiriusDocumentUtility"
        Dim oConvert As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oConvert = New SiriusDocumentUtility.Document()

            oConvert.AppendDocument(ToSafeString(v_sDestDocument), ToSafeString(v_sSourceDocument), ToSafeString(v_bAppendBreak))

            Return result
        Catch ex As Exception
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)

        Finally
            oConvert = Nothing

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: Process Policy Shares
    '
    ' Description:
    '
    ' History: DC170203 -ISS1405 -Created
    ' CJB 02/03/05 PN7241 - Changed ProcessPolicyShares to open the first doc and insert the others into it
    '              rather than open a new file (as it had different formatting (borders etc))
    ' RAM20050504   : Applied Chris Barnes changes for PN7241
    ' ***************************************************************** '
    Private Function ProcessPolicyShares() As Integer

        Dim result As Integer = 0

        Dim oDocument As Object = Nothing
        Dim oSelection As Object = Nothing
        Dim sClient As String = ""

        Dim sFileName As String = ""
        Dim bConfirmConversionsTemp As Boolean
        Dim sVersion As String = ""
        Dim lStop As Integer
        Dim bWordOK As Boolean
        Dim sMainFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' SET 18/10/2004 ISS13245 - launch word
            If m_sDocFileExtension.ToUpper() <> "XML" Then

                m_lReturn = bPMDocFunctions.StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)
            End If

            'RWH(18/09/2000) Check array is not empty first.
            If Not Information.IsArray(m_vPolicySharesArray) Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If Not (m_proProgress Is Nothing) Then

                m_proProgress.Min = m_vPolicySharesArray.GetLowerBound(1)

                m_proProgress.Max = m_vPolicySharesArray.GetUpperBound(1) + 2

                m_proProgress.value = m_vPolicySharesArray.GetLowerBound(1)
            End If

            If m_sDocFileExtension.ToUpper() <> "XML" Then

                m_oWord.ScreenUpdating = False
            End If

            ' Load the fields into the Business object
            m_lReturn = m_oBusiness.LoadFields()

            'Process main invoice for summary
            m_lReturn = CopyPolicySharesServerToClient(lCount:=1)

            sFileName = "Doc 1." & m_sDocFileExtension
            sClient = m_sClient & "\" & sFileName
            sMainFileName = sClient


            m_lReturn = ResolveDocumentXML(sFileName)


            m_sOldDocumentRef = m_sDocumentRef
            m_lOldPartyCnt = m_lPartyCnt

            'Column party_cnt
            For lTemp As Integer = m_vPolicySharesArray.GetLowerBound(1) To m_vPolicySharesArray.GetUpperBound(1)
                m_lReturn = CopyPolicySharesServerToClient(lCount:=lTemp + 2)

                sFileName = "Doc " & (CStr(lTemp + 2)) & "." & m_sDocFileExtension
                sClient = m_sClient & "\" & sFileName

                'set the values...
                m_lPartyCnt = CInt(m_vPolicySharesArray(0, lTemp))
                m_sDocumentRef = m_sOldDocumentRef & "|" & CStr(m_vPolicySharesArray(3, lTemp))


                m_lReturn = ResolveDocumentXML(sFileName)


                If Not (m_proProgress Is Nothing) Then

                    m_proProgress.value += 1
                End If
            Next lTemp

            m_sDocumentRef = m_sOldDocumentRef
            m_lPartyCnt = m_lOldPartyCnt

            'Now we have a whole load of word documents called doc 0 through doc n.
            'We must make them all part of one document.

            If m_sDocFileExtension.ToUpper() <> "XML" Then
                bWordOK = False
                Do While Not bWordOK
                    bWordOK = IsWordValid()
                Loop
            End If

            'First process main debit note - for summary
            sFileName = "Doc 1." & m_sDocFileExtension
            sClient = m_sClient & "\" & sFileName

            ' Don't open a new doc as it'll have different formatting (borders etc) but open the 1st doc  PN7241
            ' so we can then add the other shared premium docs to it.
            If m_sDocFileExtension.ToUpper() <> "XML" Then

                oDocument = m_oWord.Documents.Open(ToSafeString(sClient))

                bWordOK = False
                Do While Not bWordOK
                    bWordOK = IsWordValid()
                Loop


                oDocument.Select()

                oSelection = m_oWord.Selection

                oSelection.Collapse(Direction:=0)

                oSelection.InsertBreak(Type:=7)
            Else
                InsertBreakUsingSiriusDocumentUtility(sClient)
            End If

            'Now process all invoices for shared premiums
            For lTemp As Integer = m_vPolicySharesArray.GetLowerBound(1) To m_vPolicySharesArray.GetUpperBound(1)

                sFileName = "Doc " & (CStr(lTemp + 2)) & "." & m_sDocFileExtension
                sClient = m_sClient & "\" & sFileName

                If m_sDocFileExtension.ToUpper() <> "XML" Then

                    bConfirmConversionsTemp = m_oWord.Options.ConfirmConversions


                    oSelection.InsertFile(FileName:=ToSafeString(sClient), Range:="", ConfirmConversions:=False, link:=False, Attachment:=False)

                    'DJM 12/11/2002 : Check word has completed the insert
                    bWordOK = False
                    Do While Not bWordOK
                        bWordOK = IsWordValid()
                    Loop


                    oSelection.Collapse(Direction:=0)


                    m_oWord.Options.ConfirmConversions = bConfirmConversionsTemp

                    'PSL 13/06/2003 Iss4443 Don't do an extra page break
                    If lTemp < m_vPolicySharesArray.GetUpperBound(1) Then

                        oSelection.InsertBreak(Type:=1)
                    End If
                Else
                    AppendDocumentUsingSiriusDocumentUtility(sClient, sMainFileName, lTemp < m_vPolicySharesArray.GetUpperBound(1))
                End If
            Next lTemp

            'MKW100703 PN5298 START 1.6.9 to 1.8.6 Catchup
            '    'DJM 14/01/2003 : Remove files before saving main file in case of duplicate filename.
            '    'remove main summary page ...
            '    sFileName = "Doc 1." & m_sDocFileExtension
            '    Call DeleteClient(m_sClient, sFileName)
            '
            '    '...and remove shared premium pages
            '    For lTemp = LBound(m_vPolicySharesArray, 2) To UBound(m_vPolicySharesArray, 2)
            '
            '        sFileName = "Doc " & (lTemp + 2) & "." & m_sDocFileExtension
            '
            '        Call DeleteClient(m_sClient, sFileName)
            '
            '    Next lTemp
            'MKW100703 PN5298 END 1.6.9 to 1.8.6 Catchup

            If Not (m_proProgress Is Nothing) Then

                m_proProgress.value += 1
            End If

            'sClient = m_sClient & "\Doc 0." & m_sDocFileExtension
            sClient = m_sClient & "\Doc " & CStr(m_lDocumentTemplateId) & "." & m_sDocFileExtension

            If m_sDocFileExtension.ToUpper() <> "XML" Then
                lStop = (m_sWordVersion.IndexOf("."c) + 1)
                If lStop = 0 Then
                    sVersion = m_sWordVersion
                Else
                    sVersion = m_sWordVersion.Substring(0, lStop - 1)
                End If
                If CInt(sVersion) >= 9 Then

                    oDocument.SaveAs(FileName:=ToSafeString(sClient), FileFormat:=8)
                Else

                    oDocument.SaveAs(FileName:=ToSafeString(sClient))
                End If

                oDocument.Close()

                oSelection = Nothing
                oDocument = Nothing
                m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
            Else
                'Copy sMainFilename to sClient
                System.IO.File.Copy(sMainFileName, sClient, True)

            End If

            m_proProgress = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Clean up memory
            If oSelection Is Nothing = False Then
                oSelection = Nothing
            End If
            If oDocument Is Nothing = False Then
                oDocument.Close()
                oDocument = Nothing
            End If
            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
            m_proProgress = Nothing

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessPolicyShares Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPolicyShares", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Private Function ProcessRisksXML() As Integer

        'Declare Local Variables.
        Dim result As Integer = 0
        Dim iTemp As Integer
        Dim lType, lTemplate, lFileNumber, lInsuranceFileCnt As Integer
        Dim sClient As String = String.Empty
        Dim sFileName As String = String.Empty
        Dim sTempClientFolder As String = String.Empty

        'Objects for Word Intergration (TOC).
        Dim oDocument, oRange As Object
        Dim sVersion As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'Check array is not empty first.
        If Not Information.IsArray(m_vRiskArray) Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        If Not (m_proProgress Is Nothing) Then

            m_proProgress.Min = m_vRiskArray.GetLowerBound(1)

            m_proProgress.Max = m_vRiskArray.GetUpperBound(1) + 2

            m_proProgress.value = m_vRiskArray.GetLowerBound(1)
        End If

        ' Load the fields into the Business object
        m_lReturn = m_oBusiness.LoadFields()

        'create temporary client folder to store resolved docs in
        m_lReturn = GetClientDirectory(r_sClientDir:=sTempClientFolder, bUnique:=True)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = CreateFolderTree(sFolderName:=sTempClientFolder)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create temporary client folder.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksXml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        'Column 0 = type, 1 = template, 2 = file number, 3 = insurance file cnt
        For lTemp As Integer = m_vRiskArray.GetLowerBound(1) To m_vRiskArray.GetUpperBound(1)
            lType = CInt(m_vRiskArray(0, lTemp))
            lTemplate = CInt(m_vRiskArray(1, lTemp))
            lFileNumber = CInt(m_vRiskArray(2, lTemp))
            lInsuranceFileCnt = CInt(m_vRiskArray(3, lTemp))

            m_lReturn = CopyRiskServerToClient(lType:=lType, lTemplate:=lTemplate, lFileNumber:=lFileNumber, lCount:=lTemp + 1)

            sFileName = "Doc " & (CStr(lTemp + 1)) & "." & m_sDocFileExtension

            sClient = m_sClient & "\" & sFileName

            'set the values...
            m_lInsuranceFileCnt = lInsuranceFileCnt
            m_lClaimCnt = 0

            m_lReturn = ResolveDocumentXML(sFileName)

            m_lReturn = MoveFolderContents(sSourceDir:=m_sClient, sDestDir:=sTempClientFolder)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move resolved file to temp folder (" & sTempClientFolder & ") Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksXml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Not (m_proProgress Is Nothing) Then

                m_proProgress.value += 1
            End If
        Next lTemp

        '    m_lReturn = MoveFolderContents( _
        ''                            sSourceDir:=sTempClientFolder, _
        ''                            sDestDir:=m_sClient)
        '    If m_lReturn <> PMTrue Then
        '        ProcessRisksXML = PMError
        '        LogMessage _
        ''            sUsername:=m_sUsername, _
        ''            iType:=PMLogOnError, _
        ''            sMsg:="Failed to move retrieve temp folder contents (" & sTempClientFolder & ") Failed", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="ProcessRisksXml", _
        ''            vErrNo:=Err.Number, _
        ''            vErrDesc:=Err.Description
        '        Exit Function
        '    End If

        'Process Documents
        Dim sMainFileName As String = ""

        For lTemp As Integer = m_vRiskArray.GetLowerBound(1) To m_vRiskArray.GetUpperBound(1)
            sFileName = sTempClientFolder & "\" & "Doc " & (CStr(lTemp + 1)) & "." & m_sDocFileExtension

            If lTemp = 0 Then
                sMainFileName = m_sClient & "\Doc 0." & m_sDocFileExtension

                System.IO.File.Copy(sFileName, sMainFileName, True)

                InsertBreakUsingSiriusDocumentUtility(sMainFileName)
            Else
                'append document to sMainFileName
                AppendDocumentUsingSiriusDocumentUtility(sFileName, sMainFileName, lTemp < m_vRiskArray.GetUpperBound(1))
            End If
        Next lTemp
        sClient = sMainFileName

        'Process Table Of Contents (TOC) within Word.
        If m_bAddTOC Then
            'ADD Table Of Contents

            m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)

            oDocument = m_oWord.Documents.Open(ToSafeString(sClient))


            oRange = oDocument.Sections(1).Range

            oRange.MoveEnd()

            oRange.Collapse(Direction:=0)

            iTemp = oRange.End - 1
            oRange = Nothing

            oRange = oDocument.Range(Start:=ToSafeInteger(iTemp), End_Renamed:=ToSafeInteger(iTemp))


            oDocument.TablesOfContents.Add(Range:=oRange, UseFields:=False, UseHeadingStyles:=True)

            oRange = Nothing


            For lTemp As Integer = 1 To oDocument.Sections.Count

                With oDocument.Sections(ToSafeInteger(lTemp)).Headers(1).PageNumbers

                    .NumberStyle = 0

                    .HeadingLevelForChapter = 0

                    .IncludeChapterNumber = False

                    .ChapterPageSeparator = 0

                    .RestartNumberingAtSection = False

                    .StartingNumber = 0
                End With
                If lTemp > 1 Then

                    oDocument.Sections(ToSafeInteger(lTemp)).Footers(1).PageNumbers.Add(PageNumberAlignment:=1, FirstPage:=True)
                End If
            Next lTemp


            oDocument.TablesOfContents(1).Update()

            If (m_sWordVersion.IndexOf("."c) + 1) = 0 Then
                sVersion = m_sWordVersion
            Else
                sVersion = m_sWordVersion.Substring(0, m_sWordVersion.IndexOf("."c))
            End If
            If CInt(sVersion) >= 9 Then
                If m_sDocFileExtension.ToUpper() <> "XML" Then

                    oDocument.SaveAs(FileName:=ToSafeString(sClient), FileFormat:=8)
                Else

                    oDocument.SaveAs(FileName:=ToSafeString(sClient), FileFormat:=11)
                End If
            Else

                oDocument.SaveAs(FileName:=ToSafeString(sClient))
            End If

            oDocument.Close()

            oDocument = Nothing

            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
        End If

        If Not (m_proProgress Is Nothing) Then

            m_proProgress.value += 1
        End If

        m_proProgress = Nothing

        'Tidy Up Client Folder
        m_lReturn = DelDirectory(sTempClientFolder)

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetClient
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    '          MKW 281003 PN7287 1.8.5 to 1.8.6 catchup.
    ' ***************************************************************** '
    Private Function GetClientFolder() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Check If the Document Type is EMAIL - Renewals Back Office Changes - Amit
        If m_lMode = ACEmailMode Then
            m_sClient = ""
        End If

        If m_sClient.Trim() > "" Then
            Return result
        End If

        m_lReturn = GetClientDirectory(m_sClient, True)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_bUniqueClientDirNeedsDeleting = True
            Return result
        End If

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
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Server from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            m_sServer = sServer
        End If

        Return result

    End Function

    ' End of Private Methods

    Public Sub New()
        MyBase.New()


    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    Private Function InsertSubDocXML(ByVal sFile As String, ByRef oTextStream As Scripting.TextStream,
                                 ByRef cInputFile As FileClass,
                                 Optional ByVal v_sContentMergedWithFirstLine As String = "",
                                 Optional ByVal v_sContentMergedWithLastLine As String = "",
                                 Optional ByVal v_bMergeStyles As Boolean = False,
                                 Optional ByRef v_sCurrentLine As String = "",
                                 Optional ByRef IsEmptyDoc As Boolean = False,
                                 Optional ByRef nRiskID As Integer = -1) As Integer


        Dim result As Integer = 0
        Dim sSubDoc As String = ""


        'm_lReturn = ConvertDocumentUsingSiriusDocumentUtility(sFile, sFile)

        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '    ' Failed to call the terminate function.
        '    result = gPMConstants.PMEReturnCode.PMError

        '    Return result
        'End If


        sSubDoc = GetSubDocXmlContents(cInputFile, sFile, v_bMergeStyles)

        RemoveInvalidCharacters(sSubDoc)
        IsEmptyDoc = True

        If Not IsEmptyString(sSubDoc) Then

            IsEmptyDoc = False
            If v_sContentMergedWithFirstLine <> "" Then
                m_lReturn = MergeContentsAtStartingOfDocuemnt(v_sContentMergedWithFirstLine, sSubDoc)
            End If

            If v_sContentMergedWithLastLine <> "" Then
                m_lReturn = MergeContentsAtEndOfDocuemnt(v_sContentMergedWithLastLine, sSubDoc)
            End If


            If InStr(1, v_sCurrentLine, m_sFieldStartMarker) > 0 And bIsNestedloop Then
                m_lReturn = MergeContentsAtStartingOfDocuemnt(GetParagarphText(sSubDoc, True), v_sCurrentLine)
            Else
                m_lReturn = RemoveSpecificTags("<w:p/", sSubDoc)
                oTextStream.Write(sSubDoc)
            End If
        End If

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
    Private Function MergeContentsAtStartingOfDocuemnt(ByVal v_sContents As String, ByRef r_sDocXML As String,
                                                   Optional ByVal v_sTag As String = "<w:t") As Integer
        Dim lPos As Integer
        Dim sTemp As String
        Dim sProperTag As String
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        sProperTag = GetTagProperName(v_sTag, r_sDocXML)

        lPos = InStr(1, r_sDocXML, sProperTag)

        If lPos > 0 Then
            sTemp = Left$(r_sDocXML, lPos - 1 + Len(sProperTag))
            r_sDocXML = sTemp & v_sContents & Mid$(r_sDocXML, lPos + Len(sProperTag))
        End If
        Return result
    End Function

    Private Function MergeContentsAtEndOfDocuemnt(ByVal v_sContents As String, ByRef r_sDocXML As String) As Integer
        Dim lPos As Long
        Dim sTemp As String
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        lPos = InStrRev(r_sDocXML, "</w:t>")

        If lPos > 0 Then
            sTemp = Left$(r_sDocXML, lPos - 1)

            r_sDocXML = sTemp & v_sContents & Mid$(r_sDocXML, lPos)
        End If

        Return result
    End Function
    Private Function GetSubDocXmlContents(ByRef cInputFile As FileClass, ByRef sFile As String, ByVal v_bMegeStylesAlso As Boolean) As String

        Dim sSubDoc As String
        Dim oDoc As Xml.XmlDocument
        Dim oBodyNodeList As Xml.XmlNodeList
        Dim oDocStyleList As Xml.XmlNodeList
        Dim lPos As Long
        Dim sNodes As String
        Dim lPosEnd As Long
        Dim vParagraphArrayStyle As Object = Nothing

        Dim vParagraphArray() As Object = Nothing
        Dim sFile1 As String
        Dim lCnt As Long
        Dim bValidFile As Boolean
        Dim sTemp As String
        Dim bParagraphFound As Boolean
        Dim bIsLoopfile As Boolean
        Dim lSmartTagCnt As Integer
        Dim newXmlDeclaration As Xml.XmlDeclaration

        sSubDoc = ""

        GetSubDocXmlContents = ""
        'first check if there any data in sub document

        oDoc = New Xml.XmlDocument

        oDoc.PreserveWhitespace = False


        oDoc.Load(sFile)

        If oDoc.FirstChild.NodeType = System.Xml.XmlNodeType.XmlDeclaration Then
            newXmlDeclaration = oDoc.CreateXmlDeclaration("1.0", String.Empty, "yes")
            oDoc.ReplaceChild(newXmlDeclaration, oDoc.FirstChild)
        End If


        sFile1 = oDoc.OuterXml
        bValidFile = False
        bIsLoopfile = False

        bParagraphFound = False

        If InStr(1, sFile1, "<w:binData") > 0 Then
            bValidFile = True
        End If

        If InStr(1, sFile1, "<w:br w:type=") > 0 Then
            bValidFile = True
        End If

        If InStr(1, sFile, "loop_merge") > 0 Then
            bIsLoopfile = True
        End If

        m_lReturn = BreakStringIntoArray("<w:p", "</w:p>", sFile1, vParagraphArray)
        If IsArray(vParagraphArray) Then
            bParagraphFound = False
            For lCnt = 0 To UBound(vParagraphArray)
                sTemp = vParagraphArray(lCnt)
                If Convert.ToString(sTemp).Contains("<w:p") Then
                    bValidFile = True
                    bParagraphFound = True
                    Exit For
                End If
            Next
        End If

        If Not bParagraphFound Then
            v_bMegeStylesAlso = False
        End If

        'In any case loop file style should not be merged
        If bIsLoopfile Then
            v_bMegeStylesAlso = False
        End If

        If bValidFile Then
            'set styles
            If v_bMegeStylesAlso Then
                'Some smart tags
                oDocStyleList = oDoc.GetElementsByTagName("o:SmartTagType")

                sNodes = ""
                If oDocStyleList IsNot Nothing AndAlso oDocStyleList.Count > 0 Then

                    For lSmartTagCnt = 0 To oDocStyleList.Count - 1
                        sNodes = sNodes & oDocStyleList.Item(lSmartTagCnt).OuterXml
                    Next

                    m_lReturn = cInputFile.AddNodesInXML("<w:wordDocument", sNodes, g_sDocPreBodyFragment)

                End If

                'Styles
                oDocStyleList = oDoc.GetElementsByTagName("w:styles")

                If oDocStyleList IsNot Nothing AndAlso oDocStyleList.Count > 0 Then
                    sNodes = oDocStyleList.Item(0).OuterXml
                    lPos = InStr(1, sNodes, ">")
                    If lPos > 0 Then
                        sNodes = Mid$(sNodes, lPos + 1)

                        lPosEnd = InStrRev(sNodes, "<")

                        sNodes = Left$(sNodes, lPosEnd - 1)

                        m_lReturn = BreakStringIntoArray("<w:style", "</w:style>", sNodes, vParagraphArrayStyle)
                        sNodes = ""

                        For lCnt = 0 To UBound(vParagraphArrayStyle) Step 1
                            If Not vParagraphArrayStyle(lCnt).ToString.Contains("w:type=" & Chr(34) & "paragraph" & Chr(34)) OrElse Not vParagraphArrayStyle(lCnt).ToString.Contains("w:styleId=" & Chr(34) & "Normal" & Chr(34)) Then
                                sNodes = sNodes + vParagraphArrayStyle(lCnt)
                            Else
                                If Not g_sDocPreBodyFragment.Contains("w:type=" & Chr(34) & "paragraph" & Chr(34)) Then
                                    sNodes = sNodes + vParagraphArrayStyle(lCnt)
                                End If
                            End If
                        Next


                        m_lReturn = cInputFile.AddNodesInXML("<w:styles", sNodes, g_sDocPreBodyFragment)
                    End If
                End If
                'set styles of lists
                oDocStyleList = oDoc.GetElementsByTagName("w:fonts")

                If oDocStyleList IsNot Nothing AndAlso oDocStyleList.Count > 0 Then
                    sNodes = oDocStyleList.Item(0).OuterXml
                    lPos = InStr(1, sNodes, ">")

                    If lPos > 0 Then
                        sNodes = Mid$(sNodes, lPos + 1)

                        lPosEnd = InStrRev(sNodes, "<")

                        sNodes = Left$(sNodes, lPosEnd - 1)

                        m_lReturn = cInputFile.AddNodesInXML("<w:fonts", sNodes, g_sDocPreBodyFragment)
                    End If
                End If

                'set styles of lists
                oDocStyleList = oDoc.GetElementsByTagName("w:lists")
                If oDocStyleList IsNot Nothing AndAlso oDocStyleList.Count > 0 Then
                    sNodes = oDocStyleList.Item(0).OuterXml
                    lPos = InStr(1, sNodes, ">")

                    If lPos > 0 Then
                        'First check if the lists tag are available in main doc header fragment

                        If InStr(1, g_sDocPreBodyFragment, "w:lists") > 0 Then
                            sNodes = Mid$(sNodes, lPos + 1)

                            lPosEnd = InStrRev(sNodes, "<")

                            sNodes = Left$(sNodes, lPosEnd - 1)

                            m_lReturn = cInputFile.AddNodesInXML("<w:lists", sNodes, g_sDocPreBodyFragment)
                        Else

                            m_lReturn = cInputFile.AddNodesInXML("<w:wordDocument", sNodes, g_sDocPreBodyFragment)
                        End If
                    End If
                End If

            End If

            oBodyNodeList = oDoc.GetElementsByTagName("w:body")

            If oBodyNodeList IsNot Nothing AndAlso oBodyNodeList.Count > 0 Then
                m_lReturn = cInputFile.GetNodeXML(oDoc.OuterXml, "<w:body", "</w:body>", sSubDoc)
                lPos = InStr(1, sSubDoc, ">")

                If lPos > 0 Then
                    sSubDoc = Mid$(sSubDoc, lPos + 1)
                End If

                lPos = InStrRev(sSubDoc, "</w:body>")

                If lPos > 0 Then
                    sSubDoc = Left$(sSubDoc, lPos - 1)
                End If
            Else
                m_lReturn = cInputFile.GetNodeXML(oDoc.OuterXml, "<wx:sect", "</wx:sect>", sSubDoc)

            End If

            'remove node  </w:sectPr>
            Dim oListStr As New List(Of String)
            BreakStringIntoList("<w:sectPr", "</w:sectPr>", sSubDoc, oListStr)

            sSubDoc = ""
            If oListStr.Count > 0 Then
                For iListCnt As Integer = 0 To oListStr.Count - 1
                    Dim bAddContent As Boolean = True

                    If oListStr.Item(iListCnt).StartsWith("<w:sectPr") Then
                        If IsEmptyString(oListStr.Item(iListCnt)) Then
                            bAddContent = False
                        End If
                    End If

                    If bAddContent Then
                        sSubDoc = sSubDoc & oListStr.Item(iListCnt)
                    End If

                Next
            End If

            oListStr = Nothing
        End If

        GetSubDocXmlContents = sSubDoc

        oBodyNodeList = Nothing
        oDoc = Nothing


    End Function

    Private Function EnsureClientDirectoryClear() As Integer
        Dim result As Integer = 0

        '  Dim lAnswer As DialogResult 'MKW100703 PN5298 1.6.9 to 1.8.6 Catchup
        Dim sMessage As String = "" 'MKW100703 PN5298 1.6.9 to 1.8.6 Catchup



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lFileNumber <> 0 Then
            Return result
        End If

        ' SET 12/10/2004 ISS15027 - use the common function
        If (NoOfFilesInDirectory(v_sDirectoryName:=m_sClient)) > 0 Then

            ' Only show message box, if we are running in client
            If Not m_bRunningOnServer AndAlso m_sCCMDocProduction <> "1" Then

                'MKW100703 PN5298 START 1.6.9 to 1.8.6 Catchup
                'DJM 08/04/2003 : Ask user if they have other documents open.
                sMessage = "Files have been found in your temp directory." & Strings.ChrW(13) & Strings.ChrW(10)
                sMessage = sMessage & "Do you have any other documents open?"

                'Saj240224
                'lAnswer = MessageBox.Show(sMessage, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                'If lAnswer = System.Windows.Forms.DialogResult.Yes Then
                '    sMessage = "This document will not be opened." & Strings.ChrW(13) & Strings.ChrW(10)
                '    sMessage = sMessage & "Close down the open document to be able to open this one."
                '    MessageBox.Show(sMessage, "Cancelling letter", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    Return gPMConstants.PMEReturnCode.PMFalse
                'End If
            End If
            'MKW100703 PN5298 END 1.6.9 to 1.8.6 Catchup

            'If files exist in our processing directory remove them
            ' SET 12/10/2004 ISS15027 - use the common function
            m_lReturn = ClearDirectory(m_sClient)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete Client Work Directory [" & m_sClient & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="EnsureClientDirectoryClear", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        End If

        m_lReturn = SetZipDirectory()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = ClearDirectory(m_sZIP_DIRECTORY)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete DocZipPMDir Directory [" & m_sZIP_DIRECTORY & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="EnsureClientDirectoryClear", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

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
    ' History: 01/09/2000 RWH - Created.
    ' RAM20040301    - Bug fix for PN Issue 10231
    '                  1. Removed unwanted Dir Command as it locks the directory
    '                  2. Removed RmDir and modify code to use CreateFolderTree
    '                  3. Modified code to use CopyFolder rather than individual
    '                       filecopy (name function)
    '                  4. Modified code to use IsFileExists function, instead of Dir
    ' ***************************************************************** '
    Private Function UpdateTemplateNumberAndDependencies(ByRef sPath As String, ByRef lOldId As Integer, ByRef lNewId As Integer) As Integer
        Dim result As Integer = 0
        Dim sOldClient, sNewClient, sParentFile, sDependencyDir, sNewDependencyDir As String
        Dim iFileNumIn, iFileNumOut As Integer
        Dim sTempFile, sCurrentLine As String
        Dim iPos As Integer
        Dim sReplacement, sXML_ListFile As String

        Dim sOldDocRef, sFileName As String



        result = gPMConstants.PMEReturnCode.PMTrue

        'DJM 24/04/2002 : If by co-incident the numbers are already the same then don't do anything.
        If lOldId = lNewId Then
            Return result
        End If

        sOldDocRef = "Doc%20" & lOldId
        sReplacement = "Doc%20" & lNewId

        'RWH(01/09/2000) - RSAIB Process 108. Rename template to correct
        'number now.
        sOldClient = sPath & "\Doc " & CStr(lOldId) & "." & m_sDocFileExtension

        sFileName = "Doc " & lNewId & "." & m_sDocFileExtension

        sNewClient = sPath & "\" & sFileName

        m_lReturn = DeleteClient(sPath, sFileName)

        Directory.Move(sOldClient, sNewClient)

        'Check for dependencies and rename directory.
        sParentFile = sOldClient.Substring(0, sOldClient.Length - 4)
        sDependencyDir = sParentFile & "_files"

        If IsFolderExists(sDependencyDir) = gPMConstants.PMEReturnCode.PMTrue Then

            'Create directory of correct name.
            sNewDependencyDir = sNewClient.Substring(0, sNewClient.Length - 4) & "_files"

            m_lReturn = CreateFolderTree(sNewDependencyDir, True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to create [" & sNewDependencyDir & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Move all dependencies to correct directory.
            m_lReturn = CopyFolder(sDependencyDir, sNewDependencyDir)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Copy Folder." & Chr(13) & Chr(10) &
                                   "From [" & sDependencyDir & "] " & Chr(13) & Chr(10) &
                                       "To [" & sNewDependencyDir & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Remove original directory.
            m_lReturn = DelDirectory(sDependencyDir)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete [" & sDependencyDir & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Analyse template source and replace references to old dependency directory with new.
            sTempFile = sNewClient.Substring(0, sNewClient.Length - 3) & "tmp"

            '        sReplacement = Left(sOldDocRef, Len(sOldDocRef) - 1) & m_lDocumentTemplateId

            iFileNumIn = FreeFile()
            System.IO.File.Open(iFileNumIn, sNewClient, OpenMode.Input)

            iFileNumOut = FreeFile()
            System.IO.File.Open(iFileNumOut, sTempFile, OpenMode.Output)

            Do While Not EOF(iFileNumIn)
                sCurrentLine = LineInput(iFileNumIn)
                iPos = (sCurrentLine.IndexOf(sOldDocRef) + 1)
                If iPos > 0 Then
                    sCurrentLine = sCurrentLine.Substring(0, iPos - 1) & sReplacement & Mid(sCurrentLine, iPos + (sOldDocRef.Length))
                End If

                PrintLine(iFileNumOut, sCurrentLine)

            Loop

            FileSystem.FileClose(iFileNumIn)
            FileSystem.FileClose(iFileNumOut)

            ' RAM20040301 : Bug fix for PN Issue 10231
            m_lReturn = bPMDocFunctions.CopyFile(sTempFile, sNewClient, True, True, m_sFileCopyMsg, v_bCalledFromBatchProcessing:=IsCalledFromBatchProcessing)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Copy File." &
                                   "Soure File [" & sTempFile & Chr(13) & Chr(10) &
                                   "Destination File [" & sNewClient & Chr(13) & Chr(10) &
                                       "Error Message : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Now do xml list file.
            sXML_ListFile = sNewDependencyDir & "\filelist.xml"

            sTempFile = sXML_ListFile.Substring(0, sXML_ListFile.Length - 3) & "tmp"
            'Make sure the XML File is there
            If IsFileExists(sXML_ListFile) = gPMConstants.PMEReturnCode.PMTrue Then

                iFileNumIn = FreeFile()
                System.IO.File.Open(iFileNumOut, sTempFile, OpenMode.Output)
                System.IO.File.Open(iFileNumIn, sXML_ListFile, OpenMode.Input)

                iFileNumOut = FreeFile()
                System.IO.File.Open(iFileNumOut, sTempFile, OpenMode.Output)

                Do While Not EOF(iFileNumIn)
                    sCurrentLine = LineInput(iFileNumIn)
                    iPos = (sCurrentLine.IndexOf(sOldDocRef) + 1)
                    If iPos > 0 Then
                        sCurrentLine = sCurrentLine.Substring(0, iPos - 1) & sReplacement & Mid(sCurrentLine, iPos + (sOldDocRef.Length))
                    End If

                    PrintLine(iFileNumOut, sCurrentLine)

                Loop

                FileSystem.FileClose(iFileNumIn)
                FileSystem.FileClose(iFileNumOut)

                ' RAM20040301 : Bug fix for PN Issue 10231
                m_lReturn = bPMDocFunctions.CopyFile(sTempFile, sXML_ListFile, True, True, m_sFileCopyMsg, v_bCalledFromBatchProcessing:=IsCalledFromBatchProcessing)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Copy File." &
                                       "Soure File [" & sTempFile & Chr(13) & Chr(10) &
                                       "Destination File [" & sXML_ListFile & Chr(13) & Chr(10) &
                                           "Error Message : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumberAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End If
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: CopyDocAndDependencies
    '
    ' Description: Copy's document file between specified source and
    '               destination directories. Then checks for dependencies
    '               and copies them, if found, retaining relative
    '               directory structure. Originally written for use
    '               with mailshot.
    '
    ' History: 05/09/2000 RWH - Created.
    ' RAM20040301 : Removed unwanted Dir Commands as it locks the directory
    ' ***************************************************************** '
    Private Function CopyDocAndDependencies(ByRef sSource As String, ByRef sDestination As String) As Integer
        Dim result As Integer = 0
        Dim sTemp As String = String.Empty
        Dim sParentFile As String
        Dim sDependencyDir As String
        Dim sTempDest As String
        Dim sDestinationFolder As String
        Dim sErrorMessage As String = String.Empty

        Dim sSourceFile, sDestinationFile As String
        Dim lFileCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'Make sure the directory's there
        m_lReturn = CreateFolderTree(sDestination)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Create the Directory. (" & sDestination & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDocAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        'Move source file to destination
        lFileCount = NoOfFilesInDirectory(sSource & "\", r_vFirstFileName:=sTemp)
        If lFileCount > 0 Then
            sSourceFile = sSource & "\" & sTemp
            sDestinationFile = sDestination & "\" & sTemp
            m_lReturn = bPMDocFunctions.CopyFile(v_sSourceFile:=sSourceFile, v_sDestinationFile:=sDestinationFile, v_bCalledFromBatchProcessing:=IsCalledFromBatchProcessing)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy " & sSourceFile & " to " & sSourceFile, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDocAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If
        End If

        'Check for dependencies and move them to the destination directory if they exist.
        sParentFile = sTemp.Substring(0, sTemp.Length - 4)

        sDependencyDir = sSource & "\" & sParentFile & "_files"

        If IsFolderExists(sDependencyDir) = gPMConstants.PMEReturnCode.PMTrue Then

            sTempDest = sDestination & "\" & sParentFile & "_files"

            m_lReturn = CreateFolderTree(sTempDest, True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Directory [" & sTempDest & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDocAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Use the CopyFolder Command instead of old PMFileCopy
            sDestinationFolder = sDestination & "\" & sParentFile & "_files\"
            m_lReturn = CopyFolder(v_sSourceFolder:=sDependencyDir, v_sDestinationFolder:=sDestinationFolder, v_bOverWriteFiles:=True, r_vErrorMessage:=sErrorMessage)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Copy Directory Contents" & Chr(13) & Chr(10) &
                                   "Source Directory [" & sDependencyDir & "]" & Chr(13) & Chr(10) &
                                   "Destination Directory [" & sDestinationFolder & "]" & Chr(13) & Chr(10) &
                                       "Error Message : " & sErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDocAndDependencies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: DeleteClient
    '
    ' Description: deletes the template from the client
    '
    ' Changes: RWH(01/08/2000) Ammended to deal with htm documents.
    '          RWH(31/08/2000) Remove dependencies if they exist.
    '          RAM20040301     Removed unwanted Dir Commands as it locks the directory
    ' ***************************************************************** '
    Private Function DeleteClient(ByRef sPath As String, ByRef sFileName As String) As Integer
        Dim result As Integer = 0
        Dim sParentFile, sDependencyDir As String



        result = gPMConstants.PMEReturnCode.PMTrue

        sParentFile = sPath & "\" & sFileName
        m_lReturn = DOCGeneralFunc.DeleteFile(v_sFileName:=sParentFile)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the file. (" & sParentFile & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        'Strip off extension.
        sFileName = sFileName.Substring(0, sFileName.Length - 4)

        'RWH(31/08/2000) RSAIB Process 108. Remove HTML dependencies.
        'Check for dependencies and remove them if they exist.
        sParentFile = sPath & "\" & sFileName
        sDependencyDir = sParentFile & "_files"

        m_lReturn = DelDirectory(sDependencyDir)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the Folder. (" & sDependencyDir & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessSubDocXml
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function ProcessSubDocXml(ByRef sSourceTemplate As String,
                                 ByRef sDestinationMerge As String,
                                 ByRef oTextStreamOut As Scripting.TextStream,
                                 ByRef cInputFile As FileClass,
                                 Optional ByVal vRiskId As Object = Nothing,
                                 Optional ByVal vInstanceArray() As Object = Nothing,
                                 Optional ByVal bInRiskLoop As Boolean = False,
                                 Optional ByVal v_sContentMergedWithFirstLine As String = "",
                                 Optional ByVal v_sContentMergedWithLastLine As String = "",
                                 Optional ByVal v_bMergeStyles As Boolean = False,
                                 Optional ByRef v_sCurrentLine As String = "", Optional ByVal sDocType As String = "", Optional ByVal sDocName As String = "",
                                 Optional ByVal sRiskDescription As String = "",
                                 Optional ByRef IsEmptyDoc As Boolean = False,
                                 Optional ByRef vRiskNumber As Integer = -1) As Integer


        Dim result As Integer = 0
        Dim sTmpFile As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        sTmpFile = m_sClient & "\" & sDestinationMerge

        ' Use CopyFile rather than PMFileCopy,
        m_sFileCopyMsg = ""
        m_lReturn = bPMDocFunctions.CopyFile(m_sClient & "\" & sSourceTemplate, sTmpFile, True, False, m_sFileCopyMsg, v_bCalledFromBatchProcessing:=IsCalledFromBatchProcessing)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file." & Chr(13) & Chr(10) &
                               "Source File   : " & m_sClient & "\" & sSourceTemplate & Chr(13) & Chr(10) &
                               "Target File   : " & sTmpFile & Chr(13) & Chr(10) &
                                   "Error Details : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSubDocXml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        m_lReturn = ResolveDocumentXML(sDestinationMerge,
                                          vInstanceArray:=vInstanceArray,
                                          vRiskId:=vRiskId, bInRiskLoop:=bInRiskLoop, v_iIsSubDoc:=gPMConstants.PMEReturnCode.PMTrue) ', iLoop)


        'ConvertDocumentUsingSiriusDocumentUtility(sTmpFile, sTmpFile)

        m_lReturn = InsertSubDocXML(sTmpFile, oTextStreamOut, cInputFile, v_sContentMergedWithFirstLine, v_sContentMergedWithLastLine, v_bMergeStyles, v_sCurrentLine, IsEmptyDoc, vRiskNumber)

        If m_sCCMDocProduction = "1" AndAlso (isKCMApplicableForSelectedDocument = "0" OrElse (Not String.IsNullOrEmpty(m_sCCMDocumentName))) Then
            If Not sSourceTemplate.Contains("loop_template.xml") Then
                CopySubDocToCCMFolder(sTmpFile, sSourceTemplate)
            End If
        End If

        ' RAM20040301 : Use FSO to delete the file, rather than Kill
        m_lReturn = DOCGeneralFunc.DeleteFile(sTmpFile)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete file [" & sTmpFile & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSubDocXml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If


        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetRiskTemplate
    '
    ' Description: Searches for file names in a defined order. If the
    '               first document is not found the search continues
    '               for the next, until the file is found, Currently,
    '               this is implemented to 2 levels.
    '
    ' History: 14/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GetRiskTemplate(ByRef sRiskCode As String, ByVal iRptPointer As Integer, ByRef lDocId As Integer, ByRef lDocType As Integer) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sDocCode As String = ""
        Dim lTempDocId, lTempDocType As Integer

        Const iNUMBER_OF_RISK_CODE_OPTIONS As Integer = 2

        For iSearchCount As Integer = 1 To iNUMBER_OF_RISK_CODE_OPTIONS
            Select Case (iSearchCount)
                Case 1
                    sDocCode = RiskDocPrefix & sRiskCode & CStr(iRptPointer)
                Case 2
                    sDocCode = RiskDocPrefix & sRiskCode
            End Select
            Dim bFoundInCache As Boolean = False
            'Look for Template in List
            For Each obj As DocumentTemplateInfo In m_oDocumentTemplateInfo
                If (obj.DocCode = sDocCode) Then
                    lDocId = obj.DocTemplateId
                    lDocType = obj.DocType
                    bFoundInCache = True

                    If lDocId <> 0 Then
                        Return 1
                    End If

                    Exit For
                End If
            Next
            If (m_dtDocEffectiveDate = Date.MinValue) Then
                m_lReturn = m_oBusiness.GetPolicyEffectiveDate(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_dtEffectiveDate:=m_dtDocEffectiveDate)
            End If

            If bFoundInCache = False Then
                'Get type and id of template.
                m_lReturn = m_oBusiness.GetTemplateFromCode(sCode:=sDocCode, lDocId:=lTempDocId, lDocType:=lTempDocType, v_dtEffectiveDate:=m_dtDocEffectiveDate) 'Plico 21

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lTempDocId <> 0 Then
                    lDocId = lTempDocId
                    lDocType = lTempDocType
                    Dim oDocTemplateInfo As New DocumentTemplateInfo
                    oDocTemplateInfo.DocCode = sDocCode
                    oDocTemplateInfo.DocTemplateId = lTempDocId
                    oDocTemplateInfo.DocType = lTempDocType
                    m_oDocumentTemplateInfo.Add(oDocTemplateInfo)
                    Exit For
                Else
                    Dim oDocTemplateInfo As New DocumentTemplateInfo
                    oDocTemplateInfo.DocCode = sDocCode
                    oDocTemplateInfo.DocTemplateId = 0
                    oDocTemplateInfo.DocType = 0
                    m_oDocumentTemplateInfo.Add(oDocTemplateInfo)
                End If
            End If

        Next iSearchCount

        Return result

    End Function



    ' ***************************************************************** '
    '
    ' Name: SetWordVersionDependentVariables
    '
    ' Description: To deal with different versions of word which may
    '               behave very differently and store documents in
    '               differing formats.
    '
    ' History: 18/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function SetWordVersionDependentVariables() As Integer

        Dim result As Integer = PMEReturnCode.PMTrue

        m_sWordVersion = "15"
        m_sDocFileExtension = "xml"
        m_sFieldStartMarker = "&lt;@"
        m_sFieldEndMarker = "@&gt;"

        m_iFieldMarkerLength = m_sFieldStartMarker.Length

        m_sHyperLinkFieldStartMarker = "%3c@"
        m_sHyperLinkFieldEndMarker = "@%3e"

        m_iHyperLinkFieldMarkerLength = m_sHyperLinkFieldStartMarker.Length

        Return result

    End Function

    Private Function SetZipDirectory() As Integer

        Dim result As Integer = 0



        Dim sTemp As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_sZIP_DIRECTORY <> "" Then
            Return result
        End If


        'Get the DocZipTemp dir
        sTemp = gPMFunctions.ToSafeString(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\PureInstallation\Client", "DocZipPMDir"))

        'Make sure we have an install path
        If sTemp = "Not Found" Then
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetZipDirectory", vErrNo:=Information.Err().Number, vErrDesc:="Unable to find the registry entry for the doczip directory location")
            Return gPMConstants.PMEReturnCode.PMError
        End If

        Dim sUniqueTemp As String = ""
        'GetUnique Name as It got fail in concurrent Execution for same client.
        bPMDocFunctions.GetUniqueName(sUniqueTemp)

        m_sZIP_DIRECTORY = sTemp
        m_sZIP_DIRECTORY = m_sZIP_DIRECTORY & "\" & m_sUsername.Trim() & "\" & sUniqueTemp

        ' directory doesn't exist so attempt to create it
        m_lReturn = CreateFolderTree(m_sZIP_DIRECTORY, True)

        ' did we succeed...?
        If IsFolderExists(m_sZIP_DIRECTORY) <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetZipDirectory", vErrNo:=0, vErrDesc:="Unable to create the directory (" & m_sZIP_DIRECTORY & ")")
            Return gPMConstants.PMEReturnCode.PMError
        End If

        Return result


    End Function


    Private Function Pad(ByVal sValue As String, ByVal lLeft As Integer, Optional ByVal sPadStr As String = "*", Optional ByVal lMax As Integer = 80) As String

        If sPadStr <> "" Then
            'either pad left or right
            If lLeft = 0 Then
                Return (sValue & New String(sPadStr, lMax)).Substring(0, lMax)
            Else
                Return (New String(sPadStr, lMax) & sValue).Substring((New String(sPadStr, lMax) & sValue).Length - lMax)
            End If
        Else
            Return sValue
        End If
    End Function


    '*****************************************************
    ' Name : RemoveFormatTagsFromOurTags
    '
    ' Desc : Filters any format tags (<...>) from between our tags (<@...@>)
    '
    ' Edit History :
    '
    ' DJM 08/07/2002 : Fixed so that it works for loops.
    ' DJM 02/07/2002 : Created.
    '
    '*****************************************************
    Private Function RemoveFormatTagsFromOurTags(ByRef r_sLine As String) As Integer
        Dim result As Integer = 0
        Dim sFieldCode As String = ""

        Dim lFieldEnd, lFieldLen As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sLine As String = r_sLine

        Dim lFieldStart As Integer = (sLine.IndexOf(m_sFieldStartMarker) + 1)

        Do While lFieldStart > 0

            lFieldEnd = InStr(lFieldStart, sLine, m_sFieldEndMarker)

            If lFieldEnd = 0 Then
                Exit Do
            End If

            lFieldStart += m_iFieldMarkerLength
            lFieldLen = lFieldEnd - lFieldStart
            sFieldCode = sLine.Substring(lFieldStart - 1, System.Math.Min(sLine.Length, lFieldLen))

            m_lReturn = RemoveFormatTags(sFieldCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                sLine = sLine.Substring(0, lFieldStart - 1) & sFieldCode & sLine.Substring(lFieldEnd - 1)
            End If

            lFieldStart = InStr(lFieldStart + 1, sLine, m_sFieldStartMarker)

        Loop

        r_sLine = sLine

        Return result
    End Function

    '*****************************************************
    ' Name : RemoveFormatTags
    '
    ' Desc : Filters any format tags (<...>)
    '
    '*****************************************************
    Private Function RemoveFormatTags(ByRef r_sFieldCode As String) As Integer
        Dim result As Integer = 0
        Dim lFormatStart, lFormatEnd, lFormatLength As Integer
        Dim sFormatString As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sFieldCode As String = r_sFieldCode

        Do While sFieldCode.IndexOf("<"c) >= 0
            lFormatStart = (sFieldCode.IndexOf("<"c) + 1)
            lFormatEnd = (sFieldCode.IndexOf(">"c) + 1)
            lFormatLength = (lFormatEnd - lFormatStart) + 1
            If lFormatEnd = 0 Then
                sFormatString = sFieldCode.Substring(lFormatStart - 1, (sFieldCode.Length - lFormatStart + 1))
            Else
                sFormatString = sFieldCode.Substring(lFormatStart - 1, System.Math.Min(sFieldCode.Length, lFormatLength))
            End If

            sFieldCode = sFieldCode.Replace(sFormatString, "")
        Loop

        r_sFieldCode = sFieldCode.Trim()

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckFileType
    '
    ' Description: Opens file downloaded from server & checks first 2
    '               bytes to ensure this is a standard .doc document
    '               This is to see whether it is necessary to carry
    '               out a conversion.
    '
    ' History: 29/08/2000 RWH - Created.
    ' RAM20040227    - Removed unwanted Dir Commands, Since it lock the directory.
    '                  Ref. PN Issue 10231
    ' ***************************************************************** '
    Private Function CheckFileTypeIsDoc(Optional ByVal bWantXmlFormat As Boolean = False) As Integer
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
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Too Many Files in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            Case 0
                ' No Files Found
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            Case 1
                ' Only one file found, so check the type of the file

                lFileNum = FileSystem.FreeFile()
                FileSystem.FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)
                sLine = FileSystem.InputString(lFileNum, 5)
                FileSystem.FileClose(lFileNum)
                Select Case sLine.ToUpper()
                    Case "<?XML"
                        If Not (bWantXmlFormat) Then result = gPMConstants.PMEReturnCode.PMTrue
                            'Do Nothing
                    Case "<HTML"
                        If bWantXmlFormat Then result = gPMConstants.PMEReturnCode.PMFalse
                    Case Else
                        If sLine.StartsWith("﻿") Then
                            'File is UTF-8 Encoded.
                            FileSystem.FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)
                            sLine = FileSystem.InputString(lFileNum, 8)
                            FileSystem.FileClose(lFileNum)
                            If Mid(sLine, 4, 5).ToUpper() = "<?XML" Then
                                If Not (bWantXmlFormat) Then result = gPMConstants.PMEReturnCode.PMTrue
                            ElseIf Mid(sLine, 4, 5).ToUpper() = "<HTML" Then
                                If bWantXmlFormat Then result = gPMConstants.PMEReturnCode.PMFalse
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
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
        End Select

        Return result

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
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Too Many Files in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            Case 0
                ' No Files Found
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            Case 1
                ' Only one file found, so check the type of the file

                lFileNum = FileSystem.FreeFile()
                FileSystem.FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)
                sLine = FileSystem.InputString(lFileNum, 9)
                FileSystem.FileClose(lFileNum)
                Select Case sLine.Substring(0, 5).ToUpper()
                    Case "<HTML"
                        result = gPMConstants.PMEReturnCode.PMTrue
                    Case Else
                        If Mid(sLine, 4, 5).ToUpper() = "<HTML" Then result = gPMConstants.PMEReturnCode.PMTrue 'If File is Encoded using UTF-8
                End Select

            Case Else
                ' Some other no, so error

                ' No Files Found
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
        End Select

        Return result

    End Function
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
    ' History: 04/09/2000 RWH - Created.
    ' RAM20040227    - Removed unwanted Dir Command, since it locks the directory.
    '                  Ref. PN Issue 10231
    ' ***************************************************************** '
    Private Function CheckFileHasCorrectName(ByRef sPath As String, ByVal lCorrectFileNumber As Integer) As Integer
        Dim result As Integer = 0
        Dim sFileName As String = String.Empty
        Dim sCorrectedFile As String = String.Empty
        Dim lFilesCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get current file name.
        lFilesCount = NoOfFilesInDirectory(v_sDirectoryName:=sPath, r_vFirstFileName:=sFileName)

        If sFileName <> "" Then
            sCorrectedFile = "Doc " & lCorrectFileNumber & ".doc"
            Directory.Move(sPath & "\" & sFileName, sPath & "\" & sCorrectedFile)
        End If

        Return result

    End Function

    ' Open a document, resolve the fields, and return the
    ' document object
    Private Function ConvertDocument(ByRef oDocument As Object, ByVal lFileNo As Integer) As Integer

        Dim result As Integer = 0
        Dim oBookmark As Object
        Dim sFieldCode, sNewMergeField As String
        Dim iSep As Integer
        Dim sFieldType As String = String.Empty
        Dim sFieldName As String = String.Empty
        Dim sQuestion As String = String.Empty

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

        ' RAM20040301 : Use m_sClient Direcotry, rather than the m_sZIP_Directory
        Dim sFileName As String = m_sClient & "\" & "Doc " & CStr(lFileNo) & ".doc"

        ' Open the chosen template document

        oDocument = m_oWord.Documents.Open(ToSafeString(sFileName))


        ' Get the active window

        Dim oActiveWindow As Object = oDocument.ActiveWindow

        ' Get the bookmarks collection

        Dim oBookmarks As Object = oDocument.Bookmarks


        If oBookmarks.Count = 0 Then
            oBookmarks = Nothing
            oActiveWindow = Nothing
            Return result
        End If

        ' Reget the bookmarks collection

        oBookmarks = oDocument.Bookmarks


        If oBookmarks.Count = 0 Then
            oBookmarks = Nothing
            oActiveWindow = Nothing
            Return result
        End If

        ' Load the bookmarks into an array
        For Each oBookmark2 As Object In oBookmarks
            oBookmark = oBookmark2

            ' Get the field code for the bookmark

            sFieldCode = oBookmark.name

            ' Determine the field type
            iSep = (sFieldCode.IndexOf("_"c) + 1)
            If iSep > 0 Then
                sFieldType = sFieldCode.Substring(0, iSep - 1)
            End If

            ' Select the bookmark so it can be overwritten.

            oBookmark.Select()


            Select Case sFieldType
                Case DbTag
                    ' extract the field name
                    sFieldName = sFieldCode.Substring(sFieldCode.Length - (sFieldCode.Length - iSep))

                    ' Strip off the file name at the beginning
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
                    sNewMergeField = c_sFieldStartMarker & DbTag & Separator & sFieldName & c_sFieldEndMarker


                    oActiveWindow.Selection = sNewMergeField

                Case QuestionTag


                    sQuestion = oActiveWindow.Selection

                    'Construct new merge field
                    sNewMergeField = c_sFieldStartMarker & QuestionTag & Separator & sQuestion & c_sFieldEndMarker


                    oActiveWindow.Selection = sNewMergeField

                Case LoopTag
                    ' extract the field name
                    sFieldName = sFieldCode.Substring(sFieldCode.Length - (sFieldCode.Length - iSep))

                    ' Strip off the file name at the beginning
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
                    sNewMergeField = c_sFieldStartMarker & LoopTag & Separator & sFieldName & c_sFieldEndMarker


                    oActiveWindow.Selection = sNewMergeField


                Case EndLoopTag
                    ' extract the field name
                    sFieldName = sFieldCode.Substring(sFieldCode.Length - (sFieldCode.Length - iSep))

                    ' Strip off the file name at the beginning
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


                    oActiveWindow.Selection = sNewMergeField


                Case Else
                    'oBookmark.Range.Text = "Invalid Bookmark"

            End Select

        Next oBookmark2

        'Update the fields
        ' Alix
        'ActiveDocument.Fields.Update

        oActiveWindow.Document.Application.ActiveDocument.Fields.Update()

        ' Return the document


        Return result

Err_ConvertDocument:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SaveDocumentAsHTML
    '
    ' Description:
    '
    ' History: 25/08/2000 RWH - Created.
    ' RA
    ' ***************************************************************** '
    Public Function SaveDocumentAsHTML(ByRef oTemplate As Object, ByVal lFileNo As Integer) As Integer
        Dim result As Integer = 0
        Dim sFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAM20040301 : Use m_sClient Direcotry, rather than the m_sZIP_DIRECTORY
            sFileName = m_sClient & "\" & "Doc " & CStr(lFileNo) & ".htm"


            oTemplate.SaveAs(ToSafeString(sFileName), 8)

            oTemplate.Close()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveDocumentAsHTML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveDocumentAsHTML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : EvaluateVariableExpression
    ' Description   : Function to evaluate the Variable Expression
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.1 Text Variables
    ' Notes         : There are 2 possible situations
    '                 1. <@VAR_n = expression@> ' An expression or string value is assigned to a variable
    '                 2. <@VAR_n@>              ' Print the content of the variable
    '
    ' Sample Inputs : 1. <@VAR_1 = <@FUNC_LCase("ABC")@>@>
    '                 2. <@VAR_1@>
    ' Edit History  :
    ' RAM20030411   : Created
    ' RVH20041109   : Pass file class rather than file number
    ' ***************************************************************** '
    Private Function EvaluateVariableExpression(ByVal v_vStrInput As String, ByRef cInputFile As FileClass) As String

        Dim result As String = String.Empty
        'Chr(147)
        Const constEQStartValue As String = "="""
        Const constEQStartExpression As String = "=&"
        Const constEQStartExpressionVariable As String = "=&lt;@VAR_"
        Const constEQStartExpressionFunction As String = "=&lt;@FUNC"
        Const constEQStartExpressionVariableParsed As String = "=VAR_"
        Const constEQStartExpressionFunctionParsed As String = "=FUNC"


        Dim lReturn As Integer
        Dim lStrInputLength As Integer
        Dim strTemp As String
        Dim iVariableStart, iVariableNumber As Integer
        Dim strVariableName As String = ""
        Dim vVariableValue As String = ""

        Dim bComplete As Boolean
        Dim sNextLine As String = ""
        Dim iCount, iFieldEnd As Integer

        EvaluateVariableExpression = ""



        ' Remove all spaces.    ' Don't do that, since if we have a string arguments,
        '   it will remove the spaces in that too
        'v_vStrInput = Replace$(v_vStrInput, " ", "")

        ' so we need to remove only the white spaces
        v_vStrInput = RemoveWhiteSpaces(v_vStrInput)

        lStrInputLength = v_vStrInput.Length

        ' We got nothing to process, so quit
        If lStrInputLength = 0 Then
            Return ""
        End If

        'we've got a line which has on it either <@VAR_1@>, which means its
        'value is returned, or we've got <@VAR_1 = <@....@>...@>
        'So we can have nested start and end tokens, and we need to split out what's between
        'the first and last, noting that we could have more stuff following

        ' We may also have situations like
        ' VAR_1=<@FUNC_Lcase("Testing")@>

        If v_vStrInput.StartsWith("VAR_") Then
            iVariableStart = 1

            ' The Format tags are already removed
            strTemp = v_vStrInput
        Else

            ' Extract the Variable Name
            iVariableStart = (v_vStrInput.IndexOf(m_sFieldStartMarker) + 1)
            If iVariableStart > 0 Then

                strTemp = v_vStrInput.Substring(iVariableStart - 1)

                bComplete = False
                Do While Not bComplete
                    For iTemp As Integer = 1 To strTemp.Length - m_sFieldStartMarker.Length + 1
                        If strTemp.Substring(iTemp - 1, System.Math.Min(strTemp.Length, m_sFieldStartMarker.Length)) = m_sFieldStartMarker Then
                            iCount += 1
                        End If

                        If strTemp.Substring(iTemp - 1, System.Math.Min(strTemp.Length, m_sFieldEndMarker.Length)) = m_sFieldEndMarker Then
                            iCount -= 1
                            If iCount = 0 Then
                                iFieldEnd = iTemp
                                bComplete = True
                                Exit For
                            End If
                        End If
                    Next iTemp

                    If Not bComplete Then
                        'It's split over lines, and so let's get the next one and start again...
                        'There'll be nothing else on one of these lines, so we're ok doing this
                        ' RVH 09/11/2004: (Performance) Use passed file class
                        m_lReturn = cInputFile.GetNextLine(sNextLine)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Error fall out of do....loop loop
                            bComplete = True
                        End If
                        strTemp = strTemp & sNextLine.Trim()
                        v_vStrInput = v_vStrInput & sNextLine.Trim()
                        iCount = 0
                    End If
                Loop

                'What's between the tokens?
                If iFieldEnd <> 0 Then
                    strTemp = strTemp.Substring(m_sFieldStartMarker.Length, System.Math.Min(strTemp.Length, iFieldEnd - m_sFieldEndMarker.Length - 1))
                End If
                m_lReturn = RemoveFormatTagsFromOurTags(strTemp)
            Else
                iVariableStart = (v_vStrInput.IndexOf("VAR_") + 1)
                If iVariableStart < 1 Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Input for Function EvaluateVariableExpression : " & v_vStrInput, vApp:=ACApp, vClass:=ACClass, vMethod:="EvaluateVariableExpression", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' The Format tags are already removed
                strTemp = v_vStrInput

            End If

        End If

        'So now we've got it, let's find out what the variable is...
        'Strip off the leading VAR_
        strTemp = strTemp.Substring(4)

        'Fetch the Variable number
        Dim dbNumericTemp As Double
        Do While Double.TryParse(strTemp.Substring(0, 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)
            iVariableNumber = iVariableNumber * 10 + CInt(strTemp.Substring(0, 1))
            strTemp = strTemp.Substring(1)
            If strTemp = "" Then
                Exit Do
            End If
        Loop

        ' We got the Variable Name
        strVariableName = "VAR_" & iVariableNumber

        ' We have to get the Variable Value
        ' Note : The Variable declaration might be in the following pattern
        '        a) <@VAR_1="abcdef"@>              : Expression to set value to the variable
        '        b) <@VAR_1@>                       : Expression to get the value of the variable
        '        c) <@VAR_1=<@FUNC_UCASE("Test")@>@>: Function Expression to set the value

        ' We need to send the value of the expression back
        If strTemp = "" Then
            vVariableValue = GetVariableValue(sName:=strVariableName)
        Else
            strTemp = Trim(strTemp.ToString())
            Select Case strTemp.Substring(0, 2)
                Case constEQStartValue
                    ' =�Sample Value�  = We need to remove those =? and last ?
                    vVariableValue = strTemp.Substring(2, strTemp.Length - 3)

                    ' We got the Variable Number (which is an index to the Variables Array
                    lReturn = SetVariableValue(sName:=strVariableName, vValue:=vVariableValue)
                    Return result

                Case constEQStartExpression
                    ' we have some other format
                    ' Check if we have <@VAR_1@>


                    Select Case strTemp.Substring(0, 10)
                        Case constEQStartExpressionVariable

                            ' Note : This is going to be a Recursive Call
                            ' RVH 09/11/2004: (Performance) Pass file class
                            vVariableValue = EvaluateVariableExpression(strTemp, cInputFile)

                            ' We got the Variable Number (which is an index to the Variables Array
                            lReturn = SetVariableValue(sName:=strVariableName, vValue:=vVariableValue)
                            Return result

                        Case constEQStartExpressionFunction

                            ' RVH 09/11/2004: (Performance) Pass file class
                            vVariableValue = EvaluateFunctionExpression(strTemp, cInputFile)

                            ' We got the Variable Number (which is an index to the Variables Array
                            lReturn = SetVariableValue(sName:=strVariableName, vValue:=vVariableValue)
                            Return result
                    End Select

                Case Else
                    ' We may have the thing parsed already ie. without the Tags &lt;@
                    Select Case strTemp.Substring(0, 5)
                        Case constEQStartExpressionVariableParsed

                            ' Remove the first char =
                            strTemp = strTemp.Substring(1)

                            ' Note : This is going to be a Recursive Call
                            ' RVH 09/11/2004: (Performance) Pass file class
                            vVariableValue = EvaluateVariableExpression(strTemp, cInputFile)



                            ' We got the Variable Number (which is an index to the Variables Array
                            lReturn = SetVariableValue(sName:=strVariableName, vValue:=vVariableValue)
                            Return result

                        Case constEQStartExpressionFunctionParsed

                            ' Remove the first char =
                            strTemp = strTemp.Substring(1)

                            ' RVH 09/11/2004: (Performance) Pass file class
                            vVariableValue = EvaluateFunctionExpression(strTemp, cInputFile)

                            ' We got the Variable Number (which is an index to the Variables Array
                            lReturn = SetVariableValue(sName:=strVariableName, vValue:=vVariableValue)
                            Return result

                    End Select

            End Select
        End If


        Return vVariableValue

    End Function


    ' ***************************************************************** '
    ' Name          : EvaluateFunctionExpression
    ' Description   : Function to evaluate the Function
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Notes         : We need to evaluate a functions described as shown below
    '                 <@FUNC_functype(param1, param2,...)@>
    ' Sample Inputs : 1. <@FUNC_UCase(<@VAR_1@>)@>
    '                 2. <@FUNC_UCase("abc")@>
    ' Edit History  :
    ' RAM20030416   : Created
    ' PW240604 - CQ5678 - add byref parameter, to indicate if function
    '                     result is a numeric value
    ' RVH20041109   : Pass file class rather than file number
    ' ***************************************************************** '
    Private Function EvaluateFunctionExpression(ByVal v_vStrInput As String, ByVal cInputFile As FileClass, Optional ByRef r_bResultIsNumeric As Boolean = False) As String
        Dim result As String = String.Empty
        Dim lStrInputLength As Integer
        Dim strTemp As String = ""
        Dim iFunctionStart As Integer
        Dim strFunctionName As String = ""
        Dim vParamArray() As Object = Nothing

        Dim bComplete As Boolean
        Dim sNextLine As String = ""
        Dim iCount, iFieldEnd As Integer
        Dim strOutput As String = ""

        EvaluateFunctionExpression = ""



        result = ""

        ' Remove all spaces.    ' Don't do that, since if we have a string arguments,
        '   it will remove the spaces in that too
        'v_vStrInput = Replace$(v_vStrInput, " ", "")

        ' so we need to remove only the white spaces, not with in the string
        v_vStrInput = RemoveWhiteSpaces(v_vStrInput)

        lStrInputLength = v_vStrInput.Length

        ' We got nothing to process, so quit
        If lStrInputLength = 0 Then
            Return ""
        End If

        'we've got a line which has on it either <@FUNC_functionName(argx,,,,)@>, which means its
        'value is returned, or we've got <@FUNC_functionName(<@VAR_x@>,...@>
        'So we can have nested start and end tokens, and we need to split out what's between
        'the first and last, noting that we could have more stuff following

        ' We may also have situations like
        ' FUNC_functionName(<@VAR_n@>)
        ' i.e The Format tags are already removed
        If v_vStrInput.StartsWith("FUNC_") Then
            iFunctionStart = 1
            strTemp = v_vStrInput

            ' Remove the m_sFieldStartMarker ( &lt;@  )
            'strTemp = Replace(strTemp, m_sFieldStartMarker, "")

            ' Remove the  m_sFieldEndMarker (  @&gt;  )
            'strTemp = Replace(strTemp, m_sFieldEndMarker, "")
        Else
            ' Extract the Function Name  : If we have <@ Tags
            iFunctionStart = (v_vStrInput.IndexOf(m_sFieldStartMarker) + 1)
            If iFunctionStart > 0 Then

                strTemp = v_vStrInput.Substring(iFunctionStart - 1)

                bComplete = False
                Do While Not bComplete
                    For iTemp As Integer = 1 To strTemp.Length - m_sFieldStartMarker.Length + 1
                        If strTemp.Substring(iTemp - 1, System.Math.Min(strTemp.Length, m_sFieldStartMarker.Length)) = m_sFieldStartMarker Then
                            iCount += 1
                        End If

                        If strTemp.Substring(iTemp - 1, System.Math.Min(strTemp.Length, m_sFieldEndMarker.Length)) = m_sFieldEndMarker Then
                            iCount -= 1
                            If iCount = 0 Then
                                iFieldEnd = iTemp
                                bComplete = True
                                Exit For
                            End If
                        End If
                    Next iTemp

                    If Not bComplete Then
                        'It's split over lines, and so let's get the next one and start again...
                        'There'll be nothing else on one of these lines, so we're ok doing this
                        ' RVH 09/11/2004: (Performance) Use file class
                        m_lReturn = cInputFile.GetNextLine(sNextLine)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Error fall out of do....loop loop
                            bComplete = True
                        End If
                        strTemp = strTemp & sNextLine.Trim()
                        v_vStrInput = v_vStrInput & sNextLine.Trim()
                        iCount = 0
                    End If
                Loop

                'What's between the tokens?
                If iFieldEnd <> 0 Then
                    strTemp = strTemp.Substring(m_sFieldStartMarker.Length, System.Math.Min(strTemp.Length, iFieldEnd - m_sFieldEndMarker.Length - 1))
                End If
                m_lReturn = RemoveFormatTagsFromOurTags(strTemp)
            Else
                iFunctionStart = (v_vStrInput.IndexOf("FUNC_") + 1)
                If iFunctionStart < 1 Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Input for Function EvaluateFunctionExpression : " & v_vStrInput, vApp:=ACApp, vClass:=ACClass, vMethod:="EvaluateFunctionExpression", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
                ' The Format tags are already removed
                strTemp = v_vStrInput
            End If
        End If

        m_lReturn = ParseFunctionNameAndParams(v_vInputString:=strTemp, r_sFunctionName:=strFunctionName, r_vParamArray:=vParamArray)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            ' We got the function name and arguments
            ' so call the builtin functions
            ' PW240604 - CQ5678 - pass through flag which will return indicator
            ' of whether function result is to be treated as numeric

            strOutput = CallBuiltinFunction(v_sFunctionName:=strFunctionName, v_vParamArray:=vParamArray, r_bResultIsNumeric:=r_bResultIsNumeric)
        Else
            strOutput = ""
            Return result
        End If

        ' Return the Value

        Return strOutput

    End Function


    Private Function RemoveEmptyParagraphsXML(ByRef r_sLine As String,
                                              Optional ByVal v_bCheckAllLines As Boolean = False,
                                              Optional ByVal v_bRemoveFirstAndLastLineContentsOnly As Boolean = False) As Integer
        Dim lCnt As Long
        Dim sTemp As String
        Dim vParagarphArray() As Object
        Dim sReturnLine As String = ""
        Dim sProperTagName As String


        RemoveEmptyParagraphsXML = gPMConstants.PMEReturnCode.PMTrue

        vParagarphArray = Nothing

        If Len(Trim$(r_sLine)) > 0 Then
            m_lReturn = BreakStringIntoArray("<w:p", "</w:p>", r_sLine, vParagarphArray)
        End If

        If Not IsArray(vParagarphArray) Then
            Exit Function
        End If

        sProperTagName = GetTagProperName("<w:p", r_sLine)

        If sProperTagName.Length > 0 Then
            For lCnt = 0 To UBound(vParagarphArray)

                sTemp = ToSafeString(vParagarphArray(lCnt))

                If v_bRemoveFirstAndLastLineContentsOnly Then
                    If lCnt = 0 Or lCnt = UBound(vParagarphArray) Then
                        If Left$(sTemp, Len(sProperTagName)) = sProperTagName Then
                            If Not IsEmptyString(sTemp) Or InStr(1, sTemp, "<w:br w:type=") > 0 Or InStr(1, sTemp, "<w:binData") > 0 Then
                                sReturnLine = sReturnLine & sTemp
                            End If
                        Else
                            sReturnLine = sReturnLine & sTemp
                        End If
                    Else
                        sReturnLine = sReturnLine & sTemp
                    End If

                Else
                    If Left$(sTemp, Len(sProperTagName)) = sProperTagName Then
                        If Not IsEmptyString(sTemp) Or InStr(1, sTemp, "<w:br w:type=") > 0 Or InStr(1, sTemp, "<w:binData") > 0 Then
                            sReturnLine = sReturnLine & sTemp
                        End If
                    Else
                        sReturnLine = sReturnLine & sTemp
                    End If
                End If
            Next


            r_sLine = sReturnLine
        End If

    End Function


    Private Function InitialiseParameterXMLDOM() As Integer
        If m_oXMLDOM Is Nothing Then
            m_oXMLDOM = New XmlDocument() ' create a new instance of DOM
            With m_oXMLDOM

                'developer guide no. 22(no solution)
                '.async = "false"

                '.setProperty("SelectionNamespaces", "xmlns:xsl='http://www.w3.org/1999/XSL/Transform'")


                ' TODO
                '.setProperty("SelectionLanguage", "XPath")
                .PreserveWhitespace = False

                ' TODO
                '.resolveExternals = False

                ' TODO
                '.validateOnParse = False
            End With
        End If
        ' Load the XML
        Try
            m_oXMLDOM.LoadXml(m_sParameterXML)

        Catch
        End Try
        Return 1
    End Function

    Private Function TerminateParameterXMLDOM() As Integer
        If m_oXMLDOM Is Nothing Then
        Else
            m_oXMLDOM = Nothing
        End If
        Return 1
    End Function

    ' ***************************************************************** '
    ' Name          : ExtractParameterDetails
    ' Description   : Function to extract Parameter details (from the ParameterArray)
    ' Note          : 1. The parameter array is an XML String
    '                 2. v_iCurrentNestingLevel will be affect the final formatting of value
    '                 2. r_sValue is a string (for all type of data)
    '                 3. r_iType will be one of the value defined in PMEFormatStyle Enum
    ' Edit History  :
    ' RAM20050121   : Created
    ' ***************************************************************** '
    Private Function ExtractParameterDetails(ByVal v_iCurrentNestingLevel As Integer, ByVal v_sName As String, ByRef r_sValue As String, ByRef r_iType As Integer) As Integer

        '   /PARAMETERS/PARAMETER[@name="xxxxx"]    where xxxx should be replated with the v_sName

        Dim result As Integer = 0
        Const XPathSearhExpressionStart As String = "/PARAMETERS/PARAMETER[@Name="
        Const XPathSearhExpressionEnd As String = "]"

        Const ACXMLAttribValueCol As Integer = 1
        Const ACXMLAttribTypeCol As Integer = 2


        Dim objNodeList As XmlNodeList = Nothing
        Dim objNode As XmlNode

        Dim strXPathSearhExpression, sValue As String
        Dim iType As gPMConstants.PMEFormatStyle

        Dim strErrorMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if we have an XML, if so then used xml functions to get the data
            m_sParameterXML = m_sParameterXML.Trim()
            If m_sParameterXML.Length <= 0 Then
                ' The parameter array is empty, but we have a parameter tag, so default to
                ' empty string ��
                sValue = Chr(147).ToString() & Chr(148).ToString()
            Else

                ' First get the value from the XML String

                ' NOTE  : The ParameterXML is a string contains XML in the following format.
                ' NOTE  : IMPORTANT . The Attributes PARAMETERS, PARAMETER, Name, Value, FormatType are case sensitive)
                '<PARAMETERS>
                '<PARAMETER Name="start_date" Value="01/01/2004" FormatType="10" />
                '<PARAMETER Name="end_date" Value="01/01/2005" FormatType="10" />
                '<PARAMETER Name="showsection01" Value="False" FormatType="13" />
                '<PARAMETER Name="name" Value="Client Name" FormatType="0" />
                '<PARAMETER Name="number" Value="5" FormatType="12" />
                '</PARAMETERS>

                ' Build the xpath Search Expression
                strXPathSearhExpression = XPathSearhExpressionStart & Chr(34).ToString() & v_sName & Chr(34).ToString() & XPathSearhExpressionEnd

                objNodeList = m_oXMLDOM.DocumentElement.SelectNodes(strXPathSearhExpression)
                If (objNodeList Is Nothing) Or (objNodeList.Count <= 0) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound ' Element Not Found
                    strErrorMessage = "Parameter Missing in m_sParameterXML [" & v_sName & "]"
                    Throw New Exception()
                End If

                ' We found an element. There should be always ONLY One here, so use the first item
                objNode = objNodeList.Item(0)

                ' Get the Value

                sValue = objNode.Attributes.Item(ACXMLAttribValueCol).Value
                sValue = sValue.Trim()

                iType = CType(objNode.Attributes.Item(ACXMLAttribTypeCol).Value, gPMConstants.PMEFormatStyle)

                ' clear the memory
                objNode = Nothing
                objNodeList = Nothing

                ' Format the value according the the format type, so that this can be inserted into the document
                If v_iCurrentNestingLevel > 1 Then
                    Select Case iType
                        Case gPMConstants.PMEFormatStyle.PMFormatString
                            sValue = Chr(147).ToString() & sValue & Chr(148).ToString()
                        Case gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatDateShort, gPMConstants.PMEFormatStyle.PMFormatDayOnlyLong, gPMConstants.PMEFormatStyle.PMFormatDayOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatDayOnlyShort, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyLong, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyShort, gPMConstants.PMEFormatStyle.PMFormatDateYearOnly
                            If Information.IsDate(sValue) Then
                                sValue = Chr(147).ToString() & CDate(sValue).ToString("dd-MMM-yyyy") & Chr(148).ToString()
                            Else
                                sValue = Chr(147).ToString() & (#12/29/1899#).ToString("dd-MMM-yyyy") & Chr(148).ToString()
                            End If
                        Case gPMConstants.PMEFormatStyle.PMFormatDateTimeLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium, gPMConstants.PMEFormatStyle.PMFormatDateTimeShort
                            If Information.IsDate(sValue) Then
                                sValue = Chr(147).ToString() & CDate(sValue).ToString("dd-MMM-yyyy HH:MM:ss") & Chr(148).ToString()
                            Else
                                sValue = Chr(147).ToString() & (#12/29/1899#).ToString("dd-MMM-yyyy HH:MM:ss") & Chr(148).ToString()
                            End If
                        Case gPMConstants.PMEFormatStyle.PMFormatBoolean
                            If sValue = "1" Or sValue = "True" Or gPMFunctions.ToSafeBoolean(sValue) Then
                                sValue = Chr(147).ToString() & "Yes" & Chr(148).ToString()
                            Else
                                sValue = Chr(147).ToString() & "No" & Chr(148).ToString()
                            End If
                        Case Else
                            If sValue = "" Then
                                sValue = "0"
                            End If
                    End Select
                Else
                    Select Case iType
                        Case gPMConstants.PMEFormatStyle.PMFormatLong
                        Case gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatDateShort, gPMConstants.PMEFormatStyle.PMFormatDateTimeLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium, gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, gPMConstants.PMEFormatStyle.PMFormatDayOnlyLong, gPMConstants.PMEFormatStyle.PMFormatDayOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatDayOnlyShort, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyLong, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyShort, gPMConstants.PMEFormatStyle.PMFormatDateYearOnly
                            If Information.IsDate(sValue) Then
                                If CDate(sValue) = #12/29/1899# Then
                                    sValue = ""
                                Else
                                    sValue = gPMFunctions.FormatField(iFormatType:=iType, vFieldValue:=sValue)
                                End If
                            Else
                                sValue = ""
                            End If

                        Case gPMConstants.PMEFormatStyle.PMFormatBoolean
                            If sValue = "1" Or sValue = "True" Or gPMFunctions.ToSafeBoolean(sValue) Then
                                sValue = "Yes"
                            Else
                                sValue = "No"
                            End If
                        Case Else
                            sValue = gPMFunctions.FormatField(iFormatType:=iType, vFieldValue:=sValue)
                    End Select
                End If
            End If

            ' Return the values
            r_sValue = sValue
            r_iType = iType

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Clear the memory
            If objNodeList Is Nothing = False Then
                objNodeList = Nothing
            End If

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractParameterDetails Failed. v_sName = " & v_sName & Chr(13) & Chr(10) & strErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractParameterDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function RemoveSpellingAndGrammerTagsXML(ByRef r_sLine As String) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sLine As StringBuilder = New StringBuilder(r_sLine)
        If (sLine.ToString.Contains("</w:t></w:r><w:proofErr w:type")) Then
            sLine.Replace("</w:t></w:r><w:proofErr w:type=""spellStart""/><w:r><w:t>", "")
            sLine.Replace("</w:t></w:r><w:proofErr w:type=""gramStart""/><w:r><w:t>", "")
            sLine.Replace("</w:t></w:r><w:proofErr w:type=""spellEnd""/><w:r><w:t>", "")
            sLine.Replace("</w:t></w:r><w:proofErr w:type=""gramEnd""/><w:r><w:t>", "")

            r_sLine = sLine.ToString
        End If
        ' Do any tidy up, e.g. Set x = Nothing here
        Return result

    End Function

    Private Function GetUserSignatureFile(ByRef r_sFileFullPath As String) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        r_sFileFullPath = ""

        m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="Signatures", r_sSettingValue:=r_sFileFullPath)

        r_sFileFullPath = r_sFileFullPath & m_sUsername

        Return result
    End Function


    Public Function ConvertHTMLToPDF(ByVal sInputFileName As String, ByRef r_sOutputFilename As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ConvertHtmlToPdf"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (System.IO.File.Exists(sInputFileName)) Then
                Return result
            End If

            r_sOutputFilename = Path.GetDirectoryName(sInputFileName)
            If Not r_sOutputFilename.Trim().EndsWith("\") Then r_sOutputFilename = r_sOutputFilename & "\"
            r_sOutputFilename = r_sOutputFilename & Path.GetFileNameWithoutExtension(sInputFileName) & ".PDF"

            If sInputFileName = r_sOutputFilename Then
                Return result
            End If

            m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(sInputFileName, r_sOutputFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If
            Return result

        Catch ex As Exception
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '        Return result

            ' This is for debugging only
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function ConvertDocumentUsingSiriusDocumentUtility(ByVal v_sSourceDocument As String, ByVal v_sDestDocument As String) As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oConvert As SiriusDocumentUtility.Document
        Try

            oConvert = New SiriusDocumentUtility.Document()
            oConvert.Convert(v_sSourceDocument, v_sDestDocument, m_sCCMDocumentName)
        Catch ex As Exception
            result = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ConvertDocumentUsingSiriusDocumentUtility Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertDocumentUsingSiriusDocumentUtility", excep:=ex)
        Finally
            oConvert = Nothing
        End Try
        Return result
    End Function
    Public Function PrintDocumentUsingSiriusDocumentUtility(ByVal v_sSourceDocument As String, ByVal v_sPrinterName As String) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "PrintDocumentUsingSiriusDocumentUtility"
        Dim oConvert As SiriusDocumentUtility.Document
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oConvert = New SiriusDocumentUtility.Document()
            oConvert.PrintDocument(v_sSourceDocument, v_sPrinterName)

            Return result
        Catch ex As Exception
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)

        Finally
            oConvert = Nothing
        End Try
        Return result
    End Function
    Public Function DocumentTitleCheckUsingSiriusDocumentUtility(ByVal v_sDocument As String) As Integer
        Dim result As Integer = 0
        Dim oConvert As Object
        Const kMethodName As String = "DocumentTitleCheckUsingSiriusDocumentUtility"

        Dim lInputFileNum, lFileLength As Integer
        Dim sAllFile As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lInputFileNum = FileSystem.FreeFile()
            FileSystem.FileOpen(lInputFileNum, v_sDocument, OpenMode.Input)
            lFileLength = FileSystem.LOF(lInputFileNum)
            sAllFile = FileSystem.InputString(lInputFileNum, lFileLength)
            FileSystem.FileClose(lInputFileNum)
            If sAllFile.IndexOf("<o:Title>", StringComparison.CurrentCultureIgnoreCase) >= 0 Then

                oConvert = New SiriusDocumentUtility.Document()

                oConvert.ClearDocumentTitlePropertyIfMergeCode(ToSafeString(v_sDocument))

            End If

            Return result
        Catch ex As Exception
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)

        Finally
            oConvert = Nothing
        End Try
        Return result
    End Function
    Public Function PrintDoc(ByVal sInputFileName As String, ByVal v_sPrinterName As String) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const kMethodName As String = "PrintDoc"


        Try

            If Not (System.IO.File.Exists(sInputFileName)) Then
                Return result
            End If

            m_lReturn = PrintDocumentUsingSiriusDocumentUtility(sInputFileName, v_sPrinterName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If
            Return result
        Catch ex As Exception
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)
        Finally
        End Try
        Return result
    End Function
    Public Function ConvertHTMLToDOC(ByVal sInputFileName As String, ByVal v_sOutputFilename As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ConvertHtmlToDoc"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (System.IO.File.Exists(sInputFileName)) Then
                Return result
            End If

            If sInputFileName = v_sOutputFilename Then
                Return result
            End If

            m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(sInputFileName, v_sOutputFilename)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ConvertDocumentUsingSiriusDocumentUtility", "Failed to convert Document")
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)

        Finally
        End Try
        Return result
    End Function

    Private Function FixBrokenMarkers(ByRef cInputFile As FileClass, ByRef r_sCurrentLine As String) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        Dim sFirstHalfOfMarker, sSecondHalfOfMarker As String
        Dim lStartOfMarker, lNextCharacter As Integer
        Dim sNextLine As String = ""
        Dim bTagsExists As Boolean
        Dim sTempLine As String = ""

        For lMarkerLoop As Integer = 1 To 2

            If lMarkerLoop = 1 Then
                sFirstHalfOfMarker = "&lt;"
                sSecondHalfOfMarker = "@"
            Else
                sFirstHalfOfMarker = "@"
                sSecondHalfOfMarker = "&gt;"
            End If

            lStartOfMarker = 0

            Do While InStr(lStartOfMarker + 1, r_sCurrentLine, sFirstHalfOfMarker) > 0

                'Find the first half of the marker.
                lStartOfMarker = InStr(lStartOfMarker + 1, r_sCurrentLine, sFirstHalfOfMarker)
                lNextCharacter = lStartOfMarker + sFirstHalfOfMarker.Length
                bTagsExists = False

                If lNextCharacter > r_sCurrentLine.Length Then
                    If Not cInputFile.EOF Then
                        m_lReturn = cInputFile.GetNextLine(sNextLine)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Do
                        End If
                        r_sCurrentLine = r_sCurrentLine & sNextLine
                        m_lLineCount += 1
                    Else
                        Exit Do
                    End If
                End If

                'Loop through any tags between the first half of the marker and the next character.
                Do While Mid(r_sCurrentLine, lNextCharacter, 1) = "<"

                    bTagsExists = True

                    lNextCharacter = InStr(lNextCharacter, r_sCurrentLine, ">") + 1

                    Do While lNextCharacter = 0
                        If Not cInputFile.EOF Then
                            m_lReturn = cInputFile.GetNextLine(sNextLine)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Exit Do
                            End If
                            r_sCurrentLine = r_sCurrentLine & sNextLine
                            m_lLineCount += 1
                        Else
                            Exit Do
                        End If
                        lNextCharacter = InStr(lNextCharacter, r_sCurrentLine, ">") + 1
                    Loop

                    If lNextCharacter = 0 Then
                        Exit Do
                    ElseIf lNextCharacter > r_sCurrentLine.Length Then
                        If Not cInputFile.EOF Then
                            m_lReturn = cInputFile.GetNextLine(sNextLine)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Exit Do
                            End If
                            r_sCurrentLine = r_sCurrentLine & sNextLine
                            m_lLineCount += 1
                        Else
                            Exit Do
                        End If
                    End If

                Loop

                'If tags exist between the two halves of the marker then bring them together.
                If bTagsExists And Mid(r_sCurrentLine, lNextCharacter, sSecondHalfOfMarker.Length) = sSecondHalfOfMarker Then
                    sTempLine = r_sCurrentLine

                    If lMarkerLoop = 1 Then
                        r_sCurrentLine = sTempLine.Substring(0, lStartOfMarker - 1)
                        r_sCurrentLine = r_sCurrentLine & Mid(sTempLine, lStartOfMarker + sFirstHalfOfMarker.Length, lNextCharacter - lStartOfMarker - sFirstHalfOfMarker.Length)
                        lStartOfMarker = r_sCurrentLine.Length + 1
                        r_sCurrentLine = r_sCurrentLine & sFirstHalfOfMarker
                        r_sCurrentLine = r_sCurrentLine & Mid(sTempLine, lNextCharacter)
                    Else
                        r_sCurrentLine = sTempLine.Substring(0, lStartOfMarker + sFirstHalfOfMarker.Length - 1)
                        lStartOfMarker = r_sCurrentLine.Length + 1 - sFirstHalfOfMarker.Length
                        r_sCurrentLine = r_sCurrentLine & sSecondHalfOfMarker
                        r_sCurrentLine = r_sCurrentLine & Mid(sTempLine, lStartOfMarker + sFirstHalfOfMarker.Length, lNextCharacter - lStartOfMarker - sFirstHalfOfMarker.Length)
                        r_sCurrentLine = r_sCurrentLine & Mid(sTempLine, lNextCharacter + sSecondHalfOfMarker.Length)
                    End If
                End If
            Loop
        Next

        Return result
    End Function

    Private Function FixIfStatement(ByRef r_sCurrentLine As String) As Integer

        Dim result As Integer = 0
        Dim lStartOfMarker, lNextCharacter As Integer
        Dim sTempLine As String = ""
        result = gPMConstants.PMEReturnCode.PMTrue

        'Fix IF statements.
        lStartOfMarker = 0
        Do While InStr(lStartOfMarker + 1, r_sCurrentLine, "&lt;@IF_") > 0
            lStartOfMarker = InStr(lStartOfMarker + 1, r_sCurrentLine, "&lt;@IF_")
            lNextCharacter = lStartOfMarker + 8

            Dim dbNumericTemp As Double
            Do While Double.TryParse(Mid(r_sCurrentLine, lNextCharacter, 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)
                lNextCharacter += 1
            Loop

            'Add extra space
            sTempLine = r_sCurrentLine
            r_sCurrentLine = sTempLine.Substring(0, lNextCharacter - 1)
            r_sCurrentLine = r_sCurrentLine & " "
            r_sCurrentLine = r_sCurrentLine & Mid(sTempLine, lNextCharacter)
        Loop

        Return result
    End Function

    Private Function FixSplitMergeCodes(ByRef r_sCurrentLine As String) As Integer

        Dim result As Integer = 0
        Dim lStartOfMergeCode, lEndOfMergeCode, lCurrentNestingLevel, lStartPos, lEndPos As Integer
        Dim sTempLine As String = ""
        Dim sSuffix As New StringBuilder

        result = gPMConstants.PMEReturnCode.PMTrue

        lStartOfMergeCode = 1
        Do While InStr(lStartOfMergeCode, r_sCurrentLine, m_sFieldStartMarker) > 0

            lStartOfMergeCode = InStr(lStartOfMergeCode, r_sCurrentLine, m_sFieldStartMarker)

            lCurrentNestingLevel = 1
            lStartPos = InStr(lStartOfMergeCode + 1, r_sCurrentLine, m_sFieldStartMarker)
            lEndPos = InStr(lStartOfMergeCode, r_sCurrentLine, m_sFieldEndMarker)
            Do While lCurrentNestingLevel > 0
                If lStartPos <> 0 And lStartPos < lEndPos Then
                    lCurrentNestingLevel += 1
                    lStartPos = InStr(lStartPos + 1, r_sCurrentLine, m_sFieldStartMarker)
                Else
                    lCurrentNestingLevel -= 1
                    lEndOfMergeCode = lEndPos + m_sFieldEndMarker.Length
                    lEndPos = InStr(lEndPos + 1, r_sCurrentLine, m_sFieldEndMarker)
                End If
            Loop

            sTempLine = r_sCurrentLine
            sSuffix = New StringBuilder("")
            lStartPos = lStartOfMergeCode + m_sFieldStartMarker.Length
            'Reset the current line to everything before the starttag
            r_sCurrentLine = sTempLine.Substring(0, lStartPos - 1)
            Do While InStr(lStartPos, sTempLine, "<") > 0 And InStr(lStartPos, sTempLine, "<") < lEndOfMergeCode
                lEndPos = InStr(lStartPos, sTempLine, "<")
                'Add the mergecode text before the tag to the current line.
                r_sCurrentLine = r_sCurrentLine & Mid(sTempLine, lStartPos, lEndPos - lStartPos)
                lStartPos = lEndPos
                lEndPos = InStr(lStartPos + 1, sTempLine, ">")
                'Add the tags to a temporary string.
                sSuffix.Append(Mid(sTempLine, lStartPos, lEndPos - lStartPos + 1))
                lStartPos = lEndPos + 1
            Loop

            If lEndOfMergeCode < lStartPos Then
                lEndOfMergeCode = lStartPos
            Else
                'Add the rest of the merge code text, upto and including the end marker.
                r_sCurrentLine = r_sCurrentLine & Mid(sTempLine, lStartPos, lEndOfMergeCode - lStartPos)
            End If

            'Add the tags that were inside the merge code.
            r_sCurrentLine = r_sCurrentLine & sSuffix.ToString()
            'Add the rest of the line.
            r_sCurrentLine = r_sCurrentLine & Mid(sTempLine, lEndOfMergeCode)

            lStartOfMergeCode = lEndOfMergeCode
        Loop


        ' Do any tidy up, e.g. Set x = Nothing here
        'Return result

        ' This is for debugging only
        ' Resume

        Return result
    End Function


    Private Function RemoveContentsOfTag(ByVal v_sStartTag As String,
                                     ByVal v_sEndTag As String,
                                     ByRef r_sLine As String,
                                     Optional ByVal v_bRemovalAll As Boolean = False) As Integer
        Dim lPos As Integer
        Dim sFirstPartOfLine As String
        Dim sEndPartOfLine As String
        Dim lTmpPos As Long
        Dim lEndTagPos As Long
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        If v_bRemovalAll Then
            lPos = InStr(1, r_sLine, v_sStartTag)
            lEndTagPos = InStr(1, r_sLine, v_sEndTag)

            If lEndTagPos > 0 Then
                If lEndTagPos < lPos Then
                    r_sLine = Mid$(r_sLine, lEndTagPos)
                End If
            End If
        End If

        lPos = 1
        Do While True
            lPos = InStr(lPos, r_sLine, v_sStartTag)

            If lPos > 0 Then
                sFirstPartOfLine = Left$(r_sLine, lPos - 1 + Len(v_sStartTag))
                sEndPartOfLine = Mid$(r_sLine, lPos + Len(v_sStartTag))

                lTmpPos = InStr(1, sEndPartOfLine, v_sEndTag)

                If lTmpPos > 1 Then
                    sEndPartOfLine = Mid$(sEndPartOfLine, lTmpPos)
                Else
                    'IF THERE IS NO XML TAG AND NO OUR MARKER THEN REMOVE THE WHOLE LINE
                    If v_bRemovalAll Then
                        If InStr(1, sEndPartOfLine, "<") = 0 Then
                            sEndPartOfLine = ""
                        End If
                    Else
                        If InStr(1, sEndPartOfLine, "<") = 0 Then
                            If InStr(1, sEndPartOfLine, m_sFieldStartMarker) = 0 And InStr(1, sEndPartOfLine, m_sFieldEndMarker) = 0 Then
                                sEndPartOfLine = ""
                            End If
                        End If
                    End If
                End If

                r_sLine = sFirstPartOfLine & sEndPartOfLine

                lPos = lPos + 1
            Else
                lPos = InStr(1, r_sLine, v_sStartTag)

                If lPos = 0 Then
                    lPos = InStr(1, r_sLine, v_sEndTag)
                    If lPos > 0 Then
                        r_sLine = Mid$(r_sLine, lPos)
                    End If
                End If

                Exit Do
            End If
        Loop
        Return result


    End Function


    Private Function GetFullLines(ByRef r_sStartingPartOfSplitLine As String,
                                  ByRef r_sEndingPartOfSplitLine As String) As Integer



        Dim sTempLineFragment As String
        Dim lPos As Long
        Dim sStartingLine As String
        Dim sEndingLine As String
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        sStartingLine = r_sStartingPartOfSplitLine
        sEndingLine = r_sEndingPartOfSplitLine

        'Starting Line
        lPos = InStr(1, r_sEndingPartOfSplitLine, "<")

        sTempLineFragment = r_sEndingPartOfSplitLine

        If lPos > 1 Then
            sTempLineFragment = Mid$(r_sEndingPartOfSplitLine, lPos)
        End If

        m_lReturn = RemoveContentsOfTag("<w:t>", "</w:t>", sTempLineFragment)

        m_lReturn = RemoveSpecificTags("<w:br", sTempLineFragment)

        sStartingLine = r_sStartingPartOfSplitLine & sTempLineFragment
        'end starting line

        'Ending Line
        sTempLineFragment = r_sStartingPartOfSplitLine

        lPos = InStrRev(r_sStartingPartOfSplitLine, ">")

        If lPos > 1 Then
            sTempLineFragment = Left$(r_sStartingPartOfSplitLine, lPos)
        End If

        m_lReturn = RemoveContentsOfTag("<w:t>", "</w:t>", sTempLineFragment)

        m_lReturn = RemoveSpecificTags("<w:br", sTempLineFragment)

        sEndingLine = sTempLineFragment & r_sEndingPartOfSplitLine
        'end end line

        r_sStartingPartOfSplitLine = sStartingLine
        r_sEndingPartOfSplitLine = sEndingLine
        Return result


    End Function

    Private Function RemoveSpecificTags(ByVal v_sTag As String, ByRef r_sString As String) As Integer
        Dim sProperTag As String
        Dim lPos As Integer
        Dim sRightChar As String
        Dim lEndPos As Long
        Dim sStringToRemove As String = Nothing
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        sProperTag = GetTagProperName(v_sTag, r_sString)

        If Len(sProperTag) > 0 Then

            sRightChar = Right$(sProperTag, 1)

            If sRightChar = ">" Then
                r_sString = Replace(r_sString, sProperTag, "")
            Else
                lPos = InStr(1, r_sString, sProperTag)

                If lPos > 0 Then
                    lEndPos = InStr(lPos, r_sString, ">")
                    If lEndPos > lPos Then
                        sStringToRemove = Mid$(r_sString, lPos, lEndPos - lPos + 1)
                    End If
                End If

                r_sString = Replace(r_sString, sStringToRemove, "")
            End If
        End If
    End Function
    Private Sub CreateTable()
        dtDocument = New DataTable
        dtDocument.Columns.Add("SORTDOCTYPE", Type.GetType("System.Int32"))
        dtDocument.Columns.Add("DOCTYPE", Type.GetType("System.String"))
        dtDocument.Columns.Add("DOCNAME", Type.GetType("System.String"))
        dtDocument.Columns.Add("MAINGROUP", Type.GetType("System.String"))
        dtDocument.Columns.Add("SUBGROUP", Type.GetType("System.String"))

        dtDocument.Columns.Add("LOOP1", Type.GetType("System.String"))
        dtDocument.Columns.Add("LOOP2", Type.GetType("System.String"))
        dtDocument.Columns.Add("LOOP3", Type.GetType("System.String"))

        dtDocument.Columns.Add("FIELDNAME", Type.GetType("System.String"))
        dtDocument.Columns.Add("VALUE", Type.GetType("System.String"))
        dtDocument.Columns.Add("RISKCNT", Type.GetType("System.Int32"))
        dtDocument.Columns.Add("RISKDescription", Type.GetType("System.String"))
        dtDocument.Columns.Add("ID", Type.GetType("System.Int32")) 'For Sorting the DataTable

    End Sub

    Private Function AddToTable(ByVal iDocTypeSort As Integer, ByVal sDocType As String, ByVal sDocName As String, ByVal sMainGroup As String,
                                ByVal sSubGroup As String,
                                ByVal sFieldName As String,
                                ByVal sValue As String,
                                Optional ByVal iRiskCnt As Integer = 0,
                                Optional ByVal sRiskDescription As String = "",
                                Optional ByVal sLoop1 As String = "",
                                Optional ByVal sLoop2 As String = "",
                                Optional ByVal sLoop3 As String = "") As Integer
        Static iID As Integer



        Dim dr As DataRow = dtDocument.NewRow
        dr("SORTDOCTYPE") = iDocTypeSort
        dr("DOCTYPE") = sDocType
        dr("DOCNAME") = sDocName
        dr("MAINGROUP") = sMainGroup
        dr("SUBGROUP") = sSubGroup
        dr("FIELDNAME") = sFieldName
        dr("VALUE") = sValue
        dr("LOOP1") = sLoop1
        dr("LOOP2") = sLoop2
        dr("LOOP3") = sLoop3

        If sMainGroup = "Risk" Or sMainGroup = "Reinsurance" Then
            iID = iID + 1
            dr("RISKCNT") = iRiskCnt
            dr("RISKDescription") = sRiskDescription
            dr("ID") = iID
        End If
        dtDocument.Rows.Add(dr)
        Return PMEReturnCode.PMTrue
    End Function


    Private Sub CreateXML(ByRef sOutPut As String)
        Dim sOldMainGroup As String = ""
        Dim sOldMainLoop1 As String = ""
        Dim sMainGroup As String = ""
        Dim sSubGroup As String = ""
        Dim sOldSubGroup As String = ""
        Dim sDocType As String = ""
        Dim sOldDocType As String = ""
        Dim sDocName As String = ""
        Dim sOldDocName As String = ""
        Dim sRiskDesc As String = ""
        Dim sFieldName As String = "", sValue As String = "", sLoop1 As String = ""
        Dim sLoop2 As String = "", sOldLoop2 As String = ""
        Dim sLoop3 As String = "", sOldLoop3 As String = ""
        Dim bAddMainGroup As Boolean = False
        Dim xmlDoc As New XmlDocument
        Dim docType As XmlElement = Nothing
        Dim oXEMainGroup As XmlElement = Nothing
        Dim oXEsubGroup As XmlElement = Nothing
        Dim oXELoop1 As XmlElement = Nothing
        Dim oXELoop2 As XmlElement = Nothing
        Dim oXELoop3 As XmlElement = Nothing

        Dim iRiskCnt As Integer, iOldRiskCnt As Integer
        Dim oXERoot As XmlElement = xmlDoc.CreateElement("ResolvedXML")
        xmlDoc.AppendChild(oXERoot)
        Dim i As Integer


        For Each row As DataRow In dtDocument.Select("", "SORTDOCTYPE,DOCTYPE,DOCNAME,ID,MAINGROUP,SubGroup")
            i = i + 1
            sDocType = row("DOCTYPE")
            sDocName = ReplaceSpecialCharacter(row("DOCNAME"))
            sMainGroup = ReplaceSpecialCharacter(row("MAINGROUP"))
            sFieldName = ReplaceSpecialCharacter(row("FIELDNAME"))
            sValue = row("VALUE")
            sSubGroup = ReplaceSpecialCharacter(row("SubGroup"))
            sLoop1 = ReplaceSpecialCharacter(row("LOOP1"))
            sLoop2 = ReplaceSpecialCharacter(row("LOOP2"))
            sLoop3 = ReplaceSpecialCharacter(row("LOOP3"))
            sRiskDesc = ToSafeString(row("RISKDescription"))
            iRiskCnt = ToSafeInteger((row("RISKCNT")))
            If sDocType <> sOldDocType And sDocType <> "" Or (sDocType = sOldDocType And sDocName <> sOldDocName) Then
                docType = xmlDoc.CreateElement("DocTemplate")
                addAttribute(docType, "Type", sDocType)
                addAttribute(docType, "Code", sDocName)
                oXERoot.AppendChild(docType)
                sOldDocType = sDocType
                sOldDocName = sDocName
                bAddMainGroup = True
            End If
            If sMainGroup <> "" And sSubGroup <> "" Then
                'If sMainGroup <> sOldMainGroup Or (sMainGroup <> "" And sLoop1 = "") Then
                If (sMainGroup <> sOldMainGroup) Or (iRiskCnt <> iOldRiskCnt) Or (bAddMainGroup) Then
                    oXEMainGroup = xmlDoc.CreateElement(sMainGroup)
                    If (iRiskCnt <> 0) Then
                        addAttribute(oXEMainGroup, "Risk_Cnt", Convert.ToString(iRiskCnt))
                        addAttribute(oXEMainGroup, "RiskDescription", sRiskDesc)
                    End If
                    If (docType IsNot Nothing) Then
                        docType.AppendChild(oXEMainGroup)
                    End If
                    sOldMainGroup = sMainGroup

                    sOldSubGroup = ""
                    sOldMainLoop1 = ""
                    sOldLoop2 = ""
                    sOldLoop3 = ""
                    iOldRiskCnt = iRiskCnt
                    oXELoop1 = Nothing
                    oXELoop2 = Nothing
                    oXELoop3 = Nothing
                End If





                If sSubGroup <> sOldSubGroup And sSubGroup <> "" Or (bAddMainGroup) Then
                    oXEsubGroup = xmlDoc.CreateElement(sSubGroup)
                    If (oXEMainGroup IsNot Nothing) Then
                        oXEMainGroup.AppendChild(oXEsubGroup)
                    End If
                    sOldSubGroup = sSubGroup
                    sOldLoop3 = ""
                    oXELoop1 = Nothing
                    oXELoop2 = Nothing
                    oXELoop3 = Nothing
                End If

                If sLoop1 <> sOldMainLoop1 And sLoop1 <> "" Or (sLoop2 = "" And sOldLoop2 <> sLoop2 And sLoop1 <> "") Or (sLoop2 = "" And sLoop1 = sOldMainLoop1 And sLoop1 <> "") Then
                    If sLoop1 <> sOldMainLoop1 And sLoop1 <> "" Then
                        oXELoop1 = Nothing
                    End If
                    If (oXELoop1 IsNot Nothing) Then
                        If (oXELoop1.Attributes(sFieldName) IsNot Nothing) Then
                            oXELoop1 = xmlDoc.CreateElement(sLoop1)
                            If (oXEsubGroup IsNot Nothing) Then
                                oXEsubGroup.AppendChild(oXELoop1)
                            End If
                            sOldMainLoop1 = sLoop1

                            sOldLoop2 = ""
                            sOldLoop3 = ""
                        End If
                    Else
                        oXELoop1 = xmlDoc.CreateElement(sLoop1)
                        If (oXEsubGroup IsNot Nothing) Then
                            oXEsubGroup.AppendChild(oXELoop1)
                        End If
                        sOldMainLoop1 = sLoop1

                        sOldLoop2 = ""
                        sOldLoop3 = ""
                    End If

                End If

                If sLoop2 <> sOldLoop2 And sLoop2 <> "" Or (sLoop3 = "" And sOldLoop3 <> sLoop3 And sLoop2 <> "") Or (sLoop3 = "" And sLoop2 = sOldLoop2 And sLoop2 <> "") Then
                    If sLoop2 <> sOldLoop2 And sLoop2 <> "" Then
                        oXELoop2 = Nothing
                    End If
                    If (oXELoop2 IsNot Nothing) Then
                        If (oXELoop2.Attributes(sFieldName) IsNot Nothing) Then
                            oXELoop2 = xmlDoc.CreateElement(sLoop2)
                            If (oXELoop1 IsNot Nothing) Then
                                oXELoop1.AppendChild(oXELoop2)
                            End If
                            sOldLoop2 = sLoop2
                            sOldLoop3 = ""
                        End If
                    Else
                        oXELoop2 = xmlDoc.CreateElement(sLoop2)
                        If (oXELoop1 IsNot Nothing) Then
                            oXELoop1.AppendChild(oXELoop2)
                        End If
                        sOldLoop2 = sLoop2
                        sOldLoop3 = ""
                    End If

                End If
                If sLoop3 <> "" Then
                    If oXELoop3 Is Nothing = False Then
                        If oXELoop3.Attributes(sFieldName) Is Nothing = False Then
                            oXELoop3 = xmlDoc.CreateElement(sLoop3)
                            If (oXELoop2 IsNot Nothing) Then
                                oXELoop2.AppendChild(oXELoop3)
                            End If
                            sOldLoop3 = sLoop3
                        End If
                    Else
                        oXELoop3 = xmlDoc.CreateElement(sLoop3)
                        If (oXELoop2 IsNot Nothing) Then
                            oXELoop2.AppendChild(oXELoop3)
                        End If
                        sOldLoop3 = sLoop3
                    End If
                End If
                If sLoop3 <> "" Then
                    If oXELoop3 IsNot Nothing Then
                        addAttribute(oXELoop3, sFieldName, sValue)
                    End If
                ElseIf sLoop2 <> "" Then
                    If oXELoop2 IsNot Nothing Then
                        addAttribute(oXELoop2, sFieldName, sValue)
                    End If
                ElseIf sLoop1 <> "" Then
                    If oXELoop1 IsNot Nothing Then
                        addAttribute(oXELoop1, sFieldName, sValue)
                    End If
                Else
                    If oXEsubGroup IsNot Nothing Then
                        addAttribute(oXEsubGroup, sFieldName, sValue)
                    End If
                End If
                bAddMainGroup = False
            End If
        Next
        sOutPut = xmlDoc.OuterXml
    End Sub


    Private Sub addAttribute(ByRef ele As XmlElement, ByVal sFieldName As String, ByVal sValue As String)
        ele.SetAttribute(sFieldName, sValue)
    End Sub
    Private Function ReplaceSpecialCharacter(ByVal sValue As String) As String
        Dim arrSpecialChar() As String = {".", ",", "<", ">", ":", "?", """", "/", "{", "[", "}", "]", "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "+", "=", "|", " ", "\"}
        Dim i As Integer
        For i = 0 To arrSpecialChar.Length - 1
            sValue = Replace(sValue, arrSpecialChar(i), "")
        Next
        Return sValue
    End Function


    ''''***************Below code come from  ExpressionParser ( as suggested by Deepak Arrora ) *******
    ''**************************************************************************************************
    'This is being Removed from ExpressionParser.vb to Interface.vb to avoid the merging issue dueto of it's shared momery behaviour.

    Private Structure T_VTREC ' Variable Table Record
        Dim name As String ' name of the variable
        Dim value As Object '
        Public Function CreateInstance() As T_VTREC
            Dim result As New T_VTREC
            result.name = String.Empty
            Return result
        End Function
    End Structure
    <ThreadStatic()> Private VT() As T_VTREC = Nothing ' Variable Table
    <ThreadStatic()> Private VTtop As Integer ' Variable Table's Upper bound

    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    '
    ' Name          : ExpressionParser
    '
    ' Description   : Bas Module to Evaluate Mathematical Expression in
    '                 Document Production
    '
    ' Author        : Ram Chandrabose
    '
    ' Edit History  :
    ' RAM20030415   : Created
    '
    ' ***************************************************************** '

    ' Private Const ACClass As String = "ExpressionParser"

    ' Merge Field markers (These are inserted in Word
    'as "<@" and "@>" but when viewed as flat text appear as
    ' "&lt;@" and "@&gt;" respectively).
    'Private m_sFieldStartMarker As String = ""
    'Private m_sFieldEndMarker As String = ""
    'Private m_iFieldMarkerLength As Integer


    ' ***************************************************************** '
    ' Name          : InitialiseExpressionParser
    ' Description   : Initialise Variable Table Array, and other initialisation
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Edit History  :
    ' RAM20030417   : Created
    ' *****************************************************************
    Public Function InitialiseExpressionParser() As Integer

        ' PW180803 - CQ1734 - only do this once for the life of the component
        ' to make variables truly global for the lifetime of the documents
        ' processing (including clauses/headers and footer)

        Dim result As Integer = 0
        Static bDone As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bDone Then Return result

            m_sFieldStartMarker = "&lt;@"
            m_sFieldEndMarker = "@&gt;"
            m_iFieldMarkerLength = m_sFieldStartMarker.Length

            VTtop = -1
            Erase VT

            bDone = True

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function


    Private Function RemoveDoubleQuotes(ByVal v_sValue As String) As String

        Try
            If ToSafeString(v_sValue).Substring(0, 1) = ChrW(34) Then
                v_sValue = Mid(v_sValue, 2)
            End If

            If ToSafeString(v_sValue).Substring(ToSafeString(v_sValue).Length - 1, 1) = ChrW(34) Then
                v_sValue = Mid(ToSafeString(v_sValue), 1, ToSafeString(v_sValue).Length - 1)
            End If

        Catch ex As Exception
            Return ""
        End Try
        Return v_sValue
    End Function

    Private Overloads Function GenerateDocumentPure() As Integer
        Return GenerateDocumentPure(Nothing)
    End Function

    ''' <summary>
    ''' Generate documents in Pure
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function GenerateDocumentPure(ByVal oSWArray As ArrayList) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sClient, sClientFile As String
        Dim iInstanceStart As Integer
        Dim sTempFileName As String
        Dim sDocFile As String
        Dim sDocCode As String = ""
        Dim sDocDesc As String = ""
        Dim oDoc As XmlDocument
        Dim lPos As Integer
        Dim oDocStyleList As XmlAttributeCollection
        Dim sDocPreBodyFragment As String = ""
        g_sDocPreBodyFragment = ""
        g_sDocEndBodyFragment = ""

        nResult = GetClientFolder()

        nResult = SetWordVersionDependentVariables()
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If m_lUniqueId > 0 Then
            m_sClient = ""
            nResult = GetClientFolder() ' Get the Unique work directory for this user


            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If m_lClaimCnt > 0 Then
            If m_lInsuranceFileCnt = 0 Then
                m_lInsuranceFileCnt = m_oBusiness.GetInsuranceFileCntFromClaim(m_lClaimCnt)
            End If
        End If

        iInstanceStart = FieldInstance1

        For iIndex As Integer = 0 To InstanceCount - 1

            iInstanceIndexArray(iIndex) = iInstanceStart
            iInstanceStart += 1
        Next

        'If we're doing mailshots we do something slightly different...
        If Information.IsArray(m_vMailshotArray) Then
            ' Clear client dir. for each case.
            nResult = EnsureClientDirectoryClear()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return ProcessMailshot()
        End If

        'If we're doing risks we do something slightly different...
        If Information.IsArray(m_vRiskArray) Then
            ' Clear client dir. for each case.
            nResult = EnsureClientDirectoryClear()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return ProcessRisksXML()
        End If

        'start -for processing of shared invoices
        If Information.IsArray(m_vPolicySharesArray) Then
            nResult = EnsureClientDirectoryClear()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return ProcessPolicyShares()
        End If

        m_oBusiness.FieldParameters = m_vFieldParams

        ' Load the fields into the Business object
        nResult = m_oBusiness.LoadFields()

        'DJM 29/05/2002 : If no template then use existing file in client dir.
        If (m_lDocumentTemplateId = 0 And m_lDocumentTypeId = 0) Or (m_lFileNumber <> 0) Then
            sClientFile = "Doc " & m_lFileNumber & "." & m_sDocFileExtension
        Else
            'Clear client dir. for each case.
            nResult = EnsureClientDirectoryClear()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sClientFile = "Doc " & m_lDocumentTemplateId & "." & m_sDocFileExtension

            If m_sCCMDocProduction = "0" Then
                nResult = CopyServerToClient(m_lDocumentTypeId, m_lDocumentTemplateId)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                If m_lDocumentTypeId = PMBConst.PMBClauseTextFile OrElse isKCMApplicableForSelectedDocument = "1" Then ''working of clauses will be same as Pure in CCM
                    nResult = CopyServerToClient(m_lDocumentTypeId, m_lDocumentTemplateId)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If
        End If

        sClient = m_sClient & "\" & sClientFile

        If m_sCCMDocProduction = "1" AndAlso isKCMApplicableForSelectedDocument = "0" Then
            If Not oSWArray Is Nothing Then
                If Not oSWArray Is Nothing Then
                    m_lReturn = DOCGeneralFunc.DeleteFile(sClient)
                    Dim oWordDocument As New Document()
                    Dim oBuilder As New DocumentBuilder(oWordDocument)
                    For Each sSWVal As String In oSWArray
                        oBuilder.Writeln(sSWVal)
                    Next
                    oWordDocument.Save(m_sClient & "\" & "Doc " & CStr(m_lDocumentTemplateId) & ".xml", SaveFormat.WordML)
                End If
            End If
        End If

        If m_bArchiveAsXML Then
            CreateTable()
            nResult = m_oBusiness.GetTemplateCodeAndDescFromID(sCode:=sDocCode, sDesc:=sDocDesc, lDocId:=m_lDocumentTemplateId)
        End If
        ' Now resolve the document
        nResult = ResolveDocumentXML(sClientFile, sDocName:=sDocCode)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'ConvertDocumentUsingSiriusDocumentUtility(sClient, sClient)

        oDoc = New XmlDocument
        oDoc.PreserveWhitespace = False
        Try
            oDoc.Load(sClient)
        Catch ex As Exception

            ConvertDocumentUsingSiriusDocumentUtility(sClient, sClient)
            oDoc.Load(sClient)
        End Try
        sDocFile = oDoc.OuterXml

        'Code to merge the the aliases of document and sub-document.(Start)
        lPos = InStr(1, g_sDocPreBodyFragment, "macrosPresent") - 3
        sDocPreBodyFragment = Left(g_sDocPreBodyFragment, lPos)
        oDocStyleList = oDoc.GetElementsByTagName("w:wordDocument").Item(0).Attributes

        For lSmartTagCnt As Integer = 0 To oDoc.GetElementsByTagName("w:wordDocument").Item(0).Attributes.Count - 1
            If Not g_sDocPreBodyFragment.Contains(Left(oDocStyleList.Item(lSmartTagCnt).OuterXml, Len(oDocStyleList.Item(lSmartTagCnt).OuterXml) - 5)) Then
                sDocPreBodyFragment = sDocPreBodyFragment & oDocStyleList.Item(lSmartTagCnt).OuterXml & " "
            End If

            If Right(oDocStyleList.Item(lSmartTagCnt).OuterXml, 5).Replace("""", "") = "yes" Then
                sDocPreBodyFragment = sDocPreBodyFragment.Replace(oDocStyleList.Item(lSmartTagCnt).OuterXml, oDocStyleList.Item(lSmartTagCnt).OuterXml)
            End If
        Next

        sDocPreBodyFragment = sDocPreBodyFragment & Right(g_sDocPreBodyFragment, g_sDocPreBodyFragment.Length - lPos)
        lPos = InStr(1, sDocFile, "wx:sect")
        If lPos = 0 Then
            lPos = InStr(1, sDocFile, "w:body")
            sDocPreBodyFragment = Replace(sDocPreBodyFragment, "<wx:sect>", "")
        End If

        lPos = InStr(lPos, sDocFile, ">")
        sDocFile = sDocPreBodyFragment & Mid$(sDocFile, lPos + 1)
        oDoc.LoadXml(sDocFile)
        oDoc.Save(sClient)
        oDoc = Nothing
        
        ConvertDocumentUsingSiriusDocumentUtility(sClient, sClient)

        sTempFileName = sClient
        If m_lMode = ACEmailMode And m_bSpoolAsHTML Then
            If sClient.ToUpper.EndsWith("XML") Then
                ' This document is to be sent as an email body, so this needs to be send back as HTM
                ' rather than XML
                sTempFileName = sClient.Remove(sClient.Length - 3) & "HTM"
                'Ensure Document Consistancy by saving with aspose.
                ConvertDocumentUsingSiriusDocumentUtility(sClient, sTempFileName)
            End If
            ' set the sTempFileName's extension to htm
            ' This is to send back the document as HTM
            m_sResolvedDocumentName = sTempFileName
        End If

        If m_bSpoolAsTXT Then
            sTempFileName = sClient.Remove(sClient.Length - 3) & "TXT"
            'Ensure Document Consistancy by saving with aspose.
            ConvertDocumentUsingSiriusDocumentUtility(sClient, sTempFileName)

            m_sResolvedDocumentName = sTempFileName

        End If
        'added if documentformat is pdf
        If m_bSpoolAsPDF Then
            sTempFileName = sClient.Remove(sClient.Length - 3) & "pdf"
            'Ensure Document Consistancy by saving with aspose.
            ConvertDocumentUsingSiriusDocumentUtility(sClient, sTempFileName)

            m_sResolvedDocumentName = sTempFileName

        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Generate doc in CCM based on system option
    ''' </summary>
    ''' <param name="sCCMDocumentName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerateDocumentCCM(ByVal sCCMDocumentName As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sResolvedXML As String = String.Empty
        Dim oDocTemplatePackList() As Byte = Nothing

        Dim dtCCMDocTemplateFields As New DataTable("DataCCMFields")
        dtCCMDocTemplateFields.Columns.Add("TableName", Type.GetType("System.String"))
        dtCCMDocTemplateFields.Columns.Add("ColumnName", Type.GetType("System.String"))

        nResult = GetDataForCCMDocument(sCCMDocumentName, dtCCMDocTemplateFields)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ''create datatable in required format
        CreateTableForCCM()

        Dim sSWRiskTag As String = ""
        Dim sSWPolicyTag As String = ""
        Dim bHasEndorsement As Boolean = False

        Dim dtCCMfieldsDetailsWithSpecialsType As New DataTable

        'Get the Special type (for Standard Wording only) 
        nResult = m_oBusiness.GetCCMFieldDetailsWithSpecialsType(dtFields:=dtCCMDocTemplateFields, r_dtResultSet:=dtCCMfieldsDetailsWithSpecialsType, InsuranceFileCnt:=m_lInsuranceFileCnt)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetCCMFieldDetailsWithSpecialsType failed.")
        End If

        Dim SpecialTypesRows() As DataRow = dtCCMfieldsDetailsWithSpecialsType.Select("specials_type = 5")
        If SpecialTypesRows.Length > 0 Then
            bHasEndorsement = True
        End If

        If bHasEndorsement Then
            Dim oSWArray As ArrayList = Nothing
            For Each drRow As DataRow In dtCCMfieldsDetailsWithSpecialsType.Rows
                If drRow("specials_type") = 5 Then
                    If drRow("TableName") = "PolicyStandardWordings" Then
                        oSWArray = New ArrayList
                        oSWArray.Add("<@STANDARDWORDINGS@>")
                        AddClauseToTableForCCM(PolicyStandardWordingTag)
                        GenerateStandardWordingDocuments(oSWArray)
                        oSWArray = Nothing
                    Else
                        Dim dtSubGrpDetails As New DataTable
                        GetSubGroupDetailsForEndorsement(drRow("TableName"), drRow("ColumnName"), dtSubGrpDetails)

                        For Each drSubGrp As DataRow In dtSubGrpDetails.Rows
                            Dim sSubGrp As String = ToSafeString(drSubGrp("sub_group"))
                            Dim sLoop1 As String = ToSafeString(drSubGrp("loop1"))
                            Dim sLoop2 As String = ToSafeString(drSubGrp("loop2"))
                            Dim sLoop3 As String = ToSafeString(drSubGrp("loop3"))
                            Dim sFieldName As String = ToSafeString(drSubGrp("field_name"))
                            Dim sDataModel As String = ToSafeString(drSubGrp("data_model"))
                            Dim nDataModelLength As Integer = sDataModel.Length + 1
                            oSWArray = New ArrayList
                            If String.IsNullOrEmpty(sLoop1) AndAlso String.IsNullOrEmpty(sLoop2) AndAlso String.IsNullOrEmpty(sLoop3) Then
                                oSWArray.Add("<@DB_" & sSubGrp.ToUpper() & "_" & sFieldName & "@> ")
                            ElseIf Not String.IsNullOrEmpty(sLoop1) AndAlso String.IsNullOrEmpty(sLoop2) AndAlso String.IsNullOrEmpty(sLoop3) Then
                                oSWArray.Add("<@LOOP_" & sLoop1 & "@> ")
                                oSWArray.Add("<@DB_" & sLoop1 & "_" & sFieldName & "@> ")
                                oSWArray.Add("<@ENDLOOP_" & sLoop1 & "@> ")
                            ElseIf Not String.IsNullOrEmpty(sLoop1) AndAlso Not String.IsNullOrEmpty(sLoop2) AndAlso String.IsNullOrEmpty(sLoop3) Then
                                oSWArray.Add("<@LOOP_" & sLoop1 & "@> ")
                                oSWArray.Add("<@LOOP_" & sLoop2 & "@> ")
                                oSWArray.Add("<@DB_" & sLoop2 & "_" & sLoop2.Substring(0, nDataModelLength) & drRow("ColumnName") & "@> ")
                                oSWArray.Add("<@ENDLOOP_" & sLoop2 & "@> ")
                                oSWArray.Add("<@ENDLOOP_" & sLoop1 & "@> ")
                            ElseIf Not String.IsNullOrEmpty(sLoop1) AndAlso Not String.IsNullOrEmpty(sLoop2) AndAlso Not String.IsNullOrEmpty(sLoop3) Then
                                oSWArray.Add("<@LOOP_" & sLoop1 & "@> ")
                                oSWArray.Add("<@LOOP_" & sLoop2 & "@> ")
                                oSWArray.Add("<@LOOP_" & sLoop3 & "@> ")
                                oSWArray.Add("<@DB_" & sLoop2 & "_" & sLoop3.Substring(0, nDataModelLength) & drRow("ColumnName") & "@> ")
                                oSWArray.Add("<@ENDLOOP_" & sLoop3 & "@> ")
                                oSWArray.Add("<@ENDLOOP_" & sLoop2 & "@> ")
                                oSWArray.Add("<@ENDLOOP_" & sLoop1 & "@> ")
                            End If
                            Dim sGISDataModelField As String = ""
                            sGISDataModelField = sDataModel.ToUpper()
                            If sSubGrp <> "" Then
                                sGISDataModelField = sGISDataModelField & "_" & sSubGrp.ToUpper().Replace(sDataModel.ToUpper(), "")
                            End If
                            If sLoop1 <> "" Then
                                sGISDataModelField = sGISDataModelField & "_" & sLoop1.ToUpper().Replace(sDataModel.ToUpper(), "")
                            End If
                            If sLoop2 <> "" Then
                                sGISDataModelField = sGISDataModelField & "_" & sLoop2.ToUpper().Replace(sDataModel.ToUpper(), "")
                            End If
                            If sLoop3 <> "" Then
                                sGISDataModelField = sGISDataModelField & "_" & sLoop3.ToUpper().Replace(sDataModel.ToUpper(), "")
                            End If
                            If (sGISDataModelField = sDataModel.ToUpper()) Then
                                sGISDataModelField = sGISDataModelField & "_" & drRow("ColumnName").ToString().ToUpper()
                            End If
                            AddClauseToTableForCCM(sGISDataModelField)
                            GenerateStandardWordingDocuments(oSWArray)
                            oSWArray = Nothing
                        Next

                    End If

                End If
            Next

            'GenerateDocumentPure(oSWArray)
        End If

        'Fetch the data from DB for the Template Fields
        GetDataForCCMDocTemplateFields(dtCCMDocTemplateFields)

        'Generate the XML for CCM Request
        GetXMLForCCMDocument(m_dtDocument, sResolvedXML)

        'Call the CCM Method to generate the Document
        nResult = ComposeDocx(sCCMDocumentName, sResolvedXML, oDocTemplatePackList)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        m_sDocFileExtension = "xml"
        nResult = GetClientFolder()
        Dim sClientFile As String = "Doc " & m_lDocumentTemplateId & "." & m_sDocFileExtension
        If m_sClient.EndsWith("\") Then
            sClientFile = m_sClient & sClientFile
        Else
            sClientFile = m_sClient & "\" & sClientFile
        End If

        System.IO.File.WriteAllBytes(sClientFile, oDocTemplatePackList)

        m_sResolvedDocumentName = sClientFile

        'added if documentformat is pdf
        Dim sTempFileName As String
        If m_bSpoolAsPDF Then
            sTempFileName = sClientFile.Remove(sClientFile.Length - 3) & "pdf"
            'Ensure Document Consistancy by saving with aspose.
            ConvertDocumentUsingSiriusDocumentUtility(sClientFile, sTempFileName)

            m_sResolvedDocumentName = sTempFileName

        End If

        'If Not m_oFilePathArray Is Nothing Then
        '    For Each sFilePath As String In m_oFilePathArray
        '        If File.Exists(sFilePath) Then
        '            nResult = DOCGeneralFunc.DeleteFile(sFilePath) ''delete temp files created for ccm clauses
        '            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
        '                Return gPMConstants.PMEReturnCode.PMFalse
        '            End If
        '        End If
        '    Next
        '    If Directory.Exists(m_sDocTemplatePath & m_lInsuranceFileCnt) Then
        '        nResult = bPMDocFunctions.DelDirectory(m_sDocTemplatePath & m_lInsuranceFileCnt) ''delete temp folder created for ccm clauses
        '        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
        '            Return gPMConstants.PMEReturnCode.PMFalse
        '        End If
        '    End If
        'End If

        Return nResult
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oSWArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerateStandardWordingDocuments(ByVal oSWArray As ArrayList) As Integer
        Dim dtRisks As DataTable = Nothing
        Dim nReturn As Integer = PMEReturnCode.PMTrue

        nReturn = m_oBusiness.GetALLRisksForPolicy(nFileInsuranceCnt:=m_lInsuranceFileCnt, dtRisks:=dtRisks)
        If nReturn = PMEReturnCode.PMTrue Then
            For Each row As DataRow In dtRisks.Rows
                m_lRiskCnt = ToSafeInteger(row.Item(0))
                GenerateDocumentPure(oSWArray)
            Next
        End If
        Return nReturn
    End Function

    ''' <summary>
    ''' Method called to get fieldsets and fields
    ''' </summary>
    ''' <param name="sCCMDocumentName"></param>
    ''' <param name="r_dtCCMDocTemplateFields"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDataForCCMDocument(ByVal sCCMDocumentName As String, ByRef r_dtCCMDocTemplateFields As DataTable) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sDocumentFieldsetFieldList As String = String.Empty
        Dim oArray As String() = Nothing

        Dim oCCMDocumentProdBusiness As bCCMDocumentProduction.Business = New bCCMDocumentProduction.Business
        nResult = oCCMDocumentProdBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=(m_oDatabase))
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        ''stored proc call to get fieldsets from DB
        nResult = oCCMDocumentProdBusiness.GetDocumentFieldsetFieldList(sCCMDocumentName, sDocumentFieldsetFieldList)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        If String.IsNullOrEmpty(sDocumentFieldsetFieldList) Then
            nResult = oCCMDocumentProdBusiness.GetCCMDocumentTemplateFields(sCCMDocumentName, r_dtCCMDocTemplateFields)
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            For Each dtRow As DataRow In r_dtCCMDocTemplateFields.Rows
                sDocumentFieldsetFieldList = sDocumentFieldsetFieldList & dtRow(0) & "." & dtRow(1) & ","
            Next
            If sDocumentFieldsetFieldList <> "" Then
                sDocumentFieldsetFieldList = sDocumentFieldsetFieldList.Substring(0, sDocumentFieldsetFieldList.Length - 1)
            End If

            ''update fieldlist.field values to DB
            nResult = oCCMDocumentProdBusiness.UpdateDocumentFieldsetFieldList(sCCMDocumentName, sDocumentFieldsetFieldList)
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
        Else
            If sDocumentFieldsetFieldList <> "" Then
                oArray = sDocumentFieldsetFieldList.Split(",")
                For Each sFieldVal As String In oArray
                    Dim sFieldSetName As String = sFieldVal.Split(".")(0)
                    Dim sFieldName As String = sFieldVal.Split(".")(1)
                    If sFieldSetName <> "" Then
                        r_dtCCMDocTemplateFields.Rows.Add(sFieldSetName.Trim(), sFieldName.Trim())
                    End If
                    'If sFieldName = "FilePath" Then
                    '    r_dtCCMDocTemplateFields.Rows.Add(sFieldSetName.Trim(), "UID")
                    'End If
                Next
            End If
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Create CCM document from XML file
    ''' </summary>
    ''' <param name="sCCMDocumentName"></param>
    ''' <param name="sResolvedXML"></param>
    ''' <param name="oDocTemplatePackList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ComposeDocx(ByVal sCCMDocumentName As String, ByVal sResolvedXML As String,
                                 ByRef oDocTemplatePackList() As Byte) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Dim oCCMDocumentProdBusiness As bCCMDocumentProduction.Business = New bCCMDocumentProduction.Business
        nResult = oCCMDocumentProdBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=(m_oDatabase))
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        nResult = oCCMDocumentProdBusiness.ComposeDocx(sCCMDocumentName, sResolvedXML, oDocTemplatePackList)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get data from DB to create input XML for CCM
    ''' </summary>
    ''' <param name="dtCCMFields"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataForCCMDocTemplateFields(ByVal dtCCMFields As DataTable) As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim nRiskCnt As Integer
        Dim dtRisks As DataTable = Nothing
        Dim oRisksColl As New List(Of Integer)

        'Get All risks for Policy
        If ToSafeInteger(m_lInsuranceFileCnt) > 0 Then
            If m_lClaimCnt = 0 Then
                nReturn = m_oBusiness.GetALLRisksForPolicy(nFileInsuranceCnt:=m_lInsuranceFileCnt, dtRisks:=dtRisks)
                If nReturn = PMEReturnCode.PMTrue Then
                    For Each row As DataRow In dtRisks.Rows
                        oRisksColl.Add(ToSafeInteger(row.Item(0)))
                    Next
                End If
            ElseIf m_lClaimCnt <> 0 Then
                'if we are processing a claim document then make sure the risk_cnt is the one on this claim
                nReturn = m_oBusiness.GetRiskID(v_lClaimCnt:=m_lClaimCnt, r_lRiskCnt:=nRiskCnt)
                If nReturn = PMEReturnCode.PMTrue Then
                    oRisksColl.Add(nRiskCnt)
                End If
            End If
        Else   'm_lInsuranceFileCnt= 0 
            oRisksColl.Add(0)
        End If
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetRiskID Failed")
        End If

        Dim dtWPfieldsDetails As DataTable = New DataTable()
        Dim vInstanceArray() As Object = Nothing
        Dim dtCCMfieldsDetailsWithSpecialsType As DataTable = New DataTable()

        'Get the WP field details from DataBase.this will retrieve only the Unique SPs that need to be called.
        'this will make sure that one SP will be called only once.
        nReturn = m_oBusiness.GetWPFieldsDetails(dtFields:=dtCCMFields, r_dtResultSet:=dtWPfieldsDetails, InsurancefileCnt:=m_lInsuranceFileCnt)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetWPFieldsDetails Failed")
        End If
        nReturn = m_oBusiness.GetCCMFieldDetailsWithSpecialsType(dtFields:=dtCCMFields, r_dtResultSet:=dtCCMfieldsDetailsWithSpecialsType, InsuranceFileCnt:=m_lInsuranceFileCnt)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetCCMFieldDetailsWithSpecialsType failed.")
        End If

        'Non-Risk Type to be processed only once
        dtWPfieldsDetails.Columns.Add("Processed", Type.GetType("System.Int32"))

        dtCCMfieldsDetailsWithSpecialsType.Columns.Add("Processed", Type.GetType("System.Int32"))

        If dtWPfieldsDetails IsNot Nothing AndAlso dtWPfieldsDetails.Rows.Count > 0 Then
            Dim sStoredProcName As String = ""
            Dim sLoop1 As String = ""
            Dim sLoop2 As String = ""
            Dim sLoop3 As String = ""
            Dim sFieldName As String = String.Empty
            Dim sTableName As String
            Dim sMainGroup As String
            Dim sSubgroup As String
            Dim nParentKey As Integer = 0
            Dim nSpecialsType As Integer

            'Process Each risks
            For Each nRiskItem As Integer In oRisksColl

                nRiskCnt = nRiskItem
                vInstanceArray = Nothing

                'Process Each Row One by one
                For Each row As DataRow In dtWPfieldsDetails.Rows
                    'Richard Clarke - added this array reinitialisation here as it's not resetting correctly for different objects
                    vInstanceArray = Nothing
                    Dim sText As String = ""
                    Dim sDataModel As String = ToSafeString(row("data_model"))
                    sStoredProcName = ToSafeString(row("SQL"))
                    sLoop1 = ToSafeString(row("Loop1"))
                    sLoop2 = ToSafeString(row("Loop2"))
                    sLoop3 = ToSafeString(row("Loop3"))
                    sTableName = ToSafeString(row("table_name"))
                    sMainGroup = ToSafeString(row("main_group"))
                    sSubgroup = ToSafeString(row("sub_group"))
                    sDataModel = ToSafeString(row("Data_Model"))
                    nSpecialsType = ToSafeInteger(row("specials_type"))
                    'sMainGroup = ToSafeString(row("main_group"))
                    If ToSafeInteger(row("Processed")) = 1 Then
                        Continue For
                    End If

                    If sMainGroup <> "Risk" And sMainGroup <> "Reinsurance" Then
                        row("Processed") = 1
                    End If

                    If sLoop1 <> "" Then
                        sText = "Loop1"
                        If sLoop2 <> "" Then
                            sText += ";Loop2"
                        End If
                        If sLoop3 <> "" Then
                            sText += ";Loop3"
                        End If
                    End If
                    If sText <> "" Then
                        sText += ";DB"
                    Else
                        sText += "DB"
                    End If

                    'for the Loop tag , Filedname will be sLoop1
                    If sLoop1 <> "" Then
                        sFieldName = sLoop1
                    End If

                    If sLoop1 <> "" And sTableName.StartsWith("Core") Then
                        If m_oGetKeys.Count > 0 AndAlso (m_oGetKeys.ContainsKey(sFieldName.ToLower)) Then
                            m_oGetKeys.TryGetValue(sFieldName.ToLower, m_lReturn)
                        Else
                            m_lReturn = m_oBusiness.GetKeysExists(sFieldName)
                            m_oGetKeys.Add(sFieldName.ToLower, m_lReturn)
                        End If
                    Else
                        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                    End If

                    If (Not IsArray(vInstanceArray)) And sText <> "DB" Then
                        Dim nRiskId As Integer
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = m_oBusiness.GetActualRiskForLoopPolicy(m_lInsuranceFileCnt, sDataModel, nRiskId)
                            If nRiskId > 0 Then
                                nRiskCnt = nRiskId
                            End If

                            'Get the Parent Key
                            nReturn = m_oBusiness.GetParentKey(r_lParentKey:=nParentKey,
                                                                 v_sChildTable:=sFieldName,
                                                                 v_lPartyCnt:=m_lPartyCnt,
                                                                 v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                 v_lClaimCnt:=m_lClaimCnt,
                                                                 v_sDocumentRef:=m_sDocumentRef,
                                                                 v_vRiskId:=nRiskCnt)
                            If nReturn <> PMEReturnCode.PMTrue AndAlso nReturn <> PMEReturnCode.PMNotFound Then
                                Throw New ApplicationException("")
                            End If
                            If nReturn = PMEReturnCode.PMTrue AndAlso nParentKey > 0 Then
                                ReDim vInstanceArray(0)
                                vInstanceArray(0) = nParentKey
                            End If
                        End If
                    End If

                    GetData(nRiskCnt:=nRiskCnt, nClaimCnt:=m_lClaimCnt, sDocumentRef:=m_sDocumentRef,
                               sFieldName:=sFieldName, sMainGroup:=sMainGroup, sSubgroup:=sSubgroup, sDataModel:=sDataModel,
                               vInstanceArray:=vInstanceArray, oCCMFields:=dtCCMfieldsDetailsWithSpecialsType, sStoredProcName:=sStoredProcName,
                                sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3,
                                sTableName:=sTableName, sText:=sText, nSpecialsType:=nSpecialsType)


                Next
            Next
        End If

        Return nReturn

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nRiskCnt"></param>
    ''' <param name="nClaimCnt"></param>
    ''' <param name="sDocumentRef"></param>
    ''' <param name="sFieldName"></param>
    ''' <param name="sMainGroup"></param>
    ''' <param name="sSubgroup"></param>
    ''' <param name="sDataModel"></param>
    ''' <param name="vInstanceArray"></param>
    ''' <param name="oCCMFields"></param>
    ''' <param name="sStoredProcName"></param>
    ''' <param name="sLoop1"></param>
    ''' <param name="sLoop2"></param>
    ''' <param name="sLoop3"></param>
    ''' <param name="sTableName"></param>
    ''' <param name="sText"></param>
    ''' <remarks></remarks>
    Private Sub GetData(ByVal nRiskCnt As Integer, ByVal nClaimCnt As Integer, ByVal sDocumentRef As String,
                          ByVal sFieldName As String, ByVal sMainGroup As String, ByVal sSubgroup As String, ByVal sDataModel As String,
                          ByVal vInstanceArray() As Object, oCCMFields As DataTable, sStoredProcName As String,
                          sLoop1 As String, sLoop2 As String, sLoop3 As String,
                          sTableName As String, sText As String, nSpecialsType As Integer)

        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim sTag As String = ""
        Dim vKeyArray(,) As Object = Nothing

        If sText <> "" Then
            If sText.Contains(";") Then
                sTag = sText.Substring(0, sText.IndexOf(";"))
            Else
                sTag = sText
            End If
        End If

        If sTag = "Loop1" Then
            sFieldName = sLoop1
        ElseIf sTag = "Loop2" Then
            sFieldName = sLoop2
        Else
            sFieldName = sLoop3
        End If

        Select Case sTag
            Case "Loop1", "Loop2", "Loop3"
                If vInstanceArray IsNot Nothing AndAlso (ToSafeInteger(vInstanceArray(UBound(vInstanceArray))) <> 0) Then
                    nReturn = m_oBusiness.GetLoopKeys(r_vKeyArray:=vKeyArray,
                                                            v_sTableName:=sFieldName,
                                                            v_lPartyCnt:=m_lPartyCnt,
                                                            v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                            v_lClaimCnt:=nClaimCnt,
                                                            v_sDocumentRef:=sDocumentRef,
                                                            v_vInstanceArray:=vInstanceArray,
                                                            v_vRiskId:=nRiskCnt)

                    If (IsArray(vKeyArray)) Then
                        ReDim Preserve vInstanceArray(UBound(vInstanceArray) + 1)
                        For iLoop As Integer = 0 To UBound(vKeyArray, 2)
                            vInstanceArray(UBound(vInstanceArray)) = ToSafeInteger(vKeyArray(0, iLoop))
                            sText = sText.Replace(sTag, "")
                            If sText.StartsWith(";") Then
                                sText = sText.Substring(1)
                            End If

                            GetData(nRiskCnt:=nRiskCnt, nClaimCnt:=nClaimCnt, sDocumentRef:=sDocumentRef,
                              sFieldName:=sFieldName, sMainGroup:=sMainGroup, sSubgroup:=sSubgroup, sDataModel:=sDataModel,
                              vInstanceArray:=vInstanceArray, oCCMFields:=oCCMFields, sStoredProcName:=sStoredProcName,
                              sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3,
                              sTableName:=sTableName, sText:=sText, nSpecialsType:=nSpecialsType)

                        Next
                    End If
                Else
                    If m_oGetKeys.Count > 0 AndAlso (m_oGetKeys.ContainsKey(sFieldName.ToLower)) Then
                        m_oGetKeys.TryGetValue(sFieldName.ToLower, m_lReturn)
                    Else
                        m_lReturn = gPMConstants.PMEReturnCode.PMNotFound
                    End If
                    Dim lLoopCount As Integer
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And sTableName.StartsWith("Core") Then
                        'DJM 22/05/2002 - Get count of Loop Lines
                        m_lReturn = m_oBusiness.GetLoopCount(lLoopCount,
                                                    sFieldName,
                                                    m_lPartyCnt,
                                                    m_lInsuranceFileCnt,
                                                    m_lClaimCnt,
                                                    m_sDocumentRef,
                                                ToSafeLong(nRiskCnt))
                    End If
                    'DJM 02/04/2002 - Fill vKeyArray with incrementing numbers.
                    If lLoopCount <> 0 And m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        ReDim vKeyArray(0 To 0, 0 To lLoopCount - 1)

                        For lLoop As Integer = 0 To lLoopCount - 1
                            vKeyArray(0, lLoop) = lLoop + 1
                        Next
                    End If

                    If (IsArray(vKeyArray)) Then
                        If Not Information.IsNothing(vInstanceArray) Then
                            ReDim Preserve vInstanceArray(UBound(vInstanceArray) + 1)
                        Else
                            ReDim vInstanceArray(0)
                        End If
                        For iLoop As Integer = 0 To UBound(vKeyArray, 2)
                            vInstanceArray(UBound(vInstanceArray)) = ToSafeInteger(vKeyArray(0, iLoop))
                            sText = sText.Replace(sTag, "")
                            If sText.StartsWith(";") Then
                                sText = sText.Substring(1)
                            End If

                            GetData(nRiskCnt:=nRiskCnt, nClaimCnt:=nClaimCnt, sDocumentRef:=sDocumentRef,
                              sFieldName:=sFieldName, sMainGroup:=sMainGroup, sSubgroup:=sSubgroup, sDataModel:=sDataModel,
                              vInstanceArray:=vInstanceArray, oCCMFields:=oCCMFields, sStoredProcName:=sStoredProcName,
                              sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3,
                              sTableName:=sTableName, sText:=sText, nSpecialsType:=nSpecialsType)

                        Next
                    End If


                End If


            Case "DB"
                GetFieldValues(oCCMFields:=oCCMFields, sStoredProcName:=sStoredProcName,
                           nPartyCnt:=m_lPartyCnt, nInsuranceFileCnt:=m_lInsuranceFileCnt, nRiskCnt:=nRiskCnt,
                           sMainGroup:=sMainGroup, sSubGroup:=sSubgroup, sDataModel:=sDataModel, sTableName:=sTableName,
                           nClaimCnt:=nClaimCnt, sDocumentRef:=sDocumentRef,
                           sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3,
                           vInstanceArray:=vInstanceArray, nSpecialsType:=nSpecialsType)

        End Select

    End Sub

    ''' <summary>
    ''' Get field Values from DB
    ''' </summary>
    ''' <param name="oCCMFields"></param>
    ''' <param name="sStoredProcName"></param>
    ''' <param name="nPartyCnt"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nRiskCnt"></param>
    ''' <param name="nClaimCnt"></param>
    ''' <param name="sMainGroup"></param>
    ''' <param name="sSubGroup"></param>
    ''' <param name="sDataModel"></param>
    ''' <param name="sTableName"></param>
    ''' <param name="sDocumentRef"></param>
    ''' <param name="sLoop1"></param>
    ''' <param name="sLoop2"></param>
    ''' <param name="sLoop3"></param>
    ''' <param name="vInstanceArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFieldValues(oCCMFields As DataTable, ByVal sStoredProcName As String,
                               ByVal nPartyCnt As Integer, ByVal nInsuranceFileCnt As Integer, nRiskCnt As Integer, nClaimCnt As Integer,
                               sMainGroup As String, sSubGroup As String, sDataModel As String, sTableName As String,
                               sDocumentRef As String,
                               sLoop1 As String, sLoop2 As String, sLoop3 As String,
                               vInstanceArray() As Object, nSpecialsType As Integer) As Integer

        Dim nReturn As Integer
        Dim oResultSet As DataTable
        Dim bSWInvoked As Boolean = False
        Dim nInstance1 As Integer, nInstance2 As Integer, nInstance3 As Integer, nInstance4 As Integer = 0
        Dim bHasSpecialTypes As Boolean = False
        If IsArray(vInstanceArray) Then
            nInstance1 = ToSafeInteger(vInstanceArray(0))

            If UBound(vInstanceArray) >= 1 Then
                nInstance2 = ToSafeInteger(vInstanceArray(1))
            End If
            If UBound(vInstanceArray) >= 2 Then
                nInstance3 = ToSafeInteger(vInstanceArray(2))
            End If
            If UBound(vInstanceArray) >= 3 Then
                nInstance4 = ToSafeInteger(vInstanceArray(3))
            End If
        End If
        Dim SpecialTypesRows() As DataRow = oCCMFields.Select("(tablename = '" & sTableName & "' AND specials_type = 5 ) OR ( tablename ='PolicyStandardWordings' and Processed IS NULL)")
        'Dim SpecialTypesRows() As DataRow = oCCMFields.Select("(specials_type = 5 ) OR ( tablename ='PolicyStandardWordings' and Processed IS NULL) ")
        If (SpecialTypesRows IsNot Nothing AndAlso SpecialTypesRows.Length > 0 AndAlso nSpecialsType = 5) Then
            bHasSpecialTypes = True
        End If
        If sTableName = "CoreSystemLogic" Then
            nInstance1 = m_iUserID
        End If
        If nSpecialsType <> 5 Then ''Standard Wording row added in previos SP for processing GetData
            Dim nResult As Integer = m_oBusiness.GetFieldValuesFromDB(vInstanceArray, nPartyCnt, nInsuranceFileCnt, nRiskCnt, nClaimCnt,
                                                           sDocumentRef, nInstance1, nInstance2, nInstance3, nInstance4, sStoredProcName, oResultSet)
        End If

        If oResultSet IsNot Nothing AndAlso oResultSet.Rows.Count > 0 Then
            Dim ccmRows() As DataRow = oCCMFields.Select("tablename = '" & sTableName & "' AND specials_type <> 5 ")
            For Each row1 As DataRow In ccmRows
                Dim sColumnName As String = ToSafeString(row1("ColumnName"))
                Dim sDataStructureName As String = ToSafeString(row1("DataStructureName"))
                Dim sValue As String = ""
                If UCase$((Right(sColumnName, Len("USRSignatureFile")))) = "USRSIGNATUREFILE" Then
                    sValue = CopySignatureToCcmFolder()
                Else
                    If oResultSet.Columns.Contains(sColumnName) Then
                        sValue = ToSafeString(oResultSet.Rows(0).Item(sColumnName))
                    End If
                End If
                If sValue <> "" Then
                    AddToTableForCCM(sMainGroup:=sMainGroup,
                               sSubGroup:=sSubGroup, sFieldName:=sColumnName, sTableName:=sTableName, sDataStructureName:=sDataStructureName, sDataModel:=sDataModel,
                               sValue:=sValue,
                               iRiskCnt:=nRiskCnt, sRiskDescription:="", sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3,
                               nInstance1:=nInstance1, nInstance2:=nInstance2, nInstance3:=nInstance3, nInstance4:=nInstance4, sDataStructureNameUpdated:=sDataStructureName)
                End If
            Next
        End If

        If bHasSpecialTypes Then
            'Handle Standard Wordings
            Dim CCMRows() As DataRow = SpecialTypesRows
            Dim nColumnType As Integer = 0

            For Each row As DataRow In CCMRows
                Dim aoStandardWordingsArray(,) As Object = Nothing
                Dim nColumnSpecialsType As Integer = ToSafeInteger(row("specials_type"))
                Dim sStandardWordingProperty As String = ToSafeString(row("ColumnName"))
                Dim sDataStructureName As String = ToSafeString(row("DataStructureName"))
                Dim nIncluded As Boolean = IIf(ToSafeInteger(row("Included")) = 1, True, False)
                Dim bRiskLevelStandardWordings As Boolean = False

                ''Policy Level SW
                If sStandardWordingProperty = "PolicyStandardWordings" Then
                    nReturn = m_oBusiness.GetStandardWordings(
                                        r_vStandardWordingsArray:=aoStandardWordingsArray,
                                        vInsFileCnt:=m_lInsuranceFileCnt)
                    row("Processed") = 1
                    sTableName = sStandardWordingProperty
                    sDataModel = ""
                    sLoop1 = ""
                    sLoop2 = ""
                    sLoop3 = ""
                    sMainGroup = "Policy"
                    sSubGroup = "SW"
                Else
                    ''Risk Level SW
                    If Not Information.IsNothing(vInstanceArray) Then
                        nReturn = m_oBusiness.GetStandardWordings(
                                           r_vStandardWordingsArray:=aoStandardWordingsArray,
                                           vInsFileCnt:=m_lInsuranceFileCnt,
                                           vRiskCnt:=nRiskCnt,
                                           vStandardWordingProperty:=sStandardWordingProperty,
                                           vChildID:=vInstanceArray(UBound(vInstanceArray)),
                                           sTableName:=sTableName)
                    Else
                        nReturn = m_oBusiness.GetStandardWordings(
                            r_vStandardWordingsArray:=aoStandardWordingsArray,
                            vInsFileCnt:=m_lInsuranceFileCnt,
                            vRiskCnt:=nRiskCnt,
                            vStandardWordingProperty:=sStandardWordingProperty,
                            sTableName:=sTableName)
                    End If
                    nReturn = m_oBusiness.GetDataModel(lRiskCnt:=nRiskCnt, r_sDataModelCode:=sDataModel)

                    bRiskLevelStandardWordings = True
                End If

                If nReturn = PMEReturnCode.PMTrue Then
                    'place a check to ensure that clauses are not being pulled from other risks
                    If IsArray(aoStandardWordingsArray) And sTableName.StartsWith(sDataModel) Then
                        For iLoop As Integer = 0 To UBound(aoStandardWordingsArray, 2)
                            Dim sSWValue As String = ""
                            Dim sColumnName As String = sStandardWordingProperty
                            sSWValue = ToSafeString(aoStandardWordingsArray(3, iLoop))
                            sSWValue &= "~" & ToSafeString(aoStandardWordingsArray(4, iLoop))
                            If nIncluded Then
                                sSWValue &= "~" & m_sDocTemplatePath & m_lInsuranceFileCnt & "\Doc " & ToSafeString(aoStandardWordingsArray(1, iLoop)) & ".docx"
                            Else
                                sSWValue &= "~"
                            End If
                            Dim sDataStructureUpdated As String = sDataStructureName.Replace("_" & sColumnName, "")
                            AddToTableForCCM(sMainGroup:=sMainGroup,
                                      sSubGroup:=sSubGroup, sFieldName:=sColumnName, sTableName:=sTableName, sDataStructureName:=sDataStructureName, sDataModel:=sDataModel,
                                      sValue:=sSWValue, iRiskCnt:=nRiskCnt, sRiskDescription:="", sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3,
                                      nInstance1:=nInstance1, nInstance2:=nInstance2, nInstance3:=nInstance3, nInstance4:=nInstance4, nSpecialsType:=5, bRiskLevelStandardWordings:=bRiskLevelStandardWordings, sDataStructureNameUpdated:=sDataStructureUpdated)

                        Next
                    End If
                End If
            Next
        End If
        Return nReturn
    End Function

    ''' <summary>
    ''' Add data to datatable for CCM
    ''' </summary>
    ''' <param name="sMainGroup"></param>
    ''' <param name="sSubGroup"></param>
    ''' <param name="sFieldName"></param>
    ''' <param name="sTableName"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sDataModel"></param>
    ''' <param name="iRiskCnt"></param>
    ''' <param name="sRiskDescription"></param>
    ''' <param name="sLoop1"></param>
    ''' <param name="sLoop2"></param>
    ''' <param name="sLoop3"></param>
    ''' <param name="nInstance1"></param>
    ''' <param name="nInstance2"></param>
    ''' <param name="nInstance3"></param>
    ''' <remarks></remarks>
    Private Sub AddToTableForCCM(ByVal sMainGroup As String,
                                ByVal sSubGroup As String,
                                ByVal sFieldName As String,
                                ByVal sTableName As String,
                                ByVal sDataStructureName As String,
                                ByVal sValue As String,
                                ByVal sDataModel As String,
                                Optional ByVal iRiskCnt As Integer = 0,
                                Optional ByVal sRiskDescription As String = "",
                                Optional ByVal sLoop1 As String = "",
                                Optional ByVal sLoop2 As String = "",
                                Optional ByVal sLoop3 As String = "",
                                Optional ByVal nInstance1 As Integer = 0,
                                Optional ByVal nInstance2 As Integer = 0,
                                Optional ByVal nInstance3 As Integer = 0,
                                Optional ByVal nInstance4 As Integer = 0,
                                Optional ByVal nSpecialsType As Integer = 0,
                                 Optional bRiskLevelStandardWordings As Boolean = False,
                                 Optional sDataStructureNameUpdated As String = "")

        Static iID As Integer
        Dim dr As DataRow = m_dtDocument.NewRow

        dr("MAINGROUP") = sMainGroup
        dr("SUBGROUP") = sSubGroup
        dr("FIELDNAME") = sFieldName
        dr("tableName") = sTableName
        dr("DataStructureName") = sDataStructureName
        dr("datamodel") = sTableName
        dr("VALUE") = sValue
        dr("LOOP1") = sLoop1
        dr("LOOP2") = sLoop2
        dr("LOOP3") = sLoop3
        dr("DataModel") = sDataModel

        If sMainGroup = "Risk" Or sMainGroup = "Reinsurance" Then
            iID = iID + 1
            dr("RISKCNT") = iRiskCnt
            dr("RISKDescription") = sRiskDescription
            dr("ID") = iID

            dr("Instance1") = nInstance1
            dr("Instance2") = nInstance2
            dr("Instance3") = nInstance3
            dr("Instance4") = nInstance4
        ElseIf bRiskLevelStandardWordings Then
            dr("RISKCNT") = iRiskCnt
        End If
        dr("SpecialsType") = nSpecialsType
        dr("DataStructureNameUpdated") = sDataStructureNameUpdated
        m_dtDocument.Rows.Add(dr)
    End Sub

    Private Sub AddClauseToTableForCCM(ByVal sFieldCode As String)

        If m_dtStdWording IsNot Nothing Then
            Dim dr As DataRow = m_dtStdWording.NewRow
            dr("FieldCode") = sFieldCode
            'dr("FileName") = sClauseName
            m_dtStdWording.Rows.Add(dr)
        End If
    End Sub
    ''' <summary>
    ''' Create datatable format
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateTableForCCM()
        m_dtDocument = New DataTable("Document")
        m_dtDocument.Columns.Add("MAINGROUP", Type.GetType("System.String"))
        m_dtDocument.Columns.Add("SUBGROUP", Type.GetType("System.String"))
        m_dtDocument.Columns.Add("DATAMODEL", Type.GetType("System.String"))

        m_dtDocument.Columns.Add("LOOP1", Type.GetType("System.String"))
        m_dtDocument.Columns.Add("LOOP2", Type.GetType("System.String"))
        m_dtDocument.Columns.Add("LOOP3", Type.GetType("System.String"))

        m_dtDocument.Columns.Add("TableName", Type.GetType("System.String"))
        m_dtDocument.Columns.Add("DataStructureName", Type.GetType("System.String"))
        m_dtDocument.Columns.Add("FIELDNAME", Type.GetType("System.String"))
        m_dtDocument.Columns.Add("VALUE", Type.GetType("System.String"))
        m_dtDocument.Columns.Add("RISKCNT", Type.GetType("System.Int32"))
        m_dtDocument.Columns.Add("RISKDescription", Type.GetType("System.String"))
        m_dtDocument.Columns.Add("ID", Type.GetType("System.Int32")) 'For Sorting the DataTable

        m_dtDocument.Columns.Add("Instance1", Type.GetType("System.Int32"))
        m_dtDocument.Columns.Add("Instance2", Type.GetType("System.Int32"))
        m_dtDocument.Columns.Add("Instance3", Type.GetType("System.Int32"))
        m_dtDocument.Columns.Add("Instance4", Type.GetType("System.Int32"))
        m_dtDocument.Columns.Add("SpecialsType", Type.GetType("System.Int32"))
        m_dtDocument.Columns.Add("DataStructureNameUpdated", Type.GetType("System.String"))
        m_dtStdWording = New DataTable("SWDocument")
        m_dtStdWording.Columns.Add("FieldCode", Type.GetType("System.String"))
        m_dtStdWording.Columns.Add("FileName", Type.GetType("System.String"))
        m_dtStdWording.Columns.Add("File", Type.GetType("System.String"))
    End Sub

    ''' <summary>
    ''' Create xml string to generate document
    ''' </summary>
    ''' <param name="dtDocumentDetails"></param>
    ''' <param name="r_sInputXMLString"></param>
    ''' <remarks></remarks>
    Private Sub GetXMLForCCMDocument(ByVal dtDocumentDetails As DataTable, ByRef r_sInputXMLString As String)
        CreateInputXMLForCCM(r_sInputXMLString)
    End Sub

    ''' <summary>
    ''' Create xml string to generate document
    ''' </summary>
    ''' <param name="sOutPut"></param>
    ''' <remarks></remarks>
    Private Sub CreateInputXMLForCCM(ByRef sOutPut As String)
        Dim sMainGroup As String
        Dim sSubGroup As String
        Dim sOldSubGroup As String = ""
        Dim sRiskDesc As String
        Dim sTableName As String
        Dim sDataStructureName As String
        Dim sOldTableName As String = ""
        Dim sFieldName As String, sValue As String, sLoop1 As String
        Dim sLoop2 As String = ""
        Dim sLoop3 As String = ""
        Dim iRiskCnt As Integer
        Dim nOldRiskCnt As Integer
        Dim sOldLoop1 As String = ""
        Dim sOldLoop2 As String = ""
        Dim sDataModel As String
        Dim sOldDataModel As String = ""
        Dim i As Integer
        Dim sInstance1 As String
        Dim sInstance2 As String
        Dim sInstance3 As String
        Dim sOldInstance1 As String = ""
        Dim sOldInstance2 As String = ""
        Dim sOldInstance3 As String = ""
        Dim nLevel As Integer = 0
        Dim nSpecialsType As Integer = 0

        Dim xmlDoc As New XmlDocument
        Dim oXMLElement As XmlElement = Nothing
        Dim oXMLRootElementDS_DataModels As XmlElement = Nothing

        xmlDoc.Load(New StringReader(kXMLHeader))

        m_dtDocument.DefaultView.Sort = "RISKCNT ASC, DataModel ASC, SubGroup ASC, Loop1 ASC, Instance1 ASC, Instance2 ASC , Instance3 ASC, DataStructureName ASC"

        m_dtDocument = m_dtDocument.DefaultView.ToTable
        Dim bRiskHeaderAdded As Boolean = False

        For Each row As DataRow In m_dtDocument.Select("", "RISKCNT ASC, DataModel ASC, SubGroup ASC, Loop1 ASC, Instance1 ASC, Instance2 ASC , Instance3 ASC, DataStructureName ASC")
            i = i + 1

            sMainGroup = ReplaceSpecialCharacter(row("MAINGROUP"))
            sSubGroup = ReplaceSpecialCharacter(row("SubGroup"))
            sFieldName = ReplaceSpecialCharacter(row("FIELDNAME"))
            sValue = row("VALUE")
            sTableName = ToSafeString(row("TableName"))
            sDataModel = ToSafeString(row("DataModel"))
            nSpecialsType = ToSafeInteger(row("SpecialsType"))
            sDataStructureName = ToSafeString(row("DataStructureName"))
            Dim nonLoopingTable As Boolean = False
            If sSubGroup <> "" AndAlso sDataModel <> "" Then
                sSubGroup = sSubGroup.Replace(sDataModel, "").ToUpper()
            End If

            sLoop1 = ToSafeString(row("LOOP1"))
            If sLoop1 <> "" AndAlso sDataModel <> "" Then
                sLoop1 = sLoop1.Replace(sDataModel, "").ToUpper()
            End If

            sLoop2 = ToSafeString(row("LOOP2"))
            If sLoop2 <> "" AndAlso sDataModel <> "" Then
                sLoop2 = sLoop2.Replace(sDataModel, "").ToUpper()
            End If

            sLoop3 = ReplaceSpecialCharacter(ToSafeString(row("LOOP3")))
            If sLoop3 <> "" AndAlso sDataModel <> "" Then
                sLoop3 = sLoop3.Replace(sDataModel, "").ToUpper()
            End If
            sInstance1 = ToSafeString(row("instance1"))
            sInstance2 = ToSafeString(row("instance2"))
            sInstance3 = ToSafeString(row("instance3"))

            sTableName = sTableName.Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "")
            sSubGroup = sSubGroup.Replace("-", "").Replace(",", "").Replace("/", "").Replace(" ", "")

            sRiskDesc = ToSafeString(row("RISKDescription"))
            iRiskCnt = ToSafeInteger((row("RISKCNT")))

            If iRiskCnt = 0 Then iRiskCnt = -1

            Dim bIsParentReset As Boolean = False

            If sDataModel = "" Then 'Handle Core Fields
                Dim sPrefix As String = sTableName
                If nSpecialsType <> 5 Then
                    'most common scenario is same table and non-looping for core fields so prioritise that
                    If sTableName = sOldTableName And sLoop1 = "" Then
                        nonLoopingTable = True 'I don't think we need to do anything else here as the next if block handles adding properties to existing non-looping objects
                    ElseIf sTableName <> sOldTableName And sLoop1 = "" Then
                        nonLoopingTable = True
                        'Richard Clarke
                        'sometimes this doesn't work as you can get a repeated table such as CorePolicyGeneral after another set of tables, despite having added it earlier
                        'we either fix the order they come into here so they're always sequential and not repeated after other tables
                        'or just check the xml here, see if it's already been added and append the values?                            
                        Dim nodeList As XmlNodeList = xmlDoc.GetElementsByTagName(sTableName)

                        If Not IsNothing(nodeList) AndAlso nodeList.Count = 0 Then
                            oXMLElement = xmlDoc.CreateElement("", sPrefix, kxmlns)
                            xmlDoc.DocumentElement.AppendChild(oXMLElement)
                            sOldTableName = sTableName
                        End If
                    ElseIf sTableName <> sOldTableName AndAlso sLoop1 <> "" And sLoop2 = "" Then   'Handle Loop1
                        sPrefix = "DS_"

                        oXMLElement = xmlDoc.CreateElement("", sPrefix & "Core" & sLoop1, kxmlns)
                        xmlDoc.DocumentElement.AppendChild(oXMLElement)
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sTableName, "")
                        sOldTableName = sTableName

                    ElseIf sTableName <> sOldTableName AndAlso sLoop2 <> "" Then      'Handle Loop2
                        sPrefix = "DS_"
                        oXMLElement = xmlDoc.CreateElement("", sPrefix & "Core" & sLoop1, kxmlns)
                        xmlDoc.DocumentElement.AppendChild(oXMLElement)
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix & sTableName, "")
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sTableName, "")
                        sOldTableName = sTableName
                    End If
                    'Richard Clarke - 2704/2018 - run the new code if this is a core NON looping table
                    If nonLoopingTable Then
                        'does this core table AND attribute already exist
                        Dim oExistingNodes As XmlNodeList = xmlDoc.GetElementsByTagName(sTableName)
                        Dim bTableAndAttributeAlreadyExist As Boolean = False
                        Dim bTableAlreadyExists As Boolean = False
                        If oExistingNodes.Count > 0 Then
                            bTableAlreadyExists = True
                            'this checks whether the sTableName and sFieldName already exist 
                            If Not IsNothing(oExistingNodes.Item(0).Item(sFieldName)) Then
                                bTableAndAttributeAlreadyExist = True
                            End If

                            If bTableAlreadyExists Then
                                'add this field to the exiting sTableName node
                                oXMLElement = oExistingNodes.Item(0)
                            End If
                        End If

                        If Not bTableAndAttributeAlreadyExist Then
                            addChildElement(xmlDoc, oXMLElement, sFieldName, sValue)
                        End If
                    Else
                        'this is the original code that was run and will still run for looping core tables hopefully.
                        Dim oExistingNodes As XmlNodeList = oXMLElement.GetElementsByTagName(sFieldName)
                        If (oExistingNodes.Count >= 1) Then
                            If sLoop1 <> "" And sLoop2 = "" Then
                                sPrefix = "DS_Core" & sLoop1
                            Else
                                sPrefix = "DS_" & sTableName
                            End If
                            oXMLElement = oExistingNodes(0).ParentNode.ParentNode.ParentNode
                            oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                            oXMLElement = addChildElement(xmlDoc, oXMLElement, sTableName, "")
                        End If
                        addChildElement(xmlDoc, oXMLElement, sFieldName, sValue)
                    End If

                Else 'specialstype=5
                    Dim aValues() As String = sValue.Split("~")    ' 0 : Code , 1 : Description , 2 : filePath and Name
                    Dim sFilePath As String = String.Empty
                    Dim sFileName As String = String.Empty
                    Dim sFileBinary As String = String.Empty
                    If aValues(2) <> "" Then
                        sFilePath = ToSafeString(aValues(2))
                        sFileName = Path.GetFileName(sFilePath)
                        sFilePath = Path.GetDirectoryName(sFilePath)
                    End If

                    'For Policy Standard Wordings
                    Dim oXMLElementPrevious As XmlElement
                    sPrefix = "PolicyStandardWordings"
                    If oXMLElement Is Nothing Then
                        oXMLElement = xmlDoc.CreateElement("", sPrefix, kxmlns)
                        xmlDoc.DocumentElement.AppendChild(oXMLElement)
                        oXMLElementPrevious = oXMLElement
                    Else
                        oXMLElementPrevious = oXMLElement
                        oXMLElement = oXMLElement.ParentNode
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sFieldName, "")
                    End If
                    For Each drRows As DataRow In m_dtStdWording.Rows
                        If drRows(0) = PolicyStandardWordingTag AndAlso drRows(1) = aValues(0) Then
                            sFileBinary = drRows(2)
                            Exit For
                        End If
                    Next
                    addChildElement(xmlDoc, oXMLElement, "SWCODE", aValues(0))
                    addChildElement(xmlDoc, oXMLElement, "SWDESC", aValues(1))
                    addChildElement(xmlDoc, oXMLElement, "FileName", sFileName)
                    addChildElement(xmlDoc, oXMLElement, "FileBinary", sFileBinary)
                    addChildElement(xmlDoc, oXMLElement, "FilePath", sFilePath)

                    oXMLElement = oXMLElementPrevious
                End If

            Else
                'Handle Risk Fields
                Dim sPrefix As String = ""
                If bRiskHeaderAdded = False Then
                    sPrefix = "DS_" & "DataModels"
                    oXMLElement = xmlDoc.CreateElement("", sPrefix, kxmlns)
                    xmlDoc.DocumentElement.AppendChild(oXMLElement)
                    bRiskHeaderAdded = True
                    oXMLRootElementDS_DataModels = oXMLElement
                End If

                If nSpecialsType <> 5 Then
                    'Add a new Node for DataModel. If the next element relates to same datamodel then skip.

                    If nOldRiskCnt <> iRiskCnt Then
                        sPrefix = "DS_" & sDataModel

                        oXMLElement = oXMLRootElementDS_DataModels
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                        nOldRiskCnt = iRiskCnt

                        'Reset the variables for new risk
                        sOldSubGroup = ""
                        sOldLoop1 = ""
                        sOldTableName = ""
                        nLevel = 0
                    End If

                    'Add a new Node for Sub-Group.
                    If sSubGroup <> sOldSubGroup And sLoop1 = "" Then
                        sPrefix = "DS_" & sDataStructureName
                        oXMLElement = oXMLRootElementDS_DataModels.LastChild
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        sPrefix = sDataStructureName
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        sOldSubGroup = sSubGroup
                        sOldTableName = sTableName
                    ElseIf sSubGroup <> sOldSubGroup AndAlso sLoop1 <> "" AndAlso sLoop2 <> "" Then
                        sPrefix = "DS_" & sDataStructureName

                        oXMLElement = oXMLRootElementDS_DataModels.LastChild
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        sPrefix = "DS_" & sDataStructureName.Replace(sLoop1, "")
                        If sPrefix.EndsWith("_") Then
                            sPrefix = sPrefix.Substring(0, sPrefix.Length - 1)
                        End If

                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        sPrefix = sDataStructureName.Replace(sLoop1, "")
                        If sPrefix.EndsWith("_") Then
                            sPrefix = sPrefix.Substring(0, sPrefix.Length - 1)
                        End If

                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        oXMLElement = oXMLElement.ParentNode
                        sPrefix = "DS_" & sDataStructureName

                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                        sPrefix = sDataStructureName
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        If sLoop3 <> "" Then
                            oXMLElement = oXMLElement.ParentNode
                            sPrefix = "DS_" & sDataModel & "_" & sSubGroup & "_" & sLoop1 & "_" & sLoop2 & "_" & sLoop3
                            oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                            sPrefix = sDataModel & "_" & sSubGroup & "_" & sLoop1 & "_" & sLoop2 & "_" & sLoop3
                            oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                        End If

                        addChildElement(xmlDoc, oXMLElement, sFieldName, sValue)

                        sOldLoop1 = sLoop1
                        sOldTableName = sTableName
                        sOldSubGroup = sSubGroup
                        sOldInstance1 = sInstance1
                        sOldInstance2 = sInstance2
                        sOldInstance3 = sInstance3
                        Continue For
                    End If

                    If sSubGroup = sOldSubGroup And sLoop1 = "" AndAlso sLoop2 = "" Then
                        addChildElement(xmlDoc, oXMLElement, sFieldName, sValue)

                    ElseIf sTableName <> sOldTableName AndAlso sLoop1 <> "" AndAlso sLoop2 = "" AndAlso sLoop3 = "" Then
                        If sSubGroup <> sOldSubGroup Then
                            If iRiskCnt = -1 Then           'for Claims
                                sPrefix = "DS_" & sDataModel & "_" & sSubGroup
                            Else
                                sPrefix = "DS_" & sDataStructureName.Replace(sLoop1, "")

                                If sPrefix = "DS_" & sDataStructureName Then
                                    Dim sLoop1Copy As String = ""

                                    Dim sLoop1Temp As String = ToSafeString(row("LOOP1"))
                                    If sLoop1Temp <> "" AndAlso sDataModel <> "" Then
                                        sLoop1Temp = sLoop1Temp.Replace(sDataModel, "")
                                    End If
                                    For icount As Integer = 0 To sLoop1Temp.Length - 1
                                        If Char.IsUpper(sLoop1Temp(icount)) And icount <> 0 Then
                                            sLoop1Copy = sLoop1Copy + "_"
                                        End If
                                        sLoop1Copy = sLoop1Copy + sLoop1Temp(icount)
                                    Next
                                    sPrefix = "DS_" & sDataStructureName.Replace(sLoop1Copy.ToUpper, "")
                                End If

                                If sPrefix.EndsWith("_") Then
                                    sPrefix = sPrefix.Substring(0, sPrefix.Length - 1)
                                End If
                            End If
                            oXMLElement = oXMLRootElementDS_DataModels.LastChild
                            oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                            sOldSubGroup = sSubGroup
                            sOldTableName = sTableName
                        Else
                            oXMLElement = oXMLRootElementDS_DataModels.LastChild.LastChild
                        End If

                        If sPrefix <> "DS_" & sDataStructureName Then
                            'Add new DS Node for Loop1.
                            sPrefix = "DS_" & sDataStructureName

                            nLevel = 1 'We are at Loop1
                            oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                        End If
                        'Add new Child Node for Loop1 - under DS node.
                        sPrefix = sDataStructureName

                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        'Add new data Node for Loop1.
                        addChildElement(xmlDoc, oXMLElement, sFieldName, sValue)

                        sOldLoop1 = sLoop1
                        sOldTableName = sTableName
                        sOldInstance1 = sInstance1
                        sOldInstance2 = sInstance2
                        sOldInstance3 = sInstance3

                    ElseIf sTableName = sOldTableName AndAlso sLoop1 <> "" Then

                        If sOldInstance1 <> sInstance1 OrElse sOldInstance2 <> sInstance2 Then
                            oXMLElement = oXMLElement.ParentNode.ParentNode

                            sPrefix = "DS_" & sDataStructureName

                            'Add new DS Node for Loop.
                            oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                            'Add new Child Node for Loop1 - under DS node.
                            sPrefix = sDataStructureName
                            oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        Else
                            'Reset the Parent if the attribute already exists
                            Dim oExistingNodes As XmlNodeList = oXMLElement.GetElementsByTagName(sFieldName)

                            If (oExistingNodes.Count >= 1) Then
                                oXMLElement = oExistingNodes(0).ParentNode.ParentNode.ParentNode

                                sPrefix = "DS_" & sDataStructureName

                                'Add new DS Node for Loop.
                                oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                                'Add new Child Node for Loop1 - under DS node.
                                sPrefix = sDataStructureName
                                oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                            End If
                        End If
                        addChildElement(xmlDoc, oXMLElement, sFieldName, sValue)
                        sOldInstance1 = sInstance1
                        sOldInstance2 = sInstance2
                        sOldInstance3 = sInstance3
                        'For Same loop1 and that may have child.   i.e. Loop2 <> ""               
                    ElseIf sLoop1 = sOldLoop1 AndAlso sTableName <> sOldTableName AndAlso sLoop2 <> "" AndAlso sLoop3 = "" Then

                        'If we are coming from Loop3 to Loop2 then XMLElement needs to be pointed correctly
                        If nLevel = 3 Then
                            oXMLElement = oXMLElement.ParentNode.ParentNode.ParentNode
                        ElseIf nLevel = 2 Then
                            oXMLElement = oXMLElement.ParentNode.ParentNode
                        Else
                            oXMLElement = oXMLElement.ParentNode
                        End If
                        nLevel = 2

                        'Add new DS Node for Loop2.
                        sPrefix = "DS_" & sDataStructureName
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        'Add new Child Node for Loop1 - under DS node.
                        sPrefix = sDataStructureName
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        'Add new data Node for Loop1.
                        addChildElement(xmlDoc, oXMLElement, sFieldName, sValue)


                        sOldTableName = sTableName

                    ElseIf sLoop1 = sOldLoop1 AndAlso sTableName <> sOldTableName AndAlso sLoop3 <> "" Then

                        If nLevel = 3 Then
                            oXMLElement = oXMLElement.ParentNode.ParentNode
                        Else
                            oXMLElement = oXMLElement.ParentNode
                        End If
                        nLevel = 3

                        'Add new DS Node for Loop1.
                        sPrefix = "DS_" & sDataStructureName
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        'Add new Child Node for Loop1 - under DS node.
                        sPrefix = sDataStructureName
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")

                        'Add new data Node for Loop1.
                        sPrefix = sDataStructureName
                        addChildElement(xmlDoc, oXMLElement, sFieldName, sValue)
                        sOldTableName = sTableName
                    End If
                Else

                    'For Risk Level Standard Wordings
                    Dim oXMLElementPrevious As XmlElement = oXMLElement
                    sPrefix = "DS_" & sTableName
                    If oXMLElement.ParentNode.Name <> sPrefix Then
                        oXMLElement = oXMLRootElementDS_DataModels.LastChild
                        If oXMLElement Is Nothing Then
                            oXMLElement = oXMLElementPrevious
                        End If
                        oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                    Else
                        oXMLElement = oXMLElement.ParentNode
                    End If

                    sPrefix = sDataStructureName

                    Dim aValues() As String = sValue.Split("~")    ' 0 : Code , 1 : Description , 2 : filePath and Name
                    Dim sFilePath As String = String.Empty
                    Dim sFileName As String = String.Empty
                    Dim sFileBinary As String = String.Empty
                    If aValues(2) <> "" Then
                        sFilePath = ToSafeString(aValues(2))
                        sFileName = Path.GetFileName(sFilePath)
                        sFilePath = Path.GetDirectoryName(sFilePath)
                    End If
                    For Each drRows As DataRow In m_dtStdWording.Rows
                        If drRows(0) = sDataStructureName AndAlso drRows(1) = aValues(0) Then
                            sFileBinary = drRows(2)
                            Exit For
                        End If
                    Next
                    oXMLElement = addChildElement(xmlDoc, oXMLElement, sPrefix, "")
                    addChildElement(xmlDoc, oXMLElement, "SWCODE", aValues(0))
                    addChildElement(xmlDoc, oXMLElement, "SWDESC", aValues(1))
                    addChildElement(xmlDoc, oXMLElement, "FileName", sFileName)
                    addChildElement(xmlDoc, oXMLElement, "FileBinary", sFileBinary)
                    addChildElement(xmlDoc, oXMLElement, "FilePath", sFilePath)

                    oXMLElement = oXMLElementPrevious
                End If

            End If

        Next
        sOutPut = xmlDoc.OuterXml


    End Sub

    ''' <summary>
    ''' Add child node to XML
    ''' </summary>
    ''' <param name="odoc"></param>
    ''' <param name="sFieldName"></param>
    ''' <param name="sValue"></param>
    ''' <remarks></remarks>
    Private Sub addChildElement(ByVal odoc As XmlDocument, ByVal sFieldName As String, ByVal sValue As String)
        sValue = sValue.Trim()
        ' Create a new element and add it to the document.
        Dim elem As XmlElement = odoc.CreateElement(sFieldName)
        elem.InnerText = sValue
        odoc.DocumentElement.AppendChild(elem)
    End Sub

    ''' <summary>
    ''' Add child node to XML
    ''' </summary>
    ''' <param name="odoc"></param>
    ''' <param name="oElement"></param>
    ''' <param name="sFieldName"></param>
    ''' <param name="sValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function addChildElement(ByVal odoc As XmlDocument, ByVal oElement As XmlElement, ByVal sFieldName As String, ByVal sValue As String) As XmlElement
        sValue = sValue.Trim()
        ' Create a new element and add it to the document.
        Dim elem As XmlElement = odoc.CreateElement("", sFieldName, kxmlns)
        elem.InnerText = sValue
        oElement.AppendChild(elem)
        Return elem
    End Function

    ''' <summary>
    ''' Create CCM clauses as temp location
    ''' </summary>
    ''' <param name="sTmpFile"></param>
    ''' <param name="sFileName"></param>
    ''' <remarks></remarks>
    Private Sub CopySubDocToCCMFolder(ByVal sTmpFile As String, ByVal sFileName As String)
        Dim nDestFilePath As String = m_sDocTemplatePath & m_lInsuranceFileCnt & "\"

        Dim xmlData As String = ""
        Dim dataBuffer() As Byte
        If Not Directory.Exists(nDestFilePath) Then
            Directory.CreateDirectory(nDestFilePath)
        End If
        If m_sClauses Is Nothing Then
            ReDim Preserve m_sClauses(0)
        Else
            ReDim Preserve m_sClauses(UBound(m_sClauses) + 1)
        End If
        sFileName = sFileName.Replace(".xml", ".docx")
        Dim oWordDocument As New Document(sTmpFile)
        oWordDocument.Save(System.IO.Path.GetDirectoryName(sTmpFile) & "\" & sFileName, SaveFormat.Docx)
        dataBuffer = System.IO.File.ReadAllBytes(System.IO.Path.GetDirectoryName(sTmpFile) & "\" & sFileName)
        m_dtStdWording.Rows(m_dtStdWording.Rows.Count - 1).Item(2) = Convert.ToBase64String(dataBuffer)
        m_sClauses(UBound(m_sClauses)) = Convert.ToBase64String(dataBuffer)

        oWordDocument.Save(nDestFilePath & "\" & sFileName, SaveFormat.Docx)


    End Sub
    ''' <summary>
    ''' Create CCM Signature as temp location
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CopySignatureToCcmFolder() As String
        Try
            Dim sDestFilePath As String = m_sDocTemplatePath & "Signature" & "\"

            If Not Directory.Exists(sDestFilePath) Then
                Directory.CreateDirectory(sDestFilePath)
            End If
            Dim sSourcePath As String = String.Empty
            'Getting saved user signature
            GetUserSignatureFile(sSourcePath)
            'Saving image to Jpeg
            Dim imgFormat As System.Drawing.Imaging.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg
            Dim objBmp As New Bitmap(sSourcePath)
            Dim sNewFile As String = Path.GetFileNameWithoutExtension(sSourcePath)
            '  sNewFile &= "." & imgFormat.ToString
            '   objBmp.Save(sNewFile, imgFormat)


            Dim dest As String = Path.Combine(sDestFilePath, sNewFile)

            If (System.IO.File.Exists(dest)) Then
                System.IO.File.Delete(dest)
            End If
            System.IO.File.Copy(sSourcePath, dest)
            'returing File Path of CCM Server
            Return sDestFilePath & sNewFile
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' get wpfields details for CCM clauses
    ''' </summary>
    ''' <param name="sTableName"></param>
    ''' <param name="sColumnName"></param>
    ''' <param name="r_dtSubGrpDetails"></param>
    ''' <remarks></remarks>
    Private Sub GetSubGroupDetailsForEndorsement(ByVal sTableName As String, ByVal sColumnName As String, ByRef r_dtSubGrpDetails As DataTable)
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sDocumentFieldsetFieldList As String = String.Empty
        Dim oArray As String() = Nothing

        Dim oCCMDocumentProdBusiness As bCCMDocumentProduction.Business = New bCCMDocumentProduction.Business
        nResult = oCCMDocumentProdBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=(m_oDatabase))
        If nResult <> PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ''stored proc call to get fieldsets from DB
        nResult = oCCMDocumentProdBusiness.GetSubGroupDetailsForEndorsement(sTableName, sColumnName, r_dtSubGrpDetails)
        If nResult <> PMEReturnCode.PMTrue Then
            Exit Sub
        End If
    End Sub

    ''''***************Below code come from  ExpressionParser ( as suggested by Deepak Arrora ) *******
    ''**************************************************************************************************



    ' ***************************************************************** '
    ' Name          : InitialiseExpressionParser
    ' Description   : Initialise Variable Table Array, and other initialisation
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Edit History  :
    ' RAM20030417   : Created
    ' *****************************************************************
    Public Function InitialiseExpressionParser(ByVal v_bNewDocument As Boolean) As Integer

        ' PW180803 - CQ1734 - only do this once for the life of the component
        ' to make variables truly global for the lifetime of the documents
        ' processing (including clauses/headers and footer)

        Dim result As Integer = 0
        Static bDone As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not v_bNewDocument AndAlso bDone Then Return result

            m_sFieldStartMarker = "&lt;@"
            m_sFieldEndMarker = "@&gt;"
            m_iFieldMarkerLength = m_sFieldStartMarker.Length

            VTtop = -1
            Erase VT

            bDone = True

            Return result

        Catch
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function



    Public Function GetVariableValue(ByVal sName As String) As String

        Dim result As String = String.Empty
        Dim iCounter As Integer
        Dim Found As Boolean

        Try

            result = ""

            If VTtop = -1 Then
                ' We dont' have any variable set
                Return result
            End If

            sName = sName.Trim().ToUpper()
            VTtop = VT.GetUpperBound(0)
            Found = False
            For iCounter = 0 To VTtop
                If VT(iCounter).name = sName Then
                    Found = True
                    Exit For
                End If
            Next

            If Found And iCounter >= 0 And iCounter <= VTtop Then

                result = CStr(VT(iCounter).value)
            End If

            Return result

        Catch excep As System.Exception



            result = ""

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVariableValue Failed for : " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVariableValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : SetVariableValue
    ' Description   : Search if Variable already exists in  Variable Table,
    '                   if not found, add it
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Edit History  :
    ' RAM20030417   : Created
    ' ***************************************************************** '
    Public Function SetVariableValue(ByVal sName As String, ByVal vValue As String) As Integer

        Dim result As Integer = 0
        Dim iCounter As Integer
        Dim Found As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If VTtop = -1 Then
                VTtop = 0
                ReDim VT(VTtop)
                VT(VTtop).name = sName

                VT(VTtop).value = vValue
            Else
                VTtop = VT.GetUpperBound(0)
                For iCounter = 0 To VTtop
                    If VT(iCounter).name = sName Then
                        Found = True
                        Exit For
                    End If
                Next

                If Found Then
                    ' We have an existing variable
                    ' We need to store it

                    VT(iCounter).value = vValue
                Else
                    VTtop += 1
                    ReDim Preserve VT(VTtop)
                    VT(VTtop).name = sName

                    VT(VTtop).value = vValue
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetVariableValue Failed for : " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetVariableValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : ParseFunctionNameAndParams
    ' Description   : Function to parse the input string for a function name
    '                   and its arguments
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Notes         : 1. Currently Nesting of Functions are not supported
    '                 2. You can pass Varaiables as an argument to the function
    '                 3. This function assumes that the arguments are
    '                    a) comma separated
    '                    b) string arguments are enclosed in double quotes i.e.  ??
    '                    c) numeric arguments are not enclosed in double quotes
    '                    d) Varaible Expressions are enclosed in <@VAR_n@>
    ' Example Input : FUNC_LCase(?UPPERCASE String?)
    ' Edit History  :
    ' RAM20030416   : Created
    ' PW060404 - CQ5295 - do not replace the chr(147) and chr(148) with
    '                 quotation marks. There could be a quotation mark in
    '                 the field value, so we can't delimit with it.
    ' ***************************************************************** '
    Public Function ParseFunctionNameAndParams(ByVal v_vInputString As String, ByRef r_sFunctionName As String, ByRef r_vParamArray As Object) As Integer
        Dim result As Integer = 0

        Dim iStart, iEnd, iLen As Integer
        Dim vArgumentValue, strFunctionName, strArguments As String
        Dim vArgumentsArray As Object
        Dim iNoofArguments As Integer
        Dim strTemp As String
        Dim strVariableName As String = ""
        Dim vVariableValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            vArgumentsArray = ""

            If v_vInputString.StartsWith("FUNC_") Then
                'Strip off the leading FUNC_
                v_vInputString = v_vInputString.Substring(5)
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iStart = 1
            iEnd = (v_vInputString.IndexOf("("c) + 1)
            If iEnd > 1 Then ' !!!! Function name can't be one character
                iEnd -= 1
            End If

            iLen = v_vInputString.Length

            ' Get the Function Name
            strFunctionName = v_vInputString.Substring(iStart - 1, System.Math.Min(v_vInputString.Length, iEnd))

            iStart = iStart + strFunctionName.Length + 1
            ' PW071003 - CQ2759 - Find last ')' because there could be
            ' one in the literal string
            iEnd = IIf(v_vInputString = "" And ")" = "", 0, (v_vInputString.LastIndexOf(")") + 1))
            If iEnd > 1 Then
                strArguments = v_vInputString.Substring(iStart - 1, System.Math.Min(v_vInputString.Length, iEnd - iStart))
            End If
            If strFunctionName.ToUpper() = "LEN" Then
                strArguments = RemoveDoubleQuotes(strArguments)
            End If

            ' Check if we have any arguments
            If strArguments.Length > 0 Then

                ' Note : Some FUNNY things are going on here with the arguments.
                '        If the HTML arguments contains spaces, they are stored either as
                '        chr(20) =  or chr(160),  which looks like space, but they are not.
                '        so we need to replace then with standard spaces - chr(32)
                strArguments = strArguments.Replace(Chr(20).ToString(), " ")
                strArguments = strArguments.Replace(Chr(160).ToString(), " ")

                ' Make it into Array (since arguments are comma separated)
                If strFunctionName = "CNUM" Or strFunctionName.ToUpper() = "LEN" Then
                    ReDim vArgumentsArray(0)
                    vArgumentsArray(0) = strArguments
                Else
                    Dim strResult As String = String.Empty
                    If strFunctionName.ToLower() = "instr" AndAlso strArguments.Contains(",") AndAlso strArguments.Split(",").Length - 1 > 1 Then
                        strResult = Replace(Mid(strArguments, 1, strArguments.LastIndexOf(",")), ",", ";5;") &
                                                  Right(strArguments, Len(strArguments) - strArguments.LastIndexOf(","))
                    Else
                        strResult = strArguments
                    End If

                    vArgumentsArray = ProperSplit(strResult, ",")

                    For count As Integer = 0 To vArgumentsArray.GetUpperBound(0)
                        vArgumentsArray(count) = Convert.ToString(vArgumentsArray(count)).Replace(";5;", ",")
                    Next

                End If
                ' Check all the arugments for values, it may be any <@VAR_1@>, if so, solve the variable


                iNoofArguments = vArgumentsArray.GetUpperBound(0)

                For iCounter As Integer = 0 To iNoofArguments

                    vArgumentsArray(iCounter) = RemoveDoubleQuotes(CStr(vArgumentsArray(iCounter)))
                    vArgumentValue = CStr(vArgumentsArray(iCounter))

                    ' Resolve the Variable Value

                    ' Check if we have a variable
                    strTemp = vArgumentValue
                    If strTemp.IndexOf(m_sFieldStartMarker) + 1 Then

                        ' Remove the m_sFieldStartMarker ( &lt;@  )
                        strTemp = strTemp.Replace(m_sFieldStartMarker, "")

                        ' Remove the  m_sFieldEndMarker (  @&gt;  )
                        strTemp = strTemp.Replace(m_sFieldEndMarker, "")

                        ' We got the Variable Name
                        strVariableName = strTemp

                        ' Get the Variable's value
                        vVariableValue = GetVariableValue(sName:=strVariableName)

                        ' Set the value back into the Arguments Array

                        vArgumentsArray(iCounter) = vVariableValue

                    End If

                Next iCounter
            Else
                ReDim vArgumentsArray(0)
                vArgumentsArray(0) = ""
            End If

            ' Return the Values
            r_sFunctionName = strFunctionName


            r_vParamArray = vArgumentsArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ParseFunctionNameAndParams Failed for : " & v_vInputString, vApp:=ACApp, vClass:=ACClass, vMethod:="ParseFunctionNameAndParams", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : CallBuiltInFunction
    ' Description   : Execute the built-in function, and return its result
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Note          : The following functions are supported.
    '
    '    Concat(String1, String2)           Concatenates two strings
    '    Mod(Number1, Number2)              Returns the Modulus of two numbers
    '    Chr(CharCode)                      Returned a character given the ASCII code
    '    InStr(String1, String2)            Returns the position of String2 within String1
    '    LCase(String)                      Converts a string to lowercase
    '    UCase(String)                      Converts a string to Uppercase
    '    Len(String)                        Returns the length of a string
    '    Mid(String, Start, Length)         Returns a portion of a string
    '    StrComp(String1, String2)          Returns 1 is String1 = String2, otherwise 0
    '    String(Number, Character)          Builds a string of a number of characters
    '    Left(String, Length)               Returns a left hand section of a string
    '    Right(String, Length)              Returns a right hand section of a string
    '    LTrim (String)                     Trims the spaces from the left of a string
    '    RTrim (String)                     Trims the spaces from the right of a string
    '    Date()                             Returns the current date
    '    Time()                             Returns the current time
    '    DateAdd(Interval, Number, Date)    Adds an interval to a date
    '    DateDiff(Interval, Date1, Date2)   Returns the difference between two dates
    '    DatePart(Interval, Date)           Returns part of a date (e.g. day, month, year)
    '    FormatDateTime(Date,Format)        Formats a date to a given format
    '    WeekdayName (Day)                  Returns the name of a weekday number
    '    MonthName (Month)                  Returns the name of a month number
    '    IsDate(String)                     Returns 1 if string is a valid date, otherwise 0
    '    FormatCurrency (Value)             Formats a number into a currency value
    '    Replace(Expression, Find, Replace) Replaces part of a string
    '
    ' NOTE 2        : If more functions are to be added, add it in the select case
    '                   statement and also make sure that, IsBuiltInFunction is also
    '                   updated
    ' Edit History  :
    ' RAM20030416   : Created
    ' PW230603 - Added two new functions (CR103)
    '    CNum(String)                       Converts a string to a number
    '    CStr(Number)                       Converts a number to a string
    ' PW240604 - CQ5678 - add byref parameter, to indicate if function
    '                     result is a numeric value
    ' ***************************************************************** '
    Public Function CallBuiltinFunction(ByVal v_sFunctionName As String, ByVal v_vParamArray() As Object, ByRef r_bResultIsNumeric As Boolean) As String

        Dim result As String = String.Empty
        Dim strTemp As String = ""
        Dim vReturnValue As Object
        Dim iNoOfArgumentNeeded As Integer

        Try

            result = ""
            r_bResultIsNumeric = False

            If Not IsBuiltInFunction(v_sFunctionName) Then
                Return ""
            End If

            strTemp = v_sFunctionName.Trim().ToUpper()

            Select Case strTemp
                Case "CONCAT"
                    '   Concat(String1, String2)            Concatenates two strings
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeString(v_vParamArray(0)) & ToSafeString(v_vParamArray(1))
                    End If
                Case "MOD"
                    '    Mod(Number1, Number2)              Returns the Modulus of two numbers
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeDouble(v_vParamArray(0)) Mod ToSafeDouble(v_vParamArray(1))
                    End If
                Case "CHR"
                    '    Chr(CharCode)                      Returned a character given the ASCII code
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = Chr(ToSafeInteger(v_vParamArray(0))).ToString()
                    End If
                Case "INSTR"
                    '    InStr(String1, String2)            Returns the position of String2 within String1
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = Replace(v_vParamArray(0), Chr(34), "")
                        v_vParamArray(1) = Replace(v_vParamArray(1), Chr(34), "")
                        vReturnValue = (ToSafeString(v_vParamArray(0)).IndexOf(v_vParamArray(1)) + 1)
                    End If
                Case "LCASE"
                    '    LCase(String)                      Converts a string to lowercase
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = Replace(v_vParamArray(0), Chr(34), "")
                        vReturnValue = ToSafeString(v_vParamArray(0)).ToLower()
                    End If
                Case "UCASE"
                    '    UCase(String)                      Converts a string to Uppercase
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = Replace(v_vParamArray(0), Chr(34), "")
                        vReturnValue = ToSafeString(v_vParamArray(0)).ToUpper()
                    End If
                Case "LEN"
                    '    Len(String)                        Returns the length of a string
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = Len(ToSafeString(v_vParamArray(0)))
                    End If
                Case "MID"
                    '    Mid(String, Start, Length)         Returns a portion of a string
                    iNoOfArgumentNeeded = 3
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = Replace(v_vParamArray(0), Chr(34), "")
                        v_vParamArray(1) = Replace(v_vParamArray(1), Chr(34), "")
                        v_vParamArray(2) = Replace(v_vParamArray(2), Chr(34), "")
                        If ToSafeInteger(v_vParamArray(1)) > 0 Then
                            vReturnValue = Mid(ToSafeString(v_vParamArray(0)), ToSafeInteger(v_vParamArray(1)), ToSafeInteger(v_vParamArray(2)))
                        End If
                    End If
                Case "STRCOMP"
                    '    StrComp(String1, String2)          Returns 1 is String1 = String2, otherwise 0
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        If v_vParamArray(0).Equals(v_vParamArray(1)) Then
                            vReturnValue = 1
                        Else
                            vReturnValue = 0
                        End If
                    End If
                Case "STRING"
                    '    String(Number, Character)          Builds a string of a number of characters
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = New String(Chr(v_vParamArray(1)), ToSafeInteger(v_vParamArray(0)))
                    End If
                Case "LEFT"
                    '    Left(String, Length)               Returns a left hand section of a string
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        If Len(v_vParamArray(0)) < ToSafeInteger(v_vParamArray(1)) Then
                            v_vParamArray(1) = Len(v_vParamArray(0)).ToString
                        End If
                        vReturnValue = ToSafeString(v_vParamArray(0)).Substring(0, ToSafeInteger(v_vParamArray(1)))
                    End If
                Case "RIGHT"
                    '    Right(String, Length)              Returns a right hand section of a string
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeString(v_vParamArray(0)).Substring(ToSafeString(v_vParamArray(0)).Length - ToSafeInteger(v_vParamArray(1)))
                    End If
                Case "LTRIM"
                    '    LTrim (String)                     Trims the spaces from the left of a string
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeString(v_vParamArray(0)).TrimStart()
                    End If
                Case "RTRIM"
                    '    RTrim (String)                     Trims the spaces from the right of a string
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeString(v_vParamArray(0)).TrimEnd()
                    End If
                Case "DATE"
                    '    Date()                             Returns the current date
                    iNoOfArgumentNeeded = 0

                    vReturnValue = DateTime.Today
                Case "TIME"
                    '    Time()                             Returns the current time
                    iNoOfArgumentNeeded = 0

                    vReturnValue = DateTimeHelper.Time
                Case "DATEADD"
                    '    DateAdd(Interval, Number, Date)    Adds an interval to a date
                    iNoOfArgumentNeeded = 3
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then

                        v_vParamArray(0) = Replace(v_vParamArray(0), Chr(34), "")
                        v_vParamArray(1) = Replace(v_vParamArray(1), Chr(34), "")
                        v_vParamArray(2) = Replace(v_vParamArray(2), Chr(34), "")
                        vReturnValue = DateAndTime.DateAdd(ToSafeString(v_vParamArray(0)), v_vParamArray(1), ToSafeDate(v_vParamArray(2)))
                    End If
                Case "DATEDIFF"
                    '    DateDiff(Interval, Date1, Date2)   Returns the difference between two dates
                    iNoOfArgumentNeeded = 3
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = Replace(v_vParamArray(0), Chr(34), "")
                        v_vParamArray(1) = Replace(v_vParamArray(1), Chr(34), "")
                        v_vParamArray(2) = Replace(v_vParamArray(2), Chr(34), "")
                        vReturnValue = DateDiff(ToSafeString(v_vParamArray(0)), v_vParamArray(1), ToSafeDate(v_vParamArray(2)))
                    End If
                Case "DATEPART"
                    '    DatePart(Interval, Date)           Returns part of a date (e.g. day, month, year)
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = Informations.DatePart(ToSafeString(v_vParamArray(0)), ToSafeDate(v_vParamArray(1)), DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
                    End If
                Case "FORMATDATETIME"
                    '    FormatDateTime(Date,Format)        Formats a date to a given format
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = FormatDateTime(ToSafeDate(v_vParamArray(0)), Conversion.Val(ToSafeString(v_vParamArray(1))))
                    End If
                Case "WEEKDAYNAME"
                    '    WeekdayName (Day)                  Returns the name of a weekday number
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = DateTimeFormatInfo.CurrentInfo.GetDayName(ToSafeInteger(v_vParamArray(0)) - 1)
                    End If
                Case "MONTHNAME"
                    '    MonthName (Month)                  Returns the name of a month number
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = DateTimeFormatInfo.CurrentInfo.GetMonthName(ToSafeInteger(v_vParamArray(0)))
                    End If
                Case "ISDATE"
                    '    IsDate(String)                     Returns 1 if string is a valid date, otherwise 0
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        If Information.IsDate(v_vParamArray(0)) Then
                            vReturnValue = 1
                        Else
                            vReturnValue = 0
                        End If
                    End If
                Case "FORMATCURRENCY"
                    '    FormatCurrency (Value)             Formats a number into a currency value
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeDouble(v_vParamArray(0)).ToString("C")
                    End If
                Case "REPLACE"
                    '    Replace(Expression, Find, Replace) Replaces part of a string
                    iNoOfArgumentNeeded = 3
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeString(v_vParamArray(0)).Replace(ToSafeString(Trim(v_vParamArray(1))), ToSafeString(Trim(v_vParamArray(2))))
                    End If
                ' PW230603 - CR103: start
                Case "CSTR"
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = RemoveDoubleQuotes(v_vParamArray(0))
                        vReturnValue = ToSafeString(v_vParamArray(0))
                    End If
                Case "CNUM"
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeDouble(v_vParamArray(0))
                    End If
                    r_bResultIsNumeric = True
                Case "CDATE"
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeDate(v_vParamArray(0))
                    End If
                Case "MONTH"
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = Replace(v_vParamArray(0), Chr(34), "")
                        vReturnValue = Month(v_vParamArray(0))
                    End If
                ' PW230603 - CR103: end
                Case "FORMATNUMBER"
                    iNoOfArgumentNeeded = 5
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        If (Informations.IsNumeric(v_vParamArray(0))) Then
                            vReturnValue = FormatNumber(ToSafeDouble(ToSafeString(v_vParamArray(0)).Replace(ChrW(34), "")),
                                                        ToSafeInteger(v_vParamArray(1)),
                                                        ToSafeInteger(v_vParamArray(2)),
                                                        ToSafeInteger(v_vParamArray(3)),
                                                        ToSafeInteger(v_vParamArray(4)))
                            Dim numDigitsAfterDecimal As Integer = ToSafeInteger(v_vParamArray(1))
                            Dim includeLeadingDigit As Integer = ToSafeInteger(If(Convert.ToInt32(v_vParamArray(2)) = -2, 1, v_vParamArray(2)))
                            Dim useParensForNegativeNumbers As Integer = ToSafeInteger(If(Convert.ToInt32(v_vParamArray(3)) = -2, 0, v_vParamArray(2)))
                            Dim groupDigits As Integer = ToSafeInteger(If(Convert.ToInt32(v_vParamArray(4)) = -2, 1, v_vParamArray(2)))
                            Dim value As String = ToSafeString(v_vParamArray(0)).Replace(ChrW(34), "")

                            vReturnValue = CDbl(value).ToString(GetNumbersformatString(numDigitsAfterDecimal, includeLeadingDigit, useParensForNegativeNumbers, groupDigits))
                        Else
                            vReturnValue = FormatNumber(ToSafeDouble(Trim(v_vParamArray(0)).Replace(ChrW(34), "")),
                                                        ToSafeInteger(v_vParamArray(1)),
                                                        ToSafeInteger(v_vParamArray(2)),
                                                        ToSafeInteger(v_vParamArray(3)),
                                                        ToSafeInteger(v_vParamArray(4)))
                        End If
                    Else
                        If CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator = "," Then
                            Dim sParam As String = String.Empty
                            Dim nLength As Integer = v_vParamArray.Length
                            For i As Integer = 0 To nLength - 5
                                sParam = sParam + v_vParamArray(i).ToString()
                            Next
                            v_vParamArray(0) = sParam
                            vReturnValue = FormatNumber(ToSafeDouble(Trim(v_vParamArray(0)).Replace(ChrW(34), "")),
                                                      ToSafeInteger(v_vParamArray(nLength - 4)),
                                                      ToSafeInteger(v_vParamArray(nLength - 3)),
                                                      ToSafeInteger(v_vParamArray(nLength - 2)),
                                                      ToSafeInteger(v_vParamArray(nLength - 1)))
                        End If
                    End If
                    r_bResultIsNumeric = True
                Case "ROUND"
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = gPMMaths.PMRoundupValueVDecimal(ToSafeDecimal(v_vParamArray(0)), ToSafeInteger(v_vParamArray(1)), PMERoundupFactor.pmeRFactor50Up)
                    End If
                    r_bResultIsNumeric = True
            End Select

            ' Return the result


            Return ToSafeString(vReturnValue)

        Catch excep As System.Exception
            result = ""
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallBuiltInFunction Failed for : " & v_sFunctionName, vApp:=ACApp, vClass:=ACClass, vMethod:="CallBuiltInFunction", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function
    Private Function GetNumbersformatString(digitsAfterDecimal As Integer, includeLeadingDigit As Integer, useParensForNegative As Integer, groupDigits As Integer) As String

        ' Convert VB6 TriState to Boolean defaults
        Dim includeLeading As Boolean = (includeLeadingDigit <> 0)
        Dim useParens As Boolean = (useParensForNegative <> 0)
        Dim useGrouping As Boolean = (groupDigits <> 0)

        ' ---- Decimal portion ----
        Dim decimalPart As String = String.Empty
        If digitsAfterDecimal > 0 Then
            decimalPart = "." & New String("0"c, digitsAfterDecimal)
        End If

        ' ---- Integer portion ----
        Dim integerPart As String = If(includeLeading, "0", "#")

        ' ---- Positive format ----
        Dim positiveFormat As String =
        If(useGrouping,
           "#,##" & integerPart & decimalPart,
           integerPart & decimalPart)

        ' ---- Negative format ----
        Dim negativeNumber As String = positiveFormat

        If useParens Then
            negativeNumber = "(" & negativeNumber & ")"
        Else
            negativeNumber = "-" & negativeNumber
        End If

        ' ---- Final format ----
        Return positiveFormat & ";" & negativeNumber

    End Function

    ' ***************************************************************** '
    ' Name          : IsBuiltInFunction
    ' Description   : Check if a string name does stand for a supported built-in function
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Note          : The following functions are supported.
    ' Currently We are supporting the following functions
    '        Concat(String1, String2)
    '        Mod(Number1, Number2)
    '        Chr (CharCode)
    '        InStr(String1, String2)
    '        LCase(String)
    '        UCase(String)
    '        Len(String)
    '        Mid(String, Start, Length)
    '        StrComp(String1, String2)
    '        String(Number, Character)
    '        Left(String, Length)
    '        Right(String, Length)
    '        LTrim (String)
    '        RTrim (String)
    '        Date()
    '        Time()
    '        DateAdd(Interval, Number, Date)
    '        DateDiff(Interval, Date1, Date2)
    '        DatePart(Interval, Date)
    '        FormatDateTime(Date,Format)
    '        WeekdayName (Day)
    '        MonthName (Month)
    '        IsDate(String)
    '        FormatCurrency (value)
    '        Replace(Expression, Find, Replace)
    ' Edit History  :
    ' RAM20030416   : Created
    ' PW230603 - Added two new functions (CR103)
    '    CNum(String)                       Converts a string to a number
    '    CStr(Number)                       Converts a number to a string
    ' ***************************************************************** '
    Public Function IsBuiltInFunction(ByVal v_sFunctionName As String) As Boolean


        Dim TempName As String = v_sFunctionName.Trim().ToUpper()

        If TempName = "ROUND" Or TempName = "CONCAT" Or TempName = "MOD" Or TempName = "CHR" Or TempName = "INSTR" Or TempName = "LCASE" Or TempName = "UCASE" Or TempName = "LEN" Or TempName = "MID" Or TempName = "STRCOMP" Or TempName = "STRING" Or TempName = "LEFT" Or TempName = "RIGHT" Or TempName = "LTRIM" Or TempName = "RTRIM" Or TempName = "DATE" Or TempName = "TIME" Or TempName = "DATEADD" Or TempName = "DATEDIFF" Or TempName = "DATEPART" Or TempName = "FORMATDATETIME" Or TempName = "WEEKDAYNAME" Or TempName = "MONTHNAME" Or TempName = "ISDATE" Or TempName = "FORMATCURRENCY" Or TempName = "CNUM" Or TempName = "CSTR" Or TempName = "REPLACE" Or TempName = "CDATE" Or TempName = "MONTH" Or TempName = "FORMATNUMBER" Then

            Return True
        Else
            Return False
        End If

    End Function

    ' ***************************************************************** '
    ' Name          : IsValidArguments
    ' Description   : Check if arguments array matches with the no of
    '                   arguments needed
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Note          : The following functions are supported.
    ' Edit History  :
    ' RAM20030417   : Created
    ' ***************************************************************** '
    Public Function IsValidArguments(ByVal v_vInputArray() As Object, ByVal v_iNoOfParams As Integer) As Boolean

        Dim result As Boolean = False

        If Information.IsArray(v_vInputArray) Then
            If v_vInputArray.GetUpperBound(0) = v_iNoOfParams - 1 Then ' Base Zero
                result = True
            End If
        End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name          : ProperSplit
    ' Description   : This function is for use when parsing(splitting) a
    '                   data string that has a comma delimiter.
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Note          : This is a replacement for the VB6 Split Function
    ' Edit History  :
    ' RAM20030422   : Created
    ' PW060404 - CQ5295 - do not delimit with quotation marks.
    '                 There could be a quotation mark in
    '                 the field value, so we can't delimit with it. Use the
    '                 word quote marks instead (chr(147) and (148) like the
    '                 rest of doc production does.
    ' ***************************************************************** '
    Public Function ProperSplit(ByVal v_sInputString As String, Optional ByVal v_sDelimter As String = ",") As Object

        'This function is for use when parsing(splitting) a data string that
        'has a comma delimiter. The normal VB Split function does not take into
        'consideration of a comma embedded within a Fields' data string and
        'will parse the information incorrectly.
        '
        '
        'This function takes into consideration the a data field may contain
        'a comma and parses the data as entire string. The data string being defined
        'as the data between the two Double-Quote marks. This function also
        'prunes the leading and trailing double quote marks
        '
        ' Notes : Does NOT Correct improperly formatted Numeric amounts that
        ': contain a comma for the thousands placement, unless the number has
        ': leading and trailing Double-Quote marks.
        '
        ' Call : X() = ProperSplit(datastring to split.)
        '
        ' Returns: Single-Dimension array, same result that you get from the SPLIT Function.

        Dim result As Object = Nothing
        Dim iStringLength, iDelimPosition As Integer
        Dim sDoubleQuoteMark As String = ""
        Dim iIndex As Integer
        Dim aData1() As String = Nothing
        Dim sDatafield As String = ""
        Dim iDQPos1, iDQPos2 As Integer

        Try


            iStringLength = v_sInputString.Length
            iIndex = -1 ' To Make the return array it Zero Base
            '
            ' if the length of the data string is greater than zero

            If iStringLength > 0 Then
                ' search for a v_sDelimteriter in the datastring
                iDelimPosition = (v_sInputString.IndexOf(v_sDelimter) + 1)
                '
                Do While iDelimPosition <> 0
                    ' do while there is a v_sDelimteriter
                    ' search for a quote-enclosure set.
                    iDQPos1 = (v_sInputString.IndexOf(Chr(147).ToString()) + 1)
                    sDatafield = ""
                    '
                    If iDQPos1 <> 0 And iDQPos1 < iDelimPosition Then
                        ' found Double quote mark, and it is found BEFORE
                        ' the v_sDelimteriter. Search for matching Double Quote Mark
                        iDQPos2 = InStr(iDQPos1 + 1, v_sInputString, Chr(148).ToString())

                        If iDQPos2 <> 0 Then

                            If iDQPos2 = v_sInputString.Length Then
                                ' this is the last field of data so we remove the
                                ' surrounding Double-Quote Marks.
                                v_sInputString = v_sInputString.Substring(v_sInputString.Length - (v_sInputString.Length - 1))
                                v_sInputString = v_sInputString.Substring(0, v_sInputString.Length - 1)
                                'exit the Do loop and
                                Exit Do
                            End If
                            ' Just found the Matching double Quote Mark
                            ' data field ends at iDQPos2, not iDelim Position
                            sDatafield = v_sInputString.Substring(0, iDQPos2)
                            v_sInputString = v_sInputString.Substring(v_sInputString.Length - (v_sInputString.Length - (sDatafield.Length + 1)))
                            sDatafield = sDatafield.Substring(sDatafield.Length - (sDatafield.Length - 1))
                            sDatafield = sDatafield.Substring(0, sDatafield.Length - 1)
                            iIndex += 1
                        Else
                            ' unmatched double quote usually specifies error with the
                            ' data being read in.
                        End If
                    Else

                        If iDQPos1 <> 0 Then
                            ' Quote mark is FOUND AFTER the v_sDelimteriter meaning the
                            ' data to the v_sDelimteriter is ok to use as a full field.
                            ' Data ends at the v_sDelimteriter.
                            sDatafield = v_sInputString.Substring(0, iDelimPosition - 1)
                            v_sInputString = v_sInputString.Substring(v_sInputString.Length - (v_sInputString.Length - (sDatafield.Length + 1)))
                            iIndex += 1
                        Else
                            ' there is NO double Quote Mark Found.
                            sDatafield = v_sInputString.Substring(0, iDelimPosition - 1)
                            v_sInputString = v_sInputString.Substring(v_sInputString.Length - (v_sInputString.Length - iDelimPosition))
                            iIndex += 1
                        End If
                    End If
                    ReDim Preserve aData1(iIndex)

                    ' Remove any Starting double quote and ending doube quote

                    If sDatafield.Substring(0, 1) = Chr(147).ToString() Then
                        sDatafield = v_sInputString.Substring(1)
                    End If

                    If sDatafield.Substring(sDatafield.Length - 1) = Chr(148).ToString() Then
                        sDatafield = sDatafield.Substring(0, sDatafield.Length - 1)
                    End If

                    aData1(iIndex) = sDatafield
                    iDelimPosition = (v_sInputString.IndexOf(v_sDelimter) + 1)
                Loop
                iIndex += 1
                ReDim Preserve aData1(iIndex)

                ' Remove any Starting double quote and ending doube quote
                If v_sInputString.Trim <> "" Then
                    If v_sInputString.Substring(0, 1) = Chr(147).ToString() Then
                        v_sInputString = v_sInputString.Substring(1)
                    End If

                    If v_sInputString.Substring(v_sInputString.Length - 1) = Chr(148).ToString() Then
                        v_sInputString = v_sInputString.Substring(0, v_sInputString.Length - 1)
                    End If
                End If


                aData1(iIndex) = v_sInputString
            Else
            End If

            Return aData1

        Catch excep As System.Exception
            result = ""

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProperSplit Failed for : " & v_sInputString, vApp:=ACApp, vClass:=ACClass, vMethod:="ProperSplit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : NoOfOccurences
    ' Description   : This function is for use to Count # of occurrences of
    '                   a search string with in a string.
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Params        :(IN) v_sSource         to search,
    '                (IN) v_sStringToMatch  to look for.
    ' Returns       : # of occurrences.
    ' Edit History  :
    ' RAM20030423   : Created
    ' ***************************************************************** '
    Function NoOfOccurences(ByVal v_sSource As String, ByVal v_sStringToMatch As String) As Integer

        Dim result As Integer = 0
        Dim nPos, nCount, iOffset As Integer

        Try

            ' Check if we have valid strings
            If v_sSource.Length = 0 Or v_sStringToMatch.Length = 0 Then
                Return 0
            End If

            nCount = 0
            iOffset = v_sStringToMatch.Length
            nPos = (v_sSource.IndexOf(v_sStringToMatch) + 1)

            While nPos
                nCount += 1
                nPos = InStr(nPos + iOffset, v_sSource, v_sStringToMatch)
            End While

            ' Return the no of occurences

            Return nCount

        Catch excep As System.Exception

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NoOfOccurences Failed for : " & v_sSource & " : " & v_sStringToMatch, vApp:=ACApp, vClass:=ACClass, vMethod:="NoOfOccurences", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : NthInstanceOf
    ' Description   : Function to Return the Position Of the Nth Instance of the String
    '                  [sSearched] Within [sString].
    '                 If Instance = 0 then returns the Index of the Last Occurrence of the
    '                 String. Negative Numbers are calculated from the Last Match
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Edit History  :
    ' RAM20030423   : Created
    ' ***************************************************************** '
    Public Function NthInstanceOf(ByVal sString As String, ByVal sSearched As String, Optional ByVal Instance As Integer = 1, Optional ByVal nStartFrom As Integer = 1, Optional ByVal eCompareMethod As String = "", Optional ByVal bFromLastMatch As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim nOccurrences, nInitStartFrom As Integer

        Try

            If sString = "" Then Return result
            If sSearched = "" Then Return result

            If bFromLastMatch Then
                Instance = -1 * Instance
            End If


            Select Case System.Math.Sign(Instance)
                Case 0
                    result = IIf(sString = "" And sSearched = "", 0, (sString.LastIndexOf(sSearched) + 1))
                Case 1
                    nStartFrom = InStr(nStartFrom, sString, sSearched, eCompareMethod)

                    Do While nStartFrom > 0
                        nOccurrences += 1

                        If nOccurrences = Instance Then
                            Return nStartFrom
                        End If

                        nStartFrom = InStr(nStartFrom + 1, sString, sSearched, eCompareMethod)
                    Loop

                Case Else

                    nInitStartFrom = nStartFrom

                    nStartFrom = IIf(sString = "" And sSearched = "", 0, (sString.LastIndexOf(sSearched) + 1))

                    Do While nStartFrom >= nInitStartFrom
                        nOccurrences -= 1

                        If nOccurrences = Instance Then
                            Return nStartFrom
                        End If

                        nStartFrom = IIf(sString = "" And sSearched = "", 0, (sString.LastIndexOf(sSearched) + 1))
                    Loop

            End Select

            Return result

        Catch excep As System.Exception
            result = 0

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NthInstanceOf Failed for : " & sString & " : " & sSearched, vApp:=ACApp, vClass:=ACClass, vMethod:="NthInstanceOf", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' Get the Document Currency
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDocCurrency() As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        nReturn = m_oBusiness.GetDocumentCurrency(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sDocumentRef:=m_sDocumentRef, r_vResultArray:=m_aoDocCurrency, v_lPartyCnt:=m_lPartyCnt)
        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to get currency detail for this policy " & m_lInsuranceFileCnt)
        End If
        Return nReturn
    End Function
    ''' <summary>
    ''' Get All the Claues 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetListOfAllClauses() As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim oDT As DataTable = Nothing
        nReturn = m_oBusiness.GetAllClauses(oDT)
        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to call GetListOfAllClauses")
        End If
        If (oDT.Rows.Count > 0) Then
            m_oListClauses = New List(Of String)
        End If
        For Each dr As DataRow In oDT.Rows
            m_oListClauses.Add(dr.Item("column_name").ToString.ToUpper)
        Next
        oDT = Nothing
        Return nReturn
    End Function
    ''' <summary>
    ''' Structure to keep the Document Template Information
    ''' </summary>
    ''' <remarks></remarks>
    Structure DocumentTemplateInfo
        Public DocCode As String
        Public DocTemplateId As Integer
        Public DocType As Integer
    End Structure
    ''' <summary>
    ''' Get all the index of str in a String
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <param name="sStr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AllIndexOf(sText As String, sStr As String) As List(Of Integer)
        Dim oIndexList As List(Of Integer) = New List(Of Integer)()
        Dim index As Integer = sText.IndexOf(sStr)
        While index <> -1
            oIndexList.Add(index + 1)
            index = sText.IndexOf(sStr, index + sStr.Length)
        End While
        Return oIndexList
    End Function
    Private Function FormatDateTime(Expression As DateTime, Optional NamedFormat As DateFormat = DateFormat.GeneralDate) As String
        Try
            Return Expression.ToString(If(NamedFormat = DateFormat.LongDate, "D",
                                           If(NamedFormat = DateFormat.ShortDate, "d",
                                              If(NamedFormat = DateFormat.LongTime, "T",
                                                 If(NamedFormat = DateFormat.ShortTime, "HH:mm",
                                                    If((Expression.TimeOfDay.Ticks <> Expression.Ticks),
                                                       If((Expression.TimeOfDay.Ticks <> 0L), "G", "d"), "T"))))), Nothing)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function IsValidExpression(input As String) As Boolean
        Dim operators = New String() {"<>", "<=", ">=", "=", "<", ">"}
        Dim op As String = Nothing

        ' Trim and normalize input
        input = input.Trim()

        ' Detect operator
        For Each o In operators
            Dim idx = input.IndexOf(o)
            If idx >= 0 Then
                op = o
                Exit For
            End If
        Next

        If op IsNot Nothing Then    ' operator found

            ' Split operands
            Dim parts = input.Split(New String() {op}, StringSplitOptions.None)

            ' we assume that there must be 2 operand against an operator
            Dim left = parts(0).Trim()
            Dim right = parts(1).Trim()

            If ((left.StartsWith("""") AndAlso left.EndsWith("""")) AndAlso
                (right.Contains("""") AndAlso right.EndsWith(""""))) Then
                Return True
            End If

            ' remove double quotes if any
            left = left.Replace(Chr(34), "").Trim()
            right = right.Replace(Chr(34), "").Trim()

            Dim tempDblVar As Decimal = 0
            Dim leftIsNumeric = Decimal.TryParse(left, tempDblVar)
            Dim rightIsNumeric = Decimal.TryParse(right, tempDblVar)

            Dim leftIsString = Not leftIsNumeric
            Dim rightIsString = Not rightIsNumeric

            ' if both are not of same type, we'll not send it to eval
            If ((leftIsNumeric And rightIsString) Or (leftIsString And rightIsNumeric)) Then
                Return False
            End If
        End If

        Return True
    End Function

End Class
