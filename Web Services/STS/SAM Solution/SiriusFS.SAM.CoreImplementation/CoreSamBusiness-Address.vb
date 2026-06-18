Option Strict On

Imports dPMDAOBridge
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SSP.Shared.gPMConstants

Partial Public Class CoreSAMBusiness

    Public Overloads Function AddAddress(ByVal AddAddressInput As BaseAddAddressRequestType) As BaseAddAddressResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseAddAddressResponseType

            oResponse = AddAddress(con, AddAddressInput)
            Return oResponse

        End Using

    End Function

    Public Overloads Function UpdateAddress(ByVal UpdateAddressInput As BaseUpdateAddressRequestType) As BaseUpdateAddressResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oResponse As BaseUpdateAddressResponseType

            oResponse = UpdateAddress(con, UpdateAddressInput)

            Return oResponse
        End Using
    End Function

    Public Overloads Function UpdateAddress(ByVal con As SiriusConnection, ByVal UpdateAddressInput As BaseUpdateAddressRequestType) As BaseUpdateAddressResponseType

        Dim oUpdateAddressOut As New BaseImplementationTypes.BaseUpdateAddressResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim STSError As New STSErrorPublisher
        Dim lReturn As Int32 = 0
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim iCountryCodeID As Integer
        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim AddressKey As Integer
        Dim ds As DataSet
        If UpdateAddressInput.AddressTypeCode.ToString = "" Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                                   "AddressTypeCode")

        End If

        If UpdateAddressInput.AddressLine1 = "" Then
            ' AddressLine1 not set
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                                   "AddressLine1")
        End If

        If UpdateAddressInput.CountryCode = "" Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                                   "CountryCode")
        End If

        oSAMErrorCollection.CheckForErrors()

        ' Check the country code
        Try
            iCountryCodeID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "country", UpdateAddressInput.CountryCode)
        Catch ex As Exception
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCountryCode,
                                                                   "Invalid CountryCode provided", UpdateAddressInput.CountryCode.ToString, UpdateAddressInput.CountryCode)
        End Try

        ' exit if there are any missing parameters
        oSAMErrorCollection.CheckForErrors()

        ' Is the Postcode required for this Country Code ?
        If oCoreBusiness.GetPostCodeVisibility(iCountryCodeID) = InternalSAMConstants.PMPostCodeVisibilityVisible Then
            If SAMFunc.NothingToString(UpdateAddressInput.PostCode) = "" Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                       "PostCode")
            End If
        End If

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_party_corrospondance_address_cnt")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = UpdateAddressInput.PartyKey
            ds = con.ExecuteDataSet(cmd, "ds")
        End Using

        If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            AddressKey = Cast.ToInt32(ds.Tables(0).Rows(0).Item("address_cnt"), 0)

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Address_upd")
                cmd.AddInParameter("@address_cnt", SqlDbType.Int).Value = AddressKey
                cmd.AddInParameter("source_id", SqlDbType.Int).Value = _SiriusUser.SourceID
                cmd.AddInParameter("address_id", SqlDbType.Int).Value = 0
                cmd.AddInParameter("@address1", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine1
                cmd.AddInParameter("@address2", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine2
                cmd.AddInParameter("@address3", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine3
                cmd.AddInParameter("@address4", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine4
                cmd.AddInParameter("@postal_code", SqlDbType.VarChar, 20).Value = IIf(UpdateAddressInput.PostCode Is Nothing, "", UpdateAddressInput.PostCode)
                cmd.AddInParameter("@country_id", SqlDbType.Int).Value = iCountryCodeID
                cmd.AddInParameter("@address5", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine5
                cmd.AddInParameter("@address6", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine6
                cmd.AddInParameter("@address7", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine7
                cmd.AddInParameter("@address8", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine8
                cmd.AddInParameter("@address9", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine9
                cmd.AddInParameter("@address10", SqlDbType.VarChar, 60).Value = UpdateAddressInput.AddressLine10


                con.ExecuteNonQuery(cmd)
            End Using

        End If


        ' Get the new address_cnt
        oUpdateAddressOut.AddressKey = AddressKey

        Return oUpdateAddressOut
    End Function

    Public Overloads Function AddAddress(ByVal con As SiriusConnection, ByVal AddAddressInput As BaseAddAddressRequestType) As BaseAddAddressResponseType

        Const ACMethodName As String = "AddAddress"

        Dim oAddAddressOut As New BaseImplementationTypes.BaseAddAddressResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim STSError As New STSErrorPublisher
        Dim lReturn As Int32 = 0
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.AddAddressRequestType = Nothing
        Dim iCountryCodeID As Integer

        If AddAddressInput.GetType Is GetType(AgentImplementationTypes.AddAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oAddAddressOut = New AgentImplementationTypes.AddAddressResponseType
        ElseIf AddAddressInput.GetType Is GetType(BaseImplementationTypes.BaseAddAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oAddAddressOut = New BaseImplementationTypes.BaseAddAddressResponseType
        ElseIf AddAddressInput.GetType Is GetType(CustomerImplementationTypes.AddAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oAddAddressOut = New CustomerImplementationTypes.AddAddressResponseType
        ElseIf AddAddressInput.GetType Is GetType(SAMForInsuranceImplementationTypes.AddAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oAddAddressOut = New SAMForInsuranceImplementationTypes.AddAddressResponseType
        ElseIf AddAddressInput.GetType Is GetType(SAMForBrokingImplementationTypes.AddAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oAddAddressOut = New SAMForBrokingImplementationTypes.AddAddressResponseType
            samForBrokingRequest = DirectCast(AddAddressInput, SAMForBrokingImplementationTypes.AddAddressRequestType)
        ElseIf AddAddressInput.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oAddAddressOut = New SAMForInsuranceV2ImplementationTypes.AddAddressResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                If oCoreBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Agent Key validation failed", "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oAddAddressOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Key Validation", True)
                    Return oAddAddressOut
                End If
            End If
        End If

        If AddAddressInput.AddressTypeCode.ToString = "" Then
            ' AddressTypeCode not set
            STSError.AddInvalidField("AddressTypeCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(STSErrorPublisher.MandatoryInputMissing, "AddressTypeCode"), "")
        End If

        If AddAddressInput.AddressLine1 = "" Then
            ' AddressLine1 not set
            STSError.AddInvalidField("AddressLine1", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(STSErrorPublisher.MandatoryInputMissing, "AddressLine1"), "")
        End If

        If AddAddressInput.CountryCode = "" Then
            STSError.AddInvalidField("CountryCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "CountryCode"), "")
        End If

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oAddAddressOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            ' Return the errors
            Return oAddAddressOut
        End If

        ' Check the country code
        Try
            iCountryCodeID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "country", AddAddressInput.CountryCode)
        Catch ex As Exception
            STSError.AddInvalidField("CountryCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "CountryCode"), AddAddressInput.CountryCode)
        End Try

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oAddAddressOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oAddAddressOut
        End If

        ' Is the Postcode required for this Country Code ?
        If oCoreBusiness.GetPostCodeVisibility(iCountryCodeID) = InternalSAMConstants.PMPostCodeVisibilityVisible Then
            If SAMFunc.NothingToString(AddAddressInput.PostCode) = "" Then
                STSError.AddInvalidField("PostCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "PostCode"), "")
            End If
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oAddAddressOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oAddAddressOut
        End If

        ' Create the Address Object
        Dim oAdd As bSIRAddress.Business
        Try
            oAdd = New bSIRAddress.Business
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bSIRAddress.Business", ex.Message)
            STSErrorEx.SetContext(oAddAddressOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            Return oAddAddressOut
        Finally
        End Try

        SAMFunc.InitialiseSBOObject(Con:=con, oObject:=oAdd, SiriusUser:=_SiriusUser, sObjectName:="bSirAddress.business")

        With AddAddressInput

            Dim addressKey As Object = oAddAddressOut.AddressKey
            Dim addressLine1 As Object = .AddressLine1
            Dim addressLine2 As Object = .AddressLine2
            Dim addressLine3 As Object = .AddressLine3
            Dim addressLine4 As Object = .AddressLine4
            Dim sAddressLine5 As String = .AddressLine5
            Dim sAddressLine6 As String = .AddressLine6
            Dim sAddressLine7 As String = .AddressLine7
            Dim sAddressLine8 As String = .AddressLine8
            Dim sAddressLine9 As String = .AddressLine9
            Dim sAddressLine10 As String = .AddressLine10
            Dim postCode As Object = .PostCode
            Dim countryCodeId As Object = iCountryCodeID

            ' Add the record to the address controls internal collection
            lReturn = oAdd.EditAdd(lRow:=1,
                                   vAddressCnt:=addressKey,
                                   vAddress1:=addressLine1,
                                   vAddress2:=addressLine2,
                                   vAddress3:=addressLine3,
                                   vAddress4:=addressLine4,
                                   vPostalCode:=postCode,
                                   vCountryID:=countryCodeId,
                                   sAddress5:=sAddressLine5,
                                   sAddress6:=sAddressLine6,
                                   sAddress7:=sAddressLine7,
                                   sAddress8:=sAddressLine8,
                                   sAddress9:=sAddressLine9,
                                   sAddress10:=sAddressLine10)

            oAddAddressOut.AddressKey = Cast.ToInt32(addressKey, 0)
            .AddressLine1 = Cast.ToString(addressLine1, String.Empty)
            .AddressLine2 = Cast.ToString(addressLine2, String.Empty)
            .AddressLine3 = Cast.ToString(addressLine3, String.Empty)
            .AddressLine4 = Cast.ToString(addressLine4, String.Empty)
            .PostCode = Cast.ToString(postCode, String.Empty)
            iCountryCodeID = Cast.ToInt32(countryCodeId, 0)
            .AddressLine5 = Cast.ToString(sAddressLine5, String.Empty)
            .AddressLine6 = Cast.ToString(sAddressLine6, String.Empty)
            .AddressLine7 = Cast.ToString(sAddressLine7, String.Empty)
            .AddressLine8 = Cast.ToString(sAddressLine8, String.Empty)
            .AddressLine9 = Cast.ToString(sAddressLine9, String.Empty)
            .AddressLine10 = Cast.ToString(sAddressLine10, String.Empty)



            '' Add the record to the address controls internal collection
            'lReturn = oAdd.EditAdd(lRow:=1, _
            '                       vAddressCnt:=oAddAddressOut.AddressKey, _
            '                       vAddress1:=.AddressLine1, _
            '                       vAddress2:=.AddressLine2, _
            '                       vAddress3:=.AddressLine3, _
            '                       vAddress4:=.AddressLine4, _
            '                       vPostalCode:=.PostCode, _
            '                       vCountryID:=iCountryCodeID)

            If (lReturn <> PMEReturnCode.PMTrue) Then
                If oAdd IsNot Nothing Then
                    oAdd.Dispose()
                    oAdd = Nothing
                End If
                Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.BackofficeFailed, "Failed to EditAdd new address record. Return Code: " & lReturn, "")
                STSErrorEx.SetContext(oAddAddressOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                Return oAddAddressOut
            End If

        End With

        ' Update the record
        Try

            lReturn = oAdd.Update()

            If (lReturn <> PMEReturnCode.PMTrue) Then
                If oAdd IsNot Nothing Then
                    oAdd.Dispose()
                    oAdd = Nothing
                End If
                Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.BackofficeFailed, "Failed to update address record. Return Code: " & lReturn, "")
                STSErrorEx.SetContext(oAddAddressOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                Return oAddAddressOut
            End If

        Catch oe As Exception
            If oAdd IsNot Nothing Then
                oAdd.Dispose()
                oAdd = Nothing
            End If
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.BackofficeFailed, "Failed to update address record.", "")
            STSErrorEx.SetContext(oAddAddressOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            Return oAddAddressOut
        End Try

        ' Get the new address_cnt
        Try
            oAddAddressOut.AddressKey = oAdd.AddressCnt
        Catch oe As Exception
            If oAdd IsNot Nothing Then
                oAdd.Dispose()
                oAdd = Nothing
            End If
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.BackofficeFailed, "Failed to get address_cnt", "")
            STSErrorEx.SetContext(oAddAddressOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            Return oAddAddressOut

        End Try

        If oAdd IsNot Nothing Then
            oAdd.Dispose()
            oAdd = Nothing
        End If

        'TypeCast the response Back.
        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            Dim oResponse As New AgentImplementationTypes.AddAddressResponseType
            oResponse = DirectCast(oAddAddressOut, AgentImplementationTypes.AddAddressResponseType)
            oAddAddressOut = oResponse
        End If

        Return oAddAddressOut
    End Function

    Public Function GetAddress(ByVal GetAddressInput As BaseGetAddressRequestType) As BaseGetAddressResponseType

        Const ACMethodName As String = "GetAddress"

        Dim oGetAddressOut As New BaseImplementationTypes.BaseGetAddressResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oErrors As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.GetAddressRequestType = Nothing

        If GetAddressInput.GetType Is GetType(AgentImplementationTypes.GetAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oGetAddressOut = New AgentImplementationTypes.GetAddressResponseType
        ElseIf GetAddressInput.GetType Is GetType(BaseImplementationTypes.BaseGetAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oGetAddressOut = New BaseImplementationTypes.BaseGetAddressResponseType
        ElseIf GetAddressInput.GetType Is GetType(CustomerImplementationTypes.GetAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oGetAddressOut = New CustomerImplementationTypes.GetAddressResponseType
        ElseIf GetAddressInput.GetType Is GetType(SAMForInsuranceImplementationTypes.GetAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetAddressOut = New SAMForInsuranceImplementationTypes.GetAddressResponseType
        ElseIf GetAddressInput.GetType Is GetType(SAMForBrokingImplementationTypes.GetAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oGetAddressOut = New SAMForBrokingImplementationTypes.GetAddressResponseType
            samForBrokingRequest = DirectCast(GetAddressInput, SAMForBrokingImplementationTypes.GetAddressRequestType)
        ElseIf GetAddressInput.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetAddressRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetAddressOut = New SAMForInsuranceV2ImplementationTypes.GetAddressResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                If oCoreBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                         SAMInvalidData.MandatoryInputMissing.ToString,
                                                         "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                End If
            End If
        End If
        ' Check for any errors
        oErrors.CheckForErrors()
        If GetAddressInput.AddressKey = 0 And GetAddressInput.PartyKey = 0 Then
            ' AddressKey not set
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                  SAMInvalidData.MandatoryInputMissing.ToString,
                                                  "AddressKey or PartyKey")
        End If
        oErrors.CheckForErrors()



        Dim ds As DataSet = Nothing

        If GetAddressInput.AddressKey = 0 Then
            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_party_corrospondance_address_cnt")
                    cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = GetAddressInput.PartyKey
                    ds = con.ExecuteDataSet(cmd, "ds")
                End Using
            End Using
            If ds.Tables.Count = 1 Then
                GetAddressInput.AddressKey = Cast.ToInt32(ds.Tables(0).Rows(0).Item("address_cnt"), 0)
            End If
        End If
        ' BSJ April 09 - SQL Mixed Mode Compliance
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Address_sel")
                cmd.AddInParameter("@address_cnt", SqlDbType.Int).Value = GetAddressInput.AddressKey

                ds = con.ExecuteDataSet(cmd, "ds")
            End Using
        End Using

        If ds.Tables.Count = 1 Then
            Try
                Dim oAddress As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseAddressType
                Dim sCountryCode As String = String.Empty
                With oAddress
                    .AddressLine1 = ds.Tables(0).Rows(0).Item("address1").ToString
                    .AddressLine2 = ds.Tables(0).Rows(0).Item("address2").ToString
                    .AddressLine3 = ds.Tables(0).Rows(0).Item("address3").ToString
                    .AddressLine4 = ds.Tables(0).Rows(0).Item("address4").ToString
                    .AddressLine6 = ds.Tables(0).Rows(0).Item("address6").ToString
                    .AddressLine7 = ds.Tables(0).Rows(0).Item("address7").ToString
                    .AddressLine8 = ds.Tables(0).Rows(0).Item("address8").ToString
                    .AddressLine9 = ds.Tables(0).Rows(0).Item("address9").ToString
                    .AddressLine10 = ds.Tables(0).Rows(0).Item("address10").ToString
                    .AddressLine5 = ds.Tables(0).Rows(0).Item("address5").ToString


                    If ds.Tables(0).Rows(0).Item("country_id").ToString <> "0" Then
                        '.CountryCode
                        .CountryCode = GetListCodeFromID(Core.STSListType.PMLookup, "country", Cast.ToInt32(ds.Tables(0).Rows(0).Item("country_id"), 0))
                    End If
                    .PostCode = ds.Tables(0).Rows(0).Item("postal_code").ToString
                End With
                oGetAddressOut.Address = oAddress
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher("Failed to Get Address.", ex)
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetRisk", True)
            End Try
        End If

        'TypeCast the response Back.
        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            Dim oResponse As New AgentImplementationTypes.GetAddressResponseType
            oResponse = DirectCast(oGetAddressOut, AgentImplementationTypes.GetAddressResponseType)
            oGetAddressOut = oResponse
        End If

        Return oGetAddressOut
    End Function

    Public Function GetListCodeFromID( _
                        ByVal ListType As Core.STSListType, _
                        ByVal ListCode As String, _
                        ByVal ListItemID As Int32) As String

        Const ACMethodName As String = "GetListCodeFromID"

        ' Data View that will hold the returned results		
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)

        Dim oIn As New Core.GetListInput
        Dim oOut As Core.GetListOutput
        Dim sCode As String = String.Empty

        oIn.BranchCode = "BRANCH1"
        oIn.ListType = ListType
        oIn.ListCode = ListCode

        ' Get the List
        oOut = oCoreBusiness.GetList(oIn, True)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If oOut.ListItems Is Nothing Then
            Dim ex As New Exception("No Effective List retrieved for List : " + ListCode.ToString)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Get the DataView For the table
        dv = oOut.ListItems.Tables(0).DefaultView

        ' Specify the Filter
        sFilter = "id = " & ListItemID

        Try
            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

            If dvID Is Nothing Then
                Dim STSError As New STSErrorPublisher(PMEReturnCode.PMNotFound, "No row found for STSList : " + ListCode.ToString + " ID : " + ListItemID.ToString)
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "DataView", True)
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " ID : " + ListItemID.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Try
            ' why return an ID, when an ID has been passed ???
            'If ListType = Core.STSListType.GisList Then
            ' Return the code 
            sCode = dvID.Item(0).Item("code").ToString
            'Else
            '    ' Return the ID (Its always the first column)
            '    sCode = dvID.Item(0).Item(0)

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " Code : " + ListItemID.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Return sCode

    End Function

    ' Vivek: Added this method to get the code(specified by ReturnColumnName)
    ' based on given column (ListColumnName) and it's value (ListItemID)
    ' from table ListCode
    Public Function GetListCode(Of ListItemDataType)( _
                        ByVal ListType As Core.STSListType, _
                        ByVal ListCode As String, _
                        ByVal ListItemID As ListItemDataType, _
                        Optional ByVal ListColumnName As String = "id", _
                        Optional ByVal EncloseInQuotes As Boolean = False, _
                        Optional ByVal ReturnColumnName As String = "code") As Object

        Const ACMethodName As String = "GetListCodeFromID"

        ' Data View that will hold the returned results		
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)

        Dim oIn As New Core.GetListInput
        Dim oOut As Core.GetListOutput
        Dim sCode As Object = Nothing

        oIn.BranchCode = "BRANCH1"
        oIn.ListType = ListType
        oIn.ListCode = ListCode

        ' Get the List
        oOut = oCoreBusiness.GetList(oIn, True)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If oOut.ListItems Is Nothing Then
            Dim ex As New Exception("No Effective List retrieved for List : " + ListCode.ToString)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Get the DataView For the table
        dv = oOut.ListItems.Tables(0).DefaultView

        ' Specify the Filter
        If EncloseInQuotes Then
            sFilter = ListColumnName & " = '" & Cast.ToString(ListItemID, String.Empty) & "'"
        Else
            sFilter = ListColumnName & " = " & Cast.ToString(ListItemID, String.Empty)
        End If

        Try
            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

            If dvID Is Nothing Then
                Dim STSError As New STSErrorPublisher(PMEReturnCode.PMNotFound, "No row found for STSList : " + ListCode.ToString + " ID : " + ListItemID.ToString)
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "DataView", True)
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " ID : " + ListItemID.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Try
            ' why return an ID, when an ID has been passed ???
            'If ListType = Core.STSListType.GisList Then
            ' Return the code 
            sCode = dvID.Item(0).Item(ReturnColumnName)
            'Else
            '    ' Return the ID (Its always the first column)
            '    sCode = dvID.Item(0).Item(0)

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " Code : " + ListItemID.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Return sCode

    End Function

End Class

