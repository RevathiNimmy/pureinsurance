Option Strict On

Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.Structure
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports Microsoft.ApplicationBlocks.Data
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports System.Text.RegularExpressions
Imports SiriusFS.SAM.Structure.SAMConstants
Imports dPMDAOBridge
Imports System.Xml
Imports System.IO
Imports SSP.Shared.gPMConstants
Imports SSP.Shared

Partial Public Class CoreSAMBusiness


    Dim sAutoPlanRef As String
    Private _oCache As System.Web.Caching.Cache

    ' ****************************************************************
    ' Name: AddMtaQuote
    '
    ' Description: 
    ' ****************************************************************
    Public Function AddMtaQuote(ByVal AddMtaQuoteRequest As BaseAddMtaQuoteRequestType) As BaseAddMtaQuoteResponseType

        Dim oBusiness As New CoreBusiness
        Dim oResponse As BaseAddMtaQuoteResponseType = Nothing

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

        ' validate the data provided in the structure
        AddMtaQuoteRequest.Validate(CType(oSAMErrorCollection, Object))

        ' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()

        ' validate the data provided in the structure
        AddMtaQuoteValidateData(oBusiness, AddMtaQuoteRequest, oSAMErrorCollection)

        '' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            If AddMtaQuoteRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.AddMtaQuoteRequestType) Then
                Dim samForInsuranceRequest As SAMForInsuranceImplementationTypes.AddMtaQuoteRequestType = DirectCast(AddMtaQuoteRequest, SAMForInsuranceImplementationTypes.AddMtaQuoteRequestType)
                oResponse = AddMtaQuoteUnderwriting(con, samForInsuranceRequest)
            ElseIf AddMtaQuoteRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddMtaQuoteRequestType) Then
                Dim samForInsuranceV2Request As SAMForInsuranceV2ImplementationTypes.AddMtaQuoteRequestType = DirectCast(AddMtaQuoteRequest, SAMForInsuranceV2ImplementationTypes.AddMtaQuoteRequestType)
                oResponse = AddMtaQuoteUnderwriting(con, samForInsuranceV2Request)
            End If

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''  validates all the data in the request that can be validated
    ''' </summary>
    ''' <param name="oBusiness"></param>
    ''' <param name="OpenClaimRequest"></param>
    ''' <param name="oSAMErrorCollection"></param>
    ''' <remarks></remarks>
    Private Sub AddMtaQuoteValidateData(
                ByVal oBusiness As CoreBusiness,
                ByRef AddMtaQuoteRequest As BaseAddMtaQuoteRequestType,
                ByRef oSAMErrorCollection As SAMErrorCollection)

        ' Validate standard lookup data
        AddMtaQuoteValidateDataStandardLookup(oBusiness, AddMtaQuoteRequest, oSAMErrorCollection)

        ' validate other data
        AddMtaQuoteValidateDataOther(oBusiness, AddMtaQuoteRequest, oSAMErrorCollection)

    End Sub

    ''' <summary>
    ''' validates the standard lookup data 
    ''' </summary>
    ''' <param name="oBusiness"></param>
    ''' <param name="OpenClaimRequest"></param>
    ''' <param name="oSAMErrorCollection"></param>
    ''' <remarks></remarks>
    Private Sub AddMtaQuoteValidateDataStandardLookup(
                ByVal oBusiness As CoreBusiness,
                ByRef AddMtaQuoteRequest As BaseAddMtaQuoteRequestType,
                ByRef oSAMErrorCollection As SAMErrorCollection)

        Dim sMTAReasonDescription As String = AddMtaQuoteRequest.mtaReasonDescription
        ' Mandatory Fields in the Structure--------------------------------------
        ' branch code
        AddMtaQuoteRequest.SourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, AddMtaQuoteRequest.BranchCode, "BranchCode", oSAMErrorCollection)
        AddMtaQuoteRequest.mtaReasonID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.MTAEventDescription, AddMtaQuoteRequest.MtaReason, "code", oSAMErrorCollection, AddMtaQuoteRequest.mtaReasonDescription)

        If String.IsNullOrEmpty(sMTAReasonDescription) = False Then
            AddMtaQuoteRequest.mtaReasonDescription = AddMtaQuoteRequest.mtaReasonDescription + " - " + sMTAReasonDescription
        End If

        ' Check if the LapsedReasonCode was passed in
        If String.IsNullOrEmpty(AddMtaQuoteRequest.LapsedReasonCode) = False Then
            AddMtaQuoteRequest.LapsedReasonID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.LapsedReason, AddMtaQuoteRequest.LapsedReasonCode, "code", oSAMErrorCollection)
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBusiness"></param>
    ''' <param name="AddMtaQuoteRequest"></param>
    ''' <param name="oSAMErrorCollection"></param>
    ''' <remarks></remarks>
    Private Sub AddMtaQuoteValidateDataOther(ByVal oBusiness As CoreBusiness,
                ByRef AddMtaQuoteRequest As BaseAddMtaQuoteRequestType,
                ByRef oSAMErrorCollection As SAMErrorCollection)

        If (AddMtaQuoteRequest.TypeOfMta.ToUpper = "TEMPORARY") Then
            If AddMtaQuoteRequest.EffectiveDate > AddMtaQuoteRequest.ExpiryDate Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.CoverEndDateIsBeforeCoverStartDate,
                                                                       SAMInvalidData.CoverEndDateIsBeforeCoverStartDate.ToString,
                                                                       "ExpiryDate",
                                                                        AddMtaQuoteRequest.ExpiryDate.ToString)
            End If
        End If

        If (AddMtaQuoteRequest.TypeOfMta.ToUpper <> "TEMPORARY") And (AddMtaQuoteRequest.TypeOfMta.ToUpper <> "PERMANENT") Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                                   SAMInvalidData.InvalidLookupListValue.ToString,
                                                                   "TypeOfMta",
                                                                    AddMtaQuoteRequest.TypeOfMta)
        End If

    End Sub
    ''' <summary>
    ''' AddMtaQuoteUnderwriting
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="AddMtaQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks>Function for adding MTA Quote</remarks>
    Private Function AddMtaQuoteUnderwriting(ByVal con As SiriusConnection, ByVal AddMtaQuoteRequest As SAMForInsuranceImplementationTypes.AddMtaQuoteRequestType) As SAMForInsuranceImplementationTypes.AddMtaQuoteResponseType

        Dim response As New SAMForInsuranceImplementationTypes.AddMtaQuoteResponseType
        Dim business As New CoreBusiness(_SiriusUser)
        Dim ret As Integer = 0
        Dim errorCode As Integer = 0
        Dim mtaEndDate As DateTime = AddMtaQuoteRequest.ExpiryDate

        Dim siriusOprionRI2007 As Boolean
        siriusOprionRI2007 = Sirius.Architecture.Configuration.Database.SiriusHiddenOptionsTableAccess.GetValueAsBoolean(AddMtaQuoteRequest.SourceId, SIRHiddenOptions.SIROPTEnableRI2007, False)

        Dim bIsBackdateMTA As Boolean = False
        Dim permanentMTA As Boolean = (AddMtaQuoteRequest.TypeOfMta.ToUpper = "PERMANENT")

        'Pass in whether we're doing a permanent MTA or not
        'This is only relevant in the business object if we're MTAing an Underwriting policy
        If permanentMTA Then
            errorCode = 1
        Else
            errorCode = 0
        End If

        ' Retrieve the insurance file details.
        Dim insuranceFolderCnt As Integer = 0
        Dim insuranceRef As String = String.Empty
        Dim insuranceFileDetails As New BaseClaimType
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim sOptionValue As String = String.Empty
        insuranceFileDetails.InsuranceFileKey = AddMtaQuoteRequest.InsuranceFileKey
        GetInsuranceFileDetails(con, insuranceFileDetails)

        insuranceFolderCnt = insuranceFileDetails.InsuranceFolderCnt
        insuranceRef = insuranceFileDetails.InsuranceRef.Trim

        Dim findInsurance As New bSIRFindInsurance.Form

        SAMFunc.InitialiseSBOObject(con, findInsurance, _SiriusUser, AddMtaQuoteRequest.BranchCode, "bSIRFindInsurance.Form")

        Dim policyVersion As Integer = 0
        Dim samErrors As SAMErrorCollection = New SAMErrorCollection

        ' Get User MTA Authority
        Dim oUserMTAAuthority(,) As Object
        Const MTA_AUTHORITY_WITH_CLAIMS As Integer = 3
        Const MTA_AUTHORITY_WITHOUT_CLAIMS As Integer = 2
        Const MTA_AUTHORITY_NOT_ALLOWED As Integer = 1

        ret = findInsurance.GetOutOfSequenceMTAUserAuthority(oUserMTAAuthority)
        If (ret <> PMEReturnCode.PMTrue) Then
            samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                        "bSIRFindInsurance.Form.GetOutOfSequenceMTAUserAuthority returned an error code of " & ret & ".  Check the Sirius log for more information")
            samErrors.CheckForErrors()
        End If

        ret = findInsurance.GetVersionByDate(
            r_lInsuranceFileCnt:=AddMtaQuoteRequest.InsuranceFileKey,
            v_dtStartDate:=AddMtaQuoteRequest.EffectiveDate,
            r_lPolicyVersion:=policyVersion,
            r_lErrorCode:=errorCode)

        If (ret <> PMEReturnCode.PMTrue) Then
            samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                        "bSIRFindInsurance.Form.GetVersionByDate returned an error code of " & ret & ".  Check the Sirius log for more information")
            samErrors.CheckForErrors()
        End If

        If Not permanentMTA Then
            oCoreBusiness.GetSystemOption(AddMtaQuoteRequest.BranchCode, SystemOption.DisableTempMTAs, sOptionValue)
            If sOptionValue = "1" Then
                samErrors.AddBusinessRule(SAMBusinessErrors.TemporaryMTAsAreNotAllowedOnThisSystem,
                             "Temporary MTAs Are Not Allowed On This System")
                samErrors.CheckForErrors()
            End If
        End If

        If (errorCode <> 0) Then
            Select Case errorCode
                Case 1
                    samErrors.AddBusinessRule(SAMBusinessErrors.FutureAdjustmentFound,
                            "Future Adjustment Found")
                Case 2
                    samErrors.AddBusinessRule(SAMBusinessErrors.OverlapsWithTemporaryMTA,
                            "Overlaps with temporary MTA")
                Case 3
                    samErrors.AddBusinessRule(SAMBusinessErrors.NoPolicyVersionFoundForThisMTAEffectiveDate,
                            "No policy version found for this MTA effective date")
                Case 4
                    samErrors.AddBusinessRule(SAMBusinessErrors.ThisPolicyVersionIsAttachedToAClosedBranchWhichDoesntAllowPermanentMTAs,
                            "This policy version is attached to a closed branch which doesn't allow permanent MTAs.")
                Case 5
                    samErrors.AddBusinessRule(SAMBusinessErrors.ThisPolicyVersionIsAttachedToAClosedBranchWhichDoesntAllowTemporaryMTAs,
                            "This policy version is attached to a closed branch which doesn't allow temporary MTAs.")
                Case 6
                    samErrors.AddBusinessRule(SAMBusinessErrors.MTAEffectiveDateIsBeforeOriginalCoverFromDateOfPolicy,
                            "S4I does not support Out of Sequence MTAs prior to the Inception Date of the Policy. Please contact your System Administrator.")
                Case 7
                    samErrors.AddBusinessRule(SAMBusinessErrors.BackdatedMTAIsNotPermittedPriorToTheLastPolicyRenewalDate,
                            "This Policy's product is not configured to allow an Out of Sequence MTA or Cancellation before the last Renewal or Inception Date. Please contact your System Administrator.")
                Case 9
                    samErrors.AddBusinessRule(SAMBusinessErrors.BackdatedMTANotPermitted,
                            "This Policy's product is not configured to allow an Out of Sequence MTA or Cancellation. Please contact your System Administrator.")
                Case 10
                    samErrors.AddBusinessRule(SAMBusinessErrors.BackdatedMTAIsNotPermittedPriorToTheLastPolicyRenewalDatePlusOne,
                            "This Policy's product is not configured to allow an Out of Sequence MTA or Cancellation before the last Renewal Period + 1 or Inception Date. Please contact your System Administrator.")
            End Select

            If errorCode > 6 Then
                If CInt(oUserMTAAuthority(0, 0)) = MTA_AUTHORITY_NOT_ALLOWED Then
                    samErrors.AddBusinessRule(SAMBusinessErrors.BackdatedMTANotPermitted,
                            "You are not permitted to process an Out of Sequence MTA or Cancellation. Please contact your System Administrator.")
                End If
            End If
            If errorCode <> 8 Then
                ' raise a business rule error if one exists
                samErrors.CheckForErrors()
            ElseIf errorCode = 8 And Not siriusOprionRI2007 Then 'Backdated MTA
                bIsBackdateMTA = True

                Dim claimStatus As Integer = 0

                ret = findInsurance.CheckInClaim(
                    v_sInsuranceRef:=insuranceRef,
                    r_lClaimStatus:=claimStatus,
                    v_dtStartDate:=AddMtaQuoteRequest.EffectiveDate)

                If (ret <> PMEReturnCode.PMTrue) Then
                    samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                "bSIRFindInsurance.Form.CheckInClaim returned an error code of " & ret & ".  Check the Sirius log for more information")
                    samErrors.CheckForErrors()
                End If

                If claimStatus <> -1 Then
                    If CInt(oUserMTAAuthority(0, 0)) = MTA_AUTHORITY_WITHOUT_CLAIMS Then
                        samErrors.AddBusinessRule(SAMBusinessErrors.BackdatedMTANotPermittedWithClaimsForUser,
                                                    "You are not permitted to process an Out of Sequence MTA or Cancellation where there has been" &
                                                    "a claim on this policy after the effective date entered. Please contact your System Administrator.")
                    ElseIf CInt(oUserMTAAuthority(0, 0)) = MTA_AUTHORITY_WITH_CLAIMS Then
                        samErrors.AddBusinessRule(SAMBusinessErrors.ClaimHasBeenLodgedAfterTheEffectiveDateOfThisMTA,
                                                    "A claim has been lodged after the effective date of this MTA.")
                    End If
                    samErrors.CheckForErrors()
                End If
            ElseIf errorCode = 8 Then 'Allow Backdated MTA
                bIsBackdateMTA = True
            End If
        End If

        Dim renewalStatus As Integer = 0

        ret = findInsurance.CheckInRenewal(
            v_lInsuranceFileCnt:=AddMtaQuoteRequest.InsuranceFileKey,
            r_lRenewalStatus:=renewalStatus)

        If (ret <> PMEReturnCode.PMTrue) Then
            samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                            "bSIRFindInsurance.Form.CheckInRenewal returned an error code of " & ret & ".  Check the Sirius log for more information")
            samErrors.CheckForErrors()
        End If

        Try
            con.BeginTransaction()

            If (renewalStatus <> -1) Then
                'If policy is under renewal then delete Renewal Version
                DeleteRenewal(con, AddMtaQuoteRequest.InsuranceFileKey, AddMtaQuoteRequest.BranchCode, insuranceRef, insuranceFileDetails.InsuredCnt)

            End If

            If bIsBackdateMTA Then
                Dim baseInsuranceFileCnt As Integer = 0
                'get backdatebaseinsurancefilecount
                ret = findInsurance.GetBasePolicyCntForBackDateMTA(v_lInsuranceFolderCnt:=insuranceFolderCnt,
                                                                    v_dtMTADate:=AddMtaQuoteRequest.EffectiveDate,
                                                                    lBaseInsuranceFileCnt:=baseInsuranceFileCnt)

                If (ret <> PMEReturnCode.PMTrue) Then
                    samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                            "bSIRFindInsurance.Form.GetBasePolicyCntForBackDateMTA returned an error code of " & ret & ".  Check the Sirius log for more information")
                    samErrors.CheckForErrors()
                End If

                'In case of BackDated MTA update the InsFileCnt with actual base cnt
                If baseInsuranceFileCnt <> -1 Then
                    AddMtaQuoteRequest.InsuranceFileKey = baseInsuranceFileCnt
                End If

                Dim tempMTAEndDate As Object = Nothing
                ret = findInsurance.GetCoverEndDate(v_lInsuranceFolderCnt:=insuranceFolderCnt,
                                v_dtMTADate:=AddMtaQuoteRequest.EffectiveDate,
                                lBaseInsuranceFileCnt:=baseInsuranceFileCnt,
                                DtMTAEndDate:=tempMTAEndDate)
                If (ret <> PMEReturnCode.PMTrue) Then
                    samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                            "bSIRFindInsurance.Form.GetCoverFromDate returned an error code of " & ret & ".  Check the Sirius log for more information")
                    samErrors.CheckForErrors()
                End If

                If IsDate(tempMTAEndDate) Then
                    mtaEndDate = Cast.ToDateTime(CDate(tempMTAEndDate), DateTime.MinValue)
                End If

            End If

            Dim newInsuranceFileCnt As Integer = 0

            ret = findInsurance.CopyPolicy(
            v_lOldInsuranceFileCnt:=AddMtaQuoteRequest.InsuranceFileKey,
            r_lNewInsuranceFileCnt:=newInsuranceFileCnt,
            v_lVersion:=policyVersion,
            v_bPermanentMTA:=permanentMTA,
            v_dtMTADate:=AddMtaQuoteRequest.EffectiveDate,
            v_vMTAEndDate:=CStr(mtaEndDate),
            v_bCancellation:=IIf(AddMtaQuoteRequest.TransactionType = TransactionType.MTC, True, False))

            If (ret <> PMEReturnCode.PMTrue) Then

                samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                        "bSIRFindInsurance.Form.CopyPolicy returned an error code of " & ret & ".  Check the Sirius log for more information")
                samErrors.CheckForErrors()
            End If

            Dim originalInsuranceFileCnt As Integer = AddMtaQuoteRequest.InsuranceFileKey

            response.InsuranceFileKey = newInsuranceFileCnt

            If AddMtaQuoteRequest.QuoteExpiryDateSpecified = True Then
                response.QuoteExpiryDate = AddMtaQuoteRequest.QuoteExpiryDate
            End If
            If AddMtaQuoteRequest.LapsedDateSpecified Then
                ' Update the Lapse Fields in the copied Insurance File record
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Set_Policy_Lapse_Status")

                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = newInsuranceFileCnt
                    cmd.AddInParameter("@lapsed_reason_id", SqlDbType.Int).Value = AddMtaQuoteRequest.LapsedReasonID
                    cmd.AddInParameter("@lapsed_date", SqlDbType.DateTime).Value = AddMtaQuoteRequest.LapsedDate
                    cmd.AddInParameter("@lapsed_description", SqlDbType.VarChar).Value = AddMtaQuoteRequest.LapsedReasonDescription

                    con.ExecuteNonQuery(cmd)
                End Using
            End If
            ' copy risks for MTA

            ProcessCopyMTARisks(con, AddMtaQuoteRequest.BranchCode, newInsuranceFileCnt)


            Dim coreBusiness As New CoreBusiness
            Dim isLocked As Boolean = False

            coreBusiness.GetSAMTimestamp(con,
                            AddMtaQuoteRequest.BranchCode,
                            CoreBusiness.LockName.InsuranceFolderCnt,
                            insuranceFolderCnt,
                            response.QuoteTimeStamp,
                            isLocked)

            If (isLocked = True) Then

                samErrors.AddBusinessRule(SAMBusinessErrors.RecordLockedByAnotherUser,
                                        "The policy is already locked by another user.")
                samErrors.CheckForErrors()

            End If

            con.CommitTransaction()

            Return response

        Catch ex As Exception

            con.RollbackTransaction()
            Throw
        Finally
            If Not findInsurance Is Nothing Then
                findInsurance.Dispose()
                findInsurance = Nothing
            End If
        End Try

    End Function

    Private Function AddMtaQuoteUnderwriting(ByVal con As SiriusConnection, ByVal AddMtaQuoteRequest As SAMForInsuranceV2ImplementationTypes.AddMtaQuoteRequestType) As SAMForInsuranceV2ImplementationTypes.AddMtaQuoteResponseType

        Dim response As New SAMForInsuranceV2ImplementationTypes.AddMtaQuoteResponseType
        Dim business As New CoreBusiness(_SiriusUser)
        Dim ret As Integer = 0
        Dim errorCode As Integer = 0
        Dim mtaEndDate As DateTime = AddMtaQuoteRequest.ExpiryDate
        Dim sRI2007 As String = ""
        Dim dInceptionDate As Date
        Dim siriusOprionRI2007 As Boolean = False
        Dim bCopyRiskMTA As Boolean = False

        sRI2007 = business.GetProductOption(gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, 1)

        If sRI2007 = "1" Then
            siriusOprionRI2007 = True
        End If

        Dim bIsBackdateMTA As Boolean = False
        Dim permanentMTA As Boolean = (AddMtaQuoteRequest.TypeOfMta = "PERMANENT")
        'Begin - WPR36
        Dim bIstrueMonthlyPolicy As Boolean
        Dim bIsMidNightRenewal As Boolean
        Dim iUnifiedRenewalDay As Integer
        Dim dAnniversaryDate As Date
        Dim oUserMTAAuthority(,) As Object
        Const MTA_AUTHORITY_WITHOUT_CLAIMS As Integer = 2
        Const MTA_AUTHORITY_NOT_ALLOWED As Integer = 1


        'End - WPR36
        'Pass in whether we're doing a permanent MTA or not
        'This is only relevant in the business object if we're MTAing an Underwriting policy
        If permanentMTA Then
            errorCode = 1
        Else
            errorCode = 0
        End If

        ' Retrieve the insurance file details.
        Dim insuranceFolderCnt As Integer = 0
        Dim insuranceRef As String = String.Empty
        Dim insuranceHolderCnt As Integer = 0
        Dim insuranceFileDetails As New BaseClaimType
        'Begin - WPR36
        Dim iProductId As Integer
        Dim dtCancellationDate As Date
        Dim oCoverEndDate As Object
        'End - WPR36
        Dim lCorrespondenceTypeID As Integer
        If Not (String.IsNullOrEmpty(AddMtaQuoteRequest.CorrespondenceType)) Then
            lCorrespondenceTypeID = business.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                "Correspondence_Type", AddMtaQuoteRequest.CorrespondenceType, "CorrespondenceType")
        End If

        Dim lDefaultPreferredCorrespondenceTypeID As Integer
        If Not (String.IsNullOrEmpty(AddMtaQuoteRequest.DefaultPreferredCorrespondence)) Then
            lDefaultPreferredCorrespondenceTypeID = business.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                "Contact_Type", AddMtaQuoteRequest.DefaultPreferredCorrespondence, "DefaultPreferredCorrespondence")
        End If

        insuranceFileDetails.InsuranceFileKey = AddMtaQuoteRequest.InsuranceFileKey
        GetInsuranceFileDetails(con, insuranceFileDetails, dInceptionDate)
        insuranceHolderCnt = insuranceFileDetails.InsuranceHolderCnt
        insuranceFolderCnt = insuranceFileDetails.InsuranceFolderCnt
        insuranceRef = insuranceFileDetails.InsuranceRef.Trim
        'Begin - WPR36
        iProductId = insuranceFileDetails.ProductId
        'End - WPR36

        Dim lRecordAffected As Integer
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_GET_Renewal_Stop_Code_Details_For_Policy")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = AddMtaQuoteRequest.InsuranceFileKey
            cmd.AddOutParameter("@RenewalMethodID", SqlDbType.Int)
            cmd.AddOutParameter("@RenewalMethodDescription", SqlDbType.VarChar, 255)
            cmd.AddOutParameter("@RenewalStopCodeID", SqlDbType.Int)
            cmd.AddOutParameter("@RenewalStopCodeDescription", SqlDbType.VarChar, 255)

            lRecordAffected = con.ExecuteNonQuery(cmd)

            AddMtaQuoteRequest.RenewalMethodCode = Convert.ToString(cmd.Parameters.Item("@RenewalMethodID").Value)
            AddMtaQuoteRequest.StopReasonCode = Convert.ToString(cmd.Parameters.Item("@RenewalStopCodeID").Value)
        End Using

        Dim findInsurance As New bSIRFindInsurance.Form
        If AddMtaQuoteRequest.CreatedById > 0 Then
            _SiriusUser.UserID = CType(AddMtaQuoteRequest.CreatedById, Short)
        End If

        SAMFunc.InitialiseSBOObject(con, findInsurance, _SiriusUser, AddMtaQuoteRequest.BranchCode, "bSIRFindInsurance.Form")

        Dim policyVersion As Integer = 0
        Dim samErrors As SAMErrorCollection = New SAMErrorCollection
        ret = findInsurance.GetOutOfSequenceMTAUserAuthority(oUserMTAAuthority)
        If (ret <> PMEReturnCode.PMTrue) Then
            samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                        "bSIRFindInsurance.Form.GetOutOfSequenceMTAUserAuthority returned an error code of " & ret & ".  Check the Sirius log for more information")
            samErrors.CheckForErrors()
        End If
        ret = findInsurance.GetVersionByDate(
            r_lInsuranceFileCnt:=AddMtaQuoteRequest.InsuranceFileKey,
            v_dtStartDate:=AddMtaQuoteRequest.EffectiveDate,
            r_lPolicyVersion:=policyVersion,
            r_lErrorCode:=errorCode)

        If (ret <> PMEReturnCode.PMTrue) Then
            samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                        "bSIRFindInsurance.Form.GetVersionByDate returned an error code of " & ret & ".  Check the Sirius log for more information")
            samErrors.CheckForErrors()
        End If

        If AddMtaQuoteRequest.IsBDXRequest And AddMtaQuoteRequest.TransactionType = TransactionType.MTR Then
            findInsurance.GetCancellationDate(insuranceFileDetails.InsuranceFolderCnt, m_dtCancellationDate:=dtCancellationDate,
                                              r_lInsFileCnt:=AddMtaQuoteRequest.InsuranceFileKey)
            findInsurance.GetCoverEndDate(insuranceFileDetails.InsuranceFolderCnt, AddMtaQuoteRequest.EffectiveDate, AddMtaQuoteRequest.InsuranceFileKey, oCoverEndDate)

            'Cancel date cannot be prior than reinstate date
            If ToSafeDate(AddMtaQuoteRequest.EffectiveDate, DateTime.MinValue) < ToSafeDate(dtCancellationDate, DateTime.MinValue) Then
                samErrors.AddBusinessRule(SAMBusinessErrors.ReinstatmentEndDatePriorCancelStartDate,
                                        "Reinstatement date cannot be prior than policy cancellation date. Cannot continue.")
            End If

            'Reinstatement end date cannot be prior to start date of cancel policy
            If ToSafeDate(AddMtaQuoteRequest.ExpiryDate, DateTime.MinValue) < ToSafeDate(dtCancellationDate, DateTime.MinValue) Then
                samErrors.AddBusinessRule(SAMBusinessErrors.ReinstatmentEndDatePriorCancelStartDate,
                                        "The reinstatement cover end date is prior to the cover start date of the cancelled policy.")
            End If

            'Reinstatement start date cannot be after the end date of cancel policy
            If ToSafeDate(AddMtaQuoteRequest.EffectiveDate, DateTime.MinValue) > ToSafeDate(oCoverEndDate, DateTime.MinValue) Then
                samErrors.AddBusinessRule(SAMBusinessErrors.ReinstatmentStartDateAfterCancelEndDate,
                                        "The reinstatement cover start date is after the cover end date of the cancelled policy.")
            End If

            'Reinstatement end date cannot be after the end date of cancel policy
            If ToSafeDate(AddMtaQuoteRequest.ExpiryDate, DateTime.MinValue) < ToSafeDate(oCoverEndDate, DateTime.MinValue) Then
                samErrors.AddBusinessRule(SAMBusinessErrors.ReinstatmentEndDateAfterCancelEndDate,
                                        "The reinstatement cover end date is prior to the cover end date of the cancelled policy.")
            End If
            If samErrors.Count > 0 Then
                samErrors.CheckForErrors()
            End If
        End If
        If (errorCode <> 0) Then
            Select Case errorCode
                Case 1
                    samErrors.AddBusinessRule(SAMBusinessErrors.FutureAdjustmentFound,
                            "Future Adjustment Found")
                Case 2
                    samErrors.AddBusinessRule(SAMBusinessErrors.OverlapsWithTemporaryMTA,
                            "Overlaps with temporary MTA")
                Case 3
                    samErrors.AddBusinessRule(SAMBusinessErrors.NoPolicyVersionFoundForThisMTAEffectiveDate,
                            "No policy version found for this MTA effective date")
                Case 4
                    samErrors.AddBusinessRule(SAMBusinessErrors.ThisPolicyVersionIsAttachedToAClosedBranchWhichDoesntAllowPermanentMTAs,
                            "This policy version is attached to a closed branch which doesn't allow permanent MTAs.")
                Case 5
                    samErrors.AddBusinessRule(SAMBusinessErrors.ThisPolicyVersionIsAttachedToAClosedBranchWhichDoesntAllowTemporaryMTAs,
                            "This policy version is attached to a closed branch which doesn't allow temporary MTAs.")
                Case 6
                    samErrors.AddBusinessRule(SAMBusinessErrors.MTAEffectiveDateIsBeforeOriginalCoverFromDateOfPolicy,
                            "MTA Effective date is before original cover from date of policy. Cannot continue.")
                Case 7
                    samErrors.AddBusinessRule(SAMBusinessErrors.BackdatedMTAIsNotPermittedPriorToTheLastPolicyRenewalDate,
                            "Backdated MTA is not permitted prior to the last Policy Renewal Date. Cannot continue.")
                    'WPR 33-75 ADDED commented as per WPR 
                    ' Case 8
                    '   samErrors.AddBusinessRule(SAMBusinessErrors.FutureAdjustmentFound, _
                    '         "MTA Effective Date is prior to a previous transaction effective date. This will result in previous transactions being reversed.")
                    'WPR 33-75 END
            End Select

            If errorCode <> 8 Then
                ' raise a business rule error if one exists
                samErrors.CheckForErrors()
            ElseIf errorCode = 8 And Not siriusOprionRI2007 Then 'Backdated MTA
                bIsBackdateMTA = True

                Dim claimStatus As Integer = 0

                ret = findInsurance.CheckInClaim(
                    v_sInsuranceRef:=insuranceRef,
                    r_lClaimStatus:=claimStatus,
                    v_dtStartDate:=AddMtaQuoteRequest.EffectiveDate)

                If (ret <> PMEReturnCode.PMTrue) Then
                    samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                "bSIRFindInsurance.Form.CheckInClaim returned an error code of " & ret & ".  Check the Sirius log for more information")
                    samErrors.CheckForErrors()
                End If

                If claimStatus <> -1 Then
                    If Information.IsArray(oUserMTAAuthority) Then
                        If oUserMTAAuthority(0, 0) IsNot Nothing AndAlso CInt(oUserMTAAuthority(0, 0)) = MTA_AUTHORITY_WITHOUT_CLAIMS Then


                            samErrors.AddBusinessRule(SAMBusinessErrors.ClaimHasBeenLodgedAfterTheEffectiveDateOfThisMTA,
                                                                "A claim has been lodged after the effective date of this MTA.")
                            samErrors.CheckForErrors()
                        End If
                    End If
                End If
                'WPR 33-75 ADDED Commented as per WPR
                'ElseIf errorCode = 8 And siriusOprionRI2007 Then
                '    samErrors.AddBusinessRule(SAMBusinessErrors.MTAEffectiveDateIsPriorToAPreviousTransactionEffectiveDate, _
                '                                "MTA Effective Date is prior to a previous transaction effective date.")
                '    samErrors.CheckForErrors()
                'WPR 33-75 ADDED
            ElseIf errorCode = 8 Then 'Allow Backdated MTA
                bIsBackdateMTA = True
                'WPR 33-75 END
            End If
        End If
        'Begin - WPR36
        Dim oGetProductRiskOptionValueRequest As New BaseProductRiskOptionValueRequestType
        If iProductId > 0 Then
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.IsTrueMonthlyPolicy
            bIstrueMonthlyPolicy = Cast.ToBoolean(Convert.ToInt32(GetProductRiskOptions(con, iProductId, oGetProductRiskOptionValueRequest)), False)
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.IsMidnightRenewal
            bIsMidNightRenewal = Cast.ToBoolean(Convert.ToInt32(GetProductRiskOptions(con, iProductId, oGetProductRiskOptionValueRequest)), False)
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.UnifiedRenewalDay
            If GetProductRiskOptions(con, iProductId, oGetProductRiskOptionValueRequest).Trim = "" Then
                iUnifiedRenewalDay = 0
            Else
                iUnifiedRenewalDay = Convert.ToInt32(GetProductRiskOptions(con, iProductId, oGetProductRiskOptionValueRequest))
            End If

        End If
        If AddMtaQuoteRequest.RenewalDayNo = 0 AndAlso AddMtaQuoteRequest.RenewalDayNoSpecified AndAlso bIstrueMonthlyPolicy AndAlso iUnifiedRenewalDay <> 0 Then
            samErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "RenewalDayNo")
        End If
        samErrors.CheckForErrors()

        Dim ds As DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_CLM_Get_Insurance_File_Details")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = AddMtaQuoteRequest.InsuranceFileKey

            ds = con.ExecuteDataSet(cmd, "InsuranceFileDetails")

            Dim dr As DataRow

            If ds.Tables("InsuranceFileDetails").Rows.Count > 0 Then

                dr = ds.Tables("InsuranceFileDetails").Rows(0)


                dInceptionDate = Convert.ToDateTime(dr.Item("inception_date"))

            End If
        End Using
        If bIstrueMonthlyPolicy Then
            If AddMtaQuoteRequest.AnniversaryDateSpecified = True Then
                If AddMtaQuoteRequest.AnniversaryDate <= Date.MinValue Then
                    samErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "AnniversaryDate")
                Else
                    dAnniversaryDate = AddMtaQuoteRequest.AnniversaryDate
                End If
            Else
                GetAnniversaryDate(bIstrueMonthlyPolicy, dInceptionDate, AddMtaQuoteRequest.RenewalDayNo,
                            bIsMidNightRenewal, dAnniversaryDate)
            End If
        Else
            GetAnniversaryDate(bIstrueMonthlyPolicy, dInceptionDate, AddMtaQuoteRequest.RenewalDayNo,
                                bIsMidNightRenewal, dAnniversaryDate)
        End If
        'End - WPR36
        Try
            con.BeginTransaction()

            If bIsBackdateMTA Then
                Dim baseInsuranceFileCnt As Integer = 0
                'get backdatebaseinsurancefilecount
                ret = findInsurance.GetBasePolicyCntForBackDateMTA(v_lInsuranceFolderCnt:=insuranceFolderCnt,
                                                                    v_dtMTADate:=AddMtaQuoteRequest.EffectiveDate,
                                                                    lBaseInsuranceFileCnt:=baseInsuranceFileCnt)

                If (ret <> PMEReturnCode.PMTrue) Then
                    samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                            "bSIRFindInsurance.Form.GetBasePolicyCntForBackDateMTA returned an error code of " & ret & ".  Check the Sirius log for more information")
                    samErrors.CheckForErrors()
                End If

                'In case of BackDated MTA update the InsFileCnt with actual base cnt
                If baseInsuranceFileCnt <> -1 Then
                    AddMtaQuoteRequest.InsuranceFileKey = baseInsuranceFileCnt
                End If

                Dim tempMTAEndDate As Object = Nothing
                'ret = findInsurance.GetCoverFromDate(v_lInsuranceFolderCnt:=insuranceFolderCnt, _
                '                 v_dtMTADate:=AddMtaQuoteRequest.EffectiveDate, _
                '                 lBaseInsuranceFileCnt:=baseInsuranceFileCnt, _
                '                 DtMTAEndDate:=tempMTAEndDate)


                'WPR 33-75 ADDED Commeted as per WPR
                'ret = findInsurance.GetCoverFromDate(v_lInsuranceFolderCnt:=insuranceFolderCnt, _
                '                 v_dtMTADate:=AddMtaQuoteRequest.EffectiveDate, _
                '                 lBaseInsuranceFileCnt:=baseInsuranceFileCnt, _
                '                 DtMTAEndDate:=tempMTAEndDate)
                'WPR 33-75 ADDED
                ret = findInsurance.GetCoverEndDate(v_lInsuranceFolderCnt:=insuranceFolderCnt,
                                v_dtMTADate:=AddMtaQuoteRequest.EffectiveDate,
                                lBaseInsuranceFileCnt:=baseInsuranceFileCnt,
                                DtMTAEndDate:=tempMTAEndDate)
                'WPR 33-75 END



                If (ret <> PMEReturnCode.PMTrue) Then
                    samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                            "bSIRFindInsurance.Form.GetCoverFromDate returned an error code of " & ret & ".  Check the Sirius log for more information")
                    samErrors.CheckForErrors()
                End If

                'Changes For PN 42936
                If IsDate(tempMTAEndDate) Then
                    mtaEndDate = Cast.ToDateTime(CDate(tempMTAEndDate), DateTime.MinValue)
                End If

            End If

            Dim newInsuranceFileCnt As Integer = 0
            If AddMtaQuoteRequest.IsMarketPlacePolicy Then
                ret = findInsurance.CopyPolicy(
                v_lOldInsuranceFileCnt:=AddMtaQuoteRequest.InsuranceFileKey,
                r_lNewInsuranceFileCnt:=newInsuranceFileCnt,
                v_lVersion:=policyVersion,
                v_bPermanentMTA:=permanentMTA,
                v_dtMTADate:=AddMtaQuoteRequest.EffectiveDate,
                v_vMTAEndDate:=CStr(mtaEndDate),
                v_bReinstatement:=AddMtaQuoteRequest.IsReinstatement,
                    v_bCancellation:=IIf(AddMtaQuoteRequest.TransactionType = TransactionType.MTC, True, False),
                            v_bCopyRiskLink:=False)
            Else
                ret = findInsurance.CopyPolicy(
                    v_lOldInsuranceFileCnt:=AddMtaQuoteRequest.InsuranceFileKey,
                    r_lNewInsuranceFileCnt:=newInsuranceFileCnt,
                    v_lVersion:=policyVersion,
                    v_bPermanentMTA:=permanentMTA,
                    v_dtMTADate:=AddMtaQuoteRequest.EffectiveDate,
                    v_vMTAEndDate:=CStr(mtaEndDate),
                    v_bReinstatement:=AddMtaQuoteRequest.IsReinstatement,
            v_bCancellation:=IIf(AddMtaQuoteRequest.TransactionType = TransactionType.MTC, True, False))
            End If

            If (ret <> PMEReturnCode.PMTrue) Then

                samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                        "bSIRFindInsurance.Form.CopyPolicy returned an error code of " & ret & ".  Check the Sirius log for more information")
                samErrors.CheckForErrors()
            End If

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_UpdateAdditionalMTADetails")
                cmd.AddInParameter("@insuranceFilecnt", SqlDbType.Int).Value = newInsuranceFileCnt
                If String.IsNullOrEmpty(AddMtaQuoteRequest.InsuredName) Then
                    cmd.AddInParameter("@InsuredName", SqlDbType.VarChar, 255).Value = ""
                Else
                    cmd.AddInParameter("@InsuredName", SqlDbType.VarChar, 255).Value = AddMtaQuoteRequest.InsuredName
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.PolicyKey) Then
                    cmd.AddInParameter("@InsuranceRef", SqlDbType.VarChar, 30).Value = ""
                Else
                    cmd.AddInParameter("@InsuranceRef", SqlDbType.VarChar, 30).Value = AddMtaQuoteRequest.PolicyKey
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.BranchCode) Then
                    cmd.AddInParameter("@SourceCode", SqlDbType.VarChar, 10).Value = ""
                Else
                    cmd.AddInParameter("@SourceCode", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.BranchCode
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.Regarding) Then
                    cmd.AddInParameter("@LapsedDescription", SqlDbType.VarChar, 255).Value = ""
                Else
                    cmd.AddInParameter("@LapsedDescription", SqlDbType.VarChar, 255).Value = AddMtaQuoteRequest.Regarding
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.AlternateReference) Then
                    cmd.AddInParameter("@AlternateReference", SqlDbType.VarChar, 80).Value = ""
                Else
                    cmd.AddInParameter("@AlternateReference", SqlDbType.VarChar, 80).Value = AddMtaQuoteRequest.AlternateReference
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.PolicyStatusCode) Then
                    cmd.AddInParameter("@PolicyStatuscode", SqlDbType.VarChar, 10).Value = ""
                Else
                    cmd.AddInParameter("@PolicyStatuscode", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.PolicyStatusCode
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.AnalysisCode) Then
                    cmd.AddInParameter("@Analysiscode", SqlDbType.VarChar, 10).Value = ""
                Else
                    cmd.AddInParameter("@Analysiscode", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.AnalysisCode
                End If
                If String.IsNullOrEmpty(AddMtaQuoteRequest.BusinessTypeCode) Then
                    cmd.AddInParameter("@Businesstypecode", SqlDbType.VarChar, 10).Value = ""
                Else
                    cmd.AddInParameter("@Businesstypecode", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.BusinessTypeCode
                End If

                If AddMtaQuoteRequest.IssueDateSpecified = True Then
                    cmd.AddInParameter("@DateIssued", SqlDbType.DateTime).Value = AddMtaQuoteRequest.IssueDate
                Else
                    cmd.AddInParameter("@DateIssued", SqlDbType.DateTime).Value = DBNull.Value 'Date.MinValue
                End If

                If AddMtaQuoteRequest.ProposalDateSpecified = True Then
                    cmd.AddInParameter("@ProposalDate", SqlDbType.DateTime).Value = AddMtaQuoteRequest.ProposalDate
                Else
                    cmd.AddInParameter("@ProposalDate", SqlDbType.DateTime).Value = AddMtaQuoteRequest.EffectiveDate
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.FrequencyCode) Then
                    cmd.AddInParameter("@RenewalFrequencycode", SqlDbType.VarChar, 10).Value = ""
                Else
                    cmd.AddInParameter("@RenewalFrequencycode", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.FrequencyCode
                End If

                If AddMtaQuoteRequest.LTUExpiryDateSpecified = True Then
                    cmd.AddInParameter("@LongTermUndertakingDate", SqlDbType.DateTime).Value = AddMtaQuoteRequest.LTUExpiryDate
                Else
                    cmd.AddInParameter("@LongTermUndertakingDate", SqlDbType.DateTime).Value = DBNull.Value 'Date.MinValue
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.StopReasonCode) Then
                    cmd.AddInParameter("@RenewalStopcode", SqlDbType.VarChar, 10).Value = DBNull.Value
                Else
                    cmd.AddInParameter("@RenewalStopcode", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.StopReasonCode
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.RenewalMethodCode) Then
                    cmd.AddInParameter("@RenewalMethodcode", SqlDbType.VarChar, 10).Value = ""
                Else
                    cmd.AddInParameter("@RenewalMethodcode", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.RenewalMethodCode
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.LapseCancelReasonCode) Then
                    cmd.AddInParameter("@LapsedReasoncode", SqlDbType.VarChar, 10).Value = ""
                Else
                    cmd.AddInParameter("@LapsedReasoncode", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.LapseCancelReasonCode
                End If

                If AddMtaQuoteRequest.LapseCancelDateSpecified = True Then
                    cmd.AddInParameter("@LapsedDate", SqlDbType.DateTime).Value = AddMtaQuoteRequest.LapseCancelDate
                Else
                    cmd.AddInParameter("@LapsedDate", SqlDbType.DateTime).Value = DBNull.Value 'Date.MinValue
                End If

                If AddMtaQuoteRequest.ReferredAtRenewalSpecified = True Then
                    cmd.AddInParameter("@IsReferredAtRenewal", SqlDbType.TinyInt).Value = IIf(AddMtaQuoteRequest.ReferredAtRenewal, 1, 0)
                Else
                    cmd.AddInParameter("@IsReferredAtRenewal", SqlDbType.TinyInt).Value = 0
                End If

                If AddMtaQuoteRequest.ReferredOnMTASpecified = True Then
                    cmd.AddInParameter("@IsReferredOnMta", SqlDbType.TinyInt).Value = IIf(AddMtaQuoteRequest.ReferredOnMTA, 1, 0)
                Else
                    cmd.AddInParameter("@IsReferredOnMta", SqlDbType.TinyInt).Value = 0
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.PolicyStyleCode) Then
                    cmd.AddInParameter("@PolicyStyleCode", SqlDbType.VarChar, 10).Value = ""
                Else
                    cmd.AddInParameter("@PolicyStyleCode", SqlDbType.VarChar).Value = AddMtaQuoteRequest.PolicyStyleCode
                End If

                If AddMtaQuoteRequest.AccountHandlerCntSpecified = True Then
                    cmd.AddInParameter("@AccountHandlerCount", SqlDbType.Int).Value = Cast.NullIfDefault(AddMtaQuoteRequest.AccountHandlerCnt)

                Else
                    cmd.AddInParameter("@AccountHandlerCount", SqlDbType.Int).Value = SqlInt32.Null

                End If

                If AddMtaQuoteRequest.QuoteExpiryDateSpecified = True Then
                    cmd.AddInParameter("@QuoteExpiryDate", SqlDbType.DateTime).Value = Cast.NullIfDefault(AddMtaQuoteRequest.QuoteExpiryDate)

                Else
                    cmd.AddInParameter("@QuoteExpiryDate", SqlDbType.DateTime).Value = DBNull.Value 'SqlInt32.Null
                End If
                If AddMtaQuoteRequest.CreatedById <> 0 Then
                    cmd.AddInParameter("@CreatedById", SqlDbType.Int).Value = AddMtaQuoteRequest.CreatedById
                End If

                If ToSafeString(AddMtaQuoteRequest.MtaReason, String.Empty) <> String.Empty Then
                    cmd.AddInParameter("@MTAReasonId", SqlDbType.Int).Value = AddMtaQuoteRequest.mtaReasonID
                End If

                'Begin - WPR36
                If AddMtaQuoteRequest.PutOnNextInstalmentRenewalSpecified = True Then
                    cmd.AddInParameter("@PutOnNextInstalmentRenewal", SqlDbType.TinyInt).Value = IIf(AddMtaQuoteRequest.PutOnNextInstalmentRenewal, 1, 0)
                Else
                    cmd.AddInParameter("@PutOnNextInstalmentRenewal", SqlDbType.TinyInt).Value = 0
                End If

                cmd.AddInParameter("@OldPolicyNumber", SqlDbType.VarChar, 30).Value = AddMtaQuoteRequest.OldPolicyNumber
                If AddMtaQuoteRequest.RenewalDateSpecified = True Then
                    cmd.AddInParameter("@RenewalDate", SqlDbType.DateTime).Value = AddMtaQuoteRequest.RenewalDate
                End If
                cmd.AddInParameter("@SystemBaseDate", SqlDbType.DateTime).Value = Now.Date
                'End - WPR36

                If Not String.IsNullOrEmpty(AddMtaQuoteRequest.UnderWritingYearCode) Then
                    cmd.AddInParameter("@UnderWritingYearCode", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.UnderWritingYearCode
                Else
                    cmd.AddInParameter("@UnderWritingYearCode", SqlDbType.VarChar, 10).Value = DBNull.Value
                End If

                If String.IsNullOrEmpty(AddMtaQuoteRequest.CoInsurancePlacement) Then
                    cmd.AddInParameter("@sCoInsurancePlacement", SqlDbType.VarChar, 10).Value = ""
                Else
                    cmd.AddInParameter("@sCoInsurancePlacement", SqlDbType.VarChar, 10).Value = AddMtaQuoteRequest.CoInsurancePlacement
                End If

                If bIsBackdateMTA Then
                    cmd.AddInParameter("@is_backdated", SqlDbType.Int).Value = 1
                End If
                If ToSafeString(lCorrespondenceTypeID) <> String.Empty Then
                    cmd.AddInParameter("@Correspondence_Type", SqlDbType.Int).Value = lCorrespondenceTypeID
                End If
                If ToSafeString(lDefaultPreferredCorrespondenceTypeID) <> String.Empty Then
                    cmd.AddInParameter("@Default_Preferred_Correspondence", SqlDbType.Int).Value = lDefaultPreferredCorrespondenceTypeID
                End If

                cmd.AddInParameter("@Is_Agent_Correspondence", SqlDbType.TinyInt).Value = IIf(AddMtaQuoteRequest.IsAgentReceiveCorrespondence, 1, 0)

                ' This is Temp MTA thus update the expiry date sent in the request
                If AddMtaQuoteRequest.TypeOfMta.ToUpper <> "PERMANENT" Then
                    If AddMtaQuoteRequest.ExpiryDate <> DateTime.MinValue Then
                        cmd.AddInParameter("@ExpiryDate", SqlDbType.DateTime).Value = AddMtaQuoteRequest.ExpiryDate
                    Else
                        cmd.AddInParameter("@ExpiryDate", SqlDbType.DateTime).Value = DBNull.Value 'Date.MinValue
                    End If
                End If
                con.ExecuteNonQuery(cmd)
            End Using

            ' RAM20050825 - PN 23018 - Store the original InsuranceFileCnt
            Dim originalInsuranceFileCnt As Integer = AddMtaQuoteRequest.InsuranceFileKey

            response.InsuranceFileKey = newInsuranceFileCnt

            ' copy risks for MTA
            'ProcessCopyMTARisks(con:=con, branchCode:=AddMtaQuoteRequest.BranchCode, insuranceFileKey:=newInsuranceFileCnt, TransactionType:=AddMtaQuoteRequest.TransactionType, bCalledViaAddMTA:=True)
            'UpdateInsuranceFileSystem(con, _SiriusUser.UserID, newInsuranceFileCnt, "MTA", False, AddMtaQuoteRequest.Regarding)

            If AddMtaQuoteRequest.IsBDXRequest Then
                If AddMtaQuoteRequest.TransactionType = TransactionType.MTC OrElse AddMtaQuoteRequest.TransactionType = TransactionType.MTR OrElse AddMtaQuoteRequest.TransactionType = TransactionType.MTA Then
                    ProcessCopyMTARisks(con, AddMtaQuoteRequest.BranchCode, newInsuranceFileCnt)
                End If
            Else
                Dim sCopyRiskMTA As String = business.GetProductOption(gPMConstants.SIRHiddenOptions.SIROPTCopyRiskInMTA, 1)
                If sCopyRiskMTA = "1" AndAlso AddMtaQuoteRequest.TransactionType = TransactionType.MTA Then
                    bCopyRiskMTA = True
                End If

                If AddMtaQuoteRequest.TransactionType = TransactionType.MTC OrElse AddMtaQuoteRequest.TransactionType = TransactionType.MTR OrElse bCopyRiskMTA Then
                    If Not AddMtaQuoteRequest.IsMarketPlacePolicy Then
                        ProcessCopyMTARisks(con, AddMtaQuoteRequest.BranchCode, newInsuranceFileCnt, AddMtaQuoteRequest.TransactionType, False, True, False, bCopyRiskMTA)
                    End If
                    If bCopyRiskMTA Then
                        ProcessMandatoryRisk(con, AddMtaQuoteRequest.BranchCode, newInsuranceFileCnt, 0, AddMtaQuoteRequest.TransactionType)
                    End If
                Else
                    'EM Copy Risk Change
                    ProcessMandatoryRisk(con, AddMtaQuoteRequest.BranchCode, newInsuranceFileCnt, 0, AddMtaQuoteRequest.TransactionType)
                End If
            End If

            If bIsBackdateMTA Then
                Dim autoMTABusiness As bSIRAutoMTA.Business = CreateAndInitialiseAutoMTABusiness(con, AddMtaQuoteRequest.BranchCode)

                'This function ensures that the Risk expiry on the MTA matches the next affected version -1 day
                autoMTABusiness.IsBackdatedMTARequired(v_lInsuranceFolderCnt:=insuranceFolderCnt,
                    v_dtEffectiveDate:=AddMtaQuoteRequest.EffectiveDate, v_lNewInsuranceFileCnt:=newInsuranceFileCnt)
            End If

            'Add Event

            If Not String.IsNullOrEmpty(AddMtaQuoteRequest.mtaReasonDescription) Then
                'Add Event
                CreateEvent(con, insuranceHolderCnt,
                        insuranceFolderCnt, newInsuranceFileCnt,
                        AddMtaQuoteRequest.CreatedById, DateTime.Now,
                        AddMtaQuoteRequest.mtaReasonDescription, "POLCHANGE", True)
            End If

            Dim coreBusiness As New CoreBusiness
            Dim isLocked As Boolean = False

            coreBusiness.GetSAMTimestamp(con,
                            AddMtaQuoteRequest.BranchCode,
                            CoreBusiness.LockName.InsuranceFolderCnt,
                            insuranceFolderCnt,
                            response.QuoteTimeStamp,
                            isLocked)

            If (isLocked = True) Then

                samErrors.AddBusinessRule(SAMBusinessErrors.RecordLockedByAnotherUser,
                                        "The policy is already locked by another user.")
                samErrors.CheckForErrors()

            End If

            con.CommitTransaction()

            Return response

        Catch ex As Exception

            con.RollbackTransaction()
            Throw
        Finally
            If Not findInsurance Is Nothing Then
                findInsurance.Dispose()
                findInsurance = Nothing
            End If
        End Try

    End Function

    Private Overloads Sub DeleteRenewal(ByVal con As SiriusConnection, ByVal insuranceFileKey As Integer, ByVal branchCode As String,
                              ByVal insuranceRef As String, ByVal partyKey As Integer)

        Dim ret As Integer = 0
        Dim failureMessage As String = String.Empty
        Dim samErrors As SAMErrorCollection = New SAMErrorCollection
        Dim renewalProcess As New bSIRRenewalProcess.Business
        Dim clientName As String = ""

        SAMFunc.InitialiseSBOObject(con, renewalProcess, _SiriusUser, branchCode, "bSIRRenewalProcess.Business")

        ret = renewalProcess.DeletePolicyFromRenewal(insuranceFileKey, False, failureMessage)

        If (ret <> PMEReturnCode.PMTrue) Then
            con.RollbackTransaction()

            samErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                        "bSIRRenewalProcess.Business.DeletePolicy returned an error code of " & ret & ".  The following failure message was also returned - " & failureMessage)
            samErrors.CheckForErrors()
        End If

        Dim ds As DataSet = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_Details")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = partyKey

            ds = con.ExecuteDataSet(cmd, "PartyDetails")

            Dim dr As DataRow

            If ds.Tables("PartyDetails").Rows.Count > 0 Then

                dr = ds.Tables("PartyDetails").Rows(0)

                clientName = dr.Item("name").ToString

            End If

        End Using

        Dim createWMTaskRequest As New SAMForInsuranceImplementationTypes.CreateWmTaskRequestType
        Dim createWMTaskResponse As BaseCreateWmTaskResponseType

        'TODO - Use correct user/user group fromt the system options
        createWMTaskRequest.AllocationUser = "sirius"
        createWMTaskRequest.AllocationUserGroup = "SYSADMIN"
        createWMTaskRequest.BranchCode = branchCode
        createWMTaskRequest.Client = clientName
        createWMTaskRequest.Description = "Renewal - " & insuranceRef & " - Manual Invite Required Following MTA"
        createWMTaskRequest.DueDateTime = Now
        createWMTaskRequest.IsComplete = False
        createWMTaskRequest.Task = "RENSELPOL"
        createWMTaskRequest.TaskGroup = "UWRENEWAL"
        createWMTaskRequest.UserId = _SiriusUser.UserID
        createWMTaskResponse = CreateWmTask(con, createWMTaskRequest)
        renewalProcess.Dispose()
        renewalProcess = Nothing
    End Sub

    ' ****************************************************************
    ' Name: AddQuote
    '
    ' Description: This method accepts the base quote implementation type
    '              and adds a new quote stub record (insurance_folder and insurance_file)
    ' ****************************************************************
    Public Overloads Function AddQuote(ByVal AddQuoteRequest As BaseAddQuoteRequestType) As BaseAddQuoteResponseType

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As BaseAddQuoteResponseType

            oResponse = AddQuote(con, AddQuoteRequest)

            Return oResponse

        End Using

    End Function

    Public Overloads Function AddQuote(ByVal con As SiriusConnection, ByVal AddQuoteRequest As BaseAddQuoteRequestType) As BaseAddQuoteResponseType

        'Declare the Response object
        Dim oResponse As New BaseImplementationTypes.BaseAddQuoteResponseType

        'Declare the input and output objects for core add quote method
        Dim oAddQuoteIn As New AddQuoteIn
        Dim oAddQuoteOut As AddQuoteOut = Nothing
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)

        Dim nTypeOfPackage As enumTypeOfPackage ' naming convention?

        Dim oAgentRequest As AgentImplementationTypes.AddQuoteRequestType = Nothing
        Dim oCustomerRequest As CustomerImplementationTypes.AddQuoteRequestType = Nothing
        Dim oSAMForInsuranceRequest As SAMForInsuranceImplementationTypes.AddQuoteRequestType = Nothing
        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.AddQuoteRequestType = Nothing
        Dim performSecurityCheck As Boolean
        Dim username As String = String.Empty
        Dim partyKey As Integer
        Dim iAgentKey As Int32 = 0

        Const ACMethodName As String = "AddQuote"

        If AddQuoteRequest.GetType Is GetType(AnonymousImplementationTypes.AddQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AnonymousPackage
            oResponse = New AnonymousImplementationTypes.AddQuoteResponseType
        ElseIf AddQuoteRequest.GetType Is GetType(AgentImplementationTypes.AddQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.AddQuoteResponseType
            oAgentRequest = DirectCast(AddQuoteRequest, AgentImplementationTypes.AddQuoteRequestType)
        ElseIf AddQuoteRequest.GetType Is GetType(CustomerImplementationTypes.AddQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oResponse = New CustomerImplementationTypes.AddQuoteResponseType
            oCustomerRequest = DirectCast(AddQuoteRequest, CustomerImplementationTypes.AddQuoteRequestType)
        ElseIf AddQuoteRequest.GetType Is GetType(BaseImplementationTypes.BaseAddQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oResponse = New BaseImplementationTypes.BaseAddQuoteResponseType
        ElseIf AddQuoteRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.AddQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.AddQuoteResponseType
            oSAMForInsuranceRequest = DirectCast(AddQuoteRequest, SAMForInsuranceImplementationTypes.AddQuoteRequestType)
            iAgentKey = oSAMForInsuranceRequest.AgentKey
        ElseIf AddQuoteRequest.GetType Is GetType(SAMForBrokingImplementationTypes.AddQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oResponse = New SAMForBrokingImplementationTypes.AddQuoteResponseType
            samForBrokingRequest = DirectCast(AddQuoteRequest, SAMForBrokingImplementationTypes.AddQuoteRequestType)
        ElseIf AddQuoteRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.AddQuoteResponseType
            Dim oSAMForInsuranceV2Request As SAMForInsuranceV2ImplementationTypes.AddQuoteRequestType = DirectCast(AddQuoteRequest, SAMForInsuranceV2ImplementationTypes.AddQuoteRequestType)
            iAgentKey = oSAMForInsuranceV2Request.AgentKey
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        oResponse.InsuranceFileRef = ""

        oAddQuoteIn.DataModelCode = "AOL"
        oAddQuoteIn.BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB

        Dim Utils As New Utilities

        'Dim iRiskCodeId As Int32
        'Dim iScreenId As Int32
        'Dim iProductId As Int32
        'Dim iSourceId As Int32
        'Dim iCurrencyId As Int32
        'Dim iAnalysisId As Integer
        Dim STSListType As Core.STSListType = Nothing
        Dim lInsurerCnt As Int32
        Dim lGisSchemeID As Int32
        Dim lRiskGroupID As Int32
        Dim sRiskGroupCode As String = String.Empty
        Dim sXMLDataset As String = ""

        Dim ErrEx As Exception = Nothing

        Dim STSError As New STSErrorPublisher

        If (AddQuoteRequest Is Nothing) = True Then
            Return oResponse
        End If

        '*********************
        ' STRUCTURE VALIDATION 
        '*********************
        AddQuoteRequest.Validate(CType(STSError, Object), True)
        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        '*********************
        ' DATA VALIDATION 
        '*********************
        ' validate the data provided in the structure
        AddQuoteValidateData(con, oCoreBusiness, AddQuoteRequest, STSError, True)
        ' exit if there are any invalid parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            username = oAgentRequest.UserName
            partyKey = oAgentRequest.PartyKey
            performSecurityCheck = True
        ElseIf nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                username = samForBrokingRequest.UserName
                partyKey = samForBrokingRequest.PartyKey
                performSecurityCheck = True

                If oCoreBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Agent Key validation failed", "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Key Validation", True)
                    Return oResponse
                End If

            End If
        End If

        If performSecurityCheck Then
            If (oCoreBusiness.AgentSecurityCheck(username, CInt(partyKey), PMEEntityType.Party) = False) Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security check failed", username & " does not have permission to access party " & partyKey)
                STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddQuote", True)
                Return oResponse
            End If
        End If

        If nTypeOfPackage <> enumTypeOfPackage.UnknownPackage Then
            'Check SubBranchCode
            Try
                If (Not String.IsNullOrEmpty(AddQuoteRequest.SubBranchCode) AndAlso (AddQuoteRequest.SubBranchCode.Length <> 0)) Then
                    If SAMFunc.CheckSubBranch(AddQuoteRequest.BranchCode, AddQuoteRequest.SubBranchCode) = False Then
                        STSError.AddInvalidField("SubBranchCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "SubBranchCode"), AddQuoteRequest.SubBranchCode)
                    End If
                End If
            Catch ex As Exception
                STSError.AddInvalidField("SubBranchCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "SubBranchCode"), AddQuoteRequest.SubBranchCode)
            End Try

            If STSError.HasErrors Then
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
                Return oResponse
            End If
        End If

        Dim vAdditionalDataArray() As AdditionalData
        Dim oData As New AdditionalData
        Dim lUbnd As Integer

        ReDim vAdditionalDataArray(3)

        ' Add the Product Id to the Additional Data Array
        lUbnd = 0
        oData = New AdditionalData
        oData.Name = InternalSAMConstants.ACProductId
        oData.Value = AddQuoteRequest.ProductId
        vAdditionalDataArray(lUbnd) = oData

        ' Add the Policy Type to the Additional Data Array
        lUbnd = 1
        oData = New AdditionalData
        oData.Name = InternalSAMConstants.ACPolicyTypeIdentifier
        oData.Value = InternalSAMConstants.ACPolicyTypeValueUW
        vAdditionalDataArray(lUbnd) = oData

        ' Add the Consolidated Lead Agent Commission flag to the Additional Data Array
        lUbnd = 2
        oData = New AdditionalData
        oData.Name = InternalSAMConstants.ACConsolidatedLeadAgentCommission
        oData.Value = AddQuoteRequest.ConsolidatedLeadAgentCommission
        vAdditionalDataArray(lUbnd) = oData

        ' Add the Consolidated Sub Agent Commission flag to the Additional Data Array
        lUbnd = 3
        oData = New AdditionalData
        oData.Name = InternalSAMConstants.ACConsolidatedSubAgentCommission
        oData.Value = AddQuoteRequest.ConsolidatedSubAgentCommission
        vAdditionalDataArray(lUbnd) = oData

        ' Get the insured name, if not passed in
        If SAMFunc.NothingToString(AddQuoteRequest.InsuredName) = "" Then
            GetInsuredNameFromParty(con, AddQuoteRequest.PartyKey, AddQuoteRequest.InsuredName)
        End If

        ' Set the inputs for the core add quote method
        With oAddQuoteIn

            .BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
            .CurrencyId% = AddQuoteRequest.CurrencyId%
            .EffectiveDate = AddQuoteRequest.CoverStartDate
            .ExpirationDate = AddQuoteRequest.CoverEndDate
            .InsuranceFolderDescription = AddQuoteRequest.Description
            .PartyCnt = AddQuoteRequest.PartyKey
            .RiskGroupId = AddQuoteRequest.ProductId%
            .ScreenId = 0 ' Default to zero
            .AdditionalDataArray = vAdditionalDataArray
            .SourceId = AddQuoteRequest.SourceId%
            .InsuredName = AddQuoteRequest.InsuredName
            .AnalysisId = AddQuoteRequest.AnalysisId
            .PolicyStatusCode = AddQuoteRequest.PolicyStatusCode
            .InsuranceFileRef = AddQuoteRequest.QuoteRef
            .InsuranceFolderCnt = AddQuoteRequest.InsuranceFolderKey
            .PolicyVersion = AddQuoteRequest.PolicyVersion
            .AlternateReference = AddQuoteRequest.AlternateReference
            .sCoInsurancePlacement = AddQuoteRequest.CoInsurancePlacement
            If (nTypeOfPackage = enumTypeOfPackage.AgentsPackage) Then
                .AgentCnt = oAgentRequest.AgentKey
            ElseIf (nTypeOfPackage = enumTypeOfPackage.CustomersPackage) Then
                .AgentCnt = oCustomerRequest.AgentKey
            ElseIf (nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage) Then
                .AgentCnt = iAgentKey
            ElseIf (nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package) Then
                .AgentCnt = iAgentKey
            End If

            .LapsedReasonCode = AddQuoteRequest.LapsedReasonCode
            .LapsedReasonID = AddQuoteRequest.LapsedReasonID
            If AddQuoteRequest.LapsedDateSpecified = True Then .LapsedDate = AddQuoteRequest.LapsedDate
            .LapsedReasonDescription = AddQuoteRequest.LapsedReasonDescription
            If AddQuoteRequest.InceptionDateSpecified = True Then .InceptionDate = AddQuoteRequest.InceptionDate
            If AddQuoteRequest.InceptionDateTPISpecified = True Then .InceptionDateTPI = AddQuoteRequest.InceptionDateTPI

            If AddQuoteRequest.RenewalDateSpecified = True Then
                .RenewalDate = AddQuoteRequest.RenewalDate
            Else

                ' determine if policy is on IsMidNightRenewal product
                Dim productDetails As New ProductDetailsType(AddQuoteRequest.ProductCode)
                If productDetails.IsMidNightRenewal Then
                    ' default the renewal date to be the day after the policy has ended
                    .RenewalDate = Informations.DateAdd(DateInterval.Day, 1, AddQuoteRequest.CoverEndDate)
                Else
                    .RenewalDate = AddQuoteRequest.CoverEndDate
                End If

            End If

            .OldPolicyNumber = AddQuoteRequest.OldPolicyNumber
            .AccountExecutiveShortname = AddQuoteRequest.AccountExecutiveShortname
            .AlternateReference = AddQuoteRequest.AlternateReference
            .AccountHandlerShortname = AddQuoteRequest.AccountHandlerShortname
            .PolicyVersionTypeCode = AddQuoteRequest.PolicyVersionTypeCode
            .RiskCode = AddQuoteRequest.RiskCode
            .RiskCodeId = AddQuoteRequest.RiskCodeId
            .sCoInsurancePlacement = AddQuoteRequest.CoInsurancePlacement

        End With

        ' Call the core add quote method
        Try
            oAddQuoteOut = oCoreBusiness.AddQuote(con, oAddQuoteIn)
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Failed to add Quote.", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddQuote", True)
        End Try

        If oAddQuoteOut.STSError IsNot Nothing Then
            oResponse.STSError = oAddQuoteOut.STSError
            Return oResponse
        End If

        ' Set the results
        With oResponse
            .InsuranceFileKey = oAddQuoteOut.InsuranceFileCnt
            .InsuranceFileRef = oAddQuoteOut.InsuranceFileRef
            .InsuranceFolderKey = oAddQuoteOut.InsuranceFolderCnt
            .RiskKey = oAddQuoteOut.RiskCnt
        End With

        SetMulticurrencyValues(con, oAddQuoteOut, oCoreBusiness, AddQuoteRequest.SourceId, AddQuoteRequest.CurrencyId)

        'Update the Sub Branch Id with the supplied one
        If (Not String.IsNullOrEmpty(AddQuoteRequest.SubBranchCode) AndAlso (AddQuoteRequest.SubBranchCode.Length <> 0)) Then
            Try
                UpdateSubBranch(con, oAddQuoteOut.InsuranceFileCnt, AddQuoteRequest.BranchCode, AddQuoteRequest.SubBranchCode)
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher("Failed to update Sub Branch.", ex)
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddQuote", True)
            End Try
        End If

        ' Add sub agents associated with the lead agent to the quote policy
        If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Or
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
            AddAssociatedSubAgents(con, oAddQuoteOut.InsuranceFileCnt, oAddQuoteIn.AgentCnt)

            AssignCoverNoteSheet(con, oAddQuoteOut.InsuranceFileCnt, AddQuoteRequest.CoverNoteBookNumber, AddQuoteRequest.CoverNoteSheetNumber)

        End If

        If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Or
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then

            ' set the anniversary flag
            SetTMPAnniversaryFlag(con, oResponse.InsuranceFileKey)

        End If

        ' Get the Timestamp
        Dim AnyError As STSErrorType
        Dim bIsLocked As Boolean
        AnyError = oCoreBusiness.GetTimestamp(con,
                            AddQuoteRequest.BranchCode,
                            CoreBusiness.LockName.InsuranceFolderCnt,
                            oAddQuoteOut.InsuranceFolderCnt,
                            oResponse.QuoteTimeStamp,
                            bIsLocked)
        ' Return AnyErrors
        If AnyError Is Nothing = False Then
            oResponse.STSError = AnyError
        End If

        oResponse.XmlDataset = sXMLDataset


        Return oResponse
    End Function

    Private Sub SetTMPAnniversaryFlag(
    ByVal con As SiriusConnection,
    ByVal insuranceFileKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Set_Policy_True_Monthly_Policy_Status")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub
    Private Sub UpdateSubBranch(ByVal con As SiriusConnection, ByVal insurancefilekey As Integer, ByVal branchcode As String, ByVal subbranchcode As String)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Update_Sub_Branch_id")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insurancefilekey
            cmd.AddInParameter("@branch_code", SqlDbType.VarChar, 10).Value = branchcode
            cmd.AddInParameter("@sub_branch_code", SqlDbType.VarChar, 10).Value = subbranchcode
            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Function CreateRiskDataset(ByVal con As SiriusConnection, ByVal AddQuoteRequest As BaseAddQuoteRequestType, ByVal oResponse As BaseAddQuoteResponseType, ByVal oAddQuoteIn As AddQuoteIn, ByVal oAddQuoteOut As AddQuoteOut, ByVal oCoreBusiness As CoreBusiness, ByRef sXMLDataset As String, ByVal ErrEx As Exception, ByRef shouldReturn As Boolean) As BaseAddQuoteResponseType
        shouldReturn = False
        'Create Risk Dataset
        Dim oGIS As bGIS.Application = Nothing
        Dim iRet As Int32
        Try
            oGIS = New bGIS.Application
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Debug.WriteLine(ex.Message)
        Finally
        End Try

        ' Initialise the GIS
        SAMFunc.InitialiseGIS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

        ' Call the Method
        Try
            Dim lGisPolicyLinkID As Int32
            Dim sTopOIKey As String = ""
            Dim sXMLDataSetDef As String = ""
            iRet = oGIS.NewRiskDataset(
                v_sGisDataModelCode:=CStr(oAddQuoteIn.DataModelCode),
                r_lPolicyLinkID:=lGisPolicyLinkID,
                r_sTopOIKey:=sTopOIKey,
                r_sXMLDataSetDef:=sXMLDataSetDef,
                r_sXMLDataset:=sXMLDataset,
                v_lRiskID:=oAddQuoteOut.RiskCnt,
                v_lInsuranceFileCnt:=oAddQuoteOut.InsuranceFileCnt)

        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ErrEx = New Exception("Failed to call bGIS.Application.NewRiskDataset", ex)
            ExceptionManager.Publish(ErrEx)
            Throw ErrEx
        Finally
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            If (ErrEx Is Nothing) Then
                If (iRet <> PMEReturnCode.PMTrue) Then
                    ErrEx = New Exception("bGIS.Application.NewRiskDataset FAILED. Return Value = " + iRet.ToString)
                    ExceptionManager.Publish(ErrEx)
                    Throw ErrEx
                End If
            End If
        End Try

        ' Get the Timestamp
        Dim AnyError2 As STSErrorType
        Dim bIsLocked2 As Boolean
        AnyError2 = oCoreBusiness.GetTimestamp(
                            AddQuoteRequest.BranchCode,
                            CoreBusiness.LockName.InsuranceFolderCnt,
                            oAddQuoteOut.InsuranceFolderCnt,
                            oResponse.QuoteTimeStamp,
                            bIsLocked2)
        ' Return AnyErrors
        If AnyError2 Is Nothing = False Then
            oResponse.STSError = AnyError2
        End If

        'Get Risk
        Dim oGetRiskIn As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseGetRiskRequestType
        Dim oGetRiskOut As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseGetRiskResponseType
        oGetRiskIn.BranchCode = AddQuoteRequest.BranchCode
        oGetRiskIn.InsuranceFileKey = oResponse.InsuranceFileKey
        oGetRiskIn.InsuranceFolderKey = oResponse.InsuranceFolderKey
        oGetRiskIn.QuoteTimeStamp = oResponse.QuoteTimeStamp
        oGetRiskIn.RiskKey = oResponse.RiskKey
        oGetRiskOut = RetrieveRisk(oGetRiskIn)
        sXMLDataset = oGetRiskOut.XMLDataSet

        'RUN DEFAULT RULES (ADD)
        'Declare Implementation Type for Running Default Rules
        Dim oRulesIn As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseRunDefaultRulesAddRequestType
        Dim oRulesOut As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseRunDefaultRulesAddResponseType
        'Set Inputs In
        oRulesIn.DataModelCode = oAddQuoteIn.DataModelCode
        oRulesIn.BranchCode = AddQuoteRequest.BranchCode
        oRulesIn.ScreenCode = AddQuoteRequest.ScreenCode
        oRulesIn.XMLDataSet = sXMLDataset
        'Run Code
        oRulesOut = RunDefaultRulesAdd(oRulesIn)
        'Set Outputs Back
        sXMLDataset = oRulesOut.XMLDataSet
        If oRulesOut.STSError IsNot Nothing Then
            oResponse.STSError = oRulesOut.STSError
            shouldReturn = True : Return oResponse
        End If
        Return Nothing
    End Function

    Private Sub AddQuoteValidateData(
ByVal con As SiriusConnection,
ByVal Business As CoreBusiness,
ByRef Request As BaseAddQuoteRequestType,
ByRef STSError As STSErrorPublisher,
ByVal IsUnderwriting As Boolean)

        If Request.CoverEndDate < Request.CoverStartDate Then
            STSError = New STSErrorPublisher(STSErrorCodes.CoverEndDateIsBeforeCoverStartDate, "Cover End date is before Cover Start date", Request.CoverEndDate.ToString)
        End If

        '***************************
        ''' validate standard lookup data
        '***************************

        AddQuoteValidateDataStandardLookup(con, Business, Request, STSError)

        ' If we already have errors then return at this point
        If STSError.HasErrors Then
            Exit Sub
        End If

        '***************************
        ''' validate business data
        '***************************

        Dim samErrorsCollection As New SAMErrorCollection
        If Request.CoverNoteBookNumber = String.Empty And ((Request.CoverNoteSheetNumberSpecified = True) And Request.CoverNoteSheetNumber <> 0) Then
            ' Validation Error - Cover note book number not specified
            samErrorsCollection.AddBusinessRule(
                SAMConstants.SAMBusinessErrors.CoverNoteBookNumberNotSpecified,
                SAMConstants.SAMBusinessErrors.CoverNoteBookNumberNotSpecified.ToString)
        End If

        If Request.CoverNoteBookNumber <> String.Empty And ((Request.CoverNoteSheetNumberSpecified = False) Or Request.CoverNoteSheetNumber = 0) Then
            ' Validation Error - Cover note sheet number not specified
            samErrorsCollection.AddBusinessRule(
                SAMConstants.SAMBusinessErrors.CoverNoteSheetNumberNotSpecified,
                SAMConstants.SAMBusinessErrors.CoverNoteSheetNumberNotSpecified.ToString)
        End If

        If Request.CoverNoteBookNumber <> String.Empty And ((Request.CoverNoteSheetNumberSpecified = True) And Request.CoverNoteSheetNumber > 0) Then

            Dim validationResult As Integer = 0
            Dim sheetStatus As String = String.Empty

            IsValidCoverNote(con, samErrorsCollection, 0, Request.CoverNoteBookNumber, Request.CoverNoteSheetNumber, Request.SourceId, Request.AgentKey, Request.ProductId, validationResult, sheetStatus)

        End If

        If Request.LapsedDateSpecified = True Then
            If Request.LapsedDate < Request.CoverStartDate Then
                ' Validation Error - Lapse Date cannot be before the Cover Start Date
                samErrorsCollection.AddBusinessRule(
                    SAMConstants.SAMBusinessErrors.LapseDateIsBeforeTheCoverStartDate,
                    SAMConstants.SAMBusinessErrors.LapseDateIsBeforeTheCoverStartDate.ToString)
            ElseIf Request.LapsedDate > Request.CoverEndDate Then
                ' Validation Error - Lapse Date cannot be after the Cover End Date
                samErrorsCollection.AddBusinessRule(
                    SAMConstants.SAMBusinessErrors.LapseDateIsAfterTheCoverEndDate,
                    SAMConstants.SAMBusinessErrors.LapseDateIsAfterTheCoverEndDate.ToString)
            End If
        End If
        samErrorsCollection.CheckForErrors()

    End Sub

    Private Sub IsValidCoverNote(
            ByRef con As SiriusConnection,
            ByVal samErrorsCollection As SAMErrorCollection,
            ByVal InsuranceFileCnt As Integer,
            ByVal CoverNoteBookNumber As String,
            ByVal CoverSheetNumber As Integer,
            ByVal BranchId As Integer,
            ByVal AgentCnt As Integer,
            ByVal ProductId As Integer,
            ByRef ValidationResult As Integer,
            ByRef SheetStatus As String)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Cover_Note_Sheet_Validate")

            cmd.AddInParameter("@sCover_Note_Book_Number", SqlDbType.VarChar, 50).Value = CoverNoteBookNumber
            cmd.AddInParameter("@lCover_Note_Sheet_Number", SqlDbType.Int).Value = CoverSheetNumber
            cmd.AddInParameter("@lInsurance_File_Cnt", SqlDbType.Int).Value = InsuranceFileCnt
            cmd.AddInParameter("@lBranch_Id", SqlDbType.Int).Value = BranchId
            cmd.AddInParameter("@lAgent_Cnt", SqlDbType.Int).Value = AgentCnt
            cmd.AddInParameter("@lProduct_Id", SqlDbType.Int).Value = ProductId
            cmd.AddOutParameter("@lReturn_Status", SqlDbType.Int)
            cmd.AddOutParameter("@sSheet_Status", SqlDbType.VarChar, 20)

            con.ExecuteNonQuery(cmd)

            ValidationResult = Cast.ToInt32(cmd.Parameters.Item("@lReturn_Status").Value, 0)
            SheetStatus = Cast.ToStringTrim(cmd.Parameters.Item("@sSheet_Status").Value, String.Empty)

        End Using

        Select Case ValidationResult
            Case 1
                'Succeeded

            Case 2
                ' Validation Error - Please enter a valid Book Number
                samErrorsCollection.AddBusinessRule(
                    SAMConstants.SAMBusinessErrors.CoverNoteBookNumberIsInvalid,
                    SAMConstants.SAMBusinessErrors.CoverNoteBookNumberIsInvalid.ToString)

            Case 3
                ' Validation Error - Cover Note Number not assigned to selected agent
                samErrorsCollection.AddBusinessRule(
                    SAMConstants.SAMBusinessErrors.CoverNoteNumberNotAssignedToSelectedAgent,
                    SAMConstants.SAMBusinessErrors.CoverNoteNumberNotAssignedToSelectedAgent.ToString)

            Case 4
                ' Validation Error - Cover Note Number not assigned to selected product
                samErrorsCollection.AddBusinessRule(
                    SAMConstants.SAMBusinessErrors.CoverNoteNumberNotAssignedToSelectedProduct,
                    SAMConstants.SAMBusinessErrors.CoverNoteNumberNotAssignedToSelectedProduct.ToString)

            Case 5
                ' Validation Error - Cover Note Number not assigned to selected branch
                samErrorsCollection.AddBusinessRule(
                    SAMConstants.SAMBusinessErrors.CoverNoteNumberNotAssignedToSelectedBranch,
                    SAMConstants.SAMBusinessErrors.CoverNoteNumberNotAssignedToSelectedBranch.ToString)

            Case 6
                'TODO as and when specification will come

            Case 7
                ' Validation Error - Cover note number entered is not a valid number
                samErrorsCollection.AddBusinessRule(
                    SAMConstants.SAMBusinessErrors.CoverNoteNumberEnteredIsNotAValidNumber,
                    SAMConstants.SAMBusinessErrors.CoverNoteNumberEnteredIsNotAValidNumber.ToString)

            Case 8
                If SheetStatus.ToUpper.Trim = "MISSING" Then
                    ' Validation Error - Cover note status missing, policy cannot be issued
                    samErrorsCollection.AddBusinessRule(
                        SAMConstants.SAMBusinessErrors.CoverNoteStatusMissingPolicyCannotBeIssued,
                        SAMConstants.SAMBusinessErrors.CoverNoteStatusMissingPolicyCannotBeIssued.ToString)

                ElseIf SheetStatus.ToUpper.Trim = "CANCEL" Then
                    ' Validation Error - Cover note status cancelled, policy cannot be issued
                    samErrorsCollection.AddBusinessRule(
                        SAMConstants.SAMBusinessErrors.CoverNoteStatusCancelledPolicyCannotBeIssued,
                        SAMConstants.SAMBusinessErrors.CoverNoteStatusCancelledPolicyCannotBeIssued.ToString)

                ElseIf SheetStatus.ToUpper.Trim = "ISSUED" Then
                    ' Validation Error - Cover note number already converted to policy
                    samErrorsCollection.AddBusinessRule(
                        SAMConstants.SAMBusinessErrors.CoverNoteNumberAlreadyConvertedToPolicy,
                        SAMConstants.SAMBusinessErrors.CoverNoteNumberAlreadyConvertedToPolicy.ToString)

                Else
                    ' Validation Error - Cover note number entered is not a valid number
                    samErrorsCollection.AddBusinessRule(
                        SAMConstants.SAMBusinessErrors.CoverNoteNumberEnteredIsNotAValidNumber,
                        SAMConstants.SAMBusinessErrors.CoverNoteNumberEnteredIsNotAValidNumber.ToString)

                End If
        End Select

    End Sub
    'Begin WPR36
    Private Sub GetAnniversaryDate(
            ByVal IsTrueMonthlyPolicy As Boolean,
            ByVal InceptionDate As Date,
            ByVal RenewalDayNo As Integer,
            ByVal IsMidNightRenewal As Boolean,
            ByRef AnniversaryDate As Date)

        Dim nDay As Integer = 0

        If IsTrueMonthlyPolicy Then
            If RenewalDayNo <> 0 Then
                AnniversaryDate = Informations.DateAdd(DateInterval.Year, 1, InceptionDate)
                nDay = Day(AnniversaryDate)
                nDay = RenewalDayNo - nDay
                AnniversaryDate = Informations.DateAdd(DateInterval.Day, nDay, AnniversaryDate)
            Else
                If IsMidNightRenewal Then
                    AnniversaryDate = Informations.DateAdd(DateInterval.Day, -1, Informations.DateAdd(DateInterval.Year, 1, InceptionDate))
                Else
                    AnniversaryDate = Informations.DateAdd(DateInterval.Year, 1, InceptionDate)
                End If
            End If
        Else
            If IsMidNightRenewal Then
                AnniversaryDate = Informations.DateAdd(DateInterval.Day, -1, Informations.DateAdd(DateInterval.Year, 1, InceptionDate))
            Else
                AnniversaryDate = Informations.DateAdd(DateInterval.Year, 1, InceptionDate)
            End If
        End If
    End Sub
    'End WPR36
    Private Sub AddQuoteValidateDataStandardLookup(
ByVal con As SiriusConnection,
ByVal Business As CoreBusiness,
ByRef Request As BaseAddQuoteRequestType,
ByRef STSError As STSErrorPublisher)

        '*********************
        'mandatory
        '*********************

        ' Lookup the branch ID
        Try
            Request.SourceId = Business.GetListItemFromCode(Core.STSListType.PMLookup, "Source", Request.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), Request.BranchCode)
        End Try

        ' Lookup Product Id
        Try
            Request.ProductId = Business.GetListItemFromCode(Core.STSListType.PMLookup, "Product", Request.ProductCode)
        Catch ex As Exception
            STSError.AddInvalidField("ProductCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(MandatoryInputInvalid, "ProductCode"), Request.ProductCode)
        End Try

        '*********************
        ' optional
        '*********************

        ' Check that the currency was passed in
        If String.IsNullOrEmpty(Request.CurrencyCode) = False Then
            ' Lookup the Currency ID
            Try
                Request.CurrencyId = Business.GetListItemFromCode(Core.STSListType.PMLookup, "Currency", Request.CurrencyCode)
            Catch ex As Exception
                STSError.AddInvalidField("CurrencyCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "CurrencyCode"), Request.CurrencyCode)
            End Try
        Else
            GetBaseCurrencyFromBranch(con, Request.BranchCode, Request.CurrencyId)
            If Request.CurrencyId = 0 Then
                STSError.AddInvalidField("CurrencyCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "CurrencyCode"), Request.CurrencyCode)
            End If
        End If

        ' Check that the analysis code  was passed in
        If String.IsNullOrEmpty(Request.AnalysisCode) = False Then
            ' Lookup the analysis ID
            Try
                Request.AnalysisId = Business.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.AnalysisCode, Request.AnalysisCode)
            Catch ex As Exception
                STSError.AddInvalidField("AnalysisCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "AnalysisCode"), Request.AnalysisCode)
            End Try
        End If

        ' Check if the sub branch was passed in
        If String.IsNullOrEmpty(Request.SubBranchCode) = False Then
            ' Lookup the sub branch
            If SAMFunc.CheckSubBranch(Request.BranchCode, Request.SubBranchCode) = False Then
                STSError.AddInvalidField("SubBranchCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "SubBranchCode"), Request.SubBranchCode)
            End If
        End If

        ' Check if the LapsedReasonCode was passed in
        If String.IsNullOrEmpty(Request.LapsedReasonCode) = False Then
            ' Lookup the LapsedReason ID
            Try
                Request.LapsedReasonID = Business.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.LapsedReason, Request.LapsedReasonCode)
            Catch ex As Exception
                STSError.AddInvalidField("LapsedReasonCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "LapsedReasonCode"), Request.LapsedReasonCode)
            End Try
        End If
    End Sub

    Private Sub GetBaseCurrencyFromBranch(ByVal con As SiriusConnection,
    ByVal BranchCode As String,
    ByRef CurrencyID As Integer)

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Source_From_Code")
            cmd.AddInParameter("@source_code", SqlDbType.VarChar, 10).Value = BranchCode
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim dr As DataRow = dt.Rows(0)

            CurrencyID = Cast.ToInt32(dr.Item("base_currency_id"), 0)

        End If

    End Sub

    Private Sub GetScreenDetailsFromScreenandRisk(ByVal con As SiriusConnection,
    ByVal ScreenId As Integer,
    ByVal RiskCode As String,
    ByRef InsurerCnt As Integer,
    ByRef GisSchemeID As Integer,
    ByRef RiskGroupID As Integer,
    ByRef RiskGroupCode As String,
    ByRef DataModelCode As String)

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_GetPartySchemeForScreenidDatamodelcodeRiskCode")
            cmd.AddInParameter("@Gis_screen_id", SqlDbType.Int).Value = ScreenId
            cmd.AddInParameter("@Risk_Code", SqlDbType.VarChar, 10).Value = RiskCode
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim dr As DataRow = dt.Rows(0)

            InsurerCnt = Cast.ToInt32(dr.Item(0), 0)
            GisSchemeID = Cast.ToInt32(dr.Item(1), 0)
            RiskGroupID = Cast.ToInt32(dr.Item(2), 0)
            RiskGroupCode = Cast.ToStringTrim(dr.Item(3))
            ' Get the actual data model code. Note that the BOMRequired
            ' registry setting for this data model must be set to 'AOL'
            DataModelCode = Cast.ToStringTrim(dr.Item(4))

        End If

    End Sub

    Private Sub GetInsuredNameFromParty(ByVal con As SiriusConnection,
    ByVal PartyKey As Integer,
    ByRef InsuredName As String)

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Insured_Name")
            cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = PartyKey
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim dr As DataRow = dt.Rows(0)

            InsuredName = Cast.ToStringTrim(dr.Item(0))

        End If

    End Sub

    '''<summary>
    ''' Get the insured name from party
    '''</summary>
    '''<param name="PartyKey"></param>
    ''' <param name="InsuredName"></param>
    '''<remarks></remarks> 

    Public Sub GetInsuredNameFromParty(ByVal PartyKey As Integer, ByRef InsuredName As String)

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            GetInsuredNameFromParty(con, PartyKey, InsuredName)

        End Using

    End Sub

    '''<summary>
    ''' This is core SAM method for AddQuoteV2
    '''</summary>
    '''<param name="AddQuoteV2Request" type="BaseAddQuoteV2RequestType"></param>   
    '''<returns>BaseAddQuoteV2ResponseType</returns>
    '''<remarks></remarks> 

    Public Overloads Function AddQuoteV2(ByVal AddQuoteV2Request As BaseAddQuoteV2RequestType) As BaseAddQuoteV2ResponseType

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As BaseAddQuoteV2ResponseType

            oResponse = AddQuoteV2(con, AddQuoteV2Request)

            Return oResponse

        End Using

    End Function

    '''<summary>
    ''' This is core SAM method for AddQuoteV2
    '''</summary>
    '''<param name="con" type="SiriusConnection"></param>   
    '''<param name="oAddQuoteV2Request" type="BaseAddQuoteV2RequestType"></param>   
    '''<returns>BaseAddQuoteV2ResponseType</returns>
    '''<remarks></remarks> 

    Public Overloads Function AddQuoteV2(ByVal con As SiriusConnection, ByVal oAddQuoteV2Request As BaseAddQuoteV2RequestType) As BaseAddQuoteV2ResponseType

        Dim oCreateQuoteBO As bSIRInsuranceFile.Services = Nothing
        Dim oGenPolBO As bSIRPolicyNumMaint.Business = Nothing
        Dim oBaseCountryBO As bPMSource.Business = Nothing
        Dim oSourceDefault As bPMUSourceDefaults.Business = Nothing
        Dim icomReturnValue As Integer
        Dim oCreateQuoteResponse As New BaseAddQuoteV2ResponseType
        Dim oErrorCollection As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim nTypeOfPackage As enumTypeOfPackage

        Dim sInsuranceFileRef As String = ""
        Dim iCountryId As Int16
        Dim iCollectionFrequencyId As Integer
        Const ACInsuranceFileStructure As String = "PMB"
        Const ACPaymentMethod As String = "Cash"

        Dim iBusinessTypeCodeId As Integer
        Dim iProductCodeId As Integer
        Dim iBranchCodeId As Integer
        Dim iAnalysisCodeId As Integer
        Dim iCurrencyCodeId As Integer
        Dim iSubBranchCodeId As Integer
        Dim iHandlerCodeId As Integer
        Dim iPolicyStatusCodeId As Integer
        Dim iFrequencyCodeId As Integer
        Dim iPaymentTermId As Integer
        Dim iRenewalMethodCodeId As Integer
        Dim iLapseCancelReasonCodeId As Integer
        Dim iStopReasonCodeId As Integer
        Dim iAgentKeyId As Integer
        Dim bIstrueMonthlyPolicy As Boolean
        Dim bIsMidNightRenewal As Boolean
        Dim iUnifiedRenewalDay As Integer
        Dim dtAnniversaryDate As Date
        Dim iUnderWritingYearID As Integer = 0
        Dim nDirectBusiness As Integer
        Dim nAgentID As Integer
        Dim nPartyCnt As Integer = 0
        If oAddQuoteV2Request.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddQuoteV2RequestType) Then
            nTypeOfPackage = CoreSAMBusiness.enumTypeOfPackage.SAMForInsuranceV2Package
            oCreateQuoteResponse = New SAMForInsuranceV2ImplementationTypes.AddQuoteV2ResponseType
        Else
            oCreateQuoteResponse = New BaseImplementationTypes.BaseAddQuoteV2ResponseType
        End If

        'Mandatory Validation
        If String.IsNullOrEmpty(oAddQuoteV2Request.BranchCode) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "BranchCode")
        End If
        If oAddQuoteV2Request.CoverStartDate <= Date.MinValue Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "CoverStartDate")
        End If
        If oAddQuoteV2Request.CoverEndDate <= Date.MinValue Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "CoverEndDate")
        End If
        If String.IsNullOrEmpty(oAddQuoteV2Request.ProductCode) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "ProductCode")
        End If
        If String.IsNullOrEmpty(oAddQuoteV2Request.CurrencyCode) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "CurrencyCode")
        End If
        If oAddQuoteV2Request.AgentKey = 0 AndAlso oAddQuoteV2Request.AgentKeySpecified = True Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "AgentKey")
        End If
        If oAddQuoteV2Request.PartyKey = 0 Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "PartyKey")
        End If
        If String.IsNullOrEmpty(oAddQuoteV2Request.SubBranchCode) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "SubBranchCode")
        End If
        If String.IsNullOrEmpty(oAddQuoteV2Request.BusinessTypeCode) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "BusinessTypeCode")
        End If
        If oAddQuoteV2Request.QuoteExpiryDate <= Date.MinValue Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "QuoteExpiryDate")
        End If
        If oAddQuoteV2Request.InceptionDate <= Date.MinValue Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "InceptionDate")
        End If
        If oAddQuoteV2Request.RenewalDate <= Date.MinValue Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "RenewalDate")
        End If
        If oAddQuoteV2Request.InceptionTPI <= Date.MinValue Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "InceptionTPI")
        End If
        If String.IsNullOrEmpty(oAddQuoteV2Request.FrequencyCode) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "FrequencyCode")
        End If
        oErrorCollection.CheckForErrors()

        'Lookups Validations
        Try
            iBranchCodeId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oAddQuoteV2Request.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                    SAMInvalidData.InvalidLookupListValue.ToString,
                                    "BranchCode",
                                    oAddQuoteV2Request.BranchCode)
        End Try
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.ProductCode) Then
            Try
                iProductCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Product", oAddQuoteV2Request.ProductCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "ProductCode",
                                                        oAddQuoteV2Request.ProductCode)
            End Try
        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.CurrencyCode) Then
            Try
                iCurrencyCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Currency", oAddQuoteV2Request.CurrencyCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "CurrencyCode",
                                                        oAddQuoteV2Request.CurrencyCode)
            End Try
        End If

        If Not String.IsNullOrEmpty(oAddQuoteV2Request.CollectionFrequencyCode) Then
            Try
                iCollectionFrequencyId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "CollectionFrequency", oAddQuoteV2Request.CollectionFrequencyCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "CollectionFrequency",
                                                        oAddQuoteV2Request.CollectionFrequencyCode)
            End Try
        End If

        If Not String.IsNullOrEmpty(oAddQuoteV2Request.PaymentTermCode) Then
            Try
                iPaymentTermId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "DOPaymentTerms", oAddQuoteV2Request.PaymentTermCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "PaymentTerm",
                                                        oAddQuoteV2Request.PaymentTermCode)
            End Try
        End If
        If oAddQuoteV2Request.AgentKey <> 0 AndAlso oAddQuoteV2Request.AgentKeySpecified Then
            iAgentKeyId = GetAndValidateSpecifiedTableCode(con, "party_agent", "Party_Cnt", "Party_Cnt", oAddQuoteV2Request.AgentKey.ToString, oErrorCollection, "AgentKey")
            oErrorCollection.CheckForErrors()
        End If
        If oAddQuoteV2Request.PartyKey <> 0 Then
            nPartyCnt = oAddQuoteV2Request.PartyKey
        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.AnalysisCode) Then
            Try
                iAnalysisCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.AnalysisCode, oAddQuoteV2Request.AnalysisCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "AnalysisCode",
                                                        oAddQuoteV2Request.AnalysisCode)
            End Try
        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.SubBranchCode) Then
            Try
                iSubBranchCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "sub_branch", oAddQuoteV2Request.SubBranchCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "SubBranchCode",
                                                        oAddQuoteV2Request.SubBranchCode)
            End Try
        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.BusinessTypeCode) Then
            Try
                iBusinessTypeCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "business_type", oAddQuoteV2Request.BusinessTypeCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "BusinessTypeCode",
                                                        oAddQuoteV2Request.BusinessTypeCode)
            End Try
        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.HandlerCode) Then

            iHandlerCodeId = GetAndValidateSpecifiedTableCode(con, "Party", "Party_Cnt", "shortname", oAddQuoteV2Request.HandlerCode.ToString, oErrorCollection, "HandlerCode")
            oErrorCollection.CheckForErrors()

        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.PolicyStatusCode) Then
            Try
                iPolicyStatusCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.PolicyStatus, oAddQuoteV2Request.PolicyStatusCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "PolicyStatusCode",
                                                        oAddQuoteV2Request.PolicyStatusCode)
            End Try
        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.FrequencyCode) Then
            Try
                iFrequencyCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Renewal_Frequency", oAddQuoteV2Request.FrequencyCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "FrequencyCode",
                                                        oAddQuoteV2Request.FrequencyCode)
            End Try
        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.RenewalMethodCode) Then
            Try
                iRenewalMethodCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Renewal_Method", oAddQuoteV2Request.RenewalMethodCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "RenewalMethodCode",
                                                        oAddQuoteV2Request.RenewalMethodCode)
            End Try
        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.LapseCancelReasonCode) Then
            Try
                iLapseCancelReasonCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Lapsed_reason", oAddQuoteV2Request.LapseCancelReasonCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "LapseCancelReasonCode",
                                                        oAddQuoteV2Request.LapseCancelReasonCode)
            End Try
        End If
        If Not String.IsNullOrEmpty(oAddQuoteV2Request.StopReasonCode) Then
            Try
                iStopReasonCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Renewal_Stop_Code", oAddQuoteV2Request.StopReasonCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "StopReasonCode",
                                                        oAddQuoteV2Request.StopReasonCode)
            End Try
        End If

        Dim sEnableUnderwritingYear As String = ""
        sEnableUnderwritingYear = oCoreBusiness.GetProductOption(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, 1)
        If sEnableUnderwritingYear = "1" Then
            If Not String.IsNullOrEmpty(oAddQuoteV2Request.UnderwritingYearCode) Then
                Try
                    iUnderWritingYearID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Underwriting_Year", oAddQuoteV2Request.UnderwritingYearCode)

                    ValidateAndGetValidUnderwritingYear(con, iUnderWritingYearID, oAddQuoteV2Request.CoverStartDate, oCoreBusiness, oErrorCollection)

                Catch ex As Exception
                    oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                            SAMInvalidData.InvalidLookupListValue.ToString,
                                                            "Underwriting_Year",
                                                            oAddQuoteV2Request.UnderwritingYearCode)
                End Try
            ElseIf (String.IsNullOrEmpty(oAddQuoteV2Request.UnderwritingYearCode)) Then
                GetDefaultUnderwritingYear(con, iUnderWritingYearID, oAddQuoteV2Request.CoverStartDate, oCoreBusiness, oErrorCollection)
            End If
        End If

        oErrorCollection.CheckForErrors()
        'Begin - WPR36
        Dim oGetProductRiskOptionValueRequest As New BaseProductRiskOptionValueRequestType
        If iProductCodeId > 0 Then
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.IsTrueMonthlyPolicy
            bIstrueMonthlyPolicy = Cast.ToBoolean(Convert.ToInt32(GetProductRiskOptions(con, iProductCodeId, oGetProductRiskOptionValueRequest)), False)
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.IsMidnightRenewal
            bIsMidNightRenewal = Cast.ToBoolean(Convert.ToInt32(GetProductRiskOptions(con, iProductCodeId, oGetProductRiskOptionValueRequest)), False)
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.UnifiedRenewalDay
            If GetProductRiskOptions(con, iProductCodeId, oGetProductRiskOptionValueRequest).Trim = "" Then
                iUnifiedRenewalDay = 0
            Else
                iUnifiedRenewalDay = Convert.ToInt32(GetProductRiskOptions(con, iProductCodeId, oGetProductRiskOptionValueRequest))
            End If
        End If
        If oAddQuoteV2Request.RenewalDayNo = 0 AndAlso oAddQuoteV2Request.RenewalDayNoSpecified AndAlso bIstrueMonthlyPolicy AndAlso iUnifiedRenewalDay <> 0 Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "RenewalDayNo")
        End If

        'End - WPR36

        Dim iReturnValue As Integer
        Dim sSheetStatus As String = String.Empty
        If oAddQuoteV2Request.CoverNoteBookNumber <> String.Empty AndAlso oAddQuoteV2Request.CoverNoteSheetNumberSpecified AndAlso oAddQuoteV2Request.CoverNoteSheetNumber <> 0 Then
            'This is to validate the cover note sheet and book
            IsValidCoverNote(con, oErrorCollection, 0, oAddQuoteV2Request.CoverNoteBookNumber,
                                             oAddQuoteV2Request.CoverNoteSheetNumber, iBranchCodeId, iAgentKeyId, iProductCodeId, iReturnValue, sSheetStatus)
        End If

        oErrorCollection.CheckForErrors()
        If bIstrueMonthlyPolicy Then
            If oAddQuoteV2Request.AnniversaryDateSpecified Then
                If oAddQuoteV2Request.AnniversaryDate <= Date.MinValue Then
                    GetAnniversaryDate(bIstrueMonthlyPolicy, oAddQuoteV2Request.InceptionDate, oAddQuoteV2Request.RenewalDayNo,
                                          bIsMidNightRenewal, dtAnniversaryDate)
                Else
                    dtAnniversaryDate = oAddQuoteV2Request.AnniversaryDate
                End If
            Else
                GetAnniversaryDate(bIstrueMonthlyPolicy, oAddQuoteV2Request.InceptionDate, oAddQuoteV2Request.RenewalDayNo,
                                    bIsMidNightRenewal, dtAnniversaryDate)
            End If
        Else
            GetAnniversaryDate(bIstrueMonthlyPolicy, oAddQuoteV2Request.InceptionDate, oAddQuoteV2Request.RenewalDayNo,
                        bIsMidNightRenewal, dtAnniversaryDate)
        End If
        Dim bSkipNewPolicyNumber As Boolean
        Try
            If oAddQuoteV2Request.QuoteRef = String.Empty Then
                'Generate the policy number
                oGenPolBO = New bSIRPolicyNumMaint.Business
                SAMFunc.InitialiseSBOObject(con, oGenPolBO, _SiriusUser, "bSIRPolicyNumMaint.Business")
                icomReturnValue = oGenPolBO.GeneratePolicyNumber(v_lBusinessType:=1,
                                                             v_iBranch:=Convert.ToInt16(iBranchCodeId),
                                                             v_lProductId:=iProductCodeId,
                                                             v_lAgent:=oAddQuoteV2Request.AgentKey,
                                                             r_sGeneratedPolicyNumber:=sInsuranceFileRef,
                                                                 v_lPartyCnt:=nPartyCnt)
                If oGenPolBO IsNot Nothing Then
                    oGenPolBO.Dispose()
                    oGenPolBO = Nothing
                End If
                If icomReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPolicyNumMaint.Business.GeneratePolicyNumber", icomReturnValue)
                End If
                bSkipNewPolicyNumber = False
            Else
                bSkipNewPolicyNumber = True
                sInsuranceFileRef = oAddQuoteV2Request.QuoteRef
            End If

            'Get the branch base country
            oBaseCountryBO = New bPMSource.Business
            SAMFunc.InitialiseSBOObject(con, oBaseCountryBO, _SiriusUser, "bPmSource.Business")
            icomReturnValue = oBaseCountryBO.GetBranchBaseCountry(
                                                                v_lSourceID:=iBranchCodeId,
                                                                r_iCountryID:=CInt(iCountryId))


            If oBaseCountryBO IsNot Nothing Then
                oBaseCountryBO.Dispose()
                oBaseCountryBO = Nothing
            End If
            If icomReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bPmSource.Business.GetBranchBaseCountry", icomReturnValue)
            End If

            If Not oAddQuoteV2Request.IsBDXRequest Then
                oSourceDefault = New bPMUSourceDefaults.Business
                SAMFunc.InitialiseSBOObject(con, oSourceDefault, _SiriusUser, "bPMUSourceDefaults.Business")
                icomReturnValue = oSourceDefault.GetSourceDefaults(v_lSourceID:=iBranchCodeId,
                                                                   r_iDirectBusiness:=nDirectBusiness,
                                                                     r_lAgentID:=nAgentID)
                If nDirectBusiness = 1 Then

                    oAddQuoteV2Request.BusinessTypeCode = "DIRECT"
                Else
                    oAddQuoteV2Request.BusinessTypeCode = "AGENCY"
                    If nAgentID <> 0 And oAddQuoteV2Request.AgentKey = 0 And oAddQuoteV2Request.AgentKeySpecified = False Then
                        oAddQuoteV2Request.AgentKeySpecified = True
                        oAddQuoteV2Request.AgentKey = nAgentID
                    End If
                End If
                If oSourceDefault IsNot Nothing Then
                    oSourceDefault.Dispose()
                    oSourceDefault = Nothing
                End If
                If icomReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bPMUSourceDefaults.Business.GetSourceDefaults", icomReturnValue)
                End If
            End If
            'Now we can create the quote
            oCreateQuoteBO = New bSIRInsuranceFile.Services
            SAMFunc.InitialiseSBOObject(con, oCreateQuoteBO, _SiriusUser, "bSIRInsuranceFile.Services")
            With oAddQuoteV2Request
                oCreateQuoteBO.BusinessTypeID = IIf(iBusinessTypeCodeId <> 0, iBusinessTypeCodeId, System.DBNull.Value)
                oCreateQuoteBO.CoverStartDate = .CoverStartDate
                oCreateQuoteBO.ExpiryDate = .CoverEndDate
                oCreateQuoteBO.Product = .ProductCode
                oCreateQuoteBO.ProductID = IIf(iProductCodeId <> 0, iProductCodeId, System.DBNull.Value)
                oCreateQuoteBO.InsuranceFolderDescription = .Description
                oCreateQuoteBO.QuoteInsuranceRef = .QuoteRef
                oCreateQuoteBO.InsuredName = .InsuredName
                oCreateQuoteBO.CurrencyCode = .CurrencyCode
                oCreateQuoteBO.CurrencyID = IIf(iCurrencyCodeId <> 0, iCurrencyCodeId, System.DBNull.Value)
                oCreateQuoteBO.LeadAgentCnt = .AgentKey
                oCreateQuoteBO.AnalysisCodeId = IIf(iAnalysisCodeId <> 0, iAnalysisCodeId, Nothing)
                oCreateQuoteBO.AlternateReference = .AlternativeRef
                oCreateQuoteBO.InsuredCnt = .PartyKey
                oCreateQuoteBO.InsuranceHolderCnt = .PartyKey
                oCreateQuoteBO.SubBranchID = IIf(iSubBranchCodeId <> 0, iSubBranchCodeId, System.DBNull.Value)
                oCreateQuoteBO.SubBranch = .SubBranchCode
                oCreateQuoteBO.LeadAgentAllowCommission = Convert.ToInt32(IIf(.ConsolidatedLeadAgentCommissionSpecified, 1, 0))
                oCreateQuoteBO.SubAgentAllowCommission = Convert.ToInt32(IIf(.ConsolidatedSubAgentCommissionSpecified, 1, 0))
                oCreateQuoteBO.BusinessTypeID = IIf(iBusinessTypeCodeId <> 0, iBusinessTypeCodeId, System.DBNull.Value)
                oCreateQuoteBO.BusinessType = .BusinessTypeCode
                oCreateQuoteBO.QuoteExpiryDate = .QuoteExpiryDate
                oCreateQuoteBO.AccountHandlerCnt = IIf(iHandlerCodeId <> 0, iHandlerCodeId, System.DBNull.Value)
                oCreateQuoteBO.AccountHandler = .HandlerCode
                oCreateQuoteBO.LastTransDescription = .Regarding
                oCreateQuoteBO.OldPolicyNumber = .OldPolicyNumber
                oCreateQuoteBO.PolicyStatusID = IIf(iPolicyStatusCodeId <> 0, iPolicyStatusCodeId, 0)
                oCreateQuoteBO.PolicyStatus = .PolicyStatusCode
                oCreateQuoteBO.InceptionDate = .InceptionDate
                oCreateQuoteBO.CCInceptionDate = .InceptionDate
                oCreateQuoteBO.RenewalDate = .RenewalDate
                oCreateQuoteBO.InceptionTPI = .InceptionTPI
                oCreateQuoteBO.DateIssued = IIf(.IssuedDateSpecified, .IssuedDate, Date.Now)
                oCreateQuoteBO.ProposalDate = IIf(.ProposalDateSpecified, .ProposalDate, .CoverStartDate)
                oCreateQuoteBO.RenewalFrequencyID = IIf(iFrequencyCodeId <> 0, iFrequencyCodeId, System.DBNull.Value)
                oCreateQuoteBO.RenewalFrequency = .FrequencyCode
                oCreateQuoteBO.RenewalMethodID = IIf(iRenewalMethodCodeId <> 0, iRenewalMethodCodeId, System.DBNull.Value)
                oCreateQuoteBO.RenewalMethod = .RenewalMethodCode
                oCreateQuoteBO.LapsedReasonID = IIf(iLapseCancelReasonCodeId <> 0, iLapseCancelReasonCodeId, System.DBNull.Value)
                oCreateQuoteBO.LapsedReason = .LapseCancelReasonCode
                oCreateQuoteBO.RenewalStopCodeId = IIf(iStopReasonCodeId <> 0, iStopReasonCodeId, System.DBNull.Value)
                oCreateQuoteBO.LapsedDate = IIf(.LapseCancelDateSpecified, .LapseCancelDate, System.DBNull.Value)
                oCreateQuoteBO.RenewalCount = IIf(.RenewalCountSpecified, .RenewalCount, 0)
                oCreateQuoteBO.IsReferredAtRenewal = IIf(.ReferredAtRenewalSpecified, IIf(.ReferredAtRenewal, 1, 0), 0)
                oCreateQuoteBO.IsReferredOnMta = IIf(.ReferredAtMTASpecified, IIf(.ReferredAtMTA, 1, 0), 0)
                'Constance values
                oCreateQuoteBO.InsuranceFileStructure = ACInsuranceFileStructure

                If ToSafeString(oAddQuoteV2Request.PaymentTermCode) = "" Then
                    oCreateQuoteBO.PaymentMethod = ACPaymentMethod
                Else
                    oCreateQuoteBO.PaymentMethod = oAddQuoteV2Request.PaymentTermCode
                End If
                If iCollectionFrequencyId <> 0 Then
                    oCreateQuoteBO.CollectionFrequencyID = iCollectionFrequencyId
                End If
                If iPaymentTermId <> 0 Then
                    oCreateQuoteBO.PaymentTermsID = iPaymentTermId
                End If

                'Com Returned Values
                oCreateQuoteBO.CountryID = IIf(iCountryId <> 0, iCountryId, System.DBNull.Value)
                oCreateQuoteBO.InsuranceRef = sInsuranceFileRef
                'Added on 04-Sep-2008 as per discussion with Rahul said by shankar
                oCreateQuoteBO.PolicyTypeId = 5
                'Added on 01-sep-2008 as per the discussion with gaurav 
                oCreateQuoteBO.EventDescription = "Quotation record created"
                oCreateQuoteBO.SourceID = IIf(iBranchCodeId <> 0, iBranchCodeId, System.DBNull.Value)
                oCreateQuoteBO.AccountHandlerCnt = IIf(.AccountHandlerCntSpecified, .AccountHandlerCnt, System.DBNull.Value)
                oCreateQuoteBO.LastTransDescription = .Regarding
                oCreateQuoteBO.OldPolicyNumber = .OldPolicyNumber
                oCreateQuoteBO.PutOnNextInstalmentRenewal = Cast.ToInt32(IIf(.PutOnNextInstalmentRenewalSpecified, IIf(.PutOnNextInstalmentRenewal, 1, 0), 0), 0)
                oCreateQuoteBO.AnniversaryDate = dtAnniversaryDate
                oCreateQuoteBO.IsMarketPlacePolicy = Convert.ToInt32(IIf(.IsMarketPlacePolicy, 1, 0))
                oCreateQuoteBO.UnderwritingYearID = IIf(iUnderWritingYearID <> 0, iUnderWritingYearID, System.DBNull.Value)
                oCreateQuoteBO.CoInsPlacement = .CoInsurancePlacement
                oCreateQuoteResponse.SkipNewPolicyNumber = bSkipNewPolicyNumber
                icomReturnValue = oCreateQuoteBO.CreatePolicy()
            End With
            If icomReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRInsuranceFile.Services.CreatePolicy", icomReturnValue)
            End If
            'Response Objects
            oCreateQuoteResponse.InsuranceFileKey = oCreateQuoteBO.InsuranceFileCnt
            oCreateQuoteResponse.InsuranceFolderKey = CType(oCreateQuoteBO.InsuranceFolderCnt, Integer)
            oCreateQuoteResponse.InsuranceFileRef = CType(oCreateQuoteBO.InsuranceRef, String)
            oCreateQuoteResponse.QuoteExpiryDate = CType(oCreateQuoteBO.QuoteExpiryDate, DateTime)
            UpdateInsuranceFileSystem(con, _SiriusUser.UserID, oCreateQuoteBO.InsuranceFileCnt, "NB", False, oAddQuoteV2Request.Regarding)
            AssignCoverNoteSheet(con, oCreateQuoteBO.InsuranceFileCnt, oAddQuoteV2Request.CoverNoteBookNumber, Convert.ToInt32(IIf(oAddQuoteV2Request.CoverNoteSheetNumberSpecified, oAddQuoteV2Request.CoverNoteSheetNumber, 0)))
            SetMulticurrencyValues(con, oCreateQuoteBO, oCoreBusiness, Convert.ToInt32(oCreateQuoteBO.SourceID), Convert.ToInt32(oCreateQuoteBO.CurrencyID))
            ' WPR53 : ToDo: all AddMandatoryRisk con, oCreateQuoteBO.ProductId, has_mandatory_risk
            If oAddQuoteV2Request.AgentKeySpecified Then
                AddSubAgent(con, oCreateQuoteBO.InsuranceFileCnt, oAddQuoteV2Request.AgentKey)
            End If
            Dim bIsMandatoryRisk As Boolean
            AddMandatoryRisk(con, iProductCodeId, bIsMandatoryRisk, oCreateQuoteResponse.InsuranceFileKey, oCreateQuoteResponse.InsuranceFolderKey, oCreateQuoteResponse, oAddQuoteV2Request.BranchCode)
            oCreateQuoteResponse.IsMandatoryRisk = CType(bIsMandatoryRisk, Boolean)
            oCreateQuoteResponse.IsMandatoryRiskSpecified = CType(bIsMandatoryRisk, Boolean)

            Dim oAnyError As STSErrorType
            Dim bIsLocked As Boolean
            oAnyError = oCoreBusiness.GetTimestamp(con,
                                oAddQuoteV2Request.BranchCode,
                                CoreBusiness.LockName.InsuranceFolderCnt,
                                 oCreateQuoteResponse.InsuranceFolderKey,
                                oCreateQuoteResponse.QuoteTimeStamp,
                                bIsLocked)
            ' Return AnyErrors
            If oAnyError Is Nothing = False Then
                oCreateQuoteResponse.STSError = oAnyError
            End If
        Finally
            If oCreateQuoteBO IsNot Nothing Then
                oCreateQuoteBO.Dispose()
                oCreateQuoteBO = Nothing
            End If
        End Try
        Return oCreateQuoteResponse
    End Function

    ''' <summary>
    ''' BindQuote
    ''' </summary>
    ''' <param name="BindQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BindQuote(ByVal BindQuoteRequest As BaseBindQuoteRequestType) As BaseBindQuoteResponseType

        Dim oResponse As New BaseImplementationTypes.BaseBindQuoteResponseType
        Dim oNBTransactIn As New NBTransactIn
        Dim oNBTransactOut As NBTransactOut
        Dim oProcessAccountsIn As New ProcessAccountsIn
        Dim oProcessAccountsOut As ProcessAccountsOut = Nothing
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim oAdditionalData() As AdditionalData
        Dim oSamForBrokingRequest As SAMForBrokingImplementationTypes.BindQuoteRequestType = Nothing
        Dim STSError As New STSErrorPublisher
        Dim nInsuranceFolderCnt As Integer
        Dim nInsuranceFileCnt As Integer
        Dim nRiskCnt As Integer
        Dim nPolicyLinkID As Integer
        Dim crGrossAmount As Decimal
        Dim crCommissionAmount As Decimal
        Dim crIPTAmount As Decimal
        Dim crFeeAmount As Decimal = 0
        Dim sPolicyNum As String = String.Empty
        Dim sPaymentMethod As String = String.Empty
        Dim nDepositTransdetailID As Integer
        Dim oAnyError As STSErrorType
        Dim nInsuranceFolderKey As Integer
        Dim nBranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oSAMErrorCollection As New SAMErrorCollection
        Const ACMethodName As String = "BindQuote"
        Dim sAutoGeneratedPlanRef As String = ""
        Dim nCurrencyCodeId As Integer

        ' Set type of package
        If BindQuoteRequest.GetType Is GetType(BaseImplementationTypes.BaseBindQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.MessagingPackage
            oResponse = New BaseImplementationTypes.BaseBindQuoteResponseType
        ElseIf BindQuoteRequest.GetType Is GetType(CustomerImplementationTypes.BindQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oResponse = New CustomerImplementationTypes.BindQuoteResponseType
        ElseIf BindQuoteRequest.GetType Is GetType(AgentImplementationTypes.BindQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.BindQuoteResponseType
        ElseIf BindQuoteRequest.GetType Is GetType(AnonymousImplementationTypes.BindQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AnonymousPackage
            oResponse = New AnonymousImplementationTypes.BindQuoteResponseType
        ElseIf BindQuoteRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.BindQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.BindQuoteResponseType
        ElseIf BindQuoteRequest.GetType Is GetType(SAMForBrokingImplementationTypes.BindQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oResponse = New SAMForBrokingImplementationTypes.BindQuoteResponseType
            oSamForBrokingRequest = DirectCast(BindQuoteRequest, SAMForBrokingImplementationTypes.BindQuoteRequestType)
        ElseIf BindQuoteRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.BindQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.BindQuoteResponseType
        End If

        nBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                         PMLookupTable.Source, BindQuoteRequest.BranchCode,
                            "BranchCode", oSAMErrorCollection)

        If BindQuoteRequest.CoverStartDateSpecified Then
            If BindQuoteRequest.CreditTransactions IsNot Nothing Then
                For icount As Integer = 0 To BindQuoteRequest.CreditTransactions.Row.Length - 1
                    If BindQuoteRequest.CreditTransactions.Row(icount).CollectionDate > BindQuoteRequest.CoverStartDate Then
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CollectionDateError,
                                                   SAMBusinessErrors.CollectionDateError.ToString,
                                                   "CollectionDate")
                    End If
                Next
            End If
        End If
        oSAMErrorCollection.CheckForErrors()
        Dim sOptionValue As String = String.Empty
        oCoreBusiness.GetSystemOption(BindQuoteRequest.BranchCode, SystemOption.AllowClientPolicyAssociations, sOptionValue)

        If sOptionValue = "1" Then
            Dim sProcname As String = String.Empty
            Dim nResult As Integer = PMConst.PMTrue

            sProcname = "spu_Insurance_File_Associates_DateUpdate"

            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure(sProcname)
                    cmd.AddInParameter("@Insurance_file_cnt", SqlDbType.Int).Value = BindQuoteRequest.InsuranceFileKey
                    con.ExecuteNonQuery(cmd)
                End Using
            End Using
        End If

        If BindQuoteRequest.SourceId = 0 Then
            nBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, BindQuoteRequest.BranchCode, "BranchCode")
            BindQuoteRequest.SourceId = nBranchId
        End If

        Using con As SiriusConnection = New SiriusConnectionPMDAO(_SiriusUser.Username,
                                                                  _SiriusUser.SourceID,
                                                                  _SiriusUser.LanguageID,
                                                  SiriusUserDefaults.AppName)
            If Not (oCoreBusiness.CheckInsuranceFile(con, BindQuoteRequest.InsuranceFileKey, nInsuranceFolderKey)) Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.PolicyRecordNotFound,
                                                   SAMInvalidData.PolicyRecordNotFound.ToString,
                                                   "InsuranceFileKey",
                                                   BindQuoteRequest.InsuranceFileKey.ToString)
                oSAMErrorCollection.CheckForErrors()
            End If

            oAnyError = oCoreBusiness.CheckTSAndLock(BranchCode:=BindQuoteRequest.BranchCode,
                                                Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
                                                LockValue:=nInsuranceFolderKey,
                                                TStamp:=BindQuoteRequest.QuoteTimeStamp)

            If oAnyError Is Nothing = False Then
                oResponse.STSError = oAnyError
                Return oResponse
            End If

            Dim ds As DataSet = Nothing
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_GetTransactionCurrency")
                cmd.AddInParameter("@InsFileCnt", SqlDbType.Int).Value = BindQuoteRequest.InsuranceFileKey
                ds = con.ExecuteDataSet(cmd, "dr")
            End Using

            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                nCurrencyCodeId = Cast.ToInt32(ds.Tables(0).Rows(0).Item("currency_id"), 0)
            End If

            Try

                con.BeginTransaction()

                If (nCurrencyCodeId <> 0) Then
                    UpdateMulticurrencyValues(con, BindQuoteRequest.InsuranceFileKey, nBranchId, nCurrencyCodeId)
                End If

                ProcessBindQuote(con, oCoreBusiness, BindQuoteRequest,
                                 DirectCast(oResponse, BaseBindQuoteResponseType),
                                 sAutoGeneratedPlanRef,
                                 nDepositTransdetailID)

                nInsuranceFileCnt = BindQuoteRequest.InsuranceFileKey

                con.CommitTransaction()
            Catch ex As Exception
                con.RollbackTransaction()
                If (BindQuoteRequest.TransactionType = "REN") Then
                    Dim oErrException As New SAMErrorException
                    oErrException = DirectCast(ex, Sirius.Architecture.ExceptionHandling.SAMErrorException)
                    If oErrException IsNot Nothing AndAlso oErrException.Errors IsNot Nothing Then
                        For Each oSamErr As Sirius.Architecture.ExceptionHandling.SAMError In DirectCast((oErrException.Errors), Sirius.Architecture.ExceptionHandling.SAMErrorCollection)
                            If DirectCast(oSamErr, Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule).Detail.ToString().Contains("Rounding") Then
                                CreateWorkManagerTaskForRenewal(con, oCoreBusiness, BindQuoteRequest)
                            End If
                            Exit For
                        Next
                    End If
                End If

                oCoreBusiness.UnlockAndGetSAMTS(con, BindQuoteRequest.BranchCode,
                                                CoreBusiness.LockName.InsuranceFolderCnt,
                                                nInsuranceFolderKey,
                                                oResponse.QuoteTimeStamp)
                Throw
            End Try

            oCoreBusiness.UnlockAndGetSAMTS(con, BindQuoteRequest.BranchCode,
                                            CoreBusiness.LockName.InsuranceFolderCnt,
                                            nInsuranceFolderKey,
                                            oResponse.QuoteTimeStamp)

            Dim sDocumentComment As String = ""
            oResponse.Policy = New BaseImplementationTypes.BaseTransactResponseTypePolicy
            GetQuoteDetails(sPolicyNum, nInsuranceFolderCnt, nInsuranceFileCnt,
                            nRiskCnt, nPolicyLinkID, crGrossAmount,
                            crCommissionAmount, crIPTAmount, oResponse.Policy,
                            BindQuoteRequest.TransactionType, sDocumentComment)
            oResponse.Policy.PolicyRef = sPolicyNum
            oResponse.Policy.PremiumDueGross = crGrossAmount
            oResponse.Policy.PremiumDueNet = (crGrossAmount) - (crIPTAmount)
            oResponse.Policy.PremiumDueTax = crIPTAmount
            oResponse.Policy.TotalAnnualTax = crIPTAmount ' TODO where should this come from?
            oResponse.Policy.CommissionAmount = crCommissionAmount
            oResponse.Policy.DocumentComment = sDocumentComment
            oResponse.Policy.AutoGeneratedPlanRef = sAutoPlanRef
            oResponse.Policy.DepositTransDetailID = nDepositTransdetailID

            oResponse.Policy.PolicyLevelTaxesAndFees = New BaseTaxesAndFeesType

            If Not BindQuoteRequest.SkipPolicyLevelTaxesRecalculation Then
                GetPolicyTaxesAndFees(con, nInsuranceFileCnt, BindQuoteRequest.BranchCode, crIPTAmount, crFeeAmount, oResponse.Policy.PolicyLevelTaxesAndFees)
            End If

            'Delete An Exclusive Locks
            DeleteExlusiveLock(con,
                               oCoreBusiness,
                               CoreBusiness.LockName.InsuranceFolderCnt,
                               nInsuranceFolderCnt,
                               BindQuoteRequest.BranchCode)

            Return oResponse

        End Using

    End Function

    Private Sub ValidateBindQuoteRequestStructure(
    ByVal request As BaseBindQuoteRequestType,
    ByRef samErrorCollection As SAMErrorCollection)

        request.Validate(CObj(samErrorCollection))

        samErrorCollection.CheckForErrors()

    End Sub

    Private Sub ValidateRenewalBindQuoteRequestData(
    ByVal con As SiriusConnection,
    ByVal coreBusiness As CoreBusiness,
    ByVal request As BaseBindQuoteRequestType)

        Dim samErrorsCollection As SAMErrorCollection

        ValidatePolicyIsInRenewal(con, request.InsuranceFileKey)

        'Start  - WPR35 - Written Status -Deviation from tech spec
        ' Added validation for write policy as per Adam's comments

        'Start - (Prakash Varghese) - WPR35 - Written Status -Deviation from tech spec
        ' Added validation for write policy as per Adam's comments
        If request.WritePolicy Then

            If request.DataStore.RenewalStatusTypeCode <> RenewalStatusTypeCodes.AwaitingUpdate Then

                samErrorsCollection = New SAMErrorCollection
                samErrorsCollection.AddBusinessRule(
                    SAMBusinessErrors.RenewalStatusTypeIsInvalidForThisAction,
                    SAMBusinessErrors.RenewalStatusTypeIsInvalidForThisAction.ToString,
                    "Attempted write quote for policy in renewal with a renewal status type of " + request.DataStore.RenewalStatusTypeCode +
                    ": Only a renewal status type of " + RenewalStatusTypeCodes.AwaitingUpdate +
                    " is valid for this action")
                samErrorsCollection.CheckForErrors()
            End If
        ElseIf request.AcceptRenewal Then

            If request.DataStore.RenewalStatusTypeCode <> RenewalStatusTypeCodes.AwaitingUpdate AndAlso
                request.DataStore.RenewalStatusTypeCode <> RenewalStatusTypeCodes.AwaitingUpdateWritten Then

                samErrorsCollection = New SAMErrorCollection
                samErrorsCollection.AddBusinessRule(
                    SAMBusinessErrors.RenewalStatusTypeIsInvalidForThisAction,
                    SAMBusinessErrors.RenewalStatusTypeIsInvalidForThisAction.ToString,
                    "Attempted accept quote for policy in renewal with a renewal status type of " + request.DataStore.RenewalStatusTypeCode +
                    ": Only a renewal status type of " + RenewalStatusTypeCodes.AwaitingUpdate +
                    " is valid for this action")
                samErrorsCollection.CheckForErrors()

            End If

        Else

            If request.DataStore.RenewalStatusTypeCode <> RenewalStatusTypeCodes.AwaitingManualReview AndAlso
                request.DataStore.RenewalStatusTypeCode <> RenewalStatusTypeCodes.AwaitingRenewalNoticePrint AndAlso
                request.DataStore.RenewalStatusTypeCode <> RenewalStatusTypeCodes.AwaitingUpdateWritten Then

                samErrorsCollection = New SAMErrorCollection
                samErrorsCollection.AddBusinessRule(
                    SAMBusinessErrors.RenewalStatusTypeIsInvalidForThisAction,
                    SAMBusinessErrors.RenewalStatusTypeIsInvalidForThisAction.ToString,
                    "Attempted bind quote for policy in renewal with a renewal status type of " + request.DataStore.RenewalStatusTypeCode +
                    ": Only renewal status types of " + RenewalStatusTypeCodes.AwaitingRenewalNoticePrint +
                    " and " + RenewalStatusTypeCodes.AwaitingRenewalNoticePrint + " are valid for this action")
                samErrorsCollection.CheckForErrors()

            End If

        End If

    End Sub

    Private Sub GetRenewalStatusData(
    ByRef con As SiriusConnection,
    ByRef request As BaseBindQuoteRequestType)

        ' get the renewal status details
        Dim renewalsStatusData As DataTable = GetRenewalStatus(con, request.InsuranceFileKey)

        If renewalsStatusData IsNot Nothing AndAlso renewalsStatusData.Rows.Count > 0 Then

            request.DataStore.RenewalStatusTypeCode = Cast.ToStringTrim(renewalsStatusData.Rows(0).Item("renewal_status_type_code"), String.Empty)

            ' while we have the data table open grab the rest of the data needed
            request.DataStore.RenewalStatusOriginalInsuranceFileKey = Cast.ToInt32(renewalsStatusData.Rows(0).Item("insurance_file_cnt"), 0)
            request.DataStore.RenewalStatusKey = Cast.ToInt32(renewalsStatusData.Rows(0).Item("renewal_status_cnt"), 0)
            request.DataStore.AgentIsInTransferMode = BooleanDataConvert.ToBoolean(Cast.ToInt32(renewalsStatusData.Rows(0).Item("is_in_transfer_mode"), 0))

            ' if the agent is in transfer mode get the details of the transfer
            If request.DataStore.AgentIsInTransferMode Then
                Dim transferTo As String = Cast.ToStringTrim(renewalsStatusData.Rows(0).Item("transfer_to_party_shortname"), String.Empty)
                If transferTo = String.Empty Then
                    Cast.ToStringTrim(renewalsStatusData.Rows(0).Item("transfer_business_type_description"), String.Empty)
                End If
                request.DataStore.TransferPartyShortname = transferTo
            End If

        Else

            Dim samErrorsCollection As SAMErrorCollection = New SAMErrorCollection

            samErrorsCollection.AddBusinessRule(
                SAMBusinessErrors.NoRenewalStatusRecordExistsForTheSpecifiedPolicy,
                SAMBusinessErrors.NoRenewalStatusRecordExistsForTheSpecifiedPolicy.ToString(),
                "No renewal status record exists for insurance file " + request.InsuranceFileKey.ToString())

            samErrorsCollection.CheckForErrors()

        End If

    End Sub

    Private Sub ValidatePolicyIsInRenewal(
    ByRef con As SiriusConnection,
    ByVal insuranceFilekey As Integer)

        Dim insuranceFileTypeId As Integer
        Dim insuranceFileTypeCode As String = String.Empty

        'Get the insurance File Type to see what type of business we're doing
        GetInsuranceFileType(con, insuranceFilekey, insuranceFileTypeId, insuranceFileTypeCode)
        'WPR35 WRIEEN STATUS
        If insuranceFileTypeCode <> InsuranceFileType.Renewal AndAlso
            insuranceFileTypeCode <> InsuranceFileType.Written Then

            Dim samErrorsCollection As SAMErrorCollection = New SAMErrorCollection

            samErrorsCollection.AddBusinessRule(
                SAMBusinessErrors.InvalidTransactionType,
                SAMBusinessErrors.InvalidTransactionType.ToString,
                "Attempted a renewals call on a policy that isnt in renewals: Current policy type :" + insuranceFileTypeCode)

            samErrorsCollection.CheckForErrors()
        End If

    End Sub

    ''' <summary>
    ''' ValidateBindQuoteRequestData
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="coreBusiness"></param>
    ''' <param name="request"></param>
    ''' <param name="samErrorsCollection"></param>
    ''' <remarks></remarks>
    Private Sub ValidateBindQuoteRequestData(
    ByVal con As SiriusConnection,
    ByVal coreBusiness As CoreBusiness,
    ByVal request As BaseBindQuoteRequestType,
    ByRef samErrorsCollection As SAMErrorCollection)

        Dim nProductID As Integer
        Dim oGetProductRiskOptionRequest As BaseProductRiskOptionValueRequestType = Nothing
        Dim sValue As String = String.Empty
        Dim nInsuranceFileTypeID As Integer
        Dim sInsuranceFileTypeCode As String = String.Empty
        Dim nRenewalStatus As Integer = 0
        Dim nRet As Integer
        Dim oFindInsurance As New bSIRFindInsurance.Form
        Dim nMaxPolicyVersion As Integer = 0
        Dim bIsPolicyAlreadyMadeLive As Boolean
        Dim bIsDuplicateRenewal As Boolean
        ' branch code
        request.SourceId = coreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, request.BranchCode, "BranchCode", samErrorsCollection)

        ' transaction type
        GetTransactionTypeId(con, request.TransactionType, request.DataStore.TransactionTypeId)
        If request.DataStore.TransactionTypeId = 0 Then
            samErrorsCollection.AddInvalidData(
                SAMInvalidData.TransactionTypeIsInvalid,
                SAMInvalidData.TransactionTypeIsInvalid.ToString,
                "TransactionType",
                request.TransactionType)
        End If

        ' insurance file key
        If Not (coreBusiness.CheckInsuranceFile(con, request.InsuranceFileKey)) Then
            samErrorsCollection.AddInvalidData(
                          SAMInvalidData.PolicyRecordNotFound,
                          SAMInvalidData.PolicyRecordNotFound.ToString,
                          "InsuranceFileKey",
                          request.InsuranceFileKey.ToString)
        End If

        If (request.TransactionType.ToUpper = "REN") Then
            ValidateDuplicateRenewal(con, request.InsuranceFileKey, bIsDuplicateRenewal)
            If bIsDuplicateRenewal Then
                samErrorsCollection.AddInvalidData(
                               SAMInvalidData.PolicyAlreadyRenewed,
                               SAMInvalidData.PolicyAlreadyRenewed.ToString,
                               "InsuranceFileKey",
                               request.InsuranceFileKey.ToString)
            End If
        End If

        ValidatePolicyAlreadyMadeLive(con, request.InsuranceFileKey, bIsPolicyAlreadyMadeLive)
        If bIsPolicyAlreadyMadeLive Then
            samErrorsCollection.AddInvalidData(
                           SAMInvalidData.QuoteIsAlreadyMadeLive,
                           SAMInvalidData.QuoteIsAlreadyMadeLive.ToString,
                           "InsuranceFileKey",
                           request.InsuranceFileKey.ToString)
        End If

        If request.SelectedInstalmentQuoteSpecified Then
            ValidateBaseSelectedInstalmentQuoteTypeData(con, coreBusiness, request, samErrorsCollection)
        End If

        If request.PayNowDetails IsNot Nothing Then
            If request.PayNegativePremiumMTABalance Then
                request.PayNowDetails.PaymentStatusCode = "ISS"
            End If
            ValidateBaseReceiptTypeData(con, coreBusiness, samErrorsCollection, request.PayNowDetails)
        End If

        If request.TransactionType = TransactionTypeCode.CancelPolicy Then
            If request.SelectedInstalmentQuoteSpecified Then
                samErrorsCollection.AddBusinessRule(
                    SAMConstants.SAMBusinessErrors.InstalmentsIsNotAValidPaymentOptionWhenCancellingAPolicy,
                    SAMConstants.SAMBusinessErrors.InstalmentsIsNotAValidPaymentOptionWhenCancellingAPolicy.ToString)
            End If
        End If


        With request
            If .WritePolicy Then

                ' Added validation for write policy as per Adam's comments
                GetInsuranceFileType(con, .InsuranceFileKey, nInsuranceFileTypeID, sInsuranceFileTypeCode)
                If .TransactionType <> TransactionTypeCode.Renewals AndAlso
                    sInsuranceFileTypeCode <> InsuranceFileType.Quote Then

                    samErrorsCollection.AddBusinessRule(
                        SAMBusinessErrors.InsuranceFileTypeIsInvalidForWritePolicy,
                        SAMBusinessErrors.InsuranceFileTypeIsInvalidForWritePolicy.ToString,
                        "Only New Business Quote or Renewal Quote is valid for write policy process")
                End If

                nProductID = GetProductID(.InsuranceFileKey, con)
                oGetProductRiskOptionRequest = New BaseProductRiskOptionValueRequestType
                oGetProductRiskOptionRequest.ProducRiskOption = ProductRiskOptions.AllowWrittenStatus
                sValue = GetProductRiskOptions(con, nProductID, oGetProductRiskOptionRequest)

                If sValue <> "1" Then
                    samErrorsCollection.AddBusinessRule(
                                SAMConstants.SAMBusinessErrors.WrittenNotPermittedOnProduct,
                                SAMConstants.SAMBusinessErrors.WrittenNotPermittedOnProduct.ToString())
                End If
                'Begin : PN# 71423 - JP 09/06/2010
                If .OverriddenPolicyNumber IsNot Nothing AndAlso Trim(.OverriddenPolicyNumber) <> "" Then

                    If Not CheckPolicyNumberIsUnique(con, .OverriddenPolicyNumber) Then
                        samErrorsCollection.AddBusinessRule(
                                       SAMConstants.SAMBusinessErrors.PolicyNumberIsAlreadyInUse,
                                     SAMConstants.SAMBusinessErrors.PolicyNumberIsAlreadyInUse.ToString())

                    End If
                End If
                'End : PN# 71423
            ElseIf request.TransactionType = TransactionTypeCode.NewBusiness Then
                GetMaxPolicyVersion(con, .InsuranceFileKey, nMaxPolicyVersion)
                If nMaxPolicyVersion > 1 Then
                    samErrorsCollection.AddBusinessRule(SAMBusinessErrors.InValidInsuranceFileCnt,
                                                        SAMBusinessErrors.InValidInsuranceFileCnt.ToString,
                                                        "Invalid InsuranceFileKey for New Business")
                End If
            End If
        End With
        samErrorsCollection.CheckForErrors()

    End Sub
    'Begin : PN# 71423 - JP 09/06/2010
    Private Function CheckPolicyNumberIsUnique(
                       ByRef con As SiriusConnection,
                       ByVal sPolicyNo As String) As Boolean

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Check_Policy_Number_Is_Unique")

            cmd.AddInParameter("@PolicyNo", SqlDbType.VarChar, 30).Value = sPolicyNo
            cmd.AddInParameter("@IsExisting", SqlDbType.Bit).Direction = ParameterDirection.Output

            con.ExecuteNonQuery(cmd)

            If Cast.ToInt32(cmd.Parameters("@IsExisting").Value, 0) = 1 Then
                Return True
            Else
                Return False
            End If

        End Using

    End Function
    'End : PN# 71423
    ''' <summary>
    ''' This function will validate the  creditcard details
    '''</summary>
    '''<param name="oRequest" type="BaseCreditCardType"></param>
    '''<param name="oErrorCollection" type="SAMErrorCollection"></param>
    '''<remarks></remarks>
    '***********************************************************************************************
    'Note:The credit card ExpiryDate and StartDate should be in the format of "MM/YY" for 
    'Examaple 06/08
    '***********************************************************************************************
    Public Overridable Sub ValidateBaseCreditCardTypeData(ByVal oRequest As BaseCreditCardType, ByRef oErrorCollection As SAMErrorCollection)
        Dim oSAMErrorCollection As New SAMErrorCollection

        'Since the date is verified already, month checking is not needed and it is confirmed with qaurav
        Try
            If Not (Convert.ToInt32(oRequest.ExpiryDate.Substring(0, 2)) <= 12 AndAlso Convert.ToInt32(oRequest.ExpiryDate.Substring(0, 2)) >= 1) Then
                oSAMErrorCollection.AddInvalidData(
                                SAMConstants.SAMInvalidData.InvalidDateFormat,
                                 "Invalid Expiry Date",
                                "Expiry Date")
            End If
            If Convert.ToInt32(oRequest.ExpiryDate.Substring(3, 2)) < Convert.ToInt32(Date.Now.Year.ToString.Substring(2, 2)) OrElse Convert.ToInt32(oRequest.ExpiryDate.Substring(0, 2)) < Date.Now.Month Then

                oSAMErrorCollection.AddInvalidData(
                                SAMConstants.SAMInvalidData.InvalidDateFormat,
                                 "Expiry Date must be >= this month and year’",
                                "Expiry Date")
            End If

        Catch ex As Exception
            oSAMErrorCollection.AddInvalidData(
                                           SAMConstants.SAMInvalidData.InvalidDateFormat,
                                            "Invalid Expiry Date",
                                           "Expiry Date")
        End Try
        If Not String.IsNullOrEmpty(oRequest.StartDate) Then
            'Since the date is verified already, month checking is not needed and it is confirmed with qaurav
            'Dim dtStartDate As DateTime
            'dtStartDate = Convert.ToDateTime(oRequest.StartDate)
            Try

                If Not (Convert.ToInt32(oRequest.StartDate.Substring(0, 2)) <= 12 AndAlso Convert.ToInt32(oRequest.StartDate.Substring(0, 2)) >= 1) Then
                    oSAMErrorCollection.AddInvalidData(
                                    SAMConstants.SAMInvalidData.InvalidDateFormat,
                                     "Invalid Start Date",
                                    "Start Date")

                End If
            Catch ex As Exception
                oSAMErrorCollection.AddInvalidData(
                                                   SAMConstants.SAMInvalidData.InvalidDateFormat,
                                                    "Invalid Start Date",
                                                   "Start Date")
            End Try
        End If

        'Note that PIN code length, along with several other credit card validation rules, is 
        'configurable for the media type issuer but is out of scope at the moment. Add a suitable 
        'comment to this effect.
    End Sub

    Private Sub ValidateBaseCreditCardTypeData(
    ByVal con As SiriusConnection,
    ByVal request As BaseCreditCardType,
    ByRef samErrorsCollection As SAMErrorCollection)

        Dim pattern As String = "[0-1][0-9][/\\][0-9][0-9]"
        If Not String.IsNullOrEmpty(request.ExpiryDate) Then
            If Not Regex.IsMatch(request.ExpiryDate, pattern) Then

                samErrorsCollection.AddInvalidData(
                    SAMConstants.SAMInvalidData.InvalidFormat,
                    SAMConstants.SAMInvalidData.InvalidFormat.ToString,
                    "ExpiryDate:ExpectedFormat:MM/YY")

            End If
        End If
        If Not String.IsNullOrEmpty(request.StartDate) Then
            If Not Regex.IsMatch(request.StartDate, pattern) Then

                samErrorsCollection.AddInvalidData(
                    SAMConstants.SAMInvalidData.InvalidFormat,
                    SAMConstants.SAMInvalidData.InvalidFormat.ToString,
                    "StartDate:ExpectedFormat:MM/YY")

            End If
        End If

    End Sub

    Private Sub ValidateBaseAddressTypeData(
    ByVal con As SiriusConnection,
    ByVal coreBusiness As CoreBusiness,
    ByVal request As BaseAddressType,
    ByRef samErrorsCollection As SAMErrorCollection)

        ' validate the country code
        If Not String.IsNullOrEmpty(request.CountryCode) Then
            Dim countryId As Integer =
                coreBusiness.GetAndValidateListItemFromCode(
                    Core.STSListType.PMLookup,
                    PMLookupTable.Country,
                    request.CountryCode,
                    "CountryCode",
                    samErrorsCollection)
        End If

    End Sub

    Private Sub ValidateBaseSelectedInstalmentQuoteTypeData(
    ByVal con As SiriusConnection,
    ByVal coreBusiness As CoreBusiness,
    ByVal request As BaseBindQuoteRequestType,
    ByRef samErrorsCollection As SAMErrorCollection)

        ' scheme no / scheme version / pfrf_id
        If Not IsValidScheme(con, request.SelectedSchemeNo, request.SelectedSchemeVersion, request.PFRF_ID) Then
            samErrorsCollection.AddInvalidData(
            SAMConstants.SAMInvalidData.SelectedInstalmentQuoteDataIsInvalid,
            SAMConstants.SAMInvalidData.SelectedInstalmentQuoteDataIsInvalid.ToString,
            "SelectedSchemeNo/SelectedSchemeVersion/PFRF_ID")
        End If

        ' week day
        If request.WeekDay > 7 OrElse request.WeekDay < 0 Then
            samErrorsCollection.AddInvalidData(
                       SAMConstants.SAMInvalidData.ValueOutOfAcceptableRange,
                       SAMConstants.SAMInvalidData.ValueOutOfAcceptableRange.ToString,
                       "WeekDay:AcceptableRange:1-7", request.WeekDay.ToString)
        End If

        ' month day
        If request.MonthDay > 31 OrElse request.MonthDay < 0 Then
            samErrorsCollection.AddInvalidData(
                       SAMConstants.SAMInvalidData.ValueOutOfAcceptableRange,
                       SAMConstants.SAMInvalidData.ValueOutOfAcceptableRange.ToString,
                       "MonthDay:AcceptableRange:1-31", request.MonthDay.ToString)
        End If

        ' override interest rate
        If request.OverrideInterestRate > 100 OrElse request.OverrideInterestRate < -1 Then
            samErrorsCollection.AddInvalidData(
                       SAMConstants.SAMInvalidData.ValueOutOfAcceptableRange,
                       SAMConstants.SAMInvalidData.ValueOutOfAcceptableRange.ToString,
                       "OverrideInterestRate:AcceptableRanges:(0-100=OverrideRates)(-1=NoOverride)", request.OverrideInterestRate.ToString)
        End If

        ' credit card
        If request.CreditCard IsNot Nothing And request.TransactionType <> TransactionTypeCode.Renewals Then
            ValidateBaseCreditCardTypeData(con, request.CreditCard, samErrorsCollection)
        End If

        ' bank address 
        If request.BankAddress IsNot Nothing Then
            ValidateBaseAddressTypeData(con, coreBusiness, request.BankAddress, samErrorsCollection)
        End If

    End Sub

    Private Function IsValidScheme(
    ByRef con As SiriusConnection,
    ByVal schemeNo As Integer,
    ByVal schemeVersion As Integer,
    ByVal pfrfId As Integer) As Boolean

        Dim numberOfMatchingValidSchemes As Integer

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Validate_Scheme_Details")

            cmd.AddInParameter("@schemeNo", SqlDbType.Int).Value = schemeNo
            cmd.AddInParameter("@schemeVersion", SqlDbType.Int).Value = schemeVersion
            cmd.AddInParameter("@pfrf_Id", SqlDbType.Int).Value = pfrfId

            numberOfMatchingValidSchemes = Cast.ToInt32(con.ExecuteScalar(cmd), 0)

        End Using

        Return (numberOfMatchingValidSchemes > 0)

    End Function

    Private Sub ProcessBindQuote(
    ByRef con As SiriusConnection,
    ByRef coreBusiness As CoreBusiness,
    ByVal request As BaseBindQuoteRequestType,
     ByRef oResponse As BaseBindQuoteResponseType,
     Optional ByRef sAutoGeneratedPlanRef As String = "",
     Optional ByRef iDepositTransdetailID As Integer = 0)

        Dim samErrorsCollection As New SAMErrorCollection
        Dim response As SAMForInsuranceImplementationTypes.BindQuoteResponseType = Nothing

        ValidateBindQuoteRequestStructure(request, samErrorsCollection)

        ValidateBindQuoteRequestData(con, coreBusiness, request, samErrorsCollection)

        If request.TransactionType = TransactionTypeCode.NewBusiness Then

            'Start -(Tech Spec - WPR35 - Written Status.doc) - (5.1.3.5)
            If request.WritePolicy Then
                ProcessNewBusinessWrite(con, coreBusiness, request)
            Else
                ProcessNewBusinessBindQuote(con, coreBusiness, request, sAutoGeneratedPlanRef, iDepositTransdetailID, DirectCast(oResponse, BaseBindQuoteResponseType))
            End If
            'End - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.5)

        ElseIf request.TransactionType = TransactionTypeCode.MTA Or
            request.TransactionType = TransactionTypeCode.CancelPolicy Or
            request.TransactionType = TransactionTypeCode.ReinstatePolicy Then

            ProcessMTABindQuote(con, coreBusiness, request, DirectCast(oResponse, BaseBindQuoteResponseType), r_nDepositTransdetailID:=iDepositTransdetailID)

        ElseIf request.TransactionType = TransactionTypeCode.Renewals Then
            GetRenewalStatusData(con, request)
            ValidateRenewalBindQuoteRequestData(con, coreBusiness, request)

            'Start  - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.5)
            If request.WritePolicy Then
                ProcessRenewalWriteBindQuote(con, coreBusiness, request)
            Else
                ProcessRenewalBindQuote(con, coreBusiness, request, DirectCast(oResponse, BaseBindQuoteResponseType), sAutoGeneratedPlanRef, r_nDepositTransdetailID:=iDepositTransdetailID)
            End If
            'End  - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.5)
        End If

    End Sub
#Region "Private Method-ProcessNewBusinessWrite"
    'Start- (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.6)
    ''' <summary>
    ''' This method is responsible for NB policy write process
    '''</summary>
    '''<param name="con">An object of  a class SiriusConnection</param>
    '''<param name="oCoreBusiness">An object of  a class CoreBusiness</param>
    ''' <param name="oRequest" > An object of  a class BaseBindQuoteRequestType</param>
    '''<remarks></remarks>
    Private Sub ProcessNewBusinessWrite(
                    ByRef con As SiriusConnection,
                    ByRef oCoreBusiness As CoreBusiness,
                    ByRef oRequest As BaseBindQuoteRequestType)

        Dim oListRisksBusiness As bSIRListRisks.Business = CreateAndInitialiseListRisksBusiness(con, oRequest.BranchCode)
        Dim sNewPolicyNumber As String = String.Empty
        Try

            ' get insurance file details
            GetInsuranceFileDetails(con, oRequest, oListRisksBusiness)

            ' validate it is ok to proceed
            ValidatePolicy(con,
                oRequest.TransactionType,
                oRequest.InsuranceFileKey,
                oRequest.DataStore.IsTrueMonthlyPolicy,
                oRequest.DataStore.TotalAgentPremium,
                oRequest.DataStore.TotalAgentCommission,
                oRequest.DataStore.PolicyHasAgent,
                oRequest.DataStore.TotalPremiumAmount)

            'Begin : PN# 71423 - JP 09/06/2010
            If oRequest.OverriddenPolicyNumber IsNot Nothing AndAlso Trim(oRequest.OverriddenPolicyNumber) <> "" Then
                'Pass the Overridden Policy Number 
                sNewPolicyNumber = oRequest.OverriddenPolicyNumber
            End If
            'End : PN# 71423

            UpdatePolicyDetails(con:=con, branchCode:=oRequest.BranchCode, insuranceFileKey:=oRequest.InsuranceFileKey,
                               transactionType:=oRequest.TransactionType, bProcessAsWritten:=oRequest.WritePolicy, sNewPolicyNumber:=sNewPolicyNumber)


            CreateWrittenWorkManagerTask(con,
                                         oCoreBusiness,
                                         oRequest,
                                         sNewPolicyNumber)
        Finally

            If oListRisksBusiness IsNot Nothing Then
                oListRisksBusiness.Dispose()
                oListRisksBusiness = Nothing
            End If
        End Try
    End Sub
    'End  - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.6)
#End Region

#Region "Private Method-CreateWrittenWorkManagerTask"
    'Start  (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.9)
    ''' <summary>
    ''' This method is responsible for creatig work manger task during policy write process
    '''</summary>
    '''<param name="con">An object of  a class SiriusConnection</param>
    '''<param name="oCoreBusiness">An object of  a class CoreBusiness</param>
    ''' <param name="oRequest" > An object of  a class BaseBindQuoteRequestType</param>
    ''' <param name="sNewPolicyNumber"> A reference parameter of type string to get the new policy number</param>
    '''<remarks></remarks>
    Private Sub CreateWrittenWorkManagerTask(
            ByRef con As SiriusConnection,
            ByRef oCoreBusiness As CoreBusiness,
            ByRef oRequest As BaseBindQuoteRequestType,
            ByVal sNewPolicyNumber As String)

        Const kDefaultLeadDays As Integer = 7

        Dim sOptionValue As String = String.Empty
        Dim sPartyShortName As String = String.Empty
        Dim sTaskDescription As String = String.Empty
        Dim iReminderTaskGroupID As Integer
        Dim iReminderUserGroupID As Integer
        Dim iReminderTaskID As Integer
        Dim dtDueDate As Date
        Dim iLeadDays As Integer
        Dim iTaskInstanceCnt As Integer
        Dim oGetProductRiskOptionRequest As BaseProductRiskOptionValueRequestType = Nothing

        sPartyShortName = oRequest.DataStore.PartyShortname
        oGetProductRiskOptionRequest = New BaseProductRiskOptionValueRequestType

        oGetProductRiskOptionRequest.ProducRiskOption = ProductRiskOptions.WrittenTaskManagerDays
        sOptionValue = GetProductRiskOptions(con, oRequest.DataStore.ProductId, oGetProductRiskOptionRequest)
        If String.IsNullOrEmpty(sOptionValue) OrElse
            Not Integer.TryParse(sOptionValue, iLeadDays) Then
            iLeadDays = kDefaultLeadDays
        End If

        If oRequest.DataStore.CoverStartDate < Date.Now Then
            dtDueDate = DateTime.Now.AddDays(iLeadDays)
        Else
            dtDueDate = oRequest.DataStore.CoverStartDate.AddDays(iLeadDays)
        End If

        oGetProductRiskOptionRequest.ProducRiskOption = ProductRiskOptions.WrittenReminderUserGroup
        sOptionValue = GetProductRiskOptions(con, oRequest.DataStore.ProductId, oGetProductRiskOptionRequest)
        If String.IsNullOrEmpty(sOptionValue) OrElse
            Not Integer.TryParse(sOptionValue, iReminderUserGroupID) OrElse
            iReminderUserGroupID = 0 Then
            iReminderUserGroupID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.UserGroup, UserGroup.SystemAdministrator)
        End If

        oGetProductRiskOptionRequest.ProducRiskOption = ProductRiskOptions.WrittenReminderTaskGroup
        sOptionValue = GetProductRiskOptions(con, oRequest.DataStore.ProductId, oGetProductRiskOptionRequest)
        If String.IsNullOrEmpty(sOptionValue) OrElse
            Not Integer.TryParse(sOptionValue, iReminderTaskGroupID) OrElse
            iReminderTaskGroupID = 0 Then
            If oRequest.TransactionType = TransactionTypeCode.NewBusiness Then
                iReminderTaskGroupID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.TaskGroup, TaskGroup.UnderwritingMaintenance)
            ElseIf oRequest.TransactionType = TransactionTypeCode.Renewals Then
                iReminderTaskGroupID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.TaskGroup, TaskGroup.UnderwritingRenewal)
            End If
        End If

        If oRequest.TransactionType = TransactionTypeCode.NewBusiness Then
            iReminderTaskID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Task, Task.UnderwritingNewBusiness)
            sTaskDescription = "WRITTEN POLICY - " + sNewPolicyNumber
        ElseIf oRequest.TransactionType = TransactionTypeCode.Renewals Then
            iReminderTaskID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Task, Task.RenewalProcess)
            sTaskDescription = "WRITTEN RENEWAL - " + sNewPolicyNumber
        End If

        'Create work manager task 
        CreateTaskInstance(con, iTaskInstanceCnt, iReminderTaskGroupID, iReminderTaskID,
                        sPartyShortName, dtDueDate, iReminderUserGroupID, Nothing,
                        sTaskDescription, PMEWrkManTaskStatus.pmeWMTSNew, 1, Date.Today,
                        _SiriusUser.UserID, Date.Today, _SiriusUser.UserID, 1, String.Empty)

    End Sub
    'End - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.9)
#End Region
    'Start - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
    ''' <summary>
    ''' This method is responsible for renewal write process
    '''</summary>
    '''<param name="con">An object of  a class SiriusConnection</param>
    '''<param name="oCoreBusiness">An object of  a class CoreBusiness</param>
    ''' <param name="oRequest" > An object of  a class BaseBindQuoteRequestType</param>
    '''<remarks></remarks>
    Private Sub ProcessRenewalWriteBindQuote(
            ByRef con As SiriusConnection,
            ByRef oCoreBusiness As CoreBusiness,
            ByVal oRequest As BaseBindQuoteRequestType)

        Dim oListRisksBusiness As bSIRListRisks.Business = CreateAndInitialiseListRisksBusiness(con, oRequest.BranchCode)
        Dim sProductOptionValue As String = String.Empty
        Dim oGetProductRiskOptionRequest As BaseProductRiskOptionValueRequestType = Nothing

        Try

            ' get insurance file details
            GetInsuranceFileDetails(con, oRequest, oListRisksBusiness)

            'Check the product option ChangePolicyNumberAtRenewalAutomatically
            oGetProductRiskOptionRequest = New BaseProductRiskOptionValueRequestType

            oGetProductRiskOptionRequest.ProducRiskOption = ProductRiskOptions.ChangePolicyNumberAtRenewalAutomatically
            sProductOptionValue = GetProductRiskOptions(con, oRequest.DataStore.ProductId, oGetProductRiskOptionRequest)
            If Not String.IsNullOrEmpty(sProductOptionValue) AndAlso Trim(sProductOptionValue) = "1" Then
                'Generate new InsuranceFileRef and update the database
                ChangePolicyNumber(con, BusinessType.Policy,
                                    Convert.ToInt16(oRequest.DataStore.SourceId.ToString()),
                                    oRequest.DataStore.ProductId,
                                    oRequest.DataStore.LeadAgnetKey,
                                    oRequest.DataStore.CoverStartDate,
                                    oRequest.InsuranceFileKey,
                                    oRequest.DataStore.InsuranceRef,
                                    oRequest.DataStore.PartyKey)
            End If

            ' validate it is ok to proceed
            ValidatePolicy(con,
                    oRequest.TransactionType,
                    oRequest.InsuranceFileKey,
                    oRequest.DataStore.IsTrueMonthlyPolicy,
                    oRequest.DataStore.TotalAgentPremium,
                    oRequest.DataStore.TotalAgentCommission,
                    oRequest.DataStore.PolicyHasAgent,
                    oRequest.DataStore.TotalPremiumAmount)


            UpdatePolicyDetails(
                con,
                oRequest.BranchCode,
                oRequest.InsuranceFileKey,
                oRequest.TransactionType,
                oRequest.WritePolicy)


            UpdateRenewalStatus(
                oCoreBusiness,
                con,
                oRequest.InsuranceFileKey,
                RenewalStatusTypeCodes.AwaitingUpdateWritten)

            CreateWrittenWorkManagerTask(con,
                             oCoreBusiness,
                             oRequest,
                             oRequest.DataStore.InsuranceRef)

        Finally

            If oListRisksBusiness IsNot Nothing Then
                oListRisksBusiness.Dispose()
                oListRisksBusiness = Nothing
            End If
        End Try

    End Sub
    'End  - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
    'Private Sub ProcessPopulateRatingSections(ByVal con As SiriusConnection, _
    'ByVal branchCode As String, _
    'ByVal insuranceFileKey As Integer, _
    'ByVal insuranceFolderKey As Integer, _
    'ByVal riskKey As Integer, _
    'ByVal transactionType As String)

    '    Dim perilAllocationBusiness As bSirPerilAllocation.Business = CreateAndInitialisePerilAllocationBusiness(con, branchCode)

    '    Try

    '        perilAllocationBusiness.InsuranceFileCnt = insuranceFileKey
    '        perilAllocationBusiness.InsuranceFolderCnt = insuranceFolderKey
    '        perilAllocationBusiness.RiskID = riskKey
    '        perilAllocationBusiness.TransactionType = transactionType

    '        Dim ratingsObject As Object = Nothing

    '        perilAllocationBusiness.PopulateRatingSections(ratingsObject, False, 0)

    '        perilAllocationBusiness.UpdateRisk()

    '    Finally

    '        SAMFunc.DestroyCOMInterop(CObj(perilAllocationBusiness))

    '    End Try

    Private Sub ProcessCopyMTARisks(
     ByVal con As SiriusConnection,
     ByVal branchCode As String,
     ByVal insuranceFileKey As Integer, Optional ByRef TransactionType As TransactionType = Nothing,
     Optional ByVal Is_SAM_Copy_Quote As Boolean = False, Optional ByVal bCalledViaAddMTA As Boolean = False, Optional ByVal bSetUnQuoted As Boolean = False, Optional ByVal bCopyRiskOnMTA As Boolean = False)
        'Added  Parameter "TransactionType" in sub method ProcessCopyMTARisks(SAM Gap done by Vijayakumar as per discussed with Gaurav on 06-Nov-2008)
        Dim listRisksBusiness As bSIRListRisks.Business = CreateAndInitialiseListRisksBusiness(con, branchCode)

        listRisksBusiness.TransactionType = "MTA"
        ' copy uncopied mta risks
        CopyMTARisks(con, listRisksBusiness, insuranceFileKey, TransactionType, Is_SAM_Copy_Quote, bCalledViaAddMTA, bSetUnQuoted, bCopyRiskOnMTA)

        If listRisksBusiness IsNot Nothing Then
            listRisksBusiness.Dispose()
            listRisksBusiness = Nothing
        End If
    End Sub
    ''' <summary>
    ''' Wrappers the back ofice component to copy a single risk
    ''' </summary>
    ''' <param name="con">Database Connection</param>
    ''' <param name="branchCode">Processing Branch</param>
    ''' <param name="insuranceFileKey">Insurance file</param>
    ''' <param name="riskCnt">Risk that must be copied</param>
    ''' <param name="newRiskCnt">The copied risk cnt</param>
    ''' <param name="TransactionType">Transaction type being executed</param>
    ''' <remarks>Wrappers and cleansup the COM component</remarks>
    Private Sub ProcessCopyMTARisk(
 ByVal con As SiriusConnection,
 ByVal branchCode As String,
 ByVal insuranceFileKey As Integer,
 ByVal riskCnt As Integer,
ByRef newRiskCnt As Integer,
Optional ByRef TransactionType As TransactionType = Nothing)
        Dim listRisksBusiness As bSIRListRisks.Business = CreateAndInitialiseListRisksBusiness(con, branchCode)

        ' copy uncopied mta risks
        CopyMTARisk(con, listRisksBusiness, insuranceFileKey, riskCnt, newRiskCnt, TransactionType)

        If listRisksBusiness IsNot Nothing Then
            listRisksBusiness.Dispose()
            listRisksBusiness = Nothing
        End If
    End Sub
    Private Sub ProcessRenewalBindQuote(
                    ByRef con As SiriusConnection,
                    ByRef coreBusiness As CoreBusiness,
                    ByVal request As BaseBindQuoteRequestType,
                ByRef oResponse As BaseBindQuoteResponseType,
                Optional ByRef o_sAutoGeneratedPlanRef As String = "", Optional ByRef r_nDepositTransdetailID As Integer = 0)

        Dim listRisksBusiness As bSIRListRisks.Business = CreateAndInitialiseListRisksBusiness(con, request.BranchCode)

        Try

            ' get insurance file details
            GetInsuranceFileDetails(con, request, listRisksBusiness)

            If request.AcceptRenewal Then
                Dim sProductOptionValue As String = String.Empty
                'Start - (Prakash Varghese) - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
                If request.DataStore.RenewalStatusTypeCode <> RenewalStatusTypeCodes.AwaitingUpdateWritten Then
                    'End - (Prakash Varghese) - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
                    'Check whether the product option ChangePolicyNumberAtRenewalAutomatically
                    sProductOptionValue = GetAndValidateDescriptionById(con, PMLookupTable.Product, "Change_Ren_Policy_No_Auto", "Product_ID", request.DataStore.ProductId.ToString())
                    If Not String.IsNullOrEmpty(sProductOptionValue) Then
                        If Trim(sProductOptionValue) = "1" And request.SkipNewPolicyNumber = False Then
                            'Generate new InsuranceFileRef and update the database
                            ChangePolicyNumber(con, BusinessType.Policy,
                                                Convert.ToInt16(request.DataStore.SourceId.ToString()),
                                                request.DataStore.ProductId,
                                                request.DataStore.LeadAgnetKey,
                                                request.DataStore.CoverStartDate,
                                                request.InsuranceFileKey,
                                                request.DataStore.InsuranceRef,
                                                request.DataStore.PartyKey)
                        End If
                    End If

                End If

                If request.BankGuaranteeDetails IsNot Nothing Then
                    If request.BankGuaranteeDetails.BGKey <> 0 Then
                        ValidateBankGuaranteeTypeData(con, request.BankGuaranteeDetails, request)
                    End If
                End If

                If Not request.SkipPolicyLevelTaxesRecalculation Then
                    ' recalculate policy fees and policy taxes
                    ProcessPolicyLevelFeesAndTaxes(con, request, listRisksBusiness)
                End If

                If request.DataStore.PolicyHasAgent Then
                    ' recalculate agent commission 
                    ProcessAgentCommission(con, request, listRisksBusiness)
                End If

                ' Get the total premium amount on the policy including all fees and taxes
                ' This code was previously present after the ProcessPaymentTerms method. Since Validatecashdeposit needs
                ' TotalPremiumAmount, this code has been moved up.
                request.DataStore.TotalPremiumAmount = GetTotalPremiumAmount(con, request.InsuranceFileKey)
                DeleteUnselectedRisks(con, request.BranchCode, request.InsuranceFileKey)
                ' validate it is ok to proceed
                ValidatePolicy(con,
                    request.TransactionType,
                    request.InsuranceFileKey,
                    request.DataStore.IsTrueMonthlyPolicy,
                    request.DataStore.TotalAgentPremium,
                    request.DataStore.TotalAgentCommission,
                    request.DataStore.PolicyHasAgent,
                    request.DataStore.TotalPremiumAmount)

                If request.SelectedCashDeposit IsNot Nothing Then
                    ValidateCashDeposit(con, request, coreBusiness)
                End If
                If request IsNot Nothing AndAlso oResponse IsNot Nothing Then
                    If request.IsMarketPlacePolicy Then
                        If request.DataStore.TotalPremiumAmount <> request.MarketPlaceTotalPremium Then
                            Dim STSErrorEx As STSErrorPublisher = New STSErrorPublisher(STSErrorCode:=STSErrorCodes.RatingRulesDeclined, Description:="Rating rules declined", Detail:="Rating rules declined")
                            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "ProcessPolicyV2", "Gross premium changed from initial quote", True)
                            Exit Sub
                        End If
                    End If
                End If
                ' TODO : MEvans : Override Posting Period if Allowed

                ' ProcessPaymentTerms
                ' Either Invoice
                ' Pay Now 
                ' Instalments 
                ProcessPaymentTerms(con, coreBusiness, request)

                ' get the total premium amount on the policy including all fees and taxes
                request.DataStore.TotalPremiumAmount = GetTotalPremiumAmount(con, request.InsuranceFileKey)

                UpdatePolicyDetails(
                   con:=con,
                   branchCode:=request.BranchCode,
                  insuranceFileKey:=request.InsuranceFileKey,
                   transactionType:=request.TransactionType, SkipNewPolicyNumber:=request.SkipNewPolicyNumber)


                UpdatePolicyPaymentDetails(
                    listRisksBusiness,
                    request.InsuranceFileKey,
                    request.DataStore.PaymentMethod,
                    request.PayTrueMonthlyPolicyMTAPremiumOnRenewal,
                    request.DataStore.PaymentTerms,
                    request.DataStore.CollectionFrequency)

                ProcessMakeLive(listRisksBusiness, request, con)

                If request.BankGuaranteeDetails IsNot Nothing AndAlso request.PaymentMethod = PaymentMethodType.BankGuarantee Then
                    If request.BankGuaranteeDetails.BGKey <> 0 Then
                        ProcessBankGuarantee(con, request.BankGuaranteeDetails, request)
                    End If
                End If



                If Not request.AcceptRenewal Then

                    UpdateRenewalStatus(
                        coreBusiness,
                        con,
                        request.InsuranceFileKey,
                        RenewalStatusTypeCodes.AwaitingRenewalNoticePrint)

                    GenerateRenewalStatusChangeClientEmail(
                        con,
                        coreBusiness,
                        request,
                        RenewalEmailType.Invite)

                Else

                    ProcessAcceptRenewal(
                    con,
                    coreBusiness,
                    request, DirectCast(oResponse, BaseBindQuoteResponseType))

                End If

                If request.DataStore.PaymentMethod = PolicyPaymentMethod.Instalments Then
                    ProcessRenewalInstalments(con, request, r_nDepositTransdetailID:=r_nDepositTransdetailID)
                Else
                    ProcessRenewalInstalments(con, request, True, o_sAutoGeneratedPlanRef)
                End If
            End If
        Finally

            If listRisksBusiness IsNot Nothing Then
                listRisksBusiness.Dispose()
                listRisksBusiness = Nothing
            End If
        End Try

    End Sub

    Private Overloads Sub UpdateRenewalStatus(
ByVal coreBusiness As CoreBusiness,
ByVal con As SiriusConnection,
ByVal insuranceFileKey As Integer,
ByVal renewalStatusTypeCode As String)

        ' renewal status type id 
        Dim renewalStatusTypeKey As Integer = coreBusiness.GetAndValidateListItemFromCode(
            Core.STSListType.PMLookup,
            PMLookupTable.RenewalStatusType,
            renewalStatusTypeCode,
            "RENEWALSTATUSTYPECODE")

        If renewalStatusTypeCode = RenewalStatusTypeCodes.AwaitingRenewalNoticePrint Then
            UpdateRenewalStatus(con, insuranceFileKey, renewalStatusTypeKey, 0)
        Else
            UpdateRenewalStatus(con, insuranceFileKey, renewalStatusTypeKey)
        End If

    End Sub

    Private Overloads Sub UpdateRenewalStatus(
    ByVal con As SiriusConnection,
    ByVal insuranceFileKey As Integer,
    ByVal renewalStatusTypeKey As Integer)

        UpdateRenewalStatus(con, insuranceFileKey, renewalStatusTypeKey, -1)

    End Sub

    Private Overloads Sub UpdateRenewalStatus(
    ByVal con As SiriusConnection,
    ByVal insuranceFileKey As Integer,
    ByVal renewalStatusTypeKey As Integer,
    ByVal isInvitePrinted As Integer)

        ' update the claim receipt items associated recovery entry
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Renewal_Status_Update")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            cmd.AddInParameter("@renewal_status_type_id", SqlDbType.Int).Value = renewalStatusTypeKey

            If isInvitePrinted <> -1 Then
                cmd.AddInParameter("@is_invite_printed", SqlDbType.Int).Value = isInvitePrinted
            End If

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Sub ProcessNewBusinessBindQuote(
        ByRef con As SiriusConnection,
        ByRef coreBusiness As CoreBusiness,
        ByRef request As BaseBindQuoteRequestType,
        Optional ByRef sAutoGeneratedPlanRef As String = "",
        Optional ByRef iDepositTransdetailID As Integer = 0,
        Optional ByRef oResponse As BaseBindQuoteResponseType = Nothing)   'WPR 33-75 ADDED


        Dim listRisksBusiness As bSIRListRisks.Business = CreateAndInitialiseListRisksBusiness(con, request.BranchCode)
        Dim iAccountId As Integer = 0
        Try

            ' get insurance file details
            GetInsuranceFileDetails(con, request, listRisksBusiness)

            If request.BankGuaranteeDetails IsNot Nothing Then
                If request.BankGuaranteeDetails.BGKey <> 0 Then
                    ValidateBankGuaranteeTypeData(con, request.BankGuaranteeDetails, request)
                End If
            End If

            If Not request.SkipPolicyLevelTaxesRecalculation Then
                ' recalculate policy fees and policy taxes
                ProcessPolicyLevelFeesAndTaxes(con, request, listRisksBusiness)
            End If

            If request.DataStore.PolicyHasAgent Then
                ' recalculate agent commission 
                ProcessAgentCommission(con, request, listRisksBusiness)
            End If

            ' Get the total premium amount on the policy including all fees and taxes
            ' This code was previously present after the ProcessPaymentTerms method. Since Validatecashdeposit needs
            ' TotalPremiumAmount, this code has been moved up.
            request.DataStore.TotalPremiumAmount = GetTotalPremiumAmount(con, request.InsuranceFileKey)
            DeleteUnselectedRisks(con, request.BranchCode, request.InsuranceFileKey)
            ' validate it is ok to proceed
            ValidatePolicy(con,
                request.TransactionType,
                request.InsuranceFileKey,
                request.DataStore.IsTrueMonthlyPolicy,
                request.DataStore.TotalAgentPremium,
                request.DataStore.TotalAgentCommission,
                request.DataStore.PolicyHasAgent,
                request.DataStore.TotalPremiumAmount)

            If request.SelectedCashDeposit IsNot Nothing Then
                ValidateCashDeposit(con, request, coreBusiness)
            End If

            If request IsNot Nothing AndAlso oResponse IsNot Nothing Then
                If request.IsMarketPlacePolicy Then
                    If request.DataStore.TotalPremiumAmount <> request.MarketPlaceTotalPremium Then
                        Dim STSErrorEx As STSErrorPublisher = New STSErrorPublisher(STSErrorCode:=STSErrorCodes.RatingRulesDeclined, Description:="Rating rules declined", Detail:="Rating rules declined")
                        STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "ProcessPolicyV2", "Gross premium changed from initial quote", True)
                        Exit Sub
                    End If
                End If
            End If

            ' TODO : MEvans : Override Posting Period if Allowed

            ' ProcessPaymentTerms
            ' Either Invoice
            ' Pay Now 
            ' Instalments 
            ProcessPaymentTerms(con, coreBusiness, request)

            ' get the total premium amount on the policy including all fees and taxes
            request.DataStore.TotalPremiumAmount = GetTotalPremiumAmount(con, request.InsuranceFileKey)

            UpdatePolicyDetails(con:=con, branchCode:=request.BranchCode, insuranceFileKey:=request.InsuranceFileKey, transactionType:=request.TransactionType, SkipNewPolicyNumber:=request.SkipNewPolicyNumber)

            UpdatePolicyPaymentDetails(listRisksBusiness, request.InsuranceFileKey, request.DataStore.PaymentMethod, request.PayTrueMonthlyPolicyMTAPremiumOnRenewal, request.DataStore.PaymentTerms, request.DataStore.CollectionFrequency)

            ProcessMakeLive(listRisksBusiness, request, con)

            If request.BankGuaranteeDetails IsNot Nothing AndAlso request.PaymentMethod = PaymentMethodType.BankGuarantee Then
                If request.BankGuaranteeDetails.BGKey <> 0 Then
                    ProcessBankGuarantee(con, request.BankGuaranteeDetails, request)
                End If
            End If

            If Not request.NoTrans Then
                ProcessAccounts(con, request, iAccountId:=iAccountId)
            End If

            If request.DataStore.PaymentMethod = PolicyPaymentMethod.Instalments Then
                'WPR 33-75 ADDED
                ' ProcessNBInstalments(con, request)
                ProcessNBInstalments(con, request, sAutoGeneratedPlanRef, iDepositTransdetailID)
                'WPR 33-75 END
            End If

        Finally

            If listRisksBusiness IsNot Nothing Then
                listRisksBusiness.Dispose()
                listRisksBusiness = Nothing
            End If
        End Try

    End Sub

    Private Sub ProcessRenewalInstalments(
          ByVal con As SiriusConnection,
          ByVal request As BaseBindQuoteRequestType,
          Optional ByVal onlyDeleteExistingInstalmentPlan As Boolean = False,
          Optional ByRef o_sAutoGeneratedPlanRef As String = "", Optional ByRef r_nDepositTransdetailID As Integer = 0)

        Const NoDeposit As Boolean = False

        Dim premiumFinanceBusiness As bSIRPremiumFinance.Business =
         CreateAndInitialisePremiumFinanceBusiness(con, request.BranchCode)

        Dim iCreatePartyBankRecord As Int16 = 1

        Try

            ' ************************************
            ' This method was extracted from the following components
            ' ************************************

            ' iPMUListRisks - Refresh Quotes
            '       - Calculate And Validate
            ' iPMUListRisk - SaveQuote
            '       - Insert or Update Premium Finance Plan
            ' iPMBFinancePlanMaint - load
            '       - get Single Premium Finance Plan

            ' iPMBFinancePlanMaint - cmd_Save
            '       - bSIRPRemiumFinance.UpdateExistingRecord
            '       - bSIRPRemiumFinance.SaveInstalmentsPlanMediaTypeDetails
            '       - bSIRPremiumFinance.DeletePFTransID 
            '       - bSIRPremiumFinance.InsertPFTransID
            '       - bSIRPremiumFinance.AddPartners ( STAR GATE ONLY SO IGNORE)
            '       - bSIRPremiumFinance.CreateEvent

            ' iPMBFinancePlanMaint - cmd_Transact
            '       - bSIRPremiumFinance.CheckAllocationAgainstPolicy
            '       - ValidateDataEntry
            '       - bSirMediaTypeValidation.Business.ValidateNumber
            '       - bSIRPremiumFinance.BeginTrans
            '           - bSIRPremiumFinance.LoadPartners (STAR GATE ONLY SO IGNORE)
            '           - bSIRPremiumFinance.ProcessPlan
            '           - bSIRPremiumFinance.StatusUpdate
            '       - bSIRPremiumFinance.CommitTrans

            ' delete the renewal quote and use the instalment details passed in the request
            Dim comReturnValue As Integer

            If request.CreditCard IsNot Nothing Then
                'WPR 33-75 END
                ' Start - (Sankar) - (Tech Spec - ACR006 - FDMS Tacking Number) - (6.3.2)
                premiumFinanceBusiness.DepositCCTrackingNumber = request.CreditCard.TrackingNumber
                ' End - (Sankar) - (Tech Spec - ACR006 - FDMS Tacking Number) - (6.3.2)

                premiumFinanceBusiness.AccountType = request.CreditCard.AccountType
                premiumFinanceBusiness.VIAPaymentHub = request.CreditCard.VIAPaymentHub
            End If

            ' if this call is not just to ensure 
            ' that copied instalment plan details are removed
            ' go ahead and create the required instalment plan
            If Not onlyDeleteExistingInstalmentPlan Then

                premiumFinanceBusiness.InsuranceFileCnt = request.InsuranceFileKey
                premiumFinanceBusiness.TransType = request.TransactionType
                premiumFinanceBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)

                Dim instalmentQuotes As Object(,) = Nothing
                Dim matchingRatePositionInQuoteArray As Integer

                ' if the selected quote is not found a business error will be thrown
                ValidateSelectedQuote(
                    premiumFinanceBusiness,
                    request,
                    instalmentQuotes,
                    matchingRatePositionInQuoteArray, con)

                comReturnValue = premiumFinanceBusiness.InsertOrUpdatePremiumFinance(
                    instalmentQuotes,
                    matchingRatePositionInQuoteArray,
                    request.DataStore.PFTransactions,
                    request.DataStore.PFPremiumFinanceCnt,
                    request.DataStore.PFPremiumFinanceVersion,
                    m_klInstalmentMTAType_AddAndSpread)

                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.InsertOrUpdatePremiumFinance", comReturnValue)
                End If
                If request.DataStore.PaymentMethod = PolicyPaymentMethod.Instalments Then

                    Dim controlTransBusiness As bControlTrans.Automated = CreateAndInitialiseControlTransBusiness(con, request.BranchCode)
                    'For Object Array - When Strict On
                    Dim PFTransactions(,) As Object = CType(request.DataStore.PFTransactions, Object(,))
                    comReturnValue = controlTransBusiness.GetPFTransactions(
                request.InsuranceFileKey,
                PFTransactions)
                    request.DataStore.PFTransactions = CType(PFTransactions, Object)

                    If (comReturnValue <> PMEReturnCode.PMTrue) Then
                        RaiseComMethodException("bControlTrans.Automated.GetPFTransactions", comReturnValue)
                    End If
                    controlTransBusiness = Nothing
                End If
                Dim premiumFinanceArrayObject As Object(,) = Nothing

                comReturnValue = premiumFinanceBusiness.GetSingleFinancePlan(
                    request.DataStore.PFPremiumFinanceCnt,
                    request.DataStore.PFPremiumFinanceVersion,
                    premiumFinanceArrayObject)
                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", comReturnValue)
                End If

                If premiumFinanceArrayObject Is Nothing OrElse Not IsArray(premiumFinanceArrayObject) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", comReturnValue)
                End If

                Dim premiumFinanceArray As Object(,) = CType(premiumFinanceArrayObject, Object(,))

                premiumFinanceArray(k_PFPlanDateCreated, 0) = Date.Today
                premiumFinanceArray(k_PFPlanDateModified, 0) = Date.Today
                premiumFinanceArray(k_PfPlanDateBankDetailsChanged, 0) = Date.Today

                If IsArray(premiumFinanceArray) Then
                    o_sAutoGeneratedPlanRef = Cast.ToString(premiumFinanceArray(k_PFPlanAutoGenPlanRef, 0), String.Empty).Trim
                    sAutoPlanRef = o_sAutoGeneratedPlanRef
                End If

                If request.CreditCard IsNot Nothing Then
                    ' Start - (Prakash C Varghese)- Copied from ProcessNBInstalments
                    premiumFinanceBusiness.DepositCCTrackingNumber = request.CreditCard.TrackingNumber
                    premiumFinanceBusiness.AccountType = request.CreditCard.AccountType

                    If request.CreditCard.PartyBankKey > 0 Then
                        iCreatePartyBankRecord = 0
                    End If

                    PopulatePremiumFinanceCreditCardDetails(request, premiumFinanceArray)
                Else

                    If request.PartyBankKey > 0 Then
                        iCreatePartyBankRecord = 0
                    End If

                    PopulatePremiumFinanceBankDetails(request, premiumFinanceArray)
                End If

                Dim premiumFinanceCntObject As Integer = request.DataStore.PFPremiumFinanceCnt
                Dim premiumFinanceVersionObject As Integer = request.DataStore.PFPremiumFinanceVersion

                premiumFinanceArrayObject = CType(premiumFinanceArray, Object(,))

                'Modified the value of iCreatePartyBankRecord parameter. 
                'If request objects party bank key value is non zero, the party bank item is already created. 
                'No need to create it again.
                comReturnValue = premiumFinanceBusiness.UpdateExistingRecord(
                        vExistingRecord:=CType(CObj(premiumFinanceArrayObject), System.Array),
                        vPremiumFinanceCnt:=premiumFinanceCntObject,
                        vPremiumFinanceVersion:=premiumFinanceVersionObject,
                        nArrayIndex:=0, vPremiumFinanceMTA:=Nothing,
                        iCreatePartyBankRecord:=CInt(iCreatePartyBankRecord))
                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.UpdateExistingRecord", comReturnValue)
                End If

                ' create initial bank history record
                comReturnValue = premiumFinanceBusiness.SaveInstalmentsPlanMediaTypeDetails(
                     request.DataStore.PFPremiumFinanceCnt,
                     request.DataStore.PFPremiumFinanceVersion,
                     InstalmentHistoryActionType.Setup)
                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.SaveInstalmentsPlanMediaTypeDetails", comReturnValue)
                End If

                comReturnValue = premiumFinanceBusiness.DeletePFTransID(
                     request.DataStore.PFPremiumFinanceCnt,
                    request.DataStore.PFPremiumFinanceVersion)
                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.DeletePFTransID", comReturnValue)
                End If

                Dim pfTransactionsArrayObject As Object(,) = CType(request.DataStore.PFTransactions, Object(,))
                comReturnValue = premiumFinanceBusiness.InsertPFTransID(
                    request.DataStore.PFPremiumFinanceCnt,
                    request.DataStore.PFPremiumFinanceVersion,
                     pfTransactionsArrayObject)
                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.InsertPFTransID", comReturnValue)
                End If

                Dim pfPlanTransDetailId As Integer = 0
                Dim depositTransDetailsId As Object = Nothing

                Dim lMode As Integer
                If premiumFinanceArrayObject IsNot Nothing AndAlso Convert.ToInt32(premiumFinanceArrayObject(k_PFPlanSchemeType_ID, 0)) = 1 Then
                    lMode = InstalmentsMode.ThirdParty
                Else
                    lMode = InstalmentsMode.NewBusinessOrRenewals
                End If

                comReturnValue = premiumFinanceBusiness.ProcessPlan(
                    lMode,
                    premiumFinanceArrayObject,
                    pfTransactionsArrayObject,
                    pfPlanTransDetailId,
                    NoDeposit,
                    depositTransDetailsId,
                    m_klInstalmentMTAType_AddAndSpread)
                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.ProcessPlan", comReturnValue)
                End If
                If Not depositTransDetailsId Is Nothing Then
                    r_nDepositTransdetailID = CType(depositTransDetailsId, Integer)
                End If
                ' update the plans status to live
                comReturnValue = premiumFinanceBusiness.StatusUpdate(
                    premiumFinanceCntObject,
                    premiumFinanceVersionObject,
                    InstalmentPlanStatus.Live)
                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.StatusUpdate Failed to update the Finance Plan Status to Live")
                End If

                comReturnValue = premiumFinanceBusiness.CreateEvent(
                (ToSafeInteger(PMEComponentAction.PMAdd)).ToString,
                request.InsuranceFileKey,
                request.DataStore.PartyKey,
                ToSafeString(sAutoPlanRef))




                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.CreateEvent", comReturnValue)
                End If
            End If

        Finally
            If premiumFinanceBusiness IsNot Nothing Then
                premiumFinanceBusiness.Dispose()
                premiumFinanceBusiness = Nothing
            End If
        End Try

    End Sub

    Public Sub ProcessNBInstalments(
        ByVal con As SiriusConnection,
        ByVal request As BaseBindQuoteRequestType,
        Optional ByRef sAutoGeneratedPlanRef As String = "",
        Optional ByRef iDepositTransdetailID As Integer = 0,
         Optional ByVal iAccountId As Integer = 0)


        Const NoDeposit As Boolean = False

        Dim premiumFinanceBusiness As bSIRPremiumFinance.Business =
         CreateAndInitialisePremiumFinanceBusiness(con, request.BranchCode)

        Dim iCreatePartyBankRecord As Int16 = 1

        Try

            ' ************************************
            ' This method was extracted from the following components
            ' ************************************

            ' iPMUListRisks - Refresh Quotes
            '       - Calculate And Validate
            ' iPMUListRisk - SaveQuote
            '       - Insert or Update Premium Finance Plan
            ' iPMBFinancePlanMaint - load
            '       - get Single Premium Finance Plan

            ' iPMBFinancePlanMaint - cmd_Save
            '       - bSIRPRemiumFinance.UpdateExistingRecord
            '       - bSIRPRemiumFinance.SaveInstalmentsPlanMediaTypeDetails
            '       - bSIRPremiumFinance.DeletePFTransID 
            '       - bSIRPremiumFinance.InsertPFTransID
            '       - bSIRPremiumFinance.AddPartners ( STAR GATE ONLY SO IGNORE)
            '       - bSIRPremiumFinance.CreateEvent

            ' iPMBFinancePlanMaint - cmd_Transact
            '       - bSIRPremiumFinance.CheckAllocationAgainstPolicy
            '       - ValidateDataEntry
            '       - bSirMediaTypeValidation.Business.ValidateNumber
            '       - bSIRPremiumFinance.BeginTrans
            '           - bSIRPremiumFinance.LoadPartners (STAR GATE ONLY SO IGNORE)
            '           - bSIRPremiumFinance.ProcessPlan
            '           - bSIRPremiumFinance.StatusUpdate
            '       - bSIRPremiumFinance.CommitTrans

            premiumFinanceBusiness.InsuranceFileCnt = request.InsuranceFileKey
            premiumFinanceBusiness.TransType = request.TransactionType
            premiumFinanceBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)
            'WPR 33-75 ADDED
            If request.CreditCard IsNot Nothing Then
                'WPR 33-75 END
                ' Start - (Sankar) - (Tech Spec - ACR006 - FDMS Tacking Number) - (6.3.2)
                premiumFinanceBusiness.DepositCCTrackingNumber = request.CreditCard.TrackingNumber
                ' End - (Sankar) - (Tech Spec - ACR006 - FDMS Tacking Number) - (6.3.2)

                premiumFinanceBusiness.AccountType = request.CreditCard.AccountType
                premiumFinanceBusiness.VIAPaymentHub = request.CreditCard.VIAPaymentHub
            End If
            Dim instalmentQuotes As Object(,) = Nothing
            Dim matchingRatePositionInQuoteArray As Integer

            ' if the selected quote is not found a business error will be thrown
            ValidateSelectedQuote(
                premiumFinanceBusiness,
                request,
                instalmentQuotes,
                matchingRatePositionInQuoteArray, con)

            Dim comReturnValue As Integer

            comReturnValue = premiumFinanceBusiness.InsertOrUpdatePremiumFinance(
                instalmentQuotes,
                matchingRatePositionInQuoteArray,
                request.DataStore.PFTransactions,
                request.DataStore.PFPremiumFinanceCnt,
                request.DataStore.PFPremiumFinanceVersion,
                m_klInstalmentMTAType_AddAndSpread)

            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.InsertOrUpdatePremiumFinance", comReturnValue)
            End If

            If request.DataStore.PaymentMethod = PolicyPaymentMethod.Instalments Then

                Dim controlTransBusiness As bControlTrans.Automated = CreateAndInitialiseControlTransBusiness(con, request.BranchCode)
                'For Object Array - When Strict On
                Dim PFTransactions(,) As Object = CType(request.DataStore.PFTransactions, Object(,))
                comReturnValue = controlTransBusiness.GetPFTransactions(
                request.InsuranceFileKey,
                PFTransactions)
                request.DataStore.PFTransactions = CType(PFTransactions, Object)

                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bControlTrans.Automated.GetPFTransactions", comReturnValue)
                End If
                controlTransBusiness = Nothing
            End If

            Dim premiumFinanceArrayObject As Object(,) = Nothing

            comReturnValue = premiumFinanceBusiness.GetSingleFinancePlan(
                request.DataStore.PFPremiumFinanceCnt,
                request.DataStore.PFPremiumFinanceVersion,
                premiumFinanceArrayObject)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", comReturnValue)
            End If

            If premiumFinanceArrayObject Is Nothing OrElse Not IsArray(premiumFinanceArrayObject) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", comReturnValue)
            End If

            Dim premiumFinanceArray As Object(,) = CType(premiumFinanceArrayObject, Object(,))

            premiumFinanceArray(k_PFPlanDateCreated, 0) = Date.Today
            premiumFinanceArray(k_PFPlanDateModified, 0) = Date.Today
            premiumFinanceArray(k_PfPlanDateBankDetailsChanged, 0) = Date.Today

            'WPR 33-75 ADDED
            If IsArray(premiumFinanceArray) Then
                sAutoGeneratedPlanRef = Cast.ToString(premiumFinanceArray(k_PFPlanAutoGenPlanRef, 0), String.Empty).Trim
                sAutoPlanRef = sAutoGeneratedPlanRef
            End If
            'WPR 33-75 END

            If request.CreditCard IsNot Nothing Then

                ' Start - (Sankar) - (Tech Spec - ACR006 - FDMS Tacking Number) - (6.3.2)
                premiumFinanceBusiness.DepositCCTrackingNumber = request.CreditCard.TrackingNumber
                ' End - (Sankar) - (Tech Spec - ACR006 - FDMS Tacking Number) - (6.3.2)

                premiumFinanceBusiness.AccountType = request.CreditCard.AccountType

                If request.CreditCard.PartyBankKey > 0 Then
                    iCreatePartyBankRecord = 0
                End If

                PopulatePremiumFinanceCreditCardDetails(request, premiumFinanceArray)
            Else

                If request.PartyBankKey > 0 Then
                    iCreatePartyBankRecord = 0
                End If

                PopulatePremiumFinanceBankDetails(request, premiumFinanceArray)
            End If

            Dim premiumFinanceCntObject As Integer = request.DataStore.PFPremiumFinanceCnt
            Dim premiumFinanceVersionObject As Integer = request.DataStore.PFPremiumFinanceVersion

            premiumFinanceArrayObject = premiumFinanceArray

            'Modified the value of iCreatePartyBankRecord parameter. 
            'If request objects party bank key value is non zero, the party bank item is already created. 
            'No need to create it again.
            comReturnValue = premiumFinanceBusiness.UpdateExistingRecord(
                    vExistingRecord:=CType(CObj(premiumFinanceArrayObject), System.Array),
                    vPremiumFinanceCnt:=premiumFinanceCntObject,
                    vPremiumFinanceVersion:=premiumFinanceVersionObject,
                    nArrayIndex:=0, vPremiumFinanceMTA:=Nothing,
                    iCreatePartyBankRecord:=CInt(iCreatePartyBankRecord))
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.UpdateExistingRecord", comReturnValue)
            End If

            ' create initial bank history record
            comReturnValue = premiumFinanceBusiness.SaveInstalmentsPlanMediaTypeDetails(
                 request.DataStore.PFPremiumFinanceCnt,
                 request.DataStore.PFPremiumFinanceVersion,
                 InstalmentHistoryActionType.Setup)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.SaveInstalmentsPlanMediaTypeDetails", comReturnValue)
            End If

            comReturnValue = premiumFinanceBusiness.DeletePFTransID(
                 request.DataStore.PFPremiumFinanceCnt,
                request.DataStore.PFPremiumFinanceVersion)
            If (comReturnValue <> PMEReturnCode.PMTrue) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.DeletePFTransID", comReturnValue)
            End If

            Dim pfTransactionsArrayObject As Object(,) = CType(request.DataStore.PFTransactions, Object(,))
            comReturnValue = premiumFinanceBusiness.InsertPFTransID(
                request.DataStore.PFPremiumFinanceCnt,
                request.DataStore.PFPremiumFinanceVersion,
                 pfTransactionsArrayObject)
            If (comReturnValue <> PMEReturnCode.PMTrue) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.InsertPFTransID", comReturnValue)
            End If

            Dim pfPlanTransDetailId As Integer = 0
            Dim depositTransDetailsId As Object = Nothing

            If iDepositTransdetailID <> 0 Then
                depositTransDetailsId = iDepositTransdetailID
            End If

            comReturnValue = premiumFinanceBusiness.GetSingleFinancePlan(
                                        request.DataStore.PFPremiumFinanceCnt,
                                        request.DataStore.PFPremiumFinanceVersion,
                                        premiumFinanceArrayObject)

            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", comReturnValue)
            End If

            If premiumFinanceArrayObject Is Nothing OrElse Not IsArray(premiumFinanceArrayObject) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", comReturnValue)
            End If
            Dim lMode As Integer
            If premiumFinanceArrayObject IsNot Nothing AndAlso Convert.ToInt32(premiumFinanceArrayObject(k_PFPlanSchemeType_ID, 0)) = 1 Then
                lMode = InstalmentsMode.ThirdParty
            Else
                lMode = InstalmentsMode.NewBusinessOrRenewals
            End If
            If Not request.NoTrans Then
                comReturnValue = premiumFinanceBusiness.ProcessPlan(
                lMode,
                premiumFinanceArrayObject,
                pfTransactionsArrayObject,
                pfPlanTransDetailId,
                NoDeposit,
                depositTransDetailsId,
                m_klInstalmentMTAType_AddAndSpread, v_lAccountId:=iAccountId)
                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.ProcessPlan", comReturnValue)
                End If
            End If

            iDepositTransdetailID = CType(depositTransDetailsId, Integer)
            'iDepositTransdetailID = CType(pfPlanTransDetailId, Integer)

            ' Update the plans status to live
            comReturnValue = premiumFinanceBusiness.StatusUpdate(
                premiumFinanceCntObject,
                premiumFinanceVersionObject,
                InstalmentPlanStatus.Live)
            If (comReturnValue <> PMEReturnCode.PMTrue) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.StatusUpdate Failed to update the Finance Plan Status to Live")
            End If

            comReturnValue = premiumFinanceBusiness.CreateEvent(
               (ToSafeInteger(PMEComponentAction.PMAdd)).ToString,
               request.InsuranceFileKey,
               request.DataStore.PartyKey,
               ToSafeString(request.DataStore.PFPremiumFinanceCnt))
            If (comReturnValue <> PMEReturnCode.PMTrue) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.CreateEvent", comReturnValue)
            End If

        Finally
            If premiumFinanceBusiness IsNot Nothing Then
                premiumFinanceBusiness.Dispose()
                premiumFinanceBusiness = Nothing
            End If
        End Try

    End Sub

    Private Sub PopulatePremiumFinanceBankDetails(
    ByRef request As BaseBindQuoteRequestType,
    ByRef premiumFinanceDetailsArray As Object(,))

        premiumFinanceDetailsArray(k_PFPlanBankName, 0) = request.BankName
        premiumFinanceDetailsArray(k_PFPlanBankSortCode, 0) = request.BankSortCode
        premiumFinanceDetailsArray(k_PFPlanBankAccountNo, 0) = request.BankAccountNo
        premiumFinanceDetailsArray(k_PFPlanBankAccountName, 0) = request.BankAccountName
        premiumFinanceDetailsArray(k_PFPlanBankBranch, 0) = request.BankBranch
        premiumFinanceDetailsArray(k_PFPlanBankAreaCode, 0) = request.BankAreaCode
        premiumFinanceDetailsArray(k_PFPlanBankPhone, 0) = request.BankPhone
        premiumFinanceDetailsArray(k_PFPlanBankExtn, 0) = request.BankExtn
        premiumFinanceDetailsArray(k_PFPlanBankFaxCode, 0) = request.BankFaxCode
        premiumFinanceDetailsArray(k_PFPlanBankFax, 0) = request.BankFax
        premiumFinanceDetailsArray(bSIRPremFinConst.kBIC, 0) = request.BIC
        premiumFinanceDetailsArray(bSIRPremFinConst.kIBAN, 0) = request.IBAN

        If request.BankAddress IsNot Nothing Then
            premiumFinanceDetailsArray(k_PFPlanBankAddress1, 0) = request.BankAddress.AddressLine1
            premiumFinanceDetailsArray(k_PFPlanBankAddress2, 0) = request.BankAddress.AddressLine2
            premiumFinanceDetailsArray(k_PFPlanBankAddress3, 0) = request.BankAddress.AddressLine3
            premiumFinanceDetailsArray(k_PFPlanBankAddress4, 0) = request.BankAddress.AddressLine4
            premiumFinanceDetailsArray(k_PFPlanBankPostcode, 0) = request.BankAddress.PostCode
            premiumFinanceDetailsArray(k_PFPlanBankCountry, 0) = request.BankAddress.CountryCode
        End If

        If request.PartyBankKey > 0 Then
            premiumFinanceDetailsArray(k_PFPlanPartyBankIdSel, 0) = request.PartyBankKey
        End If

    End Sub

    Private Sub PopulatePremiumFinanceCreditCardDetails(
    ByRef request As BaseBindQuoteRequestType,
    ByRef premiumFinanceDetailsArray As Object(,))

        If request.PaymentMethodSpecified = True AndAlso
            request.PaymentMethod = PaymentMethodType.CreditCard Then

            If request.CreditCard IsNot Nothing Then
                If Not String.IsNullOrEmpty(request.CreditCard.NameOnCreditCard) Then
                    premiumFinanceDetailsArray(k_PFPlanBankAccountName, 0) = request.CreditCard.NameOnCreditCard
                End If
                premiumFinanceDetailsArray(k_PFPlanCCNumber, 0) = request.CreditCard.Number
                premiumFinanceDetailsArray(k_PFPlanCCExpiryDate, 0) = request.CreditCard.ExpiryDate
                premiumFinanceDetailsArray(k_PFPlanCCStartDate, 0) = request.CreditCard.StartDate
                premiumFinanceDetailsArray(k_PFPlanCCIssue, 0) = request.CreditCard.Issue
                premiumFinanceDetailsArray(k_PFPlanCCPin, 0) = request.CreditCard.Pin
                premiumFinanceDetailsArray(k_PFPlanCardType, 0) = request.CreditCard.TypeCode
                premiumFinanceDetailsArray(k_PFPlanAuthCode, 0) = request.CreditCard.AuthCode

                'If Party Bank_id is not nothing and AuthCode is nothing then Get the auth_code by bank_id.
                If ((request.CreditCard.PartyBankKey > 0) AndAlso (request.CreditCard.AuthCode Is Nothing OrElse request.CreditCard.AuthCode = "")) Then
                    PopulateCredicardAuthCode(request)
                    premiumFinanceDetailsArray(k_PFPlanAuthCode, 0) = request.CreditCard.AuthCode
                End If
                If request.CreditCard.PartyBankKey > 0 Then
                    premiumFinanceDetailsArray(k_PFPlanPartyBankIdSel, 0) = request.CreditCard.PartyBankKey
                End If


                premiumFinanceDetailsArray(k_PFPlanIsCardholder, 0) = request.CreditCard.CardHolder Is Nothing

                If request.CreditCard.CardHolder IsNot Nothing Then

                    premiumFinanceDetailsArray(k_PfPlanCardholderName, 0) = request.CreditCard.CardHolder.Name
                    premiumFinanceDetailsArray(k_PfPlanCardholderAddress1, 0) = request.CreditCard.CardHolder.AddressLine1
                    premiumFinanceDetailsArray(k_PfPlanCardholderAddress2, 0) = request.CreditCard.CardHolder.AddressLine2
                    premiumFinanceDetailsArray(k_PfPlanCardholderAddress3, 0) = request.CreditCard.CardHolder.AddressLine3
                    premiumFinanceDetailsArray(k_PfPlanCardholderAddress4, 0) = request.CreditCard.CardHolder.AddressLine4
                    premiumFinanceDetailsArray(k_PfPlanCardholderPostcode, 0) = request.CreditCard.CardHolder.PostCode
                End If

            End If

        End If

    End Sub

    '''<summary>
    ''' Retrieves AuthCode of Party bank item for a specified party bank item.
    '''</summary>
    '''<param name="oRequest" type="BaseBindQuoteRequestType"></param>
    '''<remarks>As Portal is not supplied the authcode details In case of MTA and REN so it will fetch the details for these situation</remarks> 
    Private Sub PopulateCredicardAuthCode(ByRef oRequest As BaseBindQuoteRequestType)
        Dim dtPartybank As New DataTable

        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            ' Get credit Card Auth code
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PartyBank_Details_ByID")
                cmd.AddInParameter("party_bank_id", SqlDbType.Int).Value = oRequest.CreditCard.PartyBankKey
                dtPartybank = con.ExecuteDataTable(cmd)
            End Using

        End Using
        If dtPartybank IsNot Nothing AndAlso dtPartybank.Rows.Count > 0 AndAlso Not String.IsNullOrEmpty(oRequest.CreditCard.AuthCode) Then
            oRequest.CreditCard.AuthCode = Convert.ToString(dtPartybank.Rows(0)("manual_auth_number"))
            Exit Sub
        End If

        'If the Above doesnt work then try this
        If oRequest.DataStore.PFPremiumFinanceCnt > 0 AndAlso oRequest.DataStore.PFPremiumFinanceVersion > 0 Then
            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PFPremiumFinance_Sel_Single")
                    cmd.AddInParameter("financeplancnt", SqlDbType.Int).Value = oRequest.DataStore.PFPremiumFinanceCnt
                    cmd.AddInParameter("financeplanversion", SqlDbType.Int).Value = oRequest.DataStore.PFPremiumFinanceVersion
                    dtPartybank = con.ExecuteDataTable(cmd)
                End Using
            End Using
            If dtPartybank IsNot Nothing AndAlso dtPartybank.Rows.Count > 0 Then
                oRequest.CreditCard.AuthCode = Convert.ToString(dtPartybank.Rows(0)("auth_code"))
            End If
        End If
    End Sub

    Public Overloads Function SavePremiumFinanceDetails(ByVal oSavePremiumFinanceDetailsRequest As BaseSavePremiumFinanceDetailsRequestType) As BaseSavePremiumFinanceDetailsResponseType
        Dim oResponse As BaseSavePremiumFinanceDetailsResponseType = Nothing
        Using con As SiriusConnection = New SiriusConnectionPMDAO(_SiriusUser.Username,
                                                                  _SiriusUser.SourceID,
                                                                  _SiriusUser.LanguageID,
                                                                  SiriusUserDefaults.AppName)
            oResponse = SavePremiumFinanceDetails(con, oSavePremiumFinanceDetailsRequest)
            Return oResponse
        End Using

    End Function

    Public Overloads Function SavePremiumFinanceDetails(ByVal con As SiriusConnection, ByVal oSavePremiumFinanceDetailsRequest As BaseSavePremiumFinanceDetailsRequestType) As BaseSavePremiumFinanceDetailsResponseType
        Const KACMethodName As String = "SavePremiumFinanceDetails"
        Dim oErrors As New SAMErrorCollection
        Dim STSError As New STSErrorPublisher
        Dim nCreatePartyBankRecord As Integer = 0
        Dim nSourceID As Integer
        Dim oBusiness As New CoreBusiness
        Dim nComReturnValue As Integer = PMReturnCode.PMTrue
        Dim nPfpremiumFinanceCnt As Integer
        Dim nPfpremiumFinanceCntversion As Integer
        Dim sPaymentMethod As String = String.Empty
        Dim oCreditCard As New BaseCreditCardType
        Dim nPartyBankKey As Integer
        Dim oPremiumFinanceBusiness As bSIRPremiumFinance.Business = CreateAndInitialisePremiumFinanceBusiness(con, oSavePremiumFinanceDetailsRequest.BranchCode)
        Dim nReturn As Integer
        Dim vPFPremiumFinance As Object(,) = Nothing
        Dim oPFArray As Object(,)
        Dim lPremiumFinanceCntObject As Object
        Dim lPremiumFinanceVerObject As Object
        Dim iInsuranceFileCnt As Integer
        Dim iClientId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oResponse As SAMForInsuranceV2ImplementationTypes.SavePremiumFinanceDetailsResponseType

        nSourceID% = 1
        ' Convert branch code to ID
        Try
            nSourceID% = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", oSavePremiumFinanceDetailsRequest.BranchCode)
            oSavePremiumFinanceDetailsRequest.SourceId = nSourceID%
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), oSavePremiumFinanceDetailsRequest.BranchCode)
        End Try

        With oSavePremiumFinanceDetailsRequest
            If .GetType Is GetType(SAMForInsuranceV2ImplementationTypes.SavePremiumFinanceDetailsRequestType) Then
                nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
                oResponse = New SAMForInsuranceV2ImplementationTypes.SavePremiumFinanceDetailsResponseType
            Else
                nTypeOfPackage = enumTypeOfPackage.UnknownPackage
                Return oResponse
            End If

            Dim ds As DataSet
            Using oCmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_SavedPFPlan_On_Renewal")
                oCmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = .InsuranceFileKey
                ds = con.ExecuteDataSet(oCmd, "FinancePlanDetails")

                Dim dr As DataRow
                If ds.Tables("FinancePlanDetails").Rows.Count > 0 Then
                    dr = ds.Tables("FinancePlanDetails").Rows(0)
                    nPfpremiumFinanceCnt = ToSafeInteger(dr.Item("pfprem_finance_cnt"))
                    nPfpremiumFinanceCntversion = ToSafeInteger(dr.Item("pfprem_finance_version"))
                    sPaymentMethod = ToSafeString(dr.Item("payment_method"))
                End If
            End Using

            If nPfpremiumFinanceCnt = 0 AndAlso nPfpremiumFinanceCntversion = 0 Then
                Dim oInstalmentQuotes As Object(,) = Nothing
                Dim nMatchingRatePositionInQuoteArray As Integer
                Dim sInsuranceFileTypeCode As String
                Dim nInsuranceFileTypeId As Integer
                Dim sProductCode As String = String.Empty
                Dim nPolicyVersion As Integer

                'Get the insurance File Type to see what type of business we're doing
                GetInsuranceFileType(con, oSavePremiumFinanceDetailsRequest.InsuranceFileKey, nInsuranceFileTypeId, sInsuranceFileTypeCode, nPolicyVersion)
                oPremiumFinanceBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)
                Select Case sInsuranceFileTypeCode
                    Case InsuranceFileType.LivePolicy, InsuranceFileType.Quote, InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated
                        If (InsuranceFileType.LivePolicy = Trim(sInsuranceFileTypeCode)) AndAlso nPolicyVersion > 1 Then
                            sProductCode = "REN"
                        Else
                            sProductCode = "NB"
                        End If
                    Case InsuranceFileType.MTAPermanentQuotation, InsuranceFileType.MTATemporaryQuotation,
                        InsuranceFileType.MTAPermanent, InsuranceFileType.MTATemporary, InsuranceFileType.MTACancellation,
                        InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated, InsuranceFileType.MTAQuotationCancellation
                        sProductCode = "MTA"

                    Case InsuranceFileType.Renewal
                        sProductCode = "REN"
                End Select
                If sProductCode = "REN" Then
                    'oSavePremiumFinanceDetailsRequest.TransactionType = sProductCode
                    If Trim(sPaymentMethod.ToUpper()) = "PREMIUMFINANCE" OrElse Trim(sPaymentMethod.ToUpper()) = "INSTALMENT" OrElse
                        Trim(sPaymentMethod.ToUpper()) = "INSTALMENTS" Then

                        Dim instalmentQuotes As Object(,) = Nothing
                        Dim oQuoteRequest As BaseBindQuoteRequestType = New BaseBindQuoteRequestType
                        oQuoteRequest.TransactionType = TransactionTypeCode.Renewals
                        oQuoteRequest.InsuranceFileKey = oSavePremiumFinanceDetailsRequest.InsuranceFileKey
                        Dim nOriginalInsKey, nPFCnt, nPFVersion As Integer
                        Dim sInsStatus As String
                        GetCurrentInsuranceFileDetails(con, oSavePremiumFinanceDetailsRequest.InsuranceFileKey, nOriginalInsKey, nPFCnt, nPFVersion, sInsStatus)
                        oQuoteRequest.DataStore.OriginalInsuranceFileKey = nOriginalInsKey

                        ' Pass the values to the implementation request structure
                        oQuoteRequest.BranchCode = oSavePremiumFinanceDetailsRequest.BranchCode
                        oQuoteRequest.InsuranceFileKey = oSavePremiumFinanceDetailsRequest.InsuranceFileKey
                        oQuoteRequest.AcceptRenewal = False
                        If String.IsNullOrEmpty(oQuoteRequest.TransactionType) Then
                            oQuoteRequest.TransactionType = "REN"
                        End If
                        oQuoteRequest.SelectedInstalmentQuoteSpecified = False
                        oPremiumFinanceBusiness.InsuranceFileCnt = oSavePremiumFinanceDetailsRequest.InsuranceFileKey
                        oPremiumFinanceBusiness.SchemeNo = oSavePremiumFinanceDetailsRequest.SchemeNo
                        oPremiumFinanceBusiness.SchemeVer = oSavePremiumFinanceDetailsRequest.SchemeVersion
                        oQuoteRequest.SelectedSchemeNo = oSavePremiumFinanceDetailsRequest.SchemeNo
                        oQuoteRequest.SelectedSchemeVersion = oSavePremiumFinanceDetailsRequest.SchemeVersion
                        oQuoteRequest.PFRF_ID = oSavePremiumFinanceDetailsRequest.PFRF_ID
                        oQuoteRequest.MonthDay = oSavePremiumFinanceDetailsRequest.MonthDay
                        oQuoteRequest.AmountToFinance = oSavePremiumFinanceDetailsRequest.AmountToFinance
                        oQuoteRequest.AmountPaid = oSavePremiumFinanceDetailsRequest.NetAmount
                        oQuoteRequest.PreferredDate = oSavePremiumFinanceDetailsRequest.PreferredDate
                        oQuoteRequest.WeekDay = oSavePremiumFinanceDetailsRequest.WeekDay
                        oQuoteRequest.BankAccountName = oSavePremiumFinanceDetailsRequest.BankAccountName
                        oQuoteRequest.BankAccountNo = oSavePremiumFinanceDetailsRequest.BankAccountNo
                        oQuoteRequest.BankSortCode = oSavePremiumFinanceDetailsRequest.BankSortCode
                        oQuoteRequest.DayOfMonthIsValid = True
                        ValidateSelectedQuote(
                            oPremiumFinanceBusiness,
                            oQuoteRequest,
                            oInstalmentQuotes,
                            nMatchingRatePositionInQuoteArray, con)
                        nReturn = oPremiumFinanceBusiness.InsertOrUpdatePremiumFinance(
                            oInstalmentQuotes,
                            nMatchingRatePositionInQuoteArray,
                            Nothing,
                            nPfpremiumFinanceCnt,
                            nPfpremiumFinanceCntversion)
                        If nReturn <> PMEReturnCode.PMTrue Then
                            RaiseComMethodException("bSIRPremiumFinance.Business.InsertOrUpdatePremiumFinance", nReturn)
                        End If

                    Else
                        nReturn = oPremiumFinanceBusiness.MarkPlanAsDeleted(.InsuranceFileKey)
                        If nReturn <> PMEReturnCode.PMTrue Then
                            RaiseComMethodException("bSIRPremiumFinance.Business.MarkPlanAsDeleted", nReturn)
                        End If
                    End If
                End If
            End If

            nReturn = oPremiumFinanceBusiness.GetSingleFinancePlan(v_lFinanceCount:=nPfpremiumFinanceCnt, v_lFinanceVersion:=nPfpremiumFinanceCntversion, r_vPFPremiumFinance:=vPFPremiumFinance)
            If nReturn <> PMEReturnCode.PMTrue AndAlso nReturn <> PMEReturnCode.PMNotFound Then
                RaiseComMethodException("bSIRPremiumFinance.BusinessClass.GetSingleFinancePlan", nReturn)
            End If

            oPFArray = DirectCast(vPFPremiumFinance, Object(,))

            If .CreditCard IsNot Nothing Then
                oCreditCard.NameOnCreditCard = .CreditCard.NameOnCreditCard
                oCreditCard.Number = .CreditCard.Number
                oCreditCard.ExpiryDate = .CreditCard.ExpiryDate

                nPartyBankKey = .CreditCard.PartyBankKey

                'Validate CC Details only if AuthCode is also not Given.
                If String.IsNullOrEmpty(.CreditCard.AuthCode) Then
                    oCreditCard.Validate(CType(oErrors, Object))
                    oErrors.CheckForErrors()
                End If
                oCreditCard.AuthCode = .CreditCard.AuthCode
                oCreditCard.PartyBankKey = .CreditCard.PartyBankKey
            Else
                nPartyBankKey = .PartyBankKey
            End If

            If nPartyBankKey > 0 Then
                Dim sReturnValue As String
                sReturnValue = GetAndValidateDescriptionById(con, "Party_Bank", "Party_Bank_Id", "Party_Bank_Id", nPartyBankKey.ToString())
                Try
                    If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                        oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                                SAMInvalidData.InvalidLookupListValue.ToString,
                                                                "PartyBankKey",
                                                               nPartyBankKey.ToString())
                    End If
                Catch
                    oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                                SAMInvalidData.InvalidLookupListValue.ToString,
                                                                "PartyBankKey",
                                                               nPartyBankKey.ToString())
                End Try
                oErrors.CheckForErrors()
            Else
                nCreatePartyBankRecord = 1
            End If


            oPFArray(k_PFPlanPFCnt, 0) = nPfpremiumFinanceCnt
            oPFArray(k_PFPlanPFVersion, 0) = nPfpremiumFinanceCntversion
            oPFArray(k_PFPlanPartyBankIdSel, 0) = nPartyBankKey

            oPFArray(k_PFPlanBankName, 0) = .BankName
            oPFArray(k_PFPlanBankSortCode, 0) = .BankSortCode
            oPFArray(k_PFPlanBankAccountNo, 0) = .BankAccountNo
            oPFArray(k_PFPlanBankAccountName, 0) = .BankAccountName
            oPFArray(k_PFPlanBankBranch, 0) = .BankBranch
            oPFArray(k_PFPlanBankAreaCode, 0) = .BankAreaCode
            oPFArray(k_PFPlanBankPhone, 0) = .BankPhone
            oPFArray(k_PFPlanBankExtn, 0) = .BankExtn
            oPFArray(k_PFPlanBankFaxCode, 0) = .BankFaxCode
            oPFArray(k_PFPlanBankFax, 0) = .BankFax

            If .BankAddress IsNot Nothing Then
                oPFArray(k_PFPlanBankAddress1, 0) = .BankAddress.AddressLine1
                oPFArray(k_PFPlanBankAddress2, 0) = .BankAddress.AddressLine2
                oPFArray(k_PFPlanBankAddress3, 0) = .BankAddress.AddressLine3
                oPFArray(k_PFPlanBankAddress4, 0) = .BankAddress.AddressLine4
                oPFArray(k_PFPlanBankPostcode, 0) = .BankAddress.PostCode
                oPFArray(k_PFPlanBankCountry, 0) = .BankAddress.CountryCode
            End If

            If .CreditCard IsNot Nothing Then
                If Not String.IsNullOrEmpty(.CreditCard.NameOnCreditCard) Then
                    oPFArray(k_PFPlanBankAccountName, 0) = .CreditCard.NameOnCreditCard
                End If
                oPFArray(k_PFPlanCCNumber, 0) = .CreditCard.Number
                oPFArray(k_PFPlanCCExpiryDate, 0) = .CreditCard.ExpiryDate
                oPFArray(k_PFPlanCCStartDate, 0) = .CreditCard.StartDate
                oPFArray(k_PFPlanCCIssue, 0) = .CreditCard.Issue
                oPFArray(k_PFPlanCCPin, 0) = .CreditCard.Pin
                oPFArray(k_PFPlanCardType, 0) = .CreditCard.TypeCode
                oPFArray(k_PFPlanAuthCode, 0) = .CreditCard.AuthCode

                If .CreditCard.PartyBankKey > 0 AndAlso String.IsNullOrEmpty(.CreditCard.AuthCode) Then
                    oPFArray(k_PFPlanAuthCode, 0) = GetCreditCardAuthCode(.CreditCard.PartyBankKey,
                                                                                            nPfpremiumFinanceCnt,
                                                                                            nPfpremiumFinanceCntversion)
                End If


                oPFArray(k_PFPlanIsCardholder, 0) = .CreditCard.CardHolder Is Nothing

                If .CreditCard.CardHolder IsNot Nothing Then

                    oPFArray(k_PfPlanCardholderName, 0) = .CreditCard.CardHolder.Name
                    oPFArray(k_PfPlanCardholderAddress1, 0) = .CreditCard.CardHolder.AddressLine1
                    oPFArray(k_PfPlanCardholderAddress2, 0) = .CreditCard.CardHolder.AddressLine2
                    oPFArray(k_PfPlanCardholderAddress3, 0) = .CreditCard.CardHolder.AddressLine3
                    oPFArray(k_PfPlanCardholderAddress4, 0) = .CreditCard.CardHolder.AddressLine4
                    oPFArray(k_PfPlanCardholderPostcode, 0) = .CreditCard.CardHolder.PostCode
                End If

            End If

            oPFArray(k_PFPlanPFRF_ID, 0) = .PFRF_ID
            oPFArray(k_PFPlanAmountToFinance, 0) = .AmountToFinance
            oPFArray(k_PFPlanStartDate, 0) = .StartDate
            oPFArray(k_PFPlanEndDate, 0) = .EndDate
            oPFArray(k_PFPlanDayOfWeekOrMonth, 0) = IIf(.WeekDay > .MonthDay, .WeekDay, .MonthDay)
            oPFArray(k_PFPlanDaysDelay, 0) = .DaysDelay
            oPFArray(k_PFPlanTotalCost, 0) = .TotalCost
            oPFArray(k_PFPlanFirstInstalmentdate, 0) = .FirstInstalmentDate
            oPFArray(k_PFPlanNextInstalmentdate, 0) = .NextInstalmentDate
            oPFArray(k_PFPlanLastInstalmentdate, 0) = .LastInstalmentDate
            oPFArray(k_PFPlanFirstInstalment, 0) = .FirstInstallment
            oPFArray(k_PFPlanOtherInstalments, 0) = .OtherInstallments
            oPFArray(k_PFPlanLastInstalment, 0) = .LastInstalment
            oPFArray(k_PFPlanNoOfInstalments, 0) = .NoOfInstallments

            oPFArray(k_PFPlanFinanceCharge, 0) = .FinanceFee
            oPFArray(k_PFPlanOriginalAmount, 0) = .OriginalAmount
            oPFArray(k_PFPlanDeposit, 0) = .Deposit

            oPFArray(k_PFPlanNetAmount, 0) = .NetAmount

            lPremiumFinanceCntObject = nPfpremiumFinanceCnt
            lPremiumFinanceVerObject = nPfpremiumFinanceCntversion
            vPFPremiumFinance = oPFArray



            nReturn = oPremiumFinanceBusiness.UpdateExistingRecord(
                   vExistingRecord:=CType(CObj(vPFPremiumFinance), System.Array),
                   vPremiumFinanceCnt:=lPremiumFinanceCntObject,
                   vPremiumFinanceVersion:=lPremiumFinanceVerObject,
                   nArrayIndex:=0, vPremiumFinanceMTA:=Nothing,
                   iCreatePartyBankRecord:=CInt(nCreatePartyBankRecord))
            nComReturnValue = oPremiumFinanceBusiness.MarkPlanAsSaved(.InsuranceFileKey)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.MarkPlanAsSaved", nComReturnValue)
            End If


            oPremiumFinanceBusiness.Dispose()
            oPremiumFinanceBusiness = Nothing

        End With

    End Function
    Private Function GetCreditCardAuthCode(ByVal nPartyBankKey As Integer,
                                               ByVal nPremiumFinanceKey As Integer,
                                               ByVal nPremiumFinanceVersion As Integer) As String
        Dim dtPartybank As New DataTable
        Dim sAuthcode As String = String.Empty
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            ' Get credit Card Auth code
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PartyBank_Details_ByID")
                cmd.AddInParameter("party_bank_id", SqlDbType.Int).Value = nPartyBankKey
                dtPartybank = con.ExecuteDataTable(cmd)
            End Using

        End Using
        If dtPartybank IsNot Nothing AndAlso dtPartybank.Rows.Count > 0 Then
            sAuthcode = Convert.ToString(dtPartybank.Rows(0)("manual_auth_number"))
        End If

        'If the Above doesnt work then try this
        If String.IsNullOrEmpty(sAuthcode) AndAlso nPremiumFinanceKey > 0 AndAlso nPremiumFinanceVersion > 0 Then
            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PFPremiumFinance_Sel_Single")
                    cmd.AddInParameter("financeplancnt", SqlDbType.Int).Value = nPremiumFinanceKey
                    cmd.AddInParameter("financeplanversion", SqlDbType.Int).Value = nPremiumFinanceVersion
                    dtPartybank = con.ExecuteDataTable(cmd)
                End Using
            End Using
            If dtPartybank IsNot Nothing AndAlso dtPartybank.Rows.Count > 0 Then
                sAuthcode = Convert.ToString(dtPartybank.Rows(0)("auth_code"))
            End If
        End If

        Return sAuthcode
    End Function

    Private Sub ProcessPolicyLevelFeesAndTaxes(
    ByRef con As SiriusConnection,
    ByRef request As BaseBindQuoteRequestType,
    ByRef listRisksBusiness As bSIRListRisks.Business)

        listRisksBusiness.UpdatePolicyPremium(request.InsuranceFileKey)

        'No need to recalculate policy fees here
        'listRisksBusiness.RecalculatePolicyFees(request.InsuranceFileKey, request.DataStore.ProductId, request.DataStore.TransactionTypeId, False)
        If request.IsMarketPlacePolicy Then
            listRisksBusiness.SetProcessModes(vTask:=SAMComponentAction.PMEdit)
        End If
        listRisksBusiness.RecalculatePolicyTaxes(request.InsuranceFileKey, SAMComponentAction.PMEdit, request.TransactionType)
        'listRisksBusiness.UpdatePolicyPremium(request.InsuranceFileKey)

    End Sub

    Private Sub ProcessAgentCommission(
    ByRef con As SiriusConnection,
    ByRef request As BaseBindQuoteRequestType,
    ByRef listRisksBusiness As bSIRListRisks.Business)

        Dim agentCommissionBusiness As BSirAgentCommission.Business =
            CreateAndInitialiseAgentCommissionBusiness(con, request.BranchCode)

        Try

            Dim agentCommissionObject As Object(,) = Nothing

            agentCommissionBusiness.GetAgentCommission(request.InsuranceFileKey, agentCommissionObject)

        Finally

            If agentCommissionBusiness IsNot Nothing Then
                agentCommissionBusiness.Dispose()
                agentCommissionBusiness = Nothing
            End If
        End Try

    End Sub
    ''' <summary>
    ''' GetOriginalInsuranceFileDetails
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="insuranceFileKey"></param>
    ''' <param name="originalInsuranceFileKey"></param>
    ''' <param name="pfPremFinanceCnt"></param>
    ''' <param name="pfPremFinanceVersion"></param>
    ''' <remarks></remarks>
    Private Sub GetOriginalInsuranceFileDetails(
    ByVal con As SiriusConnection,
    ByVal insuranceFileKey As Integer,
    ByRef originalInsuranceFileKey As Integer,
    ByRef pfPremFinanceCnt As Integer,
    ByRef pfPremFinanceVersion As Integer,
    Optional ByRef r_nPFPartyBankId As Integer = 0,
    Optional ByVal productCode As String = "")

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Original_Insurance_File_Details")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            originalInsuranceFileKey = Cast.ToInt32(dr.Item("original_insurance_file_cnt"), 0)
            pfPremFinanceCnt = Cast.ToInt32(dr.Item("pfprem_finance_cnt"), 0)
            pfPremFinanceVersion = Cast.ToInt32(dr.Item("pfprem_finance_version"), 0)
            r_nPFPartyBankId = Cast.ToInt32(dr.Item("party_bank_id"), 0)
        End If

        dt = Nothing
        If insuranceFileKey <> originalInsuranceFileKey AndAlso pfPremFinanceCnt = 0 AndAlso productCode.Length > 0 Then
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PFPremiumFinance_Sel_Latest_Valid_Plan_2")
                cmd.AddInParameter("@insurancefilecnt", SqlDbType.Int).Value = insuranceFileKey
                cmd.AddInParameter("@business_type", SqlDbType.NText).Value = productCode
                dt = con.ExecuteDataTable(cmd)
            End Using
            If dt IsNot Nothing And dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                pfPremFinanceCnt = Cast.ToInt32(dr.Item("pfPrem_finance_cnt"), 0)
                pfPremFinanceVersion = Cast.ToInt32(dr.Item("pfPrem_finance_version"), 0)
            End If
        End If
    End Sub

    ''' <summary>
    ''' This method is used to get current insurance file details based on insuranceFileKey used only for MTR (Reinstate)
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="insuranceFileKey"></param>
    ''' <param name="originalInsuranceFileKey"></param>
    ''' <param name="pfPremFinanceCnt"></param>
    ''' <param name="pfPremFinanceVersion"></param>
    ''' <param name="sInsuranceFileStatus"></param>
    ''' <param name="r_nPFPartyBankId"></param>
    ''' <remarks></remarks>
    Private Sub GetCurrentInsuranceFileDetails(
    ByVal con As SiriusConnection,
    ByVal insuranceFileKey As Integer,
    ByRef originalInsuranceFileKey As Integer,
    ByRef pfPremFinanceCnt As Integer,
    ByRef pfPremFinanceVersion As Integer,
    ByRef sInsuranceFileStatus As String,
    Optional ByRef r_nPFPartyBankId As Integer = 0)

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Current_Insurance_File_Details")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            originalInsuranceFileKey = Cast.ToInt32(dr.Item("original_insurance_file_cnt"), 0)
            pfPremFinanceCnt = Cast.ToInt32(dr.Item("pfprem_finance_cnt"), 0)
            pfPremFinanceVersion = Cast.ToInt32(dr.Item("pfprem_finance_version"), 0)
            sInsuranceFileStatus = Convert.ToString(dr.Item("insurance_file_status_description"))
            r_nPFPartyBankId = Cast.ToInt32(dr.Item("party_bank_id"), 0)
        End If

    End Sub

    Private Sub GetInsuranceFileType(
        ByVal con As SiriusConnection,
        ByVal insuranceFileKey As Integer,
        ByRef insuranceFileTypeId As Integer,
        ByRef insuranceFileTypeCode As String)

        Dim dt As DataTable = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Insurance_File_Type")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing And dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            insuranceFileTypeId = Cast.ToInt32(dr.Item("insurance_file_type_id"), 0)
            insuranceFileTypeCode = Cast.ToStringTrim(dr.Item("insurance_file_type_code"), "QUOTE")
        End If

    End Sub

    Private Sub GetInsuranceFileType(
      ByVal con As SiriusConnection,
      ByVal insuranceFileKey As Integer,
      ByRef insuranceFileTypeId As Integer,
      ByRef insuranceFileTypeCode As String,
      ByRef nPolicyVersion As Integer)

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Insurance_File_Type")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            insuranceFileTypeId = Cast.ToInt32(dr.Item("insurance_file_type_id"), 0)
            insuranceFileTypeCode = Cast.ToStringTrim(dr.Item("insurance_file_type_code"), "QUOTE")
            nPolicyVersion = Cast.ToInt32(dr.Item("policy_version"), 0)

        End If

    End Sub

    Private Sub GetInsuranceFileType(
        ByVal con As SiriusConnection,
        ByVal insuranceFileKey As Integer,
        ByRef insuranceFileTypeId As Integer,
        ByRef insuranceFileTypeCode As String,
        ByRef nPolicyVersion As Integer,
        ByRef nIsOOSMTAReplacedVersion As Integer)

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Insurance_File_Type")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            insuranceFileTypeId = Cast.ToInt32(dr.Item("insurance_file_type_id"), 0)
            insuranceFileTypeCode = Cast.ToStringTrim(dr.Item("insurance_file_type_code"), "QUOTE")
            nPolicyVersion = Cast.ToInt32(dr.Item("policy_version"), 0)
            nIsOOSMTAReplacedVersion = Cast.ToInt32(dr.Item("out_of_sequence_replaced"), 0)
        End If

    End Sub

    ''' <summary>
    ''' GetMaxPolicyVersion
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="nInsuranceFileKey"></param>
    ''' <param name="nMaxPolicyVersion"></param>
    ''' <remarks></remarks>
    Private Sub GetMaxPolicyVersion(
        ByVal con As SiriusConnection,
        ByVal nInsuranceFileKey As Integer,
        ByRef nMaxPolicyVersion As Integer)

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_max_policy_version_no")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = nInsuranceFileKey
            cmd.AddOutParameter("@max_version_no", SqlDbType.Int)
            dt = con.ExecuteDataTable(cmd)

            nMaxPolicyVersion = Cast.ToInt32(cmd.Parameters("@max_version_no").Value, 0)
        End Using
    End Sub
    ''' <summary>
    ''' ProcessMTABindQuote
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="coreBusiness"></param>
    ''' <param name="request"></param>
    ''' <remarks></remarks>
    Private Sub ProcessMTABindQuote(
    ByRef con As SiriusConnection,
    ByRef coreBusiness As CoreBusiness,
    ByRef request As BaseBindQuoteRequestType,
    Optional ByRef oResponse As BaseBindQuoteResponseType = Nothing, Optional ByRef r_nDepositTransdetailID As Integer = 0)

        Dim listRisksBusiness As bSIRListRisks.Business = CreateAndInitialiseListRisksBusiness(con, request.BranchCode)
        Dim vDebitTransDetail As Object()
        Dim bSettleTransaction As Boolean
        Dim crTotalPremiumOfAllVersions As Decimal
        Dim crTotalCommissionOfAllVersions As Decimal
        Dim crTotalTaxNotAppliedToClient As Decimal
        Dim iAccountID As Integer = 0
        Try

            ' get insurance file details
            'Pass True flag to get latest live plan on policy
            GetInsuranceFileDetails(con, request, listRisksBusiness, True)

            GetAllLiveFinancePlansOnAPolicy(con, request)

            If request.BankGuaranteeDetails IsNot Nothing Then
                If request.BankGuaranteeDetails.BGKey <> 0 Then
                    ValidateBankGuaranteeTypeData(con, request.BankGuaranteeDetails, request)
                End If
            End If

            If Not request.IsMarketPlacePolicy Then
                ' copy uncopied mta risks
                '   CopyMTARisks(con, listRisksBusiness, request.InsuranceFileKey)
            End If
            If Not request.SkipPolicyLevelTaxesRecalculation Then
                ' recalculate policy fees and policy taxes
                ProcessPolicyLevelFeesAndTaxes(con, request, listRisksBusiness)
            End If



            If request.DataStore.PolicyHasAgent Then
                ' recalculate agent commission 
                ProcessAgentCommission(con, request, listRisksBusiness)
            End If

            If request.IsBackdatedMTA = True Then
                request.DataStore.IsBackDatedMTA = True
            End If

            ' Get the total premium amount on the policy including all fees and taxes
            ' This code was previously present after the ProcessPaymentTerms method. Since Validatecashdeposit needs
            ' TotalPremiumAmount, this code has been moved up.
            request.DataStore.TotalPremiumAmount = GetTotalPremiumAmount(con, request.InsuranceFileKey)
            crTotalPremiumOfAllVersions = GetTotalPremiumAmountForALLPolicyVersions(con, request.DataStore.InsuranceRef, request.InsuranceFileKey, crTotalCommissionOfAllVersions,crTotalTaxNotAppliedToClient)
            DeleteUnselectedRisks(con, request.BranchCode, request.InsuranceFileKey)
            ' validate it is ok to proceed
            ValidatePolicy(con,
                request.TransactionType,
                request.InsuranceFileKey,
                request.DataStore.IsTrueMonthlyPolicy,
                crTotalPremiumOfAllVersions + request.DataStore.TotalAgentPremium,
                crTotalCommissionOfAllVersions + request.DataStore.TotalAgentCommission,
                request.DataStore.PolicyHasAgent,
                request.DataStore.TotalPremiumAmount)

            If request.SelectedCashDeposit IsNot Nothing Then
                ValidateCashDeposit(con, request, coreBusiness)
            End If
            If request IsNot Nothing AndAlso oResponse IsNot Nothing Then
                If request.IsMarketPlacePolicy Then
                    If request.DataStore.TotalPremiumAmount <> request.MarketPlaceTotalPremium Then
                        Dim STSErrorEx As STSErrorPublisher = New STSErrorPublisher(STSErrorCode:=STSErrorCodes.RatingRulesDeclined, Description:="Rating rules declined", Detail:="Rating rules declined")
                        STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "ProcessPolicyV2", "Gross premium changed from initial quote", True)
                        Exit Sub
                    End If
                End If
            End If
            ' TODO : MEvans : Override Posting Period if Allowed

            ' ProcessPaymentTerms
            ' Either Invoice
            ' Pay Now 
            ' Instalments 
            If Not (request.TransactionType.ToUpper() = "MTC" AndAlso request.IsBackdatedMTA = True) Then
                ValidatePolicyPremium(con, request.DataStore.InsuranceRef, request.DataStore.TotalPremiumAmount,
                                      request.InsuranceFileKey, crTotalPremiumOfAllVersions,crTotalTaxNotAppliedToClient)
            End If

            ProcessPaymentTerms(con, coreBusiness, request, vDebitTransDetail, bSettleTransaction)

            ' get the total premium amount on the policy including all fees and taxes

            'Agaist TFS ID 10570
            'ValidatePolicyPremium(con, request.DataStore.InsuranceRef, request.DataStore.TotalPremiumAmount, request.InsuranceFileKey)

            UpdatePolicyDetails(con:=con, branchCode:=request.BranchCode, insuranceFileKey:=request.InsuranceFileKey, transactionType:=request.TransactionType, sInsuranceFileStatus:=request.DataStore.OriginalInsuranceFileStatus)

            UpdatePolicyPaymentDetails(listRisksBusiness, request.InsuranceFileKey, request.DataStore.PaymentMethod, request.PayTrueMonthlyPolicyMTAPremiumOnRenewal, request.DataStore.PaymentTerms, request.DataStore.CollectionFrequency)

            ProcessMakeLive(listRisksBusiness, request, con)

            If request.BankGuaranteeDetails IsNot Nothing AndAlso request.PaymentMethod = PaymentMethodType.BankGuarantee Then
                If request.BankGuaranteeDetails.BGKey <> 0 Then
                    ProcessBankGuarantee(con, request.BankGuaranteeDetails, request)
                End If
            End If

            If Not request.NoTrans Then
                ProcessAccounts(con, request, vDebitTransDetail, bSettleTransaction, iAccountId:=iAccountID)
            End If

            If request.DataStore.IsBackDatedMTA Then
                RequoteBackDatedMTA(con, request)
            End If
            If request.DataStore.PaymentMethod = PolicyPaymentMethod.Instalments AndAlso request.PayTrueMonthlyPolicyMTAPremiumOnRenewal = False Then
                ProcessMTAInstalments(con, request, iAccountId:=iAccountID, r_nDepositTransdetailID:=r_nDepositTransdetailID)
            End If

            If request.TransactionType.ToUpper() = "MTC" Then
                Dim sAutoAllocateFromInception As String = ""
                Dim sWriteOffLimit As String = ""
                Dim sCancellationToleranceDays As String = ""
                Dim oCoreBusiness As New CoreBusiness
                oCoreBusiness.GetSystemOption(request.BranchCode, SystemOption.AutoAllocateCancelFromInception, sAutoAllocateFromInception)
                oCoreBusiness.GetSystemOption(request.BranchCode, SystemOption.AutoAllocateCancelToleranceDays, sCancellationToleranceDays)
                oCoreBusiness.GetSystemOption(request.BranchCode, SystemOption.AutoAllocateCancelToleranceAmount, sWriteOffLimit)
                If ToSafeInteger(sAutoAllocateFromInception) = 1 Then
                    If DateDiff("D", request.DataStore.CoverStartDate, Now()) <= ToSafeInteger(sCancellationToleranceDays) Then
                        AutoAllocateCancellationWithNB(con, request.InsuranceFileKey, request.DataStore.InsuranceFolderKey, request.SourceId, sWriteOffLimit)
                    End If
                End If
            End If
            If request.DataStore.CreateWorkManagerTaskForMTAReturnPremium Then
                CreateWorkManagerTaskForMTAReturnPremium(con, coreBusiness, request)
            End If

        Finally

            If listRisksBusiness IsNot Nothing Then
                listRisksBusiness.Dispose()
                listRisksBusiness = Nothing
            End If
        End Try

    End Sub

    Private Sub AutoAllocateCancellationWithNB(ByVal con As SiriusConnection, ByVal v_lInsuranceFileCnt As Integer,
                                               ByVal v_lInsuranceFolderCnt As Integer, ByVal v_iSourceID As Integer,
                                               ByVal v_sWriteOffLimit As String)
        Dim dvTransDetail_NB As DataView = Nothing
        Dim dvTransDetail_MTC As DataView = Nothing
        Dim ds As DataSet = Nothing

        Dim oDataTable As DataTable = Nothing
        Dim bOtherSideFound As Boolean = False
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_GetTransDetail_Auto_Allocation_For_Cancellation")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = v_lInsuranceFileCnt
            cmd.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = v_lInsuranceFolderCnt

            oDataTable = con.ExecuteDataTable(cmd)
        End Using

        If oDataTable IsNot Nothing Then
            If oDataTable.Rows.Count > 0 Then

                Dim dDebitAmt As Double = 0
                Dim dCreditAmt As Double = 0


                For iCnt As Integer = 0 To oDataTable.Rows.Count - 1

                    If ToSafeString(oDataTable.Rows.Item(iCnt)("transtype")) = "NB" Then
                        If ToSafeDouble(oDataTable.Rows.Item(iCnt)("amount")) > 0 Then
                            dDebitAmt = dDebitAmt + ToSafeDouble(oDataTable.Rows.Item(iCnt)("amount"))
                        End If


                    End If

                    If ToSafeString(oDataTable.Rows.Item(iCnt)("transtype")) = "MTC" Then

                        If ToSafeDouble(oDataTable.Rows.Item(iCnt)("amount")) < 0 Then
                            dCreditAmt = dCreditAmt + ToSafeDouble(oDataTable.Rows.Item(iCnt)("amount"))
                        End If
                    End If
                Next

                Dim dDifferenceAmt As Double = dDebitAmt + dCreditAmt

                Dim bAllowAllocations As Boolean = False

                If dDifferenceAmt = 0 Then
                    bAllowAllocations = True
                End If

                If bAllowAllocations Then
                    Dim oAllocationManual As bACTAllocationManual.Business
                    oAllocationManual = New bACTAllocationManual.Business
                    SAMFunc.InitialiseSBOObject(con, oAllocationManual, _SiriusUser, "bACTAllocationManual.Business")

                    Dim icomReturnValue As Integer
                    icomReturnValue = oAllocationManual.SetProcessModes(vTask:=2)

                    If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMReturnCode.PMNotFound Then
                        RaiseComMethodException("bACTAllocationManual.Business.SetProcessModes", icomReturnValue)
                    End If

                    Dim vKeys(0 To 1, 5) As Object

                    For iCnt As Integer = 0 To oDataTable.Rows.Count - 1

                        If ToSafeString(oDataTable.Rows.Item(iCnt)("transtype")) = "NB" Then

                            vKeys(0, 0) = "account_id"
                            vKeys(1, 0) = oDataTable.Rows.Item(iCnt)("account_id")

                            'TransdetailKey
                            vKeys(0, 1) = "trans_detail_id"
                            vKeys(1, 1) = ToSafeInteger(oDataTable.Rows.Item(iCnt)("transdetail_id")) & "|" & ToSafeDouble(oDataTable.Rows.Item(iCnt)("amount"))

                            ' AllocationTransdetailKeys
                            vKeys(0, 2) = "trans_detail_ids"


                            Dim iWriteOffReasonID As Integer = 0
                            Dim dWriteOffAmt As Double = 0

                            Dim oAllocationDetails() As Object = Nothing
                            ReDim oAllocationDetails(0)

                            bOtherSideFound = False
                            For iCanelledTranCnt As Integer = 0 To oDataTable.Rows.Count - 1
                                If oDataTable.Rows.Item(iCanelledTranCnt)("transtype").ToString() = "MTC" Then
                                    If ToSafeInteger(oDataTable.Rows.Item(iCanelledTranCnt)("account_id")) = ToSafeInteger(oDataTable.Rows.Item(iCnt)("account_id")) Then
                                        ' If ToSafeString(oDataTable.Rows.Item(iCanelledTranCnt)("spare")) = ToSafeString(oDataTable.Rows.Item(iCnt)("spare")) Then

                                        Dim dTranDiffAmt As Double = Math.Round(ToSafeDouble(oDataTable.Rows.Item(iCanelledTranCnt)("amount")) + ToSafeDouble(oDataTable.Rows.Item(iCnt)("amount")), 2)

                                        If dTranDiffAmt = 0 Then
                                            oAllocationDetails(0) = ToSafeInteger(oDataTable.Rows.Item(iCanelledTranCnt)("transdetail_id")) & "|" & ToSafeDouble(oDataTable.Rows.Item(iCanelledTranCnt)("amount"))

                                            bOtherSideFound = True

                                            Exit For

                                        End If
                                        ' End If
                                    End If
                                End If
                            Next

                            If bOtherSideFound Then
                                vKeys(1, 2) = oAllocationDetails

                                vKeys(0, 3) = "writeoff_reason_id"
                                vKeys(1, 3) = 0

                                vKeys(0, 4) = "writeoff_amount"
                                vKeys(1, 4) = 0

                                vKeys(0, 5) = "currency_difference"
                                vKeys(1, 5) = 0


                                ' Set the keys
                                icomReturnValue = oAllocationManual.SetKeys(vKeyArray:=DirectCast(vKeys, Object(,)))
                                If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMReturnCode.PMNotFound Then
                                    RaiseComMethodException("bACTAllocationManual.Business.SetKeys", icomReturnValue)
                                End If

                                oAllocationManual.CompanyId = v_iSourceID

                                icomReturnValue = oAllocationManual.Start()
                                If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMReturnCode.PMNotFound Then
                                    RaiseComMethodException("bACTAllocationManual.Business.Start", icomReturnValue)
                                End If
                            End If

                        End If
                    Next

                    'now check again if further allocation required with unmatched but in total equal transactions

                    Dim iMaxCounter As Integer = 0

                    Do While True

                        oDataTable = Nothing
                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_GetTransDetail_Auto_Allocation_For_Cancellation")
                            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = v_lInsuranceFileCnt
                            cmd.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = v_lInsuranceFolderCnt

                            oDataTable = con.ExecuteDataTable(cmd)
                        End Using


                        If oDataTable Is Nothing Then
                            Exit Do
                        End If

                        If oDataTable.Rows.Count = 0 Then
                            Exit Do
                        End If

                        If iMaxCounter > 20 Then
                            Exit Do
                        End If

                        iMaxCounter = iMaxCounter + 1

                        For iCnt As Integer = 0 To oDataTable.Rows.Count - 1

                            If ToSafeString(oDataTable.Rows.Item(iCnt)("transtype")) = "NB" Then

                                vKeys(0, 0) = "account_id"
                                vKeys(1, 0) = oDataTable.Rows.Item(iCnt)("account_id")

                                'TransdetailKey
                                vKeys(0, 1) = "trans_detail_id"
                                vKeys(1, 1) = ToSafeInteger(oDataTable.Rows.Item(iCnt)("transdetail_id")) & "|" & ToSafeDouble(oDataTable.Rows.Item(iCnt)("outstanding_amount"))

                                ' AllocationTransdetailKeys
                                vKeys(0, 2) = "trans_detail_ids"


                                Dim iWriteOffReasonID As Integer = 0
                                Dim dWriteOffAmt As Double = 0

                                Dim oAllocationDetails() As Object = Nothing
                                ReDim oAllocationDetails(0)

                                bOtherSideFound = False

                                For iCanelledTranCnt As Integer = 0 To oDataTable.Rows.Count - 1
                                    If oDataTable.Rows.Item(iCanelledTranCnt)("transtype").ToString() = "MTC" Then

                                        If Math.Abs(ToSafeDouble(oDataTable.Rows.Item(iCnt)("outstanding_amount"))) > Math.Abs(ToSafeDouble(oDataTable.Rows.Item(iCanelledTranCnt)("outstanding_amount"))) Then
                                            oAllocationDetails(0) = ToSafeInteger(oDataTable.Rows.Item(iCanelledTranCnt)("transdetail_id")) & "|" & ToSafeDouble(oDataTable.Rows.Item(iCanelledTranCnt)("outstanding_amount"))
                                            vKeys(1, 1) = ToSafeInteger(oDataTable.Rows.Item(iCnt)("transdetail_id")) & "|" & ToSafeDouble(oDataTable.Rows.Item(iCanelledTranCnt)("outstanding_amount")) * -1

                                            bOtherSideFound = True
                                            Exit For
                                        Else
                                            oAllocationDetails(0) = ToSafeInteger(oDataTable.Rows.Item(iCanelledTranCnt)("transdetail_id")) & "|" & ToSafeDouble(oDataTable.Rows.Item(iCnt)("outstanding_amount")) * -1

                                            bOtherSideFound = True
                                            Exit For
                                        End If


                                    End If
                                Next

                                If bOtherSideFound Then
                                    vKeys(1, 2) = oAllocationDetails

                                    vKeys(0, 3) = "writeoff_reason_id"
                                    vKeys(1, 3) = 0


                                    vKeys(0, 4) = "writeoff_amount"
                                    vKeys(1, 4) = 0

                                    vKeys(0, 5) = "currency_difference"
                                    vKeys(1, 5) = 0


                                    ' Set the keys
                                    icomReturnValue = oAllocationManual.SetKeys(vKeyArray:=DirectCast(vKeys, Object(,)))
                                    If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMReturnCode.PMNotFound Then
                                        RaiseComMethodException("bACTAllocationManual.Business.SetKeys", icomReturnValue)
                                    End If

                                    oAllocationManual.CompanyId = v_iSourceID

                                    icomReturnValue = oAllocationManual.Start()
                                    If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMReturnCode.PMNotFound Then
                                        RaiseComMethodException("bACTAllocationManual.Business.Start", icomReturnValue)
                                    End If

                                    Exit For
                                End If
                            End If
                        Next
                    Loop


                    If oAllocationManual IsNot Nothing Then
                        oAllocationManual.Dispose()
                        oAllocationManual = Nothing
                    End If
                    oAllocationManual = Nothing
                End If
            End If
        End If


    End Sub

    Private Sub CreateWorkManagerTaskForMTAReturnPremium(
    ByVal con As SiriusConnection,
    ByVal coreBusiness As CoreBusiness,
    ByVal request As BaseBindQuoteRequestType)

        Dim optionValue As String = String.Empty
        Dim partyShortname As String = String.Empty
        Dim taskDescription As String = String.Empty

        If request.DataStore.PolicyHasAgent Then
            coreBusiness.GetSystemOption(request.BranchCode, SystemOption.AgentBusinessReturnPremiumUserGroup, optionValue)
            partyShortname = request.DataStore.AgentShortname
            taskDescription = "Return Premium Payment for Agent " + partyShortname + " of " + request.DataStore.RefundAmount.ToString + " is required"
        Else
            coreBusiness.GetSystemOption(request.BranchCode, SystemOption.DirectBusinessReturnPremiumUserGroup, optionValue)
            partyShortname = request.DataStore.PartyShortname
            taskDescription = "Return Premium Payment for Client " + partyShortname + " of " + request.DataStore.RefundAmount.ToString + " is required"
        End If

        ' default user goup id - to system adminstrators
        Dim defaultUserGroupId As Integer = coreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.UserGroup, UserGroup.SystemAdministrator)
        Dim userGroupId As Integer = defaultUserGroupId

        ' use return premium system option user group is set correctly
        If Not String.IsNullOrEmpty(optionValue) Then
            If Not Integer.TryParse(optionValue, userGroupId) Then
                userGroupId = defaultUserGroupId
            End If
        End If

        ' task id
        Dim taskId As Integer = coreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Task, Task.Memo)

        ' task group 
        Dim taskGroupId As Integer = coreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.TaskGroup, TaskGroup.Common)

        ' create external handler tasks
        Dim clientName As String = partyShortname
        Dim taskInstanceCnt As Integer

        '' create external handler acknowledged task
        CreateTaskInstance(con, taskInstanceCnt, taskGroupId, taskId,
            clientName, Date.Today, userGroupId, Nothing,
            taskDescription, PMEWrkManTaskStatus.pmeWMTSNew, 0, Date.Today,
              _SiriusUser.UserID, Date.Today, _SiriusUser.UserID, 1, String.Empty)

    End Sub

    Private Sub CreateWorkManagerTaskForRenewal(
    ByVal con As SiriusConnection,
    ByVal coreBusiness As CoreBusiness,
    ByVal request As BaseBindQuoteRequestType)

        Dim ds As DataSet = Nothing
        Dim sPartyShortName As String = ""
        Dim sPolicyNumber As String = ""
        Dim iTaskInstanceCnt As Integer
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Party_Shortname")
            cmd.AddInParameter("@partycnt", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = request.InsuranceFileKey
            ds = con.ExecuteDataSet(cmd, "dr")
        End Using

        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            sPartyShortName = Cast.ToString(ds.Tables(0).Rows(0).Item("shortname"), "")
            sPolicyNumber = Cast.ToString(ds.Tables(0).Rows(0).Item("insurance_ref"), "")
        End If

        CreateTaskInstance(con:=con, lTaskInstanceCnt:=iTaskInstanceCnt, lTaskGroupId:=11, lTaskId:=214,
                sCustomer:=sPartyShortName, dtTasKDueDate:=DateAdd("ww", 1, Date.Today), lUserGroupId:=1, lUserId:=_SiriusUser.UserID,
                sDescription:="Renewal - " + sPolicyNumber + "Failed due to rounding amount greater than 1", lTaskStatus:=PMEWrkManTaskStatus.pmeWMTSNew, lIsUrgent:=1, dtDateCreated:=Date.Today,
                lCreatedById:=_SiriusUser.UserID, dtLastModified:=Date.Today, lModifiedById:=_SiriusUser.UserID, lIsVisible:=1, sWorkflowInformation:=String.Empty)

    End Sub

    Private Sub ValidateSelectedQuote(
    ByRef premiumFinanceBusiness As bSIRPremiumFinance.Business,
    ByRef request As BaseBindQuoteRequestType,
    ByRef instalmentQuotes As Object(,),
    ByRef matchingRatePositionInQuoteArray As Integer,
    Optional ByVal con As SiriusConnection = Nothing)

        Const QuoteSchemeNo As Integer = 2
        Const QuoteSchemeVersion As Integer = 3
        Const QuoteRateId As Integer = 32

        If request.TransactionType = TransactionTypeCode.NewBusiness Then
            instalmentQuotes = GetNewBusinessInstalmentQuotes(premiumFinanceBusiness, request)
        Else
            ' If it's an MTA or Renewal then get the original premium finance details
            If request.DataStore.PFPremiumFinanceCnt = 0 AndAlso (request.TransactionType = TransactionTypeCode.MTA OrElse request.TransactionType = TransactionTypeCode.Renewals) Then
                GetOriginalInsuranceFileDetails(con, request.InsuranceFileKey, request.DataStore.OriginalInsuranceFileKey, request.DataStore.PFPremiumFinanceCnt, request.DataStore.PFPremiumFinanceVersion)
            End If
            instalmentQuotes = GetMTAOrRenewalInstalmentQuotes(premiumFinanceBusiness, request)
        End If

        Dim foundSelectedRate As Boolean = False

        If IsArray(instalmentQuotes) Then

            Dim lowerBound As Integer = instalmentQuotes.GetLowerBound(1)
            Dim upperBound As Integer = instalmentQuotes.GetUpperBound(1)

            For rate As Integer = lowerBound To upperBound

                Dim schemeNo As Integer = Cast.ToInt32(instalmentQuotes(QuoteSchemeNo, rate), 0)
                Dim schemeVersion As Integer = Cast.ToInt32(instalmentQuotes(QuoteSchemeVersion, rate), 0)
                Dim rateId As Integer = Cast.ToInt32(instalmentQuotes(QuoteRateId, rate), 0)

                If schemeNo = request.SelectedSchemeNo AndAlso
                    schemeVersion = request.SelectedSchemeVersion AndAlso
                     rateId = request.PFRF_ID Then

                    matchingRatePositionInQuoteArray = rate
                    foundSelectedRate = True
                    Exit For

                End If

            Next

        End If

        If Not foundSelectedRate Then
            Dim samErrorCollection As New SAMErrorCollection()
            samErrorCollection.AddBusinessRule(SAMBusinessErrors.NoMatchingInstalmentQuoteFoundForSpecifiedDetails,
                    SAMBusinessErrors.NoMatchingInstalmentQuoteFoundForSpecifiedDetails.ToString,
                    "Instalment Quote not found for scheme no :" + request.SelectedSchemeNo.ToString() +
                    " scheme version : " + request.SelectedSchemeVersion.ToString() +
                    " pfrf_id : " + request.PFRF_ID.ToString())
            samErrorCollection.CheckForErrors()
        End If

    End Sub

    Private Function GetNewBusinessInstalmentQuotes(
    ByRef premiumFinanceBusiness As bSIRPremiumFinance.Business,
    ByVal request As BaseBindQuoteRequestType) As Object(,)

        Dim quoteArrayObject As Object = Nothing
        Dim quoteArray As Object(,) = Nothing
        Dim comReturnValue As Integer

        '' As in case of BO call of Calculate_Quotes it has to be null
        request.StartDate = Date.MinValue

        premiumFinanceBusiness.UseTransCurrency = 0

        If request.IsUseTransactionCurrency Then
            premiumFinanceBusiness.UseTransCurrency = 1
        End If

        comReturnValue = premiumFinanceBusiness.Calculate_Quotes(
            v_lSourceID:=request.DataStore.SourceId,
            v_sProductCode:=request.TransactionType,
            v_dtQuoteDate:=request.QuoteDate,
            v_dtStartDate:=request.StartDate,
            v_dtEndDate:=request.EndDate,
            v_dtPreferredDate:=request.PreferredDate,
            v_iDayInMonth:=CShort(request.MonthDay),
            v_iDayInWeek:=CShort(request.WeekDay),
            v_crAmountToFinance:=CDec(Cast.DefaultIfNull(request.AmountToFinance, 0)),
            v_bPaymentProtection:=request.PaymentProtection,
            v_dInterestOverrideRate:=request.OverrideInterestRate,
            v_bOverrideCommission:=False,
            v_lPartyCnt:=0,
            r_vQuoteArray:=quoteArrayObject,
            v_lInsuranceFileCnt:=request.InsuranceFileKey,
            v_iMTAType:=m_klInstalmentMTAType_AddAndSpread,
            r_lPremFinanceCnt:=request.DataStore.PFPremiumFinanceCnt,
            r_lPremFinanceVer:=request.DataStore.PFPremiumFinanceVersion,
            v_dOverrideDeposit:=request.InstDepositAmount,
            bDayOfMonthIsValid:=request.DayOfMonthIsValid,
            bIsCallingFromSAM:=request.IsCallingFromSAM)

        If comReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRPremiumFinance.Business.Calculate_Quotes", comReturnValue)
        End If

        quoteArray = TryCast(quoteArrayObject, Object(,))

        Return quoteArray

    End Function

    Private Function GetMTAOrRenewalInstalmentQuotes(
    ByRef premiumFinanceBusiness As bSIRPremiumFinance.Business,
    ByVal request As BaseBindQuoteRequestType) As Object(,)

        Dim quoteArrayObject As Object = Nothing
        Dim quoteArray As Object(,) = Nothing
        Dim comReturnValue As Integer
        'Begin WPR36A
        Dim iInstalmentType As Integer = 0

        If InstalmentType.AddAndSpread = request.InstalmentType Then
            iInstalmentType = InstalmentType.AddAndSpread
        ElseIf InstalmentType.AddToNewPlan = request.InstalmentType Then
            iInstalmentType = InstalmentType.AddToNewPlan
        ElseIf InstalmentType.AddToNext = request.InstalmentType Then
            iInstalmentType = InstalmentType.AddToNext
        End If
        Dim sTransactionType As String
        sTransactionType = request.TransactionType
        If sTransactionType = "MTR" Then
            sTransactionType = "NB"
        End If
        'End WPR36A

        premiumFinanceBusiness.UseTransCurrency = 0
        If request.IsUseTransactionCurrency Then
            premiumFinanceBusiness.UseTransCurrency = 1
        End If

        ''reset m_iDayOfWeekOrMonth to refresh Day in month during MTA
        premiumFinanceBusiness.m_iDayOfWeekOrMonth = 1
        comReturnValue = premiumFinanceBusiness.Calculate_Quotes(
            v_lSourceID:=request.DataStore.SourceId,
            v_sProductCode:=sTransactionType,
            v_dtQuoteDate:=request.QuoteDate,
            v_dtStartDate:=request.StartDate,
            v_dtEndDate:=request.EndDate,
            v_dtPreferredDate:=request.PreferredDate,
            v_iDayInMonth:=CShort(request.MonthDay),
            v_iDayInWeek:=CShort(request.WeekDay),
            v_crAmountToFinance:=CDec(Cast.DefaultIfNull(request.AmountToFinance, 0)),
            v_bPaymentProtection:=request.PaymentProtection,
            v_dInterestOverrideRate:=request.OverrideInterestRate,
            v_bOverrideCommission:=False,
            v_lPartyCnt:=0,
            r_vQuoteArray:=quoteArrayObject,
            v_lInsuranceFileCnt:=request.DataStore.OriginalInsuranceFileKey,
            v_lRenewalInsFileCnt:=request.InsuranceFileKey,
            v_iMTAType:=CShort(iInstalmentType),
            r_lPremFinanceCnt:=request.DataStore.PFPremiumFinanceCnt,
            r_lPremFinanceVer:=request.DataStore.PFPremiumFinanceVersion,
            v_dOverrideDeposit:=request.InstDepositAmount,
            bIsCallingFromSAM:=request.IsCallingFromSAM)

        If comReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRPremiumFinance.Business.Calculate_Quotes", comReturnValue)
        End If

        quoteArray = TryCast(quoteArrayObject, Object(,))

        Return quoteArray

    End Function

    Private Sub ProcessMTAInstalments(
    ByRef con As SiriusConnection,
    ByVal request As BaseBindQuoteRequestType, Optional ByVal iAccountId As Integer = 0, Optional ByRef r_nDepositTransdetailID As Integer = 0)

        Const NoDeposit As Boolean = False

        Dim premiumFinanceBusiness As bSIRPremiumFinance.Business =
         CreateAndInitialisePremiumFinanceBusiness(con, request.BranchCode)

        Dim iCreatePartyBankRecord As Int16 = 1

        Try

            'Begin WPR36A
            Dim iInstalmentType As Integer = 0

            If InstalmentType.AddAndSpread = request.InstalmentType Then
                iInstalmentType = InstalmentType.AddAndSpread
            ElseIf InstalmentType.AddToNewPlan = request.InstalmentType Then
                iInstalmentType = InstalmentType.AddToNewPlan
            ElseIf InstalmentType.AddToNext = request.InstalmentType Then
                iInstalmentType = InstalmentType.AddToNext
            End If
            'End WPR36A
            ' ************************************
            ' This method was extracted from the following components
            ' ************************************

            ' iPMUListRisks - Refresh Quotes
            '       - Calculate And Validate
            ' iPMUListRisk - SaveQuote
            '       - Insert or Update Premium Finance Plan
            ' iPMBFinancePlanMaint - load
            '       - get Single Premium Finance Plan

            ' iPMBFinancePlanMaint - cmd_Save
            '       - bSIRPRemiumFinance.UpdateExistingRecord
            '       - bSIRPRemiumFinance.SaveInstalmentsPlanMediaTypeDetails
            '       - bSIRPremiumFinance.DeletePFTransID 
            '       - bSIRPremiumFinance.InsertPFTransID
            '       - bSIRPremiumFinance.AddPartners
            '       - bSIRPremiumFinance.CreateEvent

            ' iPMBFinancePlanMaint - cmd_Transact
            '       - bSIRPremiumFinance.CheckAllocationAgainstPolicy
            '       - ValidateDataEntry
            '       - bSirMediaTypeValidation.Business.ValidateNumber
            '       - bSIRPremiumFinance.BeginTrans
            '           - bSIRPremiumFinance.ProcessPlan
            '           - bSIRPremiumFinance.StatusUpdate
            '       - bSIRPremiumFinance.CommitTrans

            If request.CreditCard IsNot Nothing Then
                'WPR 33-75 END
                ' Start - (Sankar) - (Tech Spec - ACR006 - FDMS Tacking Number) - (6.3.2)
                premiumFinanceBusiness.DepositCCTrackingNumber = request.CreditCard.TrackingNumber
                ' End - (Sankar) - (Tech Spec - ACR006 - FDMS Tacking Number) - (6.3.2)

                premiumFinanceBusiness.AccountType = request.CreditCard.AccountType
                premiumFinanceBusiness.VIAPaymentHub = request.CreditCard.VIAPaymentHub
            End If

            premiumFinanceBusiness.InsuranceFileCnt = request.InsuranceFileKey
            premiumFinanceBusiness.TransType = request.TransactionType
            premiumFinanceBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)

            Dim instalmentQuotes As Object(,) = Nothing
            Dim matchingRatePositionInQuoteArray As Integer

            ' if the selected quote is not found a business error will be thrown
            ValidateSelectedQuote(
                premiumFinanceBusiness,
                request,
                instalmentQuotes,
                matchingRatePositionInQuoteArray, con)

            premiumFinanceBusiness.InsuranceFileCnt = request.InsuranceFileKey

            Dim comReturnValue As Integer
            comReturnValue = premiumFinanceBusiness.InsertOrUpdatePremiumFinance(
                instalmentQuotes,
                matchingRatePositionInQuoteArray,
                request.DataStore.PFTransactions,
                request.DataStore.PFPremiumFinanceCnt,
                request.DataStore.PFPremiumFinanceVersion,
                CShort(iInstalmentType))




            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.InsertOrUpdatePremiumFinance", comReturnValue)
            End If

            Dim premiumFinanceArrayObject As Object(,) = Nothing

            comReturnValue = premiumFinanceBusiness.GetSingleFinancePlan(
                request.DataStore.PFPremiumFinanceCnt,
                request.DataStore.PFPremiumFinanceVersion,
                premiumFinanceArrayObject)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", comReturnValue)
            End If

            If premiumFinanceArrayObject Is Nothing OrElse Not IsArray(premiumFinanceArrayObject) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", comReturnValue)
            End If

            Dim premiumFinanceArray As Object(,) = CType(premiumFinanceArrayObject, Object(,))

            premiumFinanceArray(k_PFPlanDateModified, 0) = Date.Today
            premiumFinanceArray(k_PfPlanDateBankDetailsChanged, 0) = Date.Today

            If request.CreditCard IsNot Nothing Then
                ' Start - (Prakash C Varghese)- Copied from ProcessNBInstalments
                premiumFinanceBusiness.DepositCCTrackingNumber = request.CreditCard.TrackingNumber
                premiumFinanceBusiness.AccountType = request.CreditCard.AccountType

                If request.CreditCard.PartyBankKey > 0 Then
                    iCreatePartyBankRecord = 0
                End If

                PopulatePremiumFinanceCreditCardDetails(request, premiumFinanceArray)
            Else

                If request.PartyBankKey > 0 Then
                    iCreatePartyBankRecord = 0
                End If

                PopulatePremiumFinanceBankDetails(request, premiumFinanceArray)
            End If

            Dim premiumFinanceCntObject As Integer = request.DataStore.PFPremiumFinanceCnt
            Dim premiumFinanceVersionObject As Integer = request.DataStore.PFPremiumFinanceVersion

            premiumFinanceArrayObject = CType(premiumFinanceArray, Object(,))

            'Modified the value of iCreatePartyBankRecord parameter. 
            'If request objects party bank key value is non zero, the party bank item is already created. 
            'No need to create it again.
            comReturnValue = premiumFinanceBusiness.UpdateExistingRecord(
                    vExistingRecord:=CType(CObj(premiumFinanceArrayObject), System.Array),
                    vPremiumFinanceCnt:=premiumFinanceCntObject,
                    vPremiumFinanceVersion:=premiumFinanceVersionObject,
                    nArrayIndex:=0, vPremiumFinanceMTA:=Nothing,
                    iCreatePartyBankRecord:=CInt(iCreatePartyBankRecord))
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.UpdateExistingRecord", comReturnValue)
            End If

            ' create initial bank history record
            comReturnValue = premiumFinanceBusiness.SaveInstalmentsPlanMediaTypeDetails(
                 request.DataStore.PFPremiumFinanceCnt,
                 request.DataStore.PFPremiumFinanceVersion,
                 InstalmentHistoryActionType.Amendment)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.SaveInstalmentsPlanMediaTypeDetails", comReturnValue)
            End If

            comReturnValue = premiumFinanceBusiness.DeletePFTransID(
                 request.DataStore.PFPremiumFinanceCnt,
                request.DataStore.PFPremiumFinanceVersion)
            If (comReturnValue <> PMEReturnCode.PMTrue) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.DeletePFTransID", comReturnValue)
            End If

            Dim pfTransactionsArrayObject As Object = CType(request.DataStore.PFTransactions, Object)
            comReturnValue = premiumFinanceBusiness.InsertPFTransID(
                request.DataStore.PFPremiumFinanceCnt,
                request.DataStore.PFPremiumFinanceVersion,
                 CType(CObj(pfTransactionsArrayObject), Object(,)))
            If (comReturnValue <> PMEReturnCode.PMTrue) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.InsertPFTransID", comReturnValue)
            End If

            comReturnValue = premiumFinanceBusiness.CreateEvent(
                CStr(PMEComponentAction.PMEdit),
                request.InsuranceFileKey,
                request.DataStore.PartyKey)
            If (comReturnValue <> PMEReturnCode.PMTrue) Then
                RaiseComMethodException("bSIRPremiumFinance.Business.CreateEvent", comReturnValue)
            End If

            Dim pfPlanTransDetailId As Integer = 0
            Dim depositTransDetailsId As Object = Nothing

            Dim lMode As Integer
            If premiumFinanceArrayObject IsNot Nothing AndAlso Convert.ToInt32(premiumFinanceArrayObject(k_PFPlanSchemeType_ID, 0)) = 1 Then
                lMode = InstalmentsMode.ThirdParty
            Else
                lMode = InstalmentsMode.MidTermAdjustment
            End If

            If Not request.NoTrans Then
                comReturnValue = premiumFinanceBusiness.ProcessPlan(
                    lMode,
                    premiumFinanceArrayObject,
                    CType(CObj(pfTransactionsArrayObject), Object(,)),
                    pfPlanTransDetailId,
                    NoDeposit,
                    depositTransDetailsId,
                        CShort(iInstalmentType), v_lAccountId:=iAccountId)

                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.ProcessPlan", comReturnValue)
                End If
                If Not depositTransDetailsId Is Nothing Then
                    r_nDepositTransdetailID = CType(depositTransDetailsId, Integer)
                End If
            End If
            If CDbl(premiumFinanceArrayObject(k_PFPlanTotalCost, 0)) = 0 Then
                comReturnValue = premiumFinanceBusiness.StatusUpdate(vPremiumFinanceCnt:=premiumFinanceCntObject,
                                                            vPremiumFinanceVersion:=premiumFinanceVersionObject,
                                                            vStatusInd:=InstalmentPlanStatus.Completed)
                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.StatusUpdate Failed to update the Finance Plan Status to Completed")
                End If
                comReturnValue = premiumFinanceBusiness.UpdateInstalmentStatusToCollected(v_nPremiumFinanceCnt:=request.DataStore.PFPremiumFinanceCnt,
                                                     v_nPremiumFinanceVersion:=request.DataStore.PFPremiumFinanceVersion)
                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.UpdateInstalmentStatusToCollected Failed to update the Instalments Status to Collected")
                End If
            Else
                ' Update the plans status to live
                comReturnValue = premiumFinanceBusiness.StatusUpdate(
                    premiumFinanceCntObject,
                    premiumFinanceVersionObject,
                    InstalmentPlanStatus.Live)
                If (comReturnValue <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.StatusUpdate Failed to update the Finance Plan Status to Live")
                End If
            End If

            If request.TransactionType = "MTR" Then
                request.InstalmentType = InstalmentType.AddToNewPlan
            End If

            If request.InstalmentType = InstalmentType.AddToNewPlan Then
                If IsArray(premiumFinanceArray) Then
                    sAutoPlanRef = Convert.ToString(premiumFinanceArray(k_PFPlanAutoGenPlanRef, 0)).Trim
                End If
            End If
        Finally
            If premiumFinanceBusiness IsNot Nothing Then
                premiumFinanceBusiness.Dispose()
                premiumFinanceBusiness = Nothing
            End If
        End Try

    End Sub

    Private Function GetTotalPremiumAmountForALLPolicyVersions(
    ByVal con As SiriusConnection,
            ByVal insuranceRef As String,
     ByVal insurance_file_cnt As Integer) As Decimal

        Const TotalPremium As Integer = 0
        Dim dt As DataTable = Nothing

        Dim totalPremiumAmount As Decimal = 0

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Total_Premium_Amount_For_All_Policy_Versions")
            cmd.AddInParameter("@insurance_ref", SqlDbType.VarChar, 30).Value = insuranceRef
            cmd.AddInParameter("@nFileCnt", SqlDbType.Int).Value = insurance_file_cnt
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            totalPremiumAmount = Cast.ToDecimal(dt.Rows(0).Item(TotalPremium), 0)
        End If

        Return totalPremiumAmount

    End Function

    Private Function GetTotalPremiumAmountForALLPolicyVersions(
    ByVal con As SiriusConnection,
            ByVal insuranceRef As String,
            ByVal insurance_file_cnt As Integer,
            ByRef totalCommissionOfAllVersions As Decimal,
            ByRef dTotalTaxNotAppliedToClient As Decimal) As Decimal

        Const TotalPremium As Integer = 0
        Const TotalCommission As Integer = 1
        Const TotalTaxNotAppliedToClient As Integer = 2
        Dim dt As DataTable = Nothing

        Dim totalPremiumAmount As Decimal = 0

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Total_Premium_Amount_For_All_Policy_Versions")
            cmd.AddInParameter("@insurance_ref", SqlDbType.VarChar, 30).Value = insuranceRef
            cmd.AddInParameter("@nFileCnt", SqlDbType.Int).Value = insurance_file_cnt
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            totalPremiumAmount = Cast.ToDecimal(dt.Rows(0).Item(TotalPremium), 0)
            totalCommissionOfAllVersions = Cast.ToDecimal(dt.Rows(0).Item(TotalCommission), 0)
            dTotalTaxNotAppliedToClient = Cast.ToDecimal(dt.Rows(0).Item(TotalTaxNotAppliedToClient), 0)
        End If

        Return totalPremiumAmount

    End Function

    Private Function GetTotalPremiumAmount(
    ByVal con As SiriusConnection,
    ByVal insuranceFileKey As Integer) As Decimal

        Const TotalPremium As Integer = 0
        Dim dt As DataTable = Nothing

        Dim totalPremiumAmount As Decimal = 0

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Total_Premium_Amount")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            totalPremiumAmount = Cast.ToDecimal(dt.Rows(0).Item(TotalPremium), 0)
        End If

        Return totalPremiumAmount

    End Function

    Private Sub UpdatePolicyPaymentDetails(
    ByRef listRisksBusiness As bSIRListRisks.Business,
    ByVal insuranceFileKey As Integer,
    ByVal paymentMethod As String,
    ByVal putTrueMonthlyMTAPRemiumOnNextRenewal As Boolean,
    Optional ByVal paymentTermsId As Integer = 0,
    Optional ByVal collectionFrequencyId As Integer = 0)

        Dim nTrueMonthlyMTAPremiumOnNextRenewal As Integer
        If putTrueMonthlyMTAPRemiumOnNextRenewal Then
            nTrueMonthlyMTAPremiumOnNextRenewal = 1
        Else
            nTrueMonthlyMTAPremiumOnNextRenewal = 0
        End If

        Dim nComReturnValue As Integer

        nComReturnValue = listRisksBusiness.UpdatePolicyDetails(v_lInsuranceFileCnt:=insuranceFileKey, v_lPutOnNextInstalmentRenewal:=nTrueMonthlyMTAPremiumOnNextRenewal, v_sPaymentMethod:=paymentMethod, v_nCollectionFrequency:=collectionFrequencyId, v_nDOPaymentTerms:=paymentTermsId)
        If nComReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRListRisks.Business.UpdatePolicyDetails", nComReturnValue)
        End If

    End Sub

    Private Sub ProcessMakeLive(
    ByRef listRisksBusiness As bSIRListRisks.Business,
    ByRef request As BaseBindQuoteRequestType,
    ByRef con As SiriusConnection)

        Dim comReturnValue As Integer
        Dim oReturnObject(,) As Object = Nothing
        Dim RunRenewalSelectionRequest As New BaseRunRenewalSelectionRequestType
        comReturnValue = listRisksBusiness.ProcessPolicyMakeLive(request.InsuranceFileKey)
        If comReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRListRisks.Business.ProcessPolicyMakeLive", comReturnValue)
        End If

        If String.IsNullOrEmpty(request.TransactionType) = False AndAlso (request.TransactionType = "MTA" Or request.TransactionType = "MTR" Or request.TransactionType = "REN" Or request.TransactionType = "MTC") Then

            comReturnValue = listRisksBusiness.GetMTAQuotePolicyVersions(v_lInsuranceFileCnt:=request.InsuranceFileKey,
                                                              v_lInsuranceFolderCnt:=request.DataStore.InsuranceFolderKey,
                                                              r_vResults:=oReturnObject)


            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRListRisks.Business.GetMTAQuotePolicyVersions", comReturnValue)
            End If

            If oReturnObject IsNot Nothing AndAlso TypeOf (oReturnObject) Is System.Array Then

                Dim vReturnArray As Object(,) = DirectCast(oReturnObject, Object(,))
                Dim iRowFrom As Integer
                Dim iRowTo As Integer
                Dim lInsuranceFileCnt As Integer
                Dim lInsuranceFolderCnt As Integer
                iRowFrom = CShort(vReturnArray.GetLowerBound(1))
                iRowTo = CShort(vReturnArray.GetUpperBound(1))

                Dim oInsuranceFile As bSIRInsuranceFile.Business = Nothing

                Try
                    oInsuranceFile = New bSIRInsuranceFile.Business

                    SAMFunc.InitialiseSBOObject(con, oInsuranceFile, _SiriusUser, "", "bSIRInsuranceFile.Business")

                    For iRow As Integer = iRowFrom To iRowTo
                        lInsuranceFileCnt = Convert.ToInt32(vReturnArray(0, iRow))
                        lInsuranceFolderCnt = Convert.ToInt32(vReturnArray(1, iRow))

                        oInsuranceFile.MTACancellation(v_lInsuranceFileCnt:=lInsuranceFileCnt,
                                                               v_lInsuranceFolderCnt:=lInsuranceFolderCnt,
                                                               v_lPartyCnt:=request.DataStore.PartyKey)

                        If comReturnValue <> PMEReturnCode.PMTrue Then
                            RaiseComMethodException("bSIRInsuranceFile.Business.MTACancellation", comReturnValue)
                        End If

                    Next
                Catch ex As Exception
                    ExceptionManager.Publish(ex)
                    Debug.WriteLine(ex.Message)
                Finally
                    If oInsuranceFile IsNot Nothing Then
                        oInsuranceFile.Dispose()
                        oInsuranceFile = Nothing
                    End If
                End Try
            End If

            If request.TransactionType = "MTA" Then

                Dim oRenewalBusiness As bSIRRenewal.Business = Nothing
                Dim nRenewalStatusTypeId As Int32
                Dim dsRenewalStatus As DataSet = Nothing
                Dim nIsTrueMonthlyPolicy As Integer
                Dim nDoNotDeleteRenQuoteOnMTA As Integer
                Dim nDeleteRenQuoteReRunRenewal As Integer
                Dim nAnniversaryCopy As Integer
                Dim nRenewalStatusCnt As Integer
                Dim dsLatestVersion As DataSet = Nothing
                Try
                    oRenewalBusiness = New bSIRRenewal.Business
                    SAMFunc.InitialiseSBOObject(con, oRenewalBusiness, _SiriusUser, "", "bSIRRenewal.Business")

                    Dim createWMTaskRequest As New SAMForInsuranceImplementationTypes.CreateWmTaskRequestType
                    Dim createWMTaskResponse As BaseCreateWmTaskResponseType

                    'TODO - Use correct user/user group fromt the system options
                    createWMTaskRequest.AllocationUser = _SiriusUser.Username
                    createWMTaskRequest.UserId = _SiriusUser.UserID
                    createWMTaskRequest.AllocationUserGroup = "SFORU"
                    createWMTaskRequest.BranchCode = request.BranchCode
                    createWMTaskRequest.Client = request.DataStore.PartyShortname
                    createWMTaskRequest.Description = "Please also apply your amendment to the renewal version of this policy " & request.DataStore.InsuranceRef
                    createWMTaskRequest.DueDateTime = DateAdd("ww", 1, Now)
                    createWMTaskRequest.IsComplete = False
                    createWMTaskRequest.Task = "MEMO"
                    createWMTaskRequest.TaskGroup = "COMMON"

                    Using Cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_renewal_details")
                        Cmd.AddInParameter("@nInsurance_file_cnt", SqlDbType.Int).Value = request.InsuranceFileKey
                        dsRenewalStatus = con.ExecuteDataSet(Cmd, "dsRenewalStatus")
                    End Using
                    If dsRenewalStatus IsNot Nothing AndAlso dsRenewalStatus.Tables IsNot Nothing AndAlso dsRenewalStatus.Tables.Count > 0 AndAlso dsRenewalStatus.Tables(0).Rows IsNot Nothing AndAlso dsRenewalStatus.Tables(0).Rows.Count > 0 Then
                        If dsRenewalStatus.Tables(0).Rows.Count > 0 Then
                            For iCount As Integer = 0 To dsRenewalStatus.Tables(0).Rows.Count - 1
                                nRenewalStatusTypeId = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(iCount).Item(0).ToString)
                                nIsTrueMonthlyPolicy = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(iCount).Item(1).ToString)
                                nDoNotDeleteRenQuoteOnMTA = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(iCount).Item(2).ToString)
                                nAnniversaryCopy = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(iCount).Item(3).ToString)
                                nRenewalStatusCnt = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(iCount).Item(4).ToString)
                                nDeleteRenQuoteReRunRenewal = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(iCount).Item(5).ToString)
                                If nRenewalStatusTypeId <> -1 And nRenewalStatusTypeId <> 0 Then
                                    If (nDoNotDeleteRenQuoteOnMTA = 1 And nIsTrueMonthlyPolicy = 1 And nAnniversaryCopy = 1) Then
                                        createWMTaskResponse = CreateWmTask(con, createWMTaskRequest)
                                    ElseIf (nDoNotDeleteRenQuoteOnMTA = 1 And nIsTrueMonthlyPolicy = 0) Then
                                        createWMTaskResponse = CreateWmTask(con, createWMTaskRequest)
                                    ElseIf (nDoNotDeleteRenQuoteOnMTA = 1 And nIsTrueMonthlyPolicy = 1 And nAnniversaryCopy = 0) Then
                                        comReturnValue = oRenewalBusiness.DeletePolicyFromRenewal(request.InsuranceFileKey, v_bRetainAnniversaryCopy:=True)
                                        If request.IsBackdatedMTA Then
                                            Using Cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_latest_policy_version_for_Renewal")
                                                Cmd.AddInParameter("@nInsurance_folder_cnt", SqlDbType.Int).Value = request.DataStore.InsuranceFolderKey
                                                dsLatestVersion = con.ExecuteDataSet(Cmd, "dsLatestVersion")
                                            End Using
                                            If dsLatestVersion IsNot Nothing AndAlso dsLatestVersion.Tables IsNot Nothing AndAlso dsLatestVersion.Tables.Count > 0 AndAlso dsLatestVersion.Tables(0).Rows IsNot Nothing AndAlso dsLatestVersion.Tables(0).Rows.Count > 0 Then
                                                RunRenewalSelectionRequest.InsuranceFileKey = Cast.ToInt32(dsLatestVersion.Tables(0).Rows(0).Item("insurance_file_cnt"), 0)
                                                RunRenewalSelectionRequest.bDoNotCreateTMPAnniversaryVersion = True
                                                RunRenewalSelection(con, RunRenewalSelectionRequest)
                                            End If
                                        Else
                                            RunRenewalSelectionRequest.InsuranceFileKey = request.InsuranceFileKey
                                            RunRenewalSelectionRequest.bDoNotCreateTMPAnniversaryVersion = True
                                            RunRenewalSelection(con, RunRenewalSelectionRequest)
                                        End If
                                    ElseIf (nDoNotDeleteRenQuoteOnMTA = 0 AndAlso nDeleteRenQuoteReRunRenewal = 0) Then
                                        comReturnValue = oRenewalBusiness.DeletePolicyFromRenewal(request.InsuranceFileKey)
                                        Exit For
                                    End If
                                    If (nDoNotDeleteRenQuoteOnMTA = 1) Then
                                        comReturnValue = oRenewalBusiness.UpdateRenewalStatus(nRenewalStatusCnt, "", 4)
                                    End If
                                End If

                            Next
                        Else
                            nRenewalStatusTypeId = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(0).Item(0).ToString)
                            nIsTrueMonthlyPolicy = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(0).Item(1).ToString)
                            nDoNotDeleteRenQuoteOnMTA = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(0).Item(2).ToString)
                            nAnniversaryCopy = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(0).Item(3).ToString)
                            nDeleteRenQuoteReRunRenewal = Convert.ToInt32(dsRenewalStatus.Tables(0).Rows(0).Item(5).ToString)
                            If nRenewalStatusTypeId <> -1 And nRenewalStatusTypeId <> 0 Then
                                If nDoNotDeleteRenQuoteOnMTA = 0 AndAlso nDeleteRenQuoteReRunRenewal = 0 Then
                                    comReturnValue = oRenewalBusiness.DeletePolicyFromRenewal(request.InsuranceFileKey)
                                    If comReturnValue <> PMEReturnCode.PMTrue Then
                                        RaiseComMethodException("bSIRRenewal.Business.DeletePolicyFromRenewal", comReturnValue)
                                    End If
                                ElseIf (nDoNotDeleteRenQuoteOnMTA = 1 And nIsTrueMonthlyPolicy = 0) Then
                                    createWMTaskResponse = CreateWmTask(con, createWMTaskRequest)
                                ElseIf (nDoNotDeleteRenQuoteOnMTA = 1 And nIsTrueMonthlyPolicy = 1 And nAnniversaryCopy = 1) Then
                                    createWMTaskResponse = CreateWmTask(con, createWMTaskRequest)
                                ElseIf (nDoNotDeleteRenQuoteOnMTA = 1 And nIsTrueMonthlyPolicy = 1 And nAnniversaryCopy = 0) Then
                                    comReturnValue = oRenewalBusiness.DeletePolicyFromRenewal(request.InsuranceFileKey)
                                    If request.IsBackdatedMTA Then
                                        Using Cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_latest_policy_version_for_Renewal")
                                            Cmd.AddInParameter("@nInsurance_folder_cnt", SqlDbType.Int).Value = request.DataStore.InsuranceFolderKey
                                            dsLatestVersion = con.ExecuteDataSet(Cmd, "dsLatestVersion")
                                        End Using
                                        If dsLatestVersion IsNot Nothing AndAlso dsLatestVersion.Tables IsNot Nothing AndAlso dsLatestVersion.Tables.Count > 0 AndAlso dsLatestVersion.Tables(0).Rows IsNot Nothing AndAlso dsLatestVersion.Tables(0).Rows.Count > 0 Then
                                            RunRenewalSelectionRequest.InsuranceFileKey = Cast.ToInt32(dsLatestVersion.Tables(0).Rows(0).Item("insurance_file_cnt"), 0)
                                            RunRenewalSelectionRequest.bDoNotCreateTMPAnniversaryVersion = True
                                            RunRenewalSelection(con, RunRenewalSelectionRequest)
                                        End If
                                    Else
                                        RunRenewalSelectionRequest.InsuranceFileKey = request.InsuranceFileKey
                                        RunRenewalSelectionRequest.bDoNotCreateTMPAnniversaryVersion = True
                                        RunRenewalSelection(con, RunRenewalSelectionRequest)
                                    End If
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    ExceptionManager.Publish(ex)
                    Debug.WriteLine(ex.Message)
                Finally
                    If oRenewalBusiness IsNot Nothing Then
                        oRenewalBusiness.Dispose()
                        oRenewalBusiness = Nothing
                    End If
                End Try

            End If
        End If
        If request.TransactionType = "MTC" Then
            Dim oRenewalBusiness As bSIRRenewal.Business = Nothing
            Dim dsRenewalStatus As DataSet = Nothing
            Try
                oRenewalBusiness = New bSIRRenewal.Business
                SAMFunc.InitialiseSBOObject(con, oRenewalBusiness, _SiriusUser, "", "bSIRRenewal.Business")
                Using Cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_renewal_details")
                    Cmd.AddInParameter("@nInsurance_file_cnt", SqlDbType.Int).Value = request.InsuranceFileKey
                    dsRenewalStatus = con.ExecuteDataSet(Cmd, "dsRenewalStatus")
                End Using
                If dsRenewalStatus IsNot Nothing AndAlso dsRenewalStatus.Tables IsNot Nothing AndAlso dsRenewalStatus.Tables.Count > 0 AndAlso dsRenewalStatus.Tables(0).Rows IsNot Nothing AndAlso dsRenewalStatus.Tables(0).Rows.Count > 0 Then
                    comReturnValue = oRenewalBusiness.DeletePolicyFromRenewal(request.InsuranceFileKey)
                    If comReturnValue <> PMEReturnCode.PMTrue Then
                        RaiseComMethodException("bSIRRenewal.Business.DeletePolicyFromRenewal", comReturnValue)
                    End If

                End If
            Catch ex As Exception
                ExceptionManager.Publish(ex)
                Debug.WriteLine(ex.Message)
            Finally
                If oRenewalBusiness IsNot Nothing Then
                    oRenewalBusiness.Dispose()
                    oRenewalBusiness = Nothing
                End If

            End Try
        End If
    End Sub


    Private Sub ProcessAccounts(
    ByRef con As SiriusConnection,
    ByRef request As BaseBindQuoteRequestType, Optional ByRef vDebitTransDetail As Object() = Nothing,
    Optional ByRef bSettleTransaction As Boolean = False, Optional ByRef iAccountId As Integer = 0)

        Dim controlTransBusiness As bControlTrans.Automated = CreateAndInitialiseControlTransBusiness(con, request.BranchCode)
        Dim RenSelection As bSIRRenSelection.Business = CreateAndInitialiseRenewalSelection(con, request.BranchCode)
        Dim processMode As Integer
        Dim comReturnValue As Integer
        Dim vNBTransArray(,) As Object = Nothing
        Dim listRisksBusiness As bSIRListRisks.Business = CreateAndInitialiseListRisksBusiness(con, request.BranchCode)

        Select Case request.TransactionType
            Case TransactionTypeCode.MTA, TransactionTypeCode.CancelPolicy
                processMode = PMEProcessMode.PMProcessModeMTAQuote
            Case TransactionTypeCode.NewBusiness
                processMode = PMEProcessMode.PMProcessModeNBQuote
            Case TransactionTypeCode.Renewals
                processMode = PMEProcessMode.PMProcessModeRNQuote
            Case TransactionTypeCode.ReinstatePolicy
                processMode = PMEProcessMode.PMProcessModeMTRQuote
        End Select

        comReturnValue = controlTransBusiness.SetProcessModes(
            vTask:=PMEComponentAction.PMAdd,
            vNavigate:=0,
            vProcessMode:=CObj(processMode),
            vTransactionType:=CObj(request.TransactionType),
            vEffectiveDate:=Date.Today)

        controlTransBusiness.InsuranceFileCnt = request.InsuranceFileKey

        controlTransBusiness.ByPassLocking = True

        Dim cashlistID As Integer = 0
        Dim cashlistItemID As Integer = 0
        Dim transactionId As Integer = 0
        Dim accountID As Integer = 0
        Dim iReturn As Integer = 0
        Dim dRoundOffAmount As Decimal
        Dim sTempValue As String
        Dim iProductId As Integer
        Dim iRoundOffOption As Integer

        Dim iDebitAgainst As Integer = 0
        Dim vCreditTransactions As Object(,) = Nothing

        Dim dPremiumAmount As Double

        Dim oGetProductRiskOptionValueRequest As New BaseProductRiskOptionValueRequestType
        sTempValue = GetAndValidateDescriptionById(con, PMLookupTable.Insurance_file, "Product_Id", "Insurance_file_cnt", request.InsuranceFileKey.ToString)
        If Trim(sTempValue) <> "" Then
            iProductId = CInt(sTempValue)
        End If
        If iProductId > 0 Then
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.RoundOffToZero
            iRoundOffOption = Convert.ToInt32(GetProductRiskOptions(con, iProductId, oGetProductRiskOptionValueRequest))
        End If

        If request.PayNowDetails IsNot Nothing Then
            accountID = request.PayNowDetails.AccountId
            cashlistID = request.PayNowDetails.CashlistId
            cashlistItemID = request.PayNowDetails.CashListItemId
            transactionId = request.PayNowDetails.TransactionId
        ElseIf request.PayNowPaymentDetails IsNot Nothing Then
            accountID = request.PayNowPaymentDetails.PaymentAccountID
            cashlistID = request.PayNowPaymentDetails.CashListKey
            cashlistItemID = request.PayNowPaymentDetails.CashListItemKey
            transactionId = request.PayNowPaymentDetails.TransDetailKey
        End If

        iDebitAgainst = request.DebitAgainst

        If request.DataStore.AgentType = "Broker" Then
            dPremiumAmount = request.DataStore.TotalPremiumAmount - request.DataStore.TotalAgentCommission - request.DataStore.TotalAgentCommissionTax
        Else
            dPremiumAmount = request.DataStore.TotalPremiumAmount
        End If

        If iRoundOffOption = 1 Then
            dRoundOffAmount = Cast.ToDecimal((System.Math.Round(dPremiumAmount, System.MidpointRounding.AwayFromZero) - dPremiumAmount), 0)
        End If

        If request.DataStore.PaymentMethod = PolicyPaymentMethod.CashDeposit Then
            iDebitAgainst = DebitAgainstType.DebitAgainstCashDeposit
            accountID = request.SelectedCashDeposit.CashDepositAccountID
            vCreditTransactions = request.SelectedCashDeposit.AllocationDetails
        Else

            If request.CreditTransactions IsNot Nothing Then
                ReDim Preserve vCreditTransactions(2, request.CreditTransactions.Row.Length - 1)

                accountID = request.CreditTransactions.Row(0).AccountKey

                For CountFor As Integer = 0 To request.CreditTransactions.Row.Length - 1
                    vCreditTransactions(0, CountFor) = request.CreditTransactions.Row(CountFor).AccountKey
                    vCreditTransactions(1, CountFor) = request.CreditTransactions.Row(CountFor).TransDetailKey
                    If dPremiumAmount >= request.CreditTransactions.Row(CountFor).Amount Then
                        vCreditTransactions(2, CountFor) = request.CreditTransactions.Row(CountFor).Amount
                        dPremiumAmount = dPremiumAmount - request.CreditTransactions.Row(CountFor).Amount
                    Else
                        vCreditTransactions(2, CountFor) = dPremiumAmount
                        Exit For
                    End If
                Next
            End If

        End If

        Dim dTransactionAmount As Decimal
        If request.PayNowDetails IsNot Nothing Then
            dTransactionAmount = Cast.ToDecimal(request.PayNowDetails.Amount, 0)
        Else
            dTransactionAmount = request.DataStore.TotalPremiumAmount
        End If

        'PN# 63300
        If iDebitAgainst = 1 Or iDebitAgainst = 2 Then
            If accountID = 0 Then
                GetAccountDetailsForPolicy(con, request.InsuranceFileKey, accountID, "", "", bDebitAgainstAccountSpecified:=True, oDebitAgaintAccountType:=request.DebitAgainstAccount, iAccountId:=iAccountId)
            End If
        ElseIf iDebitAgainst = 3 Then
            If accountID = 0 Then
                If request.DebitAgainstAccountSpecified = True Then
                    GetAccountDetailsForPolicy(con, request.InsuranceFileKey, accountID, "", "", True, request.DebitAgainstAccount)
                Else
                    GetAccountDetailsForPolicy(con, request.InsuranceFileKey, accountID, "", "")
                End If
            End If
        End If
        If request.DataStore.AgentType.ToLower = "intermed" And (processMode <> PMEProcessMode.PMProcessModeNBQuote _
                OrElse processMode = PMEProcessMode.PMProcessModeMTAQuote _
                OrElse processMode = PMEProcessMode.PMProcessModeMTRQuote _
                OrElse processMode = PMEProcessMode.PMProcessModeRNQuote) _
            And (request.DataStore.PaymentMethod = PolicyPaymentMethod.Invoice Or request.DataStore.PaymentMethod = PolicyPaymentMethod.Instalments) Then
            iReturn = listRisksBusiness.GetTransNBAccountId(request.DataStore.InsuranceFolderKey, vNBTransArray)

            If vNBTransArray Is Nothing Then
                GetAccountDetailsForPolicy(con, request.InsuranceFileKey, accountID, "", "") ' Original policy made by Via DTU does not have spare
            Else
                If CType(vNBTransArray, Object(,)).GetValue(0, 0).ToString() <> "" Then
                    accountID = CInt(CType(vNBTransArray, Object(,)).GetValue(0, 0))
                    iAccountId = accountID
                End If
            End If
        End If
        Dim IntermediaryAgentAccountID As Integer
        IntermediaryAgentAccountID = GetIntermediaryAgentAccountID(con, request.DataStore.InsuranceFolderKey, request.InsuranceFileKey)

        If IntermediaryAgentAccountID <> 0 Then
            If accountID = 0 AndAlso (processMode = PMEProcessMode.PMProcessModeNBQuote OrElse processMode = PMEProcessMode.PMProcessModeRNQuote) AndAlso (request.DataStore.PaymentMethod = PolicyPaymentMethod.Invoice OrElse request.DataStore.PaymentMethod = PolicyPaymentMethod.Instalments) AndAlso
                request.DataStore IsNot Nothing Then
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Get_AccountID_From_ShortCode")

                    cmd.AddInParameter("@company_id", SqlDbType.Int).Value = request.SourceId
                    cmd.AddInParameter("@ShortCode", SqlDbType.VarChar, 50).Value = request.DataStore.AgentShortname
                    cmd.AddOutParameter("@AccountID", SqlDbType.Int)
                    cmd.AddOutParameter("@delete_at_purge", SqlDbType.TinyInt)

                    con.ExecuteNonQuery(cmd)
                    accountID = Cast.ToInt32(cmd.Parameters.Item("@AccountID").Value, 0)
                End Using
            End If
        End If
        'Assigned values for iDebitAgainst,vCreditTransactions as per the Tech Spec(Tech Spec - UIICWR23 - New Business - Pre Payment Functionality)- (7.2.2.4)

        'Note:-New Parameter(roundOffAmount) has been added in the function  controlTransBusiness.Start
        'If dTransactionAmount <> 0 Then
        If iRoundOffOption = 1 Then
            comReturnValue = controlTransBusiness.Start(
                iPaymentAccountId:=accountID,
                iDebitAgainst:=Convert.ToInt16(iDebitAgainst),
                vCreditTransactions:=CType(vCreditTransactions, Object),
                lCashListID:=cashlistID,
                lCashListItemId:=cashlistItemID,
                lTransactionID:=transactionId,
                cTransactionAmount:=dTransactionAmount,
                sOldPolicyNumber:=request.DataStore.InsuranceRef.Trim,
                sPaymentMethod:=request.DataStore.PaymentMethod,
                vDebitTransactions:=DirectCast(vDebitTransDetail, Object), bProcessSettleTransactions:=bSettleTransaction,
                cRoundOffAmount:=dRoundOffAmount)
        Else
            comReturnValue = controlTransBusiness.Start(
                iPaymentAccountId:=accountID,
                iDebitAgainst:=Convert.ToInt16(iDebitAgainst),
                vCreditTransactions:=CType(vCreditTransactions, Object),
                lCashListID:=cashlistID,
                lCashListItemId:=cashlistItemID,
                lTransactionID:=transactionId,
                cTransactionAmount:=dTransactionAmount,
                sOldPolicyNumber:=request.DataStore.InsuranceRef.Trim,
                sPaymentMethod:=request.DataStore.PaymentMethod,
                vDebitTransactions:=DirectCast(vDebitTransDetail, Object), bProcessSettleTransactions:=bSettleTransaction)
        End If

        If (comReturnValue <> PMEReturnCode.PMTrue) Then
            Dim failureReason As String = String.Empty
            Try
                failureReason = controlTransBusiness.Message
            Finally
                failureReason = "bControlTrans.Automated.Start : - Statistics process failed : " + failureReason
                RaiseComMethodException(failureReason)
            End Try
        End If
        'End If
        ' if this is to be paid by instalments get the transactions for this policy
        ' to save creating the business object again
        If request.DataStore.PaymentMethod = PolicyPaymentMethod.Instalments Then
            'For Object Array - When Strict On
            Dim PFTransactions(,) As Object = CType(request.DataStore.PFTransactions, Object(,))
            comReturnValue = controlTransBusiness.GetPFTransactions(
            request.InsuranceFileKey,
            PFTransactions)
            request.DataStore.PFTransactions = CType(PFTransactions, Object)

            If (comReturnValue <> PMEReturnCode.PMTrue) Then
                RaiseComMethodException("bControlTrans.Automated.GetPFTransactions", comReturnValue)
            End If
        ElseIf request.DataStore.PaymentMethod = PolicyPaymentMethod.PayNow Then
            Dim oSAMErrorCollection As New SAMErrorCollection
            Dim documentCode As String, iPartyKey As Integer, nInsuranceFolderKey As Integer
            Dim iDocumentId As Integer, sDocumentRef As String

            iDocumentId = GetAndValidateSpecifiedTableCode(con,
                                  "transdetail", "document_id", "transdetail_id", transactionId.ToString(), oSAMErrorCollection, "")

            nInsuranceFolderKey = GetAndValidateSpecifiedTableCode(con,
                                      "insurance_file", "insurance_folder_cnt", "insurance_file_cnt", request.InsuranceFileKey.ToString(), oSAMErrorCollection, "")
            iPartyKey = GetAndValidateSpecifiedTableCode(con,
                                             PMLookupTable.Party, "Party_cnt", "shortname", request.PayNowDetails.AccountShortCode, oSAMErrorCollection, request.PayNowDetails.AccountShortCode.ToString())
            If iDocumentId <> 0 Then
                sDocumentRef = GetAndValidateDescriptionById(con, "document", "document_ref", "document_id", iDocumentId.ToString())
                GenerateCashChequeReceiptAndPaymentDocument(con:=con, v_sBranchCode:=request.BranchCode,
                                                            v_nSourceId:=request.SourceId, v_nPartyKey:=iPartyKey, v_sType:="R", v_sDocumentRef:=sDocumentRef, v_sDocumentCode:=documentCode, v_nInsuranceFileKey:=request.InsuranceFileKey, v_nInsuranceFolderKey:=nInsuranceFolderKey)
            End If
        End If

        comReturnValue = RenSelection.DeleteWorkTask(v_sKeyName:="InsuranceFileKey", v_sKeyValue:=CType(request.InsuranceFileKey, String))
        If (comReturnValue <> PMEReturnCode.PMTrue) Then
            RaiseComMethodException("bSIRRenSelection.Business.DeleteWorkTask", comReturnValue)
        End If

    End Sub
    ''' <summary>
    ''' Update Policy Details
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="branchCode"></param>
    ''' <param name="insuranceFileKey"></param>
    ''' <param name="transactionType"></param>
    ''' <param name="bProcessAsWritten"></param>
    ''' <param name="sNewPolicyNumber"></param>
    ''' <param name="SkipNewPolicyNumber"></param>
    ''' <remarks></remarks>
    Private Sub UpdatePolicyDetails(ByRef con As SiriusConnection,
    ByVal branchCode As String,
    ByVal insuranceFileKey As Integer,
    ByVal transactionType As String, Optional ByVal bProcessAsWritten As Boolean = False,
    Optional ByRef sNewPolicyNumber As String = "",
    Optional ByVal SkipNewPolicyNumber As Boolean = False,
    Optional ByVal sInsuranceFileStatus As String = "")

        Dim changePolicyStatusBusiness As bSIRChangePolicyStatus.Business = CreateAndInitialiseChangePolicyStatusBusiness(con, branchCode)

        Try

            UpdatePolicyPremium(changePolicyStatusBusiness, insuranceFileKey)

            ' rejig the risk number to take into account any risks that may have been deleted
            RenumberRisks(changePolicyStatusBusiness, insuranceFileKey)

            ' set detail on the policy status business object
            changePolicyStatusBusiness.Mode = 0
            changePolicyStatusBusiness.TransactionType = transactionType

            If SkipNewPolicyNumber = False Then
                SkipNewPolicyNumber = CheckToSkipNewPolicyNumber(con, insuranceFileKey)
            End If

            'Check if Policy Number is Overridden, if yes then pass the Overridden Policy Number otherwise let the system create new one
            If bProcessAsWritten AndAlso Trim(sNewPolicyNumber) <> "" Then
                ChangePolicyStatus(changePolicyStatusBusiness:=changePolicyStatusBusiness, insuranceFileKey:=insuranceFileKey, bProcessAsWritten:=bProcessAsWritten, sOverriddenPolicyNumber:=sNewPolicyNumber, SkipNewPolicyNumber:=SkipNewPolicyNumber, sInsuranceFileStatus:=sInsuranceFileStatus)
            Else
                ChangePolicyStatus(changePolicyStatusBusiness:=changePolicyStatusBusiness, insuranceFileKey:=insuranceFileKey, bProcessAsWritten:=bProcessAsWritten, SkipNewPolicyNumber:=SkipNewPolicyNumber, sInsuranceFileStatus:=sInsuranceFileStatus)
            End If

            'Get OverriddenPolicyNumber or newly generated polcy number
            sNewPolicyNumber = changePolicyStatusBusiness.NewPolicyNumber

        Finally

            If changePolicyStatusBusiness IsNot Nothing Then
                changePolicyStatusBusiness.Dispose()
                changePolicyStatusBusiness = Nothing
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Added as part of EH031529 - Clone Quote from Live policy
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="iInunsuranceFileKey"></param>
    ''' <returns></returns>
    Private Function CheckToSkipNewPolicyNumber(ByVal con As SiriusConnection, ByVal iInunsuranceFileKey As Integer) As Boolean
        'Const ACMethodName As String = "CheckClaimKey"

        Dim count As Int32

        ' Call the stored proc

        Using cmd1 As SiriusCommand = SiriusCommand.FromProcedure("spu_Skip_New_Policy_Number")
            cmd1.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = iInunsuranceFileKey
            cmd1.AddOutParameter("@bSkipNewPolicyNumber", SqlDbType.Int)

            con.ExecuteNonQuery(cmd1)
            count = Cast.ToInt32(cmd1.Parameters("@bSkipNewPolicyNumber").Value, 0)
        End Using

        If count = 1 Then
            Return True
        End If

        Return False
    End Function

    Private Sub RenumberRisks(
    ByRef changePolicyStatusBusiness As bSIRChangePolicyStatus.Business,
    ByVal insuranceFileKey As Integer)

        Dim comReturnValue As Integer

        comReturnValue = changePolicyStatusBusiness.RenumberRisks(insuranceFileKey)
        If comReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRChangePolicyStatus.Business.RenumberRisks", comReturnValue)
        End If

    End Sub

    Private Sub UpdatePolicyPremium(
    ByRef changePolicyStatusBusiness As bSIRChangePolicyStatus.Business,
    ByVal insuranceFileKey As Integer)

        Dim comReturnValue As Integer

        comReturnValue = changePolicyStatusBusiness.UpdatePolicyPremium(insuranceFileKey)
        If comReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRChangePolicyStatus.Business.UpdatePolicyPremium", comReturnValue)
        End If

    End Sub
    ''' <summary>
    ''' ChangePolicyStatus
    ''' </summary>
    ''' <param name="changePolicyStatusBusiness"></param>
    ''' <param name="insuranceFileKey"></param>
    ''' <param name="bProcessAsWritten"></param>
    ''' <param name="sOverriddenPolicyNumber"></param>
    ''' <param name="SkipNewPolicyNumber"></param>
    ''' <param name="sInsuranceFileStatus"></param>
    ''' <remarks></remarks>
    Private Sub ChangePolicyStatus(
       ByRef changePolicyStatusBusiness As bSIRChangePolicyStatus.Business,
       ByVal insuranceFileKey As Integer, Optional ByVal bProcessAsWritten As Boolean = False,
      Optional ByVal sOverriddenPolicyNumber As String = "",
      Optional ByVal SkipNewPolicyNumber As Boolean = False,
      Optional ByVal sInsuranceFileStatus As String = "")

        Dim nReturnValue As Integer

        nReturnValue = changePolicyStatusBusiness.ChangePolicyStatus(v_lInsuranceFileCnt:=insuranceFileKey,
                                                                    v_bSetAsWritten:=bProcessAsWritten,
                                                                    v_sOverriddenPolicyNumber:=sOverriddenPolicyNumber,
                                                                    v_bSkipNewPolicyNumber:=SkipNewPolicyNumber, v_sSelectedPolicyStatus:=sInsuranceFileStatus)

        If nReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRChangePolicyStatus.Business.ChangePolicyStatus", nReturnValue)
        End If

    End Sub

    Private Sub ProcessPaymentTerms(
    ByRef con As SiriusConnection,
    ByRef coreBusiness As CoreBusiness,
    ByRef request As BaseBindQuoteRequestType, Optional ByRef vDebitTransDetail As Object() = Nothing,
    Optional ByRef bSettleTransaction As Boolean = False)

        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim iPartyKey As Integer = 0
        Dim sValue As String = ""
        If request.DataStore IsNot Nothing Then
            iPartyKey = request.DataStore.PartyKey
        End If

        If request.SelectedInstalmentQuoteSpecified Then


            request.DataStore.PaymentMethod = PolicyPaymentMethod.Instalments

            'For bank details, corresponding party bank key is provided in the request itself
            'For credit card details, corresponding party bank key is provided in the credit card structure
            'If party bank key is provided for bank item, check its validity
            If request.PartyBankKey > 0 Then
                'Validate bank item
                'Gaurav confirmed that request.BankAccountNo can be used for validating the bank account number of party bank key
                ValidatePartyBank(con, oSAMErrorCollection, request.PartyBankKey, iPartyKey, Nothing, True, request.BankAccountNo)

                oSAMErrorCollection.CheckForErrors()
            End If

            'If party bank key is provided for credit card item, check its validity
            If request.CreditCard IsNot Nothing AndAlso request.CreditCard.PartyBankKey > 0 Then

                'Validate the party bank
                ValidatePartyBank(con, oSAMErrorCollection, request.CreditCard.PartyBankKey, iPartyKey, Nothing, False, request.CreditCard.Number)

                oSAMErrorCollection.CheckForErrors()
            End If

            If request.TransactionType = TransactionTypeCode.MTA OrElse
                request.TransactionType = TransactionTypeCode.CancelPolicy Then

                ' Validate that there is a live instalment plan
                ' bSirListRisks.GetInsuranceFileDetails only returns the pfpremium finance details
                ' if the instalment plan is live so if there is a cnt the plan is live
                If request.DataStore.PFPremiumFinanceCnt <> 0 Then

                    If request.AmountToFinance < 0 AndAlso Not request.PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified Then

                        If Not request.PayNegativePremiumMTABalance Then
                            ' pay by invoice 
                            request.DataStore.PaymentMethod = PolicyPaymentMethod.Invoice

                            ' set indicator to raise work manager task
                            request.DataStore.CreateWorkManagerTaskForMTAReturnPremium = True
                            request.DataStore.RefundAmount = Cast.ToDecimal(request.AmountToFinance, 0)
                        Else
                            CheckPlanOSBalanceToSpread(con, coreBusiness, request, vDebitTransDetail, bSettleTransaction)
                        End If

                    End If

                End If

            End If

        ElseIf request.PayNowDetails IsNot Nothing AndAlso Not request.NoTrans Then

            request.DataStore.PaymentMethod = PolicyPaymentMethod.PayNow

            'If party bank key is provided for bank item, check its validity
            If request.PayNowDetails.PartyBankKey > 0 Then
                'If credit card details is provided, validate party bank item key and the card number provided
                'If credit card details are not provided, validate only the party bank item
                If Not String.IsNullOrEmpty(request.PayNowDetails.CCNumber) Then
                    'Validate credit card 
                    ValidatePartyBank(con, oSAMErrorCollection, request.PayNowDetails.PartyBankKey, iPartyKey, Nothing, False, request.PayNowDetails.CCNumber)
                Else
                    'We are not sure request.PayNowDetails.BankAccountID actually represents bank account number. Removed the validation on bank account number as told by Gaurav.
                    ValidatePartyBank(con, oSAMErrorCollection, request.PayNowDetails.PartyBankKey, iPartyKey)
                End If

                oSAMErrorCollection.CheckForErrors()
            End If

            If request.TransactionType = TransactionTypeCode.MTA OrElse
                    request.TransactionType = TransactionTypeCode.CancelPolicy Then

                CheckPlanOSBalanceToSpread(con, coreBusiness, request, vDebitTransDetail, bSettleTransaction)
                ProcessPayNow(con, request)

                If request.DataStore.LiveFinancePlans IsNot Nothing AndAlso request.TransactionType = TransactionTypeCode.CancelPolicy Then
                    Call coreBusiness.GetSystemOption(request.BranchCode, SystemOption.CancelInstalmentPlanonPolicyCancellation, sValue)
                    If sValue = "1" Then
                        CancelInHousePlan(con, coreBusiness, request, vDebitTransDetail)
                    End If

                End If

            Else
                If request.DataStore.PaymentMethod = PolicyPaymentMethod.PayNow Then

                    ' if this is renewals only process paynow if this is with accept renewal
                    If ((request.TransactionType = TransactionTypeCode.Renewals AndAlso request.AcceptRenewal) _
                        Or (request.TransactionType <> TransactionTypeCode.Renewals)) Then
                        ProcessPayNow(con, request)
                    End If

                End If
            End If
        ElseIf (request.PayNowPaymentDetails IsNot Nothing) OrElse (request.PayNowPaymentDetails IsNot Nothing AndAlso request.NoTrans) Then

            request.DataStore.PaymentMethod = PolicyPaymentMethod.PayNow
        ElseIf request.SelectedCashDeposit IsNot Nothing Then
            request.DataStore.PaymentMethod = PolicyPaymentMethod.CashDeposit

            'Get allocation details
            GetAllocationDetailsForCashDeposit(con, request, request.SelectedCashDeposit.AllocationDetails)

            'If allocation details not present and the process is not refund, make the payment method as invoice
            If request.SelectedCashDeposit.AllocationDetails Is Nothing AndAlso request.DataStore.TotalPremiumAmount > 0 Then
                request.DataStore.PaymentMethod = PolicyPaymentMethod.Invoice
            End If

        Else
            ' process it as invoice
            request.DataStore.PaymentMethod = PolicyPaymentMethod.Invoice
            If request.DataStore.LiveFinancePlans IsNot Nothing AndAlso request.TransactionType = TransactionTypeCode.CancelPolicy Then
                Call coreBusiness.GetSystemOption(request.BranchCode, SystemOption.CancelInstalmentPlanonPolicyCancellation, sValue)
                If sValue = "1" Then
                    CancelInHousePlan(con, coreBusiness, request, vDebitTransDetail)
                End If

            End If
        End If

    End Sub


    Private Sub ProcessPayNow(
    ByRef con As SiriusConnection,
    ByRef request As BaseBindQuoteRequestType)

        ' get account code for party
        GetAccountDetailsForPolicy(con,
            request.InsuranceFileKey,
            request.PayNowDetails.AccountId,
             request.PayNowDetails.AccountCode,
             request.PayNowDetails.AccountShortCode)

        'Previously, methods for handling reciept had generic names such as create cashlist. Now new methods created to
        'handle reciepts and create cashlist method that handled reciept now changed to handle payments. This method 
        'was calling CreateCashList,CreateAgentRecieptCashListItem and PostAgentReceiptCashListItem methods previously, 
        'which are now renamed to CreateAgentReceiptCashList,CreateAgentRecieptCashListItem and PostAgentReceiptCashListItem
        'respectively. Even thogh Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc doesn't mention about renaming
        'these function calls in ProcessPayNow method, it is done to make the code work.

        If Not request.PayNegativePremiumMTABalance Then
            'Create the cash list
            CreateAgentReceiptCashList(con, request.PayNowDetails, request.SourceId)

            'Create the cash list item
            CreateAgentReceiptCashListItem(con, request.PayNowDetails)
        Else
            CreateRefundPaymentCashList(con, request.PayNowDetails)

            'Create the cash list item
            CreateRefundPaymentCashListItem(con, request.PayNowDetails)
        End If
        'Post the cashlistitem
        PostAgentReceiptCashListItem(con, request.PayNowDetails)

        '' create the cash list
        'CreateCashList(con, request.PayNowDetails)

        '' create the cash list item
        'CreateCashListItem(con, request.PayNowDetails)

        '' post the cashlistitem
        'PostCashListItem(con, request.PayNowDetails)

    End Sub

    Private Sub ValidatePolicyPremium(ByRef con As SiriusConnection,
    ByVal insuranceRef As String,
                ByVal totalpremiumAmount As Decimal,
                ByVal insurance_file_cnt As Integer,
                ByVal crTotalPremiumOfAllVersions As Decimal,
                ByVal crTotalTaxNotAppliedToClient As Decimal)

        If totalpremiumAmount < 0 Then

            Dim totalPremiumAmountForAllPolicyVersions As Decimal = crTotalPremiumOfAllVersions
            'GetTotalPremiumAmountForALLPolicyVersions(con, insuranceRef, insurance_file_cnt)
            Dim totalTaxNotAppliedToClient As Decimal = crTotalTaxNotAppliedToClient

            If (Math.Round(totalPremiumAmountForAllPolicyVersions, 2) + Math.Round(totalTaxNotAppliedToClient, 2) < (Math.Round(totalpremiumAmount, 2) * (-1))) Then
                Dim samErrorCollection As SAMErrorCollection = New SAMErrorCollection
                samErrorCollection.AddBusinessRule(
                    SAMBusinessErrors.ReturnPremiumIsGreaterThanBilledPremium,
                    SAMBusinessErrors.ReturnPremiumIsGreaterThanBilledPremium.ToString())
                samErrorCollection.CheckForErrors()
            End If

        End If

    End Sub

    Private Sub ValidatePolicy(ByRef con As SiriusConnection,
    ByVal transactionType As String,
    ByVal insuranceFileKey As Integer,
    ByVal isTrueMonthlyPolicy As Boolean,
    ByVal netPremiumAmount As Decimal,
    ByVal agentCommissionAmount As Decimal,
    ByVal policyHasAgent As Boolean,
    ByVal TotalPremiumAmount As Decimal)

        ValidateRiskDetails(con, insuranceFileKey)

        If policyHasAgent Then
            ValidateAgentCommission(transactionType, isTrueMonthlyPolicy, netPremiumAmount, agentCommissionAmount)
        End If

        ValidateAccountsOnPolicy(con, insuranceFileKey)

        If transactionType = TransactionTypeCode.NewBusiness OrElse transactionType = TransactionTypeCode.Renewals Then
            ValidateAmountOnNBAndRenewal(TotalPremiumAmount)
        End If

    End Sub

    Private Sub ValidateAmountOnNBAndRenewal(ByVal TotalPremiumAmount As Decimal)

        Dim samErrorCollection As SAMErrorCollection
        If TotalPremiumAmount < 0 Then
            samErrorCollection = New SAMErrorCollection

            samErrorCollection.AddBusinessRule(
                        SAMBusinessErrors.AmountIsCreditType,
                        SAMBusinessErrors.AmountIsCreditType.ToString())

            samErrorCollection.CheckForErrors()
        End If

    End Sub

    Private Sub ValidateAgentCommission(
    ByVal transactionType As String,
    ByVal isTrueMonthlyPolicy As Boolean,
    ByVal netPremiumAmount As Decimal,
    ByVal agentCommissionAmount As Decimal)

        Dim samErrorCollection As SAMErrorCollection

        If Not isTrueMonthlyPolicy Then

            If transactionType <> TransactionTypeCode.CancelPolicy AndAlso netPremiumAmount >= 0 Then

                If Math.Abs(agentCommissionAmount) > Math.Abs(netPremiumAmount) Then

                    samErrorCollection = New SAMErrorCollection

                    samErrorCollection.AddBusinessRule(
                        SAMBusinessErrors.TotalCommissionMoreThanPremium,
                        SAMBusinessErrors.TotalCommissionMoreThanPremium.ToString())

                    samErrorCollection.CheckForErrors()

                End If

            ElseIf netPremiumAmount < 0 AndAlso agentCommissionAmount < 0 Then

                If (agentCommissionAmount < netPremiumAmount) Then

                    Dim dTolerance As Double = 0.05

                    If Math.Round(agentCommissionAmount, 2) + dTolerance < (Math.Round(netPremiumAmount, 2)) Then

                        samErrorCollection = New SAMErrorCollection

                        samErrorCollection.AddBusinessRule(
                            SAMBusinessErrors.TotalCommissionReturnIsMoreThanPremiumReturn,
                            SAMBusinessErrors.TotalCommissionReturnIsMoreThanPremiumReturn.ToString())

                        samErrorCollection.CheckForErrors()

                    End If                
                
                End If

            End If

        End If

    End Sub

    Private Sub ValidateAccountsOnPolicy(
    ByRef con As SiriusConnection,
    ByVal insuranceFileKey As Integer)

        Const InsuredId As Integer = 0
        Const AgentId As Integer = 1
        Const InsuredAccountStatusCode As Integer = 2
        Const AgentAccountStatusCode As Integer = 3

        Dim partyAccountDetails As DataTable = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Policy_Account_Details")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            partyAccountDetails = con.ExecuteDataTable(cmd)
        End Using

        If partyAccountDetails IsNot Nothing AndAlso
            partyAccountDetails.Rows.Count > 0 Then

            Dim insuredKey As Integer
            Dim agentKey As Integer
            Dim insuredAccountStatus As String
            Dim agentAccountStatus As String

            For Each detail As DataRow In partyAccountDetails.Rows

                insuredKey = Cast.ToInt32(detail.Item(InsuredId), 0)
                agentKey = Cast.ToInt32(detail.Item(AgentId), 0)
                insuredAccountStatus = Cast.ToString(detail.Item(InsuredAccountStatusCode), String.Empty)
                agentAccountStatus = Cast.ToString(detail.Item(AgentAccountStatusCode), String.Empty)

                If insuredKey <> 0 AndAlso insuredAccountStatus.Trim <> AccountStatusCode.Active Then
                    samErrorCollection = New SAMErrorCollection
                    samErrorCollection.AddBusinessRule(
                        SAMBusinessErrors.PolicyCannotBeMadeLiveAsTheInsuredsAccountHasBeenStopped,
                        SAMBusinessErrors.PolicyCannotBeMadeLiveAsTheInsuredsAccountHasBeenStopped.ToString())
                    samErrorCollection.CheckForErrors()
                End If

                If agentKey <> 0 AndAlso insuredAccountStatus.Trim <> AccountStatusCode.Active Then
                    samErrorCollection = New SAMErrorCollection
                    samErrorCollection.AddBusinessRule(
                        SAMBusinessErrors.PolicyCannotBeMadeLiveAsTheLeadAgentsAccountHasBeenStopped,
                        SAMBusinessErrors.PolicyCannotBeMadeLiveAsTheLeadAgentsAccountHasBeenStopped.ToString())
                    samErrorCollection.CheckForErrors()
                End If

            Next

            ' PLICO 9-10
            ' Validate Agent Suspense Account
            If agentKey <> 0 Then
                '    m_lReturn = ValidateAgentSuspenseAccount()
                '    If m_lReturn <> PMTrue Then
                '        r_lValidationStatus = kValidationFailed
                '        Exit Sub
                '    End If

            End If

        End If

    End Sub

    Private Sub ValidateRiskDetails(
    ByRef con As SiriusConnection,
    ByVal insuranceFileKey As Integer)

        Const RiskIsSelectedIndicator As Integer = 1
        Const RiskStatusDescription As Integer = 4

        Dim samErrorCollection As SAMErrorCollection

        ' get insurance file risks
        Dim riskDataTable As DataTable = GetInsuranceFileRisks(con, insuranceFileKey)

        Dim statusCode As String = String.Empty
        Dim isSelected As Boolean
        Dim quotedRiskIsSelected As Boolean

        ' if no risks found
        If riskDataTable Is Nothing OrElse
            riskDataTable.Rows.Count = 0 Then

            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.ThereAreNoRisksOnThePolicy,
                SAMBusinessErrors.ThereAreNoRisksOnThePolicy.ToString())
            samErrorCollection.CheckForErrors()
        Else

            For Each risk As DataRow In riskDataTable.Rows

                isSelected = (Cast.ToByte(risk.Item(RiskIsSelectedIndicator), 0) <> 0)

                statusCode = Cast.ToString(risk.Item(RiskStatusDescription), String.Empty)
                If Not String.IsNullOrEmpty(statusCode) Then
                    statusCode = statusCode.Trim()
                End If

                If isSelected Then

                    Select Case statusCode

                        Case RiskStatusCode.Quoted
                            quotedRiskIsSelected = True
                        Case RiskStatusCode.Deferred
                            quotedRiskIsSelected = True
                        Case RiskStatusCode.Referred,
                                RiskStatusCode.Declined,
                                RiskStatusCode.Unquoted,
                                String.Empty

                            samErrorCollection = New SAMErrorCollection
                            samErrorCollection.AddBusinessRule(
                                SAMBusinessErrors.NotAllSelectedRisksHaveBeenSuccessfullyQuoted,
                                SAMBusinessErrors.NotAllSelectedRisksHaveBeenSuccessfullyQuoted.ToString())
                            samErrorCollection.CheckForErrors()

                        Case RiskStatusCode.PendingReinsurance

                            samErrorCollection = New SAMErrorCollection
                            samErrorCollection.AddBusinessRule(
                                SAMBusinessErrors.ReinsuranceHasNotBeenFullyAssigned,
                                SAMBusinessErrors.ReinsuranceHasNotBeenFullyAssigned.ToString())
                            samErrorCollection.CheckForErrors()

                        Case RiskStatusCode.QuotedPostQuoteQuestionsOutstanding,
                                RiskStatusCode.QuotedPreQuoteQuestionsOutstanding,
                                RiskStatusCode.QuotedPurchaseQuestionsOutstanding

                            samErrorCollection = New SAMErrorCollection
                            samErrorCollection.AddBusinessRule(
                                SAMBusinessErrors.AtLeastOneRiskHasOutstandingQuestions,
                                SAMBusinessErrors.AtLeastOneRiskHasOutstandingQuestions.ToString())
                            samErrorCollection.CheckForErrors()

                    End Select

                End If

            Next

            If Not quotedRiskIsSelected Then

                samErrorCollection = New SAMErrorCollection
                samErrorCollection.AddBusinessRule(
                    SAMBusinessErrors.AtLeastOneQuotedRisksMustBeSelected,
                    SAMBusinessErrors.AtLeastOneQuotedRisksMustBeSelected.ToString())
                samErrorCollection.CheckForErrors()

            End If

        End If

    End Sub

    Private Function GetInsuranceFileRisks(ByVal con As SiriusConnection, ByVal insuranceFileKey As Integer) As DataTable

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_risks_by_status")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            dt = con.ExecuteDataTable(cmd)
        End Using

        Return dt

    End Function

    Private Function CreateAndInitialiseAgentCommissionBusiness(
    ByVal con As SiriusConnection,
    ByVal branchCode As String) As BSirAgentCommission.Business

        Dim comInteropObject As BSirAgentCommission.Business = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Try

            comInteropObject = New BSirAgentCommission.Business

            SAMFunc.InitialiseSBOObject(con, comInteropObject, _SiriusUser, branchCode, "bSIRAgentCommission.Business")

        Catch ex As Exception
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.FailedToCreateCOMComponent,
                SAMBusinessErrors.FailedToCreateCOMComponent.ToString,
                "bSIRAgentCommission.Business")
            samErrorCollection.CheckForErrors()
        End Try

        Return comInteropObject

    End Function

    Private Function CreateAndInitialiseAccumulationValuesBusiness(
          ByVal con As SiriusConnection,
          ByVal branchCode As String) As bSIRAccumulationValues.Business

        Dim comInteropObject As bSIRAccumulationValues.Business = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Try

            comInteropObject = New bSIRAccumulationValues.Business

            SAMFunc.InitialiseSBOObject(con, comInteropObject, _SiriusUser, branchCode, "bSIRAccumulationValues.Business")

        Catch ex As Exception
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.FailedToCreateCOMComponent,
                SAMBusinessErrors.FailedToCreateCOMComponent.ToString,
                "bSIRAccumulationValues.Business")
            samErrorCollection.CheckForErrors()
        End Try

        Return comInteropObject

    End Function

    Private Function CreateAndInitialiseRenewalBusiness(
       ByVal con As SiriusConnection,
       ByVal branchCode As String) As bSIRRenewal.Business

        Dim comInteropObject As bSIRRenewal.Business = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Try

            comInteropObject = New bSIRRenewal.Business

            SAMFunc.InitialiseSBOObject(con, comInteropObject, _SiriusUser, branchCode, "bSIRRenewal.Business")

        Catch ex As Exception
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.FailedToCreateCOMComponent,
                SAMBusinessErrors.FailedToCreateCOMComponent.ToString,
                "bSIRRenewal.Business")
            samErrorCollection.CheckForErrors()
        End Try

        Return comInteropObject

    End Function

    Private Function CreateAndInitialiseControlTransBusiness(
    ByVal con As SiriusConnection,
    ByVal branchCode As String) As bControlTrans.Automated

        Dim comInteropObject As bControlTrans.Automated = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Try
            comInteropObject = New bControlTrans.Automated

            SAMFunc.InitialiseSBOObject(con, comInteropObject, _SiriusUser, branchCode, "bControlTrans.Automated")

        Catch ex As Exception
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.FailedToCreateCOMComponent,
                SAMBusinessErrors.FailedToCreateCOMComponent.ToString,
                "bControlTrans.Automated")
            samErrorCollection.CheckForErrors()

        End Try

        Return comInteropObject

    End Function

    Private Function CreateAndInitialiseListRisksBusiness(
    ByVal con As SiriusConnection,
    ByVal branchCode As String) As bSIRListRisks.Business

        Dim comInteropObject As bSIRListRisks.Business = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Try
            comInteropObject = New bSIRListRisks.Business

            SAMFunc.InitialiseSBOObject(con, comInteropObject, _SiriusUser, branchCode, "bSIRListRisks.Business")

        Catch ex As Exception
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.FailedToCreateCOMComponent,
                SAMBusinessErrors.FailedToCreateCOMComponent.ToString,
                "bSIRListRisks.Business")
            samErrorCollection.CheckForErrors()
        End Try

        Return comInteropObject

    End Function

    Private Function CreateAndInitialisePremiumFinanceBusiness(
    ByVal con As SiriusConnection,
    ByVal branchCode As String) As bSIRPremiumFinance.Business

        Dim comInteropObject As bSIRPremiumFinance.Business = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Try

            comInteropObject = New bSIRPremiumFinance.Business

            SAMFunc.InitialiseSBOObject(con, comInteropObject, _SiriusUser, branchCode, "bSIRPremiumFinance.Business")

        Catch ex As Exception
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.FailedToCreateCOMComponent,
                SAMBusinessErrors.FailedToCreateCOMComponent.ToString,
                "bSIRPremiumFinance.Business")
            samErrorCollection.CheckForErrors()
        End Try

        Return comInteropObject

    End Function

    Private Function CreateAndInitialiseFindInsuranceBusiness(
    ByVal con As SiriusConnection,
    ByVal branchCode As String) As bSIRFindInsurance.Form

        Dim comInteropObject As bSIRFindInsurance.Form = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Try

            comInteropObject = New bSIRFindInsurance.Form

            SAMFunc.InitialiseSBOObject(con, comInteropObject, _SiriusUser, branchCode, "bSIRFindInsurance.Form")

        Catch ex As Exception
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.FailedToCreateCOMComponent,
                SAMBusinessErrors.FailedToCreateCOMComponent.ToString,
                "bSIRFindInsurance.Form")
            samErrorCollection.CheckForErrors()
        End Try

        Return comInteropObject

    End Function

    Private Function CreateAndInitialiseAutoMTABusiness(
    ByVal con As SiriusConnection,
    ByVal branchCode As String) As bSIRAutoMTA.Business

        Dim comInteropObject As bSIRAutoMTA.Business = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Try

            comInteropObject = New bSIRAutoMTA.Business
            'Rk modifies as part of SAM SFI Interop conversions.
            SAMFunc.InitialiseSBOObject(con, comInteropObject, _SiriusUser, branchCode, "bSIRAutoMTA.Business")

        Catch ex As Exception
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.FailedToCreateCOMComponent,
                SAMBusinessErrors.FailedToCreateCOMComponent.ToString,
                "bSIRAutoMTA.Business")
            samErrorCollection.CheckForErrors()
        End Try

        Return comInteropObject

    End Function

    Private Function CreateAndInitialiseChangePolicyStatusBusiness(
        ByVal con As SiriusConnection,
        ByVal branchCode As String) As bSIRChangePolicyStatus.Business

        Dim comInteropObject As bSIRChangePolicyStatus.Business = Nothing
        Dim samErrorCollection As SAMErrorCollection = Nothing

        Try

            comInteropObject = New bSIRChangePolicyStatus.Business

            SAMFunc.InitialiseSBOObject(con, comInteropObject, _SiriusUser, branchCode, "bSIRChangePolicyStatus.Business")

        Catch ex As Exception
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.FailedToCreateCOMComponent,
                SAMBusinessErrors.FailedToCreateCOMComponent.ToString,
                "bSIRChangePolicyStatus.Business")
            samErrorCollection.CheckForErrors()
        End Try

        Return comInteropObject

    End Function

    Private Sub RequoteBackDatedMTA(ByVal con As SiriusConnection,
    ByVal request As BaseBindQuoteRequestType)

        Dim autoMTABusiness As bSIRAutoMTA.Business = CreateAndInitialiseAutoMTABusiness(con, request.BranchCode)
        Dim comReturnValue As Integer
        Dim backDataMTAVersionsObject As Object = Nothing

        Try

            comReturnValue = autoMTABusiness.GetBackdatedVersions(request.InsuranceFileKey, CType(CObj(backDataMTAVersionsObject), Object(,)))
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRAutoMTA.Business.GetBackdatedVersions", comReturnValue)
            End If

            Dim failureReason As String = String.Empty
            'WPR 33-75 ADDED
            comReturnValue = autoMTABusiness.SetProcessModes(vTransactionType:=request.TransactionType.ToString)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRAutoMTA.Business.QuoteMTA", comReturnValue)
            End If

            If request.TransactionType = "MTA" Then

                'comReturnValue = autoMTABusiness.QuoteMTA(v_lPartyCnt:=request.DataStore.PartyKey, _
                '            v_lInsuranceFolderCnt:=request.DataStore.InsuranceFolderKey, _
                '            v_dtEffectiveDate:=request.DataStore.CoverStartDate, _
                '            lBaseInsuranceFileCnt:=request.InsuranceFileKey, _
                '            r_sFailureMessage:=failureReason, _
                '            vBackdatedMTAVersions:=backDataMTAVersionsObject, _
                '            bUpdateStats:=True)
                comReturnValue = autoMTABusiness.QuoteMTA(v_lPartyCnt:=request.DataStore.PartyKey,
                           v_lInsuranceFolderCnt:=request.DataStore.InsuranceFolderKey,
                           v_dtEffectiveDate:=request.DataStore.CoverStartDate,
                           lBaseInsuranceFileCnt:=request.InsuranceFileKey,
                           r_sFailureMessage:=failureReason,
                           vBackdatedMTAVersions:=backDataMTAVersionsObject,
                           bUpdateStats:=True, bIsInteractive:=True)
                'WPR 33-75 END
                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRAutoMTA.Business.QuoteMTA", comReturnValue)
                End If

                If failureReason <> String.Empty Then
                    RaiseComMethodException("bSIRAutoMTA.Business.QuoteMTA Failed - " + failureReason)
                End If
                'WPR 33-75 ADDED


            ElseIf request.TransactionType = "MTC" Then
                Dim lNewInsuranceFileCnt As Integer
                comReturnValue = autoMTABusiness.QuoteCancellation(v_lPartyCnt:=request.DataStore.PartyKey,
                            v_lInsuranceFolderCnt:=request.DataStore.InsuranceFolderKey,
                            v_dtEffectiveDate:=request.DataStore.CoverStartDate,
                            lBaseInsuranceFileCnt:=request.InsuranceFileKey,
                            r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt,
                            r_sFailureMessage:=failureReason,
                            bUpdateStats:=True,
                            bBackDateMTA:=True)

                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRAutoMTA.Business.QuoteCancellation", comReturnValue)
                End If

                If failureReason <> String.Empty Then
                    RaiseComMethodException("bSIRAutoMTA.Business.QuoteCancellation Failed - " + failureReason)
                End If
            ElseIf request.TransactionType = "MTR" Then
                Dim lNewInsuranceFileCnt As Integer

                comReturnValue = autoMTABusiness.QuoteReinstatement(nPartyCnt:=request.DataStore.PartyKey,
                            nInsuranceFolderCnt:=request.DataStore.InsuranceFolderKey,
                            dtEffectiveDate:=request.DataStore.CoverStartDate,
                            nBaseInsuranceFileCnt:=request.InsuranceFileKey,
                            nNewInsuranceFileCnt:=lNewInsuranceFileCnt,
                            sFailureMessage:=failureReason,
                            bUpdateStats:=True,
                            bBackDateMTA:=True)

                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRAutoMTA.Business.QuoteReinstatement", comReturnValue)
                End If

                If failureReason <> String.Empty Then
                    RaiseComMethodException("bSIRAutoMTA.Business.QuoteReinstatement Failed - " + failureReason)
                End If
            End If
            'WPR 33-75 END
        Finally
            If autoMTABusiness IsNot Nothing Then
                autoMTABusiness.Dispose()
                autoMTABusiness = Nothing
            End If
        End Try

    End Sub

    Private Function IsBackdatedMTA(
    ByVal con As SiriusConnection,
    ByVal branchCode As String,
    ByVal insuranceFileKey As Integer,
    ByVal startDate As Date) As Boolean

        Dim findInsuranceBusiness As bSIRFindInsurance.Form = CreateAndInitialiseFindInsuranceBusiness(con, branchCode)
        Dim comReturnValue As Integer
        Dim policyVersion As Integer
        Dim errorCode As Integer

        Try

            comReturnValue = findInsuranceBusiness.GetVersionByDate(insuranceFileKey, startDate, policyVersion, errorCode)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRListRisks.Business.CopyRisksMTA", comReturnValue)
            End If

            If errorCode = 8 Then
                Return True
            Else
                Return False
            End If

        Finally
            If findInsuranceBusiness IsNot Nothing Then
                findInsuranceBusiness.Dispose()
                findInsuranceBusiness = Nothing
            End If
        End Try

    End Function

    Private Sub RaiseComMethodException(ByVal detail As String)

        Dim samErrorCollection As SAMErrorCollection = New SAMErrorCollection

        samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.COMComponentMethodFailed,
                SAMBusinessErrors.COMComponentMethodFailed.ToString(),
                detail)

        samErrorCollection.CheckForErrors()

    End Sub

    Private Sub RaiseComMethodException(ByVal componentAndMethodName As String, ByVal comReturnValue As Integer)

        Dim samErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim detail As String = componentAndMethodName

        detail += " : Failed : Return Value = " + CType(comReturnValue, PMEReturnCode).ToString()
        samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.COMComponentMethodFailed,
                SAMBusinessErrors.COMComponentMethodFailed.ToString(),
                detail)
        samErrorCollection.CheckForErrors()

    End Sub
    ''' <summary>
    ''' GetInsuranceFileDetails
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="request"></param>
    ''' <param name="listRisksBusiness"></param>
    ''' <remarks></remarks>
    Private Sub GetInsuranceFileDetails(
    ByRef con As SiriusConnection,
    ByRef request As BaseBindQuoteRequestType,
    ByRef listRisksBusiness As bSIRListRisks.Business, Optional ByVal bIsSelectLivePlan As Boolean = False)

        Dim samErrorCollection As SAMErrorCollection = Nothing
        Dim oInsuranceFileDetailsObject(,) As Object = Nothing
        Dim nComReturnValue As Integer

        Const kInsuranceRef As Integer = 0
        Const kCoverStartDate As Integer = 2
        Const kPartyShortname As Integer = 4
        Const kAgentShortname As Integer = 7
        Const kAgentType As Integer = 13
        Const kIsTrueMonthlyPolicy As Integer = 15
        Const kProductId As Integer = 19
        Const kPartyKey As Integer = 23
        Const kInsuranceFolderKey As Integer = 24
        Const kLeadAgentKey As Integer = 25
        Const kTotalNetPremium As Integer = 26
        Const kTotalAgentCommission As Integer = 27
        Const kPFPremiumFinanceCnt As Integer = 28
        Const kPFPremiumFinanceVersion As Integer = 29
        Const kOriginalInsuranceFileKey As Integer = 30
        Const kAnniversaryCopy As Integer = 31
        Const kiSourceId As Integer = 32
        Const kiCurrencyId As Integer = 33
        Const kTotalAgentCommissionTax As Integer = 34
        Const kCollectionFrequencyId As Integer = 42
        Const kPaymentTermsId As Integer = 43

        Dim oOriginalInsuranceFileKeyObject As Object = Nothing

        If request.TransactionType <> TransactionTypeCode.NewBusiness Then
            If request.TransactionType = TransactionTypeCode.ReinstatePolicy Then
                GetCurrentInsuranceFileDetails(con, request.InsuranceFileKey, request.DataStore.OriginalInsuranceFileKey, Nothing, Nothing, request.DataStore.OriginalInsuranceFileStatus)
            Else
                GetOriginalInsuranceFileDetails(
                    con,
                    request.InsuranceFileKey,
                    request.DataStore.OriginalInsuranceFileKey,
                    Nothing,
                    Nothing)
            End If

            oOriginalInsuranceFileKeyObject = CType(request.DataStore.OriginalInsuranceFileKey, Object)

        End If

        nComReturnValue = listRisksBusiness.GetInsuranceFileDetails(
                        v_lInsuranceFileCnt:=request.InsuranceFileKey,
                        r_vResults:=oInsuranceFileDetailsObject,
                        v_lOriginalInsuranceFileCnt:=oOriginalInsuranceFileKeyObject, bIsSelectLivePlan:=bIsSelectLivePlan)

        If nComReturnValue <> PMEReturnCode.PMTrue Then
            samErrorCollection = New SAMErrorCollection
            samErrorCollection.AddBusinessRule(
                SAMBusinessErrors.COMComponentMethodFailed,
                SAMBusinessErrors.COMComponentMethodFailed.ToString(),
                "bSIRListRisks.Business.GetInsuranceFileDetails")
            samErrorCollection.CheckForErrors()
        End If

        If oInsuranceFileDetailsObject IsNot Nothing Then

            Dim insuranceFileDetailsArray As Object(,) = DirectCast(oInsuranceFileDetailsObject, Object(,))

            If request.DataStore Is Nothing Then
                request.DataStore = New BaseBindQuoteRequestDataStore
            End If

            request.DataStore.TrueMonthlyPolicyIndicator = StringDataConvert.ToInt32(insuranceFileDetailsArray(kIsTrueMonthlyPolicy, 0), 0)
            request.DataStore.CoverStartDate = StringDataConvert.ToDateTime(insuranceFileDetailsArray(kCoverStartDate, 0), Date.MinValue)
            request.DataStore.InsuranceFolderKey = StringDataConvert.ToInt32(insuranceFileDetailsArray(kInsuranceFolderKey, 0), 0)
            request.DataStore.InsuranceRef = Cast.ToString(insuranceFileDetailsArray(kInsuranceRef, 0), String.Empty)
            request.DataStore.PartyKey = StringDataConvert.ToInt32(insuranceFileDetailsArray(kPartyKey, 0), 0)
            request.DataStore.ProductId = StringDataConvert.ToInt32(insuranceFileDetailsArray(kProductId, 0), 0)

            request.DataStore.SourceId = StringDataConvert.ToInt32(insuranceFileDetailsArray(kiSourceId, 0), 0)
            request.DataStore.LeadAgnetKey = StringDataConvert.ToInt32(insuranceFileDetailsArray(kLeadAgentKey, 0), 0)
            request.DataStore.CurrencyId = StringDataConvert.ToInt32(insuranceFileDetailsArray(kiCurrencyId, 0), 0)

            ' replace these hacked returns to cope with null values
            request.DataStore.TotalAgentPremium = StringDataConvert.ToDecimal(insuranceFileDetailsArray(kTotalNetPremium, 0), 0D)
            request.DataStore.TotalAgentCommission = StringDataConvert.ToDecimal(insuranceFileDetailsArray(kTotalAgentCommission, 0), 0D)

            If Cast.ToString(insuranceFileDetailsArray(kLeadAgentKey, 0)) = String.Empty Then
                request.DataStore.PolicyHasAgent = False
            Else
                request.DataStore.PolicyHasAgent = True
            End If

            ' get the back data status
            request.DataStore.IsBackDatedMTA = IsBackdatedMTA(con, request.BranchCode, request.InsuranceFileKey, request.DataStore.CoverStartDate)
            request.DataStore.PFPremiumFinanceCnt = StringDataConvert.ToInt32(insuranceFileDetailsArray(kPFPremiumFinanceCnt, 0), 0)
            request.DataStore.PFPremiumFinanceVersion = StringDataConvert.ToInt32(insuranceFileDetailsArray(kPFPremiumFinanceVersion, 0), 0)
            request.DataStore.OriginalInsuranceFileKey = StringDataConvert.ToInt32(insuranceFileDetailsArray(kOriginalInsuranceFileKey, 0), 0)
            request.DataStore.PartyShortname = Cast.ToStringTrim(insuranceFileDetailsArray(kPartyShortname, 0))
            request.DataStore.AgentShortname = Cast.ToStringTrim(insuranceFileDetailsArray(kAgentShortname, 0))
            request.DataStore.AnniversaryCopy = BooleanDataConvert.ToBoolean(StringDataConvert.ToInt32(insuranceFileDetailsArray(kAnniversaryCopy, 0), 0))
            request.DataStore.AgentType = Cast.ToStringTrim(insuranceFileDetailsArray(kAgentType, 0))
            request.DataStore.TotalAgentCommissionTax = StringDataConvert.ToDecimal(insuranceFileDetailsArray(kTotalAgentCommissionTax, 0), 0D)
            request.DataStore.CollectionFrequency = StringDataConvert.ToInt32(insuranceFileDetailsArray(kCollectionFrequencyId, 0), 0)
            request.DataStore.PaymentTerms = StringDataConvert.ToInt32(insuranceFileDetailsArray(kPaymentTermsId, 0), 0)

        End If

    End Sub

    Private Sub CopyMTARisks(
     ByRef con As SiriusConnection,
     ByRef listRisksBusiness As bSIRListRisks.Business,
     ByVal insuranceFileKey As Integer, Optional ByRef TransactionType As TransactionType = Nothing,
     Optional ByVal Is_SAM_Copy_Quote As Boolean = False, Optional ByVal bCalledViaAddMTA As Boolean = False, Optional ByVal bSetUnQuoted As Boolean = False, Optional ByVal bCopyRiskMTA As Boolean = False)
        'Added  TransactionType as parameter for sub method CopyMTARisks to update the risk status in risk table(SAM Gap done by Vijayakumar as per discussed with Gaurav on 06-Nov-2008)
        Dim samErrorCollection As SAMErrorCollection = Nothing
        Dim comReturnValue As Integer
        Dim createLink As Integer = 1


        If TransactionType = BaseImplementationTypes.TransactionType.QUOTE Then
            createLink = 0
        End If
        comReturnValue = listRisksBusiness.SetProcessModes(vTransactionType:=TransactionType.ToString())
        If bCalledViaAddMTA Then
            comReturnValue = listRisksBusiness.CopyRisksMTA(insuranceFileKey, Is_SAM_Copy_Quote:=Is_SAM_Copy_Quote, bFromSAM:=True, bCopyRiskMTA:=bCopyRiskMTA)
            listRisksBusiness.ProcessAgentCommission(insuranceFileKey)
        Else
            'comReturnValue = listRisksBusiness.CopyRisksMTA(insuranceFileKey, bFromSAM:=True)
            comReturnValue = listRisksBusiness.CopyRisksMTA(insuranceFileKey, createLink, Is_SAM_Copy_Quote, True, bCopyRiskMTA)
        End If
        If comReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRListRisks.Business.CopyRisksMTA", comReturnValue)
        ElseIf comReturnValue = PMEReturnCode.PMTrue AndAlso (TransactionType = TransactionType.MTC OrElse TransactionType = TransactionType.MTR OrElse bSetUnQuoted Or Is_SAM_Copy_Quote) Then
            'Implemented stored Procedure "spu_SIR_Update_Risk_Status" to update the risk status in risk table for TransactionType MTC and MTR (SAM Gap done by Vijayakumar as per discussed with Gaurav on 06-Nov-2008)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Update_Risk_Status")
                cmd.AddInParameter("@risk_status_code", SqlDbType.VarChar, 20).Value = "UNQUOTED"
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
                con.ExecuteNonQuery(cmd)
            End Using
        End If

    End Sub

    Public Function GetDatasetSchema(ByVal GetDatasetSchemaRequest As BaseGetDatasetSchemaRequestType) As BaseGetDatasetSchemaResponseType

        Const ACMethodName As String = "GetDatasetSchema"
        Const ACDatasetSchemaKey As String = "DatasetSchemaKey_"

        ' Declare the Response object
        Dim oResponse As New BaseGetDatasetSchemaResponseType
        ' Declare the Core Business object
        Dim oBusiness As New CoreBusiness
        Dim CacheKey As String = String.Empty
        Dim sDatasetSchema As String = String.Empty
        Dim sDatasetSchemaFilename As String = String.Empty
        Dim sDatasetSchemaPath As String = String.Empty
        Dim iReturn As Integer
        Dim iDataModelId As Integer
        Dim iSourceId As Integer
        Dim STSError As New STSErrorPublisher

        ' Check if the branch code was passed in
        If SAMFunc.NothingToString(GetDatasetSchemaRequest.DataModelCode) = "" Then
            STSError.AddInvalidField("DataModelCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "DataModelCode"), "")
        End If

        ' Check if the branch code was passed in
        If SAMFunc.NothingToString(GetDatasetSchemaRequest.BranchCode) = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        ' DataModelCode
        Try
            iDataModelId% = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "GIS_Data_Model", GetDatasetSchemaRequest.DataModelCode)
        Catch ex As Exception
            ' not set
            STSError.AddInvalidField("DataModelCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "DataModelCode"), GetDatasetSchemaRequest.DataModelCode)
        End Try

        iSourceId% = 1
        ' Convert branch code to ID
        Try
            iSourceId% = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", GetDatasetSchemaRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), GetDatasetSchemaRequest.BranchCode)
        End Try

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Input validation", True)
            Return oResponse
        End If

        ' Get a reference to the cache object
        _oCache = HttpRuntime.Cache()

        ' Append Screen Code because we will have seperate datasets for each type of screen.
        CacheKey = ACDatasetSchemaKey & GetDatasetSchemaRequest.DataModelCode

        If (_oCache(CacheKey) Is Nothing) = False Then

            sDatasetSchema = CType(_oCache(CacheKey), String)

        Else

            iReturn = GetPMRegSetting(PMERegSettingRoot.pmeRSRLocalMachine, PMEProductFamily.pmePFSiriusSolutions, PMERegSettingLevel.pmeRSLServer, "DataSetsPath", sDatasetSchemaPath, "GIS\" & GetDatasetSchemaRequest.DataModelCode)
            If iReturn <> PMEReturnCode.PMTrue Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.DatasetPathRegistrySettingNotFound, "GetPMRegSetting failed", "The registry setting for the datasets path does not exist for datamodel " & GetDatasetSchemaRequest.DataModelCode)
                STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDatasetSchema", True)
                Return oResponse
            End If

            sDatasetSchemaPath = Cast.ToString(IIf((Right(sDatasetSchemaPath, 1) <> "\"), CObj(sDatasetSchemaPath & "\"), CObj(sDatasetSchemaPath)), String.Empty)
            sDatasetSchemaFilename = sDatasetSchemaPath & GetDatasetSchemaRequest.DataModelCode & "XSD.xsd"

            Try
                sDatasetSchema = My.Computer.FileSystem.ReadAllText(sDatasetSchemaFilename)
            Catch FileNotFoundEx As IO.FileNotFoundException
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.FileNotFound, "GetDatasetSchema failed", "The datasets file does not exist : " & sDatasetSchemaFilename)
                STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDatasetSchema", True)
                Return oResponse
            Catch ex As Exception
                Dim STSErrorEX As New STSErrorPublisher("GetDatasetSchema failed", ex)
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDatasetSchema", True)
            End Try

            Dim oDependency As New System.Web.Caching.CacheDependency(sDatasetSchemaFilename)

            ' Add the Permission indicator into the cache
            _oCache.Insert(CacheKey, sDatasetSchema, oDependency, DateTime.MaxValue, TimeSpan.FromHours(1))

        End If

        oResponse.DatasetSchema = sDatasetSchema

        Return oResponse

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetInstalmentQuotesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' wpr10
    Public Function GetInstalmentQuotes(ByVal GetInstalmentQuotesRequest As BaseGetInstalmentQuotesRequestType) As BaseGetInstalmentQuotesResponseType

        Const ACMethodName As String = "GetInstalmentQuotes"

        ' Declare the Response object
        Dim oResponse As New BaseGetInstalmentQuotesResponseType
        ' Declare the Core Business object
        Dim oBusiness As New CoreBusiness
        Dim CacheKey As String = String.Empty
        Dim sDatasetSchema As String = String.Empty
        Dim sDatasetSchemaFilename As String = String.Empty
        Dim sDatasetSchemaPath As String = String.Empty
        Dim iReturn As Integer
        'Dim iDataModelId As Integer
        Dim iSourceId As Integer
        Dim STSError As New STSErrorPublisher
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim vResultArray As Object = Nothing
        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.GetInstalmentQuotesRequestType = Nothing

        ' Set type of package
        If GetInstalmentQuotesRequest.GetType Is GetType(BaseImplementationTypes.BaseGetInstalmentQuotesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oResponse = New BaseImplementationTypes.BaseGetInstalmentQuotesResponseType
        ElseIf GetInstalmentQuotesRequest.GetType Is GetType(AgentImplementationTypes.GetInstalmentQuotesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.GetInstalmentQuotesResponseType
        ElseIf GetInstalmentQuotesRequest.GetType Is GetType(SAMForBrokingImplementationTypes.GetInstalmentQuotesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oResponse = New SAMForBrokingImplementationTypes.GetInstalmentQuotesResponseType
            samForBrokingRequest = DirectCast(GetInstalmentQuotesRequest, SAMForBrokingImplementationTypes.GetInstalmentQuotesRequestType)
        ElseIf GetInstalmentQuotesRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.GetInstalmentQuotesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.GetInstalmentQuotesResponseType
        ElseIf GetInstalmentQuotesRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetInstalmentQuotesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetInstalmentQuotesResponseType
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                If oBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Agent Key validation failed", "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Key Validation", True)
                    Return oResponse
                End If

                If oBusiness.AgentSecurityCheck(samForBrokingRequest.UserName, samForBrokingRequest.InsuranceFileKey, PMEEntityType.InsuranceFile) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security Check Failed", samForBrokingRequest.UserName & " does not have permission to access insurancefile " & samForBrokingRequest.InsuranceFileKey)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetInstalmentQuotes", True)
                    Return oResponse
                End If
            End If
        End If

        ' Check Mandatory fields
        If SAMFunc.NothingToString(GetInstalmentQuotesRequest.BranchCode) = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If
        If GetInstalmentQuotesRequest.InsuranceFileKey = 0 Then
            STSError.AddInvalidField("InsuranceFileKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFileKey"), "")
        End If
        If GetInstalmentQuotesRequest.QuoteDate = New Date Then
            STSError.AddInvalidField("QuoteDate", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "QuoteDate"), "")
        End If
        If GetInstalmentQuotesRequest.StartDate = New Date Then
            STSError.AddInvalidField("StartDate", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "StartDate"), "")
        End If
        If GetInstalmentQuotesRequest.EndDate = New Date Then
            STSError.AddInvalidField("EndDate", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "EndDate"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        iSourceId% = 1
        ' Convert branch code to ID
        Try
            iSourceId% = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", GetInstalmentQuotesRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), GetInstalmentQuotesRequest.BranchCode)
        End Try

        If GetInstalmentQuotesRequest.StartDate > GetInstalmentQuotesRequest.EndDate Then
            STSError.AddInvalidField("StartDate", CStr(STSErrorCodes.EndDateIsBeforeStartDate), "EndDate is before StartDate", GetInstalmentQuotesRequest.StartDate.ToString)
        End If

        ' exit if there are any invalid parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Input validation", True)
            Return oResponse
        End If
        Dim nPartyBankId As Integer = 0
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim productCode As String = String.Empty
            Dim insuranceFileTypeId As Integer = 0
            Dim insuranceFileTypeCode As String = String.Empty
            Dim originalInsuranceFileCnt As Integer = 0
            Dim premiumFinanceCnt As Integer = 0
            Dim premiumFinanceVersion As Integer = 0
            Dim nPolicyVersion As Integer = 0
            Dim bDayOfMonthIsValid As Boolean = False
            Dim nOrigionalPremiumFinanceCnt As Integer = 0
            Dim nOrigionalPremiumFinanceVersion As Integer = 0

            nOrigionalPremiumFinanceCnt = GetInstalmentQuotesRequest.PFPremFinanceKey
            nOrigionalPremiumFinanceVersion = GetInstalmentQuotesRequest.PFPremFinanceVersion

            'Get the insurance File Type to see what type of business we're doing
            GetInsuranceFileType(con, GetInstalmentQuotesRequest.InsuranceFileKey, insuranceFileTypeId, insuranceFileTypeCode, nPolicyVersion)

            Select Case insuranceFileTypeCode
                Case InsuranceFileType.LivePolicy, InsuranceFileType.Quote, InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated, InsuranceFileType.Written
                    If (InsuranceFileType.LivePolicy = Trim(insuranceFileTypeCode)) AndAlso nPolicyVersion > 1 Then
                        productCode = "REN"
                    Else
                        productCode = "NB"
                    End If

                Case InsuranceFileType.MTAPermanentQuotation, InsuranceFileType.MTATemporaryQuotation,
                    InsuranceFileType.MTAPermanent, InsuranceFileType.MTATemporary, InsuranceFileType.MTACancellation,
                    InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated, InsuranceFileType.MTAQuotationCancellation
                    productCode = "MTA"

                Case InsuranceFileType.Renewal
                    productCode = "REN"
                    bDayOfMonthIsValid = True

            End Select

            'Instalment Plan Maintenance
            If GetInstalmentQuotesRequest.ProcessPFMode IsNot Nothing AndAlso
               GetInstalmentQuotesRequest.ProcessPFMode <> "" Then

                Select Case GetInstalmentQuotesRequest.ProcessPFMode.ToUpper()
                    Case "MTA"
                        productCode = "MTA"
                    Case "NB"
                        productCode = "NB"
                    Case "REN"
                        productCode = "REN"
                End Select
            End If
            ' If it's an MTA or Renewal then get the original premium finance details
            If productCode = "MTA" Or productCode = "REN" Then
                GetOriginalInsuranceFileDetails(con, GetInstalmentQuotesRequest.InsuranceFileKey, originalInsuranceFileCnt, premiumFinanceCnt, premiumFinanceVersion, nPartyBankId)
            End If

            ' Reference to the Premium Finance object
            Dim oPremiumFinance As bSIRPremiumFinance.Business = Nothing

            Try
                oPremiumFinance = New bSIRPremiumFinance.Business
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher("Failed to create bSIRPremiumFinance.Business", ex)
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetInstalmentQuotes", True)
            End Try

            ' Initialise the GIS
            Try
                SAMFunc.InitialiseSBOObject(con, oPremiumFinance, _SiriusUser, GetInstalmentQuotesRequest.BranchCode)
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher("Failed to initialise bSIRPremiumFinance.Business", ex)
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetInstalmentQuotes", True)
            End Try

            'This  Condition will executed when System wants to calculate the Next Instalment Due date and returns to Portal while doing MTA.
            If GetInstalmentQuotesRequest.PreferredInstalmentDueDateonly Then

                Dim frequency As String = String.Empty
                Dim dayOfWeekOrMonth As Integer = 0
                Dim firstInstalmentDate As Date = Date.MinValue

                iReturn = oPremiumFinance.GetPreferredDates(
                                    v_lPremFinanceVersion:=GetInstalmentQuotesRequest.PFPremFinanceVersion,
                                    v_lPremFinanceCnt:=GetInstalmentQuotesRequest.PFPremFinanceKey,
                                    r_sFrequency:=frequency,
                                    r_lDayOfWeekOrMonth:=dayOfWeekOrMonth,
                                    r_dtFirstInstalmentDate:=firstInstalmentDate,
                                    v_sProductCode:=productCode)


                ' Set the Preferred date to the current first instalment date
                oResponse.PreferredInstalmentDueDate = firstInstalmentDate
            Else
                'Doing this Intensionally so that because Existing Functionality doen't break as avoid to call the GetPreferredDates further.
                GetInstalmentQuotesRequest.PFPremFinanceKey = 0
                GetInstalmentQuotesRequest.PFPremFinanceVersion = 0
            End If

            'Dont overwrite the values if we get ProcessPFMode
            If productCode = "MTA" Or productCode = "REN" Then
                ' If it's an MTA or Renewal then get the original premium finance details
                GetOriginalInsuranceFileDetails(con, GetInstalmentQuotesRequest.InsuranceFileKey, originalInsuranceFileCnt, premiumFinanceCnt, premiumFinanceVersion)
            End If
            ' Assign live plan no and version when plan was completed and one more plan available on policy during underwitter MTA
            If productCode = "MTA" AndAlso nOrigionalPremiumFinanceCnt <> 0 AndAlso (insuranceFileTypeCode = InsuranceFileType.MTAPermanentQuotation OrElse insuranceFileTypeCode = InsuranceFileType.MTATemporaryQuotation OrElse insuranceFileTypeCode = InsuranceFileType.MTAQuotationReinstatement) Then
                premiumFinanceCnt = nOrigionalPremiumFinanceCnt
                premiumFinanceVersion = nOrigionalPremiumFinanceVersion
            End If
            If GetInstalmentQuotesRequest.ProcessPFMode IsNot Nothing AndAlso
                GetInstalmentQuotesRequest.ProcessPFMode <> "" Then

                premiumFinanceCnt = GetInstalmentQuotesRequest.PFPremFinanceKey
                premiumFinanceVersion = GetInstalmentQuotesRequest.PFPremFinanceVersion
                originalInsuranceFileCnt = GetInstalmentQuotesRequest.InsuranceFileKey

            End If

            ' Call the Method
            Try
                Dim oFinancePlanTransArray As Object(,) = Nothing
                If GetInstalmentQuotesRequest.PFTransaction IsNot Nothing AndAlso IsArray(GetInstalmentQuotesRequest.PFTransaction) Then

                    ReDim Preserve oFinancePlanTransArray(5, GetInstalmentQuotesRequest.PFTransaction.Length - 1)

                    For lCount As Integer = 0 To GetInstalmentQuotesRequest.PFTransaction.Length - 1
                        oFinancePlanTransArray(0, lCount) = GetInstalmentQuotesRequest.PFTransaction(lCount).TransDetailKey
                        oFinancePlanTransArray(1, lCount) = ""
                        oFinancePlanTransArray(2, lCount) = GetInstalmentQuotesRequest.PFTransaction(lCount).OutstandingAmount
                        oFinancePlanTransArray(3, lCount) = GetInstalmentQuotesRequest.PFTransaction(lCount).Spare
                        oFinancePlanTransArray(4, lCount) = 0
                        oFinancePlanTransArray(5, lCount) = GetInstalmentQuotesRequest.PFTransaction(lCount).InsuranceFileKey
                    Next
                End If

                With GetInstalmentQuotesRequest
                    Dim frequency As String = String.Empty
                    Dim dayOfWeekOrMonth As Integer = 0
                    Dim firstInstalmentDate As Date = Date.MinValue

                    ' Set the insurancefilecnt
                    oPremiumFinance.InsuranceFileCnt = .InsuranceFileKey
                    Dim insuranceFileCnt As Integer = 0

                    If originalInsuranceFileCnt = 0 Then
                        originalInsuranceFileCnt = .InsuranceFileKey
                        insuranceFileCnt = 0
                    Else
                        insuranceFileCnt = .InsuranceFileKey
                    End If

                    If premiumFinanceCnt <> 0 And premiumFinanceVersion <> 0 Then
                        If InstalmentType.AddToNewPlan = GetInstalmentQuotesRequest.InstalmentType Then
                            dayOfWeekOrMonth = GetInstalmentQuotesRequest.MonthDay
                        End If

                        ' RAW 05/11/2003 : CQ2912, 2976 : added 1st Instalment Date param
                        iReturn = oPremiumFinance.GetPreferredDates(
                                            v_lPremFinanceVersion:=premiumFinanceVersion,
                                            v_lPremFinanceCnt:=premiumFinanceCnt,
                                            r_sFrequency:=frequency,
                                            r_lDayOfWeekOrMonth:=dayOfWeekOrMonth,
                                            r_dtFirstInstalmentDate:=firstInstalmentDate,
                                            v_sProductCode:=productCode)

                        Select Case frequency
                            Case "m"
                                .MonthDay = dayOfWeekOrMonth
                            Case "w"
                                .WeekDay = dayOfWeekOrMonth
                        End Select

                        ' Set the Preferred date to the current first instalment date
                        .PreferredDate = firstInstalmentDate

                    End If

                    iReturn = oPremiumFinance.SetProcessModes(vTask:=PMEComponentAction.PMEdit)
                    'Begin WPR36A
                    Dim iInstalmentType As Integer = 0

                    If InstalmentType.AddAndSpread = GetInstalmentQuotesRequest.InstalmentType Then
                        iInstalmentType = InstalmentType.AddAndSpread
                    ElseIf InstalmentType.AddToNewPlan = GetInstalmentQuotesRequest.InstalmentType Then
                        iInstalmentType = InstalmentType.AddToNewPlan
                    ElseIf InstalmentType.AddToNext = GetInstalmentQuotesRequest.InstalmentType Then
                        iInstalmentType = InstalmentType.AddToNext
                    ElseIf InstalmentType.NoAmountChange = GetInstalmentQuotesRequest.InstalmentType Then
                        iInstalmentType = InstalmentType.NoAmountChange
                    End If
                    'End WPR36A

                    oPremiumFinance.UseTransCurrency = 0

                    If .IsUseTransactionCurrency Then
                        oPremiumFinance.UseTransCurrency = 1
                    End If


                    'Explicitly Assign the value to setting the correct dayofmonth from Portal.
                    oPremiumFinance.m_iDayOfWeekOrMonth = 1
                    iReturn = oPremiumFinance.Calculate_Quotes(v_lSourceID:=iSourceId%,
                                                            v_sProductCode:=productCode,
                                                            v_dtQuoteDate:= .QuoteDate,
                                                            v_dtStartDate:= .StartDate,
                                                            v_dtEndDate:= .EndDate,
                                                            v_dtPreferredDate:= .PreferredDate,
                                                            v_iDayInMonth:=CShort(.MonthDay),
                                                            v_iDayInWeek:=CShort(.WeekDay),
                                                            v_crAmountToFinance:=CDec(.AmountToFinance),
                                                            v_bPaymentProtection:= .PaymentProtection,
                                                            v_dInterestOverrideRate:= .OverrideInterestRate,
                                                            v_bOverrideCommission:= .OverrideCommission,
                                                            v_dOverrideDeposit:= .OverrideDepositAmount,
                                                            v_vPFTransArray:=oFinancePlanTransArray,
                                                            v_lPartyCnt:=0,
                                                            r_vQuoteArray:=vResultArray,
                                                            v_lInsuranceFileCnt:=originalInsuranceFileCnt,
                                                            v_lRenewalInsFileCnt:=insuranceFileCnt,
                                                            v_iMTAType:=CShort(iInstalmentType),
                                                            r_lPremFinanceCnt:=premiumFinanceCnt,
                                                            r_lPremFinanceVer:=premiumFinanceVersion,
                                                            bDayOfMonthIsValid:=bDayOfMonthIsValid)

                End With

                If Trim(oPremiumFinance.ErrorCode) <> "" Then
                    If CLng(oPremiumFinance.ErrorCode) = 7 Then
                        Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "The User Credentials for the remote Premium Finance web service were invalid", "")
                        STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                        Return oResponse
                    Else
                        Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "bSIRPremiumFinance.Business.Calculate_Quotes returned the error code - " & oPremiumFinance.ErrorCode, "")
                        STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                        Return oResponse
                    End If
                End If

                If (iReturn <> PMEReturnCode.PMTrue) Then
                    ' A return code of 7 indicates invalid user credentials for the remote server
                    If (iReturn = 7) Then
                    ElseIf iReturn = PMReturnCode.PMNotFound Then
                        Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.RecordNotFound, "bSIRPremiumFinance.Business.Calculate_Quotes failed", "")
                        STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                        Return oResponse
                    Else
                        Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "bSIRPremiumFinance.Business.Calculate_Quotes failed", "")
                        STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                        Return oResponse
                    End If
                End If
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "bSIRPremiumFinance.Business.Calculate_Quotes failed", ex.Message)
                STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                Return oResponse
            End Try

            Try
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "Failed to terminate business", ex.Message)
                STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                Return oResponse
            End Try

        End Using

        ' Populate the Column structure
        Dim arrColHeaders(,) As Object = {{"CompanyNo", "CompanyName", "SchemeNo", "SchemeVersion",
                                            "SchemeName", "FrequencyID", "FrequencyDescription", "MediaTypeID",
                                            "MediaTypeDescription", "ProductClass", "ProductCode", "TotalAmountInput",
                                            "InstalmentsToPay", "FirstInstalmentDate", "NextInstalmentDate", "LastInstalmentDate",
                                            "FirstInstalmentAmount", "OtherInstalmentAmount", "TotalInstalmentsAmount", "AprRate",
                                            "InterestRate", "DaysDelay", "DepositAmount", "InterestAmount",
                                            "TaxAmount", "FinanceCharge", "ProtectionAmount", "OriginalOtherInstalmentAmount",
                                            "HighlightCell", "SchemeTypeCode", "MediaTypeValidation", "FrequencyPerYear",
                                            "PFRF_ID", "FrequencyPeriod", "FrequencyAmount", "OriginalAmount",
                                            "ClaimDebtID", "UserID", "AgentCnt", "AgentRef",
                                            "LastInstalmentAmount", "Username", "Password", "BrokerID",
                                            "BrokerURL", "Timeout", "ProviderCode", "Terms",
                                            "Ref", "OriginalRate", "RefundType", "MinMTA", "QuoteTaxGroupID", "QuoteXSLCode", "SGSchemeType",
                                            "DepositAsInstalment", "AlignTo", "BranchCodeMandatory", "BranchNameMandatory", "BankNameMandatory",
                                            "BankAddressMandatory", "StartLimit", "StartDate", "DelayULimit", "SingleInstalmentPerMonth", "FirstInstalmentAlignWithDayInMonth", "UseTransCurrncy",
                                            "FinanceToNet"},
                                            {System.Type.GetType("System.Int32"), System.Type.GetType("System.String"),
                                            System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"), System.Type.GetType("System.String"), System.Type.GetType("System.Int32"),
                                            System.Type.GetType("System.String"), System.Type.GetType("System.Int32"), System.Type.GetType("System.String"), System.Type.GetType("System.String"),
                                            System.Type.GetType("System.String"), System.Type.GetType("System.Double"), System.Type.GetType("System.Int32"), System.Type.GetType("System.DateTime"),
                                            System.Type.GetType("System.DateTime"), System.Type.GetType("System.DateTime"), System.Type.GetType("System.Double"), System.Type.GetType("System.Double"),
                                            System.Type.GetType("System.Double"), System.Type.GetType("System.Double"), System.Type.GetType("System.Double"), System.Type.GetType("System.Int32"),
                                            System.Type.GetType("System.Double"), System.Type.GetType("System.Double"), System.Type.GetType("System.Double"), System.Type.GetType("System.Double"),
                                            System.Type.GetType("System.Double"), System.Type.GetType("System.Double"), System.Type.GetType("System.Int32"), System.Type.GetType("System.String"),
                                            System.Type.GetType("System.String"), System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"), System.Type.GetType("System.String"),
                                            System.Type.GetType("System.Double"), System.Type.GetType("System.Double"), System.Type.GetType("System.Int32"), System.Type.GetType("System.String"),
                                            System.Type.GetType("System.Int32"), System.Type.GetType("System.String"), System.Type.GetType("System.Double"), System.Type.GetType("System.String"),
                                            System.Type.GetType("System.String"), System.Type.GetType("System.String"), System.Type.GetType("System.String"), System.Type.GetType("System.Int32"),
                                            System.Type.GetType("System.String"), System.Type.GetType("System.String"), System.Type.GetType("System.String"), System.Type.GetType("System.Double"),
                                            System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"),
                                            System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"),
                                            System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"),
                                            System.Type.GetType("System.DateTime"), System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"),
                                             System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32")}}



        Dim oXmlDoc As New System.Xml.XmlDocument
        Dim dsQuote As DataSet

        Try
            dsQuote = Utilities.ArrayToDataSet(vResultArray, arrColHeaders, "BaseGetInstalmentQuotesResponseTypeQuotes")
            If GetInstalmentQuotesRequest.WCFSecurityToken = "" Then
                oXmlDoc.LoadXml(dsQuote.GetXml)
            End If

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "Failed to convert Instalment Quotes to xml", ex.Message)
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            Return oResponse
        End Try
        If GetInstalmentQuotesRequest.WCFSecurityToken = "" Then
            oResponse.Quotes = oXmlDoc.DocumentElement
        End If
        oResponse.ResultData = dsQuote

        Return oResponse

    End Function

    Public Function GetPoliciesInRenewal(ByVal Request As BaseGetPoliciesInRenewalRequestType) As BaseGetPoliciesInRenewalResponseType

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness
        Dim oResponse As BaseImplementationTypes.BaseGetPoliciesInRenewalResponseType
        Dim nTypeOfPackage As enumTypeOfPackage

        ' determine the type of package and thus the type of response
        If Request.GetType Is GetType(SAMForInsuranceImplementationTypes.GetPoliciesInRenewalRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.GetPoliciesInRenewalResponseType
        ElseIf Request.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPoliciesInRenewalRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetPoliciesInRenewalResponseType
        Else
            oResponse = New BaseImplementationTypes.BaseGetPoliciesInRenewalResponseType
        End If

        '*********************
        ' STRUCTURE VALIDATION 
        '*********************

        ' validate the mandatory structure data
        Request.Validate(CType(oSAMErrorCollection, Object))

        ' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()

        '*********************
        ' DATA VALIDATION 
        '*********************

        ' validate the mandatory structure data
        GetPoliciesInRenewalValidateData(oCoreBusiness, Request, oSAMErrorCollection)

        ' if there were any errors throw an exception
        oSAMErrorCollection.CheckForErrors()

        'Prakash: look up validation for product code if provided
        If Not String.IsNullOrEmpty(Request.ProductCode) Then
            Request.ProductKey = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Product, Request.ProductCode, "ProductCode", oSAMErrorCollection)
            oSAMErrorCollection.CheckForErrors()
        End If
        '*********************
        ' Get Required Information 
        '*********************

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                             _SiriusUser.Username, _SiriusUser.SourceID,
                                              _SiriusUser.LanguageID,
                                                  SiriusUserDefaults.AppName)

            Dim ds As DataSet = GetPoliciesInRenewalDataset(con, Request)

            ds.DataSetName = "BaseGetPoliciesInRenewalResponseTypePolicies"
            If Request.WCFSecurityToken = "" Then
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.LoadXml(ds.GetXml)
                oResponse.Policies = xmlDoc.DocumentElement
            End If
            oResponse.ResultData = ds

        End Using

        Return oResponse

    End Function

    Private Sub GetPoliciesInRenewalValidateData(
ByVal business As CoreBusiness,
ByRef getPoliciesInRenewalRequest As BaseGetPoliciesInRenewalRequestType,
ByRef samErrorCollection As SAMErrorCollection)
        'for getting direct business, agent key is not required, also partykey is not mandatory

        '***************************
        ''' validate standard lookup data
        '***************************

        GetPoliciesInRenewalValidateDataStandardLookup(business, getPoliciesInRenewalRequest, samErrorCollection)

    End Sub

    Private Sub GetPoliciesInRenewalValidateDataStandardLookup(
    ByVal business As CoreBusiness,
    ByRef request As BaseGetPoliciesInRenewalRequestType,
    ByRef samErrorCollection As SAMErrorCollection)

        '*********************
        'mandatory
        '*********************

        ' branch code
        If request.BranchCode <> "0" Then
            request.SourceId = business.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, request.BranchCode, "BranchCode", samErrorCollection)
        End If
    End Sub

    Public Function RunDefaultRulesAdd(ByVal RunDefaultRulesAddInput As BaseRunDefaultRulesAddRequestType) As BaseRunDefaultRulesAddResponseType

        Const ACMethodName As String = "RunDefaultRulesAdd"

        Dim oBusiness As New CoreBusiness(_SiriusUser)
        Dim oRunDefaultRulesAddOut As New BaseImplementationTypes.BaseRunDefaultRulesAddResponseType
        Dim oDefaultRulesIn As New DefaultRulesInput
        Dim oDefaultRulesOut As DefaultRulesOutput = Nothing
        Dim STSError As New STSErrorPublisher
        Dim oError As New SAMErrorCollection
        Dim iSourceId As Int32
        Dim iScreenId As Int32
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.RunDefaultRulesAddRequestType = Nothing
        Dim sChildOIKey As String = ""

        If RunDefaultRulesAddInput.GetType Is GetType(AnonymousImplementationTypes.RunDefaultRulesAddRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AnonymousPackage
            oRunDefaultRulesAddOut = New AnonymousImplementationTypes.RunDefaultRulesAddResponseType
        ElseIf RunDefaultRulesAddInput.GetType Is GetType(AgentImplementationTypes.RunDefaultRulesAddRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oRunDefaultRulesAddOut = New AgentImplementationTypes.RunDefaultRulesAddResponseType
        ElseIf RunDefaultRulesAddInput.GetType Is GetType(BaseImplementationTypes.BaseRunDefaultRulesAddRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oRunDefaultRulesAddOut = New BaseImplementationTypes.BaseRunDefaultRulesAddResponseType
        ElseIf RunDefaultRulesAddInput.GetType Is GetType(CustomerImplementationTypes.RunDefaultRulesAddRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oRunDefaultRulesAddOut = New CustomerImplementationTypes.RunDefaultRulesAddResponseType
        ElseIf RunDefaultRulesAddInput.GetType Is GetType(SAMForInsuranceImplementationTypes.RunDefaultRulesAddRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oRunDefaultRulesAddOut = New SAMForInsuranceImplementationTypes.RunDefaultRulesAddResponseType
        ElseIf RunDefaultRulesAddInput.GetType Is GetType(SAMForBrokingImplementationTypes.RunDefaultRulesAddRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oRunDefaultRulesAddOut = New SAMForBrokingImplementationTypes.RunDefaultRulesAddResponseType
            samForBrokingRequest = DirectCast(RunDefaultRulesAddInput, SAMForBrokingImplementationTypes.RunDefaultRulesAddRequestType)
        ElseIf RunDefaultRulesAddInput.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.RunDefaultRulesAddRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oRunDefaultRulesAddOut = New SAMForInsuranceV2ImplementationTypes.RunDefaultRulesAddResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        If RunDefaultRulesAddInput.BranchCode = "" Then
            ' BranchCode not set
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If RunDefaultRulesAddInput.ScreenCode = "" Then
            ' ScreenCode not set
            STSError.AddInvalidField("ScreenCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "ScreenCode"), "")
        End If

        If RunDefaultRulesAddInput.XMLDataSet = "" Then
            ' XMLDataSet not set
            STSError.AddInvalidField("XMLDataSet", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "XMLDataSet"), "")
        End If

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oRunDefaultRulesAddOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InputValidation", True)
            ' Return the errors
            Return oRunDefaultRulesAddOut
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oRunDefaultRulesAddOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oRunDefaultRulesAddOut
        End If

        iSourceId% = 1
        ' Convert branch code to ID
        Try
            iSourceId% = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", RunDefaultRulesAddInput.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), RunDefaultRulesAddInput.BranchCode)
        End Try

        ' Convert screen code to ID
        Try
            iScreenId% = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "gis_screen", RunDefaultRulesAddInput.ScreenCode)
        Catch ex As Exception
            STSError.AddInvalidField("ScreenCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "ScreenCode"), RunDefaultRulesAddInput.ScreenCode)
        End Try

        If STSError.HasErrors Then
            STSError.SetContext(oRunDefaultRulesAddOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oRunDefaultRulesAddOut
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                If oBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Agent Key validation failed", "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oRunDefaultRulesAddOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Key Validation", True)
                    Return oRunDefaultRulesAddOut
                End If
            End If

            If oBusiness.AgentSecurityCheck(samForBrokingRequest.UserName, iSourceId, PMEEntityType.Source) = False Then
                STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security Check Failed", samForBrokingRequest.UserName & " does not have permission to access source " & iSourceId)
                STSError.SetContext(oRunDefaultRulesAddOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "RunDefaultRulesAdd", True)
                Return oRunDefaultRulesAddOut
            End If
        End If

        ' Set up Data Model Code

        If SAMFunc.NothingToString(RunDefaultRulesAddInput.DataModelCode) = "" Then
            ' Get the DataModelCode from XML if not passed
            'Dim xmlDoc As New System.Xml.XmlDocument
            'xmlDoc.LoadXml(RunDefaultRulesAddInput.XMLDataSet)
            'oDefaultRulesIn.DataModelCode = xmlDoc.SelectSingleNode("DATA_SET") _
            '    .Attributes("DataModelCode").Value
            'sChildOIKey = xmlDoc.SelectSingleNode("DATA_SET").Attributes("NextOINumber").Value


            Dim settings As New XmlReaderSettings
            settings.ProhibitDtd = False

            Using reader As XmlReader = XmlReader.Create(New System.IO.StringReader(RunDefaultRulesAddInput.XMLDataSet), settings)
                reader.ReadToFollowing("DATA_SET")
                oDefaultRulesIn.DataModelCode = reader.GetAttribute("DataModelCode").ToUpper
                sChildOIKey = reader.GetAttribute("NextOINumber").ToUpper
            End Using

        Else
            oDefaultRulesIn.DataModelCode = RunDefaultRulesAddInput.DataModelCode
        End If

        oDefaultRulesIn.BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
        oDefaultRulesIn.EffectiveDate = Today
        'oDefaultRulesIn.GISSchemeID
        'oDefaultRulesIn.RiskGroupID
        oDefaultRulesIn.RiskScreenId = iScreenId%
        oDefaultRulesIn.XMLDataset = RunDefaultRulesAddInput.XMLDataSet
        Dim oAdditionalDataArray() As AdditionalData = Nothing
        If sChildOIKey <> "" Then
            Array.Resize(oAdditionalDataArray, 2)
        Else
            Array.Resize(oAdditionalDataArray, 1)
        End If
        Dim oAdditionalData As New AdditionalData
        oAdditionalData = New AdditionalData
        oAdditionalData.Name = "SKIPSAVE"
        oAdditionalData.Value = RunDefaultRulesAddInput.SkipSaveToDB

        oAdditionalDataArray(0) = oAdditionalData
        If sChildOIKey <> "" Then
            Dim oAdditionalData_OIKEY As New AdditionalData
            oAdditionalData_OIKEY = New AdditionalData

            oAdditionalData_OIKEY.Name = "CHILD_OIKEY"
            oAdditionalData_OIKEY.Value = "OI" & sChildOIKey

            oAdditionalDataArray(1) = oAdditionalData_OIKEY

        End If
        oDefaultRulesIn.AdditionalDataArray = oAdditionalDataArray
        Try
            If RunDefaultRulesAddInput.SkipSaveToDB Then
                ClearRiskOutput(oDefaultRulesIn.DataModelCode, RunDefaultRulesAddInput.XMLDataSet)
            End If
            oDefaultRulesOut = oBusiness.RunDefaultRulesAdd(oDefaultRulesIn)
        Catch ex As ApplicationException
            Dim STSErrorEx As New STSErrorPublisher("Failed to save run default Rules (Add).", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "RunDefaultRulesAdd", True)
        End Try

        oRunDefaultRulesAddOut.XMLDataSet = oDefaultRulesOut.XMLDataset
        Dim iScreen_id, iParentScreenid As Integer
        Dim iCount As Integer = 0
        Dim iCNT As Integer = 0
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            iParentScreenid = Convert.ToInt32(GetAndValidateSpecifiedTableCode(con, "gis_screen", "parent_id", "gis_screen_id", iScreenId%.ToString, oError, "parent_id"))
            iScreen_id = iParentScreenid
            While iParentScreenid <> 0
                iParentScreenid = Convert.ToInt32(GetAndValidateSpecifiedTableCode(con, "gis_screen", "parent_id", "gis_screen_id", iScreen_id.ToString, oError, "parent_id"))
                If iParentScreenid <> 0 Then
                    iScreen_id = iParentScreenid
                End If
            End While
            oRunDefaultRulesAddOut.RiskTypeID = GetAndValidateSpecifiedTableCode(con, "risk_type", "risk_type_id", "gis_screen_id", iScreen_id.ToString, oError, "risk_type_id")
        End Using
        oRunDefaultRulesAddOut.BranchID = iSourceId
        If RunDefaultRulesAddInput.SkipSaveToDB = False Then
            'Save Output
            Dim oSaveToDBIn As New SaveToDBIn
            Dim oSaveToDBOut As SaveToDBOut = Nothing
            oSaveToDBIn.BusinessTypeCode = oDefaultRulesIn.BusinessTypeCode
            oSaveToDBIn.DataModelCode = oDefaultRulesIn.DataModelCode
            oSaveToDBIn.XMLDataset = oDefaultRulesOut.XMLDataset
            Try
                oSaveToDBOut = oBusiness.SaveToDB(oSaveToDBIn)
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher("Failed to save Data To Database.", ex)
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "RunDefaultRulesAdd", True)
            End Try
            oRunDefaultRulesAddOut.XMLDataSet = oSaveToDBOut.XMLDataset
        End If
        'TypeCast the response Back.
        If nTypeOfPackage = enumTypeOfPackage.AnonymousPackage Then
            Dim oResponseAnon As New AnonymousImplementationTypes.RunDefaultRulesAddResponseType
            oResponseAnon = DirectCast(oRunDefaultRulesAddOut, AnonymousImplementationTypes.RunDefaultRulesAddResponseType)
            oRunDefaultRulesAddOut = oResponseAnon
        ElseIf nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            Dim oResponseAgent As New AgentImplementationTypes.RunDefaultRulesAddResponseType
            oResponseAgent = DirectCast(oRunDefaultRulesAddOut, AgentImplementationTypes.RunDefaultRulesAddResponseType)
            oRunDefaultRulesAddOut = oResponseAgent
        ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Then
            Dim oResponseAgent As New SAMForInsuranceImplementationTypes.RunDefaultRulesAddResponseType
            oResponseAgent = DirectCast(oRunDefaultRulesAddOut, SAMForInsuranceImplementationTypes.RunDefaultRulesAddResponseType)
            oRunDefaultRulesAddOut = oResponseAgent
        ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
            Dim oResponseAgent As New SAMForInsuranceV2ImplementationTypes.RunDefaultRulesAddResponseType
            oResponseAgent = DirectCast(oRunDefaultRulesAddOut, SAMForInsuranceV2ImplementationTypes.RunDefaultRulesAddResponseType)
            oRunDefaultRulesAddOut = oResponseAgent
        End If

        Return oRunDefaultRulesAddOut
    End Function

    Public Function RunDefaultRulesEdit(ByVal RunDefaultRulesEditInput As BaseRunDefaultRulesEditRequestType) As BaseRunDefaultRulesEditResponseType

        Const ACMethodName As String = "RunDefaultRulesEdit"

        Dim oBusiness As New CoreBusiness(_SiriusUser)
        Dim oRunDefaultRulesEditOut As New BaseImplementationTypes.BaseRunDefaultRulesEditResponseType
        Dim oDefaultRulesIn As New DefaultRulesInput
        Dim oDefaultRulesOut As DefaultRulesOutput = Nothing
        Dim STSError As New STSErrorPublisher
        Dim oErrors As New SAMErrorCollection
        Dim iSourceId As Int32
        Dim iScreenId As Int32
        Dim iClaimKey As Integer
        Dim iClaimPerilKey As Integer
        'Dim iClaimVersion As Integer
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.RunDefaultRulesEditRequestType = Nothing

        If RunDefaultRulesEditInput.GetType Is GetType(AnonymousImplementationTypes.RunDefaultRulesEditRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AnonymousPackage
            oRunDefaultRulesEditOut = New AnonymousImplementationTypes.RunDefaultRulesEditResponseType
        ElseIf RunDefaultRulesEditInput.GetType Is GetType(AgentImplementationTypes.RunDefaultRulesEditRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oRunDefaultRulesEditOut = New AgentImplementationTypes.RunDefaultRulesEditResponseType
        ElseIf RunDefaultRulesEditInput.GetType Is GetType(CustomerImplementationTypes.RunDefaultRulesEditRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oRunDefaultRulesEditOut = New CustomerImplementationTypes.RunDefaultRulesEditResponseType
        ElseIf RunDefaultRulesEditInput.GetType Is GetType(BaseImplementationTypes.BaseRunDefaultRulesEditRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oRunDefaultRulesEditOut = New BaseImplementationTypes.BaseRunDefaultRulesEditResponseType
        ElseIf RunDefaultRulesEditInput.GetType Is GetType(SAMForInsuranceImplementationTypes.RunDefaultRulesEditRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oRunDefaultRulesEditOut = New SAMForInsuranceImplementationTypes.RunDefaultRulesEditResponseType
        ElseIf RunDefaultRulesEditInput.GetType Is GetType(SAMForBrokingImplementationTypes.RunDefaultRulesEditRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oRunDefaultRulesEditOut = New SAMForBrokingImplementationTypes.RunDefaultRulesEditResponseType
            samForBrokingRequest = DirectCast(RunDefaultRulesEditInput, SAMForBrokingImplementationTypes.RunDefaultRulesEditRequestType)
        ElseIf RunDefaultRulesEditInput.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.RunDefaultRulesEditRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oRunDefaultRulesEditOut = New SAMForInsuranceV2ImplementationTypes.RunDefaultRulesEditResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim bClaimDetailsSpecified As Boolean = False

        ' if the claim id is specified 
        If RunDefaultRulesEditInput.ClaimKeySpecified Then

            ' but the claim peril id is not specified
            If Not RunDefaultRulesEditInput.ClaimPerilKeySpecified Then

                ' raise an error as the claim peril id must be specified when the claim id is
                STSError.AddInvalidField("ClaimPerilKey", CStr(STSErrorCodes.MandatoryInputMissing),
                    [String].Format(STSErrorPublisher.MandatoryInputInvalid, "ClaimPerilKey"),
                    RunDefaultRulesEditInput.ClaimPerilKey.ToString)

            Else
                bClaimDetailsSpecified = True
            End If

        End If

        If RunDefaultRulesEditInput.BranchCode = "" Then
            ' BranchCode not set
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        ' if the claim details are specified we dont need the screen code so ignore the test
        If RunDefaultRulesEditInput.ScreenCode = "" AndAlso Not bClaimDetailsSpecified Then
            ' ScreenCode not set
            STSError.AddInvalidField("ScreenCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "ScreenCode"), "")
        End If

        If SAMFunc.NothingToString(RunDefaultRulesEditInput.XMLDataSet) = "" Then
            ' XMLDataSet not set
            STSError.AddInvalidField("XMLDataset", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "XMLDataset"), "")
        End If

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oRunDefaultRulesEditOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InputValidation", True)
            ' Return the errors
            Return oRunDefaultRulesEditOut
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oRunDefaultRulesEditOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oRunDefaultRulesEditOut
        End If

        iSourceId% = 1
        ' Convert branch code to ID      
        If (Not String.IsNullOrEmpty(RunDefaultRulesEditInput.BranchCode)) Then
            iSourceId% = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
            PMLookupTable.Source, RunDefaultRulesEditInput.BranchCode, "BranchCode", oErrors)
        End If

        Dim sTransactionTypeCode As String = String.Empty

        ' if the claim details have been specified 
        If bClaimDetailsSpecified Then

            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

                'GetVersionId(baseClaimPerilId:=RunDefaultRulesEditInput.ClaimPerilKey, versionId:=iClaimVersion, con:=con)

                iClaimKey = RunDefaultRulesEditInput.ClaimKey
                iClaimPerilKey = RunDefaultRulesEditInput.ClaimPerilKey
                Dim baseClaimPerilKey As Integer = 0

                ' then get the screen id from the claim peril rather than the screen code
                GetClaimPerilDetails(con, RunDefaultRulesEditInput.ClaimPerilKey, iScreenId, sTransactionTypeCode, baseClaimPerilKey)

            End Using

        Else
            ' Convert screen code to ID
            Try
                iScreenId% = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "gis_screen", RunDefaultRulesEditInput.ScreenCode)
            Catch ex As Exception
                STSError.AddInvalidField("ScreenCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "ScreenCode"), RunDefaultRulesEditInput.ScreenCode)
            End Try
        End If

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oRunDefaultRulesEditOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InputValidation", True)
            ' Return the errors
            Return oRunDefaultRulesEditOut
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                If oBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Agent Key validation failed", "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oRunDefaultRulesEditOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Key Validation", True)
                    Return oRunDefaultRulesEditOut
                End If

                If oBusiness.AgentSecurityCheck(samForBrokingRequest.UserName, iSourceId, PMEEntityType.Source) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security Check Failed", samForBrokingRequest.UserName & " does not have permission to access source " & iSourceId)
                    STSError.SetContext(oRunDefaultRulesEditOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "RunDefaultRulesEdit", True)
                    Return oRunDefaultRulesEditOut
                End If
            End If
        End If

        ' if the claims details were specified then 
        ' add the additional data element for the CHILD_OIKEY.
        If bClaimDetailsSpecified Then
            Dim oAdditionalDataArray() As AdditionalData = Nothing
            Array.Resize(oAdditionalDataArray, 3)

            Dim oAdditionalData As New AdditionalData
            oAdditionalData.Name = "CHILD_OIKEY"
            oAdditionalData.Value = "CP" & iClaimPerilKey

            oAdditionalDataArray(0) = oAdditionalData

            oAdditionalData = New AdditionalData
            oAdditionalData.Name = "TRANSACTION_TYPE"
            oAdditionalData.Value = sTransactionTypeCode

            oAdditionalDataArray(1) = oAdditionalData
            oAdditionalData = New AdditionalData
            oAdditionalData.Name = "SkipSave"
            oAdditionalData.Value = RunDefaultRulesEditInput.SkipSaveToDB

            oAdditionalDataArray(2) = oAdditionalData

            oDefaultRulesIn.AdditionalDataArray = oAdditionalDataArray
        Else
            If Not String.IsNullOrEmpty(RunDefaultRulesEditInput.TransactionType) Then
                Dim oAdditionalDataArray() As AdditionalData = Nothing
                Array.Resize(oAdditionalDataArray, 2)
                Dim oAdditionalData As New AdditionalData
                oAdditionalData = New AdditionalData
                oAdditionalData.Name = "TRANSACTION_TYPE"
                oAdditionalData.Value = RunDefaultRulesEditInput.TransactionType
                oAdditionalDataArray(0) = oAdditionalData

                oAdditionalData = New AdditionalData
                oAdditionalData.Name = "SkipSave"
                oAdditionalData.Value = RunDefaultRulesEditInput.SkipSaveToDB

                oAdditionalDataArray(1) = oAdditionalData

                oDefaultRulesIn.AdditionalDataArray = oAdditionalDataArray
            End If
        End If

        oDefaultRulesIn.BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
        Dim xmlDoc As New System.Xml.XmlDocument
        xmlDoc.LoadXml(RunDefaultRulesEditInput.XMLDataSet)
        oDefaultRulesIn.DataModelCode = xmlDoc.SelectSingleNode("DATA_SET").Attributes("DataModelCode").Value
        oDefaultRulesIn.EffectiveDate = Date.Today
        oDefaultRulesIn.RiskScreenId = iScreenId%
        oDefaultRulesIn.XMLDataset = RunDefaultRulesEditInput.XMLDataSet

        Try
            If RunDefaultRulesEditInput.SkipSaveToDB Then
                ClearRiskOutput(oDefaultRulesIn.DataModelCode, RunDefaultRulesEditInput.XMLDataSet)
            End If
            oDefaultRulesOut = oBusiness.RunDefaultRulesEdit(oDefaultRulesIn)
        Catch ex As ApplicationException
            Dim STSErrorEx As New STSErrorPublisher("Failed to run default rules (edit).", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "RunDefaultRulesEdit", True)
        End Try

        oRunDefaultRulesEditOut.XMLDataSet = oDefaultRulesOut.XMLDataset

        ' only save the details if this is not a claim peril
        If Not bClaimDetailsSpecified Then
            If RunDefaultRulesEditInput.SkipSaveToDB = False Then
                'Save Output
                Dim oSaveToDBIn As New SaveToDBIn
                Dim oSaveToDBOut As SaveToDBOut = Nothing
                oSaveToDBIn.BusinessTypeCode = oDefaultRulesIn.BusinessTypeCode
                oSaveToDBIn.DataModelCode = oDefaultRulesIn.DataModelCode
                oSaveToDBIn.XMLDataset = oDefaultRulesOut.XMLDataset

                Try
                    oSaveToDBOut = oBusiness.SaveToDB(oSaveToDBIn)
                Catch ex As Exception
                    Dim STSErrorEx As New STSErrorPublisher("Failed to save Data To Database.", ex)
                    STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "RunDefaultRulesEdit", True)
                End Try

                oRunDefaultRulesEditOut.XMLDataSet = oSaveToDBOut.XMLDataset
            End If
            'TypeCast the response Back.
            If nTypeOfPackage = enumTypeOfPackage.AnonymousPackage Then
                Dim oResponseAnon As New AnonymousImplementationTypes.RunDefaultRulesEditResponseType
                oResponseAnon = DirectCast(oRunDefaultRulesEditOut, AnonymousImplementationTypes.RunDefaultRulesEditResponseType)
                oRunDefaultRulesEditOut = oResponseAnon
            ElseIf nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
                Dim oResponseAgent As New AgentImplementationTypes.RunDefaultRulesEditResponseType
                oResponseAgent = DirectCast(oRunDefaultRulesEditOut, AgentImplementationTypes.RunDefaultRulesEditResponseType)
                oRunDefaultRulesEditOut = oResponseAgent
            ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Then
                Dim oResponseAgent As New SAMForInsuranceImplementationTypes.RunDefaultRulesEditResponseType
                oResponseAgent = DirectCast(oRunDefaultRulesEditOut, SAMForInsuranceImplementationTypes.RunDefaultRulesEditResponseType)
                oRunDefaultRulesEditOut = oResponseAgent
            ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
                Dim oResponseAgent As New SAMForInsuranceV2ImplementationTypes.RunDefaultRulesEditResponseType
                oResponseAgent = DirectCast(oRunDefaultRulesEditOut, SAMForInsuranceV2ImplementationTypes.RunDefaultRulesEditResponseType)
                oRunDefaultRulesEditOut = oResponseAgent
            End If
        End If

        Return oRunDefaultRulesEditOut

    End Function

    Public Function RunValidationRules(ByVal RunValidationRulesInput As BaseRunValidationRulesRequestType) As BaseRunValidationRulesResponseType

        Const ACMethodName As String = "RunValidationRules"

        Dim oBusiness As New CoreBusiness(_SiriusUser)
        Dim oRunValidationRulesOut As New BaseImplementationTypes.BaseRunValidationRulesResponseType
        Dim oValidationRulesIn As New ValidationRulesInput
        Dim oValidationRulesOut As ValidationRulesOutput = Nothing
        Dim STSError As New STSErrorPublisher
        Dim oErrors As New SAMErrorCollection
        Dim iSourceId As Int32
        Dim iScreenId As Int32
        Dim iClaimKey As Integer
        Dim iClaimPerilKey As Integer
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iTransactionTypeId As Integer = 0

        If RunValidationRulesInput.GetType Is GetType(BaseImplementationTypes.BaseRunValidationRulesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oRunValidationRulesOut = New BaseImplementationTypes.BaseRunValidationRulesResponseType
        ElseIf RunValidationRulesInput.GetType Is GetType(SAMForInsuranceImplementationTypes.RunValidationRulesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oRunValidationRulesOut = New SAMForInsuranceImplementationTypes.RunValidationRulesResponseType
        ElseIf RunValidationRulesInput.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.RunValidationRulesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oRunValidationRulesOut = New SAMForInsuranceV2ImplementationTypes.RunValidationRulesResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim bClaimDetailsSpecified As Boolean = False

        ' if the claim id is specified 
        If RunValidationRulesInput.ClaimKeySpecified Then
            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CLM_Get_Claim_Transaction_Type")
                    cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = RunValidationRulesInput.ClaimKey
                    iTransactionTypeId = Cast.ToInt32(con.ExecuteScalar(cmd), 0)
                End Using
            End Using

            ' but the claim peril id is not specified
            If Not RunValidationRulesInput.ClaimPerilKeySpecified Then
                bClaimDetailsSpecified = False
                oValidationRulesIn.ClaimTransactiontypeId = iTransactionTypeId
            Else
                bClaimDetailsSpecified = True
            End If

        End If

        If RunValidationRulesInput.BranchCode = "" Then
            ' BranchCode not set
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        ' if the claim details are specified we dont need the screen code so ignore the test
        If RunValidationRulesInput.ScreenCode = "" AndAlso Not bClaimDetailsSpecified Then
            ' ScreenCode not set
            STSError.AddInvalidField("ScreenCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "ScreenCode"), "")
        End If

        If SAMFunc.NothingToString(RunValidationRulesInput.XMLDataSet) = "" Then
            ' XMLDataSet not set
            STSError.AddInvalidField("XMLDataset", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "XMLDataset"), "")
        End If

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oRunValidationRulesOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InputValidation", True)
            ' Return the errors
            Return oRunValidationRulesOut
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oRunValidationRulesOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oRunValidationRulesOut
        End If

        iSourceId% = 1
        ' Convert branch code to ID   
       
            If (Not String.IsNullOrEmpty(RunValidationRulesInput.BranchCode)) Then
                iSourceId% = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
            PMLookupTable.Source, RunValidationRulesInput.BranchCode, "BranchCode", oErrors)
            End If
        

        Dim sTransactionTypeCode As String = String.Empty

        ' if the claim details have been specified 
        If bClaimDetailsSpecified Then

            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

                'GetVersionId(baseClaimPerilId:=RunValidationRulesInput.BaseClaimPerilKey, versionId:=iClaimVersion, con:=con)

                iClaimKey = RunValidationRulesInput.ClaimKey
                iClaimPerilKey = RunValidationRulesInput.ClaimPerilKey
                Dim baseClaimPerilKey As Integer = 0

                ' then get the screen id / transaction type code from the claim peril rather than the screen code
                GetClaimPerilDetails(con, RunValidationRulesInput.ClaimPerilKey, iScreenId, sTransactionTypeCode, baseClaimPerilKey)

            End Using

        Else
            ' Convert screen code to ID
            Try
                iScreenId% = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "gis_screen", RunValidationRulesInput.ScreenCode)
            Catch ex As Exception
                STSError.AddInvalidField("ScreenCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "ScreenCode"), RunValidationRulesInput.ScreenCode)
            End Try
        End If

        Try
            Dim xmlDocTest As New System.Xml.XmlDocument
            xmlDocTest.LoadXml(RunValidationRulesInput.XMLDataSet)
        Catch ex As Exception
            STSError.AddInvalidField("XmlDataSet", CStr(STSErrorCodes.XmldatasetBadlyFormed), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "XmlDataSet"), RunValidationRulesInput.XMLDataSet)
        End Try

        If STSError.HasErrors Then
            STSError.SetContext(oRunValidationRulesOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oRunValidationRulesOut
        End If

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oRunValidationRulesOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InputValidation", True)
            ' Return the errors
            Return oRunValidationRulesOut
        End If

        ' if the claims details were specified then 
        ' add the additional data element for the CHILD_OIKEY.
        If bClaimDetailsSpecified Then
            Dim oAdditionalDataArray() As AdditionalData = Nothing
            Array.Resize(oAdditionalDataArray, 3)

            Dim oAdditionalData As New AdditionalData
            oAdditionalData.Name = "CHILD_OIKEY"
            oAdditionalData.Value = "CP" & iClaimPerilKey

            oAdditionalDataArray(0) = oAdditionalData

            oAdditionalData = New AdditionalData
            oAdditionalData.Name = "TRANSACTION_TYPE"
            oAdditionalData.Value = sTransactionTypeCode

            oAdditionalDataArray(1) = oAdditionalData
            oAdditionalData = New AdditionalData
            oAdditionalData.Name = "SkipSave"
            oAdditionalData.Value = RunValidationRulesInput.SkipSaveToDB

            oAdditionalDataArray(2) = oAdditionalData

            oValidationRulesIn.AdditionalDataArray = oAdditionalDataArray
            oValidationRulesIn.isClaimValidation = True
        Else
            If RunValidationRulesInput.ClaimKeySpecified Then
                oValidationRulesIn.isClaimValidation = True
            End If

            If Not String.IsNullOrEmpty(RunValidationRulesInput.TransactionType) Then
                Dim oAdditionalDataArray() As AdditionalData = Nothing
                Array.Resize(oAdditionalDataArray, 2)
                Dim oAdditionalData As New AdditionalData
                oAdditionalData = New AdditionalData
                oAdditionalData.Name = "TRANSACTION_TYPE"
                oAdditionalData.Value = RunValidationRulesInput.TransactionType
                oAdditionalDataArray(0) = oAdditionalData

                oAdditionalData = New AdditionalData
                oAdditionalData.Name = "SKIPSAVE"
                oAdditionalData.Value = RunValidationRulesInput.SkipSaveToDB

                oAdditionalDataArray(1) = oAdditionalData

                oValidationRulesIn.AdditionalDataArray = oAdditionalDataArray
            End If

        End If

        oValidationRulesIn.BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
        Dim xmlDoc As New System.Xml.XmlDocument
        xmlDoc.LoadXml(RunValidationRulesInput.XMLDataSet)
        oValidationRulesIn.DataModelCode = xmlDoc.SelectSingleNode("DATA_SET").Attributes("DataModelCode").Value
        oValidationRulesIn.EffectiveDate = Date.Today
        oValidationRulesIn.RiskScreenId = iScreenId%
        If Not RunValidationRulesInput.SkipSaveToDB Then
            UpdateXmlDataSet(RunValidationRulesInput.XMLDataSet, 3, oValidationRulesIn.DataModelCode)
        End If
        oValidationRulesIn.XMLDataset = RunValidationRulesInput.XMLDataSet

        Try
            If RunValidationRulesInput.SkipSaveToDB Then
                ClearRiskOutput(oValidationRulesIn.DataModelCode, oValidationRulesIn.XMLDataset, True)
            End If
            oValidationRulesOut = oBusiness.RunValidationRules(oValidationRulesIn, _SiriusUser)
        Catch ex As ApplicationException
            Dim STSErrorEx As New STSErrorPublisher("Failed to run Validation rules (edit).", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "RunValidationRules", True)
        End Try

        oRunValidationRulesOut.XMLDataset = oValidationRulesOut.XMLDataset

        ' only save the details if this is not a claim peril
        'If Not bClaimDetailsSpecified Then
        If RunValidationRulesInput.SkipSaveToDB = False Then
            'Save Output
            Dim oSaveToDBIn As New SaveToDBIn
            Dim oSaveToDBOut As SaveToDBOut = Nothing
            oSaveToDBIn.BusinessTypeCode = oValidationRulesIn.BusinessTypeCode
            oSaveToDBIn.DataModelCode = oValidationRulesIn.DataModelCode
            oSaveToDBIn.XMLDataset = oValidationRulesOut.XMLDataset

            Try
                oSaveToDBOut = oBusiness.SaveToDB(oSaveToDBIn)
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher("Failed to save Data To Database.", ex)
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "RunValidationRules", True)
            End Try

            oRunValidationRulesOut.XMLDataset = oSaveToDBOut.XMLDataset
        End If
        'TypeCast the response Back.
        If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Then
            Dim oResponseAgent As New SAMForInsuranceImplementationTypes.RunValidationRulesResponseType
            oResponseAgent = DirectCast(oRunValidationRulesOut, SAMForInsuranceImplementationTypes.RunValidationRulesResponseType)
            oRunValidationRulesOut = oResponseAgent
        ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
            Dim oResponseAgent As New SAMForInsuranceV2ImplementationTypes.RunValidationRulesResponseType
            oResponseAgent = DirectCast(oRunValidationRulesOut, SAMForInsuranceV2ImplementationTypes.RunValidationRulesResponseType)
            oRunValidationRulesOut = oResponseAgent
        End If

        Return oRunValidationRulesOut

    End Function
    Public Sub UpdateXmlDataSet(ByRef r_sXMLDataSet As String, ByVal USValue As Integer, ByVal DataModelCode As String)
        Dim Doc As New XmlDocument
        Using srDataset As New System.IO.StringReader(r_sXMLDataSet)
            Using xmlTR As New XmlTextReader(srDataset)

                Doc.Load(xmlTR)
                xmlTR.Close()
            End Using
            srDataset.Close()
        End Using

        Dim oNodes As XmlNodeList = Doc.SelectNodes("//" & DataModelCode & "_OUTPUT[@REFER_REASON]")

        Dim oNode As XmlNode

        For Each oNode In oNodes
            If oNode IsNot Nothing AndAlso oNode.Attributes("US").Value <> "3" Then
                oNode.Attributes("US").Value = "3"
            End If
        Next

        oNodes = Doc.SelectNodes("//" & DataModelCode & "_OUTPUT[@DECLINE_REASON]")
        For Each oNode In oNodes
            If oNode IsNot Nothing AndAlso oNode.Attributes("US").Value <> "3" Then
                oNode.Attributes("US").Value = "3"
            End If
        Next

        Dim tempswContent As New System.IO.StringWriter
        Dim tempxmlwContent As New XmlTextWriter(tempswContent)

        Doc.WriteTo(tempxmlwContent)

        tempxmlwContent.Close()
        tempswContent.Close()
        r_sXMLDataSet = tempswContent.ToString()

    End Sub

    ' ***************************************************************** '
    ' Name: UpdateRisk
    '
    ' Description: 
    '
    ' ***************************************************************** '
    Public Overloads Function UpdateQuote(ByVal UpdateQuoteRequest As BaseUpdateQuoteRequestType) As BaseUpdateQuoteResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As BaseUpdateQuoteResponseType

            oResponse = UpdateQuote(con, UpdateQuoteRequest)

            Return oResponse

        End Using

    End Function

    'TODO Handle Insured Parties
    Public Overloads Function UpdateQuote(ByRef con As SiriusConnection, ByVal UpdateQuoteRequest As BaseUpdateQuoteRequestType) As BaseUpdateQuoteResponseType

        Dim oResponse As BaseUpdateQuoteResponseType
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iRet As Int32
        Dim vInsuredParties As System.Array = Nothing
        Dim oGIS As bGIS.STS = Nothing
        Dim iCurrencyId As Integer = -1
        Dim iAnalysisId As Integer

        Const ACMethodName As String = "UpdateQuote"

        Dim STSError As New STSErrorPublisher
        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.UpdateQuoteRequestType = Nothing

        Dim oAgentRequest As AgentImplementationTypes.UpdateQuoteRequestType = Nothing
        If UpdateQuoteRequest.GetType Is GetType(AgentImplementationTypes.UpdateQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.UpdateQuoteResponseType
            oAgentRequest = DirectCast(UpdateQuoteRequest, AgentImplementationTypes.UpdateQuoteRequestType)
        ElseIf UpdateQuoteRequest.GetType Is GetType(CustomerImplementationTypes.UpdateQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oResponse = New CustomerImplementationTypes.UpdateQuoteResponseType
        ElseIf UpdateQuoteRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.UpdateQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.UpdateQuoteResponseType
        ElseIf UpdateQuoteRequest.GetType Is GetType(BaseImplementationTypes.BaseUpdateQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oResponse = New BaseImplementationTypes.BaseUpdateQuoteResponseType
        ElseIf UpdateQuoteRequest.GetType Is GetType(SAMForBrokingImplementationTypes.UpdateQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oResponse = New SAMForBrokingImplementationTypes.UpdateQuoteResponseType
            samForBrokingRequest = DirectCast(UpdateQuoteRequest, SAMForBrokingImplementationTypes.UpdateQuoteRequestType)
        ElseIf UpdateQuoteRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.UpdateQuoteResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                If oCoreBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Agent Key validation failed", "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Key Validation", True)
                    Return oResponse
                End If

                If oCoreBusiness.AgentSecurityCheck(samForBrokingRequest.UserName, samForBrokingRequest.InsuranceFileKey, PMEEntityType.InsuranceFile) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security Check Failed", samForBrokingRequest.UserName & " does not have permission to access insurancefile " & samForBrokingRequest.InsuranceFileKey)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateQuote", True)
                    Return oResponse
                End If
            End If
        End If

        'Check Mandatory Fields
        If UpdateQuoteRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If
        'Commented since Description Field is OPTIONAL
        'If UpdateQuoteRequest.Description = "" Then
        '    STSError.AddInvalidField("Description", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Description"), "")

        If UpdateQuoteRequest.InsuranceFileKey = 0 Then
            STSError.AddInvalidField("InsuranceFileKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFileKey"), "")
        End If

        If UpdateQuoteRequest.InsuranceFolderKey = 0 Then
            STSError.AddInvalidField("InsuranceFolderKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFolderKey"), "")
        End If

        If UpdateQuoteRequest.CoverEndDate = New Date Then
            STSError.AddInvalidField("CoverEndDate", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "CoverEndDate"), "")
        End If

        If UpdateQuoteRequest.CoverStartDate = New Date Then
            STSError.AddInvalidField("CoverStartDate", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "CoverStartDate"), "")
        End If

        If UpdateQuoteRequest.QuoteTimeStamp Is Nothing Then
            STSError.AddInvalidField("QuoteTimeStamp", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "QuoteTimeStamp"), "")
        End If

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InputValidation", True)
            ' Return the errors
            Return oResponse
        End If

        Dim iSourceID As Int32
        iSourceID% = 1
        ' Convert branch code to ID
        Try
            iSourceID% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", UpdateQuoteRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), UpdateQuoteRequest.BranchCode)
        End Try

        ' if the analysis code was passed in the request
        If Not String.IsNullOrEmpty(UpdateQuoteRequest.AnalysisCode) Then
            ' validate the analysis code is valid
            Try
                iAnalysisId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.AnalysisCode, UpdateQuoteRequest.AnalysisCode)
            Catch ex As Exception
                STSError.AddInvalidField("AnalysisCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "AnalysisCode"), UpdateQuoteRequest.AnalysisCode)
            End Try
        End If

        ' Lookup the Currency ID
        ' Check that the currency was passed in
        If SAMFunc.NothingToString(UpdateQuoteRequest.CurrencyCode) <> "" Then
            Try
                iCurrencyId% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Currency", UpdateQuoteRequest.CurrencyCode)
            Catch ex As Exception
                STSError.AddInvalidField("CurrencyCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "CurrencyCode"), UpdateQuoteRequest.CurrencyCode)
            End Try
        End If

        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oResponse
        End If

        ' Check the insurance folder cnt is valid
        If oCoreBusiness.CheckInsuranceFolder(UpdateQuoteRequest.InsuranceFolderKey) = False Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyRecordNotFound, "Insurance Folder validation failed", "The Insurance Folder record does not exist for key: " & UpdateQuoteRequest.InsuranceFolderKey)
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If

        ' Check the insurance file cnt is valid
        Dim iFileFolderCnt As Integer
        If oCoreBusiness.CheckInsuranceFile(UpdateQuoteRequest.InsuranceFileKey, iFileFolderCnt) = False Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyRecordNotFound, "Insurance File validation failed", "The Insurance File record does not exist for key: " & UpdateQuoteRequest.InsuranceFileKey)
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If
        If iFileFolderCnt <> UpdateQuoteRequest.InsuranceFolderKey Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyMismatch, "Insurance File validation failed", "The Insurance File's Folder does not match the passed InsuranceFolder")
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If

        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            If (oCoreBusiness.AgentSecurityCheck(oAgentRequest.UserName, oAgentRequest.InsuranceFileKey, PMEEntityType.InsuranceFile) = False) Then
                STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security check failed", oAgentRequest.UserName & " does not have permission to access policy " & oAgentRequest.InsuranceFileKey)
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateQuote", True)
                Return oResponse
            End If
        End If

        ' Error if start date is in the past Validation **Removed as per PN 43376**
        'If UpdateQuoteRequest.CoverStartDate < Today Then
        '    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.CoverStartDateIsInThePast, "Cover Start date is in the past", UpdateQuoteRequest.CoverStartDate.ToString)
        '    STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Cover Start date is in the past", True)
        '    Return oResponse

        ' Error if end date is before the start date
        If UpdateQuoteRequest.CoverEndDate < UpdateQuoteRequest.CoverStartDate Then
            Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.CoverEndDateIsBeforeCoverStartDate, "Cover End date is before Cover Start date", UpdateQuoteRequest.CoverEndDate.ToString)
            STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Cover End date is before Cover Start date", True)
            Return oResponse
        End If

        ' If we already have errors then return at this point
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InputValidation", True)
            ' Return the errors
            Return oResponse
        End If

        '***************************
        ''' validate Cover Note data
        '***************************

        Dim samErrorsCollection As New SAMErrorCollection
        If UpdateQuoteRequest.CoverNoteBookNumber = String.Empty And ((UpdateQuoteRequest.CoverNoteSheetNumberSpecified = True) And UpdateQuoteRequest.CoverNoteSheetNumber <> 0) Then
            ' Validation Error - Cover note book number not specified
            samErrorsCollection.AddBusinessRule(
                SAMConstants.SAMBusinessErrors.CoverNoteBookNumberNotSpecified,
                SAMConstants.SAMBusinessErrors.CoverNoteBookNumberNotSpecified.ToString)
        End If

        If UpdateQuoteRequest.CoverNoteBookNumber <> String.Empty And ((UpdateQuoteRequest.CoverNoteSheetNumberSpecified = False) Or UpdateQuoteRequest.CoverNoteSheetNumber = 0) Then
            ' Validation Error - Cover note sheet number not specified
            samErrorsCollection.AddBusinessRule(
                SAMConstants.SAMBusinessErrors.CoverNoteSheetNumberNotSpecified,
                SAMConstants.SAMBusinessErrors.CoverNoteSheetNumberNotSpecified.ToString)
        End If

        If UpdateQuoteRequest.CoverNoteBookNumber <> String.Empty And ((UpdateQuoteRequest.CoverNoteSheetNumberSpecified = True) And UpdateQuoteRequest.CoverNoteSheetNumber > 0) Then

            Dim validationResult As Integer = 0
            Dim sheetStatus As String = String.Empty
            Dim insuranceFileDetails As New BaseClaimType

            insuranceFileDetails.InsuranceFileKey = UpdateQuoteRequest.InsuranceFileKey
            GetInsuranceFileDetails(con, insuranceFileDetails)

            IsValidCoverNote(con, samErrorsCollection, 0, UpdateQuoteRequest.CoverNoteBookNumber, UpdateQuoteRequest.CoverNoteSheetNumber, insuranceFileDetails.SourceId, insuranceFileDetails.AgentCnt, insuranceFileDetails.ProductId, validationResult, sheetStatus)

        End If

        samErrorsCollection.CheckForErrors()

        ' Check that the timestamp matches and lock the Quote
        Dim AnyError As STSErrorType
        AnyError = oCoreBusiness.CheckTSAndLock(
            BranchCode:=UpdateQuoteRequest.BranchCode,
            Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
            LockValue:=UpdateQuoteRequest.InsuranceFolderKey,
            TStamp:=UpdateQuoteRequest.QuoteTimeStamp)
        ' Check for Errors
        If AnyError Is Nothing = False Then
            ' Either the timestamp didn't match or the record couldn't be locked for some reason, return the error.
            oResponse.STSError = AnyError
            Return oResponse
        End If

        Try
            oGIS = New bGIS.STS
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Failed to create bGIS.Application", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateQuote", True)
        End Try

        ' Initialise the GIS
        Try
            SAMFunc.InitialiseGISSTS(oGIS, _SiriusUser, UpdateQuoteRequest.BranchCode)
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Failed to initialise bGIS.Application", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateQuote", True)
        End Try

        ' Call the Method
        Try
            With UpdateQuoteRequest

                Dim renewalDate As Date
                Dim productDetail As ProductDetailsType = New ProductDetailsType(UpdateQuoteRequest.InsuranceFileKey)
                If productDetail.IsMidNightRenewal Then
                    ' default the renewal date to be the day after the policy has ended
                    renewalDate = Informations.DateAdd(DateInterval.Day, 1, UpdateQuoteRequest.CoverEndDate)
                Else
                    renewalDate = UpdateQuoteRequest.CoverEndDate
                End If

                Dim additionalKeys As String = String.Empty

                Const KeyValueSplitter As String = "****"
                Const KeySplitter As String = "||||"

                additionalKeys += "RENEWALDATE" + KeyValueSplitter + renewalDate.ToString
                additionalKeys += KeySplitter
                additionalKeys += "ALTERNATIVEREF" + KeyValueSplitter + UpdateQuoteRequest.AlternativeRef

                Dim additionalKeysObject As Object = CType(additionalKeys, Object)

                iRet = oGIS.UpdateQuoteWithAdditionalData(
                            v_lInsuranceFileCnt:=CType(.InsuranceFileKey, Integer),
                            v_dtCoverStart:=CType(.CoverStartDate, Date),
                            v_dtCoverEnd:=CType(.CoverEndDate, Date),
                            v_sDescription:=CType(.Description, String),
                            v_vInsuredParties:=vInsuredParties,
                            v_lCurrencyID:=CType(iCurrencyId%, Integer),
                            v_lAnalysisCodeId:=iAnalysisId,
                            v_blConsLeadAgntComm:= .ConsolidatedLeadAgentCommission,
                            v_blConsSubAgntComm:= .ConsolidatedSubAgentCommission, v_vAdditionalDataArray:=additionalKeysObject)
            End With

            If (iRet <> PMEReturnCode.PMTrue) Then
                Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "bGis.STS.UpdateQuote failed", "")
                STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                Return oResponse
            End If

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "bGis.STS.UpdateQuote failed", ex.Message)
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            Return oResponse
        End Try

        Try
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "Failed to terminate business", ex.Message)
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            Return oResponse
        End Try

        ' Update the Covernote details with the new values
        If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Or
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then

            AssignCoverNoteSheet(con, UpdateQuoteRequest.InsuranceFileKey, UpdateQuoteRequest.CoverNoteBookNumber, UpdateQuoteRequest.CoverNoteSheetNumber)

        End If

        ' Unlock and return the new timestamp
        AnyError = oCoreBusiness.UnlockAndGetTS(
            BranchCode:=UpdateQuoteRequest.BranchCode,
            Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
            LockValue:=UpdateQuoteRequest.InsuranceFolderKey,
            TStamp:=oResponse.QuoteTimeStamp)

        ' Check for Errors
        If AnyError Is Nothing = False Then
            ' Unable to unlock, return the error.
            oResponse.STSError = AnyError
            Return oResponse
        End If

        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            Dim oResponseAgent As New AgentImplementationTypes.UpdateQuoteResponseType
            oResponseAgent = DirectCast(oResponse, AgentImplementationTypes.UpdateQuoteResponseType)
            oResponse = oResponseAgent
        End If

        Return oResponse

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="QuoteRef"></param>
    ''' <param name="InsuranceFolderCnt"></param>
    ''' <param name="InsuranceFileCnt"></param>
    ''' <param name="RiskCnt"></param>
    ''' <param name="PolicyLinkID"></param>
    ''' <param name="GrossAmount"></param>
    ''' <param name="CommissionAmount"></param>
    ''' <param name="IPTAmount"></param>
    ''' <param name="oPolicy"></param>
    ''' <param name="TransactionType"></param>
    ''' <param name="sDocumentCommnet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetQuoteDetails(
   ByRef QuoteRef As String, ByRef InsuranceFolderCnt As Int32,
   ByRef InsuranceFileCnt As Int32, ByRef RiskCnt As Int32,
   ByRef PolicyLinkID As Int32, ByRef GrossAmount As Decimal,
   ByRef CommissionAmount As Decimal, ByRef IPTAmount As Decimal, Optional ByRef oPolicy As BaseImplementationTypes.BaseTransactResponseTypePolicy = Nothing,
   Optional ByVal TransactionType As String = Nothing, Optional ByRef sDocumentCommnet As String = Nothing) As Integer

        Dim core As New CoreBusiness

        Dim ConnectionString As String = SAMFunc.ConnectionString

        core = Nothing

        ' Dataset that will hold the returned results		
        Dim dr As DataSet = Nothing

        ' call SP using a SqlParameter array

        Dim SQL As New StringBuilder()
        If TransactionType = "MTA" Or TransactionType = "MTC" Then
            SQL.Append("declare @document_comment varchar(60) ")
            SQL.Append("select @document_comment=(select TOP 1 case when isnull(document_comment,'')<>''  ")
            SQL.Append("then document_comment end document_comment  from stats_folder sf where insurance_file_cnt= ")
            SQL.Append(InsuranceFileCnt)
            SQL.Append("  and  (sf.transaction_type_code='MTA' or sf.transaction_type_code='MTC')) ")
        End If
        SQL.Append("select ")

        SQL.Append("insf.insurance_ref, ")
        SQL.Append("insf.insurance_folder_cnt, ")
        SQL.Append("insf.insurance_file_cnt, ")
        SQL.Append("ifrl.risk_cnt, ")
        SQL.Append("gpl.gis_policy_link_id, ")
        SQL.Append("insf.net_premium + insf.tax_amount 'gross_amount', ")
        SQL.Append("insf.commission_amount, ")
        SQL.Append("insf.tax_amount ")

        If TransactionType = "MTA" Or TransactionType = "MTC" Then
            SQL.Append(" ,@document_comment document_comment ")
        End If

        SQL.Append("from insurance_file insf ")
        SQL.Append("inner join gis_policy_link gpl on gpl.insurance_file_cnt = insf.insurance_folder_cnt ")
        SQL.Append("inner join insurance_file_risk_link ifrl on ifrl.insurance_file_cnt = insf.insurance_file_cnt ")
        If InsuranceFileCnt = 0 Then
            SQL.Append("where insurance_ref = '")
            SQL.Append(QuoteRef)
            SQL.Append("'")
        Else
            SQL.Append("where insf.insurance_file_cnt = ")
            SQL.Append(InsuranceFileCnt)
        End If
        SQL.Append(" and ifrl.status_flag <> 'U'")

        Try

            ' BSJ April 09 - SQL Mixed Mode Compliance
            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

                Using cmd As SiriusCommand = SiriusCommand.FromText(SQL.ToString)
                    dr = con.ExecuteDataSet(cmd, "dr")
                End Using
            End Using

        Catch ex As Exception
            Dim MyError As New Exception("Error executing SQL " & SQL.ToString, ex)
            ExceptionManager.Publish(MyError)
            dr.Dispose()
            dr = Nothing
            Throw MyError
        End Try

        If dr.Tables(0).Rows.Count > 1 Then

            For count As Integer = 0 To dr.Tables(0).Rows.Count - 1
                With dr.Tables(0).Rows(count)

                    ReDim Preserve oPolicy.MultiplePolicies(count)
                    oPolicy.MultiplePolicies(count) = New BaseTransactResponseTypePolicyMultiplePolicies
                    oPolicy.MultiplePolicies(count).PolicyRef = CType(.Item("insurance_ref"), String).Trim

                    If QuoteRef = "" Then
                        QuoteRef = CType(.Item("insurance_ref"), String).Trim
                        InsuranceFolderCnt = CType(.Item("insurance_folder_cnt"), Int32)
                    End If

                    oPolicy.MultiplePolicies(count).PremiumDueGross = Cast.ToDecimal(.Item("gross_amount"), 0)
                    oPolicy.MultiplePolicies(count).TotalAnnualTax = Cast.ToDecimal(.Item("tax_amount"), 0)
                    oPolicy.MultiplePolicies(count).PremiumDueTax = Cast.ToDecimal(.Item("tax_amount"), 0)
                    oPolicy.MultiplePolicies(count).PremiumDueNet = Cast.ToDecimal(.Item("gross_amount"), 0) - Cast.ToDecimal(.Item("tax_amount"), 0)
                    oPolicy.MultiplePolicies(count).CommissionAmount = Cast.ToDecimal(.Item("commission_amount"), 0)
                    If TransactionType = "MTA" Or TransactionType = "MTC" Then
                        oPolicy.MultiplePolicies(count).DocumentComment = Cast.ToString(.Item("document_comment"), "").Trim
                        If sDocumentCommnet = "" Then
                            sDocumentCommnet = Cast.ToString(.Item("document_comment"), "").Trim
                        End If
                    End If

                End With

            Next
        End If
        If dr.Tables(0).Rows.Count > 0 Then
            With dr.Tables(0).Rows(0)
                QuoteRef = CType(.Item("insurance_ref"), String).Trim
                InsuranceFolderCnt = CType(.Item("insurance_folder_cnt"), Int32)
                InsuranceFileCnt = CType(.Item("insurance_file_cnt"), Int32)
                RiskCnt = CType(.Item("risk_cnt"), Int32)
                PolicyLinkID = CType(.Item("gis_policy_link_id"), Int32)
                If (.Item("gross_amount").GetType Is GetType(DBNull)) = False Then _
                    GrossAmount = Cast.ToDecimal(.Item("gross_amount"), 0)
                If (.Item("commission_amount").GetType Is GetType(DBNull)) = False Then _
                    CommissionAmount = Cast.ToDecimal(.Item("commission_amount"), 0)
                If (.Item("tax_amount").GetType Is GetType(DBNull)) = False Then _
                    IPTAmount = Cast.ToDecimal(.Item("tax_amount"), 0)
            End With
        Else

            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                                 _SiriusUser.Username, _SiriusUser.SourceID,
                                                 _SiriusUser.LanguageID,
                                                 SiriusUserDefaults.AppName)

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Insurance_Ref")
                    cmd.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = InsuranceFileCnt
                    dr = con.ExecuteDataSet(cmd, "dr")
                End Using
            End Using
            If dr IsNot Nothing AndAlso (dr.Tables(0).Rows.Count <> 0) Then
                QuoteRef = CType(dr.Tables(0).Rows(0).Item("insurance_ref"), String).Trim
                InsuranceFolderCnt = CType(dr.Tables(0).Rows(0).Item("insurance_folder_cnt"), Int32)
            End If
        End If
        ' Return the DataReader
        dr.Dispose()
        dr = Nothing

        Return 1

    End Function

    Public Function GetRatingDetails(ByVal GetRatingDetailsInput As BaseGetRatingDetailsRequestType) As BaseGetRatingDetailsResponseType

        Const ACMethodName As String = "GetRatingDetails"

        Dim iRet As System.Int32
        Dim oResponse As New BaseGetRatingDetailsResponseType
        Dim oAgentRequest As AgentImplementationTypes.GetRatingDetailsRequestType
        'Dim Utils As Utilities
        Dim STSError As New STSErrorPublisher
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)

        ' Local Variable for the results of the Call
        Dim vResultArray As Object = Nothing

        Dim ErrEx As Exception = Nothing

        If GetRatingDetailsInput.GetType Is GetType(AgentImplementationTypes.GetRatingDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.GetRatingDetailsResponseType
            oAgentRequest = DirectCast(GetRatingDetailsInput, AgentImplementationTypes.GetRatingDetailsRequestType)
        ElseIf GetRatingDetailsInput.GetType Is GetType(SAMForInsuranceImplementationTypes.GetRatingDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.GetRatingDetailsResponseType
        ElseIf GetRatingDetailsInput.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetRatingDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetRatingDetailsResponseType
        ElseIf GetRatingDetailsInput.GetType Is GetType(BaseImplementationTypes.BaseGetRatingDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.MessagingPackage
            oResponse = New BaseImplementationTypes.BaseGetRatingDetailsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check Mandatory fields
        If SAMFunc.NothingToString(GetRatingDetailsInput.BranchCode) = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If GetRatingDetailsInput.InsuranceFileKey = 0 Then
            ' InsuranceFileKey not set
            STSError.AddInvalidField("InsuranceFileKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFileKey"), "")
        End If

        If GetRatingDetailsInput.RiskKey = 0 Then
            ' RiskKey not set
            STSError.AddInvalidField("RiskKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "RiskKey"), "")
        End If

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InputValidation", True)
            ' Return the errors
            Return oResponse
        End If

        ' Validate branch code to ID
        Try
            Dim iSourceID As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", GetRatingDetailsInput.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), GetRatingDetailsInput.BranchCode)
        End Try

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InputValidation", True)
            ' Return the errors
            Return oResponse
        End If

        ' Create the Security Object
        Dim oGIS As bGIS.QuotePolicy = Nothing
        Try
            oGIS = New bGIS.QuotePolicy
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Debug.WriteLine(ex.Message)
        Finally
        End Try

        ' Initialise the GIS
        SAMFunc.InitialiseGISQP(oGIS, _SiriusUser)

        Try
            With GetRatingDetailsInput
                iRet = oGIS.GetRatingDetails(
                    v_sGisDataModelCode:=InternalSAMConstants.CNAgentsOnline,
                    v_sGisBusinessTypeCode:=gPMConstants.PMTypeOfBusinessNB,
                    v_lInsuranceFolderCnt:=0,
                    v_lInsuranceFileCnt:=CType(.InsuranceFileKey, Integer),
                    v_lRiskCnt:=CType(.RiskKey, Integer),
                    r_vRatingSections:=vResultArray)
            End With
        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ErrEx = New Exception("Failed to call bGIS.QuotePolicy.GetRatingDetails", ex)
            ExceptionManager.Publish(ErrEx)
            Throw ErrEx
        Finally

            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            If (ErrEx Is Nothing) Then
                If (iRet <> 1) Then
                    Dim ex As New Exception("bGIS.QuotePolicy.GetRatingDetails FAILED. Return Value = " + iRet.ToString)
                    ExceptionManager.Publish(ex)
                    Throw ex
                End If
            End If
        End Try

        If IsArray(vResultArray) = True Then
            ' Convert the Array into XML
            Dim strXML As String = String.Empty
            Dim oXMLDoc As New System.Xml.XmlDocument
            Try

                '''The Field "Description" has been changed to "EarningPattern" in array arrColHeaders(,)

                'Prakash: 2 additional parameters (IsLeviTax and IsTaxed are added to this list since the merged COM code returns these two extra parameters 
                Dim arrColHeaders(,) As Object = {{"RatingSectionType",
                        "PolicySectionType",
                        "RateType",
                        "AnnualRate",
                        "SumInsured",
                        "ThisPremium",
                        "AnnualPremium",
                        "Country",
                        "State",
                        "RatingSectionId",
                        "RatingSectionTypeId",
                        "PolicySectionTypeId",
                        "RateTypeId",
                        "OriginalFlag",
                        "CurrencyId",
                        "CountryId",
                        "StateId",
                        "IsAmended",
                        "CalculatedPremium",
                        "OverrideReason",
                        "AutoCalculated",
                        "EarningPattern",
                        "EarningPatternId",
                        "IsLeviTax",
                        "IsTaxed",
                        "RatingSectionTypeCode",
                        "RatingTypeCode",
                        "CountryCode",
                        "StateCode",
                         "EarningPatternCode",
                         "CurrencyCode"},
                        {System.Type.GetType("System.String"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.Decimal"),
                        System.Type.GetType("System.Decimal"),
                        System.Type.GetType("System.Decimal"),
                        System.Type.GetType("System.Decimal"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.Decimal"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.Int16"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.Int32"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.String"),
                        System.Type.GetType("System.String")}}

                Dim dsRatingDetails As DataSet
                dsRatingDetails = Utilities.ArrayToDataSet(vResultArray, arrColHeaders, "BaseGetRatingDetailsResponseTypeRatingDetails")
                strXML = dsRatingDetails.GetXml()

                If GetRatingDetailsInput.WCFSecurityToken = "" Then
                    oXMLDoc.LoadXml(strXML)
                    oResponse.RatingDetails = oXMLDoc.DocumentElement
                End If
                oResponse.ResultData = dsRatingDetails
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher("Failed to convert XML.", ex)
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetQuoteAndSummariesByRef", True)
            End Try

        End If

        Return oResponse

    End Function

    ' ***************************************************************** '
    ' Name: GetAllPolicyVersions
    '
    ' Description: 
    '
    ' ***************************************************************** '
    Public Function GetAllPolicyVersions(ByVal GetAllPolicyVersionsRequest As BaseGetAllPolicyVersionsRequestType) As BaseGetAllPolicyVersionsResponseType
        Dim oResponse As BaseGetAllPolicyVersionsResponseType
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim nTypeOfPackage As enumTypeOfPackage
        'Dim iRet As Int32
        Dim arrInsuranceFile As Object = Nothing
        Dim utils As New Utilities
        Dim oBusiness As New CoreBusiness
        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.GetAllPolicyVersionsRequestType = Nothing

        Const ACMethodName As String = "GetAllPolicyVersions"

        Dim STSError As New STSErrorPublisher

        If GetAllPolicyVersionsRequest.GetType Is GetType(AgentImplementationTypes.GetAllPolicyVersionsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.GetAllPolicyVersionsResponseType
        ElseIf GetAllPolicyVersionsRequest.GetType Is GetType(BaseImplementationTypes.BaseGetAllPolicyVersionsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oResponse = New BaseImplementationTypes.BaseGetAllPolicyVersionsResponseType
        ElseIf GetAllPolicyVersionsRequest.GetType Is GetType(CustomerImplementationTypes.GetAllPolicyVersionsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oResponse = New CustomerImplementationTypes.GetAllPolicyVersionsResponseType
        ElseIf GetAllPolicyVersionsRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.GetAllPolicyVersionsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.GetAllPolicyVersionsResponseType
        ElseIf GetAllPolicyVersionsRequest.GetType Is GetType(SAMForBrokingImplementationTypes.GetAllPolicyVersionsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oResponse = New SAMForBrokingImplementationTypes.GetAllPolicyVersionsResponseType
            samForBrokingRequest = DirectCast(GetAllPolicyVersionsRequest, SAMForBrokingImplementationTypes.GetAllPolicyVersionsRequestType)
        ElseIf GetAllPolicyVersionsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetAllPolicyVersionsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetAllPolicyVersionsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                If oBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Agent Key validation failed", "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Key Validation", True)
                    Return oResponse
                End If

                If oBusiness.AgentSecurityCheck(samForBrokingRequest.UserName, samForBrokingRequest.SourceId, PMEEntityType.Source) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security Check Failed", samForBrokingRequest.UserName & " does not have permission to access source " & samForBrokingRequest.SourceId)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetAllPolicyVersions", True)
                    Return oResponse
                End If
            End If
        End If

        'Check Mandatory Fields
        If GetAllPolicyVersionsRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If GetAllPolicyVersionsRequest.InsuranceFolderKey = 0 Then
            STSError.AddInvalidField("InsuranceFolderKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFolderKey"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        ' Branch Code
        Try
            Dim iSourceId As Integer = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", GetAllPolicyVersionsRequest.BranchCode)
        Catch ex As Exception
            ' not set
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), GetAllPolicyVersionsRequest.BranchCode)
        End Try

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Input Validation", True)
            ' Return the errors
            Return oResponse
        End If

        ' Check the insurance folder cnt is valid
        If oCoreBusiness.CheckInsuranceFolder(GetAllPolicyVersionsRequest.InsuranceFolderKey) = False Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyRecordNotFound, "Insurance Folder validation failed", "The Insurance Folder record does not exist for key: " & GetAllPolicyVersionsRequest.InsuranceFolderKey)
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If

        Dim dr As DataSet = Nothing

        ' BSJ April 09 - SQL Mixed Mode Compliance
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                                  _SiriusUser.Username, _SiriusUser.SourceID,
                                                  _SiriusUser.LanguageID,
                                                  SiriusUserDefaults.AppName)

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_All_Policy_Version")
                cmd.AddInParameter("@InsuranceFolderCnt", SqlDbType.Int).Value = GetAllPolicyVersionsRequest.InsuranceFolderKey
                cmd.AddInParameter("@nUserId", SqlDbType.Int).Value = _SiriusUser.UserID
                cmd.AddInParameter("@RetrieveAssociates", SqlDbType.Int).Value = IIf(GetAllPolicyVersionsRequest.RetrieveAssociates = True, 1, 0)

                dr = con.ExecuteDataSet(cmd, "dr")
                If dr.Tables(0).Rows.Count > 0 AndAlso GetAllPolicyVersionsRequest.RetrieveAssociates = True Then
                    AddExtraXMLElementForPolicyAssociate(dr, "AssociatedClients")
                End If
            End Using
        End Using

        ' Name the dataset and table and fields
        dr.DataSetName = "BaseGetAllPolicyVersionsResponseTypePolicies"
        dr.Tables(0).TableName = "Row"

        Dim oXMLDoc As New System.Xml.XmlDocument

        Try
            If GetAllPolicyVersionsRequest.WCFSecurityToken = "" Then
                oXMLDoc.LoadXml(dr.GetXml)
            End If
        Catch ex As Exception
            Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.XMLDocumentBadlyFormed, "Unable to load SQL output as XML", ex.Message)
            STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            Return oResponse
        End Try

        Try
            If GetAllPolicyVersionsRequest.WCFSecurityToken = "" Then
                oResponse.ResultDataset = oXMLDoc.DocumentElement
            End If
            oResponse.ResultData = dr
        Catch ex As Exception
            Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.XMLDocumentBadlyFormed, "Unable to pass Document Element", ex.Message)
            STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            Return oResponse
        End Try

        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            Dim oResponseAgent As New AgentImplementationTypes.GetAllPolicyVersionsResponseType
            oResponseAgent = DirectCast(oResponse, AgentImplementationTypes.GetAllPolicyVersionsResponseType)
            oResponse = oResponseAgent
        End If

        Return oResponse

    End Function

    Private Sub GenerateRenewalStatusChangeClientEmail(
           ByVal con As SiriusConnection,
           ByRef coreBusiness As CoreBusiness,
           ByRef request As BaseBindQuoteRequestType,
           ByVal renewalEmailType As String)

        Dim renewalBusiness As bSIRRenewal.Business = Nothing

        Try

            Dim comReturnValue As Integer

            renewalBusiness = CreateAndInitialiseRenewalBusiness(con, request.BranchCode)

            If Not request.DataStore.PolicyHasAgent Then

                comReturnValue = renewalBusiness.GenerateCustomerRenewalEmail(
                request.DataStore.PartyKey,
                request.InsuranceFileKey,
                renewalEmailType)

                ' allow a return code of "NotFound" as this indicates that
                ' the email setup is not present for this policy / product
                ' and this is perfectly valid
                If comReturnValue <> PMEReturnCode.PMTrue AndAlso
                    comReturnValue <> PMEReturnCode.PMNotFound Then
                    RaiseComMethodException("bSirRenewal.Business.GenerateCustomerRenewalEmail Failed")
                End If

            End If

        Finally

            If renewalBusiness IsNot Nothing Then
                renewalBusiness.Dispose()
                renewalBusiness = Nothing
            End If
        End Try

    End Sub

    Private Sub ProcessAcceptRenewal(
        ByVal con As SiriusConnection,
        ByRef coreBusiness As CoreBusiness,
        ByRef request As BaseBindQuoteRequestType,
       ByRef oResponse As BaseBindQuoteResponseType)

        Dim renewalBusiness As bSIRRenewal.Business = Nothing

        Try

            renewalBusiness = CreateAndInitialiseRenewalBusiness(con, request.BranchCode)

            ValidateTrueMonthlyPolicyIsEligibleForRenewal(con, renewalBusiness, request)

            AcceptRenewal(renewalBusiness, request)

            ProcessAccounts(con, request)

            RepopulateAccumulations(con, request)

            CreateRenewalAcceptanceEvents(con, request)

            CreateRenewalAcceptanceDocuments(con, coreBusiness, renewalBusiness, request, DirectCast(oResponse, BaseBindQuoteResponseType))

            GenerateRenewalStatusChangeClientEmail(
                        con,
                        coreBusiness,
                        request,
                        RenewalEmailType.Update)

        Finally

            If renewalBusiness IsNot Nothing Then
                renewalBusiness.Dispose()
                renewalBusiness = Nothing
            End If
        End Try

    End Sub

    Private Sub GetAllLiveFinancePlansOnAPolicy(ByVal con As SiriusConnection, ByVal request As BaseBindQuoteRequestType)

        Dim dsfinancePlandetails As DataSet = Nothing
        Dim oPremiumFinance As Object(,)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_all_live_Financeplans_on_policy")
            cmd.AddInParameter("@Insurance_folder_cnt", SqlDbType.Int).Value = request.DataStore.InsuranceFolderKey
            cmd.AddInParameter("@Insurance_file_cnt", SqlDbType.Int).Value = request.DataStore.OriginalInsuranceFileKey
            dsfinancePlandetails = con.ExecuteDataSet(cmd, "FinancePlandetails")
            oPremiumFinance = DirectCast(Utilities.DataSetToArray(dsfinancePlandetails), Object(,))
        End Using
        If IsArray(oPremiumFinance) Then
            ReDim request.DataStore.LiveFinancePlans(Microsoft.VisualBasic.UBound(oPremiumFinance, 2))
            For iCount As Integer = 0 To Microsoft.VisualBasic.UBound(oPremiumFinance, 2)
                request.DataStore.LiveFinancePlans(iCount) = New BaseLivePremiumFinancePlans
                With request.DataStore.LiveFinancePlans(iCount)
                    .PFPremiumFinanceCnt = Cast.ToInt32(oPremiumFinance(0, iCount), 0)
                    .PFPremiumFinanceVersion = Cast.ToInt32(oPremiumFinance(1, iCount), 0)
                End With
            Next
        End If

    End Sub
    Private Sub CancelInHousePlan(
            ByRef con As SiriusConnection,
            ByRef coreBusiness As CoreBusiness,
            ByRef request As BaseBindQuoteRequestType, Optional ByRef vDebitTransDetail As Object() = Nothing)

        Dim oSirPremiumFinance As New bSIRPremiumFinance.Business
        Dim lReturn As Long
        Dim dsfinancePlandetails As DataSet = Nothing
        Dim oPremiumFinance As Object(,)
        Dim dAmount As Double
        Dim vDebitTransDetailID As Object

        SAMFunc.InitialiseSBOObject(Con:=con, oObject:=oSirPremiumFinance, SiriusUser:=_SiriusUser, sBranchCode:=request.BranchCode, sObjectName:="bSIRPremuimFinance.Business")

        If request.DataStore.LiveFinancePlans IsNot Nothing Then

            For Each oFinancePlan As BaseLivePremiumFinancePlans In request.DataStore.LiveFinancePlans
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PFPremiumFinance_Sel_Single")
                    cmd.AddInParameter("@financeplancnt", SqlDbType.Int).Value = oFinancePlan.PFPremiumFinanceCnt
                    cmd.AddInParameter("@financeplanversion", SqlDbType.Int).Value = oFinancePlan.PFPremiumFinanceVersion
                    dsfinancePlandetails = con.ExecuteDataSet(cmd, "FinancePlandetails")
                    oPremiumFinance = DirectCast(Utilities.DataSetToArray(dsfinancePlandetails), Object(,))
                End Using

                If request.PayNowDetails IsNot Nothing Then
                    dAmount = request.PayNowDetails.Amount
                Else
                    dAmount = 0.0
                End If
                lReturn = oSirPremiumFinance.CancelPlanInHouse(vPremiumFinance:=oPremiumFinance, dRefund:=dAmount, r_vDebitTransDetail:=vDebitTransDetailID)

                If lReturn <> PMReturnCode.PMTrue Then
                    Dim oSAMErrorCollection As New SAMErrorCollection
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                        SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                        "bSIRPremiumFinance.Business.CancelInHousePlan")
                    oSAMErrorCollection.CheckForErrors()
                Else
                    If vDebitTransDetail Is Nothing Then
                        ReDim Preserve vDebitTransDetail(0)
                        vDebitTransDetail(0) = vDebitTransDetailID
                    Else
                        ReDim Preserve vDebitTransDetail(Microsoft.VisualBasic.UBound(vDebitTransDetail) + 1)
                        vDebitTransDetail(Microsoft.VisualBasic.UBound(vDebitTransDetail)) = vDebitTransDetailID

                    End If
                End If
            Next
        End If
        oSirPremiumFinance.Dispose()
        oSirPremiumFinance = Nothing
    End Sub

    '''<summary>
    ''' Retrieves all the amount details of Live Versions of a policy.
    '''</summary>
    '''<param name="oGetAllLivePolicyVersionAmountsRequest" type="BaseGetAllLivePolicyVersionAmountsRequestType"></param>   
    '''<returns>BaseGetAllLivePolicyVersionAmountsResponseType</returns>
    '''<remarks></remarks> 
    Public Overloads Function GetAllLivePolicyVersionAmounts(ByVal oGetAllLivePolicyVersionAmountsRequest As BaseGetAllLivePolicyVersionAmountsRequestType) As BaseGetAllLivePolicyVersionAmountsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Return (GetAllLivePolicyVersionAmounts(con, oGetAllLivePolicyVersionAmountsRequest))
        End Using
    End Function

    '''<summary>
    ''' Retrieves details of a party bank item or all Party bank items for a specified party.
    '''</summary>
    '''<param name="con" type="SiriusConnection"></param>
    '''<param name="oGetAllLivePolicyVersionAmountsRequest" type="BaseGetAllLivePolicyVersionAmountsRequestType"></param>   
    '''<returns>BaseGetAllLivePolicyVersionAmountsResponseType</returns>
    '''<remarks></remarks> 
    Public Overloads Function GetAllLivePolicyVersionAmounts(ByVal con As SiriusConnection, ByVal oGetAllLivePolicyVersionAmountsRequest As BaseGetAllLivePolicyVersionAmountsRequestType) As BaseGetAllLivePolicyVersionAmountsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim oResponse As New BaseGetAllLivePolicyVersionAmountsResponseType

        Dim nTypeofPackage As enumTypeOfPackage
        If oGetAllLivePolicyVersionAmountsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetAllLivePolicyVersionAmountsRequestType) Then
            nTypeofPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetAllLivePolicyVersionAmountsResponseType
        Else
            nTypeofPackage = enumTypeOfPackage.UnknownPackage
            Return oResponse
        End If

        With oGetAllLivePolicyVersionAmountsRequest
            'Check Mandatory Field
            .Validate(CObj(oSAMErrorCollection))
            oSAMErrorCollection.CheckForErrors()

            'Look up validation
            Dim iSourceId As Integer
            iSourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, .BranchCode, "BranchCode", oSAMErrorCollection)
            oSAMErrorCollection.CheckForErrors()

            Dim dsInsuranceFileDetails As DataSet
            Dim bACTInstalments As New bACTInstalments.Business
            Dim cPlanOS As Decimal
            Dim pfTransactionId As Integer
            Dim lreturn As Long

            lreturn = bACTInstalments.Initialise(
                                     _SiriusUser.Username,
                                     _SiriusUser.Password,
                                     CInt(_SiriusUser.UserID),
                                     CInt(_SiriusUser.SourceID),
                                     CInt(_SiriusUser.LanguageID),
                                     CInt(_SiriusUser.CurrencyID),
                                     1,
                                     SiriusUserDefaults.AppName)
            If (lreturn <> PMEReturnCode.PMTrue) Then
                Dim oErrors As New SAMErrorCollection
                ' if the account processing fails then throw a business rule error
                oErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                    "bACTInstalments.Business.Initialise")
                oErrors.CheckForErrors()

            End If
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Premium_Details_For_All_Policy_Versions")
                cmd.AddInParameter("@nInsurance_folder_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(.InsuranceFolderKey)
                If (.InsuranceFolderKey = 0) Then
                    cmd.AddInParameter("@nInsurance_file_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(.InsuranceFileKey)
                End If
                dsInsuranceFileDetails = con.ExecuteDataSet(cmd, "InsuranceFileDetails")
            End Using

            If dsInsuranceFileDetails.Tables.Count > 0 AndAlso dsInsuranceFileDetails.Tables(0).Rows.Count > 0 Then
                ReDim oResponse.LivePolicyVerisonDetails(dsInsuranceFileDetails.Tables(0).Rows.Count - 1)
                For iCount As Integer = 0 To dsInsuranceFileDetails.Tables(0).Rows.Count - 1
                    oResponse.LivePolicyVerisonDetails(iCount) = New BaseLivePolicyAmountDetailsType
                    oResponse.LivePolicyVerisonDetails(iCount).InsuranceFileKey = Cast.ToInt32(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Insurance_file_cnt"), 0)
                    oResponse.LivePolicyVerisonDetails(iCount).ThisPremium = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("this_premium"), 0), 2)
                    oResponse.LivePolicyVerisonDetails(iCount).RiskClientTaxesTotal = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Risk_Client_Tax"), 0), 2)
                    oResponse.LivePolicyVerisonDetails(iCount).RiskNonClientTaxesTotal = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Risk_Non_Client_Tax"), 0), 2)
                    oResponse.LivePolicyVerisonDetails(iCount).PolicyNonClientTaxesTotal = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Policy_Non_Client_Tax"), 0), 2)
                    oResponse.LivePolicyVerisonDetails(iCount).PolicyClientTaxesTotal = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Policy_client_Tax"), 0), 2)
                    oResponse.LivePolicyVerisonDetails(iCount).PolicyFeesTotal = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Policy_Fee_Total"), 0), 2)
                    oResponse.LivePolicyVerisonDetails(iCount).RiskFeesTotal = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Risk_Fee_Total"), 0), 2)
                    oResponse.LivePolicyVerisonDetails(iCount).AmountCollected = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("PremiumPaid"), 0), 2)
                    oResponse.LivePolicyVerisonDetails(iCount).OutstandingAmount = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Outstanding_Amount"), 0), 2)
                    oResponse.LivePolicyVerisonDetails(iCount).TransactionAmount = Decimal.Round(Cast.ToDecimal(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Transaction_Amount"), 0), 2)

                    ''Currently we are only supporting the Payment Method as Instalments
                    Select Case Cast.ToString(dsInsuranceFileDetails.Tables(0).Rows(iCount)("Payment_Method"), "")
                        Case "Instalments", "Credit Card", "Instalment"
                            oResponse.LivePolicyVerisonDetails(iCount).PaymentMethod = BaseImplementationTypes.PolicyPaymentMethod.Installments
                        Case "Invoice"
                            oResponse.LivePolicyVerisonDetails(iCount).PaymentMethod = BaseImplementationTypes.PolicyPaymentMethod.Invoice
                        Case "PayNow"
                            oResponse.LivePolicyVerisonDetails(iCount).PaymentMethod = BaseImplementationTypes.PolicyPaymentMethod.PayNow
                    End Select
                    pfTransactionId = (Cast.ToInt32(dsInsuranceFileDetails.Tables(0).Rows(iCount)("PlanTransactionId"), 0))
                    If pfTransactionId <> 0 Then
                        lreturn = bACTInstalments.GetPlanOS(v_lPlanTransDetailID:=pfTransactionId,
                                                        r_cOS:=cPlanOS)

                        If lreturn <> PMReturnCode.PMTrue Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                                SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                                "bActInstalments.Business.GetPlanOS")
                            oSAMErrorCollection.CheckForErrors()
                        End If
                    End If
                    oResponse.LivePolicyVerisonDetails(iCount).PlanOutstandingAmount = cPlanOS
                Next
            End If
            If Not bACTInstalments Is Nothing Then
                bACTInstalments.Dispose()
                bACTInstalments = Nothing
            End If
        End With

        Return oResponse
    End Function

    Private Sub CheckPlanOSBalanceToSpread(ByRef con As SiriusConnection,
            ByRef coreBusiness As CoreBusiness,
            ByRef request As BaseBindQuoteRequestType, Optional ByRef vDebitTransDetail As Object() = Nothing,
            Optional ByRef bSettleTransactions As Boolean = False)

        Dim bACTInstalments As New bACTInstalments.Business
        Dim oSirPremiumFinance As New bSIRPremiumFinance.Business
        Dim lReturn As Long
        Dim dsfinancePlandetails As DataSet = Nothing
        Dim oPremiumFinance As Object(,)
        Dim cPlanOS As Decimal
        Dim dAmount As Double = 0.0
        Dim vDebitTransDetailID As Object

        If request.DataStore.PFPremiumFinanceCnt <> 0 Then
            lReturn = bACTInstalments.Initialise(
                         _SiriusUser.Username,
                         _SiriusUser.Password,
                         CInt(_SiriusUser.UserID),
                         CInt(_SiriusUser.SourceID),
                         CInt(_SiriusUser.LanguageID),
                         CInt(_SiriusUser.CurrencyID),
                         1,
                         SiriusUserDefaults.AppName)
            If (lReturn <> PMEReturnCode.PMTrue) Then
                Dim oErrors As New SAMErrorCollection
                ' if the account processing fails then throw a business rule error
                oErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                    "bACTInstalments.Business.Initialise")
                oErrors.CheckForErrors()

            End If
            SAMFunc.InitialiseSBOObject(Con:=con, oObject:=oSirPremiumFinance, SiriusUser:=_SiriusUser, sBranchCode:=request.BranchCode, sObjectName:="bSIRPremuimFinance.Business")

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PFPremiumFinance_Sel_Single")
                cmd.AddInParameter("@financeplancnt", SqlDbType.Int).Value = request.DataStore.PFPremiumFinanceCnt
                cmd.AddInParameter("@financeplanversion", SqlDbType.Int).Value = request.DataStore.PFPremiumFinanceVersion
                dsfinancePlandetails = con.ExecuteDataSet(cmd, "FinancePlandetails")
                oPremiumFinance = DirectCast(Utilities.DataSetToArray(dsfinancePlandetails), Object(,))
            End Using
            lReturn = bACTInstalments.GetPlanOS(v_lPlanTransDetailID:=CInt(oPremiumFinance(k_PFPlanPlanTransactionID, 0)),
                                                r_cOS:=cPlanOS)

            If lReturn <> PMReturnCode.PMTrue Then
                Dim oSAMErrorCollection As New SAMErrorCollection
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                    "bActInstalments.Business.GetPlanOS")
                oSAMErrorCollection.CheckForErrors()
            End If

            If cPlanOS + request.DataStore.TotalPremiumAmount < 0 Then
                If request.SelectedInstalmentQuoteSpecified Then
                    Dim oSAMErrorCollection As New SAMErrorCollection
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InsufficentPlanBalnceToSpreadTheReducedMTA,
                                                        SAMBusinessErrors.InsufficentPlanBalnceToSpreadTheReducedMTA.ToString,
                                                        "Insufficent Plan Balance To Spread The Reduced MTA")
                    oSAMErrorCollection.CheckForErrors()
                Else

                    request.DataStore.PaymentMethod = PolicyPaymentMethod.PayNow
                    If request.PayNowDetails IsNot Nothing Then
                        dAmount = cPlanOS + request.DataStore.TotalPremiumAmount
                        request.PayNowDetails.Amount = cPlanOS + request.DataStore.TotalPremiumAmount
                    Else
                        dAmount = 0.0
                    End If
                    lReturn = oSirPremiumFinance.CancelPlanInHouse(vPremiumFinance:=oPremiumFinance, dRefund:=dAmount, r_vDebitTransDetail:=vDebitTransDetailID)

                    If lReturn <> PMReturnCode.PMTrue Then
                        Dim oSAMErrorCollection As New SAMErrorCollection
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                            SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                            "bSIRPremiumFinance.Business.CancelInHousePlan")
                        oSAMErrorCollection.CheckForErrors()
                    Else
                        If vDebitTransDetail Is Nothing Then
                            ReDim Preserve vDebitTransDetail(0)
                            vDebitTransDetail(0) = vDebitTransDetailID
                        Else
                            ReDim Preserve vDebitTransDetail(Microsoft.VisualBasic.UBound(vDebitTransDetail) + 1)
                            vDebitTransDetail(Microsoft.VisualBasic.UBound(vDebitTransDetail)) = vDebitTransDetailID

                        End If
                        bSettleTransactions = True
                    End If

                End If

            End If
        End If
        If Not oSirPremiumFinance Is Nothing Then
            oSirPremiumFinance.Dispose()
            oSirPremiumFinance = Nothing
        End If
        If Not bACTInstalments Is Nothing Then
            bACTInstalments.Dispose()
            bACTInstalments = Nothing
        End If
    End Sub

    Private Sub GenerateRenewalDocument(
        ByRef con As SiriusConnection,
        ByRef renewalBusiness As bSIRRenewal.Business,
        ByRef request As BaseBindQuoteRequestType,
        ByVal mode As Integer,
        ByVal processTypeDocument As Integer,
        ByRef oResponse As BaseBindQuoteResponseType)

        Dim generateDocumentRequest As BaseGenerateDocumentRequestType = New BaseGenerateDocumentRequestType
        Dim generateDocumentResponse As BaseGenerateDocumentResponseType

        ' get document type code

        ' PN64308 - If we can't find the template the we can still continue without the document.
        Dim documentTemplateCodeId As Integer = CInt(GetTemplateCodesForRenewal(request.InsuranceFileKey, processTypeDocument, "RN", con))

        If (documentTemplateCodeId > 0) Then
            generateDocumentRequest.DocumentTemplateCode = GetTemplateCode(con, documentTemplateCodeId)
            generateDocumentRequest.InsuranceFolderKey = request.DataStore.InsuranceFolderKey
            generateDocumentRequest.InsuranceFileKey = request.InsuranceFileKey
            generateDocumentRequest.BranchCode = request.BranchCode
            generateDocumentRequest.Mode = mode
            generateDocumentRequest.OutputAsHTML = False
            generateDocumentRequest.OutputAsPDF = False
            generateDocumentRequest.PartyKey = request.DataStore.PartyKey
            generateDocumentRequest.SourceId = request.SourceId
            generateDocumentRequest.SpoolDocumentOnly = True

            generateDocumentResponse = GenerateDocument(generateDocumentRequest, con)

            ' ensure any sts errors raised by this legacy function are handled correctly
            SAMErrorCollection.CheckForErrorsFromSTS(generateDocumentResponse.STSError)
        ElseIf processTypeDocument = 3 Then
            AddWarning(oResponse.Warnings,
                            SAMBusinessWarnings.InAbsenceOfRenewalDebitNoteDocument,
                            "Document is not attached for Renewal DebitNote")
        ElseIf processTypeDocument = 4 Then
            AddWarning(oResponse.Warnings,
                            SAMBusinessWarnings.InAbsenceOfRenewalScheduleDocument,
                            "Document is not attached for Renewal Schedule")
        ElseIf processTypeDocument = 5 Then
            AddWarning(oResponse.Warnings,
                SAMBusinessWarnings.InAbsenceOfRenewalCertificateDocument,
                "Document is not attached for Renewal Certificate")

        End If

    End Sub

    Private Sub CreateRenewalAcceptanceDocuments(
    ByRef con As SiriusConnection,
    ByRef coreBusiness As CoreBusiness,
    ByRef renewalBusiness As bSIRRenewal.Business,
    ByRef request As BaseBindQuoteRequestType,
    ByRef oResponse As BaseBindQuoteResponseType)

        GetRenewalPrintingOptions(coreBusiness, request, con)

        If (request.DataStore.IsTrueMonthlyPolicy And request.DataStore.AnniversaryCopy) OrElse
            request.DataStore.IsTrueMonthlyPolicy = False Then

            If Not request.DataStore.PolicyHasAgent Then
                renewalBusiness.GenerateCustomerRenewalEmail(
                request.DataStore.PartyKey,
                request.InsuranceFileKey,
                RenewalEmailType.Update)
            End If

            If request.DataStore.PrintRenewalCertificate Then

                GenerateRenewalDocument(con, renewalBusiness, request,
                    ACSpoolDocMode, SIRConst.SIRDocTypeCertificate,
                    DirectCast(oResponse, BaseBindQuoteResponseType))

            End If

            If request.DataStore.PrintRenewalSchedule Then
                GenerateRenewalDocument(con, renewalBusiness, request,
                    ACSpoolDocMode, SIRConst.SIRDocTypeSchedule,
                    DirectCast(oResponse, BaseBindQuoteResponseType))

            End If

            If request.DataStore.PrintRenewalDebitNote Then
                GenerateRenewalDocument(con, renewalBusiness, request,
                    ACSpoolDocMode, SIRConst.SIRDocTypeDebitNote,
                    DirectCast(oResponse, BaseBindQuoteResponseType))
            End If

        End If

    End Sub

    Private Sub CreateRenewalAcceptanceEvents(
    ByRef con As SiriusConnection,
    ByRef request As BaseBindQuoteRequestType)

        Dim eventDescription As String = "Accept Renewal - " + request.DataStore.InsuranceRef

        ' Create Accept Renewal Event
        CreateEvent(con, request.DataStore.PartyKey,
            request.DataStore.InsuranceFolderKey, request.InsuranceFileKey,
            _SiriusUser.UserID, DateTime.Now,
            eventDescription,
            EventTypeCode.ChangeOfPolicyDetails)

        If request.DataStore.AgentIsInTransferMode Then

            eventDescription = "Renewal Accepted - Broker Transfer From " +
                request.DataStore.AgentShortname +
                    " to " + request.DataStore.TransferPartyShortname

            ' Create Accept Renewal Event 
            ' NB: TransferPartyShortname can be either the shortname or the
            ' business type if transfering from agent to direct business.
            CreateEvent(con, request.DataStore.PartyKey,
                request.DataStore.InsuranceFolderKey, request.InsuranceFileKey,
                _SiriusUser.UserID, DateTime.Now,
                eventDescription,
                EventTypeCode.ChangeOfPolicyDetails)

        End If

    End Sub

    Private Sub RepopulateAccumulations(
ByVal con As SiriusConnection,
ByVal request As BaseBindQuoteRequestType)

        Dim accumulationValuesBusiness As bSIRAccumulationValues.Business = Nothing

        Try

            accumulationValuesBusiness = CreateAndInitialiseAccumulationValuesBusiness(con, request.BranchCode)

            Dim comReturnValue As Integer

            ' initialise with the insurance file key
            accumulationValuesBusiness.InsuranceFileCnt = request.InsuranceFileKey

            comReturnValue = accumulationValuesBusiness.RepopulateAccumValues()
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRAccumulationValues.Business Failed", comReturnValue)
            End If

        Finally

            If accumulationValuesBusiness IsNot Nothing Then
                accumulationValuesBusiness.Dispose()
                accumulationValuesBusiness = Nothing
            End If
        End Try

    End Sub

    Private Sub AcceptRenewal(
    ByRef renewalBusiness As bSIRRenewal.Business,
    ByRef request As BaseBindQuoteRequestType)

        Dim comReturnValue As Integer

        ' run accept renewal
        comReturnValue = renewalBusiness.AcceptRenewal(
        CInt(request.DataStore.RenewalStatusOriginalInsuranceFileKey),
        CInt(request.InsuranceFileKey),
        CInt(request.DataStore.RenewalStatusKey))

        If comReturnValue <> PMReturnCode.PMTrue Then
            If comReturnValue = InternalSAMConstants.PMRenewalAlreadyAccepted Then
                Dim samErrorsCollection As New SAMErrorCollection
                samErrorsCollection.AddBusinessRule(
                    SAMBusinessErrors.PolicyAlreadyAccepted,
                    SAMBusinessErrors.PolicyAlreadyAccepted.ToString)
                samErrorsCollection.CheckForErrors()

            Else
                RaiseComMethodException("bSIRRenewal.Business.AcceptRenewal Failed")
            End If
        End If

    End Sub

    Private Sub GetRenewalPrintingOptions(
    ByVal coreBusiness As CoreBusiness,
    ByRef request As BaseBindQuoteRequestType,
    ByRef con As SiriusConnection)

        Dim optionPrintRenewalCertificate As Boolean = False
        Dim optionPrintRenewalSchedule As Boolean = False
        Dim optionPrintRenewalDebitNote As Boolean = False
        Dim iProduct_ID As Integer

        iProduct_ID = GetProductID(request.InsuranceFileKey, con)
        coreBusiness.GetProductDocumentOption(iProduct_ID, optionPrintRenewalCertificate, optionPrintRenewalSchedule, optionPrintRenewalDebitNote)

        request.DataStore.PrintRenewalCertificate = optionPrintRenewalCertificate
        request.DataStore.PrintRenewalSchedule = optionPrintRenewalSchedule
        request.DataStore.PrintRenewalDebitNote = optionPrintRenewalDebitNote

    End Sub

    Private Sub ValidateTrueMonthlyPolicyIsEligibleForRenewal(
    ByRef con As SiriusConnection,
    ByRef renewalBusiness As bSIRRenewal.Business,
    ByRef request As BaseBindQuoteRequestType)

        Dim comReturnValue As Integer
        Dim samErrorsCollection As New SAMErrorCollection

        If request.DataStore.IsTrueMonthlyPolicy AndAlso request.DataStore.AnniversaryCopy Then

            Dim trueMonthlyPolicyObject(,) As Object = Nothing
            Dim trueMonthlyPolicyDetails As Object(,) = Nothing

            ' validate that the last item of the previous cycle
            ' has already been accepted if not then this item cannot be accepted
            comReturnValue = renewalBusiness.ValidateAcceptTMPAnniversaryIsValidAction(request.InsuranceFileKey, request.DataStore.CoverStartDate, trueMonthlyPolicyObject)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRRenewalBusiness.ValidateAcceptTMPIsValidAction Failed", comReturnValue)
            End If
            If trueMonthlyPolicyObject IsNot Nothing AndAlso TypeOf (trueMonthlyPolicyObject) Is System.Array Then
                trueMonthlyPolicyDetails = DirectCast(trueMonthlyPolicyObject, Object(,))
            End If
            If IsArray(trueMonthlyPolicyDetails) Then

                Dim insuranceFileCnt As Integer = ToSafeInteger(trueMonthlyPolicyDetails(0, 0))

                If insuranceFileCnt = 0 Then

                    samErrorsCollection.AddBusinessRule(
                        SAMBusinessErrors.TrueMonthlyPolicyAnniversaryRenewalCannotBeProcessedUntilTheLastMonthlyCycleHasBeenAccepted,
                        SAMBusinessErrors.TrueMonthlyPolicyAnniversaryRenewalCannotBeProcessedUntilTheLastMonthlyCycleHasBeenAccepted.ToString)

                    samErrorsCollection.CheckForErrors()

                End If

            Else

                samErrorsCollection.AddBusinessRule(
                    SAMBusinessErrors.TrueMonthlyPolicyAnniversaryRenewalCannotBeProcessedUntilTheLastMonthlyCycleHasBeenAccepted,
                    SAMBusinessErrors.TrueMonthlyPolicyAnniversaryRenewalCannotBeProcessedUntilTheLastMonthlyCycleHasBeenAccepted.ToString)

                samErrorsCollection.CheckForErrors()
            End If
            GetAnnivPriorVersionInsFileCnt(con, request.InsuranceFileKey, request.DataStore.InsuranceFolderKey, request.DataStore.RenewalStatusOriginalInsuranceFileKey)
        End If

    End Sub
    Private Sub GetAnnivPriorVersionInsFileCnt(
        ByVal con As SiriusConnection,
        ByVal insuranceFileCount As Integer,
        ByVal insuranceFolderCount As Integer,
        ByRef renewalStatusOriginalInsuranceFileCnt As Integer)
        If insuranceFileCount <> 0 Then

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Anniv_PriorVersion_InsuranceFileCnt")
                cmd.AddInParameter("@nInsurance_file_cnt", SqlDbType.Int).Value = insuranceFileCount
                cmd.AddInParameter("@nInsurance_folder_count", SqlDbType.Int).Value = insuranceFolderCount
                cmd.AddOutParameter("@r_nOriginal_Insurance_file_cnt", SqlDbType.Int)

                con.ExecuteNonQuery(cmd)
                renewalStatusOriginalInsuranceFileCnt = Cast.ToInt32(cmd.Parameters("@r_nOriginal_Insurance_file_cnt").Value, 0)
            End Using
        End If

    End Sub
    Private Sub AssignCoverNoteSheet(
        ByRef con As SiriusConnection,
        ByVal InsuranceFileCnt As Integer,
        ByVal CoverNoteBookNumber As String,
        ByVal CoverSheetNumber As Integer)

        If CoverNoteBookNumber <> String.Empty And CoverSheetNumber > 0 Then

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Cover_Note_Sheet_Assign")

                cmd.AddInParameter("@cover_note_book_number", SqlDbType.VarChar, 50).Value = CoverNoteBookNumber
                cmd.AddInParameter("@cover_note_sheet_number", SqlDbType.Int).Value = CoverSheetNumber
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = InsuranceFileCnt
                cmd.AddInParameter("@user_id", SqlDbType.Int).Value = _SiriusUser.UserID

                con.ExecuteNonQuery(cmd)

            End Using

        End If

    End Sub

    Private Sub AddAssociatedSubAgents(ByVal con As SiriusConnection, ByVal insuranceFileCnt As Int64, ByVal agentCnt As Int64)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_associated_subagents_add")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileCnt
            cmd.AddInParameter("@agent_cnt", SqlDbType.Int).Value = agentCnt

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub
    Private Sub AddSubAgent(ByVal con As SiriusConnection, ByVal insuranceFileCnt As Int64, ByVal agentCnt As Int64)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_SubAgent_Add")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileCnt
            cmd.AddInParameter("@agent_cnt", SqlDbType.Int).Value = agentCnt

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub
    Private Sub SetMulticurrencyValues(ByVal con As SiriusConnection, ByVal oAddQuoteOut As AddQuoteOut, ByVal oCoreBusiness As CoreBusiness, ByVal iSourceId As Int32, ByVal iCurrencyId As Int32)
        ' Get the multi-currency values
        Dim dCurrencyBaseExchangeRate As Double
        Dim dtCurrencyBaseDate As Date
        Dim dSystemBaseExchangeRate As Double
        Dim dtSystemBaseDate As Date

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Do_Currency_Conversion")

            cmd.AddInParameter("@account_id", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@company_id", SqlDbType.Int).Value = iSourceId
            cmd.AddInParameter("@currency_id", SqlDbType.Int).Value = iCurrencyId
            cmd.AddInParameter("@currency_amount_unrounded", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@mode", SqlDbType.NVarChar).Value = Cast.NullIfDefault("ALL")

            cmd.AddOutParameter("@base_currency_id", SqlDbType.Int)
            cmd.AddOutParameter("@base_currency_code", SqlDbType.VarChar, 255)
            cmd.AddOutParameter("@base_amount", SqlDbType.Money)
            cmd.AddOutParameter("@base_amount_unrounded", SqlDbType.Money)

            cmd.AddOutParameter("@account_currency_id", SqlDbType.Int)
            cmd.AddOutParameter("@account_amount", SqlDbType.Money)
            cmd.AddOutParameter("@account_amount_unrounded", SqlDbType.Money)

            cmd.AddOutParameter("@system_currency_id", SqlDbType.Int)
            cmd.AddOutParameter("@system_amount", SqlDbType.Money)
            cmd.AddOutParameter("@system_amount_unrounded", SqlDbType.Money)

            cmd.AddInOutParameter("@currency_base_xrate", SqlDbType.Float)
            cmd.AddInOutParameter("@currency_base_date", SqlDbType.DateTime).Value = Today

            cmd.AddInOutParameter("@account_base_xrate", SqlDbType.Float)
            cmd.AddInOutParameter("@account_base_date", SqlDbType.DateTime).Value = Today

            cmd.AddInOutParameter("@system_base_xrate", SqlDbType.Float)
            cmd.AddInOutParameter("@system_base_date", SqlDbType.DateTime).Value = Today

            cmd.AddOutParameter("@return_status", SqlDbType.SmallInt)

            con.ExecuteNonQuery(cmd)

            ' Check the return value
            If Cast.ToInt32(cmd.Parameters("@return_status").Value, 0) = 1 Then

                dCurrencyBaseExchangeRate = Cast.ToDouble(cmd.Parameters("@currency_base_xrate").Value, 0)
                dtCurrencyBaseDate = Cast.ToDateTime(cmd.Parameters("@currency_base_date").Value, Date.MinValue)

                dSystemBaseExchangeRate = Cast.ToDouble(cmd.Parameters("@system_base_xrate").Value, 0)
                dtSystemBaseDate = Cast.ToDateTime(cmd.Parameters("@system_base_date").Value, Date.MinValue)

                iCurrencyId = Cast.ToInt32(cmd.Parameters("@base_currency_id").Value, 0)

            End If

        End Using

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_update_insurance_file")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oAddQuoteOut.InsuranceFileCnt
            cmd.AddInParameter("@currency_base_xrate", SqlDbType.Float).Value = dCurrencyBaseExchangeRate
            If dtCurrencyBaseDate = Date.MinValue Then
                cmd.AddInParameter("@currency_base_date", SqlDbType.DateTime).Value = System.DBNull.Value
            Else
                cmd.AddInParameter("@currency_base_date", SqlDbType.DateTime).Value = dtCurrencyBaseDate
            End If

            cmd.AddInParameter("@account_base_xrate", SqlDbType.Float).Value = System.DBNull.Value
            cmd.AddInParameter("@account_base_date", SqlDbType.DateTime).Value = System.DBNull.Value

            cmd.AddInParameter("@system_base_xrate", SqlDbType.Float).Value = dSystemBaseExchangeRate
            If dtSystemBaseDate = Date.MinValue Then
                cmd.AddInParameter("@system_base_date", SqlDbType.DateTime).Value = System.DBNull.Value
            Else
                cmd.AddInParameter("@system_base_date", SqlDbType.DateTime).Value = dtSystemBaseDate
            End If


            cmd.AddInParameter("@exchange_rate_override_reason_id", SqlDbType.Int).Value = System.DBNull.Value
            cmd.AddInParameter("@base_currency_id", SqlDbType.Int).Value = iCurrencyId

            cmd.AddInParameter("@agent_account_currency_id", SqlDbType.Int).Value = System.DBNull.Value

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub
    Private Sub SetMulticurrencyValues(ByVal con As SiriusConnection, ByVal oAddQuoteOut As bSIRInsuranceFile.Services, ByVal oCoreBusiness As CoreBusiness, ByVal iSourceId As Int32, ByVal iCurrencyId As Int32)
        ' Get the multi-currency values
        Dim dCurrencyBaseExchangeRate As Double
        Dim dtCurrencyBaseDate As Date
        Dim dSystemBaseExchangeRate As Double
        Dim dtSystemBaseDate As Date

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Do_Currency_Conversion")

            cmd.AddInParameter("@account_id", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@company_id", SqlDbType.Int).Value = iSourceId
            cmd.AddInParameter("@currency_id", SqlDbType.Int).Value = iCurrencyId
            cmd.AddInParameter("@currency_amount_unrounded", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@mode", SqlDbType.NVarChar).Value = Cast.NullIfDefault("ALL")

            cmd.AddOutParameter("@base_currency_id", SqlDbType.Int)
            cmd.AddOutParameter("@base_currency_code", SqlDbType.VarChar, 255)
            cmd.AddOutParameter("@base_amount", SqlDbType.Money)
            cmd.AddOutParameter("@base_amount_unrounded", SqlDbType.Money)

            cmd.AddOutParameter("@account_currency_id", SqlDbType.Int)
            cmd.AddOutParameter("@account_amount", SqlDbType.Money)
            cmd.AddOutParameter("@account_amount_unrounded", SqlDbType.Money)

            cmd.AddOutParameter("@system_currency_id", SqlDbType.Int)
            cmd.AddOutParameter("@system_amount", SqlDbType.Money)
            cmd.AddOutParameter("@system_amount_unrounded", SqlDbType.Money)

            cmd.AddInOutParameter("@currency_base_xrate", SqlDbType.Float)
            cmd.AddInOutParameter("@currency_base_date", SqlDbType.DateTime).Value = Today

            cmd.AddInOutParameter("@account_base_xrate", SqlDbType.Float)
            cmd.AddInOutParameter("@account_base_date", SqlDbType.DateTime).Value = Today

            cmd.AddInOutParameter("@system_base_xrate", SqlDbType.Float)
            cmd.AddInOutParameter("@system_base_date", SqlDbType.DateTime).Value = Today

            cmd.AddOutParameter("@return_status", SqlDbType.SmallInt)

            con.ExecuteNonQuery(cmd)

            ' Check the return value
            If Cast.ToInt32(cmd.Parameters("@return_status").Value, 0) = 1 Then

                dCurrencyBaseExchangeRate = Cast.ToDouble(cmd.Parameters("@currency_base_xrate").Value, 0)
                dtCurrencyBaseDate = Cast.ToDateTime(cmd.Parameters("@currency_base_date").Value, Date.MinValue)

                dSystemBaseExchangeRate = Cast.ToDouble(cmd.Parameters("@system_base_xrate").Value, 0)
                dtSystemBaseDate = Cast.ToDateTime(cmd.Parameters("@system_base_date").Value, Date.MinValue)

                iCurrencyId = Cast.ToInt32(cmd.Parameters("@base_currency_id").Value, 0)

            End If

        End Using

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_update_insurance_file")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oAddQuoteOut.InsuranceFileCnt
            cmd.AddInParameter("@currency_base_xrate", SqlDbType.Float).Value = dCurrencyBaseExchangeRate
            If dtCurrencyBaseDate = Date.MinValue Then
                cmd.AddInParameter("@currency_base_date", SqlDbType.DateTime).Value = System.DBNull.Value
            Else
                cmd.AddInParameter("@currency_base_date", SqlDbType.DateTime).Value = dtCurrencyBaseDate
            End If

            cmd.AddInParameter("@account_base_xrate", SqlDbType.Float).Value = System.DBNull.Value
            cmd.AddInParameter("@account_base_date", SqlDbType.DateTime).Value = System.DBNull.Value

            cmd.AddInParameter("@system_base_xrate", SqlDbType.Float).Value = dSystemBaseExchangeRate

            If dtSystemBaseDate = Date.MinValue Then
                cmd.AddInParameter("@system_base_date", SqlDbType.DateTime).Value = System.DBNull.Value
            Else
                cmd.AddInParameter("@system_base_date", SqlDbType.DateTime).Value = dtSystemBaseDate
            End If


            cmd.AddInParameter("@exchange_rate_override_reason_id", SqlDbType.Int).Value = System.DBNull.Value
            cmd.AddInParameter("@base_currency_id", SqlDbType.Int).Value = iCurrencyId

            cmd.AddInParameter("@agent_account_currency_id", SqlDbType.Int).Value = System.DBNull.Value

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    Private Sub UpdateMulticurrencyValues(ByVal con As SiriusConnection, ByVal InsuranceFileCnt As Integer, ByVal iSourceId As Integer, ByVal iCurrencyId As Integer)
        ' Get the multi-currency values
        Dim dCurrencyBaseExchangeRate As Double
        Dim dtCurrencyBaseDate As Date = New Date(1899, 12, 29)
        Dim dSystemBaseExchangeRate As Double
        Dim dtSystemBaseDate As Date = New Date(1899, 12, 29)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Do_Currency_Conversion")

            cmd.AddInParameter("@account_id", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@company_id", SqlDbType.Int).Value = iSourceId
            cmd.AddInParameter("@currency_id", SqlDbType.Int).Value = iCurrencyId
            cmd.AddInParameter("@currency_amount_unrounded", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@mode", SqlDbType.NVarChar).Value = Cast.NullIfDefault("ALL")

            cmd.AddOutParameter("@base_currency_id", SqlDbType.Int)
            cmd.AddOutParameter("@base_currency_code", SqlDbType.VarChar, 255)
            cmd.AddOutParameter("@base_amount", SqlDbType.Money)
            cmd.AddOutParameter("@base_amount_unrounded", SqlDbType.Money)

            cmd.AddOutParameter("@account_currency_id", SqlDbType.Int)
            cmd.AddOutParameter("@account_amount", SqlDbType.Money)
            cmd.AddOutParameter("@account_amount_unrounded", SqlDbType.Money)

            cmd.AddOutParameter("@system_currency_id", SqlDbType.Int)
            cmd.AddOutParameter("@system_amount", SqlDbType.Money)
            cmd.AddOutParameter("@system_amount_unrounded", SqlDbType.Money)

            cmd.AddInOutParameter("@currency_base_xrate", SqlDbType.Float)
            cmd.AddInOutParameter("@currency_base_date", SqlDbType.DateTime).Value = Today

            cmd.AddInOutParameter("@account_base_xrate", SqlDbType.Float)
            cmd.AddInOutParameter("@account_base_date", SqlDbType.DateTime).Value = Today

            cmd.AddInOutParameter("@system_base_xrate", SqlDbType.Float)
            cmd.AddInOutParameter("@system_base_date", SqlDbType.DateTime).Value = Today

            cmd.AddOutParameter("@return_status", SqlDbType.SmallInt)

            con.ExecuteNonQuery(cmd)

            ' Check the return value
            If Cast.ToInt32(cmd.Parameters("@return_status").Value, 0) = 1 Then

                dCurrencyBaseExchangeRate = Cast.ToDouble(cmd.Parameters("@currency_base_xrate").Value, 0)
                dtCurrencyBaseDate = Cast.ToDateTime(cmd.Parameters("@currency_base_date").Value, Date.MinValue)

                dSystemBaseExchangeRate = Cast.ToDouble(cmd.Parameters("@system_base_xrate").Value, 0)
                dtSystemBaseDate = Cast.ToDateTime(cmd.Parameters("@system_base_date").Value, Date.MinValue)
                iCurrencyId = Cast.ToInt32(cmd.Parameters("@base_currency_id").Value, 0)

            End If

        End Using

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_update_insurance_file")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = InsuranceFileCnt
            cmd.AddInParameter("@currency_base_xrate", SqlDbType.Float).Value = dCurrencyBaseExchangeRate
            cmd.AddInParameter("@currency_base_date", SqlDbType.DateTime).Value = dtCurrencyBaseDate

            cmd.AddInParameter("@account_base_xrate", SqlDbType.Float).Value = System.DBNull.Value

            cmd.AddInParameter("@account_base_date", SqlDbType.DateTime).Value = New Date(1899, 12, 29)

            cmd.AddInParameter("@system_base_xrate", SqlDbType.Float).Value = dSystemBaseExchangeRate
            cmd.AddInParameter("@system_base_date", SqlDbType.DateTime).Value = dtSystemBaseDate

            cmd.AddInParameter("@exchange_rate_override_reason_id", SqlDbType.Int).Value = System.DBNull.Value
            cmd.AddInParameter("@base_currency_id", SqlDbType.Int).Value = iCurrencyId

            cmd.AddInParameter("@agent_account_currency_id", SqlDbType.Int).Value = System.DBNull.Value

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    Private Overloads Function GetPoliciesInRenewalDataset(ByVal con As SiriusConnection,
                                  ByVal oRequest As BaseGetPoliciesInRenewalRequestType
                                  ) As DataSet

        Dim ds As DataSet = Nothing

        'If Direct buiness needs to be retrieved, then AgentKey should not be provided

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Policies_In_Renewal")
            If oRequest.PartyKey <> 0 Then
                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = oRequest.PartyKey
            End If
            If oRequest.AgentKey <> 0 AndAlso Not oRequest.DirectOnly Then
                cmd.AddInParameter("@agent_cnt", SqlDbType.Int).Value = oRequest.AgentKey
            End If

            cmd.AddInParameter("@source_id", SqlDbType.Int).Value = oRequest.SourceId

            If oRequest.ProductKey <> 0 Then
                cmd.AddInParameter("@product_id", SqlDbType.Int).Value = oRequest.ProductKey
            End If
            If oRequest.RenewalDate > Date.MinValue Then
                cmd.AddInParameter("@renewal_date", SqlDbType.DateTime).Value = oRequest.RenewalDate
            End If
            If oRequest.ForAccept Then
                cmd.AddInParameter("@forAccept", SqlDbType.TinyInt).Value = IIf(oRequest.ForAccept, 1, 0)
            End If
            If oRequest.DirectOnly Then
                cmd.AddInParameter("@OnlyDirect", SqlDbType.TinyInt).Value = IIf(oRequest.DirectOnly, 1, 0)
            End If
            'WPR73-74
            If oRequest.InsuranceRef IsNot Nothing Then
                cmd.AddInParameter("@InsuranceRef", SqlDbType.VarChar).Value = oRequest.InsuranceRef
            End If
            cmd.AddInParameter("@RetrieveAssociates", SqlDbType.TinyInt).Value = IIf(oRequest.RetrieveAssociates = True, 1, 0)

            ds = con.ExecuteDataSet(cmd, "Row")
            If oRequest.RetrieveAssociates Then
                AddExtraXMLElementForPolicyAssociate(ds, "AssociatedClients")
            End If
        End Using

        Return ds

    End Function

    'This function will generate a new policy number and udpate into the database
    Private Sub ChangePolicyNumber(ByVal con As SiriusConnection,
                                  ByVal iBusinessTypeID As Integer,
                                  ByVal iSourceID As Short,
                                  ByVal iProductID As Integer,
                                  ByVal iAgentKey As Integer,
                                  ByVal dtCoverStartDate As Date,
                                  ByVal iInsuranceFileCnt As Integer,
                                  ByRef sNewInsuranceFileRef As String,
                                  ByVal nPartyKey As Integer)

        Dim oGenPolBO As bSIRPolicyNumMaint.Business = Nothing
        Dim icomReturnValue As Integer
        oGenPolBO = New bSIRPolicyNumMaint.Business
        SAMFunc.InitialiseSBOObject(con, oGenPolBO, _SiriusUser, "bSIRPolicyNumMaint.Business")
        icomReturnValue = oGenPolBO.GeneratePolicyNumber(v_lBusinessType:=iBusinessTypeID,
                                                     v_iBranch:=iSourceID,
                                                     v_lProductId:=iProductID,
                                                     v_lAgent:=iAgentKey,
                                                     r_sGeneratedPolicyNumber:=sNewInsuranceFileRef,
                                                     v_dtTransactionDate:=dtCoverStartDate,
                                                     v_lPartyCnt:=nPartyKey)
        If icomReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRPolicyNumMaint.Business.GeneratePolicyNumber", icomReturnValue)
        End If

        'Update the New Insurance Ref to the InsuranceFile table
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Change_Insurance_File_Ref")
            cmd.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = iInsuranceFileCnt
            cmd.AddInParameter("@Insurance_Ref", SqlDbType.VarChar, 30).Value = sNewInsuranceFileRef
            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ''' <summary>
    ''' This method is used to validate the bankGuarantee details
    '''</summary>
    '''<param name="conBankGuarantee">An object of  a class SiriusConnection</param>
    '''<param name="oRequestBankGuaranteePayment">An object of  a class BaseBankGuaranteePaymentType</param>
    ''' <param name="oRequest" > An object of  a class BaseBindQuoteRequestType</param>
    ''' <returns>nothing will be returned</returns>
    '''<remarks></remarks>
    Private Sub ValidateBankGuaranteeTypeData(
        ByVal conBankGuarantee As SiriusConnection,
        ByVal oRequestBankGuaranteePayment As BaseBankGuaranteePaymentType,
        ByVal oRequest As BaseBindQuoteRequestType)

        Dim dsBankGuarantee As DataSet = Nothing
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim dTotalPremium As Decimal

        oRequestBankGuaranteePayment.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()

        Using cmdBankGuarantee As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Validate_BG_for_Policy")
            cmdBankGuarantee.AddInParameter("@product_id", SqlDbType.Int).Value = Cast.NullIfDefault(oRequest.DataStore.ProductId, 0)
            cmdBankGuarantee.AddInParameter("@Source_id", SqlDbType.Int).Value = Cast.NullIfDefault(oRequest.DataStore.SourceId, 0)
            cmdBankGuarantee.AddInParameter("@cover_from_date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oRequest.DataStore.CoverStartDate, Nothing)
            cmdBankGuarantee.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oRequest.InsuranceFileKey, 0)
            cmdBankGuarantee.AddInParameter("@Transaction_Curreny_Id", SqlDbType.Int).Value = Cast.NullIfDefault(oRequest.DataStore.CurrencyId, 0)
            cmdBankGuarantee.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oRequest.DataStore.PartyKey, 0)
            cmdBankGuarantee.AddInParameter("@Lead_Agent_Cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oRequest.DataStore.LeadAgnetKey, 0)
            cmdBankGuarantee.AddInParameter("@bg_id", SqlDbType.Int).Value = oRequestBankGuaranteePayment.BGKey
            cmdBankGuarantee.AddInParameter("@total_inf_premium", SqlDbType.Decimal).Value = Cast.NullIfDefault(GetTotalPremiumAmount(conBankGuarantee, oRequest.InsuranceFileKey), 0)
            cmdBankGuarantee.AddInParameter("@user", SqlDbType.Int).Value = Cast.NullIfDefault(_SiriusUser.UserID, 0)
            dsBankGuarantee = conBankGuarantee.ExecuteDataSet(cmdBankGuarantee, "BankGuarantee")
        End Using

        If (dsBankGuarantee IsNot Nothing) Then
            If (dsBankGuarantee.Tables.Count > 0) Then
                With dsBankGuarantee.Tables(0).Rows(0)
                    If (Cast.ToInt32(.Item("Validate_BG"), 0) <> 1) Then
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidBankGuaranteeKey,
                         SAMBusinessErrors.InValidBankGuaranteeKey.ToString())
                    Else
                        If (Cast.ToInt32(.Item("Validate_Bal"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidAvailableBalance,
                            SAMBusinessErrors.InValidAvailableBalance.ToString())
                        End If
                        If (Cast.ToInt32(.Item("Validate_Product"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidProductKey,
                            SAMBusinessErrors.InValidProductKey.ToString())
                        End If
                        If (Cast.ToInt32(.Item("Validate_Branch"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidBranchId,
                             SAMBusinessErrors.InValidBranchId.ToString())
                        End If
                        If (Cast.ToInt32(.Item("Validate_Tran_Currency"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidCurrency,
                             SAMBusinessErrors.InValidCurrency.ToString())
                        End If
                        If (Cast.ToInt32(.Item("Validate_Cover_From"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidCoverFromDate,
                             SAMBusinessErrors.InValidCoverFromDate.ToString())
                        End If
                        If (Cast.ToInt32(.Item("Validate_Deleted"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.RecordIsDeleted,
                             SAMBusinessErrors.RecordIsDeleted.ToString())
                        End If
                        If (Cast.ToInt32(.Item("Validate_Status"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidBankGuaranteeStatus,
                             SAMBusinessErrors.InValidBankGuaranteeStatus.ToString())
                        End If

                        If (Cast.ToInt32(.Item("Validate_Party_Agent"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidPartyAgent,
                             SAMBusinessErrors.InValidPartyAgent.ToString())
                        End If
                        If (Cast.ToInt32(.Item("Validate_User"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidUser,
                             SAMBusinessErrors.InValidUser.ToString())
                        End If
                        If (Cast.ToInt32(.Item("Validate_Product_Access"), 0) <> 1) Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidProduct,
                             SAMBusinessErrors.InValidProduct.ToString())
                        End If
                    End If
                End With
            End If
        End If
        oSAMErrorCollection.CheckForErrors()
    End Sub

    ''' <summary>
    ''' This method is used to add the record in the insurance_File_BG_Link table
    '''</summary>
    '''<param name="conBankGuarantee">An object of  a class SiriusConnection</param>
    '''<param name="oRequestBankGuaranteePayment">An object of  a class BaseBankGuaranteePaymentType</param>
    ''' <param name="oRequest" > An object of  a class BaseBindQuoteRequestType</param>
    ''' <returns>nothing will be returned</returns>
    '''<remarks></remarks>
    Private Sub ProcessBankGuarantee(ByVal conBankGuarantee As SiriusConnection,
                                     ByVal oRequestBankGuaranteePayment As BaseBankGuaranteePaymentType,
                                     ByVal oRequest As BaseBindQuoteRequestType)
        Using cmdBankGuarantee As SiriusCommand = SiriusCommand.FromProcedure("spu_PolicyBG_Details_Add")
            cmdBankGuarantee.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oRequest.InsuranceFileKey, 0)
            cmdBankGuarantee.AddInParameter("@Amount", SqlDbType.Decimal).Value = GetTotalPremiumAmount(conBankGuarantee, oRequest.InsuranceFileKey)
            cmdBankGuarantee.AddInParameter("@bg_id", SqlDbType.Int).Value = Cast.NullIfDefault(oRequestBankGuaranteePayment.BGKey, 0)
            cmdBankGuarantee.AddInParameter("@Cover_From_Date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oRequest.DataStore.CoverStartDate, Nothing)
            conBankGuarantee.ExecuteNonQuery(cmdBankGuarantee)
        End Using
    End Sub

    ''' <summary>
    ''' This is core sam  method for  Get Balances And Unallocated Credits
    '''<param name="GetBalancesAndUnallocatedCreditsRequest" type="BaseGetBalancesAndUnallocatedCreditsRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  
    Public Overloads Function GetBalancesAndUnallocatedCredits(ByVal oGetBalancesAndUnallocatedCreditsRequest As BaseGetBalancesAndUnallocatedCreditsRequestType) As BaseGetBalancesAndUnallocatedCreditsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As BaseGetBalancesAndUnallocatedCreditsResponseType

            oResponse = GetBalancesAndUnallocatedCredits(con, oGetBalancesAndUnallocatedCreditsRequest)

            Return oResponse

        End Using

    End Function
    Public Overloads Function GetBalancesAndUnallocatedCredits(ByVal con As SiriusConnection, ByVal oGetBalancesAndUnallocatedCreditsRequest As BaseGetBalancesAndUnallocatedCreditsRequestType) As BaseGetBalancesAndUnallocatedCreditsResponseType

        Dim oGetBalancesAndUnallocatedCreditsResponse As New BaseGetBalancesAndUnallocatedCreditsResponseType
        Dim oErrors As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetBalancesAndUnallocatedCreditsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetBalancesAndUnallocatedCreditsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetBalancesAndUnallocatedCreditsResponse = New SAMForInsuranceV2ImplementationTypes.GetBalancesAndUnallocatedCreditsResponseType
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oGetBalancesAndUnallocatedCreditsRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                        SAMInvalidData.MandatoryInputMissing.ToString,
                                        "BranchCode")
        End If

        If oGetBalancesAndUnallocatedCreditsRequest.InsuranceFileKey = 0 Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                        SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InsuranceFileKey")
        End If

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        Dim dsAccountBalance As DataSet = Nothing
        Dim dsUnAllocatedCreditForAgents As DataSet = Nothing
        Dim dsUnAllocatedCreditForClients As DataSet = Nothing
        Dim dsInsuranceDetails As DataSet = Nothing
        Dim DocUnAllocatedCreditForAgents As New System.Xml.XmlDocument
        Dim DocUnAllocatedCreditForClients As New System.Xml.XmlDocument

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Insurance_File_sel")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oGetBalancesAndUnallocatedCreditsRequest.InsuranceFileKey
            dsInsuranceDetails = con.ExecuteDataSet(cmd, "Row")
        End Using

        If dsInsuranceDetails IsNot Nothing AndAlso dsInsuranceDetails.Tables.Count > 0 AndAlso dsInsuranceDetails.Tables(0).Rows.Count > 0 Then
            oGetBalancesAndUnallocatedCreditsResponse.InsuranceRef = dsInsuranceDetails.Tables(0).Rows(0)("insurance_ref").ToString()
            oGetBalancesAndUnallocatedCreditsResponse.AgentKey = Cast.ToInt32(dsInsuranceDetails.Tables(0).Rows(0)("lead_agent_cnt"), 0)
            oGetBalancesAndUnallocatedCreditsResponse.ClientKey = Cast.ToInt32(dsInsuranceDetails.Tables(0).Rows(0)("Insured_Cnt"), 0)

        End If

        If oGetBalancesAndUnallocatedCreditsResponse.AgentKey > 0 Then
            Dim dsAgentDetails As DataSet = Nothing
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_get_Agent_Details")
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oGetBalancesAndUnallocatedCreditsRequest.InsuranceFileKey
                dsAgentDetails = con.ExecuteDataSet(cmd, "Row")
            End Using
            If dsAgentDetails IsNot Nothing AndAlso dsAgentDetails.Tables.Count > 0 AndAlso dsAgentDetails.Tables(0).Rows.Count > 0 Then
                oGetBalancesAndUnallocatedCreditsResponse.AgentType = dsAgentDetails.Tables(0).Rows(0)("Code").ToString().Trim()
                oGetBalancesAndUnallocatedCreditsResponse.IsFloatBalanceAccount = Cast.ToBoolean(dsAgentDetails.Tables(0).Rows(0)("is_float_balance_account"), False)
                oGetBalancesAndUnallocatedCreditsResponse.IsOverDraftAccount = Cast.ToBoolean(dsAgentDetails.Tables(0).Rows(0)("is_overdraft_account"), False)
                oGetBalancesAndUnallocatedCreditsResponse.FloatBalanceLimit = Cast.ToDouble(dsAgentDetails.Tables(0).Rows(0)("float_balance_limit"), 0.0)
                oGetBalancesAndUnallocatedCreditsResponse.OverDraftLimit = Cast.ToDouble(dsAgentDetails.Tables(0).Rows(0)("overDraft_limit"), 0.0)
                oGetBalancesAndUnallocatedCreditsResponse.OverDraftExpiry = Cast.ToDateTime(dsAgentDetails.Tables(0).Rows(0)("Overdraft_expiry"), Nothing)
            End If

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_ACT_Select_AccountBal")
                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = oGetBalancesAndUnallocatedCreditsResponse.AgentKey
                dsAccountBalance = con.ExecuteDataSet(cmd, "Row")
            End Using

            If dsAccountBalance IsNot Nothing AndAlso dsAccountBalance.Tables.Count > 0 AndAlso dsAccountBalance.Tables(0).Rows.Count > 0 Then
                oGetBalancesAndUnallocatedCreditsResponse.AccountBalance = Cast.ToDouble(dsAccountBalance.Tables(0).Rows(0)("SumAmount"), 0.0)
                oGetBalancesAndUnallocatedCreditsResponse.OverDraftLimit = Cast.ToDouble(dsAccountBalance.Tables(0).Rows(0)("Overdraft"), 0.0)
                oGetBalancesAndUnallocatedCreditsResponse.FloatBalanceLimit = Cast.ToDouble(dsAccountBalance.Tables(0).Rows(0)("FloatBalance"), 0.0)
            End If

            If oGetBalancesAndUnallocatedCreditsResponse.AgentType.Trim() = "Broker" OrElse oGetBalancesAndUnallocatedCreditsResponse.AgentType.Trim() = "Intermed" Then
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_UnAllocated_Credit")
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oGetBalancesAndUnallocatedCreditsRequest.InsuranceFileKey
                    cmd.AddInParameter("@party_type", SqlDbType.VarChar, 10).Value = "AGENT"
                    dsUnAllocatedCreditForAgents = con.ExecuteDataSet(cmd, "Row")
                End Using
            End If
            If dsUnAllocatedCreditForAgents IsNot Nothing AndAlso dsUnAllocatedCreditForAgents.Tables.Count > 0 AndAlso dsUnAllocatedCreditForAgents.Tables(0).Rows.Count > 0 Then

                dsUnAllocatedCreditForAgents.Tables(0).Columns(0).ColumnName = "TransDetailKey"
                dsUnAllocatedCreditForAgents.Tables(0).Columns(1).ColumnName = "DocumentRef"
                dsUnAllocatedCreditForAgents.Tables(0).Columns(2).ColumnName = "MediaType"
                dsUnAllocatedCreditForAgents.Tables(0).Columns(3).ColumnName = "Reference"
                dsUnAllocatedCreditForAgents.Tables(0).Columns(4).ColumnName = "Amount"
                dsUnAllocatedCreditForAgents.Tables(0).Columns(5).ColumnName = "AccountKey"

                dsUnAllocatedCreditForAgents.Tables(0).Columns(6).ColumnName = "CollectionDate"

                dsUnAllocatedCreditForAgents.DataSetName = "BaseGetBalancesAndUnallocatedCreditsResponseTypeUnallocatedCreditsForAgents"
                dsUnAllocatedCreditForAgents.Tables(0).TableName = "Row"
                If oGetBalancesAndUnallocatedCreditsRequest.WCFSecurityToken = "" Then
                    DocUnAllocatedCreditForAgents.LoadXml(dsUnAllocatedCreditForAgents.GetXml)
                    oGetBalancesAndUnallocatedCreditsResponse.ResultDatasetForAgents = DocUnAllocatedCreditForAgents.DocumentElement()
                End If

                oGetBalancesAndUnallocatedCreditsResponse.ResultDataForAgent = dsUnAllocatedCreditForAgents
            End If

        End If

        If oGetBalancesAndUnallocatedCreditsResponse.AgentKey = 0 OrElse String.Compare(oGetBalancesAndUnallocatedCreditsResponse.AgentType, "Broker", True) <> 0 Then

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_ACT_Select_AccountBal")
                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = oGetBalancesAndUnallocatedCreditsResponse.ClientKey
                dsAccountBalance = con.ExecuteDataSet(cmd, "Row")
            End Using
            If dsAccountBalance IsNot Nothing AndAlso dsAccountBalance.Tables.Count > 0 AndAlso dsAccountBalance.Tables(0).Rows.Count > 0 Then
                oGetBalancesAndUnallocatedCreditsResponse.AccountBalance = Cast.ToDouble(dsAccountBalance.Tables(0).Rows(0)("SumAmount"), 0.0)
            End If

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_UnAllocated_Credit")
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oGetBalancesAndUnallocatedCreditsRequest.InsuranceFileKey
                cmd.AddInParameter("@party_type", SqlDbType.VarChar, 10).Value = "CLIENT"
                dsUnAllocatedCreditForClients = con.ExecuteDataSet(cmd, "Row")
            End Using

            If dsUnAllocatedCreditForClients IsNot Nothing AndAlso dsUnAllocatedCreditForClients.Tables.Count > 0 AndAlso dsUnAllocatedCreditForClients.Tables(0).Rows.Count > 0 Then
                dsUnAllocatedCreditForClients.Tables(0).Columns(0).ColumnName = "TransDetailKey"
                dsUnAllocatedCreditForClients.Tables(0).Columns(1).ColumnName = "DocumentRef"
                dsUnAllocatedCreditForClients.Tables(0).Columns(2).ColumnName = "MediaType"
                dsUnAllocatedCreditForClients.Tables(0).Columns(3).ColumnName = "Reference"
                dsUnAllocatedCreditForClients.Tables(0).Columns(4).ColumnName = "Amount"
                dsUnAllocatedCreditForClients.Tables(0).Columns(5).ColumnName = "AccountKey"

                dsUnAllocatedCreditForClients.Tables(0).Columns(6).ColumnName = "CollectionDate"

                dsUnAllocatedCreditForClients.DataSetName = "BaseGetBalancesAndUnallocatedCreditsResponseTypeUnallocatedCreditsForClients"
                dsUnAllocatedCreditForClients.Tables(0).TableName = "Row"
                If oGetBalancesAndUnallocatedCreditsRequest.WCFSecurityToken = "" Then
                    DocUnAllocatedCreditForClients.LoadXml(dsUnAllocatedCreditForClients.GetXml)
                    oGetBalancesAndUnallocatedCreditsResponse.ResultDatasetForClients = DocUnAllocatedCreditForClients.DocumentElement()
                End If
                oGetBalancesAndUnallocatedCreditsResponse.ResultDataForClient = dsUnAllocatedCreditForClients
            End If
        End If
        Return oGetBalancesAndUnallocatedCreditsResponse
    End Function
    Private Function GetProductID(ByVal insuranceFileCnt As Int64, ByRef con As SiriusConnection) As Integer
        Dim iProductId As Integer = 0
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Product_ID")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileCnt
            iProductId = Cast.ToInt32(con.ExecuteScalar(cmd), 0)
        End Using

        Return iProductId
    End Function

    ''' <summary>  
    '''  This method is used to update a Quote.
    ''' </summary>  
    ''' <param name="oUpdateQuoteV2Request">An object of type SiriusFS.SAM.BaseImplementationTypes.BaseUpdateQuoteV2RequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.BaseImplementationTypes.BaseUpdateQuoteV2ResponseType</returns>  
    Public Overloads Function UpdateQuoteV2(ByVal oUpdateQuoteV2Request As BaseUpdateQuoteV2RequestType) As BaseUpdateQuoteV2ResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseUpdateQuoteV2ResponseType
            oResponse = UpdateQuoteV2(con, oUpdateQuoteV2Request)
            Return oResponse
        End Using
    End Function

    ''' <summary>  
    '''  This method is used to update a Quote.
    ''' </summary>  
    ''' <param name="con">An object of type Sirius.Architecture.Data.SiriusConnection</param>  
    ''' <param name="UpdateQuoteV2Request">An object of type SiriusFS.SAM.BaseImplementationTypes.BaseUpdateQuoteV2RequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.BaseImplementationTypes.BaseUpdateQuoteV2ResponseType</returns>  
    Public Overloads Function UpdateQuoteV2(ByVal con As SiriusConnection,
                                         ByVal oUpdateQuoteV2Request As BaseUpdateQuoteV2RequestType) _
     As BaseUpdateQuoteV2ResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim oUpdateQuoteV2Response As BaseUpdateQuoteV2ResponseType
        Const nInsuranceFileStructureId As Integer = 1
        Dim nBusinessTypeId As Integer
        Dim nBranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oCoreBusiness As New CoreBusiness
        Dim oGenChangePolBO As bSIRChangePolicyStatus.Business = Nothing
        Dim sInsuranceFileRef As String = Nothing
        Dim ncomReturnValue As Integer
        Dim oIntialCoverStartDate As Object = Nothing
        Dim dtIntialCoverStartDate As Date
        Dim bIstrueMonthlyPolicy As Boolean
        Dim bIsMidNightRenewal As Boolean
        Dim nUnifiedRenewalDay As Integer
        Dim dtAnniversaryDate As Date
        Dim oIntialCoverExpiryDate As Object = Nothing
        Dim dtOldCoverEndDate As Date
        Dim dtIntialCoverExpiryDate As Date
        Dim nIsNBProRata As Integer = 0
        Dim nUnderWritingYearID As Integer = 0
        Dim oIntialCoverEndDate As Object = Nothing
        Dim nIsMTAProRata As Integer = 0
        Dim iGracePeriod As Integer
        ' determine the type of package and thus the type of response
        If oUpdateQuoteV2Request.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateQuoteV2RequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateQuoteV2Response = New SAMForInsuranceV2ImplementationTypes.UpdateQuoteV2ResponseType
        Else
            oUpdateQuoteV2Response = New BaseImplementationTypes.BaseUpdateQuoteV2ResponseType
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oUpdateQuoteV2Request.BranchCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "BranchCode")
        End If
        If (oUpdateQuoteV2Request.CoverStartDate <= Date.MinValue) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "CoverStartDate")
        End If
        If (oUpdateQuoteV2Request.CoverEndDate <= Date.MinValue) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "CoverEndDate")
        End If
        If String.IsNullOrEmpty(oUpdateQuoteV2Request.ProductCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "ProductCode")
        End If
        If String.IsNullOrEmpty(oUpdateQuoteV2Request.InsuredName) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InsuredName")
        End If
        If String.IsNullOrEmpty(oUpdateQuoteV2Request.CurrencyCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "CurrencyCode")
        End If
        If (oUpdateQuoteV2Request.InsuranceFolderKey = 0) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InsuranceFolderKey")
        End If
        If (oUpdateQuoteV2Request.InsuranceFileKey = 0) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InsuranceFileKey")
        End If
        If (oUpdateQuoteV2Request.PartyKey = 0) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "PartyKey")
        End If
        If String.IsNullOrEmpty(oUpdateQuoteV2Request.SubBranchCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "SubBranchCode")
        End If
        If String.IsNullOrEmpty(oUpdateQuoteV2Request.BusinessTypeCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "BusinessTypeCode")
        End If
        If (oUpdateQuoteV2Request.QuoteExpiryDate <= Date.MinValue) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "QuoteExpiryDate")
        End If
        If oUpdateQuoteV2Request.PolicyStatusCode Is Nothing Then oUpdateQuoteV2Request.PolicyStatusCode = ""

        If (oUpdateQuoteV2Request.InceptionDate <= Date.MinValue) AndAlso
          oUpdateQuoteV2Request.PolicyStatusCode.ToString().ToUpper() <> "REN" Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                        SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InceptionDate")
        End If
        If (oUpdateQuoteV2Request.RenewalDate <= Date.MinValue) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "RenewalDate")
        End If
        If (oUpdateQuoteV2Request.InceptionTPI <= Date.MinValue) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InceptionTPI")
        End If
        If String.IsNullOrEmpty(oUpdateQuoteV2Request.FrequencyCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                        "FrequencyCode")
        End If
        oSAMErrorCollection.CheckForErrors()

        ' Lookup Codes to ensure they are valid
        nBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                 PMLookupTable.Source, oUpdateQuoteV2Request.BranchCode,
                                    "BranchCode", oSAMErrorCollection)

        Dim nProductId As Integer
        nProductId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                  "Product", oUpdateQuoteV2Request.ProductCode,
                                                                  "ProductCode",
                                oSAMErrorCollection)

        Dim nCurrencyId As Integer
        nCurrencyId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                   PMLookupTable.Currency,
                                                                   oUpdateQuoteV2Request.CurrencyCode,
                        "CurrencyCode")

        If (oUpdateQuoteV2Request.BusinessTypeCode.ToUpper <> "DIRECT" And oUpdateQuoteV2Request.BusinessTypeCode.ToUpper <> "COIN FOLL" And oUpdateQuoteV2Request.BusinessTypeCode.ToUpper <> "COIN LEAD") Then
            If (oUpdateQuoteV2Request.AgentKeySpecified) Then
                If oCoreBusiness.CheckAgentKey(oUpdateQuoteV2Request.AgentKey) = False Then
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                       "Agent Key validation failed", "AgentKey",
                                                       oUpdateQuoteV2Request.AgentKey.ToString)
                End If
            Else
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "Agent Key validation failed",
                                                   "AgentKey", oUpdateQuoteV2Request.AgentKey.ToString)
            End If
        End If

        Dim nAnalysisId As Integer
        If Not (String.IsNullOrEmpty(oUpdateQuoteV2Request.AnalysisCode)) Then
            nAnalysisId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                       PMLookupTable.AnalysisCode,
                                                                       oUpdateQuoteV2Request.AnalysisCode,
                                    "AnalysisCode", oSAMErrorCollection)
        End If

        Dim nSubBranchId As Integer
        nSubBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                    "Sub_Branch", oUpdateQuoteV2Request.SubBranchCode,
                                                                    "SubBranchCode",
                        oSAMErrorCollection)

        If Not (String.IsNullOrEmpty(oUpdateQuoteV2Request.CoverNoteBookNumber)) Then
            If Not (oUpdateQuoteV2Request.CoverNoteSheetNumberSpecified) Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "CoverNoteSheetNumber",
                                                   oUpdateQuoteV2Request.CoverNoteSheetNumber.ToString)
            ElseIf (Convert.ToInt32(oUpdateQuoteV2Request.CoverNoteSheetNumber) = 0) Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "CoverNoteSheetNumber",
                                                   oUpdateQuoteV2Request.CoverNoteSheetNumber.ToString)
            End If
        End If

        If (oUpdateQuoteV2Request.CoverNoteSheetNumberSpecified) Then
            If String.IsNullOrEmpty(oUpdateQuoteV2Request.CoverNoteBookNumber) Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                        "CoverNoteBookNumber", oUpdateQuoteV2Request.CoverNoteBookNumber)
            End If
        End If

        With oUpdateQuoteV2Request
            If _
                .CoverNoteBookNumber <> String.Empty And
                ((.CoverNoteSheetNumberSpecified = True) And .CoverNoteSheetNumber > 0) Then
                Dim validationResult As Integer = 0
                Dim sheetStatus As String = String.Empty
                IsValidCoverNote(con, oSAMErrorCollection, .InsuranceFileKey, .CoverNoteBookNumber, .CoverNoteSheetNumber,
                                 nBranchId, .AgentKey, nProductId, validationResult, sheetStatus)
            End If
        End With

        nBusinessTypeId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                       "business_type",
                                                                       oUpdateQuoteV2Request.BusinessTypeCode,
                                                                       "BusinessTypeCode",
                            oSAMErrorCollection)

        Dim nHandlerId As Integer
        If Not (String.IsNullOrEmpty(oUpdateQuoteV2Request.HandlerCode)) Then
            nHandlerId = GetAndValidateSpecifiedTableCode(con, "Party", "party_cnt", "shortname",
                                                          oUpdateQuoteV2Request.HandlerCode, oSAMErrorCollection,
                                                          "HandlerCode")
        End If

        Dim nPolicyStatusID As Integer
        

        If Not (String.IsNullOrEmpty(oUpdateQuoteV2Request.PolicyStatusCode)) Then
            nPolicyStatusID = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                           PMLookupTable.PolicyStatus,
                                                                           oUpdateQuoteV2Request.PolicyStatusCode,
                                "PolicyStatusCode", oSAMErrorCollection)
        End If

        Dim nFrequencyId As Integer
        nFrequencyId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                    "Renewal_Frequency",
                                                                    oUpdateQuoteV2Request.FrequencyCode, "FrequencyCode",
                            oSAMErrorCollection)

        Dim nRenewalMethodId As Integer
        If Not (String.IsNullOrEmpty(oUpdateQuoteV2Request.RenewalMethodCode)) Then
            nRenewalMethodId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                            "Renewal_Method",
                                                                            oUpdateQuoteV2Request.RenewalMethodCode,
                                                "RenewalMethodCode", oSAMErrorCollection)
        End If

        Dim nLapseCancelReasonId As Integer
        If Not (String.IsNullOrEmpty(oUpdateQuoteV2Request.LapseCancelReasonCode)) Then
            nLapseCancelReasonId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                "Lapsed_Reason",
                                                                                oUpdateQuoteV2Request.
                                                                                   LapseCancelReasonCode,
                                                                                "LapseCancelReasonCode",
                                                                                oSAMErrorCollection)
        End If

        Dim nStopReasonId As Integer
        If Not (String.IsNullOrEmpty(oUpdateQuoteV2Request.StopReasonCode)) Then
            nStopReasonId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                         "Renewal_stop_code",
                                                                         oUpdateQuoteV2Request.StopReasonCode,
                                "StopReasonCode", oSAMErrorCollection)
        End If

        Dim lCorrespondenceTypeID As Integer
        If Not (String.IsNullOrEmpty(oUpdateQuoteV2Request.CorrespondenceType)) Then
            lCorrespondenceTypeID = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                "Correspondence_Type", oUpdateQuoteV2Request.CorrespondenceType, "CorrespondenceType",
                                oSAMErrorCollection)
        End If

        Dim lDefaultPreferredCorrespondenceTypeID As Integer
        If Not (String.IsNullOrEmpty(oUpdateQuoteV2Request.DefaultPreferredCorrespondence)) Then
            lDefaultPreferredCorrespondenceTypeID = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                "Contact_Type", oUpdateQuoteV2Request.DefaultPreferredCorrespondence, "DefaultPreferredCorrespondence",
                                oSAMErrorCollection)
        End If

        Dim sEnableUnderwritingYear As String = ""
        sEnableUnderwritingYear = oCoreBusiness.GetProductOption(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, 1)
        If sEnableUnderwritingYear = "1" AndAlso oUpdateQuoteV2Request.UnderwritingYearId = 0 Then

            If Not String.IsNullOrEmpty(oUpdateQuoteV2Request.UnderwritingYearCode) Then
                Try
                    nUnderWritingYearID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup,
                                                                            "Underwriting_Year",
                                                                            oUpdateQuoteV2Request.UnderwritingYearCode)
                    ValidateAndGetValidUnderwritingYear(con, nUnderWritingYearID, oUpdateQuoteV2Request.CoverStartDate,
                                                        oCoreBusiness, oSAMErrorCollection)

                Catch ex As Exception
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                       SAMInvalidData.InvalidLookupListValue.ToString,
                                                       "Underwriting_Year",
                                                            oUpdateQuoteV2Request.UnderwritingYearCode)
                End Try
            ElseIf (String.IsNullOrEmpty(oUpdateQuoteV2Request.UnderwritingYearCode)) Then
                GetDefaultUnderwritingYear(con, nUnderWritingYearID, oUpdateQuoteV2Request.CoverStartDate, oCoreBusiness,
                                           oSAMErrorCollection)
                oUpdateQuoteV2Request.UnderwritingYearId = nUnderWritingYearID
                oUpdateQuoteV2Request.UnderwritingYearIdSpecified = True

            End If
        End If

        oSAMErrorCollection.CheckForErrors()

        Dim nInsuranceFileTypeId As Integer
        Dim sInsuranceFileTypeCode As String = Nothing
        GetInsuranceFileType(con, oUpdateQuoteV2Request.InsuranceFileKey, nInsuranceFileTypeId, sInsuranceFileTypeCode)

        If sInsuranceFileTypeCode = InsuranceFileType.LivePolicy Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CanNotUpdateLivePolicyDetails,
                                                SAMBusinessErrors.CanNotUpdateLivePolicyDetails.ToString,
                "Can Not Update a Live Poilcy")
        End If

        Dim oGetProductRiskOptionValueRequest As New BaseProductRiskOptionValueRequestType
        If nProductId > 0 Then
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.IsTrueMonthlyPolicy
            bIstrueMonthlyPolicy = Cast.ToBoolean(Convert.ToInt32(GetProductRiskOptions(con, nProductId, oGetProductRiskOptionValueRequest)), False)
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.IsMidnightRenewal
            bIsMidNightRenewal = Cast.ToBoolean(Convert.ToInt32(GetProductRiskOptions(con, nProductId, oGetProductRiskOptionValueRequest)), False)
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.UnifiedRenewalDay
            If GetProductRiskOptions(con, nProductId, oGetProductRiskOptionValueRequest).Trim = "" Then
                nUnifiedRenewalDay = 0
            Else
                nUnifiedRenewalDay = Convert.ToInt32(GetProductRiskOptions(con, nProductId,
                                                                           oGetProductRiskOptionValueRequest))
                oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.MTAProrata
                nIsMTAProRata = Convert.ToInt32(GetProductRiskOptions(con, nProductId, oGetProductRiskOptionValueRequest))
            End If
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.NBProrata
            nIsNBProRata = Convert.ToInt32(GetProductRiskOptions(con, nProductId, oGetProductRiskOptionValueRequest))
        End If

        If _
            oUpdateQuoteV2Request.RenewalDayNo = 0 AndAlso oUpdateQuoteV2Request.RenewalDayNoSpecified AndAlso
            bIstrueMonthlyPolicy AndAlso nUnifiedRenewalDay <> 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "RenewalDayNo")
        End If

        oSAMErrorCollection.CheckForErrors()

        UpdateMulticurrencyValues(con, oUpdateQuoteV2Request.InsuranceFileKey, nBranchId,
                                  nCurrencyId)

        Dim AnyError As STSErrorType
        AnyError = oCoreBusiness.CheckTSAndLock(con,
                                                BranchCode:=oUpdateQuoteV2Request.BranchCode,
                                                Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
                                                LockValue:=oUpdateQuoteV2Request.InsuranceFolderKey,
            TStamp:=oUpdateQuoteV2Request.Timestamp)

        If AnyError Is Nothing = False Then
            oUpdateQuoteV2Response.STSError = AnyError
            Return oUpdateQuoteV2Response
        End If
        If oUpdateQuoteV2Request.InceptionDate = DateTime.MinValue Then
            Dim dr As DataSet = Nothing
            Using cmd As SiriusCommand = SiriusCommand.FromText("SELECT cover_start_date FROM Insurance_File WHERE insurance_folder_cnt = " + oUpdateQuoteV2Request.InsuranceFolderKey.ToString() + " AND policy_version = 1")
                dr = con.ExecuteDataSet(cmd, "dr")
            End Using
            oUpdateQuoteV2Request.InceptionDate = Cast.ToDateTime(dr.Tables(0).Rows(0).Item(0), DateTime.MinValue)
        End If

        If bIstrueMonthlyPolicy Then
            If oUpdateQuoteV2Request.AnniversaryDateSpecified Then
                If oUpdateQuoteV2Request.AnniversaryDate <= Date.MinValue Then
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "AnniversaryDate")
                Else
                    dtAnniversaryDate = oUpdateQuoteV2Request.AnniversaryDate
                End If
            Else
                GetAnniversaryDate(bIstrueMonthlyPolicy, oUpdateQuoteV2Request.InceptionDate, oUpdateQuoteV2Request.RenewalDayNo,
                                   bIsMidNightRenewal, dtAnniversaryDate)
            End If
        Else
            GetAnniversaryDate(bIstrueMonthlyPolicy, oUpdateQuoteV2Request.InceptionDate, oUpdateQuoteV2Request.RenewalDayNo,
                               bIsMidNightRenewal, dtAnniversaryDate)
        End If

        Try

            oIntialCoverStartDate = GetAndValidateDescriptionById(con, "insurance_file", "cover_start_date",
                                                                  "insurance_file_cnt",
                                                                  oUpdateQuoteV2Request.InsuranceFileKey.ToString())
            If IsDate(oIntialCoverStartDate) Then
                dtIntialCoverStartDate = Cast.ToDateTime(CDate(oIntialCoverStartDate), DateTime.MinValue)
            End If

            oIntialCoverExpiryDate = GetAndValidateDescriptionById(con, "insurance_file", "expiry_date",
                                                                   "insurance_file_cnt",
                                                                   oUpdateQuoteV2Request.InsuranceFileKey.ToString())
            If IsDate(oIntialCoverExpiryDate) Then
                dtIntialCoverExpiryDate = Cast.ToDateTime(CDate(oIntialCoverExpiryDate), DateTime.MinValue)
            End If

            If _
                (dtIntialCoverStartDate <> oUpdateQuoteV2Request.CoverStartDate) And
                (nInsuranceFileTypeId = 1 Or nInsuranceFileTypeId = 0) Then

                'Generate the policy number
                oGenChangePolBO = New bSIRChangePolicyStatus.Business
                SAMFunc.InitialiseSBOObject(con, oGenChangePolBO, _SiriusUser, "bSIRChangePolicyStatus.Business")
                ncomReturnValue = oGenChangePolBO.CheckPeriodStatus(v_lBusinessType:=BusinessType.Quote,
                                                                    v_iBranch:=Cast.ToInt16(nBranchId, 0),
                                                                    v_lProductId:=nProductId,
                                                                    v_lAgent:=oUpdateQuoteV2Request.AgentKey,
                                                                    r_sGeneratedPolicyNumber:=sInsuranceFileRef,
                                                                    v_dtInitialCoverStartDate:=dtIntialCoverStartDate,
                                                                    v_dtCurrentStartDate:=
                                                                       oUpdateQuoteV2Request.CoverStartDate)
                If ncomReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRChangePolicyStatus.Business.CheckPeriodStatus", ncomReturnValue)
                End If
                If Not (sInsuranceFileRef Is Nothing) And Trim(sInsuranceFileRef) <> "" Then
                    oUpdateQuoteV2Request.QuoteRef = sInsuranceFileRef
                    oUpdateQuoteV2Response.InsuranceFileRef = sInsuranceFileRef
                Else
                    oUpdateQuoteV2Response.InsuranceFileRef = oUpdateQuoteV2Request.QuoteRef
                End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_update_risk_status_unquoted")
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value =
                        oUpdateQuoteV2Request.InsuranceFileKey
                    con.ExecuteNonQuery(cmd)
                End Using

            Else
                oUpdateQuoteV2Response.InsuranceFileRef = oUpdateQuoteV2Request.QuoteRef
            End If

            oIntialCoverEndDate = GetAndValidateDescriptionById(con, "insurance_file", "expiry_date",
                                                                "insurance_file_cnt",
                                                                oUpdateQuoteV2Request.InsuranceFileKey.ToString())
            If IsDate(oIntialCoverStartDate) Then
                dtOldCoverEndDate = Cast.ToDateTime(CDate(oIntialCoverEndDate), DateTime.MinValue)
            End If


            If (((oUpdateQuoteV2Request.CoverEndDate <> dtOldCoverEndDate) And (sInsuranceFileTypeCode = InsuranceFileType.MTAPermanent Or sInsuranceFileTypeCode = "MTAQUOTE" Or sInsuranceFileTypeCode = InsuranceFileType.Renewal Or sInsuranceFileTypeCode = "MTAQREINS")) _
                Or ((oUpdateQuoteV2Request.CoverEndDate <> dtOldCoverEndDate) And (sInsuranceFileTypeCode = InsuranceFileType.Quote Or sInsuranceFileTypeCode = InsuranceFileType.Written)) Or (oUpdateQuoteV2Request.CoverStartDate <> dtIntialCoverStartDate)) Then
                Dim sTransactionType As BaseImplementationTypes.TransactionType = TransactionType.NB
                If sInsuranceFileTypeCode = InsuranceFileType.MTAPermanent Or sInsuranceFileTypeCode = "MTAQUOTE" Then
                    sTransactionType = TransactionType.MTA
                ElseIf sInsuranceFileTypeCode = InsuranceFileType.Renewal Then
                    sTransactionType = TransactionType.REN
                ElseIf sInsuranceFileTypeCode = "MTAQREINS" Then
                    sTransactionType = TransactionType.MTR
                End If
                ProcessCopyMTARisks(con, oUpdateQuoteV2Request.BranchCode, oUpdateQuoteV2Request.InsuranceFileKey, sTransactionType, False, True, True)

                'PN 79487
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_sir_del_risks_ri")
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oUpdateQuoteV2Request.InsuranceFileKey
                    con.ExecuteNonQuery(cmd)
                End Using
            End If
            If sInsuranceFileTypeCode <> InsuranceFileType.Renewal Then
                If (oUpdateQuoteV2Request.QuoteExpiryDate < DateTime.Now.Date) Then
                    oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.GracePeriod
                    If GetProductRiskOptions(con, nProductId, oGetProductRiskOptionValueRequest).Trim = "" Then
                        iGracePeriod = 0
                    Else
                        iGracePeriod = Convert.ToInt32(GetProductRiskOptions(con, nProductId, oGetProductRiskOptionValueRequest))
                    End If
                    oUpdateQuoteV2Request.QuoteExpiryDate = DateTime.Today.AddDays(CDbl(iGracePeriod))

                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Update_Risk_Status")
                        cmd.AddInParameter("@risk_status_code", SqlDbType.VarChar, 20).Value = "UNQUOTED"
                        cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oUpdateQuoteV2Request.InsuranceFileKey
                        cmd.AddInParameter("@ApplyCheck", SqlDbType.Int).Value = 1
                        con.ExecuteNonQuery(cmd)
                    End Using
                End If
            End If
            oUpdateQuoteV2Response.QuoteExpiryDate = oUpdateQuoteV2Request.QuoteExpiryDate
            con.BeginTransaction()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Insurance_File_upd")
                With oUpdateQuoteV2Request
                    cmd.AddInParameter("@source_id", SqlDbType.SmallInt).Value = Cast.ToInt16(nBranchId)
                    cmd.AddInParameter("@cover_start_date", SqlDbType.DateTime).Value = .CoverStartDate
                    cmd.AddInParameter("@expiry_date", SqlDbType.DateTime).Value = .CoverEndDate
                    cmd.AddInParameter("@product_id", SqlDbType.Int).Value = nProductId
                    cmd.AddInParameter("@description", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(.Description)
                    cmd.AddInParameter("@insurance_ref", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(.QuoteRef)
                    cmd.AddInParameter("@insured_name", SqlDbType.VarChar, 255).Value = .InsuredName
                    cmd.AddInParameter("@currency_id", SqlDbType.SmallInt).Value = Cast.ToInt16(nCurrencyId)
                    If (.BusinessTypeCode.ToUpper <> "DIRECT") And .AgentKey <> 0 Then
                        cmd.AddInParameter("@lead_agent_cnt", SqlDbType.Int).Value = .AgentKey
                    Else
                        cmd.AddInParameter("@lead_agent_cnt", SqlDbType.Int).Value = SqlInt32.Null
                    End If
                    cmd.AddInParameter("@analysis_code_id", SqlDbType.Int).Value = Cast.NullIfDefault(nAnalysisId, 0)
                    cmd.AddInParameter("@alternate_reference", SqlDbType.VarChar, 80).Value =
                        Cast.NullIfDefault(.AlternativeRef)
                    cmd.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = .InsuranceFolderKey
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = .InsuranceFileKey
                    cmd.AddInParameter("@insured_cnt", SqlDbType.Int).Value = .PartyKey
                    cmd.AddInParameter("@branch_id", SqlDbType.SmallInt).Value = Cast.ToInt16(nSubBranchId)
                    If (.ConsolidatedLeadAgentCommissionSpecified) Then
                        cmd.AddInParameter("@lead_allow_consolidated_commission", SqlDbType.TinyInt).Value =
                            IIf(.ConsolidatedLeadAgentCommission, 1, 0)
                    Else
                        cmd.AddInParameter("@lead_allow_consolidated_commission", SqlDbType.TinyInt).Value = 0
                    End If
                    If (.ConsolidatedSubAgentCommissionSpecified) Then
                        cmd.AddInParameter("@sub_allow_consolidated_commission", SqlDbType.TinyInt).Value =
                            IIf(.ConsolidatedSubAgentCommission, 1, 0)
                    Else
                        cmd.AddInParameter("@sub_allow_consolidated_commission", SqlDbType.TinyInt).Value = 0
                    End If
                    cmd.AddInParameter("@business_type_id", SqlDbType.SmallInt).Value = Cast.ToInt16(nBusinessTypeId)
                    cmd.AddInParameter("@quote_expiry_date", SqlDbType.DateTime).Value = .QuoteExpiryDate
                    cmd.AddInParameter("@account_handler_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(nHandlerId, 0)
                    cmd.AddInParameter("@regarding", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(.Regarding)
                    cmd.AddInParameter("@policy_status_id", SqlDbType.Int).Value = Cast.NullIfDefault(nPolicyStatusID, 0)
                    cmd.AddInParameter("@inception_date", SqlDbType.DateTime).Value = .InceptionDate
                    cmd.AddInParameter("@renewal_date", SqlDbType.DateTime).Value = .RenewalDate
                    cmd.AddInParameter("@inception_date_tpi", SqlDbType.DateTime).Value = .InceptionTPI
                    If (.IssuedDateSpecified) Then
                        cmd.AddInParameter("@date_issued", SqlDbType.DateTime).Value = .IssuedDate
                    Else
                        cmd.AddInParameter("@date_issued", SqlDbType.DateTime).Value = Date.Now
                    End If
                    If (.ProposalDateSpecified) Then
                        cmd.AddInParameter("@proposal_date", SqlDbType.DateTime).Value = .ProposalDate
                    Else
                        cmd.AddInParameter("@proposal_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                    End If
                    cmd.AddInParameter("@renewal_frequency_id", SqlDbType.SmallInt).Value = Cast.ToInt16(nFrequencyId)
                    cmd.AddInParameter("@renewal_method_id", SqlDbType.Int).Value = Cast.NullIfDefault(nRenewalMethodId,
                                                                                                       0)
                    cmd.AddInParameter("@lapsed_reason_id", SqlDbType.Int).Value =
                        Cast.NullIfDefault(nLapseCancelReasonId, 0)
                    If (.LTUExpiryDateSpecified) Then
                        cmd.AddInParameter("@long_term_undertaking_date", SqlDbType.DateTime).Value = .LTUExpiryDate
                    Else
                        cmd.AddInParameter("@long_term_undertaking_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                    End If
                    cmd.AddInParameter("@renewal_stop_code_id", SqlDbType.Int).Value = Cast.NullIfDefault(nStopReasonId,
                                                                                                          0)
                    If (.LapseCancelDateSpecified) Then
                        cmd.AddInParameter("@lapsed_date", SqlDbType.DateTime).Value = .LapseCancelDate
                    Else
                        cmd.AddInParameter("@lapsed_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                    End If
                    If (.ReferredAtRenewalSpecified) Then
                        cmd.AddInParameter("@is_referred_at_renewal", SqlDbType.TinyInt).Value = IIf(.ReferredAtRenewal,
                                                                                                     1, 0)
                    Else
                        cmd.AddInParameter("@is_referred_at_renewal", SqlDbType.TinyInt).Value = 0
                    End If
                    If (.ReferredAtMTASpecified) Then
                        cmd.AddInParameter("@is_referred_on_mta", SqlDbType.TinyInt).Value = IIf(.ReferredAtRenewal, 1,
                                                                                                 0)
                    Else
                        cmd.AddInParameter("@is_referred_on_mta", SqlDbType.TinyInt).Value = 0
                    End If
                    cmd.AddInParameter("@modified_by_id", SqlDbType.Int).Value = _SiriusUser.UserID
                    cmd.AddInParameter("@insurance_file_structure_id", SqlDbType.Int).Value = nInsuranceFileStructureId
                    cmd.AddInParameter("@payment_method", SqlDbType.VarChar, 60).Value =
                        Cast.NullIfDefault(.PaymentMethod)
                    cmd.AddInParameter("@insurance_file_type_id", SqlDbType.Int).Value =
                        Cast.NullIfDefault(nInsuranceFileTypeId, 0)

                    If (.MarkedForCollectionSpecified) Then
                        cmd.AddInParameter("@marked_for_collection", SqlDbType.TinyInt).Value = IIf(.MarkedForCollection,
                                                                                                    1, 0)
                    Else
                        cmd.AddInParameter("@marked_for_collection", SqlDbType.TinyInt).Value = DBNull.Value
                    End If
                    If (.MarkedDateSpecified) Then
                        cmd.AddInParameter("@marked_date", SqlDbType.DateTime).Value = .MarkedDate
                    Else
                        cmd.AddInParameter("@marked_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                    End If
                    'Begin - WPR36
                    cmd.AddInParameter("@put_on_next_instalment_renewal", SqlDbType.TinyInt).Value =
                        IIf(.PutOnNextInstalmentRenewalSpecified, IIf(.PutOnNextInstalmentRenewal, 1, 0), 0)
                    cmd.AddInParameter("@Anniversary_Date", SqlDbType.DateTime).Value = dtAnniversaryDate
                    If (.RenewalDayNoSpecified) Then
                        cmd.AddInParameter("@renewal_day_number", SqlDbType.Int).Value =
                            Cast.NullIfDefault(.RenewalDayNo, 0)
                    End If

                    If (.UnderwritingYearIdSpecified) Then
                        cmd.AddInParameter("@underwriting_year_id", SqlDbType.Int).Value = .UnderwritingYearId
                    Else
                        cmd.AddInParameter("@underwriting_year_id", SqlDbType.Int).Value = SqlInt32.Null
                    End If

                    If (.ContactuserKeySpecified) Then
                        cmd.AddInParameter("@contact_user_id", SqlDbType.Int).Value = .ContactuserKey
                    Else
                        cmd.AddInParameter("@contact_user_id", SqlDbType.Int).Value = SqlInt32.Null
                    End If
                    cmd.AddInParameter("@OldPolicyNumber", SqlDbType.VarChar, 30).Value =
                        oUpdateQuoteV2Request.OldPolicyNumber
                    cmd.AddInParameter("@bIs_Marketplace_Policy", SqlDbType.Int).Value = IIf(.IsMarketPlacePolicy, 1, 0)

                    If (.collectionFrequencySpecified) Then
                        cmd.AddInParameter("@nCollectionFrequency", SqlDbType.Int).Value = Cast.NullIfDefault(.collectionFrequency, 0)
                    End If
                    If (.paymentTermsSpecified) Then
                        cmd.AddInParameter("@nPaymentTerms", SqlDbType.Int).Value = Cast.NullIfDefault(.paymentTerms, 0)
                    End If
                    cmd.AddInParameter("@coins_placement", SqlDbType.VarChar, 10).Value = oUpdateQuoteV2Request.CoInsurancePlacement
                    cmd.AddInParameter("@Correspondence_Type", SqlDbType.Int).Value = Cast.NullIfDefault(lCorrespondenceTypeID, 0)
                    cmd.AddInParameter("@Default_Preferred_Correspondence", SqlDbType.Int).Value = Cast.NullIfDefault(lDefaultPreferredCorrespondenceTypeID, 0)
                    cmd.AddInParameter("@Is_Agent_Correspondence", SqlDbType.TinyInt).Value = IIf(.IsAgentReceiveCorrespondence, 1, 0)
                    cmd.AddInParameter("@Sender_Email", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(.SenderEmail)
                    cmd.AddInParameter("@Receiver_Email", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(.ReceiverEmail)
                End With

                con.ExecuteNonQuery(cmd)

                With oUpdateQuoteV2Request
                    If .CoverNoteBookNumber <> String.Empty And .CoverNoteSheetNumber > 0 Then
                        AssignCoverNoteSheet(con, .InsuranceFileKey, .CoverNoteBookNumber, .CoverNoteSheetNumber)
                    End If
                End With

            End Using

            UpdateInsuranceFileSystem(con, _SiriusUser.UserID, oUpdateQuoteV2Request.InsuranceFileKey, "NB", False,
                                      oUpdateQuoteV2Request.Regarding)
            If oUpdateQuoteV2Request.AgentKeySpecified Then
                AddSubAgent(con, oUpdateQuoteV2Request.InsuranceFileKey, oUpdateQuoteV2Request.AgentKey)
            End If

            Dim dsInsuranceDetails As DataSet = Nothing
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_quote_status_sel")
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oUpdateQuoteV2Request.InsuranceFileKey
                dsInsuranceDetails = con.ExecuteDataSet(cmd, "Row")
            End Using
            If dsInsuranceDetails IsNot Nothing AndAlso dsInsuranceDetails.Tables.Count > 0 AndAlso
                dsInsuranceDetails.Tables(0).Rows.Count > 0 Then
                oUpdateQuoteV2Response.BaseInsuranceFolderKey =
                    Cast.ToInt32(dsInsuranceDetails.Tables(0).Rows(0)("base_insurance_folder_cnt"), 0)
                oUpdateQuoteV2Response.QuoteVersion = Cast.ToInt32(dsInsuranceDetails.Tables(0).Rows(0)("quote_version"),
                                                                   0)
                Select Case dsInsuranceDetails.Tables(0).Rows(0)("quote_status_id").ToString()

                    Case "0"
                        oUpdateQuoteV2Response.QuoteStatusKey = QuoteStatusType.None
                    Case "1"
                        oUpdateQuoteV2Response.QuoteStatusKey = QuoteStatusType.Pending
                    Case "2"
                        oUpdateQuoteV2Response.QuoteStatusKey = QuoteStatusType.AgentPending
                    Case "3"
                        oUpdateQuoteV2Response.QuoteStatusKey = QuoteStatusType.AgentComplete
                    Case "4"
                        oUpdateQuoteV2Response.QuoteStatusKey = QuoteStatusType.Issued
                    Case "5"
                        oUpdateQuoteV2Response.QuoteStatusKey = QuoteStatusType.Live
                    Case "6"
                        oUpdateQuoteV2Response.QuoteStatusKey = QuoteStatusType.Declined
                    Case Else
                        oUpdateQuoteV2Response.QuoteStatusKey = QuoteStatusType.None
                End Select

            End If

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Recalculate_Policy_Fees")
                cmd.AddInParameter("@Transaction_type_id", SqlDbType.Int).Value = -1
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oUpdateQuoteV2Request.InsuranceFileKey
                cmd.AddInParameter("@product_id", SqlDbType.Int).Value = -1
                If oUpdateQuoteV2Request.paymentTermsSpecified Then
                    If oUpdateQuoteV2Request.paymentTerms = 1 Then
                        cmd.AddInParameter("@sPaymentDebitOrCash", SqlDbType.VarChar).Value = "Cash"
                    ElseIf oUpdateQuoteV2Request.paymentTerms = 2 Then
                        cmd.AddInParameter("@sPaymentDebitOrCash", SqlDbType.VarChar).Value = "DebitOrder"
                    End If
                End If
                cmd.AddInParameter("@nViaSAM", SqlDbType.Int).Value = 1
                con.ExecuteNonQuery(cmd)
            End Using

            If Not oGenChangePolBO Is Nothing Then
                oGenChangePolBO.Dispose()
                oGenChangePolBO = Nothing
            End If

            con.CommitTransaction()

        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try

        AnyError = oCoreBusiness.UnlockAndGetTS(con,
                                                BranchCode:=oUpdateQuoteV2Request.BranchCode,
                                                Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
                                                LockValue:=oUpdateQuoteV2Request.InsuranceFolderKey,
            TStamp:=oUpdateQuoteV2Response.TimeStamp)

        If AnyError Is Nothing = False Then
            oUpdateQuoteV2Response.STSError = AnyError
            Return oUpdateQuoteV2Response
        End If

        Return oUpdateQuoteV2Response
    End Function

    Private Sub GetAllocationDetailsForCashDeposit(
                        ByVal conCashDeposit As SiriusConnection,
                        ByVal oRequest As BaseBindQuoteRequestType,
                        ByRef vCreditTransactions As Object(,))
        Dim dtAllocationDetails As DataTable = Nothing

        If oRequest.SelectedCashDeposit.TotalPremium > 0 Then
            Using cmdCashDeposit As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_CDReceipts_For_Allocation")
                cmdCashDeposit.AddInParameter("@CashDeposit_ID", SqlDbType.Int).Value = Cast.ToInt32(oRequest.SelectedCashDeposit.CashDepositID, 0)
                cmdCashDeposit.AddInParameter("@Total_Premium", SqlDbType.Money).Value = Cast.NullIfDefault(oRequest.SelectedCashDeposit.TotalPremium)
                If oRequest.DataStore.IsPrePayment Then
                    cmdCashDeposit.AddInParameter("@Is_PrePayment", SqlDbType.TinyInt).Value = 1
                Else
                    cmdCashDeposit.AddInParameter("@Is_PrePayment", SqlDbType.TinyInt).Value = 0
                End If
                cmdCashDeposit.AddInParameter("@Cover_Start_Date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oRequest.DataStore.CoverStartDate)
                cmdCashDeposit.AddInParameter("@Policy_Issue_Date", SqlDbType.DateTime).Value = Date.Now
                cmdCashDeposit.AddInParameter("@Retrieve_Reciepts", SqlDbType.TinyInt).Value = 1
                cmdCashDeposit.AddOutParameter("@Is_Valid", SqlDbType.TinyInt)

                dtAllocationDetails = conCashDeposit.ExecuteDataTable(cmdCashDeposit)

            End Using
            'Prakash: As per the expected functionality, if the refund MTA or cancellation occurs,
            'we need not have to allocate the refund amount back to the cashDeposit account. So, the 
            'following sp call (spu_Get_CDReceipts_For_Refund) is not needed
            'ElseIf oRequest.SelectedCashDeposit.TotalPremium < 0 AndAlso _
            '        (oRequest.TransactionType = TransactionTypeCode.MTA OrElse _
            '         oRequest.TransactionType = TransactionTypeCode.CancelPolicy) Then
            '    dtAllocationDetails = Nothing
            'Using cmdCashDeposit As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_CDReceipts_For_Refund")
            '    cmdCashDeposit.AddInParameter("@CashDeposit_ID", SqlDbType.Int).Value = Cast.ToInt32(oRequest.SelectedCashDeposit.CashDepositID, 0)
            '    cmdCashDeposit.AddInParameter("@Total_Premium", SqlDbType.Money).Value = Cast.NullIfDefault(oRequest.SelectedCashDeposit.TotalPremium)

            '    dtAllocationDetails = conCashDeposit.ExecuteDataTable(cmdCashDeposit)

        End If

        If dtAllocationDetails IsNot Nothing AndAlso dtAllocationDetails.Rows.Count > 0 Then
            ReDim Preserve vCreditTransactions(2, dtAllocationDetails.Rows.Count - 1)

            For iCnt As Integer = 0 To dtAllocationDetails.Rows.Count - 1
                vCreditTransactions(0, iCnt) = Cast.ToInt32(dtAllocationDetails.Rows(iCnt).Item("Account_ID"), 0)
                vCreditTransactions(1, iCnt) = Cast.ToInt32(dtAllocationDetails.Rows(iCnt).Item("TransDetail_ID"), 0)
                vCreditTransactions(2, iCnt) = Cast.ToDecimal(dtAllocationDetails.Rows(iCnt).Item("Amount_Allocated"), 0)
            Next
        Else
            vCreditTransactions = Nothing
        End If
    End Sub

    Private Sub ValidateCashDeposit(
        ByVal conCashDeposit As SiriusConnection,
        ByVal oRequest As BaseBindQuoteRequestType,
        ByVal oBusiness As CoreBusiness)

        Dim dtCashDeposit As DataTable = Nothing
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim sAccountStatus As String = String.Empty
        Dim dAvailableBalance As Decimal = 0
        Dim dRunningBalance As Decimal = 0

        'Check mandatory validation
        oRequest.SelectedCashDeposit.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()

        'Check multiple payment method specified
        If oRequest.SelectedInstalmentQuoteSpecified Or oRequest.PayNowDetails IsNot Nothing Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.MultiplePaymentMethodSpecified,
                         SAMBusinessErrors.MultiplePaymentMethodSpecified.ToString())
        End If

        'Get cash deposit account details
        Using cmdCashDeposit As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_CashDeposit_Account_Details")
            cmdCashDeposit.AddInParameter("@CashDeposit_Ref", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oRequest.SelectedCashDeposit.CashDepositRef, String.Empty)
            dtCashDeposit = conCashDeposit.ExecuteDataTable(cmdCashDeposit)
        End Using

        If (dtCashDeposit IsNot Nothing AndAlso dtCashDeposit.Rows.Count > 0) Then
            With dtCashDeposit.Rows(0)
                oRequest.SelectedCashDeposit.CashDepositID = Cast.ToInt32(.Item("CashDeposit_ID"), 0)
                oRequest.SelectedCashDeposit.PartyKey = Cast.ToInt32(.Item("Party_ID"), 0)
                oRequest.SelectedCashDeposit.CashDepositAccountID = Cast.ToInt32(.Item("Account_ID"), 0)
                sAccountStatus = Cast.ToString(.Item("Account_Status"), String.Empty)
            End With
        Else
            oSAMErrorCollection.AddInvalidData(
                                SAMConstants.SAMInvalidData.RecordNotFound,
                                "Failed to get cash deposit account details",
                                "CashDepsoitRef",
                                oRequest.SelectedCashDeposit.CashDepositRef)

        End If
        oSAMErrorCollection.CheckForErrors()

        'Data validation
        If oRequest.SelectedCashDeposit.CashDepositID = 0 OrElse
            oRequest.SelectedCashDeposit.CashDepositAccountID = 0 OrElse
            sAccountStatus = String.Empty Then
            oSAMErrorCollection.AddInvalidData(
                              SAMInvalidData.InvalidLookupListValue,
                              SAMInvalidData.InvalidLookupListValue.ToString,
                              "CashDepositRef",
                              oRequest.SelectedCashDeposit.CashDepositRef)
        End If
        oSAMErrorCollection.CheckForErrors()

        'Set the premium amount
        oRequest.SelectedCashDeposit.TotalPremium = oRequest.DataStore.TotalPremiumAmount

        'If Direct business, check the cash deposit belongs to insured
        If Not oRequest.DataStore.PolicyHasAgent Then
            If oRequest.DataStore.PartyKey <> oRequest.SelectedCashDeposit.PartyKey Then
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CashDepositAccountDoesNotBelongToInsured,
                                         SAMBusinessErrors.CashDepositAccountDoesNotBelongToInsured.ToString())
            End If
        Else
            'Agency business. If agent type is broker, check the cash deposit belongs to agent
            If oRequest.DataStore.AgentType.ToLower = "broker" Then
                oRequest.SelectedCashDeposit.TotalPremium = oRequest.DataStore.TotalPremiumAmount - oRequest.DataStore.TotalAgentCommission - -oRequest.DataStore.TotalAgentCommissionTax

                If oRequest.DataStore.LeadAgnetKey <> oRequest.SelectedCashDeposit.PartyKey Then
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CashDepositAccountDoesNotBelongToAgent,
                                         SAMBusinessErrors.CashDepositAccountDoesNotBelongToAgent.ToString())
                End If
            ElseIf oRequest.DataStore.AgentType.ToLower = "comm acc" Then
                If oRequest.DataStore.PartyKey <> oRequest.SelectedCashDeposit.PartyKey Then
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CashDepositAccountDoesNotBelongToInsured,
                                             SAMBusinessErrors.CashDepositAccountDoesNotBelongToInsured.ToString())
                End If
            ElseIf oRequest.DataStore.AgentType.ToLower = "intermed" Then
                If oRequest.DataStore.LeadAgnetKey <> oRequest.SelectedCashDeposit.PartyKey AndAlso oRequest.DataStore.PartyKey <> oRequest.SelectedCashDeposit.PartyKey Then
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CashDepositAccountDoesNotBelongToInsured,
                                             SAMBusinessErrors.CashDepositAccountDoesNotBelongToInsured.ToString())
                End If
                If oRequest.DataStore.LeadAgnetKey = oRequest.SelectedCashDeposit.PartyKey Then
                    oRequest.SelectedCashDeposit.TotalPremium = oRequest.DataStore.TotalPremiumAmount - oRequest.DataStore.TotalAgentCommission - -oRequest.DataStore.TotalAgentCommissionTax
                End If
            End If
        End If
        oSAMErrorCollection.CheckForErrors()

        'Check cash deposit account status
        If Trim(sAccountStatus) <> AccountStatusCode.Active Then
            oSAMErrorCollection.AddBusinessRule(
                        SAMBusinessErrors.PolicyCannotBeMadeLiveAsTheSelectedCashDepositAccountHasBeenStopped,
                        SAMBusinessErrors.PolicyCannotBeMadeLiveAsTheSelectedCashDepositAccountHasBeenStopped.ToString())

            oSAMErrorCollection.CheckForErrors()
        End If

        'Get the prepayment option
        oRequest.DataStore.IsPrePayment = oBusiness.GetProductOption(SIRHiddenOptions.SIROPTEnablePayNowOptions, oRequest.SourceId) = "1"
        Using cmdCashDeposit As SiriusCommand = SiriusCommand.FromProcedure("spu_Convert_Policy_Amount_To_Base_Currency")
            cmdCashDeposit.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oRequest.InsuranceFileKey)
            cmdCashDeposit.AddInParameter("@Policy_Amount", SqlDbType.Money).Value = Cast.NullIfDefault(oRequest.SelectedCashDeposit.TotalPremium)
            cmdCashDeposit.AddOutParameter("@Base_Amount", SqlDbType.Money)
            cmdCashDeposit.AddOutParameter("@Base_Currency_ID", SqlDbType.Int)
            cmdCashDeposit.AddOutParameter("@Base_Currency_Code", SqlDbType.VarChar, 20)
            cmdCashDeposit.AddOutParameter("@Transaction_Currency_ID", SqlDbType.Int)
            cmdCashDeposit.AddOutParameter("@Transaction_Currency_Code", SqlDbType.VarChar, 20)
            conCashDeposit.ExecuteNonQuery(cmdCashDeposit)
            oRequest.SelectedCashDeposit.TotalPremium = Cast.ToDecimal(cmdCashDeposit.Parameters.Item("@Base_Amount").Value, 0)
        End Using
        'Check the cash deposit account has enough balance for allocation if the premium is greater than zero
        If oRequest.DataStore.TotalPremiumAmount > 0 Then
            Using cmdCashDeposit As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Balance_For_CD")
                cmdCashDeposit.AddInParameter("@CashDeposit_ID", SqlDbType.Int).Value = Cast.NullIfDefault(oRequest.SelectedCashDeposit.CashDepositID)
                If oRequest.DataStore.IsPrePayment Then
                    cmdCashDeposit.AddInParameter("@Is_PrePayment", SqlDbType.TinyInt).Value = 1
                Else
                    cmdCashDeposit.AddInParameter("@Is_PrePayment", SqlDbType.TinyInt).Value = 0
                End If
                cmdCashDeposit.AddInParameter("@Cover_Start_Date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oRequest.DataStore.CoverStartDate)
                cmdCashDeposit.AddInParameter("@Policy_Issue_Date", SqlDbType.DateTime).Value = Date.Now
                cmdCashDeposit.AddOutParameter("@Available_Balance", SqlDbType.Money)
                cmdCashDeposit.AddOutParameter("@Running_Balance", SqlDbType.Money)

                conCashDeposit.ExecuteNonQuery(cmdCashDeposit)

                dAvailableBalance = Cast.ToDecimal(cmdCashDeposit.Parameters.Item("@Available_Balance").Value, 0)
                dRunningBalance = Cast.ToDecimal(cmdCashDeposit.Parameters.Item("@Running_Balance").Value, 0)

                If dAvailableBalance < oRequest.SelectedCashDeposit.TotalPremium Then
                    oSAMErrorCollection.AddBusinessRule(
                                                SAMBusinessErrors.InsufficientBalanceInCashDepositAccount,
                                                SAMBusinessErrors.InsufficientBalanceInCashDepositAccount.ToString())

                    oSAMErrorCollection.CheckForErrors()
                ElseIf oRequest.DataStore.IsPrePayment And dRunningBalance < oRequest.SelectedCashDeposit.TotalPremium Then
                    oSAMErrorCollection.AddBusinessRule(
                                                SAMBusinessErrors.InsufficientRunningBalanceInCashDepositAccount,
                                                SAMBusinessErrors.InsufficientRunningBalanceInCashDepositAccount.ToString())

                    oSAMErrorCollection.CheckForErrors()
                End If
            End Using
        End If
    End Sub

    Public Overloads Function GetPolicyDetailsForBouncedReceipt(ByVal GetPolicyDetailsForBouncedReceiptRequest As BaseGetPolicyDetailsForBouncedReceiptRequestType) As BaseGetPolicyDetailsForBouncedReceiptResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As New BaseGetPolicyDetailsForBouncedReceiptResponseType
            oResponse = GetPolicyDetailsForBouncedReceipt(con:=con,
                                                        oRequest:=GetPolicyDetailsForBouncedReceiptRequest)
            Return oResponse
        End Using

    End Function

    Public Overloads Function GetPolicyDetailsForBouncedReceipt(ByRef con As SiriusConnection,
                                                            ByVal oRequest As BaseGetPolicyDetailsForBouncedReceiptRequestType) As BaseGetPolicyDetailsForBouncedReceiptResponseType

        Dim oGetPolicyDetailsForBouncedReceiptResponse As New BaseGetPolicyDetailsForBouncedReceiptResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim iSubBranchId As Integer = 0
        Dim dsResult As DataSet = Nothing

        Dim nTypeOfPackage As enumTypeOfPackage

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPolicyDetailsForBouncedReceiptRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetPolicyDetailsForBouncedReceiptResponse = New SAMForInsuranceV2ImplementationTypes.GetPolicyDetailsForBouncedReceiptResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        oRequest.Validate(CType(oErrors, SAMErrorCollection))
        oErrors.CheckForErrors()

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Policy_Details_For_Bounced_Receipt")
            cmd.AddInParameter("@DocumentRef", SqlDbType.VarChar).Value = oRequest.DocumentRef
            cmd.AddInParameter("@MediaRef", SqlDbType.VarChar).Value = IIf(oRequest.MediaRef <> String.Empty, oRequest.MediaRef, DBNull.Value)
            cmd.AddInParameter("@InsuranceRef", SqlDbType.VarChar).Value = IIf(oRequest.InsuranceRef <> String.Empty, oRequest.InsuranceRef, DBNull.Value)
            dsResult = con.ExecuteDataSet(cmd, "GetPolicyDetailsForBouncedReceipt")
        End Using

        If (dsResult.Tables.Count > 0) Then

            If (dsResult.Tables(0).Rows.Count > 0) Then

                'Reformat the data set the get only the elements needed
                Dim xmlDoc As New System.Xml.XmlDocument
                dsResult.DataSetName = "BaseGetPolicyDetailsForBouncedReceiptResponseTypePolicies"
                dsResult.Tables(0).TableName = "Row"

                If oRequest.WCFSecurityToken = "" Then
                    xmlDoc.LoadXml(dsResult.GetXml)
                    If (dsResult.Tables.Count >= 1) Then
                        'Populate the XML Document here with the dsUserGroups information
                        oGetPolicyDetailsForBouncedReceiptResponse.ResultDataSet = xmlDoc.DocumentElement()
                    End If
                End If
                oGetPolicyDetailsForBouncedReceiptResponse.ResultData = dsResult

            End If
        End If

        Return oGetPolicyDetailsForBouncedReceiptResponse

    End Function

    Public Overloads Function GetQuotesMarkedForCollection(ByVal GetQuotesMarkedForCollectionRequest As BaseGetQuotesMarkedForCollectionRequestType) As BaseGetQuotesMarkedForCollectionResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As New BaseGetQuotesMarkedForCollectionResponseType
            oResponse = GetQuotesMarkedForCollection(con:=con,
                                                        oRequest:=GetQuotesMarkedForCollectionRequest)
            Return oResponse
        End Using

    End Function

    Public Overloads Function GetQuotesMarkedForCollection(ByRef con As SiriusConnection,
                                                            ByVal oRequest As BaseGetQuotesMarkedForCollectionRequestType) As BaseGetQuotesMarkedForCollectionResponseType

        Dim oGetQuotesMarkedForCollectionResponse As New BaseGetQuotesMarkedForCollectionResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim iSubBranchId As Integer = 0
        Dim dsResult As DataSet = Nothing

        Dim nTypeOfPackage As enumTypeOfPackage

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetQuotesMarkedForCollectionRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetQuotesMarkedForCollectionResponse = New SAMForInsuranceV2ImplementationTypes.GetQuotesMarkedForCollectionResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        oRequest.Validate(CType(oErrors, SAMErrorCollection))
        oErrors.CheckForErrors()

        Dim productCodes As String = ""

        With oRequest
            If .Products IsNot Nothing Then
                Dim products() As BaseImplementationTypes.BaseGetQuotesMarkedForCollectionRequestTypeProducts
                ReDim products(.Products.GetUpperBound(0))
                Dim productId As Integer = 0
                For cntItems As Integer = .Products.GetLowerBound(0) To .Products.GetUpperBound(0)
                    If .Products(cntItems) IsNot Nothing Then
                        productId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Product, .Products(cntItems).ProductCode, "ProductCode", oErrors)
                        productCodes = productCodes & productId.ToString & IIf((cntItems < .Products.GetUpperBound(0)), ",", "").ToString
                    End If
                Next
            End If
        End With

        oErrors.CheckForErrors()

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Quotes_Marked_ForCollection")
            cmd.AddInParameter("@client_id", SqlDbType.Int).Value = IIf(oRequest.PartyKeySpecified, oRequest.PartyKey, DBNull.Value)
            cmd.AddInParameter("@insurancefilecnt", SqlDbType.Int).Value = IIf(oRequest.InsuranceFileKeySpecified, oRequest.InsuranceFileKey, DBNull.Value)
            cmd.AddInParameter("@agent_id", SqlDbType.Int).Value = IIf(oRequest.AgentKeySpecified, oRequest.AgentKey, DBNull.Value)
            cmd.AddInParameter("@product_ids", SqlDbType.VarChar).Value = IIf(productCodes <> "", productCodes, DBNull.Value)
            cmd.AddInParameter("@start_date", SqlDbType.DateTime).Value = IIf(oRequest.SearchDateFromSpecified, oRequest.SearchDateFrom, DBNull.Value)
            cmd.AddInParameter("@end_date", SqlDbType.DateTime).Value = IIf(oRequest.SearchDateToSpecified, oRequest.SearchDateTo, DBNull.Value)
            cmd.AddInParameter("@direct", SqlDbType.Bit).Value = IIf(oRequest.DirectBusinessOnlySpecified, oRequest.DirectBusinessOnly, DBNull.Value)
            dsResult = con.ExecuteDataSet(cmd, "GetQuotesMarkedForCollection")
        End Using

        If (dsResult.Tables.Count > 0) Then

            If (dsResult.Tables(0).Rows.Count > 0) Then

                'Reformat the data set the get only the elements needed
                Dim xmlDoc As New System.Xml.XmlDocument
                dsResult.DataSetName = "BaseGetQuotesMarkedForCollectionResponseTypeMarkedQuotes"
                dsResult.Tables(0).TableName = "Row"

                If oRequest.WCFSecurityToken = "" Then
                    xmlDoc.LoadXml(dsResult.GetXml)
                    If (dsResult.Tables.Count >= 1) Then
                        'Populate the XML Document here with the dsUserGroups information
                        oGetQuotesMarkedForCollectionResponse.ResultDataSet = xmlDoc.DocumentElement()
                    End If
                End If
                oGetQuotesMarkedForCollectionResponse.ResultData = dsResult
            End If
        End If

        Return oGetQuotesMarkedForCollectionResponse

    End Function

    ''' <summary>
    ''' This function will validate the  creditcard details
    '''</summary>
    '''<param name="oRequest" type="BaseBankReceiptType"></param>
    '''<param name="oErrorCollection" type="SAMErrorCollection"></param>
    '''<remarks></remarks>
    '***********************************************************************************************
    'Note:The credit card ExpiryDate and StartDate should be in the format of "MM/YY" for 
    'Examaple 06/08
    '***********************************************************************************************
    Public Overridable Sub ValidateBaseBankReceiptTypeData(ByVal oRequest As BaseBankReceiptType, ByRef oErrorCollection As SAMErrorCollection)
        Dim CoreBusiness As New CoreBusiness

        ' validate the ChequeTypeCode
        If Not String.IsNullOrEmpty(oRequest.ChequeTypeCode) Then
            oRequest.ChequeTypeID =
                CoreBusiness.GetAndValidateListItemFromCode(
                    Core.STSListType.PMLookup,
                    PMLookupTable.ChequeType,
                    oRequest.ChequeTypeCode,
                    "ChequeTypeCode",
                    oErrorCollection)
        End If

        ' validate the ChequeClearingTypeCode
        If Not String.IsNullOrEmpty(oRequest.ChequeClearingTypeCode) Then
            oRequest.ChequeClearingTypeID =
                CoreBusiness.GetAndValidateListItemFromCode(
                    Core.STSListType.PMLookup,
                    PMLookupTable.ChequeClearingType,
                    oRequest.ChequeClearingTypeCode,
                    "ChequeClearingTypeCode",
                    oErrorCollection)
        End If

        'Note that PIN code length, along with several other credit card validation rules, is 
        'configurable for the media type issuer but is out of scope at the moment. Add a suitable 
        'comment to this effect.
    End Sub

    Public Overloads Function TransferQuote(ByVal oTransferQuoteRequest As BaseTransferQuoteRequestType) As BaseTransferQuoteResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                             _SiriusUser.Username, _SiriusUser.SourceID,
                                             _SiriusUser.LanguageID,
                                             SiriusUserDefaults.AppName)

            Dim oResponse As BaseTransferQuoteResponseType

            oResponse = TransferQuote(con, oTransferQuoteRequest)

            Return oResponse

        End Using
    End Function

    Public Overloads Function TransferQuote(ByVal con As SiriusConnection, ByVal oTransferQuoteRequest As BaseTransferQuoteRequestType) As BaseTransferQuoteResponseType
        Dim oTransferQuoteResponse As New BaseTransferQuoteResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection

        Dim nTypeOfPackage As enumTypeOfPackage

        oTransferQuoteResponse = New SAMForInsuranceV2ImplementationTypes.TransferQuoteResponseType

        '*******************************************************************************************
        'Structure Validation
        '*******************************************************************************************

        oTransferQuoteRequest.Validate(CType(oSAMErrorCollection, Object))
        '' exit if there are any missing parameters
        oSAMErrorCollection.CheckForErrors()

        'Check if Quote
        Dim dt As DataTable
        Dim cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Check_Quote")
        cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oTransferQuoteRequest.InsuranceFileKey)
        dt = con.ExecuteDataTable(cmd)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidInsuranceFileCnt,
                                                SAMBusinessErrors.InValidInsuranceFileCnt.ToString,
                                                "InsuranceFileKey is not a quote")
        End If

        'Check Both Parties Exists
        cmd = SiriusCommand.FromProcedure("spu_SAM_Get_Party")
        cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = oTransferQuoteRequest.PartyFromKey
        dt = con.ExecuteDataTable(cmd)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InvalidParty,
                                                SAMBusinessErrors.InvalidParty.ToString,
                                                "PartyFrom is not a valid party")
        End If

        cmd = SiriusCommand.FromProcedure("spu_SAM_Get_Party")
        cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = oTransferQuoteRequest.PartyToKey
        dt = con.ExecuteDataTable(cmd)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InvalidParty,
                                                SAMBusinessErrors.InvalidParty.ToString,
                                                "PartyTo is not a valid party")
        End If

        oSAMErrorCollection.CheckForErrors()
        'Check that FromParty is the party on the policy

        cmd = SiriusCommand.FromProcedure("spu_SAM_Get_Insured")
        cmd.AddInParameter("@InsuranceFileCnt", SqlDbType.Int).Value = oTransferQuoteRequest.InsuranceFileKey
        dt = con.ExecuteDataTable(cmd)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InvalidParty,
                                                SAMBusinessErrors.InvalidParty.ToString,
                                                "InsuranceFile has no Insured")
        Else
            Dim currentInsuredKey As Integer

            currentInsuredKey = CType(dt.Rows(0).Item("insured_cnt"), Integer)

            If currentInsuredKey <> oTransferQuoteRequest.PartyFromKey Then
                'Input data is incorrect 
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InvalidParty,
                                    SAMBusinessErrors.InvalidParty.ToString,
                                    "PartyFromKey is not the insured on InsuranceFileKey policy")
            End If
        End If

        oSAMErrorCollection.CheckForErrors()

        'Transfer using stored procedure
        cmd = SiriusCommand.FromProcedure("spu_SAM_Transfer_Quote")
        cmd.AddInParameter("@InsuranceFileCnt", SqlDbType.Int).Value = oTransferQuoteRequest.InsuranceFileKey
        cmd.AddInParameter("@PartyToCnt", SqlDbType.Int).Value = oTransferQuoteRequest.PartyToKey
        cmd.AddInParameter("@PartyFromCnt", SqlDbType.Int).Value = oTransferQuoteRequest.PartyFromKey
        cmd.AddInParameter("@PMUserID", SqlDbType.Int).Value = _SiriusUser.UserID
        con.ExecuteNonQuery(cmd)

        Return oTransferQuoteResponse

    End Function

    ''' <summary>
    ''' This method returns the RiskCnt for a given InsuranceFileCnt but returns -1 if there is more than one risk
    '''</summary>
    '''<param name="con">An object of  a class SiriusConnection</param>
    '''<param name="iInsuranceFileKey">Integer parameter to pass insurance file key</param>
    ''' <param name="sInsuranceFileTypeCode" > String parameter to pass the insurance file type code</param>
    '''<remarks></remarks>


    Public Overloads Function CopyQuote(ByVal oCopyQuoteRequest As BaseCopyQuoteRequestType) As BaseCopyQuoteResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As BaseCopyQuoteResponseType

            oResponse = CopyQuote(con, oCopyQuoteRequest)

            Return oResponse

        End Using
    End Function

    Public Overloads Function CopyQuote(ByVal conCopyQuote As SiriusConnection, ByVal oCopyQuoteRequest As BaseCopyQuoteRequestType) As BaseCopyQuoteResponseType
        Dim oPolicyNum As bSIRPolicyNumMaint.Business = Nothing
        Dim oCopyQuoteResponse As New BaseCopyQuoteResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness
        Dim nTypeOfPackage As enumTypeOfPackage

        If oCopyQuoteRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CopyQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oCopyQuoteResponse = New SAMForInsuranceV2ImplementationTypes.CopyQuoteResponseType
        End If
        Dim sOptionValue As String = String.Empty

        'Get QuoteVersioning from SystemOptions
        oCoreBusiness.GetSystemOption(oCopyQuoteRequest.BranchCode,
                                    SystemOption.QuoteVersioning,
                                    sOptionValue)



        '*******************************************************************************************
        'Structure Validation
        '*******************************************************************************************

        ' Check mandatory fields
        If oCopyQuoteRequest.InsuranceFileKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                        SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InsuranceFile")
        End If

        '' exit if there are any missing parameters
        oSAMErrorCollection.CheckForErrors()

        Using cmdCheckQuote As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Check_Quote")

            cmdCheckQuote.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oCopyQuoteRequest.InsuranceFileKey)
            cmdCheckQuote.AddInParameter("@bCloneLivePolicy", SqlDbType.TinyInt).Value = Cast.NullIfDefault(oCopyQuoteRequest.CloneLivePolicy)
            Dim dtQuote As DataTable
            Dim r_sGeneratedPolicyNumber As String = Nothing
            Dim sBranchCode As String
            Dim lNewInsuranceFile As Int32
            Dim lNewInsuranceFolder As Int32
            dtQuote = conCopyQuote.ExecuteDataTable(cmdCheckQuote)
            If dtQuote IsNot Nothing AndAlso dtQuote.Rows.Count > 0 Then

                If Not String.IsNullOrEmpty(sOptionValue) AndAlso CType(sOptionValue, Integer) = 1 Then
                    sBranchCode = Cast.ToString(dtQuote.Rows(0).Item("code"), String.Empty)
                    Using cmdCopyQuote As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Copy_Quote")
                        cmdCopyQuote.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oCopyQuoteRequest.InsuranceFileKey)
                        cmdCopyQuote.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = Cast.ToInt32(dtQuote.Rows(0).Item("insurance_folder_cnt"), 0)
                        cmdCopyQuote.AddOutParameter("@new_insurance_file_cnt", SqlDbType.Int)
                        cmdCopyQuote.AddOutParameter("@new_insurance_folder_cnt", SqlDbType.Int)
                        cmdCopyQuote.AddOutParameter("@base_insurance_folder_cnt", SqlDbType.Int)
                        cmdCopyQuote.AddOutParameter("@insurance_ref", SqlDbType.VarChar, 30)
                        cmdCopyQuote.AddOutParameter("@quote_status_id", SqlDbType.Int)
                        cmdCopyQuote.AddOutParameter("@quote_version", SqlDbType.Int)

                        conCopyQuote.ExecuteNonQuery(cmdCopyQuote)

                        oCopyQuoteResponse.BaseInsuranceFolderKey = Cast.ToInt32(cmdCopyQuote.Parameters("@base_insurance_folder_cnt").Value, 0)
                        oCopyQuoteResponse.InsuranceFileKey = Cast.ToInt32(cmdCopyQuote.Parameters("@new_insurance_file_cnt").Value, 0)
                        oCopyQuoteResponse.InsuranceFolderKey = Cast.ToInt32(cmdCopyQuote.Parameters("@new_insurance_folder_cnt").Value, 0)
                        oCopyQuoteResponse.InsuranceRef = cmdCopyQuote.Parameters("@insurance_ref").Value.ToString
                        oCopyQuoteResponse.QuoteVersion = Cast.ToInt32(cmdCopyQuote.Parameters("@quote_version").Value, 0)
                        oCopyQuoteResponse.QuoteStatusKey = QuoteStatusType.Pending

                        ProcessCopyMTARisks(conCopyQuote, sBranchCode, oCopyQuoteResponse.InsuranceFileKey, TransactionType.QUOTE, True)

                    End Using
                Else
                    'Pure 3.0 ---- WPR 41
                    If Cast.ToInt32(dtQuote.Rows(0).Item("quote_auto_numbering_id"), 0) > 0 Then
                        Try
                            Dim lReturn As Integer
                            Dim oDatabase As dPMDAO.Database = Nothing
                            Dim oDatabaseObject As Object = oDatabase


                            oPolicyNum = New bSIRPolicyNumMaint.Business

                            ' initialise the business object
                            lReturn = CInt(oPolicyNum.Initialise(_SiriusUser.Username,
                                                               _SiriusUser.Password,
                                                               _SiriusUser.UserID,
                                                               _SiriusUser.SourceID,
                                                               _SiriusUser.LanguageID,
                                                               _SiriusUser.CurrencyID,
                                                               1,
                                                               SiriusUserDefaults.AppName,
                                                               False,
                                                               oDatabaseObject))

                            lReturn = oPolicyNum.GeneratePolicyNumber(Cast.ToInt32(dtQuote.Rows(0).Item("insurance_file_type_id"), 0),
                                                 Cast.ToInt16(dtQuote.Rows(0).Item("source_id"), 0),
                                                 Cast.ToInt32(dtQuote.Rows(0).Item("product_id"), 0),
                                                 Cast.ToInt32(dtQuote.Rows(0).Item("lead_agent_cnt"), 0),
                                                 r_sGeneratedPolicyNumber)
                            If (lReturn <> PMEReturnCode.PMTrue) Then
                                ' if the GeneratePolicyNumber fails then throw a business rule error
                                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountsProcessingFailed,
                                                                    SAMBusinessErrors.AccountsProcessingFailed.ToString,
                                                                    "bSIRPolicyNumMaint.Business.GeneratePolicyNumber")
                                oSAMErrorCollection.CheckForErrors()
                            End If
                        Catch ex As Exception
                            Throw
                        Finally
                            If oPolicyNum IsNot Nothing Then
                                oPolicyNum.Dispose()
                                oPolicyNum = Nothing
                            End If
                        End Try
                        sBranchCode = Cast.ToString(dtQuote.Rows(0).Item("code"), String.Empty)
                        If Not String.IsNullOrEmpty(r_sGeneratedPolicyNumber) Then
                            Using cmdCopyQuote As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Copy_Quote_Without_Versioning")
                                cmdCopyQuote.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oCopyQuoteRequest.InsuranceFileKey)
                                cmdCopyQuote.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = Cast.ToInt32(dtQuote.Rows(0).Item("insurance_folder_cnt"), 0)
                                cmdCopyQuote.AddInParameter("@new_insurance_ref", SqlDbType.VarChar, 255).Value = r_sGeneratedPolicyNumber
                                cmdCopyQuote.AddOutParameter("@new_insurance_file_cnt", SqlDbType.Int)
                                cmdCopyQuote.AddOutParameter("@new_insurance_folder_cnt", SqlDbType.Int)
                                cmdCopyQuote.AddInParameter("@userID", SqlDbType.Int).Value = Cast.ToInt32(_SiriusUser.UserID, 0)

                                conCopyQuote.ExecuteNonQuery(cmdCopyQuote)

                                lNewInsuranceFile = Cast.ToInt32(cmdCopyQuote.Parameters.Item("@new_insurance_file_cnt"), 0)
                                lNewInsuranceFolder = Cast.ToInt32(cmdCopyQuote.Parameters.Item("@new_insurance_folder_cnt"), 0)

                                ProcessCopyMTARisks(conCopyQuote, sBranchCode, lNewInsuranceFile, TransactionType.QUOTE, True)

                                oCopyQuoteResponse.InsuranceRef = r_sGeneratedPolicyNumber
                                oCopyQuoteResponse.InsuranceFileKey = lNewInsuranceFile
                                oCopyQuoteResponse.InsuranceFolderKey = lNewInsuranceFolder

                                'Generate a default Sharepoint folder (if Sharepoint is enabled)
                                Dim Sharepoint As bSIRSharepoint.Business
                                Sharepoint = New bSIRSharepoint.Business

                                SAMFunc.InitialiseSBOObject(oObject:=Sharepoint, siriusUser:=_SiriusUser, sObjectName:="bSIRSharepoint.Business")
                                Sharepoint.GenerateDefaultPath(0, lNewInsuranceFile, 0, 0)

                            End Using
                        End If
                    Else
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.NumberingSchemeNotFound,
                                                                SAMBusinessErrors.NumberingSchemeNotFound.ToString,
                                                                "Numbering Scheme not attached to Product")
                        oSAMErrorCollection.CheckForErrors()
                    End If
                End If

            End If
        End Using
        Return oCopyQuoteResponse

    End Function

    Public Overloads Function UpdateQuoteStatus(ByVal oUpdateQuoteStatusRequest As BaseUpdateQuoteStatusRequestType) As BaseUpdateQuoteStatusResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseUpdateQuoteStatusResponseType

            oResponse = UpdateQuoteStatus(con, oUpdateQuoteStatusRequest)

            Return oResponse

        End Using
    End Function

    Public Overloads Function UpdateQuoteStatus(ByVal conUpdateQuoteStatus As SiriusConnection, ByVal oUpdateQuoteStatusRequest As BaseUpdateQuoteStatusRequestType) As BaseUpdateQuoteStatusResponseType

        Dim oUpdateQuoteStatusResponse As New BaseUpdateQuoteStatusResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection

        Dim nTypeOfPackage As enumTypeOfPackage

        If oUpdateQuoteStatusRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateQuoteStatusRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateQuoteStatusResponse = New SAMForInsuranceV2ImplementationTypes.UpdateQuoteStatusResponseType
        End If
        '*******************************************************************************************
        'Structure Validation
        '*******************************************************************************************

        ' Check mandatory fields
        If oUpdateQuoteStatusRequest.InsuranceFileKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                        SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InsuranceFile")
        End If

        '' exit if there are any missing parameters
        oSAMErrorCollection.CheckForErrors()

        'Check whether insurance file exists in DB or not.
        Dim dt As DataTable
        Dim cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Check_Quote")
        cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oUpdateQuoteStatusRequest.InsuranceFileKey
        dt = conUpdateQuoteStatus.ExecuteDataTable(cmd)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidInsuranceFileCnt,
                                                SAMBusinessErrors.InValidInsuranceFileCnt.ToString,
                                                "InsuranceFileKey is not a quote")

        End If
        oSAMErrorCollection.CheckForErrors()

        If oUpdateQuoteStatusRequest.QuoteStatusKey = QuoteStatusType.None Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidInsuranceFileCnt,
                                                SAMBusinessErrors.InValidInsuranceFileCnt.ToString,
                                                "Invalid QuoteStatusKey")

        End If
        oSAMErrorCollection.CheckForErrors()

        Using cmdUpdateQuoteStatus As SiriusCommand = SiriusCommand.FromProcedure("spu_Update_Quote_Status")
            cmdUpdateQuoteStatus.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oUpdateQuoteStatusRequest.InsuranceFileKey
            cmdUpdateQuoteStatus.AddInParameter("@quote_status_id", SqlDbType.Int).Value = oUpdateQuoteStatusRequest.QuoteStatusKey
            conUpdateQuoteStatus.ExecuteNonQuery(cmdUpdateQuoteStatus)
        End Using

        oUpdateQuoteStatusResponse.InsuranceFileKey = oUpdateQuoteStatusRequest.InsuranceFileKey
        oUpdateQuoteStatusResponse.QuoteStatusKey = oUpdateQuoteStatusRequest.QuoteStatusKey

        Return oUpdateQuoteStatusResponse

    End Function

    Public Overloads Function DeletePolicy(ByVal oDeletePolicyRequest As BaseDeletePolicyRequestType) As BaseDeletePolicyResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseDeletePolicyResponseType

            oResponse = DeletePolicy(con, oDeletePolicyRequest)

            Return oResponse

        End Using
    End Function

    Public Overloads Function DeletePolicy(ByVal conDeletePolicy As SiriusConnection, ByVal oDeletePolicyRequest As BaseDeletePolicyRequestType) As BaseDeletePolicyResponseType

        Dim oDeletePolicyResponse As New BaseDeletePolicyResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection

        Dim nTypeOfPackage As enumTypeOfPackage

        If oDeletePolicyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.DeletePolicyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oDeletePolicyResponse = New SAMForInsuranceV2ImplementationTypes.DeletePolicyResponseType
        End If
        '*******************************************************************************************
        'Structure Validation
        '*******************************************************************************************

        ' Check mandatory fields
        If oDeletePolicyRequest.InsuranceFileKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                        SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InsuranceFile")
        End If

        '' exit if there are any missing parameters
        oSAMErrorCollection.CheckForErrors()

        'Check whether insurance file exists in DB or not.
        Dim dt As DataTable
        Dim cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Check_Quote")
        cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oDeletePolicyRequest.InsuranceFileKey
        dt = conDeletePolicy.ExecuteDataTable(cmd)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InValidInsuranceFileCnt,
                                                SAMBusinessErrors.InValidInsuranceFileCnt.ToString,
                                                "InsuranceFileKey is not a quote")

        End If
        oSAMErrorCollection.CheckForErrors()




        'TODO
        Using cmdDeletePolicy As SiriusCommand = SiriusCommand.FromProcedure("spu_DeletePolicy")
            cmdDeletePolicy.AddInParameter("@InsuranceFileCnt", SqlDbType.Int).Value = oDeletePolicyRequest.InsuranceFileKey
            conDeletePolicy.ExecuteNonQuery(cmdDeletePolicy)
        End Using

        'Set the response
        Return oDeletePolicyResponse

    End Function
    'WPR 33-75 ADDED
#Region "AddBackDatedMTAQuote"

    Public Overloads Function AddBackDatedMTAQuote(ByVal oAddBackDatedMTAQuoteRequest As BaseAddBackDatedMTAQuoteRequestType) As BaseAddBackDatedMTAQuoteResponseType


        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                              _SiriusUser.Username, _SiriusUser.SourceID,
                                              _SiriusUser.LanguageID,
                                              SiriusUserDefaults.AppName)

            Dim oResponse As BaseAddBackDatedMTAQuoteResponseType

            oResponse = AddBackDatedMTAQuote(con, oAddBackDatedMTAQuoteRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    ''' To add backdated MTA quote
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function AddBackDatedMTAQuote(ByRef con As SiriusConnection,
    ByVal oRequest As BaseAddBackDatedMTAQuoteRequestType) As BaseAddBackDatedMTAQuoteResponseType

        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseAddBackDatedMTAQuoteResponseType
        'WPR 33-75 ADDED
        Dim failureReason As String = String.Empty
        'WPR 33-75 END

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddBackDatedMTAQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.AddBackDatedMTAQuoteResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oErrorCollection As New SAMErrorCollection

        '*******************
        ' DATA VALIDATION
        '*******************
        'Mandatory Validation
        If String.IsNullOrEmpty(oRequest.BranchCode) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "BranchCode")
        End If
        If oRequest.PartyCnt = 0 Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "PartyCnt")
        End If
        If oRequest.EffectiveDate <= Date.MinValue Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "EffectiveDate")
        End If
        If oRequest.InsuranceFolderKey = 0 Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "InsuranceFolderKey")
        End If
        If oRequest.InsuranceFileKey = 0 Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "InsuranceFileKey")
        End If

        If Not (oRequest.TransactionType = TransactionType.MTA Or oRequest.TransactionType = TransactionType.MTC Or oRequest.TransactionType = TransactionType.MTR) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.TransactionTypeIsInvalid,
                                    SAMInvalidData.TransactionTypeIsInvalid.ToString,
                                    "TransactionType",
                                    Convert.ToString(oRequest.TransactionType))
        End If

        oErrorCollection.CheckForErrors()

        Dim iBranchCodeId As Integer
        Dim iInsuranceFolderKey As Integer
        Dim iInsuranceFileKey As Integer
        'Lookups Validations
        Try
            iBranchCodeId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                    SAMInvalidData.InvalidLookupListValue.ToString,
                                    "BranchCode",
                                    oRequest.BranchCode)
        End Try

        If oRequest.InsuranceFolderKey <> 0 Then
            iInsuranceFolderKey = GetAndValidateSpecifiedTableCode(con, "Insurance_Folder", "Insurance_Folder_Cnt", "Insurance_Folder_Cnt", oRequest.InsuranceFolderKey.ToString, oErrorCollection, "InsuranceFolderKey")
            oErrorCollection.CheckForErrors()
        End If

        If oRequest.InsuranceFileKey <> 0 Then
            iInsuranceFileKey = GetAndValidateSpecifiedTableCode(con, "Insurance_File", "Insurance_File_Cnt", "Insurance_File_Cnt", oRequest.InsuranceFileKey.ToString, oErrorCollection, "InsuranceFileKey")
            oErrorCollection.CheckForErrors()
        End If

        oErrorCollection.CheckForErrors()

        Dim autoMTABusiness As bSIRAutoMTA.Business = CreateAndInitialiseAutoMTABusiness(con, oRequest.BranchCode)
        Dim comReturnValue As Integer
        Dim ovResultArray As Object(,) = Nothing
        Dim bShowQuoteMsg As Boolean = False

        Try

            If oRequest.TransactionType = TransactionType.MTA Then
                comReturnValue = autoMTABusiness.QuoteMTA(v_lPartyCnt:=oRequest.PartyCnt,
                            v_lInsuranceFolderCnt:=oRequest.InsuranceFolderKey,
                            v_dtEffectiveDate:=oRequest.EffectiveDate,
                            lBaseInsuranceFileCnt:=oRequest.InsuranceFileKey,
                            r_sFailureMessage:=failureReason,
                            vBackdatedMTAVersions:=ovResultArray,
                            bUpdateStats:=False, r_bShowQuoteMsg:=bShowQuoteMsg, bIsDirty:=True, bIsInteractive:=oRequest.IsInteractive)
                'WPR 33-75 END
                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRAutoMTA.Business.QuoteMTA", comReturnValue)
                End If

                If failureReason <> String.Empty Then
                    oResponse.FailureReason = failureReason
                    Return oResponse
                End If


                ' BackDate is not quote able quote all risks automatically     
                If bShowQuoteMsg = True Then
                    oResponse.FailureReason = "System cannot quote all risk for reprocessed transactions - MTA or Cancellation should be processed manually in the current period"
                    Return oResponse
                    'RaiseComMethodException("bSIRAutoMTA.Business.QuoteMTA Failed - System cannot quote all risk for reprocessed transactions - MTA or Cancellation should be processed manually in the current period.")
                End If
            ElseIf oRequest.TransactionType = TransactionType.MTC Then
                Dim lNewInsuranceFileCnt As Integer
                'WPR 33-75 ADDED
                comReturnValue = autoMTABusiness.QuoteCancellation(v_lPartyCnt:=oRequest.PartyCnt,
                           v_lInsuranceFolderCnt:=oRequest.InsuranceFolderKey,
                           v_dtEffectiveDate:=oRequest.EffectiveDate,
                           lBaseInsuranceFileCnt:=oRequest.InsuranceFileKey,
                           r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt,
                           r_sFailureMessage:=failureReason,
                           bUpdateStats:=False,
                           bBackDateMTA:=True, vPolicyRef:="", bIsDirty:=True)
                'WPR 33-75 END
                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRAutoMTA.Business.QuoteCancellation", comReturnValue)
                End If

                If failureReason <> String.Empty Then
                    oResponse.FailureReason = failureReason
                    Return oResponse
                End If
            ElseIf oRequest.TransactionType = TransactionType.MTR Then

                Dim nNewInsuranceFileCnt As Integer

                comReturnValue = autoMTABusiness.QuoteReinstatement(nPartyCnt:=oRequest.PartyCnt,
                            nInsuranceFolderCnt:=oRequest.InsuranceFolderKey,
                            dtEffectiveDate:=oRequest.EffectiveDate,
                            nBaseInsuranceFileCnt:=oRequest.InsuranceFileKey,
                            nNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                            sFailureMessage:=failureReason,
                            bUpdateStats:=False,
                            bBackDateMTA:=True, oPolicyRef:="", bIsDirty:=True)
                If comReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRAutoMTA.Business.QuoteReinstatement", comReturnValue)
                End If

                'WPR 33-75 ADDED
                If failureReason <> String.Empty Then
                    oResponse.FailureReason = failureReason
                    Return oResponse
                End If

            End If

            Dim oDataRequest As New BaseGetHeaderAndSummariesByKeyRequestType
            oDataRequest.InsuranceFileKey = oRequest.InsuranceFileKey
            oDataRequest.BranchCode = oRequest.BranchCode
            oDataRequest.SourceId = oRequest.SourceId

            oResponse = GetBackdatedMTAPolicyVersions(con, oDataRequest)

            Return oResponse
        Finally

            If autoMTABusiness IsNot Nothing Then
                autoMTABusiness.Dispose()
                autoMTABusiness = Nothing
            End If
        End Try
    End Function


    ''' <summary>
    ''' To get backdated versions for policy
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetBackdatedMTAPolicyVersions(ByVal oRequest As BaseGetHeaderAndSummariesByKeyRequestType) As BaseAddBackDatedMTAQuoteResponseType


        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As BaseImplementationTypes.BaseAddBackDatedMTAQuoteResponseType

            oResponse = New SAMForInsuranceV2ImplementationTypes.AddBackDatedMTAQuoteResponseType

            oResponse = GetBackdatedMTAPolicyVersions(con, oRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    ''' To get backdated versions for policy
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetBackdatedMTAPolicyVersions(ByRef con As SiriusConnection,
        ByVal oRequest As BaseGetHeaderAndSummariesByKeyRequestType) As BaseAddBackDatedMTAQuoteResponseType

        Dim oResponse As BaseImplementationTypes.BaseAddBackDatedMTAQuoteResponseType
        Dim iInsuranceFileKey As Integer
        Dim oErrorCollection As New SAMErrorCollection
        Dim failureReason As String = String.Empty

        oResponse = New SAMForInsuranceV2ImplementationTypes.AddBackDatedMTAQuoteResponseType

        If oRequest.InsuranceFileKey <> 0 Then
            iInsuranceFileKey = GetAndValidateSpecifiedTableCode(con, "Insurance_File", "Insurance_File_Cnt", "Insurance_File_Cnt", oRequest.InsuranceFileKey.ToString, oErrorCollection, "InsuranceFileKey")
            oErrorCollection.CheckForErrors()
        End If

        Dim autoMTABusiness As bSIRAutoMTA.Business = CreateAndInitialiseAutoMTABusiness(con, oRequest.BranchCode)
        Dim comReturnValue As Integer
        Dim oResultArray(,) As Object = Nothing

        Try
            comReturnValue = autoMTABusiness.GetBackdatedPolicyVersions(oRequest.InsuranceFileKey, oResultArray)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRAutoMTA.Business.GetBackdatedMTAPolicyVersions", comReturnValue)
            End If

        Finally
            If autoMTABusiness IsNot Nothing Then
                autoMTABusiness.Dispose()
                autoMTABusiness = Nothing
            End If
        End Try
        If oResultArray IsNot Nothing Then
            Dim oResultArrayOut As Object = Nothing
            Dim oResultArrayCopy(,) As Object = Nothing

            Dim oFilterCol() As Object = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}

            Dim sXML As String = String.Empty
            Const sACMethodName As String = "GetBackdatedMTAPolicyVersions"
            Dim DocoXML As System.Xml.XmlDocument

            Dim oCoreBusiness As New CoreBusiness(_SiriusUser)

            Dim oResultData As Object = Nothing
            Dim dsBackdatedTransactionDetails As New DataSet

            Dim oResult(,) As Object = Nothing

            sXML = String.Empty
            DocoXML = New System.Xml.XmlDocument

            If oResultArray.ToString <> "" Then
                Try
                    Dim arrColHeaders(,) As Object = {{"InsuranceFileCnt", "PolicyType",
                               "CoverStartDate", "CoverEndDate", "MTAPremium", "OriginalMTAPremium",
                               "PolicyStatus", "ReversedInsuranceFileCnt", "QuoteStatus",
                               "OriginalCommission", "MTACommission", "OriginalFee", "MTAFee"},
                               {System.Type.GetType("System.Int32"), System.Type.GetType("System.String"),
                                System.Type.GetType("System.DateTime"), System.Type.GetType("System.DateTime"), System.Type.GetType("System.Double"),
                               System.Type.GetType("System.Double"), System.Type.GetType("System.String"),
                               System.Type.GetType("System.Int32"), System.Type.GetType("System.String"),
                               System.Type.GetType("System.Double"), System.Type.GetType("System.Double"),
                               System.Type.GetType("System.Double"), System.Type.GetType("System.Double")}}


                    oResultArrayOut = CopyArrayWithColumns(CType(oResultArray, Object(,)), oResultArrayCopy, oFilterCol)
                    dsBackdatedTransactionDetails = Utilities.ArrayToDataSet(oResultArrayOut, arrColHeaders, "BaseAddBackDatedMTAQuoteResponseTypeBackdatedTransactions")
                    If dsBackdatedTransactionDetails IsNot Nothing AndAlso dsBackdatedTransactionDetails.Tables.Count > 0 AndAlso dsBackdatedTransactionDetails.Tables(0).Rows.Count > 0 Then
                        dsBackdatedTransactionDetails.DataSetName = "BaseAddBackDatedMTAQuoteResponseTypeBackdatedTransactions"
                        dsBackdatedTransactionDetails.Tables(0).TableName = "Row"
                        dsBackdatedTransactionDetails.Tables(0).Columns(0).ColumnName = "InsuranceFileCnt"
                        dsBackdatedTransactionDetails.Tables(0).Columns(1).ColumnName = "PolicyType"
                        dsBackdatedTransactionDetails.Tables(0).Columns(2).ColumnName = "CoverStartDate"
                        dsBackdatedTransactionDetails.Tables(0).Columns(3).ColumnName = "CoverEndDate"
                        dsBackdatedTransactionDetails.Tables(0).Columns(4).ColumnName = "MTAPremium"
                        dsBackdatedTransactionDetails.Tables(0).Columns(5).ColumnName = "OriginalMTAPremium"
                        dsBackdatedTransactionDetails.Tables(0).Columns(6).ColumnName = "PolicyStatus"
                        dsBackdatedTransactionDetails.Tables(0).Columns(7).ColumnName = "ReversedInsuranceFileCnt"
                        dsBackdatedTransactionDetails.Tables(0).Columns(8).ColumnName = "QuoteStatus"
                        dsBackdatedTransactionDetails.Tables(0).Columns(9).ColumnName = "OriginalCommission"
                        dsBackdatedTransactionDetails.Tables(0).Columns(10).ColumnName = "MTACommission"
                        dsBackdatedTransactionDetails.Tables(0).Columns(11).ColumnName = "OriginalFee"
                        dsBackdatedTransactionDetails.Tables(0).Columns(12).ColumnName = "MTAFee"
                    End If
                    If oRequest.WCFSecurityToken = "" Then
                        sXML = dsBackdatedTransactionDetails.GetXml()

                        DocoXML.LoadXml(sXML)
                        oResponse.ResultDataset = DocoXML.DocumentElement
                    End If
                    oResponse.ResultData = dsBackdatedTransactionDetails

                    If failureReason <> String.Empty Then
                        oResponse.FailureReason = failureReason
                    End If

                Catch ex As Exception
                    Dim STSErrorEx As New STSErrorPublisher("Failed to convert XML (ResultArray).", ex)
                    STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), sACMethodName, "GetBackdatedMTAPolicyVersions", True)
                End Try
            End If
        End If
        Return oResponse
    End Function

#End Region
#Region "DeleteBackDatedVersions"
    ''' <summary>
    ''' To delete the previously generated back dated versions for a policy
    ''' </summary>
    ''' <param name="oDeleteBackDatedVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function DeleteBackDatedVersions(ByVal oDeleteBackDatedVersionsRequest As BaseDeleteBackDatedVersionsRequestType) As BaseDeleteBackDatedVersionsResponseType


        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                             SiriusUserDefaults.AppName)

            Dim oResponse As BaseDeleteBackDatedVersionsResponseType

            oResponse = DeleteBackDatedVersions(con, oDeleteBackDatedVersionsRequest)

            Return oResponse

        End Using

    End Function

    Public Overloads Function DeleteBackDatedVersions(ByRef con As SiriusConnection,
    ByVal oRequest As BaseDeleteBackDatedVersionsRequestType) As BaseDeleteBackDatedVersionsResponseType

        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseDeleteBackDatedVersionsResponseType

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.DeleteBackDatedVersionsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.DeleteBackDatedVersionsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oErrorCollection As New SAMErrorCollection

        '*******************
        ' DATA VALIDATION
        '*******************
        'Mandatory Validation
        If String.IsNullOrEmpty(oRequest.BranchCode) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "BranchCode")
        End If

        If oRequest.InsuranceFileKey = 0 Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "InsuranceFileKey")
        End If

        Dim iBranchCodeId As Integer
        Dim iInsuranceFileKey As Integer

        Try
            iBranchCodeId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                    SAMInvalidData.InvalidLookupListValue.ToString,
                                    "BranchCode",
                                    oRequest.BranchCode)
        End Try

        If oRequest.InsuranceFileKey <> 0 Then
            iInsuranceFileKey = GetAndValidateSpecifiedTableCode(con, "Insurance_File", "Insurance_File_Cnt", "Insurance_File_Cnt", oRequest.InsuranceFileKey.ToString, oErrorCollection, "InsuranceFileKey")
            oErrorCollection.CheckForErrors()
        End If

        oErrorCollection.CheckForErrors()

        Dim autoMTABusiness As bSIRAutoMTA.Business = CreateAndInitialiseAutoMTABusiness(con, oRequest.BranchCode)
        Dim comReturnValue As Integer
        Dim ovResultArray As Object = Nothing

        Try
            comReturnValue = autoMTABusiness.ClearBackdateMTAData(oRequest.InsuranceFileKey)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRAutoMTA.Business.ClearBackdateMTAData", comReturnValue)
            End If

        Finally
            If autoMTABusiness IsNot Nothing Then
                autoMTABusiness.Dispose()
                autoMTABusiness = Nothing
            End If
        End Try

        Return oResponse

    End Function

#End Region
    'WPR 33-75 END

    Public Overloads Sub GetRiskCntFromInsuranceFileCnt(
                ByVal insuranceFileKey As Integer,
                ByRef riskKey As Integer)

        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            GetRiskCntFromInsuranceFileCnt(con, insuranceFileKey, riskKey)

        End Using
    End Sub

    Public Overloads Sub GetRiskCntFromInsuranceFileCnt(
                        ByVal con As SiriusConnection,
                        ByVal insuranceFileKey As Integer,
                        ByRef riskKey As Integer)

        ' Update the existing insurance file type with the given insurance file type
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_GetPolicyRisks")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            cmd.AddOutParameter("@risk_cnt", SqlDbType.Int)

            con.ExecuteNonQuery(cmd)

            riskKey = Cast.ToInt32(cmd.Parameters.Item("@risk_cnt"), -1)

        End Using

    End Sub

    ''' <summary>
    ''' GetDefaultUnderwritingYear
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="iUnderwritingyearid"></param>
    ''' <param name="dtCoverStartDate"></param>
    ''' <param name="oBusiness"></param>
    ''' <param name="oError"></param>
    ''' <remarks></remarks>
    Public Sub GetDefaultUnderwritingYear(ByVal con As SiriusConnection, ByRef iUnderwritingyearid As Integer, ByVal dtCoverStartDate As DateTime, ByRef oBusiness As CoreBusiness, ByRef oError As SAMErrorCollection)
        Try
            iUnderwritingyearid = 0
            'get the default underwriting year code
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_underwriting_year")

                cmd.AddInParameter("@required_date", SqlDbType.DateTime).Value = dtCoverStartDate
                cmd.AddInOutParameter("@underwriting_year_id", SqlDbType.Int)
                con.ExecuteNonQuery(cmd)

                If Cast.ToInt32(cmd.Parameters("@underwriting_year_id").Value, 0) <> 0 Then
                    iUnderwritingyearid = Cast.ToInt32(cmd.Parameters("@underwriting_year_id").Value, 0)
                End If

            End Using
        Catch

        Finally
            If iUnderwritingyearid <= 0 Then
                oError.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                   SAMInvalidData.InvalidLookupListValue.ToString,
                                                   "No Underwriting year configured in Lookup",
                                                 "Underwriting year")

            End If
        End Try
    End Sub

    ''' <summary>
    ''' ValidateAndGetValidUnderwritingYear
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="iUnderwritingyearid"></param>
    ''' <param name="dtCoverStartDate"></param>
    ''' <param name="oBusiness"></param>
    ''' <param name="oError"></param>
    ''' <remarks></remarks>
    Public Sub ValidateAndGetValidUnderwritingYear(ByVal con As SiriusConnection, ByRef iUnderwritingyearid As Integer, ByVal dtCoverStartDate As DateTime, ByRef oBusiness As CoreBusiness, ByRef oError As SAMErrorCollection)
        Try

            'get the default underwriting year code
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_ValidateAndGet_Underwriting_Year")

                cmd.AddInParameter("@required_date", SqlDbType.DateTime).Value = dtCoverStartDate
                cmd.AddInParameter("@iCheckUnderWritingYearId", SqlDbType.Int).Value = iUnderwritingyearid
                cmd.AddInOutParameter("@underwriting_year_id", SqlDbType.Int)
                con.ExecuteNonQuery(cmd)

                If Cast.ToInt32(cmd.Parameters("@underwriting_year_id").Value, 0) <> 0 Then
                    iUnderwritingyearid = Cast.ToInt32(cmd.Parameters("@underwriting_year_id").Value, 0)
                End If

            End Using
        Catch

        Finally
            If iUnderwritingyearid <= 0 Then
                oError.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                   SAMInvalidData.InvalidLookupListValue.ToString,
                                                   "No Underwriting year configured in Lookup",
                                                 "Underwriting year")

            End If
        End Try
    End Sub
    Public Function ProcessPaymentAndBindQuote(
    ByVal ProcessPaymentAndBindQuoteRequest As BaseProcessPaymentAndBindQuoteRequestType) As BaseProcessPaymentAndBindQuoteResponseType

        Dim oResponse As BaseProcessPaymentAndBindQuoteResponseType

        Try

            ' validate the request structure against the specified business rules
            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                                 _SiriusUser.Username, _SiriusUser.SourceID,
                                                  _SiriusUser.LanguageID,
                                                      SiriusUserDefaults.AppName)



                oResponse = ProcessPaymentAndBindQuote(con, ProcessPaymentAndBindQuoteRequest)

            End Using

        Catch

            Throw
        End Try

        Return oResponse

    End Function


    Private Function ProcessPaymentAndBindQuote(ByVal con As SiriusConnection,
            ByVal ProcessPaymentAndBindQuoteRequest As BaseProcessPaymentAndBindQuoteRequestType) As BaseProcessPaymentAndBindQuoteResponseType

        Dim oResponse As BaseImplementationTypes.BaseProcessPaymentAndBindQuoteResponseType
        Dim lInsuranceFileKey As Integer
        Dim lInsuranceFolderCnt As Integer
        Dim oErrorCollection As New SAMErrorCollection
        Dim failureReason As String = String.Empty
        Dim oGIS As bGIS.Application = Nothing
        Dim BindQuoteRequest As New BaseBindQuoteRequestType
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim sCurrencyCode As String
        Dim iDepositTransdetailID As Integer, lProductID As Integer, lPartyKey As Integer
        Dim m_sOption As String
        Dim sQuoteRef As String

        Dim lInsuranceFileCnt As Integer
        Dim lRiskCnt As Integer
        Dim lPolicyLinkID As Integer
        Dim dGrossAmount As Decimal
        Dim dCommissionAmount As Decimal
        Dim dIPTAmount As Decimal
        Dim dFeeAmount As Decimal = 0
        Dim sPolicyNum As String = String.Empty
        Dim sPaymentMethod As String = String.Empty
        Dim sDocumentComment As String = ""

        Dim sAutoGeneratedPlanRef As String
        Dim oBindQuoteResponse As New BaseBindQuoteResponseType
        Dim iAccountID As Integer


        Dim oReceiptCashListRequest As BaseCreateReceiptCashListWithItemsRequestType
        Dim oReceiptCashListResponse As BaseCreateReceiptCashListWithItemsResponseType

        Dim oReceipt As New BaseReceiptCashListType
        Dim oReceiptItem(0) As BaseReceiptCashListItemType
        Dim sBankAccountCode As String, sMediaTypeCode As String
        Dim sAccountShortCode As String
        Dim sSRPDocument As String
        Dim sJNDocument As String


        oResponse = New SAMForInsuranceV2ImplementationTypes.ProcessPaymentAndBindQuoteResponseType

        ProcessPaymentAndBindQuoteRequest.Validate(CType(oErrorCollection, Object))

        Using cmd As SiriusCommand = SiriusCommand.FromText("SELECT c.code CurrencyCode,Insurance_Ref,product_id,insured_cnt,source_id,insurance_folder_cnt,insurance_ref from Insurance_file ifi JOIN Currency c on c.Currency_id=ifi.Currency_id Where insurance_file_cnt=" & ProcessPaymentAndBindQuoteRequest.InsuranceFileKey.ToString())
            Dim ds As New DataSet
            ds = con.ExecuteDataSet(cmd, "PolicyDetails")

            Dim dr As DataRow
            If ds IsNot Nothing AndAlso ds.Tables("PolicyDetails") IsNot Nothing Then
                If ds.Tables("PolicyDetails").Rows.Count > 0 Then
                    dr = ds.Tables("PolicyDetails").Rows(0)
                    sCurrencyCode = dr.Item("CurrencyCode").ToString()
                    sQuoteRef = dr.Item("insurance_ref").ToString()
                    lProductID = Cast.ToInt32(dr.Item("product_id"), 0)
                    lPartyKey = Cast.ToInt32(dr.Item("insured_cnt"), 0)
                    lInsuranceFolderCnt = Cast.ToInt32(dr.Item("insurance_folder_cnt"), 0)
                    ProcessPaymentAndBindQuoteRequest.SourceId = Cast.ToInt32(dr.Item("source_id"), 0)
                End If
            End If
        End Using

        GetAccountDetailsForParty(con, lPartyKey, iAccountID, "", sAccountShortCode)


        oCoreBusiness.GetSystemOption(ProcessPaymentAndBindQuoteRequest.BranchCode, 5042, m_sOption)

        Dim iRet As Int32

        Try

            oGIS = New bGIS.Application

            ' Initialise the GIS
            SAMFunc.InitialiseGIS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

            If ProcessPaymentAndBindQuoteRequest.InstalmentDepositAmount > 0 Then

                iRet = oGIS.RunPaymentProcessingRule(ProcessPaymentAndBindQuoteRequest.InsuranceFileKey, lInsuranceFolderCnt, lPartyKey, ProcessPaymentAndBindQuoteRequest.PaymentGatewayToken, ProcessPaymentAndBindQuoteRequest.InstalmentDepositAmount, sCurrencyCode)

                If (iRet <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bGIS.Application.RunPaymentProcessingRule", iRet)
                End If

            End If


            Try
                con.BeginTransaction()

                '    Call BindQuote()
                BindQuoteRequest.BranchCode = ProcessPaymentAndBindQuoteRequest.BranchCode
                BindQuoteRequest.InsuranceFileKey = ProcessPaymentAndBindQuoteRequest.InsuranceFileKey
                BindQuoteRequest.AcceptRenewal = ProcessPaymentAndBindQuoteRequest.AcceptRenewal
                BindQuoteRequest.AcceptRenewalSpecified = ProcessPaymentAndBindQuoteRequest.AcceptRenewalSpecified
                BindQuoteRequest.CoverStartDate = ProcessPaymentAndBindQuoteRequest.CoverStartDate
                BindQuoteRequest.CoverStartDateSpecified = ProcessPaymentAndBindQuoteRequest.CoverStartDateSpecified
                BindQuoteRequest.InstalmentType = ProcessPaymentAndBindQuoteRequest.InstalmentType
                BindQuoteRequest.InstalmentTypeSpecified = ProcessPaymentAndBindQuoteRequest.InstalmentTypeSpecified
                BindQuoteRequest.IsBackdatedMTA = ProcessPaymentAndBindQuoteRequest.IsBackDatedMTA
                BindQuoteRequest.OverriddenPolicyNumber = ProcessPaymentAndBindQuoteRequest.OverriddenPolicyNumber
                BindQuoteRequest.PaymentMethod = ProcessPaymentAndBindQuoteRequest.PaymentMethod
                BindQuoteRequest.PaymentMethodSpecified = ProcessPaymentAndBindQuoteRequest.PaymentMethodSpecified
                BindQuoteRequest.QuoteTimeStamp = ProcessPaymentAndBindQuoteRequest.QuoteTimeStamp
                BindQuoteRequest.TransactionType = ProcessPaymentAndBindQuoteRequest.TransactionType
                BindQuoteRequest.SourceId = ProcessPaymentAndBindQuoteRequest.SourceId
                BindQuoteRequest.SelectedInstalmentQuoteSpecified = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuoteSpecified
                If ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote IsNot Nothing Then

                    BindQuoteRequest.BankAccountName = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAccountName
                    BindQuoteRequest.BankAccountNo = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAccountNo
                    BindQuoteRequest.BankSortCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankSortCode

                    If ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress IsNot Nothing Then
                        BindQuoteRequest.BankAddress = New BaseImplementationTypes.BaseAddressType
                        BindQuoteRequest.BankAddress.AddressLine1 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine1
                        BindQuoteRequest.BankAddress.AddressLine2 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine2
                        BindQuoteRequest.BankAddress.AddressLine3 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine3
                        BindQuoteRequest.BankAddress.AddressLine4 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine4
                        BindQuoteRequest.BankAddress.AddressTypeCode = CType(ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressTypeCode, BaseImplementationTypes.AddressTypeType)
                        BindQuoteRequest.BankAddress.CountryCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.CountryCode
                        BindQuoteRequest.BankAddress.PostCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.PostCode
                    End If

                    BindQuoteRequest.BankAreaCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAreaCode
                    BindQuoteRequest.BankBranch = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankBranch
                    BindQuoteRequest.BankExtn = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankExtn
                    BindQuoteRequest.BankFax = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankFax
                    BindQuoteRequest.BankFaxCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankFaxCode
                    BindQuoteRequest.BankName = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankName
                    BindQuoteRequest.BankPhone = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankPhone
                    BindQuoteRequest.SelectedSchemeNo = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.SelectedSchemeNo
                    BindQuoteRequest.SelectedSchemeVersion = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.SelectedSchemeVersion
                    BindQuoteRequest.QuoteDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.QuoteDate.Date
                    BindQuoteRequest.StartDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.StartDate.Date
                    BindQuoteRequest.EndDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.EndDate.Date
                    BindQuoteRequest.PreferredDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.PreferredDate.Date
                    BindQuoteRequest.MonthDay = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.MonthDay
                    BindQuoteRequest.WeekDay = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.WeekDay
                    BindQuoteRequest.AmountToFinance = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.AmountToFinance
                    BindQuoteRequest.PaymentProtection = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.PaymentProtection
                    BindQuoteRequest.OverrideRate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.OverrideRate
                    BindQuoteRequest.OverrideInterestRate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.OverrideInterestRate
                    BindQuoteRequest.AmountPaid = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.AmountPaid
                    BindQuoteRequest.PFRF_ID = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.PFRF_ID

                    BindQuoteRequest.PartyBankKey = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.PartyBankKey
                    BindQuoteRequest.IsUseTransactionCurrency = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.IsUseTransactionCurrency
                End If



                ProcessBindQuote(con, oCoreBusiness, BindQuoteRequest, DirectCast(oBindQuoteResponse, BaseBindQuoteResponseType), sAutoGeneratedPlanRef, iDepositTransdetailID)



                con.CommitTransaction()
            Catch
                con.RollbackTransaction()
                If ProcessPaymentAndBindQuoteRequest.InstalmentDepositAmount > 0 Then

                    Dim oCreateWmTaskRequest As BaseCreateWmTaskRequestType
                    Dim oCreateWmTaskResponse As BaseCreateWmTaskResponseType

                    oCreateWmTaskRequest = New SAMForInsuranceV2ImplementationTypes.CreateWmTaskRequestType
                    oCreateWmTaskResponse = New SAMForInsuranceV2ImplementationTypes.CreateWmTaskResponseType

                    oCreateWmTaskRequest.BranchCode = ProcessPaymentAndBindQuoteRequest.BranchCode
                    oCreateWmTaskRequest.AllocationUserGroup = m_sOption
                    oCreateWmTaskRequest.Client = sAccountShortCode
                    oCreateWmTaskRequest.UserId = _SiriusUser.UserID
                    oCreateWmTaskRequest.DueDateTime = Now
                    oCreateWmTaskRequest.TaskGroup = "COMMON"
                    oCreateWmTaskRequest.Task = "MEMO"

                    oCreateWmTaskRequest.Description = sQuoteRef & " - Error occurred during Policy make live process. Payment has been received from bank against reference number - " & ProcessPaymentAndBindQuoteRequest.PaymentGatewayToken & ". Process the refund."

                    oCreateWmTaskResponse = CreateWmTask(con, oCreateWmTaskRequest)
                End If
                Throw

            End Try


            ' Return the details
            oResponse.Policy = New BaseImplementationTypes.BaseTransactResponseTypePolicy
            ' Get the Premium, tax and commission details for this Quote
            GetQuoteDetails(sPolicyNum, lInsuranceFolderCnt, ProcessPaymentAndBindQuoteRequest.InsuranceFileKey, lRiskCnt, lPolicyLinkID, dGrossAmount, dCommissionAmount, dIPTAmount, oResponse.Policy, BindQuoteRequest.TransactionType, sDocumentComment)
            oResponse.Policy.PolicyRef = sPolicyNum
            oResponse.Policy.PremiumDueGross = dGrossAmount
            oResponse.Policy.PremiumDueNet = (dGrossAmount) - (dIPTAmount)
            oResponse.Policy.PremiumDueTax = dIPTAmount
            oResponse.Policy.TotalAnnualTax = dIPTAmount ' TODO where should this come from?
            oResponse.Policy.CommissionAmount = dCommissionAmount
            oResponse.Policy.DocumentComment = sDocumentComment
            'WPR 33-75 ADDED
            oResponse.Policy.AutoGeneratedPlanRef = sAutoPlanRef
            oResponse.Policy.DepositTransDetailID = iDepositTransdetailID

            sPolicyNum = sPolicyNum.Trim()

            SchedulePolicyDocument(con, oCoreBusiness, lPartyKey, ProcessPaymentAndBindQuoteRequest.InsuranceFileKey, ProcessPaymentAndBindQuoteRequest.SourceId, ProcessPaymentAndBindQuoteRequest.BranchCode, 1, lProductID)
            If iDepositTransdetailID > 0 Then
                Try

                    con.BeginTransaction()

                    oReceiptCashListRequest = New SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListWithItemsRequestType
                    oReceiptCashListResponse = New SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListWithItemsResponseType
                    oReceiptCashListRequest.BranchCode = ProcessPaymentAndBindQuoteRequest.BranchCode
                    oReceiptCashListRequest.SourceId = ProcessPaymentAndBindQuoteRequest.SourceId


                    Using cmd As SiriusCommand = SiriusCommand.FromText("SELECT b.code BankCode,m.code MediaTypeCode from bankAccount_Default bd JOIN BankAccount b on bd.bankAccount_id=b.bankAccount_id JOIN mediatype m on m.mediatype_id=bd.mediatype_id  Where source_id=" & BindQuoteRequest.SourceId & " AND cashlisttype_id=2 And bd.is_deleted=0")
                        Dim ds As New DataSet
                        ds = con.ExecuteDataSet(cmd, "BankDetails")

                        Dim dr As DataRow
                        If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing Then
                            If ds.Tables("BankDetails").Rows.Count > 0 Then
                                dr = ds.Tables("BankDetails").Rows(0)
                                sBankAccountCode = dr.Item("BankCode").ToString()
                                sMediaTypeCode = dr.Item("MediaTypeCode").ToString()
                            End If
                        End If
                    End Using

                    oReceipt.BranchCode = ProcessPaymentAndBindQuoteRequest.BranchCode
                    oReceipt.CurrencyCode = sCurrencyCode
                    oReceipt.ListDate = Now.Date
                    oReceipt.Reference = Left(sPolicyNum, 25)
                    oReceipt.BankAccountCode = sBankAccountCode
                    oReceipt.StatusCode = "E"
                    oReceipt.TypeCode = "R"

                    oReceiptItem(0) = New BaseReceiptCashListItemType
                    oReceiptItem(0).AccountShortCode = sAccountShortCode
                    oReceiptItem(0).AccountKey = iAccountID
                    oReceiptItem(0).AllocationStatusCode = "U"
                    oReceiptItem(0).Amount = ProcessPaymentAndBindQuoteRequest.InstalmentDepositAmount

                    oReceiptItem(0).MediaTypeCode = sMediaTypeCode
                    oReceiptItem(0).MediaReference = Left(sPolicyNum, 30)
                    oReceiptItem(0).StatusCode = "ADD"
                    oReceiptItem(0).TransactionDate = Now.Date
                    oReceiptItem(0).TypeCode = "STD"

                    If sMediaTypeCode.Trim() = "CC" Then
                        oReceiptItem(0).CreditCard = New BaseCreditCardType
                        oReceiptItem(0).CreditCard.Number = ProcessPaymentAndBindQuoteRequest.DepositPartialCCNumber
                        oReceiptItem(0).CreditCard.AuthCode = ProcessPaymentAndBindQuoteRequest.DepositCCAuthCode
                    End If

                    oReceipt.ReceiptItem = oReceiptItem

                    oReceiptCashListRequest.ReceiptCashList = oReceipt

                    oReceiptCashListResponse = CreateReceiptCashListWithItems(con, oReceiptCashListRequest)
                    con.CommitTransaction()

                Catch
                    con.RollbackTransaction()

                    Dim oCreateWmTaskRequest As BaseCreateWmTaskRequestType
                    Dim oCreateWmTaskResponse As BaseCreateWmTaskResponseType

                    oCreateWmTaskRequest = New SAMForInsuranceV2ImplementationTypes.CreateWmTaskRequestType
                    oCreateWmTaskResponse = New SAMForInsuranceV2ImplementationTypes.CreateWmTaskResponseType

                    oCreateWmTaskRequest.BranchCode = ProcessPaymentAndBindQuoteRequest.BranchCode
                    oCreateWmTaskRequest.AllocationUserGroup = m_sOption
                    oCreateWmTaskRequest.DueDateTime = Now
                    oCreateWmTaskRequest.Client = sAccountShortCode
                    oCreateWmTaskRequest.UserId = _SiriusUser.UserID
                    oCreateWmTaskRequest.TaskGroup = "SLACS"
                    oCreateWmTaskRequest.Task = "ACTRCTV2"
                    oCreateWmTaskRequest.Description = sPolicyNum & " - Policy has been made live but CashList Receipt has failed, needs to be generated manually"
                    oCreateWmTaskResponse = CreateWmTask(con, oCreateWmTaskRequest)

                    Throw

                End Try

                Try

                    con.BeginTransaction()
                    AllocateInstalmentDeposit(con, ProcessPaymentAndBindQuoteRequest.SourceId, iAccountID, ProcessPaymentAndBindQuoteRequest.InstalmentDepositAmount, oReceiptCashListResponse.CashListItem(0).CashListItemKey, oReceiptCashListResponse.CashListItem(0).TransDetailKey, iDepositTransdetailID)
                    con.CommitTransaction()

                Catch ex As Exception
                    con.RollbackTransaction()

                    Using cmd As SiriusCommand = SiriusCommand.FromText("SELECT Document_Ref,Transdetail_id from Transdetail td JOIN document d on td.document_id=d.document_id Where transdetail_id=" & oReceiptCashListResponse.CashListItem(0).TransDetailKey & " AND transdetail_id=" & iDepositTransdetailID)
                        Dim ds As New DataSet
                        ds = con.ExecuteDataSet(cmd, "DocumentDetails")

                        Dim dr As DataRow
                        If ds IsNot Nothing AndAlso ds.Tables("DocumentDetails") IsNot Nothing Then
                            If ds.Tables("DocumentDetails").Rows.Count > 0 Then
                                For Each dr In ds.Tables("DocumentDetails").Rows
                                    If iDepositTransdetailID = Cast.ToInt32(dr.Item("Transdetail_id"), 0) Then
                                        sJNDocument = dr.Item("Document_Ref").ToString()
                                    Else
                                        sSRPDocument = dr.Item("Document_Ref").ToString()
                                    End If
                                Next
                            End If
                        End If
                    End Using


                    Dim oCreateWmTaskRequest As BaseCreateWmTaskRequestType
                    Dim oCreateWmTaskResponse As BaseCreateWmTaskResponseType

                    oCreateWmTaskRequest = New SAMForInsuranceV2ImplementationTypes.CreateWmTaskRequestType
                    oCreateWmTaskResponse = New SAMForInsuranceV2ImplementationTypes.CreateWmTaskResponseType

                    oCreateWmTaskRequest.BranchCode = ProcessPaymentAndBindQuoteRequest.BranchCode
                    oCreateWmTaskRequest.AllocationUserGroup = m_sOption
                    oCreateWmTaskRequest.Client = sAccountShortCode
                    oCreateWmTaskRequest.DueDateTime = Now
                    oCreateWmTaskRequest.UserId = _SiriusUser.UserID
                    oCreateWmTaskRequest.TaskGroup = "SLACS"
                    oCreateWmTaskRequest.Task = "FINDTXN"
                    oCreateWmTaskRequest.Description = sPolicyNum & " - Receipt has been generated but Allocation of Receipt " & sSRPDocument & " has failed against Journal " & sJNDocument & ", needs to be done manually"
                    oCreateWmTaskResponse = CreateWmTask(con, oCreateWmTaskRequest)
                    Throw
                End Try

                iRet = oGIS.RunPostPaymentProcessingRule(ProcessPaymentAndBindQuoteRequest.InsuranceFileKey, lInsuranceFolderCnt, lPartyKey, ProcessPaymentAndBindQuoteRequest.PaymentGatewayToken, sPolicyNum, m_sOption)

                If (iRet <> PMEReturnCode.PMTrue) Then
                    RaiseComMethodException("bGIS.Application.RunPaymentProcessingRule", iRet)
                End If


            End If
        Catch
            Throw
        Finally
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
        End Try


        Return oResponse

    End Function


    Private Sub AllocateInstalmentDeposit(ByVal con As SiriusConnection, ByVal iSourceID As Integer, ByVal lAccountKey As Integer, ByVal AllocationAmount As Decimal, ByVal CashListItemKey As Integer, ByVal CashListTransDetailID As Integer, ByVal InstalmentDepositTransDetailID As Integer)

        Dim vKeys(0 To 1, 6) As Object



        vKeys(0, 0) = "account_id"
        vKeys(1, 0) = lAccountKey

        'TransdetailKey
        vKeys(0, 1) = "trans_detail_id"
        vKeys(1, 1) = CashListTransDetailID.ToString & "|" & (-AllocationAmount).ToString

        ' AllocationTransdetailKeys
        vKeys(0, 2) = "trans_detail_ids"

        Dim oAllocationDetails() As Object = Nothing
        ReDim oAllocationDetails(0)

        oAllocationDetails(0) = InstalmentDepositTransDetailID.ToString & "|" & AllocationAmount.ToString


        vKeys(1, 2) = oAllocationDetails

        vKeys(0, 3) = "writeoff_reason_id"
        vKeys(1, 3) = 0

        vKeys(0, 4) = "writeoff_amount"
        vKeys(1, 4) = 0

        vKeys(0, 5) = "currency_difference"
        vKeys(1, 5) = 0

        vKeys(0, 6) = "cashlistitem_id"
        vKeys(1, 6) = CashListItemKey

        Dim oAllocationManual As bACTAllocationManual.Business
        oAllocationManual = New bACTAllocationManual.Business
        SAMFunc.InitialiseSBOObject(con, oAllocationManual, _SiriusUser, "bACTAllocationManual.Business")

        Dim icomReturnValue As Integer
        icomReturnValue = oAllocationManual.SetProcessModes(vTask:=2)
        If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMReturnCode.PMNotFound Then
            RaiseComMethodException("bACTAllocationManual.Business.SetProcessModes", icomReturnValue)
        End If

        ' Set the keys
        icomReturnValue = oAllocationManual.SetKeys(vKeyArray:=DirectCast(vKeys, Object(,)))
        If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMReturnCode.PMNotFound Then
            RaiseComMethodException("bACTAllocationManual.Business.SetKeys", icomReturnValue)
        End If

        oAllocationManual.CompanyId = iSourceID

        icomReturnValue = oAllocationManual.Start()
        If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMReturnCode.PMNotFound Then
            RaiseComMethodException("bACTAllocationManual.Business.Start", icomReturnValue)
        End If

        If oAllocationManual IsNot Nothing Then
            oAllocationManual.Dispose()
            oAllocationManual = Nothing
        End If
        oAllocationManual = Nothing
    End Sub



    Private Sub SchedulePolicyDocument(ByVal con As SiriusConnection, ByVal oCoreBusiness As CoreBusiness, ByVal iPartyKey As Integer, ByVal iInsuranceFileKey As Integer, ByVal iSourceID As Integer, ByVal sBranchCode As String, ByVal ProcessType As Integer, ByVal ProductID As Integer)
        Dim sArchiveAsPDF As String
        Const ARCHIVE_AS_PDF As Integer = 5009
        Dim ds As New DataSet
        Dim sPageType As String
        Dim oCreateBackgroundJobRequest As BaseCreateBackgroundJobRequestType
        Dim oCreateBackgroundJobResponse As BaseCreateBackgroundJobResponseType

        oCoreBusiness.GetSystemOption(sBranchCode, ARCHIVE_AS_PDF, sArchiveAsPDF)
        If sArchiveAsPDF <> "0" Then
            sPageType = "PDF"
        Else
            sPageType = "XML"
        End If

        Dim xlItem As Linq.XElement = New Linq.XElement("BACKGROUND_JOB")
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_GetSFIDocumentTemplates")
            cmd.AddInParameter("@functional_area", SqlDbType.Int).Value = 1
            cmd.AddInParameter("@product_Id", SqlDbType.Int).Value = ProductID
            cmd.AddInParameter("@Process_Type_Id", SqlDbType.Int).Value = ProcessType
            cmd.AddInParameter("@source_id", SqlDbType.Int).Value = iSourceID

            ds = con.ExecuteDataSet(cmd, "Documents")

            Dim dr As DataRow
            If ds IsNot Nothing AndAlso ds.Tables("Documents") IsNot Nothing Then
                If ds.Tables("Documents").Rows.Count > 0 Then
                    Dim DocumentTemplateDesc As String, DocumentTemplateCode As String, DocumentTemplateGroupID As Integer, DocumentTemplateSubGroupID As Integer, InternalOnly As Boolean
                    For Each dr In ds.Tables(0).Rows
                        oCreateBackgroundJobRequest = New SAMForInsuranceV2ImplementationTypes.CreateBackgroundJobRequestType
                        oCreateBackgroundJobResponse = New SAMForInsuranceV2ImplementationTypes.CreateBackgroundJobResponseType

                        DocumentTemplateDesc = dr.Item("Description").ToString()
                        DocumentTemplateCode = dr.Item("Code").ToString()
                        DocumentTemplateGroupID = Cast.ToInt32(dr.Item("document_template_group_id"), 0)
                        DocumentTemplateSubGroupID = Cast.ToInt32(dr.Item("document_template_sub_group_id"), 0)
                        InternalOnly = Cast.ToInt32(dr.Item("InternalOnly"), 0) = 1
                        oCreateBackgroundJobRequest.BranchCode = sBranchCode
                        oCreateBackgroundJobRequest.Description = "Archive documents"
                        oCreateBackgroundJobRequest.JobWhenToStart = Now
                        oCreateBackgroundJobRequest.SourceId = iSourceID
                        xlItem = <BACKGROUND_JOB>
                                     <JOB jobtype="DOCUPACK">
                                         <PARAMETERS>
                                             <PARAMETER name="destination" value="archive"/>
                                             <PARAMETER name="archive" value="true"/>
                                             <PARAMETER name="PartyCnt" value=<%= iPartyKey %>/>
                                             <PARAMETER name="ClaimID" value="0"/>
                                             <PARAMETER name="InsuranceFileCnt" value=<%= iInsuranceFileKey %>/>
                                             <PARAMETER name="code" value=<%= DocumentTemplateCode.Trim() %>/>
                                             <PARAMETER name="Internalonly" value=<%= IIf(InternalOnly, "true", "false") %>/>
                                             <PARAMETER name="DocumentTemplateGroupID" value=<%= DocumentTemplateGroupID %>/>
                                             <PARAMETER name="DocumentTemplateSubGroupID" value=<%= DocumentTemplateSubGroupID %>/>
                                             <PARAMETER name="OutputFormat" value=<%= sPageType %>/>
                                             <PARAMETER name="DestinationFilename" value=""/>
                                         </PARAMETERS>
                                         <ITEMS></ITEMS></JOB>
                                 </BACKGROUND_JOB>
                        oCreateBackgroundJobRequest.JobXML = xlItem.ToString()
                        oCreateBackgroundJobResponse = CreateBackgroundJob(con, oCreateBackgroundJobRequest)
                    Next
                End If
            End If
        End Using

    End Sub
    ''' <summary>
    ''' Creates a copy of a risk that currently points to an old risk and sets the risk as unquoted
    ''' </summary>
    ''' <param name="con">Database connection</param>
    ''' <param name="listRisksBusiness">Back Office List Risk instance</param>
    ''' <param name="insuranceFileKey">Insurance file</param>
    ''' <param name="riskCnt">The risk that must be copied</param>
    ''' <param name="newRiskCnt">The copied risks cnt</param>
    ''' <param name="TransactionType">Transaction type for this version</param>
    Private Sub CopyMTARisk(
     ByRef con As SiriusConnection,
     ByRef listRisksBusiness As bSIRListRisks.Business,
     ByVal insuranceFileKey As Integer,
     ByVal riskCnt As Integer,
     ByRef newRiskCnt As Integer,
Optional ByRef TransactionType As TransactionType = Nothing)
        'Added  TransactionType as parameter for sub method CopyMTARisks to update the risk status in risk table(SAM Gap done by Vijayakumar as per discussed with Gaurav on 06-Nov-2008)
        Dim samErrorCollection As SAMErrorCollection = Nothing
        Dim comReturnValue As Integer

        comReturnValue = listRisksBusiness.SetProcessModes(vTransactionType:=TransactionType.ToString)

        comReturnValue = listRisksBusiness.CopyRisksMTAEx(insuranceFileKey, True, v_lOnlyRiskCnt:=riskCnt, r_lLastNewRiskCnt:=newRiskCnt)

        If comReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRListRisks.Business.CopyMTARisk", comReturnValue)
        ElseIf comReturnValue = PMEReturnCode.PMTrue AndAlso (TransactionType = TransactionType.MTC Or TransactionType = TransactionType.MTR) Then
            'Implemented stored Procedure "spu_SIR_Update_Risk_Status" to update the risk status in risk table for TransactionType MTC and MTR (SAM Gap done by Vijayakumar as per discussed with Gaurav on 06-Nov-2008)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Update_Risk_Status")
                cmd.AddInParameter("@risk_status_code", SqlDbType.VarChar, 20).Value = "UNQUOTED"
                cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = newRiskCnt
                con.ExecuteNonQuery(cmd)
            End Using
        End If

    End Sub
#Region "AddMandatoryRisk"
    Private Sub AddMandatoryRisk(ByVal con As SiriusConnection,
                 ByVal iProductId As Integer, ByRef bIsMandatoryRisk As Boolean,
                 ByVal iInsuranceFileKey As Integer, ByVal iInsuranceFolderKey As Integer,
                 ByRef oCreateQuoteResponse As BaseAddQuoteV2ResponseType,
                 ByVal sBranchCode As String)
        ' WPR53 : Implement AddRisk
        Dim obSIRRiskScreen As bSIRRiskScreen.Business = Nothing
        Try
            Const ACMethodName As String = "AddMandatoryRisk"
            'Declare the input and output objects for core add quote method
            Dim oAddRiskIn As New AddRiskIn
            Dim oAddRiskOut As AddRiskOut = Nothing
            Dim lRiskTypeId As Long = 0
            Dim sRiskDescription As String = String.Empty
            Dim lRiskScreenId As Long = 0
            Dim obGIS As bGIS.Application = Nothing
            Dim iFunctionReturn As Integer = 0
            Dim lGISDataModelId As Long = 0
            Dim sGISDataModel As String = String.Empty
            Dim oAdditionalData() As AdditionalData = Nothing
            Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
            Dim AddRiskRequest As BaseAddRiskRequestType
            Dim sXMLDataset As String = String.Empty
            Dim sScreenCode As String

            obSIRRiskScreen = New bSIRRiskScreen.Business
            ' Get Mandatory Risk Type Details
            Dim dsResult As DataSet = Nothing
            If iProductId > 0 Then
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Mandatory_Risk_Sel")
                    cmd.AddInParameter("@ProductId", SqlDbType.Int).Value = iProductId
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = iInsuranceFileKey
                    dsResult = con.ExecuteDataSet(cmd, "GetMandatoryRisk")
                End Using
                If dsResult IsNot Nothing Then
                    If dsResult.Tables.Count > 0 Then
                        If dsResult.Tables(0).Rows.Count > 0 Then
                            lRiskTypeId = Cast.ToInt64(dsResult.Tables(0).Rows(0).Item("Risk_Type_Id"), 0)
                            sRiskDescription = Cast.ToString(dsResult.Tables(0).Rows(0).Item("description"), "")
                            lRiskScreenId = Cast.ToInt64(dsResult.Tables(0).Rows(0).Item("gis_screen_id"), 0)
                        End If
                    End If
                End If
            End If

            ' If there is any mandatory risk attached then only allow to proceed to AddRisk
            If lRiskTypeId > 0 Then
                SAMFunc.InitialiseSBOObject(con, obSIRRiskScreen, _SiriusUser, "bSIRRiskScreen.Business")
                iFunctionReturn = obSIRRiskScreen.SetProcessModes(vTask:=CStr(PMEComponentAction.PMAdd),
                                                              vNavigate:="1",
                                                              vProcessMode:="1",
                                                              vTransactionType:="AOL",
                                                              vEffectiveDate:=CStr(Now))
                If (iFunctionReturn <> PMReturnCode.PMTrue And iFunctionReturn <> PMReturnCode.PMNotFound) Then
                    Exit Sub
                End If

                Dim r_lRiskFolderCnt As Long = 0
                Dim r_lRiskCnt As Long = 0
                ' Set the Business object keys
                obSIRRiskScreen.InsuranceFolderCnt = iInsuranceFolderKey
                obSIRRiskScreen.InsuranceFileCnt = iInsuranceFileKey
                obSIRRiskScreen.RiskId = CInt(r_lRiskCnt)
                obSIRRiskScreen.RiskTypeId = CInt(lRiskTypeId)
                obSIRRiskScreen.ProductId = iProductId
                obSIRRiskScreen.RiskFolderCnt = CInt(r_lRiskFolderCnt)

                If (lRiskScreenId > 0) Then
                    ' Set the risk screen id
                    obSIRRiskScreen.ScreenId = CInt(lRiskScreenId)
                    ' Get DataModel code
                    iFunctionReturn = obSIRRiskScreen.GetGISDataModel(r_lGISDataModelId:=CInt(lGISDataModelId),
                                                                        r_sGISDataModel:=sGISDataModel)
                    If (iFunctionReturn <> PMReturnCode.PMTrue) Then
                        Exit Sub
                    End If
                End If

                'AddRiskRequest As BaseAddRiskRequestType
                With oAddRiskIn
                    .BackOfficeMapperCode = InternalSAMConstants.CNAgentsOnline
                    .DataModelCode = sGISDataModel
                    .BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
                    .InsuranceFileCnt = iInsuranceFileKey
                    .InsuranceFolderCnt = iInsuranceFolderKey
                    .RiskScreenId = CInt(lRiskScreenId)
                    .RiskTypeId = CInt(lRiskTypeId)
                    .ProductID = iProductId
                    .AdditionalDataArray = oAdditionalData
                    .RiskDescription = sRiskDescription
                End With

                Try
                    oAddRiskOut = oCoreBusiness.AddRisk(con, oAddRiskIn)
                Catch ex As ApplicationException
                    Dim STSErrorEx As STSErrorPublisher = New STSErrorPublisher(STSErrorCodes.FailedToAddRiskRecord, "Failed to add risk record", "Failed to add risk record")
                    STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "SiriusFS.STS." & ACMethodName, True)
                End Try

                bIsMandatoryRisk = True
                ' Might need to update Sasria Risk to UnQuote
                If Cast.ToInt32(oAddRiskOut.RiskCnt, 0) > 0 Then
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Update_Mandatory_Risk_Details")
                        cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = oAddRiskOut.RiskCnt
                        cmd.AddInParameter("@risk_status_id", SqlDbType.Int).Value = 4 ' For status UnQuote
                        cmd.AddInParameter("@description", SqlDbType.Text).Value = sRiskDescription
                        cmd.AddInParameter("@variation_number", SqlDbType.Int).Value = 0
                        con.ExecuteNonQuery(cmd)
                    End Using
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Update_Mandatory_Risk")
                        cmd.AddInParameter("@riskId", SqlDbType.Int).Value = oAddRiskOut.RiskCnt
                        con.ExecuteNonQuery(cmd)
                    End Using
                End If
                Dim oSAMBusiness As New CoreSAMBusiness()
                Dim oRulesIn As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseRunDefaultRulesAddRequestType
                Dim oRulesOut As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseRunDefaultRulesAddResponseType
                sScreenCode = oCoreBusiness.GetListItemFromID(Core.STSListType.PMLookup, "gis_screen", CInt(lRiskScreenId))
                oRulesIn.BranchCode = sBranchCode
                oRulesIn.ScreenCode = sScreenCode
                oRulesIn.DataModelCode = sGISDataModel
                oRulesIn.XMLDataSet = oAddRiskOut.XMLDataset
                oRulesOut = oSAMBusiness.RunDefaultRulesAdd(oRulesIn)
                oCreateQuoteResponse.RiskKey = oAddRiskOut.RiskCnt
                oCreateQuoteResponse.RiskFolderKey = oAddRiskOut.RiskFolderCnt
                oCreateQuoteResponse.XMLDataSet = oRulesOut.XMLDataSet
            Else
                bIsMandatoryRisk = False
            End If

        Catch ex As Exception
            Throw
        Finally
            If obSIRRiskScreen IsNot Nothing Then
                obSIRRiskScreen.Dispose()
                obSIRRiskScreen = Nothing
            End If
        End Try
    End Sub

    ''' <summary>
    ''' This method is called to delete unselected risk
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="branchCode"></param>
    ''' <param name="insuranceFileKey"></param>
    ''' <remarks></remarks>
    Private Sub DeleteUnselectedRisks(ByRef con As SiriusConnection,
  ByVal branchCode As String,
  ByVal insuranceFileKey As Integer)

        Dim oChangePolicyStatusBusiness As bSIRChangePolicyStatus.Business = CreateAndInitialiseChangePolicyStatusBusiness(con, branchCode)

        Try

            Dim nComReturnValue As Integer = 0
            Dim oRisks As Object(,) = Nothing
            nComReturnValue = oChangePolicyStatusBusiness.GetRisksByStatus(
                                                                 v_lInsuranceFileCnt:=insuranceFileKey,
                                                                r_vRisks:=oRisks)

            nComReturnValue = oChangePolicyStatusBusiness.DeleteRisks(v_vrisks:=oRisks)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRChangePolicyStatus.Business.ChangePolicyStatus", nComReturnValue)
            End If

            ' rejig the risk number to take into account any risks that may have been deleted
            RenumberRisks(oChangePolicyStatusBusiness, insuranceFileKey)

        Finally

            If oChangePolicyStatusBusiness IsNot Nothing Then
                oChangePolicyStatusBusiness.Dispose()
                oChangePolicyStatusBusiness = Nothing
            End If
        End Try


    End Sub
#End Region


#Region "Cancel Quote"

    ''' <summary>
    ''' To Cancel a MTA Quote. It will delete the quote version from database
    ''' </summary>
    ''' <param name="CancelQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function CancelQuote(ByVal CancelQuoteRequest As BaseCancelQuoteRequestType) As BaseCancelQuoteResponseType

        ' validate the request structure against the specified business rules

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseCancelQuoteResponseType

            oResponse = CancelQuote(con, CancelQuoteRequest)

            Return oResponse
        End Using

    End Function

    ''' <summary>
    ''' To Cancel a MTA Quote.It will delete the quote version from database
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oCancelQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function CancelQuote(ByVal con As SiriusConnection, ByVal oCancelQuoteRequest As BaseCancelQuoteRequestType) As BaseCancelQuoteResponseType

        Dim oCancelQuoteResponse As New BaseCancelQuoteResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim o_aRetrunTimeStamp() As Byte = Nothing

        Dim nTypeOfPackage As enumTypeOfPackage

        If oCancelQuoteRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CancelQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oCancelQuoteResponse = New SAMForInsuranceV2ImplementationTypes.CancelQuoteResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        'Mandatory validations
        oCancelQuoteRequest.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()

        If oCancelQuoteRequest.InsuranceFileKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
            SAMInvalidData.MandatoryInputMissing.ToString,
            "InsuranceFileKey")
        End If
        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()

        ' Retrieve the insurance file details.
        Dim insuranceFolderCnt As Integer = 0
        Dim partyCnt As Integer = 0
        Dim insuranceFileDetails As New BaseClaimType

        insuranceFileDetails.InsuranceFileKey = oCancelQuoteRequest.InsuranceFileKey
        GetInsuranceFileDetails(con, insuranceFileDetails)

        insuranceFolderCnt = insuranceFileDetails.InsuranceFolderCnt
        partyCnt = insuranceFileDetails.InsuredCnt

        Dim AnyError As STSErrorType
        AnyError = oCoreBusiness.CheckTSAndLock(con,
            BranchCode:=oCancelQuoteRequest.BranchCode,
            Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
            LockValue:=insuranceFolderCnt,
            TStamp:=oCancelQuoteRequest.TimeStamp)

        If AnyError Is Nothing = False Then
            oCancelQuoteResponse.STSError = AnyError
            Return oCancelQuoteResponse
        End If

        Try
            Dim eventDescription As String = "Endorsement Cancelled"

            ' Create Accept Renewal Event
            CreateEvent(con, partyCnt,
                insuranceFolderCnt, oCancelQuoteRequest.InsuranceFileKey,
                _SiriusUser.UserID, Date.Today,
                eventDescription,
                EventTypeCode.ChangeOfPolicyDetails)

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_sir_cancel_quote")
                cmd.AddInParameter("@nInsuranceFileCnt", SqlDbType.Int).Value = Cast.NullIfDefault(oCancelQuoteRequest.InsuranceFileKey)
                con.ExecuteNonQuery(cmd)
            End Using

            ' Unlock. No need to return the new timestamp as this function should release the policy.
            AnyError = oCoreBusiness.UnlockAndGetTS(
                BranchCode:=oCancelQuoteRequest.BranchCode,
                Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
                LockValue:=insuranceFolderCnt,
                TStamp:=o_aRetrunTimeStamp)

            ' Check for Errors
            If AnyError Is Nothing = False Then
                ' Unable to unlock, return the error.
                oCancelQuoteResponse.STSError = AnyError
                Return oCancelQuoteResponse
            End If

            Return oCancelQuoteResponse

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function
#End Region

    Public Overloads Function UpdateMarketplacePolicyStatus(ByVal oUpdateMarketplacePolicyStatusRequest As BaseUpdateMarketplacePolicyStatusRequestType) As BaseUpdateMarketplacePolicyStatusResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseUpdateMarketplacePolicyStatusResponseType

            oResponse = UpdateMarketplacePolicyStatus(con, oUpdateMarketplacePolicyStatusRequest)

            Return oResponse

        End Using
    End Function

    Public Overloads Function UpdateMarketplacePolicyStatus(ByVal con As SiriusConnection, ByVal oUpdateMarketplacePolicyStatusRequest As BaseUpdateMarketplacePolicyStatusRequestType) As BaseUpdateMarketplacePolicyStatusResponseType

        Dim oUpdateMarketplacePolicyStatusResponse As New BaseUpdateMarketplacePolicyStatusResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection

        Dim nTypeOfPackage As enumTypeOfPackage

        If oUpdateMarketplacePolicyStatusRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateMarketplacePolicyStatusRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateMarketplacePolicyStatusResponse = New SAMForInsuranceV2ImplementationTypes.UpdateMarketplacePolicyStatusResponseType
        End If
        '*******************************************************************************************
        'Structure Validation
        '*******************************************************************************************

        ' Check mandatory fields
        If oUpdateMarketplacePolicyStatusRequest.InsuranceFileKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                        SAMInvalidData.MandatoryInputMissing.ToString,
                                        "InsuranceFile")
        End If

        '' exit if there are any missing parameters
        oSAMErrorCollection.CheckForErrors()

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Update_MarketplacePolicy_Status")
            cmd.AddInParameter("@nInsuranceFileKey", SqlDbType.Int).Value = oUpdateMarketplacePolicyStatusRequest.InsuranceFileKey
            cmd.AddInParameter("@nIsMarketplacePolicy", SqlDbType.Int).Value = IIf(oUpdateMarketplacePolicyStatusRequest.IsMarketPlacePolicy, 1, 0)
            con.ExecuteNonQuery(cmd)
        End Using

        oSAMErrorCollection.CheckForErrors()

        Return oUpdateMarketplacePolicyStatusResponse
    End Function


    Public Function FindPoliciesByRiskIndex(ByVal oFindPoliciesByRiskIndexRequest As SAMForInsuranceV2ImplementationTypes.BaseFindPoliciesByRiskIndexRequestType) As SAMForInsuranceV2ImplementationTypes.BaseFindPoliciesByRiskIndexResponseType

        Dim oResponse As SAMForInsuranceV2ImplementationTypes.BaseFindPoliciesByRiskIndexResponseType

        Try

            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

                oResponse = FindPoliciesByRiskIndex(con, oFindPoliciesByRiskIndexRequest.RiskIndex, oFindPoliciesByRiskIndexRequest.PartyKey)

            End Using

        Catch

            Throw
        End Try

        Return oResponse

    End Function


    Public Function FindPoliciesByRiskIndex(ByVal con As SiriusConnection, ByVal riskIndex As String, Optional ByVal nPartyKey As Integer = 0, Optional ByVal lMaxRecords As Long = -1) As SAMForInsuranceV2ImplementationTypes.BaseFindPoliciesByRiskIndexResponseType


        Dim oResponse As SAMForInsuranceV2ImplementationTypes.BaseFindPoliciesByRiskIndexResponseType

        oResponse = New SAMForInsuranceV2ImplementationTypes.FindPoliciesByRiskIndexResponseType

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_FindPolicies_By_RiskIndex")
            cmd.AddInParameter("@search_object_name", SqlDbType.VarChar, 70).Value = String.Empty
            cmd.AddInParameter("@search_value", SqlDbType.VarChar, 255).Value = riskIndex
            cmd.AddInParameter("@Specials_Type_Filter", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = nPartyKey
            If lMaxRecords <> -1 Then
                cmd.AddInParameter("@MaxRowsToFetch", SqlDbType.Int).Value = lMaxRecords
            End If

            Dim dt As DataTable = Nothing
            dt = con.ExecuteDataTable(cmd)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                oResponse.InsuranceFolderKeys = Cast.ToStringTrim(dr.Item(0))
                oResponse.InsuranceFileKeys = Cast.ToStringTrim(dr.Item(1))
                oResponse.RiskKeys = Cast.ToStringTrim(dr.Item(2))
            End If
        End Using
        Return oResponse
    End Function
#Region "CheckPendingOOSVersions"
    ''' <summary>
    ''' This method will return boolean flag depending on pending OOS version on policy
    ''' </summary>
    ''' <param name="oCheckPendingOOSVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function CheckPendingOOSVersions(ByVal oCheckPendingOOSVersionsRequest As BaseCheckPendingOOSVersionsRequestType) As BaseCheckPendingOOSVersionsResponseType


        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseCheckPendingOOSVersionsResponseType

            oResponse = CheckPendingOOSVersions(con, oCheckPendingOOSVersionsRequest)

            Return oResponse

        End Using

    End Function
    ''' <summary>
    ''' This method will return boolean flag depending on pending OOS version on policy
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function CheckPendingOOSVersions(ByRef con As SiriusConnection,
    ByVal oRequest As BaseCheckPendingOOSVersionsRequestType) As BaseCheckPendingOOSVersionsResponseType

        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseCheckPendingOOSVersionsResponseType

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CheckPendingOOSVersionsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.CheckPendingOOSVersionsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oErrorCollection As New SAMErrorCollection

        '*******************
        ' DATA VALIDATION
        '*******************
        'Mandatory Validation
        If String.IsNullOrEmpty(oRequest.BranchCode) Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "BranchCode")
        End If

        If oRequest.InsuranceFolderKey = 0 Then
            oErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, "InsuranceFolderKey")
        End If

        Dim nBranchCodeId As Integer
        Dim nInsuranceFileKey As Integer

        Try
            nBranchCodeId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                    SAMInvalidData.InvalidLookupListValue.ToString,
                                    "BranchCode",
                                    oRequest.BranchCode)
        End Try

        If oRequest.InsuranceFolderKey <> 0 Then
            nInsuranceFileKey = GetAndValidateSpecifiedTableCode(con, "Insurance_Folder", "Insurance_Folder_Cnt", "Insurance_Folder_Cnt", oRequest.InsuranceFolderKey.ToString, oErrorCollection, "InsuranceFolderKey")
            oErrorCollection.CheckForErrors()
        End If

        oErrorCollection.CheckForErrors()

        ' Dataset that will hold the returned results		
        Dim ds As DataSet = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_saved_oos_versions")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oRequest.InsuranceFileKey
            cmd.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = oRequest.InsuranceFolderKey
            ds = con.ExecuteDataSet(cmd, "Row")

        End Using

        If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            oResponse.ResultData = ds.Tables(0)
        End If
        Return oResponse

    End Function
    Public Function CheckDocumentTemplateExists(ByVal oGetDocumentTemplateStatusRequest As SAMForInsuranceV2ImplementationTypes.BaseGetDocumentTemplateStatusRequestType) As SAMForInsuranceV2ImplementationTypes.BaseGetDocumentTemplateStatusResponseType

        Dim oResponse As SAMForInsuranceV2ImplementationTypes.BaseGetDocumentTemplateStatusResponseType

        Try

            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

                oResponse = CheckDocumentTemplateExists(con, oGetDocumentTemplateStatusRequest.DocumentTemplateKey, oGetDocumentTemplateStatusRequest.BranchCode)

            End Using

        Catch

            Throw
        End Try

        Return oResponse

    End Function


    Public Function CheckDocumentTemplateExists(ByVal con As SiriusConnection, ByVal DocumentTemplateKey As Integer, ByVal sBranchCode As String) As SAMForInsuranceV2ImplementationTypes.BaseGetDocumentTemplateStatusResponseType

        Dim oResponse As SAMForInsuranceV2ImplementationTypes.BaseGetDocumentTemplateStatusResponseType
        Dim nReturn As Integer
        Dim sOptionValue As String = String.Empty
        Dim sDatasetSchemaPath As String = String.Empty
        oResponse = New SAMForInsuranceV2ImplementationTypes.GetDocumentTemplateStatusResponseType
        Const ACMethodName As String = "GetDatasetSchema"
        Const ACDatasetSchemaKey As String = "DatasetSchemaKey_"
        Dim sDocumentTypeID As String = String.Empty
        Dim sCCMDocumentTemplate As String = String.Empty
        Dim isKCMApplicableForSelectedDocument As String = String.Empty


        nReturn = GetPMRegSetting(PMERegSettingRoot.pmeRSRLocalMachine, PMEProductFamily.pmePFSiriusSolutions, PMERegSettingLevel.pmeRSLClient, "DocServer", sDatasetSchemaPath)
        If nReturn <> PMEReturnCode.PMTrue Then
            'Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.DatasetPathRegistrySettingNotFound, "GetPMRegSetting failed", "The registry setting for the datasets path does not exist for datamodel " & GetDatasetSchemaRequest.DataModelCode)
            'STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDatasetSchema", True)
            'Return oResponse
        End If
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_document_template_sel")
            cmd.AddInParameter("@document_template_id", SqlDbType.Int).Value = DocumentTemplateKey

            Dim dt As DataTable = Nothing
            dt = con.ExecuteDataTable(cmd)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                sDocumentTypeID = Convert.ToString(dr.Item("document_type_id")).Trim()
                sCCMDocumentTemplate = Convert.ToString(dr.Item("CCMDocumentTemplate")).Trim()
                oResponse.DocumentTemplateStatus = True
            Else
                oResponse.DocumentTemplateStatus = False
            End If
        End Using
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        oCoreBusiness.GetSystemOption(sBranchCode, SystemOption.DocumentProductionInHouseOrCCM, sOptionValue)
        Dim isKCMForSelectedDocument As String = String.Empty
        oCoreBusiness.GetSystemOption(sBranchCode, SystemOption.IsKCMApplicableForSelectedDocument, isKCMForSelectedDocument)

        'Is KCM applicable only for selected documents
        If Not (String.IsNullOrEmpty(sOptionValue) OrElse sOptionValue = "0") Then
            oCoreBusiness.GetSystemOption(sBranchCode, SystemOption.KCMApplicableForSelectedTemplate, isKCMApplicableForSelectedDocument)
            If String.IsNullOrEmpty(isKCMApplicableForSelectedDocument) Then
                isKCMApplicableForSelectedDocument = "0"
            End If
        Else
            isKCMApplicableForSelectedDocument = "0"
        End If


        If sOptionValue = "1" AndAlso (isKCMApplicableForSelectedDocument = "0" OrElse (Not String.IsNullOrEmpty(sCCMDocumentTemplate))) Then
            If isKCMForSelectedDocument = "0" AndAlso String.IsNullOrEmpty(sCCMDocumentTemplate) Then
                oResponse.DocumentTemplateStatus = False
            ElseIf isKCMForSelectedDocument = "1" AndAlso String.IsNullOrEmpty(sCCMDocumentTemplate) AndAlso
                String.IsNullOrEmpty(sDocumentTypeID) Then
                oResponse.DocumentTemplateStatus = False
            Else
                oResponse.DocumentTemplateStatus = True
            End If
        Else
            Dim sFolderName As String = "TYPE " & sDocumentTypeID
            Dim sDocumentFolderName As String = "Doc " & Convert.ToString(DocumentTemplateKey)
            'dim sFUllPath as String= = @sPMDocPath & '\' & @sFolderName & '\' & @sDocumentFolderName & '.zip'
            Dim sFUllPath As String = sDatasetSchemaPath & "\" & sFolderName & "\" & sDocumentFolderName & ".zip"
            If My.Computer.FileSystem.DirectoryExists(sDatasetSchemaPath & "\" & sFolderName) Then
                If My.Computer.FileSystem.FileExists(sFUllPath) Then
                    oResponse.DocumentTemplateStatus = True
                Else
                    oResponse.DocumentTemplateStatus = False
                End If
            End If
        End If


        Return oResponse
    End Function


#End Region

    ''' <summary>
    ''' ValidatePolicyAlreadyMadeLive
    ''' </summary>
    ''' <param name="Con"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="bIsPolicyAlreadyMadeLive"></param>
    ''' <remarks></remarks>
    Private Sub ValidatePolicyAlreadyMadeLive(ByVal Con As SiriusConnection,
                             ByVal nInsuranceFileCnt As Integer,
                             ByRef bIsPolicyAlreadyMadeLive As Boolean)

        Dim dsIsPolicyMadeLive As DataSet = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Validate_IsPolicyLive")
            cmd.AddInParameter("@insuranceFileCnt", SqlDbType.Int).Value = Cast.ToInt32(nInsuranceFileCnt)

            dsIsPolicyMadeLive = Con.ExecuteDataSet(cmd, "IsPolicyMadeLive")
            If dsIsPolicyMadeLive.Tables(0).Rows.Count > 0 Then
                bIsPolicyAlreadyMadeLive = True
            End If

        End Using

    End Sub

    ''' <summary>
    ''' ValidateDuplicateRenewal
    ''' </summary>
    ''' <param name="Con"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="bIsDuplicateRenewal"></param>
    ''' <remarks></remarks>
    Private Sub ValidateDuplicateRenewal(ByVal Con As SiriusConnection,
                                    ByVal nInsuranceFileCnt As Integer,
                                    ByRef bIsDuplicateRenewal As Boolean)
        Dim dtResultDataTable As DataTable
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Select_Duplicate_Renewal")
            cmd.AddInParameter("@nInsurance_file_cnt", SqlDbType.Int).Value = Cast.ToInt32(nInsuranceFileCnt)
            dtResultDataTable = Con.ExecuteDataTable(cmd)
        End Using
        If dtResultDataTable IsNot Nothing AndAlso dtResultDataTable.Rows.Count > 0 Then
            bIsDuplicateRenewal = True
        End If

    End Sub

#Region "CancelMTAQuote"
    '''<summary>
    '''This function is used to cancel the MTA Quote and Communicate with the backoffice Component
    '''</summary>
    '''<param name="r_oCancelMTAQuoteRequest">BaseCancelMTAQuoteRequestType</param>
    '''<remarks></remarks>
    Public Function CancelMTAQuote(ByVal r_oCancelMTAQuoteRequest As BaseCancelMTAQuoteRequestType) As _
    BaseCancelMTAQuoteResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
        _SiriusUser.Username, _SiriusUser.SourceID,
        _SiriusUser.LanguageID, SiriusUserDefaults.AppName)
            Dim oResponse As New BaseImplementationTypes.BaseCancelMTAQuoteResponseType

            Try
                oResponse = CancelMTAQuoteUnderwriting(con, r_oCancelMTAQuoteRequest)

            Finally
                con.Dispose()
            End Try
            Return oResponse
        End Using
    End Function

    Private Function CancelMTAQuoteUnderwriting(ByRef con As SiriusConnection,
                                                ByVal r_oCancelMTAQuoteRequest As BaseCancelMTAQuoteRequestType) As BaseCancelMTAQuoteResponseType

        Dim oResponse As New BaseCancelMTAQuoteResponseType
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim nInsuranceFileKey As Integer = 0
        Dim nInsuranceFolderKey As Integer = 0
        Dim nPartyKey As Integer = 0
        Dim oErrorCollection As New SAMErrorCollection
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nTypeOfPackage As enumTypeOfPackage

        If r_oCancelMTAQuoteRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CancelMTAQuoteRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.CancelMTAQuoteResponseType
        End If

        If r_oCancelMTAQuoteRequest.InsuranceFileKey <> 0 Then
            nInsuranceFileKey = GetAndValidateSpecifiedTableCode(con, "Insurance_file", "insurance_file_cnt", "insurance_file_cnt", r_oCancelMTAQuoteRequest.InsuranceFileKey.ToString, oErrorCollection, "InsuranceFileKey")
            oErrorCollection.CheckForErrors()
        End If

        'Get other insurance file data
        nInsuranceFolderKey = GetAndValidateSpecifiedTableCode(con, "Insurance_file", "insurance_folder_cnt", "insurance_file_cnt", r_oCancelMTAQuoteRequest.InsuranceFileKey.ToString, oErrorCollection, "InsuranceFolderKey")
        oErrorCollection.CheckForErrors()

        nPartyKey = GetAndValidateSpecifiedTableCode(con, "Insurance_file", "insured_cnt", "insurance_file_cnt", r_oCancelMTAQuoteRequest.InsuranceFileKey.ToString, oErrorCollection, "insured_cnt")

        oErrorCollection.CheckForErrors()

        Dim oInsuranceFile As bSIRInsuranceFile.Business = Nothing
        Try
            oInsuranceFile = New bSIRInsuranceFile.Business

            SAMFunc.InitialiseSBOObject(con, oInsuranceFile, _SiriusUser, "", "bSIRInsuranceFile.Business")

            nResult = oInsuranceFile.MTACancellation(v_lInsuranceFileCnt:=nInsuranceFileKey,
                            v_lInsuranceFolderCnt:=nInsuranceFolderKey,
                          v_lPartyCnt:=nPartyKey)

            If nResult <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRInsuranceFile.Business.MTACancellation", nResult)
            End If

            'Delete An Exclusive Locks
            DeleteExlusiveLock(con,
                               oCoreBusiness,
                               CoreBusiness.LockName.InsuranceFolderCnt,
                               nInsuranceFolderKey,
                               r_oCancelMTAQuoteRequest.BranchCode)
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Failed to call the bSIRInsuranceFile.Business", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), "CancelMTAQuoteUnderwriting", "Call Back Office Component", True)

            nResult = PMEReturnCode.PMFalse

        Finally
            'oResponse = Nothing
            oCoreBusiness = Nothing
            oErrorCollection = Nothing
        End Try

        Return oResponse
    End Function
#End Region

#Region "GetInstalmentSettlementAmount"
    Public Function GetInstalmentSettlementAmount(ByVal oUpdateMarketplacePolicyStatusRequest As SAMForInsuranceV2ImplementationTypes.BaseGetInstalmentSettlementAmountRequestType) As SAMForInsuranceV2ImplementationTypes.GetInstalmentSettlementAmountResponseType
        Dim ds As New DataSet
        Dim nOriginalInsuranceFileCnt As Integer
        Dim nPfpremFinanceCnt As Integer
        Dim nPfpremFinanceVersion As Integer
        Dim dSettleAmount As Decimal
        Dim dRefundFee As Decimal
        Dim dRefundTax As Decimal
        Dim dtNextInstalmentDate As DateTime
        Dim dtNextInstalmentDatePlus1 As DateTime
        Dim dtLastInstalmentDate As DateTime
        Dim dtLastPaidInstalmentDate As DateTime
        Dim oResponse As New SAMForInsuranceV2ImplementationTypes.GetInstalmentSettlementAmountResponseType

        Using con As SiriusConnection = New SiriusConnectionPMDAO(_SiriusUser.Username,
                                                               _SiriusUser.SourceID,
                                                               _SiriusUser.LanguageID,
                                                               SiriusUserDefaults.AppName)
            GetOriginalInsuranceFileDetails(con, oUpdateMarketplacePolicyStatusRequest.nInsuranceFileKey, nOriginalInsuranceFileCnt,
                                                nPfpremFinanceCnt, nPfpremFinanceVersion)
        End Using

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PFPremiumFinance_settlement")
            cmd.AddInParameter("@pfprem_finance_cnt", SqlDbType.Int).Value = nPfpremFinanceCnt
            cmd.AddInParameter("@pfprem_finance_version", SqlDbType.Int).Value = nPfpremFinanceVersion
            cmd.AddInParameter("@SettleAmount", SqlDbType.Decimal).Direction = ParameterDirection.Output
            cmd.AddInParameter("@RefundFee", SqlDbType.Decimal).Direction = ParameterDirection.Output
            cmd.AddInParameter("@RefundTax", SqlDbType.Decimal).Direction = ParameterDirection.Output
            cmd.AddInParameter("@NextInstalmentDate", SqlDbType.DateTime).Direction = ParameterDirection.Output
            cmd.AddInParameter("@NextInstalmentDatePlus1", SqlDbType.DateTime).Direction = ParameterDirection.Output
            cmd.AddInParameter("@LastInstalmentDate", SqlDbType.DateTime).Direction = ParameterDirection.Output
            cmd.AddInParameter("@LastPaidInstalmentDate", SqlDbType.DateTime).Direction = ParameterDirection.Output
            Using con As SiriusConnection = New SiriusConnectionPMDAO(_SiriusUser.Username,
                                                               _SiriusUser.SourceID,
                                                               _SiriusUser.LanguageID,
                                                               SiriusUserDefaults.AppName)

                con.ExecuteNonQuery(cmd)
            End Using

            dSettleAmount = Cast.ToDecimal(cmd.Parameters("@SettleAmount"), 0)
            dRefundFee = Cast.ToDecimal(cmd.Parameters("@RefundFee"), 0)
            dRefundTax = Cast.ToDecimal(cmd.Parameters("@RefundTax"), 0)
            dtNextInstalmentDate = Cast.ToDateTime(cmd.Parameters("@NextInstalmentDate"), DateTime.MinValue)
            dtNextInstalmentDatePlus1 = Cast.ToDateTime(cmd.Parameters("@NextInstalmentDatePlus1"), DateTime.MinValue)
            dtLastInstalmentDate = Cast.ToDateTime(cmd.Parameters("@LastInstalmentDate"), DateTime.MinValue)
            dtLastPaidInstalmentDate = Cast.ToDateTime(cmd.Parameters("@LastPaidInstalmentDate"), DateTime.MinValue)
        End Using

        oResponse.dInstalmentSettlementAmount = dSettleAmount
        Return oResponse
    End Function
#End Region


#Region "Public-Method Policy Associates"
#Region "Get Policy Associates"
    ''' <summary>
    '''This method is used to call the Base implementation function where  base logic is implemeted
    '''</summary>
    '''<param name="oGetPolicyAssociatesRequest" type="BaseGetPolicyAssociatesRequestType"></param>
    '''<remarks></remarks>
    Public Overloads Function GetPolicyAssociates(ByVal oGetPolicyAssociatesRequest As BaseGetPolicyAssociatesRequestType) As BaseGetPolicyAssociatesResponseType

        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oResponse As BaseGetPolicyAssociatesResponseType
            oResponse = GetPolicyAssociates(con, oGetPolicyAssociatesRequest)
            Return oResponse
        End Using

    End Function

    ''' <summary>
    '''In this Method  base logic is  imnplemented to execute the functionality.
    '''</summary>
    '''<param name="con" type="SiriusConnection"></param>
    '''<param name="oGetPolicyAssociatesRequest" type="BaseGetPolicyAssociatesRequestType"></param>
    '''<remarks></remarks>
    Private Overloads Function GetPolicyAssociates(ByVal conAssociates As SiriusConnection,
                                                  ByVal oGetPolicyAssociatesRequest As BaseGetPolicyAssociatesRequestType) As BaseGetPolicyAssociatesResponseType
        Dim dtPolicyAssociates As DataTable = Nothing
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oGetPolicyAssociatesResponse As BaseGetPolicyAssociatesResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection

        Try
            If oGetPolicyAssociatesRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPolicyAssociatesRequestType) Then
                nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
                oGetPolicyAssociatesResponse = New SAMForInsuranceV2ImplementationTypes.GetPolicyAssociatesResponseType
            End If

            'Mandatory validations
            oGetPolicyAssociatesRequest.Validate(CObj(oSAMErrorCollection))
            oSAMErrorCollection.CheckForErrors()

            Dim oGetPolicyAssociatesDetails() As BaseGetPolicyAssociatesResponseTypeAssociatesRow
            Dim oCoreBusiness As New CoreBusiness

            ' retrieve the party bank details from the database
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_get_insurance_file_Associates")
                cmd.AddInParameter("@InsuranceFileKey", SqlDbType.Int).Value = oGetPolicyAssociatesRequest.InsuranceFileKey
                dtPolicyAssociates = conAssociates.ExecuteDataTable(cmd)
            End Using

            If Not dtPolicyAssociates Is Nothing AndAlso dtPolicyAssociates.Rows.Count > 0 Then
                ReDim oGetPolicyAssociatesDetails(dtPolicyAssociates.Rows.Count - 1)
                Dim iArrayIndex As Integer = 0
                Dim oGetPolicyAssociatesResponseItem As BaseGetPolicyAssociatesResponseTypeAssociatesRow
                Dim oPolicyAssociatesTypeItem As BasePolicyAssociatesType
                Dim oBaseAddressTypeItem As BaseAddressType

                For Each drPolicyAssociates As DataRow In dtPolicyAssociates.Rows
                    oGetPolicyAssociatesResponseItem = New BaseGetPolicyAssociatesResponseTypeAssociatesRow

                    oPolicyAssociatesTypeItem = New BasePolicyAssociatesType
                    With oPolicyAssociatesTypeItem
                        .RowKey = iArrayIndex
                        .InsuranceFileAssociatesKey = Cast.ToInt32(drPolicyAssociates.Item("Insurance_file_associates_cnt"), 0)
                        .InsuranceFileKey = Cast.ToInt32(drPolicyAssociates.Item("Insurance_file_cnt"), 0)
                        .InsuranceFolderCnt = Cast.ToInt32(drPolicyAssociates.Item("insurance_folder_cnt"), 0)
                        .PartyKey = Cast.ToInt32(drPolicyAssociates.Item("Party_cnt"), 0)
                        .AssociationTypeKey = Cast.ToInt32(drPolicyAssociates.Item("Association_type_id"), 0)
                        .IsDeletedSpecified = True
                        .IsDeleted = CType(drPolicyAssociates.Item("Is_Deleted"), Boolean)
                        .DateRemovedSpecified = True
                        .DateRemoved = Cast.ToDateTime(drPolicyAssociates.Item("Date_Removed"), Nothing)
                        .DateAttachedSpecified = True
                        .DateAttached = Cast.ToDateTime(drPolicyAssociates.Item("Date_Attached"), Nothing)
                        .AssociationDetail = Cast.ToString(drPolicyAssociates.Item("Association_Detail"))
                        .IsAddUnConfirmed = Cast.ToBoolean(drPolicyAssociates.Item("is_AddUnConfirmed"), False)
                        .IsDelUnConfirmed = Cast.ToBoolean(drPolicyAssociates.Item("is_DelUnConfirmed"), False)
                    End With
                    oGetPolicyAssociatesResponseItem.Associates = oPolicyAssociatesTypeItem
                    oPolicyAssociatesTypeItem = Nothing 'Free memory for next instance

                    oBaseAddressTypeItem = New BaseAddressType
                    With oBaseAddressTypeItem
                        .AddressLine1 = Cast.ToString(drPolicyAssociates.Item("address1"), "")
                        .AddressLine2 = Cast.ToString(drPolicyAssociates.Item("address2"), "")
                        .AddressLine3 = Cast.ToString(drPolicyAssociates.Item("address3"), "")
                        .AddressLine4 = Cast.ToString(drPolicyAssociates.Item("address4"), "")
                        .AddressTypeCode = SAMFunc.GetAddressTypeCode(Cast.ToString(drPolicyAssociates.Item("Address_Usage_Type_Code"), String.Empty).Trim)
                        .CountryCode = Cast.ToString(drPolicyAssociates.Item("Country_Code"), "")
                        .CountryId = Cast.ToInt16(drPolicyAssociates.Item("country_id"), 0)
                        .PostCode = Cast.ToString(drPolicyAssociates.Item("postal_code"), "")
                    End With

                    oGetPolicyAssociatesResponseItem.Address = oBaseAddressTypeItem
                    oGetPolicyAssociatesResponseItem.PartyCode = Cast.ToString(drPolicyAssociates.Item("PartyShortName"), "")
                    Select Case UCase(Trim(CStr(drPolicyAssociates.Item("PartyTypeCode"))))
                        Case "PC"
                            oGetPolicyAssociatesResponseItem.PartyType = PartyTypeType.PC
                        Case "CC"
                            oGetPolicyAssociatesResponseItem.PartyType = PartyTypeType.CC
                        Case Else
                            oGetPolicyAssociatesResponseItem.PartyType = Nothing
                    End Select

                    oGetPolicyAssociatesResponseItem.PartyName = Cast.ToString(drPolicyAssociates.Item("resolved_name"), "")

                    'Put the associates details into the main collection
                    oGetPolicyAssociatesDetails(iArrayIndex) = oGetPolicyAssociatesResponseItem
                    oGetPolicyAssociatesResponseItem = Nothing
                    iArrayIndex += 1
                Next
                oGetPolicyAssociatesResponse.AssociatesRow = oGetPolicyAssociatesDetails
            End If

        Catch ex As Exception
            Throw
        Finally
            conAssociates.Dispose()
        End Try

        Return oGetPolicyAssociatesResponse

    End Function

#End Region

#Region "Public Method Update Policy Associates"
    ''' <summary>
    '''This method is used to call the Base implementation function where  core logic is implemeted
    '''</summary>
    '''<param name="oGetPolicyAssociatesRequest" type="BaseGetPolicyAssociatesRequestType"></param>
    '''<remarks></remarks>
    Public Overloads Function UpdatePolicyAssociates(ByVal oUpdatePolicyAssociatesRequest As BaseUpdatePolicyAssociatesRequestType) As BaseUpdatePolicyAssociatesResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oResponse As BaseUpdatePolicyAssociatesResponseType
            oResponse = UpdatePolicyAssociatesItem(con, oUpdatePolicyAssociatesRequest)
            Return oResponse
        End Using
    End Function
#End Region
#Region "Private Method Update Policy Associates"
    ''' <summary>
    '''In this Method  base logic is  imnplemented to execute the functionality.
    '''</summary>
    '''<param name="con" type="SiriusConnection"></param>
    '''<param name="oGetPolicyAssociatesRequest" type="BaseGetPolicyAssociatesRequestType"></param>
    '''<remarks></remarks>
    Private Overloads Function UpdatePolicyAssociatesItem(ByVal con As SiriusConnection,
                                                     ByVal oUpdatePolicyAssociatesRequest As BaseUpdatePolicyAssociatesRequestType
                                                   ) As BaseUpdatePolicyAssociatesResponseType

        Dim oUpdatePolicyAssociatesResponse As New BaseUpdatePolicyAssociatesResponseType
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage

        Try
            If oUpdatePolicyAssociatesRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdatePolicyAssociatesRequestType) Then
                nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
                oUpdatePolicyAssociatesResponse = New SAMForInsuranceV2ImplementationTypes.UpdatePolicyAssociatesResponseType
            End If

            'Mandatory validations
            oUpdatePolicyAssociatesRequest.Validate(CObj(oSAMErrorCollection))
            oSAMErrorCollection.CheckForErrors()

            'Look up validation
            Dim iSourceId As Integer
            iSourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oUpdatePolicyAssociatesRequest.BranchCode, "BranchCode", oSAMErrorCollection)
            oSAMErrorCollection.CheckForErrors()

            If oUpdatePolicyAssociatesRequest Is Nothing Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                            SAMInvalidData.MandatoryInputMissing.ToString,
                                                                            "UpdatePolicy Associates not valid")
                oSAMErrorCollection.CheckForErrors()
            End If
            'Check time stamp should be passed else raise error
            If oUpdatePolicyAssociatesRequest.TimeStamp Is Nothing Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                              SAMInvalidData.MandatoryInputMissing.ToString,
                                                                              "TimeStamp")
                oSAMErrorCollection.CheckForErrors()
            End If

            If Not oUpdatePolicyAssociatesRequest.Associates Is Nothing AndAlso Val(oUpdatePolicyAssociatesRequest.Associates(0).InsuranceFileKey.ToString) = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                       SAMInvalidData.MandatoryInputMissing.ToString,
                                                                       "InsuranceFileKey")
                oSAMErrorCollection.CheckForErrors()
            End If


            ' Suggestion
            If Not oUpdatePolicyAssociatesRequest.Associates Is Nothing Then
                If oCoreBusiness.CheckInsuranceFile(oUpdatePolicyAssociatesRequest.Associates(0).InsuranceFileKey,
                                                    oUpdatePolicyAssociatesRequest.Associates(0).InsuranceFolderCnt) = False Then
                    oSAMErrorCollection.AddBusinessRule(SAMConstants.
                            SAMInvalidData.InvalidInsuranceFileType,
                            "Invalid Insurance File Key Provided.")
                    oSAMErrorCollection.CheckForErrors()
                End If
            End If

            Dim o_insuranceFileTypeId As Integer = 0
            Dim o_insuranceFileTypeCode As String = String.Empty

            If Not oUpdatePolicyAssociatesRequest.Associates Is Nothing Then
                GetInsuranceFileType(con, oUpdatePolicyAssociatesRequest.Associates(0).InsuranceFileKey,
                                     o_insuranceFileTypeId, o_insuranceFileTypeCode)
            End If

            'Validate policy vesrion should not be live
            If o_insuranceFileTypeCode = InsuranceFileType.LivePolicy AndAlso oUpdatePolicyAssociatesRequest.SkipPolicyTypeCheck = False Then
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CanNotUpdateLivePolicyDetails,
                    SAMBusinessErrors.CanNotUpdateLivePolicyDetails.ToString,
                    "Can not update Policy Associates  for a live poilcy")
                oSAMErrorCollection.CheckForErrors()
            End If

            'Lock the Current Insurance_FolderCnt
            Dim oAnyError As STSErrorType
            oAnyError = oCoreBusiness.CheckTSAndLock(
                BranchCode:=oUpdatePolicyAssociatesRequest.BranchCode,
                Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
                LockValue:=oUpdatePolicyAssociatesRequest.Associates(0).InsuranceFolderCnt,
                TStamp:=oUpdatePolicyAssociatesRequest.TimeStamp)

            ' Check for Errors
            If oAnyError Is Nothing = False Then
                ' Either the timestamp didn't match or the record couldn't be locked for some reason, return the error.
                oUpdatePolicyAssociatesResponse.STSError = oAnyError
                Return oUpdatePolicyAssociatesResponse
            End If

            Dim oUpdatePolicyAssociatesItem As New BasePolicyAssociatesType
            con.BeginTransaction()

            If Not oUpdatePolicyAssociatesRequest.Associates Is Nothing Then
                For Each oUpdatePolicyAssociatesItem In oUpdatePolicyAssociatesRequest.Associates

                    If Not oUpdatePolicyAssociatesItem Is Nothing AndAlso oUpdatePolicyAssociatesItem.AssociationTypeKey > 0 Then

                        If oUpdatePolicyAssociatesItem.PartyKey = 0 Then
                            ' PartyKey not set
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                                                  "Party Key")
                            oSAMErrorCollection.CheckForErrors()
                        End If

                        If oUpdatePolicyAssociatesItem.AssociationTypeKey = 0 Then
                            ' AssociationTypeKey not set
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                                                   "AssociationTypeKey")
                            oSAMErrorCollection.CheckForErrors()
                        End If

                        If oUpdatePolicyAssociatesItem.ActionType = RowAction.DeleteRow AndAlso CInt(oUpdatePolicyAssociatesItem.IsDeleted) = 0 Then
                            ' AssociationTypeKey not set
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                                                   "IsDeleted is Passed wrong value When Action Type is DeleteRow ")
                            oSAMErrorCollection.CheckForErrors()
                        End If

                        If oUpdatePolicyAssociatesItem.ActionType <> RowAction.DeleteRow AndAlso oUpdatePolicyAssociatesItem.DateAttached = Nothing Then
                            ' AssociationTypeKey not set
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                                                   "Date Attched is Missing")
                            oSAMErrorCollection.CheckForErrors()
                        End If

                        'Call function to perform action task as per row action
                        If UpdatePolicyAssociatesItem(con, oUpdatePolicyAssociatesItem) = PMReturnCode.PMFalse Then
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CollectionDateError,
                                 SAMBusinessErrors.CollectionDateError.ToString,
                                "Failed to call Method UpdatePolicyAssociatesItem ")
                            oSAMErrorCollection.CheckForErrors()
                        End If

                    End If

                Next
            End If
            If Not oUpdatePolicyAssociatesItem Is Nothing Then
                oCoreBusiness.UnlockAndGetSAMTS(Con:=con,
                    BranchCode:=oUpdatePolicyAssociatesRequest.BranchCode,
                    Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
                    LockValue:=oUpdatePolicyAssociatesRequest.Associates(0).InsuranceFolderCnt,
                    TStamp:=oUpdatePolicyAssociatesRequest.TimeStamp)

                ' Check for Errors
                If oAnyError Is Nothing = False Then
                    ' Either the timestamp didn't match or the record couldn't be locked for some reason, return the error.
                    oUpdatePolicyAssociatesResponse.STSError = oAnyError
                    Return oUpdatePolicyAssociatesResponse
                End If
            End If
            con.CommitTransaction()
            oUpdatePolicyAssociatesResponse.TimeStamp = oUpdatePolicyAssociatesRequest.TimeStamp

        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        Finally
            con.Dispose()
            oCoreBusiness = Nothing
        End Try

        Return oUpdatePolicyAssociatesResponse
    End Function
    ''' <summary>
    '''This method is used to Perform Update Operation based on request collection
    '''</summary>
    '''<param name="con" type="SiriusConnection"></param>
    '''<param name="lInsuranceFileKey" type="Integer"></param>
    '''<remarks></remarks>
    Private Overloads Function UpdatePolicyAssociatesItem(ByVal con As SiriusConnection, ByVal oUpdatePolicyAssociatesItem As BasePolicyAssociatesType) As Integer
        Dim sProcname As String = String.Empty
        Dim nResult As Integer = PMConst.PMTrue

        Try
            Select Case oUpdatePolicyAssociatesItem.ActionType
                Case RowAction.AddRow
                    sProcname = "Spu_insurance_file_associates_add"
                Case RowAction.DeleteRow, RowAction.EditRow
                    sProcname = "Spu_insurance_file_associates_upd"
            End Select

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure(sProcname)
                cmd.AddInParameter("@Insurance_file_associates_cnt", SqlDbType.Int).Value = oUpdatePolicyAssociatesItem.InsuranceFileAssociatesKey
                cmd.AddInParameter("@Association_type_id", SqlDbType.Int).Value = oUpdatePolicyAssociatesItem.AssociationTypeKey
                cmd.AddInParameter("@Insurance_file_cnt", SqlDbType.Int).Value = oUpdatePolicyAssociatesItem.InsuranceFileKey
                cmd.AddInParameter("@Party_cnt", SqlDbType.Int).Value = oUpdatePolicyAssociatesItem.PartyKey

                If oUpdatePolicyAssociatesItem.DateAttachedSpecified Then
                    If oUpdatePolicyAssociatesItem.DateAttached <> Date.MinValue Then
                        cmd.AddInParameter("@Date_attached", SqlDbType.DateTime).Value = oUpdatePolicyAssociatesItem.DateAttached
                    Else
                        cmd.AddInParameter("@Date_attached", SqlDbType.DateTime).Value = DBNull.Value
                    End If
                Else
                    cmd.AddInParameter("@Date_attached", SqlDbType.DateTime).Value = DBNull.Value
                End If

                If oUpdatePolicyAssociatesItem.DateRemovedSpecified AndAlso oUpdatePolicyAssociatesItem.ActionType = RowAction.DeleteRow Then
                    If oUpdatePolicyAssociatesItem.DateAttached <> Date.MinValue Then

                        cmd.AddInParameter("@Date_removed", SqlDbType.DateTime).Value = oUpdatePolicyAssociatesItem.DateRemoved
                    Else
                        cmd.AddInParameter("@Date_removed", SqlDbType.DateTime).Value = DBNull.Value
                    End If
                Else
                    cmd.AddInParameter("@Date_removed", SqlDbType.DateTime).Value = DBNull.Value
                End If

                cmd.AddInParameter("@Is_Deleted", SqlDbType.TinyInt).Value = Cast.ToByte(oUpdatePolicyAssociatesItem.IsDeleted, 0)
                cmd.AddInParameter("@Is_DelUnConfirmed", SqlDbType.TinyInt).Value = Cast.ToByte(oUpdatePolicyAssociatesItem.IsDelUnConfirmed, 0)
                cmd.AddInParameter("@Is_AddUnConfirmed", SqlDbType.TinyInt).Value = Cast.ToByte(oUpdatePolicyAssociatesItem.IsAddUnConfirmed, 0)
                cmd.AddInParameter("@Association_detail", SqlDbType.VarChar, 255).Value = Cast.ToString(oUpdatePolicyAssociatesItem.AssociationDetail, "")
                con.ExecuteNonQuery(cmd)

            End Using
            nResult = PMEReturnCode.PMTrue

        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            Throw
        End Try

        Return nResult
    End Function
#End Region

#End Region

#Region "GetIntermediaryAgentAccountID"
    ''' <summary>
    ''' Get intermediary_agent_account_id on basis of InsuranceFolderCnt and InsuranceFileCnt
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="InsuranceFolderCnt"></param>
    ''' <param name="InsuranceFileCnt"></param>
    ''' <remarks></remarks>
    Private Function GetIntermediaryAgentAccountID(ByVal con As SiriusConnection, ByVal InsuranceFolderCnt As Integer, ByVal InsuranceFileCnt As Integer) As Integer

        Dim intermediary_agent_account_id As Integer
        Dim dt As DataTable = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Policy_Intermediary_Agent_Account")
            cmd.AddInParameter("@InsuranceFolderCnt", SqlDbType.Int).Value = InsuranceFolderCnt
            cmd.AddInParameter("@InsuranceFileCnt", SqlDbType.Int).Value = InsuranceFileCnt
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim dr As DataRow = dt.Rows(0)

            intermediary_agent_account_id = Cast.ToInt32(dr.Item("intermediary_agent_account_id"), 0)

        End If
        Return intermediary_agent_account_id

    End Function
#End Region
End Class
