Option Strict On

Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports dPMDAOBridge
Imports SSP.Shared

'Namespace CoreImplementation

Namespace XBuilder

    Public Class Party

        Public _siriusUser As SIRIUSUSER

        ''' <summary>
        ''' constructor ( with new sirius user )
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _siriusUser = New SIRIUSUSER
        End Sub

        ''' <summary>
        ''' constructor ( with existing sirius user )
        ''' </summary>
        ''' <param name="siriusUser"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal siriusUser As SIRIUSUSER)
            _siriusUser = siriusUser
        End Sub

        ''' <summary>
        ''' AddDataset - Add Party XMLDataset ( create new connection )
        ''' </summary>
        ''' <param name="branchCode"></param>
        ''' <param name="partyKey"></param>
        ''' <param name="xmlDataset"></param>
        ''' <param name="task"></param>
        ''' <remarks></remarks>
        Public Sub AddDataSet(
        ByVal branchCode As String,
        ByVal partyKey As Integer,
        ByRef xmlDataset As String)
            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                            _siriusUser.Username, _siriusUser.SourceID,
                            _siriusUser.LanguageID,
                            SiriusUserDefaults.AppName)
                AddDataset(con, branchCode, partyKey, xmlDataset)

            End Using

        End Sub

        ''' <summary>
        ''' AddDataset - Add Party XMLDataset ( use specified connection )
        ''' </summary>
        ''' <param name="request"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Sub AddDataset(
        ByVal con As SiriusConnection,
        ByVal branchCode As String,
        ByVal partyKey As Integer,
        ByRef xmlDataset As String)

            'Const ACMethodName As String = "AddDataset"

            Dim internalDataSet As String = String.Empty
            Dim samErrorCollection As New SAMErrorCollection
            Dim dataModelCode As String = String.Empty

            ' does the same thing as get party so forward request to that method
            GetDataset(con, branchCode, partyKey, internalDataSet, dataModelCode, SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction.PMAdd)

            ' if a replacement dataset has been specified to be saved  
            ' ( ONLY EVER DONE BY DATA TRANSFER PROCESS ) 
            ' over the top of the newly added dataset
            If Not String.IsNullOrEmpty(xmlDataset) Then

                If String.IsNullOrEmpty(internalDataSet) Then

                    ' cant replace because there is no dataset to replace -  raise an error
                    samErrorCollection.AddBusinessRule(
                            SAMBusinessErrors.DataTransferAddPartyRiskDatasetFailedAsGetDataSetFailedToReturnAnNewPartyDataset,
                            SAMBusinessErrors.DataTransferAddPartyRiskDatasetFailedAsGetDataSetFailedToReturnAnNewPartyDataset.ToString)

                    samErrorCollection.CheckForErrors()

                Else

                    ' replace the data set
                    ReplaceDataset(con, internalDataSet, xmlDataset, samErrorCollection)

                End If

            Else

                xmlDataset = internalDataSet

            End If

            If Not String.IsNullOrEmpty(xmlDataset) Then
                ' save the data to the database
                ' this is either the translated data transfer dataset
                ' or the newly created dataset with default rules 
                ' already run
                SaveDatasetToDB(con, samErrorCollection, dataModelCode, xmlDataset)
            End If

        End Sub

        ''' <summary>
        ''' GetDataset - Get Party XMLDataset ( create new connection )
        ''' </summary>
        ''' <param name="branchCode"></param>
        ''' <param name="partyKey"></param>
        ''' <param name="xmlDataset"></param>
        ''' <param name="task"></param>
        ''' <remarks></remarks>
        Public Sub GetDataSet(
        ByVal branchCode As String,
        ByVal partyKey As Integer,
        ByRef xmlDataset As String,
        Optional ByVal task As SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction = SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction.PMView)
            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                            _siriusUser.Username, _siriusUser.SourceID,
                            _siriusUser.LanguageID,
                            SiriusUserDefaults.AppName)
                GetDataset(con, branchCode, partyKey, xmlDataset, String.Empty, task)

            End Using

        End Sub

        ''' <summary>
        ''' GetDataset - Get Party XMLDataSet ( use specified connection )
        ''' </summary>
        ''' <param name="request"></param>
        ''' <returns></returns>
        ''' <remarks></remarks> 
        Public Sub GetDataset(
        ByVal con As SiriusConnection,
        ByVal branchCode As String,
        ByVal partyKey As Integer,
        ByRef xmlDataset As String,
        ByRef dataModelCode As String,
        Optional ByVal task As SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction = SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction.PMView)

            'Const ACMethodName As String = "GetDataset"

            Dim samErrorCollection As SAMErrorCollection = New SAMErrorCollection

            ' get the gis screen id 
            Dim gisScreenId As Integer
            Dim partyTypeId As Integer
            Dim dataModelTypeId As Integer
            Dim partyTypeCode As String = String.Empty

            GetXBuilderDetails(con, partyKey, gisScreenId, dataModelCode, dataModelTypeId, partyTypeId, partyTypeCode)

            ' if there is no gis screen associated with this party type then 
            If gisScreenId <> 0 Then

                '*****************************************
                ' Data Validation
                '*****************************************
                ValidateXBuilderData(partyKey, gisScreenId, dataModelCode, dataModelTypeId, samErrorCollection)

                '*****************************************
                ' Processing 
                '*****************************************
                LoadPartyBuilderData(con, partyKey, gisScreenId, dataModelCode, dataModelTypeId, xmlDataset, task)

            End If

        End Sub

        ''' <summary>
        ''' UpdateDataset - Update Party XMLDataSet -  Used By UpdatePartyRisk ( creates a new connection )
        ''' </summary>
        ''' <param name="request"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateDataSet(
        ByVal request As BaseUpdatePartyRiskRequestType) As BaseUpdatePartyRiskResponseType

            'Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
            Dim coreBusiness As New CoreBusiness
            Dim response As BaseUpdatePartyRiskResponseType
            Dim nTypeOfPackage As CoreSAMBusiness.enumTypeOfPackage

            ' determine the type of package and thus the type of response
            If request.GetType Is GetType(SAMForInsuranceImplementationTypes.UpdatePartyRiskRequestType) Then
                nTypeOfPackage = CoreSAMBusiness.enumTypeOfPackage.SAMForInsurancePackage
                response = New SAMForInsuranceImplementationTypes.UpdatePartyRiskResponseType
            Else
                response = New BaseImplementationTypes.BaseUpdatePartyRiskResponseType
            End If
            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                            _siriusUser.Username, _siriusUser.SourceID,
                            _siriusUser.LanguageID,
                            SiriusUserDefaults.AppName)

                ' check timestamp and lock
                coreBusiness.CheckSAMTSAndLock(con,
                    request.BranchCode,
                    CoreBusiness.LockName.PartyCnt,
                    request.PartyKey,
                    request.TimeStamp)

                ' update the dataset
                UpdateDataset(con, request.BranchCode, request.PartyKey, request.XMLDataSet)

                ' populate the response
                response.XMLDataSet = request.XMLDataSet

                ' unlock and get the new timestamp
                coreBusiness.UnlockAndGetSAMTS(
                    con,
                    request.BranchCode,
                    CoreBusiness.LockName.PartyCnt,
                    request.PartyKey,
                    response.TimeStamp)

            End Using

            Return response

        End Function

        ''' <summary>
        ''' UpdateDataset - Update Party XMLDataSet ( use specified connection ) 
        ''' </summary>
        ''' <param name="request"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Sub UpdateDataset( _
        ByVal con As SiriusConnection, _
        ByVal branchCode As String, _
        ByVal partyKey As Integer, _
        ByRef xmlDataset As String)

            'Const ACMethodName As String = "UpdateDataset"

            Dim samErrorCollection As SAMErrorCollection = New SAMErrorCollection

            ' get the gis screen id 
            Dim gisScreenId As Integer
            Dim partyTypeId As Integer
            Dim dataModelCode As String = String.Empty
            Dim dataModelTypeId As Integer
            Dim partyTypeCode As String = String.Empty

            GetXBuilderDetails(con, partyKey, gisScreenId, dataModelCode, dataModelTypeId, partyTypeId, partyTypeCode)

            '*****************************************
            ' Data Validation
            '*****************************************
            ValidateXBuilderData(partyKey, gisScreenId, dataModelCode, dataModelTypeId, samErrorCollection)

            If Not String.IsNullOrEmpty(xmlDataset) Then
                '*****************************************
                ' Processing 
                '*****************************************
                SavePartyBuilderData(con, partyKey, samErrorCollection, gisScreenId, dataModelTypeId, dataModelCode, xmlDataset)
            End If
        End Sub

        ''' <summary>
        ''' save party builder details
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="partyKey"></param>
        ''' <param name="samErrorCollection"></param>
        ''' <param name="screenId"></param>
        ''' <param name="dataModelTypeId"></param>
        ''' <param name="dataModelCode"></param>
        ''' <param name="xmlDataset"></param>
        ''' <remarks></remarks>
        Private Sub SavePartyBuilderData( _
        ByVal con As SiriusConnection, _
        ByVal partyKey As Integer, _
        ByVal samErrorCollection As SAMErrorCollection, _
        ByVal screenId As Integer, _
        ByVal dataModelTypeId As Integer, _
        ByVal dataModelCode As String, _
        ByRef xmlDataset As String)

            Const ACMethodName As String = "RiskScreenOKClickForParty"

            Dim comReturnVal As Integer
            Dim dpmDAODatabase As Object = Nothing
            Dim riskScreen As bSIRRiskScreen.Stateless = New bSIRRiskScreen.Stateless

            riskScreen = CType(riskScreen, bSIRRiskScreen.Stateless)

            'Rk modifies as part of SAM SFI Interop conversions by replacing .PMDAODatabase by .FromSirius.SqlConnection.Database for vDatabase parameter.
            con = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            'dpmDAODatabase = con.SqlConnection.Database

            ' initialise bSIRRiskScreen
            comReturnVal = CInt(riskScreen.Initialise(sUsername:=_siriusUser.Username, _
                                       sPassword:=_siriusUser.Password, _
                                       iUserID:=_siriusUser.UserID, _
                                       iSourceID:=_siriusUser.SourceID, _
                                       iLanguageID:=_siriusUser.LanguageID, _
                                       iCurrencyID:=_siriusUser.CurrencyID, _
                                       iLogLevel:=CShort(SiriusUserDefaults.LogLevel), _
                                       sCallingAppName:=ACMethodName, _
                                       vDatabase:=dpmDAODatabase))

            If (comReturnVal <> PMEReturnCode.PMTrue) Then
                ' if the call to Initialise fails then throw a business rule error
                samErrorCollection = New SAMErrorCollection
                samErrorCollection.AddBusinessRule(SAMBusinessErrors.FailedToInitialiseCOMComponent, _
                                                    SAMBusinessErrors.FailedToInitialiseCOMComponent.ToString, _
                                                    "bSIRRiskScreen.Stateless.Initialise returned the error code - " & comReturnVal)
                samErrorCollection.CheckForErrors()
            End If

            Dim riskScreenOKClick As New XMLTransRiskScreenOKClick
            Dim TheInput As New XMLTransRiskScreenOKClick.RiskScreenOKClickIn
            Dim TheOutput As XMLTransRiskScreenOKClick.RiskScreenOKClickOut

            'Populate the XML for the call to the stateless object
            With TheInput

                .iTask = CShort(SAMComponentAction.PMAdd)
                .iSourceID = 0
                .lNavigate = 0
                .lProcessMode = 0
                .sTransactionType = String.Empty  ' From Claim Table
                .lTransactionType = 0 ' From Claim Table
                .dtEffectiveDate = Now
                .bSubScreen = False
                .lScreenId = screenId ' From Claim Table
                .lRiskId = 0
                .lRiskTypeId = 0 ' Need to get this
                .sGisDataModelCode = dataModelCode ' Claims Datamodel code - Need this
                .lGISDataModelType = dataModelTypeId ' Claims
                .lObjectType = 0
                .sGISXMLDataset = xmlDataset
                .sMyOIKey = "" ' This is passed into bGIS.NBQuoteStatefull but it's never used.
                .sMyObjectName = "" ' Only used for disclosures and standard wordings
                .sParentOIKey = "" ' Only used for disclosures and standard wordings
                .sParentObjectName = "" ' Only used for disclosures and standard wordings
                .lPolicyLinkId = 0 ' Only used for standard wordings
                .lInsuranceFileCnt = 0 ' Need this
                .lPartyKey = partyKey
                .bPostQuote = False
                .dtCoverStartDate = Now
                ReDim .vScreenDetailsArray(34, 0) ' Needs to appear as an array but content is only needed for StandardWordings

            End With

            Dim sInput As String = riskScreenOKClick.SerializeRiskScreenOKClickIn(TheInput)

            ' initialise bSIRRiskScreen
            Dim sOutput As String = riskScreen.RiskScreenOKClick(v_sInput:=sInput)

            TheOutput = riskScreenOKClick.DeserializeRiskScreenOKClickOut(sOutput)

            If (TheOutput.HasErrors = True) Then

                If TheOutput.HasBusinessRuleErrors Then
                    samErrorCollection = New SAMErrorCollection
                    samErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                        TheOutput.Errors.BusinessRule.Detail.Description, _
                                                        TheOutput.Errors.BusinessRule.Detail.Detail.ToString)
                    samErrorCollection.CheckForErrors()
                ElseIf TheOutput.HasBackOfficeErrors Then
                    samErrorCollection = New SAMErrorCollection
                    samErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                        TheOutput.Errors.BackOffice.Detail.Description)
                    samErrorCollection.CheckForErrors()
                ElseIf TheOutput.HasInternalExceptionErrors Then
                    samErrorCollection = New SAMErrorCollection
                    samErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                        TheOutput.Errors.InternalException.Detail.Description)
                    samErrorCollection.CheckForErrors()
                ElseIf TheOutput.HasInvalidDataErrors Then
                    'oSAMErrorCollection = New SAMErrorCollection
                    'oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                    '                                     TheOutput.Errors.InvalidData.Details(0)..Description)                           
                    'oSAMErrorCollection.CheckForErrors()
                End If

            End If

            ' return the updated xmldataset
            With TheOutput
                xmlDataset = .sGISXMLDataset
            End With

        End Sub

        ''' <summary>
        ''' load party builder details ( either initial or saved ) 
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="partyKey"></param>
        ''' <param name="screenId"></param>
        ''' <param name="dataModelCode"></param>
        ''' <param name="dataModelTypeId"></param>
        ''' <param name="xmlDataset"></param>
        ''' <param name="task"></param>
        ''' <remarks></remarks>
        Private Sub LoadPartyBuilderData( _
        ByVal con As SiriusConnection, _
        ByVal partyKey As Integer, _
        ByVal screenId As Integer, _
        ByVal dataModelCode As String, _
        ByVal dataModelTypeId As Integer, _
        ByRef xmlDataset As String, _
        ByVal task As SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction)

            Const ACMethodName As String = "GetPartyRisk"

            Dim riskScreen As bSIRRiskScreen.Stateless = New bSIRRiskScreen.Stateless

            Try

                Dim samErrorCollection As New SAMErrorCollection
                Dim pmDAODataBase As Object = Nothing
                Dim comReturnVal As Integer

                riskScreen = CType(riskScreen, bSIRRiskScreen.Stateless)

                'Rk modifies as part of SAM SFI Interop conversions by replacing .PMDAODatabase by .FromSirius.SqlConnection.Database for vDatabase parameter.
                con = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                'pmDAODataBase = con.SqlConnection.Database

                ' initialise bSIRRiskScreen
                comReturnVal = CInt(riskScreen.Initialise(sUsername:=_siriusUser.Username, _
                                           sPassword:=_siriusUser.Password, _
                                           iUserID:=_siriusUser.UserID, _
                                           iSourceID:=_siriusUser.SourceID, _
                                           iLanguageID:=_siriusUser.LanguageID, _
                                           iCurrencyID:=_siriusUser.CurrencyID, _
                                           iLogLevel:=CShort(SiriusUserDefaults.LogLevel), _
                                           sCallingAppName:=ACMethodName, _
                                           vDatabase:=pmDAODataBase))

                If (comReturnVal <> PMEReturnCode.PMTrue) Then

                    ' if the call to Initialise fails then throw a business rule error
                    samErrorCollection = New SAMErrorCollection
                    samErrorCollection.AddBusinessRule(SAMBusinessErrors.FailedToInitialiseCOMComponent, _
                                                        SAMBusinessErrors.FailedToInitialiseCOMComponent.ToString, _
                                                        "bSIRRiskScreen.Stateless.Initialise returned - " & comReturnVal)
                    samErrorCollection.CheckForErrors()

                End If

                Dim riskScreenLoadRisk As New XMLTransRiskScreenLoadScreen
                Dim TheInput As New XMLTransRiskScreenLoadScreen.RiskScreenLoadRiskIn
                Dim TheOutput As XMLTransRiskScreenLoadScreen.RiskScreenLoadRiskOut

                With TheInput
                    .iTask = CShort(task) ' PMAdd or PMEdit or PMView 
                    .iSourceID = 0 ' Not needed
                    .lNavigate = 0 ' Not needed
                    .lProcessMode = CShort(0) ' Not needed
                    .dtEffectiveDate = Now
                    .bSubScreen = False ' Not a sub screen
                    .lScreenId = screenId ' NEED THE SCREEN ID PASSED IN
                    .lRiskId = 0 ' Pretty sure not needed
                    .lRiskTypeId = 0
                    .sGisDataModelCode = dataModelCode
                    .lGISDataModelType = dataModelTypeId
                    .lObjectType = GISOTRisk
                    .sGISXMLDataset = String.Empty  ' not needed unless processing a subscreen
                    .sMyOIKey = String.Empty  ' Not needed
                    .sMyObjectName = String.Empty ' Not needed
                    .sParentOIKey = String.Empty  ' Not needed
                    .sParentObjectName = String.Empty  ' Not needed
                    .lPolicyLinkId = 0 ' Loaded by the GIS
                    .lInsuranceFolderCnt = 0 ' Only used by Risks DMs
                    .lInsuranceFileCnt = 0 ' Only used by Risks DMs
                    .vScreenDetailsArray = Nothing ' Get's loaded by bSIRRiskScreen
                    .vScreenValuesArray = Nothing ' Doesn't get used
                    .vRiskDetailsArray = Nothing ' Only used by Risks DMs
                    .vRiskTypeDetailsArray = Nothing ' Only used by Risks DMs
                    .sTransactionType = String.Empty  ' The transaction type code of the claim, i.e. Open, Maintain etc.
                    .lTransactionType = 0 ' The transaction type of the claim, i.e. Open, Maintain etc. (I don't know why we need both)
                    .lProductId = 0 ' Only used by Risks DMs
                    .lPartyCnt = partyKey ' Only used by Party DMs 
                    .lClaimID = 0 ' Proper claim id, not the base claim id
                    .bCopyRisk = False
                End With

                Dim sInput As String = riskScreenLoadRisk.SerializeRiskScreenLoadRiskIn(TheInput)

                ' initialise bSIRRiskScreen
                Dim sOutput As String = riskScreen.RiskScreenLoadRisk(v_sInput:=sInput)

                TheOutput = riskScreenLoadRisk.DeserializeRiskScreenLoadRiskOut(sOutput)

                If (TheOutput.HasErrors = True) Then
                    If TheOutput.HasBusinessRuleErrors Then
                        samErrorCollection = New SAMErrorCollection
                        samErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                            TheOutput.Errors.BusinessRule.Detail.Description, _
                                                            TheOutput.Errors.BusinessRule.Detail.Detail.ToString)
                        samErrorCollection.CheckForErrors()
                    ElseIf TheOutput.HasBackOfficeErrors Then
                        samErrorCollection = New SAMErrorCollection
                        samErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                            TheOutput.Errors.BackOffice.Detail.Description)
                        samErrorCollection.CheckForErrors()
                    ElseIf TheOutput.HasInternalExceptionErrors Then
                        samErrorCollection = New SAMErrorCollection
                        samErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                            TheOutput.Errors.InternalException.Detail.Description)
                        samErrorCollection.CheckForErrors()
                    ElseIf TheOutput.HasInvalidDataErrors Then
                        samErrorCollection = New SAMErrorCollection
                        If IsArray(TheOutput.Errors.InvalidData.Details) Then
                            For Cnt As Integer = TheOutput.Errors.InvalidData.Details.GetLowerBound(0) To TheOutput.Errors.InvalidData.Details.GetUpperBound(0)
                                With TheOutput.Errors.InvalidData.Details(Cnt)
                                    samErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                                        .Description, "Invalid data found in " & .FieldName & ".  The supplied value was " & .SuppliedValue.ToString)
                                End With
                            Next Cnt
                        Else
                            samErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                                "bSIRRiskScreen.RiskScreenLoadRisk reported invalid data.")
                        End If

                        samErrorCollection.CheckForErrors()
                    End If
                End If

                With TheOutput
                    xmlDataset = .sGISXMLDataset
                End With

            Finally

                If riskScreen IsNot Nothing Then
                    riskScreen.Dispose()
                    riskScreen = Nothing
                End If

            End Try

        End Sub

        ''' <summary>
        ''' validates the required party builder data is valid
        ''' </summary>
        ''' <param name="partyKey"></param>
        ''' <param name="gisScreenId"></param>
        ''' <param name="dataModelCode"></param>
        ''' <param name="dataModelTypeId"></param>
        ''' <param name="samErrorCollection"></param>
        ''' <remarks></remarks>
        Private Sub ValidateXBuilderData( _
        ByVal partyKey As Integer, _
        ByRef gisScreenId As Integer, _
        ByRef dataModelCode As String, _
        ByRef dataModelTypeId As Integer, _
        ByRef samErrorCollection As SAMErrorCollection)

            Try

                ' if the party does not exist
                If partyKey = 0 Then
                    samErrorCollection.AddInvalidData( _
                            SAMInvalidData.InvalidPartyKeySpecified, _
                            SAMInvalidData.InvalidPartyKeySpecified.ToString, _
                            "PartyKey:" & partyKey & " not found.")
                End If

                ' if there is no valid gis screen id associated with 
                ' the specified parties party type then
                If dataModelTypeId <> GISDMTypeParty AndAlso gisScreenId <> 0 Then
                    samErrorCollection.AddInvalidData( _
                        SAMInvalidData.GisScreenAssociatedWithSpecifiedPartyTypeIsOfTheWrongType, _
                        SAMInvalidData.GisScreenAssociatedWithSpecifiedPartyTypeIsOfTheWrongType.ToString)
                End If

            Finally

                ' if there were any errors throw an exception
                samErrorCollection.CheckForErrors()

            End Try
        End Sub

        ''' <summary>
        ''' returns the required details necessary for party datasets
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="partyKey"></param>
        ''' <param name="gisScreenId"></param>
        ''' <param name="dataModelCode"></param>
        ''' <param name="dataModelTypeId"></param>
        ''' <param name="partyTypeId"></param>
        ''' <remarks></remarks>
        Private Sub GetXBuilderDetails( _
        ByVal con As SiriusConnection, _
        ByRef partyKey As Integer, _
        ByRef gisScreenId As Integer, _
        ByRef dataModelCode As String, _
        ByRef dataModelTypeId As Integer, _
        ByRef partyTypeId As Integer, _
        ByRef partyTypeCode As String)

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Party_Get_GIS_Details")

                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = partyKey
                cmd.AddOutParameter("@gis_screen_id", SqlDbType.Int)
                cmd.AddOutParameter("@party_type_id", SqlDbType.Int)
                cmd.AddOutParameter("@data_model_code", SqlDbType.VarChar, 10)
                cmd.AddOutParameter("@data_model_type_id", SqlDbType.Int)
                cmd.AddOutParameter("@party_type_code", SqlDbType.VarChar, 10)

                con.ExecuteNonQuery(cmd)

                partyKey = Cast.ToInt32(cmd.Parameters("@party_cnt").Value, 0)
                gisScreenId = Cast.ToInt32(cmd.Parameters("@gis_screen_id").Value, 0)
                partyTypeId = Cast.ToInt32(cmd.Parameters("@party_type_id").Value, 0)
                dataModelCode = cmd.Parameters("@data_model_code").Value.ToString
                dataModelTypeId = Cast.ToInt32(cmd.Parameters("@data_model_type_id").Value, 0)
                partyTypeCode = cmd.Parameters("@party_Type_code").Value.ToString

            End Using

        End Sub

        ''' <summary>
        ''' replace the default dataset with the one provided
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="originalXMLDataset"></param>
        ''' <param name="productId"></param>
        ''' <param name="replacementXMLDataset"></param>
        ''' <param name="samErrorCollection"></param>
        ''' <remarks></remarks>
        Private Sub ReplaceDataset(
        ByVal con As SiriusConnection,
        ByVal defaultXMLDataset As String,
        ByRef replacementXMLDataset As String,
        ByVal samErrorCollection As SAMErrorCollection)

            'Const MethodName As String = "ReplaceDataset"

            ' Transform the Dataset into its PB Format
            Dim dataModelCode As String = String.Empty
            Dim gisPolicyLinkId As Integer
            Dim policyBinderId As Integer
            Dim oiKey As String = String.Empty
            Dim outputXMLDataset As String = String.Empty
            Dim xmlDataset As String = String.Empty
            Dim nextOINumber As Integer
            Dim uS As Integer
            Dim xmlDoc As New System.Xml.XmlDocument

            ' load the xml data set generated by default
            ' to get the keys for gis policy link id / policy binder id
            xmlDoc.LoadXml(defaultXMLDataset)

            dataModelCode = xmlDoc.SelectSingleNode("DATA_SET").Attributes("DataModelCode").Value.ToUpper
            gisPolicyLinkId = Int32.Parse(xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode(dataModelCode & "_POLICY_BINDER").Attributes("GIS_POLICY_LINK_ID").Value)
            oiKey = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode(dataModelCode & "_POLICY_BINDER").Attributes("OI").Value
            policyBinderId = Int32.Parse(xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode(dataModelCode & "_POLICY_BINDER").Attributes(dataModelCode & "_POLICY_BINDER_ID").Value)
            nextOINumber = Int32.Parse(xmlDoc.SelectSingleNode("DATA_SET").Attributes("NextOINumber").Value)


            ' create a new xml document
            Dim xmlDocReplacement = New System.Xml.XmlDocument

            ' load the xml to be saved
            xmlDocReplacement.LoadXml(replacementXMLDataset)

            ' replace the gis policy link id / policy binder id 
            ' and update the US (Update Status ) indicators 
            xmlDocReplacement.DocumentElement.Item("RISK_OBJECTS").Item(dataModelCode & "_POLICY_BINDER").SetAttribute("OI", oiKey)
            xmlDocReplacement.DocumentElement.Item("RISK_OBJECTS").Item(dataModelCode & "_POLICY_BINDER").SetAttribute("US", "2")
            xmlDocReplacement.DocumentElement.SetAttribute("DataModelCode", dataModelCode)
            xmlDocReplacement.DocumentElement.Item("RISK_OBJECTS").Item(dataModelCode & "_POLICY_BINDER").SetAttribute("GIS_POLICY_LINK_ID", gisPolicyLinkId.ToString)
            xmlDocReplacement.DocumentElement.Item("RISK_OBJECTS").Item(dataModelCode & "_POLICY_BINDER").SetAttribute(dataModelCode & "_POLICY_BINDER_ID", policyBinderId.ToString)
            xmlDocReplacement.DocumentElement.SetAttribute("NextOINumber", nextOINumber.ToString)



            Dim nodeQueue As New System.Collections.Generic.Queue(Of XmlNode)
            Dim nodeReplacementQueue As New System.Collections.Generic.Queue(Of XmlElement)
            Dim nodePolicyBinder As XmlNode = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode(dataModelCode & "_POLICY_BINDER")
            Dim nodeReplacementPolicyBinder As XmlElement = xmlDocReplacement.DocumentElement.Item("RISK_OBJECTS").Item(dataModelCode & "_POLICY_BINDER")
            nodeQueue.Enqueue(nodePolicyBinder)
            nodeReplacementQueue.Enqueue(nodeReplacementPolicyBinder)

            While nodeQueue.Count > 0
                Dim selectedNode As XmlNode = nodeQueue.Dequeue()
                Dim selectedReplacementNode As XmlElement = nodeReplacementQueue.Dequeue()
                For Each node As XmlNode In selectedNode.ChildNodes
                    uS = Int32.Parse(node.Attributes("US").Value)
                    For Each nodeReplacement As XmlElement In selectedReplacementNode.ChildNodes
                        If nodeReplacement.Name = node.Name Then
                            nodeReplacement.SetAttribute("US", uS.ToString)
                            nodeQueue.Enqueue(node)
                            nodeReplacementQueue.Enqueue(nodeReplacement)
                            Exit For
                        End If
                    Next
                Next
            End While


            xmlDataset = xmlDocReplacement.OuterXml

            ' transform the xml data set into the form required by product builder
            replacementXMLDataset = SAMFunc.TransformDatasetSAMtoPB(con, xmlDataset)

        End Sub



        Private Sub SaveDatasetToDB(ByVal con As SiriusConnection, _
        ByVal samErrorCollection As SAMErrorCollection, _
        ByVal dataModelCode As String, _
        ByVal xmlDataSet As String)

            Dim saveToDBIn As New SaveToDBIn
            Dim saveToDBOut As New SaveToDBOut
            Dim coreBusiness As New CoreBusiness(_siriusUser)

            With saveToDBIn
                .DataModelCode = dataModelCode
                .BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
                .XMLDataset = xmlDataSet
            End With

            ' Save the new dataset to the DB
            saveToDBOut = coreBusiness.SaveToDB(con, saveToDBIn)

            If saveToDBOut.XMLDataset = "" Then
                samErrorCollection.AddFatal("SaveToDB failed to return a valid XMLDatset in the method ProcessClaimRiskXMLDataset")
                samErrorCollection.CheckForErrors()
            End If

        End Sub

    End Class

End Namespace
'End Namespace

