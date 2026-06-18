Option Strict On

Imports SiriusFS.SAM.Structure.STSErrorPublisher
'Imports Siriusfs.SAM.ServiceAgent.InternalSAMConstants
Imports SiriusFS.SAM.Structure
'Imports Siriusfs.SAM.ServiceAgent.SAMFunc
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports Microsoft.ApplicationBlocks.Data
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SSP.Shared
Partial Public Class CoreSAMBusiness

    ' ***************************************************************** '
    ' Name: DeleteRisk
    '
    ' Description: This method accepts the base implementation type 
    '              and deletes the risk record.
    '
    ' ***************************************************************** '
    Public Function DeleteRisk(ByVal DeleteRiskRequest As BaseDeleteRiskRequestType) As BaseDeleteRiskResponseType

        Dim oResponse As BaseDeleteRiskResponseType
        Dim oAgentRequest As AgentImplementationTypes.DeleteRiskRequestType = Nothing
        Dim oCoreBusiness As New CoreBusiness
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iSourceID As Int32
        Dim oDeleteRiskIn As New DeleteRiskIn
        Dim oDeleteRiskOut As DeleteRiskOut

        Const ACMethodName As String = "DeleteRisk"

        Dim STSError As New STSErrorPublisher

        If DeleteRiskRequest.GetType Is GetType(AnonymousImplementationTypes.DeleteRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AnonymousPackage
            oResponse = New AnonymousImplementationTypes.DeleteRiskResponseType
        ElseIf DeleteRiskRequest.GetType Is GetType(AgentImplementationTypes.DeleteRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.DeleteRiskResponseType
            oAgentRequest = DirectCast(DeleteRiskRequest, AgentImplementationTypes.DeleteRiskRequestType)
        ElseIf DeleteRiskRequest.GetType Is GetType(BaseImplementationTypes.BaseDeleteRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oResponse = New BaseImplementationTypes.BaseDeleteRiskResponseType
        ElseIf DeleteRiskRequest.GetType Is GetType(CustomerImplementationTypes.DeleteRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oResponse = New CustomerImplementationTypes.DeleteRiskResponseType
        ElseIf DeleteRiskRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.DeleteRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.DeleteRiskResponseType
        ElseIf DeleteRiskRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.DeleteRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.DeleteRiskResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        'Check Mandatory Fields
        If DeleteRiskRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If DeleteRiskRequest.InsuranceFolderKey = 0 Then
            STSError.AddInvalidField("InsuranceFolderKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFolderKey"), "")
        End If

        If DeleteRiskRequest.InsuranceFileKey = 0 Then
            STSError.AddInvalidField("InsuranceFileKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFileKey"), "")
        End If

        If DeleteRiskRequest.RiskKey = 0 Then
            STSError.AddInvalidField("RiskKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "RiskKey"), "")
        End If

        If DeleteRiskRequest.QuoteTimeStamp Is Nothing Then
            STSError.AddInvalidField("QuoteTimeStamp", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "QuoteTimeStamp"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        iSourceID% = 1
        ' Convert branch code to ID
        Try
            iSourceID% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", DeleteRiskRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(MandatoryInputInvalid, "BranchCode"), DeleteRiskRequest.BranchCode)
        End Try

        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oResponse
        End If

        ' Check the insurance folder cnt is valid
        If oCoreBusiness.CheckInsuranceFolder(DeleteRiskRequest.InsuranceFolderKey) = False Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyRecordNotFound, "Insurance Folder validation failed", "The Insurance Folder record does not exist for key: " & DeleteRiskRequest.InsuranceFolderKey)
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If
        ' Check the insurance file cnt is valid
        Dim iFileFolderCnt As Integer
        If oCoreBusiness.CheckInsuranceFile(DeleteRiskRequest.InsuranceFileKey, iFileFolderCnt) = False Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyRecordNotFound, "Insurance File validation failed", "The Insurance File record does not exist for key: " & DeleteRiskRequest.InsuranceFileKey)
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If
        If iFileFolderCnt <> DeleteRiskRequest.InsuranceFolderKey Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyMismatch, "Insurance File validation failed", "The Insurance File's Folder does not match the passed InsuranceFolder")
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If
        ' Check the risk cnt is valid
        If oCoreBusiness.CheckRisk(DeleteRiskRequest.RiskKey) = False Then
            STSError = New STSErrorPublisher(STSErrorCodes.RiskRecordNotFound, "Risk validation failed", "The Risk record does not exist for key: " & DeleteRiskRequest.RiskKey)
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If
        If oCoreBusiness.CheckPolicyRiskLink(DeleteRiskRequest.InsuranceFileKey, DeleteRiskRequest.RiskKey) = False Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyRiskLinkRecordNotFound, "Risk validation failed", "The Risk (" & DeleteRiskRequest.RiskKey & ") is not linked to the Insurance File key: " & DeleteRiskRequest.InsuranceFileKey)
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If

        ' Security check
        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            If (oCoreBusiness.AgentSecurityCheck(oAgentRequest.UserName, oAgentRequest.InsuranceFileKey, PMEEntityType.InsuranceFile) = False) Then
                STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security check failed", oAgentRequest.UserName & " does not have permission to access policy " & oAgentRequest.InsuranceFileKey)
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
                Return oResponse
            End If
        End If

        ' Check that the timestamp matches and lock the Quote
        Dim AnyError As STSErrorType
        AnyError = oCoreBusiness.CheckTSAndLock( _
            BranchCode:=DeleteRiskRequest.BranchCode, _
            Lockname:=CoreBusiness.LockName.InsuranceFolderCnt, _
            LockValue:=DeleteRiskRequest.InsuranceFolderKey, _
            TStamp:=DeleteRiskRequest.QuoteTimeStamp)
        ' Check for Errors
        If AnyError Is Nothing = False Then
            ' Either the timestamp didn't match or the record couldn't be locked for some reason, return the error.
            oResponse.STSError = AnyError
            Return oResponse
        End If

        oDeleteRiskIn.RiskCnt = DeleteRiskRequest.RiskKey
        oDeleteRiskIn.InsuranceFileCnt = DeleteRiskRequest.InsuranceFileKey
        oDeleteRiskIn.InsuranceFolderCnt = DeleteRiskRequest.InsuranceFolderKey
        oDeleteRiskIn.TransactionType = DeleteRiskRequest.TransactionType.ToString
        oDeleteRiskIn.nOrignalRiskKey = DeleteRiskRequest.OrignalRiskKey
        Try
            Dim iIsNew As Integer

            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_check_is_new_risk")
                    cmd.AddInParameter("@nRisk_cnt", SqlDbType.Int).Value = DeleteRiskRequest.RiskKey
                    cmd.AddOutParameter("@is_new", SqlDbType.Int)
                    con.ExecuteNonQuery(cmd)
                    iIsNew = Cast.ToInt16(cmd.Parameters("@is_new").Value, 0)
                End Using
            End Using
            If DeleteRiskRequest.TransactionType = TransactionType.NB Or iIsNew = 1 Then
                oDeleteRiskOut = oCoreBusiness.DeleteRiskOnAdd(oDeleteRiskIn)
            Else
                oDeleteRiskOut = oCoreBusiness.DeleteRisk(oDeleteRiskIn)
            End If
        Catch ex As ApplicationException
            Dim STSErrorEx As STSErrorPublisher = New STSErrorPublisher(STSErrorCodes.FailedToAddRiskRecord, "Failed to delete risk record", "Failed to delete risk record")
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "SiriusFS.STS." & ACMethodName, True)
        End Try
        If DeleteRiskRequest.RiskKey > 0 And DeleteRiskRequest.InsuranceFileKey > 0 Then
            Dim sRiskDescription As String = String.Empty
            Dim iVariationNumber As Int32 = 0
            Dim iRiskCnt As Int32 = 0
            Dim dsResult As DataSet = Nothing
            Dim iCommandReturn As Int32
            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Get_Mandatory_Risk")
                    cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = DeleteRiskRequest.RiskKey
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = DeleteRiskRequest.InsuranceFileKey
                    dsResult = con.ExecuteDataSet(cmd, "GetMandatoryRisk")
                End Using


                If dsResult IsNot Nothing Then
                    If dsResult.Tables.Count > 0 Then
                        If dsResult.Tables(0).Rows.Count > 0 Then
                            iRiskCnt = Cast.ToInt32(dsResult.Tables(0).Rows(0).Item("risk_cnt"), 0)
                            sRiskDescription = Cast.ToString(dsResult.Tables(0).Rows(0).Item("description"), "")
                            iVariationNumber = Cast.ToInt32(dsResult.Tables(0).Rows(0).Item("variation_number"), 0)
                        End If
                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Update_Mandatory_Risk_Details")
                            cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = iRiskCnt
                            cmd.AddInParameter("@risk_status_id", SqlDbType.Int).Value = 4 ' For Status to Unquote Risk
                            cmd.AddInParameter("@description", SqlDbType.Text).Value = sRiskDescription
                            cmd.AddInParameter("@variation_number", SqlDbType.Int).Value = iVariationNumber
                            iCommandReturn = con.ExecuteNonQuery(cmd)
                        End Using
                    End If
                End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Unquote_Risk_Forard")
                    cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = DeleteRiskRequest.RiskKey
                    iCommandReturn = con.ExecuteNonQuery(cmd)
                End Using
            End Using
        End If
        ' Unlock and return the new timestamp
        AnyError = oCoreBusiness.UnlockAndGetTS( _
            BranchCode:=DeleteRiskRequest.BranchCode, _
            Lockname:=CoreBusiness.LockName.InsuranceFolderCnt, _
            LockValue:=DeleteRiskRequest.InsuranceFolderKey, _
            TStamp:=oResponse.QuoteTimeStamp)
        ' Check for Errors
        If AnyError Is Nothing = False Then
            ' Unable to unlock, return the error.
            oResponse.STSError = AnyError
            Return oResponse
        End If

        If nTypeOfPackage = enumTypeOfPackage.AnonymousPackage Then
            Dim oResponseAnon As New AnonymousImplementationTypes.DeleteRiskResponseType
            oResponseAnon = DirectCast(oResponse, AnonymousImplementationTypes.DeleteRiskResponseType)
            oResponse = oResponseAnon
        ElseIf nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            Dim oResponseAgent As New AgentImplementationTypes.DeleteRiskResponseType
            oResponseAgent = DirectCast(oResponse, AgentImplementationTypes.DeleteRiskResponseType)
            oResponse = oResponseAgent
        End If

        Return oResponse

    End Function

End Class

