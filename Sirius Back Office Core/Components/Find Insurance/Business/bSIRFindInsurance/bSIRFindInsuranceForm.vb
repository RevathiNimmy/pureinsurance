Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Text
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 07/10/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, business rules required for the
    '              SIRFindInsurance summary form.
    '
    ' Edit History:
    'TF071098 - Created from bFindInsurance
    'sj040800  - Add "PolicyTypeId" parameter to "SearchAllGIIM"
    '                           method ( used by Gemini only)
    ' SJP14062002 - getUnderWritingOrAgency and checkForUnderWriting
    '               use new product options scheme
    ' SJP04072002 calcCombinedKey will pass in Insurance File Cnt
    ' ED 05082002 : Code added to search SBO Policy based on the Front Office
    '               data based on the registry setting, whethere Carole Nash
    '               Search is activated
    ' CJB23032005 : PN19733 Added GetCurrentPolicyVersion to enable current active policy
    '               version identification and to denote it in list policy version listview.
    ' CJB28062005 : PN21986 Set default value for v_vOldXMLDataSet to "" in CopyDataSet to prevent errors
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 12/01/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer
    Private m_iFileType As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    Private m_oGISDatabase As dPMDAO.Database


    Private m_oGIS As Object
    Private m_oDataSet As cGISDataSetControl.Application
    Private m_lRiskId As Integer
    Private m_lRiskTypeId As Integer
    Private m_lScreenId As Integer


    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lInsuranceFileCnt As Integer

    Private m_lPartyCnt As Integer
    ' Insurance Folder ID
    Private m_lInsuranceFolderCnt As Integer

    'Link to Gemini
    Private m_oVehicle As Object

    Private m_bGeminiLink As Boolean
    Private m_bGeminiIILink As Boolean
    Private m_bSwiftLink As Boolean

    Private m_sUnderwritingOrAgency As String = ""

    Private m_lFindMode As Integer
    Private m_bBackDatedMTA As Boolean

    Private m_bAffectedArrayFilled As Boolean = False
    'RJG 21/06/2000 - I have duplicated this E-Num in our Sirius Link object so
    'that calling gemini net apps can get to it.
    Public Enum InsuranceFileSearchType
        IFSTQuote = 1
        IFSTPolicy = 2
        IFSTRenewal = 3
        IFSTQuotePolicy = 4
        IFSTQuotePolicyRenewal = 5
        IFSTMTAQuote = 6
        IFSTMTAQuoteMTATempQuote = 7
        IFSTQuoteQteCan = 8
    End Enum

    ' CTAF 090701
    'developer guide no 39. 
    Private Const ACCopyPolicyV2SQL As String = "spu_pmb_copy_policy"
    Private Const ACCopyPolicyV2Name As String = "CopyPolicyV2"
    Private Const ACCopyPolicyV2Stored As Boolean = True
    'developer guide no 39. 
    Private Const ACCopyPolicyV3SQL As String = "spu_pmb_copy_policy"
    Private Const ACCopyPolicyV3Name As String = "CopyPolicyV3"
    Private Const ACCopyPolicyV3Stored As Boolean = True

    Private Const ACGetPartyIdFromKeyStored As Boolean = False
    Private Const ACGetPartyIdFromKeyName As String = "PartyKeyFromPartyCnt"
    Private Const ACGetPartyIdFromKeySQL As String = "SELECT party_id, source_id FROM Party WHERE party_cnt = {party_cnt}"


    'SJ 23/02/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    'SJ 23/02/2004 - end

    'ED 05082002 : Added the following to check whether Registration Search is activated
    Private m_bRegSearch As Boolean

    Private m_vMTAInsuranceFileLinkArray As Object
    'developer guide no 39. 
    Private Const ACAddMtaInsuranceFileLinkSQL As String = "spu_SIR_mta_insurance_file_link_add"
    Private Const ACAddMtaInsuranceFileLinkName As String = "CopyPolicyV3"
    Private Const ACAddMtaInsuranceFileLinkStored As Boolean = True

    '1.12 PLICO45
    Private Const MTA_DATE_NOT_ALLOWED As Integer = 0
    Private Const MTA_DATE_CURRENT_PERIOD_ONLY As Integer = 1
    Private Const MTA_DATE_CURRENT_PLUS_1 As Integer = 2
    Private Const MTA_DATE_UNRESTRICTED As Integer = 3
    'WPR 33-75 added
    Public Function GetCancellationDate(ByVal v_lInsuranceFolderCnt As Integer, ByRef m_dtCancellationDate As Date, Optional ByRef r_lInsFileCnt As Integer = 0) As Integer
        'WPR 33-75 END
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsFolderCnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCancellationDateSQL, sSQLName:=ACGetCancellationDateName, bStoredProcedure:=True, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            ElseIf informations.IsArray(vArray) Then

                m_dtCancellationDate = CDate(vArray(0, 0))
                'WPR 33-75 added
                r_lInsFileCnt = ToSafeInteger(vArray(1, 0), 0)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCancellationDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCancellationDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    Public Property RiskId() As Integer
        Get
            Return m_lRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    Public Property RiskTypeId() As Integer
        Get
            Return m_lRiskTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property
    Public Property ScreenId() As Integer
        Get
            Return m_lScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lScreenId = Value
        End Set
    End Property


    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public ReadOnly Property GeminiLink() As Boolean
        Get

            Return m_bGeminiLink

        End Get
    End Property

    Public ReadOnly Property GeminiIILink() As Boolean
        Get

            Return m_bGeminiIILink

        End Get
    End Property

    Public ReadOnly Property SwiftLink() As Boolean
        Get

            Return m_bSwiftLink

        End Get
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    Public Property FindMode() As Integer
        Get
            Return m_lFindMode
        End Get
        Set(ByVal Value As Integer)
            m_lFindMode = Value
        End Set
    End Property

    Public ReadOnly Property RegSearch() As Boolean
        Get
            Return m_bRegSearch
        End Get
    End Property
    Public Property BackDatedMTA() As Boolean
        Get
            Return m_bBackDatedMTA
        End Get
        Set(ByVal value As Boolean)
            m_bBackDatedMTA = value
        End Set
    End Property



    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageId As Integer, ByVal iCurrencyId As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageId
            m_iSourceID = iSourceId
            m_iCurrencyID = iCurrencyId
            m_iLogLevel = iLogLevel

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDataSet = New cGISDataSetControl.Application()

            m_bSwiftLink = False

            m_lReturn = gPMComponentServices.CheckPMProductInstalled(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFGeminiII, r_bInstalled:=m_bGeminiIILink)

            'Tomo210301
            'This should speed up loading the object - it's been slow on site
            If UnderwritingOrAgency = "U" Then
                m_bGeminiIILink = False
                m_bGeminiLink = False
                Return result
            End If

            'New PMDAO to get the extra gemini (or should it be GIS database)
            'Only if GII is installed
            If m_bGeminiIILink Then
                m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFGeminiII, r_oDatabase:=m_oGISDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'TF210802 - Move this down as only GII will be merged anyway (?)
                'End If

                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oVehicle, v_sClassName:="bSirToGemVehicle.Business", v_sCallingAppName:=ACApp, v_sUsername:=sUsername, v_sPassword:=sPassword, v_iUserID:=iUserID, v_iSourceID:=iSourceId, v_iLanguageID:=iLanguageId, v_iCurrencyID:=iCurrencyId, v_iLogLevel:=iLogLevel)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_bGeminiLink = False
                    Return result
                End If

                m_bGeminiLink = m_oVehicle.PMGeminiLink

                If Not m_bGeminiLink Then

                    m_oVehicle.Dispose()
                    m_oVehicle = Nothing
                End If
            End If

            'ED 05082002 : Check whether Registration Search is activated
            m_bRegSearch = AllowRegSearch()

            'SJ 23/02/2004 - start
            'Are we running the folgate branch acting as insurer solution
            m_lReturn = bUnderwritingBranchFunc.GetUnderwritingBranchDetails(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase, v_sCallingAppName:=m_sCallingAppName, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUnderwritingBranchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oDataSet IsNot Nothing Then
                    m_oDataSet.Dispose()
                    m_oDataSet = Nothing
                End If
                If Not (m_oGISDatabase Is Nothing) Then
                    m_oGISDatabase.CloseDatabase()


                    m_oGISDatabase = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If

                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' SearchByQuery
    ''' </summary>
    ''' <param name="r_vResultArray"></param>
    ''' <param name="v_vInsuranceRef"></param>
    ''' <param name="v_vInsFileType"></param>
    ''' <param name="v_vShortName"></param>
    ''' <param name="v_vVehicleRegNo"></param>
    ''' <param name="v_bShowLapsedOnly"></param>
    ''' <param name="v_bLimitResults"></param>
    ''' <param name="v_bShowCurrentPolicyOnly"></param>
    ''' <param name="v_lNumberOfRecords"></param>
    ''' <param name="v_vAgentGroupCnt"></param>
    ''' <param name="v_bAgencyProductOnly"></param>
    ''' <param name="v_bShowCancelledForEvents"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SearchByQuery(ByRef r_vResultArray(,) As Object, Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vInsFileType As Object = Nothing,
                                  Optional ByVal v_vShortName As Object = Nothing, Optional ByVal v_vVehicleRegNo As Object = Nothing,
                                  Optional ByVal v_bShowLapsedOnly As Boolean = False, Optional ByVal v_bLimitResults As Boolean = False,
                                  Optional ByVal v_bShowCurrentPolicyOnly As Boolean = False, Optional ByVal v_lNumberOfRecords As Integer = -1,
                                  Optional ByVal v_vAgentGroupCnt As Object = 0, Optional ByVal v_bAgencyProductOnly As Boolean = False,
                                  Optional ByVal v_bShowCancelledForEvents As Boolean = False, Optional ByVal bRetrieveAssociated As Boolean = False, Optional ByVal v_vAgentCnt As Integer = 0, Optional v_vAgentKey As Integer = 0) As Integer

        Dim nResult As Integer = 0
        Dim sSQL As String = ""
        Dim iParamCount As Integer
        Dim nAgentGroupCnt As Integer
        Dim nAgentKey As Integer
        Dim iParamCondition As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If Not False Then
                nAgentGroupCnt = v_vAgentGroupCnt
            Else
                nAgentGroupCnt = 0
            End If

            If Not False Then
                nAgentKey = v_vAgentKey
            Else
                nAgentKey = 0
            End If

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = ""

            'Modifying the inline query to make it compatible with SQL server 2005

            sSQL = sSQL & "SELECT Insurance_File.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            'SJ 23/02/2004 - start
            If m_bIsUnderwritingBranch Then
                sSQL = sSQL & " case isnull(Insurance_File.alternate_reference,'') when '' then Insurance_file.Insurance_ref else Insurance_file.Alternate_reference end," & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & " Insurance_File.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            'SJ 23/02/2004 - end
            sSQL = sSQL & " Insurance_Folder.description insurance_folder_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_type.code type_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_System.last_modified," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_Folder.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_Folder.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.product_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Product.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            ' TF311298 - reinstated for last modified column
            sSQL = sSQL & " Insurance_File_System.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
            'RWH(25/05/01) Added Insurance_File_Status
            sSQL = sSQL & " Insurance_File_Status.Description," & Strings.ChrW(13) & Strings.ChrW(10)
            'SJ 19/04/2004 - start
            sSQL = sSQL & " Policy_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " alternate_reference," & Strings.ChrW(13) & Strings.ChrW(10)
            If m_bUnderwritingBranchEnabled Then
                sSQL = sSQL & " Source.underwriting_branch_ind," & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "0,"
            End If
            sSQL = sSQL & " Product.is_renewable," & Strings.ChrW(13) & Strings.ChrW(10) ' PN 78589
            sSQL = sSQL & " Insurance_File.is_marketplace_policy," & Strings.ChrW(13) & Strings.ChrW(10)

            If bRetrieveAssociated = True Then

                sSQL = sSQL & "(SELECT P.resolved_name + ' ( ' + AT.description + ')'  as Name  "
                sSQL = sSQL & "FROM insurance_file_associates Associate "
                sSQL = sSQL & "INNER JOIN party P ON Associate.party_cnt = P.party_cnt  "
                sSQL = sSQL & "INNER JOIN Association_Type AT on Associate.Association_Type_id=AT.Association_Type_id  "
                sSQL = sSQL & "Where Associate.Insurance_file_cnt=Insurance_File.insurance_file_cnt And  "
                sSQL = sSQL & "ISNUll(Associate.Is_Deleted,0) <> 1  FOR XML AUTO, TYPE )  As AssociatedClients "
            Else
                sSQL = sSQL & " '' As AssociatedClient "
            End If

            sSQL = sSQL & " FROM Insurance_File" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_Folder" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND Insurance_File.source_id not in (select source_id from PMUser_source where user_id =" & (m_iUserID.ToString) & ")" & Strings.ChrW(13) & Strings.ChrW(10)

            If Not v_bShowLapsedOnly Then

                'SJ 22/04/2004 - end
                sSQL = sSQL & " AND ((Insurance_File.insurance_file_status_id IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " OR (Insurance_File.insurance_file_status_id IN" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " (SELECT insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " FROM Insurance_File_Status" & Strings.ChrW(13) & Strings.ChrW(10)

                If m_lFindMode = 0 Then
                    sSQL = sSQL & " WHERE code = 'REN')))" & Strings.ChrW(13) & Strings.ChrW(10)
                Else
                    If v_bShowCancelledForEvents Then
                        sSQL = sSQL & " WHERE code IN ('REN', 'LAP', 'REP','CAN','REPBDMTA'))))" & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & " WHERE code IN ('REN', 'LAP', 'REP','REPBDMTA'))))" & Strings.ChrW(13) & Strings.ChrW(10)
                    End If
                End If
                'SJ 22/04/2004 - start
            Else
                'Only show the latest version of the policy and then only when it is lapsed
                sSQL = sSQL & " AND (Insurance_File.insurance_file_status_id IN" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " (SELECT insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " FROM Insurance_File_Status" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " WHERE code = 'LAP'))" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " AND insurance_file.insurance_file_cnt = (SELECT MAX(i2.insurance_file_cnt) FROM" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " insurance_file i2 WHERE i2.insurance_folder_cnt = insurance_file.insurance_folder_cnt)" & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            'SJ 22/04/2004 - end
            sSQL = sSQL & " AND Insurance_file.policy_ignore IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)

            'append the parameters
            iParamCount = 1

            If Not informations.IsNothing(v_vInsuranceRef) Then
                If (v_vInsuranceRef <> "") And (v_vInsuranceRef <> "%") Then
                    'Policy Ref is present
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND" & Strings.ChrW(13) & Strings.ChrW(10)
                    End If
                    If (Not informations.IsNothing(v_vInsFileType) AndAlso v_vInsFileType.Trim().ToUpper() = "RENEWAL") Then
                        sSQL = sSQL & " Insurance_File.insurance_ref Like  '" & v_vInsuranceRef.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        If m_bIsUnderwritingBranch Then
                            sSQL = sSQL & " (Insurance_File.insurance_ref Like '" & v_vInsuranceRef.Trim() & "%' " & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & " OR (Insurance_File.insurance_file_cnt = (SELECT  renewal_insurance_file_cnt FROM" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & "  renewal_status WHERE insurance_file_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & " (SELECT insurance_file_cnt FROM insurance_file WHERE insurance_folder_cnt =" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & " (SELECT TOP 1 insurance_folder_cnt FROM insurance_file WHERE insurance_ref Like '" & v_vInsuranceRef.Trim() & "%' "
                            sSQL = sSQL & " OR Insurance_file.alternate_reference LIKE '" & v_vInsuranceRef.Trim() & "%') " & Strings.ChrW(13) & Strings.ChrW(10)
                        Else
                            sSQL = sSQL & " Insurance_file_cnt in ( " & Strings.ChrW(13) & Strings.ChrW(10)
                             If v_vAgentKey <> 0 Then
                                sSQL = sSQL & " SELECT Insurance_file_cnt FROM Insurance_File WHERE Insurance_File.insurance_ref Like '" & v_vInsuranceRef.Trim() & "%' AND lead_agent_cnt = " & v_vAgentKey
                            Else
                                sSQL = sSQL & " SELECT Insurance_file_cnt FROM Insurance_File WHERE Insurance_File.insurance_ref Like '" & v_vInsuranceRef.Trim() & "%' "
                            End If
                            sSQL = sSQL & " UNION " & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & " (SELECT  renewal_insurance_file_cnt FROM renewal_status WHERE insurance_file_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & " (SELECT insurance_file_cnt FROM insurance_file WHERE insurance_folder_cnt =" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & " (SELECT TOP 1 insurance_folder_cnt FROM insurance_file WHERE insurance_ref Like '" & v_vInsuranceRef.Trim() & "%')))) "
                        End If
                    End If
                End If
            End If
            sSQL = sSQL & " INNER JOIN Party" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

            If Not informations.IsNothing(v_vShortName) Then
                If v_vShortName <> "" Then
                    'Policy Holder is present
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND"
                    End If
                    sSQL = sSQL & " (Insurance_Folder.insurance_holder_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " (SELECT Party.party_cnt FROM Party" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " WHERE Party.party_cnt = Insurance_Folder.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " AND Party.shortname Like '" & v_vShortName.Trim() & "') " & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " OR Party.party_cnt IN (select ifa.party_cnt from Insurance_file_associates ifa where Insurance_file_cnt = insurance_file.insurance_file_cnt and ISNULL(Is_Deleted,0) <> 1 )) " & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            sSQL = sSQL & " INNER JOIN Insurance_File_System" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_File_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)

            ' TF311298
            If Not informations.IsNothing(v_vInsFileType) Then
                If (v_vInsFileType <> "") And (v_vInsFileType <> "%") Then
                    'Ins File Type is present
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " AND" & Strings.ChrW(13) & Strings.ChrW(10)
                    End If

                    If v_vInsFileType.Trim().ToUpper() = "POLICY" Then
                        If m_lFindMode = 1 Then
                            sSQL = sSQL & "(Insurance_File.insurance_file_cnt = (SELECT TOP 1 ifi.insurance_file_cnt FROM Insurance_File ifi "
                            sSQL = sSQL & "WHERE ifi.insurance_folder_cnt = Insurance_File.insurance_folder_cnt "
                            If v_bShowCancelledForEvents Then
                                sSQL = sSQL & "AND ifi.insurance_file_type_id IN (2,5,9,8) AND ISNULL(ifi.out_of_sequence_replaced, 0) = 0"
                            Else
                                sSQL = sSQL & "AND ifi.insurance_file_type_id IN (2,5,9) AND ISNULL(ifi.out_of_sequence_replaced, 0) = 0"
                            End If
                            If Not informations.IsNothing(v_vInsuranceRef) Then
                                sSQL = sSQL & " AND ifi.insurance_ref Like '" & v_vInsuranceRef.Trim() & "%'"
                            End If

                            sSQL = sSQL & " ORDER BY ifi.inception_date_tpi DESC,ifi.insurance_file_cnt DESC))" & Strings.ChrW(13) & Strings.ChrW(10)

                        Else
                            If v_bShowCurrentPolicyOnly Then
                                sSQL = sSQL & "(Insurance_File.insurance_file_cnt = (SELECT TOP 1 ifi.insurance_file_cnt FROM Insurance_File ifi "
                                sSQL = sSQL & "WHERE ifi.insurance_folder_cnt = Insurance_File.insurance_folder_cnt "
                                sSQL = sSQL & "AND ifi.insurance_file_type_id IN (2,5,9) ORDER BY ifi.inception_date_tpi DESC,ifi.insurance_file_cnt DESC))" & Strings.ChrW(13) & Strings.ChrW(10)
                                sSQL = sSQL & " AND" & Strings.ChrW(13) & Strings.ChrW(10)
                            End If
                            sSQL = sSQL & " ((Insurance_File_Type.code LIKE '" & v_vInsFileType.Trim() & "')" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & " OR (Insurance_File_Type.code LIKE 'MTA PERM')" & Strings.ChrW(13) & Strings.ChrW(10)
                            sSQL = sSQL & " OR (Insurance_File_Type.code LIKE 'MTA TEMP')" & Strings.ChrW(13) & Strings.ChrW(10)
                            'WPR 33-75 added
                            sSQL = sSQL & " OR (Insurance_File_Type.code LIKE 'MTAREINS'))" & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                    ElseIf (v_vInsFileType.Trim().ToUpper() = "ALLQUOTE") Then
                        sSQL = sSQL & " Insurance_File_Type.code IN('QUOTE','RENEWAL','MTAQUOTE','MTAQTETEMP','MTAQREINS')" & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        sSQL = sSQL & " Insurance_File_Type.code LIKE '" & v_vInsFileType.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    End If
                End If
            End If

            sSQL = sSQL & " INNER JOIN Product" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Product.product_id = Insurance_File.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            If (informations.IsNothing(v_bAgencyProductOnly) = False) Then
                If v_bAgencyProductOnly Then
                    sSQL = sSQL & " AND (insurance_file.product_id IN (select product_id from party_agent_product pap" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "INNER JOIN PMUser pmu ON pmu.party_cnt = pap.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "WHERE pmu.user_id = " & m_iUserID & ") OR NOT EXISTS (Select Party_cnt From pmuser where user_id =  " & m_iUserID & " AND party_cnt IS NOT NULL))" & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If
            sSQL = sSQL & " INNER JOIN PMCaption" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)

            'SJ 23/02/2004 - start
            If m_bUnderwritingBranchEnabled Then
                sSQL = sSQL & " INNER JOIN Source" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " ON insurance_file.source_id = source.source_id " & Strings.ChrW(13) & Strings.ChrW(10)
                If m_bIsUnderwritingBranch Then
                    sSQL = sSQL & " AND (source.underwriting_branch_ind = 1 OR insurance_file.policy_type_id = 3 OR insurance_file.source_id = " & CStr(m_iSourceID) & ")" & Strings.ChrW(13) & Strings.ChrW(10)
                Else
                    sSQL = sSQL & " AND (source.underwriting_branch_ind = 0 OR source.underwriting_branch_ind IS NULL)"
                End If
            End If
            'SJ 23/02/2004 - end

            sSQL = sSQL & " LEFT OUTER JOIN Insurance_File_Status" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_File_Status.insurance_file_status_id = Insurance_File.insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10)

            iParamCondition = 0

            If nAgentGroupCnt <> 0 Then
                'Added filter for the Agent Group Association
                If iParamCondition = 0 Then
                    iParamCondition += 1
                    sSQL = sSQL & " WHERE "
                End If
                sSQL = sSQL & " (Party.Agent_Cnt IN (SELECT party_cnt FROM party_agent  WHERE linked_account_group =" & CStr(nAgentGroupCnt) & ")"
                sSQL = sSQL & " OR Insurance_file.lead_agent_cnt IN (SELECT party_cnt FROM party_agent  WHERE linked_account_group =" & CStr(nAgentGroupCnt) & "))"
            End If

            If v_vAgentCnt <> 0 Then
                If iParamCondition <> 0 Then
                    sSQL = sSQL & " AND "
                Else
                    sSQL = sSQL & " WHERE "
                End If

                sSQL = sSQL & " Insurance_file.lead_agent_cnt = " & CStr(v_vAgentCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            'add the order by clause
            sSQL = sSQL & " ORDER BY Insurance_File.insurance_ref, Insurance_File_System.date_created DESC"

            ' Execute SQL Statement - use array for speed
            With m_oDatabase

                .Parameters.Clear()

                If v_bLimitResults Then
                    If v_lNumberOfRecords = -1 Then
                        m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray)
                    Else
                        m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, lNumberRecords:=v_lNumberOfRecords, vResultArray:=r_vResultArray)
                    End If
                Else
                    m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
                End If

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO records were found return PMFalse
                If Not informations.IsArray(r_vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByQuery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SearchOtherPolicies
    '
    ' Description: Search those policies where client is used as a risk
    '
    ' History: 17/07/2007 Pankaj - Created.
    '
    ' ***************************************************************** '
    Public Function SearchOtherPolicies(Optional ByVal v_vPartyCnt As Object = Nothing, Optional ByRef r_vResultArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bParty As Boolean

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SearhOtherPolices")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            bParty = (Not informations.IsNothing(v_vPartyCnt))

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            If bParty Then
                ' Add the party_cnt

                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACPolListGetOtherPolicies, sSQLName:=ACPolListGetOtherPoliciesName, bStoredProcedure:=ACPolListGetOtherPoliciesStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SearchAll")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: SearchAll
    '
    ' Description: Replacement for SearchAllOld
    '
    ' History: 06/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    'developer guide no. 17
    Public Function SearchAll(ByRef r_vResultArray(,) As Object, Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vInsFileType As Object = Nothing, Optional ByVal v_vShortName As Object = Nothing, Optional ByVal v_vPartyCnt As Object = Nothing, Optional ByVal v_vUserInsurerCnt As Object = 0) As Integer

        Dim result As Integer = 0
        Dim bParty, bAgency As Boolean
        Dim sSQL As String = ""
        Dim sVbsFlag As String = ""

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SearchAll")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' UnderwritingOrAgency
            bAgency = False



            bParty = (Not informations.IsNothing(v_vPartyCnt))

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the new ones
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_file_type", vValue:=v_vInsFileType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If (informations.IsNothing(v_vInsuranceRef)) Or (v_vInsuranceRef = "") Then


                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_ref", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            Else

                ' Insurance_ref
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_ref", vValue:=v_vInsuranceRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bParty Then
                ' Add the party_cnt

                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'sj 03/07/2002 - start
            If Val(v_vUserInsurerCnt) > 0 And bParty And bAgency Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_insurer_cnt", vValue:=v_vUserInsurerCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'CMG/PB 28082002 Only add this parameter if party and agency.
            ElseIf bParty And bAgency Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_insurer_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'Do nothing
            End If
            'sj 03/07/2002 - end



            'DJM 04/03/2004 : These two parameters are now used for Underwriting as well.
            'DD 08/10/2003 - Add current user's branch and
            'the transaction type. This is used for filtering
            'on a Multi-Company setup
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Treat Quotes as MTA Transaction type
            'we don't want to pick up Quotes from a different
            'company if we are running a Multi-Company system
            If v_vInsFileType = "QUOTE" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type", vValue:="G_MTA", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type", vValue:=m_sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call one of the many many sql statements
            If bAgency Then
                If bParty Then
                    sSQL = ACPolListAgencyPartySQL
                Else
                    sSQL = ACPolListAgencySQL
                End If
            Else
                If bParty Then
                    sSQL = ACPolListUWPartySQL
                Else
                    sSQL = ACPolListUWSQL
                End If
            End If

            ' Call the SQL
            'DC070104 PN9388 get ALL records not first 500
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACPolListName, bStoredProcedure:=ACPolListStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SearchAll")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SearchAll")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAllGIIM
    '
    ' Description: SQL Query to Get all GIIM policies
    '              Based on SearchAll
    '
    ' ***************************************************************** '
    Public Function SearchAllGIIM(ByRef r_vResultArray(,) As Object, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lPolicyTypeId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing
        Dim lSchemeTypeFlag As Integer
        Dim sVbsFlag As String = String.Empty
        Dim sQuoteObject As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = ""
            sSQL = sSQL & "SELECT Insurance_File.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_System.last_trans_description insurance_folder_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_type.code type_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_Folder.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_Folder.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.product_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Product.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_System.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_status.code status_code," & Strings.ChrW(13) & Strings.ChrW(10)
            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " Party2.shortname agent_name," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " Insurance_File.this_premium," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.policy_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Policy_Type.description policy_type," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.gemini_policy_status," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - Start adding Risk Type Description
            sSQL = sSQL & " Insurance_File_type.description type_desc," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - End
            sSQL = sSQL & " 'NON EDI'," & Strings.ChrW(13) & Strings.ChrW(10)
            'SJ 06/04/2004 - start
            sSQL = sSQL & "' '," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "' '," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Insurance_File.tax_amount," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "null," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Insurance_File.alternate_reference," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Source.underwriting_branch_ind," & Strings.ChrW(13) & Strings.ChrW(10)
            'SJ 06/04/2004 - end
            Select Case v_lPolicyTypeId
                Case 2
                    sQuoteObject = "giimquick_quote_result"
                Case 4
                    sQuoteObject = "qh_quote_out"
                Case 6
                    sQuoteObject = "giitquick_quote_result"
            End Select

            If v_lPolicyTypeId = 2 Or v_lPolicyTypeId = 4 Or v_lPolicyTypeId = 6 Then
                sSQL = sSQL & "(SELECT MAX(q.stored_ind)" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM gis_policy_link l," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "  " & sQuoteObject & " q" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE Insurance_File.insurance_file_cnt = l.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND l.gis_policy_link_id = q.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND q.stored_ind IS NOT NULL) AS stored_ind" & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "0" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            sSQL = sSQL & " FROM "
            sSQL = sSQL & " Insurance_File INNER JOIN Insurance_File_System ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_Folder ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_File_Type ON  Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " LEFT JOIN Insurance_File_Status ON Insurance_File_Status.insurance_file_status_id = Insurance_File.insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Party ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Product ON Product.product_id = Insurance_File.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN PMCaption ON PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Policy_Type ON Insurance_file.policy_type_id = Policy_Type.policy_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Source ON Source.source_id = Insurance_File.source_id" & Strings.ChrW(13) & Strings.ChrW(10)
            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " LEFT JOIN Party AS Party2 ON Party2.party_cnt = Insurance_File.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10)


            sSQL = sSQL & " WHERE Insurance_file.policy_ignore IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND Insurance_file.policy_type_id = Policy_Type.policy_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND Insurance_file.policy_type_id = " & CStr(v_lPolicyTypeId) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND Party.party_Cnt = " & CStr(v_lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            'IDP 09/04/03 end

            'add the order by clause
            sSQL = sSQL & " ORDER BY Insurance_File_System.date_created DESC"

            ' Execute SQL Statement - use array for speed
            With m_oDatabase

                .Parameters.Clear()

                m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIM")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO records were found return PMFalse
                If Not informations.IsArray(r_vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            'Now let's get the GIS data
            If Not (m_oGISDatabase Is Nothing) Then

                For lRow As Integer = r_vResultArray.GetLowerBound(1) To r_vResultArray.GetUpperBound(1)

                    ' Clear the Database Parameters Collection
                    m_oGISDatabase.Parameters.Clear()

                    ' Add the Insurance File Cnt parameter (INPUT)

                    m_lReturn = m_oGISDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(r_vResultArray(2, lRow)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Parameters.Add 2 failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIM")

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement
                    m_lReturn = m_oGISDatabase.SQLSelect(sSQL:=ACInsRiskDetailsSQL, sSQLName:=ACInsRiskDetailsName, bStoredProcedure:=ACInsRiskDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oGISDatabase.SQLSelect 2 failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIM")

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                    ' Check a number is being processed
                    If informations.IsArray(vArray) Then

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            lSchemeTypeFlag = CInt(vArray(0, 0))

                            'Decode scheme flags
                            m_lReturn = DecodeSchemeFlags(v_lSchemeFlags:=lSchemeTypeFlag, r_sVbsFlag:=sVbsFlag)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sVbsFlag = ""
                            End If
                        Else
                            sVbsFlag = ""
                        End If


                        Select Case sVbsFlag
                            Case "v", "o"
                                ' edi

                                r_vResultArray(24, lRow) = "EDI"

                        End Select

                    End If
                Next lRow

                vArray = Nothing

            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAllGIIM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIM", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAllGIIMPOLICY
    '
    ' Description: SQL Query to Get all GIIM policies
    '              Based on SearchAll
    '
    ' IDP March 2003 - Branch and merge in GII 1.6 Changes
    ' ***************************************************************** '
    Public Function SearchAllGIIMPOLICY(ByRef r_vResultArray(,) As Object, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lPolicyTypeId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vArray(,) As Object = Nothing
        Dim lSchemeTypeFlag As Integer
        Dim sVbsFlag As String = ""
        Dim lLower As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' To improve efficiency 1st retrieve a small subset of rows to feed
            ' into the second query
            sSQL = New StringBuilder(" SELECT " &
                   " insurance_file.insurance_ref, MAX(insurance_file.policy_version) policy_version " &
                   " FROM " &
                   " insurance_file " &
                   " INNER JOIN Insurance_Folder ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt " &
                   " INNER JOIN Party ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt " &
                   " INNER JOIN insurance_file_type IFT on insurance_file.insurance_file_type_id = IFT.insurance_file_type_id " &
                   " WHERE " &
                   " policy_ignore IS NULL " &
                   " AND Insurance_file.policy_type_id = " & CStr(v_lPolicyTypeId) &
                   " AND Party.party_Cnt = " & CStr(v_lPartyCnt) &
                   " AND IFT.code in ('POLICY','MTA PERM')" &
                   " GROUP BY " &
                   " insurance_file.insurance_ref")

            ' Execute SQL Statement - Use array for speed
            With m_oDatabase

                ' Clear parameters
                .Parameters.Clear()

                ' Execute SQL
                m_lError = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

                ' Check error status flag
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIMPOLICY")

                    Return result
                End If

                ' If NO records were found return PMNotFound
                If Not informations.IsArray(r_vResultArray) Then


                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End With

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = New StringBuilder("")
            sSQL.Append(" SELECT ")
            sSQL.Append(" Insurance_File.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File_System.last_trans_description insurance_folder_code," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File_type.code type_code," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_Folder.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_Folder.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.product_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Product.code," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File_System.date_created," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File_status.code status_code," & Strings.ChrW(13) & Strings.ChrW(10))
            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL.Append(" Party2.shortname agent_name," & Strings.ChrW(13) & Strings.ChrW(10))

            sSQL.Append(" Insurance_File.this_premium," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.policy_type_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Policy_Type.description policy_type," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.gemini_policy_status," & Strings.ChrW(13) & Strings.ChrW(10))
            ' MSS250701 - Start adding Risk Type Description
            sSQL.Append(" Insurance_File_Type.description type_desc," & Strings.ChrW(13) & Strings.ChrW(10))
            ' MSS250701 - End
            sSQL.Append(" 'NON EDI'," & Strings.ChrW(13) & Strings.ChrW(10))
            'SJ 13/07/2004 - start
            sSQL.Append("' '," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("' '," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("Insurance_File.tax_amount," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("null," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("Insurance_File.alternate_reference," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("Source.underwriting_branch_ind" & Strings.ChrW(13) & Strings.ChrW(10))
            'SJ 13/07/2004 - end

            sSQL.Append(" FROM ")
            sSQL.Append(" Insurance_File INNER JOIN Insurance_File_System ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Insurance_Folder ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Insurance_File_Type ON  Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" LEFT JOIN Insurance_File_Status ON Insurance_File_Status.insurance_file_status_id = Insurance_File.insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Party ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Product ON Product.product_id = Insurance_File.product_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN PMCaption ON PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Policy_Type ON Insurance_file.policy_type_id = Policy_Type.policy_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Source ON Source.source_id = Insurance_File.source_id" & Strings.ChrW(13) & Strings.ChrW(10))
            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL.Append(" LEFT JOIN Party AS Party2 ON Party2.party_cnt = Insurance_File.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10))


            ' Add the where clause
            sSQL.Append(" WHERE ")

            ' Store the lower bound of the array
            lLower = r_vResultArray.GetLowerBound(1)

            For lRow As Integer = lLower To r_vResultArray.GetUpperBound(1)

                ' Add an 'OR' statement if required
                If lRow > lLower Then

                    sSQL.Append(" OR ")
                End If

                ' Append restriction criteria

                sSQL.Append(" (insurance_file.insurance_ref = '" & CStr(r_vResultArray(0, lRow)) & "'")

                sSQL.Append(" AND insurance_file.policy_version = " & CStr(r_vResultArray(1, lRow)) & ") " & Strings.ChrW(13) & Strings.ChrW(10))
            Next  'lRow

            ' 27/06/00 - Rrestrict to party cnt (gets past bug with duplicate temp ref no's produced for default policies)
            sSQL.Append(" AND Party.party_Cnt = " & v_lPartyCnt)

            ' Add the order by clause
            sSQL.Append(" ORDER BY Insurance_File_System.date_created DESC")

            ' Clear the array from previouse query
            r_vResultArray = Nothing

            ' Execute SQL Statement - use array for speed
            With m_oDatabase

                ' Clear Parameters
                .Parameters.Clear()

                m_lError = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

                ' Check error status
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIMPOLICY")

                    Return result
                End If

                ' If NO records were found return PMFalse
                If Not informations.IsArray(r_vResultArray) Then


                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End With

            'Now let's get the GIS data
            If Not (m_oGISDatabase Is Nothing) Then

                For lRow As Integer = r_vResultArray.GetLowerBound(1) To r_vResultArray.GetUpperBound(1)

                    ' Clear the Database Parameters Collection
                    m_oGISDatabase.Parameters.Clear()

                    ' Add the Insurance File Cnt parameter (INPUT)

                    m_lReturn = m_oGISDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(r_vResultArray(2, lRow)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Parameters.Add 2 failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIMPOLICY")

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement
                    m_lReturn = m_oGISDatabase.SQLSelect(sSQL:=ACInsRiskDetailsSQL, sSQLName:=ACInsRiskDetailsName, bStoredProcedure:=ACInsRiskDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oGISDatabase.SQLSelect 2 failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIMPOLICY")

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                    ' Check a number is being processed
                    If informations.IsArray(vArray) Then

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            lSchemeTypeFlag = CInt(vArray(0, 0))

                            'Decode scheme flags
                            m_lReturn = DecodeSchemeFlags(v_lSchemeFlags:=lSchemeTypeFlag, r_sVbsFlag:=sVbsFlag)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sVbsFlag = ""
                            End If
                        Else
                            sVbsFlag = ""
                        End If


                        Select Case sVbsFlag
                            Case "v", "o"
                                ' edi

                                r_vResultArray(24, lRow) = "EDI"

                        End Select

                    End If
                Next lRow

                vArray = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAllGIIMPOLICY Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIMPOLICY", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAllByType
    '
    ' Description: SQL Query to Get all policies
    '              Based on SearchAll
    '
    ' ***************************************************************** '
    Public Function SearchAllByType(ByRef r_vResultArray(,) As Object, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_sPolicyType As String = "", Optional ByVal v_IFSTInsuranceFileType As InsuranceFileSearchType = 0, Optional ByVal v_bIncludeLapsedAndCancelled As Boolean = False) As Integer

        'RJG 21/06/2000 - Changed variable type of PolicyType to string so we can pass a
        ' code rather than an ID and also added Insurance File Type Enum.

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim sVbsFlag As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = ""

            'Modifying the inline query to make it compatible with SQL server 2005

            sSQL = sSQL & "SELECT Insurance_File.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_System.last_trans_description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_type.code type_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.cover_start_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_Folder.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_Folder.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.product_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Product.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_System.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
            If v_bIncludeLapsedAndCancelled Then
                sSQL = sSQL & " Insurance_File_status.code status_code," & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & " Null status_code," & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " Party2.shortname agent_name," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " Insurance_File.this_premium," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.policy_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            '    sSQL = sSQL & " Policy_Type.code policy_type," & vbCrLf
            sSQL = sSQL & " Policy_Type.description policy_type," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.gemini_policy_status," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - Start adding Risk Type Description
            'sSQL = sSQL & " Insurance_File_type.description type_desc," & vbCrLf
            ' MSS250701 - End
            sSQL = sSQL & " ''," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " party_personal_client.forename," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.expiry_date" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " FROM Insurance_File" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_Folder" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            If v_lPartyCnt = 0 Then
                sSQL = sSQL & " AND Insurance_File.insurance_file_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " (SELECT Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " FROM Insurance_File INNER JOIN  Insurance_folder" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " ON Insurance_File.insurance_folder_cnt = Insurance_Folder.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " WHERE policy_ignore IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & " AND Insurance_File.insurance_file_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " (SELECT Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " FROM Insurance_File INNER JOIN Insurance_folder" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " ON Insurance_File.insurance_folder_cnt = Insurance_Folder.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " WHERE Insurance_Folder.insurance_holder_cnt = " & CStr(v_lPartyCnt) & " " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " AND policy_ignore IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            sSQL = sSQL & " INNER JOIN Party" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN party_personal_client" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON party_personal_client.party_cnt = party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_File_System" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_File_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id " & Strings.ChrW(13) & Strings.ChrW(10)

            'RJG 21/06/2000 - If we have defined a particular insurance file type then add the SQL.
            Select Case v_IFSTInsuranceFileType
                Case InsuranceFileSearchType.IFSTQuote
                    sSQL = sSQL & " AND Insurance_File_Type.code = 'QUOTE'" & Strings.ChrW(13) & Strings.ChrW(10)
                Case InsuranceFileSearchType.IFSTPolicy
                    sSQL = sSQL & " AND (Insurance_File_Type.code = 'POLICY' OR Insurance_File_Type.code = 'MTA PERM' OR Insurance_File_Type.code = 'MTAREINS')" & Strings.ChrW(13) & Strings.ChrW(10)
                Case InsuranceFileSearchType.IFSTQuotePolicy
                    sSQL = sSQL & " AND (Insurance_File_Type.code = 'QUOTE' OR Insurance_File_Type.code = 'POLICY')" & Strings.ChrW(13) & Strings.ChrW(10)
                Case InsuranceFileSearchType.IFSTRenewal
                    sSQL = sSQL & " AND Insurance_File_Type.code = 'RENEWAL'" & Strings.ChrW(13) & Strings.ChrW(10)
                Case InsuranceFileSearchType.IFSTQuotePolicyRenewal
                    sSQL = sSQL & " AND (Insurance_File_Type.code = 'QUOTE' OR Insurance_File_Type.code = 'POLICY'" &
                           " OR Insurance_File_Type.code = 'MTAREINS' OR Insurance_File_Type.code = 'RENEWAL')" & Strings.ChrW(13) & Strings.ChrW(10)
                Case InsuranceFileSearchType.IFSTMTAQuote
                    sSQL = sSQL & " AND Insurance_File_Type.code = 'MTAQUOTE'" & Strings.ChrW(13) & Strings.ChrW(10)
                Case InsuranceFileSearchType.IFSTMTAQuoteMTATempQuote
                    sSQL = sSQL & " AND (Insurance_File_Type.code = 'MTAQUOTE' OR Insurance_File_Type.code = 'MTAQTETEMP')" & Strings.ChrW(13) & Strings.ChrW(10)
                Case InsuranceFileSearchType.IFSTQuoteQteCan
                    sSQL = sSQL & " AND (Insurance_File_Type.code = 'QUOTE' OR Insurance_File_Type.code = 'MTAQTECAN')" & Strings.ChrW(13) & Strings.ChrW(10)
            End Select

            sSQL = sSQL & " INNER JOIN Product" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Product.product_id = Insurance_File.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN PMCaption" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Policy_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_file.policy_type_id = Policy_Type.policy_type_id" & Strings.ChrW(13) & Strings.ChrW(10)

            If v_sPolicyType <> "" Then
                'RJG 21/06/2000 - Match Policy types by code and not an ID
                'sSQL = sSQL & " AND Insurance_file.policy_type_id = " & v_lPolicyType & vbCrLf
                sSQL = sSQL & " AND Policy_Type.code = '" & v_sPolicyType & "'" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            If v_bIncludeLapsedAndCancelled Then
                sSQL = sSQL & " LEFT OUTER JOIN Insurance_File_Status" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " ON Insurance_File_Status.insurance_file_status_id = Insurance_File.insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & " AND Insurance_File.insurance_file_status_id IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            sSQL = sSQL & " LEFT OUTER JOIN Party Party2" & Strings.ChrW(13) & Strings.ChrW(10)
            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " ON Party2.party_cnt = Insurance_File.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10)


            'add the order by clause
            sSQL = sSQL & " ORDER BY Insurance_File.insurance_ref, Insurance_File.Insurance_Folder_Cnt, Insurance_File.insurance_file_cnt , Insurance_File.cover_start_date"


            ' Execute SQL Statement - use array for speed
            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllByType")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMFalse
            If Not informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAllByType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllByType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAllByProductId
    '
    ' Description: SQL Query to Get all policies
    '              Based on SearchAll
    '
    ' ***************************************************************** '
    Public Function SearchAllByProductId(ByRef r_vResultArray(,) As Object, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lProductId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim sVbsFlag As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = ""

            'Modifying the inline query to make it compatible with SQL server 2005

            sSQL = sSQL & "SELECT Insurance_File.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_System.last_trans_description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_type.code type_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10)

            'TN20010417 Start

            sSQL = sSQL & " Insurance_File.renewal_date," & Strings.ChrW(13) & Strings.ChrW(10)

            'TN20010417 End

            sSQL = sSQL & " Insurance_Folder.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_Folder.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.product_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Product.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File_System.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Null status_code," & Strings.ChrW(13) & Strings.ChrW(10)
            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " Party2.shortname agent_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.this_premium," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.policy_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Policy_Type.description policy_type," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Insurance_File.gemini_policy_status," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - Start adding Risk Type Description
            sSQL = sSQL & " Insurance_File_type.description type_desc," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - End
            sSQL = sSQL & " ''," & Strings.ChrW(13) & Strings.ChrW(10)

            'Tomo270901
            'Don't force link to personal client if Underwriting.  Not that I can think
            'of a reason for Broking to keep it but it's not my place etc...

            sSQL = sSQL & " ''," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " Insurance_File.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW251102 - add tax amount
            ' PS411
            sSQL = sSQL & " Insurance_File.tax_amount" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " FROM Insurance_File" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_Folder" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND Insurance_File.insurance_file_status_id IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND Insurance_File.policy_type_id = " & CStr(PMBConst.PMBPolicyTypeUnderwriting) & Strings.ChrW(13) & Strings.ChrW(10)
            'Thinh Nguyen 22/01/2001 (start)
            If v_lPartyCnt <> 0 Then
                sSQL = sSQL & " AND Insurance_File.insurance_file_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " (SELECT Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " FROM Insurance_File INNER JOIN Insurance_folder" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " ON Insurance_File.insurance_folder_cnt = Insurance_Folder.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " WHERE Insurance_Folder.insurance_holder_cnt =" & CStr(v_lPartyCnt) & " " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " AND policy_ignore IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            'Thinh Nguyen 22/01/2001 (end)

            'Thinh Nguyen 22/01/2002 (start)
            If v_lProductId <> 0 Then
                'Tomo220501
                'sSQL = sSQL & " AND Insurance_File.product_id = " & v_lProductId & vbCrLf
                sSQL = sSQL & " AND Insurance_File.product_id IN" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " (SELECT  DISTINCT(p1.product_id)" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " FROM product_risk_type_group p1 INNER JOIN product_risk_type_group p2" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " ON p1.risk_type_group_id = p2.risk_type_group_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " WHERE p2.product_id =" & CStr(v_lProductId) & ")" & Strings.ChrW(13) & Strings.ChrW(10)
                'Tomo220501
            End If
            'Thinh Nguyen 22/01/2002 (end)

            sSQL = sSQL & " INNER JOIN Party" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)


            sSQL = sSQL & " INNER JOIN Insurance_File_System" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_File_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Product" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Product.product_id = Insurance_File.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN PMCaption" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Policy_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Insurance_file.policy_type_id = Policy_Type.policy_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " LEFT OUTER JOIN Party Party2" & Strings.ChrW(13) & Strings.ChrW(10)

            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " ON Party2.party_cnt = Insurance_File.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10)


            'add the order by clause
            sSQL = sSQL & " ORDER BY Insurance_File.insurance_ref, Insurance_File.Insurance_Folder_Cnt, Insurance_File.insurance_file_cnt , Insurance_File.cover_start_date"


            ' Execute SQL Statement - use array for speed
            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllByProductId")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMFalse
            If Not informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAllByProductId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllByProductId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAllPMUQuotes
    '
    ' Description: SQL Query to Get all PMU policies that are quotes
    '              Based on SearchAll
    '
    ' ***************************************************************** '
    Public Function SearchAllPMUQuotes(ByRef r_vResultArray(,) As Object, Optional ByVal v_lPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = ""

            'Modifying the inline query to make it compatible with SQL server 2005

            sSQL = sSQL & "SELECT ifi.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifs.last_trans_description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ift.code type_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            'TN20010417 Start
            'sSQL = sSQL & " ifi.cover_start_date," & vbCrLf
            sSQL = sSQL & " ifi.renewal_date," & Strings.ChrW(13) & Strings.ChrW(10)
            'TN20010417 End
            sSQL = sSQL & " ifo.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifo.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.product_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Product.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifs.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Null status_code," & Strings.ChrW(13) & Strings.ChrW(10)
            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " Party2.shortname agent_name," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " ifi.this_premium," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.policy_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Policy_Type.description policy_type," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.gemini_policy_status," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - Start adding Risk Type Description
            sSQL = sSQL & " ift.description type_desc," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - End
            sSQL = sSQL & " ''," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.insurance_file_type_id," & Strings.ChrW(13) & Strings.ChrW(10) 'was party personal client.forename
            sSQL = sSQL & " ifi.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW251102 - add tax amount
            ' PS411
            ' CTAF 20030620 - Correct syntax in SQL, should be ifi. not Insurance_File.
            sSQL = sSQL & " ifi.tax_amount" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " FROM Insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_Folder ifo" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND ifi.insurance_file_status_id IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)

            'Thinh Nguyen 22/01/2002 (start)
            If v_lPartyCnt <> 0 Then
                sSQL = sSQL & " AND ifi.insurance_file_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " (SELECT ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " FROM insurance_file ifi, Insurance_folder ifo" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " WHERE ifo.insurance_holder_cnt = " & CStr(v_lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " AND ifi.insurance_folder_cnt = ifo.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " AND policy_ignore IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            'Thinh Nguyen 22/01/2002 (end)

            sSQL = sSQL & " INNER JOIN Party" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Party.party_cnt = ifo.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_file_System ifs" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifs.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_file_Type ift" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ift.insurance_file_type_id = ifi.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND ift.insurance_file_type_id = 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Product" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Product.product_id = ifi.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN PMCaption" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Policy_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifi.policy_type_id = Policy_Type.policy_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND ifi.policy_type_id = " & CStr(PMBConst.PMBPolicyTypeUnderwriting) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " LEFT OUTER JOIN Party Party2" & Strings.ChrW(13) & Strings.ChrW(10)

            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " ON Party2.party_cnt = ifi.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10)


            sSQL = sSQL & " ORDER BY ifi.insurance_ref, ifi.Insurance_Folder_Cnt, ifi.insurance_file_cnt , ifi.cover_start_date"


            ' Execute SQL Statement - use array for speed
            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllPMUQuotes")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMFalse
            If Not informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAllPMUQuotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllPMUQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAllPMUMTAs
    '
    ' Description: SQL Query to Get all PMU policies that are quotes
    '              Based on SearchAll
    '
    ' ***************************************************************** '
    Public Function SearchAllPMUMTAs(ByRef r_vResultArray(,) As Object, Optional ByVal v_lPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = ""

            'Modifying the inline query to make it compatible with SQL server 2005

            sSQL = sSQL & "SELECT ifi.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifs.last_trans_description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ift.code type_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            'TN20010417 Start
            'sSQL = sSQL & " ifi.cover_start_date," & vbCrLf
            sSQL = sSQL & " ifi.renewal_date," & Strings.ChrW(13) & Strings.ChrW(10)
            'TN20010417 End
            sSQL = sSQL & " ifo.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifo.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.product_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Product.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifs.date_created," & Strings.ChrW(13) & Strings.ChrW(10)

            '    sSQL = sSQL & " null status_code," & vbCrLf
            sSQL = sSQL & " ifstat.code status_code," & Strings.ChrW(13) & Strings.ChrW(10)

            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " Party2.shortname agent_name," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " ifi.this_premium," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.policy_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Policy_Type.description policy_type," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.gemini_policy_status," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - Start adding Risk Type Description
            'sSQL = sSQL & " ift.description type_desc," & vbCrLf
            ' MSS250701 - End
            sSQL = sSQL & " ''," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.insurance_file_type_id," & Strings.ChrW(13) & Strings.ChrW(10) 'was party personal client.forename
            sSQL = sSQL & " ifi.expiry_date" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " FROM Insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_Folder ifo" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND ifi.policy_type_id = " & CStr(PMBConst.PMBPolicyTypeUnderwriting) & Strings.ChrW(13) & Strings.ChrW(10)
            'Include policies under renewal or lapsed...
            sSQL = sSQL & " AND (ifi.insurance_file_status_id IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " OR (ifi.insurance_file_status_id IN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " (SELECT insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " FROM Insurance_File_Status" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " WHERE code IN ('REN'))))" & Strings.ChrW(13) & Strings.ChrW(10)

            If v_lPartyCnt <> 0 Then
                sSQL = sSQL & " AND ifi.insurance_file_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " (SELECT ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " FROM insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " INNER JOIN Insurance_folder ifo" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " ON  ifi.insurance_folder_cnt = ifo.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " WHERE ifo.insurance_holder_cnt =" & CStr(v_lPartyCnt) & " " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " AND policy_ignore IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            sSQL = sSQL & " INNER JOIN Party" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Party.party_cnt = ifo.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_file_System ifs" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifs.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_file_Type ift" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ift.insurance_file_type_id = ifi.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            'TN20010417 Start
            sSQL = sSQL & " AND ift.insurance_file_type_id IN (2)" & Strings.ChrW(13) & Strings.ChrW(10)
            'TN20010417 End
            sSQL = sSQL & " INNER JOIN Product" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Product.product_id = ifi.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN PMCaption" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Policy_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifi.policy_type_id = Policy_Type.policy_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " LEFT OUTER JOIN Insurance_file_Status ifstat" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifi.insurance_file_status_id = ifstat.insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " LEFT OUTER JOIN Party Party2" & Strings.ChrW(13) & Strings.ChrW(10)

            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL = sSQL & " ON Party2.party_cnt = ifi.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10)


            'after restructuring, there is not condition to be checked in where clause
            'add the order by clause
            sSQL = sSQL & " ORDER BY ifi.insurance_ref, ifi.cover_start_date"

            ' Execute SQL Statement - use array for speed
            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllPMUMTAs")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMFalse
            If Not informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAllPMUMTAs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllPMUMTAs", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAllPMUEditable
    '
    ' Description: SQL Query to Get all PMU policies that are editable
    '              Based on SearchAllPMUMTAs
    '
    ' ***************************************************************** '
    Public Function SearchAllPMUEditable(ByRef r_vResultArray(,) As Object, Optional ByVal v_lPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered

            sSQL = ""
            sSQL = sSQL & "CREATE TABLE tempdb..Wobble" & CStr(m_iUserID) & " (insurance_file_cnt int)" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create Temp Table", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = ""
            sSQL = sSQL & "INSERT INTO tempdb..Wobble" & CStr(m_iUserID) & " (insurance_file_cnt)"
            sSQL = sSQL & "SELECT MAX(ifi.insurance_file_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM insurance_file ifi, Insurance_folder ifo" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE ifo.insurance_holder_cnt = " & CStr(v_lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND ifi.insurance_folder_cnt = ifo.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND ifi.policy_ignore IS NULL AND ifi.insurance_file_type_id IN (2,3,5)" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "GROUP BY ifi.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Insert Into Temp Table", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(21/11/01) Replaced join to tempdb..Wobble table with IN and sub query. This
            'speeded the query from 51 seconds to under 1 second.
            sSQL = ""

            'Modifying the inline query to make it compatible with SQL server 2005

            sSQL = sSQL & "SELECT ifi.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifs.last_trans_description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ift.code type_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.renewal_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifo.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifo.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.product_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Product.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifs.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifstat.code status_code," & Strings.ChrW(13) & Strings.ChrW(10)


            sSQL = sSQL & " Party2.shortname agent_name," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " ifi.this_premium," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.policy_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " Policy_Type.description policy_type," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.gemini_policy_status," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - Start adding Risk Type Description
            sSQL = sSQL & " ift.description type_desc," & Strings.ChrW(13) & Strings.ChrW(10)
            ' MSS250701 - End
            sSQL = sSQL & " ''," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.insurance_file_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ifi.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW251102 - add tax amount
            ' PS411
            sSQL = sSQL & " ifi.tax_amount" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " FROM Insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_file_System ifs" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifs.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND ifi.policy_type_id = " & CStr(PMBConst.PMBPolicyTypeUnderwriting) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " AND ifi.insurance_file_cnt IN(SELECT insurance_file_cnt FROM tempdb..Wobble" & CStr(m_iUserID) & ")" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_file_Type ift" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ift.insurance_file_type_id = ifi.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Policy_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifi.policy_type_id = Policy_Type.policy_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Product" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Product.product_id = ifi.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN PMCaption" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Insurance_Folder ifo" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " INNER JOIN Party" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON Party.party_cnt = ifo.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " LEFT OUTER JOIN Insurance_file_Status ifstat" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ON ifi.insurance_file_status_id = ifstat.insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " LEFT OUTER JOIN Party Party2" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " ON Party2.party_cnt = ifi.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10)


            sSQL = sSQL & " ORDER BY ifi.insurance_ref, ifi.cover_start_date"

            ' Execute SQL Statement - use array for speed
            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllPMUEditable")

                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMFalse
            If Not informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            sSQL = ""
            sSQL = sSQL & "DROP TABLE tempdb..Wobble" & CStr(m_iUserID) & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Drop Temp Table", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAllPMUEditable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllPMUEditable", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindLikeRef (Public)
    '
    ' Description: Selects Insurance Files with a reference like the
    '              one supplied.
    '
    ' ***************************************************************** '
    Public Function FindLikeRef(ByRef sInsuranceRef As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sInsFileType As String = ""
            Dim iFindPipeInString As Integer

            iFindPipeInString = (sInsuranceRef.IndexOf("|"c) + 1)

            If iFindPipeInString > 0 Then

                sInsFileType = sInsuranceRef.Substring(iFindPipeInString, sInsuranceRef.Length - iFindPipeInString)
                sInsuranceRef = sInsuranceRef.Substring(0, Math.Min(sInsuranceRef.Length, iFindPipeInString - 1))

            Else

                sInsFileType = "%"

            End If

            If sInsuranceRef = "*" Then
                ' change the code to the sql standard wildcard
                sInsuranceRef = "%"
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Insurance Ref parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_Ref", vValue:=CStr(sInsuranceRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRef")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the SearchType parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_type", vValue:=sInsFileType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRef")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsLikeRefSQL, sSQLName:=ACInsLikeRefName, bStoredProcedure:=ACInsLikeRefStored, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRef")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO Insurance Files were found return Not Found
            If Not informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindSingleRef (Public)
    '
    ' Description: Selects Insurance Files with a reference like the
    '              one supplied.
    '
    ' ***************************************************************** '
    Public Function FindSingleRef(ByVal sInsuranceRef As String, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""

            sSQL = "SELECT Insurance_File_Cnt, Insurance_Folder_Cnt From Insurance_File WHERE Insurance_Ref = '" & sInsuranceRef.Trim() & "'"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="FindSingleRef", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindSingleRef")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO Insurance Files were found return Not Found
            If Not informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindSingleRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindSingleRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function FindQuote(ByRef r_vResultArray(,) As Object, Optional ByVal v_sQuoteRef As String = "", Optional ByVal v_dtCoverStartDate As Object = Nothing, Optional ByVal v_sInsuranceFolderDescription As String = "", Optional ByVal v_lLeadAgentCnt As String = "") As Integer

        Dim result As Integer = 0
        Try

            Dim sSQL, sFormatDate, sFormatDateAddOneDay As String

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT insurance_file.insurance_file_cnt, insurance_file.insurance_ref, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "party.resolved_name, insurance_file.cover_start_date, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "insurance_folder.description, party.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM insurance_file, insurance_folder, party " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE insurance_file.insurance_folder_cnt = insurance_folder.insurance_folder_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND insurance_folder.insurance_holder_cnt = party.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10)


            If Not informations.IsNothing(v_sQuoteRef) Then
                If v_sQuoteRef.Trim() <> "" Then
                    sSQL = sSQL & "AND insurance_file.insurance_ref = '" & v_sQuoteRef & "' " & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If


            If Not informations.IsNothing(v_dtCoverStartDate) Then

                If CStr(v_dtCoverStartDate).Trim() <> "" Then

                    sFormatDate = CDate(v_dtCoverStartDate).ToString("yyyy/MM/dd")

                    sFormatDateAddOneDay = CDate(v_dtCoverStartDate).AddDays(1).ToString("yyyy/MM/dd")
                    sSQL = sSQL & "AND insurance_file.cover_start_date >= '" & sFormatDate & "' " & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND insurance_file.cover_start_date < '" & sFormatDateAddOneDay & "' " & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If


            If Not informations.IsNothing(v_sInsuranceFolderDescription) Then
                If v_sInsuranceFolderDescription.Trim() <> "" Then
                    If (v_sInsuranceFolderDescription.IndexOf("%"c) + 1) = 0 Then
                        v_sInsuranceFolderDescription = v_sInsuranceFolderDescription.Trim() & "%"
                    End If
                    sSQL = sSQL & "AND insurance_folder.description LIKE '" & v_sInsuranceFolderDescription & "' " & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If


            If Not informations.IsNothing(v_lLeadAgentCnt) Then
                If ToSafeDouble(v_lLeadAgentCnt) <> 0 And v_lLeadAgentCnt <> "" Then
                    sSQL = sSQL & "AND insurance_file.lead_agent_cnt = " & v_lLeadAgentCnt & " " & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACFindQuoteName, bStoredProcedure:=ACFindQuoteStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindQuote")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO Insurance Files were found return Not Found
            If Not informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindLikeRefAndHolder (Public)
    '
    ' Description: Selects Insurance Files with a reference like the
    '              one supplied and equal to Insurance Holder ID
    '
    ' ***************************************************************** '
    Public Function FindLikeRefAndHolder(ByRef sInsuranceRef As String, ByRef lInsuranceHolderCnt As Integer, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sInsFileType As String = ""
            Dim iFindPipeInString As Integer

            iFindPipeInString = (sInsuranceRef.IndexOf("|"c) + 1)

            If iFindPipeInString > 0 Then

                sInsFileType = sInsuranceRef.Substring(iFindPipeInString, sInsuranceRef.Length - iFindPipeInString)
                sInsuranceRef = sInsuranceRef.Substring(0, Math.Min(sInsuranceRef.Length, iFindPipeInString - 1))

            Else

                sInsFileType = "%"

            End If


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Insurance Ref parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_Ref", vValue:=CStr(sInsuranceRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRefAndHolder")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Policy Holder ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_holder_cnt", vValue:=CStr(lInsuranceHolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRefAndHolder")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the SearchType parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_type", vValue:=sInsFileType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRefAndHolder")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsLikeRefHolderSQL, sSQLName:=ACInsLikeRefHolderName, bStoredProcedure:=ACInsLikeRefHolderStored, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRefAndHolder")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO Insurance Files were found return Not Found
            If Not informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeRefAndHolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRefAndHolder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindLikeVehicle (Public)
    '
    ' Description: Selects Insurance Files with a vehicle like the
    '              one supplied.
    '
    ' Edit History :
    ' ED 05082002  : Added bypass code for Reg for Vehicle Registration
    '                Number Search
    ' ***************************************************************** '
    Public Function FindLikeVehicle(ByRef sRegistration As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        Dim vArray2(,) As Object = Nothing
        Dim lKey, lSource, lId, lNumberRecords As Integer
        Dim sString As New StringBuilder
        'Link to Gemini
        Dim m_oVehicle As Object = Nothing
        'EK 140199 Bug 206 added Component services to convert keys

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bGeminiIILink Then
                m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID,
                    v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFGeminiII,
                    r_oDatabase:=m_oGISDatabase)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    FindLikeVehicle = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                'TF210802 - Move this down as only GII will be merged anyway (?)
                'End If

                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oVehicle,
                                                                    v_sClassName:="bSirToGemVehicle.Business",
                                                                    v_sCallingAppName:=ACApp,
                                                                    v_sUsername:=m_sUsername,
                                                                    v_sPassword:=m_sPassword,
                                                                    v_iUserID:=m_iUserID,
                                                                    v_iSourceID:=m_iSourceID,
                                                                    v_iLanguageID:=m_iLanguageID,
                                                                    v_iCurrencyID:=m_iCurrencyID,
                                                                    v_iLogLevel:=m_iLogLevel)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    m_bGeminiLink = False
                    Exit Function
                End If


                m_bGeminiLink = m_oVehicle.PMGeminiLink

                If (m_bGeminiLink = False) Then
                    m_oVehicle.Dispose()
                    m_oVehicle = Nothing
                End If
            End If





            If sRegistration = "" Then
                sRegistration = "%"
            Else
                If Not sRegistration.EndsWith("%") Then
                    sRegistration = sRegistration & "%"
                End If
            End If

            ' ED 05082002 : Added bypass code for Vehicle Registration
            '               Number Search
            If AllowRegSearch() Then


                m_lReturn = FindLikeIndex(sIndex:=sRegistration, lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=vResultArray, iFileType:=m_iFileType)

            Else

                m_lReturn = m_oVehicle.SelectVehicle(sRegistration:=ToString(sRegistration), vArray:=CType(vArray, Object(,)))

                'So we've got this array, which has insurance_file_cnt and registration number

                'Or do we?  Nothing found...
                If Not Informations.IsArray(vArray) Then

                    vResultArray = Nothing
                    Return result
                End If

                'Let's build up the list of numbers to get...

                sString = New StringBuilder("")

                'EK 140199


                For lRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    lKey = CInt(vArray(0, lRow))
                    'EK 140199

                    m_lReturn = UncalcCombinedKey(lSource, lId, lKey)
                    '       lSource = lKey \ 2 ^ 21
                    '       lId = lKey - (lSource * 2 ^ 21)

                    sString.Append(StringsHelper.Format(lId, "0000000"))

                    vArray(0, lRow) = lId
                Next lRow
                'EK 140199

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="source", vValue:=CStr(lSource), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="list", vValue:=CStr(sString.ToString()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsLikeRegistrationSQL, sSQLName:=ACInsLikeRegistrationName, bStoredProcedure:=ACInsLikeRegistrationStored, lNumberRecords:=lNumberRecords, vResultArray:=vArray2)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    vArray = Nothing

                    vResultArray = Nothing
                    Return result
                End If

                If Not Informations.IsArray(vArray2) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    vArray = Nothing

                    vResultArray = Nothing
                    Return result
                End If

                'Right, so we've got this array, which we've now got to put in vResultArray,
                'and add the registration number as well...


                ReDim vResultArray(vArray2.GetUpperBound(0) + 1, vArray2.GetUpperBound(1))


                For lRow As Integer = vArray2.GetLowerBound(1) To vArray2.GetUpperBound(1)
                    'Move in the right stuff

                    For lRow2 As Integer = vArray2.GetLowerBound(0) To vArray2.GetUpperBound(0)


                        vResultArray(lRow2, lRow) = vArray2(lRow2, lRow)
                    Next lRow2
                    'Get the registration number

                    For lRow2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                        If CInt(vResultArray(ACIInsFileId, lRow)) = CDbl(vArray(0, lRow2)) Then


                            vResultArray(ACIRegistration, lRow) = vArray(1, lRow2)
                        End If
                    Next lRow2
                Next lRow

                vArray = Nothing
                vArray2 = Nothing
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeVehicle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeVehicle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: FindLikeIndex (Public)
    '
    ' Description: Selects Index Description from the value supplied
    '
    ' Edit History  : 1
    ' Author        : Ram Chandrabose
    ' Date          : 05-01-2001
    ' Description   : Added code to Use the GIS Search Property Find
    '                 Stored Procedure (Commented the old codes )
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function FindLikeIndex(ByRef sIndex As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional ByRef lSpecificDataModelIndex As Integer = 1,
                                  Optional ByRef sSearchType As String = "Like", Optional ByRef lSpecificFieldType As Integer = 0, Optional ByVal v_vAgentGroupCnt As Object = 0,
                                  Optional ByVal v_bAgencyProductOnly As Boolean = False, Optional ByRef iFileType As Integer = 0, Optional ByVal bRetrieveAssociates As Boolean = False) As Integer
        Dim nResult As Integer = 0
        Dim sSQL As String = ""

        ' Ram 05-01-2001
        Dim vDataModelCodeArray(,) As Object = Nothing
        Dim vGISSearchDataArray(,) As Object = Nothing

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue


            ' Get All Data Model Codes
            m_lReturn = GetAllDataModelCodes(vDataModelCodeArray, lSpecificDataModelIndex)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not informations.IsArray(vDataModelCodeArray) Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get DataModel Codes", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndex")

                Return gPMConstants.PMEReturnCode.PMFalse

            Else


                ' Get Search Results for all available Data Model Codes
                m_lReturn = GetAllGISSearchResults(sIndex, lNumberOfRecords, vDataModelCodeArray,
                                                   vGISSearchDataArray,
                                                   lSpecificFieldType,
                                                   v_vAgentGroupCnt,
                                                   v_bAgencyProductOnly,
                                                   iFileType,
                                                   bRetrieveAssociates)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="FindLikeIndex Failed. Failed to GetAllGISSearchResults", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndex")

                    vResultArray = vGISSearchDataArray
                    nResult = gPMConstants.PMEReturnCode.PMFalse

                Else
                    vResultArray = vGISSearchDataArray

                    ' If NO Indexes were found return Not Found
                    If Not informations.IsArray(vResultArray) Then
                        nResult = gPMConstants.PMEReturnCode.PMNotFound
                    End If

                End If

            End If

            Return nResult

        Catch excep As System.Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsuranceFolder (Public) - TF100398
    '
    ' Description: Gets the InsuranceFolderCnt using InsuranceFile.Detail
    '
    ' ***************************************************************** '
    Public Function GetInsuranceFolder(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim oInsFile As New bSIRInsuranceFile.Services

            m_lReturn = oInsFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oInsFile.InsuranceFileCnt = v_lInsuranceFileCnt


            r_lInsuranceFolderCnt = oInsFile.InsuranceFolderCnt

            oInsFile.Dispose()

            oInsFile = Nothing

            Return result

        Catch excep As System.Exception




            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFolder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetVersionArray
    '
    ' Description:
    '
    ' History: 22/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetVersionArray(ByRef r_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_sPolicyNumber As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not informations.IsNothing(v_sPolicyNumber) Then
                sSQL = "SELECT ifi.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ifi.policy_version," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ifs.last_trans_date," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ift.code," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ifi.cover_start_date," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ifi.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ifi.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ifs.last_trans_description," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ifs.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "party.name," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "party_personal_client.forename" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM insurance_file ifi," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "insurance_file_system ifs," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "insurance_file_type ift," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "insurance_folder," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "party," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "party_personal_client" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE ifi.insurance_file_cnt = ifs.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND ifi.policy_ignore is null" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND ifi.insurance_file_type_id = ift.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND ifi.insurance_ref = '" & v_sPolicyNumber & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND ifi.insurance_folder_cnt = insurance_folder.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND insurance_folder.insurance_holder_cnt = party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND party.party_cnt = party_personal_client.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

                'RJG 12/07/2000 - The MTA Quote Filter has been superseeded by filtering MTASUSPEND
                'RJG 22/06/2000 - Filter out MTAQUOTE's that are before todays date
                'sSQL = sSQL & "AND ifi.insurance_file_cnt not in (SELECT insurance_file_cnt" & vbCrLf
                'sSQL = sSQL & "FROM insurance_file, insurance_file_type" & vbCrLf
                'sSQL = sSQL & "WHERE insurance_file_type.code = 'MTAQUOTE'" & vbCrLf
                'sSQL = sSQL & "AND insurance_file.cover_start_date < GetDate()" & vbCrLf
                'sSQL = sSQL & "AND insurance_file.insurance_file_type_id = insurance_file_type.insurance_file_type_id)"

                sSQL = sSQL & "AND ifi.insurance_file_status_id is null" & Strings.ChrW(13) & Strings.ChrW(10)
                'RJG 12/07/00 - END
            Else
                If v_lInsuranceFolderCnt = 0 Then
                    sSQL = "SELECT ifi.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifi.policy_version," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifs.last_trans_date," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ift.code," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifi.cover_start_date," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifi.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifi.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifs.last_trans_description," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifs.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "party.name," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "party_personal_client.forename" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "FROM insurance_file ifi," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "insurance_file_system ifs," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "insurance_file_type ift," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "insurance_file ifi2," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "insurance_folder," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "party," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "party_personal_client" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "WHERE ifi.insurance_file_cnt = ifs.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND ifi.policy_ignore is null" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND ifi.insurance_file_type_id = ift.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND ifi2.insurance_file_cnt = " & CStr(r_lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND ifi.insurance_folder_cnt = insurance_folder.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND insurance_folder.insurance_holder_cnt = party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND party.party_cnt = party_personal_client.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

                    'RJG 12/07/2000 - The MTA Quote Filter has been superseeded by filtering MTASUSPEND
                    'RJG 22/06/2000 - Filter out MTAQUOTE's that are before todays date
                    'sSQL = sSQL & "AND ifi.insurance_file_cnt not in (SELECT insurance_file_cnt" & vbCrLf
                    'sSQL = sSQL & "FROM insurance_file, insurance_file_type" & vbCrLf
                    'sSQL = sSQL & "WHERE insurance_file_type.code = 'MTAQUOTE'" & vbCrLf
                    'sSQL = sSQL & "AND insurance_file.cover_start_date < GetDate()" & vbCrLf
                    'sSQL = sSQL & "AND insurance_file.insurance_file_type_id = insurance_file_type.insurance_file_type_id)"

                    sSQL = sSQL & "AND ifi.insurance_file_status_id is null" & Strings.ChrW(13) & Strings.ChrW(10)

                    'RJG 12/07/00 - END
                Else
                    sSQL = "SELECT ifi.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifi.policy_version," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifs.last_trans_date," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ift.code," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifi.cover_start_date," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifi.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifi.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifs.last_trans_description," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "ifs.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "party.name," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "party_personal_client.forename" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "FROM insurance_file ifi," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "insurance_file_system ifs," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "insurance_file_type ift," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "insurance_folder," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "party," & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "party_personal_client" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "WHERE ifi.insurance_file_cnt = ifs.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND ifi.policy_ignore is null" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND ifi.insurance_file_type_id = ift.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND ifi.insurance_folder_cnt = " & CStr(v_lInsuranceFolderCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND ifi.insurance_folder_cnt = insurance_folder.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND insurance_folder.insurance_holder_cnt = party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND party.party_cnt = party_personal_client.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

                    'RJG 12/07/2000 - The MTA Quote Filter has been superseeded by filtering where insurance file status id is null
                    'RJG 22/06/2000 - Filter out MTAQUOTE's that are before todays date
                    'sSQL = sSQL & "AND ifi.insurance_file_cnt not in (SELECT insurance_file_cnt" & vbCrLf
                    'sSQL = sSQL & "FROM insurance_file, insurance_file_type" & vbCrLf
                    'sSQL = sSQL & "WHERE insurance_file_type.code = 'MTAQUOTE'" & vbCrLf
                    'sSQL = sSQL & "AND insurance_file.cover_start_date < GetDate()" & vbCrLf
                    'sSQL = sSQL & "AND insurance_file.insurance_file_type_id = insurance_file_type.insurance_file_type_id)"

                    sSQL = sSQL & "AND ifi.insurance_file_status_id is null" & Strings.ChrW(13) & Strings.ChrW(10)
                    'RJG 12/07/00 - END

                End If
            End If

            sSQL = sSQL & " ORDER BY 5, 6"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetVersionArray", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetVersionByDate
    '
    ' Description:
    '
    ' History: 22/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetVersionByDate(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_lSubErrorCode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing
        Dim lPolicyTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT policy_type_id FROM insurance_file WHERE insurance_file_cnt = " & r_lInsuranceFileCnt

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyTypeId", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If informations.IsArray(vArray) Then

                lPolicyTypeId = CInt(vArray(0, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case lPolicyTypeId
                Case PMBConst.PMBPolicyTypeUnderwriting
                    m_lReturn = GetUnderwritingVersionByDate(r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_dtStartDate:=v_dtStartDate, r_lPolicyVersion:=r_lPolicyVersion, r_lErrorCode:=r_lErrorCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_lSubErrorCode:=r_lSubErrorCode)
                Case Else
                    m_lReturn = GetOtherVersionByDate(r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_dtStartDate:=v_dtStartDate, r_lPolicyVersion:=r_lPolicyVersion, r_lErrorCode:=r_lErrorCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionByDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUnderwritingVersionByDate
    '
    ' Description:
    '
    ' History: 07/06/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetUnderwritingVersionByDate(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_lSubErrorCode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As StringBuilder
        Dim sSQL1 As StringBuilder
        Dim vArray(,) As Object = Nothing
        Dim dtCoverStartDate, dtExpiryDate As Date
        Dim lUbound, lThisOne As Integer
        Dim sType, sStatus As String
        Dim bPermanentMTA As Boolean
        Dim dtLastRenewalDate, dtLastMTADate As Date
        Dim bRenewal, bLastMta As Boolean
        Dim iMTADatesAllowed As Integer
        Dim lProduct_id As Integer
        Dim dtSecondLastRenewalDate As Date
        Dim bLastSecondRenewal As Boolean
        Dim vResultArray(,) As Object = Nothing
        Dim r_vResults(,) As Object = Nothing
        Dim bFoundLastMTA As Boolean

        'Const AC_InsuranceFileCnt As Integer = 0
        'Const AC_PolicyVersion As Integer = 1
        'Const AC_LastTransDate As Integer = 2
        'Const AC_PolicyTypeCode As Integer = 3
        'Const AC_CoverStartDate As Integer = 4
        'Const AC_ExpiryDate As Integer = 5
        'Const AC_PolicyStatusCode As Integer = 6
        'Const AC_SourceDesc As Integer = 7
        Const AC_SourceIsDeleted As Integer = 8
        Const AC_SourceAllowTempMTA As Integer = 9
        Const AC_SourceAllowPermMTA As Integer = 10



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the product from the insurance_file table
        m_lReturn = GetValueFromTable(v_sTableName:="insurance_file", v_vReturnColumn:="product_id", v_sKeyColumn:="insurance_file_cnt", v_sKeyValue:=r_lInsuranceFileCnt, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResultArray)
        lProduct_id = vResultArray(0, 0)

        'Get Out of Sequence dates allowed parameter
        m_lReturn = GetValueFromTable(v_sTableName:="product", v_vReturnColumn:="out_of_sequence_mta_dates", v_sKeyColumn:="product_id", v_sKeyValue:=lProduct_id, v_iDataType:=gPMConstants.PMEDataType.PMInteger, r_vResult:=vResultArray)

        iMTADatesAllowed = vResultArray(0, 0)
        'If we're underwriting, pass if we're permanent or not
        'The things we do to avoid breaking binary compatibility
        'bPermanentMTA = (r_lErrorCode = 1)

        'r_lErrorCode = 1
        If (r_lErrorCode = 1) Then
            bPermanentMTA = True
        Else
            bPermanentMTA = False
        End If


        r_lErrorCode = 0

        If v_lInsuranceFolderCnt = 0 Then

            'Modifying the inline query to make it compatible with SQL server 2005
            sSQL = New StringBuilder
            sSQL.Append("SELECT DISTINCT ifi.insurance_file_cnt,")
            sSQL.Append("ifi.policy_version,")
            sSQL.Append("ifs.last_trans_date,")
            sSQL.Append("ift.code,")
            sSQL.Append("ifi.cover_start_date,")
            sSQL.Append("ifi.expiry_date,")
            sSQL.Append("ifst.code,")
            sSQL.Append("s.description,")
            sSQL.Append("s.is_deleted,")
            sSQL.Append("s.closed_allow_temp_mta,")
            sSQL.Append("s.closed_allow_perm_mta")
            sSQL.Append(" FROM insurance_file ifi")
            sSQL.Append(" INNER JOIN insurance_file_system ifs")
            sSQL.Append(" ON ifi.insurance_file_cnt = ifs.insurance_file_cnt")
            sSQL.Append(" AND ifi.policy_ignore is null")
            sSQL.Append(" INNER JOIN insurance_file_type ift")
            sSQL.Append(" ON ifi.insurance_file_type_id = ift.insurance_file_type_id")
            sSQL.Append(" INNER JOIN insurance_file ifi2")
            sSQL.Append(" ON ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt")
            sSQL.Append(" AND ifi2.insurance_file_cnt = " & CStr(r_lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN  source s")
            sSQL.Append(" ON ifi.source_id = s.source_id")
            sSQL.Append(" LEFT OUTER JOIN insurance_file_status ifst")
            sSQL.Append(" ON ifi.insurance_file_status_id = ifst.insurance_file_status_id")
            'sSQL.Append(" LEFT JOIN MTA_Insurance_FIle_link MIFL")
            'sSQL.Append(" ON MIFL.original_linked_insurance_file_cnt=ifi.insurance_file_cnt")
            'sSQL.Append(" OR MIFL.cancelled_linked_insurance_file_cnt=ifi.insurance_file_cnt")
            'sSQL.Append(" WHERE (MIFL.original_linked_insurance_file_cnt IS NULL OR MIFL.cancelled_linked_insurance_file_cnt IS NULL OR MIFL.cancelled_linked_insurance_file_cnt =0  OR ISNULL(ifi.insurance_file_status_id,0)<>1 )")
        Else
            sSQL = New StringBuilder
            sSQL.Append("SELECT ifi.insurance_file_cnt,")
            sSQL.Append("ifi.policy_version,")
            sSQL.Append("ifs.last_trans_date,")
            sSQL.Append("ift.code,")
            sSQL.Append("ifi.cover_start_date,")
            sSQL.Append("ifi.expiry_date,")
            sSQL.Append("ifst.code,")
            sSQL.Append("s.description,")
            sSQL.Append("s.is_deleted,")
            sSQL.Append("s.closed_allow_temp_mta,")
            sSQL.Append("s.closed_allow_perm_mta")
            sSQL.Append(" FROM insurance_file ifi")
            sSQL.Append(" INNER JOIN Insurance_file_system ifs")
            sSQL.Append(" ON ifi.insurance_file_cnt = ifs.insurance_file_cnt")
            sSQL.Append(" AND ifi.policy_ignore is null")
            sSQL.Append(" AND ifi.insurance_folder_cnt = " & CStr(v_lInsuranceFolderCnt) & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN insurance_file_type ift")
            sSQL.Append(" ON ifi.insurance_file_type_id = ift.insurance_file_type_id")
            sSQL.Append(" INNER JOIN source s")
            sSQL.Append(" ON ifi.source_id = s.source_id")
            sSQL.Append(" LEFT OUTER JOIN insurance_file_status ifst")
            sSQL.Append(" ON ifi.insurance_file_status_id = ifst.insurance_file_status_id")
            sSQL.Append(" WHERE ISNull(ifi.out_of_sequence_replaced, 0) <> 1 ")
        End If

        sSQL.Append(" ORDER BY ifi.cover_start_date, ifi.insurance_file_cnt, ifi.expiry_date")

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetUnderwritingVersionByDate", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not informations.IsArray(vArray) Then
            r_lErrorCode = 3
            Return result
        End If


        'So now we work out which one we're after...


        lUbound = vArray.GetUpperBound(1)

        'First let's validate - get out when there's an error
        'Here the rules are: if this is a temporary MTA, the future does not matter
        'If this is a permanent MTA, worry only about permanent stuff
        If lUbound > 0 Then

            bRenewal = False
            bLastMta = False
            bLastSecondRenewal = False
            bFoundLastMTA = False
            'Fetch the last renewal date
            For lTemp As Integer = lUbound To 0 Step -1

                If CStr(vArray(3, lTemp)).Trim() = "POLICY" Then
                    If Not bRenewal Then

                        dtLastRenewalDate = CDate(vArray(4, lTemp))
                        bRenewal = True
                    ElseIf Not bLastSecondRenewal Then

                        dtSecondLastRenewalDate = CDate(vArray(4, lTemp))
                        bLastSecondRenewal = True
                    End If
                ElseIf (CStr(vArray(3, lTemp)).Trim() = "MTA PERM" Or CStr(vArray(3, lTemp)).Trim() = "MTAREINS" Or Trim$(vArray(3, lTemp)) = "MTACAN") And Not bLastMta Then
                    dtLastMTADate = CDate(vArray(4, lTemp))
                    bLastMta = True
                End If

                If bLastMta = True And bFoundLastMTA = False Then
                    bLastMta = True
                    bFoundLastMTA = True
                End If

            Next

            For lTemp As Integer = 0 To lUbound

                sType = CStr(vArray(3, lTemp))
                Dim auxVar As Object = vArray(6, lTemp)

                If Informations.IsDBNull(vArray(6, lTemp)) Or lTemp = 0 Then  ' include NB version that has to be first
                    sStatus = ""
                Else
                    sStatus = CStr(vArray(6, lTemp))
                End If


                dtCoverStartDate = CDate(vArray(4, lTemp))
                dtExpiryDate = CDate(vArray(5, lTemp))

                Select Case sStatus.Trim()
                    'Ignore cancelled or lapsed policies etc.

                    Case Else
                        'Check if MTA effective date < Original Policy Cover Date
                        If lTemp = 0 And (dtCoverStartDate.Date > v_dtStartDate.Date) Then
                            r_lErrorCode = 6

                            vArray = Nothing
                            Return result
                            'Check if MTA effective date < Last Renewal Date
                        ElseIf dtLastRenewalDate.Date > v_dtStartDate.Date And bPermanentMTA And iMTADatesAllowed = MTA_DATE_CURRENT_PERIOD_ONLY Then
                            r_lErrorCode = 7

                            vArray = Nothing
                            Return result
                        ElseIf dtSecondLastRenewalDate.Date > v_dtStartDate.Date And bPermanentMTA And iMTADatesAllowed = MTA_DATE_CURRENT_PLUS_1 Then
                            r_lErrorCode = 10

                            vArray = Nothing
                            Return result
                        End If
                        If bPermanentMTA Then
                            'Any future adjustments?
                            If dtCoverStartDate.Date > v_dtStartDate.Date Then

                                'Renewals are ok
                                Select Case sType.Trim()
                                        ''PN 77505 Opening BackDateMTA after re-instatement
                                    Case "RENEWAL", "MTAQUOTE", "MTA TEMP", "MTAQTETEMP", "MTAREINS", "MTAQCAN", "MTAQREINS"
                                    Case Else
                                        If iMTADatesAllowed >= MTA_DATE_CURRENT_PERIOD_ONLY Then
                                            r_lErrorCode = 8
                                        Else
                                            'Oops, let's get out
                                            r_lErrorCode = 1

                                            vArray = Nothing
                                            Return result
                                        End If
                                End Select
                            ElseIf dtLastMTADate.Date > v_dtStartDate.Date Then
                                If iMTADatesAllowed = MTA_DATE_NOT_ALLOWED Then
                                    r_lErrorCode = 9

                                    vArray = Nothing
                                    Return result
                                Else
                                    r_lErrorCode = 8

                                End If
                            End If
                        End If

                        'Any current (temporary) adjustments?
                        '                If (dtExpiryDate >= v_dtStartDate) Then
                        '                    If (Trim$(sType) = "MTA TEMP") Then
                        '                        'Oops, let's get out again
                        '                        r_lErrorCode = 2
                        '                        vArray = ""
                        '                        Exit Function
                        '                    End If
                        '                End If
                End Select

            Next lTemp
        End If

        'So, this is a valid MTA, we need to decide which one we're working on

        'The rule is, latest start date before passed date, earliest end date after passed date.
        'Note that end dates should be all the same, except for temp MTAs _before_ the new one
        'We can work through the array backwards...

        'It should always find something, but...
        lThisOne = -1

        For lTemp As Integer = lUbound To 0 Step -1

            sType = CStr(vArray(3, lTemp))

            dtCoverStartDate = CDate(vArray(4, lTemp))

            dtExpiryDate = CDate(vArray(5, lTemp))
            If (dtCoverStartDate <= v_dtStartDate) AndAlso
                (dtExpiryDate >= v_dtStartDate) AndAlso
                    ((sType.Trim() = "MTA PERM") OrElse (sType.Trim() = "POLICY") OrElse (sType.Trim() = "MTAREINS") OrElse (Trim$(sType) = "MTACAN")) Then
                bFoundLastMTA = True
                'It's this one...
                If bFoundLastMTA = True Then
                    lThisOne = lTemp
                    Exit For
                End If
            ElseIf lTemp = 0 Then
                If v_dtStartDate.Date < dtCoverStartDate.Date Then
                    r_lSubErrorCode = 0
                ElseIf (v_dtStartDate.Date > dtExpiryDate.Date) Then
                    r_lSubErrorCode = 1
                End If
            End If
        Next lTemp

        If lThisOne = -1 Then
            'We should never, ever get here, but just in case
            r_lErrorCode = 3
            vArray = Nothing
            Return result
        End If

        ' We need to check if the branch is closed. If it is, we need to make sure
        ' the "allow mta on closed branch" flag is set to 1

        If CStr(vArray(AC_SourceIsDeleted, lThisOne)) = "1" Then
            If bPermanentMTA Then
                ' Check permanent MTA flag

                If CStr(vArray(AC_SourceAllowPermMTA, lThisOne)) = "0" Then
                    r_lErrorCode = 4
                    vArray = Nothing
                    Return result
                End If
            Else
                ' Check temp MTA flag

                If CStr(vArray(AC_SourceAllowTempMTA, lThisOne)) = "0" Then
                    r_lErrorCode = 5
                    vArray = Nothing
                    Return result
                End If
            End If
        End If


        r_lInsuranceFileCnt = CInt(vArray(0, lThisOne))

        'Last thing - what version is the new adjustment?
        sSQL1 = New StringBuilder
        sSQL1.Append("SELECT MAX(ifi.policy_version)")
        sSQL1.Append(" FROM insurance_file ifi,")
        sSQL1.Append(" insurance_file ifi2")
        sSQL1.Append(" WHERE ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt")
        sSQL1.Append(" AND ifi.policy_ignore is null")
        sSQL1.Append(" AND ifi2.insurance_file_cnt = " & CStr(r_lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10))

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL1.ToString(), sSQLName:="GetUnderwritingVersionByDate2", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vArray) Then
            Return result
        End If


        r_lPolicyVersion = CInt(vArray(0, 0))

        vArray = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetOption
    '
    ' Description: Get a system option - lifted from bOpenClaim.Business
    '
    ' History: PW221102 - Created (PS411)
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_nOptionValue As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim m_oSystemOption As bSIROptions.Business = Nothing
            Dim sOptionValue As String = ""

            If m_oSystemOption Is Nothing Then

                m_oSystemOption = New bSIROptions.Business()

                m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get system option " & v_iOptionNumber, vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            r_nOptionValue = CInt(sOptionValue)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function





    ' ***************************************************************** '
    '
    ' Name: GetOtherVersionByDate
    '
    ' Description:
    '
    ' History: 22/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetOtherVersionByDate(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing
        Dim lUbound, lThisOne As Integer
        Dim dtTemp1, dtTemp2 As Date
        Dim sCode As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_lInsuranceFolderCnt = 0 Then
            sSQL = "SELECT ifi.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifi.policy_version," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifs.last_trans_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ift.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifi.cover_start_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifi.expiry_date" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM insurance_file ifi," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "insurance_file_system ifs," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "insurance_file_type ift," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "insurance_file ifi2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE ifi.insurance_file_cnt = ifs.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifi.policy_ignore is null" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifi.insurance_file_type_id = ift.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifi2.insurance_file_cnt = " & CStr(r_lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
        Else
            sSQL = "SELECT ifi.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifi.policy_version," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifs.last_trans_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ift.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifi.cover_start_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifi.expiry_date" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM insurance_file ifi," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "insurance_file_system ifs," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "insurance_file_type ift" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE ifi.insurance_file_cnt = ifs.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifi.policy_ignore is null" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifi.insurance_file_type_id = ift.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifi.insurance_folder_cnt = " & CStr(v_lInsuranceFolderCnt) & Strings.ChrW(13) & Strings.ChrW(10)
        End If

        sSQL = sSQL & "ORDER BY 5, 6 DESC"

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOtherVersionByDate", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vArray) Then
            r_lErrorCode = 3
            Return result
        End If

        'So now we work out which one we're after...


        lUbound = vArray.GetUpperBound(1)

        'First let's validate - get out when there's an error
        If lUbound > 0 Then
            For lTemp As Integer = 1 To lUbound

                sCode = CStr(vArray(3, lTemp))

                dtTemp1 = CDate(vArray(4, lTemp))

                dtTemp2 = CDate(vArray(5, lTemp))

                'Any future adjustments?
                If dtTemp1 > v_dtStartDate Then
                    'Renewals are ok
                    If sCode.Trim() <> "RENEWAL" Then
                        'Oops, let's get out
                        r_lErrorCode = 1
                        vArray = Nothing
                        Return result
                    End If
                End If

                'Any current (temporary) adjustments?
                If dtTemp2 >= v_dtStartDate Then
                    If sCode.Trim() = "MTA TEMP" Then
                        'Oops, let's get out again
                        r_lErrorCode = 2
                        vArray = Nothing
                        Return result
                    End If
                End If
            Next lTemp
        End If

        'So, this is a valid MTA, we need to decide which one we're working on

        'The rule is, latest start date before passed date, earliest end date after passed date.
        'Note that end dates should be all the same, except for temp MTAs _before_ the new one
        'We can work through the array backwards...

        'It should always find something, but...
        lThisOne = -1

        For lTemp As Integer = lUbound To 0 Step -1

            sCode = CStr(vArray(3, lTemp))

            dtTemp1 = CDate(vArray(4, lTemp))

            dtTemp2 = CDate(vArray(5, lTemp))
            If (dtTemp1 <= v_dtStartDate) And (dtTemp2 > v_dtStartDate) And ((sCode.Trim() = "MTA PERM") Or (sCode.Trim() = "POLICY")) Then
                'It's this one...
                lThisOne = lTemp
                Exit For
            End If
        Next lTemp

        If lThisOne = -1 Then
            'We should never, ever get here, but just in case
            r_lErrorCode = 3
            vArray = Nothing
            Return result
        End If


        r_lInsuranceFileCnt = CInt(vArray(0, lThisOne))

        'Last thing - what version is the new adjustment?

        sSQL = "SELECT MAX(ifi.policy_version)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "FROM insurance_file ifi," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "insurance_file ifi2" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND ifi.policy_ignore is null" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND ifi2.insurance_file_cnt = " & CStr(r_lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOtherVersionByDate2", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vArray) Then
            Return result
        End If


        r_lPolicyVersion = CInt(vArray(0, 0))

        vArray = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckForUnderwriting
    '
    ' Description: Check if this rubbish is installed or not
    '
    ' History: 05/07/2001 CTAF - Created.
    '          14/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Private Function CheckForUnderwriting(ByRef r_bInstalled As Boolean) As Integer

        Dim result As Integer = 0


        Dim strUnderwriting As String = ""

        result = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, strUnderwriting)

        ' Check if its installed
        r_bInstalled = True

        Return result

    End Function

    ''' <summary>
    ''' Copy Policy details
    ''' </summary>
    ''' <param name="v_lOldInsuranceFileCnt"></param>
    ''' <param name="r_lNewInsuranceFileCnt"></param>
    ''' <param name="v_lVersion"></param>
    ''' <param name="v_bPermanentMTA"></param>
    ''' <param name="v_dtMTADate"></param>
    ''' <param name="v_bReinstatement"></param>
    ''' <param name="v_vMTAEndDate"></param>
    ''' <param name="v_bCancellation"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <param name="v_bIsBackdatedMTA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyPolicy(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer,
                               ByVal v_lVersion As Integer, ByVal v_bPermanentMTA As Boolean, ByVal v_dtMTADate As Date,
                               Optional ByVal v_bReinstatement As Boolean = False,
                               Optional ByVal v_vMTAEndDate As Object = Nothing,
                               Optional ByVal v_bCancellation As Object = Nothing,
                               Optional ByVal v_sTransactionType As String = "",
                               Optional ByVal v_bIsBackdatedMTA As Boolean = False,
                               Optional v_bCopyRiskLink As Boolean = True) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oObject As bSIRInsuranceFile.Services
        Dim sInsuredName As String
        Dim bUWInstalled As Boolean
        Try

            oObject = New bSIRInsuranceFile.Services
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oObject.InsuranceFileCnt = v_lOldInsuranceFileCnt

            ' This line isn't really needed as the above does the same, but I'll leave it for
            ' now
            m_lReturn = oObject.GetDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse

                oObject.Dispose()
                oObject = Nothing
                Return nResult
            End If

            If v_bReinstatement Then
                oObject.InsuranceFileTypeID = 10
                oObject.LapsedDate = Nothing
            ElseIf v_bPermanentMTA Then

                oObject.InsuranceFileTypeID = 4
                'This need to be differentiated for different Roadmaps
                '(Backdated MTAs, Manual and unattended(via sirius import routine) Credit Control)

                If Not informations.IsNothing(v_bCancellation) Then
                    If v_bCancellation Then
                        'it's a cancellation live version
                        Dim oInsuranceFileTypeArray As Object = Nothing
                        oObject.InsuranceFileTypeID = 12
                        'Get InsuranceFileTypeId for InsuranceFileTypeCode="MTAQCAN"
                        m_oDatabase.Parameters.Clear()
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileTypeCode", vValue:="MTAQCAN", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsuranceFileTypeIdFromCodeSQL,
                                  sSQLName:=ACGetInsuranceFileTypeIdFromCodeName,
                                  bStoredProcedure:=ACGetInsuranceFileTypeIdFromCodeStored,
                                  vResultArray:=oInsuranceFileTypeArray)

                        oObject.InsuranceFileTypeID = oInsuranceFileTypeArray(0, 0)
                        oObject.LapsedDate = v_dtMTADate
                        If Not (m_sCallingAppName = "iPMUChaseCycleProcessing") _
                            AndAlso Not (m_sCallingAppName = "uctListPolicyVersionControl") Then
                            oObject.LapsedReason = PMBConst.PMBAutoCancelLapsedCode
                        End If
                    End If
                ElseIf m_sCallingAppName = "iACTCreditControlProcessing" Then

                    oObject.LapsedDate = m_dtEffectiveDate

                    oObject.LapsedReason = PMBConst.PMBAutoCancelLapsedCode
                End If
            Else
                oObject.InsuranceFileTypeID = 7
            End If

            ' Check for null
            If Not (Informations.IsDBNull(oObject.InsuredName) OrElse informations.IsNothing(oObject.InsuredName)) Then
                sInsuredName = oObject.InsuredName
            Else
                sInsuredName = ""
            End If

            oObject.CoverStartDate = v_dtMTADate

            If Not informations.IsNothing(v_vMTAEndDate) Then
                'NIIT Comment: when "" Compared with Date gives error.
                If v_vMTAEndDate <> Date.MinValue Then
                    oObject.ExpiryDate = v_vMTAEndDate
                End If
            End If

            ' If we haven't recieved a version increment the current one.
            If v_lVersion = -1 Then
                oObject.PolicyVersion += 1
            Else
                oObject.PolicyVersion = v_lVersion + 1
            End If

            oObject.EventDescription = "Endorsement"

            'Tracy Richards 02/09/03 Make the Quote Expiry Date blank

            If oObject.InsuranceFileStatus <> "LAP" Then
                oObject.InsuranceFileStatus = Nothing
            End If

            ' The copied policy will have an unquoted premium of zero
            oObject.AnnualPremium = 0
            oObject.ThisPremium = 0
            oObject.NetPremium = 0
            oObject.TaxAmount = 0

            'get default Date for QuoteExpiry
            Dim dtQuoteExpiryDate As Object = Nothing
            m_lReturn = DefaultQuoteExpiryDate(v_nInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_dtMTADate:=v_dtMTADate, r_dtQuoteExpiryDate:=dtQuoteExpiryDate)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                oObject.QuoteExpiryDate = dtQuoteExpiryDate
            End If

            m_lReturn = oObject.CreatePolicy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                oObject.Dispose()
                oObject = Nothing
                Return nResult
            End If

            r_lNewInsuranceFileCnt = oObject.InsuranceFileCnt
            oObject.Dispose()
            oObject = Nothing
            If v_bCopyRiskLink Then
                'EM
                m_lReturn = CopyRiskLinks(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt,
                                          v_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt,
                                          v_bCopyDeletedRisk:=v_sTransactionType = "MTCA" And v_bIsBackdatedMTA = True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' CTAF 050701 - Check for Underwriting
            m_lReturn = CheckForUnderwriting(r_bInstalled:=bUWInstalled)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 050701 - Call the following if UW is installed
            If bUWInstalled Then

                m_lReturn = CopyCoinsurance(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt,
                                            v_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CopySubAgent(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt,
                                         v_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt,
                                                       v_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CopyPolicyAssociates(nOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, nNewInsuranceFileCnt:=r_lNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: CopyPolicyV2
    '
    ' Description:
    '
    ' History: 09/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function CopyPolicyV2(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ByVal v_iSourceID As Integer, ByVal v_lTargetPartyCnt As Integer, Optional ByRef v_lDontCopyTextFiles As Integer = 0) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".CopyPolicyV2")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_insurance_file_cnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=CStr(r_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Target Party_cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="target_party_cnt", vValue:=CStr(v_lTargetPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Add parameter
            ' This is here incase someone wants to change it to a different source in future.
            ' It's not been specced for that doesn't mean squat.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="target_source_id", vValue:=CStr(v_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DJM 24/10/2003 : Check using PMTrue, not just True.
            If v_lDontCopyTextFiles = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="CopyTextFile", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="CopyTextFile", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If
            '
            ' Call the SP
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyPolicyV3SQL, sSQLName:=ACCopyPolicyV3Name, bStoredProcedure:=ACCopyPolicyV2Stored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the new insurance_file
            r_lNewInsuranceFileCnt = m_oDatabase.Parameters.Item("new_insurance_file_cnt").Value

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".CopyPolicyV2")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".CopyPolicyV2")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyV2 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: CopyDataSet
    '
    ' Description: Calls the GIS to copy the dataset
    '
    ' History: 04/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CopyDataSet(ByVal v_sDataModelCode As String, ByRef r_lNewGISPolicyLinkID As Object, ByRef r_sXMLDataSetDef As Object, ByRef r_sXMLDataSet As Object, Optional ByVal v_vOldGISPolicyLinkId As Object = Nothing, Optional ByVal v_vOldInsuranceFileCnt As Object = Nothing, Optional ByVal v_vOldXMLDataSet As String = "", Optional ByVal v_vNewInsuranceFileCnt As Object = Nothing, Optional ByVal v_vOldRiskID As Object = Nothing, Optional ByVal v_vNewRiskID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oGIS As Object = Nothing
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create the GIS
        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        ' Call the gis


        m_lReturn = oGIS.CopyDataSet(v_sDataModelCode:=ToSafeString(v_sDataModelCode), r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataSet:=r_sXMLDataSet, v_vOldGISPolicyLinkId:=v_vOldGISPolicyLinkId, v_vOldInsuranceFileCnt:=v_vOldInsuranceFileCnt, v_vOldXMLDataSet:=ToSafeString(v_vOldXMLDataSet), v_vNewInsuranceFileCnt:=v_vNewInsuranceFileCnt, v_vOldRiskID:=If(Informations.IsNothing(v_vOldRiskID), -1, v_vOldRiskID), v_vNewRiskID:=If(Informations.IsNothing(v_vNewRiskID), -1, v_vNewRiskID))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Clear up

        oGIS.Dispose()

        oGIS = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: CopyRisk
    '
    ' Description: Copies a risk after a policy has been copied
    '
    ' History: 04/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function CopyRisk(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lOldRiskID, lNewRiskID As Integer
        Dim sDataModelCode As String = ""
        Dim lNewGISPolicyLinkID As Integer
        Dim vOldGISPolicyLinkID As String = String.Empty
        Dim sXMLDataSetDef As String = String.Empty
        Dim sXMLDataSet As String = String.Empty

        'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup START
        'KNCMGRISK Start
        Dim vRiskArrayNew(,) As Object = Nothing
        Dim r_vRiskTypeDetails As Object = Nothing
        Dim lOldPolicyBinderId, lNewPolicyBinderId As Integer
        'KNCMGRISK End
        'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup END

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get data model code
            sSQL = ""
            sSQL = sSQL & "SELECT gdm.code, gpl.gis_policy_link_id" & Environment.NewLine
            sSQL = sSQL & "FROM gis_data_model gdm" & Environment.NewLine
            sSQL = sSQL & "INNER JOIN gis_policy_link gpl" & Environment.NewLine
            sSQL = sSQL & "ON gpl.gis_data_model_id = gdm.gis_data_model_id" & Environment.NewLine
            sSQL = sSQL & "WHERE gpl.insurance_file_cnt = {insurance_file_cnt}"

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDataModelCode", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Failed : insurance_file_cnt = " & v_lOldInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' If we have no result, then no problem
            If Not Informations.IsArray(vResultArray) Then
                Return result
            End If

            ' Get the results

            sDataModelCode = CStr(vResultArray(0, 0)).Trim()

            vOldGISPolicyLinkID = CStr(vResultArray(1, 0))

            ' Check if we have a Risk
            sSQL = ""
            sSQL = sSQL & "SELECT risk_cnt" & Environment.NewLine
            sSQL = sSQL & "FROM insurance_file_risk_link" & Environment.NewLine
            sSQL = sSQL & "WHERE insurance_file_cnt = {insurance_file_cnt}"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRisk", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then

                ' Call GIS without RiskID
                m_lReturn = CopyDataSet(v_sDataModelCode:=sDataModelCode, r_lNewGISPolicyLinkID:=lNewGISPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_vOldGISPolicyLinkId:=vOldGISPolicyLinkID, v_vOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_vNewInsuranceFileCnt:=v_lNewInsuranceFileCnt)
            Else

                ' Grab the RiskId

                lOldRiskID = CInt(vResultArray(0, 0))

                'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup START
                'Get the new insurance_folder_cnt
                sSQL = ""
                sSQL = sSQL & "SELECT insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM insurance_file i" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE i.insurance_file_cnt = " & CStr(v_lNewInsuranceFileCnt)

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Getinsurance_folder_cnt", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Failed : insurance_file_cnt = " & v_lOldInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lRiskId = lOldRiskID
                m_lInsuranceFileCnt = v_lNewInsuranceFileCnt

                m_lInsuranceFolderCnt = CInt(vResultArray(0, 0))


                'developer guide no. 98
                m_lReturn = CreateRisk(vRiskDetails:=vRiskArrayNew, vRiskTypeArray:=r_vRiskTypeDetails)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'KNCMGRISK End


                lNewRiskID = CInt(vRiskArrayNew(0, 0))
                'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup END

                'prepare details to copy GIS Stuff attached to current risk

                'do we have any data


                m_lReturn = GIS_LoadFromDB(sDataModelCode, CInt(vRiskArrayNew(ACRRiskFolderCnt, 0)), vOldGISPolicyLinkID, CInt(vRiskArrayNew(0, 0))) 'copy GIS details to NewInsuranceFileCnt


                lNewRiskID = CInt(vRiskArrayNew(0, 0))
                'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup END

                'prepare details to copy GIS Stuff attached to current risk

                'do we have any data

                'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup START
                '        ' Get the new RiskID too
                '        sSQL = ""
                '        sSQL = sSQL & "SELECT risk_cnt" & vbNewLine
                '        sSQL = sSQL & "FROM insurance_file_risk_link" & vbNewLine
                '        sSQL = sSQL & "WHERE insurance_file_cnt = {insurance_file_cnt}"
                '
                '        m_oDatabase.Parameters.Clear
                '
                '        m_lReturn& = m_oDatabase.Parameters.Add( _
                ''                        sName:="insurance_file_cnt", _
                ''                        vValue:=v_lNewInsuranceFileCnt, _
                ''                        idirection:=PMParamInput, _
                ''                        iDataType:=PMLong)
                '        If (m_lReturn& <> PMTrue) Then
                '            CopyRisk = PMFalse
                '            Exit Function
                '        End If
                '
                '        ' Call the SQL
                '        m_lReturn& = m_oDatabase.SQLSelect( _
                ''                        sSQL:=sSQL, _
                ''                        sSQLName:="GetRisk", _
                ''                        bStoredProcedure:=False, _
                ''                        vResultArray:=vResultArray)
                '        If (m_lReturn& <> PMTrue) Then
                '            CopyRisk = PMFalse
                '            Exit Function
                '        End If
                '
                '        ' We have a value?
                '        If (informations.IsArray(vResultArray) = True) Then
                '            lNewRiskID = vResultArray(0, 0)
                '        Else
                '            ' Need a new risk too
                '            CopyRisk = PMFalse
                '            Exit Function
                '        End If
                '
                ' Call the GIS with RiskID
                m_lReturn = CopyDataSet(v_sDataModelCode:=sDataModelCode, r_lNewGISPolicyLinkID:=lNewGISPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_vOldGISPolicyLinkId:=vOldGISPolicyLinkID, v_vOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_vNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vOldRiskID:=lOldRiskID, v_vNewRiskID:=lNewRiskID)

                m_lReturn = GIS_LoadFromDB(sDataModelCode, CInt(vRiskArrayNew(ACRRiskFolderCnt, 0)), vOldGISPolicyLinkID, CInt(vRiskArrayNew(0, 0))) 'copy GIS details to NewInsuranceFileCnt


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup START
                '        ' Get the new RiskID too
                '        sSQL = ""
                '        sSQL = sSQL & "SELECT risk_cnt" & vbNewLine
                '        sSQL = sSQL & "FROM insurance_file_risk_link" & vbNewLine
                '        sSQL = sSQL & "WHERE insurance_file_cnt = {insurance_file_cnt}"
                '
                '        m_oDatabase.Parameters.Clear
                '
                '        m_lReturn& = m_oDatabase.Parameters.Add( _
                ''                        sName:="insurance_file_cnt", _
                ''                        vValue:=v_lNewInsuranceFileCnt, _
                ''                        idirection:=PMParamInput, _
                ''                        iDataType:=PMLong)
                '        If (m_lReturn& <> PMTrue) Then
                '            CopyRisk = PMFalse
                '            Exit Function
                '        End If
                '
                '        ' Call the SQL
                '        m_lReturn& = m_oDatabase.SQLSelect( _
                ''                        sSQL:=sSQL, _
                ''                        sSQLName:="GetRisk", _
                ''                        bStoredProcedure:=False, _
                ''                        vResultArray:=vResultArray)
                '        If (m_lReturn& <> PMTrue) Then
                '            CopyRisk = PMFalse
                '            Exit Function
                '        End If
                '
                '        ' We have a value?
                '        If (informations.IsArray(vResultArray) = True) Then
                '            lNewRiskID = vResultArray(0, 0)
                '        Else
                '            ' Need a new risk too
                '            CopyRisk = PMFalse
                '            Exit Function
                '        End If
                '
                ' Call the GIS with RiskID
                m_lReturn = CopyDataSet(v_sDataModelCode:=sDataModelCode, r_lNewGISPolicyLinkID:=lNewGISPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_vOldGISPolicyLinkId:=vOldGISPolicyLinkID, v_vOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_vNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vOldRiskID:=lOldRiskID, v_vNewRiskID:=lNewRiskID)

                ' Initialise the Data Set with the Object/Properties
                m_lReturn = LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'RWH(28/02/2001)
                m_lReturn = GIS_SaveToDB(v_sGisDataModelCode:=sDataModelCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get Policy Binder Ids
                m_lReturn = GetPolicyBinderId(v_sDataModelCode:=sDataModelCode, v_lGISPolicyLinkId:=lNewGISPolicyLinkID, r_lPolicyBinderId:=lNewPolicyBinderId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'developer guide no. 98
                m_lReturn = GetPolicyBinderId(v_sDataModelCode:=sDataModelCode, v_lGISPolicyLinkId:=vOldGISPolicyLinkID, r_lPolicyBinderId:=lOldPolicyBinderId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CopyRiskStandardWordings(v_lOldPolicyBinderId:=lOldPolicyBinderId, v_lNewPolicyBinderId:=lNewPolicyBinderId, v_sDataModelCode:=sDataModelCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'KNCMGRISK End
                'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup END

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyPolicyForEdit
    '
    ' Description:
    '
    ' History: 18/11/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function CopyPolicyForEdit(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oObject As bSIRInsuranceFile.Services
        'Dim oObject As bSIRInsuranceFile.Services
        Dim sSQL As String = ""
        Dim vStatus As Object = Nothing
        Dim vArray(,) As Object = Nothing
        Dim lVersion As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            oObject = New bSIRInsuranceFile.Services
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oObject.InsuranceFileCnt = v_lOldInsuranceFileCnt


            m_lReturn = oObject.GetDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse


                oObject.Dispose()
                oObject = Nothing

                Return result
            End If

            'Now we get the current highest version number

            sSQL = "SELECT MAX(policy_version)" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM insurance_file" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE insurance_folder_cnt = " & oObject.InsuranceFolderCnt & Strings.ChrW(13) & Strings.ChrW(10)

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyVersion", bStoredProcedure:=False, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                lVersion = CInt(vArray(0, 0))
            Else
                lVersion = 0
            End If


            vArray = Nothing



            vStatus = oObject.InsuranceFileStatus


            oObject.InsuranceFileStatus = "REP"


            m_lReturn = oObject.UpdatePolicy()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse


                oObject.Dispose()
                oObject = Nothing

                Return result
            End If

            'Don't keep the old status - it only leads to confusion when the old version
            'is in debug
            '    oObject.InsuranceFileStatus = vStatus


            oObject.InsuranceFileStatus = Nothing


            oObject.PolicyVersion = lVersion + 1


            oObject.EventDescription = "Edit Policy"


            m_lReturn = oObject.CreatePolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oObject.Dispose()
                oObject = Nothing
                Return result
            End If


            r_lNewInsuranceFileCnt = oObject.InsuranceFileCnt


            oObject.Dispose()

            oObject = Nothing

            'Tomo19072001 - Replace status flag with U, as it's unchanged on this version...
            'Now we copy the insurance file link records
            sSQL = "INSERT INTO insurance_file_risk_link (" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "risk_cnt," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "status_flag, " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "original_risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "SELECT " & CStr(r_lNewInsuranceFileCnt) & "," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "risk_cnt," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "'U'," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "original_risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM insurance_file_risk_link" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE insurance_file_cnt = " & CStr(v_lOldInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            '           "AND status_flag <> 'D'" & vbCrLf

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CopyInsuranceFileRiskLink", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CopyCoinsurance(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CopySubAgent(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyForEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyForEdit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyCoinsurance
    '
    ' Description:
    '
    ' History: 13/06/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CopyCoinsurance(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyCoinsuranceSQL, sSQLName:=ACCopyCoinsuranceName, bStoredProcedure:=ACCopyCoinsuranceStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CopySubAgent
    '
    ' Description:
    '
    ' History: 13/06/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CopySubAgent(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopySubAgentSQL, sSQLName:=ACCopySubAgentName, bStoredProcedure:=ACCopySubAgentStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyPolicyStandardWordings
    '
    ' Description:
    '
    ' History: 23/02/2001 RWH - Created (in bSIRRenSelection)
    '        : 10/08/2001 Tom - Liberated and amended somewhat
    '
    ' ***************************************************************** '
    Public Function CopyPolicyStandardWordings(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_insurance_file_cnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyPolicyStandardWordingsSQL, sSQLName:=ACCopyPolicyStandardWordingsName, bStoredProcedure:=ACCopyPolicyStandardWordingsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckInRenewal
    '
    ' Description:
    '
    ' History: 09/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function CheckInRenewal(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRenewalStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lRenewalStatus = -1

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckRenewalsSQL, sSQLName:=ACCheckRenewalsName, bStoredProcedure:=ACCheckRenewalsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If


            r_lRenewalStatus = CInt(vArray(0, 0))

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRenewalStatus
    '
    ' Description:
    '
    ' History: 09/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateRenewalStatus(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The value here is known - it's 'Policy details changed'
            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_status_type_id", vValue:=CStr(4), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRenewalStatusSQL, sSQLName:=ACUpdateRenewalStatusName, bStoredProcedure:=ACUpdateRenewalStatusStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CalcCombinedKey
    '
    ' Description: This is a wrapper to the component services function
    ' to derive the Unique Invariant Key from source and entity ID.
    '
    ' ***************************************************************** '
    Public Function calccombinedkey(ByVal v_lSourceID As Integer, ByVal v_lKeyID As Integer, ByRef r_lCombinedKeyID As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=v_lSourceID, v_lKeyID:=v_lKeyID, r_lCombinedKeyID:=r_lCombinedKeyID)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalcCombinedKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalcCombinedKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetDefaultSearchFields
    '
    ' Description:  Populate search fields from supplied ID's
    '
    ' ***************************************************************** '
    Public Function SetDefaultSearchFields(ByRef r_sInsRef As String, ByRef r_sShortName As String, Optional ByVal v_lInsuranceFileCnt As Object = Nothing, Optional ByVal v_lInsuranceHolderCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If (Not informations.IsNothing(v_lInsuranceFileCnt)) And (Not Object.Equals(v_lInsuranceFileCnt, Nothing)) Then
                With m_oDatabase
                    .Parameters.Clear()

                    ' Add InsuranceFileCnt as INPUT

                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(CInt(v_lInsuranceFileCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    sSQL = "SELECT insurance_ref FROM Insurance_File WHERE insurance_file_cnt = {insurance_file_cnt}"

                    m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetInsRefFromCnt", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Carry on without default set
                    End If

                    If .Records.Count() = 1 Then
                        'developer guide no. 111
                        r_sInsRef = .Records.Item(0).Fields()("insurance_ref")
                    End If
                End With
            End If



            If (Not informations.IsNothing(v_lInsuranceHolderCnt)) And (Not Object.Equals(v_lInsuranceHolderCnt, Nothing)) Then
                With m_oDatabase
                    .Parameters.Clear()

                    ' Add InsuranceFileCnt as INPUT

                    m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(CInt(v_lInsuranceHolderCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    sSQL = "SELECT shortname FROM Party WHERE party_cnt = {party_cnt}"

                    m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetShortnameFromCnt", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Carry on without default set
                    End If

                    If .Records.Count() = 1 Then
                        'developer guide no. 111
                        r_sShortName = .Records.Item(0).Fields()("shortname")
                    End If
                End With
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetDefaultSearchFieldsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDefaultSearchFields", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' History:  14/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '************************************************************************************
    ' Name : GetAllPolicyVersion
    '
    ' Desc : get all versions of policy
    '
    ' Hist : 26/02/2001 Created - Tinny
    '************************************************************************************
    'WPR 33-75 added
    Public Function GetAllPolicyVersion(ByRef r_vResultArray(,) As Object, ByVal v_lInsuranceFolderCnt As Integer, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lNonTempPolicies As Integer = gPMConstants.PMEReturnCode.PMFalse, Optional ByVal v_lfilterBackdatedVersions As Integer = 0, Optional ByVal v_lViaClientManager As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lfilterCancellationQuote As Integer

            If m_sTransactionType = "MTC" Then
                lfilterCancellationQuote = 1
            End If

            If m_sTransactionType = "MTC" Then
                lfilterCancellationQuote = 1
            End If

            If v_lInsuranceFolderCnt = 0 And v_lInsuranceFileCnt = 0 Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insurance folder count and insurance file count are both zero", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllPolicyVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="InsuranceFolderCnt", vValue:=v_lInsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'MKW250703 PN4271 START 1.8.5 to 1.8.6 Catchup

            'ISS1497 JAS 11/03/03
            'Call one of two different stored procedure depending on wether we want to see
            'Temporary policy versions or not
            If v_lNonTempPolicies = gPMConstants.PMEReturnCode.PMFalse Then

                GetAllPolicyVersion = m_oDatabase.Parameters.Add(sName:="filterBackdatedVersions", vValue:=v_lfilterBackdatedVersions, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If GetAllPolicyVersion <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                GetAllPolicyVersion = m_oDatabase.Parameters.Add(sName:="filterCancellationQuote", vValue:=lfilterCancellationQuote, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If GetAllPolicyVersion <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
                GetAllPolicyVersion = m_oDatabase.Parameters.Add(sName:="ViaClientManager", vValue:=v_lViaClientManager, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If GetAllPolicyVersion <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
                result = m_oDatabase.SQLSelect(sSQL:=ACGetAllPolicyVersionSQL, sSQLName:=ACGetAllPolicyVersionName, bStoredProcedure:=ACGetAllPolicyVersionStored, vResultArray:=r_vResultArray)
            Else
                'use the non temporary policy versions version of the SP
                result = m_oDatabase.SQLSelect(sSQL:=ACGetAllNonTempPolicyVersionSQL, sSQLName:=ACGetAllNonTempPolicyVersionName, bStoredProcedure:=ACGetAllNonTempPolicyVersionStored, vResultArray:=r_vResultArray)

            End If

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllPolicyVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '************************************************************************************
    ' Name : GetCurrentPolicyVersion
    '
    ' Desc : Get the insurance_file_cnt of the current live policy version
    '
    ' Hist : 21/03/2005 CJB - Created for PN19733
    '************************************************************************************
    Public Function GetCurrentPolicyVersion(ByRef r_lCurrentPolicyVersionInsFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, Optional ByVal v_lInsuranceFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lInsuranceFolderCnt = 0 And v_lInsuranceFileCnt = 0 Then
                gPMFunctions.RaiseError("GetCurrentPolicyVersion", "Insurance folder count and insurance file count are both zero", gPMConstants.PMELogLevel.PMLogError)
            End If

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("InsuranceFolderCnt", CStr(v_lInsuranceFolderCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("InsuranceFileCnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'developer guide no. 39
                m_lReturn = .SQLSelect("spu_SIR_Get_Current_Policy_Version", "Select_SelectCurrentPolicyVersion", True, , vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", GetCurrentPolicyVersion, spu_Get_All_Policy_Version failed.")
                End If


                If (Not Informations.IsArray(vResultArray)) Or (CStr(vResultArray(0, 0)) = "") Then
                    Throw New System.Exception(m_lReturn.ToString() + ", GetCurrentPolicyVersion, spu_Get_All_Policy_Version failed.")
                End If


                r_lCurrentPolicyVersionInsFileCnt = CInt(vResultArray(0, 0))

            End With

            Return result
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPolicyVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally

        End Try
        Return result
    End Function

    ' PUBLIC Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: SearchMTAGIIM
    '
    ' Description: SQL Query to Get GIIM policies that are suitable
    '              For MTA's
    '
    ' History : 09/06/2000 Created BSJ
    '           29/09/2000 Added policy type parameter
    ' IDP March 2003 - Branch and merge in GII 1.6 Changes
    ' ***************************************************************** '
    Public Function SearchMTAGIIM(ByRef r_vResultArray(,) As Object, ByVal v_lPartyCnt As Integer, ByVal v_lPolicyTypeId As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vArray(,) As Object = Nothing
        Dim lSchemeTypeFlag As Integer
        Dim sVbsFlag As String = ""
        Dim lLower As Integer

        Try

            ' Innocent until proved
            result = gPMConstants.PMEReturnCode.PMFalse

            ' To improve efficiency 1st retrieve a small subset of rows to feed
            ' into the second query
            sSQL = New StringBuilder(" SELECT " &
                   " insurance_file.insurance_ref, MAX(insurance_file.policy_version) policy_version " &
                   " FROM " &
                   " insurance_file " &
                   " INNER JOIN Insurance_Folder ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt " &
                   " INNER JOIN Party ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt " &
                   " INNER JOIN insurance_file_type IFT on insurance_file.insurance_file_type_id = IFT.insurance_file_type_id " &
                   " WHERE " &
                   " policy_ignore IS NULL " &
                   " AND Insurance_file.policy_type_id = " & CStr(v_lPolicyTypeId) &
                   " AND Party.party_Cnt = " & CStr(v_lPartyCnt) &
                   " AND IFT.code in ('POLICY','MTA PERM')" &
                   " GROUP BY " &
                   " insurance_file.insurance_ref")
            ' NOTE in above syntax : insurance_file_type_id IN (2,5) = search for 'POLICY' or 'MTAPERM' only

            ' Execute SQL Statement - Use array for speed
            With m_oDatabase

                ' Clear parameters
                .Parameters.Clear()

                ' Execute SQL
                m_lError = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

                ' Check error status flag
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIM")

                    Return result
                End If

                ' If NO records were found return PMNotFound
                If Not Informations.IsArray(r_vResultArray) Then


                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End With

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = New StringBuilder("")
            sSQL.Append(" SELECT ")
            sSQL.Append(" Insurance_File.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File_System.last_trans_description insurance_folder_code," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File_type.code type_code," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_Folder.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_Folder.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.product_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Product.code," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File_System.date_created," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File_status.code status_code," & Strings.ChrW(13) & Strings.ChrW(10))
            'RWH(10/04/2001) UW displays agent rather than insurer.
            sSQL.Append(" Party2.shortname agent_name," & Strings.ChrW(13) & Strings.ChrW(10))

            sSQL.Append(" Insurance_File.this_premium," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.policy_type_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Policy_Type.description policy_type," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" Insurance_File.gemini_policy_status," & Strings.ChrW(13) & Strings.ChrW(10))
            ' MSS250701 - Start adding Risk Type Description
            sSQL.Append(" Insurance_File_Type.description type_desc," & Strings.ChrW(13) & Strings.ChrW(10))
            ' MSS250701 - End
            sSQL.Append(" 'NON EDI'," & Strings.ChrW(13) & Strings.ChrW(10))
            'SJ 12/07/2004 - start
            sSQL.Append("' '," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("' '," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("Insurance_File.tax_amount," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("null," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("Insurance_File.alternate_reference," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("Source.underwriting_branch_ind" & Strings.ChrW(13) & Strings.ChrW(10))
            'SJ 12/07/2004 - end

            sSQL.Append(" FROM ")
            sSQL.Append(" Insurance_File INNER JOIN Insurance_File_System ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Insurance_Folder ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Insurance_File_Type ON  Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" LEFT JOIN Insurance_File_Status ON Insurance_File_Status.insurance_file_status_id = Insurance_File.insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Party ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Product ON Product.product_id = Insurance_File.product_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN PMCaption ON PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(" INNER JOIN Policy_Type ON Insurance_file.policy_type_id = Policy_Type.policy_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
            'SJ 12/07/2004 - start
            sSQL.Append(" INNER JOIN Source ON Source.source_id = Insurance_File.source_id" & Strings.ChrW(13) & Strings.ChrW(10))
            'SJ 12/07/2004 - end
            'RWH(10/04/2001) UW displays agent rather than insurer.

            sSQL.Append(" LEFT JOIN Party AS Party2 ON Party2.party_cnt = Insurance_File.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10))

            ' Add the where clause
            sSQL.Append(" WHERE ")

            ' Store the lower bound of the array
            lLower = r_vResultArray.GetLowerBound(1)

            For lRow As Integer = lLower To r_vResultArray.GetUpperBound(1)

                ' Add an 'OR' statement if required
                If lRow > lLower Then

                    sSQL.Append(" OR ")
                End If

                ' Append restriction criteria

                sSQL.Append(" (insurance_file.insurance_ref = '" & CStr(r_vResultArray(0, lRow)) & "'")

                sSQL.Append(" AND insurance_file.policy_version = " & CStr(r_vResultArray(1, lRow)) & ") " & Strings.ChrW(13) & Strings.ChrW(10))
            Next  'lRow

            ' 27/06/00 - Rrestrict to party cnt (gets past bug with duplicate temp ref no's produced for default policies)
            sSQL.Append(" AND Party.party_Cnt = " & v_lPartyCnt)

            ' Add the order by clause
            sSQL.Append(" ORDER BY Insurance_File_System.date_created DESC")

            ' Clear the array from previouse query
            r_vResultArray = Nothing

            ' Execute SQL Statement - use array for speed
            With m_oDatabase

                ' Clear Parameters
                .Parameters.Clear()

                m_lError = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACInsFileFromQueryName, bStoredProcedure:=ACInsFileFromQueryStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

                ' Check error status
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchMTAGIIM")

                    Return result
                End If

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(r_vResultArray) Then


                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End With

            ' Get the GIS data for above query results
            If Not (m_oGISDatabase Is Nothing) Then

                For lRow As Integer = r_vResultArray.GetLowerBound(1) To r_vResultArray.GetUpperBound(1)

                    ' Clear the Database Parameters Collection
                    m_oGISDatabase.Parameters.Clear()

                    ' Add the Insurance File Cnt parameter (INPUT)

                    m_lReturn = m_oGISDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(r_vResultArray(2, lRow)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Check status of flag
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Parameters.Add 2 failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchMTAGIIM")

                        Return result
                    End If

                    ' Execute SQL Statement
                    m_lReturn = m_oGISDatabase.SQLSelect(sSQL:=ACInsRiskDetailsSQL, sSQLName:=ACInsRiskDetailsName, bStoredProcedure:=ACInsRiskDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Gis Select SQL failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchMTAGIIM")

                        Return result
                    End If

                    ' Check a number is being processed
                    If Informations.IsArray(vArray) Then


                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then


                            lSchemeTypeFlag = CInt(vArray(0, 0))

                            'Decode scheme flags
                            m_lReturn = DecodeSchemeFlags(v_lSchemeFlags:=lSchemeTypeFlag, r_sVbsFlag:=sVbsFlag)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                sVbsFlag = ""
                            End If
                        Else

                            sVbsFlag = ""
                        End If


                        Select Case sVbsFlag
                            Case "v", "o"

                                ' edi

                                r_vResultArray(24, lRow) = "EDI"
                        End Select
                    End If
                Next lRow

                vArray = Nothing
            End If

            ' Set return status for function

            ' Exit before error handler
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchMTAGIIM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchMTAGIIM", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' Author        : Ram Chandrabose
    ' Date          : 05-01-2000
    ' Description   : Funtion to get all Data Model Code
    Private Function GetAllDataModelCodes(ByRef vArray(,) As Object, Optional ByRef lSpecificModelIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue


        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="SpecificModelIndex", vValue:=CStr(lSpecificModelIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetdataModelCodesSQL, sSQLName:=ACGetdataModelCodesName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed to get all DataModel Codes", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllDataModelCodes")

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        Return result

    End Function

    ' Author        : Ram Chandrabose
    ' Date          : 08-01-2000
    ' Description   : Funtion to get all Search Results for all Data Model Code
    'developer guide no. 101
    Public Function GetAllGISSearchResults(ByRef sSearchStr As String, ByRef lNoOfRecords As Integer,
                                           ByRef vDataModelsArray(,) As Object,
                                           ByRef vResultArray(,) As Object,
                                           Optional ByRef lSpecificFieldType As Integer = 0,
                                           Optional ByVal v_vAgentGroupCnt As Object = 0,
                                           Optional ByVal v_bAgencyProductOnly As Boolean = False,
                                           Optional ByVal iFileType As Integer = 0,
                                           Optional ByVal bRetrieveAssociates As Boolean = False) As Integer


        Dim nResult As Integer = 0
        Dim NoofFields As Integer
        Dim vTempData(,) As Object = Nothing
        Dim vResultData(,) As Object = Nothing
        Dim sDataModelCode As String = ""
        Dim iMaxRow, iFromRow As Integer
        'Start (Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.2.1)
        Dim lAgentGroupCnt As Integer
        'End (Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.2.1)
        Dim bterminateLooping As Boolean = False
        Dim nNumberOfRecords As Integer = lNoOfRecords

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Start (Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.2.1)
            If Not False Then
                lAgentGroupCnt = v_vAgentGroupCnt
            Else
                lAgentGroupCnt = 0
            End If
            'End (Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.2.1)

            ' Loop thru all Data Model Codes
            For iCounter As Integer = vDataModelsArray.GetLowerBound(1) To vDataModelsArray.GetUpperBound(1) Step 1


                sDataModelCode = CStr(vDataModelsArray(0, iCounter)).Trim()

                ' Clear
                m_oDatabase.Parameters.Clear()

                ' Add the Index parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (DataModel Code)", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="search_object_name", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (Object Name)", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="search_value", vValue:=sSearchStr, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (Search Value)", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="is_insurance_ref_reqd", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (Insurace Ref Required", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Specials_Type_Filter", vValue:=CStr(lSpecificFieldType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (Insurace Ref Required", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Find_Mode", vValue:=CStr(m_lFindMode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (Find_Mode", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Start (Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.2.1)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_group_cnt", vValue:=CStr(lAgentGroupCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (Agent Group Key)", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'End (Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.2.1)

                'WPR 33-75 added
                m_lReturn = m_oDatabase.Parameters.Add(sName:="User_Id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (User Id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="File_Type", vValue:=iFileType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="RetrieveAssociates", vValue:=If(bRetrieveAssociates = True, 1, 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ' Log Error Message
                    bPMFunc.LogMessage(bRetrieveAssociates, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (bRetrieveAssociates", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsLikeIndexGISSearchSQL, sSQLName:=ACInsLikeIndexGISSearchName, bStoredProcedure:=ACInsLikeIndexGISSearchStored, lNumberRecords:=lNoOfRecords, vResultArray:=vTempData)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vTempData) Then
                    ' We have some search results for this data model code.
                    ' So merge the result Array

                    ' Get the no of fields selected

                    NoofFields = vTempData.GetUpperBound(0)

                    If Not Informations.IsArray(vResultData) Then


                        iFromRow = -1
                    Else
                        ' We alreay have some data and we have to merge it with new data

                        iFromRow = vResultData.GetUpperBound(1)

                    End If

                    If Not bterminateLooping Then


                        For iCounter1 As Integer = vTempData.GetLowerBound(1) To vTempData.GetUpperBound(1)
                            iFromRow += 1
                            If nNumberOfRecords <> -1 AndAlso iFromRow >= nNumberOfRecords Then
                                bterminateLooping = True
                                If bterminateLooping Then Exit For
                            End If

                            ReDim Preserve vResultData(NoofFields, iFromRow)

                            For iCounter2 As Integer = 0 To NoofFields
                                vResultData(iCounter2, iFromRow) = vTempData(iCounter2, iCounter1)
                            Next iCounter2

                        Next iCounter1
                    End If
                End If

                ' End If
                If bterminateLooping Then Exit For

            Next iCounter

            ' Set the return Value

            vResultArray = vResultData

            If Not Informations.IsArray(vResultArray) Then nResult = gPMConstants.PMEReturnCode.PMNotFound

            Return nResult

        Catch excep As System.Exception



            ' Error Section.

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllGISSearchResults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' Author        : Ram Chandrabose
    ' Date          : 08-01-2000
    ' Description   : Funtion to get all Policy Details for all the Insurance Refs got from GIS Search Index
    Public Function GetAllPolicyByGISSearchIndex(ByRef vInputData(,) As Object, ByRef vOutputData(,) As Object) As Integer

        Dim result As Integer = 0
        Dim NoofFields, iMaxRow, iFromRow As Integer
        Dim sSQL As New StringBuilder
        Dim vTempData(,) As Object = Nothing
        Dim vResultData(,) As Object = Nothing

        ' Note : We are getting only Six Fields
        Dim vInsuranceFileCnt As String = ""
        Dim vGISPolicyLinkId As String = ""
        Dim vObjectName As String = ""
        Dim vPropertyName As String = ""
        Dim vValue As String = ""
        Dim vInsuranceRef As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get all related data
            For iCounter As Integer = vInputData.GetLowerBound(1) To vInputData.GetUpperBound(1)

                ' Initialise the search Criteria Variables

                vInsuranceFileCnt = CStr(vInputData(0, iCounter))

                vGISPolicyLinkId = CStr(vInputData(1, iCounter))

                vObjectName = CStr(vInputData(2, iCounter))

                vPropertyName = CStr(vInputData(3, iCounter))

                vValue = CStr(vInputData(4, iCounter))

                vInsuranceRef = CStr(vInputData(5, iCounter))

                ' Frame the SQL Statement

                sSQL = New StringBuilder("")
                sSQL.Append("SELECT Insurance_File.insurance_file_id," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File.source_id ins_file_source_id," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_Folder.description insurance_folder_code," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File_type.code type_code," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Party.name insured_name," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Party.shortname insured_shortname," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Party.source_id party_source_id," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File_System.last_modified," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_Folder.insurance_holder_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_Folder.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File.product_id," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Product.code," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" PMCaption.caption," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File.lead_agent_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File_System.date_created," & Strings.ChrW(13) & Strings.ChrW(10))

                ' New 3 Columns
                sSQL.Append("'" & vObjectName & "'" & " Object_Name," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append("'" & vPropertyName & "'" & " Property_Name," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append("'" & vValue & "'" & " Value" & Strings.ChrW(13) & Strings.ChrW(10))

                sSQL.Append(" FROM Insurance_File," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File_System," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_Folder," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Insurance_File_Type," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Party," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" PMCaption," & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" Product" & Strings.ChrW(13) & Strings.ChrW(10))

                sSQL.Append(" WHERE Insurance_File.Insurance_file_cnt = " & vInsuranceFileCnt & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" AND Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" AND Party.party_cnt = Insurance_Folder.insurance_holder_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" AND Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" AND Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" AND ((Insurance_File.insurance_file_status_id IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" OR (Insurance_File.insurance_file_status_id = " & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" (SELECT insurance_file_status_id" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" FROM Insurance_File_Status" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" WHERE code = 'REN')))" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" AND Product.product_id = Insurance_File.product_id" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" AND PMCaption.caption_id = Product.caption_id" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" AND Insurance_file.policy_ignore IS NULL" & Strings.ChrW(13) & Strings.ChrW(10))

                With m_oDatabase
                    ' Clear the Database Parameters Collection
                    .Parameters.Clear()

                    m_lReturn = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetAllPolicyByGISSearchIndex", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTempData)
                End With

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_odatabse.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllPolicyByGISSearchIndex")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vTempData) Then
                    ' We have some search results for this Insurance cnt.
                    ' So merge the result Array

                    ' Get the no of fields selected

                    NoofFields = vTempData.GetUpperBound(0)

                    If Not Informations.IsArray(vResultData) Then


                        vResultData = vTempData
                    Else
                        ' We alreay have some data and we have to merge it with new data

                        iFromRow = vResultData.GetUpperBound(1)


                        iMaxRow = vResultData.GetUpperBound(1) + vTempData.GetUpperBound(1) + 1
                        ReDim Preserve vResultData(NoofFields, iMaxRow)


                        For iCounter1 As Integer = vTempData.GetLowerBound(1) To vTempData.GetUpperBound(1)
                            iFromRow += 1
                            For iCounter2 As Integer = 0 To NoofFields


                                vResultData(iCounter2, iFromRow) = vTempData(iCounter2, iCounter1)
                            Next iCounter2
                        Next iCounter1
                    End If
                End If


            Next iCounter

            ' Set the return Value

            vOutputData = vResultData

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllPolicyByGISSearchIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllPolicyByGISSearchIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UncalcCombinedKey  - 'SJP05072002
    '
    ' Description: This will use the Combined Key (Party Count) to
    '   get the Party Id and source
    ' ***************************************************************** '
    Private Function UncalcCombinedKey(ByRef r_lSourceID As Integer, ByRef r_lKeyID As Integer, ByVal v_lCombinedKeyID As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        r_lSourceID = 0
        r_lKeyID = 0

        If v_lCombinedKeyID < 0 Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Combined KeyID must be greater than zero.", vApp:=ACApp, vClass:=ACClass, vMethod:="UncalcCombinedKey")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        With m_oDatabase
            .Parameters.Clear()

            'DC 11/08/00 was iSourceId%
            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lCombinedKeyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .SQLSelect(sSQL:=ACGetPartyIdFromKeySQL, sSQLName:=ACGetPartyIdFromKeyName, bStoredProcedure:=ACGetPartyIdFromKeyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Party Id and source id could not be retrieved from database.", vApp:=ACApp, vClass:=ACClass, vMethod:="UncalcCombinedKey")

            Return result
        End If

        If Informations.IsArray(vArray) Then

            r_lKeyID = CInt(vArray(0, 0))

            r_lSourceID = CInt(vArray(1, 0))
        Else
            result = m_lReturn
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=CStr(CDbl("Could not find party with party cnt ") + v_lCombinedKeyID), vApp:=ACApp, vClass:=ACClass, vMethod:="UncalcCombinedKey")
            Return result
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name          : AllowRegSearch
    '
    ' Description   : Function to get the flag if the Registration Search Criteria
    '                 is enabled.
    ' Created by    : E Dhalech
    ' Created on    : 05-08-2002
    '
    ' Edit History  :
    ' ED 05082002   : Created
    ' ***************************************************************** '
    Private Function AllowRegSearch() As Boolean

        Dim result As Boolean = False
        Dim sSetting As String = ""

        Try


            m_lReturn = bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTAllowRegSearch, gPMConstants.SIRBCHHeadOffice, sSetting)


            Select Case sSetting.Trim()
                Case "1"
                    Return True
                Case Else
                    Return False
            End Select
        Catch
        End Try



        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowRegSearch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllowRegSearch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result
    End Function


    'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup START
    'KNCMGRISK Start
    ' ***************************************************************** '
    ' Name: GetRisk (Public)
    '
    ' Desc: get all associate risks for policy
    '
    ' ***************************************************************** '
    Public Function GetRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return m_oDatabase.SQLSelect(sSQL:=ACSaaRiskSQL, sSQLName:=ACSaaRiskName, bStoredProcedure:=ACSaaRiskStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateRiskFolder
    '
    ' Description:
    '
    ' History: 25/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateRiskFolder(ByRef vRiskFolderArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()



        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt", vValue:=CStr(vRiskFolderArray(ACRFRiskFolderCnt, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_id", vValue:=CStr(vRiskFolderArray(ACRFRiskFolderId, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(vRiskFolderArray(ACRFSourceId, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_type_id", vValue:=CStr(vRiskFolderArray(ACRFRiskFolderTypeId, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(vRiskFolderArray(ACRFCode, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vRiskFolderArray(ACRFDescription, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(vRiskFolderArray(ACRFInsuranceFolderCnt, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRiskFolderDetailsSQL, sSQLName:=ACInsertRiskFolderDetailsName, bStoredProcedure:=ACInsertRiskFolderDetailsStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If CDbl(vRiskFolderArray(ACRFRiskFolderCnt, 0)) = 0 Then

            vRiskFolderArray(ACRFRiskFolderCnt, 0) = m_oDatabase.Parameters.Item("risk_folder_cnt").Value
        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateRisk
    '
    ' Description:
    '
    ' History: 25/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateRisk(ByRef vRiskArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=CStr(vRiskArray(ACRRiskStatusId, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt", vValue:=CStr(vRiskArray(ACRRiskFolderCnt, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="accumulation_id", vValue:=CStr(vRiskArray(ACRAccumulationId, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vRiskArray(ACRDescription, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="sequence_number", vValue:=CStr(vRiskArray(ACRSequenceNumber, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured_requested", vValue:=CStr(vRiskArray(ACRSumInsuredRequested, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="inception_date", vValue:=CStr(vRiskArray(ACRInceptionDate, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="expiry_date", vValue:=CStr(vRiskArray(ACRExpiryDate, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_not_index_linked", vValue:=CStr(vRiskArray(ACRIsNotIndexLinked, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_accumulated", vValue:=CStr(vRiskArray(ACRIsAccumulated, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_reason_id", vValue:=CStr(vRiskArray(ACRLapsedReasonId, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_date", vValue:=CStr(vRiskArray(ACRLapsedDate, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_description", vValue:=CStr(vRiskArray(ACRLapsedDescription, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="var_data_ref", vValue:=CStr(vRiskArray(ACRVarDataRef, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_sum_insured", vValue:=CStr(vRiskArray(ACRTotalSumInsured, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_annual_premium", vValue:=CStr(vRiskArray(ACRTotalAnnualPremium, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_this_premium", vValue:=CStr(vRiskArray(ACRTotalThisPremium, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_ri_at_risk_level", vValue:=CStr(vRiskArray(ACRIsRiAtRiskLevel, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_auto_reinsured", vValue:=CStr(vRiskArray(ACRIsAutoReinsured, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=CStr(vRiskArray(ACRGISScreenId, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="eml_percentage", vValue:=CStr(vRiskArray(ACREMLPercentage, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRiskDetailsSQL, sSQLName:=ACInsertRiskDetailsName, bStoredProcedure:=ACInsertRiskDetailsStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            vRiskArray(ACRRiskId, 0) = m_oDatabase.Parameters.Item("risk_cnt").Value

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
            'Resume

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateInsuranceFileRiskLink
    '
    ' Description:
    '
    ' History: 25/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateInsuranceFileRiskLink(ByRef vInsuranceFileRiskLinkArray(,) As Object, ByRef iMode As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()


        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileRiskLinkArray(ACIFRLInsuranceFileCnt, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(vInsuranceFileRiskLinkArray(ACIFRLRiskCnt, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="status_flag", vValue:=CStr(vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="original_risk_cnt", vValue:=CStr(vInsuranceFileRiskLinkArray(ACIFRLOriginalRiskCnt, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertInsuranceFileRiskLinkDetailsSQL, sSQLName:=ACInsertInsuranceFileRiskLinkDetailsName, bStoredProcedure:=ACInsertInsuranceFileRiskLinkDetailsStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPolicyBinderId
    '
    ' Description:
    '
    ' History: 23/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyBinderId(ByVal v_sDataModelCode As String, ByVal v_lGISPolicyLinkId As Integer, ByRef r_lPolicyBinderId As Integer) As Integer

        Dim result As Integer = 0
        Dim sPolicyBinderTable, sIdName, sSQL As String
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sDataModelCode = v_sDataModelCode.Trim()

            If v_sDataModelCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Data Model passed in", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyBinderId", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Build up table & id names using DataModelCode.
            sPolicyBinderTable = v_sDataModelCode & "_Policy_binder"
            sIdName = sPolicyBinderTable & "_id"

            sSQL = "SELECT " & sIdName & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM " & sPolicyBinderTable & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE gis_policy_link_id = " & CStr(v_lGISPolicyLinkId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyBinderId", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_lPolicyBinderId = CInt(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyBinderId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyBinderId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: CopyRiskStandardWordings
    '
    ' Description:
    '
    ' History: 23/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function CopyRiskStandardWordings(ByVal v_lOldPolicyBinderId As Integer, ByVal v_lNewPolicyBinderId As Integer, ByVal v_sDataModelCode As String) As Integer

        Dim result As Integer = 0
        Dim sWordingTable, sPolicyBinderIdName As String
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim oDocTemplate As Object = Nothing
        Dim lDocumentTemplateID As Integer

        Const ACPosSequenceID As Integer = 0
        Const ACPosDocTemplateID As Integer = 1
        Const ACPosPropertyID As Integer = 2
        Const ACPosObjectID As Integer = 3
        Const ACPosChild As Integer = 4
        'Const ACPosCopyOfOriginal As Integer = 5

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sDataModelCode = v_sDataModelCode.Trim()
            sWordingTable = v_sDataModelCode & "_standard_wording"
            sPolicyBinderIdName = v_sDataModelCode & "_Policy_binder_id"

            sSQL = "SELECT SW.sequence_id, SW.document_template_id, SW.gis_property_id, SW.gis_object_id, "
            sSQL = sSQL & "SW.child, DT.copy_of_original" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM " & sWordingTable & " SW" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "INNER JOIN Document_Template DT ON DT.document_template_id=SW.document_template_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE SW." & sPolicyBinderIdName & " = " & CStr(v_lOldPolicyBinderId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRiskStandardWordings", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", Select statement failed.")

            End If

            sSQL = ""

            If Informations.IsArray(vResultArray) Then

                For iCount As Integer = 0 To vResultArray.GetUpperBound(1)

                    lDocumentTemplateID = gPMFunctions.ToSafeLong(vResultArray(ACPosDocTemplateID, iCount), 0)

                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="data_model", vValue:=v_sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="new_policy_binder", vValue:=CStr(v_lNewPolicyBinderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="old_policy_binder", vValue:=CStr(v_lOldPolicyBinderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_prop_id", vValue:=CStr(gPMFunctions.ToSafeLong(vResultArray(ACPosPropertyID, iCount))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_obj_id", vValue:=CStr(gPMFunctions.ToSafeLong(vResultArray(ACPosObjectID, iCount))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="doc_template_id", vValue:=CStr(lDocumentTemplateID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="seq_id", vValue:=CStr(gPMFunctions.ToSafeLong(vResultArray(ACPosSequenceID, iCount))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="isChild", vValue:=CStr(gPMFunctions.ToSafeInteger(vResultArray(ACPosChild, iCount), CInt("0"))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'WPR 33-75 added
                    If ToSafeInteger(vResultArray(ACPosChild, iCount), 0) = 1 Then

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="ChildId", vValue:=CStr(ToSafeLong(vResultArray(ACPosDocTemplateID, iCount), 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Execute SP
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRiskStandardWordingsSQL, sSQLName:=ACCopyRiskStandardWordingsName, bStoredProcedure:=ACCopyRiskStandardWordingsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", " + +", Insert statement failed.")
                    End If

                Next iCount

                If Not (oDocTemplate Is Nothing) Then

                    oDocTemplate.Dispose()
                    oDocTemplate = Nothing
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetGISPolicyLink (Private)
    '
    ' Desc: get details from gis policy link table using InsuranceFileCnt and RiskID
    '
    ' ***************************************************************** '
    Public Function GetGISPolicyLink(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(20/11/2000) We are using file_cnt field to hold folder_cnt (are you scared yet!!!)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="quote_ref", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(v_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.SQLSelect(sSQL:=ACSelGisPolicyLinkSQL, sSQLName:=ACSelGisPolicyLinkName, bStoredProcedure:=ACSelGisPolicyLinkStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISPolicyLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISPolicyLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: LoadFromDB
    '
    ' Description:
    '
    ' RFC160300 - DataModel code param added to LoadFromDB call.
    ' RFC111000 - Able to Specify a RiskID when creating/loading a dataset
    ' ***************************************************************** '
    Public Function GIS_LoadFromDB(ByVal v_sGisDataModelCode As String, Optional ByRef r_vInsuranceFileCnt As Object = 0, Optional ByRef r_vPolicyLinkID As Object = "", Optional ByRef r_vRiskID As Object = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSetDef As Object
        Dim sXMLDataSet As Object
        Dim oGIS As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data From the Database in XML
            ' RFC160300 - DataModel code param added to LoadFromDB call.
            ' Create the GIS
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = oGIS.LoadFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_sGisDataModelCode:=ToSafeString(v_sGisDataModelCode), r_vInsuranceFileCnt:=r_vInsuranceFileCnt, r_vPolicyLinkID:=r_vPolicyLinkID, r_vRiskID:=r_vRiskID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load Data as XML
            lReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create a new component services

        ' New instance of bSIRIUSLink
        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=v_sClassName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of " & v_sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        End If


        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: LoadFromXML
    '
    ' Description:
    '
    ' History: 24/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function LoadFromXML(ByVal v_sXMLDataSetDef As String, ByVal v_sXMLDataSet As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Return m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=v_sXMLDataSetDef, v_sXMLDataSet:=v_sXMLDataSet)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GIS_SaveToDB
    '
    ' Description: Duplicates functionality of iGIS.SaveToDB so we can
    '               call straight thru' to the business object.
    '
    ' History: 20/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GIS_SaveToDB(ByVal v_sGisDataModelCode As String) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef As String = String.Empty
        Dim sXMLDataSet As String = String.Empty
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oGIS As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the GIS
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data as an XML String
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save it to the DataBase

            lReturn = oGIS.SaveToDB(v_sGisDataModelCode:=ToSafeString(v_sGisDataModelCode), r_sXMLDataSet:=ToSafeString(sXMLDataSet))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the Saved to DB Results
            lReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIS_SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIS_SaveToDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateRisk
    '
    ' Description:
    '
    ' History: 05/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function CreateRisk(ByRef vRiskDetails(,) As Object, ByRef vRiskTypeArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vProductArray(,) As Object
        Dim vRiskFolderArray(,) As Object
        Dim vInsuranceFileRiskLinkArray(,) As Object

        'New logic for endorsements
        'If we don't find a risk record for the passed cnt (which would then be 0) we
        'proceed as normal, creating a risk folder record, a risk record, and an insurance file
        'risk link record
        'If we do find a record and the status flag is "C" we can exit
        'If we do find a record and the status flag is "U" we need to delete the existing
        'insurance file risk link record, create a new risk record, and create a new insurance
        'file risk link record

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            vRiskDetails = Nothing
            vRiskTypeArray = Nothing
            vProductArray = Nothing

            vRiskFolderArray = Nothing

            vInsuranceFileRiskLinkArray = Nothing

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(m_lRiskId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDetailsSQL, sSQLName:=ACGetRiskDetailsName, bStoredProcedure:=ACGetRiskDetailsStored, vResultArray:=vRiskDetails, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt", vValue:=CStr(vRiskDetails(ACRRiskFolderCnt, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'KN (CMG) spe_Risk_Group_sel
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskFolderDetailsSQL, sSQLName:=ACGetRiskFolderDetailsName, bStoredProcedure:=ACGetRiskFolderDetailsStored, vResultArray:=vRiskFolderArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'So we must create the risk record, but first we need to create the risk folder

            vRiskFolderArray(ACRFRiskFolderCnt, 0) = 0

            vRiskFolderArray(ACRFRiskFolderId, 0) = 0
            'vRiskFolderArray(ACRFSourceId, 0) = m_iSourceId
            'vRiskFolderArray(ACRFRiskFolderTypeId, 0) = 1
            'vRiskFolderArray(ACRFCode, 0) = ""
            'vRiskFolderArray(ACRFDescription, 0) = vRiskTypeArray(ACRTDescription, 0)

            vRiskFolderArray(ACRFInsuranceFolderCnt, 0) = m_lInsuranceFolderCnt
            'spe_risk_folder_add KN (CMG)

            m_lReturn = UpdateRiskFolder(vRiskFolderArray:=vRiskFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lRiskId = 0


            vRiskDetails(ACRRiskId, 0) = 0


            vRiskDetails(ACRRiskFolderCnt, 0) = vRiskFolderArray(ACRFRiskFolderCnt, 0)
            'spe_risk_add KN(CMG)

            m_lReturn = UpdateRisk(vRiskArray:=vRiskDetails)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lRiskId = CInt(vRiskDetails(ACRRiskId, 0))

            ReDim vInsuranceFileRiskLinkArray(ACIFRLOriginalRiskCnt, 0)


            vInsuranceFileRiskLinkArray(ACIFRLInsuranceFileCnt, 0) = m_lInsuranceFileCnt

            vInsuranceFileRiskLinkArray(ACIFRLRiskCnt, 0) = m_lRiskId

            vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0) = "C"


            vInsuranceFileRiskLinkArray(ACIFRLOriginalRiskCnt, 0) = DBNull.Value

            m_lReturn = UpdateInsuranceFileRiskLink(vInsuranceFileRiskLinkArray:=vInsuranceFileRiskLinkArray, iMode:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            vProductArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function
    'KNCMGRISK End
    'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup END

    ' ***************************************************************** '
    '
    ' Name: AllowOtherBranchesToViewPolicies
    '
    ' Description:
    '
    ' History: 08/04/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function AllowOtherBranchesToViewPolicies(ByRef r_vAllowBranchesArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOption92SQL, sSQLName:=ACGetOption92Name, bStoredProcedure:=ACGetOption92Stored, vResultArray:=r_vAllowBranchesArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowOtherBranchesToViewPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllowOtherBranchesToViewPolicies", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    Public Function GetPartyType(ByVal v_lPartyCnt As Integer, ByRef r_sPartyType As String) As Integer
        Dim result As Integer = 0
        Try

            Dim vArray(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="partycnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyTypeSQL, sSQLName:=ACGetPartyTypeName, bStoredProcedure:=ACGetPartyTypeStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                r_sPartyType = CStr(vArray(2, 0)).Trim()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetVersionsByDate
    '
    ' Description:
    '
    ' History: 22/05/2000 Tomo - Created.
    '          03/01/2003 SJ - Add v_bAllowBackdating,
    '                          v_bIsReinstatement and r_vAffectedInsuranceFileCnts
    ' RAW 13/11/2003 : CQ1765 : added v_bIsPermanentMTA param
    ' RAW 29/04/2004 : CQ5139 : added r_vResultArray param
    ' ***************************************************************** '

    'Replaced Optional ByVal v_lMTAType As Variant = 0 with Optional ByVal v_bIsPermanentMTA As Boolean = False


    Public Function GetVersionsByDate(ByRef r_lInsuranceFileCnt As Integer,
                                      ByVal v_dtStartDate As Date,
                                      ByRef r_lPolicyVersion As Integer,
                                      ByRef r_lErrorCode As Integer,
                                      Optional ByVal v_lInsuranceFolderCnt As Integer = 0,
                                      Optional ByRef r_bBackdatingRequired As Boolean = False,
                                      Optional ByVal v_bIsReinstatement As Boolean = False,
                                      Optional ByVal v_bIsCancellation As Boolean = False,
                                      Optional ByRef r_vAffectedInsuranceFileCnts As Object = Nothing,
                                      Optional ByRef v_lDeletedRiskInsuranceFileCnt As Integer = 0,
                                      Optional ByVal v_lMTAType As Object = 0,
                                      Optional ByRef r_vResultArray(,) As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing
        Dim lPolicyTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If r_lInsuranceFileCnt > 0 Then
                sSQL = "SELECT policy_type_id FROM insurance_file WHERE insurance_file_cnt = " & r_lInsuranceFileCnt
            Else
                sSQL = "SELECT policy_type_id " &
                       "FROM insurance_file i, insurance_folder f " &
                       "Where i.insurance_folder_cnt = f.insurance_folder_cnt " &
                       "AND f.insurance_folder_cnt =  " & v_lInsuranceFolderCnt &
                       " GROUP BY policy_type_id "
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyTypeId", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                lPolicyTypeId = CInt(vArray(0, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case lPolicyTypeId
                Case PMBConst.PMBPolicyTypeUnderwriting
                    ' RAW 13/11/2003 : CQ1765 : added v_bIsPermanentMTA param
                    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - Allow Return Premium.doc) - (5.2.1.1)
                    'Not mentioned in the spec.  But Refering the definition of the function
                    'Replaced v_bIsPermanentMTA:=v_bIsPermanentMTA with v_lMTAType:=kMTATypePermanentAndTemporary
                    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - Allow Return Premium.doc) - (5.2.1.1)

                    m_lReturn = GetUnderwritingVersionsByDate(r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_dtStartDate:=v_dtStartDate, r_lPolicyVersion:=r_lPolicyVersion, r_lErrorCode:=r_lErrorCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_bBackdatingRequired:=r_bBackdatingRequired, v_bIsReinstatement:=v_bIsReinstatement, v_bIsCancellation:=v_bIsCancellation, r_vAffectedInsuranceFileCnts:=r_vAffectedInsuranceFileCnts, v_lDeletedRiskInsuranceFileCnt:=v_lDeletedRiskInsuranceFileCnt, v_lMTAType:=v_lMTAType)


                Case Else
                    m_lReturn = GetOtherVersionByDate(r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_dtStartDate:=v_dtStartDate, r_lPolicyVersion:=r_lPolicyVersion, r_lErrorCode:=r_lErrorCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If r_lErrorCode = 0 And r_lInsuranceFileCnt <> 0 Then
                With m_oDatabase
                    ' Clear the Database Parameters Collection
                    .Parameters.Clear()
                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=r_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception
                    End If

                    ' Execute SQL Statement
                    m_lReturn = .SQLSelect(sSQL:=ACSelectInsuranceFileSQL, sSQLName:=ACSelectInsuranceFileName, bStoredProcedure:=ACSelectInsuranceFileStored, vResultArray:=vArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception
                    End If

                    r_vResultArray = vArray
                End With
            End If

            obj_m_vMTAInsuranceFileLinkArray = m_vMTAInsuranceFileLinkArray

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionsByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionsByDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function
    ''' <summary>
    ''' GetUnderwritingVersionsByDate
    ''' </summary>
    ''' <param name="r_lInsuranceFileCnt"></param>
    ''' <param name="v_dtStartDate"></param>
    ''' <param name="r_lPolicyVersion"></param>
    ''' <param name="r_lErrorCode"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="r_bBackdatingRequired"></param>
    ''' <param name="v_bIsReinstatement"></param>
    ''' <param name="v_bIsCancellation"></param>
    ''' <param name="r_vAffectedInsuranceFileCnts"></param>
    ''' <param name="v_lDeletedRiskInsuranceFileCnt"></param>
    ''' <param name="v_lMTAType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUnderwritingVersionsByDate(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_bBackdatingRequired As Boolean = False, Optional ByVal v_bIsReinstatement As Boolean = False, Optional ByVal v_bIsCancellation As Boolean = False, Optional ByRef r_vAffectedInsuranceFileCnts As Object = Nothing, Optional ByRef v_lDeletedRiskInsuranceFileCnt As Integer = 0, Optional ByVal v_lMTAType As Object = 0) As Integer


        Dim nResult As Integer
        Dim vArray(,) As Object = Nothing
        Dim dtCoverStartDate As Date
        Dim dtExpiryDate As Date
        Dim dtExpiryDateToUse As Date
        Dim dtEffectiveExpiryDate As Date
        Dim nUbound As Integer
        Dim nThisOne As Integer
        Dim sType As String
        Dim sStatus As String
        Dim nAffectedInsuranceFileCnts As Integer
        Dim bProcess As Boolean
        Dim bAddToArray As Boolean
        Dim nTypeInd As Integer
        Dim nMTAInsuranceFileLinkCnt As Integer
        Dim nInsuranceFileCnt As Integer

        Dim m_sDisableTempMTA As String = String.Empty
        Const AC1InsuranceFileCnt As Integer = 0
        Const AC1PolicyVersion As Integer = 1
        'Const AC1LastTransDate As Integer = 2
        Const AC1InsFileTypeCode As Integer = 3
        Const AC1CoverStartDate As Integer = 4
        Const AC1ExpiryDate As Integer = 5
        Const AC1InsFileStatusCode As Integer = 6
        Const AC1InsuranceFileStatusId As Integer = 7
        'Const AC1OriginalExpiryDate As Integer = 8

        Const AC2InsuranceFileCnt As Integer = 0
        Const AC2CoverStartDate As Integer = 1
        Const AC2PolicyVersion As Integer = 2
        Const AC2InsFileType As Integer = 3
        Const AC2ExpiryDate As Integer = 4
        Const AC2InsFileStatus As Integer = 5
        Const AC2ArraySize As Integer = 5
        Dim sSQL As StringBuilder


        nResult = gPMConstants.PMEReturnCode.PMTrue

        'If we're underwriting, pass if we're permanent or not
        'The things we do to avoid breaking binary compatibility
        ' RAW 13/11/2003 : CQ1765 : v_bIsPermanentMTA has now been added as a param but this has been kept for backward compatibility
        If r_lErrorCode = 1 Then
            v_lMTAType = gPMConstants.kMTATypePermanent
        End If

        r_lErrorCode = 0

        If v_lInsuranceFolderCnt = 0 Then
            'Modifying the inline query to make it compatible with SQL server 2005
            sSQL = New StringBuilder
            sSQL.Append("SELECT ifi.insurance_file_cnt, ")
            sSQL.Append("ifi.policy_version, ")
            sSQL.Append("ifs.last_trans_date, ")
            sSQL.Append("ift.code, ")
            sSQL.Append("ifi.cover_start_date,")
            sSQL.Append("ifi.expiry_date,")
            sSQL.Append("ifst.code,")
            sSQL.Append("ifi.insurance_file_status_id ")
            sSQL.Append(" FROM insurance_file ifi ")
            sSQL.Append(" INNER JOIN insurance_file_system ifs ")
            sSQL.Append(" ON ifi.insurance_file_cnt = ifs.insurance_file_cnt ")
            sSQL.AppendLine()
            sSQL.Append(" AND ifi.policy_ignore is null ")
            If r_lInsuranceFileCnt > 0 Then
                sSQL.Append(" AND ifi.insurance_ref = (select insurance_ref from insurance_file where insurance_file_cnt= " & r_lInsuranceFileCnt & ")" & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            sSQL.Append(" INNER JOIN insurance_file_type ift ")
            sSQL.Append(" ON ifi.insurance_file_type_id = ift.insurance_file_type_id ")
            sSQL.AppendLine()
            sSQL.Append(" INNER JOIN insurance_file ifi2 ")
            sSQL.Append(" ON ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt ")
            sSQL.AppendLine()
            sSQL.Append(" AND ifi2.insurance_file_cnt = " & r_lInsuranceFileCnt)
            sSQL.AppendLine()
            sSQL.Append(" LEFT OUTER JOIN insurance_file_status ifst ")
            sSQL.Append(" ON ifi.insurance_file_status_id = ifst.insurance_file_status_id ")

        Else
            'Modifying the inline query to make it compatible with SQL server 2005
            sSQL = New StringBuilder
            sSQL.Append("SELECT DISTINCT ifi.insurance_file_cnt,")
            sSQL.Append("ifi.policy_version,")
            sSQL.Append("ifs.last_trans_date,")
            sSQL.Append("ift.code,")
            sSQL.Append("ifi.cover_start_date,")
            sSQL.Append("ifi.expiry_date,")
            sSQL.Append("ifst.code,")
            sSQL.Append("ifi.insurance_file_status_id")
            sSQL.AppendLine()
            sSQL.Append(" FROM insurance_file ifi ")
            sSQL.Append(" INNER JOIN insurance_file_system ifs ")
            sSQL.Append(" ON ifi.insurance_file_cnt = ifs.insurance_file_cnt ")
            sSQL.Append(" AND ifi.policy_ignore is null ")
            sSQL.AppendLine()
            sSQL.Append(" INNER JOIN insurance_file_type ift ")
            sSQL.Append(" ON ifi.insurance_file_type_id = ift.insurance_file_type_id ")
            sSQL.AppendLine()
            sSQL.Append(" INNER JOIN product ")
            sSQL.Append(" ON ifi.product_id = product.product_id ")
            sSQL.AppendLine()
            If v_bIsReinstatement Then
                sSQL.Append(" INNER JOIN mta_insurance_file_link ifl ")
                sSQL.Append(" ON ifi.insurance_file_cnt = ifl.cancelled_linked_insurance_file_cnt  OR (ifi.insurance_file_cnt = ifl.insurance_file_cnt AND ifl.cancelled_linked_insurance_file_cnt = 0)")
                sSQL.Append(" AND ifl.type_ind = 1 ")
                If m_bBackDatedMTA = True Then
                    sSQL.Append(" INNER JOIN (SELECT  MAX(Base_Insurance_File_Cnt) Base_Insurance_File_Cnt FROM insurance_file WHERE insurance_file_type_id=8 AND insurance_folder_cnt=" & v_lInsuranceFolderCnt)
                    sSQL.Append(" ) i ON i.Base_Insurance_File_Cnt = ifl.insurance_file_cnt")
                End If
                sSQL.Append(" AND ifl.processed_ind = 0 ")
                sSQL.Append(" AND ifi.insurance_file_type_id = 8 ")
            End If

            sSQL.Append(" LEFT OUTER JOIN insurance_file_status ifst ")
            sSQL.Append(" ON ifi.insurance_file_status_id = ifst.insurance_file_status_id ")
            sSQL.Append(" WHERE  ifi.insurance_folder_cnt = " & v_lInsuranceFolderCnt)
            sSQL.Append(" AND ISNull(ifi.out_of_sequence_replaced, 0) <> 1 ")

        End If

        If m_sCallingAppName = "iACTCreditControlProcessing" Or m_sCallingAppName = "CreditControlCLI" Or m_sCallingAppName = "SiriusImport" Then
            sSQL.Append("ORDER BY ifi.cover_start_date desc, ifi.insurance_file_cnt desc ")
        Else
            sSQL.Append("ORDER BY ifi.insurance_file_cnt,ifi.cover_start_date ")
        End If
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString, sSQLName:="GetUnderwritingVersionsByDate", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=CInt("5116"), r_sOptionValue:=m_sDisableTempMTA)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetUnderwritingVersionsByDate = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If Not Informations.IsArray(vArray) Then
            r_lErrorCode = 3
            Return nResult
        End If


        nUbound = vArray.GetUpperBound(1)
        If m_sCallingAppName = "iACTCreditControlProcessing" Or m_sCallingAppName = "CreditControlCLI" Or m_sCallingAppName = "SiriusImport" Then
            r_lInsuranceFileCnt = vArray(ACIInsFileId, 0)
        End If

        'First let's validate - get out when there's an error
        'Here the rules are: if this is a temporary MTA, the future does not matter
        'If this is a permanent MTA, worry only about permanent stuff
        If nUbound > 0 Then
            For lTemp As Integer = 0 To nUbound

                sType = CStr(vArray(AC1InsFileTypeCode, lTemp))

                If (Informations.IsDBNull(vArray(AC1InsFileStatusCode, lTemp)) Or lTemp = 0) And v_bIsReinstatement = False Then  ' include NB version that has to be first

                    sStatus = ""
                Else

                    sStatus = Trim(CStr(vArray(AC1InsFileStatusCode, lTemp)))
                End If


                nInsuranceFileCnt = CInt(vArray(AC1InsuranceFileCnt, lTemp))

                dtCoverStartDate = CDate(vArray(AC1CoverStartDate, lTemp))

                dtExpiryDate = CDate(vArray(AC1ExpiryDate, lTemp))

                If Trim$(sStatus) = "LAP" And m_sDisableTempMTA = "1" Then
                    If v_lMTAType = kMTATypePermanent Then
                        If v_lDeletedRiskInsuranceFileCnt > 0 Then
                            If nInsuranceFileCnt > v_lDeletedRiskInsuranceFileCnt Then
                                r_bBackdatingRequired = True
                                Exit For
                            End If
                        Else
                            If (dtCoverStartDate > v_dtStartDate) Then
                                'Renewals are ok
                                Select Case Trim$(sType)
                                    Case "RENEWAL", "MTAQUOTE", "MTA TEMP", "MTAQTETEMP", "MTAQCAN"
                                    Case Else
                                        r_bBackdatingRequired = True
                                        Exit For
                                End Select
                            End If
                        End If
                    End If
                End If
                Select Case sStatus.Trim()
                    Case "LAP", "REP"
                        'Ignore cancelled or lapsed policies etc.
                    Case "CAN"
                        If (dtCoverStartDate > v_dtStartDate) And sStatus.ToUpper = "CAN" And v_bIsReinstatement = True Then
                            r_bBackdatingRequired = True
                            Exit For
                        End If
                    Case Else
                        If v_lMTAType = gPMConstants.kMTATypePermanent Then
                            If v_lDeletedRiskInsuranceFileCnt > 0 Then
                                If nInsuranceFileCnt > v_lDeletedRiskInsuranceFileCnt Then
                                    r_bBackdatingRequired = True
                                    Exit For
                                End If
                            Else
                                If dtCoverStartDate > v_dtStartDate Then
                                    Select Case sType.Trim()
                                        Case "RENEWAL", "MTAQUOTE", "MTA TEMP", "MTAQTETEMP", "MTAQREINS", "MTAQCAN"
                                        Case Else
                                            r_bBackdatingRequired = True
                                            Exit For
                                    End Select
                                End If
                            End If
                        End If
                End Select
            Next lTemp
        End If

        'So, this is a valid MTA, we need to decide which one we're working on
        'The rule is, latest start date before passed date, earliest end date after passed date.
        'Note that end dates should be all the same, except for temp MTAs _before_ the new one
        'We can work through the array backwards...
        'It should always find something, but...

        'It should always find something, but...
        nThisOne = -1

        For lTemp As Integer = nUbound To 0 Step -1

            sType = CStr(vArray(AC1InsFileTypeCode, lTemp))

            dtCoverStartDate = CDate(vArray(AC1CoverStartDate, lTemp))
            dtExpiryDate = CDate(vArray(AC1ExpiryDate, lTemp))

            ' set default value of the expiry date that is to be used for matching.
            ' Any exceptions will be set next
            dtExpiryDateToUse = dtExpiryDate

            If (sType.Trim() = "MTA PERM") Or (sType.Trim() = "POLICY") Or (sType.Trim() = "MTAREINS") Then

                If dtEffectiveExpiryDate = Date.MinValue Then
                    dtEffectiveExpiryDate = dtExpiryDate
                ElseIf dtExpiryDate < dtEffectiveExpiryDate Then
                    ' this version has an expiry date earlier than a version that supercedes it
                    ' so save the expiry date from this version in case we need to use it for another version
                    dtEffectiveExpiryDate = dtExpiryDate
                Else
                    ' Coming through CreditControlProcessing, array order is ORDER BY ifi.cover_start_date desc, ifi.insurance_file_cnt desc
                    ' verify if a higher version lies within the Ubound
                    ' if its status is REP, use the current versions expiry date
                    If m_sCallingAppName = "iACTCreditControlProcessing" AndAlso
                        ((lTemp + 1) <= nUbound) AndAlso
                        (CStr(vArray.GetValue(AC1InsFileStatusCode, lTemp + 1)) = "REP") Then

                        dtEffectiveExpiryDate = dtExpiryDate
                    ElseIf m_sCallingAppName = "CreditControlCLI" AndAlso ((lTemp + 1) <= nUbound) Then
                        dtEffectiveExpiryDate = dtExpiryDate
                    Else
                        ' this version has an expiry date later than a version that supercedes it
                        ' so use the expiry date from that version instead
                        dtExpiryDateToUse = dtEffectiveExpiryDate ' RAW 03/11/2004 : CQ6457 : added
                    End If
                End If
            End If


            nInsuranceFileCnt = CInt(vArray(AC1InsuranceFileCnt, lTemp))
            bProcess = True

            If v_bIsCancellation = False _
            And v_bIsReinstatement = False _
            And (v_lMTAType = kMTATypeTemporary Or v_lMTAType = kMTATypePermanentAndTemporary) Then
                'This is a temporary MTA
                bProcess = False
            End If

            If bProcess Then
                nTypeInd = AC3StatusOnly

                bAddToArray = False

                If nThisOne = -1 Then
                    'We haven't yet found the one we are looking for
                    If v_bIsReinstatement Then
                        bAddToArray = True
                    ElseIf (dtCoverStartDate > v_dtStartDate) And (sType.Trim() = "MTA PERM") OrElse (sType.Trim() = "POLICY") OrElse ((sType.Trim) = "MTAREINS") OrElse (Trim$(sType) = "MTACAN") Then
                        bAddToArray = True
                    ElseIf (sType.Trim() = "MTA TEMP") AndAlso (dtCoverStartDate >= v_dtStartDate) AndAlso (Not r_bBackdatingRequired) Then
                        bAddToArray = True
                    End If
                End If


                If bAddToArray Then
                    If v_bIsCancellation Then
                        nTypeInd = AC3Cancellation
                    ElseIf v_bIsReinstatement Then
                        nTypeInd = AC3Reinstatement
                    Else
                        nTypeInd = AC3MTA
                    End If

                    If nAffectedInsuranceFileCnts = 0 Then
                        ReDim r_vAffectedInsuranceFileCnts(AC2ArraySize, nAffectedInsuranceFileCnts)
                    Else
                        ReDim Preserve r_vAffectedInsuranceFileCnts(AC2ArraySize, nAffectedInsuranceFileCnts)
                    End If



                    r_vAffectedInsuranceFileCnts(AC2InsuranceFileCnt, nAffectedInsuranceFileCnts) = vArray(AC1InsuranceFileCnt, lTemp)


                    r_vAffectedInsuranceFileCnts(AC2CoverStartDate, nAffectedInsuranceFileCnts) = dtCoverStartDate


                    r_vAffectedInsuranceFileCnts(AC2PolicyVersion, nAffectedInsuranceFileCnts) = vArray(AC1PolicyVersion, lTemp)


                    r_vAffectedInsuranceFileCnts(AC2InsFileType, nAffectedInsuranceFileCnts) = vArray(AC1InsFileTypeCode, lTemp)


                    r_vAffectedInsuranceFileCnts(AC2ExpiryDate, nAffectedInsuranceFileCnts) = dtExpiryDate


                    r_vAffectedInsuranceFileCnts(AC2InsFileStatus, nAffectedInsuranceFileCnts) = vArray(AC1InsuranceFileStatusId, lTemp)

                    nAffectedInsuranceFileCnts += 1


                    If nMTAInsuranceFileLinkCnt = 0 Then
                        ReDim m_vMTAInsuranceFileLinkArray(AC4ArraySize, nMTAInsuranceFileLinkCnt)
                    Else
                        ReDim Preserve m_vMTAInsuranceFileLinkArray(AC4ArraySize, nMTAInsuranceFileLinkCnt)
                    End If

                    m_vMTAInsuranceFileLinkArray(AC4InsuranceFileCnt, nMTAInsuranceFileLinkCnt) = vArray(AC1InsuranceFileCnt, lTemp)

                    m_vMTAInsuranceFileLinkArray(AC4InsuranceFileStatusId, nMTAInsuranceFileLinkCnt) = vArray(AC1InsuranceFileStatusId, lTemp)
                    m_vMTAInsuranceFileLinkArray(AC4TypeInd, nMTAInsuranceFileLinkCnt) = nTypeInd

                    m_vMTAInsuranceFileLinkArray(AC4InsuranceFileType, nMTAInsuranceFileLinkCnt) = vArray(AC1InsFileTypeCode, lTemp)
                    nMTAInsuranceFileLinkCnt += 1
                End If

                If nThisOne = -1 And bAddToArray = True Then
                    If v_bIsReinstatement Then
                        '    lThisOne = 0
                    Else 'deepak
                        If (sType.Trim() = "MTA PERM") OrElse (sType.Trim() = "POLICY") OrElse (sType.Trim() = "MTAREINS") OrElse (sType.Trim() = "MTACAN") Then

                            If v_lDeletedRiskInsuranceFileCnt > 0 And nInsuranceFileCnt < v_lDeletedRiskInsuranceFileCnt Then
                                nThisOne = lTemp

                            Else
                                If (dtCoverStartDate <= v_dtStartDate) And (dtExpiryDateToUse >= v_dtStartDate) Then
                                    nThisOne = lTemp
                                    'Exit For
                                    'Exit For
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Next lTemp

        If v_bIsReinstatement = True Then
            nThisOne = 0
        End If
        If m_sCallingAppName = "iACTCreditControlProcessing" Then
            r_lInsuranceFileCnt = vArray(ACIInsFileId, 0)
        End If
        If nThisOne = -1 Then
            'We should never, ever get here, but just in case
            r_lErrorCode = 3
            vArray = Nothing
            Return nResult
        End If
        r_lInsuranceFileCnt = CInt(vArray(0, nThisOne))
        r_lPolicyVersion = CInt(vArray(1, nThisOne))

        'Last thing - what version is the new adjustment?
        'sSQL = New StringBuilder
        'sSQL.Append("SELECT MAX(ifi.policy_version) ")
        'sSQL.Append("FROM insurance_file ifi, ")
        'sSQL.Append("insurance_file ifi2 ")
        'sSQL.Append("WHERE ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt ")
        'sSQL.Append("AND ifi.policy_ignore is null ")
        'sSQL.Append("AND ifi.policy_ignore is null ")
        'sSQL.Append("AND ifi2.insurance_file_cnt = " & r_lInsuranceFileCnt & Strings.ChrW(13) & Strings.ChrW(10))
        'sSQL.Append("AND ifi.policy_ignore is null" & Strings.ChrW(13) & Strings.ChrW(10))
        'm_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString, sSQLName:="GetUnderwritingVersionsByDate2", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If

        'If Not informations.IsArray(vArray) Then
        '    Return nResult
        'End If


        'r_lPolicyVersion = CInt(vArray(0, 0))

        vArray = Nothing

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: CreateMTAInsuranceFileLink
    '
    ' Description:
    '
    ' History: 14/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    'WPR 33-75 added
    Public Function CreateMTAInsuranceFileLink(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal bIsDirty As Boolean = False) As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_vMTAInsuranceFileLinkArray Is Nothing Then
                If obj_m_vMTAInsuranceFileLinkArray Is Nothing Then
                    Return result
                End If
                m_vMTAInsuranceFileLinkArray = obj_m_vMTAInsuranceFileLinkArray
            End If

            'm_vMTAInsuranceFileLinkArray(AC4SequenceNo, lTemp)
            For lTemp As Integer = 0 To m_vMTAInsuranceFileLinkArray.GetUpperBound(1)
                If CDbl(m_vMTAInsuranceFileLinkArray(AC4InsuranceFileCnt, lTemp)) <> v_lInsuranceFileCnt Then
                    'WPR 33-75 added
                    m_lReturn = AddMtaInsuranceFileLink(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSequenceNumber:=(m_vMTAInsuranceFileLinkArray.GetUpperBound(1) + 1) - lTemp, v_iTypeInd:=CInt(m_vMTAInsuranceFileLinkArray(AC4TypeInd, lTemp)), v_iProcessedInd:=0, v_lOriginalInsuranceFileStatusId:=CInt(Val(CStr(m_vMTAInsuranceFileLinkArray(AC4InsuranceFileStatusId, lTemp)))), v_vOriginalLinkedInsuranceFileCnt:=m_vMTAInsuranceFileLinkArray(AC4InsuranceFileCnt, lTemp), v_bIsDirty:=bIsDirty)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddMtaInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateMTAInsuranceFileLink")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If ((m_vMTAInsuranceFileLinkArray(AC4InsuranceFileType, lTemp)).ToString.ToUpper.Trim = "MTAQUOTE" Or m_vMTAInsuranceFileLinkArray(AC4InsuranceFileType, lTemp).ToString.ToUpper.Trim = "MTAQCAN" _
                        Or Trim((m_vMTAInsuranceFileLinkArray(AC4InsuranceFileType, lTemp))).ToUpper = "MTAQTETEMP") And ToSafeLong(m_vMTAInsuranceFileLinkArray(AC4InsuranceFileStatusId, lTemp)) <> 1 Then
                        'Update the status of existing quotes to replaced
                        m_lReturn = UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=CInt(m_vMTAInsuranceFileLinkArray(AC4InsuranceFileCnt, lTemp)), v_sInsuranceFileStatusCode:="REP")
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateMTAInsuranceFileLink")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                End If
            Next lTemp


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateMTAInsuranceFileLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddMtaInsuranceFileLink
    '
    ' Description:
    '
    ' History: 14/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    'WPR 33-75 added
    Private Function AddMtaInsuranceFileLink(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSequenceNumber As Integer, ByVal v_iTypeInd As Integer, ByVal v_iProcessedInd As Integer, ByVal v_lOriginalInsuranceFileStatusId As Integer, Optional ByVal v_vOriginalLinkedInsuranceFileCnt As Object = Nothing, Optional ByVal v_vCancelledLinkedInsuranceFileCnt As Object = Nothing, Optional ByVal v_vNewLinkedInsuranceFileCnt As Object = Nothing, Optional ByRef v_bIsDirty As Boolean = False) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Add parameter
        'InsuranceFileCnt
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Sequence Number
        m_lReturn = m_oDatabase.Parameters.Add(sName:="sequence_number", vValue:=CStr(v_lSequenceNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Type Ind
        m_lReturn = m_oDatabase.Parameters.Add(sName:="type_ind", vValue:=CStr(v_iTypeInd), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Processed Ind
        m_lReturn = m_oDatabase.Parameters.Add(sName:="processed_ind", vValue:=CStr(v_iProcessedInd), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'OriginalInsuranceFileStatusId
        If v_lOriginalInsuranceFileStatusId <> 0 Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_insurance_file_status_id", vValue:=CStr(v_lOriginalInsuranceFileStatusId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_insurance_file_status_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'OriginalLinkedInsuranceFileCnt

        If Not informations.IsNothing(v_vOriginalLinkedInsuranceFileCnt) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_linked_insurance_file_cnt", vValue:=CStr(v_vOriginalLinkedInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_linked_insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'CancelledLinkedInsuranceFileCnt

        If Not informations.IsNothing(v_vCancelledLinkedInsuranceFileCnt) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cancelled_linked_insurance_file_cnt", vValue:=CStr(v_vCancelledLinkedInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cancelled_linked_insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'NewLinkedInsuranceFileCnt

        If Not informations.IsNothing(v_vNewLinkedInsuranceFileCnt) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_linked_insurance_file_cnt", vValue:=CStr(v_vNewLinkedInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_linked_insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'WPR 33-75 added
        m_lReturn = m_oDatabase.Parameters.Add(sName:="isDirty", vValue:=If(v_bIsDirty, 1, 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddMtaInsuranceFileLinkSQL, sSQLName:=ACAddMtaInsuranceFileLinkName, bStoredProcedure:=ACAddMtaInsuranceFileLinkStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: UpdateInsuranceFileStatus
    '
    ' Description:
    '
    ' History: 21/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateInsuranceFileStatus(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_sInsuranceFileStatusCode As String = "", Optional ByVal v_lInsuranceFileStatusId As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""

            sSQL = "UPDATE insurance_file SET insurance_file_status_id = "
            If v_sInsuranceFileStatusCode <> "" Then
                sSQL = sSQL & "(SELECT insurance_file_status_id FROM insurance_file_status WHERE code = '"
                sSQL = sSQL & v_sInsuranceFileStatusCode & "') "
            Else
                If v_lInsuranceFileStatusId = 0 Then
                    sSQL = sSQL & "NULL"
                Else
                    sSQL = sSQL & CStr(v_lInsuranceFileStatusId)
                End If
            End If
            sSQL = sSQL & " WHERE insurance_file_cnt = " & CStr(v_lInsuranceFileCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateInsuranceFileStatus", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' UpdateMTAInsuranceFileLink
    ''' </summary>
    ''' <param name="v_iType"></param>
    ''' <param name="v_vInsuranceFileCnt"></param>
    ''' <param name="v_vOriginalLinkedInsuranceFileCnt"></param>
    ''' <param name="v_vCancelledLinkedInsuranceFileCnt"></param>
    ''' <param name="v_vNewLinkedInsuranceFileCnt"></param>
    ''' <param name="v_vProcessedInd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateMTAInsuranceFileLink(ByVal v_iType As Integer, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vOriginalLinkedInsuranceFileCnt As Object = Nothing, Optional ByVal v_vCancelledLinkedInsuranceFileCnt As Object = Nothing, Optional ByVal v_vNewLinkedInsuranceFileCnt As Object = Nothing, Optional ByVal v_vProcessedInd As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim sSQL As String = ""

            Select Case v_iType
                Case 1

                    If Not informations.IsNothing(v_vCancelledLinkedInsuranceFileCnt) Then
                        sSQL = "UPDATE mta_insurance_file_link SET cancelled_linked_insurance_file_cnt = "
                        sSQL = sSQL & v_vCancelledLinkedInsuranceFileCnt & " WHERE insurance_file_cnt = "
                        sSQL = sSQL & v_vInsuranceFileCnt & " AND original_linked_insurance_file_cnt = "
                        sSQL = sSQL & v_vOriginalLinkedInsuranceFileCnt
                    Else
                        sSQL = "UPDATE mta_insurance_file_link SET new_linked_insurance_file_cnt = "
                        sSQL = sSQL & v_vNewLinkedInsuranceFileCnt & " WHERE insurance_file_cnt = "
                        sSQL = sSQL & v_vInsuranceFileCnt & " AND original_linked_insurance_file_cnt = "
                        sSQL = sSQL & v_vOriginalLinkedInsuranceFileCnt
                    End If
                Case 2
                    sSQL = "UPDATE mta_insurance_file_link SET processed_ind = " & v_vProcessedInd
                    sSQL = sSQL & " WHERE cancelled_linked_insurance_file_cnt = " & v_vCancelledLinkedInsuranceFileCnt
                Case 3
                    sSQL = "UPDATE mta_insurance_file_link SET new_linked_insurance_file_cnt = "
                    sSQL = sSQL & v_vNewLinkedInsuranceFileCnt & " WHERE insurance_file_cnt = "
                    sSQL = sSQL & v_vInsuranceFileCnt & " AND original_linked_insurance_file_cnt = "
                    sSQL = sSQL & v_vOriginalLinkedInsuranceFileCnt
                Case 4

                    If Not informations.IsNothing(v_vInsuranceFileCnt) Then
                        sSQL = sSQL & "DELETE from mta_insurance_file_link "
                        sSQL = sSQL & "WHERE insurance_file_cnt = " & v_vInsuranceFileCnt
                    ElseIf Not informations.IsNothing(v_vNewLinkedInsuranceFileCnt) Then
                        sSQL = sSQL & "DELETE from mta_insurance_file_link "
                        sSQL = sSQL & "WHERE new_linked_insurance_file_cnt = " & v_vNewLinkedInsuranceFileCnt
                    Else
                        sSQL = sSQL & "DELETE from mta_insurance_file_link "
                        sSQL = sSQL & "WHERE cancelled_linked_insurance_file_cnt = " & v_vCancelledLinkedInsuranceFileCnt
                    End If
                Case 5
                    sSQL = "DELETE from mta_insurance_file_link "
                    sSQL = sSQL & "WHERE insurance_file_cnt in "
                    sSQL = sSQL & "(SELECT insurance_file_cnt From mta_insurance_file_link "

                    If Not informations.IsNothing(v_vNewLinkedInsuranceFileCnt) Then
                        sSQL = sSQL & "WHERE new_linked_insurance_file_cnt = " & v_vNewLinkedInsuranceFileCnt & ")"
                    Else
                        sSQL = sSQL & "WHERE cancelled_linked_insurance_file_cnt = " & v_vCancelledLinkedInsuranceFileCnt & ")"
                    End If
            End Select

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateMTAInsuranceFileLink", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateMTAInsuranceFileLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateInsuranceFileType
    '
    ' Description:
    '
    ' History: 21/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateInsuranceFileType(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceFileTypeCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""

            sSQL = "UPDATE insurance_file SET insurance_file_type_id = "
            sSQL = sSQL & "(SELECT insurance_file_type_id FROM insurance_file_type WHERE code = '"
            sSQL = sSQL & v_sInsuranceFileTypeCode & "') "
            sSQL = sSQL & "WHERE insurance_file_cnt = " & CStr(v_lInsuranceFileCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateInsuranceFileType", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetOriginalLinkedVersion......
    '
    ' Description:
    '
    ' History: 21/01/2003 sj - Created.
    ' RAW 18/02/2004 : CQ3665 : changed r_dtExpiryDate param to be optional
    ' RAW 08/03/2004 : CQ4180 : made more generic by renaming and adding new v_bLookForCancelled parameter
    ' ***************************************************************** '
    Public Function GetOriginalLinkedVersion(ByVal v_lNewLinkedInsuranceFileCnt As Integer, ByRef r_lOriginalLinkedInsuranceFileCnt As Integer, Optional ByRef r_dtExpiryDate As Date = #12/30/1899#, Optional ByVal v_bLookForCancelled As Boolean = False, Optional ByRef r_bRenewals As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""
            Dim vResultArray(,) As Object = Nothing

            sSQL = "SELECT l.original_linked_insurance_file_cnt, i.expiry_date "
            sSQL = sSQL & "FROM mta_insurance_file_link l, insurance_file i "



            If v_bLookForCancelled Then
                sSQL = sSQL & " WHERE l.cancelled_linked_insurance_file_cnt = " & CStr(v_lNewLinkedInsuranceFileCnt)
            Else
                ' RAW 08/03/2004 : CQ4180 : added
                sSQL = sSQL & " WHERE l.new_linked_insurance_file_cnt = " & CStr(v_lNewLinkedInsuranceFileCnt)
            End If

            sSQL = sSQL & " AND i.insurance_file_cnt = l.original_linked_insurance_file_cnt"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOriginalLinkedVersion", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                r_lOriginalLinkedInsuranceFileCnt = 0
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else

                r_lOriginalLinkedInsuranceFileCnt = CInt(vResultArray(0, 0))

                r_dtExpiryDate = CDate(vResultArray(1, 0))
            End If
            If v_bLookForCancelled = True Then
                sSQL = "Select NULL From insurance_file WHERE insurance_file_type_id = 2 AND insurance_file_cnt = " & r_lOriginalLinkedInsuranceFileCnt

                m_lReturn = m_oDatabase.SQLSelect(
                    sSQL:=sSQL,
                    sSQLName:="GetOriginalLinkedVersion",
                    bStoredProcedure:=False,
                    vResultArray:=vResultArray)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    GetOriginalLinkedVersion = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            If Informations.IsArray(vResultArray) = False Then
                r_bRenewals = False
            Else
                r_bRenewals = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalLinkedVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalLinkedVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function CheckInClaim(ByVal v_sInsuranceRef As String, ByRef r_lClaimStatus As Integer, ByRef v_dtStartDate As Date) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lClaimStatus = -1

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_ref", vValue:=v_sInsuranceRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Loss_date", vValue:=v_dtStartDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckClaimSQL, sSQLName:=ACCheckClaimName, bStoredProcedure:=False, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If


            r_lClaimStatus = CInt(vArray(0, 0))

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function

    Public Function GetBasePolicyCntForBackDateMTA(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtMTADate As Date, ByRef lBaseInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lBaseInsuranceFileCnt = -1

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsFolderCnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="MTAEffectiveDate", vValue:=v_dtMTADate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBasePolicyCntForBackDateMTASQL, sSQLName:=ACGetBasePolicyCntForBackDateMTAName, bStoredProcedure:=True, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If
            lBaseInsuranceFileCnt = CInt(vArray(0, 0))


            vArray = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBasePolicyCntForBackDateMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBasePolicyCntForBackDateMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function GetCoverFromDate(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtMTADate As Date, ByVal lBaseInsuranceFileCnt As Integer, ByRef DtMTAEndDate As Object) As Integer


        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsFolderCnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="MTAEffectiveDt", vValue:=v_dtMTADate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsFileCnt", vValue:=CStr(lBaseInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCoverFromDateSQL, sSQLName:=ACGetCoverFromDateName, bStoredProcedure:=False, vResultArray:=vArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            ElseIf Informations.IsArray(vArray) Then


                DtMTAEndDate = vArray(0, 0)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCoverFromDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoverFromDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    'WPR 33-75 ADDED
    Public Function GetCoverEndDate(ByVal v_lInsuranceFolderCnt As Integer,
                                               ByVal v_dtMTADate As Date,
                                               ByVal lBaseInsuranceFileCnt As Integer,
                                               ByRef DtMTAEndDate As Object) As Integer



        Dim vArray(,) As Object = Nothing

        Try

            GetCoverEndDate = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsFileCnt",
                                                   vValue:=lBaseInsuranceFileCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCoverEndDateSQL,
                                              sSQLName:=ACGetCoverEndDateName,
                                              bStoredProcedure:=False,
                                              vResultArray:=vArray)


            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            ElseIf Informations.IsArray(vArray) Then
                DtMTAEndDate = vArray(0, 0)
            End If

            Return gPMConstants.PMEReturnCode.PMTrue


        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCoverEndDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoverEndDate", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    'WPR 33-75 END

    Public Function GetInsuranceFileType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sInsuranceFileTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsFileCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsFileTypeSQL, sSQLName:=ACGetInsFileTypeName, bStoredProcedure:=False, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If


            r_sInsuranceFileTypeCode = CStr(vArray(0, 0))

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFileType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'WPR 33-75 added
    Public Function CheckInsuranceFileExistance(ByVal v_lInsuranceFileCnt As Integer, ByRef bInsuranceFileExists As Boolean) As Integer

        Dim vArray(,) As Object = Nothing
        Try

            CheckInsuranceFileExistance = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_Get_Insurance_Ref", sSQLName:="spu_Get_Insurance_Ref", bStoredProcedure:=True, vResultArray:=vArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                bInsuranceFileExists = False
            Else
                bInsuranceFileExists = True
            End If

            Return gPMConstants.PMEReturnCode.PMFalse

        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="CheckInsuranceFileExistance", r_lFunctionReturn:=CheckInsuranceFileExistance, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    'WPR 33-75 END

    '************************************************************************************
    ' Name : RebuildArrayFromLinkedPolicyVersions
    '
    ' Desc : get all versions of policy linked by mta_insurance_file_link table
    '      : Type 1 = Cancelled
    ' Hist : 16/01/2003 SJ Created
    '************************************************************************************
    Public Function RebuildArrayFromLinkedPolicyVersions(ByRef r_vResultArray(,) As Object, ByVal v_lInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()



            result = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACRebuildArrayFromLinkedPolicyVersionsSQL, sSQLName:=ACRebuildArrayFromLinkedPolicyVersionsName, bStoredProcedure:=ACRebuildArrayFromLinkedPolicyVersionsStored, vResultArray:=r_vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RebuildArrayFromLinkedPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildArrayFromLinkedPolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************************
    ' Get values from specified columns (v_vReturnColumn)
    ' if v_sKeyColumn and v_sKeyValue is passed in then use it as criteria otherwise
    ' whole table is selected
    ' if v_vReturnColumn is not an array then single value is returned
    ' Note: v_lColumnType = PMLong, PMString etc
    '
    '****************************************************************************
    'NIIT - Replaced with the Migrated code 1144 
    Public Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_iDataType As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            sSQL = New StringBuilder("SELECT DISTINCT ")

            If Informations.IsArray(v_vReturnColumn) Then

                For Each v_vReturnColumn_item As Object In v_vReturnColumn

                    sSQL.Append(CStr(v_vReturnColumn_item) & ",")
                Next v_vReturnColumn_item

                'get rid of last comma
                sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1))

            Else
                sSQL.Append(CStr(v_vReturnColumn))
            End If

            sSQL.Append(Strings.ChrW(13) & Strings.ChrW(10) & "FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10))

            If v_sKeyColumn <> "" Then
                sSQL.Append("WHERE " & v_sKeyColumn & " = {KeyValue}")

                m_oDatabase.Parameters.Clear()

                Select Case v_iDataType
                    Case gPMConstants.PMEDataType.PMString
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMLong
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMInteger
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDouble
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDbl(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDate
                        'Developer Guide No 40
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CDate(v_sKeyValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMBoolean
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CBool(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMCurrency
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDec(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception
                End If
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="Get single value from table", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            'are we returning an array or a single value?
            If Informations.IsArray(v_vReturnColumn) Then

                r_vResult = vResultArray
            Else
                If Informations.IsArray(vResultArray) Then

                    'NIIT Comments: String/Integer Value canot be assigned to a array. Only an array can be assigned to an array
                    r_vResult = vResultArray
                Else
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If

            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get value from " & v_sTableName & "." & v_sKeyColumn & " = " & v_sKeyValue, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValueFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally

        End Try
        Return result
    End Function

    'developer guide no. 17
    Public Function GetOutOfSequenceMTAUserAuthority(ByVal v_iMTAAuthority(,) As Object) As Integer

        Dim result As Integer = 0
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="out_of_sequence_mta_authority", v_sKeyColumn:="user_id", v_sKeyValue:=m_iUserID, v_iDataType:=gPMConstants.PMEDataType.PMInteger, r_vResult:=v_iMTAAuthority)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetOutOfSequenceMTAUserAuthority", "Failed to fetch User MTA Authority", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetOutOfSequenceMTAUserAuthority", r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    'Start(Sriram P)PN56443
    Public Function GetAllTransactionDates(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sUsername As String, ByRef r_vTransactionArray(,) As Object, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer) As Integer
        Dim Catch_Renamed As Boolean = False


        Dim result As Integer = 0
        Const kMethodName As String = "GetAllTransactionDates"
        Try
            Catch_Renamed = True



            Dim v_oDatabase As dPMDAO.Database = Nothing

            Dim m_lReturn As gPMConstants.PMEReturnCode
            Dim sSQL As String = ""
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(gPMComponentServices.NewDatabase(v_sUsername:=v_sUsername, v_iSourceID:=v_iSourceID, v_iLanguageID:=v_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=v_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create new database connection", gPMConstants.PMELogLevel.PMLogError)

            End If
            v_oDatabase.Parameters.Clear()
            m_lReturn = v_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


            End If

            m_lReturn = v_oDatabase.SQLSelect(sSQL:=ACGetAllTransactionDatesSQL, sSQLName:=ACGetAllTransactionDates, bStoredProcedure:=GetAllTransactionDatesStored, vResultArray:=r_vTransactionArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetAllTransactionDatesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then


                ' DO Not Call any functions before here or the error will be lost
                bPMFunc.LogError(v_sUsername:=v_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

                ' If you want to rollback a transaction or something, do it here

            End If
Finally_Renamed:
        End Try
    End Function
    'End(Sriram P)PN56443

    Public Function CheckPolicyStatus(ByVal v_lInsuranceFileCnt As Object, ByRef r_sPolicyStatus As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If (Not False) And (Not Object.Equals(v_lInsuranceFileCnt, Nothing)) Then
                With m_oDatabase
                    .Parameters.Clear()

                    ' Add InsuranceFileCnt as INPUT

                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(CInt(v_lInsuranceFileCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    sSQL = "SELECT ift.code as policystatuscode FROM Insurance_File_Type ift JOIN Insurance_File ifi on ift.insurance_file_type_id =ifi.insurance_file_type_id " &
                           "WHERE insurance_file_cnt = {insurance_file_cnt}"

                    m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetInsFileTypeId", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Carry on without default set
                    End If

                    If .Records.Count() = 1 Then
                        'developer guide no. 111
                        r_sPolicyStatus = .Records.Item(0).Fields()("policystatuscode")
                    End If
                End With
            End If
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPolicyStatus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPolicyStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetInsuranceFileStatus
    '
    ' Description:
    '
    ' History: 01/09/2009
    '
    ' ***************************************************************** '
    Public Function GetInsuranceFileStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0
        'developer guide no. 17
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetInsuranceFileStatus", "Error in adding Parameter insurance_file_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsuranceFileStatusSQL, sSQLName:=ACGetInsuranceFileStatusName, bStoredProcedure:=ACGetInsuranceFileStatusStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError("GetInsuranceFileStatus", "Error in ACGetInsuranceFileStatusName", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If

            r_vArray = vArray
            vArray = Nothing
            Return result

        Catch ex As Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFileStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally

        End Try
        Return result
    End Function

    Public Function IsMarkedForCollection(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIsMarked As Integer, ByRef r_dMarkedDate As Date) As Integer

        'WPR12- Enhancement Quote Collection Process

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lIsMarked = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsMarkedForCollectionSQL, sSQLName:=ACIsMarkedForCollectionName, bStoredProcedure:=True, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If

            r_lIsMarked = gPMFunctions.ToSafeLong(vArray(0, 0))
            r_dMarkedDate = gPMFunctions.ToSafeDate(vArray(1, 0))


            vArray = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsMarkedForCollection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsMarkedForCollection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateMarkedForCollectionStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIsMarked As Integer) As Integer
        'call spu_SIR_Update_Marked_For_Collection by passing v_lInsuranceFileCnt and r_lIsMarked as parameters


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(gPMFunctions.ToSafeLong(v_lInsuranceFileCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="marked_for_collection", vValue:=CStr(gPMFunctions.ToSafeLong(r_lIsMarked)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="marked_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateMarkedForCollectionStatusSQL, sSQLName:=ACUpdateMarkedForCollectionStatusName, bStoredProcedure:=ACUpdateMarkedForCollectionStatusStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateMarkedForCollectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateMarkedForCollectionStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindLikeIndexForCollection (Public)
    '
    ' Description: Selects Index Description from the value supplied
    '
    ' Edit History  : 1
    ' Description   : Added code to Use the GIS Search Property Find
    '                 Stored Procedure (Commented the old codes )
    ' ***************************************************************** '
    Public Function FindLikeIndexForCollection(ByRef sIndex As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional ByRef lSpecificDataModelIndex As Integer = 1, Optional ByRef sSearchType As String = "Like", Optional ByRef lSpecificFieldType As Integer = 0) As Integer

        'WPR 33-75 added
        'Dim result As Integer = 0
        Dim result As Integer = 1
        Dim sSQL As String = ""

        Dim vDataModelCodeArray(,) As Object = Nothing
        Dim vGISSearchDataArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get All Data Model Codes

            m_lReturn = GetAllDataModelCodes(vDataModelCodeArray, lSpecificDataModelIndex)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Informations.IsArray(vDataModelCodeArray) Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get DataModel Codes", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndexForCollection")

                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' Get Search Results for all available Data Model Codes


                m_lReturn = GetAllGISSearchResults(sIndex, lNumberOfRecords, vDataModelCodeArray, vGISSearchDataArray, lSpecificFieldType)

                If ((m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Informations.IsArray(vGISSearchDataArray)) And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="FindLikeIndex Failed. Failed to GetAllGISSearchResults", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndexForCollection")

                    Return gPMConstants.PMEReturnCode.PMFalse

                Else


                    vResultArray = vGISSearchDataArray

                    ' If NO Indexes were found return Not Found
                    If Not Informations.IsArray(vResultArray) Then
                        result = gPMConstants.PMEReturnCode.PMNotFound
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeIndexForCollection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndexForCollection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetQuotesMarkedForCollection(Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lAgentCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_sProductIds As String = "", Optional ByVal v_lIsDirectBusiness As Integer = 0, Optional ByVal v_dtStartDate As Date = #12/30/1899#, Optional ByVal v_dtEndDate As Date = #12/30/1899#, Optional ByRef r_vResultArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            If v_lPartyCnt > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="client_id", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_lAgentCnt > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_id", vValue:=CStr(v_lAgentCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_lInsuranceFileCnt > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurancefilecnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sProductIds.Trim().Length > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="product_ids", vValue:=v_sProductIds, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_dtStartDate <> #12:00:00 AM# Then
                'developer guide no.40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="start_date", vValue:=v_dtStartDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_dtEndDate <> #12:00:00 AM# Then
                'developer guide no.40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="end_date", vValue:=v_dtEndDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not (Informations.IsDBNull(v_lIsDirectBusiness) Or Informations.IsNothing(v_lIsDirectBusiness)) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="direct", vValue:=CStr(v_lIsDirectBusiness), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetQuotesMarkedForCollectionSQL, sSQLName:=ACGetQuotesMarkedForCollectionName, bStoredProcedure:=True, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotesMarkedForCollection")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO Insurance Files were found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesMarkedForCollection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookUp (Private)
    '
    ' Description: get values from look up table
    '
    ' ***************************************************************** '
    Public Function GetLookUp(ByVal v_sTableName As String, ByVal v_sKeyIDFieldName As String, ByVal v_sDescFieldName As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = ""
            sSQL = sSQL & "SELECT " & v_sKeyIDFieldName & ", " & v_sDescFieldName
            sSQL = sSQL & " FROM " & v_sTableName

            If v_sTableName = "Source" Then
                ' Only load branches accessible to the current user
                sSQL = sSQL & " WHERE source_id not in (select source_id from PMUser_Source where user_id = " & CStr(m_iUserID) & ") "
            Else
                ' Exclude deleted records if we are not loading the branches.
                ' (if we ARE loading the branches, then we do want the deleted ones, i.e. the closed branches)
                sSQL = sSQL & " WHERE is_deleted = 0 "
            End If
            sSQL = sSQL & " ORDER BY " & v_sDescFieldName

            m_oDatabase.Parameters.Clear()

            Return m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLookupValues", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookUp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookUp", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'PN 74021- Priya

    Public Function GetLatestPolicyVersion(ByVal v_lInsuranceFileCnt As Long, ByRef r_lPolicyVersion As Long) As Long
        Dim sSQL As String
        Dim vArray(,) As Object = Nothing

        Try

            GetLatestPolicyVersion = gPMConstants.PMEReturnCode.PMTrue


            sSQL = "SELECT MAX(ifi.policy_version)" & vbCrLf
            sSQL = sSQL & "FROM insurance_file ifi," & vbCrLf
            sSQL = sSQL & "insurance_file ifi2" & vbCrLf
            sSQL = sSQL & "WHERE ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt" & vbCrLf
            sSQL = sSQL & "AND ifi.policy_ignore is null" & vbCrLf
            sSQL = sSQL & "AND ifi2.insurance_file_cnt = " & v_lInsuranceFileCnt & vbCrLf

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL,
                                              sSQLName:="GetLAtestVersion",
                                              bStoredProcedure:=False,
                                              lNumberRecords:=gPMConstants.PMAllRecords,
                                          vResultArray:=vArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetLatestPolicyVersion = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If Not Informations.IsArray(vArray) Then
                Exit Function
            End If

            r_lPolicyVersion = vArray(0, 0)

            vArray = Nothing

            Exit Function

        Catch ex As Exception

            GetLatestPolicyVersion = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage((m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLatestPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLatestPolicyVersion", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)
            Exit Function

        End Try
    End Function

    ''' <summary>
    ''' This function check this is market place policy or not 
    ''' </summary>
    ''' <param name="nInsuranceFileKey"></param>
    ''' <param name="o_bIsMarketplacePolicy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckIsMarketplacePolicy(ByVal nInsuranceFileKey As Integer, ByRef o_bIsMarketplacePolicy As Boolean, ByRef o_bIsReferredQuote As Boolean) As Integer
        Const kMethodName As String = "CheckIsMarketplacePolicy"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse

        Try
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="nInsuranceFileKey", vValue:=nInsuranceFileKey, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
                Return nReturn
            End If

            Dim dtResults As DataTable = Nothing
            nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACCheckIsMarketplacePolicySQL, sSQLName:=ACCheckIsMarketplacePolicyName, bStoredProcedure:=ACCheckIsMarketplacePolicyStored, oRecordset:=dtResults)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACCheckIsMarketplacePolicySQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
                Return nReturn
            Else
                If dtResults IsNot Nothing AndAlso dtResults.Rows IsNot Nothing AndAlso dtResults.Rows.Count > 0 Then
                    o_bIsMarketplacePolicy = ToSafeBoolean(dtResults.Rows(0).Item(0))
                    o_bIsReferredQuote = ToSafeBoolean(dtResults.Rows(0).Item(1))
                End If
            End If
            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIsMarketplacePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nReturn
        Finally
            'clear all objects here
        End Try

    End Function
    ''' <summary>
    ''' Copies risk links between insurance files
    ''' </summary>
    ''' <param name="v_lOldInsuranceFileCnt"></param>
    ''' <param name="v_lNewInsuranceFileCnt"></param>
    ''' <param name="v_bCopyDeletedRisk"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CopyRiskLinks(ByVal v_lOldInsuranceFileCnt As Long,
                               ByVal v_lNewInsuranceFileCnt As Long,
                               ByVal v_bCopyDeletedRisk As Boolean) As Long


        CopyRiskLinks = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt",
                                               vValue:=v_lOldInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CopyRiskLinks = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt",
                                               vValue:=v_lNewInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CopyRiskLinks = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="CopyDeletedRisks",
                                               vValue:=v_bCopyDeletedRisk,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMBoolean)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CopyRiskLinks = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If


        m_lReturn = m_oDatabase.SQLAction(sSQL:=kCopyRiskLinksSQL,
                                          sSQLName:=kCopyRiskLinksName,
                                          bStoredProcedure:=kCopyRiskLinksStored)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CopyRiskLinks = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

    End Function

    ''' <summary>
    ''' This function update the market place policy or not 
    ''' </summary>
    ''' <param name="nInsuranceFileKey"></param>
    ''' <param name="o_bIsMarketplacePolicy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateMarketplacePolicyStatus(ByVal nInsuranceFileKey As Integer, ByRef o_bIsMarketplacePolicy As Boolean) As Integer
        Const kMethodName As String = "UpdateMarketplacePolicyStatus"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse

        Try
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="nInsuranceFileKey", vValue:=nInsuranceFileKey, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
                Return nReturn
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="nIsMarketplacePolicy", vValue:=If(o_bIsMarketplacePolicy, 1, 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
                Return nReturn
            End If

            nReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateMarketplacePolicyStatusSQL, sSQLName:=ACUpdateMarketplacePolicyStatusName, bStoredProcedure:=ACUpdateMarketplacePolicyStatusStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACUpdateMarketplacePolicyStatusSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
                Return nReturn
            End If
            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateMarketplacePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nReturn
        Finally
            'clear all objects here
        End Try

    End Function

    ''' <summary>
    ''' Copy Policy Associates
    ''' </summary>
    ''' <param name="nOldInsuranceFileCnt"></param>
    ''' <param name="nNewInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyPolicyAssociates(ByVal nOldInsuranceFileCnt As Integer, ByVal nNewInsuranceFileCnt As Integer) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Old_Insurance_File_Cnt", vValue:=nOldInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="New_Insurance_File_Cnt", vValue:=nNewInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kCopyPolicyAssociatesSQL, sSQLName:=kCopyPolicyAssociateName, bStoredProcedure:=kCopyPolicyAssociateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyAssociates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAssociates", excep:=excep)
        End Try

        Return nResult
    End Function

    Private Function DefaultQuoteExpiryDate(ByVal v_nInsuranceFileCnt As Integer, ByVal v_dtMTADate As Date, ByRef r_dtQuoteExpiryDate As Object) As Integer

        Dim nResult As Integer = 0
        Dim lGracePeriodDays As Integer = 0
        Dim nProduct_Id As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim nGracePeriodDays As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Get the product from the insurance_file table
            nResult = GetValueFromTable(v_sTableName:="insurance_file", v_vReturnColumn:="product_id", v_sKeyColumn:="insurance_file_cnt", v_sKeyValue:=v_nInsuranceFileCnt, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResultArray)
            nProduct_Id = vResultArray(0, 0)

            nResult = GetValueFromTable(v_sTableName:="product", v_vReturnColumn:="grace_period", v_sKeyColumn:="product_id", v_sKeyValue:=nProduct_Id, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResultArray)
            nGracePeriodDays = vResultArray(0, 0)

            r_dtQuoteExpiryDate = CType(System.DateTime.Now.ToString(), Date).AddDays(nGracePeriodDays)



        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DefaultQuoteExpiryDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DefaultQuoteExpiryDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        Finally
            vResultArray = Nothing
        End Try

        Return nResult
    End Function


    'WPR12
    ''' <summary>
    ''' CheckDataModelCompatibility
    ''' </summary>
    ''' <param name="nOldInsuranceFileCnt"></param>
    ''' <param name="nNewRiskTypeId"></param>r_nReturnValue
    ''' <param name="r_nReturnValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckDataModelCompatibility(ByVal nOldInsuranceFileCnt As Integer, ByVal nNewRiskTypeId As Integer, ByRef r_nReturnValue As Integer) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="OldInsurance_File_Cnt", vValue:=ToSafeInteger(nOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.Parameters.Add(sName:="NewRisk_Type_Id", vValue:=ToSafeInteger(nNewRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.Parameters.Add(sName:="Return_Value", vValue:=ToSafeInteger(r_nReturnValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.SQLSelect(sSQL:=ACCheckDataModelCompatibilitySQL, sSQLName:=ACCheckDataModelCompatibilityName, bStoredProcedure:=ACCheckDataModelCompatibilityStored)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            r_nReturnValue = m_oDatabase.Parameters.Item("Return_Value").Value
            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CheckDataModelCompatibility Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDataModelCompatibility", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return nResult
        End Try
    End Function
    'WPR12
    ''' <summary>
    ''' CopyQuote
    ''' </summary>
    ''' <param name="nOldInsuranceFileCnt"></param>
    ''' <param name="nOldInsuranceFolderCnt"></param>
    ''' <param name="sPolicyRef"></param>
    ''' <param name="r_nNewInsuranceFileCnt"></param>
    ''' <param name="r_nNewInsuranceFolderCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyQuote(ByVal nOldInsuranceFileCnt As Integer, ByVal nOldInsuranceFolderCnt As Integer, ByVal sPolicyRef As String, ByRef r_nNewInsuranceFileCnt As Integer, ByRef r_nNewInsuranceFolderCnt As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim dtQuote As DataTable
        Try
            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeInteger(nOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=ToSafeInteger(nOldInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="new_insurance_ref", vValue:=ToSafeString(sPolicyRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=ToSafeInteger(r_nNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="new_insurance_folder_cnt", vValue:=ToSafeInteger(r_nNewInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="userID", vValue:=ToSafeInteger(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.ExecuteDataTable(sSQL:=ACCopyPolicyToQuoteWithoutVersioningSQL, sSQLName:=ACCopyPolicyToQuoteWithoutVersioningName, bStoredProcedure:=ACCopyPolicyToQuoteWithoutVersioningStored, oRecordset:=dtQuote)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            r_nNewInsuranceFileCnt = m_oDatabase.Parameters.Item("new_insurance_file_cnt").Value
            r_nNewInsuranceFolderCnt = m_oDatabase.Parameters.Item("new_insurance_folder_cnt").Value
            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CopyQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return nResult
        End Try
    End Function
    'WPR12
    ''' <summary>
    ''' UpdateRiskStatus
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateRiskStatus(ByVal nInsuranceFileCnt As Integer, ByVal nRiskTypeId As Integer) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kQuoteStatus As String = "UNQUOTED"
        Try
            'Update Risk Status
            m_oDatabase.Parameters.Clear()
            nResult = m_oDatabase.Parameters.Add(sName:="risk_status_code", vValue:=ToSafeString(kQuoteStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeInteger(nInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskStatusSQL, sSQLName:=ACUpdateRiskStatusName, bStoredProcedure:=ACUpdateRiskStatusStored)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'Update Risk Type
            m_oDatabase.Parameters.Clear()
            nResult = m_oDatabase.Parameters.Add(sName:="risk_type_ID", vValue:=ToSafeInteger(nRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeInteger(nInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskTypeSQL, sSQLName:=ACUpdateRiskTypeName, bStoredProcedure:=ACUpdateRiskTypeStored)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="UpdateRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' UpdateIsRiskSelected
    ''' </summary>
    ''' <param name="nRiskId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateIsRiskSelected(ByVal nRiskId As Integer) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            'Update Risk Is Selected
            m_oDatabase.Parameters.Clear()
            nResult = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=ToSafeInteger(nRiskId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.SQLAction(sSQL:=ACUpdateIsRiskSelectedSQL, sSQLName:=ACUpdateIsRiskSelectedName, bStoredProcedure:=ACUpdateIsRiskSelectedStored)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="UpdateIsRiskSelected Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateIsRiskSelected", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return nResult
        End Try
    End Function


    Public Function UpdateEventLogUser(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nResult = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            nResult = m_oDatabase.SQLAction(sSQL:=ACUpdateEventlogUserSQL, sSQLName:=ACUpdateEventlogUserName, bStoredProcedure:=ACUpdateEventlogUserStored)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateEventLogUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateEventLogUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return nResult

        End Try
    End Function

End Class

