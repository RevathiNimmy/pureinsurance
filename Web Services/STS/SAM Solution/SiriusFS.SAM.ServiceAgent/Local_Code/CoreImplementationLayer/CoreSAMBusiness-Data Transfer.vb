Option Strict On

Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.ServiceAgent.SAMFunc
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports Microsoft.ApplicationBlocks.Data
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports dPMDAOBridge

Partial Public Class CoreSAMBusiness

    Private Const STR_DATATRANSFER As String = "DATATRANSFER"
    Private Const STR_WORK_CLAIM As String = "WORK_CLAIM"
    Private Const STR_DataModelCode As String = "DataModelCode"
    Private Const STR_OI As String = "OI"
    Private Const STR_US As String = "US"
    Private Const STR_RISK_OBJECTS As String = "RISK_OBJECTS"
    Private Const STR_GIS_POLICY_LINK_ID As String = "GIS_POLICY_LINK_ID"
    Private Const STR_POLICY_BINDER_ID As String = "_POLICY_BINDER_ID"
    Private Const STR_POLICY_BINDER As String = "_POLICY_BINDER"
    Private Const STR_DATA_SET As String = "DATA_SET"
    Private Const STR_CLAIM_PERIL_ID As String = "CLAIM_PERIL_ID"
    Private Const STR_CP As String = "CP"
    Private Const STR_CLAIM_ID As String = "CLAIM_ID"
    Private Const STR_POLICY_ID As String = "POLICY_ID"

    Public Function GetAccountBalance(ByVal GetAccountBalanceRequest As BaseGetAccountBalanceRequestType) As BaseGetAccountBalanceResponseType
        'Const ACMethodName As String = "GetAccountBalance"

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim oBusiness As New CoreBusiness
        Dim oResponse As BaseGetAccountBalanceResponseType = Nothing
        Dim nTypeOfPackage As enumTypeOfPackage

        Dim iSourceID As Integer
        'Dim iRet As Integer

        ' determine the type of package and thus the type of response
        If GetAccountBalanceRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.GetAccountBalanceRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.GetAccountBalanceResponseType
        ElseIf GetAccountBalanceRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetAccountBalanceRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetAccountBalanceResponseType
        End If

        ' validate the mandatory structure data
        GetAccountBalanceRequest.Validate(CType(oSAMErrorCollection, Object))

        ' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()

        ' validate the data provided in the structure

        iSourceID = 1
        ' Convert branch code to ID
        Try
            iSourceID = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", GetAccountBalanceRequest.BranchCode)
        Catch ex As Exception
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, SAMInvalidData.InvalidLookupListValue.ToString, "BranchCode", GetAccountBalanceRequest.BranchCode)
        End Try

        ' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()

        Dim ds As DataSet = Nothing

        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_ACT_Select_AccountBal")
                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.ToInt32(GetAccountBalanceRequest.PartyKey)
                ds = con.ExecuteDataSet(cmd, "Row")
            End Using

        End Using
        If GetAccountBalanceRequest.WCFSecurityToken = "" Then
            Dim oXMLDoc As New System.Xml.XmlDocument

            ds.DataSetName = "BaseGetAccountBalanceResponseTypeAccountBalance"

            oXMLDoc.LoadXml(ds.GetXml)

            oResponse.AccountBalance = oXMLDoc.DocumentElement
        End If

        oResponse.ResultData = ds

        Return oResponse
    End Function

    Public Overloads Function GetClaimRisk(ByVal GetClaimRiskRequest As BaseGetClaimRiskRequestType) As BaseGetClaimRiskResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)

            Dim oResponse As BaseGetClaimRiskResponseType

            oResponse = GetClaimRisk(con, GetClaimRiskRequest, True)

            Return oResponse

        End Using

    End Function

    Public Overloads Function GetClaimRisk(ByVal con As SiriusConnection, _
                                 ByVal GetClaimRiskRequest As BaseGetClaimRiskRequestType, _
                                 ByVal UseTimeStamp As Boolean) As BaseGetClaimRiskResponseType

        Const ACMethodName As String = "GetClaimRisk"

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim oBusiness As New CoreBusiness(_SiriusUser)
        Dim oResponse As BaseGetClaimRiskResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iSourceID As Integer
        Dim iRet As Integer

        ' determine the type of package and thus the type of response
        If GetClaimRiskRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.GetClaimRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.GetClaimRiskResponseType
        ElseIf GetClaimRiskRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetClaimRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetClaimRiskResponseType
        Else
            oResponse = New BaseImplementationTypes.BaseGetClaimRiskResponseType
        End If

        ' validate the mandatory structure data
        GetClaimRiskRequest.Validate(CType(oSAMErrorCollection, Object))

        ' Check the BaseClaimKey is valid
        If Not oBusiness.CheckClaimKey(con, GetClaimRiskRequest.BaseClaimKey) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, "Base Claim Key validation failed.", "ClaimKey", GetClaimRiskRequest.BaseClaimKey.ToString)
        End If

        ' Check the ClaimKey is valid
        If (GetClaimRiskRequest.ClaimKey > 0) AndAlso (Not oBusiness.CheckClaimKey(con, GetClaimRiskRequest.ClaimKey)) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, "Claim Key validation failed.", "ClaimKey", GetClaimRiskRequest.ClaimKey.ToString)
        End If

        ' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()

        iSourceID = 1
        ' Convert branch code to ID

        If GetClaimRiskRequest.BranchCode <> STR_DATATRANSFER Then

            'Try
            '    iSourceID = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", GetClaimRiskRequest.BranchCode)
            'Catch ex As Exception
            '    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, SAMInvalidData.InvalidLookupListValue.ToString, "BranchCode", GetClaimRiskRequest.BranchCode)
            'End Try
            If (Not String.IsNullOrEmpty(GetClaimRiskRequest.BranchCode)) Then
                iSourceID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
                PMLookupTable.Source, GetClaimRiskRequest.BranchCode, "BranchCode", oSAMErrorCollection)
            End If
        End If

        ' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()

        Try
            Dim iClaimId As Integer = GetClaimRiskRequest.ClaimKey
            Dim iScreenId As Integer
            Dim iRiskTypeId As Integer
            Dim sGisDataModelCode As String = String.Empty
            Dim iTransactionTypeId As Integer
            Dim sTransactionTypeCode As String = String.Empty
            Dim iInsuranceFileCnt As Integer

            If (GetClaimRiskRequest.Task = SAMComponentAction.PMAdd) Then

                If (UseTimeStamp = True) Then

                    ' check timestamp and lock
                    oBusiness.CheckSAMTSAndLock(con, GetClaimRiskRequest.BranchCode, _
                        CoreBusiness.LockName.ClaimId, _
                        GetClaimRiskRequest.BaseClaimKey, _
                        GetClaimRiskRequest.TimeStamp)
                End If

                ' We're adding a risk screen to this claim so update claim with the screen ids
                UpdateClaimGISScreen(con, GetClaimRiskRequest.BaseClaimKey)

            End If

            ' Retrieve the screen and risk type details for the claim.
            GetClaimRiskScreenDetails(con, GetClaimRiskRequest.BaseClaimKey, _
                                         iClaimId, _
                                         iScreenId, _
                                         iRiskTypeId, _
                                         sGisDataModelCode, _
                                         iTransactionTypeId, _
                                         sTransactionTypeCode, _
                                         iInsuranceFileCnt)

            Dim oDatabase As Object = Nothing
            Dim oRiskScreen As bSIRRiskScreen.Stateless = New bSIRRiskScreen.Stateless
            oRiskScreen = CType(oRiskScreen, bSIRRiskScreen.Stateless)
            'Rk modifies as part of SAM SFI Interop conversions by replacing .PMDAODatabase by .FromSirius.SqlConnection.Database for vDatabase parameter.
            'con = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            con = New SiriusConnectionPMDAO( _
                                                _SiriusUser.Username, _SiriusUser.SourceID, _
                                                _SiriusUser.LanguageID, _
                                                SiriusUserDefaults.AppName)
            'oDatabase = con.SqlConnection.Database

            ' initialise bSIRRiskScreen
            iRet = CInt(oRiskScreen.Initialise(sUsername:=_SiriusUser.Username, _
                                       sPassword:=_SiriusUser.Password, _
                                       iUserID:=_SiriusUser.UserID, _
                                       iSourceID:=_SiriusUser.SourceID, _
                                       iLanguageID:=_SiriusUser.LanguageID, _
                                       iCurrencyID:=_SiriusUser.CurrencyID, _
                                       iLogLevel:=CShort(SiriusUserDefaults.LogLevel), _
                                       sCallingAppName:=ACMethodName, _
                                       vDatabase:=oDatabase))

            If (iRet <> PMEReturnCode.PMTrue) Then

                ' if the call to Initialise fails then throw a business rule error
                oSAMErrorCollection = New SAMErrorCollection
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.FailedToInitialiseCOMComponent, _
                                                    SAMBusinessErrors.FailedToInitialiseCOMComponent.ToString, _
                                                    "bSIRRiskScreen.Stateless return the error code - " & iRet)
                oSAMErrorCollection.CheckForErrors()

            End If

            Dim oRiskScreenLoadRisk As New XMLTransRiskScreenLoadScreen
            Dim TheInput As New XMLTransRiskScreenLoadScreen.RiskScreenLoadRiskIn
            Dim TheOutput As XMLTransRiskScreenLoadScreen.RiskScreenLoadRiskOut

            With TheInput
                .iTask = CShort(GetClaimRiskRequest.Task) ' Cast.ToInt16(GetClaimRiskRequest.Task.ToObject, 0) ' PMAdd or PMEdit or PMView 
                .iSourceID = 0 ' Not needed
                .lNavigate = 0 ' Not needed
                .lProcessMode = 0 ' Not needed
                .dtEffectiveDate = Now
                .bSubScreen = False ' Not a sub screen
                .lScreenId = iScreenId ' NEED THE SCREEN ID PASSED IN
                .lRiskId = 0 ' Pretty sure not needed
                .lRiskTypeId = iRiskTypeId
                .sGisDataModelCode = sGisDataModelCode
                .lGISDataModelType = GISDMTypeClaim
                .lObjectType = GISOTRisk
                .sGISXMLDataset = "" ' not needed unless processing a subscreen
                .sMyOIKey = "" ' Not needed
                .sMyObjectName = "" ' Not needed
                .sParentOIKey = "" ' Not needed
                .sParentObjectName = "" ' Not needed
                .lPolicyLinkId = 0 ' Loaded by the GIS
                .lInsuranceFolderCnt = 0 ' Only used by Risks DMs
                .lInsuranceFileCnt = 0 ' Only used by Risks DMs
                .vScreenDetailsArray = Nothing ' Get's loaded by bSIRRiskScreen
                .vScreenValuesArray = Nothing ' Doesn't get used
                .vRiskDetailsArray = Nothing ' Only used by Risks DMs
                .vRiskTypeDetailsArray = Nothing ' Only used by Risks DMs
                .sTransactionType = sTransactionTypeCode ' The transaction type code of the claim, i.e. Open, Maintain etc.
                .lTransactionType = iTransactionTypeId ' The transaction type of the claim, i.e. Open, Maintain etc. (I don't know why we need both)
                .lProductId = 0 ' Only used by Risks DMs
                .lPartyCnt = 0 ' Only used by Party DMs 
                .lClaimID = iClaimId ' Proper claim id, not the base claim id
                .bCopyRisk = False
            End With

            Dim sInput As String = oRiskScreenLoadRisk.SerializeRiskScreenLoadRiskIn(TheInput)

            ' initialise bSIRRiskScreen
            Dim sOutput As String = oRiskScreen.RiskScreenLoadRisk(v_sInput:=sInput)

            TheOutput = oRiskScreenLoadRisk.DeserializeRiskScreenLoadRiskOut(sOutput)

            If (TheOutput.HasErrors = True) Then
                If TheOutput.HasBusinessRuleErrors Then
                    oSAMErrorCollection = New SAMErrorCollection
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                        TheOutput.Errors.BusinessRule.Detail.Description, _
                                                        TheOutput.Errors.BusinessRule.Detail.Detail.ToString)
                    oSAMErrorCollection.CheckForErrors()
                ElseIf TheOutput.HasBackOfficeErrors Then
                    oSAMErrorCollection = New SAMErrorCollection
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                        TheOutput.Errors.BackOffice.Detail.Description)
                    oSAMErrorCollection.CheckForErrors()
                ElseIf TheOutput.HasInternalExceptionErrors Then
                    oSAMErrorCollection = New SAMErrorCollection
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                        TheOutput.Errors.InternalException.Detail.Description)
                    oSAMErrorCollection.CheckForErrors()
                ElseIf TheOutput.HasInvalidDataErrors Then
                    oSAMErrorCollection = New SAMErrorCollection
                    If IsArray(TheOutput.Errors.InvalidData.Details) Then
                        For Cnt As Integer = TheOutput.Errors.InvalidData.Details.GetLowerBound(0) To TheOutput.Errors.InvalidData.Details.GetUpperBound(0)
                            With TheOutput.Errors.InvalidData.Details(Cnt)
                                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                                    .Description, "Invalid data found in " & .FieldName & ".  The supplied value was " & .SuppliedValue.ToString)
                            End With
                        Next Cnt
                    Else
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                            "bSIRRiskScreen.RiskScreenLoadRisk reported invalid data.")
                    End If
                    oSAMErrorCollection.CheckForErrors()
                End If
            End If

            With TheOutput
                oResponse.XMLDataSet = .sGISXMLDataset
            End With

            If (UseTimeStamp = True) Then

                If (GetClaimRiskRequest.Task = SAMComponentAction.PMAdd) Then

                    ' unlock and get the new timestamp
                    oBusiness.UnlockAndGetSAMTS(con, _
                        GetClaimRiskRequest.BranchCode, _
                        CoreBusiness.LockName.ClaimId, _
                        GetClaimRiskRequest.BaseClaimKey, _
                         oResponse.TimeStamp)

                Else

                    Dim bislocked As Boolean = False
                    oBusiness.GetTimestamp(GetClaimRiskRequest.BranchCode, _
                                       CoreBusiness.LockName.ClaimId, _
                                       GetClaimRiskRequest.BaseClaimKey, _
                                       oResponse.TimeStamp, _
                                       bislocked)

                End If

            End If

            If Not GetClaimRiskRequest.IsDataTransfer Then
                ' commit transaction
                'con.CommitTransaction()
            End If

        Catch

            If Not GetClaimRiskRequest.IsDataTransfer Then
                ' rollback transaction
                'con.RollbackTransaction()
            End If

            Throw

        End Try

        Return oResponse

    End Function

    Friend Overloads Sub SaveClaimRiskData(ByVal con As SiriusConnection, _
                                 ByVal CDTClaim As BaseCDTClaimType, _
                                 ByRef XMLDataSet As String)

        Dim GetClaimRiskRequest As New BaseImplementationTypes.BaseGetClaimRiskRequestType
        Dim GetClaimRiskResponse As BaseGetClaimRiskResponseType

        With GetClaimRiskRequest
            .BaseClaimKey = CDTClaim.SiriusBaseClaimKey
            .BranchCode = STR_DATATRANSFER
            .ClaimKey = CDTClaim.SiriusClaimKey
            .SourceId = 1
            .Task = SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction.PMAdd
            .IsDataTransfer = True

        End With

        GetClaimRiskResponse = GetClaimRisk(con, GetClaimRiskRequest, False)

        XMLDataSet = GetClaimRiskResponse.XMLDataSet

        Dim NewXmlDoc As New System.Xml.XmlDocument
        NewXmlDoc.LoadXml(CDTClaim.XMLDATASET)

        If NewXmlDoc.DocumentElement.Item(STR_RISK_OBJECTS) IsNot Nothing Then
            ProcessClaimRiskXMLDataset(con, CDTClaim, XMLDataSet)
        End If
        NewXmlDoc = Nothing
    End Sub

    Private Sub ProcessClaimRiskXMLDataset(ByVal con As SiriusConnection, _
                                           ByVal CDTClaim As BaseCDTClaimType, _
                                           ByRef NewXMLDataset As String)

        ' Declare the Core Business object
        Dim oBusiness As New CoreBusiness
        Dim SAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

        ' Transform the Dataset into its PB Format
        Dim sDataModelCode As String = String.Empty
        Dim lGisPolicyLinkId As Integer
        Dim lPolicyBinderId As Integer
        Dim sOI As String = ""
        Dim sWorkClaimOI As String = ""
        Dim sWorkClaimPerilOI As String = ""
        Dim oSaveToDBIn As New SaveToDBIn
        Dim oSaveToDBOut As New SaveToDBOut
        Dim OriginalXMLDataset As String = CDTClaim.XMLDATASET

        Dim xmlDoc As New System.Xml.XmlDocument
        'Try

        xmlDoc.LoadXml(NewXMLDataset)
        sDataModelCode = xmlDoc.SelectSingleNode(STR_DATA_SET).Attributes(STR_DataModelCode).Value.ToUpper

        Dim PolicyBinderNode As XmlNode = xmlDoc.SelectSingleNode(STR_DATA_SET).SelectSingleNode(STR_RISK_OBJECTS).SelectSingleNode(sDataModelCode & STR_POLICY_BINDER)

        lGisPolicyLinkId = Integer.Parse(PolicyBinderNode.Attributes(STR_GIS_POLICY_LINK_ID).Value)
        sOI = PolicyBinderNode.Attributes(STR_OI).Value
        lPolicyBinderId = Integer.Parse(PolicyBinderNode.Attributes(sDataModelCode & STR_POLICY_BINDER_ID).Value)

        Dim NewXmlDoc As New System.Xml.XmlDocument
        NewXmlDoc.LoadXml(OriginalXMLDataset)

        Dim PolicyBinderElement As XmlElement = NewXmlDoc.DocumentElement.Item(STR_RISK_OBJECTS).Item(sDataModelCode & STR_POLICY_BINDER)
        With PolicyBinderElement
            .SetAttribute(STR_OI, sOI)
            .SetAttribute(STR_US, "2")
            NewXmlDoc.DocumentElement.SetAttribute(STR_DataModelCode, sDataModelCode)
            .SetAttribute(STR_GIS_POLICY_LINK_ID, lGisPolicyLinkId.ToString)
            .SetAttribute(sDataModelCode & STR_POLICY_BINDER_ID, lPolicyBinderId.ToString)
        End With

        Dim DestWorkClaim As XmlElement = PolicyBinderElement.Item(STR_WORK_CLAIM)
        Dim SrcWorkClaim As XmlElement = xmlDoc.DocumentElement.Item(STR_RISK_OBJECTS).Item(sDataModelCode & STR_POLICY_BINDER).Item(STR_WORK_CLAIM)

        With DestWorkClaim
            sOI = "C" & CDTClaim.SiriusClaimKey.ToString
            .SetAttribute(STR_OI, sOI)
            .SetAttribute(STR_CLAIM_ID, CDTClaim.SiriusClaimKey.ToString)
            .SetAttribute(STR_US, "2")
            .SetAttribute("POLICY_ID", SrcWorkClaim.GetAttribute("POLICY_ID"))
            .SetAttribute("POLICY_NUMBER", SrcWorkClaim.GetAttribute("POLICY_NUMBER"))
            .SetAttribute("CLAIM_NUMBER", SrcWorkClaim.GetAttribute("CLAIM_NUMBER"))
            .SetAttribute("DESCRIPTION", SrcWorkClaim.GetAttribute("DESCRIPTION"))
            .SetAttribute("CLAIM_STATUS_ID", SrcWorkClaim.GetAttribute("CLAIM_STATUS_ID"))
            .SetAttribute("PROGRESS_STATUS_ID", SrcWorkClaim.GetAttribute("PROGRESS_STATUS_ID"))
            .SetAttribute("PRIMARY_CAUSE_ID", SrcWorkClaim.GetAttribute("PRIMARY_CAUSE_ID"))
            .SetAttribute("SECONDARY_CAUSE_ID", SrcWorkClaim.GetAttribute("SECONDARY_CAUSE_ID"))
            .SetAttribute("CATASTROPHE_CODE_ID", SrcWorkClaim.GetAttribute("CATASTROPHE_CODE_ID"))
            .SetAttribute("COINSURANCE_TREATMENT_ID", SrcWorkClaim.GetAttribute("COINSURANCE_TREATMENT_ID"))
            .SetAttribute("LOSS_FROM_DATE", SrcWorkClaim.GetAttribute("LOSS_FROM_DATE"))
            .SetAttribute("LOSS_TO_DATE", SrcWorkClaim.GetAttribute("LOSS_TO_DATE"))
            .SetAttribute("REPORTED_DATE", SrcWorkClaim.GetAttribute("REPORTED_DATE"))
            .SetAttribute("REPORTED_TO_DATE", SrcWorkClaim.GetAttribute("REPORTED_TO_DATE"))
            .SetAttribute("LAST_MODIFIED_DATE", SrcWorkClaim.GetAttribute("LAST_MODIFIED_DATE"))
            .SetAttribute("HANDLER_ID", SrcWorkClaim.GetAttribute("HANDLER_ID"))
            .SetAttribute("CURRENCY_ID", SrcWorkClaim.GetAttribute("CURRENCY_ID"))
            .SetAttribute("INFO_ONLY", SrcWorkClaim.GetAttribute("INFO_ONLY"))
            .SetAttribute("LIKELY_CLAIM", SrcWorkClaim.GetAttribute("LIKELY_CLAIM"))
            .SetAttribute("LOCATION", SrcWorkClaim.GetAttribute("LOCATION"))
            .SetAttribute("TOWN", SrcWorkClaim.GetAttribute("TOWN"))
            .SetAttribute("RISK_TYPE_ID", SrcWorkClaim.GetAttribute("RISK_TYPE_ID"))
            .SetAttribute("CLIENT_NAME", SrcWorkClaim.GetAttribute("CLIENT_NAME"))
            .SetAttribute("CLIENT_ADDRESS", SrcWorkClaim.GetAttribute("CLIENT_ADDRESS"))
            .SetAttribute("CLIENT_TEL_NO", SrcWorkClaim.GetAttribute("CLIENT_TEL_NO"))
            .SetAttribute("CLIENT_FAX_NO", SrcWorkClaim.GetAttribute("CLIENT_FAX_NO"))
            .SetAttribute("CLIENT_MOBILE_NO", SrcWorkClaim.GetAttribute("CLIENT_MOBILE_NO"))
            .SetAttribute("CLIENT_EMAIL", SrcWorkClaim.GetAttribute("CLIENT_EMAIL"))
            .SetAttribute("CLIENT_CLAIM_NUMBER", SrcWorkClaim.GetAttribute("CLIENT_CLAIM_NUMBER"))
            .SetAttribute("INSURER_NAME", SrcWorkClaim.GetAttribute("INSURER_NAME"))
            .SetAttribute("INSURER_ADDRESS", SrcWorkClaim.GetAttribute("INSURER_ADDRESS"))
            .SetAttribute("INSURER_TEL_NO", SrcWorkClaim.GetAttribute("INSURER_TEL_NO"))
            .SetAttribute("INSURER_FAX_NO", SrcWorkClaim.GetAttribute("INSURER_FAX_NO"))
            .SetAttribute("INSURER_EMAIL", SrcWorkClaim.GetAttribute("INSURER_EMAIL"))
            .SetAttribute("INSURER_CLAIM_NUMBER", SrcWorkClaim.GetAttribute("INSURER_CLAIM_NUMBER"))
            .SetAttribute("INSURER_CONTACT", SrcWorkClaim.GetAttribute("INSURER_CONTACT"))
            .SetAttribute("VAT_REGISTERED", SrcWorkClaim.GetAttribute("VAT_REGISTERED"))
            .SetAttribute("VAT_REG_NO", SrcWorkClaim.GetAttribute("VAT_REG_NO"))
            .SetAttribute("COMMENTS", SrcWorkClaim.GetAttribute("COMMENTS"))
            .SetAttribute("CLAIMS_STATUS_DATE", SrcWorkClaim.GetAttribute("CLAIMS_STATUS_DATE"))
            .SetAttribute("CLIENT_SHORT_NAME", SrcWorkClaim.GetAttribute("CLIENT_SHORT_NAME"))
            .SetAttribute("INSURER_SHORT_NAME", SrcWorkClaim.GetAttribute("INSURER_SHORT_NAME"))
            .SetAttribute("CLIENT_TEL_NO_OFF", SrcWorkClaim.GetAttribute("CLIENT_TEL_NO_OFF"))
            .SetAttribute("USER_DEFINED_FIELD_A", SrcWorkClaim.GetAttribute("USER_DEFINED_FIELD_A"))
            .SetAttribute("USER_DEFINED_FIELD_B", SrcWorkClaim.GetAttribute("USER_DEFINED_FIELD_B"))
            .SetAttribute("USER_DEFINED_FIELD_C", SrcWorkClaim.GetAttribute("USER_DEFINED_FIELD_C"))
            .SetAttribute("USER_DEFINED_FIELD_D", SrcWorkClaim.GetAttribute("USER_DEFINED_FIELD_D"))
            .SetAttribute("USER_DEFINED_FIELD_E", SrcWorkClaim.GetAttribute("USER_DEFINED_FIELD_E"))
            .SetAttribute("CLIENT_ID", SrcWorkClaim.GetAttribute("CLIENT_ID"))
            .SetAttribute("CLAIM_FOLDER_ID", SrcWorkClaim.GetAttribute("CLAIM_FOLDER_ID"))
            .SetAttribute("CLAIM_VERSION_NUMBER", SrcWorkClaim.GetAttribute("CLAIM_VERSION_NUMBER"))
            .SetAttribute("CLAIM_VERSION_STATUS_ID", SrcWorkClaim.GetAttribute("CLAIM_VERSION_STATUS_ID"))
            .SetAttribute("CREATE_DATE", SrcWorkClaim.GetAttribute("CREATE_DATE"))
            .SetAttribute("CREATED_BY_ID", SrcWorkClaim.GetAttribute("CREATED_BY_ID"))
            .SetAttribute("MODIFIED_BY_ID", SrcWorkClaim.GetAttribute("MODIFIED_BY_ID"))
            .SetAttribute("ACCEPTANCE_STATUS_ID", SrcWorkClaim.GetAttribute("ACCEPTANCE_STATUS_ID"))
            .SetAttribute("ORIGINAL_CLAIM_ID", SrcWorkClaim.GetAttribute("ORIGINAL_CLAIM_ID"))
            .SetAttribute("REALLOW_NCD", SrcWorkClaim.GetAttribute("REALLOW_NCD"))
            .SetAttribute("NCD_STATUS_CHANGED", SrcWorkClaim.GetAttribute("NCD_STATUS_CHANGED"))
            .SetAttribute("GIS_SCREEN_ID", SrcWorkClaim.GetAttribute("GIS_SCREEN_ID"))

        End With

        Dim PerilsProcessed As Integer = 0

        For Each ClaimPeril As BaseCDTClaimPerilType In CDTClaim.ClaimPeril

            Dim DestWorkClaimPeril As XmlElement = CType(DestWorkClaim.SelectSingleNode("WORK_CLAIM_PERIL[@OI=""CP" & ClaimPeril.SAMStagingClaimPerilKey.ToString & """]"), XmlElement)
            Dim SrcWorkClaimPeril As XmlElement = CType(SrcWorkClaim.SelectSingleNode("WORK_CLAIM_PERIL[@OI=""CP" & ClaimPeril.SiriusClaimPerilKey.ToString & """]"), XmlElement)
            If DestWorkClaimPeril IsNot Nothing Then
                PerilsProcessed += 1
                DestWorkClaimPeril.SetAttribute(STR_OI, SrcWorkClaimPeril.GetAttribute(STR_OI))
                DestWorkClaimPeril.SetAttribute(STR_CLAIM_PERIL_ID, SrcWorkClaimPeril.GetAttribute(STR_CLAIM_PERIL_ID))
                DestWorkClaimPeril.SetAttribute(STR_CLAIM_ID, SrcWorkClaimPeril.GetAttribute(STR_CLAIM_ID))
                DestWorkClaimPeril.SetAttribute(STR_US, "2")
                DestWorkClaimPeril.SetAttribute("PERIL_TYPE_ID", SrcWorkClaimPeril.GetAttribute("PERIL_TYPE_ID"))
                DestWorkClaimPeril.SetAttribute("DESCRIPTION", SrcWorkClaimPeril.GetAttribute("DESCRIPTION"))
                DestWorkClaimPeril.SetAttribute("COMMENTS", SrcWorkClaimPeril.GetAttribute("COMMENTS"))
                DestWorkClaimPeril.SetAttribute("SUM_INSURED", SrcWorkClaimPeril.GetAttribute("SUM_INSURED"))
                DestWorkClaimPeril.SetAttribute("RI_BAND", SrcWorkClaimPeril.GetAttribute("RI_BAND"))
                DestWorkClaimPeril.SetAttribute("RESERVE_DETAILS", SrcWorkClaimPeril.GetAttribute("RESERVE_DETAILS"))
                DestWorkClaimPeril.SetAttribute("PAYMENT_DETAILS", SrcWorkClaimPeril.GetAttribute("PAYMENT_DETAILS"))
                DestWorkClaimPeril.SetAttribute("GIS_SCREEN_ID", SrcWorkClaimPeril.GetAttribute("GIS_SCREEN_ID"))
            End If

        Next ClaimPeril

        'If PerilsProcessed <> DestWorkClaim.SelectNodes("WORK_CLAIM_PERIL").Count Then
        '    SAMErrorCollection.AddBusinessRule(SAMBusinessErrors.WorkClaimPerilsInXMLDatasetDoNotMatchPerilsInCDTClaimPeril, _
        '                                        "There are work claim perils in the XML dataset which did not correspond to perils in the CDTClaimPeril.  SAMStaging Claim Key = " & CDTClaim.SAMStagingClaimKey.ToString)
        '    SAMErrorCollection.CheckForErrors()

        NewXMLDataset = SAMFunc.TransformDatasetSAMtoPB(con, NewXmlDoc.OuterXml)

        With oSaveToDBIn
            .DataModelCode = sDataModelCode
            .BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
            .XMLDataset = NewXMLDataset
        End With

        ' Save the new dataset to the DB
        oSaveToDBOut = oBusiness.SaveToDB(con, oSaveToDBIn)

        If oSaveToDBOut.XMLDataset = "" Then
            SAMErrorCollection.AddFatal("SaveToDB failed to return a valid XMLDatset in the method ProcessClaimRiskXMLDataset")
            SAMErrorCollection.CheckForErrors()
        End If

        NewXMLDataset = oSaveToDBOut.XMLDataset

    End Sub

    Public Function UpdateClaimRisk(ByVal UpdateClaimRiskRequest As BaseUpdateClaimRiskRequestType) As BaseUpdateClaimRiskResponseType
        Const ACMethodName As String = "UpdateClaimRisk"

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim oBusiness As New CoreBusiness(_SiriusUser)
        Dim oResponse As BaseUpdateClaimRiskResponseType = Nothing
        Dim nTypeOfPackage As enumTypeOfPackage

        Dim iSourceID As Integer

        ' determine the type of package and thus the type of response
        If UpdateClaimRiskRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.UpdateClaimRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.UpdateClaimRiskResponseType
        ElseIf UpdateClaimRiskRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateClaimRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.UpdateClaimRiskResponseType
        End If

        ' validate the mandatory structure data
        UpdateClaimRiskRequest.Validate(CType(oSAMErrorCollection, Object))

        ' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()

        ' validate the data provided in the structure

        iSourceID = 1
        ' Convert branch code to ID

        'Try
        '    iSourceID = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", UpdateClaimRiskRequest.BranchCode)
        'Catch ex As Exception
        '    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, SAMInvalidData.InvalidLookupListValue.ToString, "BranchCode", UpdateClaimRiskRequest.BranchCode)
        'End Try
        If (Not String.IsNullOrEmpty(UpdateClaimRiskRequest.BranchCode)) Then
            iSourceID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
            PMLookupTable.Source, UpdateClaimRiskRequest.BranchCode, "BranchCode", oSAMErrorCollection)
        End If
        Try
            Dim xmlDocTest As New System.Xml.XmlDocument
            xmlDocTest.LoadXml(UpdateClaimRiskRequest.XMLDataSet)
        Catch ex As Exception
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, "XmlDataSet is not in valid format.", "XmlDataSet", UpdateClaimRiskRequest.XMLDataSet)
        End Try

        ' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()

        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            ' Check the ClaimKey is valid
            If Not oBusiness.CheckClaimKey(con, UpdateClaimRiskRequest.BaseClaimKey) Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, "Claim Key validation failed.", "ClaimKey", UpdateClaimRiskRequest.BaseClaimKey.ToString)
            End If

            ' begin transaction
            con.BeginTransaction()

            Try

                oBusiness.CheckSAMTSAndLock(con, UpdateClaimRiskRequest.BranchCode, _
                                            CoreBusiness.LockName.ClaimId, UpdateClaimRiskRequest.BaseClaimKey, _
                                            UpdateClaimRiskRequest.TimeStamp)

                Dim iClaimId As Integer
                Dim iScreenId As Integer
                Dim iRiskTypeId As Integer
                Dim sGisDataModelCode As String = String.Empty
                Dim iTransactionTypeId As Integer
                Dim sTransactionTypeCode As String = String.Empty
                Dim iInsuranceFileCnt As Integer
                Dim iRet As Integer

                ' get claim key from base claim key

                ' Retrieve the screen and risk type details for the claim.
                GetClaimRiskScreenDetails(con, UpdateClaimRiskRequest.BaseClaimKey, _
                                             iClaimId, _
                                             iScreenId, _
                                             iRiskTypeId, _
                                             sGisDataModelCode, _
                                             iTransactionTypeId, _
                                             sTransactionTypeCode, _
                                             iInsuranceFileCnt)

                Dim oDatabase As Object = Nothing
                Dim oRiskScreen As bSIRRiskScreen.Stateless = New bSIRRiskScreen.Stateless
                oRiskScreen = CType(oRiskScreen, bSIRRiskScreen.Stateless)
                'Following line is being added by rk as part of SAM Interop conversions and as per internal and SSP discussion in place of LOC next to it (now commented somewhere below saying oDatabase = con.PMDAODatabase)
                'oDatabase = con.SqlConnection.Database
                'oDatabase = con.PMDAODatabase

                ' initialise bSIRRiskScreen
                iRet = CInt(oRiskScreen.Initialise(sUsername:=_SiriusUser.Username, _
                                           sPassword:=_SiriusUser.Password, _
                                           iUserID:=_SiriusUser.UserID, _
                                           iSourceID:=_SiriusUser.SourceID, _
                                           iLanguageID:=_SiriusUser.LanguageID, _
                                           iCurrencyID:=_SiriusUser.CurrencyID, _
                                           iLogLevel:=CShort(SiriusUserDefaults.LogLevel), _
                                           sCallingAppName:=ACMethodName, _
                                           vDatabase:=oDatabase))

                If (iRet <> PMEReturnCode.PMTrue) Then
                    ' if the call to Initialise fails then throw a business rule error
                    oSAMErrorCollection = New SAMErrorCollection
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.FailedToInitialiseCOMComponent, _
                                                        SAMBusinessErrors.FailedToInitialiseCOMComponent.ToString, _
                                                        "bSIRRiskScreen.Stateless return the error code - " & iRet)

                    oSAMErrorCollection.CheckForErrors()

                End If

                Dim oRiskScreenOKClick As New XMLTransRiskScreenOKClick
                Dim TheInput As New XMLTransRiskScreenOKClick.RiskScreenOKClickIn
                Dim TheOutput As XMLTransRiskScreenOKClick.RiskScreenOKClickOut

                'Populate the XML for the call to the stateless object
                With TheInput

                    .iTask = CShort(SAMComponentAction.PMAdd)
                    .iSourceID = 0
                    .lNavigate = 0
                    .lProcessMode = 0
                    .sTransactionType = sTransactionTypeCode ' From Claim Table
                    .lTransactionType = iTransactionTypeId ' From Claim Table
                    .dtEffectiveDate = Now
                    .bSubScreen = False
                    .lScreenId = iScreenId ' From Claim Table
                    .lRiskId = 0
                    .lRiskTypeId = iRiskTypeId ' Need to get this
                    .sGisDataModelCode = sGisDataModelCode ' Claims Datamodel code - Need this
                    .lGISDataModelType = GISDMTypeClaim ' Claims
                    .lObjectType = 0
                    .sGISXMLDataset = UpdateClaimRiskRequest.XMLDataSet
                    .sMyOIKey = "" ' This is passed into bGIS.NBQuoteStatefull but it's never used.
                    .sMyObjectName = "" ' Only used for disclosures and standard wordings
                    .sParentOIKey = "" ' Only used for disclosures and standard wordings
                    .sParentObjectName = "" ' Only used for disclosures and standard wordings
                    .lPolicyLinkId = 0 ' Only used for standard wordings
                    .lInsuranceFileCnt = iInsuranceFileCnt ' Need this
                    .bPostQuote = False
                    .dtCoverStartDate = Now
                    ReDim .vScreenDetailsArray(34, 0) ' Needs to appear as an array but content is only needed for StandardWordings

                End With

                Dim sInput As String = oRiskScreenOKClick.SerializeRiskScreenOKClickIn(TheInput)

                ' initialise bSIRRiskScreen
                Dim sOutput As String = oRiskScreen.RiskScreenOKClick(v_sInput:=sInput)

                TheOutput = oRiskScreenOKClick.DeserializeRiskScreenOKClickOut(sOutput)

                If (TheOutput.HasErrors = True) Then

                    If TheOutput.HasBusinessRuleErrors Then
                        oSAMErrorCollection = New SAMErrorCollection
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                            TheOutput.Errors.BusinessRule.Detail.Description, _
                                                            TheOutput.Errors.BusinessRule.Detail.Detail.ToString)
                        oSAMErrorCollection.CheckForErrors()
                    ElseIf TheOutput.HasBackOfficeErrors Then
                        oSAMErrorCollection = New SAMErrorCollection
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                            TheOutput.Errors.BackOffice.Detail.Description)
                        oSAMErrorCollection.CheckForErrors()
                    ElseIf TheOutput.HasInternalExceptionErrors Then
                        oSAMErrorCollection = New SAMErrorCollection
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                            TheOutput.Errors.InternalException.Detail.Description)
                        oSAMErrorCollection.CheckForErrors()
                    ElseIf TheOutput.HasInvalidDataErrors Then
                        'oSAMErrorCollection = New SAMErrorCollection
                        'oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                        '                                     TheOutput.Errors.InvalidData.Details(0)..Description)                           
                        'oSAMErrorCollection.CheckForErrors()
                    End If

                End If

                With TheOutput
                    'oResponse.XMLDataSet = .sGISXMLDataset
                End With

                oBusiness.UnlockAndGetSAMTS(con, UpdateClaimRiskRequest.BranchCode, _
                                            CoreBusiness.LockName.ClaimId, UpdateClaimRiskRequest.BaseClaimKey, _
                                            oResponse.TimeStamp)

                ' commit transaction
                con.CommitTransaction()

            Catch

                ' rollback transaction
                con.RollbackTransaction()

                Throw

            End Try

        End Using

        Return oResponse

    End Function

    ' ***************************************************************** '
    ' Name: ClientDataImport
    '
    ' Description: This method is the implementation of the ClientDataImport 
    '              Web Method on the Messaging service
    '
    ' ***************************************************************** '
    Public Function ClientDataImport(ByVal ClientDataImportRequest As BaseClientDataImportRequestType) As BaseClientDataImportResponseType

        'Const ACMethodName As String = "ClientDataImport"

        ' Declare the Response object
        Dim oResponse As New BaseImplementationTypes.BaseClientDataImportResponseType
        Dim nTypeOfPackage As enumTypeOfPackage

        ' Declare the Core SAM business object
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)

        Dim oAddPartyResponse As New BaseAddPartyResponseType

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

        Dim PolicyStatusID As Integer = 0

        ' determine the type of package and thus the type of response
        If ClientDataImportRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.ClientDataImportRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.ClientDataImportResponseType
        ElseIf ClientDataImportRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ClientDataImportRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.ClientDataImportResponseType
        End If

        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Try

                ' validate the mandatory structure data
                ClientDataImportRequest.Validate(CType(oSAMErrorCollection, Object))

                ' if there were any errors throw an exception
                oSAMErrorCollection.CheckForErrors()

                Dim bPartyKeyPresent As Boolean = False
                Dim lPartyKey As Integer
                If (ClientDataImportRequest.SiriusPartyKeySpecified) Then
                    ' Checking whether the party key is valid
                    lPartyKey = GetAndValidateSpecifiedTableCode(con, "Party", "Party_Cnt", "Party_Cnt", ClientDataImportRequest.SiriusPartyKey.ToString, oSAMErrorCollection, "PartyKey")
                    oSAMErrorCollection.CheckForErrors()
                    oResponse.PartyKey = lPartyKey
                    bPartyKeyPresent = True
                Else
                    lPartyKey = GetPartyKey(ClientDataImportRequest.Party)
                    If (lPartyKey <> 0) Then
                        oResponse.PartyKey = lPartyKey
                        bPartyKeyPresent = True
                    End If
                End If
                If bPartyKeyPresent Then
                    Dim oUpdatePartyRequest As New BaseImplementationTypes.BaseUpdatePartyRequestType
                    oUpdatePartyRequest.BranchCode = ClientDataImportRequest.BranchCode
                    oUpdatePartyRequest.Party = ClientDataImportRequest.Party
                    oUpdatePartyRequest.PartyKey = oResponse.PartyKey
                    oAddPartyResponse.PartyKey = oResponse.PartyKey

                    Dim oUpdatePartyResponse As New BaseUpdatePartyResponseType

                    ' Get the Timestamp
                    Dim bIsLocked As Boolean
                    Dim AnyError As STSErrorType
                    AnyError = oCoreBusiness.GetTimestamp( _
                                        oUpdatePartyRequest.BranchCode, _
                                        CoreBusiness.LockName.PartyCnt, _
                                        oUpdatePartyRequest.PartyKey, _
                                        oUpdatePartyRequest.PartyTimestamp, _
                                        bIsLocked)
                    ' Return AnyErrors 
                    If AnyError Is Nothing = False Then
                        oUpdatePartyResponse.STSError = AnyError
                    End If

                    'Update the Party Record
                    Try
                        con.BeginTransaction()
                        oUpdatePartyResponse = UpdateParty(con, oUpdatePartyRequest)
                        con.CommitTransaction()
                    Catch
                        con.RollbackTransaction()
                        Throw
                    End Try
                    ' Check the response for errors
                    SAMErrorCollection.CheckForErrorsFromSTS(oUpdatePartyResponse.STSError)

                    ' Determine the Party Type
                    If ClientDataImportRequest.Party.GetType Is GetType(BasePartyPCType) Then
                        ' Process personal client
                        Dim oBasePartyPC As BasePartyPCType = DirectCast(ClientDataImportRequest.Party, BasePartyPCType)
                        oAddPartyResponse.ResolvedName = oBasePartyPC.Title & " " & oBasePartyPC.Initials & " " & oBasePartyPC.Surname
                    Else
                        ' Process corporate client
                        Dim oBasePartyCC As BasePartyCCType = DirectCast(ClientDataImportRequest.Party, BasePartyCCType)
                        oAddPartyResponse.ResolvedName = oBasePartyCC.CompanyName
                    End If

                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_ShortName")
                        cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = oAddPartyResponse.PartyKey
                        oAddPartyResponse.Shortname = Cast.ToString(con.ExecuteScalar(cmd), String.Empty)
                    End Using

                Else
                    Dim oAddPartyRequest As New BaseImplementationTypes.BaseAddPartyRequestType
                    ' Add the Party record by calling the implementation method 
                    oAddPartyRequest.BranchCode = ClientDataImportRequest.BranchCode
                    oAddPartyRequest.Party = ClientDataImportRequest.Party
                    oAddPartyRequest.AgentKey = ClientDataImportRequest.AgentKey

                    'Add the Party Record
                    Try
                        con.BeginTransaction()
                        oAddPartyResponse = AddParty(con, AddPartyRequest:=oAddPartyRequest)
                        con.CommitTransaction()
                    Catch
                        con.RollbackTransaction()
                        Throw
                    End Try

                    ' Check the response for errors
                    SAMErrorCollection.CheckForErrorsFromSTS(oAddPartyResponse.STSError)

                    oResponse.PartyKey = oAddPartyResponse.PartyKey
                End If

            Catch

                Throw

            End Try

        End Using

        Return oResponse

    End Function

    Private Function ToImplementationBasePolicyDataImportResponseTypeRisks(ByVal ConvertFrom As BaseRiskResultType) As BasePolicyDataImportResponseTypeRisks

        Dim ConvertTo As BasePolicyDataImportResponseTypeRisks = Nothing

        If ConvertFrom IsNot Nothing Then

            ConvertTo = New BasePolicyDataImportResponseTypeRisks

            ConvertTo.RiskFolderKey = ConvertFrom.RiskFolderID
            ConvertTo.RiskKey = ConvertFrom.RiskID
            ConvertTo.SAMStagingRiskKey = ConvertFrom.SAMStagingRiskKey

        End If

        Return ConvertTo

    End Function

    ''' <summary>
    ''' Checks whether the party information is already present in the database. If it is present, party key is returned.
    ''' </summary>
    ''' <param name="oBaseParty">Object of BasePartyType class</param>
    ''' <returns >Returns the party key if details about party is present in database. Else, returns zero.</returns>
    ''' <remarks></remarks>
    Private Function GetPartyKey(ByVal oBaseParty As BasePartyType) As Integer
        Dim nReturn As Integer = 0
        ' Determine the Party Type
        If oBaseParty.GetType Is GetType(BasePartyPCType) Then
            ' Check whether personal client exists in the Sirius database
            Dim oBasePartyPC As BasePartyPCType = DirectCast(oBaseParty, BasePartyPCType)
            Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                           _SiriusUser.Username, _SiriusUser.SourceID, _
                                           _SiriusUser.LanguageID, _
                                           SiriusUserDefaults.AppName)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Check_For_Existing_Personal_Party")
                    cmd.AddInParameter("@Last_Name", SqlDbType.VarChar, 255).Value = oBasePartyPC.Surname
                    cmd.AddInParameter("@First_Name", SqlDbType.VarChar, 60).Value = oBasePartyPC.Forename
                    If (oBasePartyPC.Addresses IsNot Nothing) Then
                        cmd.AddInParameter("@Address_Line_1", SqlDbType.VarChar, 60).Value = oBasePartyPC.Addresses(0).AddressLine1
                        cmd.AddInParameter("@PostCode", SqlDbType.VarChar, 20).Value = oBasePartyPC.Addresses(0).PostCode
                    Else
                        cmd.AddInParameter("@Address_Line_1", SqlDbType.VarChar, 60).Value = Nothing
                        cmd.AddInParameter("@PostCode", SqlDbType.VarChar, 20).Value = Nothing
                    End If

                    cmd.AddInParameter("@Date_of_Birth", SqlDbType.DateTime).Value = oBasePartyPC.DateOfBirth

                    nReturn = Cast.ToInt32(con.ExecuteScalar(cmd), 0)
                End Using
            End Using
        Else
            ' Process corporate client
            Dim oBasePartyCC As BasePartyCCType = DirectCast(oBaseParty, BasePartyCCType)
            Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                           _SiriusUser.Username, _SiriusUser.SourceID, _
                                           _SiriusUser.LanguageID, _
                                           SiriusUserDefaults.AppName)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Check_For_Existing_Corporate_Party")
                    cmd.AddInParameter("@Trading_Name", SqlDbType.VarChar, 255).Value = oBasePartyCC.CompanyName
                    If (oBasePartyCC.Addresses IsNot Nothing) Then
                        cmd.AddInParameter("@Address_Line_1", SqlDbType.VarChar, 60).Value = oBasePartyCC.Addresses(0).AddressLine1
                        cmd.AddInParameter("@PostCode", SqlDbType.VarChar, 20).Value = oBasePartyCC.Addresses(0).PostCode
                    Else
                        cmd.AddInParameter("@Address_Line_1", SqlDbType.VarChar, 60).Value = Nothing
                        cmd.AddInParameter("@PostCode", SqlDbType.VarChar, 20).Value = Nothing
                    End If
                    nReturn = Cast.ToInt32(con.ExecuteScalar(cmd), 0)
                End Using
            End Using
        End If
        Return nReturn
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Con"></param>
    ''' <param name=""></param>
    ''' <remarks></remarks>
    Private Sub GetClaimRiskScreenDetails(ByVal con As SiriusConnection, _
                                    ByVal iBaseClaimId As Integer, _
                                    ByRef r_iClaimId As Integer, _
                                    ByRef r_iScreenId As Integer, _
                                    ByRef r_iRiskTypeId As Integer, _
                                    ByRef r_sGisDataModelCode As String, _
                                    ByRef r_iTransactionTypeId As Integer, _
                                    ByRef r_sTransactionTypeCode As String, _
                                    ByRef r_iInsuranceFileCnt As Integer)

        Dim dsClaimRiskScreen As DataSet = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CLM_Get_Claim_Risk_Screen_Details")
            cmd.AddInParameter("@BaseClaim_id", SqlDbType.Int).Value = Cast.ToInt32(iBaseClaimId)
            cmd.AddInParameter("@Claim_id", SqlDbType.Int).Value = Cast.ToInt32(r_iClaimId)
            dsClaimRiskScreen = con.ExecuteDataSet(cmd, "ClaimRiskScreen")
            If dsClaimRiskScreen IsNot Nothing AndAlso dsClaimRiskScreen.Tables.Count > 0 AndAlso dsClaimRiskScreen.Tables("ClaimRiskScreen").Rows.Count > 0 Then
                r_iClaimId = Cast.ToInt32(dsClaimRiskScreen.Tables(0).Rows(0).Item("claim_id"), 0)
                r_iScreenId = Cast.ToInt32(dsClaimRiskScreen.Tables(0).Rows(0).Item("gis_screen_id"), 0)
                r_iRiskTypeId = Cast.ToInt32(dsClaimRiskScreen.Tables(0).Rows(0).Item("risk_type_id"), 0)
                r_sGisDataModelCode = Cast.ToString(dsClaimRiskScreen.Tables(0).Rows(0).Item("gis_data_model_code"))
                r_iTransactionTypeId = Cast.ToInt32(dsClaimRiskScreen.Tables(0).Rows(0).Item("transaction_type_id"), 0)
                r_sTransactionTypeCode = Cast.ToString(dsClaimRiskScreen.Tables(0).Rows(0).Item("transaction_type_code"))
                r_iInsuranceFileCnt = Cast.ToInt32(dsClaimRiskScreen.Tables(0).Rows(0).Item("insurance_file_cnt"), 0)
            End If
        End Using

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Con"></param>
    ''' <param name=""></param>
    ''' <remarks></remarks>
    Private Sub UpdateClaimGISScreen(ByVal con As SiriusConnection, _
                                    ByVal lBaseClaimId As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CLM_Update_Claim_GIS_Screen")
            cmd.AddInParameter("@BaseClaim_id", SqlDbType.Int).Value = lBaseClaimId

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Con"></param>
    ''' <param name=""></param>
    ''' <remarks></remarks>
    Private Sub UpdateInsuranceFileDetails(ByVal con As SiriusConnection, _
                                                ByVal iInsuranceFileKey As Integer, _
                                                Optional ByVal sInsuranceFileTypeCode As String = "")

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Update_Insurance_File_ThisPremium")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = iInsuranceFileKey
            cmd.AddInParameter("@insurance_file_type_code", SqlDbType.VarChar, 20).Value = sInsuranceFileTypeCode
            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Con"></param>
    ''' <param name=""></param>
    ''' <remarks></remarks>
    Private Sub GetPFRFID(ByVal con As SiriusConnection, _
                                    ByVal v_iCompanyNo As Integer, _
                                    ByVal v_iSchemeNo As Integer, _
                                    ByVal v_iSchemeVersion As Integer, _
                                    ByRef r_iPFRFID As Integer)

        Dim dsInsuranceFolderDetails As DataSet = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_PFRF_ID")
            cmd.AddInParameter("@CompanyNo", SqlDbType.Int).Value = v_iCompanyNo
            cmd.AddInParameter("@SchemeNo", SqlDbType.Int).Value = v_iSchemeNo
            cmd.AddInParameter("@SchemeVersion", SqlDbType.Int).Value = v_iSchemeVersion

            dsInsuranceFolderDetails = con.ExecuteDataSet(cmd, "GetPFRFID")

            r_iPFRFID = Cast.ToInt32(dsInsuranceFolderDetails.Tables(0).Rows(0).Item("pfrf_id"), 0)

        End Using

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Con"></param>
    ''' <param name=""></param>
    ''' <remarks></remarks>
    Private Sub GetInsuranceFolderAndParty(ByVal con As SiriusConnection, _
                                    ByVal sInsuranceFileRef As String, _
                                    ByRef r_iInsuranceFolderKey As Integer, _
                                    ByRef r_iPartyKey As Integer)

        Dim dsInsuranceFolderDetails As DataSet = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Insurance_Folder_And_Party")
            cmd.AddInParameter("@InsuranceFileRef", SqlDbType.VarChar).Value = Cast.ToString(sInsuranceFileRef)

            dsInsuranceFolderDetails = con.ExecuteDataSet(cmd, "InsuranceFolderAndParty")

            If dsInsuranceFolderDetails.Tables(0).Rows.Count <> 0 Then
                r_iInsuranceFolderKey = Cast.ToInt32(dsInsuranceFolderDetails.Tables(0).Rows(0).Item("Insurance_Folder_Cnt"), 0)
                r_iPartyKey = Cast.ToInt32(dsInsuranceFolderDetails.Tables(0).Rows(0).Item("Party_Cnt"), 0)
            Else
                r_iInsuranceFolderKey = 0
                r_iPartyKey = 0
            End If

        End Using

    End Sub

    Private Sub ProcessImportFinancePlan(ByVal con As SiriusConnection, _
                                         ByVal InsuranceFileKey As Integer, _
                                         ByVal PolicyVersion As BaseQuoteRiskMsgType, _
                                         ByVal ClientAddress As BaseAddressType, _
                                         ByVal sShortName As String, _
                                         ByVal sResolvedName As String, _
                                         ByVal oSAMErrorCollection As SAMErrorCollection)

        PolicyVersion.ValidateFinancePlan(CObj(oSAMErrorCollection))

        oSAMErrorCollection.CheckForErrors()

        Dim vArray(k_PFPlanCountOfFields, 0) As Object
        Dim iPFRFID As Integer

        With PolicyVersion

            'Retrieve the Premium Finance Rate ID
            GetPFRFID(con, .FinanceCompanyNo, .FinanceSchemeNo, .FinanceSchemeVersion, iPFRFID)

            vArray(k_PFPlanClientId, 0) = .PartyKey
            vArray(k_PFPlanCompanyNo, 0) = .FinanceCompanyNo
            vArray(k_PFPlanSchemeNo, 0) = .FinanceSchemeNo
            vArray(k_PFPlanSchemeVersion, 0) = .FinanceSchemeVersion
            vArray(k_PFPlanStartDate, 0) = .FinanceStartDate
            vArray(k_PFPlanEndDate, 0) = .FinanceEndDate
            vArray(k_PFPlanCompanyName, 0) = ""
            vArray(k_PFPlanSchemeName, 0) = ""
            vArray(k_PFPlanProductClass, 0) = "NB"
            vArray(k_PFPlanTransactionType, 0) = "NB"
            vArray(k_PFPlanAmountToFinance, 0) = .AmountToFinance
            vArray(k_PFPlanAPR, 0) = 0
            vArray(k_PFPlanInterestRate, 0) = 0
            vArray(k_PFPlanDaysDelay, 0) = 0
            vArray(k_PFPlanNoOfInstalments, 0) = 1
            vArray(k_PFPlanFirstInstalment, 0) = .AmountToFinance
            vArray(k_PFPlanOtherInstalments, 0) = 0
            vArray(k_PFPlanCostOfProtection, 0) = 0
            vArray(k_PFPlanDeposit, 0) = 0
            vArray(k_PFPlanNetAmount, 0) = .AmountToFinance
            vArray(k_PFPlanTotalCost, 0) = .AmountToFinance
            vArray(k_PFPlanInterestCost, 0) = 0
            vArray(k_PFPlanMinFinanceCharge, 0) = 0
            vArray(k_PFPlanPayProtection, 0) = "N"
            If ClientAddress IsNot Nothing Then
                vArray(k_PFPlanClientName, 0) = sResolvedName
                vArray(k_PFPlanClientAddress1, 0) = ClientAddress.AddressLine1
                vArray(k_PFPlanClientAddress2, 0) = ClientAddress.AddressLine2
                vArray(k_PFPlanClientAddress3, 0) = ClientAddress.AddressLine3
                vArray(k_PFPlanClientAddress4, 0) = ClientAddress.AddressLine4
                vArray(k_PFPlanClientPostcode, 0) = ClientAddress.PostCode
            End If
            vArray(k_PFPlanBankName, 0) = .BankName
            vArray(k_PFPlanBankSortCode, 0) = .BankSortCode
            vArray(k_PFPlanBankAccountNo, 0) = .BankAccountNo
            vArray(k_PFPlanBankBranch, 0) = .BankBranch
            If .BankAddress IsNot Nothing Then
                vArray(k_PFPlanBankAddress1, 0) = .BankAddress.AddressLine1
                vArray(k_PFPlanBankAddress2, 0) = .BankAddress.AddressLine2
                vArray(k_PFPlanBankAddress3, 0) = .BankAddress.AddressLine3
                vArray(k_PFPlanBankAddress4, 0) = .BankAddress.AddressLine3
                vArray(k_PFPlanBankTown, 0) = ""
                vArray(k_PFPlanBankPostcode, 0) = .BankAddress.PostCode
                vArray(k_PFPlanBankCountry_ID, 0) = .BankAddress.CountryId
            End If
            vArray(k_PFPlanBankAreaCode, 0) = ""
            vArray(k_PFPlanBankPhone, 0) = .BankPhone
            vArray(k_PFPlanBankExtn, 0) = ""
            vArray(k_PFPlanBankFaxCode, 0) = ""
            vArray(k_PFPlanBankFax, 0) = ""
            vArray(k_PFPlanStatusInd, 0) = InstalmentPlanStatus.Completed
            vArray(k_PFPlanClientCode, 0) = sShortName
            vArray(k_PFPlanSystemTag, 0) = ""
            vArray(k_PFPlanAutoGenPlanRef, 0) = ""
            vArray(k_PFPlanFinCollPlanRef, 0) = ""
            vArray(k_PFPlanInterestFree, 0) = "N"
            vArray(k_PFPlanIsQuote, 0) = 0
            vArray(k_PFPlanIsParentPlan, 0) = 0
            vArray(k_PFPlanParentPlanCnt, 0) = DBNull.Value
            vArray(k_PFPlanParentPlanVersion, 0) = DBNull.Value
            vArray(k_PFPlanPlanTransactionID, 0) = DBNull.Value
            vArray(k_PFPlanPFRF_ID, 0) = iPFRFID
            vArray(k_PFPlanFirstInstalmentdate, 0) = .FinanceStartDate
            vArray(k_PFPlanNextInstalmentdate, 0) = .FinanceStartDate
            vArray(k_PFPlanLastInstalmentdate, 0) = .FinanceStartDate
            vArray(k_PFPlanTaxCost, 0) = 0
            vArray(k_PFPlanCCNumber, 0) = ""
            vArray(k_PFPlanCCExpiryDate, 0) = ""
            vArray(k_PFPlanCCStartDate, 0) = ""
            vArray(k_PFPlanCCIssue, 0) = ""
            vArray(k_PFPlanCCPin, 0) = ""
            vArray(k_PFPlanInsuranceFileCnt, 0) = InsuranceFileKey
            vArray(k_PFPlanFinanceCharge, 0) = 0
            vArray(k_PFPlanDayOfWeekOrMonth, 0) = .DayOfWeekOrMonth
            vArray(k_PFPlanOriginalAmount, 0) = .AmountToFinance
            vArray(k_PFPlanLastInstalment, 0) = 0
            vArray(k_PFPlanClaimDebtID, 0) = DBNull.Value
            vArray(k_PFPlanUserID, 0) = 1
            vArray(k_PFPlanAgentCnt, 0) = DBNull.Value
            vArray(k_PFPlanAgentRef, 0) = DBNull.Value
            vArray(k_PFPlanDateCreated, 0) = .FinanceStartDate
            vArray(k_PFPlanDateModified, 0) = DBNull.Value
            vArray(k_PFPlanDateConfirmed, 0) = DBNull.Value
            vArray(k_PFPlanDateReview, 0) = DBNull.Value
            vArray(k_PFPlanDateLastStatement, 0) = DBNull.Value
            vArray(k_PFPlanDateLastGeneration, 0) = DBNull.Value
            vArray(k_PFPlanFinalStatementSet, 0) = DBNull.Value
            vArray(k_PFPlanNoStatements, 0) = DBNull.Value
            vArray(k_PFPlanStatementPFFrequencyID, 0) = DBNull.Value
            vArray(k_PFPlanReviewPMWorkTaskInstance, 0) = DBNull.Value
            vArray(k_PFPlanAuthCode, 0) = ""
            vArray(k_PFPlanBusinessCode, 0) = ""
            vArray(k_PFPlanTerms, 0) = DBNull.Value
            vArray(k_PFPlanReference, 0) = ""
            vArray(k_PFPlanDocURL, 0) = ""
            vArray(k_PFPlanOriginalRate, 0) = DBNull.Value
            vArray(k_PFPlanRefundType, 0) = ""
            vArray(k_PFPlanLimitedCompany, 0) = DBNull.Value
            vArray(k_PFPlanIsCardholder, 0) = DBNull.Value
            vArray(k_PfPlanCardholderName, 0) = ""
            vArray(k_PfPlanCardholderAddress1, 0) = ""
            vArray(k_PfPlanCardholderAddress2, 0) = ""
            vArray(k_PfPlanCardholderAddress3, 0) = ""
            vArray(k_PfPlanCardholderAddress4, 0) = ""
            vArray(k_PfPlanCardholderPostcode, 0) = ""
            vArray(k_PFPlanCardType, 0) = ""
            vArray(k_PFPlanProviderCollectDeposit, 0) = DBNull.Value
            vArray(k_PfPlanDateBankDetailsChanged, 0) = DBNull.Value
            vArray(k_PfPlanTaxGroupID, 0) = DBNull.Value
            vArray(k_PFPlanSource_ID, 0) = .SourceId
            vArray(k_PFPlanSubBranchID, 0) = 0
            vArray(k_PFPlanBatchID, 0) = 0
        End With

        Dim oPremiumFinance As bSIRPremiumFinance.Business = Nothing
        Try
            oPremiumFinance = New bSIRPremiumFinance.Business
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Debug.WriteLine(ex.Message)
        Finally
        End Try

        ' Initialise the GIS
        SAMFunc.InitialiseSBOObject(Con:=con, oObject:=oPremiumFinance, SiriusUser:=_SiriusUser, sObjectName:="bSIRPremiumFinance.Business")

        Dim lReturn As Integer = oPremiumFinance.InsertNewPremiumFinance( _
                                        v_vPFPlanArray:=vArray, _
                                        v_vPFTransArray:=Nothing, _
                                        r_lPremiumFinanceCnt:=0, _
                                        r_lPremiumFinanceVer:=0)

        If (lReturn <> PMEReturnCode.PMTrue) Then
            ' if the account processing fails then throw a business rule error
            oSAMErrorCollection = New SAMErrorCollection
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                SAMBusinessErrors.COMComponentMethodFailed.ToString, _
                                                "bSIRPremiumFinance.Business")
            oSAMErrorCollection.CheckForErrors()
        End If

    End Sub

    Public Sub SaveClaimRIArrangement( _
    ByVal con As SiriusConnection, _
    ByVal claimRIArrangement As BaseCDTClaimReinsuranceTypeClaimRIArrangement)

        Dim business As New CoreBusiness
        Dim samErrorCollection As New SAMErrorCollection

        '*********************
        ' STRUCTURE VALIDATION 
        '*********************
        ' validate the mandatory structure data
        claimRIArrangement.Validate(CType(samErrorCollection, Object))

        ' if there were any errors throw an exception
        samErrorCollection.CheckForErrors()

        '*********************
        ' DATA VALIDATION 
        '*********************
        ' validate the data provided in the structure
        ClaimRIArrangementValidateData(con, business, claimRIArrangement, samErrorCollection)
        samErrorCollection.CheckForErrors()

        ' clear down any existing rows ( created by copy claim routine ) 
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CLM_DeleteClaimReinsurance")
            cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = claimRIArrangement.ClaimKey
            con.ExecuteNonQuery(cmd)
        End Using

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Claim_RI_Arrangement_add")

            cmd.AddOutParameter("@ri_arrangement_id", SqlDbType.Int)
            cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = claimRIArrangement.ClaimKey
            cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = claimRIArrangement.RiskKey
            cmd.AddInParameter("@claim_allocation_type", SqlDbType.Int).Value = claimRIArrangement.ClaimAllocationType
            cmd.AddInParameter("@ri_band_id", SqlDbType.Int).Value = claimRIArrangement.RIBandID
            cmd.AddInParameter("@ri_model_id", SqlDbType.Int).Value = claimRIArrangement.RIModelID
            cmd.AddInParameter("@sum_insured", SqlDbType.Money).Value = claimRIArrangement.SumInsured
            cmd.AddInParameter("@reserve", SqlDbType.Money).Value = claimRIArrangement.Reserve
            cmd.AddInParameter("@payment", SqlDbType.Money).Value = claimRIArrangement.Payment
            cmd.AddInParameter("@salvage", SqlDbType.Money).Value = claimRIArrangement.Salvage
            cmd.AddInParameter("@recovery", SqlDbType.Money).Value = claimRIArrangement.Recovery
            cmd.AddInParameter("@is_modified", SqlDbType.TinyInt).Value = 0
            cmd.AddInParameter("@this_reserve", SqlDbType.Money).Value = claimRIArrangement.ThisReserve
            cmd.AddInParameter("@this_payment", SqlDbType.Money).Value = claimRIArrangement.ThisPayment
            cmd.AddInParameter("@this_salvage", SqlDbType.Money).Value = claimRIArrangement.ThisSalvage
            cmd.AddInParameter("@this_recovery", SqlDbType.Money).Value = claimRIArrangement.ThisRecovery
            cmd.AddInParameter("@original_ri_arrangement_id", SqlDbType.TinyInt).Value = 0
            cmd.AddInParameter("@ri_arrangement_version", SqlDbType.TinyInt).Value = 0

            con.ExecuteNonQuery(cmd)

            claimRIArrangement.ClaimRIArrangementId = Cast.ToInt32(cmd.Parameters("@ri_arrangement_id").Value, 0)

            AddClaimRIArrangementLines(con, _
                               business, _
                               claimRIArrangement.ClaimRIArrangmentLine, _
                               claimRIArrangement.ClaimKey, _
                               claimRIArrangement.ClaimRIArrangementId, _
                               samErrorCollection)
        End Using

    End Sub

    Private Sub ClaimRIArrangementValidateData( _
ByVal con As SiriusConnection, _
ByVal oBusiness As CoreBusiness, _
ByRef ClaimRIArrangement As BaseCDTClaimReinsuranceTypeClaimRIArrangement, _
ByRef oSAMErrorCollection As SAMErrorCollection)

        '***************************
        ''' validate standard lookup data
        '***************************

        ClaimRIArrangementValidateDataStandardLookup(oBusiness, ClaimRIArrangement, oSAMErrorCollection)

    End Sub

    Private Sub ClaimRIArrangementValidateDataStandardLookup( _
   ByVal oBusiness As CoreBusiness, _
   ByRef oRequest As BaseCDTClaimReinsuranceTypeClaimRIArrangement, _
   ByRef oSAMErrorCollection As SAMErrorCollection)

        '*********************
        'mandatory
        '*********************

        '*********************
        ' optional
        '*********************

        ' RI_Band Code
        If String.IsNullOrEmpty(oRequest.RIBandCode) = False Then
            oRequest.RIBandID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RIBand, oRequest.RIBandCode, "Code", oSAMErrorCollection)
        End If

        ' RI_Model Code
        If String.IsNullOrEmpty(oRequest.RIModelCode) = False Then
            oRequest.RIModelID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RIModel, oRequest.RIModelCode, "Code", oSAMErrorCollection)
        End If

    End Sub

    Public Sub AddClaimRIArrangementLines( _
    ByVal con As SiriusConnection, _
    ByVal business As CoreBusiness, _
    ByVal claimRIArrangementLines() As BaseCDTClaimRIArrangmentLineType, _
    ByVal claimKey As Integer, _
    ByVal riArrangementID As Integer, _
    ByVal samErrorCollection As SAMErrorCollection)

        If claimRIArrangementLines IsNot Nothing Then

            For Each claimRIArrangementLine As BaseCDTClaimRIArrangmentLineType In claimRIArrangementLines

                '*********************
                ' STRUCTURE VALIDATION 
                '*********************
                ' validate the mandatory structure data
                claimRIArrangementLine.Validate(CType(samErrorCollection, Object))
                ' if there were any errors throw an exception
                samErrorCollection.CheckForErrors()

                '*********************
                ' DATA VALIDATION 
                '*********************
                ' validate the data provided in the structure
                ClaimRIArrangementLineValidateData(con, business, claimRIArrangementLine, samErrorCollection)

                samErrorCollection.CheckForErrors()

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Claim_RI_Arrangement_Line_add")

                    cmd.AddOutParameter("@ri_arrangement_line_id", SqlDbType.Int)
                    cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = claimKey
                    cmd.AddInParameter("@ri_arrangement_id", SqlDbType.Int).Value = riArrangementID
                    cmd.AddInParameter("@type", SqlDbType.VarChar, 2).Value = Cast.ToStringTrim(claimRIArrangementLine.Type)
                    cmd.AddInParameter("@treaty_id", SqlDbType.Int).Value = Cast.NullIfDefault(claimRIArrangementLine.TreatyID, 0)
                    cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(claimRIArrangementLine.PartyKey, 0)
                    cmd.AddInParameter("@xol_arrangement_id", SqlDbType.Int).Value = Nothing
                    cmd.AddInParameter("@default_share_percent", SqlDbType.Float).Value = CDbl(claimRIArrangementLine.DefaultSharePercent)
                    cmd.AddInParameter("@this_share_percent", SqlDbType.Float).Value = CDbl(claimRIArrangementLine.ThisSharePercent)
                    cmd.AddInParameter("@agreement_code", SqlDbType.VarChar, 255).Value = Cast.ToStringTrim(claimRIArrangementLine.AgreementCode)
                    cmd.AddInParameter("@priority", SqlDbType.Int).Value = claimRIArrangementLine.Priority
                    cmd.AddInParameter("@number_of_lines", SqlDbType.SmallInt).Value = claimRIArrangementLine.NumberOfLines
                    cmd.AddInParameter("@line_limit", SqlDbType.Money).Value = claimRIArrangementLine.LineLimit
                    cmd.AddInParameter("@sum_insured", SqlDbType.Money).Value = claimRIArrangementLine.SumInsured
                    cmd.AddInParameter("@reserve", SqlDbType.Money).Value = claimRIArrangementLine.Reserve
                    cmd.AddInParameter("@payment", SqlDbType.Money).Value = claimRIArrangementLine.Payment
                    cmd.AddInParameter("@this_reserve", SqlDbType.Money).Value = claimRIArrangementLine.ThisReserve
                    cmd.AddInParameter("@this_payment", SqlDbType.Money).Value = claimRIArrangementLine.ThisPayment
                    cmd.AddInParameter("@lower_limit", SqlDbType.Money).Value = claimRIArrangementLine.LowerLimit
                    cmd.AddInParameter("@retained", SqlDbType.Float).Value = CDbl(claimRIArrangementLine.Retained)
                    cmd.AddInParameter("@participation_percent", SqlDbType.Float).Value = CDbl(claimRIArrangementLine.ParticipationPercent)
                    If claimRIArrangementLine.Grouping <> 0 Then
                        cmd.AddInParameter("@grouping", SqlDbType.Int).Value = claimRIArrangementLine.Grouping
                    End If

                    con.ExecuteNonQuery(cmd)

                End Using

            Next

        End If

    End Sub

    Private Sub ClaimRIArrangementLineValidateData( _
ByVal con As SiriusConnection, _
ByVal oBusiness As CoreBusiness, _
ByRef ClaimRIArrangementLine As BaseCDTClaimRIArrangmentLineType, _
ByRef oSAMErrorCollection As SAMErrorCollection)

        '***************************
        ''' validate standard lookup data
        '***************************

        ClaimRIArrangementLineValidateDataStandardLookup(oBusiness, ClaimRIArrangementLine, oSAMErrorCollection)

    End Sub

    Private Sub ClaimRIArrangementLineValidateDataStandardLookup( _
   ByVal oBusiness As CoreBusiness, _
   ByRef oRequest As BaseCDTClaimRIArrangmentLineType, _
   ByRef oSAMErrorCollection As SAMErrorCollection)

        '*********************
        'mandatory
        '*********************

        '*********************
        ' optional
        '*********************

        ' Treaty Code
        If String.IsNullOrEmpty(oRequest.TreatyCode) = False Then
            oRequest.TreatyID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Treaty, oRequest.TreatyCode, "Code", oSAMErrorCollection)
        End If

    End Sub

    Public Sub AddRiskRatingSections( _
ByVal con As SiriusConnection, _
ByVal oBusiness As CoreBusiness, _
ByVal AddRiskRatingSectionsRequest() As BaseRiskRatingSectionType, _
ByVal iInsuranceFileCnt As Integer, _
ByVal iRiskKey As Integer, _
ByRef oSAMErrorCollection As SAMErrorCollection)

        If IsArray(AddRiskRatingSectionsRequest) Then

            Dim iLBound As Integer = AddRiskRatingSectionsRequest.GetLowerBound(0)
            Dim iUBound As Integer = AddRiskRatingSectionsRequest.GetUpperBound(0)

            For iCnt As Integer = iLBound To iUBound

                Dim RiskRatingSection As BaseRiskRatingSectionType = AddRiskRatingSectionsRequest(iCnt)

                '*********************
                ' STRUCTURE VALIDATION 
                '*********************
                ' validate the mandatory structure data
                RiskRatingSection.Validate(CType(oSAMErrorCollection, Object))
                ' if there were any errors throw an exception
                oSAMErrorCollection.CheckForErrors()
                '*********************
                ' DATA VALIDATION 
                '*********************
                ' validate the data provided in the structure
                RiskRatingSectionValidateData(con, oBusiness, RiskRatingSection, oSAMErrorCollection)
                oSAMErrorCollection.CheckForErrors()

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_sir_peril_allocation")

                    cmd.AddInParameter("@rating_section_type_id", SqlDbType.Int).Value = RiskRatingSection.RatingSectionTypeId
                    cmd.AddInParameter("@policy_section_type_id", SqlDbType.Int).Value = DBNull.Value
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = iInsuranceFileCnt
                    cmd.AddInParameter("@risk_id", SqlDbType.Int).Value = iRiskKey
                    cmd.AddInParameter("@sum_insured", SqlDbType.Money).Value = RiskRatingSection.SumInsured
                    cmd.AddInParameter("@annual_rate", SqlDbType.Money).Value = RiskRatingSection.AnnualRate
                    cmd.AddInParameter("@annual_premium", SqlDbType.Money).Value = RiskRatingSection.AnnualPremium
                    cmd.AddInParameter("@this_premium", SqlDbType.Money).Value = RiskRatingSection.ThisPremium
                    cmd.AddInParameter("@rate_type_id", SqlDbType.Int).Value = RiskRatingSection.RateTypeID
                    cmd.AddInParameter("@insurance_file_no_of_dp", SqlDbType.SmallInt).Value = DBNull.Value
                    cmd.AddInParameter("@original_flag", SqlDbType.Int).Value = IIf(RiskRatingSection.OriginalFlag = True, 1, 0)
                    cmd.AddInParameter("@currency_id", SqlDbType.SmallInt).Value = DBNull.Value
                    cmd.AddInParameter("@country_id", SqlDbType.SmallInt).Value = IIf(RiskRatingSection.CountryID = 0, DBNull.Value, RiskRatingSection.CountryID)
                    cmd.AddInParameter("@state_id", SqlDbType.SmallInt).Value = IIf(RiskRatingSection.StateID = 0, DBNull.Value, RiskRatingSection.StateID)
                    cmd.AddInParameter("@is_amended", SqlDbType.SmallInt).Value = DBNull.Value
                    cmd.AddInParameter("@calculated_premium", SqlDbType.Money).Value = DBNull.Value
                    cmd.AddInParameter("@override_reason", SqlDbType.VarChar, 255).Value = DBNull.Value
                    cmd.AddInParameter("@Auto_calculated", SqlDbType.VarChar, 255).Value = DBNull.Value

                    con.ExecuteNonQuery(cmd)

                End Using

            Next iCnt

        End If

    End Sub

    Private Sub RiskRatingSectionValidateData( _
ByVal con As SiriusConnection, _
ByVal oBusiness As CoreBusiness, _
ByRef RiskRatingSection As BaseRiskRatingSectionType, _
ByRef oSAMErrorCollection As SAMErrorCollection)

        '***************************
        ''' validate standard lookup data
        '***************************

        RiskRatingSectionValidateDataStandardLookup(oBusiness, RiskRatingSection, oSAMErrorCollection)

    End Sub

    Private Sub RiskRatingSectionValidateDataStandardLookup( _
   ByVal oBusiness As CoreBusiness, _
   ByRef oRequest As BaseRiskRatingSectionType, _
   ByRef oSAMErrorCollection As SAMErrorCollection)

        '*********************
        'mandatory
        '*********************

        ' Rating section type code
        oRequest.RatingSectionTypeId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RatingSectionType, oRequest.RatingSectionTypeCode, "code", oSAMErrorCollection)

        '*********************
        ' optional
        '*********************

        ' Rate type Code
        If String.IsNullOrEmpty(oRequest.RateTypeCode) = False Then
            oRequest.RateTypeID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RateType, oRequest.RateTypeCode, "Code", oSAMErrorCollection)
        End If

        ' Country Code
        If String.IsNullOrEmpty(oRequest.CountryCode) = False Then
            oRequest.CountryID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Country, oRequest.CountryCode, "CountryCode", oSAMErrorCollection)
        End If

        ' State Code
        If String.IsNullOrEmpty(oRequest.StateCode) = False Then
            oRequest.StateID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.State, oRequest.StateCode, "Code", oSAMErrorCollection)
        End If

    End Sub

    Public Sub AddRiskRIArrangements( _
ByVal con As SiriusConnection, _
ByVal oBusiness As CoreBusiness, _
ByVal AddRiskRIArrangementsRequest() As BaseRiskRIArrangementType, _
ByVal RiskKey As Integer, _
ByRef RIArrangementID As Integer, _
ByRef oSAMErrorCollection As SAMErrorCollection)

        If IsArray(AddRiskRIArrangementsRequest) Then

            ' Clear down any existing Reinsurance
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_DT_Delete_Risk_RI_Arrangement_Lines")
                cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = RiskKey
                con.ExecuteNonQuery(cmd)
            End Using
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_DT_Delete_Risk_RI_Arrangements")
                cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = RiskKey
                con.ExecuteNonQuery(cmd)
            End Using
            Dim iLBound As Integer = AddRiskRIArrangementsRequest.GetLowerBound(0)
            Dim iUBound As Integer = AddRiskRIArrangementsRequest.GetUpperBound(0)

            For iCnt As Integer = iLBound To iUBound

                Dim RiskRIArrangement As BaseRiskRIArrangementType = AddRiskRIArrangementsRequest(iCnt)

                '*********************
                ' STRUCTURE VALIDATION 
                '*********************
                ' validate the mandatory structure data
                RiskRIArrangement.Validate(CType(oSAMErrorCollection, Object))
                ' if there were any errors throw an exception
                oSAMErrorCollection.CheckForErrors()
                '*********************
                ' DATA VALIDATION 
                '*********************
                ' validate the data provided in the structure
                RiskRIArrangementValidateData(con, oBusiness, RiskRIArrangement, oSAMErrorCollection)
                oSAMErrorCollection.CheckForErrors()

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_RI_Arrangement_add")

                    cmd.AddOutParameter("@ri_arrangement_id", SqlDbType.Int)
                    cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = RiskKey
                    cmd.AddInParameter("@ri_band_id", SqlDbType.Int).Value = RiskRIArrangement.RIBandID
                    cmd.AddInParameter("@ri_model_id", SqlDbType.Int).Value = RiskRIArrangement.RIModelID
                    cmd.AddInParameter("@sum_insured", SqlDbType.Money).Value = RiskRIArrangement.SumInsured
                    cmd.AddInParameter("@premium", SqlDbType.Money).Value = RiskRIArrangement.Premium
                    cmd.AddInParameter("@original_flag", SqlDbType.TinyInt).Value = IIf(RiskRIArrangement.OriginalFlag = True, 1, 0)
                    cmd.AddInParameter("@is_modified", SqlDbType.TinyInt).Value = 0

                    con.ExecuteNonQuery(cmd)

                    RIArrangementID = Cast.ToInt32(cmd.Parameters("@ri_arrangement_id").Value, 0)

                End Using

                AddRiskRIArrangementLines(con, _
                                            oBusiness, _
                                            RiskRIArrangement.RIArrangementLine, _
                                            RiskKey, _
                                            RIArrangementID, _
                                            oSAMErrorCollection)
            Next iCnt

        End If

    End Sub

    Private Sub RiskRIArrangementValidateData( _
ByVal con As SiriusConnection, _
ByVal oBusiness As CoreBusiness, _
ByRef RiskRIArrangement As BaseRiskRIArrangementType, _
ByRef oSAMErrorCollection As SAMErrorCollection)

        '***************************
        ''' validate standard lookup data
        '***************************

        RiskRIArrangementValidateDataStandardLookup(oBusiness, RiskRIArrangement, oSAMErrorCollection)

    End Sub

    Private Sub RiskRIArrangementValidateDataStandardLookup( _
   ByVal oBusiness As CoreBusiness, _
   ByRef oRequest As BaseRiskRIArrangementType, _
   ByRef oSAMErrorCollection As SAMErrorCollection)

        '*********************
        'mandatory
        '*********************

        '*********************
        ' optional
        '*********************

        ' RI_Band Code
        If String.IsNullOrEmpty(oRequest.RIBandCode) = False Then
            oRequest.RIBandID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RIBand, oRequest.RIBandCode, "Code", oSAMErrorCollection)
        End If

        ' RI_Model Code
        If String.IsNullOrEmpty(oRequest.RIModelCode) = False Then
            oRequest.RIModelID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RIModel, oRequest.RIModelCode, "Code", oSAMErrorCollection)
        End If

    End Sub

    Public Sub AddRiskRIArrangementLines( _
ByVal con As SiriusConnection, _
ByVal oBusiness As CoreBusiness, _
ByVal AddRiskRIArrangementLinesRequest() As BaseRiskRIArrangementLineType, _
ByVal RiskKey As Integer, _
ByVal RIArrangementID As Integer, _
ByRef oSAMErrorCollection As SAMErrorCollection)

        If IsArray(AddRiskRIArrangementLinesRequest) Then

            Dim iLBound As Integer = AddRiskRIArrangementLinesRequest.GetLowerBound(0)
            Dim iUBound As Integer = AddRiskRIArrangementLinesRequest.GetUpperBound(0)

            For iCnt As Integer = iLBound To iUBound

                Dim RiskRIArrangementLine As BaseRiskRIArrangementLineType = AddRiskRIArrangementLinesRequest(iCnt)

                '*********************
                ' STRUCTURE VALIDATION 
                '*********************
                ' validate the mandatory structure data
                RiskRIArrangementLine.Validate(CType(oSAMErrorCollection, Object))
                ' if there were any errors throw an exception
                oSAMErrorCollection.CheckForErrors()
                '*********************
                ' DATA VALIDATION 
                '*********************
                ' validate the data provided in the structure
                RiskRIArrangementLineValidateData(con, oBusiness, RiskRIArrangementLine, oSAMErrorCollection)
                oSAMErrorCollection.CheckForErrors()

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_RI_Arrangement_Line_add")

                    cmd.AddOutParameter("@ri_arrangement_line_id", SqlDbType.Int)
                    cmd.AddInParameter("@ri_arrangement_id", SqlDbType.Int).Value = RIArrangementID
                    cmd.AddInParameter("@type", SqlDbType.VarChar, 2).Value = Cast.ToStringTrim(RiskRIArrangementLine.Type)
                    cmd.AddInParameter("@treaty_id", SqlDbType.Int).Value = Cast.NullIfDefault(RiskRIArrangementLine.TreatyID, 0)
                    cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(RiskRIArrangementLine.PartyKey, 0)
                    cmd.AddInParameter("@default_share_percent", SqlDbType.Float).Value = (RiskRIArrangementLine.DefaultSharePercent / 100)
                    cmd.AddInParameter("@this_share_percent", SqlDbType.Float).Value = (RiskRIArrangementLine.ThisSharePercent / 100)
                    cmd.AddInParameter("@premium_percent", SqlDbType.Float).Value = (RiskRIArrangementLine.PremiumPercent / 100)
                    cmd.AddInParameter("@commission_percent", SqlDbType.Float).Value = (RiskRIArrangementLine.CommissionPercent / 100)
                    cmd.AddInParameter("@agreement_code", SqlDbType.VarChar, 255).Value = Cast.ToStringTrim(RiskRIArrangementLine.AgreementCode)
                    cmd.AddInParameter("@priority", SqlDbType.Int).Value = RiskRIArrangementLine.Priority
                    cmd.AddInParameter("@number_of_lines", SqlDbType.SmallInt).Value = RiskRIArrangementLine.NumberOfLines
                    cmd.AddInParameter("@line_limit", SqlDbType.Money).Value = RiskRIArrangementLine.LineLimit
                    cmd.AddInParameter("@sum_insured", SqlDbType.Money).Value = RiskRIArrangementLine.SumInsured
                    cmd.AddInParameter("@premium_value", SqlDbType.Money).Value = RiskRIArrangementLine.PremiumValue
                    cmd.AddInParameter("@commission_value", SqlDbType.Money).Value = RiskRIArrangementLine.CommissionValue
                    cmd.AddInParameter("@premium_tax", SqlDbType.Money).Value = RiskRIArrangementLine.PremiumTax
                    cmd.AddInParameter("@commission_tax", SqlDbType.Money).Value = RiskRIArrangementLine.CommissionTax
                    cmd.AddInParameter("@is_commission_modified", SqlDbType.TinyInt).Value = 0
                    cmd.AddInParameter("@lower_limit", SqlDbType.Money).Value = RiskRIArrangementLine.LowerLimit
                    cmd.AddInParameter("@retained_percentage", SqlDbType.Float).Value = RiskRIArrangementLine.Retained
                    cmd.AddInParameter("@participation_percent", SqlDbType.Float).Value = RiskRIArrangementLine.ParticipationPercent
                    If RiskRIArrangementLine.Grouping <> 0 And RiskRIArrangementLine.GroupingSpecified = True Then
                        cmd.AddInParameter("@grouping", SqlDbType.Int).Value = RiskRIArrangementLine.Grouping
                    End If

                    con.ExecuteNonQuery(cmd)

                End Using

            Next iCnt

        End If

    End Sub

    Private Sub RiskRIArrangementLineValidateData( _
ByVal con As SiriusConnection, _
ByVal oBusiness As CoreBusiness, _
ByRef RiskRIArrangementLine As BaseRiskRIArrangementLineType, _
ByRef oSAMErrorCollection As SAMErrorCollection)

        '***************************
        ''' validate standard lookup data
        '***************************

        RiskRIArrangementLineValidateDataStandardLookup(oBusiness, RiskRIArrangementLine, oSAMErrorCollection)

    End Sub

    Private Sub RiskRIArrangementLineValidateDataStandardLookup( _
   ByVal oBusiness As CoreBusiness, _
   ByRef oRequest As BaseRiskRIArrangementLineType, _
   ByRef oSAMErrorCollection As SAMErrorCollection)

        '*********************
        'mandatory
        '*********************

        '*********************
        ' optional
        '*********************

        ' Treaty Code
        If String.IsNullOrEmpty(oRequest.TreatyCode) = False Then
            oRequest.TreatyID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Treaty, oRequest.TreatyCode, "Code", oSAMErrorCollection)
        End If

    End Sub

    Public Function PolicyDataImport(ByVal PolicyDataImportRequest As BasePolicyDataImportRequestType) As BasePolicyDataImportResponseType

        'Const ACMethodName As String = "PolicyDataImport"

        ' Declare the Response object
        Dim oResponse As New BaseImplementationTypes.BasePolicyDataImportResponseType
        Dim nTypeOfPackage As enumTypeOfPackage

        ' Declare the Core SAM business object
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

        Dim PolicyStatusID As Integer = 0

        ' determine the type of package and thus the type of response
        If PolicyDataImportRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.PolicyDataImportRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.PolicyDataImportResponseType
        End If

        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Try

                ' validate the mandatory structure data
                PolicyDataImportRequest.Validate(CType(oSAMErrorCollection, Object))

                ' if there were any errors throw an exception
                oSAMErrorCollection.CheckForErrors()

                Dim bPartyKeyPresent As Boolean = False
                Dim lPartyKey As Integer
                If (PolicyDataImportRequest.PolicyVersion.PartyKeySpecified) Then
                    ' Checking whether the party key is valid
                    lPartyKey = GetAndValidateSpecifiedTableCode(con, "Party", "Party_Cnt", "Party_Cnt", PolicyDataImportRequest.PolicyVersion.PartyKey.ToString, oSAMErrorCollection, "PartyKey")
                    oSAMErrorCollection.CheckForErrors()
                    bPartyKeyPresent = True
                End If

                Dim ClientAddress As BaseAddressWithContactsType = Nothing
                Dim sShortCode As String
                Dim sResolveName As String
                Dim dsClient As DataSet = Nothing

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_sel")
                    cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PolicyDataImportRequest.PolicyVersion.PartyKey

                    dsClient = con.ExecuteDataSet(cmd, "Client")

                    If dsClient.Tables(0).Rows.Count <> 0 Then
                        sShortCode = dsClient.Tables(0).Rows(0).Item("shortname").ToString
                        sResolveName = dsClient.Tables(0).Rows(0).Item("resolved_name").ToString
                    Else
                        sShortCode = ""
                        sResolveName = ""
                    End If

                End Using

                Dim Addresses() As BaseAddressWithContactsType
                Addresses = GetPartyAddresses(con, PolicyDataImportRequest.PolicyVersion.PartyKey)

                If Addresses IsNot Nothing Then
                    For iCorrespondence As Integer = Addresses.GetLowerBound(0) To Addresses.GetUpperBound(0)
                        If Addresses(iCorrespondence).AddressTypeCode = AddressTypeType.Correspondence Then
                            ClientAddress = Addresses(iCorrespondence)
                            Exit For
                        End If
                    Next iCorrespondence%
                End If

                ' Process the Policy Version structure
                If PolicyDataImportRequest.PolicyVersion IsNot Nothing Then

                    Dim iInsuranceFolderKey As Integer
                    Dim iPartyKey As Integer

                    With PolicyDataImportRequest.PolicyVersion

                        'Check if the insurance folder already exists for this PolicyNumber 
                        GetInsuranceFolderAndParty(con, .QuoteRef, iInsuranceFolderKey, iPartyKey)

                        If (iPartyKey <> .PartyKey) And (iPartyKey <> 0) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.PolicyNumberIsAlreadyInUse, _
                                               SAMBusinessErrors.PolicyNumberIsAlreadyInUse.ToString, _
                                               "Policy Number (QuoteRef) - " & .QuoteRef & " is in use by Party Key - " & iPartyKey.ToString)

                            oSAMErrorCollection.CheckForErrors()

                        End If

                        If iInsuranceFolderKey > 0 Then
                            .InsuranceFolderKey = iInsuranceFolderKey
                        End If

                        If .PolicyVersion = 0 Then
                            oSAMErrorCollection.AddInvalidData( _
                                        SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "PolicyVersion")
                        End If

                        If SAMFunc.NothingToString(.PolicyStatusCode) = String.Empty Then
                            oSAMErrorCollection.AddInvalidData( _
                                        SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "PolicyStatusCode")
                        End If

                        oSAMErrorCollection.CheckForErrors()

                        PolicyStatusID = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
                            PMLookupTable.PolicyStatus, .PolicyStatusCode, "PolicyStatusCode", oSAMErrorCollection)

                        oSAMErrorCollection.CheckForErrors()

                    End With

                    Dim oAddPolicyResponse As BaseNBQuoteResponseTypePolicy

                    con.BeginTransaction()
                    PolicyDataImportRequest.PolicyVersion.DataTransfer = True ' ' PN 2561

                    PolicyDataImportRequest.PolicyVersion.DataTransfer() = True ' ' PN 2561

                    ' Add the Policy record by calling the implementation method 
                    oAddPolicyResponse = AddPolicy(con, PolicyDataImportRequest.PolicyVersion)

                    ' Check the response for errors
                    SAMErrorCollection.CheckForErrorsFromSTS(oAddPolicyResponse.STSError)

                    iInsuranceFolderKey = oAddPolicyResponse.InsuranceFolderKey

                    'Update the Insurance File Premium to that of the rating sections
                    UpdateInsuranceFileDetails(con, oResponse.InsuranceFileKey)

                    'If they passed through a finance scheme number then process the finance plan
                    If PolicyDataImportRequest.PolicyVersion.FinanceSchemeNo <> 0 Then
                        ProcessImportFinancePlan(con, _
                                                oResponse.InsuranceFileKey, _
                                                PolicyDataImportRequest.PolicyVersion, _
                                                ClientAddress, _
                                                sShortCode, _
                                                sResolveName, _
                                                oSAMErrorCollection)

                    End If
                    con.CommitTransaction()

                    oResponse.InsuranceFileKey = oAddPolicyResponse.InsuranceFileKey
                    oResponse.SAMStagingPolicyKey = PolicyDataImportRequest.PolicyVersion.SAMStagingPolicyKey
                    oResponse.InsuranceFolderKey = oAddPolicyResponse.InsuranceFolderKey

                    If oAddPolicyResponse.Risks IsNot Nothing Then
                        oResponse.Risks = Array.ConvertAll(oAddPolicyResponse.Risks, _
                                                            New Converter(Of BaseRiskResultType, BasePolicyDataImportResponseTypeRisks) _
                                                            (AddressOf ToImplementationBasePolicyDataImportResponseTypeRisks))
                    End If

                End If

            Catch
                con.RollbackTransaction()
                Throw

            End Try

        End Using

        Return oResponse

    End Function

    Public Function DocumentDataImport(ByVal DocumentDataImportRequest As BaseDocumentDataImportRequestType) As BasePostDocumentResponseType
        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BasePostDocumentResponseType

        Dim nTypeOfPackage As enumTypeOfPackage

        If DocumentDataImportRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.DocumentDataImportRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New BaseImplementationTypes.BasePostDocumentResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorCollection As New SAMErrorCollection

        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                             _SiriusUser.Username, _SiriusUser.SourceID, _
                                              _SiriusUser.LanguageID, _
                                                  SiriusUserDefaults.AppName)
            Try
                con.BeginTransaction()
                oResponse = PostDocument(con, DocumentDataImportRequest.Document)
                con.CommitTransaction()

            Catch ex As Exception
                con.RollbackTransaction()
                Throw

            End Try
        End Using
        Return oResponse
    End Function

End Class
