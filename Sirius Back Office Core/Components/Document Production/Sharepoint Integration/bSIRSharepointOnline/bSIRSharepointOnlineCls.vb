Imports System.Data
Imports System.Net
Imports System.Security
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.SharePoint.Client
Imports SSP.Shared

Public NotInheritable Class BusinessSharepointOnline
    Implements IDisposable

    Private Const ACClass As String = "Business"

    Private m_oDatabase As dPMDAO.Database

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_nUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_nSourceID As Integer
    Private m_nLanguageID As Integer
    Private m_nCurrencyID As Integer
    Private m_nLogLevel As Integer
    Private m_bCloseDatabase As Boolean
    Private m_nReturn As Integer
    Private m_sErrorString As String = ""
    Private kSharepointURlPath As String
    Private m_bIssharepointOnline As Boolean = False
    Private m_sSharepointUserName As String = String.Empty
    Private m_sSharepointPassword As String = String.Empty
    Private m_sDocumentLibrary As String = String.Empty
    'Built-in content type
    Const kCustomContentType As String = "PureDocument"
    Const kPureGroupName As String = "SSP Templates"

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property ErrorString() As String
        Get
            Return m_sErrorString
        End Get
    End Property
#Region "Sharepoint Online Properties"
    ''' <summary>
    ''' Holds the sharepoin Url
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SharepointUrl() As String
        Get
            Return kSharepointURlPath
        End Get

        Set(value As String)
            kSharepointURlPath = value
        End Set
    End Property
    ''' <summary>
    ''' Holds the document library
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DocumentLibrary() As String
        Get
            Return m_sDocumentLibrary
        End Get

        Set(value As String)
            m_sDocumentLibrary = value
        End Set
    End Property

    ''' <summary>
    ''' This Property will used to determine the sharepoint online configuration.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsSharePointOnline() As Boolean
        Get
            Return m_bIssharepointOnline
        End Get

        Set(value As Boolean)
            m_bIssharepointOnline = value
        End Set
    End Property
    ''' <summary>
    ''' This Property will used to hold the sharepoint UserName.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SharePointUserName() As String
        Get
            Return m_sSharepointUserName
        End Get

        Set(value As String)
            m_sSharepointUserName = value
        End Set
    End Property

    ''' <summary>
    ''' This Property will used to hold the sharepoint Password
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SharePointPassword() As String
        Get
            Return m_sSharepointPassword
        End Get

        Set(value As String)
            m_sSharepointPassword = value
        End Set
    End Property
    ''' <summary>
    ''' Create The SP property so that we can iterate using dictionlay writing same code for the Property value assignment and retreival and code reusability.
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property SPAttributes() As Dictionary(Of String, Object)
        Get
            Dim SPAttributesFields As New Dictionary(Of String, Object)
            With SPAttributesFields
                'Set the feild and type in form of Key,Value
                .Add("Title", "Text")
                .Add("DocumentGroup", "Text")
                .Add("DocumentSubGroup", "Text")
                .Add("PartyShortName", "Text")
                .Add("PartyFullName", "Text")
                .Add("PolicyNumber", "Text")
                .Add("ProductCode", "Text")
                .Add("CoverStartDate", "DateTime")
                .Add("CoverExpiryDate", "DateTime")
                .Add("ClaimNumber", "Text")
                .Add("ClaimLossDate", "DateTime")
                .Add("ClaimStatus", "Text")
                .Add("ClaimPaymentDate", "DateTime")
                .Add("ClaimPrimaryCause", "Text")
                .Add("ClaimIncurredAmount", "Currency")
                .Add("AgentShortName", "Text")
                .Add("AgentFullName", "Text")
                .Add("InternalOnly", "Boolean")
                .Add("party_cnt", "Number")
                .Add("insurance_file_cnt", "Number")
                .Add("claim_id", "Number")
                .Add("PureUser", "Text")
                .Add("DmeDate", "DateTime")
                .Add("DmeUser", "Text")
            End With
            Return SPAttributesFields
        End Get
    End Property


