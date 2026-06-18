Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Claims_NET.Claims")>
Public NotInheritable Class Claims

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 27/10/2003
    ' Username.
    Private m_sUserName As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_oLookup As bPMLookup.Business

    Private Const ACClass As String = "Claims"
    'developer guide no 39. 
    Private Const ACSelectClaimLinkSQL As String = "spu_SirRen_Claim_Link_Sel"
    Private Const ACSelectClaimLinkName As String = "SelectClaimLink"
    Private Const ACSelectClaimLinkStored As Boolean = True
    'developer guide no 39.
    Private Const ACSelectClaimHouseholdSQL As String = "spu_SirRen_Claim_HH_Sel"
    Private Const ACSelectClaimHouseholdName As String = "SelectClaimHousehold"
    Private Const ACSelectClaimHouseholdStored As Boolean = True
    'developer guide no 39.
    Private Const ACSelectClaimMotorSQL As String = "spu_SirRen_Claim_Motor_Sel"
    Private Const ACSelectClaimMotorName As String = "SelectClaimMotor"
    Private Const ACSelectClaimMotorStored As Boolean = True

    'AK 230402 - for generic data model
    'developer guide no 39.
    Private Const ACSelectClaimSQL As String = "spu_SirRen_Claims_Sel"
    Private Const ACSelectClaimName As String = "SelectClaimMotor"
    Private Const ACSelectClaimStored As Boolean = True

    'SJ 19/05/2004 - start
    'developer guide no 39.
    Private Const ACInsuranceFileSelSQL As String = "spe_Insurance_File_sel"
    Private Const ACInsuranceFileSelName As String = "InsuranceFileSel"
    Private Const ACInsuranceFileSelStored As Boolean = True
    'developer guide no 39.
    Private Const ACReserveTypeSelSQL As String = "spu_reserve_type_by_name_sel"
    Private Const ACReserveTypeSelName As String = "ReserveType"
    Private Const ACReserveTypeSelStored As Boolean = True
    'developer guide no 39.
    Private Const ACClaimPerilByTypeSQL As String = "spu_claim_peril_by_type_sel"
    Private Const ACClaimPerilByTypeName As String = "ClaimPerilByType"
    Private Const ACClaimPerilByTypeStored As Boolean = True
    'developer guide no 39.
    Private Const ACClaimDetailsByNumberSQL As String = "spu_claim_details_by_number_sel"
    Private Const ACClaimDetailsByNumberName As String = "ClaimDetailsByNumberName"
    Private Const ACClaimDetailsByNumberStored As Boolean = True
    'developer guide no 39.
    Private Const ACClaimNumberUpdSQL As String = "spu_claim_number_upd"
    Private Const ACClaimNumberUpdName As String = "ClaimNumberUpd"
    Private Const ACClaimNumberUpdStored As Boolean = True
    'SJ 19/05/2004 - end

    'SJ 20/05/2004 - start
    Private m_lSTSErrorReturnValue As gPMConstants.PMEReturnCode
    Private m_sSTSErrorDescription As String = ""
    'SJ 20/05/2004 - end


    'SJ 20/05/2004 - start

    Public Property STSErrorReturnValue() As Integer
        Get
            Return m_lSTSErrorReturnValue
        End Get
        Set(ByVal Value As Integer)
            m_lSTSErrorReturnValue = Value
        End Set
    End Property

    Public Property STSErrorDescription() As String
        Get
            Return m_sSTSErrorDescription
        End Get
        Set(ByVal Value As String)
            m_sSTSErrorDescription = Value
        End Set
    End Property
    'SJ 20/05/2004 - end

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bNewInstanceCreated As Boolean

            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUserName = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' PM Lookup
            m_oLookup = New bPMLookup.Business()
            m_lReturn = m_oLookup.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_oLookup = Nothing
                Return result
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            'SJ 19/05/2004 - start

            m_lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=m_sUserName, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=bNewInstanceCreated, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_oLookup = Nothing
                Return result
            End If
            '    If (IsMissing(vDatabase) = False) Then
            '        Set m_oDatabase = vDatabase
            '    End If
            'SJ 19/05/2004 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetClaimsForPolicyYear
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetClaimsForPolicyYear(ByVal v_sDataModel As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_vKey(,) As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the gis_policy_link_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_sDataModel = ACDataModelHouse Then
                ' Call the SQL
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectClaimHouseholdSQL, sSQLName:=ACSelectClaimHouseholdName, bStoredProcedure:=ACSelectClaimHouseholdStored, vResultArray:=r_vKey)
            End If

            If v_sDataModel = ACDataModelMotor Then
                ' Call the SQL
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectClaimMotorSQL, sSQLName:=ACSelectClaimMotorName, bStoredProcedure:=ACSelectClaimMotorStored, vResultArray:=r_vKey)
            End If

            'AK 230402 - if it is none of these datamodels, apply generic one
            If v_sDataModel <> ACDataModelMotor And v_sDataModel <> ACDataModelHouse Then
                ' Call the SQL
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectClaimSQL, sSQLName:=ACSelectClaimName, bStoredProcedure:=ACSelectClaimStored, vResultArray:=r_vKey)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimsForPolicyYear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimsForPolicyYear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    Public Function GetClaimLink(ByVal v_sDataModel As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_vKey(,) As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear the parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the gis_policy_link_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectClaimLinkSQL, sSQLName:=ACSelectClaimLinkName, bStoredProcedure:=ACSelectClaimLinkStored, vResultArray:=r_vKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ProcessSTSClaim
    '
    ' Description:
    '
    ' History: 13/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessSTSClaim(ByVal v_bIsNewClaim As Boolean, ByVal v_lInsuranceFileCnt As Integer, ByVal v_dtLossDate As Date, ByVal v_sClaimHandler As String, ByVal v_sProgressStatus As String, ByVal v_sDescription As String, ByVal v_sPrimaryCause As String, ByVal v_dtDateReported As Date, ByVal v_lClaimStatusId As Integer, ByVal v_sClaimNumber As String, ByVal v_sPerilType As String, ByVal v_sReserveType As String, ByVal v_cReserveAmount As Decimal, ByVal v_dtPaymentDate As Date, ByVal v_cPaymentAmount As Decimal, ByVal v_sSalvageRecoveryType As String, ByVal v_cSalvageAmount As Decimal, ByVal v_sTPRecoveryType As String, ByVal v_cTPAmount As Decimal, ByVal v_sInsurerClaimNo As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSTSErrorDescription = ""

            Dim lClaimHandlerId, lProgressStatusID, lPrimaryCauseID, lPerilTypeId, lReserveTypeId, lSalvageRecoveryTypeId, lTPRecoveryTypeId As Integer

            'Get all the ids from the suppled codes making sure they are all valid
            m_lReturn = CType(GetClaimIdsFromCode(v_sClaimHandler:=v_sClaimHandler.Trim(), v_sProgressStatus:=v_sProgressStatus.Trim(), v_sPrimaryCause:=v_sPrimaryCause.Trim(), v_sPerilType:=v_sPerilType.Trim(), v_sReserveType:=v_sReserveType.Trim(), v_sSalvageRecoveryType:=v_sSalvageRecoveryType.Trim(), v_sTPRecoveryType:=v_sTPRecoveryType.Trim(), r_lClaimHandlerId:=lClaimHandlerId, r_lProgressStatusId:=lProgressStatusID, r_lPrimaryCauseId:=lPrimaryCauseID, r_lPerilTypeId:=lPerilTypeId, r_lReserveTypeId:=lReserveTypeId, r_lSalvageRecoveryTypeId:=lSalvageRecoveryTypeId, r_lTPRecoveryTypeId:=lTPRecoveryTypeId), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimIdsFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSTSClaim")
                Return result
            End If

            If v_bIsNewClaim Then
                'Create a new claim
                m_lReturn = CType(AddNewClaim(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dtLossDate:=v_dtLossDate, v_lClaimHandlerId:=lClaimHandlerId, v_lProgressStatusId:=lProgressStatusID, v_sDescription:=v_sDescription, v_lPrimaryCauseId:=lPrimaryCauseID, v_dtDateReported:=v_dtDateReported, v_lClaimStatusId:=v_lClaimStatusId, v_sClaimNumber:=v_sClaimNumber, v_lPerilTypeId:=lPerilTypeId, v_lReserveTypeId:=lReserveTypeId, v_cReserveAmount:=v_cReserveAmount, v_dtPaymentDate:=v_dtPaymentDate, v_cPaymentAmount:=v_cPaymentAmount, v_lSalvageRecoveryTypeId:=lSalvageRecoveryTypeId, v_cSalvageAmount:=v_cSalvageAmount, v_lTPRecoveryTypeId:=lTPRecoveryTypeId, v_cTPAmount:=v_cTPAmount, v_sInsurerClaimNo:=v_sInsurerClaimNo), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddNewClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSTSClaim")
                    Return result
                End If

            Else
                'Update an existing claim
                m_lReturn = CType(UpdateClaim(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sClaimNumber:=v_sClaimNumber, v_dtLossDate:=v_dtLossDate, v_lClaimHandlerId:=lClaimHandlerId, v_lProgressStatusId:=lProgressStatusID, v_sDescription:=v_sDescription, v_lPrimaryCauseId:=lPrimaryCauseID, v_dtDateReported:=v_dtDateReported, v_lClaimStatusId:=v_lClaimStatusId, v_lPerilTypeId:=lPerilTypeId, v_lReserveTypeId:=lReserveTypeId, v_cReserveAmount:=v_cReserveAmount, v_cPaymentAmount:=v_cPaymentAmount, v_lSalvageRecoveryTypeId:=lSalvageRecoveryTypeId, v_cSalvageAmount:=v_cSalvageAmount, v_lTPRecoveryTypeId:=lTPRecoveryTypeId, v_cTPAmount:=v_cTPAmount, v_sInsurerClaimNo:=v_sInsurerClaimNo), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSTSClaim")
                    Return result
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSTSClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSTSClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddNewClaim
    '
    ' Description:
    '
    ' History: 13/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function AddNewClaim(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dtLossDate As Date, ByVal v_lClaimHandlerId As Integer, ByVal v_lProgressStatusId As Integer, ByVal v_sDescription As String, ByVal v_lPrimaryCauseId As Integer, ByVal v_dtDateReported As Date, ByVal v_lClaimStatusId As Integer, ByVal v_sClaimNumber As String, ByVal v_lPerilTypeId As Integer, ByVal v_lReserveTypeId As Integer, ByVal v_cReserveAmount As Decimal, ByVal v_dtPaymentDate As Date, ByVal v_cPaymentAmount As Decimal, ByVal v_lSalvageRecoveryTypeId As Integer, ByVal v_cSalvageAmount As Decimal, ByVal v_lTPRecoveryTypeId As Integer, ByVal v_cTPAmount As Decimal, ByVal v_sInsurerClaimNo As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACCClientName As Integer = 0
            Const ACCClientShortName As Integer = 1
            Const ACCAddress1 As Integer = 2
            Const ACCAddress2 As Integer = 3
            Const ACCAddress3 As Integer = 4
            Const ACCAddress4 As Integer = 5
            Const ACCPostCode As Integer = 6
            Const ACCTeleHome As Integer = 7
            Const ACCTeleOff As Integer = 8
            Const ACCFax As Integer = 9
            Const ACCMobile As Integer = 10
            Const ACCEmail As Integer = 11

            Const ACIInsurerName As Integer = 0
            Const ACIInsurerShortName As Integer = 1
            Const ACIAddress1 As Integer = 2
            Const ACIAddress2 As Integer = 3
            Const ACIAddress3 As Integer = 4
            Const ACIAddress4 As Integer = 5
            Const ACIPostCode As Integer = 6
            Const ACITeleHome As Integer = 7
            Const ACIFax As Integer = 8
            Const ACIEmail As Integer = 9

            Const ACConTelephoneHomeArea As Integer = 0
            Const ACConTelephoneHomeNumber As Integer = 1
            Const ACConTelephoneHomeExt As Integer = 2

            Const ACConTelephoneOfficeArea As Integer = 3
            Const ACConTelephoneOfficeNumber As Integer = 4
            Const ACConTelephoneOfficeExt As Integer = 5

            Const ACConFaxArea As Integer = 6
            Const ACConFaxNumber As Integer = 7
            Const ACConFaxExt As Integer = 8

            Const ACConMobileArea As Integer = 9
            Const ACConMobileNumber As Integer = 10
            Const ACConMobileExt As Integer = 11

            Const ACConEmail As Integer = 12

            Const PMBEventNewClaim As Integer = 3
            Const ACEventClaimDescription As String = "Claim imported via call to STS"

            Dim oOpenClaim As Object = Nothing

            Dim sPolicyNo As String = ""
            Dim lSecondaryCauseID, lCatastropheCodeID As Integer
            Dim dtLossFromDate, dtLossToDate, dtLastModifiedDate As Date
            Dim lInfoOnly, lLikelyClaim As Integer
            Dim sLocation As String = ""
            Dim lTown As Integer
            Dim vResultArray As Object = Nothing
            Dim lRiskCodeId As Integer
            Dim sClientName, sClientShortName, sClientAddress1, sClientAddress2, sClientAddress3, sClientAddress4, sClientPostCode As String
            Dim lClientAddressCnt As Integer
            Dim sInsurerShortName As String = String.Empty
            Dim sInsurerAddress1 As String = String.Empty
            Dim sInsurerAddress2 As String = String.Empty
            Dim sInsurerAddress3 As String = String.Empty
            Dim sInsurerAddress4 As String = String.Empty
            Dim sInsurerPostCode As String = String.Empty
            Dim lInsurerAddressCnt As Integer
            Dim sClientFaxNo As String = String.Empty
            Dim sClientMobileNo As String = String.Empty
            Dim sClientEMail As String = String.Empty
            Dim sClientClaimNo As String = String.Empty
            Dim sInsurerName As String = String.Empty
            Dim sInsurerTelNo As String = String.Empty
            Dim sInsurerFaxNo As String = String.Empty
            Dim sInsurerEmail As String = String.Empty
            Dim sInsurerContact As String = String.Empty
            Dim lVATRegistered As String = String.Empty
            Dim sVATRegisteredNo As String = String.Empty
            Dim sComments As String = String.Empty
            Dim dtClaimsStatusDate As Date
            Dim sClientTelNoOff As String = String.Empty
            Dim sClientTelNo As String = String.Empty
            Dim lClaimId, lUserDefFldA, lUserDefFldB, lUserDefFldC, lUserDefFldD, lUserDefFldE As Integer
            'developer guide no 17. 
            Dim vUnderwritingYear As Object
            Dim iSourceID As Integer
            Dim lClaimPerilId, lEventCnt, lPartyCnt As Integer

            'Check to make sure the claim does not exist
            If v_sClaimNumber <> "" Then
                m_lReturn = CType(GetClaimDetailsByNumber(v_sClaimNumber:=v_sClaimNumber, r_lClaimId:=lClaimId, r_lRiskCodeId:=lRiskCodeId), gPMConstants.PMEReturnCode)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    WriteSTSError("bSiriusLink.AddNewClaim Claim number " & v_sClaimNumber.Trim() & " already exists")
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Claim number " & v_sClaimNumber.Trim() & " already exists!", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                    Return result
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    WriteSTSError("Error checking if claim exists")
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimDetailsByNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                    Return result
                End If
            End If

            'Get policy number
            m_lReturn = CType(GetInsuranceDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sInsuranceRef:=sPolicyNo, r_iSourceId:=iSourceID, r_lPartyCnt:=lPartyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to retrieve policy number and company id for Insurance file " & v_lInsuranceFileCnt)
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If
            'Create an instance of the open claim business object

            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oOpenClaim, v_sClassName:="bOpenClaim.Business", v_sCallingAppName:=CStr(ACApp), v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to create instance of Open Claim Component")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error: Unable to create instance of Open Claim Component", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If

            'Get the risk

            m_lReturn = oOpenClaim.GetRiskDetails(ToSafeInteger(v_lInsuranceFileCnt), CDate(v_dtLossDate), vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to retrieve risk code id from policy")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOpenClaim.GetRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If

            If Informations.IsArray(vResultArray) Then

                lRiskCodeId = ToSafeInteger(vResultArray(0, 0))
            End If

            'Get the client details

            vResultArray = Nothing

            m_lReturn = oOpenClaim.GetClientDetails(ToSafeInteger(v_lInsuranceFileCnt), ToSafeDate(v_dtLossDate), vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to retrieve client details from policy")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOpenClaim.GetClientDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to retrieve client details from policy")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to find any client details", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If

            sClientName = CStr(vResultArray(ACCClientName))

            sClientShortName = CStr(vResultArray(ACCClientShortName))

            sClientAddress1 = CStr(vResultArray(ACCAddress1)).Trim()

            sClientAddress2 = CStr(vResultArray(ACCAddress2)).Trim()

            sClientAddress3 = CStr(vResultArray(ACCAddress3)).Trim()

            sClientAddress4 = CStr(vResultArray(ACCAddress4)).Trim()

            sClientPostCode = CStr(vResultArray(ACCPostCode)).Trim()

            'Add the client address

            m_lReturn = oOpenClaim.AddAddress(ToSafeString(sClientAddress1), ToSafeString(sClientAddress2), ToSafeString(sClientAddress3), ToSafeString(sClientAddress4), ToSafeString(sClientPostCode), ToSafeInteger(lClientAddressCnt))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                WriteSTSError("bSiriusLink.AddNewClaim failed to add client address for claim")
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed Add Client Address", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If


            If CStr(vResultArray(ACCTeleHome)).Length > 0 Then

                sClientFaxNo = CStr(vResultArray(ACCFax))

                sClientMobileNo = CStr(vResultArray(ACCMobile))

                sClientEMail = CStr(vResultArray(ACCEmail))

                sClientTelNoOff = CStr(vResultArray(ACCTeleOff))

                sClientTelNo = CStr(vResultArray(ACCTeleHome))
            Else
                'Get client contact details

                vResultArray = Nothing

                m_lReturn = oOpenClaim.GetDefaultContacts(v_lPolicyId:=ToSafeInteger(v_lInsuranceFileCnt), r_vResults:=vResultArray, v_bIsClient:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    WriteSTSError("bSiriusLink.AddNewClaim failed to get default client contacts for claim")
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get client contact details", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                    Return result
                End If

                If Informations.IsArray(vResultArray) Then
                    'We have contact details



                    sClientFaxNo = BuildPhoneNumber(sAreaCode:=CStr(vResultArray(ACConFaxArea)), sNumber:=CStr(vResultArray(ACConFaxNumber)), sExtension:=CStr(vResultArray(ACConFaxExt)))



                    sClientMobileNo = BuildPhoneNumber(sAreaCode:=CStr(vResultArray(ACConMobileArea)), sNumber:=CStr(vResultArray(ACConMobileNumber)), sExtension:=CStr(vResultArray(ACConMobileExt)))



                    sClientTelNoOff = BuildPhoneNumber(sAreaCode:=CStr(vResultArray(ACConTelephoneOfficeArea)), sNumber:=CStr(vResultArray(ACConTelephoneOfficeNumber)), sExtension:=CStr(vResultArray(ACConTelephoneOfficeExt)))

                    sClientEMail = CStr(vResultArray(ACConEmail)).Trim()



                    sClientTelNo = BuildPhoneNumber(sAreaCode:=CStr(vResultArray(ACConTelephoneHomeArea)), sNumber:=CStr(vResultArray(ACConTelephoneHomeNumber)), sExtension:=CStr(vResultArray(ACConTelephoneHomeExt)))

                End If
            End If

            'Get the insurer details

            vResultArray = Nothing

            m_lReturn = oOpenClaim.GetInsurerDetails(ToSafeInteger(v_lInsuranceFileCnt), ToSafeDate(v_dtLossDate), vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to retrieve insurer details from policy")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOpenClaim.GetInsurerDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If

            If Informations.IsArray(vResultArray) Then
                'We have an insurer

                sInsurerName = CStr(vResultArray(ACIInsurerName)).Trim()

                sInsurerShortName = CStr(vResultArray(ACIInsurerShortName)).Trim()

                sInsurerAddress1 = CStr(vResultArray(ACIAddress1)).Trim()

                sInsurerAddress2 = CStr(vResultArray(ACIAddress2)).Trim()

                sInsurerAddress3 = CStr(vResultArray(ACIAddress3)).Trim()

                sInsurerAddress4 = CStr(vResultArray(ACIAddress4)).Trim()

                sInsurerPostCode = CStr(vResultArray(ACIPostCode)).Trim()
                'Add the insurer address

                m_lReturn = oOpenClaim.AddAddress(ToSafeString(sInsurerAddress1), ToSafeString(sInsurerAddress2), ToSafeString(sInsurerAddress3), ToSafeString(sInsurerAddress4), ToSafeString(sInsurerPostCode), ToSafeInteger(lInsurerAddressCnt))
                If Not Informations.IsArray(vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    WriteSTSError("bSiriusLink.AddNewClaim failed to add insurer address for claim")
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed Add Insurer Address", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                    Return result
                End If

                If CStr(vResultArray(ACITeleHome)).Length > 0 Then

                    sInsurerTelNo = CStr(vResultArray(ACITeleHome)).Trim()

                    sInsurerFaxNo = CStr(vResultArray(ACIFax)).Trim()

                    sInsurerEmail = CStr(vResultArray(ACIEmail))
                Else
                    'Get insurer contact details

                    vResultArray = Nothing

                    m_lReturn = oOpenClaim.GetDefaultContacts(v_lPolicyId:=ToSafeInteger(v_lInsuranceFileCnt), r_vResults:=vResultArray, v_bIsClient:=False)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        WriteSTSError("bSiriusLink.AddNewClaim failed to get default insurer contacts for claim")
                        bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get insurer contact details", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                        Return result
                    End If

                    If Informations.IsArray(vResultArray) Then
                        'We have contact details
                        'Insurer Fax Number



                        sInsurerFaxNo = BuildPhoneNumber(sAreaCode:=CStr(vResultArray(ACConFaxArea)), sNumber:=CStr(vResultArray(ACConFaxNumber)), sExtension:=CStr(vResultArray(ACConFaxExt)))
                        'Insurer Email

                        sInsurerEmail = CStr(vResultArray(ACConEmail)).Trim()
                    End If

                End If
            End If

            'Set up defaults
            lSecondaryCauseID = 0
            lCatastropheCodeID = 0
            dtLossFromDate = v_dtLossDate
            dtLossToDate = v_dtLossDate
            dtLastModifiedDate = v_dtDateReported
            lInfoOnly = 0
            lLikelyClaim = 0
            sLocation = ""
            lTown = 0
            sClientClaimNo = ""
            sInsurerContact = ""
            lVATRegistered = CStr(0)
            sVATRegisteredNo = ""
            sComments = ""
            dtClaimsStatusDate = v_dtLossDate
            lClaimId = 0
            lUserDefFldA = 0
            lUserDefFldB = 0
            lUserDefFldC = 0
            lUserDefFldD = 0
            lUserDefFldE = 0
            vUnderwritingYear = 0



            oOpenClaim.SetProperties(gPMConstants.PMEComponentAction.PMAdd, ToSafeString(v_sClaimNumber), ToSafeString(sPolicyNo), ToSafeInteger(v_lInsuranceFileCnt), ToSafeString(v_sDescription), ToSafeInteger(v_lClaimStatusId), ToSafeInteger(v_lProgressStatusId), ToSafeInteger(v_lPrimaryCauseId), ToSafeInteger(lSecondaryCauseID), ToSafeInteger(lCatastropheCodeID), ToSafeDate(dtLossFromDate), ToSafeDate(dtLossToDate), ToSafeDate(v_dtDateReported), ToSafeDate(v_dtDateReported), ToSafeDate(dtLastModifiedDate), ToSafeInteger(v_lClaimHandlerId), ToSafeInteger(m_iCurrencyID), ToSafeInteger(lInfoOnly), ToSafeInteger(lLikelyClaim), ToSafeString(sLocation), ToSafeInteger(lTown), ToSafeInteger(lRiskCodeId), ToSafeString(sClientName), ToSafeInteger(lClientAddressCnt), ToSafeString(sClientTelNo), ToSafeString(sClientFaxNo), ToSafeString(sClientMobileNo), ToSafeString(sClientEMail), ToSafeString(sClientClaimNo), ToSafeString(sInsurerName), ToSafeInteger(lInsurerAddressCnt), ToSafeString(sInsurerTelNo), ToSafeString(sInsurerFaxNo), ToSafeString(sInsurerEmail), ToSafeString(v_sInsurerClaimNo), ToSafeString(sInsurerContact), ToSafeString(lVATRegistered), ToSafeString(sVATRegisteredNo), ToSafeString(sComments), ToSafeDate(dtClaimsStatusDate), ToSafeString(sClientShortName), ToSafeString(sInsurerShortName), ToSafeString(sClientTelNoOff), ToSafeInteger(lClaimId), ToSafeInteger(lUserDefFldA), ToSafeInteger(lUserDefFldB), ToSafeInteger(lUserDefFldC), ToSafeInteger(lUserDefFldD), ToSafeInteger(lUserDefFldE), ToSafeInteger(iSourceID), ToSafeInteger(m_iLanguageID), CType(vUnderwritingYear, Object))


            m_lReturn = oOpenClaim.Add
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to get add new claim to database")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add claim", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")

                oOpenClaim.Dispose()
                oOpenClaim = Nothing
                Return result
            End If


            lClaimId = oOpenClaim.ClaimId

            'Create an event log entry

            m_lReturn = oOpenClaim.CreateEvent(r_lEventCnt:=ToSafeInteger(lEventCnt), v_vClaimCnt:=ToSafeInteger(lClaimId), v_vDescription:=ToSafeString(ACEventClaimDescription), v_lEventTypeId:=ToSafeInteger(PMBEventNewClaim), v_lPartyid:=ToSafeInteger(lPartyCnt))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to add an entry to the event log")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOpenClaim.CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")

                oOpenClaim.Dispose()
                oOpenClaim = Nothing
                Return result
            End If


            oOpenClaim.Dispose()
            oOpenClaim = Nothing


            If v_sClaimNumber <> "" Then
                'Update the claim number with the one provided
                m_lReturn = CType(UpdateClaimNumber(v_sClaimNumber:=v_sClaimNumber, v_lClaimId:=lClaimId), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    WriteSTSError("bSiriusLink.AddNewClaim failed to replace generated claim number with " & v_sClaimNumber)
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                    Return result
                End If
            End If

            If v_lPerilTypeId = 0 Then
                'We have no peril so no more processing can take place
                Return result
            End If

            'Add the claim risk and claim_peril tables
            m_lReturn = CType(AddRiskAndPerils(v_lClaimId:=lClaimId, v_lPerilTypeId:=v_lPerilTypeId, v_lRiskCodeID:=lRiskCodeId, r_lClaimPerilId:=lClaimPerilId), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to get add new risk and peril entries")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRiskAndPerils Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If

            'Update the reserve
            m_lReturn = CType(UpdateReserve(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCodeID:=lRiskCodeId, v_lPerilId:=lClaimPerilId, v_lClaimId:=lClaimId, v_lReserveTypeId:=v_lReserveTypeId, v_cReserveAmount:=v_cReserveAmount, v_cPaymentAmount:=v_cPaymentAmount, v_bUpdating:=False), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("bSiriusLink.AddNewClaim failed to update reserve")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateReserve Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If

            'Update Third Party Recovery
            m_lReturn = CType(UpdateThirdPartyRecovery(v_lClaimPerilId:=lClaimPerilId, v_lTPRecoveryTypeId:=v_lTPRecoveryTypeId, v_cTPAmount:=v_cTPAmount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("Error: Failed to update third party recovery")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateThirdPartyRecovery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If

            'Update Salvage Recovery
            m_lReturn = CType(UpdateSalvageRecovery(v_lClaimPerilId:=lClaimPerilId, v_lSalvageRecoveryTypeId:=v_lSalvageRecoveryTypeId, v_cSalvageAmount:=v_cSalvageAmount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                WriteSTSError("Error: Failed to update salvage recovery")
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateSalvageRecovery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddNewClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateClaim
    '
    ' Description:
    '
    ' History: 19/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateClaim(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sClaimNumber As String, ByVal v_dtLossDate As Date, ByVal v_lClaimHandlerId As Integer, ByVal v_lProgressStatusId As Integer, ByVal v_sDescription As String, ByVal v_lPrimaryCauseId As Integer, ByVal v_dtDateReported As Date, ByVal v_lClaimStatusId As Integer, ByVal v_lPerilTypeId As Integer, ByVal v_lReserveTypeId As Integer, ByVal v_cReserveAmount As Decimal, ByVal v_cPaymentAmount As Decimal, ByVal v_lSalvageRecoveryTypeId As Integer, ByVal v_cSalvageAmount As Decimal, ByVal v_lTPRecoveryTypeId As Integer, ByVal v_cTPAmount As Decimal, ByVal v_sInsurerClaimNo As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lClaimPerilId, lClaimId, lRiskCodeId As Integer

        'Get the claim id for the claim
        m_lReturn = CType(GetClaimDetailsByNumber(v_sClaimNumber:=v_sClaimNumber, r_lClaimId:=lClaimId, r_lRiskCodeId:=lRiskCodeId), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            WriteSTSError("Error: Failed to get details for claim " & v_sClaimNumber)
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimDetailsByNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaim")
            Return result
        End If

        'Update the details on the claim record
        m_lReturn = CType(UpdateClaimDetails(v_lClaimId:=lClaimId, v_dtLossDate:=v_dtLossDate, v_lClaimHandlerId:=v_lClaimHandlerId, v_lProgressStatusId:=v_lProgressStatusId, v_sDescription:=v_sDescription, v_dtDateReported:=v_dtDateReported, v_lClaimStatusId:=v_lClaimStatusId, v_sInsurerClaimNo:=v_sInsurerClaimNo), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateClaimDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaim")
            Return result
        End If

        If v_lPerilTypeId = 0 Then
            'We have no peril so no more processing can take place
            Return result
        End If

        'Add the claim risk and claim_peril tables
        m_lReturn = CType(AddRiskAndPerils(v_lClaimId:=lClaimId, v_lPerilTypeId:=v_lPerilTypeId, v_lRiskCodeID:=lRiskCodeId, r_lClaimPerilId:=lClaimPerilId), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            WriteSTSError("Error: Failed to update risk and peril ")
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRiskAndPerils Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaim")
            Return result
        End If

        'Update the reserve
        m_lReturn = CType(UpdateReserve(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCodeID:=lRiskCodeId, v_lPerilId:=lClaimPerilId, v_lClaimId:=lClaimId, v_lReserveTypeId:=v_lReserveTypeId, v_cReserveAmount:=v_cReserveAmount, v_cPaymentAmount:=v_cPaymentAmount, v_bUpdating:=True), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            WriteSTSError("Error: Failed to update reserve details")
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateReserve Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaim")
            Return result
        End If

        'Update Third Party Recovery
        m_lReturn = CType(UpdateThirdPartyRecovery(v_lClaimPerilId:=lClaimPerilId, v_lTPRecoveryTypeId:=v_lTPRecoveryTypeId, v_cTPAmount:=v_cTPAmount), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            WriteSTSError("Error: Failed to update third party recovery details")
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateThirdPartyRecovery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaim")
            Return result
        End If

        'Update Salvage Recovery
        m_lReturn = CType(UpdateSalvageRecovery(v_lClaimPerilId:=lClaimPerilId, v_lSalvageRecoveryTypeId:=v_lSalvageRecoveryTypeId, v_cSalvageAmount:=v_cSalvageAmount), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            WriteSTSError("Error: Failed to update salvage recovery details")
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateSalvageRecovery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaim")
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateClaimNumber
    '
    ' Description:
    '
    ' History: 19/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateClaimNumber(ByVal v_sClaimNumber As String, ByRef v_lClaimId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' claim_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' claim_number
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_number", vValue:=v_sClaimNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACClaimNumberUpdSQL, sSQLName:=ACClaimNumberUpdName, bStoredProcedure:=ACClaimNumberUpdStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetClaimDetailsByNumber
    '
    ' Description:
    '
    ' History: 19/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetClaimDetailsByNumber(ByVal v_sClaimNumber As String, ByRef r_lClaimId As Integer, ByRef r_lRiskCodeId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing

            m_oDatabase.Parameters.Clear()

            ' claim_number
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_number", vValue:=v_sClaimNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACClaimDetailsByNumberSQL, sSQLName:=ACClaimDetailsByNumberName, bStoredProcedure:=ACClaimDetailsByNumberStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lClaimId = ToSafeInteger(CStr(vResultArray(0, 0)).Trim())

            r_lRiskCodeId = ToSafeInteger(vResultArray(1, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimDetailsByNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimDetailsByNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsuranceDetails
    '
    ' Description:
    '
    ' History: 19/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetInsuranceDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sInsuranceRef As String, ByRef r_iSourceId As Integer, ByRef r_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing

            m_oDatabase.Parameters.Clear()

            ' insurance_file_cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsuranceFileSelSQL, sSQLName:=ACInsuranceFileSelName, bStoredProcedure:=ACInsuranceFileSelStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_sInsuranceRef = CStr(vResultArray(7, 0)).Trim()

            r_iSourceId = ToSafeInteger(vResultArray(5, 0))

            r_lPartyCnt = ToSafeInteger(vResultArray(13, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetReserveTypeDetailsFromName
    '
    ' Description:
    '
    ' History: 19/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetReserveTypeDetailsFromName(ByVal v_sName As String, Optional ByRef r_lId As Integer = 0, Optional ByRef r_sDescription As String = "") As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing

            m_oDatabase.Parameters.Clear()

            ' insurance_file_cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="name", vValue:=v_sName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACReserveTypeSelSQL, sSQLName:=ACReserveTypeSelName, bStoredProcedure:=ACReserveTypeSelStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_sDescription = CStr(vResultArray(1, 0))

            r_lId = ToSafeInteger(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReserveTypeDetailsFromName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveTypeDetailsFromName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPerilId
    '
    ' Description:
    '
    ' History: 19/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetClaimPerilId(ByVal v_lClaimId As Integer, ByVal v_lPerilTypeId As Integer, ByRef r_lClaimPerilId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing

            m_oDatabase.Parameters.Clear()

            ' claim_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' claim_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="peril_type_id", vValue:=CStr(v_lPerilTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACClaimPerilByTypeSQL, sSQLName:=ACClaimPerilByTypeName, bStoredProcedure:=ACClaimPerilByTypeStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lClaimPerilId = ToSafeInteger(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimPerilId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimPerilId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BuildPhoneNumber
    '
    ' Description: Formats the display of a phone number into a single
    '              text field.
    '
    ' ***************************************************************** '
    Private Function BuildPhoneNumber(ByVal sAreaCode As String, ByVal sNumber As String, ByVal sExtension As String) As String
        Dim result As String = String.Empty
        Dim ACApp As String = "bSiriusLink"

        Dim sPhoneNumber As String = ""



        result = ""

        If sAreaCode.Trim().Length > 0 Then
            sPhoneNumber = "(" & sAreaCode.Trim() & ") "
        End If

        If sNumber.Trim().Length > 0 Then
            sPhoneNumber = sPhoneNumber & sNumber.Trim() & " "
        End If

        If sExtension.Trim().Length > 0 Then
            sPhoneNumber = sPhoneNumber & "ext " & sExtension.Trim()
        End If


        Return sPhoneNumber

    End Function
    ' ***************************************************************** '
    ' Name: AddRiskAndPerils
    '
    ' Description:
    '
    ' History: 18/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function AddRiskAndPerils(ByVal v_lClaimId As Integer, ByVal v_lPerilTypeId As Integer, ByVal v_lRiskCodeID As Integer, ByRef r_lClaimPerilId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oRiskDetails As bCLMRiskDetails.Business

        oRiskDetails = New bCLMRiskDetails.Business
        m_lReturn = CType(oRiskDetails.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bCLMRiskDetails.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRiskAndPerils")
            WriteSTSError("bSiriusLink.AddRiskAndPerils Failed to create bCLMRiskDetails.Business")
            Return result
        End If

        'See if we already have a claim peril set up
        m_lReturn = CType(GetClaimPerilId(v_lClaimId:=v_lClaimId, v_lPerilTypeId:=v_lPerilTypeId, r_lClaimPerilId:=r_lClaimPerilId), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oRiskDetails.AddClaimRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimPerilId")
            WriteSTSError("bSiriusLink.AddRiskAndPerils GetClaimPerilId failed")

            Return result
        End If

        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            'Add the claim risk

            m_lReturn = oRiskDetails.AddClaimRisk(v_lClaimId:=ToSafeInteger(v_lClaimId), v_lRiskTypeId:=ToSafeInteger(v_lRiskCodeID), v_sDescription:="", v_sComments:="")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oRiskDetails.AddClaimRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRiskAndPerils")
                WriteSTSError("bSiriusLink.AddRiskAndPerils AddClaimRisk failed")
                Return result
            End If

            'No existing record
            If ValidPerilType(r_oRiskDetails:=oRiskDetails, v_lClaimId:=v_lClaimId, v_lPerilTypeId:=v_lPerilTypeId) Then

                'Add a new claim peril record

                m_lReturn = oRiskDetails.AddClaimPeril(v_lClaimId:=ToSafeInteger(v_lClaimId), v_lPerilTypeID:=ToSafeInteger(v_lPerilTypeId), r_lClaimPerilId:=r_lClaimPerilId, v_lRiskId:=0, v_sDescription:="")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oRiskDetails.AddClaimPeril Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRiskAndPerils")
                    WriteSTSError("bSiriusLink.AddRiskAndPerils AddClaimPeril failed")
                    Return result
                End If
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Peril Type - " & v_lPerilTypeId, vApp:=ACApp, vClass:=ACClass, vMethod:="AddRiskAndPerils")
                WriteSTSError("bSiriusLink.AddRiskAndPerils Invalid Peril Type - " & v_lPerilTypeId)
                Return result
            End If
        End If


        oRiskDetails.Dispose()

        oRiskDetails = Nothing

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: UpdateReserve
    '
    ' Description:
    '
    ' History: 18/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateReserve(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lPerilId As Integer, ByVal v_lClaimId As Integer, ByVal v_lReserveTypeId As Integer, ByVal v_cReserveAmount As Decimal, ByVal v_cPaymentAmount As Decimal, Optional ByVal v_bUpdating As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim oCLMPeril As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACRInReserveId As Integer = 0
            Const ACRInInitialReserve As Integer = 1
            Const ACRInPaidToDate As Integer = 2
            Const ACRInRevisedReserve As Integer = 3
            Const ACRInReserveTypeId As Integer = 7

            Const ACROutReserveId As Integer = 0
            Const ACROutInitialReserve As Integer = 1
            Const ACROutRevisedReserve As Integer = 2
            Const ACROutPaidToDate As Integer = 4
            Const ACROutRevisedReserveEntered As Integer = 7
            Const ACROutSize As Integer = 7

            Dim vReserveDetailsArray As Object = Nothing
            Dim iInstance As Integer
            Dim bFound As Boolean
            Dim lReserveId As Integer
            Dim cInitialReserve, cRevisedReserve, cPaidToDate As Decimal
            Dim vControlArray As Object = Nothing

            If v_lReserveTypeId = 0 Then
                'Nothing to do
                Return result
            End If

            'oCLMPeril = New bCLMPeril.Business
            oCLMPeril = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=oCLMPeril, v_sClassName:="bCLMPeril.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bSIRRiskScreen.Business"
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bCLMPeril.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If


            oCLMPeril.PerilID = v_lPerilId

            oCLMPeril.ClaimId = v_lClaimId

            m_lReturn = oCLMPeril.GetControls(vControlArray)

            'Get the reserve details

            m_lReturn = oCLMPeril.GetReserveDetails(v_vPolicyID:=ToSafeInteger(v_lInsuranceFileCnt), v_vRiskID:=ToSafeInteger(v_lRiskCodeID), r_vReserveDetailsArray:=vReserveDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oCLMPeril.GetReserveDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReserve")

                oCLMPeril.Dispose()
                oCLMPeril = Nothing
                Return result
            End If

            'Find the reserve type we want in the array
            bFound = False

            For i As Integer = 0 To vReserveDetailsArray.GetUpperBound(1)

                If CDbl(vReserveDetailsArray(ACRInReserveTypeId, i)) = v_lReserveTypeId Then
                    bFound = True
                    iInstance = i
                    Exit For
                End If
            Next i

            If Not bFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to to find reserve type in array - " & v_lReserveTypeId, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReserve")

                oCLMPeril.Dispose()
                oCLMPeril = Nothing
                Return result
            End If

            'Get data from the array

            lReserveId = ToSafeInteger(vReserveDetailsArray(ACRInReserveId, iInstance))

            cInitialReserve = Val(CStr(vReserveDetailsArray(ACRInInitialReserve, iInstance)))

            cRevisedReserve = Val(CStr(vReserveDetailsArray(ACRInRevisedReserve, iInstance)))

            cPaidToDate = Val(CStr(vReserveDetailsArray(ACRInPaidToDate, iInstance)))

            'Construct the update array
            ReDim vReserveDetailsArray(ACROutSize, 0)
            iInstance = 0 '2004-09-16 'prevent array bounds error

            vReserveDetailsArray(ACROutReserveId, iInstance) = lReserveId
            If v_bUpdating Then
                'Initial reserve

                vReserveDetailsArray(ACROutInitialReserve, iInstance) = cInitialReserve
                'Revised reserve

                vReserveDetailsArray(ACROutRevisedReserve, iInstance) = v_cReserveAmount

                vReserveDetailsArray(ACROutRevisedReserveEntered, iInstance) = 1
            Else
                'Reserve amount

                vReserveDetailsArray(ACROutInitialReserve, iInstance) = v_cReserveAmount
                cInitialReserve = v_cReserveAmount

                vReserveDetailsArray(ACROutRevisedReserve, iInstance) = v_cReserveAmount

                vReserveDetailsArray(ACROutRevisedReserveEntered, iInstance) = 0
            End If
            'Paid to date
            cPaidToDate += v_cPaymentAmount

            vReserveDetailsArray(ACROutPaidToDate, iInstance) = cPaidToDate

            'Update the reserve details

            m_lReturn = oCLMPeril.UpdateReserveDetails(v_vReserveDetailsArray:=vReserveDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oCLMPeril.UpdateReserveDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReserve")
                Return result
            End If

            'Update the payment
            m_lReturn = CType(UpdateReservePayment(r_oCLMPeril:=oCLMPeril, v_iInstance:=iInstance, v_cRevisedReserve:=v_cPaymentAmount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateReservePayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReserve")
                Return result
            End If


            oCLMPeril.Dispose()
            oCLMPeril = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateReserve Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReserve", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            oCLMPeril.Dispose()

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateSalvageRecovery
    '
    ' Description:
    '
    ' History: 19/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateSalvageRecovery(ByVal v_lClaimPerilId As Integer, ByVal v_lSalvageRecoveryTypeId As Integer, ByVal v_cSalvageAmount As Decimal) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Const ACRecovery As Integer = 0

        Dim oSalvageRecovery As Object = Nothing
        Dim lTempRecoveryTypeId, lRecoveryId As Integer
        Dim vRecoveryId, lCurrentRecord As Integer
        Dim bEOF As Boolean

        lRecoveryId = 0

        If v_lSalvageRecoveryTypeId = 0 Then
            Return result
        End If


        m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oSalvageRecovery, v_sClassName:="bCLMSalvageRecovery.Business", v_sCallingAppName:=CStr(ACApp), v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bCLMSalvageRecovery.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSalvageRecovery")
            oSalvageRecovery = Nothing
            Return result
        End If

        'Load the details for the current claim peril

        m_lReturn = oSalvageRecovery.GetDetails(vPerilId:=ToSafeInteger(v_lClaimPerilId))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSalvageRecovery.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSalvageRecovery")
            oSalvageRecovery = Nothing
            Return result
        End If

        'Search to see if we have a record for this recovery type
        If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            bEOF = False
            While Not bEOF

                m_lReturn = oSalvageRecovery.GetNext(vRecoveryId:=ToSafeInteger(vRecoveryId), vRecoveryTypeId:=ToSafeInteger(lTempRecoveryTypeId))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bEOF = True
                Else


                    If oSalvageRecovery.CurrentRecord >= oSalvageRecovery.RecordCount Then
                        bEOF = True
                    End If
                    If lTempRecoveryTypeId = v_lSalvageRecoveryTypeId Then
                        'We have an existing record
                        lRecoveryId = vRecoveryId

                        lCurrentRecord = oSalvageRecovery.CurrentRecord
                    End If
                End If
            End While
        End If

        If lRecoveryId = 0 Then
            'New Record

            lCurrentRecord = ToSafeInteger(oSalvageRecovery.RecordCount + 1)

            m_lReturn = oSalvageRecovery.EditAdd(lRow:=ToSafeInteger(lCurrentRecord), vRecoveryTypeId:=ToSafeInteger(v_lSalvageRecoveryTypeId), vPerilId:=ToSafeInteger(v_lClaimPerilId), vCurrencyID:=ToSafeInteger(m_iCurrencyID), vIntialReserve:=ToSafeDecimal(v_cSalvageAmount), vRevisedReserve:=0, vTable:=ToSafeInteger(ACRecovery), vTaxAmount:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSalvageRecovery.EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSalvageRecovery")
                oSalvageRecovery = Nothing
                Return result
            End If
        Else

            m_lReturn = oSalvageRecovery.EditUpdate(lRow:=ToSafeInteger(lCurrentRecord), vRevisedReserve:=ToSafeDecimal(v_cSalvageAmount))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSalvageRecovery.EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSalvageRecovery")
                oSalvageRecovery = Nothing
                Return result
            End If
        End If

        'Update the database

        m_lReturn = oSalvageRecovery.Update
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSalvageRecovery.Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSalvageRecovery")
            oSalvageRecovery = Nothing
            Return result
        End If


        oSalvageRecovery.Dispose()
        oSalvageRecovery = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateThirdPartyRecovery
    '
    ' Description:
    '
    ' History: 19/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateThirdPartyRecovery(ByVal v_lClaimPerilId As Integer, ByVal v_lTPRecoveryTypeId As Integer, ByVal v_cTPAmount As Decimal) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Const ACRecovery As Integer = 0

        Dim oThirdPartyRecovery As Object = Nothing
        Dim lTempRecoveryTypeId, lRecoveryId As Integer
        Dim vRecoveryId, lCurrentRecord As Integer
        Dim bEOF As Boolean

        lRecoveryId = 0

        If v_lTPRecoveryTypeId = 0 Then
            Return result
        End If

        m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oThirdPartyRecovery, v_sClassName:="bCLMThirdParty.Business", v_sCallingAppName:=CStr(ACApp), v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bCLMSalvageRecovery.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateThirdPartyRecovery")
            oThirdPartyRecovery = Nothing
            Return result
        End If

        'Load the details for the current claim peril

        m_lReturn = oThirdPartyRecovery.GetDetails(vPerilId:=ToSafeInteger(v_lClaimPerilId))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oThirdPartyRecovery.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateThirdPartyRecovery")
            oThirdPartyRecovery = Nothing
            Return result
        End If

        'Search to see if we have a record for this recovery type
        If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            bEOF = False
            While Not bEOF

                m_lReturn = oThirdPartyRecovery.GetNext(vRecoveryId:=ToSafeInteger(vRecoveryId), vRecoveryTypeId:=ToSafeInteger(lTempRecoveryTypeId))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bEOF = True
                Else


                    If oThirdPartyRecovery.CurrentRecord >= oThirdPartyRecovery.RecordCount Then
                        bEOF = True
                    End If
                    If lTempRecoveryTypeId = v_lTPRecoveryTypeId Then
                        'We have an existing record
                        lRecoveryId = vRecoveryId

                        lCurrentRecord = oThirdPartyRecovery.CurrentRecord
                    End If
                End If
            End While
        End If

        If lRecoveryId = 0 Then
            'New Record

            lCurrentRecord = ToSafeInteger(oThirdPartyRecovery.RecordCount + 1)

            m_lReturn = oThirdPartyRecovery.EditAdd(lRow:=ToSafeInteger(lCurrentRecord), vRecoveryTypeId:=ToSafeInteger(v_lTPRecoveryTypeId), vPerilId:=ToSafeInteger(v_lClaimPerilId), vCurrencyID:=ToSafeInteger(m_iCurrencyID), vIntialReserve:=ToSafeDecimal(v_cTPAmount), vTable:=ToSafeInteger(ACRecovery), vTaxAmount:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oThirdPartyRecovery.EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateThirdPartyRecovery")
                oThirdPartyRecovery = Nothing
                Return result
            End If
        Else

            m_lReturn = oThirdPartyRecovery.EditUpdate(lRow:=ToSafeInteger(lCurrentRecord), vRevisedReserve:=ToSafeDecimal(v_cTPAmount))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oThirdPartyRecovery.EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateThirdPartyRecovery")
                oThirdPartyRecovery = Nothing
                Return result
            End If
        End If

        'Update the database

        m_lReturn = oThirdPartyRecovery.Update
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oThirdPartyRecovery.Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateThirdPartyRecovery")
            oThirdPartyRecovery = Nothing
            Return result
        End If


        oThirdPartyRecovery.Dispose()
        oThirdPartyRecovery = Nothing

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: UpdateReservePayment
    '
    ' Description:
    '
    ' History: 19/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateReservePayment(ByRef r_oCLMPeril As Object, ByVal v_iInstance As Integer, ByVal v_cRevisedReserve As Decimal) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vInPaymentDetailsArray As Object = Nothing
        Dim vOutPaymentDetailsArray As Object = Nothing


        Const ACPInPaymentId As Integer = 0

        Const ACPOutPaymentId As Integer = 0
        Const ACPOutRevisedReserve As Integer = 1
        Const ACPOutSize As Integer = 11

        If v_cRevisedReserve = 0 Then
            Return result
        End If

        'Get the payment details

        m_lReturn = r_oCLMPeril.GetPaymentDetails(vInPaymentDetailsArray)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            If Not Informations.IsArray(vInPaymentDetailsArray) Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            Else

                If vInPaymentDetailsArray.GetUpperBound(1) < v_iInstance Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="r_oCLMPeril.GetPaymentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReservePayment")
            Return result
        End If


        ReDim vOutPaymentDetailsArray(ACPOutSize, vInPaymentDetailsArray.GetUpperBound(1))

        For i As Integer = 0 To vInPaymentDetailsArray.GetUpperBound(1)


            vOutPaymentDetailsArray(ACPOutPaymentId, i) = vInPaymentDetailsArray(ACPInPaymentId, i)
            If i = v_iInstance Then

                vOutPaymentDetailsArray(ACPOutRevisedReserve, i) = v_cRevisedReserve
            Else

                vOutPaymentDetailsArray(ACPOutRevisedReserve, i) = 0
            End If
        Next i

        'Update payment details

        m_lReturn = r_oCLMPeril.UpdatePaymentDetails(vOutPaymentDetailsArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="r_oCLMPeril.UpdatePaymentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReservePayment")
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ValidPerilType
    '
    ' Description:
    '
    ' History: 18/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function ValidPerilType(ByRef r_oRiskDetails As Object, ByVal v_lClaimId As Object, ByVal v_lPerilTypeId As Integer) As Boolean
        Dim result As Boolean = False
        Dim ACApp As String = "bSiriusLink"



        Dim vResultArray As Object = Nothing



        m_lReturn = r_oRiskDetails.GetPerilTypeForRisk(v_lClaimId:=ToSafeInteger(v_lClaimId), v_lRisk:=0, v_lPolicy:=0, r_vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = False
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="r_oRiskDetails.GetPerilTypeForRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidPerilType")
            Return result
        End If

        If Not Informations.IsArray(vResultArray) Then
            'No codes left to be selected
            Return result
        End If

        'These are the ids which are available

        For i As Integer = 0 To vResultArray.GetUpperBound(1)

            If CDbl(vResultArray(0, i)) = v_lPerilTypeId Then
                Return True
            End If
        Next i

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: WriteSTSError
    '
    ' Description:
    '
    ' History: 20/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Sub WriteSTSError(ByVal v_sSTSErrorDescription As String)
        Dim ACApp As String = "bSiriusLink"



        If m_sSTSErrorDescription = "" Then
            m_sSTSErrorDescription = v_sSTSErrorDescription
            m_lSTSErrorReturnValue = gPMConstants.PMEReturnCode.PMError
        End If


    End Sub

    ' ***************************************************************** '
    ' Name: GetClaimIdsFromCode
    '
    ' Description:
    '
    ' History: 21/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetClaimIdsFromCode(ByVal v_sClaimHandler As String, ByVal v_sProgressStatus As String, ByVal v_sPrimaryCause As String, ByVal v_sPerilType As String, ByVal v_sReserveType As String, ByVal v_sSalvageRecoveryType As String, ByVal v_sTPRecoveryType As String, ByRef r_lClaimHandlerId As Integer, ByRef r_lProgressStatusId As Integer, ByRef r_lPrimaryCauseId As Integer, ByRef r_lPerilTypeId As Integer, ByRef r_lReserveTypeId As Integer, ByRef r_lSalvageRecoveryTypeId As Integer, ByRef r_lTPRecoveryTypeId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sMessage As String = ""

        'Claim Handler
        If v_sClaimHandler <> "" Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="handler", v_sCode:=v_sClaimHandler, v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lID:=r_lClaimHandlerId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_lClaimHandlerId = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Unable to get lookup id for claim handler - " & v_sClaimHandler
                WriteSTSError(sMessage)
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIdsFromCode")
                Return result
            End If
        End If
        'Progress Status
        If v_sClaimHandler <> "" Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="progress_status", v_sCode:=v_sProgressStatus, v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lID:=r_lProgressStatusId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_lProgressStatusId = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Unable to get lookup id for progress status - " & v_sProgressStatus
                WriteSTSError(sMessage)
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIdsFromCode")
                Return result
            End If
        End If
        'Primary Cause
        If v_sPrimaryCause <> "" Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="primary_cause", v_sCode:=v_sPrimaryCause, v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lID:=r_lPrimaryCauseId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_lPrimaryCauseId = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Unable to get lookup id for primary cause - " & v_sPrimaryCause
                WriteSTSError(sMessage)
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIdsFromCode")
                Return result
            End If
        End If
        'Peril Type
        If v_sPerilType <> "" Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="peril_type", v_sCode:=v_sPerilType, v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lID:=r_lPerilTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_lPerilTypeId = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Unable to get lookup id for peril type - " & v_sPerilType
                WriteSTSError(sMessage)
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIdsFromCode")
                Return result
            End If
        End If
        'Reserve Type
        If v_sReserveType <> "" Then
            m_lReturn = CType(GetReserveTypeDetailsFromName(v_sName:=v_sReserveType, r_lId:=r_lReserveTypeId), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_lReserveTypeId = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Unable to get lookup id for reserve type - " & v_sReserveType
                WriteSTSError(sMessage)
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIdsFromCode")
                Return result
            End If
        End If

        'Salvage Recovery Type
        If v_sSalvageRecoveryType <> "" Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="recovery_type", v_sCode:=v_sSalvageRecoveryType, v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lID:=r_lSalvageRecoveryTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_lSalvageRecoveryTypeId = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Unable to get lookup id for salvage recovery type - " & v_sSalvageRecoveryType
                WriteSTSError(sMessage)
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIdsFromCode")
                Return result
            End If
        End If
        'Third Party Recovery Type
        If v_sTPRecoveryType <> "" Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="recovery_type", v_sCode:=v_sTPRecoveryType, v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lID:=r_lTPRecoveryTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_lTPRecoveryTypeId = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Unable to get lookup id for thrid party recovery type - " & v_sTPRecoveryType
                WriteSTSError(sMessage)
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIdsFromCode")
                Return result
            End If
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: UpdateClaimDetails
    '
    ' Description:
    '
    ' History: 21/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateClaimDetails(ByVal v_lClaimId As Integer, ByVal v_dtLossDate As Date, ByVal v_lClaimHandlerId As Integer, ByVal v_lProgressStatusId As Integer, ByVal v_sDescription As String, ByVal v_dtDateReported As Date, ByVal v_lClaimStatusId As Integer, ByVal v_sInsurerClaimNo As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue


        Dim oOpenClaim As Object = Nothing

        Dim iSourceID, iLanguageID As Integer
        Dim vUnderwritingYearID As Object = Nothing

        Dim sClaimNo As String = String.Empty
        Dim sPolicyNo As String = String.Empty
        Dim lPolicyID As Integer
        Dim sDescription As String = ""
        Dim lClaimStatusID As String = String.Empty
        Dim lProgressStatusID As String = String.Empty
        Dim lPrimaryCauseID As String = String.Empty
        Dim lSecondaryCauseID As String = String.Empty
        Dim lCatastropheCodeID As Integer
        Dim sLossFromDate As String = String.Empty
        Dim sLossToDate As String = String.Empty
        Dim sReportedDate As String = String.Empty
        Dim sReportedToDate As String = String.Empty
        Dim sLastModifiedDate As String = String.Empty
        Dim lHandlerID, lCurrencyID As Integer
        Dim nInfoOnly, nLikelyClaim As Integer
        Dim sLocation As String = ""
        Dim lTown, lRiskTypeID As Integer
        Dim sClientName As String = ""
        Dim sClientAddress As Integer
        Dim sClientTelNo As String = String.Empty
        Dim sClientTelNoOff As String = String.Empty
        Dim sClientFaxNo As String = String.Empty
        Dim sClientMobileNo As String = String.Empty
        Dim sClientEMail As String = String.Empty
        Dim sClientClaimNo As String = String.Empty
        Dim sInsurerName As String = String.Empty
        Dim sInsurerAddress As Integer
        Dim sInsurerTelNo As String = String.Empty
        Dim sInsurerFaxNo As String = String.Empty
        Dim sInsurerEmail As String = String.Empty
        Dim sInsurerClaimNo As String = String.Empty
        Dim sInsurerContact As String = String.Empty
        Dim nVATRegistered As Integer
        Dim sVATRegisteredNo As String = String.Empty
        Dim sComments As String = String.Empty
        Dim sClaimsStatusDate As String = String.Empty
        Dim sClientShortName As String = String.Empty
        Dim sInsurerShortName As String = String.Empty
        Dim lUserDefFldA, lUserDefFldB, lUserDefFldC, lUserDefFldD, lUserDefFldE As Integer

        'Create an instance of the open claim business object

        m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oOpenClaim, v_sClassName:="bOpenClaim.Business", v_sCallingAppName:=CStr(ACApp), v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error: Unable to create instance of Open Claim Component", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimDetails")
            Return result
        End If

        'Set the Claim ID in the Business Object

        oOpenClaim.SetKeyID(ToSafeInteger(v_lClaimId))

        'Select the Claim from the Database

        m_lReturn = oOpenClaim.SelectSingle

        'Get the data

        oOpenClaim.GetProperties(gPMConstants.PMEComponentAction.PMEdit, ToSafeString(sClaimNo), ToSafeString(sPolicyNo), ToSafeInteger(lPolicyID), ToSafeString(sDescription), ToSafeString(lClaimStatusID), ToSafeString(lProgressStatusID), ToSafeString(lPrimaryCauseID), ToSafeString(lSecondaryCauseID), ToSafeInteger(lCatastropheCodeID), ToSafeString(sLossFromDate), ToSafeString(sLossToDate), ToSafeString(sReportedDate), ToSafeString(sReportedToDate), ToSafeString(sLastModifiedDate), ToSafeInteger(lHandlerID), ToSafeInteger(lCurrencyID), ToSafeInteger(nInfoOnly), ToSafeInteger(nLikelyClaim), ToSafeString(sLocation), ToSafeInteger(lTown), ToSafeInteger(lRiskTypeID), ToSafeString(sClientName), ToSafeString(sClientAddress), ToSafeString(sClientTelNo), ToSafeString(sClientFaxNo), ToSafeString(sClientMobileNo), ToSafeString(sClientEMail), ToSafeString(sClientClaimNo), ToSafeString(sInsurerName), ToSafeString(sInsurerAddress), ToSafeString(sInsurerTelNo), ToSafeString(sInsurerFaxNo), ToSafeString(sInsurerEmail), ToSafeString(sInsurerClaimNo), ToSafeString(sInsurerContact), ToSafeInteger(nVATRegistered), ToSafeString(sVATRegisteredNo), ToSafeString(sComments), ToSafeString(sClaimsStatusDate), ToSafeString(sClientShortName), ToSafeString(sInsurerShortName), ToSafeString(sClientTelNoOff), ToSafeInteger(lUserDefFldA), ToSafeInteger(lUserDefFldB), ToSafeInteger(lUserDefFldC), ToSafeInteger(lUserDefFldD), ToSafeInteger(lUserDefFldE), ToSafeInteger(iSourceID), ToSafeInteger(iLanguageID), CType(vUnderwritingYearID, Object))

        'Update the data
        sLossToDate = v_dtLossDate.ToString()
        sLossFromDate = v_dtLossDate.ToString()
        lHandlerID = v_lClaimHandlerId
        lProgressStatusID = v_lProgressStatusId
        sDescription = v_sDescription
        sReportedDate = v_dtDateReported.ToString()
        sReportedToDate = v_dtDateReported.ToString
        sLastModifiedDate = DateTime.Now.ToString()
        lClaimStatusID = v_lClaimStatusId
        sInsurerClaimNo = v_sInsurerClaimNo

        'Set the data

        oOpenClaim.SetProperties(gPMConstants.PMEComponentAction.PMEdit, sClaimNo.ToString, sPolicyNo.ToString, ToSafeInteger(lPolicyID), sDescription.ToString, lClaimStatusID.ToString, lProgressStatusID.ToString, lPrimaryCauseID.ToString, lSecondaryCauseID.ToString, ToSafeInteger(lCatastropheCodeID), sLossFromDate.ToString, sLossToDate.ToString, sReportedDate.ToString, sReportedToDate.ToString, sLastModifiedDate.ToString, ToSafeInteger(lHandlerID), ToSafeInteger(lCurrencyID), ToSafeInteger(nInfoOnly), ToSafeInteger(nLikelyClaim), sLocation.ToString, ToSafeInteger(lTown), ToSafeInteger(lRiskTypeID), sClientName.ToString, ToSafeInteger(sClientAddress), sClientTelNo.ToString, sClientFaxNo.ToString, sClientMobileNo.ToString, sClientEMail.ToString, sClientClaimNo.ToString, sInsurerName.ToString, sInsurerAddress.ToString, sInsurerTelNo.ToString, sInsurerFaxNo.ToString, sInsurerEmail.ToString, sInsurerClaimNo.ToString, sInsurerContact.ToString, ToSafeInteger(nVATRegistered), sVATRegisteredNo.ToString, sComments.ToString, sClaimsStatusDate.ToString, sClientShortName.ToString, sInsurerShortName.ToString, sClientTelNoOff.ToString, ToSafeInteger(v_lClaimId), ToSafeInteger(lUserDefFldA), ToSafeInteger(lUserDefFldB), ToSafeInteger(lUserDefFldC), ToSafeInteger(lUserDefFldD), ToSafeInteger(lUserDefFldE), ToSafeInteger(iSourceID), ToSafeInteger(iLanguageID), CType(vUnderwritingYearID, Object))

        '    g_oBusiness.SetProperties _
        ''                    g_nPMMode, g_sClaimNo, g_sPolicyNo, g_lPolicyID, g_sDescription, _
        ''                    g_lClaimStatusID, g_lProgressStatusID, g_lPrimaryCauseID, _
        ''                    g_lSecondaryCauseID, g_lCatastropheCodeID, g_sLossFromDate, _
        ''                    g_sLossToDate, g_sReportedDate, g_sReportedToDate, g_sLastModifiedDate, _
        ''                    g_lHandlerID, g_lCurrencyID, g_nInfoOnly, g_nLikelyClaim, g_sLocation, _
        ''                    g_lTown, g_lRiskTypeID, g_sClientName, g_sClientAddress, g_sClientTelNo, _
        ''                    g_sClientFaxNo, g_sClientMobileNo, g_sClientEMail, g_sClientClaimNo, _
        ''                    g_sInsurerName, g_sInsurerAddress, g_sInsurerTelNo, g_sInsurerFaxNo, _
        ''                    g_sInsurerEmail, g_sInsurerClaimNo, g_sInsurerContact, g_nVATRegistered, _
        ''                    g_sVATRegisteredNo, g_sComments, g_sClaimsStatusDate, g_sClientShortName, _
        ''                    g_sInsurerShortName, g_sClientTelNoOff, g_lClaimID, _
        ''                    g_lUserDefFldA, g_lUserDefFldB, g_lUserDefFldC, _
        ''                    g_lUserDefFldD, g_lUserDefFldE, g_iSourceID, g_iLanguageID, g_vUnderwritingYearID
        'Update the database

        m_lReturn = oOpenClaim.Update
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOpenClaim.Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimDetails")
            Return result
        End If


        oOpenClaim.Dispose()
        oOpenClaim = Nothing


        Return result

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
                If Not (m_oLookup Is Nothing) Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If

                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

End Class

