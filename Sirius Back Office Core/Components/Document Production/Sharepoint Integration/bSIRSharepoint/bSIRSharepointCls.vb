Imports System.Data
Imports System.IO
Imports System.Reflection
Imports System.Xml
Imports SSP.Shared

Public NotInheritable Class Business
    Implements IDisposable

    Private Const ACClass As String = "Business"

    Private m_oDatabase As dPMDAO.Database

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_bCloseDatabase As Boolean
    Private m_lReturn As Integer
    Private m_sErrorString As String = ""
    Private m_bIssharepointOnline As Boolean = False
    Private m_SharepointURlPath As String
    ''' <summary>
    ''' Used below variable to create Object for Sharepoin online
    ''' </summary>
    ''' <remarks></remarks>
    Private oType As System.Type
    Private oAssembly As System.Reflection.Assembly
    Public m_oSharepoinOnlineBusiness As System.Object
    Private strPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
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
    ''' Hold The sharepoin url.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SharepointURl() As String
        Get
            Return m_SharepointURlPath
        End Get

        Set(value As String)
            m_SharepointURlPath = value
        End Set
    End Property

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
    ''' <summary>
    ''' Object bSIRSharepointApi class
    ''' </summary>
    Public obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls
    ''' <summary>
    ''' Object of sharepoint model
    ''' </summary>
    Public SPModel As bSIRSharepointApi.Models.SPContextConfiguration
    ''' <summary>
    ''' Entry point for any initialisation code for this
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
                               ByVal iUserID As Integer, ByVal iSourceID As Integer,
                               ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer,
                               ByVal iLogLevel As Integer, ByVal sCallingAppName As String,
                               Optional ByVal bStandAlone As Boolean = False,
                               Optional ByVal vDatabase As Object = Nothing) As Long
        Try

            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Sharepoint Server from the System Options
            Dim sTempOptionValue As String = ""
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionIsSharePointOnline, r_sOptionValue:=sTempOptionValue)
            ''Set The Sharepoint Configuration
            If sTempOptionValue Is Nothing OrElse sTempOptionValue = "" Then
                IsSharePointOnline = False
            Else
                IsSharePointOnline = ToSafeBoolean(sTempOptionValue)
            End If

            If IsSharePointOnline = True Then
                m_oSharepoinOnlineBusiness = CreateLateBoundObject("bSIRSharePointOnline.BusinessSharepointOnline")
                If m_oSharepoinOnlineBusiness IsNot Nothing Then
                    m_lReturn = m_oSharepoinOnlineBusiness.Initialise(sUsername:=ToSafeString(m_sUsername),
                                               sPassword:=ToSafeString(m_sPassword),
                                               iUserID:=ToSafeInteger(m_iUserID),
                                               iSourceID:=ToSafeInteger(m_iSourceID),
                                               iLanguageID:=ToSafeInteger(m_iLanguageID),
                                               iCurrencyID:=ToSafeInteger(m_iCurrencyID),
                                               iLogLevel:=ToSafeInteger(m_iLogLevel),
                                               sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=CType(vDatabase, dPMDAO.Database))
                End If
            End If

            Return m_lReturn

        Catch excep As System.Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return m_lReturn
        End Try
    End Function

    ''' <summary>
    ''' This method will initalise the api component of Sharepoint
    ''' </summary>
    ''' <param name="sSharepointSite"></param>
    ''' <param name="sSharepointLibrary"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="IsSharepointOnline"></param>
    ''' <returns></returns>
    Private Function InitialiseSPRestApi(ByVal sSharepointSite As String,
                                                ByVal sSharepointLibrary As String,
                                                ByVal sUserName As String, sPassword As String, Optional ByVal IsSharepointOnline As Boolean = False) As bSIRSharepointApi.bSIRSharepointApiCls
        Try
            Dim oUri As New Uri(sSharepointSite)
            If obSIRSharePointApi Is Nothing Then
                SPModel = New bSIRSharepointApi.Models.SPContextConfiguration()
                SPModel.IsSharePointOnline = IsSharepointOnline
                SPModel.SharePointSiteURL = sSharepointSite.TrimEnd("/")
                SPModel.SharePointDocumentLibrary = sSharepointLibrary
                SPModel.SharePointUserName = sUserName
                SPModel.SharePointPassword = bPMFunc.DecryptPassword(sPassword, PMEncryptionEntropy)
                If IsSharepointOnline Then
                    Dim sClientId As String = ""
                    Dim sTenantId As String = ""
                    'Get the Sharepoint Document Library from the System Options
                    m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionSharepointOnlineClientId, r_sOptionValue:=sClientId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5258 - Sharepoint Online Client Id", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Throw New Exception("Failed to get the system option 5258 - Sharepoint Online Client Id")
                    End If
                    SPModel.AppClientId = sClientId
                    'Get the Sharepoint Document Library from the System Options
                    m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=KSystemOptionSharepointOnlineTenantId, r_sOptionValue:=sTenantId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5259 - Sharepoint Online Teanant Id", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Throw New Exception("Failed to get the system option 5259 - Sharepoint Online Teanant Id")
                    End If
                    SPModel.SharepointTenantId = sTenantId
                End If
                obSIRSharePointApi = New bSIRSharepointApi.bSIRSharepointApiCls(SPModel)
                obSIRSharePointApi.Initialise()
            End If

            Return obSIRSharePointApi
        Catch excep As Exception
            Throw excep
        End Try
    End Function
    ''' <summary>
    ''' Create Late bound Object 
    ''' </summary>
    ''' <param name="ClassName"></param>
    ''' <returns></returns>
    ''' <remarks>We are not using because it logs an error which not required because we need to load only when componet called from 4.0</remarks>
    Private Function CreateLateBoundObject(ByVal ClassName As String) As Object
        Dim sPurePath As String

        Try
            sPurePath = New Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath
            sPurePath = Path.GetDirectoryName(sPurePath)

            Dim libraryPath As String
            Dim DLLAssembly As [Assembly]

            libraryPath = Path.Combine(sPurePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".dll")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName, True)
            End If

            libraryPath = Path.Combine(sPurePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".exe")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName, True)
            End If
        Catch excep As Exception
            'No need to log an error.
        End Try
        Return Nothing
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
    ''' Check And Validate Party Document Library
    ''' </summary>
    ''' <param name="nPartyCnt"></param>
    ''' <param name="DocumentLibrary"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="spassword"></param>
    ''' <param name="IsDME"></param>
    ''' <param name="sDMEDestinationFilename"></param>
    ''' <returns></returns>
    Public Function CheckAndValidatePartyDocumentLibrary(ByVal nPartyCnt As Long, ByRef DocumentLibrary As String, Optional ByVal sUserName As String = "", Optional ByVal sPassword As String = "", Optional ByVal IsDME As Boolean = False, Optional ByVal sDMEDestinationFilename As String = "") As Integer
        Try
            Dim sPartyShortName As String = ""
            If (IsDME And sDMEDestinationFilename.Length > 0) Then
                Dim str As String() = sDMEDestinationFilename.Split("/")
                sPartyShortName = str(0)
            End If
            Dim sDocumentLibrary As String
            Dim sPartyCode As String
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("PartyCnt", nPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("PartyShortName", sPartyShortName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                m_lReturn = .SQLSelect("spu_SIR_Get_Party_Document_Library", "Get_Party_Document_Library", True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the tags for the Sharepoint archive", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return m_lReturn
                End If
            End With

            sDocumentLibrary = Convert.ToString(m_oDatabase.Records.Item(0).Fields("DocumentLibrary"))
            sPartyCode = Convert.ToString(m_oDatabase.Records.Item(0).Fields("shortname"))
            nPartyCnt = Convert.ToInt32(m_oDatabase.Records.Item(0).Fields("party_cnt"))
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
                    sDocumentLibrary = CreateAndUpdatePartyDocumentLibrary(DocumentLibrary, sUserName, sPassword)
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
                    m_lReturn = .SQLSelect("spu_SIR_Upd_Party_Document_Library", "Update_Party_Document_Library", True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed To Check And Validate Party DocumentLibrary", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAndValidatePartyDocumentLibrary", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return m_lReturn
                    End If
                End With

            End If
            DocumentLibrary = sDocumentLibrary
            Return m_lReturn
        Catch ex As Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message & " " & CheckAndValidatePartyDocumentLibrary, vApp:=ACApp, vClass:=ACClass,
                               vMethod:="CheckAndValidatePartyDocumentLibrary", excep:=ex)
            m_sErrorString = ex.Message
            Throw (ex)
            Return m_lReturn
        End Try

    End Function
    ''' <summary>
    ''' Create And Update Party DocumentLibrary
    ''' </summary>
    ''' <param name="nPartyCnt"></param>
    ''' <param name="DocumentLibrary"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="spassword"></param>
    ''' <returns></returns>
    Public Function CreateAndUpdatePartyDocumentLibrary(ByVal DocumentLibrary As String, Optional ByVal sUserName As String = "", Optional ByVal spassword As String = "") As String
        Try
            Dim sNewDocumentLibrary As String = ""
            If (obSIRSharePointApi Is Nothing) Then
                InitialiseSPRestApi(SharepointURl, DocumentLibrary, sUserName, spassword)
            End If
            sNewDocumentLibrary = obSIRSharePointApi.CreatePartyDocumentLibrary()
            Return sNewDocumentLibrary
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    '''  'Archive a Document into Sharepoint. This will auto-generate the folder structure as:
    '<party code>
    '   <policynum>
    '       <emails>
    '   <claimnum>
    '       <emails>
    '   <general>
    '       <emails>
    ''' </summary>
    ''' <param name="PartyCnt"></param>
    ''' <param name="InsuranceFileCnt"></param>
    ''' <param name="ClaimID"></param>
    ''' <param name="CaseID"></param>
    ''' <param name="DocumentTemplateID"></param>
    ''' <param name="TemplateGroupID"></param>
    ''' <param name="TemplateSubGroupID"></param>
    ''' <param name="InternalOnly"></param>
    ''' <param name="SourceFile"></param>
    ''' <param name="SharepointPath"></param>
    ''' <param name="PartyCode"></param>
    ''' <param name="PolicyNumber"></param>
    ''' <param name="ClaimNumber"></param>
    ''' <param name="DestinationFilename"></param>
    ''' <param name="DebugMode"></param>
    ''' <param name="Background_Job_Id"></param>
    ''' <param name="IsGeneratedMail"></param>
    ''' <param name="bIsDMEMigration"></param>
    ''' <param name="sCreatedBy"></param>
    ''' <param name="sCreateddate"></param>
    ''' <param name="sArchiveDocFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ArchiveDocument(ByVal PartyCnt As Integer, ByVal InsuranceFileCnt As Integer,
                                     ByVal ClaimID As Integer, ByVal CaseID As Integer, ByVal DocumentTemplateID As Integer,
                                     ByVal TemplateGroupID As Integer, ByVal TemplateSubGroupID As Integer, ByVal InternalOnly As Boolean,
                                     ByVal SourceFile As String, ByRef SharepointPath As String,
                                     ByVal PartyCode As String, ByVal PolicyNumber As String,
                                     ByVal ClaimNumber As String,
                                     Optional ByVal DestinationFilename As String = "",
                                     Optional ByVal DebugMode As Boolean = False,
                                     Optional ByVal Background_Job_Id As Integer = 0,
                                     Optional ByVal IsGeneratedMail As Boolean = True,
                                     Optional ByVal bIsDMEMigration As Boolean = False,
                                     Optional ByVal sCreatedBy As String = "",
                                     Optional ByVal sCreateddate As DateTime = Nothing,
                                     Optional ByVal sArchiveDocFileName As String = "",
                                     Optional ByVal bIsCalledFromBackGroundJob As Boolean = False) As Integer
        Dim sSPURL As String = ""
        Dim sDocLib As String = ""
        Dim sQuoteRef As String = String.Empty
        Dim nInsuranceFileTypeId As Integer = 0
        Dim xbackgroundJobXml As String = String.Empty
        Dim IsTimeStampAppended As Boolean = False

        Try
            'Get the Sharepoint Server from the System Options
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionSharePointURl, r_sOptionValue:=sSPURL)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5086 - Sharepoint Server", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If
            If sSPURL <> "" Then
                SharepointURl = sSPURL
            End If

            'Get the Sharepoint Document Library from the System Options
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionSharePointDocLib, r_sOptionValue:=sDocLib)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5087 - Sharepoint Document Library", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If

            'For SharePoint Online - if not called from backgroundJob then create the background job
            If IsSharePointOnline And bIsCalledFromBackGroundJob = False Then
                'Create Background Job and Return
                CreateBackgroundJob(nPartyCnt:=PartyCnt, nInsuranceFileCnt:=InsuranceFileCnt, nClaimID:=ClaimID, nCaseID:=CaseID,
                                     nDocumentTemplateID:=DocumentTemplateID, nTemplateGroupID:=TemplateGroupID, nTemplateSubGroupID:=TemplateSubGroupID,
                                      sSourceFile:=SourceFile, sSharepointPath:=SharepointPath, sPartyCode:=PartyCode,
                                       sPolicyNumber:=PolicyNumber, sClaimNumber:=ClaimNumber, sDestinationFilename:=DestinationFilename,
                                      sArchiveDocFileName:=sArchiveDocFileName, IsGeneratedMail:=IsGeneratedMail,
                                      bIsDMEMigration:=bIsDMEMigration, bInternalOnly:=InternalOnly, sCreateddate:=sCreateddate)

                Return PMEReturnCode.PMTrue
            End If
            If bIsCalledFromBackGroundJob Then
                obSIRSharePointApi = Nothing
            End If
            If bIsDMEMigration = False Then
                'Get all the information about the Document for the Content Type Tags
                With m_oDatabase
                    .Parameters.Clear()
                    .Parameters.Add("document_template_id", DocumentTemplateID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("template_group_id", TemplateGroupID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("template_sub_group_id", TemplateSubGroupID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("party_cnt", PartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("insurance_file_cnt", InsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("claim_id", ClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("background_job_id", Convert.ToInt32(Background_Job_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .SQLSelect("spu_SIR_Get_Sharepoint_Tags", "Get Sharepoint Tags", True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the tags for the Sharepoint archive", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return m_lReturn
                    End If
                End With

                If PartyCode = "" Then
                    PartyCode = m_oDatabase.Records.Item(0).Fields("party_shortname").ToString.Trim
                End If

                If PolicyNumber = "" Then
                    PolicyNumber = m_oDatabase.Records.Item(0).Fields("policy_number").ToString.Trim
                ElseIf m_oDatabase.Records.Item(0).Fields("policy_number").ToString.Trim <> m_oDatabase.Records.Item(0).Fields("QUOTE_REF").ToString.Trim AndAlso m_oDatabase.Records.Item(0).Fields("Insurance_File_Type_Id").ToString.Trim = "2" Then
                    PolicyNumber = m_oDatabase.Records.Item(0).Fields("policy_number").ToString.Trim
                End If

                If ClaimNumber = "" Then
                    ClaimNumber = m_oDatabase.Records.Item(0).Fields("claim_number").ToString.Trim
                End If
                sQuoteRef = m_oDatabase.Records.Item(0).Fields("QUOTE_REF").ToString.Trim
                nInsuranceFileTypeId = CInt(m_oDatabase.Records.Item(0).Fields("Insurance_File_Type_Id"))
                xbackgroundJobXml = m_oDatabase.Records.Item(0).Fields("BackGroundJobXML").ToString
            End If

            '===================Fetch TimeStamp Flag From the Background XML =========================================================
            If xbackgroundJobXml <> "" Then
                Dim settings As New XmlReaderSettings
                settings.ValidationType = ValidationType.None
                Dim doc As XmlDocument = New XmlDocument()
                doc.Load(XmlReader.Create(New StringReader(xbackgroundJobXml), settings))

                'Create an XmlNamespaceManager for resolving namespaces.
                Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
                nsmgr.AddNamespace("xs", String.Empty)

                Dim nodeList As XmlNodeList
                Dim root As XmlElement = doc.DocumentElement
                Dim paramterNode As XmlNode = Nothing
                Dim parameterName As String = String.Empty
                Dim parameterValue As String = String.Empty

                ' Get all the DOCUPACKJob Parameters
                nodeList = root.SelectNodes("/BACKGROUND_JOB/JOB/PARAMETERS/PARAMETER", nsmgr)

                For Each paramterNode In nodeList
                    If Not paramterNode.Attributes.ItemOf("name") Is Nothing Then
                        parameterName = paramterNode.Attributes.ItemOf("name").Value.ToLower
                    End If
                    If Not paramterNode.Attributes.ItemOf("value") Is Nothing Then
                        parameterValue = paramterNode.Attributes.ItemOf("value").Value
                    End If
                    Select Case parameterName.ToUpper
                        Case "ISTIMESTAMPAPPENDED"
                            If Trim(parameterValue.ToUpper) = "TRUE" Then
                                IsTimeStampAppended = True
                            End If

                    End Select
                Next
            End If
            '=================== End =================================================================================================
            Dim SPService As New SharepointServices.Sharepoint
            Dim SPAttributes As New Dictionary(Of String, Object)()
            Dim filename As String = IO.Path.GetFileName(SourceFile)

            If DestinationFilename = "" Then
                If (Not String.IsNullOrEmpty(sArchiveDocFileName) AndAlso sArchiveDocFileName.Length > 0) Then
                    DestinationFilename = sArchiveDocFileName & IO.Path.GetExtension(SourceFile)
                Else
                    DestinationFilename = filename
                End If
            End If

            Dim sOptionValue As String = String.Empty
            'Get the Sharepoint Server from the System Options
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                v_sCallingAppName:=ACClass,
                                                v_iOptionNumber:=5145, r_sOptionValue:=sOptionValue)

            If Not String.IsNullOrEmpty(sOptionValue) AndAlso sOptionValue = "1" AndAlso IsTimeStampAppended = False Then
                'Get the Sharepoint Server from the System Options
                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                    v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                    v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                    v_sCallingAppName:=ACClass,
                                                    v_iOptionNumber:=5146, r_sOptionValue:=sOptionValue)

                If Not String.IsNullOrEmpty(sOptionValue) Then
                    Dim sFormatString As String = String.Empty
                    If sOptionValue = "0" Then
                        sFormatString = "yyyyMMdd hhmmss tt"
                    ElseIf sOptionValue = "1" Then
                        sFormatString = "MMddyyyy hhmmss tt"
                    End If

                    Dim sExt As String = Mid(DestinationFilename, DestinationFilename.LastIndexOf(".") + 1)
                    Dim sFileName As String = Informations.Left(DestinationFilename, DestinationFilename.LastIndexOf("."))
                    If bIsDMEMigration = False Then
                        'sFileName = sFileName & " " & Format(Date.Now, sFormatString)
                        sFileName = sFileName & " " & Date.Now.ToString(sFormatString)
                    Else
                        If Not String.IsNullOrEmpty(sCreateddate.ToString) Then
                            sFileName = sFileName & " " & sCreateddate.ToString(sFormatString)
                        End If
                    End If
                    DestinationFilename = sFileName & sExt
                End If
            End If

            'Read file into a byte array
            Dim contents As FileStream = IO.File.OpenRead(SourceFile)
            Dim lBytes As Integer = contents.Length
            Dim byteContents() As Byte = Nothing

            If lBytes > 0 Then
                ReDim byteContents(lBytes - 1)
                contents.Read(byteContents, 0, lBytes)
            End If

            contents.Close()

            If bIsDMEMigration = False Then
                Dim sTitle As String = Convert.ToString(m_oDatabase.Records.Item(0).Fields("document_template_description"))
                Dim sDocumentGroup As String = Convert.ToString(m_oDatabase.Records.Item(0).Fields("document_group"))
                Dim sDocumentSubGroup As String = Convert.ToString(m_oDatabase.Records.Item(0).Fields("document_sub_group"))
                Dim sPartyShortName As String = Convert.ToString(PartyCode)
                Dim sPartyFullName As String = Convert.ToString(m_oDatabase.Records.Item(0).Fields("party_resolved_name"))
                Dim sPolicyNumber As String = Convert.ToString(PolicyNumber)
                Dim sProductCode As String = Convert.ToString(m_oDatabase.Records.Item(0).Fields("product_code"))
                Dim sClaimNumber As String = Convert.ToString(ClaimNumber)
                Dim sClaimStatus As String = Convert.ToString(m_oDatabase.Records.Item(0).Fields("claim_status_description"))
                Dim sClaimPrimaryCause As String = Convert.ToString(m_oDatabase.Records.Item(0).Fields("claim_primary_cause_description"))
                Dim sAgentShortName As String = Convert.ToString(m_oDatabase.Records.Item(0).Fields("agent_shortname"))
                Dim sAgentFullName As String = Convert.ToString(m_oDatabase.Records.Item(0).Fields("agent_resolved_name"))
                Dim sPureUser As String = String.Empty
                If Not String.IsNullOrEmpty(NullToString(m_oDatabase.Records.Item(0).Fields("USER_NAME"))) Then
                    sPureUser = Convert.ToString(m_oDatabase.Records.Item(0).Fields("USER_NAME"))
                Else
                    sPureUser = m_sUsername

                End If

                sTitle = ReplaceString(sTitle)
                sDocumentGroup = ReplaceString(sDocumentGroup)
                sDocumentSubGroup = ReplaceString(sDocumentSubGroup)
                sPartyShortName = ReplaceString(sPartyShortName)
                sPartyFullName = ReplaceString(sPartyFullName)
                sPolicyNumber = ReplaceString(sPolicyNumber)
                sProductCode = ReplaceString(sProductCode)
                sClaimNumber = ReplaceString(sClaimNumber)
                sClaimStatus = ReplaceString(sClaimStatus)
                sClaimPrimaryCause = ReplaceString(sClaimPrimaryCause)
                sAgentShortName = ReplaceString(sAgentShortName)
                sAgentFullName = ReplaceString(sAgentFullName)
                sPureUser = ReplaceString(sPureUser)

                With SPAttributes
                    'Set the Tags
                    .Add("Title", sTitle)
                    .Add("DocumentGroup", sDocumentGroup)
                    .Add("DocumentSubGroup", sDocumentSubGroup)
                    .Add("PartyShortName", sPartyShortName)
                    .Add("PartyFullName", sPartyFullName)
                    .Add("PolicyNumber", sPolicyNumber)
                    .Add("ProductCode", sProductCode)
                    If Informations.IsDate(m_oDatabase.Records.Item(0).Fields("cover_start_date")) Then
                        .Add("CoverStartDate", DirectCast(m_oDatabase.Records.Item(0).Fields("cover_start_date"), DateTime).ToString("yyyy-MM-ddTHH:mm:ssZ"))
                    End If
                    If Informations.IsDate(m_oDatabase.Records.Item(0).Fields("cover_expiry_date")) Then
                        .Add("CoverExpiryDate", DirectCast(m_oDatabase.Records.Item(0).Fields("cover_expiry_date"), DateTime).ToString("yyyy-MM-ddTHH:mm:ssZ"))
                    End If
                    .Add("ClaimNumber", sClaimNumber)
                    If Informations.IsDate(m_oDatabase.Records.Item(0).Fields("loss_date")) Then
                        .Add("ClaimLossDate", DirectCast(m_oDatabase.Records.Item(0).Fields("loss_date"), DateTime).ToString("yyyy-MM-ddTHH:mm:ssZ"))
                    End If
                    .Add("ClaimStatus", sClaimStatus)
                    If Informations.IsDate(m_oDatabase.Records.Item(0).Fields("claim_payment_date")) Then
                        .Add("ClaimPaymentDate", DirectCast(m_oDatabase.Records.Item(0).Fields("claim_payment_date"), DateTime).ToString("yyyy-MM-ddTHH:mm:ssZ"))
                    End If
                    .Add("ClaimPrimaryCause", sClaimPrimaryCause)
                    .Add("ClaimIncurredAmount", m_oDatabase.Records.Item(0).Fields("claim_incurred_amount"))
                    .Add("AgentShortName", sAgentShortName)
                    .Add("AgentFullName", sAgentFullName)
                    .Add("InternalOnly", InternalOnly)
                    .Add("party_cnt", PartyCnt)
                    .Add("insurance_file_cnt", InsuranceFileCnt)
                    .Add("claim_id", ClaimID)
                    .Add("PureUser", sPureUser)

                End With
            Else
                With SPAttributes
                    'Set the Tags
                    .Add("DmeDate", sCreateddate.ToString("yyyy-MM-ddTHH:mm:ssZ"))
                    .Add("DmeUser", sCreatedBy)
                End With
            End If
            'Check the registry setting to override the debug mode passed in.
            'Passing true will get more detailed errors on upload.
            'DO NOT SET TO TRUE FOR PRODUCTION CODE!

            Dim archiveDebugMode As String = String.Empty
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SharePointArchiveDebugMode", r_sSettingValue:=archiveDebugMode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                ' Not fatal to just log a message and carry on.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to read the SharePointArchiveDebugMode registry setting.", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=0, vErrDesc:="Failed to read the SharePointArchiveDebugMode registry setting.")
            End If

            If archiveDebugMode = "1" Then
                DebugMode = True
            End If
            'Make call as per the sharepoint configuration, both implementations are different since sharepoint online has been implemented.
            If IsSharePointOnline Then
                If m_oSharepoinOnlineBusiness IsNot Nothing Then
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocTemplate.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument")
                        Return m_lReturn
                    End If
                    If (obSIRSharePointApi Is Nothing) Then
                        InitialiseSPRestApi(SharepointURl, sDocLib, m_oSharepoinOnlineBusiness.SharePointUsername, m_oSharepoinOnlineBusiness.SharepointPassword, True)
                    End If
                    ' Check the document library for client 
                    Dim sDocumentLibrary As String = ""
                    m_lReturn = CheckAndValidatePartyDocumentLibrary(PartyCnt, sDocumentLibrary, IsDME:=bIsDMEMigration, sDMEDestinationFilename:=DestinationFilename)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5087 - Sharepoint Document Library", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return m_lReturn
                    End If

                    If sDocumentLibrary.Length > 0 Then
                        sDocLib = sDocumentLibrary
                        If (obSIRSharePointApi IsNot Nothing) Then
                            obSIRSharePointApi.model.SharePointDocumentLibrary = sDocLib
                        End If
                    End If
                    SharepointPath = BuildDestinationPath(SharepointSite:=SharepointURl, SharepointLibrary:=sDocLib,
                                                                      PartyShortname:=PartyCode,
                                                                      PolicyNumber:=PolicyNumber,
                                                                      ClaimNumber:=ClaimNumber,
                                                                      Filename:=DestinationFilename, IsGeneratedMail:=IsGeneratedMail,
                                                                      bIsDMEMigration:=bIsDMEMigration)

                    'Rename the folder once it get live and move all files from the Quote to Policy.
                    If Not String.IsNullOrEmpty(sQuoteRef) _
                        AndAlso PolicyNumber.ToString.ToUpper <> sQuoteRef.ToString.ToUpper _
                        AndAlso nInsuranceFileTypeId = 2 Then

                        Dim sPolicyURL As String = Informations.Left(SharepointPath, SharepointPath.LastIndexOf("/") + 1)
                        Dim sQuoteURL As String = sPolicyURL.Replace(PolicyNumber, sQuoteRef)

                        m_lReturn = m_oSharepoinOnlineBusiness.RenameQuoteFolderToPolicyFolder(ToSafeString(sQuoteURL), ToSafeString(sPolicyURL), CType(obSIRSharePointApi, bSIRSharepointApi.bSIRSharepointApiCls))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Rename Quote Folder To Policy Folder", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return m_lReturn
                        End If

                        m_oSharepoinOnlineBusiness.Upload(ToSafeString(SharepointPath), CType(byteContents, Byte()), CType(SPAttributes, Dictionary(Of String, Object)), ToSafeString(sDocLib), ToSafeString(SourceFile), CType(obSIRSharePointApi, bSIRSharepointApi.bSIRSharepointApiCls), IsDME:=ToSafeBoolean(bIsDMEMigration))
                    Else
                        m_oSharepoinOnlineBusiness.Upload(ToSafeString(SharepointPath), CType(byteContents, Byte()), CType(SPAttributes, Dictionary(Of String, Object)), ToSafeString(sDocLib), ToSafeString(SourceFile), CType(obSIRSharePointApi, bSIRSharepointApi.bSIRSharepointApiCls), IsDME:=ToSafeBoolean(bIsDMEMigration))
                    End If
                End If
            Else
                SPService.DebugMode = DebugMode

                If (obSIRSharePointApi Is Nothing) Then
                    InitialiseSPRestApi(SharepointURl, sDocLib, "", "")
                End If
                Dim sSPDocumentLibrary As String = ""
                m_lReturn = CheckAndValidatePartyDocumentLibrary(PartyCnt, sSPDocumentLibrary, IsDME:=bIsDMEMigration, sDMEDestinationFilename:=DestinationFilename)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5087 - Sharepoint Document Library", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return m_lReturn
                End If

                If sSPDocumentLibrary.Length > 0 Then
                    sDocLib = sSPDocumentLibrary
                    If (obSIRSharePointApi IsNot Nothing) Then
                        obSIRSharePointApi.model.SharePointDocumentLibrary = sDocLib
                    End If
                End If
                SharepointPath = BuildDestinationPath(SharepointSite:=SharepointURl, SharepointLibrary:=sDocLib,
                                                                  PartyShortname:=PartyCode,
                                                                  PolicyNumber:=PolicyNumber,
                                                                  ClaimNumber:=ClaimNumber,
                                                                  Filename:=DestinationFilename, IsGeneratedMail:=IsGeneratedMail,
                                                                  bIsDMEMigration:=bIsDMEMigration)
                '  Dim bResult As Boolean
                'obSIRSharePointApi.Upload(New FileInfo(destinationUrl, bytes, properties), SharepointPath, byteContents, SPAttributes, sDocLib, bIsDMEMigration)
                SPService.Upload(SharepointPath, byteContents, SPAttributes, sDocLib, bIsDMEMigration, SourceFile, obSIRSharePointApi)
                If Not String.IsNullOrEmpty(sQuoteRef) AndAlso PolicyNumber.ToString.ToUpper <> sQuoteRef.ToString.ToUpper AndAlso nInsuranceFileTypeId = 2 Then

                    Dim sPolicyURL As String = Informations.Left(SharepointPath, SharepointPath.LastIndexOf("/") + 1)
                    Dim sQuoteURL As String = sPolicyURL.Replace(PolicyNumber, sQuoteRef)
                    Dim sQuoteFolderPath As String = sQuoteURL.Replace(sSPURL, "")
                    Dim sPolicyFolderPath As String = sPolicyURL.Replace(sSPURL, "")

                    If (sQuoteFolderPath.StartsWith("/")) Then sQuoteFolderPath = sQuoteFolderPath.Substring(1, sQuoteFolderPath.Length - 1)
                    If (sPolicyFolderPath.StartsWith("/")) Then sPolicyFolderPath = sPolicyFolderPath.Substring(1, sPolicyFolderPath.Length - 1)
                    If (sQuoteFolderPath.EndsWith("/")) Then sQuoteFolderPath = sQuoteFolderPath.Substring(0, sQuoteFolderPath.Length - 1)
                    If (sPolicyFolderPath.EndsWith("/")) Then sPolicyFolderPath = sPolicyFolderPath.Substring(0, sPolicyFolderPath.Length - 1)

                    obSIRSharePointApi.GetFileLists(sQuoteFolderPath, sPolicyFolderPath, True)

                End If
            End If

            'Catch ex As SoapException
            '    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=ex.Detail.InnerText, vErrDesc:=Informations.Err().Description, excep:=ex)
            '    m_sErrorString = ex.Message
            '    Throw (ex)
            '    Return m_lReturn
        Catch ex As Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message & " " & SharepointPath, vApp:=ACApp, vClass:=ACClass,
                               vMethod:="ArchiveDocument", excep:=ex)
            m_sErrorString = ex.Message
            Throw (ex)
            Return m_lReturn
        End Try

        Return m_lReturn
    End Function

    ''' <summary>
    ''' Get a list of documents
    ''' </summary>
    ''' <param name="PartyShortname"></param>
    ''' <param name="PolicyNumber"></param>
    ''' <param name="ClaimNumber"></param>
    ''' <param name="destinationUrl"></param>
    ''' <param name="FileList"></param>
    ''' <param name="CreateFolder"></param>
    ''' <returns></returns>
    Public Function GetFileList(ByVal PartyShortname As String, ByVal PolicyNumber As String,
                                ByVal ClaimNumber As String, ByRef destinationUrl As String,
                                ByRef FileList As DataTable,
                                Optional ByVal CreateFolder As Boolean = False) As Integer
        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        Try
            Dim sSPURL As String = ""
            Dim sDocLib As String = ""

            If destinationUrl Is Nothing Then
                destinationUrl = ""
            End If

            'Get the Sharepoint Server from the System Options
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionSharePointURl, r_sOptionValue:=sSPURL)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5086 - Sharepoint Server", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If

            'Get the Sharepoint Document Library from the System Options
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=kSystemOptionSharePointDocLib, r_sOptionValue:=sDocLib)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5087 - Sharepoint Document Library", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If

            If destinationUrl.Length = 0 Then
                'Build the default path
                Dim sDocumentLibrary As String
                sDocumentLibrary = gPMFunctions.GetDocumentLibrary(m_oDatabase, 0, PartyShortname)
                If sDocumentLibrary <> "" Then
                    sDocLib = sDocumentLibrary
                End If
                destinationUrl = BuildDestinationPath(sSPURL, sDocLib, PartyShortname, PolicyNumber, ClaimNumber, "")
            End If

            'Sharepoint Online Implementation
            If IsSharePointOnline Then
                If m_oSharepoinOnlineBusiness IsNot Nothing Then

                    If CreateFolder Then
                        m_oSharepoinOnlineBusiness.CreateSharePointOnlineFolders(ToSafeString(destinationUrl))
                    End If

                    'Impelemnted new GetList Method to retreive the files from sharepoint line.
                    FileList = m_oSharepoinOnlineBusiness.GetFileList(ToSafeString(sSPURL), ToSafeString(sDocLib), ToSafeString(destinationUrl))
                End If
            Else

                Dim SPService As New SharepointServices.Sharepoint
                If (obSIRSharePointApi Is Nothing) Then
                    InitialiseSPRestApi(sSPURL, sDocLib, "", "")
                End If
                If CreateFolder Then
                    Dim sFolderNames As String = ""
                    If destinationUrl.EndsWith("/") Then
                        sFolderNames = destinationUrl.Replace(sSPURL + sDocLib + "/", "")
                    ElseIf destinationUrl.Length > 0 Then
                        sFolderNames = destinationUrl.Replace(sSPURL + "/" + sDocLib + "/", "")
                    End If

                    Dim lstFolderNames As String()
                    If sFolderNames.Length > 0 Then
                        If sFolderNames.EndsWith("/") Then
                            sFolderNames = sFolderNames.Substring(0, sFolderNames.Length - 1)
                        End If
                        lstFolderNames = sFolderNames.Split("/")

                        Dim bFolderCreated As Boolean = obSIRSharePointApi.CreateSharePointFolders(lstFolderNames)
                    End If
                End If

                FileList = obSIRSharePointApi.GetFileList(destinationUrl)

            End If
        Catch ex As Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFileList", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Throw (ex)
            Return m_lReturn
        End Try
        Return m_lReturn
    End Function
    'This function generates a default path for a Party, Policy, Claim
    ''' <summary>
    ''' GenerateDefaultPath
    ''' </summary>
    ''' <param name="PartyCnt"></param>
    ''' <param name="InsuranceFileCnt"></param>
    ''' <param name="ClaimID"></param>
    ''' <param name="CaseID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateDefaultPath(ByVal PartyCnt As Integer, ByVal InsuranceFileCnt As Integer,
                                     ByVal ClaimID As Integer, ByVal CaseID As Integer) As Integer

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        Try
            Dim sSPURL As String = ""
            Dim sDocLib As String = ""
            Dim sSharepointPath As String = ""
            Dim sArchiveOption As String = ""

            'Get the Sharepoint Server from the System Options
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=10, r_sOptionValue:=sArchiveOption)
            If sArchiveOption = "2" Then
                'Sharepoint Only
                'Get the Sharepoint Server from the System Options
                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                    v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                    v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                    v_sCallingAppName:=ACApp,
                                                    v_iOptionNumber:=5085, r_sOptionValue:=sSPURL)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5086 - Sharepoint Server", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDefaultPath")
                    Return m_lReturn
                End If

                'Get the Sharepoint Document Library from the System Options
                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                    v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                                    v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel,
                                                    v_sCallingAppName:=ACApp,
                                                    v_iOptionNumber:=5086, r_sOptionValue:=sDocLib)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5087 - Sharepoint Document Library", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDefaultPath")
                    Return m_lReturn
                End If

                'For Sharepoint Online 
                If IsSharePointOnline = True Then
                    If m_oSharepoinOnlineBusiness IsNot Nothing Then

                        m_lReturn = m_oSharepoinOnlineBusiness.GenerateDefaultPathForSharePointOnline(nPartyCnt:=ToSafeInteger(PartyCnt), nInsuranceFileCnt:=ToSafeInteger(InsuranceFileCnt), nClaimID:=ToSafeInteger(ClaimID), nCaseID:=0)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GenerateDefaultPathForSharePointOnline", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDefaultPath")
                            Return m_lReturn
                        End If

                    End If
                    'Note-For the Email  folder logic moved to the Sharepoint online when the atucal party folder get created.
                Else

                    InitialiseSPRestApi(sSPURL, sDocLib, "", "")
                    ' Check the document library for client 
                    Dim sDocumentLibrary As String = ""
                    m_lReturn = CheckAndValidatePartyDocumentLibrary(PartyCnt, sDocumentLibrary)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the system option 5087 - Sharepoint Document Library", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return m_lReturn
                    End If

                    If sDocumentLibrary.Length > 0 Then
                        sDocLib = sDocumentLibrary
                    End If
                    If (obSIRSharePointApi IsNot Nothing) Then
                        obSIRSharePointApi.model.SharePointDocumentLibrary = sDocLib
                    End If
                    'Get all the information about the Document for the Content Type Tags
                    With m_oDatabase
                        .Parameters.Clear()
                        .Parameters.Add("document_template_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add("template_group_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add("template_sub_group_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add("party_cnt", PartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add("insurance_file_cnt", InsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add("claim_id", ClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        m_lReturn = .SQLSelect("spu_SIR_Get_Sharepoint_Tags", "Get Sharepoint Tags", True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the tags for the Sharepoint archive", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return m_lReturn
                        End If
                    End With

                    Dim SPService As New SharepointServices.Sharepoint
                    Dim SPAttributes As New Dictionary(Of String, Object)()

                    'Use a dummy filename to just build the path
                    sSharepointPath = BuildDestinationPath(SharepointSite:=sSPURL, SharepointLibrary:=sDocLib,
                                                      PartyShortname:=m_oDatabase.Records.Item(0).Fields("party_shortname"),
                                                      PolicyNumber:=m_oDatabase.Records.Item(0).Fields("policy_number"),
                                                      ClaimNumber:=m_oDatabase.Records.Item(0).Fields("claim_number"),
                                                      Filename:="x.DOC")

                    If Not String.IsNullOrEmpty(m_oDatabase.Records.Item(0).Fields(5)) _
                        AndAlso m_oDatabase.Records.Item(0).Fields(18).ToString.ToUpper <> m_oDatabase.Records.Item(0).Fields(5).ToString.ToUpper _
                        AndAlso m_oDatabase.Records.Item(0).Fields(19) = 2 AndAlso String.IsNullOrEmpty(m_oDatabase.Records.Item(0).Fields(11)) Then

                        Dim sQuoteRef As String = m_oDatabase.Records.Item(0).Fields(18).ToString.ToUpper.Trim
                        Dim sPolicyNumber As String = m_oDatabase.Records.Item(0).Fields(5).ToString.ToUpper.Trim
                        Dim sPolicyURL As String = Informations.Left(sSharepointPath, sSharepointPath.LastIndexOf("/") + 1)
                        Dim sQuoteURL As String = sPolicyURL.Replace(sPolicyNumber, sQuoteRef)
                        sPolicyURL = sPolicyURL.Replace("%20", " ")
                        sQuoteURL = sQuoteURL.Replace("%20", " ")
                        sQuoteURL = sQuoteURL.Substring(0, sQuoteURL.Length - 1)


                        Dim sQuoteFolderPath As String = sQuoteURL.Replace(sSPURL, "")
                        Dim sPolicyFolderPath As String = sPolicyURL.Replace(sSPURL, "")

                        If (sQuoteFolderPath.StartsWith("/")) Then sQuoteFolderPath = sQuoteFolderPath.Substring(1, sQuoteFolderPath.Length - 1)
                        If (sPolicyFolderPath.StartsWith("/")) Then sPolicyFolderPath = sPolicyFolderPath.Substring(1, sPolicyFolderPath.Length - 1)
                        If (sQuoteFolderPath.EndsWith("/")) Then sQuoteFolderPath = sQuoteFolderPath.Substring(0, sQuoteFolderPath.Length - 1)
                        If (sPolicyFolderPath.EndsWith("/")) Then sPolicyFolderPath = sPolicyFolderPath.Substring(0, sPolicyFolderPath.Length - 1)

                        obSIRSharePointApi.RenameQuoteFolderToPolicyFolder(sQuoteRef, sPolicyNumber, sQuoteFolderPath, sPolicyFolderPath, False)

                    Else

                        Dim uri As String = sSharepointPath.Replace("%20", " ")

                        Dim sFolderNames As String = ""
                        Dim sSharePointUrl As String = obSIRSharePointApi.model.SharePointSiteURL
                        Dim bFolderCreated As Boolean = False
                        obSIRSharePointApi.model.SharePointDocumentLibrary = sDocLib
                        If sSharePointUrl.EndsWith("/") Then
                            sFolderNames = uri.Replace(sSharePointUrl + sDocLib + "/", "")
                        ElseIf sSharePointUrl.Length > 0 Then
                            sFolderNames = uri.Replace(sSharePointUrl + "/" + sDocLib + "/", "")
                        End If

                        Dim fileName As String = System.IO.Path.GetFileName(uri)
                        sFolderNames = sFolderNames.Replace(fileName, "")
                        Dim lstFolderNames As String()
                        If sFolderNames.Length > 0 Then
                            If sFolderNames.EndsWith("/") Then
                                sFolderNames = sFolderNames.Substring(0, sFolderNames.Length - 1)
                            End If
                            lstFolderNames = sFolderNames.Split("/")

                            bFolderCreated = obSIRSharePointApi.CreateSharePointFolders(lstFolderNames)
                        End If

                        If bFolderCreated AndAlso
                            (m_oDatabase.Records.Item(0).Fields("policy_number") = "" And
                            m_oDatabase.Records.Item(0).Fields("claim_number") = "") Then
                            sSharepointPath = BuildDestinationPath(SharepointSite:=sSPURL, SharepointLibrary:=sDocLib,
                                                                  PartyShortname:=m_oDatabase.Records.Item(0).Fields("party_shortname"),
                                                                  PolicyNumber:=m_oDatabase.Records.Item(0).Fields("policy_number"),
                                                                  ClaimNumber:=m_oDatabase.Records.Item(0).Fields("claim_number"),
                                                                  Filename:="x.EML", IsGeneratedMail:=True)

                            uri = sSharepointPath
                            If sSharePointUrl.EndsWith("/") Then
                                sFolderNames = uri.Replace(sSharePointUrl + sDocLib + "/", "")
                            ElseIf sSharePointUrl.Length > 0 Then
                                sFolderNames = uri.Replace(sSharePointUrl + "/" + sDocLib + "/", "")
                            End If

                            fileName = System.IO.Path.GetFileName(uri)
                            sFolderNames = sFolderNames.Replace(fileName, "")

                            If sFolderNames.Length > 0 Then
                                If sFolderNames.EndsWith("/") Then
                                    sFolderNames = sFolderNames.Substring(0, sFolderNames.Length - 1)
                                End If
                                lstFolderNames = sFolderNames.Split("/")

                                bFolderCreated = obSIRSharePointApi.CreateSharePointFolders(lstFolderNames)
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", excep:=ex)
            Throw (ex)
            Return m_lReturn
        End Try
        Return m_lReturn
    End Function

    ''' <summary>
    ''  This function returns the path in the Document Libary based on the Type of Document
    ''' </summary>
    ''' <param name="SharepointSite"></param>
    ''' <param name="SharepointLibrary"></param>
    ''' <param name="PartyShortname"></param>
    ''' <param name="PolicyNumber"></param>
    ''' <param name="ClaimNumber"></param>
    ''' <param name="Filename"></param>
    ''' <param name="IsGeneratedMail"></param>
    ''' <param name="bIsDMEMigration"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
        SharepointLibrary.Trim.Replace(" ", "%20")
        If Not Filename.ToUpper.Contains("/REPORTS/") AndAlso Not bIsDMEMigration Then
            If (Filename.ToUpper.EndsWith(".EML") Or Filename.ToUpper.EndsWith(".MSG")) And IsGeneratedMail Then
                DestinationFolder &= PartyShortname.Trim & "/"
                If ClaimNumber.Length > 0 Then
                    DestinationFolder &= "Claim" & "/"
                    DestinationFolder &= ClaimNumber.Trim & "/"
                ElseIf PolicyNumber.Length > 0 Then
                    DestinationFolder &= "Policy" & "/"
                    DestinationFolder &= PolicyNumber.Trim & "/"
                Else
                    DestinationFolder &= "Generated Emails" & "/"
                End If
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
    ''' <summary>
    ''' This Method will Create the BackGround Job - to create the Sharepoint Folder Structure
    ''' </summary>
    ''' <param name="nPartyCnt"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nClaimID"></param>
    ''' <param name="nCaseID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerateDefaultPathBackgroundJob(ByVal nPartyCnt As Integer,
                                                      ByVal nInsuranceFileCnt As Integer,
                                                      ByVal nClaimID As Integer, ByVal nCaseID As Integer)
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim xlItem As System.Xml.Linq.XElement = New System.Xml.Linq.XElement("BACKGROUND_JOB")
        xlItem = <BACKGROUND_JOB>
                     <JOB jobtype="DOCUPACK">
                         <PARAMETERS>
                             <PARAMETER name="destination" value="archive"/>
                             <PARAMETER name="archive" value="true"/>
                             <PARAMETER name="CreateFolderStructure" value="true"/>
                             <PARAMETER name="PartyCnt" value=<%= nPartyCnt %>/>
                             <PARAMETER name="ClaimID" value=<%= nClaimID %>/>
                             <PARAMETER name="InsuranceFileCnt" value=<%= nInsuranceFileCnt %>/>
                         </PARAMETERS>
                         <ITEMS></ITEMS>
                     </JOB>
                 </BACKGROUND_JOB>

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("background_job_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
            .Parameters.Add("description", "GenerateDefaultPath", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            .Parameters.Add("job_xml", xlItem.ToString, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            .Parameters.Add("job_when_to_start", DateTime.Now, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            .Parameters.Add("job_user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            nReturn = .SQLAction("spu_SIR_Background_Job_add", "Create Background Job", True)
        End With
        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to execute spu_SIR_Background_Job_add", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDefaultPathBackgroundJob")
        End If
        Return nReturn
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nPartyCnt"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nClaimID"></param>
    ''' <param name="nCaseID"></param>
    ''' <param name="nDocumentTemplateID"></param>
    ''' <param name="nTemplateGroupID"></param>
    ''' <param name="nTemplateSubGroupID"></param>
    ''' <param name="bInternalOnly"></param>
    ''' <param name="sSourceFile"></param>
    ''' <param name="sSharepointPath"></param>
    ''' <param name="sPartyCode"></param>
    ''' <param name="sPolicyNumber"></param>
    ''' <param name="sClaimNumber"></param>
    ''' <param name="sDestinationFilename"></param>
    ''' <param name="DebugMode"></param>
    ''' <param name="Background_Job_Id"></param>
    ''' <param name="IsGeneratedMail"></param>
    ''' <param name="bIsDMEMigration"></param>
    ''' <param name="sCreatedBy"></param>
    ''' <param name="sCreateddate"></param>
    ''' <param name="sArchiveDocFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateBackgroundJob(ByVal nPartyCnt As Integer, ByVal nInsuranceFileCnt As Integer,
                                         ByVal nClaimID As Integer, ByVal nCaseID As Integer, ByVal nDocumentTemplateID As Integer,
                                         ByVal nTemplateGroupID As Integer, ByVal nTemplateSubGroupID As Integer, ByVal bInternalOnly As Boolean,
                                         ByVal sSourceFile As String, ByRef sSharepointPath As String,
                                         ByVal sPartyCode As String, ByVal sPolicyNumber As String, ByVal sClaimNumber As String,
                                         Optional ByVal sDestinationFilename As String = "",
                                         Optional ByVal DebugMode As Boolean = False,
                                         Optional ByVal Background_Job_Id As Integer = 0,
                                         Optional ByVal IsGeneratedMail As Boolean = True,
                                         Optional ByVal bIsDMEMigration As Boolean = False,
                                         Optional ByVal sCreatedBy As String = "",
                                         Optional ByVal sCreateddate As DateTime = Nothing,
                                         Optional ByVal sArchiveDocFileName As String = "")
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim xlJob As System.Xml.Linq.XElement =
          <BACKGROUND_JOB>
              <JOB jobtype="DOCUPACK">
                  <PARAMETERS>
                      <PARAMETER name="destination" value="archive"/>
                      <PARAMETER name="archive" value="true"/>
                      <PARAMETER name="PartyCnt" value=<%= nPartyCnt %>/>
                      <PARAMETER name="ClaimID" value=<%= nClaimID %>/>
                      <PARAMETER name="InsuranceFileCnt" value=<%= nInsuranceFileCnt %>/>
                      <PARAMETER name="bIsDMEMigration" value=<%= bIsDMEMigration %>/>
                  </PARAMETERS>
                  <ITEMS>

                  </ITEMS>
              </JOB>
          </BACKGROUND_JOB>
        Dim sDocumentTemplateCode As String = ""
        GetdocumentTemplateCode(nDocumentTemplateID, sDocumentTemplateCode)
        Dim sFileLocation As String = sSourceFile
        Dim sOutputFormat As String = Right(sFileLocation, Len(sFileLocation) - sFileLocation.LastIndexOf(".") - 1).ToUpper

        If sFileLocation <> "" Then
            Dim xlPath As System.Xml.Linq.XElement = <PARAMETER name="Path" value=<%= sFileLocation %>/>
            xlJob.Element("JOB").Element("PARAMETERS").Add(xlPath)
            'we need to specify format
            xlJob.Element("JOB").Element("PARAMETERS").Add(New System.Xml.Linq.XElement(<PARAMETER name="OutputFormat" value=<%= sOutputFormat %>/>))
            'documents to generate so specify the document template code
            Dim xlCode As System.Xml.Linq.XElement = <PARAMETER name="code" value=<%= sDocumentTemplateCode %>/>
            xlJob.Element("JOB").Element("PARAMETERS").Add(xlCode)

        Else
            'documents to generate so specify the document template code
            Dim xlCode As System.Xml.Linq.XElement = <PARAMETER name="code" value=<%= sDocumentTemplateCode %>/>
            xlJob.Element("JOB").Element("PARAMETERS").Add(xlCode)
        End If

        Dim xlInternalonly As System.Xml.Linq.XElement = <PARAMETER name="Internalonly" value=<%= bInternalOnly %>/>
        xlJob.Element("JOB").Element("PARAMETERS").Add(xlInternalonly)
        Dim xlDocumentTemplateGroupID As System.Xml.Linq.XElement = <PARAMETER name="DocumentTemplateGroupID" value=<%= nTemplateGroupID %>/>
        xlJob.Element("JOB").Element("PARAMETERS").Add(xlDocumentTemplateGroupID)
        Dim xlDocumentTemplateSubGroupID As System.Xml.Linq.XElement = <PARAMETER name="DocumentTemplateSubGroupID" value=<%= nTemplateSubGroupID %>/>
        xlJob.Element("JOB").Element("PARAMETERS").Add(xlDocumentTemplateSubGroupID)

        If ToSafeString(sDestinationFilename) = "" Then
            sDestinationFilename = sArchiveDocFileName
        End If
        Dim xlDestinationFilename As System.Xml.Linq.XElement = <PARAMETER name="DestinationFilename" value=<%= sDestinationFilename %>/>
        xlJob.Element("JOB").Element("PARAMETERS").Add(xlDestinationFilename)
        Dim sIsTimeStampAppended As String = "False"
        If nPartyCnt = 0 AndAlso nClaimID = 0 AndAlso nInsuranceFileCnt = 0 Then
            sIsTimeStampAppended = "True"
        End If
        Dim xIsTimeStampAppended As System.Xml.Linq.XElement = <PARAMETER name="IsTimeStampAppended" value=<%= sIsTimeStampAppended %>/>
        xlJob.Element("JOB").Element("PARAMETERS").Add(xIsTimeStampAppended)

        Dim xCreatedDate As System.Xml.Linq.XElement = <PARAMETER name="sCreateddate" value=<%= sCreateddate %>/>
        xlJob.Element("JOB").Element("PARAMETERS").Add(xCreatedDate)

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("background_job_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
            .Parameters.Add("description", "Archive documents", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            .Parameters.Add("job_xml", xlJob.ToString, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            .Parameters.Add("job_when_to_start", DateTime.Now, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            .Parameters.Add("job_user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            nReturn = .SQLAction("spu_SIR_Background_Job_add", "Create Background Job", True)
        End With
        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to execute spu_SIR_Background_Job_add", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDefaultPathBackgroundJob")
        End If
        Return nReturn
    End Function

    Private Function GetdocumentTemplateCode(nDocumentTemplateID As Integer, ByRef sDocumentTemplateCode As String)
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim oResultArray(,) As Object = Nothing
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("document_template_id", nDocumentTemplateID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            nReturn = .SQLSelect("spe_document_template_sel", "document_template_sel", True, vResultArray:=oResultArray)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("GetdocumentTemplateCode Failed.")
            End If
            If Informations.IsArray(oResultArray) Then
                sDocumentTemplateCode = ToSafeString(oResultArray(1, 0)).Trim
            End If
        End With
        Return nReturn
    End Function

    Private Shared Function ReplaceString(ByRef str As String) As String
        Dim sillegalChars As Char() = ":~""%&*:<>?/\{}|".ToCharArray()

        Dim ssb As New System.Text.StringBuilder

        For Each ch As Char In str
            If ch = "#"c Then
                ' URL encode the # character for SharePoint compatibility
                ssb.Append("%23")
            ElseIf Array.IndexOf(sillegalChars, ch) = -1 Then
                ssb.Append(ch)
            End If
        Next
        Return ssb.ToString()
    End Function
End Class