#End Region

    ''' <summary>
    '''   Initialise (Standard Method)
    '''   Description: Entry point for any initialisation code for this object.
    ''' </summary>
    ''' <param name="sUsername"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="iUserID"></param>
    ''' <param name="iSourceID"></param>
    ''' <param name="iLanguageID"></param>
    ''' <param name="iCurrencyID"></param>
    ''' <param name="iLogLevel"></param>
    ''' <param name="sCallingAppName"></param>
    ''' <param name="bStandAlone"></param>
    ''' <param name="vDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String,
                               ByVal iUserID As Integer,
                               ByVal iSourceID As Integer,
                               ByVal iLanguageID As Integer,
                               ByVal iCurrencyID As Integer,
                               ByVal iLogLevel As Integer,
                               ByVal sCallingAppName As String,
                               Optional ByVal bStandAlone As Boolean = False,
                               Optional ByVal vDatabase As Object = Nothing) As Long
        Try

            m_nReturn = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_nUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_nLanguageID = iLanguageID
            m_nSourceID = iSourceID
            m_nCurrencyID = iCurrencyID
            m_nLogLevel = iLogLevel

            m_nReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_nSourceID, m_nLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Get the Sharepoint Server from the System Options
            m_nReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_nUserID,
                                                v_iMainSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID,
                                                v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionIsSharePointOnline, r_sOptionValue:=IsSharePointOnline) 'Is sharepoint Online 5177

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option " & kSystemOptionIsSharePointOnline & " - Sharepoint Server", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return m_nReturn
            End If
            'Get the Sharepoint Server from the System Options
            m_nReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_nUserID,
                                                v_iMainSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID,
                                                v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionSharePointURl, r_sOptionValue:=SharepointUrl)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option " & kSystemOptionSharePointURl & " - Sharepoint Server", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return m_nReturn
            End If

            'Get the Sharepoint Document Library from the System Options
            m_nReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_nUserID,
                                                v_iMainSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID,
                                                v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionSharePointDocLib, r_sOptionValue:=DocumentLibrary)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option " & kSystemOptionSharePointDocLib & " - Sharepoint Server", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return m_nReturn
            End If

            'Get the Sharepoint User Name from the System Options
            m_nReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_nUserID,
                                                v_iMainSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID,
                                                v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionSharePointUserName, r_sOptionValue:=SharePointUserName) 'Pass Correct Option here

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option " & kSystemOptionSharePointUserName & " - Sharepoint Server", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return m_nReturn
            End If
            'Get the Sharepoint PassWord from the System Options
            m_nReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_nUserID,
                                                v_iMainSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID,
                                                v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionSharePointPassword, r_sOptionValue:=SharePointPassword) 'Pass Correct Option here

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option " & kSystemOptionSharePointPassword & " - Sharepoint Server", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument")
                Return m_nReturn
            End If
            Return m_nReturn
        Catch excep As System.Exception
            m_nReturn = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)
            Return m_nReturn
        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub
    ''' <summary>
    ''' Get File List wrapper class which will be called from SAM 
    ''' It's a capable to create Folder as well.
    ''' </summary>
    ''' <param name="PartyShortname"></param>
    ''' <param name="PolicyNumber"></param>
    ''' <param name="ClaimNumber"></param>
    ''' <param name="destinationUrl"></param>
    ''' <param name="FileList"></param>
    ''' <param name="CreateFolder"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileList(ByVal PartyShortname As String, ByVal PolicyNumber As String,
                               ByVal ClaimNumber As String, ByRef destinationUrl As String,
                               ByRef FileList As DataTable,
                               Optional ByVal CreateFolder As Boolean = False) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            If destinationUrl Is Nothing Then
                destinationUrl = ""
            End If
            If destinationUrl.Length = 0 Then
                'Build the default path
                Dim sDocumentLibrary As String
                sDocumentLibrary = gPMFunctions.GetDocumentLibrary(m_oDatabase, 0, PartyShortname)
                If sDocumentLibrary <> "" Then
                    DocumentLibrary = sDocumentLibrary
                End If
                destinationUrl = BuildDestinationPath(SharepointUrl, DocumentLibrary, PartyShortname, PolicyNumber, ClaimNumber, "")
            End If

            'Sharepoint Online Implementation
            If CreateFolder Then
                Dim root As String() = destinationUrl.Split("/")
                Dim strFolderPath As String = String.Empty
                If (root.Length > 0 AndAlso SharepointUrl.Length > 0) Then
                    If (SharepointUrl.LastIndexOf("/") = SharepointUrl.Length - 1) Then
                        strFolderPath = SharepointUrl & DocumentLibrary
                    Else
                        strFolderPath = SharepointUrl & "/" & DocumentLibrary
                    End If
                End If
                CreateSharePointOnlineFolders(destinationUrl)
                For i As Integer = 0 To root.Length - 1
                    strFolderPath = strFolderPath & "/" & root(i)
                Next
                destinationUrl = strFolderPath
            End If
            'Work To Do-Impelemnt new GetList Method to retreive the files from sharepoin line.                 
            FileList = GetFileList(destinationUrl)

        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFileList", excep:=ex)
            Throw (ex)
            Return nResult
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Check And Validate Party Document Library
    ''' </summary>
    ''' <param name="nPartyCnt"></param>
    ''' <param name="DocumentLibrary"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="spassword"></param>
    ''' <param name="IsDME"></param>
    ''' <param name="sDMEDestinationFilename"></param>
    ''' <returns></returns>
    Public Function CheckAndValidatePartyDocumentLibrary(ByVal nPartyCnt As Long, ByRef DocumentLibrary As String, Optional ByVal sUserName As String = "", Optional ByVal spassword As String = "", Optional ByVal IsDME As Boolean = False, Optional ByVal sDMEDestinationFilename As String = "") As Integer
        Try
            Dim PartyShortName As String = ""
            If (IsDME And sDMEDestinationFilename.Length > 0) Then
                Dim str As String() = sDMEDestinationFilename.Split("/")
                PartyShortName = str(0)
            End If
            Dim sDocumentLibrary As String
            Dim sPartyCode As String
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("PartyCnt", nPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("PartyShortName", PartyShortName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                m_nReturn = .SQLSelect("spu_SIR_Get_Party_Document_Library", "Get_Party_Document_Library", True)

                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the tags for the Sharepoint archive", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return m_nReturn
                End If
            End With

            sDocumentLibrary = Convert.ToString(m_oDatabase.Records.Item(0).Fields("DocumentLibrary"))
            sPartyCode = Convert.ToString(m_oDatabase.Records.Item(0).Fields("shortname"))
            If (sDocumentLibrary.Length > 0 AndAlso obSIRSharePointApi.IsDocumentLibraryNotExists(sDocumentLibrary)) Then
                obSIRSharePointApi.CreateDocumentLibrary(sDocumentLibrary, bLinkContentType:=True)
                If String.IsNullOrEmpty(obSIRSharePointApi.GetDocumentLibrarybyPartyShortname(sPartyCode.Trim())) Then
                    Dim folderNames As String()
                    folderNames = Convert.ToString(sPartyCode.Trim() + "/General").Split("/")
                    obSIRSharePointApi.model.SharePointDocumentLibrary = sDocumentLibrary
                    obSIRSharePointApi.CreateSharePointFolders(folderNames)
                End If
            End If

            If String.IsNullOrEmpty(sDocumentLibrary) Then
                sDocumentLibrary = obSIRSharePointApi.GetDocumentLibrarybyPartyShortname(sPartyCode.Trim())
                If String.IsNullOrEmpty(sDocumentLibrary) Then
                    sDocumentLibrary = CreateAndUpdatePartyDocumentLibrary(DocumentLibrary, sUserName, spassword)
                    If String.IsNullOrEmpty(obSIRSharePointApi.GetDocumentLibrarybyPartyShortname(sPartyCode.Trim())) Then
                        Dim folderNames As String()
                        folderNames = Convert.ToString(sPartyCode.Trim() + "/General").Split("/")
                        obSIRSharePointApi.model.SharePointDocumentLibrary = sDocumentLibrary
                        obSIRSharePointApi.CreateSharePointFolders(folderNames)
                    End If
                End If
                With m_oDatabase
                    .Parameters.Clear()
                    .Parameters.Add("PartyCnt", nPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("DocumentLibrary", sDocumentLibrary, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    m_nReturn = .SQLSelect("spu_SIR_Upd_Party_Document_Library", "Update_Party_Document_Library", True)

                    If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the tags for the Sharepoint archive", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return m_nReturn
                    End If
                End With

            End If
            DocumentLibrary = sDocumentLibrary
            Return m_nReturn
        Catch ex As Exception
            m_nReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message & " " & CheckAndValidatePartyDocumentLibrary, vApp:=ACApp, vClass:=ACClass,
                               vMethod:="CheckAndValidatePartyDocumentLibrary", excep:=ex)
            m_sErrorString = ex.Message
            Throw (ex)
            Return m_nReturn
        End Try
    End Function

    ''' <summary>
    ''' Create And Update Party Document Library
    ''' </summary>
    ''' <param name="partyCnt"></param>
    ''' <param name="DocumentLibrary"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="sPassword"></param>
    ''' <returns></returns>
    Public Function CreateAndUpdatePartyDocumentLibrary(ByVal DocumentLibrary As String, ByVal sUserName As String, ByVal sPassword As String) As String
        Try
            Dim sNewDocumentLibrary As String = ""
            If (obSIRSharePointApi Is Nothing) Then
                InitialiseSPRestApi(SharepointUrl, DocumentLibrary, sUserName, sPassword, True)
            End If
            sNewDocumentLibrary = obSIRSharePointApi.CreatePartyDocumentLibrary()
            Return sNewDocumentLibrary
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' GenerateDefaultPathForSharePointOnline Is used to create the folder in Sharepoint through background job.
    ''' </summary>
    ''' <param name="nPartyCnt"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nClaimID"></param>
    ''' <param name="nCaseID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateDefaultPathForSharePointOnline(ByVal nPartyCnt As Integer, ByVal nInsuranceFileCnt As Integer,
                                     ByVal nClaimID As Integer, ByVal nCaseID As Integer) As Integer
        m_nReturn = gPMConstants.PMEReturnCode.PMTrue
        Try
            Dim sSharepointPath As String = ""
            Dim sArchiveOption As String = ""

            If (obSIRSharePointApi Is Nothing) Then
                InitialiseSPRestApi(SharepointUrl, DocumentLibrary, SharePointUserName, SharePointPassword, True)
            End If
            Dim sDocumentLibrary As String = ""
            If (nPartyCnt > 0) Then

                m_nReturn = CheckAndValidatePartyDocumentLibrary(nPartyCnt, DocumentLibrary, SharePointUserName, SharePointPassword)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5087 - Sharepoint Document Library", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return m_nReturn
                End If

            End If
            If (obSIRSharePointApi IsNot Nothing) Then
                obSIRSharePointApi.model.SharePointDocumentLibrary = DocumentLibrary
            End If

            'Get all the information about the Document for the Content Type Tags
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("document_template_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("template_group_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("template_sub_group_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("party_cnt", nPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("insurance_file_cnt", nInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("claim_id", nClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                m_nReturn = .SQLSelect("spu_SIR_Get_Sharepoint_Tags", "Get Sharepoint Tags", True)

                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the tags for the Sharepoint archive", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDefaultPathForSharePointOnline")
                    Return m_nReturn
                End If

                'Use a dummy filename to just build the path
                sSharepointPath = BuildDestinationPath(SharepointSite:=SharepointUrl, SharepointLibrary:=DocumentLibrary,
                                                      PartyShortname:=m_oDatabase.Records.Item(0).Fields("party_shortname"),
                                                      PolicyNumber:=m_oDatabase.Records.Item(0).Fields("policy_number"),
                                                      ClaimNumber:=m_oDatabase.Records.Item(0).Fields("claim_number"),
                                                      Filename:="x.DOC")

                'If it's case for policy folder then try to rename the quote folder first.
                If Not String.IsNullOrEmpty(m_oDatabase.Records.Item(0).Fields(5)) _
                   AndAlso m_oDatabase.Records.Item(0).Fields(18).ToString.ToUpper <> m_oDatabase.Records.Item(0).Fields(5).ToString.ToUpper _
                   AndAlso m_oDatabase.Records.Item(0).Fields(19) = 2 Then

                    Dim sQuoteRef As String = m_oDatabase.Records.Item(0).Fields(18).ToString.ToUpper.Trim
                    Dim sPolicyNumber As String = m_oDatabase.Records.Item(0).Fields(5).ToString.ToUpper.Trim
                    Dim sPolicyURL As String = Informations.Left(sSharepointPath, sSharepointPath.LastIndexOf("/"))
                    Dim sQuoteURL As String = sPolicyURL.Replace(sPolicyNumber, sQuoteRef)
                    sPolicyURL = sPolicyURL.Replace("%20", " ")
                    sQuoteURL = sQuoteURL.Replace("%20", " ")

                    Dim sQuoteFolderPath As String = sQuoteURL.Replace(SharepointUrl, "")
                    Dim sPolicyFolderPath As String = sPolicyURL.Replace(SharepointUrl, "")

                    If (sQuoteFolderPath.StartsWith("/")) Then sQuoteFolderPath = sQuoteFolderPath.Substring(1, sQuoteFolderPath.Length - 1)
                    If (sPolicyFolderPath.StartsWith("/")) Then sPolicyFolderPath = sPolicyFolderPath.Substring(1, sPolicyFolderPath.Length - 1)
                    If (sQuoteFolderPath.EndsWith("/")) Then sQuoteFolderPath = sQuoteFolderPath.Substring(0, sQuoteFolderPath.Length - 1)
                    If (sPolicyFolderPath.EndsWith("/")) Then sPolicyFolderPath = sPolicyFolderPath.Substring(0, sPolicyFolderPath.Length - 1)

                    obSIRSharePointApi.RenameQuoteFolderToPolicyFolder(sQuoteRef, sPolicyNumber, sQuoteFolderPath, sPolicyFolderPath, False)

                Else 'Generate the Given path
                    m_nReturn = CreateSharePointOnlineFolders(sSharepointPath)
                    If m_nReturn <> PMEReturnCode.PMTrue Then
                        Throw New ApplicationException("GenerateDefaultPathForSharePointOnline -failed.")
                    End If
                    If m_nReturn = PMEReturnCode.PMTrue AndAlso
                    (m_oDatabase.Records.Item(0).Fields("policy_number") = "" And
                    m_oDatabase.Records.Item(0).Fields("claim_number") = "") Then

                        'Create the Email folder under Client folder Only it's a client creation call.
                        sSharepointPath = BuildDestinationPath(SharepointSite:=SharepointUrl, SharepointLibrary:=DocumentLibrary,
                                                              PartyShortname:=m_oDatabase.Records.Item(0).Fields("party_shortname"),
                                                              PolicyNumber:=m_oDatabase.Records.Item(0).Fields("policy_number"),
                                                              ClaimNumber:=m_oDatabase.Records.Item(0).Fields("claim_number"),
                                                              Filename:="x.EML", IsGeneratedMail:=True)
                        m_nReturn = CreateSharePointOnlineFolders(sSharepointPath)
                        If m_nReturn <> PMEReturnCode.PMTrue Then
                            Throw New ApplicationException("GenerateDefaultPathForSharePointOnline -failed.")
                        End If
                    End If
                End If
            End With
        Catch ex As Exception
            m_nReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Please Check Configuration setting for SharepointOnline in System option task" + ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDefaultPathForSharePointOnline", excep:=ex)
            Throw
        End Try
        Return m_nReturn
    End Function
    ''' <summary>
    ''' Return the files for given URls
    ''' </summary>
    ''' <param name="rootUrl"></param>
    ''' <param name="docLib"></param>
    ''' <param name="destinationUrl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileList(ByVal destinationUrl As String) As DataTable

        Try
            If (obSIRSharePointApi Is Nothing) Then
                InitialiseSPRestApi(SharepointUrl, DocumentLibrary, SharePointUserName, SharePointPassword, True)
            End If
            Return obSIRSharePointApi.GetFileList(destinationUrl)

        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFileList", excep:=ex)
        Finally

        End Try
        Return New DataTable()
    End Function
    'This function returns the path in the Document Libary based on the Type of Document
    Private Function BuildDestinationPath(ByRef SharepointSite As String, ByVal SharepointLibrary As String,
                                          ByVal PartyShortname As String, ByVal PolicyNumber As String,
                                          ByVal ClaimNumber As String, ByVal Filename As String, Optional ByVal IsGeneratedMail As Boolean = False, Optional ByVal bIsDMEMigration As Boolean = False) As String

        Dim DestinationFolder As String = SharepointSite.Trim
        If DestinationFolder.EndsWith("/") Then
            DestinationFolder = DestinationFolder.Substring(0, DestinationFolder.Length - 1)
        End If
        If SharepointLibrary.Trim.Length > 0 Then
            DestinationFolder &= "/" & SharepointLibrary.Trim.Replace(" ", "%20") & "/"
        End If
        If Not Filename.ToUpper.Contains("/REPORTS/") AndAlso Not bIsDMEMigration Then
            If (Filename.ToUpper.EndsWith(".EML") Or Filename.ToUpper.EndsWith(".MSG")) And IsGeneratedMail Then
                DestinationFolder &= PartyShortname.Trim & "/"
                DestinationFolder &= "Generated Emails" & "/"
            ElseIf ClaimNumber.Length > 0 Then
                DestinationFolder &= PartyShortname.Trim & "/"
                DestinationFolder &= "Claim" & "/"
                DestinationFolder &= ClaimNumber.Trim & "/"
            ElseIf PolicyNumber.Length > 0 Then
                DestinationFolder &= PartyShortname.Trim & "/"
                DestinationFolder &= "Policy" & "/"
                DestinationFolder &= PolicyNumber.Trim & "/"
            Else
                DestinationFolder &= PartyShortname.Trim & "/"
                DestinationFolder &= "General" & "/"
            End If
        End If
        DestinationFolder &= Filename.Trim()
        DestinationFolder = DestinationFolder.Replace("//", "/")
        DestinationFolder = DestinationFolder.Replace("http:/", "http://")
        DestinationFolder = DestinationFolder.Replace("https:/", "https://")

        Return DestinationFolder
    End Function

    Private Shared Function ReplaceString(ByRef str As String) As String
        Dim illegalChars As Char() = ":~""#%&*:<>?/\{}|".ToCharArray()

        Dim sb As New System.Text.StringBuilder

        For Each ch As Char In str
            If Array.IndexOf(illegalChars, ch) = -1 Then
                sb.Append(ch)
            End If
        Next
        Return sb.ToString()
    End Function
#Region "Sharepoin365 Methods"
    ''' <summary>
    ''' This function is used to upload the files into sharepoin online using CSOM client object
    ''' </summary>
    ''' <param name="sUploadFileName">Pass the source file name</</param>
    ''' <param name="sDestinationFolder">Complete sharepoint url where file need to be upload</param>
    ''' <param name="oProperties">Pass the pure standard propeties.</</param>
    ''' <param name="bCreateFolder">Optional Parameter you pass false to improve the performance.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UploadFile365(ByVal sUploadFileName As String,
                                  ByVal sDestinationFolder As String,
                                  ByVal oProperties As Dictionary(Of String, Object),
                                  Optional ByVal bCreateFolder As Boolean = True) As Integer
        Dim oweb As Web
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oSharepointlist As List
        Dim oUploadFile As Microsoft.SharePoint.Client.File
        Dim oListItem As Microsoft.SharePoint.Client.ListItem
        Dim sFileName As String = String.Empty
        Dim oClientContext As New ClientContext(SharepointUrl)
        Dim ObjNewFile As New FileCreationInformation()
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Ssl3
        oClientContext.Credentials = SetSystemoptionCredentials()
        oweb = oClientContext.Web
        ObjNewFile.Content = System.IO.File.ReadAllBytes(sUploadFileName)
        ObjNewFile.Overwrite = True

        If Trim(sDestinationFolder) <> "" Then
            ObjNewFile.Url = sDestinationFolder
        End If
        Try
            oSharepointlist = oweb.Lists.GetByTitle(DocumentLibrary)
            sFileName = IO.Path.GetFileName(sDestinationFolder)
            oUploadFile = oSharepointlist.RootFolder.Files.Add(ObjNewFile)
            oListItem = oUploadFile.ListItemAllFields

            'We assume that all the pure standard column must be created in sharepoint server at the time of configuration.
            For Each [property] As KeyValuePair(Of String, Object) In oProperties
                If [property].Value IsNot Nothing Then
                    oListItem.Item([property].Key) = [property].Value

                End If
            Next
            oListItem.Update()
            oClientContext.ExecuteQuery()

        Catch ex As Exception
            'There is possiblity to upload the file if somehow folder is not exist in sharepoin so handled that case too.
            Select Case DirectCast(ex, Microsoft.SharePoint.Client.ServerException).ServerErrorCode
                Case SharepoinErrorCodes.FileNotFound
                    If bCreateFolder Then
                        CreateSharePointOnlineFolders(sDestinationFolder)
                        oSharepointlist = oweb.Lists.GetByTitle(DocumentLibrary)
                        sFileName = IO.Path.GetFileName(sDestinationFolder)
                        oUploadFile = oSharepointlist.RootFolder.Files.Add(ObjNewFile)
                        oListItem = oUploadFile.ListItemAllFields
                        'We assume that all the pure standard column must be created in sharepoint server at the time of configuration.
                        For Each [property] As KeyValuePair(Of String, Object) In oProperties
                            If [property].Value IsNot Nothing Then
                                oListItem.Item([property].Key) = [property].Value
                            End If
                        Next
                        oListItem.Update()
                        oClientContext.ExecuteQuery()
                    End If

                Case Else
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to upload file in sharepoint online", vApp:=ACApp, vClass:=ACClass, vMethod:="UploadFile365")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
            End Select

        Finally
            oClientContext.Dispose()
            ObjNewFile = Nothing
            oUploadFile = Nothing
            'Delete file from physical location
            'If nResult = gPMConstants.PMEReturnCode.PMTrue Then
            '    If IsFileExists(sUploadFileName) Then
            '        Dim oFileinfo As FileInfo = New FileInfo(sUploadFileName)
            '        oFileinfo.Delete()
            '        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            '            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete file [" & sUploadFileName & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UploadFile365")
            '        End If

            '    End If
            'End If
        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' This function is used to create the folder if it's not available in sharepoint list.
    ''' </summary>
    ''' <param name="sCompleteFileUrl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateSharePointOnlineFolders(ByVal sCompleteFileUrl As String) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oFolderCollection As String()
        'Remove the file name as it not a folder name
        sCompleteFileUrl = sCompleteFileUrl.Substring(0, sCompleteFileUrl.LastIndexOf("/"))
        Try

            If (SharepointUrl.LastIndexOf("/") = SharepointUrl.Length - 1) Then
                sCompleteFileUrl = sCompleteFileUrl.Replace(SharepointUrl + DocumentLibrary + "/", "")
            Else
                sCompleteFileUrl = sCompleteFileUrl.Replace(SharepointUrl + "/" + DocumentLibrary + "/", "")
            End If
            oFolderCollection = sCompleteFileUrl.Split("/")
            If (obSIRSharePointApi Is Nothing) Then
                InitialiseSPRestApi(SharepointUrl, DocumentLibrary, SharePointUserName, SharePointPassword, True)
            End If
            obSIRSharePointApi.CreateSharePointFolders(oFolderCollection)

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="Create SharePoint OnlineFolders", excep:=ex)
            Throw (ex)
            Return nResult
        Finally

        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' This function is used to get the credetial configuration from system option .
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSystemoptionCredentials() As SharePointOnlineCredentials
        Try
            Dim sDecryptkey As String = String.Empty
            If IsSharePointOnline Then
                sDecryptkey = bPMFunc.DecryptPassword(SharePointPassword, PMEncryptionEntropy)
            End If
            Return New SharePointOnlineCredentials(SharePointUserName, SecureStore(sDecryptkey))
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary>
    ''' This Function is used to delete the perticular list from the sharepoint.
    ''' </summary>
    ''' <param name="sFileName">Pass the exact file name along with ext.</param>
    ''' <param name="sUrl">Pass the complete url of file</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteSharepointFiles(ByVal sFileName As String, ByVal sUrl As String)
        Dim oRegEx As New Regex(sFileName, RegexOptions.IgnoreCase)
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oSharepointlist As List
        Dim oweb As Web
        Dim uri As New Uri(sUrl.Substring(0, sUrl.LastIndexOf("/")))
        Dim sServerRelativeURL As String = uri.AbsolutePath
        Dim oFolder As Folder
        Try
            Using oCtx As New ClientContext(SharepointUrl)
                oweb = oCtx.Web
                oCtx.Credentials = SetSystemoptionCredentials()
                oSharepointlist = oweb.Lists.GetByTitle(DocumentLibrary)
                oFolder = oweb.GetFolderByServerRelativeUrl(sServerRelativeURL)
                oCtx.Load(oFolder.Files)
                oCtx.ExecuteQuery()

                For Each file As Microsoft.SharePoint.Client.File In oFolder.Files
                    oRegEx.IsMatch(file.Name)
                    file.DeleteObject()
                    oCtx.ExecuteQuery()
                    Exit For
                Next

            End Using
            Return nResult
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteSharepointFiles", excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' This function is converting into Secure password
    ''' </summary>
    ''' <param name="sPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SecureStore(ByVal sPassword As String) As SecureString
        Dim objSecureString As New SecureString
        For Each c In sPassword.ToCharArray()
            objSecureString.AppendChar(c)
        Next
        Return objSecureString
    End Function
    ''' <summary>
    ''' Rename Quote Folder To Policy Folder
    ''' </summary>
    ''' <param name="sQuoteFolderRelativeURL"></param>
    ''' <param name="sPolicyFolderRelativeURL"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RenameQuoteFolderToPolicyFolder(ByVal sQuoteFolderRelativeURL As String, ByVal sPolicyFolderRelativeURL As String, ByRef obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls)
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            If (obSIRSharePointApi Is Nothing) Then
                InitialiseSPRestApi(SharepointUrl, DocumentLibrary, SharePointUserName, SharePointPassword, True)
            End If

            nResult = obSIRSharePointApi.RenameFolder(sQuoteFolderRelativeURL, sPolicyFolderRelativeURL)
            If (nResult <> PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error returned from Sharepoint rest api", vApp:=ACApp, vClass:=ACClass, vMethod:="RenameQuoteFolderToPolicyFolder")
            End If
            '//_api/web/GetFolderByServerRelativeUrl('{0}')/moveTo(newurl='{1}'
        Catch ex As Exception
            Select Case DirectCast(ex, Microsoft.SharePoint.Client.ServerException).ServerErrorCode
                Case SharepoinErrorCodes.FolderNotFound
                    'No need to throw an error.
                Case Else
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="RenameQuoteFolderToPolicyFolder", excep:=ex)
            End Select

            Return nResult
        End Try

        Return nResult
    End Function
    ''' <summary>
    ''' Upload File
    ''' </summary>
    ''' <param name="destinationUrl"></param>
    ''' <param name="bytes"></param>
    ''' <param name="properties"></param>
    ''' <param name="docLib"></param>
    ''' <param name="sSourceFile"></param>
    ''' <param name="obSIRSharePointApi"></param>
    ''' <param name="IsDME"></param>
    ''' <returns></returns>
    Public Function Upload(ByVal destinationUrl As String, ByVal bytes As Byte(), ByVal properties As Dictionary(Of String, Object), ByVal docLib As String,
                           Optional ByVal sSourceFile As String = "", Optional ByRef obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls = Nothing, Optional ByVal IsDME As Boolean = False) As Boolean
        Return Upload(New FileInfo(destinationUrl, bytes, properties), docLib, sSourceFile, obSIRSharePointApi, IsDME:=IsDME)
    End Function
    ''' <summary>
    ''' Upload File
    ''' </summary>
    ''' <param name="fileInfo"></param>
    ''' <param name="docLib"></param>
    ''' <param name="sSourceFile"></param>
    ''' <param name="obSIRSharePointApi"></param>
    ''' <param name="IsDME"></param>
    ''' <returns></returns>
    Public Function Upload(ByVal fileInfo As FileInfo, ByVal docLib As String, Optional ByVal sSourceFile As String = "", Optional ByRef obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls = Nothing, Optional ByVal IsDME As Boolean = False) As Boolean
        Try

            Dim bResult As Boolean = False
            Dim bFolderCreated As Boolean = False

            If fileInfo.m_ensureFolders Then

                Dim uri As String = fileInfo.URI.AbsoluteUri
                Dim sFolderNames As String = ""
                Dim lstDocFolder As String()
                Dim libPath As String = String.Empty
                Dim sSharePointUrl As String = obSIRSharePointApi.model.SharePointSiteURL
                obSIRSharePointApi.model.SharePointDocumentLibrary = docLib
                If sSharePointUrl.EndsWith("/") Then
                    If uri.Contains(" ") OrElse uri.Contains("%20") Then
                        libPath = uri.Replace(sSharePointUrl, "")
                        lstDocFolder = libPath.Split("/")
                        sFolderNames = uri.Replace(sSharePointUrl + lstDocFolder(1) + "/", "")
                    Else
                        sFolderNames = uri.Replace(sSharePointUrl + docLib + "/", "")
                    End If
                ElseIf sSharePointUrl.Length > 0 Then
                    If uri.Contains(" ") OrElse uri.Contains("%20") Then
                        libPath = uri.Replace(sSharePointUrl, "")
                        lstDocFolder = libPath.Split("/")
                        sFolderNames = uri.Replace(sSharePointUrl + "/" + lstDocFolder(1) + "/", "")
                    Else
                        sFolderNames = uri.Replace(sSharePointUrl + "/" + docLib + "/", "")
                    End If
                End If

                Dim fileName As String = System.IO.Path.GetFileName(uri)
                sFolderNames = sFolderNames.Replace(fileName, "")
                Dim lstFolderNames As String()
                If sFolderNames.Length > 0 Then
                    If Not obSIRSharePointApi.IsFolderExists(docLib + "/" + sFolderNames) Then
                        If sFolderNames.EndsWith("/") Then
                            sFolderNames = sFolderNames.Substring(0, sFolderNames.Length - 1)
                        End If
                        lstFolderNames = sFolderNames.Split("/")

                        bFolderCreated = obSIRSharePointApi.CreateSharePointFolders(lstFolderNames)
                    Else
                        bFolderCreated = True
                    End If
                End If

                If bFolderCreated Then
                    bResult = TryToUpload(fileInfo, docLib, sSourceFile, obSIRSharePointApi)
                End If
            End If

            Return bResult
        Catch ex As Exception
            Throw (ex)
        End Try
    End Function

    ''' <summary>
    ''' Try To Upload files
    ''' </summary>
    ''' <param name="fileInfo"></param>
    ''' <param name="docLib"></param>
    ''' <param name="sSourceFile"></param>
    ''' <param name="obSIRSharePointApi"></param>
    ''' <returns></returns>
    Private Function TryToUpload(ByVal fileInfo As FileInfo, ByVal docLib As String, Optional ByVal sSourceFile As String = "", Optional ByRef obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls = Nothing) As Boolean

        Try
            obSIRSharePointApi.UploadFile365(obSIRSharePointApi.model.SharePointSiteURL, fileInfo.m_URL, sSourceFile, docLib)

            If fileInfo.HasProperties Then
                Dim iDocID As Integer = obSIRSharePointApi.GetFileId(obSIRSharePointApi.model.SharePointSiteURL, docLib, fileInfo.m_URL)
                obSIRSharePointApi.UpdateListItems(fileInfo.m_properties, iDocID, fileInfo.m_URL)
            End If

            Return True

        Catch generatedExceptionName As WebException
            'We must not throw the exception here - as we may be attempting to create a file in a 
            'folder that doesn't exist yet and need to trap it.
            If generatedExceptionName.Response IsNot Nothing AndAlso Not DirectCast(generatedExceptionName.Response, System.Net.HttpWebResponse).StatusCode = HttpStatusCode.Conflict Then
                Throw New Exception("TryToUpload - " & generatedExceptionName.Message)
            End If
            Return False
        Catch ex As Exception
            m_sErrorString = ex.Message
            Return False
        End Try

    End Function
#End Region
#Region "Share Point Configuration Methods"
    Public obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls
    Public SPModel As bSIRSharepointApi.Models.SPContextConfiguration
    ''' <summary>
    ''' Initialise Sharepoint Rest api component
    ''' </summary>
    ''' <param name="sSharepointSite"></param>
    ''' <param name="sSharepointLibrary"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="IsSharepointOnline"></param>
    ''' <param name="bCalledViaSystemOption"></param>
    ''' <returns></returns>
    Private Function InitialiseSPRestApi(ByVal sSharepointSite As String,
                                                ByVal sSharepointLibrary As String,
                                                ByVal sUserName As String, ByVal sPassword As String,
                                                Optional ByVal IsSharepointOnline As Boolean = False,
                                                Optional ByVal bCalledViaSystemOption As Boolean = False,
                                                Optional ByVal sAppClientId As String = "",
                                                Optional ByVal sSharepointTenantId As String = "") As bSIRSharepointApi.bSIRSharepointApiCls
        Dim oUri As New Uri(sSharepointSite)
        If obSIRSharePointApi Is Nothing Then
            SPModel = New bSIRSharepointApi.Models.SPContextConfiguration()
            SPModel.IsSharePointOnline = IsSharepointOnline
            SPModel.SharePointSiteURL = sSharepointSite.TrimEnd("/")
            SPModel.SharePointDocumentLibrary = sSharepointLibrary
            SPModel.SharePointUserName = sUserName
            If (bCalledViaSystemOption) Then
                SPModel.SharePointPassword = sPassword
            Else
                SPModel.SharePointPassword = bPMFunc.DecryptPassword(sPassword, PMEncryptionEntropy)
                m_nReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_nUserID,
                                               v_iMainSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID,
                                               v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel,
                                               v_sCallingAppName:=ACApp,
                                               v_iOptionNumber:=kSystemOptionSharepointOnlineClientId, r_sOptionValue:=sAppClientId) 'Is sharepoint Online 5177
                m_nReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_nUserID,
                                                v_iMainSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID,
                                                v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=KSystemOptionSharepointOnlineTenantId, r_sOptionValue:=sSharepointTenantId) 'Is sharepoint Online 5177
            End If
            SPModel.AppClientId = sAppClientId
            SPModel.SharepointTenantId = sSharepointTenantId
                obSIRSharePointApi = New bSIRSharepointApi.bSIRSharepointApiCls(SPModel)
                obSIRSharePointApi.Initialise()
            End If

            Return obSIRSharePointApi

    End Function

    ''' <summary>
    ''' This will be invoked from the system option when Sharepoin online will be configured
    ''' </summary>
    ''' <param name="sSharepointSite"></param>
    ''' <param name="sSharepointLibrary"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="sResponse"></param>
    ''' <remarks></remarks>
    Public Sub ValidateSharepointOnlineURL(ByVal sSharepointSite As String,
                                                ByVal sSharepointLibrary As String,
                                                ByVal sUserName As String, sPassword As String,
                                                ByRef sResponse As String, ByVal sAppClientId As String, ByVal sSharepointTenantId As String)

        Dim nResult As Integer = PMEReturnCode.PMTrue

        If Trim(sSharepointLibrary) <> "" Then
            DocumentLibrary = sSharepointLibrary
        End If
        If Trim(sSharepointSite) <> "" Then
            SharepointUrl = sSharepointSite
        End If
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Ssl3
        Try
            Dim DestinationFolder As String = sSharepointSite.Trim
            If DestinationFolder.EndsWith("/") Then
                DestinationFolder = DestinationFolder.Substring(0, DestinationFolder.Length - 1)
            End If
            If sSharepointLibrary.Trim.Length > 0 Then
                DestinationFolder &= "/" & sSharepointLibrary.Trim.Replace(" ", "%20") & "/"
            End If
            DestinationFolder = DestinationFolder.Replace("//", "/")
            DestinationFolder = DestinationFolder.Replace("http:/", "http://")
            DestinationFolder = DestinationFolder.Replace("https:/", "https://")
            If (obSIRSharePointApi Is Nothing) Then
                InitialiseSPRestApi(SharepointUrl, DocumentLibrary, sUserName, sPassword, True, True, sAppClientId, sSharepointTenantId)
            End If
            nResult = obSIRSharePointApi.GetListByTitle()
            nResult = obSIRSharePointApi.GetSitesGroupByName(sSharepointSite, sSharepointLibrary)
            nResult = obSIRSharePointApi.GetListContentTypes(sSharepointSite, sSharepointLibrary)

        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateSharepointOnlineURL", excep:=ex)
            Throw ex
        End Try

    End Sub
    ''' <summary>
    ''' This method is used to create the custom group for Pure documents.
    ''' </summary>
    ''' <param name="oClientContext"></param>
    ''' <param name="sGroupName"></param>
    ''' <param name="sGroupDescription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateSharepointGroup(ByRef oClientContext As ClientContext, ByVal sGroupName As String, ByVal sGroupDescription As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Ssl3
        Dim oweb As Web = oClientContext.Web
        Dim oGroupColl As Microsoft.SharePoint.Client.GroupCollection = oweb.SiteGroups
        Dim oCreationInfo As New GroupCreationInformation()
        oCreationInfo.Title = sGroupName
        oCreationInfo.Description = sGroupDescription
        Dim newGroup As Microsoft.SharePoint.Client.Group = oGroupColl.Add(oCreationInfo)
        Try
            oClientContext.Load(newGroup)
            oClientContext.ExecuteQuery()

        Catch ex As ServerException
            Select Case DirectCast(ex, Microsoft.SharePoint.Client.ServerException).ServerErrorCode
                Case SharepoinErrorCodes.ObjectAlreadyExist
                    nResult = PMEReturnCode.PMTrue
                Case Else
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateSharepointGroup", excep:=ex)
                    nResult = PMEReturnCode.PMFalse
            End Select
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateSharepointGroup", excep:=ex)
            nResult = PMEReturnCode.PMFalse
        Finally
            oweb = Nothing
            oCreationInfo = Nothing
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' This function is used to create the Content type for first time if not exist
    ''' </summary>
    ''' <param name="ContentTypeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateContentType(ByRef octx As ClientContext, ByVal ContentTypeName As String, Optional ByVal sSharepointLibrary As String = "") As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim oweb As Web
        Dim bIsExist As Boolean = False
        Dim oSharepointlist As List
        oweb = octx.Site.RootWeb
        If (sSharepointLibrary <> "") Then
            DocumentLibrary = sSharepointLibrary
        End If

        Dim oContentTypes As ContentTypeCollection = oweb.Lists.GetByTitle(DocumentLibrary).ContentTypes

        Try
            oSharepointlist = oweb.Lists.GetByTitle(DocumentLibrary)
            octx.Load(oSharepointlist, Function(s) s.ContentTypesEnabled)
            octx.ExecuteQuery()
            If oSharepointlist.ContentTypesEnabled = False Then
                oSharepointlist.ContentTypesEnabled = True
                oSharepointlist.Update()
                octx.ExecuteQuery()
            End If
            octx.Load(oContentTypes)
            octx.ExecuteQuery()
            For Each ContentType As ContentType In oContentTypes
                If ContentType.Name.ToUpper = ContentTypeName.ToUpper Then
                    bIsExist = True
                    Exit For
                End If
            Next
            If Not bIsExist Then
                Dim oChildContentType As New ContentTypeCreationInformation
                Dim oContentType As ContentType
                oChildContentType.Description = ContentTypeName
                oChildContentType.Name = ContentTypeName
                oChildContentType.Group = kPureGroupName

                'Adding to the pureDocument library as well
                oSharepointlist = oweb.Lists.GetByTitle(DocumentLibrary)
                oContentType = oSharepointlist.ContentTypes.Add(oChildContentType)
                oChildContentType.ParentContentType = oSharepointlist.ContentTypes(0)
                octx.Load(oSharepointlist)
                octx.ExecuteQuery()

                If oContentType.Name IsNot Nothing Then
                    nReturn = SetDefaultContentType(oSharepointlist, oContentType.Name, octx)
                    If nReturn <> PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Set Default Content Type", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateContentType")
                    End If
                End If
                'Create pure feilds and apply into contenttype
                nReturn = CreatePureDocumentFeilds(oSharepointlist, octx)
                If nReturn <> PMEReturnCode.PMFalse Then
                    nReturn = CreateFeildLinkToContentType(oweb, oContentType, oSharepointlist, octx)
                    octx.Load(oweb)
                    octx.ExecuteQuery()
                End If
            End If

        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateContentType", excep:=ex)
            nReturn = PMEReturnCode.PMFalse
        Finally
            octx.Dispose()
            oContentTypes = Nothing
            oweb = Nothing
        End Try
        Return nReturn
    End Function
    ''' <summary>
    ''' Set Default Content Type
    ''' </summary>
    ''' <param name="list"></param>
    ''' <param name="ContenTypeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetDefaultContentType(ByRef list As List, ByVal ContenTypeName As String, ByRef oClientContext As ClientContext) As Integer
        Dim cTypes As IList(Of ContentTypeId) = New List(Of ContentTypeId)()
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Ssl3
        Try
            Dim oContentTypes As ContentTypeCollection = oClientContext.Web.Lists.GetByTitle(DocumentLibrary).ContentTypes
            oClientContext.Load(oContentTypes)
            oClientContext.ExecuteQuery()
            For Each ContentType As ContentType In oContentTypes
                If ContentType.Name.ToUpper = ContenTypeName.ToUpper Then
                    cTypes.Add(ContentType.Id)
                    Exit For
                End If
            Next
            list.RootFolder.UniqueContentTypeOrder = cTypes
            oClientContext.ExecuteQuery()
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="SetDefaultContentType", excep:=ex)
            nReturn = PMEReturnCode.PMFalse
        Finally
            cTypes = Nothing
        End Try

        Return nReturn
    End Function

    ''' <summary>
    ''' this function is used to create the default site column as per pure standards
    ''' </summary>
    ''' <param name="oSharepointlist"></param>
    ''' <param name="oWeb"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreatePureDocumentFeilds(ByRef oSharepointlist As List,
                                              ByRef oclientcontext As ClientContext) As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim sfieldSchema As String
        Dim sFeildName As String = String.Empty
        Dim sViewXml As New StringBuilder
        Dim oCamlQuery As New CamlQuery
        Dim oweb As Web
        Dim oNewfeild As Field
        Dim OfeildCollection As FieldCollection
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Ssl3
        oweb = oclientcontext.Web

        oSharepointlist = oweb.Lists.GetByTitle(DocumentLibrary)
        oclientcontext.Load(oSharepointlist)
        oclientcontext.ExecuteQuery()

        OfeildCollection = oSharepointlist.Fields
        oclientcontext.Load(OfeildCollection)
        oclientcontext.ExecuteQuery()

        Try
            If SPAttributes IsNot Nothing Then
                'We assume that all the pure standard column must be created in sharepoint server at the time of configuration.
                For Each [property] As KeyValuePair(Of String, Object) In SPAttributes
                    If [property].Value IsNot Nothing Then
                        Try
                            sFeildName = [property].Key
                            If sFeildName <> "Title" Then
                                Select Case [property].Value
                                    Case "Text"
                                        sfieldSchema = "<Field Type='Text' DisplayName='" & sFeildName & "' Name= '" & sFeildName & "' />"
                                    Case "Boolean"
                                        sfieldSchema = "<Field Type='Boolean' DisplayName='" & sFeildName & "' Name= '" & sFeildName & "' />"
                                    Case "DateTime"
                                        sfieldSchema = "<Field Type='DateTime' DisplayName='" & sFeildName & "' Name= '" & sFeildName & "' />"
                                    Case "Currency"
                                        sfieldSchema = "<Field Type='Currency' DisplayName='" & sFeildName & "' Name= '" & sFeildName & "' />"
                                    Case "Number"
                                        sfieldSchema = "<Field Type='Number' DisplayName='" & sFeildName & "' Name= '" & sFeildName & "' />"
                                    Case Else
                                        sfieldSchema = "<Field Type='Text' DisplayName='" & sFeildName & "' Name= '" & sFeildName & "' />"
                                End Select
                                oNewfeild = OfeildCollection.AddFieldAsXml(sfieldSchema, False, AddFieldOptions.AddToDefaultContentType)
                                oclientcontext.Load(oNewfeild)
                                oclientcontext.ExecuteQuery()
                            End If
                        Catch ex As Exception
                            'Loop throgh all properties
                            'No need to raise an error if it's already exist
                        End Try
                    End If
                Next
            End If
        Catch ex As Exception
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePureDocumentFeilds", excep:=ex)
        End Try
        Return nReturn
    End Function
    ''' <summary>
    ''' Function is used to link the field with content type
    ''' </summary>
    ''' <param name="oWeb"></param>
    ''' <param name="oContentType"></param>
    ''' <param name="oProperties"></param>
    ''' <returns></returns>
    Private Function CreateFeildLinkToContentType(ByRef oWeb As Web, ByRef oContentType As ContentType,
                                                  ByRef oSharepointlist As List,
                                                  ByRef oClientContext As ClientContext) As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim ofeild As Field
        Dim oFeildlink As New FieldLinkCreationInformation
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Ssl3
        oSharepointlist = oWeb.Lists.GetByTitle(DocumentLibrary)
        oClientContext.Load(oSharepointlist)
        oClientContext.ExecuteQuery()

        Try
            If SPAttributes IsNot Nothing Then
                'We assume that all the pure standard column must be created in sharepoint server at the time of configuration.
                For Each [property] As KeyValuePair(Of String, Object) In SPAttributes
                    If [property].Value IsNot Nothing Then
                        If [property].Key IsNot Nothing Then
                            Try
                                ofeild = oSharepointlist.Fields.GetByInternalNameOrTitle([property].Key)
                                oClientContext.Load(ofeild)
                                oClientContext.ExecuteQuery()

                                If ofeild IsNot Nothing Then
                                    oFeildlink.Field = ofeild
                                    oContentType.FieldLinks.Add(oFeildlink)
                                    oSharepointlist.Update()
                                    oClientContext.ExecuteQuery()
                                End If
                            Catch ex As Exception
                                'No Exception required here just want to iterate all the pure feilds
                            End Try
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFeildLinkToContentType", excep:=ex)
            Return nReturn
        End Try
        Return nReturn
    End Function
#End Region
    Public Enum SharepoinErrorCodes
        FileNotFound = -2147024893
        FolderNotFound = -2147024894
        UserAuthorisedError = -2147186446
        IncorrectUrl = -2146233079
        ObjectAlreadyExist = -2130575293
    End Enum
End Class
Public NotInheritable Class FileInfo
    Public m_URL As String
    Public m_bytes As Byte()
    Public m_properties As Dictionary(Of String, Object)
    Public m_ensureFolders As Boolean = True
    Private m_uri As Uri

    Public ReadOnly Property HasProperties() As Boolean
        Get
            Return m_properties IsNot Nothing AndAlso m_properties.Count > 0
        End Get
    End Property

    Public ReadOnly Property URI() As Uri
        Get
            If m_uri Is Nothing Then
                m_uri = New Uri(m_URL)
            End If

            Return m_uri
        End Get
    End Property

    Public Sub New(ByVal url As String, ByVal bytes As Byte(), ByVal properties As Dictionary(Of String, Object))
        m_URL = url.Replace("%20", " ")
        m_bytes = bytes
        m_properties = properties
    End Sub

End Class